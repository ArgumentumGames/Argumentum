using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.CardPenConfiguration
{
	/// <summary>
	/// Organe de la décision (i) du pool #458 (28/10/2026) : les deux documents Tarot Print&amp;Play
	/// ouvraient sur une page de dos <b>vide</b> (chunk 0 = les 6 cartes Rules, aucune ne portant de
	/// dos ; #1536 émet quand même la page de dos pour préserver l'appariement recto-verso).
	///
	/// <para>
	/// Ce qui a été <b>mesuré</b> avant le geste, et non supposé : les Rules n'ont de dos <b>nulle
	/// part</b>. Le Tarot imprimé les émet faces seules — pages 1 à 15 d'un document de 379 pages,
	/// comparées au pixel aux PNG sources (MAE 0,3 à 6,2 contre <c>rules_NN_face</c>, 75,6 contre le
	/// dos des sophismes) — et le seul dos du deck est celui des sophismes
	/// (<c>Fallacies/card_001.png</c>, MAE 0,8 contre les pages 30 et 32, même sha256). « Donner aux
	/// Rules le dos qu'elles ont dans le Tarot » n'a donc qu'une lecture possible : leur donner
	/// <b>le dos du deck</b>, déclaré explicitement.
	/// </para>
	///
	/// <para>
	/// Le risque à ne pas rouvrir est <b>#138</b>, qui a retiré ce dos parce que les Rules recevaient
	/// un dos <b>arbitraire</b>. Cette voie arbitraire est le repli de <see cref="ImageFileGenerator.ResolveCardBack"/>,
	/// et il n'est atteignable qu'avec <b>plusieurs</b> dos dans le dictionnaire — or
	/// <see cref="ImageFileGenerator"/> construit ce dictionnaire <b>dans</b> la boucle par CardSet.
	/// Le gabarit de dos des sophismes ne produit qu'une image de dos, donc la branche <c>Count == 1</c>
	/// rend ce dos à <b>toutes</b> les faces, sans sous-chaîne ni repli. Les deux moitiés sont
	/// épinglées ici : le contrat de config (Facts 1-2), la mécanique déterministe (Fact 3), et son
	/// contrôle inverse (Fact 4) — sans lequel un « pas de repli » ne prouverait rien.
	/// </para>
	///
	/// <para>
	/// Portée volontairement limitée au Print&amp;Play (Fact 2) : le Tarot imprimé reste à 379 pages
	/// avec des Rules recto seul. La décision (i) ne change pas le deck, elle change les deux PDF
	/// à imprimer chez soi — et le nombre de pages de ceux-ci ne bouge pas non plus, puisque la page
	/// de dos existait déjà, blanche (Fact 5).
	/// </para>
	///
	/// <para>
	/// ⚠ <b>L'instrument, et ce qu'il a fallu corriger en l'écrivant</b> : la première version de ce
	/// test asserait <c>BackCardSetInfo</c> <i>null</i> ou <i>non-null</i>. C'est un instrument
	/// aveugle — <see cref="CardSetConfig"/> initialise la propriété à <c>new CardSetInfo()</c>
	/// (<c>Cardpen/CardSetConfig.cs:21</c>), donc elle n'est <b>jamais</b> nulle : le « non-null »
	/// était vert même avant cette PR, et le « null » était impossible. Le <b>discriminant</b> est le
	/// <i>gabarit</i> : <see cref="CardSetInfo.GetJsonFilePath"/> sélectionne Debug ou Release, et une
	/// info vide rend <c>null</c>, que <see cref="CardSetInfo.GetCardSetDocument"/> traduit en
	/// « pas de document de dos » (<c>Cardpen/CardSetInfo.cs:57-60</c>) ⇒ aucun dos moissonné ⇒
	/// <c>hadNoAvailableBack</c> ⇒ faces seules, page de dos blanche en Print&amp;Play. On asserte donc
	/// les <b>deux chemins</b> (couvre les deux modes sans construire l'<c>AssetConverterConfig</c>
	/// complet), et le Fact 2 sert de contrôle inverse : sur l'arbre sain, le prédicat doit pouvoir
	/// être <b>faux</b> quelque part, sinon le Fact 1 ne prouve rien.
	/// </para>
	/// </summary>
	public class RulesPrintAndPlayBackContractTests
	{
		private const string DeckBackTemplate = "Argumentum_Fallacies_Back_fr.json";

		private static WebBasedGeneratorConfig Config() => new WebBasedGeneratorConfig();

		private static CardSetConfig CardSet(string name) =>
			Config().CardSets.Single(cs => cs.Name == name);

		[Fact]
		public void RulesPrintAndPlay_Declares_The_Deck_Back()
		{
			var back = CardSet(KnownCardSets.RulesPrintAndPlay).BackCardSetInfo;

			// Précondition, pas discriminant : CardSetConfig initialise BackCardSetInfo à une info vide,
			// donc « non null » est vrai même sans dos déclaré. Ce qui compte est le gabarit ci-dessous.
			back.Should().NotBeNull("l'info de dos est toujours instanciée par CardSetConfig (défaut vide)");

			back.JsonFilePathDebug.Should().NotBeNullOrEmpty(
				"sans chemin Debug, GetCardSetDocument rend null et le chunk Rules émet une page de dos vide : " +
				"c'est l'état d'avant la décision (i) du pool #458");
			back.JsonFilePathDebug.Should().EndWith(DeckBackTemplate,
				"le seul dos existant dans le deck est celui des sophismes (mesuré au pixel) : " +
				"il n'existe aucun gabarit de dos propre aux Rules");

			back.JsonFilePathRelease.Should().NotBeNullOrEmpty(
				"sans chemin Release, la régénération — qui est le mode de production — n'aurait pas de dos");
			back.JsonFilePathRelease.Should().EndWith(DeckBackTemplate,
				"Debug et Release doivent désigner le même dos, sinon la relecture et la régénération divergent");
		}

		[Fact]
		public void The_Printed_Tarot_Rules_Carry_No_Back_Template()
		{
			var back = CardSet(KnownCardSets.Rules).BackCardSetInfo;

			// Contrôle inverse du Fact précédent — la calibration de l'instrument. Le Tarot imprimé
			// doit rester sans gabarit de dos : c'est ce qui produit ses 15 pages de Rules recto seul.
			back.Should().NotBeNull(
				"CardSetConfig instancie toujours une info de dos : l'objet n'est jamais null, " +
				"c'est précisément pourquoi ce test n'asserte pas dessus");

			back.JsonFilePathDebug.Should().BeNullOrEmpty(
				"garde de portée : la décision (i) vise le Print&Play. Le Tarot imprimé émet les Rules " +
				"faces seules (15 pages sur 379, mesuré) ; y ajouter un dos changerait le produit, " +
				"pas seulement le PDF — c'est une autre décision, qui n'a pas été prise");
			back.JsonFilePathRelease.Should().BeNullOrEmpty(
				"idem en Release : un gabarit ici ferait passer le Tarot imprimé de 379 à un autre " +
				"nombre de pages, ce que la décision (i) n'autorise pas");
		}

		[Fact]
		public void One_Back_Reaches_Every_Rules_Face_Without_Fallback()
		{
			// Forme réelle après cette PR : le CardSet Rules-Print&Play porte exactement un dos,
			// et ImageFileGenerator construit son dictionnaire par CardSet (donc une seule entrée).
			var backImages = new Dictionary<string, string> { { "card_001", "/images/rulespp_back.png" } };

			var resolved = ImageFileGenerator.ResolveCardBack(
				"rulespp_01_face", backImages, out var hadNoAvailableBack, out var usedFallback);

			resolved.Should().Be("/images/rulespp_back.png",
				"avec un seul dos, ResolveCardBack le rend inconditionnellement (branche Count == 1)");
			hadNoAvailableBack.Should().BeFalse("le CardSet déclare désormais un dos");
			usedFallback.Should().BeFalse(
				"c'est le point exact de #138 : le dos ne doit pas venir du repli arbitraire");
		}

		[Fact]
		public void The_Arbitrary_Fallback_Only_Exists_With_Several_Backs()
		{
			// Contrôle inverse : il établit que le Fact précédent peut échouer. Sans lui, un
			// « usedFallback = false » serait indistinguable d'un instrument qui ne teste rien.
			var backImages = new Dictionary<string, string>
			{
				{ "-histoire", "/images/scenarii_histoire.png" },
				{ "-mythologie", "/images/scenarii_mythologie.png" },
			};

			var resolved = ImageFileGenerator.ResolveCardBack(
				"rulespp_01_face", backImages, out var hadNoAvailableBack, out var usedFallback);

			usedFallback.Should().BeTrue(
				"avec plusieurs dos et une face qui n'en contient aucun, le repli s'arme — c'est " +
				"précisément ce que #138 a retiré, et ce que la forme « un seul dos » rend inatteignable");
			hadNoAvailableBack.Should().BeFalse("il y a des dos disponibles, le repli n'est pas une absence");
			resolved.Should().NotBeNull("le repli rend tout de même une image — d'où le défaut silencieux de #138");
		}

		[Fact]
		public void Both_Tarot_PrintAndPlay_Documents_Consume_The_Rules_CardSet()
		{
			var docs = Config().CardSetDocuments
				.Where(d => d.DocumentName.Contains("TarotCards_Print&Play"))
				.ToList();

			docs.Should().HaveCount(2,
				"la décision (i) porte sur les DEUX documents Tarot Print&Play (complet et allégé)");

			foreach (var doc in docs)
			{
				doc.CardSets.Should().Contain(
					cs => cs.CardSetName == KnownCardSets.RulesPrintAndPlay,
					$"{doc.DocumentName} doit consommer le CardSet dont le dos vient d'être déclaré, " +
					"sinon la correction ne s'applique à aucun PDF");
			}
		}
	}
}
