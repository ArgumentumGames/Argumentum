using System;
using System.IO;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests
{
    /// <summary>
    /// Stage 2 of #212: PDF structural regression via Verify snapshot comparison.
    /// Captures page dimensions, page count, text length, and letter count.
    /// Tests pass silently if Target/ doesn't exist (CI cold-start).
    /// </summary>
    public class PdfSnapshotTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private static readonly string TargetRoot = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..",
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Debug", "net9.0-windows", "Target"));

        public PdfSnapshotTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Dispose() { }

        private static string GetPdfPath(string lang, string filename) =>
            Path.Combine(TargetRoot, lang, "Documents", "density-0", filename);

        private bool EnsureTarget()
        {
            if (!Directory.Exists(TargetRoot))
            {
                _output.WriteLine("Skipped: Target/ not found — run pipeline first");
                return false;
            }
            return true;
        }

        // --- FallaciesWeb A4 (all languages) ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public async Task FallaciesWeb_A4_FirstPage_Structure(string lang)
        {
            if (!EnsureTarget()) return;

            var pdfPath = GetPdfPath(lang, $"Argumentum_Fallacies_Web_A4_{lang}.pdf");
            if (!File.Exists(pdfPath)) { _output.WriteLine($"PDF not found: {pdfPath}"); return; }

            using var doc = PdfDocument.Open(pdfPath);
            var page = doc.GetPage(1);

            var info = new
            {
                File = Path.GetFileName(pdfPath),
                PageWidth = Math.Round(page.Width, 1),
                PageHeight = Math.Round(page.Height, 1),
                PageCount = doc.NumberOfPages,
                TextLength = page.Text?.Length ?? 0,
                TextPreview = (page.Text ?? "").Substring(0, Math.Min(200, (page.Text ?? "").Length)),
                LetterCount = page.Letters.Count
            };

            _output.WriteLine($"Snapshot: {info.File} — {info.PageWidth}x{info.PageHeight}pt, {info.PageCount} pages, {info.LetterCount} letters");

            await Verifier.Verify(info).UseParameters(lang);
        }

        // --- TarotCards (all languages) ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public async Task TarotCards_FirstPage_Structure(string lang)
        {
            if (!EnsureTarget()) return;

            var pdfPath = GetPdfPath(lang, $"Argumentum_TarotCards_{lang}.pdf");
            if (!File.Exists(pdfPath)) { _output.WriteLine($"PDF not found: {pdfPath}"); return; }

            using var doc = PdfDocument.Open(pdfPath);
            var page = doc.GetPage(1);

            var info = new
            {
                File = Path.GetFileName(pdfPath),
                PageWidth = Math.Round(page.Width, 1),
                PageHeight = Math.Round(page.Height, 1),
                PageCount = doc.NumberOfPages,
                LetterCount = page.Letters.Count
            };

            _output.WriteLine($"Snapshot: {info.File} — {info.PageWidth}x{info.PageHeight}pt, {info.PageCount} pages");

            await Verifier.Verify(info).UseParameters(lang);
        }

        // --- PokerCards (all languages) ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public async Task PokerCards_FirstPage_Structure(string lang)
        {
            if (!EnsureTarget()) return;

            var pdfPath = GetPdfPath(lang, $"Argumentum_PokerCards_{lang}.pdf");
            if (!File.Exists(pdfPath)) { _output.WriteLine($"PDF not found: {pdfPath}"); return; }

            using var doc = PdfDocument.Open(pdfPath);
            var page = doc.GetPage(1);

            var info = new
            {
                File = Path.GetFileName(pdfPath),
                PageWidth = Math.Round(page.Width, 1),
                PageHeight = Math.Round(page.Height, 1),
                PageCount = doc.NumberOfPages,
                LetterCount = page.Letters.Count
            };

            _output.WriteLine($"Snapshot: {info.File} — {info.PageWidth}x{info.PageHeight}pt, {info.PageCount} pages");

            await Verifier.Verify(info).UseParameters(lang);
        }
    }
}
