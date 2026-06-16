using System;
using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="PrintAndPlayDocument.ReorderBacksForRectoVerso{T}"/> — dispatch #204
    /// tertiaire / γ (cont. po-2024).
    ///
    /// The Print &amp; Play PDF (<see cref="PrintAndPlayDocument"/>) prints card backs on the reverse of
    /// each sheet so a sheet can be flipped to read the back. To make each back line up behind its
    /// matching front when the sheet is turned along its horizontal edge, the backs of EACH GRID ROW
    /// are reversed before rendering. This is a per-row reversal of the back grid — NOT a full mirror
    /// of the flat array — because a horizontal flip swaps left/right WITHIN each row while preserving
    /// row order (row 0 stays row 0). In other words the back at output position (row, col) is the
    /// original back at (row, nbColumns-1-col).
    ///
    /// This reordering was previously INLINED inside <see cref="PrintAndPlayDocument.Compose"/> as the
    /// composition <c>backs.ToJaggedArray(nbColumns).Select(row =&gt; row.Reverse().ToArray()).ToArray().Flatten()</c>,
    /// with ZERO unit coverage — it could only be exercised by rendering a full QuestPDF document and
    /// visually inspecting the back alignment. A regression here (e.g. reversing the WHOLE array
    /// instead of per-row, or flipping rows too) produces printed sheets whose backs mismatch their
    /// fronts — unusable sheets, only caught at print time. It has been extracted (output-neutral —
    /// the call site preserves the exact computation) into the pure, deterministic
    /// <see cref="PrintAndPlayDocument.ReorderBacksForRectoVerso{T}"/> so the alignment contract is
    /// unit-testable. These tests pin the contract additively.
    /// </summary>
    public class PrintAndPlayRectoVersoContractTests
    {
        /// <summary>Builds N labelled backs B0..B(N-1) for readable ordering assertions.</summary>
        private static string[] Backs(int count) =>
            Enumerable.Range(0, count).Select(i => $"B{i}").ToArray();

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) THE HEADLINE — the per-row reversal that aligns backs with fronts on a
        //     horizontal flip. Each row is reversed in place; row ORDER is preserved (row 0
        //     stays row 0). For 6 backs in 3 columns: input rows [B0,B1,B2] and [B3,B4,B5]
        //     become [B2,B1,B0] and [B5,B4,B3], flattened to [B2,B1,B0,B5,B4,B3].
        //     A regression that mirrors the WHOLE flat array would yield [B5,B4,B3,B2,B1,B0]
        //     (rows swapped) — the assertion below rejects both that and a no-op.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void FullRows_EvenlyDivisible_ReversesEachRow_PreservesRowOrder()
        {
            var backs = Backs(6);

            var result = PrintAndPlayDocument.ReorderBacksForRectoVerso(backs, nbColumns: 3);

            result.Should().Equal(new[] { "B2", "B1", "B0", "B5", "B4", "B3" },
                "each 3-wide row is reversed so backs align with fronts on a horizontal flip: " +
                "[B0,B1,B2]→[B2,B1,B0] and [B3,B4,B5]→[B5,B4,B3]. Row order is preserved — NOT a full " +
                "array mirror, which would wrongly yield [B5,B4,B3,B2,B1,B0].");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) Trailing short row — the last row has fewer cards than nbColumns. Its few
        //     cards are still reversed (a single-element row reverses to itself). The short
        //     row is NOT padded nor moved. 7 backs in 3 columns → the lone B6 on row 2
        //     reverses to [B6] and stays at the tail.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void TrailingShortRow_LastRowReversed_SingletonStaysInPlace()
        {
            var backs = Backs(7);

            var result = PrintAndPlayDocument.ReorderBacksForRectoVerso(backs, nbColumns: 3);

            result.Should().Equal(new[] { "B2", "B1", "B0", "B5", "B4", "B3", "B6" },
                "the trailing short row [B6] is reversed to itself (a single-element row is its own " +
                "reverse) and stays at the tail — it is neither dropped, padded, nor relocated.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) Single column — a horizontal flip over a single column is a no-op (each
        //     1-wide row reverses to itself). This pins that the reordering correctly
        //     degrades to identity for 1-wide sheets, rather than doing something
        //     surprising with the lone element per row.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void SingleColumn_HorizontalFlipIsNoOp_IdentityOrder()
        {
            var backs = Backs(3);

            var result = PrintAndPlayDocument.ReorderBacksForRectoVerso(backs, nbColumns: 1);

            result.Should().Equal(new[] { "B0", "B1", "B2" },
                "a 1-wide sheet has no within-row ordering to swap, so a horizontal flip is an identity: " +
                "each single-element row reverses to itself, and the output equals the input.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) One short row — nbColumns larger than the card count yields a single row,
        //     which is reversed wholesale. 2 backs in 5 columns → the lone row [B0,B1] is
        //     reversed to [B1,B0].
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void OneShortRow_ReversesWithinRow()
        {
            var backs = Backs(2);

            var result = PrintAndPlayDocument.ReorderBacksForRectoVerso(backs, nbColumns: 5);

            result.Should().Equal(new[] { "B1", "B0" },
                "when the whole page fits one short row, that row is reversed wholesale: [B0,B1]→[B1,B0].");
        }

        [Fact]
        public void EvenRowWidthTwo_ReversesEachRow()
        {
            var backs = Backs(4);

            var result = PrintAndPlayDocument.ReorderBacksForRectoVerso(backs, nbColumns: 2);

            result.Should().Equal(new[] { "B1", "B0", "B3", "B2" },
                "two 2-wide rows [B0,B1] and [B2,B3] reverse to [B1,B0] and [B3,B2].");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) Degenerate inputs — empty backs and a lone back must not throw and must
        //     round-trip to empty / unchanged. Guards against an IndexOutOfRange or a
        //     NullReference in the jagged/reverse chain when a page has no backs.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void EmptyBacks_ReturnsEmpty_NoCrash()
        {
            var result = PrintAndPlayDocument.ReorderBacksForRectoVerso(Array.Empty<string>(), nbColumns: 3);

            result.Should().BeEmpty("an empty back set has nothing to reorder and must not throw.");
        }

        [Fact]
        public void SingleBack_AnyColumnCount_Unchanged()
        {
            var result = PrintAndPlayDocument.ReorderBacksForRectoVerso(new[] { "B0" }, nbColumns: 3);

            result.Should().Equal(new[] { "B0" },
                "a lone card forms a single-element row that reverses to itself — there is nothing to " +
                "flip it against, so it is unchanged.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (6) PRODUCTION ELEMENT TYPE GROUNDING — the reordering is generic; production calls
        //     it with byte[] (each card's PNG/JPEG bytes). Tagging each byte[] card with a
        //     single byte lets us assert the ordering without a deep image comparison. This
        //     proves the generic works for the actual production element type, not just string.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Works_WithProductionElementType_ByteArray()
        {
            byte[] Card(int n) => new byte[] { (byte)n };
            var backs = Enumerable.Range(0, 6).Select(Card).ToArray();

            var result = PrintAndPlayDocument.ReorderBacksForRectoVerso(backs, nbColumns: 3);

            // Same per-row-reversal contract as the string case: [0,1,2]→[2,1,0], [3,4,5]→[5,4,3].
            result.Select(b => (int)b[0]).Should().Equal(new[] { 2, 1, 0, 5, 4, 3 },
                "the generic reordering works for the production element type byte[] (card image bytes), " +
                "applying the identical per-row reversal.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (7) THE ALIGNMENT SEMANTIC, stated directly. After reordering, the back at output
        //     position (row, col) is the back that was at INPUT position (row, nbColumns-1-col).
        //     This is the precise mirror-within-row that makes a horizontally-flipped sheet's
        //     backs line up with its fronts. Asserted by rebuilding the input and output grids
        //     and checking every cell — catches any deviation from within-row reversal.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void AlignmentContract_OutputRowCol_EqualsInputRowMirroredCol()
        {
            const int cols = 3;
            const int rows = 2;
            var backs = Backs(rows * cols); // B0..B5

            var output = PrintAndPlayDocument.ReorderBacksForRectoVerso(backs, cols);

            // Re-jag both input and output into their grids for cell-wise comparison.
            var inputGrid = backs.ToJaggedArray(cols);
            var outputGrid = output.ToJaggedArray(cols);

            output.Length.Should().Be(backs.Length, "reordering preserves the total card count");
            outputGrid.Length.Should().Be(rows, "the output has the same row count as the input");

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    outputGrid[row][col].Should().Be(inputGrid[row][cols - 1 - col],
                        $"output[{row}][{col}] must equal input[{row}][{cols - 1 - col}] — the within-row " +
                        $"mirror that aligns backs with fronts on a horizontal flip");
                }
            }
        }
    }
}
