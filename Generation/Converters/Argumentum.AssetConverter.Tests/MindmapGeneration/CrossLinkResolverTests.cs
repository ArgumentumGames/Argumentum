using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// #1181: the mindmap cross-link resolver must reproduce the OWL emitter's semantics exactly —
    /// split on ';', trim, drop unresolvable paths, drop self-links — because the organ test
    /// (CrossLinkArrowCountTests) asserts arrow counts against corpus counts derived with those
    /// same rules. A divergence here would make the organ's exact-count assertion incoherent.
    /// </summary>
    public class CrossLinkResolverTests
    {
        private static Fallacy Item(string path, string? opposes = null, string? leverages = null, string? pk = null)
        {
            return new Fallacy
            {
                PK = pk ?? path,
                Path = path,
                CrossLinkOpposes = opposes!,
                CrossLinkLeverages = leverages!,
            };
        }

        [Fact]
        public void Resolve_MultiTargetCell_SplitsOnSemicolonAndTrims()
        {
            var a = Item("1.1", leverages: "2.1 ; 2.2 ");
            var items = new[] { a, Item("2.1"), Item("2.2") };
            var byPath = CrossLinkResolver.ItemsByPath(items);

            var links = CrossLinkResolver.Resolve(a, byPath, CrossLink.All).ToList();

            links.Should().HaveCount(2);
            links.Should().OnlyContain(l => l.Verb == CrossLink.Leverages);
            links.Select(l => l.Target.Path).Should().BeEquivalentTo("2.1", "2.2");
        }

        [Fact]
        public void Resolve_DanglingPathOrSelfLink_IsDropped()
        {
            var a = Item("1.1", opposes: "9.9;1.1;2.1");
            var items = new[] { a, Item("2.1") };
            var byPath = CrossLinkResolver.ItemsByPath(items);

            var links = CrossLinkResolver.Resolve(a, byPath, CrossLink.All).ToList();

            links.Should().ContainSingle("the dangling target (9.9) and the self-link (1.1) must both be dropped")
                .Which.Target.Path.Should().Be("2.1");
        }

        [Fact]
        public void Resolve_FlagGating_OnlyEnabledVerbsAreDrawn()
        {
            var a = Item("1.1", opposes: "2.1", leverages: "2.1");
            var items = new[] { a, Item("2.1") };
            var byPath = CrossLinkResolver.ItemsByPath(items);

            var links = CrossLinkResolver.Resolve(a, byPath, CrossLink.Leverages).ToList();

            links.Should().ContainSingle("only crossLink_Leverages was enabled")
                .Which.Verb.Should().Be(CrossLink.Leverages);
        }

        [Fact]
        public void ItemsByPath_DuplicatePath_FirstOccurrenceWins()
        {
            var first = Item("1.1", pk: "first");
            var second = Item("1.1", pk: "second");

            var byPath = CrossLinkResolver.ItemsByPath(new[] { first, second });

            byPath["1.1"].Should().BeSameAs(first,
                "paths are the taxonomy's structural key; the OWL emitter's fallaciesByPath makes the same first-wins assumption");
        }

        [Fact]
        public void GetCrossLinkColor_EveryVerbHasAColor_AndUnknownThrows()
        {
            foreach (var verb in new[] { CrossLink.PredatesOn, CrossLink.Denounces, CrossLink.Leverages,
                         CrossLink.Allows, CrossLink.Opposes, CrossLink.Inverts, CrossLink.Mirrors,
                         CrossLink.IsRelatedTo })
            {
                var color = FallacyMindMapDocumentConfig.GetCrossLinkColor(verb);
                color.Should().StartWith("#").And.HaveLength(7,
                    $"{verb} needs a well-formed #rrggbb color — FreeMind renders it on the arrow");
            }

            var act = () => FallacyMindMapDocumentConfig.GetCrossLinkColor(CrossLink.None);
            act.Should().Throw<System.ArgumentOutOfRangeException>(
                "None is not a drawable verb — the default: branch of the old switch THREW, and the replacement must keep failing loud on unhandled verbs (#1181)");
        }
    }
}
