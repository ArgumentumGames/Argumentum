using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using ImageMagick;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Spectre.Console;

namespace Argumentum.AssetConverter
{
    public class PdfManager
    {
        public PdfManager()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        private const float InchToCentimetre = 2.54f;
        private const float InchToPoints = 72;
        private float MmToPointsFactor = 0.1f / InchToCentimetre * InchToPoints;

        internal void GenerateFacesOnly(string baseName, List<CardImages> cardImages, bool overwriteExistingDocs)
        {
            var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();
            var collecBuilderAFB = () =>
            {
                var collec = new MagickImageCollection(cardImages.Select(s => new MagickImage(s.Front)));
                return collec;
            };

            targetFiles.Add((baseName, collecBuilderAFB));
            GeneratePdfsFromImages(targetFiles, overwriteExistingDocs);
        }

        public void GenerateAlternateFaceAndBack(string baseName, List<CardImages> cardImages, bool overwriteExistingDocs)
        {
            var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();
            var collecBuilderAFB = () =>
            {
                var collec = new MagickImageCollection(cardImages.SelectMany(s =>
                {
                    return new[] { new MagickImage(s.Front), new MagickImage(s.Back) };
                }));
                return collec;
            };

            targetFiles.Add((baseName, collecBuilderAFB));
            GeneratePdfsFromImages(targetFiles, overwriteExistingDocs);
        }

        public void GenerateBackFirstOneDocPerBack(string baseName, List<CardImages> cardImages, bool overwriteExistingDocs)
        {
            var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();
            var indexInsert = baseName.LastIndexOf('.');
            var cardsPerBack = cardImages.GroupBy(card => card.Back).ToArray();
            for (int backIndex = 0; backIndex < cardsPerBack.Count(); backIndex++)
            {
                var closureBackIndex = backIndex;
                var collecBuilderBF = () =>
                {
                    var frontsAndBack = cardsPerBack[closureBackIndex];
                    var backThenFronts = new[] { new MagickImage(frontsAndBack.Key) }.Concat(
                        frontsAndBack.Select(card => new MagickImage(card.Front)));
                    var collec = new MagickImageCollection(backThenFronts);
                    return collec;
                };

                var newName =
                    $"{baseName.Substring(0, indexInsert)}-{backIndex + 1}{baseName.Substring(indexInsert)}";
                targetFiles.Add((newName, collecBuilderBF));
            }

            GeneratePdfsFromImages(targetFiles, overwriteExistingDocs);
        }

        public void GeneratePrintAndPlay(string fileName, CardSetDocumentConfig docConfig, List<CardImages> images, bool configOverwriteExistingDocs)
        {
            if (File.Exists(fileName) && !configOverwriteExistingDocs)
            {
                Logger.Log($"Skipping Existing pdf document {fileName}");
            }
            else
            {
                var pageSizeType = typeof(PageSizes);
                var dynProp = pageSizeType.GetProperty(docConfig.PageSize, BindingFlags.Static | BindingFlags.Public);

                var pageSize = (PageSize)dynProp.GetValue(null);
                var pageMarginMm = 0f;

                var cardWidthPoints = ((float)docConfig.CardSets[0].FrontCards.WidthMM) * MmToPointsFactor;
                var cardHeightPoints = ((float)docConfig.CardSets[0].FrontCards.HeigthMM) * MmToPointsFactor;

                var totalMarginPoints = 2 * pageMarginMm * MmToPointsFactor;
                var contentWidthPoints = pageSize.Width - totalMarginPoints;
                var contentHeightPoints = pageSize.Height - totalMarginPoints;

                int nbColumns;
                if (docConfig.NbColumns > 0)
                {
                    nbColumns = docConfig.NbColumns;
                }
                else
                {
                    nbColumns = (int)(contentWidthPoints / cardWidthPoints);
                }
                var nbRows = (int)(contentHeightPoints / cardHeightPoints);

                var nbCardsPerPage = nbRows * nbColumns;
                var nbPages = (int)Math.Ceiling((decimal)images.Count / (decimal)nbCardsPerPage);

                var docMetadata = new DocumentMetadata()
                {
                    Author = "Argumentum",
                    Creator = "Argumentum",
                    Producer = "Argumentum",
                    Subject = "Jeu de carte sur l'argumentation",
                    Keywords = "Argumentation, rhétorique, arguments fallacieux, sophismes, éloquence",
                    Title = "Argumentum Print & Play"
                };

                Document.Create(container =>
                {
                    for (int pageIndex = 0; pageIndex < nbPages; pageIndex++)
                    {
                        var pageCards = images.Skip(pageIndex * nbCardsPerPage);
                        if (pageIndex < nbPages - 1)
                        {
                            pageCards = pageCards.Take(nbCardsPerPage);
                        }

                        var pageCardsArray = pageCards.ToArray();

                        if (!docConfig.NoBack)
                        {
                            Logger.Log($"Generating back page {pageIndex + 1}/{nbPages} for {fileName}");
                            GenerateCardsPage(container, docConfig, pageSize, pageMarginMm, nbColumns, pageCardsArray, cardWidthPoints, cardImages => new MagickImage(cardImages.Back));
                            pageCardsArray = pageCardsArray.ToJaggedArray(nbColumns).Select(row => row.Reverse().ToArray())
                                .ToArray().Flatten();
                        }

                        Logger.Log($"Generating front page {pageIndex + 1}/{nbPages} for {fileName}");
                        GenerateCardsPage(container, docConfig, pageSize, pageMarginMm, nbColumns, pageCardsArray, cardWidthPoints, cardImages => new MagickImage(cardImages.Front));
                    }
                })
                .WithMetadata(docMetadata)
                .GeneratePdf(fileName);
                Logger.LogSuccess($"Generated pdf document {fileName}");
            }
        }

        private static void GenerateCardsPage(IDocumentContainer container, CardSetDocumentConfig docConfig, PageSize pageSize, float pageMarginMm, int nbColumns, CardImages[] pageCardsArray, float cardWidthPoints, Func<CardImages, MagickImage> frontOrBack)
        {
            container.Page(page =>
            {
                page.Size(pageSize);
                page.Margin(pageMarginMm, Unit.Millimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                if (!string.IsNullOrEmpty(docConfig.Header))
                {
                    var projectRoot = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\"));
                    var imagePath = Path.Combine(projectRoot, docConfig.Header);
                    page.Header()
                    	.AlignCenter()
                    	.Height(pageSize.Height / 20)
                    	.Padding(pageSize.Width / 150)
                    	.Image(imagePath)
                    	.FitHeight();
                }

                page.Content()
                    .Padding(0)
                    .AlignCenter()
                    .AlignTop()
                    .Table(table =>
                    {
                        table.ColumnsDefinition(h =>
                        {
                            for (int i = 0; i < nbColumns; i++)
                            {
                                h.ConstantColumn(cardWidthPoints + 1);
                            }
                        });

                        foreach (var card in pageCardsArray)
                        {
                            table.Cell()
                                .Padding(docConfig.Padding)
                                .Element(cell =>
                                {
                                    if (card != null)
                                    {
                                       MagickImage toPrint = frontOrBack(card);
                                       PrintMagickImageIntoTableCell(toPrint, cell);
                                    }
                                });
                        }
                    });
            });
        }

        private static void PrintMagickImageIntoTableCell(MagickImage toPrint, IContainer gridCell)
        {
            if (toPrint == null)
            {
                Logger.LogWarning("Attempted to print a null MagickImage.");
                return;
            }

            if (!string.IsNullOrEmpty(toPrint.FileName))
            {
                if (File.Exists(toPrint.FileName))
                {
                    gridCell.Image(toPrint.FileName);
                }
                else
                {
                    Logger.LogWarning($"Image file not found: {toPrint.FileName}");
                }
            }
            else
            {
                using (var memStream = new MemoryStream())
                {
                    if (toPrint.Width > 0 && toPrint.Height > 0)
                    {
                        toPrint.Write(memStream);
                        if (memStream.Length > 0)
                        {
                            gridCell.Image(memStream.ToArray());
                        }
                        else
                        {
                            Logger.LogWarning("MagickImage has no data to write to stream.");
                        }
                    }
                    else
                    {
                        Logger.LogWarning("MagickImage has invalid dimensions.");
                    }
                }
            }
        }

        public void GeneratePdfsFromImages(List<(string fileName, Func<MagickImageCollection> documentImages)> targetFiles,
            bool configOverwriteExistingDocs)
        {
            foreach (var targetFile in targetFiles)
            {
                if (File.Exists(targetFile.fileName) && !configOverwriteExistingDocs)
                {
                    Logger.Log($"Skipping Existing pdf document {targetFile.fileName}");
                }
                else
                {
                    targetFile.documentImages().Write(targetFile.fileName);
                    Logger.LogSuccess($"Generated pdf document {targetFile.fileName}");
                }
            }
        }
    }
}
