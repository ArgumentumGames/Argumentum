using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.CardPenConfiguration
{
	/// <summary>
	/// Contract: a card template that sweeps an RTL/CJK language scope with a universal
	/// descendant selector (`.argu-lang-{ar,fa,zh} *`) and an `!important` font-family MUST
	/// also carry a counter-rule that re-asserts the latin brand font on the brand mark inside
	/// that scope. The sweep is what makes Arabic/Persian/Chinese glyphs render on the local
	/// text of an ar/fa/zh card; the same sweep is what silently clobbers the latin brand text
	/// (the ARGUMENTUM word-mark, the deck name, the tagline) with an Arabic/CJK body font, which
	/// is invisible in Debug (only the local-language channels are normally viewed) and surfaces
	/// only in the print channels that ship.
	///
	/// <para><b>Why this exists.</b> Found 2026-08-29 as #1225, a print blocker. The
	/// `.argu-lang-{ar,fa,zh} *` rule overrides `font-family` on every descendant, so the latin
	/// brand text inside an ar/fa/zh card falls back to Vazirmatn / Noto Sans SC. Two visible
	/// consequences:
	/// <list type="bullet">
	///   <item>On the shared Fallacies back (×176 per box) the ARGUMENTUM word-mark — an SVG
	///   `<text>` `AR`/`GU`/`MEN`/`TUM` — renders in an Arabic body font.</item>
	///   <item>On the Rules cover the big «Argumentum» masthead (TrendSlab) renders in an Arabic/
	///   CJK font and breaks mid-word: «Argumen/tum» (ar/fa), «Argume/ntum» (zh).</item>
	/// </list>
	/// Nothing in the pipeline reports this — the harvest succeeds and the wrong typeface reaches
	/// the printer. The fix is a <i>second</i> rule, scoped under the same `.argu-lang-{ar,fa,zh}`
	/// and carrying the same `!important`, that re-asserts the brand font on the brand container.
	/// Because both rules carry `!important`, the more specific counter-rule wins; and because the
	/// counter-rule targets the brand selector — not `*` — it never touches the local glyph text,
	/// so the inverse control (ar/fa/zh keep Vazirmatn / Noto Sans SC on local text) holds.</para>
	///
	/// <para><b>Scope is derived, not tabulated.</b> The template list is read from
	/// <see cref="WebBasedGeneratorConfig"/> exactly like <see cref="InsecureSubResourceContractTests"/>;
	/// adding a CardSet automatically extends the guard.</para>
	/// </summary>
	public class BrandFontCounterRuleContractTests
	{
		/// <summary>
		/// Latin brand fonts a card may legitimately want on its brand mark. DINPro is also the
		/// base card font, but on a brand container it is a deliberate re-assertion, not a sweep.
		/// The two fonts that are NOT allowed as the counter-rule here are the RTL/CJK glyph fonts
		/// — a counter-rule that re-declares Vazirmatn would be the pendulum, not the fix.
		/// </summary>
		private static readonly Regex BrandFont = new Regex(
			@"'?(?:Bebas Neue|TrendSlabW00-Four|Oswald|Dosis|Gadugi|DINPro)'?",
			RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex LocalGlyphFont = new Regex(
			@"'?(?:Vazirmatn|Noto Naskh Arabic|Noto Sans SC)'?",
			RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// The universal descendant sweep: `.argu-lang-ar *`, `.argu-lang-fa *`, `.argu-lang-zh *`.
		/// A scope is "swept" when a rule exists whose selector is <c>.argu-lang-&lt;scope&gt; *</c>
		/// and whose body forces a font with <c>!important</c>.
		/// </summary>
		private static readonly Regex SweptScope = new Regex(
			@"(?:^|[\s,}])\.argu-lang-(ar|fa|zh)\s*\*",
			RegexOptions.Compiled);

		/// <summary>
		/// Detects a counter-rule: a rule selector that carries an <c>.argu-lang-&lt;scope&gt;</c>
		/// scope <b>and</b> a body that forces a latin brand font with <c>!important</c> — while NOT
		/// naming a local glyph font. The `*` in the selector is rejected separately so a counter-rule
		/// cannot simply mirror the sweep.
		/// </summary>
		private static readonly Regex RuleBlock = new Regex(
			@"([^{}]+)\{([^}]*)\}",
			RegexOptions.Compiled);

		private static IEnumerable<(string Label, string RelPath)> DeclaredTemplates()
		{
			var config = new WebBasedGeneratorConfig();

			foreach (var cardSet in config.CardSets)
			{
				var sides = new[]
				{
					("face", cardSet.FaceCardSetInfo),
					("back", cardSet.BackCardSetInfo),
				};

				foreach (var (side, info) in sides)
				{
					if (info == null)
					{
						continue;
					}

					foreach (var declared in new[] { info.JsonFilePathDebug, info.JsonFilePathRelease })
					{
						var rel = ToRepoRelative(declared);
						if (rel != null)
						{
							yield return ($"{cardSet.Name}/{side}", rel);
						}
					}
				}
			}
		}

		private static string? ToRepoRelative(string? declared)
		{
			if (string.IsNullOrWhiteSpace(declared))
			{
				return null;
			}

			var normalised = declared.Replace('\\', '/');
			var idx = normalised.IndexOf("Cards/", StringComparison.OrdinalIgnoreCase);
			return idx < 0 ? null : normalised.Substring(idx);
		}

		public static IEnumerable<object[]> ActiveTemplates() =>
			DeclaredTemplates()
				.GroupBy(t => t.RelPath, StringComparer.OrdinalIgnoreCase)
				.OrderBy(g => g.Key, StringComparer.Ordinal)
				.Select(g => new object[] { g.Key, string.Join(", ", g.Select(t => t.Label).Distinct()) });

		/// <summary>The templates that carry a swept scope <b>and</b> actually hold latin brand text.</summary>
		private static List<(string RelPath, string UsedBy)> BrandTemplates()
		{
			var result = new List<(string, string)>();
			foreach (var obj in ActiveTemplates())
			{
				var relPath = (string)obj[0];
				var usedBy = (string)obj[1];
				var path = Path.Combine(TestRepoRoot.Find(), relPath);
				if (!File.Exists(path))
				{
					continue;
				}

				try
				{
					using var doc = JsonDocument.Parse(File.ReadAllText(path));
					var css = doc.RootElement.GetProperty("css").GetString() ?? "";
					var mustache = doc.RootElement.GetProperty("mustache").GetString() ?? "";

					if (!HasSweptScope(css))
					{
						continue;
					}

					if (HasLatinBrandText(mustache) || HasTrendSlab(css))
					{
						result.Add((relPath, usedBy));
					}
				}
				catch (JsonException)
				{
					// A template that is not parseable as JSON (e.g. the Virtues face carries a literal
					// newline) is skipped by the guard — it is not a brand-bearing template to fix.
				}
			}

			return result;
		}

		private static bool HasSweptScope(string css) => SweptScope.IsMatch(css);

		/// <summary>
		/// Latin brand text hardcoded in the mustache HTML, outside the debug <c>cardName</c>
		/// container. The cardName debug label (<c>Argumentum_Fallacies_{{path}}..{{text}}</c>,
		/// <c>Memo_face</c>, …) is stripped first — it is the one place all the Face templates
		/// carry literal latin text, so without the strip every Face would masquerade as brand.
		/// The <c>Argumentum_&lt;Deck&gt;</c> prefix is stripped too, as it only ever appears in
		/// that debug label. What remains and is counted is the visible brand mark: the
		/// ARGUMENTUM word-mark (or its SVG letters), the deck name, «MEMO» spelled letter by
		/// letter. A count of ≥ 4 latin letters is the threshold — the word-mark letters
		/// (<c>AR</c>/<c>GU</c>/<c>MEN</c>/<c>TUM</c>) and the single-letter «MEMO» boxes both
		/// clear it, while a template that only ships localized <c>{{…}}</c> content collapses to
		/// whitespace and does not.
		/// </summary>
		private static bool HasLatinBrandText(string mustache)
		{
			var withoutCardName = Regex.Replace(
				mustache,
				@"<div[^>]*class=['""][^'""]*cardName[^'""]*['""][^>]*>.*?</div>",
				"",
				RegexOptions.IgnoreCase | RegexOptions.Singleline);
			var withoutDebug = Regex.Replace(withoutCardName, @"Argumentum_[A-Za-z0-9_]+", "");
			var withoutVars = Regex.Replace(withoutDebug, @"\{\{.*?\}\}", "");
			var text = Regex.Replace(withoutVars, @"<[^>]*>", " ");
			return Regex.Matches(text, "[A-Za-z]").Count >= 4;
		}

		private static bool HasTrendSlab(string css) =>
			Regex.IsMatch(css, @"TrendSlab", RegexOptions.IgnoreCase);

		/// <summary>
		/// For a given scope (ar/fa/zh), is there a counter-rule re-asserting a latin brand font on
		/// a non-universal selector within that scope?
		/// </summary>
		private static bool HasCounterRule(string css, string scope)
		{
			foreach (Match m in RuleBlock.Matches(css))
			{
				var selector = m.Groups[1].Value;
				var body = m.Groups[2].Value;

				if (!Regex.IsMatch(selector, @"\.argu-lang-" + scope + @"\b", RegexOptions.IgnoreCase))
				{
					continue;
				}

				// The counter must target a real element, not the universal sweep (which would be a
				// no-op mirror or the defect itself).
				if (Regex.IsMatch(selector, @"\.argu-lang-" + scope + @"\s*\*"))
				{
					continue;
				}

				if (!body.Contains("font-family", StringComparison.OrdinalIgnoreCase)
					|| !body.Contains("!important", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				if (LocalGlyphFont.IsMatch(body))
				{
					continue;
				}

				if (BrandFont.IsMatch(body))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Inverse control: the local glyph font must still be forced on the swept scope. This is
		/// the guard against the pendulum — a "fix" that re-asserts the brand font by removing the
		/// sweep would break the ar/fa/zh glyphs, which are exactly what the sweep exists to fix.
		/// </summary>
		private static bool HasLocalGlyphSweep(string css, string scope)
		{
			foreach (Match m in RuleBlock.Matches(css))
			{
				var selector = m.Groups[1].Value;
				var body = m.Groups[2].Value;

				if (!Regex.IsMatch(selector, @"\.argu-lang-" + scope + @"\s*\*", RegexOptions.IgnoreCase))
				{
					continue;
				}

				if (body.Contains("!important", StringComparison.OrdinalIgnoreCase)
					&& LocalGlyphFont.IsMatch(body))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Anti-no-op. The trigger set is asserted <em>before</em> anything is asserted about it, so a
		/// change to the derivation (CardSets renamed, JsonFilePath* moved) fails loudly instead of
		/// silently reporting green on zero files.
		/// </summary>
		[Fact]
		public void BrandTemplates_IsDerivedFromConfigAndNonEmpty()
		{
			var templates = BrandTemplates();

			templates.Should().NotBeEmpty(
				"the guard reads its scope from WebBasedGeneratorConfig.CardSets. An empty scope means " +
				"the config shape changed and the Theory below has silently become a no-op.");

			templates.Count.Should().BeGreaterThanOrEqualTo(4,
				"four shipped templates carry latin brand text (or a TrendSlab masthead) under a swept " +
				"RTL/CJK scope: the Fallacies back, the Memo back, the Memo front («MEMO») and the Rules " +
				"masthead. A sudden drop below 4 means the brand-text detection broke rather than that " +
				"brand mark was removed.");
		}

		[Theory]
		[MemberData(nameof(ActiveTemplates))]
		public void SweptTemplate_RestoresLatinBrandFontUnderAllScopes(string relPath, string usedBy)
		{
			var path = Path.Combine(TestRepoRoot.Find(), relPath);
			File.Exists(path).Should().BeTrue(
				"{0} (used by {1}) is declared in WebBasedGeneratorConfig but is not on disk.",
				relPath, usedBy);

			string css;
			string mustache;
			try
			{
				using var doc = JsonDocument.Parse(File.ReadAllText(path));
				css = doc.RootElement.GetProperty("css").GetString() ?? "";
				mustache = doc.RootElement.GetProperty("mustache").GetString() ?? "";
			}
			catch (JsonException)
			{
				// A template whose JSON is not parseable as-is cannot be linted for a CSS rule; skip.
				return;
			}

			if (!HasSweptScope(css))
			{
				return;
			}

			if (!HasLatinBrandText(mustache) && !HasTrendSlab(css))
			{
				return;
			}

			var scopes = new[] { "ar", "fa", "zh" };
			foreach (var scope in scopes)
			{
				HasCounterRule(css, scope).Should().BeTrue(
					"{0} (used by {1}) sweeps the `.argu-lang-{2} *` scope at `!important`, which clobbers " +
					"the latin brand mark; as of #1225 the scope MUST carry a counter-rule re-asserting a " +
					"latin brand font (Bebas Neue / TrendSlab / DINPro / …) on the brand container so the " +
					"mark keeps its typeface. Absent that rule, the brand text falls back to Vazirmatn / " +
					"Noto Sans SC and the mark renders in an Arabic/CJK body font — for the shared Fallacies " +
					"back and the Rules «Argumentum» masthead this breaks mid-word.",
					relPath, usedBy, scope);

				HasLocalGlyphSweep(css, scope).Should().BeTrue(
					"inverse control (#1225): {0} (used by {1}) must KEEP forcing Vazirmatn / Noto Sans SC " +
					"at `!important` on the `.argu-lang-{2} *` scope. Re-asserting the brand font must not " +
					"come at the cost of the ar/fa/zh glyphs — that is the pendulum the fix must avoid.",
					relPath, usedBy, scope);
			}
		}
	}
}
