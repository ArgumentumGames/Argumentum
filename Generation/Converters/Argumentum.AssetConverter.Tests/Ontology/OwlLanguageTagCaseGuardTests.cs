using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using OWLSharp;
using RDFSharp.Model;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// Garde #1622 point 3 : les balises de langue sortent en minuscules de l'écriture OWL2XML.
    ///
    /// Cause rejouée le 2026-09-30 (elle était rapportée par décompilation, non mesurée) : le
    /// générateur passe bien <c>"fr"</c> / <c>"en"</c> aux constructeurs de littéraux, et c'est
    /// <b>OWLSharp 5.0</b> qui met la balise en majuscules à la sérialisation. Une ontologie
    /// minimale portant <c>new RDFPlainLiteral("essai", "fr")</c> sortait en
    /// <c>&lt;Literal xml:lang="FR"&gt;essai&lt;/Literal&gt;</c>.
    ///
    /// Ce que ça coûte : une balise est insensible à la casse (BCP 47), donc l'artefact reste
    /// valide — mais une requête qui compare <c>lang(?x) = "fr"</c> ne trouve <b>rien</b>, et RDF
    /// 1.1 place la valeur des balises de langue en minuscules. Les deux artefacts committés
    /// portaient <b>11 927</b> littéraux dans ce cas, aucun en minuscules :
    /// <c>argumentum.owl</c> 10 428 (5 562 EN + 4 866 FR), <c>argumentum_virtues.owl</c> 1 499
    /// (640 EN + 859 FR).
    ///
    /// Le test 2 est le contrôle qui compte : OWLSharp échappe <c>&lt;</c> et <c>&amp;</c> mais
    /// <b>pas</b> le guillemet, donc un littéral dont la valeur contient la séquence visée est
    /// écrit verbatim dans le contenu de l'élément. Une réécriture textuelle nue corromprait la
    /// charge utile ; celle-ci suit l'état du balisage et la laisse intacte.
    /// </summary>
    public class OwlLanguageTagCaseGuardTests
    {
        private const string Ns = "https://ex.example/ns#";

        private static OwlAdapter Skeleton()
        {
            var adapter = new OwlAdapter(Ns);
            var scheme = new RDFResource($"{Ns}testScheme");
            adapter.DeclareConceptScheme(scheme);
            adapter.DeclareConcept(new RDFResource($"{Ns}testConcept"), scheme);
            return adapter;
        }

        private static async Task<string> SerializeAsync(OwlAdapter adapter)
        {
            var path = Path.Combine(Path.GetTempPath(), $"owl-lang-{Guid.NewGuid():N}.owl");
            await adapter.ToFileAsync(OWLEnums.OWLFormats.OWL2XML, path);
            return path;
        }

        private static readonly Regex LangAttribute = new Regex("xml:lang=\"([^\"]*)\"", RegexOptions.Compiled);

        [Fact]
        public async Task LanguageTags_AreLowercase_OnDisk()
        {
            var adapter = Skeleton();
            var concept = new RDFResource($"{Ns}testConcept");
            adapter.AnnotateConceptPreferredLabel(concept, new RDFPlainLiteral("essai", "fr"));
            adapter.AnnotateConceptPreferredLabel(concept, new RDFPlainLiteral("trial", "en"));
            adapter.DocumentConcept(concept, SKOSDocumentationTypes.Definition, new RDFPlainLiteral("def", "fr"));

            var path = await SerializeAsync(adapter);
            try
            {
                var xml = File.ReadAllText(path);
                var tags = LangAttribute.Matches(xml).Select(m => m.Groups[1].Value).ToList();

                // Anti-vacuité : sur un document sans littéral, « aucune balise en majuscules »
                // serait vrai sans rien prouver.
                tags.Should().HaveCountGreaterThanOrEqualTo(3,
                    "les trois littéraux du montage portent chacun une balise de langue.");
                tags.Should().Contain("fr", "le générateur a passé \"fr\" et l'artefact doit le porter tel quel.");
                tags.Should().Contain("en", "le générateur a passé \"en\" et l'artefact doit le porter tel quel.");
                tags.Should().BeEquivalentTo(tags.Select(t => t.ToLowerInvariant()).ToList(),
                    "aucune balise ne sort en majuscules : OWLSharp les met en majuscules à la sérialisation (#1622 point 3).");
                xml.Should().NotContain("xml:lang=\"FR\"", "la forme majuscule ne doit plus être écrite.");
                xml.Should().NotContain("xml:lang=\"EN\"", "la forme majuscule ne doit plus être écrite.");
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public async Task LiteralPayload_ContainingTheAttributeSequence_IsNotRewritten()
        {
            // OWLSharp n'échappe pas le guillemet : cette charge utile est écrite verbatim dans
            // le contenu de l'élément (mesuré le 2026-09-30). Un remplacement textuel nu la
            // réécrirait aussi ; la réécriture par balisage non.
            const string payload = "exemple avec xml:lang=\"FR\" dans le texte";

            var adapter = Skeleton();
            var concept = new RDFResource($"{Ns}testConcept");
            adapter.DocumentConcept(concept, SKOSDocumentationTypes.Example, new RDFPlainLiteral(payload, "fr"));

            var path = await SerializeAsync(adapter);
            try
            {
                var xml = File.ReadAllText(path);
                xml.Should().Contain(payload,
                    "la charge utile d'un littéral est du texte, pas du balisage : elle doit ressortir " +
                    "octet pour octet, guillemets compris.");
                LangAttribute.Matches(xml).Select(m => m.Groups[1].Value)
                    .Should().Contain("fr",
                    "la balise du littéral lui-même est bien normalisée — c'est le contrôle positif " +
                    "qui empêche le test de passer sur un document vide.");
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}