using System;
using System.IO;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Mindmapper;
using Argumentum.AssetConverter.Tests;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests
{
    /// <summary>
    /// Behavioural golden-master instrument for the mindmap HTML wrapper (issue #830).
    ///
    /// The headless <c>MindmapWrapperGoldenMasterTests</c> proves capabilities #10/#11 by
    /// *substring presence* only. This suite proves the *runtime behaviour* the user actually
    /// sees when the wrapper is open in a browser:
    ///   #1  initial zoom is readable AND the requestAnimationFrame recentring (#829/#831) ran;
    ///   #2  recentring re-centers the SVG root node (id=0) in the viewport;
    ///   #3  drag pans the viewport;
    ///   #4  mouse-wheel zooms;
    ///   #6  double-click zooms;
    ///   #7  clicking a real semantic .node opens the overlay card.
    ///
    /// === THE ZOOM-INITIAL DELAY CAVEAT (the reason this suite exists) ===
    /// svg-pan-zoom initializes in fit-to-viewport, THEN the wrapper's requestAnimationFrame
    /// (included.html) zooms to ~2600 user-units of height and recenters on the root node. For a
    /// 2.4–5 MB inline SVG (1400+ nodes) the parse can take a second or more, so the *first*
    /// CTM sample often measures a transient fit-scale — a regression #831 (initial zoom too
    /// distant to read) would look GREEN if you sampled once at the first tick. This suite
    /// therefore NEVER measures a single first sample: every assertion runs through
    /// <see cref="WaitForViewportSettledAsync"/> which polls until the viewport scale is stable
    /// across two consecutive samples (or times out). Any future harness measuring cap #1 must
    /// copy this settle-wait, else it produces a false positive.
    ///
    /// Composed via the same <see cref="MindMapHtmlWrapper.FormatWrapper"/> path the pipeline uses
    /// (committed template + committed .content.svg), so a regression in the helper surfaces here.
    /// </summary>
    public class MindmapWrapperCapabilitiesTests : IAsyncLifetime
    {
        private readonly ITestOutputHelper _output;
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;
        private string _tempDir = null!;

        private static readonly string RepoRoot = TestRepoRoot.Find();

        private static readonly string IncludedTemplatePath =
            Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", "included.html");

        private const int ViewportWidth = 1400;
        private const int ViewportHeight = 900;

        // Settle-wait constants (the zoom-initial delay caveat).
        private const double SettleTolerance = 1e-6;
        private const double SettleMinIntervalMs = 60;
        private const double SettleMaxWaitMs = 15000;

        public MindmapWrapperCapabilitiesTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            _tempDir = Path.Combine(Path.GetTempPath(), "argumentum-mm-caps-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public async Task DisposeAsync()
        {
            if (_browser != null) await _browser.CloseAsync();
            _playwright?.Dispose();
            try { if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true); }
            catch { /* best-effort temp cleanup */ }
        }

        private static string GetSvgPath(string lang, string fileName)
            => Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", lang, fileName);

        // ---- helpers -------------------------------------------------------

        /// <summary>
        /// Compose an inline variant (template + real .content.svg) and write it to the temp dir.
        /// This is exactly what the pipeline writes under Cards/Fallacies/Mindmaps/{lang}/.
        /// </summary>
        private async Task<string> ComposeIncludedAsync(string lang, string svgFileName)
        {
            Assert.True(File.Exists(IncludedTemplatePath), $"Missing template: {IncludedTemplatePath}");
            var svgPath = GetSvgPath(lang, svgFileName);
            Assert.True(File.Exists(svgPath), $"Missing SVG fixture: {svgPath}");

            var template = await File.ReadAllTextAsync(IncludedTemplatePath);
            var svg = await File.ReadAllTextAsync(svgPath);
            var wrapper = MindMapHtmlWrapper.FormatWrapper(template, svgFileName, svg);

            // Hard guarantee the placeholders never ship.
            Assert.DoesNotContain("[SVGCONTENT]", wrapper);
            Assert.DoesNotContain("[SVGPATH]", wrapper);

            var wrapperPath = Path.Combine(_tempDir, $"caps_{lang}_{svgFileName.Replace('.', '_')}.html");
            await File.WriteAllTextAsync(wrapperPath, wrapper);
            return wrapperPath;
        }

        /// <summary>Open a wrapper at a FIXED viewport, waiting for the full load (not just DOMContentLoaded).</summary>
        private async Task<IPage> OpenPageAsync(string wrapperPath)
        {
            var page = await _browser.NewPageAsync(new BrowserNewPageOptions
            {
                ViewportSize = new ViewportSize { Width = ViewportWidth, Height = ViewportHeight }
            });
            await page.GotoAsync("file:///" + wrapperPath.Replace('\\', '/'));
            await page.WaitForLoadStateAsync(LoadState.Load);
            return page;
        }

        private static string _transformScaleJs = @"el => { const m = el.getCTM(); return m ? m.a : null; }";

        private Task<double?> GetViewportScaleAsync(IPage page)
            => page.Locator(".svg-pan-zoom_viewport").EvaluateAsync<double?>(_transformScaleJs);

        private static readonly string _transformPanJs =
            @"el => { const m = el.getCTM(); return m ? [m.e, m.f] : [0, 0]; }";

        private async Task<(double x, double y)> GetViewportPanAsync(IPage page)
        {
            var arr = await page.Locator(".svg-pan-zoom_viewport").EvaluateAsync<double[]>(_transformPanJs);
            return (arr.Length >= 2 ? arr[0] : 0, arr.Length >= 2 ? arr[1] : 0);
        }

        private static readonly string _textStatsJs =
            @"() => {
                const texts = Array.from(document.querySelectorAll('#mindmap svg text'));
                const heights = texts
                    .map(t => { try { return t.getBBox().height; } catch (e) { return 0; } })
                    .filter(v => v > 0 && v < 500)
                    .sort((a, b) => a - b);
                if (!heights.length) return [0, 0, 0];
                const mid = Math.floor(heights.length / 2);
                const median = heights.length % 2 ? heights[mid] : (heights[mid - 1] + heights[mid]) / 2;
                const readable = heights.filter(v => v >= 9).length;
                return [heights.length, median, readable];
            }";

        /// <summary>
        /// THE zoom-initial cable: poll the viewport scale until it stays constant across two
        /// consecutive samples, then return the settled value. See the caveat in the class doc.
        /// </summary>
        private async Task<(double scale, double elapsedMs)> WaitForViewportSettledAsync(IPage page)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            double? prev = null;
            int stableRuns = 0;
            double settled = double.NaN;

            while (sw.ElapsedMilliseconds < SettleMaxWaitMs)
            {
                var s = await GetViewportScaleAsync(page);
                if (s.HasValue)
                {
                    if (prev.HasValue && Math.Abs(s.Value - prev.Value) < SettleTolerance)
                    {
                        stableRuns++;
                        if (stableRuns >= 2)
                        {
                            settled = s.Value;
                            break;
                        }
                    }
                    else
                    {
                        stableRuns = 0;
                    }
                    prev = s;
                }
                else
                {
                    stableRuns = 0;
                }
                await page.WaitForTimeoutAsync((int)SettleMinIntervalMs);
            }

            _output.WriteLine($"WaitForViewportSettled: scale={settled:F6} after {sw.ElapsedMilliseconds}ms");
            return (settled, sw.ElapsedMilliseconds);
        }

        // ---- #1: initial zoom readable + recentring ran ---------------------

        // fr + ar + zh cover Latin, RTL, CJK; the Virtues family was regenerated by #983 across
        // all languages, so the pre-regen golden master must cover it too (#830).
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        [InlineData("ar", "Fallacies_ar.content.svg")]
        [InlineData("zh", "Fallacies_zh.content.svg")]
        [InlineData("fr", "Argumentum_Virtues_MindMap_fr.content.svg")]
        public async Task Cap1_InitialZoom_AfterSettle_ExceedsFit_RecentringRan(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                var (scaleStable, _) = await WaitForViewportSettledAsync(page);
                Assert.True(scaleStable > 0, $"no positive settled scale for {lang}: {scaleStable}");

                // Reference: reset() returns the library's ORIGINAL state. With fit disabled the
                // original zoom equals the fit-to-viewport scale computed from the SVG viewBox.
                await page.ClickAsync("#svg-pan-zoom-reset-pan-zoom");
                await page.WaitForTimeoutAsync(300);
                var scaleFit = await GetViewportScaleAsync(page);
                Assert.True(scaleFit.HasValue && scaleFit.Value > 0, $"no positive fit scale for {lang}");

                var ratio = scaleStable / scaleFit.Value;
                // A regression #831 (no recentring) would leave the wrapper DOWN at the fit scale:
                // ratio ≈ 1.0. The recentring must zoom in at least 1.5× so the taxonomy reads.
                // (fr/ar/zh measure ≈×7 — the 1.5 floor is the falsifiable bar, not the observed.)
                Assert.True(ratio >= 1.5,
                    $"initial zoom must exceed fit by ≥1.5× (recentring #831), got {ratio:F2} for {lang}");

                // Readability measurement (information + a very permissive floor; the hard gate is
                // the ratio above. Font availability in headless can shift absolute heights).
                var stats = await page.Locator("#mindmap svg").EvaluateAsync<double[]>(_textStatsJs);
                var count = stats.Length >= 1 ? stats[0] : 0;
                var median = stats.Length >= 2 ? stats[1] : 0;
                var readable = stats.Length >= 3 ? stats[2] : 0;
                _output.WriteLine($"[{lang}] scaleStable={scaleStable:F5} fit={scaleFit.Value:F5} " +
                    $"ratio={ratio:F2} textVisible={count} medianHeight={median:F2} readable(≥9px)={readable}");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ---- #2: recentring centers the root node --------------------------

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        [InlineData("zh", "Fallacies_zh.content.svg")]
        public async Task Cap2_Recentring_CentersRootNode_InViewport(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                // The recentring recenters on g.node[id="0"] (the root). Its visible centre must sit
                // near the centre of the on-screen canvas (#mindmap, the 1400×900 container the
                // user actually sees), within 10% of the smaller dimension. js returns [dx, dy, mw, mh].
                var centre = await page.Locator("#mindmap svg g.node[id=\"0\"]")
                    .EvaluateAsync<double[]>(@"
                        (el) => {
                            const r = el.getBoundingClientRect();
                            const c = document.querySelector('#mindmap').getBoundingClientRect();
                            const cx = r.left + r.width / 2;
                            const cy = r.top + r.height / 2;
                            return [Math.abs(cx - (c.left + c.width / 2)),
                                    Math.abs(cy - (c.top + c.height / 2)),
                                    c.width, c.height];
                        }");
                Assert.True(centre.Length >= 4, $"center probe returned {centre.Length} values for {lang}");
                var dx = centre[0]; var dy = centre[1]; var mw = centre[2]; var mh = centre[3];
                var floor = Math.Min(mw, mh) * 0.10;
                _output.WriteLine($"[{lang}] root-centre offset dx={dx:F1} dy={dy:F1} canvas={mw:F0}x{mh:F0} floor={floor:F1}px");
                Assert.True(dx < floor, $"root not horizontally centered for {lang}: dx={dx:F1} vs floor {floor:F1}px");
                Assert.True(dy < floor, $"root not vertically centered for {lang}: dy={dy:F1} vs floor {floor:F1}px");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ---- #3: drag pans ------------------------------------------------

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap3_Drag_PansViewport(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                var before = await GetViewportPanAsync(page);
                _output.WriteLine($"[{lang}] pan before x={before.x:F1} y={before.y:F1}");

                // Drag from the SVG centre (away from the control icons at bottom-right).
                await page.Mouse.MoveAsync(ViewportWidth / 2, ViewportHeight / 2);
                await page.Mouse.DownAsync();
                await page.Mouse.MoveAsync(ViewportWidth / 2 + 120, ViewportHeight / 2 + 90, new MouseMoveOptions { Steps = 8 });
                await page.Mouse.UpAsync();
                await page.WaitForTimeoutAsync(150);

                var after = await GetViewportPanAsync(page);
                _output.WriteLine($"[{lang}] pan after x={after.x:F1} y={after.y:F1}");
                Assert.True(Math.Abs(after.x - before.x) > 1 || Math.Abs(after.y - before.y) > 1,
                    $"drag should pan the viewport for {lang}: before({before.x:F1},{before.y:F1}) after({after.x:F1},{after.y:F1})");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ---- #4: mouse wheel zooms ----------------------------------------

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap4_Wheel_ZoomsViewport(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                var before = await GetViewportScaleAsync(page);
                Assert.True(before.HasValue, $"no initial scale for {lang}");

                // Hover the SVG (wheel handler is bound to the svg) then scroll up (zoom in).
                await page.Mouse.MoveAsync(ViewportWidth / 2, ViewportHeight / 2);
                await page.Mouse.WheelAsync(0, -240);
                await page.WaitForTimeoutAsync(200);

                var after = await GetViewportScaleAsync(page);
                _output.WriteLine($"[{lang}] wheel scale before={before.Value:F5} after={after.Value:F5}");
                Assert.True(after.HasValue && after.Value > before.Value,
                    $"mouse wheel should zoom in for {lang}: {before.Value:F5} -> {after.Value:F5}");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ---- #6: double-click zooms ---------------------------------------

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap6_DoubleClick_ZoomsViewport(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                var before = await GetViewportScaleAsync(page);
                Assert.True(before.HasValue, $"no initial scale for {lang}");

                // Double-click on the SVG body (svg-pan-zoom dblClickZoomEnabled). Focus a point
                // mid-canvas, away from the control icons.
                await page.Mouse.DblClickAsync(ViewportWidth / 2, ViewportHeight / 2);
                await page.WaitForTimeoutAsync(200);

                var after = await GetViewportScaleAsync(page);
                _output.WriteLine($"[{lang}] dblclick scale before={before.Value:F5} after={after.Value:F5}");
                Assert.True(after.HasValue && after.Value > before.Value,
                    $"double-click should zoom in for {lang}: {before.Value:F5} -> {after.Value:F5}");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ---- #7: clicking a real semantic node opens the overlay card ----

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap7_ClickRealNode_OpensOverlayCard(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                // The real committed SVGs DO carry semantic .node markers (1400 for Fallacies_fr).
                // The root node (id=0) is centered by the recentring, hence clicked.
                var root = page.Locator("#mindmap svg g.node[id=\"0\"]");
                Assert.True(await root.CountAsync() >= 1, $"no root .node for {lang}");

                var family = await root.GetAttributeAsync("family");
                Assert.False(string.IsNullOrWhiteSpace(family), $"root .node has no family attribute for {lang}");

                await root.ClickAsync();
                await page.WaitForTimeoutAsync(150);

                var famille = await page.Locator("card .famille").EvaluateAsync<string?>("el => el.textContent");
                _output.WriteLine($"[{lang}] overlay .famille after click = '{famille}' (node family='{family}')");
                Assert.False(string.IsNullOrWhiteSpace(famille), "overlay card .famille should be populated after clicking a .node");

                var hidden = await page.Locator("card").EvaluateAsync<bool>("el => el.classList.contains('hidden')");
                Assert.False(hidden, "overlay card should become visible after clicking a .node");
            }
            finally
            {
                await page.CloseAsync();
            }
        }
    }
}
