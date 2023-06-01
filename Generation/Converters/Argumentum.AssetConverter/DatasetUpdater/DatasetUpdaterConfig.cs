using OpenAI.GPT3.ObjectModels;
using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Argumentum.AssetConverter.DatasetUpdater;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.CoreSkills;

namespace Argumentum.AssetConverter;

public class DatasetUpdaterConfig
{

	public SemanticMode SemanticMode { get; set; } = SemanticMode.IndexWithSemanticKernel;

	public string SystemPrompt { get; set; } = Resource1.JsonPromptSystem;

	public string UserPrompt { get; set; } = Resource1.JsonPromptSampleUser;


	public string AssistantPrompt { get; set; } = Resource1.JsonPromptSampleAssistant;

	public string OpenAIKeyPath { get; set; } = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\OpenAI-Key.txt";

	public string Model { get; set; } = Models.ChatGpt3_5Turbo;

	public int ChunkSize { get; set; } = 3;

	public int NbMessageCalls { get; set; } = 3;

	public int SkipChunkNb { get; set; } = 0;

	public int TakeChunkNb { get; set; } = 0;

	public int MaxDegreeOfParallelismWebService { get; set; } = 8;

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
		//"title_fr",
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

		switch (SemanticMode)
		{
			case SemanticMode.UpdateWithChatpGptAPI:
				await UpdateWithChatGPT(useDebugPath, openAIKey);
				break;
			case SemanticMode.IndexWithSemanticKernel:
				await IndexTaxonomyWithSemanticKernel(useDebugPath, openAIKey);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		
	}

	


	private async Task IndexTaxonomyWithSemanticKernel(bool useDebugPath, string openAIKey)
	{

		var chunks = await SourceDataset.SplitContentIntoJsonChunks(ChunkSize, FieldsToInclude, useDebugPath);

		var kernel = Kernel.Builder
			.WithLogger(ConsoleLogger.Log)
			//.WithOpenAITextCompletionService("text-davinci-003", openAIKey)
			.WithOpenAITextCompletionService(Models.Model.TextDavinciV3.EnumToString(), openAIKey)
			//.WithOpenAITextEmbeddingGenerationService("text-embedding-ada-002", openAIKey)
			.WithOpenAITextEmbeddingGenerationService(Models.Model.TextEmbeddingAdaV2.EnumToString(), openAIKey)
			.WithMemoryStorage(new VolatileMemoryStore())
			
			.Build();

		AddSkills(kernel);

	}

	private void AddSkills(IKernel kernel)
	{
		kernel.ImportSkill(new MathSkill(), "math");
		kernel.ImportSkill(new HttpSkill(), "http");
		kernel.ImportSkill(new FileIOSkill(), "fileIO");
		kernel.ImportSkill(new TextSkill(), "text");
		kernel.ImportSkill(new TimeSkill(), "time");
		kernel.ImportSkill(new WaitSkill(), "wait");
		var parentDirectory = @"E:\Dev\AI\Libs\Argumentum\Generation\Converters\Argumentum.AssetConverter\DatasetUpdater\Resources\Skills\Core";
		kernel.ImportSemanticSkillFromDirectory(parentDirectory);


	}


	private async Task UpdateWithChatGPT(bool useDebugPath, string openAIKey)
	{
		try
		{
			// load dataset in chunks, 
			var chunks = await SourceDataset.SplitContentIntoJsonChunks(ChunkSize, FieldsToInclude, useDebugPath);

			//Doing short tests
			if (SkipChunkNb > 0)
			{
				chunks = chunks.Skip(SkipChunkNb).ToList();
			}

			if (TakeChunkNb > 0)
			{
				chunks = chunks.Take(TakeChunkNb).ToList();
			}

			var csvAnswers = new ConcurrentBag<string>();


			var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelismWebService };
			await Parallel.ForEachAsync(chunks, parallelOptions, async (chunk, token) =>
			{
				SystemPrompt = Resource1.JsonPromptSystem;
				var dataPrompt = new Prompt()
				{
					ApiKey = openAIKey,
					SystemPrompt = SystemPrompt,
					Example = new PromptExample()
					{
						UserPrompt = UserPrompt,
						Answer = AssistantPrompt
					},
					UserPrompt = chunk
				};

				string result = "";

				for (int i = 0; i < NbMessageCalls; i++)
				{
					Logger.Log($"Calling ChatGPT API with chunk: \n{Markup.Escape(chunk)}\n");

					try
					{
						result = await dataPrompt.Send();
						//result = chunk;
						Logger.Log($"ChatGPT answered \n{Markup.Escape(chunk)}\n with chunk \n{Markup.Escape(result)}\n");
						dataPrompt.UserPrompt = result;
					}
					catch (Exception e)
					{
						Logger.LogException(e);
					}
				}


				csvAnswers.Add(result);
			});


			// Merge answers into one csv
			var mergedCsv =
				await SourceDataset.MergeJsonResponsesIntoCsv(csvAnswers.ToList(), PrimaryField, FieldsToUpdate, ",",
					false);


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

public enum SemanticMode
{
	UpdateWithChatpGptAPI,
	IndexWithSemanticKernel
}






/// <summary>
/// Basic logger printing to console
/// </summary>
internal static class ConsoleLogger
{
	internal static ILogger Log => LogFactory.CreateLogger<object>();

	private static ILoggerFactory LogFactory => s_loggerFactory.Value;

	private static readonly Lazy<ILoggerFactory> s_loggerFactory = new(LogBuilder);

	private static ILoggerFactory LogBuilder()
	{
		return LoggerFactory.Create(builder =>
		{
			builder.SetMinimumLevel(LogLevel.Warning);

			// builder.AddFilter("Microsoft", LogLevel.Trace);
			// builder.AddFilter("Microsoft", LogLevel.Debug);
			// builder.AddFilter("Microsoft", LogLevel.Information);
			// builder.AddFilter("Microsoft", LogLevel.Warning);
			// builder.AddFilter("Microsoft", LogLevel.Error);

			builder.AddFilter("Microsoft", LogLevel.Warning);
			builder.AddFilter("System", LogLevel.Warning);

			builder.AddConsole();
		});
	}
}