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
using Sprache;


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
		/// Generates PDF documents from a ConcurrentDictionary of CardSetGenerationDocument and language, and a list of CardImages.
		/// </summary>
		private void GenerateCardSetDocuments(ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>> docImages)
		{
			Console.WriteLine($"{Stopwatch.Elapsed}: Generation pdf documents");

			var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = Config.MaxDegreeOfParallelismDocuments };

			Parallel.ForEach(docImages, parallelOptionsDocuments, docImageList =>
			{
				try
				{
					var pdfDirectory = Config.GetPdfsDirectory(docImageList.Key.language);
					var densityDirectory = Path.Combine(pdfDirectory,
						$@".\density-{docImageList.Key.document.TargetDensity}\");
					if (!Directory.Exists(densityDirectory))
					{
						Directory.CreateDirectory(densityDirectory);
					}

					var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();
					//MagickImageCollection collec;
					var documentName = CardSetLocalization.GetLocalizedFileName(docImageList.Key.document.DocumentName,
						Config.LocalizationConfig.DefaultLanguage, docImageList.Key.language);
					var baseName = Path.Combine(densityDirectory, documentName);
					
					Console.WriteLine($"{Stopwatch.Elapsed}: Start Generating pdf document {baseName}");

					var objPdfManager = new PdfManager() { Stopwatch = Stopwatch };
					switch (docImageList.Key.document.DocumentFormat)
					{
						case CardDocumentFormat.AlternateFaceAndBack:

							var collecBuilderAFB = () => {
								var collec = new MagickImageCollection(docImageList.Value.SelectMany(s =>
								{
									return new[] { new MagickImage(s.Front), new MagickImage(s.Back) };
								}));
								return collec;
							};
							

							targetFiles.Add((baseName, collecBuilderAFB));
							objPdfManager.GeneratePdfsFromImages(targetFiles, Config.OverwriteExistingDocs);
							break;
						case CardDocumentFormat.BackFirstOneDocPerBack:

							var indexInsert = baseName.LastIndexOf('.');
							var cardsPerBack = docImageList.Value.GroupBy(card => card.Back).ToArray();
							for (int backIndex = 0; backIndex < cardsPerBack.Count(); backIndex++)
							{
								var collecBuilderBF = () => {
									var frontsAndBack = cardsPerBack[backIndex];
									var backThenFronts =
										new[] { new MagickImage(frontsAndBack.Key) }.Concat(
											frontsAndBack.Select(card => new MagickImage(card.Front)));
									var collec = new MagickImageCollection(backThenFronts);
									return collec;
								};
								
								var newName =
									$"{baseName.Substring(0, indexInsert)}-{backIndex + 1}{baseName.Substring(indexInsert)}";
								targetFiles.Add((newName, collecBuilderBF));
							}

							objPdfManager.GeneratePdfsFromImages(targetFiles, Config.OverwriteExistingDocs);
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
					Console.WriteLine(e);
				}


			});
		}

		/// <summary>
		/// Generates MindMap documents from the given configuration.
		/// </summary>
		private void GenerateMindMapDocuments()
		{
			foreach (var mindMap in Config.MindMapDocuments)
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
						fallacies = Fallacy.LoadFallaciesAsync(dataSet).GetAwaiter().GetResult();
					}

					var targetLanguages = Config.LocalizationConfig.BuildLanguageList(mindMap.Translations);
					foreach (var targetLanguage in targetLanguages)
					{
						var currentTranslatedMap = mindMap.CloneMindMap();
						foreach (var documentLocalization in Config.LocalizationConfig.MindMapLocalization)
						{
							documentLocalization.DoReflectionTranslate(currentTranslatedMap, targetLanguage);
						}
						currentTranslatedMap.GenerateMindMapFile(fallacies, Config);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}


			}
		}

	}
}