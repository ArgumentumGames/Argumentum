using Xunit;
using FluentAssertions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Argumentum.AssetConverter.Entities;
using ImageMagick;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.ImageGeneration
{
    public class ImageFileGeneratorTests : IDisposable
    {
        private readonly string _testOutputDir;
        private readonly ITestOutputHelper _output;

        public ImageFileGeneratorTests(ITestOutputHelper output)
        {
            _output = output;
            // Utiliser le répertoire de base de l'application (bin/Debug/...) pour les tests
            var testRunDir = Path.Combine(AppContext.BaseDirectory, "TestRuns", Guid.NewGuid().ToString());
            _testOutputDir = testRunDir;
            Directory.CreateDirectory(_testOutputDir);
        }

        private string SetupTestConfiguration(List<CardSetDocumentConfig> docConfigs)
        {
            // Le répertoire de sortie des images doit être un sous-répertoire du répertoire de test
            var imageOutputDir = Path.Combine(_testOutputDir, "GeneratedImages");
            Directory.CreateDirectory(imageOutputDir);

            var assetConfig = new AssetConverterConfig
            {
                // Le BaseTargetDirectoryName DOIT pointer vers le sous-répertoire d'images
                BaseTargetDirectoryName = imageOutputDir,
                LocalizationConfig = new LocalizationConfig { Enabled = false, DefaultLanguage = "en" },
                WebBasedGeneratorConfig = new WebBasedGeneratorConfig
                {
                    EnableParallelism = false,
                    CardSetDocuments = docConfigs
                }
            };
            
            // Le fichier de configuration est créé à la racine du répertoire d'exécution du tes
            var configPath = Path.Combine(_testOutputDir, "AssetConverterConfig.json");
            var jsonString = JsonSerializer.Serialize(assetConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configPath, jsonString);
            
            _output.WriteLine($"Configuration file created at: {configPath}");
            _output.WriteLine($"Images will be generated in: {imageOutputDir}");
            
            // On retourne le chemin absolu du fichier de config
            return Path.GetFullPath(configPath);
        }

        private void CreateFakeImageFile(AssetConverterConfig config, string language, string cardSetName, DocumentConfig docConfig, string imageName)
        {
            var fakeImagePath = ImageHelper.GetImageFileName(config, docConfig, language, cardSetName, imageName);
            Directory.CreateDirectory(Path.GetDirectoryName(fakeImagePath));
            File.Create(fakeImagePath).Close();
        }

        [Fact]
        public void GenerateDocumentImages_WithFrontAndBack_ShouldReturnPathsToPreexistingFiles()
        {
            // Arrange
            var docConfig = new CardSetDocumentConfig { DocumentName = "TestDoc", Enabled = true, NoBack = false, ImageFormat = MagickFormat.Png, CardSets = new List<DocumentCardSet> { new DocumentCardSet { CardSetName = "TestSet" } } };
            var configPath = SetupTestConfiguration(new List<CardSetDocumentConfig> { docConfig });

            var config = AssetConverterConfig.GetConfig(configPath, out _);
            var sut = new ImageFileGenerator
            {
                AssetConverterConfig = config,
                Config = config.WebBasedGeneratorConfig
            };

            CreateFakeImageFile(config, "en", "TestSet", docConfig, "card1_face");
            CreateFakeImageFile(config, "en", "TestSet", docConfig, "default");

            var facePath = ImageHelper.GetImageFileName(config, docConfig, "en", "TestSet", "card1_face");
            var backPath = ImageHelper.GetImageFileName(config, docConfig, "en", "TestSet", "default");

            var harvest = new CardSetHarvest();
            harvest.Faces.Images.TryAdd("card1", facePath);
            harvest.Backs.Images.TryAdd("default", backPath);

            var harvestDictionary = new ConcurrentDictionary<(string, string), Func<CardSetHarvest>>();
            harvestDictionary.TryAdd(("TestSet", "en"), () => harvest);

            // Act
            var result = sut.GenerateDocumentImages(harvestDictionary);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            var imageList = result.First().Value;
            imageList.Should().HaveCount(1);
            imageList[0].Front.Should().EndWith("card1_face.png");
            imageList[0].Back.Should().EndWith("default.png");
        }

        [Fact]
        public void GenerateDocumentImages_WithFrontOnly_ShouldReturnFacePathAndNullBack()
        {
            // Arrange
            var docConfig = new CardSetDocumentConfig { DocumentName = "TestDoc", Enabled = true, NoBack = true, ImageFormat = MagickFormat.Png, CardSets = new List<DocumentCardSet> { new DocumentCardSet { CardSetName = "TestSet" } } };
            var configPath = SetupTestConfiguration(new List<CardSetDocumentConfig> { docConfig });

            var config = AssetConverterConfig.GetConfig(configPath, out _);
            var sut = new ImageFileGenerator
            {
                AssetConverterConfig = config,
                Config = config.WebBasedGeneratorConfig
            };

            CreateFakeImageFile(config, "en", "TestSet", docConfig, "card1");

            var facePath = ImageHelper.GetImageFileName(config, docConfig, "en", "TestSet", "card1");

            var harvest = new CardSetHarvest();
            harvest.Faces.Images.TryAdd("card1", facePath);
            
            var harvestDictionary = new ConcurrentDictionary<(string, string), Func<CardSetHarvest>>();
            harvestDictionary.TryAdd(("TestSet", "en"), () => harvest);

            // Act
            var result = sut.GenerateDocumentImages(harvestDictionary);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            var imageList = result.First().Value;
            imageList.Should().HaveCount(1);
            imageList[0].Front.Should().EndWith("card1.png");
            imageList[0].Back.Should().BeNull();
        }

        [Fact]
        public void GenerateDocumentImages_WhenFileCreationIsSkipped_ShouldStillRun()
        {
            // Arrange
            var docConfig = new CardSetDocumentConfig { DocumentName = "TestDoc", Enabled = true, NoBack = false, CardSets = new List<DocumentCardSet> { new DocumentCardSet { CardSetName = "FailingSet" } } };
            var configPath = SetupTestConfiguration(new List<CardSetDocumentConfig> { docConfig });

            var config = AssetConverterConfig.GetConfig(configPath, out _);
            var sut = new ImageFileGenerator
            {
                AssetConverterConfig = config,
                Config = config.WebBasedGeneratorConfig
            };

            var harvest = new CardSetHarvest();
            // Pour ce test, on veut toujours simuler un échec, donc on garde une URL invalide.
            harvest.Faces.Images.TryAdd("card1", "http://url-that-would-fail");

            var harvestDictionary = new ConcurrentDictionary<(string, string), Func<CardSetHarvest>>();
            harvestDictionary.TryAdd(("FailingSet", "en"), () => harvest);
            
            // Act
            ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>> result = null;
            Action act = () => result = sut.GenerateDocumentImages(harvestDictionary);

            // Assert
            act.Should().NotThrow("because the process should handle errors gracefully.");
            
            result.Should().NotBeNull();
            _output.WriteLine($"Result dictionary contains {result.Count} entries.");
            var imageList = result.First().Value;
            _output.WriteLine($"The resulting image list for the first entry contains {imageList.Count} item(s).");
            
            imageList.Should().BeEmpty();
        }

        public void Dispose()
        {
            if (Directory.Exists(_testOutputDir))
            {
                Directory.Delete(_testOutputDir, true);
            }
        }
    }
}