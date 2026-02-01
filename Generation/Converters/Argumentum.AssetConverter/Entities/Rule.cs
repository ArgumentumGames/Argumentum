using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities
{
    public class Rule : CsvBase<Rule, RuleClassMap>, ICsvBase
    {
        /// <summary>
        /// Returns a unique ID based on the first 20 alphanumeric characters of the Text.
        /// Falls back to base.Id if Text is empty.
        /// </summary>
        public new string GetId()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return Id ?? string.Empty;

            var sanitized = System.Text.RegularExpressions.Regex.Replace(Text, "[^a-zA-Z0-9]", "");
            return sanitized.Length > 0
                ? sanitized.Substring(0, System.Math.Min(sanitized.Length, 20))
                : Id ?? string.Empty;
        }

        public string Text { get; set; }
        public string Text_en { get; set; }
        public string Text_ru { get; set; }
        public string Text_pt { get; set; }
        public string PrintAndPlay { get; set; }
    }

    public sealed class RuleClassMap : ClassMap<Rule>
    {
        public RuleClassMap()
        {
            Map(m => m.Text).Name("Text");
            Map(m => m.Text_en).Name("Text_en");
            Map(m => m.Text_ru).Name("Text_ru");
            Map(m => m.Text_pt).Name("Text_pt");
            Map(m => m.PrintAndPlay).Name("print_and_play").Optional();
        }
    }
}