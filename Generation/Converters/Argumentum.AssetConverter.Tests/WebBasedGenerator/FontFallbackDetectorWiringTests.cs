using System;
using System.IO;
using System.Linq;
using System.Text;
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
    /// These tests read the source with its COMMENTS REMOVED (see <see cref="WithoutComments"/>):
    /// a probe silenced with <c>//</c> still contains its call text, so the raw text matched it
    /// and the whole organ stayed green (measured 2026-11-27 by po-2024, c.7210566223: 27/27 with
    /// both calls commented out, on a probe no test executes in a real harvest).
    /// </summary>
    public class FontFallbackDetectorWiringTests
    {
        private const string FaceProbeCall = "await ProbeFonts(page, configCardSet, currentLanguage ?? \"\", \"face\");";
        private const string BackProbeCall = "await ProbeFonts(page, configCardSet, currentLanguage ?? \"\", \"back\");";

        private static string RawSource(string fileName)
            => File.ReadAllText(Path.Combine(TestRepoRoot.Find(),
                "Generation", "Converters", "Argumentum.AssetConverter", "WebBasedGenerator", "Cardpen", fileName));

        private static string Source(string fileName) => WithoutComments(RawSource(fileName));

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

        /// <summary>
        /// The inverse control the wiring guard was missing: silence one probe with a line comment
        /// and the guard must read the call as ABSENT. The mutation is applied to the real source
        /// text, and the text is asserted to still carry the call afterwards — otherwise the test
        /// would pass on a mutation that silently did nothing.
        /// </summary>
        [Theory]
        [InlineData("face")]
        [InlineData("back")]
        public void WiringGuard_ReadsACommentedOutProbeCallAsAbsent(string side)
        {
            var raw = RawSource("HarvestManager.cs");
            var call = side == "face" ? FaceProbeCall : BackProbeCall;

            raw.Should().Contain(call);
            WithoutComments(raw).Should().Contain(call, "the guard sees a live call");

            var at = raw.IndexOf(call, StringComparison.Ordinal);
            var lineStart = raw.LastIndexOf('\n', at) + 1;
            var commented = raw.Insert(lineStart, "// ");

            commented.Length.Should().Be(raw.Length + 3, "the mutation landed: three characters added");
            commented.Substring(lineStart).TrimStart().Should().StartWith("//",
                "the probe now sits behind a line comment");
            commented.Should().Contain(call, "commenting out does not delete the text — that is the whole trap");
            WithoutComments(commented).Should().NotContain(call,
                "a probe silenced by a comment must read as absent, or the organ is green on a dead probe");
        }

        /// <summary>
        /// Same trap, other comment form: a block comment swallows the call just as well.
        /// </summary>
        [Fact]
        public void WiringGuard_ReadsABlockCommentedProbeCallAsAbsent()
        {
            var raw = RawSource("HarvestManager.cs");
            var at = raw.IndexOf(FaceProbeCall, StringComparison.Ordinal);
            var lineStart = raw.LastIndexOf('\n', at) + 1;
            var lineEnd = raw.IndexOf('\n', at);

            var commented = raw.Substring(0, lineStart) + "/* " + raw.Substring(lineStart, lineEnd - lineStart)
                                                       + " */" + raw.Substring(lineEnd);

            commented.Should().Contain("/* ", "the mutation landed");
            WithoutComments(commented).Should().NotContain(FaceProbeCall);
            WithoutComments(commented).Length.Should().Be(commented.Length,
                "the stripper keeps the length, whatever the comment form");
        }

        /// <summary>
        /// The stripper's own guard, because a comment remover that eats too much would quietly
        /// blind every test of this class. Two traps measured on the real file: a URL inside a
        /// normal string, and a multi-line verbatim string holding the harvest script.
        /// </summary>
        [Fact]
        public void WithoutComments_KeepsTheCodeAndEatsOnlyTheProse()
        {
            var raw = RawSource("HarvestManager.cs");
            var stripped = WithoutComments(raw);

            stripped.Length.Should().Be(raw.Length, "same length: a position taken on one text means the same place in the file");
            stripped.Count(c => c == '\n').Should().Be(raw.Count(c => c == '\n'), "line numbers survive");

            stripped.Should().Contain(FaceProbeCall).And.Contain(BackProbeCall);
            stripped.Should().Contain("\"https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/\"",
                "a // inside a string literal is not a C# comment — eating it truncates the line");
            stripped.Should().Contain("string script = @\"", "the multi-line verbatim string is copied verbatim");
            stripped.Should().NotContain("#1485: which fonts actually paint the letters of the face DOM",
                "and the prose is gone — otherwise this stripper removes nothing");
        }

        /// <summary>
        /// Every comment becomes spaces and newlines stay: the text keeps the length of the file
        /// it came from, so an index taken on one is valid on the other. String and character
        /// literals are copied as they are, escapes included — a URL, a JS comment inside the
        /// harvest script or an <c>@"…""…"</c> pair is not a C# comment.
        /// </summary>
        private static string WithoutComments(string source)
        {
            var stripped = new StringBuilder(source.Length);
            var i = 0;
            while (i < source.Length)
            {
                var c = source[i];
                var next = i + 1 < source.Length ? source[i + 1] : '\0';

                if (c == '/' && next == '/')
                {
                    while (i < source.Length && source[i] != '\n')
                    {
                        stripped.Append(' ');
                        i++;
                    }
                    continue;
                }

                if (c == '/' && next == '*')
                {
                    stripped.Append(' ').Append(' ');
                    i += 2;
                    while (i < source.Length)
                    {
                        if (source[i] == '*' && i + 1 < source.Length && source[i + 1] == '/')
                        {
                            stripped.Append(' ').Append(' ');
                            i += 2;
                            break;
                        }
                        stripped.Append(source[i] == '\n' ? '\n' : ' ');
                        i++;
                    }
                    continue;
                }

                // @"…", $@"…", @$"…" — doubled quote is the escape, the backslash is literal
                var quoteAt = -1;
                if (c == '@' && next == '"')
                {
                    quoteAt = i + 1;
                }
                else if ((c == '$' && next == '@') || (c == '@' && next == '$'))
                {
                    if (i + 2 < source.Length && source[i + 2] == '"') quoteAt = i + 2;
                }

                if (quoteAt >= 0)
                {
                    while (i <= quoteAt)
                    {
                        stripped.Append(source[i]);
                        i++;
                    }
                    while (i < source.Length)
                    {
                        if (source[i] == '"' && i + 1 < source.Length && source[i + 1] == '"')
                        {
                            stripped.Append('"').Append('"');
                            i += 2;
                            continue;
                        }
                        if (source[i] == '"')
                        {
                            stripped.Append('"');
                            i++;
                            break;
                        }
                        stripped.Append(source[i]);
                        i++;
                    }
                    continue;
                }

                if (c == '"' || c == '\'')
                {
                    var quote = c;
                    stripped.Append(quote);
                    i++;
                    while (i < source.Length)
                    {
                        if (source[i] == '\\' && i + 1 < source.Length)
                        {
                            stripped.Append(source[i]).Append(source[i + 1]);
                            i += 2;
                            continue;
                        }
                        if (source[i] == quote)
                        {
                            stripped.Append(quote);
                            i++;
                            break;
                        }
                        stripped.Append(source[i]);
                        i++;
                    }
                    continue;
                }

                stripped.Append(c);
                i++;
            }
            return stripped.ToString();
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
