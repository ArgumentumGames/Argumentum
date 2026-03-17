using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities
{
    public class Rule : CsvBase<Rule, RuleClassMap>, ICsvBase
    {
        // public string Id => System.Text.RegularExpressions.Regex.Replace(Text ?? string.Empty, "[^a-zA-Z0-9]", "").Substring(0, Math.Min(Text?.Length ?? 0, 10));

        public string GetId()
        {
            return string.Empty;
        }

        public string Text { get; set; }
        public string Text_en { get; set; }
        public string Text_ru { get; set; }
        public string Text_pt { get; set; }
        public string print_and_play { get; set; }
    }

    public sealed class RuleClassMap : ClassMap<Rule>
    {
        public RuleClassMap()
        {
            Map(m => m.Text).Name("Text");
            Map(m => m.Text_en).Name("Text_en");
            Map(m => m.Text_ru).Name("Text_ru");
            Map(m => m.Text_pt).Name("Text_pt");
        }
    }
}