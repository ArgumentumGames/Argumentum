using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Playwright;
using UglyToad.PdfPig;
using Xunit;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    public class PdfAssemblerTests : IDisposable
    {
        private readonly string _testOutputDir;
        private readonly List<string> _htmlTestFiles;

        public PdfAssemblerTests()
        {
            // 1. Setup des chemins
            _testOutputDir = Path.Combine(Path.GetTempPath(), "ArgumentumTests_PdfAssembly", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testOutputDir);

            var assetsDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Assets", "PdfAssemblyTest"));
            _htmlTestFiles = Directory.GetFiles(assetsDir, "*.html").ToList();
        }

        [Fact] // Timeout augmenté pour la génération de 3 images + 1 PDF
        public async Task AssemblePngsToPdf_WithGeneratedImages_ShouldProduceValidPdf()
        {
            // ARRANGE

            // Guard (#1046 MED #7): HaveCount below is tautological (the generator iterates
            // _htmlTestFiles), so an empty assets folder degrades the whole test to 0 == 0.
            _htmlTestFiles.Should().NotBeEmpty(
                "the PdfAssembly test assets must contain HTML fixtures — otherwise this test degenerates to 0 == 0");

            // 1. Générer les images PNG à partir du HTML
            var generatedPngPaths = await GeneratePngsFromHtmlFiles();
            generatedPngPaths.Should().HaveCount(_htmlTestFiles.Count, "A PNG should be generated for each HTML file.");

            // 2. Préparer les entrées pour l'assembleur PDF
            var outputPdfPath = Path.Combine(_testOutputDir, "assembled.pdf");
            var cardImages = generatedPngPaths.Select(pngPath => new CardImages { Front = pngPath }).ToList();
            
            // Création d'une configuration minimale pour le document
            var docConfig = new CardSetDocumentConfig
            {
                DocumentFormat = CardDocumentFormat.PrintAndPlay,
                PageSize = "A4",
                CardSets = new List<DocumentCardSet>
                {
                    new DocumentCardSet()
                }
            };

            var pdfManager = new PdfManager();

            // ACT
            pdfManager.GeneratePrintAndPlay(outputPdfPath, docConfig, cardImages, true);
            
            // ASSERT
            // a. Le fichier PDF existe et n'est pas vide
            File.Exists(outputPdfPath).Should().BeTrue("The output PDF file must be created.");
            new FileInfo(outputPdfPath).Length.Should().BeGreaterThan(0, "The output PDF file should not be empty.");

            // b. Le nombre de pages correspond au nombre d'images
            using var pdfDocument = PdfDocument.Open(outputPdfPath);
            pdfDocument.NumberOfPages.Should().Be(_htmlTestFiles.Count, "The PDF page count should match the number of input images.");
        }

        private async Task<List<string>> GeneratePngsFromHtmlFiles()
        {
            var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });
            exitCode.Should().Be(0, "Playwright browsers should install correctly.");

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();

            var pngPaths = new List<string>();
            for (int i = 0; i < _htmlTestFiles.Count; i++)
            {
                var htmlPath = _htmlTestFiles[i];
                var pngPath = Path.Combine(_testOutputDir, $"page_{i + 1}.png");
                await page.GotoAsync($"file://{htmlPath}");
                await page.ScreenshotAsync(new PageScreenshotOptions { Path = pngPath, Type = ScreenshotType.Png, FullPage = true });
                pngPaths.Add(pngPath);
            }
            return pngPaths;
        }

        [Fact]
        // Regression guard for #906 (UglyToad.PdfPig 1.7.0-custom-5 → official PdfPig 0.1.14 swap):
        // PdfAuditor's PdfPig read path — PdfDocument.Open → GetPages → GetImages → RawBytes — must
        // still execute against a real, generated PDF on the official package. A compile-only check
        // would miss a runtime API divergence (e.g. GetImages renamed/removed); this generates a real
        // PDF with embedded card images and runs the auditor end-to-end.
        public async Task AuditPdf_WithGeneratedPdf_ShouldReadEmbeddedImagesViaPdfPig()
        {
            // ARRANGE — generate a real PDF with embedded PNG images (same path as the assembly test)
            var generatedPngPaths = await GeneratePngsFromHtmlFiles();
            var outputPdfPath = Path.Combine(_testOutputDir, "audited.pdf");
            var cardImages = generatedPngPaths.Select(p => new CardImages { Front = p }).ToList();

            var docConfig = new CardSetDocumentConfig
            {
                DocumentFormat = CardDocumentFormat.PrintAndPlay,
                PageSize = "A4",
                NbColumns = 3,
                NoBack = true,
                CardSets = new List<DocumentCardSet>
                {
                    new DocumentCardSet
                    {
                        FrontCards = new DocumentCard { WidthMM = 63, HeigthMM = 88 }
                    }
                }
            };

            new PdfManager().GeneratePrintAndPlay(outputPdfPath, docConfig, cardImages, true);
            File.Exists(outputPdfPath).Should().BeTrue("the PDF must exist before auditing");

            // ACT — run PdfAuditor (exercises GetPages/GetImages/RawBytes on official PdfPig 0.1.14)
            var result = global::Argumentum.AssetConverter.PdfAuditor.PdfAuditor.AuditPdf(outputPdfPath, docConfig, cardImages);

            // ASSERT — the PdfPig read path ran without an API-divergence exception, and GetImages
            // returned the embedded card images (so RawBytes hashing was actually exercised). A clean
            // PASS or a count mismatch are both acceptable — both prove images were read; an
            // "unexpected error" or "found 0" would signal PdfPig 0.1.14 divergence and fail the swap.
            var messages = string.Join("\n", result.Messages);
            // Guard (#1046 MED #8): NotContain-only assertions pass vacuously on an empty
            // message list — assert the audit actually produced diagnostics first.
            messages.Should().NotBeEmpty(
                "the audit must produce diagnostics — empty messages would make the NotContain checks below vacuous");
            messages.Should().NotContain("unexpected error occurred during PDF audit",
                "PdfPig 0.1.14 must support the GetImages/RawBytes path PdfAuditor relies on");
            messages.Should().NotContain("found 0 images",
                "PdfPig GetImages must surface the embedded card images so RawBytes is exercised");
        }

        public void Dispose()
        {
            if (Directory.Exists(_testOutputDir))
            {
                Directory.Delete(_testOutputDir, true);
            }
        }
    }
}