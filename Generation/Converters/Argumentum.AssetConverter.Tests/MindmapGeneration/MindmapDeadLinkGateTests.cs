using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// #1438 acceptance organ for the mindmap RE-DERIVATION — authored by <b>po-2024</b>
    /// (the lane that repaired the corpus), to be PASSED by the lane that re-derives the
    /// mindmaps. It must NOT be authored by its own executor: an expected value written by
    /// the lane that also runs the check is green by construction (#1112 class of trap).
    ///
    /// The corpus side of #1438 is complete: dead links were repaired or cleared in the
    /// Fallacies taxonomy (#1439 deck — 7 links; #1440 off-deck — 32 don-lindsay cells
    /// mirrored to wayback) and in the Virtues taxonomy (44 dead cells cleared, PR #1442 —
    /// 25 URLs with no wayback snapshot and no equivalent living page). The COMMITTED
    /// mindmaps still carry the dead URLs (measured 2026-09-20: <b>1 215 bare occurrences
    /// of 33 distinct dead URLs across 51 files — 810 in the 43 shipped SVGs + 405 in the
    /// 17 inlining HTML wrappers</b>) until the re-derivation runs. The wrapper half of the
    /// perimeter was an ANGLE BLIND of the first version of this organ (SVG-only: it saw
    /// 67% of the defect and would have passed with 405 clickable dead links in the files a
    /// reader actually opens); it is in scope since grain ⑩ — the #1112 trap applies to the
    /// PERIMETER as much as to the values.
    ///
    /// This organ freezes the end-state of #1438 as three falsifiable invariants over the
    /// shipped SVGs:
    /// <list type="number">
    /// <item><description><b>No bare dead URL</b> — every occurrence of every dead URL must
    /// either be gone (cleared links) or prefixed by a wayback snapshot
    /// (<c>web.archive.org/web/&lt;14 digits&gt;/</c>). The predicate is wayback-aware
    /// because a wayback URL CONTAINS the dead URL as a substring: a naive "0 occurrence
    /// of the old URL" DoD is self-refuting (the measured baseline had a first pass count
    /// doubled by exactly this class of instrument artifact).</description></item>
    /// <item><description><b>Repairs are present</b> (inverse control) — the replacement
    /// URLs recorded in #1439/#1440 must OCCUR in the re-derived SVGs. Without this, a
    /// stale-harvest re-derivation that silently skips link rendering would pass invariant
    /// 1 vacuously (no dead URL anywhere, but no living link either).</description></item>
    /// <item><description><b>Card thumbnails survive</b> — the FR-only cards variant
    /// (<c>Argumentum_Fallacies_MindMap_cards_fr.content.svg</c>) must embed exactly one
    /// base64 thumbnail per deck card, where the deck card count is DERIVED from the
    /// taxonomy CSV at run time (rows with a non-empty <c>carte</c> column — the same
    /// derivation family as <c>PdfDeckCountContractTests</c>, #1187/#1288), never a
    /// hardcoded number.</description></item>
    /// </list>
    ///
    /// <b>Explicit-run gate.</b> The Tests project runs UNFILTERED in CI: a red-by-design
    /// acceptance gate would block the whole repository while the re-derivation is pending.
    /// The organ is therefore inert unless <c>ARGUMENTUM_DEADLINK_GATE=1</c> is set — CI
    /// runs it as a trivial pass, and the re-deriving lane executes it for real with:
    /// <code>ARGUMENTUM_DEADLINK_GATE=1 dotnet test --filter MindmapDeadLinkGate</code>.
    /// The gate is an environment variable rather than a <c>[Fact(Skip=…)]</c> so the
    /// executor never has to edit source (and risk shipping an un-skipped red gate) to run
    /// it. Witness state: RED on the committed SVGs as of 2026-09-20 (810 bare
    /// occurrences), proven by the authoring lane before delivery.
    /// </summary>
    public class MindmapDeadLinkGateTests
    {
        private const string GateEnvVariable = "ARGUMENTUM_DEADLINK_GATE";

        // ── Dead-URL dictionary: state FROZEN by po-2024 from the repair commits ──────────
        // #1439 (deck): 7 links — 4 mirrored to wayback, 1 replaced by the live en-Wikipedia
        // article, 2 cleared (page deleted even from fr.wikipedia; no equivalent kept).
        private static readonly (string Dead, string? Expected)[] RepairedDeckLinks =
        {
            ("http://www.don-lindsay-archive.org/skeptic/arguments.html#poetry",
                "http://web.archive.org/web/20210109083616/http://www.don-lindsay-archive.org/skeptic/arguments.html#poetry"),
            ("http://www.nizkor.org/features/fallacies/questionable-cause.html",
                "https://en.wikipedia.org/wiki/Questionable_cause"),
            ("http://www.sceptiques.qc.ca/dictionnaire/falseanalogy.html",
                "http://web.archive.org/web/20210201140130/http://www.sceptiques.qc.ca/dictionnaire/falseanalogy.html"),
            ("https://fr.wikipedia.org/wiki/Amalgame_(communication)",
                "http://web.archive.org/web/20210214002734/http://fr.wikipedia.org/wiki/Amalgame_(communication)"),
            ("http://www.sceptiques.qc.ca/dictionnaire/perfectsolution.html",
                "http://web.archive.org/web/20210115204652/http://www.sceptiques.qc.ca/dictionnaire/perfectsolution.html"),
            ("http://www.nizkor.org/features/fallacies/appeal-to-common-practice.html", null),
            ("https://fr.wikipedia.org/wiki/Thought-terminating_cliché", null),
        };

        // #1440 (off-deck): the don-lindsay root URL, 32 cells mirrored to this snapshot.
        private const string DonLindsayRoot = "http://www.don-lindsay-archive.org/skeptic/arguments.html";
        private const string DonLindsayWaybackPrefix = "http://web.archive.org/web/20210109083616/";
        private const string DonLindsayWaybackRoot = DonLindsayWaybackPrefix + DonLindsayRoot;

        // PR #1442 (Virtues): 25 dead URLs, all cells cleared — no wayback snapshot exists
        // (24×404 no-snapshot from web.archive.org/web/<ts>/ probes; 2 ladissertation 404s)
        // and no equivalent living page. Two ladissertation entries are the same page under
        // two encodings (literal é / %C3%A9) — both shipped in the mindmaps, both listed.
        private static readonly string[] ClearedVirtuesLinks =
        {
            "https://philosophie.cegeptr.qc.ca/wp-content/uploads/sites/2/2019/01/FICHE-6-Lanalyse-des-analogies.pdf",
            "https://www.agoracademie.com/respect-mutuel-et-respect-de-l-autre-dans-l-echange-constructif/",
            "https://www.ameli.fr/sites/default/files/Documents/911739/document/gestion-risque-methodes-identification-points-vigilance.pdf",
            "https://www.caissedesdepots.fr/agir-ensemble-professionnels/une-explication-suffisante",
            "https://www.franceculture.fr/societe/faut-il-etre-neutre-ou-partisan-sur-les-questions-de-societe",
            "https://www.francetvinfo.fr/societe/education/justice-comment-gerer-les-discussions-en-classe-autour-de-ce-sujet-sensible_2091325.html",
            "https://www.huffpost.com/entry/comment-reconnaitre-une-information-fiable-sur-internet_b_5ac4497ee4b094d3f5c0b93d",
            "https://www.journalism.org/2018/12/03/many-americans-believe-fake-news-is-sowing-confusion/",
            "https://www.ladissertation.com/Philosophie/Les-sophismes/Sophismes-Liste-compl%C3%A8te-sophismes?fbclid=IwAR21LunKe6jOu7e5AadRavnUgHbxMdkdLs7ZyPMZ-I86WzI727kQzt0jLQU",
            "https://www.ladissertation.com/Philosophie/Les-sophismes/Sophismes-Liste-complète-sophismes?fbclid=IwAR21LunKe6jOu7e5AadRavnUgHbxMdkdLs7ZyPMZ-I86WzI727kQzt0jLQU",
            "https://www.laligue.be/associations/la-ligue-de-l-enseignement/methode-pedagogique/les-cinq-regles-d-or-de-l-argumentation/n-2-clarte-des-enjeux",
            "https://www.lesmotssontdescadeaux.com/2018/02/comment-critiquer-sans-agresser.html",
            "https://www.lexpress.fr/actualite/societe/education/neuf-reformes-pour-une-meilleure-ecole_1355387.html",
            "https://www.opinion-internationale.com/2016/09/26/argumenter-construire-idees-renforcer-arugments-efficace_46817.html",
            "https://www.persee.fr/doc/comm_0588-8018_2005_num_78_1_3769",
            "https://www.persee.fr/doc/hel_0750-8069_2003_num_25_1_3148",
            "https://www.philomag.com/les-idees/analyse/les-biais-cognitifs-ou-lassurance-davoir-toujours-raison-14096",
            "https://www.psychologies.com/Culture/Philosophie/L-ouverture-d-esprit-une-vertu",
            "https://www.psychologies.com/Therapies/Toutes-les-therapies/Psychotherapies/Articles-et-Dossiers/Suspendre-le-jugement",
            "https://www.psychologies.com/Therapies/Toutes-les-therapies/Therapies-breves/Articles-et-Dossiers/Le-chantage-affectif",
            "https://www.scienceshumaines.com/les-biais-culturels-nouvelle-source-d-inegalites_fr_40797.html",
            "https://www.service-public.fr/professionnels-entreprises/vosdroits/F33492",
            "https://www.usherbrooke.ca/dca/documents/Dictionnaire_de_lacronyme_UEL.pdf",
            "https://www.usherbrooke.ca/udeconsulter/ethique/ethique/integrite-scientifique/principes/",
            "https://www.village-justice.com/articles/transparence-et-communication-dans-entreprise,35542.html",
        };

        /// <summary>A wayback prefix ends exactly where the archived (dead) URL begins.</summary>
        private static readonly Regex WaybackPrefixAtEnd = new(
            @"web\.archive\.org/web/\d{14}/$", RegexOptions.Compiled);

        /// <summary>Permalink to one base64 card thumbnail in a FreeMind/Batik export.</summary>
        private const string ThumbnailMarker = "xlink:href=\"data:image/png;base64";

        [Fact]
        public void ReDerivedMindmaps_CarryNoBareDeadUrl_AndCarryTheRepairs()
        {
            // Explicit-run gate: inert (trivial pass) in CI, real when the re-deriving lane
            // sets the variable. See the class doc for why this is not [Fact(Skip=…)].
            if (Environment.GetEnvironmentVariable(GateEnvVariable) != "1")
            {
                return;
            }

            var repoRoot = TestRepoRoot.Find();
            var mindmapDir = Path.Combine(repoRoot, "Cards", "Fallacies", "Mindmaps");
            var svgs = Directory.EnumerateFiles(mindmapDir, "*.svg", SearchOption.AllDirectories)
                .OrderBy(p => p, StringComparer.Ordinal)
                .ToList();

            // Anti-collapse floor on the scan perimeter (43 SVGs shipped today: 24 Fallacies
            // + 16 Virtues + 3 FR-only cards). A floor, not an equality — the set may grow;
            // what must never happen is the organ silently scanning an empty directory.
            svgs.Should().HaveCount(c => c >= 40,
                "the shipped mindmap set holds 43 SVGs today; a re-derivation that ships fewer "
                + "than 40 has collapsed a language or a variant");

            // #1438 grain ⑩ — the INLINE HTML WRAPPERS are part of the shipped defect surface,
            // not an afterthought: measured on c56ec6d6, the 17 inlining wrappers (.html files
            // that embed a mindmap SVG verbatim — 2 per language + Fallacies_cards_fr) carry
            // 405 bare dead-URL occurrences, exactly one third of the total, and they are the
            // files a reader actually opens. The 17 _ext wrappers reference their SVG through
            // <object data> and carry 0 URL by construction; the 2 root templates likewise.
            // All .html files are scanned anyway (they can only add zero), but the floor is
            // asserted on the INLINING subset so a re-derivation that stops shipping inline
            // wrappers entirely cannot pass vacuously.
            var htmls = Directory.EnumerateFiles(mindmapDir, "*.html", SearchOption.AllDirectories)
                .OrderBy(p => p, StringComparer.Ordinal)
                .ToList();
            var inliningHtmls = htmls
                .Where(h => File.ReadAllText(h).Contains("<svg", StringComparison.Ordinal))
                .ToList();
            inliningHtmls.Should().HaveCount(c => c >= 17,
                "the shipped set holds 17 inlining wrappers today (2 per language × 8 + "
                + "Fallacies_cards_fr); fewer means the inline delivery of the mindmaps has collapsed");
            var targets = svgs.Concat(htmls).ToList();

            // Every dead URL is probed under both accent encodings: the taxonomies carry the
            // literal é while some SVG export paths percent-encode it.
            var deadForms = new List<string>();
            foreach (var url in RepairedDeckLinks.Select(r => r.Dead)
                         .Append(DonLindsayRoot)
                         .Concat(ClearedVirtuesLinks))
            {
                deadForms.Add(url);
                var encoded = url.Replace("é", "%C3%A9");
                if (!deadForms.Contains(encoded)) deadForms.Add(encoded);
            }

            var bareByForm = deadForms.ToDictionary(f => f, _ => new List<string>(), StringComparer.Ordinal);
            foreach (var target in targets)
            {
                var text = File.ReadAllText(target);
                foreach (var form in deadForms)
                {
                    var start = text.IndexOf(form, StringComparison.Ordinal);
                    while (start >= 0)
                    {
                        var windowStart = Math.Max(0, start - 60);
                        var prefix = text.Substring(windowStart, start - windowStart);
                        if (!WaybackPrefixAtEnd.IsMatch(prefix))
                        {
                            bareByForm[form].Add(Path.GetFileName(target));
                        }
                        start = text.IndexOf(form, start + 1, StringComparison.Ordinal);
                    }
                }
            }

            var bare = bareByForm.Where(kv => kv.Value.Count > 0).ToList();
            var bareSummary = string.Join("; ", bare.Select(kv =>
                $"{kv.Key} x{kv.Value.Count} in {string.Join(", ", kv.Value.Distinct())}"));
            bare.Should().BeEmpty(
                "every dead link of #1438 must be either cleared (no occurrence) or mirrored "
                + $"behind a wayback prefix ({GateEnvVariable}=1 acceptance run). Bare occurrences by URL: "
                + bareSummary);

            // Inverse control — the repairs themselves must SHIP. A re-derivation that drops
            // link rendering entirely would pass the bare-dead-URL check vacuously. Scoped to
            // SVGs AND wrappers: the inlining wrappers embed the SVG verbatim, so a repair
            // that reached the SVGs but was skipped on the wrappers (the silent
            // OverwriteExistingHtmlMaps=false skip, #1438 c.5750260550) must still be caught
            // by the bare-URL assertion above — this control just proves the live URLs exist.
            var allText = targets.Select(File.ReadAllText).ToList();
            foreach (var expected in RepairedDeckLinks.Where(r => r.Expected != null).Select(r => r.Expected!)
                         .Append(DonLindsayWaybackRoot))
            {
                allText.Should().Contain(t => t.Contains(expected, StringComparison.Ordinal),
                    $"the repair recorded in #1439/#1440 must occur in the re-derived mindmaps: {expected}");
            }

            // Card thumbnails — exactly one per deck card, deck size DERIVED from the CSV
            // (non-empty 'carte' column), never hardcoded (#1187/#1288 derivation family).
            var csvPath = Path.Combine(repoRoot, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");
            var deckCards = new HarvestCardIdsCsv(csvPath)
                .LoadColumn("carte")
                .Count(v => !string.IsNullOrWhiteSpace(v));
            deckCards.Should().BeGreaterThan(0, "the Fallacies taxonomy must hold deck rows");

            var cardsSvg = svgs.Single(p =>
                Path.GetFileName(p).Equals("Argumentum_Fallacies_MindMap_cards_fr.content.svg", StringComparison.Ordinal));
            var thumbnails = CountOccurrences(File.ReadAllText(cardsSvg), ThumbnailMarker);
            thumbnails.Should().Be(deckCards,
                $"the FR cards mindmap must embed one base64 thumbnail per deck card ({deckCards} rows "
                + "with a non-empty 'carte' column in the taxonomy)");
        }

        private static int CountOccurrences(string text, string value)
        {
            var count = 0;
            var start = text.IndexOf(value, StringComparison.Ordinal);
            while (start >= 0)
            {
                count++;
                start = text.IndexOf(value, start + 1, StringComparison.Ordinal);
            }
            return count;
        }
    }
}
