using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter.Mindmapper
{
	/// <summary>
	/// Appariement items ↔ nœuds SVG d'une carte mentale FreeMind/Batik — génération #1700 + #1704
	/// (suites A et B), partagée entre <see cref="FallacyMindMapDocumentConfig"/> et
	/// <see cref="VirtueMindMapDocumentConfig"/>. Le jumeau Vertus portait l'ancienne génération
	/// (préfixe <c>Substring(0, 3)</c>, candidat unique sans exclusion, écrasement silencieux) : les
	/// artefacts Vertus étaient sains uniquement parce qu'aucun titre n'était homonyme, et les trois
	/// défauts auraient tiré en silence le jour où un retitrage les aurait armés (mesure 0-écriture
	/// c.5949190678). Une copie divergente du même algorithme est exactement ce qui a laissé passer
	/// #1698 côté Fallacies — la logique vit ici UNE fois.
	/// </summary>
	internal static class MindMapSvgItemPairing
	{
		public static Dictionary<IMindMapItem, List<XElement>> CollectPossibleSvgNodes(IList<IMindMapItem> items,
			XDocument svgDoc, XNamespace svgNamespace, Func<IMindMapItem, string> titleFunc)
		{
			Dictionary<IMindMapItem, List<XElement>> itemToSvgNodes = new();
			var textGroups = svgDoc.Descendants(svgNamespace + "g").Where(g => g.Elements(svgNamespace + "text").Any()).ToList();

			foreach (var item in items)
			{
				string title = titleFunc(item);
				var matchingGroups = textGroups.Where(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Contains(title)).ToList();

				if (matchingGroups.Any())
				{
					// Group the g elements by the length of their text content
					var groupedGroups = matchingGroups.GroupBy(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Length);

					// Get the minimum length among the groups
					var groups = groupedGroups as IGrouping<int, XElement>[] ?? groupedGroups.ToArray();
					var minLength = groups.Min(g => g.Key);

					// Retain only the g elements with the minimum length
					var minLengthGroups = groups.First(g => g.Key == minLength).ToList();

					itemToSvgNodes[item] = minLengthGroups;
				}
				else
				{
					// #1700 — le préfixe de diagnostic se tronque à la longueur du titre :
					// ~150 titres zh font 2 caractères (« 谬论 », « 偏见 »…) et un titre court
					// sans candidat tuait l'injection zh entière en
					// ArgumentOutOfRangeException sur Substring(0, 3).
					var titleHead = title.Length <= 3 ? title : title.Substring(0, 3);
					var closeMatches = textGroups.Where(g => string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value)).Contains(titleHead)).ToList();
					var closeMatchesMessages = closeMatches.Select(g => string.Join(" ", g.Elements(svgNamespace + "text").Select(t => t.Value))).ToList().Aggregate("", (s1, s2) => $"{s1}\n{s2}");
					Logger.LogProblem($"Could not find Svg node for item {titleFunc(item)}\nClose matches:\n{closeMatchesMessages}");
				}
			}

			return itemToSvgNodes;
		}

		public static Dictionary<IMindMapItem, XElement> DisambiguateSvgNodes(
			Dictionary<IMindMapItem, List<XElement>> itemToSvgNodes, IList<IMindMapItem> items, XNamespace svgNamespace,
			Func<IMindMapItem, string> titleFunc)
		{
			if (!itemToSvgNodes.Any() || itemToSvgNodes.First().Value.Any() == false)
			{
				Logger.LogProblem("No SVG nodes to disambiguate.");
				return new Dictionary<IMindMapItem, XElement>();
			}

			var tempNode = itemToSvgNodes.First().Value.First();
			var allNodesList = tempNode.Document.Descendants(svgNamespace + tempNode.Name.LocalName).ToList();
			var nodeIndices = allNodesList.Select((n, i) => new { Node = n, Index = i }).ToDictionary(n => n.Node, n => n.Index);

			foreach (var itemToSvgNode in itemToSvgNodes)
			{
				foreach (var svgNode in itemToSvgNode.Value)
				{
					if (!nodeIndices.ContainsKey(svgNode))
					{
						Logger.LogWarning($"SVG node for item {titleFunc(itemToSvgNode.Key)} not found in document index. It might be a new or detached node.");
					}
				}
			}

			Dictionary<IMindMapItem, XElement> disambiguatedItemToSvgNode = new();
			Dictionary<XElement, IMindMapItem> svgNodeToItem = new();

			// #1700 suite A (revue c.5945920853, injection en place) : un nœud dont le texte
			// EST ÉGAL au titre d'un item du lot n'est candidat QUE pour le groupe de CE
			// titre. Mesuré en ar : le nœud exact de PK 975 « المغالطات الاعتراضية » était
			// aussi candidat du groupe de PK 298 « الاعتراض » (titre CONTENU), traité plus
			// tôt au rang 2.x : le premier attribué gardait le nœud — fiche FAUSSE sur le
			// nœud de 975, et 975 muet. Sans effet sur une carte fraîchement dérivée
			// (chaque rangée y a son nœud à son titre exact).
			var exactOwner = new Dictionary<XElement, string>();
			foreach (var exactGroup in itemToSvgNodes.GroupBy(pair => titleFunc(pair.Key) ?? "", StringComparer.Ordinal))
			{
				foreach (var node in exactGroup.SelectMany(pair => pair.Value).Distinct())
				{
					if (string.Equals(SvgNodeText(node, svgNamespace), exactGroup.Key, StringComparison.Ordinal))
					{
						exactOwner[node] = exactGroup.Key;
					}
				}
			}

			// #1700 — appariement UN-POUR-UN, indépendant de l'unicité des titres. L'ordre du
			// document SVG suit le parcours préfixe À FRÈRES INVERSÉS (voir
			// CompareBatikDocumentOrder) : pour un titre donné, la k-ième occurrence dans cet
			// ordre correspond à la k-ième occurrence dans l'ordre du document. La première
			// version (caa5aee3, rejetée) prenait l'ordre de la liste CSV pour ordre du
			// document — faux pour les frères : ~170 fiches/langue ouvraient le mauvais
			// homonyme. L'ancien départage d'origine (proximité du parent par troncature de
			// caractère sur DecimalPath) laissait MUETS les homonymes — parent ET enfant dans
			// le cas 614/615 — et écrivait deux fois sur le même nœud sans exclure les nœuds
			// déjà attribués.

			// Pass 1 — groupes homonymes ET groupes triviaux : appariement par rang préfixe
			// inversé. Les groupes sont parcourus dans l'ordre des items (celui de
			// itemToSvgNodes), les candidats dans l'ordre du document.
			var pendingSingles = new List<KeyValuePair<IMindMapItem, List<XElement>>>();
			foreach (var titleGroup in itemToSvgNodes.GroupBy(pair => titleFunc(pair.Key) ?? "", StringComparer.Ordinal))
			{
				var groupItems = titleGroup.Select(pair => pair.Key)
					.OrderBy(item => item.Path ?? "", BatikPathComparer.Instance).ToList();
				var candidates = titleGroup.SelectMany(pair => pair.Value).Distinct()
					.Where(node => nodeIndices.ContainsKey(node))
					.OrderBy(node => nodeIndices[node]).ToList();
				// Un nœud déjà attribué à un item d'UN AUTRE groupe ne se ré-attribue pas :
				// c'est l'exclusion minimale demandée par #1698 pour les collisions de
				// sous-chaînes entre titres différents. Suite A : un nœud qui porte le titre
				// EXACT d'un item d'un autre groupe leur reste réservé.
				var available = candidates.Where(node => !svgNodeToItem.ContainsKey(node)
					&& (!exactOwner.TryGetValue(node, out var exactTitle) || exactTitle == titleGroup.Key)).ToList();

				if (available.Count == 0)
				{
					Logger.LogProblem($"No available SVG node for title \"{titleGroup.Key}\" ({groupItems.Count} item(s)) - candidates already attributed to other items or carrying the exact title of another item (reserved by it).");
					continue;
				}

				if (available.Count < groupItems.Count)
				{
					// #1700 suite B (revue c.5945920853, injection en place) : moins de nœuds
					// que d'homonymes — mesuré en ar 295/296 et 43/46, fa 791/793 et 255/259 :
					// le retitrage d'un DESCENDANT au titre de son ANCIÊTRE laisse un seul
					// nœud pour deux items sur la carte périmée, et le texte ne permet pas de
					// trancher (la carte committée portait le nœud du descendant, le rang
					// d'appariement le donne à l'ancêtre). « Un nœud muet vaut mieux qu'une
					// fiche fausse » : RIEN n'est attribué dans ce groupe — les deux items
					// restent muets jusqu'à la re-dérivation. Sans effet sur une carte fraîche.
					var wholeGroup = groupItems.Select(item => $"{item.Path}-{titleFunc(item)}").ToList();
					Logger.LogProblem($"Title \"{titleGroup.Key}\": {groupItems.Count} items share this title but only {available.Count} SVG node(s) exist - the text cannot disambiguate on a stale map, leaving every item of the group without a node: {string.Join(", ", wholeGroup)}.");
					continue;
				}

				if (groupItems.Count == 1 && available.Count > 1)
				{
					// Titre unique côté items mais plusieurs candidats de longueur minimale :
					// départage par proximité du PARENT (pass 2, après le rang — le parent
					// homonyme est alors déjà résolu).
					pendingSingles.Add(new KeyValuePair<IMindMapItem, List<XElement>>(groupItems[0], available));
					continue;
				}

				if (available.Count > groupItems.Count)
				{
					// #1700 (reprise, surnuméraires) : le SVG porte PLUS d'occurrences du
					// titre que le CSV d'items — mesuré en zh : un nœud orphelin « 循环论证 »
					// sous 5.1.3.3 (branche sans enfant CSV, reste d'un texte antérieur à la
					// re-dérivation). Le k-ième↔k-ième décale alors TOUT le groupe (chaque
					// item prenait le nœud du suivant). Départage par proximité du PARENT —
					// le même instrument que le pass 2, dont les groupes parents sont déjà
					// résolus (ordre des groupes = ordre des items, parent avant enfant) ;
					// à défaut de parent résolu, le premier candidat restant dans l'ordre du
					// document. Le surnuméraire reste sans item : journalisé.
					var remaining = new List<XElement>(available);
					foreach (var item in groupItems)
					{
						var chosen = ChooseNodeNearestToParent(item, remaining, itemToSvgNodes,
							disambiguatedItemToSvgNode, items, nodeIndices, titleFunc) ?? remaining[0];
						AssignNode(disambiguatedItemToSvgNode, svgNodeToItem, item, chosen, titleFunc);
						remaining.Remove(chosen);
					}
					var surplus = remaining
						.Select(node => nodeIndices[node]).ToList();
					Logger.LogWarning($"Title \"{titleGroup.Key}\": {available.Count} SVG nodes for {groupItems.Count} item(s) - surplus node(s) left unattributed at document index {string.Join(", ", surplus)} (orphan node of an earlier tree state).");
				}
				else
				{
					for (var k = 0; k < groupItems.Count; k++)
					{
						AssignNode(disambiguatedItemToSvgNode, svgNodeToItem, groupItems[k], available[k], titleFunc);
					}
				}
			}

			// Pass 2 — items uniques à candidats multiples : le candidat le plus proche du
			// nœud du parent, parent calculé par Path en notation pointée ("3.1.2.1" -> "3.1.2"),
			// robuste à toute profondeur et aux fratries de plus de 9 (l'ancien
			// DecimalPath.Remove(length-1) ne redonnait le bon parent qu'à partir de la
			// profondeur 3). Défaut documenté : si le parent est introuvable ou lui-même
			// irrésolu, on retombe sur le premier candidat disponible dans l'ordre du document
			// plutôt que de laisser l'item muet.
			foreach (var pending in pendingSingles)
			{
				var item = pending.Key;
				var available = pending.Value;
				var chosen = ChooseNodeNearestToParent(item, available, itemToSvgNodes, disambiguatedItemToSvgNode, items, nodeIndices, titleFunc)
					?? available[0];
				AssignNode(disambiguatedItemToSvgNode, svgNodeToItem, item, chosen, titleFunc);
			}

			return disambiguatedItemToSvgNode;
		}

		/// <summary>
		/// #1700 — ordre du document SVG produit par FreeMind/Batik, DÉRIVÉ de <paramref name="leftPath"/>/
		/// <paramref name="rightPath"/> (notation pointée « 4.3.2.1.1 »). Swing peint les enfants du
		/// DERNIER au PREMIER : le document suit le parcours préfixe À FRÈRES INVERSÉS (mesure ai-01
		/// sur Fallacies_fr.content.svg @ 9e765e4b : 2 ruptures pour cet ordre contre 891 pour le
		/// préfixe droit). Trois règles :
		/// <list type="bullet">
		/// <item><description>les segments se comparent comme ENTIERS — une fratrie de 24 trie après
		/// le frère 2 et avant le frère 3, jamais entre 2 et 3 par ordre lexicographique ;</description></item>
		/// <item><description>à la première divergence, le PLUS GRAND segment vient D'ABORD (frères
		/// inversés) ;</description></item>
		/// <item><description>un chemin préfixe de l'autre (l'ancêtre) précède toujours ses
		/// descendants — le préfixe ne s'inverse pas.</description></item>
		/// </list>
		/// Le path vide ou <c>"0"</c> (la racine du map, unique rangée au segment 0 du CSV) est
		/// peint PREMIER : sans frère, l'inversion ne s'applique pas à lui (mesure : la racine
		/// ouvre le document dans les 9 content.svg committés).
		/// </summary>
		internal static int CompareBatikDocumentOrder(string leftPath, string rightPath)
		{
			var left = ParsePathSegments(leftPath);
			var right = ParsePathSegments(rightPath);
			var leftIsRoot = IsRootPath(left);
			var rightIsRoot = IsRootPath(right);
			if (leftIsRoot || rightIsRoot)
			{
				return leftIsRoot && rightIsRoot ? 0 : (leftIsRoot ? -1 : 1);
			}
			var common = Math.Min(left.Length, right.Length);
			for (var i = 0; i < common; i++)
			{
				if (left[i] != right[i])
				{
					// Frères inversés : le plus grand segment est peint EN PREMIER.
					return right[i].CompareTo(left[i]);
				}
			}
			// Préfixe : l'ancêtre (chemin plus court) précède ses descendants.
			return left.Length.CompareTo(right.Length);
		}

		private static int[] ParsePathSegments(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return Array.Empty<int>();
			}
			return path.Split('.').Select(segment => int.Parse(segment, CultureInfo.InvariantCulture)).ToArray();
		}

		/// <summary>La racine du map : path vide, ou réduit au seul segment 0.</summary>
		private static bool IsRootPath(int[] segments)
		{
			return segments.Length == 0 || (segments.Length == 1 && segments[0] == 0);
		}

		/// <summary>Adapte <see cref="CompareBatikDocumentOrder"/> au tri LINQ sur <c>item.Path</c>.</summary>
		private sealed class BatikPathComparer : IComparer<string>
		{
			public static readonly BatikPathComparer Instance = new();

			public int Compare(string x, string y)
			{
				return CompareBatikDocumentOrder(x ?? "", y ?? "");
			}
		}

		/// <summary>
		/// Texte d'un nœud <c>g</c> de la carte : concaténation de ses <c>text</c> directs — la
		/// MÊME extraction que <see cref="CollectPossibleSvgNodes"/>, pour que la réservation des
		/// titres exacts (suite A) juge l'égalité sur la chaîne qui a servi au filtre par contenu.
		/// </summary>
		private static string SvgNodeText(XElement g, XNamespace svgNamespace)
		{
			return string.Join("", g.Elements(svgNamespace + "text").Select(t => t.Value));
		}

		private static void AssignNode(Dictionary<IMindMapItem, XElement> disambiguatedItemToSvgNode,
			Dictionary<XElement, IMindMapItem> svgNodeToItem, IMindMapItem item, XElement node,
			Func<IMindMapItem, string> titleFunc)
		{
			disambiguatedItemToSvgNode[item] = node;
			if (!svgNodeToItem.TryAdd(node, item))
			{
				// Ne peut arriver que par collision de sous-chaînes entre titres DIFFÉRENTS
				// (les groupes homonymes s'apparient un-pour-un par rang) : le premier
				// attribué garde le nœud, l'écart est journalisé.
				Logger.LogProblem($"Conflicting attribution of SVG node to items: {item.Path}-{titleFunc(item)} and {svgNodeToItem[node].Path}-{titleFunc(svgNodeToItem[node])}");
			}
		}

		private static XElement ChooseNodeNearestToParent(IMindMapItem item, List<XElement> availableNodes,
			Dictionary<IMindMapItem, List<XElement>> itemToSvgNodes,
			Dictionary<IMindMapItem, XElement> disambiguatedItemToSvgNode, IList<IMindMapItem> items,
			Dictionary<XElement, int> nodeIndices, Func<IMindMapItem, string> titleFunc)
		{
			var lastSeparator = (item.Path ?? "").LastIndexOf('.');
			if (lastSeparator < 0)
			{
				Logger.LogProblem($"Cannot determine parent for item {titleFunc(item)} - {item.Path}");
				return null;
			}
			var parentPath = item.Path.Substring(0, lastSeparator);
			var parentItem = items.FirstOrDefault(f => f.Path == parentPath);
			if (parentItem == null)
			{
				Logger.LogProblem($"Parent item not found for {titleFunc(item)} - {item.Path}");
				return null;
			}

			XElement parentSvgNode;
			if (!disambiguatedItemToSvgNode.TryGetValue(parentItem, out parentSvgNode))
			{
				if (!itemToSvgNodes.TryGetValue(parentItem, out var parentSvgNodes) || parentSvgNodes.Count != 1)
				{
					// Parent homonyme non encore résolu ou sans candidat unique : l'ancien
					// code abandonnait ici (enfant muet, cas 614/615). Le rang l'a normalement
					// déjà départagé en pass 1 ; sinon l'appelant retombe sur l'ordre du
					// document plutôt que de laisser l'item muet.
					return null;
				}
				parentSvgNode = parentSvgNodes[0];
			}

			if (!nodeIndices.TryGetValue(parentSvgNode, out var parentIndex))
			{
				Logger.LogProblem($"SVG Node index for parent item: {parentItem.Path}-{titleFunc(parentItem)} of item {item.Path}-{titleFunc(item)} not found");
				return null;
			}

			return availableNodes
				.OrderBy(node => Math.Abs(nodeIndices[node] - parentIndex))
				.First();
		}
	}
}
