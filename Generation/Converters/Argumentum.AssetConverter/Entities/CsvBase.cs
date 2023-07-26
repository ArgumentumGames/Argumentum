using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities;

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

	private static IList<T> LoadFromContent(string fileContent)
	{
		IEnumerable<T> items;
		using (var reader = new StringReader(fileContent))
		using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
		{
			csv.Context.RegisterClassMap<TMap>();
			items = csv.GetRecords<T>().SkipLast(1).ToList();
		}
		Logger.Log($"Loaded {items.Count()} items");
		return items.ToList();
	}
}