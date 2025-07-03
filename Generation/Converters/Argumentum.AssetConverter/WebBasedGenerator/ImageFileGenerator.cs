using Spectre.Console;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
		Logger.LogTitle("Generating document images");

		Logger.LogExplanations("In its second stage, Argumentum creates individual image files from the harvested collections. Images are processed with Magick.Net according to configuration parameters. This is the more taxing stage, the degree of parallelism of which can also be configured.");

		var toReturn = new ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>>();
		var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = Config.EnableParallelism? Config.MaxDegreeOfParallelismImages : 1 };

		using (var logStream = new StreamWriter("debug_log.txt", false))
		using (var synchronizedLogWriter = TextWriter.Synchronized(logStream))
		{
			Parallel.ForEach(Config.CardSetDocuments.Where(d => d.Enabled), parallelOptionsDocuments, configDocument =>
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

							GenerateFacesAndAssembleCard(configCardSet, configDocument, currentLanguage, currentHarvest, backImages, targetList, synchronizedLogWriter);


						}
					}
					catch (Exception e)
					{
						Logger.LogException(e);
					}


				});
			});
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
				if (backName.Contains('-'))
				{
					backName = backName.Substring(backName.LastIndexOf('-'));
				}

				backImages[backName] = backImage;
			}
		}
	}



	private void GenerateFacesAndAssembleCard(DocumentCardSet configCardSet, CardSetDocumentConfig configDocument, string currentLanguage, CardSetHarvest currentHarvest, ConcurrentDictionary<string, string> backImages, List<CardImages> targetList, TextWriter logWriter)
	{
		foreach (var currentHarvestFace in currentHarvest.Faces.Images)
		{
			var faceName = $"{currentHarvestFace.Key.ToLowerInvariant()}";
			if (!configDocument.NoBack)
			{
				faceName = $"{faceName}_face";
			}

			var faceImageUrl = currentHarvestFace.Value;
			var faceImage = configCardSet.LoadAndProcessImageUrl(currentLanguage, false, AssetConverterConfig, configDocument, faceName, faceImageUrl, currentHarvest.Faces.Dpi);

			AssembleCurrentCardImages(configDocument, faceName, faceImage, targetList, backImages, logWriter);
		}
	}

	private static void AssembleCurrentCardImages(CardSetDocumentConfig configDocument, string faceName, string faceImage, List<CardImages> targetList, ConcurrentDictionary<string, string> backImages, TextWriter logWriter)
	{
		var currentCard = new CardImages { Front = faceImage };
		targetList.Add(currentCard);

		if (configDocument.NoBack)
		{
			return;
		}

		if (backImages.Count > 0)
		{
			try
			{
				var targetBackName = backImages.Keys.FirstOrDefault(bn => faceName.Contains(bn));

				logWriter.WriteLine("--- Log Entry ---");
				logWriter.WriteLine($"Timestamp: {DateTime.Now}");
				logWriter.WriteLine($"faceName: {faceName}");
				logWriter.WriteLine($"Available backImages Keys: {string.Join(", ", backImages.Keys)}");

				if (targetBackName == null)
				{
					logWriter.WriteLine($"!!! targetBackName is NULL. No matching key found for face '{faceName}'. Using default. !!!");
					targetBackName = "default";
				}

				if (!backImages.ContainsKey(targetBackName))
				{
					logWriter.WriteLine($"!!! KEY NOT FOUND: '{targetBackName}'. Attempting to use default fallback. !!!");
					Logger.LogProblem($"Key '{targetBackName}' not found for face '{faceName}'. Available keys: {string.Join(", ", backImages.Keys)}");

					if (backImages.ContainsKey("default"))
					{
						targetBackName = "default";
						logWriter.WriteLine($"Found 'default' key as fallback.");
					}
					else
					{
						targetBackName = backImages.Keys.FirstOrDefault();
						logWriter.WriteLine($"!!! No 'default' key. Using first available key as last resort: {targetBackName} !!!");
					}
				}

				if (targetBackName != null)
				{
					logWriter.WriteLine($"Final targetBackName to be used: {targetBackName}");
					currentCard.Back = backImages[targetBackName];
				}
				else
				{
					logWriter.WriteLine("!!! CRITICAL: No keys available in backImages dictionary and no fallback possible. Cannot assign back. !!!");
					Logger.LogProblem($"CRITICAL: No back could be assigned for face '{faceName}'.");
				}

				logWriter.WriteLine($"-----------------\n");
			}
			catch (Exception e)
			{
				logWriter.WriteLine($"--- EXCEPTION ---");
				logWriter.WriteLine($"Timestamp: {DateTime.Now}");
				logWriter.WriteLine($"Exception during back assignment for faceName: {faceName}");
				logWriter.WriteLine($"Exception Type: {e.GetType().FullName}");
				logWriter.WriteLine($"Exception Message: {e.Message}");
				logWriter.WriteLine($"Stack Trace: {e.StackTrace}");
				logWriter.WriteLine($"-----------------\n");

				Logger.LogProblem($"Problem with Document Card Back: Front: {currentCard?.Front}\nFace Name: {faceName}\nAvailable keys: {string.Join(", ", backImages.Keys)}");
				Logger.LogException(e);
				// Do not rethrow to allow other threads to continue processing
			}
		}
	}




}