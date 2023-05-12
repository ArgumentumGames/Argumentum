using System;
using System.IO;
using ImageMagick;

namespace Argumentum.AssetConverter
{
    public enum BatchImageOperation
    {
        PngToCnyk,
        ModulateHue
    }


    public class BatchImageConverterConfig
    {
        public string SourcePath { get; set; } = @"..\..\..\Data\Source";
        public string DestPath { get; set; } = @"..\..\..\Data\Target";

        public BatchImageOperation Operation { get; set; } = BatchImageOperation.ModulateHue;

        public double Modulation { get; set; } = 200;


        public void Apply()
        {
            var objSourceDir = new DirectoryInfo(SourcePath);
            var objTargetDir = new DirectoryInfo(DestPath);

            switch (Operation)
            {
                case BatchImageOperation.PngToCnyk:
                    BatchImagePngToCnykJpegsInternal(objSourceDir, objTargetDir);
                    break;
                case BatchImageOperation.ModulateHue:
                    BatchImageModulate(objSourceDir, objTargetDir);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            

        }

        private void BatchImageModulate(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            foreach (var sourceFile in sourceDir.GetFiles())
            {
                if (sourceFile.Extension.ToLower() == ".png")
                {

                    using (var image = ImageHelper.LoadImageFromPath(sourceFile.ToString()))
                    {
                        ImageHelper.Modulate(image, Modulation);

                        var targetFile = new FileInfo(Path.Combine(targetDir.ToString(), sourceFile.Name));
                        image.Write(targetFile);
                        
                        Logger.LogSuccess($"Image Converted: {targetFile.Directory?.Name}\\{targetFile.Name}");

                    }

                }
                else
                {
                    var targetFile = Path.Combine(targetDir.ToString(), sourceFile.Name);
                    sourceFile.CopyTo(targetFile, true);
                }
            }
        }


        private void BatchImagePngToCnykJpegsInternal(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            foreach (var sourceFile in sourceDir.GetFiles())
            {
                if (sourceFile.Extension.ToLower() == ".png")
                {

                    using (var image = ImageHelper.LoadImageFromPath(sourceFile.ToString()))
                    {
                       ImageHelper.ConvertToCmyk(image);

                        var targetFile = new FileInfo(Path.Combine(targetDir.ToString(), sourceFile.Name.Replace("png", "jpg")));
                        // Save image as png
                        image.Write(targetFile);
                        //var info = new MagickImageInfo(targetFile);
                        //Logger.Log($"Width {info.Width}");
                        //Logger.Log($"Height {info.Height}");
                        //Logger.Log($"ColorSpace {info.ColorSpace}");
                        //Logger.Log($"Format {info.Format}");
                        //Logger.Log($"Density.X {info.Density.X}");
                        //Logger.Log($"Density.Y {info.Density.Y}");
                        //Logger.Log($"Density.Units {info.Density.Units}");
                        Logger.LogSuccess($"Image Converted: {targetFile.Directory?.Name}\\{targetFile.Name}");

                    }

                }
                else
                {
                    var targetFile = Path.Combine(targetDir.ToString(), sourceFile.Name);
                    sourceFile.CopyTo(targetFile, true);
                }
            }

            foreach (var subSourceDir in sourceDir.GetDirectories())
            {
                var subTargetDir = targetDir.CreateSubdirectory(subSourceDir.Name);
                BatchImagePngToCnykJpegsInternal(subSourceDir, subTargetDir);
            }

        }


    }
}