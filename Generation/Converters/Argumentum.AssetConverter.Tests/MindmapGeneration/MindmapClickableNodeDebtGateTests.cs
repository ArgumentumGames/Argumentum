using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// #1698 — plafond de dette UNIDIRECTIONNEL sur les nœuds muets des cartes mentales
	/// committées. Mesure d'origine : ai-01 (lecture seule, master <c>9e765e4b</c>, issue
	/// #1698) — des nœuds restent muets (aucune donnée attachée : pas de <c>class="node"</c>,
	/// le clic n'ouvre pas la fiche) parce que des retitrages ont créé des titres identiques,
	/// attendus par la polyhiérarchie. Les chiffres ci-dessous sont CEUX D'AI-01,
	/// contre-vérifiés par cette lane le 02/10 avec l'instrument même de cette porte
	/// (égalité exacte 9/9, les deux compteurs <c>class="node"</c> et <c>depth</c> d'accord).
	///
	/// <para><b>Pourquoi un plafond et pas une égalité.</b> Le correctif d'appariement
	/// un-pour-un (<c>MindmapSvgNodePairingTests</c>) corrige le GÉNÉRATEUR, pas les SVG
	/// committés — ceux-ci ne bougent que par la re-dérivation n° 3 (FreeMind, po-2023), qui
	/// n'a pas encore tourné sur le corpus courant. Une porte stricte « nœuds cliquables =
	/// nœuds texte » serait donc ROUGE dès son merge. La porte actuelle n'autorise qu'une
	/// direction : la dette peut DIMINUER (re-dérivation), jamais AUGMENTER. Elle passera à
	/// l'égalité stricte après la re-dérivation n° 3 (DoD #1698 : 1408/1408 cliquables par
	/// langue et dans cards_fr).</para>
	///
	/// <para><b>Instrument.</b> « Nœud texte » = <c>g</c> portant au moins un <c>text</c>
	/// DIRECT (le même prédicat que <c>CollectPossibleSvgNodes</c>, qui ne voit que les textes
	/// directs). « Cliquable » = <c>g</c> avec <c>class="node"</c>, posé par
	/// <c>UpdateSvgMatch</c>. Le compte <c>depth</c> (second instrument d'ai-01) est
	/// asserté égal au compte cliquable — les deux instruments doivent continuer de
	/// coïncider, sinon l'un des deux est cassé et la mesure ne veut plus rien dire.</para>
	///
	/// <para><b>Anti-vacuité.</b> Le total de nœuds texte est épinglé à 1408 par fichier
	/// (une rangée de taxonomie = un nœud ; le compte de rangées est lui-même épinglé par
	/// <c>DuplicateConceptTitleAlignmentGuardTests</c>) : un parseur silencieusement aveugle
	/// rendrait la porte verte à vide.</para>
	/// </summary>
	public class MindmapClickableNodeDebtGateTests
	{
		private static readonly XNamespace Svg = "http://www.w3.org/2000/svg";

		// (clé, plancher de nœuds cliquables) — dette muette mesurée par ai-01 sur 9e765e4b :
		// fr 4 · en 1 · ru 1 · pt 1 · es 2 · ar 12 · fa 16 · zh 4 · cards_fr 4 (sur 1408).
		[Theory]
		[InlineData("fr", 1404)]
		[InlineData("en", 1407)]
		[InlineData("ru", 1407)]
		[InlineData("pt", 1407)]
		[InlineData("es", 1406)]
		[InlineData("ar", 1396)]
		[InlineData("fa", 1392)]
		[InlineData("zh", 1404)]
		[InlineData("cards_fr", 1404)]
		public void CommittedMindmaps_ClickableNodes_DoNotFallBelowDebtFloor(string key, int floor)
		{
			var repoRoot = TestRepoRoot.Find();
			var relativePath = key == "cards_fr"
				? Path.Combine("fr", "Argumentum_Fallacies_MindMap_cards_fr.content.svg")
				: Path.Combine(key, $"Fallacies_{key}.content.svg");
			var svgPath = Path.Combine(repoRoot, "Cards", "Fallacies", "Mindmaps", relativePath);
			File.Exists(svgPath).Should().BeTrue($"la carte mentale committée doit exister : {relativePath}");

			var svgDoc = XDocument.Load(svgPath);
			var textNodes = svgDoc.Descendants(Svg + "g")
				.Where(g => g.Elements(Svg + "text").Any())
				.ToList();
			var clickable = textNodes.Count(g => (string?)g.Attribute("class") == "node");
			var withDepth = svgDoc.Descendants(Svg + "g")
				.Count(g => g.Attribute("depth") != null);

			// Anti-vacuité : l'instrument voit bien les 1408 nœuds texte (une rangée de
			// taxonomie = un nœud). Recalibrer avec la date si le corpus change de taille,
			// jamais effacer la ligne.
			textNodes.Should().HaveCount(1408,
				$"anti-vacuité : {relativePath} porte 1408 nœuds texte (une rangée de taxonomie par nœud). " +
				"Un compte différent signifie que l'instrument est aveugle ou que la taxonomie a changé de taille " +
				"— recalibrer avec la date, jamais effacer la ligne.");

			// Les deux instruments d'ai-01 doivent coïncider : la mesure ne vaut que par là.
			withDepth.Should().Be(clickable,
				$"les deux instruments (class=\"node\" et depth) donnaient le même compte sur {relativePath} " +
				"à la mesure d'origine — leur divergence casse la mesure elle-même");

			// Plafond de dette, en sens unique : la dette peut diminuer (re-dérivation n° 3),
			// jamais augmenter. DoD #1698 : 1408/1408 après la re-dérivation — à ce moment-là,
			// remplacer ce plancher par l'égalité stricte.
			clickable.Should().BeGreaterThanOrEqualTo(floor,
				$"plafond de dette unidirectionnel #1698 ({relativePath}) : la mesure d'origine est " +
				$"{floor} cliquables sur 1408 (dette {1408 - floor}); un compte INFÉRIEUR signifie que des nœuds " +
				"ont PERDU leurs données — régression interdite. Un compte supérieur (re-dérivation n° 3) " +
				"doit s'accompagner du passage à l'égalité stricte.");
		}
	}
}
