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
            _config = new AssetConverterConfig
            {
                OverwriteExistingDocs = true,
                FreeMindPath = "",  // Disable FreeMind GUI in tests — use XSLT fallback only
                FreeplanePath = "", // Disable Freeplane GUI in tests
            };
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
                xmlDoc.Root!.Name.LocalName.Should().Be("map");

                // Validate root node (TitleFunc uses TitleExpression = "{item.TextFr}")
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
                xmlDoc.Root!.Element("node")!.Elements("node").Should().NotBeEmpty("La map doit contenir des noeuds enfants pour les données d'entrée.");
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
                xmlDoc.Root!.Element("node")!.Elements("node").Should().NotBeEmpty("the map should contain child nodes.");

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

        /// <summary>
        /// Loads Virtue records from an absolute CSV path (used to feed the real taxonomy CSV,
        /// which carries the fully-translated <c>*_ar/_fa/_zh</c> columns, into the mind-map path).
        /// </summary>
        private static async Task<List<Virtue>> LoadVirtuesFromPathAsync(string csvPath)
        {
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

        /// <summary>
        /// Walks up from the test bin directory to locate the committed Virtues taxonomy CSV
        /// (<c>Cards/Fallacies/Argumentum Virtues - Taxonomy.csv</c>, at the repo root in every checkout).
        /// </summary>
        private static string? FindRepoVirtuesCsv()
        {
            var dir = new DirectoryInfo(System.AppContext.BaseDirectory);
            for (int i = 0; i < 12 && dir != null; i++, dir = dir.Parent)
            {
                var candidate = Path.Combine(dir.FullName, "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");
                if (File.Exists(candidate)) return candidate;
            }
            return null;
        }

        /// <summary>
        /// #665 empirical guard — the Virtue mind map must render NATIVE script (not French fallback)
        /// for ar/fa/zh once the entity + MindMapLocalization tables are wired. Generates a real .mm
        /// from the actual taxonomy CSV (FreeMind GUI disabled via <c>FreeMindPath=""</c>), applying the
        /// production localization exactly as the pipeline does, and asserts the target Unicode block is
        /// present in the node text. This is the regression that would have caught the "Virtues mind maps
        /// in French instead of the target languages" defect.
        /// </summary>
        [Theory]
        [InlineData("ar", 0x0600, 0x06FF)] // Arabic block
        [InlineData("fa", 0x0600, 0x06FF)] // Persian (Arabic script + Persian extensions, both in this block)
        [InlineData("zh", 0x4E00, 0x9FFF)] // CJK Unified Ideographs
        public async Task VirtueMindMap_GeneratesNativeScript_ForArFaZh(string lang, int lo, int hi)
        {
            // Arrange — real taxonomy CSV (carries the translated *_ar/_fa/_zh columns).
            var csvPath = FindRepoVirtuesCsv();
            csvPath.Should().NotBeNull("the committed Virtues taxonomy CSV must be locatable from the test bin dir");

            var virtues = await LoadVirtuesFromPathAsync(csvPath!);
            virtues.Should().NotBeEmpty();

            // Apply the production MindMapLocalization for the target language (rewrites the FR-suffixed
            // source tokens in the expressions to the per-language Virtue properties), as the pipeline does.
            var generator = new VirtueMindMapDocumentConfig { DocumentName = $"virtues-{lang}.mm" };
            foreach (var localization in _config.LocalizationConfig.MindMapLocalization)
            {
                localization.DoReflectionTranslate(generator, lang);
            }

            var originalInteractive = Program.IsInteractive;
            try
            {
                Program.IsInteractive = false; // no interactive SVG prompt
                // Act — .mm is serialized before any SVG export; FreeMindPath="" ⇒ no GUI, XSLT throwaway.
                await generator.GenerateMindMapFile(virtues, _config, _tempTestDirectory, lang);
            }
            finally
            {
                Program.IsInteractive = originalInteractive;
            }

            var mmPath = Path.Combine(_tempTestDirectory, generator.DocumentName);
            File.Exists(mmPath).Should().BeTrue($"the {lang} Virtue mind map .mm must be generated");
            var mm = await File.ReadAllTextAsync(mmPath);

            // Assert — native script present in node text (would be ~0 if it fell back to French).
            var nativeCount = mm.Count(ch => ch >= lo && ch <= hi);
            nativeCount.Should().BeGreaterThan(50,
                $"the generated '{lang}' Virtue mind map must contain native-script node text " +
                $"(Unicode U+{lo:X4}–U+{hi:X4}), not French fallback — got {nativeCount} native code points");
        }

        [Theory]
        [InlineData("en", "TextEn", "DescEn", "ExampleEn", "LinkEnFallback", "Subfamily", "Subsubfamily", "Family")]
        [InlineData("ru", "TextRu", "DescRu", "Exampleru", "LinkRuFallback", "SubfamilyRu", "SubsubfamilyRu", "FamilyRu")]
        [InlineData("pt", "TextPt", "DescPt", "ExamplePt", "LinkPtFallback", "SubfamilyPt", "SubsubfamilyPt", "FamilyPt")]
        public void MindMapLocalization_ShouldTranslateAllFallacyExpressions(
            string lang, string expectedText, string expectedDesc, string expectedExample,
            string expectedLink, string expectedSubFamily, string expectedSubSubFamily, string expectedFamily)
        {
            // Arrange
            var config = new FallacyMindMapDocumentConfig();

            // Act - apply all localizations like the pipeline does
            foreach (var localization in _config.LocalizationConfig.MindMapLocalization)
            {
                localization.DoReflectionTranslate(config, lang);
            }

            // Assert - text fields
            config.TitleExpression.Should().Contain(expectedText,
                $"TitleExpression should reference {expectedText} for language '{lang}'");
            config.DescriptionExpression.Should().Contain(expectedDesc,
                $"DescriptionExpression should reference {expectedDesc} for language '{lang}'");
            config.ExampleExpression.Should().Contain(expectedExample,
                $"ExampleExpression should reference {expectedExample} for language '{lang}'");
            config.LinkExpression.Should().Contain(expectedLink,
                $"LinkExpression should reference {expectedLink} for language '{lang}'");

            // Assert - family hierarchy
            config.FamilleExpression.Should().Contain(expectedFamily,
                $"FamilleExpression should reference {expectedFamily} for language '{lang}'");
            config.SousFamilleExpression.Should().Contain(expectedSubFamily,
                $"SousFamilleExpression should reference {expectedSubFamily} for language '{lang}'");
            config.SoussousFamilleExpression.Should().Contain(expectedSubSubFamily,
                $"SoussousFamilleExpression should reference {expectedSubSubFamily} for language '{lang}'");
        }

        [Fact]
        public void MindMapLocalization_FrenchDefaults_ShouldUseFrProperties()
        {
            // Verify the default (FR) expressions use concrete FR property names
            var config = new FallacyMindMapDocumentConfig();

            config.TitleExpression.Should().Contain("TextFr");
            config.DescriptionExpression.Should().Contain("DescFr");
            config.ExampleExpression.Should().Contain("ExampleFr");
            config.LinkExpression.Should().Contain("LinkFrFallback");
            config.FamilleExpression.Should().Contain("Famille");
            config.SousFamilleExpression.Should().Contain("SousFamille");
            config.SoussousFamilleExpression.Should().Contain("Soussousfamille");
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