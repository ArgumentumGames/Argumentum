using OpenAI.GPT3.ObjectModels;
using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Argumentum.AssetConverter.DatasetUpdater;

namespace Argumentum.AssetConverter;

public class DatasetUpdaterConfig
{
	public string SystemPrompt { get; set; } = Resource1.JsonPromptSystem;

	public string UserPrompt { get; set; } = Resource1.JsonPromptSampleUser;


	public string AssistantPrompt { get; set; } = Resource1.JsonPromptSampleAssistant;

	public string OpenAIKeyPath { get; set; } = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\OpenAI-Key.txt";

	public string Model { get; set; } = Models.ChatGpt3_5Turbo;

	public int ChunkSize { get; set; } = 3;

	public int MaxDegreeOfParallelismWebService { get; set; } = 5;

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


		try
		{

			// load dataset in chunks, 
			var chunks = await SourceDataset.SplitContentIntoJsonChunks(ChunkSize, FieldsToInclude, useDebugPath);

			//Doing short tests
			//chunks = chunks.Skip(25).Take(5).ToList();

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
				Logger.Log($"Calling ChatGPT API with chunk: \n{Markup.Escape(chunk)}\n");
				string result;
				try
				{
					result = await dataPrompt.Send();
					//result = chunk;
					Logger.Log($"ChatGPT answered \n{Markup.Escape(chunk)}\n with chunk \n{Markup.Escape(result)}\n");
				}
				catch (Exception e)
				{
					Logger.LogException(e);
					result = "";
				}


				csvAnswers.Add(result);
			});



			// Merge answers into one csv
			var mergedCsv = await SourceDataset.MergeJsonResponsesIntoCsv(csvAnswers.ToList(), PrimaryField, FieldsToUpdate, ",", false);


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