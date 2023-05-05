using System;
using System.IO;
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

            var settings = new MagickReadSettings();
            settings.ColorSpace = ColorSpace.sRGB;
            if (urlExtractorRegex.IsMatch(srcUrl))
            {
                var base64Content = urlExtractorRegex.Match(srcUrl).Groups[base64ContentGroupName].Captures[0].Value;
                byte[] imageContent = Convert.FromBase64String(base64Content);

                return new MagickImage(imageContent);
            }
            else
            {
                var readSettings = new MagickReadSettings() { Format = MagickFormat.Svg };
                var svgString = srcUrl.Substring(srcUrl.IndexOf("<svg", StringComparison.InvariantCultureIgnoreCase));
                byte[] byteArray = Encoding.UTF8.GetBytes(svgString);
                MemoryStream stream = new MemoryStream(byteArray);
                using (var objStream = new MemoryStream(byteArray))
                {
                    return new MagickImage(objStream, readSettings);
                }
            }
            
        }



        public static string LoadAndProcessImageUrl(this DocumentCardSet documentCardSet, string language, bool isBack, WebBasedGeneratorConfig config, CardSetDocumentConfig docConfig,
             string imageName, string imageUrl, double sourceDpi)
        {
	        string toReturn;
            MagickImage imageFromEmbeddedUrl;
            var imagesFolderName = config.GetImagesDirectory(language);

            var densityFolderName = Path.Combine(imagesFolderName, $@"density-{docConfig.TargetDensity}\");
            if (!Directory.Exists(densityFolderName))
            {
                Directory.CreateDirectory(densityFolderName);
            }

            var cardSetFolderName = Path.Combine(densityFolderName, $@"{documentCardSet.CardSetName}\");
            if (!Directory.Exists(cardSetFolderName))
            {
                Directory.CreateDirectory(cardSetFolderName);
            }

            var imageFileName = $"{imageName}.{docConfig.ImageFormat.ToString().ToLowerInvariant()}";
            imageFileName = Path.Combine(cardSetFolderName, imageFileName);
            if (File.Exists(imageFileName))
            {
				//imageFromEmbeddedUrl = new MagickImage(imageFileName);
				Console.WriteLine($"{WebBasedGenerator.StopWatch.Elapsed}: Skipping Existing image: {imageFileName}");
				toReturn = imageFileName;
			}
            else
            {
                imageFromEmbeddedUrl = ImageHelper.LoadImageFromEmbeddedUrl(imageUrl);
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
                        Console.WriteLine($"{WebBasedGenerator.StopWatch.Elapsed}: Saved image: {imageOriginalFileName}");
					}
                }

                if (documentCardSet.ConvertToCmyk)
                {
                    imageFromEmbeddedUrl.ConvertToCmyk();
                }

                var documentCard = documentCardSet.FrontCards;
                if (isBack)
                {
                    documentCard = documentCardSet.BackCards;
                }

                if (documentCard.WidthMM > 0 && documentCard.HeigthMM > 0)
                {
                    imageFromEmbeddedUrl.ResizeInMM(documentCard.WidthMM, documentCard.HeigthMM, documentCard.BorderMM);
                }

                if (docConfig.TargetDensity > 0)
                {
                    imageFromEmbeddedUrl.Resample(docConfig.TargetDensity, docConfig.TargetDensity);
                }
                imageFromEmbeddedUrl.Write(imageFileName, docConfig.ImageFormat);
                Console.WriteLine($"{WebBasedGenerator.StopWatch.Elapsed}: Saved image: {imageFileName}");
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