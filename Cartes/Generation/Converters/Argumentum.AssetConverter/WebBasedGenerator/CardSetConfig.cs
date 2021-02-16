using System.Collections.Generic;
using System.IO;
using ImageMagick;

namespace Argumentum.AssetConverter
{

    public class DocumentConfig
    {


        public string DocumentName { get; set; }

        public List<DocumentCardSet> CardSets { get; set; }

    }


    public class DocumentCardSet
    {


        public string CardSetName { get; set; }


        public string TemplateDocumentName { get; set; }

        public int NbCopies { get; set; } = 1;

        public decimal HeigthMM { get; set; }

        public decimal WidthMM { get; set; }

        public decimal BorderMM { get; set; }


        public MagickImage LoadAndProcessImageUrl(string imageUrl)
        {
            var toReturn = ImageHelper.LoadImageFromEmbeddedUrl(imageUrl);
            ImageHelper.ConvertToCmyk(toReturn);
            if (WidthMM > 0 && HeigthMM > 0)
            {
                ImageHelper.ResizeInMM(toReturn, WidthMM, HeigthMM, BorderMM);
            }

            return toReturn;
        }

    }

    public class CardSetConfig
    {

        public string Name { get; set; }


        public string FaceExampleName { get; set; }

        public string BackExampleName { get; set; }


        

        public string GetSerializationName()
        {
            return Path.Combine(System.Environment.CurrentDirectory, $"{Name}-harvest.json");
        }

    }
}