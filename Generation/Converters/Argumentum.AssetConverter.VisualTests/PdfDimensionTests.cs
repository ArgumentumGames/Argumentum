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
    /// Tests pass silently if Target/ doesn't exist (CI cold-start).
    /// </summary>
    public class PdfDimensionTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private static readonly string TargetRoot = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..",
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Debug", "net9.0-windows", "Target"));

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

        private bool EnsureTarget()
        {
            if (!Directory.Exists(TargetRoot))
            {
                _output.WriteLine("Skipped: Target/ not found — run pipeline first");
                return false;
            }
            return true;
        }

        // --- A0 Format Tests ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public void A0_Pdf_Has_Correct_Dimensions(string lang)
        {
            if (!EnsureTarget()) return;
            var pdfs = GetPdfs(lang, "*_A0_*.pdf").ToList();
            if (pdfs.Count == 0) { _output.WriteLine($"No A0 PDFs for {lang}"); return; }

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
            if (!EnsureTarget()) return;
            var pdfs = GetPdfs(lang, "*_A0_*.pdf").ToList();
            if (pdfs.Count == 0) return;

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
            if (!EnsureTarget()) return;
            var pdfs = GetPdfs(lang, "*_A4_*.pdf").ToList();
            if (pdfs.Count == 0) { _output.WriteLine($"No A4 PDFs for {lang}"); return; }

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
            if (!EnsureTarget()) return;
            var pdfs = GetPdfs(lang, "*_A4_*.pdf").ToList();
            if (pdfs.Count == 0) return;

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
            if (!EnsureTarget()) return;
            var pdfs = GetPdfs(lang, "*_Print&Play_*.pdf").ToList();
            if (pdfs.Count == 0) return;

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
            if (!EnsureTarget()) return;
            var dir = GetPdfDir(lang);
            if (!Directory.Exists(dir)) return;
            var pdfs = Directory.GetFiles(dir, "*.pdf");
            if (pdfs.Length == 0) return;

            foreach (var pdf in pdfs)
                Assert.True(GetPageCount(pdf) > 0, $"{Path.GetFileName(pdf)} has 0 pages");

            _output.WriteLine($"{lang}: {pdfs.Length} PDFs checked, all have >= 1 page");
        }

        [Fact]
        public void All_Languages_Have_FallaciesWeb_A0()
        {
            if (!EnsureTarget()) return;
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
            if (!EnsureTarget()) return;
            foreach (var lang in Languages)
            {
                var pdfs = GetPdfs(lang, "*_Fallacies_Web_A4_*.pdf").ToList();
                Assert.True(pdfs.Count >= 1, $"No FallaciesWeb A4 PDF for {lang}");
            }
        }

        [Fact]
        public void All_Languages_Have_TarotCards()
        {
            if (!EnsureTarget()) return;
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
            if (!EnsureTarget()) return;
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
            if (!EnsureTarget()) return;
            foreach (var lang in Languages)
            {
                var pdfs = GetPdfs(lang, "*_Print&Play_*.pdf").ToList();
                Assert.True(pdfs.Count >= 1, $"No Print&Play PDF for {lang}");
            }
        }
    }
}
