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
	/// <para><b>Marqueurs LUS DANS LE GABARIT</b> (réserve ai-01 c.5975615062, exécutée au
	/// pool c.5976781537) : plus de littéraux recopiés ici — la porte relit
	/// <c>mm2svg.xslt</c> à chaque exécution via <c>MindmapXsltFallbackMarkers</c> et dérive
	/// ses motifs de ce que le gabarit émet réellement : ses commentaires
	/// <c>&lt;xsl:comment&gt;</c> (« Processing node level », « Processing edge level ») et
	/// ses attributs de coordonnées tirés de <c>dc:Bounds</c> (x, y, cx, …) dont une entrée
	/// manquante sort en <c>name="NaN"</c>. Mesuré le 04/10/2026 : 2 commentaires +
	/// 12 attributs (compte épinglé dans le fait — recalibrer avec la date si le gabarit
	/// change, jamais effacer la ligne). Sur le triplet fautif, <c>x="NaN"</c> et
	/// <c>y="NaN"</c> ×223 chacun (un par nœud) ; 0 partout sur les fichiers sains sondés.
	/// Le troisième symptôme mesuré, <c>x=""</c>, n'est PAS gardé : un attribut vide est
	/// ambigu (un gabarit futur peut le légitimer).</para>
	///
	/// <para><b>Portée</b> : tout <c>.svg</c> et <c>.html</c> sous
	/// <c>Cards/Fallacies/Mindmaps/</c>, wrappers compris — ils embarquent le SVG inline,
	/// la marque voyagerait avec lui. Rouge nominative sur la tête fautive <c>559fd6fa</c>
	/// (preuve capturée à l'implémentation), verte après restauration des octets de
	/// master.</para>
	/// </summary>
	public class MindmapXsltFallbackGateTests
	{
		// Motifs LUS dans le gabarit (réserve ai-01 c.5975615062, pool c.5976781537) :
		// plus de littéraux recopiés — la porte relit mm2svg.xslt à chaque exécution.
		private static Regex[] ReadXsltMarkers()
		{
			var (comments, nanAttributes) = MindmapXsltFallbackMarkers.ReadMarkersFromTemplate(
				MindmapXsltFallbackMarkers.TemplatePath());
			return comments.Concat(nanAttributes).ToArray();
		}

		[Fact]
		public void Committed_Mindmaps_Carry_No_XsltFallback_Marker()
		{
			var markers = ReadXsltMarkers();
			markers.Should().HaveCount(14, "anti-vacuité et recalibrage daté : 2 commentaires + 12 attributs dc:Bounds lus dans mm2svg.xslt le 04/10/2026 — si le gabarit change, re-mesurer et mettre la date, jamais effacer la ligne");

			var root = Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies", "Mindmaps");
			Directory.Exists(root).Should().BeTrue($"le répertoire des mindmaps committées doit exister : {root}");

			var offenders = new List<string>();
			foreach (var file in Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories)
				.Where(p => p.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)
					|| p.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
				.OrderBy(p => p, StringComparer.Ordinal))
			{
				var text = File.ReadAllText(file);
				var hits = markers.Where(m => m.IsMatch(text)).Select(m => m.ToString()).ToList();
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
