using System;
using System.IO;
using ImageMagick;

namespace Argumentum.AssetConverter
{
    public class BatchImageConverterConfig
    {
        public string SourcePath { get; set; } = @"..\..\..\Data\Source";
        public string DestPath { get; set; } = @"..\..\..\Data\Target";


        public void Apply()
        {
            var objSourceDir = new DirectoryInfo(SourcePath);
            var objTargetDir = new DirectoryInfo(DestPath);
            BatchImagePngToCnykJpegsInternal(objSourceDir, objTargetDir);

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
                        //Console.WriteLine($"Width {info.Width}");
                        //Console.WriteLine($"Height {info.Height}");
                        //Console.WriteLine($"ColorSpace {info.ColorSpace}");
                        //Console.WriteLine($"Format {info.Format}");
                        //Console.WriteLine($"Density.X {info.Density.X}");
                        //Console.WriteLine($"Density.Y {info.Density.Y}");
                        //Console.WriteLine($"Density.Units {info.Density.Units}");
                        Console.WriteLine($"Image Converted: {targetFile.Directory?.Name}\\{targetFile.Name}");

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