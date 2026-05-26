using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities
{
    public class Rule : CsvBase<Rule, RuleClassMap>, ICsvBase
    {
        public string Pk { get; set; }

        /// <summary>
        /// Returns Pk if set, otherwise generates sequential ID from RowIndex.
        /// </summary>
        public new string GetId()
        {
            return !string.IsNullOrEmpty(Pk) ? Pk : $"Rules_{RowIndex + 1:D2}";
        }

        public string Text { get; set; }
        public string Text_en { get; set; }
        public string Text_ru { get; set; }
        public string Text_pt { get; set; }
        public string Text_es { get; set; }
        public string PrintAndPlay { get; set; }
    }

    public sealed class RuleClassMap : ClassMap<Rule>
    {
        public RuleClassMap()
        {
            Map(m => m.Pk).Name("pk").Optional();
            Map(m => m.Text).Name("Text");
            Map(m => m.Text_en).Name("Text_en");
            Map(m => m.Text_ru).Name("Text_ru");
            Map(m => m.Text_pt).Name("Text_pt");
            Map(m => m.Text_es).Name("Text_es");
            Map(m => m.PrintAndPlay).Name("print_and_play").Optional();
        }
    }
}