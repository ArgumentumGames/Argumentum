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
using Spectre.Console;
using Utf8Json;
using System.Text.Json;
using Argumentum.AssetConverter.Entities;
using Xunit.Abstractions;
 
 namespace Argumentum.AssetConverter;
public class HarvestManager : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        if (_browser != null && !KeepBrowserOpen)
        {
            await _browser.CloseAsync();
            _browser = null;
        }
    }
	public Stopwatch Stopwatch { get; set; }

	public AssetConverterConfig AssetConverterConfig { get; set; }
	public WebBasedGeneratorConfig Config { get; set; }
	  public ITestOutputHelper Output { get; set; }

	private static readonly SemaphoreSlim _browserSemaphore = new SemaphoreSlim(1, 1);
	private static IBrowser _browser;
	public IPage LastPageUsed { get; private set; }
	public bool KeepBrowserOpen { get; set; } = false;
	
	private async Task<IBrowser> GetBrowserAsync()
	{
		Log("Entering GetBrowserAsync.");
		if (_browser == null)
		{
			Log("Browser is null, entering semaphore.");
			await _browserSemaphore.WaitAsync();
			try
			{
				if (_browser == null)
				{
					Log("Browser is still null inside lock, initializing Playwright.");
					var playwright = await Playwright.CreateAsync();
					Log("Playwright created. Launching Chromium.");
					_browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
					{
						Headless = true,
						Timeout = 120 * 1000,
					});
					Log("Browser launched successfully.");
				}
			}
			finally
			{
				_browserSemaphore.Release();
				Log("Semaphore released.");
			}
		}
		Log("Exiting GetBrowserAsync.");
		return _browser;
	}

	public async Task<ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>>> HarvestImages()
	{
		Log("Harvesting Cardpen Images");
		Log("In its first stage, Argumentum uses the web-based cardpen generator...");

		var harvestDictionary = new ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>>();
		var parallelOptionsLoading = new ParallelOptions { MaxDegreeOfParallelism = 4 };

		var targetCardSets = GetTargetCardSets();
		await LoadHarvestsAsync(targetCardSets, parallelOptionsLoading, harvestDictionary);

		var funcBrowser = new Func<Task<IBrowser>>(GetBrowserAsync);

		var parallelOptionsCardset = new ParallelOptions { MaxDegreeOfParallelism = Config.EnableParallelism? Config.MaxDegreeOfParallelismCardpen : 1 };
		await Parallel.ForEachAsync(targetCardSets, parallelOptionsCardset, async (configCardSet, token) =>
		{
			var targetLanguages = AssetConverterConfig.LocalizationConfig.BuildLanguageList(configCardSet.Translations);
			var parallelOptionsCardsetLanguage = new ParallelOptions { MaxDegreeOfParallelism = Config.EnableParallelism?  Config.MaxDegreeOfParallelismCardpenTranslations : 1 };
			await Parallel.ForEachAsync(targetLanguages, parallelOptionsCardsetLanguage, async (currentLanguage, newToken) =>
			{
				await ProcessLocalizedHarvest(configCardSet, currentLanguage, harvestDictionary, funcBrowser);
			});
		});

		return harvestDictionary;
	}

	public CardSetJob[] GetTargetCardSets()
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
		Log($"Found {targetCardSets.Length} target card sets to process.");
		return targetCardSets;
	}

	public async Task LoadHarvestsAsync(CardSetJob[] targetCardSets, ParallelOptions parallelOptionsLoading, ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>> harvestDictionary)
	{
		await Parallel.ForEachAsync(targetCardSets, parallelOptionsLoading, (configCardSet, token) =>
		{
			var targetlanguages = GetTargetLanguages(configCardSet);
			foreach (var currentLanguage in targetlanguages)
			{
				var jsonHarvestName = configCardSet.Config.GetHarvestSerializationName(AssetConverterConfig, currentLanguage);
				if (File.Exists(jsonHarvestName))
				{
					Log($"Found existing Harvest {jsonHarvestName}");
					var funcLoad = () => { return LoadCardSetHarvest(jsonHarvestName); };
					harvestDictionary[(configCardSet.Name, currentLanguage)] = funcLoad;
				}
			}
			return ValueTask.CompletedTask;
		});
		Log($"Loaded {harvestDictionary.Count} existing harvests.");
	}

	public List<string> GetTargetLanguages(CardSetJob configCardSet)
	{
		var targetlanguages = new List<string>(new[] { AssetConverterConfig.LocalizationConfig.DefaultLanguage });
		if (AssetConverterConfig.LocalizationConfig.Enabled)
		{
			targetlanguages.AddRange(configCardSet.Translations.Select(t => t.targetLanguage));
		}
		return targetlanguages;
	}

	public async Task ProcessLocalizedHarvest(CardSetJob configCardSet, string currentLanguage, ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>> harvestDictionary, Func<Task<IBrowser>> browser)
	{
		if (!harvestDictionary.ContainsKey((configCardSet.Name, currentLanguage)))
		{
			try
			{
				var cardSetDocuments = await PrepareCardSetDocuments(configCardSet, currentLanguage);
				var currentHarvest = await GenerateHarvestImages(browser, configCardSet, cardSetDocuments);
				var jsonHarvestName = configCardSet.Config.GetHarvestSerializationName(AssetConverterConfig, currentLanguage);
				using var fileStream = File.Create(jsonHarvestName);
				System.Text.Json.JsonSerializer.Serialize(fileStream, currentHarvest, new JsonSerializerOptions { WriteIndented = true });
				fileStream.Flush();
				Func<CardSetHarvest> funcLoad = () => { return LoadCardSetHarvest(jsonHarvestName); };
				harvestDictionary[(configCardSet.Name, currentLanguage)] = funcLoad;
			}
			catch (Exception e)
			{
				Logger.LogException(e);
				throw;
			}
		}
	}

	public async Task<(CardSetPayload front, CardSetPayload back)> PrepareCardSetDocuments(CardSetJob configCardSet, string currentLanguage)
	{
		(CardSetPayload front, CardSetPayload back) cardSetDocuments;
		if (currentLanguage == AssetConverterConfig.LocalizationConfig.DefaultLanguage)
		{
			var frontCardSetDocument = await configCardSet.Config.FaceCardSetInfo.GetCardSetDocument(AssetConverterConfig);
			var backCardSetDocument = await configCardSet.Config.BackCardSetInfo.GetCardSetDocument(AssetConverterConfig);
			cardSetDocuments = (frontCardSetDocument, backCardSetDocument);
		}
		else
		{
			cardSetDocuments = await AssetConverterConfig.LocalizationConfig.TranslateCardSet(configCardSet.Config, (AssetConverterConfig.LocalizationConfig.DefaultLanguage, currentLanguage), AssetConverterConfig);
		}
		await UpdateCardSetDocumentInfo(cardSetDocuments.front, configCardSet.Config.FaceCardSetInfo);
		if (cardSetDocuments.back != null)
		{
			await UpdateCardSetDocumentInfo(cardSetDocuments.back, configCardSet.Config.BackCardSetInfo);
		}
		return cardSetDocuments;
	}

	private async Task UpdateCardSetDocumentInfo(CardSetPayload cardSetDocumentWrapper, CardSetInfo cardSetInfo)
	{
		if (!cardSetInfo.SkipDataUpdate && !string.IsNullOrEmpty(cardSetInfo.DataSet))
		{
			Log("Dumping AssetConverterConfig and cardSetInfo before First()");
			Logger.LogJson(System.Text.Json.JsonSerializer.Serialize(AssetConverterConfig));
			Log($"Searching for DataSet with name: '{cardSetInfo.DataSet}'");
			var dataSet = AssetConverterConfig.DataSets.First(ds => ds.Name == cardSetInfo.DataSet);
			string csvContent;
			if (!string.IsNullOrEmpty(cardSetInfo.CsvFilterField) && cardSetInfo.CsvFilterValues.Count>0)
			{
				csvContent = await dataSet.GetContent(AssetConverterConfig.UseDebugParams, ",", "",  cardSetInfo.CsvFilterField, cardSetInfo.CsvFilterValues);
			}
			else
			{
				csvContent = await dataSet.GetContent(AssetConverterConfig.UseDebugParams);
			}
			if (csvContent != null)
			{
				cardSetDocumentWrapper.CardSetDocument.csv = csvContent;
			}
			cardSetDocumentWrapper.CsvType = dataSet.CsvType;
		}
		if (cardSetInfo.Dpi > 0)
		{
			cardSetDocumentWrapper.CardSetDocument.dpi = cardSetInfo.Dpi;
		}
		if (cardSetInfo.RowsetNb > 0)
		{
			cardSetDocumentWrapper.CardSetDocument.rscount = cardSetInfo.RowsetNb;
		}
		if (!string.IsNullOrEmpty(cardSetInfo.CardSize))
		{
			cardSetDocumentWrapper.CardSetDocument.csize = cardSetInfo.CardSize;
		}
	}

	private ConcurrentStack<IPage> Freepages = new ConcurrentStack<IPage>();

	private async Task<IPage> GetFreePage(Func<Task<IBrowser>> browser)
	{
		if (Freepages.TryPop(out var page))
		{
			return page;
		}
		else
		{
			var b = await browser();
			var newPage = await b.NewPageAsync();
			LastPageUsed = newPage;
			return newPage;
		}
	}

	private void ReleasePage(IPage page)
	{
		Freepages.Push(page);
	}

	public async Task<CardSetHarvest> GenerateHarvestImages(Func<Task<IBrowser>> browser, CardSetJob configCardSet, (CardSetPayload front, CardSetPayload back) cardSetDocuments)
	{
		Log("Entering GenerateHarvestImages.");
		var currentHarvest = new CardSetHarvest();
		var page = await GetFreePage(browser);
		var consoleMessages = new List<string>();

		void Page_Console(object sender, IConsoleMessage msg) => Log($"[BROWSER CONSOLE] {msg.Type}: {msg.Text}");
		page.Console += Page_Console;

		try
		{
			var cardpenUrl = $"{Config.CardpenUrl}?_={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
			Log($"Navigating to Cardpen URL: {cardpenUrl}");
			await page.GotoAsync(cardpenUrl, new PageGotoOptions { Timeout = 60000 });
			Log("Navigation successful.");
			await page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions { Timeout = 60000 });

			var result = await page.EvaluateAsync<System.Text.Json.JsonElement>(
			    "(() => {" +
			    "    const startTime = Date.now();" +
			    "    const timeout = 58000;" +
			    "    let logs = ['Diagnostic script started'];" +
			    "    return new Promise(resolve => {" +
			    "        const intervalId = setInterval(() => {" +
			    "            const iframe = document.getElementById('cpOutput');" +
			    "            if (iframe && iframe.contentWindow) {" +
			    "                clearInterval(intervalId);" +
			    "                logs.push('SUCCESS: #cpOutput iframe found.');" +
			    "                resolve({ success: true, logs: logs });" +
			    "            } else if (Date.now() - startTime > timeout) {" +
			    "                clearInterval(intervalId);" +
			    "                logs.push('FAILURE: Timeout waiting for #cpOutput iframe.');" +
			    "                resolve({ success: false, logs: logs });" +
			    "            }" +
			    "        }, 100);" +
			    "    });" +
			    "})()");

			if (!result.GetProperty("success").GetBoolean())
			{
				var logs = result.GetProperty("logs").EnumerateArray().Select(l => l.GetString()).ToList();
				var errorMsg = "Diagnostic check failed: #cpOutput iframe did not become available in time.";
				Log(errorMsg);
				foreach(var log in logs) { Log($"[BROWSER LOG] {log}"); }
				throw new TimeoutException(errorMsg);
			}

			Log("Diagnostic check passed: #cpOutput iframe is ready.");

			var faces = await GenerateImages(page, cardSetDocuments.front, configCardSet.Config.FaceCardSetInfo, consoleMessages);
			currentHarvest.Faces = faces;

			if (cardSetDocuments.back != null)
			{
				var backs = await GenerateImages(page, cardSetDocuments.back, configCardSet.Config.BackCardSetInfo, consoleMessages);
				currentHarvest.Backs = backs;
			}
		}
		finally
		{
			page.Console -= Page_Console;
			ReleasePage(page);
		}

		return currentHarvest;
	}


	public async Task<CardPenHarvest> GenerateImages(IPage page, CardSetPayload cardSetDocument, CardSetInfo cardSetInfo, List<string> consoleMessages)
	{
		var toReturn = new CardPenHarvest();
		var objIFrame = page.FrameLocator("#cpOutput");

		await UploadCardPenDocument(page, cardSetDocument, cardSetInfo.PauseForEdits);
		
		try
		{
			Log("Waiting for card rendering to complete...");
			await page.WaitForFunctionAsync("() => document.getElementById('cpOutput')?.contentWindow?.cardRenderingComplete === true", null, new PageWaitForFunctionOptions { Timeout = 60000 });
			Log("Card rendering confirmed.");
		}
		catch (Exception ex)
		{
			Log("Timeout waiting for cards to render. Dumping console and taking screenshot.");
			foreach (var msg in consoleMessages) { Log(msg); }
			var screenshotPath = $"debug_screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
			await page.ScreenshotAsync(new() { Path = screenshotPath, FullPage = true });
			Log($"Screenshot saved to: {screenshotPath}");
			throw new Exception("Failed to render cards.", ex);
		}

		var dpi = await page.Locator("#dpi").InputValueAsync();
		toReturn.Dpi = Convert.ToInt32(dpi);
		await ClickGenerateAndWait(objIFrame, consoleMessages);

		if (cardSetDocument.CsvType == null)
		{
			Log($"WARNING: No CsvType for DataSet '{cardSetInfo.DataSet}'. Skipping image download.");
			return toReturn;
		}

		var csvType = cardSetDocument.CsvType;
		var classMapType = csvType.Assembly.GetType($"{csvType.FullName}ClassMap");
		var csvBaseType = typeof(CsvBase<,>);
		var genericCsvBaseType = csvBaseType.MakeGenericType(csvType, classMapType);
		var loadMethod = genericCsvBaseType.GetMethod("LoadFromContent", new[] { typeof(string) });
		var cardData = (System.Collections.IEnumerable)loadMethod.Invoke(null, new object[] { cardSetDocument.CardSetDocument.csv });
		var cardIds = cardData.Cast<ICsvBase>().Select(c => c.GetId()).ToList();
		await DownloadImages(toReturn, objIFrame, cardIds);
		return toReturn;
	}

	public async Task UploadCardPenDocument(IPage page, CardSetPayload cardSetDocument, bool pauseForEdits)
	{
		Log($"Generating CardSet {cardSetDocument.FileName} by direct data injection.");
		var jsonString = System.Text.Json.JsonSerializer.Serialize(cardSetDocument.CardSetDocument);
		var escapedJsonString = System.Text.Json.JsonSerializer.Serialize(jsonString);

		var script = $"let jsonData = JSON.parse({escapedJsonString}); window.cardpen.form.set(jsonData); window.cardpen.write.generate(window.cardpen.form.get(), 'image');";
		await page.EvaluateAsync(script);

		Log("Data injection script executed.");
		if (pauseForEdits)
		{
			Log("Browser is paused. Press any key to resume.");
			Console.Read();
		}
	}

	public async Task WaitForCards(IPage driver, IFrameLocator objIFrame)
	{
		Log("=== Entering WaitForCards ===");
		var objCardTag = objIFrame.Locator("card");
		Log("Waiting for cards to appear and be visible...");
		
		try
		{
			await objCardTag.First.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 60000 });
			Log("First card is visible.");
		}
		catch (TimeoutException)
		{
			Log("Timeout (60s) while waiting for the first card to become visible.");
			throw;
		}

		var finalCount = await objCardTag.CountAsync();
		Log($"Final card count: {finalCount}");
		Log("=== Exiting WaitForCards ===");
	}

	public async Task ClickGenerateAndWait(IFrameLocator objIFrame, List<string> consoleMessages = null)
	{
		Log("=== Entering ClickGenerateAndWait ===");
		var objGenerateButton = objIFrame.Locator("#generateButton");
		await objGenerateButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 60000 });
		await objGenerateButton.ClickAsync();
		var zipButton = objIFrame.Locator("#zipButton");
		await zipButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 120000 });
		Log("Zip button is visible.");
		var generatedImagesDiv = objIFrame.Locator("#cpImages");
		var generatedImages = generatedImagesDiv.Locator("img");
		var imageCount = await generatedImages.CountAsync();
		Log($"Found {imageCount} generated images.");
		Log("=== Exiting ClickGenerateAndWait ===");
	}

    public async Task DownloadImages(CardPenHarvest toReturn, IFrameLocator objIFrame, List<string> cardIds)
    {
        Log("=== Entering DownloadImages ===");
        var generatedImagesDiv = objIFrame.Locator("#cpImages");
        var generatedImages = generatedImagesDiv.Locator("img");
        var generatedCount = await generatedImages.CountAsync();
        Log($"Expecting {cardIds.Count} images, found {generatedCount} img tags.");

        if (generatedCount != cardIds.Count)
        {
            if (cardIds.Count == 1 && generatedCount == 0)
            {
                Log("Detected common card back. No images to download.");
                return;
            }
            var idsStr = string.Join(", ", cardIds);
            throw new ApplicationException($"Mismatch between generated image count ({generatedCount}) and expected card count ({cardIds.Count}). Card IDs: [{idsStr}]");
        }

        for (int i = 0; i < generatedCount; i++)
        {
            var currentCardId = cardIds[i];
            
            try
            {
                var selector = $"#cpImages img:nth-child({i + 1})";
                string script = @"
                    (body, selector) => {
                        return new Promise((resolve, reject) => {
                            const startTime = Date.now();
                            const timeout = 60000;

                            const checkImage = () => {
                                // Query inside the iframe's document context
                                const img = document.querySelector(selector);
                                if (img && img.getAttribute('src') && img.getAttribute('src').startsWith('data:image')) {
                                    resolve(img.getAttribute('src'));
                                } else if (Date.now() - startTime > timeout) {
                                    // Construct a meaningful error message
                                    const imgExists = !!img;
                                    const src = img ? img.getAttribute('src') : 'null';
                                    reject(new Error(`Timeout: Image source for selector '${selector}' did not load in ${timeout}ms. Img found: ${imgExists}, src: '${src}'.`));
                                } else {
                                    setTimeout(checkImage, 250); // Poll every 250ms
                                }
                            };
                            checkImage();
                        });
                    }";

                var iframeBody = objIFrame.Locator("body");
                var imgSrc = await iframeBody.EvaluateAsync<string>(script, selector);

                Log($"Downloaded Card Image src for ID '{currentCardId}'. Length: {imgSrc.Length}");
                toReturn.Images[currentCardId] = imgSrc;
            }
            catch (PlaywrightException ex)
            {
                // This will catch the timeout from the rejected promise and other Playwright errors
                Log($"!!! ERROR: Playwright/JS error for card ID '{currentCardId}'. Exception: {ex.Message}");
                toReturn.Images[currentCardId] = null;
            }
        }
        Log("=== Exiting DownloadImages ===");
    }

	private  CardSetHarvest LoadCardSetHarvest(string jsonHarvestName)
	{
		using var configStream = File.OpenRead(jsonHarvestName);
		var currentHarvest = System.Text.Json.JsonSerializer.Deserialize<CardSetHarvest>(configStream);
		Log($"Loaded Harvest {jsonHarvestName}");
		return currentHarvest;
	}

	   private void Log(string message)
	   {
	       if (Output != null)
	       {
	           Output.WriteLine(message);
	       }
	       else
	       {
	           Logger.Log(message);
	       }
	   }
}