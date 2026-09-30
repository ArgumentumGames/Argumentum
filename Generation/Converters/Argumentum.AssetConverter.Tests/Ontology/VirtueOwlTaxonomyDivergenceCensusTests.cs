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
    /// Census des Vertus — le miroir de <see cref="OwlTaxonomyDivergenceCensusTests"/> (sophismes,
    /// #1666/#1672) pour <c>docs/ontology/argumentum_virtues.owl</c> : l'écart entre les IRI que la
    /// taxonomie des Vertus PRODUIT et les concepts que l'ontologie committée PORTE, nommé plutôt
    /// que silencieux (dispatch #458 c.5912442926).
    ///
    /// <para><b>Pourquoi ce second organe.</b> Le premier census a prouvé le 2026-09-30 qu'une
    /// divergence d'identités traverse les comptes d'assertions sans les faire bouger : 16 absentes
    /// et 9 orphelines vivaient dans <c>argumentum.owl</c> sans qu'aucun organe les voie. Le fichier
    /// Vertus vit exactement le même risque — la régénération #1681 y a d'ailleurs porté un
    /// renommage (<c>considerationOfOnesIdeologicalBiases</c> →
    /// <c>recognitionOfPersonalIdeologicalBiases</c>, constaté au verdict, non annoncé dans la PR).
    /// La chaîne de fabrication est identique : <see cref="VirtueOwlDocumentConfig.GetId"/> sur le
    /// titre anglais, <c>skos:prefLabel</c> comme seul ancrage des concepts portés.</para>
    ///
    /// <para><b>Bornes.</b> Même doctrine que le census des sophismes : planchers d'anti-vacuité
    /// sur les deux populations, plafonds sur l'écart posés à la valeur RÉCONCILIÉE (la
    /// régénération #1681 a passé le fichier à l'égalité) — l'écart est donc un invariant
    /// d'égalité : tout renommage anglais non suivi d'une régénération rougit nommément. Que faire
    /// si c'est rouge : régénérer, ou recaler le plafond avec la date — jamais effacer la ligne.</para>
    ///
    /// <para><b>Pas de table annoncée ici.</b> Le membre « inclusion d'une table annoncée » du
    /// census des sophismes n'a pas d'équivalent : aucune identité Vertus n'est annoncée à un
    /// consommateur extérieur (l'annonce CoursIA#4960 porte les sophismes). Le témoin de renommage
    /// ci-dessous joue ce rôle localement : la paire nommée au verdict #1681.</para>
    /// </summary>
    [Collection(PublishedOntologyCollection.Name)]
    public class VirtueOwlTaxonomyDivergenceCensusTests
    {
        private readonly ITestOutputHelper _out;

        public VirtueOwlTaxonomyDivergenceCensusTests(ITestOutputHelper output) => _out = output;

        // Planchers : anti-vacuité. Un fichier illisible ou une lecture vide rend 0 des deux côtés,
        // et « 0 absente » serait alors vrai sans rien prouver.
        private const int ProducedFloor = 200;
        private const int DeclaredFloor = 200;

        // Plafonds : la valeur RÉCONCILIÉE par la régénération #1681 — l'égalité est l'invariant.
        private const int AbsentCeiling = 0;
        private const int OrphanCeiling = 0;

        private static readonly Lazy<OwlAdapter> CommittedOntology = new(() =>
            OwlAdapter.FromFile(Path.Combine(TestRepoRoot.Find(), "docs", "ontology", "argumentum_virtues.owl")));

        /// <summary>
        /// Les lignes du CSV des Vertus, lues une seule fois : <c>pk</c> et <c>title_en</c> (chaîne
        /// vide si la cellule est vide) — les deux colonnes que la chaîne de fabrication consomme.
        /// </summary>
        private static IEnumerable<(int Pk, string TitleEn)> Rows()
        {
            var csvPath = Path.Combine(TestRepoRoot.Find(),
                "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                BadDataFound = null,
                HeaderValidated = null,
            };
            using var reader = new StringReader(File.ReadAllText(csvPath));
            using var csv = new CsvReader(reader, config);

            foreach (var record in csv.GetRecords<dynamic>())
            {
                var row = (IDictionary<string, object>)record;
                var rawPk = row.TryGetValue("pk", out var pkRaw) ? pkRaw?.ToString()?.Trim() : null;
                if (!int.TryParse(rawPk, NumberStyles.Integer, CultureInfo.InvariantCulture, out var pk))
                    continue;
                var titleEn = row.TryGetValue("title_en", out var enRaw) ? enRaw?.ToString()?.Trim() : null;
                yield return (pk, titleEn ?? string.Empty);
            }
        }

        /// <summary>
        /// Les IRI que la taxonomie des Vertus produit : <see cref="VirtueOwlDocumentConfig.GetId"/>
        /// sur <c>title_en</c> — l'organe de production lui-même, jamais une réimplémentation (la
        /// chaîne est byte-identique à celle des sophismes, épinglée par
        /// <c>VirtueOwlGenerationContractTests</c>).
        /// </summary>
        private static HashSet<string> ProducedIds()
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var (_, titleEn) in Rows())
            {
                if (string.IsNullOrEmpty(titleEn))
                    continue;
                var id = VirtueOwlDocumentConfig.GetId(titleEn);
                if (!string.IsNullOrEmpty(id))
                    ids.Add(id);
            }
            return ids;
        }

        /// <summary>
        /// Les concepts que le fichier porte : les sujets des assertions <c>skos:prefLabel</c>,
        /// réduits au fragment d'IRI — même ancrage que le census des sophismes (<c>rdf:type</c>
        /// ne survit pas à l'aller-retour OWL2XML, et les ressources AIF externes ne portent pas de
        /// <c>prefLabel</c> : elles ne polluent pas l'ensemble).
        /// </summary>
        private static HashSet<string> DeclaredFragments()
        {
            var fragments = new HashSet<string>(StringComparer.Ordinal);
            var prefLabel = SKOSVocabulary.PrefLabel.ToString();
            foreach (var subject in CommittedOntology.Value.GetOntology().AnnotationAxioms
                         .OfType<OWLAnnotationAssertion>()
                         .Where(a => a.AnnotationProperty.GetIRI().ToString() == prefLabel)
                         .Select(a => a.SubjectIRI?.ToString() ?? string.Empty)
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
                "le CSV des Vertus doit produire ses IRI. Un compte bas signifie que la lecture a échoué, " +
                "pas que l'écart a disparu.");
            declared.Should().HaveCountGreaterThanOrEqualTo(DeclaredFloor,
                "docs/ontology/argumentum_virtues.owl doit porter ses concepts. Un compte bas signifie que " +
                "le chargement a échoué, pas que la divergence est résolue.");

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
                $"une IRI que la taxonomie des Vertus produit et que le fichier ne porte pas est un concept " +
                $"que la publication #133 servirait sous une identité qui n'existe plus. Plafond 0 depuis la " +
                $"réconciliation #1681. Régénérer (--generate-owl), ou recaler ce plafond avec la date — " +
                $"jamais effacer la ligne. Absentes aujourd'hui ({absent.Count}) : {string.Join(", ", absent)}");

            orphans.Should().HaveCountLessThanOrEqualTo(OrphanCeiling,
                $"un concept que le fichier porte et qu'aucune ligne ne produit est une identité que la " +
                $"publication #133 figerait sans corpus derrière. Plafond 0 depuis la réconciliation #1681. " +
                $"Régénérer (--generate-owl), ou recaler ce plafond avec la date — jamais effacer la ligne. " +
                $"Orphelines aujourd'hui ({orphans.Count}) : {string.Join(", ", orphans)}");
        }

        /// <summary>
        /// Contrôle positif du census : il ne suffit pas que l'écart soit petit, il faut qu'il
        /// décrive la bonne chose. Un concept aligné doit être des deux côtés (preuve que les deux
        /// lectures parlent du même espace), et la paire de renommage constatée au verdict #1681 —
        /// <c>considerationOfOnesIdeologicalBiases</c> contre <c>recognitionOfPersonalIdeologicalBiases</c>,
        /// signalée par ai-01 comme non annoncée dans la PR — doit respecter le XOR « exactement un
        /// des deux noms côté OWL » : le nouveau depuis la régénération, jamais les deux (doublon),
        /// jamais aucun (perte).
        /// </summary>
        [Fact]
        public void AlignedConcepts_AreOnBothSides_AndThe1681Rename_HoldsItsXor()
        {
            var (produced, declared) = Measure();

            foreach (var aligned in new[] { "validArgument", "empiricalEvidence" })
            {
                produced.Should().Contain(aligned,
                    "« {0} » est produit par la taxonomie des Vertus — il doit rester dans l'ensemble CSV.",
                    aligned);
                declared.Should().Contain(aligned,
                    "« {0} » est porté par l'ontologie committée — il doit rester dans l'ensemble OWL. " +
                    "S'il manque ici alors qu'il est produit, le census a cessé de lire le fichier.", aligned);
            }

            AssertRenamePair(produced, declared, "considerationOfOnesIdeologicalBiases",
                "recognitionOfPersonalIdeologicalBiases", "Recognition of personal ideological biases");
        }

        /// <summary>
        /// Même contrat que <c>OwlTaxonomyDivergenceCensusTests.AssertRenamePair</c> : côté CSV
        /// l'invariant est absolu (le nom d'aujourd'hui oui, celui d'hier non), côté OWL c'est le
        /// XOR — l'ancien avant la régénération, le nouveau après, jamais les deux (doublon),
        /// jamais aucun (perte). Dupliquée plutôt que partagée : deux organes indépendants doivent
        /// pouvoir diverger l'un de l'autre sans casser l'autre.
        /// </summary>
        private static void AssertRenamePair(HashSet<string> produced, HashSet<string> declared,
            string oldName, string newName, string label)
        {
            produced.Should().Contain(newName,
                $"« {newName} » est le nom d'aujourd'hui de {label} — il doit rester dans l'ensemble CSV.");
            produced.Should().NotContain(oldName,
                $"« {oldName} » est le nom d'hier de {label} — le corpus ne le produit plus. S'il " +
                "réapparaît ici, la taxonomie a régressé, et c'est cette régression qu'il faut traiter.");

            var declaredOld = declared.Contains(oldName);
            var declaredNew = declared.Contains(newName);

            (declaredOld ^ declaredNew).Should().BeTrue(
                $"l'ontologie committée doit porter exactement un des deux noms de {label} : " +
                $"« {oldName} » avant régénération, « {newName} » après. " +
                $"Lu : ancien={declaredOld}, nouveau={declaredNew}. " +
                "Les deux vrais = le concept a été doublé au lieu d'être renommé ; " +
                "les deux faux = il a été perdu au lieu d'être renommé.");
        }

        /// <summary>
        /// Témoin des deux états interdits du XOR, alimenté à la main (même forme que le census des
        /// sophismes — la mutation réelle des données est mesurée dans le corps de PR).
        /// </summary>
        [Fact]
        public void TheRenameInvariant_RejectsBothForbiddenStates()
        {
            var produced = new HashSet<string>(StringComparer.Ordinal) { "newName" };
            HashSet<string> Declared(params string[] names) =>
                new HashSet<string>(names, StringComparer.Ordinal);

            Action oldOnly = () => AssertRenamePair(produced, Declared("oldName"),
                "oldName", "newName", "la paire de témoin");
            oldOnly.Should().NotThrow("l'état d'avant régénération est légitime : l'OWL porte le nom d'hier.");

            Action newOnly = () => AssertRenamePair(produced, Declared("newName"),
                "oldName", "newName", "la paire de témoin");
            newOnly.Should().NotThrow("l'état réconcilié est légitime : l'OWL porte le nom d'aujourd'hui.");

            Action both = () => AssertRenamePair(produced, Declared("oldName", "newName"),
                "oldName", "newName", "la paire de témoin");
            both.Should().Throw<Exception>().WithMessage("*exactement un*",
                "déclarer les deux noms signale un doublon, pas un renommage.");

            Action neither = () => AssertRenamePair(produced, Declared("autreChose"),
                "oldName", "newName", "la paire de témoin");
            neither.Should().Throw<Exception>().WithMessage("*exactement un*",
                "ne déclarer aucun des deux noms signale une perte, pas un renommage.");
        }
    }
}
