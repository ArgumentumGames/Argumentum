using System.Linq;
using Argumentum.AssetConverter;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// Regression suite for the Virtues mind map localization — the successor of the #601
    /// FR-frozen characterization suite (<c>VirtueMindMapFRFrozenCharacterizationTests</c>).
    ///
    /// #601 (<c>docs/investigations/2025-06-25-virtues-mindmap-fr-frozen-mechanism.md</c>) root-caused
    /// the Virtues mind map rendering French node text for every language as a 2-layer gap:
    ///   <b>Layer B (config)</b>: the Virtue <c>MindMapLocalization</c> entry was a stale stub that
    ///   only rewrote the tree-root literal <c>"Vertus"</c>.
    ///   <b>Layer A (entity)</b>: <see cref="Virtue"/> exposed no per-language title/family binding for
    ///   the mind-map expressions.
    ///
    /// #636 §2 landed the fix PARTIALLY (wire the 4 languages whose data + entity binding already
    /// exist — en/ru/pt/es — and defer the 3 that needed an entity extension — ar/fa/zh). The
    /// deferral was an executable guardrail with an explicit instruction to "flip these assertions
    /// to the localized expectation rather than silently deleting them" once the entity grew the
    /// Ar/Fa/Zh columns.
    ///
    /// #665 lands that entity extension and completes the wiring for ALL EIGHT languages:
    ///   • Layer A — <see cref="Virtue"/> now exposes Title/Description/Remark/Link/Family/Subfamily/
    ///     Subsubfamily × Ar/Fa/Zh, with ClassMap <c>.Optional()</c> bindings to the CSV
    ///     <c>*_ar/_fa/_zh</c> columns (which were already fully translated, 223/223, native script).
    ///   • Layer B — the two Virtue per-field conversion tables (title/description + family hierarchy)
    ///     now carry ar/fa/zh entries alongside en/ru/pt/es, so the StaticConversions rewrite
    ///     <c>{item.TitleFr}</c> → <c>{item.TitleAr}</c> etc. for every non-FR language.
    ///
    /// This suite now pins the completed end-state: localized for all 7 non-FR languages, FR default
    /// via the FR-suffixed source tokens, and the entity carrying all eight language columns. The
    /// former deferral tests (ar/fa/zh stay FR; entity lacks Ar/Fa/Zh; tables cover en/ru/pt/es only)
    /// are flipped in place to their localized counterparts.
    /// </summary>
    public class VirtueMindMapLocalizationTests
    {
        /// <summary>
        /// Fresh default config — its <c>MindMapLocalization</c> initializer is the real production
        /// entry list. Constructed per-test so localization mutations never leak across tests.
        /// </summary>
        private static AssetConverterConfig FreshConfig => new AssetConverterConfig();

        /// <summary>
        /// Applies EVERY production <c>MindMapLocalization</c> entry to a fresh Virtue document config
        /// exactly as the pipeline does (mirrors
        /// <see cref="MindMapLocalizationRegressionTests.FallacyExpressions_LocalizeForGapLanguages_es_ar_fa_zh"/>).
        /// </summary>
        private static VirtueMindMapDocumentConfig LocalizedVirtueConfig(string lang)
        {
            var config = new VirtueMindMapDocumentConfig();
            foreach (var localization in FreshConfig.LocalizationConfig.MindMapLocalization)
            {
                localization.DoReflectionTranslate(config, lang);
            }
            return config;
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) TITLE + DESCRIPTION — all 7 non-FR languages. After localization the title and
        //     description expressions reference the per-language Virtue property, and the FR source
        //     token is fully rewritten. ar/fa/zh were flipped here from the #636 §2 deferral guardrail
        //     once #665 grew their entity columns.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        // lang, expectedTitle, expectedDescription
        [InlineData("en", "TitleEn", "DescriptionEn")]
        [InlineData("ru", "TitleRu", "DescriptionRu")]
        [InlineData("pt", "TitlePt", "DescriptionPt")]
        [InlineData("es", "TitleEs", "DescriptionEs")]
        [InlineData("ar", "TitleAr", "DescriptionAr")]
        [InlineData("fa", "TitleFa", "DescriptionFa")]
        [InlineData("zh", "TitleZh", "DescriptionZh")]
        public void VirtueTitleAndDescription_LocalizeForEveryNonFrLanguage(
            string lang, string expectedTitle, string expectedDescription)
        {
            var config = LocalizedVirtueConfig(lang);

            config.TitleExpression.Should().Be($"{{item.{expectedTitle}}}",
                $"the Virtue title expression must be rewritten from {{item.TitleFr}} to {{item.{expectedTitle}}} " +
                $"for lang '{lang}' (#636 §2 / #665 Layer B table)");
            config.TitleExpression.Should().NotContain("TitleFr",
                $"the FR source token must be fully rewritten for lang '{lang}'");

            config.DescriptionExpression.Should().Contain(expectedDescription,
                $"the Virtue description expression must reference {expectedDescription} for lang '{lang}'");
            config.DescriptionExpression.Should().NotContain("DescriptionFr",
                $"the FR description token must be fully rewritten for lang '{lang}'");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) FAMILY HIERARCHY — all 7 non-FR languages; pins the most-specific-first ordering
        //     (Subsubfamily > Subfamily > Family) so no partial-match cross-talk, mirroring the
        //     Fallacy FamilyHierarchy_LocalizationOrderingPreventsPartialMatchContamination test.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        // lang, expectedFamily, expectedSubfamily, expectedSubsubfamily
        [InlineData("en", "FamilyEn", "SubfamilyEn", "SubsubfamilyEn")]
        [InlineData("ru", "FamilyRu", "SubfamilyRu", "SubsubfamilyRu")]
        [InlineData("pt", "FamilyPt", "SubfamilyPt", "SubsubfamilyPt")]
        [InlineData("es", "FamilyEs", "SubfamilyEs", "SubsubfamilyEs")]
        [InlineData("ar", "FamilyAr", "SubfamilyAr", "SubsubfamilyAr")]
        [InlineData("fa", "FamilyFa", "SubfamilyFa", "SubsubfamilyFa")]
        [InlineData("zh", "FamilyZh", "SubfamilyZh", "SubsubfamilyZh")]
        public void VirtueFamilyHierarchy_LocalizesForEveryNonFrLanguage(
            string lang, string expectedFamily, string expectedSubfamily, string expectedSubsubfamily)
        {
            var config = LocalizedVirtueConfig(lang);

            config.FamilleExpression.Should().Be($"{{item.{expectedFamily}}}",
                $"Famille must localize to {expectedFamily} for lang '{lang}'");
            config.SousFamilleExpression.Should().Be($"{{item.{expectedSubfamily}}}",
                $"SousFamille must localize to {expectedSubfamily} for lang '{lang}'");
            config.SoussousFamilleExpression.Should().Be($"{{item.{expectedSubsubfamily}}}",
                $"Soussousfamille must localize to {expectedSubsubfamily} for lang '{lang}'");

            // The corruption signature of a broken most-specific-first ordering: the middle token
            // ("SubfamilyFr") is a prefix-family of "SubsubfamilyFr". Order guards it; pin its absence.
            config.SoussousFamilleExpression.Should().NotContain("SubfamilyFr",
                "the Soussousfamille expression must resolve wholly — no leftover intermediate FR token");
            (config.FamilleExpression + config.SousFamilleExpression + config.SoussousFamilleExpression)
                .Should().NotContain("Fr}",
                    $"no FR family token may survive localization for lang '{lang}'");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) FORMERLY-DEFERRED LANGUAGES — ar/fa/zh. This is the in-place flip of the old
        //     VirtueExpressions_StayFrForDeferredLanguages_ar_fa_zh guardrail: with #665 the Virtue
        //     conversion tables + entity now cover ar/fa/zh, so every Virtue expression localizes and
        //     no FR-suffixed source token survives.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("ar")]
        [InlineData("fa")]
        [InlineData("zh")]
        public void VirtueExpressions_NoLongerFrozenFr_ForFormerlyDeferredLanguages_ar_fa_zh(string lang)
        {
            var config = LocalizedVirtueConfig(lang);
            var suffix = lang == "ar" ? "Ar" : lang == "fa" ? "Fa" : "Zh";

            config.TitleExpression.Should().Be($"{{item.Title{suffix}}}",
                $"Virtue title now localizes for formerly-deferred lang '{lang}' (#665 entity extension)");
            config.FamilleExpression.Should().Be($"{{item.Family{suffix}}}",
                $"Virtue family now localizes for '{lang}'");
            config.SousFamilleExpression.Should().Be($"{{item.Subfamily{suffix}}}",
                $"Virtue subfamily now localizes for '{lang}'");
            config.SoussousFamilleExpression.Should().Be($"{{item.Subsubfamily{suffix}}}",
                $"Virtue subsubfamily now localizes for '{lang}'");
            config.DescriptionExpression.Should().Contain($"Description{suffix}",
                $"Virtue description now localizes for '{lang}'");

            // No FR-suffixed source token may survive for the formerly-deferred languages.
            (config.TitleExpression + config.DescriptionExpression + config.FamilleExpression
             + config.SousFamilleExpression + config.SoussousFamilleExpression)
                .Should().NotContain("Fr}",
                    $"no FR source token may survive localization for formerly-deferred lang '{lang}'");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) CONFIG SHAPE — the two Virtue-targeting entries (title/description + family hierarchy)
        //     are real per-field conversion tables sourced from Virtue.*Fr properties, and every
        //     conversion now covers all 7 non-FR languages (en/ru/pt/es/ar/fa/zh). The retired #601
        //     single "Vertus" root-literal conversion stays gone.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void VirtueLocalizationEntries_MirrorFallacyTables_CoverAllEightLanguages()
        {
            var mindMapLoc = FreshConfig.LocalizationConfig.MindMapLocalization;

            // The Virtue title/description entry: real per-field table, sourced from Virtue.TitleFr.
            var titleEntry = mindMapLoc.Single(l =>
                l.TargetProperties.Contains(nameof(VirtueMindMapDocumentConfig.TitleExpression))
                && l.StaticConversions.Any(c => c.sourceText == nameof(Virtue.TitleFr)));
            titleEntry.TargetProperties.Should().Contain(nameof(VirtueMindMapDocumentConfig.DescriptionExpression),
                "the Virtue text entry also localizes the description expression");

            // The Virtue family-hierarchy entry: sourced from Virtue.SubsubfamilyFr (most specific).
            var familyEntry = mindMapLoc.Single(l =>
                l.TargetProperties.Contains(nameof(VirtueMindMapDocumentConfig.FamilleExpression))
                && l.StaticConversions.Any(c => c.sourceText == nameof(Virtue.SubsubfamilyFr)));
            familyEntry.TargetProperties.Should().BeEquivalentTo(new[]
            {
                nameof(VirtueMindMapDocumentConfig.FamilleExpression),
                nameof(VirtueMindMapDocumentConfig.SousFamilleExpression),
                nameof(VirtueMindMapDocumentConfig.SoussousFamilleExpression),
            }, "the Virtue family entry mirrors the Fallacy family-hierarchy shape");

            // Every Virtue conversion now covers all 7 non-FR languages — ar/fa/zh wired by #665.
            foreach (var entry in new[] { titleEntry, familyEntry })
            {
                foreach (var (_, textConversions) in entry.StaticConversions)
                {
                    textConversions.Select(c => c.Language).Should().BeEquivalentTo(
                        new[] { "en", "ru", "pt", "es", "ar", "fa", "zh" },
                        "the Virtue tables are wired for all 7 non-FR languages (#665 completed the ar/fa/zh trio)");
                }
            }

            // The retired stale stub: no Virtue localization rewrites the tree-root literal "Vertus".
            mindMapLoc.Should().NotContain(
                l => l.StaticConversions.Any(c => c.sourceText == "Vertus"),
                "the #601 root-literal 'Vertus' stub is superseded by the per-field tables");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) ENTITY BOUNDARY — Virtue now maps all eight languages (Layer A complete). This is the
        //     in-place flip of VirtueEntity_WiresEnRuPtEs_DefersArFaZh: TitleAr/Fa/Zh now exist, which
        //     is the concrete reason ar/fa/zh localize. The convenience accessor Text still delegates
        //     to TitleFr (localization flows through the expression table, not Text).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void VirtueEntity_WiresAllEightLanguages()
        {
            var virtueType = typeof(Virtue);

            // FR source-of-truth + the four previously-wired languages.
            virtueType.GetProperty("TitleFr").Should().NotBeNull("TitleFr is the FR source-of-truth");
            virtueType.GetProperty("TitleEn").Should().NotBeNull("TitleEn exists (en is wired)");
            virtueType.GetProperty("TitleRu").Should().NotBeNull("TitleRu exists (ru is wired)");
            virtueType.GetProperty("TitlePt").Should().NotBeNull("TitlePt exists (pt is wired)");
            virtueType.GetProperty("TitleEs").Should().NotBeNull("TitleEs exists (es is wired)");

            // The formerly-deferred trio now has its title property — the concrete reason ar/fa/zh localize.
            virtueType.GetProperty("TitleAr").Should().NotBeNull(
                "Virtue now exposes TitleAr — ar mind map localization is wired (#665)");
            virtueType.GetProperty("TitleFa").Should().NotBeNull(
                "Virtue now exposes TitleFa — fa mind map localization is wired (#665)");
            virtueType.GetProperty("TitleZh").Should().NotBeNull(
                "Virtue now exposes TitleZh — zh mind map localization is wired (#665)");

            // Full-family coverage for the formerly-deferred trio (family hierarchy also localizes).
            virtueType.GetProperty("FamilyAr").Should().NotBeNull("FamilyAr exists (ar family hierarchy wired)");
            virtueType.GetProperty("SubfamilyFa").Should().NotBeNull("SubfamilyFa exists (fa family hierarchy wired)");
            virtueType.GetProperty("SubsubfamilyZh").Should().NotBeNull("SubsubfamilyZh exists (zh family hierarchy wired)");

            // The Text convenience accessor still delegates to the FR title — localization flows through
            // the {item.TitleFr} expression + config table, not by making Text language-aware. Kept as a
            // pin so a future Text change is a conscious decision.
            var virtue = new Virtue { TitleFr = "Échange enrichissant", TitleEn = "Enriching exchange" };
            virtue.Text.Should().Be("Échange enrichissant",
                "Virtue.Text delegates to TitleFr; localization flows through the expression table, not Text");
        }
    }
}
