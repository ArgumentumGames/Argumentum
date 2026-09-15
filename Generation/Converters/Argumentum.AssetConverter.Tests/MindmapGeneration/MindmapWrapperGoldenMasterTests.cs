using System;
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

        /// <summary>
        /// #1285 — expected external-wrapper count, DERIVED from the C# creator configs (the
        /// source of truth, SkipConfigFile=true), never hardcoded: the external.html template,
        /// plus one <c>_ext</c> wrapper per enabled document that ships SVG variants, times
        /// (1 + |Translations|). The FR-only cards document (#1269) legitimately raised 17 → 18;
        /// a future language or document must not require touching this organ.
        /// </summary>
        private static int ExpectedExternalWrapperCount()
        {
            var fallacyDocs = new Argumentum.AssetConverter.Mindmapper.FallacyMindMapCreatorConfig().DocumentConfigs
                .Select(d => (d.Enabled, Langs: 1 + (d.Translations?.Count ?? 0), ShipsSvg: d.SVGMaps.Any(m => m.Enabled)));
            var virtueDocs = new Argumentum.AssetConverter.Mindmapper.VirtueMindMapCreatorConfig().DocumentConfigs
                .Select(d => (d.Enabled, Langs: 1 + (d.Translations?.Count ?? 0), ShipsSvg: d.SVGMaps.Any(m => m.Enabled)));
            return 1 + fallacyDocs.Concat(virtueDocs)
                .Where(d => d.Enabled && d.ShipsSvg)
                .Sum(d => d.Langs);
        }

        private const int AntiCollapseFloor = 17; // 8 Fallacies + 8 Virtues + template, pre-cards baseline

        // The three substrings that together prove the wiring is live (not a dead stub):
        // the two listeners AND that they actually call resize().
        private static readonly string[] RequiredWiring =
        {
            "window.addEventListener('resize'",
            "window.addEventListener('orientationchange'",
            "panZoomInstance.resize()"
        };

        // Capability #11 of issue #830 (added by #1040): the external-path artifacts load the SVG
        // through an <object>, and svg-pan-zoom init (plus click-to-card) lived inside that
        // object's 'load' listener. These substrings together prove the init function is NAMED and
        // reached on at least one of two converging paths -- the immediate call OR the 'load'
        // subscription -- rather than only subscribed (and silently skipped when the object had
        // already finished loading before DOMContentLoaded fired).
        //
        // CRITICAL: a readyState === 'complete' check ALONE is insufficient. The <object>'s
        // contentDocument can report readyState 'complete' while its documentElement is still
        // <html> (the SVG resource not yet parsed) -- at which point svg-pan-zoom resolves the
        // svg to a <html> element, createSVGMatrix is undefined, and init throws. The guard must
        // ALSO confirm documentElement is the <svg> root. That is the tagName check below.
        private static readonly string[] RequiredRaceGuard =
        {
            "var initSvgViewer = function () {",
            "__svgDoc.readyState === 'complete'",
            "__svgDoc.documentElement.tagName.toLowerCase() === 'svg'",
            "initSvgViewer();",
            "svgObject.addEventListener('load', initSvgViewer)"
        };

        // The legacy racy pattern that #1040 replaces: an anonymous 'load' handler subscribed with
        // no readyState guard. Present-but-never-executed is precisely the failure mode this guards.
        private const string LegacyRacyLoadSubscription = "svgObject.addEventListener('load', function () {";

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

        /// <summary>
        /// Capability #11 of issue #830 (added by #1040). The 16 <c>_ext</c> wrappers load their
        /// SVG through an <c>&lt;object&gt;</c> element; svg-pan-zoom init and click-to-card both
        /// lived inside that object's <c>'load'</c> listener, which was itself subscribed inside
        /// <c>DOMContentLoaded</c>. When the object finished loading before <c>DOMContentLoaded</c>
        /// fired (warm cache, fast local serve) the <c>'load'</c> event was already gone and
        /// NOTHING executed: no zoom, no pan, no click-to-card, no console error -- the viewer was
        /// simply absent. Presence of the code is not execution; this test pins the fix.
        /// </summary>
        /// <remarks>
        /// <para>External path only (the <c>external.html</c> template + the 16 <c>_ext</c> wrappers).
        /// The embedded path (<c>included.html</c> + the 16 inline-SVG wrappers) inlines the SVG
        /// and has no <c>&lt;object&gt;</c> load race, so it is deliberately out of scope here.</para>
        /// <para>The guard asserts BOTH <c>readyState === 'complete'</c> AND
        /// <c>documentElement.tagName === 'svg'</c>. A readyState check alone is insufficient: the
        /// object's contentDocument can report 'complete' while its documentElement is still
        /// <c>&lt;html&gt;</c> (SVG resource not yet parsed), at which point svg-pan-zoom resolves
        /// the svg to a <c>&lt;html&gt;</c> node, <c>createSVGMatrix</c> is undefined, and init
        /// throws <c>createSVGMatrix is not a function</c>. The tagName check is what closes that.</para>
        /// <para>The behavioral complement (3 consecutive cold/hot loads, identical results) lives
        /// in the Playwright suite <c>MindmapWrapperTests.cs</c>.</para>
        /// </remarks>
        [Fact]
        public void ExternalMindmapWrappers_GuardObjectLoadRace_SoInitExecutes()
        {
            // external.html template + 16 _ext wrappers (8 langs × {Fallacies,Virtues}).
            var externalFiles = Directory.EnumerateFiles(MindmapsRoot, "*.html", SearchOption.AllDirectories)
                .OrderBy(f => f)
                .Where(f => Path.GetFileName(f).Equals("external.html", StringComparison.OrdinalIgnoreCase)
                            || Path.GetFileName(f).EndsWith("_ext.html", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var expectedCount = ExpectedExternalWrapperCount();
            expectedCount.Should().BeGreaterThanOrEqualTo(AntiCollapseFloor,
                "anti-collapse floor (#1285): 8 Fallacies + 8 Virtues registries + template is the pre-cards baseline — a lower expectation means documents went missing, not that scope shrank");
            externalFiles.Count.Should().Be(expectedCount,
                "external.html template + one _ext wrapper per enabled SVG-bearing document × (1 + |Translations|) — scope derived from the creator configs, never hardcoded (#1285)");

            var missing = new List<string>();
            var legacy = new List<string>();
            foreach (var file in externalFiles)
            {
                var content = File.ReadAllText(file);
                foreach (var needle in RequiredRaceGuard)
                {
                    if (!content.Contains(needle))
                        missing.Add($"{Path.GetFileName(file)}: missing '{needle}'");
                }
                // The racy anonymous 'load' subscription must be gone -- a file can carry all the
                // init code and still never run it, which is exactly the regression this prevents.
                if (content.Contains(LegacyRacyLoadSubscription))
                    legacy.Add($"{Path.GetFileName(file)}: still uses the racy anonymous 'load' subscription");
            }

            missing.Should().BeEmpty(
                "capability #11 (#830 / #1040): every external-path artifact must name its init and guard the <object> load race with a readyState + documentElement-is-svg check so init actually executes (readyState alone is insufficient -- the svg root may not be parsed yet)");
            legacy.Should().BeEmpty(
                "capability #11 (#830 / #1040): the racy anonymous 'load' subscription must be replaced by the named initSvgViewer + readyState/documentElement guard");
        }
    }
}
