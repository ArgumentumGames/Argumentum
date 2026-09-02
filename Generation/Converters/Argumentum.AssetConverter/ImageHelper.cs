using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ImageMagick;

namespace Argumentum.AssetConverter
{
    public static class ImageHelper
    {

        private const string base64ContentGroupName = "base64Content";

        private static Regex urlExtractorRegex = new Regex(@$"^data:[a-z]+\/(?:[a-z]+);base64,(?<{base64ContentGroupName}>.*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// #1179/#1121: ImageMagick's native path buffer is MAX_PATH (260) — a longer path fails
        /// deep inside the encoder as MagickCoderErrorException "WriteBlob Failed", after the work
        /// is done, and historically got swallowed into a silent PDF skip. Signal well before,
        /// naming the path and its length.
        /// </summary>
        public const int MaxSafeImagePathLength = 250;

        public static void EnsurePathWithinLimit(string path)
        {
            if (path.Length > MaxSafeImagePathLength)
            {
                throw new InvalidOperationException(
                    $"Image path is {path.Length} chars (> {MaxSafeImagePathLength}): {path}. "
                    + "This exceeds the safe limit before the native MAX_PATH(260) ImageMagick write would fail (#1177/#1121). "
                    + "Run from the short junction (D:\\A1114); note that Git-Bash/MSYS2 resolves the junction at spawn and adds ~30 chars.");
            }
        }



        public static MagickImage LoadImageFromPath(string sourceFile)
        {
            var settings = new MagickReadSettings();
            settings.ColorSpace = ColorSpace.sRGB;
            return new MagickImage(sourceFile);
        }

        public static MagickImage LoadImageFromEmbeddedUrl(string srcUrl)
        {
            var settings = new MagickReadSettings
            {
                ColorSpace = ColorSpace.sRGB
            };

            var match = urlExtractorRegex.Match(srcUrl);
            if (match.Success)
            {
                var base64Content = match.Groups[base64ContentGroupName].Value;
                byte[] imageContent = Convert.FromBase64String(base64Content);
                return new MagickImage(imageContent, settings);
            }

            // Gérer le cas où l'URL est un SVG embarqué (pas en base64)
            var svgIndex = srcUrl.IndexOf("<svg", StringComparison.InvariantCultureIgnoreCase);
            if (svgIndex != -1)
            {
                var svgString = srcUrl.Substring(svgIndex);
                byte[] byteArray = Encoding.UTF8.GetBytes(svgString);
                using (var stream = new MemoryStream(byteArray))
                {
                    var readSettings = new MagickReadSettings() { Format = MagickFormat.Svg };
                    return new MagickImage(stream, readSettings);
                }
            }

            // Si ce n'est ni base64, ni un SVG direct, on suppose que c'est une URL standard.
            // Cette partie reste un point de défaillance potentiel si l'URL n'est pas valide.
            // Pour l'instant on retourne une exception claire.
            throw new NotSupportedException($"The provided image URL is not a valid data URL (base64) or an embedded SVG: {srcUrl}");
        }

        public static string GetImageFileName(AssetConverterConfig config, DocumentConfig docConfig, string language, string cardSetName, string imageName, bool isBack = false)
        {

	        var cardSetFolderName = GetImageFolder(config, docConfig, language, cardSetName, isBack);

	        var imageFileName = $"{imageName.RemoveInvalidFileNameChars().Replace(" ", "_")}.{docConfig.ImageFormat.ToString().ToLowerInvariant()}";
	        return Path.Combine(cardSetFolderName, imageFileName);
        }

		public static string GetImageFolder(AssetConverterConfig config, DocumentConfig docConfig, string language, string cardSetName, bool isBack = false)
		{
			var imagesFolderName = config.GetImagesDirectory(language);

			//var densityFolderName = Path.Combine(imagesFolderName, $@"density-{docConfig.TargetDensity}\");
			var densityFolderName = docConfig.GetDensityDirectory(imagesFolderName);
			var cardSetFolderName = Path.Combine(densityFolderName, $@"{cardSetName}\");

			// Issue #28 (a): optionally split front/back images into distinct sub-folders.
			if (config.SeparateFrontBackFolders)
			{
				cardSetFolderName = Path.Combine(cardSetFolderName, isBack ? @"back\" : @"front\");
			}

			Directory.CreateDirectory(cardSetFolderName);

			return cardSetFolderName;

		}

		public static string LoadAndProcessImageUrl(this DocumentCardSet documentCardSet, string language, bool isBack, AssetConverterConfig config, CardSetDocumentConfig docConfig,
             string imageName, string imageUrl, double sourceDpi)
        {
	        string toReturn;

			var imagesFolderName = config.GetImagesDirectory(language);

			var imageFileName = GetImageFileName(config, docConfig, language, documentCardSet.CardSetName, imageName, isBack);

			EnsurePathWithinLimit(imageFileName);

			         if (File.Exists(imageFileName))
            {
				Logger.Log($"Skip existing image: {imageFileName}");
				toReturn = imageFileName;
			}
            else
            {
                // #29 fix: deterministic dispose of MagickImage after processing
                using var imageFromEmbeddedUrl = imageUrl switch
                {
                    _ when imageUrl.StartsWith("data:image") => ImageHelper.LoadImageFromEmbeddedUrl(imageUrl),
                    _ when imageUrl.PathIsUrl() => ImageHelper.LoadImageFromEmbeddedUrl(imageUrl),
                    _ => ImageHelper.LoadImageFromPath(imageUrl)
                };
                imageFromEmbeddedUrl.Density = new Density(sourceDpi);
                if (documentCardSet.SaveOriginalImage)
                {
                    var originalFolderName = Path.Combine(imagesFolderName, $@"original\");
                    if (!Directory.Exists(originalFolderName))
                    {
                        Directory.CreateDirectory(originalFolderName);
                    }
                    var cardSetOriginalFolderName = Path.Combine(originalFolderName, $@"{documentCardSet.CardSetName}\");
                    if (!Directory.Exists(cardSetOriginalFolderName))
                    {
                        Directory.CreateDirectory(cardSetOriginalFolderName);
                    }
                    var imageOriginalFileName = $"{imageName}.png";
                    imageOriginalFileName = Path.Combine(cardSetOriginalFolderName, imageOriginalFileName);
                    EnsurePathWithinLimit(imageOriginalFileName);
                    if (!File.Exists(imageOriginalFileName))
                    {
                        imageFromEmbeddedUrl.Write(imageOriginalFileName);

                        Logger.Log($"Saved image: {imageOriginalFileName}");
					}
                }

                if (documentCardSet.GetConvertToCmyk(config))
                {
                    imageFromEmbeddedUrl.ConvertToCmyk();
                }

                var documentCard = documentCardSet.FrontCards;
                if (isBack)
                {
                    documentCard = documentCardSet.BackCards;
                }

                // ✅ FIX CRITIQUE: Corriger le DPI avant toute opération
                // Les images de CardPen peuvent avoir un metadata DPI incorrect (~400 DPI)
                // On force le DPI à 300 pour garantir un redimensionnement correct
                // Cela évite les dimensions 8x trop grandes dans les PDFs finaux
                imageFromEmbeddedUrl.Density = new Density(300, DensityUnit.PixelsPerInch);

                if (documentCard.WidthMM > 0 && documentCard.HeigthMM > 0)
                {
                    imageFromEmbeddedUrl.ResizeInMM(documentCard.WidthMM, documentCard.HeigthMM, documentCard.BorderMM);
                }

                if (docConfig.TargetDensity > 0)
                {
                    imageFromEmbeddedUrl.Resample(docConfig.TargetDensity, docConfig.TargetDensity);
                }
                imageFromEmbeddedUrl.Write(imageFileName, docConfig.ImageFormat);
                Logger.LogSuccess($"Saved image: {imageFileName}");
                toReturn = imageFileName;
			}

            return toReturn;
        }


        public static void ConvertToCmyk(this MagickImage image)
        {

            image.Alpha(AlphaOption.Remove);
            image.Settings.BackgroundColor = MagickColors.White;
            //image.TransformColorSpace(ColorProfile.SRGB, ColorProfile.USWebCoatedSWOP);
            image.TransformColorSpace( ColorProfiles.USWebCoatedSWOP, ColorTransformMode.Quantum);

            image.ColorSpace = ColorSpace.CMYK;
            image.Settings.ColorSpace = ColorSpace.CMYK;

        }

        /// <summary>
        /// Resizes a harvested card to its document geometry <b>without deforming it</b>: the source
        /// aspect ratio is preserved, the image is scaled to cover the target box, and the excess —
        /// the gabarit's bleed — is cropped away, centred.
        /// </summary>
        /// <remarks>
        /// <para>Issue #1250. Until 2026-09 this method set <c>IgnoreAspectRatio = true</c> and
        /// stretched the harvest straight onto <c>WidthMM x HeigthMM</c>. Because CardPen renders each
        /// card at its gabarit size <i>plus</i> a bleed (5 mm on the Fallacies face, 3 mm on its back,
        /// 5 mm on Scenarii, 0 on others), the source ratio never matched the target and the bleed was
        /// squashed into the card instead of being trimmed off. Measured drift on the shipped tree:
        /// +15.1 % vertical stretch on the Fallacies face, +12.8 % Rules, +9.0 % Memo, +4.0 % Scenarii
        /// — and, inside one deck, the back stretched differently from the face. Every card shipped
        /// since April 2024 carried it; nobody saw it because the CardPen preview everyone looked at is
        /// correct, and the deformation happens one stage later, here.</para>
        /// <para>Cover-and-crop is what a bleed is for, so the fix restores the intended meaning rather
        /// than adding a correction on top: scale until both target dimensions are covered, then crop
        /// the overhang. Every shipped gabarit trims to exactly 1.7273 (tarot) or 1.4000 (poker), so
        /// after cropping the drift against the retargeted 70 x 120 mm tarot is 0.76 % and against
        /// 63.5 x 88.9 mm poker is nil.</para>
        /// <para><paramref name="bordermm"/> keeps its original meaning — a white margin inside the
        /// card, the image being fitted into the reduced box. It is 0 on every shipped document.</para>
        /// </remarks>
        public static void ResizeInMM(this MagickImage image, decimal widthmm, decimal lengthmm, decimal bordermm)
        {
            if (image.Density.Units == DensityUnit.Undefined)
            {
                image.Density = new Density(300, DensityUnit.PixelsPerInch);
            }
            image.Density = image.Density.ChangeUnits(DensityUnit.PixelsPerCentimeter);

            var cardGeometry = image.Density.ToGeometry((double)(widthmm / 10), (double)lengthmm / 10);

            IMagickGeometry extentGeometry = null;
            var innerGeometry = cardGeometry;
            if (bordermm > 0)
            {
                extentGeometry = cardGeometry;
                var innerWidthMM = widthmm - (2 * bordermm);
                var innerLengthMM = lengthmm - (2 * bordermm);
                innerGeometry = image.Density.ToGeometry((double)(innerWidthMM / 10), (double)innerLengthMM / 10);
            }

            CoverAndCrop(image, innerGeometry);

            if (extentGeometry != null)
            {
                image.BorderColor = MagickColors.White;
                image.BackgroundColor = MagickColors.White;
                image.MatteColor = MagickColors.White;
                image.Extent(extentGeometry, Gravity.Center, MagickColors.White);
            }
        }

        /// <summary>
        /// Scales <paramref name="image"/> so it covers <paramref name="target"/> with its own aspect
        /// ratio intact, then crops the overhang from the centre. The result is exactly
        /// <paramref name="target"/>, and no pixel has been stretched.
        /// </summary>
        internal static void CoverAndCrop(MagickImage image, IMagickGeometry target)
        {
            if (image.Width == 0 || image.Height == 0 || target.Width == 0 || target.Height == 0)
            {
                return;
            }

            var scale = Math.Max(
                (double)target.Width / image.Width,
                (double)target.Height / image.Height);

            var coverWidth = (uint)Math.Max(target.Width, Math.Round(image.Width * scale));
            var coverHeight = (uint)Math.Max(target.Height, Math.Round(image.Height * scale));

            if (coverWidth != image.Width || coverHeight != image.Height)
            {
                // Proportional by construction; IgnoreAspectRatio only pins the exact pixel count so
                // rounding cannot leave the cover one pixel short of the crop box.
                image.AdaptiveResize(new MagickGeometry(coverWidth, coverHeight) { IgnoreAspectRatio = true });
            }

            if (image.Width != target.Width || image.Height != target.Height)
            {
                image.Crop(new MagickGeometry(target.Width, target.Height), Gravity.Center);
                image.ResetPage();
            }
        }

        internal static void Modulate(MagickImage image, double modulation)
        {
            image.Modulate(new Percentage(100), new Percentage(100), new Percentage(modulation));
        }
    }
}