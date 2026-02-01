using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
	public bool KeepBrowserOpen { get; set; } = true;
	
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
						Headless = true, // ✅ Mode headless pour exécution sans encombrer le bureau
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
			usedCardSet.Config = Config.CardSets.FirstOrDefault(c => c.Name == usedCardSet.Name);
		}
		Log($"Found {targetCardSets.Length} target card sets to process.");
		return targetCardSets;
	}

	public async Task LoadHarvestsAsync(CardSetJob[] targetCardSets, ParallelOptions parallelOptionsLoading, ConcurrentDictionary<(string cardsetName, string language), Func<CardSetHarvest>> harvestDictionary)
	{
		await Parallel.ForEachAsync(targetCardSets, parallelOptionsLoading, (configCardSet, token) =>
		{
			if (configCardSet.Config != null)
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
		Logger.Log($"[PrepareCardSetDocuments] Processing CardSet: {configCardSet?.Name}, Language: {currentLanguage}");

		if (configCardSet == null) throw new ArgumentNullException(nameof(configCardSet));
		if (configCardSet.Config == null) throw new NullReferenceException($"configCardSet.Config is null for {configCardSet.Name}");

		(CardSetPayload front, CardSetPayload back) cardSetDocuments;
		if (currentLanguage == AssetConverterConfig.LocalizationConfig.DefaultLanguage)
		{
			if (configCardSet.Config.FaceCardSetInfo == null)
			{
				Logger.Log($"[WARNING] FaceCardSetInfo is null for {configCardSet.Name}. Skipping front card generation.");
			}
			var frontCardSetDocument = configCardSet.Config.FaceCardSetInfo != null
				? await configCardSet.Config.FaceCardSetInfo.GetCardSetDocument(AssetConverterConfig)
				: null;

			if (configCardSet.Config.BackCardSetInfo == null)
			{
				Logger.Log($"[WARNING] BackCardSetInfo is null for {configCardSet.Name}. Skipping back card generation.");
			}
			var backCardSetDocument = configCardSet.Config.BackCardSetInfo != null
				? await configCardSet.Config.BackCardSetInfo.GetCardSetDocument(AssetConverterConfig)
				: null;
				
			cardSetDocuments = (frontCardSetDocument, backCardSetDocument);
		}
		else
		{
			cardSetDocuments = await AssetConverterConfig.LocalizationConfig.TranslateCardSet(configCardSet.Config, (AssetConverterConfig.LocalizationConfig.DefaultLanguage, currentLanguage), AssetConverterConfig);
		}
		if (cardSetDocuments.front != null)
		{
			await UpdateCardSetDocumentInfo(cardSetDocuments.front, configCardSet.Config.FaceCardSetInfo);
		}
		if (cardSetDocuments.back != null)
		{
			await UpdateCardSetDocumentInfo(cardSetDocuments.back, configCardSet.Config.BackCardSetInfo);
		}
		return cardSetDocuments;
	}

	private async Task UpdateCardSetDocumentInfo(CardSetPayload cardSetDocumentWrapper, CardSetInfo cardSetInfo)
	{
		if (cardSetInfo != null && !cardSetInfo.SkipDataUpdate && !string.IsNullOrEmpty(cardSetInfo.DataSet))
		{
			Log("Dumping AssetConverterConfig and cardSetInfo before First()");
			Logger.LogJson(System.Text.Json.JsonSerializer.Serialize(AssetConverterConfig));
			Log($"Searching for DataSet with name: '{cardSetInfo.DataSet}'");
			var dataSet = AssetConverterConfig.DataSets.FirstOrDefault(ds => ds.Name == cardSetInfo.DataSet);
			if (dataSet != null)
			{
				string csvContent;
				if (!string.IsNullOrEmpty(cardSetInfo.CsvFilterField) && cardSetInfo.CsvFilterValues.Count > 0)
				{
					csvContent = await dataSet.GetContent(AssetConverterConfig.UseDebugParams, ",", "", cardSetInfo.CsvFilterField, cardSetInfo.CsvFilterValues);
				}
				else
				{
					csvContent = await dataSet.GetContent(AssetConverterConfig.UseDebugParams);
				}
				if (!string.IsNullOrEmpty(csvContent) && cardSetDocumentWrapper?.CardSetDocument != null)
				{
					// Golden Master (avril 2024): passer le CSV sans modification
					// PapaParse gère correctement les newlines dans les cellules entre guillemets
					cardSetDocumentWrapper.CardSetDocument.csv = csvContent;
				}
				cardSetDocumentWrapper.CsvType = dataSet.CsvType;
			}
		}
		if (cardSetInfo != null && cardSetInfo.Dpi > 0)
		{
			cardSetDocumentWrapper.CardSetDocument.dpi = cardSetInfo.Dpi;
		}
		// ✅ FIX: Ne modifier rscount QUE si RowsetNb est explicitement défini dans la config C#
		// Sinon, conserver la valeur du template JSON (ex: Memo a rscount=200, rsstyle="bunch")
		// Note: Le comportement de référence avril 2024 ne forçait PAS rscount à 0
		if (cardSetInfo != null && cardSetInfo.RowsetNb > 0)
		{
			cardSetDocumentWrapper.CardSetDocument.rscount = cardSetInfo.RowsetNb;
			Console.WriteLine($"[DEBUG rscount] CardSet '{cardSetDocumentWrapper.CardSetDocument.name}': rscount overridden to {cardSetInfo.RowsetNb} from C# config");
		}
		else
		{
			Console.WriteLine($"[DEBUG rscount] CardSet '{cardSetDocumentWrapper.CardSetDocument.name}': keeping template rscount={cardSetDocumentWrapper.CardSetDocument.rscount}");
		}
		if (cardSetInfo != null && !string.IsNullOrEmpty(cardSetInfo.CardSize))
		{
			cardSetDocumentWrapper.CardSetDocument.csize = cardSetInfo.CardSize;
		}
	}

	/// <summary>
	/// Nettoie le contenu CSV en remplaçant les cellules vides par des chaînes vides
	/// pour éviter les valeurs undefined qui cassent le parser Markdown de CardPen
	/// </summary>
	private string CleanCsvContent(string csvContent)
	{
		if (string.IsNullOrEmpty(csvContent))
		{
			return csvContent;
		}

		var lines = csvContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
		var cleanedLines = new List<string>();

		foreach (var line in lines)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				cleanedLines.Add(line);
				continue;
			}

			// Parser CSV simple : séparer par virgules en tenant compte des guillemets
			var cells = new List<string>();
			var currentCell = new System.Text.StringBuilder();
			bool inQuotes = false;

			for (int i = 0; i < line.Length; i++)
			{
				char c = line[i];

				if (c == '"')
				{
					inQuotes = !inQuotes;
					currentCell.Append(c);
				}
				else if (c == ',' && !inQuotes)
				{
					// Fin de cellule : nettoyer et ajouter
					var cellValue = currentCell.ToString().Trim();
					// Remplacer les cellules vides par des chaînes vides entre guillemets
					cells.Add(string.IsNullOrWhiteSpace(cellValue) || cellValue == "\"\"" ? "\"\"" : cellValue);
					currentCell.Clear();
				}
				else
				{
					currentCell.Append(c);
				}
			}

			// Ajouter la dernière cellule
			var lastCellValue = currentCell.ToString().Trim();
			cells.Add(string.IsNullOrWhiteSpace(lastCellValue) || lastCellValue == "\"\"" ? "\"\"" : lastCellValue);

			cleanedLines.Add(string.Join(",", cells));
		}

		return string.Join("\n", cleanedLines);
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

		void Page_Console(object sender, IConsoleMessage msg)
		{
			Log("--- CONSOLE MESSAGE RECEIVED ---");
			Log($"Type: {msg.Type}");
			Log($"Text: {msg.Text}");
			Log($"Location: {msg.Location}");
			Log("--- END CONSOLE MESSAGE ---");
		}
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
	          if (cardSetDocument?.CardSetDocument == null)
	          {
	              return toReturn;
	          }
	          
	          // Sanitize data to prevent JS errors
	          cardSetDocument.CardSetDocument.csv ??= string.Empty;
	          cardSetDocument.CardSetDocument.mustache ??= string.Empty;
	          cardSetDocument.CardSetDocument.css ??= string.Empty;
	          
	          try
	          {
	              // Étape 1 & 2 : Injection des données et déclenchement du rendu de l'iframe
	              Log("Injecting data and triggering iframe render...");
	              
	              // ✅ CORRECTION BUG #4: Réécrire les chemins d'images relatifs en URLs absolues IIS
	              // Les chemins relatifs comme "../../Cards/..." ne fonctionnent pas quand CardPen est servi depuis IIS
	              RewriteImagePathsToAbsoluteUrls(cardSetDocument.CardSetDocument);
	              
	              // [DEBUG] Compter les lignes CSV réelles
	              var csv = cardSetDocument.CardSetDocument.csv ?? string.Empty;
	              var realCsvLines = csv.Split(new[] { "\n" }, StringSplitOptions.None).Length;
	              Log($"[DEBUG] CSV real newlines count: {realCsvLines}");
	              Log($"[DEBUG] CSV first 500 chars: {csv.Substring(0, Math.Min(500, csv.Length))}");
	              
	              // ✅ CORRECTION BUG #3: Sérialiser normalement SANS ignorer les valeurs par défaut
	              // TOUJOURS inclure rscount dans le JSON pour écraser les valeurs précédentes du formulaire
	              var cardsJson = System.Text.Json.JsonSerializer.Serialize(cardSetDocument.CardSetDocument);
	              
	              // [DEBUG] Log JSON first chars to inspect escaping
	              Log($"[DEBUG] JSON first 500 chars: {cardsJson.Substring(0, Math.Min(500, cardsJson.Length))}");
	              
	              // ✅ CORRECTION FINALE: Passer le JSON comme argument de fonction
	              // Cela évite les problèmes d'échappement multiples des string literals JavaScript
	              await page.EvaluateAsync("(jsonString) => cardpen.form.set(JSON.parse(jsonString))", cardsJson);
	              await page.EvaluateAsync("cardpen.write.generate(cardpen.form.get(), 'image')");

	              // Étape 3 : Obtenir une référence sur l'iframe
	              Log("Getting iframe handle...");
	              var iframeElement = await page.QuerySelectorAsync("#cpOutput");
	              if (iframeElement == null)
	              {
	                  throw new ApplicationException("Could not find the #cpOutput iframe element.");
	              }
	              var iframe = await iframeElement.ContentFrameAsync();
	              if (iframe == null)
	              {
	                  throw new ApplicationException("Could not find or access the content of the #cpOutput iframe.");
	              }

	              // Étape 4 : Attendre que l'iframe soit prêt et appeler generateImages()
	              // Note: frame.js est déjà chargé par CardPen via main.js ligne 1199
	              Log("Waiting for iframe to be ready...");
	              
	              // Attendre que le bouton Generate Images soit présent (signal que l'iframe est chargé)
	              var generateButtonLocator = iframe.Locator("#generateButton");
	              await generateButtonLocator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 30000 });
	              Log("Generate button found, iframe is ready.");
	              
	              // Appeler generateImages() dans le contexte de l'iframe pour démarrer la génération
	              Log("Calling generateImages() in iframe context...");
	              await iframe.EvaluateAsync("generateImages()");
	              Log("generateImages() called, waiting for completion...");
	              
	              // Attendre que le bouton ZIP devienne visible (signal que la génération est terminée)
	              var zipButtonLocator = iframe.Locator("#zipButton");
	              await zipButtonLocator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 120000 });
	              Log("Image generation process completed successfully.");

	              var objIFrame = page.FrameLocator("#cpOutput");
	              List<string> cardIds;
	              int expectedImageCount;

	              // ✅ FIX SCENARII: Gérer les CardSets sans CsvType (comme Scenarii)
	              // Pour ces CardSets, on génère des IDs synthétiques basés sur le nombre d'images
	              if (cardSetDocument.CsvType == null)
	              {
	                  Log($"[No CsvType] CardSet sans type CSV défini - détection des images générées...");
	                  var generatedImagesDiv = objIFrame.Locator("#cpImages");
	                  var generatedImages = generatedImagesDiv.Locator("img");
	                  var generatedCount = await generatedImages.CountAsync();
	                  Log($"[No CsvType] Détecté {generatedCount} images générées");

	                  // Générer des IDs synthétiques pour les images
	                  cardIds = Enumerable.Range(1, generatedCount).Select(i => $"card_{i:D3}").ToList();
	                  expectedImageCount = generatedCount;
	              }
	              else
	              {
	                  var csvType = cardSetDocument.CsvType;
	                  var classMapType = csvType.Assembly.GetType($"{csvType.FullName}ClassMap");
	                  var csvBaseType = typeof(CsvBase<,>);
	                  var genericCsvBaseType = csvBaseType.MakeGenericType(csvType, classMapType);
	                  var loadMethod = genericCsvBaseType.GetMethod("LoadFromContent", new[] { typeof(string) });
	                  // ✅ CORRECTION: Dé-échapper les \\n pour CsvHelper
	                  // Le CSV a été échappé (ligne 216) pour PapaParse JavaScript
	                  // Maintenant il faut restaurer les vraies newlines pour CsvHelper C#
	                  var csvContentUnescaped = cardSetDocument.CardSetDocument.csv.Replace("\\n", "\n");
	                  var cardData = (System.Collections.IEnumerable)loadMethod.Invoke(null, new object[] { csvContentUnescaped });
	                  cardIds = cardData.Cast<ICsvBase>().Select(c => c.GetId()).ToList();

	                  // ✅ FIX: Calculer le nombre d'images attendues en tenant compte de rscount/rsstyle
	                  // Avec rsstyle="bunch" et rscount>=N, CardPen génère ceil(cardIds.Count/rscount) images
	                  var rscount = cardSetDocument.CardSetDocument.rscount;
	                  var rsstyle = cardSetDocument.CardSetDocument.rsstyle;
	                  expectedImageCount = cardIds.Count;

	                  if (rscount > 1 && !string.IsNullOrEmpty(rsstyle) &&
	                      (rsstyle == "bunch" || rsstyle == "cycle" || rsstyle == "random"))
	                  {
	                      expectedImageCount = (int)Math.Ceiling((double)cardIds.Count / rscount);
	                      Log($"[rscount adjustment] rscount={rscount}, rsstyle={rsstyle}, original={cardIds.Count} -> expected={expectedImageCount}");
	                  }
	              }

	              await DownloadImages(toReturn, objIFrame, cardIds, expectedImageCount);
	          }
	          catch (PlaywrightException ex)
	          {
	              Log($"!!! PlaywrightException in GenerateImages: {ex.Message}");
	              Log($"Problematic CSV content:\n{cardSetDocument.CardSetDocument.csv}");
	              throw;
	          }
	          return toReturn;
	       }

    public async Task DownloadImages(CardPenHarvest toReturn, IFrameLocator objIFrame, List<string> cardIds, int expectedImageCount = -1)
    {
        Log("=== Entering DownloadImages ===");
        var generatedImagesDiv = objIFrame.Locator("#cpImages");
        var generatedImages = generatedImagesDiv.Locator("img");
        var generatedCount = await generatedImages.CountAsync();

        // ✅ FIX: Utiliser expectedImageCount si fourni, sinon cardIds.Count
        var expectedCount = expectedImageCount > 0 ? expectedImageCount : cardIds.Count;
        Log($"Expecting {expectedCount} images (cardIds={cardIds.Count}, expectedImageCount={expectedImageCount}), found {generatedCount} img tags.");

        if (generatedCount != expectedCount)
        {
            if (cardIds.Count == 1 && generatedCount == 0)
            {
                Log("Detected common card back. No images to download.");
                return;
            }
            // ✅ FIX: Accepter 1 image si c'est un template "bunch" (rscount > cardIds)
            if (expectedCount == 1 && generatedCount == 1)
            {
                Log("Single bunched image generated as expected.");
            }
            else
            {
                var idsStr = string.Join(", ", cardIds);
                throw new ApplicationException($"Mismatch between generated image count ({generatedCount}) and expected card count ({expectedCount}). Card IDs: [{idsStr}]");
            }
        }

        for (int i = 0; i < generatedCount; i++)
        {
            var currentCardId = cardIds[i];
            Log($"Processing card ID: {currentCardId}");
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

 /// <summary>
 /// Réécrit les chemins d'images relatifs en URLs absolues vers GitHub.
 /// Les chemins relatifs comme "../../Cards/...", "../Cards/...", "Fallacies/...", "Scenarii/..." ne fonctionnent pas
 /// quand CardPen est servi depuis IIS ou GitHub Pages.
 /// </summary>
 private void RewriteImagePathsToAbsoluteUrls(CardSetDocument cardSetDocument)
 {
     if (cardSetDocument == null || string.IsNullOrEmpty(cardSetDocument.mustache))
     {
         return;
     }

     // URL de base GitHub pour les assets (sans /Cards/ pour éviter duplication)
     // La regex capture le chemin complet incluant "Cards/" si présent
     const string githubBaseUrl = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/";
     
     // Regex pour trouver les chemins relatifs d'images dans les attributs src
     // Correspond à: src="../../Cards/...", src="../Cards/...", src="Fallacies/...", src="Scenarii/..."
     // NOTE: Les variables Handlebars comme {{path}} seront remplacées par les valeurs réelles lors du rendu,
     // donc nous remplaçons les chemins relatifs AVANT le rendu Handlebars.
     var relativePathRegex = new Regex(@"src=""(\.\./)+((?:Fallacies|Scenarii|Cards)/[^""]+)""", RegexOptions.Compiled);
     
     // Remplacer les chemins relatifs par des URLs absolues vers GitHub
     cardSetDocument.mustache = relativePathRegex.Replace(cardSetDocument.mustache, match =>
     {
         var relativePath = match.Groups[1].Value; // Le chemin relatif complet (ex: "../../Cards/Scenarii/Assets/...")
         var assetPath = match.Groups[2].Value; // Le chemin après "Cards/" (ex: "Scenarii/Assets/...")
         var absoluteUrl = $"{githubBaseUrl}{assetPath}";
         Log($"Rewriting image path: {relativePath} -> {absoluteUrl}");
         return $"src=\"{absoluteUrl}\"";
     });
     
     Log("Image paths rewritten to absolute GitHub URLs.");
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

	private string GetProjectRoot()
	{
	    var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
	    while (currentDir != null && currentDir.Name != "Argumentum")
	    {
	        currentDir = currentDir.Parent;
	    }
	    return currentDir?.FullName ?? throw new DirectoryNotFoundException("Could not find the project root directory 'Argumentum'.");
	}
}