using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Argumentum.AssetConverter.Mindmapper;
using Argumentum.AssetConverter.Entities;
using CsvHelper;
using CsvHelper.Configuration; // Required for configuration
using System.Globalization;
using FluentAssertions;
using Argumentum.AssetConverter;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    public class MmGeneratorTests
    {
        private readonly AssetConverterConfig _config;
        private readonly string _tempTestDirectory;

        public MmGeneratorTests()
        {
            _config = new AssetConverterConfig { OverwriteExistingDocs = true };
            _config.LocalizationConfig.DefaultLanguage = "fr"; // Set default language for test consistency
            _tempTestDirectory = Path.Combine(Path.GetTempPath(), "MmGeneratorTests", Path.GetRandomFileName());
            Directory.CreateDirectory(_tempTestDirectory);
        }

        private async Task<List<Fallacy>> GetTestDataAsync(string csvFileName)
        {
            var csvPath = Path.Combine("Assets", csvFileName);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Ignore headers that are in the class map but not in the CSV file
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<FallacyClassMap>();
            
            var records = new List<Fallacy>();
            await foreach (var record in csv.GetRecordsAsync<Fallacy>())
            {
                records.Add(record);
            }
            return records;
        }

        [Fact]
        public async Task GenerateMmFile_WithValidCsv_ShouldProduceWellFormedXmlWithCorrectIdsAndText()
        {
            // Arrange
            var fallacies = await GetTestDataAsync("simple-fallacies.csv");
            var generator = new FallacyMindMapDocumentConfig
            {
                DocumentName = "test-fallacies.mm",
                CrossLinks = CrossLink.None // Simplify test
            };
            
            generator.TitleExpression = "{item.TextFr}";

            // Act
            await generator.GenerateMindMapFile(fallacies, _config, _tempTestDirectory, "fr");
            var generatedFilePath = Path.Combine(_tempTestDirectory, generator.DocumentName);

            // Assert
            File.Exists(generatedFilePath).Should().BeTrue("because the mindmap file should have been created.");

            var mmContent = await File.ReadAllTextAsync(generatedFilePath);
            mmContent.Should().NotBeNullOrEmpty();

            var xmlDoc = XDocument.Parse(mmContent);
            xmlDoc.Should().NotBeNull();
            xmlDoc.Root.Name.LocalName.Should().Be("map");

            // Validate root node separately if it has a special, hardcoded ID
            xmlDoc.Root.Element("node")?.Attribute("TEXT")?.Value.Should().Be("Sophismes");
            
            foreach (var fallacy in fallacies)
            {
                if (string.IsNullOrEmpty(fallacy.Id) || fallacy.Id == "ROOT") continue;

                var node = xmlDoc.XPathSelectElement($"//node[@ID='{fallacy.Id}']");
                node.Should().NotBeNull($"because a node for fallacy with ID '{fallacy.Id}' should exist.");

                var textAttribute = node.Attribute("TEXT")?.Value;
                textAttribute.Should().Be(fallacy.TextFr, $"because the node text should match the fallacy TextFr '{fallacy.TextFr}'.");
            }
        }

        [Fact]
        public async Task CreateFreemindmap_ViaPublicApi_ShouldCreateNonEmptyFile()
        {
            // Arrange
            var fallacies = await GetTestDataAsync("simple-fallacies.csv");
            // S'assurer qu'il y a au moins une donnée pour le test
            fallacies.Should().NotBeEmpty();

            var generator = new FallacyMindMapDocumentConfig
            {
                DocumentName = "safety-net-test.mm",
            };

            // Act
            // On appelle la méthode publique qui, en interne, appelle CreateFreemindmap
            await generator.GenerateMindMapFile(fallacies, _config, _tempTestDirectory, "fr");
            var generatedFilePath = Path.Combine(_tempTestDirectory, generator.DocumentName);

            // Assert
            // 1. Valide que le fichier est créé (ne lève pas d'exception et existe)
            File.Exists(generatedFilePath).Should().BeTrue();

            var fileInfo = new FileInfo(generatedFilePath);
            fileInfo.Length.Should().BeGreaterThan(0, "Le fichier ne doit pas être vide.");

            // 2. Valide la structure de base (contient des noeuds)
            var mmContent = await File.ReadAllTextAsync(generatedFilePath);
            var xmlDoc = XDocument.Parse(mmContent);
            xmlDoc.Root.Should().NotBeNull();
            xmlDoc.Root.Element("node").Should().NotBeNull("La map doit contenir au moins un noeud racine.");
            xmlDoc.Root.Element("node").Elements("node").Should().NotBeEmpty("La map doit contenir des noeuds enfants pour les données d'entrée.");
        }
    }
}