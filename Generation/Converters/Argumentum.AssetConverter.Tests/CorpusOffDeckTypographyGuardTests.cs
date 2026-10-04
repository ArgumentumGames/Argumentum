using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde des tirets typographiques HORS deck (pool #458, dispatch
	/// c.5975630522 grain 3, inventaire d'origine g35/#1734) : 65 cellules
	/// ASCII « - » → cadratin espacé « — » sur les rangées non imprimées
	/// (carte vide), mêmes colonnes text_*/desc_*/example_*.
	/// <list type="bullet">
	/// <item>Réconciliation avec le « 84 rangées » du dispatch (mesure g35
	/// PRÉ-#1736) : 119 cellules / 104 rangées mesurées − le deck (exécuté
	/// par #1736) − Remarques (37 cellules, interne non rendue, excluse par
	/// le dispatch) − intervalles numériques (règle appliquée, **0
	/// occurrence** mesurée) = **65 cellules / 58 rangées** écrites ici.</item>
	/// <item>Répartition : example_ru 46 · desc_ru 8 · example_ar 5 ·
		/// example_pt 2 · example_fr 1 · example_fa 1 · text_fa 1 · text_ru 1.</item>
	/// <item>Les doublons hors deck 1055/1341 des cartes 51/121 example_ru
	/// sont convertis et **synchronisés avec leur jumeau deck** (la garde
	/// l'épingle par égalité pleine cellule).</item>
	/// <item>Le balayage du deck vit dans CorpusDialogueTypographyGuardTests
	/// (#1736) — non dupliqué ici.</item>
	/// </list>
	/// </summary>
	public class CorpusOffDeckTypographyGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		/// <summary>Intervalle numérique « 2010 - 2020 » : exclu de la conversion (règle du dispatch) — le tiret y est séparateur de nombres, pas ponctuation.</summary>
		private static readonly Regex NumericInterval = new(@"(?<=\d) - (?=\d)", RegexOptions.Compiled);

		private static string Cell(string column, string pk)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column, "PK", new[] { pk });
			values.Should().HaveCount(1, "une seule rangée porte le PK {0} dans la taxonomie.", pk);
			return values[0];
		}

		[Fact]
		public void OffDeck_TextDescExample_NoAsciiDashRemains()
		{
			var cartes = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("carte");
			var pks = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("PK");
			foreach (var field in new[] { "text", "desc", "example" })
			{
				foreach (var lang in new[] { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" })
				{
					var column = field + "_" + lang;
					var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column);
					values.Count.Should().Be(cartes.Count, "les deux colonnes couvrent les mêmes rangées.");
					for (var i = 0; i < values.Count; i++)
					{
						if (!string.IsNullOrWhiteSpace(cartes[i]))
						{
							continue; // deck : balayé par CorpusDialogueTypographyGuardTests
						}
						var cell = values[i] ?? string.Empty;
						var ascii = Regex.Matches(cell, Regex.Escape(" - ")).Count;
						var numeric = NumericInterval.Matches(cell).Count;
						(ascii - numeric).Should().Be(0,
							"tiret ASCII espacé interdit hors deck ({0}, PK {1}) — grain 3 converti en cadratin ; "
							+ "les intervalles numériques (chiffre - chiffre) restent en ASCII par règle",
							column, pks[i]);
					}
				}
			}
		}

		[Fact]
		public void OffDeckTwins_1055_1341_SynchronizedWithDeckCards()
		{
			// 1055 = copie hors deck de la carte 51 ; 1341 = copie de la 121 (example_ru) —
			// converties ici à l'identique du deck (#1736), l'égalité doit tenir.
			Cell("example_ru", "1055").Should().Be(Cell("example_ru", "51"),
				"1055 est le jumeau hors deck de la carte 51 — même texte, même tiret cadratin.");
			Cell("example_ru", "1341").Should().Be(Cell("example_ru", "121"),
				"1341 est le jumeau hors deck de la carte 121 — même texte, même tiret cadratin.");
		}

		[Fact]
		public void ConvertedPins_FullCell()
		{
			// deux pins pleine cellule tirées de la worklist : la seule cellule text_ru (1239)
			// et la première desc_ru (1051) — couvrent les colonnes secondaires du grain.
			Cell("text_ru", "1239").Should().Be("Эффект «меньше — значит лучше»",
				"1239 text_ru : « - » ASCII → cadratin espacé (grain 3, seule cellule text_ru).");
			Cell("desc_ru", "1051").Should().Be("Вам трудно представить, каково это — не знать что-то, что вы уже знаете.",
				"1051 desc_ru : cadratin espacé (grain 3, première des 8 desc_ru hors deck).");
		}
	}
}
