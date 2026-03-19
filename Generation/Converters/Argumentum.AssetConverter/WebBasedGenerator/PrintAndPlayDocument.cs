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
            var contentWidthPoints = pageSize.Width - totalMarginPoints;

            // ✅ FIX: Soustraire la hauteur du header de l'espace disponible pour le contenu
            // Le header utilise pageSize.Height / 10, donc on le soustrait de contentHeightPoints
            var headerHeightPoints = !string.IsNullOrEmpty(_docConfig.Header) ? pageSize.Height / 10 : 0;
            var contentHeightPoints = pageSize.Height - totalMarginPoints - headerHeightPoints;

            int nbColumns = _docConfig.NbColumns > 0 ? _docConfig.NbColumns : (int)(contentWidthPoints / cardWidthPoints);
            var nbRows = (int)(contentHeightPoints / cardHeightPoints);
            var nbCardsPerPage = nbRows * nbColumns;
            var nbPages = (int)Math.Ceiling((decimal)_frontImagesData.Count / (decimal)nbCardsPerPage);

            for (int pageIndex = 0; pageIndex < nbPages; pageIndex++)
            {
                var pageFrontImages = _frontImagesData.Skip(pageIndex * nbCardsPerPage).Take(nbCardsPerPage).ToArray();
                var pageBackImages = _backImagesData.Skip(pageIndex * nbCardsPerPage).Take(nbCardsPerPage).ToArray();

                // Back page — only render if at least one card on this page has a non-null back
                if (!_docConfig.NoBack && pageBackImages.Any(b => b != null))
                {
                    var backCardsArray = pageBackImages.ToJaggedArray(nbColumns).Select(row => row.Reverse().ToArray()).ToArray().Flatten();
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