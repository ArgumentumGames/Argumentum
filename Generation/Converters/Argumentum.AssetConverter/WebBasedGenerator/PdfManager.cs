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
            
            // BUGFIX CORRIGÉ: Partitionner les cartes avec/sans dos au lieu de filtrer
            var cardsWithBack = cardImages.Where(card => !string.IsNullOrEmpty(card.Back)).ToList();
            var cardsWithoutBack = cardImages.Where(card => string.IsNullOrEmpty(card.Back)).ToList();
            
            AnsiConsole.MarkupLine($"[cyan]INFO: Processing {cardsWithBack.Count} cards with back, {cardsWithoutBack.Count} cards without back for '{baseName}'[/]");
            
            var collecBuilderAFB = () =>
            {
                var allImages = new List<MagickImage>();
                
                // Ajouter les cartes avec dos (face + dos alternés)
                foreach (var card in cardsWithBack)
                {
                    allImages.Add(new MagickImage(card.Front));
                    allImages.Add(new MagickImage(card.Back));
                }
                
                // Ajouter les cartes sans dos (face uniquement)
                foreach (var card in cardsWithoutBack)
                {
                    allImages.Add(new MagickImage(card.Front));
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
