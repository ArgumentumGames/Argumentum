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

        [Fact(Timeout = 60000)] // Timeout augmenté pour la génération de 3 images + 1 PDF
        public async Task AssemblePngsToPdf_WithGeneratedImages_ShouldProduceValidPdf()
        {
            // ARRANGE

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

        public void Dispose()
        {
            if (Directory.Exists(_testOutputDir))
            {
                Directory.Delete(_testOutputDir, true);
            }
        }
    }
}