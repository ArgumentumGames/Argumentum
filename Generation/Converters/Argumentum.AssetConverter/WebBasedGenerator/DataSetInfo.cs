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
using System.Text.Json.Serialization;
using Argumentum.AssetConverter.Json;
using NewtonsoftJson = Newtonsoft.Json;

namespace Argumentum.AssetConverter;



public class DataSetInfo
{
	public string Name { get; set; }

	public string ReleaseFilePath { get; set; }
	public string DebugFilePath { get; set; }
	public bool IsDirectory { get; set; }

	[JsonConverter(typeof(TypeConverter))]
	public Type CsvType { get; set; }

	public string FilePath(bool debug) =>
		debug && !string.IsNullOrEmpty(DebugFilePath) ? DebugFilePath : ReleaseFilePath;

	private string _StringContent;

	public async Task<string> GetContent(bool useDebugPath, string filePattern = "*.csv")
	{
		if (!string.IsNullOrEmpty(_StringContent))
		{
			Logger.Log($"Using cached content for {Name}");
			return _StringContent;
		}

		var path = FilePath(useDebugPath);
		var contentBuilder = new StringBuilder();

		if (IsDirectory)
		{
			if (Directory.Exists(path))
			{
				var files = Directory.GetFiles(path, filePattern);
				Logger.Log($"Found {files.Length} files in directory {path} with pattern {filePattern}");
				bool firstFile = true;
				foreach (var file in files)
				{
					var fileContent = await file.GetDocumentContent();
					if (!firstFile)
					{
						// Remove header from subsequent files
						var headerEndIndex = fileContent.IndexOf(Environment.NewLine);
						if (headerEndIndex != -1)
						{
							fileContent = fileContent.Substring(headerEndIndex + Environment.NewLine.Length);
						}
					}
					contentBuilder.Append(fileContent);
					firstFile = false;
				}
			}
			else
			{
				Logger.Log($"Directory not found: {path}");
			}
		}
		else
		{
			contentBuilder.Append(await path.GetDocumentContent());
		}

		_StringContent = contentBuilder.ToString();
		return _StringContent;
	}


	public async Task SaveContent(string strPath, string content)
	{

		if (!string.IsNullOrEmpty(content))
		{
			await File.WriteAllTextAsync(strPath, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			Logger.LogSuccess($"Successfully saves {strPath}");
		}
		else
		{
			Logger.Log($"Cannot save empty content to {strPath}");
		}

	}


	public async Task<string> GetContent(bool useDebugPath, string delimiterIn, string primaryKeyColumn, string csvFilterField, IList<string> csvFilterValues, string filePattern = "*.csv")
	{
		var content = await GetContent(useDebugPath, filePattern);

		if (!string.IsNullOrEmpty(csvFilterField) && csvFilterValues != null && csvFilterValues.Any())
		{
			var filteredTable = GetDataTableFromCsv(delimiterIn, primaryKeyColumn, csvFilterField, csvFilterValues, content);

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




	public static string MergeResponsesIntoCsv(List<string> responses, string delimiterIn, string delimiterOut, string primaryKeyColumn)
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


	public static List<string> SplitContentIntoJsonChunks(List<Dictionary<string, object>> records, int chunkSize)
	{

		
		var chunks = new List<string>();
		for (var i = 0; i < records.Count; i += chunkSize)
		{
			var chunkRecords = records.Skip(i).Take(chunkSize);
			string jsonChunk = NewtonsoftJson.JsonConvert.SerializeObject(chunkRecords, NewtonsoftJson.Formatting.Indented);
			chunks.Add(jsonChunk);
		}

		return chunks;
	}


	public static List<string> SplitContentIntoJsonChunks(List<List<Dictionary<string, object>>> hierarchicalRecords)
	{

		var chunks = new List<string>();

		foreach (var hierarchicalRecordList in hierarchicalRecords)
		{
			var jsonChunk = NewtonsoftJson.JsonConvert.SerializeObject(hierarchicalRecordList, NewtonsoftJson.Formatting.Indented);
			chunks.Add(jsonChunk);
		}

		return chunks;

	}

	public static List<List<Dictionary<string, object>>> GetHierarchicalRecords(List<Dictionary<string, object>> records, string pkField, char pKHierarchicalChar, int pKHierarchyLevel, bool includeChildren, int maxChildren)
	{

		var rootRecords = GetRootRecords(records, pkField, pKHierarchicalChar, pKHierarchyLevel);
		var rootWithParentsSiblingsAndChildrenRecords = rootRecords.Select(rootRecord => GetRecordHierarchies(records, pkField, pKHierarchicalChar, pKHierarchyLevel, rootRecord, includeChildren, maxChildren)).ToList();

		var groups = rootWithParentsSiblingsAndChildrenRecords.SelectMany(list => list).ToList();

		// Completeness guarantee (issue #409 Bug 1): the standard root selection picks records
		// with exactly (level-1) separators, and GetRecordHierarchies gathers descendants via
		// StartsWith on those roots. Records with FEWER than (level-1) separators that are NOT
		// reached as ancestors of a selected root (e.g. a dot-less top-level PK "0" with no
		// children, or a shallow parent the parent-walk fails to include) are silently orphaned.
		// Sweep for any record absent from every group and emit it as its own singleton group so
		// it still reaches downstream processing. Previously this shortfall was silent.
		var coveredPks = new HashSet<string>(
			groups.SelectMany(group => group)
				.Select(record => (record[pkField]?.ToString() ?? string.Empty)));

		var orphanedRecords = records
			.Where(record => !coveredPks.Contains(record[pkField]?.ToString() ?? string.Empty))
			.ToList();

		if (orphanedRecords.Count > 0)
		{
			Logger.Log(
				$"GetHierarchicalRecords: {orphanedRecords.Count} record(s) not covered by any root group at level {pKHierarchyLevel} " +
				$"(PKs: {string.Join(", ", orphanedRecords.Take(20).Select(record => record[pkField]?.ToString()))}" +
				$"{(orphanedRecords.Count > 20 ? ", ..." : string.Empty)}). " +
				"Adding them as shallow-root singleton groups so they are not silently dropped.");

			foreach (var orphanedRecord in orphanedRecords)
			{
				groups.Add(new List<Dictionary<string, object>> { orphanedRecord });
			}
		}

		var coveredCount = groups.SelectMany(group => group)
			.Select(record => record[pkField]?.ToString() ?? string.Empty)
			.Distinct()
			.Count();

		if (coveredCount != records.Count)
		{
			Logger.Log(
				$"GetHierarchicalRecords: coverage assertion shortfall — {coveredCount} distinct PK(s) covered out of {records.Count} input record(s) at level {pKHierarchyLevel}.");
		}

		return groups;
	}

	public static List<Dictionary<string, object>> GetRootRecords(List<Dictionary<string, object>> records, string pkField, char pKHierarchicalChar, int pKHierarchyLevel)
	{
		var rootSeparatorCount = pKHierarchyLevel - 1;

		// Standard roots: PKs with exactly (level-1) separators.
		var standardRoots = records
			.Where(record => (record[pkField]?.ToString() ?? string.Empty).Count(c => c == pKHierarchicalChar) == rootSeparatorCount)
			.ToList();

		// Shallow roots (issue #409 Bug 1): PKs with FEWER than (level-1) separators that have no
		// descendant in the dataset (terminal shallow nodes, e.g. a dot-less top-level PK "0" with
		// no children). Without this they match neither the standard root predicate nor any root's
		// StartsWith children query, so they are silently dropped at level >= 2. Shallow nodes that
		// DO have descendants are intentionally excluded here: their subtrees are already covered by
		// the deeper standard roots, and the post-grouping sweep in GetHierarchicalRecords re-adds
		// any shallow parent the hierarchy walk still misses.
		var shallowRoots = records
			.Where(record =>
			{
				var pk = record[pkField]?.ToString() ?? string.Empty;
				if (pk.Count(c => c == pKHierarchicalChar) >= rootSeparatorCount)
				{
					return false;
				}

				var childPrefix = pk + pKHierarchicalChar;
				var hasDescendant = records.Any(other =>
					!ReferenceEquals(other, record)
					&& (other[pkField]?.ToString() ?? string.Empty).StartsWith(childPrefix, StringComparison.Ordinal));

				return !hasDescendant;
			})
			.ToList();

		return standardRoots.Concat(shallowRoots).ToList();
	}

	public static List<List<Dictionary<string, object>>> GetRecordHierarchies(List<Dictionary<string, object>> records, string pkField, char pKHierarchicalChar, int pKHierarchyLevel,
		Dictionary<string, object> rootRecord, bool includeChildren, int maxChildren)
	{

		var rootPk = rootRecord[pkField].ToString();

		List<List<Dictionary<string, object>>> subHierarchies = new();
		if (includeChildren)
		{
			var allChildren = records.Where(record => record[pkField].ToString() != rootPk
							&& record[pkField].ToString().StartsWith(rootPk)).ToList();

			if (allChildren.Count> maxChildren)
			{
				//var nbGroups = (int)Math.Ceiling((double)allChildren.Count / maxChildren);
				//for (var i = 0; i < nbGroups; i++)
				//{
				//	var children = allChildren.Skip(i * maxChildren).Take(maxChildren).ToList();
				//	subHierarchies.Add(children);
				//}
				var nbHierarchicalChars = rootPk.Count(c => c == pKHierarchicalChar);
				var children = allChildren.Where(record => record[pkField].ToString().Count(c => c == pKHierarchicalChar) == nbHierarchicalChars + 1).ToList();
				var childrenHierarchies = children.Select(child => GetRecordHierarchies(records, pkField, pKHierarchicalChar, pKHierarchyLevel+1, child, includeChildren, maxChildren)).ToList();
				return childrenHierarchies.SelectMany(list => list).ToList();
			}
			else
			{
				subHierarchies.Add(allChildren);
			}

		}
		else
		{
			subHierarchies.Add(new());
		}

		var returnHierarchies = new List<List<Dictionary<string, object>>>();

		foreach (var subHierarchy in subHierarchies)
		{
			var currentRecordHierarchy = subHierarchy;

			var currentRecord = rootRecord;
			var currentRecordPk = rootPk;

			for (var parentLevel = 1; parentLevel < pKHierarchyLevel; parentLevel++)
			{
				var siblingsRecords = records.Where(record =>
						record[pkField].ToString().Length == currentRecordPk.Length
						&& (currentRecordPk.Length == 1
						    || record[pkField].ToString()
							    .StartsWith(currentRecordPk.Substring(0, currentRecordPk.LastIndexOf(pKHierarchicalChar) + 1))))
					.ToList();
				var newHierarchy = new List<Dictionary<string, object>>();
				foreach (var siblingRecord in siblingsRecords)
				{
					var siblingRecordPk = siblingRecord[pkField].ToString();
					newHierarchy.Add(siblingRecord);
					if (siblingRecordPk == currentRecordPk)
					{
						newHierarchy.AddRange(currentRecordHierarchy);
					}
				}

				if (parentLevel < pKHierarchyLevel)
				{
					var lastSeparatorIndex = currentRecordPk.LastIndexOf(pKHierarchicalChar);
					if (lastSeparatorIndex < 0)
					{
						// Shallow root (issue #409 Bug 1): a PK with no separator has no parent to
						// walk up to. Without this guard LastIndexOf returns -1 and Substring(0, -1)
						// throws. Stop the parent walk here — the current hierarchy is complete.
						currentRecordHierarchy = newHierarchy;
						break;
					}
					currentRecordPk = currentRecordPk.Substring(0, lastSeparatorIndex);
				}
				currentRecordHierarchy = newHierarchy;
			}

			returnHierarchies.Add(currentRecordHierarchy);
		}
		return returnHierarchies;
	}

		


	public List<Dictionary<string, object>> GetDictionaryFromCsv(string content, List<string> fieldsToInclude, bool useDebugPath)
	{
		
		using var textReader = new StringReader(content);
		var configIn = new CsvConfiguration(CultureInfo.InvariantCulture);
		using var csvReader = new CsvReader(textReader, configIn);
		var records = csvReader.GetRecords<dynamic>().ToList();
		var recordsDictionary = records.Select(record =>
		{
			var recordDict = (IDictionary<string, object>)record;
			return recordDict.Where(pair => fieldsToInclude.Contains(pair.Key))
				.ToDictionary(pair => pair.Key, pair => pair.Value);
		}).ToList();
		return recordsDictionary;
	}

	public DataTable GetDataTableFromCsv(string delimiterIn, string primaryKeyColumn, string csvFilterField,
		IList<string> csvFilterValues, string content)
	{
		var dataTable = LoadCsvIntoDataTable(content, delimiterIn, primaryKeyColumn);

		var filteredRows = dataTable.AsEnumerable()
			.Where(row => csvFilterValues.Contains(row[csvFilterField]?.ToString()));

		var filteredTable = dataTable.Clone();
		foreach (var row in filteredRows)
		{
			filteredTable.ImportRow(row);
		}

		return filteredTable;
	}


	public static DataTable LoadCsvIntoDataTable(string content, string delimiter, string primaryKeyColumn)
	{
		using var textReader = new StringReader(content);
		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			Delimiter = delimiter,
			MissingFieldFound = null,
			BadDataFound = null
		};
		using var csvReader = new CsvReader(textReader, config);
		using var csvDataReader = new CsvDataReader(csvReader);

		var dataTable = new DataTable();
		dataTable.Load(csvDataReader);
		if (!string.IsNullOrEmpty(primaryKeyColumn))
		{
			dataTable.PrimaryKey = new[] { dataTable.Columns[primaryKeyColumn] };
		}
		return dataTable;
	}


	//public async Task<string> MergeJsonResponsesIntoCsv(List<string> responses, string primaryKeyColumn,
	//	List<string> fieldsToUpdate, string delimiterOut, bool addNewRows)
	//{
	//	var existingContent = await GetContent(false);  // Assuming we load the existing data
	//	var resultTable = LoadCsvIntoDataTable(existingContent, delimiterOut, primaryKeyColumn);

	//	foreach (var response in responses)
	//	{
	//		var currentResponse = response;
	//		if (!string.IsNullOrEmpty(currentResponse))
	//		{
	//			try
	//			{
	//				var records = GetDictionaryRecordsFromJson(currentResponse);

	//				UpdateTableFromRecords(primaryKeyColumn, fieldsToUpdate, addNewRows, records, resultTable);
	//			}
	//			catch (Exception e)
	//			{
	//				Logger.LogException(e);
	//			}
	//		}
	//	}

	//	return WriteDataTableToCsv(delimiterOut, resultTable);
	//}

	public static string WriteDataTableToCsv(DataTable resultTable, string delimiterOut)
	{
		// Serialize final table into a CSV
		using var stringWriter = new StringWriter();
		var configOut = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			Delimiter = delimiterOut,
			Encoding = Encoding.UTF8
		};
		using var csvWriter = new CsvWriter(stringWriter, configOut);

		csvWriter.ExportDataTable(resultTable);

		return stringWriter.ToString();
	}

	public static void UpdateTableFromRecords(string primaryKeyColumn, List<string> fieldsToUpdate, bool addNewRows,
		List<Dictionary<string, object>> records,
		bool writeOneTargetFileByField,
		Dictionary<string, DataTable> resultTables)
	{

		var globalResultTable = resultTables[""];


		foreach (var record in records)
		{
			var row = globalResultTable.Rows.Find(record[primaryKeyColumn]);

			if (addNewRows && row == null)
			{
				row = globalResultTable.NewRow();
				globalResultTable.Rows.Add(row);
			}

			if (row != null)
			{
				if (!writeOneTargetFileByField)
				{
					UpdateTableRow(primaryKeyColumn, fieldsToUpdate, record, globalResultTable, row);
				}
				else
				{
					foreach (var pair in record)
					{
						var columnName = pair.Key;
						if (columnName != primaryKeyColumn)
						{
							if (fieldsToUpdate.Contains(columnName))
							{
								if (!resultTables.TryGetValue(columnName, out var fieldDataTable))
								{
									fieldDataTable = globalResultTable.Copy();

									resultTables.Add(columnName, fieldDataTable);
								}

								row = fieldDataTable.Rows.Find(record[primaryKeyColumn]);
								fieldDataTable.Columns[columnName].ReadOnly = false;
								row[columnName] = pair.Value;
							}
						}
					}
				}


				
			}
		}
	}

	public static void UpdateTableRow(string primaryKeyColumn, List<string> fieldsToUpdate, Dictionary<string, object> record,
		DataTable resultTable, DataRow row)
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

	public static List<Dictionary<string, object>> GetDictionaryRecordsFromJson(string currentResponse)
	{
		if (!currentResponse.StartsWith("["))
		{
			currentResponse = currentResponse.Substring(currentResponse.IndexOf("[", StringComparison.InvariantCulture));
			currentResponse =
				currentResponse.Substring(0, currentResponse.LastIndexOf("]", StringComparison.InvariantCulture) + 1);
		}
	
		var records = NewtonsoftJson.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(currentResponse);
		return records;
	}

	
}