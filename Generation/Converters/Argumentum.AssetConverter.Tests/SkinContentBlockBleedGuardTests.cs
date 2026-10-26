using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Contract of the mobile 3px-bleed fix (#458): the skin grid sets a 30px row gutter
	/// (±15px negative margins, theme.min.css) while the skin container pads content by 12px
	/// per side, so every bare <c>.row</c> inside a <c>.sc-content-block</c> (padding 0) bleeds
	/// 3px past the right edge — measured document.scrollWidth 363 for a 360px viewport
	/// (378 for 375, the residue left over after #1936). The fix clips that empty margin bleed
	/// at the block boundary (<c>overflow-x: clip</c>) instead of shifting layout: no column
	/// moves, only the horizontal rubber-band disappears. These tests pin the rule in BOTH
	/// skin builds (LTR and RTL) — a rebuild of theme.min.css without the rule goes red here.
	/// </summary>
	public class SkinContentBlockBleedGuardTests
	{
		private static readonly string DistDir = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "_default", "Skins", "2shineBS5", "dist");

		[Theory]
		[InlineData("theme.min.css")]
		[InlineData("theme.rtl.min.css")]
		public void SkinBuildClipsTheContentBlockBleed(string cssFile)
		{
			var css = File.ReadAllText(Path.Combine(DistDir, cssFile));
			Assert.Matches(new Regex(@"\.sc-content-block\s*\{[^}]*overflow-x:\s*clip[^}]*\}"), css);
		}
	}
}
