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

		public List<DataSetInfo> DataSets { get; set; } = new List<DataSetInfo>(new[]
		{
			new DataSetInfo()
			{
				Name = KnownDataSets.Rules,
				ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum%20Rules%20-%20Cards.csv",
				DebugFilePath = @"..\..\..\..\..\..\Cards\Rules\Argumentum Rules - Cards.csv",
				CsvType = typeof(Argumentum.AssetConverter.Entities.Rule)
			},
			new DataSetInfo()
			{
				Name = KnownDataSets.RulesPrintAndPlay,
				ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum%20Rules%20-%20Cards%20Print%20and%20Play.csv",
				DebugFilePath = @"..\..\..\..\..\..\Cards\Rules\Argumentum Rules - Cards Print and Play.csv",
				CsvType = typeof(Argumentum.AssetConverter.Entities.Rule)
			},
			new DataSetInfo()
			{
				Name = KnownDataSets.Scenarii,
				ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum%20Scenarii%20-%20Cards.csv",
				DebugFilePath = @"..\..\..\..\..\..\Cards\Scenarii\Argumentum Scenarii - Cards.csv",
				CsvType = typeof(Argumentum.AssetConverter.Entities.Scenario)
			},
			new DataSetInfo()
			{
				Name = KnownDataSets.FallaciesTaxonomy,
				CsvType = typeof(Fallacy),
				ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Fallacies%20-%20Taxonomy.csv",
				DebugFilePath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv"
			},
			new DataSetInfo()
			{
				Name = KnownDataSets.VirtuesTaxonomy,
				CsvType = typeof(Virtue),
				ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Virtues%20-%20Taxonomy.csv",
				DebugFilePath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Virtues - Taxonomy.csv"
			}
		});

		public LocalizationConfig LocalizationConfig { get; set; } = new LocalizationConfig()
		{
			CardSetLocalizations = new List<CardSetLocalization>(new[]
			{
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Fallacies,
						KnownCardSets.Fallacies2,
						KnownCardSets.Fallacies3,
						KnownCardSets.FallaciesPrintAndPlay,
						KnownCardSets.FallaciesWeb,
						KnownCardSets.FallaciesWebThumbnails
					}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Titre", new List<(string Language, string destFieldName)>(new []{("en", "Title"), ("ru", "Title_ru"), ("pt", "title_pt") }) ),
						("Famille", new List<(string Language, string destFieldName)>(new []{("en", "Family"), ("ru", "Family_ru"), ("pt", "family_pt") }) ),
						("Sous-Famille", new List<(string Language, string destFieldName)>(new []{("en", "Subfamily"), ("ru", "Subfamily_ru"), ("pt", "subfamily_pt") }) ),
						("soussousfamille", new List<(string Language, string destFieldName)>(new []{("en", "subsubfamily"), ("ru", "subsubfamily_ru"), ("pt", "subsubfamily_pt") }) ),
						("Definition", new List<(string Language, string destFieldName)>(new []{("en", "Definition_en"), ("ru", "Definition_ru"), ("pt", "Definition_pt") }) ),
						("Exemple", new List<(string Language, string destFieldName)>(new []{("en", "Example"), ("ru", "Example_ru"), ("pt", "Example_pt") }) ),
						("Contre-Exemple", new List<(string Language, string destFieldName)>(new []{("en", "Counterexample"), ("ru", "Counterexample_ru"), ("pt", "Counterexample_pt") }) )
					}),
				},
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Virtues,
					}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Nom", new List<(string Language, string destFieldName)>(new []{("en", "Name"), ("ru", "Name_ru"), ("pt", "Name_pt") }) ),
						("Description", new List<(string Language, string destFieldName)>(new []{("en", "Description_en"), ("ru", "Description_ru"), ("pt", "Description_pt") }) ),
					}),
				},
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Scenarii,
						KnownCardSets.ScenariiPrintAndPlay
					}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("catégorie", new List<(string Language, string destFieldName)>(new []{("en", "category"), ("ru", "category_ru"), ("pt", "category_pt") }) ),
						("titre", new List<(string Language, string destFieldName)>(new []{("en", "title"), ("ru", "title_ru"), ("pt", "title_pt") }) ),
						("contexte", new List<(string Language, string destFieldName)>(new []{("en", "context"), ("ru", "context_ru"), ("pt", "context_pt") }) ),
						("enjeu", new List<(string Language, string destFieldName)>(new []{("en", "issue"), ("ru", "issue_ru"), ("pt", "issue_pt") }) ),
						("piocheur", new List<(string Language, string destFieldName)>(new []{("en", "drawer"), ("ru", "drawer_ru"), ("pt", "drawer_pt") }) ),
						("baratineur", new List<(string Language, string destFieldName)>(new []{("en", "smoothTalker"), ("ru", "smoothTalker_ru"), ("pt", "smoothTalker_pt") }) ),
						("suggestion", new List<(string Language, string destFieldName)>(new []{("en", "suggestion_en"), ("ru", "suggestion_ru"), ("pt", "suggestion_pt") }) ),
					}),
					BackFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("catégorie", new List<(string Language, string destFieldName)>(new []{("en", "category"), ("ru", "category_ru"), ("pt", "category_pt") }) ),
					}),
					ExceptionPatterns = new List<string>(new []
					{
						"{{rowset.[0].catégorie}}.jpg",
						"{{rowset.[0].catégorie}}.png"
					})
				}
			}),
			MindMapLocalization = new List<DocumentLocalization>(new[]
			{
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof(FallacyMindMapDocumentConfig.TitleExpression),
						nameof (FallacyMindMapDocumentConfig.CardExpression)
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						(nameof(Fallacy.SousFamille), new List<(string Language, string destText)>(new []{("en", nameof(Fallacy.Subfamily)), ("ru", nameof(Fallacy.SubfamilyRu)), ("pt", nameof(Fallacy.SubfamilyPt)) }) )
					}),
				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof(FallacyMindMapDocumentConfig.TitleExpression),
						nameof (FallacyMindMapDocumentConfig.CardExpression)
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						(nameof(Fallacy.Soussousfamille), new List<(string Language, string destText)>(new []{("en", nameof(Fallacy.Subsubfamily)), ("ru", nameof(Fallacy.SubsubfamilyRu)), ("pt", nameof(Fallacy.SubsubfamilyPt)) }) )
					}),
				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof(VirtueMindMapDocumentConfig.TitleExpression)
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						("Vertus", new List<(string Language, string destText)>(new []{("en", "Virtues"), ("ru", "Dobrodeteli"), ("pt", "Virtudes") }) )
					}),
				}
			})
		};

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

			var defaultConfig = new AssetConverterConfig();
			if (defaultConfig.SkipConfigFile)
			{
				Logger.Log($"Config file skipped by default: {path}");
				toReturn = defaultConfig;
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
