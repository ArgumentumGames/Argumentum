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

        /// <summary>Same page, at an explicit viewport — the #2400 controls run desktop AND phone.
        /// <paramref name="beforeNavigate"/> runs on the fresh page before any of its requests
        /// leaves: Cap27 installs there the route that holds the document's web fonts back.</summary>
        private async Task<IPage> OpenPageAtAsync(string wrapperPath, int width, int height,
            Func<IPage, Task>? beforeNavigate = null)
        {
            var page = await _browser.NewPageAsync(new BrowserNewPageOptions
            {
                ViewportSize = new ViewportSize { Width = width, Height = height }
            });
            if (beforeNavigate != null) await beforeNavigate(page);
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
        /// <paramref name="beforeNavigate"/> runs on the fresh page before the route below and
        /// before the navigation (Cap27).
        /// </summary>
        private async Task<IPage> OpenExternalAsync(string lang, string svgFileName, int width, int height,
            Func<IPage, Task>? beforeNavigate = null)
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
            if (beforeNavigate != null) await beforeNavigate(page);
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
        private const string CardFitSweepJs = @"(arg) => {
            // Optional argument (Cap25), all three properties optional:
            //   minChars — before a node is opened, its title is lengthened to at least this many
            //              characters by repeating its own label (script and direction preserved);
            //   only     — the keys of the only nodes to open (the fast inverse controls);
            //   sample   — open only this many nodes: a third of them the longest natural labels,
            //              a third the busiest sheets, the rest evenly spaced over document order.
            //              Opening all 1408 sheets per case cost the CI ~8 min on every pull
            //              request (#2403), so the long-title stress takes a declared sample and
            //              proves below that it kept the extremes it was chosen for;
            //   noTextCut — judge the text even when its frame scrolls (Cap20/Cap21 only: their
            //              titles are natural, so a scrolling frame there is a defect; Cap25
            //              lengthens titles on purpose and keeps the last-resort scroll).
            const opt = arg || {};
            const minChars = opt.minChars || 0;
            const only = opt.only || null;
            const sample = opt.sample > 0 ? Math.floor(opt.sample) : 0;
            const noTextCut = !!opt.noTextCut;
            const host = document.getElementById('svgObject');
            const nodeDoc = (host && host.contentDocument) ? host.contentDocument : document;
            const all = Array.from(nodeDoc.querySelectorAll('g.node'));
            const nav = window.__mmNav;
            const card = document.querySelector('card');
            const q = (s) => card.querySelector(s);
            const rel = q('.relations'), foot = q('.footer'), body = q('.body'),
                  texte = q('.texte'), img = q('.imageSection'),
                  desc = q('.desc_fr'), exemple = q('.exemple_fr');
            const link = document.getElementById('externalLink');
            const R = (el) => el.getBoundingClientRect();
            // The TEXT area of a block, its own padding removed: the footer badge sits on the
            // family-coloured band's corner by design, and only a glyph under it is a defect.
            const contentRect = (el) => {
                const r = R(el), cs = getComputedStyle(el);
                const pl = parseFloat(cs.paddingLeft) || 0, pr = parseFloat(cs.paddingRight) || 0,
                      pt = parseFloat(cs.paddingTop) || 0, pb = parseFloat(cs.paddingBottom) || 0;
                return { left: r.left + pl, right: r.right - pr, top: r.top + pt, bottom: r.bottom - pb };
            };
            const overlap = (a, b) => Math.min(a.right, b.right) > Math.max(a.left, b.left) + 0.5
                && Math.min(a.bottom, b.bottom) > Math.max(a.top, b.top) + 0.5;
            const out = { all: all.length, nodes: 0, withRelations: 0, withLink: 0,
                          withoutLink: 0, violations: [], tally: {},
                          minTitleChars: Infinity, maxTitleChars: 0 };
            const NOTE = (name, key) => {
                out.tally[name] = (out.tally[name] || 0) + 1;
                if (out.violations.length < 12) out.violations.push(key + ':' + name);
            };
            const tol = 0.6;
            // Relations per path, counted the way the card fills its own relation list (source or
            // target): the busiest sheet is the one with the most rows on screen.
            const relCount = {};
            (nav.linkRecs || []).forEach((l) => {
                if (l.source) relCount[l.source] = (relCount[l.source] || 0) + 1;
                if (l.target) relCount[l.target] = (relCount[l.target] || 0) + 1;
            });
            const labelLen = (e) => (e.rec.label || '').length;
            const opened = [];   // every node the navigation index resolves, in document order
            for (const n of all) {
                const rec = nav.byPk[n.getAttribute('pk')] || nav.byPath[n.getAttribute('path')];
                if (!rec) continue;
                const key = n.getAttribute('pk') || n.getAttribute('path') || '?';
                if (only && only.indexOf(key) < 0) continue;
                opened.push({ n: n, rec: rec, key: key });
            }
            let targets = opened;
            if (sample > 0 && opened.length > sample) {
                const quota = Math.floor(sample / 3);
                const byKey = (a, b) => (a.key < b.key ? -1 : a.key > b.key ? 1 : 0);
                const byLabel = opened.slice().sort((a, b) => labelLen(b) - labelLen(a) || byKey(a, b));
                const byRel = opened.slice().sort(
                    (a, b) => (relCount[b.rec.path] || 0) - (relCount[a.rec.path] || 0) || byKey(a, b));
                const chosen = new Set();
                byLabel.slice(0, quota).forEach((e) => chosen.add(e.key));
                byRel.slice(0, quota).forEach((e) => chosen.add(e.key));
                for (let i = 0; i < sample && chosen.size < sample; i++)
                    chosen.add(opened[Math.floor(i * opened.length / sample)].key);
                for (const e of opened) {
                    if (chosen.size >= sample) break;
                    chosen.add(e.key);
                }
                targets = opened.filter((e) => chosen.has(e.key));
            }
            const longestLabel = (arr) => arr.reduce((m, e) => Math.max(m, labelLen(e)), 0);
            const busiest = (arr) => arr.reduce((m, e) => Math.max(m, relCount[e.rec.path] || 0), 0);
            for (const t of targets) {
                const n = t.n, rec = t.rec, key = t.key;
                if (minChars > 0) {
                    const base = ((rec.label || n.textContent || '').trim()) || 'titre';
                    let long = base;
                    while (long.length < minChars) long += ' — ' + base;
                    const textEl = n.querySelector('text');
                    if (textEl) textEl.textContent = long;
                }
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
                // #2403 reserve 1 — the badge must clear the TEXT, not only the boxes: it overlaps
                // the example's box on 703 of 1408 sheets, which is that block's own padding corner.
                // Measured on the fix: 0 sheet where a glyph is under the badge (fr, ar).
                const footOverText = (desc && getComputedStyle(desc).display !== 'none'
                                      && overlap(contentRect(desc), fr))
                    || (exemple && getComputedStyle(exemple).display !== 'none'
                                      && overlap(contentRect(exemple), fr));
                if (footOverText) NOTE('footerOverText', key);
                const tchars = (title.textContent || '').length;
                if (tchars > out.maxTitleChars) out.maxTitleChars = tchars;
                if (tchars < out.minTitleChars) out.minTitleChars = tchars;
                // #2403 — a scrolling frame that cuts the text does not mean the sheet fits: the sweep
                // used to count it as clean, so a frame showing 128 of 186 px read as green. With
                // `noTextCut` (natural titles) the frame's own overflow is a violation; otherwise
                // only a frame clipped by the card is (Cap25 lengthens titles on purpose).
                const textOverflows = texte.scrollHeight > texte.clientHeight + 1;
                if (textOverflows && (noTextCut || !texte.classList.contains('mm-scroll')))
                    NOTE('textClipped', key + (texte.classList.contains('mm-scroll') ? '(scroll)' : ''));

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
                    // #2403 reserve 2 — in a right-to-left sheet the rows must start at the
                    // inline start (the right): `text-align: left` pushed them to the far side.
                    if (rows) {
                        const r0 = getComputedStyle(rel.querySelector('.mm-relation'));
                        if (r0.direction === 'rtl' && r0.textAlign !== 'start')
                            NOTE('relationRowsNotAtInlineStart', key);
                    }
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
                        // A target, not a 0x0 anchor. The height floor follows the viewport (20 px
                        // at 900 px tall, 12.6 px at 450): a fixed 20 px floor would call the
                        // pre-#2400 fixed-px link correct and its vh-sized replacement too
                        // small on a short viewport.
                        const minTargetH = Math.min(20, 0.028 * window.innerHeight);
                        if (!(lr.width >= 40 && lr.height >= minTargetH - 0.5)) NOTE('linkTooSmall', key);
                        if (lr.top < ir.top - tol || lr.bottom > ir.bottom + tol) NOTE('linkOutsideImageSection', key);
                        // #2403 — the link follows the sheet: ONE line, and an imprint of at most
                        // 9 % of the card's height (box + margins). Pre-fix (6c2505d1), one line at
                        // 10.8 % on a CSS 1280x900 viewport, but two lines on 1361 of 1361 link-bearing
                        // sheets once the sheet is short: 33.3 % of the card at 512x360, 26.7 % at
                        // 640x450 (the same 96 px of box + margins over a shorter card). Post-fix:
                        // one line on every link-bearing sheet, 5.7 % to 7.6 %, fr and ar, 512x360
                        // to 1280x900.
                        // The line count is read from the TEXT RANGE, not from the box: the box
                        // carries the padding, so at the 10 px floor (Cap25's long titles) a
                        // one-line link is 20.5 px tall against a 18.4 px bound and read as two
                        // lines on 194 of 197 sheets — measured, false.
                        const rngT = document.createRange();
                        rngT.selectNodeContents(link);
                        if (rngT.getClientRects().length > 1) NOTE('linkOnTwoLines', key);
                        const lmt = parseFloat(ls.marginTop) || 0, lmb = parseFloat(ls.marginBottom) || 0;
                        if (lr.height + lmt + lmb > 0.09 * cr.height) NOTE('linkFootprintTooTall', key);
                        if (lr.width > ir.width + tol) NOTE('linkWiderThanImageSection', key);
                    }
                } else {
                    out.withoutLink++;
                    if (shown) NOTE('linkShownWithoutDestination', key);
                    if (link.hasAttribute('href')) NOTE('linkHrefWithoutDestination', key);
                }
                card.dispatchEvent(new MouseEvent('mouseleave'));
            }
            // Fields 7 and 8: shortest and longest title the sweep opened (characters).
            // Fields 9 to 12: the longest natural label and the highest relation count over the
            // whole map, then over what was opened — equal means the sample kept both extremes.
            return [String(out.all), String(out.nodes), String(out.withRelations), String(out.withLink),
                    String(out.withoutLink), JSON.stringify(out.tally), out.violations.join(' | '),
                    String(out.nodes ? out.minTitleChars : 0), String(out.maxTitleChars),
                    String(longestLabel(opened)), String(longestLabel(targets)),
                    String(busiest(opened)), String(busiest(targets))];
        }";

        /// <summary>Asserts the sweep found no violation, and states what it actually covered.</summary>
        private void AssertSweepClean(string[] sweep, string lang, string variant, int width, int height,
                                      int sampledNodes = 0)
        {
            Assert.NotNull(sweep);
            Assert.True(sweep.Length >= 7, "the sweep must return 7 fields");
            var (all, nodes, withRel, withLink, withoutLink) = (
                int.Parse(sweep[0]), int.Parse(sweep[1]), int.Parse(sweep[2]),
                int.Parse(sweep[3]), int.Parse(sweep[4]));
            _output.WriteLine($"[{lang}/{variant} {width}x{height}] nodes={nodes}/{all} " +
                              $"withRelations={withRel} withLink={withLink} withoutLink={withoutLink} " +
                              $"titleChars={(sweep.Length > 8 ? sweep[7] + ".." + sweep[8] : "?")} tally={sweep[5]}");
            // A full sweep must reach every navigable node; a declared sample must reach exactly its
            // size, bounded by the corpus. Anything else means the navigation index did not resolve
            // and the assertions below would be vacuous.
            var wanted = sampledNodes > 0 ? Math.Min(sampledNodes, all) : all;
            Assert.True(nodes >= 200 && nodes == wanted,
                $"the sweep must cover the nodes it promised ({nodes} opened, wanted {wanted}, map {all}) — " +
                "another count means the navigation index did not resolve and the assertions below would be vacuous");
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
                var sweep = await page.EvaluateAsync<string[]>(CardFitSweepJs, new { noTextCut = true });
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
                var sweep = await page.EvaluateAsync<string[]>(CardFitSweepJs, new { noTextCut = true });
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

        /// <summary>
        /// #2400 — "un titre long". A declared sample of the map's sheets is opened with a title
        /// lengthened to at least <see cref="LongTitleChars"/> characters and must still fit the
        /// card. The title is lengthened by repeating the node's own label, so script and writing
        /// direction stay those of the language under test. The sweep's three title notes had never
        /// been seen to fire on the corpus: the second half of the control injects one style per
        /// note — a title cut by its box, a title leaving the image section, a header band leaving
        /// the card — and requires the note to fire, else "0 violation" would only say that no
        /// title was long enough.
        /// <para>
        /// Why a sample and not every sheet: opening all 1408 sheets with a long title in each of
        /// the eight cases took the behavioural workflow from about 4 minutes to 11 min 59 s, and
        /// that workflow runs on every pull request. The property under test depends on the title's
        /// length, the number of relations and whether the sheet carries a link — not on which
        /// node carries them. The sample therefore keeps the longest natural labels, the busiest
        /// sheets and evenly spaced ones, and the test asserts that both extremes are really in it.
        /// The exhaustive sweep was measured once on these same template bytes, at commit e1ebce3d
        /// of #2403: 34 controls red on the pre-#2400 templates, 102 green on the fix.
        /// </para>
        /// </summary>
        private const int LongTitleChars = 240;

        /// <summary>Sheets opened per case by the long-title sweep — see the rationale above.</summary>
        private const int LongTitleSample = 200;

        private static readonly (string Css, string Note)[] TitleNoteMutants =
        {
            (".title { max-height: 6px !important; overflow: hidden !important; }", "titleClipped"),
            (".title { position: relative !important; top: 2000px !important; }", "titleOutsideImageSection"),
            (".header { position: relative !important; top: 2000px !important; }", "headerOutsideCard"),
        };

        private const string FirstNodeKeyJs = @"() => {
            const nav = window.__mmNav;
            for (const k of Object.keys(nav.byPk)) if (nav.byPk[k]) return k;
            return '';
        }";

        /// <summary>Installs, replaces or — with an empty argument — removes one style element.</summary>
        private const string SetMutantStyleJs = @"css => {
            let st = document.getElementById('p2400-mutant');
            if (!css) { if (st) st.remove(); return; }
            if (!st) { st = document.createElement('style'); st.id = 'p2400-mutant'; document.head.appendChild(st); }
            st.textContent = css;
        }";

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline", 390, 844)]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline", 1400, 900)]
        [InlineData("fr", "Fallacies_fr.content.svg", "ext", 390, 844)]
        [InlineData("fr", "Fallacies_fr.content.svg", "ext", 1400, 900)]
        [InlineData("ar", "Fallacies_ar.content.svg", "inline", 390, 844)]
        [InlineData("ar", "Fallacies_ar.content.svg", "inline", 1400, 900)]
        [InlineData("ar", "Fallacies_ar.content.svg", "ext", 390, 844)]
        [InlineData("ar", "Fallacies_ar.content.svg", "ext", 1400, 900)]
        public async Task Cap25_LongTitle_ASampleOfSheetsStillFits_AndTheControlSeesACutTitle(
            string lang, string svgFileName, string variant, int width, int height)
        {
            var page = variant == "ext"
                ? await OpenExternalAsync(lang, svgFileName, width, height)
                : await OpenPageAtAsync(await ComposeIncludedAsync(lang, svgFileName), width, height);
            try
            {
                if (variant == "ext") await WaitForExternalNavigationAsync(page);
                else await WaitForViewportSettledAsync(page);

                var longSweep = await page.EvaluateAsync<string[]>(CardFitSweepJs,
                    new { minChars = LongTitleChars, sample = LongTitleSample });
                AssertSweepClean(longSweep, lang, variant + "+longTitle", width, height, LongTitleSample);
                Assert.True(int.Parse(longSweep[7]) >= LongTitleChars,
                    $"the sweep must really have opened long titles: the shortest it opened is {longSweep[7]} " +
                    $"characters, wanted at least {LongTitleChars}");
                Assert.True(longSweep.Length >= 13, "the sweep must report the extremes it was asked to keep");
                Assert.Equal(longSweep[9], longSweep[10]);   // the longest natural label of the map is in the sample
                Assert.Equal(longSweep[11], longSweep[12]);  // so is its busiest sheet
                _output.WriteLine($"[{lang}/{variant} {width}x{height}] long title over {longSweep[1]} sheets " +
                                  $"of {longSweep[0]}: longest label {longSweep[9]} chars, " +
                                  $"busiest sheet {longSweep[11]} relations, titles {longSweep[7]}..{longSweep[8]} chars");

                var key = await page.EvaluateAsync<string>(FirstNodeKeyJs);
                Assert.False(string.IsNullOrEmpty(key), "the navigation index must offer at least one node");

                foreach (var (css, note) in TitleNoteMutants)
                {
                    string[] mutant;
                    await page.EvaluateAsync(SetMutantStyleJs, css);
                    try
                    {
                        mutant = await page.EvaluateAsync<string[]>(CardFitSweepJs, new { only = new[] { key } });
                    }
                    finally
                    {
                        await page.EvaluateAsync(SetMutantStyleJs, "");
                    }
                    Assert.NotNull(mutant);
                    Assert.Equal("1", mutant[1]);
                    using var tally = System.Text.Json.JsonDocument.Parse(mutant[5]);
                    Assert.True(tally.RootElement.TryGetProperty(note, out var count) && count.GetInt32() >= 1,
                        $"the sweep must see '{note}' once this style is injected — tally={mutant[5]} | {css}");
                    _output.WriteLine($"[{lang}/{variant} {width}x{height}] mutant '{note}': tally={mutant[5]}");
                }
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ===== #2403 — second pass of #2400: the link follows the sheet, and a cut text is a defect =====
        //
        // Verdict of ai-01 on 6c2505d1: the link had become "the largest element of the sheet" —
        // 16 px fixed in a sheet typeset in vh — and the text paid for it; and Cap20..Cap25 counted
        // a scrolling frame that cuts the text as "the sheet fits".
        //
        // The verdict's figures are read on a CSS 512x360 viewport — a 1280x900 window on a display
        // scaled at 250 %: there the pre-#2403 link, two lines, fills the image section at
        // 146.7 x 56 CSS px, i.e. 366.8 x 140 device px (the verdict reads "about 366 x 140"), and
        // DARVO's text frame shows 130 of its 193 px (the verdict: 128 of 186). Measured on
        // 6c2505d1, `fr`, 1408 sheets: at a CSS 640x450 the link is on two lines on 1361 of 1361
        // link-bearing sheets (26.7 % of the card) and 392 sheets cut their text; at a CSS 1280x900
        // it is on one line (10.8 %) and no sheet does.
        //
        // The fix, measured twice on the same bytes with the four DINPro faces loaded — Chromium
        // 143 (the reviewer's browser) and the Chromium 124 of the suite's own driver, `fr` and
        // `ar`, 512x360 to 1280x900, 1408 sheets per run: the link is on one line on all 1361
        // link-bearing sheets in both; its imprint is 5.65 % to 7.56 % of the card in both; no
        // sheet cuts its text at 1280x900 in either; at 640x450, 3 sheets cut theirs in `fr` and
        // 0 in `ar` in both, at most 50 px hidden in 143 (#pk=200) and 35 px in 124 (#pk=1300).
        // At 512x360 the card is 288 px tall: 450 sheets in `fr` and 31 in `ar` still cut their
        // text there in 143 (452 and 31 in 124), at the 10 px floor of the type — up to 198 px
        // hidden (#pk=200, `fr`) in 143 and 156 px in 124, more than two lines on 406 in 143 and
        // 344 in 124 — a residual stated to the reviewer, not a pass. The two browsers agree on
        // which sheets owe their whole text to a yielded relations frame and on the height that
        // frame is given (#pk=1084, 7 relations, 25.19 px, in both); they differ on the counts,
        // never on the link's geometry.
        //
        // What gives the text its room, in the template: the link follows the sheet (vh-sized,
        // one line, a floor of its own), the badge is the width of the border band (2.8 vh), and
        // the relations frame yields height to the text before the text scrolls — down to one
        // whole line of its first row, and only when that makes the text whole. On 512x360, `fr`:
        // 170 sheets owe it their whole text in 143, 167 in 124.
        //
        // The link's size is 3 vh — the description's (3.2 vh), under the sub-family's (3.4 vh) —
        // and not the family label's (5.6 vh), which was measured and set aside (Chromium 124, the
        // four faces loaded): at 512x360 the `fr` label holds on one line only up to 14.5 px
        // (4.03 vh, its box at 86.3 % of the card) and `ar` up to 17 px. Declared at 5.6 vh the
        // link falls to the 15 px floor of its 20 px target and still wraps — on all 1361
        // link-bearing sheets — so 747 sheets cut their text in `fr` (452 at 3 vh), up to 178 px
        // hidden, the link's imprint at 15.23 % of the card. At 1280x900 the horizontal pass caps
        // it at 33.4 px on DARVO (wider than the description) for 7.15 % to 9.06 % of the card,
        // against 5.65 % to 7.56 % at 3 vh, with the same coverage: no sheet cuts its text in
        // either, and 3 do at 640x450 in both (36 px hidden against 35).
        //
        // And the state these contracts are read in is forced, not hoped for: the four faces come
        // from a CDN and are requested lazily, so the same sheet reads 31 px hidden with them
        // loaded and 42 px read before they arrive — and 42 px again with the CDN unreachable,
        // because the fallback face is the one laid out in both states (measured 2026-11-27,
        // Chromium 124, `fr`, 512x360, #pk=2: 42 px in both, and 0 px at 1280x900 and 640x450
        // with every font request aborted). Cap26 loads them first and fails by name if one
        // cannot be had; Cap27 walks the reader who clicks first — see the cold-document control
        // at the end of this class.
        //
        // Three sheets, named by the verdict and picked from the corpus — never a hardcoded pk.

        /// <summary>The label of the DARVO sheet per language — an independent oracle, like the
        /// link labels: in `ar` the acronym is transliterated, so a Latin match finds nothing
        /// there (measured: the pick returned no sheet in `ar`). With the parenthetical removed,
        /// compared on the normalised label.</summary>
        private static readonly System.Collections.Generic.Dictionary<string, string> P2400DarvoLabel =
            new System.Collections.Generic.Dictionary<string, string>
            {
                ["fr"] = "DARVO",
                ["ar"] = "دارفو",
            };

        /// <summary>
        /// Picks the three sheets the verdict names. The DARVO sheet is the one whose label, with
        /// its parentheticals removed, is the language's label of the DARVO sheet: if the data
        /// stops offering it the case fails loudly instead of quietly testing another sheet. The
        /// median is taken among the sheets that carry a link (the size of the link is what is
        /// under test), by relation count and by description length — the two measures the
        /// verdict names.
        /// </summary>
        private const string P2400PickThreeSheetsJs = @"(darvoWanted) => {
            const nav = window.__mmNav;
            const keys = Object.keys(nav.byPk);
            const relCount = {};
            (nav.linkRecs || []).forEach((l) => {
                if (l.source) relCount[l.source] = (relCount[l.source] || 0) + 1;
                if (l.target) relCount[l.target] = (relCount[l.target] || 0) + 1;
            });
            const norm = (s) => (s || '').replace(/\([^)]*\)/g, '').replace(/\s+/g, ' ').trim().toUpperCase();
            const num = (k) => (isNaN(Number(k)) ? 1e9 : Number(k));
            const byKey = (a, b) => num(a) - num(b) || (a < b ? -1 : a > b ? 1 : 0);
            const rels = (k) => relCount[nav.byPk[k].path] || 0;
            let darvo = '';
            for (const k of keys.slice().sort(byKey)) if (norm(nav.byPk[k].label) === norm(darvoWanted)) { darvo = k; break; }
            let busiest = '', busiestRel = -1;
            for (const k of keys.slice().sort(byKey)) {
                if (rels(k) > busiestRel) { busiest = k; busiestRel = rels(k); }
            }
            const withLink = keys.filter((k) => nav.byPk[k].el.getAttribute('link'));
            const descLen = (k) => (nav.byPk[k].el.getAttribute('description') || '').length;
            const ranks = (fn) => {
                const m = {};
                withLink.slice().sort((a, b) => fn(a) - fn(b) || byKey(a, b)).forEach((k, i) => { m[k] = i; });
                return m;
            };
            const rRel = ranks(rels), rDesc = ranks(descLen);
            const mid = Math.floor(withLink.length / 2);
            let median = '', best = Infinity;
            for (const k of withLink.slice().sort(byKey)) {
                const d = Math.abs(rRel[k] - mid) + Math.abs(rDesc[k] - mid);
                if (d < best) { best = d; median = k; }
            }
            const L = (k) => k ? (nav.byPk[k].label || '') : '';
            return [darvo, L(darvo), median, L(median), busiest, L(busiest),
                    String(busiestRel), String(rels(median)), String(descLen(median)),
                    String(withLink.length)];
        }";

        /// <summary>
        /// Opens one sheet and reads the contracts of the verdict, without deciding anything.
        /// Field 2 lists every frame of the sheet that scrolls because its content overflows:
        /// the text frame (`div.texte`) and the relations frame (`div.relations`) are the two the
        /// template can produce. Fields 11 to 13 read the relations frame: whether it still shows
        /// a whole line of its first row (a frame that gave height back to the text can be
        /// shorter than that row), how many pixels of text the text frame hides, and the height
        /// the frame was given when it yielded (empty when it kept its own).
        /// </summary>
        private const string P2400SheetContractJs = @"(pk) => {
            const nav = window.__mmNav;
            nav.focusNode(nav.byPk[pk]);
            const card = document.querySelector('card');
            const q = (s) => card.querySelector(s);
            const texte = q('.texte'), rel = q('.relations'), foot = q('.footer'),
                  img = q('.imageSection'), link = document.getElementById('externalLink');
            const R = (el) => el.getBoundingClientRect();
            const cr = R(card), fr = R(foot), ir = R(img), lr = R(link);
            const ls = getComputedStyle(link);
            const shown = ls.display !== 'none' && link.getClientRects().length > 0;
            const rngT = document.createRange();
            rngT.selectNodeContents(link);
            const lines = rngT.getClientRects().length;
            const lmt = parseFloat(ls.marginTop) || 0, lmb = parseFloat(ls.marginBottom) || 0;
            const frames = Array.from(card.querySelectorAll('*')).filter((el) => {
                const cs = getComputedStyle(el);
                return (cs.overflowY === 'auto' || cs.overflowY === 'scroll')
                    && el.scrollHeight > el.clientHeight + 1;
            }).map((el) => el.tagName.toLowerCase() + '.' +
                (typeof el.className === 'string' ? el.className.trim().replace(/\s+/g, '.') : '') +
                ':' + el.scrollHeight + '/' + el.clientHeight);
            const rows = rel.querySelectorAll('.mm-relation');
            let startGap = '';
            if (rows.length) {
                const rng = document.createRange();
                rng.selectNodeContents(rows[0]);
                const t = rng.getBoundingClientRect(), box = R(rel), cs = getComputedStyle(rel);
                const padR = parseFloat(cs.paddingRight) || 0, padL = parseFloat(cs.paddingLeft) || 0;
                startGap = getComputedStyle(rows[0]).direction === 'rtl'
                    ? String(+(box.right - padR - t.right).toFixed(1))
                    : String(+(t.left - (box.left + padL)).toFixed(1));
            }
            let relLine = 'relNoRows';
            if (rows.length) {
                const rcs = getComputedStyle(rel), rowCs = getComputedStyle(rows[0]);
                const inner = rel.clientHeight
                    - (parseFloat(rcs.paddingTop) || 0) - (parseFloat(rcs.paddingBottom) || 0);
                const line = parseFloat(rowCs.lineHeight) || parseFloat(rowCs.fontSize) || 12;
                relLine = inner >= line - 0.5 ? 'relFrameShowsALine' : 'relFrameBelowALine';
            }
            const content = (el) => {
                const r = R(el), cs = getComputedStyle(el);
                const pl = parseFloat(cs.paddingLeft) || 0, pr = parseFloat(cs.paddingRight) || 0,
                      pt = parseFloat(cs.paddingTop) || 0, pb = parseFloat(cs.paddingBottom) || 0;
                return { left: r.left + pl, right: r.right - pr, top: r.top + pt, bottom: r.bottom - pb };
            };
            const overlap = (a, b) => Math.min(a.right, b.right) > Math.max(a.left, b.left) + 0.5
                && Math.min(a.bottom, b.bottom) > Math.max(a.top, b.top) + 0.5;
            let overText = false;
            for (const s of ['.desc_fr', '.exemple_fr']) {
                const el = q(s);
                if (el && getComputedStyle(el).display !== 'none' && overlap(content(el), fr)) overText = true;
            }
            return [
                pk,
                texte.scrollHeight > texte.clientHeight + 1 ? 'textCut' : 'textFits',
                frames.join(','),
                !shown ? 'linkHidden' : (lines > 1 ? 'linkTwoLines' : 'linkOneLine'),
                String(+((lr.height + lmt + lmb) / cr.height).toFixed(4)),
                lr.left >= ir.left - 0.6 && lr.right <= ir.right + 0.6
                    && lr.top >= ir.top - 0.6 && lr.bottom <= ir.bottom + 0.6
                    ? 'insideImageSection' : 'outsideImageSection',
                String(+fr.width.toFixed(1)),
                String(+fr.height.toFixed(1)),
                overText ? 'overText' : 'clearOfText',
                String(rows.length),
                startGap,
                relLine,
                String(Math.max(0, texte.scrollHeight - texte.clientHeight)),
                rel.style.maxHeight || ''
            ];
        }";

        /// <summary>
        /// Finds, at the viewport the page has now, the sheet that owes its whole text to its
        /// relations frame: the frame carries a height of its own (it yielded) and the text frame
        /// no longer overflows. Among those, the one with the most relations — the case the
        /// verdict allows a scrolling relations frame for. A viewport where no sheet needed the
        /// yield returns an empty key: the caller says so instead of reading a pass there.
        /// </summary>
        private const string P2400PickYieldedJs = @"() => {
            const nav = window.__mmNav;
            const card = document.querySelector('card');
            const rel = card.querySelector('.relations'), texte = card.querySelector('.texte');
            const relCount = {};
            (nav.linkRecs || []).forEach((l) => {
                if (l.source) relCount[l.source] = (relCount[l.source] || 0) + 1;
                if (l.target) relCount[l.target] = (relCount[l.target] || 0) + 1;
            });
            const num = (k) => (isNaN(Number(k)) ? 1e9 : Number(k));
            const byKey = (a, b) => num(a) - num(b) || (a < b ? -1 : a > b ? 1 : 0);
            let best = '', bestRel = -1, yielded = 0, frame = '';
            for (const k of Object.keys(nav.byPk).sort(byKey)) {
                nav.focusNode(nav.byPk[k]);
                if (!rel.style.maxHeight || texte.scrollHeight > texte.clientHeight + 1) continue;
                yielded++;
                const c = relCount[nav.byPk[k].path] || 0;
                if (c > bestRel) { bestRel = c; best = k; frame = rel.style.maxHeight; }
            }
            card.dispatchEvent(new MouseEvent('mouseleave'));
            return [best, best ? (nav.byPk[best].label || '') : '', String(bestRel), String(yielded), frame];
        }";

        /// <summary>The upper bound of the link's imprint (box + margins) as a share of the card's
        /// height — the same 9 % the corpus sweep notes as `linkFootprintTooTall`. Calibrated
        /// between the two measured states: 10.8 % at a CSS 1280x900 viewport and 33.3 % at 512x360
        /// (26.7 % at 640x450 — the same 96 px of box + margins over a taller card) before; 5.7 % to
        /// 7.6 % after over the 1361 link-bearing sheets, `fr` and `ar`, 512x360
        /// to 1280x900.</summary>
        private const double LinkImprintMax = 0.09;

        /// <summary>How many pixels of text the busiest sheet may still hide at the verdict's own
        /// window (CSS 512x360): 31 px measured in `fr` on #pk=2, whose 170 px of text need more
        /// than the 139 px its frame holds once the type is at its 10 px floor and the relations
        /// frame is down to one line (it gives nothing back there: the text stays cut, so the frame
        /// keeps its own height). In `ar` the same sheet is whole.
        ///
        /// That reading is a function of the document's web-font state, which the case now
        /// forces before it reads anything: 31 px with the four DINPro faces loaded, 42 px on the
        /// same sheet read before they arrive, and 42 px again with the CDN unreachable — the
        /// fallback face is the one laid out in both states, so "the fonts will never come" and
        /// "the fonts are on their way" read alike (measured 2026-11-27 in the Chromium 124 of
        /// the suite's own driver, all font requests aborted; 0 px hidden at 1280x900 and at
        /// 640x450 with the CDN down). The bound keeps its value beside the force rather than
        /// growing to admit the states the force removes: with the faces loaded the reading does
        /// not depend on them arriving, and a case that silently measured 42 px would be
        /// measuring a state no reader with the CDN up ever sees. A bound, not a target — it
        /// leaves one line of floor-size type (13.5 px) of slack over the loaded reading, and a
        /// corpus edit that deepens the deficit trips it.</summary>
        private const double ResidualHiddenPxMax = 48;

        /// <summary>The three viewports of the control, shortest last: a CSS 1280x900 one, a
        /// 640x450 one, and the verdict's own window — a 1280x900 window on a display scaled at
        /// 250 % is a CSS 512x360 viewport.</summary>
        private static readonly (int Width, int Height)[] P2403Viewports =
            { (1280, 900), (640, 450), (512, 360) };

        /// <summary>The four DINPro faces the sheet lays its own text in, in the `document.fonts`
        /// shorthand. They come from a CDN and are requested lazily — the 900 one by the first
        /// sheet whose text is laid out — so "the faces are loaded" is a state the case has to
        /// force, not a property it may assume (see the residual bound above).</summary>
        private static readonly string[] P2403FontSpecs =
            { "300 12px DINPro", "500 12px DINPro", "700 12px DINPro", "900 12px DINPro" };

        /// <summary>Loads the wanted faces and reports each as `spec=loaded` or `spec=missing`;
        /// the caller fails on a missing one, naming it, instead of measuring the fallback font's
        /// layout in its place.</summary>
        private const string P2403FontsJs = @"async (specs) => {
            for (const spec of specs) {
                try { await document.fonts.load(spec); } catch (e) { /* reported by check() below */ }
            }
            await document.fonts.ready;
            return specs.map((spec) => spec + '=' + (document.fonts.check(spec) ? 'loaded' : 'missing'));
        }";

        /// <summary>Reads the open sheet's frames WITHOUT opening it again: the contract probe
        /// refits the sheet, so it cannot read what a late event left behind. Returns the pixels
        /// of text the frame hides, whether the text scrolls, the height the relations frame was
        /// given, the raw `scrollHeight/clientHeight` of the text, and the type the three fitted
        /// elements were left at — the size is the part of the fit a font swap silently
        /// invalidates (it is chosen for the font that was laid out).</summary>
        private const string P2403OpenSheetFramesJs = @"() => {
            const card = document.querySelector('card');
            const texte = card.querySelector('.texte'), rel = card.querySelector('.relations');
            const px = (s) => { const el = card.querySelector(s); return el ? getComputedStyle(el).fontSize : ''; };
            return [String(Math.max(0, texte.scrollHeight - texte.clientHeight)),
                    texte.classList.contains('mm-scroll') ? 'textScrolls' : 'textNoScroll',
                    rel.style.maxHeight || '',
                    texte.scrollHeight + '/' + texte.clientHeight,
                    px('.desc_fr'), px('.exemple_fr'), px('#externalLink')];
        }";

        /// <summary>The link rule of the pre-#2403 template, reinjected verbatim: 16 px, two lines,
        /// the imprint of 78 px. The mutation the verdict asks for.</summary>
        private const string LinkPxMutantCss =
            "#externalLink { font-size: 16px !important; line-height: normal !important; " +
            "padding: 10px 20px !important; margin: 20px auto !important; white-space: normal !important; }";

        /// <summary>The badge of the pre-#2403 template: 4 vh, wider than the 2.8 vh border band.</summary>
        private const string FooterVh4MutantCss = ".footer { width: 4vh !important; height: 4vh !important; }";

        /// <summary>The relation rows of the pre-#2403 template, pushed to the far side of an
        /// right-to-left sheet. Measured on #pk=1084 in `ar`: 126 px from the inline start.</summary>
        private const string RowsLeftMutantCss = "card .relations .mm-relation { text-align: left !important; }";

        /// <summary>A text frame that cuts what it shows — the state Cap20..Cap25 used to pass.</summary>
        private const string TextCutMutantCss = "card .texte { max-height: 60px !important; overflow-y: auto !important; }";

        private static double Num(string s) => double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline")]
        [InlineData("fr", "Fallacies_fr.content.svg", "ext")]
        [InlineData("ar", "Fallacies_ar.content.svg", "inline")]
        [InlineData("ar", "Fallacies_ar.content.svg", "ext")]
        public async Task Cap26_LinkFollowsTheSheet_AndNoFrameCutsTheText_OnTheNamedSheets(
            string lang, string svgFileName, string variant)
        {
            var page = variant == "ext"
                ? await OpenExternalAsync(lang, svgFileName, 1280, 900)
                : await OpenPageAtAsync(await ComposeIncludedAsync(lang, svgFileName), 1280, 900);
            try
            {
                if (variant == "ext") await WaitForExternalNavigationAsync(page);
                else await WaitForViewportSettledAsync(page);

                var pick = await page.EvaluateAsync<string[]>(P2400PickThreeSheetsJs, P2400DarvoLabel[lang]);
                Assert.NotNull(pick);
                Assert.True(pick.Length >= 10 && !string.IsNullOrEmpty(pick[0]) && !string.IsNullOrEmpty(pick[2])
                            && !string.IsNullOrEmpty(pick[4]),
                    $"#2403 [{lang}/{variant}]: the fixture must offer a DARVO sheet, a median one and a busiest one — got {string.Join(" | ", pick)}");
                var (darvoPk, medianPk, busiestPk) = (pick[0], pick[2], pick[4]);
                Assert.Contains(P2400DarvoLabel[lang], pick[1]);
                Assert.True(int.Parse(pick[6]) >= 3, "the busiest sheet must carry enough relations for its frame to scroll");
                _output.WriteLine($"[{lang}/{variant}] DARVO pk={darvoPk} '{pick[1]}' | median pk={medianPk} '{pick[3]}' " +
                                  $"({pick[7]} relations, description {pick[8]} chars, among {pick[9]} link-bearing sheets) | " +
                                  $"busiest pk={busiestPk} '{pick[5]}' ({pick[6]} relations)");

                // The three roles may fall on one sheet (a median sheet that is also the busiest):
                // each sheet is judged once, under its last role — `busiest` is the only one
                // allowed a scrolling frame, and it must keep that allowance.
                var sheets = new[] { (darvoPk, "darvo"), (medianPk, "median"), (busiestPk, "busiest") }
                    .GroupBy(x => x.Item1).Select(g => g.Last()).ToList();

                // ---- the state the contracts are read in -----------------------------------------
                // The sheet's fit is a function of the font actually laid out, and the four DINPro
                // faces are requested lazily: a read taken before they arrive measures the
                // fallback font's geometry (42 px hidden on #pk=2 where the loaded state hides 31
                // — and the same 42 px with the CDN unreachable, the fallback face being the one
                // laid out in both states; measured 2026-11-27). Force them first, and fail by name if one
                // cannot be had — a silent fallback reading would pass this control on a state no
                // reader with the CDN up ever sees.
                var faces = await page.EvaluateAsync<string[]>(P2403FontsJs, P2403FontSpecs);
                Assert.NotNull(faces);
                foreach (var face in faces)
                    Assert.True(face.EndsWith("=loaded", StringComparison.Ordinal),
                        $"#2403 [{lang}/{variant}]: the sheet's web font is not available — '{face}'. The contract is " +
                        "read in the loaded state only; check fonts.cdnfonts.com before reading anything into this run.");
                _output.WriteLine($"[{lang}/{variant}] faces: {string.Join(", ", faces)}");

                // ---- the contracts, on the three viewports ---------------------------------------
                var yieldedSeen = 0;
                foreach (var (width, height) in P2403Viewports)
                {
                    await page.SetViewportSizeAsync(width, height);
                    var verdictWindow = width == 512;
                    foreach (var (pk, role) in sheets)
                    {
                        var s = await page.EvaluateAsync<string[]>(P2400SheetContractJs, pk);
                        Assert.NotNull(s);
                        _output.WriteLine($"[{lang}/{variant} {width}x{height}] {role} pk={pk}: {s[1]} frames='{s[2]}' " +
                                          $"link={s[3]} imprint={s[4]} badge={s[6]}x{s[7]} {s[8]} rows={s[9]} " +
                                          $"startGap={s[10]} relFrame={s[11]} hidden={s[12]}px");
                        Assert.Equal("linkOneLine", s[3]);
                        Assert.True(Num(s[4]) <= LinkImprintMax,
                            $"#2403 [{lang}/{variant} {width}x{height}] {role} pk={pk}: the link's imprint is {s[4]} of the card's " +
                            $"height, wanted at most {LinkImprintMax}");
                        Assert.Equal("insideImageSection", s[5]);
                        var wantedBadge = 0.028 * height;
                        Assert.True(Math.Abs(Num(s[6]) - wantedBadge) <= 1.5 && Math.Abs(Num(s[7]) - wantedBadge) <= 1.5,
                            $"#2403 [{lang}/{variant} {width}x{height}] {role} pk={pk}: the badge is {s[6]}x{s[7]} px, wanted the " +
                            $"{wantedBadge:F1}px of the border band");
                        Assert.Equal("clearOfText", s[8]);
                        if (!string.IsNullOrEmpty(s[10]))
                            Assert.True(Num(s[10]) <= 1.0,
                                $"#2403 [{lang}/{variant} {width}x{height}] {role} pk={pk}: the relation rows start {s[10]} px " +
                                "from the inline start");
                        Assert.NotEqual("relFrameBelowALine", s[11]);
                        // The verdict's control: no scrolling frame on DARVO and on a median sheet.
                        // At the verdict's own window the busiest sheet is the one case the type
                        // cannot make whole (measured in `fr`: 170 px of text for 139 at the 10 px
                        // floor, its relations frame back at its own height because one line of it
                        // did not suffice): it is held to a bounded residual rather than passed
                        // over in silence.
                        if (role == "busiest" && verdictWindow && s[1] == "textCut")
                        {
                            Assert.True(Num(s[12]) <= ResidualHiddenPxMax,
                                $"#2403 [{lang}/{variant} {width}x{height}] {role} pk={pk}: the text frame hides {s[12]} px, " +
                                $"above the {ResidualHiddenPxMax} px measured at this window");
                            foreach (var frame in s[2].Split(','))
                                Assert.True(frame.StartsWith("div.relations") || frame.StartsWith("div.texte.mm-scroll"),
                                    $"#2403 [{lang}/{variant} {width}x{height}] {role} pk={pk}: unexpected frame '{frame}'");
                            continue;
                        }
                        Assert.Equal("textFits", s[1]);
                        if (!string.IsNullOrEmpty(s[2]))
                        {
                            Assert.True(role == "busiest",
                                $"#2403 [{lang}/{variant} {width}x{height}] {role} pk={pk}: only the busiest sheet may keep a " +
                                $"scrolling frame, got '{s[2]}'");
                            foreach (var frame in s[2].Split(','))
                                Assert.StartsWith("div.relations", frame);
                        }
                    }

                    // #2403 (2e passe) — the relations frame yields height to the text before the
                    // text scrolls, and only when that makes the text whole: the sheet that owes
                    // its whole text to its frame is judged here, at this viewport.
                    var y = await page.EvaluateAsync<string[]>(P2400PickYieldedJs);
                    Assert.NotNull(y);
                    if (string.IsNullOrEmpty(y[0]))
                    {
                        _output.WriteLine($"[{lang}/{variant} {width}x{height}] no sheet needed its relations frame to " +
                                          "yield here — the yield is not exercised at this viewport");
                    }
                    else
                    {
                        yieldedSeen++;
                        var ys = await page.EvaluateAsync<string[]>(P2400SheetContractJs, y[0]);
                        _output.WriteLine($"[{lang}/{variant} {width}x{height}] yielded pk={y[0]} '{y[1]}' " +
                                          $"({y[2]} relations, frame {y[4]}, {y[3]} sheets rescued at this viewport): " +
                                          $"{ys[1]} frames='{ys[2]}' relFrame={ys[11]}");
                        Assert.Equal("textFits", ys[1]);
                        Assert.False(string.IsNullOrEmpty(ys[2]),
                            $"#2403 [{lang}/{variant} {width}x{height}] the rescued sheet pk={y[0]} must keep its relations " +
                            "frame scrolling");
                        foreach (var frame in ys[2].Split(','))
                            Assert.StartsWith("div.relations", frame);
                        Assert.Equal("relFrameShowsALine", ys[11]);
                        Assert.True(int.Parse(y[2]) >= 1,
                            $"#2403 [{lang}/{variant} {width}x{height}] the rescued sheet pk={y[0]} must be a " +
                            $"relation-bearing sheet, got {y[2]}");
                    }
                }
                Assert.True(yieldedSeen >= 1,
                    "#2403: no viewport exercised the relations frame yielding — the pass that gives the text its room " +
                    "is untested");

                // ---- each contract must be able to fail ------------------------------------------
                async Task<string[]> UnderAsync(string css, string pk)
                {
                    string[] s;
                    await page.EvaluateAsync(SetMutantStyleJs, css);
                    try { s = await page.EvaluateAsync<string[]>(P2400SheetContractJs, pk); }
                    finally { await page.EvaluateAsync(SetMutantStyleJs, ""); }
                    return s;
                }

                // 1. The mutation the verdict asks for: the link back to 16 px on two lines. Every
                //    viewport must see it; and at the verdict's own window the two lines must be
                //    what pushes a named sheet's text into its frame — the defect read on the first
                //    head — so that "the text is whole" is a contract the link can break.
                var cutByLink = false;
                foreach (var (width, height) in P2403Viewports)
                {
                    await page.SetViewportSizeAsync(width, height);
                    var tripped = false;
                    foreach (var (pk, role) in sheets)
                    {
                        var m = await UnderAsync(LinkPxMutantCss, pk);
                        var twoLines = m[3] == "linkTwoLines";
                        var tooTall = Num(m[4]) > LinkImprintMax;
                        _output.WriteLine($"[{lang}/{variant} {width}x{height}] mutant 'link back to 16 px': {role} pk={pk} " +
                                          $"link={m[3]} imprint={m[4]} text={m[1]} frames='{m[2]}' hidden={m[12]}px");
                        if (width == 512 && role != "busiest" && m[1] == "textCut") cutByLink = true;
                        tripped |= twoLines || tooTall;
                    }
                    Assert.True(tripped,
                        $"#2403 [{lang}/{variant} {width}x{height}]: putting the link back to 16 px must break the control — " +
                        "neither 'one line' nor the imprint bound saw it");
                }
                Assert.True(cutByLink,
                    $"#2403 [{lang}/{variant}]: at the verdict's window (512x360) the link back to 16 px on two lines must " +
                    "cut the text of the DARVO or of the median sheet — the control would not see the defect it exists for");

                // 2. A text frame that cuts what it shows.
                await page.SetViewportSizeAsync(1280, 900);
                var cut = await UnderAsync(TextCutMutantCss, darvoPk);
                Assert.NotEqual("textFits", cut[1]);
                Assert.Contains("div.texte", cut[2]);

                // 3. The badge back to 4 vh.
                var badge = await UnderAsync(FooterVh4MutantCss, darvoPk);
                Assert.True(Math.Abs(Num(badge[6]) - 0.028 * 900) > 1.5,
                    $"#2403: a 4 vh badge must break the control, got {badge[6]} px");

                // 4. The relation rows pushed to the far side — meaningful in a right-to-left
                //    sheet only: in ltr `left` and `start` lay the text at the same place.
                if (lang == "ar")
                {
                    var rows = await UnderAsync(RowsLeftMutantCss, busiestPk);
                    Assert.True(Num(rows[10]) > 10,
                        $"#2403: `text-align: left` must push the rows away from the inline start, gap={rows[10]}");
                }
                else
                {
                    _output.WriteLine($"[{lang}/{variant}] mutant 'rows to the left' not run: the sheet is left-to-right");
                }
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        /// <summary>
        /// The first sheet a reader opens on a document whose web fonts are not in cache yet.
        /// DINPro is requested by the first layout of a sheet's text, so that sheet is fitted on
        /// the fallback font — and, on the template this case was written against, nothing
        /// refitted it when the real font swapped in. Measured there (Chromium 124, CSS 512x360,
        /// `fr`, a fresh browser context per sheet, three sheets, identical readings in the
        /// `inline` and `_ext` variants):
        ///
        ///   sheet            at the open        after the swap, no refit   a warm open
        ///   DARVO #pk=1356   207/204, 3 px      170/170, text still scrolls  170/170, no scroll
        ///   #pk=2            181/139, 42 px     170/139, 31 px              170/139, 31 px
        ///   #pk=1084         161/128, 33 px     146/128, 18 px, no frame    146/146, 0 px, 25.19 px frame
        ///
        /// So the cold fit's residue that survives the swap is the text frame still carrying
        /// `.mm-scroll` on the sheet that ends up whole (DARVO) and the relations frame that never
        /// yields its height (#pk=1084, 18 px hidden where a warm open is whole); #pk=2 resolves by
        /// itself. The type sizes (9.52 / 9.44 / 10.8 px) coincide in the three states: these
        /// sheets sit at the 10 px floor either way, and this case does not claim more than that.
        /// The template now refits the open sheet when the document's fonts finish arriving; here
        /// the route holds the sheet's faces, the sheet is opened while they are in flight, the
        /// route lets them through, and the sheet must then read exactly as a warm open does.
        ///
        /// The walk is checked, not assumed: the face must be absent at the open (and the route
        /// must have seen the request), and each sheet must have read differently at the open
        /// than at a warm open — otherwise the case would be comparing two warm reads and would
        /// pass on a document that never had the defect.
        ///
        /// Witnesses: DARVO, whose text ends up whole, and the sheet the warm state rescues by
        /// yielding its relations frame (#pk=1084 at this viewport, picked on the page itself
        /// once its fonts are in), which keeps 18 px hidden without the refit.
        ///
        /// `fr` only: the DINPro swap is a property of the Latin text; `ar` sets its own text in
        /// another font, and its cold state is not what this case walks.
        ///
        /// What the route may hold: the two faces a SHEET lays out and the page does not — Black
        /// (900, the description) and Bold (700, the depth badge). Measured after `load` on the
        /// `fr` inline, `fr` `_ext` and `ar` inline wrappers: only `DINPro-Medium` has been
        /// requested (500 loaded, 300/700/900 unloaded). The page's own text is set in that
        /// face; a request held there keeps the document from ever reaching `load` — Cap27's
        /// first run timed out in `GotoAsync` for exactly that reason — and it is not the state
        /// this case walks: the reader it walks has a loaded page and clicks before the sheet's
        /// own faces have answered.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline")]
        [InlineData("fr", "Fallacies_fr.content.svg", "ext")]
        public async Task Cap27_SheetOpenedBeforeTheWebFontsArrive_IsRefittedWhenTheyDo(
            string lang, string svgFileName, string variant)
        {
            string[] sheetFaces = { "**/DINPro-Bold*", "**/DINPro-Black*" };
            const string faceProbeJs = "() => document.fonts.check('900 12px DINPro')";
            var inlinePath = variant == "ext" ? string.Empty : await ComposeIncludedAsync(lang, svgFileName);

            // Filled on the first page: DARVO from the corpus, then — once that page's fonts are
            // in — the sheet the warm state owes its whole text to its relations frame.
            var pks = new string[2];
            var witnessedCold = 0;

            for (var round = 0; round < 2; round++)
            {
                var role = round == 0 ? "darvo" : "yielded";
                var gate = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                var held = new int[1];
                Func<IPage, Task> holdFonts = async p =>
                {
                    foreach (var glob in sheetFaces)
                    {
                        await p.RouteAsync(glob, async route =>
                        {
                            Interlocked.Increment(ref held[0]);
                            await gate.Task;
                            await route.ContinueAsync();
                        });
                    }
                };

                // Every page gets its own browser context — hence its own (empty) font cache — so
                // each sheet is a cold document and needs its own gate.
                var page = variant == "ext"
                    ? await OpenExternalAsync(lang, svgFileName, 512, 360, holdFonts)
                    : await OpenPageAtAsync(inlinePath, 512, 360, holdFonts);
                try
                {
                    if (variant == "ext") await WaitForExternalNavigationAsync(page);
                    else await WaitForViewportSettledAsync(page);

                    if (round == 0)
                    {
                        var pick = await page.EvaluateAsync<string[]>(P2400PickThreeSheetsJs, P2400DarvoLabel[lang]);
                        Assert.NotNull(pick);
                        Assert.True(pick.Length >= 10 && !string.IsNullOrEmpty(pick[0]) && int.Parse(pick[6]) >= 3,
                            $"#2403 [{lang}/{variant}] cold: the fixture must offer the DARVO sheet and a " +
                            $"relation-bearing sheet — got {string.Join(" | ", pick)}");
                        Assert.Contains(P2400DarvoLabel[lang], pick[1]);
                        pks[0] = pick[0];
                    }
                    var pk = pks[round];

                    // Open the sheet with the font responses still held: this is the reader who
                    // clicks before the CDN answers, and the fit that runs on fallback metrics.
                    await page.EvaluateAsync<string[]>(P2400SheetContractJs, pk);
                    var atOpen = await page.EvaluateAsync<string[]>(P2403OpenSheetFramesJs);
                    var fontAtOpen = await page.EvaluateAsync<bool>(faceProbeJs);
                    _output.WriteLine($"[{lang}/{variant} cold] {role} pk={pk}: at the open {atOpen[3]} " +
                                      $"hidden={atOpen[0]}px text={atOpen[1]} relFrame='{atOpen[2]}' " +
                                      $"desc={atOpen[4]} exemple={atOpen[5]} link={atOpen[6]} " +
                                      $"font={(fontAtOpen ? "loaded" : "absent")} heldRequests={held[0]}");
                    Assert.False(fontAtOpen,
                        $"#2403 [{lang}/{variant}] {role} pk={pk}: the web font was already available when the sheet " +
                        "opened — the walk did not reach the cold state and would compare two warm reads");
                    Assert.True(held[0] >= 1,
                        $"#2403 [{lang}/{variant}] {role} pk={pk}: the route never saw a font request, so no face was " +
                        "in flight when the sheet opened — the cold state was not created");

                    // Let the fonts through, wait for the face to land, then let the swap and the
                    // refit settle: they are separate tasks from the load that completes.
                    gate.TrySetResult(true);
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    var arrived = false;
                    while (sw.ElapsedMilliseconds < SettleMaxWaitMs)
                    {
                        if (arrived = await page.EvaluateAsync<bool>(faceProbeJs)) break;
                        await page.WaitForTimeoutAsync(100);
                    }
                    Assert.True(arrived, $"#2403 [{lang}/{variant}] {role} pk={pk}: the web font did not arrive within " +
                                         $"{SettleMaxWaitMs} ms of the release");
                    await page.WaitForTimeoutAsync(400);

                    // Read the sheet as the refit left it, then as a warm open lays it out.
                    var after = await page.EvaluateAsync<string[]>(P2403OpenSheetFramesJs);
                    await page.EvaluateAsync<string[]>(P2400SheetContractJs, pk);
                    var warm = await page.EvaluateAsync<string[]>(P2403OpenSheetFramesJs);
                    _output.WriteLine($"[{lang}/{variant} cold] {role} pk={pk}: after the fonts arrived " +
                                      $"{after[3]} hidden={after[0]}px text={after[1]} relFrame='{after[2]}' " +
                                      $"desc={after[4]} exemple={after[5]} link={after[6]} | " +
                                      $"a warm open {warm[3]} hidden={warm[0]}px relFrame='{warm[2]}' " +
                                      $"desc={warm[4]} exemple={warm[5]} link={warm[6]}");
                    if (!atOpen.SequenceEqual(warm)) witnessedCold++;
                    Assert.Equal(warm, after);

                    if (round == 0)
                    {
                        // The second witness, read on this page now that its fonts are in: the
                        // sheet the yield rescues at this viewport.
                        var y = await page.EvaluateAsync<string[]>(P2400PickYieldedJs);
                        Assert.NotNull(y);
                        Assert.True(!string.IsNullOrEmpty(y[0]) && int.Parse(y[2]) >= 1 && int.Parse(y[3]) >= 1,
                            $"#2403 [{lang}/{variant}] cold: no sheet owes its whole text to its relations frame at " +
                            $"512x360 — the second witness has nothing to be measured on: {string.Join(" | ", y)}");
                        pks[1] = y[0];
                        _output.WriteLine($"[{lang}/{variant} cold] second witness: pk={pks[1]} '{y[1]}' " +
                                          $"({y[2]} relations, frame {y[4]}, {y[3]} sheets rescued at this viewport)");
                    }
                }
                finally
                {
                    gate.TrySetResult(true);
                    await page.CloseAsync();
                }
            }

            Assert.Equal(2, witnessedCold);
        }

        // -------------------------------------------------------------------------------------
        // #2406 — la fiche effacait son propre nœud de sous-sous-famille.
        //
        // Defect: `sous_famille.textContent = …` replaces the container's children, DETACHING the
        // `.Soussousfamille` span; the next line then wrote the sub-sub-family into that detached
        // node. No sheet ever displayed it (fr: 1379 of 1408 nodes carry the attribute), and the
        // level-3 climb installed by `wireFamilyNav` kept its listener on a node that had left the
        // document — the first open killed it for the rest of the page.
        //
        // The control opens EVERY shallow sheet (depth <= 3: the ones that must stay silent) and a
        // DECLARED sample of the deep ones (depth > 3: the ones that must show the line), then
        // asserts what the fix owes: the element stays in the document, it carries the attribute
        // verbatim, it is shown exactly when the ancestor it names is not the sheet itself, and it
        // starts a line of its own. Cap20/Cap21 keep sweeping every sheet for the fit the line
        // costs, so this case does not re-judge the whole sheet.
        //
        // Three cases: the two variants of the Fallacies map (fr inline, ar `_ext`) and the Virtues
        // map, which the issue listed as unmeasured and which carries the same attribute with the
        // same structure (194 carriers: 131 deep, 63 at level 3). The Virtues case opens every one
        // of its deep sheets (no sample needed at that size); the Fallacies ones declare one.
        //
        // Red before the fix, measured rather than assumed: the same case run against the two
        // templates as `git show <base>:` hands them reports `elementDetached` on every sheet it
        // opens (PR body carries the run).
        // -------------------------------------------------------------------------------------
        private const string P2406SubSubFamilySweepJs = @"(arg) => {
            const opt = arg || {};
            const deepSample = opt.deepSample > 0 ? Math.floor(opt.deepSample) : 0;
            const nav = window.__mmNav;
            const card = document.querySelector('card');
            const sf = card.querySelector('.sous_famille');
            const ssf = card.querySelector('.Soussousfamille');
            const R = (el) => el.getBoundingClientRect();
            const out = { nodes: 0, deep: 0, shallow: 0, shown: 0, hidden: 0, longest: 0,
                          violations: [], tally: {} };
            const NOTE = (name, key) => {
                out.tally[name] = (out.tally[name] || 0) + 1;
                if (out.violations.length < 12) out.violations.push(key + ':' + name);
            };
            const num = (k) => (isNaN(Number(k)) ? 1e9 : Number(k));
            const byKey = (a, b) => num(a) - num(b) || (a < b ? -1 : a > b ? 1 : 0);
            const depthOf = (k) => parseInt(nav.byPk[k].el.getAttribute('depth'), 10);
            const deep = Object.keys(nav.byPk).filter((k) => depthOf(k) > 3).sort(byKey);
            const shallow = Object.keys(nav.byPk).filter((k) => !(depthOf(k) > 3)).sort(byKey);
            // Declared sample of the deep population: evenly spaced, plus its two extremes (the
            // longest and the shortest sub-sub-family label) so the widest line is exercised.
            let deepTargets = deep;
            if (deepSample > 0 && deep.length > deepSample) {
                const chosen = new Set();
                for (let i = 0; i < deepSample; i++)
                    chosen.add(deep[Math.floor(i * deep.length / deepSample)]);
                const len = (k) => (nav.byPk[k].el.getAttribute('subsubfamily') || '').trim().length;
                const byLen = deep.slice().sort((a, b) => len(b) - len(a) || byKey(a, b));
                chosen.add(byLen[0]);
                chosen.add(byLen[byLen.length - 1]);
                deepTargets = Array.from(chosen).sort(byKey);
            }
            for (const k of shallow.concat(deepTargets)) {
                const rec = nav.byPk[k];
                nav.focusNode(rec);
                out.nodes++;
                const depth = depthOf(k);
                const want = (rec.el.getAttribute('subsubfamily') || '').trim();
                const expectShown = depth > 3 && !!want;
                if (expectShown) out.deep++; else out.shallow++;
                if (!ssf) { NOTE('elementMissing', k); continue; }
                if (!ssf.isConnected) { NOTE('elementDetached', k); continue; }
                const got = (ssf.textContent || '').trim();
                if (got !== want) NOTE('textMismatch', k + '=' + got);
                const cs = getComputedStyle(ssf);
                const visible = cs.display !== 'none' && ssf.clientHeight > 0;
                if (visible !== expectShown) {
                    NOTE(expectShown ? 'notShown' : 'shownWhereItShouldNotBe', k);
                    continue;
                }
                if (!visible) { out.hidden++; continue; }
                out.shown++;
                if (got.length > out.longest) out.longest = got.length;
                if (cs.display !== 'block') NOTE('notABlock', k);
                if (Math.abs(ssf.clientWidth - sf.clientWidth) > 1) NOTE('notFullWidth', k);
                const wantSf = (rec.el.getAttribute('subfamily') || '').trim();
                const sfText = sf.firstChild && sf.firstChild.nodeType === 3
                    ? sf.firstChild.textContent.trim() : '';
                if (sfText !== wantSf) NOTE('subFamilyLost', k);
                // Its OWN line: on the same line the span's top sits with the text (a few pixels
                // under the container's top); stacked, it clears at least half a line box.
                const sfCs = getComputedStyle(sf);
                const line = parseFloat(sfCs.lineHeight) || parseFloat(sfCs.fontSize) || 12;
                if (R(ssf).top < R(sf).top + line * 0.5) NOTE('notOnItsOwnLine', k);
                const hr = R(card.querySelector('.header')), cr = R(card);
                if (hr.top < cr.top - 0.6 || hr.bottom > cr.bottom + 0.6) NOTE('headerOutsideCard', k);
            }
            return [
                String(out.nodes), String(out.deep), String(out.shallow),
                String(out.shown), String(out.hidden),
                out.violations.join('|'),
                Object.keys(out.tally).sort().map((t) => t + '=' + out.tally[t]).join(','),
                String(out.longest),
                String(deep.length), String(shallow.length)
            ];
        }";

        /// <summary>
        /// The level-3 climb the fix restores: picks a sheet deeper than level 3 straight from the
        /// navigation index, opens it, and names the ancestor its sub-sub-family line holds.
        /// </summary>
        private const string P2406PickDeepSheetJs = @"() => {
            const nav = window.__mmNav;
            const keys = Object.keys(nav.byPk);
            const num = (k) => (isNaN(Number(k)) ? 1e9 : Number(k));
            const byKey = (a, b) => num(a) - num(b) || (a < b ? -1 : a > b ? 1 : 0);
            for (const k of keys.slice().sort(byKey)) {
                const rec = nav.byPk[k];
                const path = rec.path || '';
                const parts = path.split('.');
                if (parts.length <= 3) continue;
                const want = (rec.el.getAttribute('subsubfamily') || '').trim();
                if (!want) continue;
                const ancPath = parts.slice(0, 3).join('.');
                const anc = nav.byPath[ancPath];
                if (!anc) continue;
                nav.focusNode(rec);
                return [k, path, want, ancPath, anc.label || '', rec.label || '', String(parts.length)];
            }
            return [];
        }";

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline", 160)]
        [InlineData("ar", "Fallacies_ar.content.svg", "ext", 120)]
        [InlineData("fr", "Argumentum_Virtues_MindMap_fr.content.svg", "inline", 0)]
        public async Task Cap28_SubSubFamily_IsShownBelowLevel3_OnEverySheet_AndClimbsToIt(
            string lang, string svgFileName, string variant, int deepSample)
        {
            var inlinePath = variant == "ext" ? string.Empty : await ComposeIncludedAsync(lang, svgFileName);
            var page = variant == "ext"
                ? await OpenExternalAsync(lang, svgFileName, 1400, 900)
                : await OpenPageAtAsync(inlinePath, 1400, 900);
            try
            {
                if (variant == "ext") await WaitForExternalNavigationAsync(page);
                else await WaitForViewportSettledAsync(page);

                var sweep = await page.EvaluateAsync<string[]>(P2406SubSubFamilySweepJs, new { deepSample });
                Assert.NotNull(sweep);
                _output.WriteLine($"[{lang}/{variant}] #2406 sub-sub-family: {sweep[0]} sheets opened " +
                                  $"({sweep[1]} deep expected shown, {sweep[2]} shallow expected silent) → " +
                                  $"shown={sweep[3]} hidden={sweep[4]} longestLabel={sweep[7]} " +
                                  $"populations deep={sweep[8]} shallow={sweep[9]} tally={sweep[6]}");

                Assert.True(sweep[5].Length == 0,
                    $"#2406 [{lang}/{variant}] the sheet must keep its sub-sub-family line: {sweep[5]}");
                Assert.True(int.Parse(sweep[2]) >= 3,
                    $"#2406 [{lang}/{variant}] the fixture offers no level-3 sheet, so nothing here proves " +
                    "the silence rule — the case cannot pass on an absent population");
                Assert.True(int.Parse(sweep[1]) >= 10,
                    $"#2406 [{lang}/{variant}] the fixture offers no sheet below level 3 with a sub-sub-family");
                Assert.Equal(sweep[1], sweep[3]);
                Assert.Equal(sweep[2], sweep[4]);
                Assert.True(int.Parse(sweep[7]) >= 8,
                    $"#2406 [{lang}/{variant}] no long sub-sub-family label was opened — the sample missed " +
                    "the widest line it exists to exercise");

                // The climb. Before the fix the listener sat on a node the first open had removed
                // from the document, so clicking the line did nothing after the sheet was open.
                // Labels are compared with their whitespace folded: an SVG label is cut into
                // `tspan`s at every line break, and the title receives that text as is.
                static string FoldWs(string? s) =>
                    string.IsNullOrEmpty(s) ? string.Empty : System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ").Trim();
                var pick = await page.EvaluateAsync<string[]>(P2406PickDeepSheetJs);
                Assert.NotNull(pick);
                Assert.True(pick.Length >= 7,
                    $"#2406 [{lang}/{variant}] no sheet deeper than level 3 in the fixture: [{string.Join("|", pick)}]");
                _output.WriteLine($"[{lang}/{variant}] #2406 climb: sheet pk={pick[0]} '{pick[5]}' " +
                                  $"(depth {pick[6]}) names '{pick[2]}' → ancestor path {pick[3]} '{pick[4]}'");
                var lineText = await page.Locator("card .Soussousfamille").EvaluateAsync<string>("el => el.textContent");
                Assert.Equal(FoldWs(pick[2]), FoldWs(lineText));
                var ssfBox = await page.Locator("card .Soussousfamille").BoundingBoxAsync();
                Assert.True(ssfBox != null && ssfBox.Height > 0,
                    $"#2406 [{lang}/{variant}] the sub-sub-family line is not a clickable box on a deep sheet");
                await page.Locator("card .Soussousfamille").ClickAsync();
                await page.WaitForTimeoutAsync(400);
                var afterTitle = await page.Locator("card .title").EvaluateAsync<string>("el => el.textContent");
                var afterDepth = await page.Locator("card .niveau").EvaluateAsync<string>("el => el.textContent");
                _output.WriteLine($"[{lang}/{variant}] #2406 after the click: '{afterTitle}' depth={afterDepth}");
                Assert.Equal(FoldWs(pick[4]), FoldWs(afterTitle));
                Assert.Equal("3", (afterDepth ?? string.Empty).Trim());
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        // ---- #2407 — fenetre courte : la fiche ouverte doit rester au-dessus ---------------

        /// <summary>
        /// Opens the first sheet of the navigation index through the page's own API, the same
        /// entry point a reader uses (Cap28 already relies on `focusNode`).
        /// </summary>
        private const string P2407OpenFirstSheetJs = @"() => {
            const nav = window.__mmNav;
            if (!nav) return ['no-nav'];
            const keys = Object.keys(nav.byPk || {});
            if (!keys.length) return ['no-sheet'];
            const num = (k) => (isNaN(Number(k)) ? 1e9 : Number(k));
            keys.sort((a, b) => num(a) - num(b) || (a < b ? -1 : a > b ? 1 : 0));
            nav.focusNode(nav.byPk[keys[0]]);
            return ['ok', keys[0]];
        }";

        /// <summary>
        /// The short-window measure itself. It asks the two questions the issue names — is the
        /// point AT the sheet's top and bottom edge inside the sheet, and does the document scroll
        /// sideways — plus the state of the navigation layer, so a green cannot come from a layer
        /// that was never on top.
        /// </summary>
        private const string P2407ShortWindowMeasureJs = @"() => {
            const card = document.querySelector('card');
            if (!card) return ['no-card'];
            if (card.classList.contains('hidden')) return ['card-hidden'];
            const de = document.documentElement;
            const r = card.getBoundingClientRect();
            const sw = Math.max(de.scrollWidth, document.body.scrollWidth);
            const probe = (x, y) => {
                const el = document.elementFromPoint(x, y);
                if (!el) return 'nothing';
                return card.contains(el) ? 'card' : (el.id || (el.className && el.className.toString()) || el.tagName);
            };
            const topAt = probe(r.left + r.width / 2, Math.max(0, r.top + 1));
            const bottomAt = probe(r.left + r.width / 2, Math.min(de.clientHeight - 1, r.bottom - 1));
            const layers = ['mmToolbar', 'mmLegend'].map((id) => {
                const el = document.getElementById(id);
                if (!el) return id + ':absent:-';
                const b = el.getBoundingClientRect();
                const covered = b.width > 0 && r.width > 0
                    && b.left < r.right && r.left < b.right && b.top < r.bottom && r.top < b.bottom;
                return id + ':' + (covered ? 'covered' : 'clear') + ':' + getComputedStyle(el).visibility;
            });
            return [
                String(sw), String(de.clientWidth), String(de.clientHeight),
                String(Math.round(r.top)), String(Math.round(r.bottom)),
                String(Math.round(r.left)), String(Math.round(r.right)),
                topAt, bottomAt, layers.join(','),
                document.body.classList.contains('mm-layer-hidden') ? 'class-on' : 'class-off'
            ];
        }";

        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline", 195, 422)]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline", 156, 338)]
        [InlineData("fr", "Fallacies_fr.content.svg", "ext", 195, 422)]
        [InlineData("fr", "Fallacies_fr.content.svg", "ext", 156, 338)]
        public async Task Cap29_ShortWindow_TheSheetStaysOnTop_AndThePageDoesNotScrollSideways(
            string lang, string svgFileName, string variant, int width, int height)
        {
            var inlinePath = variant == "ext" ? string.Empty : await ComposeIncludedAsync(lang, svgFileName);
            var page = variant == "ext"
                ? await OpenExternalAsync(lang, svgFileName, width, height)
                : await OpenPageAtAsync(inlinePath, width, height);
            try
            {
                if (variant == "ext") await WaitForExternalNavigationAsync(page);
                else await WaitForViewportSettledAsync(page);

                var opened = await page.EvaluateAsync<string[]>(P2407OpenFirstSheetJs);
                Assert.Equal("ok", opened[0]);
                await page.WaitForFunctionAsync(
                    "() => { const c = document.querySelector('card'); return c && !c.classList.contains('hidden'); }");
                await page.WaitForTimeoutAsync(300);

                var m = await page.EvaluateAsync<string[]>(P2407ShortWindowMeasureJs);
                Assert.NotNull(m);
                Assert.True(m.Length >= 11, $"#2407 [{lang}/{variant} {width}x{height}] no measure: [{string.Join("|", m)}]");
                _output.WriteLine($"[{lang}/{variant} {width}x{height}] #2407 sheet pk={opened[1]} " +
                                  $"box y={m[3]}..{m[4]} x={m[5]}..{m[6]} | scrollWidth={m[0]} clientWidth={m[1]} " +
                                  $"| top→{m[7]} bottom→{m[8]} | {m[9]} | {m[10]}");

                Assert.True(int.Parse(m[0]) <= int.Parse(m[1]),
                    $"#2407 [{lang}/{variant} {width}x{height}] the page scrolls sideways: " +
                    $"scrollWidth {m[0]} > clientWidth {m[1]}");
                Assert.Equal("card", m[7]);
                Assert.Equal("card", m[8]);

                // The case only proves something if a layer really was over the sheet: an
                // assertion that passes because nothing was ever on top measures the fixture, not
                // the rule.
                var covered = m[9].Split(',').Where(s => s.Contains(":covered:")).ToArray();
                Assert.True(covered.Length >= 1,
                    $"#2407 [{lang}/{variant} {width}x{height}] no navigation layer covers the sheet — " +
                    $"the case cannot pass on an absent population ({m[9]})");
                foreach (var c in covered)
                {
                    Assert.EndsWith(":hidden", c);
                }
                Assert.Equal("class-on", m[10]);

                // The decision is measured, not latched: a viewport tall enough to clear the layer
                // must give it back, and coming back to the short window must hide it again.
                await page.SetViewportSizeAsync(1280, 900);
                await page.WaitForTimeoutAsync(300);
                var tall = await page.EvaluateAsync<string[]>(P2407ShortWindowMeasureJs);
                _output.WriteLine($"[{lang}/{variant}] #2407 at 1280x900: {tall[9]} | {tall[10]}");
                Assert.DoesNotContain(":covered:", tall[9]);
                Assert.Equal("class-off", tall[10]);
                Assert.DoesNotContain(":hidden", tall[9]);
                Assert.Equal("card", tall[7]);
                Assert.Equal("card", tall[8]);

                await page.SetViewportSizeAsync(width, height);
                await page.WaitForTimeoutAsync(300);
                var back = await page.EvaluateAsync<string[]>(P2407ShortWindowMeasureJs);
                _output.WriteLine($"[{lang}/{variant} {width}x{height}] #2407 back: {back[9]} | {back[10]}");
                Assert.Equal(m[9], back[9]);
                Assert.Equal("class-on", back[10]);
                Assert.Equal("card", back[7]);
                Assert.Equal("card", back[8]);
                Assert.True(int.Parse(back[0]) <= int.Parse(back[1]),
                    $"#2407 [{lang}/{variant} {width}x{height}] sideways scroll came back: {back[0]} > {back[1]}");
            }
            finally
            {
                await page.CloseAsync();
            }
        }

        /// <summary>
        /// The inverse control: the same measure on a page where the fix is reverted by CSS — the
        /// template's own pre-#2407 values (default `body` margin, `96vw`/`100vw` sizing with no
        /// clamp, layer visible even when the class is on). It must be RED, and it must say which
        /// clause went red: a control that fails for an unrelated reason proves nothing.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg", "inline", 195, 422)]
        [InlineData("fr", "Fallacies_fr.content.svg", "ext", 195, 422)]
        public async Task Cap29Inverse_WithoutTheFix_TheSameMeasureIsRed(
            string lang, string svgFileName, string variant, int width, int height)
        {
            var inlinePath = variant == "ext" ? string.Empty : await ComposeIncludedAsync(lang, svgFileName);
            var page = variant == "ext"
                ? await OpenExternalAsync(lang, svgFileName, width, height)
                : await OpenPageAtAsync(inlinePath, width, height);
            try
            {
                if (variant == "ext") await WaitForExternalNavigationAsync(page);
                else await WaitForViewportSettledAsync(page);

                var mutant = variant == "ext"
                    ? "html, body { margin: 8px !important; height: auto !important; } " +
                      "#mindmap { width: 100vw !important; height: 100vh !important; } " +
                      "body.mm-layer-hidden #mmToolbar, body.mm-layer-hidden #mmLegend " +
                      "{ visibility: visible !important; pointer-events: auto !important; }"
                    : "body { margin: 8px !important; } " +
                      "#mindmap > svg { max-width: none !important; } " +
                      "body.mm-layer-hidden #mmToolbar, body.mm-layer-hidden #mmLegend " +
                      "{ visibility: visible !important; pointer-events: auto !important; }";
                await page.EvaluateAsync(SetMutantStyleJs, mutant);

                // Re-open on the mutated layout: the class is recomputed from a fresh measure.
                var opened = await page.EvaluateAsync<string[]>(P2407OpenFirstSheetJs);
                Assert.Equal("ok", opened[0]);
                await page.WaitForFunctionAsync(
                    "() => { const c = document.querySelector('card'); return c && !c.classList.contains('hidden'); }");
                await page.WaitForTimeoutAsync(300);

                var m = await page.EvaluateAsync<string[]>(P2407ShortWindowMeasureJs);
                _output.WriteLine($"[{lang}/{variant} INVERSE] #2407 scrollWidth={m[0]} clientWidth={m[1]} " +
                                  $"top→{m[7]} bottom→{m[8]} | {m[9]} | {m[10]}");

                var sideways = int.Parse(m[0]) > int.Parse(m[1]);
                var topLost = m[7] != "card";
                var bottomLost = m[8] != "card";
                var layerOnTop = m[9].Contains(":covered:visible");
                Assert.True(sideways || topLost || bottomLost || layerOnTop,
                    "#2407 INVERSE control is GREEN: reverting the fix changed nothing, so the " +
                    $"control never had a hold on the defect (scrollWidth={m[0]} clientWidth={m[1]} " +
                    $"top→{m[7]} bottom→{m[8]} {m[9]})");
                // Name the clause that went red, so a later reader knows which mechanism the
                // control actually exercised at this window.
                _output.WriteLine($"[{lang}/{variant} INVERSE] red clauses: " +
                                  $"sideways={sideways} topLost={topLost} bottomLost={bottomLost} layerOnTop={layerOnTop}");
            }
            finally
            {
                await page.CloseAsync();
            }
        }
    }
}
