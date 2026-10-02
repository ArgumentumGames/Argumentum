using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// #1698/#1700 — appariement un-pour-un items CSV → nœuds SVG, indépendant de l'unicité des
	/// titres. Défaut mesuré par ai-01 (master <c>9e765e4b</c>, lecture seule) : des nœuds
	/// restent MUETS quand des retitrages ont créé des titres identiques — la polyhiérarchie
	/// place un même concept dans deux branches et le titre y est identique PAR CONSTRUCTION.
	///
	/// <para><b>La reprise #1700 corrige l'hypothèse d'ordre de la première version (rejetée,
	/// <c>caa5aee3</c>).</b> FreeMind peint les frères du DERNIER au PREMIER (Swing peint les
	/// enfants en ordre inverse) : l'ordre du document SVG est le parcours préfixe À FRÈRES
	/// INVERSÉS, mesuré par ai-01 sur <c>Fallacies_fr.content.svg</c> — 2 ruptures pour le
	/// préfixe inversé contre 891 pour le préfixe droit. L'appariement par rang de liste CSV
	/// (~170 fiches/langue) croisait les homonymes INTER-BRANCHES : chaque nœud ouvrait la
	/// fiche du mauvais concept. Le rang des items doit dériver de <c>Path</c> (segments
	/// comparés comme ENTIERS, divergence inversée, ancêtre avant descendant), pas de
	/// l'ordre d'entrée.</para>
	///
	/// <para>Cas couverts :</para>
	/// <list type="bullet">
	/// <item><description><b>Parent et enfant homonymes</b> — « Sophisme de l'accident »,
	/// PK 614 (<c>3.1.2</c>) et PK 615 (<c>3.1.2.1</c>) : le préfixe ne s'inverse PAS,
	/// l'ancêtre précède toujours ses descendants dans le document.</description></item>
	/// <item><description><b>Un titre sous 3 branches</b> — « Vrai Écossais » (PK 65 =
	/// <c>1.1</c>, PK 616 = <c>2.3</c>, PK 813 = <c>5.1.2</c>) : dans le document, la branche
	/// 5 est peinte AVANT la 2, elle-même avant la 1 — le renversement inter-branches est LE
	/// défaut de la version rejetée.</description></item>
	/// <item><description><b>Distracteurs plus longs</b> — des nœuds dont le texte CONTIENT le
	/// titre : le filtre de longueur minimale doit continuer de les écarter.</description></item>
	/// </list>
	///
	/// <para><b>Piège du test synthétique (la leçon de la rejetée)</b> : un test qui construit
	/// son SVG dans l'ordre qu'il suppose ne peut pas voir un défaut d'ordre. Ici le document
	/// est construit dans l'ordre préfixe inversé RÉEL et la liste d'items dans l'ordre CSV
	/// (les deux DIFFÈRENT) : la version <c>caa5aee3</c> rend ce test ROUGE sur le trio
	/// inter-branches. L'application aux SVG réels passe par la re-dérivation n° 3 (FreeMind,
	/// po-2023) : ce test ne régénère RIEN.</para>
	/// </summary>
	public class MindmapSvgNodePairingTests
	{
		private static readonly XNamespace Svg = "http://www.w3.org/2000/svg";
		private static readonly XNamespace XLink = "http://www.w3.org/1999/xlink";

		/// <summary>
		/// Item de test : <see cref="TextFr"/> est la propriété consommée par le
		/// <c>TitleExpression</c> par défaut (<c>{item.TextFr}</c>) ; <see cref="DescFr"/>,
		/// <see cref="ExampleFr"/> et <see cref="LinkFr"/> par les expressions par défaut de
		/// description/exemple/lien. Les valeurs sont propres à chaque instance pour que le
		/// test vérifie QUEL item a écrit sur QUEL nœud.
		/// </summary>
		private sealed class TestMindMapItem : IMindMapItem
		{
			public string Id { get; set; } = "";
			public string Path { get; set; } = "";
			public int Depth { get; set; }
			public string Family { get; set; } = "Contrôle";
			public string SubFamily { get; set; } = "";
			public string SubSubFamily { get; set; } = "";
			public string Title { get; set; } = "";
			public string Text => Title;
			public string TextFr { get; set; } = "";
			public string Description { get; set; } = "";
			public string DescFr { get; set; } = "";
			public string Example { get; set; } = "";
			public string ExampleFr { get; set; } = "";
			public string Link { get; set; } = "";
			public string LinkFr { get; set; } = "";
			public string LinkFrFallback => LinkFr;
			public int? Carte { get; set; }
			public string Pk { get; set; } = "";
			public string PK { get; set; } = "";
			public string DecimalPath { get; set; } = "";
			public string Famille { get; set; } = "Contrôle";
			public string SousFamille { get; set; } = "";
			public string Soussousfamille { get; set; } = "";
		}

		private static TestMindMapItem Item(string id, string title, string path, string decimalPath, int depth)
		{
			return new TestMindMapItem
			{
				Id = id,
				Title = title,
				TextFr = title,
				DescFr = $"description de {id}",
				ExampleFr = $"exemple de {id}",
				LinkFr = $"https://example.org/{id}",
				Path = path,
				DecimalPath = decimalPath,
				Depth = depth,
				Pk = id,
				PK = id,
			};
		}

		/// <summary>Un nœud SVG FreeMind minimal : un <c>g</c> portant un <c>text</c> direct.</summary>
		private static XDocument SyntheticSvg(params string[] nodeTexts)
		{
			var elements = nodeTexts.Select(t => new XElement(Svg + "g", new XElement(Svg + "text", t)));
			return new XDocument(new XElement(Svg + "svg", elements));
		}

		private static string NodeText(XElement g)
		{
			return string.Concat(g.Elements(Svg + "text").Select(t => t.Value));
		}

		/// <summary>Lecture d'attribut null-safe (chaîne vide si absent).</summary>
		private static string Attr(XElement e, XName name)
		{
			return (string?)e.Attribute(name) ?? "";
		}

		[Fact]
		public void Homonymous_Titles_Receive_OneToOne_Pairing_In_Batik_Document_Order()
		{
			// Items dans l'ordre de la liste CSV (préfixe DROIT) : parent puis enfant
			// homonymes (PK 614/615), un titre sous 3 branches (PK 65/616/813), un titre
			// unique témoin. C'est l'ordre que la version rejetée (caa5aee3) prenait pour
			// l'ordre du document — à tort.
			var typedItems = new List<TestMindMapItem>
			{
				Item("accident-parent", "Sophisme de l'accident", "3.1.2", "3,12", 3),
				Item("accident-child", "Sophisme de l'accident", "3.1.2.1", "3,121", 4),
				Item("ecossais-1", "Vrai Écossais", "1.1", "1,1", 2),
				Item("ecossais-2", "Vrai Écossais", "2.3", "2,3", 2),
				Item("ecossais-3", "Vrai Écossais", "5.2", "5,2", 2),
				Item("appel", "Appel à la nature", "4.1", "4,1", 2),
				// Parents des branches porteuses des homonymes : nécessaires au départage
				// par proximité du parent quand le groupe porte un surnuméraire (cas zh).
				Item("branche-un", "Branche Un", "1", "1", 1),
				Item("branche-deux", "Branche Deux", "2", "2", 1),
				Item("branche-cinq", "Branche Cinq", "5", "5", 1),
				// #1700 — titre de 2 caractères SANS candidat dans le SVG (la branche de
				// diagnostic de CollectPossibleSvgNodes) : ~150 titres zh font 2 caractères
				// (« 谬论 », « 偏见 »…) ; l'injection zh crasait en
				// ArgumentOutOfRangeException sur title.Substring(0, 3) avant le garde.
				Item("zh-court", "谬论", "6.1", "6,1", 2),
			};
			IList<IMindMapItem> items = typedItems.Cast<IMindMapItem>().ToList();

			// Ordre du document = parcours préfixe À FRÈRES INVERSÉS (mesure ai-01 : c'est ce
			// que FreeMind/Batik peint). Les branches se lisent 5, 4, 3, 2, 1 ; dans la
			// branche 3, l'ancêtre 3.1.2 précède son descendant 3.1.2.1 (le préfixe ne
			// s'inverse pas). Le PREMIER nœud est un SURNUMÉRAIRE : une occurrence du titre
			// écossais placée hors de toute branche porteuse (le cas zh mesuré — un nœud
			// orphelin « 循环论证 » sous 5.1.3.3, branche sans enfant CSV) : le k-ième↔k-ième
			// naïf le consommerait et décalerait TOUT le groupe. Les deux derniers nœuds
			// sont des distracteurs plus longs.
			var svgDoc = SyntheticSvg(
				"Branche Cinq",                  // 5     — branche-cinq
				"Vrai Écossais",                 // 5.2   — ecossais-3
				"Appel à la nature",             // 4.1   — appel
				"Vrai Écossais",                 // SURNUMÉRAIRE (orphelin d'un état antérieur
				                                 // de l'arbre, posé dans une branche non
				                                 // porteuse — le cas zh mesuré)
				"Sophisme de l'accident",        // 3.1.2   — accident-parent (ancêtre)
				"Sophisme de l'accident",        // 3.1.2.1 — accident-child (descendant)
				"Branche Deux",                  // 2     — branche-deux
				"Vrai Écossais",                 // 2.3   — ecossais-2
				"Branche Un",                    // 1     — branche-un
				"Vrai Écossais",                 // 1.1   — ecossais-1
				"Sophisme de l'accident et ses cousins",
				"Vrai Écossais — variante longue");

			var config = new FallacyMindMapDocumentConfig();
			var svgMap = new SVGFreemindMap { SetSVGNodeAttributes = true, WrapNodeByLink = true };
			config.UpdateSvgWithItems(svgMap, items, svgDoc, Svg, XLink);

			var attributed = svgDoc.Descendants(Svg + "g")
				.Where(g => Attr(g, "class") == "node")
				.ToList();

			// 1. Couverture : les SIX items Avec candidat reçoivent un nœud — l'ancien
			//    départage laissait muets parent ET enfant homonymes (défaut #1698 mesuré :
			//    1/6 attribué). Le 7e (titre zh de 2 caractères absent du SVG) reste SANS
			//    nœud, journalisé — sans l'exception qui tuait l'injection zh entière.
			attributed.Should().HaveCount(items.Count - 1,
				"chaque item homonyme ou non reçoit un nœud ; seul l'item sans candidat (titre zh " +
				"court absent du SVG) n'en reçoit pas — sans crasher : le diagnostic des candidats " +
				"proches ne doit pas découper le titre au-delà de sa longueur");

			// 2. Un-pour-un : un nœud ne porte les données que d'UN item.
			attributed.Select(g => Attr(g, "id")).Should().OnlyHaveUniqueItems(
				"l'appariement est un-pour-un : deux items homonymes ne peuvent pas écrire sur le même nœud");

			// 3. k-ième occurrence du document ↔ k-ième item AU RANG PRÉFIXE INVERSÉ — vérifié
			//    TITRE PAR TITRE, avec les distracteurs exclus par l'égalité exacte du texte.
			//    C'est L'assertion qui rougissait contre caa5aee3 : le trio inter-branches y
			//    était croisé (e1 sur le nœud de e3 et réciproquement).
			AssertPairingOrder(svgDoc, "Sophisme de l'accident", new[] { "accident-parent", "accident-child" },
				"parent et enfant homonymes (PK 614/615) : le préfixe ne s'inverse pas, l'ancêtre précède le descendant");
			AssertPairingOrder(svgDoc, "Vrai Écossais", new[] { "ecossais-3", "", "ecossais-2", "ecossais-1" },
				"titre sous 3 branches + un SURNUMÉRAIRE orphelin en zone non porteuse (cas zh mesuré) : chaque " +
				"item prend le nœud de SA branche par proximité du parent — le k-ième↔k-ième naïf consommait " +
				"l'orphelin à partir du 2e item et décalait tout le reste du groupe ; l'orphelin reste VIERGE");

			// 4. Chaque nœud porte SES données — valeur propre à l'item, pas seulement le
			//    compte (la moitié du défaut était invisible à un décompte).
			foreach (var item in typedItems.Where(i => i.Id != "zh-court"))
			{
				var node = attributed.Single(g => Attr(g, "id") == item.Id);
				Attr(node, "description").Should().Contain(item.DescFr,
					$"le nœud attribué à {item.Id} doit porter SA définition");
				Attr(node, "link").Should().Be(item.LinkFr,
					$"le nœud attribué à {item.Id} doit porter SON lien");
				Attr(node, "depth").Should().Be(item.Depth.ToString(),
					$"le nœud attribué à {item.Id} doit porter SA profondeur");
				node.Parent.Should().NotBeNull("WrapNodeByLink=true enveloppe chaque nœud attribué dans un <a>");
				node.Parent!.Name.LocalName.Should().Be("a",
					"le clic du lecteur passe par l'enveloppe <a> portée par le générateur");
				Attr(node.Parent!, XLink + "href").Should().Be(item.LinkFr,
					$"l'enveloppe <a> du nœud de {item.Id} doit pointer sur SON lien");
			}

			// 5. Les distracteurs plus longs restent vierges (aucune donnée) : le correctif
			//    ne doit pas élargir le périmètre d'attribution au-delà des occurrences.
			var distractors = svgDoc.Descendants(Svg + "g")
				.Where(g => NodeText(g).Contains("Sophisme de l'accident et ses cousins", StringComparison.Ordinal)
					|| NodeText(g).Contains("Vrai Écossais — variante longue", StringComparison.Ordinal))
				.ToList();
			distractors.Should().HaveCount(2, "le SVG synthétique porte exactement deux distracteurs");
			distractors.Should().OnlyContain(g => Attr(g, "class") != "node",
				"un nœud plus long qui CONTIENT un titre n'est pas une occurrence du concept");
		}

		[Fact]
		public void BatikDocumentOrder_Ranks_Segments_As_Integers_With_Reversed_Siblings()
		{
			// Le comparateur DÉRIVÉ de Path, épingle sur les cas limites mesurés : fratrie de
			// plus de 9 (une branche .24 se peint APRÈS .2 et AVANT .3 à l'échelle des frères —
			// le classement lexicographique la mettrait entre 2 et 3, l'artefact nommé par
			// ai-01), ancêtre avant descendant, branches inversées.
			var ordered = new[]
			{
				"5.1.10",            // fratrie > 9 : le frère 10 se peint avant le frère 2
				"5.1.2",
				"4.11",              // fratrie > 9 au niveau 1 : le frère 11 se peint avant le frère 3
				"4.3.2",             // l'ancêtre précède ses descendants…
				"4.3.2.24",          // …puis ses enfants inversés : 24 avant 1
				"4.3.2.1.1",
				"4.1",
				"3.1.2",             // ancêtre avant descendant : le préfixe ne s'inverse pas
				"3.1.2.1",
				"2.3",
				"1.1",
			};

			for (var i = 0; i < ordered.Length; i++)
			{
				for (var j = 0; j < ordered.Length; j++)
				{
					var actual = FallacyMindMapDocumentConfig.CompareBatikDocumentOrder(ordered[i], ordered[j]);
					var sign = Math.Sign(actual);
					var expected = Math.Sign(i.CompareTo(j));
					sign.Should().Be(expected,
						$"CompareBatikDocumentOrder(\"{ordered[i]}\", \"{ordered[j]}\") doit classer " +
						$"{(i < j ? $"\"{ordered[i]}\" avant" : i == j ? "égal" : $"\"{ordered[j]}\" avant")} — " +
						"segments ENTIERS, frères inversés, ancêtre d'abord");
				}
			}

			// La racine du map : le path "0" (l'unique rangée au segment 0 du CSV — « Argument
			// fallacieux », PK 0) et le path vide désignent le MÊME nœud racine, peint PREMIER :
			// sans frère, l'inversion des frères ne s'applique pas. Sans cette règle, le
			// segment 0 inverse la racine en DERNIERE — l'artefact racine mesuré sur les 9
			// content.svg committés (la racine ouvre chaque document).
			foreach (var root in new[] { "0", "" })
			{
				foreach (var other in ordered)
				{
					FallacyMindMapDocumentConfig.CompareBatikDocumentOrder(root, other).Should().BeNegative(
						$"la racine (\"{root}\") se peint avant \"{other}\"");
					FallacyMindMapDocumentConfig.CompareBatikDocumentOrder(other, root).Should().BePositive(
						$"\"{other}\" se peint après la racine (\"{root}\")");
				}
			}
			FallacyMindMapDocumentConfig.CompareBatikDocumentOrder("0", "").Should().Be(0,
				"le path \"0\" et le path vide désignent le même nœud racine : même rang");
		}

		/// <summary>
		/// #1700 suite A (revue c.5945920853, défaut introduit par la reprise — ne se produit que
		/// sur l'injection EN PLACE après un retitrage) : un nœud dont le texte EST ÉGAL au titre
		/// d'un item n'est pas attribuable à un item dont le titre est seulement CONTENU dans ce
		/// texte. Cas mesuré en arabe : le nœud exact de PK 975 « المغالطات الاعتراضية » était le
		/// seul candidat du groupe de PK 298 « الاعتراض » (traité plus tôt au rang 2.x) : le
		/// premier attribué gardait le nœud — fiche FAUSSE sur le nœud de 975, et 975 muet.
		/// </summary>
		[Fact]
		public void Exact_Title_Owns_Its_Node_Over_A_Containing_Title()
		{
			// Ordre CSV (préfixe droit) : l'item au titre CONTENU (« 2.1 », l'équivalent de
			// PK 298) vient AVANT l'item au titre EXACT (« 6.1 », l'équivalent de PK 975) —
			// c'est l'ordre qui traite le groupe voleur en premier.
			var typedItems = new List<TestMindMapItem>
			{
				Item("titre-contenu", "Titre Contenu", "2.1", "2,1", 2),
				Item("titre-exact", "Long Titre Contenu Étendu", "6.1", "6,1", 2),
			};
			IList<IMindMapItem> items = typedItems.Cast<IMindMapItem>().ToList();

			// Carte périmée : le nœud de « titre-exact » porte son titre exact (le retitrage
			// n'est pas encore re-dérivé) ; le nœud de « titre-contenu » porte un ancien
			// libellé absent du SVG de test — son texte ne matche plus rien.
			var svgDoc = SyntheticSvg("Long Titre Contenu Étendu");

			var config = new FallacyMindMapDocumentConfig();
			var svgMap = new SVGFreemindMap { SetSVGNodeAttributes = true, WrapNodeByLink = true };
			config.UpdateSvgWithItems(svgMap, items, svgDoc, Svg, XLink);

			var attributed = svgDoc.Descendants(Svg + "g")
				.Where(g => Attr(g, "class") == "node")
				.ToList();

			// Le nœud exact porte la fiche de l'item EXACT — pas celle de l'item au titre contenu.
			attributed.Should().ContainSingle(
				"un seul nœud existe et un seul item doit le recevoir : celui dont le titre EST le texte du nœud");
			var node = attributed.Single();
			Attr(node, "id").Should().Be("titre-exact",
				"un nœud qui porte exactement le titre d'un item n'est pas attribué à un autre item dont le " +
				"titre est seulement CONTENU dans ce texte (cas ar 298/975 : fiche fausse + 975 muet)");
			Attr(node, "description").Should().Contain(typedItems[1].DescFr,
				"le nœud doit porter la définition de l'item exact, pas celle de l'item au titre contenu");
		}

		/// <summary>
		/// #1700 suite B (revue c.5945920853) : quand un groupe d'homonymes a MOINS de nœuds que
		/// d'items, le texte ne permet pas de trancher — cas mesurés ar 295/296 et 43/46, fa
		/// 791/793 et 255/259 : le retitrage d'un DESCENDANT au titre de son ANCIÊTRE laisse un
		/// seul nœud pour deux items sur la carte périmée, et la carte committée portait le nœud
		/// du descendant alors que le rang d'appariement le donne à l'ancêtre. Principe de la PR
		/// elle-même : « un nœud muet vaut mieux qu'une fiche fausse » — ne rien attribuer.
		/// </summary>
		[Fact]
		public void Fewer_Nodes_Than_Homonyms_Leaves_The_Whole_Group_Unattributed()
		{
			// L'ancêtre (6.1) et le descendant retitré (2.1) : au rang préfixe inversé,
			// l'ancêtre 6.x vient AVANT le descendant 2.x — c'est lui que le code d'avant la
			// suite B attribuait (le cas mesuré : le rang donne l'ancêtre, la carte portait
			// le descendant).
			var typedItems = new List<TestMindMapItem>
			{
				Item("descendant-retitre", "Nom Commun", "2.1", "2,1", 2),
				Item("ancetre", "Nom Commun", "6.1", "6,1", 2),
				Item("temoin", "Branche Témoin", "4.1", "4,1", 2),
			};
			IList<IMindMapItem> items = typedItems.Cast<IMindMapItem>().ToList();

			// Ordre document (frères inversés) : la branche 6 avant la 4 avant la 2. Un seul
			// nœud « Nom Commun », dans la branche de l'ancêtre.
			var svgDoc = SyntheticSvg(
				"Nom Commun",
				"Branche Témoin");

			var config = new FallacyMindMapDocumentConfig();
			var svgMap = new SVGFreemindMap { SetSVGNodeAttributes = true, WrapNodeByLink = true };
			config.UpdateSvgWithItems(svgMap, items, svgDoc, Svg, XLink);

			var attributed = svgDoc.Descendants(Svg + "g")
				.Where(g => Attr(g, "class") == "node")
				.ToList();

			// Personne du groupe homonyme ne reçoit le nœud : le texte ne tranche pas entre
			// l'ancêtre et le descendant, la carte est périmée — attribuer serait deviner.
			var homonymNode = svgDoc.Descendants(Svg + "g")
				.Single(g => string.Equals(NodeText(g), "Nom Commun", StringComparison.Ordinal));
			Attr(homonymNode, "class").Should().NotBe("node",
				"un groupe d'homonymes sous-approvisionné en nœuds ne doit recevoir AUCUNE attribution : " +
				"le texte ne peut pas départager l'ancêtre du descendant retitré (ar 295/296, fa 791/793)");
			attributed.Select(g => Attr(g, "id")).Should().BeEquivalentTo(new[] { "temoin" },
				"le groupe ambigu reste muet mais les groupes sains continuent d'être appariés");
		}

		/// <summary>
		/// Les occurrences EXACTES d'un titre, dans l'ordre du document, doivent porter les ids
		/// des items homonymes dans l'ordre préfixe inversé.
		/// </summary>
		private static void AssertPairingOrder(XDocument svgDoc, string title, string[] expectedIdsInOrder, string because)
		{
			var occurrences = svgDoc.Descendants(Svg + "g")
				.Where(g => string.Equals(NodeText(g), title, StringComparison.Ordinal))
				.ToList();
			occurrences.Should().HaveCount(expectedIdsInOrder.Length,
				$"« {title} » doit exister exactement {expectedIdsInOrder.Length} fois dans le SVG synthétique " +
				"(les distracteurs plus longs sont exclus par l'égalité exacte)");
			occurrences.Select(g => Attr(g, "id"))
				.Should().Equal(expectedIdsInOrder, because);
		}
	}
}
