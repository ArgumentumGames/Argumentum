using System.Collections.Generic;

namespace Argumentum.AssetConverter
{
    public class DocumentConfig
    {
        public bool Enabled { get; set; } = true;

        public string DocumentName { get; set; }

      

		public List<(string sourceLanguage, string targetLanguage)> Translations { get; set; } =
			new List<(string sourceLanguage, string targetLanguage)>();

		
    }
}