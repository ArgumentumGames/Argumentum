using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using OWLSharp;
using OWLSharp.Ontology;
using RDFSharp.Model;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// #133 publication bridge (pool #458, grain ⑥) — dcterms + rdfs:seeAlso towards the served
    /// Pages endpoints. The gap was measured on 2026-09-22 (133-endpoint-vs-ticket-gap): zero
    /// occurrence of purl.org/dc/terms/ and zero seeAlso towards the served URL — the ontology
    /// documented its concepts but did not bridge its own endpoint.
    ///
    /// These tests pin the emission contract additively: generator defaults, serialized shape,
    /// the frozen-IRI guarantee (no Argumentum-namespace IRI created or moved — the Q-11 surface
    /// stays intact), and reload survival of the bridge through the same OWL2XML path the
    /// generators use. They never read the committed ontology artefacts: those move only with the
    /// post-Q-11 regeneration (#1529), so this file needs no PublishedOntology serialization.
    /// </summary>
    public class OwlPublicationMetadataBridgeTests
    {
        private const string FallaciesNs = "https://www.argumentum.games/argumentum_fallacies.owl#";
        private const string VirtuesNs = "https://www.argumentum.games/argumentum_virtues.owl#";
        private const string ServedBase = "https://argumentumgames.github.io/Argumentum/docs/ontology";
        private const string DcTermsNs = "http://purl.org/dc/terms/";
        // Q-15a (24/09/2026): the published .owl reuses the text of the CC BY-SA 4.0 taxonomies,
        // so the content licence — not the generator's LGPL-3.0 — applies (LICENSE-CONTENT.md §1).
        private const string CcBySaIri = "https://creativecommons.org/licenses/by-sa/4.0/";

        private static string FallaciesFile => $"{ServedBase}/argumentum.owl";
        private static string VirtuesFile => $"{ServedBase}/argumentum_virtues.owl";

        /// <summary>Minimal ontology skeleton identical to what the generators build first:
        /// namespace + scheme + one concept, so the A/B IRI control compares like with like.</summary>
        private static OwlAdapter BuildSkeleton(string ns)
        {
            var adapter = new OwlAdapter(ns);
            var scheme = new RDFResource($"{ns}testScheme");
            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareConcept(new RDFResource($"{ns}testConcept"), scheme);
            return adapter;
        }

        private static async Task<string> SerializeToTemp(OwlAdapter adapter)
        {
            var path = Path.Combine(Path.GetTempPath(), $"owl-bridge-{Guid.NewGuid():N}.owl");
            await adapter.ToFileAsync(OWLEnums.OWLFormats.OWL2XML, path);
            return path;
        }

        private static readonly Regex ArgumentumIriRegex = new Regex(
            @"https://www\.argumentum\.games/[A-Za-z0-9_.#\-]+", RegexOptions.Compiled);

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) Generator defaults — the bridge must be ON by default on both corpora,
        //     symmetric (self first, sibling second), and point only at the served base.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void FallaciesDefaults_CarryThePublicationBridge()
        {
            var doc = new OwlGeneratorConfig().DocumentConfigs.Single();

            doc.Title.Should().Be("Argumentum Fallacies Ontology");
            doc.SeeAlsoEndpoints.Should().Equal(new[] { FallaciesFile, VirtuesFile },
                "self first, sibling second — the cross-corpus bridge is symmetric with the Virtues pass.");
            doc.LicenseIri.Should().Be(CcBySaIri,
                "Q-15a: the artefact carries the content licence of the taxonomies it reuses.");
        }

        [Fact]
        public void VirtuesDefaults_CarryThePublicationBridge()
        {
            var doc = new VirtueOwlGeneratorConfig().DocumentConfigs.Single();

            doc.Title.Should().Be("Argumentum Virtues Ontology");
            doc.SeeAlsoEndpoints.Should().Equal(VirtuesFile, FallaciesFile);
            doc.LicenseIri.Should().Be(CcBySaIri, "Q-15a applies to both corpora symmetrically.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) Serialized shape — the artefact gains the two dcterms annotations as
        //     ontology-level <Annotation> children and the endpoints as seeAlso
        //     annotation assertions on the ontology IRI.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public async Task Bridge_SerializesDctermsAndEndpointSeeAlso()
        {
            var adapter = BuildSkeleton(FallaciesNs);
            adapter.AnnotatePublicationMetadata("Argumentum Fallacies Ontology", "Argumentum",
                new[] { FallaciesFile, VirtuesFile }, CcBySaIri);
            var path = await SerializeToTemp(adapter);
            try
            {
                var xml = File.ReadAllText(path);

                xml.Should().Contain($"{DcTermsNs}title",
                    "the gap measured on 2026-09-22 was zero purl.org/dc/terms/ occurrence.");
                xml.Should().Contain($"{DcTermsNs}creator");
                xml.Should().Contain(">Argumentum Fallacies Ontology<");
                xml.Should().Contain(FallaciesFile,
                    "seeAlso must bridge the ontology to its own served endpoint.");
                xml.Should().Contain(VirtuesFile,
                    "seeAlso must bridge the ontology to its sibling corpus endpoint.");
                xml.Should().Contain($"{DcTermsNs}license",
                    "Q-15a: the artefact states its content licence machine-readably.");
                xml.Should().Contain(CcBySaIri);
                xml.Should().MatchRegex(
                    $@"<AnnotationAssertion>\s*<AnnotationProperty IRI=""{Regex.Escape(DcTermsNs)}license""",
                    "the licence is an IRI-valued annotation assertion on the ontology IRI, " +
                    "same serialization family as the endpoint seeAlso.");

                xml.Should().MatchRegex(
                    $@"<Annotation>\s*<AnnotationProperty IRI=""{Regex.Escape(DcTermsNs)}title""",
                    "dcterms annotations are ontology-level children, like comment/versionInfo/dc:creator.");
                xml.Should().MatchRegex(
                    $@"<AnnotationAssertion>\s*<AnnotationProperty IRI=""[^""]*seeAlso""",
                    "the endpoint bridge is an annotation assertion, same serialization family as the " +
                    "concept-level seeAlso.");
            }
            finally
            {
                File.Delete(path);
            }
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) ⛔ Frozen-IRI control (order: "Aucune IRI touchée", "diff d'IRI = 0") —
        //     A/B on identical skeletons: the Argumentum-namespace IRI set is byte-equal
        //     with and without the bridge.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public async Task Bridge_IntroducesNoArgumentumNamespaceIri()
        {
            var bare = BuildSkeleton(FallaciesNs);
            var bridged = BuildSkeleton(FallaciesNs);
            bridged.AnnotatePublicationMetadata("Argumentum Fallacies Ontology", "Argumentum",
                new[] { FallaciesFile, VirtuesFile }, CcBySaIri);

            var barePath = await SerializeToTemp(bare);
            var bridgedPath = await SerializeToTemp(bridged);
            try
            {
                var bareIris = ArgumentumIriRegex.Matches(File.ReadAllText(barePath))
                    .Select(m => m.Value).ToHashSet();
                var bridgedIris = ArgumentumIriRegex.Matches(File.ReadAllText(bridgedPath))
                    .Select(m => m.Value).ToHashSet();

                bareIris.Should().NotBeEmpty("the skeleton must carry Argumentum IRIs for the control to bite.");
                bridgedIris.Should().Equal(bareIris,
                    "the publication bridge adds dcterms predicates and endpoint IRIs only — the " +
                    "Argumentum IRI surface is frozen (Q-11).");
            }
            finally
            {
                File.Delete(barePath);
                File.Delete(bridgedPath);
            }
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) Round-trip — the bridge must survive the OWL2XML reload the validators use.
        //     (OWLSharp's reload drops rdf:type from annotation streams — pinned separately
        //     in OwlE2EGenerationValidationTests; here we pin what happens to OUR bridge.)
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public async Task Bridge_SurvivesOwl2XmlReload()
        {
            var adapter = BuildSkeleton(VirtuesNs);
            adapter.AnnotatePublicationMetadata("Argumentum Virtues Ontology", "Argumentum",
                new[] { VirtuesFile, FallaciesFile }, CcBySaIri);
            var path = await SerializeToTemp(adapter);
            try
            {
                var reloaded = await OWLOntology.FromFileAsync(OWLEnums.OWLFormats.OWL2XML, path);

                var seeAlsoValues = reloaded.AnnotationAxioms.OfType<OWLAnnotationAssertion>()
                    .Where(a => a.AnnotationProperty.GetIRI().ToString() == RDFVocabulary.RDFS.SEE_ALSO.ToString())
                    .Select(a => a.ValueIRI)
                    .ToList();

                seeAlsoValues.Should().Contain(VirtuesFile,
                    "an IRI-valued seeAlso assertion on the ontology must survive the reload — " +
                    "unlike rdf:type, nothing in the documented serializer drops annotation objects.");
                seeAlsoValues.Should().Contain(FallaciesFile);

                var licenseValues = reloaded.AnnotationAxioms.OfType<OWLAnnotationAssertion>()
                    .Where(a => a.AnnotationProperty.GetIRI().ToString() == $"{DcTermsNs}license")
                    .Select(a => a.ValueIRI)
                    .ToList();

                licenseValues.Should().Contain(CcBySaIri,
                    "the IRI-valued dcterms:license assertion must survive the OWL2XML reload, " +
                    "same as seeAlso — Q-15a must stay machine-readable after a round-trip.");
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
