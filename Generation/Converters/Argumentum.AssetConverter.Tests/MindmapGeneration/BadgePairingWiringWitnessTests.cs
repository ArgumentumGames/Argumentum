using System;
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
    /// P23-18 / #2275 — the two WIRING witnesses asked for by the ai-01 dispatch (c.6890339774):
    /// « un test qui passe par <c>UpdateSvgWithItems(…, language)</c>, rouge sous la mutation
    /// <c>null</c> ; un témoin de carte périmée et badgée, rouge sous la mutation “titre nu” de
    /// la suite A. »
    ///
    /// <see cref="BadgePairingFullCorpusTests"/> already freezes the PAIRING primitive on the full
    /// corpus (1 408 nodes × 8 languages) — but it calls
    /// <see cref="MindMapSvgItemPairing.CollectPossibleSvgNodes"/> directly. Nothing in it goes red
    /// if a PRODUCTION call site forgets to forward <c>language</c> down to
    /// <c>UpdateSvgWithItems</c> (FallacyMindMapDocumentConfig.cs:1447 and :1512 are the two sites;
    /// the forwarders at :1637/:1642 compose <see cref="MindMapSvgItemPairing.WrittenNodeTextFunc"/>,
    /// which returns <c>null</c> for a null language — the pre-#2275 title-only oracle, silently).
    /// These witnesses close that gap:
    ///
    /// <list type="bullet">
    /// <item><description><see cref="ProcessSvgFilesAsync_ForwardsLanguage_HomonymPairStaysWired"/>
    /// enters through the internal pipeline overload (the :1447 call site's own entry) and asserts
    /// the observable the wrappers consume: <c>class="node"</c> count. Under the null-language
    /// mutation the #2275 signature reproduces at the smallest scale — both homonyms collapse onto
    /// the bare node, suite B (rightly) mutes the whole group, ZERO nodes get wired.</description>
    /// </item>
    /// <item><description><see cref="SuiteA_WrittenTextReservation_KeepsStaleBadgedNodeForItsItem"/>
    /// is the stale-badged-map witness at the primitive level: the map carries the BADGED text of
    /// one item whose old node is gone (staleness), while a second item's title is a PREFIX of the
    /// first's written text. The suite A reservation (<c>exactOwner</c>, judged on the WRITTEN
    /// text) is what keeps the surviving node for its rightful owner. Reverted to the bare title
    /// (the “titre nu” mutation — nodeTextFunc→titleFunc in the reservation AND in
    /// <c>groupTexts</c>), the stale item STEALS the node and the rightful owner goes mute: a false
    /// card instead of a silent one — exactly what #1700 suite A exists to prevent.</description>
    /// </item>
    /// </list>
    ///
    /// Mutation protocol (verified RED then restored before commit — backups by file copy, never
    /// <c>git checkout --</c>): the null mutation replaces <c>language</c> by <c>null</c> at
    /// FallacyMindMapDocumentConfig.cs:1447 (and :1512); the “titre nu” mutation replaces
    /// <c>nodeTextFunc(...)</c> by <c>titleFunc(...)</c> at MindMapSvgItemPairing.cs:131 and
    /// :170-171.
    /// </summary>
    public class BadgePairingWiringWitnessTests
    {
        private static readonly XNamespace Svg = "http://www.w3.org/2000/svg";

        // --- Shared helpers (compact twins of BadgePairingFullCorpusTests' — same corpus, same
        // localization recipe; kept local so each witness file reads standalone). ---

        private static List<Fallacy> LoadTaxonomy()
        {
            var csvPath = Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies",
                "Argumentum Fallacies - Taxonomy.csv");
            File.Exists(csvPath).Should().BeTrue(
                "the committed taxonomy CSV is the corpus the homonym pair is drawn from");

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
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
        /// Witness 1 — the pipeline overload forwards <c>language</c> to the pairing (call site
        /// FallacyMindMapDocumentConfig.cs:1447). Corpus premise: PK 787 and PK 21 share the fr
        /// title « Syllogisme du politicien » (the taxonomy's own homonym pair for this signature);
        /// PK 787 has a native <c>link_fr</c> (badge empty) while PK 21 falls back to
        /// <c>link_en</c> (badge « (en) ») — the exact « X » / « X (en) » collision measured 14/11
        /// at 31 lost nodes on the fr map.
        /// </summary>
        [Fact]
        public async Task ProcessSvgFilesAsync_ForwardsLanguage_HomonymPairStaysWired()
        {
            const string barePk = "787";   // link_fr present  → written = title (no badge)
            const string badgedPk = "21";  // link_fr empty, link_en present → written = title + " (en)"
            // Select by Path (the tree's unique key); the PK premise is asserted through Id,
            // which the ClassMap binds to the literal "PK" column.
            var corpus = LoadTaxonomy();
            var bare = corpus.Single(f => f.Path == "4.3.3.1.1.1");
            var badged = corpus.Single(f => f.Path == "1.1.1.2.1");
            bare.Id.Should().Be(barePk);
            badged.Id.Should().Be(badgedPk);

            var config = LocalizedConfig("fr");
            var title = config.TitleFunc(bare);
            config.TitleFunc(badged).Should().Be(title,
                $"premise: PK {barePk} and PK {badgedPk} are fr-title homonyms (« {title} »)");

            var writtenFunc = MindMapSvgItemPairing.WrittenNodeTextFunc(config.TitleFunc, "fr");
            writtenFunc.Should().NotBeNull("language \"fr\" is known — the oracle must compose title + badge");
            var written = writtenFunc!;
            written(bare).Should().Be(title,
                $"premise: PK {barePk} carries a native link_fr — its written text is the bare title");
            written(badged).Should().Be(title + " (en)",
                $"premise: PK {badgedPk} falls back to link_en on the fr map — its written text is badged");

            // One node per item, text = EXACTLY what CreateNode writes into the .mm: the map the
            // next FreeMind pass derives from the badged .mm.
            var items = new List<IMindMapItem> { bare, badged };
            var fixtureSvg = new XDocument(new XElement(Svg + "svg",
                items.Select(item => new XElement(Svg + "g",
                    new XElement(Svg + "text", written(item))))));

            config.SVGMaps = new List<SVGFreemindMap>
            {
                new SVGFreemindMap { DocumentName = "content.svg", SetSVGNodeAttributes = true }
            };

            var tempDir = Path.Combine(Path.GetTempPath(), "p2318-witness-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            try
            {
                var sourcePath = Path.Combine(tempDir, "fixture.svg");
                fixtureSvg.Save(sourcePath);

                // The production entry: ProcessSvgFilesAsync(paths, items, language) is the
                // overload the Mindmapper stage calls; it forwards `language` at :1447.
                var processed = await config.ProcessSvgFilesAsync(new[] { sourcePath }, items, "fr");

                processed.Should().HaveCount(1,
                    "one SVGMap on the config → one processed document");
                var nodeCount = processed.Values.Single()
                    .Descendants(Svg + "g")
                    .Count(g => g.Attribute("class")?.Value == "node");
                nodeCount.Should().Be(items.Count,
                    "both homonyms must keep their node through the pipeline overload when language " +
                    "is forwarded — under the null-language mutation the title-only oracle collapses " +
                    "both items onto the bare node, suite B mutes the group and ZERO nodes get wired " +
                    "(the #2275 signature, measured 31 nodes lost on the fr corpus map)");
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        /// <summary>
        /// Witness 2 — stale-badged map, primitive level. The map carries ONE node, badged
        /// (« menteur compulsif (en) », item F's written text); item V's own node is GONE (the map
        /// is one pass stale) and V's TITLE is a prefix of F's written text. On the written-text
        /// oracle, suite A reserves the node for F (its text IS the written text) and V stays
        /// silent — a mute node, not a false card. Under the « titre nu » mutation
        /// (MindMapSvgItemPairing.cs:131 and :170-171 judging on titleFunc), no reservation
        /// exists: V's group is processed first (insertion order) and STEALS the node — F goes
        /// mute and the surviving card describes V: false card.
        /// </summary>
        [Fact]
        public void SuiteA_WrittenTextReservation_KeepsStaleBadgedNodeForItsItem()
        {
            var staleBadged = new Fallacy { Path = "1", TextFr = "menteur", LinkFr = null, LinkEn = "https://example.org/v" };
            var rightfulOwner = new Fallacy { Path = "1.1", TextFr = "menteur compulsif", LinkFr = null, LinkEn = "https://example.org/f" };
            // Both fall back to link_en on the fr map → both written texts are badged:
            // V writes « menteur (en) », F writes « menteur compulsif (en) ».
            var items = new List<IMindMapItem> { staleBadged, rightfulOwner };

            Func<IMindMapItem, string> titleFunc = item => ((Fallacy)item).TextFr;
            var nodeTextFunc = MindMapSvgItemPairing.WrittenNodeTextFunc(titleFunc, "fr");
            nodeTextFunc.Should().NotBeNull("language \"fr\" is known — the oracle must compose title + badge");

            nodeTextFunc!(staleBadged).Should().Be("menteur (en)");
            nodeTextFunc(rightfulOwner).Should().Be("menteur compulsif (en)");

            // The STALE map: only F's badged node survives; V's node is missing.
            var survivingNodeText = nodeTextFunc(rightfulOwner);
            var doc = new XDocument(new XElement(Svg + "svg",
                new XElement(Svg + "g", new XElement(Svg + "text", survivingNodeText))));

            var collected = MindMapSvgItemPairing.CollectPossibleSvgNodes(
                items, doc, Svg, titleFunc, nodeTextFunc);
            var paired = MindMapSvgItemPairing.DisambiguateSvgNodes(
                collected, items, Svg, titleFunc, nodeTextFunc);

            var survivingNode = doc.Root!.Elements(Svg + "g").Single();
            paired.Should().ContainKey(rightfulOwner)
                .WhoseValue.Should().Be(survivingNode,
                    "the node whose text IS the item's written text belongs to that item — suite A " +
                    "reservation judged on the WRITTEN text (badge included)");
            paired.Should().NotContainKey(staleBadged,
                "the stale item's own node is gone and the survivor is reserved for its rightful " +
                "owner: a mute node, not a false card. Under the « titre nu » mutation the " +
                "reservation disappears, V's group runs first and steals the node — F goes mute " +
                "and the card carries V's data (false card)");
        }
    }
}
