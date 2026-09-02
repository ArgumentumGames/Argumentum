using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// Contract: the mind map's click-to-define modal — the <c>&lt;card&gt;</c> element shown when a
	/// node is clicked — must keep the aspect ratio of a real game card, and must stay inside the
	/// viewport whatever its content.
	///
	/// <para><b>Why this exists.</b> Reported by the owner and mis-qualified twice before being
	/// measured (issue #1253): the modal was "complètement étirée en hauteur, genre elle dépasse
	/// l'écran". Both earlier readings blamed the mind map SVG — measurement cleared the SVG
	/// (176 images, ratio 1.000, zero internal distortion) and located the defect here, in the
	/// wrapper's own CSS.</para>
	///
	/// <para><b>The mechanism, and why nothing caught it.</b> Commit <c>c877c5ba</c> (#827,
	/// "restore click-to-define card colors, centering and size") replaced
	/// <c>width: 50vw; height: 50vh</c> with a bare <c>width: 300px</c> and <b>dropped the height
	/// entirely</b>. The modal's typography, however, is declared in viewport units
	/// (<c>.famille: 5.6vh</c>, <c>.sous_famille: 3.4vh</c>, …). Absolute width plus
	/// viewport-relative content plus no height bound means the box grows with the screen in one
	/// dimension only:</para>
	/// <list type="bullet">
	///   <item>viewport 1280 × 900 → modal 300 × 1097 px, ratio 3.66 — 122 % of screen height;</item>
	///   <item>viewport 1280 × 1400 → modal 300 × 2135 px, ratio 7.12 — 152 % of screen height.</item>
	/// </list>
	/// <para>The taller the display, the worse it gets, which is why it reads as an intermittent
	/// regression rather than a deterministic one. Before #827, <c>height: 50vh</c> made overflow
	/// structurally impossible; the "restore" commit is what removed the bound.</para>
	///
	/// <para><b>Scope is derived, not tabulated.</b> The file list is discovered on disk — the two
	/// templates plus every generated wrapper under <c>Cards/*/Mindmaps/*/</c>. A hardcoded list
	/// would silently stop covering a language or a deck added later, which is the failure mode
	/// where a guard keeps passing because it stopped looking (cf. #1250, where a hand-written
	/// table of four CardSets hid six more).</para>
	/// </summary>
	public class MindMapCardModalAspectContractTests
	{
		/// <summary>
		/// Isolates the <i>structure</i> rule for <c>card</c> — the one carrying
		/// <c>position: fixed</c>. Each wrapper also declares a colour-only <c>card { --color-… }</c>
		/// fallback earlier in the sheet; matching that one instead would test nothing.
		/// </summary>
		private static readonly Regex StructureRule = new(
			@"(?<!\S)card\s*\{(?<body>[^}]*position\s*:\s*fixed[^}]*)\}",
			RegexOptions.Compiled);

		private static readonly Regex AbsoluteWidth = new(@"(?<!-)\bwidth\s*:\s*\d+(\.\d+)?px", RegexOptions.Compiled);
		private static readonly Regex AspectRatio = new(@"\baspect-ratio\s*:\s*[\d.]+\s*/\s*[\d.]+", RegexOptions.Compiled);
		private static readonly Regex ViewportHeight = new(@"(?<!-)\bheight\s*:\s*\d+(\.\d+)?v[hw]", RegexOptions.Compiled);

		public static IEnumerable<object[]> WrapperFiles()
		{
			var root = TestRepoRoot.Find();
			var templates = Directory.EnumerateFiles(Path.Combine(root, "Cards", "Fallacies", "Mindmaps"), "*.html");
			var generated = Directory.EnumerateFiles(Path.Combine(root, "Cards"), "*.html", SearchOption.AllDirectories)
				.Where(p => p.Replace('\\', '/').Contains("/Mindmaps/"));

			return templates.Concat(generated)
				.Distinct()
				.OrderBy(p => p)
				.Select(p => new object[] { Path.GetRelativePath(root, p) });
		}

		[Theory]
		[MemberData(nameof(WrapperFiles))]
		public void CardModal_KeepsGameCardAspectAndBoundedHeight(string relativePath)
		{
			var body = ReadStructureRule(relativePath);

			AspectRatio.IsMatch(body).Should().BeTrue(
				"{0}: the modal must declare an explicit aspect-ratio so it renders at the shape of a " +
				"real card. Without it the height is whatever the vh-sized content sums to — 7.12:1 at " +
				"a 1400px viewport, against 1.727:1 for a tarot card (#1253).", relativePath);

			ViewportHeight.IsMatch(body).Should().BeTrue(
				"{0}: the modal must bound its height in viewport units, otherwise it overflows the " +
				"screen on tall displays. This is the exact declaration #827 deleted.", relativePath);

			AbsoluteWidth.IsMatch(body).Should().BeFalse(
				"{0}: an absolute px width combined with vh-sized typography is what produced the " +
				"stretch — the box cannot grow sideways while its content grows with the viewport. " +
				"Let the width derive from the height and the aspect-ratio.", relativePath);
		}

		/// <summary>
		/// Inverse control. A guard that only ever sees corrected files proves nothing about its own
		/// sensitivity — it can be green because it is blind (cf. #1112, a self-baselining SVG test
		/// that wrote its own expectation). This feeds the detector the <i>historical</i> defective
		/// rule, verbatim from <c>c877c5ba</c>, and requires it to be rejected on all three counts.
		/// </summary>
		[Fact]
		public void Detector_RejectsThePre1253Rule()
		{
			const string regressed = @"
        card {
            background-color: white;
            display: flex;
            flex-direction: column;
            left: 50%;
            position: fixed;
            top: 50%;
            transform: translate(-50%, -50%);
            width: 300px;
            z-index: 100;
        }";
			var body = StructureRule.Match(regressed).Groups["body"].Value;

			body.Should().NotBeEmpty("the structure rule must be located before it can be judged");
			AbsoluteWidth.IsMatch(body).Should().BeTrue("the regressed rule pins width at 300px");
			AspectRatio.IsMatch(body).Should().BeFalse("the regressed rule declares no aspect-ratio");
			ViewportHeight.IsMatch(body).Should().BeFalse("the regressed rule declares no height at all");
		}

		/// <summary>
		/// Second inverse control: the pre-#827 shape (<c>50vw × 50vh</c>) was bounded but carried no
		/// card aspect either. It must fail the aspect assertion while passing the height one, which
		/// proves the two checks are independent rather than one assertion wearing three hats.
		/// </summary>
		[Fact]
		public void Detector_SeparatesBoundedFromCorrectlyShaped()
		{
			const string pre827 = @"
        card {
            position: fixed;
            z-index: 100;
            width: 50vw;
            height: 50vh;
            display: flex;
        }";
			var body = StructureRule.Match(pre827).Groups["body"].Value;

			ViewportHeight.IsMatch(body).Should().BeTrue("50vh is a viewport-bounded height");
			AspectRatio.IsMatch(body).Should().BeFalse("but it still carries no card aspect");
			AbsoluteWidth.IsMatch(body).Should().BeFalse("and no absolute width");
		}

		private static string ReadStructureRule(string relativePath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), relativePath);
			File.Exists(path).Should().BeTrue("{0} was discovered on disk by the theory source", relativePath);

			var match = StructureRule.Match(File.ReadAllText(path));
			match.Success.Should().BeTrue(
				"{0}: no `card {{ … position: fixed … }}` rule found. Either the modal lost its " +
				"structure rule, or this guard is looking at the wrong selector and has gone blind.",
				relativePath);

			return match.Groups["body"].Value;
		}
	}
}
