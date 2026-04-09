using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Mindmapper;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests
{
    /// <summary>
    /// Playwright integration tests for mind map HTML wrapper rendering.
    /// Issue #196: proves that the wrappers generated from the committed templates +
    /// committed Batik SVGs actually render in a real browser, nodes are clickable,
    /// and the overlay card reacts to clicks. Every assertion corresponds to something
    /// the user would spot visually when opening the file.
    ///
    /// These tests DO NOT require a full pipeline run — they compose the wrapper in a
    /// temp directory using the same <see cref="MindMapHtmlWrapper.FormatWrapper"/> path
    /// the pipeline uses, so a regression in the helper will surface here.
    /// </summary>
    public class MindmapWrapperTests : IAsyncLifetime
    {
        private readonly ITestOutputHelper _output;
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;
        private string _tempDir = null!;

        private static readonly string RepoRoot =
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".."));

        private static readonly string IncludedTemplatePath =
            Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", "included.html");

        private static readonly string ExternalTemplatePath =
            Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", "external.html");

        public MindmapWrapperTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            _tempDir = Path.Combine(Path.GetTempPath(), "argumentum-wrapper-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public async Task DisposeAsync()
        {
            if (_browser != null) await _browser.CloseAsync();
            _playwright?.Dispose();
            try { if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true); }
            catch { /* best-effort temp cleanup */ }
        }

        private static string GetSvgPath(string lang, string fileName)
            => Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", lang, fileName);

        /// <summary>
        /// Inline-SVG variant driven by the real committed Batik SVGs. Verifies the embedded
        /// <c>&lt;svg&gt;</c> is present in the DOM and carries non-empty geometric content
        /// (groups, paths, text). Does NOT assert on <c>.node</c> markers — the current Batik
        /// SVGs are visual-only and intentionally carry no semantic node class (a tracked gap,
        /// not a regression). The interactive overlay path is covered in a separate test below
        /// with a synthetic SVG that mimics the expected marker convention.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        [InlineData("en", "Fallacies_en.content.svg")]
        [InlineData("ru", "Fallacies_ru.content.svg")]
        [InlineData("pt", "Fallacies_pt.content.svg")]
        public async Task Included_Wrapper_Renders_Inline_Svg_With_Content(string lang, string svgFileName)
        {
            var svgPath = GetSvgPath(lang, svgFileName);
            Assert.True(File.Exists(IncludedTemplatePath), $"Missing template: {IncludedTemplatePath}");
            Assert.True(File.Exists(svgPath), $"Missing SVG fixture: {svgPath}");

            var template = await File.ReadAllTextAsync(IncludedTemplatePath);
            var svg = await File.ReadAllTextAsync(svgPath);

            var wrapperHtml = MindMapHtmlWrapper.FormatWrapper(
                template,
                svgRelativePath: svgFileName,
                svgContent: svg);

            var wrapperPath = Path.Combine(_tempDir, $"included_{lang}.html");
            await File.WriteAllTextAsync(wrapperPath, wrapperHtml);

            // Hard guarantee the regression cannot ship silently.
            Assert.DoesNotContain("[SVGCONTENT]", wrapperHtml);
            Assert.DoesNotContain("[SVGPATH]", wrapperHtml);

            var page = await _browser.NewPageAsync();
            await page.GotoAsync("file:///" + wrapperPath.Replace('\\', '/'));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            var svgCount = await page.Locator("#mindmap svg").CountAsync();
            Assert.True(svgCount >= 1, $"Expected an inline <svg> under #mindmap for {lang}, got {svgCount}");

            // Any geometric child demonstrates the SVG was actually parsed and embedded.
            var pathCount = await page.Locator("#mindmap svg path").CountAsync();
            _output.WriteLine($"[{lang}] inline SVG path elements: {pathCount}");
            Assert.True(pathCount > 0, $"Expected <path> elements inside the inline SVG for {lang}");

            await page.CloseAsync();
        }

        /// <summary>
        /// Interactive overlay path, driven with a synthetic SVG fixture that carries the
        /// <c>class="node"</c> markers the overlay JS expects. This is the regression harness
        /// for the interactive layer — if <c>included.html</c>'s JS ever breaks, this test fails
        /// even before we commit to a particular SVG generator.
        /// </summary>
        [Fact]
        public async Task Included_Wrapper_Click_Node_Shows_Overlay_Card()
        {
            Assert.True(File.Exists(IncludedTemplatePath), $"Missing template: {IncludedTemplatePath}");

            var template = await File.ReadAllTextAsync(IncludedTemplatePath);

            // Synthetic SVG with the attributes the overlay JS reads on click:
            // family / subfamily / subsubfamily / description / example / link / depth / familyclass.
            const string syntheticSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""400"" height=""200"">
  <g class=""node"" family=""Insuffisance"" subfamily=""Argument bâclé"" subsubfamily=""Justification triviale""
     description=""Test description"" example=""Test example"" link=""https://example.org/test""
     depth=""3"" familyclass=""insuffisance"">
    <rect x=""10"" y=""10"" width=""380"" height=""180"" fill=""#f0f0f0""/>
    <text x=""200"" y=""100"" text-anchor=""middle"">Synthetic node</text>
  </g>
</svg>";

            var wrapperHtml = MindMapHtmlWrapper.FormatWrapper(template, "synthetic.svg", syntheticSvg);
            var wrapperPath = Path.Combine(_tempDir, "included_synthetic_click.html");
            await File.WriteAllTextAsync(wrapperPath, wrapperHtml);

            var page = await _browser.NewPageAsync();
            await page.GotoAsync("file:///" + wrapperPath.Replace('\\', '/'));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            // The overlay JS only wires up click handlers on page load. We confirm the node is
            // present, click it, and verify the overlay reacts.
            var nodeLocator = page.Locator("#mindmap svg .node");
            Assert.Equal(1, await nodeLocator.CountAsync());

            await nodeLocator.ClickAsync();

            // After the click, the JS copies the node's 'family' attribute into .famille,
            // which is the most observable user-facing change. We read textContent (not
            // innerText) because the template applies `text-transform: uppercase` to
            // `.famille`, and innerText would return the visually-uppercased string.
            var familleText = await page.Locator("card .famille").EvaluateAsync<string>("el => el.textContent");
            _output.WriteLine($"Overlay famille textContent after click: '{familleText}'");
            Assert.Equal("Insuffisance", familleText?.Trim());

            // And the card loses its 'hidden' class (it starts hidden after the JS adds it).
            var cardHasHidden = await page.Locator("card").EvaluateAsync<bool>("el => el.classList.contains('hidden')");
            Assert.False(cardHasHidden, "The overlay card should become visible after clicking a .node");

            await page.CloseAsync();
        }

        /// <summary>
        /// External variant: the wrapper references the SVG via <c>&lt;object data&gt;</c>. Verifies
        /// the object tag carries the correct relative path (no literal placeholder) and that
        /// the referenced file exists alongside the wrapper in the expected layout.
        /// </summary>
        [Theory]
        [InlineData("fr", "Fallacies_fr.content.svg")]
        [InlineData("en", "Fallacies_en.content.svg")]
        [InlineData("ru", "Fallacies_ru.content.svg")]
        [InlineData("pt", "Fallacies_pt.content.svg")]
        public async Task External_Wrapper_References_Svg_Via_Object_Tag(string lang, string svgFileName)
        {
            var svgPath = GetSvgPath(lang, svgFileName);
            Assert.True(File.Exists(ExternalTemplatePath), $"Missing template: {ExternalTemplatePath}");
            Assert.True(File.Exists(svgPath), $"Missing SVG fixture: {svgPath}");

            var template = await File.ReadAllTextAsync(ExternalTemplatePath);
            var wrapperHtml = MindMapHtmlWrapper.FormatWrapper(
                template,
                svgRelativePath: svgFileName,
                svgContent: string.Empty); // external variant ignores SVGCONTENT

            // Stage both the wrapper AND the SVG side-by-side in the temp dir, mirroring
            // what the pipeline does when writing Cards/Fallacies/Mindmaps/{lang}/.
            var stagedWrapper = Path.Combine(_tempDir, $"external_{lang}.html");
            var stagedSvg = Path.Combine(_tempDir, svgFileName);
            await File.WriteAllTextAsync(stagedWrapper, wrapperHtml);
            File.Copy(svgPath, stagedSvg, overwrite: true);

            Assert.DoesNotContain("[SVGPATH]", wrapperHtml);

            var page = await _browser.NewPageAsync();
            await page.GotoAsync("file:///" + stagedWrapper.Replace('\\', '/'));
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            var dataAttr = await page.Locator("#svgObject").GetAttributeAsync("data");
            Assert.Equal(svgFileName, dataAttr);

            await page.CloseAsync();
        }

    }
}
