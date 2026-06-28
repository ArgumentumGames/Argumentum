using System.Linq;
using System.Reflection;
using Argumentum.AssetConverter;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// CHARACTERIZATION (Golden-Master) suite pinning the CURRENT behaviour of the Virtues mind map
    /// localization — which is that Virtues <c>.content.svg</c> are FR-frozen across all 8 release
    /// languages, in stark contrast to Fallacies (localized 8-lang, pinned by
    /// <see cref="MindMapLocalizationRegressionTests"/>).
    ///
    /// This is NOT a regression test of desired behaviour. It is an executable snapshot of the
    /// behaviour verified in investigation <c>docs/investigations/2025-06-25-virtues-mindmap-fr-frozen-mechanism.md</c>
    /// (#601, merged at <c>71033f8c</c>): the Virtues mind map pipeline renders French node text for
    /// every language because of a 2-layer gap —
    ///   <b>Layer B (config)</b>: the Virtues <c>MindMapLocalization</c> entry is a stale stub that
    ///   only rewrites the tree-root literal <c>"Vertus"</c>, for 4 of 7 target languages.
    ///   <b>Layer A (entity)</b>: <see cref="Virtue"/> exposes no <c>TitleAr/TitleFa/TitleZh</c>
    ///   properties (the CSV columns exist post-#590/#595 but are invisible to the mind map).
    ///
    /// Each test below is GREEN today and is a GUARDRAIL: it is expected to FAIL when the 3-step fix
    /// documented in #601 lands (entity ar/fa/zh props + <c>{item.TitleFr}</c> expression + mirrored
    /// config table). When that happens, flip these assertions to the localized expectation (mirroring
    /// <see cref="MindMapLocalizationRegressionTests"/>) rather than silently deleting them — the
    /// failure is the signal that the FR-frozen gap is closed.
    ///
    /// Additive only: no production code or existing test is modified. 0 write under <c>Cards/</c>
    /// (release freeze). Dispatch ai-01 2026-06-27, primary (characterization test, GO).
    /// </summary>
    public class VirtueMindMapFRFrozenCharacterizationTests
    {
        /// <summary>
        /// Fresh default config — its <c>MindMapLocalization</c> initializer is the real production
        /// entry list (4 entries: Fallacy text, Fallacy hierarchy, Virtue root title, DocumentName).
        /// Constructed per-test so localization mutations never leak across tests.
        /// </summary>
        private static AssetConverterConfig FreshConfig => new AssetConverterConfig();

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) THE HEADLINE — Virtues mind map node text is FR-frozen. The pipeline localizes a
        //     document config by applying every MindMapLocalization entry via
        //     DocumentLocalization.DoReflectionTranslate. For Fallacies this rewrites the title
        //     expression to {fallacy.TextZh} etc.; for Virtues it is a NO-OP — the only conversion
        //     that targets VirtueMindMapDocumentConfig.TitleExpression looks for the literal "Vertus"
        //     (the tree-root word), which is NOT present in "{item.Text}". So the expression survives
        //     localization untouched, and at render time TitleFunc evaluates {item.Text} = Virtue.Text
        //     = TitleFr = French, for every target language. This test pins that no-op for all 7
        //     non-FR release languages. GUARDRAIL: fails when the #601 fix changes
        //     DefaultTitleExpression to {item.TitleFr} and mirrors the conversion table.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        // The 7 non-FR release languages. (es is declared but, like ar/fa/zh, cannot fire either —
        // the conversion's sourceText "Vertus" is absent from "{item.Text}" regardless of language.)
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        [InlineData("es")]
        [InlineData("ar")]
        [InlineData("fa")]
        [InlineData("zh")]
        public void VirtueTitleExpression_RemainsFrozenFRAfterLocalizationForEveryNonFrLang(string lang)
        {
            // Fresh Virtue config — its TitleExpression defaults to "{item.Text}" (see
            // VirtueMindMapDocumentConfig.DefaultTitleExpression).
            var config = new VirtueMindMapDocumentConfig();

            // Apply ALL MindMapLocalization entries exactly like the pipeline does (mirrors
            // MindMapLocalizationRegressionTests.FallacyExpressions_LocalizeForGapLanguages_es_ar_fa_zh).
            foreach (var localization in FreshConfig.LocalizationConfig.MindMapLocalization)
            {
                localization.DoReflectionTranslate(config, lang);
            }

            // CHARACTERIZATION: the expression is UNCHANGED — localization is a silent no-op on
            // "{item.Text}". This is exactly why the rendered .content.svg carries Virtue.Text =
            // TitleFr = French for every language.
            config.TitleExpression.Should().Be("{item.Text}",
                $"Virtue TitleExpression must stay FR-frozen ({{item.Text}}) for lang '{lang}' — the " +
                $"production Virtue conversion is a root-literal stub that cannot rewrite {{item.Text}}. " +
                $"When #601's fix lands ({'{'}item.TitleFr{'}'} + TitleFr→Title{lang} conversion), this " +
                "assertion is EXPECTED to fail and should be updated to the localized expression.");

            // Negative pin: the expression must NOT carry any per-language title token — that is the
            // signature of a Fallacies-style localization, which Virtues do not have today.
            config.TitleExpression.Should().NotContain($"Title{char.ToUpper(lang[0])}{lang.Substring(1)}",
                $"the FR-frozen expression must not reference a localized Title property for '{lang}' — " +
                "if it did, Layer A/B would already be fixed and this characterization is stale");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) ROOT CAUSE — LAYER B (config): the single MindMapLocalization entry that targets Virtues
        //     is a stale stub. It only rewrites the tree-root literal "Vertus" (NOT a Virtue entity
        //     field), and only for en/ru/pt/es — ar/fa/zh are absent entirely. This documents WHY (1)
        //     is a no-op even for the 4 covered languages (the substring "Vertus" is not in "{item.Text}"),
        //     and why ar/fa/zh could not localize even if the expression matched. GUARDRAIL: fails when
        //     #601 step 3 replaces this stub with a TitleFr→TitleZh mirror of the Fallacy text entry.
        // ─────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Locates the REAL Virtues-root DocumentLocalization from the default config. NB: it is NOT
        /// located by <c>TargetProperties.Contains(nameof(VirtueMindMapDocumentConfig.TitleExpression))</c>,
        /// because <c>nameof(...TitleExpression)</c> returns the bare property name "TitleExpression",
        /// which is SHARED with the Fallacy text entry (#1) — that entry also lists "TitleExpression"
        /// among its 5 target properties, so a name-based lookup returns #1, not #3. The unambiguous
        /// discriminator is the conversion whose <c>sourceText</c> is the tree-root literal "Vertus",
        /// which exists only in the Virtues entry. (Robust to list reordering.)
        /// </summary>
        private static DocumentLocalization VirtueRootTitleLocalization =>
            FreshConfig.LocalizationConfig.MindMapLocalization
                .First(l => l.StaticConversions.Any(c => c.sourceText == "Vertus"));

        [Fact]
        public void VirtueLocalizationEntry_IsStaleStub_RootLiteralOnly_4Langs_NoArFaZh()
        {
            var entry = VirtueRootTitleLocalization;

            // The entry exists and targets exactly the Virtue title expression.
            entry.TargetProperties.Should().ContainSingle(
                "the production config carries exactly one Virtue-targeting localization entry");

            // CHARACTERIZATION: the stub has exactly ONE conversion, and it rewrites the tree-root
            // LITERAL "Vertus" — not a Virtue.* entity property name. This is the root-only design
            // documented by the config comment "data is FR-only, only tree root name changes" (#601
            // Layer B). It cannot localize node titles because node titles come from {item.Text}.
            entry.StaticConversions.Should().ContainSingle(
                "the Virtue stub is a single root-literal conversion, not a per-field table");
            var conversion = entry.StaticConversions.Single();
            conversion.sourceText.Should().Be("Vertus",
                "the stub rewrites the tree-root literal 'Vertus', not a Virtue entity field — so it " +
                "cannot touch node titles sourced from {item.Text}");

            // CHARACTERIZATION: only 4 of 7 target languages are covered. ar/fa/zh are entirely
            // absent — even the tree root would stay "Vertus" (French) for those three languages.
            var coveredLangs = conversion.textConversions.Select(c => c.Language).ToArray();
            coveredLangs.Should().BeEquivalentTo(new[] { "en", "ru", "pt", "es" },
                "the stub covers only en/ru/pt/es — ar/fa/zh are absent, so the tree root itself stays " +
                "FR for those languages today");
            coveredLangs.Should().NotContain(new[] { "ar", "fa", "zh" },
                "ar/fa/zh have no conversion at all in the Virtue stub (Layer B gap)");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) ROOT CAUSE — LAYER A (entity): Virtue maps only TitleFr/En/Ru/Pt/Es — there is no
        //     TitleAr/TitleFa/TitleZh property to bind {item.TitleAr} etc. against. Additionally the
        //     convenience accessor Virtue.Text hard-delegates to TitleFr, which is why {item.Text}
        //     (the current default expression) yields French for every language. GUARDRAIL: fails when
        //     #601 step 1 adds the ar/fa/zh title properties (and/or step 2 makes Text language-aware).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void VirtueEntity_LacksArFaZhTitleProperties_AndTextDelegatesToFrozenFr()
        {
            var virtueType = typeof(Virtue);

            // CHARACTERIZATION: the ar/fa/zh title properties do NOT exist on the entity. The CSV
            // columns (title_ar/title_fa/title_zh) were added by #590/#595 but are invisible to the
            // mind map pipeline because nothing maps them. This is Layer A of the #601 root cause.
            virtueType.GetProperty("TitleAr").Should().BeNull(
                "Virtue exposes no TitleAr property — the ar CSV column is invisible to the mind map");
            virtueType.GetProperty("TitleFa").Should().BeNull(
                "Virtue exposes no TitleFa property — the fa CSV column is invisible to the mind map");
            virtueType.GetProperty("TitleZh").Should().BeNull(
                "Virtue exposes no TitleZh property — the zh CSV column is invisible to the mind map");

            // Sanity: the FR/EN/RU/PT/ES title properties DO exist (so the gap is specifically ar/fa/zh,
            // not a wholesale absence of localization columns).
            virtueType.GetProperty("TitleFr").Should().NotBeNull("TitleFr is the FR source-of-truth");
            virtueType.GetProperty("TitleEn").Should().NotBeNull("TitleEn exists (en is mapped)");
            virtueType.GetProperty("TitleRu").Should().NotBeNull("TitleRu exists (ru is mapped)");
            virtueType.GetProperty("TitlePt").Should().NotBeNull("TitlePt exists (pt is mapped)");
            virtueType.GetProperty("TitleEs").Should().NotBeNull("TitleEs exists (es is mapped)");

            // CHARACTERIZATION: the default expression {item.Text} resolves to TitleFr because the
            // Text accessor is hard-wired to the French title. This is the direct mechanism by which
            // the FR-frozen node text reaches the .content.svg.
            var virtue = new Virtue { TitleFr = "Échange enrichissant" };
            virtue.Text.Should().Be("Échange enrichissant",
                "Virtue.Text delegates to TitleFr — the default expression {item.Text} therefore " +
                "renders the French title regardless of the target language");

            virtue.TitleEn = "Enriching exchange";
            virtue.Text.Should().Be("Échange enrichissant",
                "even when an EN translation is present, Virtue.Text ignores it — Text is not " +
                "language-aware, it always returns TitleFr");
        }
    }
}
