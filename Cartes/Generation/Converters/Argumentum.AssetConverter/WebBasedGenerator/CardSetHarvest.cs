using System.Collections.Generic;

namespace Argumentum.AssetConverter
{
    public class CardSetHarvest
    {
        public CardPenHarvest Faces { get; set; } = new CardPenHarvest();
        public CardPenHarvest Backs { get; set; } = new CardPenHarvest();

    }


    public class CardPenHarvest
    {

        public double Dpi { get; set; }

        public Dictionary<string, string> Images { get; set; } = new Dictionary<string, string>();

    }


}