using System.Collections.Generic;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="LocalizationConfig.BuildLanguageList"/> — #204 coverage sweep
    /// (cont. po-2024): the language-set decision contract for the entire asset-generation pipeline.
    ///
    /// <see cref="LocalizationConfig.BuildLanguageList"/> decides WHICH languages the pipeline
    /// processes. It is called from <c>HarvestManager</c> (per-CardSet) and
    /// <c>ParallelDocumentCreatorConfigBase</c> (per-mind-map), each driving a
    /// <c>Parallel.ForEachAsync</c> over the returned list — so the return value is the single
    /// decision point for "how many localized outputs ship for this CardSet / mind-map".
    ///
    /// The contract has three observable branches, each a silent-corruption hazard if regressed:
    /// <list type="bullet">
    /// <item><c>Enabled == false</c> → returns ONLY <see cref="LocalizationConfig.DefaultLanguage"/>
    /// (the base language, default "fr"). A regression that ignored the gate would leak target
    /// languages even when localization is off, producing empty/partial outputs for locales whose
    /// templates or data aren't ready.</item>
    /// <item><c>Enabled == true</c> → returns <c>DefaultLanguage</c> FOLLOWED BY every
    /// translation's <c>targetLanguage</c>. A regression that dropped the default would lose the
    /// base-language output; one that dropped the <c>AddRange</c> would lose every translation.</item>
    /// <item>The default language is ALWAYS first, even when enabled — base language is generated
    /// before translations (call sites rely on this ordering for harvest caching/overwrite).</item>
    /// </list>
    ///
    /// The method reads only two in-memory mutable config properties (<c>DefaultLanguage</c>,
    /// <c>Enabled</c>) and the input list — no File/HTTP/Playwright I/O, no randomness, no time —
    /// so it is deterministic given the object state and trivially unit-testable by constructing a
    /// <see cref="LocalizationConfig"/> with controlled property values. (An earlier sweep flagged
    /// it "config-dependent"; that is overly cautious — config *state* is not I/O, and the method
    /// is pure with respect to its inputs.) It had ZERO unit coverage. These tests pin the contract
    /// additively (no production code changed).
    /// </summary>
    public class BuildLanguageListContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // (1) DISABLED — the localization gate. Only the default language is returned, regardless
        //     of how many translations are configured. This is the guard that prevents the pipeline
        //     from generating empty/partial outputs for locales whose templates or data aren't ready.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Disabled_ReturnsOnlyDefaultLanguage_IgnoresTranslations()
        {
            var config = new LocalizationConfig { Enabled = false, DefaultLanguage = "fr" };
            var translations = new List<(string sourceLanguage, string targetLanguage)>
            {
                ("fr", "en"), ("fr", "ru"), ("fr", "pt"),
            };

            var result = config.BuildLanguageList(translations);

            result.Should().Equal(new[] { "fr" },
                "when Enabled==false the localization gate is closed: only the default language is " +
                "processed, no matter how many translations are configured — guarding against partial/" +
                "empty outputs for un-ready locales.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) ENABLED — the default language FIRST, then every translation's target language, in
        //     input order. This is the happy path: the full localized set. Pinned so a refactor
        //     cannot drop the default (base output lost) or drop the AddRange (translations lost).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Enabled_ReturnsDefaultFirst_ThenTranslationTargetsInOrder()
        {
            var config = new LocalizationConfig { Enabled = true, DefaultLanguage = "fr" };
            var translations = new List<(string sourceLanguage, string targetLanguage)>
            {
                ("fr", "en"), ("fr", "ru"), ("fr", "pt"),
            };

            var result = config.BuildLanguageList(translations);

            result.Should().Equal(new[] { "fr", "en", "ru", "pt" },
                "when Enabled==true the default language is emitted first, followed by every " +
                "translation's target language in input order — the full localized output set, " +
                "base language before translations.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) DEFAULT LANGUAGE ORDERING — the base language is ALWAYS first, even when enabled,
        //     and even if a translation happens to target the same code. Call sites rely on this
        //     (harvest caching / overwrite semantics process the base before translations).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void DefaultLanguage_AlwaysFirst_EvenWhenAlsoATranslationTarget()
        {
            // Edge: a translation targets the same code as the default. The default must still lead
            // exactly once, not be duplicated or reordered after the translations.
            var config = new LocalizationConfig { Enabled = true, DefaultLanguage = "fr" };
            var translations = new List<(string sourceLanguage, string targetLanguage)>
            {
                ("en", "fr"), ("fr", "en"),
            };

            var result = config.BuildLanguageList(translations);

            result.Should().StartWith("fr",
                "the default language is always the first element of the returned list.");
            // "fr" appears as default (index 0) AND as the first translation target → present twice.
            // This is the OBSERVED contract (no de-duplication); pinned so an added Distinct() —
            // which would change the language count and thus the number of generated outputs — is
            // caught.
            result.Should().Equal(new[] { "fr", "fr", "en" },
                "the implementation does not de-duplicate: default 'fr' is prepended verbatim, then " +
                "every translation target is appended verbatim, even if it repeats the default.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) EMPTY translations — enabled with no translations configured still yields the default
        //     language alone. Guards a regression that would return an empty list (→ zero outputs)
        //     when the translations list is empty.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Enabled_NoTranslations_ReturnsOnlyDefaultLanguage()
        {
            var config = new LocalizationConfig { Enabled = true, DefaultLanguage = "fr" };
            var translations = new List<(string sourceLanguage, string targetLanguage)>();

            var result = config.BuildLanguageList(translations);

            result.Should().Equal(new[] { "fr" },
                "the default language is seeded unconditionally, so even an enabled config with no " +
                "translations yields exactly the default — never an empty list (which would produce " +
                "zero outputs).");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) CUSTOM default language — DefaultLanguage is honored whatever it is (not hardcoded to
        //     "fr"). The property defaults to "fr" but is settable; a regression that hardcoded the
        //     seed would ignore a non-French base.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void CustomDefaultLanguage_Honored_WhenEnabled()
        {
            var config = new LocalizationConfig { Enabled = true, DefaultLanguage = "en" };
            var translations = new List<(string sourceLanguage, string targetLanguage)>
            {
                ("en", "es"), ("en", "ar"),
            };

            var result = config.BuildLanguageList(translations);

            result.Should().Equal(new[] { "en", "es", "ar" },
                "DefaultLanguage is read from config, not hardcoded — a non-'fr' base language leads " +
                "the list followed by its translation targets.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (6) DISABLED with custom default — same gate as (1) but with a non-default base, proving
        //     the gate and the default-language seed are independent (both read config faithfully).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Disabled_CustomDefault_ReturnsOnlyThatCustomDefault()
        {
            var config = new LocalizationConfig { Enabled = false, DefaultLanguage = "zh" };
            var translations = new List<(string sourceLanguage, string targetLanguage)>
            {
                ("zh", "en"), ("zh", "fa"),
            };

            var result = config.BuildLanguageList(translations);

            result.Should().Equal(new[] { "zh" },
                "the Enabled gate and the DefaultLanguage seed are independent: disabled still " +
                "returns exactly the (custom) default, ignoring translations.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (7) DETERMINISM — same config + translations always yield the same list (no hidden state,
        //     no ordering non-determinism). Guards against a refactor introducing mutation/randomness.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Deterministic_SameInputs_SameOutput()
        {
            var config = new LocalizationConfig { Enabled = true, DefaultLanguage = "fr" };
            var translations = new List<(string sourceLanguage, string targetLanguage)>
            {
                ("fr", "en"), ("fr", "ru"),
            };

            var first = config.BuildLanguageList(translations);
            var second = config.BuildLanguageList(translations);

            first.Should().Equal(second,
                "BuildLanguageList is deterministic — identical config + translations yield the " +
                "identical language list across repeated calls.");
        }
    }
}
