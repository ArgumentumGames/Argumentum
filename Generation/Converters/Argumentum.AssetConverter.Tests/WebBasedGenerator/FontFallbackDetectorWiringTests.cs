using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Source-text organ for the wiring of the #1485 font probe in the harvest. The binding
    /// organ (<c>FontFallbackDetectorBindingTests</c>) proves that the detector measures; this
    /// one proves that the harvest CALLS it while the iframe still holds the faces, then the
    /// backs, for every card set — and that a probe that cannot run is logged as a Problem.
    /// The #190 overflow detector stayed dead for months behind a call that existed, a catch
    /// that swallowed, and a log line nobody saw.
    /// </summary>
    public class FontFallbackDetectorWiringTests
    {
        private const string FaceProbeCall = "await ProbeFonts(page, configCardSet, currentLanguage ?? \"\", \"face\");";
        private const string BackProbeCall = "await ProbeFonts(page, configCardSet, currentLanguage ?? \"\", \"back\");";

        private static string Source(string fileName)
            => File.ReadAllText(Path.Combine(TestRepoRoot.Find(),
                "Generation", "Converters", "Argumentum.AssetConverter", "WebBasedGenerator", "Cardpen", fileName));

        [Fact]
        public void Harvest_ProbesTheFacesBeforeTheBacksOverwriteTheIframe_ThenTheBacks()
        {
            var source = Source("HarvestManager.cs");

            var facesAssigned = source.IndexOf("currentHarvest.Faces = faces;", StringComparison.Ordinal);
            var backsAssigned = source.IndexOf("currentHarvest.Backs = backs;", StringComparison.Ordinal);
            var released = source.IndexOf("page.Console -= Page_Console;", StringComparison.Ordinal);
            facesAssigned.Should().BeGreaterThan(0, "the faces are still assigned to the harvest");
            backsAssigned.Should().BeGreaterThan(facesAssigned, "the backs are generated after the faces");
            released.Should().BeGreaterThan(backsAssigned, "the page goes back to the pool after both");

            var faceProbe = source.IndexOf(FaceProbeCall, facesAssigned, StringComparison.Ordinal);
            var backProbe = source.IndexOf(BackProbeCall, backsAssigned, StringComparison.Ordinal);

            faceProbe.Should().BeInRange(facesAssigned, backsAssigned,
                "the face DOM must still be in the iframe when it is probed — the back card set overwrites it");
            backProbe.Should().BeInRange(backsAssigned, released,
                "the back DOM must be probed before the page goes back to the pool");
            Count(source, "await ProbeFonts(").Should().Be(2, "one probe per side, exactly");
            source.IndexOf("await ProbeFonts(", StringComparison.Ordinal).Should().Be(faceProbe);
            source.LastIndexOf("await ProbeFonts(", StringComparison.Ordinal).Should().Be(backProbe);
        }

        [Fact]
        public void Harvest_ProbesEveryCardSet_WithoutAGate()
        {
            var source = Source("HarvestManager.cs");
            var facesAssigned = source.IndexOf("currentHarvest.Faces = faces;", StringComparison.Ordinal);
            var faceProbe = source.IndexOf(FaceProbeCall, facesAssigned, StringComparison.Ordinal);
            faceProbe.Should().BeGreaterThan(facesAssigned);

            // Between the capture of the faces and the probe call there is no condition. The
            // overflow detector is gated on two decks; the font probe is not — a back, a rule
            // card and a scenario all carry letters.
            source.Substring(facesAssigned, faceProbe - facesAssigned).Should().NotContain("if (");
        }

        [Fact]
        public void Harvest_ReportsEveryFindingAndEveryFailureAsAProblem()
        {
            var source = Source("HarvestManager.cs");

            source.Should().Contain(
                "Logger.Log(line.Message, line.IsProblem ? MessageType.Problem : MessageType.Info);",
                "a finding, a face in error and an empty probe are Problems — never a plain Log line");
            source.Should().Contain(
                "Logger.Log($\"[FONT-PROBE] {where}: the font probe did not run — {ex.Message}\", MessageType.Problem);",
                "a probe that cannot run must not be swallowed at Info level");
            source.Should().Contain("Path.Combine(AssetConverterConfig.GetBaseTargetDirectory(language), \"QA\")");
            source.Should().Contain("$\"{configCardSet.Name}_fonts_{side}_{language}.md\"");
        }

        [Fact]
        public void Detector_ThrowsWhenTheScriptDidNotComplete_SoNoCallSiteCanReadItAsClean()
        {
            var source = Source("FontFallbackDetector.cs");

            var okCheck = source.IndexOf(
                "!raw.TryGetProperty(\"ok\", out var ok) || ok.ValueKind != JsonValueKind.True",
                StringComparison.Ordinal);
            okCheck.Should().BeGreaterThan(0, "the script reports whether it completed");
            source.Substring(okCheck, 800).Should().Contain("throw new InvalidOperationException(",
                "ok=false is an exception, not an empty report");

            // EvaluateAsync(expression, arg) calls the expression with the ELEMENT first: the
            // script binds (element, args) and reads the element's own document.
            source.Should().Contain("private const string ProbeScript = @\"async (element, args) => {");
            source.Should().Contain("iframe.Locator(\"body\").EvaluateAsync<JsonElement>(");
        }

        private static int Count(string text, string needle)
        {
            var count = 0;
            for (var at = text.IndexOf(needle, StringComparison.Ordinal); at >= 0;
                 at = text.IndexOf(needle, at + needle.Length, StringComparison.Ordinal))
            {
                count++;
            }
            return count;
        }
    }
}
