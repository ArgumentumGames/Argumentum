using System;
using System.IO;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Mindmapper;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// Regression guard for issue #725: the Virtues mind-map HTML wrappers
    /// (<c>Argumentation_Virtues_{lang}.html</c>) must inline the **localized**
    /// <c>Argumentum_Virtues_MindMap_{lang}.content.svg</c> per language — not the
    /// French source SVG. The pipeline path is
    /// <c>VirtueMindMapDocumentConfig.GenerateHtmlSvgWrappers</c> →
    /// <see cref="MindMapHtmlWrapper.FormatWrapper"/>, which substitutes the
    /// post-localization <c>content.svg</c> into the <c>[SVGCONTENT]</c> placeholder.
    ///
    /// The **committed** wrappers are stale (generated 2026-05-24 by the pre-#665
    /// code, when the Virtues <c>content.svg</c> was FR-frozen — see
    /// <c>docs/investigations/2026-06-25-virtues-mindmap-fr-frozen-mechanism.md</c>).
    /// They were skipped by every later SVG regen because
    /// <c>AssetConverterConfig.OverwriteExistingHtmlMaps</c> defaults to <c>false</c>
    /// while <c>OverwriteExistingDocs</c> is <c>true</c>. The stale files are tracked
    /// separately for a post-tag regen (po-2023 lane); this test does NOT touch
    /// <c>Cards/</c>.
    ///
    /// What this test pins is the **assembly contract**: feeding the *already-localized*
    /// committed <c>content.svg</c> through <see cref="MindMapHtmlWrapper.FormatWrapper"/>
    /// must yield a wrapper whose visible node text matches the target language. It
    /// mirrors the Fallacies Playwright suite
    /// (<c>VisualTests/MindmapWrapperTests.cs</c>) but runs headless in CI (no browser),
    /// so a future regression in the helper or in the SVG-localization wiring surfaces
    /// here without RDP/FreeMind.
    /// </summary>
    public class VirtuesMindmapWrapperLocalizationTests
    {
        /// <summary>
        /// Visible node text that is French-only in the FR-frozen Virtues mind map.
        /// Empirically (master <c>6ce91ef8</c>) present 27× in the FR <c>content.svg</c>
        /// and 0× in every localized one — a sharp cross-contamination discriminator.
        /// </summary>
        private const string FrenchFrozenMarker = "Honnêteté intellectuelle";

        private static readonly string RepoRoot =
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".."));

        private static readonly string IncludedTemplatePath =
            Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", "included.html");

        private static string GetVirtuesContentSvgPath(string lang)
            => Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", lang,
                $"Argumentum_Virtues_MindMap_{lang}.content.svg");

        private static int CountInRange(string s, char low, char high)
        {
            var n = 0;
            foreach (var c in s)
                if (c >= low && c <= high) n++;
            return n;
        }

        private static int CountCyrillic(string s) => CountInRange(s, 'Ѐ', 'ӿ');
        private static int CountCjk(string s) => CountInRange(s, '一', '鿿');
        private static int CountArabicScript(string s) => CountInRange(s, '؀', 'ۿ');

        /// <summary>
        /// For each language, the wrapper assembled from the committed localized
        /// <c>content.svg</c> must carry that language's script/labels and must NOT
        /// carry the French-frozen marker. This is the headless proof that the correct
        /// per-language <c>content.svg</c> is the one being inlined.
        /// </summary>
        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("zh")]
        [InlineData("ar")]
        [InlineData("fa")]
        public async Task FormatWrapper_WithLocalizedVirtuesContentSvg_WrapperMatchesTargetLanguage(string lang)
        {
            Assert.True(File.Exists(IncludedTemplatePath),
                $"Missing included.html template: {IncludedTemplatePath}");
            var svgPath = GetVirtuesContentSvgPath(lang);
            Assert.True(File.Exists(svgPath),
                $"Missing localized Virtues content.svg fixture: {svgPath}. " +
                "If this was deleted, the test fixture must be regenerated (see issue #725).");

            var template = await File.ReadAllTextAsync(IncludedTemplatePath);
            var svg = await File.ReadAllTextAsync(svgPath);

            var wrapper = MindMapHtmlWrapper.FormatWrapper(
                template,
                svgRelativePath: $"Argumentum_Virtues_MindMap_{lang}.content.svg",
                svgContent: svg);

            // The two placeholder tokens must both be consumed (no partial substitution).
            Assert.DoesNotContain("[SVGCONTENT]", wrapper);
            Assert.DoesNotContain("[SVGPATH]", wrapper);
            // The inlined SVG body must be present.
            Assert.Contains("<svg", wrapper);

            switch (lang)
            {
                case "fr":
                    // FR is the source language: the FR-frozen marker IS expected here.
                    Assert.Contains(FrenchFrozenMarker, wrapper);
                    break;
                case "en":
                    Assert.Contains("Intellectual honesty", wrapper);
                    Assert.DoesNotContain(FrenchFrozenMarker, wrapper);
                    break;
                case "ru":
                    Assert.True(CountCyrillic(wrapper) > 100,
                        $"RU wrapper should carry Cyrillic node text, got {CountCyrillic(wrapper)}.");
                    Assert.DoesNotContain(FrenchFrozenMarker, wrapper);
                    break;
                case "zh":
                    Assert.True(CountCjk(wrapper) > 100,
                        $"ZH wrapper should carry CJK node text, got {CountCjk(wrapper)}.");
                    Assert.DoesNotContain(FrenchFrozenMarker, wrapper);
                    break;
                case "ar":
                case "fa":
                    // Arabic and Persian both use the Arabic script block (U+0600–U+06FF);
                    // Persian-specific letters (U+06CC etc.) also fall in this range.
                    Assert.True(CountArabicScript(wrapper) > 100,
                        $"{lang} wrapper should carry Arabic-script node text, got {CountArabicScript(wrapper)}.");
                    Assert.DoesNotContain(FrenchFrozenMarker, wrapper);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lang), lang, "Unhandled InlineData language.");
            }
        }

        /// <summary>
        /// Cross-language no-contamination contract: the EN wrapper must not contain
        /// the FR-frozen marker AND must not accidentally carry another language's
        /// native script. Guards against a future regression where the wrong per-language
        /// <c>content.svg</c> is inlined (e.g. all wrappers seeded from FR).
        /// </summary>
        [Fact]
        public async Task FormatWrapper_VirtuesEnglish_NoFrenchOrForeignScriptContamination()
        {
            var svgPath = GetVirtuesContentSvgPath("en");
            Assert.True(File.Exists(svgPath), $"Missing fixture: {svgPath}");
            var template = await File.ReadAllTextAsync(IncludedTemplatePath);
            var svg = await File.ReadAllTextAsync(svgPath);

            var wrapper = MindMapHtmlWrapper.FormatWrapper(template, "x.svg", svg);

            Assert.Contains("Intellectual honesty", wrapper);
            Assert.DoesNotContain(FrenchFrozenMarker, wrapper);
            Assert.True(CountCyrillic(wrapper) == 0,
                $"EN wrapper should have zero Cyrillic codepoints, got {CountCyrillic(wrapper)}.");
            Assert.True(CountCjk(wrapper) == 0,
                $"EN wrapper should have zero CJK codepoints, got {CountCjk(wrapper)}.");
            Assert.True(CountArabicScript(wrapper) == 0,
                $"EN wrapper should have zero Arabic-script codepoints, got {CountArabicScript(wrapper)}.");
        }
    }
}
