using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities
{
    public class Rule : CsvBase<Rule, RuleClassMap>, ICsvBase
    {
        /// <summary>
        /// Returns a sequential ID in format "Rules_01", "Rules_02", etc.
        /// Uses the RowIndex assigned during CSV loading for predictable ordering.
        /// </summary>
        public new string GetId()
        {
            // Use 1-based numbering for user-friendly names (Rules_01, Rules_02, etc.)
            return $"Rules_{RowIndex + 1:D2}";
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