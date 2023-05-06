using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using CsvHelper;

namespace Argumentum.AssetConverter
{
    public class CardSetConfig
    {

        public string Name { get; set; }


		public CardSetInfo FaceCardSetInfo { get; set; } = new CardSetInfo();

        public CardSetInfo BackCardSetInfo { get; set; } = new CardSetInfo();




        public string GetHarvestSerializationName(WebBasedGeneratorConfig config, string language)
        {
            return Path.Combine(config.GetHarvestDirectory(language), $"{Name}_harvest_{language}.json");
        }

    }
}