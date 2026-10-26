using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde des corrections Scenarii ① — familles inter-langues etablies (#458, pool ai-01).
	/// <para>29 cellules ecrites sur 6 rangees, dans 5 langues cibles. Chaque epingle porte la
	/// valeur PLEINE de la cellule : une correction qui ne toucherait qu'une partie du texte
	/// (ou qui en ajouterait) fait rougir ici, nommant la rangee, la colonne et les deux textes.</para>
	/// <para><b>Trois familles de defaut, une seule cause : un ajout ou une substitution que les
	/// DEUX sources (FR et EN) ne portent pas.</b> 1.1.3 : le felide est un lion dans les deux
	/// sources ET dans l'issue de la meme carte — quatre langues y mettent un chaton. 3.3.2 et
	/// 4.3.4 : une clause ajoutee (« engagement mutuel », « convaincre un comite ») absente des
	/// deux sources ; en 4.3.4 la carte dit ainsi deux fois la meme chose. 1.2.3 et 2.1.8 :
	/// ponctuation finale absente la ou les sources ponctuent.</para>
	/// <para><b>Grain 3 (7.3.5, cinq langues) :</b> la replique « Le theme de la soiree, c'est
	/// Venise. Pourquoi etes-vous deguise en fromage ? » etait REECRITE en question (« quel
	/// etait deja le theme de la fete ? ») dans es/ru/ar/fa/zh — l'ancrage Venise, c'est-a-dire
	/// la mecanique meme de la carte, avait disparu. Restaure par traduction de la source FR,
	/// en echoyant le vocabulaire du contexte de chaque langue (ru « переодевшись сыром »,
	/// fa « تم ونیزی », zh « 打扮成…奶酪 »). L'exclusion nommee qui retenait l'ecriture zh
	/// (delegation ①) est morte AVEC sa raison dans le meme commit.</para>
	/// </summary>
	public class ScenariiInterLanguageFamilyGuardTests
	{
		private static string ScenariiCsv => System.IO.Path.Combine(
			TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		/// <summary>Rangee, colonne, valeur pleine attendue — 24 cellules corrigees.</summary>
		private static readonly (string Path, string Column, string Expected)[] Fixed =
		{
			("1.1.3", "suggestion_zh", "嗯，这可是喂我狮子的上等美味。"),
			("1.1.3", "suggestion_ar", "هممم، يا لها من وجبة فاخرة لأسودي."),
			("1.1.3", "suggestion_fa", "هوم، چه لقمه دل\u200cچسبی برای شیرهای من."),
			("1.1.3", "suggestion_es", "Mmm, un bocado exquisito para mis leones."),
			("3.3.2", "issue_zh", "诡辩者想让伴侣相信：与其结婚，不如领养一块石头当宠物。"),
			("3.3.2", "issue_ar", "يريد المغالِط إقناع شريكه بأنهما، بدلًا من الزواج، يستطيعان تبنّي حصاة كحيوان أليف."),
			("3.3.2", "issue_fa", "چرب\u200cزبان می\u200cخواهد شریکش را قانع کند که به\u200cجای ازدواج، یک قلوه\u200cسنگ را به\u200cعنوان حیوان خانگی به سرپرستی بگیرند."),
			("3.3.2", "issue_es", "Convencer a su pareja de que, en vez de casarse, podrían adoptar una piedra como mascota."),
			("3.3.2", "issue_ru", "Софист хочет убедить партнёра, что вместо свадьбы они могут завести камень в качестве питомца."),
			("4.3.4", "context_zh", "诡辩者声称要打造一个合乎伦理、负责任的 AI，却不设置任何安全护栏。"),
			("4.3.4", "context_fa", "چرب\u200cزبان ادعا می\u200cکند بدون هیچ سازوکار ایمنی، هوش مصنوعی اخلاقی و مسئولانه می\u200cسازد."),
			("4.3.4", "context_es", "El embaucador afirma estar creando una IA ética y responsable sin ninguna salvaguarda."),
			("4.3.4", "context_ru", "Софист уверяет, что создаёт этичный и ответственный ИИ без каких-либо ограничителей."),
			("1.2.3", "context_zh", "拿破仑想入侵俄国。"),
			("1.2.3", "issue_zh", "诡辩者必须想办法劝他打消这个念头。"),
			("1.2.3", "context_ar", "نابليون يريد غزو روسيا."),
			("1.2.3", "issue_ar", "على المغالِط أن يحاول ثنيه عن ذلك."),
			("1.2.3", "context_fa", "ناپلئون می\u200cخواهد به روسیه حمله کند."),
			("1.2.3", "issue_fa", "چرب\u200cزبان باید تلاش کند او را از این کار منصرف کند."),
			("1.2.3", "context_es", "Napoleón quiere invadir Rusia."),
			("1.2.3", "issue_es", "Intentar disuadirlo."),
			("2.1.8", "suggestion_zh", "陛下，请您不要动怒。"),
			("2.1.8", "suggestion_ar", "سيدي، لا تغضب جلالتكم."),
			("2.1.8", "suggestion_es", "Señor, que Vuestra Majestad no se enfade."),
			("7.3.5", "suggestion_zh", "今晚派对的主题是威尼斯。你为什么打扮成奶酪？"),
			("7.3.5", "suggestion_ar", "موضوع الحفلة هو البندقية. لماذا تنكرت في هيئة جبن؟"),
			("7.3.5", "suggestion_fa", "تم مهمانی ونیز است. چرا لباس پنیر پوشیده\u200cای؟"),
			("7.3.5", "suggestion_es", "El tema de la fiesta es Venecia. ¿Por qué estás disfrazado de queso?"),
			("7.3.5", "suggestion_ru", "Тема вечеринки — Венеция. Почему вы переоделись сыром?"),
		};

		/// <summary>Les 6 rangees touchees — anti-vacuite du tableau ci-dessus.</summary>
		private static readonly string[] TouchedPaths = { "1.1.3", "1.2.3", "2.1.8", "3.3.2", "4.3.4", "7.3.5" };

		/// <summary>
		/// Deteteur, factorise pour etre exerce sur des donnees SYNTHETIQUES par le controle
		/// inverse : sans temoin rouge, une garde vide peut etre morte par construction.
		/// </summary>
		internal static List<string> Mismatches(
			IEnumerable<(string Path, string Column, string Expected, string Actual)> cells)
		{
			var offenders = new List<string>();
			foreach (var (path, column, expected, actual) in cells)
			{
				if (!string.Equals(expected, actual, StringComparison.Ordinal))
					offenders.Add($"{path}.{column} : attendu «{expected}», lu «{actual}»");
			}
			return offenders;
		}

		private static List<(string, string, string, string)> ReadPinnedCells()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var read = new List<(string, string, string, string)>();
			foreach (var (path, column, expected) in Fixed)
			{
				var actual = csv.LoadColumn(column, "path", new[] { path });
				actual.Should().HaveCount(1, $"la rangee {path} existe et est unique (colonne {column}).");
				read.Add((path, column, expected, actual[0]));
			}
			return read;
		}

		[Fact]
		public void Every_Corrected_Cell_Still_Holds_Its_Value()
		{
			Mismatches(ReadPinnedCells()).Should().BeEmpty(
				"#458 ① + grain 3 : les 29 cellules corrigees gardent leur texte — un lion reste un lion, " +
				"la clause ajoutee ne revient pas, la ponctuation finale est toujours la, Venise est ancre.");
		}

		[Fact]
		public void Pinned_Table_Is_Complete_And_Not_Vacuous()
		{
			Fixed.Should().HaveCount(29, "29 cellules corrigees, mesurees ; le compte est le sujet.");
			Fixed.Select(f => (f.Path, f.Column)).Distinct().Should().HaveCount(29,
				"aucune cellule n'est epinglee deux fois (un doublon masquerait une cellule non couverte).");
			Fixed.Select(f => f.Path).Distinct().OrderBy(p => p, StringComparer.Ordinal)
				.Should().BeEquivalentTo(TouchedPaths, "les 6 rangees de la famille sont toutes couvertes.");
		}

		[Fact]
		public void Detector_Fires_On_A_Synthetic_Revert()
		{
			// Controle inverse : on rejoue l'ETAT D'AVANT sur une cellule, en memoire seulement.
			var reverted = new[]
			{
				("1.1.3", "suggestion_zh", "\u55ef\uff0c\u8fd9\u53ef\u662f\u5582\u6211\u72ee\u5b50\u7684\u4e0a\u7b49\u7f8e\u5473\u3002",
				 "\u55ef\uff0c\u8fd9\u53ef\u662f\u5582\u6211\u5c0f\u732b\u54aa\u7684\u4e0a\u7b49\u7f8e\u5473\u3002"),
				("3.3.2", "issue_ru", Fixed[8].Expected, Fixed[8].Expected + " \u0434\u043e\u0431\u0430\u0432\u043a\u0430"),
				("7.3.5", "suggestion_zh", Fixed[24].Expected,
				 "今晚派对的主题是什么来着？你看起来像块奶酪，认真的吗？"),
			};
			var offenders = Mismatches(reverted);
			offenders.Should().HaveCount(3, "le deteteur voit exactement les trois ecarts injectes.");
			offenders[0].Should().Contain("1.1.3.suggestion_zh").And.Contain("\u5c0f\u732b\u54aa",
				"le temoin nomme la cellule ET la valeur fautive relue.");
			offenders[1].Should().Contain("3.3.2.issue_ru");
			offenders[2].Should().Contain("7.3.5.suggestion_zh",
				"la replique reecrite sans Venise est vue comme un ecart, pas comme une variante.");
		}

		}
}
