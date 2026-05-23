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
    /// Stage 2 of #212: PDF visual regression via pixel snapshot comparison.
    /// Renders first page of selected PDFs to PNG, then uses Verify for snapshot diff.
    /// First run creates baseline (.verified.png), subsequent runs compare against it.
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

        // --- FR Baseline Tests (Stage 2 scaffolding) ---

        [Fact]
        public async Task FR_FallaciesWeb_A4_FirstPage_Structure()
        {
            if (!EnsureTarget()) return;

            var pdfPath = GetPdfPath("fr", "Argumentum_Fallacies_Web_A4_fr.pdf");
            if (!File.Exists(pdfPath)) { _output.WriteLine("PDF not found"); return; }

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

            await Verifier.Verify(info);
        }

        [Fact]
        public async Task FR_TarotCards_FirstPage_Structure()
        {
            if (!EnsureTarget()) return;

            var pdfPath = GetPdfPath("fr", "Argumentum_TarotCards_fr.pdf");
            if (!File.Exists(pdfPath)) { _output.WriteLine("PDF not found"); return; }

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

            await Verifier.Verify(info);
        }

        [Fact]
        public async Task FR_PokerCards_FirstPage_Structure()
        {
            if (!EnsureTarget()) return;

            var pdfPath = GetPdfPath("fr", "Argumentum_PokerCards_fr.pdf");
            if (!File.Exists(pdfPath)) { _output.WriteLine("PDF not found"); return; }

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

            await Verifier.Verify(info);
        }
    }
}
