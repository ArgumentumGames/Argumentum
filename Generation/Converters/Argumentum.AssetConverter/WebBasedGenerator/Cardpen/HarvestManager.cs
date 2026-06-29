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

		// Issue #613: collect per-set failures so one set timing out (e.g. a large set under high
		// parallelism) does not abort the whole multi-language harvest. All reachable sets are
		// attempted and persisted; a single aggregate error is raised at the end if any failed.
		var failedSets = new ConcurrentBag<(string cardSet, string language, string error)>();

		var parallelOptionsCardset = new ParallelOptions { MaxDegreeOfParallelism = Config.EnableParallelism? Config.MaxDegreeOfParallelismCardpen : 1 };
		await Parallel.ForEachAsync(targetCardSets, parallelOptionsCardset, async (configCardSet, token) =>
		{
			var targetLanguages = AssetConverterConfig.LocalizationConfig.BuildLanguageList(configCardSet.Translations);
			var parallelOptionsCardsetLanguage = new ParallelOptions { MaxDegreeOfParallelism = Config.EnableParallelism?  Config.MaxDegreeOfParallelismCardpenTranslations : 1 };
			await Parallel.ForEachAsync(targetLanguages, parallelOptionsCardsetLanguage, async (currentLanguage, newToken) =>
			{
				try
				{
					await ProcessLocalizedHarvest(configCardSet, currentLanguage, harvestDictionary, funcBrowser);
				}
				catch (Exception ex) when (Config.ContinueOnHarvestSetFailure)
				{
					// Persist the failure and keep harvesting the other sets (issue #613).
					failedSets.Add((configCardSet.Name, currentLanguage, ex.Message));
					Logger.Log($"[HARVEST-FAILURE] Card set '{configCardSet.Name}' / '{currentLanguage}' failed and was skipped: {ex.Message}", MessageType.Problem);
				}
			});
		});

		if (!failedSets.IsEmpty)
		{
			var summary = string.Join("; ", failedSets.Select(f => $"{f.cardSet}/{f.language}"));
			Logger.Log($"[HARVEST-PARTIAL] {failedSets.Count} card set(s) failed to harvest and were skipped: {summary}. " +
				"All successful harvests were persisted to disk — re-run to collect the missing sets (the cache skips the good ones). " +
				"If large sets time out, set EnableParallelism=false or lower MaxDegreeOfParallelismCardpen (issue #613).", MessageType.Problem);
			throw new ApplicationException(
				$"Harvest completed with {failedSets.Count} failed card set(s): {summary}. " +
				"Successful harvests were persisted; re-run to collect the missing sets (issue #613).");
		}

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
				var currentHarvest = await GenerateHarvestImages(browser, configCardSet, cardSetDocuments, currentLanguage);
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
		// ✅ FIX: Forcer le DPI depuis la config C# pour écraser la valeur du template JSON
		// Cela permet de garantir une résolution cohérente (300 DPI) pour tous les CardSets
		if (cardSetInfo != null && cardSetInfo.Dpi > 0)
		{
			int originalDpi = cardSetDocumentWrapper.CardSetDocument.dpi;
			cardSetDocumentWrapper.CardSetDocument.dpi = cardSetInfo.Dpi;
			Log($"[DPI Override] CardSet '{cardSetDocumentWrapper.CardSetDocument.name}': DPI {originalDpi} -> {cardSetInfo.Dpi}");
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
			// CardPen renders heavy datasets (AR/FA/ZH cards) slowly; the Playwright default
			// operation timeout is only 30s, which causes uncaught TimeoutExceptions that kill
			// the whole Release run. Raise the default for every await on this page to 300s
			// (Fallacies 1408 cards / 8 bunch = 177 images can take >120s; PR #410 raised zipButton to 300s).
			newPage.SetDefaultTimeout(300000);
			LastPageUsed = newPage;
			return newPage;
		}
	}

	private void ReleasePage(IPage page)
	{
		Freepages.Push(page);
	}

	public async Task<CardSetHarvest> GenerateHarvestImages(Func<Task<IBrowser>> browser, CardSetJob configCardSet, (CardSetPayload front, CardSetPayload back) cardSetDocuments, string currentLanguage = null)
	{
		Log("Entering GenerateHarvestImages.");
		var currentHarvest = new CardSetHarvest();
		var page = await GetFreePage(browser);

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
			await page.GotoAsync(cardpenUrl, new PageGotoOptions { Timeout = 120000 });
			Log("Navigation successful.");
			await page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions { Timeout = 120000 });

			var result = await page.EvaluateAsync<System.Text.Json.JsonElement>(
			    "(() => {" +
			    "    const startTime = Date.now();" +
			    "    const timeout = 118000;" +
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

			var faces = await GenerateImages(page, cardSetDocuments.front, configCardSet.Config.FaceCardSetInfo);
			currentHarvest.Faces = faces;

			// Issue #190 phase 1: opportunistic overflow detection on Virtues face cards.
			// Runs while the face DOM is still in the iframe — must happen before the back
			// cardset generation overwrites the iframe content.
			if (configCardSet.Config.FaceCardSetInfo?.DataSet == KnownDataSets.VirtuesTaxonomy)
			{
				try
				{
					var iframe = page.FrameLocator("#cpOutput");
					var report = await OverflowDetector.DetectAsync(iframe, configCardSet.Name, currentLanguage ?? "");
					var qaDir = Path.Combine(AssetConverterConfig.GetBaseTargetDirectory(currentLanguage ?? ""), "QA");
					var reportPath = Path.Combine(qaDir, $"{configCardSet.Name}_overflow_{currentLanguage}.md");
					OverflowDetector.WriteMarkdownReport(report, reportPath);
					Log($"[Overflow] {report.CardsWithOverflowCount}/{report.Cards.Count} cards with overflow. Report: {reportPath}");
				}
				catch (Exception ex)
				{
					// Non-fatal: a detection failure must not break the harvest pipeline.
					Log($"[Overflow] Detection failed: {ex.Message}");
				}
			}

			if (cardSetDocuments.back != null)
			{
				var backs = await GenerateImages(page, cardSetDocuments.back, configCardSet.Config.BackCardSetInfo);
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


	      public async Task<CardPenHarvest> GenerateImages(IPage page, CardSetPayload cardSetDocument, CardSetInfo cardSetInfo)
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

	              // Garde anti-race (Release / GitHub Pages) : le global `cardpen` est défini par
	              // main.js (main.js:22), qui est le DERNIER script de la page. Le gate amont
	              // (attente de l'iframe #cpOutput) ne vérifie que la présence d'un élément HTML
	              // statique, pas l'exécution de main.js. En local IIS (Debug) la latence ~0 masque
	              // la course ; servi depuis GitHub Pages (Release) elle peut être perdue → l'appel
	              // ci-dessous lève "ReferenceError: cardpen is not defined". Attendre explicitement
	              // que l'API cardpen soit prête rend le harvest robuste quelle que soit la source.
	              await page.WaitForFunctionAsync(
	                  "() => typeof cardpen !== 'undefined' && cardpen.form && cardpen.write",
	                  null,
	                  new PageWaitForFunctionOptions { Timeout = 120000 });

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
	              await generateButtonLocator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 120000 });
	              Log("Generate button found, iframe is ready.");
	              
	              // Appeler generateImages() dans le contexte de l'iframe pour démarrer la génération
	              Log("Calling generateImages() in iframe context...");
	              await iframe.EvaluateAsync("generateImages()");
	              Log("generateImages() called, waiting for completion...");

	              // Attendre que le bouton ZIP devienne visible (signal que la génération est terminée)
	              // NOTE: 300s (5 min) nécessaire pour les gros CardSets (Fallacies 1408 cartes × plusieurs langues)
	              var zipButtonLocator = iframe.Locator("#zipButton");
	              await zipButtonLocator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 300000 });
	              Log("Image generation process completed successfully.");

	              // ✅ FIX: Capturer le DPI depuis CardPen après la génération
	              // Le DPI utilisé pour générer les images est stocké dans cardpen.form.data.dpi
	              try
	              {
	                  var capturedDpi = await iframe.EvaluateAsync<int>("() => { const d = cardpen.form.get().data; return d && d.dpi ? parseInt(d.dpi, 10) : 0; }");
	                  Log($"[DPI] Captured DPI from CardPen: {capturedDpi}");
	                  if (capturedDpi > 0)
	                  {
	                      toReturn.Dpi = capturedDpi;
	                  }
	                  else
	                  {
	                      // Fallback: utiliser le DPI de la configuration si CardPen ne retourne rien
	                  }
	              }
	              catch (Exception dpiEx)
	              {
	                  Log($"[DPI] Warning: Could not capture DPI from CardPen: {dpiEx.Message}");
	              }

	              var objIFrame = page.FrameLocator("#cpOutput");
	              List<string> cardIds;
	              int expectedImageCount;

	              // ✅ FIX: Gérer les CardSets sans CsvType (comme Scenarii, Rules Back)
	              // Pour ces CardSets, on essaie d'extraire les IDs depuis la colonne 'card' du CSV
	              // Si pas de colonne 'card', on génère des IDs synthétiques
	              if (cardSetDocument.CsvType == null)
	              {
	                  // Essayer d'extraire les IDs depuis la colonne 'card' du CSV
	                  var csvContent = cardSetDocument.CardSetDocument.csv;
	                  cardIds = ExtractCardIdsFromCsv(csvContent);

	                  if (cardIds.Count > 0)
	                  {
	                      Log($"[No CsvType] Extrait {cardIds.Count} IDs depuis le CSV: {string.Join(", ", cardIds.Take(3))}...");
	                      expectedImageCount = cardIds.Count;
	                  }
	                  else
	                  {
	                      // Fallback: détecter le nombre d'images générées et générer des IDs synthétiques
	                      Log($"[No CsvType] Pas de colonne 'card' - détection des images générées...");
	                      var generatedImagesDiv = objIFrame.Locator("#cpImages");
	                      var generatedImages = generatedImagesDiv.Locator("img");
	                      var generatedCount = await generatedImages.CountAsync();
	                      Log($"[No CsvType] Détecté {generatedCount} images générées");

	                      // Générer des IDs synthétiques pour les images
	                      cardIds = Enumerable.Range(1, generatedCount).Select(i => $"card_{i:D3}").ToList();
	                      expectedImageCount = generatedCount;
	                  }
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
	                  cardIds = cardData.Cast<ICsvBase>().Select(c => c.GetId()).Where(id => !string.IsNullOrEmpty(id)).ToList();

	                  // ✅ FIX: Calculer le nombre d'images attendues en tenant compte de rscount/rsstyle
	                  // Avec rsstyle="bunch" et rscount>=N, CardPen génère ceil(cardIds.Count/rscount) images
	                  var rscount = cardSetDocument.CardSetDocument.rscount;
	                  var rsstyle = cardSetDocument.CardSetDocument.rsstyle;
	                  expectedImageCount = ComputeExpectedImageCount(cardIds.Count, rscount, rsstyle);

	                  if (rscount > 1 && !string.IsNullOrEmpty(rsstyle) &&
	                      (rsstyle == "bunch" || rsstyle == "cycle" || rsstyle == "random"))
	                  {
	                      Log($"[rscount adjustment] rscount={rscount}, rsstyle={rsstyle}, original={cardIds.Count} -> expected={expectedImageCount}");
	                  }
	              }

	              // ✅ RESTORED from Golden Master: Extract card names from .cardName elements
	              // This is critical for category-based back matching (e.g., Scenarii backs keyed by catégorie)
	              // The golden master used .cardName text as harvest keys, enabling:
	              // - Multiple batches with same category to overwrite → only 7 unique backs
	              // - Face-to-back matching via faceName.Contains(backName)
	              var cardNames = await ExtractCardNames(objIFrame);
	              if (cardNames.Count == expectedImageCount && cardNames.Any(n => !Regex.IsMatch(n, @"^\d{3}$")))
	              {
	                  // Card names from .cardName are available and meaningful (not just indices)
	                  Log($"[ExtractCardNames] Using {cardNames.Count} card names from .cardName: {string.Join(", ", cardNames.Take(5))}...");
	                  await DownloadImages(toReturn, objIFrame, cardNames, expectedImageCount);
	              }
	              else
	              {
	                  // Fallback to cardIds when .cardName is not available or doesn't match
	                  Log($"[ExtractCardNames] Card names not usable (count={cardNames.Count}, expected={expectedImageCount}). Using cardIds.");
	                  await DownloadImages(toReturn, objIFrame, cardIds, expectedImageCount);
	              }
	          }
	          catch (PlaywrightException ex)
	          {
	              Log($"!!! PlaywrightException in GenerateImages: {ex.Message}");
	              Log($"Problematic CSV content:\n{cardSetDocument.CardSetDocument.csv}");
	              throw;
	          }
	          return toReturn;
	       }

    /// <summary>
    /// Computes the number of images CardPen will generate for a card set, given the source row
    /// count, the rscount (rows grouped per card) and the rsstyle. Mirrors CardPen's client-side
    /// rowset grouping: with rsstyle "bunch"/"cycle"/"random" and rscount &gt; 1, CardPen groups
    /// rscount rows onto each card and emits ceil(cardCount/rscount) images; otherwise one image per
    /// row. This is the formula documented in CLAUDE.md ("Lecons Apprises -- Calcul du nombre
    /// d'images attendues") and was previously inlined in the Playwright flow with zero unit
    /// coverage. Pure and deterministic -- extracted (output-neutral) so the CardPen-mirroring
    /// contract is unit-testable outside the browser. See HarvestManagerExpectedImageCountTests.
    /// </summary>
    public static int ComputeExpectedImageCount(int cardCount, int rscount, string rsstyle)
    {
        if (rscount > 1 && !string.IsNullOrEmpty(rsstyle) &&
            (rsstyle == "bunch" || rsstyle == "cycle" || rsstyle == "random"))
        {
            return (int)Math.Ceiling((double)cardCount / rscount);
        }
        return cardCount;
    }

    /// <summary>
    /// RESTORED from Golden Master (commit 0087f0ec).
    /// Extracts card names from .cardName elements in the rendered CardPen HTML.
    /// This enables category-based back matching for Scenarii and other card sets
    /// where the back design depends on a field from the CSV data (e.g., catégorie).
    /// </summary>
    public async Task<List<string>> ExtractCardNames(IFrameLocator objIFrame)
    {
        var cardNames = new List<string>();
        var cardsHtml = objIFrame.Locator("card");
        var cardCount = await cardsHtml.CountAsync();

        for (var idxCard = 0; idxCard < cardCount; idxCard++)
        {
            var strCardName = idxCard.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0');
            var cardElement = cardsHtml.Nth(idxCard);
            var cardCssName = cardElement.Locator(".cardName");
            if (await cardCssName.CountAsync() > 0)
            {
                var currentCard = cardCssName.Nth(0);
                var textContent = await currentCard.TextContentAsync();
                if (!string.IsNullOrEmpty(textContent))
                {
                    strCardName = textContent.Trim('-').Trim();
                }
            }
            cardNames.Add(strCardName);
        }
        Log($"[ExtractCardNames] Extracted {cardNames.Count} card names from {cardCount} card elements");
        return cardNames;
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
                            const timeout = 120000;

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

	/// <summary>
	/// Extrait les IDs de carte depuis la colonne 'card' d'un CSV.
	/// Utilisé pour les CardSets sans CsvType défini (comme les backs de cartes).
	/// </summary>
	/// <param name="csvContent">Le contenu CSV à parser</param>
	/// <returns>Liste des IDs de carte extraits, ou liste vide si pas de colonne 'card'</returns>
	private List<string> ExtractCardIdsFromCsv(string csvContent)
	{
		var cardIds = new List<string>();
		if (string.IsNullOrEmpty(csvContent))
		{
			return cardIds;
		}

		try
		{
			// Parser le CSV manuellement pour extraire la colonne 'card'
			var lines = csvContent.Split('\n');
			if (lines.Length < 2)
			{
				return cardIds; // Pas assez de lignes (header + data)
			}

			// Trouver l'index de la colonne 'card'
			var headerLine = lines[0].Trim();
			var headers = ParseCsvLine(headerLine);
			int cardIndex = -1;
			for (int i = 0; i < headers.Count; i++)
			{
				if (headers[i].Equals("card", StringComparison.OrdinalIgnoreCase))
				{
					cardIndex = i;
					break;
				}
			}

			if (cardIndex < 0)
			{
				return cardIds; // Pas de colonne 'card' trouvée
			}

			// Extraire les valeurs de la colonne 'card' pour chaque ligne de données
			for (int i = 1; i < lines.Length; i++)
			{
				var line = lines[i].Trim();
				if (string.IsNullOrEmpty(line))
				{
					continue;
				}

				var values = ParseCsvLine(line);
				if (values.Count > cardIndex)
				{
					var cardValue = values[cardIndex].Trim();
					if (!string.IsNullOrEmpty(cardValue))
					{
						cardIds.Add(cardValue);
					}
				}
			}

			Log($"[ExtractCardIdsFromCsv] Trouvé {cardIds.Count} IDs: {string.Join(", ", cardIds.Take(5))}{(cardIds.Count > 5 ? "..." : "")}");
		}
		catch (Exception ex)
		{
			Log($"[ExtractCardIdsFromCsv] Erreur lors du parsing CSV: {ex.Message}");
		}

		return cardIds;
	}

	/// <summary>
	/// Parse une ligne CSV en gérant les guillemets et les virgules à l'intérieur des valeurs
	/// </summary>
	private List<string> ParseCsvLine(string line)
	{
		var values = new List<string>();
		var currentValue = new System.Text.StringBuilder();
		bool inQuotes = false;

		for (int i = 0; i < line.Length; i++)
		{
			char c = line[i];

			if (c == '"')
			{
				if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
				{
					// Guillemet échappé
					currentValue.Append('"');
					i++;
				}
				else
				{
					// Toggle l'état des guillemets
					inQuotes = !inQuotes;
				}
			}
			else if (c == ',' && !inQuotes)
			{
				// Fin de la valeur
				values.Add(currentValue.ToString().Trim());
				currentValue.Clear();
			}
			else
			{
				currentValue.Append(c);
			}
		}

		// Ajouter la dernière valeur
		values.Add(currentValue.ToString().Trim());

		return values;
	}
}