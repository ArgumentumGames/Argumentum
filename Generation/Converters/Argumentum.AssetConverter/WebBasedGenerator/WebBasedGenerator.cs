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
//using Svg.Skia;


namespace Argumentum.AssetConverter
{
	public class WebBasedGenerator
	{



		public static Stopwatch StopWatch = Stopwatch.StartNew();



		public WebBasedGeneratorConfig Config { get; set; }

		public WebBasedGenerator(WebBasedGeneratorConfig config, Stopwatch objSw)
		{
			Config = config;
			StopWatch = objSw;

		}



		/// <summary>
		/// Runs the program to generate card set documents from harvested images.
		/// </summary>
		public void Run()
		{
			var harvestManager = new HarvestManager() { Stopwatch = StopWatch, Config = Config };
			//var harvestDictionary = HarvestImages();
			var harvestDictionary = Task.Run(async () => await harvestManager.HarvestImages()).Result;
			var docImages = GenerateDocumentImages(harvestDictionary);

			GenerateCardSetDocuments(docImages);
			//Console.WriteLine($"Generation finished. Total duration: {sw.Elapsed}");

		}




		/// <summary>
		/// Generates images for a given document and language, and returns a ConcurrentDictionary of the generated images.
		/// </summary>
		/// <param name="harvestDictionary">A ConcurrentDictionary of card set names and languages, and their associated harvest functions.</param>
		/// <returns>A ConcurrentDictionary of the generated images.</returns>
		public ConcurrentDictionary<(CardSetGenerationDocument document, string language), List<CardImages>> GenerateDocumentImages(ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>> harvestDictionary)
		{
			var toReturn = new ConcurrentDictionary<(CardSetGenerationDocument document, string language), List<CardImages>>();
			var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = Config.MaxDegreeOfParallelismDocumentImages };

			Parallel.ForEach(Config.Documents.Where(d => d.Enabled), parallelOptionsDocuments, configDocument =>
			//foreach (var configDocument in Config.Documents.Where(d => d.Enabled))
			{

				var targetLanguages = new List<string>(new[] { Config.LocalizationConfig.DefaultLanguage });
				if (Config.LocalizationConfig.Enabled)
				{
					targetLanguages.AddRange(configDocument.Translations.Select(t => t.targetLanguage));
				}
				var parallelOptionsDocumentsTranslations = new ParallelOptions { MaxDegreeOfParallelism = Config.MaxDegreeOfParallelismDocumentTranslations };
				Parallel.ForEach(targetLanguages, parallelOptionsDocumentsTranslations, currentLanguage =>
				//foreach (var currentLanguage in targetLanguages)
				{

					try
					{
						List<CardImages> targetList;

						if (!toReturn.TryGetValue((configDocument, currentLanguage), out targetList))
						{
							targetList = new List<CardImages>();
							toReturn[(configDocument, currentLanguage)] = targetList;
						}

						//foreach (var configCardSet in configDocument.CardSets)


						foreach (var configCardSet in configDocument.CardSets)
						{
							var documentLocalizedName = LocalizationConfig.GetLocalizedFileName(
								configDocument.DocumentName,
								Config.LocalizationConfig.DefaultLanguage, currentLanguage);
							Console.WriteLine(
								$"{StopWatch.Elapsed}: Generating card set images for {documentLocalizedName} - {configCardSet.CardSetName}");



							var currentHarvest = harvestDictionary[(configCardSet.CardSetName, currentLanguage)]();
							var backImages = new ConcurrentDictionary<string, string>();
							GenerateBacks(configCardSet, configDocument, currentLanguage, currentHarvest, backImages);

							GenerateFacesAndAssembleCard(configCardSet, configDocument, currentLanguage, currentHarvest, backImages, targetList);


						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}


				});
			});

			return toReturn;
		}

		private void GenerateBacks(DocumentCardSet configCardSet, CardSetGenerationDocument configDocument, string currentLanguage, CardSetHarvest currentHarvest,
			ConcurrentDictionary<string, string> backImages)
		{
			if (currentHarvest.Backs != null)
			{
				foreach (var currentHarvestBack in currentHarvest.Backs.Images)
				{
					var backName = $"{currentHarvestBack.Key.ToLowerInvariant()}";
					var backImageUrl = currentHarvestBack.Value;
					var backImage = configCardSet.LoadAndProcessImageUrl(currentLanguage, true, Config,
						configDocument, backName, backImageUrl, currentHarvest.Backs.Dpi);
					if (backName.Contains('-'))
					{
						backName = backName.Substring(backName.LastIndexOf('-'));
					}

					backImages[backName] = backImage;
				}
			}
		}



		private CardImages GenerateFacesAndAssembleCard(DocumentCardSet configCardSet, CardSetGenerationDocument configDocument, string currentLanguage, CardSetHarvest currentHarvest, ConcurrentDictionary<string, string> backImages, List<CardImages> targetList)
		{
			CardImages currentCard = null;


			foreach (var currentHarvestFace in currentHarvest.Faces.Images)
			{
				var faceName = $"{currentHarvestFace.Key.ToLowerInvariant()}";
				if (!configDocument.NoBack)
				{
					faceName = $"{faceName}_face";
				}

				var faceImageUrl = currentHarvestFace.Value;
				var faceImage = configCardSet.LoadAndProcessImageUrl(currentLanguage, false, Config, configDocument, faceName, faceImageUrl, currentHarvest.Faces.Dpi);

				currentCard = AssembleCurrentCardImages(currentHarvestFace, configDocument, configCardSet, currentLanguage, faceName, faceImage, currentCard, targetList, backImages);
			}

			return currentCard;
		}


		private static CardImages AssembleCurrentCardImages(KeyValuePair<string, string> currentHarvestFace, CardSetGenerationDocument configDocument, DocumentCardSet configCardSet, string currentLanguage, string faceName, string faceImage, CardImages currentCard, List<CardImages> targetList, ConcurrentDictionary<string, string> backImages)
		{
			if (currentCard == null)
			{
				currentCard = new CardImages();
				targetList.Add(currentCard);
				currentCard.Front = faceImage;
				if (configDocument.NoBack)
				{
					currentCard = null;
				}
			}
			else
			{
				currentCard.Back = faceImage;
				currentCard = null;
			}

			if (backImages.Count > 0)
			{
				if (backImages.Count == 1)
				{
					currentCard.Back = backImages.Values.First();
				}
				else
				{
					try
					{
						var targetBackName = backImages.Keys.First(bn => faceName.Contains(bn));
						currentCard.Back = backImages[targetBackName];
					}
					catch (Exception e)
					{
						Console.WriteLine($"Back not found:\n keys: {backImages.Keys.ToList().Aggregate((key1, key2) => $"{key1},{key2}")} / faceName: {faceName}");
						Console.WriteLine(e);
						throw;
					}
				}

				currentCard = null;
			}

			return currentCard;
		}


		/// <summary>
		/// Generates PDF documents from a ConcurrentDictionary of CardSetGenerationDocument and language, and a list of CardImages.
		/// </summary>
		private void GenerateCardSetDocuments(ConcurrentDictionary<(CardSetGenerationDocument document, string language), List<CardImages>> docImages)
		{
			Console.WriteLine($"{StopWatch.Elapsed}: Generation pdf documents");


			foreach (var docImageList in docImages)
			{
				var pdfDirectory = Config.GetPdfsDirectory(docImageList.Key.language);
				var densityDirectory = Path.Combine(pdfDirectory, $@".\density-{docImageList.Key.document.TargetDensity}\");
				if (!Directory.Exists(densityDirectory))
				{
					Directory.CreateDirectory(densityDirectory);
				}

				var targetFiles = new List<(string fileName, MagickImageCollection documentImages)>();
				MagickImageCollection collec;
				var documentName = LocalizationConfig.GetLocalizedFileName(docImageList.Key.document.DocumentName,
					Config.LocalizationConfig.DefaultLanguage, docImageList.Key.language);
				var baseName = Path.Combine(densityDirectory, documentName);

				var objPdfManager = new PdfManager() { Stopwatch = StopWatch };
				switch (docImageList.Key.document.DocumentFormat)
				{
					case CardDocumentFormat.AlternateFaceAndBack:
						collec = new MagickImageCollection(docImageList.Value.SelectMany(s =>
						{
							return new[] { new MagickImage(s.Front), new MagickImage(s.Back) };
						}));

						targetFiles.Add((baseName, collec));
						objPdfManager.GeneratePdfsFromImages(targetFiles);
						break;
					case CardDocumentFormat.BackFirstOneDocPerBack:

						var indexInsert = baseName.LastIndexOf('.');
						var cardsPerBack = docImageList.Value.GroupBy(card => card.Back).ToArray();
						for (int backIndex = 0; backIndex < cardsPerBack.Count(); backIndex++)
						{
							var frontsAndBack = cardsPerBack[backIndex];
							var backThenFronts = new[] { new MagickImage(frontsAndBack.Key) }.Concat(frontsAndBack.Select(card => new MagickImage(card.Front)));
							collec = new MagickImageCollection(backThenFronts);
							var newName = $"{baseName.Substring(0, indexInsert)}-{backIndex + 1}{baseName.Substring(indexInsert)}";
							targetFiles.Add((newName, collec));
						}
						objPdfManager.GeneratePdfsFromImages(targetFiles);
						break;
					case CardDocumentFormat.PrintAndPlay:
						objPdfManager.GeneratePrintAndPlay(baseName, docImageList.Key.document, docImageList.Value);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

			}
		}



	}
}


