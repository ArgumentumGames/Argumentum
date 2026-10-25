using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Contract of the #458 footer grain: in both 2shineBS5 footer templates the "Imprint"
	/// label must reach the terms/imprint target and the "Privacy" label the privacy target —
	/// never crossed.
	///
	/// Why it exists: Landingpage.ascx bound <c>Imprint.Text</c> to <c>PrivacyTabId</c> and
	/// <c>Privacy.Text</c> to <c>TermsTabId</c> (lines 132-133) while theme-body.ascx bound the
	/// same two labels to <c>/terms</c> and <c>/privacy</c> (lines 157-158). The defect is
	/// latent today — measured 2026-10-25: no tab of portal 1 selects Landingpage.ascx
	/// (<c>Tabs.SkinSrc</c>, <c>Tabs.ContainerSrc</c> and <c>PortalSettings</c> hold 0 row
	/// naming it; the only explicit per-tab skins are the 404 page and the store back office),
	/// and both portal settings it reads are unset, so its links would resolve through
	/// <c>NavigateURL(-1)</c>. It stops being latent the first time a page picks that template,
	/// which is why the invariant is pinned in a guard rather than left to review.
	///
	/// One assertion pair per file, each with an anti-vacuity count: a pattern that matched
	/// nothing would otherwise read as a pass.
	/// </summary>
	public class SkinFooterLinkContractTests
	{
		private static readonly string SkinDir = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "_default", "Skins", "2shineBS5");

		private static readonly string[] MarkupFiles =
		{
			Path.Combine(SkinDir, "controls", "theme-body.ascx"),
			Path.Combine(SkinDir, "Landingpage.ascx")
		};

		private static readonly Regex AnchorPattern =
			new Regex(@"<a\b[^>]*>.*?</a>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

		private static readonly Regex HrefPattern =
			new Regex(@"href\s*=\s*(?:""([^""]*)""|'([^']*)')", RegexOptions.IgnoreCase);

		/// <summary>Anchors of the footer imprint block carrying the given resx key.</summary>
		private static string[] AnchorsFor(string markup, string resxKey)
		{
			var needle = "LocalizeString(\"" + resxKey + "\")";
			var start = markup.IndexOf("to-shine-footer-imprint", StringComparison.OrdinalIgnoreCase);
			Assert.True(start >= 0, "footer imprint block not found in the template");
			var block = markup.Substring(start);
			return AnchorPattern.Matches(block)
				.Cast<Match>()
				.Select(m => m.Value)
				.Where(a => a.Contains(needle, StringComparison.Ordinal))
				.ToArray();
		}

		private static string HrefOf(string anchor)
		{
			var m = HrefPattern.Match(anchor);
			Assert.True(m.Success, "footer link without an href in the template: " + anchor);
			return m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value;
		}

		[Theory]
		[InlineData("controls/theme-body.ascx")]
		[InlineData("Landingpage.ascx")]
		public void ImprintLabelReachesTheTermsTarget(string relativePath)
		{
			var file = Path.Combine(SkinDir, relativePath);
			var anchors = AnchorsFor(File.ReadAllText(file), "Imprint.Text");
			Assert.True(anchors.Length == 1,
				relativePath + ": expected exactly one footer link carrying Imprint.Text, found " + anchors.Length);
			Assert.Matches("(?i)terms", HrefOf(anchors[0]));
		}

		[Theory]
		[InlineData("controls/theme-body.ascx")]
		[InlineData("Landingpage.ascx")]
		public void PrivacyLabelReachesThePrivacyTarget(string relativePath)
		{
			var file = Path.Combine(SkinDir, relativePath);
			var anchors = AnchorsFor(File.ReadAllText(file), "Privacy.Text");
			Assert.True(anchors.Length == 1,
				relativePath + ": expected exactly one footer link carrying Privacy.Text, found " + anchors.Length);
			Assert.Matches("(?i)privacy", HrefOf(anchors[0]));
		}

		/// <summary>
		/// The two labels are distinct links: an anchor carrying both keys would satisfy the two
		/// tests above while serving one target for both labels.
		/// </summary>
		[Theory]
		[InlineData("controls/theme-body.ascx")]
		[InlineData("Landingpage.ascx")]
		public void TheTwoLabelsAreDistinctLinks(string relativePath)
		{
			var anchors = AnchorsFor(File.ReadAllText(Path.Combine(SkinDir, relativePath)), "Imprint.Text");
			Assert.DoesNotContain("Privacy.Text", anchors[0]);
		}
	}
}
