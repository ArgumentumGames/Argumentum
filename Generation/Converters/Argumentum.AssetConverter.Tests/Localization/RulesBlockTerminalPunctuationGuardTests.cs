using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Self-defending guard for grain 7 of pool #458 (Règles typographie, PR on master after
	/// the 2026-10-04 dossier docs/translation/458-fidelite-regles-6-langues-2026-10-04.md §4-§6).
	///
	/// MEASURED DEFECT FAMILY (the fix this guard pins):
	/// - 16 prose line-endings without terminal punctuation across 12 cells — Rules_08 (winner
	///   declaration), Rules_12 (first to 20 points) and Rules_15 (trump-card value + 1000-point
	///   game end), in ar/es/zh/fa only. The four reference languages (fr/en/ru/pt) all carry
	///   the period — including the «12 - (2 × depth).» period AFTER the closing parenthesis —
	///   so the missing marks were translation drop-offs, not source style. Screened with the
	///   same TERM sets as the corpus instrument (docs/corpus/regles-fidelity-instrument.py).
	/// - ru Rules_12: two typos in the joker sentence («вытазить» → вытянуть, «слудующую» →
	///   следующую) — correct forms were absent corpus-wide before the fix.
	/// - ru Rules_11: orphan fragment «…для добора. ые в середине стола.» — the tail of a
	///   sentence whose head vanished; its FR source clause («puis placez-les au milieu de la
	///   table») is already carried by «На середину стола мы кладем…», so the fragment was
	///   removed, not recomposed.
	///
	/// NAMED EXCLUSION (burn-down, see Rules_06_Colon_Exclusion_Still_Has_Its_Reason): the
	/// Rules_06 block ending with ':' in EACH of the 8 languages is the SOURCE's own style
	/// (a colon introducing the draw list — FR itself ends the block «…un certain nombre de
	/// cartes :»). It is a shared-source artifact, not a translation defect, and it is excluded
	/// from the terminal-punctuation screen BY NAME: the day the source is punctuated, the
	/// exclusion's companion test REDDENS and forces removing the exclusion WITH that fix.
	///
	/// Scope: the MAIN Rules CSV only. The Print &amp; Play CSV was never measured for block
	/// punctuation (grain 7's family lives in the main file); extending this guard there
	/// requires measuring it first, not assuming.
	/// </summary>
	public class RulesBlockTerminalPunctuationGuardTests
	{
		private const string MainRulesCsv = "Cards/Rules/Argumentum Rules - Cards.csv";

		// The 8 Rules language columns (Text is FR; the other 7 are the release languages).
		private static readonly string[] RulesLanguageColumns =
			{ "Text", "Text_en", "Text_ru", "Text_pt", "Text_es", "Text_ar", "Text_fa", "Text_zh" };

		// Terminal punctuation per column — EXACTLY the calibrated sets of the instrument
		// (docs/corpus/regles-fidelity-instrument.py, TERM) which measured the defect family.
		// Broadening these sets would silently weaken the screen; narrow them only with a
		// re-measure that keeps fr/en/ru/pt at exactly the one Rules_06 colon artifact.
		private static readonly Dictionary<string, string> TerminalByColumn = new()
		{
			["Text"] = ".!?…\"",
			["Text_en"] = ".!?…\"",
			["Text_ru"] = ".!?…»",
			["Text_pt"] = ".!?…»\"",
			["Text_es"] = ".!?…»\"",
			["Text_ar"] = ".!؟۔…",
			["Text_fa"] = ".!؟…",
			["Text_zh"] = "。！？…”』」：；",
		};

		// ru literals (grain 7): joker-sentence correct form, the two typos it replaces, the
		// orphan fragment, and the carrier sentence that must stay intact after its removal.
		private const string RuCorrectForm = "вытянуть следующую"; // вытянуть следующую
		private const string RuTypoDraw = "вытазить";   // вытазить
		private const string RuTypoNext = "слудующую"; // слудующую
		private const string RuOrphan = " в середине стола"; // " в середине стола"
		private const string RuCarrier = "для добора"; // "для добора"

		private static string FindRepoRoot() => TestRepoRoot.Find();

		/// <summary>
		/// Paragraph blocks (blank-line separated) whose LAST line is prose — i.e. neither a
		/// markdown heading (#) nor a list item (* - +). Headings and list items legitimately
		/// end unpunctuated in every language INCLUDING the source; only prose blocks carry
		/// the terminal-punctuation expectation. Mirrors the grain-7 measurement exactly.
		/// </summary>
		internal static List<string> ProseBlocks(string cell)
		{
			var blocks = new List<List<string>>();
			var current = new List<string>();
			foreach (var line in cell.Split('\n'))
			{
				if (line.Trim().Length == 0)
				{
					if (current.Count > 0) blocks.Add(current);
					current = new List<string>();
				}
				else
				{
					current.Add(line);
				}
			}
			if (current.Count > 0) blocks.Add(current);

			return blocks
				.Where(b => !IsStructural(b.Last(l => l.Trim().Length > 0).TrimStart()))
				.Select(b => string.Join("\n", b))
				.ToList();
		}

		private static bool IsStructural(string line) =>
			line.StartsWith("#") || line.StartsWith("*") || line.StartsWith("-") || line.StartsWith("+");

		/// <summary>The named exclusion: Rules_06's draw-list intro ends with ':' in EVERY
		/// language — the source's own style, mirrored by every translator. Dies with its
		/// reason (see the companion burn-down test).</summary>
		private static bool IsNamedExclusion(string pk, string block) =>
			pk == "Rules_06" && block.TrimEnd().EndsWith(":", StringComparison.Ordinal);

		private static Dictionary<string, Dictionary<string, string>> ReadRulesCells(string csvPath)
		{
			var byPk = new Dictionary<string, Dictionary<string, string>>();
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};
			using (var reader = new StreamReader(csvPath, Encoding.UTF8))
			using (var csv = new CsvReader(reader, config))
			{
				csv.Read();
				csv.ReadHeader();
				var headers = csv.HeaderRecord ?? Array.Empty<string>();
				int pkIdx = Array.IndexOf(headers, "pk");
				pkIdx.Should().BeGreaterThanOrEqualTo(0, "the Rules CSV must expose the pk column");

				var columnIndex = new Dictionary<string, int>();
				foreach (var col in RulesLanguageColumns)
				{
					int idx = Array.IndexOf(headers, col);
					if (idx >= 0) columnIndex[col] = idx;
				}
				columnIndex.Should().HaveCount(RulesLanguageColumns.Length,
					"the Rules CSV must expose all 8 language columns: {0}", string.Join(", ", RulesLanguageColumns));

				while (csv.Read())
				{
					var pk = csv.GetField(pkIdx) ?? string.Empty;
					byPk[pk] = columnIndex.ToDictionary(kv => kv.Key, kv => csv.GetField(kv.Value) ?? string.Empty);
				}
			}
			byPk.Should().HaveCount(15, "the main Rules CSV carries 15 rows (Rules_01..Rules_15)");
			return byPk;
		}

		[Fact]
		public void Rules_Prose_Blocks_End_With_Terminal_Punctuation_In_Every_Language()
		{
			var path = Path.Combine(FindRepoRoot(), MainRulesCsv);
			File.Exists(path).Should().BeTrue($"the Rules DataSet CSV must exist at {MainRulesCsv}");
			var byPk = ReadRulesCells(path);

			var offenders = new List<string>();
			foreach (var (pk, columns) in byPk)
			{
				foreach (var (col, cell) in columns)
				{
					if (string.IsNullOrWhiteSpace(cell)) continue;
					string terminals = TerminalByColumn[col];
					foreach (var block in ProseBlocks(cell))
					{
						string trimmed = block.TrimEnd();
						if (trimmed.Length == 0) continue;
						if (IsNamedExclusion(pk, block)) continue;
						if (terminals.IndexOf(trimmed[^1]) < 0)
						{
							string last = trimmed[^1].ToString();
							string code = char.IsSurrogate(trimmed[^1])
								? "surrogate pair"
								: $"U+{char.ConvertToUtf32(last, 0):X4}";
							offenders.Add($"{pk}.{col}: block ends with '{last}' ({code}) …{Tail(trimmed)}");
						}
					}
				}
			}
			offenders.Should().BeEmpty(
				"every prose block of the Rules CSV must end with terminal punctuation in all 8 languages " +
				"(grain 7, #458: the ar/es/zh/fa drop-offs of Rules_08/12/15 were measured and fixed on 2026-10-04; " +
				"a regression here is a NEW drop-off, not a style choice). Offenders: {0}",
				string.Join(" | ", offenders));
		}

		[Fact]
		public void Rules_06_Colon_Exclusion_Still_Has_Its_Reason()
		{
			// Burn-down for the named exclusion: it is valid ONLY while the source's draw-list
			// intro still ends with a colon in EVERY language. The day that colon disappears
			// (source punctuated), this REDDENS and the exclusion must be removed WITH that fix —
			// an exclusion must die with its reason, never rot in silence.
			var path = Path.Combine(FindRepoRoot(), MainRulesCsv);
			var byPk = ReadRulesCells(path);
			var missing = new List<string>();
			foreach (var col in RulesLanguageColumns)
			{
				var colonBlocks = ProseBlocks(byPk["Rules_06"][col])
					.Count(b => b.TrimEnd().EndsWith(":", StringComparison.Ordinal));
				if (colonBlocks != 1)
					missing.Add($"{col}: {colonBlocks} colon block(s)");
			}
			missing.Should().BeEmpty(
				"Rules_06 must carry EXACTLY ONE colon-ending prose block per language (the shared-source " +
				"artifact that justifies the terminal-punctuation exclusion). If this fails, the source style " +
				"changed: remove the exclusion IN THE SAME COMMIT as the change. Offenders: {0}",
				string.Join(" | ", missing));
		}

		[Fact]
		public void Rules_12_Ru_Joker_Sentence_Carries_The_Correct_Forms()
		{
			var path = Path.Combine(FindRepoRoot(), MainRulesCsv);
			var byPk = ReadRulesCells(path);
			var ru12 = byPk["Rules_12"]["Text_ru"];

			ru12.Should().Contain(RuCorrectForm,
				"the ru joker sentence of Rules_12 was corrected in grain 7 (#458): 'вытянуть следующую'");

			var typoHolders = new List<string>();
			foreach (var (pk, columns) in byPk)
			{
				foreach (var (col, cell) in columns)
				{
					if (cell.Contains(RuTypoDraw)) typoHolders.Add($"{pk}.{col}: {RuTypoDraw}");
					if (cell.Contains(RuTypoNext)) typoHolders.Add($"{pk}.{col}: {RuTypoNext}");
				}
			}
			typoHolders.Should().BeEmpty(
				"the grain-7 typos were measured absent from the whole corpus after the fix; any hit is a " +
				"regression. Offenders: {0}",
				string.Join(" | ", typoHolders));
		}

		[Fact]
		public void Rules_11_Ru_Has_No_Orphan_Fragment_And_Keeps_The_Carrier()
		{
			var path = Path.Combine(FindRepoRoot(), MainRulesCsv);
			var byPk = ReadRulesCells(path);
			var ru11 = byPk["Rules_11"]["Text_ru"];

			ru11.Should().NotContain(RuOrphan,
				"the orphan fragment 'ые в середине стола.' (tail of a sentence whose head vanished) was removed " +
				"in grain 7 (#458); its FR clause is already carried by 'На середину стола мы кладем…'");
			ru11.Should().Contain(RuCarrier,
				"the carrier sentence of the Rules_11 ru setup ('для добора') must stay intact");
		}

		private static string Tail(string text) =>
			text.Length <= 30 ? text : "…" + text[^30..];
	}
}
