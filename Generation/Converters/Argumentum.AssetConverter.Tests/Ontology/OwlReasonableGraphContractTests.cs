using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// Contrat #133 : les aretes du graphe argumentatif doivent etre RAISONNABLES, pas seulement lisibles.
    ///
    /// POURQUOI CET ORGANE EXISTE
    /// Les crosslinks transverses et le typage d'attaque AIF etaient emis uniquement comme
    /// <c>AnnotationAssertion</c>. C'est correct pour un thesaurus SKOS, mais les annotations sont
    /// hors de la semantique logique d'OWL : un raisonneur (HermiT, Pellet, owlrl) ne les voit pas.
    ///
    /// Mesure sur l'artefact publie du 21/08, avant correction :
    ///   crosslinks presents (leverages 403, mirrors 722, isRelatedTo 643, ...) = 1989 mentions
    ///   ObjectPropertyAssertion                                               = 0
    /// Le graphe etait donc interrogeable en SPARQL et strictement non raisonnable : tout delta
    /// d'inference sur ces aretes etait vide PAR CONSTRUCTION, quel que soit le raisonneur.
    ///
    /// L'aval en depend explicitement. CoursIA#13567 ("brancher le graphe argumentatif AIF/Dung sur
    /// le coup ontologique executable de SW-14") pose en critere d'acceptation que "le delta
    /// d'inferences owlrl soit non vide et exhibe -- si l'extension ne produit aucune consequence
    /// nouvelle, ce n'est pas un coup ontologique, c'est une decoration". Avec des annotations
    /// seules, ce critere etait inatteignable avec nos donnees.
    ///
    /// CE QUE LA GARDE VERIFIE
    ///   1. Les deux ontologies publiees portent des ObjectPropertyAssertion.
    ///   2. Elles portent TOUJOURS leurs AnnotationAssertion : l'emission est additive. Retirer
    ///      l'annotation casserait la lecture SKOS du thesaurus -- une "simplification" qui
    ///      n'emettrait plus qu'une seule des deux formes doit rougir ici.
    ///   3. Les predicats crosslink apparaissent bien en position de propriete d'assertion, et pas
    ///      seulement en declaration : une propriete declaree mais jamais assertee ne relie rien.
    ///
    /// QUE FAIRE SI C'EST ROUGE : regenerer les ontologies (--generate-owl) et republier dans
    /// docs/ontology/. Ne pas relacher le seuil : le rouge dit que le graphe publie a cesse d'etre
    /// raisonnable, pas que l'attente est trop stricte.
    /// </summary>
    [Collection(PublishedOntologyCollection.Name)]
    public class OwlReasonableGraphContractTests
    {
        private static string RepoRoot => TestRepoRoot.Find();

        private static string Published(string fileName)
            => File.ReadAllText(Path.Combine(RepoRoot, "docs", "ontology", fileName));

        [Theory]
        [InlineData("argumentum.owl")]
        [InlineData("argumentum_virtues.owl")]
        public void PublishedOntology_CarriesObjectPropertyAssertions(string fileName)
        {
            var owl = Published(fileName);
            var assertions = CountOccurrences(owl, "<ObjectPropertyAssertion");

            assertions.Should().BeGreaterThan(0,
                $"{fileName} doit porter des aretes assertees : sans elles aucun raisonneur ne voit " +
                "le graphe argumentatif, et tout delta d'inference est vide par construction. " +
                "Regenerer via --generate-owl puis republier dans docs/ontology/.");
        }

        [Theory]
        [InlineData("argumentum.owl")]
        [InlineData("argumentum_virtues.owl")]
        public void PublishedOntology_KeepsAnnotationsAlongsideAssertions(string fileName)
        {
            var owl = Published(fileName);

            CountOccurrences(owl, "<AnnotationAssertion").Should().BeGreaterThan(0,
                $"{fileName} doit CONSERVER ses annotations SKOS : l'emission d'assertions est " +
                "additive, elle ne remplace pas la lecture thesaurus.");
            CountOccurrences(owl, "<ObjectPropertyAssertion").Should().BeGreaterThan(0,
                $"{fileName} doit porter les deux formes simultanement.");
        }

        [Fact]
        public void FallaciesOntology_AssertsCrossLinkPredicates_NotOnlyDeclaresThem()
        {
            var owl = Published("argumentum.owl");

            // Un predicat peut etre declare ObjectProperty et n'etre jamais asserte : il ne relie
            // alors rien du tout. On exige sa presence DANS un bloc d'assertion.
            var asserted = owl.Split("<ObjectPropertyAssertion").Skip(1).ToList();
            asserted.Should().NotBeEmpty("l'ontologie doit contenir des blocs d'assertion");

            foreach (var predicate in new[] { "leverages", "mirrors", "isRelatedTo" })
            {
                asserted.Any(block => block.Contains($"#{predicate}\"")).Should().BeTrue(
                    $"le predicat crosslink '{predicate}' doit apparaitre en propriete d'une " +
                    "assertion, pas seulement en declaration.");
            }
        }

        private static int CountOccurrences(string haystack, string needle)
        {
            int count = 0, index = 0;
            while ((index = haystack.IndexOf(needle, index, System.StringComparison.Ordinal)) >= 0)
            {
                count++;
                index += needle.Length;
            }
            return count;
        }
    }
}
