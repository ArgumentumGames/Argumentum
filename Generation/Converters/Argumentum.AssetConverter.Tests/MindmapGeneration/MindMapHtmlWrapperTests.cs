using System.IO;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// Pure unit tests for <see cref="MindMapHtmlWrapper.FormatWrapper"/> — exercises the three
    /// placeholder substitutions on both real templates (<c>included.html</c>, <c>external.html</c>)
    /// without spinning up Playwright. Issue #196 phase 1 — ensures the regression that shipped
    /// literal "[SVGCONTENT]" strings in the DOM cannot reappear silently.
    /// </summary>
    public class MindMapHtmlWrapperTests
    {
        private static readonly string RepoRoot = TestRepoRoot.Find();

        private static readonly string IncludedTemplatePath =
            Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", "included.html");

        private static readonly string ExternalTemplatePath =
            Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", "external.html");

        [Fact]
        public void FormatWrapper_Included_ReplacesSvgContentPlaceholder()
        {
            var template = File.ReadAllText(IncludedTemplatePath);
            template.Should().Contain("[SVGCONTENT]",
                "pre-condition: the committed template must contain the placeholder this fix targets");

            var result = MindMapHtmlWrapper.FormatWrapper(
                template,
                "Fallacies_fr.content.svg",
                "<svg xmlns=\"http://www.w3.org/2000/svg\" id=\"embedded-test\"><g class=\"node\" id=\"n1\"/></svg>",
                language: "fr");

            result.Should().NotContain("[SVGCONTENT]",
                "the regression introduced in #129 shipped wrappers with the literal placeholder in the DOM");
            result.Should().Contain("id=\"embedded-test\"", "the SVG markup must be inlined inside the wrapper");
            result.Should().Contain("class=\"node\"", "clickable nodes must be preserved in the inline SVG");
        }

        [Fact]
        public void FormatWrapper_External_ReplacesSvgPathPlaceholder()
        {
            var template = File.ReadAllText(ExternalTemplatePath);
            template.Should().Contain("[SVGPATH]");

            var result = MindMapHtmlWrapper.FormatWrapper(
                template,
                "Fallacies_fr.content.svg",
                "<svg/>", // external template ignores SVGCONTENT — safe no-op
                language: "fr");

            result.Should().NotContain("[SVGPATH]");
            result.Should().Contain("data=\"Fallacies_fr.content.svg\"",
                "the <object> tag should reference the SVG file via the replaced [SVGPATH]");
        }

        [Fact]
        public void FormatWrapper_External_TemplateHasNoSvgContentPlaceholder()
        {
            // Documents the design: only included.html carries [SVGCONTENT]. This prevents future
            // edits from accidentally embedding an SVG twice in the external variant.
            var template = File.ReadAllText(ExternalTemplatePath);
            template.Should().NotContain("[SVGCONTENT]");
        }

        [Fact]
        public void FormatWrapper_HandlesNullSvgContentGracefully()
        {
            var template = "<html>[SVGPATH] / [SVGCONTENT]</html>";

            var result = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", svgContent: null!, language: "fr");

            result.Should().Be("<html>a.svg / </html>");
        }

        [Fact]
        public void FormatWrapper_HandlesNullSvgPathGracefully()
        {
            var template = "<html>[SVGPATH] / [SVGCONTENT]</html>";

            var result = MindMapHtmlWrapper.FormatWrapper(template, svgRelativePath: null!, "<svg/>", language: "fr");

            result.Should().Be("<html> / <svg/></html>");
        }

        [Fact]
        public void FormatWrapper_NullTemplate_Throws()
        {
            Action act = () => MindMapHtmlWrapper.FormatWrapper(null!, "a.svg", "<svg/>", "fr");
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FormatWrapper_StripsXmlDeclaration_FromInlineSvg()
        {
            var template = "<html><div id=\"mindmap\">[SVGCONTENT]</div></html>";
            var svgWithXmlDecl = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\n<svg><g/></svg>";

            var result = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", svgWithXmlDecl, "fr");

            result.Should().NotContain("<?xml",
                "XML declarations are invalid inside HTML and break browser rendering");
            result.Should().Contain("<svg><g/></svg>",
                "the SVG body must be preserved after stripping the XML declaration");
        }

        [Fact]
        public void FormatWrapper_StripsXmlDeclaration_NoChange_WhenAbsent()
        {
            var template = "<html>[SVGCONTENT]</html>";
            var svgNoXmlDecl = "<svg xmlns=\"http://www.w3.org/2000/svg\"><g/></svg>";

            var result = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", svgNoXmlDecl, "fr");

            result.Should().Contain("<svg xmlns=\"http://www.w3.org/2000/svg\"><g/></svg>");
        }

        [Fact]
        public void FormatWrapper_Idempotent_SecondCallIsNoOp()
        {
            var template = "<html lang=\"[LANGUAGE]\">[SVGPATH] [SVGCONTENT]</html>";
            var once = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", "<svg/>", "ru");
            var twice = MindMapHtmlWrapper.FormatWrapper(once, "b.svg", "<svg id=\"other\"/>", "zh");

            // #1046 Lot C (LOW #21): Be(once) alone is self-referential — a no-op FormatWrapper
            // (returns its input unchanged) satisfies it. Positive controls first: the first
            // call must actually inject both values and consume both placeholders.
            once.Should().Contain("a.svg", "the first call must inject the svg path");
            once.Should().Contain("<svg/>", "the first call must inject the svg content");
            once.Should().Contain("lang=\"ru\"", "the first call must inject the language");
            once.Should().NotContain("[SVGPATH]");
            once.Should().NotContain("[SVGCONTENT]");
            once.Should().NotContain("[LANGUAGE]");
            once.Should().NotBe(template, "a no-op FormatWrapper must not satisfy the idempotence contract");

            // Guardrail: once the placeholders are gone, re-running the helper must not mutate
            // the content. Documents the expectation and catches regressions where someone adds
            // a fourth placeholder without updating the contract. The language case is the
            // sharpest: the second call passes a DIFFERENT language ("zh") and must not win.
            twice.Should().Be(once);
            twice.Should().Contain("lang=\"ru\"", "a consumed language token cannot be re-substituted");
        }

        /// <summary>
        /// #457 T4a: both committed templates must carry the <c>[LANGUAGE]</c> token and must no
        /// longer hardcode <c>lang="en"</c>. Before this fix every one of the 36 committed
        /// wrappers — French, Russian and Arabic included — declared itself English.
        /// </summary>
        [Theory]
        [InlineData("fr")]
        [InlineData("ar")]
        [InlineData("zh")]
        public void FormatWrapper_LanguageToken_ReplacesTheHardcodedEnglish(string lang)
        {
            foreach (var templatePath in new[] { IncludedTemplatePath, ExternalTemplatePath })
            {
                var template = File.ReadAllText(templatePath);

                template.Should().Contain("[LANGUAGE]",
                    $"{Path.GetFileName(templatePath)} must carry the language token, not a frozen value");
                template.Should().NotContain("lang=\"en\"",
                    $"{Path.GetFileName(templatePath)} must not hardcode English — that is the T4a defect");

                var result = MindMapHtmlWrapper.FormatWrapper(template, "x.svg", "<svg/>", lang);

                result.Should().Contain($"lang=\"{lang}\"");
                result.Should().NotContain("[LANGUAGE]", "no placeholder may survive into the written wrapper");
            }
        }

        /// <summary>
        /// The language argument follows the same null contract as the other two: null substitutes
        /// to the empty string, yielding <c>lang=""</c> — "unknown", which is honest — rather than
        /// silently falling back to a wrong language. A defaulted parameter would have reproduced
        /// the T4a defect, which is why the argument is required.
        /// </summary>
        [Fact]
        public void FormatWrapper_NullLanguage_SubstitutesEmpty_NotEnglish()
        {
            var result = MindMapHtmlWrapper.FormatWrapper(
                "<html lang=\"[LANGUAGE]\"></html>", "a.svg", "<svg/>", language: null!);

            result.Should().Be("<html lang=\"\"></html>");
            result.Should().NotContain("en");
        }
    }
}
