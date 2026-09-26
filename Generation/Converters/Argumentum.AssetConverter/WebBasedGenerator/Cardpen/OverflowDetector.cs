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
/// Detects CSS overflow / clipping on rendered CardPen cards inside the #cpOutput iframe,
/// before the DOM is captured to PNG. Non-intrusive: only reads layout metrics via
/// a JS snippet evaluated in the iframe body. Used in phase 1 of issue #190 to produce
/// an objective QA report of cards whose text exceeds its allotted space.
/// </summary>
public static class OverflowDetector
{
    /// <summary>
    /// Selectors of text-bearing elements we measure inside each &lt;card&gt;. Order matters:
    /// the "container" .texte is the primary overflow site (it has CSS overflow:hidden),
    /// the others are reported for context so simplifications can target the right field.
    /// </summary>
    private static readonly string[] DefaultTargetSelectors = new[]
    {
        ".texte",
        ".desc_fr", ".desc_en", ".desc_ru", ".desc_pt", ".desc_es", ".desc_ar", ".desc_fa", ".desc_zh",
        ".exemple_fr", ".exemple_en", ".exemple_ru", ".exemple_pt", ".exemple_es", ".exemple_ar", ".exemple_fa", ".exemple_zh",
        ".title",
        ".famille",
        ".sous_famille"
    };

    /// <summary>
    /// Runs overflow detection on the currently rendered CardPen iframe body and returns
    /// one result entry per card. Cards with no overflow are still present in the list
    /// with an empty Findings collection; this keeps the report stable across runs.
    /// </summary>
    public static async Task<OverflowReport> DetectAsync(
        IFrameLocator iframe,
        string cardSetName,
        string language,
        int tolerancePx = 2,
        IEnumerable<string> targetSelectors = null)
    {
        if (iframe == null) throw new ArgumentNullException(nameof(iframe));
        var selectors = (targetSelectors ?? DefaultTargetSelectors).ToArray();

        // Single-pass JS: walks every <card> in the iframe, measures the declared selectors
        // with both box-model (offsetHeight/Width) and content-size (scrollHeight/Width),
        // PLUS two geometric comparisons the self predicate is blind to by construction
        // (#1567): an element whose box crosses out of its parent block (kind "container" —
        // a title whose last line slides under the banner), and an element whose box reaches
        // past the card edge (kind "card" — a remark sliced by the bottom of the card).
        // Returns a JSON-serializable payload that we hydrate to strongly-typed results.
        const string js = @"(element, args) => {
            const selectors = args.selectors;
            const tolerance = args.tolerance;
            const cards = Array.from(document.querySelectorAll('card'));
            return cards.map((cardEl, idx) => {
                const nameEl = cardEl.querySelector('.cardName');
                const cardName = nameEl ? (nameEl.textContent || '').trim() : '';
                const findings = [];
                const cardRect = cardEl.getBoundingClientRect();
                const push = (el, sel, kind, excessH, excessW) => {
                    const cs = getComputedStyle(el);
                    const text = (el.textContent || '').replace(/\s+/g, ' ').trim();
                    findings.push({
                        selector: sel,
                        kind: kind,
                        scrollWidth: el.scrollWidth,
                        scrollHeight: el.scrollHeight,
                        clientWidth: el.clientWidth,
                        clientHeight: el.clientHeight,
                        offsetWidth: el.offsetWidth,
                        offsetHeight: el.offsetHeight,
                        excessWidth: Math.max(0, excessW),
                        excessHeight: Math.max(0, excessH),
                        overflowCss: cs.overflow,
                        fontSizePx: parseFloat(cs.fontSize) || 0,
                        textLength: text.length,
                        textSnippet: text.length > 160 ? text.substring(0, 157) + '...' : text
                    });
                };
                for (const sel of selectors) {
                    const nodes = Array.from(cardEl.querySelectorAll(sel));
                    for (const el of nodes) {
                        const excessH = el.scrollHeight - el.clientHeight;
                        const excessW = el.scrollWidth - el.clientWidth;
                        if (excessH > tolerance || excessW > tolerance) {
                            push(el, sel, 'self', excessH, excessW);
                        }
                        const r = el.getBoundingClientRect();
                        const byCardH = r.bottom - cardRect.bottom;
                        const byCardW = r.right - cardRect.right;
                        if (byCardH > tolerance || byCardW > tolerance) {
                            push(el, sel, 'card', byCardH, byCardW);
                            continue;
                        }
                        const parent = el.parentElement;
                        if (parent && parent !== cardEl) {
                            const pr = parent.getBoundingClientRect();
                            const byParentH = r.bottom - pr.bottom;
                            const byParentW = r.right - pr.right;
                            if (byParentH > tolerance || byParentW > tolerance) {
                                push(el, sel, 'container', byParentH, byParentW);
                            }
                        }
                    }
                }
                return { cardIndex: idx, cardName: cardName, findings: findings };
            });
        }";

        var body = iframe.Locator("body");
        var raw = await body.EvaluateAsync<JsonElement>(
            js,
            new { selectors = selectors, tolerance = tolerancePx });

        var cards = new List<CardOverflowResult>();
        foreach (var item in raw.EnumerateArray())
        {
            var findings = new List<OverflowFinding>();
            foreach (var f in item.GetProperty("findings").EnumerateArray())
            {
                findings.Add(new OverflowFinding
                {
                    Kind = f.TryGetProperty("kind", out var kindProp)
                        ? kindProp.GetString() ?? "self"
                        : "self",
                    Selector = f.GetProperty("selector").GetString() ?? string.Empty,
                    ScrollWidth = f.GetProperty("scrollWidth").GetDouble(),
                    ScrollHeight = f.GetProperty("scrollHeight").GetDouble(),
                    ClientWidth = f.GetProperty("clientWidth").GetDouble(),
                    ClientHeight = f.GetProperty("clientHeight").GetDouble(),
                    OffsetWidth = f.GetProperty("offsetWidth").GetDouble(),
                    OffsetHeight = f.GetProperty("offsetHeight").GetDouble(),
                    ExcessWidth = f.GetProperty("excessWidth").GetDouble(),
                    ExcessHeight = f.GetProperty("excessHeight").GetDouble(),
                    OverflowCss = f.GetProperty("overflowCss").GetString() ?? string.Empty,
                    FontSizePx = f.GetProperty("fontSizePx").GetDouble(),
                    TextLength = f.GetProperty("textLength").GetInt32(),
                    TextSnippet = f.GetProperty("textSnippet").GetString() ?? string.Empty
                });
            }
            cards.Add(new CardOverflowResult
            {
                CardIndex = item.GetProperty("cardIndex").GetInt32(),
                CardName = item.GetProperty("cardName").GetString() ?? string.Empty,
                Findings = findings
            });
        }

        return new OverflowReport
        {
            CardSetName = cardSetName,
            Language = language,
            GeneratedAtUtc = DateTime.UtcNow,
            TolerancePx = tolerancePx,
            Cards = cards
        };
    }

    /// <summary>
    /// Renders the markdown report and writes it to <paramref name="outputPath"/>.
    /// Creates the parent directory if it does not exist. Overwrites any existing file.
    /// </summary>
    public static void WriteMarkdownReport(OverflowReport report, string outputPath)
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
    /// Formats the report as markdown. Pure function (no I/O) to make it unit-testable
    /// without Playwright or filesystem.
    /// </summary>
    public static string FormatMarkdown(OverflowReport report)
    {
        if (report == null) throw new ArgumentNullException(nameof(report));
        var sb = new StringBuilder();
        var culture = CultureInfo.InvariantCulture;
        sb.Append("# Overflow detection report — ").Append(report.CardSetName)
          .Append(" (").Append(report.Language).AppendLine(")");
        sb.Append("Generated: ")
          .Append(report.GeneratedAtUtc.ToString("yyyy-MM-ddTHH:mm:ssZ", culture))
          .Append(" · tolerance: ").Append(report.TolerancePx.ToString(culture))
          .AppendLine("px");
        sb.AppendLine();

        var cardsWithOverflow = report.Cards.Where(c => c.Findings.Count > 0).ToList();
        sb.Append("**").Append(cardsWithOverflow.Count.ToString(culture))
          .Append(" / ").Append(report.Cards.Count.ToString(culture))
          .AppendLine(" cards have at least one overflow.**");
        sb.AppendLine();

        if (cardsWithOverflow.Count == 0)
        {
            sb.AppendLine("No overflow detected. :sparkles:");
            return sb.ToString();
        }

        // Per-card sections, sorted by worst excess first so the most urgent cards are on top.
        var sorted = cardsWithOverflow
            .OrderByDescending(c => c.Findings.Max(f => Math.Max(f.ExcessHeight, f.ExcessWidth)))
            .ToList();

        sb.AppendLine("## Cards with overflow");
        sb.AppendLine();
        sb.AppendLine("| # | Card | Worst excess (px) | Selectors |");
        sb.AppendLine("|---|------|-------------------|-----------|");
        foreach (var card in sorted)
        {
            var worst = card.Findings.Max(f => Math.Max(f.ExcessHeight, f.ExcessWidth));
            var selectors = string.Join(", ",
                card.Findings.Select(f => $"{f.Selector} ({f.Kind})").Distinct());
            sb.Append("| ").Append(card.CardIndex.ToString(culture))
              .Append(" | ").Append(EscapePipes(card.CardName))
              .Append(" | ").Append(worst.ToString("F1", culture))
              .Append(" | ").Append(EscapePipes(selectors))
              .AppendLine(" |");
        }
        sb.AppendLine();

        sb.AppendLine("## Detailed findings");
        sb.AppendLine();
        foreach (var card in sorted)
        {
            sb.Append("### ").Append(card.CardIndex.ToString(culture)).Append(" — ").AppendLine(card.CardName);
            sb.AppendLine();
            sb.AppendLine("| Selector | Kind | Excess H (px) | Excess W (px) | Font (px) | Text len | Snippet |");
            sb.AppendLine("|----------|------|---------------|---------------|-----------|----------|---------|");
            foreach (var f in card.Findings.OrderByDescending(f => Math.Max(f.ExcessHeight, f.ExcessWidth)))
            {
                sb.Append("| `").Append(f.Selector).Append("` | ").Append(f.Kind).Append(" | ")
                  .Append(f.ExcessHeight.ToString("F1", culture)).Append(" | ")
                  .Append(f.ExcessWidth.ToString("F1", culture)).Append(" | ")
                  .Append(f.FontSizePx.ToString("F1", culture)).Append(" | ")
                  .Append(f.TextLength.ToString(culture)).Append(" | ")
                  .Append(EscapePipes(f.TextSnippet))
                  .AppendLine(" |");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private static string EscapePipes(string value)
        => string.IsNullOrEmpty(value) ? string.Empty : value.Replace("|", "\\|");
}

public class OverflowReport
{
    public string CardSetName { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public DateTime GeneratedAtUtc { get; set; }
    public int TolerancePx { get; set; }
    public List<CardOverflowResult> Cards { get; set; } = new();

    [JsonIgnore]
    public int CardsWithOverflowCount => Cards.Count(c => c.Findings.Count > 0);
}

public class CardOverflowResult
{
    public int CardIndex { get; set; }
    public string CardName { get; set; } = string.Empty;
    public List<OverflowFinding> Findings { get; set; } = new();
}

public class OverflowFinding
{
    /// <summary>
    /// Which comparison produced this finding:
    /// <c>self</c> — the element's own content exceeds its box (scroll vs client);
    /// <c>container</c> — the element's box crosses out of its parent block (#1567: a title
    /// whose last line slides under the banner);
    /// <c>card</c> — the element's box reaches past the card edge (#1567: a remark sliced by
    /// the bottom of the card).
    /// </summary>
    public string Kind { get; set; } = "self";

    public string Selector { get; set; } = string.Empty;
    public double ScrollWidth { get; set; }
    public double ScrollHeight { get; set; }
    public double ClientWidth { get; set; }
    public double ClientHeight { get; set; }
    public double OffsetWidth { get; set; }
    public double OffsetHeight { get; set; }
    public double ExcessWidth { get; set; }
    public double ExcessHeight { get; set; }
    public string OverflowCss { get; set; } = string.Empty;
    public double FontSizePx { get; set; }
    public int TextLength { get; set; }
    public string TextSnippet { get; set; } = string.Empty;
}
