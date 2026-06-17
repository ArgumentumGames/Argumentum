using System;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="PrintAndPlayDocument.ComputePageGeometry"/> — dispatch #204
    /// primaire (cont. po-2024).
    ///
    /// The Print &amp; Play PDF (<see cref="PrintAndPlayDocument"/>) lays cards out in a grid on each
    /// sheet and must compute, from the page size, the card size, an optional header band, the page
    /// margin and the deck length, FOUR derived quantities that drive the whole render:
    /// <list type="bullet">
    /// <item><description><c>NbColumns</c> — the configured column count when &gt; 0, ELSE the floor of
    /// contentWidth / cardWidth (integer truncation, NOT rounding — a card that does not fully fit is dropped).</description></item>
    /// <item><description><c>NbRows</c> — floor of contentHeight / cardHeight.</description></item>
    /// <item><description><c>NbCardsPerPage</c> — NbRows × NbColumns.</description></item>
    /// <item><description><c>NbPages</c> — ceil of frontImageCount / NbCardsPerPage (a partial last sheet still prints).</description></item>
    /// </list>
    /// A header band, when present, reserves <c>pageHeight / 10</c> of the content height (matching the
    /// header height drawn in <c>ComposePage</c>); the page margin (<c>2 × pageMarginMm × factor</c>) is
    /// subtracted from BOTH width and height.
    ///
    /// This arithmetic was previously INLINED inside <see cref="PrintAndPlayDocument.Compose"/> with
    /// ZERO unit coverage — a regression in any of the four quantities (e.g. rounding instead of
    /// truncating columns, forgetting the header reserve, or flooring instead of ceiling the page
    /// count) silently changes how many sheets print and how cards distribute across them, caught only
    /// by rendering and eyeballing the PDF. It has been extracted (output-neutral — the call site
    /// preserves the exact computation) into the pure, deterministic
    /// <see cref="PrintAndPlayDocument.ComputePageGeometry"/> so the layout contract is unit-testable.
    /// These tests pin the contract additively.
    /// </summary>
    public class PrintAndPlayPageGeometryContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // (1) COLUMNS — the configured value is honored when &gt; 0; otherwise floor-divided.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Columns_PositiveConfig_IsHonoredVerbatim()
        {
            // FallaciesWeb A0 ships a fixed 12-column grid regardless of card width.
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 10_000f, pageHeightPoints: 10_000f,
                cardWidthPoints: 1f, cardHeightPoints: 1f,
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: 12, frontImageCount: 1);

            g.NbColumns.Should().Be(12);
        }

        [Fact]
        public void Columns_ZeroConfig_FallsBackToFloorDivision()
        {
            // No configured columns → fit by width: contentWidth 600 / cardWidth 50 = 12 columns.
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 600f, pageHeightPoints: 800f,
                cardWidthPoints: 50f, cardHeightPoints: 100f,
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: 0, frontImageCount: 1);

            g.NbColumns.Should().Be(12);
        }

        [Fact]
        public void Columns_NegativeConfig_AlsoFallsBack()
        {
            // The guard is `> 0`, so -1 (a sentinel / unset value) falls back like 0.
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 600f, pageHeightPoints: 800f,
                cardWidthPoints: 50f, cardHeightPoints: 100f,
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: -1, frontImageCount: 1);

            g.NbColumns.Should().Be(12);
        }

        [Fact]
        public void Columns_FloorTruncation_NotRounding()
        {
            // 699 / 100 = 6.99 → truncates to 6, NOT 7. A card that does not fully fit in the
            // remaining 99pts is dropped. This is THE fragile bit: Math.Round would yield 7.
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 699f, pageHeightPoints: 800f,
                cardWidthPoints: 100f, cardHeightPoints: 100f,
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: 0, frontImageCount: 1);

            g.NbColumns.Should().Be(6);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) ROWS — always floor-divided from the content height.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Rows_FloorDivisionOfContentHeight()
        {
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 600f, pageHeightPoints: 850f,
                cardWidthPoints: 50f, cardHeightPoints: 100f,
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: 0, frontImageCount: 1);

            // 850 / 100 = 8.5 → 8 rows.
            g.NbRows.Should().Be(8);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) CARDS PER PAGE = rows × columns.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void CardsPerPage_IsRowsTimesColumns()
        {
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 600f, pageHeightPoints: 800f,
                cardWidthPoints: 50f, cardHeightPoints: 100f,
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: 0, frontImageCount: 1);

            // 12 cols × 8 rows = 96 cards/sheet.
            g.NbCardsPerPage.Should().Be(96);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) PAGES — ceil(count / cardsPerPage). A partial last sheet still prints.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Pages_CeilsWhenRemainder()
        {
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 600f, pageHeightPoints: 800f,
                cardWidthPoints: 600f, cardHeightPoints: 800f,   // 1 card/page
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: 1, frontImageCount: 5);

            // 5 cards / 1 per page = 5 pages.
            g.NbPages.Should().Be(5);
        }

        [Fact]
        public void Pages_ExactMultiple_NoExtraSheet()
        {
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 600f, pageHeightPoints: 800f,
                cardWidthPoints: 600f, cardHeightPoints: 800f,   // 1 card/page
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: 1, frontImageCount: 3);

            g.NbPages.Should().Be(3);
        }

        [Fact]
        public void Pages_PartialLastSheet_StillPrints()
        {
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 600f, pageHeightPoints: 800f,
                cardWidthPoints: 50f, cardHeightPoints: 100f,    // 96 cards/page
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: 12, frontImageCount: 97);

            // 97 / 96 = 1.0104 → ceil → 2 pages (last sheet holds the 1 leftover card).
            g.NbPages.Should().Be(2);
        }

        [Fact]
        public void Pages_ZeroCards_IsZero()
        {
            // An empty deck produces zero pages (ceil(0 / n) == 0). Important: this does NOT hit the
            // degenerate divide-by-zero, because cardsPerPage is non-zero here.
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 600f, pageHeightPoints: 800f,
                cardWidthPoints: 50f, cardHeightPoints: 100f,
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: 12, frontImageCount: 0);

            g.NbPages.Should().Be(0);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) HEADER — reserves 1/10 of page height (matches ComposePage's header band).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Header_ReservesOneTenthOfPageHeight()
        {
            var withHeader = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 600f, pageHeightPoints: 1000f,
                cardWidthPoints: 50f, cardHeightPoints: 100f,
                totalMarginPoints: 0f, hasHeader: true,
                configuredNbColumns: 12, frontImageCount: 1);

            // pageHeight 1000 − header 100 = content 900 → 9 rows (vs 10 without header).
            withHeader.NbRows.Should().Be(9);
        }

        [Fact]
        public void NoHeader_KeepsFullContentHeight()
        {
            var noHeader = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 600f, pageHeightPoints: 1000f,
                cardWidthPoints: 50f, cardHeightPoints: 100f,
                totalMarginPoints: 0f, hasHeader: false,
                configuredNbColumns: 12, frontImageCount: 1);

            noHeader.NbRows.Should().Be(10);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (6) MARGIN — subtracted from BOTH width and height.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Margin_SubtractedFromBothDimensions()
        {
            var g = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints: 610f, pageHeightPoints: 810f,
                cardWidthPoints: 50f, cardHeightPoints: 100f,
                totalMarginPoints: 10f, hasHeader: false,        // 10pt margin each side
                configuredNbColumns: 0, frontImageCount: 1);

            // width: 610 − 10 = 600 → 12 cols ; height: 810 − 10 = 800 → 8 rows.
            g.NbColumns.Should().Be(12);
            g.NbRows.Should().Be(8);
        }
    }
}
