using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter;



public class DataSetInfo
{
	public string Name { get; set; }

	public string ReleaseFilePath { get; set; }
	public string DebugFilePath { get; set; }

	public string FilePath(bool debug) =>
		debug && !string.IsNullOrEmpty(DebugFilePath) ? DebugFilePath : ReleaseFilePath;

	private string _StringContent;

	public async Task<string> GetContent(bool debugPath)
	{
		var strPath = FilePath(debugPath);
		if (string.IsNullOrEmpty(_StringContent))
		{
			
			var payLoad = await strPath.GetDocumentPayload();
			_StringContent = Encoding.UTF8.GetString(payLoad.Content);
		}
		else
		{
			Logger.Log($"Using cached Value for {strPath}");
		}

		return _StringContent;
	}
	
	public async Task<string> GetContent(bool debugPath, string csvFilterField, IList<string> csvFilterValues)
	{
		var content = await GetContent(debugPath);

		if (!string.IsNullOrEmpty(csvFilterField) && csvFilterValues != null && csvFilterValues.Any())
		{
			using var textReader = new StringReader(content);
			using var csvReader = new CsvReader(textReader, CultureInfo.InvariantCulture);

			using var dr = new CsvDataReader(csvReader);
			var dataTable = new DataTable();
			dataTable.Load(dr);

			var filteredRows = dataTable.AsEnumerable()
				.Where(row => csvFilterValues.Contains(row[csvFilterField]?.ToString()));

			var filteredTable = dataTable.Clone();
			foreach (var row in filteredRows)
			{
				filteredTable.ImportRow(row);
			}

			using var textWriter = new StringWriter();
			using var csvWriter = new CsvWriter(textWriter, CultureInfo.InvariantCulture);

			csvWriter.ExportDataTable(filteredTable);

			content = textWriter.ToString();
		}

		return content;
	}
}