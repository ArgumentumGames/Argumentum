using System.Threading.Tasks;
using Xunit;
using System.IO;
using System.Reflection;
using Argumentum.AssetConverter.Mindmapper;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    public class SvgPostProcessingTests
    {
        private readonly ITestOutputHelper _output;

        public SvgPostProcessingTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ProcessSvgFilesAsync_ShouldMatchVerifiedSnapshot()
        {
            // Arrange
            var config = new FallacyMindMapDocumentConfig();
            config.SVGMaps.Add(new SVGFreemindMap { DocumentName = "snapshot.svg" });
            var testFilePath = await SetupTestFileAsync("Argumentum.AssetConverter.Tests.Assets.Mindmap.sample_fallacy_map.svg");
            
            _output.WriteLine($"Using temporary SVG file at: {testFilePath}");

            var processedSvgDocs = await config.ProcessSvgFilesAsync(new[] { testFilePath });
            var processedSvgContent = FallacyMindMapDocumentConfig.GetSvgContent(processedSvgDocs.Values.First());

            // Assert
            var snapshotDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "snapshots");
            Directory.CreateDirectory(snapshotDirectory);
            var snapshotFile = Path.Combine(snapshotDirectory, "SvgPostProcessing.snapshot.svg");
            var receivedFile = Path.Combine(snapshotDirectory, "SvgPostProcessing.received.svg");

            await File.WriteAllTextAsync(receivedFile, processedSvgContent);

            if (!File.Exists(snapshotFile))
            {
                Assert.Fail($"Snapshot file not found. Review the received file and rename it to .snapshot.svg:\n{receivedFile}");
            }

            var expected = await File.ReadAllTextAsync(snapshotFile);
            Assert.Equal(expected, processedSvgContent);
        }

        private async Task<string> SetupTestFileAsync(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new FileNotFoundException($"Embedded resource not found: {resourceName}");

                using (var reader = new StreamReader(stream))
                {
                    var svgContent = await reader.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(svgContent))
                    {
                        throw new InvalidDataException("The embedded resource stream is empty or contains only whitespace.");
                    }
                    _output.WriteLine($"--- SVG Content (first 200 chars) ---\n{svgContent.Substring(0, Math.Min(svgContent.Length, 200))}\n------------------------------------");
                    
                    var tempPath = Path.GetTempFileName().Replace(".tmp", ".svg");
                    await File.WriteAllTextAsync(tempPath, svgContent);
                    return tempPath;
                }
            }
        }
    }
}