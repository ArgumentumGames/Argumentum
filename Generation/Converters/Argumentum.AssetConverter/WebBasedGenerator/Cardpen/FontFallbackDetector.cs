using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Argumentum.AssetConverter;

/// <summary>
/// Reports which fonts actually paint the letters of the cards rendered in the #cpOutput
/// iframe (#1485). Two things the harvest log could not say until now:
/// <list type="bullet">
/// <item>a declared <c>@font-face</c> that the page needed and that did not load
/// (<c>[FONT-LOAD-FAILURE]</c>) — its <c>FontFace.status</c> is <c>error</c>;</item>
/// <item>a stack whose declared families do not render the letters of a script
/// (<c>[FONT-FALLBACK]</c>) — the browser then paints them with a system font, silently.</item>
/// </list>
/// Non-intrusive: reads computed styles, <c>document.fonts</c> and canvas advance widths. The
/// images have already been captured when it runs; the report says what they contain.
/// </summary>
/// <remarks>
/// <para>
/// The fallback is measured LETTER BY LETTER against a sentinel font embedded below: it covers
/// every code point with a 3 em advance, so for a family F and a character c,
/// <c>width("F, sentinel", c) == width("sentinel", c)</c> holds exactly when F does not render c.
/// </para>
/// <para>
/// Why not simpler instruments. <c>document.fonts.check()</c> only tests the faces of its own
/// list: it is true for a family that loaded and cannot see that a glyph is missing. Comparing
/// the width of <c>"F", monospace</c> to <c>monospace</c> does not work either — a character F
/// lacks goes to the system fallback, whose choice depends on the generic family of the primary
/// font, so two different stacks can render the same width for the wrong reason.
/// </para>
/// <para>
/// Letters only (<c>\p{L}</c>). Digits, punctuation and emoji are normally painted by a symbol
/// font and are not probed. Text that is not laid out (display:none, e.g. .cardName) is counted
/// but never reported as a finding.
/// </para>
/// <para>
/// The script is declared <c>async (element, args) =&gt; { … }</c>: <c>ILocator.EvaluateAsync</c>
/// calls the expression with the ELEMENT as first argument and <c>arg</c> as second. The #190
/// overflow detector stayed dead for months behind a <c>(args) =&gt;</c> signature and a muted
/// catch — hence the binding test (<c>FontFallbackDetectorBindingTests</c>, test project),
/// which drives the real Playwright path and asserts this detector's OUTPUT.
/// </para>
/// </remarks>
public static class FontFallbackDetector
{
    public const string VerdictClean = "clean";
    public const string VerdictGeneric = "generic";
    public const string VerdictFallback = "fallback";
    public const string VerdictPartial = "partial";
    public const string VerdictSplit = "split";

    /// <summary>Family of the sentinel face added to the document for the measurement, then deleted.</summary>
    internal const string SentinelFamily = "ArgumentumProbeSentinel";

    /// <summary>TrueType bytes of the sentinel, Base64 — built by tools/fonts/make-probe-fonts.py, verified by its --check.</summary>
    internal const string SentinelFontBase64 =
        "AAEAAAAKAIAAAwAgT1MvMl8IWYMAAACsAAAAYGNtYXAAIQA0AAABDAAAAChnbHlmA8AClAAAATQAAAAiaGVhZF8WQOUAAAFYAAAANmhoZWEDIgrz" +
        "AAABkAAAACRobXR4F3AAAAAAAbQAAAAIbG9jYQARAAAAAAG8AAAABm1heHAABAAGAAABxAAAACBuYW1lGRQvjgAAAeQAAAE0cG9zdP+fADIAAAMY" +
        "AAAAIAAEAfQBkAAFAAACigJY/7UBXgKKAlj/tQFeADIA+gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABOT05FAEAAAP//AyD/OAAAAyAAyAAA" +
        "AAEAAAAAAfQCvAAAACAAAAAAAAEAAwAKAAAADAANAAAAAAAcAAAAAAAAAAEAAAAAABD//wAAAAEAAQAAAAABkAK8AAMAAAEBAQEBkAAA/nAAAAK8" +
        "AAD9RAAAAAAAAQAAAAEAALrk6s9fDzz1AAMD6AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAgAAAAAAAAABAAADIP84AAALuAAAAAAAAAAB" +
        "AAAAAAAAAAAAAAAAAAAAAgu4AAALuAAAAAAAAAARAAAAAQAAAAIABAABAAAAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAYATgADAAEECQABAC4AAAAD" +
        "AAEECQACAA4ALgADAAEECQADADgAPAADAAEECQAEAC4AdAADAAEECQAFABYAogADAAEECQAGAC4AuABBAHIAZwB1AG0AZQBuAHQAdQBtAFAAcgBv" +
        "AGIAZQBTAGUAbgB0AGkAbgBlAGwAUgBlAGcAdQBsAGEAcgBBAHIAZwB1AG0AZQBuAHQAdQBtAFAAcgBvAGIAZQBTAGUAbgB0AGkAbgBlAGwAOwAg" +
        "ADEALgAwAEEAcgBnAHUAbQBlAG4AdAB1AG0AUAByAG8AYgBlAFMAZQBuAHQAaQBuAGUAbABWAGUAcgBzAGkAbwBuACAAMQAuADAAQQByAGcAdQBt" +
        "AGUAbgB0AHUAbQBQAHIAbwBiAGUAUwBlAG4AdABpAG4AZQBsAAMAAAAAAAD/nAAyAAAAAAAAAAAAAAAAAAAAAAAAAAA=";

    // Measured in a real Chromium on 2026-11-25 (po-2023, #1485): stack "Barlow, sans-serif" →
    // Cyrillic verdict "fallback" (0 of 12 letters rendered by Barlow), Latin verdict "clean";
    // "Rubik, sans-serif" → Cyrillic "clean" (12 of 12). The double-quote character never
    // appears literally below (String.fromCharCode) so the script fits a verbatim string.
    private const string ProbeScript = @"async (element, args) => {
                const doc = element.ownerDocument;
                const win = doc.defaultView;
                const out = { ok: false, error: null, faces: [], groups: [], stats: {} };
                let sentinel = null;
                try {
                    if (!doc.fonts || !win.FontFace) { throw new Error('FontFaceSet API unavailable in this document'); }
                    const GENERIC = new Set(['serif', 'sans-serif', 'monospace', 'cursive', 'fantasy', 'system-ui',
                        'ui-serif', 'ui-sans-serif', 'ui-monospace', 'ui-rounded', 'math', 'emoji', 'fangsong']);
                    const SCRIPTS = [
                        ['Cyrillic', /\p{sc=Cyrillic}/u], ['Latin', /\p{sc=Latin}/u], ['Greek', /\p{sc=Greek}/u],
                        ['Arabic', /\p{sc=Arabic}/u], ['Hebrew', /\p{sc=Hebrew}/u], ['Armenian', /\p{sc=Armenian}/u],
                        ['Georgian', /\p{sc=Georgian}/u], ['Han', /\p{sc=Han}/u], ['Hiragana', /\p{sc=Hiragana}/u],
                        ['Katakana', /\p{sc=Katakana}/u], ['Hangul', /\p{sc=Hangul}/u], ['Thai', /\p{sc=Thai}/u],
                        ['Devanagari', /\p{sc=Devanagari}/u]
                    ];
                    const LETTER = /\p{L}/u;
                    const scriptOf = (ch) => {
                        for (let i = 0; i < SCRIPTS.length; i++) { if (SCRIPTS[i][1].test(ch)) { return SCRIPTS[i][0]; } }
                        return 'Other';
                    };
                    const SKIP = 'script, style, noscript, template';

                    // ---- 1. walk every text node; group by (stack, weight, style, script)
                    const groups = new Map();
                    const scriptCache = new Map();
                    const stats = { textNodes: 0, renderedTextNodes: 0, hiddenTextNodes: 0 };
                    const walker = doc.createTreeWalker(element, win.NodeFilter.SHOW_TEXT);
                    for (let node = walker.nextNode(); node; node = walker.nextNode()) {
                        const raw = node.nodeValue;
                        if (!raw || raw.trim().length === 0) { continue; }
                        const el = node.parentElement;
                        if (!el || (el.closest && el.closest(SKIP))) { continue; }
                        const letters = [];
                        for (const ch of raw) {
                            if (!LETTER.test(ch)) { continue; }
                            let sc = scriptCache.get(ch);
                            if (sc === undefined) { sc = scriptOf(ch); scriptCache.set(ch, sc); }
                            letters.push([ch, sc]);
                        }
                        if (letters.length === 0) { continue; }
                        const cs = win.getComputedStyle(el);
                        const rendered = el.getClientRects().length > 0;
                        const cls = el.classList ? Array.from(el.classList) : [];
                        const selector = el.tagName.toLowerCase() + (cls.length ? '.' + cls.join('.') : '');
                        const cardEl = el.closest ? el.closest('card') : null;
                        const nameEl = cardEl ? cardEl.querySelector('.cardName') : null;
                        const cardName = nameEl ? (nameEl.textContent || '').trim() : '';
                        const snippet = raw.replace(/\s+/g, ' ').trim().slice(0, 60);
                        stats.textNodes++;
                        if (rendered) { stats.renderedTextNodes++; } else { stats.hiddenTextNodes++; }
                        const perScript = new Map();
                        for (let i = 0; i < letters.length; i++) {
                            let list = perScript.get(letters[i][1]);
                            if (!list) { list = []; perScript.set(letters[i][1], list); }
                            list.push(letters[i][0]);
                        }
                        for (const entry of perScript) {
                            const script = entry[0];
                            const chars = entry[1];
                            const key = [cs.fontFamily, cs.fontWeight, cs.fontStyle, script].join('\u0001');
                            let g = groups.get(key);
                            if (!g) {
                                g = { stack: cs.fontFamily, weight: cs.fontWeight, style: cs.fontStyle, script: script,
                                      chars: new Set(), nodes: [] };
                                groups.set(key, g);
                            }
                            for (let i = 0; i < chars.length; i++) { g.chars.add(chars[i]); }
                            g.nodes.push({ chars: chars, rendered: rendered, selector: selector, cardName: cardName, snippet: snippet });
                        }
                    }

                    // ---- 2. FontFace statuses. Faces whose loading was triggered by the rendered text
                    // must have settled ('loaded' or 'error') before they are read.
                    try { await doc.fonts.ready; } catch (e) {}
                    for (const f of doc.fonts) {
                        out.faces.push({ family: f.family, weight: String(f.weight), style: String(f.style), status: f.status });
                    }

                    // ---- 3. sentinel: covers everything with an absurd advance
                    const canvas = doc.createElement('canvas');
                    const ctx = canvas.getContext('2d');
                    const DQ = String.fromCharCode(34);
                    const SQ = String.fromCharCode(39);
                    const quote = (name) => DQ + name.replace(/\\/g, '\\\\').replace(new RegExp(DQ, 'g'), '\\' + DQ) + DQ;
                    const splitStack = (stack) => {
                        const names = [];
                        let cur = '';
                        let q = null;
                        for (let i = 0; i < stack.length; i++) {
                            const ch = stack[i];
                            if (q) {
                                if (ch === '\\' && q === DQ) { cur += stack[i + 1]; i++; }
                                else if (ch === q) { q = null; names.push(cur); cur = ''; }
                                else { cur += ch; }
                            } else if (ch === DQ || ch === SQ) { q = ch; }
                            else if (ch === ',') { if (cur.trim().length) { names.push(cur.trim()); } cur = ''; }
                            else { cur += ch; }
                        }
                        if (cur.trim().length) { names.push(cur.trim()); }
                        return names;
                    };
                    const PROBE = '10px sans-serif';
                    let current = '';
                    const measure = (font, text) => {
                        // An unparsable canvas font is ignored silently: the previous font would be measured.
                        if (font !== current) {
                            ctx.font = PROBE;
                            const before = ctx.font;
                            ctx.font = font;
                            if (ctx.font === before) { throw new Error('Unsupported canvas font: ' + font); }
                            current = font;
                        }
                        return ctx.measureText(text).width;
                    };

                    sentinel = new win.FontFace(args.sentinelFamily,
                        'url(data:font/ttf;base64,' + args.sentinelBase64 + ')');
                    await sentinel.load();
                    doc.fonts.add(sentinel);

                    const sentFont = quote(args.sentinelFamily);
                    const sentWidth = new Map();
                    for (const g of groups.values()) {
                        const prefix = g.style + ' ' + g.weight + ' 100px';
                        const sw = measure(prefix + ' ' + sentFont, 'A');
                        if (sw < 200) { throw new Error('Sentinel font not usable (canvas font: ' + prefix + ' ' + sentFont + ')'); }
                        const families = splitStack(g.stack);
                        const declared = families.filter((name) => !GENERIC.has(name.toLowerCase()));
                        const chars = Array.from(g.chars);
                        // The canvas does not wait for a font to load: ask for every declared family first.
                        for (const name of declared) {
                            try { await doc.fonts.load(prefix + ' ' + quote(name), chars.join('')); } catch (e) {}
                        }
                        const by = new Map();
                        const coverage = [];
                        for (const name of declared) {
                            const famFont = prefix + ' ' + quote(name) + ', ' + sentFont;
                            let covered = 0;
                            for (const ch of chars) {
                                if (by.has(ch)) { continue; }
                                const k = prefix + '\u0001' + ch;
                                let s = sentWidth.get(k);
                                if (s === undefined) { s = measure(prefix + ' ' + sentFont, ch); sentWidth.set(k, s); }
                                if (measure(famFont, ch) !== s) { by.set(ch, name); covered++; }
                            }
                            coverage.push({ family: name, covered: covered });
                        }
                        const missing = chars.filter((ch) => !by.has(ch));
                        const distinct = Array.from(new Set(by.values()));
                        let verdict;
                        if (declared.length === 0) { verdict = 'generic'; }
                        else if (missing.length === 0) { verdict = distinct.length === 1 ? 'clean' : 'split'; }
                        else { verdict = by.size === 0 ? 'fallback' : 'partial'; }
                        const missingSet = new Set(missing);
                        const isFinding = verdict !== 'clean' && verdict !== 'generic';
                        let affected = 0;
                        const examples = [];
                        let renderedNodes = 0;
                        for (const n of g.nodes) {
                            if (!n.rendered) { continue; }
                            renderedNodes++;
                            const hit = isFinding && (verdict === 'split' || n.chars.some((ch) => missingSet.has(ch)));
                            if (hit) {
                                affected++;
                                if (examples.length < 5) {
                                    examples.push({ selector: n.selector, cardName: n.cardName, snippet: n.snippet });
                                }
                            }
                        }
                        out.groups.push({
                            stack: g.stack, weight: g.weight, style: g.style, script: g.script,
                            families: declared, coverage: coverage, sample: chars.join(''), letterCount: chars.length,
                            missing: missing.join(''), missingCount: missing.length,
                            renderedBy: distinct.length === 0 ? null : distinct.join(' | '),
                            verdict: verdict, textNodes: g.nodes.length, renderedTextNodes: renderedNodes,
                            affectedTextNodes: affected, examples: examples
                        });
                    }
                    let errorFaces = 0;
                    let unloadedFaces = 0;
                    for (const f of out.faces) {
                        if (f.status === 'error') { errorFaces++; }
                        else if (f.status === 'unloaded') { unloadedFaces++; }
                    }
                    out.stats = {
                        textNodes: stats.textNodes,
                        renderedTextNodes: stats.renderedTextNodes,
                        hiddenTextNodes: stats.hiddenTextNodes,
                        declaredFaces: out.faces.length,
                        errorFaces: errorFaces,
                        unloadedFaces: unloadedFaces,
                        groups: out.groups.length
                    };
                    out.ok = true;
                } catch (e) {
                    out.error = String((e && e.message) || e);
                } finally {
                    if (sentinel) { try { doc.fonts.delete(sentinel); } catch (e) {} }
                }
                return out;
            }";

    /// <summary>
    /// Runs the probe on the iframe body and returns the report. Throws when the script itself
    /// did not complete — the caller logs it as a Problem; a probe that cannot run must not
    /// read as a clean bill.
    /// </summary>
    /// <param name="iframe">The CardPen output iframe (<c>#cpOutput</c>).</param>
    /// <param name="cardSetName">Card set name, for the report title and file name.</param>
    /// <param name="language">Harvested language.</param>
    /// <param name="side">What the iframe currently holds: "face" or "back".</param>
    public static async Task<FontProbeReport> DetectAsync(
        IFrameLocator iframe,
        string cardSetName,
        string language,
        string side)
    {
        if (iframe == null) throw new ArgumentNullException(nameof(iframe));

        var raw = await iframe.Locator("body").EvaluateAsync<JsonElement>(
            ProbeScript,
            new { sentinelFamily = SentinelFamily, sentinelBase64 = SentinelFontBase64 });

        if (!raw.TryGetProperty("ok", out var ok) || ok.ValueKind != JsonValueKind.True)
        {
            var jsError = Text(raw, "error");
            throw new InvalidOperationException(
                "the font probe script did not complete: " + (jsError ?? "ok=false without an error message"));
        }

        var report = new FontProbeReport
        {
            CardSetName = cardSetName ?? string.Empty,
            Language = language ?? string.Empty,
            Side = side ?? string.Empty,
            GeneratedAtUtc = DateTime.UtcNow
        };

        foreach (var f in raw.GetProperty("faces").EnumerateArray())
        {
            report.Faces.Add(new FontFaceStatus
            {
                Family = Text(f, "family") ?? string.Empty,
                Weight = Text(f, "weight") ?? string.Empty,
                Style = Text(f, "style") ?? string.Empty,
                Status = Text(f, "status") ?? string.Empty
            });
        }

        foreach (var g in raw.GetProperty("groups").EnumerateArray())
        {
            var probe = new FontStackProbe
            {
                Stack = Text(g, "stack") ?? string.Empty,
                Weight = Text(g, "weight") ?? string.Empty,
                Style = Text(g, "style") ?? string.Empty,
                Script = Text(g, "script") ?? string.Empty,
                Sample = Text(g, "sample") ?? string.Empty,
                LetterCount = Int(g, "letterCount"),
                Missing = Text(g, "missing") ?? string.Empty,
                MissingLetterCount = Int(g, "missingCount"),
                RenderedBy = Text(g, "renderedBy"),
                Verdict = Text(g, "verdict") ?? string.Empty,
                TextNodes = Int(g, "textNodes"),
                RenderedTextNodes = Int(g, "renderedTextNodes"),
                AffectedTextNodes = Int(g, "affectedTextNodes")
            };
            foreach (var c in g.GetProperty("coverage").EnumerateArray())
            {
                probe.Coverage.Add(new FontFamilyCoverage
                {
                    Family = Text(c, "family") ?? string.Empty,
                    Covered = Int(c, "covered")
                });
            }
            foreach (var x in g.GetProperty("examples").EnumerateArray())
            {
                probe.Examples.Add(new FontProbeExample
                {
                    Selector = Text(x, "selector") ?? string.Empty,
                    CardName = Text(x, "cardName") ?? string.Empty,
                    Snippet = Text(x, "snippet") ?? string.Empty
                });
            }
            report.Probes.Add(probe);
        }

        var stats = raw.GetProperty("stats");
        report.Stats = new FontProbeStats
        {
            TextNodes = Int(stats, "textNodes"),
            RenderedTextNodes = Int(stats, "renderedTextNodes"),
            HiddenTextNodes = Int(stats, "hiddenTextNodes"),
            DeclaredFaces = Int(stats, "declaredFaces"),
            ErrorFaces = Int(stats, "errorFaces"),
            UnloadedFaces = Int(stats, "unloadedFaces"),
            Groups = Int(stats, "groups")
        };
        return report;
    }

    private static string Text(JsonElement element, string name)
        => element.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.String
            ? p.GetString()
            : null;

    private static int Int(JsonElement element, string name)
        => element.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.Number && p.TryGetInt32(out var v)
            ? v
            : 0;

    /// <summary>
    /// The lines the harvest logs for a report. Pure function (no I/O). A finding, a face in
    /// error, or an empty probe is a Problem; the clean summary is Info.
    /// </summary>
    public static IReadOnlyList<FontProbeLogLine> BuildLogLines(FontProbeReport report, string reportPath = null)
    {
        if (report == null) throw new ArgumentNullException(nameof(report));
        var lines = new List<FontProbeLogLine>();
        var where = $"{report.CardSetName} {report.Language} {report.Side}".Trim();

        foreach (var face in report.ErrorFaces)
        {
            lines.Add(FontProbeLogLine.Problem(
                $"[FONT-LOAD-FAILURE] {where}: @font-face '{face.Family}' (weight {face.Weight}, style {face.Style}) " +
                "is in status 'error' — declared, needed by the page, not loaded."));
        }

        foreach (var p in report.Findings.OrderByDescending(f => f.AffectedTextNodes))
        {
            var detail = p.Verdict == VerdictFallback
                ? $"no declared family of the stack renders these letters ({p.MissingLetterCount}/{p.LetterCount}) — the system fallback font paints them"
                : p.Verdict == VerdictPartial
                    ? $"'{p.RenderedBy}' renders {p.LetterCount - p.MissingLetterCount}/{p.LetterCount} letters — the other {p.MissingLetterCount} ({p.Missing}) go to the system fallback font"
                    : $"letters of one script shared by several declared families ({p.RenderedBy})";
            var example = p.Examples.Count > 0
                ? $" e.g. \"{p.Examples[0].CardName}\" ({p.Examples[0].Selector}): \"{p.Examples[0].Snippet}\""
                : string.Empty;
            lines.Add(FontProbeLogLine.Problem(
                $"[FONT-FALLBACK] {where}: {p.Script} ({p.Weight}, {p.Style}) — {detail}. " +
                $"Stack: {p.Stack}. {p.AffectedTextNodes} text node(s) affected.{example}"));
        }

        var trailer = string.IsNullOrEmpty(reportPath) ? "." : $". Report: {reportPath}";
        if (report.IsEmpty)
        {
            lines.Add(FontProbeLogLine.Problem(
                $"[Fonts] {where}: no laid-out text found in the iframe — this probe says nothing about fonts" + trailer));
            return lines;
        }

        var summary =
            $"[Fonts] {where}: {report.Stats.DeclaredFaces} face(s) declared — " +
            $"{report.Stats.DeclaredFaces - report.Stats.ErrorFaces - report.Stats.UnloadedFaces} loaded, " +
            $"{report.Stats.ErrorFaces} in error, {report.Stats.UnloadedFaces} unused; " +
            $"{report.Stats.Groups} stack(s) x script(s) probed — {report.FindingCount} with a fallback, " +
            $"{report.AffectedTextNodes} text node(s) affected" + trailer;
        lines.Add(report.HasFinding ? FontProbeLogLine.Problem(summary) : FontProbeLogLine.Info(summary));
        return lines;
    }

    /// <summary>
    /// Renders the markdown report and writes it to <paramref name="outputPath"/>. Creates the
    /// parent directory if it does not exist. Overwrites any existing file.
    /// </summary>
    public static void WriteMarkdownReport(FontProbeReport report, string outputPath)
    {
        if (report == null) throw new ArgumentNullException(nameof(report));
        if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentException("outputPath required", nameof(outputPath));
        var dir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(outputPath, FormatMarkdown(report), new UTF8Encoding(false));
    }

    /// <summary>
    /// Formats the report as markdown. Pure function (no I/O) to make it unit-testable without
    /// Playwright or filesystem. A report with nothing probed says so instead of reading clean.
    /// </summary>
    public static string FormatMarkdown(FontProbeReport report)
    {
        if (report == null) throw new ArgumentNullException(nameof(report));
        var sb = new StringBuilder();
        var culture = CultureInfo.InvariantCulture;
        sb.Append("# Font fallback report — ").Append(report.CardSetName)
          .Append(" (").Append(report.Language).Append(") — ").AppendLine(report.Side);
        sb.Append("Generated: ")
          .AppendLine(report.GeneratedAtUtc.ToString("yyyy-MM-ddTHH:mm:ssZ", culture));
        sb.AppendLine();
        sb.AppendLine("Verdicts are per letter: a declared family renders a letter, or it does not. " +
                      "Digits, punctuation and emoji are not probed (a symbol font normally paints them). " +
                      "Text that is not laid out is counted but never reported as a finding.");
        sb.AppendLine();
        sb.Append("**").Append(report.Stats.DeclaredFaces.ToString(culture)).Append(" face(s) declared · ")
          .Append((report.Stats.DeclaredFaces - report.Stats.ErrorFaces - report.Stats.UnloadedFaces).ToString(culture))
          .Append(" loaded · ").Append(report.Stats.ErrorFaces.ToString(culture)).Append(" in error · ")
          .Append(report.Stats.UnloadedFaces.ToString(culture)).AppendLine(" unused**");
        sb.Append("**").Append(report.Stats.TextNodes.ToString(culture)).Append(" text node(s) with letters · ")
          .Append(report.Stats.RenderedTextNodes.ToString(culture)).Append(" laid out · ")
          .Append(report.Stats.Groups.ToString(culture)).Append(" stack(s) x script(s) probed · ")
          .Append(report.FindingCount.ToString(culture)).Append(" with a fallback · ")
          .Append(report.AffectedTextNodes.ToString(culture)).AppendLine(" text node(s) affected**");
        sb.AppendLine();

        if (report.IsEmpty)
        {
            sb.AppendLine("No laid-out text in the iframe: this report says nothing about fonts. :warning:");
            return sb.ToString();
        }

        var errorFaces = report.ErrorFaces.ToList();
        var findings = report.Findings.OrderByDescending(f => f.AffectedTextNodes).ToList();
        if (!report.HasFinding)
        {
            sb.AppendLine("No font fallback detected. :sparkles:");
        }

        if (errorFaces.Count > 0)
        {
            sb.AppendLine("## Faces in error");
            sb.AppendLine();
            sb.AppendLine("| Family | Weight | Style | Status |");
            sb.AppendLine("|--------|--------|-------|--------|");
            foreach (var f in errorFaces)
            {
                sb.Append("| `").Append(EscapePipes(f.Family)).Append("` | ").Append(EscapePipes(f.Weight))
                  .Append(" | ").Append(EscapePipes(f.Style)).Append(" | ").Append(f.Status).AppendLine(" |");
            }
            sb.AppendLine();
        }

        if (findings.Count > 0)
        {
            sb.AppendLine("## Fallback findings");
            sb.AppendLine();
            sb.AppendLine("| Script | Verdict | Stack | Weight | Letters without a declared family | Rendered by | Text nodes affected |");
            sb.AppendLine("|--------|---------|-------|--------|----------------------------------|-------------|---------------------|");
            foreach (var p in findings)
            {
                sb.Append("| ").Append(EscapePipes(p.Script))
                  .Append(" | ").Append(p.Verdict)
                  .Append(" | `").Append(EscapePipes(p.Stack)).Append("` | ").Append(EscapePipes(p.Weight))
                  .Append(" | ").Append(p.MissingLetterCount.ToString(culture)).Append(" / ").Append(p.LetterCount.ToString(culture))
                  .Append(" | ").Append(EscapePipes(p.RenderedBy ?? "—"))
                  .Append(" | ").Append(p.AffectedTextNodes.ToString(culture)).AppendLine(" |");
            }
            sb.AppendLine();

            foreach (var p in findings)
            {
                sb.Append("### ").Append(p.Script).Append(" — ").Append(p.Verdict).Append(" — `")
                  .Append(EscapePipes(p.Stack)).Append("` (").Append(EscapePipes(p.Weight)).Append(", ")
                  .Append(EscapePipes(p.Style)).AppendLine(")");
                sb.AppendLine();
                sb.Append("Letters probed (").Append(p.LetterCount.ToString(culture)).Append("): ")
                  .AppendLine(p.Sample);
                if (p.MissingLetterCount > 0)
                {
                    sb.Append("Letters no declared family renders (").Append(p.MissingLetterCount.ToString(culture))
                      .Append("): ").AppendLine(p.Missing);
                }
                sb.AppendLine();
                sb.AppendLine("| Family (stack order) | Letters it renders |");
                sb.AppendLine("|----------------------|--------------------|");
                foreach (var c in p.Coverage)
                {
                    sb.Append("| `").Append(EscapePipes(c.Family)).Append("` | ")
                      .Append(c.Covered.ToString(culture)).AppendLine(" |");
                }
                sb.AppendLine();
                sb.Append(p.RenderedTextNodes.ToString(culture)).Append(" text node(s) laid out on ")
                  .Append(p.TextNodes.ToString(culture)).Append(" in this stack; ")
                  .Append(p.AffectedTextNodes.ToString(culture)).AppendLine(" affected.");
                sb.AppendLine();
                foreach (var x in p.Examples)
                {
                    sb.Append("- `").Append(EscapePipes(x.Selector)).Append("` in card \"")
                      .Append(EscapePipes(x.CardName)).Append("\": \"").Append(EscapePipes(x.Snippet)).AppendLine("\"");
                }
                sb.AppendLine();
            }
        }

        var inventory = report.Probes.Where(p => !p.IsFinding)
            .OrderBy(p => p.Script, StringComparer.Ordinal).ThenBy(p => p.Stack, StringComparer.Ordinal).ToList();
        if (inventory.Count > 0)
        {
            sb.AppendLine("## Probed stacks without finding");
            sb.AppendLine();
            sb.AppendLine("| Script | Verdict | Stack | Weight | Style | Rendered by | Text nodes laid out / total |");
            sb.AppendLine("|--------|---------|-------|--------|-------|-------------|-----------------------------|");
            foreach (var p in inventory)
            {
                var renderedBy = p.RenderedBy ?? (p.Verdict == VerdictGeneric ? "(generic only)" : "—");
                sb.Append("| ").Append(EscapePipes(p.Script))
                  .Append(" | ").Append(p.Verdict)
                  .Append(" | `").Append(EscapePipes(p.Stack)).Append("` | ").Append(EscapePipes(p.Weight))
                  .Append(" | ").Append(EscapePipes(p.Style))
                  .Append(" | ").Append(EscapePipes(renderedBy))
                  .Append(" | ").Append(p.RenderedTextNodes.ToString(culture)).Append(" / ")
                  .Append(p.TextNodes.ToString(culture)).AppendLine(" |");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private static string EscapePipes(string value)
        => string.IsNullOrEmpty(value) ? string.Empty : value.Replace("|", "\\|");
}

public class FontProbeReport
{
    public string CardSetName { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;

    /// <summary>What the iframe held when probed: "face" or "back".</summary>
    public string Side { get; set; } = string.Empty;

    public DateTime GeneratedAtUtc { get; set; }
    public List<FontFaceStatus> Faces { get; } = new();
    public List<FontStackProbe> Probes { get; } = new();
    public FontProbeStats Stats { get; set; } = new();

    [JsonIgnore]
    public IEnumerable<FontFaceStatus> ErrorFaces => Faces.Where(f => f.IsError);

    /// <summary>Probes of laid-out text whose declared families leave letters unpainted.</summary>
    [JsonIgnore]
    public IEnumerable<FontStackProbe> Findings => Probes.Where(p => p.IsFinding);

    [JsonIgnore]
    public int FindingCount => Probes.Count(p => p.IsFinding);

    [JsonIgnore]
    public int AffectedTextNodes => Probes.Where(p => p.IsFinding).Sum(p => p.AffectedTextNodes);

    /// <summary>True when nothing is laid out: the verdicts concern painted letters, so nothing was measured.</summary>
    [JsonIgnore]
    public bool IsEmpty => Stats.RenderedTextNodes == 0;

    [JsonIgnore]
    public bool HasFinding => Faces.Any(f => f.IsError) || Probes.Any(p => p.IsFinding);
}

public class FontProbeStats
{
    public int TextNodes { get; set; }
    public int RenderedTextNodes { get; set; }
    public int HiddenTextNodes { get; set; }
    public int DeclaredFaces { get; set; }

    /// <summary>Faces whose load was triggered and failed.</summary>
    public int ErrorFaces { get; set; }

    /// <summary>Faces declared and never needed by the page (e.g. a script the language does not use). Not a defect.</summary>
    public int UnloadedFaces { get; set; }
    public int Groups { get; set; }
}

public class FontFaceStatus
{
    public string Family { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;

    /// <summary><c>unloaded</c>, <c>loading</c>, <c>loaded</c> or <c>error</c>.</summary>
    public string Status { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsError => Status == "error";
}

public class FontFamilyCoverage
{
    public string Family { get; set; } = string.Empty;

    /// <summary>Letters of the group this family renders (attributed in stack order: a letter already rendered by an earlier family is not counted here).</summary>
    public int Covered { get; set; }
}

public class FontProbeExample
{
    public string Selector { get; set; } = string.Empty;
    public string CardName { get; set; } = string.Empty;
    public string Snippet { get; set; } = string.Empty;
}

public class FontStackProbe
{
    public string Stack { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;

    /// <summary>Unicode script of the letters probed by this group (Cyrillic, Latin, Han…).</summary>
    public string Script { get; set; } = string.Empty;

    /// <summary>Every distinct letter measured in this group.</summary>
    public string Sample { get; set; } = string.Empty;
    public int LetterCount { get; set; }

    /// <summary>Letters no declared family of the stack renders.</summary>
    public string Missing { get; set; } = string.Empty;
    public int MissingLetterCount { get; set; }

    /// <summary>The declared families that render letters ("A | B" when two share them); null when none does (verdict <c>fallback</c>).</summary>
    public string RenderedBy { get; set; }

    /// <summary>
    /// <c>clean</c>: one declared family renders every letter. <c>generic</c>: the stack names no
    /// family (nothing is declared). <c>fallback</c>: no declared family renders any letter.
    /// <c>partial</c>: a declared family renders some, the system fallback paints the rest.
    /// <c>split</c>: declared families render everything but share the letters of one script.
    /// </summary>
    public string Verdict { get; set; } = string.Empty;

    public int TextNodes { get; set; }

    /// <summary>Text nodes of the group that are laid out (not display:none).</summary>
    public int RenderedTextNodes { get; set; }

    public int AffectedTextNodes { get; set; }
    public List<FontFamilyCoverage> Coverage { get; } = new();
    public List<FontProbeExample> Examples { get; } = new();

    [JsonIgnore]
    public bool IsFinding => RenderedTextNodes > 0 &&
        (Verdict == FontFallbackDetector.VerdictFallback
         || Verdict == FontFallbackDetector.VerdictPartial
         || Verdict == FontFallbackDetector.VerdictSplit);
}

/// <summary>One line for the harvest log.</summary>
public class FontProbeLogLine
{
    public string Message { get; }
    public bool IsProblem { get; }

    private FontProbeLogLine(string message, bool isProblem)
    {
        Message = message;
        IsProblem = isProblem;
    }

    public static FontProbeLogLine Problem(string message) => new(message, true);

    public static FontProbeLogLine Info(string message) => new(message, false);
}
