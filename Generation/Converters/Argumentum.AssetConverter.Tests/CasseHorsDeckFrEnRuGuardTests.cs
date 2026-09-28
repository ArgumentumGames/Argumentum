using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde ⑩ du pool v22 (#458 c.5857960866 item 4) : casse de phrase hors deck
	/// fr/en/ru, garde dans les DEUX sens (excès de majuscules ET noms propres
	/// préservés). Fichier distinct de CassePhraseGuardTests (⑤, master) pour éviter
	/// la collision avec les PR CSV parallèles.
	///
	/// Sens 1 (excès) : balayage complet fr + ru — tout mot majuscule après le
	/// premier, hors liste de noms propres, est rouge. Pour EN le balayage complet
	/// attendra le merge de #1624 (⑨) : cinq retardataires EN lui appartiennent et
	/// rendraient le balayage rouge sur cet arbre — en attendant, épinglage positif
	/// des 7 cellules ⑩ + absence des formes retirées.
	///
	/// Sens 2 (noms propres) : les majuscules légitimes sont épinglées valeur exacte
	/// — un balayage de minusculation excessif les tuerait. Règle fr (dispatch) :
	/// gardent la majuscule le nom d'habitant (« vrai Écossais »), l'astre (« la
	/// Lune ») et les noms de personnes (« méthode Coué », « effet Barnum »).
	/// </summary>
	public class CasseHorsDeckFrEnRuGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static readonly Regex LatinWordRe =
			new("[A-Za-zÀ-ÖØ-öø-ÿĒēĪīŌōŪū']+", RegexOptions.Compiled);
		private static readonly Regex CyrillicWordRe =
			new("[А-Яа-яЁё]+", RegexOptions.Compiled);

		/// <summary>Noms propres hors deck ⑩ (fr + en + ru, formes fléchies russes
		/// incluses). Hérite conceptuellement du ProperNouns ①+⑤ de CassePhraseGuardTests.</summary>
		private static readonly HashSet<string> ProperNouns = new(StringComparer.Ordinal)
		{
			// fr
			"Écossais", "Lune", "Coué", "Pygmalion", "Golem", "Matthieu", "France",
			"Spider", "Man", "Galilée", "Sherlock", "Holmes",
			// reprise ⑩ (review 5333238621) : Flintstones = « Pierrafeu » (es « Picapiedra »)
			"Pierrafeu",
			// ru — « Судный день » garde sa majuscule (nom religieux, forme majoritaire)
			"Судного",
			// tokens latins ①/⑤ (CassePhraseGuardTests) repris tels quels : les titres
			// fr hors deck portent eux aussi ces noms propres (Effet Gold, Réflexe
			// Semmelweis, effet Barnum…)
			"Chewbacca", "Christianity", "Hitler", "Hitlerum", "Linda", "Moon", "Morgan's",
			"Flintstones", "Rogers", "Latin", "Gold", "Semmelweis", "Lua", "Kafkaiana",
			"Kafka", "Barnum", "Gish", "V", "Van", "Gogh", "Von", "Restorff", "Woozle",
			"El", "Greco", "Carthago", "Cartago", "Galileu", "Galileo", "Neyman", "Murphy",
			"Simpson", "Stroop", "Weber", "Fechner", "IKEA", "Google", "Pangloss",
			"Russell", "Will", "Hanlon", "Morgan", "Morton", "Wobegon", "Picapiedra",
			"España", "Pigmalião", "Pigmalión", "Mateus", "Mateo", "Hans", "Clever",
			"França",
			// en
			"I", "Scotsman", "Kool", "Aid",
			// ru — génitifs et formes fléchies des personnes ①/⑤ + entités
			"Шерлока", "Холмса", "Гольда", "Семмельвейса", "Луне", "Руссо", "Флинтстоунов",
			"Хэнлона", "Моргана", "Христианстве", "Барнума", "Пигмалиона", "Голема",
			"Матфея", "Ганса", "Линды", "Струпа", "Неймана", "Симпсона", "СМИ",
			"Рассела", "Роджерса", "Ван", "Гога", "Человека", "Панглосса", "Вузла",
			"Вебера", "Фехнера", "Ресторффа", "Мерфи", "Вобигон", "ИКЕА", "Галилея",
			"Эль", "Греко", "Карфаген",
			// « Жаргон Вайэтис » : translittération erronée de « Varietyese » traitée
			// en nom propre — défaut de CONTENU signalé à #1611, pas corrigeable en
			// ⑩ (casse). Épinglée pour ne pas fausser le balayage ru.
			"Вайэтис",
		};

		private static IReadOnlyList<string> OffDeckTitles(string column)
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			return csv.LoadColumn(column, "carte", new[] { "" });
		}

		private static List<string> Violators(string column, Regex wordRe)
		{
			var violators = new List<string>();
			foreach (var raw in OffDeckTitles(column))
			{
				var t = raw.Trim();
				// Règle du second titre (reprise ⑩, review 5333238621) : après une barre
				// oblique commence un nouveau titre — son premier mot garde sa majuscule
				// (« Drinking the Kool-Aid / Peer pressure »).
				var words = t.Split('/')
					.SelectMany((seg, i) => wordRe.Matches(seg).Select(m => m.Value).Skip(1))
					.ToList();
				if (wordRe.Matches(t).Count < 2) continue;
				if (words.Any(w => char.IsUpper(w[0]) && !ProperNouns.Contains(w)))
					violators.Add(t);
			}
			return violators;
		}

		private static string CellOf(string path, string column)
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			var values = csv.LoadColumn(column, "path", new[] { path });
			values.Should().HaveCount(1, "une seule rangée porte ce path.");
			return values[0].Trim();
		}

		[Fact]
		public void Fr_HorsDeck_NoFakeCapitals()
		{
			Violators("text_fr", LatinWordRe).Should().BeEmpty(
				"⑩ : casse de phrase fr hors deck — majuscule réservée au premier mot et aux noms propres (Écossais, Lune, Coué…).");
		}

		[Fact]
		public void Ru_HorsDeck_NoFakeCapitals()
		{
			Violators("text_ru", CyrillicWordRe).Should().BeEmpty(
				"⑩ : casse de phrase ru hors deck — « Профессор Ничего » & co. corrigés ; génitifs de personnes (Барнума, Вебера…) en liste.");
		}

		[Fact]
		public void En_G10_Cells_Pinned()
		{
			// Balayage complet EN désormais possible : ⑨ (#1624) est mergé, ses cinq
			// retardataires sont sentence case sur cet arbre.
			Violators("text_en", LatinWordRe).Should().BeEmpty(
				"⑩ en : balayage complet post-merge ⑨ — majuscule réservée au premier mot, aux noms propres, " +
				"et au premier mot d'un second titre après barre oblique (reprise ⑩ : « Peer pressure »).");
			CellOf("1.2.2.2.3", "text_en").Should().Be("Drinking the Kool-Aid / Peer pressure",
				"reprise ⑩ : après la barre commence un SECOND titre (majuscule) ; le point parasite sort.");
			CellOf("2.3.2.2.2", "text_en").Should().Be("Disrupt then reframe", "⑩ en.");
			CellOf("2.3.3.5.1", "text_en").Should().Be("Socio-cultural marker", "⑩ en.");
			CellOf("3.2.3.2.1", "text_en").Should().Be("Vicious infinite regress", "⑩ en.");
			CellOf("5.3.2.1.2", "text_en").Should().Be("Semantic slippery slope", "⑩ en.");
			CellOf("6.1.3.1.1.3", "text_en").Should().Be("One-sided argument", "⑩ en.");
			CellOf("6.3.1.1.1.3", "text_en").Should().Be("Levels of processing model",
				"⑩ en : terme établi de psychologie (« levels of processing »).");
		}

		[Fact]
		public void G10_Rework_Cells_Pinned()
		{
			// Reprise ⑩ — review 5333238621 : quatre cellules ne sont pas des
			// majuscules « en trop » mais des noms propres ou un second titre.
			CellOf("1.3.1.3.1.1.1", "text_fr").Should().Be("Sophisme des Pierrafeu",
				"reprise ⑩ : famille Flintstones (« Les Pierrafeu », es « Picapiedra ») — " +
				"en minuscule, « pierre-à-feu » devient un silex.");
			CellOf("2.3.3.4.2.3.9", "text_ru").Should().Be("Вулканский салют",
				"reprise ⑩ : nom russe établi du salut de Star Trek — en minuscule, " +
				"« вулкана » voudrait dire « d'un volcan ».");
			CellOf("3.2.2.2.2", "text_ru").Should().Be("Аргумент Судного дня",
				"reprise ⑩ : « Судный день » garde la majuscule (nom religieux, forme " +
				"majoritaire presse/vulgarisation).");
			CellOf("1.2.2.2.3", "text_en").Should().Be("Drinking the Kool-Aid / Peer pressure",
				"reprise ⑩ : second titre capitalisé, point parasite sorti.");
		}

		[Fact]
		public void ProperNouns_KeepCapitals()
		{
			CellOf("1.1.3.3.2", "text_fr").Should().Be("Sophisme du vrai Écossais",
				"règle fr : nom d'habitant garde la majuscule.");
			CellOf("1.2.1.2.1.1", "text_fr").Should().Be("Appel à la Lune",
				"règle fr : l'astre garde la majuscule.");
			CellOf("2.3.1.3.1.1", "text_fr").Should().Be("Appel à la méthode Coué",
				"règle fr : nom de personne.");
			CellOf("2.3.1.3.2.1", "text_fr").Should().Be("Appel à l’effet Pygmalion",
				"règle fr : effet éponyme (déjà noms propres pt/es en ⑤).");
			CellOf("4.1.2.2.1", "text_fr").Should().Be("Sophisme de Spider-Man",
				"nom propre (⑧ a épinglé sa contre-garde).");
			CellOf("1.1.2.1.5", "text_en").Should().Be("I know it when I see it",
				"en : le pronom I est toujours capitalisé.");
			CellOf("1.1.3.3.2", "text_en").Should().Be("No true Scotsman",
				"en : Scotsman = habitant (miroir du fr « vrai Écossais »).");
			CellOf("1.1.1.1.6.2", "text_ru").Should().Be("Софизм Шерлока Холмса",
				"ru : génitifs de personnes.");
			CellOf("2.3.1.1.2.1", "text_ru").Should().Be("Апелляция к эффекту Барнума",
				"ru : effet Barnum (miroir fr « effet Barnum »).");
			CellOf("6.3.1.1.1.2", "text_ru").Should().Be("Закон Вебера-Фехнера",
				"ru : loi éponyme.");
			CellOf("6.3.1.2.3.1.1.2", "text_ru").Should().Be("Эффект ИКЕА", "ru : sigle.");
			CellOf("7.1.3.3.2", "text_ru").Should().Be("Да погибнет Карфаген",
				"ru : Carthago delenda est — toponyme.");
		}

		[Fact]
		public void Removed_G10_Forms_Absent()
		{
			var fr = OffDeckTitles("text_fr");
			var en = OffDeckTitles("text_en");
			var ru = OffDeckTitles("text_ru");
			foreach (var t in fr)
				"Émotive|Intermittent|Verbal|Sexuel|Homophobe|Transphobe|Apocalypse|Usage Mention|Pierre-à-feu"
					.Split('|').ToList().ForEach(w =>
						t.Should().NotContain($" {w}", $"forme retirée « {w} » (⑩ fr)."));
			foreach (var t in en)
				"Reframe|Marker|Infinite|Slippery|Argument|Processing"
					.Split('|').ToList().ForEach(w =>
						t.Should().NotContain($" {w}", $"forme retirée « {w} » (⑩ en)."));
			// « Peer pressure » (second titre après barre) et les formes russes
			// « Вулкана »/« Судного » sont redevenues CORRECTES en reprise ⑩
			// (review 5333238621) : elles ne sont plus des formes interdites.
			foreach (var t in ru)
			{
				t.Should().NotContain(" Ничего", "forme retirée (⑩ ru).");
				t.Should().NotContain(" Фальшивых Экспертов", "forme retirée (⑩ ru).");
				t.Should().NotContain(" Дебат", "forme retirée (⑩ ru).");
				t.Should().NotContain(" Авторитет", "forme retirée (⑩ ru).");
				t.Should().NotContain(" Автофеизм", "forme retirée (⑩ ru).");
				t.Should().NotContain(" Шантаж", "forme retirée (⑩ ru).");
				t.Should().NotContain(" Издевательство", "forme retirée (⑩ ru).");
				t.Should().NotContain(" Каннибализм", "forme retirée (⑩ ru).");
				t.Should().NotContain(" Кондиционирование", "forme retirée (⑩ ru).");
				t.Should().NotContain(", Скаут", "forme retirée (⑩ ru — retrait du capital ; ⑯ retraduira le titre).");
				t.Should().NotContain(" Будо", "forme retirée (⑩ ru — ⑯ retraduira).");
				t.Should().NotContain(" Энтимем", "forme retirée (⑩ ru).");
			}
		}
	}
}
