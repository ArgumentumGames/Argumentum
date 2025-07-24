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

            Logger.Log($"Starting PDF generation for {images.Count} images.");

            // 1. Lire toutes les images en mémoire une seule fois
            var frontImagesData = images
                .Select(img => !string.IsNullOrEmpty(img.Front) && File.Exists(img.Front) ? File.ReadAllBytes(img.Front) : null)
                .ToList();

            var backImagesData = images
                .Select(img => !string.IsNullOrEmpty(img.Back) && File.Exists(img.Back) ? File.ReadAllBytes(img.Back) : null)
                .ToList();

            // La logique de livret sera gérée à l'intérieur de PrintAndPlayDocument
            // if (isBooklet) { ... }

            // 2. Instancier le document
            var document = new PrintAndPlayDocument(docConfig, frontImagesData, backImagesData);

            // 3. Générer le PDF
            document.GeneratePdf(fileName);

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
