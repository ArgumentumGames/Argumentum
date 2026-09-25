using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
	/// <summary>
	/// Garde #1539/#1536 — la séquence de pages du Print &amp; Play doit être recto-verso.
	/// Relevé par ai-01 pendant le verdict visuel de la fenêtre #1525 C (24/09) : le Tarot P&P
	/// rendait 105 pages (impair) sur un document recto-verso, page 1 = faces des règles, page 2
	/// = dos du lot suivant — le verso des règles portait les dos Fallacies, et la dernière page
	/// n'avait pas de verso.
	/// </summary>
	/// <remarks>
	/// <para><b>Cause</b> : <c>PrintAndPlayDocument.Compose</c> n'émettait la page de dos d'un lot
	/// que si au moins une carte du lot en avait une. Le premier lot du Tarot P&amp;P (les 6 cartes
	/// Rules, sans dos) n'émettait donc aucun dos — et une imprimante recto-verso, qui associe les
	/// pages (1,2), (3,4)…, décalait alors chaque paire d'une page.</para>
	/// <para><b>Correction</b> : un document à dos émet TOUJOURS une page de dos par lot — blanche
	/// quand le lot n'en a pas. La séquence repasse à Back·Front·Back·Front…, paire, et chaque
	/// imprimante duplex imprime le verso du lot k derrière le recto du lot k.</para>
	/// <para><b>Le Poker P&amp;P ne bouge pas</b> (témoin de la revue) : chacun de ses lots porte un
	/// dos, la règle ancienne et la nouvelle émettent la même séquence — 38 pages avant comme après.</para>
	/// </remarks>
	public class PrintAndPlayRectoVersoParityContractTests
	{
		private static readonly bool[] TarotLikeDeck = // 318 instances : Rules(6, sans dos) + 312 avec dos
			Enumerable.Repeat(false, 6).Concat(Enumerable.Repeat(true, 312)).ToArray();

		private static readonly bool[] PokerDeck = Enumerable.Repeat(true, 167).ToArray();

		// ─────────────── les deux invariants, sur les decks réels ───────────────

		[Fact]
		public void TarotLike_Deck_With_A_Backless_First_Chunk_Alternates_And_Is_Even()
		{
			// Grille 3×2 = 6 cartes/planche : le premier lot (6 Rules sans dos) tombait sur sa
			// propre planche et n'émettait pas de dos — séquence F·B·F·B… impaire.
			var sequence = PrintAndPlayDocument.EmittedPageSequence(TarotLikeDeck, noBack: false, nbCardsPerPage: 6);

			sequence.Count.Should().Be(106,
				"53 planches × 2 pages : la planche 0 (6 Rules sans dos) émet désormais une page de dos BLANCHE (#1536)");

			for (var i = 0; i < sequence.Count; i += 2)
			{
				sequence[i].Should().Be(PrintAndPlayDocument.PageKind.Back,
					"les pages impaires (0, 2, 4…) doivent être des dos — sinon la paire recto-verso est décalée d'une page");
				sequence[i + 1].Should().Be(PrintAndPlayDocument.PageKind.Front,
					"les pages paires (1, 3, 5…) doivent être des faces — chaque feuille imprimée recto-verso associe (dos, faces) du même lot");
			}
		}

		[Fact]
		public void A_Deck_Whose_First_Chunk_Has_Backs_Alternates_Too()
		{
			// Témoin : un deck sans cartes sans dos ne dépend pas de la correction — il alternait déjà.
			var sequence = PrintAndPlayDocument.EmittedPageSequence(PokerDeck, noBack: false, nbCardsPerPage: 9);

			sequence.Count.Should().Be(38, "ceil(167/9) = 19 planches × 2 pages — inchangé, avant comme après #1536");
			for (var i = 0; i < sequence.Count; i += 2)
			{
				sequence[i].Should().Be(PrintAndPlayDocument.PageKind.Back);
				sequence[i + 1].Should().Be(PrintAndPlayDocument.PageKind.Front);
			}
		}

		// ─────────────────── témoins rouges : la dérive d'avant #1536 ───────────────────

		[Fact]
		public void Witness_A_Backless_First_Chunk_Without_A_Blank_Back_Was_Red()
		{
			// Le défaut, rejoué : si Compose revenait à « pas de dos = pas de page de dos », le
			// premier lot deviendrait une page de faces seule et la paire (0,1) associerait les
			// faces du lot 0 aux dos du lot 1. Ce témoin vérifie que la GARDE le voit rouge —
			// sans lui, une régression de Compose repasserait verte sur les deux Facts du haut
			// tant que le compte resterait juste par accident.
			var buggySequence = BuggyEmittedPageSequence(TarotLikeDeck, noBack: false, nbCardsPerPage: 6);

			buggySequence.Count.Should().Be(105,
				"avant #1536 : 53 planches mais la planche 0 n'émet pas de dos ⇒ 52 dos + 53 faces = 105, IMPAIR");
			buggySequence[0].Should().Be(PrintAndPlayDocument.PageKind.Front,
				"le défaut était exactement : page 0 = faces, page 1 = dos du lot suivant — le recto-verso est décalé dès la première feuille");

			// Et la même garde, sur le deck corrigé, doit être verte : le contrôle inverse.
			var fixedSequence = PrintAndPlayDocument.EmittedPageSequence(TarotLikeDeck, noBack: false, nbCardsPerPage: 6);
			fixedSequence.Count.Should().Be(106, "la correction ajoute une page de dos blanche à la planche 0");
			fixedSequence[0].Should().Be(PrintAndPlayDocument.PageKind.Back,
				"page 0 = dos (blanc) des 6 Rules, page 1 = leurs faces — la première feuille est déjà correctement appariée");
		}

		[Fact]
		public void Witness_An_Odd_Page_Count_Is_Itself_A_Defect_On_A_Duplex_Document()
		{
			// Un document recto-verso a un nombre PAIR de pages : un compte impair annonce une
			// page sans verso. La garde ne se contente pas d'alterner — elle épinglé aussi la
			// parité, parce que c'est la mesure qui a révélé #1536 (105 pages).
			var sequence = PrintAndPlayDocument.EmittedPageSequence(TarotLikeDeck, noBack: false, nbCardsPerPage: 6);

			(sequence.Count % 2).Should().Be(0,
				"un document recto-verso dont la dernière page n'a pas de verso imprime une feuille à demi vide — "
				+ "le compte de pages doit être pair dès que le document a des dos");
		}

		// ─────────────────── invariant général : NoBack = faces seules ───────────────────

		[Fact]
		public void NoBack_Documents_Emit_Faces_Only_Unchanged()
		{
			// #1536 ne touche pas les documents sans dos : leur séquence est toute de faces.
			var sequence = PrintAndPlayDocument.EmittedPageSequence(TarotLikeDeck, noBack: true, nbCardsPerPage: 6);

			sequence.Count.Should().Be(53, "sans dos : 53 planches de faces, une par planche");
			sequence.Should().OnlyContain(p => p == PrintAndPlayDocument.PageKind.Front);
		}

		/// <summary>La règle d'avant #1536, rejouée pour le témoin rouge.</summary>
		private static IReadOnlyList<PrintAndPlayDocument.PageKind> BuggyEmittedPageSequence(
			IReadOnlyList<bool> instanceHasBack, bool noBack, int nbCardsPerPage)
		{
			var sequence = new List<PrintAndPlayDocument.PageKind>();
			for (var chunk = 0; chunk * nbCardsPerPage < instanceHasBack.Count; chunk++)
			{
				var hasAnyBack = instanceHasBack
					.Skip(chunk * nbCardsPerPage).Take(nbCardsPerPage).Any(b => b);
				if (!noBack && hasAnyBack) sequence.Add(PrintAndPlayDocument.PageKind.Back);
				sequence.Add(PrintAndPlayDocument.PageKind.Front);
			}
			return sequence;
		}
	}
}