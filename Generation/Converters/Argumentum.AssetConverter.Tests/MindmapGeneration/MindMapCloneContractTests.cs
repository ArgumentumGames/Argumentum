using System.Collections.Generic;
using Argumentum.AssetConverter;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// Contract pin for the mind-map config **deep-clone** machinery — #204 coverage sweep (cont.
    /// po-2024), extending the sweep into the clone path that had ZERO coverage (no reference to
    /// <c>CloneMindMap</c>, <c>GetClone</c>, or <c>.Clone()</c> anywhere in the test project).
    ///
    /// The production entry is <see cref="DocumentConfig.Clone"/> (IClonable), called from
    /// <c>ParallelDocumentCreatorConfigBase.cs</c> to fork a config **per language** in the parallel
    /// translation loop (up to 7 languages × 2 formats). Each clone is then mutated for its language.
    /// The load-bearing invariant: **the clone's collections must be INDEPENDENT of the original's** —
    /// a refactor that turned any deep copy into a shallow one (sharing the same
    /// <c>Dictionary</c>/<c>List</c> reference) would let one language's colors/fonts/SVG maps leak
    /// into every other language's config, producing wrong mind maps with no exception.
    ///
    /// Four pure clone bodies are pinned (all output-neutral; production code untouched, exercised via
    /// the public <see cref="DocumentConfig.Clone"/> / <see cref="SVGFreemindMap"/> entries):
    /// <list type="bullet">
    /// <item><see cref="MindMapDocumentConfig.CloneMindMap"/> — MemberwiseClone + deep copy of
    /// <c>Colors</c>, <c>FontSizes</c>, <c>EdgeSizes</c>, and <c>SVGMaps</c> (element-wise).</item>
    /// <item><see cref="FallacyMindMapDocumentConfig.CloneMindMap"/> — identical 4-collection pattern.</item>
    /// <item><see cref="VirtueMindMapDocumentConfig.CloneMindMap"/> — identical 4-collection pattern.</item>
    /// <item><see cref="SVGFreemindMap"/>.<c>GetClone</c> — MemberwiseClone + element-wise deep copy of
    /// <c>HtmlWrappers</c> (2-level: the list AND each wrapper element).</item>
    /// </list>
    ///
    /// The three <c>MindMapDocumentConfig</c> variants declare their own <c>Colors</c>/<c>FontSizes</c>/
    /// <c>EdgeSizes</c>/<c>SVGMaps</c> (no shared base exposing them), so the deep-copy battery runs
    /// duck-typed via <c>dynamic</c> over the identical member names. Pinned per collection:
    /// reference-distinct (not the same instance), value/count preserved (it is a copy, not empty),
    /// and **independent** (mutating the clone leaves the original untouched — THE shallow-copy
    /// regression signal). The <c>SVGMaps</c> path is pinned 2-levels deep: the list, each element
    /// (element-wise <c>Clone</c>), and each element's <c>HtmlWrappers</c> (chained into
    /// <see cref="SVGFreemindMap"/>'s own clone).
    /// </summary>
    public class MindMapCloneContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // Deep-copy battery for the 3 MindMapDocumentConfig variants.
        // Runs duck-typed (dynamic) over the identical Colors/FontSizes/EdgeSizes/SVGMaps members.
        // ─────────────────────────────────────────────────────────────────────────────

        private static void AssertDocumentConfigCloneIsDeep(dynamic original)
        {
            // Seed CONTROLLED values so the count/value-preservation assertions are non-trivial
            // (the default Colors has 7 family entries; we replace with a known single entry).
            original.Colors.Clear();
            original.Colors[100] = "# seeded";
            original.FontSizes.Clear();
            original.FontSizes.Add(111);
            original.EdgeSizes.Clear();
            original.EdgeSizes.Add(222);
            original.SVGMaps.Clear();
            // A 2-level-deep SVG map: the map itself + an HtmlWrapper it owns.
            dynamic svg = new SVGFreemindMap { SvgWidth = "999" };
            svg.HtmlWrappers.Add(new DocumentConfig { DocumentName = "wrapper-seed" });
            original.SVGMaps.Add(svg);
            original.Translations.Clear();
            original.Translations.Add(("fr", "en"));

            dynamic clone = original.Clone();

            // Distinct instance. (Cast args to object so ReferenceEquals is statically typed bool;
            // otherwise the dynamic args make the bool result dynamic and the .Should() extension won't bind.)
            ReferenceEquals((object)original, (object)clone).Should().BeFalse("Clone must return a new instance, not 'this'");

            // — Colors: reference-distinct, value/count preserved, independent.
            ((object)clone.Colors).Should().NotBeSameAs((object)original.Colors);
            ((int)clone.Colors.Count).Should().Be(1);
            ((string)clone.Colors[100]).Should().Be("# seeded");
            clone.Colors[100] = "# mutated";
            ((string)original.Colors[100]).Should().Be("# seeded",
                "Colors must be deep-copied — mutating the clone must not touch the original");

            // — FontSizes: reference-distinct + independent.
            ((object)clone.FontSizes).Should().NotBeSameAs((object)original.FontSizes);
            clone.FontSizes.Add(999);
            ((IEnumerable<int>)original.FontSizes).Should().NotContain(999);

            // — EdgeSizes: reference-distinct + independent.
            ((object)clone.EdgeSizes).Should().NotBeSameAs((object)original.EdgeSizes);
            clone.EdgeSizes.Add(888);
            ((IEnumerable<int>)original.EdgeSizes).Should().NotContain(888);

            // — SVGMaps: 2-level deep. List reference-distinct, count preserved.
            ((object)clone.SVGMaps).Should().NotBeSameAs((object)original.SVGMaps);
            ((int)clone.SVGMaps.Count).Should().Be(1);
            // Element reference-distinct (element-wise Clone), value preserved.
            ((object)clone.SVGMaps[0]).Should().NotBeSameAs((object)original.SVGMaps[0]);
            ((string)clone.SVGMaps[0].SvgWidth).Should().Be("999");
            // 2nd level: the SVGMap's HtmlWrappers must also be deep (chained SVGFreemindMap.GetClone).
            ((object)clone.SVGMaps[0].HtmlWrappers).Should().NotBeSameAs((object)original.SVGMaps[0].HtmlWrappers);
            ((object)clone.SVGMaps[0].HtmlWrappers[0]).Should().NotBeSameAs((object)original.SVGMaps[0].HtmlWrappers[0]);
            ((string)clone.SVGMaps[0].HtmlWrappers[0].DocumentName).Should().Be("wrapper-seed");

            // — Translations: deep-copied by the base DocumentConfig.Clone overlay.
            ((object)clone.Translations).Should().NotBeSameAs((object)original.Translations);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // The 3 MindMapDocumentConfig variants — same 4-collection deep-copy contract.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void MindMapDocumentConfig_Clone_DeepCopiesCollections()
            => AssertDocumentConfigCloneIsDeep(new MindMapDocumentConfig());

        [Fact]
        public void FallacyMindMapDocumentConfig_Clone_DeepCopiesCollections()
            => AssertDocumentConfigCloneIsDeep(new FallacyMindMapDocumentConfig());

        [Fact]
        public void VirtueMindMapDocumentConfig_Clone_DeepCopiesCollections()
            => AssertDocumentConfigCloneIsDeep(new VirtueMindMapDocumentConfig());

        // ─────────────────────────────────────────────────────────────────────────────
        // SVGFreemindMap — HtmlWrappers element-wise deep copy (its own clone body).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void SVGFreemindMap_Clone_DeepCopiesHtmlWrappers()
        {
            var original = new SVGFreemindMap
            {
                SvgWidth = "800",
                SvgHeight = "600",
                SvgViewBox = "0 0 800 600",
            };
            original.HtmlWrappers.Add(new DocumentConfig { DocumentName = "wrapper-A" });
            original.HtmlWrappers.Add(new DocumentConfig { DocumentName = "wrapper-B" });
            original.Translations.Add(("fr", "en"));

            var clone = (SVGFreemindMap)original.Clone();

            // Distinct instance + scalar values preserved (MemberwiseClone copies scalars).
            ReferenceEquals(original, clone).Should().BeFalse();
            clone.SvgWidth.Should().Be("800");
            clone.SvgHeight.Should().Be("600");

            // HtmlWrappers: list reference-distinct, count preserved, element-wise deep.
            clone.HtmlWrappers.Should().NotBeSameAs(original.HtmlWrappers);
            clone.HtmlWrappers.Count.Should().Be(2);
            clone.HtmlWrappers[0].Should().NotBeSameAs(original.HtmlWrappers[0],
                "each wrapper must be element-wise cloned, not shared by reference");
            clone.HtmlWrappers[1].Should().NotBeSameAs(original.HtmlWrappers[1]);
            clone.HtmlWrappers[0].DocumentName.Should().Be("wrapper-A");

            // Independence: mutating a cloned wrapper leaves the original's wrapper untouched.
            clone.HtmlWrappers[0].DocumentName = "mutated";
            original.HtmlWrappers[0].DocumentName.Should().Be("wrapper-A");

            // Translations: deep-copied by the base DocumentConfig.Clone overlay.
            clone.Translations.Should().NotBeSameAs(original.Translations);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // CloneMindMap — pin the PUBLIC seam directly (not just via the Clone() wrapper).
        // The 4 collections live in CloneMindMap; the base Clone() only adds the Translations overlay.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void MindMapDocumentConfig_CloneMindMap_Direct_DeepCopiesFourCollections()
        {
            var original = new MindMapDocumentConfig();
            original.Colors.Clear();
            original.Colors[42] = "# direct";
            original.FontSizes.Clear();
            original.FontSizes.Add(7);
            original.EdgeSizes.Clear();
            original.EdgeSizes.Add(9);
            original.SVGMaps.Clear();
            original.SVGMaps.Add(new SVGFreemindMap { SvgWidth = "123" });

            // Call the public CloneMindMap() directly (the seam GetClone delegates to).
            var clone = original.CloneMindMap();

            clone.Should().NotBeSameAs(original);
            // 4 collections reference-distinct (the 4 deep copies CloneMindMap performs).
            clone.Colors.Should().NotBeSameAs(original.Colors);
            clone.FontSizes.Should().NotBeSameAs(original.FontSizes);
            clone.EdgeSizes.Should().NotBeSameAs(original.EdgeSizes);
            clone.SVGMaps.Should().NotBeSameAs(original.SVGMaps);
            // Values preserved.
            clone.Colors[42].Should().Be("# direct");
            clone.FontSizes.Should().Contain(7);
            clone.EdgeSizes.Should().Contain(9);
            ((SVGFreemindMap)clone.SVGMaps[0]).SvgWidth.Should().Be("123");
            // Element-wise deep: SVGMap element is a distinct instance.
            clone.SVGMaps[0].Should().NotBeSameAs(original.SVGMaps[0]);

            // Independence: mutating the clone leaves the original's collections untouched.
            clone.Colors[42] = "# changed";
            clone.FontSizes.Add(100);
            clone.EdgeSizes.Add(200);
            original.Colors[42].Should().Be("# direct");
            original.FontSizes.Should().NotContain(100);
            original.EdgeSizes.Should().NotContain(200);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Determinism + characterization.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Clone_IsDeterministic_AndProducesEquivalentCopy()
        {
            var original = new MindMapDocumentConfig();
            original.Colors.Clear();
            original.Colors[1] = "#abc";

            var c1 = (MindMapDocumentConfig)original.Clone();
            var c2 = (MindMapDocumentConfig)original.Clone();

            // Two clones of the same original are equivalent (same colors) but distinct instances.
            c1.Should().NotBeSameAs(c2);
            c1.Colors[1].Should().Be("#abc");
            c2.Colors[1].Should().Be("#abc");
            c1.Colors.Should().NotBeSameAs(c2.Colors);
        }
    }
}
