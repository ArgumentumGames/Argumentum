using Argumentum.AssetConverter.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageMagick;
using UglyToad.PdfPig;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests
{
    /// <summary>
    /// D2 guard (#1190, ai-01 verdict 2026-08-27): on the Memo back of the Tarot deck, the
    /// last <c>.familyItems</c> border-bottom (the separator trait, in normal flow) must never
    /// descend into the <c>.colorPalette</c> band (absolutely positioned, bottom:0.6em). Two
    /// positioning systems that ignore each other produced a latent collision — crossed by
    /// en/ru/es (3 px inside the band), with fr at 1 px of clearance: not healthy, a hair.
    ///
    /// The assertion is geometric and NON-BASELINING: no y coordinate is ever compared to a
    /// constant — only the DISJUNCTION of two independently measured intervals, plus a
    /// minimum clearance that is strictly positive and excludes the 1 px hair (a &gt;= 0 organ
    /// would let fr through today and protect nobody).
    ///
    /// Two independent predicates (#1112 lesson — never derive one from the other), both
    /// geometric, both calibrated on the page's OWN color inventory:
    /// - the PALETTE is a row of >= 5 small square clusters (12-44 px side at 300 dpi,
    ///   aspect ~1:1) aligned in one band, in the bottom-right region where nothing else
    ///   draws squares (the colorBox bars sit at the left edge and are tall, not square);
    /// - the TRAIT is a hairline: consecutive pixel rows (<= 8 px tall) where a single one
    ///   of the page's dominant saturated colors covers >= 45% of the card width — only a
    ///   border-bottom row reaches that coverage (text, colorBox bars and the palette
    ///   squares stay under 25%).
    /// Why self-calibration instead of the template's CSS family tokens: the bundle PDFs are
    /// Ghostscript-CMYK (SWOP) and their rendered colors drift far past any token tolerance
    /// (measured 2026-08-27: green #8DC801 renders as #80FF00, G +55) while Target/ PDFs stay
    /// RGB — a constant set can only match one pipeline. The page's own dominant saturated
    /// colors are exactly the family fills in EITHER pipeline, and the predicates remain
    /// geometric.
    /// Measuring both in the same column merges the bands through antialiasing — the
    /// instrument lie ai-01 hit twice; the separate regions and predicates keep them apart.
    ///
    /// Inverse control obligation (#1046): on the v0.9.0-review bundle (2026-08) this test
    /// MUST FAIL on en/ru/es (trait 3 px inside the band) and fr (1 px clearance &lt; 5).
    /// If it runs green on that bundle, it verifies nothing. Degeneracy fails loud: a
    /// missing bundle, a missing PDF, an undetected palette (fewer than 5 squares) or an
    /// undetected trait each FAIL — never a silent pass "for lack of a defect".
    /// </summary>
    public class MemoBackPaletteGutterTests
    {
        private readonly ITestOutputHelper _output;

        private static readonly string[] Languages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

        /// <summary>Same convention as PdfBundleIntegrityTests: an explicit bundle root is
        /// honored (and must exist), else the generated Target tree, else fail loud.</summary>
        private const string BundleRootEnvVar = "ARGUMENTUM_PDF_BUNDLE_ROOT";

        /// <summary>How many dominant saturated colors the page inventory keeps. The Memo back
        /// draws 7 family fills (palette squares, traits, colorBox bars, family text) plus
        /// darker text variants — 14 covers fills and variants with margin.</summary>
        private const int PaletteColorInventorySize = 14;

        /// <summary>Saturation floor for the inventory (max-min channel spread): separates the
        /// family fills from the cream background and the header photo's low-spread tones.</summary>
        private const int SaturationSpread = 60;

        /// <summary>Minimum trait-to-palette clearance, in pixels at 300 dpi (~0.42 mm).
        /// Strictly positive AND strictly above the 1 px hair fr sits at on the 2026-08
        /// bundle; tolerates pt's current healthy 7 px until the next full regeneration.</summary>
        private const int MinClearancePx = 5;

        /// <summary>A trait row is one where a single inventory color covers at least this
        /// fraction of the card width. The border-bottom spans ~90% of the card; text,
        /// colorBox bars (~5%) and the 7 palette squares (~25%) stay well below.</summary>
        private const double TraitCoverageThreshold = 0.45;

        /// <summary>A qualifying trait interval is at most this tall: the 1 px CSS border
        /// renders ~3 px at 300 dpi. Anything taller is a block, not a hairline.</summary>
        private const int MaxTraitHeightPx = 8;

        /// <summary>Palette square side window at 300 dpi: 0.5em squares, card em ~42 px →
        /// ~21 px. [12..44] absorbs font rounding without accepting blobs or bars.</summary>
        private const int MinSquareSidePx = 12;
        private const int MaxSquareSidePx = 44;

        /// <summary>All palette squares must fit in one band of this height: one row of 21 px
        /// squares plus antialiasing. A taller spread means the clusters are not one row.</summary>
        private const int MaxPaletteRowSpanPx = 48;

        /// <summary>The palette sits at right:1em, bottom:0.6em — searched only in this
        /// region; the family colorBox bars (left edge) and the family area cannot interfere.</summary>
        private const double PaletteRegionXFraction = 0.55;
        private const double PaletteRegionYFraction = 0.75;

        /// <summary>Trait scan starts below the header (the header photo could otherwise
        /// contribute coverage) and covers everything the family area can reach.</summary>
        private const double TraitScanStartYFraction = 0.30;

        /// <summary>Memo backs live at pages 16-28 of the tarot deck (Rules 1-15, then
        /// Memo 7x2 back-first). Scan order starts at 16 and widens: the page is LOCATED by
        /// the palette predicate, never pinned — a pagination shift moves the scan, it does
        /// not silently measure the wrong page.</summary>
        private static readonly int[] CandidateMemoBackPages = { 16, 15, 17, 14, 18, 19, 20 };

        public MemoBackPaletteGutterTests(ITestOutputHelper output) => _output = output;

        [Fact]
        public void Memo_Back_Separator_Trait_Never_Enters_The_Palette_Band()
        {
            var root = ResolveBundleRootOrFail();
            var failures = new List<string>();
            var table = new List<string> { "| lang | page | trait y | palette y | clearance (px) |", "|---|---|---|---|---|" };

            foreach (var lang in Languages)
            {
                var pdf = Path.Combine(root, lang, "Documents", "density-0", $"Argumentum_TarotCards_{lang}.pdf");
                if (!File.Exists(pdf))
                {
                    failures.Add($"{lang}: {Path.GetFileName(pdf)} not found under {root} — the Memo back gutter cannot be verified without it (fail loud, never pass for lack of data).");
                    continue;
                }

                try
                {
                    var result = MeasureMemoBackPage(pdf);
                    if (result.Failure != null)
                    {
                        failures.Add($"{lang}: {result.Failure}");
                        continue;
                    }
                    int clearance = result.PaletteMinY - result.TraitMaxY;
                    table.Add($"| {lang} | {result.Page} | {result.TraitMinY}-{result.TraitMaxY} | {result.PaletteMinY}-{result.PaletteMaxY} | {clearance} |");
                    if (clearance < MinClearancePx)
                        failures.Add($"{lang} (tarot page {result.Page}): separator trait [{result.TraitMinY}-{result.TraitMaxY}] vs palette band [{result.PaletteMinY}-{result.PaletteMaxY}] — clearance {clearance}px < {MinClearancePx}px minimum (trait inside or touching the palette band, #1190 D2).");
                }
                catch (Exception ex)
                {
                    failures.Add($"{lang}: measurement failed ({ex.GetType().Name}: {FirstLine(ex.Message)}) — an unusable instrument fails loud, it never passes.");
                }
            }

            foreach (var row in table) _output.WriteLine(row);
            if (failures.Count > 0)
                Assert.Fail($"Memo back palette-band gutter violations (#1190 D2 — the in-flow separator trait must stay at least {MinClearancePx}px above the absolute palette band, on every language):\n  {string.Join("\n  ", failures)}");

            _output.WriteLine($"PASS: {Languages.Length} languages, every Memo back trait clears the palette band by >= {MinClearancePx}px.");
        }

        /// <summary>Pre-bundle validation lane: the SAME predicates on the harvested Memo
        /// back PNGs (Target/{lang}/Images/**/memo_back.png) — the exact pixels the tarot
        /// page embeds. This is how a template fix gets validated minutes after a scoped
        /// harvest (#1190 DoD: "regenerate the Memo back only, not the bundle") instead of
        /// waiting for a full bundle pass. Opt-in via {ImageRootEnvVar}: an explicit root
        /// must exist and EVERY language must be found there — fail loud, never green for
        /// lack of data. Without the env var the lane runs against Target Memo images when
        /// present and reports "not applicable" when absent — a visible note, and the
        /// bundle-page Fact above remains the shipped-artifact guard.</summary>
        [Fact]
        public void Memo_Back_Separator_Trait_Clears_Palette_Band_On_Harvested_Images()
        {
            const string imageRootEnvVar = "ARGUMENTUM_MEMO_BACK_IMAGE_ROOT";
            var explicitRoot = Environment.GetEnvironmentVariable(imageRootEnvVar);
            string? root;
            bool explicitRequest = false;
            if (!string.IsNullOrWhiteSpace(explicitRoot))
            {
                if (!Directory.Exists(explicitRoot))
                    Assert.Fail($"{imageRootEnvVar} is set to '{explicitRoot}' which does not exist — an explicitly requested image root must fail loud, never pass green.");
                root = explicitRoot;
                explicitRequest = true;
            }
            else
            {
                root = ExistingTargetRootOrNull();
            }

            var failures = new List<string>();
            var table = new List<string> { "| lang | image | trait y | palette y | clearance (px) |", "|---|---|---|---|---|" };
            int measured = 0;
            foreach (var lang in Languages)
            {
                var file = root == null ? null : FindMemoBackImage(root, lang);
                if (file == null)
                {
                    if (explicitRequest)
                        failures.Add($"{lang}: no memo_back.png under {Path.Combine(root!, lang)} — an explicitly requested lane must find every language, never pass for lack of data.");
                    else
                        _output.WriteLine($"NOTE {lang}: no harvested memo_back.png — lane not applicable for this language.");
                    continue;
                }
                using var image = new MagickImage(file);
                var failure = MeasureCard(image, out var band, out var traits);
                if (failure != null)
                {
                    failures.Add($"{lang}: {failure}");
                    continue;
                }
                var last = traits.OrderBy(t => t.MaxY).Last();
                int clearance = band.MinY - last.MaxY;
                table.Add($"| {lang} | {Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(file)))} | {last.MinY}-{last.MaxY} | {band.MinY}-{band.MaxY} | {clearance} |");
                measured++;
                if (clearance < MinClearancePx)
                    failures.Add($"{lang}: separator trait [{last.MinY}-{last.MaxY}] vs palette band [{band.MinY}-{band.MaxY}] — clearance {clearance}px < {MinClearancePx}px minimum (#1190 D2, harvested image).");
            }

            foreach (var row in table) _output.WriteLine(row);
            if (failures.Count > 0)
                Assert.Fail($"Harvested Memo back gutter violations (#1190 D2):\n  {string.Join("\n  ", failures)}");
            if (measured == 0)
                _output.WriteLine($"NOTE: lane not applicable — no harvested Memo back images found (set {imageRootEnvVar} to validate a specific harvest, or run the pipeline). This Fact asserted nothing; the bundle-page guard is the shipped-artifact one.");
            else
                _output.WriteLine($"PASS: {measured} harvested Memo back(s) clear the palette band by >= {MinClearancePx}px.");
        }

        /// <summary>The tarot-density Memo CardSet folder is plain "Memo" (the P&P variant
        /// has its own folder); prefer it when both exist.</summary>
        private static string? FindMemoBackImage(string root, string lang)
        {
            var candidates = Directory.EnumerateFiles(Path.Combine(root, lang), "memo_back.png", SearchOption.AllDirectories).ToList();
            return candidates.FirstOrDefault(c => c.Contains($"{Path.DirectorySeparatorChar}Memo{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
                ?? candidates.FirstOrDefault();
        }

        private static string? ExistingTargetRootOrNull()
        {
            var repoRoot = TestRepoRoot.Find();
            foreach (var build in new[] { "Release", "Debug" })
            {
                var target = Path.Combine(repoRoot, "Generation", "Converters", "Argumentum.AssetConverter", "bin", build, "net9.0-windows", "Target");
                if (Directory.Exists(target)) return target;
            }
            return null;
        }

        private readonly record struct PageMeasure(int Page, int TraitMinY, int TraitMaxY, int PaletteMinY, int PaletteMaxY, string? Failure);

        /// <summary>Locates the first Memo back page — the page whose BOTH predicates fire
        /// (>= 5 aligned palette squares AND >= 4 family-row hairlines: the 7-family table
        /// signature). Requiring both is what keeps the locator off look-alike pages (a
        /// Rules face can carry square-ish color chips; it does not carry 7 full-width
        /// hairlines). Then returns the measured intervals.</summary>
        private PageMeasure MeasureMemoBackPage(string pdf)
        {
            var misses = new List<(int Page, string Why)>();
            foreach (var page in CandidateMemoBackPages)
            {
                using var card = ExtractPageCardImage(pdf, page);
                var failure = MeasureCard(card, out var band, out var traits);
                if (failure != null)
                { misses.Add((page, failure)); continue; }
                var last = traits.OrderBy(t => t.MaxY).Last();
                return new PageMeasure(page, last.MinY, last.MaxY, band.MinY, band.MaxY, null);
            }
            return new PageMeasure(0, 0, 0, 0, 0,
                $"no Memo back page found among pages {string.Join("/", CandidateMemoBackPages)} (signature hit on none: {string.Join("; ", misses.Select(m => $"p{m.Page}: {m.Why}"))}) — either pagination moved beyond the scan window or the Memo back no longer renders its 7-family table; both must fail loud, not pass.");
        }

        /// <summary>Shared core: measures one Memo back render (embedded PDF image or
        /// harvested PNG — same pixels, the tarot page embeds the harvest). Returns null on
        /// success, else a failure naming the degenerate predicate.</summary>
        private static string? MeasureCard(MagickImage card, out (int MinY, int MaxY) paletteBand, out List<(int MinY, int MaxY)> traits)
        {
            paletteBand = default;
            traits = new List<(int, int)>();
            var (w, h) = ((int)card.Width, (int)card.Height);
            if (w < 600 || h < 1100)
                return $"raster too small ({w}x{h}) — not a 300dpi tarot card render";
            var rgb = RgbBytesOrThrow(card, w, h);
            var inventory = DominantSaturatedColors(rgb, w, h);
            if (!DetectPaletteBand(rgb, w, h, inventory, out paletteBand, out var whyNot))
                return whyNot;
            traits = DetectTraitIntervals(rgb, w, h, inventory);
            if (traits.Count < MinTraitsOnMemoBack)
                return $"palette found but only {traits.Count} full-width hairline(s) (need >= {MinTraitsOnMemoBack}) — not the 7-family Memo back signature";
            return null;
        }

        /// <summary>The Memo back draws one border-bottom per family row — 7 rows, of which
        /// at least this many must register as hairlines for a page to BE the Memo back.</summary>
        private const int MinTraitsOnMemoBack = 4;

        /// <summary>Extracts the card image EMBEDDED in the page instead of rasterizing the
        /// PDF: the tarot documents are pure-image QuestPDF impositions, so the page's image
        /// XObject IS the card render at native density. This needs no Ghostscript delegate
        /// (portable instrument) and measures the actual shipped pixels, not a re-render.</summary>
        private static MagickImage ExtractPageCardImage(string pdf, int oneBasedPage)
        {
            using var doc = PdfDocument.Open(pdf);
            var page = doc.GetPage(oneBasedPage);
            var images = page.GetImages().ToList();
            if (images.Count == 0)
                throw new InvalidDataException($"page {oneBasedPage} of {Path.GetFileName(pdf)} has no image XObject — not a card page of a pure-image imposition.");
            var largest = images.OrderByDescending(i => i.RawBytes.Length).First();
            if (!largest.TryGetPng(out var png))
                throw new InvalidDataException($"page {oneBasedPage} of {Path.GetFileName(pdf)}: PdfPig could not decode the embedded image to PNG (unsupported filter/colorspace) — the instrument is unusable, failing loud.");
            return new MagickImage(new MemoryStream(png));
        }

        /// <summary>PALETTE predicate (geometry): a row of >= 5 square clusters of inventory
        /// colors in the bottom-right region, aligned in one band. Returns the row's y-band.
        /// Each color is split into COLUMN-CONTIGUOUS groups before its bbox is evaluated:
        /// when the defective red trait descends INTO the band, it x-extends the red square's
        /// pixels into one wide blob — a whole-color bbox would reject the square and blind
        /// the organ exactly on the page it exists to judge.</summary>
        private static bool DetectPaletteBand(byte[] rgb, int w, int h, List<int> inventory, out (int MinY, int MaxY) band, out string whyNot)
        {
            band = default;
            int x0 = (int)(w * PaletteRegionXFraction), y0 = (int)(h * PaletteRegionYFraction);
            var squares = new List<(int MinY, int MaxY)>();
            foreach (var color in inventory)
            {
                // Column histogram of this color in the region, then split into groups of
                // non-empty columns separated by >= 8 empty columns.
                var colCount = new int[w - x0];
                var colMinY = new int[w - x0];
                var colMaxY = new int[w - x0];
                for (int x = 0; x < colCount.Length; x++) colMinY[x] = int.MaxValue;
                for (int y = y0; y < h; y++)
                    for (int x = x0; x < w; x++)
                    {
                        int i = (y * w + x) * 3;
                        if (((rgb[i] << 16) | (rgb[i + 1] << 8) | rgb[i + 2]) == color)
                        {
                            int c = x - x0;
                            colCount[c]++;
                            if (y < colMinY[c]) colMinY[c] = y;
                            if (y > colMaxY[c]) colMaxY[c] = y;
                        }
                    }
                int groupStart = -1, emptyRun = 0;
                for (int c = 0; c <= colCount.Length; c++)
                {
                    bool occupied = c < colCount.Length && colCount[c] >= 4;
                    if (occupied)
                    {
                        if (groupStart < 0) groupStart = c;
                        emptyRun = 0;
                    }
                    else if (groupStart >= 0 && ++emptyRun >= 8)
                    {
                        EvaluateSquareGroup(colCount, colMinY, colMaxY, groupStart, c - emptyRun + 1, squares);
                        groupStart = -1;
                    }
                }
            }
            if (squares.Count < 5)
            {
                whyNot = $"palette predicate degenerate: {squares.Count} square cluster(s) found in the bottom-right region (need >= 5) — either the palette no longer renders or the inventory missed its colors; both must fail loud";
                return false;
            }
            // Median band, not extremes: a same-color trait crossing a square's columns
            // stretches that one cluster's y-extent (measured on the 2026-08 bundle: fr and
            // pt bands pulled down to the trait itself). The median of >= 5 aligned squares
            // absorbs 1-2 contaminated clusters without moving the band.
            int rowMin = MedianOf(squares.Select(s => s.MinY).OrderBy(v => v).ToList());
            int rowMax = MedianOf(squares.Select(s => s.MaxY).OrderBy(v => v).ToList());
            if (rowMax - rowMin > MaxPaletteRowSpanPx)
            {
                whyNot = $"palette clusters span {rowMax - rowMin}px vertically (max one-row span {MaxPaletteRowSpanPx}px) — not a single aligned row, refusing to measure an ambiguous band";
                return false;
            }
            whyNot = "";
            band = (rowMin, rowMax);
            return true;
        }

        private static int MedianOf(IReadOnlyList<int> sorted) => sorted[sorted.Count / 2];

        /// <summary>Evaluates one column group of one color as a square candidate: the
        /// group's y-extent (dense columns only) must be square-ish and inside the side
        /// window — that is a palette square; wide-flat or tall-thin groups are not.</summary>
        private static void EvaluateSquareGroup(int[] colCount, int[] colMinY, int[] colMaxY, int start, int endExclusive, List<(int MinY, int MaxY)> squares)
        {
            int sideX = endExclusive - start;
            if (sideX < MinSquareSidePx || sideX > MaxSquareSidePx) return;
            int minY = int.MaxValue, maxY = 0, count = 0;
            for (int c = start; c < endExclusive; c++)
            {
                if (colCount[c] < 4) continue;
                count += colCount[c];
                if (colMinY[c] < minY) minY = colMinY[c];
                if (colMaxY[c] > maxY) maxY = colMaxY[c];
            }
            int sideY = maxY - minY + 1;
            if (sideY < MinSquareSidePx || sideY > MaxSquareSidePx) return;
            double aspect = (double)sideX / sideY;
            if (aspect < 0.6 || aspect > 1.6 || count < MinSquareSidePx * MinSquareSidePx - MinSquareSidePx) return;
            squares.Add((minY, maxY));
        }

        /// <summary>TRAIT predicate (coverage): pixel rows where a single inventory color
        /// covers at least TraitCoverageThreshold of the card width, grouped into hairline
        /// intervals. Independent of the palette predicate — it never looks at the palette's
        /// position or geometry.</summary>
        private static List<(int MinY, int MaxY)> DetectTraitIntervals(byte[] rgb, int w, int h, List<int> inventory)
        {
            int threshold = (int)(TraitCoverageThreshold * w);
            var isTraitRow = new bool[h];
            for (int y = (int)(h * TraitScanStartYFraction); y < h; y++)
            {
                var counts = new int[inventory.Count];
                for (int x = 0; x < w; x++)
                {
                    int i = (y * w + x) * 3;
                    int key = (rgb[i] << 16) | (rgb[i + 1] << 8) | rgb[i + 2];
                    int c = inventory.IndexOf(key);
                    if (c >= 0) counts[c]++;
                }
                isTraitRow[y] = counts.Max() >= threshold;
            }
            var intervals = new List<(int MinY, int MaxY)>();
            int start = -1;
            for (int y = 0; y < h; y++)
            {
                if (isTraitRow[y] && start < 0) start = y;
                else if (!isTraitRow[y] && start >= 0)
                {
                    if (y - start <= MaxTraitHeightPx) intervals.Add((start, y - 1));
                    start = -1;
                }
            }
            if (start >= 0 && h - start <= MaxTraitHeightPx) intervals.Add((start, h - 1));
            return intervals;
        }

        /// <summary>The page's own color inventory: the dominant saturated exact colors. The
        /// 7 family fills dominate (palette squares + traits + colorBox bars + family text);
        /// variants and darker text tones fill the rest of the window. Near-white/near-black
        /// are excluded regardless of count — background and hairline-dark text must never
        /// enter the inventory.</summary>
        private static List<int> DominantSaturatedColors(byte[] rgb, int w, int h)
        {
            var counts = new Dictionary<int, int>();
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    int i = (y * w + x) * 3;
                    int key = (rgb[i] << 16) | (rgb[i + 1] << 8) | rgb[i + 2];
                    counts[key] = counts.TryGetValue(key, out var c) ? c + 1 : 1;
                }
            return counts
                .Where(kv =>
                {
                    int r = (kv.Key >> 16) & 0xFF, g = (kv.Key >> 8) & 0xFF, b = kv.Key & 0xFF;
                    int max = Math.Max(r, Math.Max(g, b)), min = Math.Min(r, Math.Min(g, b));
                    if (max - min < SaturationSpread) return false;                 // unsaturated: background/photo tones
                    if (min >= 240) return false;                                    // near-white
                    if (max <= 16) return false;                                     // near-black
                    return kv.Value >= 100;                                          // sparse exact shades (photo noise) stay out
                })
                .OrderByDescending(kv => kv.Value)
                .Take(PaletteColorInventorySize)
                .Select(kv => kv.Key)
                .ToList();
        }

        private static byte[] RgbBytesOrThrow(MagickImage image, int w, int h)
        {
            var rgb = image.GetPixels().ToByteArray(PixelMapping.RGB);
            if (rgb == null || rgb.Length < w * h * 3)
                throw new InvalidDataException($"pixel export returned {rgb?.Length ?? 0} bytes for a {w}x{h} RGB raster — the instrument is unusable, failing loud instead of measuring garbage.");
            return rgb;
        }

        private static string FirstLine(string s) => s.Split('\n')[0];

        /// <summary>Same resolution contract as PdfBundleIntegrityTests: the env var when
        /// set (must exist — fail loud, no fallback, no green), else Release-then-Debug
        /// Target, else fail loud for the caller to report.</summary>
        private static string ResolveBundleRootOrFail()
        {
            var envRoot = Environment.GetEnvironmentVariable(BundleRootEnvVar);
            if (!string.IsNullOrWhiteSpace(envRoot))
            {
                if (!Directory.Exists(envRoot))
                    Assert.Fail($"{BundleRootEnvVar} is set to '{envRoot}' which does not exist — an explicitly requested bundle must fail loud, never fall back nor pass green (#1176 no-silent-skip).");
                return envRoot;
            }
            var repoRoot = TestRepoRoot.Find();
            foreach (var build in new[] { "Release", "Debug" })
            {
                var target = Path.Combine(repoRoot, "Generation", "Converters", "Argumentum.AssetConverter", "bin", build, "net9.0-windows", "Target");
                if (Directory.Exists(target)) return target;
            }
            Assert.Fail($"MemoBackPaletteGutterTests require a bundle: set {BundleRootEnvVar} to a bundle root ({{lang}}/Documents/density-0/*.pdf) or run the pipeline for a Target tree — without the artifact this test verified nothing.");
            throw new InvalidOperationException("unreachable");
        }
    }
}
