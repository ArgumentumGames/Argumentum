using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using CsvHelper;

namespace Argumentum.AssetConverter
{
    public class CardSetConfig
    {

        public string Name { get; set; }


        public string FaceExampleName
        {
            get => null;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    FaceCardSetInfo.ExampleName = value;
                }
            }
        }

        public CardSetInfo FaceCardSetInfo { get; set; } = new CardSetInfo();


        public string BackExampleName
        {
            get => null;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    BackCardSetInfo.ExampleName = value;
                }
            }
        }

        public CardSetInfo BackCardSetInfo { get; set; } = new CardSetInfo();


        

        public bool PauseFaceForEdits { get; set; }

        public bool PauseBackForEdits { get; set; }

        public string GetHarvestSerializationName(WebBasedGeneratorConfig config)
        {
            return Path.Combine(config.GetHarvestDirectory(), $"{Name}-harvest.json");
        }

    }

    public enum CardSetType
    {
        ExampleByName,
        CustomJson
    }


    public class CardSetInfo
    {

        public bool IsSet => (CardSetType == CardSetType.ExampleByName && !string.IsNullOrEmpty(ExampleName))
                             || (CardSetType == CardSetType.CustomJson && !string.IsNullOrEmpty(CustomJsonFileName));

        public CardSetType CardSetType { get; set; }

        public string ExampleName { get; set; }


        public string CustomJsonFileName { get; set; }

    }

}