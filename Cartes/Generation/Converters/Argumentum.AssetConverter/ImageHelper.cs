﻿using System;
using System.IO;
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
            var base64Content = urlExtractorRegex.Match(srcUrl).Groups[base64ContentGroupName].Captures[0].Value;
            byte[] imageContent = Convert.FromBase64String(base64Content);

            return new MagickImage(imageContent);
        }



        public static MagickImage LoadAndProcessImageUrl(this DocumentCardSet documentCardSet, WebBasedGeneratorConfig config, DocumentConfig docConfig,
             string imageName, string imageUrl, double sourceDpi)
        {
            MagickImage toReturn;
            var imagesFolderName = config.GetImagesDirectory();

            var densityFolderName = Path.Combine(imagesFolderName, $@".\density-{docConfig.TargetDensity}\");
            if (!Directory.Exists(densityFolderName))
            {
                Directory.CreateDirectory(densityFolderName);
            }

            var cardSetFolderName = Path.Combine(densityFolderName, $@".\{documentCardSet.CardSetName}\");
            if (!Directory.Exists(cardSetFolderName))
            {
                Directory.CreateDirectory(cardSetFolderName);
            }

            var imageFileName = $"{imageName}.{docConfig.ImageFormat.ToString().ToLowerInvariant()}";
            imageFileName = Path.Combine(cardSetFolderName, imageFileName);
            if (File.Exists(imageFileName))
            {
                toReturn = new MagickImage(imageFileName);
            }
            else
            {
                toReturn = ImageHelper.LoadImageFromEmbeddedUrl(imageUrl);
                toReturn.Density = new Density(sourceDpi);
                if (documentCardSet.SaveOriginalImage)
                {
                    var originalFolderName = Path.Combine(imagesFolderName, $@".\original\");
                    if (!Directory.Exists(originalFolderName))
                    {
                        Directory.CreateDirectory(originalFolderName);
                    }
                    var cardSetOriginalFolderName = Path.Combine(originalFolderName, $@".\{documentCardSet.CardSetName}\");
                    if (!Directory.Exists(cardSetOriginalFolderName))
                    {
                        Directory.CreateDirectory(cardSetOriginalFolderName);
                    }
                    var imageOriginalFileName = $"{imageName}.png";
                    imageOriginalFileName = Path.Combine(cardSetOriginalFolderName, imageOriginalFileName);
                    toReturn.Write(imageOriginalFileName);

                }

                if (documentCardSet.ConvertToCmyk)
                {
                    toReturn.ConvertToCmyk();
                }
                if (documentCardSet.WidthMM > 0 && documentCardSet.HeigthMM > 0)
                {
                    toReturn.ResizeInMM(documentCardSet.WidthMM, documentCardSet.HeigthMM, documentCardSet.BorderMM);
                }

                if (docConfig.TargetDensity > 0)
                {
                    toReturn.Resample(docConfig.TargetDensity, docConfig.TargetDensity);
                }

                toReturn.Write(imageFileName, docConfig.ImageFormat);
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