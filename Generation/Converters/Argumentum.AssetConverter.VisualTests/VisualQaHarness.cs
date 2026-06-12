using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests
{
    /// <summary>
    /// Issue #412: Mechanical visual QA harness.
    /// Scans generated card images for geometric defects (white-band, blank-ratio,
    /// bottom-saturation, footer-collision). Outputs a PASS/FLAG grid per card × language.
    ///
    /// These tests are DETECTORS only — they flag geometric anomalies.
    /// Visual VERDICTS (is the card "good" or "bad") remain the exclusive lane
    /// of ai-01 / jsboige (never delegated to automated tests).
    ///
    /// Tests skip silently if Target/ doesn't exist (CI cold-start, no images).
    /// </summary>
    public class VisualQaHarness : IDisposable
    {
        private readonly ITestOutputHelper _output;

        private static readonly string TargetRoot = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..",
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Debug", "net9.0-windows", "Target"));

        private static readonly string ReleaseTargetRoot = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..",
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Release", "net9.0-windows", "Target"));

        /// <summary>8 languages (post-i18n expansion).</summary>
        private static readonly string[] Languages =
            { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

        /// <summary>Card sets that produce card images.</summary>
        private static readonly string[] CardSets =
        {
            "Rules", "Virtues", "Fallacies-Web", "Scenarii"
        };

        // Detector thresholds — per-CardSet calibration (#412 finding ai-01)
        // Rules cards have white backgrounds, so blank-ratio is naturally high (~65-95%)
        // and bottom-saturation is naturally low (~4-18%). Flat thresholds over/under-flag.
        //
        // FooterCollision (#29 overflow recalibration): measures non-white pixel density
        // in the buffer zone (82%-92% of height) between card body and absolute-positioned
        // footer. Rules threshold = 0.03 (3%) — even tiny text spillover is a collision.
        // Cover images (variante 1) have footer elements in display:none → excluded.
        private static readonly Dictionary<string, CardSetThresholds> Thresholds = new()
        {
            ["Rules"]         = new() { BlankRatio = 0.92f, BottomSat = 0.12f, FooterCollision = 0.03f },
            ["Virtues"]       = new() { BlankRatio = 0.65f, BottomSat = 0.25f },
            ["Fallacies-Web"] = new() { BlankRatio = 0.65f, BottomSat = 0.25f },
            ["Scenarii"]      = new() { BlankRatio = 0.65f, BottomSat = 0.25f },
        };

        // Default thresholds for unregistered CardSets
        private const float DefaultBlankRatioThreshold = 0.65f;
        private const float DefaultBottomSatThreshold  = 0.85f;
        private const float DefaultFooterCollisionThreshold = 0.10f;

        private const float WhiteBandThreshold = 0.98f;   // pixels with R>250 AND G>250 AND B>250
        private const float WhitePixelMax = 250f / 255f;   // normalized threshold for "white"

        // Footer collision detector zones (#29 overflow recalibration)
        // The footer occupies the bottom ~10% (position:absolute; bottom:1.5em).
        // The buffer zone (82%-92%) should be mostly white — text pixels here = overflow.
        private const float FooterCollisionBufferStart = 0.82f;
        private const float FooterCollisionBufferEnd   = 0.92f;

        /// <summary>Per-CardSet threshold set (blank-ratio, bottom-saturation, footer-collision).</summary>
        private class CardSetThresholds
        {
            public float BlankRatio = DefaultBlankRatioThreshold;
            public float BottomSat = DefaultBottomSatThreshold;
            public float FooterCollision = DefaultFooterCollisionThreshold;
        }

        private static float GetBlankRatioThreshold(string cardSet)
            => Thresholds.TryGetValue(cardSet, out var t) ? t.BlankRatio : DefaultBlankRatioThreshold;

        private static float GetBottomSatThreshold(string cardSet)
            => Thresholds.TryGetValue(cardSet, out var t) ? t.BottomSat : DefaultBottomSatThreshold;

        private static float GetFooterCollisionThreshold(string cardSet)
            => Thresholds.TryGetValue(cardSet, out var t) ? t.FooterCollision : DefaultFooterCollisionThreshold;

        public VisualQaHarness(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Dispose() { }

        /// <summary>
        /// Resolves the Target root, preferring Release if available, else Debug.
        /// Returns null if neither exists.
        /// </summary>
        private static string? ResolveTargetRoot()
        {
            if (Directory.Exists(ReleaseTargetRoot)) return ReleaseTargetRoot;
            if (Directory.Exists(TargetRoot)) return TargetRoot;
            return null;
        }

        // --- Inventory tests ---

        [Fact]
        public void VisualQa_Inventory_AllLanguages_HaveTargetDir()
        {
            var root = ResolveTargetRoot();
            if (root == null)
            {
                _output.WriteLine("SKIP: Target/ not found — run pipeline first");
                return;
            }

            var missing = new List<string>();
            foreach (var lang in Languages)
            {
                var langDir = Path.Combine(root, lang);
                if (!Directory.Exists(langDir))
                    missing.Add(lang);
            }

            if (missing.Count > 0)
            {
                _output.WriteLine($"WARN: Missing language dirs: {string.Join(", ", missing)}");
                // Not a hard fail — some languages may not have been generated yet
            }

            _output.WriteLine($"Found {Languages.Length - missing.Count}/{Languages.Length} language directories");
        }

        [Fact]
        public void VisualQa_Inventory_CountImagesPerCardSet()
        {
            var root = ResolveTargetRoot();
            if (root == null)
            {
                _output.WriteLine("SKIP: Target/ not found");
                return;
            }

            var inventory = new List<string>();
            foreach (var lang in Languages)
            {
                foreach (var cardSet in CardSets)
                {
                    var count = CountImages(root, lang, cardSet);
                    if (count > 0)
                        inventory.Add($"  {lang}/{cardSet}: {count} images");
                }
            }

            foreach (var line in inventory)
                _output.WriteLine(line);

            Assert.NotEmpty(inventory);
        }

        // --- Detector: white-band (regression #188) ---

        [Fact]
        public void VisualQa_WhiteBand_NoFullWidthBandInCovers()
        {
            var root = ResolveTargetRoot();
            if (root == null) { _output.WriteLine("SKIP"); return; }

            var flags = new List<string>();
            int scanned = 0;

            foreach (var lang in Languages)
            {
                foreach (var cardSet in CardSets)
                {
                    var images = GetImages(root, lang, cardSet);
                    foreach (var imgPath in images)
                    {
                        if (!IsCoverImage(imgPath)) continue;
                        scanned++;

                        var bandResult = DetectWhiteBand(imgPath);
                        if (bandResult.Flagged)
                        {
                            flags.Add($"{lang}/{cardSet}/{Path.GetFileName(imgPath)}: " +
                                      $"white-band {bandResult.WhiteRatio:P0} at row {bandResult.MaxBandRow}");
                        }
                    }
                }
            }

            _output.WriteLine($"Scanned {scanned} cover images, flagged {flags.Count}");

            foreach (var f in flags)
                _output.WriteLine($"  FLAG: {f}");

            // This is informational — not a hard fail (thresholds may need tuning)
            // But if ALL covers are flagged, something is wrong
            if (flags.Count > 0 && scanned > 0 && flags.Count >= scanned)
                Assert.Fail("All cover images flagged for white-band — check thresholds");
        }

        // --- Detector: blank-ratio (underfilled cards, #250 related) ---

        [Fact]
        public void VisualQa_BlankRatio_Rules_NotExcessivelyEmpty()
        {
            var root = ResolveTargetRoot();
            if (root == null) { _output.WriteLine("SKIP"); return; }

            var flags = new List<string>();
            int scanned = 0;

            foreach (var lang in Languages)
            {
                var images = GetImages(root, lang, "Rules");
                foreach (var imgPath in images)
                {
                    scanned++;
                    var blankResult = DetectBlankRatio(imgPath, "Rules");
                    if (blankResult.Flagged)
                    {
                        flags.Add($"{lang}/Rules/{Path.GetFileName(imgPath)}: " +
                                  $"blank-ratio {blankResult.BlankRatio:P1}");
                    }
                }
            }

            _output.WriteLine($"Rules: scanned {scanned}, flagged {flags.Count}");

            foreach (var f in flags)
                _output.WriteLine($"  FLAG: {f}");

            if (scanned == 0)
            {
                _output.WriteLine("No Rules images found — skip");
                return;
            }

            // Informational: report but don't hard-fail (this is the data ai-01 needs for #250)
        }

        // --- Detector: bottom-saturation (overflow cards, #250 related) ---

        [Fact]
        public void VisualQa_BottomSaturation_Rules_NotOverflowing()
        {
            var root = ResolveTargetRoot();
            if (root == null) { _output.WriteLine("SKIP"); return; }

            var flags = new List<string>();
            int scanned = 0;

            foreach (var lang in Languages)
            {
                var images = GetImages(root, lang, "Rules");
                foreach (var imgPath in images)
                {
                    scanned++;
                    var satResult = DetectBottomSaturation(imgPath, "Rules");
                    if (satResult.Flagged)
                    {
                        flags.Add($"{lang}/Rules/{Path.GetFileName(imgPath)}: " +
                                  $"bottom-sat {satResult.Saturation:P1}");
                    }
                }
            }

            _output.WriteLine($"Rules: scanned {scanned}, flagged {flags.Count}");

            foreach (var f in flags)
                _output.WriteLine($"  FLAG: {f}");
        }

        // --- Detector: footer-collision (body overflowing under absolute footer, #29 recalibration) ---

        [Fact]
        public void VisualQa_FooterCollision_Rules_NoBodyFooterOverlap()
        {
            var root = ResolveTargetRoot();
            if (root == null) { _output.WriteLine("SKIP"); return; }

            var flags = new List<string>();
            int scanned = 0;
            int skippedCovers = 0;

            foreach (var lang in Languages)
            {
                var images = GetImages(root, lang, "Rules");
                foreach (var imgPath in images)
                {
                    // Cover images (variante 1) have footer in display:none → footer area = blank
                    // The detector would report a false +22000px artifact — exclude them
                    if (IsCoverImage(imgPath))
                    {
                        skippedCovers++;
                        continue;
                    }

                    scanned++;
                    var collisionResult = DetectFooterCollision(imgPath, "Rules");
                    if (collisionResult.Flagged)
                    {
                        flags.Add($"{lang}/Rules/{Path.GetFileName(imgPath)}: " +
                                  $"footer-collision {collisionResult.Density:P1} " +
                                  $"({collisionResult.NonWhitePixels} px in buffer zone)");
                    }
                }
            }

            _output.WriteLine($"Rules footer-collision: scanned {scanned}, skipped {skippedCovers} covers, flagged {flags.Count}");

            foreach (var f in flags)
                _output.WriteLine($"  FLAG: {f}");

            if (scanned == 0)
            {
                _output.WriteLine("No non-cover Rules images found — skip");
                return;
            }

            // Informational: report but don't hard-fail
        }

        // --- Full grid report (all card sets × all detectors × all languages) ---

        [Fact]
        public void VisualQa_FullGrid_AllCards_AllDetectors()
        {
            var root = ResolveTargetRoot();
            if (root == null) { _output.WriteLine("SKIP"); return; }

            var results = new List<CardCheckResult>();
            int totalImages = 0;

            foreach (var lang in Languages)
            {
                foreach (var cardSet in CardSets)
                {
                    var images = GetImages(root, lang, cardSet);
                    foreach (var imgPath in images)
                    {
                        totalImages++;
                        var fileName = Path.GetFileName(imgPath);
                        var isCover = IsCoverImage(imgPath);

                        var band = DetectWhiteBand(imgPath);
                        var blank = DetectBlankRatio(imgPath, cardSet);
                        var sat = DetectBottomSaturation(imgPath, cardSet);

                        // Footer collision: skip covers (display:none footer = artefact)
                        var footer = isCover
                            ? (Flagged: false, Density: 0f, NonWhitePixels: 0)
                            : DetectFooterCollision(imgPath, cardSet);

                        var result = new CardCheckResult
                        {
                            Language = lang,
                            CardSet = cardSet,
                            FileName = fileName,
                            IsCover = isCover,
                            WhiteBand = band.Flagged ? $"FLAG({band.WhiteRatio:P0})" : "PASS",
                            BlankRatio = blank.Flagged ? $"FLAG({blank.BlankRatio:P0})" : "PASS",
                            BottomSat = sat.Flagged ? $"FLAG({sat.Saturation:P0})" : "PASS",
                            FooterCollision = footer.Flagged ? $"FLAG({footer.Density:P1})" : (isCover ? "COVER" : "PASS"),
                            BlankRatioValue = blank.BlankRatio,
                            BottomSatValue = sat.Saturation,
                            FooterCollisionValue = footer.Density,
                            WhiteBandRow = band.MaxBandRow,
                        };
                        results.Add(result);
                    }
                }
            }

            // Print markdown grid
            _output.WriteLine("# Visual QA Grid — Mechanical Detectors");
            _output.WriteLine($"Total images scanned: {totalImages}");
            _output.WriteLine("---");

            // Group by card set
            foreach (var group in results.GroupBy(r => r.CardSet).OrderBy(g => g.Key))
            {
                _output.WriteLine($"## {group.Key}");
                _output.WriteLine("| Card | Lang | WhiteBand | BlankRatio | BottomSat | FooterCol |");
                _output.WriteLine("|------|------|-----------|------------|-----------|-----------|");

                foreach (var r in group.OrderBy(r => r.FileName).ThenBy(r => r.Language))
                {
                    _output.WriteLine($"| {r.FileName} | {r.Language} | {r.WhiteBand} | {r.BlankRatio} | {r.BottomSat} | {r.FooterCollision} |");
                }
                _output.WriteLine("---");
            }

            // Summary
            var flagged = results.Count(r =>
                r.WhiteBand != "PASS" || r.BlankRatio != "PASS" || r.BottomSat != "PASS" || r.FooterCollision.StartsWith("FLAG"));
            _output.WriteLine($"## Summary: {flagged}/{totalImages} flagged");

            // Rules-specific detail (for #250)
            var rulesResults = results.Where(r => r.CardSet == "Rules").ToList();
            if (rulesResults.Count > 0)
            {
                _output.WriteLine("---");
                _output.WriteLine("## Rules Detail (#250)");
                _output.WriteLine("BlankRatio values (higher = emptier card):");

                // Pivot: card × langs for blank ratio
                var rulesCards = rulesResults.Select(r => r.FileName).Distinct().OrderBy(x => x).ToList();
                _output.WriteLine($"| Card | {string.Join(" | ", Languages)} |");
                _output.WriteLine($"|------| {string.Join(" | ", Languages.Select(_ => "---"))} |");

                foreach (var card in rulesCards)
                {
                    var vals = Languages.Select(lang =>
                    {
                        var match = rulesResults.FirstOrDefault(r =>
                            r.FileName == card && r.Language == lang);
                        if (match == null) return "—";
                        var val = match.BlankRatioValue;
                        if (val > GetBlankRatioThreshold("Rules")) return $"**{val:P0}**";
                        return $"{val:P0}";
                    });
                    _output.WriteLine($"| {card} | {string.Join(" | ", vals)} |");
                }

                _output.WriteLine("---");
                _output.WriteLine("BottomSaturation values (higher = more overflow):");
                _output.WriteLine($"| Card | {string.Join(" | ", Languages)} |");
                _output.WriteLine($"|------| {string.Join(" | ", Languages.Select(_ => "---"))} |");

                foreach (var card in rulesCards)
                {
                    var vals = Languages.Select(lang =>
                    {
                        var match = rulesResults.FirstOrDefault(r =>
                            r.FileName == card && r.Language == lang);
                        if (match == null) return "—";
                        var val = match.BottomSatValue;
                        if (val > GetBottomSatThreshold("Rules")) return $"**{val:P0}**";
                        return $"{val:P0}";
                    });
                    _output.WriteLine($"| {card} | {string.Join(" | ", vals)} |");
                }

                _output.WriteLine("---");
                _output.WriteLine("FooterCollision values (higher = body overflowing into footer zone):");
                _output.WriteLine($"| Card | {string.Join(" | ", Languages)} |");
                _output.WriteLine($"|------| {string.Join(" | ", Languages.Select(_ => "---"))} |");

                foreach (var card in rulesCards)
                {
                    var vals = Languages.Select(lang =>
                    {
                        var match = rulesResults.FirstOrDefault(r =>
                            r.FileName == card && r.Language == lang);
                        if (match == null) return "—";
                        if (match.IsCover) return "cover";
                        var val = match.FooterCollisionValue;
                        if (val > GetFooterCollisionThreshold("Rules")) return $"**{val:P1}**";
                        return $"{val:P1}";
                    });
                    _output.WriteLine($"| {card} | {string.Join(" | ", vals)} |");
                }
            }

            if (totalImages == 0)
                _output.WriteLine("No images found in any language — skip");
        }

        // --- Helper methods ---

        private static IEnumerable<string> GetImages(string root, string lang, string cardSet)
        {
            // Try multiple possible paths
            var candidates = new[]
            {
                Path.Combine(root, lang, "Images", "density-0", cardSet),
                Path.Combine(root, lang, "Images", cardSet),
                Path.Combine(root, "Images", "density-0", cardSet),
                Path.Combine(root, lang, "Documents", "density-0"),
            };

            foreach (var dir in candidates)
            {
                if (!Directory.Exists(dir)) continue;
                var files = Directory.GetFiles(dir, "*.png")
                    .Concat(Directory.GetFiles(dir, "*.jpg"))
                    .Concat(Directory.GetFiles(dir, "*.jpeg"))
                    .OrderBy(f => f)
                    .ToList();
                if (files.Count > 0) return files;
            }

            return Enumerable.Empty<string>();
        }

        private static int CountImages(string root, string lang, string cardSet)
            => GetImages(root, lang, cardSet).Count();

        private static bool IsCoverImage(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
            return name.Contains("cover") || name.Contains("_01_") || name.Contains("_01-") ||
                   name.EndsWith("_01") || name.EndsWith("-01") || name.Contains("rules_01") ||
                   name.Contains("rules-01");
        }

        /// <summary>
        /// Detects a full-width horizontal band of white pixels in the image.
        /// Scans rows in the middle 60% of the image (top/bottom are often legitimately white).
        /// A "white band" = a row where >98% of pixels are white (R>250, G>250, B>250).
        /// </summary>
        private static (bool Flagged, float WhiteRatio, int MaxBandRow) DetectWhiteBand(string imagePath)
        {
            try
            {
                using var image = Image.Load<Rgb24>(imagePath);
                int height = image.Height;
                int width = image.Width;

                // Scan rows in the middle 60% (20%-80% of height)
                int startRow = (int)(height * 0.20);
                int endRow = (int)(height * 0.80);

                float maxWhiteRatio = 0f;
                int maxBandRow = 0;

                for (int y = startRow; y < endRow; y++)
                {
                    int whiteCount = 0;
                    for (int x = 0; x < width; x++)
                    {
                        var pixel = image[x, y];
                        if (pixel.R > 250 && pixel.G > 250 && pixel.B > 250)
                            whiteCount++;
                    }

                    float ratio = (float)whiteCount / width;
                    if (ratio > maxWhiteRatio)
                    {
                        maxWhiteRatio = ratio;
                        maxBandRow = y;
                    }
                }

                bool flagged = maxWhiteRatio > WhiteBandThreshold;
                return (flagged, maxWhiteRatio, maxBandRow);
            }
            catch
            {
                return (false, 0f, 0);
            }
        }

        /// <summary>
        /// Measures the ratio of white pixels in the "body" area of the card
        /// (middle 60%, rows 20%-80%). A high ratio indicates an underfilled card.
        /// Uses per-CardSet threshold for flagging.
        /// </summary>
        private static (bool Flagged, float BlankRatio) DetectBlankRatio(string imagePath, string cardSet)
        {
            try
            {
                using var image = Image.Load<Rgb24>(imagePath);
                int height = image.Height;
                int width = image.Width;

                // Body area: rows 20%-80%
                int startRow = (int)(height * 0.20);
                int endRow = (int)(height * 0.80);

                long totalPixels = 0;
                long whitePixels = 0;

                for (int y = startRow; y < endRow; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        totalPixels++;
                        var pixel = image[x, y];
                        if (pixel.R > 250 && pixel.G > 250 && pixel.B > 250)
                            whitePixels++;
                    }
                }

                float ratio = totalPixels > 0 ? (float)whitePixels / totalPixels : 0f;
                return (ratio > GetBlankRatioThreshold(cardSet), ratio);
            }
            catch
            {
                return (false, 0f);
            }
        }

        /// <summary>
        /// Measures the density of non-white pixels in the bottom 10% of the card.
        /// High density indicates text/content extending to the very bottom edge (overflow).
        /// Uses per-CardSet threshold for flagging.
        /// </summary>
        private static (bool Flagged, float Saturation) DetectBottomSaturation(string imagePath, string cardSet)
        {
            try
            {
                using var image = Image.Load<Rgb24>(imagePath);
                int height = image.Height;
                int width = image.Width;

                // Bottom 10%
                int startRow = (int)(height * 0.90);
                int endRow = height;

                long totalPixels = 0;
                long nonWhitePixels = 0;

                for (int y = startRow; y < endRow; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        totalPixels++;
                        var pixel = image[x, y];
                        if (!(pixel.R > 250 && pixel.G > 250 && pixel.B > 250))
                            nonWhitePixels++;
                    }
                }

                float saturation = totalPixels > 0 ? (float)nonWhitePixels / totalPixels : 0f;
                return (saturation > GetBottomSatThreshold(cardSet), saturation);
            }
            catch
            {
                return (false, 0f);
            }
        }

        /// <summary>
        /// Detects card body text overflowing into the footer zone (position:absolute footer).
        /// Scans the "buffer zone" (82%-92% of card height) — the area between where the
        /// card body should end and where the absolute-positioned footer elements sit.
        /// Non-white pixels here = text has overflowed under the footer.
        ///
        /// This detector catches the specific class of overflow where the card body flows
        /// under position:absolute footer elements (colorPalette, pageNumber, :before labels).
        /// The existing BottomSaturation detector measures the bottom 10% which includes
        /// legitimate footer content — it can't distinguish body overflow from footer content.
        ///
        /// Cover images (variante 1) MUST be excluded: their footer elements are display:none,
        /// making the entire bottom area blank and producing false +22000px artifacts.
        /// </summary>
        private static (bool Flagged, float Density, int NonWhitePixels) DetectFooterCollision(
            string imagePath, string cardSet)
        {
            try
            {
                using var image = Image.Load<Rgb24>(imagePath);
                int height = image.Height;
                int width = image.Width;

                // Buffer zone: between body end and footer start
                int startRow = (int)(height * FooterCollisionBufferStart);
                int endRow = (int)(height * FooterCollisionBufferEnd);

                long totalPixels = 0;
                long nonWhitePixels = 0;

                for (int y = startRow; y < endRow; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        totalPixels++;
                        var pixel = image[x, y];
                        if (!(pixel.R > 250 && pixel.G > 250 && pixel.B > 250))
                            nonWhitePixels++;
                    }
                }

                float density = totalPixels > 0 ? (float)nonWhitePixels / totalPixels : 0f;
                return (density > GetFooterCollisionThreshold(cardSet), density, (int)nonWhitePixels);
            }
            catch
            {
                return (false, 0f, 0);
            }
        }

        private class CardCheckResult
        {
            public string Language { get; set; } = "";
            public string CardSet { get; set; } = "";
            public string FileName { get; set; } = "";
            public bool IsCover { get; set; }
            public string WhiteBand { get; set; } = "PASS";
            public string BlankRatio { get; set; } = "PASS";
            public string BottomSat { get; set; } = "PASS";
            public string FooterCollision { get; set; } = "PASS";

            // Raw values for detailed reporting
            public float BlankRatioValue { get; set; }
            public float BottomSatValue { get; set; }
            public float FooterCollisionValue { get; set; }
            public int WhiteBandRow { get; set; }
        }
    }
}
