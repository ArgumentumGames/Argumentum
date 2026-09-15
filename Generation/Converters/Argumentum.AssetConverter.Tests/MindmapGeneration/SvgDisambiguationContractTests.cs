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
    /// Contract pin for the SVG disambiguation heuristics in
    /// <see cref="FallacyMindMapDocumentConfig"/> — CLAUDE.md "Known Fragile Area #1":
    /// "SVG disambiguation in mind map generation" with "fragile heuristics dependent on
    /// Freeplane's output structure."
    ///
    /// <see cref="FallacyMindMapDocumentConfig.CollectPossibleSvgNodes"/> and
    /// <see cref="FallacyMindMapDocumentConfig.DisambiguateSvgNodes"/> are PRIVATE instance methods
    /// bound to <c>TitleFunc</c> / <c>Logger</c> — NOT cleanly extractable (audited + rejected for
    /// the #204 extraction lane). The coordinator dispatched pinning their BEHAVIOR via
    /// <c>XElement</c> fixtures (no production extraction) because the silent-wrong-output risk is
    /// the #1 documented fragility: a regression in the min-length grouping, the 3-char prefix
    /// fallback, or the parent-distance tie-break silently wires each fallacy to the WRONG mind-map
    /// node — caught only by visually inspecting every generated SVG.
    ///
    /// These tests invoke the private methods via reflection against real <see cref="Fallacy"/>
    /// instances (which implement <see cref="IMindMapItem"/> and expose <c>TextFr</c>, so
    /// <c>TitleFunc</c>'s <c>{item.TextFr}</c> interpolation resolves). Additive only — no
    /// production code is modified.
    /// </summary>
    public class SvgDisambiguationContractTests
    {
        private static readonly XNamespace Svg = "http://www.w3.org/2000/svg";

        // Minimal <g> with one <text> child — the shape CollectPossibleSvgNodes scans for.
        private static XElement G(string text, string? id = null)
        {
            var g = new XElement(Svg + "g");
            if (id != null) g.SetAttributeValue("id", id);
            g.Add(new XElement(Svg + "text", text));
            return g;
        }

        // A Fallacy whose TitleFunc ({item.TextFr}) resolves to `textFr`.
        private static Fallacy Item(string textFr, string decimalPath = "1") => new()
        {
            TextFr = textFr,
            DecimalPath = decimalPath,
            Path = decimalPath
        };

        private static XDocument Doc(params XElement[] groups)
            => new(new XElement(Svg + "svg", groups));

        // Reflection entry to the private CollectPossibleSvgNodes.
        private static Dictionary<IMindMapItem, List<XElement>> Collect(
            FallacyMindMapDocumentConfig config, IList<IMindMapItem> items, XDocument doc)
        {
            var m = typeof(FallacyMindMapDocumentConfig).GetMethod(
                "CollectPossibleSvgNodes", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Should().NotBeNull("CollectPossibleSvgNodes must exist on FallacyMindMapDocumentConfig");
            return (Dictionary<IMindMapItem, List<XElement>>)m!.Invoke(config, new object[] { items, doc, Svg })!;
        }

        // Reflection entry to the private DisambiguateSvgNodes.
        private static Dictionary<IMindMapItem, XElement> Disambiguate(
            FallacyMindMapDocumentConfig config,
            Dictionary<IMindMapItem, List<XElement>> collected, IList<IMindMapItem> items)
        {
            var m = typeof(FallacyMindMapDocumentConfig).GetMethod(
                "DisambiguateSvgNodes", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Should().NotBeNull("DisambiguateSvgNodes must exist on FallacyMindMapDocumentConfig");
            return (Dictionary<IMindMapItem, XElement>)m!.Invoke(config, new object[] { collected, items, Svg })!;
        }

        private static FallacyMindMapDocumentConfig Config() => new() { AddNodePath = false };

        // ─────────────────────────────────────────────────────────────────────────────
        // CollectPossibleSvgNodes — the matching + min-length grouping contract.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Collect_SingleExactMatch_ReturnsThatNode()
        {
            var item = Item("Ad Hominem");
            var doc = Doc(G("Ad Hominem", "node1"));

            var collected = Collect(Config(), new List<IMindMapItem> { item }, doc);

            collected.Should().ContainKey(item);
            collected[item].Should().ContainSingle().Which.Attribute("id")!.Value.Should().Be("node1");
        }

        [Fact]
        public void Collect_MultipleMatches_DifferentLengths_KeepsShortest()
        {
            // THE fragile bit: when several <g> contain the title as a substring, the one with the
            // SHORTEST concatenated text wins (the title node itself, not a descendant that happens
            // to repeat it). A regression that keeps the FIRST or the LONGEST silently wires every
            // fallacy to the wrong node.
            var item = Item("Ad Hominem");
            var doc = Doc(
                G("Ad Hominem", "short"),            // length 11 — the true title node
                G("Ad Hominem explanation", "long"), // length 24 — a child re-quoting it
                G("Ad Hominem attack", "mid"));      // length 17

            var collected = Collect(Config(), new List<IMindMapItem> { item }, doc);

            collected[item].Should().ContainSingle().Which.Attribute("id")!.Value.Should().Be("short",
                "the min-length group must win so the title node is selected, not a re-quoting descendant");
        }

        [Fact]
        public void Collect_NoExactMatch_LeavesItemAbsentFromResult()
        {
            // The silent-drop contract: a fallacy with no exact match is NOT added to the result
            // dictionary — the caller only iterates what's present. A regression that added a null
            // entry, or that the caller assumed presence, would NRE downstream. Pin the absence.
            var item = Item("Nonexistent Fallacy");
            var doc = Doc(G("Ad Hominem"));

            var collected = Collect(Config(), new List<IMindMapItem> { item }, doc);

            collected.Should().NotContainKey(item,
                "an unmatched item must be absent (logged via Logger.LogProblem), never a null value entry");
        }

        [Fact]
        public void Collect_SubstringMatchAcrossTextElements_Resolves()
        {
            // The match joins ALL <text> children of a <g> then Contains — a title split across two
            // <text> elements (Freeplane sometimes emits label + detail separately) must still match.
            var item = Item("Ad Hominem");
            var split = new XElement(Svg + "g", new XAttribute("id", "split"),
                new XElement(Svg + "text", "Ad "),
                new XElement(Svg + "text", "Hominem"));
            var doc = Doc(split);

            var collected = Collect(Config(), new List<IMindMapItem> { item }, doc);

            collected[item].Should().ContainSingle().Which.Attribute("id")!.Value.Should().Be("split",
                "string.Join over the <text> children means a title split across elements still matches");
        }

        [Fact]
        public void Collect_TitleShorterThan3CharsAndNoMatch_ThrowsOnPrefixFallback()
        {
            // Documents a LATENT bug worth pinning: the no-match branch calls title.Substring(0, 3)
            // UNCONDITIONALLY (line 1304). A title shorter than 3 chars with no exact match throws
            // ArgumentOutOfRangeException. In practice fallacy titles are long enough, but the code
            // has no guard — pinning the throw documents the hazard (a future caller with a short
            // label crashes loud here rather than silently misbehaving).
            var item = Item("Ab"); // 2 chars
            var doc = Doc(G("Something else entirely"));

            var act = () => Collect(Config(), new List<IMindMapItem> { item }, doc);

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<System.ArgumentOutOfRangeException>(
                    "title.Substring(0,3) on a <3-char title with no exact match throws — the latent guard-less fallback");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // DisambiguateSvgNodes — the passthrough + early-exit contract.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Disambiguate_SingleCandidatePerItem_Passthrough()
        {
            // When each item has exactly one candidate, Disambiguate assigns it directly — no
            // parent-distance logic. Pin that the common path doesn't mutate the selection.
            var item = Item("Ad Hominem");
            var doc = Doc(G("Ad Hominem", "node1"));
            var collected = Collect(Config(), new List<IMindMapItem> { item }, doc);

            var disambiguated = Disambiguate(Config(), collected, new List<IMindMapItem> { item });

            disambiguated.Should().ContainKey(item);
            disambiguated[item].Attribute("id")!.Value.Should().Be("node1");
        }

        [Fact]
        public void Disambiguate_EmptyInput_ReturnsEmpty_NotThrows()
        {
            // Early-exit guard (line 1317): an empty collected dict, or one whose first value is
            // empty, returns an empty dict — it must NOT throw on the Descendants() call.
            var config = Config();
            var empty = new Dictionary<IMindMapItem, List<XElement>>();

            var act = () => Disambiguate(config, empty, new List<IMindMapItem>());

            act.Should().NotThrow();
            var result = act();
            result.Should().BeEmpty();
        }
    }
}
