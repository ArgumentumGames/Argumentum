using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter;

public class ImageFileGenerator
{
	public  WebBasedGeneratorConfig Config { get; set; }

	public Stopwatch Stopwatch { get; set; }

	public ImageFileGenerator(WebBasedGeneratorConfig config, Stopwatch stopwatch)
	{
		Config = config;
		Stopwatch = stopwatch;
	}


	/// <summary>
	/// Generates images for a given document and language, and returns a ConcurrentDictionary of the generated images.
	/// </summary>
	/// <param name="harvestDictionary">A ConcurrentDictionary of card set names and languages, and their associated harvest functions.</param>
	/// <returns>A ConcurrentDictionary of the generated images.</returns>
	public ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>> GenerateDocumentImages(ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>> harvestDictionary)
	{
		var toReturn = new ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>>();
		var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = Config.MaxDegreeOfParallelismImages };

		Parallel.ForEach(Config.CardSetDocuments.Where(d => d.Enabled), parallelOptionsDocuments, configDocument =>
			//foreach (var configDocument in Config.Documents.Where(d => d.Enabled))
		{

			var targetLanguages = new List<string>(new[] { Config.LocalizationConfig.DefaultLanguage });
			if (Config.LocalizationConfig.Enabled)
			{
				targetLanguages.AddRange(configDocument.Translations.Select(t => t.targetLanguage));
			}
			var parallelOptionsDocumentsTranslations = new ParallelOptions { MaxDegreeOfParallelism = Config.MaxDegreeOfParallelismImageTranslations };
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
							Config.LocalizationConfig.DefaultLanguage, currentLanguage);
						Console.WriteLine(
							$"{Stopwatch.Elapsed}: Generating card set images for {documentLocalizedName} - {configCardSet.CardSetName}");



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

	private void GenerateBacks(DocumentCardSet configCardSet, CardSetDocumentConfig configDocument, string currentLanguage, CardSetHarvest currentHarvest,
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



	private CardImages GenerateFacesAndAssembleCard(DocumentCardSet configCardSet, CardSetDocumentConfig configDocument, string currentLanguage, CardSetHarvest currentHarvest, ConcurrentDictionary<string, string> backImages, List<CardImages> targetList)
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


	private static CardImages AssembleCurrentCardImages(KeyValuePair<string, string> currentHarvestFace, CardSetDocumentConfig configDocument, DocumentCardSet configCardSet, string currentLanguage, string faceName, string faceImage, CardImages currentCard, List<CardImages> targetList, ConcurrentDictionary<string, string> backImages)
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




}