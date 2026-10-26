using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// Motifs du repli XSLT **LUS dans le gabarit** (réserve ai-01 sur #1740 c.5975615062 :
	/// « son motif est recopié en dur au lieu d'être lu dans <c>mm2svg.xslt</c> » ; pool
	/// c.5976781537). La porte d'arbre (<c>MindmapXsltFallbackGateTests</c>) et la garde de
	/// passe (<c>MindmapXsltFallbackSignalTests</c>) partagent cette lecture : le jour où le
	/// gabarit change ses sorties, les gardes suivent — au lieu de rester sur des littéraux
	/// périmés qui ne mesureraient plus rien.
	///
	/// <para><b>Deux familles dérivées de <c>mm2svg.xslt</c></b> :</para>
	/// <list type="number">
	/// <item>les commentaires qu'il émet — <c>&lt;xsl:comment&gt;</c> dont le texte littéral
	/// finit tel quel dans le SVG (« Processing node level », « Processing edge level ») ;</item>
	/// <item>les attributs de coordonnées qu'il tire de <c>dc:Bounds</c> (x, y, cx, …) — une
	/// entrée de bounds manquante sort en <c>name="NaN"</c>. Ne sont retenus que les
	/// <c>&lt;xsl:attribute&gt;</c> à UN SEUL enfant <c>&lt;xsl:value-of&gt;</c> citant
	/// <c>dc:Bounds</c> et à nom nu : les attributs composites (ex. <c>transform</c>, dont la
	/// valeur est un <c>translate(…)</c> à plusieurs enfants) sont exclus — leur valeur n'est
	/// pas un nombre nu, un « transform="NaN" » n'existe pas.</item>
	/// </list>
	///
	/// <para><b>Anti-vacuité</b> : <see cref="ReadMarkersFromTemplate"/> EXIGE au moins un
	/// marqueur par famille et lève sinon — un gabarit illisible ou une extraction vide ne
	/// doit jamais rendre un feu vert (règle « un 0 n'est une absence que si l'instrument
	/// pouvait voir un 1 »).</para>
	/// </summary>
	internal static class MindmapXsltFallbackMarkers
	{
		private static readonly XNamespace Xsl = "http://www.w3.org/1999/XSL/Transform";

		/// <summary>Chemin du gabarit dans l'arbre du dépôt (résolu depuis la racine du repo, comme les autres gardes).</summary>
		internal static string TemplatePath()
		{
			return Path.Combine(TestRepoRoot.Find(), "Generation", "Converters",
				"Argumentum.AssetConverter", "Mindmapper", "xslt", "mm2svg.xslt");
		}

		internal static (IReadOnlyList<Regex> CommentMarkers, IReadOnlyList<Regex> NanAttributeMarkers) ReadMarkersFromTemplate(string xsltPath)
		{
			var doc = XDocument.Load(xsltPath);

			var commentLiterals = doc.Descendants(Xsl + "comment")
				.Select(c => c.Value.Trim())
				.Where(v => v.Length > 0)
				.Distinct(StringComparer.Ordinal)
				.OrderBy(v => v, StringComparer.Ordinal)
				.ToList();

			var nanAttributes = doc.Descendants(Xsl + "attribute")
				.Where(a => a.Elements().Count() == 1
					&& a.Elements().First().Name == Xsl + "value-of"
					&& ((((string?)a.Elements().First().Attribute("select")) ?? string.Empty).Contains("dc:Bounds")))
				.Select(a => (((string?)a.Attribute("name")) ?? string.Empty).Trim())
				.Where(n => n.Length > 0)
				.Where(n => Regex.IsMatch(n, "^[A-Za-z][A-Za-z0-9]*$", RegexOptions.CultureInvariant))
				.Distinct(StringComparer.Ordinal)
				.OrderBy(n => n, StringComparer.Ordinal)
				.ToList();

			if (commentLiterals.Count == 0)
			{
				throw new InvalidOperationException(
					$"Anti-vacuité : aucun xsl:comment lisible dans le gabarit '{xsltPath}' — " +
					"la garde ne peut pas rendre un feu vert sans motifs.");
			}
			if (nanAttributes.Count == 0)
			{
				throw new InvalidOperationException(
					$"Anti-vacuité : aucun attribut dc:Bounds à valeur nue dans le gabarit '{xsltPath}' — " +
					"le marqueur NaN ne peut pas être dérivé.");
			}

			return (
				commentLiterals.Select(v => new Regex(Regex.Escape(v), RegexOptions.Compiled)).ToList(),
				nanAttributes.Select(n => new Regex(Regex.Escape(n) + "=\"NaN\"", RegexOptions.Compiled)).ToList());
		}
	}
}