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

	public int MaxDegreeOfParallelismWebService { get; set; } = 2;

	public List<string> FieldsToInclude { get; set; } = new();


	public string PrimaryField { get; set; } = "";

	public List<string> FieldsToUpdate { get; set; } = new();

	public DataSetInfo SourceDataset { get; set; } = new();

	public string TargetPath { get; set; } = "";


	public bool CompareMode { get; set; }


	public string CompareField { get; set; }
	public bool AutoCompare { get; set; }
	public string AutoCompareField { get; set; } = "";

	public async Task Apply(bool useDebugPath)
	{
		var openAIKey = await File.ReadAllTextAsync(OpenAIKeyPath);
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


		try
		{

			var content = await SourceDataset.GetContent(useDebugPath);

			var resultTable = DataSetInfo.LoadCsvIntoDataTable(content, ",", PrimaryField);




			if (!CompareMode)
			{

				// load dataset in chunks, 

				var records = SourceDataset.GetDictionaryFromCsv(content, FieldsToInclude, useDebugPath);


				List<List<Dictionary<string, object>>> recordGroups;

				switch (this.DivisionMode)
				{
					case DivisionMode.PKHierarchicalChar:

						recordGroups = SourceDataset.GetHierarchicalRecords(records, PrimaryField, PKHierarchicalChar, PKHierarchyLevel);
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




				//Doing short tests
				if (SkipChunkNb > 0)
				{
					recordGroups = recordGroups.Skip(SkipChunkNb).ToList();
				}

				if (TakeChunkNb >= 0)
				{
					recordGroups = recordGroups.Take(TakeChunkNb).ToList();
				}

				if (RandomizeChunks)
				{
					recordGroups = recordGroups.OrderBy(x => Guid.NewGuid()).ToList();
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
					try
					{
						ct.ThrowIfCancellationRequested();

						var chunk = SourceDataset.SplitContentIntoJsonChunks(recordGroup, int.MaxValue)[0];



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
							dataPrompt.Functions = FunctionCallingHelper.GetFunctionDefinitions<RecordsUpdater>().Select(definition => (definition, (object)recordsUpdator)).ToList();
							if (!string.IsNullOrEmpty(FunctionName))
							{
								dataPrompt.FunctionName = FunctionName;
							}
						}

						string result = "";

						for (int i = 0; i < NbMessageCalls; i++)
						{
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
						}
						List<Dictionary<string, object>> records;
						if (UseFunctionCalling)
						{
							records = recordGroup;
						}
						else
						{
							records = DataSetInfo.GetDictionaryRecordsFromJson(result);
						}



						lock (resultTable)
						{
							DataSetInfo.UpdateTableFromRecords(PrimaryField, FieldsToUpdate, false, records, resultTable);
						}


						answers.Add(result);
					}
					catch (OperationCanceledException)
					{
						Logger.Log("Operation cancelled by user.");
					}
				});



				//// Merge answers into one csv
				//var mergedCsv =
				//	await SourceDataset.MergeJsonResponsesIntoCsv(answers.ToList(), PrimaryField, FieldsToUpdate, ",",
				//		false);

				var mergedCsv = DataSetInfo.WriteDataTableToCsv(resultTable, ",");


				// Save mergedCsv to file

				if (File.Exists(TargetPath))
				{
					try
					{
						File.Delete(TargetPath);
					}
					catch (Exception e)
					{
						Logger.LogException(e);
						Console.ReadKey();
					}

				}

				if (!Directory.Exists(Path.GetDirectoryName(TargetPath)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(TargetPath));
				}

				await SourceDataset.SaveContent(TargetPath, mergedCsv);


				Logger.Log("Completed ChatGPT calls and saved the output to " + TargetPath);
			}
			else
			{
				if (File.Exists(TargetPath))
				{
					

					DataTable targetTable;

					if (AutoCompare)
					{
						targetTable = resultTable.Copy();
					}
					else
					{
						var targetContent = await TargetPath.GetDocumentContent();
						targetTable = DataSetInfo.LoadCsvIntoDataTable(targetContent, ",", PrimaryField);
					}
					

					for (int i = 0; i < resultTable.Rows.Count; i++)
					{
						var originalRow = resultTable.Rows[i];
						List<DataRow>  targetRows;
						if (AutoCompare)
						{
							targetRows = targetTable.Select().Where(row => row[AutoCompareField].ToString() == originalRow[AutoCompareField].ToString()).ToList();
						}
						else
						{
							var targetRow = targetTable.Rows.Find(originalRow[PrimaryField]);
							targetRows = new List<DataRow>() { targetRow };
						}

						foreach (var targetRow in targetRows)
						{
							if (targetRow != null)
							{
								var originalValue = originalRow[CompareField];
								var targetValue = targetRow[CompareField];
								if (originalValue.ToString() != targetValue.ToString())
								{
									var originalAutoCompareValue = !string.IsNullOrEmpty(AutoCompareField) ? originalRow[AutoCompareField] : "";
									Logger.Log($"Difference found for {originalAutoCompareValue}: {originalRow[PrimaryField]}:\n{originalValue}\nvs {targetRow[PrimaryField]}:\n{targetValue}");
								}
							}
						}
					}

				}
			}




		}
		catch (Exception ex)
		{
			Logger.LogException(ex);
		}

	}


}