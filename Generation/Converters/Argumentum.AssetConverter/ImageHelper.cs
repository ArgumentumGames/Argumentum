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

        public static string GetImageFileName(AssetConverterConfig config, DocumentConfig docConfig, string language, string cardSetName, string imageName)
        {
	      
	        var cardSetFolderName = GetImageFolder(config, docConfig, language, cardSetName);

	        var imageFileName = $"{imageName.RemoveInvalidFileNameChars().Replace(" ", "_")}.{docConfig.ImageFormat.ToString().ToLowerInvariant()}";
	        return Path.Combine(cardSetFolderName, imageFileName);
        }

		public static string GetImageFolder(AssetConverterConfig config, DocumentConfig docConfig, string language, string cardSetName)
		{
			var imagesFolderName = config.GetImagesDirectory(language);

			//var densityFolderName = Path.Combine(imagesFolderName, $@"density-{docConfig.TargetDensity}\");
			var densityFolderName = docConfig.GetDensityDirectory(imagesFolderName);
			var cardSetFolderName = Path.Combine(densityFolderName, $@"{cardSetName}\");

			Directory.CreateDirectory(cardSetFolderName);

			return cardSetFolderName;
			
		}

		public static string LoadAndProcessImageUrl(this DocumentCardSet documentCardSet, string language, bool isBack, AssetConverterConfig config, CardSetDocumentConfig docConfig,
             string imageName, string imageUrl, double sourceDpi)
        {
	        string toReturn;
            MagickImage imageFromEmbeddedUrl;

			var imagesFolderName = config.GetImagesDirectory(language);

			var imageFileName = GetImageFileName(config, docConfig, language, documentCardSet.CardSetName, imageName);

			         if (File.Exists(imageFileName))
            {
				//imageFromEmbeddedUrl = new MagickImage(imageFileName);

				Logger.Log($"Skip existing image: {imageFileName}");
				toReturn = imageFileName;
			}
            else
            {
                if (imageUrl.StartsWith("data:image"))
                {
                    imageFromEmbeddedUrl = ImageHelper.LoadImageFromEmbeddedUrl(imageUrl);
                }
                else if (imageUrl.PathIsUrl())
                {
                    // This case might be for other URL types in the future,
                    // but for now, we assume it's also an embedded URL.
                    imageFromEmbeddedUrl = ImageHelper.LoadImageFromEmbeddedUrl(imageUrl);
                }
                else
                {
                    imageFromEmbeddedUrl = ImageHelper.LoadImageFromPath(imageUrl);
                }
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
            image.TransformColorSpace( ColorProfile.USWebCoatedSWOP, ColorTransformMode.Quantum);

            image.ColorSpace = ColorSpace.CMYK;
            image.Settings.ColorSpace = ColorSpace.CMYK;

        }

        public static void ResizeInMM(this MagickImage image, decimal widthmm, decimal lengthmm, decimal bordermm)
        {
            if (image.Density.Units == DensityUnit.Undefined)
            {
                image.Density = new Density(300, DensityUnit.PixelsPerInch);
            }
            image.Density = image.Density.ChangeUnits(DensityUnit.PixelsPerCentimeter);
            
            var targetGeometry = image.Density.ToGeometry((double)(widthmm / 10), (double)lengthmm / 10);
            targetGeometry.IgnoreAspectRatio = true;

            IMagickGeometry extentGeometry = null ;
            if (bordermm>0)
            {
                extentGeometry = targetGeometry;
                widthmm = widthmm - (2 * bordermm);
                lengthmm = lengthmm - (2 * bordermm);
                targetGeometry = image.Density.ToGeometry((double)(widthmm / 10), (double)lengthmm / 10);
                targetGeometry.IgnoreAspectRatio = true;
            }

            //image.Resize(targetGeometry);
            
            image.AdaptiveResize(targetGeometry);
            if (extentGeometry != null)
            {
                image.BorderColor = MagickColors.White;
                image.BackgroundColor = MagickColors.White;
                image.MatteColor = MagickColors.White;
                image.Extent(extentGeometry, Gravity.Center, MagickColors.White);
            }

        }

        internal static void Modulate(MagickImage image, double modulation)
        {
            image.Modulate(new Percentage(100), new Percentage(100), new Percentage(modulation));
        }
    }
}