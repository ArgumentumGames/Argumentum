using Argumentum.AssetConverter.Entities;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Parsing
{
    /// <summary>
    /// Contract pin for the 8 <c>LinkXxFallback</c> cascades on <see cref="Virtue"/> — added in #804
    /// to mirror <see cref="FallacyLinkFallbackTests"/>. Before #804, <see cref="Virtue.Link"/> was
    /// hardcoded to <c>LinkFr</c>, so every non-FR Virtues <c>links.svg</c> resolved its clickable
    /// overlay URLs to French Wikipedia (161 <c>fr.wikipedia.org</c> refs per lang).
    ///
    /// The per-language mind-map link resolution feeds <c>LinkExpression</c>
    /// (<c>{item.LinkFrFallback}</c>) in <c>VirtueMindMapDocumentConfig</c>, which the
    /// <c>MindMapLocalization</c> token-swap (<c>AssetConverterConfig</c>) rewrites per dest-lang
    /// (<c>LinkFrFallback</c> -> <c>LinkEnFallback</c> / <c>LinkRuFallback</c> / ...).
    ///
    /// French is the source language. The cascade design is intentionally ASYMMETRIC, mirroring
    /// <see cref="Fallacy"/>:
    /// <list type="bullet">
    /// <item><description><see cref="Virtue.LinkFrFallback"/> / <see cref="Virtue.LinkEnFallback"/>
    /// are 2-deep (Fr↔En: the two source-ish languages).</description></item>
    /// <item><description><see cref="Virtue.LinkRuFallback"/> / Pt / Es / Ar / Fa / Zh are 3-deep:
    /// target → En → Fr (a non-source language falls back to En, then to the French source).</description></item>
    /// </list>
    ///
    /// The silent-wrong-output risk this guards against: if a refactor "symmetrizes" the cascade
    /// (e.g. makes every language a 2-deep target→Fr, or reorders En/Fr), a virtue that ships with
    /// ONLY a French link would resolve to <c>string.Empty</c> in non-EN exports — producing a
    /// mind-map node with an EMPTY link, no exception, no log. These tests pin the cascade order
    /// so such a regression fails loud.
    /// </summary>
    public class VirtueLinkFallbackTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // (0) Virtue.Link itself must resolve through the cascade (was hardcoded LinkFr pre-#804).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Link_ResolvesThroughFrFallback_NotHardcodedFr()
        {
            // Pre-#804 this returned LinkFr unconditionally. Post-#804 it must respect a present
            // French link but be wired to the fallback property (so the per-lang swap can redirect it).
            var v = new Virtue { LinkFr = "fr-link", LinkEn = "en-link" };
            v.Link.Should().Be("fr-link");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) LinkFrFallback — 2-deep: Fr → En.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LinkFrFallback_PrimaryPresent_ReturnsLinkFr()
        {
            var v = new Virtue { LinkFr = "fr-link", LinkEn = "en-link" };
            v.LinkFrFallback.Should().Be("fr-link");
        }

        [Fact]
        public void LinkFrFallback_PrimaryNull_FallsBackToEn()
        {
            var v = new Virtue { LinkFr = null, LinkEn = "en-link" };
            v.LinkFrFallback.Should().Be("en-link");
        }

        [Fact]
        public void LinkFrFallback_BothNull_ReturnsNull()
        {
            // Same contract as Fallacy: with BOTH Fr and En null, the expression returns LinkEn (null),
            // NOT string.Empty. This is the silent-wrong-output hazard pinned explicitly.
            var v = new Virtue { LinkFr = null, LinkEn = null };
            v.LinkFrFallback.Should().BeNull();
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) LinkEnFallback — 2-deep, INVERSE direction: En → Fr.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LinkEnFallback_PrimaryPresent_ReturnsLinkEn()
        {
            var v = new Virtue { LinkFr = "fr-link", LinkEn = "en-link" };
            v.LinkEnFallback.Should().Be("en-link");
        }

        [Fact]
        public void LinkEnFallback_PrimaryNull_FallsBackToFr()
        {
            var v = new Virtue { LinkFr = "fr-link", LinkEn = null };
            v.LinkEnFallback.Should().Be("fr-link");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) Non-source languages (Ru/Pt/Es/Ar/Fa/Zh) — 3-deep cascade: target → En → Fr.
        // ─────────────────────────────────────────────────────────────────────────────

        // (3a) Target present → returns target (cascade short-circuits).
        [Theory]
        [InlineData(nameof(Virtue.LinkRu), nameof(Virtue.LinkRuFallback))]
        [InlineData(nameof(Virtue.LinkPt), nameof(Virtue.LinkPtFallback))]
        [InlineData(nameof(Virtue.LinkEs), nameof(Virtue.LinkEsFallback))]
        [InlineData(nameof(Virtue.LinkAr), nameof(Virtue.LinkArFallback))]
        [InlineData(nameof(Virtue.LinkFa), nameof(Virtue.LinkFaFallback))]
        [InlineData(nameof(Virtue.LinkZh), nameof(Virtue.LinkZhFallback))]
        public void NonSource_PrimaryPresent_ReturnsPrimary(string linkProp, string fallbackProp)
        {
            var v = new Virtue { LinkFr = "fr-link", LinkEn = "en-link" };
            SetLink(v, linkProp, "xx-link");

            GetFallback(v, fallbackProp).Should().Be("xx-link",
                "the cascade must short-circuit when the target language has its own link");
        }

        // (3b) Target null, En present → returns En (second tier).
        [Theory]
        [InlineData(nameof(Virtue.LinkRu), nameof(Virtue.LinkRuFallback))]
        [InlineData(nameof(Virtue.LinkPt), nameof(Virtue.LinkPtFallback))]
        [InlineData(nameof(Virtue.LinkEs), nameof(Virtue.LinkEsFallback))]
        [InlineData(nameof(Virtue.LinkAr), nameof(Virtue.LinkArFallback))]
        [InlineData(nameof(Virtue.LinkFa), nameof(Virtue.LinkFaFallback))]
        [InlineData(nameof(Virtue.LinkZh), nameof(Virtue.LinkZhFallback))]
        public void NonSource_PrimaryNull_EnPresent_ReturnsEn(string linkProp, string fallbackProp)
        {
            var v = new Virtue { LinkFr = "fr-link", LinkEn = "en-link" };
            SetLink(v, linkProp, null);

            GetFallback(v, fallbackProp).Should().Be("en-link",
                "a non-source language with no own link must fall back to English, NOT French");
        }

        // (3c) Target null, En null, Fr present → returns Fr (source-language floor).
        [Theory]
        [InlineData(nameof(Virtue.LinkRu), nameof(Virtue.LinkRuFallback))]
        [InlineData(nameof(Virtue.LinkPt), nameof(Virtue.LinkPtFallback))]
        [InlineData(nameof(Virtue.LinkEs), nameof(Virtue.LinkEsFallback))]
        [InlineData(nameof(Virtue.LinkAr), nameof(Virtue.LinkArFallback))]
        [InlineData(nameof(Virtue.LinkFa), nameof(Virtue.LinkFaFallback))]
        [InlineData(nameof(Virtue.LinkZh), nameof(Virtue.LinkZhFallback))]
        public void NonSource_TargetAndEnNull_FrPresent_ReturnsFr(string linkProp, string fallbackProp)
        {
            var v = new Virtue { LinkFr = "fr-link", LinkEn = null };
            SetLink(v, linkProp, null);

            GetFallback(v, fallbackProp).Should().Be("fr-link",
                "French is the source language — it is the floor of the cascade, not just a peer");
        }

        // (3d) All three null → null (NOT empty).
        [Theory]
        [InlineData(nameof(Virtue.LinkRu), nameof(Virtue.LinkRuFallback))]
        [InlineData(nameof(Virtue.LinkPt), nameof(Virtue.LinkPtFallback))]
        [InlineData(nameof(Virtue.LinkEs), nameof(Virtue.LinkEsFallback))]
        [InlineData(nameof(Virtue.LinkAr), nameof(Virtue.LinkArFallback))]
        [InlineData(nameof(Virtue.LinkFa), nameof(Virtue.LinkFaFallback))]
        [InlineData(nameof(Virtue.LinkZh), nameof(Virtue.LinkZhFallback))]
        public void NonSource_AllNull_ReturnsNull(string linkProp, string fallbackProp)
        {
            var v = new Virtue { LinkFr = null, LinkEn = null };
            SetLink(v, linkProp, null);

            GetFallback(v, fallbackProp).Should().BeNull();
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // Reflection helpers — let one Theory cover all 6 non-source languages uniformly.
        // ─────────────────────────────────────────────────────────────────────────────

        private static void SetLink(Virtue v, string propName, string? value)
            => typeof(Virtue).GetProperty(propName)!.SetValue(v, value);

        private static string GetFallback(Virtue v, string propName)
            => (string)typeof(Virtue).GetProperty(propName)!.GetValue(v)!;
    }
}
