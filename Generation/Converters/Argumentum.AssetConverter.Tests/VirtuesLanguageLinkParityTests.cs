using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Organe de parité linguistique des liens Virtus (#458 grain « Vertus ru/pt/es », 2026-10-14).
    /// Avant ce grain, les colonnes <c>link_ru</c>/<c>link_pt</c>/<c>link_es</c> portaient 143/141/125
    /// cellules non vides ; 104 cellules vides ont été remplies depuis les langlinks Wikipedia des
    /// articles sources du corpus (cibles curatées : 10 refus éditoriaux documentés, homonymies
    /// exclues mécaniquement), chaque URL sondée vivante (redirections suivies, 3 retries, témoins
    /// ASCII <c>fr:Sophisme</c> **et non-ASCII** <c>ru:Философия</c> fail-closed — le premier passage
    /// a mesuré 63 « mortes » qui étaient toutes des URL non-ASCII que le transport ne savait pas
    /// encoder, cf. leçon « un témoin partage la surface de panne de ce qu'il garde »).
    /// </summary>
    /// <remarks>
    /// <para><b>Trois portes</b> : (1) planchers de couverture par langue aux valeurs mesurées
    /// post-grain — toute perte reddît ; (2) aucun marqueur d'homonymie dans les <c>link_*</c> du
    /// corpus, avec témoin positif synthétique (un détecteur incapable de voir le cas positif n'a
    /// aucun compte à rendre) ; (3) échantillon de cellules épinglées — liens sourcés exacts et
    /// refus éditoriaux épinglés <b>vides</b> (« un lien qui pointe à côté est pire qu'une absence »,
    /// discipline #1435).</para>
    /// <para><b>Refus éditoriaux épinglés vides</b> : homonymies mécaniques (pk 9/13/14 en pt/ru)
    /// et hors-sujets sémantiques (pk 15 es : fiabilité des systèmes = génie pour un nœud de
    /// journalisme ; pk 52 es/pt : collusion orthogonale au nœud ; pk 174 es/pt : pays neutre =
    /// un État, pas une personne). Une cellule vide ici est un <b>état délibéré</b>, pas une lacune.</para>
    /// </remarks>
    public class VirtuesLanguageLinkParityTests
    {
        private const string VirtuesCsvName = "Argumentum Virtues - Taxonomy.csv";

        /// <summary>Marqueurs d'homonymie — le filtre mécanique du grain (cible = article, jamais page d'homonymie).</summary>
        private static readonly string[] DisambiguationMarkers =
            { "(desambig", "(значения)", "(disambiguation)", "(неоднозначность)" };

        /// <summary>Nœuds du corpus Virtus au grain (mesuré 2026-10-14) — un plancher de couverture sur un CSV amputé de rangées serait vacue.</summary>
        private const int NodeCount = 223;

        private const int RuLinkFloor = 174;
        private const int PtLinkFloor = 167;
        private const int EsLinkFloor = 172;

        private static string VirtuesCsv => Path.Combine(
            TestRepoRoot.Find(), "Cards", "Fallacies", VirtuesCsvName);

        [Fact]
        public void NonEmptyLinkCoveragePerLanguage_MeetsMeasuredFloors()
        {
            var csv = new HarvestCardIdsCsv(VirtuesCsv);
            var pks = csv.LoadColumn("pk");
            pks.Count.Should().Be(NodeCount,
                because: "le corpus Virtus porte 223 nœuds au grain ru/pt/es (mesuré 2026-10-14) ; " +
                         "un compte moindre ampute silencieusement les planchers ci-dessous");

            foreach (var (column, floor, pre) in new[]
            {
                ("link_ru", RuLinkFloor, 143), ("link_pt", PtLinkFloor, 141), ("link_es", EsLinkFloor, 125),
            })
            {
                var filled = csv.LoadColumn(column).Count(v => v.Length > 0);
                filled.Should().BeGreaterThanOrEqualTo(floor,
                    because: $"états mesurés : {pre} avant le grain (pré-remplissage, backup du 2026-10-14), " +
                             $"{floor} après les fills langlinks — lu : {filled}. Une perte ne peut venir que " +
                             "d'une vidange ou d'un rollback partiel, jamais d'un ajout");
            }
        }

        [Fact]
        public void NoLinkCellCarriesADisambiguationMarker()
        {
            var offenders = LoadAllLinkCells()
                .Where(c => DisambiguationMarkers.Any(m => c.Value.Contains(m, StringComparison.Ordinal)))
                .ToList();

            offenders.Should().BeEmpty(
                because: "une page d'homonymie n'est pas un article : le lien doit viser la cible curatée, " +
                         "pas la page de désambiguïsation. Contrevenantes : " +
                         $"{string.Join(", ", offenders.Take(3).Select(FormatCell))}{(offenders.Count > 3 ? " …" : "")}");
        }

        /// <summary>
        /// Témoin positif du détecteur d'homonymie : <see cref="NoLinkCellCarriesADisambiguationMarker"/>
        /// lit 0 sur le corpus — ce fait prouve que le 0 est un état mesuré, pas un détecteur muet
        /// (les quatre marqueurs, un par famille linguistique, doivent chacun être attrapé).
        /// </summary>
        [Fact]
        public void DisambiguationDetector_FlagsEverySyntheticPositive()
        {
            var synthetic = new List<string>
            {
                "https://pt.wikipedia.org/wiki/Evidência_(desambiguação)",
                "https://ru.wikipedia.org/wiki/Разум_(значения)",
                "https://en.wikipedia.org/wiki/Reason_(disambiguation)",
                "https://ru.wikipedia.org/wiki/Разум_(неоднозначность)",
                "https://fr.wikipedia.org/wiki/Raison", // propre : ne doit PAS être attrapée
            };

            var flagged = synthetic.Where(u => DisambiguationMarkers.Any(m => u.Contains(m, StringComparison.Ordinal))).ToList();

            flagged.Should().BeEquivalentTo(synthetic.Take(4),
                because: "les 4 marqueurs (pt/ru×2/en) doivent chacun être détectés sur des témoin synthétiques, " +
                         "et la 5e URL propre rester hors du lot — sinon le 0 du corpus mesure un détecteur mort");
        }

        [Fact]
        public void PinnedCells_CarryTheirSourcedUrlOrStayDeliberatelyEmpty()
        {
            var csv = new HarvestCardIdsCsv(VirtuesCsv);
            var pks = csv.LoadColumn("pk").ToList();

            string Cell(string pk, string column) => csv.LoadColumn(column)[pks.IndexOf(pk)];

            // Liens sourcés : échantillon exact du grain (URL brutes — cyrillique/accents non encodés, convention corpus).
            var pinnedUrls = new[]
            {
                ("59", "link_ru", "https://ru.wikipedia.org/wiki/Числовая_грамотность"),
                ("59", "link_pt", "https://pt.wikipedia.org/wiki/Numeracia"),
                ("70", "link_pt", "https://pt.wikipedia.org/wiki/Espaço_amostral"),
                ("63", "link_es", "https://es.wikipedia.org/wiki/Razonamiento_por_analogía"),
                ("23", "link_es", "https://es.wikipedia.org/wiki/Evidencia_empírica"),
                ("23", "link_pt", "https://pt.wikipedia.org/wiki/Evidência_empírica"),
                ("23", "link_ru", "https://ru.wikipedia.org/wiki/Эмпирические_данные"),
                ("171", "link_es", "https://es.wikipedia.org/wiki/Competencia_intercultural"),
                ("204", "link_ru", "https://ru.wikipedia.org/wiki/Управление_временем"),
            };
            foreach (var (pk, column, url) in pinnedUrls)
            {
                Cell(pk, column).Should().Be(url,
                    because: $"pin exact du grain ru/pt/es (pk {pk}, sondée vivante le 2026-10-14)");
            }

            // Refus éditoriaux : épinglés VIDES — un remplissage ici doit être une décision délibérée, pas un diff perdu.
            var pinnedEmpties = new[]
            {
                ("9", "link_pt", "page d'homonymie (desambiguação)"),
                ("13", "link_pt", "page d'homonymie (desambiguação)"),
                ("13", "link_ru", "page d'homonymie (значения)"),
                ("14", "link_pt", "page d'homonymie (desambiguação)"),
                ("14", "link_ru", "page d'homonymie (значения)"),
                ("15", "link_es", "Fiabilidad de sistemas = génie, nœud = journalisme"),
                ("52", "link_es", "Colusión orthogonal au nœud"),
                ("52", "link_pt", "Conluio orthogonal au nœud"),
                ("174", "link_es", "País neutral = un État, pas une personne"),
                ("174", "link_pt", "País neutro = un État, pas une personne"),
            };
            foreach (var (pk, column, reason) in pinnedEmpties)
            {
                Cell(pk, column).Should().BeEmpty(
                    because: $"refus éditorial délibéré du grain (pk {pk} : {reason}) — « un lien qui pointe à côté " +
                             "est pire qu'une absence » (#1435) ; remplir cette cellule exige de trancher le refus, pas de le contourner");
            }
        }

        /// <summary>Toutes les cellules <c>link*</c> non vides, découvertes depuis l'en-tête (même règle que la garde census).</summary>
        private static List<(string Pk, string Column, string Value)> LoadAllLinkCells()
        {
            var csv = new HarvestCardIdsCsv(VirtuesCsv);
            var linkColumns = csv.ReadHeader()
                .Where(h => h == "link" || h.StartsWith("link_", StringComparison.Ordinal))
                .ToList();
            linkColumns.Should().NotBeEmpty(because: "le CSV Virtus doit porter ses colonnes link_*");

            var pks = csv.LoadColumn("pk");
            var cells = new List<(string, string, string)>();
            foreach (var column in linkColumns)
            {
                var values = csv.LoadColumn(column);
                for (var i = 0; i < values.Count; i++)
                {
                    if (values[i].Length > 0)
                    {
                        cells.Add((pks[i], column, values[i]));
                    }
                }
            }

            return cells;
        }

        private static string FormatCell((string Pk, string Column, string Value) cell) =>
            $"pk {cell.Pk}/{cell.Column}: {cell.Value}";
    }
}
