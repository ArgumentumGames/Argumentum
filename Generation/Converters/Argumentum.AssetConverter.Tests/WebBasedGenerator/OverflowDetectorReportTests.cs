using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Pure unit tests for OverflowDetector.FormatMarkdown — exercises the report
    /// rendering layer without Playwright. The Playwright DetectAsync path is covered
    /// implicitly by integration runs of the harvest pipeline (issue #190 phase 1).
    /// </summary>
    public class OverflowDetectorReportTests
    {
        private static OverflowReport BuildReport(params CardOverflowResult[] cards)
        {
            return new OverflowReport
            {
                CardSetName = "Virtues",
                Language = "fr",
                GeneratedAtUtc = new DateTime(2026, 4, 9, 12, 0, 0, DateTimeKind.Utc),
                TolerancePx = 2,
                Cards = new List<CardOverflowResult>(cards)
            };
        }

        [Fact]
        public void FormatMarkdown_AllCardsClean_ReportsZeroOverflow()
        {
            var report = BuildReport(
                new CardOverflowResult { CardIndex = 0, CardName = "Argument valable", Findings = new() },
                new CardOverflowResult { CardIndex = 1, CardName = "Echange enrichissant", Findings = new() }
            );

            var md = OverflowDetector.FormatMarkdown(report);

            md.Should().Contain("Overflow detection report");
            md.Should().Contain("0 / 2 cards");
            md.Should().Contain("No overflow detected");
            md.Should().NotContain("## Cards with overflow");
        }

        [Fact]
        public void FormatMarkdown_OneCardOverflows_ReportsCardInSummaryAndDetail()
        {
            var report = BuildReport(
                new CardOverflowResult { CardIndex = 0, CardName = "Clean card", Findings = new() },
                new CardOverflowResult
                {
                    CardIndex = 1,
                    CardName = "Argumentum_Virtues_..Concision",
                    Findings = new List<OverflowFinding>
                    {
                        new()
                        {
                            Selector = ".texte",
                            ScrollHeight = 200, ClientHeight = 150,
                            ScrollWidth = 100, ClientWidth = 100,
                            ExcessHeight = 50, ExcessWidth = 0,
                            FontSizePx = 8.4, TextLength = 320,
                            TextSnippet = "Trop long pour la carte"
                        }
                    }
                }
            );

            var md = OverflowDetector.FormatMarkdown(report);

            md.Should().Contain("1 / 2 cards have at least one overflow");
            md.Should().Contain("## Cards with overflow");
            md.Should().Contain("Argumentum_Virtues_..Concision");
            md.Should().Contain("`.texte`");
            md.Should().Contain("50.0");
            md.Should().Contain("Trop long pour la carte");
        }

        [Fact]
        public void FormatMarkdown_MultipleCards_OrdersByWorstExcessDescending()
        {
            var report = BuildReport(
                new CardOverflowResult
                {
                    CardIndex = 0,
                    CardName = "Mild overflow",
                    Findings = new() { new OverflowFinding { Selector = ".texte", ExcessHeight = 5, FontSizePx = 8 } }
                },
                new CardOverflowResult
                {
                    CardIndex = 1,
                    CardName = "Severe overflow",
                    Findings = new() { new OverflowFinding { Selector = ".texte", ExcessHeight = 80, FontSizePx = 8 } }
                },
                new CardOverflowResult
                {
                    CardIndex = 2,
                    CardName = "Medium overflow",
                    Findings = new() { new OverflowFinding { Selector = ".exemple_fr", ExcessHeight = 30, FontSizePx = 7 } }
                }
            );

            var md = OverflowDetector.FormatMarkdown(report);

            var idxSevere = md.IndexOf("Severe overflow", StringComparison.Ordinal);
            var idxMedium = md.IndexOf("Medium overflow", StringComparison.Ordinal);
            var idxMild = md.IndexOf("Mild overflow", StringComparison.Ordinal);

            idxSevere.Should().BeGreaterThan(0);
            idxSevere.Should().BeLessThan(idxMedium);
            idxMedium.Should().BeLessThan(idxMild);
        }

        [Fact]
        public void FormatMarkdown_PipeInTextSnippet_IsEscaped()
        {
            var report = BuildReport(
                new CardOverflowResult
                {
                    CardIndex = 0,
                    CardName = "Pipe | inside",
                    Findings = new() { new OverflowFinding { Selector = ".texte", ExcessHeight = 10, TextSnippet = "a | b | c" } }
                });

            var md = OverflowDetector.FormatMarkdown(report);

            // Pipes inside table cells must be escaped to avoid breaking the markdown grid.
            md.Should().Contain("Pipe \\| inside");
            md.Should().Contain("a \\| b \\| c");
        }

        /// <summary>
        /// The overflowCss value (hidden = text clipped/lost, visible = painted outside its
        /// box) was captured in every finding since #1567 but never printed — a reader of the
        /// report could not tell a cut text from an out-of-box one. It must now appear in BOTH
        /// tables: per finding in the detail rows, and as the distinct set on the summary line.
        /// </summary>
        [Fact]
        public void FormatMarkdown_OverflowCss_AppearsInBothTables()
        {
            var report = BuildReport(
                new CardOverflowResult { CardIndex = 0, CardName = "Clean card", Findings = new() },
                new CardOverflowResult
                {
                    CardIndex = 1,
                    CardName = "Carte mixte",
                    Findings = new List<OverflowFinding>
                    {
                        new()
                        {
                            Selector = ".texte", Kind = "self", ExcessHeight = 40, FontSizePx = 8,
                            OverflowCss = "hidden", TextSnippet = "texte coupe par overflow:hidden"
                        },
                        new()
                        {
                            Selector = ".title", Kind = "container", ExcessHeight = 25, FontSizePx = 9,
                            OverflowCss = "visible", TextSnippet = "titre peint hors de sa boite"
                        }
                    }
                });

            var md = OverflowDetector.FormatMarkdown(report);

            // Detail table: each finding carries its own overflowCss right after the kind.
            md.Should().Contain("| `.texte` | self | hidden |");
            md.Should().Contain("| `.title` | container | visible |");

            // Summary table: the card's line shows the distinct set next to the selectors.
            var summaryLine = md.Split('\n').First(l => l.Contains("Carte mixte"));
            summaryLine.Should().Contain("hidden", "a clipped text reads differently from an out-of-box one")
                       .And.Contain("visible");
        }

        /// <summary>
        /// The .famille label overflows on 123 of the 131 Virtues fr cards, always the same
        /// way — a constant structural signal that dominated the headline tables (reco (b) of
        /// the 25/09 review). Those findings must move to a dedicated section: the main
        /// tables list only cards with findings BEYOND .famille, the headline splits the
        /// count, and the structural findings stay listed and counted — quarantined, not
        /// dropped.
        /// </summary>
        [Fact]
        public void FormatMarkdown_FamilleFindings_QuarantinedInTheirOwnSection()
        {
            var report = BuildReport(
                new CardOverflowResult { CardIndex = 0, CardName = "Carte propre", Findings = new() },
                new CardOverflowResult
                {
                    CardIndex = 1,
                    CardName = "Structurelle-seule",
                    Findings = new() { new OverflowFinding
                    {
                        Selector = ".famille", Kind = "self", ExcessHeight = 12, FontSizePx = 9,
                        OverflowCss = "hidden", TextSnippet = "Presentation integre"
                    } }
                },
                new CardOverflowResult
                {
                    CardIndex = 2,
                    CardName = "Mixte",
                    Findings = new List<OverflowFinding>
                    {
                        new() { Selector = ".title", Kind = "container", ExcessHeight = 30, FontSizePx = 9, OverflowCss = "visible", TextSnippet = "titre" },
                        new() { Selector = ".famille", Kind = "self", ExcessHeight = 12, FontSizePx = 9, OverflowCss = "hidden", TextSnippet = "Presentation integre" }
                    }
                });

            var md = OverflowDetector.FormatMarkdown(report);

            // Headline: full count, split between actionable and structural-only.
            md.Should().Contain("2 / 3 cards have at least one overflow");
            md.Should().Contain("1 card(s) with actionable findings");
            md.Should().Contain("1 whose only findings are the structural `.famille` label");

            var mainPart = md.Split("## Structural findings")[0];
            var structuralPart = md.Substring(md.IndexOf("## Structural findings", StringComparison.Ordinal));

            // Main tables: the structural-only card disappears, the mixed card stays with
            // its non-famille finding only.
            mainPart.Should().Contain("Mixte");
            mainPart.Should().NotContain("Structurelle-seule");
            var mixteLine = mainPart.Split('\n').First(l => l.Contains("Mixte"));
            mixteLine.Should().Contain(".title").And.NotContain("famille");

            // Quarantine section: every .famille finding stays listed, sorted by excess.
            structuralPart.Should().Contain("Structurelle-seule");
            structuralPart.Should().Contain("Mixte");
            structuralPart.Should().Contain("| 1 | Structurelle-seule | self | hidden |");

            // The aggregate the harvest logs is untouched: still per-card, not per-section.
            report.CardsWithOverflowCount.Should().Be(2);
        }

        /// <summary>
        /// Refinement of the .famille quarantine (ai-01 reservation on #1575, v18 grain 4):
        /// ONLY the small vertical self excess (the line-height:0.9 residue) is structural.
        /// A .famille finding that crosses the card edge (kind "card") is a real defect and
        /// must stay in the MAIN tables — headline count, summary row and detail row — not
        /// be quarantined away with the constant label signal.
        /// </summary>
        [Fact]
        public void FormatMarkdown_FamilleCardKind_StaysInMainTables()
        {
            var report = BuildReport(
                new CardOverflowResult
                {
                    CardIndex = 0,
                    CardName = "Bord-coupe",
                    Findings = new() { new OverflowFinding
                    {
                        Selector = ".famille", Kind = "card", ExcessHeight = 8, ExcessWidth = 0,
                        FontSizePx = 9, OverflowCss = "hidden", TextSnippet = "Echange enrichissant"
                    } }
                });

            var md = OverflowDetector.FormatMarkdown(report);

            md.Should().Contain("1 / 1 cards have at least one overflow");

            var mainPart = md.Split("## Structural findings")[0];

            mainPart.Should().Contain("Bord-coupe");
            var summaryLine = mainPart.Split('\n').First(l => l.Contains("Bord-coupe"));
            summaryLine.Should().Contain(".famille (card)");
            mainPart.Should().Contain("| `.famille` | card | hidden | 8.0 |",
                "le constat .famille kind=card apparaît comme ligne de détail normale.");

            md.Should().NotContain("## Structural findings",
                "aucun constat structurel ici : le .famille kind=card n'est pas mis en quarantaine.");
        }

        /// <summary>
        /// Second face of the same refinement: a .famille finding that is a HORIZONTAL self
        /// excess (ExcessWidth beyond tolerance — the label sliding out of its box sideways)
        /// is not the vertical line-height residue either, and stays in the main tables.
        /// </summary>
        [Fact]
        public void FormatMarkdown_FamilleSelfHorizontal_StaysInMainTables()
        {
            var report = BuildReport(
                new CardOverflowResult
                {
                    CardIndex = 0,
                    CardName = "Glisse-laterale",
                    Findings = new() { new OverflowFinding
                    {
                        Selector = ".famille", Kind = "self", ExcessHeight = 0, ExcessWidth = 6,
                        FontSizePx = 9, OverflowCss = "hidden", TextSnippet = "Justesse lexicale"
                    } }
                });

            var md = OverflowDetector.FormatMarkdown(report);

            md.Should().Contain("1 / 1 cards have at least one overflow");
            md.Split("## Structural findings")[0].Should().Contain("Glisse-laterale",
                "6 px de débordement horizontal dépassent la tolérance (2) : pas structurel.");
            md.Should().Contain("| `.famille` | self |");
            md.Should().NotContain("## Structural findings");
        }

        [Fact]
        public void CardsWithOverflowCount_ComputedFromFindings()
        {
            var report = BuildReport(
                new CardOverflowResult { CardIndex = 0, Findings = new() },
                new CardOverflowResult { CardIndex = 1, Findings = new() { new OverflowFinding { Selector = ".texte" } } },
                new CardOverflowResult { CardIndex = 2, Findings = new() { new OverflowFinding { Selector = ".exemple_fr" }, new OverflowFinding { Selector = ".texte" } } }
            );

            report.CardsWithOverflowCount.Should().Be(2);
        }
    }
}
