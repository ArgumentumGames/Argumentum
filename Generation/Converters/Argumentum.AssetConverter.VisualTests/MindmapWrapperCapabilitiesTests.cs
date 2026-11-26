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
            var wrapper = MindMapHtmlWrapper.FormatWrapper(template, svgFileName, svg, lang, "Argumentum");

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

        private static readonly string ExternalTemplatePath =
            Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", "external.html");

        /// <summary>Same page, at an explicit viewport — the #2400 controls run desktop AND phone.</summary>
        private async Task<IPage> OpenPageAtAsync(string wrapperPath, int width, int height)
        {
            var page = await _browser.NewPageAsync(new BrowserNewPageOptions
            {
                ViewportSize = new ViewportSize { Width = width, Height = height }
            });
            await page.GotoAsync("file:///" + wrapperPath.Replace('\\', '/'));
            await page.WaitForLoadStateAsync(LoadState.Load);
            return page;
        }

        /// <summary>
        /// Compose the `_ext` variant (external.html) and serve it over a routed http origin.
        ///
        /// Under `file://` Chromium gives each file an opaque origin, so `svgObject.contentDocument`
        /// — the object the wrapper's own JS reads — is not reachable from the page; a test
        /// measuring it there would measure the harness, not the wrapper. The route answers the
        /// wrapper and its SVG and leaves everything else alone.
        /// </summary>
        private async Task<IPage> OpenExternalAsync(string lang, string svgFileName, int width, int height)
        {
            Assert.True(File.Exists(ExternalTemplatePath), $"Missing template: {ExternalTemplatePath}");
            var template = await File.ReadAllTextAsync(ExternalTemplatePath);
            var wrapper = MindMapHtmlWrapper.FormatWrapper(
                template, svgFileName, string.Empty, lang, "Argumentum");
            Assert.DoesNotContain("[SVGPATH]", wrapper);
            Assert.DoesNotContain("[SVGCONTENT]", wrapper);

            var svgPath = GetSvgPath(lang, svgFileName);
            Assert.True(File.Exists(svgPath), $"Missing SVG fixture: {svgPath}");
            var svgBytes = await File.ReadAllBytesAsync(svgPath);

            var page = await _browser.NewPageAsync(new BrowserNewPageOptions
            {
                ViewportSize = new ViewportSize { Width = width, Height = height }
            });
            const string origin = "http://mm.test";
            await page.RouteAsync(origin + "/**", async route =>
            {
                var path = new Uri(route.Request.Url).AbsolutePath.TrimStart('/');
                if (path == "wrapper.html")
                {
                    await route.FulfillAsync(new RouteFulfillOptions
                    {
                        ContentType = "text/html; charset=utf-8",
                        Body = wrapper
                    });
                }
                else if (path == svgFileName)
                {
                    await route.FulfillAsync(new RouteFulfillOptions
                    {
                        ContentType = "image/svg+xml",
                        BodyBytes = svgBytes
                    });
                }
                else
                {
                    await route.ContinueAsync();
                }
            });
            await page.GotoAsync(origin + "/wrapper.html");
            await page.WaitForLoadStateAsync(LoadState.Load);
            return page;
        }

        /// <summary>
        /// `_ext` only. The SVG lives in the `<object>`'s document and svg-pan-zoom runs THERE,
        /// so the host-page `.svg-pan-zoom_viewport` the settle-wait reads does not exist. Wait
        /// instead for what the #2400 controls consume: the navigation index, which the wrapper
        /// builds last, once the object's document is parsed.
        /// </summary>
        private async Task WaitForExternalNavigationAsync(IPage page)
        {
            const string ready = @"() => {
                const host = document.getElementById('svgObject');
                const d = host && host.contentDocument;
                if (!d || !d.querySelectorAll('g.node').length) return false;
                const nav = window.__mmNav;
                return !!(nav && nav.byPk && Object.keys(nav.byPk).length);
            }";
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < SettleMaxWaitMs)
            {
                if (await page.EvaluateAsync<bool>(ready)) return;
                await page.WaitForTimeoutAsync(100);
            }
            throw new TimeoutException("the _ext wrapper's navigation index was not built within the settle budget");
        }

        /// <summary>
        /// #2400 — open the sheet of EVERY node of the map in the page and return the geometric
        /// violations. The node document is the `<object>`'s document for the `_ext` variant and
        /// the page itself for the inline one, exactly as the wrapper's own JS resolves it.
        ///
        /// Every threshold below was calibrated on the corpus before being written down
        /// (2026-11-26, `fr`/`ru`/`ar` × 1280x900 and 390x844, 705 relation-bearing sheets):
        /// "at least one whole relation row is reachable" holds on 705/705, while boxes showing
        /// fewer rows are capped by the template's own `max-height: 12vh` and scroll. The floor
        /// is therefore ONE row — a box too short to show one is the witness-2 defect (25 px for
        /// seven rows on #pk=1084).
        /// </summary>
        private const string CardFitSweepJs = @"() => {
            const host = document.getElementById('svgObject');
            const nodeDoc = (host && host.contentDocument) ? host.contentDocument : document;
            const all = Array.from(nodeDoc.querySelectorAll('g.node'));
            const nav = window.__mmNav;
            const card = document.querySelector('card');
            const q = (s) => card.querySelector(s);
            const rel = q('.relations'), foot = q('.footer'), body = q('.body'),
                  texte = q('.texte'), img = q('.imageSection');
            const link = document.getElementById('externalLink');
            const R = (el) => el.getBoundingClientRect();
            const out = { all: all.length, nodes: 0, withRelations: 0, withLink: 0,
                          withoutLink: 0, violations: [], tally: {} };
            const NOTE = (name, key) => {
                out.tally[name] = (out.tally[name] || 0) + 1;
                if (out.violations.length < 12) out.violations.push(key + ':' + name);
            };
            const tol = 0.6;
            for (const n of all) {
                const rec = nav.byPk[n.getAttribute('pk')] || nav.byPath[n.getAttribute('path')];
                if (!rec) continue;
                const key = n.getAttribute('pk') || n.getAttribute('path') || '?';
                nav.focusNode(rec);
                out.nodes++;
                const cr = R(card), br = R(body), ir = R(img), tr = R(texte), fr = R(foot), rr = R(rel);
                if (br.top < cr.top - tol || br.bottom > cr.bottom + tol) NOTE('bodyOutsideCard', key);
                if (fr.top < cr.top - tol || fr.bottom > cr.bottom + tol
                    || fr.left < cr.left - tol || fr.right > cr.right + tol) NOTE('footerOutsideCard', key);
                if (card.scrollHeight > card.clientHeight + 1 || card.scrollWidth > card.clientWidth + 1)
                    NOTE('cardScrolls', key);
                if (ir.bottom > tr.top + 0.5) NOTE('titleBandOverlapsText', key);
                // A long title: cut by its own box, or leaving the image section it belongs to.
                const title = q('.title'), header = q('.header');
                const trc = R(title), hr = R(header);
                if (title.scrollHeight > title.clientHeight + 1 || title.scrollWidth > title.clientWidth + 1)
                    NOTE('titleClipped', key);
                if (trc.top < ir.top - tol || trc.bottom > ir.bottom + tol) NOTE('titleOutsideImageSection', key);
                if (hr.top < cr.top - tol || hr.bottom > cr.bottom + tol) NOTE('headerOutsideCard', key);
                if (!texte.classList.contains('mm-scroll') && texte.scrollHeight > texte.clientHeight + 1)
                    NOTE('textClipped', key);

                if (rel.getClientRects().length > 0) {
                    out.withRelations++;
                    if (rr.top < cr.top - tol || rr.bottom > cr.bottom + tol
                        || rr.left < cr.left - tol || rr.right > cr.right + tol) NOTE('relationsOutsideCard', key);
                    if (fr.top < rr.bottom - 0.5 && fr.left < rr.right && fr.right > rr.left)
                        NOTE('relationsUnderFooter', key);
                    const rows = rel.querySelectorAll('.mm-relation').length;
                    const cs = getComputedStyle(rel);
                    const padT = parseFloat(cs.paddingTop) || 0, padB = parseFloat(cs.paddingBottom) || 0;
                    const rowH = rows ? rel.querySelector('.mm-relation').getBoundingClientRect().height : 0;
                    const scrollable = cs.overflowY === 'auto' || cs.overflowY === 'scroll';
                    const allRowsVisible = rel.scrollHeight <= rel.clientHeight + 1;
                    const visibleRows = rowH > 0
                        ? Math.floor((rel.clientHeight - padT - padB + 0.5) / rowH) : 0;
                    if (rows && visibleRows < 1)
                        NOTE('relationsNoWholeRow', key + '(' + rows + 'rows/' + Math.round(rel.clientHeight) + 'px)');
                    if (rows && !allRowsVisible && !scrollable) NOTE('relationsClippedNotScrollable', key);
                }

                const nodeLink = n.getAttribute('link');
                const ls = getComputedStyle(link);
                const shown = ls.display !== 'none' && link.getClientRects().length > 0;
                if (nodeLink) {
                    out.withLink++;
                    const lr = R(link);
                    const href = link.getAttribute('href');
                    if (!shown) NOTE('linkHiddenThoughNodeHasOne', key);
                    else {
                        if (!(link.textContent || '').trim()) NOTE('linkWithoutText', key);
                        if (!href || href === 'null' || href === '#') NOTE('linkDeadHref', key);
                        else if (href !== nodeLink) NOTE('linkWrongDestination', key);
                        if (!(lr.width >= 40 && lr.height >= 20)) NOTE('linkTooSmall', key);
                        if (lr.top < ir.top - tol || lr.bottom > ir.bottom + tol) NOTE('linkOutsideImageSection', key);
                    }
                } else {
                    out.withoutLink++;
                    if (shown) NOTE('linkShownWithoutDestination', key);
                    if (link.hasAttribute('href')) NOTE('linkHrefWithoutDestination', key);
                }
                card.dispatchEvent(new MouseEvent('mouseleave'));
            }
            return [String(out.all), String(out.nodes), String(out.withRelations), String(out.withLink),
                    String(out.withoutLink), JSON.stringify(out.tally), out.violations.join(' | ')];
        }";

        /// <summary>Asserts the sweep found no violation, and states what it actually covered.</summary>
        private void AssertSweepClean(string[] sweep, string lang, string variant, int width, int height)
        {
            Assert.NotNull(sweep);
            Assert.True(sweep.Length >= 7, "the sweep must return 7 fields");
            var (all, nodes, withRel, withLink, withoutLink) = (
                int.Parse(sweep[0]), int.Parse(sweep[1]), int.Parse(sweep[2]),
                int.Parse(sweep[3]), int.Parse(sweep[4]));
            _output.WriteLine($"[{lang}/{variant} {width}x{height}] nodes={nodes}/{all} " +
                              $"withRelations={withRel} withLink={withLink} withoutLink={withoutLink} " +
                              $"tally={sweep[5]}");
            Assert.True(nodes >= 200 && nodes == all,
                $"the sweep must cover every navigable node ({nodes} of {all}) — a lower count means the " +
                "navigation index did not resolve and the assertions below would be vacuous");
            Assert.True(withLink >= 1 && withoutLink >= 1,
                $"the fixture must offer both a node WITH a destination ({withLink}) and one WITHOUT ({withoutLink}) — " +
                "otherwise the link assertions of the sweep ran on one side only");
            Assert.True(string.IsNullOrEmpty(sweep[6]),
                $"#2400 [{lang}/{variant} {width}x{height}] — {sweep[5]} | {sweep[6]}");
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
        [InlineData("ar", "Fallacies_ar.content.svg")]
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
                Assert.True(after.HasValue, $"no post-wheel scale for {lang}");
                _output.WriteLine($"[{lang}] wheel scale before={before.Value:F5} after={after.Value:F5}");
                Assert.True(after.Value > before.Value,
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
                Assert.True(after.HasValue, $"no post-dblclick scale for {lang}");
                _output.WriteLine($"[{lang}] dblclick scale before={before.Value:F5} after={after.Value:F5}");
                Assert.True(after.Value > before.Value,
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

        // ---- #2190 lot 2 — navigation capabilities (#10–#17) -----------------
        //
        // Lot 2 adds to the wrapper (both templates; this suite proves the behaviour on the
        // INLINING variant, the substring presence on both variants is pinned by the headless
        // golden master): search over the nodes with suggestions, `#pk=` deep links with
        // browser-back, clickable family ancestors, keyboard access (Échap closes, Tab reaches
        // nodes and links with screen-reader names), crosslink hover/click panel, and the
        // node's relations listed in the card. Every test carries its inverse control — the
        // measurement that would stay green if the capability silently degraded.

        private static string Norm(string? s)
            => System.Text.RegularExpressions.Regex.Replace(s ?? "", @"\s+", " ").Trim();

        /// <summary>
        /// Cap 10 — search: a matching query shows suggestions and Enter opens the card on the
        /// selected node. Inverse control: a no-match query keeps the suggestion box hidden and
        /// the card closed (the pre-lot-2 wrapper had no search field at all, so "the box stays
        /// hidden" is also exactly the degraded state this test must distinguish from).
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap10_Search_SuggestsAndEnterOpensCard(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                var search = page.Locator("#mmSearch");
                Assert.Equal(1, await search.CountAsync());
                var card = page.Locator("card");

                // Inverse FIRST: no-match query → box hidden, card closed.
                Assert.True(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "card must start hidden");
                await search.FillAsync("zzqqxxnoexist");
                await page.WaitForTimeoutAsync(120);
                Assert.True(await page.Locator("#mmSuggestions")
                    .EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "no-match query must keep the suggestion box hidden");
                Assert.True(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "no-match query must not open the card");

                // Real query: « Homme de paille » is a real fr label.
                await search.FillAsync("Homme de paille");
                await page.WaitForTimeoutAsync(120);
                Assert.False(await page.Locator("#mmSuggestions")
                    .EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "matching query must show the suggestion box");
                var firstLabel = await page.Locator("#mmSuggestions .mm-sugg span").First
                    .EvaluateAsync<string>("el => el.textContent");
                _output.WriteLine($"[{lang}] first suggestion = '{firstLabel}'");
                Assert.Contains("homme de paille", (firstLabel ?? "").ToLower());

                // Enter activates the selected suggestion → deep link → card opens on that node.
                await search.PressAsync("Enter");
                await page.WaitForTimeoutAsync(500);
                Assert.False(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "Enter on a suggestion must open the card");
                var cardTitle = Norm(await page.Locator("card .title").EvaluateAsync<string>("el => el.textContent"));
                _output.WriteLine($"[{lang}] card title after Enter = '{cardTitle}'");
                Assert.Contains("Homme de paille", cardTitle);
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        /// <summary>
        /// Cap 11 — deep link: <c>#pk=N</c> focuses the node and opens its card, a second hash
        /// pushes history so the browser BACK returns to the previous node, and an invalid pk is
        /// a bounded no-op (the inverse control: nothing moves for a pk that does not exist —
        /// an unguarded handler would throw or open a stale card).
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap11_DeepLink_PkHashNavigates_BackReturns_InvalidIsNoOp(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);
                var card = page.Locator("card");

                // Inverse FIRST: pk that matches no node, then a malformed hash — the card must
                // stay closed for both.
                await page.EvaluateAsync("location.hash = '#pk=999999'");
                await page.WaitForTimeoutAsync(300);
                Assert.True(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "unknown pk must be a no-op (card stays closed)");
                await page.EvaluateAsync("location.hash = '#pk=abc'");
                await page.WaitForTimeoutAsync(200);
                Assert.True(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "malformed pk hash must be a no-op");

                // Two distinct nodes, picked from the index (never hardcoded: the corpus moves).
                var pick = await page.EvaluateAsync<string[]>(@"() => {
                    const nav = window.__mmNav;
                    const ks = Object.keys(nav.byPk);
                    const a = nav.byPk[ks[10]], b = nav.byPk[ks[11]];
                    return [a.pk, a.label, b.pk, b.label];
                }");
                Assert.NotNull(pick); Assert.True(pick.Length >= 4);
                var (pkA, labelA, pkB, labelB) = (pick[0], pick[1], pick[2], pick[3]);
                _output.WriteLine($"[{lang}] pkA={pkA} '{labelA}' pkB={pkB} '{labelB}'");

                await page.EvaluateAsync<string>("pk => { location.hash = 'pk=' + pk; }", pkA);
                await page.WaitForTimeoutAsync(400);
                Assert.False(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "#pk= must open the card");
                Assert.Equal(Norm(labelA), Norm(await page.Locator("card .title").EvaluateAsync<string>("el => el.textContent")));
                var activeA = await page.EvaluateAsync<bool>(
                    "pk => window.__mmNav.byPk[pk].el.classList.contains('active')", pkA);
                Assert.True(activeA, "the deep-linked node itself must carry the active state");

                await page.EvaluateAsync<string>("pk => { location.hash = 'pk=' + pk; }", pkB);
                await page.WaitForTimeoutAsync(400);
                Assert.Equal(Norm(labelB), Norm(await page.Locator("card .title").EvaluateAsync<string>("el => el.textContent")));

                // Browser back: history returns to pkA and the hashchange re-applies it.
                await page.GoBackAsync();
                await page.WaitForTimeoutAsync(500);
                var titleAfterBack = Norm(await page.Locator("card .title").EvaluateAsync<string>("el => el.textContent"));
                _output.WriteLine($"[{lang}] title after browser back = '{titleAfterBack}'");
                Assert.Equal(Norm(labelA), titleAfterBack);
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        /// <summary>
        /// Cap 12 — Échap: a no-op when everything is already closed (inverse), closes the card
        /// when only the card is open, and closes the LINK PANEL first when both are open (the
        /// declared precedence panel → card), the second Échap then closing the card.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap12_Escape_NoOpWhenClosed_ClosesCard_PanelTakesPrecedence(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);
                var card = page.Locator("card");
                var panel = page.Locator("#mmLinkPanel");

                // Inverse: Échap with everything closed changes nothing (and must not throw).
                await page.Keyboard.PressAsync("Escape");
                await page.WaitForTimeoutAsync(120);
                Assert.True(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "Échap with everything closed must leave the card closed");

                // Card only.
                await page.Locator("#mindmap svg g.node[id=\"0\"]").ClickAsync();
                await page.WaitForTimeoutAsync(150);
                Assert.False(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"));
                await page.Keyboard.PressAsync("Escape");
                await page.WaitForTimeoutAsync(120);
                Assert.True(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "Échap must close the card");

                // Both open: the panel is closed by a real crosslink click, the card by a node click.
                await page.EvaluateAsync(@"() => {
                    const nav = window.__mmNav;
                    const L = nav.linkRecs.find(l => nav.byPath[l.source] && nav.byPath[l.target]);
                    L.g.dispatchEvent(new MouseEvent('click', { bubbles: true }));
                }");
                await page.WaitForTimeoutAsync(120);
                Assert.False(await panel.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "crosslink click must open the relation panel");
                await page.Locator("#mindmap svg g.node[id=\"0\"]").ClickAsync();
                await page.WaitForTimeoutAsync(120);
                Assert.False(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"));

                await page.Keyboard.PressAsync("Escape");
                await page.WaitForTimeoutAsync(120);
                Assert.True(await panel.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "first Échap must close the PANEL");
                Assert.False(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "first Échap must NOT close the card while the panel was open (declared precedence)");

                await page.Keyboard.PressAsync("Escape");
                await page.WaitForTimeoutAsync(120);
                Assert.True(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "second Échap must close the card");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        /// <summary>
        /// Cap 13 — keyboard: nodes and crosslinks are tab-reachable (tabindex=0) and every one
        /// carries a screen-reader name (aria-label) and a role. The inverse control is the
        /// mass: on the pre-lot-2 wrapper these counts were ZERO (no tabindex, no name), so the
        /// assertions "named == tabbable == roled, over the whole map" are exactly the
        /// differential. Tab traversal is exercised from the page start until focus lands on a
        /// node, and one crosslink is focused directly (focusability is what Tab consumes).
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap13_Keyboard_NodesAndLinksAreTabbableAndNamed(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                var counts = await page.EvaluateAsync<long[]>(@"() => {
                    const nodes = document.querySelectorAll('#mindmap svg g.node');
                    let named = 0, tabbable = 0, roled = 0;
                    nodes.forEach(n => {
                        if ((n.getAttribute('aria-label') || '').trim()) named++;
                        if (n.getAttribute('tabindex') === '0') tabbable++;
                        if (n.getAttribute('role') === 'button') roled++;
                    });
                    const links = document.querySelectorAll('g.crosslink');
                    let lNamed = 0, lTabbable = 0, lRoled = 0;
                    links.forEach(g => {
                        if ((g.getAttribute('aria-label') || '').trim()) lNamed++;
                        if (g.getAttribute('tabindex') === '0') lTabbable++;
                        if (g.getAttribute('role') === 'link') lRoled++;
                    });
                    return [nodes.length, named, tabbable, roled, links.length, lNamed, lTabbable, lRoled];
                }");
                Assert.NotNull(counts);
                Assert.True(counts.Length >= 8, $"counts probe returned {counts.Length} values");
                var (n, named, tabbable, roled) = (counts[0], counts[1], counts[2], counts[3]);
                var (l, lNamed, lTabbable, lRoled) = (counts[4], counts[5], counts[6], counts[7]);
                _output.WriteLine($"[{lang}] nodes={n} named={named} tabbable={tabbable} roled={roled} | " +
                    $"links={l} named={lNamed} tabbable={lTabbable} roled={lRoled}");

                // Named nodes are the tabbable ones, in full, with the button role. The floor
                // (>1000) is the mass of the map (measured 1408 nodes, 581 links on fr) — the
                // exact figure is corpus-derived and stays out of the test.
                Assert.True(n > 1000, $"expected the full Fallacies map, got {n} nodes");
                Assert.Equal(named, tabbable);
                Assert.Equal(named, roled);
                Assert.True(l > 0, "the fr Fallacies map must carry crosslinks");
                Assert.Equal(l, lNamed);
                Assert.Equal(l, lTabbable);
                Assert.Equal(l, lRoled);

                // Tab from the page start reaches a node (bounded walk past the toolbar).
                string? reached = null;
                for (var i = 0; i < 12 && reached == null; i++)
                {
                    await page.Keyboard.PressAsync("Tab");
                    reached = await page.EvaluateAsync<string?>(@"() => {
                        const a = document.activeElement;
                        if (!a || !a.getAttribute) return null;
                        const cls = a.getAttribute('class') || '';
                        if (cls.split(/\s+/).indexOf('node') >= 0 && a.closest && a.closest('#mindmap svg'))
                            return a.getAttribute('aria-label') || '';
                        return null;
                    }");
                }
                _output.WriteLine($"[{lang}] Tab reached node with aria-label '{reached}'");
                Assert.False(string.IsNullOrWhiteSpace(reached),
                    "Tab must reach a mindmap node carrying a non-empty screen-reader name");

                // A crosslink is focusable (what Tab consumes once focus order reaches it).
                var linkFocused = await page.EvaluateAsync<bool>(@"() => {
                    const nav = window.__mmNav;
                    const L = nav.linkRecs.find(l2 => nav.byPath[l2.source] && nav.byPath[l2.target]);
                    L.g.focus();
                    return document.activeElement === L.g &&
                        !!(L.g.getAttribute('aria-label') || '').trim();
                }");
                Assert.True(linkFocused, "a crosslink must be focusable with a non-empty aria-label");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        /// <summary>
        /// Cap 14 — crosslink hover: entering the link highlights the curve (mm-link-hover) and
        /// BOTH endpoint nodes (mm-endpoint); leaving removes all three. The leave measurement
        /// is the inverse control — a highlight that never cleared would read as a sticky state.
        /// The pre-state (no class) is asserted inside the same evaluate, before the enter.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap14_CrosslinkHover_HighlightsLinkAndEndpoints_LeaveClears(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                var on = await page.EvaluateAsync<bool[]>(@"() => {
                    const nav = window.__mmNav;
                    const L = nav.linkRecs.find(l => nav.byPath[l.source] && nav.byPath[l.target]);
                    const pre = !L.g.classList.contains('mm-link-hover');
                    L.g.dispatchEvent(new MouseEvent('pointerenter'));
                    const g = L.g.classList.contains('mm-link-hover');
                    const s = nav.byPath[L.source].el.classList.contains('mm-endpoint');
                    const t = nav.byPath[L.target].el.classList.contains('mm-endpoint');
                    return [pre, g, s, t];
                }");
                Assert.NotNull(on);
                Assert.True(on.Length >= 4, "hover probe returned too few values");
                _output.WriteLine($"[{lang}] hover: pre-clean={on[0]} link={on[1]} src={on[2]} tgt={on[3]}");
                Assert.True(on[0]); // pre-state: nothing highlighted
                Assert.True(on[1]); // the curve
                Assert.True(on[2]); // the source node
                Assert.True(on[3]); // the target node

                // Inverse: leaving clears every class the enter set.
                var off = await page.EvaluateAsync<bool[]>(@"() => {
                    const nav = window.__mmNav;
                    const L = nav.linkRecs.find(l => nav.byPath[l.source] && nav.byPath[l.target]);
                    L.g.dispatchEvent(new MouseEvent('pointerleave'));
                    const g = !L.g.classList.contains('mm-link-hover');
                    const s = !nav.byPath[L.source].el.classList.contains('mm-endpoint');
                    const t = !nav.byPath[L.target].el.classList.contains('mm-endpoint');
                    return [g, s, t];
                }");
                Assert.NotNull(off);
                Assert.True(off.Length >= 3);
                Assert.True(off[0]);
                Assert.True(off[1]);
                Assert.True(off[2]);
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        /// <summary>
        /// Cap 15 — crosslink click opens the relation panel naming both endpoints
        /// (« source — verbe → target »), and clicking the TARGET button closes the panel and
        /// opens the target's card. Inverse control: the panel is hidden before the click (and
        /// the card too), so the opening is the click's effect, not a default state.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap15_CrosslinkClick_PanelNamesBothNodes_TargetButtonNavigates(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);
                var card = page.Locator("card");
                var panel = page.Locator("#mmLinkPanel");

                // Inverse pre-state.
                Assert.True(await panel.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "panel must start hidden");
                Assert.True(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "card must start hidden");

                var info = await page.EvaluateAsync<string[]>(@"() => {
                    const nav = window.__mmNav;
                    const L = nav.linkRecs.find(l => nav.byPath[l.source] && nav.byPath[l.target]);
                    L.g.dispatchEvent(new MouseEvent('click', { bubbles: true }));
                    return [nav.byPath[L.source].label, nav.byPath[L.target].label, L.verbLabel];
                }");
                Assert.NotNull(info); Assert.True(info.Length >= 3);
                var (srcLabel, tgtLabel, verb) = (info[0], info[1], info[2]);
                _output.WriteLine($"[{lang}] panel link: '{srcLabel}' --{verb}--> '{tgtLabel}'");

                await page.WaitForTimeoutAsync(150);
                Assert.False(await panel.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "crosslink click must open the relation panel");

                var buttons = page.Locator("#mmLinkBody .mm-node-btn");
                Assert.Equal(2, await buttons.CountAsync());
                var b0 = Norm(await buttons.Nth(0).EvaluateAsync<string>("el => el.textContent"));
                var b1 = Norm(await buttons.Nth(1).EvaluateAsync<string>("el => el.textContent"));
                Assert.Equal(Norm(srcLabel), b0);
                Assert.Equal(Norm(tgtLabel), b1);
                var bodyText = Norm(await page.Locator("#mmLinkBody").EvaluateAsync<string>("el => el.textContent"));
                Assert.Contains(verb, bodyText);

                // Click the TARGET: panel closes, card opens on the target node.
                await buttons.Nth(1).ClickAsync();
                await page.WaitForTimeoutAsync(500);
                Assert.True(await panel.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "choosing an endpoint must close the panel");
                Assert.False(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "choosing an endpoint must open that node's card");
                var title = Norm(await page.Locator("card .title").EvaluateAsync<string>("el => el.textContent"));
                _output.WriteLine($"[{lang}] card after clicking target = '{title}'");
                Assert.Equal(Norm(tgtLabel), title);
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        /// <summary>
        /// Cap 16 — the card lists the node's relations and clicking one navigates to the related
        /// node (card title changes). Inverse control: a node with NO relation keeps the
        /// .relations block hidden — the block is content-driven, not always-rendered.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        public async Task Cap16_CardListsRelations_ClickingOneNavigates_LonelyNodeHidesBlock(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);
                var card = page.Locator("card");
                var relations = page.Locator("card .relations");

                // Pick an endpoint node and its first relation's far side, from the corpus.
                var pick = await page.EvaluateAsync<string[]>(@"() => {
                    const nav = window.__mmNav;
                    const involved = new Set();
                    nav.linkRecs.forEach(l => { involved.add(l.source); involved.add(l.target); });
                    const L = nav.linkRecs.find(l => nav.byPath[l.source] && nav.byPath[l.target]);
                    const me = nav.byPath[L.target];
                    const other = L.source === me.path ? L.target : L.source;
                    // A node with no relation at all, for the inverse.
                    let lonely = null;
                    for (const k of Object.keys(nav.byPk)) {
                        const r = nav.byPk[k];
                        if (r.path && !involved.has(r.path)) { lonely = r; break; }
                    }
                    return [me.pk, me.label, nav.byPath[other].label,
                            lonely ? lonely.pk : '', lonely ? lonely.label : ''];
                }");
                Assert.NotNull(pick); Assert.True(pick.Length >= 5);
                var (mePk, meLabel, otherLabel, lonelyPk, lonelyLabel) =
                    (pick[0], pick[1], pick[2], pick[3], pick[4]);
                _output.WriteLine($"[{lang}] me pk={mePk} '{meLabel}' -> other '{otherLabel}' | lonely pk={lonelyPk} '{lonelyLabel}'");

                // Inverse FIRST: a node with no relation shows no relations block.
                if (!string.IsNullOrEmpty(lonelyPk))
                {
                    await page.EvaluateAsync<string>("pk => { location.hash = 'pk=' + pk; }", lonelyPk);
                    await page.WaitForTimeoutAsync(450);
                    Assert.False(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                        "deep link must open the lonely node's card");
                    Assert.True(await relations.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                        $"node '{lonelyLabel}' has no relation — the .relations block must stay hidden");
                }

                // The endpoint node: relations listed, clicking the first navigates.
                await page.EvaluateAsync<string>("pk => { location.hash = 'pk=' + pk; }", mePk);
                await page.WaitForTimeoutAsync(450);
                Assert.False(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "deep link must open the node's card");
                Assert.False(await relations.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    $"node '{meLabel}' has relations — the block must be visible");
                var rows = page.Locator("card .relations .mm-relation");
                Assert.True(await rows.CountAsync() >= 1, "at least one relation row must be listed");
                var rowText = Norm(await rows.First.EvaluateAsync<string>("el => el.textContent"));
                _output.WriteLine($"[{lang}] first relation row = '{rowText}'");
                Assert.Contains("→", rowText);

                await rows.First.ClickAsync();
                await page.WaitForTimeoutAsync(500);
                var titleAfter = Norm(await page.Locator("card .title").EvaluateAsync<string>("el => el.textContent"));
                _output.WriteLine($"[{lang}] card title after relation click = '{titleAfter}' (was '{meLabel}')");
                Assert.NotEqual(Norm(meLabel), titleAfter);
                Assert.Contains(Norm(otherLabel), titleAfter);
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        /// <summary>
        /// Cap 17 — Virtues map with its lot-2 anchors present. Since #2289 the pipeline writes
        /// `pk`/`path` on every paired Virtues node ("same navigation anchors as the Fallacies
        /// twin"); #2303 ships the first re-derived Virtues SVGs, so this fixture no longer is
        /// the degrade case it was when the test was born (no pk, no path, `#pk=` a no-op).
        /// What still holds BY CONSTRUCTION is the crosslink-free topology (the Virtues writer
        /// poses no data-crosslink-* elements): the legend must stay hidden. Everything the
        /// anchors enable is on and measured against the source index: every node carries `pk`,
        /// `byPk`/`byPath` are fully populated, `#pk=<n>` opens that exact node's card, an
        /// unknown pk stays a bounded no-op (the inverse control), and the lot-1 search
        /// behaviour survives the pairing (suggestions, Enter opens the card).
        /// </summary>
        [Theory]
        [InlineData("fr", "Argumentum_Virtues_MindMap_fr.content.svg")]
        public async Task Cap17_Virtues_PairedAnchors_LegendHidden_PkDeepLinkAndSearchWork(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);
                var card = page.Locator("card");

                // Precondition measured on the fixture: full pairing, crosslink-free topology.
                // The node total is read from the DOM (never hardcoded: the corpus moves) and
                // every anchor count must equal it — a partial pairing is a red, not a degrade.
                var pre = await page.EvaluateAsync<long[]>(@"() => [
                    document.querySelectorAll('#mindmap svg g.node').length,
                    document.querySelectorAll('#mindmap svg g.node[pk]').length,
                    document.querySelectorAll('g.crosslink').length,
                    Object.keys(window.__mmNav.byPk).length,
                    Object.keys(window.__mmNav.byPath).length
                ]");
                Assert.NotNull(pre);
                Assert.True(pre.Length >= 5, "precondition probe returned too few values");
                _output.WriteLine($"[{lang}] Virtues precondition: nodes={pre[0]} pk-attr={pre[1]} crosslinks={pre[2]} " +
                    $"byPk={pre[3]} byPath={pre[4]}");
                Assert.True(pre[0] > 0, "fixture must carry at least one node");
                Assert.Equal(pre[0], pre[1]); // every node paired — the #2289 premise, re-measured
                Assert.Equal(0, pre[2]);      // crosslink-free by construction — the Virtues constant
                Assert.Equal(pre[0], pre[3]);
                Assert.Equal(pre[0], pre[4]);

                // Crosslink-free topology: the legend never unhides on this map.
                Assert.True(await page.Locator("#mmLegend").EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "no crosslinks → the legend must stay hidden");

                // Inverse control FIRST: a pk matching no node stays a bounded no-op.
                await page.EvaluateAsync("location.hash = '#pk=999999'");
                await page.WaitForTimeoutAsync(400);
                Assert.True(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "unknown pk must be a no-op (card stays closed)");

                // Deep link on a REAL node, picked from the index (never hardcoded).
                var pick = await page.EvaluateAsync<string[]>(@"() => {
                    const nav = window.__mmNav;
                    const k = Object.keys(nav.byPk)[0];
                    const a = nav.byPk[k];
                    return [String(a.pk), a.label];
                }");
                Assert.NotNull(pick);
                Assert.True(pick.Length >= 2 && !string.IsNullOrEmpty(pick[0]), "byPk index must be readable");
                var (pkA, labelA) = (pick[0], pick[1]);
                _output.WriteLine($"[{lang}] deep link pk={pkA} '{labelA}'");
                await page.EvaluateAsync<string>("pk => { location.hash = 'pk=' + pk; }", pkA);
                await page.WaitForTimeoutAsync(400);
                Assert.False(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "#pk= must open the card on the paired Virtues map");
                Assert.Equal(Norm(labelA), Norm(await page.Locator("card .title").EvaluateAsync<string>("el => el.textContent")));

                // Lot-1 behaviour survives the pairing: search over the node labels reaches a
                // node and opens its card.
                var firstLabel = await page.EvaluateAsync<string>(@"() => {
                    const n = document.querySelector('#mindmap svg g.node');
                    return (n.textContent || '').replace(/\s+/g, ' ').trim();
                }");
                Assert.False(string.IsNullOrWhiteSpace(firstLabel), "fixture must carry at least one labelled node");
                var probe = firstLabel.Split(' ')[0];
                _output.WriteLine($"[{lang}] searching '{probe}' (from label '{firstLabel}')");
                await page.Locator("#mmSearch").FillAsync(probe);
                await page.WaitForTimeoutAsync(150);
                Assert.False(await page.Locator("#mmSuggestions")
                    .EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "search must still work on the paired Virtues map");
                await page.Locator("#mmSearch").PressAsync("Enter");
                await page.WaitForTimeoutAsync(500);
                Assert.False(await card.EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "Enter on a suggestion must still open the card");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        /// <summary>
        /// Cap 18 — #2190 lot 3. P23-23: the legend shows one swatch per relation, its colour
        /// READ from the drawn arrows of that verb, never hardcoded. The verification reads the
        /// colour by a DIFFERENT instrument than the implementation (the SVG stroke ATTRIBUTE,
        /// where readCrosslinkStroke uses the computed style): a swatch that diverges from the
        /// arrows, or a legend without swatches, fails. Inverse control baked in: an empty or
        /// missing backgroundColor on any dot is a red. P23-20: every interface element that
        /// declares a `font:` shorthand (which resets font-variant-ligatures) must still compute
        /// no-common-ligatures — the DINPro fi/fl/ff ligatures corrupt interface words.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        [InlineData("en", "Fallacies_en.content.svg")]
        [InlineData("ru", "Fallacies_ru.content.svg")]
        [InlineData("pt", "Fallacies_pt.content.svg")]
        [InlineData("es", "Fallacies_es.content.svg")]
        [InlineData("ar", "Fallacies_ar.content.svg")]
        [InlineData("fa", "Fallacies_fa.content.svg")]
        [InlineData("zh", "Fallacies_zh.content.svg")]
        public async Task Cap18_LegendSwatchesMatchArrowStrokes_InterfaceCutsLigatures(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                // Legend must be awake (the fixture carries crosslinks).
                Assert.False(await page.Locator("#mmLegend").EvaluateAsync<bool>("el => el.classList.contains('hidden')"),
                    "fixture has crosslinks → the legend must be visible");

                // Two independent reads: what the legend dots show, and what the arrows carry.
                var probe = await page.EvaluateAsync<string[]>(@"() => {
                    const norm = (s) => (s || '').replace(/\s+/g, '').toLowerCase();
                    const nav = window.__mmNav;
                    const verbStrokes = {};
                    nav.linkRecs.forEach((l) => {
                        if (!(l.verb in verbStrokes)) verbStrokes[l.verb] = norm(l.g.getAttribute('stroke'));
                    });
                    const rows = [];
                    document.querySelectorAll('#mmLegendList li').forEach((li) => {
                        const dot = li.querySelector('.mm-verb-dot');
                        rows.push([
                            (li.textContent || '').trim(),
                            dot ? norm(dot.style.backgroundColor) : '',
                            dot ? dot.getBoundingClientRect().width : 0
                        ]);
                    });
                    return [JSON.stringify(verbStrokes)].concat(rows.map(r => r.join('||')));
                }");
                Assert.NotNull(probe);
                Assert.True(probe.Length >= 2, "legend must list at least one relation");

                var verbStrokes = System.Text.Json.JsonSerializer
                    .Deserialize<System.Collections.Generic.Dictionary<string, string>>(probe[0]);
                Assert.NotNull(verbStrokes);
                Assert.True(verbStrokes.Count >= 1, "fixture must carry at least one verb");
                _output.WriteLine($"[{lang}] verbs on the map: {verbStrokes.Count}");

                var dotColors = new System.Collections.Generic.List<string>();
                for (var i = 1; i < probe.Length; i++)
                {
                    var row = probe[i];
                    var parts = row.Split("||");
                    Assert.True(parts.Length >= 3, $"malformed legend row: '{row}'");
                    var (text, bg, width) = (parts[0], parts[1], parts[2]);
                    Assert.True(!string.IsNullOrEmpty(bg.Trim()),
                        $"legend row '{text}' has NO swatch colour — the pastille must exist and be filled (P23-23)");
                    Assert.True(double.TryParse(width, System.Globalization.CultureInfo.InvariantCulture,
                            out var w) && w >= 10,
                        $"legend row '{text}': swatch is {width}px wide — a pastel stroke needs a large, bordered dot");
                    dotColors.Add(bg);
                }

                // Every verb's arrow colour must be carried by exactly the swatches.
                Assert.True(verbStrokes.Count == dotColors.Count,
                    $"one legend entry per relation present on the map: {verbStrokes.Count} verbs vs {dotColors.Count} legend rows");
                foreach (var kv in verbStrokes)
                {
                    Assert.True(dotColors.Contains(kv.Value),
                        $"verb '{kv.Key}': arrows are {kv.Value} but no legend swatch carries it — the colour must be READ from the drawn arrows (P23-23)");
                }

                // P23-20 — computed style on every `font:`-shorthand bearer: the reasserted
                // longhand must win over the shorthand's reset.
                var lig = await page.EvaluateAsync<string[]>(@"() => {
                    const sel = ['#mmSearch', '#mmLegend', '#mmLinkPanel', 'card .relations'];
                    return sel.map((s) => {
                        const el = document.querySelector(s);
                        return s + '=' + (el ? getComputedStyle(el).fontVariantLigatures : 'absent');
                    });
                }");
                Assert.NotNull(lig);
                foreach (var line in lig)
                {
                    _output.WriteLine($"[{lang}] ligature computed: {line}");
                    Assert.True(line.EndsWith("no-common-ligatures", StringComparison.Ordinal),
                        $"P23-20: {line} — the `font:` shorthand resets font-variant-ligatures; the reasserted rule must win");
                }
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ===== #2190 lot 3, P23-21 — interface strings localised by document language =====
        //
        // The wrapper's static HTML ships French (no-JS degrade); the JS layer reads
        // <html lang> — substituted by the writer's [LANGUAGE] token (#457 T4a), the same
        // FormatWrapper path ComposeIncludedAsync exercises — picks its strings from MM_UI
        // (repli fr) and overlays them onto placeholder, aria-labels, legend summary and
        // verb labels. Until P23-21 the interface was French in all 8 languages.
        //
        // INDEPENDENT ORACLE: the tables below restate the expected strings and verbs per
        // language; they are NOT read from the template's MM_UI — that would test the
        // implementation against itself. Any divergence (a language left on the French
        // statics because the overlay did not run, a typo in the dictionary, a broken
        // [LANGUAGE] substitution) turns the case red. The fr case pins the identity
        // mapping; by itself it cannot catch a dead overlay (statics are already French) —
        // the seven non-fr cases and the authoring-time mutation control carry the
        // falsifiability: replacing `var ui = MM_UI[uiLang] || MM_UI.fr;` with
        // `var ui = MM_UI.fr;` in the worktree template turned them red (backup + restore).
        private static readonly System.Collections.Generic.Dictionary<string, string[]> P2321ExpectedUi =
            new System.Collections.Generic.Dictionary<string, string[]>
            {
                // [placeholder, searchAria, suggestionsAria, legendSummary, panelAria, closeAria]
                ["fr"] = new[] { "Rechercher dans la carte…", "Rechercher un sophisme dans la carte", "Résultats de recherche", "Relations entre sophismes", "Relation entre deux sophismes", "Fermer le panneau de relation" },
                ["en"] = new[] { "Search the map…", "Search the map for a fallacy", "Search results", "Relations between fallacies", "Relation between two fallacies", "Close the relation panel" },
                ["ru"] = new[] { "Поиск по карте…", "Искать софизм на карте", "Результаты поиска", "Связи между софизмами", "Связь между двумя софизмами", "Закрыть панель связей" },
                ["pt"] = new[] { "Pesquisar no mapa…", "Pesquisar um sofisma no mapa", "Resultados da pesquisa", "Relações entre sofismas", "Relação entre dois sofismas", "Fechar o painel de relações" },
                ["es"] = new[] { "Buscar en el mapa…", "Buscar una falacia en el mapa", "Resultados de la búsqueda", "Relaciones entre falacias", "Relación entre dos falacias", "Cerrar el panel de relaciones" },
                ["ar"] = new[] { "ابحث في الخريطة…", "ابحث عن مغالطة في الخريطة", "نتائج البحث", "العلاقات بين المغالطات", "علاقة بين مغالطتين", "إغلاق لوحة العلاقات" },
                ["fa"] = new[] { "جستجو در نقشه…", "جستجوی مغلطه در نقشه", "نتایج جستجو", "روابط میان مغلطه‌ها", "رابطه میان دو مغلطه", "بستن پنل رابطه" },
                ["zh"] = new[] { "在地图中搜索…", "在地图中搜索谬误", "搜索结果", "谬误之间的关系", "两个谬误之间的关系", "关闭关系面板" },
            };

        private static readonly System.Collections.Generic.Dictionary<string, string[]> P2321ExpectedVerbs =
            new System.Collections.Generic.Dictionary<string, string[]>
            {
                ["fr"] = new[] { "précède", "dénonce", "exploite", "permet", "s’oppose à", "inverse", "reflète", "est apparenté à" },
                ["en"] = new[] { "precedes", "denounces", "exploits", "allows", "opposes", "inverts", "mirrors", "is related to" },
                ["ru"] = new[] { "предшествует", "разоблачает", "использует", "допускает", "противостоит", "переворачивает", "отражает", "родственен с" },
                ["pt"] = new[] { "precede", "denuncia", "explora", "permite", "opõe-se a", "inverte", "reflete", "é aparentado com" },
                ["es"] = new[] { "precede", "denuncia", "explota", "permite", "se opone a", "invierte", "refleja", "está emparentado con" },
                ["ar"] = new[] { "يسبق", "يفضح", "يستغل", "يتيح", "يعارض", "يقلب", "يعكس", "يرتبط بـ" },
                ["fa"] = new[] { "پیش می‌آید", "رسوا می‌کند", "بهره می‌گیرد", "ممکن می‌سازد", "مخالفت می‌کند با", "وارونه می‌کند", "بازتاب می‌دهد", "هم‌خانواده است با" },
                ["zh"] = new[] { "先于", "揭露", "利用", "容许", "对立于", "颠倒", "映照", "同类于" },
            };

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        [InlineData("en", "Fallacies_en.content.svg")]
        [InlineData("ru", "Fallacies_ru.content.svg")]
        [InlineData("pt", "Fallacies_pt.content.svg")]
        [InlineData("es", "Fallacies_es.content.svg")]
        [InlineData("ar", "Fallacies_ar.content.svg")]
        [InlineData("fa", "Fallacies_fa.content.svg")]
        [InlineData("zh", "Fallacies_zh.content.svg")]
        public async Task Cap19_InterfaceStringsLocalisedByDocumentLanguage(string lang, string svgFileName)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAsync(wrapperPath);
            try
            {
                await WaitForViewportSettledAsync(page);

                var probe = await page.EvaluateAsync<string[]>(@"() => {
                    const gs = (id) => document.getElementById(id);
                    const attr = (el, name) => (el ? (el.getAttribute(name) || '') : 'ABSENT');
                    const summary = document.querySelector('#mmLegend summary');
                    const rows = [];
                    document.querySelectorAll('#mmLegendList li').forEach((li) => rows.push((li.textContent || '').trim()));
                    return [
                        (document.documentElement.lang || ''),
                        attr(gs('mmSearch'), 'placeholder'),
                        attr(gs('mmSearch'), 'aria-label'),
                        attr(gs('mmSuggestions'), 'aria-label'),
                        summary ? (summary.textContent || '').trim() : 'ABSENT',
                        attr(gs('mmLinkPanel'), 'aria-label'),
                        attr(gs('mmLinkClose'), 'aria-label'),
                        rows.join('||')
                    ];
                }");
                Assert.NotNull(probe);
                Assert.True(probe.Length >= 8, "the probe must return 8 fields");

                var expected = P2321ExpectedUi[lang];
                var fieldNames = new[] { "html[lang]", "search placeholder", "search aria-label", "suggestions aria-label", "legend summary", "link-panel aria-label", "close aria-label" };
                Assert.True(string.Equals(probe[0], lang, StringComparison.Ordinal),
                    $"html[lang] is '{probe[0]}' — the writer's [LANGUAGE] substitution feeds the whole lookup (P23-21)");
                for (var i = 0; i < 6; i++)
                {
                    _output.WriteLine($"[{lang}] {fieldNames[i + 1]}: '{probe[i + 1]}'");
                    Assert.True(string.Equals(probe[i + 1], expected[i], StringComparison.Ordinal),
                        $"P23-21 [{lang}] {fieldNames[i + 1]}: expected '{expected[i]}', got '{probe[i + 1]}' — the interface must speak the document language, not the French statics");
                }

                // Legend rows: 'verb (count)' — the verb part must be one of the language's
                // verbs (count varies per map, the label does not).
                var rows = probe[7].Split(new[] { "||" }, System.StringSplitOptions.RemoveEmptyEntries);
                Assert.True(rows.Length >= 1, "fixture has crosslinks → the legend must list relations");
                var verbs = P2321ExpectedVerbs[lang];
                var rowVerb = new System.Collections.Generic.List<string>();
                foreach (var row in rows)
                {
                    var stripped = System.Text.RegularExpressions.Regex.Replace(row, @"\s*\(\d+\)$", "");
                    _output.WriteLine($"[{lang}] legend row: '{row}' → verb '{stripped}'");
                    Assert.True(System.Array.IndexOf(verbs, stripped) >= 0,
                        $"P23-21 [{lang}] legend verb '{stripped}' (row '{row}') is not one of the localised verb labels — the legend is still speaking another language");
                    rowVerb.Add(stripped);
                }
                var distinctVerbs = new System.Collections.Generic.HashSet<string>(rowVerb);
                Assert.True(distinctVerbs.Count == rowVerb.Count,
                    $"P23-21 [{lang}]: each verb must label exactly one legend row ({distinctVerbs.Count} distinct for {rowVerb.Count} rows)");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ===== #2400 — the sheet fits inside the card, and the external link is a real link =====
        //
        // Two defects reproduced in the browser on the public site (issue #2400):
        //   witness 1 — #pk=1356 : `#externalLink` carried an href and NO text, a 40x20 px empty
        //               anchor at 390x844 and at 1280x900, in BOTH templates;
        //   witness 2 — #pk=1084 at 390x844 : seven relation rows, the relations box squeezed to
        //               25 px (one row of seven) and the footer extending 16 px past the card,
        //               13 px of overlap.
        //
        // Both are measured here on EVERY node of the real corpus rather than on the two
        // witnesses: a reported defect is a probe, not a perimeter (AGENTS.md §4). The controls
        // are red on the pre-#2400 templates — measured, not assumed: the PR body carries the
        // same probe run against `git show HEAD:` copies of the two templates.
        //
        // The label table below is an INDEPENDENT ORACLE: it restates the eight labels instead
        // of reading them from the template's MM_UI, which would test the implementation
        // against itself (same discipline as P2321ExpectedUi above).

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", 1400, 900)]
        [InlineData("fr", "Fallacies_fr.content.svg", 390, 844)]
        [InlineData("ru", "Fallacies_ru.content.svg", 1400, 900)]
        [InlineData("ru", "Fallacies_ru.content.svg", 390, 844)]
        [InlineData("ar", "Fallacies_ar.content.svg", 1400, 900)]
        [InlineData("ar", "Fallacies_ar.content.svg", 390, 844)]
        [InlineData("ru", "Argumentum_Virtues_MindMap_ru.content.svg", 1400, 900)]
        [InlineData("ru", "Argumentum_Virtues_MindMap_ru.content.svg", 390, 844)]
        public async Task Cap20_SheetStaysInsideCard_OnEveryNode_BothViewports(string lang, string svgFileName, int width, int height)
        {
            var wrapperPath = await ComposeIncludedAsync(lang, svgFileName);
            var page = await OpenPageAtAsync(wrapperPath, width, height);
            try
            {
                await WaitForViewportSettledAsync(page);
                var sweep = await page.EvaluateAsync<string[]>(CardFitSweepJs);
                AssertSweepClean(sweep, lang, "inline", width, height);
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", 1400, 900)]
        [InlineData("fr", "Fallacies_fr.content.svg", 390, 844)]
        [InlineData("ar", "Fallacies_ar.content.svg", 1400, 900)]
        [InlineData("ar", "Fallacies_ar.content.svg", 390, 844)]
        public async Task Cap21_ExternalVariant_SheetStaysInsideCard_OnEveryNode(string lang, string svgFileName, int width, int height)
        {
            var page = await OpenExternalAsync(lang, svgFileName, width, height);
            try
            {
                await WaitForExternalNavigationAsync(page);
                var sweep = await page.EvaluateAsync<string[]>(CardFitSweepJs);
                AssertSweepClean(sweep, lang, "ext", width, height);
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        private static readonly System.Collections.Generic.Dictionary<string, string> P2400LinkLabel =
            new System.Collections.Generic.Dictionary<string, string>
            {
                ["fr"] = "Ouvrir le lien externe",
                ["en"] = "Open the external link",
                ["ru"] = "Открыть внешнюю ссылку",
                ["pt"] = "Abrir o link externo",
                ["es"] = "Abrir el enlace externo",
                ["ar"] = "افتح الرابط الخارجي",
                ["fa"] = "پیوند بیرونی را باز کنید",
                ["zh"] = "打开外部链接",
            };

        /// <summary>Picks, from the corpus: a node WITH a destination, a SECOND node with a
        /// DIFFERENT destination, and a node WITHOUT one. Never hardcodes a pk — the fixture
        /// decides what its own data offers.</summary>
        private const string P2400PickSheetsJs = @"() => {
            const nav = window.__mmNav;
            const withLink = [], without = [];
            for (const k of Object.keys(nav.byPk)) {
                const r = nav.byPk[k];
                if (!r || !r.el) continue;
                const l = r.el.getAttribute('link');
                if (l) withLink.push({ pk: r.pk || k, label: r.label || '', link: l });
                else without.push({ pk: r.pk || k, label: r.label || '' });
            }
            if (withLink.length < 2 || !without.length)
                return ['', '', '', '', '', ''];
            let b = null;
            for (const w of withLink) if (w.link !== withLink[0].link) { b = w; break; }
            return [withLink[0].pk, withLink[0].label, withLink[0].link,
                    b ? b.pk : '', b ? b.link : '', without[0].pk];
        }";

        private const string P2400LinkStateJs = @"(arg) => {
            const nav = window.__mmNav;
            const card = document.querySelector('card');
            const link = document.getElementById('externalLink');
            const img = card.querySelector('.imageSection');
            nav.focusNode(nav.byPk[arg.pk]);
            const cs = getComputedStyle(link);
            const r = link.getBoundingClientRect();
            const ir = img.getBoundingClientRect();
            const shown = cs.display !== 'none' && r.width > 0 && r.height > 0;
            return [
                shown ? 'shown' : 'hidden',
                (link.textContent || ''),
                link.getAttribute('href') || '',
                link.getAttribute('target') || '',
                link.getAttribute('rel') || '',
                String(Math.round(r.width)),
                String(Math.round(r.height)),
                (r.top >= ir.top - 0.6 && r.bottom <= ir.bottom + 0.6) ? 'insideImageSection' : 'outsideImageSection'
            ];
        }";

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline")]
        [InlineData("en", "Fallacies_en.content.svg", "inline")]
        [InlineData("ru", "Fallacies_ru.content.svg", "inline")]
        [InlineData("pt", "Fallacies_pt.content.svg", "inline")]
        [InlineData("es", "Fallacies_es.content.svg", "inline")]
        [InlineData("ar", "Fallacies_ar.content.svg", "inline")]
        [InlineData("fa", "Fallacies_fa.content.svg", "inline")]
        [InlineData("zh", "Fallacies_zh.content.svg", "inline")]
        [InlineData("fr", "Fallacies_fr.content.svg", "ext")]
        [InlineData("ar", "Fallacies_ar.content.svg", "ext")]
        public async Task Cap22_ExternalLink_VisibleLocalisedLabel_HiddenWithoutDestination(string lang, string svgFileName, string variant)
        {
            var page = variant == "ext"
                ? await OpenExternalAsync(lang, svgFileName, ViewportWidth, ViewportHeight)
                : await OpenPageAtAsync(await ComposeIncludedAsync(lang, svgFileName), ViewportWidth, ViewportHeight);
            try
            {
                if (variant == "ext") await WaitForExternalNavigationAsync(page);
                else await WaitForViewportSettledAsync(page);

                var pick = await page.EvaluateAsync<string[]>(P2400PickSheetsJs);
                Assert.NotNull(pick);
                Assert.True(pick.Length >= 6 && !string.IsNullOrEmpty(pick[0]) && !string.IsNullOrEmpty(pick[3]),
                    $"#2400 [{lang}/{variant}]: the fixture must offer two distinct destinations and a node without one");
                var (withPk, withLabel, withLink, secondPk, secondLink, withoutPk) =
                    (pick[0], pick[1], pick[2], pick[3], pick[4], pick[5]);
                _output.WriteLine($"[{lang}/{variant}] with-link pk={withPk} '{withLabel}' -> {withLink} | " +
                                  $"second pk={secondPk} -> {secondLink} | without-link pk={withoutPk}");

                var shown = await page.EvaluateAsync<string[]>(P2400LinkStateJs, new { pk = withPk });
                _output.WriteLine($"[{lang}/{variant}] link text = '{shown[1]}'");
                Assert.Equal("shown", shown[0]);
                Assert.True(string.Equals(P2400LinkLabel[lang], shown[1], StringComparison.Ordinal),
                    $"#2400 witness 1 [{lang}/{variant}]: pk={withPk} — the link must carry the visible label " +
                    $"'{P2400LinkLabel[lang]}', got '{shown[1]}' (an empty anchor is the defect)");
                Assert.Equal(withLink, shown[2]);
                Assert.Equal("_blank", shown[3]);
                Assert.Contains("noopener", shown[4]);
                Assert.True(int.Parse(shown[5]) >= 40 && int.Parse(shown[6]) >= 20,
                    $"#2400 [{lang}/{variant}]: the link's box is {shown[5]}x{shown[6]} px — too small to be a target");
                Assert.Equal("insideImageSection", shown[7]);

                // The second destination proves the href tracks the sheet, not the first node.
                var second = await page.EvaluateAsync<string[]>(P2400LinkStateJs, new { pk = secondPk });
                Assert.Equal("shown", second[0]);
                Assert.Equal(secondLink, second[2]);
                Assert.Equal(P2400LinkLabel[lang], second[1]);

                // Inverse control: a node with no destination shows nothing, and carries no href.
                var hidden = await page.EvaluateAsync<string[]>(P2400LinkStateJs, new { pk = withoutPk });
                Assert.Equal("hidden", hidden[0]);
                Assert.Equal("", hidden[2]);
                Assert.True(int.Parse(hidden[5]) == 0 && int.Parse(hidden[6]) == 0,
                    $"#2400 [{lang}/{variant}]: a hidden link must occupy no box, got {hidden[5]}x{hidden[6]}");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline")]
        [InlineData("fr", "Fallacies_fr.content.svg", "ext")]
        public async Task Cap23_ExternalLinkDestination_FollowsEverySheetChange_BothPaths(string lang, string svgFileName, string variant)
        {
            var page = variant == "ext"
                ? await OpenExternalAsync(lang, svgFileName, ViewportWidth, ViewportHeight)
                : await OpenPageAtAsync(await ComposeIncludedAsync(lang, svgFileName), ViewportWidth, ViewportHeight);
            try
            {
                if (variant == "ext") await WaitForExternalNavigationAsync(page);
                else await WaitForViewportSettledAsync(page);
                var pick = await page.EvaluateAsync<string[]>(P2400PickSheetsJs);
                Assert.NotNull(pick);
                Assert.True(pick.Length >= 6 && !string.IsNullOrEmpty(pick[3]) && !string.IsNullOrEmpty(pick[5]),
                    $"#2400 [{lang}/{variant}]: the fixture must offer two destinations and a node without one");
                var (aPk, aLink, bPk, bLink, cPk) = (pick[0], pick[2], pick[3], pick[4], pick[5]);

                async Task<string[]> StateAsync(string pk) =>
                    await page.EvaluateAsync<string[]>(P2400LinkStateJs, new { pk });

                // Navigation path: A -> B -> none -> back to A.
                Assert.Equal(aLink, (await StateAsync(aPk))[2]);
                Assert.Equal(bLink, (await StateAsync(bPk))[2]);
                var none = await StateAsync(cPk);
                Assert.Equal("hidden", none[0]);
                Assert.Equal("", none[2]);
                Assert.Equal(aLink, (await StateAsync(aPk))[2]);

                // Direct-click path (`showOverlay`), the one the visitor actually uses.
                async Task<string[]> ClickStateAsync(string pk)
                {
                    await page.EvaluateAsync(
                        @"pk => {
                            const rec = window.__mmNav.byPk[pk];
                            rec.el.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true }));
                        }", pk);
                    await page.WaitForTimeoutAsync(150);
                    return await page.EvaluateAsync<string[]>(
                        @"() => {
                            const card = document.querySelector('card');
                            const link = document.getElementById('externalLink');
                            const cs = getComputedStyle(link);
                            return [cs.display !== 'none' ? 'shown' : 'hidden',
                                    link.getAttribute('href') || '',
                                    (card.querySelector('.title').textContent || '').trim()];
                        }");
                }

                var clickA = await ClickStateAsync(aPk);
                Assert.Equal("shown", clickA[0]);
                Assert.Equal(aLink, clickA[1]);
                var clickB = await ClickStateAsync(bPk);
                Assert.Equal("shown", clickB[0]);
                Assert.Equal(bLink, clickB[1]);
                var clickC = await ClickStateAsync(cPk);
                Assert.Equal("hidden", clickC[0]);
                Assert.Equal("", clickC[1]);
                var clickBack = await ClickStateAsync(aPk);
                Assert.Equal("shown", clickBack[0]);
                Assert.Equal(aLink, clickBack[1]);
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        private const string P2400RelationsStateJs = @"() => {
            const host = document.getElementById('svgObject');
            const nodeDoc = (host && host.contentDocument) ? host.contentDocument : document;
            const card = document.querySelector('card');
            const rel = card.querySelector('.relations');
            const foot = card.querySelector('.footer');
            const leg = document.getElementById('mmLegend');
            const list = document.getElementById('mmLegendList');
            const cr = card.getBoundingClientRect();
            const rr = rel.getBoundingClientRect();
            const fr = foot.getBoundingClientRect();
            const rows = rel.querySelectorAll('.mm-relation');
            const cs = getComputedStyle(rel);
            const rowH = rows.length ? rows[0].getBoundingClientRect().height : 0;
            const padT = parseFloat(cs.paddingTop) || 0, padB = parseFloat(cs.paddingBottom) || 0;
            const visibleRows = rowH > 0 ? Math.floor((rel.clientHeight - padT - padB + 0.5) / rowH) : 0;
            const actives = Array.from(nodeDoc.querySelectorAll('.node.active'));
            return [
                String(rows.length),
                String(visibleRows),
                rr.top >= cr.top - 0.6 && rr.bottom <= cr.bottom + 0.6 ? 'relationsInsideCard' : 'relationsOutsideCard',
                fr.top < rr.bottom - 0.5 && fr.left < rr.right && fr.right > rr.left ? 'overlapsFooter' : 'clearOfFooter',
                card.scrollHeight > card.clientHeight + 1 ? 'cardScrolls' : 'cardFits',
                card.classList.contains('hidden') ? 'cardHidden' : 'cardOpen',
                (card.querySelector('.title').textContent || '').trim(),
                getComputedStyle(leg).display !== 'none' ? 'legendShown' : 'legendHidden',
                String(list ? list.children.length : 0),
                (list ? list.textContent : '').replace(/\s+/g, ' ').trim(),
                String(actives.length),
                actives.length ? (actives[0].getAttribute('pk') || '') : '',
                location.hash
            ];
        }";

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline")]
        [InlineData("fr", "Fallacies_fr.content.svg", "ext")]
        public async Task Cap24_DirectClickFillsRelations_RelationClickOpensTargetAndLegend(string lang, string svgFileName, string variant)
        {
            var page = variant == "ext"
                ? await OpenExternalAsync(lang, svgFileName, ViewportWidth, ViewportHeight)
                : await OpenPageAtAsync(await ComposeIncludedAsync(lang, svgFileName), ViewportWidth, ViewportHeight);
            try
            {
                if (variant == "ext") await WaitForExternalNavigationAsync(page);
                else await WaitForViewportSettledAsync(page);

                // The node with the MOST relations in the corpus (7 on the current fixture) —
                // derived, never a hardcoded pk, so the case keeps meaning if the data moves.
                var pick = await page.EvaluateAsync<string[]>(@"() => {
                    const nav = window.__mmNav;
                    const counts = {};
                    nav.linkRecs.forEach(l => {
                        counts[l.source] = (counts[l.source] || 0) + 1;
                        counts[l.target] = (counts[l.target] || 0) + 1;
                    });
                    let best = null;
                    for (const p of Object.keys(counts)) if (!best || counts[p] > counts[best]) best = p;
                    const rec = nav.byPath[best];
                    return rec ? [rec.pk, rec.label, String(counts[best]), best] : ['', '', '0', ''];
                }");
                Assert.True(pick != null && !string.IsNullOrEmpty(pick[0]));
                var (sourcePk, sourceLabel, expectedRows) = (pick[0], pick[1], int.Parse(pick[2]));
                _output.WriteLine($"[{lang}/{variant}] busiest sheet pk={sourcePk} '{sourceLabel}' with {expectedRows} relations");
                Assert.True(expectedRows >= 3, "the busiest sheet must carry enough relations for its box to overflow");

                // DIRECT CLICK — the path that never filled the relations before #2400.
                await page.EvaluateAsync(
                    @"pk => {
                        const rec = window.__mmNav.byPk[pk];
                        rec.el.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true }));
                    }", sourcePk);
                await page.WaitForTimeoutAsync(300);

                var afterDirect = await page.EvaluateAsync<string[]>(P2400RelationsStateJs);
                Assert.NotNull(afterDirect);
                Assert.Equal("cardOpen", afterDirect[5]);
                Assert.Equal(Norm(sourceLabel), Norm(afterDirect[6]));
                _output.WriteLine($"[{lang}/{variant}] direct click: rows={afterDirect[0]} visibleRows={afterDirect[1]} " +
                                  $"legend={afterDirect[7]} legendRows={afterDirect[8]}");
                Assert.Equal(expectedRows, int.Parse(afterDirect[0]));
                Assert.Equal("relationsInsideCard", afterDirect[2]);
                Assert.Equal("clearOfFooter", afterDirect[3]);
                Assert.Equal("cardFits", afterDirect[4]);
                Assert.True(int.Parse(afterDirect[1]) >= 1,
                    $"#2400 witness 2 [{lang}/{variant}]: the relations box shows {afterDirect[1]} whole row(s) " +
                    $"for {afterDirect[0]} relations — 25 px for seven rows is the defect");
                Assert.Equal("legendShown", afterDirect[7]);
                Assert.True(int.Parse(afterDirect[8]) >= 1, "the legend must list the verbs of the map");

                // Relation row click: the target node becomes the active one, its own sheet fills,
                // the legend survives — "cliquer une relation et vérifier les deux nœuds et la légende".
                var firstRow = page.Locator("card .relations .mm-relation").First;
                var rowText = Norm(await firstRow.EvaluateAsync<string>("el => el.textContent"));
                await firstRow.ClickAsync();
                await page.WaitForTimeoutAsync(500);
                var afterRelation = await page.EvaluateAsync<string[]>(P2400RelationsStateJs);
                Assert.NotNull(afterRelation);
                _output.WriteLine($"[{lang}/{variant}] relation row '{rowText}' -> pk={afterRelation[11]} " +
                                  $"'{afterRelation[6]}' rows={afterRelation[0]} active={afterRelation[10]} hash={afterRelation[12]}");

                Assert.Equal("cardOpen", afterRelation[5]);
                Assert.Equal("1", afterRelation[10]);
                Assert.NotEqual(sourcePk, afterRelation[11]);
                Assert.Equal("#pk=" + afterRelation[11], afterRelation[12]);
                Assert.Contains(Norm(afterRelation[6]), rowText);
                Assert.True(int.Parse(afterRelation[0]) >= 1, "the target's own relations must be listed on its sheet");
                Assert.Equal("relationsInsideCard", afterRelation[2]);
                Assert.Equal("clearOfFooter", afterRelation[3]);
                Assert.Equal("cardFits", afterRelation[4]);
                Assert.True(int.Parse(afterRelation[1]) >= 1);
                Assert.Equal("legendShown", afterRelation[7]);
                Assert.True(int.Parse(afterRelation[8]) >= 1);

                // Both nodes: the source sheet is still reachable and refills its own relations.
                await page.EvaluateAsync<string[]>(P2400LinkStateJs, new { pk = sourcePk });
                var backSheet = await page.EvaluateAsync<string[]>(P2400RelationsStateJs);
                Assert.Equal(Norm(sourceLabel), Norm(backSheet[6]));
                Assert.Equal(expectedRows, int.Parse(backSheet[0]));
            }
            finally
            {
                await page.CloseAsync();
            }
        }
    }
}
