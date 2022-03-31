using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.Playwright;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Svg.Skia;
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
            
            GenerateDocuments(docImages);
            //Console.WriteLine($"Generation finished. Total duration: {sw.Elapsed}");

        }


        async Task<Dictionary<string, CardSetHarvest>>  HarvestImages()
        {

            Dictionary<string, CardSetHarvest> harvestDictionary;
            //var jsonHarvestName = Path.Combine(Environment.CurrentDirectory, "harvest.json");
            //if (File.Exists(jsonHarvestName))
            //{
            //    using var configStream = File.OpenRead(jsonHarvestName);
            //    harvestDictionary = JsonSerializer.Deserialize<Dictionary<string, CardSetHarvest>>(configStream);
            //    Console.WriteLine($"Loaded Harvest {jsonHarvestName}");
            //}
            //else
            //{

            harvestDictionary = new Dictionary<string, CardSetHarvest>();
            var usedCardSets = Config.Documents.Where(d=>d.Enabled).SelectMany(d => d.CardSets.Select(dc => dc.CardSetName)).Distinct()
                .ToArray();
            var targetCardSets = Config.CardSets.Where(c => usedCardSets.Contains(c.Name)).ToArray();
            foreach (var configCardSet in targetCardSets)
            {

                var jsonHarvestName = configCardSet.GetHarvestSerializationName(Config);
                if (File.Exists(jsonHarvestName))
                {
                    using var configStream = File.OpenRead(jsonHarvestName);
                    var currentHarvest = JsonSerializer.Deserialize<CardSetHarvest>(configStream);
                    Console.WriteLine($"Loaded Harvest {jsonHarvestName}: {sw.Elapsed}");
                    harvestDictionary[configCardSet.Name] = currentHarvest;
                }

            }

            if (harvestDictionary.Count < targetCardSets.Count())
            {
                //var options = new ChromeOptions();
                //options.BinaryLocation = Config.ChromeBinaryPath;

                var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
                if (exitCode != 0)
                {
                    throw new Exception($"Playwright exited with code {exitCode}");
                }

                using var playwright = await Playwright.CreateAsync();
                await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false,
                    //SlowMo = 50,
                });

                //using (var driver = new ChromeDriver(options))
                //{
                    try
                    {
                        //driver.Manage().Window.Minimize();
                        ////js = (IJavaScriptExecutor)driver;
                        ////vars = new Dictionary<String, Object>();
                        //driver.Navigate().GoToUrl(Config.CardpenUrl);
                        //driver.FindElement(By.Id("eg")).Click();


                        var page = await browser.NewPageAsync();
                        await page.GotoAsync(Config.CardpenUrl);




                    Thread.Sleep(TimeSpan.FromSeconds(5));

                        foreach (var configCardSet in targetCardSets)
                        {
                            if (!harvestDictionary.ContainsKey(configCardSet.Name))
                            {
                                var currentHarvest = new CardSetHarvest();
                                var faces = await GenerateImages(page, configCardSet.FaceCardSetInfo);
                                currentHarvest.Faces = faces;
                                if (configCardSet.BackCardSetInfo.IsSet)
                                {
                                    var backs = await GenerateImages(page, configCardSet.BackCardSetInfo);
                                    currentHarvest.Backs = backs;
                                }

                                var jsonHarvestName = configCardSet.GetHarvestSerializationName(Config);
                                var strNewConfig = JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(currentHarvest));
                                File.WriteAllText(jsonHarvestName, strNewConfig);
                                Console.WriteLine($"Serialized Harvest {jsonHarvestName}: {sw.Elapsed}");


                                harvestDictionary[configCardSet.Name] = currentHarvest;
                            }


                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    //finally
                    //{
                    //    driver.Quit();
                    //}
                //}
            }




            //var strNewConfig = JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(harvestDictionary));
            //File.WriteAllText(jsonHarvestName, strNewConfig);
            //Console.WriteLine($"Serialized Harvest {jsonHarvestName}");
            //}

            return harvestDictionary;
        }



        Dictionary<DocumentConfig, List<CardImages>> GenerateDocumentImages(Dictionary<string, CardSetHarvest> harvestDictionary)
        {
            Dictionary<DocumentConfig, List<CardImages>> toReturn = new Dictionary<DocumentConfig, List<CardImages>>();

            foreach (var configDocument in Config.Documents.Where(d=>d.Enabled))
            {
                List<CardImages> targetList;

                if (!toReturn.TryGetValue(configDocument, out targetList))
                {
                    targetList = new List<CardImages>();
                    toReturn[configDocument] = targetList;
                }

                foreach (var configCardSet in configDocument.CardSets)
                {
                    Console.WriteLine($"Generating card set images for {configDocument.DocumentName} - {configCardSet.CardSetName}: {sw.Elapsed}");



                    var currentHarvest = harvestDictionary[configCardSet.CardSetName];
                    var backImages = new Dictionary<string, MagickImage>();
                    if (currentHarvest.Backs != null)
                    {
                        foreach (var currentHarvestBack in currentHarvest.Backs.Images)
                        {
                            var backName = $"{currentHarvestBack.Key.ToLowerInvariant()}" ;
                            var backImageUrl = currentHarvestBack.Value;
                            var backImage = configCardSet.LoadAndProcessImageUrl(true, Config, configDocument,  backName, backImageUrl, currentHarvest.Backs.Dpi);
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
                            var faceImage = configCardSet.LoadAndProcessImageUrl(false, Config, configDocument, faceName, faceImageUrl, currentHarvest.Faces.Dpi);
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



            return toReturn;
        }


        private void GenerateDocuments(Dictionary<DocumentConfig, List<CardImages>> docImages)
        {
            Console.WriteLine($"Generation pdf documents: {sw.Elapsed}");
            var pdfDirectory = Config.GetPdfsDirectory();

            foreach (var docImageList in docImages)
            {
                var densityDirectory = Path.Combine(pdfDirectory, $@".\density-{docImageList.Key.TargetDensity}\");
                if (!Directory.Exists(densityDirectory))
                {
                    Directory.CreateDirectory(densityDirectory);
                }

                var targetFiles = new List<(string fileName, MagickImageCollection documentImages)>();
                MagickImageCollection collec;
                var baseName = Path.Combine(densityDirectory, docImageList.Key.DocumentName);
                switch (docImageList.Key.DocumentFormat)
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
                            var newName = $"{baseName.Substring(0, indexInsert)}-{backIndex+1}{baseName.Substring(indexInsert)}";
                            targetFiles.Add((newName,collec));
                        }
                        GeneratePdfsFromImages(targetFiles);
                        break;
                    case CardDocumentFormat.PrintAndPlay:
                        GeneratePrintAndPlay(baseName, docImageList.Key, docImageList.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }
        }

        private const float InchToCentimetre = 2.54f;
        private const float InchToPoints = 72;
        private float MmToPointsFactor = 0.1f / InchToCentimetre * InchToPoints;

        private void GeneratePrintAndPlay(string fileName, DocumentConfig docConfig, List<CardImages> images)
        {

            var pageSizeType = typeof(PageSizes);
            var dynProp = pageSizeType.GetProperty(docConfig.PageSize, BindingFlags.Static | BindingFlags.Public);

            var pageSize = (PageSize)  dynProp.GetValue(null);
            var pageMarginMm = 7f;
            //var imagePaddingMm = 2f;


            var cardWidthPoints = ((float)docConfig.CardSets[0].FrontCards.WidthMM) * MmToPointsFactor;
            var cardHeightPoints = ((float)docConfig.CardSets[0].FrontCards.HeigthMM) * MmToPointsFactor;

            
            var totalMarginPoints = 2 * pageMarginMm * MmToPointsFactor;
            var contentWidthPoints = pageSize.Width - totalMarginPoints;
            var contentHeightPoints = pageSize.Height - totalMarginPoints;

            var nbColumns = (int) (contentWidthPoints / cardWidthPoints);
            var nbRows = (int) (contentHeightPoints / cardHeightPoints);

            var nbCardsPerPage = nbRows * nbColumns;
            var nbPages = (int) Math.Ceiling((decimal) images.Count / (decimal) nbCardsPerPage);

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
                        if (pageIndex<nbPages-1)
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

        private static void GenerateCardsPage(IDocumentContainer container, DocumentConfig docConfig, PageSize pageSize, float pageMarginMm,
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
                    page.Header()
                        .AlignCenter()
                        .Height(pageSize.Height / 20)
                        .Padding(pageSize.Width / 70)
                        .Image(docConfig.Header, ImageScaling.FitHeight);
                       
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
                                g.Spacing(2, Unit.Millimetre);
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


        private void GeneratePdfsFromImages(List<(string fileName, MagickImageCollection documentImages)> targetFiles)
        {
            foreach (var targetFile in targetFiles)
            {
                targetFile.documentImages.Write(targetFile.fileName);
                Console.WriteLine($"Generated pdf document {targetFile.fileName}: {sw.Elapsed}");
            }
        }


        //private List<ImageMagick.MagickImage> GenerateImages(string exampleName)
        private async Task<CardPenHarvest> GenerateImages(IPage driver, CardSetInfo cardSet)
        {
            var toReturn = new CardPenHarvest();

            

            switch (cardSet.CardSetType)
            {
                case CardSetType.ExampleByName:
                    //driver.FindElement(By.Id("exampleList")).Click();
                    //var playwright = Task.Run(async () => await Playwright.CreateAsync()).Result ;
                    
                    //var dropdown = driver.FindElement(By.Id("exampleList"));
                    var dropdown = driver.Locator("#exampleList");
                    if (!await dropdown.IsVisibleAsync())
                    {
                        var exampleButton = driver.Locator("#eg");
                        await exampleButton.ClickAsync();
                        //Thread.Sleep(TimeSpan.FromSeconds(5));
                    }

                    await dropdown.ClickAsync();
                    Console.WriteLine($"Generating example {cardSet.ExampleName}: {sw.Elapsed}");
                    //dropdown.FindElement(By.XPath($"//option[. = '{cardSet.ExampleName}']")).Click();
                    await dropdown.SelectOptionAsync(cardSet.ExampleName);
                    //Thread.Sleep(TimeSpan.FromSeconds(5));
                    break;
                case CardSetType.CustomJson:
                    //driver.FindElement(By.Id("load")).Click();
                    //var importInput= driver.FindElement(By.Id("import"));
                    var importInput = driver.Locator("#import");
                    if (!await importInput.IsVisibleAsync())
                    {
                        var exampleButton = driver.Locator("#load");
                        await exampleButton.ClickAsync();
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                    }
                    
                    var customFilePath = cardSet.CustomJsonFileName;
                    if (!Path.IsPathFullyQualified(cardSet.CustomJsonFileName))
                    {
                        customFilePath = Path.Combine(Environment.CurrentDirectory, customFilePath);
                    }
                    Console.WriteLine($"Generating CardSet {customFilePath}: {sw.Elapsed}");
                    await driver.SetInputFilesAsync("#import", customFilePath);
                    Thread.Sleep(TimeSpan.FromSeconds(5));



                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

           
            if (cardSet.PauseForEdits)
            {
                Console.WriteLine($"Chrome est en pause le temps de faire vos éditions.\n Appuyez sur une touche pour démarrer la génération");
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
            while (await objCardTag.CountAsync()==0)
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

            await zipButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Attached  });
            await zipButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible,Timeout = 0 });
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
                    strCardName = (await currentCard.TextContentAsync())?.Trim('-').Replace(" ","_");
                }
                cardNames.Add(strCardName);
            }

            if (await generatedImages.CountAsync() != cardNames.Count)
            {
                throw new ApplicationException("not same number of generated cards and card names");
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


