using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Utf8Json;

namespace Argumentum.AssetConverter;

public class HarvestManager
{
	public async Task<ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>>> HarvestImages()
	{
		ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>> harvestDictionary;
		var parallelOptionsLoading = new ParallelOptions { MaxDegreeOfParallelism = 4 };

		var targetCardSets = GetTargetCardSets();
		harvestDictionary = await LoadHarvestsAsync(targetCardSets, parallelOptionsLoading);

		var expectedHarvestNb = targetCardSets.Sum(tcs => tcs.Translations.Count + 1);


		using var playwright = await Playwright.CreateAsync();
		await using IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		{
			Headless = false,
			//SlowMo = 50,
		});

		var parallelOptionsCardset = new ParallelOptions { MaxDegreeOfParallelism = Config.MaxDegreeOfParallelismCardpen };
		await Parallel.ForEachAsync(targetCardSets, parallelOptionsCardset, async (configCardSet, token) =>
		{
			var targetLanguages = Config.LocalizationConfig.BuildLanguageList(configCardSet.Translations);

			var parallelOptionsCardsetLanguage = new ParallelOptions { MaxDegreeOfParallelism = Config.MaxDegreeOfParallelismCardpenTranslations };
			await Parallel.ForEachAsync(targetLanguages, parallelOptionsCardsetLanguage, async (currentLanguage, newToken) =>
			{
				await ProcessLocalizedHarvest(configCardSet, currentLanguage, harvestDictionary, browser);
			});
		});

		return harvestDictionary;
	}


	/// <summary>
	/// Gets the target card sets from the configuration.
	/// </summary>
	/// <returns>An array of CardSetJob objects.</returns>
	private CardSetJob[] GetTargetCardSets()
	{
		var targetCardSets = Config.CardSetDocuments
			.Where(d => d.Enabled)
			.SelectMany(d => d.CardSets.Select(dc => new CardSetJob { Name = dc.CardSetName, Translations = d.Translations }))
			.Distinct(CardSetJob.Comparer)
			.ToArray();
		foreach (var usedCardSet in targetCardSets)
		{
			usedCardSet.Config = Config.CardSets.First(c => c.Name == usedCardSet.Name);
		}

		return targetCardSets;
	}


	//Rewritten code with comments

	public async Task<ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>>> LoadHarvestsAsync(CardSetJob[] targetCardSets, ParallelOptions parallelOptionsLoading)
	{
		//Create a new concurrent dictionary to store the card set harvests
		var harvestDictionary = new ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>>();

		//Loop through each card set job in the target card sets
		await Parallel.ForEachAsync(targetCardSets, parallelOptionsLoading, async (configCardSet, token) =>
		{
			//Get the target languages for the current card set job
			var targetlanguages = GetTargetLanguages(configCardSet);

			//Loop through each language in the target languages
			foreach (var currentLanguage in targetlanguages)
			{
				//Get the serialization name for the harvest
				var jsonHarvestName = configCardSet.Config.GetHarvestSerializationName(Config, currentLanguage);

				//If the file exists
				if (File.Exists(jsonHarvestName))
				{
					//Print out the elapsed time and the existing harvest
					Console.WriteLine($"{Stopwatch.Elapsed}: Found existing Harvest {jsonHarvestName}");

					//Create a function to load the card set harvest
					var funcLoad = () => { return LoadCardSetHarvest(jsonHarvestName); };

					//Add the card set name and language to the dictionary with the function to load the harvest
					harvestDictionary[(configCardSet.Name, currentLanguage)] = funcLoad;
				}
			}
		});

		//Return the dictionary
		return harvestDictionary;
	}

	public List<string> GetTargetLanguages(CardSetJob configCardSet)
	{
		var targetlanguages = new List<string>(new[] { Config.LocalizationConfig.DefaultLanguage });
		if (Config.LocalizationConfig.Enabled)
		{
			targetlanguages.AddRange(configCardSet.Translations.Select(t => t.targetLanguage));
		}

		return targetlanguages;
	}


	

	public async Task ProcessLocalizedHarvest(CardSetJob configCardSet, string currentLanguage, ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>> harvestDictionary, IBrowser browser)
	{
		if (!harvestDictionary.ContainsKey((configCardSet.Name, currentLanguage)))
		{
			try
			{
				// Get the documents needed to prepare the card set
				var cardSetDocuments = await PrepareCardSetDocuments(configCardSet, currentLanguage);

				// Generate the harvest images
				var currentHarvest = await GenerateHarvestImages(browser, configCardSet, cardSetDocuments);

				// Get the name of the JSON file for the harvest
				var jsonHarvestName = configCardSet.Config.GetHarvestSerializationName(Config, currentLanguage);

				// Serialize the harvest into a JSON string
				var strNewConfig = JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(currentHarvest));

				// Write the JSON string to the file
				File.WriteAllText(jsonHarvestName, strNewConfig);

				// Log the time it took to serialize the harvest
				Console.WriteLine($"{Stopwatch.Elapsed}: Serialized Harvest {jsonHarvestName}");

				// Create a function to load the card set harvest
				Func<CardSetHarvest> funcLoad = () => { return LoadCardSetHarvest(jsonHarvestName); };

				// Add the function to the harvest dictionary
				harvestDictionary[(configCardSet.Name, currentLanguage)] = funcLoad;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}

	public async Task<(CardSetPayload front, CardSetPayload back)> PrepareCardSetDocuments(CardSetJob configCardSet, string currentLanguage)
	{
		(CardSetPayload front, CardSetPayload back) cardSetDocuments;

		if (currentLanguage == Config.LocalizationConfig.DefaultLanguage)
		{
			var frontCardSetDocument = await configCardSet.Config.FaceCardSetInfo.GetCardSetDocument();
			var backCardSetDocument = await configCardSet.Config.BackCardSetInfo.GetCardSetDocument();
			cardSetDocuments = (frontCardSetDocument, backCardSetDocument);
		}
		else
		{
			cardSetDocuments = await Config.LocalizationConfig.TranslateCardSet(configCardSet.Config, (Config.LocalizationConfig.DefaultLanguage, currentLanguage));
		}

		if (!configCardSet.Config.FaceCardSetInfo.SkipDataUpdate && !string.IsNullOrEmpty(configCardSet.Config.FaceCardSetInfo.DataSet))
		{
			var dataSet = Config.DataSets.First(ds => ds.Name == configCardSet.Config.FaceCardSetInfo.DataSet);
			cardSetDocuments.front.CardSetDocument.csv = await dataSet.GetContent();
		}

		if (cardSetDocuments.back != null && !configCardSet.Config.BackCardSetInfo.SkipDataUpdate && !string.IsNullOrEmpty(configCardSet.Config.BackCardSetInfo.DataSet))
		{
			var dataSet = Config.DataSets.First(ds => ds.Name == configCardSet.Config.BackCardSetInfo.DataSet);
			cardSetDocuments.back.CardSetDocument.csv = await dataSet.GetContent();
		}

		return cardSetDocuments;
	}

	public async Task<CardSetHarvest> GenerateHarvestImages(IBrowser browser, CardSetJob configCardSet, (CardSetPayload front, CardSetPayload back) cardSetDocuments)
	{
		var currentHarvest = new CardSetHarvest();

		var page = await browser.NewPageAsync();
		await page.GotoAsync(Config.CardpenUrl);
		await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
		await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
		Thread.Sleep(TimeSpan.FromSeconds(2));

		var faces = await GenerateImages(page, cardSetDocuments.front, configCardSet.Config.FaceCardSetInfo.PauseForEdits);
		currentHarvest.Faces = faces;

		if (cardSetDocuments.back != null)
		{
			var backs = await GenerateImages(page, cardSetDocuments.back, configCardSet.Config.BackCardSetInfo.PauseForEdits);
			currentHarvest.Backs = backs;
		}

		await page.CloseAsync();

		return currentHarvest;
	}


	public async Task<CardPenHarvest> GenerateImages(IPage driver, CardSetPayload cardSetDocument, bool pauseForEdits)
	{
		var toReturn = new CardPenHarvest();

		await UploadCardPenDocument(driver, cardSetDocument, pauseForEdits);
		//var objIFrame = driver.FindElement(By.Id("cpOutput"));
		var objIFrame = driver.FrameLocator("#cpOutput");


		//var objSession = ((ChromiumDriver) driver).CreateDevToolsSession();


		await WaitForCards(driver, objIFrame);

		//var dpi = (long)driver.ExecuteScript("return dpi;");
		//var dpi = await driver.EvaluateAsync("return dpi;");
		//var dpi = await driver.EvaluateAsync("dpi");

		var dpi = await driver.Locator("#dpi").InputValueAsync();


		toReturn.Dpi = Convert.ToInt32(dpi);

		await ClickGenerateAndWait(objIFrame);

		var cardNames = await ExtractCardNames(objIFrame);

		await DownloadImages(toReturn, objIFrame, cardNames);



		return toReturn;
	}


	public async Task UploadCardPenDocument(IPage driver, CardSetPayload cardSetDocument, bool pauseForEdits)
	{
		var importInput = driver.Locator("#import");


		if (!await importInput.IsVisibleAsync())
		{
			var exampleButton = driver.Locator("#load");
			await exampleButton.ClickAsync();

			Thread.Sleep(TimeSpan.FromSeconds(2));
		}

		Console.WriteLine($"{Stopwatch.Elapsed}: Generating CardSet {cardSetDocument.FileName}");

		var filePayLoad = new FilePayload()
		{
			Name = cardSetDocument.FileName,
			MimeType = cardSetDocument.GetMimeType(),
			Buffer = JsonSerializer.Serialize(cardSetDocument.CardSetDocument)
		};

		await driver.SetInputFilesAsync("#import", filePayLoad);


		if (pauseForEdits)
		{
			Console.WriteLine($"Le navigateur est en pause le temps de faire vos éditions.\n Appuyez sur une touche pour démarrer la génération");
			Console.Read();
		}
		else
		{
			Thread.Sleep(TimeSpan.FromSeconds(5));
		}

	}


	public async Task WaitForCards(IPage driver, IFrameLocator objIFrame)
	{
		var objCardTag = objIFrame.Locator("card");
		while (await objCardTag.CountAsync() == 0)
		{
			Thread.Sleep(100);
		}
		await driver.Locator(".image").ClickAsync();

		Thread.Sleep(TimeSpan.FromSeconds(5));

		while (await objCardTag.CountAsync() == 0)
		{
			Thread.Sleep(100);
		}
	}

	public async Task ClickGenerateAndWait(IFrameLocator objIFrame)
	{
		var objGenerateButton = objIFrame.Locator("#generateButton");
		Thread.Sleep(TimeSpan.FromSeconds(5));
		await objGenerateButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Attached, Timeout = 0 });
		await objGenerateButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 0 });

		await objGenerateButton.ClickAsync();

		Thread.Sleep(TimeSpan.FromSeconds(5));

		var zipButton = objIFrame.Locator("#zipButton");
		await zipButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Attached, Timeout = 0 });
		await zipButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 0 });
		var zipHandler = await zipButton.ElementHandleAsync();
		await zipHandler.WaitForElementStateAsync(ElementState.Enabled);

		//Console.WriteLine($"images generated {sw.Elapsed}");
		//var generatedImagesDiv = driver.FindElement(By.Id("cpImages"));
		var generatedImagesDiv = objIFrame.Locator("#cpImages");

		//var generatedImages = generatedImagesDiv.FindElements(By.TagName("img"));
		var generatedImages = generatedImagesDiv.Locator("img");
	}

	public async Task<List<string>> ExtractCardNames(IFrameLocator objIFrame)
	{
		var cardNames = new List<string>();
		var cardsHtml = objIFrame.Locator("card");
		for (var idxCard = 0; idxCard < await cardsHtml.CountAsync(); idxCard++)
		{
			var strCardName = idxCard.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0');
			var cardElement = cardsHtml.Nth(idxCard);
			var cardCssName = cardElement.Locator(".cardName");
			if (await cardCssName.CountAsync() > 0)
			{
				var currentCard = cardCssName.Nth(0);
				strCardName = (await currentCard.TextContentAsync())?.Trim('-').Replace(" ", "_");
			}
			cardNames.Add(strCardName);
		}
		return cardNames;
	}

	public async Task DownloadImages(CardPenHarvest toReturn, IFrameLocator objIFrame, List<string> cardNames)
	{
		var generatedImagesDiv = objIFrame.Locator("#cpImages");
		var generatedImages = generatedImagesDiv.Locator("img");

		if (await generatedImages.CountAsync() != cardNames.Count)
		{
			var message = $"not same number of generated cards ({await generatedImages.CountAsync()}) and card names ({cardNames.Count})";
			throw new ApplicationException(message);
		}

		for (int i = 0; i < await generatedImages.CountAsync(); i++)
		{
			var currentGeneratedImage = generatedImages.Nth(i);
			var currentCardName = cardNames[i];
			toReturn.Images[currentCardName] = await currentGeneratedImage.GetAttributeAsync("src");
			Console.WriteLine($"{Stopwatch.Elapsed}: Downloaded Card Image - {currentCardName}");
		}
	}

	private  CardSetHarvest LoadCardSetHarvest(string jsonHarvestName)
	{
		using var configStream = File.OpenRead(jsonHarvestName);
		var currentHarvest = JsonSerializer.Deserialize<CardSetHarvest>(configStream);
		Console.WriteLine($"{Stopwatch.Elapsed}: Loaded Harvest {jsonHarvestName}");
		return currentHarvest;
	}

	public Stopwatch Stopwatch { get; set; }
	public WebBasedGeneratorConfig Config { get; set; }
}