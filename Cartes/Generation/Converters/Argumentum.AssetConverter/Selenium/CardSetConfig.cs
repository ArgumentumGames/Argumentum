using ImageMagick;

namespace Argumentum.AssetConverter
{
    public class CardSetConfig
    {

        public string Name { get; set; }

        public string DocumentName { get; set; }

        public int NbCopies { get; set; } = 1;

        public decimal HeigthMM { get; set; }

        public decimal WidthMM { get; set; }

        public string TemplateDocumentName { get; set; }

        public string FaceExampleName { get; set; }

        public string BackExampleName { get; set; }


        public MagickImage LoadAndProcessImageUrl(string imageUrl)
        {
            var toReturn = ImageHelper.LoadImageFromEmbeddedUrl(imageUrl);
            ImageHelper.ConvertToCmyk(toReturn);
            if (WidthMM>0 && HeigthMM>0)
            {
                ImageHelper.ResizeInMM(toReturn, WidthMM, HeigthMM);
            }
            
            return toReturn;
        }


    }
}