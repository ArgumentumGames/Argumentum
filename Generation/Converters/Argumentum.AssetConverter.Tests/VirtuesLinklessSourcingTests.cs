using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Épinglage du sourcing éditorial des 8 nœuds Vertus devenus sans lien au burn-down #1851
    /// (pool #458, dispatch ai-01 c.6096790964 « grain de sourcing éditorial » et c.6143960857 ;
    /// worklist fondatrice : <c>docs/corpus/458-v23-g40-virtues-wikipedia-deadlinks-2026-10-10.md</c> §8
    /// — « 8 nodes newly keep no link (23, 59, 63, 70, 119, 171, 203, 204) »).
    ///
    /// Le grain (14/10/2026, po-2024) remplit 7 nœuds — 16 cellules, toutes VIDES avant
    /// (remplissage, jamais écrasement), 10 URL nouvelles sondées vivantes + 6 déjà recensées —
    /// et laisse 2 vides JUSTIFIÉS nommément :
    /// <list type="bullet">
    /// <item><b>PK 203 « Concentration sur l'essentiel »</b> : compétence de facilitation du débat,
    /// aucun article encyclopédique ne couvre le nœud tel que défini. « Pareto principle » vit mais
    /// a été REFUSÉ comme approximatif — un lien qui pointe à côté est pire qu'une absence, il est
    /// cliqué (discipline #1435).</item>
    /// <item><b>PK 59 « Sens quantitatif » link_fr</b> : aucun article fr correspondant
    /// (« Raisonnement quantitatif » = 404 mesuré) ; link_en = Numeracy, l'article qui couvre le
    /// concept (en l'absence d'article « Quantitative reasoning », 404 mesuré).</item>
    /// </list>
    ///
    /// Les attendus sont INSCRITS ici : toute dérive du CSV fait rouge et NOMME la cellule.
    /// La couverture census de chaque URL reste le travail de <see cref="VirtuesWikipediaLinkCensusGuardTests"/>.
    /// </summary>
    public class VirtuesLinklessSourcingTests
    {
        private static readonly (string pk, string column, string expected)[] Filled =
        {
            ("23", "link_fr", "https://fr.wikipedia.org/wiki/Preuve_empirique"),
            ("23", "link_en", "https://en.wikipedia.org/wiki/Empirical_evidence"),
            ("59", "link_en", "https://en.wikipedia.org/wiki/Numeracy"),
            ("63", "link_fr", "https://fr.wikipedia.org/wiki/Analogie"),
            ("63", "link_en", "https://en.wikipedia.org/wiki/Argument_from_analogy"),
            ("70", "link_fr", "https://fr.wikipedia.org/wiki/Univers_(probabilit%C3%A9s)"),
            ("70", "link_en", "https://en.wikipedia.org/wiki/Sample_space"),
            // PK 119 Datisi : copie conforme de la rangée du mode voisin pk 118 (Disamis) —
            // mêmes ancres « variantes du syllogisme standard », URLs déjà recensées vivantes.
            ("119", "link_fr", "https://fr.wikipedia.org/wiki/Syllogisme#Les_variantes_du_syllogisme_standard"),
            ("119", "link_en", "https://en.wikipedia.org/wiki/Syllogism#Standard-form_syllogisms"),
            ("119", "link_ru", "https://ru.wikipedia.org/wiki/Силлогизм#Варианты_стандартного_силлогизма"),
            ("119", "link_pt", "https://pt.wikipedia.org/wiki/Silogismo"),
            ("119", "link_es", "https://es.wikipedia.org/wiki/Silogismo"),
            ("171", "link_fr", "https://fr.wikipedia.org/wiki/Comp%C3%A9tence_interculturelle"),
            ("171", "link_en", "https://en.wikipedia.org/wiki/Intercultural_competence"),
            ("204", "link_fr", "https://fr.wikipedia.org/wiki/Gestion_du_temps"),
            ("204", "link_en", "https://en.wikipedia.org/wiki/Time_management"),
        };

        private static readonly (string pk, string column, string reason)[] JustifiedEmpty =
        {
            ("59", "link_fr", "aucun article fr correspondant (404 mesuré) — Numeracy couvre link_en seulement"),
            ("203", "link_fr", "compétence de facilitation, aucun référent encyclopédique (Pareto refusé comme approximatif)"),
            ("203", "link_en", "compétence de facilitation, aucun référent encyclopédique (Pareto refusé comme approximatif)"),
        };

        private static string VirtuesCsv => Path.Combine(
            TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

        [Fact]
        public void SourcedNodes_CarryTheirPinnedLinks()
        {
            var csv = new HarvestCardIdsCsv(VirtuesCsv);
            csv.ReadHeader();
            var pks = csv.LoadColumn("pk");

            foreach (var (pk, column, expected) in Filled)
            {
                var values = csv.LoadColumn(column);
                var i = pks.ToList().IndexOf(pk);
                i.Should().BeGreaterThanOrEqualTo(0, "le pk {0} doit exister dans le CSV Virtues", pk);
                values[i].Should().Be(expected,
                    "pk {0}/{1} porte le lien sourcé du grain #458 (14/10) — toute dérive doit être délibérée",
                    pk, column);
            }
        }

        [Fact]
        public void JustifiedEmpties_StayEmptyUntilADeliberateDecision()
        {
            var csv = new HarvestCardIdsCsv(VirtuesCsv);
            csv.ReadHeader();
            var pks = csv.LoadColumn("pk");

            foreach (var (pk, column, reason) in JustifiedEmpty)
            {
                var values = csv.LoadColumn(column);
                var i = pks.ToList().IndexOf(pk);
                i.Should().BeGreaterThanOrEqualTo(0, "le pk {0} doit exister dans le CSV Virtues", pk);
                values[i].Should().BeEmpty(
                    "pk {0}/{1} est un vide JUSTIFIÉ du grain #458 : {2}. Le remplir exige une décision " +
                    "éditoriale nommée, pas un lien approximatif pour faire monter un pourcentage",
                    pk, column, reason);
            }
        }

        [Fact]
        public void TheEightWorklistNodes_AreAllAccountedFor()
        {
            // La worklist §8 du doc #1851 fait 8 pks ; le grain rend un verdict pour chacun :
            // 7 sourcés, 1 vide justifié sur les DEUX colonnes (203). Un 9e pk qui deviendrait
            // sans lien n'appartient pas à ce grain — il doit être trié à part, pas silencieusement
            // absorbé ici.
            var csv = new HarvestCardIdsCsv(VirtuesCsv);
            csv.ReadHeader();
            var pks = csv.LoadColumn("pk");
            var worklist = new HashSet<string> { "23", "59", "63", "70", "119", "171", "203", "204" };
            pks.Where(p => worklist.Contains(p)).Should().HaveCount(8,
                "la worklist §8 de #1851 doit rester intégrale dans le CSV");

            var accounted = Filled.Select(f => f.pk).Concat(JustifiedEmpty.Select(j => j.pk)).ToHashSet();
            accounted.Should().BeEquivalentTo(worklist,
                "chaque pk de la worklist a un verdict : lien épinglé ou vide justifié nommé");
        }
    }
}
