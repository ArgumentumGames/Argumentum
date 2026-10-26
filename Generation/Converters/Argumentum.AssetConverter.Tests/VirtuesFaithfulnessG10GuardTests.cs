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
	/// Grain 10 (#458, pool c.6036602091) — corrections de FIDELITE des Vertus, <b>sur delegation</b> :
	/// les Vertus n'ont jamais ete imprimees (la premisse « EN imprimee » du dossier #1793 etait fausse,
	/// cf. c.6036588022). <b>28 cellules / 17 rangees / 6 langues</b> (en ru pt es ar fa).
	/// <para>pk 54 description (en ru pt es) — la vertu etait decrite comme le VICE dans exactement quatre
	/// langues ; ar/fa/zh portaient la vertu ET gardaient « liees aux consequences » : ils donnent le modele.
	/// pk 162 description (en) — l'EN seul derivait (« the opponent », « attainable ») ; 7 langues fideles.
	/// pk 128 titre (en ru es ar) — « solide » affaibli ; modeles pt/fa/zh.
	/// pk 176 titre (en ru pt es ar fa) — titre retreci a l'opposant alors que la description dit
	/// « parties prenantes » ; modele zh.</para>
	/// <para><b>13 points finaux EN</b> : la famille typographique du dossier #1793. Elle n'a PAS d'ecran
	/// d'eradication — la forme ancienne est un PREFIXE de la nouvelle, donc un ecran `Contains` matcherait
	/// encore la cellule corrigee. La garde est l'epingle + le fait de terminaison dedie.</para>
	/// <para>0 cellule FR touchee : le FR est la source des 5 rangees.</para>
	/// </summary>
	public class VirtuesFaithfulnessG10GuardTests
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

		/// <summary>Les 28 cellules ecrites : rangee, colonne, empreinte SHA-256 de la cellule PLEINE
		/// (derivee du fichier corrige, jamais de memoire), ce que la correction repare.</summary>
		private static readonly (string Pk, string Column, string Sha256, string Why)[] Fixed =
		{
			("54", "description_en", "49fded8b7cf129909240ef7921affcc80df650d3d02f4475023012c5052c855f", "restaure la stance de la vertu (FR \u00ab Refuser de manipuler \u00bb) ; modeles ar/fa/zh (le renversement vivait dans exactement en/ru/pt/es)"),
			("54", "description_ru", "0accc66a90fd0ae8327f1abee252212b698b93b77a2044b4ff969d5e8263dc05", "restaure la stance ET \u00ab liees aux consequences \u00bb, perdus ensemble avec le renversement"),
			("54", "description_pt", "529b6254470519ac36b01e6ef9555f6b3fb457d8da84ed5f453174af42964dc9", "restaure la stance ET le lien aux consequences ; le pt avait perdu les deux (« ligadas as consequencias » absent)"),
			("54", "description_es", "811af0b8341b5421eee71761164573b5de1f81bfc007587782549f94dfef3d3f", "restaure la stance ET le lien aux consequences ; l'es avait perdu les deux (« vinculadas a las consecuencias » absent)"),
			("162", "description_en", "28b59e41d16f67fe532625a250fd607e39cded585fb93c9c8ea745dc57d5e2b9", "l'EN seul derivait (\u00ab the opponent \u00bb, \u00ab attainable \u00bb) alors que les 7 autres langues suivent le FR"),
			("128", "title_en", "0bfd39411f894f8e48de25145ee880d3dec29d06cac402e56817e98863077ca3", "\u00ab solide \u00bb affaibli en \u00ab Acceptable \u00bb (modeles pt/fa/zh)"),
			("128", "title_ru", "fe0eb36336234d0dd77a78989919ba10720091d3da69f413c2c3caf5b1f72426", "\u00ab solide \u00bb affaibli en \u00ab \u041f\u0440\u0438\u0435\u043c\u043b\u0435\u043c\u0430\u044f \u00bb (modele pt \u00ab solidA \u00bb, fa, zh)"),
			("128", "title_es", "cb1e8f1583bc98b90d1a9aae241382814817da8d252acf475a7749cad3c11f42", "\u00ab solide \u00bb affaibli en \u00ab aceptable \u00bb (modele pt \u00ab s\u00f3lida \u00bb)"),
			("128", "title_ar", "8a89d864186f8cd9c6ca56de73870ba485243896b8e8fc25a51fa17141b7fd40", "\u00ab solide \u00bb affaibli en \u00ab\u0645\u0642\u0628\u0648\u0644\u00bb (modeles pt/fa/zh)"),
			("176", "title_en", "c77a36b49c9134b4e9386c644970b77abf144e750122c65b90860511c9cfd89b", "titre retreci a l'opposant alors que sa propre description dit \u00ab the parties to an exchange \u00bb"),
			("176", "title_ru", "098f9c3664fc991a777eec9a699f4f78c56307e8319299f183a787913f1c0e3b", "titre retreci a \u00ab\u043e\u043f\u043f\u043e\u043d\u0435\u043d\u0442\u0430\u00bb ; la description dit \u00ab\u0441\u0442\u043e\u0440\u043e\u043d\u0430\u043c \u043e\u0431\u043c\u0435\u043d\u0430\u00bb"),
			("176", "title_pt", "49b1b7393da8cb6fab61881a3fb6aa659a66743271fce414590d382a66e4b31f", "titre retreci a \u00abadvers\u00e1rio\u00bb ; la description dit \u00abpartes interessadas\u00bb"),
			("176", "title_es", "73b2857d3d554540d218b01f00c6b1de033c16a24d170a1022c5f64817b54c95", "titre retreci a \u00abadversario\u00bb ; la description dit \u00abpartes interesadas\u00bb"),
			("176", "title_ar", "8c151beefe28ad14c9990d1ab89c235a76f6d47ea03252e76aa4b8938da7a5ef", "titre retreci a \u00ab\u0627\u0644\u062e\u0635\u0645\u00bb ; la description dit \u00ab\u0644\u0623\u0637\u0631\u0627\u0641 \u0627\u0644\u062a\u0628\u0627\u062f\u0644\u00bb (modele zh \u76f8\u5173\u65b9)"),
			("176", "title_fa", "c2cf74ed1b77fbfc5ec01913995907bab35883be385777dc429d48103e8d7033", "titre retreci a \u00ab\u0637\u0631\u0641 \u0645\u0642\u0627\u0628\u0644\u00bb ; la description dit \u00ab\u0637\u0631\u0641\u200c\u0647\u0627\u06cc \u062f\u0631\u06af\u06cc\u0631\u00bb (modele zh)"),
			("2", "description_en", "fdb1b89bbea774b5d26359af7af29533e17779e27cf65e0b78c428313a3903c0", "point final absent (famille typographique EN, 13 cellules)"),
			("3", "description_en", "b974d6fbf2e5be1d0d73229a3acb7c155956a1d5fb9ad2625cf03bcce348f893", "point final absent (famille typographique EN, 13 cellules)"),
			("4", "description_en", "4fe05cadf2db7146c3ee74e0c5fd10d7579f4c4010ff106820279c598d6fdf66", "point final absent (famille typographique EN, 13 cellules)"),
			("5", "description_en", "dd3372ecb58246f13cd2fb0eb424ccf80c93da40fdcc7b82aedcee298fa032ed", "point final absent (famille typographique EN, 13 cellules)"),
			("6", "description_en", "205dc7de7d8998f80f037c0339fec7c1b396fbfdc2ff97f3c913b1aa3a7b79e0", "point final absent (famille typographique EN, 13 cellules)"),
			("7", "description_en", "c9fc116b9203881bdfea1e454f127fdd9c937a033aa1eb320b12d44fc5b56f89", "point final absent (famille typographique EN, 13 cellules)"),
			("8", "description_en", "5c323237cef34b26ee94847e7e5e287452b745409eefabe7b811b83255b0f4ce", "point final absent (famille typographique EN, 13 cellules)"),
			("11", "description_en", "b0239b080484307031d8c3d2027e9455466995e9b4d334837992095fa154de5e", "point final absent (famille typographique EN, 13 cellules)"),
			("12", "description_en", "c26dc439d8541b6c9703fa1e46c49cdae1d34d7ef3d683b27119533d3b129610", "point final absent (famille typographique EN, 13 cellules)"),
			("89", "description_en", "c3dc52d00a8a59269d52626605da9851752adb185e8953fbf6612674aa6b6c95", "point final absent (famille typographique EN, 13 cellules)"),
			("90", "description_en", "17bc61e55e0fe93d9440536eb26c05a52776493c3a1f8c2f1fb6df17d452b2a2", "point final absent (famille typographique EN, 13 cellules)"),
			("220", "description_en", "fe66898f978ef19c8ee70ddf1655d8af1adcfde8df0c47d8de9d3ef24d648e8d", "point final absent (famille typographique EN, 13 cellules)"),
			("221", "description_en", "73327bb550fd1636c08c4c56a3c0f80fb22044230d12ea8493980781510c5bcf", "point final absent (famille typographique EN, 13 cellules)"),
		};

		/// <summary>Les 15 cellules remplacees (hors les 13 points, cf. en-tete) : chacune a ete mesuree
		/// <b>1 occurrence avant</b> et <b>0 apres</b> — l'ecran pouvait voir un 1.</summary>
		private static readonly string[] EradicatedCells =
		{
			"Manipulating one's interlocutor by resorting to threats or promises constitutes an affront to honest and ethical exchange.",
			"\u041c\u0430\u043d\u0438\u043f\u0443\u043b\u0438\u0440\u043e\u0432\u0430\u043d\u0438\u0435 \u0441\u043e\u0431\u0435\u0441\u0435\u0434\u043d\u0438\u043a\u043e\u043c \u043f\u043e\u0441\u0440\u0435\u0434\u0441\u0442\u0432\u043e\u043c \u0443\u0433\u0440\u043e\u0437 \u0438\u043b\u0438 \u043e\u0431\u0435\u0449\u0430\u043d\u0438\u0439 \u044f\u0432\u043b\u044f\u0435\u0442\u0441\u044f \u043f\u043e\u0441\u044f\u0433\u0430\u0442\u0435\u043b\u044c\u0441\u0442\u0432\u043e\u043c \u043d\u0430 \u0447\u0435\u0441\u0442\u043d\u044b\u0439 \u0438 \u044d\u0442\u0438\u0447\u0435\u0441\u043a\u0438\u0439 \u043e\u0431\u043c\u0435\u043d.",
			"Manipular o interlocutor recorrendo a amea\u00e7as ou promessas \u00e9 uma viola\u00e7\u00e3o da troca honesta e \u00e9tica.",
			"La manipulaci\u00f3n del interlocutor mediante amenazas o promesas constituye un atentado contra el intercambio honesto y \u00e9tico.",
			"The expected objective for the opponent must be attainable and fair, with measurable, clear and well-defined criteria for assessing success.",
			"Acceptable informal logic",
			"\u041f\u0440\u0438\u0435\u043c\u043b\u0435\u043c\u0430\u044f \u043d\u0435\u0444\u043e\u0440\u043c\u0430\u043b\u044c\u043d\u0430\u044f \u043b\u043e\u0433\u0438\u043a\u0430",
			"L\u00f3gica informal aceptable",
			"\u0645\u0646\u0637\u0642 \u063a\u064a\u0631 \u0635\u0648\u0631\u064a \u0645\u0642\u0628\u0648\u0644",
			"Taking the opponent's ideological biases into account",
			"\u0423\u0447\u0435\u0442 \u0438\u0434\u0435\u043e\u043b\u043e\u0433\u0438\u0447\u0435\u0441\u043a\u0438\u0445 \u043f\u0440\u0435\u0434\u0443\u0431\u0435\u0436\u0434\u0435\u043d\u0438\u0439 \u043e\u043f\u043f\u043e\u043d\u0435\u043d\u0442\u0430",
			"Considera\u00e7\u00e3o dos enviesamentos ideol\u00f3gicos do advers\u00e1rio",
			"Consideraci\u00f3n de los sesgos ideol\u00f3gicos del adversario",
			"\u0645\u0631\u0627\u0639\u0627\u0629 \u0627\u0644\u062a\u062d\u064a\u0632\u0627\u062a \u0627\u0644\u0623\u064a\u062f\u064a\u0648\u0644\u0648\u062c\u064a\u0629 \u0644\u062f\u0649 \u0627\u0644\u062e\u0635\u0645",
			"\u062f\u0631\u0646\u0638\u0631\u06af\u0631\u0641\u062a\u0646 \u0633\u0648\u06af\u06cc\u0631\u06cc\u200c\u0647\u0627\u06cc \u0627\u06cc\u062f\u0626\u0648\u0644\u0648\u0698\u06cc\u06a9 \u0637\u0631\u0641 \u0645\u0642\u0627\u0628\u0644",
		};

		/// <summary>Les 13 descriptions EN dont le point final manquait (famille typographique).</summary>
		private static readonly string[] TerminalDotPks =
		{
			"2", "3", "4", "5", "6", "7", "8", "11", "12", "89", "90", "220", "221",
		};

		/// <summary>Temoins : des fragments legitimement presents ailleurs, que l'ecran ne devait PAS emporter.</summary>
		private static readonly (string Column, string Fragment)[] Witnesses =
		{
			("title_fa", "\u0627\u0633\u062a\u0648\u0627\u0631"),
			("title_ru", "\u043e\u043f\u043f\u043e\u043d\u0435\u043d\u0442"),
			("title_ar", "\u0645\u0642\u0628\u0648\u0644"),
			("title_pt", "s\u00f3lida"),
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
				"#458 grain 10 : les 28 cellules sont ecrites — repasser une seule doit nommer la rangee ET la colonne.");
		}

		[Fact]
		public void Fixed_Table_Is_Not_Vacuous()
		{
			Fixed.Should().HaveCount(28, "4 + 1 + 4 + 6 + 13 cellules.");
			Fixed.Select(f => f.Pk).Distinct().Should().HaveCount(17, "17 rangees : 4 structurelles (54, 162, 128, 176) + les 13 points finaux.");
			Fixed.Select(f => (f.Pk, f.Column)).Distinct().Should().HaveCount(28, "28 couples (rangee, colonne) distincts : aucune epingle recopiee.");
			Fixed.Select(f => f.Sha256).Distinct().Should().HaveCount(28, "28 cellules, 28 empreintes distinctes.");
			Fixed.Select(f => f.Why).Distinct().Should().HaveCount(16, "15 raisons propres aux corrections structurelles + la famille typographique commune aux 13 points.");
			Fixed.Should().OnlyContain(f => f.Sha256.Length == 64);
		}

		[Fact]
		public void Deviating_Cells_Are_Absent_From_The_Corpus()
		{
			var raw = File.ReadAllText(VirtuesCsv);
			var found = EradicatedCells.Where(c => raw.Contains(c, StringComparison.Ordinal)).ToList();
			found.Should().BeEmpty("les 15 cellules deviantes ont ete mesurees 1 occurrence avant et 0 apres.");
		}

		[Fact]
		public void The_Thirteen_English_Descriptions_End_With_A_Period()
		{
			var rows = LoadRowsByPk();
			var offenders = TerminalDotPks.Where(pk => !rows[pk]["description_en"].TrimEnd().EndsWith(".", StringComparison.Ordinal)).ToList();
			offenders.Should().BeEmpty("un point ajoute n'a pas d'ecran d'eradication (la forme ancienne est un prefixe) : la garde est ici.");
		}

		[Fact]
		public void Witnesses_Survive()
		{
			var rows = LoadRowsByPk();
			foreach (var (column, fragment) in Witnesses)
				rows.Values.Should().Contain(r => r[column].Contains(fragment, StringComparison.Ordinal),
					$"le temoin «{fragment}» de {column} vit legitimement ailleurs : l'ecran ne visait que les cellules entieres.");
		}

		[Fact]
		public void Pin_Detector_Fires_On_A_Mutated_Cell_And_Spares_The_Other_Pins()
		{
			var rows = LoadRowsByPk();
			var pins = new List<(string, string, string)>();
			foreach (var f in Fixed)
			{
				var actual = Sha256Hex(rows[f.Pk][f.Column]);
				if (f.Pk == "54" && f.Column == "description_en")
					actual = Sha256Hex(rows[f.Pk][f.Column] + " ");
				pins.Add((f.Pk, f.Column, actual));
			}
			var offenders = Mismatches(pins, rows);
			offenders.Should().HaveCount(1, "seule la cellule mutee doit rougir : les 27 autres pins restent verts.");
			offenders[0].Should().Contain("54.description_en");
		}
	}
}