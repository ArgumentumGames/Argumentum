﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Argumentum.AssetConverter.DatasetUpdater;
using Argumentum.AssetConverter.Dnn2sxc;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using Argumentum.AssetConverter.Ontology;
using Argumentum.AssetConverter.Optimization;
using Argumentum.AssetConverter.Tests;
using Spectre.Console;
using Spectre.Console.Json;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;

namespace Argumentum.AssetConverter
{
    public class AssetConverterConfig
    {



		//Debug Switch to configure default values
	    public bool SkipConfigFile { get; set; } = false;

	    public ConverterMode Mode { get; set; } = ConverterMode.WebBasedImageGeneration | ConverterMode.Mindmapper | ConverterMode.OwlGenerator; // | ConverterMode.WebBasedImageGeneration; // ConverterMode.DatasetUpdater;

		public bool ForceDebugParams { get; set; }

		public bool ForceReleaseParams { get; set; }


		public string BaseTargetDirectoryName { get; set; } = @"Target\";

		public List<DataSetInfo> DataSets { get; set; } = new List<DataSetInfo>(
			new[]
			{
				new DataSetInfo()
				{
					Name = KnownDataSets.Rules,
					ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum%20Rules%20-%20Cards.csv",
					DebugFilePath = @"..\..\..\..\..\..\Cards\Rules\Argumentum Rules - Cards.csv"
				},
				new DataSetInfo()
				{
					Name = KnownDataSets.RulesPrintAndPlay,
					ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Rules/Argumentum%20Rules%20-%20Cards%20Print%20and%20Play.csv",
					DebugFilePath = @"..\..\..\..\..\..\Cards\Rules\Argumentum Rules - Cards Print and Play.csv"
				},
				new DataSetInfo()
				{
					Name = KnownDataSets.Scenarii,
					ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Scenarii/Argumentum%20Scenarii%20-%20Cards.csv",
					DebugFilePath = @"..\..\..\..\..\..\Cards\Scenarii\Argumentum Scenarii - Cards.csv"
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
					CsvType = typeof(ArgumentVirtue),
					ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Virtues%20-%20Taxonomy.csv",
					DebugFilePath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Virtues - Taxonomy.csv"
				}



			});

		public LocalizationConfig LocalizationConfig { get; set; } = new LocalizationConfig()
		{
			Enabled = true,
			CardSetLocalizations = new List<CardSetLocalization>(new[]{
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Fallacies,
						KnownCardSets.Fallacies2,
						KnownCardSets.Fallacies3,
						KnownCardSets.FallaciesPrintAndPlay,
						KnownCardSets.FallaciesWeb,
						KnownCardSets.FallaciesWebLight,
						KnownCardSets.FallaciesWebThumbnails,

					}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Soussousfamille", new List<(string Language, string destFieldName)>(new []{("en", "Subsubfamily"), ("ru", "Subsubfamily_ru"), ("pt", "Subsubfamily_pt") }) ),
						("Sous-Famille", new List<(string Language, string destFieldName)>(new []{("en", "Subfamily"), ("ru", "Subfamily_ru"), ("pt", "Subfamily_pt") }) ),
						("Famille", new List<(string Language, string destFieldName)>(new []{("en", "Family"), ("ru", "Family_ru"), ("pt", "Family_pt") }) ),
						("text_fr", new List<(string Language, string destFieldName)>(new []{("en", "text_en"), ("ru", "text_ru"), ("pt", "text_pt") }) ),
						("desc_fr", new List<(string Language, string destFieldName)>(new []{("en", "desc_en"), ("ru", "desc_ru"), ("pt", "desc_pt") }) ),
						("example_fr", new List<(string Language, string destFieldName)>(new []{("en", "example_en"), ("ru", "example_ru"), ("pt", "example_pt") }) ),
					}),
					BackFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("tagline_fr", new List<(string Language, string destFieldName)>(new []{("en", "tagline_en"), ("ru", "tagline_ru"), ("pt", "tagline_pt") }) ),
					})
				},
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Scenarii,
						KnownCardSets.ScenariiPrintAndPlay,

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
				},
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Rules,
						KnownCardSets.RulesPrintAndPlay,

					}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Text", new List<(string Language, string destFieldName)>(new []{("en", "Text_en"), ("ru", "Text_ru"), ("pt", "Text_pt") }) ),
					})
				},
				new CardSetLocalization()
				{
					CardSetNames = new List<string>(new []
					{
						KnownCardSets.Memo,
						KnownCardSets.MemoPrintAndPlay,

					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new []{
							("L'art de jamais avoir tort", new List<(string Language, string destFieldName)>(new []{("en", "The art of never being wrong"), ("ru", "Искусство никогда не ошибаться"), ("pt", "A arte de nunca errar") }) ),
						}),
					FrontFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Famille", new List<(string Language, string destFieldName)>(new []{("en", "Family"), ("ru", "Family_ru"), ("pt", "Family_pt") }) ),
						("desc_fr", new List<(string Language, string destFieldName)>(new []{("en", "desc_en"), ("ru", "desc_ru"), ("pt", "desc_pt") }) ),
					}),
					BackFieldConversions = new List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)>(new []{
						("Soussousfamille", new List<(string Language, string destFieldName)>(new []{("en", "Subsubfamily"), ("ru", "Subsubfamily_ru"), ("pt", "Subsubfamily_pt") }) ),
						("Sous-Famille", new List<(string Language, string destFieldName)>(new []{("en", "Subfamily"), ("ru", "Subfamily_ru"), ("pt", "Subfamily_pt") }) ),
						("Famille", new List<(string Language, string destFieldName)>(new []{("en", "Family"), ("ru", "Family_ru"), ("pt", "Family_pt") }) ),
						("tagline_fr", new List<(string Language, string destFieldName)>(new []{("en", "tagline_en"), ("ru", "tagline_ru"), ("pt", "tagline_pt") }) ),
					})
				},
			}),
			MindMapLocalization = new List<DocumentLocalization>(new[]
{
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
						{
							nameof (MindMapDocumentConfig.DocumentName),
						}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						("_fr", new List<(string Language, string destFieldName)>(new []{("en", "_en"), ("ru", "_ru"), ("pt", "_pt") }) ),

					}),
				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof(MindMapDocumentConfig.TitleExpression),
						nameof (MindMapDocumentConfig.CardExpression),
						nameof (MindMapDocumentConfig.FamilleExpression),
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						(nameof(Fallacy.Famille), new List<(string Language, string destFieldName)>(new []{("en", nameof(Fallacy.Family)), ("ru", nameof(Fallacy.FamilyRu)), ("pt", nameof(Fallacy.FamilyPt)) }) ),

					}),
				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof(MindMapDocumentConfig.TitleExpression),
						nameof (MindMapDocumentConfig.CardExpression)
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						(nameof(Fallacy.SousFamille), new List<(string Language, string destFieldName)>(new []{("en", nameof(Fallacy.Subfamily)), ("ru", nameof(Fallacy.SubfamilyRu)), ("pt", nameof(Fallacy.SubfamilyPt)) }) ),

					}),
				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof(MindMapDocumentConfig.TitleExpression),
						nameof (MindMapDocumentConfig.CardExpression)
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						(nameof(Fallacy.Soussousfamille), new List<(string Language, string destFieldName)>(new []{("en", nameof(Fallacy.Subsubfamily)), ("ru", nameof(Fallacy.SubsubfamilyRu)), ("pt", nameof(Fallacy.SubsubfamilyPt)) }) ),

					}),
				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof(MindMapDocumentConfig.TitleExpression),
						nameof (MindMapDocumentConfig.CardExpression)
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						(nameof(Fallacy.TextFr), new List<(string Language, string destFieldName)>(new []{("en", "TextEn"), ("ru", "TextRu"), ("pt", "TextPt") }) ),

					}),
				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof (MindMapDocumentConfig.DescriptionExpression),
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						(nameof(Fallacy.DescFr), new List<(string Language, string destFieldName)>(new []{("en", nameof(Fallacy.DescEn)), ("ru", nameof(Fallacy.DescRu)), ("pt", nameof(Fallacy.DescPt)) }) )
					}),

				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof (MindMapDocumentConfig.ExampleExpression),
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						(nameof(Fallacy.ExampleFr), new List<(string Language, string destFieldName)>(new []{("en", nameof(Fallacy.ExampleEn)), ("ru", nameof(Fallacy.ExampleRu)), ("pt", nameof(Fallacy.ExamplePt)) }) ),
					}),

				},
				new DocumentLocalization(){
					TargetProperties = new List<string>(new []
					{
						nameof (MindMapDocumentConfig.LinkExpression),
					}),
					StaticConversions = new List<(string sourceText, List<(string Language, string destText)> textConversions)>(new[]{
						(nameof(Fallacy.LinkFrFallback), new List<(string Language, string destFieldName)>(new []{("en", nameof(Fallacy.LinkEnFallback)), ("ru", nameof(Fallacy.LinkRuFallback)), ("pt", nameof(Fallacy.LinkPtFallback)) }) ),

					})
				}
			}),
		};

		public BatchImageConverterConfig BatchImageConverterConfig { get; set; } = new BatchImageConverterConfig();


		public DatasetUpdaterRootConfig DatasetUpdaterRootConfig { get; set; } = new DatasetUpdaterRootConfig();


		public WebBasedGeneratorConfig WebBasedGeneratorConfig { get; set; } = new WebBasedGeneratorConfig();

		public MindMapCreatorConfig MindMapCreatorConfig { get; set; } = new MindMapCreatorConfig();


		public Dnn2sxcConfig Dnn2sxcConfig { get; set; } = new Dnn2sxcConfig();

public OwlGeneratorConfig OwlGeneratorConfig { get; set; } = new OwlGeneratorConfig();

public TaxonomyValidatorConfig TaxonomyValidatorConfig { get; set; } = new TaxonomyValidatorConfig();

public OwlValidatorConfig OwlValidatorConfig { get; set; } = new OwlValidatorConfig();

public CardValidatorConfig CardValidatorConfig { get; set; } = new CardValidatorConfig();

public ContinuousValidationConfig ContinuousValidationConfig { get; set; } = new ContinuousValidationConfig();

public TranslationCoverageConfig TranslationCoverageConfig { get; set; } = new TranslationCoverageConfig();

public ParallelismOptimizerConfig ParallelismOptimizerConfig { get; set; } = new ParallelismOptimizerConfig();

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
			toReturn = Path.Combine(toReturn, $"{language}\\");
			if (!Directory.Exists(toReturn))
			{
				Directory.CreateDirectory(toReturn);
			}

			return toReturn;
		}

		public bool OverwriteExistingDocs { get; set; }

		public bool OverwriteExistingHtmlMaps { get; set; }

		public bool EnableSVGPrompt { get; set; } = true;


		public bool AsynchronousPipeline { get; set; }




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

			if (toReturn.SkipConfigFile || new AssetConverterConfig().SkipConfigFile)
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
				    tasks.Add(Task.Run(() => MindMapCreatorConfig.Apply(this)));
			    }
			    else
			    {
				    await MindMapCreatorConfig.Apply(this);
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
