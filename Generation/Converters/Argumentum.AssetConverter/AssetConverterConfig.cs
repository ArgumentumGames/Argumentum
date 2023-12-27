using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Argumentum.AssetConverter.Dnn2sxc;
using Argumentum.AssetConverter.Mindmapper;
using OpenAI.ObjectModels;
using Spectre.Console;
using Spectre.Console.Json;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;

namespace Argumentum.AssetConverter
{
    public class AssetConverterConfig
    {

		private const string PromptsRootPath = @".\DatasetUpdater\Resources\";


	    public bool SkipConfigFile { get; set; } = true;

		public ConverterMode Mode { get; set; } = ConverterMode.DatasetUpdater;

		public bool ForceDebugParams { get; set; }

		public bool ForceReleaseParams { get; set; }

		public WebBasedGeneratorConfig WebBasedGeneratorConfig { get; set; } = new WebBasedGeneratorConfig();

        public BatchImageConverterConfig BatchImageConverterConfig { get; set; } = new BatchImageConverterConfig();

        public List<DatasetUpdaterConfig> DatasetUpdaterConfigs { get; set; } = new List<DatasetUpdaterConfig>()
        {
            new DatasetUpdaterConfig()
            {
				Enabled = false,
				SourceDataset = new DataSetInfo()
				{
					Name = "Argumentum - Virtues - Taxonomy",
					ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Virtues%20-%20Taxonomy.csv",
					DebugFilePath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Virtues - Taxonomy.csv",
				},
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
				Enabled = true,
				SourceDataset = new DataSetInfo()
				{
					Name = "Argumentum - Fallacies - Taxonomy",
					ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Fallacies%20-%20Taxonomy.csv",
					DebugFilePath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv",
				},
				FieldsToInclude = new List<string>()
				{
					"path",
					//"Famille",
					//"Sous-Famille",
					//"Soussousfamille",
					"text_fr",
					"desc_fr",
					"example_fr",
					"carte",
					//"link_fr"
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
						UserPromptPath = PromptsRootPath + "PromptDocumentsUser.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptDocumentsAssistant.txt"
					},
					new PromptExample()
					{
						UserPromptPath = PromptsRootPath + "PromptInstructionsUserExamples.txt",
						AssistantAnswerPath = PromptsRootPath + "PromptInstructionsAssistantExamples.txt"
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
				MaxDegreeOfParallelismWebService = 3,
				CompareMode = false,
				AutoCompare = true,
				AutoCompareField = "text_fr",
				CompareField = "example_fr"
			}
		};

		public Dnn2sxcConfig Dnn2sxcConfig { get; set; } = new Dnn2sxcConfig();

		public MindMapCreatorConfig MindMapCreatorConfig { get; set; } = new MindMapCreatorConfig();


		public static AssetConverterConfig GetConfig(string path, out bool newConfig)
        {
            AssetConverterConfig toReturn;
            CompositeResolver.RegisterAndSetAsDefault(new IJsonFormatter[] { new TimeSpanFormatter() }, new IJsonFormatterResolver[] { StandardResolver.Default });
            newConfig = false;
            if (!File.Exists(path))
            {

                toReturn = new AssetConverterConfig();
                var strNewConfig = JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(toReturn));


                File.WriteAllText(path, strNewConfig);
                newConfig = true;


				//Logger.LogJson(strNewConfig);

				Logger.LogSuccess($"Config file created: {path}");
			}

            
            using var configStream = File.OpenRead(path);

            

			toReturn = JsonSerializer.Deserialize<AssetConverterConfig>(configStream);

			if (toReturn.SkipConfigFile)
			{
				Logger.Log($"Config loaded and skipped: {path}");
				toReturn = new AssetConverterConfig();
			}
			else
			{
				Logger.Log($"Config loaded: {path}");
			}
			
			return toReturn;
        }


        [IgnoreDataMember]
        [JsonIgnore]
        public bool UseDebugParams => (isInDebugMode || ForceDebugParams) && !ForceReleaseParams;


#if DEBUG
		bool isInDebugMode = true;
#else
		bool isInDebugMode = false;
#endif

		public async Task<bool> Apply()
        {
            switch (Mode)
            {
                case ConverterMode.BatchImageProcessor:
                    BatchImageConverterConfig.Apply();
                    break;
                case ConverterMode.WebBasedImageGeneration:
	                WebBasedGeneratorConfig.UseDebugParams = () => UseDebugParams;
					WebBasedGeneratorConfig.Apply();
					break;
				case ConverterMode.Mindmapper:
					MindMapCreatorConfig.Run(null);
					break;
				case ConverterMode.Dnn2sxc:
					Dnn2sxcConfig.Apply();
					break;
				case ConverterMode.DatasetUpdater:

					foreach (var config in this.DatasetUpdaterConfigs)
					{
						if (config.Enabled)
						{
							Logger.LogTitle($"Updating Dataset {config.SourceDataset.Name}");
							await config.Apply(UseDebugParams).ConfigureAwait(false);
							Logger.LogTitle($"Updated Dataset {config.SourceDataset.Name}");
						}

					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return true;
		}


	}
}
