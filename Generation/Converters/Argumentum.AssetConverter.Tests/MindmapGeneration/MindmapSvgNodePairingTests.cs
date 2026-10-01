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
	/// #1698 — appariement un-pour-un items CSV → nœuds SVG, indépendant de l'unicité des
	/// titres. Le défaut mesuré par ai-01 (master <c>9e765e4b</c>, lecture seule) : sur les
	/// cartes mentales des sophismes, des nœuds restent MUETS (aucune donnée attachée, le clic
	/// n'ouvre pas la fiche) quand des retitrages ont créé des titres identiques — la
	/// polyhiérarchie place un même concept dans deux branches et le titre y est identique PAR
	/// CONSTRUCTION (décision ai-01 du 01/10 : pas de fusion). Le défaut est dans le
	/// générateur, pas dans les titres.
	///
	/// <para>Cas nommés par ai-01 et reproduits ici sur un SVG synthétique :</para>
	/// <list type="bullet">
	/// <item><description><b>Parent et enfant homonymes</b> — « Sophisme de l'accident »,
	/// PK 614 (<c>3.1.2</c>) et PK 615 (<c>3.1.2.1</c>) : l'ANCIEN départage cherchait le
	/// parent par troncature de caractère sur <c>DecimalPath</c>, échouait, et laissait les
	/// DEUX nœuds muets.</description></item>
	/// <item><description><b>Un titre sous 3 branches</b> — « Vrai Écossais » (PK 65, 616,
	/// 813) : un des trois restait muet.</description></item>
	/// <item><description><b>Distracteurs plus longs</b> — des nœuds dont le texte CONTIENT le
	/// titre : le filtre de longueur minimale doit continuer de les écarter (ils ne sont pas
	/// des occurrences du concept).</description></item>
	/// </list>
	///
	/// <para><b>Contrat</b> : FreeMind exporte les nœuds dans l'ordre de l'arbre et les items
	/// suivent le même ordre — pour un titre donné, la <i>k</i>-ième occurrence dans l'ordre
	/// des items correspond à la <i>k</i>-ième occurrence dans l'ordre du document SVG. Chaque
	/// item reçoit un nœud DISTINCT et SES données (vérifiées par une valeur propre à l'item,
	/// pas seulement par le compte — la moitié du défaut était invisible à un simple
	/// décompte).</para>
	///
	/// <para>Écrit ROUGE contre le départage d'origine (mesuré : 1 item attribué sur 6, les
	/// homonymes muets) avant l'implémentation — c'est la mutation falsifiante du
	/// DoD. L'application aux SVG réels passe par la re-dérivation n° 3 (FreeMind, po-2023) :
	/// ce test ne régénère RIEN.</para>
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
		public void Homonymous_Titles_Receive_OneToOne_Pairing_In_Tree_Order()
		{
			// Items dans l'ordre de l'arbre (= ordre CSV = ordre d'export FreeMind) :
			// parent puis enfant homonymes (PK 614/615), un titre sous 3 branches
			// (PK 65/616/813), un titre unique témoin.
			var typedItems = new List<TestMindMapItem>
			{
				Item("accident-parent", "Sophisme de l'accident", "3.1.2", "3,12", 3),
				Item("accident-child", "Sophisme de l'accident", "3.1.2.1", "3,121", 4),
				Item("ecossais-1", "Vrai Écossais", "1.1", "1,1", 2),
				Item("ecossais-2", "Vrai Écossais", "2.3", "2,3", 2),
				Item("ecossais-3", "Vrai Écossais", "5.1.2", "5,12", 3),
				Item("appel", "Appel à la nature", "4.1", "4,1", 2),
			};
			IList<IMindMapItem> items = typedItems.Cast<IMindMapItem>().ToList();

			// Ordre du document = ordre de l'arbre. Les deux derniers nœuds sont des
			// distracteurs : leur texte CONTIENT un titre mais est plus long — le filtre de
			// longueur minimale doit les écarter.
			var svgDoc = SyntheticSvg(
				"Sophisme de l'accident",
				"Sophisme de l'accident",
				"Vrai Écossais",
				"Vrai Écossais",
				"Vrai Écossais",
				"Appel à la nature",
				"Sophisme de l'accident et ses cousins",
				"Vrai Écossais — variante longue");

			var config = new FallacyMindMapDocumentConfig();
			var svgMap = new SVGFreemindMap { SetSVGNodeAttributes = true, WrapNodeByLink = true };
			config.UpdateSvgWithItems(svgMap, items, svgDoc, Svg, XLink);

			var attributed = svgDoc.Descendants(Svg + "g")
				.Where(g => Attr(g, "class") == "node")
				.ToList();

			// 1. Couverture : les SIX items reçoivent un nœud — l'ancien départage laissait
			//    muets parent ET enfant homonymes (défaut #1698 mesuré : accident et écossais
			//    muets, 1/6 attribué).
			attributed.Should().HaveCount(items.Count,
				"chaque item, homonyme ou non, doit recevoir un nœud : c'est le défaut #1698");

			// 2. Un-pour-un : un nœud ne porte les données que d'UN item. L'ancien code
			//    journalisait « Conflicting attribution » puis écrivait QUAND MÊME — un nœud
			//    écrit deux fois, l'autre jamais.
			attributed.Select(g => Attr(g, "id")).Should().OnlyHaveUniqueItems(
				"l'appariement est un-pour-un : deux items homonymes ne peuvent pas écrire sur le même nœud");

			// 3. k-ième occurrence d'items ↔ k-ième occurrence du document (l'ordre de
			//    l'arbre est le même des deux côtés) — vérifié TITRE PAR TITRE, avec les
			//    distracteurs exclus par l'égalité exacte du texte.
			AssertPairingOrder(svgDoc, "Sophisme de l'accident", new[] { "accident-parent", "accident-child" },
				"parent et enfant homonymes (PK 614/615) : le parent précède l'enfant dans l'arbre, donc dans le document");
			AssertPairingOrder(svgDoc, "Vrai Écossais", new[] { "ecossais-1", "ecossais-2", "ecossais-3" },
				"titre sous 3 branches (PK 65/616/813) : la k-ième occurrence d'items prend la k-ième occurrence du document");

			// 4. Chaque nœud porte SES données — valeur propre à l'item, pas seulement le
			//    compte (l'autre moitié du défaut était invisible à un décompte).
			foreach (var item in typedItems)
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

		/// <summary>
		/// Les occurrences EXACTES d'un titre, dans l'ordre du document, doivent porter les ids
		/// des items homonymes dans l'ordre des items.
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
