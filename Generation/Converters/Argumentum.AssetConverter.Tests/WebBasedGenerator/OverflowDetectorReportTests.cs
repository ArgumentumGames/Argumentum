using System;
using System.Collections.Generic;
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
