using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// #1740 reprise (verdict ai-01 c.5975615062, 04/10) — porte « marque XSLT » : quand
	/// l'export Batik n'est pas détecté, le pipeline tombe sur le repli XSLT (chemin mort
	/// #184) qui produit un SVG aux coordonnées vides — page blanche dans le navigateur,
	/// MAIS des attributs id présents : les portes qui comptent les ids passent donc vertes
	/// dessus. La re-dérivation n° 3 a committé un tel triplet (Vertus zh : content.svg,
	/// links.svg, wrapper) et seule la lecture visuelle l'a attrapé ; cette porte ferme la
	/// famille au niveau de l'arbre.
	///
	/// <para><b>Marqueurs</b>, mesurés ×223 sur le triplet fautif (un par nœud) et 0 sur
	/// tous les fichiers sains sondés (Fallacies zh, Vertus fr, octets de master) :
	/// <c>x="NaN"</c> — coordonnée non calculée, n'existe jamais dans une sortie Batik —
	/// et <c>Processing node level</c> — commentaire littéral de <c>mm2svg.xslt:127</c>.
	/// Le troisième symptôme mesuré, <c>x=""</c>, n'est PAS gardé : un attribut vide est
	/// ambigu (un gabarit futur peut le légitimer) ; les deux premiers ne peuvent pas être
	/// faux positifs et suffisent.</para>
	///
	/// <para><b>Portée</b> : tout <c>.svg</c> et <c>.html</c> sous
	/// <c>Cards/Fallacies/Mindmaps/</c>, wrappers compris — ils embarquent le SVG inline,
	/// la marque voyagerait avec lui. Rouge nominative sur la tête fautive <c>559fd6fa</c>
	/// (preuve capturée à l'implémentation), verte après restauration des octets de
	/// master.</para>
	/// </summary>
	public class MindmapXsltFallbackGateTests
	{
		private static readonly Regex[] XsltMarkers =
		{
			new Regex("x=\"NaN\"", RegexOptions.Compiled),
			new Regex("Processing node level", RegexOptions.Compiled),
		};

		[Fact]
		public void Committed_Mindmaps_Carry_No_XsltFallback_Marker()
		{
			var root = Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies", "Mindmaps");
			Directory.Exists(root).Should().BeTrue($"le répertoire des mindmaps committées doit exister : {root}");

			var offenders = new List<string>();
			foreach (var file in Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories)
				.Where(p => p.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)
					|| p.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
				.OrderBy(p => p, StringComparer.Ordinal))
			{
				var text = File.ReadAllText(file);
				var hits = XsltMarkers.Where(m => m.IsMatch(text)).Select(m => m.ToString()).ToList();
				if (hits.Count > 0)
				{
					offenders.Add($"{file} :: {string.Join(", ", hits)}");
				}
			}

			offenders.Should().BeEmpty(
				"le repli XSLT (#184) produit des coordonnées vides (page blanche au rendu) tout en portant " +
				"les ids — les portes de comptage passent vertes dessus. Chaque fichier listé doit être " +
				"remplacé par les octets d'un export Batik sain, jamais recalibré");
		}
	}
}
