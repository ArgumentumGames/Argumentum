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

            // BUGFIX #119: Préserver l'ordre original des CardSets (Rules en premier)
            var cardsWithBackCount = cardImages.Count(card => !string.IsNullOrEmpty(card.Back));
            var cardsWithoutBackCount = cardImages.Count(card => string.IsNullOrEmpty(card.Back));

            AnsiConsole.MarkupLine($"[cyan]INFO: Processing {cardsWithBackCount} cards with back, {cardsWithoutBackCount} cards without back for '{baseName}'[/]");
            AnsiConsole.MarkupLine($"[cyan]INFO: Preserving original CardSet order (Rules first, then Memo, Fallacies, etc.)[/]");

            var collecBuilderAFB = () =>
            {
                var allImages = new List<MagickImage>();

                // Parcourir les cartes dans l'ordre original des CardSets
                foreach (var card in cardImages)
                {
                    if (!string.IsNullOrEmpty(card.Back))
                    {
                        // Carte avec dos: dos d'abord, face ensuite (pour recto-verso)
                        allImages.Add(new MagickImage(card.Back));
                        allImages.Add(new MagickImage(card.Front));
                    }
                    else
                    {
                        // Carte sans dos (Rules): face uniquement
                        allImages.Add(new MagickImage(card.Front));
                    }
                }

                var collec = new MagickImageCollection(allImages);
                return collec;
            };

            targetFiles.Add((baseName, collecBuilderAFB));
            GeneratePdfsFromImages(targetFiles, overwriteExistingDocs);
        }

        public void GenerateBackFirstOneDocPerBack(string baseName, List<CardImages> cardImages, bool overwriteExistingDocs)
        {
            var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();
            var indexInsert = baseName.LastIndexOf('.');
            
            // BUGFIX CORRIGÉ: Partitionner les cartes avec/sans dos au lieu de filtrer
            var cardsWithBack = cardImages.Where(card => !string.IsNullOrEmpty(card.Back)).ToList();
            var cardsWithoutBack = cardImages.Where(card => string.IsNullOrEmpty(card.Back)).ToList();
            
            AnsiConsole.MarkupLine($"[cyan]INFO: Processing {cardsWithBack.Count} cards with back, {cardsWithoutBack.Count} cards without back for '{baseName}'[/]");
            
            // Grouper les cartes avec dos par image de dos
            var cardsPerBack = cardsWithBack.GroupBy(card => card.Back).ToArray();
            
            // Générer un PDF par type de dos
            for (int backIndex = 0; backIndex < cardsPerBack.Count(); backIndex++)
            {
                var closureBackIndex = backIndex;
                var collecBuilderBF = () =>
                {
                    var allImages = new List<MagickImage>();
                    
                    // Ajouter le dos en premier
                    var frontsAndBack = cardsPerBack[closureBackIndex];
                    allImages.Add(new MagickImage(frontsAndBack.Key));
                    
                    // Ajouter toutes les faces avec ce dos
                    allImages.AddRange(frontsAndBack.Select(card => new MagickImage(card.Front)));
                    
                    var collec = new MagickImageCollection(allImages);
                    return collec;
                };

                var newName =
                    $"{baseName.Substring(0, indexInsert)}-{backIndex + 1}{baseName.Substring(indexInsert)}";
                targetFiles.Add((newName, collecBuilderBF));
            }
            
            // Si des cartes n'ont pas de dos, créer un PDF supplémentaire "FacesOnly"
            if (cardsWithoutBack.Count > 0)
            {
                var collecBuilderFacesOnly = () =>
                {
                    var collec = new MagickImageCollection(cardsWithoutBack.Select(card => new MagickImage(card.Front)));
                    return collec;
                };
                
                var facesOnlyName = $"{baseName.Substring(0, indexInsert)}-FacesOnly{baseName.Substring(indexInsert)}";
                targetFiles.Add((facesOnlyName, collecBuilderFacesOnly));
                AnsiConsole.MarkupLine($"[cyan]INFO: Creating additional 'FacesOnly' PDF for {cardsWithoutBack.Count} cards without back[/]");
            }

            GeneratePdfsFromImages(targetFiles, overwriteExistingDocs);
        }

        public void GeneratePrintAndPlay(string fileName, CardSetDocumentConfig docConfig, List<CardImages> images, bool configOverwriteExistingDocs, bool useReleaseMode = false)
        {
            if (File.Exists(fileName) && !configOverwriteExistingDocs)
            {
                Logger.Log($"Skipping Existing pdf document {fileName}");
                return;
            }

            Logger.Log($"Starting PDF generation for {images.Count} images. File: {fileName}");

            if (images.Count == 0)
            {
                Logger.LogWarning($"Skipping PDF generation for {fileName} because there are no images.");
                return;
            }

            // 1. Read images — JPEG Q=85 for Debug (Edge preview), PNG lossless for Release (printer)
            byte[] ProcessImage(string path) => !string.IsNullOrEmpty(path) && File.Exists(path)
                ? (useReleaseMode ? File.ReadAllBytes(path) : ConvertToJpeg(File.ReadAllBytes(path), 85))
                : null;

            var frontImagesData = images.Select(img => ProcessImage(img.Front)).ToList();
            var backImagesData = images.Select(img => ProcessImage(img.Back)).ToList();

            // La logique de livret sera gérée à l'intérieur de PrintAndPlayDocument
            // if (isBooklet) { ... }

            // 2. Instancier le document
            var document = new PrintAndPlayDocument(docConfig, frontImagesData, backImagesData);

            // 3. Générer le PDF
            try
            {
                document.GeneratePdf(fileName);
                Logger.LogSuccess($"Generated pdf document {fileName}");
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"FAILED to generate PDF document {fileName}: {ex.Message}");
                Logger.LogException(ex);
                throw; // Rethrow to maintain behavior
            }
        }

        private static byte[] ConvertToJpeg(byte[] imageData, int quality)
        {
            if (imageData == null || imageData.Length == 0) return imageData;
            try
            {
                using var image = new MagickImage(imageData);
                image.Quality = (uint)quality;
                image.Format = MagickFormat.Jpeg;
                return image.ToByteArray();
            }
            catch
            {
                return imageData;
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
