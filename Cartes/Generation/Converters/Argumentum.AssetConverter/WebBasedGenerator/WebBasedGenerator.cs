// Generated by Selenium IDE
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using ImageMagick;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.Page;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
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
            var harvestDictionary = HarvestImages();
            var docImages = GenerateDocumentImages(harvestDictionary);
            GenerateDocuments(docImages);
            //Console.WriteLine($"Generation finished. Total duration: {sw.Elapsed}");

        }

        private void GenerateDocuments(Dictionary<string, List<MagickImage>> docImages)
        {
            Console.WriteLine($"Generation pdf documents: {sw.Elapsed}");
            foreach (var docImageList in docImages)
            {
                var collec = new MagickImageCollection(docImageList.Value);
                var targetFile = Path.Combine(Environment.CurrentDirectory, docImageList.Key);
                collec.Write(targetFile);
                Console.WriteLine($"Generated pdf document {targetFile}: {sw.Elapsed}");
            }
        }


        Dictionary<string, List<MagickImage>> GenerateDocumentImages(Dictionary<string, CardSetHarvest> harvestDictionary)
        {
            Dictionary<string, List<MagickImage>> toReturn = new Dictionary<string, List<MagickImage>>();

            foreach (var configDocument in Config.Documents)
            {
                foreach (var configCardSet in configDocument.CardSets)
                {
                    Console.WriteLine($"Generating card set images for {configDocument.DocumentName} - {configCardSet.CardSetName}: {sw.Elapsed}");
                    List<MagickImage> targetList;
                    if (!toReturn.TryGetValue(configDocument.DocumentName, out targetList))
                    {
                        targetList = new List<MagickImage>();
                        toReturn[configDocument.DocumentName] = targetList;
                    }

                    var currentHarvest = harvestDictionary[configCardSet.CardSetName];
                    var backImages = new Dictionary<string, MagickImage>();
                    if (currentHarvest.Backs != null)
                    {
                        foreach (var currentHarvestBack in currentHarvest.Backs)
                        {
                            var backName = currentHarvestBack.Key.ToLowerInvariant();
                            var backImageUrl = currentHarvestBack.Value;
                            var backImage = configCardSet.LoadAndProcessImageUrl(backImageUrl);
                            if (backName.Contains('-'))
                            {
                                backName = backName.Substring(backName.LastIndexOf('-'));
                            }

                            backImages[backName] = backImage;
                        }
                    }

                    foreach (var currentHarvestFace in currentHarvest.Faces)
                    {
                        var faceName = currentHarvestFace.Key.ToLowerInvariant();
                        var faceImageUrl = currentHarvestFace.Value;
                        var faceImage = configCardSet.LoadAndProcessImageUrl(faceImageUrl);
                        for (int i = 0; i < configCardSet.NbCopies; i++)
                        {
                            targetList.Add(new MagickImage(faceImage));
                            if (backImages.Count > 0)
                            {
                                if (backImages.Count == 1)
                                {
                                    targetList.Add(new MagickImage(backImages.Values.First()));
                                }
                                else
                                {

                                    var targetBackName = backImages.Keys.First(bn => faceName.Contains(bn));
                                    targetList.Add(new MagickImage(backImages[targetBackName]));
                                }
                            }

                        }

                    }
                }
            }

            

            return toReturn;
        }


        Dictionary<string, CardSetHarvest> HarvestImages()
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
            foreach (var configCardSet in Config.CardSets)
            {

                var jsonHarvestName = configCardSet.GetSerializationName();
                if (File.Exists(jsonHarvestName))
                {
                    using var configStream = File.OpenRead(jsonHarvestName);
                    var currentHarvest = JsonSerializer.Deserialize<CardSetHarvest>(configStream);
                    Console.WriteLine($"Loaded Harvest {jsonHarvestName}: {sw.Elapsed}");
                    harvestDictionary[configCardSet.Name] = currentHarvest;
                }

            }

            if (harvestDictionary.Count < Config.CardSets.Count)
            {
                var options = new ChromeOptions();
                options.BinaryLocation = Config.ChromeBinaryPath;
                using (var driver = new ChromeDriver(options))
                {
                    try
                    {
                        driver.Manage().Window.Minimize();
                        //js = (IJavaScriptExecutor)driver;
                        //vars = new Dictionary<String, Object>();
                        driver.Navigate().GoToUrl(Config.CardpenUrl);

                        driver.FindElement(By.Id("eg")).Click();
                        Thread.Sleep(TimeSpan.FromSeconds(5));

                        foreach (var configCardSet in Config.CardSets)
                        {
                            if (! harvestDictionary.ContainsKey(configCardSet.Name))
                            {
                                var currentHarvest = new CardSetHarvest();
                                var faces = GenerateImages(driver, configCardSet.FaceExampleName);
                                currentHarvest.Faces = faces;
                                if (!string.IsNullOrEmpty(configCardSet.BackExampleName))
                                {
                                    var backs = GenerateImages(driver, configCardSet.BackExampleName);
                                    currentHarvest.Backs = backs;
                                }

                                var jsonHarvestName = configCardSet.GetSerializationName();
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
                    finally
                    {
                        driver.Quit();
                    }
                }
            }




            //var strNewConfig = JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(harvestDictionary));
            //File.WriteAllText(jsonHarvestName, strNewConfig);
            //Console.WriteLine($"Serialized Harvest {jsonHarvestName}");
            //}

            return harvestDictionary;
        }


        //private List<ImageMagick.MagickImage> GenerateImages(string exampleName)
        private Dictionary<string, string> GenerateImages(RemoteWebDriver driver, string exampleName)
        {
            var toReturn = new Dictionary<string, string>();

            driver.FindElement(By.Id("exampleList")).Click();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var dropdown = driver.FindElement(By.Id("exampleList"));
            Console.WriteLine($"Generating example {exampleName}: {sw.Elapsed}");
            dropdown.FindElement(By.XPath($"//option[. = '{exampleName}']")).Click();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var objIFrame = driver.FindElement(By.Id("cpOutput"));

            //var objSession = ((ChromiumDriver) driver).CreateDevToolsSession();


            driver.SwitchTo().Frame(objIFrame);
            //Console.WriteLine($"Waiting for html display {sw.Elapsed}");

            new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(drv => drv.FindElement(By.TagName("card")));
            driver.SwitchTo().ParentFrame();

            
            driver.FindElement(By.CssSelector(".image")).Click();

            Thread.Sleep(TimeSpan.FromSeconds(5));

            //objIFrame = new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(drv => drv.FindElement(By.Id("cpOutput")));
            driver.SwitchTo().Frame(objIFrame);
            //Console.WriteLine($"Waiting for image display {sw.Elapsed}");
            new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(drv => drv.FindElement(By.TagName("card")));


            Func<IWebDriver, IWebElement> generateButtonLambda = (IWebDriver drv) => drv.FindElement(By.Id("generateButton"));
            var generateButton = new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(drv => generateButtonLambda(drv));
            //new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(drv => generateButtonLambda(drv).Displayed && generateButtonLambda(drv).Enabled);
            //Thread.Sleep(TimeSpan.FromSeconds(2));

            var zipButtoLambda = new Func<IWebDriver, IWebElement>((IWebDriver drv) => drv.FindElement(By.Id("zipButton")));

            //Thread.Sleep(TimeSpan.FromSeconds(1));

            var zipButton = zipButtoLambda(driver);
            Thread.Sleep(TimeSpan.FromSeconds(5));
            generateButton.Click();
            //Console.WriteLine($"Waiting for Generated images  {sw.Elapsed}");
            new WebDriverWait(driver, TimeSpan.FromSeconds(600)).Until(drv => zipButtoLambda(drv).Displayed && zipButtoLambda(drv).Enabled);
            //Console.WriteLine($"images generated {sw.Elapsed}");
            var generatedImagesDiv = driver.FindElement(By.Id("cpImages"));
            var generatedImages = generatedImagesDiv.FindElements(By.TagName("img"));

            var cardNames = new List<string>();

            var cardsHtml = driver.FindElements(By.TagName("card"));
            for (var idxCard = 0; idxCard < cardsHtml.Count; idxCard++)
            {
                var strCardName = idxCard.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0');
                var cardElement = cardsHtml[idxCard];
                var cardCssName = cardElement.FindElements(By.ClassName("cardName"));
                if (cardCssName.Count > 0)
                {
                    strCardName = cardCssName[0].GetAttribute("textContent").Trim('-');
                }
                cardNames.Add(strCardName);
            }

            if (generatedImages.Count != cardNames.Count)
            {
                throw new ApplicationException("not same number of generated cards and card names");
            }

            for (int i = 0; i < generatedImages.Count; i++)
            {
                toReturn[cardNames[i]] = generatedImages[i].GetAttribute("src");
            }


            driver.SwitchTo().ParentFrame();

            return toReturn;
        }



    }


}


