using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
    /// drops <c>rdf:type</c> from the reloaded annotation stream (rdf:type==0, skos:inScheme==0 among
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

        private static string ResolveRepoFile(string repoRelativePath)
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
        // (1) The serializer drop is REAL and UNFIXED BY DESIGN (read-path-only fix).
        //     rdf:type==0 and skos:inScheme==0 among the reloaded AnnotationAxioms — OWLSharp's
        //     OWL2XML round-trip drops/absorbs both. This stays true after the fix: the readers no
        //     longer depend on them, but the drop itself is not patched (out of scope: write/serialize).
        //     Contrast: prefLabel (literal-valued) survives — the loss is predicate-selective.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LoadedOntology_RdfTypeAndInScheme_DroppedByOwl2XmlRoundTrip()
        {
            int rdfType = CountAnnotations("/rdf-syntax-ns#type");
            int inScheme = CountAnnotations("skos/core#inScheme");
            int prefLabel = CountAnnotations("skos/core#prefLabel");

            rdfType.Should().Be(0,
                "OWLSharp's OWL2XML round-trip drops rdf:type annotation assertions (the generator emitted " +
                "one rdf:type=skos:Concept per concept plus one for the ConceptScheme, none survive). This " +
                "drop is REAL and left in place by the read-path fix — the readers now locate concepts via " +
                "surviving prefLabel annotations instead (see test 2), so the drop is benign, not patched.");

            inScheme.Should().Be(0,
                "skos:inScheme is also absent from the reloaded AnnotationAxioms (absorbed elsewhere by the " +
                "reader) — a read-path fix cannot rely on it either; the fix uses prefLabel + hasTopConcept.");

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
