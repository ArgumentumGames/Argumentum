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
    /// journalisme ; pk 174 es/pt : pays neutre = un État, pas une personne ; pk 52 es/pt : collusion
    /// orthogonale — RENVERSÉ le 2026-10-15 pour es et tranché pour pt, voir « Re-curation » ci-dessous).
    /// Une cellule vide ici est un <b>état délibéré</b>, pas une lacune.</para>
    /// <para><b>Grain de suivi ar/zh/fa (2026-10-15)</b> : les colonnes portaient 72/88/82 cellules
    /// non vides ; 30 cellules (12 ar + 8 zh + 10 fa, 13 nœuds) ont été remplies depuis les langlinks
    /// des articles sources fr/en — chaque URL sondée vivante (200, témoins non-ASCII par langue :
    /// ar:منطق, zh:逻辑, fa:منطق). Le reste est <b>sans équivalent</b> : sur les 115 sources, 28 ne sont
    /// pas Wikipedia (cairn, halshs, LSE… — pas de langlinks) et les articles wiki restants n'ont
    /// pas d'équivalent ar/zh/fa. Les partages sont légitimes (pk 104-115/120/121 pointent déjà
    /// le même قياس ar ; pk 136/138 le même تعريف) — 26 clés distinctes pour 30 fills.</para>
    /// <para><b>Grain link_fr (2026-10-15)</b> : la colonne portait 140 cellules non vides ; 41 ont été
    /// remplies depuis les langlinks fr des sources en du corpus (33 articles distincts, partages
    /// légitimes), chaque URL sondée vivante (200, témoin fr:Logique). Restent vides : **29 nœuds sans
    /// source** (têtes de famille abstraites), 11 sources en sans équivalent fr, et **2 refus épinglés**
    /// (pk 52, pk 102 — faiblesses d'origine versées au pool de re-curation ; « un lien qui pointe à
    /// côté est pire qu'une absence », #1435).</para>
    /// <para><b>Re-curation pk 52/102 (2026-10-15, pool item 2)</b> : les deux refus sont TRANCHÉS par
    /// correction du référent d'origine, pas contournés — pk 52 « Neutralité émotionnelle » : link_en
    /// pointait <c>Collusion</c> (orthogonal) → <c>en:Equanimity</c>, l'article du concept (« Emotional
    /// neutrality » n'existe pas sur en.wiki) ; pk 102 « résolution unitaire » : link_en pointait
    /// <c>Synthetic_reasoning</c> — une REDIRECTION vers « Analytic–synthetic distinction » (philosophie
    /// kantienne, autre domaine), et es/pt portaient la même dérive → <c>en:Resolution (logic)</c>, la
    /// règle d'inférence parente. 12 cellules écrites (5 + 7), langlinks sondés vivants (200, témoin par
    /// sonde). Restent vides : pk 52 pt/fa/zh (aucun langlink ; candidat pt:Equanimidade 404 mesuré),
    /// pk 102 ar (aucun langlink). Les 3 refus épinglés fr/es sont RENVERSÉS ci-dessous, motif conservé
    /// en commentaire (« l'exclusion meurt AVEC sa raison »).</para>
    /// <para><b>Grain langlinks intégral (2026-10-16, GO ai-01 c.6213886301)</b> : la mesure du
    /// 16/10 matin (« ar/fa/zh : 11-13 dérivables, le reste sans-langlink ») était <b>TRONQUÉE</b> —
    /// l'API <c>langlinks</c> coupe à 500 liens par requête et la sonde ne suivait pas
    /// <c>llcontinue</c> ; corrigée, le lot dérivable explose : <b>191 cellules</b> ar/fa/zh
    /// remplies (70+64+60 moins 3 refus, ci-dessous), chaque URL sondée vivante (200) et
    /// <b>auditée en épaisseur</b> : 2 pages fines seulement, déclarées (zh:Example 225 o pk 8).
    /// Le refus éditorial pk 174 est <b>ÉTENDU à ar/fa/zh</b> : la dérivation mécanique y pointait
    /// دولة_حيادية / بی‌طرفی (روابط بین‌الملل) / 中立國 — l'État, exactement le référent pour
    /// lequel es/pt sont épinglés vides ; « un lien qui pointe à côté est pire qu'une absence ».
    /// ⚠️ Les verdicts « sans équivalent ar/zh/fa » du grain du 2026-10-15 sont MORTS avec
    /// l'instrument qui les a produits — ne pas les citer.</para>
    /// </remarks>
    public class VirtuesLanguageLinkParityTests
    {
        private const string VirtuesCsvName = "Argumentum Virtues - Taxonomy.csv";

        /// <summary>Marqueurs d'homonymie — le filtre mécanique du grain (cible = article, jamais page d'homonymie).</summary>
        private static readonly string[] DisambiguationMarkers =
            { "(desambig", "(значения)", "(disambiguation)", "(неоднозначность)" };

        /// <summary>Nœuds du corpus Virtus au grain (mesuré 2026-10-14) — un plancher de couverture sur un CSV amputé de rangées serait vacue.</summary>
        private const int NodeCount = 223;

        /// <summary>Re-curation pk 52/102 (2026-10-15) : ru 174→176, es 172→173 (correction, pas fill).</summary>
        private const int RuLinkFloor = 176;
        private const int PtLinkFloor = 167;
        private const int EsLinkFloor = 173;

        /// <summary>Grain ar/zh/fa (2026-10-15) : mesuré 72/88/82 avant, 84/96/92 après les 30 fills langlinks ;
        /// re-curation pk 52/102 (même jour) : 84→85, 96→97, 92→93. Grain langlinks du 2026-10-16
        /// (sonde corrigée <c>llcontinue</c>, voir remarks) : 85→154, 97→156, 93→156.</summary>
        private const int ArLinkFloor = 154;
        private const int ZhLinkFloor = 156;
        private const int FaLinkFloor = 156;

        /// <summary>Grain link_fr (2026-10-15) : mesuré 140 avant, 181 après les 41 fills langlinks ;
        /// re-curation pk 52/102 (même jour) : 181→183.</summary>
        private const int FrLinkFloor = 183;

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
                ("link_ru", RuLinkFloor, 174), ("link_pt", PtLinkFloor, 167), ("link_es", EsLinkFloor, 172),
                ("link_ar", ArLinkFloor, 84), ("link_zh", ZhLinkFloor, 96), ("link_fa", FaLinkFloor, 92),
                ("link_fr", FrLinkFloor, 181),
            })
            {
                var filled = csv.LoadColumn(column).Count(v => v.Length > 0);
                filled.Should().BeGreaterThanOrEqualTo(floor,
                    because: $"états mesurés : {pre} avant le dernier grain, {floor} après (re-curation " +
                             $"pk 52/102 du 2026-10-15 comprise) — lu : {filled}. Une perte ne peut venir que " +
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
                // Grain ar/zh/fa (2026-10-15) — sondées vivantes ; le pin fa:21 porte un ZWNJ (U+200C) invisible.
                ("141", "link_ar", "https://ar.wikipedia.org/wiki/تعريف"),
                ("141", "link_zh", "https://zh.wikipedia.org/wiki/定义"),
                ("141", "link_fa", "https://fa.wikipedia.org/wiki/تعریف"),
                ("21", "link_fa", "https://fa.wikipedia.org/wiki/روزنامه‌نگاری"),
                ("70", "link_ar", "https://ar.wikipedia.org/wiki/فضاء_العينة"),
                ("177", "link_zh", "https://zh.wikipedia.org/wiki/概率"),
                ("120", "link_ar", "https://ar.wikipedia.org/wiki/قياس_(منطق)"),
                ("141", "link_fr", "https://fr.wikipedia.org/wiki/Définition"),
                ("21", "link_fr", "https://fr.wikipedia.org/wiki/Journalisme"),
                ("193", "link_fr", "https://fr.wikipedia.org/wiki/Biais_cognitif"),
                ("124", "link_fr", "https://fr.wikipedia.org/wiki/Syllogisme"),
                // Re-curation pk 52/102 (2026-10-15, pool item 2) — référents d'origine corrigés
                // (Collusion → Equanimity ; redirection Synthetic_reasoning → Resolution (logic)),
                // 12 cellules, langlinks sondés vivants (200).
                ("52", "link_en", "https://en.wikipedia.org/wiki/Equanimity"),
                ("52", "link_fr", "https://fr.wikipedia.org/wiki/Équanimité"),
                ("52", "link_es", "https://es.wikipedia.org/wiki/Ecuanimidad"),
                ("52", "link_ru", "https://ru.wikipedia.org/wiki/Хладнокровие"),
                ("52", "link_ar", "https://ar.wikipedia.org/wiki/رباطة_الجأش"),
                ("102", "link_en", "https://en.wikipedia.org/wiki/Resolution_(logic)"),
                ("102", "link_es", "https://es.wikipedia.org/wiki/Resolución_(lógica)"),
                ("102", "link_pt", "https://pt.wikipedia.org/wiki/Princípio_da_resolução"),
                ("102", "link_fr", "https://fr.wikipedia.org/wiki/Règle_de_résolution"),
                ("102", "link_ru", "https://ru.wikipedia.org/wiki/Правило_резолюций"),
                ("102", "link_fa", "https://fa.wikipedia.org/wiki/رزولوشن_(منطق)"),
                ("102", "link_zh", "https://zh.wikipedia.org/wiki/归结原理"),
                // Grain langlinks intégral (2026-10-16, sonde llcontinue corrigée) — 191 fills,
                // sondés vivants (200) ; échantillon épinglé, convention Unicode brut conservée.
                ("0", "link_ar", "https://ar.wikipedia.org/wiki/حجة_(منطق)"),
                ("0", "link_fa", "https://fa.wikipedia.org/wiki/برهان_(منطق)"),
                ("0", "link_zh", "https://zh.wikipedia.org/wiki/逻辑论证"),
                ("1", "link_ar", "https://ar.wikipedia.org/wiki/نظرية_الحجج"),
                ("6", "link_zh", "https://zh.wikipedia.org/wiki/證據"),
                ("10", "link_zh", "https://zh.wikipedia.org/wiki/參考文獻"),
                ("23", "link_ar", "https://ar.wikipedia.org/wiki/دليل_تجريبي"),
                ("23", "link_fa", "https://fa.wikipedia.org/wiki/شواهد_تجربی"),
                ("55", "link_zh", "https://zh.wikipedia.org/wiki/沟通"),
                ("59", "link_ar", "https://ar.wikipedia.org/wiki/حساب_(مهارة)"),
                ("63", "link_ar", "https://ar.wikipedia.org/wiki/الحجة_من_القياس"),
                ("124", "link_zh", "https://zh.wikipedia.org/wiki/三段論"),
                ("129", "link_zh", "https://zh.wikipedia.org/wiki/理智"),
                ("190", "link_zh", "https://zh.wikipedia.org/wiki/積極聆聽"),
                ("205", "link_zh", "https://zh.wikipedia.org/wiki/寬容原則"),
                ("8", "link_zh", "https://zh.wikipedia.org/wiki/Example"), // page FINE (225 o) :
                // le langlink mécanique de en:Example — vivante, même référent, déclarée au PR.
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
                ("174", "link_es", "País neutral = un État, pas une personne"),
                ("174", "link_pt", "País neutro = un État, pas une personne"),
                // VERDICTS RENVERSÉS le 2026-10-15 (re-curation, pool item 2) — motifs d'origine conservés :
                // pk 52 link_es « Colusión orthogonal au nœud » → REMPLI (Ecuanimidad) : le refus tenait au
                //   référent fautif Collusion, corrigé à la source (link_en → Equanimity) ;
                // pk 52 link_fr « link_en = Collusion, faiblesse d'origine » → REMPLI (Équanimité), même
                //   tranchement ;
                // pk 102 link_fr « link_en = Distinction analytique-synthétique » → REMPLI (Règle de
                //   résolution) : la source était une REDIRECTION Synthetic_reasoning vers la distinction
                //   kantienne, domaine étranger à la résolution unitaire.
                ("52", "link_pt", "aucun équivalent pt : en:Equanimity sans langlink pt, candidat direct pt:Equanimidade 404 mesuré (2026-10-15)"),
                // Grain 2026-10-16 — refus ÉTENDU : la sonde corrigée TROUVE des langlinks ar/fa/zh
                // pour pk 174, mais ils pointent l'État (中性國 = pays neutre), le référent refusé
                // en es/pt dès le 2026-10-14. La mécanique ne prime pas sur le verdict éditorial.
                ("174", "link_ar", "دولة_حيادية = un État, pas une personne (refus es/pt étendu)"),
                ("174", "link_fa", "بی‌طرفی (روابط بین‌الملل) = relations internationales, pas une personne"),
                ("174", "link_zh", "中立國 = pays neutre, pas une personne"),
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
