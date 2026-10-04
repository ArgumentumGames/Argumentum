using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Grain 6 (#458) — titres ru : les DEUX restaurations INFORMATIONNELLES non ambigues.
	/// <para>Le lot ru compte 12 titres nivelees (dossier de lecture, §3 n° 4) : 8 jeux de mots
	/// remplaces par des descriptions fonctionnelles, et 3 titres DESPECIFIES qui perdent une
	/// information que le FR, l'EN <b>et</b> le pt portent tous les trois. Seuls les deux dont
	/// le rendu russe est STANDARD sont ecrits ici : «атомная бомба» (terme courant pour
	/// A-bomb) et «марочное» (l'adjectif russe du millesime, cf. марочное вино).</para>
	/// <para><b>Les 9 autres sont RECENSES, pas ecrits</b> — recreer un jeu de mot est un
	/// jugement litteraire russe, pas une restauration : le pool prevoit d'ailleurs de
	/// GARDER un equivalent idiomatique quand il tient (regle C). Voir le dossier
	/// <c>docs/translation/458-v22-g6-titres-ru-cas-par-cas-2026-10-04.md</c>.</para>
	/// </summary>
	public class ScenariiRuTitleSpecificityGuardTests
	{
		private static string ScenariiCsv => System.IO.Path.Combine(
			TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		/// <summary>Les deux restaurations ecrites : rangee, valeur pleine, ce qui etait perdu.</summary>
		private static readonly (string Path, string Expected, string Lost)[] Restored =
		{
			("1.3.2", "Президент Трумэн и атомная бомба", "fr «bombe A» / en \"A-Bomb\" / pt «bomba A» — ru ne disait que «бомба»"),
			("7.2.6", "Марочное фуа-гра", "fr «Millésime» / en \"vintage\" / pt «Safra» — ru disait «изысканное» (exquis)"),
		};

		/// <summary>
		/// BURN-DOWN : les 9 titres analyses et NON ecrits, epingles a leur valeur COURANTE.
		/// Le jour ou l'un est corrige, ce test rougit : c'est le signal qu'il faut le
		/// retirer de cette liste AVEC la correction — jamais laisser une exclusion survivre
		/// a sa raison.
		/// </summary>
		private static readonly (string Path, string Current)[] HandedOff =
		{
			("2.2.7", "Завоевать Пенелопу"),
			("3.1.1", "Второй парень на свидании"),
			("3.3.6", "Нам крышка"),
			("3.3.10", "Свадьба или нет?"),
			("4.1.1", "Уловки продавца"),
			("4.1.2", "Телесные наказания"),
			("5.3.1", "Теория плоской земли"),
			("7.1.5", "Праздник"),
			("6.3.2", "Старый друг"),
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
				"#458 grain 6 : «атомная бомба» et «марочное» : l'information portee par le FR, l'EN et le pt est revenue dans le titre russe.");
		}

		[Fact]
		public void Restored_Table_Is_Not_Vacuous()
		{
			Restored.Should().HaveCount(2);
			Restored.Select(r => r.Path).Distinct().Should().HaveCount(2);
			Restored.Should().OnlyContain(r => r.Lost.Length > 20, "chaque epingle dit ce qui etait perdu.");
		}

		[Fact]
		public void Detector_Fires_On_The_Pre_Correction_Value()
		{
			var reverted = new[]
			{
				("1.3.2", Restored[0].Expected, "\u041f\u0440\u0435\u0437\u0438\u0434\u0435\u043d\u0442\u0020\u0422\u0440\u0443\u043c\u044d\u043d\u0020\u0438\u0020\u0431\u043e\u043c\u0431\u0430"),
				("7.2.6", Restored[1].Expected, Restored[1].Expected),
			};
			var offenders = Mismatches(reverted);
			offenders.Should().HaveCount(1, "seule la cellule revertee doit rougir : le temoin sain reste vert.");
			offenders[0].Should().Contain("1.3.2.title_ru").And.Contain("\u0431\u043e\u043c\u0431\u0430");
		}

		[Fact]
		public void Handed_Off_Titles_Are_Still_Unwritten()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var offenders = new List<string>();
			foreach (var (path, current) in HandedOff)
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
	}
}
