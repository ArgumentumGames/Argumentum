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
using Newtonsoft.Json;

namespace Argumentum.AssetConverter;



public class DataSetInfo
{
	public string Name { get; set; }

	public string ReleaseFilePath { get; set; }
	public string DebugFilePath { get; set; }
	public Type CsvType { get; set; }

	public string FilePath(bool debug) =>
		debug && !string.IsNullOrEmpty(DebugFilePath) ? DebugFilePath : ReleaseFilePath;

	private string _StringContent;

	public async Task<string> GetContent(bool useDebugPath)
	{
		var strPath = FilePath(useDebugPath);
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


	public async Task SaveContent(string strPath, string content)
	{

		if (!string.IsNullOrEmpty(content))
		{
			await File.WriteAllTextAsync(strPath, content, Encoding.UTF8);
			Logger.LogSuccess($"Successfully saves {strPath}");
		}
		else
		{
			Logger.Log($"Cannot save empty content to {strPath}");
		}

	}


	public async Task<string> GetContent(bool useDebugPath, string delimiterIn, string primaryKeyColumn, string csvFilterField, IList<string> csvFilterValues, bool startsWithValues)
	{
		var content = await GetContent(useDebugPath);

		if (!string.IsNullOrEmpty(csvFilterField) && csvFilterValues != null && csvFilterValues.Any())
		{
			var dataTable = LoadCsvIntoDataTable(content, delimiterIn, primaryKeyColumn);

			EnumerableRowCollection<DataRow> filteredRows;
			if (startsWithValues)
			{
				filteredRows = dataTable.AsEnumerable()
					.Where(row => csvFilterValues.Any(value=> row[csvFilterField]?.ToString()?.StartsWith(value)??false));
			}
			else
			{
				filteredRows = dataTable.AsEnumerable()
					.Where(row => csvFilterValues.Contains(row[csvFilterField]?.ToString()));
			}
			

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

	public async Task<List<string>> SplitContentIntoChunks(int chunkSize, string delimiterIn, string delimiterOut, string primaryKeyColumn)
	{
		var content = await GetContent(false); // consider handling this async call properly
		var records = LoadCsvIntoDataTable(content, delimiterIn, primaryKeyColumn);

		var chunks = new List<string>();
		var rows = records.Rows.Cast<DataRow>().ToList();
		for (var i = 0; i < rows.Count; i += chunkSize)
		{
			var chunkRecords = rows.Skip(i).Take(chunkSize);
			using var stringWriter = new StringWriter();
			var configOut = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				Delimiter = delimiterOut
			};
			using var csvWriter = new CsvWriter(stringWriter, configOut);
			csvWriter.WriteRecords(chunkRecords);

			chunks.Add(stringWriter.ToString());
		}

		return chunks;
	}




	public string MergeResponsesIntoCsv(List<string> responses, string delimiterIn, string delimiterOut, string primaryKeyColumn)
	{
		var resultTable = new DataTable();

		foreach (var response in responses)
		{
			var dataTable = LoadCsvIntoDataTable(response, delimiterIn, primaryKeyColumn);


			if (resultTable.Columns.Count == 0)
			{
				// Copy columns structure for the first time
				foreach (DataColumn column in dataTable.Columns)
				{
					resultTable.Columns.Add(column.ColumnName, column.DataType);
				}
			}

			// Copy rows
			foreach (DataRow row in dataTable.Rows)
			{
				resultTable.ImportRow(row);
			}
		}

		// Serialize final table into a CSV
		using var stringWriter = new StringWriter();
		var configOut = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			Delimiter = delimiterOut
		};
		using var csvWriter = new CsvWriter(stringWriter, configOut);

		csvWriter.ExportDataTable(resultTable);

		return stringWriter.ToString();
	}


	public async Task<List<string>> SplitContentIntoJsonChunks(int chunkSize, List<string> fieldsToInclude, bool useDebugPath)
	{
		// Get the list of dictionaries using the second method.
		var records = await GetContentDictionary(fieldsToInclude, useDebugPath);

		var chunks = new List<string>();
		for (var i = 0; i < records.Count; i += chunkSize)
		{
			// Chunk the list of dictionaries into smaller parts based on the chunk size.
			var chunkRecords = records.Skip(i).Take(chunkSize);

			// Serialize each chunk to JSON.
			string jsonChunk = JsonConvert.SerializeObject(chunkRecords, Formatting.Indented);

			chunks.Add(jsonChunk);
		}

		return chunks;
	}



	public async Task<List<Dictionary<string, object>>> GetContentDictionary(List<string> fieldsToInclude, bool useDebugPath)
	{
		var toReturn = new List<Dictionary<string, object>>();
		var content = await GetContent(useDebugPath); // consider handling this async call properly
		using var textReader = new StringReader(content);
		var configIn = new CsvConfiguration(CultureInfo.InvariantCulture);
		using var csvReader = new CsvReader(textReader, configIn);
		var records = csvReader.GetRecords<dynamic>().ToList();

		// For each record, we filter out the fields we are interested in, and add the resulting dictionary to toReturn.
		for (var i = 0; i < records.Count; i += 1)
		{
			var recordDict = (IDictionary<string, object>)records[i];
			var selectedFields = recordDict.Where(pair => fieldsToInclude.Contains(pair.Key)).ToDictionary(pair => pair.Key, pair => pair.Value);
			toReturn.Add(selectedFields);
		}

		return toReturn;
	}


	public async Task<Dictionary<string,Dictionary<string, object>>> GetContentDictionary(List<string> fieldsToInclude,
		string delimiterIn, string primaryKeyColumn, string csvFilterField, IList<string> csvFilterValues,
		 bool startsWithValues, bool useDebugPath)
	{
		var toReturn = new Dictionary<string, Dictionary<string, object>>();
		var content = await GetContent(useDebugPath,delimiterIn,primaryKeyColumn, csvFilterField, csvFilterValues, startsWithValues ); // consider handling this async call properly
		using var textReader = new StringReader(content);
		var configIn = new CsvConfiguration(CultureInfo.InvariantCulture);
		using var csvReader = new CsvReader(textReader, configIn);
		var records = csvReader.GetRecords<dynamic>().ToList();

		// For each record, we filter out the fields we are interested in, and add the resulting dictionary to toReturn.
		for (var i = 0; i < records.Count; i += 1)
		{
			var recordDict = (IDictionary<string, object>)records[i];
			var selectedFields = recordDict.Where(pair => fieldsToInclude.Contains(pair.Key)).ToDictionary(pair => pair.Key, pair => pair.Value);
			toReturn.Add(selectedFields[primaryKeyColumn].ToString() ?? throw new InvalidOperationException("missing primary key"), selectedFields);
		}

		return toReturn;
	}


	public async Task<string> MergeJsonResponsesIntoCsv(List<string> responses, string primaryKeyColumn,
		List<string> fieldsToUpdate, string delimiterOut, bool addNewRows)
	{
		var existingContent = await GetContent(false);  // Assuming we load the existing data
		var resultTable = LoadCsvIntoDataTable(existingContent, delimiterOut, primaryKeyColumn);

		foreach (var response in responses)
		{
			if (!string.IsNullOrEmpty(response))
			{
				try
				{
					var records = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response);

					foreach (var record in records)
					{
						var row = resultTable.Rows.Find(record[primaryKeyColumn]);

						if (addNewRows && row == null)
						{
							row = resultTable.NewRow();
							resultTable.Rows.Add(row);
						}

						if (row != null)
						{
							foreach (var pair in record)
							{
								if (pair.Key != primaryKeyColumn)
								{
									if (fieldsToUpdate.Contains(pair.Key))
									{
										resultTable.Columns[pair.Key].ReadOnly = false;
										row[pair.Key] = pair.Value;
									}

								}
							}
						}
					}
				}
				catch (Exception e)
				{
					Logger.LogException(e);
				}
			}
		}

		// Serialize final table into a CSV
		using var stringWriter = new StringWriter();
		var configOut = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			Delimiter = delimiterOut
		};
		using var csvWriter = new CsvWriter(stringWriter, configOut);

		csvWriter.ExportDataTable(resultTable);

		return stringWriter.ToString();
	}





	private DataTable LoadCsvIntoDataTable(string content, string delimiter, string primaryKeyColumn)
	{
		using var textReader = new StringReader(content);
		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			Delimiter = delimiter
		};
		using var csvReader = new CsvReader(textReader, config);
		using var csvDataReader = new CsvDataReader(csvReader);

		var dataTable = new DataTable();
		dataTable.Load(csvDataReader);
		if (!string.IsNullOrEmpty(primaryKeyColumn))
		{
			var groupedbyPrimaryKey = dataTable.AsEnumerable().GroupBy(row => row[primaryKeyColumn]);
			var duplicates = groupedbyPrimaryKey.Where(group => group.Count() > 1).ToList();
			Console.WriteLine($"Found {duplicates.Count} duplicates for primary key {primaryKeyColumn}");
			Console.WriteLine($"duplicate keys: {string.Join(",", duplicates.Select(group => group.Key))}");
			dataTable.PrimaryKey = new[] { dataTable.Columns[primaryKeyColumn] };
		}
		return dataTable;
	}


}