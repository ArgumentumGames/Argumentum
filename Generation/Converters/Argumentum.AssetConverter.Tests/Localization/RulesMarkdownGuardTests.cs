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
	/// The guard iterates over BOTH Rules DataSets — the main CSV (120 cells) AND the Print &amp;
	/// Play CSV (48 cells) — because both feed the same <c>{{markdown}}</c> helper. The markdown
	/// surface is bounded to these 168 cells: <c>{{markdown}}</c> is used only by Rules templates.
	///
	/// Compatible with the pre-tag freeze: touches neither CardPen nor any CSV — only the test
	/// project. Validated empirically at 0 forbidden construction across 168 cells (15+6 rows × 8
	/// languages) on master <c>d10952f4</c>.
	/// </summary>
	public class RulesMarkdownGuardTests
	{
		// The two Rules DataSets (AssetConverterConfig.cs:50-59 — KnownDataSets.Rules and
		// KnownDataSets.RulesPrintAndPlay). BOTH feed the {{markdown}} helper, therefore the same
		// vendored marked.js 0.3.x. A table written into the PnP CSV would render noop at the same
		// rate as in the main CSV, and a guard over only one would stay green — exactly the #965
		// decision expiring in silence. The markdown surface is BOUNDED to these 168 cells:
		// {{markdown}} is used only by Rules templates (ai-01 grep, cycle 64 — 2 live + 5 archived
		// under Cards/Rules/Archive/); no Fallacies/Virtues/Scenarii/Memo template touches it.
		//   - main: 15 rows × 8 langs = 120 cells
		//   - PnP:   6 rows × 8 langs =  48 cells
		private static readonly (string Name, string RelPath)[] RulesCsvs =
		{
			("Argumentum Rules - Cards",                "Cards/Rules/Argumentum Rules - Cards.csv"),
			("Argumentum Rules - Cards Print and Play", "Cards/Rules/Argumentum Rules - Cards Print and Play.csv"),
		};

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
		// 0.3.x. Validated empirically at 0 occurrence on master d10952f4 across BOTH Rules DataSets
		// (168 cells: 120 main + 48 P&P). See #965. Patterns are line-aware (Multiline) where the
		// construction is block-level.
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

		private static string FindRepoRoot()
		{
			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Cards", "Fallacies")))
			{
				dir = dir.Parent;
			}
			return dir?.FullName
				?? throw new DirectoryNotFoundException("Could not locate repository root (Cards/Fallacies not found).");
		}

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
		/// The 8-column assertion is carried PER CSV — a DataSet whose header silently dropped a
		/// language column would otherwise make the guard green for the wrong reason (anti-#909).
		/// </summary>
		private static List<(long Row, string Column, string Content)> ReadRulesLanguageCells(string csvPath, string csvName)
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
					"the Rules CSV '{0}' must expose all 8 language columns: {1}", csvName, string.Join(", ", RulesLanguageColumns));

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
		public void Rules_Csvs_Use_Only_Validated_Markdown_Constructions_Across_Both_Datasets()
		{
			// The #965 stay decision is self-defending through this test. If it ever fails, do NOT
			// silence it: either bring the content back into the validated domain, or reopen #965.
			// Both Rules DataSets feed the {{markdown}} helper (same marked.js 0.3.x), so BOTH must
			// stay within the validated domain — a guard over only one would expire in silence.
			var repoRoot = FindRepoRoot();
			var offenders = new List<string>();

			foreach (var (name, relPath) in RulesCsvs)
			{
				var path = Path.Combine(repoRoot, relPath);
				File.Exists(path).Should().BeTrue($"the Rules DataSet CSV '{name}' must exist at {relPath}");

				var cells = ReadRulesLanguageCells(path, name);
				cells.Should().NotBeEmpty(
					"the Rules CSV '{0}' must carry localized body text in all 8 languages", name);

				foreach (var (row, column, content) in cells)
				{
					var hits = FindForbiddenMarkdown(content);
					if (hits.Count > 0)
						offenders.Add($"[{name}] CSV row {row}, column '{column}': {string.Join(", ", hits)}");
				}
			}

			offenders.Should().BeEmpty(
				"Rules content must stay within the 3 validated markdown constructions (#965 verdict), " +
				$"across BOTH Rules DataSets (168 cells). Found {offenders.Count} offender(s):\n" +
				$"{string.Join("\n", offenders)}\n\n{Verdict965Reference}");
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
