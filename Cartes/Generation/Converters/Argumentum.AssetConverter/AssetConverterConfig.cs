using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImageMagick;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;

namespace Argumentum.AssetConverter
{

    public enum ConverterMode
    {
        BatchImagePngToCnykJpegs
    }

   public class AssetConverterConfig
    {

        public string SourcePath { get; set; } = @"..\..\..\Data\Source";
        public string DestPath { get; set; } = @"..\..\..\Data\Target";
        public ConverterMode Mode { get; set; }

        public static AssetConverterConfig GetConfig(string path)
        {
            AssetConverterConfig toReturn;
            CompositeResolver.RegisterAndSetAsDefault(new IJsonFormatter[] { new TimeSpanFormatter() }, new IJsonFormatterResolver[] { StandardResolver.Default });
            if (!File.Exists(path))
            {
                toReturn = new AssetConverterConfig();
                var strNewConfig = JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(toReturn));
                File.WriteAllText(path, strNewConfig);
            }
            using var configStream = File.OpenRead(path);
            toReturn = JsonSerializer.Deserialize<AssetConverterConfig>(configStream);
            return toReturn;
        }


        public bool Apply()
        {
            switch (Mode)
            {
                case ConverterMode.BatchImagePngToCnykJpegs:
                    BatchImagePngToCnykJpegs();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        public void BatchImagePngToCnykJpegs()
        {
            var objSourceDir = new DirectoryInfo(SourcePath);
            var objTargetDir = new DirectoryInfo(DestPath);
            BatchImagePngToCnykJpegsInternal(objSourceDir, objTargetDir);

        }


        private void BatchImagePngToCnykJpegsInternal(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            foreach (var sourceFile in sourceDir.GetFiles())
            {
                if (sourceFile.Extension.ToLower()==".png")
                {

                    var settings = new MagickReadSettings();
                    settings.ColorSpace = ColorSpace.sRGB;

                    using (var image = new MagickImage(sourceFile))
                    {
                        image.Alpha(AlphaOption.Remove);
                        image.Settings.BackgroundColor = MagickColors.White;
                        image.TransformColorSpace(ColorProfile.SRGB, ColorProfile.USWebCoatedSWOP);
                        
                        image.ColorSpace = ColorSpace.CMYK;
                        image.Settings.ColorSpace = ColorSpace.CMYK;




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
