using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// #1622 point 2 organ — two PKs minting the same IRI, measured on the CSV, before generation.
    ///
    /// #1622 measured ONE pair (PK 703 / PK 840, both converging on <c>questionBeggingAnalogy</c>) and
    /// closed its own report with the honest caveat that this was all it had looked for: "Seule cette
    /// paire de collision a été trouvée ; d'autres libellés anglais qui ne diffèrent que par un
    /// caractère retiré (tiret, apostrophe, virgule) restent possibles — c'est ce que la garde du
    /// point 2 mesurera." This organ IS that measurement, and it answers the question the report left
    /// open.
    ///
    /// It reads the taxonomy CSV — not the OWL — so the next collision is named BEFORE a
    /// <c>--generate-owl</c> pass mints it. That placement is load-bearing, not stylistic: the
    /// committed <c>docs/ontology/argumentum.owl</c> no longer describes the current taxonomy
    /// (measured: 16 concepts it lacks, 9 it carries that no row produces — #1666), so an OWL-side
    /// collision guard would police a corpus that has already moved.
    ///
    /// The transform is the real one, not a re-implementation: <see cref="OwlDocumentConfig.GetId"/>
    /// for Fallacies and <see cref="VirtueOwlDocumentConfig.GetId"/> for Virtues, both invoked on the
    /// corpus labels. A hand-rolled approximation of Humanizer's <c>Camelize</c> here would drift from
    /// the emitter and start guarding a transform nobody ships.
    ///
    /// Boundary against the three neighbour organs (measured, so the overlap is stated rather than
    /// assumed):
    /// <list type="bullet">
    ///   <item><see cref="OwlGetIdPureContractTests"/> pins the TRANSFORM on inline strings; its
    ///   docstring names the collision risk but never walks the corpus.</item>
    ///   <item><see cref="OwlIriFragmentValidityTests"/> reads the COMMITTED OWL, after generation,
    ///   and only rejects raw spaces.</item>
    ///   <item><c>OwlE2EGenerationValidationTests</c> reconciles CSV and OWL on crossLink/AIF
    ///   assertion COUNTS — a collision perturbs none of them.</item>
    /// </list>
    /// None of the three can see a collision before generation; that is the gap this file fills.
    ///
    /// ── MEASURED STATE (base f95c0b70) ───────────────────────────────────────────────────────────
    /// Fallacies: 1408 rows → 1313 distinct fragments and <b>88 collisions</b> — 87 of them two rows
    /// carrying the SAME English label (7 of those involve three rows), 1 of them two DIFFERENT labels
    /// converging through <c>GetId</c> (the #1622 pair). Virtues: 223 rows, <b>0 collisions</b>.
    /// Cross-check against the artefact: 87 of the 88 fragments are already present in the committed
    /// OWL as <c>skos:prefLabel</c> subjects, which is what validates this file's model of the emitter
    /// against the real one.
    ///
    /// This class reads no OWL file — only the two CSVs — but it joins
    /// <see cref="PublishedOntologyCollection"/> because
    /// <see cref="PublishedOntologyCollectionGuardTests"/> matches on the ontology path appearing
    /// anywhere in the source, prose included, and its documented remediation is to join rather than
    /// to argue. Keeping the reference to the OWL is worth the membership: the staleness of that file
    /// is what justifies measuring collisions on the CSV.
    ///
    /// ⚠ The 88 are an OWNER DECISION, not an accepted state (#1622: "Pour les deux nœuds, c'est une
    /// décision éditoriale : doublon à fusionner, ou IRI désambiguïsée"). They are pinned so the set
    /// cannot GROW unnoticed, and the guard reds in the other direction too: resolving one without
    /// removing its pin is itself a failure, so the list gets burned down instead of going stale.
    /// </summary>
    [Collection(PublishedOntologyCollection.Name)]
    public class OwlIriCollisionGuardTests
    {
        private const string FallaciesCsv = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv";
        private const string VirtuesCsv = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv";

        /// <summary>
        /// Row counts of the two corpora, pinned so the organ cannot degenerate to <c>0 == 0</c>
        /// (the #1046 no-op guard): a CSV that fails to load, or a column renamed out from under the
        /// reader, must red rather than pass on an empty walk.
        /// </summary>
        private const int FallaciesRowCount = 1408;
        private const int VirtuesRowCount = 223;

        /// <summary>
        /// Measured collision set — IRI fragment → the PKs minting it, comma-joined.
        ///
        /// Two shapes reach the same fragment, and both are defects (#1622 point 2):
        /// <list type="bullet">
        ///   <item><b>[convergent]</b> — two distinct English labels differing only by a character
        ///   <c>GetId</c> strips (<c>'</c>, <c>-</c>, <c>,</c>, space). The OWL then carries one concept
        ///   with two different <c>prefLabel</c> values for the same language, which SKOS forbids (at
        ///   most one prefLabel per language per concept).</item>
        ///   <item><b>[identical]</b> — several rows carrying the same English label. The OWL merges
        ///   them into one concept and re-declares that class once per row. These sit in DIFFERENT
        ///   families with divergent definitions, so the merge silently fuses taxonomy nodes the CSV
        ///   keeps apart.</item>
        /// </list>
        /// The <c>[…]</c> tags are derived from the measured labels, and
        /// <see cref="ExpectedConvergentFragments"/> re-derives them on every run, so a tag cannot rot
        /// into a lie while the fragment set stays constant.
        /// </summary>
        private static readonly Dictionary<string, string> FallaciesExpectedCollisions = new Dictionary<string, string>
        {
            { "ableism", "132,344" },   // identical
            { "affirmingTheConsequent", "708,731" },   // identical
            { "alwaysBeingRight", "706,1171" },   // identical
            { "amazingFamiliarity", "64,764" },   // identical
            { "anecdotalEvidence", "34,1087" },   // identical
            { "apophenia", "172,1083" },   // identical
            { "appealToConfidence", "75,301" },   // identical
            { "appealToIdentity", "1018,1378" },   // identical
            { "appealToMinority", "120,316" },   // identical
            { "appealToNovelty", "115,1069" },   // identical
            { "appealToTheStick", "343,468" },   // identical
            { "appealToTheStone", "17,1292" },   // identical
            { "argumentOfTheBeard", "660,859" },   // identical
            { "beggingTheQuestion", "183,698" },   // identical
            { "blindItem", "880,924" },   // identical
            { "brainwashing", "478,1302" },   // identical
            { "buckPassing", "1166,1344" },   // identical
            { "clusteringIllusion", "174,643" },   // identical
            { "confirmationBias", "602,965" },   // identical
            { "damningWithFaintPraise", "363,879,1387" },   // identical
            { "deadCatStrategy", "914,1318" },   // identical
            { "deepity", "206,307" },   // identical
            { "denyingTheAntecedent", "722,729" },   // identical
            { "dichotomousThinking", "817,1120" },   // identical
            { "digression", "297,1316" },   // identical
            { "emotionalReasoning", "53,1119" },   // identical
            { "emotiveConjugation", "218,812" },   // identical
            { "exaggeration", "233,893" },   // identical
            { "experimenterEffect", "411,971,1056" },   // identical
            { "falseEquivalence", "768,843" },   // identical
            { "falsePrecision", "668,857" },   // identical
            { "farFetchedHypothesis", "63,763" },   // identical
            { "faultyGeneralisation", "595,1123" },   // identical
            { "fearUncertaintyAndDoubt", "338,920" },   // identical
            { "fearmongering", "418,919" },   // identical
            { "firehoseOfFalsehood", "460,915" },   // identical
            { "gamblersFallacy", "654,717" },   // identical
            { "geneticFallacy", "761,1371" },   // identical
            { "gishGallop", "475,772,1331" },   // identical
            { "gossip", "491,923" },   // identical
            { "gratitudeTrap", "451,1103,1324" },   // identical
            { "haloEffect", "302,1231" },   // identical
            { "hastyConclusion", "759,1127" },   // identical
            { "hindsightBias", "141,1155" },   // identical
            { "idiosyncraticLanguage", "821,1304" },   // identical
            { "illusoryCorrelation", "642,1085" },   // identical
            { "illusoryTruthEffect", "368,1067" },   // identical
            { "infiniteRegress", "979,1349" },   // identical
            { "insensitivityToSampleSize", "600,1084" },   // identical
            { "kafkatrap", "161,986" },   // identical
            { "leapOfFaith", "22,770" },   // identical
            { "lessIsBetterEffect", "1042,1239" },   // identical
            { "magnificationAndMinimization", "895,1101" },   // identical
            { "mentalReservation", "900,1329" },   // identical
            { "metonymy", "295,866" },   // identical
            { "mindProjectionFallacy", "52,1164" },   // identical
            { "minimisation", "496,892" },   // identical
            { "modalScopeFallacy", "757,849" },   // identical
            { "moralEquivalence", "769,1321" },   // identical
            { "moralPanic", "339,922" },   // identical
            { "nirvanaFallacy", "977,1350" },   // identical
            { "noTrueScotsman", "65,616,813" },   // identical
            { "notInventedHere", "106,1187" },   // identical
            { "onTheSpotFallacy", "477,988,1348" },   // identical
            { "personalization", "723,1169" },   // identical
            { "persuasiveDefinition", "184,819" },   // identical
            { "politicalCorrectness", "121,1341" },   // identical
            { "politiciansSyllogism", "21,787" },   // identical
            { "proofSurrogate", "19,771" },   // identical
            { "pseudoscience", "695,1271" },   // identical
            { "psychologicalProjection", "1165,1355" },   // identical
            { "psychologistsFallacy", "51,1055" },   // identical
            { "quantifierShift", "749,850" },   // identical
            { "questionBeggingAnalogy", "703,840" },   // convergent
            { "rationalization", "62,762" },   // identical
            { "scapegoating", "501,960" },   // identical
            { "shiftingGround", "983,1319" },   // identical
            { "should/shouldntAndMust/mustntStatements", "756,1102" },   // identical
            { "slipperySlope", "677,705" },   // identical
            { "spreading", "476,1300" },   // identical
            { "stereotype", "619,1199" },   // identical
            { "strawMan", "168,894,1365" },   // identical
            { "streetlightEffect", "138,1074" },   // identical
            { "substitutingExplanationForPremise", "155,767" },   // identical
            { "sunkCostFallacy", "440,1020" },   // identical
            { "tuQuoque", "714,1362" },   // identical
            { "twoWrongsMakeARight", "713,1325" },   // identical
            { "whisperingCampaign", "882,918" },   // identical
        };

        /// <summary>
        /// The collisions whose rows carry DIFFERENT labels — the SKOS-violating class. Only one today:
        /// the #1622 pair itself. Derived on every run from the measured labels, so the <c>[…]</c> tags
        /// above are checked rather than trusted.
        /// </summary>
        private static readonly HashSet<string> ExpectedConvergentFragments = new HashSet<string>(StringComparer.Ordinal)
        {
            "questionBeggingAnalogy",
        };

        /// <summary>
        /// The Fallacies corpus carries every pinned collision, and every one of them is inside the
        /// Fallacies csv — so this is the only corpus the convergent class is asserted against.
        /// </summary>
        [Fact]
        public void Fallacies_Corpus_MintsOneIriPerRow_ExceptTheDeclaredCollisions()
            => AssertCollisionSet(FallaciesCsv, "PK", "text_en", FallaciesRowCount, OwlDocumentConfig.GetId,
                FallaciesExpectedCollisions, ExpectedConvergentFragments);

        /// <summary>
        /// Measured: 223 rows, zero collisions. Passed here as an EMPTY expectation rather than an
        /// omitted one, so the run states the corpus is clean instead of leaving the class unchecked.
        /// </summary>
        [Fact]
        public void Virtues_Corpus_MintsOneIriPerRow_ExceptTheDeclaredCollisions()
            => AssertCollisionSet(VirtuesCsv, "pk", "title_en", VirtuesRowCount, VirtueOwlDocumentConfig.GetId,
                new Dictionary<string, string>(), Array.Empty<string>());

        /// <summary>
        /// The reading chain end to end on the real corpus: every row is walked, and the collision set
        /// is exactly the pinned one — in both directions.
        /// </summary>
        private static void AssertCollisionSet(string csvRelativePath, string pkColumn, string labelColumn,
            int expectedRowCount, Func<string, string> getId,
            IReadOnlyDictionary<string, string> expectedCollisions, IReadOnlyCollection<string> expectedConvergent)
        {
            var rows = Measure(ResolveRepoFile(csvRelativePath), pkColumn, labelColumn, getId);

            rows.Should().HaveCount(expectedRowCount,
                "the organ must walk the whole corpus '{0}'. A different count means the CSV moved, a " +
                "column was renamed, or the reader silently dropped rows — none of which is a pass.",
                csvRelativePath);

            var collisions = DetectCollisions(rows);
            var measured = collisions.ToDictionary(c => c.Iri, c => string.Join(",", c.Pks), StringComparer.Ordinal);

            var appeared = collisions.Select(c => c.Iri)
                .Except(expectedCollisions.Keys, StringComparer.Ordinal)
                .OrderBy(k => k, StringComparer.Ordinal).ToList();
            var resolved = expectedCollisions.Keys
                .Except(measured.Keys, StringComparer.Ordinal)
                .OrderBy(k => k, StringComparer.Ordinal).ToList();
            var changed = measured.Keys.Intersect(expectedCollisions.Keys, StringComparer.Ordinal)
                .Where(k => !string.Equals(measured[k], expectedCollisions[k], StringComparison.Ordinal))
                .OrderBy(k => k, StringComparer.Ordinal).ToList();

            appeared.Should().BeEmpty(
                "two PKs NEWLY mint the same IRI fragment in '{0}'. Publication (#133) freezes IRIs, so " +
                "this is fixed in the taxonomy — merge the duplicate, or disambiguate the label — and " +
                "never by extending the pin. Measured: {1}",
                csvRelativePath,
                Describe(collisions.Where(c => appeared.Contains(c.Iri))));

            changed.Should().BeEmpty(
                "the PK set behind these fragments moved in '{0}' — same fragment, different rows. " +
                "Measured: {1} | pinned: {2}",
                csvRelativePath,
                Describe(collisions.Where(c => changed.Contains(c.Iri))),
                Describe(expectedCollisions, changed));

            resolved.Should().BeEmpty(
                "these pinned collisions NO LONGER collide in '{0}'. That is the fix landing — remove " +
                "them from the pin so it keeps describing the corpus; a stale pin " +
                "silently re-authorises the collision if it ever comes back. Resolved: {1}",
                csvRelativePath,
                string.Join(", ", resolved));

            var convergent = collisions.Where(c => !c.LabelsAreIdentical).Select(c => c.Iri)
                .OrderBy(k => k, StringComparer.Ordinal).ToList();
            convergent.Should().BeEquivalentTo(expectedConvergent,
                "exactly these fragments are shared by rows with DIFFERENT labels in '{0}', which is the " +
                "shape SKOS forbids (two prefLabels for one language on one concept). A fragment moving " +
                "between the identical-label class and this one is a change the pin alone cannot see. " +
                "Measured: {1}",
                csvRelativePath,
                string.Join(", ", convergent));
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) The detector — pure, on (PK, label, IRI) rows, so a witness can drive it directly.
        //     A mutation here must be caught by the witnesses at the bottom of this file, not only
        //     by the pin: a detector that always returned empty would leave the pin tests green.
        // ─────────────────────────────────────────────────────────────────────────────

        internal sealed class Collision
        {
            public string Iri { get; init; } = "";
            public IReadOnlyList<string> Pks { get; init; } = Array.Empty<string>();
            public bool LabelsAreIdentical { get; init; }
        }

        internal static IReadOnlyList<Collision> DetectCollisions(
            IEnumerable<(string Pk, string Label, string Iri)> rows)
        {
            return rows
                .GroupBy(r => r.Iri, StringComparer.Ordinal)
                .Select(g => new Collision
                {
                    Iri = g.Key,
                    Pks = g.Select(r => r.Pk)
                           .Distinct(StringComparer.Ordinal)
                           .OrderBy(PkRank)
                           .ThenBy(pk => pk, StringComparer.Ordinal)
                           .ToList(),
                    LabelsAreIdentical = g.Select(r => r.Label).Distinct(StringComparer.Ordinal).Count() == 1,
                })
                .Where(c => c.Pks.Count > 1)
                .OrderBy(c => c.Iri, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>
        /// PKs are numeric in both corpora; ordering them as strings would sort "1171" before "706".
        /// Non-numeric PKs fall to the end rather than throwing — the guard's job is to name a
        /// collision, not to reject a corpus over an odd key.
        /// </summary>
        private static int PkRank(string pk)
            => int.TryParse(pk, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) ? n : int.MaxValue;

        private static string Describe(IEnumerable<Collision> collisions)
            => string.Join(" | ", collisions.Select(c =>
                string.Join(",", c.Pks) + " => " + c.Iri + (c.LabelsAreIdentical ? " [identical labels]" : " [convergent labels]")));

        private static string Describe(IReadOnlyDictionary<string, string> map, IEnumerable<string> keys)
            => string.Join(" | ", keys.Select(k => map[k] + " => " + k));

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) The reader — CSV → (PK, label, IRI). Order and duplicates are preserved by
        //     HarvestCardIdsCsv, so the two columns zip into aligned rows.
        // ─────────────────────────────────────────────────────────────────────────────

        private static IReadOnlyList<(string Pk, string Label, string Iri)> Measure(string csvPath,
            string pkColumn, string labelColumn, Func<string, string> getId)
        {
            var csv = new HarvestCardIdsCsv(csvPath);
            var pks = csv.LoadColumn(pkColumn);
            var labels = csv.LoadColumn(labelColumn);

            pks.Should().HaveCount(labels.Count,
                "columns '{0}' and '{1}' of '{2}' must be read in lockstep — a length mismatch means " +
                "the reader dropped rows on one side only, and every pair below would be misaligned",
                pkColumn, labelColumn, csvPath);

            return pks.Select((pk, i) => (Pk: pk, Label: labels[i], Iri: getId(labels[i]))).ToList();
        }

        private static string ResolveRepoFile(string relativePath)
        {
            var candidate = Path.Combine(TestRepoRoot.Find(), relativePath);
            File.Exists(candidate).Should().BeTrue(
                "'{0}' not found under the repo root. It is the source the committed ontology is " +
                "generated from; failing loud rather than skipping, because a skip here would leave " +
                "the collision set unmeasured.", relativePath);
            return candidate;
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) Witnesses — the #1046 doctrine: a guard never seen red is a no-op. These drive the
        //     REAL detector with a shape the corpus does not currently contain, so its ability to red
        //     is asserted rather than assumed.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Witness_ConvergentLabels_AreCaught_AndClassifiedAsSuch()
        {
            // The #1622 pair's own shape. A detector comparing RAW labels would pass this and miss the
            // defect entirely, which is why the IRI — not the label — is the grouping key.
            var left = "Question begging analogy";
            var right = "Question-begging analogy";
            OwlDocumentConfig.GetId(left).Should().Be(OwlDocumentConfig.GetId(right),
                "the witness is only meaningful if these two labels really do converge through GetId");

            var collisions = DetectCollisions(new[]
            {
                ("703", left, OwlDocumentConfig.GetId(left)),
                ("840", right, OwlDocumentConfig.GetId(right)),
                ("1", "Ad Hominem", OwlDocumentConfig.GetId("Ad Hominem")),
            });

            collisions.Should().HaveCount(1);
            collisions[0].Pks.Should().BeEquivalentTo(new[] { "703", "840" },
                "the detector must name BOTH PKs, not merely report that a collision exists");
            collisions[0].LabelsAreIdentical.Should().BeFalse(
                "these rows carry different labels — that is the class SKOS forbids, and it must not be " +
                "reported as the harmless-looking identical-label one");
        }

        [Fact]
        public void Witness_IdenticalLabels_AreCaught_AndClassifiedAsSuch()
        {
            var label = "No true Scotsman";

            var collisions = DetectCollisions(new[]
            {
                ("65", label, OwlDocumentConfig.GetId(label)),
                ("616", label, OwlDocumentConfig.GetId(label)),
                ("813", label, OwlDocumentConfig.GetId(label)),
            });

            collisions.Should().HaveCount(1);
            collisions[0].Pks.Should().BeEquivalentTo(new[] { "65", "616", "813" },
                "a label repeated three times is ONE collision naming three PKs — a pairwise scan would " +
                "report it twice and undercount");
            collisions[0].LabelsAreIdentical.Should().BeTrue();
        }

        [Fact]
        public void Witness_DistinctFragments_ProduceNoCollision()
        {
            var collisions = DetectCollisions(new[]
            {
                ("1", "Ad Hominem", OwlDocumentConfig.GetId("Ad Hominem")),
                ("2", "Straw Man", OwlDocumentConfig.GetId("Straw Man")),
            });

            collisions.Should().BeEmpty(
                "the detector must stay silent on a healthy corpus, otherwise the pin assertions prove " +
                "nothing about the corpus they describe");
        }

        [Fact]
        public void Witness_PksAreOrderedNumerically_NotLexically()
        {
            var label = "Whispering campaign";

            var collisions = DetectCollisions(new[]
            {
                ("918", label, OwlDocumentConfig.GetId(label)),
                ("882", label, OwlDocumentConfig.GetId(label)),
            });

            collisions[0].Pks.Should().BeEquivalentTo(new[] { "882", "918" },
                "and the pinned key below reads in that same order: a lexical sort would render this " +
                "pair as 918,882 and make every pinned line harder to check against the CSV");
        }
    }
}