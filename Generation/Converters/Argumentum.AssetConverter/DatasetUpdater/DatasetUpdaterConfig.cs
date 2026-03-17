using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Argumentum.AssetConverter.DatasetUpdater;
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

	public string SystemPrompt { get; set; } = Resource1.VirtuesJsonPromptSystem;

	public bool AddPromptSample { get; set; }

	public string UserPrompt { get; set; } = Resource1.VirtuesJsonPromptSampleUser;


	public string AssistantPrompt { get; set; } = Resource1.VirtuesJsonPromptSampleAssistant;

	public string OpenAIKeyPath { get; set; } = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\OpenAI-Key.txt";

	public string Model { get; set; } = Models.Gpt_4_1106_preview;

	public int MaxTokensPerMinute { get; set; } = 70000;

	public DivisionMode DivisionMode { get; set; }

	
	public int ChunkSize { get; set; } = 3;

	public Char PKHierarchicalChar { get; set; } = '.';

	public int PKHierarchyLevel { get; set; } = 3;

	public bool UseFunctionCalling { get; set; } = false;

	public int NbMessageCalls { get; set; } = 1;

	public int SkipChunkNb { get; set; } = 30;

	public int TakeChunkNb { get; set; } = 0;

	public int MaxDegreeOfParallelismWebService { get; set; } = 2;

	public List<string> FieldsToInclude { get; set; } = new List<string>(new[]
	{
		//Virtues
		"path",
		"family_fr",
		"subfamily_fr",
		"subsubfamily_fr",
		"title_fr",
		"description_fr",
		"remark_fr",
		"link_fr"

		//Fallacies
		//"path",
		//"Famille",
		//"Sous-Famille",
		//"Soussousfamille",
		//"text_fr",
		//"desc_fr",
		//"example_fr",
		//"link_fr"

	});

	public string PrimaryField { get; set; } = "path";

	public List<string> FieldsToUpdate { get; set; } = new List<string>(new[]
	{
		//Virtues
		"title_fr",
		"description_fr",
		"remark_fr",
		"link_fr"

		//Fallacies
		//"path",
		//"text_fr",
		//"desc_fr",
		//"example_fr",
		//"link_fr"
	});

	public DataSetInfo SourceDataset { get; set; } = new DataSetInfo()
	{
		Name = "Argumentum - Virtues - Taxonomy",
		ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Virtues%20-%20Taxonomy.csv",
		DebugFilePath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Virtues - Taxonomy.csv",
	};

	public string TargetPath { get; set; } = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv";


	public async Task Apply(bool useDebugPath)
	{
		var openAIKey = await File.ReadAllTextAsync(OpenAIKeyPath);
		var cts = new CancellationTokenSource();
		var token = cts.Token;

		_ = Task.Run(async () =>
		{
			while (!token.IsCancellationRequested)
			{
				if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
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

			if (TakeChunkNb > 0)
			{
				recordGroups = recordGroups.Take(TakeChunkNb).ToList();
			}

			var answers = new ConcurrentBag<string>();

			var tokenManager = new TokenManager(MaxTokensPerMinute, Model);

			var parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = MaxDegreeOfParallelismWebService,
				CancellationToken = token
			};

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
							SystemPrompt = SystemPrompt,
							
							UserPrompt = chunk,
							Tokenizer = tokenManager.TokenizerAction
						};

						if (this.AddPromptSample)
						{
							dataPrompt.Example = new PromptExample()
							{
								UserPrompt = UserPrompt,
								Answer = AssistantPrompt
							};
						}

						if (UseFunctionCalling)
						{
							var recordsUpdator = new RecordsUpdater()
							{
								PrimaryKeyField = PrimaryField,
								Records = recordGroup
							};
							dataPrompt.Functions = FunctionCallingHelper.GetFunctionDefinitions<RecordsUpdater>().Select(definition => (definition, (object) recordsUpdator)).ToList();
							
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
		catch (Exception ex)
		{
			Logger.LogException(ex);
		}

	}


}