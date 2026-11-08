using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Argumentum.AssetConverter.Tests;
using Argumentum.AssetConverter.Tests.Structure;
using ImageMagick;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests.Structure
{
	/// <summary>
	/// #1460 <b>phase C</b>, organe 3 — la transposition de l'organe 2 aux <b>vertus</b> :
	/// chaque carte du paquet Virtues porte la couleur de sa famille, mesurée sur les pixels
	/// des PNG de récolte. Même moteur (<see cref="FamilyColorIntegrity"/>, source unique
	/// linkée), trois ancres identiques (CSV × gabarit × pixel) — ce qui change est mesuré,
	/// pas supposé :
	///
	/// <para><b>1. Les colonnes ne s'appellent pas pareil.</b> Le CSV Virtues
	/// (<c>Argumentum Virtues - Taxonomy.csv</c>, 223 nœuds) marque ses cartes par la colonne
	/// <c>card</c> (131 lignes non vides, mesuré le 07/11) et porte la classe dans
	/// <c>family_fr_camelcase</c> — là où le CSV Fallacies dit <c>carte</c> et
	/// <c>Famille_camelCase</c>. Les DEUX fichiers sont à virgules (mesuré aux octets).</para>
	///
	/// <para><b>2. Le gabarit Virtues écrit ses familles en listes de sélecteurs</b> — 7 blocs
	/// sur 8 (renommage additif, mesuré le 07/11) : <c>card.ancien,\ncard.nouveau { … }</c>,
	/// chacun précédé d'un commentaire CSS. Le moteur assigne la couleur à chaque nom de la
	/// liste et la garde de séparation court sur les couleurs DISTINCTES (les 7 paires d'alias
	/// mesurées à 0,0 sont UNE famille).</para>
	///
	/// <para><b>Calibrage mesuré</b> (07/11, arbre Release local, 8 langues × 113 cartes
	/// présentes) : dominante <b>exacte</b> (distance 0,0) partout ; part dominante minimale
	/// <b>0,4635</b> (pt) ; familles distinctes les plus proches à <b>114,3</b> l'une de l'autre
	/// (<c>inférenceMaîtrisée</c> ↔ <c>honnêtetéIntellectuelle</c>). Les bornes de l'organe 2
	/// (30 / 60 / 5 %) tiennent avec les réserves 3,8× / 1,9× / 9,3×.</para>
	///
	/// <para>⚠️ <b>État mesuré de l'arbre au jour de la livraison</b> : les PNG Virtues de
	/// l'arbre Release datent du <b>25/06</b> alors que le CSV a bougé jusqu'au 31/10 — le côté
	/// Virtues de l'arbre est PÉRIMÉ de 18 cartes (113 présentes sur 131 attendues, uniformément
	/// sur les 8 langues). Armé sur cet arbre, l'organe ROUGIT en nommant les 18 chemins par
	/// langue : c'est l'état 3 de la charpente (« un arbre qui existe mais ne contient pas les
	/// objets de la mesure est un échec, pas un skip ») — le même signal que la garde d'arbre.
	/// Il verdira le jour d'une régén couvrant Virtues.</para>
	///
	/// <para><b>Armement.</b> <c>ARGUMENTUM_PDF_BUNDLE_ROOT</c> — la variable des autres organes.
	/// Absente <b>et</b> arbre par défaut absent : <b>NON MESURABLE</b>, rendu tel quel (cas
	/// nominal en CI). Présente mais inutilisable : <b>échec</b>.</para>
	/// </summary>
	public class VirtuesFamilyColorGateTests
	{
		private const string BundleRootEnvVar = "ARGUMENTUM_PDF_BUNDLE_ROOT";

		private static readonly string TargetRoot = Path.Combine(
			TestRepoRoot.Find(), "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Release",
			"net9.0-windows", "Target");

		private readonly ITestOutputHelper _output;
		public VirtuesFamilyColorGateTests(ITestOutputHelper output) => _output = output;

		[Fact]
		public void Every_Virtue_Card_Carries_Its_Family_Color()
		{
			var envRoot = Environment.GetEnvironmentVariable(BundleRootEnvVar);
			var root = string.IsNullOrWhiteSpace(envRoot) ? TargetRoot : envRoot;

			if (!Directory.Exists(root))
			{
				// Cas nominal en CI : pas d'arbre, rien à mesurer.
				_output.WriteLine($"{BundleRootEnvVar} absent et arbre par défaut absent ({root}) — NON MESURABLE.");
				return;
			}

			// ── L'attendu : CSV (famille par carte) × gabarit (couleur par famille) ──
			var repoRoot = TestRepoRoot.Find();
			var csvPath = Path.Combine(repoRoot, "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");
			var templatePath = Path.Combine(repoRoot, "Cards", "Fallacies", "Argumentum_Virtues_Face_fr.json");

			var reader = new HarvestCardIdsCsv(csvPath);
			var paths = reader.LoadColumn("path");
			var cartes = reader.LoadColumn("card");
			var classes = reader.LoadColumn("family_fr_camelcase");
			Assert.True(paths.Count == cartes.Count && paths.Count == classes.Count,
				$"les trois colonnes lues n'ont pas le même nombre de lignes ({paths.Count}/{cartes.Count}/{classes.Count}) — " +
				$"une relecture non alignée ferait correspondre une carte à la famille de sa voisine");

			var cards = new List<FamilyColorIntegrity.CardFamily>();
			for (var i = 0; i < paths.Count; i++)
			{
				if (string.IsNullOrWhiteSpace(cartes[i])) continue; // nœud sans carte (223 nœuds, 131 cartes)
				cards.Add(new FamilyColorIntegrity.CardFamily(Prefix(paths[i]), classes[i]));
			}
			Assert.True(cards.Count > 0,
				$"aucune ligne avec colonne card non vide dans {csvPath} ({paths.Count} nœuds lus) — " +
				$"l'attendu est vide, la mesure n'aurait rien à confronter");
			_output.WriteLine($"{cards.Count} cartes attendues (colonne card non vide sur {paths.Count} nœuds)");

			string? css = null;
			Assert.True(File.Exists(templatePath), $"gabarit introuvable : {templatePath}");
			using (var doc = JsonDocument.Parse(File.ReadAllText(templatePath)))
			{
				css = doc.RootElement.TryGetProperty("css", out var cssElement) ? cssElement.GetString() : null;
			}

			var familyColors = FamilyColorIntegrity.ParseFamilyColors(css);
			Assert.True(familyColors.Count > 0,
				$"aucune couleur de famille parsée dans la clé css de {templatePath} — " +
				"la clé est-elle absente, vide, ou sans bloc card.<famille> portant --color-background ?");
			// Identité de BLOC (c.6714912295) : la garde court sur les blocs règle — les alias
			// d'une même liste sont UNE famille, deux blocs distincts de même couleur rougissent.
			var blocks = FamilyColorIntegrity.ParseFamilyBlocks(css);
			Assert.True(blocks.Count > 0,
				$"aucun bloc règle card.<famille> parsé dans {templatePath} alors que la vue aplatie en donne {familyColors.Count}");

			var distinctColors = familyColors.Values.Distinct().ToList();
			_output.WriteLine(
				$"{familyColors.Count} noms de famille parsés, {distinctColors.Count} couleurs distinctes " +
				"(les alias hérités du renommage additif partagent la couleur de leur famille)");

			// ── La mesure : langue par langue, toutes celles qui portent le dossier d'images ──
			var languages = Directory.GetDirectories(root)
				.Select(Path.GetFileName)
				.Where(lang => Directory.Exists(Path.Combine(root, lang!, "Images", "density-0", "Virtues")))
				.OrderBy(l => l, StringComparer.Ordinal)
				.Select(l => l!)
				.ToList();
			Assert.True(languages.Count > 0,
				$"sous « {root} », aucune langue ne porte Images/density-0/Virtues — " +
				"un arbre qui existe mais ne contient pas les objets de la mesure est un échec, pas un skip");

			var allFailures = new List<string>();

			foreach (var lang in languages)
			{
				var imageDir = Path.Combine(root, lang, "Images", "density-0", "Virtues");
				var files = Directory.GetFiles(imageDir, "*_face.png")
					.Select(Path.GetFileName)
					.Select(f => f!)
					.OrderBy(f => f, StringComparer.Ordinal)
					.ToList();

				var measured = new List<FamilyColorIntegrity.MeasuredDominant>();
				var joinFailures = new List<string>();
				FamilyColorIntegrity.MeasuredDominant? worstShare = null;
				var worstDistance = 0.0;

				foreach (var card in cards)
				{
					var hits = files.Where(f => f.StartsWith(card.Name, StringComparison.Ordinal)).ToList();
					if (hits.Count != 1)
					{
						joinFailures.Add($"{card.Name} → {hits.Count} fichier(s)");
						continue;
					}

					var dominant = Dominant(card.Name, Path.Combine(imageDir, hits[0]));
					if (dominant == null)
					{
						joinFailures.Add($"{card.Name} → aucune couleur non blanche mesurable");
						continue;
					}

					measured.Add(dominant);
					if (worstShare == null || dominant.Share < worstShare.Share) worstShare = dominant;
					if (familyColors.TryGetValue(card.FamilyClass, out var expectedForWorst))
					{
						var d = FamilyColorIntegrity.Distance(dominant.Color, expectedForWorst);
						if (d > worstDistance) worstDistance = d;
					}
				}

				if (joinFailures.Count > 0)
				{
					// Le diagnostic de péremption fait partie du message : un relecteur qui voit
					// rouge doit lire QUEL état de la charpente il regarde, pas deviner.
					allFailures.Add(
						$"{lang} : {files.Count} faces sur l'arbre pour {cards.Count} attendues du CSV — " +
						"arbre PÉRIMÉ côté Virtues (charpente §1 état 3 : la régén de l'arbre précède la " +
						"dernière écriture du corpus ; la fraîcheur relève de la garde d'arbre, la couleur " +
						"présente reste mesurée sur les cartes existantes). Jointure : " +
						string.Join(" | ", joinFailures.Take(5)) +
						(joinFailures.Count > 5 ? $" … (+{joinFailures.Count - 5})" : string.Empty));
					_output.WriteLine($"=== {lang} : {joinFailures.Count} jointure(s) manquante(s) sur {cards.Count} — " +
						$"les {measured.Count} cartes présentes restent mesurées ci-dessous");
				}

				_output.WriteLine(
					$"=== {lang} : {measured.Count} cartes mesurées · pire distance {FamilyColorIntegrity.F(worstDistance)} · " +
					$"part dominante min {(worstShare == null ? "n/a" : FamilyColorIntegrity.P(worstShare.Share))}");

				// Les cartes PRÉSENTES sont confrontées même sur un arbre périmé : la couleur des
				// 113 présentes est un fait, indépendant des 18 manquantes.
				allFailures.AddRange(
					FamilyColorIntegrity.Check(cards, familyColors, measured, blocks)
						.Where(f => !f.Contains("aucune mesure de couleur dominante")) // les manquantes ont déjà leur échec nommé
						.Select(f => $"{lang} : {f}"));
			}

			Assert.True(allFailures.Count == 0,
				"chaque carte du paquet Virtues doit porter la couleur de sa famille (colonne family_fr_camelcase × " +
				"--color-background du gabarit) — mesuré sur les pixels des PNG de récolte. Échecs : " +
				string.Join(" | ", allFailures));
		}

		/// <summary>Le préfixe de jointure : <c>argumentum_virtues_{path}..</c>. Même règle que
		/// l'organe 2 : le path CSV porte la hiérarchie en points et entre tel quel dans le nom
		/// de fichier ; le <c>Replace</c> n'absorbe qu'une future source à virgule décimale.
		/// Mesuré : 113/113 appariements uniques sur les cartes présentes, 8 langues.</summary>
		private static string Prefix(string csvPath) => $"argumentum_virtues_{csvPath.Replace(',', '.')}..";

		/// <summary>Couleur dominante non blanche, échantillonnage 1 point sur 16 — copie
		/// conforme de l'organe 2 : les deux organes doivent mesurer la même chose, une
		/// divergence de méthode entre paquets serait une divergence de verdict.</summary>
		private static FamilyColorIntegrity.MeasuredDominant? Dominant(string name, string pngPath)
		{
			using var image = new MagickImage(pngPath);
			var rgb = image.GetPixels().ToByteArray(PixelMapping.RGB);
			if (rgb == null || rgb.Length == 0) return null;

			var counts = new Dictionary<int, long>();
			long samples = 0;
			for (var y = 0; y < image.Height; y += 4)
			{
				var rowOffset = y * image.Width * 3;
				for (var x = 0; x < image.Width; x += 4)
				{
					var i = rowOffset + x * 3;
					if (i + 2 >= rgb.Length) break;
					samples++;
					var key = (rgb[i] << 16) | (rgb[i + 1] << 8) | rgb[i + 2];
					counts.TryGetValue(key, out var n);
					counts[key] = n + 1;
				}
			}
			if (samples == 0) return null;

			var bestKey = -1;
			long bestCount = 0;
			foreach (var pair in counts)
			{
				var r = (pair.Key >> 16) & 0xFF;
				var g = (pair.Key >> 8) & 0xFF;
				var b = pair.Key & 0xFF;
				if (Math.Min(r, Math.Min(g, b)) >= 240) continue; // blanc / quasi blanc
				if (pair.Value > bestCount)
				{
					bestCount = pair.Value;
					bestKey = pair.Key;
				}
			}
			if (bestKey < 0) return null;

			var color = new FamilyColorIntegrity.Rgb(
				(byte)((bestKey >> 16) & 0xFF), (byte)((bestKey >> 8) & 0xFF), (byte)(bestKey & 0xFF));
			return new FamilyColorIntegrity.MeasuredDominant(name, color, (double)bestCount / samples);
		}
	}
}
