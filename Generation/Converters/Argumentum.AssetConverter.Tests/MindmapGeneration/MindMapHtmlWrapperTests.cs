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
        private static readonly string RepoRoot =
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".."));

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
                "<svg xmlns=\"http://www.w3.org/2000/svg\" id=\"embedded-test\"><g class=\"node\" id=\"n1\"/></svg>");

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
                "<svg/>"); // external template ignores SVGCONTENT — safe no-op

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

            var result = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", svgContent: null!);

            result.Should().Be("<html>a.svg / </html>");
        }

        [Fact]
        public void FormatWrapper_HandlesNullSvgPathGracefully()
        {
            var template = "<html>[SVGPATH] / [SVGCONTENT]</html>";

            var result = MindMapHtmlWrapper.FormatWrapper(template, svgRelativePath: null!, "<svg/>");

            result.Should().Be("<html> / <svg/></html>");
        }

        [Fact]
        public void FormatWrapper_NullTemplate_Throws()
        {
            Action act = () => MindMapHtmlWrapper.FormatWrapper(null!, "a.svg", "<svg/>");
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FormatWrapper_StripsXmlDeclaration_FromInlineSvg()
        {
            var template = "<html><div id=\"mindmap\">[SVGCONTENT]</div></html>";
            var svgWithXmlDecl = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\n<svg><g/></svg>";

            var result = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", svgWithXmlDecl);

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

            var result = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", svgNoXmlDecl);

            result.Should().Contain("<svg xmlns=\"http://www.w3.org/2000/svg\"><g/></svg>");
        }

        [Fact]
        public void FormatWrapper_Idempotent_SecondCallIsNoOp()
        {
            var template = "<html>[SVGPATH] [SVGCONTENT]</html>";
            var once = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", "<svg/>");
            var twice = MindMapHtmlWrapper.FormatWrapper(once, "b.svg", "<svg id=\"other\"/>");

            // Guardrail: once the placeholders are gone, re-running the helper must not mutate
            // the content. Documents the expectation and catches regressions where someone adds
            // a third placeholder without updating the contract.
            twice.Should().Be(once);
        }
    }
}
