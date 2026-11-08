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
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// #2190 (voie B) organ — every cross-link of a Fallacies mindmap carries a UNIQUE tint from
    /// the .mm to the Batik SVG export, and the reader maps each tint back to its
    /// (verb, source, target), restores the verb pastel and annotates the group. The draw order
    /// is NOT recoverable (measured 07/11/2026, 8/8 languages: 369/581 divergences, no traversal
    /// rule reproduces the permutation — see CrossLinkTintTable's class comment), so the tint IS
    /// the identity: these tests pin the properties that make that identity sound.
    ///
    /// 1. GEOMETRY: tints are unique per verb, unique across verbs, within drift 25 of their
    ///    pastel, and disjoint from both color registers (a restored value must never be
    ///    readable as a tint, and a tint must never read as another verb).
    /// 2. DETERMINISM: the table is a pure function of the corpus walk — the reader rebuilds it
    ///    in the same run and must land on the same assignment.
    /// 3. WRITER: the generated .mm carries exactly the table's tints, in the resolver's order.
    /// 4. READER: a Batik-shaped SVG is restored to the pastel palette and annotated;
    ///    a legacy pastel-only export is left untouched.
    /// 5. INVERSE CONTROL (tint swap): swapping two tints before the reader runs must be
    ///    DETECTED by the geometric check — a restored identity is verified against the arrow's
    ///    endpoints, never taken on faith. Without this control, a reader that annotated every
    ///    group with a wrong-but-plausible identity would still pass 1-4.
    /// </summary>
    public class CrossLinkTintTableTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly string _tempTestDirectory;

        public CrossLinkTintTableTests(ITestOutputHelper output)
        {
            _output = output;
            _tempTestDirectory = Path.Combine(Path.GetTempPath(), "CrossLinkTintTableTests", Path.GetRandomFileName());
            Directory.CreateDirectory(_tempTestDirectory);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempTestDirectory))
            {
                Directory.Delete(_tempTestDirectory, true);
            }
        }

        private static CrossLink[] AllVerbs => Enum.GetValues(typeof(CrossLink)).Cast<CrossLink>()
            .Where(v => v != CrossLink.None && v != CrossLink.All).ToArray();

        private static string PastelOf(CrossLink verb) => FallacyMindMapDocumentConfig.GetCrossLinkColor(verb);

        private static (int r, int g, int b) ChannelsOf(string hex) =>
            (Convert.ToInt32(hex.Substring(1, 2), 16),
             Convert.ToInt32(hex.Substring(3, 2), 16),
             Convert.ToInt32(hex.Substring(5, 2), 16));

        // ─────────────────────────────────────────────────────────────────────────────
        // 1. Tint geometry
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void TintFor_PerVerbGrid_IsUniqueNearPastelAndOffBothRegisters()
        {
            foreach (var verb in AllVerbs)
            {
                var pastel = PastelOf(verb);
                var study = FallacyMindMapDocumentConfig.GetStudyCrossLinkColor(verb);
                var (pr, pg, pb) = ChannelsOf(pastel);

                var seen = new HashSet<string>();
                for (var ordinal = 0; ordinal < CrossLinkTintTable.Side * CrossLinkTintTable.Side - 1; ordinal++)
                {
                    var tint = CrossLinkTintTable.TintFor(verb, ordinal, out _);
                    seen.Add(tint).Should().BeTrue(
                        "tint ordinal {0} of verb {1} must be distinct from every lower ordinal of the same verb " +
                        "(the grid is the uniqueness mechanism — a repeat means Drift/Side drifted apart)",
                        ordinal, verb);

                    var (r, g, b) = ChannelsOf(tint);
                    Math.Abs(r - pr).Should().BeLessOrEqualTo(CrossLinkTintTable.Drift);
                    Math.Abs(g - pg).Should().BeLessOrEqualTo(CrossLinkTintTable.Drift);
                    Math.Abs(b - pb).Should().BeLessOrEqualTo(CrossLinkTintTable.Drift);

                    tint.Should().NotBe(pastel,
                        "the pastel itself (grid index 0) is never a tint — it is the value the reader RESTORES to");
                    tint.Should().NotBe(study,
                        "a study-register color must never serve as a tint (the study variant rewrites pastels)");
                }

                seen.Count.Should().Be(CrossLinkTintTable.Side * CrossLinkTintTable.Side - 1);
            }
        }

        [Fact]
        public void TintFor_AcrossVerbs_AllGridsAreDisjoint()
        {
            // The measured invariant the per-family geometry buys: G stays at the verb's base
            // value, and the two G-colliding pairs (PredatesOn/Allows, Leverages/Mirrors) have
            // disjoint R/B boxes at drift 25. Asserted over the full capacity, not just the
            // corpus — a Drift change that breaks disjointness must fail HERE, before any
            // corpus symptom.
            var global = new HashSet<string>();
            foreach (var verb in AllVerbs)
            {
                for (var ordinal = 0; ordinal < CrossLinkTintTable.Side * CrossLinkTintTable.Side - 1; ordinal++)
                {
                    var tint = CrossLinkTintTable.TintFor(verb, ordinal, out _);
                    global.Add(tint).Should().BeTrue(
                        "tint '{0}' of verb {1} ordinal {2} collides with another verb's grid — cross-family " +
                        "uniqueness is broken (see the disjointness proof in CrossLinkTintTable's class comment)",
                        tint, verb, ordinal);
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // 2. Table build — synthetic corpus
        // ─────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Minimal corpus: root + 3 families + 4 linked nodes, 7 resolvable links
        /// (Leverages 3, Mirrors 1, Denounces 3). Order-sensitive on purpose: the table's walk
        /// is the resolver's walk.
        /// </summary>
        private static List<Fallacy> SyntheticCorpus()
        {
            Fallacy Node(string path, string text) => new Fallacy
            {
                Path = path,
                // The taxonomy's depth column is 1-based: family = 1, sub-family = 2 (SetNodeStyle
                // indexes EdgeSizes[Depth - 1] for family nodes, so Depth 0 would underflow).
                Depth = path.Count(c => c == '.') + 1,
                TextFr = text,
                DescFr = "<p>desc</p>",
                ExampleFr = "<p>example</p>",
                PK = "pk-" + path,
                Id = "ID_" + path,
            };

            var items = new List<Fallacy>
            {
                Node("0", "Racine"),
                Node("1", "Famille une"),
                Node("1.1", "Nœud un-un"),
                Node("1.2", "Nœud un-deux"),
                Node("2", "Famille deux"),
                Node("2.1", "Nœud deux-un"),
                Node("3", "Famille trois"),
                Node("3.1", "Nœud trois-un"),
            };

            items.First(i => i.Path == "1.1").CrossLinkLeverages = "2.1; 3.1";
            items.First(i => i.Path == "1.2").CrossLinkLeverages = "2.1";
            items.First(i => i.Path == "2.1").CrossLinkMirrors = "1.1";
            items.First(i => i.Path == "3.1").CrossLinkDenounces = "1.1; 1.2; 9.9"; // 9.9 = unresolved, skipped

            return items;
        }

        [Fact]
        public void Build_SyntheticCorpus_MatchesResolverWalkAndIsDeterministic()
        {
            var items = SyntheticCorpus().Cast<IMindMapItem>().ToList();

            var first = CrossLinkTintTable.Build(items, CrossLink.All);
            var second = CrossLinkTintTable.Build(items, CrossLink.All);

            first.Count.Should().Be(6,
                "6 resolvable links in the synthetic corpus (Leverages 2+1, Mirrors 1, Denounces 2 — the '9.9' target is unresolved and must be skipped)");

            // Determinism: same corpus walk ⇒ byte-identical tints, or the reader's rebuild in
            // the same run would map strokes to the WRONG links.
            first.Entries.Select(e => e.TintHex)
                .Should().Equal(second.Entries.Select(e => e.TintHex),
                    "the tint table must be a pure function of the corpus walk");

            // The walk itself: reproduce it with the resolver's public enumeration and compare
            // (verb, source path, target path) triple by triple.
            var itemsByPath = CrossLinkResolver.ItemsByPath(items);
            var expected = items.Where(i => !string.IsNullOrEmpty(i.PK))
                .OfType<Fallacy>()
                .SelectMany(fallacy => CrossLinkResolver.ResolveOrdered(fallacy, itemsByPath, CrossLink.All)
                    .SelectMany(group => group.Targets.Select(target => (group.Verb, fallacy.Path, target.Path))))
                .ToList();

            var actual = first.Entries.Select(e => (e.Verb, e.SourcePath, e.TargetPath)).ToList();
            actual.Should().Equal(expected,
                "the table's entry order IS the resolver's enumeration order — the writer hands tints out in " +
                "this order and any drift would mislabel arrows");

            // Uniqueness on the actual corpus (the geometry tests cover the full capacity; this
            // pins that Build's own walk did not produce collisions).
            first.Entries.Select(e => e.TintHex).Distinct().Should().HaveCount(6);

            // Per-verb ordinals: Leverages links get ordinals 0,1,2 in walk order, etc.
            first.Entries.Where(e => e.Verb == CrossLink.Leverages).Select(e => e.VerbOrdinal)
                .Should().ContainInOrder(0, 1, 2);
            first.Entries.Where(e => e.Verb == CrossLink.Mirrors).Select(e => e.VerbOrdinal)
                .Should().Equal(0);
            first.Entries.Where(e => e.Verb == CrossLink.Denounces).Select(e => e.VerbOrdinal)
                .Should().ContainInOrder(0, 1);
        }

        [Fact]
        public void TintsForItem_IsAlignedWithResolveOrdered()
        {
            var items = SyntheticCorpus().Cast<IMindMapItem>().ToList();
            var table = CrossLinkTintTable.Build(items, CrossLink.All);
            var itemsByPath = CrossLinkResolver.ItemsByPath(items);

            foreach (var item in items)
            {
                var expectedVerbs = CrossLinkResolver.ResolveOrdered((Fallacy)item, itemsByPath, CrossLink.All)
                    .Select(group => group.Verb).ToList();
                var handout = table.TintsForItem(item);

                handout.Select(pair => pair.Verb).Should().Equal(expectedVerbs,
                    "the handout must enumerate the item's verbs in the resolver's order");
                foreach (var (verb, tints) in handout)
                {
                    var expectedTints = table.Entries
                        .Where(e => e.SourceId == item.Id && e.Verb == verb)
                        .Select(e => e.TintHex).ToList();
                    tints.Should().Equal(expectedTints,
                        "the handout's tints must be the entries of (item, verb) in walk order");
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // 3. Writer — the .mm carries the tints
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public async Task GenerateMindMapFile_WithCrossLinksEnabled_WritesTheTableTintsIntoTheMm()
        {
            var fallacies = SyntheticCorpus();
            var config = new AssetConverterConfig
            {
                OverwriteExistingDocs = true,
                FreeMindPath = "", // no FreeMind GUI in tests — the .mm is written before any SVG export
            };
            config.LocalizationConfig.DefaultLanguage = "fr";
            var generator = new FallacyMindMapDocumentConfig
            {
                DocumentName = "tint-writer-test.mm",
                CrossLinks = CrossLink.All,
            };

            await generator.GenerateMindMapFile(fallacies, config, _tempTestDirectory, "fr");

            var mmPath = Path.Combine(_tempTestDirectory, generator.DocumentName);
            File.Exists(mmPath).Should().BeTrue("the mind map .mm must be generated");
            var mm = XDocument.Load(mmPath);

            var table = CrossLinkTintTable.Build(fallacies.Cast<IMindMapItem>().ToList(), CrossLink.All);
            var readBack = new List<(string sourceId, string destination, string color)>();
            foreach (var node in mm.Descendants("node"))
            {
                foreach (var arrowlink in node.Elements("arrowlink"))
                {
                    readBack.Add((node.Attribute("ID")!.Value,
                        arrowlink.Attribute("DESTINATION")!.Value,
                        arrowlink.Attribute("COLOR")!.Value));
                }
            }

            readBack.Should().HaveCount(table.Count,
                "one arrowlink per table entry — the .mm is the tint carrier");
            readBack.Select(t => t.color).Distinct().Should().HaveCount(table.Count,
                "every arrowlink carries a DISTINCT tint (uniqueness survived serialization)");
            readBack.Select(t => t.color).Should().BeEquivalentTo(table.Entries.Select(e => e.TintHex),
                "the .mm carries exactly the table's tints");

            // Identity joins through DESTINATION: for each arrowlink, the table must hold the
            // matching (source, verb, target) triple.
            var byId = fallacies.ToDictionary(f => f.Id, f => f.Path);
            foreach (var (sourceId, destination, color) in readBack)
            {
                var entry = table.Entries.Single(e => e.TintHex == color);
                byId[entry.SourceId].Should().Be(byId[sourceId],
                    "tint '{0}' must identify the arrowlink's SOURCE node", color);
                byId[entry.TargetId].Should().Be(byId[destination],
                    "tint '{0}' must identify the arrowlink's DESTINATION node", color);
            }
        }

        [Fact]
        public async Task GenerateMindMapFile_WithCrossLinksDisabled_KeepsTheVerbPastels()
        {
            // Regression guard: CrossLinks=None must produce today's .mm — verb pastels, no
            // tints. The tint machinery is strictly additive.
            var fallacies = SyntheticCorpus();
            var config = new AssetConverterConfig { OverwriteExistingDocs = true, FreeMindPath = "" };
            config.LocalizationConfig.DefaultLanguage = "fr";
            var generator = new FallacyMindMapDocumentConfig
            {
                DocumentName = "tint-disabled-test.mm",
                CrossLinks = CrossLink.None,
            };

            await generator.GenerateMindMapFile(fallacies, config, _tempTestDirectory, "fr");

            var mm = XDocument.Load(Path.Combine(_tempTestDirectory, generator.DocumentName));
            var colors = mm.Descendants("arrowlink").Attributes("COLOR").Select(a => a.Value).ToList();
            colors.Should().BeEmpty("CrossLinks=None draws no arrowlink at all (pre-#2190 behaviour)");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // 4. Reader — Batik-shaped SVG
        // ─────────────────────────────────────────────────────────────────────────────

        private static readonly XNamespace SvgNs = "http://www.w3.org/2000/svg";

        /// <summary>XAttribute string value with the nullability the synthetic documents guarantee.</summary>
        private static string Attr(XElement element, string name) => (string)element.Attribute(name)!;

        /// <summary>
        /// A Batik-shaped export of the synthetic corpus: node texts in translated groups,
        /// one stroked group per cross-link (path + arrowhead polygons inherit the group stroke).
        /// Node positions are spread so the geometric check of the inverse control can tell
        /// them apart.
        /// </summary>
        private static (XDocument svg, Dictionary<string, (double x, double y)> positions) SyntheticBatikSvg(
            CrossLinkTintTable table, Func<string, string> pathToText)
        {
            var positions = new Dictionary<string, (double, double)>
            {
                ["0"] = (5000, 0),
                ["1"] = (4000, -3000), ["1.1"] = (2500, -2500), ["1.2"] = (2500, -2000),
                ["2"] = (6000, -3000), ["2.1"] = (7500, -2500),
                ["3"] = (4000, 3000), ["3.1"] = (5500, 2500),
            };

            var root = new XElement(SvgNs + "svg");
            foreach (var (path, (x, y)) in positions)
            {
                root.Add(new XElement(SvgNs + "g",
                    new XAttribute("transform", $"translate({x},{y})"),
                    new XElement(SvgNs + "text", pathToText(path))));
            }

            foreach (var entry in table.Entries)
            {
                var (sx, sy) = positions[entry.SourcePath];
                var (tx, ty) = positions[entry.TargetPath];
                var rgb = $"rgb({entry.TintRgbKey})";
                root.Add(new XElement(SvgNs + "g",
                    new XAttribute("stroke", rgb),
                    new XAttribute("fill", rgb),
                    new XElement(SvgNs + "path",
                        new XAttribute("d", $"M {sx + 50} {sy + 50} C 1 2 3 4 {tx - 50} {ty - 50}")),
                    new XElement(SvgNs + "polygon", new XAttribute("points", "0,0 10,0 5,10"))));
            }

            return (new XDocument(root), positions);
        }

        [Fact]
        public void RestoreTints_BatikShapedSvg_RestoresPastelsAndAnnotatesEveryLink()
        {
            var items = SyntheticCorpus().Cast<IMindMapItem>().ToList();
            var table = CrossLinkTintTable.Build(items, CrossLink.All);
            var (svg, _) = SyntheticBatikSvg(table, path => "Nœud " + path.Replace('.', '-'));

            var restored = CrossLinkTintTable.RestoreTints(svg, SvgNs, table, "test.content.svg");

            restored.Should().Be(table.Count,
                "every tint-stroked group is restored — count == table size is the full-restore contract");
            var groups = svg.Descendants(SvgNs + "g")
                .Where(g => Attr(g, "class") == "crosslink").ToList();
            groups.Should().HaveCount(table.Count);

            foreach (var entry in table.Entries)
            {
                var strokeKeys = groups.Select(g => CrossLinkTintTable.NormalizeRgbKey(Attr(g, "stroke")));
                strokeKeys.Should().Contain(entry.PastelRgbKey,
                    "verb {0} pastel must be present after restore", entry.Verb);
            }

            // Annotation joins back to the corpus: data-crosslink-source/target are taxonomy PATHS.
            foreach (var g in groups)
            {
                var source = Attr(g, "data-crosslink-source");
                var target = Attr(g, "data-crosslink-target");
                var verb = Attr(g, "data-crosslink-verb");
                table.Entries.Should().Contain(e =>
                        e.SourcePath == source && e.TargetPath == target && e.Verb.ToString() == verb,
                    "every annotation must name a real table entry");
            }

            // Stroke AND fill restored together (the fill carried the tint too).
            groups.Should().OnlyContain(g => Attr(g, "stroke") == Attr(g, "fill"));

            // No tint left anywhere in the document.
            foreach (var entry in table.Entries)
            {
                svg.ToString().Should().NotContain($"rgb({entry.TintRgbKey})",
                    "restore leaves no tint behind — the shipped palette is the verb register");
            }
        }

        [Fact]
        public void RestoreTints_LegacyPastelExport_IsLeftUntouched()
        {
            // The committed assets carry verb pastels (pre-tint exports). The reader must not
            // touch them: zero tint found + pastels present = legacy export, palette already final.
            var items = SyntheticCorpus().Cast<IMindMapItem>().ToList();
            var table = CrossLinkTintTable.Build(items, CrossLink.All);

            var root = new XElement(SvgNs + "svg",
                new XElement(SvgNs + "g",
                    new XAttribute("stroke", "rgb(204,255,255)"),
                    new XAttribute("fill", "rgb(204,255,255)"),
                    new XElement(SvgNs + "path", new XAttribute("d", "M 1 2 C 3 4 5 6 7 8"))));
            var svg = new XDocument(root);
            var before = root.ToString();

            var restored = CrossLinkTintTable.RestoreTints(svg, SvgNs, table, "legacy.content.svg");

            restored.Should().Be(0, "a pastel-only export carries no tint to restore");
            root.ToString().Should().Be(before,
                "the legacy export is byte-identical after the pass — no annotation, no recolor");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // 5. Inverse control — a swapped tint must be DETECTED, not trusted
        // ─────────────────────────────────────────────────────────────────────────────

        /// <summary>
        /// The geometric check of the #2190 proof instrument, in miniature: for every restored
        /// cross-link group, the path's FIRST endpoint must sit near the source node's text and
        /// the LAST endpoint near the target node's text. Identity comes from the tint; geometry
        /// verifies it. Returns the number of links whose endpoints do NOT match their restored
        /// identity.
        /// </summary>
        private static int CountGeometricMismatches(XDocument svg, CrossLinkTintTable table,
            Dictionary<string, (double x, double y)> positions)
        {
            var mismatches = 0;
            foreach (var g in svg.Descendants(SvgNs + "g").Where(g => Attr(g, "class") == "crosslink"))
            {
                var source = positions[Attr(g, "data-crosslink-source")];
                var target = positions[Attr(g, "data-crosslink-target")];
                var numbers = g.Descendants(SvgNs + "path").Attributes("d")
                    .SelectMany(d => d.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Where(t => double.TryParse(t.Trim('M', 'C'), NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                        .Select(t => double.Parse(t.Trim('M', 'C'), NumberStyles.Float, CultureInfo.InvariantCulture)))
                    .ToList();
                var (startX, startY) = (numbers[0], numbers[1]);
                var (endX, endY) = (numbers[^2], numbers[^1]);

                // Tolerance: generous vs the 50 px stub, small vs the 1000+ px inter-node spacing.
                if (Math.Abs(startX - source.x) > 400 || Math.Abs(startY - source.y) > 400) mismatches++;
                if (Math.Abs(endX - target.x) > 400 || Math.Abs(endY - target.y) > 400) mismatches++;
            }
            return mismatches;
        }

        [Fact]
        public void RestoreTints_TintSwapInverseControl_TheGeometricCheckSeesTheDisagreement()
        {
            var items = SyntheticCorpus().Cast<IMindMapItem>().ToList();
            var table = CrossLinkTintTable.Build(items, CrossLink.All);
            var (svg, positions) = SyntheticBatikSvg(table, path => "Nœud " + path.Replace('.', '-'));

            // Unswapped: restore + geometric check = 0 mismatch (the instrument's green path).
            CrossLinkTintTable.RestoreTints(svg, SvgNs, table, "proof.content.svg");
            CountGeometricMismatches(svg, table, positions).Should().Be(0,
                "with tints intact, every restored identity must agree with the arrow's geometry");

            // INVERSE CONTROL: rebuild the export, swap the strokes of two cross-link groups with
            // DIFFERENT sources and different targets (as if the carrier had crossed two links),
            // restore again. The reader — by design — trusts the tint, so it annotates each group
            // with the OTHER link's identity. The geometric check MUST see it: a restored
            // identity is only worth what the endpoints confirm.
            var (swappedSvg, _) = SyntheticBatikSvg(table, path => "Nœud " + path.Replace('.', '-'));
            var crosslinkGroups = swappedSvg.Descendants(SvgNs + "g")
                .Select(g => (Group: g, Entry: table.TryGetEntryByStroke(Attr(g, "stroke"))))
                .Where(pair => pair.Entry != null)
                .ToList();
            crosslinkGroups.Should().HaveCountGreaterOrEqualTo(2, "the control needs two links to swap");
            var first = crosslinkGroups.First(pair => pair.Entry!.SourcePath == "1.1" && pair.Entry.TargetPath == "2.1");
            var second = crosslinkGroups.First(pair => pair.Entry!.SourcePath == "2.1" && pair.Entry.TargetPath == "1.1");
            var strokeA = Attr(first.Group, "stroke");
            var strokeB = Attr(second.Group, "stroke");
            first.Group.SetAttributeValue("stroke", strokeB);
            second.Group.SetAttributeValue("stroke", strokeA);
            // fill follows stroke in a Batik cross-link group — swap it too, as the real defect would.
            var fillA = Attr(first.Group, "fill");
            first.Group.SetAttributeValue("fill", Attr(second.Group, "fill"));
            second.Group.SetAttributeValue("fill", fillA);

            var restored = CrossLinkTintTable.RestoreTints(swappedSvg, SvgNs, table, "proof-swapped.content.svg");
            restored.Should().Be(table.Count,
                "the reader restores every tint it finds — the swap does not lose strokes, it MISLABELS them");

            var mismatches = CountGeometricMismatches(swappedSvg, table, positions);
            mismatches.Should().BeGreaterOrEqualTo(4,
                "both swapped links must disagree geometrically with their restored identity " +
                "(≥ 2 mismatched endpoints per link) — 0 here would mean the check cannot see a " +
                "wrong identity, and every green above is worthless (inverse control)");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // 6. Real corpus — capacity and uniqueness on the shipped taxonomy
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public async Task Build_RealTaxonomyCorpus_IsUniqueDeterministicAndWithinCapacity()
        {
            var csvPath = FindRepoTaxonomyCsv();
            csvPath.Should().NotBeNull("the committed Fallacies taxonomy CSV must be locatable from the test bin dir");

            var items = (await LoadFallaciesFromPathAsync(csvPath!)).Cast<IMindMapItem>().ToList();
            items.Should().NotBeEmpty();

            var first = CrossLinkTintTable.Build(items, CrossLink.All);
            first.Count.Should().BeGreaterThan(0, "the real corpus resolves cross-links (581 at the time of writing)");

            // Derived expectation — never a hardcoded count: the table must hold EXACTLY as many
            // entries as the resolver resolves, with the same triples.
            var itemsByPath = CrossLinkResolver.ItemsByPath(items);
            var expected = items.Where(i => !string.IsNullOrEmpty(i.PK)).OfType<Fallacy>()
                .SelectMany(f => CrossLinkResolver.ResolveOrdered(f, itemsByPath, CrossLink.All)
                    .SelectMany(g => g.Targets.Select(t => (g.Verb, f.Path, t.Path))))
                .ToList();
            first.Entries.Select(e => (e.Verb, e.SourcePath, e.TargetPath)).Should().Equal(expected);

            // Global uniqueness over the real corpus (Leverages is the heavy verb — 327 links,
            // i.e. ~half the capacity; the assert documents the headroom).
            first.Entries.Select(e => e.TintHex).Distinct().Should().HaveCount(first.Count);
            var worst = first.Entries.GroupBy(e => e.Verb).OrderByDescending(g => g.Count()).First();
            _output.WriteLine($"Heaviest verb: {worst.Key} = {worst.Count()} tints (capacity {CrossLinkTintTable.Side * CrossLinkTintTable.Side - 1})");
            worst.Count().Should().BeLessOrEqualTo(CrossLinkTintTable.Side * CrossLinkTintTable.Side - 1,
                "the corpus grew past the per-verb capacity — raise Drift and re-prove family disjointness");

            // Determinism over the real corpus.
            var second = CrossLinkTintTable.Build(items, CrossLink.All);
            first.Entries.Select(e => e.TintHex).Should().Equal(second.Entries.Select(e => e.TintHex));
        }

        private static string? FindRepoTaxonomyCsv()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            for (var i = 0; i < 12 && dir != null; i++, dir = dir.Parent)
            {
                var candidate = Path.Combine(dir.FullName, "Cards", "Fallacies",
                    "Argumentum Fallacies - Taxonomy.csv");
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }
            return null;
        }

        private static async Task<List<Fallacy>> LoadFallaciesFromPathAsync(string csvPath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                // Same header normalization class as the pipeline's loader (CsvBase): the
                // taxonomy's header is "PK" while the ClassMap names "pk" — without this the
                // bind silently yields PK=null on every row and Build skips the whole corpus.
                PrepareHeaderForMatch = args => args.Header.ToLowerInvariant(),
            };
            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<FallacyClassMap>();
            var records = new List<Fallacy>();
            await foreach (var record in csv.GetRecordsAsync<Fallacy>())
            {
                records.Add(record);
            }
            return records;
        }
    }
}
