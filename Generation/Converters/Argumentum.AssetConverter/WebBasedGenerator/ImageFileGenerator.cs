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

		var toReturn = new ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>>();
		var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = Config.EnableParallelism? Config.MaxDegreeOfParallelismImages : 1 };

		var enabledDocs = Config.CardSetDocuments.Where(d => d.Enabled).ToList();
		Logger.Log($"Found {enabledDocs.Count} enabled documents to process in ImageFileGenerator.");
		Parallel.ForEach(enabledDocs, parallelOptionsDocuments, configDocument =>
			//foreach (var configDocument in Config.Documents.Where(d => d.Enabled))
		{
			var targetLanguages = new List<string>(new[] { AssetConverterConfig.LocalizationConfig.DefaultLanguage });
			if (AssetConverterConfig.LocalizationConfig.Enabled)
			{
				targetLanguages.AddRange(configDocument.Translations.Select(t => t.targetLanguage));
			}
			var parallelOptionsDocumentsTranslations = new ParallelOptions { MaxDegreeOfParallelism =  Config.EnableParallelism ?  Config.MaxDegreeOfParallelismImageTranslations : 1 };
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
						var currentHarvest = harvestDictionary[harvestKey]();
						var backImages = new ConcurrentDictionary<string, string>();
						GenerateBacks(configCardSet, configDocument, currentLanguage, currentHarvest, backImages);

						GenerateFacesAndAssembleCard(configCardSet, configDocument, currentLanguage, currentHarvest, backImages, targetList);
					}
				}
				catch (Exception e)
				{
					Logger.LogException(e);
				}
			});
		});
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
				if (backName.Contains('-'))
				{
					backName = backName.Substring(backName.LastIndexOf('-'));
				}

				backImages[backName] = backImage;
			}
		}
	}



	private void GenerateFacesAndAssembleCard(DocumentCardSet configCardSet, CardSetDocumentConfig configDocument, string currentLanguage, CardSetHarvest currentHarvest, ConcurrentDictionary<string, string> backImages, List<CardImages> targetList)
	{
		foreach (var (faceKey, cardFaceUrl) in currentHarvest.Faces.Images)
		{
			var faceName = $"{faceKey.ToLowerInvariant()}";
			if (!configDocument.NoBack)
			{
				faceName = $"{faceName}_face";
			}

			var faceImage = configCardSet.LoadAndProcessImageUrl(currentLanguage, false, AssetConverterConfig, configDocument, faceName, cardFaceUrl, currentHarvest.Faces.Dpi);

			AssembleCurrentCardImages(configDocument, faceKey, faceImage, targetList, backImages);
		}
	}

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
	           Logger.LogProblem($"CRITICAL: No back could be assigned for face '{faceKey}'. No back images available for this card set.");
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
	               var firstAvailableBack = backImages.FirstOrDefault();
	               if (firstAvailableBack.Value != null)
	               {
	                   foundBackImage = firstAvailableBack.Value;
	                   Logger.LogWarning($"Default back not found. Using first available back '{firstAvailableBack.Key}' as a fallback for face '{faceKey}'.");
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