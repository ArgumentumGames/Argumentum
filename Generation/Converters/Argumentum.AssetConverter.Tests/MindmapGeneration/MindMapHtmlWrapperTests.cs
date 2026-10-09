using System;
using System.IO;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// Pure unit tests for <see cref="MindMapHtmlWrapper.FormatWrapper"/> — exercises the
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
                language: "fr",
                title: "Argument fallacieux");

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
                language: "fr",
                title: "Argument fallacieux");

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

            var result = MindMapHtmlWrapper.FormatWrapper(
                template, "a.svg", svgContent: null!, language: "fr", title: null!);

            result.Should().Be("<html>a.svg / </html>");
        }

        [Fact]
        public void FormatWrapper_HandlesNullSvgPathGracefully()
        {
            var template = "<html>[SVGPATH] / [SVGCONTENT]</html>";

            var result = MindMapHtmlWrapper.FormatWrapper(
                template, svgRelativePath: null!, "<svg/>", language: "fr", title: null!);

            result.Should().Be("<html> / <svg/></html>");
        }

        [Fact]
        public void FormatWrapper_NullTemplate_Throws()
        {
            Action act = () => MindMapHtmlWrapper.FormatWrapper(null!, "a.svg", "<svg/>", "fr", "t");
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FormatWrapper_StripsXmlDeclaration_FromInlineSvg()
        {
            var template = "<html><div id=\"mindmap\">[SVGCONTENT]</div></html>";
            var svgWithXmlDecl = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\n<svg><g/></svg>";

            var result = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", svgWithXmlDecl, "fr", "t");

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

            var result = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", svgNoXmlDecl, "fr", "t");

            result.Should().Contain("<svg xmlns=\"http://www.w3.org/2000/svg\"><g/></svg>");
        }

        [Fact]
        public void FormatWrapper_Idempotent_SecondCallIsNoOp()
        {
            var template =
                "<html lang=\"[LANGUAGE]\" dir=\"[DIRECTION]\"><title>[TITLE]</title>[SVGPATH] [SVGCONTENT]</html>";
            var once = MindMapHtmlWrapper.FormatWrapper(template, "a.svg", "<svg/>", "ru", "Карта — Argumentum");
            var twice = MindMapHtmlWrapper.FormatWrapper(once, "b.svg", "<svg id=\"other\"/>", "zh", "其他");

            // #1046 Lot C (LOW #21): Be(once) alone is self-referential — a no-op FormatWrapper
            // (returns its input unchanged) satisfies it. Positive controls first: the first
            // call must actually inject every value and consume every placeholder.
            once.Should().Contain("a.svg", "the first call must inject the svg path");
            once.Should().Contain("<svg/>", "the first call must inject the svg content");
            once.Should().Contain("lang=\"ru\"", "the first call must inject the language");
            once.Should().Contain("dir=\"ltr\"", "the first call must inject the direction");
            once.Should().Contain("<title>Карта — Argumentum</title>", "the first call must inject the title");
            once.Should().NotContain("[SVGPATH]");
            once.Should().NotContain("[SVGCONTENT]");
            once.Should().NotContain("[LANGUAGE]");
            once.Should().NotContain("[DIRECTION]");
            once.Should().NotContain("[TITLE]");
            once.Should().NotBe(template, "a no-op FormatWrapper must not satisfy the idempotence contract");

            // Guardrail: once the placeholders are gone, re-running the helper must not mutate
            // the content. Documents the expectation and catches regressions where someone adds
            // another placeholder without updating the contract. The language case is the
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

                var result = MindMapHtmlWrapper.FormatWrapper(template, "x.svg", "<svg/>", lang, "T");

                result.Should().Contain($"lang=\"{lang}\"");
                result.Should().NotContain("[LANGUAGE]", "no placeholder may survive into the written wrapper");
            }
        }

        /// <summary>
        /// #457 T4b: the <c>[TITLE]</c> token replaces the frozen
        /// <c>&lt;title&gt;Taxonomy Mind Map&lt;/title&gt;</c> both templates carried. That literal is
        /// the same defect class as T4a's <c>lang="en"</c>, and it survived T4a: all 36 committed
        /// wrappers titled themselves in English whatever their language.
        /// </summary>
        [Fact]
        public void BothTemplates_CarryTheDirectionAndTitleTokens_NotTheFrozenEnglishTitle()
        {
            foreach (var templatePath in new[] { IncludedTemplatePath, ExternalTemplatePath })
            {
                var name = Path.GetFileName(templatePath);
                var template = File.ReadAllText(templatePath);

                template.Should().Contain("[DIRECTION]",
                    $"{name} must carry the direction token so each wrapper states its own writing direction");
                template.Should().Contain("[TITLE]",
                    $"{name} must carry the title token so each wrapper titles itself in its own language");
                template.Should().NotContain("Taxonomy Mind Map",
                    $"{name} must not keep the frozen English title — that literal is the T4b defect");
                template.Should().Contain("<title>[TITLE]</title>",
                    $"{name} must place the title token inside a real <title> element");
            }
        }

        /// <summary>
        /// <c>dir</c> is written for every language, not only the RTL pair, so that a wrapper states
        /// its direction instead of inheriting a default. The rule is a property of the language and
        /// is therefore derived — but it is derived from an explicit set, so a new RTL language shows
        /// up here rather than silently rendering left-to-right.
        /// </summary>
        [Theory]
        [InlineData("ar", "rtl")]
        [InlineData("fa", "rtl")]
        [InlineData("fr", "ltr")]
        [InlineData("en", "ltr")]
        [InlineData("ru", "ltr")]
        [InlineData("pt", "ltr")]
        [InlineData("es", "ltr")]
        [InlineData("zh", "ltr")]
        public void DirectionFor_MapsEveryCorpusLanguage(string lang, string expected)
        {
            MindMapHtmlWrapper.DirectionFor(lang).Should().Be(expected);

            var result = MindMapHtmlWrapper.FormatWrapper(
                "<html lang=\"[LANGUAGE]\" dir=\"[DIRECTION]\"></html>", "a.svg", "<svg/>", lang, "T");
            result.Should().Be($"<html lang=\"{lang}\" dir=\"{expected}\"></html>");
        }

        /// <summary>
        /// An unknown or blank language is not RTL by default: <c>ltr</c> is the HTML default, and
        /// guessing "rtl" from an unknown code would be the same class of silent invention as the
        /// frozen <c>lang="en"</c>.
        /// </summary>
        [Fact]
        public void DirectionFor_UnknownOrBlankLanguage_IsLeftToRight()
        {
            MindMapHtmlWrapper.DirectionFor(null!).Should().Be("ltr");
            MindMapHtmlWrapper.DirectionFor("").Should().Be("ltr");
            MindMapHtmlWrapper.DirectionFor("he").Should().Be("ltr",
                "Hebrew is not a corpus language: an undeclared RTL language must be visible here, " +
                "not silently guessed");
        }

        /// <summary>
        /// The title is declared data, never derived from the language: null substitutes to the empty
        /// string, which shows as a blank browser tab — visible, hence testable — rather than an
        /// invented title.
        /// </summary>
        [Fact]
        public void FormatWrapper_NullTitle_SubstitutesEmpty_NotAnInventedTitle()
        {
            var result = MindMapHtmlWrapper.FormatWrapper(
                "<html><title>[TITLE]</title></html>", "a.svg", "<svg/>", "fr", title: null!);

            result.Should().Be("<html><title></title></html>");
        }

        /// <summary>
        /// The title ladder: exact language when declared, else the French source language, else
        /// nothing. <see cref="MindMapHtmlWrapper.HasWrapperTitleFor"/> is what lets a caller tell
        /// "declared" from "fell back" and warn — a fallback nobody can observe is a silent default.
        /// </summary>
        [Fact]
        public void ResolveWrapperTitle_PrefersTheExactLanguage_ThenFrench_ThenNothing()
        {
            var titles = new System.Collections.Generic.Dictionary<string, string>
            {
                ["fr"] = "Carte",
                ["ru"] = "Карта",
            };

            MindMapHtmlWrapper.HasWrapperTitleFor(titles, "ru").Should().BeTrue();
            MindMapHtmlWrapper.ResolveWrapperTitle(titles, "ru").Should().Be("Карта");

            MindMapHtmlWrapper.HasWrapperTitleFor(titles, "zh").Should().BeFalse(
                "zh is not declared — that is exactly the case the writer must warn about");
            MindMapHtmlWrapper.ResolveWrapperTitle(titles, "zh").Should().Be("Carte",
                "an undeclared language degrades to the source language, visibly French rather than blank");

            MindMapHtmlWrapper.ResolveWrapperTitle(new System.Collections.Generic.Dictionary<string, string>(), "zh")
                .Should().BeNull("with no fallback declared either there is no title to write");
            MindMapHtmlWrapper.ResolveWrapperTitle(null!, "zh").Should().BeNull();
            MindMapHtmlWrapper.ResolveWrapperTitle(titles, null!).Should().Be("Carte",
                "a blank language never matches an exact entry; it goes to the fallback");
            MindMapHtmlWrapper.HasWrapperTitleFor(titles, "").Should().BeFalse();
        }
    }
}
