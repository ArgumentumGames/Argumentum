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

namespace Argumentum.AssetConverter;

public class DatasetUpdaterConfig
{
	public string SystemPrompt { get; set; } = Resource1.VirtuesJsonPromptSystem;

	public string UserPrompt { get; set; } = Resource1.VirtuesJsonPromptSampleUser;


	public string AssistantPrompt { get; set; } = Resource1.VirtuesJsonPromptSampleAssistant;

	public string OpenAIKeyPath { get; set; } = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\OpenAI-Key.txt";

	public string Model { get; set; } = Models.Gpt_4_1106_preview;

	public int MaxTokensPerMinute { get; set; } = 70000;

	public int ChunkSize { get; set; } = 3;

	public int NbMessageCalls { get; set; } = 1;

	public int SkipChunkNb { get; set; } = 30;

	public int TakeChunkNb { get; set; } = 30;

	public int MaxDegreeOfParallelismWebService { get; set; } = 1;

	public List<string> FieldsToInclude { get; set; } = new List<string>(new[]
	{
		"path",
		"family_fr",
		"subfamily_fr",
		"subsubfamily_fr",
		"title_fr",
		"description_fr",
		"remark_fr",
		"link_fr"
	});

	public string PrimaryField { get; set; } = "path";

	public List<string> FieldsToUpdate { get; set; } = new List<string>(new[]
	{
		"title_fr",
		"description_fr",
		"remark_fr",
		"link_fr"
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

		Task.Run(async () =>
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
		});

		try
		{

			// load dataset in chunks, 
			var chunks = await SourceDataset.SplitContentIntoJsonChunks(ChunkSize, FieldsToInclude, useDebugPath);

			//Doing short tests
			if (SkipChunkNb>0)
			{
				chunks=chunks.Skip(SkipChunkNb).ToList();
			}

			if (TakeChunkNb>0)
			{
				chunks = chunks.Take(TakeChunkNb).ToList();
			}

			var answers = new ConcurrentBag<string>();

			var tokenManager = new TokenManager(MaxTokensPerMinute, Model);

			var parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = MaxDegreeOfParallelismWebService,
				CancellationToken = token
			};
			await Parallel.ForEachAsync(chunks, parallelOptions, async (chunk, ct) =>
			{
				ct.ThrowIfCancellationRequested();
				var dataPrompt = new Prompt()
				{
					ApiKey = openAIKey,
					Model = Model,
					SystemPrompt = SystemPrompt,
					Example = new PromptExample()
					{
						UserPrompt = UserPrompt,
						Answer = AssistantPrompt
					},
					UserPrompt = chunk,
					Tokenizer = tokenManager.TokenizerAction
				};

				string result = "";

				for (int i = 0; i < NbMessageCalls; i++)
				{
					ct.ThrowIfCancellationRequested();
					Logger.Log($"Calling ChatGPT API with chunk: \n{Markup.Escape(chunk)}\n");
					
					try
					{
						tokenManager.WaitForTokenAvailability();
						result = await dataPrompt.Send();
						//result = chunk;
						Logger.Log($"ChatGPT answered chunk: \n{Markup.Escape(chunk)}\n with chunk \n{Markup.Escape(result)}\n");
						dataPrompt.UserPrompt = result;
					}
					catch (Exception e)
					{
						Logger.LogException(e);
					}
				}
				


				answers.Add(result);
			});



			// Merge answers into one csv
			var mergedCsv = await SourceDataset.MergeJsonResponsesIntoCsv(answers.ToList(), PrimaryField, FieldsToUpdate, ",", false);


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
		catch (OperationCanceledException)
		{
			Logger.Log("Operation cancelled by user.");
		}
		catch (Exception ex)
		{
			Logger.LogException(ex);
		}

	}



}