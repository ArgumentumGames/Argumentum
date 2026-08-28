using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Organe de comptage tertiaire (#1187): épingle le nombre attendu de cartes par CardSet au niveau
	/// <b>CSV × predicate × config</b>, afin que toute dérive de compte soit détectée sans avoir à
	/// régénérer le pipeline. Les comptes mesurés manuellement (PowerShell <c>ConvertFrom-Json</c> sur
	/// <c>Target/&lt;lang&gt;/Harvest/*_harvest_*.json</c>) sont lents (&gt;30 MB par Scenarii) et
	/// fragiles (hashtable <c>Count</c> = vide). Cette suite lit directement les CSVs source, de sorte
	/// qu'un ajout/suppression de carte en amont fait échouer le contrat avant toute régénération.
	///
	/// Les comptes cibles reflètent la régénération 8-langues du 20/08 (base <c>d03fa9f3</c>) et la
	/// mesure source de ce tick (28/08). Aucun seed ni donnée embarquée : chaque Fact charge un CSV et
	/// compte. Lire le body complet (corps de mission) avant toute modification.
	/// </summary>
	public class CardSetExpectedCardCountContractTests
	{
		// ── Chemin source résolu via TestRepoRoot (auto-détection de la racine) ─────

		private static string RepoRoot => TestRepoRoot.Find();

		private static string RulesCsv => Path.Combine(RepoRoot, "Cards", "Rules", "Argumentum Rules - Cards.csv");
		private static string ScenariiCsv => Path.Combine(RepoRoot, "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");
		private static string FallaciesCsv => Path.Combine(RepoRoot, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		// Le Memo n'a PAS de CSV source autonome dans le dépôt : sa donnée est embarquée dans le
		// template JSON (Cards/Memo/Argumentum_Memo_Face_fr.json, rscount=200). Le Fact ci-dessous
		// tente le chemin attendu et, s'il n'existe pas, skip avec un message explicite (règle de la
		// mission : skip explicite, jamais un fail ni un skip silencieux).
		private static string MemoCsv => Path.Combine(RepoRoot, "Cards", "Memo", "Argumentum_Memo.csv");

		// ─────────────────────────────────────────────────────────────────────────
		// (1) Rules = 15 faces / langue. DataSet KnownDataSets.Rules, aucun filtre →
		//     toutes les lignes de « Argumentum Rules - Cards.csv » deviennent des faces.
		// ─────────────────────────────────────────────────────────────────────────
		[Fact]
		public void Rules_Has_15_Faces_Per_Language()
		{
			var csv = new HarvestCardIdsCsv(RulesCsv);
			var count = csv.LoadColumn("pk").Count;

			count.Should().Be(15,
				"Régén 8-langues 20/08 + mesure 28/08 : Rules = 15 faces/lang — « Argumentum Rules - Cards.csv », " +
				"DataSet KnownDataSets.Rules sans CsvFilterField (toutes les lignes deviennent des faces).");
		}

		// ─────────────────────────────────────────────────────────────────────────
		// (2) Scenarii = 167 faces / langue. DataSet KnownDataSets.Scenarii, aucun filtre
		//     sur le face (RowsetNb non défini → rscount=1 du template) → 167 lignes = 167 faces.
		// ─────────────────────────────────────────────────────────────────────────
		[Fact]
		public void Scenarii_Has_167_Faces_Per_Language()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var count = csv.LoadColumn("catégorie").Count;

			count.Should().Be(167,
				"Régén 8-langues 20/08 + mesure 28/08 : Scenarii = 167 faces/lang — « Argumentum Scenarii - Cards.csv », " +
				"DataSet KnownDataSets.Scenarii sans CsvFilterField sur le Face (rscount=1).");
		}

		// ─────────────────────────────────────────────────────────────────────────
		// (3) Scenarii = 7 dos distincts / langue. Le Back groupe par « catégorie »
		//     (RowsetNb=14, template {{rowset.[0].catégorie}}). 7 catégories → 7 dos.
		// ─────────────────────────────────────────────────────────────────────────
		[Fact]
		public void Scenarii_Has_7_Distinct_Backs_Per_Language()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var backs = csv.LoadColumnSet("catégorie");

			backs.Count.Should().Be(7,
				"Régén 8-langues 20/08 + mesure 28/08 : Scenarii = 7 dos distincts — 7 catégories distinctes " +
				"(relation intime, vie professionnelle, mythologie, vie personnelle, pop culture, histoire, politique), " +
				"le Back regroupant par « catégorie » (RowsetNb=14, {{rowset.[0].catégorie}}).");
		}

		// ─────────────────────────────────────────────────────────────────────────
		// (4) Memo = 1 carte. Pas de CSV source autonome → SKIP explicite (règle mission :
		//     si CSV absent au path attendu, skip avec message, ni fail ni skip silencieux).
		// ─────────────────────────────────────────────────────────────────────────
		// NOTE skip statique (xUnit 2.9.3 ne fournit pas de SkipException jettable — le skip dynamique
		// au runtime est une fonctionnalité de xUnit v3 / du package Xunit.SkippableFact). Comme il
		// n'existe réellement AUCUN CSV Memo autonome aujourd'hui, un skip explicite est le représentant
		// honnête. Si un jour Cards/Memo/Argumentum_Memo.csv est ajouté, retirer l'attribut Skip.
		// Le corps ci-dessous est l'assertion qui s'activera alors.
		[Fact(Skip = "Memo has no standalone source CSV in the repository: the memo card data is embedded in " +
			"Cards/Memo/Argumentum_Memo_Face_fr.json (rscount=200 groups the rows into a single card), " +
			"not in a .csv source file. The Memo=1 contract is therefore NOT pinned from a CSV source. " +
			"Add 'Cards/Memo/Argumentum_Memo.csv' and remove this Skip attribute to start asserting Memo=1.")]
		public void Memo_Has_1_Card()
		{
			var csv = new HarvestCardIdsCsv(MemoCsv);
			var count = csv.LoadColumn("pk").Count;

			count.Should().Be(1, "Memo = 1 carte (rscount=200 regroupe toutes les lignes en une seule carte).");
		}

		// ─────────────────────────────────────────────────────────────────────────
		// (5) Contrôle-inverse : Fallacies doit rester dans une fourchette saine. Le CardSet
		//     Fallacies filtre la taxonomie sur « carte » ∈ {1,2} (WebBasedGeneratorConfig.cs:121-126)
		//     → 176 faces mesurées ce tick. Ce Fact n'épingle PAS un compte exact (il le ferait avec un
		//     compte figé fragile) : il vérifie >100 ET ∈ [170,180] pour détecter toute dérive grossière.
		// ─────────────────────────────────────────────────────────────────────────
		[Fact]
		public void Fallacies_Card_Count_Is_In_Sane_Range_InverseControl()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			// Compter les LIGNES (pas les valeurs distinctes) : « carte » n'a que 2 valeurs distinctes
			// {1,2}, mais 176 lignes. LoadColumn conserve les doublons → le Count = nb de cartes.
			var count = csv.LoadColumn("carte", "carte", new[] { "1", "2" }).Count;

			count.Should().BeGreaterThan(100,
				$"Contrôle-inverse Fallacies : {count} face(s) — la taxonomie contient 1408 nœuds, mais seuls " +
				"les nœuds avec « carte » ∈ {1,2} deviennent des faces. Un compte ≤100 signale une dérive du filtre.");
			count.Should().BeInRange(170, 180,
				$"Contrôle-inverse Fallacies : {count} face(s) mesurée(s) via le filtre « carte » ∈ {{1,2}} " +
				"(=176). Range de soupçon 170-180 — hors de cette fourchette, un nœud a été ajouté/retiré ou le " +
				"filtre a dérivé et il faut régénérer + re-valider les PDFs.");
		}

		// ─────────────────────────────────────────────────────────────────────────
		// (6) L'étiquette d'encodage atteint réellement le message d'échec (#1213 / review T2.2).
		//     Le repli Latin-1 de DecodeCsvBytes est SILENCIEUX par conception : il ne lève pas, pour
		//     qu'un accent ne dégénère jamais en caractère de remplacement. Le prix est qu'une source
		//     mal encodée se lit « avec succès » et fausse les comptes sans rien dire. #1213 y répond
		//     en suffixant les InvalidOperationException par « [decoded as …] ».
		//
		//     Un diagnostic que rien n'exerce se perd au premier refactor : ce Fact est l'organe qui le
		//     tient. Il est gardé DANS LES DEUX SENS — il échoue si l'étiquette disparaît (régression),
		//     et il échoue aussi si elle est figée sur « Latin-1 » (étiquette décorative, qui mentirait
		//     sur une source propre et serait pire que pas d'étiquette du tout puisqu'elle inspire
		//     confiance).
		//
		//     Les deux fichiers sont écrits en octets bruts et ne diffèrent QUE par l'encodage de
		//     « été » : Latin-1 (0xE9) contre UTF-8 (0xC3 0xA9) précédé du BOM. L'encodage est donc la
		//     seule variable de l'expérience. Les octets sont choisis, pas devinés : 0xE9 seul est
		//     invalide en UTF-8 (il réclame deux octets de continuation, « t » = 0x74 n'en est pas un),
		//     donc le repli est réellement emprunté et non simplement supposé.
		// ─────────────────────────────────────────────────────────────────────────
		[Fact]
		public void Failure_Message_Names_The_Encoding_Actually_Used_Both_Directions()
		{
			var dir = Path.Combine(Path.GetTempPath(), "argumentum-1213-" + Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(dir);
			try
			{
				// (a) Source NON-UTF-8 — « été,1 » en Latin-1 ⇒ UTF-8 strict échoue, le repli est pris.
				var latin1 = Path.Combine(dir, "latin1.csv");
				File.WriteAllBytes(latin1, new byte[]
				{
					0x74, 0x69, 0x74, 0x72, 0x65, 0x2C, 0x76, 0x61, 0x6C, 0x0D, 0x0A, // titre,val + CRLF
					0xE9, 0x74, 0xE9, 0x2C, 0x31, 0x0D, 0x0A,                         // été,1 (Latin-1) + CRLF
				});

				var latin1Failure = Assert.Throws<InvalidOperationException>(
					() => new HarvestCardIdsCsv(latin1).LoadColumn("colonne-absente"));

				latin1Failure.Message.Should().Contain("Latin-1",
					"le repli Latin-1 est silencieux par conception — s'il n'est pas nommé dans l'échec, une " +
					"source mal encodée devient indiscernable d'une source propre, et c'est exactement le " +
					"reproche de la review T2.2 sur #1212. Message obtenu : " + latin1Failure.Message);
				latin1Failure.Message.Should().Contain("colonne-absente",
					"l'étiquette d'encodage s'AJOUTE au diagnostic existant, elle ne le remplace pas : la " +
					"colonne manquante doit rester nommée. Message obtenu : " + latin1Failure.Message);

				// (b) Contrôle inverse — même contenu, encodé proprement en UTF-8 avec BOM. Sans ce
				//     second sens, une étiquette figée en dur sur « Latin-1 » passerait (a) sans rien
				//     mesurer.
				var utf8 = Path.Combine(dir, "utf8bom.csv");
				File.WriteAllBytes(utf8, new byte[]
				{
					0xEF, 0xBB, 0xBF,                                                 // BOM UTF-8
					0x74, 0x69, 0x74, 0x72, 0x65, 0x2C, 0x76, 0x61, 0x6C, 0x0D, 0x0A, // titre,val + CRLF
					0xC3, 0xA9, 0x74, 0xC3, 0xA9, 0x2C, 0x31, 0x0D, 0x0A,             // été,1 (UTF-8) + CRLF
				});

				var utf8Failure = Assert.Throws<InvalidOperationException>(
					() => new HarvestCardIdsCsv(utf8).LoadColumn("colonne-absente"));

				utf8Failure.Message.Should().Contain("UTF-8 BOM",
					"une source à BOM UTF-8 doit être annoncée comme telle, sans quoi l'étiquette ne " +
					"distingue rien. Message obtenu : " + utf8Failure.Message);
				utf8Failure.Message.Should().NotContain("Latin-1",
					"une source propre étiquetée « Latin-1 » signalerait une étiquette décorative : elle " +
					"inspirerait une confiance qu'elle ne mesure pas. Message obtenu : " + utf8Failure.Message);
			}
			finally
			{
				try { Directory.Delete(dir, recursive: true); } catch (IOException) { }
			}
		}
	}
}
