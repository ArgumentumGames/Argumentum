using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// Witnesses for the Virtue-twin port (#458, ai-01 GO c.5961901512): the pairing of
    /// <see cref="VirtueMindMapDocumentConfig"/> is now the SHARED
    /// <see cref="MindMapSvgItemPairing"/> generation (#1700 + #1704 suites A and B), not a
    /// divergent copy. Measured 0-write dossier c.5949190678: the committed Virtue artefacts are
    /// healthy (223/223 × 8, 0 homonym) ONLY because no virtue title is homonymous — the old
    /// generation carried three latent defects that would fire SILENTLY the day a retitling
    /// armed them. Each witness below pins one of them, on the twin, red before the port:
    /// <list type="bullet">
    /// <item><description>① <c>Substring(0, 3)</c> crash on a ≤2-char title with no candidate
    /// (the zh Fallacies pass died whole on it — a Virtue title like « 举例 » would too);</description></item>
    /// <item><description>② a node whose text EXACTLY equals item B's title is stolen by item A
    /// whose title it merely CONTAINS (suite A — false fiche on B's node, B mute);</description></item>
    /// <item><description>③ a homonym group with fewer nodes than items is attributed
    /// PARTIALLY with a silent <c>svgNodeToItem[candidate] = item</c> overwrite (suite B —
    /// « a mute node beats a false fiche », the whole group stays mute).</description></item>
    /// </list>
    /// Same reflection pattern as <see cref="SvgDisambiguationContractTests"/>: the methods stay
    /// private instance members of the config (thin forwarders), invoked here via reflection on
    /// real <see cref="Virtue"/> entities so <c>TitleFunc</c> resolves as in production.
    /// </summary>
    public class VirtueSvgNodePairingWitnessTests
    {
        private static readonly XNamespace Svg = "http://www.w3.org/2000/svg";

        private static XElement G(string text, string? id = null)
        {
            var g = new XElement(Svg + "g");
            if (id != null) g.SetAttributeValue("id", id);
            g.Add(new XElement(Svg + "text", text));
            return g;
        }

        private static XDocument Doc(params XElement[] groups)
            => new(new XElement(Svg + "svg", groups));

        private static Virtue Item(string titleFr, string path = "1") => new()
        {
            TitleFr = titleFr,
            DecimalPath = path,
            Path = path
        };

        // TitleFunc is a computed getter resolving TitleExpression — the default
        // "{item.TitleFr}" already resolves our stubs' TitleFr, as in production.
        private static VirtueMindMapDocumentConfig Config() => new();

        // Reflection entry to the private VirtueMindMapDocumentConfig.CollectPossibleSvgNodes.
        private static Dictionary<IMindMapItem, List<XElement>> Collect(
            VirtueMindMapDocumentConfig config, IList<IMindMapItem> items, XDocument doc)
        {
            var m = typeof(VirtueMindMapDocumentConfig).GetMethod(
                "CollectPossibleSvgNodes", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Should().NotBeNull("CollectPossibleSvgNodes must exist on VirtueMindMapDocumentConfig");
            return (Dictionary<IMindMapItem, List<XElement>>)m!.Invoke(config, new object[] { items, doc, Svg })!;
        }

        // Reflection entry to the private VirtueMindMapDocumentConfig.DisambiguateSvgNodes.
        private static Dictionary<IMindMapItem, XElement> Disambiguate(
            VirtueMindMapDocumentConfig config,
            Dictionary<IMindMapItem, List<XElement>> collected, IList<IMindMapItem> items)
        {
            var m = typeof(VirtueMindMapDocumentConfig).GetMethod(
                "DisambiguateSvgNodes", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Should().NotBeNull("DisambiguateSvgNodes must exist on VirtueMindMapDocumentConfig");
            return (Dictionary<IMindMapItem, XElement>)m!.Invoke(config, new object[] { collected, items, Svg })!;
        }

        /// <summary>Sanity floor — the shared pairing still wires the trivial case on the twin.
        /// Guards against a vacuous pass (a port that attributes nothing would pass ①/②/③).</summary>
        [Fact]
        public void SingleExactMatch_Virtue_IsWiredToItsNode()
        {
            var item = Item("Prudence", "2.1");
            var doc = Doc(G("Prudence", "node1"));

            var collected = Collect(Config(), new List<IMindMapItem> { item }, doc);
            var paired = Disambiguate(Config(), collected, new List<IMindMapItem> { item });

            paired.Should().ContainKey(item, "the trivial exact match must stay wired after the port");
            paired[item].Attribute("id")!.Value.Should().Be("node1");
        }

        // ── Witness ① — the 举例 crash ───────────────────────────────────────────────

        /// <summary>
        /// Old twin: the no-candidate branch called <c>title.Substring(0, 3)</c> unconditionally —
        /// a 2-char virtue title with no SVG candidate threw ArgumentOutOfRangeException and killed
        /// the whole language pass (the exact death the zh Fallacies pass died of in #1700).
        /// Shared generation: the diagnostic prefix truncates to the title's own length.
        /// </summary>
        [Fact]
        public void ShortTitleNoMatch_Virtue_IsLoggedNotThrown()
        {
            var item = Item("举例"); // 2 chars — the only <3-char title shape in the corpus
            var doc = Doc(G("完全不同的标题"));

            var act = () =>
            {
                var collected = Collect(Config(), new List<IMindMapItem> { item }, doc);
                collected.Should().NotContainKey(item,
                    "an item whose title matches nothing is left without a node (logged), not force-fed a wrong one");
            };

            act.Should().NotThrow(
                "the close-matches diagnostic must not slice past the title's length — a 2-char " +
                "virtue title with no candidate must not kill the pass");
        }

        // ── Witness ② — exact-title reservation (suite A) ────────────────────────────

        /// <summary>
        /// Old twin: item A (title CONTAINED in B's title) and item B shared the single node whose
        /// text is B's EXACT title — the <c>Count == 1</c> fast path attributed it to BOTH, last
        /// write wins on <c>svgNodeToItem</c>, A's fiche landed on B's node.
        /// Shared generation: <c>exactOwner</c> reserves that node for B's group; A is logged
        /// without a node instead of stealing.
        /// </summary>
        [Fact]
        public void ExactTitleNode_Virtue_IsReservedForItsOwner_NotStolenByContainedTitle()
        {
            // Real shapes from the Fallacies ar review (c.5945920853, PK 298/975) — on Virtues
            // the exposure is conditional (a retitling arms it), the mechanism is identical.
            var shortItem = Item("الاعتراض", "2.1");
            var longItem = Item("المغالطات الاعتراضية", "3.1");
            var items = new List<IMindMapItem> { shortItem, longItem };
            var doc = Doc(G("المغالطات الاعتراضية", "exact-long")); // exact text of longItem only

            var collected = Collect(Config(), items, doc);
            var paired = Disambiguate(Config(), collected, items);

            paired.Should().ContainKey(longItem, "the exact-title owner keeps its node");
            paired[longItem].Attribute("id")!.Value.Should().Be("exact-long");
            paired.Should().NotContainKey(shortItem,
                "an item whose ONLY candidate carries another item's exact title stays mute — " +
                "a false fiche on the exact node is the defect #1700 suite A closed on Fallacies");
        }

        // ── Witness ③ — scarce homonym group (suite B) ───────────────────────────────

        /// <summary>
        /// Old twin: two homonym items, one node — the <c>Count == 1</c> fast path attributed the
        /// node to each in turn (silent overwrite), producing one guaranteed-false fiche.
        /// Shared generation: fewer nodes than items in a group → the WHOLE group stays mute and
        /// is logged (« a mute node beats a false fiche », #1704).
        /// </summary>
        [Fact]
        public void ScarceHomonymGroup_Virtue_StaysMuteEntirely()
        {
            var first = Item("Redoublement", "2.1");
            var second = Item("Redoublement", "3.1");
            var items = new List<IMindMapItem> { first, second };
            var doc = Doc(G("Redoublement", "only-node"));

            var collected = Collect(Config(), items, doc);
            var paired = Disambiguate(Config(), collected, items);

            paired.Should().NotContainKey(first,
                "a scarce homonym group must stay mute entirely, not attribute by guess");
            paired.Should().NotContainKey(second,
                "both homonyms share the fate — partial attribution is a guaranteed false fiche");
        }
    }
}
