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

        [JsonIgnore()]
        public string FaceExampleName
        {
            get => null;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    FaceCardSetInfo.CardSetType = CardSetType.ExampleByName;
                    FaceCardSetInfo.ExampleName = value;
                }
            }
        }

        public CardSetInfo FaceCardSetInfo { get; set; } = new CardSetInfo();

        [JsonIgnore()]
        public string BackExampleName
        {
            get => null;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    BackCardSetInfo.CardSetType = CardSetType.ExampleByName;
                    BackCardSetInfo.ExampleName = value;
                }
            }
        }

        public CardSetInfo BackCardSetInfo { get; set; } = new CardSetInfo();




        public string GetHarvestSerializationName(WebBasedGeneratorConfig config)
        {
            return Path.Combine(config.GetHarvestDirectory(), $"{Name}-harvest.json");
        }

    }
}