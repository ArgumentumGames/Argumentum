using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Http;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using AutoMapper;
using ExtendedXmlSerializer.Core.Sources;
using ImageMagick;
using Spectre.Console;
using Sprache;
using Rule = Spectre.Console.Rule;


namespace Argumentum.AssetConverter
{
    public class WebBasedGenerator
	{

		

		public WebBasedGeneratorConfig Config { get; set; }

		public WebBasedGenerator(WebBasedGeneratorConfig config)
		{
			Config = config;

		}


		public async Task Run()
		{
			var tempLogSwitch = Logger.LogInfo;
			if (!Config.ShowInfoLogs)
			{
				Logger.LogInfo = false;
			}

			var harvestManager = new HarvestManager() { Config = Config };
			var harvestDictionary = await harvestManager.HarvestImages();

			var imageManager = new ImageFileGenerator(Config);
			var docImages = imageManager.GenerateDocumentImages(harvestDictionary);
			GenerateCardSetDocuments(docImages);
			 await GenerateMindMapDocuments();
			Logger.LogInfo = tempLogSwitch;
		}





		/// <summary>
		/// Generates PDF documents for a given card set document configuration and language.
		/// </summary>
		/// <param name="docImages">The card images to generate the documents from.</param>
		private void GenerateCardSetDocuments(ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>> docImages)
		{
			Logger.LogTitle("Generating pdf documents");

			Logger.LogExplanations("In this third stage, Pdf documents are compiled from the individual image files. \nThose are essentially Print&Play documents for individual printers, professional services printing formats, and various posters");

			var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = Config.MaxDegreeOfParallelismDocuments };

			Parallel.ForEach(docImages, parallelOptionsDocuments, docImageList =>
			{
				try
				{
					var pdfDirectory = Config.GetDocumentDirectory(docImageList.Key.language);
					var densityDirectory = docImageList.Key.document.GetDensityDirectory(pdfDirectory);

					var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();

					var documentName = CardSetLocalization.GetLocalizedFileName(docImageList.Key.document.DocumentName,
						Config.LocalizationConfig.DefaultLanguage, docImageList.Key.language);
					var baseName = Path.Combine(densityDirectory, documentName);

					Logger.Log($"Start Generating pdf document {baseName}");

					var objPdfManager = new PdfManager() ;

					switch (docImageList.Key.document.DocumentFormat)
					{
						case CardDocumentFormat.AlternateFaceAndBack:
							objPdfManager.GenerateAlternateFaceAndBack( baseName, docImageList.Value, Config.OverwriteExistingDocs);
							break;
						case CardDocumentFormat.BackFirstOneDocPerBack:
							objPdfManager.GenerateBackFirstOneDocPerBack( baseName, docImageList.Value, Config.OverwriteExistingDocs);
							break;
						case CardDocumentFormat.PrintAndPlay:
							objPdfManager.GeneratePrintAndPlay(baseName, docImageList.Key.document, docImageList.Value, Config.OverwriteExistingDocs);
							break;
						default:
							throw new InvalidOperationException($"Document format {docImageList.Key.document.DocumentFormat} unsupported");
					}
				}
				catch (Exception e)
				{
					Logger.LogException(e);
				}
			});
		}




		private async Task GenerateMindMapDocuments()
		{
			Logger.LogTitle("Generating Freemind, SVG & Html Mindmaps");
			Logger.LogExplanations("In this last stage, Freemind mindmaps are generated from the same dataset that was used for cards pdfs. \nOptional Manual intervention is required for SVG processing. Once a Freemind mindmap is generated, you get prompted to use the free tool to generate an SVG file, which is then further processed for HTML generation. \nNote that Html files with the svg file embedded externally will only display properly when hosted behind a URL, whereas html documents with svg embedded inside will also display properly when opened locally");

			var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = Config.MaxDegreeOfParallelismMindMaps };

			await Task.WhenAll(Config.MindMapDocuments
				.Where(config => config.Enabled)
				.Select(mindMap => ProcessMindMapDocumentAsync(mindMap, parallelOptionsDocuments)));

		}

		private async Task ProcessMindMapDocumentAsync(MindMapDocumentConfig mindMap, ParallelOptions parallelOptions)
		{
			IList<Fallacy> fallacies;
			var dataSet = Config.DataSets.FirstOrDefault(ds => ds.Name == mindMap.DataSet);
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
						var task = (Task)loadAsyncMethod.Invoke(null, new object[] { dataSet, Config.UseDebugParams() });

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
					fallacies = await Fallacy.LoadAsync(dataSet, Config.UseDebugParams());
				}
				
			}

			var targetLanguages = Config.LocalizationConfig.BuildLanguageList(mindMap.Translations);
			await Parallel.ForEachAsync(targetLanguages, parallelOptions, async (targetLanguage, token) =>
			{
				var currentTranslatedMap = mindMap.CloneMindMap();
				foreach (var documentLocalization in Config.LocalizationConfig.MindMapLocalization)
				{
					documentLocalization.DoReflectionTranslate(currentTranslatedMap, targetLanguage);
				}

				var documentDirectory = Config.GetDocumentDirectory(targetLanguage);

				await currentTranslatedMap.GenerateMindMapFile(fallacies, Config, documentDirectory, targetLanguage);
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



	}
}