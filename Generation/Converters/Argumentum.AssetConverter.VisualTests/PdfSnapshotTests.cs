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
    /// Fails LOUD if Target/ doesn't exist (#957 residu ii) — was a silent faux-vert (pass-on-nothing).
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

        private void EnsureTarget()
        {
            // #957 residu ii: cold-start faux-vert. A missing Target/ must FAIL LOUD, not pass silently —
            // these tests verify generated PDFs and assert nothing without them. Assert.Fail is a subtraction
            // (removes the silent `return;`), not a counterweight: no [Fact(Skip)] (static, would kill the
            // tests where they work), no continue-on-error. VisualTests is NOT run by CI (build.yml targets
            // only Argumentum.AssetConverter.Tests.csproj), so this fails locally, not in the release gate.
            if (!Directory.Exists(TargetRoot))
                Assert.Fail("PdfSnapshotTests require a generated Target/ — run the pipeline first (bin/Debug/net9.0-windows/Target not found). This test verified nothing.");
        }

        // --- FallaciesWeb A4 (all languages) ---

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        public async Task FallaciesWeb_A4_FirstPage_Structure(string lang)
        {
            EnsureTarget();

            var pdfPath = GetPdfPath(lang, $"Argumentum_Fallacies_Web_A4_{lang}.pdf");
            if (!File.Exists(pdfPath)) Assert.Fail($"Expected PDF not found: {pdfPath} — Target/ exists but this file is missing (test verified nothing; check harvest/pipeline output).");

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
            EnsureTarget();

            var pdfPath = GetPdfPath(lang, $"Argumentum_TarotCards_{lang}.pdf");
            if (!File.Exists(pdfPath)) Assert.Fail($"Expected PDF not found: {pdfPath} — Target/ exists but this file is missing (test verified nothing; check harvest/pipeline output).");

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
            EnsureTarget();

            var pdfPath = GetPdfPath(lang, $"Argumentum_PokerCards_{lang}.pdf");
            if (!File.Exists(pdfPath)) Assert.Fail($"Expected PDF not found: {pdfPath} — Target/ exists but this file is missing (test verified nothing; check harvest/pipeline output).");

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
