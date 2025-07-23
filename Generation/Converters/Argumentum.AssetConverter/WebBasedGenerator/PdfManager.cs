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
                return;
            }

            var pageSizeType = typeof(PageSizes);
            var dynProp = pageSizeType.GetProperty(docConfig.PageSize, BindingFlags.Static | BindingFlags.Public);
            var pageSize = (PageSize)dynProp.GetValue(null);
            var pageMarginMm = 0f;

            var cardWidthPoints = ((float)docConfig.CardSets[0].FrontCards.WidthMM) * MmToPointsFactor;
            var cardHeightPoints = ((float)docConfig.CardSets[0].FrontCards.HeigthMM) * MmToPointsFactor;

            var totalMarginPoints = 2 * pageMarginMm * MmToPointsFactor;
            var contentWidthPoints = pageSize.Width - totalMarginPoints;
            var contentHeightPoints = pageSize.Height - totalMarginPoints;

            int nbColumns = docConfig.NbColumns > 0 ? docConfig.NbColumns : (int)(contentWidthPoints / cardWidthPoints);
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

            Logger.Log($"Starting PDF generation for {images.Count} images.");

            // Booklet logic
            var isBooklet = docConfig.CardSets.Any(cs => cs.CardSetName == KnownCardSets.RulesPrintAndPlay);

            if (isBooklet)
            {
                // Create a list of all pages (cards), adding blank pages at the end to make the total a multiple of 4.
                var allPagesContent = new List<CardImages[]>();
                for (int i = 0; i < nbPages; i++)
                {
                    allPagesContent.Add(images.Skip(i * nbCardsPerPage).Take(nbCardsPerPage).ToArray());
                }

                var totalPages = nbPages;
                while (totalPages % 4 != 0)
                {
                    allPagesContent.Add(new CardImages[0]); // Add a blank page
                    totalPages++;
                }

                // Correct booklet imposition algorithm
                var bookletPagesOrder = new List<int>();
                for (int i = 0; i < totalPages / 2; i++)
                {
                    if (i % 2 == 0)
                    {
                        bookletPagesOrder.Add(totalPages - i - 1);
                        bookletPagesOrder.Add(i);
                    }
                    else
                    {
                        bookletPagesOrder.Add(i);
                        bookletPagesOrder.Add(totalPages - i - 1);
                    }
                }
                
                // This is the standard imposition for a booklet
                // Example for 8 pages: 8,1,2,7,6,3,4,5
                var impositionOrder = new List<int>();
                for (int i = 0; i < totalPages / 2; i += 2)
                {
                    impositionOrder.Add(totalPages - i -1);
                    impositionOrder.Add(i);
                    impositionOrder.Add(i+1);
                    impositionOrder.Add(totalPages - i - 2);
                }


                Document.Create(container =>
                {
                    foreach (var pageNumber in impositionOrder)
                    {
                        container.Page(page =>
                        {
                            page.Size(pageSize);
                            page.Margin(pageMarginMm, Unit.Millimetre);
                            var pageCards = allPagesContent[pageNumber];

                            page.Header()
                                .AlignCenter()
                                .Text($"Argumentum - Livret de règles")
                                .SemiBold().FontSize(10).FontColor(Colors.Grey.Medium);
                            
                            page.Content().Table(table =>
                                {
                                    table.ColumnsDefinition(h => { for (int i = 0; i < nbColumns; i++) h.ConstantColumn(cardWidthPoints + 1); });
                                    foreach (var card in pageCards)
                                    {
                                        table.Cell().Padding(docConfig.Padding).Image(File.ReadAllBytes(card.Front));
                                    }
                                });

                            page.Footer()
                                .AlignCenter()
                                .Text(x =>
                                {
                                    x.Span("Page ");
                                    x.CurrentPageNumber();
                                    x.Span(" / ");
                                    x.TotalPages();
                                });
                        });
                    }
                })
                .WithMetadata(docMetadata)
                .GeneratePdf(fileName);
            }
            else
            {
                // Original logic for non-booklet print and play
                Document.Create(container =>
                {
                    for (int pageIndex = 0; pageIndex < nbPages; pageIndex++)
                    {
                        var pageCards = images.Skip(pageIndex * nbCardsPerPage).Take(nbCardsPerPage).ToArray();

                        // Back page
                        if (!docConfig.NoBack)
                        {
                            Logger.Log($"Generating back page {pageIndex + 1}/{nbPages} for {fileName}");
                            var backCardsArray = pageCards.ToJaggedArray(nbColumns).Select(row => row.Reverse().ToArray()).ToArray().Flatten();
                            container.Page(page =>
                            {
                                page.Size(pageSize);
                                page.Margin(pageMarginMm, Unit.Millimetre);
                                page.PageColor(Colors.White);
                                page.DefaultTextStyle(x => x.FontSize(20));

                                if (!string.IsNullOrEmpty(docConfig.Header))
                                {
                                    var imagePath = Path.Combine(Environment.CurrentDirectory, docConfig.Header);
                                    page.Header().AlignCenter().Height(pageSize.Height / 20).Padding(pageSize.Width / 150).Image(imagePath).FitHeight();
                                }

                                page.Content().Padding(0).AlignCenter().AlignTop().Table(table =>
                                {
                                    table.ColumnsDefinition(h => { for (int i = 0; i < nbColumns; i++) h.ConstantColumn(cardWidthPoints + 1); });
                                    foreach (var card in backCardsArray)
                                    {
                                        table.Cell().Padding(docConfig.Padding).Element(cell =>
                                        {
                                            if (card != null && !string.IsNullOrEmpty(card.Back) && File.Exists(card.Back))
                                            {
                                                cell.Image(File.ReadAllBytes(card.Back));
                                            }
                                        });
                                    }
                                });
                            });
                        }

                        // Front page
                        Logger.Log($"Generating front page {pageIndex + 1}/{nbPages} for {fileName}");
                        container.Page(page =>
                        {
                            page.Size(pageSize);
                            page.Margin(pageMarginMm, Unit.Millimetre);
                            page.PageColor(Colors.White);
                            page.DefaultTextStyle(x => x.FontSize(20));

                            if (!string.IsNullOrEmpty(docConfig.Header))
                            {
                                var imagePath = Path.Combine(Environment.CurrentDirectory, docConfig.Header);
                                page.Header().AlignCenter().Height(pageSize.Height / 20).Padding(pageSize.Width / 150).Image(imagePath).FitHeight();
                            }

                            page.Content().Padding(0).AlignCenter().AlignTop().Table(table =>
                            {
                                table.ColumnsDefinition(h => { for (int i = 0; i < nbColumns; i++) h.ConstantColumn(cardWidthPoints + 1); });
                                foreach (var card in pageCards)
                                {
                                    table.Cell().Padding(docConfig.Padding).Element(cell =>
                                    {
                                        if (card != null)
                                        {
                                            cell.Image(File.ReadAllBytes(card.Front));
                                        }
                                    });
                                }
                            });
                        });
                    }
                })
                .WithMetadata(docMetadata)
                .GeneratePdf(fileName);
            }

            Logger.LogSuccess($"Generated pdf document {fileName}");
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
