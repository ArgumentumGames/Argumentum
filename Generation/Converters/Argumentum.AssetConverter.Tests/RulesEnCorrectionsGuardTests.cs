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
	/// Grain 4 du pool c.5988407613 (#458) — les Règles EN + Rules_13 32 -&gt; 28.
	/// <para><b>Rules_13, les 8 langues</b> : la ligne de Matériel annonçait « 32 cartes » là où
	/// l'Installation dit « Sélectionnez 28 cartes… soit 4 par couleur » (7 × 4 = 28) — contradiction
	/// interne de la source (dossier regles-sources-fr-en §4.1), recopiée fidèlement par chaque
	/// langue. GO owner du 05/10 (« oui à tout » au dossier de l'imprimé 2022).</para>
	/// <para><b>EN, trois défauts du dossier §3</b> : « pack »/« package » pour le même objet que
	/// « deck » dit ailleurs (§3.3, Rules_09 et Rules_11) ; « classes of colors » non idiomatique
	/// contre « color classes »/« color-coded classes » (§3.4) ; le calque « on a scenario drawn »
	/// (§3.2, Rules_11) là où Rules_02/03 disent « a randomly drawn scenario ». Plus Rules_10 :
	/// « his/he » -&gt; « their/they » — la phrase n'a <b>jamais été imprimée</b> (vérifié par ai-01
	/// sur l'archive 2022, c.5988397998) et la cellule dit déjà « they » ailleurs — et deux
	/// cosmétiques du §3.6 (ligne vide double, virgule de « Otherwise »).</para>
	/// <para>⚠️ Le 3ᵉ cosmétique du §3.6 (majuscules « The smooth talker » dans la liste de
	/// Rules_10) <b>n'existe pas dans la cellule mesurée</b> (la liste réelle : « The Reader /
	/// The round of arguments / End of the round ») — écart consigné, non corrigé.</para>
	/// <para><b>Gardés</b> : « tied » (Rules_05) — fidèle au FR « tous déclarés vainqueurs »
	/// (arbitrage ai-01 05/10 : plusieurs vainqueurs à égalité, PAS une question de typographie).
	/// Le CSV Print&amp;Play est mesuré : il ne porte aucune ligne de sélection — rien à y écrire.</para>
	/// </summary>
	public class RulesEnCorrectionsGuardTests
	{
		private static string RulesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Rules", "Argumentum Rules - Cards.csv");

		private static string PpCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Rules", "Argumentum Rules - Cards Print and Play.csv");

		/// <summary>
		/// Les 11 cellules écrites : rangée, colonne, empreinte SHA-256 de la cellule PLEINE
		/// (dérivée du fichier corrigé, jamais de mémoire), ce que la correction répare.
		/// </summary>
		private static readonly (string Path, string Column, string Sha256, string Why)[] Fixed =
		{
			("Rules_13", "Text", "ccbe33dfadba74970aa234104663f7faf9cf6e1786c75dd405dfa98d2617b28d", "«Une sélection de 32 cartes» -> 28 : la contradiction interne 32/28 (GO owner 05/10)"),
			("Rules_13", "Text_en", "7e97adaa2b85b5e6ba9504d9c81ca18575e3063a9bb11340ce59fb5a1865fb33", "\"A selection of 32 cards\" -> 28 : l'EN recopiait fidèlement la contradiction de la source"),
			("Rules_13", "Text_ru", "23b4fced369a3d594399490ec8539b9dfba55a83cc253958af40f206cd8d8a16", "«колода из 32 карт» -> 28 : même contradiction, langue jamais imprimée (délégation)"),
			("Rules_13", "Text_pt", "5ca3d33a898034d5c71df4833466709c15cd88dbf1376b1af1ce2712a8f95dac", "«Uma seleção de 32 cartas» -> 28 : même contradiction, langue jamais imprimée"),
			("Rules_13", "Text_ar", "3ea7636d8980c0799acd99b1cb1a442679fe0428e7e18dac2cca52432368bf0b", "«اختيار من 32 بطاقة» -> 28 : même contradiction, langue jamais imprimée"),
			("Rules_13", "Text_es", "1f2ffdd998fcdfc05d80451fa7cdd6f522a03b4a7b0a60a11c6f4a93acd269be", "«Una selección de 32 cartas» -> 28 : même contradiction, langue jamais imprimée"),
			("Rules_13", "Text_zh", "22694145f17f9d1429a7edd834b4727467fc3ebf4a78e2b194f5253c4c098110", "«共选用32张牌» -> 28 : même contradiction, langue jamais imprimée"),
			("Rules_13", "Text_fa", "9ba07232f878588bee3ba270a437920a61b67cfe3f21754a40a8e0fe21caa904", "«گزیده‌ای از ۳۲ کارت» -> ۲۸ : même contradiction, chiffres persans, langue jamais imprimée"),
			("Rules_09", "Text_en", "391faccbe5a576d473bfa180cf949b99b31508deda77823c0afd992d277d8f74", "pack + package -> deck ×2, classes of colors -> color classes : trois mots pour un objet (§3.3/§3.4)"),
			("Rules_10", "Text_en", "1c8da2df07f8ee48ad6df5faf55bb774b623e8786aad2ef89442cdb045bc1b17", "his/he -> their/they (jamais imprimé, ai-01) + virgule «Otherwise,» + ligne vide double retirée"),
			("Rules_11", "Text_en", "6b97c0cbf827aac3a86be0b3e1f54f476e0d82456b55b65bd1a2b28b39feb808", "idem Rules_09 + le calque «on a scenario drawn» -> «from a randomly drawn scenario» (§3.2)"),
		};

		/// <summary>Les formes déviantes, chacune mesurée ≥ 1 avant la correction et 0 après.</summary>
		private static readonly string[] EradicatedForms =
		{
			"A selection of 32 cards", "Une sélection de 32 cartes", "seleção de 32 cartas",
			"selección de 32 cartas", "из 32 карт", "من 32 بطاقة", "选用32张牌", "از ۳۲ کارت",
			"1 pack of", "1 package of", "classes of colors", "on a scenario drawn",
			"all his cards", "he wins the game", "Otherwise the game stops",
		};

		/// <summary>Le négatif de l'écran ne voit pas une disparition : contrôle positif.</summary>
		private static readonly string[] RestoredForms =
		{
			"A selection of 28 cards", "Une sélection de 28 cartes", "seleção de 28 cartas",
			"selección de 28 cartas", "из 28 карт", "من 28 بطاقة", "选用28张牌", "از ۲۸ کارت",
			"1 deck of fallacious argument cards organized in 7 color classes",
			"1 deck of scenario cards organized in 7 identifiable themes",
			"from a randomly drawn scenario", "all their cards, they win the game.",
			"Otherwise, the game stops",
		};

		private static string Sha256Hex(string cell)
		{
			using var sha = SHA256.Create();
			return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(cell))).ToLowerInvariant();
		}

		private static string Corpus()
		{
			var sb = new StringBuilder();
			sb.Append(File.ReadAllText(RulesCsv, Encoding.UTF8));
			sb.Append('\n');
			sb.Append(File.ReadAllText(PpCsv, Encoding.UTF8));
			return sb.ToString();
		}

		internal static List<string> Mismatches(IEnumerable<(string Path, string Column, string Sha)> cells)
		{
			var csv = new HarvestCardIdsCsv(RulesCsv);
			var offenders = new List<string>();
			foreach (var (path, column, sha) in cells)
			{
				var v = csv.LoadColumn(column, "pk", new[] { path });
				if (v.Count != 1)
					offenders.Add($"{path}.{column} : {v.Count} rangée(s) lue(s), 1 attendue");
				else if (Sha256Hex(v[0]) != sha)
					offenders.Add($"{path}.{column} : empreinte «{sha[..12]}…» attendue, lue «{Sha256Hex(v[0])[..12]}…»");
			}
			return offenders;
		}

		[Fact]
		public void Corrected_Cells_Match_Their_Sha_Pins()
		{
			Mismatches(Fixed.Select(f => (f.Path, f.Column, f.Sha256))).Should().BeEmpty(
				"#458 grain 4 : Rules_13 dit 28 dans les 8 langues, l'EN dit deck/color classes/from a randomly drawn scenario/they — repasser une seule cellule doit la nommer.");
		}

		[Fact]
		public void Fixed_Table_Is_Not_Vacuous()
		{
			Fixed.Should().HaveCount(11, "8 cellules Rules_13 (les 8 langues) + Rules_09/10/11 Text_en.");
			Fixed.Select(f => f.Path).Distinct().Should().HaveCount(4, "Rules_09, Rules_10, Rules_11, Rules_13.")
				.And.BeEquivalentTo(new[] { "Rules_09", "Rules_10", "Rules_11", "Rules_13" });
			Fixed.Select(f => f.Column).Distinct().Should().HaveCount(8,
				"les 8 colonnes de langue de Rules_13 — Text_en sert aussi Rules_09/10/11.");
			Fixed.Should().OnlyContain(f => f.Sha256.Length == 64, "une empreinte SHA-256 fait 64 caractères hexadécimaux.");
			Fixed.Should().OnlyContain(f => !string.IsNullOrWhiteSpace(f.Why), "chaque épingle dit ce qu'elle répare.");
			Fixed.Select(f => f.Why).Distinct().Should().HaveCount(11, "11 raisons distinctes.");
			Fixed.Select(f => f.Sha256).Distinct().Should().HaveCount(11, "11 cellules, 11 empreintes distinctes.");
		}

		[Fact]
		public void Deviating_Forms_Are_Absent_From_Both_Corpus()
		{
			var corpus = Corpus();
			var found = EradicatedForms.Where(f => corpus.Contains(f, StringComparison.Ordinal)).ToList();
			found.Should().BeEmpty(
				"les 15 formes déviantes étaient mesurées ≥ 1 avant la correction et 0 après — l'écran couvre le CSV principal ET le Print&Play.");
		}

		[Fact]
		public void Restored_Forms_Are_Present()
		{
			var corpus = Corpus();
			RestoredForms.Where(f => !corpus.Contains(f, StringComparison.Ordinal)).Should().BeEmpty(
				"l'absence d'une forme déviante ne prouve pas la présence de la forme restaurée.");
		}

		[Fact]
		public void Tied_Is_Kept_By_Arbitration()
		{
			var csv = new HarvestCardIdsCsv(RulesCsv);
			var v = csv.LoadColumn("Text_en", "pk", new[] { "Rules_05" });
			v.Should().HaveCount(1, "la rangée Rules_05 existe et est unique.");
			v[0].Should().Contain("tied",
				"GARDE (arbitrage ai-01, 05/10) : « tied » est fidèle au FR « tous déclarés vainqueurs » — " +
				"plusieurs vainqueurs à égalité. Ce n'est PAS une question de typographie : le retirer exige un nouvel arbitrage.");
		}

		[Fact]
		public void PrintAndPlay_Carries_No_Selection_Line()
		{
			var pp = File.ReadAllText(PpCsv, Encoding.UTF8);
			pp.Should().NotContain("32",
				"mesuré 05/10 : le CSV Print&Play ne porte aucune ligne de sélection de cartes — le GO owner " +
				"« mesure aussi le CSV PP » s'arrête à une mesure, il n'y a rien à y écrire.");
		}

		[Fact]
		public void Pin_Detector_Fires_On_A_Mutated_Cell_And_Spares_The_Witness()
		{
			var csv = new HarvestCardIdsCsv(RulesCsv);
			var rows = new List<(string, string, string)>();
			foreach (var (path, column, sha, _) in Fixed)
			{
				var v = csv.LoadColumn(column, "pk", new[] { path }).Single();
				var actual = Sha256Hex(v);
				if (path == "Rules_13" && column == "Text_en")
					actual = Sha256Hex(v.Replace("28 cards", "32 cards")); // la mutation testée
				rows.Add((path, column, actual));
			}
			var offenders = Mismatches(rows);
			offenders.Should().HaveCount(1, "seule la cellule mutée doit rougir : le témoin sain reste vert.");
			offenders[0].Should().Contain("Rules_13.Text_en");
		}
	}
}
