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
    public abstract class CsvBase<T, TMap> : ICsvBase where T : CsvBase<T, TMap>, new() where TMap : ClassMap<T>, new()
{
    public string Id { get; set; }

    /// <summary>
    /// 0-based index of this row in the CSV file. Assigned during LoadFromContent.
    /// </summary>
    public int RowIndex { get; set; }

    public string GetId()
    {
        return Id;
    }

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

    public static IList<T> LoadFromContent(string fileContent)
    {
        IEnumerable<T> items;
        using (var reader = new StringReader(fileContent))
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => RemoveDiacritics(args.Header.ToLower().Replace("_", "").Replace("-", "").Replace(" ", "")),
                MissingFieldFound = args =>
                {
                    // Log missing fields for debugging instead of silently ignoring
                    var headerNames = args.HeaderNames != null ? string.Join(", ", args.HeaderNames) : "unknown";
                    Logger.Log($"[CSV Warning] Missing field '{headerNames}' at index {args.Index} in {typeof(T).Name}");
                },
            };
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<TMap>();
                items = csv.GetRecords<T>().ToList();
            }
        }
        var itemList = items.ToList();
        // Assign sequential row indices
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].RowIndex = i;
        }
        Logger.Log($"Loaded {itemList.Count} items");
        return itemList;
    }
    }
}