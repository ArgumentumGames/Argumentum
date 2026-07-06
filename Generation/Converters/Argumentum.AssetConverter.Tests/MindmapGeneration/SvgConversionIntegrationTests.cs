using System;
using System.IO;
using System.Reflection;
using Argumentum.AssetConverter.Mindmapper;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    [Trait("Category", "Integration")]
    public class SvgConversionIntegrationTests : IDisposable
    {
        private readonly FallacyMindMapDocumentConfig _generator;
        private readonly AssetConverterConfig _config;
        private readonly string _tempTestDirectory;

        public SvgConversionIntegrationTests()
        {
            _config = new AssetConverterConfig
            {
                OverwriteExistingDocs = true,
                FreeMindPath = "",  // Disable FreeMind GUI in tests
                FreeplanePath = "", // Disable Freeplane GUI in tests
            };
            _tempTestDirectory = Path.Combine(Path.GetTempPath(), "SvgConversionTests", Path.GetRandomFileName());
            Directory.CreateDirectory(_tempTestDirectory);

            _generator = new FallacyMindMapDocumentConfig
            {
                DocumentName = "integration-test.mm"
            };
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempTestDirectory))
            {
                Directory.Delete(_tempTestDirectory, true);
            }
        }

        [Fact(Skip = "Temporairement désactivé pour isoler le crash Magick.NET")]
        public void TryAutomateSvgConversion_WithValidMmFile_ShouldReturnTrueAndCreateSvgFile()
        {
            // Arrange
            var mmPath = Path.Combine(_tempTestDirectory, _generator.DocumentName);
            var svgPath = Path.ChangeExtension(mmPath, ".svg");

            // Create a simple, valid .mm file for the test
            var mmContent = @"
<map version=""1.0.1"">
<!-- To view this file, download free mind mapping software FreeMind from http://freemind.sourceforge.net -->
<node TEXT=""Root"">
<node TEXT=""Child 1"" />
</node>
</map>";
            File.WriteAllText(mmPath, mmContent);

            // Act
            var method = typeof(FallacyMindMapDocumentConfig).GetMethod("TryAutomateSvgConversion", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(string), typeof(string), typeof(AssetConverterConfig), typeof(bool) }, null);
            Assert.NotNull(method);
            var result = (bool)method!.Invoke(_generator, new object[] { mmPath, svgPath, _config, false })!;

            // Assert
            Assert.True(result, "L'exécution du processus de conversion SVG doit réussir.");
            Assert.True(File.Exists(svgPath), "Le fichier SVG aurait dû être créé par le processus.");

            var fileInfo = new FileInfo(svgPath);
            Assert.True(fileInfo.Length > 0, "Le fichier SVG ne doit pas être vide.");
        }

        [Fact]
        public void TryXsltSvgConversion_WithValidMmFile_ShouldProduceValidSvg()
        {
            // Arrange
            var mmPath = Path.Combine(_tempTestDirectory, "xslt-test.mm");
            var svgPath = Path.Combine(_tempTestDirectory, "xslt-test.svg");

            var mmContent = @"<map version=""1.0.1"">
<node TEXT=""Fallacies"">
<node TEXT=""Ad Hominem"" POSITION=""right"">
  <node TEXT=""Tu Quoque""/>
  <node TEXT=""Poisoning the Well""/>
</node>
<node TEXT=""Straw Man"" POSITION=""left""/>
<node TEXT=""Appeal to Authority"" POSITION=""right"">
  <node TEXT=""Appeal to Celebrity""/>
</node>
</node>
</map>";
            File.WriteAllText(mmPath, mmContent);

            // Act
            var result = FallacyMindMapDocumentConfig.TryXsltSvgConversion(mmPath, svgPath);

            // Assert
            Assert.True(result, "XSLT SVG conversion should succeed.");
            Assert.True(File.Exists(svgPath), "SVG file should be created.");

            var svgContent = File.ReadAllText(svgPath);
            Assert.Contains("<svg", svgContent);
            Assert.Contains("Fallacies", svgContent);
            Assert.Contains("Ad Hominem", svgContent);
            Assert.Contains("Tu Quoque", svgContent);
            Assert.Contains("Straw Man", svgContent);
            Assert.Contains("Appeal to Authority", svgContent);
            Assert.Contains("Appeal to Celebrity", svgContent);

            var fileInfo = new FileInfo(svgPath);
            Assert.True(fileInfo.Length > 100, "SVG file should have meaningful content.");
        }
    }
}