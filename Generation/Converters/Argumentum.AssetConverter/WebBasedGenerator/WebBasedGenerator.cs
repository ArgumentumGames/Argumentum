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

		public static Stopwatch Stopwatch = Stopwatch.StartNew();

		public WebBasedGeneratorConfig Config { get; set; }

		public WebBasedGenerator(WebBasedGeneratorConfig config, Stopwatch objSw)
		{
			Config = config;
			Stopwatch = objSw;

		}


		public void Run()
		{

			var harvestManager = new HarvestManager() { Stopwatch = Stopwatch, Config = Config };
			var harvestDictionary = Task.Run(async () => await harvestManager.HarvestImages()).Result;

			var imageManager = new ImageFileGenerator(Config, Stopwatch);
			var docImages = imageManager.GenerateDocumentImages(harvestDictionary);
			GenerateCardSetDocuments(docImages);
			GenerateMindMapDocuments();
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
					var densityDirectory = CreateDensityDirectory(pdfDirectory, docImageList.Key.document.TargetDensity);

					var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();

					var documentName = CardSetLocalization.GetLocalizedFileName(docImageList.Key.document.DocumentName,
						Config.LocalizationConfig.DefaultLanguage, docImageList.Key.language);
					var baseName = Path.Combine(densityDirectory, documentName);

					Console.WriteLine($"{Stopwatch.Elapsed}: Start Generating pdf document {baseName}");

					var objPdfManager = new PdfManager() { Stopwatch = Stopwatch };

					switch (docImageList.Key.document.DocumentFormat)
					{
						case CardDocumentFormat.AlternateFaceAndBack:
							GenerateAlternateFaceAndBack(objPdfManager, baseName, docImageList.Value, Config.OverwriteExistingDocs);
							break;
						case CardDocumentFormat.BackFirstOneDocPerBack:
							GenerateBackFirstOneDocPerBack(objPdfManager, baseName, docImageList.Value, Config.OverwriteExistingDocs);
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

		private string CreateDensityDirectory(string pdfDirectory, int targetDensity)
		{
			var densityDirectory = Path.Combine(pdfDirectory, $@".\density-{targetDensity}\");
			if (!Directory.Exists(densityDirectory))
			{
				Directory.CreateDirectory(densityDirectory);
			}
			return densityDirectory;
		}

		private void GenerateAlternateFaceAndBack(PdfManager objPdfManager, string baseName, List<CardImages> cardImages, bool overwriteExistingDocs)
		{
			var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();
			var collecBuilderAFB = () =>
			{
				var collec = new MagickImageCollection(cardImages.SelectMany(s =>
				{
					return new[] { new MagickImage(s.Front), new MagickImage(s.Back) };
				}));
				return collec;
			};

			targetFiles.Add((baseName, collecBuilderAFB));
			objPdfManager.GeneratePdfsFromImages(targetFiles, overwriteExistingDocs);
		}

		private void GenerateBackFirstOneDocPerBack(PdfManager objPdfManager, string baseName, List<CardImages> cardImages, bool overwriteExistingDocs)
		{
			var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();
			var indexInsert = baseName.LastIndexOf('.');
			var cardsPerBack = cardImages.GroupBy(card => card.Back).ToArray();
			for (int backIndex = 0; backIndex < cardsPerBack.Count(); backIndex++)
			{
				var closureBackIndex = backIndex;
				var collecBuilderBF = () =>
				{
					var frontsAndBack = cardsPerBack[closureBackIndex];
					var backThenFronts = new[] { new MagickImage(frontsAndBack.Key) }.Concat(
						frontsAndBack.Select(card => new MagickImage(card.Front)));
					var collec = new MagickImageCollection(backThenFronts);
					return collec;
				};

				var newName =
					$"{baseName.Substring(0, indexInsert)}-{backIndex + 1}{baseName.Substring(indexInsert)}";
				targetFiles.Add((newName, collecBuilderBF));
			}

			objPdfManager.GeneratePdfsFromImages(targetFiles, overwriteExistingDocs);
		}





		/// <summary>
		/// Generates MindMap documents from the given configuration.
		/// </summary>
		private void GenerateMindMapDocuments()
		{
			AnsiConsole.WriteLine();
			var rule = new Rule("[red]Generating pdf documents[/]");
			AnsiConsole.Write(rule);
			AnsiConsole.WriteLine();

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
						fallacies = Fallacy.LoadFallaciesAsync(dataSet, Config.UseDebugParams).GetAwaiter().GetResult();
					}

					var targetLanguages = Config.LocalizationConfig.BuildLanguageList(mindMap.Translations);
					foreach (var targetLanguage in targetLanguages)
					{
						var currentTranslatedMap = mindMap.CloneMindMap();
						foreach (var documentLocalization in Config.LocalizationConfig.MindMapLocalization)
						{
							documentLocalization.DoReflectionTranslate(currentTranslatedMap, targetLanguage);
						}

						if (File.Exists(currentTranslatedMap.DocumentName) && !Config.OverwriteExistingDocs)
						{
							//imageFromEmbeddedUrl = new MagickImage(imageFileName);

							Console.WriteLine($"{Stopwatch.Elapsed}: Skip existing Mindmap: {currentTranslatedMap.DocumentName}");
							
						}
						else
						{
							Console.WriteLine($"{Stopwatch.Elapsed}: Creating Freemind mind map {currentTranslatedMap.DocumentName}");
							currentTranslatedMap.GenerateMindMapFile(fallacies, Config, Config.GetDocumentDirectory(targetLanguage), targetLanguage);
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