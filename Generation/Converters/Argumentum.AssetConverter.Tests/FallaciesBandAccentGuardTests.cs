using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde des 3 accents de bandeau du pool v19 grain 4b (règle typographique
	/// permanente « on corrige tout le temps », owner 25/09). #369
	/// (<c>9d45b4f9</c>) a accenté les titres « Équivoque » (855) et « Évasion »
	/// (1313) sans suivre les bandeaux <c>Soussousfamille</c> des cartes deck :
	/// la mesure 4a (#1588) compte ces 3 écarts fr sur l'imprimé. Cette garde
	/// épingle les 3 cellules corrigées ET le retour à l'invariant local
	/// « bandeau de niveau 3 = titre de l'ancêtre de niveau 3 » pour ces cartes.
	/// ⚠️ Périmètre délibérément restreint au dispatch : les 45 autres porteuses
	/// non imprimées de la forme non accentuée (sous-arbres 5.3.2 / 7.2.1) et les
	/// 2 cellules « self » (cartes 5.3.2 et 7.2.1) restent à l'arbitrage ai-01.
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
		public void FixedCards_Band_Matches_Level3_Ancestor_Title()
		{
			foreach (var (card, ancestor) in new[] { ("5.3.2.3.2.1", "5.3.2"), ("7.2.1.1", "7.2.1"), ("7.2.1.2.2", "7.2.1") })
			{
				var band = BandOf(card);
				var title = TitleOf(ancestor);
				band.Should().Be(title,
					$"l'invariant local « bandeau de niveau 3 = titre de l'ancêtre » tient pour la carte {card} " +
					$"({band} vs {title}) — une des deux cellules aurait rebougé.");
			}
		}

		[Fact]
		public void Only_The_Two_Named_Self_Cells_Still_Carry_The_Unaccented_Forms()
		{
			var deckBands = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("Soussousfamille", "carte", new[] { "1", "2" });
			deckBands.Count(b => string.Equals(b, "Equivoque", StringComparison.Ordinal)
			                     || string.Equals(b, "Evasion", StringComparison.Ordinal)).Should().Be(2,
				"seules les 2 cellules « self » nommées ci-dessous portent encore la forme non accentuée " +
				"parmi les cartes deck — une 3e porteuse = un bandeau imprimé non accentué de plus (mesure 4a).");
			BandOf("5.3.2").Should().Be("Equivoque",
				"cellule self (la carte 855 EST l'ancêtre de niveau 3) : hors périmètre du dispatch 4b, " +
				"arbitrage ai-01 en attente — ce fait l'épingle pour que la liste d'exceptions ne fasse que rétrécir.");
			BandOf("7.2.1").Should().Be("Evasion",
				"cellule self (la carte 1313 EST l'ancêtre de niveau 3) : même arbitrage en attente.");
		}
	}
}
