using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
	/// <summary>
	/// Garde #1539 sur l'arbre + témoins du détecteur. Le grain a été livré le 25/09/2026 :
	/// 40 cellules (5 cartes × 8 langues) portaient « * 5 cartes mémo » et consorts, corrigées en
	/// « * Cartes mémo » — le chiffre part, la ligne reste (décision owner « Q-14 ok pour a »).
	///
	/// Les témoins exercent le détecteur dans les DEUX sens : un chiffre ouvrant la ligne de
	/// matériel doit être vu (et dans les quatre systèmes que l'issue nomme), une ligne corrigée
	/// ne doit pas l'être, et un plancher de présence doit rendre rouge la disparition de la ligne.
	/// </summary>
	public class RulesMaterialMemoCountGateTests
	{
		private static readonly string[] Pks = { "Rules_02", "Rules_09", "Rules_11", "Rules_13", "RulesPP_02" };

		// Ligne de matériel « mémo » SAINE (corrigée) et FAUTIVE (d'avant #1539), par langue.
		// Mesurées sur le corpus, pas inventées : la forme corrigée est celle que porte le fichier
		// aujourd'hui, la forme fautive celle qu'il portait au commit d'avant.
		private static readonly Dictionary<string, (string Saine, string Fautive)> ParLangue = new()
		{
			["Text"] = ("* Cartes mémo.", "* 5 cartes mémo."),
			["Text_en"] = ("* Memo cards.", "* 5 memo cards."),
			["Text_ru"] = ("* Карты Мемо", "* 5 карт Мемо"),
			["Text_pt"] = ("* Cartas de ajuda.", "* 5 cartas de ajuda."),
			["Text_ar"] = ("* بطاقات تذكير", "* 5 بطاقات تذكير"),
			["Text_es"] = ("* Cartas recordatorio", "* 5 cartas recordatorio"),
			["Text_zh"] = ("* 提示卡", "* 5张提示卡"),
			["Text_fa"] = ("* کارت‌های یادآور", "* ۵ کارتِ یادآور"),
		};

		// ───────────────────────────── garde sur l'arbre ─────────────────────────────

		[Fact]
		public void No_Material_Bullet_Announces_A_Memo_Count_On_Head()
		{
			var failures = RulesMaterialMemoCountGate.FindViolations(TestRepoRoot.Find());

			failures.Should().BeEmpty(
				"la liste de matériel des cartes de règles ne doit annoncer aucun nombre de cartes mémo — "
				+ "7 en boîte, 5 en P&P A4, 1 en P&P Light, donc aucun chiffre n'est juste partout (#1539). "
				+ "Offenders: {0}",
				string.Join(" | ", failures));
		}

		[Fact]
		public void Every_Language_Still_Carries_Its_Memo_Material_Line_On_Head()
		{
			// Le plancher est porté par CompareTables, mais il est asserté ici NOMMÉMENT sur l'arbre :
			// le compte exact (40 = 5 cartes × 8 langues) doit être visible à la lecture du rapport,
			// pas seulement déduit d'un seuil.
			var deck = ReadTable(RulesMaterialMemoCountGate.DeckCsvName);
			var printAndPlay = ReadTable(RulesMaterialMemoCountGate.PrintAndPlayCsvName);

			var cells = (from table in new[] { deck, printAndPlay }
						 from pk in table.Keys
						 from column in RulesMaterialMemoCountGate.TextColumns
						 where RulesMaterialMemoCountGate.MemoBullets(table[pk][column]).Any()
						 select $"{pk} · {column}").ToList();

			cells.Should().HaveCount(RulesMaterialMemoCountGate.MinMemoCells,
				"chaque carte de règles doit conserver sa ligne de matériel « mémo » dans les 8 langues — "
				+ "une ligne supprimée tiendrait l'invariant « pas de chiffre » par disparition, pas par correction. "
				+ "Présentes: {0}", string.Join(" | ", cells.OrderBy(c => c, StringComparer.Ordinal)));
		}

		[Fact]
		public void Deck_And_PrintAndPlay_Agree_On_Rules_02_For_Every_Language()
		{
			// #1527 impose la parité deck ↔ P&P ; #1539 change les DEUX côtés ensemble. Cette assertion
			// locale nomme la carte concernée, là où RulesTextParity la couvre sur les six.
			var deck = ReadTable(RulesMaterialMemoCountGate.DeckCsvName);
			var printAndPlay = ReadTable(RulesMaterialMemoCountGate.PrintAndPlayCsvName);
			var failures = new List<string>();
			foreach (var column in RulesMaterialMemoCountGate.TextColumns)
			{
				if (!string.Equals(deck["Rules_02"][column], printAndPlay["RulesPP_02"][column], StringComparison.Ordinal))
				{
					failures.Add(column);
				}
			}

			failures.Should().BeEmpty(
				"Rules_02 et RulesPP_02 doivent rester identiques sur les 8 colonnes de texte : #1539 les a "
				+ "modifiées ensemble, et une correction d'un seul côté romprait la parité #1502/#1527. "
				+ "Colonnes divergentes: {0}", string.Join(", ", failures));
		}

		// ───────────────────────── témoins du détecteur ─────────────────────────

		[Theory]
		[InlineData("* 5 cartes mémo", "arabe occidental")]
		[InlineData("* ٥ بطاقات تذكير", "arabo-indien")]
		[InlineData("* ۵ کارتِ یادآور", "arabo-indien étendu")]
		[InlineData("* 五张备忘卡", "chinois")]
		public void Detector_Sees_Each_Digit_System(string line, string systeme)
		{
			var digit = RulesMaterialMemoCountGate.LeadingDigit(line);

			digit.Should().NotBeNull(
				"un item de liste qui ouvre par un chiffre doit être vu, quel que soit le système de chiffres "
				+ "({0}) — l'issue nomme les quatre, et un contrôle qui n'en verrait qu'un rendrait un 0 "
				+ "trompeur sur les trois autres", systeme);
			digit.Should().Contain(systeme);
		}

		[Fact]
		public void Cjk_Ideographs_Are_Not_Char_IsDigit_So_The_Detector_Cannot_Delegate_To_It()
		{
			// Le piège, exécutable : les arabo-indiens SONT des chiffres pour le CLR, les idéogrammes
			// chinois NON (OtherLetter). Une détection qui déléguerait à char.IsDigit laisserait donc
			// passer « * 五张备忘卡 » — et l'issue exige explicitement le chinois.
			char.IsDigit('5').Should().BeTrue();
			char.IsDigit('٥').Should().BeTrue();
			char.IsDigit('۵').Should().BeTrue();
			char.IsDigit('五').Should().BeFalse(
				"五 n'est pas DecimalDigitNumber — c'est précisément pourquoi l'énumération des systèmes "
				+ "est écrite en clair dans la garde au lieu d'être déléguée à char.IsDigit");

			RulesMaterialMemoCountGate.LeadingDigit("* 五张备忘卡").Should().NotBeNull();
		}

		[Theory]
		[InlineData("* Cartes mémo.")]
		[InlineData("* Карты Мемо")]
		[InlineData("* کارت‌های یادآور")]
		public void Detector_Does_Not_Fire_On_A_Corrected_Line(string line)
		{
			RulesMaterialMemoCountGate.LeadingDigit(line).Should().BeNull(
				"la forme corrigée n'ouvre par aucun chiffre — un détecteur qui vire au rouge sur la "
				+ "correction est un instrument qui interdit sa propre réparation");
		}

		[Fact]
		public void Witness_A_Digit_Initial_Memo_Bullet_Is_Red()
		{
			var (deck, pp) = HealthyTables();
			var pk = "Rules_02";
			var column = "Text_fa";
			deck[pk][column] = $"*Règles du jeu*\n{ParLangue[column].Fautive}\n* Règles du jeu";

			var failures = RulesMaterialMemoCountGate.CompareTables(deck, pp);

			failures.Should().Contain(f => f.Contains("[chiffre]") && f.Contains(pk) && f.Contains(column),
				"le défaut #1539 doit rendre rouge en NOMMANT la carte et la langue — c'est ce qui le rend "
				+ "corrigeable sans re-dérouler l'instrument. Reçu: {0}", string.Join(" | ", failures));
			failures.Should().Contain(f => f.Contains("arabo-indien étendu"),
				"le chiffre persan doit être nommé dans son système : un message qui dirait seulement "
				+ "« un chiffre » ne dirait pas lequel des quatre, et le relecteur chercherait « 5 »");
		}

		[Fact]
		public void Witness_Healthy_Tables_Are_Green()
		{
			var (deck, pp) = HealthyTables();

			RulesMaterialMemoCountGate.CompareTables(deck, pp).Should().BeEmpty(
				"40 cellules saines = 5 cartes × 8 langues : le plancher est atteint exactement, aucun "
				+ "chiffre n'ouvre une ligne de matériel");
		}

		[Fact]
		public void Witness_A_Missing_Memo_Bullet_Is_Red_On_The_Floor()
		{
			var (deck, pp) = HealthyTables();
			deck["Rules_09"]["Text"] = "*Règles du jeu*\n* 1 paquet de cartes\n* Règles du jeu";

			var failures = RulesMaterialMemoCountGate.CompareTables(deck, pp);

			failures.Should().Contain(f => f.Contains("[plancher]") && f.Contains("39"),
				"supprimer la ligne éteindrait l'invariant « pas de chiffre » sans rien corriger : le "
				+ "plancher de présence doit rendre rouge une disparition, et nommer le compte obtenu. "
				+ "Reçu: {0}", string.Join(" | ", failures));
		}

		[Fact]
		public void Witness_A_Duplicated_Memo_Bullet_Is_Red()
		{
			var (deck, pp) = HealthyTables();
			deck["Rules_11"]["Text_es"] += $"\n{ParLangue["Text_es"].Saine}";

			var failures = RulesMaterialMemoCountGate.CompareTables(deck, pp);

			failures.Should().Contain(f => f.Contains("[doublon]") && f.Contains("Rules_11") && f.Contains("Text_es"),
				"deux lignes de matériel « mémo » dans la même cellule annoncent le même matériel deux fois — "
				+ "un défaut que le seul invariant « pas de chiffre » ne verrait pas. Reçu: {0}",
				string.Join(" | ", failures));
		}

		// ───────────────────────────── helpers ─────────────────────────────

		/// <summary>
		/// Grille saine en mémoire : 5 cartes × 8 langues, chacune portant sa ligne de matériel
		/// corrigée — soit exactement <see cref="RulesMaterialMemoCountGate.MinMemoCells"/> cellules.
		/// </summary>
		private static (Dictionary<string, Dictionary<string, string>> Deck,
						Dictionary<string, Dictionary<string, string>> PrintAndPlay) HealthyTables()
		{
			var deck = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
			foreach (var pk in Pks)
			{
				var row = new Dictionary<string, string>(StringComparer.Ordinal);
				foreach (var column in RulesMaterialMemoCountGate.TextColumns)
				{
					row[column] = $"*Règles du jeu*\n* 1 paquet\n{ParLangue[column].Saine}\n* Règles du jeu";
				}
				deck[pk] = row;
			}
			// RulesPP_02 vit dans le second fichier : les 40 cellules sont réparties 32 + 8.
			var pp = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal)
			{
				["RulesPP_02"] = deck["RulesPP_02"],
			};
			deck.Remove("RulesPP_02");
			return (deck, pp);
		}

		private static Dictionary<string, Dictionary<string, string>> ReadTable(string csvName)
		{
			var path = System.IO.Path.Combine(TestRepoRoot.Find(), "Cards", "Rules", csvName);
			var reader = new HarvestCardIdsCsv(path);
			var pks = reader.LoadColumn("pk");
			var columns = RulesMaterialMemoCountGate.TextColumns
				.ToDictionary(c => c, c => reader.LoadColumn(c), StringComparer.Ordinal);
			var table = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
			for (var i = 0; i < pks.Count; i++)
			{
				table[pks[i]] = RulesMaterialMemoCountGate.TextColumns
					.ToDictionary(c => c, c => columns[c][i], StringComparer.Ordinal);
			}
			return table;
		}
	}
}