using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI.ObjectModels;

namespace Argumentum.AssetConverter;

public class DatasetUpdaterRootConfig
{

	public async Task Apply(AssetConverterConfig config)
	{
		foreach (var datasetUpdaterConfig in this.DatasetUpdaterConfigs)
		{
			if (datasetUpdaterConfig.Enabled)
			{
				Logger.LogTitle($"Updating Dataset {datasetUpdaterConfig.SourceDataset}");
				await datasetUpdaterConfig.Apply(config).ConfigureAwait(false);
				Logger.LogTitle($"Updated Dataset {datasetUpdaterConfig.SourceDataset}");
			}
		}
	}

	private const string PromptsRootPath = @".\DatasetUpdater\Resources\";

	public List<DatasetUpdaterConfig> DatasetUpdaterConfigs { get; set; } = new List<DatasetUpdaterConfig>()
	{
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			SourceDataset = @"Argumentum - Virtues - Taxonomy",
			FieldsToInclude = new List<string>()
			{
				"path",
				"family_fr",
				"subfamily_fr",
				"subsubfamily_fr",
				"title_fr",
				"description_fr",
				"remark_fr",
				"link_fr"
			},
			FieldsToUpdate = new List<string>()
			{
				"title_fr",
				"description_fr",
				"remark_fr",
				"link_fr"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "VirtuesJsonPromptSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "VirtuesJsonPromptSampleUser.json",
					AssistantAnswerPath = PromptsRootPath + "VirtuesJsonPromptSampleAssistant.json"
				}
			},
			Model = Models.Gpt_3_5_Turbo_1106,
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 3,
			UseFunctionCalling = false,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			MaxDegreeOfParallelismWebService = 2
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			SourceDataset = @"Argumentum - Fallacies - Taxonomy",
			FieldsToInclude = new List<string>()
			{
				"path",
				//"Famille",
				//"Sous-Famille",
				//"Soussousfamille",
				"text_fr",
				"desc_fr",
				//"example_fr",
				//"carte",
				//"link_fr"
				"text_en",
				"desc_en",
				//"example_en"
				"link_en"
			},
			FieldsToUpdate = new List<string>()
			{
				//"path",
				//"text_fr",
				"desc_fr",
				//"example_fr",
				//"link_fr"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Fallacies - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptDocumentsLightUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptDocumentsAssistant.txt"
				},
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptInstructionsUserDescription.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptInstructionsAssistantDescription.txt"
				}
			},
			Model = Models.Gpt_4_1106_preview,
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.PKHierarchicalChar,
			PKHierarchyLevel = 3,
			UseFunctionCalling = true,
			//FunctionName = nameof(RecordsUpdater.UpdateRecord),
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			RandomizeChunks = true,
			MaxDegreeOfParallelismWebService = 3,
			CompareMode = true,
			AutoCompare = true,
			AutoCompareField = "text_fr",
			CompareField = "desc_fr",
			MaxGroupItemNb = 20,
			MaxChildren = 12,
			SelectEmptyTargets = false
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			SourceDataset = @"Argumentum - Fallacies - Taxonomy",
			FieldsToInclude = new List<string>()
			{
				"path",
				//"Famille",
				//"Sous-Famille",
				//"Soussousfamille",
				"text_fr",
				"desc_fr",
				"example_fr",
				//"carte",
				//"link_fr"
				"text_en",
				//"desc_en",
				//"example_en",
				//"link_en"
			},
			FieldsToUpdate = new List<string>()
			{
				//"path",
				//"text_fr",
				//"desc_fr",
				"example_fr",
				//"link_fr"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Fallacies - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptDocumentsLightUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptDocumentsAssistant.txt"
				},
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptInstructionsLightUserExamples.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptInstructionsLightAssistantExamples.txt"
				}
			},
			Model = Models.Gpt_4_1106_preview,
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.PKHierarchicalChar,
			PKHierarchyLevel = 3,
			UseFunctionCalling = true,
			//FunctionName = nameof(RecordsUpdater.UpdateRecord),
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = 0,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 3,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "text_fr",
			CompareField = "example_fr",
			MaxGroupItemNb = 25,
		}
	};
}