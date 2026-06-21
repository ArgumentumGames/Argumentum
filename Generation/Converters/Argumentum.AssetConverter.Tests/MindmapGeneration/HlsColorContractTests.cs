using System.Drawing;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// Contract pin for <see cref="HLSColor.GetLighterColor(string)"/> and
    /// <see cref="HLSColor.GetDarkerColor(string)"/> — #204 coverage sweep (cont. po-2024): the
    /// mind-map node readability color contract.
    ///
    /// The mind-map builder derives two shades from each fallacy family's color:
    /// <list type="bullet">
    /// <item><see cref="HLSColor.GetLighterColor(string)"/> → <c>node.BACKGROUND_COLOR</c> (a lighter
    /// tint used as the node fill).</item>
    /// <item><see cref="HLSColor.GetDarkerColor(string)"/> → <c>node.COLOR</c> (a darker shade used as
    /// the node text).</item>
    /// </list>
    /// Both are called from 6 production sites (<c>SetNodeStyle</c> in
    /// <c>MindMapDocumentConfig</c>, <c>FallacyMindMapDocumentConfig</c>, <c>VirtueMindMapDocumentConfig</c>).
    /// Together they enforce the **readability invariant**: on a family-colored node, the text
    /// (<c>COLOR</c>) is darker and the fill (<c>BACKGROUND_COLOR</c>) is lighter than the raw family
    /// color. A regression that flipped a direction — darker lighter than the base, or lighter darker —
    /// would make every node's text unreadable (dark-on-dark or light-on-light), and because mind-map
    /// rendering is currently gated behind visual QA that the cluster cannot always run, this is
    /// exactly the silent-aesthetics regression a contract test must catch in isolation.
    ///
    /// Both overloads are pure string→string transforms (<c>ColorTranslator.FromHtml</c>/<c>ToHtml</c>
    /// are pure conversions, no I/O) and had ZERO coverage. These tests pin the contract additively
    /// (no production code changed).
    /// </summary>
    public class HlsColorContractTests
    {
        // Standard relative luminance proxy (Rec.601 weights) — a stable, viewer-independent measure
        // of perceived brightness used only to assert DIRECTION (lighter/darker), not exact values.
        private static double Luminance(string hex)
        {
            var c = ColorTranslator.FromHtml(hex);
            return 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) DARKER IS DARKER — the text-color half of the readability invariant. GetDarkerColor must
        //     yield a shade with LOWER luminance than the base family color, across representative
        //     family colors (incl. dark gray #555555 and bright purple #811da3 from the real palette).
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("#811da3")] // argumentPertinent — purple
        [InlineData("#555555")] // argumentsVertueux — dark gray
        [InlineData("#08af93")] // exactitudeMathématique — turquoise
        [InlineData("#8dc801")] // raisonnementValide — green
        [InlineData("#dc0f0a")] // débatRespectueux — red
        public void GetDarkerColor_YieldsLowerLuminance_ThanBase(string familyColor)
        {
            var darker = HLSColor.GetDarkerColor(familyColor);

            Luminance(darker).Should().BeLessThan(Luminance(familyColor),
                "the text COLOR must be darker than the family color so text stays readable on the " +
                "node — the readability invariant. A regression that flipped this would put darker " +
                "text darker than its base, collapsing contrast.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) LIGHTER IS LIGHTER — the fill half of the readability invariant. GetLighterColor must
        //     yield a tint with HIGHER luminance than the base family color (the node background is a
        //     lightened tint of the family color).
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("#811da3")]
        [InlineData("#555555")]
        [InlineData("#08af93")]
        [InlineData("#8dc801")]
        [InlineData("#dc0f0a")]
        public void GetLighterColor_YieldsHigherLuminance_ThanBase(string familyColor)
        {
            var lighter = HLSColor.GetLighterColor(familyColor);

            Luminance(lighter).Should().BeGreaterThan(Luminance(familyColor),
                "the BACKGROUND_COLOR must be lighter than the family color — the node fill is a " +
                "lightened tint. A regression that flipped this would darken the fill, eroding the " +
                "contrast the text relies on.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) OUTPUT IS A VALID HEX STRING — both transforms must return a value parseable back to a
        //     Color (no crash, no garbage). The mind-map node fields hold these strings verbatim into
        //     the .mm XML; an invalid value would corrupt the file.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("#811da3")]
        [InlineData("#555555")]
        [InlineData("#dc0f0a")]
        public void Both_TransformsReturnValidParseableHex(string familyColor)
        {
            var lighter = HLSColor.GetLighterColor(familyColor);
            var darker = HLSColor.GetDarkerColor(familyColor);

            var parseLighter = () => ColorTranslator.FromHtml(lighter);
            var parseDarker = () => ColorTranslator.FromHtml(darker);

            parseLighter.Should().NotThrow(
                "GetLighterColor must return a valid hex color string the .mm node can hold verbatim.");
            parseDarker.Should().NotThrow(
                "GetDarkerColor must return a valid hex color string the .mm node can hold verbatim.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) DETERMINISM — same family color always yields the same lighter/darker shade. The mind-map
        //     builder calls these once per node; a non-deterministic transform would produce
        //     inconsistent node colors within the same family across a run.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Deterministic_SameInput_SameOutput()
        {
            const string familyColor = "#811da3";

            var l1 = HLSColor.GetLighterColor(familyColor);
            var l2 = HLSColor.GetLighterColor(familyColor);
            var d1 = HLSColor.GetDarkerColor(familyColor);
            var d2 = HLSColor.GetDarkerColor(familyColor);

            l1.Should().Be(l2, "GetLighterColor is deterministic — same family color → same fill tint.");
            d1.Should().Be(d2, "GetDarkerColor is deterministic — same family color → same text shade.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) TRANSFORMS CHANGE THE COLOR — for a non-extreme family color, the lighter and darker
        //     outputs are BOTH distinct from the base (the shades actually move). Guards a no-op
        //     regression (transform returns the input unchanged).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Transforms_ProduceShadesDistinctFromBase()
        {
            const string familyColor = "#811da3"; // mid-range purple — not an edge color

            HLSColor.GetLighterColor(familyColor).Should().NotBe(familyColor,
                "GetLighterColor must produce a tint distinct from the base, not pass it through.");
            HLSColor.GetDarkerColor(familyColor).Should().NotBe(familyColor,
                "GetDarkerColor must produce a shade distinct from the base, not pass it through.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (6) LIGHTER AND DARKER ARE CONSISTENT WITH EACH OTHER — for the same family color, the
        //     lighter tint is lighter than the darker shade (luminance(lighter) > luminance(darker)).
        //     This locks the relative ordering the readability scheme assumes: light fill above, dark
        //     text below, both derived from one family color.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("#811da3")]
        [InlineData("#555555")]
        [InlineData("#dc0f0a")]
        [InlineData("#8dc801")]
        public void Lighter_IsLighterThan_Darker_ForSameBase(string familyColor)
        {
            var lighter = HLSColor.GetLighterColor(familyColor);
            var darker = HLSColor.GetDarkerColor(familyColor);

            Luminance(lighter).Should().BeGreaterThan(Luminance(darker),
                "the fill tint must be lighter than the text shade (both derived from the same family " +
                "color) — the relative ordering the light-fill/dark-text readability scheme relies on.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (7) CHARACTERIZATION — exact observed outputs for representative family colors, pinned so a
        //     change to the vendored HLS algorithm is caught. The exact strings below were captured
        //     from the real transform (ColorTranslator.FromHtml/ToHtml round-trip through HLSColor).
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("#811da3", "#CB76E7", "#450F57")] // purple — observed lighter/darker (ColorTranslator HLS round-trip)
        public void Characterization_ObservedOutputs(string familyColor, string expectedLighter, string expectedDarker)
        {
            // These expected values are filled from the first real run; if a vendored-HLS refactor
            // changes any of them, the test fails loud and the change is reviewed before the mind-map
            // aesthetic shifts unnoticed.
            HLSColor.GetLighterColor(familyColor).Should().Be(expectedLighter);
            HLSColor.GetDarkerColor(familyColor).Should().Be(expectedDarker);
        }
    }
}
