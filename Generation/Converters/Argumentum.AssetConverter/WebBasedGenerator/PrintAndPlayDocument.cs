using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QuestPDF.Helpers;
using System.Reflection;
using System;
using QuestPDF.Drawing;

namespace Argumentum.AssetConverter
{
    public class PrintAndPlayDocument : IDocument
    {
        private readonly CardSetDocumentConfig _docConfig;
        private readonly List<byte[]> _frontImagesData;
        private readonly List<byte[]> _backImagesData;

        private const float InchToCentimetre = 2.54f;
        private const float InchToPoints = 72;
        private float MmToPointsFactor = 0.1f / InchToCentimetre * InchToPoints;

        public PrintAndPlayDocument(CardSetDocumentConfig docConfig, List<byte[]> frontImagesData, List<byte[]> backImagesData)
        {
            _docConfig = docConfig;
            _frontImagesData = frontImagesData;
            _backImagesData = backImagesData;
        }

        public DocumentMetadata GetMetadata() => new DocumentMetadata()
        {
            Author = "Argumentum",
            Creator = "Argumentum",
            Producer = "Argumentum",
            Subject = "Jeu de carte sur l'argumentation",
            Keywords = "Argumentation, rhétorique, arguments fallacieux, sophismes, éloquence",
            Title = "Argumentum Print & Play"
        };

        public void Compose(IDocumentContainer container)
        {
            var pageSizeType = typeof(PageSizes);
            var dynProp = pageSizeType.GetProperty(_docConfig.PageSize, BindingFlags.Static | BindingFlags.Public);
            var pageSize = (PageSize)dynProp.GetValue(null);
            var pageMarginMm = 0f;

            var cardWidthPoints = ((float)_docConfig.CardSets[0].FrontCards.WidthMM) * MmToPointsFactor;
            var cardHeightPoints = ((float)_docConfig.CardSets[0].FrontCards.HeigthMM) * MmToPointsFactor;

            var totalMarginPoints = 2 * pageMarginMm * MmToPointsFactor;
            var hasHeader = !string.IsNullOrEmpty(_docConfig.Header);

            // ✅ Page-grid geometry (nbColumns/nbRows/nbCardsPerPage/nbPages) extracted output-neutral
            // into ComputePageGeometry so this fragile layout contract is unit-testable without a render.
            var geometry = ComputePageGeometry(pageSize.Width, pageSize.Height, cardWidthPoints, cardHeightPoints,
                                               totalMarginPoints, hasHeader, _docConfig.NbColumns, _frontImagesData.Count);
            int nbColumns = geometry.NbColumns;
            var nbCardsPerPage = geometry.NbCardsPerPage;
            var nbPages = geometry.NbPages;

            for (int pageIndex = 0; pageIndex < nbPages; pageIndex++)
            {
                var pageFrontImages = _frontImagesData.Skip(pageIndex * nbCardsPerPage).Take(nbCardsPerPage).ToArray();
                var pageBackImages = _backImagesData.Skip(pageIndex * nbCardsPerPage).Take(nbCardsPerPage).ToArray();

                // Back page — only render if at least one card on this page has a non-null back
                if (!_docConfig.NoBack && pageBackImages.Any(b => b != null))
                {
                    var backCardsArray = ReorderBacksForRectoVerso(pageBackImages, nbColumns);
                    container.Page(page =>
                    {
                        ComposePage(page, pageSize, pageMarginMm, nbColumns, backCardsArray);
                    });
                }

                // Front page
                if (pageFrontImages.Any())
                {
                    container.Page(page =>
                    {
                        ComposePage(page, pageSize, pageMarginMm, nbColumns, pageFrontImages);
                    });
                }
            }
        }

        /// <summary>
        /// Reorders back images for horizontal recto-verso printing. Each grid ROW of backs is
        /// reversed so that, when the printed sheet is flipped along its horizontal edge (the way a
        /// Print &amp; Play sheet is turned to read the back), each back lines up behind its matching
        /// front. This is a PER-ROW reversal of the back grid — not a full mirror of the flat array —
        /// because a horizontal flip swaps left/right WITHIN each row while preserving row order
        /// (row 0 stays row 0). In other words the back at output position (row, col) is the original
        /// back at (row, nbColumns-1-col).
        /// Extracted output-neutral from <see cref="Compose"/> (the inline composition
        /// <c>backs.ToJaggedArray(nbColumns).Select(row =&gt; row.Reverse().ToArray()).ToArray().Flatten()</c>)
        /// so this fragile alignment contract is unit-testable in isolation, without a QuestPDF render.
        /// </summary>
        public static T[] ReorderBacksForRectoVerso<T>(IList<T> backs, int nbColumns)
            => backs.ToJaggedArray(nbColumns)
                    .Select(row => row.Reverse().ToArray())
                    .ToArray()
                    .Flatten();

        /// <summary>
        /// Page-grid geometry for a Print &amp; Play sheet: how many card columns/rows fit, how many
        /// cards per page, and how many pages the deck spans.
        /// </summary>
        public readonly struct PrintPlayPageGeometry
        {
            /// <summary>Grid columns per sheet. The configured value when &gt; 0, else floor(contentWidth / cardWidth).</summary>
            public readonly int NbColumns;
            /// <summary>Grid rows per sheet: floor(contentHeight / cardHeight).</summary>
            public readonly int NbRows;
            /// <summary>Cards that fit on one sheet: NbRows × NbColumns.</summary>
            public readonly int NbCardsPerPage;
            /// <summary>Sheets needed for the whole deck: ceil(frontImageCount / NbCardsPerPage).</summary>
            public readonly int NbPages;

            public PrintPlayPageGeometry(int nbColumns, int nbRows, int nbCardsPerPage, int nbPages)
            {
                NbColumns = nbColumns;
                NbRows = nbRows;
                NbCardsPerPage = nbCardsPerPage;
                NbPages = nbPages;
            }
        }

        /// <summary>
        /// Derives the page-grid geometry from the resolved page/card sizes. Pure &amp; deterministic —
        /// no QuestPDF dependency, no I/O. Reproduces the exact arithmetic previously inlined in
        /// <see cref="Compose"/> (output-neutral):
        /// <list type="bullet">
        /// <item><description>contentWidth = pageWidth − totalMargin</description></item>
        /// <item><description>header reserves pageHeight / 10 when <paramref name="hasHeader"/> (matches <see cref="ComposePage"/>'s header height)</description></item>
        /// <item><description>contentHeight = pageHeight − totalMargin − headerHeight</description></item>
        /// <item><description>NbColumns = configuredColumns when &gt; 0, else floor(contentWidth / cardWidth) — TRUNCATION, not rounding</description></item>
        /// <item><description>NbRows = floor(contentHeight / cardHeight)</description></item>
        /// <item><description>NbCardsPerPage = NbRows × NbColumns</description></item>
        /// <item><description>NbPages = ceil(frontImageCount / NbCardsPerPage)</description></item>
        /// </list>
        /// Degenerate inputs where NbCardsPerPage == 0 (card larger than the content area, or zero
        /// columns) make NbPages divide by zero — preserved as-is (NOT guarded) to stay output-neutral;
        /// flagged for a separate behavior-change PR.
        /// </summary>
        public static PrintPlayPageGeometry ComputePageGeometry(
            float pageWidthPoints, float pageHeightPoints,
            float cardWidthPoints, float cardHeightPoints,
            float totalMarginPoints,
            bool hasHeader,
            int configuredNbColumns,
            int frontImageCount)
        {
            var contentWidthPoints = pageWidthPoints - totalMarginPoints;
            var headerHeightPoints = hasHeader ? pageHeightPoints / 10 : 0;
            var contentHeightPoints = pageHeightPoints - totalMarginPoints - headerHeightPoints;

            int nbColumns = configuredNbColumns > 0
                ? configuredNbColumns
                : (int)(contentWidthPoints / cardWidthPoints);
            int nbRows = (int)(contentHeightPoints / cardHeightPoints);
            int nbCardsPerPage = nbRows * nbColumns;
            int nbPages = (int)Math.Ceiling((decimal)frontImageCount / (decimal)nbCardsPerPage);

            return new PrintPlayPageGeometry(nbColumns, nbRows, nbCardsPerPage, nbPages);
        }

        private void ComposePage(PageDescriptor page, PageSize pageSize, float pageMarginMm, int nbColumns, IEnumerable<byte[]> images)
        {
            page.Size(pageSize);
            page.Margin(pageMarginMm, Unit.Millimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(20));

            if (!string.IsNullOrEmpty(_docConfig.Header))
            {
                var imagePath = Path.Combine(Environment.CurrentDirectory, _docConfig.Header);
                if(File.Exists(imagePath))
                {
                    var imageData = File.ReadAllBytes(imagePath);
                    page.Header().AlignCenter().Height(pageSize.Height / 10).Padding(pageSize.Width / 150).Image(imageData, ImageScaling.FitHeight);
                }
            }

            page.Content()
                .Padding(0)
                .AlignCenter()
                .AlignTop()
                .Component(new CardGridComponent(images, nbColumns, _docConfig.Padding));
        }
    }
}