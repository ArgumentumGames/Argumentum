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

            try
            {
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
                                    var projectRoot = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\"));
                                    var imagePath = Path.Combine(projectRoot, docConfig.Header);
                                    page.Header().AlignCenter().Height(pageSize.Height / 20).Padding(pageSize.Width / 150).Image(imagePath).FitHeight();
                                }

                                page.Content().Padding(0).AlignCenter().AlignTop().Table(table =>
                                {
                                    table.ColumnsDefinition(h => { for (int i = 0; i < nbColumns; i++) h.ConstantColumn(cardWidthPoints + 1); });
                                    foreach (var card in backCardsArray)
                                    {
                                        table.Cell().Padding(docConfig.Padding).Element(cell =>
                                        {
                                            if (card != null)
                                            {
                                                using var toPrint = new MagickImage(card.Back);
                                                using var memStream = new MemoryStream();
                                                toPrint.Write(memStream);
                                                cell.Image(memStream.ToArray());
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
                                var projectRoot = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\"));
                                var imagePath = Path.Combine(projectRoot, docConfig.Header);
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
                                            using var toPrint = new MagickImage(card.Front);
                                            using var memStream = new MemoryStream();
                                            toPrint.Write(memStream);
                                            cell.Image(memStream.ToArray());
                                        }
                                    });
                                }
                            });
                        });
                    }
                })
                .WithMetadata(docMetadata)
                .GeneratePdf(fileName);

                Logger.LogSuccess($"Generated pdf document {fileName}");
            }
            catch (Exception ex)
            {
                Logger.Log($"[red]FATAL: Error generating PDF {fileName}: {ex.ToString()}[/]");
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
