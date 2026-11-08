using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

using Argumentum.AssetConverter;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;

using FluentAssertions;

using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// #1471 — badge « (en) » / « (fr) » on mind-map nodes whose served link is a cross-language
    /// fallback (reported from #1444). The badge is appended to the FreeMind node TEXT at .mm
    /// generation time (<see cref="MindMapLinkBadge"/> via CreateNode), so it flows into the SVGs
    /// at the next FreeMind pass; the committed SVGs are untouched by this change.
    /// <para>
    /// The derivation mirrors the entity cascade (<c>link_xx ?? link_en ?? link_fr</c>) — the same
    /// fields the token-swapped <c>LinkExpression</c> reads — never an URL probe. Unit tests pin
    /// the derivation per branch (native / en hop / fr hop / no link), the inverse control flips a
    /// badge ON by clearing the native link, and the E2E tests drive the REAL
    /// <c>GenerateMindMapFile</c> path (Fallacies + Virtues) asserting TEXT and LINK together.
    /// </para>
    /// The existing <c>MmGeneratorTests</c> fixture (simple-fallacies.csv) carries a single empty
    /// link_fr column and no link_en — the badge never fires there, which is why its exact-equality
    /// TEXT assertions stay valid: that is the no-badge control pair of this file.
    /// </summary>
    public class MindMapLinkBadgeTests : IDisposable
    {
        private readonly AssetConverterConfig _config;
        private readonly string _tempTestDirectory;

        public MindMapLinkBadgeTests()
        {
            _config = new AssetConverterConfig
            {
                OverwriteExistingDocs = true,
                FreeMindPath = "", // Disable FreeMind GUI in tests — .mm serialization only
            };
            _config.LocalizationConfig.DefaultLanguage = "fr";
            _tempTestDirectory = Path.Combine(Path.GetTempPath(), "MindMapLinkBadgeTests", Path.GetRandomFileName());
            Directory.CreateDirectory(_tempTestDirectory);
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(_tempTestDirectory, true);
            }
            catch (IOException)
            {
                // Temp cleanup is best-effort; a locked file must never fail a test run.
            }
        }

        // ── Unit: the derivation, per cascade branch ─────────────────────────────────────

        [Fact]
        public void Fallacy_NativeLinkPresent_NoBadge()
        {
            var f = new Fallacy { LinkRu = "https://ru.example/native", LinkEn = "https://en.example/x", LinkFr = "https://fr.example/x" };

            MindMapLinkBadge.GetBadgeSourceLanguage(f, "ru").Should().BeNull("the native ru link is served, not a fallback");
            MindMapLinkBadge.GetBadge(f, "ru").Should().BeEmpty();
        }

        [Fact]
        public void Fallacy_NativeMissing_EnServed_BadgeEn()
        {
            var f = new Fallacy { LinkRu = null, LinkEn = "https://en.example/x", LinkFr = "https://fr.example/x" };

            MindMapLinkBadge.GetBadgeSourceLanguage(f, "ru").Should().Be("en", "cascade hop 1 serves link_en when link_ru is empty");
            MindMapLinkBadge.GetBadge(f, "ru").Should().Be(" (en)");
        }

        [Fact]
        public void Fallacy_NativeAndEnMissing_FrServed_BadgeFr()
        {
            var f = new Fallacy { LinkRu = null, LinkEn = null, LinkFr = "https://fr.example/x" };

            MindMapLinkBadge.GetBadgeSourceLanguage(f, "ru").Should().Be("fr", "cascade hop 2 serves link_fr when both link_ru and link_en are empty");
            MindMapLinkBadge.GetBadge(f, "ru").Should().Be(" (fr)");
        }

        [Fact]
        public void Fallacy_NoLinkAnywhere_NoBadge()
        {
            var f = new Fallacy { LinkRu = null, LinkEn = null, LinkFr = null };

            MindMapLinkBadge.GetBadgeSourceLanguage(f, "ru").Should().BeNull("no link at all — the node carries no LINK and no badge");
            MindMapLinkBadge.GetBadge(f, "ru").Should().BeEmpty();
        }

        [Fact]
        public void Fallacy_FrMap_FrMissing_EnServed_BadgeEn()
        {
            // The fr map's own cascade is 2-deep (LinkFr ?? LinkEn): a missing fr link serves
            // link_en — the dominant real case (702/1408 nodes measured on the 2026-11-08 corpus).
            var f = new Fallacy { LinkFr = null, LinkEn = "https://en.example/x" };

            MindMapLinkBadge.GetBadge(f, "fr").Should().Be(" (en)");
        }

        [Fact]
        public void Fallacy_EnMap_EnMissing_FrServed_BadgeFr()
        {
            // The en map's cascade is the inverse 2-deep (LinkEn ?? LinkFr): 23 nodes measured.
            var f = new Fallacy { LinkEn = null, LinkFr = "https://fr.example/x" };

            MindMapLinkBadge.GetBadge(f, "en").Should().Be(" (fr)");
        }

        [Fact]
        public void Fallacy_ClearingNativeLink_FlipsBadgeOn_InverseControl()
        {
            // Inverse control: the SAME item, badge off then on — the mutation that arms the badge
            // is the exact cell the #1471 campaign fills, so this is the coupling between the
            // campaign and the badge receding.
            var f = new Fallacy { LinkRu = "https://ru.example/native", LinkEn = "https://en.example/x" };
            MindMapLinkBadge.GetBadge(f, "ru").Should().BeEmpty("native present — control state");

            f.LinkRu = null; // the campaign would FILL this cell; here we empty it to arm the badge

            MindMapLinkBadge.GetBadge(f, "ru").Should().Be(" (en)", "clearing the native link must flip the badge on");
        }

        [Fact]
        public void Virtue_SameCascade_BadgesMatchFallacies()
        {
            var native = new Virtue { LinkRu = "https://ru.example/v", LinkEn = "https://en.example/v" };
            var viaEn = new Virtue { LinkRu = null, LinkEn = "https://en.example/v" };
            var viaFr = new Virtue { LinkRu = null, LinkEn = null, LinkFr = "https://fr.example/v" };

            MindMapLinkBadge.GetBadge(native, "ru").Should().BeEmpty();
            MindMapLinkBadge.GetBadge(viaEn, "ru").Should().Be(" (en)", "the Virtue cascade (#804) mirrors Fallacy: target -> En -> Fr");
            MindMapLinkBadge.GetBadge(viaFr, "ru").Should().Be(" (fr)");
        }

        [Fact]
        public void UnknownLanguage_NoLinkProperty_NoBadgeAndNoThrow()
        {
            var f = new Fallacy { LinkEn = "https://en.example/x" };

            // A language with no Link<Xx> property on the entity: fail-loud is a logged problem,
            // never an exception in the generation loop, and never a wrong badge.
            var act = () => MindMapLinkBadge.GetBadge(f, "xx");

            act.Should().NotThrow();
            act().Should().BeEmpty("an unknown language cannot derive an origin — no badge");
        }

        // ── E2E: the REAL .mm generation path, Fallacies ─────────────────────────────────

        private static List<Fallacy> BadgeFixtureFallacies() => new()
        {
            // Root: native links in BOTH fr and en (as the real root row does, 2026-11-08 corpus)
            // — the root must NOT be badged.
            new Fallacy { PK = "0", Id = "ROOT", Path = "0", Depth = 0, TextFr = "Sophismes", Famille = "Sophismes",
                DescFr = "Root node", LinkFr = "https://fr.example/sophisme", LinkEn = "https://en.example/fallacy" },
            // Family head with a native fr link — no badge, LINK = fr.
            new Fallacy { PK = "1", Id = "F1", Path = "1", Depth = 1, TextFr = "Famille test", Famille = "Sophismes",
                DescFr = "Family", LinkFr = "https://fr.example/famille" },
            // Native fr MISSING, link_en present — the fr map serves en: badge « (en) », LINK = en.
            new Fallacy { PK = "2", Id = "F11", Path = "1.1", Depth = 2, TextFr = "Natif manquant", Famille = "Sophismes",
                DescFr = "Cell", LinkFr = null, LinkEn = "https://en.example/f11" },
            // Native fr PRESENT, link_en missing — no badge on the fr map; on the EN map it serves
            // link_fr: badge « (fr) », LINK = fr.
            new Fallacy { PK = "3", Id = "F12", Path = "1.2", Depth = 2, TextFr = "Natif présent", Famille = "Sophismes",
                DescFr = "Cell", LinkFr = "https://fr.example/f12", LinkEn = null },
            // No link anywhere — no LINK attribute, no badge.
            new Fallacy { PK = "4", Id = "F13", Path = "1.3", Depth = 2, TextFr = "Sans lien", Famille = "Sophismes",
                DescFr = "Cell", LinkFr = null, LinkEn = null },
        };

        private async Task<XDocument> GenerateFallacyMm(FallacyMindMapDocumentConfig generator, string language)
        {
            // #1192/#2203: Program.IsInteractive is set once for the whole run at assembly
            // level — a test must never assign it (ProcessWideStaticMutationGuard).
            await generator.GenerateMindMapFile(BadgeFixtureFallacies(), _config, _tempTestDirectory, language);
            var mmPath = Path.Combine(_tempTestDirectory, generator.DocumentName);
            File.Exists(mmPath).Should().BeTrue("the mindmap file should have been created");
            return XDocument.Parse(await File.ReadAllTextAsync(mmPath));
        }

        [Fact]
        public async Task GenerateMmFile_FrMap_FallbackNodeCarriesEnBadgeAndLink()
        {
            var generator = new FallacyMindMapDocumentConfig
            {
                DocumentName = "badge-fr.mm",
                CrossLinks = CrossLink.None,
            };

            var xmlDoc = await GenerateFallacyMm(generator, "fr");

            // Root: native fr present — NO badge (the real corpus root has all 8 native links).
            (xmlDoc.Root?.Element("node")?.Attribute("TEXT")?.Value).Should().Be("Sophismes",
                "the root carries a native fr link — never badged");

            // F1 — native fr link: exact title, LINK = fr.
            var f1 = xmlDoc.XPathSelectElement("//node[@ID='F1']");
            (f1?.Attribute("TEXT")?.Value).Should().Be("Famille test", "native link served — the title stays bare");
            (f1?.Attribute("LINK")?.Value).Should().Be("https://fr.example/famille");

            // F11 — fr missing, en served: BADGE + LINK = en. The pair is the feature.
            var f11 = xmlDoc.XPathSelectElement("//node[@ID='F11']");
            (f11?.Attribute("TEXT")?.Value).Should().Be("Natif manquant (en)",
                "the fr map serves link_en — the node tells the reader");
            (f11?.Attribute("LINK")?.Value).Should().Be("https://en.example/f11",
                "the cascade serves the en link (LinkFrFallback with LinkFr empty)");

            // F12 — native fr present: no badge.
            var f12 = xmlDoc.XPathSelectElement("//node[@ID='F12']");
            (f12?.Attribute("TEXT")?.Value).Should().Be("Natif présent");
            (f12?.Attribute("LINK")?.Value).Should().Be("https://fr.example/f12");

            // F13 — no link anywhere: no LINK attribute, no badge.
            var f13 = xmlDoc.XPathSelectElement("//node[@ID='F13']");
            (f13?.Attribute("TEXT")?.Value).Should().Be("Sans lien");
            (f13?.Attribute("LINK")?.Value).Should().BeNull("the cascade is empty — no LINK attribute is written");
        }

        [Fact]
        public async Task GenerateMmFile_EnMap_FrFallbackCarriesFrBadge()
        {
            var generator = new FallacyMindMapDocumentConfig
            {
                DocumentName = "badge-en.mm",
                CrossLinks = CrossLink.None,
                // Mirror of the AssetConverterConfig.MindMapLocalization token-swap for "en"
                // (LinkFrFallback -> LinkEnFallback): the swap is driven by pipeline config, so the
                // test sets the swapped expression explicitly to exercise the en map coherently.
                LinkExpression = @"{item.LinkEnFallback}",
            };

            var xmlDoc = await GenerateFallacyMm(generator, "en");

            // F11 — link_en native on the en map: NO badge (the fr map badged this same node).
            var f11 = xmlDoc.XPathSelectElement("//node[@ID='F11']");
            (f11?.Attribute("TEXT")?.Value).Should().Be("Natif manquant",
                "the en map serves the native en link — no badge, the mirror of the fr map's « (en) »");
            (f11?.Attribute("LINK")?.Value).Should().Be("https://en.example/f11");

            // F12 — link_en missing, fr served: badge « (fr) », LINK = fr.
            var f12 = xmlDoc.XPathSelectElement("//node[@ID='F12']");
            (f12?.Attribute("TEXT")?.Value).Should().Be("Natif présent (fr)",
                "the en map falls back to link_fr — the badge names the served language");
            (f12?.Attribute("LINK")?.Value).Should().Be("https://fr.example/f12");
        }

        // ── E2E: the Virtues side (same injection, own entity) ───────────────────────────

        [Fact]
        public async Task VirtueMm_FrMap_FallbackNodeCarriesEnBadgeAndLink()
        {
            var virtues = new List<Virtue>
            {
                new Virtue { PK = "0", Path = "0", Depth = 0, TitleFr = "Vertus", DescriptionFr = "Root",
                    LinkFr = "https://fr.example/vertu", LinkEn = "https://en.example/virtue" },
                new Virtue { PK = "1", Path = "1", Depth = 1, TitleFr = "Argument pertinent", DescriptionFr = "Family",
                    LinkFr = "https://fr.example/famille" },
                new Virtue { PK = "2", Path = "1.1", Depth = 2, TitleFr = "Virtu sans lien natif", DescriptionFr = "Node",
                    LinkFr = null, LinkEn = "https://en.example/v11" },
            };

            var generator = new VirtueMindMapDocumentConfig
            {
                DocumentName = "badge-virtues-fr.mm",
            };

            // #1192/#2203: Program.IsInteractive is set once for the whole run at assembly
            // level — a test must never assign it (ProcessWideStaticMutationGuard).
            await generator.GenerateMindMapFile(virtues, _config, _tempTestDirectory, "fr");
            var mmPath = Path.Combine(_tempTestDirectory, generator.DocumentName);
            File.Exists(mmPath).Should().BeTrue();
            var xmlDoc = XDocument.Parse(await File.ReadAllTextAsync(mmPath));

            (xmlDoc.Root?.Element("node")?.Attribute("TEXT")?.Value).Should().Be("Vertus",
                "native fr link on the root — no badge");

            var badged = xmlDoc.Descendants("node").FirstOrDefault(n => n.Attribute("TEXT")?.Value == "Virtu sans lien natif (en)");
            badged.Should().NotBeNull("the Virtues CreateNode carries the same #1471 badge as the Fallacies side");
            (badged?.Attribute("LINK")?.Value).Should().Be("https://en.example/v11");
        }
    }
}
