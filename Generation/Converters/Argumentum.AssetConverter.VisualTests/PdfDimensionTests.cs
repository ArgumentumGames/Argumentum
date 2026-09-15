using Argumentum.AssetConverter.Tests;
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
    /// Stage 1 of #212: PDF dimension and page count regression tests.
    /// Validates that generated PDFs have correct page sizes and reasonable page counts.
    /// Fails LOUD if Target/ doesn't exist (#957 residu ii) — was a silent faux-vert (pass-on-nothing).
    /// </summary>
    public class PdfDimensionTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private static readonly string TargetRoot = Path.Combine(
            TestRepoRoot.Find(),
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Debug", "net9.0-windows", "Target");

        private static readonly string[] Languages = { "fr", "en", "ru", "pt" };

        public PdfDimensionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Dispose() { }

        private static string GetPdfDir(string lang) =>
            Path.Combine(TargetRoot, lang, "Documents", "density-0");

        private static IEnumerable<string> GetPdfs(string lang, string pattern)
        {
            var dir = GetPdfDir(lang);
            if (!Directory.Exists(dir)) return Enumerable.Empty<string>();
            return Directory.GetFiles(dir, pattern);
        }

        private static (double width, double height) GetFirstPageSize(string pdfPath)
        {
            using var doc = PdfDocument.Open(pdfPath);
            var page = doc.GetPage(1);
            return (page.Width, page.Height);
        }

        private static int GetPageCount(string pdfPath)
        {
            using var doc = PdfDocument.Open(pdfPath);
            return doc.NumberOfPages;
        }

        private void EnsureTarget()
        {
            // #957 residu ii: cold-start faux-vert. A missing Target/ must FAIL LOUD, not pass silently —
            // these tests verify generated PDFs and assert nothing without them. Assert.Fail is a subtraction
            // (removes the silent `return;`), not a counterweight: no [Fact(Skip)] (static, would kill the
            // tests where they work), no continue-on-error. VisualTests is NOT run by CI (build.yml targets
            // only Argumentum.AssetConverter.Tests.csproj), so this fails locally, not in the release gate.
            if (!Directory.Exists(TargetRoot))
                Assert.Fail("PdfDimensionTests require a generated Target/ — run the pipeline first (bin/Debug/net9.0-windows/Target not found). This test verified nothing.");
        }

        // --- A0 Format Tests ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void A0_Pdf_Has_Correct_Dimensions(string lang)
        {
            EnsureTarget();
            var pdfs = GetPdfs(lang, "*_A0_*.pdf").ToList();
            if (pdfs.Count == 0) Assert.Fail($"No A0 PDFs for {lang} in Target/ — test verified nothing (check pipeline output).");

            foreach (var pdf in pdfs)
            {
                var (w, h) = GetFirstPageSize(pdf);
                _output.WriteLine($"{Path.GetFileName(pdf)}: {w:F0}x{h:F0}pt");
                Assert.InRange(w, 2382, 2386);
                Assert.InRange(h, 3369, 3373);
            }
        }

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void A0_Pdf_Has_Exactly_One_Page(string lang)
        {
            EnsureTarget();
            var pdfs = GetPdfs(lang, "*_A0_*.pdf").ToList();
            if (pdfs.Count == 0) Assert.Fail($"No matching PDFs found for {lang} in Target/ — test verified nothing (Target/ exists but this CardSet pattern produced no files; check pipeline output).");

            foreach (var pdf in pdfs)
                Assert.Equal(1, GetPageCount(pdf));
        }

        // --- A4 Format Tests ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void A4_Pdf_Has_Correct_Dimensions(string lang)
        {
            EnsureTarget();
            var pdfs = GetPdfs(lang, "*_A4_*.pdf").ToList();
            if (pdfs.Count == 0) Assert.Fail($"No A4 PDFs for {lang} in Target/ — test verified nothing (check pipeline output).");

            foreach (var pdf in pdfs)
            {
                var (w, h) = GetFirstPageSize(pdf);
                _output.WriteLine($"{Path.GetFileName(pdf)}: {w:F0}x{h:F0}pt, {GetPageCount(pdf)} pages");
                Assert.InRange(w, 593, 597);
                Assert.InRange(h, 840, 844);
            }
        }

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void A4_Pdf_Has_At_Least_One_Page(string lang)
        {
            EnsureTarget();
            var pdfs = GetPdfs(lang, "*_A4_*.pdf").ToList();
            if (pdfs.Count == 0) Assert.Fail($"No matching PDFs found for {lang} in Target/ — test verified nothing (Target/ exists but this CardSet pattern produced no files; check pipeline output).");

            foreach (var pdf in pdfs)
                Assert.True(GetPageCount(pdf) >= 1, $"{Path.GetFileName(pdf)} has 0 pages");
        }

        // --- Print&Play Format Tests ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void PrintAndPlay_Pdf_Is_A4_Format(string lang)
        {
            EnsureTarget();
            var pdfs = GetPdfs(lang, "*_Print&Play_*.pdf").ToList();
            if (pdfs.Count == 0) Assert.Fail($"No matching PDFs found for {lang} in Target/ — test verified nothing (Target/ exists but this CardSet pattern produced no files; check pipeline output).");

            foreach (var pdf in pdfs)
            {
                var (w, h) = GetFirstPageSize(pdf);
                Assert.InRange(w, 593, 597);
                Assert.InRange(h, 840, 844);
                _output.WriteLine($"{Path.GetFileName(pdf)}: {w:F0}x{h:F0}pt — OK");
            }
        }

        // --- General Integrity Tests ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void All_Pdfs_Have_NonZero_Pages(string lang)
        {
            EnsureTarget();
            var dir = GetPdfDir(lang);
            if (!Directory.Exists(dir)) Assert.Fail($"Language directory not found: {dir} — test verified nothing (Target/ exists but this language produced no Documents/density-0/; check pipeline output).");
            var pdfs = Directory.GetFiles(dir, "*.pdf");
            if (pdfs.Length == 0) Assert.Fail($"No PDFs at all for {lang} in {dir} — test verified nothing.");

            foreach (var pdf in pdfs)
                Assert.True(GetPageCount(pdf) > 0, $"{Path.GetFileName(pdf)} has 0 pages");

            _output.WriteLine($"{lang}: {pdfs.Length} PDFs checked, all have >= 1 page");
        }

        [Fact]
        public void All_Languages_Have_FallaciesWeb_A0()
        {
            EnsureTarget();
            foreach (var lang in Languages)
            {
                var pdfs = GetPdfs(lang, "*_Fallacies_Web_A0_*.pdf").ToList();
                Assert.True(pdfs.Count >= 1, $"No FallaciesWeb A0 PDF for {lang}");
                _output.WriteLine($"{lang}: FallaciesWeb A0 present");
            }
        }

        [Fact]
        public void All_Languages_Have_FallaciesWeb_A4()
        {
            EnsureTarget();
            foreach (var lang in Languages)
            {
                var pdfs = GetPdfs(lang, "*_Fallacies_Web_A4_*.pdf").ToList();
                Assert.True(pdfs.Count >= 1, $"No FallaciesWeb A4 PDF for {lang}");
            }
        }

        [Fact]
        public void All_Languages_Have_TarotCards()
        {
            EnsureTarget();
            foreach (var lang in Languages)
            {
                var pdfs = GetPdfs(lang, "*_TarotCards_*.pdf")
                    .Where(p => !Path.GetFileName(p).Contains("Virtues"))
                    .ToList();
                Assert.True(pdfs.Count >= 1, $"No TarotCards PDF for {lang}");
                _output.WriteLine($"{lang}: {pdfs.Count} TarotCards PDFs");
            }
        }

        [Fact]
        public void All_Languages_Have_PokerCards()
        {
            EnsureTarget();
            foreach (var lang in Languages)
            {
                var pdfs = GetPdfs(lang, "*_PokerCards_*.pdf").ToList();
                Assert.True(pdfs.Count >= 1, $"No PokerCards PDF for {lang}");
                _output.WriteLine($"{lang}: {pdfs.Count} PokerCards PDFs");
            }
        }

        [Fact]
        public void All_Languages_Have_PrintAndPlay()
        {
            EnsureTarget();
            foreach (var lang in Languages)
            {
                var pdfs = GetPdfs(lang, "*_Print&Play_*.pdf").ToList();
                Assert.True(pdfs.Count >= 1, $"No Print&Play PDF for {lang}");
            }
        }
    }
}
