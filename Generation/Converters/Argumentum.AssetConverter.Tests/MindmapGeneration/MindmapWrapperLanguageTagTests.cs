using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// #457 grain T4a — the mind-map HTML wrappers must declare their OWN language on
    /// <c>&lt;html lang="..."&gt;</c>.
    ///
    /// <para><b>The measured defect.</b> Both committed templates (<c>included.html</c>,
    /// <c>external.html</c>) hardcoded <c>&lt;html lang="en"&gt;</c>, and
    /// <see cref="MindMapHtmlWrapper.FormatWrapper"/> had no language substitution at all — the
    /// <c>language</c> argument reached <c>MindMapSvgWrapperWriter</c> and was consumed only by
    /// <c>DocumentName.Replace("[LANGUAGE]", language)</c>. Consequence on the committed tree:
    /// <b>36/36 wrappers declared English</b> — the French, Russian, Chinese, Arabic and Persian
    /// ones included (§6.1 of #1810, which reported it for the French page alone). A page whose
    /// inlined node text is Arabic and whose root element says <c>lang="en"</c> misdirects screen
    /// readers, spell-checking, hyphenation and CJK glyph selection (Han unification).</para>
    ///
    /// <para><b>Why the wrappers can be verified without the pipeline.</b> A committed wrapper is
    /// exactly <c>FormatWrapper(template, svgPath, svgContent, language)</c> — measured
    /// byte-for-byte on all 34 <c>&lt;lang&gt;/</c> files before the fix. This organ therefore
    /// re-derives each wrapper IN MEMORY from the committed template and asserts the on-disk bytes
    /// match, which pins the artefact and the generator together with no FreeMind, no Playwright
    /// and no pipeline run.</para>
    ///
    /// <para><b>Scope declared.</b> This organ covers <c>lang</c> only. Two neighbouring items of
    /// the same grain are deliberately NOT covered here: <c>&lt;title&gt;</c> still reads
    /// "Taxonomy Mind Map" in all 8 languages (fixing it needs eight authored strings — a
    /// translation act, not a mechanical one) and <c>dir="rtl"</c> is still absent for ar/fa
    /// (adding it changes layout, so it awaits ai-01's visual verdict). Neither is silently
    /// assumed correct.</para>
    /// </summary>
    public class MindmapWrapperLanguageTagTests
    {
        private static readonly string RepoRoot = TestRepoRoot.Find();
        private static readonly string MindmapDir = Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps");
        private static readonly string IncludedTemplatePath = Path.Combine(MindmapDir, "included.html");
        private static readonly string ExternalTemplatePath = Path.Combine(MindmapDir, "external.html");

        private static readonly Regex HtmlLangRegex =
            new(@"<html[^>]*\blang=""(?<lang>[^""]*)""", RegexOptions.Compiled);

        private static readonly Regex ObjectDataRegex =
            new(@"<object[^>]*\bdata=""(?<path>[^""]*)""", RegexOptions.Compiled);

        /// <summary>A committed wrapper paired with everything needed to re-derive it.</summary>
        private readonly record struct Wrapper(string Path, string Lang, string TemplatePath, string SvgName, string? SvgContent);

        /// <summary>
        /// Every committed wrapper under <c>Cards/Fallacies/Mindmaps/&lt;lang&gt;/</c>, both variants.
        /// The inlining half is paired through <see cref="MindmapWrapperFreshnessGateTests.WrapperNameFor"/>
        /// — the SAME naming rule the freshness organ uses, deliberately not a second copy of it, so
        /// the two organs cannot drift apart. The <c>_ext</c> half is paired through its own
        /// <c>&lt;object data="..."&gt;</c> attribute, which is the actual value the generator
        /// substituted into <c>[SVGPATH]</c>.
        /// </summary>
        private static List<Wrapper> CommittedWrappers()
        {
            var result = new List<Wrapper>();

            foreach (var langDir in Directory.EnumerateDirectories(MindmapDir).OrderBy(d => d, StringComparer.Ordinal))
            {
                var lang = Path.GetFileName(langDir);

                foreach (var svg in Directory.EnumerateFiles(langDir, "*.content.svg").OrderBy(p => p, StringComparer.Ordinal))
                {
                    var wrapperName = MindmapWrapperFreshnessGateTests.WrapperNameFor(Path.GetFileName(svg));
                    if (wrapperName is null) continue;
                    var wrapperPath = Path.Combine(langDir, wrapperName);
                    if (!File.Exists(wrapperPath)) continue;
                    result.Add(new Wrapper(wrapperPath, lang, IncludedTemplatePath,
                        Path.GetFileName(svg), File.ReadAllText(svg)));
                }

                foreach (var extPath in Directory.EnumerateFiles(langDir, "*_ext.html").OrderBy(p => p, StringComparer.Ordinal))
                {
                    var match = ObjectDataRegex.Match(File.ReadAllText(extPath));
                    if (!match.Success) continue;
                    result.Add(new Wrapper(extPath, lang, ExternalTemplatePath, match.Groups["path"].Value, null));
                }
            }

            return result;
        }

        private static string DeclaredLang(string html)
        {
            var match = HtmlLangRegex.Match(html);
            match.Success.Should().BeTrue(
                "every committed wrapper must declare <html lang=\"...\"> — the element is the whole point of this organ");
            return match.Groups["lang"].Value;
        }

        /// <summary>First index at which two strings differ, plus a readable window around it.</summary>
        private static string FirstDivergence(string expected, string actual)
        {
            var shared = Math.Min(expected.Length, actual.Length);
            var at = 0;
            while (at < shared && expected[at] == actual[at]) at++;

            if (at == shared && expected.Length == actual.Length)
                return "(no divergence)";

            var from = Math.Max(0, at - 60);
            var exp = expected.Substring(from, Math.Min(120, expected.Length - from));
            var act = actual.Substring(from, Math.Min(120, actual.Length - from));
            return $"len expected={expected.Length} actual={actual.Length}, first divergence at {at}\n" +
                   $"      expected: ...{exp}...\n" +
                   $"      actual  : ...{act}...";
        }

        [Fact]
        public void BothTemplates_CarryTheLanguageToken_AndNoLongerHardcodeEnglish()
        {
            foreach (var templatePath in new[] { IncludedTemplatePath, ExternalTemplatePath })
            {
                var template = File.ReadAllText(templatePath);
                var name = Path.GetFileName(templatePath);

                template.Should().Contain("[LANGUAGE]",
                    $"{name} must carry the language token so each wrapper is written in its own language");
                template.Should().NotContain("lang=\"en\"",
                    $"{name} must not hardcode English — that hardcoded value is the T4a defect, " +
                    "and it is what made all 36 committed wrappers declare themselves English");
            }
        }

        [Fact]
        public void EveryCommittedWrapper_DeclaresItsOwnDirectoryLanguage()
        {
            var wrappers = CommittedWrappers();

            wrappers.Should().HaveCount(34,
                "the tree ships 17 inlining + 17 _ext wrappers (8 Fallacies + 8 Virtues + 1 FR cards, " +
                "each in both variants) — a lower count means files went missing, not that scope shrank");

            var offenders = wrappers
                .Select(w => (Name: Path.GetFileName(w.Path), Declared: DeclaredLang(File.ReadAllText(w.Path)), w.Lang))
                .Where(t => t.Declared != t.Lang)
                .Select(t => $"{t.Name}: declares lang=\"{t.Declared}\" but lives in the \"{t.Lang}\" directory")
                .ToList();

            offenders.Should().BeEmpty(
                "each wrapper serves its own language's inlined mind map; declaring another language " +
                "misdirects screen readers, hyphenation and CJK glyph selection");
        }

        /// <summary>
        /// The artefact pin: each committed wrapper must be byte-identical to what the fixed
        /// generator produces from the committed template. This is what makes the tree state and the
        /// generator state provably the same object — a hand-edited wrapper, or a template change
        /// that was not re-derived onto the tree, fails here by name.
        /// </summary>
        [Fact]
        public void EveryCommittedWrapper_IsByteIdenticalToTheGeneratorOutput()
        {
            var offenders = new List<string>();

            foreach (var wrapper in CommittedWrappers())
            {
                var expected = MindMapHtmlWrapper.FormatWrapper(
                    File.ReadAllText(wrapper.TemplatePath),
                    wrapper.SvgName,
                    wrapper.SvgContent,
                    wrapper.Lang);

                var actual = File.ReadAllText(wrapper.Path);
                if (expected != actual)
                {
                    offenders.Add($"{wrapper.Lang}/{Path.GetFileName(wrapper.Path)}: " +
                                  FirstDivergence(expected, actual));
                }
            }

            offenders.Should().BeEmpty(
                "a committed wrapper must equal FormatWrapper(template, svg, language) byte for byte — " +
                "otherwise the tree carries a wrapper the generator would not reproduce");
        }

        /// <summary>
        /// Inverse control: the derivation check above must be able to FAIL. Without this, a check
        /// that silently compared a file to itself would look identical to a passing one.
        /// </summary>
        [Fact]
        public void InverseControl_ADivergentLanguage_FailsTheDerivationCheck()
        {
            var wrapper = CommittedWrappers().First(w => w.Lang == "fr" && w.SvgContent is not null);
            var includedTemplate = File.ReadAllText(IncludedTemplatePath);

            var faithful = MindMapHtmlWrapper.FormatWrapper(
                includedTemplate, wrapper.SvgName, wrapper.SvgContent, wrapper.Lang);
            var forged = MindMapHtmlWrapper.FormatWrapper(
                includedTemplate, wrapper.SvgName, wrapper.SvgContent, "en");

            faithful.Should().Be(File.ReadAllText(wrapper.Path),
                "pre-condition: this pair must be faithful, otherwise the control proves nothing");
            forged.Should().NotBe(faithful,
                "re-deriving with another language MUST change the bytes — if it did not, the language " +
                "token would be inert and this whole organ would be vacuous");
            forged.Should().Contain("lang=\"en\"");
        }
    }
}
