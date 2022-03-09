using System.Text.Json.Serialization;

namespace Argumentum.AssetConverter
{
    public class CardSetInfo
    {
        [JsonIgnore()]
        public bool IsSet => (CardSetType == CardSetType.ExampleByName && !string.IsNullOrEmpty(ExampleName))
                             || (CardSetType == CardSetType.CustomJson && !string.IsNullOrEmpty(CustomJsonFileName));

        public CardSetType CardSetType { get; set; }

        public string ExampleName { get; set; }


        public string CustomJsonFileName { get; set; }

        public bool PauseForEdits { get; set; }

    }
}