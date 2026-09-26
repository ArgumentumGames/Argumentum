using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde des 5 retours règle C du pool v19 grain 3 (décisions ai-01 du 26/09, revues
	/// #1584/#1585 — délégation owner du 09/09, veto ouvert). Trois rangées hors deck
	/// reprennent le libellé qu'elles portaient avant la passe qui a créé leur collision :
	/// <list type="bullet">
	/// <item>PK 178 (<c>2.1.1.1</c>) fr/en/ru — #369 (<c>9d45b4f9</c>) a aligné son titre sur
	/// « Question piège » de son fils 179 et de la 701 ; le libellé d'avant se lit dans le
	/// parent du commit qui l'a changé (fr « Argument par la question », en/ru changés par
	/// <c>95db4425</c> : « Argument by question », « Аргумент через вопрос »).</item>
	/// <item>PK 96 (<c>1.2.2</c>) fr — #369 l'a aligné sur « Appel à la nature » de ses 4
	/// cartes filles (98, 104, 105, 108) ; avant : « Sophisme naturaliste ». Corroboration :
	/// LTfr de la rangée vaut 20, la longueur du libellé restauré.</item>
	/// <item>PK 717 (<c>4.1.2.6</c>) ru — la passe #397 (<c>5e2477b5</c>) l'a aligné sur
	/// « Ошибка игрока » de la 654, effaçant une distinction que le français garde
	/// (« Sophisme du joueur » ≠ « Erreur du parieur ») ; avant : « Софизм игрока ».</item>
	/// </list>
	/// La garde échoue aussi si un libellé restauré devient porté par plus d'une rangée
	/// (échanger une homonymie contre une autre), ou si une rangée voisine épinglée bouge.
	/// NB : exprimé en HaveCount(1) + Be, jamais ContainSingle(valeur, parce-que) — cette
	/// surcharge résout vers ContainSingle(because) qui n'épingle PAS la valeur.
	/// </summary>
	public class FallaciesRuleCRestoresGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static readonly string[] Pk96 = { "96" };
		private static readonly string[] Pk178 = { "178" };
		private static readonly string[] Pk179 = { "179" };
		private static readonly string[] Pk654 = { "654" };
		private static readonly string[] Pk717 = { "717" };

		private static void AssertSingleRowCell(string column, string[] pk, string expected, string because)
		{
			var values = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn(column, "PK", pk);
			values.Should().HaveCount(1, "une seule rangée porte ce PK dans la taxonomie.");
			values[0].Should().Be(expected, because);
		}

		[Fact]
		public void Pk178_TitleFr_Is_Pre369_Label()
		{
			AssertSingleRowCell("text_fr", Pk178, "Argument par la question",
				"v19 grain 3 : PK 178 (2.1.1.1, hors deck) reprend le libellé lu dans git show " +
				"9d45b4f9^ — #369 l'avait aligné sur « Question piège » (179/701), créant la " +
				"collision (dossier #1584 corrigé).");
		}

		[Fact]
		public void Pk178_TitleEn_Is_PrePass_Label()
		{
			AssertSingleRowCell("text_en", Pk178, "Argument by question",
				"l'anglais avait été aligné sur « Loaded question » par 95db4425 ; le retour fr " +
				"du 26/09 s'applique aux trois langues.");
		}

		[Fact]
		public void Pk178_TitleRu_Is_PrePass_Label()
		{
			AssertSingleRowCell("text_ru", Pk178, "Аргумент через вопрос",
				"même passe 95db4425 pour le russe ; octets d'avant lus dans le parent du commit.");
		}

		[Fact]
		public void Pk96_TitleFr_Is_Pre369_Label()
		{
			AssertSingleRowCell("text_fr", Pk96, "Sophisme naturaliste",
				"v19 grain 3 : PK 96 (1.2.2, hors deck) reprend son libellé d'avant #369 ; les " +
				"bandeaux imprimés de ses 4 cartes filles (98/104/105/108) le portent déjà. " +
				"Corroboration : LTfr de la rangée = 20 = longueur du libellé restauré.");
		}

		[Fact]
		public void Pk717_TitleRu_Is_Pre397_Label()
		{
			AssertSingleRowCell("text_ru", Pk717, "Софизм игрока",
				"v19 grain 3 : #397 (5e2477b5) avait aligné la 717 sur « Ошибка игрока » de la " +
				"654, effaçant une distinction que le français garde (« Sophisme du joueur » ≠ " +
				"« Erreur du parieur ») — perte, donc retour.");
		}

		[Fact]
		public void PinnedNeighbours_KeepTheirLabels()
		{
			AssertSingleRowCell("text_fr", Pk179, "Question piège",
				"le fils 2.1.1.1.1 (carte deck) garde « Question piège » : seul le parent est renommé.");
			AssertSingleRowCell("text_ru", Pk654, "Ошибка игрока",
				"la 654 « Erreur du parieur » garde son libellé russe : le retour de la 717 " +
				"rétablit la distinction, il ne la déplace pas.");
		}

		[Fact]
		public void RestoredLabels_HaveExactlyOneCarrier_EachColumn()
		{
			var fr = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("text_fr");
			fr.Count(t => string.Equals(t, "Argument par la question", StringComparison.Ordinal)).Should().Be(1,
				"le renommage ne doit pas échanger une homonymie contre une autre : le libellé " +
				"restauré de la 178 n'était porté par aucune autre rangée (mesuré 26/09).");
			fr.Count(t => string.Equals(t, "Sophisme naturaliste", StringComparison.Ordinal)).Should().Be(1,
				"après le retour de la 96, aucune rangée ne doit partager son libellé.");
			fr.Count(t => string.Equals(t, "Appel à la nature", StringComparison.Ordinal)).Should().Be(1,
				"la collision 96/108 est résolue : seule la carte fille 108 (1.2.2.4) garde " +
				"« Appel à la nature » comme titre exact — les 4 filles 98/104/105/108 partagent " +
				"le bandeau Famille, pas le titre (mesuré 26/09).");

			var en = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("text_en");
			en.Count(t => string.Equals(t, "Argument by question", StringComparison.Ordinal)).Should().Be(1,
				"unique porteuse en anglais après le retour.");

			var ru = new HarvestCardIdsCsv(FallaciesCsv).LoadColumn("text_ru");
			ru.Count(t => string.Equals(t, "Аргумент через вопрос", StringComparison.Ordinal)).Should().Be(1,
				"unique porteuse en russe après le retour.");
			ru.Count(t => string.Equals(t, "Софизм игрока", StringComparison.Ordinal)).Should().Be(1,
				"le retour de la 717 rétablit deux libellés distincts : « Софизм игрока » (717) " +
				"et « Ошибка игрока » (654).");
		}
	}
}
