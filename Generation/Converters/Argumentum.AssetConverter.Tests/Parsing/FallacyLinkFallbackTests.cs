using Argumentum.AssetConverter.Entities;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Parsing
{
    /// <summary>
    /// Contract pin for the 8 <c>LinkXxFallback</c> cascades on <see cref="Fallacy"/> — the
    /// per-language mind-map link resolution that feeds <c>LinkExpression</c>
    /// (<c>{item.LinkFrFallback}</c>) in <c>FallacyMindMapDocumentConfig</c>, and which is
    /// referenced (but NOT behavior-pinned) by <c>MmGeneratorTests</c> and
    /// <c>MindMapLocalizationRegressionTests</c>.
    ///
    /// French is the source language. The cascade design is intentionally ASYMMETRIC — a subtle
    /// fragility that has had ZERO unit coverage:
    /// <list type="bullet">
    /// <item><description><see cref="Fallacy.LinkFrFallback"/> / <see cref="Fallacy.LinkEnFallback"/>
    /// are 2-deep (Fr↔En: the two source-ish languages).</description></item>
    /// <item><description><see cref="Fallacy.LinkRuFallback"/> / Pt / Es / Ar / Fa / Zh are 3-deep:
    /// target → En → Fr (a non-source language falls back to En, then to the French source).</description></item>
    /// </list>
    ///
    /// The silent-wrong-output risk this guards against: if a refactor "symmetrizes" the cascade
    /// (e.g. makes every language a 2-deep target→Fr, or reorders En/Fr), a fallacy that ships with
    /// ONLY a French link would resolve to <c>string.Empty</c> in non-EN exports — producing a
    /// mind-map node with an EMPTY link, no exception, no log. The regression is invisible except
    /// by opening every generated mind-map and checking each node. These tests pin the cascade
    /// order additively so such a regression fails loud.
    /// </summary>
    public class FallacyLinkFallbackTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // (1) LinkFrFallback — 2-deep: Fr → En.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LinkFrFallback_PrimaryPresent_ReturnsLinkFr()
        {
            var f = new Fallacy { LinkFr = "fr-link", LinkEn = "en-link" };
            f.LinkFrFallback.Should().Be("fr-link");
        }

        [Fact]
        public void LinkFrFallback_PrimaryNull_FallsBackToEn()
        {
            // The fragile bit: a missing French link silently uses the English one.
            var f = new Fallacy { LinkFr = null, LinkEn = "en-link" };
            f.LinkFrFallback.Should().Be("en-link");
        }

        [Fact]
        public void LinkFrFallback_BothNull_ReturnsNull()
        {
            // Edge of the cascade: no Fr AND no En. The expression is
            // `IsNullOrEmpty(LinkFr) ? LinkEn : LinkFr` — with BOTH null it returns LinkEn, which is
            // null. NOT string.Empty. This is the actual (current) contract: a fallacy with no Fr/En
            // link resolves to NULL, which propagates into the Mustache template as empty text but is
            // NULL to any C# consumer — a silent-wrong-output hazard this test pins explicitly.
            var f = new Fallacy { LinkFr = null, LinkEn = null };
            f.LinkFrFallback.Should().BeNull();
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) LinkEnFallback — 2-deep, INVERSE direction: En → Fr.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LinkEnFallback_PrimaryPresent_ReturnsLinkEn()
        {
            var f = new Fallacy { LinkFr = "fr-link", LinkEn = "en-link" };
            f.LinkEnFallback.Should().Be("en-link");
        }

        [Fact]
        public void LinkEnFallback_PrimaryNull_FallsBackToFr()
        {
            // Inverse of LinkFrFallback: missing En silently uses French.
            var f = new Fallacy { LinkFr = "fr-link", LinkEn = null };
            f.LinkEnFallback.Should().Be("fr-link");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) Non-source languages (Ru/Pt/Es/Ar/Fa/Zh) — 3-deep cascade: target → En → Fr.
        //     Pinned via Theory across all 6 languages × the 3 meaningful branches.
        // ─────────────────────────────────────────────────────────────────────────────

        // (3a) Target present → returns target (cascade short-circuits).
        [Theory]
        [InlineData(nameof(Fallacy.LinkRu), nameof(Fallacy.LinkRuFallback))]
        [InlineData(nameof(Fallacy.LinkPt), nameof(Fallacy.LinkPtFallback))]
        [InlineData(nameof(Fallacy.LinkEs), nameof(Fallacy.LinkEsFallback))]
        [InlineData(nameof(Fallacy.LinkAr), nameof(Fallacy.LinkArFallback))]
        [InlineData(nameof(Fallacy.LinkFa), nameof(Fallacy.LinkFaFallback))]
        [InlineData(nameof(Fallacy.LinkZh), nameof(Fallacy.LinkZhFallback))]
        public void NonSource_PrimaryPresent_ReturnsPrimary(string linkProp, string fallbackProp)
        {
            var f = new Fallacy { LinkFr = "fr-link", LinkEn = "en-link" };
            SetLink(f, linkProp, "xx-link");

            GetFallback(f, fallbackProp).Should().Be("xx-link",
                "the cascade must short-circuit when the target language has its own link");
        }

        // (3b) Target null, En present → returns En (second tier).
        [Theory]
        [InlineData(nameof(Fallacy.LinkRu), nameof(Fallacy.LinkRuFallback))]
        [InlineData(nameof(Fallacy.LinkPt), nameof(Fallacy.LinkPtFallback))]
        [InlineData(nameof(Fallacy.LinkEs), nameof(Fallacy.LinkEsFallback))]
        [InlineData(nameof(Fallacy.LinkAr), nameof(Fallacy.LinkArFallback))]
        [InlineData(nameof(Fallacy.LinkFa), nameof(Fallacy.LinkFaFallback))]
        [InlineData(nameof(Fallacy.LinkZh), nameof(Fallacy.LinkZhFallback))]
        public void NonSource_PrimaryNull_EnPresent_ReturnsEn(string linkProp, string fallbackProp)
        {
            var f = new Fallacy { LinkFr = "fr-link", LinkEn = "en-link" };
            SetLink(f, linkProp, null);

            GetFallback(f, fallbackProp).Should().Be("en-link",
                "a non-source language with no own link must fall back to English, NOT French");
        }

        // (3c) Target null, En null, Fr present → returns Fr (source-language floor).
        [Theory]
        [InlineData(nameof(Fallacy.LinkRu), nameof(Fallacy.LinkRuFallback))]
        [InlineData(nameof(Fallacy.LinkPt), nameof(Fallacy.LinkPtFallback))]
        [InlineData(nameof(Fallacy.LinkEs), nameof(Fallacy.LinkEsFallback))]
        [InlineData(nameof(Fallacy.LinkAr), nameof(Fallacy.LinkArFallback))]
        [InlineData(nameof(Fallacy.LinkFa), nameof(Fallacy.LinkFaFallback))]
        [InlineData(nameof(Fallacy.LinkZh), nameof(Fallacy.LinkZhFallback))]
        public void NonSource_TargetAndEnNull_FrPresent_ReturnsFr(string linkProp, string fallbackProp)
        {
            // THE most fragile branch: a fallacy shipping with ONLY a French link must still resolve
            // to French in every non-source export. A "symmetrized" 2-deep cascade would drop this
            // floor and return empty here → silent empty mind-map link.
            var f = new Fallacy { LinkFr = "fr-link", LinkEn = null };
            SetLink(f, linkProp, null);

            GetFallback(f, fallbackProp).Should().Be("fr-link",
                "French is the source language — it is the floor of the cascade, not just a peer");
        }

        // (3d) All three null → null (NOT empty — see LinkFrFallback_BothNull_ReturnsNull note).
        [Theory]
        [InlineData(nameof(Fallacy.LinkRu), nameof(Fallacy.LinkRuFallback))]
        [InlineData(nameof(Fallacy.LinkPt), nameof(Fallacy.LinkPtFallback))]
        [InlineData(nameof(Fallacy.LinkEs), nameof(Fallacy.LinkEsFallback))]
        [InlineData(nameof(Fallacy.LinkAr), nameof(Fallacy.LinkArFallback))]
        [InlineData(nameof(Fallacy.LinkFa), nameof(Fallacy.LinkFaFallback))]
        [InlineData(nameof(Fallacy.LinkZh), nameof(Fallacy.LinkZhFallback))]
        public void NonSource_AllNull_ReturnsNull(string linkProp, string fallbackProp)
        {
            var f = new Fallacy { LinkFr = null, LinkEn = null };
            SetLink(f, linkProp, null);

            GetFallback(f, fallbackProp).Should().BeNull();
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Reflection helpers — let one Theory cover all 6 non-source languages uniformly.
        // ─────────────────────────────────────────────────────────────────────────────

        private static void SetLink(Fallacy f, string propName, string value)
            => typeof(Fallacy).GetProperty(propName)!.SetValue(f, value);

        private static string GetFallback(Fallacy f, string propName)
            => (string)typeof(Fallacy).GetProperty(propName)!.GetValue(f)!;
    }
}
