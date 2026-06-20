using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="PdfManager.OrderImagesForBackFirstOneDocPerBack"/> — #204
    /// continuation (po-2024), the per-back PDF image-ordering contract.
    ///
    /// <see cref="PdfManager.GenerateBackFirstOneDocPerBack"/> groups a deck's cards by their back
    /// art and emits ONE PDF per distinct back: the shared back image FIRST, then every card's
    /// FRONT in their original CardSet order. That back-first-then-fronts sequence is what lets a
    /// print shop place one back behind a whole family of faces on a recto-verso sheet.
    ///
    /// This ordering is a fragile contract: a regression that emits fronts-first, drops the back,
    /// duplicates it, or reorders the fronts silently misaligns every printed sheet and is caught
    /// only by opening the PDF. It was previously inlined inside the MagickImage collection builder
    /// (with ZERO unit coverage), asymmetric with the alternate-face-and-back format whose ordering
    /// was already extracted into <see cref="PdfManager.OrderImagesForAlternateFaceAndBack"/>. It
    /// has been extracted (output-neutral — the call site emits the exact same path sequence) into
    /// the pure, deterministic <see cref="PdfManager.OrderImagesForBackFirstOneDocPerBack"/> so the
    /// contract is unit-testable. These tests pin it additively.
    /// </summary>
    public class PdfBackFirstOneDocPerBackContractTests
    {
        // Helper: build a card list from front-path specs. Backs are irrelevant to this format's
        // per-group ordering (the group key already selected them), so the helper takes fronts only.
        private static List<CardImages> Cards(params string[] fronts)
            => fronts.Select(f => new CardImages { Front = f }).ToList();

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) Back-first emission — the shared back art is always slot 0.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void BackIsEmittedFirst_BeforeAnyFront()
        {
            // THE fragile bit: the back MUST be slot 0 so it lands behind its faces recto-verso.
            var outSeq = PdfManager.OrderImagesForBackFirstOneDocPerBack(
                "BackArt", Cards("F1", "F2", "F3"));

            outSeq.First().Should().Be("BackArt");
        }

        [Fact]
        public void SingleFront_BackThenFront()
        {
            // Minimal group: one shared back + one face → two slots, back first.
            var outSeq = PdfManager.OrderImagesForBackFirstOneDocPerBack(
                "B", Cards("OnlyFace"));

            outSeq.Should().Equal("B", "OnlyFace");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) Front order is preserved — original CardSet order, no reordering.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void FrontsPreserveOriginalCardSetOrder()
        {
            // The faces keep their input order; the method must NOT sort or reverse them.
            var outSeq = PdfManager.OrderImagesForBackFirstOneDocPerBack(
                "Back", Cards("Fallacy1", "Fallacy2", "Fallacy3"));

            outSeq.Should().Equal("Back", "Fallacy1", "Fallacy2", "Fallacy3");
        }

        [Fact]
        public void FrontsAreNotReversed()
        {
            // Anti-regression: a naive back-first implementation that "prepends" each front to an
            // accumulator would reverse the faces. Pin that they stay in input order.
            var outSeq = PdfManager.OrderImagesForBackFirstOneDocPerBack(
                "B", Cards("A", "B_face", "C")).ToList();

            outSeq.Skip(1).Should().Equal("A", "B_face", "C");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) Count semantics — back is one slot, each front is one slot.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void CountIsOneBackPlusNumberOfFronts()
        {
            // 1 (back) + N (fronts) = N+1 emitted paths. A regression that duplicates the back or
            // drops a front breaks this count silently.
            var outSeq = PdfManager.OrderImagesForBackFirstOneDocPerBack(
                "Back", Cards("F1", "F2", "F3", "F4"));

            outSeq.Should().HaveCount(5);
        }

        [Fact]
        public void BackAppearsExactlyOnce()
        {
            // The shared back is emitted ONCE (as the group key), not once per front. A regression
            // that interleaves back-front-back-front would put the back N times.
            var outSeq = PdfManager.OrderImagesForBackFirstOneDocPerBack(
                "BackArt", Cards("F1", "F2", "F3")).ToList();

            outSeq.Count(p => p == "BackArt").Should().Be(1);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) Degenerate inputs.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void EmptyFronts_EmitsBackOnly()
        {
            // No faces for this back → just the back slot (matches the builder: Key + empty concat).
            var outSeq = PdfManager.OrderImagesForBackFirstOneDocPerBack(
                "LonelyBack", Cards());

            outSeq.Should().Equal("LonelyBack");
        }
    }
}
