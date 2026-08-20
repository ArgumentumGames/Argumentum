using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter.Tests.PdfAssembly;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    /// <summary>
    /// Issue #1121 guard — inverse control: proves the guard logic FAILS on the actual defect
    /// (PokerCards_en = 2 pages instead of 334, PokerCards_ru absent). A green test on
    /// defective data is worthless (#1046 lesson); this test asserts the guard mords.
    ///
    /// The guard lives in VisualTests/PdfBundleIntegrityTests (needs a real Target/).
    /// This companion tests the pure logic in <see cref="PdfBundleIntegrity"/> with a fixture
    /// that reproduces the review-v2.0.0-regen-20260820 bundle signature:
    /// - 8 langs × 10 PDFs, EXCEPT ru = 9 (PokerCards_ru.pdf missing).
    /// - PokerCards: fr/pt/zh = 334, en = 2 (the broken assembly).
    /// Both checks MUST return non-empty failures.
    /// </summary>
    public class PdfBundleIntegrityLogicTests
    {
        /// <summary>#1121 exact signature: ru has 9 PDFs (missing PokerCards_ru).</summary>
        [Fact]
        public void CheckProfile_Fails_On_1111_Signature_Ru_Missing_PokerCards()
        {
            var counts = new Dictionary<string, int>
            {
                ["fr"] = 10, ["en"] = 10, ["ru"] = 9, ["pt"] = 10,
                ["es"] = 10, ["ar"] = 10, ["fa"] = 10, ["zh"] = 10,
            };

            var failures = PdfBundleIntegrity.CheckProfile(counts);

            failures.Should().NotBeEmpty("the #1121 bundle has ru=9 (missing PokerCards_ru) — the profile check must catch it");
            failures.Should().Contain(f => f.Contains("ru: 9 PDFs"),
                "the failure must name ru and its actual count vs expected 10");
        }

        /// <summary>#1121 exact signature: PokerCards_en = 2 pages vs median 334.</summary>
        [Fact]
        public void CheckParity_Fails_On_1111_Signature_En_Two_Pages()
        {
            var records = new List<(string, string, int, string)>
            {
                ("PokerCards", "fr", 334, "Argumentum_PokerCards_fr.pdf"),
                ("PokerCards", "en", 2,   "Argumentum_PokerCards_en.pdf"),  // the defect
                ("PokerCards", "pt", 334, "Argumentum_PokerCards_pt.pdf"),
                ("PokerCards", "zh", 334, "Argumentum_PokerCards_zh.pdf"),
                ("PokerCards", "es", 334, "Argumentum_PokerCards_es.pdf"),
                ("PokerCards", "ar", 334, "Argumentum_PokerCards_ar.pdf"),
                ("PokerCards", "fa", 334, "Argumentum_PokerCards_fa.pdf"),
                // ru absent — no record, so PokerCards is present in 7 langs (median 334)
            };

            var failures = PdfBundleIntegrity.CheckParity(records);

            failures.Should().NotBeEmpty("PokerCards_en at 2 pages (median 334) is -99% — the parity check must catch it");
            failures.Should().Contain(f => f.Contains("PokerCards_en") && f.Contains("2 pages"),
                "the failure must name en and its 2-page count");
        }

        /// <summary>Negative control: a healthy bundle (all 334, all 10/lang) passes both checks.</summary>
        [Fact]
        public void CheckProfile_And_Parity_Pass_On_Healthy_Bundle()
        {
            var counts = PdfBundleIntegrity.Languages.ToDictionary(l => l, _ => PdfBundleIntegrity.ExpectedPdfsPerLanguage);
            PdfBundleIntegrity.CheckProfile(counts).Should().BeEmpty("a healthy 8×10 bundle passes the profile check");

            var records = new List<(string, string, int, string)>();
            foreach (var lang in PdfBundleIntegrity.Languages)
                records.Add(("PokerCards", lang, 334, $"Argumentum_PokerCards_{lang}.pdf"));

            PdfBundleIntegrity.CheckParity(records).Should().BeEmpty("all-equal page counts pass the parity check");
        }

        /// <summary>DocKey extraction strips Argumentum_ prefix and _{lang} suffix.</summary>
        [Theory]
        [InlineData("Argumentum_PokerCards_en.pdf", "en", "PokerCards")]
        [InlineData("Argumentum_Fallacies_Web_A0_fr.pdf", "fr", "Fallacies_Web_A0")]
        [InlineData("Argumentum_TarotCards_Virtues_ru.pdf", "ru", "TarotCards_Virtues")]
        public void DocKeyFromName_Strips_Convention(string fileName, string lang, string expected)
        {
            PdfBundleIntegrity.DocKeyFromName(fileName, lang).Should().Be(expected);
        }
    }
}
