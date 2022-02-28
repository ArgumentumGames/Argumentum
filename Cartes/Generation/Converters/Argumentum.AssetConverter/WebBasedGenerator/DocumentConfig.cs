using System.Collections.Generic;
using ImageMagick;

namespace Argumentum.AssetConverter
{
    public class DocumentConfig
    {
        public string DocumentName { get; set; }

        public List<DocumentCardSet> CardSets { get; set; }

        public int TargetDensity { get; set; } = 0;

        public MagickFormat ImageFormat { get; set; } = MagickFormat.Png;

        public CardDocumentFormat DocumentFormat { get; set; } = CardDocumentFormat.BackFirstOneDocPerBack;

    }

    public enum CardDocumentFormat
    {
        AlternateFaceAndBack,
        BackFirstOneDocPerBack
    }


}