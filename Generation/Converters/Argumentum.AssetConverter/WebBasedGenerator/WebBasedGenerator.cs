using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using ExtendedXmlSerializer.Core.Sources;
using ImageMagick;
using Microsoft.Playwright;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
//using Svg.Skia;
using Utf8Json;


namespace Argumentum.AssetConverter
{
	public class WebBasedGenerator
	{

		private Stopwatch sw;
		public WebBasedGeneratorConfig Config { get; set; }

		public WebBasedGenerator(WebBasedGeneratorConfig config, Stopwatch objSw)
		{
			Config = config;
			sw = objSw;

		}


		public void Run()
		{
			sw = Stopwatch.StartNew();
			//var harvestDictionary = HarvestImages();
			var harvestDictionary = Task.Run(async () => await HarvestImages()).Result;
			var docImages = GenerateDocumentImages(harvestDictionary);

			GenerateCardSetDocuments(docImages);
			//Console.WriteLine($"Generation finished. Total duration: {sw.Elapsed}");

		}

		

		async Task<Dictionary<(string cardsetName, string language), CardSetHarvest>> HarvestImages()
		{

			Dictionary<(string cardsetName, string language), CardSetHarvest> harvestDictionary;


			harvestDictionary = new Dictionary<(string cardsetName, string language), CardSetHarvest>();
			var targetCardSets = Config.Documents
				.Where(d => d.Enabled)
				.SelectMany(d => d.CardSets.Select(dc => new CardSetJob { Name = dc.CardSetName, Translations = d.Translations} ))
				.Distinct(CardSetJob.Comparer)
				.ToArray();
			foreach (var usedCardSet in targetCardSets)
			{
				usedCardSet.Config = Config.CardSets.First(c => c.Name == usedCardSet.Name);
			}

			

			foreach (var configCardSet in targetCardSets)
			{
				var targetlanguages = new List<string>(new[] { Config.LocalizationConfig.DefaultLanguage });
				if (Config.LocalizationConfig.Enabled)
				{
					targetlanguages.AddRange(configCardSet.Translations.Select(t=>t.targetLanguage));
				}

				foreach (var currentLanguage in targetlanguages)
				{

					var jsonHarvestName = configCardSet.Config.GetHarvestSerializationName(Config, currentLanguage);
					if (File.Exists(jsonHarvestName))
					{
						using var configStream = File.OpenRead(jsonHarvestName);
						var currentHarvest = JsonSerializer.Deserialize<CardSetHarvest>(configStream);
						Console.WriteLine($"Loaded Harvest {jsonHarvestName}: {sw.Elapsed}");
						harvestDictionary[(configCardSet.Name, currentLanguage)] = currentHarvest;
					}
				}

			}

			if (harvestDictionary.Count < targetCardSets.Count())
			{



				var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
				if (exitCode != 0)
				{
					throw new Exception($"Playwright exited with code {exitCode}");
				}

				using var playwright = await Playwright.CreateAsync();
				await using IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
				{
					Headless = false,
					//SlowMo = 50,
				});


				try
				{
					
					var page = await browser.NewPageAsync();
					await page.GotoAsync(Config.CardpenUrl);
					await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
					//await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

					Thread.Sleep(TimeSpan.FromSeconds(2));

					foreach (var configCardSet in targetCardSets)
					{

						var targetLanguages = new List<string>(new[] { Config.LocalizationConfig.DefaultLanguage });
						if (Config.LocalizationConfig.Enabled)
						{
							targetLanguages.AddRange(configCardSet.Translations.Select(t => t.targetLanguage));
						}

						foreach (var currentLanguage in targetLanguages)
						{

							if (!harvestDictionary.ContainsKey((configCardSet.Name, currentLanguage)))
							{
								var currentHarvest = new CardSetHarvest();

								(CardSetPayload front, CardSetPayload back) cardSetDocuments;

								//Optionally localize templates

								if (currentLanguage == Config.LocalizationConfig.DefaultLanguage)
								{
									var frontCardSetDocument = await configCardSet.Config.FaceCardSetInfo.GetCardSetDocument();
									var backCardSetDocument = await configCardSet.Config.BackCardSetInfo.GetCardSetDocument();
									cardSetDocuments = (frontCardSetDocument, backCardSetDocument);
								}
								else
								{
									cardSetDocuments = await Config.LocalizationConfig.TranslateCardSet(configCardSet.Config,
										(Config.LocalizationConfig.DefaultLanguage, currentLanguage));
								}

								//Optionally update data


								if (!configCardSet.Config.FaceCardSetInfo.SkipDataUpdate && !string.IsNullOrEmpty(configCardSet.Config.FaceCardSetInfo.DataSet))
								{
									var dataSet = Config.DataSets.First(ds =>
										ds.Name == configCardSet.Config.FaceCardSetInfo.DataSet);
									cardSetDocuments.front.CardSetDocument.csv = await dataSet.GetContent();
								}

								if (cardSetDocuments.back!=null && !configCardSet.Config.BackCardSetInfo.SkipDataUpdate && !string.IsNullOrEmpty(configCardSet.Config.FaceCardSetInfo.DataSet))
								{
									var dataSet = Config.DataSets.First(ds =>
										ds.Name == configCardSet.Config.BackCardSetInfo.DataSet);
									cardSetDocuments.back.CardSetDocument.csv = await dataSet.GetContent();
								}


								var faces = await GenerateImages(page, cardSetDocuments.front, configCardSet.Config.FaceCardSetInfo.PauseForEdits);
								currentHarvest.Faces = faces;
								if (cardSetDocuments.back != null)
								{
									var backs = await GenerateImages(page, cardSetDocuments.back, configCardSet.Config.BackCardSetInfo.PauseForEdits);
									currentHarvest.Backs = backs;
								}

								var jsonHarvestName = configCardSet.Config.GetHarvestSerializationName(Config, currentLanguage);
								var strNewConfig = JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(currentHarvest));
								await File.WriteAllTextAsync(jsonHarvestName, strNewConfig);
								Console.WriteLine($"Serialized Harvest {jsonHarvestName}: {sw.Elapsed}");


								harvestDictionary[(configCardSet.Name, currentLanguage)] = currentHarvest;
							}


						}


						


					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
				
			}

			return harvestDictionary;
		}



		Dictionary<(CardSetGenerationDocument document, string language), List<CardImages>> GenerateDocumentImages(Dictionary<(string cardsetName, string language), CardSetHarvest> harvestDictionary)
		{
			var toReturn = new Dictionary<(CardSetGenerationDocument document, string language), List<CardImages>>();

			foreach (var configDocument in Config.Documents.Where(d => d.Enabled))
			{

				var targetLanguages = new List<string>(new[] { Config.LocalizationConfig.DefaultLanguage });
				if (Config.LocalizationConfig.Enabled)
				{
					targetLanguages.AddRange(configDocument.Translations.Select(t => t.targetLanguage));
				}

				foreach (var currentLanguage in targetLanguages)
				{
					List<CardImages> targetList;

					if (!toReturn.TryGetValue((configDocument, currentLanguage), out targetList))
					{
						targetList = new List<CardImages>();
						toReturn[(configDocument, currentLanguage)] = targetList;
					}

					foreach (var configCardSet in configDocument.CardSets)
					{
						var documentLocalizedName = LocalizationConfig.GetLocalizedFileName(configDocument.DocumentName,
							Config.LocalizationConfig.DefaultLanguage, currentLanguage);
						Console.WriteLine($"Generating card set images for {documentLocalizedName} - {configCardSet.CardSetName}: {sw.Elapsed}");



						var currentHarvest = harvestDictionary[(configCardSet.CardSetName, currentLanguage)];
						var backImages = new Dictionary<string, MagickImage>();
						if (currentHarvest.Backs != null)
						{
							foreach (var currentHarvestBack in currentHarvest.Backs.Images)
							{
								var backName = $"{currentHarvestBack.Key.ToLowerInvariant()}";
								var backImageUrl = currentHarvestBack.Value;
								var backImage = configCardSet.LoadAndProcessImageUrl(currentLanguage,true, Config, configDocument, backName, backImageUrl, currentHarvest.Backs.Dpi);
								if (backName.Contains('-'))
								{
									backName = backName.Substring(backName.LastIndexOf('-'));
								}

								backImages[backName] = backImage;
							}
						}


						for (int i = 0; i < configCardSet.NbCopies; i++)
						{
							CardImages currentCard = null;
							foreach (var currentHarvestFace in currentHarvest.Faces.Images)
							{

								var faceName = $"{currentHarvestFace.Key.ToLowerInvariant()}";
								if (!configDocument.NoBack)
								{
									faceName = $"face_{faceName}";
								}
								var faceImageUrl = currentHarvestFace.Value;
								var faceImage = configCardSet.LoadAndProcessImageUrl(currentLanguage,false, Config, configDocument, faceName, faceImageUrl, currentHarvest.Faces.Dpi);
								if (currentCard == null)
								{
									currentCard = new CardImages();
									targetList.Add(currentCard);
									//currentCard.Front = new MagickImage(faceImage);
									currentCard.Front = faceImage;
									if (configDocument.NoBack)
									{
										currentCard = null;
									}
								}
								else
								{
									//currentCard.Back = new MagickImage(faceImage);
									currentCard.Back = faceImage;
									currentCard = null;
								}

								if (backImages.Count > 0)
								{
									if (backImages.Count == 1)
									{
										//currentCard.Back = new MagickImage(backImages.Values.First());
										currentCard.Back = backImages.Values.First();
									}
									else
									{

										var targetBackName = backImages.Keys.First(bn => faceName.Contains(bn));
										//currentCard.Back = new MagickImage(backImages[targetBackName]);
										currentCard.Back = backImages[targetBackName];
									}
									currentCard = null;
								}
							}
						}
					}



				}



			}



			return toReturn;
		}


		private void GenerateCardSetDocuments(Dictionary<(CardSetGenerationDocument document, string language), List<CardImages>> docImages)
		{
			Console.WriteLine($"Generation pdf documents: {sw.Elapsed}");
			

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
				switch (docImageList.Key.document.DocumentFormat)
				{
					case CardDocumentFormat.AlternateFaceAndBack:
						collec = new MagickImageCollection(docImageList.Value.SelectMany(s =>
						{
							return new[] { s.Front, s.Back };
						}));

						targetFiles.Add((baseName, collec));
						GeneratePdfsFromImages(targetFiles);
						break;
					case CardDocumentFormat.BackFirstOneDocPerBack:

						var indexInsert = baseName.LastIndexOf('.');
						var cardsPerBack = docImageList.Value.GroupBy(card => card.Back).ToArray();
						for (int backIndex = 0; backIndex < cardsPerBack.Count(); backIndex++)
						{
							var frontsAndBack = cardsPerBack[backIndex];
							var backThenFronts = new[] { frontsAndBack.Key }.Concat(frontsAndBack.Select(card => card.Front));
							collec = new MagickImageCollection(backThenFronts);
							var newName = $"{baseName.Substring(0, indexInsert)}-{backIndex + 1}{baseName.Substring(indexInsert)}";
							targetFiles.Add((newName, collec));
						}
						GeneratePdfsFromImages(targetFiles);
						break;
					case CardDocumentFormat.PrintAndPlay:
						GeneratePrintAndPlay(baseName, docImageList.Key.document, docImageList.Value);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

			}
		}

		private const float InchToCentimetre = 2.54f;
		private const float InchToPoints = 72;
		private float MmToPointsFactor = 0.1f / InchToCentimetre * InchToPoints;

		/// <summary>
		/// Generates a pdf file from config arguments and a list of generated images to compose the set.
		/// </summary>
		/// <param name="fileName">The result pdf file name</param>
		/// <param name="docConfig">The configuration to process</param>
		/// <param name="images">The images requested by the configuration to build the pdf document</param>
		private void GeneratePrintAndPlay(string fileName, CardSetGenerationDocument docConfig, List<CardImages> images)
		{




			var pageSizeType = typeof(PageSizes);
			var dynProp = pageSizeType.GetProperty(docConfig.PageSize, BindingFlags.Static | BindingFlags.Public);

			var pageSize = (PageSize)dynProp.GetValue(null);
			//var pageMarginMm = 7f;
			var pageMarginMm = 0f;
			//var imagePaddingMm = 2f;


			var cardWidthPoints = ((float)docConfig.CardSets[0].FrontCards.WidthMM) * MmToPointsFactor;
			var cardHeightPoints = ((float)docConfig.CardSets[0].FrontCards.HeigthMM) * MmToPointsFactor;


			var totalMarginPoints = 2 * pageMarginMm * MmToPointsFactor;
			var contentWidthPoints = pageSize.Width - totalMarginPoints;
			var contentHeightPoints = pageSize.Height - totalMarginPoints;

			var nbColumns = (int)(contentWidthPoints / cardWidthPoints);
			var nbRows = (int)(contentHeightPoints / cardHeightPoints);

			var nbCardsPerPage = nbRows * nbColumns;
			var nbPages = (int)Math.Ceiling((decimal)images.Count / (decimal)nbCardsPerPage);

			var docMetadata = new DocumentMetadata()
			{
				ApplyCaching = true,
				Author = "Argumentum",
				Creator = "Argumentum",
				Producer = "Argumentum",
				Subject = "Jeu de carte sur l'argumentation",
				Keywords = "Argumentation, rhétorique, arguments fallacieux, sophismes, éloquence",
				Title = "Argumentum Print & Play"
			};

			Document.Create(container =>
				{
					for (int pageIndex = 0; pageIndex < nbPages; pageIndex++)
					{
						var pageCards = images.Skip(pageIndex * nbCardsPerPage);
						if (pageIndex < nbPages - 1)
						{
							pageCards = pageCards.Take(nbCardsPerPage);
						}
						//else
						//{
						//    Debugger.Break();
						//}

						var pageCardsArray = pageCards.ToArray();

						//var cardArray = pageCards.ToList().ToJaggedArray(nbColumns);
						if (!docConfig.NoBack)
						{
							GenerateCardsPage(container, docConfig, pageSize, pageMarginMm, nbColumns, pageCardsArray, cardWidthPoints, cardImages => cardImages.Back);
							pageCardsArray = pageCardsArray.ToJaggedArray(nbColumns).Select(row => row.Reverse().ToArray())
								.ToArray().Flatten();
						}

						GenerateCardsPage(container, docConfig, pageSize, pageMarginMm, nbColumns, pageCardsArray, cardWidthPoints, cardImages => cardImages.Front);
					}


				})
				.WithMetadata(docMetadata)
				.GeneratePdf(fileName);
			Console.WriteLine($"Generated pdf document {fileName}: {sw.Elapsed}");

		}

		private static void GenerateCardsPage(IDocumentContainer container, CardSetGenerationDocument docConfig, PageSize pageSize, float pageMarginMm,
			int nbColumns, CardImages[] pageCardsArray, float cardWidthPoints, Func<CardImages, MagickImage> frontOrBack)
		{

			container.Page(page =>
			{
				page.Size(pageSize);
				page.Margin(pageMarginMm, Unit.Millimetre);
				page.PageColor(Colors.White);
				page.DefaultTextStyle(x => x.FontSize(20));

				if (!string.IsNullOrEmpty(docConfig.Header))
				{
					var imagePath = Path.Combine(Environment.CurrentDirectory, docConfig.Header);
					var currentContainer = page.Header();
					currentContainer = currentContainer.AlignCenter();
					currentContainer = currentContainer.Height(pageSize.Height / 40);
					currentContainer = currentContainer.Padding(pageSize.Width / 150);
					currentContainer.Image(imagePath, ImageScaling.FitHeight);

				}


				page.Content()
					.Padding(0)
					.AlignCenter()
					.AlignTop()
					.Column(c =>
					{

						c.Item()
							.AlignCenter()
							.AlignTop()
							.Grid(g =>
							{
								g.AlignCenter();
								g.Spacing(0, Unit.Millimetre);
								g.Columns(nbColumns);
								for (int cardIndex = 0; cardIndex < pageCardsArray.Length; cardIndex++)
								{
									var gridCell = g.Item()
										//.Border(0.2f, Unit.Millimetre)
										.AlignCenter()
										.AlignMiddle()
										.Width(cardWidthPoints);
									var pageCard = pageCardsArray[cardIndex];
									if (pageCard != null)
									{
										MagickImage toPrint = frontOrBack(pageCard);

										PrintMagickImageIntoGridCell(toPrint, gridCell);

									}
								}
							});
					});


				//page.Footer()
				//    .AlignCenter()
				//    .Text(x =>
				//    {
				//        x.Span("Page ");
				//        x.CurrentPageNumber();
				//    });
			});
		}

		/// <summary>
		/// uses one of the IContainer.Image extension method overloads to add MagickImage object to the corresponding grid cell
		/// </summary>
		/// <param name="toPrint">the MagickImage object to render</param>
		/// <param name="gridCell">the IContainer grid cell in which to add the target image</param>
		private static void PrintMagickImageIntoGridCell(MagickImage toPrint, IContainer gridCell)
		{
			if (!string.IsNullOrEmpty(toPrint.FileName))
			{
				gridCell.Image(toPrint.FileName);
			}
			else
			{
				using (var memStream = new MemoryStream())
				{
					toPrint.Write(memStream);
					gridCell.Image(memStream.ToArray());
				}
			}
		}





		private void GeneratePdfsFromImages(List<(string fileName, MagickImageCollection documentImages)> targetFiles)
		{
			foreach (var targetFile in targetFiles)
			{
				targetFile.documentImages.Write(targetFile.fileName);
				Console.WriteLine($"Generated pdf document {targetFile.fileName}: {sw.Elapsed}");
			}
		}


		//private List<ImageMagick.MagickImage> GenerateImages(string exampleName)
		private async Task<CardPenHarvest> GenerateImages(IPage driver, CardSetPayload cardSetDocument, bool pauseForEdits)
		{
			var toReturn = new CardPenHarvest();

			//driver.FindElement(By.Id("load")).Click();
			//var importInput= driver.FindElement(By.Id("import"));
			var importInput = driver.Locator("#import");
			if (!await importInput.IsVisibleAsync())
			{
				var exampleButton = driver.Locator("#load");
				await exampleButton.ClickAsync();

				Thread.Sleep(TimeSpan.FromSeconds(2));
			}

			
			Console.WriteLine($"Generating CardSet {cardSetDocument.FileName}: {sw.Elapsed}");
	
			//{
			//	if (!Path.IsPathFullyQualified(cardSet.JsonFilePath))
			//	{
			//		customFilePath = Path.Combine(Environment.CurrentDirectory, customFilePath);
			//	}

			//	await driver.SetInputFilesAsync("#import", customFilePath);
			//}

			var filePayLoad = new FilePayload()
			{
				Name = cardSetDocument.FileName,
				MimeType = cardSetDocument.GetMimeType(),
				Buffer = JsonSerializer.Serialize(cardSetDocument.CardSetDocument)
			};
			await driver.SetInputFilesAsync("#import", filePayLoad);



			Thread.Sleep(TimeSpan.FromSeconds(5));


			if (pauseForEdits)
			{
				Console.WriteLine($"Le navigateur est en pause le temps de faire vos éditions.\n Appuyez sur une touche pour démarrer la génération");
				Console.Read();
			}
			//var objIFrame = driver.FindElement(By.Id("cpOutput"));
			var objIFrame = driver.FrameLocator("#cpOutput");


			//var objSession = ((ChromiumDriver) driver).CreateDevToolsSession();


			//driver.SwitchTo().Frame(objIFrame);
			//Console.WriteLine($"Waiting for html display {sw.Elapsed}");

			//new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(drv => drv.FindElement(By.TagName("card")));
			var objCardTag = objIFrame.Locator("card");
			//await objCardTag.WaitForAsync(new LocatorWaitForOptions(){State = WaitForSelectorState.Attached});
			while (await objCardTag.CountAsync() == 0)
			{
				Thread.Sleep(100);
			}


			//driver.SwitchTo().ParentFrame();


			//driver.FindElement(By.CssSelector(".image")).Click();
			await driver.Locator(".image").ClickAsync();

			Thread.Sleep(TimeSpan.FromSeconds(5));

			//objIFrame = new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(drv => drv.FindElement(By.Id("cpOutput")));
			//driver.SwitchTo().Frame(objIFrame);
			Console.WriteLine($"Waiting for image display {sw.Elapsed}");
			//new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(drv => drv.FindElement(By.TagName("card")));
			//await objCardTag.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Attached });
			while (await objCardTag.CountAsync() == 0)
			{
				Thread.Sleep(100);
			}

			//var dpi = (long)driver.ExecuteScript("return dpi;");
			//var dpi = await driver.EvaluateAsync("return dpi;");
			//var dpi = await driver.EvaluateAsync("dpi");

			var dpi = await driver.Locator("#dpi").InputValueAsync();


			toReturn.Dpi = Convert.ToInt32(dpi);

			//Func<IWebDriver, IWebElement> generateButtonLambda = (IWebDriver drv) => drv.FindElement(By.Id("generateButton"));
			//var generateButton = new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(drv => generateButtonLambda(drv));
			var objGenerateButton = objIFrame.Locator("#generateButton");
			await objGenerateButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Attached });



			//var zipButtoLambda = new Func<IWebDriver, IWebElement>((IWebDriver drv) => drv.FindElement(By.Id("zipButton")));

			Thread.Sleep(TimeSpan.FromSeconds(1));

			//var zipButton = zipButtoLambda(driver);
			var zipButton = objIFrame.Locator("#zipButton"); ;

			Thread.Sleep(TimeSpan.FromSeconds(5));
			//generateButton.Click();
			await objGenerateButton.ClickAsync();
			//Console.WriteLine($"Waiting for Generated images  {sw.Elapsed}");
			//new WebDriverWait(driver, TimeSpan.FromSeconds(600)).Until(drv => zipButtoLambda(drv).Displayed && zipButtoLambda(drv).Enabled);

			await zipButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Attached });
			await zipButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 0 });
			var zipHandler = await zipButton.ElementHandleAsync();
			await zipHandler.WaitForElementStateAsync(ElementState.Enabled);

			//Console.WriteLine($"images generated {sw.Elapsed}");
			//var generatedImagesDiv = driver.FindElement(By.Id("cpImages"));
			var generatedImagesDiv = objIFrame.Locator("#cpImages");

			//var generatedImages = generatedImagesDiv.FindElements(By.TagName("img"));
			var generatedImages = generatedImagesDiv.Locator("img");

			var cardNames = new List<string>();

			//var cardsHtml = driver.FindElements(By.TagName("card"));
			var cardsHtml = objIFrame.Locator("card");
			for (var idxCard = 0; idxCard < await cardsHtml.CountAsync(); idxCard++)
			{
				var strCardName = idxCard.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0');
				//var cardElement = cardsHtml[idxCard];
				var cardElement = cardsHtml.Nth(idxCard);
				//var cardCssName = cardElement.FindElements(By.ClassName("cardName"));
				var cardCssName = cardElement.Locator(".cardName");
				//if (cardCssName.Count > 0)
				if (await cardCssName.CountAsync() > 0)
				{
					//strCardName = cardCssName[0].GetAttribute("textContent").Trim('-');
					var currentCard = cardCssName.Nth(0);
					//strCardName = (await currentCard.GetAttributeAsync("textContent"))?.Trim('-');
					strCardName = (await currentCard.TextContentAsync())?.Trim('-').Replace(" ", "_");
				}
				cardNames.Add(strCardName);
			}

			if (await generatedImages.CountAsync() != cardNames.Count)
			{
				var message = $"not same number of generated cards ({await generatedImages.CountAsync()}) and card names ({cardNames.Count})";
				throw new ApplicationException(message);
			}

			for (int i = 0; i < await generatedImages.CountAsync(); i++)
			{
				//toReturn.Images[cardNames[i]] = generatedImages[i].GetAttribute("src");
				var currentGeneratedImage = generatedImages.Nth(i);
				var currentCardName = cardNames[i];
				toReturn.Images[currentCardName] = await currentGeneratedImage.GetAttributeAsync("src");
			}

			//driver.SwitchTo().ParentFrame();

			return toReturn;
		}



	}
}


