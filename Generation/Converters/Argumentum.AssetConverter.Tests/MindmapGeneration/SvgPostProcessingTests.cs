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

        [Fact(DisplayName = "ProcessSvgFilesAsync should return a valid SVG file that matches the approved snapshot")]
        public async Task ProcessSvgFilesAsync_WithSimpleSvg_ShouldMatchApprovedSnapshot()
        {
            // Arrange
            var config = new FallacyMindMapDocumentConfig();
            config.SVGMaps.Add(new SVGFreemindMap { DocumentName = "snapshot.svg" });
            var testFilePath = await SetupTestFileAsync("Argumentum.AssetConverter.Tests.Assets.Mindmap.sample_fallacy_map.svg");
            
            var processedSvgDocs = await config.ProcessSvgFilesAsync(new[] { testFilePath });
            var processedSvgContent = FallacyMindMapDocumentConfig.GetSvgContent(processedSvgDocs.Values.First());

            // Assert — the approved snapshot is committed and copied to output (csproj snapshots\** rule);
            // a missing snapshot must fail the test, never be regenerated here (#1046 HIGH #2).
            var snapshotDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "snapshots");
            var snapshotFile = Path.Combine(snapshotDirectory, "sample_fallacy_map.snapshot.svg");

            Assert.True(File.Exists(snapshotFile),
                $"Approved snapshot not found at {snapshotFile} — the csproj snapshots copy rule or the committed snapshot is missing.");
            Assert.False(string.IsNullOrWhiteSpace(await File.ReadAllTextAsync(snapshotFile)),
                "Approved snapshot is empty or whitespace — degenerate snapshot, regenerate from a real run output.");

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