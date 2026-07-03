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
    /// The characterization suite was an executable GUARDRAIL, expected to FAIL when the fix landed,
    /// with an explicit instruction to "flip these assertions to the localized expectation (mirroring
    /// <see cref="MindMapLocalizationRegressionTests"/>) rather than silently deleting them".
    ///
    /// #636 §2 landed that fix, but PARTIALLY, per jsboige's arbitration (wire the 4 languages whose
    /// data + entity binding already exist; defer the 3 that need an entity extension):
    ///   • Layer B — the stub is replaced by two real per-field conversion tables mirroring Fallacies
    ///     (title/description + family hierarchy), sourced from the FR-suffixed <c>Virtue.*Fr</c>
    ///     properties, covering <b>en/ru/pt/es</b> only.
    ///   • Layer A — the Virtue mind-map expressions now reference the raw FR-suffixed props
    ///     (<c>{item.TitleFr}</c>, <c>{item.FamilyFr}</c>, …) so the StaticConversions can rewrite them.
    ///   • Ar/Fa/Zh are DEFERRED: <see cref="Virtue"/> still has no Title/Family Ar/Fa/Zh property, so
    ///     those languages keep rendering FR until the entity is extended (a follow-up chantier).
    ///
    /// This suite pins that mixed end-state: localized for the 4 wired languages, FR for the 3 deferred
    /// ones, and the entity boundary that draws the line between them.
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
        // (1) WIRED LANGUAGES — en/ru/pt/es. After localization the title and description
        //     expressions reference the per-language Virtue property, and the FR source token is
        //     fully rewritten. This is the flipped counterpart of the old
        //     VirtueTitleExpression_RemainsFrozenFRAfterLocalizationForEveryNonFrLang guardrail.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        // lang, expectedTitle, expectedDescription
        [InlineData("en", "TitleEn", "DescriptionEn")]
        [InlineData("ru", "TitleRu", "DescriptionRu")]
        [InlineData("pt", "TitlePt", "DescriptionPt")]
        [InlineData("es", "TitleEs", "DescriptionEs")]
        public void VirtueTitleAndDescription_LocalizeForWiredLanguages_en_ru_pt_es(
            string lang, string expectedTitle, string expectedDescription)
        {
            var config = LocalizedVirtueConfig(lang);

            config.TitleExpression.Should().Be($"{{item.{expectedTitle}}}",
                $"the Virtue title expression must be rewritten from {{item.TitleFr}} to {{item.{expectedTitle}}} " +
                $"for the wired lang '{lang}' (#636 §2 Layer B table)");
            config.TitleExpression.Should().NotContain("TitleFr",
                $"the FR source token must be fully rewritten for wired lang '{lang}'");

            config.DescriptionExpression.Should().Contain(expectedDescription,
                $"the Virtue description expression must reference {expectedDescription} for wired lang '{lang}'");
            config.DescriptionExpression.Should().NotContain("DescriptionFr",
                $"the FR description token must be fully rewritten for wired lang '{lang}'");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) WIRED LANGUAGES — family hierarchy. Same four languages; pins the most-specific-first
        //     ordering (Subsubfamily > Subfamily > Family) so no partial-match cross-talk, mirroring
        //     the Fallacy FamilyHierarchy_LocalizationOrderingPreventsPartialMatchContamination test.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        // lang, expectedFamily, expectedSubfamily, expectedSubsubfamily
        [InlineData("en", "FamilyEn", "SubfamilyEn", "SubsubfamilyEn")]
        [InlineData("ru", "FamilyRu", "SubfamilyRu", "SubsubfamilyRu")]
        [InlineData("pt", "FamilyPt", "SubfamilyPt", "SubsubfamilyPt")]
        [InlineData("es", "FamilyEs", "SubfamilyEs", "SubsubfamilyEs")]
        public void VirtueFamilyHierarchy_LocalizesForWiredLanguages_en_ru_pt_es(
            string lang, string expectedFamily, string expectedSubfamily, string expectedSubsubfamily)
        {
            var config = LocalizedVirtueConfig(lang);

            config.FamilleExpression.Should().Be($"{{item.{expectedFamily}}}",
                $"Famille must localize to {expectedFamily} for wired lang '{lang}'");
            config.SousFamilleExpression.Should().Be($"{{item.{expectedSubfamily}}}",
                $"SousFamille must localize to {expectedSubfamily} for wired lang '{lang}'");
            config.SoussousFamilleExpression.Should().Be($"{{item.{expectedSubsubfamily}}}",
                $"Soussousfamille must localize to {expectedSubsubfamily} for wired lang '{lang}'");

            // The corruption signature of a broken most-specific-first ordering: the middle token
            // ("SubfamilyFr") is a prefix-family of "SubsubfamilyFr". Order guards it; pin its absence.
            config.SoussousFamilleExpression.Should().NotContain("SubfamilyFr",
                "the Soussousfamille expression must resolve wholly — no leftover intermediate FR token");
            (config.FamilleExpression + config.SousFamilleExpression + config.SoussousFamilleExpression)
                .Should().NotContain("Fr}",
                    $"no FR family token may survive localization for wired lang '{lang}'");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) DEFERRED LANGUAGES — ar/fa/zh. The Virtue conversion tables cover only en/ru/pt/es
        //     (Layer A entity gap), so every Virtue expression stays FR-frozen for these three.
        //     This is the intentional deferral boundary; it flips to localized only once the entity
        //     grows TitleAr/Fa/Zh + Family/… Ar/Fa/Zh properties and the config tables extend.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("ar")]
        [InlineData("fa")]
        [InlineData("zh")]
        public void VirtueExpressions_StayFrForDeferredLanguages_ar_fa_zh(string lang)
        {
            var config = LocalizedVirtueConfig(lang);

            config.TitleExpression.Should().Be("{item.TitleFr}",
                $"Virtue title stays FR for deferred lang '{lang}' — the entity has no Title{lang} property yet");
            config.FamilleExpression.Should().Be("{item.FamilyFr}",
                $"Virtue family stays FR for deferred lang '{lang}'");
            config.SousFamilleExpression.Should().Be("{item.SubfamilyFr}",
                $"Virtue subfamily stays FR for deferred lang '{lang}'");
            config.SoussousFamilleExpression.Should().Be("{item.SubsubfamilyFr}",
                $"Virtue subsubfamily stays FR for deferred lang '{lang}'");
            config.DescriptionExpression.Should().Contain("DescriptionFr",
                $"Virtue description stays FR for deferred lang '{lang}'");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) CONFIG SHAPE — Layer B is no longer a stale root-literal stub. There are now two
        //     Virtue-targeting entries (title/description + family hierarchy), each a real per-field
        //     conversion table sourced from Virtue.*Fr properties, covering en/ru/pt/es. The old
        //     single "Vertus" root-literal conversion is gone.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void VirtueLocalizationEntries_MirrorFallacyTables_NotStaleStub()
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

            // Every Virtue conversion covers exactly en/ru/pt/es — ar/fa/zh deferred (Layer A gap).
            foreach (var entry in new[] { titleEntry, familyEntry })
            {
                foreach (var (_, textConversions) in entry.StaticConversions)
                {
                    textConversions.Select(c => c.Language).Should().BeEquivalentTo(
                        new[] { "en", "ru", "pt", "es" },
                        "the Virtue tables are wired for en/ru/pt/es only; ar/fa/zh are deferred until the entity grows their columns");
                }
            }

            // The retired stale stub: no Virtue localization rewrites the tree-root literal "Vertus".
            mindMapLoc.Should().NotContain(
                l => l.StaticConversions.Any(c => c.sourceText == "Vertus"),
                "the #601 root-literal 'Vertus' stub is superseded by the per-field tables");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) ENTITY BOUNDARY — documents WHY ar/fa/zh are deferred (Layer A). Virtue maps
        //     Fr/En/Ru/Pt/Es titles but no Ar/Fa/Zh; the convenience accessor Text still delegates
        //     to TitleFr. Flips to a wired expectation only when the entity extension chantier lands.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void VirtueEntity_WiresEnRuPtEs_DefersArFaZh()
        {
            var virtueType = typeof(Virtue);

            // Wired languages have their title property (so Layer A is present for en/ru/pt/es).
            virtueType.GetProperty("TitleFr").Should().NotBeNull("TitleFr is the FR source-of-truth");
            virtueType.GetProperty("TitleEn").Should().NotBeNull("TitleEn exists (en is wired)");
            virtueType.GetProperty("TitleRu").Should().NotBeNull("TitleRu exists (ru is wired)");
            virtueType.GetProperty("TitlePt").Should().NotBeNull("TitlePt exists (pt is wired)");
            virtueType.GetProperty("TitleEs").Should().NotBeNull("TitleEs exists (es is wired)");

            // Deferred languages have NO title property yet — the concrete reason ar/fa/zh stay FR.
            virtueType.GetProperty("TitleAr").Should().BeNull(
                "Virtue exposes no TitleAr property yet — ar mind map localization is deferred");
            virtueType.GetProperty("TitleFa").Should().BeNull(
                "Virtue exposes no TitleFa property yet — fa mind map localization is deferred");
            virtueType.GetProperty("TitleZh").Should().BeNull(
                "Virtue exposes no TitleZh property yet — zh mind map localization is deferred");

            // The Text convenience accessor still delegates to the FR title (unchanged by #636 §2 —
            // the fix works through the {item.TitleFr} expression + config table, not by making Text
            // language-aware). Kept as a pin so a future Text change is a conscious decision.
            var virtue = new Virtue { TitleFr = "Échange enrichissant", TitleEn = "Enriching exchange" };
            virtue.Text.Should().Be("Échange enrichissant",
                "Virtue.Text delegates to TitleFr; localization flows through the expression table, not Text");
        }
    }
}
