using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests
{
    /// <summary>
    /// Stage 3 of #212: structural content checks in generated PDFs.
    /// Validates file sizes, page counts, and format consistency across languages.
    /// Text extraction is limited because QuestPDF generates image-based PDFs
    /// (TarotCards/PokerCards) or uses font encodings without ToUnicode mappings
    /// (FallaciesWeb). Structural checks catch the same regressions (empty cards,
    /// broken generation) more reliably.
    /// Fails LOUD if Target/ doesn't exist (#957 residu ii) — was a silent faux-vert (pass-on-nothing).
    /// </summary>
    public class PdfContentTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private static readonly string TargetRoot = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..",
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Debug", "net9.0-windows", "Target"));

        private static readonly string[] Languages = { "fr", "en", "ru", "pt" };

        private const int MinPdfSizeBytes = 10_000; // 10KB — anything smaller is likely broken

        public PdfContentTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Dispose() { }

        private static string GetPdfDir(string lang) =>
            Path.Combine(TargetRoot, lang, "Documents", "density-0");

        private void EnsureTarget()
        {
            // #957 residu ii: cold-start faux-vert. A missing Target/ must FAIL LOUD, not pass silently —
            // these tests verify generated PDFs and assert nothing without them. Assert.Fail is a subtraction
            // (removes the silent `return;`), not a counterweight: no [Fact(Skip)] (static, would kill the
            // tests where they work), no continue-on-error. VisualTests is NOT run by CI (build.yml targets
            // only Argumentum.AssetConverter.Tests.csproj), so this fails locally, not in the release gate.
            if (!Directory.Exists(TargetRoot))
                Assert.Fail("PdfContentTests require a generated Target/ — run the pipeline first (bin/Debug/net9.0-windows/Target not found). This test verified nothing.");
        }

        // --- File size sanity checks ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void All_Pdfs_Have_Minimum_Size(string lang)
        {
            EnsureTarget();
            var dir = GetPdfDir(lang);
            if (!Directory.Exists(dir)) Assert.Fail($"Language directory not found: {dir} — test verified nothing (Target/ exists but this language produced no Documents/density-0/; check pipeline output).");
            var pdfs = Directory.GetFiles(dir, "*.pdf");
            if (pdfs.Length == 0) Assert.Fail($"No PDFs found for {lang} in Target/ — test verified nothing (check pipeline output).");

            var failures = new List<string>();
            foreach (var pdf in pdfs)
            {
                var size = new FileInfo(pdf).Length;
                if (size < MinPdfSizeBytes)
                    failures.Add($"{Path.GetFileName(pdf)}: {size} bytes (min {MinPdfSizeBytes})");
            }

            Assert.Empty(failures);
            _output.WriteLine($"{lang}: {pdfs.Length} PDFs checked, all >= {MinPdfSizeBytes / 1024}KB");
        }

        // --- A0 size consistency across languages ---

        [Fact]
        public void A0_Pdf_Sizes_Are_Consistent_Across_Languages()
        {
            EnsureTarget();

            var sizes = new Dictionary<string, long>();
            foreach (var lang in Languages)
            {
                var pdfs = Directory.GetFiles(GetPdfDir(lang), "*_Fallacies_Web_A0_*.pdf");
                if (pdfs.Length > 0)
                    sizes[lang] = new FileInfo(pdfs[0]).Length;
            }

            if (sizes.Count < 2) Assert.Fail($"Only {sizes.Count} language(s) with A0 PDFs found in Target/ — test verified nothing (expected >= 2 languages to compare; check pipeline output for missing languages).");

            var avgSize = sizes.Values.Average();
            var minSize = sizes.Values.Min();
            var maxSize = sizes.Values.Max();

            // No A0 PDF should be less than 15% of the average (catches empty/broken).
            // Note: RU A0 can be significantly smaller (~20MB vs ~100MB EN) due to font/image differences.
            foreach (var (lang, size) in sizes)
            {
                var ratio = (double)size / avgSize;
                Assert.True(ratio > 0.15,
                    $"{lang} A0 PDF is {size / 1024 / 1024:F1}MB ({ratio:P0} of avg {avgSize / 1024 / 1024:F1}MB)");
            }

            _output.WriteLine($"A0 sizes: {string.Join(", ", sizes.Select(s => $"{s.Key}={s.Value / 1024 / 1024:F1}MB"))}");
        }

        // --- A4 FallaciesWeb page count consistency ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void FallaciesWeb_A4_Has_Reasonable_Page_Count(string lang)
        {
            EnsureTarget();
            var dir = GetPdfDir(lang);
            if (!Directory.Exists(dir)) Assert.Fail($"Language directory not found: {dir} — test verified nothing (Target/ exists but this language produced no Documents/density-0/; check pipeline output).");
            var pdfs = Directory.GetFiles(dir, "*_Fallacies_Web_A4_*.pdf")
                .Where(p => !Path.GetFileName(p).Contains("Thumbnails"))
                .ToArray();
            if (pdfs.Length == 0) Assert.Fail($"No PDFs found for {lang} in Target/ — test verified nothing (check pipeline output).");

            foreach (var pdf in pdfs)
            {
                using var doc = PdfDocument.Open(pdf);
                var pages = doc.NumberOfPages;
                // FallaciesWeb A4 should have multiple pages (15 observed in FR)
                Assert.True(pages >= 5,
                    $"{Path.GetFileName(pdf)}: {pages} pages (expected >= 5)");
                _output.WriteLine($"{Path.GetFileName(pdf)}: {pages} pages");
            }
        }

        // --- TarotCards presence + minimum page count ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void TarotCards_Has_Multiple_Pages(string lang)
        {
            EnsureTarget();
            var dir = GetPdfDir(lang);
            if (!Directory.Exists(dir)) Assert.Fail($"Language directory not found: {dir} — test verified nothing (Target/ exists but this language produced no Documents/density-0/; check pipeline output).");
            // Main TarotCards PDF (not split, not FacesOnly, not Virtues)
            var pdf = Directory.GetFiles(dir, "Argumentum_TarotCards_*.pdf")
                .FirstOrDefault(p => !Path.GetFileName(p).Contains("-")
                    && !Path.GetFileName(p).Contains("Virtues")
                    && !Path.GetFileName(p).Contains("FacesOnly")
                    && !Path.GetFileName(p).Contains("Print"));
            if (pdf == null) Assert.Fail($"No matching PDF found for {lang} in Target/ — test verified nothing (Target/ exists but the expected CardSet PDF is missing; check pipeline output).");

            using var doc = PdfDocument.Open(pdf);
            var pages = doc.NumberOfPages;
            Assert.True(pages >= 100,
                $"{Path.GetFileName(pdf)}: {pages} pages (expected >= 100)");
            _output.WriteLine($"{Path.GetFileName(pdf)}: {pages} pages");
        }

        // --- PokerCards presence + minimum page count ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void PokerCards_Has_Multiple_Pages(string lang)
        {
            EnsureTarget();
            var dir = GetPdfDir(lang);
            if (!Directory.Exists(dir)) Assert.Fail($"Language directory not found: {dir} — test verified nothing (Target/ exists but this language produced no Documents/density-0/; check pipeline output).");
            // Main PokerCards PDF (not split, not Print&Play)
            var pdf = Directory.GetFiles(dir, "Argumentum_PokerCards_*.pdf")
                .FirstOrDefault(p => !Path.GetFileName(p).Contains("-")
                    && !Path.GetFileName(p).Contains("Print"));
            if (pdf == null) Assert.Fail($"No matching PDF found for {lang} in Target/ — test verified nothing (Target/ exists but the expected CardSet PDF is missing; check pipeline output).");

            using var doc = PdfDocument.Open(pdf);
            var pages = doc.NumberOfPages;
            Assert.True(pages >= 50,
                $"{Path.GetFileName(pdf)}: {pages} pages (expected >= 50)");
            _output.WriteLine($"{Path.GetFileName(pdf)}: {pages} pages");
        }

        // --- Total PDF count per language ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void Each_Language_Has_Multiple_Pdf_Types(string lang)
        {
            EnsureTarget();
            var dir = GetPdfDir(lang);
            if (!Directory.Exists(dir)) Assert.Fail($"Language directory not found: {dir} — test verified nothing (Target/ exists but this language produced no Documents/density-0/; check pipeline output).");
            var pdfs = Directory.GetFiles(dir, "*.pdf");

            // Each language should have at least: A0, A4, Thumbnails, Print&Play,
            // TarotCards, PokerCards = 6+ PDFs
            Assert.True(pdfs.Length >= 6,
                $"{lang}: only {pdfs.Length} PDFs (expected >= 6)");
            _output.WriteLine($"{lang}: {pdfs.Length} PDFs found");
        }
    }
}
