using System;
using System.Text.RegularExpressions;
using ImageMagick;

namespace Argumentum.AssetConverter
{
    public class ImageHelper
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


        public static void ConvertToCmyk(MagickImage image)
        {

            image.Alpha(AlphaOption.Remove);
            image.Settings.BackgroundColor = MagickColors.White;
            image.TransformColorSpace(ColorProfile.SRGB, ColorProfile.USWebCoatedSWOP);

            image.ColorSpace = ColorSpace.CMYK;
            image.Settings.ColorSpace = ColorSpace.CMYK;

        }

        public static void ResizeInMM(MagickImage image, decimal widthmm, decimal lengthmm)
        {
            if (image.Density.Units == DensityUnit.Undefined)
            {
                image.Density = new Density(300, DensityUnit.PixelsPerInch);
            }
            image.Density = image.Density.ChangeUnits(DensityUnit.PixelsPerCentimeter);
            var targetGeometry = image.Density.ToGeometry((double)(widthmm / 10), (double)lengthmm / 10);
            targetGeometry.IgnoreAspectRatio = true;
            //image.Resize(targetGeometry);
            image.AdaptiveResize(targetGeometry);


        }



    }
}