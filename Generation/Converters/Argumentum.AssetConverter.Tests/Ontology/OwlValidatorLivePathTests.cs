using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using RDFSharp.Model;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// Proves the production OWL validation path is LIVE after the OwlAdapter reader fix (PR #481).
    ///
    /// NEW additive file (dispatch `4rkh1s` secondaire). The production validation module
    /// <c>Tests/OwlOntologyValidationTests.cs</c> (NOT an xUnit suite — a runtime validator invoked by
    /// <c>OwlValidatorConfig.Apply</c>) had a **silent false-pass**: <c>ValidateMultilingualAnnotations</c>
    /// and <c>ValidateAIFMappings</c> both early-return <c>true</c> ("No concepts to validate — skipping")
    /// when <c>_ontology.GetResourcesByType(SKOSVocabulary.Concept)</c> returned empty. Before the fix,
    /// that reader ALWAYS returned empty (RDFResource type-mismatch), so the validators reported PASS
    /// regardless of whether any annotation or AIF mapping actually existed.
    ///
    /// These tests drive the REAL production validator (via reflection — the validator's ontology field
    /// and methods are private, and it has no public injection seam). They prove:
    ///  (1) with annotated concepts, the validator no longer early-returns — it inspects the concepts
    ///      and reports the genuine PASS;
    ///  (2) with UNannotated concepts, the validator now FAILS (the annotation check actually ran and
    ///      found the missing labels/definitions) — the dead silent false-pass is gone.
    /// This is the exact delta the #481 fix delivers to the production validation path.
    ///
    /// Deterministic, key-free, release-independent. No existing file modified.
    /// </summary>
    public class OwlValidatorLivePathTests
    {
        private const string Ns = "http://argumentum.test/onto#";

        private static OwlAdapter NewAdapter() => new OwlAdapter(Ns);
        private static RDFResource Res(string local) => new RDFResource(Ns + local);
        private static RDFPlainLiteral Lit(string value) => new RDFPlainLiteral(value);

        /// <summary>
        /// Builds a fully-injected production <see cref="OwlOntologyValidationTests"/> instance whose
        /// private <c>_ontology</c> field points at <paramref name="ontology"/>. The validator's public
        /// constructor requires an <see cref="AssetConverterConfig"/> (for the OwlValidatorConfig), but
        /// the ontology is otherwise loaded from disk via <c>LoadOntology</c> — we bypass that by setting
        /// the private field directly, mirroring what a post-fix production run would hold in memory.
        /// </summary>
        private static object BuildValidator(OwlAdapter ontology)
        {
            var config = new AssetConverterConfig();
            var validator = new OwlOntologyValidationTests(config);
            var field = typeof(OwlOntologyValidationTests).GetField("_ontology",
                BindingFlags.NonPublic | BindingFlags.Instance);
            field.Should().NotBeNull("the validator must carry a private _ontology field");
            field!.SetValue(validator, ontology);
            return validator;
        }

        private static Task<bool> InvokeValidate(object validator, string methodName)
        {
            // ValidateMultilingualAnnotations / ValidateAIFMappings are PUBLIC methods on the
            // production validator (OwlOntologyValidationTests is a public class in the production
            // project). Bind Public|Instance to locate them.
            var method = typeof(OwlOntologyValidationTests).GetMethod(methodName,
                BindingFlags.Public | BindingFlags.Instance);
            method.Should().NotBeNull($"the validator must expose a {methodName} validation method");
            var task = (Task<bool>)method!.Invoke(validator, null)!;
            return task;
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) With annotated concepts, the validator no longer early-returns: it inspects
        //     the concepts and reports a genuine PASS. Before the fix this returned true for the
        //     wrong reason (empty concepts → skip). After the fix it returns true because the
        //     annotations are actually present.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public async Task Annotation_Validator_Inspects_Concepts_And_Passes_When_Annotated()
        {
            var adapter = NewAdapter();
            var scheme = Res("Scheme");
            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareTopConcept(Res("Top"), scheme);
            // Two fully-annotated concepts under the scheme.
            foreach (var name in new[] { "AdHominem", "StrawMan" })
            {
                var concept = Res(name);
                adapter.DeclareConcept(concept, scheme);
                adapter.AnnotateConceptPreferredLabel(concept, Lit(name + " label"));
                adapter.DocumentConcept(concept, SKOSDocumentationTypes.Definition, Lit(name + " definition"));
            }

            var validator = BuildValidator(adapter);
            var valid = await InvokeValidate(validator, "ValidateMultilingualAnnotations");

            valid.Should().BeTrue(
                "with 2 fully-annotated concepts the annotation validator must PASS — and crucially " +
                "it must PASS because the annotations ARE present, not because of the 'no concepts → skip' early-return " +
                "(the early-return was the silent false-pass before the #481 fix)");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) The decisive proof: with UNannotated concepts, the validator now FAILS. Before the
        //     fix, GetResourcesByType(Concept) returned empty → early-return true (PASS) even though
        //     every concept lacked a label/definition. After the fix the concepts resolve, the check
        //     actually runs, finds the missing annotations, and FAILS. This is the dead silent-false-pass.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public async Task Annotation_Validator_Fails_When_Concepts_Lack_Annotations_After_Fix()
        {
            var adapter = NewAdapter();
            var scheme = Res("Scheme");
            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareTopConcept(Res("Top"), scheme);
            // Concepts declared but with NO prefLabel / NO definition.
            foreach (var name in new[] { "C1", "C2", "C3", "C4" })
            {
                adapter.DeclareConcept(Res(name), scheme);
            }

            var validator = BuildValidator(adapter);
            var valid = await InvokeValidate(validator, "ValidateMultilingualAnnotations");

            valid.Should().BeFalse(
                "4 declared concepts with zero annotations must FAIL the annotation check after the fix — " +
                "before the fix this returned true (silent false-pass: 'no concepts → skip') because " +
                "GetResourcesByType(Concept) was empty due to the RDFResource type-mismatch bug");
        }

        [Fact]
        public async Task Aif_Validator_Fails_When_Concepts_Have_No_Match_Mappings_After_Fix()
        {
            // Same silent-false-pass for the AIF mapping validator: concepts declared but no
            // exactMatch/closeMatch/relatedMatch. Before the fix → empty concepts → skip → PASS.
            // After the fix → concepts resolve → check runs → finds no mappings → FAIL.
            var adapter = NewAdapter();
            var scheme = Res("Scheme");
            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareTopConcept(Res("Top"), scheme);
            adapter.DeclareConcept(Res("LonelyConcept"), scheme); // no match mappings

            var validator = BuildValidator(adapter);
            var valid = await InvokeValidate(validator, "ValidateAIFMappings");

            valid.Should().BeFalse(
                "a concept with no AIF match mappings must FAIL the AIF validator after the fix — " +
                "before the fix the empty GetResourcesByType made this a silent false-pass");
        }

        [Fact]
        public async Task Aif_Validator_Passes_When_Concepts_Carry_ExactMatch_After_Fix()
        {
            var adapter = NewAdapter();
            var scheme = Res("Scheme");
            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareTopConcept(Res("Top"), scheme);
            var en = Res("ConceptEN");
            var fr = Res("ConceptFR");
            adapter.DeclareConcept(en, scheme);
            adapter.DeclareConcept(fr, scheme);
            adapter.DeclareExactMatchConcepts(en, fr); // cross-language exactMatch mapping

            var validator = BuildValidator(adapter);
            var valid = await InvokeValidate(validator, "ValidateAIFMappings");

            valid.Should().BeTrue(
                "concepts carrying an exactMatch mapping must PASS the AIF validator after the fix " +
                "(genuine pass: the mapping is present and resolved, not a 'no concepts → skip' false-pass)");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) Contract guard: the GetResourcesByType(Concept) reader — the exact predicate the
        //     validators branch on — resolves non-empty when concepts are declared. This is the
        //     single line that decided silent-false-pass vs live-check. Pinned in isolation so a
        //     future regression to the empty-return bug is caught at the unit level too.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void The_Reader_The_Validators_Branch_On_Resolves_Concepts_NonEmpty()
        {
            // ValidateMultilingualAnnotations / ValidateAIFMappings both start with:
            //   var concepts = _ontology.GetResourcesByType(SKOSVocabulary.Concept);
            //   if (concepts.Count == 0) { return true; }  // <-- the silent false-pass
            // Pin that this predicate is non-empty post-fix → the early-return is unreachable.
            var adapter = NewAdapter();
            var scheme = Res("Scheme");
            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareConcept(Res("C1"), scheme);
            adapter.DeclareConcept(Res("C2"), scheme);

            adapter.GetResourcesByType(SKOSVocabulary.Concept)
                .Should().HaveCount(2,
                    "the validator's silent-false-pass early-return triggers exactly when this is empty; " +
                    "after the #481 fix it must resolve the declared concepts");
        }
    }
}
