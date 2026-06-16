using System.Linq;
using Argumentum.AssetConverter;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// Regression suite for the MindMap localization mechanism — <see cref="DocumentLocalization"/>.
    /// The pipeline localizes each Fallacy mind map expression by running
    /// <see cref="DocumentLocalization.DoReflectionTranslate"/> over the
    /// <c>LocalizationConfig.MindMapLocalization</c> entries, which in turn calls
    /// <see cref="DocumentLocalization.DoStaticConversions"/>: a naive
    /// <c>template.Replace(sourceText, destText)</c> per language. There is no token-boundary
    /// awareness — it is plain substring replacement, the same fragility class as the
    /// card-set localization mapping pinned by #477 and the CSV contract pinned by #488.
    ///
    /// The existing <c>MindMapLocalization_ShouldTranslateAllFallacyExpressions</c> theory in
    /// <c>MmGeneratorTests</c> covers only the happy path for en/ru/pt, where each default
    /// expression happens to carry exactly ONE family keyword — so it never exercises the
    /// partial-match interaction the config itself warns about. This file pins the two genuine
    /// fragile zones that coverage misses:
    ///
    /// 1. The ORDERING CONTRACT (the headline). The family-hierarchy
    ///    <c>DocumentLocalization</c> in <c>AssetConverterConfig</c> carries the comment
    ///    "Order: most specific first (Soussousfamille > SousFamille > Famille) to avoid partial
    ///    matches" — because <c>Famille</c> is a substring of <c>SousFamille</c>, running the
    ///    less-specific conversion first corrupts the more-specific one. No test pins this.
    /// 2. The 8-language release SCOPE. The config declares 7 target languages
    ///    (en/ru/pt/es/ar/fa/zh); the existing theory only asserts en/ru/pt. This file closes the
    ///    gap for es/ar/fa/zh so a dropped conversion surfaces immediately.
    ///
    /// Additive only: no production code or existing test is modified. Dispatch #204 secondaire.
    /// </summary>
    public class MindMapLocalizationRegressionTests
    {
        /// <summary>
        /// Fresh default config — its <c>MindMapLocalization</c> initializer is the real production
        /// entry list (4 entries: Fallacy text fields, Fallacy family hierarchy, Virtue title,
        /// DocumentName). Constructed per-test so localization mutations never leak across tests.
        /// </summary>
        private static AssetConverterConfig FreshConfig => new AssetConverterConfig();

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) THE HEADLINE — the documented most-specific-first ordering contract.
        //     DoStaticConversions is naive substring Replace. "Famille" is a substring of
        //     "SousFamille", so if the StaticConversions list were ever reordered less-specific
        //     first, a template mentioning both would have its SousFamille corrupted into
        //     "SousFamily" before the SousFamille conversion could fire. The config comment
        //     documents the mitigation (order); this test pins it. Runs the REAL family-hierarchy
        //     DocumentLocalization (located by its TargetProperties, not by fragile list index) on
        //     a crafted template carrying all three family keywords, for every target language.
        // ─────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Locates the REAL family-hierarchy DocumentLocalization from the default config by what
        /// it targets (robust to list reordering), rather than by positional index.
        /// </summary>
        private static DocumentLocalization FamilyHierarchyLocalization =>
            FreshConfig.LocalizationConfig.MindMapLocalization
                .First(l => l.TargetProperties.Contains(nameof(FallacyMindMapDocumentConfig.FamilleExpression)));

        [Theory]
        // lang, expectedSoussousfamille, expectedSousFamille, expectedFamille
        [InlineData("en", "Subsubfamily", "Subfamily", "Family")]
        [InlineData("ru", "SubsubfamilyRu", "SubfamilyRu", "FamilyRu")]
        [InlineData("pt", "SubsubfamilyPt", "SubfamilyPt", "FamilyPt")]
        [InlineData("es", "SubsubfamilyEs", "SubfamilyEs", "FamilyEs")]
        [InlineData("ar", "SubsubfamilyAr", "SubfamilyAr", "FamilyAr")]
        [InlineData("fa", "SubsubfamilyFa", "SubfamilyFa", "FamilyFa")]
        [InlineData("zh", "SubsubfamilyZh", "SubfamilyZh", "FamilyZh")]
        public void FamilyHierarchy_LocalizationOrderingPreventsPartialMatchContamination(
            string lang, string expectedSoussousfamille, string expectedSousFamille, string expectedFamille)
        {
            // A single template carrying all three family keywords at once. This is the
            // configuration the ordering comment is written for: if "Famille" were converted
            // before "SousFamille", the middle token would become "SousFamily" (corrupted) and the
            // SousFamille conversion would then no longer match.
            const string template = "{item.Soussousfamille} / {item.SousFamille} / {item.Famille}";

            var result = FamilyHierarchyLocalization.DoStaticConversions(template, lang);

            // Each keyword resolved to its distinct localized property name — no cross-talk.
            result.Should().Contain($"{{item.{expectedSoussousfamille}}}",
                $"Soussousfamille must map to {expectedSoussousfamille} for lang '{lang}'");
            result.Should().Contain($"{{item.{expectedSousFamille}}}",
                $"SousFamille must map to {expectedSousFamille} for lang '{lang}'");
            result.Should().Contain($"{{item.{expectedFamille}}}",
                $"Famille must map to {expectedFamille} for lang '{lang}'");

            // The corruption signature of a broken ordering: "SousFamily" (Famille converted the
            // Famille substring inside SousFamille before SousFamille got its own conversion).
            // Asserting its absence is the direct pin of the documented ordering contract.
            result.Should().NotContain("SousFamily",
                "if Famille were converted before SousFamille, 'SousFamille' would become 'SousFamily' " +
                "(corrupted) — the documented most-specific-first ordering prevents this partial match");

            // No French source keyword should survive localization for a non-fr language.
            result.Should().NotContain("Famille}",
                "every family keyword must be localized away for lang '{0}' — a leftover FR token would " +
                "mean a conversion did not fire");
            result.Should().NotContain("SousFamille}", "the SousFamille token must be fully localized");
            result.Should().NotContain("Soussousfamille}", "the Soussousfamille token must be fully localized");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) SCOPE — the four target languages the existing MmGeneratorTests theory does NOT
        //     cover (es/ar/fa/zh). The release targets 8 languages; the config declares 7 target
        //     langs (en/ru/pt/es/ar/fa/zh); the existing theory asserts only en/ru/pt. These four
        //     rows prove the config actually carries the es/ar/fa/zh StaticConversions and that
        //     they fire for every Fallacy expression. A dropped conversion (e.g. someone trims the
        //     config back to en/ru/pt) surfaces here.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        // lang, text, desc, example, link, famille, sousFamille, soussousFamille
        [InlineData("es", "TextEs", "DescEs", "ExampleEs", "LinkEsFallback", "FamilyEs", "SubfamilyEs", "SubsubfamilyEs")]
        [InlineData("ar", "TextAr", "DescAr", "ExampleAr", "LinkArFallback", "FamilyAr", "SubfamilyAr", "SubsubfamilyAr")]
        [InlineData("fa", "TextFa", "DescFa", "ExampleFa", "LinkFaFallback", "FamilyFa", "SubfamilyFa", "SubsubfamilyFa")]
        [InlineData("zh", "TextZh", "DescZh", "ExampleZh", "LinkZhFallback", "FamilyZh", "SubfamilyZh", "SubsubfamilyZh")]
        public void FallacyExpressions_LocalizeForGapLanguages_es_ar_fa_zh(
            string lang, string expectedText, string expectedDesc, string expectedExample,
            string expectedLink, string expectedFamille, string expectedSousFamille, string expectedSoussousFamille)
        {
            // Fresh config — apply ALL MindMapLocalization entries exactly like the pipeline does
            // (mirrors MindMapLocalization_ShouldTranslateAllFallacyExpressions, for the gap langs).
            var config = new FallacyMindMapDocumentConfig();
            foreach (var localization in FreshConfig.LocalizationConfig.MindMapLocalization)
            {
                localization.DoReflectionTranslate(config, lang);
            }

            // Text fields (the first DocumentLocalization entry targets these five expressions).
            config.TitleExpression.Should().Contain(expectedText,
                $"TitleExpression must reference {expectedText} for gap lang '{lang}'");
            config.DescriptionExpression.Should().Contain(expectedDesc,
                $"DescriptionExpression must reference {expectedDesc} for gap lang '{lang}'");
            config.ExampleExpression.Should().Contain(expectedExample,
                $"ExampleExpression must reference {expectedExample} for gap lang '{lang}'");
            config.LinkExpression.Should().Contain(expectedLink,
                $"LinkExpression must reference {expectedLink} for gap lang '{lang}'");

            // Family hierarchy (the second entry targets these three expressions).
            config.FamilleExpression.Should().Contain(expectedFamille,
                $"FamilleExpression must reference {expectedFamille} for gap lang '{lang}'");
            config.SousFamilleExpression.Should().Contain(expectedSousFamille,
                $"SousFamilleExpression must reference {expectedSousFamille} for gap lang '{lang}'");
            config.SoussousFamilleExpression.Should().Contain(expectedSoussousFamille,
                $"SoussousFamilleExpression must reference {expectedSoussousFamille} for gap lang '{lang}'");
        }
    }
}
