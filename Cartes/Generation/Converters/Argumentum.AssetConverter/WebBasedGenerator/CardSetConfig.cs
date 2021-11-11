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

        public int TargetDensity { get; set; } = 0;

        public MagickFormat ImageFormat { get; set; } = MagickFormat.Png;

    }


    public class DocumentCardSet
    {

        public string CardSetName { get; set; }


        //public string TemplateDocumentName { get; set; }

        public int NbCopies { get; set; } = 1;


        public bool SaveOriginalImage { get; set; } = true;

        public bool ConvertToCmyk { get; set; } = true;

        public decimal HeigthMM { get; set; }

        public decimal WidthMM { get; set; }

        public decimal BorderMM { get; set; }

       


        

    }

    public class CardSetConfig
    {

        public string Name { get; set; }


        public string FaceExampleName { get; set; }

        public string BackExampleName { get; set; }

        public bool PauseFaceForEdits { get; set; }

        public bool PauseBackForEdits { get; set; }

        public string GetHarvestSerializationName(WebBasedGeneratorConfig config)
        {
            return Path.Combine(config.GetHarvestDirectory(), $"{Name}-harvest.json");
        }

    }
}