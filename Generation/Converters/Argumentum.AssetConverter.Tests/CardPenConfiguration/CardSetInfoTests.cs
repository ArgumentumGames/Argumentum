using Xunit;
using FluentAssertions;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.Tests.CardPenConfiguration
{
	/// <summary>
	/// Tests pour CardSetInfo, notamment la transformation des chemins
	/// pour l'utilisation avec CardPen local vs distant.
	/// </summary>
	public class CardSetInfoTests : IDisposable
	{
		private readonly string _tempDir;
		private readonly string _testJsonPath;

		public CardSetInfoTests()
		{
			// Créer un répertoire temporaire pour les tests
			_tempDir = Path.Combine(Path.GetTempPath(), $"CardSetInfoTests_{Guid.NewGuid():N}");
			Directory.CreateDirectory(_tempDir);
			_testJsonPath = Path.Combine(_tempDir, "test_cardset.json");
		}

		public void Dispose()
		{
			// Nettoyer après les tests
			if (Directory.Exists(_tempDir))
			{
				Directory.Delete(_tempDir, true);
			}
		}

		[Fact]
		public async Task GetCardSetDocument_WithUseLocalCardpen_ShouldTransformPaths()
		{
			// Arrange
			var cardSetDoc = new CardSetDocument
			{
				name = "Test CardSet",
				css = "../../Cards/Fallacies/style.css",
				mustache = "../../Cards/Fallacies/template.html",
				extCSS = "../../Cards/Fallacies/external.css"
			};

			// Sérialiser le document dans un fichier JSON temporaire
			var json = JsonSerializer.Serialize(cardSetDoc);
			await File.WriteAllTextAsync(_testJsonPath, json);

			var cardSetInfo = new CardSetInfo
			{
				JsonFilePathDebug = _testJsonPath,
				JsonFilePathRelease = _testJsonPath
			};

			var config = new AssetConverterConfig
			{
				ForceDebugParams = true,
				WebBasedGeneratorConfig = new WebBasedGeneratorConfig
				{
					UseLocalCardpen = true
				}
			};

			// Act
			var result = await cardSetInfo.GetCardSetDocument(config);

			// Assert
			result.Should().NotBeNull();
			result.CardSetDocument.css.Should().Be("/Cards/Fallacies/style.css",
				"car les chemins relatifs doivent être transformés en chemins absolus web");
			result.CardSetDocument.mustache.Should().Be("/Cards/Fallacies/template.html",
				"car les chemins relatifs doivent être transformés en chemins absolus web");
			result.CardSetDocument.extCSS.Should().Be("/Cards/Fallacies/external.css",
				"car les chemins relatifs doivent être transformés en chemins absolus web");
		}

		[Fact]
		public async Task GetCardSetDocument_WithoutUseLocalCardpen_ShouldNotTransformPaths()
		{
			// Arrange
			var cardSetDoc = new CardSetDocument
			{
				name = "Test CardSet",
				css = "../../Cards/Fallacies/style.css",
				mustache = "../../Cards/Fallacies/template.html",
				extCSS = "../../Cards/Fallacies/external.css"
			};

			// Sérialiser le document dans un fichier JSON temporaire
			var json = JsonSerializer.Serialize(cardSetDoc);
			await File.WriteAllTextAsync(_testJsonPath, json);

			var cardSetInfo = new CardSetInfo
			{
				JsonFilePathDebug = _testJsonPath,
				JsonFilePathRelease = _testJsonPath
			};

			var config = new AssetConverterConfig
			{
				ForceDebugParams = true,
				WebBasedGeneratorConfig = new WebBasedGeneratorConfig
				{
					UseLocalCardpen = false  // Mode distant (GitHub Pages)
				}
			};

			// Act
			var result = await cardSetInfo.GetCardSetDocument(config);

			// Assert
			result.Should().NotBeNull();
			result.CardSetDocument.css.Should().Be("../../Cards/Fallacies/style.css",
				"car les chemins ne doivent PAS être transformés en mode distant");
			result.CardSetDocument.mustache.Should().Be("../../Cards/Fallacies/template.html",
				"car les chemins ne doivent PAS être transformés en mode distant");
			result.CardSetDocument.extCSS.Should().Be("../../Cards/Fallacies/external.css",
				"car les chemins ne doivent PAS être transformés en mode distant");
		}

		[Fact]
		public async Task GetCardSetDocument_WithNullFields_ShouldNotThrow()
		{
			// Arrange
			var cardSetDoc = new CardSetDocument
			{
				name = "Test CardSet",
				css = null,
				mustache = null,
				extCSS = null
			};

			// Sérialiser le document dans un fichier JSON temporaire
			var json = JsonSerializer.Serialize(cardSetDoc);
			await File.WriteAllTextAsync(_testJsonPath, json);

			var cardSetInfo = new CardSetInfo
			{
				JsonFilePathDebug = _testJsonPath,
				JsonFilePathRelease = _testJsonPath
			};

			var config = new AssetConverterConfig
			{
				ForceDebugParams = true,
				WebBasedGeneratorConfig = new WebBasedGeneratorConfig
				{
					UseLocalCardpen = true
				}
			};

			// Act
			var result = await cardSetInfo.GetCardSetDocument(config);

			// Assert
			result.Should().NotBeNull();
			result.CardSetDocument.css.Should().BeNull();
			result.CardSetDocument.mustache.Should().BeNull();
			result.CardSetDocument.extCSS.Should().BeNull();
		}

		[Fact]
		public async Task GetCardSetDocument_WithEmptyFields_ShouldNotTransform()
		{
			// Arrange
			var cardSetDoc = new CardSetDocument
			{
				name = "Test CardSet",
				css = string.Empty,
				mustache = string.Empty,
				extCSS = string.Empty
			};

			// Sérialiser le document dans un fichier JSON temporaire
			var json = JsonSerializer.Serialize(cardSetDoc);
			await File.WriteAllTextAsync(_testJsonPath, json);

			var cardSetInfo = new CardSetInfo
			{
				JsonFilePathDebug = _testJsonPath,
				JsonFilePathRelease = _testJsonPath
			};

			var config = new AssetConverterConfig
			{
				ForceDebugParams = true,
				WebBasedGeneratorConfig = new WebBasedGeneratorConfig
				{
					UseLocalCardpen = true
				}
			};

			// Act
			var result = await cardSetInfo.GetCardSetDocument(config);

			// Assert
			result.Should().NotBeNull();
			result.CardSetDocument.css.Should().BeEmpty();
			result.CardSetDocument.mustache.Should().BeEmpty();
			result.CardSetDocument.extCSS.Should().BeEmpty();
		}

		[Fact]
		public async Task GetCardSetDocument_WithPartialPathMatch_ShouldOnlyTransformMatchingParts()
		{
			// Arrange
			var cardSetDoc = new CardSetDocument
			{
				name = "Test CardSet",
				css = "../../Cards/Fallacies/style.css and more ../../Cards/test.css",
				mustache = "No transformation needed here",
				extCSS = "../../Cards/external.css"
			};

			// Sérialiser le document dans un fichier JSON temporaire
			var json = JsonSerializer.Serialize(cardSetDoc);
			await File.WriteAllTextAsync(_testJsonPath, json);

			var cardSetInfo = new CardSetInfo
			{
				JsonFilePathDebug = _testJsonPath,
				JsonFilePathRelease = _testJsonPath
			};

			var config = new AssetConverterConfig
			{
				ForceDebugParams = true,
				WebBasedGeneratorConfig = new WebBasedGeneratorConfig
				{
					UseLocalCardpen = true
				}
			};

			// Act
			var result = await cardSetInfo.GetCardSetDocument(config);

			// Assert
			result.Should().NotBeNull();
			result.CardSetDocument.css.Should().Be("/Cards/Fallacies/style.css and more /Cards/test.css",
				"car toutes les occurrences de ../../Cards/ doivent être transformées");
			result.CardSetDocument.mustache.Should().Be("No transformation needed here",
				"car il n'y a pas de pattern ../../Cards/ à transformer");
			result.CardSetDocument.extCSS.Should().Be("/Cards/external.css",
				"car le pattern ../../Cards/ doit être transformé");
		}
	}
}
