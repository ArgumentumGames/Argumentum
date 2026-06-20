using System;
using Argumentum.AssetConverter;
// PdfAuditor is a static class whose namespace shares its name (Argumentum.AssetConverter.PdfAuditor),
// so a plain PdfAuditor.X call resolves X against the NAMESPACE, not the class. `using static` imports
// the class's static members directly, letting us call ComputeAuditPageGeometry unqualified.
using static Argumentum.AssetConverter.PdfAuditor.PdfAuditor;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="PdfAuditor.ComputeAuditPageGeometry"/> — #204 tertiary (cont.
    /// po-2024): the PDF-audit page-grid geometry contract.
    ///
    /// Before chunking the deck into page-sized groups for <see cref="BuildExpectedImageOrder"/>, the
    /// audit derives how many card columns/rows fit on one sheet and how many cards that sheet holds.
    /// That capacity drives the page boundaries the expected image sequence is built around — if it is
    /// wrong, every page boundary is wrong, and the audit silently reports false mismatches or false
    /// passes against the rendered PDF.
    ///
    /// The arithmetic was previously INLINED inside <see cref="PdfAuditor.GetExpectedImageOrder"/> with
    /// only a code comment ("Assuming A4 for calculation, needs to be dynamic if possible") guarding
    /// the assumption. It has been extracted (output-neutral — the call site preserves the exact
    /// computation) into the pure, deterministic <see cref="PdfAuditor.ComputeAuditPageGeometry"/> so
    /// the layout contract is unit-testable without a PDF render. These tests pin the contract
    /// additively.
    /// </summary>
    public class PdfAuditorPageGeometryContractTests
    {
        // QuestPDF PageSizes.A4 in points: 210mm × 297mm = 595.2756 × 841.8898 (1 inch = 72pt, 25.4mm/in).
        // Used as exact float values so the floor-division assertions are precise.
        private const float A4WidthPoints = 595.2755905511812f;
        private const float A4HeightPoints = 841.8897637795276f;

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) COLUMNS — the configured value is honored when &gt; 0; otherwise floor-divided against A4.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Columns_PositiveConfig_IsHonoredVerbatim()
        {
            // A fixed grid (e.g. a 12-column sheet) is honored regardless of card width.
            var g = ComputeAuditPageGeometry(cardWidthPoints: 1f, cardHeightPoints: 1f, configuredNbColumns: 12);

            g.NbColumns.Should().Be(12);
        }

        [Fact]
        public void Columns_ZeroConfig_FallsBackToFloorDivisionAgainstA4()
        {
            // No configured columns → fit by A4 width: 595.28 / 50 = 11.9 → 11 columns.
            var g = ComputeAuditPageGeometry(cardWidthPoints: 50f, cardHeightPoints: 100f, configuredNbColumns: 0);

            g.NbColumns.Should().Be(11);
        }

        [Fact]
        public void Columns_NegativeConfig_AlsoFallsBack()
        {
            // The guard is `> 0`, so -1 (a sentinel / unset value) falls back like 0.
            var g = ComputeAuditPageGeometry(cardWidthPoints: 50f, cardHeightPoints: 100f, configuredNbColumns: -1);

            g.NbColumns.Should().Be(11);
        }

        [Fact]
        public void Columns_FloorTruncation_NotRounding()
        {
            // A4 width 595.28 / 100 = 5.95 → truncates to 5, NOT 6. A card that does not fully fit in
            // the remaining 95pts is dropped. This is THE fragile bit: Math.Round would yield 6.
            var g = ComputeAuditPageGeometry(cardWidthPoints: 100f, cardHeightPoints: 100f, configuredNbColumns: 0);

            g.NbColumns.Should().Be(5);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) ROWS — always floor-divided against A4 height.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Rows_FloorDivisionAgainstA4Height()
        {
            // A4 height 841.89 / 100 = 8.42 → 8 rows.
            var g = ComputeAuditPageGeometry(cardWidthPoints: 50f, cardHeightPoints: 100f, configuredNbColumns: 0);

            g.NbRows.Should().Be(8);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) CARDS PER PAGE = rows × columns.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void CardsPerPage_IsRowsTimesColumns()
        {
            // 11 cols × 8 rows = 88 cards/sheet (50pt-wide, 100pt-tall cards on plain A4).
            var g = ComputeAuditPageGeometry(cardWidthPoints: 50f, cardHeightPoints: 100f, configuredNbColumns: 0);

            g.NbCardsPerPage.Should().Be(88);
        }

        [Fact]
        public void CardsPerPage_ConfiguredColumns_MultipliedByA4Rows()
        {
            // A 12-column config: 12 cols × floor(841.89/100)=8 rows = 96 cards/sheet.
            var g = ComputeAuditPageGeometry(cardWidthPoints: 50f, cardHeightPoints: 100f, configuredNbColumns: 12);

            g.NbCardsPerPage.Should().Be(96);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) HARDCODED A4 — the geometry depends ONLY on A4, never on the actual page size nor on a
        //     configured column fallback that would differ for A0/Tarot. This pins the "Assuming A4"
        //     assumption as an OBSERVABLE contract (the latent divergence from the renderer's dynamic
        //     page size — see the divergence pin below).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Columns_AlwaysDerivedFromA4_NeverFromActualPageSize()
        {
            // There is no page-size input to ComputeAuditPageGeometry: the column count is a pure
            // function of (cardWidth, configuredColumns) against A4. An A0 deck (841×1189mm) passed
            // through the audit would be chunked as if it were A4. Asserting the A4-derived value
            // makes that assumption explicit and catches any future "dynamic page size" refactor.
            var g = ComputeAuditPageGeometry(cardWidthPoints: 50f, cardHeightPoints: 100f, configuredNbColumns: 0);

            // A4-only: 595.28 / 50 = 11 cols. An A0 derivation (841mm ≈ 2384pts / 50) would be 47.
            g.NbColumns.Should().Be(11, "the audit's geometry is derived from hardcoded A4, not the deck's " +
                "actual page size — a divergence from the renderer that this assertion makes explicit.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) DIVERGENCE FROM THE RENDERER — pins the latent correctness bug (NOT a fix). The renderer
        //     (PrintAndPlayDocument.ComputePageGeometry) subtracts the page margin AND reserves a
        //     pageHeight/10 header band, then uses the ACTUAL page size; the audit does none of this.
        //     This test documents that the audit's capacity exceeds the renderer's whenever a header is
        //     present, so the divergence is observable and fixable in isolation.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Divergence_AuditCapacity_ExceedsRenderer_WhenHeaderPresent()
        {
            // Same 12-col sheet, 50×100pt cards, plain A4. The audit ignores a header band entirely.
            var audit = ComputeAuditPageGeometry(cardWidthPoints: 50f, cardHeightPoints: 100f, configuredNbColumns: 12);

            // The renderer, with hasHeader=true on A4 (841.89pt), reserves 841.89/10 ≈ 84.19pt → content
            // 757.7pt → floor(757.7/100)=7 rows → 12×7 = 84 cards. The audit computes 12×8 = 96.
            // The 12-card gap is the header band the renderer drops and the audit keeps.
            audit.NbCardsPerPage.Should().Be(96, "the audit ignores the header band the renderer subtracts " +
                "(pageHeight/10), so it over-counts cards-per-page by one row whenever a header is present. " +
                "Documenting the divergence — a fix belongs in its own behavior-change PR.");
            audit.NbCardsPerPage.Should().BeGreaterThan(84,
                "the renderer with hasHeader=true would yield 84 cards on this sheet; the audit's 96 exceeds " +
                "it, which is the root of the false-mismatch/false-pass risk on headered documents.");
        }
    }
}
