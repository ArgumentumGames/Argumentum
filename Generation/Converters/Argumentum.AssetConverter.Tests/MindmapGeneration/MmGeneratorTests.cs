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
    public class MmGeneratorTests : IDisposable
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

            var originalInteractive = Program.IsInteractive;
            try
            {
                // Must disable interactive mode to prevent hang on SVG file not found prompt
                Program.IsInteractive = false;

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

                // Validate root node (TitleFunc uses DefaultTitleExpression = "{item.Text}")
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
            finally
            {
                Program.IsInteractive = originalInteractive;
            }
        }

        [Fact]
        public async Task CreateFreemindmap_ViaPublicApi_ShouldCreateNonEmptyFile()
        {
            // Arrange
            var fallacies = await GetTestDataAsync("simple-fallacies.csv");
            fallacies.Should().NotBeEmpty();

            var generator = new FallacyMindMapDocumentConfig
            {
                DocumentName = "safety-net-test.mm",
            };

            var originalInteractive = Program.IsInteractive;
            try
            {
                Program.IsInteractive = false;

                // Act
                await generator.GenerateMindMapFile(fallacies, _config, _tempTestDirectory, "fr");
                var generatedFilePath = Path.Combine(_tempTestDirectory, generator.DocumentName);

                // Assert
                File.Exists(generatedFilePath).Should().BeTrue();
                var fileInfo = new FileInfo(generatedFilePath);
                fileInfo.Length.Should().BeGreaterThan(0, "Le fichier ne doit pas être vide.");

                var mmContent = await File.ReadAllTextAsync(generatedFilePath);
                var xmlDoc = XDocument.Parse(mmContent);
                xmlDoc.Root.Should().NotBeNull();
                xmlDoc.Root.Element("node").Should().NotBeNull("La map doit contenir au moins un noeud racine.");
                xmlDoc.Root.Element("node").Elements("node").Should().NotBeEmpty("La map doit contenir des noeuds enfants pour les données d'entrée.");
            }
            finally
            {
                Program.IsInteractive = originalInteractive;
            }
        }

        [Fact]
        public async Task CreateVirtueMindmap_ShouldCreateNonEmptyFile()
        {
            // Arrange
            var virtues = await GetVirtueTestDataAsync("simple-virtues.csv");
            virtues.Should().NotBeEmpty();

            var generator = new VirtueMindMapDocumentConfig
            {
                DocumentName = "test-virtues.mm",
            };

            var originalInteractive = Program.IsInteractive;
            try
            {
                Program.IsInteractive = false;

                // Act
                await generator.GenerateMindMapFile(virtues, _config, _tempTestDirectory, "fr");
                var generatedFilePath = Path.Combine(_tempTestDirectory, generator.DocumentName);

                // Assert
                File.Exists(generatedFilePath).Should().BeTrue("the Virtue mind map file should be created.");
                var fileInfo = new FileInfo(generatedFilePath);
                fileInfo.Length.Should().BeGreaterThan(0, "the file should not be empty.");

                var mmContent = await File.ReadAllTextAsync(generatedFilePath);
                var xmlDoc = XDocument.Parse(mmContent);
                xmlDoc.Root.Should().NotBeNull();
                xmlDoc.Root.Element("node").Should().NotBeNull("the map should contain a root node.");
                xmlDoc.Root.Element("node").Elements("node").Should().NotBeEmpty("the map should contain child nodes.");

                // Verify specific virtue nodes exist
                mmContent.Should().Contain("Argument pertinent");
                mmContent.Should().Contain("Présentation intègre");
            }
            finally
            {
                Program.IsInteractive = originalInteractive;
            }
        }

        [Fact]
        public async Task XsltConversion_WithGeneratedMm_ShouldProduceValidSvg()
        {
            // Arrange - generate a real .mm from test data
            var fallacies = await GetTestDataAsync("simple-fallacies.csv");
            var generator = new FallacyMindMapDocumentConfig
            {
                DocumentName = "xslt-pipeline-test.mm",
            };

            var originalInteractive = Program.IsInteractive;
            try
            {
                Program.IsInteractive = false;
                await generator.GenerateMindMapFile(fallacies, _config, _tempTestDirectory, "fr");
            }
            finally
            {
                Program.IsInteractive = originalInteractive;
            }

            var mmPath = Path.Combine(_tempTestDirectory, generator.DocumentName);
            File.Exists(mmPath).Should().BeTrue();

            var svgPath = Path.ChangeExtension(mmPath, ".svg");

            // Act - apply XSLT conversion
            var result = FallacyMindMapDocumentConfig.TryXsltSvgConversion(mmPath, svgPath);

            // Assert
            result.Should().BeTrue("XSLT conversion should succeed on a pipeline-generated .mm file.");
            File.Exists(svgPath).Should().BeTrue();

            var svgContent = await File.ReadAllTextAsync(svgPath);
            svgContent.Should().Contain("<svg", "output should be valid SVG.");
            svgContent.Should().Contain("Sophismes", "SVG should contain root node text.");
        }

        private async Task<List<Virtue>> GetVirtueTestDataAsync(string csvFileName)
        {
            var csvPath = Path.Combine("Assets", csvFileName);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<VirtueClassMap>();

            var records = new List<Virtue>();
            await foreach (var record in csv.GetRecordsAsync<Virtue>())
            {
                records.Add(record);
            }
            return records;
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempTestDirectory))
            {
                Directory.Delete(_tempTestDirectory, true);
            }
        }
    }
}