using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="HarvestManager.ComputeExpectedImageCount"/> — dispatch #204
    /// secondaire (cont. po-2024).
    ///
    /// The number of images CardPen generates for a card set depends on its client-side rowset
    /// grouping. CLAUDE.md documents the formula in "Leçons Apprises — Calcul du nombre d'images
    /// attendues":
    /// <code>
    ///   with rsstyle="bunch" and rscount &gt;= N:  expectedImageCount = ceil(cardIds.Count / rscount)
    ///   otherwise:                                expectedImageCount = cardIds.Count
    /// </code>
    /// A mismatch here surfaces at harvest time as a confusing "image count mismatch" (one of the
    /// documented debugging symptoms in CLAUDE.md) — the harvest expects the wrong number of images
    /// and either hangs or returns a partial set.
    ///
    /// This formula was previously INLINED inside the Playwright flow in
    /// <see cref="HarvestManager"/> (GenerateImages), gated by <c>rscount &gt; 1 &amp;&amp; rsstyle in
    /// {bunch,cycle,random}</c>, with ZERO unit coverage — it could only be exercised by running a
    /// full browser harvest. It has been extracted (output-neutral — the call site preserves the
    /// exact computation and the Log condition verbatim) into the pure, deterministic
    /// <see cref="HarvestManager.ComputeExpectedImageCount"/> so the CardPen-mirroring contract is
    /// unit-testable. These tests pin the contract additively.
    /// </summary>
    public class HarvestManagerExpectedImageCountTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // (1) THE MATRIX — the full input space of the formula, parameterized. Each row is one
        //     (cardCount, rscount, rsstyle) → expected image count. Covers: grouping that divides
        //     evenly, grouping that does not (ceil), the documented Memo golden-master case, the
        //     single-card edge, all three grouping styles, and every non-grouping guard
        //     (rscount&lt;=1, rscount==0, empty/null/unknown rsstyle, case-sensitivity).
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        // Grouping APPLIES (rscount > 1 + a valid lowercase rsstyle) → ceil(cardCount / rscount)
        [InlineData(6, 3, "bunch", 2)]    // divides evenly: 6/3 = 2
        [InlineData(7, 3, "bunch", 3)]    // does not divide: ceil(7/3) = 3
        [InlineData(200, 200, "bunch", 1)] // CLAUDE.md Memo example: 200 rows, rscount=200 → 1 image
        [InlineData(1, 3, "bunch", 1)]    // single card: ceil(1/3) = 1 (a CardSet with rscount>rows still yields 1)
        [InlineData(5, 2, "cycle", 3)]    // cycle style groups the same way
        [InlineData(5, 2, "random", 3)]   // random style groups the same way
        // Grouping does NOT apply → cardCount verbatim
        [InlineData(10, 1, "bunch", 10)]  // rscount == 1: no grouping (one row per card)
        [InlineData(10, 0, "bunch", 10)]  // rscount == 0: no grouping
        [InlineData(10, 3, "", 10)]       // empty rsstyle: no grouping
        [InlineData(10, 3, null, 10)]     // null rsstyle: no grouping
        [InlineData(10, 3, "other", 10)]  // unknown rsstyle: no grouping
        [InlineData(10, 3, "BUNCH", 10)]  // case-sensitive: CardPen rsstyle is lowercase; "BUNCH" does NOT group
        [InlineData(10, 3, "Bunch", 10)]  // case-sensitive: mixed-case does NOT group either
        public void ComputeExpectedImageCount_MirrorsCardPenGrouping(
            int cardCount, int rscount, string? rsstyle, int expected)
        {
            HarvestManager.ComputeExpectedImageCount(cardCount, rscount, rsstyle)
                .Should().Be(expected,
                    $"the formula must mirror CardPen's rowset grouping for cardCount={cardCount}, " +
                    $"rscount={rscount}, rsstyle={rsstyle ?? "<null>"}");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) The documented Memo golden-master contract (CLAUDE.md): "Memo avec 200 lignes CSV et
        //     rscount=200 → génère 1 seule image". Pinned as its own Fact because it is THE
        //     headline example in the project lessons — a regression that returned 200 (no grouping)
        //     or 0 (empty) would break the Memo harvest silently.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Memo_GoldenMaster_200Rows_Rscount200_Bunch_YieldsExactlyOneImage()
        {
            // CLAUDE.md: "Exemple: Memo avec 200 lignes CSV et rscount=200 → génère 1 seule image."
            HarvestManager.ComputeExpectedImageCount(cardCount: 200, rscount: 200, rsstyle: "bunch")
                .Should().Be(1,
                    "the documented Memo golden-master contract: 200 CSV rows grouped with rscount=200 " +
                    "bunch → CardPen emits exactly ONE image. A regression here breaks the Memo harvest.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) Case-sensitivity — CardPen's rsstyle values are lowercase ("bunch"/"cycle"/"random").
        //     A capitalized variant must NOT trigger grouping; otherwise a template authored with a
        //     different case would silently change the image count. Pins the lowercase-only contract.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Rsstyle_IsCaseSensitive_OnlyLowercaseStylesGroup()
        {
            // Lowercase groups; any other capitalization falls through to one-image-per-row.
            HarvestManager.ComputeExpectedImageCount(9, 3, "bunch").Should().Be(3, "lowercase 'bunch' groups");
            HarvestManager.ComputeExpectedImageCount(9, 3, "Bunch").Should().Be(9, "'Bunch' does not group (case-sensitive)");
            HarvestManager.ComputeExpectedImageCount(9, 3, "BUNCH").Should().Be(9, "'BUNCH' does not group (case-sensitive)");
            HarvestManager.ComputeExpectedImageCount(9, 3, "bUnCh").Should().Be(9, "mixed-case does not group (case-sensitive)");
        }
    }
}
