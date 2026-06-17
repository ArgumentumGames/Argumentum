using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="ImageFileGenerator.ResolveCardBack"/> — #204 secondary
    /// (cont. po-2024), the Golden Master (commit 0087f0ec) face→back name-matching contract.
    ///
    /// When a card set ships several distinct back arts, the harvest must pair EACH face with the
    /// correct back by NAME, not randomly: the back whose (lower-cased) key is contained in the
    /// (lower-cased) face key wins (e.g. face "Argumentum_Scenarii_1.1.1..histoire_titre" picks the
    /// "histoire" back). With a single back, every face shares it; with no backs, the face ships
    /// alone; if no name matches, it falls back to the first available back.
    ///
    /// This is a fragile, silently-wrong-output contract: a regression here pairs the WRONG back
    /// art behind a face while leaving page count, geometry, and ordering all correct — the defect
    /// surfaces only by inspecting which back sits behind which printed card. It was previously
    /// inlined inside <c>AssembleCurrentCardImages</c> with ZERO unit coverage. It has been
    /// extracted (output-neutral — the call site assembles the exact same Front/Back pair) into the
    /// pure, deterministic <see cref="ImageFileGenerator.ResolveCardBack"/> so the contract is
    /// unit-testable. These tests pin it additively.
    ///
    /// Conceptual complement to <see cref="PdfAlternateFaceAndBackContractTests"/> (#119): that
    /// contract ORDERS fronts/backs back-then-front; THIS contract CHOOSES which back each face
    /// gets.
    /// </summary>
    public class FaceToBackMatchingContractTests
    {
        // Helper: build a back dictionary from (name, path) tuples.
        private static Dictionary<string, string> Backs(params (string name, string path)[] specs)
            => specs.ToDictionary(s => s.name, s => s.path);

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) No-back / single-back branches.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void NoBacks_ReturnsNull_AndFlagsNoAvailableBack()
        {
            // A card set with no harvested backs ships faces alone — Back stays null.
            var back = ImageFileGenerator.ResolveCardBack(
                "Argumentum_Scenarii_1.1_histoire_titre", Backs(),
                out var hadNoAvailableBack, out var usedFallback);

            back.Should().BeNull();
            hadNoAvailableBack.Should().BeTrue();
            usedFallback.Should().BeFalse();
        }

        [Fact]
        public void SingleBack_IsSharedByEveryFace()
        {
            // One back art → it pairs behind every face, regardless of the face name.
            var back = ImageFileGenerator.ResolveCardBack(
                "Anything_unrelated_name", Backs(("-histoire", "/back/histoire.png")),
                out var hadNoAvailableBack, out var usedFallback);

            back.Should().Be("/back/histoire.png");
            hadNoAvailableBack.Should().BeFalse();
            usedFallback.Should().BeFalse();
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) Name-matching — the Golden Master substring rule.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void MultipleBacks_PicksBackWhoseKeyIsContainedInFaceKey()
        {
            // Back keys are normalized by GenerateBacks to a leading-hyphen suffix (e.g. harvested
            // "scenarii-01-histoire" → key "-histoire"). The face key produced by CardPen for that
            // scenario contains the SAME "-histoire" substring, so Contains("-histoire") matches.
            // (NB: the raw token "histoire" alone would NOT match — the leading hyphen is part of
            // the contract. See LongestMatchingKeyWins for the tie-break over shared suffixes.)
            var back = ImageFileGenerator.ResolveCardBack(
                "Argumentum_Scenarii_1.1.1.-histoire.titre",
                Backs(("-histoire", "/back/histoire.png"), ("-logique", "/back/logique.png")),
                out _, out var usedFallback);

            back.Should().Be("/back/histoire.png");
            usedFallback.Should().BeFalse();
        }

        [Fact]
        public void NameMatching_IsCaseInsensitive()
        {
            // The contract lower-cases both sides before Contains — "HISTOIRE" in the face matches
            // a "-histoire" back key (the hyphen prefix is still required).
            var back = ImageFileGenerator.ResolveCardBack(
                "Face_with_-HISTOIRE_token",
                Backs(("-histoire", "/back/histoire.png")),
                out _, out _);

            back.Should().Be("/back/histoire.png");
        }

        [Fact]
        public void MultipleBacks_PicksUnrelatedBackForUnrelatedFace()
        {
            // A face matching a DIFFERENT token gets that token's back, not the first one.
            var back = ImageFileGenerator.ResolveCardBack(
                "Argumentum_Scenarii_2.3.-logique.suite",
                Backs(("-histoire", "/back/histoire.png"), ("-logique", "/back/logique.png")),
                out _, out _);

            back.Should().Be("/back/logique.png");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) THE fragile bit — longest-key-first tie-break.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LongestMatchingKeyWins_WhenMultipleKeysMatch()
        {
            // Both "-hist" and "-histoire" are contained in "Face_-histoire.titre". The contract
            // picks the LONGEST match first (OrderByDescending(Length)) so the more specific back
            // wins. Dropping the tie-break would let "-hist" (or insertion order) steal the match.
            var back = ImageFileGenerator.ResolveCardBack(
                "Face_-histoire.titre",
                Backs(("-hist", "/back/short.png"), ("-histoire", "/back/specific.png")),
                out _, out _);

            back.Should().Be("/back/specific.png");
        }

        [Fact]
        public void ShorterOnlyMatch_StillPicked_WhenNoLongerMatch()
        {
            // When only the short key matches (face contains "-hist" but not "-histoire"), the short
            // key is chosen — longest-first does not prevent a match, only breaks ties.
            var back = ImageFileGenerator.ResolveCardBack(
                "Face_-hist.something",
                Backs(("-hist", "/back/short.png"), ("-histoire", "/back/specific.png")),
                out _, out _);

            back.Should().Be("/back/short.png");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) Fallback — no name matches → first available back, flagged.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void NoNameMatch_FallsBackToFirstBack_AndFlagsFallback()
        {
            // A face whose key contains NEITHER back token gets the first available back, and the
            // caller is told it used the fallback so it can warn.
            var back = ImageFileGenerator.ResolveCardBack(
                "Face_with_no_matching_token",
                Backs(("-histoire", "/back/histoire.png"), ("-logique", "/back/logique.png")),
                out var hadNoAvailableBack, out var usedFallback);

            back.Should().Be("/back/histoire.png");
            hadNoAvailableBack.Should().BeFalse();
            usedFallback.Should().BeTrue();
        }
    }
}
