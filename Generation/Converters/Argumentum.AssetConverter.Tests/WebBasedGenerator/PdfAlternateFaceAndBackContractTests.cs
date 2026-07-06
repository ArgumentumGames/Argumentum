using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="PdfManager.OrderImagesForAlternateFaceAndBack"/> — #204
    /// secondary (cont. po-2024), the issue-#119 recto-verso ordering contract.
    ///
    /// <see cref="PdfManager.GenerateAlternateFaceAndBack"/> assembles a single PDF where each
    /// card's BACK is printed immediately before its FRONT, so that on a recto-verso sheet each
    /// back lines up behind its matching front. The original CardSet order MUST be preserved —
    /// cards without a back (Rules) keep their place and contribute their front only — which is
    /// what makes Rules appear first in TarotCards PDFs (the #119 fix, CLAUDE.md fragile area).
    ///
    /// This ordering is a fragile contract: a regression that emits front-then-back, drops the
    /// back-less cards, or reorders by back-presence silently misaligns every printed sheet and
    /// is caught only by printing and checking. It was previously inlined inside the MagickImage
    /// collection builder with ZERO unit coverage. It has been extracted (output-neutral — the
    /// call site emits the exact same path sequence) into the pure, deterministic
    /// <see cref="PdfManager.OrderImagesForAlternateFaceAndBack"/> so the contract is unit-testable.
    /// These tests pin it additively.
    /// </summary>
    public class PdfAlternateFaceAndBackContractTests
    {
        // Helper: build a card list from (front, back) tuples; null/empty back = no back.
        // `back` is nullable here because the contract tests deliberately pass null to exercise
        // OrderImagesForAlternateFaceAndBack's IsNullOrEmpty guard (a Rules card has no back).
        // The `s.back!` suppression at the CardImages.Back assignment acknowledges that the prod
        // entity annotates Back as non-nullable, but prod tolerates a null Back at runtime via the
        // guard — this is the intentional null-passing idiom #710 §2 flags for test arrange.
        private static List<CardImages> Cards(params (string front, string? back)[] specs)
            => specs.Select(s => new CardImages { Front = s.front, Back = s.back! }).ToList();

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) Per-card emission — back-then-front when a back exists, front-only otherwise.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void CardWithBack_EmitsBackThenFront()
        {
            // THE fragile bit: back MUST precede its front so they align recto-verso.
            var outSeq = PdfManager.OrderImagesForAlternateFaceAndBack(
                Cards(("F1", "B1")));

            outSeq.Should().Equal("B1", "F1");
        }

        [Fact]
        public void CardWithoutBack_EmitsFrontOnly()
        {
            // Rules have no back — they contribute their front alone, no phantom slot.
            var outSeq = PdfManager.OrderImagesForAlternateFaceAndBack(
                Cards(("F1", null)));

            outSeq.Should().Equal("F1");
        }

        [Fact]
        public void EmptyBack_TreatedAsNoBack()
        {
            // The guard is string.IsNullOrEmpty — "" behaves like null (no back emitted).
            var outSeq = PdfManager.OrderImagesForAlternateFaceAndBack(
                Cards(("F1", "")));

            outSeq.Should().Equal("F1");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) Original CardSet order is preserved — no reordering by back-presence.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void PreservesOriginalCardSetOrder_MixedBacks()
        {
            // Rules (no back), then a Fallacy (back), then another no-back, then a back.
            // The output walks cards in INPUT order — it does NOT group backs together.
            var outSeq = PdfManager.OrderImagesForAlternateFaceAndBack(
                Cards(("Rules1", null), ("Fall1", "Back1"), ("Rules2", null), ("Fall2", "Back2")));

            outSeq.Should().Equal("Rules1", "Back1", "Fall1", "Rules2", "Back2", "Fall2");
        }

        [Fact]
        public void RulesFirstPreservedInSequence()
        {
            // The #119 contract: a back-less card at the head of the list stays first in output.
            var outSeq = PdfManager.OrderImagesForAlternateFaceAndBack(
                Cards(("Rules", null), ("Memo", "MemoBack"), ("Fallacy", "FallBack")));

            outSeq.First().Should().Be("Rules");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) Count semantics — back doubles the slot, front-only is a single slot.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void AllCardsWithBack_DoublesTheCount()
        {
            var outSeq = PdfManager.OrderImagesForAlternateFaceAndBack(
                Cards(("F1", "B1"), ("F2", "B2"), ("F3", "B3")));

            // 3 cards × 2 (back+front) = 6 emitted paths.
            outSeq.Should().HaveCount(6);
        }

        [Fact]
        public void AllCardsWithoutBack_SinglePerCard()
        {
            var outSeq = PdfManager.OrderImagesForAlternateFaceAndBack(
                Cards(("F1", null), ("F2", null), ("F3", null)));

            outSeq.Should().HaveCount(3);
            outSeq.Should().Equal("F1", "F2", "F3");
        }

        [Fact]
        public void BackImmediatelyPrecedesItsFront()
        {
            // For an all-with-back deck, every even index (0,2,4…) is a back and the odd index
            // right after it is that back's own front — the pairing must not drift.
            var outSeq = PdfManager.OrderImagesForAlternateFaceAndBack(
                Cards(("F1", "B1"), ("F2", "B2"))).ToList();

            outSeq[0].Should().Be("B1");
            outSeq[1].Should().Be("F1");
            outSeq[2].Should().Be("B2");
            outSeq[3].Should().Be("F2");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) Degenerate inputs.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void EmptyInput_EmitsNothing()
        {
            var outSeq = PdfManager.OrderImagesForAlternateFaceAndBack(
                Cards());

            outSeq.Should().BeEmpty();
        }
    }
}
