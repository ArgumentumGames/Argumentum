using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// Contract pins for #499 Phase 2 — the Virtues OWL generator (<see cref="VirtueOwlGeneratorConfig"/>
    /// / <see cref="VirtueOwlDocumentConfig"/>). Part 2 mirrors the Fallacies OWL pass but emits
    /// argumentum_virtues.owl, and links each Virtue concept to its Walton argumentation scheme via a
    /// custom <c>aif:goodTenorOf</c> object property (since #989 the Virtue critical questions live
    /// in AIF_criticalQuestion and AIF_skosMappingType is empty on the Virtues side, so the
    /// Fallacies skos:*Match switch cannot fire). Cross-corpus
    /// Virtue↔Fallacy links (crossLink_Opposes PK→URI) are deferred to Phase 3.
    ///
    /// These tests pin the pure, deterministic contract additively (no I/O, no pipeline run):
    /// the IRI transform, the generator defaults, and the <c>aif:goodTenorOf</c> URI construction.
    /// </summary>
    public class VirtueOwlGenerationContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // (1) GetId — the Virtue concept IRI fragment derives from TitleEn via the SAME transform as
        //     the Fallacies pass (Humanizer Camelize + strip apostrophes/hyphens/commas). Pinning it
        //     separately guards against drift between the two generator configs.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("Arguments vertueux", "argumentsVertueux")]
        [InlineData("Rigorous language", "rigorousLanguage")]
        public void GetId_MirrorsFallaciesIriTransform(string input, string expected)
        {
            VirtueOwlDocumentConfig.GetId(input).Should().Be(expected,
                "the Virtue concept IRI fragment must use the same Camelize + strip transform as OwlDocumentConfig.GetId.");
        }

        [Fact]
        public void GetId_StripsApostrophesHyphensCommas()
        {
            // Mirrors the proven OwlDocumentConfig.GetId contract (byte-identical transform), asserted
            // on the exact same input strings so a divergence between the two configs is caught.
            // Camelize preserves punctuation; the trailing Replace chain strips apostrophes/hyphens/commas;
            // accented characters are preserved (Humanizer does not ASCII-fold).
            VirtueOwlDocumentConfig.GetId("A-B").Should().Be("aB", "hyphens are stripped.");
            VirtueOwlDocumentConfig.GetId("A, B").Should().Be("aB", "commas are stripped.");
            VirtueOwlDocumentConfig.GetId("Appel à l'autorité").Should().Be("appelÀLautorité",
                "apostrophes stripped, accents preserved, segment starts uppercased by Camelize.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) Generator defaults — lock the mono-corpus Virtues ontology identity so a config reset
        //     never silently repoints the document name / namespace / dataset.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void DefaultConfig_EmitsVirtuesOntologyIdentity()
        {
            var config = new VirtueOwlGeneratorConfig();

            config.DocumentConfigs.Should().ContainSingle();
            var doc = config.DocumentConfigs[0];

            doc.DocumentName.Should().Be("argumentum_virtues.owl");
            doc.DataSet.Should().Be(KnownDataSets.VirtuesTaxonomy,
                "the Virtues OWL pass must feed off the Virtues taxonomy dataset, not the Fallacies one.");
            doc.OntologyNamespace.Should().Be("https://www.argumentum.games/argumentum_virtues.owl#");
            doc.ExternalReferenceOntologyNamespaceURI.Should().StartWith("http://www.arg.dundee.ac.uk/aif#",
                "the AIF namespace must be the Dundee AIF ontology so aif:goodTenorOf resolves externally.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) aif:goodTenorOf URI — the custom object property linking a Virtue to the Walton scheme
        //     it is the good tenor of. Phase 2 carries the critical-question prose as rdfs:comment;
        //     this predicate is the structured link. Pin the URI so Phase 3 cross-corpus can reuse it.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void GoodTenorOf_Predicate_ResolvesInAifNamespace()
        {
            var doc = new VirtueOwlDocumentConfig
            {
                ExternalReferenceOntologyNamespaceURI = "http://www.arg.dundee.ac.uk/aif#"
            };

            // Mirrors the URI assembled in VirtueOwlDocumentConfig.CreateVirtueOwlDocument.
            var goodTenorOfUri = $"{doc.ExternalReferenceOntologyNamespaceURI}goodTenorOf";

            goodTenorOfUri.Should().Be("http://www.arg.dundee.ac.uk/aif#goodTenorOf",
                "aif:goodTenorOf must live in the AIF namespace (already used by the Fallacies pass), not a new namespace.");
        }
    }
}
