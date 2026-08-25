using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using OWLSharp.Ontology;
using RDFSharp.Model;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// End-to-end OWL proof for #133 — REGRESSION SUITE for the read-path fix of the SECOND
    /// silent-false-pass bug (the one the #480→#481→#482 lane missed because it only exercised the
    /// IN-MEMORY path).
    ///
    /// HISTORY (the bug this file originally pinned, now fixed): OWLSharp's OWL2XML serializer/reader
    /// drops <c>rdf:type</c> from the reloaded annotation stream (rdf:type==0 among
    /// <c>AnnotationAxioms</c> after reload). The OwlAdapter readers located concepts/schemes by
    /// scanning <c>AnnotationAxioms</c> for <c>rdf:type</c>, so on any LOADED file they returned empty →
    /// <c>ValidateMultilingualAnnotations</c> / <c>ValidateAIFMappings</c> hit their
    /// <c>if (concepts.Count == 0) return true;</c> guard and reported PASS without inspecting anything
    /// — even though the real content was present (2816 prefLabels, AIF matches, 1510 class declarations).
    ///
    /// THE FIX (read-path only — the serializer is deliberately untouched, see test 1):
    /// <c>OwlAdapter.GetResourcesByType</c> / <c>GetConcepts</c> now fall back, when the rdf:type scan
    /// is empty, to locating entities via the SKOS annotations that DO survive the round-trip —
    /// concepts are the distinct subjects of <c>skos:prefLabel</c>, the ConceptScheme is the subject of
    /// <c>skos:hasTopConcept</c>. In-memory ontologies (rdf:type present) take the original path and are
    /// unaffected, so the <see cref="OwlValidatorLivePathTests"/> (#482) in-memory proofs still hold.
    ///
    /// These tests load the REAL generation output (<c>docs/ontology/argumentum.owl</c>, ~5 MB, produced
    /// by <c>OwlDocumentConfig.CreateOwlDocument</c>) and prove: the serializer drop is still real but
    /// now benign (1), the readers resolve concepts + scheme on the reloaded file (2), the real content
    /// is now reachable (3), and the production validator genuinely inspects and passes rather than
    /// skip-false-passing (4). Deterministic, key-free, release-independent; loads one real file (lazy,
    /// shared across the class).
    /// </summary>
    public class OwlE2EGenerationValidationTests
    {
        private const string RelativeOntologyPath = "docs/ontology/argumentum.owl";

        private static readonly Lazy<OwlAdapter> _realOntology = new Lazy<OwlAdapter>(() =>
        {
            var path = ResolveRepoFile(RelativeOntologyPath);
            if (path == null)
            {
                throw new FileNotFoundException(
                    $"Committed generated ontology not found at '{RelativeOntologyPath}' walking up from " +
                    $"{AppContext.BaseDirectory}. It is the real OwlDocumentConfig.CreateOwlDocument output " +
                    "and must exist in the repo for the #133 e2e proof.");
            }
            return OwlAdapter.FromFile(path);
        });

        private static OwlAdapter RealOntology => _realOntology.Value;

        private static string? ResolveRepoFile(string repoRelativePath)
        {
            var relative = repoRelativePath.Replace('/', Path.DirectorySeparatorChar);
            for (var d = AppContext.BaseDirectory; d != null; d = Path.GetDirectoryName(d))
            {
                var candidate = Path.Combine(d, relative);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }
            return null;
        }

        /// <summary>Counts reloaded AnnotationAssertions whose predicate IRI ends with the given fragment.</summary>
        private static int CountAnnotations(string iriFragment)
        {
            return RealOntology.GetOntology().AnnotationAxioms.OfType<OWLAnnotationAssertion>()
                .Count(a => a.AnnotationProperty.GetIRI().ToString().EndsWith(iriFragment));
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) The serializer drop is PARTIAL — rdf:type is dropped, skos:inScheme now SURVIVES.
        //     OWLSharp's OWL2XML round-trip still drops rdf:type (rdf:type==0 among the reloaded
        //     AnnotationAxioms), but as of the committed argumentum.owl (145 AIF-typed, regenerated
        //     2026-07-12 #787) skos:inScheme SURVIVES the round-trip (1408 assertions, one per concept).
        //     Earlier this test asserted both were dropped; that was true of the in-memory build path
        //     but is OBSOLETE for the committed file. The read-path fix (test 2) is unaffected — it
        //     keys on prefLabel/hasTopConcept, which survive regardless. The rdf:type drop stays real
        //     and is left in place (out of scope: write/serialize); only the obsolete inScheme==0
        //     assertion is corrected here. (Anti-greenwashing: inScheme==1408 was verified empirically
        //     by a temporary probe on the reloaded AnnotationAxioms, not by grepping the raw file.)
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LoadedOntology_RdfTypeDropped_InSchemeSurvives_Owl2XmlRoundTrip()
        {
            int rdfType = CountAnnotations("/rdf-syntax-ns#type");
            int inScheme = CountAnnotations("skos/core#inScheme");
            int prefLabel = CountAnnotations("skos/core#prefLabel");

            rdfType.Should().Be(0,
                "OWLSharp's OWL2XML round-trip drops rdf:type annotation assertions (the generator emitted " +
                "one rdf:type=skos:Concept per concept plus one for the ConceptScheme, none survive). This " +
                "drop is REAL and left in place by the read-path fix — the readers now locate concepts via " +
                "surviving prefLabel annotations instead (see test 2), so the drop is benign, not patched.");

            inScheme.Should().BeGreaterThan(0,
                "skos:inScheme SURVIVES the OWL2XML round-trip on the committed argumentum.owl (1408 assertions, " +
                "one per concept) — empirically verified on the reloaded AnnotationAxioms. The earlier assertion " +
                "(inScheme==0) was true of the in-memory build path but OBSOLETE for the committed file. " +
                "Either way the read-path fix keys on prefLabel+hasTopConcept, so this correction is hygiene only.");

            prefLabel.Should().BeGreaterThan(0,
                "contrast: prefLabel (literal-valued) DOES survive the round-trip — the serialization loss is " +
                "predicate-selective, and the real content (concepts' labels) IS present, which is exactly what " +
                "the read-path fix keys on to locate concepts.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) THE FIX: the readers the production validators branch on now RESOLVE concepts and the
        //     scheme on the reloaded file — via the surviving prefLabel (concepts) and hasTopConcept
        //     (scheme) annotations. Before the fix both returned empty (the silent-false-pass precondition).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LoadedOntology_ConceptAndSchemeReaders_ResolveViaSurvivingAnnotations_AfterReadPathFix()
        {
            var concepts = RealOntology.GetResourcesByType(SKOSVocabulary.Concept);
            var schemes = RealOntology.GetResourcesByType(SKOSVocabulary.ConceptScheme);

            concepts.Should().NotBeEmpty().And.HaveCountGreaterThan(1000,
                "FIX: GetResourcesByType(Concept) now resolves the ~1408 concepts on the reloaded ontology by " +
                "falling back to the distinct subjects of skos:prefLabel when the rdf:type scan is empty. " +
                "Before the fix this returned empty (the silent-false-pass precondition); the early-return " +
                "'No concepts → skip → PASS' in the validators is now unreachable.");

            schemes.Should().NotBeEmpty(
                "FIX: GetResourcesByType(ConceptScheme) now resolves the scheme as the subject of the surviving " +
                "skos:hasTopConcept annotation. Before the fix this returned empty (same rdf:type-drop root cause).");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) The reloaded ontology DOES contain real content — 2816 prefLabels, AIF match mappings,
        //     1510 class declarations — and the readers now RESOLVE that content. (Before the fix the
        //     concepts existed but the readers could not locate them via the dropped rdf:type.)
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LoadedOntology_ContainsRealContent_NowResolvableByReaders()
        {
            int prefLabel = CountAnnotations("skos/core#prefLabel");
            int matches = CountAnnotations("#exactMatch")
                + CountAnnotations("#closeMatch")
                + CountAnnotations("#relatedMatch");
            int classDecls = RealOntology.GetOntology().DeclarationAxioms.Count(d => d.Entity is OWLClass);

            prefLabel.Should().BeGreaterThan(1000,
                "the reloaded ontology carries ~2816 prefLabels (one fr+en pair per concept) — real content");
            matches.Should().BeGreaterThan(0,
                "the reloaded ontology carries AIF match mappings (exactMatch/closeMatch/relatedMatch, plus " +
                "broadMatch/narrowMatch) — real content that a LIVE validator now inspects");
            classDecls.Should().BeGreaterThan(1000,
                "~1510 OWL class declarations survive (concepts + scheme + AIF classes)");

            // The readers now resolve the real content (the smoking gun of the fix): a substantial set
            // of distinct concept subjects (~1305) is located via prefLabel. (Not exactly prefLabel/2
            // because the concept URI derives from GetId(TextEn) — fallacy rows whose TextEn collides
            // aggregate onto one subject, so distinct subjects < fr+en label count.)
            int conceptCount = RealOntology.GetResourcesByType(SKOSVocabulary.Concept).Count;
            conceptCount.Should().BeGreaterThan(1000,
                "the reader-resolved distinct concept subjects are substantial on the reloaded ontology — " +
                "the content is no longer invisible to the readers");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) DECISIVE: the PRODUCTION validator (OwlOntologyValidationTests, injected via reflection)
        //     now GENUINELY inspects the concepts and passes on the reloaded ontology — NOT a skip
        //     false-pass. Before the fix it returned true because GetResourcesByType(Concept) was empty
        //     and both methods early-returned; now the concepts resolve, the inspection actually runs,
        //     finds the present prefLabels/definitions and AIF mappings, and passes for the right reason.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public async Task ProdValidator_InspectsConceptsAndGenuinelyPassesOnLoadedOntology()
        {
            // Precondition that makes the 'concepts.Count == 0 → return true' skip guard UNREACHABLE.
            // Asserting it here proves the passes below are GENUINE, not silent false-passes.
            RealOntology.GetResourcesByType(SKOSVocabulary.Concept)
                .Should().NotBeEmpty(
                    "the validators' silent-false-pass early-return triggers only when this is empty; with the " +
                    "read-path fix the concepts resolve, so any PASS from the validators is a genuine inspection " +
                    "pass, not a skip");

            var validator = BuildProdValidator(RealOntology);
            bool annotationsOk = await InvokeValidate(validator, "ValidateMultilingualAnnotations");
            bool aifOk = await InvokeValidate(validator, "ValidateAIFMappings");

            annotationsOk.Should().BeTrue(
                "Genuine pass: with the concepts resolved, ValidateMultilingualAnnotations inspects them and " +
                "finds the present skos:prefLabel + skos:definition annotations — it returns true because the " +
                "annotations ARE there and were inspected, not because the concept list was empty");
            aifOk.Should().BeTrue(
                "Genuine pass: with the concepts resolved, ValidateAIFMappings inspects them and finds AIF match " +
                "mappings (closeMatch/broadMatch/narrowMatch) on at least some concepts — it returns true because " +
                "the mappings were inspected and present, not because the concept list was empty");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) TRANSVERSE CROSS-LINKS + AIF ATTACK TYPING — counts asserted EXACTLY against the corpus.
        //     Proves the second-pass emitter in OwlGeneratorConfig.CreateOwlDocument writes the
        //     crossLink assertions (predatesOn/denounces/leverages/allows/opposes/inverts/mirrors/
        //     isRelatedTo) resolved from the crossLink_* CSV columns, plus the aifAttackType literal and
        //     the aifAttackedNode (RA/I/CA-node) resource for each AIF-typed leaf — and that, like the
        //     SKOS *Match resource-valued annotations of test 3, they SURVIVE the OWL2XML round-trip.
        //
        //     ⚠ STRENGTHENED 2026-08-25. This test previously asserted BeGreaterThan(0) on every count,
        //     and its own comment claimed it guarded against "these counts fall to 0". It did — and
        //     nothing else: a decay from 1230 links to 3 passed green, as did an AIF layer collapsing
        //     from 145 typed nodes to 1. A threshold of 0 is not a guard, it is a liveness check.
        //
        //     The expectation is now DERIVED from Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv at
        //     test time, reproducing the emitter's own semantics (split on ';', trim, skip unresolvable
        //     paths, skip self-links), times 2 for the four Symmetric verbs which are emitted in both
        //     directions. Measured on the committed pair: 1230 corpus links → 1977 assertions
        //     (predatesOn 13, denounces 2, leverages 402, allows 66 at ×1; opposes 25→50, inverts 41→82,
        //     mirrors 360→720, isRelatedTo 321→642 at ×2), AIF 145/145. Every ratio is exactly 1.00 or
        //     2.00, which is what makes the predicate derivable rather than tabulated.
        //
        //     Two DISTINCT failure modes are asserted separately so the message names the right cause:
        //       • count mismatch  → the CSV moved without a --generate-owl regeneration (stale artefact)
        //       • raw ≠ resolvable → a crossLink cell points at a path that does not exist, or at itself
        //                            (corpus defect; the emitter drops it silently, so nothing else sees it)
        //     Loads the same committed docs/ontology/argumentum.owl as the tests above.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LoadedOntology_CrossLinkAndAifCounts_MatchTheCorpusExactly()
        {
            var corpus = Corpus;

            foreach (var (verb, _) in CrossLinkVerbs)
            {
                corpus.Resolvable[verb].Should().Be(corpus.Raw[verb],
                    "every target listed in crossLink_{0} must resolve to an existing taxonomy path and must not " +
                    "point at its own node — the emitter silently drops both. {1} dangling target(s) found. " +
                    "A dangling link is a CORPUS defect, not a stale-ontology symptom, which is why it is " +
                    "asserted BEFORE the count below: a dangling target lowers Resolvable, so the count would " +
                    "otherwise diverge first and blame a stale ontology for a corpus defect.",
                    Capitalize(verb), corpus.Raw[verb] - corpus.Resolvable[verb]);
            }

            foreach (var (verb, symmetric) in CrossLinkVerbs)
            {
                var expected = corpus.Resolvable[verb] * (symmetric ? 2 : 1);

                CountAnnotations("#" + verb).Should().Be(expected,
                    "the emitter writes one '{0}' assertion per resolvable target of crossLink_{1}{2}. " +
                    "Corpus says {3} resolvable target(s) => {4} assertion(s) expected. A mismatch means the " +
                    "taxonomy CSV was edited without regenerating docs/ontology/argumentum.owl (--generate-owl), " +
                    "or the CSV->OWL wiring changed. The count is EXACT on purpose: a >0 threshold stays green " +
                    "while 1230 links decay to 3.",
                    verb, Capitalize(verb),
                    symmetric ? " plus a second assertion in the reverse direction (Symmetric=true)" : " (Symmetric=false)",
                    corpus.Resolvable[verb], expected);
            }

            CountAnnotations("#aifAttackType").Should().Be(corpus.AifAttackType,
                "one aifAttackType literal (undercut/undermine/rebut) per fallacy carrying AIF_attackType.");

            CountAnnotations("#aifAttackedNode").Should().Be(corpus.AifAttackedNode,
                "each AIF-typed fallacy links to its AIF node (RA-node/I-node/CA-node) via aifAttackedNode — " +
                "the formal ASPIC+/AIF attachment (undercut->RA, undermine->I, rebut->CA).");
        }

        // ── Corpus-derived expectations ─────────────────────────────────────────────────

        /// <summary>
        /// Mirrors the emitter table in <c>OwlGeneratorConfig.CreateOwlDocument</c> (<c>crossLinkVerbs</c>).
        /// Flipping a Symmetric flag there without flipping it here is a deliberate red: the expected
        /// assertion count doubles or halves and the failure message says which verb moved.
        /// </summary>
        private static readonly (string Verb, bool Symmetric)[] CrossLinkVerbs =
        {
            ("predatesOn",  false),
            ("denounces",   false),
            ("leverages",   false),
            ("allows",      false),
            ("opposes",     true),
            ("inverts",     true),
            ("mirrors",     true),
            ("isRelatedTo", true),
        };

        private sealed class CorpusCounts
        {
            public Dictionary<string, int> Raw { get; } = new Dictionary<string, int>();
            public Dictionary<string, int> Resolvable { get; } = new Dictionary<string, int>();
            public int AifAttackType { get; set; }
            public int AifAttackedNode { get; set; }
        }

        private static readonly Lazy<CorpusCounts> _corpus = new Lazy<CorpusCounts>(ReadCorpusCounts);

        private static CorpusCounts Corpus => _corpus.Value;

        private static string Capitalize(string verb) => char.ToUpperInvariant(verb[0]) + verb.Substring(1);

        /// <summary>
        /// Recomputes from the taxonomy CSV exactly what the emitter would write: same split on ';',
        /// same trim, same skip of unresolvable paths and self-links. Divergence between this and the
        /// emitter is itself worth a red.
        /// </summary>
        private static CorpusCounts ReadCorpusCounts()
        {
            var csvPath = ResolveRepoFile("Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv");
            if (csvPath == null)
            {
                throw new FileNotFoundException(
                    "Taxonomy CSV not found walking up from " + AppContext.BaseDirectory +
                    ". It is the source the committed ontology is generated from, and the only way to know how " +
                    "many crossLink assertions the OWL should carry. Failing loud rather than skipping.");
            }

            var rows = new List<string[]>();
            string[] header;
            using (var reader = new StreamReader(csvPath, new UTF8Encoding(true)))
            using (var csv = new CsvReader(reader,
                       new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
            {
                csv.Read();
                csv.ReadHeader();
                header = csv.HeaderRecord ?? Array.Empty<string>();
                while (csv.Read())
                {
                    var row = new string[csv.Parser.Count];
                    for (var i = 0; i < csv.Parser.Count; i++)
                    {
                        row[i] = csv.GetField(i) ?? string.Empty;
                    }
                    rows.Add(row);
                }
            }

            int Col(string name)
            {
                var i = Array.FindIndex(header, h => string.Equals(h, name, StringComparison.Ordinal));
                if (i < 0)
                {
                    throw new InvalidOperationException(
                        "Column '" + name + "' absent from the taxonomy CSV. Renaming a column without updating " +
                        "the CsvHelper ClassMap breaks the pipeline silently; this test refuses to guess.");
                }
                return i;
            }

            var pathCol = Col("path");
            var knownPaths = new HashSet<string>(
                rows.Where(r => pathCol < r.Length).Select(r => r[pathCol].Trim()),
                StringComparer.Ordinal);

            var counts = new CorpusCounts();
            foreach (var (verb, _) in CrossLinkVerbs)
            {
                var col = Col("crossLink_" + Capitalize(verb));
                var raw = 0;
                var resolvable = 0;
                foreach (var row in rows)
                {
                    if (col >= row.Length || string.IsNullOrWhiteSpace(row[col]))
                    {
                        continue;
                    }
                    var self = pathCol < row.Length ? row[pathCol].Trim() : string.Empty;
                    foreach (var target in row[col].Split(';').Select(x => x.Trim()).Where(x => x.Length > 0))
                    {
                        raw++;
                        if (knownPaths.Contains(target) && !string.Equals(target, self, StringComparison.Ordinal))
                        {
                            resolvable++;
                        }
                    }
                }
                counts.Raw[verb] = raw;
                counts.Resolvable[verb] = resolvable;
            }

            var attackTypeCol = Col("AIF_attackType");
            var attackedNodeCol = Col("AIF_attackedNode");
            counts.AifAttackType = rows.Count(r => attackTypeCol < r.Length && !string.IsNullOrWhiteSpace(r[attackTypeCol]));
            counts.AifAttackedNode = rows.Count(r => attackedNodeCol < r.Length && !string.IsNullOrWhiteSpace(r[attackedNodeCol]));

            return counts;
        }

        private static object BuildProdValidator(OwlAdapter ontology)
        {
            // OwlOntologyValidationTests is a PUBLIC class in the PRODUCTION project (namespace
            // Argumentum.AssetConverter.Tests). Its public constructor takes an AssetConverterConfig
            // (for OwlValidatorConfig); the ontology is otherwise loaded from disk via LoadOntology —
            // we bypass that by setting the private _ontology field, mirroring a post-fix prod run.
            var config = new AssetConverterConfig();
            var validator = new OwlOntologyValidationTests(config);
            var field = typeof(OwlOntologyValidationTests).GetField("_ontology",
                BindingFlags.NonPublic | BindingFlags.Instance);
            field.Should().NotBeNull("the production validator must carry a private _ontology field");
            field!.SetValue(validator, ontology);
            return validator;
        }

        private static Task<bool> InvokeValidate(object validator, string methodName)
        {
            var method = typeof(OwlOntologyValidationTests).GetMethod(methodName,
                BindingFlags.Public | BindingFlags.Instance);
            method.Should().NotBeNull($"the production validator must expose a {methodName} method");
            return (Task<bool>)method!.Invoke(validator, null)!;
        }
    }
}
