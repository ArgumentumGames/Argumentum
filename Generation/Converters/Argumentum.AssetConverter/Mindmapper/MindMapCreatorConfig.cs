using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Argumentum.AssetConverter.Entities;
using AutoMapper;
using CsvHelper;
using ImageMagick;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;


namespace Argumentum.AssetConverter.Mindmapper
{
    public class MindMapCreatorConfig
    {

	     static MindMapCreatorConfig()
	    {
            //Attempt to fix missing Type/assembly.
		    var tempConverter = new TypeConverter();
	    }

		//public MindMapCreatorConfig()
		//{
		//    var newConfig = new List<MindMapDocumentConfig>();
		//    newConfig.Add(new MindMapDocumentConfig(){DocumentName = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm" });

		//    var frConfigCards = new MindMapDocumentConfig() { DocumentName = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2_cards.mm" };
		//    frConfigCards.InsertCardsThumbnails = true;

		//    newConfig.Add(frConfigCards);

		//    var enConfig = new MindMapDocumentConfig(){DocumentName = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm" };
		//    enConfig.DocumentName = enConfig.DocumentName.Replace("Fr", "En");
		//    enConfig.DescriptionExpression = enConfig.DescriptionExpression.Replace("Fr", "En");
		//    enConfig.TitleExpression = enConfig.TitleExpression.Replace("Fr", "En");
		//    enConfig.ExampleExpression = enConfig.ExampleExpression.Replace("Fr", "En");
		//    enConfig.LinkExpression = enConfig.LinkExpression.Replace("Fr", "En");
		//    newConfig.Add(enConfig);

		//    var enConfigCards = new MindMapDocumentConfig(){DocumentName = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm" };
		//    enConfigCards.InsertCardsThumbnails = true;
		//    enConfigCards.DocumentName = enConfig.DocumentName.Replace(".mm", "_cards.mm");
		//    enConfigCards.DescriptionExpression = enConfig.DescriptionExpression;
		//    enConfigCards.TitleExpression = enConfig.TitleExpression;
		//    enConfigCards.ExampleExpression = enConfig.ExampleExpression;
		//    enConfigCards.LinkExpression = enConfig.LinkExpression;
		//    enConfigCards.CardExpression = enConfig.CardExpression.Replace("Fr", "En");
		//    newConfig.Add(enConfigCards);

		//    MindMapDocuments = newConfig;
		//}


		//public void Run(string[] args)
		//{

		//    foreach (var config in this.MindMapDocuments)
		//    {

		//        var fallacies = Fallacy.Load(config.DataSet);
		//       config.GenerateMindMapFile( fallacies, null, "", "").GetAwaiter().GetResult();

		//    }
		//    Logger.LogTitle($"Generation finished, press any key to close");
		//    Console.ReadKey();

		//}

		public int MaxDegreeOfParallelismMindMaps { get; set; } = 1;



		public async Task Apply(AssetConverterConfig config)
		{
			Logger.LogTitle("Generating Freemind, SVG & Html Mindmaps");
			Logger.LogExplanations("In this last stage, Freemind mindmaps are generated from the same dataset that was used for cards pdfs. \nOptional Manual intervention is required for SVG processing. Once a Freemind mindmap is generated, you get prompted to use the free tool to generate an SVG file, which is then further processed for HTML generation. \nNote that Html files with the svg file embedded externally will only display properly when hosted behind a URL, whereas html documents with svg embedded inside will also display properly when opened locally");

	        var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelismMindMaps };

	        await Task.WhenAll(MindMapDocuments
		        .Where(config => config.Enabled)
		        .Select(mindMap => ProcessMindMapDocumentAsync(mindMap, config, parallelOptionsDocuments)));

        }

		private async Task ProcessMindMapDocumentAsync(MindMapDocumentConfig mindMap,
			AssetConverterConfig assetConverterConfig, ParallelOptions parallelOptions)
		{
			
				IList<Fallacy> fallacies;
				var dataSet = assetConverterConfig.DataSets.FirstOrDefault(ds => ds.Name == mindMap.DataSet);
				if (dataSet == null)
				{
					fallacies = Fallacy.Load(mindMap.DataSet);
				}
				else
				{
					if (dataSet.CsvType != null && dataSet.CsvType != typeof(Fallacy))
					{

						if (IsSubclassOfRawGeneric(typeof(CsvBase<,>), dataSet.CsvType))
						{

							var config = new MapperConfiguration(cfg =>
							{
								cfg.AddProfile<MappingProfile>();

							});
							var mapper = config.CreateMapper();

							// Trouver la m�thode LoadAsync sur le type sp�cifique
							var loadAsyncMethod = dataSet.CsvType.GetMethod("LoadAsync", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);


							// Invoquer la m�thode
							var task = (Task)loadAsyncMethod.Invoke(null, new object[] { dataSet, assetConverterConfig.UseDebugParams });

							// Attendre la fin de la t�che et obtenir le r�sultat
							await task.ConfigureAwait(false);
							var result = (IEnumerable)task.GetType().GetProperty("Result")?.GetValue(task, null);

							// Mapper les objets � Fallacy
							fallacies = (from object baseObject in result select mapper.Map<Fallacy>(baseObject)).ToList();

						}
						else
						{
							throw new InvalidOperationException(
								$"Dataset type {dataSet.CsvType.AssemblyQualifiedName} is incompatible with mindmap generation");
						}

				}
				else
				{
					fallacies = await Fallacy.LoadAsync(dataSet, assetConverterConfig.UseDebugParams);
				}

			}

			var targetLanguages = assetConverterConfig.LocalizationConfig.BuildLanguageList(mindMap.Translations);
			await Parallel.ForEachAsync(targetLanguages, parallelOptions, async (targetLanguage, token) =>
			{
				try
				{
					var currentTranslatedMap = mindMap.CloneMindMap();
					foreach (var documentLocalization in assetConverterConfig.LocalizationConfig.MindMapLocalization)
					{
						documentLocalization.DoReflectionTranslate(currentTranslatedMap, targetLanguage);
					}

					var documentDirectory = assetConverterConfig.GetDocumentDirectory(targetLanguage);

					await currentTranslatedMap.GenerateMindMapFile(fallacies, assetConverterConfig, documentDirectory, targetLanguage);
				}
				catch (Exception e)
				{
					Logger.LogException(e);
				}
			});
		



	}

	public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
		{
			while (toCheck != null && toCheck != typeof(object))
			{
				var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if (generic == cur)
				{
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}


		public List<MindMapDocumentConfig> MindMapDocuments { get; set; } = new List<MindMapDocumentConfig>(new[]
			{
				new MindMapDocumentConfig()
				{
					Enabled = true,
					DocumentName = "Argumentum_Fallacies_MindMap_fr.mm",
					DataSet = KnownDataSets.FallaciesTaxonomy,
					Translations = new List<(string sourceLang, string destLang)>(new []
					{
						("fr","en"),
						("fr", "ru"),
						("fr", "pt")
					}),
					ImageFormat = MagickFormat.Png,
					TargetDensity = 0,
					KeepOriginalSVG = false,
					SVGMaps = new List<SVGFreemindMap>(new []
					{
						new SVGFreemindMap()
						{
							Enabled = true,
							DocumentName = "links.svg",
							WrapNodeByLink = true,
							SetSVGNodeAttributes = false,
							RemoveImages = true
						},
						new SVGFreemindMap()
						{
							Enabled = true,
							DocumentName = "content.svg",
							WrapNodeByLink = false,
							SetSVGNodeAttributes = true,
							RemoveImages = true,
							HtmlWrappers = new List<DocumentConfig>(new []
							{
								new DocumentConfig()
								{
									TemplatePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Mindmaps/included.html",
									TemplatePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Mindmaps\included.html"
								},
								new DocumentConfig()
								{
									TemplatePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Mindmaps/external.html",
									TemplatePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Mindmaps\external.html"
								},

							})


						},
					})
				},
				new MindMapDocumentConfig()
				{
					Enabled = false,
					DocumentName = "Argumentum_Fallacies_MindMap_cards_fr.mm",
					DataSet = KnownDataSets.FallaciesTaxonomy,
					InsertCardsThumbnails = true,
					ThumbnailsCardSetName = KnownCardSets.FallaciesWebThumbnails,
					Translations = new List<(string sourceLang, string destLang)>(new []
					{
						("fr","en"),
						("fr", "ru"),
						("fr", "pt")
					}),
					ImageFormat = MagickFormat.Png,
					TargetDensity = 0,
					SVGMaps = new List<SVGFreemindMap>(new []
					{
						new SVGFreemindMap()
						{
							Enabled = true,
							DocumentName = "links.svg",
							WrapNodeByLink = true,
							SetSVGNodeAttributes = false,
						},
					})
				},
				new MindMapDocumentConfig()
				{
					Enabled = true,
					DocumentName = "Argumentum_Virtues_MindMap_fr.mm",
					DataSet = KnownDataSets.VirtuesTaxonomy,
					Translations = new List<(string sourceLang, string destLang)>(),
					ImageFormat = MagickFormat.Png,
					TargetDensity = 0,
					KeepOriginalSVG = false,
					NbBranchesRight = 4,
					SVGMaps = new List<SVGFreemindMap>(new []
					{
						new SVGFreemindMap()
						{
							Enabled = true,
							DocumentName = "links.svg",
							WrapNodeByLink = true,
							SetSVGNodeAttributes = false,
							RemoveImages = true
						},
						new SVGFreemindMap()
						{
							Enabled = true,
							DocumentName = "content.svg",
							WrapNodeByLink = false,
							SetSVGNodeAttributes = true,
							RemoveImages = true,
							HtmlWrappers = new List<DocumentConfig>(new []
							{
								new DocumentConfig()
								{
									TemplatePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Mindmaps/included.html",
									TemplatePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Mindmaps\included.html"
								},
								new DocumentConfig()
								{
									TemplatePathRelease = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Mindmaps/external.html",
									TemplatePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Mindmaps\external.html"
								},

							})
						},
					})
				}
			}
			);
	}
}
