using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Organe de couverture des liens Fallacies (#458 grain langlinks, 2026-10-16, GO ai-01
    /// c.6213886301) — pendant de <see cref="VirtuesLanguageLinkParityTests"/> pour le corpus
    /// sophismes. Le grain a rempli <b>23 cellules</b> du deck (colonne <c>carte</c>) en
    /// ru/pt/es/ar/fa/zh depuis les langlinks des articles <c>link_en</c> déjà curatés.
    /// </summary>
    /// <remarks>
    /// <para><b>Leçon d'instrument (fondatrice du grain)</b> : la mesure du 16/10 matin lisait
    /// « 1-4 dérivables par langue » parce que l'API <c>langlinks</c> coupe à <b>500 liens par
    /// requête</b> et que la sonde ne suivait pas <c>llcontinue</c> — les langlinks au-delà du
    /// seuil disparaissaissent en silence (témoin : pk 973 « Moving the goalposts », zh présent
    /// en direct, absent du batch tronqué). Corrigée, la mesure rend le vrai lot. Toute sonde
    /// langlinks qui n'affiche pas avoir épuisé <c>continue</c> est réputée muette.</para>
    /// <para><b>Conventions mesurées</b> : le corpus fallacies encode ses URLs en
    /// percent-encoding (<c>Fal%C3%A1cia</c>, apostrophe <c>%27</c>, parenthèses <c>%28/%29</c>,
    /// espaces en underscores) — les fills suivent la convention dominante de LEUR colonne,
    /// mesurée avant écriture (202/202 pct en ar, 169/169 en fa, 193/194 en zh à la mesure).
    /// Les planchers ci-dessous portent la colonne ENTIÈRE (1408 rangées) ; les fills du grain
    /// ne touchaient que le deck (175), le hors-deck reste à couvrir.</para>
    /// <para><b>Déclaré au PR</b> : pk 758 <c>es:Non sequitur</c> est une page FINE (325 o) —
    /// langlink mécanique de la source, vivant (200), même référent ; conservé et signalé.</para>
    /// </remarks>
    public class FallaciesLanguageLinkParityTests
    {
        private const string FallaciesCsvName = "Argumentum Fallacies - Taxonomy.csv";

        /// <summary>Rangées du corpus au grain (mesuré 2026-10-16) — un plancher sur un CSV amputé serait vacue.</summary>
        private const int NodeCount = 1408;

        /// <summary>Grain langlinks du 2026-10-16 : mesuré avant le grain 202/169/194 (ar/fa/zh),
        /// 162/181/190 (ru/pt/es) ; après les 23 fills du deck : valeurs ci-dessous.</summary>
        private const int RuLinkFloor = 164;
        private const int PtLinkFloor = 184;
        private const int EsLinkFloor = 197;
        private const int ArLinkFloor = 204;
        private const int FaLinkFloor = 172;
        private const int ZhLinkFloor = 200;

        private static string FallaciesCsv => Path.Combine(
            TestRepoRoot.Find(), "Cards", "Fallacies", FallaciesCsvName);

        [Fact]
        public void NonEmptyLinkCoveragePerLanguage_MeetsMeasuredFloors()
        {
            var csv = new HarvestCardIdsCsv(FallaciesCsv);
            var pks = csv.LoadColumn("PK");
            pks.Count.Should().Be(NodeCount,
                because: "le corpus Fallacies porte 1408 rangées au grain langlinks (mesuré 2026-10-16) ; " +
                         "un compte moindre ampute silencieusement les planchers ci-dessous");

            foreach (var (column, floor, pre) in new[]
            {
                ("link_ru", RuLinkFloor, 162), ("link_pt", PtLinkFloor, 181), ("link_es", EsLinkFloor, 190),
                ("link_ar", ArLinkFloor, 202), ("link_fa", FaLinkFloor, 169), ("link_zh", ZhLinkFloor, 194),
            })
            {
                var filled = csv.LoadColumn(column).Count(v => v.Length > 0);
                filled.Should().BeGreaterThanOrEqualTo(floor,
                    because: $"états mesurés : {pre} avant le grain langlinks du 2026-10-16, {floor} après — " +
                             $"lu : {filled}. Une perte ne peut venir que d'une vidange ou d'un rollback partiel");
            }
        }

        [Fact]
        public void PinnedCells_CarryTheirSourcedUrl()
        {
            var csv = new HarvestCardIdsCsv(FallaciesCsv);
            var pks = csv.LoadColumn("PK").ToList();

            string Cell(string pk, string column) => csv.LoadColumn(column)[pks.IndexOf(pk)];

            // Échantillon exact du grain (URL percent-encodées, convention du corpus fallacies).
            // pk 358 porte les SIX langues du GO — le cas multi-langue le plus large ;
            // pk 973 zh est le TÉMOIN de la troncature llcontinue (absent du batch du 16/10 matin).
            var pinnedUrls = new[]
            {
                ("358", "link_ru", "https://ru.wikipedia.org/wiki/%D0%A4%D1%80%D0%B5%D0%B9%D0%BC_%28%D1%81%D0%BE%D1%86%D0%B8%D0%B0%D0%BB%D1%8C%D0%BD%D1%8B%D0%B5_%D0%BD%D0%B0%D1%83%D0%BA%D0%B8%29"),
                ("358", "link_pt", "https://pt.wikipedia.org/wiki/Enquadramento_noticioso"),
                ("358", "link_es", "https://es.wikipedia.org/wiki/Encuadre_%28ciencias_sociales%29"),
                ("358", "link_ar", "https://ar.wikipedia.org/wiki/%D8%AA%D8%A3%D8%B7%D9%8A%D8%B1"),
                ("358", "link_zh", "https://zh.wikipedia.org/wiki/%E6%A1%86%E6%9E%B6_%28%E7%A4%BE%E6%9C%83%E7%A7%91%E5%AD%B8%29"),
                ("973", "link_zh", "https://zh.wikipedia.org/wiki/%E6%8C%AA%E5%8B%95%E9%96%80%E6%9F%B1"),
                ("1024", "link_es", "https://es.wikipedia.org/wiki/%C3%8Ddolos_de_la_tribu"),
                ("1174", "link_es", "https://es.wikipedia.org/wiki/%C3%8Ddolos_de_la_caverna"),
                ("614", "link_pt", "https://pt.wikipedia.org/wiki/Dicto_simpliciter"),
                ("614", "link_zh", "https://zh.wikipedia.org/wiki/%E7%B1%A0%E7%B5%B1%E6%8E%A8%E8%AB%96"),
                ("839", "link_zh", "https://zh.wikipedia.org/wiki/%E7%B1%BB%E6%8E%A8%E8%AE%BA%E8%AF%81"),
                ("1357", "link_ru", "https://ru.wikipedia.org/wiki/%D0%98%D1%81%D0%BA%D1%83%D1%81%D1%81%D1%82%D0%B2%D0%BE_%D0%BF%D0%BE%D0%B1%D0%B5%D0%B6%D0%B4%D0%B0%D1%82%D1%8C_%D0%B2_%D1%81%D0%BF%D0%BE%D1%80%D0%B0%D1%85"),
                ("1357", "link_es", "https://es.wikipedia.org/wiki/Dial%C3%A9ctica_er%C3%ADstica_o_el_arte_de_tener_raz%C3%B3n%2C_expuesta_en_38_estratagemas"),
                ("1297", "link_es", "https://es.wikipedia.org/wiki/Prueba_por_aseveraci%C3%B3n"),
                ("758", "link_es", "https://es.wikipedia.org/wiki/Non_sequitur"), // page FINE 325 o, déclarée
                ("989", "link_zh", "https://zh.wikipedia.org/wiki/%E8%88%89%E8%AD%89%E8%B2%AC%E4%BB%BB_%28%E5%93%B2%E5%AD%B8%29"),
            };
            foreach (var (pk, column, url) in pinnedUrls)
            {
                Cell(pk, column).Should().Be(url,
                    because: $"pin exact du grain langlinks du 2026-10-16 (pk {pk}, URL sondée vivante)");
            }
        }
    }
}
