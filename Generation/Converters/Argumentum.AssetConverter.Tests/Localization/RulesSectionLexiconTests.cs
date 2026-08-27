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

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Structural guard for the recurring-section lexicon of the Rules deck (#1199).
	///
	/// The four sub-game rule cards — <c>Rules_07</c>, <c>Rules_09</c>, <c>Rules_11</c>,
	/// <c>Rules_13</c> — are structurally parallel: each carries exactly four <c>h2</c> headings,
	/// the sub-game name followed by the SAME three sections in the SAME order (fr reference:
	/// Materiel / Resume du jeu / Installation).
	///
	/// Those three labels are a navigation lexicon: a reader uses them to find the same
	/// information from one sub-game to the next. They must therefore be identical from card to
	/// card WITHIN a language. They are not, today, in five of the eight languages — the same
	/// section is called "Components" on cards 07/13 and "Material" on 09/11 in English, and
	/// ar / fa / zh carry up to three competing terms for a single section.
	///
	/// This is a CONTENT defect (the CSV), not a rendering one: the CSS is identical for the four
	/// cards. It is distinct from the heading-LEVEL defect of #1190 (a card title assigned by
	/// markdown position rather than by meaning), which lives in the template.
	///
	/// <para>
	/// Why an EXACT set and not a threshold: <see cref="KnownFloatingSections"/> pins the nine
	/// divergences measured on master <c>5022ad71</c>, and the assertion is set equality. The guard
	/// therefore goes red BOTH when a new divergence appears AND when a known one is repaired
	/// without updating the list. An "at most N" budget would silently absorb a new defect as an
	/// old one is fixed; set equality cannot. As #1199 is corrected, entries are removed one by
	/// one — the empty set is the end state, and it is the end state that must be reached, not a
	/// number that must be kept low.
	/// </para>
	/// </summary>
	public class RulesSectionLexiconTests
	{
		private const string RulesCsvRelPath = "Cards/Rules/Argumentum Rules - Cards.csv";

		/// <summary>Primary-key column of the Rules CSV.</summary>
		private const string PkColumn = "pk";

		/// <summary>
		/// The four structurally parallel sub-game rule cards. They are the ONLY cards that repeat
		/// the same three sections, which is what makes their lexicon comparable at all.
		/// </summary>
		private static readonly string[] ParallelVariantCards =
			{ "Rules_07", "Rules_09", "Rules_11", "Rules_13" };

		/// <summary>The 8 Rules language columns (<c>Text</c> is FR).</summary>
		private static readonly string[] RulesLanguageColumns =
			{ "Text", "Text_en", "Text_ru", "Text_pt", "Text_es", "Text_ar", "Text_fa", "Text_zh" };

		/// <summary>
		/// Each parallel card carries exactly 4 <c>h2</c>: index 0 is the sub-game name (varies by
		/// design), indices 1..3 are the three recurring sections compared here.
		/// </summary>
		private const int ExpectedH2CountPerVariantCard = 4;

		/// <summary>ASCII keys for the three recurring sections (h2 index 1, 2, 3).</summary>
		private static readonly string[] RecurringSectionKeys = { "S2", "S3", "S4" };

		/// <summary>
		/// The exact set of (language column, section) pairs whose label floats across the four
		/// parallel cards, measured on master <c>5022ad71</c> — nine originally, tracked by #1199.
		/// fr / pt / es are clean and MUST stay absent from this set. en/S2, en/S3, ru/S3 and
		/// fa/S3 were repaired (2026-08-27) and removed; the remaining five need native-speaker
		/// arbitration (ar/fa/zh section vocabulary), gated on the #988 instrument.
		/// </summary>
		private static readonly string[] KnownFloatingSections =
		{
			"Text_ar/S2",   // 3 terms
			"Text_fa/S2",   // 3 terms
			"Text_fa/S4",   // 3 terms
			"Text_zh/S3",   // 2 terms
			"Text_zh/S4",   // 2 terms
		};

		/// <summary>
		/// Matches an ATX <c>h2</c> line. The negative lookahead excludes <c>h3</c>: in this deck a
		/// level-3 heading is a numbered step, never a section (#1190), and counting one as a
		/// section would shift every index below.
		/// </summary>
		private static readonly Regex H2Line =
			new Regex(@"^[^\S\n]*##(?!#)[^\S\n]*(?<text>.+?)[^\S\n]*$",
				RegexOptions.Multiline | RegexOptions.Compiled);

		/// <summary>Extracts the <c>h2</c> heading texts of one markdown cell, in document order.</summary>
		internal static List<string> ExtractH2Headings(string markdown)
		{
			if (string.IsNullOrEmpty(markdown)) return new List<string>();
			return H2Line.Matches(markdown.Replace("\r", string.Empty))
				.Select(m => m.Groups["text"].Value.Trim())
				.ToList();
		}

		/// <summary>
		/// Normalizes a heading for comparison: whitespace collapsed, case folded. Deliberately
		/// does NOT strip punctuation or diacritics — in fa, a missing ezafe IS one of the
		/// divergences we want to see, and folding it away would hide the defect.
		/// </summary>
		internal static string NormalizeLabel(string label) =>
			string.Join(" ", (label ?? string.Empty)
					.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
				.ToLowerInvariant();

		/// <summary>
		/// Reads the four parallel cards' language cells, keyed by (pk, column). Access is by
		/// header index to stay robust to header-matching configuration.
		/// </summary>
		private static Dictionary<(string Pk, string Column), string> ReadParallelCardCells(string csvPath)
		{
			var cells = new Dictionary<(string, string), string>();
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};
			using var reader = new StreamReader(csvPath);
			using var csv = new CsvReader(reader, config);
			csv.Read();
			csv.ReadHeader();
			var headers = csv.HeaderRecord ?? Array.Empty<string>();

			int pkIndex = Array.IndexOf(headers, PkColumn);
			pkIndex.Should().BeGreaterThanOrEqualTo(0,
				"the Rules CSV must expose a '{0}' column", PkColumn);

			var columnIndex = new Dictionary<string, int>();
			foreach (var col in RulesLanguageColumns)
			{
				int idx = Array.IndexOf(headers, col);
				if (idx >= 0) columnIndex[col] = idx;
			}

			// A header that silently lost a language column would make this guard green for the
			// wrong reason: fewer columns compared means fewer divergences found (anti-#909).
			columnIndex.Should().HaveCount(RulesLanguageColumns.Length,
				"the Rules CSV must expose all 8 language columns: {0}",
				string.Join(", ", RulesLanguageColumns));

			while (csv.Read())
			{
				string pk = csv.GetField(pkIndex) ?? string.Empty;
				if (!ParallelVariantCards.Contains(pk)) continue;
				foreach (var (col, idx) in columnIndex)
					cells[(pk, col)] = csv.GetField(idx) ?? string.Empty;
			}
			return cells;
		}

		[Fact]
		public void Recurring_Section_Labels_Are_Stable_Across_The_Four_Parallel_Rule_Cards()
		{
			var repoRoot = TestRepoRoot.Find();
			var csvPath = Path.Combine(repoRoot, RulesCsvRelPath);
			// Fail loudly rather than skip: a missing CSV must never read as "no divergence".
			File.Exists(csvPath).Should().BeTrue("the Rules CSV must exist at {0}", RulesCsvRelPath);

			var cells = ReadParallelCardCells(csvPath);

			// Structural precondition, asserted BEFORE the comparison. If a card lost a section the
			// comparison below would run on fewer labels and could pass while the deck is broken —
			// the vacuous-green failure mode.
			foreach (var pk in ParallelVariantCards)
			{
				foreach (var col in RulesLanguageColumns)
				{
					cells.Should().ContainKey((pk, col), "card {0} must carry a '{1}' cell", pk, col);
					ExtractH2Headings(cells[(pk, col)]).Should().HaveCount(ExpectedH2CountPerVariantCard,
						"card {0} column '{1}' must keep its 4 h2 headings (sub-game name + the 3 " +
						"recurring sections); a different count means the deck was restructured and " +
						"this guard's parallelism assumption must be revisited (#1199)", pk, col);
				}
			}

			var floating = new List<string>();
			var detail = new List<string>();
			foreach (var col in RulesLanguageColumns)
			{
				for (int section = 0; section < RecurringSectionKeys.Length; section++)
				{
					int h2Index = section + 1;   // index 0 is the sub-game name
					var labels = ParallelVariantCards
						.Select(pk => ExtractH2Headings(cells[(pk, col)])[h2Index])
						.ToList();
					var distinct = labels.Select(NormalizeLabel)
						.Distinct(StringComparer.Ordinal)
						.ToList();
					if (distinct.Count > 1)
					{
						string key = $"{col}/{RecurringSectionKeys[section]}";
						floating.Add(key);
						detail.Add($"{key}: {distinct.Count} labels -> " + string.Join(" | ",
							ParallelVariantCards.Zip(labels, (pk, l) => $"{pk}={l}")));
					}
				}
			}

			// Set equality, NOT a budget: red on a new divergence AND red on a repaired one whose
			// entry was not removed. That is what stops the pinned list rotting into a stale alibi.
			floating.Should().BeEquivalentTo(KnownFloatingSections,
				"the recurring section lexicon of the 4 parallel Rules cards must match the exact " +
				"set of divergences tracked by #1199. If you REPAIRED one, remove its entry from " +
				"KnownFloatingSections (the empty set is the end state). If a NEW one appeared, a " +
				"section label drifted between the four sub-game cards and readers lose the " +
				"navigation lexicon that lets them find the same information from one sub-game to " +
				"the next.\nMeasured:\n{0}",
				detail.Count == 0 ? "  (none)" : string.Join("\n", detail.Select(d => "  " + d)));
		}

		[Fact]
		public void Guard_Sees_A_Drifting_Label_And_Ignores_Cosmetic_Variation()
		{
			// Inverse control: the comparison must be ABLE to go red. A detector that always
			// answered "all identical" would keep this guard green forever while the lexicon drifts
			// — the no-op failure mode that a green test cannot distinguish from a healthy deck.
			var uniform = new[] { "Components", "components", "  COMPONENTS  ", "Components" }
				.Select(NormalizeLabel).Distinct(StringComparer.Ordinal).ToList();
			uniform.Should().HaveCount(1, "case and surrounding whitespace are not drift");

			var drifting = new[] { "Components", "Material", "Components", "Material" }
				.Select(NormalizeLabel).Distinct(StringComparer.Ordinal).ToList();
			drifting.Should().HaveCount(2, "two different words for one section IS drift");
		}

		[Theory]
		[InlineData("## Materiel\n\ntext\n\n## Resume du jeu", 2)]
		[InlineData("## Only one", 1)]
		[InlineData("### 1. Le piocheur\n### 2. Le baratineur", 0)]   // h3 is a step, never a section
		[InlineData("# Cover title", 0)]                              // h1 is not a section either
		[InlineData("", 0)]
		public void ExtractH2Headings_Counts_Only_Level_Two(string markdown, int expected)
		{
			ExtractH2Headings(markdown).Should().HaveCount(expected);
		}
	}
}
