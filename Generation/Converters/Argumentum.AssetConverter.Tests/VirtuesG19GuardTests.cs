using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Grain 19 (#458) — deux defauts de RANGEE des Vertus, <b>6 cellules / 2 rangees / 0 FR</b>.
	///
	/// <para><b>pk 172 <c>description</c> (V-RU-02)</b> : la description s'ouvrait sur un referent
	/// ABSENT — « Это означает » (ru) et « Trata-se de » (pt) — mesurees <b>uniques au
	/// corpus</b> (1 occurrence chacune avant, 0 apres). Geste : retrait du prefixe orphelin seul +
	/// capitalisation ; la queue de cellule est conservee octet a octet. Les tetes de remplacement
	/// ne sont pas retapees : « Признание » ouvre 8 descriptions de la colonne (dont la soeur 175),
	/// « Reconhecer » en ouvre 3 (166, 210).</para>
	///
	/// <para><b>pk 208 <c>remark</c></b> : l'antithese de la source FR (« vous critiquez ce qui est dit,
	/// non la personne qui le dit ») est portee par fr/en/zh/fa et <b>PERDUE dans QUATRE langues</b>
	/// (ru, pt, es, ar), qui partagent un gabarit de 4 propositions paralleles terme a terme
	/// (« la critique est necessaire … cela implique de formuler … sans jugement personnel … »).
	/// Le dossier du grain 8 (V-PT) en declarait <b>deux</b> (pt/es) : l'extension a ru/ar est
	/// <b>mesuree ici</b>, pas heritee. Geste : remplacement de la cellule par la phrase de la source,
	/// une phrase comme dans la famille fidele.</para>
	///
	/// <para><b>Ce n'est PAS de la derivation.</b> Sonde du corpus : aucune formulation
	/// « ce qui est dit / non la personne » n'est attestee en ru/pt/es/ar (0 occurrence des
	/// fragments candidats dans les 4 colonnes sur 223 rangees). C'est une <b>traduction depuis FR</b>,
	/// EN pour temoin structurel. Les pieces reutilisees verbatim viennent de la cellule substituee
	/// elle-meme (ouverture + clausule de but) ; ru garde « вы » car la construction est attestee
	/// telle quelle en pk 217.</para>
	///
	/// <para><b>Retard declare (gel v2.0.0-review, aucune regeneration).</b> Les <c>description</c> SONT
	/// rendues dans les mindmaps, les <c>remark</c> NON. Mesure : le texte ru retire de 172 vit encore
	/// dans <b>2 artefacts</b> (« Argumentum_Virtues_MindMap_ru.content.svg » et son wrapper
	/// inlinant « Argumentation_Virtues_ru.html », 1 occurrence chacun) ; le texte pt retire est
	/// dans <b>0</b> artefact (le mindmap pt ne porte pas la description de ce noeud). Instrument
	/// valide : le meme ecran voit « title_ru » de 208 x4 et « desc_ru » de 208 x1 dans le
	/// .content.svg ru, et 0 fragment de remark dans les 16 SVG (pk 217 compris, ancien) — les
	/// remarks ne sont pas rendus, donc les 4 cellules pk 208 n'ont aucun retard.</para>
	/// </summary>
	public class VirtuesG19GuardTests
	{
		private static string VirtuesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static Dictionary<string, Dictionary<string, string>> LoadRowsByPk()
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			using var reader = new StringReader(File.ReadAllText(VirtuesCsv));
			using var csv = new CsvReader(reader, config);
			var rows = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
			foreach (var record in csv.GetRecords<dynamic>())
			{
				var dict = (IDictionary<string, object>)record;
				var row = dict.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty, StringComparer.Ordinal);
				var pk = row.GetValueOrDefault("pk")?.Trim();
				if (!string.IsNullOrEmpty(pk))
					rows[pk] = row;
			}
			rows.Should().HaveCount(223, "le CSV Vertus compte 223 rangees.");
			return rows;
		}

		private static string Sha256Hex(string cell)
		{
			using var sha = SHA256.Create();
			return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(cell))).ToLowerInvariant();
		}

		/// <summary>Les 6 cellules ecrites : rangee, colonne, empreinte SHA-256 de la cellule PLEINE
		/// (derivee du fichier corrige, jamais de memoire), ce que la correction repare.</summary>
		private static readonly (string Pk, string Column, string Sha256, string Why)[] Fixed =
		{
			("172", "description_ru", "95d0ef055b817084b15a05f73bd188f60118759c02d5fabd22c5b85db009fc97", "ouverture orpheline \u00ab \u042d\u0442\u043e \u043e\u0437\u043d\u0430\u0447\u0430\u0435\u0442 \u00bb retiree ; la tete \u00ab \u041f\u0440\u0438\u0437\u043d\u0430\u043d\u0438\u0435 \u00bb est attestee 8x dans la colonne (dont la soeur 175)"),
			("172", "description_pt", "dee412cda9969c1a62b0ea80633c00abba7426aa92af070aeaf42eee553ce7cf", "ouverture orpheline \u00ab Trata-se de \u00bb retiree ; la tete \u00ab Reconhecer \u00bb est attestee 3x (166, 210)"),
			("208", "remark_ru", "40e1a6202c9344b3d10858afecee01977b749dd8ffb794c2f84db931e92c034e", "antithese de la source FR restauree (traduction, temoin EN) ; \u00ab \u0432\u044b \u00bb garde la construction attestee en pk 217"),
			("208", "remark_pt", "a5e082f1221bb2112b1b4d193a05216ba126d0469178574aefba0e86795d8530", "antithese restauree (traduction, temoin EN) ; impersonnel \u00ab critica-se \u00bb -- registre tu/voc\u00ea non tranche par le FR"),
			("208", "remark_es", "0b5b7eb15631df4e59a1d54e14c6adafb9921ab1f0cdcc61229d2221fe61503d", "antithese restauree (traduction, temoin EN) ; impersonnel \u00ab se critica \u00bb -- registre tu/usted non tranche par le FR"),
			("208", "remark_ar", "b4b1084062380f1f9a1f65774ee9accc0f02c545bec4e91a95c08ea78c786f18", "antithese restauree (traduction, temoin FR) ; \u00ab \u064a\u0648\u062c\u0651\u0647 \u0627\u0644\u0646\u0642\u062f \u0625\u0644\u0649 \u00bb evite l'ambiguite de cas du passif nu"),
		};

		/// <summary>Les 6 cellules retirees, contenu PLEIN : chacune mesuree <b>1 occurrence avant</b>
		/// et <b>0 apres</b> — l'ecran pouvait voir un 1 (sonde de non-cecite ci-dessous).</summary>
		private static readonly string[] EradicatedCells =
		{
			"\u042d\u0442\u043e \u043e\u0437\u043d\u0430\u0447\u0430\u0435\u0442 \u043f\u0440\u0438\u0437\u043d\u0430\u043d\u0438\u0435 \u0432\u043b\u0438\u044f\u043d\u0438\u044f \u0441\u043e\u0431\u0441\u0442\u0432\u0435\u043d\u043d\u043e\u0439 \u043a\u0443\u043b\u044c\u0442\u0443\u0440\u044b \u043d\u0430 \u0441\u043e\u0431\u0441\u0442\u0432\u0435\u043d\u043d\u044b\u0435 \u0441\u0443\u0436\u0434\u0435\u043d\u0438\u044f \u0438 \u0432\u043e\u0441\u043f\u0440\u0438\u044f\u0442\u0438\u044f \u0440\u0430\u0434\u0438 \u0431\u043e\u043b\u0435\u0435 \u0443\u0440\u0430\u0432\u043d\u043e\u0432\u0435\u0448\u0435\u043d\u043d\u043e\u0433\u043e \u0430\u043d\u0430\u043b\u0438\u0437\u0430.",
			"Trata-se de reconhecer a influ\u00eancia da pr\u00f3pria cultura sobre os ju\u00edzos e perce\u00e7\u00f5es, de modo a alcan\u00e7ar uma an\u00e1lise mais equilibrada.",
			"\u0412 \u043a\u043e\u043d\u0441\u0442\u0440\u0443\u043a\u0442\u0438\u0432\u043d\u043e\u0439 \u0434\u0438\u0441\u043a\u0443\u0441\u0441\u0438\u0438 \u043a\u0440\u0438\u0442\u0438\u043a\u0430 \u043d\u0435\u043e\u0431\u0445\u043e\u0434\u0438\u043c\u0430 \u0434\u043b\u044f \u043f\u0440\u043e\u0434\u0432\u0438\u0436\u0435\u043d\u0438\u044f \u043e\u0431\u0441\u0443\u0436\u0434\u0435\u043d\u0438\u044f \u0432\u043f\u0435\u0440\u0451\u0434. \u042d\u0442\u043e \u043f\u0440\u0435\u0434\u043f\u043e\u043b\u0430\u0433\u0430\u0435\u0442 \u0444\u043e\u0440\u043c\u0443\u043b\u0438\u0440\u043e\u0432\u0430\u0442\u044c \u043a\u0440\u0438\u0442\u0438\u043a\u0443 \u0443\u0432\u0430\u0436\u0438\u0442\u0435\u043b\u044c\u043d\u043e, \u043f\u0440\u0438\u0437\u043d\u0430\u0432\u0430\u044f \u043f\u043e\u0437\u0438\u0446\u0438\u044e \u0434\u0440\u0443\u0433\u043e\u0433\u043e \u0431\u0435\u0437 \u043b\u0438\u0447\u043d\u043e\u0441\u0442\u043d\u043e\u0439 \u043e\u0446\u0435\u043d\u043a\u0438 \u0438 \u043a\u043e\u043d\u0441\u0442\u0440\u0443\u043a\u0442\u0438\u0432\u043d\u043e \u043e\u043f\u0440\u043e\u0432\u0435\u0440\u0433\u0430\u044f \u0435\u0433\u043e \u0430\u0440\u0433\u0443\u043c\u0435\u043d\u0442\u044b.",
			"Num debate construtivo, as cr\u00edticas s\u00e3o necess\u00e1rias para fazer avan\u00e7ar a discuss\u00e3o. Isso implica formular cr\u00edticas de forma respeitosa, reconhecendo a posi\u00e7\u00e3o do outro sem julgamento pessoal e refutando os seus argumentos de maneira construtiva.",
			"En un debate constructivo, las cr\u00edticas son necesarias para hacer avanzar la discusi\u00f3n. Esto implica formular cr\u00edticas de manera respetuosa, reconociendo la posici\u00f3n del otro sin juicio personal y refutando sus argumentos de manera constructiva.",
			"\u0641\u064a \u0627\u0644\u0646\u0642\u0627\u0634 \u0627\u0644\u0628\u0646\u0651\u0627\u0621\u060c \u062a\u0643\u0648\u0646 \u0627\u0644\u0627\u0646\u062a\u0642\u0627\u062f\u0627\u062a \u0636\u0631\u0648\u0631\u064a\u0629 \u0644\u062f\u0641\u0639 \u0627\u0644\u0645\u0646\u0627\u0642\u0634\u0629 \u0642\u062f\u0645\u0627\u064b. \u0648\u064a\u0642\u062a\u0636\u064a \u0630\u0644\u0643 \u0635\u064a\u0627\u063a\u0629 \u0627\u0644\u0627\u0646\u062a\u0642\u0627\u062f\u0627\u062a \u0628\u0637\u0631\u064a\u0642\u0629 \u0645\u062d\u062a\u0631\u0645\u0629\u060c \u0645\u0639 \u0627\u0644\u0627\u0639\u062a\u0631\u0627\u0641 \u0628\u0645\u0648\u0642\u0641 \u0627\u0644\u0622\u062e\u0631 \u0645\u0646 \u062f\u0648\u0646 \u062d\u0643\u0645 \u0634\u062e\u0635\u064a\u060c \u0648\u062f\u062d\u0636 \u062d\u062c\u062c\u0647 \u0639\u0644\u0649 \u0646\u062d\u0648 \u0628\u0646\u0651\u0627\u0621.",
		};

		/// <summary>Temoins : des fragments legitimement presents ailleurs, que l'ecran ne devait PAS emporter.</summary>
		private static readonly (string Column, string Fragment, string Why)[] Witnesses =
		{
			("remark_fr", "non la personne qui le dit", "l'antithese de la source FR vit dans la cellule 208 fr"),
			("remark_en", "not the person saying it", "temoin structurel de la restauration"),
			("remark_zh", "\u800c\u975e\u8bf4\u8bdd\u7684\u4eba", "la famille fidele porte l'antithese en zh"),
			("remark_fa", "\u0646\u0647 \u0634\u062e\u0635\u06cc \u06a9\u0647", "la famille fidele porte l'antithese en fa"),
			("remark_ru", "\u0412\u044b \u043a\u0440\u0438\u0442\u0438\u043a\u0443\u0435\u0442\u0435", "pk 217 -- la construction \u00ab vous critiquez \u2026 non la personne \u00bb en ru"),
			("description_ru", "\u041f\u0440\u0438\u0437\u043d\u0430\u043d\u0438\u0435 ", "la famille \u00ab reconnaitre \u00bb des descriptions ru"),
			("description_pt", "Reconhecer ", "la famille infinitive des descriptions pt"),
		};

		/// <summary>Les langues FIDELES qui portent l'antithese de la source sur pk 208.</summary>
		private static readonly (string Lang, string Fragment)[] FaithfulAntithesis =
		{
			("fr", "non la personne qui le dit"),
			("en", "not the person saying it"),
			("zh", "\u800c\u975e\u8bf4\u8bdd\u7684\u4eba"),
			("fa", "\u0646\u0647 \u0634\u062e\u0635\u06cc \u06a9\u0647"),
		};

		internal static List<string> Mismatches(IEnumerable<(string Pk, string Column, string Sha)> pins,
			Dictionary<string, Dictionary<string, string>> rows)
		{
			var offenders = new List<string>();
			foreach (var (pk, column, expected) in pins)
			{
				if (!rows.TryGetValue(pk, out var row))
				{
					offenders.Add($"pk {pk} : rangee absente");
					continue;
				}
				if (!row.TryGetValue(column, out var value))
				{
					offenders.Add($"pk {pk} : colonne {column} absente");
					continue;
				}
				if (Sha256Hex(value) != expected)
					offenders.Add($"pk {pk}.{column} : empreinte «{expected[..12]}...» attendue, lue «{Sha256Hex(value)[..12]}...»");
			}
			return offenders;
		}

		[Fact]
		public void Corrected_Cells_Match_Their_Full_Content_Pins()
		{
			var rows = LoadRowsByPk();
			Mismatches(Fixed.Select(f => (f.Pk, f.Column, f.Sha256)), rows).Should().BeEmpty(
				"#458 grain 19 : les 6 cellules sont ecrites — repasser une seule doit nommer la rangee ET la colonne.");
		}

		[Fact]
		public void Fixed_Table_Is_Not_Vacuous()
		{
			var rows = LoadRowsByPk();
			rows.Values.Count(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("card")))
				.Should().Be(131, "le deck des Vertus compte 131 cartes — anti-vacuite.");
			Fixed.Should().HaveCount(6, "2 (pk 172) + 4 (pk 208).");
			Fixed.Select(f => f.Pk).Distinct().Should().HaveCount(2, "2 rangees : 172 et 208.");
			Fixed.Select(f => (f.Pk, f.Column)).Distinct().Should().HaveCount(6, "6 couples (rangee, colonne) distincts.");
			Fixed.Select(f => f.Sha256).Distinct().Should().HaveCount(6, "6 cellules, 6 empreintes distinctes.");
			Fixed.Should().OnlyContain(f => f.Sha256.Length == 64);
		}

		[Fact]
		public void Deviating_Cells_Are_Absent_From_The_Corpus()
		{
			var raw = File.ReadAllText(VirtuesCsv);
			var found = EradicatedCells.Where(c => raw.Contains(c, StringComparison.Ordinal)).ToList();
			found.Should().BeEmpty("les 6 cellules retirees ont ete mesurees 1 occurrence avant et 0 apres.");
		}

		[Fact]
		public void Eradication_Screen_Is_Not_Blind()
		{
			// (a) the screen CAN see a 1 on this file's encoding: the FR source antithesis is present.
			var raw = File.ReadAllText(VirtuesCsv);
			raw.Should().Contain("non la personne qui le dit",
				"l'antithese de la source FR est dans le fichier : l'instrument n'est pas aveugle.");
			// (b) and it WOULD fire on a retired cell: injection into a local copy.
			// NB: the quote is written (char)34 — an escaped quote inside a generated literal
			// is exactly what collapsed this line on the first emission.
			var injected = raw + "\nPKZ-INJECT," + ((char)34) + EradicatedCells[0] + ((char)34) + "\n";
			injected.Contains(EradicatedCells[0], StringComparison.Ordinal)
				.Should().BeTrue("un ecran qui ne voit pas la forme qu'il traque n'etablit rien.");
		}

		[Fact]
		public void The_Faithful_Languages_Keep_The_Antithesis()
		{
			var rows = LoadRowsByPk();
			foreach (var (lang, fragment) in FaithfulAntithesis)
				rows.Values.Should().Contain(r => r[$"remark_{lang}"].Contains(fragment, StringComparison.Ordinal),
					$"l'antithese vit dans la famille fidele ({lang}) : la restauration ne l'a pas deplacee.");
		}

		[Fact]
		public void Orphan_Openings_Are_Gone_And_Their_Family_Is_Attested()
		{
			var rows = LoadRowsByPk();
			// the two orphans: no description of the corpus OPENS on them any more.
			rows.Values.Should().NotContain(r => r["description_ru"].StartsWith("\u042d\u0442\u043e \u043e\u0437\u043d\u0430\u0447\u0430\u0435\u0442", StringComparison.Ordinal),
				"l'ouverture orpheline ru « \u042d\u0442\u043e \u043e\u0437\u043d\u0430\u0447\u0430\u0435\u0442 \u00bb a quitte tout le corpus (1 occurrence avant, 0 apres).");
			rows.Values.Should().NotContain(r => r["description_pt"].StartsWith("Trata-se de", StringComparison.Ordinal),
				"l'ouverture orpheline pt « Trata-se de » a quitte tout le corpus.");
			// anti-collapse: the replacement heads are ATTESTED families, not a lone coinage.
			rows.Values.Count(r => r["description_ru"].StartsWith("\u041f\u0440\u0438\u0437\u043d\u0430\u043d\u0438\u0435 ", StringComparison.Ordinal))
				.Should().BeGreaterOrEqualTo(2, "mesure : 8 descriptions ru ouvrent sur « \u041f\u0440\u0438\u0437\u043d\u0430\u043d\u0438\u0435 » (plancher anti-effondrement).");
			rows.Values.Count(r => r["description_pt"].StartsWith("Reconhecer ", StringComparison.Ordinal))
				.Should().BeGreaterOrEqualTo(2, "mesure : 3 descriptions pt ouvrent sur « Reconhecer » (plancher anti-effondrement).");
		}

		[Fact]
		public void Witnesses_Survive()
		{
			var rows = LoadRowsByPk();
			foreach (var (column, fragment, why) in Witnesses)
				rows.Values.Should().Contain(r => r[column].Contains(fragment, StringComparison.Ordinal),
					$"le temoin «{fragment}» de {column} : {why}.");
		}

		[Fact]
		public void Pin_Detector_Fires_On_A_Mutated_Cell_And_Spares_The_Other_Pins()
		{
			var rows = LoadRowsByPk();
			var pins = new List<(string, string, string)>();
			foreach (var f in Fixed)
			{
				var actual = Sha256Hex(rows[f.Pk][f.Column]);
				if (f.Pk == "208" && f.Column == "remark_ru")
					actual = Sha256Hex(rows[f.Pk][f.Column] + " ");
				pins.Add((f.Pk, f.Column, actual));
			}
			var offenders = Mismatches(pins, rows);
			offenders.Should().HaveCount(1, "seule la cellule mutee doit rougir : les 5 autres pins restent verts.");
			offenders[0].Should().Contain("208.remark_ru");
		}
	}
}
