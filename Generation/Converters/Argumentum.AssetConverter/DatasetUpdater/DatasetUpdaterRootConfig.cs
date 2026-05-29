using System.Collections.Generic;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.DatasetUpdater;

public class DatasetUpdaterRootConfig
{

    public async Task Apply(AssetConverterConfig config)
    {
        foreach (var datasetUpdaterConfig in DatasetUpdaterConfigs)
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
			Name = "Update Virtues Taxonomy by chunks 1-shot",
            SourceDataset = KnownDataSets.VirtuesTaxonomy,
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
            // Taxonomy creation/refinement — quality tier. Fallback: gpt-4.1-mini
            Model = "gpt-5.4",
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
            Name = "Update Fallacies French Description by branch 0-shot",
			SourceDataset = KnownDataSets.FallaciesTaxonomy,
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
            // desc_fr refinement — quality tier for nuanced taxonomic descriptions. Fallback: gpt-4.1
            Model = "gpt-5.4",
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
            Name = "Update Fallacies French example by branch 0-shot",
			SourceDataset = KnownDataSets.FallaciesTaxonomy,
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
            // example_fr creative generation — quality tier. Fallback: gpt-4.1
            Model = "gpt-5.4",
            MaxTokensPerMinute = 70000,
            DivisionMode = DivisionMode.PKHierarchicalChar,
            PKHierarchyLevel = 3,
            UseFunctionCalling = true,
			//FunctionName = nameof(RecordsUpdater.UpdateRecord),
			NbMessageCalls = 1,
            SkipChunkNb = 0,
            TakeChunkNb = 10,
            SelectEmptyTargets = true,
            RandomizeChunks = false,
            MaxDegreeOfParallelismWebService = 3,
            CompareMode = false,
            AutoCompare = true,
            AutoCompareField = "text_fr",
            CompareField = "example_fr",
            MaxGroupItemNb = 30,
            WriteOneTargetFileByField = true,
            MaxChildren = 12
        },
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Fallacies to English by branch empty-only 0-shot",
			SourceDataset = KnownDataSets.FallaciesTaxonomy,
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
				"link_fr",
				"text_en",
				"desc_en",
				"example_en",
				"link_en"
			},
			FieldsToUpdate = new List<string>()
			{
				//"path",
				//"text_fr",
				"desc_fr",
				"example_fr",
				//"link_fr",
				"text_en",
				"desc_en",
				"example_en",
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Fallacies - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptTranslateFrEnInstructionsUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptTranslateFrEnInstructionsAssistant.txt"
				}
			},
			// FR → EN translation empty-only — eco tier. Fallback: gpt-4.1-mini
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.PKHierarchicalChar,
			PKHierarchyLevel = 3,
			UseFunctionCalling = true,
			//FunctionName = nameof(RecordsUpdater.UpdateRecord),
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 3,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "text_fr",
			CompareField = "example_fr",
			MaxGroupItemNb = 30,
			WriteOneTargetFileByField = true,
			MaxChildren = 12
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Fallacies to Russian by chunk empty-only 0-shot",
			SourceDataset = KnownDataSets.FallaciesTaxonomy,
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
				"link_fr",
				"text_en",
				"desc_en",
				"example_en",
				"link_en",
				"text_ru",
				"desc_ru",
				"example_ru",
				"link_ru"
			},
			FieldsToUpdate = new List<string>()
			{
				"text_ru",
				"desc_ru",
				"example_ru"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Fallacies - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptTranslateRuInstructionsUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptTranslateRuInstructionsAssistant.txt"
				}
			},
			// FR → RU translation empty-only — eco tier. Fallback: gpt-4.1-mini
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			PKHierarchyLevel = 3,
			UseFunctionCalling = true,
			//FunctionName = nameof(RecordsUpdater.UpdateRecord),
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			ChunkSize = 8,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 5,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "text_fr",
			CompareField = "example_fr",
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = true,
			MaxChildren = 8
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Fallacies to Portuguese by chunk empty-only 0-shot",
			SourceDataset = KnownDataSets.FallaciesTaxonomy,
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
				"link_fr",
				"text_en",
				"desc_en",
				"example_en",
				"link_en",
				"text_pt",
				"desc_pt",
				"example_pt",
				"link_pt"
			},
			FieldsToUpdate = new List<string>()
			{
				"text_pt",
				"desc_pt",
				"example_pt"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Fallacies - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptTranslatePtInstructionsUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptTranslatePtInstructionsAssistant.txt"
				}
			},
			// FR → PT translation empty-only — eco tier. Fallback: gpt-4.1-mini
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			PKHierarchyLevel = 3,
			UseFunctionCalling = true,
			//FunctionName = nameof(RecordsUpdater.UpdateRecord),
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			ChunkSize = 8,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 6,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "text_fr",
			CompareField = "example_fr",
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = false,
			MaxChildren = 8,
			NbGlobalPasses = 2
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Cleanup Fallacies translations by chunk empty-only 0-shot",
			SourceDataset = KnownDataSets.FallaciesTaxonomy,
			FieldsToInclude = new List<string>()
			{
				"path",
				"Famille",
				"Sous-Famille",
				"Soussousfamille",
				"text_fr",
				"desc_fr",
				"example_fr",
				//"carte",
				"link_fr",
				"text_en",
				"desc_en",
				"example_en",
				"link_en",
				"text_ru",
				"desc_ru",
				"example_ru",
				"text_pt",
				"desc_pt",
				"example_pt",
			},
			FieldsToUpdate = new List<string>()
			{
				"text_en",
				"desc_en",
				"example_en",
				"text_ru",
				"desc_ru",
				"example_ru",
				"text_pt",
				"desc_pt",
				"example_pt"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Fallacies - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptTranslateCleanupInstructionsUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptTranslateCleanupInstructionsAssistant.txt"
				}
			},
			// Multi-lang cleanup review — quality tier for nuanced comparisons. Fallback: gpt-4.1
			Model = "gpt-5.4",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			PKHierarchyLevel = 3,
			UseFunctionCalling = true,
			//FunctionName = nameof(RecordsUpdater.UpdateRecord),
			NbMessageCalls = 2,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			ChunkSize = 8,
			SelectEmptyTargets = false,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 6,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "text_fr",
			CompareField = "example_fr",
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = true,
			MaxChildren = 8,
			NbGlobalPasses = 2
},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Virtues to English by chunk empty-only 0-shot",
			SourceDataset = KnownDataSets.VirtuesTaxonomy,
			FieldsToInclude = new List<string>()
			{
				"path",
				"family_fr",
				"subfamily_fr",
				"subsubfamily_fr",
				"title_fr",
				"description_fr",
				"remark_fr",
				"link_fr",
				"family_en",
				"subfamily_en",
				"subsubfamily_en",
				"title_en",
				"description_en",
				"remark_en",
				"link_en"
			},
			FieldsToUpdate = new List<string>()
			{
				"family_en",
				"subfamily_en",
				"subsubfamily_en",
				"title_en",
				"description_en",
				"remark_en",
				"link_en"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptVirtuesTranslateEnUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptVirtuesTranslateEnAssistant.txt"
				}
			},
			// FR → EN via OpenAI gpt-5.5 (best quality per benchmark)
			Model = "gpt-5.5",
			OpenAIKeyPath = @".keys\openai-key.txt",
			MaxOutputTokens = 4096,
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 4,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 4,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "title_fr",
			CompareField = "title_en",
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = true,
			MaxChildren = 8
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Virtues to Russian by chunk empty-only 0-shot",
			SourceDataset = KnownDataSets.VirtuesTaxonomy,
			FieldsToInclude = new List<string>()
			{
				"path",
				"family_fr",
				"subfamily_fr",
				"subsubfamily_fr",
				"title_fr",
				"description_fr",
				"remark_fr",
				"link_fr",
				"family_ru",
				"subfamily_ru",
				"subsubfamily_ru",
				"title_ru",
				"description_ru",
				"remark_ru",
				"link_ru"
			},
			FieldsToUpdate = new List<string>()
			{
				"family_ru",
				"subfamily_ru",
				"subsubfamily_ru",
				"title_ru",
				"description_ru",
				"remark_ru",
				"link_ru"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptVirtuesTranslateRuUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptVirtuesTranslateRuAssistant.txt"
				}
			},
			// FR → RU translation empty-only — eco tier. Fallback: gpt-4.1-mini
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 8,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 4,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "title_fr",
			CompareField = "title_ru",
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = true,
			MaxChildren = 8
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Virtues to Portuguese by chunk empty-only 0-shot",
			SourceDataset = KnownDataSets.VirtuesTaxonomy,
			FieldsToInclude = new List<string>()
			{
				"path",
				"family_fr",
				"subfamily_fr",
				"subsubfamily_fr",
				"title_fr",
				"description_fr",
				"remark_fr",
				"link_fr",
				"family_pt",
				"subfamily_pt",
				"subsubfamily_pt",
				"title_pt",
				"description_pt",
				"remark_pt",
				"link_pt"
			},
			FieldsToUpdate = new List<string>()
			{
				"family_pt",
				"subfamily_pt",
				"subsubfamily_pt",
				"title_pt",
				"description_pt",
				"remark_pt",
				"link_pt"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptVirtuesTranslatePtUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptVirtuesTranslatePtAssistant.txt"
				}
			},
			// FR → PT translation empty-only — eco tier. Fallback: gpt-4.1-mini
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 8,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 4,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "title_fr",
			CompareField = "title_pt",
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = true,
			MaxChildren = 8
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Generate Virtues Portuguese Wikipedia links empty-only 0-shot",
			SourceDataset = KnownDataSets.VirtuesTaxonomy,
			FieldsToInclude = new List<string>()
			{
				"path",
				"title_fr",
				"title_pt",
				"link_fr",
				"link_en",
				"link_pt"
			},
			FieldsToUpdate = new List<string>()
			{
				"link_pt"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptVirtuesLinksPtUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptVirtuesLinksPtAssistant.txt"
				}
			},
			// Wikipedia link lookup - eco tier. Fallback: gpt-4.1-mini
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 8,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 4,
			CompareMode = false,
			AutoCompare = false,
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = false,
			MaxChildren = 8
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Virtues to Spanish by chunk empty-only 0-shot",
			SourceDataset = KnownDataSets.VirtuesTaxonomy,
			FieldsToInclude = new List<string>()
			{
				"path",
				"family_fr",
				"subfamily_fr",
				"subsubfamily_fr",
				"title_fr",
				"description_fr",
				"remark_fr",
				"link_fr",
				"family_es",
				"subfamily_es",
				"subsubfamily_es",
				"title_es",
				"description_es",
				"remark_es",
				"link_es"
			},
			FieldsToUpdate = new List<string>()
			{
				"family_es",
				"subfamily_es",
				"subsubfamily_es",
				"title_es",
				"description_es",
				"remark_es",
				"link_es"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptVirtuesTranslateEsUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptVirtuesTranslateEsAssistant.txt"
				}
			},
			// FR -> ES translation empty-only - quality tier
			Model = "gpt-5.5",
			MaxOutputTokens = 4096,
			MaxTokensPerMinute = 300000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 4,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 1,
			CompareMode = false,
			AutoCompare = false,
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = false,
			MaxChildren = 8
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Rules to Portuguese by chunk 0-shot",
			SourceDataset = KnownDataSets.Rules,
			FieldsToInclude = new List<string>()
			{
				"pk",
				"Text",
				"Text_pt"
			},
			FieldsToUpdate = new List<string>()
			{
				"Text_pt"
			},
			PrimaryField = "pk",
			TargetPath = @".\Target\Datasets\Argumentum Rules - Cards.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptRulesTranslatePtUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptRulesTranslatePtAssistant.txt"
				}
			},
			Model = "gpt-5.5", // Fallback: gpt-4.1-mini
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 3,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = false,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 3,
			CompareMode = false,
			AutoCompare = false,
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = false,
			MaxChildren = 8
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Scenarii to English by chunk empty-only 0-shot",
			SourceDataset = KnownDataSets.Scenarii,
			FieldsToInclude = new List<string>()
			{
				"path",
				"catégorie",
				"sous-catégorie",
				"titre",
				"baratineur",
				"piocheur",
				"contexte",
				"enjeu",
				"suggestion",
				"category",
				"subcategory",
				"title",
				"smoothTalker",
				"drawer",
				"context",
				"issue",
				"suggestion_en"
			},
			FieldsToUpdate = new List<string>()
			{
				"category",
				"subcategory",
				"title",
				"smoothTalker",
				"drawer",
				"context",
				"issue",
				"suggestion_en"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptScenariiTranslateEnUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptScenariiTranslateEnAssistant.txt"
				}
			},
			// FR → EN via OpenAI gpt-5.5 (best quality per benchmark)
			Model = "gpt-5.5",
			OpenAIKeyPath = @".keys\openai-key.txt",
			MaxOutputTokens = 4096,
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 4,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 4,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "titre",
			CompareField = "title",
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = true,
			MaxChildren = 8
		},

		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Scenarii to Russian by chunk empty-only 0-shot",
			SourceDataset = KnownDataSets.Scenarii,
			FieldsToInclude = new List<string>()
			{
				"path",
				"catégorie",
				"sous-catégorie",
				"titre",
				"baratineur",
				"piocheur",
				"contexte",
				"enjeu",
				"suggestion",
				"category_ru",
				"subcategory_ru",
				"title_ru",
				"smoothTalker_ru",
				"drawer_ru",
				"context_ru",
				"issue_ru",
				"suggestion_ru"
			},
			FieldsToUpdate = new List<string>()
			{
				"category_ru",
				"subcategory_ru",
				"title_ru",
				"smoothTalker_ru",
				"drawer_ru",
				"context_ru",
				"issue_ru",
				"suggestion_ru"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptScenariiTranslateRuUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptScenariiTranslateRuAssistant.txt"
				}
			},
			// FR → RU translation empty-only — eco tier. Fallback: gpt-4.1-mini
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 8,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 4,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "titre",
			CompareField = "title_ru",
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = true,
			MaxChildren = 8
		},

		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Scenarii to Portuguese by chunk empty-only 0-shot",
			SourceDataset = KnownDataSets.Scenarii,
			FieldsToInclude = new List<string>()
			{
				"path",
				"catégorie",
				"sous-catégorie",
				"titre",
				"baratineur",
				"piocheur",
				"contexte",
				"enjeu",
				"suggestion",
				"category_pt",
				"subcategory_pt",
				"title_pt",
				"smoothTalker_pt",
				"drawer_pt",
				"context_pt",
				"issue_pt",
				"suggestion_pt"
			},
			FieldsToUpdate = new List<string>()
			{
				"category_pt",
				"subcategory_pt",
				"title_pt",
				"smoothTalker_pt",
				"drawer_pt",
				"context_pt",
				"issue_pt",
				"suggestion_pt"
			},
			PrimaryField = "path",
			TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptScenariiTranslatePtUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptScenariiTranslatePtAssistant.txt"
				}
			},
			// FR → PT translation empty-only — eco tier. Fallback: gpt-4.1-mini
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 8,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = true,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 4,
			CompareMode = false,
			AutoCompare = true,
			AutoCompareField = "titre",
			CompareField = "title_pt",
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = true,
			MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Refine Virtues ES translations",
				SourceDataset = KnownDataSets.VirtuesTaxonomy,
				FieldsToInclude = new List<string>()
				{
					"path",
					"family_fr",
					"subfamily_fr",
					"subsubfamily_fr",
					"title_fr",
					"description_fr",
					"remark_fr",
					"link_fr",
					"family_es",
					"subfamily_es",
					"subsubfamily_es",
					"title_es",
					"description_es",
					"remark_es",
					"link_es"
				},
				FieldsToUpdate = new List<string>()
				{
					"family_es",
					"subfamily_es",
					"subsubfamily_es",
					"title_es",
					"description_es",
					"remark_es",
					"link_es"
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptVirtuesRefineEsUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptVirtuesRefineEsAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.PKHierarchicalChar,
				PKHierarchyLevel = 2,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
					AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 12
			},

			new DatasetUpdaterConfig()
			{
				Enabled = false , 
				Name = "Translate Virtues to Ar by chunk empty-only 0-shot",
				SourceDataset = KnownDataSets.VirtuesTaxonomy,
				FieldsToInclude = new List<string>()
				{
					"path",
					"family_fr",
					"subfamily_fr",
					"subsubfamily_fr",
					"title_fr",
					"description_fr",
					"remark_fr",
					"link_fr",
					"family_ar",
					"subfamily_ar",
					"subsubfamily_ar",
					"title_ar",
					"description_ar",
					"remark_ar",
					"link_ar"
				},
				FieldsToUpdate = new List<string>()
				{
					"family_ar",
					"subfamily_ar",
					"subsubfamily_ar",
					"title_ar",
					"description_ar",
					"remark_ar",
					"link_ar"
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptVirtuesTranslateArUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptVirtuesTranslateArAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.PKHierarchicalChar,
				PKHierarchyLevel = 2,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = true,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 12
			},

			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Translate Virtues to Fa by chunk empty-only 0-shot",
				SourceDataset = KnownDataSets.VirtuesTaxonomy,
				FieldsToInclude = new List<string>()
				{
					"path",
					"family_fr",
					"subfamily_fr",
					"subsubfamily_fr",
					"title_fr",
					"description_fr",
					"remark_fr",
					"link_fr",
					"family_fa",
					"subfamily_fa",
					"subsubfamily_fa",
					"title_fa",
					"description_fa",
					"remark_fa",
					"link_fa"
				},
				FieldsToUpdate = new List<string>()
				{
					"family_fa",
					"subfamily_fa",
					"subsubfamily_fa",
					"title_fa",
					"description_fa",
					"remark_fa",
					"link_fa"
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptVirtuesTranslateFaUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptVirtuesTranslateFaAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.PKHierarchicalChar,
				PKHierarchyLevel = 2,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = true,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 12
			},

			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Translate Virtues to Zh by chunk empty-only 0-shot",
				SourceDataset = KnownDataSets.VirtuesTaxonomy,
				FieldsToInclude = new List<string>()
				{
					"path",
					"family_fr",
					"subfamily_fr",
					"subsubfamily_fr",
					"title_fr",
					"description_fr",
					"remark_fr",
					"link_fr",
					"family_zh",
					"subfamily_zh",
					"subsubfamily_zh",
					"title_zh",
					"description_zh",
					"remark_zh",
					"link_zh"
				},
				FieldsToUpdate = new List<string>()
				{
					"family_zh",
					"subfamily_zh",
					"subsubfamily_zh",
					"title_zh",
					"description_zh",
					"remark_zh",
					"link_zh"
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptVirtuesTranslateZhUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptVirtuesTranslateZhAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.PKHierarchicalChar,
				PKHierarchyLevel = 2,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = true,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 12
			},

		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Rules to Spanish by chunk 0-shot",
			SourceDataset = KnownDataSets.Rules,
			FieldsToInclude = new List<string>()
			{
				"pk",
				"Text",
				"Text_es"
			},
			FieldsToUpdate = new List<string>()
			{
				"Text_es"
			},
			PrimaryField = "pk",
			TargetPath = @".\Target\Datasets\Argumentum Rules - Cards.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptRulesTranslateEsUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptRulesTranslateEsAssistant.txt"
				}
			},
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 3,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = false,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 3,
			CompareMode = false,
			AutoCompare = false,
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = false,
			MaxChildren = 8
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Rules to Arabic by chunk 0-shot",
			SourceDataset = KnownDataSets.Rules,
			FieldsToInclude = new List<string>()
			{
				"pk",
				"Text",
				"Text_ar"
			},
			FieldsToUpdate = new List<string>()
			{
				"Text_ar"
			},
			PrimaryField = "pk",
			TargetPath = @".\Target\Datasets\Argumentum Rules - Cards.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptRulesTranslateArUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptRulesTranslateArAssistant.txt"
				}
			},
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 3,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = false,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 3,
			CompareMode = false,
			AutoCompare = false,
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = false,
			MaxChildren = 8
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Rules to Farsi by chunk 0-shot",
			SourceDataset = KnownDataSets.Rules,
			FieldsToInclude = new List<string>()
			{
				"pk",
				"Text",
				"Text_fa"
			},
			FieldsToUpdate = new List<string>()
			{
				"Text_fa"
			},
			PrimaryField = "pk",
			TargetPath = @".\Target\Datasets\Argumentum Rules - Cards.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptRulesTranslateFaUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptRulesTranslateFaAssistant.txt"
				}
			},
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 3,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = false,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 3,
			CompareMode = false,
			AutoCompare = false,
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = false,
			MaxChildren = 8
		},
		new DatasetUpdaterConfig()
		{
			Enabled = false,
			Name = "Translate Rules to Chinese Simplified by chunk 0-shot",
			SourceDataset = KnownDataSets.Rules,
			FieldsToInclude = new List<string>()
			{
				"pk",
				"Text",
				"Text_zh"
			},
			FieldsToUpdate = new List<string>()
			{
				"Text_zh"
			},
			PrimaryField = "pk",
			TargetPath = @".\Target\Datasets\Argumentum Rules - Cards.csv",
			SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
			DialogPrompts = new List<PromptExample>()
			{
				new PromptExample()
				{
					UserPromptPath = PromptsRootPath + "PromptRulesTranslateZhUser.txt",
					AssistantAnswerPath = PromptsRootPath + "PromptRulesTranslateZhAssistant.txt"
				}
			},
			Model = "gpt-5.4-mini",
			MaxTokensPerMinute = 70000,
			DivisionMode = DivisionMode.SequentialChunks,
			ChunkSize = 3,
			UseFunctionCalling = true,
			NbMessageCalls = 1,
			SkipChunkNb = 0,
			TakeChunkNb = -1,
			SelectEmptyTargets = false,
			RandomizeChunks = false,
			MaxDegreeOfParallelismWebService = 3,
			CompareMode = false,
			AutoCompare = false,
			MaxGroupItemNb = 12,
			WriteOneTargetFileByField = false,
			MaxChildren = 8
		},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Translate Scenarii to Spanish empty-only gpt-5.5",
				SourceDataset = KnownDataSets.Scenarii,
				FieldsToInclude = new List<string>()
				{
					"path",
					"catégorie",
					"sous-catégorie",
					"titre",
					"baratineur",
					"piocheur",
					"contexte",
					"enjeu",
					"suggestion",
					"category_es",
					"subcategory_es",
					"title_es",
					"smoothTalker_es",
					"drawer_es",
					"context_es",
					"issue_es",
					"suggestion_es",
				},
				FieldsToUpdate = new List<string>()
				{
					"category_es",
					"subcategory_es",
					"title_es",
					"smoothTalker_es",
					"drawer_es",
					"context_es",
					"issue_es",
					"suggestion_es",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptScenariiTranslateEsUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptScenariiTranslateEsAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = true,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Translate Scenarii to Arabic empty-only gpt-5.5",
				SourceDataset = KnownDataSets.Scenarii,
				FieldsToInclude = new List<string>()
				{
					"path",
					"catégorie",
					"sous-catégorie",
					"titre",
					"baratineur",
					"piocheur",
					"contexte",
					"enjeu",
					"suggestion",
					"category_ar",
					"subcategory_ar",
					"title_ar",
					"smoothTalker_ar",
					"drawer_ar",
					"context_ar",
					"issue_ar",
					"suggestion_ar",
				},
				FieldsToUpdate = new List<string>()
				{
					"category_ar",
					"subcategory_ar",
					"title_ar",
					"smoothTalker_ar",
					"drawer_ar",
					"context_ar",
					"issue_ar",
					"suggestion_ar",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptScenariiTranslateArUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptScenariiTranslateArAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = true,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Translate Scenarii to Persian empty-only gpt-5.5",
				SourceDataset = KnownDataSets.Scenarii,
				FieldsToInclude = new List<string>()
				{
					"path",
					"catégorie",
					"sous-catégorie",
					"titre",
					"baratineur",
					"piocheur",
					"contexte",
					"enjeu",
					"suggestion",
					"category_fa",
					"subcategory_fa",
					"title_fa",
					"smoothTalker_fa",
					"drawer_fa",
					"context_fa",
					"issue_fa",
					"suggestion_fa",
				},
				FieldsToUpdate = new List<string>()
				{
					"category_fa",
					"subcategory_fa",
					"title_fa",
					"smoothTalker_fa",
					"drawer_fa",
					"context_fa",
					"issue_fa",
					"suggestion_fa",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptScenariiTranslateFaUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptScenariiTranslateFaAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = true,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Translate Scenarii to Chinese empty-only gpt-5.5",
				SourceDataset = KnownDataSets.Scenarii,
				FieldsToInclude = new List<string>()
				{
					"path",
					"catégorie",
					"sous-catégorie",
					"titre",
					"baratineur",
					"piocheur",
					"contexte",
					"enjeu",
					"suggestion",
					"category_zh",
					"subcategory_zh",
					"title_zh",
					"smoothTalker_zh",
					"drawer_zh",
					"context_zh",
					"issue_zh",
					"suggestion_zh",
				},
				FieldsToUpdate = new List<string>()
				{
					"category_zh",
					"subcategory_zh",
					"title_zh",
					"smoothTalker_zh",
					"drawer_zh",
					"context_zh",
					"issue_zh",
					"suggestion_zh",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptScenariiTranslateZhUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptScenariiTranslateZhAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = true,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Rules FR clarity review gpt-5.5",
				SourceDataset = KnownDataSets.Rules,
				FieldsToInclude = new List<string>()
				{
					"pk",
					"Text",
				},
				FieldsToUpdate = new List<string>()
				{
					"Text",
				},
				PrimaryField = "pk",
				TargetPath = @".\Target\Datasets\Argumentum Rules - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptRulesFrClarityUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptRulesFrClarityAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Scenarii FR clarity review gpt-5.5",
				SourceDataset = KnownDataSets.Scenarii,
				FieldsToInclude = new List<string>()
				{
					"path",
					"titre",
					"contexte",
					"enjeu",
					"suggestion",
				},
				FieldsToUpdate = new List<string>()
				{
					"titre",
					"contexte",
					"enjeu",
					"suggestion",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptScenariiFrClarityUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptScenariiFrClarityAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 8192,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				PKHierarchicalChar = '.',
				PKHierarchyLevel = 1,
				ChunkSize = 12,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Scenarii cascade multi-lang gpt-5.5",
				SourceDataset = KnownDataSets.Scenarii,
				FieldsToInclude = new List<string>()
				{
					"path",
					"titre","contexte","enjeu","suggestion",
					"title","context","issue","suggestion_en",
					"title_ru","context_ru","issue_ru","suggestion_ru",
					"title_pt","context_pt","issue_pt","suggestion_pt",
					"title_es","context_es","issue_es","suggestion_es",
					"title_ar","context_ar","issue_ar","suggestion_ar",
					"title_fa","context_fa","issue_fa","suggestion_fa",
					"title_zh","context_zh","issue_zh","suggestion_zh",
				},
				FieldsToUpdate = new List<string>()
				{
					"title_ru","context_ru","issue_ru","suggestion_ru",
					"title_pt","context_pt","issue_pt","suggestion_pt",
					"title_es","context_es","issue_es","suggestion_es",
					"title_ar","context_ar","issue_ar","suggestion_ar",
					"title_fa","context_fa","issue_fa","suggestion_fa",
					"title_zh","context_zh","issue_zh","suggestion_zh",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptScenariiCascadeDriftUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptScenariiCascadeDriftAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 8192,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				PKHierarchicalChar = '.',
				PKHierarchyLevel = 3,
				ChunkSize = 12,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = 1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Fallacies FR clarity review gpt-5.5",
				SourceDataset = KnownDataSets.FallaciesTaxonomy,
				FieldsToInclude = new List<string>()
				{
					"path",
					"text_fr",
					"desc_fr",
					"example_fr",
				},
				FieldsToUpdate = new List<string>()
				{
					"text_fr",
					"desc_fr",
					"example_fr",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Fallacies - Taxonomy.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptFallaciesFrClarityUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptFallaciesFrClarityAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 8192,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.PKHierarchicalChar,
				PKHierarchicalChar = '.',
				PKHierarchyLevel = 2,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 20
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Fallacies cascade multi-lang gpt-5.5",
				SourceDataset = KnownDataSets.FallaciesTaxonomy,
				FieldsToInclude = new List<string>()
				{
					"path",
					"text_fr","desc_fr","example_fr",
					"text_en","desc_en","example_en",
					"text_ru","desc_ru","example_ru",
					"text_pt","desc_pt","example_pt",
					"text_es","desc_es","example_es",
					"text_ar","desc_ar","example_ar",
					"text_fa","desc_fa","example_fa",
					"text_zh","desc_zh","example_zh",
				},
				FieldsToUpdate = new List<string>()
				{
					"text_ru","desc_ru","example_ru",
					"text_pt","desc_pt","example_pt",
					"text_es","desc_es","example_es",
					"text_ar","desc_ar","example_ar",
					"text_fa","desc_fa","example_fa",
					"text_zh","desc_zh","example_zh",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Fallacies - Taxonomy.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptFallaciesCascadeDriftUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptFallaciesCascadeDriftAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 8192,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.PKHierarchicalChar,
				PKHierarchicalChar = '.',
				PKHierarchyLevel = 2,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = 1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 20
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Virtues FR clarity review gpt-5.5",
				SourceDataset = KnownDataSets.VirtuesTaxonomy,
				FieldsToInclude = new List<string>()
				{
					"pk",
					"title_fr",
					"description_fr",
					"remark_fr",
				},
				FieldsToUpdate = new List<string>()
				{
					"title_fr",
					"description_fr",
					"remark_fr",
				},
				PrimaryField = "pk",
				TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptVirtuesFrClarityUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptVirtuesFrClarityAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 8192,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Rules cascade multi-lang gpt-5.5",
				SourceDataset = KnownDataSets.Rules,
				FieldsToInclude = new List<string>()
				{
					"pk",
					"Text",
					"Text_en",
					"Text_ru","Text_pt","Text_es",
					"Text_ar","Text_fa","Text_zh",
				},
				FieldsToUpdate = new List<string>()
				{
					"Text_ru","Text_pt","Text_es",
					"Text_ar","Text_fa","Text_zh",
				},
				PrimaryField = "pk",
				TargetPath = @".\Target\Datasets\Argumentum Rules - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptRulesCascadeDriftUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptRulesCascadeDriftAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				ChunkSize = 8,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = 1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Virtues cascade multi-lang gpt-5.5",
				SourceDataset = KnownDataSets.VirtuesTaxonomy,
				FieldsToInclude = new List<string>()
				{
					"pk",
					"title_fr","description_fr","remark_fr",
					"title_en","description_en","remark_en",
					"title_ru","description_ru","remark_ru",
					"title_pt","description_pt","remark_pt",
					"title_es","description_es","remark_es",
					"title_ar","description_ar","remark_ar",
					"title_fa","description_fa","remark_fa",
					"title_zh","description_zh","remark_zh",
				},
				FieldsToUpdate = new List<string>()
				{
					"title_ru","description_ru","remark_ru",
					"title_pt","description_pt","remark_pt",
					"title_es","description_es","remark_es",
					"title_ar","description_ar","remark_ar",
					"title_fa","description_fa","remark_fa",
					"title_zh","description_zh","remark_zh",
				},
				PrimaryField = "pk",
				TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptVirtuesCascadeDriftUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptVirtuesCascadeDriftAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 8192,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				ChunkSize = 6,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = 1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Rules cascade EN-only gpt-5.5",
				SourceDataset = KnownDataSets.Rules,
				FieldsToInclude = new List<string>()
				{
					"pk",
					"Text",
					"Text_en",
				},
				FieldsToUpdate = new List<string>()
				{
					"Text_en",
				},
				PrimaryField = "pk",
				TargetPath = @".\Target\Datasets\Argumentum Rules - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptRulesCascadeDriftUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptRulesCascadeDriftAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 4096,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				ChunkSize = 8,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = 1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Virtues cascade EN-only gpt-5.5",
				SourceDataset = KnownDataSets.VirtuesTaxonomy,
				FieldsToInclude = new List<string>()
				{
					"pk",
					"title_fr","description_fr","remark_fr",
					"title_en","description_en","remark_en",
				},
				FieldsToUpdate = new List<string>()
				{
					"title_en","description_en","remark_en",
				},
				PrimaryField = "pk",
				TargetPath = @".\Target\Datasets\Argumentum Virtues - Taxonomy.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptVirtuesCascadeDriftUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptVirtuesCascadeDriftAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 8192,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				ChunkSize = 6,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = 1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Scenarii cascade EN-only gpt-5.5",
				SourceDataset = KnownDataSets.Scenarii,
				FieldsToInclude = new List<string>()
				{
					"path",
					"titre","contexte","enjeu","suggestion",
					"title","context","issue","suggestion_en",
				},
				FieldsToUpdate = new List<string>()
				{
					"title","context","issue","suggestion_en",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptScenariiCascadeDriftUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptScenariiCascadeDriftAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 8192,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				PKHierarchicalChar = '.',
				PKHierarchyLevel = 3,
				ChunkSize = 12,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = 1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = true,
				Name = "Scenarii PT refine gpt-5.5",
				SourceDataset = KnownDataSets.Scenarii,
				FieldsToInclude = new List<string>()
				{
					"path",
					"titre","contexte","enjeu","suggestion",
					"title","context","issue","suggestion_en",
					"title_pt","context_pt","issue_pt","suggestion_pt",
				},
				FieldsToUpdate = new List<string>()
				{
					"title_pt","context_pt","issue_pt","suggestion_pt",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Scenarii - Cards.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptScenariiPtRefineUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptScenariiPtRefineAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 8192,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.SequentialChunks,
				PKHierarchicalChar = '.',
				PKHierarchyLevel = 3,
				ChunkSize = 12,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = -1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 4,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 8
			},
			new DatasetUpdaterConfig()
			{
				Enabled = false,
				Name = "Fallacies cascade EN-only gpt-5.5",
				SourceDataset = KnownDataSets.FallaciesTaxonomy,
				FieldsToInclude = new List<string>()
				{
					"path",
					"text_fr","desc_fr","example_fr",
					"text_en","desc_en","example_en",
				},
				FieldsToUpdate = new List<string>()
				{
					"text_en","desc_en","example_en",
				},
				PrimaryField = "path",
				TargetPath = @".\Target\Datasets\Argumentum Fallacies - Taxonomy.csv",
				SystemPromptPath = PromptsRootPath + "PromptGeneralSystem.txt",
				DialogPrompts = new List<PromptExample>()
				{
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptFallaciesCascadeDriftUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptFallaciesCascadeDriftAssistant.txt"
					}
				},
				Model = "gpt-5.5",
				MaxOutputTokens = 8192,
				MaxTokensPerMinute = 300000,
				DivisionMode = DivisionMode.PKHierarchicalChar,
				PKHierarchicalChar = '.',
				PKHierarchyLevel = 2,
				ChunkSize = 4,
				UseFunctionCalling = true,
				NbMessageCalls = 1,
				SkipChunkNb = 0,
				TakeChunkNb = 1,
				SelectEmptyTargets = false,
				RandomizeChunks = false,
				MaxDegreeOfParallelismWebService = 1,
				CompareMode = false,
				AutoCompare = false,
				MaxGroupItemNb = 12,
				WriteOneTargetFileByField = false,
				MaxChildren = 20
			}
		};
}
