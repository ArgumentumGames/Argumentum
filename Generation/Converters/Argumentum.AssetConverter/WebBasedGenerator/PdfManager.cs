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

            // ✅ #119 image-sequence ordering extracted output-neutral into
            // OrderImagesForAlternateFaceAndBack so this fragile recto-verso contract
            // (original CardSet order preserved — Rules first; back-then-front per card) is
            // unit-testable without a MagickImage render.
            var orderedPaths = OrderImagesForAlternateFaceAndBack(cardImages);
            var collecBuilderAFB = () =>
            {
                var collec = new MagickImageCollection(orderedPaths.Select(p => new MagickImage(p)));
                return collec;
            };

            targetFiles.Add((baseName, collecBuilderAFB));
            GeneratePdfsFromImages(targetFiles, overwriteExistingDocs);
        }

        /// <summary>
        /// Produces the ordered image-path sequence for alternate face-and-back recto-verso
        /// printing (issue #119). The original CardSet order is PRESERVED — so cards without a
        /// back (Rules) keep their place and appear in sequence with the rest — and for each
        /// card that HAS a back, the BACK is emitted immediately before its FRONT. That back-
        /// then-front ordering is what makes each back line up behind its matching front when
        /// the sheet is printed recto-verso.
        /// Pure &amp; deterministic — no MagickImage, no I/O. Extracted output-neutral from
        /// <see cref="GenerateAlternateFaceAndBack"/> (which previously inlined this logic inside
        /// the collection builder) so this fragile alignment + ordering contract is unit-testable
        /// in isolation. A regression here (e.g. emitting front-then-back, or dropping the
        /// back-less cards) silently misaligns every printed sheet.
        /// </summary>
        public static IEnumerable<string> OrderImagesForAlternateFaceAndBack(IEnumerable<CardImages> cards)
        {
            foreach (var card in cards)
            {
                if (!string.IsNullOrEmpty(card.Back))
                {
                    yield return card.Back;
                    yield return card.Front;
                }
                else
                {
                    yield return card.Front;
                }
            }
        }

        /// <summary>
        /// Inserts <paramref name="suffix"/> into <paramref name="baseFileName"/> just before its
        /// FINAL dot, producing e.g. <c>Cards-1.pdf</c> from <c>Cards.pdf</c> + <c>"-1"</c>, or
        /// <c>Cards-FacesOnly.pdf</c> from <c>Cards.pdf</c> + <c>"-FacesOnly"</c>. The suffix INCLUDES
        /// its own leading separator (the call sites pass <c>"-N"</c> / <c>"-FacesOnly"</c>).
        ///
        /// Extracted output-neutral from <see cref="GenerateBackFirstOneDocPerBack"/> so the per-back
        /// and FacesOnly naming contract is unit-testable in isolation. The implementation preserves
        /// the original <c>baseName.Substring(0, LastIndexOf('.'))</c> / <c>Substring(LastIndexOf('.'))</c>
        /// split EXACTLY — including its current behavior when there is no dot: <c>LastIndexOf</c>
        /// returns <c>-1</c>, and <c>Substring(0, -1)</c> throws <see cref="ArgumentOutOfRangeException"/>.
        /// That throw is the existing contract (call sites always pass an extension-bearing base name);
        /// this method does NOT silently "fix" it, so a future caller passing a dotless name fails loud
        /// exactly as before. A regression here (inserting at the FIRST dot, dropping the extension, or
        /// off-by-one on the per-back counter) silently produces wrongly-named PDFs that overwrite each
        /// other or land in the wrong slot — caught only by inspecting the output directory.
        /// </summary>
        public static string InsertSuffixBeforeExtension(string baseFileName, string suffix)
        {
            var indexInsert = baseFileName.LastIndexOf('.');
            return baseFileName.Substring(0, indexInsert) + suffix + baseFileName.Substring(indexInsert);
        }

        public void GenerateBackFirstOneDocPerBack(string baseName, List<CardImages> cardImages, bool overwriteExistingDocs)
        {
            var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();

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

                var newName = InsertSuffixBeforeExtension(baseName, $"-{backIndex + 1}");
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
                
                var facesOnlyName = InsertSuffixBeforeExtension(baseName, "-FacesOnly");
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
                    // #29 fix: deterministic dispose of MagickImageCollection after write
                    // prevents ~1.2 GB peak held until GC finalizer (especially Fallacies Tarot ~277 images)
                    using var collection = targetFile.documentImages();
                    collection.Write(targetFile.fileName);
                    Logger.LogSuccess($"Generated pdf document {targetFile.fileName}");
                }
            }
        }
    }
}
