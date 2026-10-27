using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Argumentum.AssetConverter.Tests.DatasetUpdater;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du nom de rôle du joueur qui pioche la carte de scénario, dans les règles du site
	/// (registre EAV, type « Game Rule » ; pool #458 c.6446645563 item 7). Consigne d'ai-01 à la
	/// fusion de #1998 : le rôle suit l'édition imprimée de 2022 (#1941 : « piocheur »,
	/// <c>drawer</c>), jamais la variante « lecteur ».
	///
	/// Le terme attendu par langue n'est pas recopié : il est lu dans la ligne « piocheur » du
	/// glossaire verrouillé (<c>PromptRulesCascadeDriftUser.txt</c>), par l'analyseur de
	/// <see cref="PromptRulesGlossaryConsistencyTests"/>. Deux contrôles :
	///   - présence : dans une rangée dont le fr porte « piocheur », chaque cellule non vide
	///     porte le terme du glossaire (en, pt, es, ar, fa, zh) ;
	///   - absence : aucune cellule « Game Rule » ne porte la forme « lecteur » de sa langue —
	///     reader / lector / leitor (les formes que #1941 a retirées des règles imprimées),
	///     lecteur pour le fr, قارئ / خواننده pour ar et fa, les deux langues de cette contre-revue.
	/// ru est hors des deux contrôles : la colonne décline le participe (« Берущим карту », 2 rangées),
	/// que le nominatif du glossaire ne couvre pas à l'échelle d'une cellule, et le russe n'a pas de
	/// forme « lecteur » à bannir (« Берущий карту » nommait déjà le piocheur, constat de #1941). zh est en
	/// présence seulement, pour la même raison.
	/// La correspondance est exacte : mot entier, pluriel simple admis, pour les langues latines ;
	/// sous-chaîne pour ar/fa/zh, où la forme contractée de l'arabe (« للساحب », ل devant l'article)
	/// compte pour le terme. Pas les replis de lemme d'<c>IsAttested</c>, faits pour un
	/// corpus entier : sur fa, son repli « کارت‌ » (le terme moins ses deux dernières lettres) se
	/// trouve dans chaque « کارت‌ها » (« cartes »), et le contrôle de présence ne verrait plus rien.
	/// </summary>
	public class DnnGameRuleDrawerRoleGuardTests
	{
		private const string RegistryCsv = "docs/dnn-localization/eav-content.csv";
		private const string GameRule = "Game Rule";
		private const string RowsCheck = "rows";
		private const string PresenceCheck = "presence";
		private const string AbsenceCheck = "absence";

		private static readonly string[] PresenceLanguages = { "en", "pt", "es", "ar", "fa", "zh" };
		private static readonly string[] LatinLanguages = { "fr", "en", "pt", "es" };

		// la forme « lecteur » du rôle, par langue
		private static readonly Dictionary<string, string> ReaderForms = new Dictionary<string, string>
		{
			["fr"] = "lecteur", ["en"] = "reader", ["es"] = "lector", ["pt"] = "leitor", ["ar"] = "قارئ", ["fa"] = "خواننده",
		};

		private static readonly Lazy<List<Dictionary<string, string>>> Registry = new Lazy<List<Dictionary<string, string>>>(ReadRegistry);

		private static List<Dictionary<string, string>> ReadRegistry()
		{
			var path = Path.Combine(TestRepoRoot.Find(), RegistryCsv.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le registre doit exister dans le checkout ({RegistryCsv})");
			var config = new CsvConfiguration(CultureInfo.InvariantCulture) { MissingFieldFound = null, HeaderValidated = null };
			var rows = new List<Dictionary<string, string>>();
			using (var reader = new StreamReader(path))
			using (var csv = new CsvReader(reader, config))
			{
				csv.Read();
				csv.ReadHeader();
				var header = csv.HeaderRecord ?? Array.Empty<string>();
				while (csv.Read())
				{
					var row = new Dictionary<string, string>(StringComparer.Ordinal);
					for (var i = 0; i < header.Length; i++) row[header[i]] = csv.GetField(i) ?? string.Empty;
					rows.Add(row);
				}
			}
			return rows;
		}

		private static List<Dictionary<string, string>> Copy() => Registry.Value.Select(r => new Dictionary<string, string>(r, StringComparer.Ordinal)).ToList();

		private static Dictionary<string, string> DrawerTerms()
		{
			var lines = PromptRulesGlossaryConsistencyTests.ParseGlossary().Where(t => t["fr"] == "piocheur").ToList();
			lines.Should().HaveCount(1, "le glossaire verrouillé doit porter une ligne « piocheur », et une seule");
			return lines[0];
		}

		// ar : « ل » devant l'article absorbe son alif (ل + الساحب = للساحب), et la forme contractée nomme le rôle
		private static string[] Forms(string term, string lang) =>
			lang == "ar" && term.StartsWith("ال", StringComparison.Ordinal) ? new[] { term, "لل" + term.Substring(2) } : new[] { term };

		private static bool Carries(string cell, string term, string lang) =>
			LatinLanguages.Contains(lang)
				? Regex.IsMatch(cell, @"(?<!\w)" + Regex.Escape(term) + @"(?:s|es)?(?!\w)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)
				: Forms(term, lang).Any(f => cell.Contains(f, StringComparison.Ordinal));

		private static string Replace(string cell, string term, string by, string lang) =>
			LatinLanguages.Contains(lang)
				? Regex.Replace(cell, @"(?<!\w)" + Regex.Escape(term), by, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)
				: Forms(term, lang).Aggregate(cell, (c, f) => c.Replace(f, by, StringComparison.Ordinal));

		private static List<Dictionary<string, string>> DrawerRows(List<Dictionary<string, string>> rows, Dictionary<string, string> terms) =>
			rows.Where(r => r["content_type"] == GameRule && Carries(r["fr"], terms["fr"], "fr")).ToList();

		/// <summary>
		/// Tous les contrôles de la garde sur <paramref name="rows"/> ; vide quand tout tient.
		/// Rendus plutôt qu'assertés, pour que chaque témoin montre lequel se déclenche.
		/// </summary>
		private static List<(string Check, string Key, string Lang, string Detail)> Problems(List<Dictionary<string, string>> rows)
		{
			var problems = new List<(string Check, string Key, string Lang, string Detail)>();
			var terms = DrawerTerms();
			var drawerRows = DrawerRows(rows, terms);
			if (drawerRows.Count == 0)
			{
				problems.Add((RowsCheck, "", "fr", $"aucune rangée « {GameRule} » dont le fr porte « {terms["fr"]} » : la garde ne verrait rien"));
			}

			foreach (var row in drawerRows)
			{
				foreach (var lang in PresenceLanguages)
				{
					if (!string.IsNullOrWhiteSpace(row[lang]) && !Carries(row[lang], terms[lang], lang))
					{
						problems.Add((PresenceCheck, row["key"], lang, $"le fr dit « {terms["fr"]} », la cellule ne porte pas « {terms[lang]} »"));
					}
				}
			}

			foreach (var row in rows.Where(r => r["content_type"] == GameRule))
			{
				foreach (var (lang, form) in ReaderForms)
				{
					if (Carries(row[lang], form, lang))
					{
						problems.Add((AbsenceCheck, row["key"], lang, $"la cellule porte « {form} », la variante « lecteur » du rôle"));
					}
				}
			}

			return problems;
		}

		private static string Describe(IEnumerable<(string Check, string Key, string Lang, string Detail)> problems) =>
			string.Join("\n", problems.Select(p => $"[{p.Check}] {p.Key} {p.Lang} : {p.Detail}"));

		[Fact]
		public void Registry_Game_Rules_Name_The_Drawer_Not_The_Reader()
		{
			var problems = Problems(Registry.Value);
			problems.Should().BeEmpty("le rôle suit l'imprimé 2022 (#1941) dans toutes les règles du registre :\n{0}", Describe(problems));
		}

		// Les langues des témoins sont écrites ici, pas lues dans les tableaux de la garde : une langue
		// retirée de la garde laisse son témoin rouge au lieu de l'emporter avec elle.
		[Theory]
		[InlineData("en")]
		[InlineData("pt")]
		[InlineData("es")]
		[InlineData("ar")]
		[InlineData("fa")]
		[InlineData("zh")]
		public void Witness_Drawer_Term_Removed_Is_Caught_By_Presence_Alone(string lang)
		{
			var terms = DrawerTerms();
			var rows = Copy();
			var targets = DrawerRows(rows, terms).Where(r => !string.IsNullOrWhiteSpace(r[lang])).ToList();
			targets.Should().NotBeEmpty("le témoin retire le terme {0} de rangées qui le portent", lang);
			foreach (var row in targets) row[lang] = Replace(row[lang], terms[lang], "X", lang);

			var problems = Problems(rows);
			problems.Should().HaveCount(targets.Count, "chaque rangée privée du terme se voit, et rien d'autre :\n{0}", Describe(problems));
			problems.Should().OnlyContain(p => p.Check == PresenceCheck && p.Lang == lang);
		}

		[Theory]
		[InlineData("fr")]
		[InlineData("en")]
		[InlineData("es")]
		[InlineData("pt")]
		[InlineData("ar")]
		[InlineData("fa")]
		public void Witness_Reader_Form_Added_Is_Caught_By_Absence_Alone(string lang)
		{
			var rows = Copy();
			var target = DrawerRows(rows, DrawerTerms())[0];
			target[lang] = target[lang] + " " + ReaderForms[lang] + " ";

			var problems = Problems(rows);
			problems.Should().HaveCount(1, "seule la forme ajoutée se voit :\n{0}", Describe(problems));
			problems[0].Check.Should().Be(AbsenceCheck);
			problems[0].Key.Should().Be(target["key"]);
			problems[0].Lang.Should().Be(lang);
		}

		/// <summary>
		/// Le défaut que vise la consigne : le terme du rôle remplacé partout par la forme
		/// « lecteur ».
		/// </summary>
		[Theory]
		[InlineData("ar")]
		[InlineData("fa")]
		[InlineData("en")]
		[InlineData("es")]
		[InlineData("pt")]
		public void Witness_Reader_Swapped_For_The_Drawer_Is_Caught(string lang)
		{
			var terms = DrawerTerms();
			var rows = Copy();
			foreach (var row in DrawerRows(rows, terms)) row[lang] = Replace(row[lang], terms[lang], ReaderForms[lang], lang);

			var checks = Problems(rows).Where(p => p.Lang == lang).Select(p => p.Check).Distinct().ToList();
			checks.Should().Contain(c => c == PresenceCheck, "le terme du glossaire a disparu des cellules {0}", lang);
			checks.Should().Contain(c => c == AbsenceCheck, "la forme « lecteur » est apparue dans les cellules {0}", lang);
		}

		/// <summary>
		/// ar : une cellule qui ne nomme le rôle que sous la forme contractée (« للساحب ») le nomme bien ;
		/// la présence ne doit pas la voir rouge.
		/// </summary>
		[Fact]
		public void Witness_Arabic_Contracted_Drawer_Still_Counts()
		{
			var terms = DrawerTerms();
			var rows = Copy();
			var target = DrawerRows(rows, terms)[0];
			var contracted = "لل" + terms["ar"].Substring(2);
			target["ar"] = target["ar"].Replace(terms["ar"], contracted, StringComparison.Ordinal);
			target["ar"].Contains(terms["ar"], StringComparison.Ordinal).Should().BeFalse("le témoin retire la forme pleine");
			target["ar"].Contains(contracted, StringComparison.Ordinal).Should().BeTrue("le témoin laisse la forme contractée");

			var problems = Problems(rows);
			problems.Should().BeEmpty("la forme contractée nomme le piocheur :\n{0}", Describe(problems));
		}
	}
}
