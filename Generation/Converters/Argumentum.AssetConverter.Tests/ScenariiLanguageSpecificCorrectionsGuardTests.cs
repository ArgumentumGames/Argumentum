using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Grain 2 (#458) — corrections PROPRES A UNE LANGUE : les 6 cellules ecrites.
	/// <para>Chacune est un defaut qu'aucune autre langue ne partage, etabli a trois voies par
	/// le dossier de la langue concernee : zh 4.3.3/4.3.5/4.3.6 (espace parasite en run contigu),
	/// es 7.2.3 (point final), ru 3.2.4 (fuite d'interface de jeu), fa 5.2.5 (nom du role).</para>
	/// <para>⚠️ <b>4.3.4 est absente d'ici a dessein</b> : c'est la meme famille zh (run contigu)
	/// mais la rangee appartient a la PR #1759 (corrections 1). Une rangee = une carte = une
	/// seule PR de correction.</para>
	/// <para><b>Les 9 defers</b> (Deferred) sont epingles a leur valeur COURANTE : le jour ou
	/// l'un est corrige, ce test rougit et demande de retirer son entree en meme temps.</para>
	/// </summary>
	public class ScenariiLanguageSpecificCorrectionsGuardTests
	{
		private static string ScenariiCsv => System.IO.Path.Combine(
			TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		/// <summary>Les 6 cellules ecrites : rangee, colonne, valeur pleine, ce que la correction repare.</summary>
		private static readonly (string Path, string Column, string Expected, string Why)[] Fixed =
		{
			("4.3.3", "context_zh", "诡辩者预判国家足球队会在世界杯上大热门夺冠，于是订了一大批国家队球衣，货已经送到了。", "espace parasite apres le role (run contigu 4.3.3-4.3.6)"),
			("4.3.5", "context_zh", "诡辩者是一家异想天开公司的老板，号称要通过人类的宠物来彻底革新人与人之间的关系。", "espace parasite apres le role (run contigu 4.3.3-4.3.6)"),
			("4.3.6", "context_zh", "诡辩者发明了自己的滑雪运动：滑雪者打扮成松鼠，一边下坡一边努力收集橡果。", "espace parasite apres le role (run contigu 4.3.3-4.3.6)"),
			("7.2.3", "context_es", "El embaucador tiene un perro revoltoso que se ha escapado y ha destrozado el jardín de su vecino.", "point final absent, alors que le FR et l'EN ponctuent tous deux"),
			("3.2.4", "context_ru", "После долгих отношений Софист и его партнёр решают завести ребёнка. После нескольких попыток беременность наконец начинает успешно развиваться.", "fuite d'interface : «Берущий карту» imprime dans la cellule, les deux sources disent « partenaire »"),
			("5.2.5", "issue_fa", "چرب\u200cزبان باید او را قانع کند که ماجرا این\u200cطور نیست.", "nom du role : 47 cellules du meme champ disent le terme retabli"),
		};

		/// <summary>
		/// BURN-DOWN : les 9 defauts etablis et NON ecrits, epingles a leur valeur COURANTE.
		/// </summary>
		private static readonly (string Path, string Column, string Current, string Why)[] Deferred =
		{
			("6.2.1", "title_zh", "初选连环跳", "titre invente : ne correspond ni au FR ni a l'EN — composer"),
			("6.2.1", "title_ru", "Рокировка", "titre invente («Рокировка») — composer"),
			("5.3.5", "suggestion_fa", "زمین ما را از خودش دور می\u200cراند؟ چه فکر کاملاً عجیب\u200cوغریبی!", "replique du physicien remplacee — composer"),
			("7.2.7", "suggestion_ru", "Слушай, в итоге я не смогу с тобой поехать.", "l'entretien, pivot de la carte, a disparu de la replique — composer"),
			("1.2.2", "issue_ru", "Софист должен убедить императора в том, индейцев можно сделать рабами.", "grammaire : «в том, индейцев» (что manquant) — le dossier la dit a confirmer en relecture native"),
			("1.3.3", "suggestion_ru", "Вечная Франция никогда не будет согнется под Тевтонским игом.", "grammaire : «никогда не будет согнется» (double futur) — idem"),
			("3.2.1", "issue_ru", "Софист должен убедить позволить ему приехать к ней жить.", "grammaire : «убедить позволить» (objet manquant) — idem"),
			("3.2.8", "title_zh", "被忘掉的纪念日", "coherence interne : 纪念日 (anniversaire d'evenement) contre 生日 dans context_zh"),
			("4.3.3", "issue_zh", "结果球队第一轮就惨兮兮出局了；这位转卖商必须说服供应商：他现在只想买下其中一半球衣。", "coherence interne : 转卖商 contre 采购商 dans smoothTalker_zh"),
		};

		internal static List<string> Mismatches(
			IEnumerable<(string, string, string, string)> cells)
		{
			var offenders = new List<string>();
			foreach (var (path, column, expected, actual) in cells)
				if (!string.Equals(expected, actual, StringComparison.Ordinal))
					offenders.Add($"{path}.{column} : attendu «{expected}», lu «{actual}»");
			return offenders;
		}

		private static List<(string, string, string, string)> ReadAll(
			IEnumerable<(string Path, string Column, string Expected, string Why)> table)
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var read = new List<(string, string, string, string)>();
			foreach (var (path, column, expected, why) in table)
			{
				var v = csv.LoadColumn(column, "path", new[] { path });
				v.Should().HaveCount(1, $"la rangee {path} existe et est unique.");
				read.Add((path, column, expected, v[0]));
			}
			return read;
		}

		[Fact]
		public void Corrected_Cells_Still_Hold_Their_Value()
		{
			Mismatches(ReadAll(Fixed)).Should().BeEmpty(
				"#458 grain 2 : les 6 defauts propres a une langue sont corriges (zh espace, es point, ru fuite d'interface, fa nom du role).");
		}

		[Fact]
		public void Fixed_Table_Is_Not_Vacuous()
		{
			Fixed.Should().HaveCount(6);
			Fixed.Select(f => f.Path).Distinct().Should().HaveCount(6, "6 cellules sur 6 rangees distinctes.");
			Fixed.Select(f => f.Column).Distinct().Should().BeEquivalentTo(new[] { "context_zh", "context_es", "context_ru", "issue_fa" });
			Fixed.Should().OnlyContain(f => f.Why.Length > 20, "chaque epingle dit ce qu'elle repare.");
			Fixed.Should().OnlyContain(f => !f.Expected.Contains("\u0020\u8be1"), "aucun espace ne subsiste devant le role zh.");
		}

		[Fact]
		public void Detector_Fires_On_A_Reverted_Cell()
		{
			var reverted = new[]
			{
				("4.3.3", "context_zh", Fixed[0].Expected, Fixed[0].Expected.Replace("\u8be1\u8fa9\u8005", "\u8be1\u8fa9\u8005\u0020")),
				("7.2.3", "context_es", Fixed[3].Expected, Fixed[3].Expected),
			};
			var offenders = Mismatches(reverted);
			offenders.Should().HaveCount(1, "seule la cellule revertee doit rougir : le temoin sain reste vert.");
			offenders[0].Should().Contain("4.3.3.context_zh");
		}

		[Fact]
		public void Deferred_Defects_Are_Still_Unwritten()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var offenders = new List<string>();
			foreach (var (path, column, current, _) in Deferred)
			{
				var v = csv.LoadColumn(column, "path", new[] { path });
				v.Should().HaveCount(1);
				if (!string.Equals(current, v[0], StringComparison.Ordinal))
					offenders.Add($"{path}.{column} : «{current}» -> «{v[0]}»");
			}
			offenders.Should().BeEmpty(
				"un defaut differe a change : retirer son entree EN MEME TEMPS que la correction, " +
				"sinon l'exclusion survit a sa raison (cf. le dossier de la langue concernee).");
		}
	}
}
