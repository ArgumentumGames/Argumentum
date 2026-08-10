using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// Golden-master content scan over the committed mindmap HTML artifacts (2 templates +
    /// 32 wrappers). Headless (no browser) so it runs in CI; it pins the shipped files directly
    /// and complements the Playwright behavioral tests in
    /// <c>Argumentum.AssetConverter.VisualTests/MindmapWrapperTests.cs</c>, which operate on a
    /// single composed wrapper written to a temp dir and therefore never read these files.
    /// </summary>
    /// <remarks>
    /// Capability #10 of issue #830 (added by #1037): every shipped wrapper must wire
    /// <c>window.resize</c> and <c>orientationchange</c> to <c>panZoomInstance.resize()</c>.
    /// svg-pan-zoom does not subscribe to <c>window.resize</c> itself, and #827 inadvertently
    /// removed the listener that compensated — without it the viewport freezes on the geometry
    /// captured at load time and the root node drifts off-screen after a window resize or a
    /// mobile orientation change.
    /// </remarks>
    public class MindmapWrapperGoldenMasterTests
    {
        private static readonly string RepoRoot = TestRepoRoot.Find();

        private static readonly string MindmapsRoot =
            Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps");

        // The three substrings that together prove the wiring is live (not a dead stub):
        // the two listeners AND that they actually call resize().
        private static readonly string[] RequiredWiring =
        {
            "window.addEventListener('resize'",
            "window.addEventListener('orientationchange'",
            "panZoomInstance.resize()"
        };

        [Fact]
        public void AllCommittedMindmapHtmlFiles_WireResizeAndOrientationChange_ToPanZoomResize()
        {
            var files = Directory.EnumerateFiles(MindmapsRoot, "*.html", SearchOption.AllDirectories)
                .OrderBy(f => f)
                .ToList();

            // 8 langs × {Fallacies, Virtues} × {embed, _ext} = 32 wrappers + 2 templates = 34.
            // Floor (not exact count) so a legitimately-added language does not false-alarm;
            // the per-file substring loop below is the hard guard for missing wiring.
            files.Count.Should().BeGreaterThanOrEqualTo(34,
                "expected at least 32 wrappers (8 langs × {Fallacies,Virtues} × {embed,_ext}) + 2 templates");

            var missing = new List<string>();
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                foreach (var needle in RequiredWiring)
                {
                    if (!content.Contains(needle))
                        missing.Add($"{Path.GetFileName(file)}: missing '{needle}'");
                }
            }

            missing.Should().BeEmpty(
                "capability #10 (#830 / #1037): resize + orientationchange must call panZoomInstance.resize() in every shipped mindmap artifact");
        }
    }
}
