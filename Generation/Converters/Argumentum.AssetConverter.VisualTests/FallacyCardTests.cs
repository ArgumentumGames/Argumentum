using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Playwright;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests
{
    public class FallacyCardTests
    {
        private readonly ITestOutputHelper _output;

        public FallacyCardTests(ITestOutputHelper output)
        {
            _output = output;

            // --- Test Cleanup ---
            // Supprime les anciens fichiers de "harvest" pour garantir un état propre
            // et éviter la pollution entre les exécutions de test.
            var harvestPath = "TestData/FallacyCard/Render_Nominal/Output/fr/Harvest/FallacyTestSet_harvest_fr.json";
            if (File.Exists(harvestPath))
            {
                File.Delete(harvestPath);
                _output.WriteLine($"CLEANUP: Deleted stale harvest file at {harvestPath}");
            }
        }

        [Fact]
        public async Task Render_NominalCard()
        {
            // 1. Préparation
            var testDirectory = "TestData/FallacyCard/Render_Nominal";
            var configPath = Path.Combine(testDirectory, "AssetConverterConfig.test.json");
            // Charger manuellement les datasets de la config de test pour les injecter
            var jsonString = File.ReadAllText(configPath);
            using var doc = JsonDocument.Parse(jsonString);
            var webConfigNode = doc.RootElement.GetProperty("WebBasedGeneratorConfig");
            var datasetsNode = webConfigNode.GetProperty("DataSets");
            var testDataSets = JsonSerializer.Deserialize<System.Collections.Generic.List<DataSetInfo>>(datasetsNode.GetRawText());

            var config = AssetConverterConfig.GetConfig(configPath, out _);
            if (testDataSets != null)
            {
                // Corrige le chemin du fichier de données de test et l'ajoute à la config
                var testDataSet = testDataSets.First();
                var csvPath = Path.Combine(testDirectory, "test-data.csv");
                testDataSet.DebugFilePath = csvPath;
                testDataSet.ReleaseFilePath = csvPath;
                config.DataSets.AddRange(testDataSets);
            }
            var webGenerator = new WebBasedGenerator { AssetConverterConfig = config, Config = config.WebBasedGeneratorConfig, Output = _output, KeepBrowserOpen = true };
            
            _output.WriteLine("--- DUMPING CONFIGURATION BEFORE RUN ---");
            _output.WriteLine(JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
            _output.WriteLine("--------------------------------------");

            string? imageFile = null;
            try
            {
                // 2. Exécution "in-process"
                await webGenerator.Run();

                var postProcessor = new ImageFileGenerator() { AssetConverterConfig = config, Config = config.WebBasedGeneratorConfig };
                var harvests = new System.Collections.Concurrent.ConcurrentDictionary<(string, string), System.Func<CardSetHarvest>>();
                await webGenerator.HarvestManager.LoadHarvestsAsync(
                    webGenerator.HarvestManager.GetTargetCardSets(),
                    new System.Threading.Tasks.ParallelOptions { MaxDegreeOfParallelism = 1 },
                    harvests);
                var docImages = postProcessor.GenerateDocumentImages(harvests);
                _output.WriteLine($"docImages collection contains {docImages.Count} items.");
                foreach (var item in docImages)
                {
                    _output.WriteLine($"  - Document: {item.Key.document.DocumentName}, Lang: {item.Key.language}, Images: {item.Value.Count}");
                }
                webGenerator.GenerateCardSetDocuments(docImages);

                imageFile = Directory.EnumerateFiles(config.GetImagesDirectory("fr"), "chewbacca-defense_face.png", SearchOption.AllDirectories).FirstOrDefault();

                // 3. Assertion
                if (imageFile == null)
                {
                    var errorDiv = webGenerator.LastPageUsed.FrameLocator("#cpOutput").Locator("div.cp-js-error");
                    var errorMessage = "Image file was not generated, but no JavaScript error was detected in the DOM.";
                    if (await errorDiv.IsVisibleAsync())
                    {
                        var jsError = await errorDiv.InnerTextAsync();
                        errorMessage = $"Image file not generated. A JavaScript error was captured in the DOM: {jsError}";
                    }
                    Assert.Fail(errorMessage);
                }
            }
            finally
            {
                // Ensure the browser is closed even if assertions fail
                if (webGenerator.HarvestManager?.LastPageUsed?.Context.Browser != null)
                {
                    await webGenerator.HarvestManager.LastPageUsed.Context.Browser.CloseAsync();
                }
            }
            
            var imageBytes = await File.ReadAllBytesAsync(imageFile);
 
            // 4. Vérification du Snapshot
            await Verifier.Verify(imageBytes, "png");
        }
    }
}