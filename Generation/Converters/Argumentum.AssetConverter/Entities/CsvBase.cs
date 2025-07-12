using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities
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
        string Id { get; set; }
        string PK { get; }
        string DecimalPath { get; }
    }
    public abstract class CsvBase<T, TMap> where T : CsvBase<T, TMap>, new() where TMap : ClassMap<T>, new()
{
    public static IList<T> Load(string filePath)
    {
        Logger.Log($"Loading csv from file {filePath}");
        var fileContent = File.ReadAllText(filePath);
        return LoadFromContent(fileContent);
    }

    public static async Task<IList<T>> LoadAsync(DataSetInfo dataSet, bool debugPath)
    {
        Logger.Log($"Loading csv from dataSet {dataSet.Name}");
        var payLoad = await dataSet.GetContent(debugPath);
        return LoadFromContent(payLoad);
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

    private static IList<T> LoadFromContent(string fileContent)
    {
        IEnumerable<T> items;
        using (var reader = new StringReader(fileContent))
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => RemoveDiacritics(args.Header.ToLower().Replace("_", "").Replace("-", "").Replace(" ", "")),
            };
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<TMap>();
                items = csv.GetRecords<T>().ToList();
            }
        }
        Logger.Log($"Loaded {items.Count()} items");
        return items.ToList();
    }
    }
}