using System.Collections.Generic;
using ImageMagick;

namespace Argumentum.AssetConverter
{
    public class DocumentConfig
    {
        public bool Enabled { get; set; } = true;

        public string DocumentName { get; set; }

      

		public List<(string sourceLanguage, string targetLanguage)> Translations { get; set; } =
			new List<(string sourceLanguage, string targetLanguage)>();

		public List<DocumentCardSet> CardSets { get; set; }

        public int TargetDensity { get; set; } = 0;

        public MagickFormat ImageFormat { get; set; } = MagickFormat.Png;

        public CardDocumentFormat DocumentFormat { get; set; } = CardDocumentFormat.BackFirstOneDocPerBack;

        public bool NoBack { get; set; }

        public string PageSize { get; set; } = "A4";

        public string Header { get; set; } = "";
    }
}