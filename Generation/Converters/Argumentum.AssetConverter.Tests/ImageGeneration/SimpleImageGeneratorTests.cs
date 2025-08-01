using Xunit;
using System.IO;
using System.Threading.Tasks;
using Argumentum.AssetConverter;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using FluentAssertions;
using System.Collections.Concurrent;
using System;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter.Tests.ImageGeneration
{
    public class SimpleImageGeneratorTests
    {
        private const string TestOutputDirectory = "TestOutput/SimpleImageGeneratorTests";

        public SimpleImageGeneratorTests()
        {
            if (Directory.Exists(TestOutputDirectory))
            {
                Directory.Delete(TestOutputDirectory, true);
            }
            Directory.CreateDirectory(TestOutputDirectory);
        }

        private AssetConverterConfig LoadConfig()
        {
            var configPath = Path.GetFullPath("Assets/SimpleImageGenerator/test-config.json");
            var jsonString = File.ReadAllText(configPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
            var config = JsonSerializer.Deserialize<AssetConverterConfig>(jsonString, options);
            
            // Override the output dir to our test-specific one
            config.BaseTargetDirectoryName = TestOutputDirectory;

            return config;
        }

        private CardSetHarvest LoadTestHarvest()
        {
            var harvest = new CardSetHarvest
            {
                Faces = new CardPenHarvest
                {
                    Images = { { "1.1", "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=" } }
                }
            };
            return harvest;
        }

        [Fact]
        public void GenerateSingleImage_ShouldCreateNonEmptyPngFile()
        {
            // Arrange
            var config = LoadConfig();
            var document = config.WebBasedGeneratorConfig.CardSetDocuments.First();
            var cardSet = document.CardSets.First();
            var language = config.LocalizationConfig.DefaultLanguage;

            var harvestDictionary = new ConcurrentDictionary<(string, string), Func<CardSetHarvest>>();
            harvestDictionary.TryAdd((cardSet.CardSetName, language), LoadTestHarvest);

            var generator = new ImageFileGenerator
            {
                AssetConverterConfig = config,
                Config = config.WebBasedGeneratorConfig
            };

            // Act
            generator.GenerateDocumentImages(harvestDictionary);

            // Assert
            var densityDirectory = document.GetDensityDirectory(Path.Combine(TestOutputDirectory, config.GetImagesDirectory(language)));
            var expectedImagePath = Path.Combine(densityDirectory, cardSet.CardSetName, "1.1_face.png");

            Assert.True(File.Exists(expectedImagePath), $"Image file should exist at {expectedImagePath}");
            var fileInfo = new FileInfo(expectedImagePath);
            fileInfo.Length.Should().BeGreaterThan(0, "Image file should not be empty.");
        }
    }
}