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
    /// #457 grains T4a + T4b — the mind-map HTML wrappers must declare their OWN language, writing
    /// direction and title, in the three head locations that were frozen to English.
    ///
    /// <para><b>The measured defect (T4a).</b> Both committed templates
    /// (<c>included.html</c>, <c>external.html</c>) hardcoded <c>&lt;html lang="en"&gt;</c>, and
    /// <see cref="MindMapHtmlWrapper.FormatWrapper"/> had no language substitution at all — the
    /// <c>language</c> argument reached <c>MindMapSvgWrapperWriter</c> and was consumed only by
    /// <c>DocumentName.Replace("[LANGUAGE]", language)</c>. Consequence on the committed tree:
    /// <b>36/36 wrappers declared English</b> — the French, Russian, Chinese, Arabic and Persian
    /// ones included (§6.1 of #1810, which reported it for the French page alone). A page whose
    /// inlined node text is Arabic and whose root element says <c>lang="en"</c> misdirects screen
    /// readers, spell-checking, hyphenation and CJK glyph selection (Han unification).</para>
    ///
    /// <para><b>The neighbouring defect (T4b).</b> The same templates also froze
    /// <c>&lt;title&gt;Taxonomy Mind Map&lt;/title&gt;</c>, so all 36 wrappers titled themselves in
    /// English, and no wrapper declared a writing direction at all — the two RTL corpus languages
    /// (ar, fa) rendered left-to-right. Both were reported at the T4a verdict and fixed here.</para>
    ///
    /// <para><b>Why the wrappers can be verified without the pipeline.</b> A committed wrapper is
    /// exactly <c>FormatWrapper(template, svgPath, svgContent, language, title)</c> — measured
    /// byte-for-byte on all 34 <c>&lt;lang&gt;/</c> files before each fix. This organ re-derives each
    /// wrapper IN MEMORY from the committed template plus the title the creator config declares, and
    /// asserts the on-disk bytes match, which pins artefact, generator and config together with no
    /// FreeMind, no Playwright and no pipeline run.</para>
    /// </summary>
    public class MindmapWrapperLanguageTagTests
    {
        private static readonly string RepoRoot = TestRepoRoot.Find();
        private static readonly string MindmapDir = Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps");
        private static readonly string IncludedTemplatePath = Path.Combine(MindmapDir, "included.html");
        private static readonly string ExternalTemplatePath = Path.Combine(MindmapDir, "external.html");

        private static readonly Regex HtmlLangRegex =
            new(@"<html[^>]*\blang=""(?<lang>[^""]*)""", RegexOptions.Compiled);

        private static readonly Regex HtmlDirRegex =
            new(@"<html[^>]*\bdir=""(?<dir>[^""]*)""", RegexOptions.Compiled);

        private static readonly Regex TitleRegex =
            new(@"<title>(?<title>.*?)</title>", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex ObjectDataRegex =
            new(@"<object[^>]*\bdata=""(?<path>[^""]*)""", RegexOptions.Compiled);

        /// <summary>
        /// The <c>#mindmap</c> rule of a wrapper's stylesheet — the SVG's container. Matched as a whole
        /// block so the pin is read from the rule that actually governs the SVG rather than from any
        /// <c>direction: ltr</c> anywhere in the file, which a naive substring check would accept.
        /// </summary>
        private static readonly Regex MindmapContainerRuleRegex =
            new(@"#mindmap\s*\{(?<body>[^}]*)\}", RegexOptions.Compiled);

        /// <summary>
        /// A committed wrapper paired with everything needed to re-derive it. <see cref="SvgName"/> is
        /// both the <c>[SVGPATH]</c> value of the external variant and the key into the declared
        /// titles; <see cref="Title"/> is the title the creator config declares for this language.
        /// </summary>
        private readonly record struct Wrapper(
            string Path, string Lang, string SvgName, string TemplatePath, string? SvgContent, string Title);

        /// <summary>
        /// The declared wrapper titles, keyed by the wrapper's own <c>DocumentName</c> pattern
        /// (<c>Fallacies_[LANGUAGE].html</c> and friends) — the <c>[LANGUAGE]</c> form, not a resolved
        /// file name, because one document config generates all eight languages.
        ///
        /// <para>⚠️ Deliberately NOT keyed by the parent document name (<c>Fallacies_fr.mm</c>): that
        /// name carries the <b>source</b> language, so a key built from it resolves only for French
        /// and reports every other language as undeclared — measured, and it is what made this organ
        /// fail on the first run.</para>
        ///
        /// <para>Read from the creator configs, so this organ pins the artefact against the very table
        /// the production writer consumes; a second hand-written copy of the titles here would drift
        /// silently.</para>
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> WrapperTitlesByPattern()
        {
            var index = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);

            void Index(IEnumerable<SVGFreemindMap> maps)
            {
                foreach (var map in maps)
                {
                    if (map.HtmlWrapperTitles.Count == 0) continue;
                    foreach (var wrapper in map.HtmlWrappers)
                        index[wrapper.DocumentName] = map.HtmlWrapperTitles;
                }
            }

            foreach (var doc in new FallacyMindMapCreatorConfig().DocumentConfigs)
                Index(doc.SVGMaps);
            foreach (var doc in new VirtueMindMapCreatorConfig().DocumentConfigs)
                Index(doc.SVGMaps);

            return index;
        }

        /// <summary>
        /// The <c>[LANGUAGE]</c> pattern a committed wrapper file name was generated from. Derived by
        /// substituting the language back out, with the same two shapes the configs declare — a
        /// wrapper whose name matches neither shape fails loudly rather than silently resolving to a
        /// table it does not belong to.
        /// </summary>
        private static string WrapperPatternOf(string fileName, string lang)
        {
            foreach (var candidate in new[]
                     {
                         fileName.Replace("_" + lang + "_ext.html", "_[LANGUAGE]_ext.html"),
                         fileName.Replace("_" + lang + ".html", "_[LANGUAGE].html"),
                     })
            {
                if (candidate != fileName && candidate.Contains("[LANGUAGE]")) return candidate;
            }

            throw new InvalidOperationException(
                $"cannot derive the wrapper-name pattern of '{fileName}' for language '{lang}': neither " +
                $"the '_{lang}.html' nor the '_{lang}_ext.html' shape matches");
        }

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
            var titles = WrapperTitlesByPattern();

            string TitleFor(string fileName, string lang, string wrapperPath)
            {
                var pattern = WrapperPatternOf(fileName, lang);
                Assert.True(titles.ContainsKey(pattern),
                    $"no creator config declares wrapper titles for '{pattern}' (needed by {wrapperPath}) — " +
                    "either the config lost its HtmlWrapperTitles or this organ's addressing convention drifted");
                var declared = MindMapHtmlWrapper.ResolveWrapperTitle(titles[pattern], lang);
                Assert.False(string.IsNullOrWhiteSpace(declared),
                    $"the creator config declares no usable title for '{lang}' on '{pattern}'");
                return declared!;
            }

            foreach (var langDir in Directory.EnumerateDirectories(MindmapDir).OrderBy(d => d, StringComparer.Ordinal))
            {
                var lang = Path.GetFileName(langDir);

                foreach (var svg in Directory.EnumerateFiles(langDir, "*.content.svg").OrderBy(p => p, StringComparer.Ordinal))
                {
                    var wrapperName = MindmapWrapperFreshnessGateTests.WrapperNameFor(Path.GetFileName(svg));
                    if (wrapperName is null) continue;
                    var wrapperPath = Path.Combine(langDir, wrapperName);
                    if (!File.Exists(wrapperPath)) continue;
                    var svgName = Path.GetFileName(svg);
                    result.Add(new Wrapper(wrapperPath, lang, svgName, IncludedTemplatePath,
                        File.ReadAllText(svg), TitleFor(wrapperName, lang, wrapperPath)));
                }

                foreach (var extPath in Directory.EnumerateFiles(langDir, "*_ext.html").OrderBy(p => p, StringComparer.Ordinal))
                {
                    var match = ObjectDataRegex.Match(File.ReadAllText(extPath));
                    if (!match.Success) continue;
                    var svgName = match.Groups["path"].Value;
                    result.Add(new Wrapper(extPath, lang, svgName, ExternalTemplatePath, null,
                        TitleFor(Path.GetFileName(extPath), lang, extPath)));
                }
            }

            return result;
        }

        private static string MatchOrFail(Regex regex, string html, string what)
        {
            var match = regex.Match(html);
            match.Success.Should().BeTrue(
                $"every committed wrapper must declare {what} — that declaration is the whole point of this organ");
            return match.Groups[1].Value;
        }

        private static string DeclaredLang(string html) => MatchOrFail(HtmlLangRegex, html, "<html lang=\"...\">");
        private static string DeclaredDir(string html) => MatchOrFail(HtmlDirRegex, html, "<html ... dir=\"...\">");
        private static string DeclaredTitle(string html) => MatchOrFail(TitleRegex, html, "<title>...</title>");

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

        private static int CountInRange(string s, char low, char high)
        {
            var n = 0;
            foreach (var c in s)
                if (c >= low && c <= high) n++;
            return n;
        }

        [Fact]
        public void BothTemplates_CarryTheThreeHeadTokens_AndNoFrozenEnglish()
        {
            foreach (var templatePath in new[] { IncludedTemplatePath, ExternalTemplatePath })
            {
                var template = File.ReadAllText(templatePath);
                var name = Path.GetFileName(templatePath);

                template.Should().Contain("[LANGUAGE]",
                    $"{name} must carry the language token so each wrapper is written in its own language");
                template.Should().Contain("[DIRECTION]",
                    $"{name} must carry the direction token so each wrapper states its writing direction");
                template.Should().Contain("[TITLE]",
                    $"{name} must carry the title token so each wrapper titles itself in its own language");
                template.Should().NotContain("lang=\"en\"",
                    $"{name} must not hardcode English — that hardcoded value is the T4a defect, " +
                    "and it is what made all 36 committed wrappers declare themselves English");
                template.Should().NotContain("Taxonomy Mind Map",
                    $"{name} must not keep the frozen English title — that literal is the T4b defect");
            }
        }

        /// <summary>
        /// Anti-silent-fallback: every family must declare a title for all eight corpus languages.
        /// <see cref="MindMapHtmlWrapper.ResolveWrapperTitle"/> degrades to French when a language is
        /// missing — that degradation is warned about at runtime, but it must never be reachable on
        /// the shipped corpus, so a missing line is a red test rather than a French title on a
        /// Chinese page.
        /// </summary>
        [Fact]
        public void EveryWrapperShippedOnDisk_HasATitleDeclaredForItsLanguage()
        {
            var titles = WrapperTitlesByPattern();

            titles.Keys.Should().HaveCount(6,
                "three wrapper documents ship two halves each (inlining + _ext), so six name patterns " +
                "must resolve to a declared table — a lower count means a config lost its declaration");
            titles.Values.Distinct().Should().HaveCount(3,
                "three documents declare their own title table: the Fallacies map, the FR-only Fallacies " +
                "cards map and the Virtues map");

            // Deliberately scanned from the TREE rather than from the config: asking the config which
            // languages it generates, then checking the config declares them, would be circular. The
            // claim is the other way round — every wrapper that actually ships must be covered.
            var scanned = 0;
            var offenders = new List<string>();
            foreach (var langDir in Directory.EnumerateDirectories(MindmapDir).OrderBy(d => d, StringComparer.Ordinal))
            {
                var lang = Path.GetFileName(langDir);
                foreach (var file in Directory.EnumerateFiles(langDir, "*.html").OrderBy(p => p, StringComparer.Ordinal))
                {
                    scanned++;
                    var name = Path.GetFileName(file);
                    var pattern = WrapperPatternOf(name, lang);
                    if (!titles.TryGetValue(pattern, out var table))
                    {
                        offenders.Add($"{lang}/{name}: pattern '{pattern}' has no declared title table");
                        continue;
                    }

                    if (!MindMapHtmlWrapper.HasWrapperTitleFor(table, lang))
                        offenders.Add($"{lang}/{name}: no title declared for '{lang}'");
                }
            }

            scanned.Should().Be(34, "the tree ships 34 wrappers under the language directories");

            offenders.Should().BeEmpty(
                "a wrapper whose language has no declared title silently degrades to French via ResolveWrapperTitle");
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

        [Fact]
        public void EveryCommittedWrapper_DeclaresItsWritingDirection()
        {
            var offenders = CommittedWrappers()
                .Select(w => (Name: $"{w.Lang}/{Path.GetFileName(w.Path)}",
                              Declared: DeclaredDir(File.ReadAllText(w.Path)),
                              Expected: MindMapHtmlWrapper.DirectionFor(w.Lang)))
                .Where(t => t.Declared != t.Expected)
                .Select(t => $"{t.Name}: declares dir=\"{t.Declared}\" but should declare dir=\"{t.Expected}\"")
                .ToList();

            offenders.Should().BeEmpty(
                "the two RTL corpus languages (ar, fa) rendered left-to-right in every committed wrapper " +
                "before T4b; the direction is a property of the language and must be stated, not inherited");
        }

        private static string MindmapContainerRule(string html)
        {
            var match = MindmapContainerRuleRegex.Match(html);
            match.Success.Should().BeTrue(
                "the wrapper must carry a '#mindmap { ... }' rule — it is the SVG's container");
            return match.Groups["body"].Value;
        }

        /// <summary>
        /// The other half of the direction work, and the one the byte pin cannot see: stating the
        /// language's direction on the document must NOT let it reach the inlined SVG. FreeMind/Batik
        /// computed every coordinate and the anchors left-to-right, so under a rtl base direction
        /// <c>text-anchor: start</c> resolves to the RIGHT edge and each label is displaced by its own
        /// width. Measured in Chromium on the committed <c>ar/Fallacies_ar.html</c> (1408 <c>&lt;text&gt;</c>):
        /// no <c>dir</c> at all -> 1377 anchored left; <c>dir="rtl"</c> alone -> 0 left and 1224 right;
        /// <c>dir="rtl"</c> plus this container pin -> back to 1377. The document keeps its direction.
        /// </summary>
        [Fact]
        public void InliningTemplate_PinsTheSvgContainerToLtr()
        {
            var template = File.ReadAllText(IncludedTemplatePath);

            MindmapContainerRule(template).Should().Contain("direction: ltr",
                "the inlining template must pin the SVG container to ltr — without it, 'dir=\"rtl\"' on " +
                "the document flips every text anchor to the right edge and shifts each label by its width");

            template.Should().Contain("[DIRECTION]",
                "the document must still declare its own language's direction: the container pin is a " +
                "second, narrower rule, not a replacement for stating the direction");
        }

        /// <summary>
        /// The same pin, on the artefacts, plus the reason the <c>_ext</c> half needs none: the external
        /// wrappers embed the SVG through <c>&lt;object&gt;</c>, whose document is separate, so the host's
        /// direction cannot reach it. Measured on the real artefacts, not assumed — the host document of
        /// every <c>_ext</c> wrapper carries zero <c>&lt;svg&gt;</c> elements, and the SVG inside the object
        /// reports <c>ltr</c> with its anchors on the left.
        /// </summary>
        [Fact]
        public void EveryInliningWrapper_PinsTheContainer_AndNoExtHostDocumentCarriesAnSvg()
        {
            var missingPin = new List<string>();
            var svgInExtHost = new List<string>();
            var inlinersWithoutSvg = new List<string>();
            var inliners = 0;
            var externals = 0;

            foreach (var langDir in Directory.EnumerateDirectories(MindmapDir).OrderBy(d => d, StringComparer.Ordinal))
            {
                foreach (var file in Directory.EnumerateFiles(langDir, "*.html").OrderBy(p => p, StringComparer.Ordinal))
                {
                    var name = $"{Path.GetFileName(langDir)}/{Path.GetFileName(file)}";
                    var html = File.ReadAllText(file);
                    var isExt = Path.GetFileName(file).EndsWith("_ext.html", StringComparison.Ordinal);

                    if (isExt)
                    {
                        externals++;
                        if (html.Contains("<svg", StringComparison.Ordinal))
                            svgInExtHost.Add(name);
                        continue;
                    }

                    inliners++;
                    if (!MindmapContainerRule(html).Contains("direction: ltr", StringComparison.Ordinal))
                        missingPin.Add(name);
                    if (!html.Contains("<svg", StringComparison.Ordinal))
                        inlinersWithoutSvg.Add(name);
                }
            }

            inliners.Should().Be(17, "the tree ships 17 inlining wrappers (8 Fallacies + 8 Virtues + 1 FR cards)");
            externals.Should().Be(17, "the tree ships the same 17 documents again as _ext wrappers");

            missingPin.Should().BeEmpty(
                "every inlining wrapper embeds the SVG in its own document, so every one of them must pin " +
                "the container to ltr — the template change is only real once it is re-derived onto the tree");

            inlinersWithoutSvg.Should().BeEmpty(
                "an inlining wrapper with no <svg> would mean the [SVGCONTENT] token silently resolved to " +
                "nothing, and the pin above would then be guarding an empty document");

            svgInExtHost.Should().BeEmpty(
                "an external wrapper carrying an <svg> in its own document would inherit the document's " +
                "direction and need the pin too; today the SVG lives only inside the <object>");
        }

        /// <summary>
        /// The title pin, in two halves. First the artefact must carry the title its creator config
        /// declares (catching a hand-edited wrapper or a template change not re-derived onto the
        /// tree). Then the title must actually be in its own language — a table filled with the
        /// English string for all eight languages would satisfy the first half perfectly.
        /// </summary>
        [Fact]
        public void EveryCommittedWrapper_CarriesItsDeclaredLocalizedTitle()
        {
            var offenders = new List<string>();
            var englishTitles = new Dictionary<string, string>(StringComparer.Ordinal);

            var wrappers = CommittedWrappers();
            foreach (var w in wrappers.Where(w => w.Lang == "en"))
                englishTitles[w.SvgName] = w.Title;

            foreach (var wrapper in wrappers)
            {
                var name = $"{wrapper.Lang}/{Path.GetFileName(wrapper.Path)}";
                var declared = DeclaredTitle(File.ReadAllText(wrapper.Path));

                if (declared != wrapper.Title)
                    offenders.Add($"{name}: title is \"{declared}\" but the config declares \"{wrapper.Title}\"");

                if (wrapper.Lang == "en") continue;

                if (englishTitles.TryGetValue(wrapper.SvgName, out var en) && declared == en)
                    offenders.Add($"{name}: title is the English one (\"{en}\") — the wrapper is not localized");

                switch (wrapper.Lang)
                {
                    case "ru" when CountInRange(declared, 'Ѐ', 'ӿ') == 0:
                        offenders.Add($"{name}: title \"{declared}\" carries no Cyrillic");
                        break;
                    case "zh" when CountInRange(declared, '一', '鿿') == 0:
                        offenders.Add($"{name}: title \"{declared}\" carries no CJK ideograph");
                        break;
                    case "ar" when CountInRange(declared, '؀', 'ۿ') == 0:
                        offenders.Add($"{name}: title \"{declared}\" carries no Arabic-script letter");
                        break;
                    case "fa" when CountInRange(declared, '؀', 'ۿ') == 0:
                        offenders.Add($"{name}: title \"{declared}\" carries no Arabic-script letter");
                        break;
                }
            }

            offenders.Should().BeEmpty(
                "every wrapper must title itself in its own language; before T4b all 36 said " +
                "\"Taxonomy Mind Map\" in English whatever their language");
        }

        /// <summary>
        /// The artefact pin: each committed wrapper must be byte-identical to what the fixed generator
        /// produces from the committed template plus the config-declared title. This is what makes the
        /// tree state and the generator state provably the same object — a hand-edited wrapper, or a
        /// template change that was not re-derived onto the tree, fails here by name.
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
                    wrapper.Lang,
                    wrapper.Title);

                var actual = File.ReadAllText(wrapper.Path);
                if (expected != actual)
                {
                    offenders.Add($"{wrapper.Lang}/{Path.GetFileName(wrapper.Path)}: " +
                                  FirstDivergence(expected, actual));
                }
            }

            offenders.Should().BeEmpty(
                "a committed wrapper must equal FormatWrapper(template, svg, language, title) byte for " +
                "byte — otherwise the tree carries a wrapper the generator would not reproduce");
        }

        /// <summary>
        /// Inverse controls: the derivation check above must be able to FAIL, on each of the three
        /// head tokens. Without this, a check that silently compared a file to itself would look
        /// identical to a passing one.
        /// </summary>
        [Fact]
        public void InverseControl_ADivergentHeadToken_FailsTheDerivationCheck()
        {
            var wrapper = CommittedWrappers().First(w => w.Lang == "fr" && w.SvgContent is not null);
            var includedTemplate = File.ReadAllText(IncludedTemplatePath);

            string Derive(string lang, string title) =>
                MindMapHtmlWrapper.FormatWrapper(includedTemplate, wrapper.SvgName, wrapper.SvgContent, lang, title);

            var faithful = Derive(wrapper.Lang, wrapper.Title);

            faithful.Should().Be(File.ReadAllText(wrapper.Path),
                "pre-condition: this pair must be faithful, otherwise the control proves nothing");

            var otherLang = Derive("en", wrapper.Title);
            otherLang.Should().NotBe(faithful,
                "re-deriving with another language MUST change the bytes — if it did not, the language " +
                "token would be inert and this whole organ would be vacuous");
            otherLang.Should().Contain("lang=\"en\"");
            otherLang.Should().Contain("dir=\"ltr\"");

            var otherTitle = Derive(wrapper.Lang, "Taxonomy Mind Map");
            otherTitle.Should().NotBe(faithful,
                "re-deriving with another title MUST change the bytes — if it did not, the title token " +
                "would be inert and the T4b half of this organ would be vacuous");
            otherTitle.Should().Contain("<title>Taxonomy Mind Map</title>");

            var otherDirection = MindMapHtmlWrapper.FormatWrapper(
                includedTemplate.Replace("dir=\"[DIRECTION]\"", "dir=\"[DIRECTION]\" "), // keep the shape, change nothing
                wrapper.SvgName, wrapper.SvgContent, wrapper.Lang, wrapper.Title);
            otherDirection.Should().NotBe(faithful,
                "touching the template around the direction token must change the bytes, so the byte " +
                "comparison is really reading the file and not a cached value");
        }
    }
}
