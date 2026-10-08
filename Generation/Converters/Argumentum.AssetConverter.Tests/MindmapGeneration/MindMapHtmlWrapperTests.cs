using System.IO;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// Pure unit tests for <see cref="MindMapHtmlWrapper.FormatWrapper"/> — exercises the two
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
                "<svg xmlns=\"http://www.w3.org/2000/svg\" id=\"embedded-test\"><g class=\"node\" id=\"n1\"/></svg>", "fr");

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
                "<svg/>", "fr"); // external template ignores SVGCONTENT — safe no-op

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

        /// <summary>
        /// T4-a: the wrapper's <c>&lt;html lang&gt;</c> must carry the language of the map
        /// <em>content</em>. Before the fix both templates hard-coded <c>lang="en"</c>, so all 36
        /// committed wrappers (fr, ru, zh, ar, fa, pt, es …) declared English to screen readers,
        /// search engines and the browser's translation prompt.
        /// </summary>
        [Theory]
        [InlineData("fr")]
        [InlineData("ar")]
        [InlineData("zh")]
        public void FormatWrapper_SubstitutesLangPlaceholder(string lang)
        {
            foreach (var templatePath in new[] { IncludedTemplatePath, ExternalTemplatePath })
            {
                var template = File.ReadAllText(templatePath);

                // Pre-condition: the committed template must carry the placeholder this fix targets.
                // Without this, a template regression would make the assertion below vacuous.
                template.Should().Contain("[LANG]",
                    $"pre-condition: {Path.GetFileName(templatePath)} must carry the [LANG] placeholder");

                var result = MindMapHtmlWrapper.FormatWrapper(template, "x.svg", "<svg/>", lang);

                result.Should().NotContain("[LANG]", "the placeholder must be consumed, never shipped");
                result.Should().Contain($"lang=\"{lang}\"",
                    "the document language must match the language of the map content");
            }
        }

        /// <summary>
        /// Anti-regression guard for the exact defect T4-a removes: a hard-coded <c>lang="en"</c>
        /// re-introduced in either root template would silently re-mislabel every wrapper again.
        /// </summary>
        [Fact]
        public void FormatWrapper_Templates_HaveNoHardCodedLangEnglish()
        {
            foreach (var templatePath in new[] { IncludedTemplatePath, ExternalTemplatePath })
            {
                File.ReadAllText(templatePath).Should().NotContain("lang=\"en\"",
                    $"{Path.GetFileName(templatePath)} must not hard-code lang=\"en\" (T4-a regression)");
            }
        }

        /// <summary>
        /// An empty language would emit <c>lang=""</c> — invalid HTML that assistive technology
        /// reads as "unknown", i.e. the same accessibility failure the fix removes, but silent.
        /// Failing fast makes a missing caller argument a loud compile-or-test error instead.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void FormatWrapper_EmptyLanguage_Throws(string lang)
        {
            Action act = () => MindMapHtmlWrapper.FormatWrapper("<html>[LANG]</html>", "a.svg", "<svg/>", lang);
            act.Should().Throw<ArgumentException>()
                .WithParameterName("language");
        }

        [Fact]
        public void FormatWrapper_LanguageIsTrimmed()
        {
            var result = MindMapHtmlWrapper.FormatWrapper("<html lang=\"[LANG]\"></html>", "a.svg", "<svg/>", " fr ");
            result.Should().Be("<html lang=\"fr\"></html>");
        }

        [Fact]
        public void FormatWrapper_HandlesNullSvgContentGracefully()
        {
            var template = "<html>[SVGPATH] / [SVGCONTENT]</html>";

            var result = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", svgContent: null!, "fr");

            result.Should().Be("<html>a.svg / </html>");
        }

        [Fact]
        public void FormatWrapper_HandlesNullSvgPathGracefully()
        {
            var template = "<html>[SVGPATH] / [SVGCONTENT]</html>";

            var result = MindMapHtmlWrapper.FormatWrapper(template, svgRelativePath: null!, "<svg/>", "fr");

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
            var template = "<html>[SVGPATH] [SVGCONTENT]</html>";
            var once = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", "<svg/>", "fr");
            var twice = MindMapHtmlWrapper.FormatWrapper(once, "b.svg", "<svg id=\"other\"/>", "fr");

            // #1046 Lot C (LOW #21): Be(once) alone is self-referential — a no-op FormatWrapper
            // (returns its input unchanged) satisfies it. Positive controls first: the first
            // call must actually inject both values and consume both placeholders.
            once.Should().Contain("a.svg", "the first call must inject the svg path");
            once.Should().Contain("<svg/>", "the first call must inject the svg content");
            once.Should().NotContain("[SVGPATH]");
            once.Should().NotContain("[SVGCONTENT]");
            once.Should().NotBe(template, "a no-op FormatWrapper must not satisfy the idempotence contract");

            // Guardrail: once the placeholders are gone, re-running the helper must not mutate
            // the content. Documents the expectation and catches regressions where someone adds
            // a fourth placeholder without updating the contract (the third, [LANG], arrived
            // with T4-a and is covered by FormatWrapper_SubstitutesLangPlaceholder above).
            twice.Should().Be(once);
        }
    }
}
