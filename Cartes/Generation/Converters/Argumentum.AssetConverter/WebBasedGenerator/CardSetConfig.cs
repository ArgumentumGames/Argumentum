using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ImageMagick;

namespace Argumentum.AssetConverter
{

    public class DocumentConfig
    {


        public string DocumentName { get; set; }

        public List<DocumentCardSet> CardSets { get; set; }

        public int TargetDensity { get; set; } = 70;

        public MagickFormat ImageFormat { get; set; } = MagickFormat.Png;

    }


    public class DocumentCardSet
    {


        public string CardSetName { get; set; }


        public string TemplateDocumentName { get; set; }

        public int NbCopies { get; set; } = 1;

        public decimal HeigthMM { get; set; }

        public decimal WidthMM { get; set; }

        public decimal BorderMM { get; set; }

       


        public MagickImage LoadAndProcessImageUrl(WebBasedGeneratorConfig config, DocumentConfig docConfig,
            DocumentCardSet documentCardSet, string imageName, string imageUrl)
        {
            MagickImage toReturn;
            var densityFolderName = config.GetImagesDirectory();

            densityFolderName = Path.Combine(densityFolderName, $@".\density-{docConfig.TargetDensity}\");
            if (!Directory.Exists(densityFolderName))
            {
                Directory.CreateDirectory(densityFolderName);
            }

            var cardSetFolderName = Path.Combine(densityFolderName, $@".\{documentCardSet.CardSetName}\");
            if (!Directory.Exists(cardSetFolderName))
            {
                Directory.CreateDirectory(cardSetFolderName);
            }

            imageName = $"{imageName}.{docConfig.ImageFormat.ToString().ToLowerInvariant()}";
            var imageFileName = Path.Combine(cardSetFolderName, imageName);
            if (File.Exists(imageFileName))
            {
                toReturn = new MagickImage(imageFileName);
            }
            else
            {
                toReturn = ImageHelper.LoadImageFromEmbeddedUrl(imageUrl);
                ImageHelper.ConvertToCmyk(toReturn);
                if (WidthMM > 0 && HeigthMM > 0)
                {
                    ImageHelper.ResizeInMM(toReturn, WidthMM, HeigthMM, BorderMM);
                }

                if (docConfig.TargetDensity > 0)
                {
                    toReturn.Resample(docConfig.TargetDensity, docConfig.TargetDensity);
                }

                toReturn.Write(imageFileName, docConfig.ImageFormat);
            }

            return toReturn;
        }

    }

    public class CardSetConfig
    {

        public string Name { get; set; }


        public string FaceExampleName { get; set; }

        public string BackExampleName { get; set; }


        

        public string GetHarvestSerializationName(WebBasedGeneratorConfig config)
        {
            return Path.Combine(config.GetHarvestDirectory(), $"{Name}-harvest.json");
        }

    }
}