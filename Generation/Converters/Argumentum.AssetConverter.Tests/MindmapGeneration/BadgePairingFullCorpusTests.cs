using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Argumentum.AssetConverter;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;

using CsvHelper;
using CsvHelper.Configuration;

using FluentAssertions;

using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// #2275 freeze — the badge must cost NO node. The corpus is the real taxonomy CSV
    /// (1 408 rows), the map under test is the freshly derived one: exactly one
    /// <c>&lt;g&gt;</c> per row whose text is the text <c>CreateNode</c> actually writes
    /// (title + « (en) » / « (fr) » fallback badge, <see cref="MindMapLinkBadge"/>). On that
    /// map, EVERY item must be wired to a node, in ALL 8 languages — anything less is a node
    /// the badge silently retired.
    ///
    /// Measured on master before the fix (14/11, #2275): items left without a node per
    /// language — en 0, fr 31, ru 43, pt 42, es 42, ar 56, fa 59, zh 56. The mechanism: two
    /// homonyms badged differently (« X » / « X (en) ») left a single surviving candidate
    /// group after the min-length filter, and <c>#1700</c> suite B (rightly) refuses to wire a
    /// short-handed group — the whole group went mute.
    ///
    /// <see cref="TitleOnlyOracle_DropsNodesOnTheSameCorpus"/> is the differential control:
    /// the SAME fixture paired through the pre-#2275 oracle (title only) still drops nodes,
    /// proving this freeze is not vacuous — reverting the pairing at the oracle level turns
    /// the main theory RED.
    /// </summary>
    public class BadgePairingFullCorpusTests
    {
        private static readonly XNamespace Svg = "http://www.w3.org/2000/svg";

        /// <summary>
        /// DELIBERATE FREEZE (ai-01 dispatch, #2275): 1 408 nodes in the 8 languages. Adding a
        /// row to the taxonomy moves this number — update the freeze consciously, never delete it.
        /// </summary>
        private const int ExpectedNodeCount = 1408;

        private static readonly string[] AllLanguages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

        private static List<Fallacy> LoadTaxonomy()
        {
            var csvPath = Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies",
                "Argumentum Fallacies - Taxonomy.csv");
            File.Exists(csvPath).Should().BeTrue(
                "the committed taxonomy CSV is the corpus this freeze is derived from");

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Same tolerance as the production loader (MmGeneratorTests.GetTestDataAsync):
                // optional columns absent from the CSV must not kill the parse.
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, csvConfig);
            csv.Context.RegisterClassMap<FallacyClassMap>();
            var records = new List<Fallacy>();
            while (csv.Read())
            {
                var record = csv.GetRecord<Fallacy>();
                if (record != null)
                {
                    records.Add(record);
                }
            }
            return records;
        }

        /// <summary>
        /// The production-localized config: every <c>MindMapLocalization</c> entry applied for
        /// <paramref name="language"/>, exactly as the pipeline does before generating a map.
        /// </summary>
        private static FallacyMindMapDocumentConfig LocalizedConfig(string language)
        {
            var config = new FallacyMindMapDocumentConfig();
            foreach (var localization in new AssetConverterConfig().LocalizationConfig.MindMapLocalization)
            {
                localization.DoReflectionTranslate(config, language);
            }
            return config;
        }

        /// <summary>
        /// The freshly derived map: one <c>&lt;g&gt;</c> per corpus row, text = EXACTLY what
        /// <c>CreateNode</c> writes into the .mm (title + badge) — the map the next FreeMind
        /// pass will produce from the badged .mm files.
        /// </summary>
        private static XDocument FreshlyDerivedSvg(
            Func<IMindMapItem, string> titleFunc, List<IMindMapItem> items, string language)
        {
            var written = MindMapSvgItemPairing.WrittenNodeTextFunc(titleFunc, language);
            // No id attribute needed: the pairing keys on item identity and node text only.
            return new XDocument(new XElement(Svg + "svg",
                items.Select(item => new XElement(Svg + "g",
                    new XElement(Svg + "text", written(item))))));
        }

        private static Dictionary<IMindMapItem, XElement> Pair(
            FallacyMindMapDocumentConfig config, List<IMindMapItem> items, XDocument doc, string language)
        {
            var nodeTextFunc = MindMapSvgItemPairing.WrittenNodeTextFunc(config.TitleFunc, language);
            var collected = MindMapSvgItemPairing.CollectPossibleSvgNodes(
                items, doc, Svg, config.TitleFunc, nodeTextFunc);
            return MindMapSvgItemPairing.DisambiguateSvgNodes(
                collected, items, Svg, config.TitleFunc, nodeTextFunc);
        }

        [Theory]
        [InlineData("fr")]
        [InlineData("en")]
        [InlineData("ru")]
        [InlineData("pt")]
        [InlineData("es")]
        [InlineData("ar")]
        [InlineData("fa")]
        [InlineData("zh")]
        public void FreshlyDerivedMap_WiresEveryCorpusNode_InEveryLanguage(string language)
        {
            var corpus = LoadTaxonomy();
            corpus.Should().HaveCount(ExpectedNodeCount,
                "the #2275 freeze pins the corpus size — a taxonomy change must update it consciously");

            var items = corpus.Cast<IMindMapItem>().ToList();
            var config = LocalizedConfig(language);
            var doc = FreshlyDerivedSvg(config.TitleFunc, items, language);

            var paired = Pair(config, items, doc, language);

            paired.Should().HaveCount(ExpectedNodeCount,
                $"every corpus row must keep its node on the freshly derived {language} map — " +
                "a shortfall is a node the fallback badge silently retired (#2275)");
            paired.Values.Should().OnlyHaveUniqueItems(
                "one node per item — a badge must never collapse two rows onto one node either");
        }

        /// <summary>
        /// Differential control (#2275): the SAME freshly derived maps paired through the
        /// pre-fix oracle (title only, <c>nodeTextFunc = null</c>) MUST drop nodes. This is the
        /// assertion that goes red if the pairing is reverted to matching on the bare title —
        /// it documents that the main theory is not vacuously green. Measured 14/11: 7 of the
        /// 8 languages lost 31–59 items each under the old oracle (en loses none — its link
        /// cascade is native, which is why the freeze must cover all 8 languages).
        /// </summary>
        [Fact]
        public void TitleOnlyOracle_DropsNodesOnTheSameCorpus()
        {
            var corpus = LoadTaxonomy();
            corpus.Should().HaveCount(ExpectedNodeCount);

            var droppedLanguages = new List<string>();
            foreach (var language in AllLanguages)
            {
                var items = corpus.Cast<IMindMapItem>().ToList();
                var config = LocalizedConfig(language);
                var doc = FreshlyDerivedSvg(config.TitleFunc, items, language);

                // nodeTextFunc deliberately NULL = the pre-#2275 oracle (title only).
                var collected = MindMapSvgItemPairing.CollectPossibleSvgNodes(
                    items, doc, Svg, config.TitleFunc, null);
                var paired = MindMapSvgItemPairing.DisambiguateSvgNodes(
                    collected, items, Svg, config.TitleFunc, null);

                if (paired.Count < ExpectedNodeCount)
                {
                    droppedLanguages.Add($"{language}: {ExpectedNodeCount - paired.Count} dropped");
                }
            }

            droppedLanguages.Should().NotBeEmpty(
                "the title-only oracle must still drop nodes on this corpus — if it stopped " +
                "dropping any, the #2275 fix would be redundant and the main freeze vacuous " +
                $"(measured 14/11: 7 of 8 languages affected). Actual: {(droppedLanguages.Count > 0 ? string.Join(", ", droppedLanguages) : "none")}.");
        }
    }
}
