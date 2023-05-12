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

		public WebBasedGenerator(WebBasedGeneratorConfig config, Stopwatch objSw)
		{
			Config = config;
			Stopwatch = objSw;

		}


		public async Task Run()
		{

			var harvestManager = new HarvestManager() { Stopwatch = Stopwatch, Config = Config };
			var harvestDictionary = Task.Run(async () => await harvestManager.HarvestImages()).Result;

			var imageManager = new ImageFileGenerator(Config, Stopwatch);
			var docImages = imageManager.GenerateDocumentImages(harvestDictionary);
			GenerateCardSetDocuments(docImages);
			await GenerateMindMapDocuments();
		}





		/// <summary>
		/// Generates PDF documents for a given card set document configuration and language.
		/// </summary>
		/// <param name="docImages">The card images to generate the documents from.</param>
		private void GenerateCardSetDocuments(ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>> docImages)
		{
			AnsiConsole.WriteLine();
			var rule = new Rule("[red]Generating pdf documents[/]");
			AnsiConsole.Write(rule);
			AnsiConsole.WriteLine();

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

					Logger.Log("Start Generating pdf document {baseName}");

					var objPdfManager = new PdfManager() { Stopwatch = Stopwatch };

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
					AnsiConsole.WriteException(e);
				}
			});
		}

		





		/// <summary>
		/// Generates MindMap documents from the given configuration.
		/// </summary>
		private async Task GenerateMindMapDocuments()
		{
			

			foreach (var mindMap in Config.MindMapDocuments.Where(config => config.Enabled))
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

						var documentPath = Path.Combine(documentDirectory, currentTranslatedMap.DocumentName);


						if (File.Exists(documentPath) && !Config.OverwriteExistingDocs)
						{
							//imageFromEmbeddedUrl = new MagickImage(imageFileName);

							Logger.Log("Skip existing Mindmap: {documentPath}");
							
						}
						else
						{
							Logger.Log("Creating Freemind mind map {currentTranslatedMap.DocumentName}");
							await currentTranslatedMap.GenerateMindMapFile(fallacies, Config, documentDirectory, targetLanguage);
						}
						
					}
				}
				catch (Exception e)
				{
					AnsiConsole.WriteException(e);
				}


			}
		}

	}
}