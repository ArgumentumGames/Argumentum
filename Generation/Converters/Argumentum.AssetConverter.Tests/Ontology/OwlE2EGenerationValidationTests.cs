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
    /// End-to-end OWL proof for #133 — SURFACES A SECOND SILENT-FALSE-PASS BUG in the production
    /// validation path that the #480→#481→#482 lane missed.
    ///
    /// <see cref="OwlValidatorLivePathTests"/> (#482) proved with SYNTHETIC in-memory adapters that the
    /// production validator no longer silent-false-passes after the OwlAdapter reader fix (#481). But
    /// that lane only exercised the IN-MEMORY path. This class loads the REAL generation output
    /// (<c>docs/ontology/argumentum.owl</c>, ~5 MB, produced by <c>OwlDocumentConfig.CreateOwlDocument</c>)
    /// and proves the readers are STILL BLIND on a reloaded file — so the production validator
    /// (<c>OwlOntologyValidationTests.LoadOntology</c> → <c>OwlAdapter.FromFile</c> → validate) STILL
    /// silent-false-passes in the real prod path, for a DIFFERENT root cause than the #480 type-mismatch.
    ///
    /// ROOT CAUSE (measured on the loaded file): OWLSharp's OWL2XML serializer DROPS the
    /// <c>rdf:type</c> and <c>skos:inScheme</c> annotation assertions during serialization — neither
    /// survives the round-trip (rdf:type == 0, inScheme == 0 after reload). The OwlAdapter readers find
    /// concepts/schemes by scanning <c>AnnotationAxioms</c> for <c>rdf:type</c>, so on any LOADED file
    /// they return empty. <c>ValidateMultilingualAnnotations</c> / <c>ValidateAIFMappings</c> then hit
    /// their <c>if (concepts.Count == 0) return true;</c> guard → report PASS without inspecting anything.
    /// Meanwhile the real content IS present (2816 prefLabels, 10 AIF matches, 1510 class declarations) —
    /// the validator just cannot see it.
    ///
    /// These tests are GREEN by pinning the CURRENT BROKEN behavior (characterization). When the fix
    /// lands (readers must locate concepts via surviving annotations — prefLabel/definition/example
    /// subjects, or filtered DeclarationAxioms — NOT rdf:type/inScheme), these assertions flip to the
    /// honest "detection works" form. This is a coordinator-scope fix (prod behavior + release-gate
    /// implications), so the worker surfaces it rather than shipping a unilateral prod change.
    ///
    /// Deterministic, key-free, release-independent. Loads one real file (lazy, shared across the class).
    /// NEW additive file (dispatch #133 primaire) — no existing file modified.
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
        // (1) ROOT CAUSE: rdf:type and skos:inScheme annotations are DROPPED by OWLSharp's
        //     OWL2XML round-trip. rdf:type==0 on the reloaded ontology (the generator emitted one
        //     rdf:type=skos:Concept per concept + one for the scheme). inScheme==0 likewise. Contrast:
        //     prefLabel (literal-valued) survives — so the loss is predicate-selective, not total.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LoadedOntology_RdfTypeAndInScheme_DroppedByOwl2XmlRoundTrip()
        {
            int rdfType = CountAnnotations("/rdf-syntax-ns#type");
            int inScheme = CountAnnotations("skos/core#inScheme");
            int prefLabel = CountAnnotations("skos/core#prefLabel");

            rdfType.Should().Be(0,
                "BUG: OWLSharp's OWL2XML serializer drops rdf:type annotation assertions — the generator " +
                "emitted one rdf:type=skos:Concept per concept (~1400) plus one for the ConceptScheme, but " +
                "ZERO survive the round-trip. This is the root cause of the loaded-file reader blindness.");

            inScheme.Should().Be(0,
                "BUG: skos:inScheme (resource-valued, emitted per concept by DeclareConcept) is ALSO dropped " +
                "by the round-trip — so a fallback fix cannot rely on inScheme either.");

            prefLabel.Should().BeGreaterThan(0,
                "contrast: prefLabel (literal-valued) DOES survive the round-trip — the serialization loss is " +
                "predicate-selective, and the real content (concepts' labels) IS present in the reloaded ontology");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) CONSEQUENCE: the readers the production validators branch on return EMPTY on the
        //     reloaded file. GetResourcesByType(Concept) scans AnnotationAxioms for rdf:type, which is
        //     now absent (test 1). This is the exact precondition for the silent false-pass.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LoadedOntology_ConceptAndSchemeReaders_ReturnEmpty_BugPinned()
        {
            var concepts = RealOntology.GetResourcesByType(SKOSVocabulary.Concept);
            var schemes = RealOntology.GetResourcesByType(SKOSVocabulary.ConceptScheme);

            concepts.Should().BeEmpty(
                "BUG PINNED: on the reloaded real ontology GetResourcesByType(Concept) returns empty because " +
                "rdf:type is absent (test 1). In-memory (post-#481) this resolves concepts, but on a LOADED " +
                "file the reader is blind — so the production validators see zero concepts.");

            schemes.Should().BeEmpty(
                "same root cause for ConceptScheme: rdf:type=skos:ConceptScheme was dropped on round-trip");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) SMOKING GUN: the reloaded ontology DOES contain real content — 2816 prefLabels,
        //     10 AIF match mappings, 1510 class declarations. The concepts exist; the readers simply
        //     cannot locate them via the dropped rdf:type. This proves the empty reader is a reader
        //     defect, not a data defect.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void LoadedOntology_ContainsRealContent_ReadersCannotSee()
        {
            int prefLabel = CountAnnotations("skos/core#prefLabel");
            int matches = CountAnnotations("#exactMatch")
                + CountAnnotations("#closeMatch")
                + CountAnnotations("#relatedMatch");
            int classDecls = RealOntology.GetOntology().DeclarationAxioms.Count(d => d.Entity is OWLClass);

            prefLabel.Should().BeGreaterThan(1000,
                "the reloaded ontology carries ~2816 prefLabels (one fr+en pair per concept) — real content " +
                "is present");
            matches.Should().BeGreaterThan(0,
                "the reloaded ontology carries AIF match mappings (exactMatch/closeMatch/relatedMatch) — " +
                "real content is present, yet ValidateAIFMappings will still skip because concepts resolves empty");
            classDecls.Should().BeGreaterThan(1000,
                "~1510 OWL class declarations survive (concepts + scheme + AIF classes) — an alternative fix " +
                "could locate concepts via DeclarationAxioms rather than the dropped rdf:type");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) DECISIVE PROOF: the PRODUCTION validator (OwlOntologyValidationTests, injected via
        //     reflection) returns TRUE for both annotation and AIF validation on the reloaded ontology —
        //     NOT because the content is valid, but because the concept reader is empty and the methods
        //     early-return true ("No concepts to validate — skipping"). The silent false-pass is STILL
        //     ACTIVE in the production load-and-validate path, contradicting #133's premise.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public async Task ProdValidator_SilentFalsePass_StillActiveOnLoadedOntology()
        {
            var validator = BuildProdValidator(RealOntology);
            bool annotationsOk = await InvokeValidate(validator, "ValidateMultilingualAnnotations");
            bool aifOk = await InvokeValidate(validator, "ValidateAIFMappings");

            // These return TRUE — but that is the SILENT FALSE-PASS, not a genuine pass: the concept
            // reader is empty (tests 1+2), so both validators hit `if (concepts.Count == 0) return true;`
            // and skip inspection entirely. The ontology demonstrably contains prefLabels + AIF matches
            // (test 3) that a LIVE validator would inspect. Pinning this FALSE pass until the reader fix.
            annotationsOk.Should().BeTrue(
                "ValidateMultilingualAnnotations returns TRUE on the reloaded ontology — the silent false-pass " +
                "is still alive in production. It returns true because GetResourcesByType(Concept) is empty " +
                "(test 2) so the method early-returns, NOT because the (present) annotations were inspected");
            aifOk.Should().BeTrue(
                "ValidateAIFMappings returns TRUE on the reloaded ontology for the same reason — silent " +
                "false-pass. #133's 'confidence restored' premise does not hold for the prod load path");
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
