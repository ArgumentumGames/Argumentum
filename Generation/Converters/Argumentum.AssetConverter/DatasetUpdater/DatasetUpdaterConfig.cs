using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OpenAI.ObjectModels;
using System.Threading;
using OpenAI.Utilities.FunctionCalling;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;

namespace Argumentum.AssetConverter;

public class DatasetUpdaterConfig
{

	public bool Enabled { get; set; } = true;

	public string SystemPromptPath { get; set; }


	public List<PromptExample> DialogPrompts { get; set; } = new();


	public string OpenAIKeyPath { get; set; } = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\OpenAI-Key.txt";

	public string Model { get; set; } = Models.Gpt_4_1106_preview;

	public int MaxTokensPerMinute { get; set; } = 70000;

	public DivisionMode DivisionMode { get; set; }


	public int ChunkSize { get; set; } = 3;

	public Char PKHierarchicalChar { get; set; } = '.';

	public int PKHierarchyLevel { get; set; } = 3;

	public bool UseFunctionCalling { get; set; } = false;

	public string FunctionName { get; set; }

	public int NbMessageCalls { get; set; } = 1;

	public int SkipChunkNb { get; set; } = 0;

	public int TakeChunkNb { get; set; } = -1;

	public bool RandomizeChunks { get; set; }

	public bool SelectEmptyTargets { get; set; }

	public int MaxDegreeOfParallelismWebService { get; set; } = 2;

	public List<string> FieldsToInclude { get; set; } = new();


	public string PrimaryField { get; set; } = "";

	public List<string> FieldsToUpdate { get; set; } = new();

	public string SourceDataset { get; set; }

	public string TargetPath { get; set; } = "";

	public bool WriteOneTargetFileByField { get; set; }

	public bool CompareMode { get; set; }


	public string CompareField { get; set; }
	public bool AutoCompare { get; set; }
	public string AutoCompareField { get; set; } = "";
	public int MaxGroupItemNb { get; set; } = 30;


	public int MaxChildren { get; set; } = 12;
	public int NbGlobalPasses { get; set; } = 1;

	public async Task Apply(AssetConverterConfig config)
	{
		var openAIKey = await File.ReadAllTextAsync(OpenAIKeyPath);



		try
		{

			var cts = new CancellationTokenSource();
			var token = cts.Token;

			_ = Task.Run(async () =>
			{
				while (!token.IsCancellationRequested)
				{
					if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Z)
					{
						cts.Cancel();
						Logger.Log("Cancel asked by user");
					}

					await Task.Delay(100, token);
				}
			}, token);

			var sourceDataset = config.DataSets.First(dataset => dataset.Name == SourceDataset);

			var content = await sourceDataset.GetContent(config.UseDebugParams);

			


			var saveResults = false;

			bool RecordHasEmptyTargetFields(Dictionary<string, object> dictionary)
			{
				foreach (var field in FieldsToUpdate)
				{
					if (string.IsNullOrEmpty(dictionary[field].ToString()))
					{
						return true;
					}
				}

				return false;
			}


			DataTable globalDataTable = DataSetInfo.LoadCsvIntoDataTable(content, ",", PrimaryField);
			var resultDataTablesPerField = new Dictionary<string, DataTable> { };
			resultDataTablesPerField[""] = globalDataTable;

			for (int globalPassIndex = 0; globalPassIndex < this.NbGlobalPasses; globalPassIndex++)
			{
				//globalDataTable = DataSetInfo.LoadCsvIntoDataTable(content, ",", PrimaryField);


				if (!CompareMode)
				{

					saveResults = true;

					// load dataset in chunks, 

					var records = sourceDataset.GetDictionaryFromCsv(content, FieldsToInclude, config.UseDebugParams);

					//if (SelectEmptyTargets)
					//{


					//	var toReturn = new List<Dictionary<string, object>>();


					//	var emptyRecords = records.Where(RecordHasEmptyTargetFields).ToList();

					//	toReturn.AddRange(emptyRecords);

					//	if (DivisionMode == DivisionMode.PKHierarchicalChar)
					//	{
					//		var rootRecords = DataSetInfo.GetHierarchicalRecords(records, PrimaryField, PKHierarchicalChar, PKHierarchyLevel, false, MaxChildren);
					//		foreach (var rootRecord in rootRecords.SelectMany(list => list))
					//		{
					//			if (!toReturn.Exists(record => record[PrimaryField] == rootRecord[PrimaryField]))
					//			{
					//				toReturn.Add(rootRecord);
					//			}
					//		}

					//		foreach (var emptyRecord in emptyRecords)
					//		{
					//			var pkEmptyRecord = emptyRecord[PrimaryField].ToString();
					//			var pkEmptyRecordRoot = pkEmptyRecord.Substring(0, Math.Max(0, pkEmptyRecord.LastIndexOf(PKHierarchicalChar)));
					//			var siblings = records.Where(record => record[PrimaryField].ToString().Length == pkEmptyRecord.Length
					//						&& record[PrimaryField].ToString().StartsWith(pkEmptyRecordRoot)).ToList();
					//			var nonEmptySiblings = siblings.Where(record => !RecordHasEmptyTargetFields(record)).ToList();
					//			foreach (var nonEmptySibling in nonEmptySiblings)
					//			{
					//				if (!toReturn.Exists(record => record[PrimaryField] == nonEmptySibling[PrimaryField]))
					//				{
					//					toReturn.Add(nonEmptySibling);
					//				}
					//			}
					//		}
					//	}

					//	records = toReturn;

					//}

					List<List<Dictionary<string, object>>> recordGroups;

					switch (this.DivisionMode)
					{
						case DivisionMode.PKHierarchicalChar:

							recordGroups = DataSetInfo.GetHierarchicalRecords(records, PrimaryField, PKHierarchicalChar, PKHierarchyLevel, true, MaxChildren);

							break;

						case DivisionMode.SequentialChunks:
							recordGroups = new List<List<Dictionary<string, object>>>();
							for (var i = 0; i < records.Count; i += ChunkSize)
							{
								recordGroups.Add(records.Skip(i).Take(ChunkSize).ToList());
							}
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					if (SelectEmptyTargets)
					{
						recordGroups = recordGroups.Where(group => group.Exists(RecordHasEmptyTargetFields)).ToList();
					}



					//Doing short tests
					if (SkipChunkNb > 0)
					{
						recordGroups = recordGroups.Skip(SkipChunkNb).ToList();
					}

					if (RandomizeChunks)
					{
						recordGroups = recordGroups.OrderBy(x => Guid.NewGuid()).ToList();
					}

					if (TakeChunkNb >= 0)
					{
						recordGroups = recordGroups.Take(TakeChunkNb).ToList();
					}

					if (recordGroups.Count > 0)
					{

						recordGroups = recordGroups.Where(group => group.Count > 0).ToList();

						if (MaxGroupItemNb > 0)
						{
							for (int i = 0; i < recordGroups.Count; i++)
							{
								recordGroups[i] = recordGroups[i].Take(Math.Min(recordGroups[i].Count, MaxGroupItemNb)).ToList();
							}
						}

						var answers = new ConcurrentBag<string>();

						var tokenManager = new TokenManager(MaxTokensPerMinute, Model);

						var parallelOptions = new ParallelOptions
						{
							MaxDegreeOfParallelism = MaxDegreeOfParallelismWebService,
							CancellationToken = token
						};

						var systemPrompt = await File.ReadAllTextAsync(SystemPromptPath, token);

						await Parallel.ForEachAsync(recordGroups, parallelOptions, async (recordGroup, ct) =>
						{
							await DoChatGPTCall(ct, recordGroup, openAIKey, systemPrompt, tokenManager, token, resultDataTablesPerField, answers);
						});



						//// Merge answers into one csv
						//var mergedCsv =
						//	await SourceDataset.MergeJsonResponsesIntoCsv(answers.ToList(), PrimaryField, FieldsToUpdate, ",",
						//		false);



					}


				}
				else
				{
					if (File.Exists(TargetPath))
					{


						DataTable targetTable;

						if (AutoCompare)
						{
							targetTable = globalDataTable.Copy();
						}
						else
						{
							var targetContent = await TargetPath.GetDocumentContent();
							targetTable = DataSetInfo.LoadCsvIntoDataTable(targetContent, ",", PrimaryField);
						}

						var differences = new List<List<DataRow>>();



						for (int i = 0; i < globalDataTable.Rows.Count; i++)
						{
							var originalRow = globalDataTable.Rows[i];

							var originalKey = originalRow[PrimaryField].ToString();



							List<DataRow> targetRows;
							if (AutoCompare)
							{
								targetRows = targetTable.Select().Where(row => row[AutoCompareField].ToString() == originalRow[AutoCompareField].ToString()).ToList();
							}
							else
							{
								var targetRow = targetTable.Rows.Find(originalKey);
								targetRows = new List<DataRow>() { targetRow };
							}


							List<DataRow> currentAlternative = null;

							var originalValue = originalRow[CompareField];

							foreach (var targetRow in targetRows)
							{
								if (targetRow != null)
								{

									var targetValue = targetRow[CompareField];
									if (originalValue.ToString() != targetValue.ToString())
									{
										if (currentAlternative == null)
										{
											currentAlternative = new();
											currentAlternative.Add(originalRow);
											differences.Add(currentAlternative);
										}
										currentAlternative.Add(targetRow);

									}
								}
							}

						}
						var checkedKeys = new HashSet<string>();
						globalDataTable.Columns[CompareField].ReadOnly = false;
						foreach (var difference in differences)
						{
							var originalAutoCompareValue = !string.IsNullOrEmpty(AutoCompareField) ? difference[0][AutoCompareField] : "";

							var originalKey = difference[0][PrimaryField].ToString();
							if (checkedKeys.Contains(originalKey))
							{
								continue;
							}
							checkedKeys.Add(originalKey);

							Logger.Log($"Found {difference.Count} options for {originalAutoCompareValue} - {difference[0][PrimaryField]}");


							var keys = new List<object>();
							for (int i = 0; i < difference.Count; i++)
							{
								var differenceKey = difference[i][PrimaryField];
								keys.Add(differenceKey);
								checkedKeys.Add(differenceKey.ToString());
								Logger.Log($"{i} - {differenceKey.ToString()}\n{difference[i][CompareField]}");
							}

							Logger.Log("Enter preferred Option:");
							var optionKey = Console.ReadLine();
							var intOptionKey = int.Parse(optionKey);
							var selectedValue = difference[intOptionKey][CompareField];
							foreach (var key in keys)
							{
								var row = globalDataTable.Rows.Find(key);
								row[CompareField] = selectedValue;
							}

						}


						Logger.Log("Save result (y/n)?");
						var savekey = Console.ReadKey();
						if (savekey.KeyChar == 'y')
						{
							saveResults = true;
						}

					}
				}



				content = DataSetInfo.WriteDataTableToCsv(globalDataTable, ",");

			}


			if (saveResults)
			{

				foreach (var dataTable in resultDataTablesPerField)
				{
					var mergedCsv = DataSetInfo.WriteDataTableToCsv(dataTable.Value, ",");

					var csvPath = TargetPath.Replace(".csv", $"{dataTable.Key}.csv");

					// Save mergedCsv to file

					if (File.Exists(csvPath))
					{
						try
						{
							File.Delete(csvPath);
						}
						catch (Exception e)
						{
							Logger.LogException(e);
							Console.ReadKey();
						}

					}

					if (!Directory.Exists(Path.GetDirectoryName(csvPath)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(csvPath));
					}

					await sourceDataset.SaveContent(csvPath, mergedCsv);


					Logger.LogSuccess("Completed ChatGPT calls and saved the output to " + csvPath);
				}


			}


		}
		catch (Exception ex)
		{
			Logger.LogException(ex);
		}

	}

	private async Task DoChatGPTCall(CancellationToken ct, List<Dictionary<string, object>> recordGroup, string openAIKey, string systemPrompt,
		TokenManager tokenManager, CancellationToken token, Dictionary<string, DataTable> resultDataTablesPerField, ConcurrentBag<string> answers)
	{
		try
		{

			string result = "";
			for (int i = 0; i < NbMessageCalls; i++)
			{
				ct.ThrowIfCancellationRequested();

				var chunk = DataSetInfo.SplitContentIntoJsonChunks(recordGroup, int.MaxValue)[0];

				var dataPrompt = new Prompt()
				{
					ApiKey = openAIKey,
					Model = Model,
					SystemPrompt = systemPrompt,
					DialogPrompts = DialogPrompts,
					UserPrompt = chunk,
					Tokenizer = tokenManager.TokenizerAction
				};

				if (UseFunctionCalling)
				{
					var recordsUpdator = new RecordsUpdater()
					{
						PrimaryKeyField = PrimaryField,
						Records = recordGroup
					};
					dataPrompt.Functions = FunctionCallingHelper.GetToolDefinitions<RecordsUpdater>().Select(definition => (definition.Function, (object)recordsUpdator)).ToList();
					if (!string.IsNullOrEmpty(FunctionName))
					{
						dataPrompt.FunctionName = FunctionName;
					}
				}




				ct.ThrowIfCancellationRequested();
				Logger.Log($"Calling ChatGPT API with chunk: \n{Markup.Escape(chunk)}\n");

				try
				{
					tokenManager.WaitForTokenAvailability();
					result = await dataPrompt.Send(token).ConfigureAwait(false);
					//result = chunk;
					Logger.Log(
						$"ChatGPT answered chunk: \n{Markup.Escape(chunk)}\n with chunk \n{Markup.Escape(result)}\n");
					dataPrompt.UserPrompt = result;
				}
				catch (Exception e)
				{
					Logger.LogException(e);
				}

				List<Dictionary<string, object>> newRecords;
				if (!UseFunctionCalling)
				{
					recordGroup = DataSetInfo.GetDictionaryRecordsFromJson(result);
				}


				lock (resultDataTablesPerField)
				{
					DataSetInfo.UpdateTableFromRecords(PrimaryField, FieldsToUpdate, false, recordGroup, this.WriteOneTargetFileByField, resultDataTablesPerField);
				}

			}


			answers.Add(result);
		}
		catch (OperationCanceledException)
		{
			Logger.Log("Operation cancelled by user.");
		}
	}
}