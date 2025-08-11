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
            var webGenerator = new WebBasedGenerator { AssetConverterConfig = config, Config = config.WebBasedGeneratorConfig, Output = _output };
            
            _output.WriteLine("--- DUMPING CONFIGURATION BEFORE RUN ---");
            _output.WriteLine(JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
            _output.WriteLine("--------------------------------------");

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
            
            var imageFile = Directory.EnumerateFiles(config.GetImagesDirectory("fr"), "fr-fallacytestset-chewbacca-defense_face.png", SearchOption.AllDirectories).FirstOrDefault();
            
            // 3. Assertion
            Assert.NotNull(imageFile);
            
            var imageBytes = await File.ReadAllBytesAsync(imageFile);

            // 4. Vérification du Snapshot
            await Verifier.Verify(imageBytes, "png");
        }
    }
}