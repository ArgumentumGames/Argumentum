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
	/// Self-defending guard for the #965 stay decision (ai-01 verdict: remain on the vendored
	/// marked.js 0.3.x for the Rules renderer).
	///
	/// The stay decision is valid ONLY while Rules content stays within the three markdown
	/// constructions that 0.3.x renders correctly and that were measured at 100% of the living
	/// surface: ATX headings (<c>#</c>), unordered lists (<c>*</c>/<c>-</c>/<c>+</c>), and simple
	/// emphasis (<c>*…*</c>). The day a table, link, image, fenced code, inline HTML, blockquote,
	/// inline code, horizontal rule, bold, <c>_…_</c> emphasis, or ordered list is written into the
	/// Rules CSV, 0.3.x renders it wrong (<c>table: noop</c>, <c>fences: noop</c>) or injects it raw
	/// (the helper is a <c>SafeString</c> without <c>sanitize</c>) — silently, with no link back to
	/// this decision.
	///
	/// This guard fails the instant any of those constructions appears in any of the 8 Rules
	/// language columns (<c>Text</c>/<c>Text_en/_ru/_pt/_es/_ar/_fa/_zh</c>), and its failure
	/// message points back to the #965 verdict so the decision does not expire in silence.
	///
	/// Compatible with the pre-tag freeze: touches neither CardPen nor any CSV — only the test
	/// project. Validated empirically at 0 forbidden construction across 15 rows × 8 languages
	/// (120 cells) on master <c>c9415d6a</c>.
	/// </summary>
	public class RulesMarkdownGuardTests
	{
		private const string RulesCsvRelPath = "Cards/Rules/Argumentum Rules - Cards.csv";

		private const string Verdict965Reference =
			"This violates the #965 verdict (ai-01: STAY on vendored marked.js 0.3.x for the Rules " +
			"renderer). The stay is valid ONLY while Rules content stays within the three validated " +
			"constructions — ATX headings (#), unordered lists (* / - / +), and simple emphasis (*…*). " +
			"A table/link/image/fence/HTML/blockquote/code/rule/bold/_…_/ordered list would render wrong " +
			"(table/fences are noop on 0.3.x) or inject raw (SafeString without sanitize). Either rewrite " +
			"the content using only validated constructions, or reopen #965 to revisit the stay decision.";

		// The 8 Rules language columns (Text is FR; the other 7 are the release languages).
		private static readonly string[] RulesLanguageColumns =
			{ "Text", "Text_en", "Text_ru", "Text_pt", "Text_es", "Text_ar", "Text_fa", "Text_zh" };

		// Forbidden markdown constructions — each renders incorrectly or injects raw on marked.js
		// 0.3.x. Validated empirically at 0 occurrence on master c9415d6a (120 cells). See #965.
		// Patterns are line-aware (Multiline) where the construction is block-level.
		private static readonly (string Name, Regex Pattern)[] ForbiddenConstructions =
		{
			("fenced code block ```", new Regex(@"```", RegexOptions.Compiled)),
			("table row (>=2 pipes)", new Regex(@"^[^\S\n]*\|[^\n]*\|[^\n]*\|", RegexOptions.Multiline | RegexOptions.Compiled)),
			("table separator |---|", new Regex(@"^[^\S\n]*\|?[ \s:]*-{2,}[ \s:|\-]*\|", RegexOptions.Multiline | RegexOptions.Compiled)),
			("link [text](url)",      new Regex(@"\[[^\]]+\]\([^)]+\)", RegexOptions.Compiled)),
			("image ![alt](url)",     new Regex(@"!\[[^\]]*\]\([^)]+\)", RegexOptions.Compiled)),
			("inline HTML <tag>",     new Regex(@"</?[a-zA-Z][^>]*>", RegexOptions.Compiled)),
			("blockquote >",          new Regex(@"^[^\S\n]*>", RegexOptions.Multiline | RegexOptions.Compiled)),
			("inline code `x`",       new Regex(@"`[^`]+`", RegexOptions.Compiled)),
			("horizontal rule ---/***", new Regex(@"^[^\S\n]*(-{3,}|\*{3,}|_{3,})[^\S\n]*$", RegexOptions.Multiline | RegexOptions.Compiled)),
			("bold **…**",            new Regex(@"\*\*[^*]+\*\*", RegexOptions.Compiled)),
			("_…_ emphasis",          new Regex(@"(?<!\w)_[^_\n]+_(?!\w)", RegexOptions.Compiled)),
			("ordered list N.",       new Regex(@"^[^\S\n]*\d+\.[^\S\n]+", RegexOptions.Multiline | RegexOptions.Compiled)),
		};

		private static string FindRepoRoot() => TestRepoRoot.Find();

		/// <summary>
		/// Scans one cell; returns the names of the forbidden constructions it contains
		/// (empty = the cell stays within the validated domain).
		/// </summary>
		internal static List<string> FindForbiddenMarkdown(string content)
		{
			var hits = new List<string>();
			if (string.IsNullOrEmpty(content)) return hits;
			foreach (var (name, pattern) in ForbiddenConstructions)
			{
				if (pattern.IsMatch(content))
					hits.Add(name);
			}
			return hits;
		}

		/// <summary>
		/// Reads the 8 Rules language columns cell-by-cell from the CSV (CsvHelper handles the
		/// quoted multi-line cells). Returns a map of (row index, column name) -> cell text for
		/// non-blank cells. Access is by header index to stay robust to PrepareHeaderForMatch.
		/// </summary>
		private static List<(long Row, string Column, string Content)> ReadRulesLanguageCells(string csvPath)
		{
			var cells = new List<(long, string, string)>();
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};
			using (var reader = new StreamReader(csvPath))
			using (var csv = new CsvReader(reader, config))
			{
				csv.Read();
				csv.ReadHeader();
				var headers = csv.HeaderRecord ?? Array.Empty<string>();

				var columnIndex = new Dictionary<string, int>();
				foreach (var col in RulesLanguageColumns)
				{
					int idx = Array.IndexOf(headers, col);
					if (idx >= 0)
						columnIndex[col] = idx;
				}
				columnIndex.Should().HaveCount(RulesLanguageColumns.Length,
					"the Rules CSV must expose all 8 language columns: {0}", string.Join(", ", RulesLanguageColumns));

				while (csv.Read())
				{
					long row = csv.Parser.Row;
					foreach (var (col, idx) in columnIndex)
					{
						string cell = csv.GetField(idx) ?? string.Empty;
						if (!string.IsNullOrWhiteSpace(cell))
							cells.Add((row, col, cell));
					}
				}
			}
			return cells;
		}

		[Fact]
		public void Rules_Csv_Uses_Only_Validated_Markdown_Constructions_Across_8_Languages()
		{
			// The #965 stay decision is self-defending through this test. If it ever fails, do NOT
			// silence it: either bring the content back into the validated domain, or reopen #965.
			var path = Path.Combine(FindRepoRoot(), RulesCsvRelPath);
			File.Exists(path).Should().BeTrue($"Rules CSV must exist at {RulesCsvRelPath}");

			var cells = ReadRulesLanguageCells(path);
			cells.Should().NotBeEmpty("the Rules CSV must carry localized body text in all 8 languages");

			var offenders = new List<string>();
			foreach (var (row, column, content) in cells)
			{
				var hits = FindForbiddenMarkdown(content);
				if (hits.Count > 0)
					offenders.Add($"CSV row {row}, column '{column}': {string.Join(", ", hits)}");
			}

			offenders.Should().BeEmpty(
				"Rules content must stay within the 3 validated markdown constructions (#965 verdict). " +
				$"Found {offenders.Count} offender(s):\n{string.Join("\n", offenders)}\n\n{Verdict965Reference}");
		}

		[Theory]
		[InlineData("a **bold** word", "bold **…**")]
		[InlineData("see [site](https://x.com)", "link [text](url)")]
		[InlineData("<b>html</b>", "inline HTML <tag>")]
		[InlineData("| a | b |\n|---|---|\n| 1 | 2 |", "table row (>=2 pipes)")]
		[InlineData("> a quote", "blockquote >")]
		[InlineData("```code```", "fenced code block ```")]
		[InlineData("![img](x.png)", "image ![alt](url)")]
		[InlineData("an `inline` code span", "inline code `x`")]
		[InlineData("_em_", "_…_ emphasis")]
		[InlineData("1. first item", "ordered list N.")]
		[InlineData("---\n", "horizontal rule ---/***")]
		[InlineData("| a | b |", "table row (>=2 pipes)")]
		public void Guard_Detects_Forbidden_Construction(string content, string expectedConstructionName)
		{
			// DoD: the guard must go red the instant content leaves the validated domain.
			// Each forbidden construction is independently detectable.
			var hits = FindForbiddenMarkdown(content);
			hits.Should().Contain(expectedConstructionName,
				$"the guard must flag '{expectedConstructionName}' — a construction that breaks the #965 stay decision");
		}

		[Fact]
		public void Guard_Accepts_Validated_Constructions_And_Plain_Text()
		{
			// Sanity inverse: the three validated constructions (ATX headings, unordered lists,
			// simple *…* emphasis) plus plain text must NOT trigger the guard.
			var clean = string.Join("\n", new[]
			{
				"# Argumentum",
				"",
				"## L'école des menteurs",
				"",
				"*Règles du jeu : de 4 à 8 joueurs*",
				"",
				"* 1 paquet de cartes Fallacies",
				"* 5 cartes mémo",
				"",
				"- aussi avec des tirets",
				"",
				"Un paragraphe simple, sans markdown particulier, sur plusieurs mots.",
			});
			var hits = FindForbiddenMarkdown(clean);
			hits.Should().BeEmpty(
				"ATX headings, unordered lists (* / -), and simple *…* emphasis are the validated domain (#965); " +
				$"the guard flagged: {string.Join(", ", hits)}");
		}
	}
}
