using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="CardSetLocalization.GetLocalizedFileName"/> — #204 idle (cont.
    /// po-2024): the localized-filename transform contract.
    ///
    /// When the pipeline localizes a CardSet, it derives the output template filename from the
    /// default-language one by swapping (or inserting) the language code. That derived filename
    /// drives which template gets loaded for each language — if it is wrong, the wrong template (or
    /// none) is loaded, silently producing cards in the wrong language or no cards at all.
    ///
    /// <see cref="CardSetLocalization.GetLocalizedFileName"/> is a pure, deterministic string
    /// transform (no I/O) with three branches and several subtle edge cases (substring matches,
    /// multiple occurrences, multi-dot filenames) that had ZERO unit coverage. These tests pin the
    /// contract additively so a refactor cannot silently change which template a language resolves to.
    /// </summary>
    public class LocalizedFileNameContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // (1) EMPTY / NULL — passed through untouched (guard clause).
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void EmptyOrNull_ReturnedAsIs(string fileName)
        {
            // The first guard returns the input verbatim, so no language substitution is attempted.
            CardSetLocalization.GetLocalizedFileName(fileName, "fr", "en")
                .Should().Be(fileName, "an empty/null filename cannot be localized and is returned as-is.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) CONTAINS _{defaultLanguage} — replaced (the happy path for standard templates).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void ContainsDefaultLanguageToken_ReplacedWithTarget()
        {
            // Standard template: Argumentum_Fallacies_fr.json → ..._en.json
            CardSetLocalization.GetLocalizedFileName("Argumentum_Fallacies_fr.json", "fr", "en")
                .Should().Be("Argumentum_Fallacies_en.json",
                    "when the filename carries the default-language token, it is swapped for the target.");
        }

        [Fact]
        public void ContainsDefaultLanguageToken_PreservesPathAndExtension()
        {
            // A path with subdirectories: the token is replaced in place, the rest of the path is kept.
            CardSetLocalization.GetLocalizedFileName(@"locales\cards_fr.json", "fr", "pt")
                .Should().Be(@"locales\cards_pt.json");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) SUBSTRING MATCH — the token must be preceded by '_'. A bare substring of the language
        //     code elsewhere in the name is NOT matched and falls through to the insertion branch.
        //     Pins the Contains($"_{defaultLanguage}") guard so a regression to a plain language-code
        //     Contains (which would misfire on "french.json") is caught.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LanguageCodeWithoutUnderscore_NotMatched_InsertedBeforeExtension()
        {
            // "french.json" CONTAINS "fr" but NOT "_fr" → falls through to the insertion branch.
            CardSetLocalization.GetLocalizedFileName("french.json", "fr", "en")
                .Should().Be("french_en.json",
                    "the match requires a leading underscore, so 'french' is not mistaken for the 'fr' token.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) MULTIPLE OCCURRENCES — string.Replace replaces ALL matches, not just the language-tag
        //     one. A filename that legitimately contains the token twice (e.g. a templated name)
        //     gets both swapped. This is THE fragile bit: a future "replace only the last segment"
        //     refactor would change behavior. Pinned as-is.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void MultipleTokenOccurrences_AllReplaced()
        {
            // my_fr_cards_fr.json → my_en_cards_en.json (BOTH occurrences replaced, not just the last).
            CardSetLocalization.GetLocalizedFileName("my_fr_cards_fr.json", "fr", "en")
                .Should().Be("my_en_cards_en.json",
                    "the implementation uses string.Replace which swaps every occurrence; a filename with " +
                    "two tokens has both replaced. Documenting the contract — a surgical 'last-only' fix " +
                    "would belong in a separate behavior-change PR.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) NO TOKEN, HAS EXTENSION — the target code is inserted before the extension.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void NoToken_WithExtension_TargetInsertedBeforeExtension()
        {
            // cards.json (no language token) → cards_en.json
            CardSetLocalization.GetLocalizedFileName("cards.json", "fr", "en")
                .Should().Be("cards_en.json");
        }

        [Fact]
        public void MultiDotFilename_OnlyLastExtensionTreatedAsExtension()
        {
            // Path.GetExtension treats only the LAST dot as the extension: Cards.v2.json →
            // GetFileNameWithoutExtension = "Cards.v2", extension = ".json" → Cards.v2_en.json.
            CardSetLocalization.GetLocalizedFileName("Cards.v2.json", "fr", "en")
                .Should().Be("Cards.v2_en.json",
                    "only the final segment after the last dot is treated as the extension, so a versioned " +
                    "name like Cards.v2 keeps its '.v2' infix.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (6) NO TOKEN, NO EXTENSION — the target code is appended.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void NoToken_NoExtension_TargetAppended()
        {
            // cards (no dot at all) → cards_en
            CardSetLocalization.GetLocalizedFileName("cards", "fr", "en")
                .Should().Be("cards_en");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (7) ROUND-TRIP NEUTRALITY — localizing back to the default language restores the original
        //     for the happy path. Guards against asymmetric swaps.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void RoundTrip_ToTargetAndBack_RestoresOriginal()
        {
            const string original = "Argumentum_Fallacies_fr.json";

            var localized = CardSetLocalization.GetLocalizedFileName(original, "fr", "en");
            var restored = CardSetLocalization.GetLocalizedFileName(localized, "en", "fr");

            restored.Should().Be(original,
                "swapping to the target and back should restore the original for the standard token case.");
        }
    }
}
