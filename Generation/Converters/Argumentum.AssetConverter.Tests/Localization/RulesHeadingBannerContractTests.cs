using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Contract guard for the Rules heading CSS (#1190 D1, owner decision 2026-08-27 — partial
	/// reversal of #438). The deck's heading hierarchy is carried by ONE template CSS, and two
	/// defects are known to have lived in it:
	/// <list type="number">
	/// <item>POSITIONAL PROMOTION: <c>card:not([class~="1"]) h2:first-child</c> styled a card
	/// title by markdown POSITION (is anything before it?) instead of by meaning — the same word
	/// ("Installation"/"SETUP") was a big card title on Rules_03 and a small underlined section
	/// on Rules_07/09/11/13.</item>
	/// <item>THE THIN HAIRLINE: section <c>h2</c>s rendered as plain text with a 0.12em
	/// underline while <c>h3</c> steps had colored banners — the inconsistency the owner asked
	/// to remove ("garder le bandeau et l'étendre", two weights: section banner larger than
	/// step banner).</item>
	/// </list>
	///
	/// The guard parses EVERY rule of the template CSS (the #1138 lesson: a passing substring
	/// fragment proves nothing about the rest of the file) and checks the contract, not a
	/// byte-string:
	/// <list type="bullet">
	/// <item>no selector promotes <c>h2:first-child</c>/<c>:first-of-type</c> unless it carries
	/// a <c>:has(</c> structural guard;</item>
	/// <item>the semantic promotion (<c>:has(h2 ~ h2 ~ h2 ~ h2)</c> — a card whose 4th h2
	/// exists; measured corpus fact: only the 4 sub-game cards have ≥4 h2) must EXIST, else the
	/// variant cards silently lost their titles;</item>
	/// <item>the generic section <c>h2</c> rule must be a BANNER (non-transparent background),
	/// never a plain-text hairline;</item>
	/// <item>two weights: section banner font strictly larger than step banner font;</item>
	/// <item>the 5 sub-game color groups must keep card-level <c>--color-group-bg</c> /
	/// <c>--color-group-box</c> fallbacks — a banner falling back to default gray is a silent
	/// regression on exactly the cards nobody re-checks.</item>
	/// </list>
	///
	/// Seen RED before being proposed: restoring the pre-fix CSS (positional selector,
	/// transparent hairline, single weight) fails guards 1, 3 and 4; deleting the group
	/// fallbacks fails guard 5. An organ never seen red is not an organ (#1046).
	/// </summary>
	public class RulesHeadingBannerContractTests
	{
		private const string RulesTemplateRelPath = "Cards/Rules/Argumentum_Rules_fr.json";

		private sealed record CssRule(string Selector, IReadOnlyDictionary<string, string> Declarations);

		/// <summary>Splits the stylesheet into top-level rules, stripping comments. Flat parser:
		/// this template has no @media nesting in its heading system; any future @ rule is
		/// skipped rather than mis-parsed (an unknown construct must not become a false green).</summary>
		private static List<CssRule> ParseRules(string css)
		{
			var noComments = Regex.Replace(css, @"/\*.*?\*/", string.Empty, RegexOptions.Singleline);
			var rules = new List<CssRule>();
			int i = 0;
			while (i < noComments.Length)
			{
				int open = noComments.IndexOf('{', i);
				if (open < 0) break;
				int close = noComments.IndexOf('}', open);
				if (close < 0) break;
				string selector = noComments[i..open].Trim();
				string body = noComments[(open + 1)..close];
				i = close + 1;
				if (selector.Length == 0 || selector.StartsWith("@")) continue;
				var declarations = body.Split(';', StringSplitOptions.RemoveEmptyEntries)
					.Select(d => d.Split(':', 2))
					.Where(kv => kv.Length == 2)
					.GroupBy(kv => kv[0].Trim(), StringComparer.OrdinalIgnoreCase)
					.ToDictionary(g => g.Key, g => g.Last().Length > 1 ? g.Last()[1].Trim() : string.Empty,
						StringComparer.OrdinalIgnoreCase);
				rules.Add(new CssRule(selector, declarations));
			}
			return rules;
		}

		private static string LoadTemplateCss()
		{
			var repoRoot = TestRepoRoot.Find();
			var path = Path.Combine(repoRoot, RulesTemplateRelPath);
			// Fail loudly rather than skip: a missing template must never read as "contract held".
			File.Exists(path).Should().BeTrue("the Rules template must exist at {0}", RulesTemplateRelPath);
			using var doc = JsonDocument.Parse(File.ReadAllText(path));
			return doc.RootElement.GetProperty("css").GetString()
				?? throw new InvalidDataException("the Rules template carries an empty 'css' key");
		}

		/// <summary>A selector entry (comma-separated) that styles an h2 by bare position —
		/// :first-child/:first-of-type/:nth-child(1) — with no :has( structural guard.</summary>
		private static bool IsBarePositionalH2Promotion(string selectorEntry)
		{
			if (!selectorEntry.Contains("h2", StringComparison.OrdinalIgnoreCase)) return false;
			if (selectorEntry.Contains(":has(", StringComparison.OrdinalIgnoreCase)) return false;
			return Regex.IsMatch(selectorEntry,
				@":first-child|:first-of-type|:nth-child\(\s*1\s*\)|:nth-of-type\(\s*1\s*\)",
				RegexOptions.IgnoreCase);
		}

		[Fact]
		public void No_Heading_Is_Promoted_By_Bare_Position()
		{
			var offenders = ParseRules(LoadTemplateCss())
				// A rule that only assigns CSS custom properties (the per-index COLOR groups,
				// e.g. `[class~="7"] h2:nth-of-type(1) { --color-background: ... }`) cannot style
				// or promote anything — positional COLOR assignment is legitimate and kept from
				// #438. Only a rule carrying real declarations (typography, layout, paint) on a
				// positionally-selected h2 can be a promotion.
				.Where(r => r.Declarations.Any(d => !d.Key.StartsWith("--")))
				.SelectMany(r => r.Selector.Split(',').Select(s => s.Trim()).Where(IsBarePositionalH2Promotion)
					.Select(s => s))
				.ToList();
			// The cover ([class~="1"]) selectors are the cover's OWN title system, not a promotion;
			// they are excluded the same way the historical rule excluded them.
			offenders = offenders.Where(s => !s.Contains("[class~=\"1\"]")).ToList();
			offenders.Should().BeEmpty(
				"an h2 styled by bare position (:first-child etc., no :has guard) is the #1190 D1 " +
				"defect: the same word becomes a card title or a section depending on what precedes " +
				"it in the markdown. Promotion must be structural (:has(h2 ~ h2 ~ h2 ~ h2)). " +
				"Offenders: {0}", string.Join(" | ", offenders));
		}

		[Fact]
		public void Semantic_SubGame_Promotion_Exists()
		{
			var css = LoadTemplateCss();
			ParseRules(css).Should().ContainSingle(
				r => r.Selector.Contains(":has(h2 ~ h2 ~ h2 ~ h2)", StringComparison.OrdinalIgnoreCase),
				"the four sub-game cards (the only ones with >= 4 h2 — measured corpus fact) get " +
				"their big title from this structural guard. If it is gone the variant cards lost " +
				"their names and no pixel test on the cover would notice.");
		}

		/// <summary>The generic heading rules are identified by their EXACT collapsed selector —
		/// an EndsWith heuristic would also match the promotion rule or per-index color rules.</summary>
		private static CssRule? RuleByCollapsedSelector(List<CssRule> rules, string collapsed) =>
			rules.SingleOrDefault(r => Regex.Replace(r.Selector, @"\s+", "").Equals(collapsed,
				StringComparison.OrdinalIgnoreCase));

		[Fact]
		public void Section_Headings_Are_Banners_Not_Hairlines()
		{
			var sectionRule = RuleByCollapsedSelector(ParseRules(LoadTemplateCss()),
				"card:not([class~=\"1\"])h2");
			sectionRule.Should().NotBeNull("the generic section h2 rule must exist");
			sectionRule!.Declarations.TryGetValue("background-color", out var bg).Should().BeTrue(
				"the section h2 rule must declare a background-color");
			bg!.Should().NotBe("transparent").And.NotBe("none",
				"a transparent section heading is the retired hairline style (#1190 D1): sections " +
				"must render as banners like their h3 siblings, two weights, one family");
		}

		[Fact]
		public void Section_Banner_Is_Strictly_Larger_Than_Step_Banner()
		{
			var rules = ParseRules(LoadTemplateCss());
			var h2Rule = RuleByCollapsedSelector(rules, "card:not([class~=\"1\"])h2");
			var h3Rule = RuleByCollapsedSelector(rules, "card:not([class~=\"1\"])h3");
			(h2Rule, h3Rule).Should().NotBe((null, null), "both generic heading rules must exist");
			h2Rule!.Declarations.TryGetValue("font-size", out var h2Size).Should().BeTrue();
			h3Rule!.Declarations.TryGetValue("font-size", out var h3Size).Should().BeTrue();
			decimal h2 = decimal.Parse(h2Size!.TrimEnd('m', 'e'), System.Globalization.CultureInfo.InvariantCulture);
			decimal h3 = decimal.Parse(h3Size!.TrimEnd('m', 'e'), System.Globalization.CultureInfo.InvariantCulture);
			h2.Should().BeGreaterThan(h3,
				"two weights, one family: the section banner (h2, {0}em) must be strictly larger " +
				"than the numbered-step banner (h3, {1}em) — equal sizes collapse the hierarchy the " +
				"owner asked to keep", h2, h3);
		}

		[Fact]
		public void All_Five_SubGame_Color_Groups_Keep_Their_Card_Level_Fallbacks()
		{
			var rules = ParseRules(LoadTemplateCss());
			var covered = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (var rule in rules)
			{
				if (!rule.Declarations.ContainsKey("--color-group-bg")) continue;
				if (!rule.Declarations.ContainsKey("--color-group-box")) continue;
				foreach (Match m in Regex.Matches(rule.Selector, @"\[class~=""(\d+)""\]"))
					covered.Add(m.Groups[1].Value);
			}
			var missing = Enumerable.Range(2, 14).Select(i => i.ToString())
				.Where(c => !covered.Contains(c)).ToList();
			missing.Should().BeEmpty(
				"every Rules card class 2..15 must fall under a card-level --color-group-bg/box " +
				"definition; class(es) {0} would render their banners in default gray — the silent " +
				"color regression the #1190 D1 DoD names explicitly",
				string.Join(",", missing));
		}

		/// <summary>
		/// The CORPUS PREMISE of the semantic promotion, asserted instead of merely asserted-in-a-comment.
		///
		/// <para>
		/// <c>card:has(h2 ~ h2 ~ h2 ~ h2) h2:first-child</c> promotes by STRUCTURE: any card whose 4th
		/// <c>h2</c> exists gets a big card title. That is only the right rendering while "4 or more h2"
		/// and "is a sub-game card" name the same four cards. Today they do — measured, all 8 languages.
		/// Nothing in the CSS can keep them aligned, because the alignment lives in the CSV.
		/// </para>
		///
		/// <para>
		/// The margin is one section wide: <c>Rules_08</c> and <c>Rules_12</c> carry 3 h2 each. Adding a
		/// fourth section to either — an ordinary editorial act, in any one of eight language columns —
		/// would silently give it a promoted card title, which is precisely the #438 defect walking back
		/// in through the content door after #1190 D1 shut the CSS one. Symmetrically, a sub-game card
		/// that LOSES a section silently loses its title. Neither shows up in a CSS contract test, and
		/// neither shows up in a diff review of the CSV.
		/// </para>
		///
		/// <para>
		/// Set equality PER LANGUAGE, not on the union: a promotion that appears in <c>zh</c> only would
		/// be invisible to a union check, and it is exactly the kind of defect nobody re-reads.
		/// </para>
		/// </summary>
		[Fact]
		public void Only_The_Four_SubGame_Cards_Reach_The_Promotion_Threshold_In_Every_Language()
		{
			const int PromotionThreshold = 4;   // the 4 in :has(h2 ~ h2 ~ h2 ~ h2)
			var expected = new[] { "Rules_07", "Rules_09", "Rules_11", "Rules_13" };

			var repoRoot = TestRepoRoot.Find();
			var csvPath = Path.Combine(repoRoot, "Cards/Rules/Argumentum Rules - Cards.csv");
			// Fail loudly rather than skip: a missing CSV must never read as "premise holds".
			File.Exists(csvPath).Should().BeTrue("the Rules CSV must exist at {0}", csvPath);

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

			int pkIndex = Array.IndexOf(headers, "pk");
			pkIndex.Should().BeGreaterThanOrEqualTo(0, "the Rules CSV must expose a 'pk' column");

			var languageColumns = new[]
				{ "Text", "Text_en", "Text_ru", "Text_pt", "Text_es", "Text_ar", "Text_fa", "Text_zh" };
			var columnIndex = languageColumns
				.Select(c => (Column: c, Index: Array.IndexOf(headers, c)))
				.Where(t => t.Index >= 0)
				.ToDictionary(t => t.Column, t => t.Index);
			// A header that quietly lost a column would make this guard green for the wrong reason:
			// fewer languages scanned means fewer promotions found (anti-#909).
			columnIndex.Should().HaveCount(languageColumns.Length,
				"the Rules CSV must expose all 8 language columns: {0}", string.Join(", ", languageColumns));

			var h2CountByLanguage = new Dictionary<string, Dictionary<string, int>>();
			foreach (var col in languageColumns) h2CountByLanguage[col] = new Dictionary<string, int>();
			while (csv.Read())
			{
				string pk = csv.GetField(pkIndex) ?? string.Empty;
				if (pk.Length == 0) continue;
				foreach (var (col, idx) in columnIndex)
					h2CountByLanguage[col][pk] =
						RulesSectionLexiconTests.ExtractH2Headings(csv.GetField(idx) ?? string.Empty).Count;
			}

			// The corpus must not be empty — an empty read would satisfy every set comparison below.
			h2CountByLanguage["Text"].Should().NotBeEmpty("the Rules CSV must carry data rows");

			foreach (var col in languageColumns)
			{
				var counts = h2CountByLanguage[col];
				var promoted = counts.Where(kv => kv.Value >= PromotionThreshold)
					.Select(kv => kv.Key).OrderBy(k => k, StringComparer.Ordinal).ToList();
				int runnerUp = counts.Where(kv => !expected.Contains(kv.Key))
					.Select(kv => kv.Value).DefaultIfEmpty(0).Max();

				promoted.Should().BeEquivalentTo(expected,
					"in column '{0}', the cards reaching {1} h2 — the ones the CSS promotes to a card " +
					"title via :has(h2 ~ h2 ~ h2 ~ h2) — must be exactly the four sub-game cards. A card " +
					"that GAINED a section now shows a promoted title it was never meant to have (#438 " +
					"returning through the CSV instead of the CSS); a sub-game card that LOST one has " +
					"silently lost its title. Highest h2 count outside the four: {2} (threshold {1}). " +
					"Measured: {3}",
					col, PromotionThreshold, runnerUp,
					string.Join(", ", counts.OrderBy(kv => kv.Key, StringComparer.Ordinal)
						.Select(kv => $"{kv.Key}={kv.Value}")));
			}
		}

		[Fact]
		public void Parser_Sees_The_Defect_It_Guards_Against()
		{
			// Inverse control on the detector itself: the pre-fix selector and the hairline
			// declaration must trip the predicates, and the structural guard must not (#1046 —
			// a guard that cannot go red protects nothing).
			IsBarePositionalH2Promotion("card:not([class~=\"1\"]) h2:first-child").Should().BeTrue(
				"the historical positional rule is the defect");
			IsBarePositionalH2Promotion("card:has(h2 ~ h2 ~ h2 ~ h2) h2:first-child").Should().BeFalse(
				"the structural guard makes the positional part meaningful, not bare");
			IsBarePositionalH2Promotion("[class~=\"1\"] h2").Should().BeFalse(
				"a class selector with no positional pseudo-class is not a promotion");
		}
	}
}
