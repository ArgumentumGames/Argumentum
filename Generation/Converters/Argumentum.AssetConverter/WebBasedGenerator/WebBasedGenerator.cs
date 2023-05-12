using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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

			Logger.LogExplanations("In this third stage, Pdf documents are compiled from the individual image files. Those are essentially Print&Play documents for individual printers, professional services printing formats, and various posters");

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
							throw new ArgumentOutOfRangeException();
					}
				}
				catch (Exception e)
				{
					Logger.LogException(e);
				}
			});
		}

		





		/// <summary>
		/// Generates MindMap documents from the given configuration.
		/// </summary>
		private async Task GenerateMindMapDocuments()
		{

			Logger.LogTitle("Generating Freemind, SVG & Html Mindmaps");

			Logger.LogExplanations("In this last stage, Freemind mindmaps are generated from the same dataset that was used for cards pdfs. Optional Manual intervention is required for SVG processing. Once a Freemind mindmap is generated, you get prompted to use the free tool to generate a svg file, which is then further processed for Html generation");

			var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = Config.MaxDegreeOfParallelismDocuments };

			await Parallel.ForEachAsync(Config.MindMapDocuments.Where(config => config.Enabled), parallelOptionsDocuments,
				async (mindMap, token) =>
				{
					try
					{

						IList<Fallacy> fallacies;
						var dataSet = Config.DataSets.FirstOrDefault(ds => ds.Name == mindMap.DataSet, null);
						if (dataSet == null)
						{
							fallacies = Fallacy.LoadFallacies(mindMap.DataSet);
						}
						else
						{
							fallacies = await Fallacy.LoadFallaciesAsync(dataSet, Config.UseDebugParams);
						}

						var targetLanguages = Config.LocalizationConfig.BuildLanguageList(mindMap.Translations);
						foreach (var targetLanguage in targetLanguages)
						{

							var currentTranslatedMap = mindMap.CloneMindMap();
							foreach (var documentLocalization in Config.LocalizationConfig.MindMapLocalization)
							{
								documentLocalization.DoReflectionTranslate(currentTranslatedMap, targetLanguage);
							}

							var documentDirectory = Config.GetDocumentDirectory(targetLanguage);



							//Task.Run(async () => await currentTranslatedMap.GenerateMindMapFile(fallacies, Config, documentDirectory,
							//	targetLanguage)).GetAwaiter().GetResult();
							//Logger.LogTitle("coucou currentTranslatedMap");
							await currentTranslatedMap.GenerateMindMapFile(fallacies, Config, documentDirectory,
								targetLanguage);

						}
					}
					catch (Exception e)
					{
						Logger.LogException(e);
					}


				});
			//Logger.LogTitle("coucou Parallel");
		}

	}
}