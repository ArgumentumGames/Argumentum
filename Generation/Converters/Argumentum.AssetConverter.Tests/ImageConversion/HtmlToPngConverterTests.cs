using Xunit;
using System.IO;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Argumentum.AssetConverter.Tests.ImageConversion
{
    public class HtmlToPngConverterTests : IDisposable
    {
        private readonly string _testOutputDir;

        public HtmlToPngConverterTests()
        {
            _testOutputDir = Path.Combine(Path.GetTempPath(), "ArgumentumTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testOutputDir);
        }

        [Fact(Timeout = 30000)]
        public async Task LocalHtmlFile_ShouldBeConvertedToPng_Successfully()
        {
            // Arrange
            // 1. Ensure Playwright is installed
            var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });
            exitCode.Should().Be(0, "Playwright browsers should install correctly.");

            // 2. Setup Playwright
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            var page = await browser.NewPageAsync();

            // 3. Prepare paths
            var sourceHtmlPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "ImageConversion", "Assets", "sample.html"));
            var outputPngPath = Path.Combine(_testOutputDir, "output.png");

            // Act
            // Navigate to the local HTML file and take a screenshot
            await page.GotoAsync($"file://{sourceHtmlPath}");
            await page.ScreenshotAsync(new PageScreenshotOptions { Path = outputPngPath });

            // Assert
            File.Exists(outputPngPath).Should().BeTrue("The output PNG file should have been created.");
            var fileInfo = new FileInfo(outputPngPath);
            fileInfo.Length.Should().BeGreaterThan(0, "The output PNG file should not be empty.");
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