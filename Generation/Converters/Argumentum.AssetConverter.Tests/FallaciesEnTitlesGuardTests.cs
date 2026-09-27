using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du grain ⑥ du pool v20 (décision 3 d'ai-01, fidélité des titres anglais,
	/// première impression anglaise 2.0.0). Épingle les 5 titres changés, leurs voisins
	/// gardés, l'unicité des nouveaux libellés, et le suivi des 14 bandeaux
	/// « Vagueness » → « Imprecision » du sous-arbre 3.3.1.
	/// ⚠️ <see cref="P1005_AdHocRescue_Held_PendingArbitration"/> épingle l'état SUSPENDU :
	/// la cible « Ad hoc rescue » de la décision est déjà portée par la carte deck 55
	/// « Sauvetage ad hoc » — l'exécuter tel quel créerait la collision que la preuve
	/// « 0 nouvelle collision » du dispatch interdit. Arbitrage ai-01 en attente.
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
		public void PinnedNeighbours_KeepTheirLabels()
		{
			EnTitleOf("6.2.2.5.1").Should().Be("Moving the goalposts",
				"1005 SUSPENDU : sa cible « Ad hoc rescue » est déjà portée par la carte deck 55 — " +
				"état gelé en attendant l'arbitrage ai-01 (voir P1005_AdHocRescue_Held_PendingArbitration).");
			EnTitleOf("6.2").Should().Be("Moving the goalposts",
				"973 garde son libellé exact (changement silencieux des critères = sens exact).");
			EnTitleOf("1.1.3").Should().Be("Ad hoc rescue",
				"la carte deck 55 « Sauvetage ad hoc » reste l'unique porteur — le motif du gel de 1005.");
			EnTitleOf("5.3.2.1").Should().Be("Vagueness",
				"856 garde « Vagueness » : termes vagues = sens exact, la décision ne le touche pas.");
			EnTitleOf("4.1.2.6").Should().Be("Gambler's fallacy",
				"« Gambler's fallacy » gardé sur 654/717 : collision d'origine à l'import (décision).");
		}

		[Fact]
		public void NewTitles_HaveExactlyOneCarrier_Each()
		{
			var all = AllEnTitles();
			foreach (var title in new[] { "Circular definition", "Appeal to lack of accomplishment", "Imprecision",
				                            "Contrast framing", "Hook", "Ad hoc rescue" })
			{
				all.Count(t => string.Equals(t, title, StringComparison.Ordinal)).Should().Be(1,
					$"«{title}» doit avoir exactement un porteur — 0 nouvelle collision (preuve du dispatch).");
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
	}
}
