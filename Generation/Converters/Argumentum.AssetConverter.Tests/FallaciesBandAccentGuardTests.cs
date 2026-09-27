using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde des 5 accents de bandeau du pool v19 grain 4b (règle typographique
	/// permanente « on corrige tout le temps », owner 25/09). #369
	/// (<c>9d45b4f9</c>) a accenté les titres « Équivoque » (855) et « Évasion »
	/// (1313) sans suivre les bandeaux <c>Soussousfamille</c>. Le gabarit imprime
	/// « Sous-Famille | Soussousfamille » dès que Soussousfamille est rempli
	/// (revue #1588) : les cellules « self » des cartes 855/1313 s'impriment
	/// aussi — l'extension demandée en revue #1589 les couvre. Garde SANS
	/// exception : chaque bandeau de ces 5 cartes porte l'accent.
	/// ⚠️ Périmètre délibérément restreint au dispatch : les porteuses non
	/// imprimées de la forme non accentuée (sous-arbres 5.3.2 / 7.2.1) relèvent
	/// du grain 5 (62 accents, pool v20).
	/// NB : HaveCount(1) + Be, jamais ContainSingle(valeur, parce-que) — surcharge
	/// vacue (mesurée #1581).
	/// </summary>
	public class FallaciesBandAccentGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static string BandOf(string path)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("Soussousfamille", "path", new[] { path });
			values.Should().HaveCount(1, "une seule rangée porte ce path.");
			return values[0];
		}

		private static string TitleOf(string path)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("text_fr", "path", new[] { path });
			values.Should().HaveCount(1, "une seule rangée porte ce path.");
			return values[0];
		}

		[Fact]
		public void Card869_BandFr_Is_Accented_Equivoque()
		{
			BandOf("5.3.2.3.2.1").Should().Be("Équivoque",
				"v19 grain 4b : la carte 869 imprime le bandeau de son ancêtre 5.3.2 « Équivoque » — " +
				"#369 avait accenté le titre sans suivre les bandeaux (mesure 4a, #1588).");
		}

		[Fact]
		public void Cards1314_1330_BandFr_Is_Accented_Evasion()
		{
			BandOf("7.2.1.1").Should().Be("Évasion",
				"v19 grain 4b : la carte 1314 imprime le bandeau de son ancêtre 7.2.1 « Évasion ».");
			BandOf("7.2.1.2.2").Should().Be("Évasion",
				"v19 grain 4b : la carte 1330 imprime le bandeau de son ancêtre 7.2.1 « Évasion ».");
		}

		[Fact]
		public void SelfBands_855_1313_Is_Accented()
		{
			BandOf("5.3.2").Should().Be("Équivoque",
				"extension demandée en revue #1589 : le gabarit imprime la ligne " +
				"« Sous-Famille | Soussousfamille » — la carte 855 affiche donc « … | Équivoque » " +
				"au-dessus de son titre ; son propre bandeau porte l'accent.");
			BandOf("7.2.1").Should().Be("Évasion",
				"extension demandée en revue #1589 : la carte 1313 affiche « … | Évasion » — " +
				"son propre bandeau porte l'accent.");
		}

		[Fact]
		public void FixedCards_Band_Matches_Level3_Ancestor_Title()
		{
			foreach (var (card, ancestor) in new[]
			{
				("5.3.2.3.2.1", "5.3.2"), ("7.2.1.1", "7.2.1"), ("7.2.1.2.2", "7.2.1"),
				("5.3.2", "5.3.2"), ("7.2.1", "7.2.1"),
			})
			{
				var band = BandOf(card);
				var title = TitleOf(ancestor);
				band.Should().Be(title,
					$"l'invariant local « bandeau de niveau 3 = titre de l'ancêtre » tient pour la carte {card} " +
					$"({band} vs {title}) — une des deux cellules aurait rebougé.");
			}
		}

		[Fact]
		public void No_Deck_Card_Carries_The_Unaccented_Forms()
		{
			var deckBands = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("Soussousfamille", "carte", new[] { "1", "2" });
			deckBands.Count(b => string.Equals(b, "Equivoque", StringComparison.Ordinal)
			                     || string.Equals(b, "Evasion", StringComparison.Ordinal)).Should().Be(0,
				"aucune carte deck n'imprime plus la forme non accentuée : les 5 porteuses deck " +
				"(855, 869, 1313, 1314, 1330) sont corrigées — l'exception nommée de la première version " +
				"est retirée, la garde n'admet plus aucun bandeau imprimé non accentué.");
		}
	}
}
