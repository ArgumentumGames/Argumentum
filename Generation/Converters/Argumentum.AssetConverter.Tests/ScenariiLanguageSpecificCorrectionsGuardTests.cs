using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Grain 2 (#458) — corrections PROPRES A UNE LANGUE : les 12 cellules ecrites.
	/// <para>Chacune est un defaut qu'aucune autre langue ne partage, etabli a trois voies par
	/// le dossier de la langue concernee : zh 4.3.3/4.3.5/4.3.6 (espace parasite en run contigu),
	/// es 7.2.3 (point final), ru 3.2.4 (fuite d'interface de jeu), fa 5.2.5 (nom du role).</para>
	/// <para>⚠️ <b>4.3.4 est absente d'ici a dessein</b> : c'est la meme famille zh (run contigu)
	/// mais la rangee appartient a la PR #1759 (corrections 1). Une rangee = une carte = une
	/// seule PR de correction.</para>
	/// <para><b>Grain 2 du pool c.5985353780</b> — 2 restaurations <b>chirurgicales</b> sorties
	/// de la burn-down : 3.2.1 (issue_ru, objet manquant) et 4.3.3 (issue_zh, 转卖商 -> 采购商).
	/// ⭐ <i>Rangées disjointes n'implique pas lignes disjointes</i> : 1.2.2, 1.3.3 et 3.2.8 ont
	/// ete laissees alors parce que #1769 tenait leurs lignes.</para>
	/// <para><b>Grain 1 du pool c.5988407613</b> — ces 3 restaurations, ecrites depuis le merge
	/// de #1769 : 1.2.2 (issue_ru, «что» manquant), 1.3.3 (suggestion_ru, double futur) et
	/// 3.2.8 (title_zh + suggestion_zh, 纪念日 -> 生日 : le context_zh de la carte dit 生日,
	/// defect etabli sur titre ET suggestion par le dossier zh n°2).</para>
	/// <para><b>Les 4 marquees « composer »</b> par le burn-down (titres inventes 6.2.1 zh/ru,
	/// repliques remplacees 5.3.5 fa et 7.2.7 ru) restent differees pour un autre motif.</para>
	/// <para><b>Les 5 defers restants</b> (Deferred) sont epingles a leur valeur COURANTE (le 5e, 4.3.1.suggestion_fa, est un constat MESURE du grain 3b du 07/10) : le jour
	/// ou l'un est corrige, ce test rougit et demande de retirer son entree en meme temps.</para>
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
			// -- grain 2 du pool c.5985353780 : les 5 restaurations chirurgicales, sorties de la burn-down --
			("3.2.1", "issue_ru", "Софист должен убедить партнёра позволить ему приехать к ней жить.", "grammaire : objet de « убедить » manquant (FR et EN nomment « son partenaire »)"),
			("4.3.3", "issue_zh", "结果球队第一轮就惨兮兮出局了；这位采购商必须说服供应商：他现在只想买下其中一半球衣。", "coherence interne : 转卖商 contre 采购商, le terme du personnage (smoothTalker_zh et FR « Un acheteur »)"),
			("5.2.5", "issue_fa", "چرب\u200cزبان باید ریچل را قانع کند که به او خیانت نکرده است.", "grain 3b (07/10) : la 2e moitie est ecrite — le dossier fa n°5 etablissait «majara in-towr nist» comme dilution de l'EN ; le NOM DU ROLE (correction du grain 2) est PRESERVE ; les siblings zh/ar/es nomment Rachel"),
			// -- grain 1 du pool c.5988407613 : les 3 restaurees apres le merge de #1769 --
			("1.2.2", "issue_ru", "Софист должен убедить императора в том, что индейцев можно сделать рабами.", "grammaire : «что» manquant apres «в том, » — la subordonnee etait coupee"),
			("1.3.3", "suggestion_ru", "Вечная Франция никогда не согнётся под Тевтонским игом.", "grammaire : double futur «будет согнется» -> futur simple «согнётся» (FR « ne pliera jamais »)"),
			("3.2.8", "title_zh", "被忘掉的生日", "coherence interne : 纪念日 (jour commémoratif) contre 生日 — les 4 sources disent l'anniversaire de naissance"),
			("3.2.8", "suggestion_zh", "认真的？你这次真的把我的生日忘了？", "coherence interne : meme cause que le titre — le context_zh de la carte dit 生日"),
		};

		/// <summary>
		/// BURN-DOWN : les 4 choix « composer » etablis et NON ecrits, epingles a leur valeur COURANTE.
		/// </summary>
		private static readonly (string Path, string Column, string Current, string Why)[] Deferred =
		{
			("6.2.1", "title_zh", "初选连环跳", "titre invente : ne correspond ni au FR ni a l'EN — composer"),
			("6.2.1", "title_ru", "Рокировка", "titre invente («Рокировка») — composer"),
			("5.3.5", "suggestion_fa", "زمین ما را از خودش دور می\u200cراند؟ چه فکر کاملاً عجیب\u200cوغریبی!", "replique du physicien remplacee — composer"),
			("7.2.7", "suggestion_ru", "Слушай, в итоге я не смогу с тобой поехать.", "l'entretien, pivot de la carte, a disparu de la replique — composer"),
			("4.3.1", "suggestion_fa", "\u06CC\u0627 \u062D\u0636\u0631\u062A \u0639\u062C\u0628\u060C \u0646\u06A9\u0646\u062F \u0686\u0634\u0645\u200c\u0647\u0627\u06CC\u0645 \u0633\u06CC\u0627\u0647\u06CC \u0645\u06CC\u200c\u0631\u0648\u062F!", "meme defaut que l'EN corrige au grain 3a (\u00AB I'm seeing things! \u00BB / FR \u00AB la berlue \u00BB) : le fa dit l'evanouissement (\u00AB mes yeux se noircissent \u00BB). La regle du GO couvre la rangee, mais le grain 3a ne portait que ses siblings ru. Constat MESURE le 07/10 (grain 3b) \u2014 hors perimetre de ce grain, epingle ici pour qu'un futur balayage le voie et que l'exclusion meure avec sa raison."),
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
				"#458 : les 12 cellules epinglees sont corrigees (grain 2 : zh espace, es point, ru fuite, fa role ; pools : restaurations chirurgicales).");
		}

		[Fact]
		public void Fixed_Table_Is_Not_Vacuous()
		{
			Fixed.Should().HaveCount(12, "6 cellules du grain 2 de c.5982328511, 2 du grain 2 de c.5985353780, 4 du grain 1 de c.5988407613 (les 3 laissees a #1769 : 1.2.2, 1.3.3, 3.2.8 x2).");
			Fixed.Select(f => f.Path).Distinct().Should().HaveCount(10, "12 cellules sur 10 rangees : 4.3.3 et 3.2.8 en portent deux chacune.");
			Fixed.Select(f => f.Column).Distinct().Should().BeEquivalentTo(
				new[] { "context_zh", "context_es", "context_ru", "issue_fa", "issue_ru", "issue_zh", "suggestion_ru", "title_zh", "suggestion_zh" });
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
