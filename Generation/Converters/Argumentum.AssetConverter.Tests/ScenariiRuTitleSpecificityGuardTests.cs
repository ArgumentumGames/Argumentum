using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Grains 6 puis 3 (#458) — titres ru : les restaurations dont le rendu russe est ETABLI.
	/// <para>Le lot ru compte 12 titres nivelees (dossier de lecture, §3 n° 4). Le grain 6
	/// (cycle S) a ecrit les 2 restaurations informationnelles non ambigues ; l'arbitrage
	/// ai-01 (c.5980960236, repris par le pool c.5985353780 grain 3) a statue : « les 7 titres
	/// marques candidats s'appliquent ; 6.3.2 et 3.1.1 sont gardes ». Ce fichier epingle les
	/// 7 ecrites (2 + 5) et le burn-down des 4 restantes : 2 bloquees par une PR ouverte sur
	/// la meme rangee/ligne (#1769), 2 gardees par arbitrage. Voir le dossier
	/// <c>docs/translation/458-v22-g6-titres-ru-cas-par-cas-2026-10-04.md</c> (§6).</para>
	/// </summary>
	public class ScenariiRuTitleSpecificityGuardTests
	{
		private static string ScenariiCsv => System.IO.Path.Combine(
			TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		/// <summary>Les sept restaurations ecrites : rangee, valeur pleine, ce qui etait perdu.</summary>
		private static readonly (string Path, string Expected, string Lost)[] Restored =
		{
			("1.3.2", "Президент Трумэн и атомная бомба", "fr «bombe A» / en \"A-Bomb\" / pt «bomba A» — ru ne disait que «бомба»"),
			("7.2.6", "Марочное фуа-гра", "fr «Millésime» / en \"vintage\" / pt «Safra» — ru disait «изысканное» (exquis)"),
		("3.3.6", "Батюшки, мой муж!", "fr «Ciel, mon mari !» / en \"Heavens my husband\" / pt «Por Deus, meu marido» — l'exclamation de l'epouse ; ru disait un verdict («Нам крышка» = on est fichus)"),
		("3.3.10", "Свадьба? Нет, спасибо", "fr «Non merci» / en \"No Thanks\" / pt «Não, obrigado» — le refus ; ru disait une question neutre (« ou non ? »)"),
		("4.1.1", "Понты", "fr «Rouler des mécaniques» / pt «Fazer-se de importante» — la frime du CLIENT ; ru decrivait le vendeur. Понты = frime ostensible, пыль в глаза eut deplace vers la duperie"),
		("4.1.2", "Розги", "fr «Le martinet» / en \"The Cane\" / pt «O chicote» — l'instrument de la punition scolaire ; ru disait l'abstraction. Розги repond au cane, кнут eut deplace vers la torture"),
		("7.1.5", "Кермесса", "fr «La kermesse» / en \"Kermesse\" / pt «A quermesse» — les trois sources gardent le mot ; calque etabli (gallicismes, Ermitage/Pouchkine). Ru disait generique («Праздник» = fete)"),
		};

		/// <summary>
		/// BURN-DOWN : les 4 titres non ecrits, epingles a leur valeur COURANTE, avec leur motif.
		/// Deux GARDES par arbitrage ai-01 (c.5980960236) : 3.1.1 (jeu de mots sans
		/// equivalent russe, regle C) et 6.3.2 (aucun rendu russe neutre pour la duree).
		/// Deux BLOQUES par la PR #1769, ouvertes sur le meme CSV : 2.2.7 (MEME rangee,
		/// ligne 35) et 5.3.1 (ligne 125 ADJACENTE a 5.3.2, ligne 126) — a ecrire en un
		/// seul lot apres son merge. Le jour ou l'un est corrige, ce test rougit : retirer
		/// son entree AVEC la correction — jamais laisser une exclusion survivre a sa raison.
		/// </summary>
		private static readonly (string Path, string Current, string Why)[] HandedOff =
		{
			("2.2.7", "Завоевать Пенелопу", "candidat arbitre «Претендент на Пенелопу» (c.5980960236) — BLOQUE : meme rangee que #1769 (title_pt «Penélope»), reprise apres son merge"),
			("5.3.1", "Теория плоской земли", "candidat arbitre «Дебаты с плоскоземельцем» — BLOQUE : ligne 125 adjacente a 5.3.2 (#1769, ligne 126), reprise apres son merge"),
			("3.1.1", "Второй парень на свидании", "GARDE par arbitrage (c.5980960236) : jeu de mots a double sens sans equivalent russe — regle C (absence d'equivalent, pas acceptation du nivellement)"),
			("6.3.2", "Старый друг", "GARDE par arbitrage (c.5980960236) : aucun rendu russe de «vingt ans» ne reste neutre (autre duree ou lecture « age de »), relecture native"),
		};

		internal static List<string> Mismatches(
			IEnumerable<(string Path, string Expected, string Actual)> cells)
		{
			var offenders = new List<string>();
			foreach (var (path, expected, actual) in cells)
				if (!string.Equals(expected, actual, StringComparison.Ordinal))
					offenders.Add($"{path}.title_ru : attendu «{expected}», lu «{actual}»");
			return offenders;
		}

		private static List<(string, string, string)> ReadPinned()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var read = new List<(string, string, string)>();
			foreach (var (path, expected, _) in Restored)
			{
				var v = csv.LoadColumn("title_ru", "path", new[] { path });
				v.Should().HaveCount(1, $"la rangee {path} existe et est unique.");
				read.Add((path, expected, v[0]));
			}
			return read;
		}

		[Fact]
		public void Restored_Ru_Titles_Carry_Back_The_Lost_Information()
		{
			Mismatches(ReadPinned()).Should().BeEmpty(
				"#458 grains 6+3 : «атомная бомба», «марочное», l'exclamation, le refus, la frime, l'instrument et la kermesse : " +
				"l'information portee par les sources est revenue dans le titre russe.");
		}

		[Fact]
		public void Restored_Table_Is_Not_Vacuous()
		{
			Restored.Should().HaveCount(7, "2 ecritures du grain 6 + 5 du grain 3 (arbitrage c.5980960236).");
			Restored.Select(r => r.Path).Distinct().Should().HaveCount(7, "7 cellules sur 7 rangees distinctes.");
			Restored.Should().OnlyContain(r => r.Lost.Length > 20, "chaque epingle dit ce qui etait perdu.");
			Restored.Select(r => r.Lost).Distinct().Should().HaveCount(7, "7 libelles distincts, pas un copier-coller.");
		}

		[Fact]
		public void Detector_Fires_On_The_Pre_Correction_Value()
		{
			var reverted = new[]
			{
				("1.3.2", Restored[0].Expected, "\u041f\u0440\u0435\u0437\u0438\u0434\u0435\u043d\u0442\u0020\u0422\u0440\u0443\u043c\u044d\u043d\u0020\u0438\u0020\u0431\u043e\u043c\u0431\u0430"),
				("7.2.6", Restored[1].Expected, Restored[1].Expected),
				("4.1.1", Restored[4].Expected, "\u0423\u043b\u043e\u0432\u043a\u0438 \u043f\u0440\u043e\u0434\u0430\u0432\u0446\u0430"),
			};
			var offenders = Mismatches(reverted);
			offenders.Should().HaveCount(2, "seules les cellules revertes doivent rougir : le temoin sain reste vert.");
			offenders.Should().Contain(s => s.Contains("1.3.2.title_ru"));
			offenders.Should().Contain(s => s.Contains("4.1.1.title_ru"));
		}

		[Fact]
		public void Handed_Off_Titles_Are_Still_Unwritten()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var offenders = new List<string>();
			foreach (var (path, current, _) in HandedOff)
			{
				var v = csv.LoadColumn("title_ru", "path", new[] { path });
				v.Should().HaveCount(1);
				if (!string.Equals(current, v[0], StringComparison.Ordinal))
					offenders.Add($"{path} : «{current}» -> «{v[0]}»");
			}
			offenders.Should().BeEmpty(
				"un titre recense a change : retirer son entree de HandedOff EN MEME TEMPS que la correction, " +
				"sinon l'exclusion survit a sa raison. Details : docs/translation/458-v22-g6-titres-ru-cas-par-cas-2026-10-04.md");
		}

		[Fact]
		public void Handed_Off_Table_Is_Not_Vacuous_And_Reasoned()
		{
			HandedOff.Should().HaveCount(4, "2 bloquees par #1769 (meme rangee / ligne adjacente) + 2 gardees par arbitrage.");
			HandedOff.Select(h => h.Path).Distinct().Should().HaveCount(4);
			HandedOff.Should().OnlyContain(h => h.Why.Contains("BLOQUE") || h.Why.Contains("GARDE"),
				"chaque entree du burn-down porte son motif : bloquee par une PR ou gardee par arbitrage.");
			HandedOff.Should().Contain(h => h.Why.Contains("BLOQUE") && h.Why.Contains("meme rangee"), "2.2.7 : meme rangee que #1769.");
			HandedOff.Should().Contain(h => h.Why.Contains("BLOQUE") && h.Why.Contains("adjacente"), "5.3.1 : ligne adjacente a #1769.");
		}
	}
}
