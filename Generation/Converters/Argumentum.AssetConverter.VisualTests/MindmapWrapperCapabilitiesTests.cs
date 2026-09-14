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
    ///   #5  the three control icons exist, are clickable, and each MOVES the viewport;
    ///   #6  double-click zooms;
    ///   #7  clicking a real semantic .node opens the overlay card;
    ///   #8  the clicked node's family class reaches the overlay card and a family
    ///       palette colour actually computes (measured, never guessed — see Cap8);
    ///   #9  reset and the maximal zoom-out are TWO DISTINCT STATES, related by the template's
    ///       declared minZoom — the golden-master wording ("reset = zoom-out max") conflated them.
    ///       NOTE: #9 does NOT independently establish the fit. See the Cap 9 docstring.
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
    /// AMENDED RULE for anything that MOVES the viewport (a control click, a reset): "two
    /// identical samples" is NOT sufficient on its own. Immediately after the click the
    /// pre-movement state is itself stable, so two samples of it read as "settled" and record the
    /// PREVIOUS action's state. Those reads go through
    /// <see cref="WaitForSettledAfterActionAsync"/>, which waits for a CHANGE first and only then
    /// for two identical samples.
    ///
    /// ⚠️ PROVENANCE OF THE "DEFERRED RESET" — read before citing a duration. The deferral that
    /// motivated the gate was measured on ANOTHER harness, not this one (ai-01, #830
    /// c.5651920638: transition at t≈1 048 ms, settle 1 365–1 422 ms, against 310–390 ms for
    /// wheel/drag). It is NOT reproduced here: re-measured on this harness 2026-09-14 (fr, both
    /// fixtures, click → first CTM change) the delay is **12 ms** and **57 ms**, and reset lands
    /// directly on its final value — no transition is observable. The repo CI's previous fixed
    /// 300 ms read stayed green for exactly that reason. The gate is therefore kept as a
    /// ROBUSTNESS measure — one extra poll, and it closes a whole failure class — not because this
    /// suite observes a ~1 s deferral. Never cite the ~1 s figure as this suite's measurement.
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

        private static bool _chromiumEnsureAttempted;

        /// <summary>
        /// A CI runner carries no preinstalled browser, so the suite provisions Chromium itself
        /// once per process — the pattern the Tests suites already use (ImageConversion,
        /// PdfAssembly). Keeping it here rather than in the workflow leaves the suite runnable on
        /// any checkout, and keeps a failed download a loud test failure, never a silent skip.
        /// </summary>
        private static void EnsureChromium()
        {
            if (_chromiumEnsureAttempted) return;
            _chromiumEnsureAttempted = true;
            if (Microsoft.Playwright.Program.Main(new[] { "install", "chromium" }) != 0)
                throw new InvalidOperationException(
                    "Playwright chromium install failed — the behavioural wrapper suite cannot run.");
        }

        public async Task InitializeAsync()
        {
            EnsureChromium();
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

        /// <summary>
        /// The zoom-out bound the template DECLARES, read from the fixture rather than hard-coded.
        /// Cap 9's ratio assertion is anchored on this constant: it is the one quantity in that
        /// test that does not come from the observation under test.
        /// </summary>
        private static double DeclaredMinZoom()
        {
            var m = System.Text.RegularExpressions.Regex.Match(
                File.ReadAllText(IncludedTemplatePath), @"minZoom\s*:\s*([0-9.]+)");
            Assert.True(m.Success, $"no minZoom declared in {IncludedTemplatePath}");
            return double.Parse(m.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
        }

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
                // Rendered (screen) heights: getBoundingClientRect applies the svg-pan-zoom viewport
                // CTM, getBBox alone would return user units and read green even at fit-to-viewport
                // scale (the exact false negative the #830 capacity-1 refinement closes).
                const heights = texts
                    .map(t => { try { return t.getBoundingClientRect().height; } catch (e) { return 0; } })
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

        /// <summary>
        /// Settle-wait for an action that MOVES the viewport (a control click, a reset). Same
        /// two-consecutive-samples rule as <see cref="WaitForViewportSettledAsync"/>, but gated on
        /// the action having actually taken effect first.
        ///
        /// WHY THE GATE EXISTS: right after the click the viewport may not have started moving yet,
        /// so "two consecutive identical samples" is satisfied by the PRE-MOVEMENT state — and the
        /// harness records the PREVIOUS action's state instead of the new one. Requiring a change
        /// first is what separates "not started yet" from "arrived".
        ///
        /// The rule was amended after a deferral was observed on ANOTHER harness (ai-01, #830
        /// c.5651920638: transition at t≈1 048 ms, settle 1 365-1 422 ms, vs 310-390 ms for
        /// wheel/drag). ⚠️ That deferral does NOT reproduce here, and the ~1 s figure must not be
        /// cited as this suite's measurement: re-measured on this harness 2026-09-14 (fr, both
        /// fixtures, click → first CTM change) the delay is 12 ms and 57 ms, with reset landing
        /// directly on its final value. The gate is kept as ROBUSTNESS against the class of bug,
        /// not to work around a delay this suite actually sees.
        ///
        /// The amended rule: wait for a CHANGE, THEN two identical samples — never "two identical
        /// samples" alone.
        /// </summary>
        /// <param name="fromScale">
        /// The scale the action must move AWAY from. Null skips the gate (use only when the action
        /// is known to already be in flight).
        /// </param>
        private async Task<(double scale, bool changed, double elapsedMs)> WaitForSettledAfterActionAsync(
            IPage page, double? fromScale)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var sawChange = !fromScale.HasValue;
            double? prev = null;
            int stableRuns = 0;
            double settled = double.NaN;

            while (sw.ElapsedMilliseconds < SettleMaxWaitMs)
            {
                var s = await GetViewportScaleAsync(page);
                if (s.HasValue)
                {
                    if (!sawChange && Math.Abs(s.Value - fromScale!.Value) > SettleTolerance)
                    {
                        sawChange = true;
                    }

                    if (sawChange && prev.HasValue && Math.Abs(s.Value - prev.Value) < SettleTolerance)
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

            _output.WriteLine($"WaitForSettledAfterAction: from={fromScale?.ToString("F6") ?? "(none)"} " +
                $"-> scale={settled:F6} changed={sawChange} after {sw.ElapsedMilliseconds}ms");
            return (settled, sawChange, sw.ElapsedMilliseconds);
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

                // Readability measurement. Refined criterion (#830, jsboige 2026-08-10): once the
                // scale has settled, the median RENDERED text height must reach 10px — this is the
                // member that separates "recentred on the root, taxonomy readable" from "whole map
                // fits the window, nothing reads" (fit-only settles around 1-4px). Measured BEFORE
                // the reset click: reset drops the viewport to fit-to-viewport, and measuring after
                // it would assert readability on exactly the unreadable state.
                var stats = await page.Locator("#mindmap svg").EvaluateAsync<double[]>(_textStatsJs);
                var count = stats.Length >= 1 ? stats[0] : 0;
                var median = stats.Length >= 2 ? stats[1] : 0;
                var readable = stats.Length >= 3 ? stats[2] : 0;

                // Reference: reset() returns the library's ORIGINAL state — the fit-to-viewport
                // scale. It is read through the change-gated settle, NOT a fixed sleep. A fixed
                // sleep is only correct while the reset happens to land faster than the sleep —
                // measured here at 12-57 ms, so the previous 300 ms passed — and it silently
                // substitutes the ZOOMED scale as the fit reference the moment a reset is
                // deferred, which drives the ratio below to ≈1,0 and fails while accusing the
                // wrong thing. The change gate is correct either way.
                // ⚠️ This PR therefore DOES modify Cap 1: fixed 300 ms sleep -> gated settle, plus
                // a new `fitChanged` assertion. It is an improvement, not an absence of change.
                await page.Locator("#svg-pan-zoom-reset-pan-zoom").ClickAsync();
                var (fitScale, fitChanged, _) = await WaitForSettledAfterActionAsync(page, scaleStable);
                Assert.True(fitChanged,
                    $"reset must move the viewport away from the settled initial zoom for {lang} " +
                    $"(settled={scaleStable:F6}, read-back={fitScale:F6}) — a deferred reset read as " +
                    $"'already settled' records the PRE-reset state");
                Assert.True(fitScale > 0, $"no positive fit scale for {lang}");
                double? scaleFit = fitScale;

                var ratio = scaleStable / scaleFit.Value;
                // A regression #831 (no recentring) would leave the wrapper DOWN at the fit scale:
                // ratio ≈ 1.0. The recentring must zoom in at least 1.5× so the taxonomy reads.
                // (fr/ar/zh measure ≈×7 — the 1.5 floor is the falsifiable bar, not the observed.)
                Assert.True(ratio >= 1.5,
                    $"initial zoom must exceed fit by ≥1.5× (recentring #831), got {ratio:F2} for {lang}");

                _output.WriteLine($"[{lang}] scaleStable={scaleStable:F5} fit={scaleFit.Value:F5} " +
                    $"ratio={ratio:F2} textVisible={count} medianRenderedHeight={median:F2}px readable(≥9px)={readable}");
                Assert.True(median >= 10.0,
                    $"settled median rendered text height must be >= 10px for {lang}: {median:F2}px " +
                    $"(scale {scaleStable:F5}, fit {scaleFit.Value:F5} — if this fails with ratio OK, " +
                    $"check font rendering before declaring a recentring regression)");
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

        // ---- #8: the family class reaches the overlay card and a palette colour fires ----

        /// <summary>
        /// The mindmap palette has no single written spec (unlike the CardPen family colours
        /// documented in CLAUDE.md), so this capability is instrumented WITHOUT guessing any
        /// colour. What the wrapper contract actually promises on click (included.html):
        /// the overlay's class attribute is wiped, then the clicked node's `familyclass`
        /// attribute is added to the overlay `card`, which selects a `card.&lt;family&gt;` rule
        /// that drives `--color-background` — consumed as a real background by `card .texte`.
        /// The test asserts that mechanism: class applied + a non-transparent, non-white
        /// background computing on the card body. A missing family rule still passes through
        /// the documented fallback colour; a broken class application fails the first assert.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap8_ClickNode_AppliesFamilyClassAndColoursOverlay(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                var root = page.Locator("#mindmap svg g.node[id=\"0\"]");
                Assert.True(await root.CountAsync() >= 1, $"no root .node for {lang}");

                var familyClass = await root.GetAttributeAsync("familyclass");
                Assert.False(string.IsNullOrWhiteSpace(familyClass),
                    $"root .node has no familyclass attribute for {lang}");

                var overlay = page.Locator("card");
                var startsHidden = await overlay.EvaluateAsync<bool>("el => el.classList.contains('hidden')");
                Assert.True(startsHidden, "overlay card should start hidden before any click");

                await root.ClickAsync();
                await page.WaitForTimeoutAsync(150);

                var applied = await overlay.EvaluateAsync<bool>(
                    "(el, cls) => el.classList.contains(cls)", familyClass);
                Assert.True(applied,
                    $"overlay card should carry the clicked node's familyclass '{familyClass}' for {lang}");

                // The colour lives on the card body (.texte consumes var(--color-background)),
                // not on the <card> element itself — measuring the card would always read
                // transparent regardless of the palette working.
                var texte = page.Locator("card .texte");
                Assert.True(await texte.CountAsync() >= 1, $"no .texte body inside overlay card for {lang}");
                var bg = await texte.First.EvaluateAsync<string>("el => getComputedStyle(el).backgroundColor");
                _output.WriteLine($"[{lang}] familyclass '{familyClass}' -> card .texte background '{bg}'");
                Assert.False(string.IsNullOrEmpty(bg) || bg == "rgba(0, 0, 0, 0)",
                    $"a palette rule must compute a real background for familyclass '{familyClass}', got '{bg}'");
                Assert.True(bg != "rgb(255, 255, 255)",
                    $"family palette must not fall back to white for familyclass '{familyClass}', got '{bg}'");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ---- #5: the control icons are present, clickable AND effective -----

        /// <summary>
        /// Capability #5 of #830. Regression #825 had replaced a working svg-pan-zoom with an
        /// inert <c>transform:scale()</c>; presence of the control cluster proves nothing about what
        /// the buttons DO. Three assertions of increasing strength:
        ///   (a) the cluster and each of its three icons exists;
        ///   (b) each icon is hit-testable (a real, non-degenerate box);
        ///   (c) each icon MOVES the viewport, in its own direction.
        /// Before this test, Cap 5 was pinned only by substring presence in the headless golden
        /// master, and the effectiveness of zoom-in/zoom-out rested on an external pass, never on
        /// this suite (see the #830 verdict of 2026-09-13).
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        [InlineData("fr", "Argumentum_Virtues_MindMap_fr.content.svg")]
        public async Task Cap5_ControlIcons_PresentClickableAndEffective(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                var (scaleStable, _) = await WaitForViewportSettledAsync(page);
                Assert.True(scaleStable > 0, $"no positive settled scale for {lang}");

                const string cluster = "#svg-pan-zoom-controls";
                const string zoomIn = "#svg-pan-zoom-zoom-in";
                const string zoomOut = "#svg-pan-zoom-zoom-out";
                const string reset = "#svg-pan-zoom-reset-pan-zoom";

                // (a) presence.
                foreach (var id in new[] { cluster, zoomIn, zoomOut, reset })
                {
                    Assert.True(await page.Locator(id).CountAsync() == 1,
                        $"{id} must be present exactly once for {lang}");
                }
                var icons = await page.Locator($"{cluster} > g").CountAsync();
                Assert.True(icons == 3, $"expected 3 control icons for {lang}, got {icons}");

                // (b) clickable — presence is not clickability.
                foreach (var id in new[] { zoomIn, zoomOut, reset })
                {
                    Assert.True(await page.Locator(id).IsVisibleAsync(),
                        $"{id} is present but not visible for {lang}");
                    // A box of all zeros — or no box at all — is what Playwright returns for an
                    // element it cannot lay out: presence in the DOM is not a hit box.
                    var box = await page.Locator(id).BoundingBoxAsync();
                    Assert.True(box is { Width: > 0, Height: > 0 },
                        $"{id} has no hit box for {lang} (present but not clickable): {box?.Width}x{box?.Height}");
                }

                // (c) effective. Every read goes through the change-gated settle: an icon whose
                // effect is deferred would otherwise be recorded as "unchanged" and misread as inert.
                await page.Locator(zoomIn).ClickAsync();
                var (afterIn, inMoved, _) = await WaitForSettledAfterActionAsync(page, scaleStable);
                _output.WriteLine($"[{lang}] zoom-in {scaleStable:F6} -> {afterIn:F6}");
                Assert.True(inMoved && afterIn > scaleStable,
                    $"zoom-in control must increase the scale for {lang}: {scaleStable:F6} -> {afterIn:F6}");

                await page.Locator(zoomOut).ClickAsync();
                var (afterOut, outMoved, _) = await WaitForSettledAfterActionAsync(page, afterIn);
                _output.WriteLine($"[{lang}] zoom-out {afterIn:F6} -> {afterOut:F6}");
                Assert.True(outMoved && afterOut < afterIn,
                    $"zoom-out control must decrease the scale for {lang}: {afterIn:F6} -> {afterOut:F6}");

                // Reset returns to the library's ORIGINAL state — the fit-to-viewport scale. The
                // harness starts ABOVE the fit (the recentring zooms in ~2,2-7,7x), so a working
                // reset moves the scale DOWN, not up. Cap 9 owns the reset-vs-zoom-out bound
                // distinction; here the claim is only effectiveness — the icon moves the viewport.
                await page.Locator(reset).ClickAsync();
                var (afterReset, resetMoved, _) = await WaitForSettledAfterActionAsync(page, afterOut);
                _output.WriteLine($"[{lang}] reset {afterOut:F6} -> {afterReset:F6}");
                Assert.True(resetMoved && afterReset < afterOut,
                    $"reset control must move the viewport back toward the fit for {lang}: " +
                    $"{afterOut:F6} -> {afterReset:F6} (the harness settles above the fit, so a " +
                    $"working reset reads as a DECREASE)");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ---- #9: reset and the maximal zoom-out are two DISTINCT states -----

        /// <summary>
        /// Capability #9 of #830: reset and the maximal zoom-out are TWO DISTINCT STATES.
        ///
        /// ⚠️ WHAT THIS TEST DOES **NOT** PROVE — do not rename it back to "lands on the fit".
        /// The post-reset value (<c>fit</c> below) is merely the scale observed after the reset;
        /// nothing independent establishes that it equals the fit-to-viewport scale. An independent
        /// reference was attempted and FAILED to reproduce it: a geometric fit, derived from the
        /// viewport group's <c>getBBox()</c> against the container box, lands 1,0-1,5 % away from
        /// the observed post-reset scale (measured 2026-09-14 on this harness — 0,141989 vs
        /// 0,144136 on the Virtues fixture, 0,041447 vs 0,041850 on Fallacies). The cause is that
        /// svg-pan-zoom snapshots its sizes at construction, before the wrapper's CSS is applied,
        /// so no post-hoc geometry reproduces it. <c>getSizes().realZoom</c> would be the correct
        /// reference, but the wrapper keeps <c>panZoomInstance</c> in a closure and never exposes
        /// it. The claim asserted here is therefore the DISTINCTION, not the landing point.
        ///
        /// What IS proven, and why it is still worth pinning:
        ///   · the golden-master wording carried a false equivalence — "reset revient au fit complet
        ///     (zoom-out max)" — conflating two states. Read from the vendored library (svg-pan-zoom
        ///     3.6.2, shipped inside the wrapper): <c>resetZoom()</c> re-reads
        ///     <c>getOriginalState()</c> and calls <c>zoom(t.zoom, true)</c>, while
        ///     <c>zoomAtPoint()</c> clamps with <c>minZoom * n.zoom</c> — so the zoom-out bound is
        ///     RELATIVE to the reference the reset returns to;
        ///   · the two observed states are strictly ordered (<c>floor &lt; fit</c>) and their ratio
        ///     equals the template's DECLARED <c>minZoom</c> — a constant read from the fixture,
        ///     not from the observation. A ratio of 1,0 would mean the golden-master wording was
        ///     right after all.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        [InlineData("fr", "Argumentum_Virtues_MindMap_fr.content.svg")]
        public async Task Cap9_ResetAndZoomOutMaxAreDistinctStates(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                var (zoomed, _) = await WaitForViewportSettledAsync(page);
                Assert.True(zoomed > 0, $"no positive settled scale for {lang}");

                // (1) reset lands ON the fit.
                await page.Locator("#svg-pan-zoom-reset-pan-zoom").ClickAsync();
                var (fit, fitMoved, _) = await WaitForSettledAfterActionAsync(page, zoomed);
                Assert.True(fitMoved, $"reset must move the viewport for {lang}");
                Assert.True(fit < zoomed,
                    $"reset must zoom OUT from the settled initial zoom for {lang}: {zoomed:F6} -> {fit:F6}");

                // (2) the zoom-out BOUND is a different, strictly further-out state. Click down to
                // the clamp; the trailing clicks are no-ops once minZoom is reached.
                for (var k = 0; k < 40; k++)
                {
                    await page.Locator("#svg-pan-zoom-zoom-out").ClickAsync();
                    await page.WaitForTimeoutAsync(60);
                }
                var (floorScale, floorMoved, _) = await WaitForSettledAfterActionAsync(page, fit);
                var ratio = floorScale / fit;

                _output.WriteLine($"[{lang}] fit={fit:F6} zoomOutFloor={floorScale:F6} ratio(floor/fit)={ratio:F3}");

                // The discriminating assertion. Asserting `moved` FIRST (rather than comparing a
                // NaN read-back) is what makes the failure legible: if zoom-out cannot leave the
                // fit, the two states are one and the golden-master wording was right after all.
                Assert.True(floorMoved,
                    $"the zoom-out control must move the viewport strictly BELOW the fit for {lang} " +
                    $"(fit={fit:F6}, no further movement observed) — if it cannot, 'reset' and " +
                    $"'zoom-out max' really are one state and the golden-master wording was right");
                Assert.True(floorScale < fit,
                    $"the maximal zoom-out must be strictly further out than reset for {lang}: " +
                    $"floor={floorScale:F6} vs fit={fit:F6}");

                // The discriminating bar, anchored on the template's DECLARED minZoom — a fixture
                // constant, not a band drawn around the observed value. The ratio equals minZoom
                // exactly when reset returns to the reference the zoom-out clamp is relative to; a
                // ±10 % tolerance absorbs the quantised CTM reads. Ratio 1,0 (the golden-master
                // equivalence) sits far outside it, and a silently edited minZoom moves the bar
                // with the fixture instead of turning this red for the wrong reason.
                var declaredMinZoom = DeclaredMinZoom();
                _output.WriteLine($"[{lang}] declared minZoom={declaredMinZoom:F3} observed ratio={ratio:F4}");
                Assert.InRange(ratio, declaredMinZoom * 0.9, declaredMinZoom * 1.1);
            }
            finally
            {
                await page.CloseAsync();
            }
        }
    }
}
