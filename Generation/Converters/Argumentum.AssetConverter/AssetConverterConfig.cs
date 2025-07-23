using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Argumentum.AssetConverter.DatasetUpdater;
using Argumentum.AssetConverter.Dnn2sxc;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using Argumentum.AssetConverter.Ontology;
using Argumentum.AssetConverter.Optimization;
using Argumentum.AssetConverter.PdfAuditor;
using Argumentum.AssetConverter.Tests;
using Spectre.Console;
using Spectre.Console.Json;

namespace Argumentum.AssetConverter
{
    public class AssetConverterConfig
    {



		//Debug Switch to configure default values
	    public bool SkipConfigFile { get; set; } = false;

	       [JsonConverter(typeof(JsonStringEnumConverter))]
	       public ConverterMode Mode { get; set; } = ConverterMode.WebBasedImageGeneration | ConverterMode.QuestPdfGeneration;

		public bool ForceDebugParams { get; set; }

		public bool ForceReleaseParams { get; set; }


		public string BaseTargetDirectoryName { get; set; } = @"Target\";

		public List<DataSetInfo> DataSets { get; set; } = new List<DataSetInfo>();

		public LocalizationConfig LocalizationConfig { get; set; } = new LocalizationConfig();

		public BatchImageConverterConfig BatchImageConverterConfig { get; set; } = new BatchImageConverterConfig();


		public DatasetUpdaterRootConfig DatasetUpdaterRootConfig { get; set; } = new DatasetUpdaterRootConfig();


		public WebBasedGeneratorConfig WebBasedGeneratorConfig { get; set; } = new WebBasedGeneratorConfig();

		public FallacyMindMapCreatorConfig FallacyMindMapCreatorConfig { get; set; } = new FallacyMindMapCreatorConfig();
		public VirtueMindMapCreatorConfig VirtueMindMapCreatorConfig { get; set; } = new VirtueMindMapCreatorConfig();

		public string FreeplanePath { get; set; } = @"C:\Program Files (x86)\Freeplane\freeplane.bat";


		public Dnn2sxcConfig Dnn2sxcConfig { get; set; } = new Dnn2sxcConfig();

public OwlGeneratorConfig OwlGeneratorConfig { get; set; } = new OwlGeneratorConfig();

public TaxonomyValidatorConfig TaxonomyValidatorConfig { get; set; } = new TaxonomyValidatorConfig();

public OwlValidatorConfig OwlValidatorConfig { get; set; } = new OwlValidatorConfig();

public CardValidatorConfig CardValidatorConfig { get; set; } = new CardValidatorConfig();

public ContinuousValidationConfig ContinuousValidationConfig { get; set; } = new ContinuousValidationConfig();

public TranslationCoverageConfig TranslationCoverageConfig { get; set; } = new TranslationCoverageConfig();

public ParallelismOptimizerConfig ParallelismOptimizerConfig { get; set; } = new ParallelismOptimizerConfig();

public PdfAuditorConfig PdfAuditorConfig { get; set; } = new PdfAuditorConfig();

public string DocumentsDirectoryName { get; set; } = @"Documents\";



		public string HarvestDirectoryName { get; set; } = @"Harvest\";


		public string ImagesDirectoryName { get; set; } = @"Images\";

		public string GetDocumentDirectory(string language)
		{
			var toReturn = Path.Combine(GetBaseTargetDirectory(language), DocumentsDirectoryName);
			if (!Directory.Exists(toReturn))
			{
				Directory.CreateDirectory(toReturn);
			}

			return toReturn;
		}

		public string GetBaseTargetDirectory(string language)
		{
			var toReturn = Path.Combine(System.Environment.CurrentDirectory, BaseTargetDirectoryName);
			if (!Directory.Exists(toReturn))
			{
				Directory.CreateDirectory(toReturn);
			}
			if (!string.IsNullOrEmpty(language))
			{
				toReturn = Path.Combine(toReturn, $"{language}\\");
				if (!Directory.Exists(toReturn))
				{
					Directory.CreateDirectory(toReturn);
				}
			}

			return toReturn;
		}

		public bool OverwriteExistingDocs { get; set; } = true;

		public bool OverwriteExistingHtmlMaps { get; set; }

		public bool EnableSVGPrompt { get; set; } = true;


		public bool AsynchronousPipeline { get; set; }




		public static AssetConverterConfig GetConfig(string path, out bool newConfig)
		{
			AssetConverterConfig toReturn;
			newConfig = false;
			if (!File.Exists(path))
			{
				toReturn = new AssetConverterConfig();
				var options = new JsonSerializerOptions { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };
				var strNewConfig = System.Text.Json.JsonSerializer.Serialize(toReturn, options);

				File.WriteAllText(path, strNewConfig);
				newConfig = true;

				//Logger.LogJson(strNewConfig);
				Logger.LogSuccess($"Config file created: {path}");
			}

			var jsonString = File.ReadAllText(path);
			var serializerOptions = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				Converters = { new JsonStringEnumConverter() }
			};
			toReturn = System.Text.Json.JsonSerializer.Deserialize<AssetConverterConfig>(jsonString, serializerOptions);

			if (toReturn.SkipConfigFile)
			{
				Logger.Log($"Config loaded and skipped: {path}");
				toReturn = new AssetConverterConfig();
			}
			else
			{
				Logger.Log($"Config loaded: {path}");
			}
			
			if (toReturn.WebBasedGeneratorConfig?.CardSets != null)
			{
				var cardSetNames = toReturn.WebBasedGeneratorConfig.CardSets.Select(cs => cs.Name);
				Logger.Log($"Loaded {cardSetNames.Count()} card sets: {string.Join(", ", cardSetNames)}");
			}

			return toReturn;
        }


        [IgnoreDataMember]
        [JsonIgnore]
        public bool UseDebugParams => (isInDebugMode || ForceDebugParams) && !ForceReleaseParams;


        [IgnoreDataMember]
        [JsonIgnore]
        public bool UseReleaseParams => (!isInDebugMode && !ForceDebugParams) || ForceReleaseParams;

#if DEBUG
		bool isInDebugMode = true;
#else
		bool isInDebugMode = false;
#endif

	    public async Task<bool> Apply()
	    {

		    List<Task> tasks = new List<Task>();


			if (Mode.HasFlag(ConverterMode.BatchImageProcessor))
		    {
			    if (AsynchronousPipeline)
			    {
				    tasks.Add(Task.Run(() => BatchImageConverterConfig.Apply()));
				}
			    else
			    {
				    await BatchImageConverterConfig.Apply();
			    }

			    

		    }

			if (Mode.HasFlag(ConverterMode.DatasetUpdater))
			{
				if (AsynchronousPipeline)
				{
					tasks.Add(Task.Run(() => DatasetUpdaterRootConfig.Apply(this)));
				}
				else
				{
					await DatasetUpdaterRootConfig.Apply(this);
				}


			}

			if (Mode.HasFlag(ConverterMode.WebBasedImageGeneration))
		    {
			    if (AsynchronousPipeline)
			    {
				    tasks.Add(Task.Run(() => WebBasedGeneratorConfig.Apply(this)));
			    }
			    else
			    {
				    await WebBasedGeneratorConfig.Apply(this);
			    }



			}
			if (Mode.HasFlag(ConverterMode.Mindmapper))
		    {
			    if (AsynchronousPipeline)
			    {
				    tasks.Add(Task.Run(() => FallacyMindMapCreatorConfig.Apply(this)));
				    tasks.Add(Task.Run(() => VirtueMindMapCreatorConfig.Apply(this)));
				   }
				   else
				   {
				    await FallacyMindMapCreatorConfig.Apply(this);
				    await VirtueMindMapCreatorConfig.Apply(this);
				   }


			}


			if (Mode.HasFlag(ConverterMode.Dnn2sxc))
		    {
				if (AsynchronousPipeline)
				{
					tasks.Add(Task.Run(() => Dnn2sxcConfig.Apply()));
				}
				else
				{
					Dnn2sxcConfig.Apply();
				}

				
		    }

			if (Mode.HasFlag(ConverterMode.OwlGenerator))
			{
				if (AsynchronousPipeline)
				{
					tasks.Add(Task.Run(() => OwlGeneratorConfig.Apply(this)));
				}
				else
				{
					await OwlGeneratorConfig.Apply(this);
				}
			}

			if (Mode.HasFlag(ConverterMode.TaxonomyValidator))
			{
				if (AsynchronousPipeline)
				{
					tasks.Add(Task.Run(() => TaxonomyValidatorConfig.Apply(this)));
				}
				else
				{
					await TaxonomyValidatorConfig.Apply(this);
				}
			}

			if (Mode.HasFlag(ConverterMode.OwlValidator))
			{
				if (AsynchronousPipeline)
				{
					tasks.Add(Task.Run(() => OwlValidatorConfig.Apply(this)));
				}
				else
				{
					await OwlValidatorConfig.Apply(this);
				}
			}

			if (Mode.HasFlag(ConverterMode.CardValidator))
			{
				if (AsynchronousPipeline)
				{
					tasks.Add(Task.Run(() => CardValidatorConfig.Apply(this)));
				}
				else
				{
					await CardValidatorConfig.Apply(this);
				}
			}

			if (Mode.HasFlag(ConverterMode.ContinuousValidator))
			{
				if (AsynchronousPipeline)
				{
					tasks.Add(Task.Run(() => ContinuousValidationConfig.Apply(this)));
				}
				else
				{
					await ContinuousValidationConfig.Apply(this);
				}
			}

			if (Mode.HasFlag(ConverterMode.TranslationCoverage))
			{
				if (AsynchronousPipeline)
				{
					tasks.Add(Task.Run(() => TranslationCoverageConfig.Apply(this)));
				}
				else
				{
					await TranslationCoverageConfig.Apply(this);
				}
			}

			if (Mode.HasFlag(ConverterMode.ParallelismOptimizer))
			{
				if (AsynchronousPipeline)
				{
					tasks.Add(Task.Run(() => ParallelismOptimizerConfig.Apply(this)));
				}
				else
				{
					await ParallelismOptimizerConfig.Apply(this);
				}
			}

			if (Mode.HasFlag(ConverterMode.PdfAuditor))
			{
				if (AsynchronousPipeline)
				{
					tasks.Add(Task.Run(() => PdfAuditorConfig.Apply(this)));
				}
				else
				{
					await PdfAuditorConfig.Apply(this);
				}
			}

			if (AsynchronousPipeline)
			{
				await Task.WhenAll(tasks);
			}



			// Handling for None or unrecognized values
			if (Mode == ConverterMode.None)
		    {
				// Handle None case
				Logger.LogTitle($"No action was planned in the config file.");
			}

		    return true;
	    }

	   

	    public string GetHarvestDirectory(string language)
	    {
		    var toReturn = Path.Combine(GetBaseTargetDirectory(language), HarvestDirectoryName);
		    if (!Directory.Exists(toReturn))
		    {
			    Directory.CreateDirectory(toReturn);
		    }

		    return toReturn;
	    }

	    public string GetImagesDirectory(string language)
	    {
		    var toReturn = Path.Combine(GetBaseTargetDirectory(language), ImagesDirectoryName);
		    if (!Directory.Exists(toReturn))
		    {
			    Directory.CreateDirectory(toReturn);
		    }

		    return toReturn;
	    }

	}
}
