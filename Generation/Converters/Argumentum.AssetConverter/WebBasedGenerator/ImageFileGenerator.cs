using Argumentum.AssetConverter;
using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter;

public class ImageFileGenerator
{
	private static readonly object _logLock = new object();

	public AssetConverterConfig AssetConverterConfig { get; set; }

	public  WebBasedGeneratorConfig Config { get; set; }




	/// <summary>
	/// Generates images for a given document and language, and returns a ConcurrentDictionary of the generated images.
	/// </summary>
	/// <param name="harvestDictionary">A ConcurrentDictionary of card set names and languages, and their associated harvest functions.</param>
	/// <returns>A ConcurrentDictionary of the generated images.</returns>
	public ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>> GenerateDocumentImages(ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>> harvestDictionary)
	{
		Logger.Log("Entering GenerateDocumentImages");
		Logger.LogTitle("Generating document images");

		Logger.LogExplanations("In its second stage, Argumentum creates individual image files from the harvested collections. Images are processed with Magick.Net according to configuration parameters. This is the more taxing stage, the degree of parallelism of which can also be configured.");

		var intermediateDict = new ConcurrentDictionary<(string docName, string lang), (CardSetDocumentConfig doc, List<CardImages> images)>();

		var enabledDocs = Config.CardSetDocuments.Where(d => d.Enabled).ToList();
		Logger.Log($"Found {enabledDocs.Count} enabled documents to process in ImageFileGenerator.");

		var parallelOptionsDocs = new ParallelOptions { MaxDegreeOfParallelism = Config.EnableParallelism ? Config.MaxDegreeOfParallelismImageTranslations : 1 };
		Parallel.ForEach(enabledDocs, parallelOptionsDocs, configDocument =>
		{
			var targetLanguages = new List<string>(new[] { AssetConverterConfig.LocalizationConfig.DefaultLanguage });
			if (AssetConverterConfig.LocalizationConfig.Enabled)
			{
				targetLanguages.AddRange(configDocument.Translations.Select(t => t.targetLanguage));
			}

			var parallelOptionsLang = new ParallelOptions { MaxDegreeOfParallelism = Config.EnableParallelism ? Config.MaxDegreeOfParallelismImageTranslations : 1 };
			Parallel.ForEach(targetLanguages, parallelOptionsLang, currentLanguage =>
			{
				try
				{
					var imageList = new List<CardImages>();
					foreach (var configCardSet in configDocument.CardSets)
					{
						var documentLocalizedName = CardSetLocalization.GetLocalizedFileName(
							configDocument.DocumentName,
							AssetConverterConfig.LocalizationConfig.DefaultLanguage, currentLanguage);
						Logger.Log($"Generating card set images for {documentLocalizedName} - {configCardSet.CardSetName}");

						var harvestKey = (configCardSet.CardSetName, currentLanguage);
						if (!harvestDictionary.ContainsKey(harvestKey))
						{
							Logger.LogWarning($"Harvest key not found: {harvestKey}. Skipping.");
							continue;
						}
						if (!harvestDictionary.TryGetValue(harvestKey, out var harvestFunc))
						{
							Logger.LogWarning($"Harvest key not found: {harvestKey}. Skipping.");
							continue;
						}
						var currentHarvest = harvestFunc();
						var backImages = new ConcurrentDictionary<string, string>();
						GenerateBacks(configCardSet, configDocument, currentLanguage, currentHarvest, backImages);

						GenerateFacesAndAssembleCard(configCardSet, configDocument, currentLanguage, currentHarvest, backImages, imageList);
					}
					intermediateDict.TryAdd((configDocument.DocumentName, currentLanguage), (configDocument, imageList));
				}
				catch (Exception e)
				{
					Logger.LogException(e);
					// Ensure that even in case of an error, we return an entry for this document/language pair.
					// This is important for the caller to know that a process was attempted.
					intermediateDict.TryAdd((configDocument.DocumentName, currentLanguage), (configDocument, new List<CardImages>()));
				}
			});
		});

		var toReturn = new ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>>();
		foreach (var item in intermediateDict)
		{
			toReturn.TryAdd((item.Value.doc, item.Key.lang), item.Value.images);
		}

		return toReturn;
	}

	private void GenerateBacks(DocumentCardSet configCardSet, CardSetDocumentConfig configDocument, string currentLanguage, CardSetHarvest currentHarvest,
		ConcurrentDictionary<string, string> backImages)
	{
		if (currentHarvest.Backs != null)
		{
			foreach (var currentHarvestBack in currentHarvest.Backs.Images)
			{
				var backName = $"{currentHarvestBack.Key.ToLowerInvariant()}";
				var backImageUrl = currentHarvestBack.Value;
				var backImage = configCardSet.LoadAndProcessImageUrl(currentLanguage, true, AssetConverterConfig,
					configDocument, backName, backImageUrl, currentHarvest.Backs.Dpi);
				backImages[backName] = backImage;
			}
		}
	}



	private void GenerateFacesAndAssembleCard(DocumentCardSet configCardSet, CardSetDocumentConfig configDocument, string currentLanguage, CardSetHarvest currentHarvest, ConcurrentDictionary<string, string> backImages, List<CardImages> targetList)
	{
		Logger.Log($"Processing {currentHarvest.Faces.Images.Count} face images for card set.");
		foreach (var (faceKey, cardFaceUrl) in currentHarvest.Faces.Images)
		{
			if (string.IsNullOrEmpty(cardFaceUrl))
			{
				Logger.LogWarning($"Face image URL for '{faceKey}' is null or empty. Skipping processing.");
				continue;
			}
			var faceName = $"{faceKey.ToLowerInvariant()}";
			if (!configDocument.NoBack)
			{
				faceName = $"{faceName}_face";
			}
			var faceImage = configCardSet.LoadAndProcessImageUrl(currentLanguage, false, AssetConverterConfig, configDocument, faceName, cardFaceUrl, currentHarvest.Faces.Dpi);
			Logger.Log($"Processed face '{faceKey}'. Resulting image path: {(string.IsNullOrEmpty(faceImage) ? "NULL or EMPTY" : faceImage)}");

			if (!string.IsNullOrEmpty(faceImage))
			{
				Logger.Log($"Image for '{faceKey}' is valid, proceeding to assemble.");
				AssembleCurrentCardImages(configDocument, faceKey, faceImage, targetList, backImages);
			}
			else
			{
				Logger.LogWarning($"Image for '{faceKey}' was not generated or path is empty. Skipping assembly.");
			}
		}
	}

	// Static field to track current back index for cycling through backs when multiple faces share backs
	private static int _currentBackIndex = 0;
	private static readonly object _backIndexLock = new object();

	private static void AssembleCurrentCardImages(CardSetDocumentConfig configDocument, string faceKey, string faceImage, List<CardImages> targetList, ConcurrentDictionary<string, string> backImages)
	   {
	       var currentCard = new CardImages { Front = faceImage };

	       if (configDocument.NoBack)
	       {
	           targetList.Add(currentCard);
	           return;
	       }

	       if (backImages.Count == 0)
	       {
	           Logger.LogWarning($"No back could be assigned for face '{faceKey}'. No back images available for this card set. Adding face only.");
	           targetList.Add(currentCard); // Ajoute la carte avec seulement le recto
	           return;
	       }

	       string foundBackImage = null;
	       var backType = "default";
	       if (faceKey.Contains('-'))
	       {
	           backType = faceKey.Substring(faceKey.LastIndexOf('-') + 1);
	       }

	       if (backImages.TryGetValue(backType, out var backImage))
	       {
	           foundBackImage = backImage;
	       }
	       else
	       {
	           Logger.LogWarning($"Back type '{backType}' not found for face '{faceKey}'. Trying 'default'.");
	           if (backImages.TryGetValue("default", out var defaultBackImage))
	           {
	               foundBackImage = defaultBackImage;
	           }
	           else
	           {
	               // BUGFIX: Cycle through available backs instead of always using first one
	               // This supports the RowsetNb pattern where N faces share M backs (N > M)
	               var backsList = backImages.Values.ToList();
	               if (backsList.Count > 0)
	               {
	                   lock (_backIndexLock)
	                   {
	                       foundBackImage = backsList[_currentBackIndex % backsList.Count];
	                       _currentBackIndex++;
	                   }
	                   Logger.LogWarning($"Default back not found. Using cycled back (index {_currentBackIndex - 1} mod {backsList.Count}) as fallback for face '{faceKey}'.");
	               }
	           }
	       }

	       if (foundBackImage != null)
	       {
	           currentCard.Back = foundBackImage;
	           targetList.Add(currentCard);
	       }
	       else
	       {
	           Logger.LogProblem($"CRITICAL: No back could be assigned for face '{faceKey}'. No back of type '{backType}', no 'default' back, and no other backs available.");
	       }
	   }




}