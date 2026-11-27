using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Pure unit tests for the report, the log lines and the markdown of the #1485 font
    /// fallback detector — no Playwright here: the binding organ
    /// (<c>FontFallbackDetectorBindingTests</c>) drives the real path.
    /// </summary>
    public class FontFallbackDetectorReportTests
    {
        private const string Barlow = "\"Barlow\", sans-serif";

        private static FontStackProbe Probe(
            string verdict,
            string script = "Cyrillic",
            int rendered = 1,
            string? renderedBy = null)
        {
            // clean, generic: every letter painted or nothing declared; split: every letter
            // painted by a declared family; fallback and partial: letters left to the system.
            var missing = verdict == FontFallbackDetector.VerdictFallback || verdict == FontFallbackDetector.VerdictPartial
                ? 12
                : 0;
            var isFinding = missing > 0 || verdict == FontFallbackDetector.VerdictSplit;
            var probe = new FontStackProbe
            {
                Stack = Barlow,
                Weight = "400",
                Style = "normal",
                Script = script,
                Sample = "Логическая ошибка",
                LetterCount = 12,
                Missing = missing == 0 ? string.Empty : "Логическая ошибка",
                MissingLetterCount = missing,
                RenderedBy = renderedBy,
                Verdict = verdict,
                TextNodes = rendered,
                RenderedTextNodes = rendered,
                AffectedTextNodes = isFinding ? rendered : 0
            };
            if (verdict != FontFallbackDetector.VerdictGeneric)
            {
                probe.Coverage.Add(new FontFamilyCoverage { Family = "Barlow", Covered = 12 - missing });
            }
            return probe;
        }

        private static FontProbeReport CleanReport()
        {
            var report = new FontProbeReport { CardSetName = "Fallacies", Language = "ru", Side = "face" };
            report.Faces.Add(new FontFaceStatus { Family = "Barlow", Weight = "400", Style = "normal", Status = "loaded" });
            report.Stats = new FontProbeStats
            {
                TextNodes = 20, RenderedTextNodes = 18, HiddenTextNodes = 2, DeclaredFaces = 1, Groups = 2
            };
            return report;
        }

        [Theory]
        [InlineData(FontFallbackDetector.VerdictFallback, 1, true)]
        [InlineData(FontFallbackDetector.VerdictPartial, 1, true)]
        [InlineData(FontFallbackDetector.VerdictSplit, 1, true)]
        [InlineData(FontFallbackDetector.VerdictClean, 1, false)]
        [InlineData(FontFallbackDetector.VerdictGeneric, 1, false)]
        // Inverse control: the same defective stack, only in text that is not laid out.
        [InlineData(FontFallbackDetector.VerdictFallback, 0, false)]
        [InlineData(FontFallbackDetector.VerdictPartial, 0, false)]
        [InlineData(FontFallbackDetector.VerdictSplit, 0, false)]
        public void IsFinding_IsTrueOnlyForALaidOutStackWhoseDeclaredFamiliesLeaveLettersUnpainted(
            string verdict, int rendered, bool expected)
        {
            Probe(verdict, rendered: rendered).IsFinding.Should().Be(expected,
                "verdict {0} over {1} laid-out text node(s)", verdict, rendered);
        }

        [Fact]
        public void HasFinding_IsTrueForAFaceInErrorEvenWhenEveryStackIsClean()
        {
            var report = CleanReport();
            report.Probes.Add(Probe(FontFallbackDetector.VerdictClean, renderedBy: "Barlow"));
            report.HasFinding.Should().BeFalse("a loaded face and a clean stack are not findings");

            report.Faces.Add(new FontFaceStatus { Family = "TrendSlab", Status = "error" });
            report.Stats.DeclaredFaces = 2;
            report.Stats.ErrorFaces = 1;

            report.FindingCount.Should().Be(0, "no stack falls back");
            report.ErrorFaces.Should().ContainSingle().Which.Family.Should().Be("TrendSlab");
            report.HasFinding.Should().BeTrue("a face needed by the page and never loaded is a finding of its own");
        }

        [Fact]
        public void BuildLogLines_OnTheBarlowCase_WritesTheFaceTheFindingAndTheSummaryAsProblems()
        {
            var report = new FontProbeReport { CardSetName = "Fallacies", Language = "ru", Side = "face" };
            report.Faces.Add(new FontFaceStatus { Family = "TrendSlab", Weight = "normal", Style = "normal", Status = "error" });
            report.Faces.Add(new FontFaceStatus { Family = "Barlow", Weight = "400", Style = "normal", Status = "loaded" });
            var probe = Probe(FontFallbackDetector.VerdictFallback);
            probe.Examples.Add(new FontProbeExample
            {
                Selector = "div.title", CardName = "Appel à la nature", Snippet = "Апелляция к природе"
            });
            report.Probes.Add(probe);
            report.Stats = new FontProbeStats
            {
                TextNodes = 3, RenderedTextNodes = 3, DeclaredFaces = 2, ErrorFaces = 1, Groups = 1
            };

            var lines = FontFallbackDetector.BuildLogLines(report, @"C:\out\QA\Fallacies_fonts_face_ru.md");

            lines.Select(l => l.Message).Should().ContainInOrder(
                "[FONT-LOAD-FAILURE] Fallacies ru face: @font-face 'TrendSlab' (weight normal, style normal) " +
                "is in status 'error' — declared, needed by the page, not loaded.",
                "[FONT-FALLBACK] Fallacies ru face: Cyrillic (400, normal) — no declared family of the stack renders " +
                "these letters (12/12) — the system fallback font paints them. Stack: \"Barlow\", sans-serif. " +
                "1 text node(s) affected. e.g. \"Appel à la nature\" (div.title): \"Апелляция к природе\"",
                "[Fonts] Fallacies ru face: 2 face(s) declared — 1 loaded, 1 in error, 0 unused; " +
                "1 stack(s) x script(s) probed — 1 with a fallback, 1 text node(s) affected. " +
                "Report: C:\\out\\QA\\Fallacies_fonts_face_ru.md");
            lines.Should().OnlyContain(l => l.IsProblem);
        }

        [Fact]
        public void BuildLogLines_WithoutFinding_IsOneInfoLineEndingWithADot()
        {
            var report = CleanReport();
            report.Probes.Add(Probe(FontFallbackDetector.VerdictClean, renderedBy: "Barlow"));
            report.Probes.Add(Probe(FontFallbackDetector.VerdictGeneric, script: "Latin"));

            var lines = FontFallbackDetector.BuildLogLines(report);

            lines.Should().ContainSingle("a clean probe logs one summary line, nothing else");
            lines[0].IsProblem.Should().BeFalse();
            lines[0].Message.Should().Be(
                "[Fonts] Fallacies ru face: 1 face(s) declared — 1 loaded, 0 in error, 0 unused; " +
                "2 stack(s) x script(s) probed — 0 with a fallback, 0 text node(s) affected.");
        }

        [Fact]
        public void BuildLogLines_OnNothingLaidOut_IsOneProblemLine_NeverACleanSummary()
        {
            var report = CleanReport();
            report.Stats = new FontProbeStats { TextNodes = 8, RenderedTextNodes = 0, HiddenTextNodes = 8, DeclaredFaces = 1 };
            report.Probes.Add(Probe(FontFallbackDetector.VerdictFallback, rendered: 0));
            report.IsEmpty.Should().BeTrue();

            var lines = FontFallbackDetector.BuildLogLines(report);

            lines.Should().ContainSingle();
            lines[0].IsProblem.Should().BeTrue("a probe that measured nothing must not read as a clean bill");
            lines[0].Message.Should().Be(
                "[Fonts] Fallacies ru face: no laid-out text found in the iframe — this probe says nothing about fonts.");
        }

        [Fact]
        public void FormatMarkdown_OnTheBarlowCase_ShowsTheFindingItsLettersAndItsExample()
        {
            var report = new FontProbeReport { CardSetName = "Fallacies", Language = "ru", Side = "face" };
            report.Faces.Add(new FontFaceStatus { Family = "Barlow", Weight = "400", Style = "normal", Status = "loaded" });
            report.Faces.Add(new FontFaceStatus { Family = "TrendSlab", Weight = "normal", Style = "normal", Status = "error" });
            var probe = Probe(FontFallbackDetector.VerdictPartial, renderedBy: "Barlow");
            probe.Missing = "Ё";
            probe.MissingLetterCount = 1;
            probe.Coverage[0].Covered = 11;
            probe.Examples.Add(new FontProbeExample { Selector = "div.title", CardName = "Homme de paille", Snippet = "Соломенное чучело" });
            report.Probes.Add(probe);
            report.Probes.Add(Probe(FontFallbackDetector.VerdictClean, script: "Latin", renderedBy: "Barlow"));
            report.Stats = new FontProbeStats { TextNodes = 3, RenderedTextNodes = 3, DeclaredFaces = 2, ErrorFaces = 1, Groups = 2 };

            var md = FontFallbackDetector.FormatMarkdown(report);

            md.Should().StartWith("# Font fallback report — Fallacies (ru) — face");
            md.Should().Contain("| Cyrillic | partial | `\"Barlow\", sans-serif` | 400 | 1 / 12 | Barlow | 1 |");
            md.Should().Contain("Letters no declared family renders (1): Ё");
            md.Should().Contain("| `Barlow` | 11 |");
            md.Should().Contain("| `TrendSlab` | normal | normal | error |");
            md.Should().Contain("- `div.title` in card \"Homme de paille\": \"Соломенное чучело\"");
            md.Should().NotContain("No font fallback detected");
            md.Should().Contain("## Probed stacks without finding");
            md.Should().Contain("| Latin | clean |");
        }

        [Fact]
        public void FormatMarkdown_OnACleanReport_SaysSoAndStillListsWhatWasProbed()
        {
            var report = CleanReport();
            report.Probes.Add(Probe(FontFallbackDetector.VerdictClean, renderedBy: "Barlow"));
            var generic = Probe(FontFallbackDetector.VerdictGeneric, script: "Latin");
            generic.Stack = "serif";
            report.Probes.Add(generic);

            var md = FontFallbackDetector.FormatMarkdown(report);

            md.Should().Contain("No font fallback detected. :sparkles:");
            md.Should().Contain("2 stack(s) x script(s) probed");
            md.Should().Contain("| Cyrillic | clean | `\"Barlow\", sans-serif` | 400 | normal | Barlow | 1 / 1 |");
            md.Should().Contain("| Latin | generic | `serif` | 400 | normal | (generic only) | 1 / 1 |");
            md.Should().NotContain("## Fallback findings");
            md.Should().NotContain("## Faces in error");
        }

        [Fact]
        public void FormatMarkdown_OnNothingLaidOut_SaysItMeasuredNothing()
        {
            var report = CleanReport();
            report.Stats = new FontProbeStats { TextNodes = 8, RenderedTextNodes = 0, HiddenTextNodes = 8 };

            var md = FontFallbackDetector.FormatMarkdown(report);

            md.Should().Contain("No laid-out text in the iframe: this report says nothing about fonts. :warning:");
            md.Should().NotContain(":sparkles:", "an instrument that saw nothing has no clean bill to give");
        }

        [Fact]
        public void FormatMarkdown_OnAStackOrACardNameWithPipes_DoesNotBreakTheTables()
        {
            var report = CleanReport();
            var probe = Probe(FontFallbackDetector.VerdictFallback);
            probe.Stack = "\"A|B\", serif";
            probe.Examples.Add(new FontProbeExample { Selector = "div.texte", CardName = "Either|Or", Snippet = "Ou bien | ou bien" });
            report.Probes.Add(probe);

            var md = FontFallbackDetector.FormatMarkdown(report);

            md.Should().Contain("| `\"A\\|B\", serif` |");
            md.Should().Contain("in card \"Either\\|Or\": \"Ou bien \\| ou bien\"");
            md.Should().NotContain("\"A|B\"");
        }

        [Fact]
        public void FormatMarkdown_OrdersTheFindingsByAffectedTextNodes_AndNamesMissingLettersOnlyWhenSome()
        {
            var report = CleanReport();
            report.Probes.Add(Probe(FontFallbackDetector.VerdictFallback, script: "Latin", rendered: 1));
            report.Probes.Add(Probe(FontFallbackDetector.VerdictFallback, script: "Cyrillic", rendered: 40));
            report.Probes.Add(Probe(FontFallbackDetector.VerdictSplit, script: "Greek", rendered: 3, renderedBy: "A | B"));
            report.AffectedTextNodes.Should().Be(44);

            var md = FontFallbackDetector.FormatMarkdown(report);

            var cyrillic = md.IndexOf("| Cyrillic | fallback |", StringComparison.Ordinal);
            var greek = md.IndexOf("| Greek | split |", StringComparison.Ordinal);
            var latin = md.IndexOf("| Latin | fallback |", StringComparison.Ordinal);
            cyrillic.Should().BeGreaterThan(0);
            greek.Should().BeGreaterThan(cyrillic, "the stack that touches the most text nodes comes first");
            latin.Should().BeGreaterThan(greek);
            md.Should().Contain("| Greek | split | `\"Barlow\", sans-serif` | 400 | 0 / 12 | A \\| B | 3 |");
        }

        [Fact]
        public void WriteMarkdownReport_CreatesTheDirectory_AndWritesUtf8WithoutBom()
        {
            var report = CleanReport();
            report.Probes.Add(Probe(FontFallbackDetector.VerdictFallback));
            var root = Path.Combine(Path.GetTempPath(), "ArgumentumTests", Guid.NewGuid().ToString());
            var dir = Path.Combine(root, "QA", "nested");
            var path = Path.Combine(dir, "Fallacies_fonts_face_ru.md");
            try
            {
                Directory.Exists(dir).Should().BeFalse("the organ creates the missing directory");

                FontFallbackDetector.WriteMarkdownReport(report, path);

                var bytes = File.ReadAllBytes(path);
                bytes.Should().StartWith(0x23, "the file starts with '#' — no BOM");
                File.ReadAllText(path, new UTF8Encoding(false)).Should().Contain("Логическая ошибка");
            }
            finally
            {
                if (Directory.Exists(root))
                {
                    try { Directory.Delete(root, true); } catch (IOException) { }
                }
            }
        }

        [Fact]
        public void BuildLogLines_AndFormatMarkdown_OnANullReport_Throw()
        {
            Action log = () => FontFallbackDetector.BuildLogLines(null);
            Action md = () => FontFallbackDetector.FormatMarkdown(null);

            log.Should().Throw<ArgumentNullException>();
            md.Should().Throw<ArgumentNullException>();
        }
    }
}
