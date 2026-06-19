using System;
using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter;
// PdfAuditor is a static class whose namespace shares its name (Argumentum.AssetConverter.PdfAuditor),
// so a plain PdfAuditor.X call resolves X against the NAMESPACE, not the class. `using static` imports
// the class's static members directly, letting us call BuildExpectedImageOrder unqualified.
using static Argumentum.AssetConverter.PdfAuditor.PdfAuditor;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="PdfAuditor.BuildExpectedImageOrder"/> — #204 secondary
    /// (cont. po-2024): the PDF-audit expected-image-order contract.
    ///
    /// The <see cref="PdfAuditor"/> hashes every image embedded in a rendered recto-verso PDF and
    /// compares them, IN ORDER, against the sequence of expected image paths built from the deck. That
    /// expected sequence must mirror exactly what the renderer (<see cref="PrintAndPlayDocument"/>)
    /// placed on the sheet: for each page-sized chunk of cards, the BACKS come first — per grid ROW
    /// reversed, so they line up behind their fronts on a horizontal flip — then the FRONTS in natural
    /// order. If the audit's expected order ever drifts from the renderer's actual order, the audit
    /// reports false mismatches (or false passes) with no signal beyond the PDF render — a silent
    /// corruption of the only automated correctness check on the printed sheets.
    ///
    /// The per-row back reversal now calls the SAME method the renderer uses
    /// (<see cref="PrintAndPlayDocument.ReorderBacksForRectoVerso{T}"/>, pinned by
    /// <see cref="PrintAndPlayRectoVersoContractTests"/>). Previously <see cref="PdfAuditor"/>
    /// re-implemented that reversal inline (<c>ToJaggedArray/Reverse/Flatten</c>) with only a code
    /// comment ("must match PdfManager exactly") guarding the duplication — a change to the renderer's
    /// reversal would have silently desynchronized the audit. Extracted output-neutral into
    /// <see cref="PdfAuditor.BuildExpectedImageOrder"/> (the <c>File.Exists</c> filter stays at the
    /// call site) so the ordering contract is unit-testable in isolation, without a PDF render.
    /// </summary>
    public class PdfAuditorExpectedOrderContractTests
    {
        /// <summary>
        /// Builds N cards C0..C(N-1) where card Ci has Front = "F{i}" and Back = "B{i}". Fronts and
        /// backs share the index so the expected interleaving is readable in assertions.
        /// </summary>
        private static List<CardImages> Cards(int count) =>
            Enumerable.Range(0, count)
                .Select(i => new CardImages { Front = $"F{i}", Back = $"B{i}" })
                .ToList();

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) THE HEADLINE — a single full page with backs. The 6 cards fit one 3-column page
        //     (2 rows × 3 cols). Backs come first, per-row reversed ([B0,B1,B2]→[B2,B1,B0] and
        //     [B3,B4,B5]→[B5,B4,B3] → [B2,B1,B0,B5,B4,B3]), then the fronts in natural order.
        //     A regression that emitted backs UN-reversed would yield [B0..B5] here; one that
        //     mirrored the whole back array would yield [B5,B4,B3,B2,B1,B0]. Both rejected.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void SingleFullPage_BacksRowReversed_ThenFronts()
        {
            var cards = Cards(6); // one page: 2 rows × 3 cols, nbCardsPerPage = 6

            var result = BuildExpectedImageOrder(cards, nbCardsPerPage: 6, nbColumns: 3, noBack: false)
                .ToList();

            result.Should().Equal(
                new[] { "B2", "B1", "B0", "B5", "B4", "B3", // backs, each 3-wide row reversed
                        "F0", "F1", "F2", "F3", "F4", "F5" }, // fronts, natural order
                "backs precede fronts, and each grid row of backs is reversed so they align with " +
                "their fronts on a horizontal flip — the same contract the renderer applies.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) Trailing short page — 8 cards over two 6-card pages. Page 0 holds C0..C5 (full),
        //     page 1 holds C6,C7 (short). Each page's backs are reversed WITHIN their rows; the
        //     short page's lone row [B6,B7] reverses to [B7,B6]. Pins that the per-page chunking
        //     interleaves backs-then-fronts correctly across page boundaries.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void TwoPages_EachPageBacksReversedThenFronts_ShortLastPage()
        {
            var cards = Cards(8); // page 0: C0..C5 (full 6), page 1: C6,C7 (short)

            var result = BuildExpectedImageOrder(cards, nbCardsPerPage: 6, nbColumns: 3, noBack: false)
                .ToList();

            result.Should().Equal(
                new[] { "B2", "B1", "B0", "B5", "B4", "B3", // page 0 backs (full)
                        "F0", "F1", "F2", "F3", "F4", "F5", // page 0 fronts
                        "B7", "B6",                         // page 1 backs (short row [B6,B7]→[B7,B6])
                        "F6", "F7" },                       // page 1 fronts
                "each page-sized chunk emits its backs (per-row reversed) then its fronts, so the " +
                "page boundary is respected and the short last page reverses within its lone row.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) noBack = true — faces ship alone (no backs at all). The sequence is just the fronts
        //     in natural order, regardless of column count. Pins the NoBack branch so a regression
        //     that still emitted backs (or emitted them for the wrong pages) is caught.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void NoBack_FrontsOnly_NaturalOrder()
        {
            var cards = Cards(6);

            var result = BuildExpectedImageOrder(cards, nbCardsPerPage: 6, nbColumns: 3, noBack: true)
                .ToList();

            result.Should().Equal(
                new[] { "F0", "F1", "F2", "F3", "F4", "F5" },
                "when the deck has no backs, every page emits only its fronts in natural order — no " +
                "reversal, no interleaving, the column count is irrelevant.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) Single column — a 1-wide grid reverses each single-element row to itself, so the
        //     backs come out in natural order (B0,B1,...) followed by fronts. Pins that the audit
        //     degrades correctly for single-column sheets rather than doing something surprising.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void SingleColumn_BacksUnchangedThenFronts()
        {
            var cards = Cards(3);

            var result = BuildExpectedImageOrder(cards, nbCardsPerPage: 3, nbColumns: 1, noBack: false)
                .ToList();

            result.Should().Equal(
                new[] { "B0", "B1", "B2", "F0", "F1", "F2" },
                "a 1-wide grid has no within-row ordering to swap, so each single-element back row " +
                "reverses to itself and the backs come out in natural order before the fronts.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) SYNC WITH THE RENDERER — the per-row back reversal the audit builds must equal the
        //     EXACT sequence <see cref="PrintAndPlayDocument.ReorderBacksForRectoVerso{T}"/> produces
        //     for the same page (the method both sides now call). This is the anti-drift guarantee:
        //     if someone changes the renderer's reversal, this test fails unless the audit changes
        //     with it. Asserted page-by-page rather than across the whole deck.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void BacksOrder_EqualsRendererReorderBacksForRectoVerso_PerPage()
        {
            const int cols = 3;
            const int perPage = 6;
            var cards = Cards(perPage * 2); // two full pages

            var all = BuildExpectedImageOrder(cards, perPage, cols, noBack: false).ToList();

            // Each page contributes `perPage` backs then `perPage` fronts.
            for (int page = 0; page < 2; page++)
            {
                var pageCards = cards.Skip(page * perPage).Take(perPage).ToList();
                var rendererBacks = PrintAndPlayDocument.ReorderBacksForRectoVerso(pageCards, cols)
                    .Select(c => c?.Back);
                var auditBacks = all.Skip(page * (perPage * 2)).Take(perPage);

                auditBacks.Should().Equal(rendererBacks,
                    $"page {page}: the audit's back sequence must equal the renderer's " +
                    $"ReorderBacksForRectoVerso output for the same page — they share the same method, " +
                    $"so any drift is a contract violation, not a coincidence.");
            }
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (6) Degenerate input — an empty deck must produce an empty sequence, not throw. Guards
        //     against a NullReference or index error in the chunk/reorder chain when the audit is
        //     handed a deck with no cards.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void EmptyDeck_ReturnsEmpty_NoCrash()
        {
            var result = BuildExpectedImageOrder(
                Array.Empty<CardImages>(), nbCardsPerPage: 6, nbColumns: 3, noBack: false).ToList();

            result.Should().BeEmpty("an empty deck has nothing to order and the audit must not throw.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (7) Null backs are carried through as null — the real GetExpectedImageOrder filters them
        //     (with !string.IsNullOrEmpty) at the boundary, but BuildExpectedImageOrder itself is
        //     agnostic: a card with no back yields null in the back slot, preserving position so the
        //     front still lands at the right offset. Pins the c?.Back null-propagation contract.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void NullBack_YieldsNullInBackSlot_PreservesPosition()
        {
            // 3 cards in 1 page, 3 cols: the middle card C1 has NO back (Back = null).
            var cards = new List<CardImages>
            {
                new() { Front = "F0", Back = "B0" },
                new() { Front = "F1", Back = null },
                new() { Front = "F2", Back = "B2" },
            };

            var result = BuildExpectedImageOrder(cards, nbCardsPerPage: 3, nbColumns: 3, noBack: false)
                .ToList();

            // Row [B0,null,B2] reverses to [B2,null,B0]; then fronts F0,F1,F2.
            result.Should().Equal(
                new[] { "B2", null, "B0", "F0", "F1", "F2" },
                "a null back propagates through the reversal at its row position (the card has no back " +
                "art) and the front still lands at the correct offset — the boundary filter later " +
                "drops the null, but the ordering contract must preserve position.");
        }
    }
}
