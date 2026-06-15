using System;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using RDFSharp.Model;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// Regression / characterization tests for <see cref="OwlAdapter"/> — dispatch #204 (primaire, cont.).
    ///
    /// NEW additive file. The Ontology subsystem had zero xUnit coverage prior to this (the
    /// production-side <c>Tests/OwlOntologyValidationTests.cs</c> module is NOT an xUnit suite).
    /// CLAUDE.md flags the SKOS/OWL layer as fragile: <c>OwlAdapter</c> bypasses the broken
    /// <c>SKOSHelper</c> extension methods in OWLSharp and self-retrieves concepts via fallback
    /// annotation scanners (<c>GetAnnotationSubjects</c> / <c>GetResourceAnnotations</c> /
    /// <c>GetLiteralAnnotations</c>).
    ///
    /// ⚠️ These tests surfaced a REAL BUG (not greenwashed). Every fallback reader compares
    /// <c>a.ValueIRI.Equals(value.URI)</c> / <c>a.SubjectIRI.Equals(subject.URI)</c> where
    /// <c>.URI</c> is a <b>string</b> and <c>ValueIRI</c>/<c>SubjectIRI</c> are
    /// <c>RDFResource</c>. <c>RDFResource.Equals(string)</c> returns <b>false by type-mismatch</b>,
    /// so every read returns empty. See <see cref="Diag_RDFResource_Equals_String_Is_False_By_Type_Mismatch"/>.
    /// This is the root cause behind the production validation module silently reporting
    /// "no concepts → skip → PASS" on annotation/AIF checks. Reported as [BUG] on the dashboard.
    ///
    /// Tests are split into: (A) characterization of the bug (pinned, documents current behavior),
    /// (B) the correct comparison semantics (documents the fix), (C) write-path + serialization
    /// (these DO work). No fix is applied — the file only pins observed behavior so a future fix
    /// flips the [BUG] tests red→green and the fix author has a regression suite ready.
    ///
    /// Deterministic, key-free, release-independent. No existing file modified. Baseline additive.
    /// </summary>
    public class OwlAdapterRegressionTests
    {
        private const string Ns = "http://argumentum.test/onto#";

        private static OwlAdapter NewAdapter() => new OwlAdapter(Ns);

        private static RDFResource Res(string local) => new RDFResource(Ns + local);

        private static RDFPlainLiteral Lit(string value) => new RDFPlainLiteral(value);

        // ─────────────────────────────────────────────────────────────────────────────
        // (B) Root-cause probe — the comparison semantics the readers depend on.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Diag_RDFResource_Equals_String_Is_False_By_Type_Mismatch()
        {
            // ROOT CAUSE of the [BUG]: RDFResource.Equals(object) requires the argument to BE an
            // RDFResource with the same URI. Passing a string (which is what `.URI` yields) returns
            // false on every comparison. The OwlAdapter fallback readers all do
            // `a.ValueIRI.Equals(value.URI)` and `a.SubjectIRI.Equals(subject.URI)` — so they never
            // match. This test pins the RDFSharp semantics so the root cause is undeniable.
            var r = new RDFResource(Ns + "X");
            var sameUriString = Ns + "X";

            r.Equals(sameUriString).Should().BeFalse(
                "RDFResource.Equals(string) is false by type-mismatch — the read-path bug");
            r.Equals(new RDFResource(sameUriString)).Should().BeTrue(
                "RDFResource.Equals(RDFResource) is true when URIs match — what the readers SHOULD compare against");
            (r.ToString() == sameUriString).Should().BeTrue(
                "ToString() yields the plain URI string — the correct string basis for comparison");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (A) [BUG] characterization — readers return empty despite writes succeeding.
        //     These pin the BROKEN behavior. When OwlAdapter is fixed, these flip to red and
        //     become proper round-trip assertions (remove the .Be(0) and assert the populated set).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void BUG_GetConcepts_Returns_Empty_Despite_Declared_Concepts()
        {
            // Write 3 concepts; the reader should return 3 but returns 0 (comparison bug).
            var adapter = NewAdapter();
            var scheme = Res("Scheme");
            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareConcept(Res("C1"), scheme);
            adapter.DeclareConcept(Res("C2"), scheme);
            adapter.DeclareConcept(Res("C3"), scheme);

            var concepts = adapter.GetConcepts();
            concepts.Should().BeEmpty(
                "[BUG] GetConcepts returns empty because the fallback scanner compares RDFResource.Equals(string)");
        }

        [Fact]
        public void BUG_GetResourcesByType_Concept_Returns_Empty()
        {
            // Same root cause, different reader. ValidateOwlOntologyStructure relies on this.
            var adapter = NewAdapter();
            var scheme = Res("Scheme");
            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareConcept(Res("C1"), scheme);

            adapter.GetResourcesByType(SKOSVocabulary.Concept).Should().BeEmpty("[BUG] RDFResource.Equals(string)");
            adapter.GetResourcesByType(SKOSVocabulary.ConceptScheme).Should().BeEmpty("[BUG] RDFResource.Equals(string)");
        }

        [Fact]
        public void BUG_GetTopConcepts_Returns_Empty_Despite_Declared()
        {
            var adapter = NewAdapter();
            adapter.DeclareTopConcept(Res("Top1"), Res("Scheme"));
            adapter.DeclareTopConcept(Res("Top2"), Res("Scheme"));

            adapter.GetTopConcepts().Should().BeEmpty("[BUG] GetAnnotationObjects compares RDFResource.Equals(string)");
        }

        [Fact]
        public void BUG_HasAnnotation_Returns_False_Despite_Annotation_Present()
        {
            var adapter = NewAdapter();
            var scheme = Res("Scheme");
            adapter.DeclareConceptScheme(scheme);

            // The annotation IS written (see write-path test below), but HasAnnotation can't find it.
            adapter.HasAnnotation(scheme, RDFVocabulary.RDF.TYPE, SKOSVocabulary.ConceptScheme)
                .Should().BeFalse("[BUG] HasAnnotation compares ValueIRI.Equals(value.URI string) → false");
        }

        [Fact]
        public void BUG_CheckIsNarrowerConcept_Returns_False_Despite_Declared()
        {
            var adapter = NewAdapter();
            var parent = Res("Fallacies");
            var child = Res("AdHominem");
            adapter.DeclareNarrowerConcepts(parent, child);

            // CheckIsNarrowerConcept has a try/except fallback that ALSO uses .Equals(string.URI),
            // so even the fallback fails.
            adapter.CheckIsNarrowerConcept(child, parent)
                .Should().BeFalse("[BUG] fallback compares RDFResource.Equals(string) → false");
        }

        [Fact]
        public void BUG_GetConceptPreferredLabels_Returns_Empty_Despite_Label_Set()
        {
            var adapter = NewAdapter();
            var concept = Res("C");
            adapter.DeclareConcept(concept, Res("Scheme"));
            adapter.AnnotateConceptPreferredLabel(concept, Lit("Ad Hominem"));

            adapter.GetConceptPreferredLabels(concept).Should().BeEmpty(
                "[BUG] GetLiteralAnnotations compares AnnotationProperty.GetIRI().Equals(property.URI string)");
        }

        [Fact]
        public void BUG_GetConceptDocumentation_Returns_Empty_Despite_Documented()
        {
            var adapter = NewAdapter();
            var concept = Res("C");
            adapter.DeclareConcept(concept, Res("Scheme"));
            adapter.DocumentConcept(concept, SKOSDocumentationTypes.Definition, Lit("A fallacy..."));

            adapter.GetConceptDocumentation(concept, SKOSDocumentationTypes.Definition)
                .Should().BeEmpty("[BUG] same RDFResource.Equals(string) root cause");
        }

        [Fact]
        public void BUG_GetExactMatchConcepts_Returns_Empty_Despite_Declared()
        {
            var adapter = NewAdapter();
            var c1 = Res("EN");
            var c2 = Res("FR");
            adapter.DeclareExactMatchConcepts(c1, c2);

            adapter.GetExactMatchConcepts(c1).Should().BeEmpty("[BUG] RDFResource.Equals(string)");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (C) Write-path + serialization — these WORK (the ontology is correctly built in
        //     memory; only the self-retrieval readers are broken). Pinned so a fix doesn't
        //     regress the write side.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Constructor_Produces_An_Adapter_With_The_Declared_Namespace_Uri()
        {
            var adapter = NewAdapter();
            adapter.Uri.ToString().Should().Be(Ns);
            adapter.GetOntology().Should().NotBeNull();
        }

        [Fact]
        public void DeclareConcept_Appends_AnnotationAxioms_To_The_Ontology()
        {
            // The WRITE side is correct: DeclareConcept adds real annotation axioms to the
            // ontology (verifiable by inspecting the underlying graph directly, bypassing the
            // broken reader). This is why ToFileAsync produces a valid ontology despite the
            // readers being broken.
            var adapter = NewAdapter();
            var scheme = Res("Scheme");
            var concept = Res("C1");

            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareConcept(concept, scheme);

            var onto = adapter.GetOntology();
            onto.AnnotationAxioms.Should().NotBeEmpty("DeclareConcept writes annotation axioms");

            // Verify by ToString() comparison (the correct basis), not the broken reader.
            var hasConceptType = onto.AnnotationAxioms.OfType<OWLSharp.Ontology.OWLAnnotationAssertion>()
                .Any(a => a.AnnotationProperty.GetIRI().ToString() == RDFVocabulary.RDF.TYPE.URI.ToString()
                       && a.SubjectIRI.ToString() == concept.URI.ToString()
                       && a.ValueIRI?.ToString() == SKOSVocabulary.Concept.URI.ToString());
            hasConceptType.Should().BeTrue(
                "the concept's rdf:type=skos:Concept axiom IS present in the graph (write works; reader is broken)");
        }

        [Fact]
        public void DeclareNarrowerConcepts_Appends_Both_Directional_Axioms()
        {
            var adapter = NewAdapter();
            var parent = Res("P");
            var child = Res("C");
            adapter.DeclareNarrowerConcepts(parent, child);

            var onto = adapter.GetOntology();
            var hasNarrower = onto.AnnotationAxioms.OfType<OWLSharp.Ontology.OWLAnnotationAssertion>()
                .Any(a => a.AnnotationProperty.GetIRI().ToString() == SKOSVocabulary.Narrower.URI.ToString()
                       && a.SubjectIRI.ToString() == parent.URI.ToString()
                       && a.ValueIRI?.ToString() == child.URI.ToString());
            var hasBroader = onto.AnnotationAxioms.OfType<OWLSharp.Ontology.OWLAnnotationAssertion>()
                .Any(a => a.AnnotationProperty.GetIRI().ToString() == SKOSVocabulary.Broader.URI.ToString()
                       && a.SubjectIRI.ToString() == child.URI.ToString()
                       && a.ValueIRI?.ToString() == parent.URI.ToString());

            hasNarrower.Should().BeTrue("skos:narrower parent→child written");
            hasBroader.Should().BeTrue("skos:broader child→parent written (reciprocal)");
        }

        [Fact]
        public void AnnotateConceptPreferredLabel_Appends_The_PrefLabel_Axiom()
        {
            // Write-path verification via direct graph inspection (bypassing the broken reader).
            // Assert the prefLabel axiom exists for the concept — the literal value rendering
            // format is OWLSharp-internal, so we only assert the property+subject binding.
            var adapter = NewAdapter();
            var concept = Res("C");
            adapter.DeclareConcept(concept, Res("Scheme"));
            adapter.AnnotateConceptPreferredLabel(concept, Lit("Ad Hominem"));

            var onto = adapter.GetOntology();
            var hasLabel = onto.AnnotationAxioms.OfType<OWLSharp.Ontology.OWLAnnotationAssertion>()
                .Any(a => a.AnnotationProperty.GetIRI().ToString() == SKOSVocabulary.PrefLabel.URI.ToString()
                       && a.SubjectIRI.ToString() == concept.URI.ToString()
                       && a.ValueLiteral != null);
            hasLabel.Should().BeTrue("skos:prefLabel axiom written to the graph (write works; reader broken)");
        }

        [Fact]
        public void BUG_CheckHasClass_Returns_False_Despite_Class_Declared()
        {
            // CheckHasClass uses DeclarationAxioms and compares cls.GetIRI().Equals(resource.URI)
            // where .URI is a System.Uri. RDFResource.Equals(Uri) is false by type-mismatch — the
            // SAME root cause as the annotation readers. So CheckHasClass is also broken.
            var adapter = NewAdapter();
            var declared = Res("DeclaredClass");
            adapter.DeclareClass(declared);

            // The class IS declared in DeclarationAxioms:
            var onto = adapter.GetOntology();
            onto.DeclarationAxioms.Should().NotBeEmpty("DeclareClass wrote the declaration axiom");

            // ...but CheckHasClass can't find it:
            adapter.CheckHasClass(declared).Should().BeFalse(
                "[BUG] cls.GetIRI().Equals(resource.URI) compares RDFResource.Equals(Uri) → false by type-mismatch");
        }

        [Fact]
        public void DocumentConcept_Unknown_Type_Throws()
        {
            var adapter = NewAdapter();
            var concept = Res("C");
            var invalid = (SKOSDocumentationTypes)999;

            Action act = () => adapter.DocumentConcept(concept, invalid, Lit("x"));
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ToFileAsync_Writes_A_Non_Empty_OWL2XML_File()
        {
            // Serialization works end-to-end (this is why #133 ships a non-empty ontology despite
            // the readers being broken — the graph is correctly built, only self-retrieval fails).
            var adapter = NewAdapter();
            var scheme = Res("Scheme");
            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareConcept(Res("C1"), scheme);
            adapter.AnnotateConceptPreferredLabel(Res("C1"), Lit("Test Concept"));

            var tempPath = Path.Combine(Path.GetTempPath(), $"arg_onto_test_{Guid.NewGuid():N}.owl");
            try
            {
                adapter.ToFileAsync(OWLSharp.OWLEnums.OWLFormats.OWL2XML, tempPath).Wait();
                File.Exists(tempPath).Should().BeTrue();
                var content = File.ReadAllText(tempPath);
                content.Should().NotBeNullOrEmpty("the serialized ontology must be non-empty");
                content.Should().Match(s => s.Contains("<rdf:RDF") || s.Contains("xmlns"),
                    "the OWL2XML output must be RDF/XML");
            }
            finally
            {
                if (File.Exists(tempPath)) File.Delete(tempPath);
            }
        }
    }
}
