using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Contract for issue #458, grain "Scenarii role label touches its icon in RTL".
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
		/// Declarations in <paramref name="selector"/>'s rules that pin a margin to a <i>physical</i>
		/// side (<c>margin-left</c>, <c>margin-right</c>, or the 4-value shorthand, whose 2nd and 4th
		/// values are the physical right and left). Logical properties — <c>margin-inline-start</c>,
		/// <c>margin-inline-end</c> — are deliberately not reported: they follow the writing
		/// direction, which is the whole point of the fix.
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

				var physical = prop is "margin-left" or "margin-right"
					|| (prop == "margin" && value.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries).Length == 4);

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
		/// A four-value <c>margin</c> shorthand also pins both physical sides, so the guard must see
		/// it too — otherwise a rewrite to <c>margin: 0 0.3em 0 0</c> would pass as "no margin-right".
		/// </summary>
		[Fact]
		public void The_guard_reports_a_four_value_margin_shorthand()
		{
			PhysicalHorizontalMarginsIn(".roles img { margin: 0 0.3em 0 0; }", RolesIconSelector)
				.Should().ContainSingle().Which.Should().Be("margin: 0 0.3em 0 0",
					"the 2nd and 4th shorthand values are the physical right and left margins");

			PhysicalHorizontalMarginsIn(".roles img { margin: 0; }", RolesIconSelector)
				.Should().BeEmpty("a single-value shorthand applies to all four sides and pins no side");
		}
	}
}
