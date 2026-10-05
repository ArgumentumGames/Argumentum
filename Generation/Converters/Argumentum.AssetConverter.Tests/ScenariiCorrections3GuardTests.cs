using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Grain 5 (#458) — CORRECTIONS (3) : les six familles etablies par la matrice inter-langues
	/// (<c>docs/translation/458-matrice-inter-langues-2026-10-04.md</c> §3), corrigees en
	/// restaurant la <b>source FR</b>. <b>22 cellules / 5 rangees / 6 langues.</b>
	/// <para>2.2.9 contexte (ru ar fa es zh pt) — apposition « personnage mythologique » ajoutee,
	/// absente du FR ET de l'EN ; 3.2.16 enjeu (ru ar fa es zh) — modalite affaiblie
	/// (« doit convaincre » rendu « essaie de convaincre ») ; 4.1.12 enjeu (ru ar fa zh pt) —
	/// meme affaiblissement ; 4.1.11 enjeu (ru ar fa es zh) — « destination de reve » remplacee
	/// par un superlatif comparatif, <b>et Mars perdu</b> ; 4.2.8 titre (es) — « inattendu »
	/// ajoute et concept dedouble.</para>
	/// <para>Les langues cibles <b>portent</b> l'ecart : aucune n'est imprimee (l'edition de
	/// fevrier 2022 porte le FR et l'EN), aucune cellule FR ni EN n'est touchee ici.</para>
	/// <para><b>Les 6 choix differes</b> (<see cref="Deferred"/>) sont epingles a leur valeur
	/// COURANTE : le FR ne fournit pas la forme cible (il faut choisir entre des synonymes de
	/// registre different), donc composer n'est pas restaurer. Le jour ou l'un est ecrit, ce test
	/// rougit et demande de retirer son entree <b>en meme temps</b>.</para>
	/// </summary>
	public class ScenariiCorrections3GuardTests
	{
		private static string ScenariiCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		/// <summary>
		/// Les 22 cellules ecrites : rangee, colonne, empreinte SHA-256 de la cellule PLEINE
		/// (derivee du fichier corrige, jamais de memoire), ce que la correction repare.
		/// </summary>
		private static readonly (string Path, string Column, string Sha256, string Why)[] Fixed =
		{
			// -- 2.2.9 contexte : apposition « personnage mythologique » retiree --
			("2.2.9", "context_ru", "3df67c1b5f650503443e599b3e3fbff3d941307d6c1811bf40c88cfc173a1bef", "apposition «мифологический персонаж» ajoutee — absente du FR et de l'EN"),
			("2.2.9", "context_ar", "3d9c02be725b41761f50b5845bf930405f5e6bb91033e24c9bb2f3fe6f665dc3", "apposition «شخصية ميثولوجية» ajoutee — absente du FR et de l'EN"),
			("2.2.9", "context_fa", "7afefd0ebd93117b17341906b9d08a6917a253536ff3787e9e980136857fda6e", "apposition «چهره‌ای اسطوره‌ای» ajoutee (et «تا تا ابد» dedouble, corrige au passage)"),
			("2.2.9", "context_es", "175c327ed5a418ceb0d384393a261b464e0521d87e243ddff5545010d6b56cd0", "apposition «personaje mitológico» ajoutee — le participe devient verbe (está condenado)"),
			("2.2.9", "context_zh", "306bda95bd23161064ea515a8be1a8f5c5a43c8c35fe4e7a2118cab7521203c1", "apposition «是神话人物» ajoutee — absente du FR et de l'EN"),
			("2.2.9", "context_pt", "37e4495c1dd7cee92c49beeb5250c14f5749b48d4f68ada61f9a1c3e1d34ed47", "apposition «personagem mitológica» ajoutee (accord masc. au passage)"),
			// -- 3.2.16 enjeu : modalite « doit convaincre » restauree --
			("3.2.16", "issue_ru", "6364967711f47de4a111f62e42e9f370454764aa9d0802632289f8932abb707f", "modalite affaiblie «пытается убедить» -> «должен убедить» (FR « doit convaincre »)"),
			("3.2.16", "issue_ar", "d1f288fd1a25b7ef5949f2e1ac2fcb787050da0f604f88ac92b6fd463cddcffd", "modalite affaiblie «يحاول إقناع» -> «عليه أن يقنع» (syntaxe du corpus : على ... أن)"),
			("3.2.16", "issue_fa", "bcdb212286d253bf138ee712ded07a6a212e12673726f67e264ad7ac71e9865d", "modalite affaiblie «سعی می‌کند» -> «باید»"),
			("3.2.16", "issue_es", "1f3cdcafe2616ee7eafb636afee30097ff89374a09e84e508ab0f3a41ec396a0", "infinitif «Intentar» -> «Debe» (le FR et l'EN portent le modal)"),
			("3.2.16", "issue_zh", "17666445f05f80eb159021e56144bc2912d50d09dc58c37bd3aa914640fe947a", "modalite affaiblie «试图» -> «必须»"),
			// -- 4.1.12 enjeu : modalite « convaincre » restauree --
			("4.1.12", "issue_ru", "016c77e50880e56410aaff584685d4cca47e9c67fcbf95dd723eea28837fef8a", "«и попытаться убедить» -> «и убедить» (l'EN dit « and convince »)"),
			("4.1.12", "issue_ar", "17579a8145cff686ad48c6bf8a35e2a544afafc1fa0cf0895ae36d505663b692", "«ويحاول إقناع» -> «ويقنع»"),
			("4.1.12", "issue_fa", "3d3d5e11d950fec23ffe0af0bf7b0e1357ebc3032c2a3a3697ac850651638425", "«و بکوشد ... قانع کند» -> «و ... قانع کند»"),
			("4.1.12", "issue_zh", "a5b626226d6e49eff7fe21663aee7510d3ea6da5a753c05f9c52a8176ad0b42c", "«并试图说服» -> «并说服»"),
			("4.1.12", "issue_pt", "5e1c692b08d0fb9e7dc26a36276b6a9d61462eefb601e941f032e566f5100afd", "«e tentar convencer» -> «e convencer»"),
			// -- 4.1.11 enjeu : « destination de reve » + Mars restaures --
			("4.1.11", "issue_ru", "e1567ff4da53d1cdb8a9cba55d6b2c7e6a084e0ccc99dab3c3586e644f37daf1", "superlatif comparatif -> «направление мечты» ; Mars etait perdu"),
			("4.1.11", "issue_ar", "9b9387c0bf4efafaf50935ff00dbf525403178351d13a401d265aa8206847e21", "comparatif -> «وجهة الأحلام» ; «هذه الوجهة» perdait Mars"),
			("4.1.11", "issue_fa", "2fcc5a062802c03de0954ae092985603d7a67d3b665d882ed98b1a6db3fc5efd", "comparatif (et «از این ... از این») -> «مقصدی رؤیایی» ; Mars etait perdu"),
			("4.1.11", "issue_es", "150abdf61235196fba284181ad460a83d8e9a958156ecbcfdf363cedc0b700f1", "comparatif -> «un destino de ensueño» ; «este destino» perdait Mars"),
			("4.1.11", "issue_zh", "3c68c3f61cf52a9b1bbc519fdb12ac3f963fec0e9787f772189ff5ea165f9585", "comparatif -> «梦想中的目的地» ; «这个目的地» perdait Mars"),
			// -- 4.2.8 titre : seul l'es a un cognat determinE --
			("4.2.8", "title_es", "bdd1a65a68b3960d607839db5e9a80d6916f04e4cc3bfb0ce7f9b889c77ac0f2", "«imprevisto» ajoute + concept dedouble -> «Despertar comprometido» (forme que le pt porte deja)"),
		};

		/// <summary>
		/// Les collocations deviantes, une par cellule ecrite. Chacune a ete mesuree <b>1 occurrence
		/// avant</b> la correction et <b>0 apres</b> : l'ecran pouvait voir un 1. Les fragments
		/// courts partages par des cellules legitimes (ex. «试图» nu, 10 occurrences) sont
		/// volontairement exclus — les ecraner ferait rougir une forme saine.
		/// </summary>
		private static readonly string[] EradicatedForms =
		{
			"мифологический персонаж", "شخصية ميثولوجية", "چهره‌ای اسطوره‌ای",
			"personaje mitológico", "是神话人物", "personagem mitológica",
			"пытается убедить свою половину", "يحاول إقناع نصفه الآخر", "سعی می‌کند نیمه دیگرش",
			"Intentar convencer a su media naranja", "他试图说服另一半",
			"и попытаться убедить родителя", "ويحاول إقناع الوالد", "و بکوشد والد را قانع کند",
			"并试图说服家长", "e tentar convencer o pai/mãe",
			"более желанного направления просто не найти", "أكثر إغراءً مما هي عليه",
			"از این خواستنی‌تر نمی‌شود", "no puede ser más deseable", "诱人到不能再诱人",
			"Despertar imprevisto y compromiso delicado",
		};

		/// <summary>
		/// Les formes restaurees, une par cellule ecrite : le negatif de <see cref="EradicatedForms"/>
		/// ne voit pas une <b>disparition</b>, il faut donc un controle positif.
		/// </summary>
		private static readonly string[] RestoredForms =
		{
			"Сизиф обречён вечно катить камень", "سيزيف محكوم عليه", "سیزیف به‌خاطر سرپیچی",
			"Sísifo está condenado", "西西弗斯因挑战", "Sísifo está condenado",
			"Он должен убедить свою половину", "عليه أن يقنع نصفه الآخر", "باید نیمه دیگرش",
			"Debe convencer a su media naranja", "他必须说服另一半",
			"и убедить родителя заплатить", "ويقنع الوالد بدفع أجره", "و والد را قانع کند که تمام",
			"并说服家长全额付钱", "e convencer o pai/mãe",
			"что Марс — направление мечты", "بأن المريخ وجهة الأحلام", "که مریخ مقصدی رؤیایی است",
			"de que Marte es un destino de ensueño", "火星是一个梦想中的目的地",
			"Despertar comprometido",
		};

		/// <summary>
		/// BURN-DOWN : les 6 choix differes, epingles a leur valeur COURANTE. Le FR ne donne pas la
		/// forme cible — il faudrait choisir entre des synonymes de registre different, ce qui est
		/// <b>composer</b>, pas restaurer.
		/// </summary>
		private static readonly (string Path, string Column, string Current, string Why)[] Deferred =
		{
			("4.2.8", "title_ar", "استيقاظ غير متوقع ومأزق حساس", "«compromis» n'a pas d'equivalent unique en ar (محرج/فاضح ...) — composer"),
			("4.2.8", "title_fa", "بیداریِ غیرمنتظره و مصالحه‌ای حساس", "«compromis» n'a pas d'equivalent unique en fa — composer"),
			("4.2.8", "title_zh", "意外醒来与微妙妥协", "«compromis» n'a pas d'equivalent unique en zh (尴尬/微妙) — composer"),
			("3.1.5", "drawer_ar", "حبيبته من الليلة السابقة", "«conquete» romantique : le registre familier du FR (une conquete = une personne) manque en ar — composer"),
			("3.1.5", "drawer_fa", "دلبر دیشبی‌اش", "«conquete» romantique : idem en fa (شکار/معشوقه) — composer"),
			("3.1.5", "drawer_ru", "Новая избранница", "«conquete» romantique : choix entre победа/завоевание/добыча, nuances differentes — composer"),
		};

		private static string Sha256Hex(string cell)
		{
			using var sha = SHA256.Create();
			return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(cell))).ToLowerInvariant();
		}

		private static string Corpus() => File.ReadAllText(ScenariiCsv, Encoding.UTF8);

		internal static List<string> Mismatches(IEnumerable<(string Path, string Column, string Sha)> cells)
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var offenders = new List<string>();
			foreach (var (path, column, sha) in cells)
			{
				var v = csv.LoadColumn(column, "path", new[] { path });
				if (v.Count != 1)
					offenders.Add($"{path}.{column} : {v.Count} rangee(s) lue(s), 1 attendue");
				else if (Sha256Hex(v[0]) != sha)
					offenders.Add($"{path}.{column} : empreinte «{sha[..12]}…» attendue, lue «{Sha256Hex(v[0])[..12]}…»");
			}
			return offenders;
		}

		[Fact]
		public void Corrected_Cells_Match_Their_Full_Content_Pins()
		{
			Mismatches(Fixed.Select(f => (f.Path, f.Column, f.Sha256))).Should().BeEmpty(
				"#458 grain 5 : les 22 cellules des six familles sont restaurees — repasser une seule d'entre elles doit nommer la rangee et la colonne.");
		}

		[Fact]
		public void Fixed_Table_Is_Not_Vacuous()
		{
			Fixed.Should().HaveCount(22, "6 + 5 + 5 + 5 + 1 cellules.");
			Fixed.Select(f => f.Path).Distinct().Should().HaveCount(5, "5 rangees : 2.2.9, 3.2.16, 4.1.12, 4.1.11, 4.2.8.")
				.And.BeEquivalentTo(new[] { "2.2.9", "3.2.16", "4.1.12", "4.1.11", "4.2.8" });
			// 13 libelles distincts : context_{ru,ar,fa,es,zh,pt} + issue_{ru,ar,fa,zh,pt,es} + title_es.
			Fixed.Select(f => f.Column).Distinct().Should().HaveCount(13, "6 colonnes de contexte, 6 d'enjeu, 1 de titre.");
			Fixed.Where(f => f.Path == "2.2.9").Should().OnlyContain(f => f.Column.StartsWith("context_"));
			Fixed.Where(f => f.Path == "4.2.8").Should().OnlyContain(f => f.Column == "title_es");
			Fixed.Should().OnlyContain(f => f.Sha256.Length == 64, "une empreinte SHA-256 fait 64 caracteres hexadecimaux.");
			Fixed.Should().OnlyContain(f => !string.IsNullOrWhiteSpace(f.Why), "chaque epingle dit ce qu'elle repare.");
			Fixed.Select(f => f.Why).Distinct().Should().HaveCount(22, "22 raisons distinctes : une epingle recopiee ne dit rien de sa cellule.");
			Fixed.Select(f => f.Sha256).Distinct().Should().HaveCount(22, "22 cellules, 22 empreintes distinctes.");
		}

		[Fact]
		public void Deviating_Forms_Are_Absent_From_The_Corpus()
		{
			var corpus = Corpus();
			var found = EradicatedForms.Where(f => corpus.Contains(f, StringComparison.Ordinal)).ToList();
			found.Should().BeEmpty(
				"les 22 collocations deviantes ont ete mesurees 1 occurrence avant la correction et 0 apres : une reapparition signale une regression.");
		}

		[Fact]
		public void Restored_Forms_Are_Present_And_Neighbours_Survive()
		{
			var corpus = Corpus();
			RestoredForms.Where(f => !corpus.Contains(f, StringComparison.Ordinal)).Should().BeEmpty(
				"l'absence d'une forme deviante ne prouve pas la presence de la forme restauree (une suppression passerait l'ecran).");
			// Temoins : des fragments legitimes partages que l'ecran ne devait PAS emporter.
			corpus.Should().Contain("试图", "«试图» nu reste legitime ailleurs (10 occurrences mesurees) : l'ecran ne vise que la collocation deviante.");
			corpus.Should().Contain("пытается", "«пытается» nu reste legitime ailleurs (15 occurrences mesurees).");
		}

		[Fact]
		public void Deferred_Choices_Are_Still_Unwritten()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var offenders = new List<string>();
			foreach (var (path, column, current, _) in Deferred)
			{
				var v = csv.LoadColumn(column, "path", new[] { path });
				v.Should().HaveCount(1, $"la rangee {path} existe et est unique.");
				if (!string.Equals(current, v[0], StringComparison.Ordinal))
					offenders.Add($"{path}.{column} : «{current}» -> «{v[0]}»");
			}
			offenders.Should().BeEmpty(
				"un choix differe a ete ecrit : retirer son entree EN MEME TEMPS que la correction, sinon l'exclusion survit a sa raison.");
		}

		[Fact]
		public void Pin_Detector_Fires_On_A_Mutated_Cell_And_Spares_The_Witness()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var rows = new List<(string, string, string)>();
			foreach (var (path, column, sha, _) in Fixed)
			{
				var v = csv.LoadColumn(column, "path", new[] { path }).Single();
				var actual = Sha256Hex(v);
				// Mutation : la cellule telle qu'elle serait si un espace y rentrait.
				if (path == "2.2.9" && column == "context_ru")
					actual = Sha256Hex(v + " ");
				rows.Add((path, column, actual));
			}
			var offenders = Mismatches(rows);
			offenders.Should().HaveCount(1, "seule la cellule mutee doit rougir : le temoin sain reste vert.");
			offenders[0].Should().Contain("2.2.9.context_ru");
		}
	}
}
