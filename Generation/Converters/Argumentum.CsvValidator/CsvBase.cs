using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Argumentum.CsvValidator
{
    public interface IMindMapItem
    {
        string Path { get; }
        int Depth { get; }
        string Family { get; }
        string SubFamily { get; }
        string SubSubFamily { get; }
        string Title { get; }
        string Text { get; }
        string Description { get; }
        string Example { get; }
        string Link { get; }
        int? Carte { get; }
        string Pk { get; set; }
        string DecimalPath { get; }
    }
    public abstract class CsvBase<T, TMap> where T : CsvBase<T, TMap>, new() where TMap : ClassMap<T>, new()
    {
        public static IList<T> Load(string filePath, BadDataFound? badDataFound = null)
        {
            var fileContent = File.ReadAllText(filePath);
            return LoadFromContent(fileContent, badDataFound);
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static IList<T> LoadFromContent(string fileContent, BadDataFound? badDataFound = null)
        {
            IEnumerable<T> items;
            using (var reader = new StringReader(fileContent))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    PrepareHeaderForMatch = args => {
                        var header = args.Header;
                        var transformedHeader = RemoveDiacritics(header.ToLower().Replace("_", "").Replace("-", "").Replace(" ", ""));
                        System.Console.WriteLine($"Header: '{header}' -> Transformed: '{transformedHeader}'");
                        return transformedHeader;
                    },
                    MissingFieldFound = null,
                    BadDataFound = badDataFound
                };
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<TMap>();
                    items = csv.GetRecords<T>().ToList();
                }
            }
            return items.ToList();
        }
    }
}