using System;
using System.IO;
using System.Text.RegularExpressions;

using FluentAssertions;

using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Contract for issue #458, grain "contrat sur le chemin de lecture #1962" (ai-01 option).
	///
	/// <para><b>What this guards.</b> The resx-side guards (<see cref="SkinResxMetaContractTests"/>,
	/// <see cref="SkinLoginTooltipContractTests"/>) pin that the culture files CARRY the right keys;
	/// they say nothing about whether the skin still READS those files. The read path is the part
	/// that was measured broken on preprod (2026-10-24, #1946/#1962): the culture file must be
	/// named EXPLICITLY — <c>GetString(key, root + "." + culture + ".resx")</c> — never through the
	/// culture-name overload <c>GetString(key, file, culture)</c>, which DNN normalizes to the
	/// invariant culture whenever the current culture is not a registered portal language, so the
	/// lookup silently reads the base file. If any of these code sites is "simplified" back to the
	/// overload, every resx guard stays green while /en serves the base file again.</para>
	///
	/// <para><b>The sites.</b> <c>nav/main/template.cshtml</c> and <c>nav/main-mobile/template.cshtml</c>
	/// (<c>MenuLookup</c>: menu labels/links, missing key falls back to the DNN TabName), and
	/// <c>controls/theme-body.ascx</c> (<c>CultureString</c>/<c>CultureResourceFile</c> via
	/// <c>LocalizeString</c>, missing key falls back to the DNN resource path). The sentinel
	/// <c>s == key</c> is part of the mechanism: it is what stops a missing key from leaking the
	/// base file's value through per-key fallback.</para>
	///
	/// <para><b>Deliberate asymmetry.</b> <c>Landingpage.ascx</c> keeps the plain DNN path
	/// (<c>Localization.GetString(key, ResourceFilePath)</c>): its keys live in every culture file
	/// AND in the base file, so per-key fallback can only ever land on a value that was placed
	/// deliberately. Pinned here so a future "harmonisation" of Landingpage onto the explicit path
	/// is a conscious decision, not a drive-by.</para>
	/// </summary>
	public class SkinCultureFileReadPathContractTests
	{
		private static readonly string SkinDir = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "_default", "Skins", "2shineBS5");

		private static readonly string[] MenuTemplates =
		{
			Path.Combine("nav", "main", "template.cshtml"),
			Path.Combine("nav", "main-mobile", "template.cshtml")
		};

		private static string SkinSource(string relativePath) =>
			File.ReadAllText(Path.Combine(SkinDir, relativePath));

		/// <summary>
		/// The measured-broken shape: a three-argument <c>Localization.GetString(key, file, culture)</c>
		/// call, whose last argument is the culture. The explicit path never passes the culture as a
		/// lookup argument — it is part of the FILE NAME it passes.
		/// <para>
		/// Comments are stripped first: the #1946 comments deliberately CITE the broken overload
		/// ("GetString(key, …, \"en-US\") reads the invariant base file…") — that documentation is
		/// correct and must not trip the detector. Only full-line C# comments and Razor
		/// <c>@* … *@</c> blocks are stripped, so a URL inside a string literal is never cut.</para>
		/// </summary>
		internal static bool UsesCultureNameOverload(string source)
		{
			var code = Regex.Replace(source, @"@\*.*?\*@", " ", RegexOptions.Singleline);
			code = Regex.Replace(code, @"(?m)^\s*//.*$", " ");
			return Regex.IsMatch(code, @"Localization\.GetString\s*\([^()]*,[^()]*,[^()]*\)");
		}

		/// <summary>
		/// The explicit shape: the culture is concatenated into the resource file name. The literal
		/// <c>+ "." + culture + ".resx"</c> is shared verbatim by both families of sites — the nav
		/// templates build <c>root + "." + culture + ".resx"</c>, theme-body builds
		/// <c>ResxRoot + "." + culture + ".resx"</c>.
		/// </summary>
		internal static bool NamesCultureFileExplicitly(string source) =>
			source.Contains("+ \".\" + culture + \".resx\"", StringComparison.Ordinal);

		[Fact]
		public void MenuTemplatesNameTheCultureFileExplicitly()
		{
			foreach (var template in MenuTemplates)
			{
				var source = SkinSource(template);

				NamesCultureFileExplicitly(source).Should().BeTrue(
					$"{template} must build the culture resx path explicitly (root + \".\" + culture + \".resx\") — "
					+ "the culture-name overload reads the base file for any culture that is not a registered "
					+ "portal language (measured on preprod, #1946/#1962)");

				UsesCultureNameOverload(source).Should().BeFalse(
					$"{template} must not pass the culture as a lookup argument to Localization.GetString");

				source.Should().Contain("s == key",
					$"{template}: the missing-key sentinel is part of the mechanism — without it a missing key "
					+ "leaks the base-file value through per-key fallback and the localized labels cannot fall "
					+ "back to the DNN TabName");
			}
		}

		[Fact]
		public void ThemeBodyReadsTheCultureFileBeforeTheDnnFallback()
		{
			var source = SkinSource(Path.Combine("controls", "theme-body.ascx"));

			source.Should().Contain("CultureString(key) ?? Localization.GetString(key, ResourceFilePath)",
				"the explicit culture read must run FIRST, the DNN resource path only as fallback — inverting "
				+ "or dropping the first leg puts every culture back on the normalized-to-base lookup (#1946)");

			NamesCultureFileExplicitly(source).Should().BeTrue(
				"theme-body's CultureResourceFile must build the culture resx path explicitly "
				+ "(ResxRoot + \".\" + culture + \".resx\")");

			source.Should().Contain("s == key) ? null",
				"a key missing from the culture file must yield null — that null is what skips the override and "
				+ "lets fr pages keep their native meta (the base file carries English strings)");

			UsesCultureNameOverload(source).Should().BeFalse(
				"theme-body must not pass the culture as a lookup argument to Localization.GetString");
		}

		[Fact]
		public void LandingpageKeepsThePlainDnnPathByDesign()
		{
			var source = SkinSource("Landingpage.ascx");

			source.Should().Contain("Localization.GetString(key, ResourceFilePath)",
				"Landingpage's LocalizeString is deliberately the plain DNN path: its keys live in every "
				+ "culture file and in the base file (pinned by SkinLoginTooltipContractTests), so per-key "
				+ "fallback always lands on a deliberate value. Moving it onto the explicit path is a "
				+ "decision to make consciously — update this guard with the reasoning, do not drift onto it");
		}

		/// <summary>
		/// The guards above are only informative if they can see the defect they claim is absent. This
		/// injects the pre-fix shapes — the culture-name overload in a menu template, and a
		/// LocalizeString with the explicit leg removed — and requires each to be caught.
		/// </summary>
		[Fact]
		public void TheGuardsSeeThePreFixShapes()
		{
			var revertedMenu = GetText(
				"var s = string.IsNullOrEmpty(culture)",
				"? Localization.GetString(key, root + \".resx\")",
				": Localization.GetString(key, root + \".resx\", culture);",
				"return (string.IsNullOrWhiteSpace(s) || s == key) ? fallback : s;");
			UsesCultureNameOverload(revertedMenu).Should().BeTrue(
				"the three-argument overload is the measured-broken read path — the detector must fire on it");
			NamesCultureFileExplicitly(revertedMenu).Should().BeFalse(
				"the reverted template no longer names the culture file");

			var revertedBody = GetText(
				"protected string LocalizeString(string key)",
				"{",
				"    return Localization.GetString(key, ResourceFilePath);",
				"}");
			revertedBody.Should().NotContain("CultureString(key) ?? Localization.GetString(key, ResourceFilePath)",
				"a LocalizeString without the explicit first leg must fail ThemeBodyReadsTheCultureFileBeforeTheDnnFallback");

			var overloadInBody = GetText(
				"var s = Localization.GetString(key, ResxRoot + \".resx\", culture);",
				"return (string.IsNullOrWhiteSpace(s) || s == key) ? null : s;");
			UsesCultureNameOverload(overloadInBody).Should().BeTrue(
				"the overload with a literal file path is still the three-argument broken shape");
		}

		private static string GetText(params string[] lines) => string.Join(Environment.NewLine, lines);
	}
}
