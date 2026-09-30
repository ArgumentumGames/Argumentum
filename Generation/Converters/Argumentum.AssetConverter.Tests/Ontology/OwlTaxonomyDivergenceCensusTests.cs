using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter.Ontology;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using OWLSharp.Ontology;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// Census #1666 — l'ecart entre les IRI que la taxonomie PRODUIT et les concepts que
    /// l'ontologie committée PORTE, nommé plutôt que silencieux.
    ///
    /// <para><b>Pourquoi cet organe existe.</b> <c>docs/ontology/argumentum.owl</c> est produit par une
    /// passe <c>--generate-owl</c> délibérée : un écart entre deux régénérations est donc <b>attendu</b>.
    /// Ce qui ne l'est pas, c'est qu'il soit <b>invisible</b> : rien ne distingue une dérive ordinaire
    /// d'un oubli, et #133 (publication, qui <b>fige les IRI</b>) peut partir à tout moment. Mesuré sur
    /// <c>f95c0b70</c> : 1313 IRI produites par le CSV, 1306 concepts portés par le fichier, 16 absents,
    /// 9 orphelins.</para>
    ///
    /// <para><b>Pourquoi aucun organe existant ne le voyait.</b>
    /// <c>OwlE2EGenerationValidationTests.LoadedOntology_CrossLinkAndAifCounts_MatchTheCorpusExactly</c>
    /// réconcilie bien CSV et OWL — mais sur des <i>comptes d'assertions par verbe</i>. Un concept
    /// <b>renommé</b> conserve ses liens : les comptes restent exacts, l'identité du concept a changé.
    /// Le test est structurellement aveugle au renommage, et <c>OwlIriFragmentValidityTests</c> ne lit
    /// que la grammaire des fragments. Celui-ci compare les <b>ensembles d'identités</b>.</para>
    ///
    /// <para><b>La divergence n'est pas un invariant — c'est un point de décision.</b> Les bornes sont
    /// donc <b>posées sur l'écart, jamais sur l'égalité</b>, dans le sens qui laisse la réparation
    /// passer au vert : des <b>planchers</b> sur les deux populations (anti-vacuité — un OWL tronqué
    /// ne peut pas rendre « 0 absent » sur 0 concept), et des <b>plafonds</b> sur l'écart lui-même.
    /// Une régénération qui réconcilie fait <b>baisser</b> l'écart : vert. Un renommage anglais qui
    /// n'est pas suivi d'une régénération le fait <b>monter</b> : rouge, et le message nomme les IRI.
    /// <b>Que faire si c'est rouge</b> : régénérer, ou recaler le plafond avec la date — jamais
    /// effacer la ligne.</para>
    /// </summary>
    [Collection(PublishedOntologyCollection.Name)]
    public class OwlTaxonomyDivergenceCensusTests
    {
        private readonly ITestOutputHelper _out;

        public OwlTaxonomyDivergenceCensusTests(ITestOutputHelper output) => _out = output;

        // Planchers : anti-vacuité. Un fichier illisible ou un glob cassé rend 0 des deux côtés,
        // et « 0 absent » serait alors vrai sans rien prouver.
        private const int ProducedFloor = 1300;
        private const int DeclaredFloor = 1290;

        // Plafonds : l'écart mesuré sur f95c0b70. Ils ne peuvent que descendre.
        private const int AbsentCeiling = 16;
        private const int OrphanCeiling = 9;

        private static readonly Lazy<OwlAdapter> CommittedOntology = new(() =>
            OwlAdapter.FromFile(Path.Combine(TestRepoRoot.Find(), "docs", "ontology", "argumentum.owl")));

        /// <summary>
        /// Les IRI que la taxonomie produit : <see cref="OwlDocumentConfig.GetId"/> sur <c>text_en</c>,
        /// l'organe de production lui-même — jamais une réimplémentation, qui dériverait au premier
        /// changement de <c>GetId</c> (c'est exactement ce que #1651/#1622 point 1 modifient).
        /// </summary>
        private static HashSet<string> ProducedIds()
        {
            var csvPath = Path.Combine(TestRepoRoot.Find(),
                "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                BadDataFound = null,
                HeaderValidated = null,
            };
            using var reader = new StringReader(File.ReadAllText(csvPath));
            using var csv = new CsvReader(reader, config);

            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var record in csv.GetRecords<dynamic>())
            {
                var row = (IDictionary<string, object>)record;
                var textEn = row.TryGetValue("text_en", out var raw) ? raw?.ToString()?.Trim() : null;
                if (string.IsNullOrEmpty(textEn))
                    continue;
                var id = OwlDocumentConfig.GetId(textEn);
                if (!string.IsNullOrEmpty(id))
                    ids.Add(id);
            }
            return ids;
        }

        /// <summary>
        /// Les concepts que le fichier porte : les sujets des assertions <c>skos:prefLabel</c>,
        /// réduits au fragment d'IRI. C'est la définition que #1666 a mesurée (1306 sur
        /// <c>f95c0b70</c>), et <c>rdf:type</c> ne survit pas à l'aller-retour OWL2XML — donc
        /// <c>prefLabel</c> est le seul ancrage disponible, comme le note <c>OwlAdapter.GetConcepts</c>.
        /// </summary>
        private static HashSet<string> DeclaredFragments()
        {
            var fragments = new HashSet<string>(StringComparer.Ordinal);
            var prefLabel = SKOSVocabulary.PrefLabel.ToString();
            foreach (var subject in CommittedOntology.Value.GetOntology().AnnotationAxioms
                         .OfType<OWLAnnotationAssertion>()
                         .Where(a => a.AnnotationProperty.GetIRI().ToString() == prefLabel)
                         .Select(a => a.SubjectIRI?.ToString())
                         .Where(s => !string.IsNullOrEmpty(s)))
            {
                var hash = subject.LastIndexOf('#');
                if (hash >= 0 && hash + 1 < subject.Length)
                    fragments.Add(subject.Substring(hash + 1));
            }
            return fragments;
        }

        private (HashSet<string> Produced, HashSet<string> Declared) Measure()
        {
            var produced = ProducedIds();
            var declared = DeclaredFragments();

            // Anti-vacuité, avant tout verdict : les deux populations doivent exister.
            produced.Should().HaveCountGreaterThanOrEqualTo(ProducedFloor,
                "le CSV Fallacies doit produire ses IRI. Un compte bas signifie que la lecture a échoué, " +
                "pas que l'écart a disparu.");
            declared.Should().HaveCountGreaterThanOrEqualTo(DeclaredFloor,
                "docs/ontology/argumentum.owl doit porter ses concepts. Un compte bas signifie que le " +
                "chargement a échoué, pas que la divergence est résolue.");

            return (produced, declared);
        }

        [Fact]
        public void TheDivergence_StaysWithinItsPinnedBounds()
        {
            var (produced, declared) = Measure();
            var absent = produced.Except(declared).OrderBy(x => x, StringComparer.Ordinal).ToList();
            var orphans = declared.Except(produced).OrderBy(x => x, StringComparer.Ordinal).ToList();

            _out.WriteLine($"produites={produced.Count} portees={declared.Count} " +
                           $"absentes={absent.Count} orphelines={orphans.Count}");
            _out.WriteLine("ABSENTES  : " + string.Join(", ", absent));
            _out.WriteLine("ORPHELINES: " + string.Join(", ", orphans));

            absent.Should().HaveCountLessThanOrEqualTo(AbsentCeiling,
                $"une IRI que la taxonomie produit et que le fichier ne porte pas est un concept que la " +
                $"publication #133 servirait sous une identité qui n'existe plus. Écart mesuré à f95c0b70 : " +
                $"{AbsentCeiling}. Régénérer (--generate-owl), ou recaler ce plafond avec la date — jamais " +
                $"effacer la ligne. Absentes aujourd'hui ({absent.Count}) : {string.Join(", ", absent)}");

            orphans.Should().HaveCountLessThanOrEqualTo(OrphanCeiling,
                $"un concept que le fichier porte et qu'aucune ligne ne produit est une identité que la " +
                $"publication #133 figerait sans corpus derrière. Écart mesuré à f95c0b70 : {OrphanCeiling}. " +
                $"Régénérer (--generate-owl), ou recaler ce plafond avec la date — jamais effacer la ligne. " +
                $"Orphelines aujourd'hui ({orphans.Count}) : {string.Join(", ", orphans)}");
        }

        /// <summary>
        /// Contrôle positif du census lui-même : il ne suffit pas que l'écart soit petit, il faut
        /// qu'il <b>décrive la bonne chose</b>. Deux formes sont épinglées par leur nom.
        ///
        /// <para><b>Un concept aligné est des deux côtés.</b> C'est la preuve que les deux lectures
        /// parlent du même espace : sans elle, deux lecteurs revenant vides rendraient un écart nul
        /// que le plafond accepterait.</para>
        ///
        /// <para><b>Un renommage tombe d'un côté et entre de l'autre, jamais les deux.</b> Les deux
        /// paires épinglées sont celles que #1666 a appariées par leur <c>prefLabel</c> français :
        /// « Faulty reasoning » contre « Faulty logics », et « Cafeteria Christianity » contre
        /// « Cafetaria Christianity » — la seconde documentant au passage la coquille du côté OWL.</para>
        ///
        /// <para><b>Cycle de vie, le même que les plafonds</b> : une régénération réconcilie et fait
        /// disparaître les noms d'hier. Que le rouge soit lu comme « recaler et dater », jamais comme
        /// « effacer ».</para>
        /// </summary>
        [Fact]
        public void Renames_LandOnOppositeSides_AndAlignedConcepts_OnBoth()
        {
            var (produced, declared) = Measure();

            foreach (var aligned in new[] { "ableism", "absurd" })
            {
                produced.Should().Contain(aligned,
                    "« {0} » est produit par la taxonomie — il doit rester dans l'ensemble CSV.", aligned);
                declared.Should().Contain(aligned,
                    "« {0} » est porté par l'ontologie committée — il doit rester dans l'ensemble OWL. " +
                    "S'il manque ici alors qu'il est produit, le census a cessé de lire le fichier.", aligned);
            }

            produced.Should().Contain("faultyReasoning").And.NotContain("faultyLogics",
                "le nom d'aujourd'hui est « Faulty reasoning » : « faultyLogics » est l'identité d'hier, " +
                "que seule l'ontologie non régénérée porte encore.");
            declared.Should().Contain("faultyLogics").And.NotContain("faultyReasoning",
                "l'ontologie committée porte l'identité d'hier — c'est cet écart que le grain #1666 mesure.");

            produced.Should().Contain("cafeteriaChristianity").And.NotContain("cafetariaChristianity",
                "« Cafeteria » est l'orthographe de la taxonomie d'aujourd'hui.");
            declared.Should().Contain("cafetariaChristianity").And.NotContain("cafeteriaChristianity",
                "« Cafetaria » est l'orthographe figée dans l'ontologie — la coquille est côté OWL.");
        }
    }
}