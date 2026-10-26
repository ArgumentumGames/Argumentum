using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Contract for issue #458, grains "Scenarii role label touches its icon in RTL" and
	/// "Scenarii suggestion quote icon touches its text in RTL" (same physical-margin family).
	///
	/// <para><b>The defect.</b> <c>.roles</c> is a flex row holding two <c>[icon, label]</c> pairs.
	/// The gap between an icon and its label was declared as a <i>physical</i> right margin. A
	/// physical margin does not follow the writing direction: under <c>dir=rtl</c> the flex row
	/// reverses (label first, icon after) while the margin keeps pointing at the physical right,
	/// i.e. at the card's outer edge. The pair itself then carries a <b>0 px</b> gap — the label
	/// visibly touches its icon.</para>
	///
	/// <para><b>Measured, not argued</b> (2026-10-24, the real CardPen render at
	/// <c>https://argumentum.myia.io/</c>, ar payload with the shipped Arabic strings, viewport
	/// 375): LTR <c>piocheur</c> 3.83 px, <c>baratineur</c> 3.83 px; forced <c>dir=rtl</c> on the
	/// same DOM <b>0 px / 0 px</b>; with the logical declaration below, RTL <b>3.83 / 3.83</b> and
	/// LTR unchanged at 3.83 / 3.83. A pixel scan of the shipped ar harvest PNG independently
	/// shows 15-16 px icon→label gaps, so the touch is specific to a right-to-left pass and not
	/// to the current LTR render.</para>
	///
	/// <para><b>The suggestion grain, measured the same way</b> (2026-10-25, same instrument).
	/// <c>.suggestion</c> is a flex row <c>[quote-icon, text]</c> whose div ships
	/// <c>dir="auto"</c>: with Arabic content the browser resolves RTL <i>by itself</i>, so the
	/// defect is native to the shipped render — no forced direction involved. ar payload: gap
	/// <b>0.00 px</b> (icon edge = text edge); the same DOM forced <c>dir=ltr</c> 6.03 px; with
	/// the logical declaration 6.03 px under native RTL; fr payload native 6.03 px unchanged.
	/// The same session re-measured <c>.roles</c> at 3.83 px LTR, reproducing the #1966 figure
	/// and calibrating the instrument.</para>
	///
	/// <para><b>The three remaining margins, measured the same way</b> (2026-10-25, pool item 3).
	/// <c>.suggestion</c> itself (the container, not the icon) hangs its quote block out of the
	/// text column with a negative <i>start</i> margin: natively RTL (ar) the hang stayed on the
	/// physical left — the far side — leaving the block at 13.61/25.00 px from the card edges
	/// (symmetric, no hang at all) where fr reads 13.61 outside / 25.00 inside. With the logical
	/// declaration the ar render mirrors fr exactly (25.00/13.61). <c>.baratineur</c> (5% pair
	/// offset) and <c>.footer div</c> (label gap) live in direction-inert subtrees today — Chrome
	/// resolves <c>.texte</c> <c>dir=auto</c> to LTR because the Arabic text sits in descendant
	/// divs that carry their own <c>dir</c>, and the footer has no <c>dir</c> at all — so their
	/// defect only shows under a forced/true RTL context, exactly like <c>.roles</c> before it:
	/// forced RTL put the 5% offset mid-row (9.50 px between the pairs instead of following the
	/// baratineur pair) and the label gap at 3.19 px instead of 6.06; with the logical
	/// declarations both forced-RTL figures equal their fr values. The fr render is byte-identical
	/// before/after — under LTR <c>margin-inline-start</c> maps to the same physical side, so no
	/// shipped language moves.</para>
	///
	/// <para><b>Why the JSON is parsed rather than grepped.</b> A template's CSS lives inside a
	/// JSON string: on disk the quotes are escaped, and a regex over the raw bytes reads the escape
	/// rather than the declaration. Worse, this file is <i>deserialized</i> by production — the
	/// guard therefore loads it through the very deserializer <c>CardSetInfo</c> uses, so a
	/// template production cannot load can never pass here. (See
	/// <see cref="LanguageSpecificCssContractTests"/> for the same reasoning on the #1132 block,
	/// including why it is Utf8Json and not System.Text.Json.)</para>
	/// </summary>
	public class LogicalInlineMarginContractTests
	{
		private const string ScenariiFaceRelPath = "Cards/Scenarii/Argumentum_Scenarii_Face_fr.json";

		/// <summary>
		/// The icon of a role pair. Written exactly as in the template so it cannot silently match
		/// the neighbouring <c>.roles .piocheur img</c> / <c>.roles .baratineur img</c> size rules.
		/// </summary>
		private const string RolesIconSelector = ".roles img";

		/// <summary>
		/// The quote icon of the suggestion block. Written exactly as in the template so it cannot
		/// silently match the <c>.suggestion</c> container rule.
		/// </summary>
		private const string SuggestionIconSelector = ".suggestion img";

		/// <summary>The suggestion container itself, whose negative start margin hangs the quote block out.</summary>
		private const string SuggestionContainerSelector = ".suggestion";

		/// <summary>
		/// The baratineur pair div. Exact match so it cannot silently catch
		/// <c>.roles .baratineur img</c> (size rule) or <c>.roles &gt; div</c> (flex-basis rule).
		/// </summary>
		private const string BaratineurSelector = ".baratineur";

		/// <summary>The CC label div of the footer. Exact match, distinct from <c>.footer</c>/<c>.footer img</c>.</summary>
		private const string FooterLabelSelector = ".footer div";

		/// <summary>Declarations of every rule whose selector list contains <paramref name="selector"/>.</summary>
		internal static IReadOnlyList<string> DeclarationsFor(string css, string selector)
		{
			var stripped = Regex.Replace(css ?? "", @"/\*.*?\*/", " ", RegexOptions.Singleline);
			return Regex.Matches(stripped, @"(?<sel>[^{}]+)\{(?<body>[^{}]*)\}", RegexOptions.Singleline)
				.Where(m => m.Groups["sel"].Value
					.Split(',')
					.Any(s => string.Equals(s.Trim(), selector, StringComparison.Ordinal)))
				.Select(m => m.Groups["body"].Value)
				.ToList();
		}

		/// <summary>
		/// Splits a CSS value on <i>top-level</i> whitespace only: whitespace inside parentheses or
		/// brackets (and inside quoted strings) does not start a new value. <c>calc(1px + 2px)</c> is
		/// therefore <b>one</b> value, not three — the arity a shorthand is judged on is the arity the
		/// browser applies (CSS Values 3: a function token is a single component value).
		/// <para>A plain <c>Split</c> on whitespace reads <c>margin: 0 calc(1px + 2px + 3px + 4px)</c>
		/// as eight fragments, which falls outside the two-to-four window and let that bypass the
		/// guard (raised in review on #1994).</para>
		/// </summary>
		internal static IReadOnlyList<string> TopLevelValueParts(string value)
		{
			var parts = new List<string>();
			var current = new StringBuilder();
			var depth = 0;
			var quote = '\0';

			for (var i = 0; i < value.Length; i++)
			{
				var c = value[i];

				if (quote != '\0')
				{
					current.Append(c);
					if (c == '\\' && i + 1 < value.Length)
					{
						current.Append(value[++i]);
					}
					else if (c == quote)
					{
						quote = '\0';
					}
					continue;
				}

				switch (c)
				{
					case '"':
					case '\'':
						quote = c;
						current.Append(c);
						break;
					case '(':
					case '[':
						depth++;
						current.Append(c);
						break;
					case ')':
					case ']':
						if (depth > 0) depth--;
						current.Append(c);
						break;
					case '\\':
						current.Append(c);
						if (i + 1 < value.Length) current.Append(value[++i]);
						break;
					default:
						if (depth == 0 && char.IsWhiteSpace(c))
						{
							if (current.Length > 0)
							{
								parts.Add(current.ToString());
								current.Clear();
							}
						}
						else
						{
							current.Append(c);
						}
						break;
				}
			}

			if (current.Length > 0) parts.Add(current.ToString());
			return parts;
		}

		/// <summary>
		/// Declarations in <paramref name="selector"/>'s rules that pin an <i>asymmetric</i> margin
		/// to a physical side: <c>margin-left</c>, <c>margin-right</c>, or a four-value shorthand
		/// whose second (right) and fourth (left) <i>top-level</i> values differ. Two- and three-value
		/// shorthands, and symmetric four-value shorthands, give both horizontal sides the same margin
		/// and therefore do not create the directional defect. Logical properties —
		/// <c>margin-inline-start</c>, <c>margin-inline-end</c> — are deliberately not reported: they
		/// follow the writing direction, which is the whole point of the fix.
		/// </summary>
		internal static IReadOnlyList<string> PhysicalHorizontalMarginsIn(string css, string selector)
		{
			var found = new List<string>();
			foreach (var body in DeclarationsFor(css, selector))
			foreach (var decl in body.Split(';', StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = decl.Split(':', 2);
				if (parts.Length != 2) continue;
				var prop = parts[0].Trim().ToLowerInvariant();
				var value = parts[1].Trim();

				var physical = prop is "margin-left" or "margin-right";
				if (prop == "margin")
				{
					var values = TopLevelValueParts(value);
					physical = values.Count == 4
						&& !string.Equals(values[1], values[3], StringComparison.Ordinal);
				}

				if (physical) found.Add($"{prop}: {value}");
			}
			return found;
		}

		private static string ScenariiFaceCss()
		{
			var path = Path.Combine(TestRepoRoot.Find(), ScenariiFaceRelPath);
			File.Exists(path).Should().BeTrue($"template must exist at {ScenariiFaceRelPath}");
			var doc = Utf8Json.JsonSerializer.Deserialize<CardSetDocument>(File.ReadAllBytes(path));
			doc.Should().NotBeNull($"{ScenariiFaceRelPath} must deserialize as a CardSetDocument");
			return doc!.css ?? "";
		}

		/// <summary>
		/// The role icon's gap must be written as a logical margin, so it stays between the icon and
		/// its label in both writing directions.
		/// </summary>
		[Fact]
		public void Scenarii_role_icon_gap_is_declared_with_a_logical_margin()
		{
			var css = ScenariiFaceCss();

			DeclarationsFor(css, RolesIconSelector).Should().NotBeEmpty(
				$"'{RolesIconSelector}' must still be styled in {ScenariiFaceRelPath} — if the rule was "
				+ "renamed, update this guard rather than letting it pass on an empty selector");

			PhysicalHorizontalMarginsIn(css, RolesIconSelector).Should().BeEmpty(
				$"the gap between a role icon and its label must not be pinned to a physical side: under "
				+ "dir=rtl the flex row .roles reverses and a physical margin lands on the card's outer "
				+ "edge, leaving 0 px between the icon and its label (measured 2026-10-24, #458). "
				+ "Use margin-inline-end (or margin-inline-start) instead.");

			var declarations = DeclarationsFor(css, RolesIconSelector).SelectMany(d => d.Split(';'))
				.Select(d => d.Split(':', 2))
				.Where(p => p.Length == 2)
				.Select(p => (Property: p[0].Trim().ToLowerInvariant(), Value: p[1].Trim()))
				.ToList();

			declarations.Should().Contain(d => d.Property == "margin-inline-end" && d.Value != "0",
				$"'{RolesIconSelector}' must carry a non-zero margin-inline-end — that declaration is what "
				+ "produces the 3.83 px icon→label gap in both directions (#458)");
		}

		/// <summary>
		/// The quote icon's gap must be logical for the same reason — with the added twist that the
		/// suggestion div ships <c>dir="auto"</c>, so the 0 px touch is native to the ar/fa render,
		/// not something a forced direction would reveal (measured 2026-10-25, #458).
		/// </summary>
		[Fact]
		public void Scenarii_suggestion_icon_gap_is_declared_with_a_logical_margin()
		{
			var css = ScenariiFaceCss();

			DeclarationsFor(css, SuggestionIconSelector).Should().NotBeEmpty(
				$"'{SuggestionIconSelector}' must still be styled in {ScenariiFaceRelPath} — if the rule was "
				+ "renamed, update this guard rather than letting it pass on an empty selector");

			PhysicalHorizontalMarginsIn(css, SuggestionIconSelector).Should().BeEmpty(
				$"the gap between the quote icon and its text must not be pinned to a physical side: the "
				+ "suggestion div ships dir=auto, so on ar/fa content the browser resolves RTL by itself, the "
				+ "flex row reverses and a physical margin lands on the card's outer edge, leaving 0 px "
				+ "between the icon and its text — natively, with no forced direction (measured 2026-10-25, "
				+ "#458). Use margin-inline-end instead.");

			var declarations = DeclarationsFor(css, SuggestionIconSelector).SelectMany(d => d.Split(';'))
				.Select(d => d.Split(':', 2))
				.Where(p => p.Length == 2)
				.Select(p => (Property: p[0].Trim().ToLowerInvariant(), Value: p[1].Trim()))
				.ToList();

			declarations.Should().Contain(d => d.Property == "margin-inline-end" && d.Value != "0",
				$"'{SuggestionIconSelector}' must carry a non-zero margin-inline-end — that declaration is what "
				+ "produces the 6.03 px icon→text gap in both directions (#458)");
		}

		/// <summary>
		/// The suggestion container's hang — the negative start margin that pulls the quote block out
		/// of the text column — must be logical. Like the icon rule above, the defect is native to
		/// the ar/fa render (the div ships <c>dir="auto"</c>), measured 2026-10-25: fr 13.61 px
		/// outside / 25.00 px inside; ar before 13.61/25.00 symmetric — the hang swallowed on the far
		/// side; ar after the exact fr mirror 25.00/13.61.
		/// </summary>
		[Fact]
		public void Scenarii_suggestion_container_hang_is_declared_with_a_logical_margin()
		{
			var css = ScenariiFaceCss();

			DeclarationsFor(css, SuggestionContainerSelector).Should().NotBeEmpty(
				$"'{SuggestionContainerSelector}' must still be styled in {ScenariiFaceRelPath} — if the rule was "
				+ "renamed, update this guard rather than letting it pass on an empty selector");

			PhysicalHorizontalMarginsIn(css, SuggestionContainerSelector).Should().BeEmpty(
				$"the quote block's hang must not be pinned to a physical side: the suggestion div ships "
				+ "dir=auto, so on ar/fa content the browser resolves RTL by itself and the physical left "
				+ "lands on the card's outer edge — the block then sits inside the text column instead of "
				+ "hanging out of it, natively with no forced direction (measured 2026-10-25: fr "
				+ "13.61/25.00, ar before 13.61/25.00 symmetric = hang swallowed, ar after 25.00/13.61 = "
				+ "exact fr mirror, #458). Use margin-inline-start instead.");

			var declarations = DeclarationsFor(css, SuggestionContainerSelector).SelectMany(d => d.Split(';'))
				.Select(d => d.Split(':', 2))
				.Where(p => p.Length == 2)
				.Select(p => (Property: p[0].Trim().ToLowerInvariant(), Value: p[1].Trim()))
				.ToList();

			declarations.Should().Contain(d => d.Property == "margin-inline-start" && d.Value != "0",
				$"'{SuggestionContainerSelector}' must carry a non-zero margin-inline-start — that declaration "
				+ "is what hangs the quote block out of the text column in both directions (#458)");
		}

		/// <summary>
		/// The baratineur pair's 5% offset must be logical. The pair lives in a direction-inert
		/// subtree today — Chrome resolves <c>.texte</c>'s <c>dir=auto</c> to LTR because the Arabic
		/// text sits in descendant divs that carry their own <c>dir</c> — so, exactly like
		/// <c>.roles</c> before it, the defect shows under a forced/true RTL context: the physical
		/// offset landed mid-row (9.50 px between the two pairs) instead of following the baratineur
		/// pair; with the logical declaration the pair follows the row in both directions (measured
		/// 2026-10-25, #458). Inert in native LTR: the 9.50 px spacing is unchanged.
		/// </summary>
		[Fact]
		public void Scenarii_baratineur_pair_offset_is_declared_with_a_logical_margin()
		{
			var css = ScenariiFaceCss();

			DeclarationsFor(css, BaratineurSelector).Should().NotBeEmpty(
				$"'{BaratineurSelector}' must still be styled in {ScenariiFaceRelPath} — if the rule was "
				+ "renamed, update this guard rather than letting it pass on an empty selector");

			PhysicalHorizontalMarginsIn(css, BaratineurSelector).Should().BeEmpty(
				$"the baratineur pair's offset must not be pinned to a physical side: under a true RTL "
				+ "context the flex .roles row reverses and the physical offset lands mid-row, 9.50 px "
				+ "between the two pairs instead of following the baratineur pair (measured 2026-10-25, "
				+ "#458). Use margin-inline-start instead.");

			var declarations = DeclarationsFor(css, BaratineurSelector).SelectMany(d => d.Split(';'))
				.Select(d => d.Split(':', 2))
				.Where(p => p.Length == 2)
				.Select(p => (Property: p[0].Trim().ToLowerInvariant(), Value: p[1].Trim()))
				.ToList();

			declarations.Should().Contain(d => d.Property == "margin-inline-start" && d.Value != "0",
				$"'{BaratineurSelector}' must carry a non-zero margin-inline-start — that declaration is what "
				+ "keeps the 5% offset on the baratineur pair in both directions (#458)");
		}

		/// <summary>
		/// The footer CC-label's gap must be logical. The footer carries no <c>dir</c> of its own, so
		/// the defect is latent — under a forced/true RTL context the physical gap fell on the outer
		/// side and the label sat 3.19 px from its icons instead of 6.06; with the logical
		/// declaration both directions read the same 6.06 px gap (measured 2026-10-25, #458).
		/// Inert in native LTR: fr renders identically before/after.
		/// </summary>
		[Fact]
		public void Scenarii_footer_label_gap_is_declared_with_a_logical_margin()
		{
			var css = ScenariiFaceCss();

			DeclarationsFor(css, FooterLabelSelector).Should().NotBeEmpty(
				$"'{FooterLabelSelector}' must still be styled in {ScenariiFaceRelPath} — if the rule was "
				+ "renamed, update this guard rather than letting it pass on an empty selector");

			PhysicalHorizontalMarginsIn(css, FooterLabelSelector).Should().BeEmpty(
				$"the footer label's gap must not be pinned to a physical side: under a true RTL context "
				+ "the physical margin falls on the outer side and the label sits 3.19 px from its icons "
				+ "instead of 6.06 (measured 2026-10-25, #458). Use margin-inline-start instead.");

			var declarations = DeclarationsFor(css, FooterLabelSelector).SelectMany(d => d.Split(';'))
				.Select(d => d.Split(':', 2))
				.Where(p => p.Length == 2)
				.Select(p => (Property: p[0].Trim().ToLowerInvariant(), Value: p[1].Trim()))
				.ToList();

			declarations.Should().Contain(d => d.Property == "margin-inline-start" && d.Value != "0",
				$"'{FooterLabelSelector}' must carry a non-zero margin-inline-start — that declaration is what "
				+ "keeps the 6.06 px label→icons gap in both directions (#458)");
		}

		/// <summary>
		/// The guard above is only informative if it is able to see the defect it claims is absent.
		/// This injects the pre-fix declaration — verbatim the shape that shipped — and requires the
		/// guard to report it.
		/// </summary>
		[Fact]
		public void The_guard_reports_the_physical_margin_that_shipped_before_the_fix()
		{
			PhysicalHorizontalMarginsIn(".roles img { margin-right: 0.3em; }", RolesIconSelector)
				.Should().ContainSingle().Which.Should().Be("margin-right: 0.3em",
					"the pre-fix declaration must be visible to the guard, otherwise a green result "
					+ "would only mean the instrument is blind");
		}

		/// <summary>And it stays silent on the corrected declaration — the inverse control's other half.</summary>
		[Fact]
		public void The_guard_is_silent_on_the_logical_declaration()
		{
			PhysicalHorizontalMarginsIn(".roles img { margin-inline-end: 0.3em; }", RolesIconSelector)
				.Should().BeEmpty("a logical margin follows the writing direction and is not the defect");
		}

		/// <summary>
		/// Only an asymmetric four-value shorthand pins opposite horizontal sides differently. The
		/// second value is right and the fourth is left; when they are equal, mirroring cannot move
		/// the gap to the wrong side. Two- and three-value forms likewise expand to equal left/right
		/// margins. The comparison is deliberately lexical: equivalent CSS expressions need not be
		/// evaluated for this guard, while a conservative report remains preferable to a false negative.
		/// </summary>
		[Theory]
		[InlineData("0 0.3em 1em 0")]
		[InlineData("0 calc(1px + 2px + 3px + 4px) 1em 0")]
		[InlineData("0 min(1px, calc(2px + 3px)) 1em 0")]
		public void The_guard_reports_an_asymmetric_four_value_shorthand(string margin)
		{
			PhysicalHorizontalMarginsIn($".roles img {{ margin: {margin}; }}", RolesIconSelector)
				.Should().ContainSingle().Which.Should().Be($"margin: {margin}",
					"different second and fourth shorthand values pin right and left asymmetrically");
		}

		[Theory]
		[InlineData("0")]
		[InlineData("0 0.3em")]
		[InlineData("0 0.3em 1em")]
		[InlineData("0 0.3em 1em 0.3em")]
		[InlineData("0 calc(1px + 2px + 3px + 4px)")]
		[InlineData("0 min(1px, calc(2px + 3px)) 1em")]
		[InlineData("0 calc(1px + 2px) 1em calc(1px + 2px)")]
		public void The_guard_is_silent_when_the_shorthand_has_equal_horizontal_margins(string margin)
		{
			PhysicalHorizontalMarginsIn($".roles img {{ margin: {margin}; }}", RolesIconSelector)
				.Should().BeEmpty("a one-to-three-value or symmetric four-value shorthand gives left and "
					+ "right the same margin, so mirroring does not create the directional defect");
		}

		/// <summary>
		/// The inverse control for the tokenizer: the very string that bypassed the previous guard must
		/// still read as eight fragments under a naive split, so the case above is proven to exercise
		/// the fix rather than merely to pass.
		/// </summary>
		[Fact]
		public void The_shorthand_tokenizer_keeps_css_functions_whole()
		{
			const string twoValueFunction = "0 calc(1px + 2px + 3px + 4px)";

			twoValueFunction.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries)
				.Should().HaveCount(8, "a naive whitespace split reads this two-value shorthand as eight "
					+ "fragments — that is the bypass the tokenizer closes");

			TopLevelValueParts(twoValueFunction)
				.Should().Equal(new[] { "0", "calc(1px + 2px + 3px + 4px)" },
					"a function token is one component value, so this shorthand has two");

			TopLevelValueParts("0 min(1px, calc(2px + 3px)) 1em")
				.Should().Equal(new[] { "0", "min(1px, calc(2px + 3px))", "1em" },
					"nesting must not end the first value early");

			TopLevelValueParts("  0   0.3em  ")
				.Should().Equal(new[] { "0", "0.3em" },
					"runs of whitespace and edge padding do not create empty values");

			TopLevelValueParts("calc(1px + 2px)")
				.Should().ContainSingle("a single function value applies to all four sides and pins no side");
		}
	}
}
