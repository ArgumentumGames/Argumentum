using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde des grains ⑥ (pool v20) et ⑨ (pool v21) — fidélité des titres anglais,
	/// première impression anglaise 2.0.0. ⑥ : 5 titres + 14 bandeaux « Imprecision ».
	/// ⑨ (décisions ai-01 du 27/09 sur #1595/#1597) : 1005 arbitré « Backpedaling »,
	/// 6 titres retenus, 7 casses de phrase, 223 bandeaux EN qui suivent, desc_en 300
	/// (« it's » → « its »). Gardés : « Connivance », « Chewbacca defense » (règle C),
	/// « Moving the goalposts » (973 seul porteur — la collision 1005/973 est résolue).
	/// NB : HaveCount(1) + Be, jamais ContainSingle(valeur, parce-que) — surcharge vacue (#1581).
	/// </summary>
	public class FallaciesEnTitlesGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static string EnTitleOf(string path)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("text_en", "path", new[] { path });
			values.Should().HaveCount(1, "une seule rangée porte ce path.");
			return values[0];
		}

		private static IReadOnlyList<string> AllEnTitles()
			=> new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("text_en");

		[Fact]
		public void Five_Decision_Titles_Applied()
		{
			EnTitleOf("5.1.3.2").Should().Be("Circular definition",
				"grain ⑥ v20 : la fr distingue définir en cercle (829) et argumenter en cercle (699).");
			EnTitleOf("7.3.2.3").Should().Be("Appeal to lack of accomplishment",
				"grain ⑥ v20 : 1386 REJETE faute de réussite, 79 ACCORDE — la polarité s'inverse.");
			EnTitleOf("3.3.1").Should().Be("Imprecision",
				"grain ⑥ v20 : la fr distingue termes vagues (856, garde « Vagueness ») et données imprécises (667).");
			EnTitleOf("2.3.1.2.3.1").Should().Be("Contrast framing",
				"grain ⑥ v20 : la tactique sur l'adversaire (390), le biais cognitif garde « Contrast effect » (1040).");
			EnTitleOf("6.1.2.1.1").Should().Be("Hook",
				"grain ⑥ v20 : l'accroche réductrice (944), la petite phrase politique garde « Sound bite » (187).");
		}

		[Fact]
		public void Seven_V21_Titles_Applied()
		{
			EnTitleOf("6.2.2.5.1").Should().Be("Backpedaling",
				"⑨, arbitrage ai-01 27/09 : « Ad hoc rescue » et « Moving the goalposts » déjà pris par des " +
				"cartes deck (55, 973) — 1005 « Argument après contestation » prend « Backpedaling » " +
				"(orthographe américaine, majoritaire dans le corpus).");
			EnTitleOf("4").Should().Be("Faulty reasoning",
				"⑨ : « Faulty logics » était agrammatical — la desc_en dit elle-même « flawed reasoning ».");
			EnTitleOf("7.2").Should().Be("Debate sabotage",
				"⑨ : le complément « du débat » revient — le bandeau des filles dit ce qui est saboté.");
			EnTitleOf("3.3.3").Should().Be("Inappropriate operation",
				"⑨ : le sens mathématique revient, sans confusion avec « Faulty reasoning » (famille 4).");
			EnTitleOf("2.1.1.1.3").Should().Be("False alternative",
				"⑨ : « Alternative advance » n'est pas un terme ; ≠ 814 « False dilemma » (issues convergentes).");
			EnTitleOf("7.2.3.4").Should().Be("Sham arguments",
				"⑨ : les deux descs parlent d'arguments, pas de propositions.");
			EnTitleOf("6.2.3.3").Should().Be("Appeal to effort",
				"⑨ : « Notable effort » ne nommait pas un sophisme ; terme établi.");
		}

		[Fact]
		public void Seven_Casing_Fixes_Applied()
		{
			EnTitleOf("1.1.2").Should().Be("Trivial justification",
				"⑨ casse : le deck est en casse de phrase (7 titres, décision ai-01 27/09).");
			EnTitleOf("1.2.1").Should().Be("Argument from authority", "⑨ casse.");
			EnTitleOf("1.2.2.3.1").Should().Be("Appeal to precedent", "⑨ casse.");
			EnTitleOf("2.2.1.4").Should().Be("Appeal to pity", "⑨ casse.");
			EnTitleOf("2.3.2.3.4").Should().Be("Playing the victim", "⑨ casse.");
			EnTitleOf("3.3.1.3.4").Should().Be("Least plausible hypothesis", "⑨ casse.");
			EnTitleOf("6.2.2").Should().Be("Having your cake and eating it too", "⑨ casse.");
		}

		[Fact]
		public void KeptLabels_StayPut()
		{
			EnTitleOf("2.2.1").Should().Be("Connivance",
				"⑨ règle C : « Complicity » porte le même sens de complicité dans une faute — gardé.");
			EnTitleOf("7.2.1.2.2").Should().Be("Chewbacca defense",
				"⑨ règle C : forme anglaise du nom vulgarisé imprimé en 2022 (« Défense Chewbacca »).");
			EnTitleOf("6.2").Should().Be("Moving the goalposts",
				"973 garde son libellé — et devient l'unique porteur : la collision 1005/973 est résolue.");
			EnTitleOf("1.1.3").Should().Be("Ad hoc rescue", "la carte deck 55 « Sauvetage ad hoc ».");
			EnTitleOf("5.3.2.1").Should().Be("Vagueness", "856 : termes vagues = sens exact.");
			EnTitleOf("4.1.2.6").Should().Be("Gambler's fallacy",
				"« Gambler's fallacy » gardé sur 654/717 : collision d'origine à l'import (décision).");
		}

		[Fact]
		public void Every_Decision_Title_HasExactlyOneCarrier()
		{
			var all = AllEnTitles();
			foreach (var title in new[] {
					// grain ⑥
					"Circular definition", "Appeal to lack of accomplishment", "Imprecision",
					"Contrast framing", "Hook", "Ad hoc rescue",
					// grain ⑨ — renommages et arbitrage
					"Backpedaling", "Faulty reasoning", "Debate sabotage", "Inappropriate operation",
					"False alternative", "Sham arguments", "Appeal to effort",
					// grain ⑨ — casses
					"Trivial justification", "Argument from authority", "Appeal to precedent",
					"Appeal to pity", "Playing the victim", "Least plausible hypothesis",
					"Having your cake and eating it too",
					// résolution de collision
					"Moving the goalposts" })
			{
				all.Count(t => string.Equals(t, title, StringComparison.Ordinal)).Should().Be(1,
					$"«{title}» doit avoir exactement un porteur — aucune réaffectation d'IRI (décision ⑨).");
			}
			all.Count(t => string.Equals(t, "Vagueness", StringComparison.Ordinal)).Should().Be(1,
				"seul 856 porte encore « Vagueness » en titre.");
		}

		[Fact]
		public void Imprecision_Bands_Follow_Under_331()
		{
			var paths = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("path");
			var subsub = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("Subsubfamily");
			var subfam = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("Subfamily");
			int vaguenessBands = 0, imprecisionBands = 0;
			for (var i = 0; i < paths.Count; i++)
			{
				var p = paths[i].Trim();
				if (!(p == "3.3.1" || p.StartsWith("3.3.1.", StringComparison.Ordinal)))
					continue;
				if (subsub[i].Trim() == "Vagueness" || subfam[i].Trim() == "Vagueness")
					vaguenessBands++;
				if (subsub[i].Trim() == "Imprecision" || subfam[i].Trim() == "Imprecision")
					imprecisionBands++;
			}
			vaguenessBands.Should().Be(0,
				"plus aucun bandeau en du sous-arbre 3.3.1 ne porte « Vagueness » — ils ont suivi le titre.");
			imprecisionBands.Should().Be(14,
				"les 14 bandeaux mesurés (le sien + 4 cartes filles deck + 9 descendants hors deck) portent « Imprecision ».");
		}

		[Fact]
		public void RetiredForms_AbsentFrom_Titles_And_Bands()
		{
			var retired = new[] {
				"Faulty logics", "Sabotage", "Wrong reasoning", "Alternative advance",
				"False propositions", "Notable effort",
				"Trivial Justification", "Argument from Authority", "Appeal to Precedent",
				"Appeal to Pity", "Playing the Victim", "Least Plausible Hypothesis",
				"Having Your Cake and Eating It Too" };
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			var columns = new[] { "text_en", "Family", "Subfamily", "Subsubfamily" };
			foreach (var col in columns)
			{
				var values = csv.LoadColumn(col);
				foreach (var form in retired)
					values.Count(v => string.Equals(v.Trim(), form, StringComparison.Ordinal)).Should().Be(0,
						$"la forme retirée «{form}» ne doit plus exister en {col} — les bandeaux ont suivi (223 cellules, ⑨).");
			}
		}

		[Fact]
		public void RenamedHeads_OwnBands_MatchTheirNewTitles()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			BandOf(csv, "4", "Family").Should().Be("Faulty reasoning",
				"la tête de famille porte son propre titre en bandeau (leçon #1588 : self cells).");
			BandOf(csv, "7.2", "Subfamily").Should().Be("Debate sabotage");
			BandOf(csv, "3.3.3", "Subsubfamily").Should().Be("Inappropriate operation");
			BandOf(csv, "1.1.2", "Subsubfamily").Should().Be("Trivial justification");
			BandOf(csv, "1.2.1", "Subsubfamily").Should().Be("Argument from authority");
			BandOf(csv, "6.2.2", "Subsubfamily").Should().Be("Having your cake and eating it too");
		}

		[Fact]
		public void Desc300_Uses_Its_Not_ItsApostrophe()
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("desc_en", "path", new[] { "2.2.1" });
			values.Should().HaveCount(1);
			values[0].Should().Contain("win its support",
				"⑨ : correction typographique décidée avec le gardé « Connivance » (#1597 c., ai-01).");
			values[0].Should().NotContain("win it's support", "la forme fautive est retirée.");
		}

		private static string BandOf(HarvestCardIdsCsv csv, string path, string column)
		{
			var values = csv.LoadColumn(column, "path", new[] { path });
			values.Should().HaveCount(1, "une seule rangée porte ce path.");
			return values[0].Trim();
		}
	}
}
