using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde G2-C-W (#1499, Q-12 1a/2a/3a du 24/09, arbitrage ai-01
	/// [#458 c.5825475175](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5825475175)) :
	/// les 11 gloses didactiques sont retirées, PK 598 porte son exemple 2024 restauré
	/// (source <c>3eb08fc6^</c>), PK 1361 est au registre poli en fr/ru/pt/es/zh/fa.
	///
	/// ⚠️ Périmètre du registre : PK 1361 SEUL. Le deck conserve ~30 tutoiements
	/// légitimes ailleurs (dialogues des PK 403-1397, mesuré 25/09) — une sonde
	/// deck-wide « 0 tu » serait rouge par construction et hors arbitrage
	/// (« fr + ce que ta sonde trouve sur PK 1361 »).
	///
	/// Ce que cette garde n'établit pas : la justesse des traductions ; la qualité
	/// littéraire des 93 reformulations restantes du dossier ; l'état des PDF
	/// (régénération post-merge, 8 langues).
	/// </summary>
	public class FallaciesGlossRegisterGateTests
	{
		private static string FallaciesCsv =>
			Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		// 'tu' pronom : bornes lettres (accents inclus) — laisse « coutumes », « virtuosité » tranquilles ;
		// t'élisions avec les DEUX apostrophes (U+0027 et U+2019) et le verbe borné à droite.
		private static readonly Regex TuWord = new Regex(
			@"(?<![A-Za-zÀ-ÿ])tu(?![A-Za-zÀ-ÿ])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly Regex TElision = new Regex(
			@"(?<![A-Za-zÀ-ÿ])t['’](es|as|avais|aurais|est|étais|serais|a|ai|aie|peux|pourras|pourrais|veux|voudrais|vois|voyais|sais|savais|fais|faisais|penses|pensais|crois|croyais|dis|disais|appartiens|entends|appelles)(?![A-Za-zÀ-ÿ])",
			RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>Texte 1361 fr AVANT conversion (backup .BEFORE-g2cw) — témoin rouge.</summary>
		private const string Pk1361FrBefore =
			"À t’entendre, tout achat est immoral. Pourtant je t’ai vu faire les soldes l’autre jour, et ma morale le tolère.";

		/// <summary>Texte 598 en composite (#1265) — témoin rouge de la restauration.</summary>
		private const string Pk598EnComposite =
			"The poll was conducted outside the conference: 90% of those surveyed support the motion. The country's opinion is therefore settled.";

		/// <summary>658 fr avec glose (leçon après le dernier tiret) — témoin rouge du retrait.</summary>
		private const string Pk658FrWithGloss =
			"Cette affirmation est vraie. — Mais comment le savez-vous ? Je l’ai vérifiée. — Mais comment avez-vous vérifié cette vérification ? Et la vérification de cette vérification, comment l’avez-vous vérifiée ? … — Régression à l’infini de la justification épistémique : chaque niveau de preuve en exige un autre.";

		private static (List<string> headers, Dictionary<string, string[]> byPk) Load()
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};
			using var reader = new StreamReader(FallaciesCsv);
			using var csv = new CsvReader(reader, config);
			csv.Read();
			csv.ReadHeader();
			var headers = (csv.HeaderRecord ?? Array.Empty<string>()).ToList();
			var byPk = new Dictionary<string, string[]>();
			while (csv.Read())
			{
				var row = new string[headers.Count];
				for (var i = 0; i < headers.Count; i++)
				{
					row[i] = csv.GetField(i) ?? string.Empty;
				}
				var pk = row[headers.IndexOf("PK")]?.Trim() ?? "";
				if (pk.Length > 0)
				{
					byPk[pk] = row;
				}
			}
			return (headers, byPk);
		}

		private static string Cell(Dictionary<string, string[]> byPk, List<string> headers, string pk, string column)
		{
			byPk.Keys.Should().Contain(pk, $"PK {pk} must exist in the Fallacies CSV");
			var idx = headers.IndexOf(column);
			idx.Should().BeGreaterThanOrEqualTo(0, $"column {column} must exist (header guard)");
			return byPk[pk][idx];
		}

		/// <summary>Cellules « pk.column » dont la valeur courante diffère de l'attendu.</summary>
		private static List<string> Mismatches(
			(List<string> headers, Dictionary<string, string[]> byPk) csv,
			Dictionary<string, Dictionary<string, string>> expectedByPk)
		{
			var bad = new List<string>();
			foreach (var (pk, byLang) in expectedByPk)
			{
				foreach (var (lang, expected) in byLang)
				{
					if (Cell(csv.byPk, csv.headers, pk, $"example_{lang}") != expected)
					{
						bad.Add($"{pk}.example_{lang}");
					}
				}
			}
			return bad;
		}

		private static List<string> Mismatches(
			(List<string> headers, Dictionary<string, string[]> byPk) csv,
			string pk,
			Dictionary<string, string> expectedByLang)
		{
			var bad = new List<string>();
			foreach (var (lang, expected) in expectedByLang)
			{
				if (Cell(csv.byPk, csv.headers, pk, $"example_{lang}") != expected)
				{
					bad.Add($"{pk}.example_{lang}");
				}
			}
			return bad;
		}

		[Fact]
		public void Header_Exposes_Pk_And_All_Eight_Example_Columns()
		{
			var (headers, _) = Load();
			headers.Should().Contain("PK", "header guard: PK column");
			foreach (var lang in FallaciesGlossRegisterGate.Languages)
			{
				headers.Should().Contain($"example_{lang}", $"header guard: example_{lang}");
			}
		}

		[Fact]
		public void Gloss_Cells_Match_Post_Surgery_Expectations()
		{
			var csv = Load();
			var bad = Mismatches(csv, FallaciesGlossRegisterGate.GlossExpected);
			bad.Should().BeEmpty(
				$"the 11 didactic glosses (88 cells) must stay removed; drifted cells: {string.Join(", ", bad)}");
		}

		[Fact]
		public void Pk598_Example_Matches_The_Restored_2024_Text()
		{
			var csv = Load();
			var bad = Mismatches(csv, "598", FallaciesGlossRegisterGate.Pk598Expected);
			bad.Should().BeEmpty(
				$"PK 598 must carry the 2024 example restored from 3eb08fc6^ (pre-composite #1265); drifted cells: {string.Join(", ", bad)}");
		}

		[Fact]
		public void Pk1361_Example_Matches_The_Polite_Register_Decision()
		{
			var csv = Load();
			var bad = Mismatches(csv, "1361", FallaciesGlossRegisterGate.Pk1361Expected);
			bad.Should().BeEmpty(
				"PK 1361 must carry the Q-12 3a register conversion (fr/ru/pt/es/zh/fa; ar and en deliberately unchanged, pinned); " +
				$"drifted cells: {string.Join(", ", bad)}");
		}

		[Fact]
		public void Pk1361_French_Example_Has_No_Tu_Forms()
		{
			var csv = Load();
			var cell = Cell(csv.byPk, csv.headers, "1361", "example_fr");

			TuWord.IsMatch(cell).Should().BeFalse($"PK 1361 example_fr must be vouvoyé, got: {cell}");
			TElision.IsMatch(cell).Should().BeFalse($"PK 1361 example_fr must be vouvoyé, got: {cell}");

			// Self-test des sondes : l'élision VOIT le texte d'avant (il est fait de
			// t'élisions, pas de « tu » nus — mesuré)…
			TElision.IsMatch(Pk1361FrBefore).Should().BeTrue("the elision-probe must see the pre-conversion text");
			// …et ne crient pas sur les faux positifs lexicaux.
			TuWord.IsMatch("Il en coûte de coutumes rompre la virtuosité du tabou.").Should().BeFalse(
				"the tu-probe must not flag 'tu' inside words (coutumes, virtuosité, tabou)");
		}

		// --- Témoins rouges : le comparateur NOMME la cellule quand le défaut revient.

		[Fact]
		public void Witness_Comparator_Names_The_Cell_When_A_Gloss_Is_Re_Appended()
		{
			var csv = Load();
			var mutated = new Dictionary<string, string[]>(csv.byPk.ToDictionary(
				kv => kv.Key, kv => (string[])kv.Value.Clone()));
			var idx = csv.headers.IndexOf("example_fr");
			mutated["658"][idx] = Pk658FrWithGloss;

			var view = (csv.headers, mutated);
			var bad = Mismatches(view, new Dictionary<string, Dictionary<string, string>>
			{
				["658"] = FallaciesGlossRegisterGate.GlossExpected["658"],
			});
			bad.Should().ContainSingle("re-appending the gloss to 658.example_fr must be named")
				.Which.Should().Be("658.example_fr");
		}

		[Fact]
		public void Witness_Comparator_Names_The_Cell_When_The_Composite_Example_Returns()
		{
			var csv = Load();
			var mutated = new Dictionary<string, string[]>(csv.byPk.ToDictionary(
				kv => kv.Key, kv => (string[])kv.Value.Clone()));
			var idx = csv.headers.IndexOf("example_en");
			mutated["598"][idx] = Pk598EnComposite;

			var view = (csv.headers, mutated);
			var bad = Mismatches(view, "598", FallaciesGlossRegisterGate.Pk598Expected);
			bad.Should().ContainSingle("re-injecting the #1265 composite into 598.example_en must be named")
				.Which.Should().Be("598.example_en");
		}

		[Fact]
		public void Witness_Register_Probe_Flags_The_Pre_Conversion_Text()
		{
			// Le texte d'avant la conversion (backup) est fait de t'élisions (« À t'entendre…
			// je t'ai vu… ») : c'est la SONDE ÉLISION qui doit le voir — pas TuWord, qui
			// ne verrait qu'un « tu » nu (asserté faux exprès : documente la forme réelle).
			TElision.IsMatch(Pk1361FrBefore).Should().BeTrue(
				"the pre-conversion text is built from t-elisions; the elision-probe must see it");
			TuWord.IsMatch(Pk1361FrBefore).Should().BeFalse(
				"the pre-conversion text carries no bare 'tu' (measured 25/09) - documented on purpose");

			// Et une cellule 1361 re-mutée en tutoiement serait vue (simulation hors CSV).
			var csv = Load();
			var cell = Cell(csv.byPk, csv.headers, "1361", "example_fr");
			var reverted = cell.Replace("vous ", "tu ");
			TuWord.IsMatch(reverted).Should().BeTrue("a reverted PK 1361 must trip the tu-probe");
			TElision.IsMatch(cell.Replace("vous ", "t'")).Should().BeTrue("an elision form must trip the elision-probe");
		}
	}
}
