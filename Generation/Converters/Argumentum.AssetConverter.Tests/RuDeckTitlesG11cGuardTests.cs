using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Garde ⑪c du pool v22 (#458 c.5868537788, arbitrage ai-01 du 28/09) : les 23 titres
    /// russes du deck arbitrés par ai-01, les bandeaux russes qui les suivent, et les
    /// DEUX titres gardés (miroirs du français) épinglés eux aussi — sans cela, une passe
    /// future pourrait « corriger » ce que l'arbitrage a délibérément gardé (Règle C).
    ///
    /// Critère d'arbitrage : on corrige quand le titre russe dit autre chose que le
    /// français et l'anglais (non-sens, contresens, moitié du concept perdue) ; on garde
    /// un miroir du français ou une traduction défendable. Fichier distinct des gardes
    /// précédentes pour éviter la collision avec les PR CSV parallèles.
    ///
    /// ⚠️ Ces 23 titres sont imprimés (deck) : ils ne sont servis qu'à la passe n°5,
    /// regroupés avec les résultats des mesures ⑫-⑮.
    /// </summary>
    public class RuDeckTitlesG11cGuardTests
    {
        private static string FallaciesCsv => Path.Combine(
            TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

        private static string CellOf(string path, string column)
        {
            var csv = new HarvestCardIdsCsv(FallaciesCsv);
            var values = csv.LoadColumn(column, "path", new[] { path });
            values.Should().HaveCount(1, "une seule rangée porte ce path.");
            return values[0].Trim();
        }

        /// <summary>Titres text_ru des rangées du deck (carte non vide : 175 attendues).</summary>
        private static IReadOnlyList<string> DeckRuTitles()
        {
            var csv = new HarvestCardIdsCsv(FallaciesCsv);
            return csv.LoadColumn("text_ru", "carte", new[] { "1", "2" });
        }

        public static TheoryData<string, string, string> ArbitratedTitles => new()
        {
            // path, nouvelle valeur, raison (arbitrage ai-01)
            { "7.2.1.2.2", "Защита Чубакки", "forme établie (ru.wiki, Meduza) ; l'ancien « Компостирование рыбы » = « compostage du poisson », non-sens" },
            { "1.2.1", "Апелляция к авторитету", "forme établie ; les rangées sœurs utilisent déjà « Апелляция к… »" },
            { "1.2.1.2", "Апелляция к уважению", "même famille que 1.2.1" },
            { "1.3.2.1", "Ошибка ошибки", "fallacist's fallacy (Digitable #67) ; l'ancien « Софизм » était dégénéré" },
            { "2.1.1.4", "Клише, пресекающее мысль", "terme de Lifton établi (Digitable #51)" },
            { "3.1.2", "Софизм акциденции", "correction minimale : « авария » (accident de la route) devient le terme philosophique, le reste du titre est gardé" },
            { "3.2.2.3.1", "Ошибка горячей руки", "massivement établi (hot hand fallacy)" },
            { "4.3.2", "Противоречивость", "la desc_ru de la rangée écrit elle-même « противоречивые гипотезы »" },
            { "5.1.3", "Противоречивое определение", "suit 4.3.2 (dérive « несостоятельн- » ×3)" },
            { "5.2.1.3", "Противоречивое сравнение", "suit 4.3.2" },
            { "4.3.1.2", "Ложная эквивалентность", "nomme le sophisme, pas l'objet (Digitable #39)" },
            { "6.2.1.4", "Перенос бремени доказательства", "forme juridique établie (onus probandi)" },
            { "6.2.3.4", "Ошибка невозвратных затрат", "massivement établi (sunk cost)" },
            { "2.3.1.1.1.2", "Приманка и подмена", "l'ancien ne gardait que l'appât, la substitution était perdue" },
            { "6.2.3.3", "Аргумент заметного усилия", "même choix qu'en portugais (#1606)" },
            { "7.3.2.1.1", "Reductio ad Hitlerum", "latin, comme le français, l'anglais et le portugais ; le deck ru porte déjà du latin (« Спасение ad hoc »)" },
            { "4.2.2", "Ошибка квантификации", "le concept est la logique des quantificateurs, pas les chiffres" },
            { "4.2.1", "Ошибка логики высказываний", "« в предложении » (dans une phrase) au lieu du terme établi « логика высказываний »" },
            { "3.1.1", "Смещённая выборка", "terme statistique établi ; l'ancien était descriptif et faible" },
            { "6.1.1.1.3.3", "Мысленная оговорка", "terme théologique établi (mental reservation)" },
            { "1", "Недостаток", "la seule minuscule initiale du deck ru — casse, toujours corriger" },
            { "6.1.2", "Ложная атрибуция", "coquille : « аттрибуция » n'existe pas (un seul т)" },
            { "5.2.1", "Злоупотребление сравнением", "coquille : « злопотребление » n'est pas un mot russe" },
        };

        [Theory, MemberData(nameof(ArbitratedTitles))]
        public void G11c_Title_Pinned(string path, string expected, string reason)
        {
            CellOf(path, "text_ru").Should().Be(expected,
                $"⑪c ({path}) : {reason}.");
        }

        [Fact]
        public void Kept_Mirror_Titles_Pinned()
        {
            // Arbitrage Règle C : gardés comme miroirs exacts du français — épinglés pour
            // qu'aucune passe future ne les « corrige » sans nouvel arbitrage.
            CellOf("4.3.2.2.1", "text_ru").Should().Be("Логика котла",
                "gardé (ai-01) : miroir exact de « Logique du chaudron », bien que « Логика чайника » soit la forme ru.wiki.");
            CellOf("6.2", "text_ru").Should().Be("Изменение направления",
                "gardé (ai-01) : miroir exact de « Changement de cap ».");
        }

        [Fact]
        public void Removed_G11c_Forms_Absent_From_Deck()
        {
            // Comparaison de VALEUR ENTIÈRE sur les titres du deck : « Приманка » et
            // « Умственное удержание » survivent hors deck (2.3.2.2.1.4 « Leurre »,
            // 7.2.1.2.1.1 « Restriction mentale ») — hors deck = jamais imprimé.
            var deck = DeckRuTitles();
            deck.Should().HaveCount(175, "le deck ru compte 175 cartes (#1288).");
            string[] banned =
            {
                "Компостирование рыбы", "Воззвание к авторитету", "Воззвание к уважению",
                "Софизм", "Штамп, не приемлющий критики", "Софизм аварии",
                "Разогретые мышцы", "Несостоятельность", "Несостоятельное определение",
                "Несостоятельное сравнение", "Ложный эквивалент", "Переворот доказательств",
                "Логическая ошибка затонувших издержек", "Приманка", "Заметное усилие",
                "Карта Гитлера", "Количественная ошибка", "Логическая ошибка в предложении",
                "Пример с искажениями", "Умственное удержание", "недостаток",
                "Ложная аттрибуция", "Злопотребление сравнением",
            };
            foreach (var old in banned)
                deck.Should().NotContain(old,
                    $"forme arbitrée hors du deck ⑪c : « {old} ».");
        }

        [Fact]
        public void G11c_Banners_Follow_The_Arbitrated_Titles()
        {
            // La cascade a écrit 277 cellules de bandeau (267 descendants + 10 bandeaux
            // propres des rangées de profondeur ≤ 3). Échantillon épinglé — l'invariant
            // systématique reste porté par FallaciesDeckBandInvariantGuardTests (④c).
            CellOf("1", "Family_ru").Should().Be("Недостаток",
                "bandeau propre (niveau 1) : la racine porte son nouveau titre.");
            CellOf("1.2.1", "Subsubfamily_ru").Should().Be("Апелляция к авторитету",
                "bandeau propre (niveau 3).");
            CellOf("1.2.1.1", "Subsubfamily_ru").Should().Be("Апелляция к авторитету",
                "descendant : le bandeau suit l'ancêtre arbitré.");
            CellOf("4.2.2.1", "Subsubfamily_ru").Should().Be("Ошибка квантификации",
                "descendant de 4.2.2.");
            CellOf("3.1.1.1.1", "Subsubfamily_ru").Should().Be("Смещённая выборка",
                "descendant de 3.1.1 (rangée de deck).");
        }

        [Fact]
        public void Witnesses_Kept_CNotes_Unchanged()
        {
            // C-notes 7 et 14 de la mesure ⑪ : traductions défendables, gardées par
            // l'arbitrage — la passe ⑪c ne les a pas touchées.
            CellOf("7.1.3.3", "text_ru").Should().Be("До отвращения",
                "C-note 7 gardée (forme figée « до тошноты » signalée, non arbitrée).");
            CellOf("2.2.2.4", "text_ru").Should().Be("Аргумент к ужасу",
                "C-note 14 gardée (« ужас » justifiable par le fr « terreur »).");
        }
    }
}
