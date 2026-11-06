using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Argumentum.AssetConverter.Tests.Structure
{
	/// <summary>
	/// #1460 <b>phase C</b>, organe 1 — <b>les cartes Rules viennent en tête du deck Tarot</b>,
	/// mesuré sur les <b>pixels</b> du PDF produit.
	///
	/// <para><b>Pourquoi pas le texte.</b> Les PDF de cartes n'ont <b>aucune couche texte</b>
	/// (0 caractère avec <c>pypdf</c> et <c>pdftotext</c>, 0 police ; contrôle positif 17 616
	/// caractères, mesuré par ai-01 le 06/11). Un organe « Rules d'abord » qui aurait lu le texte
	/// était donc <b>vert sur n'importe quel défaut</b>. Le seul contenu lisible est l'image.</para>
	///
	/// <para><b>Le piège de l'égalité exacte.</b> Le sha256 des pixels décodés ne matche <b>jamais</b>
	/// entre une page du PDF et son PNG source : l'image embarquée est en <b>CMYK</b> et le PNG en
	/// <b>RGB</b> (mesuré sur <c>Argumentum_TarotCards_fr.pdf</c> le 06/11 : <c>R9.tif</c>,
	/// <c>mode=CMYK</c>, 826×1417, contre <c>rules_01_face.png</c>, <c>RGB</c>, mêmes dimensions).
	/// L'organe compare donc une <b>empreinte perceptuelle</b> : luminance réduite à
	/// <see cref="Grid"/>×<see cref="Grid"/>, écart moyen absolu.</para>
	///
	/// <para><b>La séparation est mesurée, pas espérée</b> (même arbre, deux instruments indépendants —
	/// Magick.NET côté C#, pypdf+PIL côté Python) : page 1 → <c>rules_01_face.png</c> à <b>6,3–6,5</b>
	/// selon la langue (C#) / <b>5,70</b> (Python), la meilleure mauvaise à <b>82,6–83,2</b>. Sur
	/// <b>les 8 langues</b> (120 pages, 06/11), la marge minimale contre le second candidat est
	/// <b>2,92×</b> (<c>ar</c>, page 9) et l'écart gagnant maximal <b>6,52</b> — d'où les planchers
	/// <see cref="MinSeparationRatio"/> = 2 et <see cref="MaxBestDistance"/> = 20.</para>
	///
	/// <para>⚠️ <b>La marge la plus serrée est structurelle, pas accidentelle</b> : page 9 de
	/// <c>ar</c>, le poursuivant descend à <b>~8,7</b> quand il vaut 9–15 ailleurs. Un plancher de 2
	/// garde 1,46× de réserve sur ce point — c'est mince, et c'est mesuré plutôt que supposé : un
	/// changement de gabarit ou de résolution de lecture doit donc re-mesurer les 8 langues avant de
	/// déplacer ce plancher.</para>
	///
	/// <para><b>Ce que cet organe n'établit pas</b> : il ne dit rien de la <i>lisibilité</i> d'une
	/// carte (deux cartes peuvent être correctement ordonnées et illisibles), ni de la <b>justesse</b>
	/// d'une traduction, ni de l'absence de doublon dans une famille. Il ne compare pas les langues
	/// entre elles : chaque deck porte son propre attendu. Et un <b>ratio élevé ne vaut pas
	/// certitude</b> — il dit « le meilleur candidat est nettement le meilleur », pas « c'est la bonne
	/// carte » : c'est <see cref="MaxBestDistance"/> qui porte cette seconde exigence.</para>
	/// </summary>
	internal static class RulesFirstOrderIntegrity
	{
		/// <summary>Côté de la grille de luminance. <b>48</b>×48 = 6 912 octets de luminance par
		/// empreinte. La valeur a été <b>portée de 32 à 48 par la mesure</b> : à 32, la page 9 de
		/// <c>ar</c> ne rendait qu'une marge de <b>2,29×</b>, sous le plancher de 2 avec une réserve de
		/// 0,29 seulement ; à 48 la même page remonte à <b>2,92×</b>. Assez fin pour séparer deux
		/// cartes de Rules voisines, assez grossier pour absorber le rendu Ghostscript (150 dpi → page
		/// rééchantillonnée) et la conversion CMYK→RGB.</summary>
		public const int Grid = 48;

		/// <summary>Marge minimale entre le meilleur et le second candidat. <b>Mesuré</b> le 06/11 sur
		/// les 15 premières pages <b>des 8 langues</b> (120 pages) : de <b>2,92×</b> (<c>ar</c>, page 9)
		/// à 17,5×. Le plancher est posé à 2 — sous le minimum mesuré, donc il ne rougit pas sur un
		/// deck sain, et au-dessus de 1, qui est le cas « ambigu » qu'il doit précisément refuser.
		/// ⚠️ La réserve n'est que de 1,46× sur <c>ar</c>/page 9 : une re-mesure est due à tout
		/// changement de gabarit ou de résolution de lecture.</summary>
		public const double MinSeparationRatio = 2.0;

		/// <summary>Distance maximale admise pour le meilleur candidat. <b>Mesuré</b> : la plus grande
		/// valeur gagnante des 120 pages est <b>6,52</b> (<c>zh</c>, page 1 — la seule où la page est
		/// rasterisée en CMYK contre un PNG RGB sans rééchantillonnage identique ; les autres pages
		/// gagnent entre 0,8 et 2,4). 20 laisse la place au bruit de rendu sans jamais atteindre la
		/// meilleure mauvaise mesure (<b>≥ 82,6</b> sur les 8 langues) — soit un facteur 4 d'écart
		/// entre le plancher et le premier faux candidat.</summary>
		public const double MaxBestDistance = 20.0;

		/// <summary>Une carte de référence et son empreinte.</summary>
		public sealed class CardPrint
		{
			public CardPrint(string name, byte[] fingerprint)
			{
				Name = name;
				Fingerprint = fingerprint;
			}

			public string Name { get; }
			public byte[] Fingerprint { get; }
		}

		/// <summary>Le résultat d'appariement d'une page : le gagnant, son écart, et le second —
		/// parce qu'un gagnant sans son poursuivant ne dit pas si la décision était serrée.</summary>
		public sealed class PageMatch
		{
			public PageMatch(int pageNumber, string? bestName, double bestDistance,
				string? runnerUpName, double runnerUpDistance)
			{
				PageNumber = pageNumber;
				BestName = bestName;
				BestDistance = bestDistance;
				RunnerUpName = runnerUpName;
				RunnerUpDistance = runnerUpDistance;
			}

			public int PageNumber { get; }
			public string? BestName { get; }
			public double BestDistance { get; }
			public string? RunnerUpName { get; }
			public double RunnerUpDistance { get; }

			/// <summary>Écart relatif au gagnant. Infini quand l'écart du gagnant est nul.</summary>
			public double Ratio => RunnerUpDistance / Math.Max(BestDistance, 1e-3);
		}

		/// <summary>Écart moyen absolu entre deux empreintes. <c>NaN</c> si les tailles diffèrent :
		/// comparer deux empreintes de tailles différentes n'a pas de sens, et rendre 0 ou l'infini
		/// fabriquerait un verdict.</summary>
		public static double Distance(byte[]? a, byte[]? b)
		{
			if (a == null || b == null || a.Length == 0 || a.Length != b.Length) return double.NaN;

			long sum = 0;
			for (var i = 0; i < a.Length; i++) sum += Math.Abs(a[i] - b[i]);
			return (double)sum / a.Length;
		}

		/// <summary>Meilleur candidat et poursuivant pour une page. Une liste de moins de deux
		/// candidats ne permet aucune marge : elle rend <c>NaN</c> plutôt qu'une confiance inventée.</summary>
		public static PageMatch Match(int pageNumber, byte[] page, IReadOnlyList<CardPrint> cards)
		{
			if (cards == null || cards.Count < 2)
				return new PageMatch(pageNumber, null, double.NaN, null, double.NaN);

			CardPrint? best = null, second = null;
			var bestD = double.PositiveInfinity;
			var secondD = double.PositiveInfinity;

			foreach (var card in cards)
			{
				var d = Distance(page, card.Fingerprint);
				if (double.IsNaN(d)) continue;

				if (d < bestD)
				{
					second = best; secondD = bestD;
					best = card; bestD = d;
				}
				else if (d < secondD)
				{
					second = card; secondD = d;
				}
			}

			return new PageMatch(pageNumber, best?.Name, bestD, second?.Name, secondD);
		}

		/// <summary>
		/// Confronte les premières pages aux cartes attendues, dans l'ordre. Rend des échecs
		/// <b>nommés</b> (page, attendu, obtenu, mesures) — jamais un booléen : un message de garde est
		/// lu par un humain avant d'être cru par une assertion.
		/// </summary>
		/// <param name="pages">Empreintes des pages, dans l'ordre du document.</param>
		/// <param name="cards">Toutes les cartes candidates (Rules <b>et</b> les autres familles) —
		/// restreindre aux Rules rendrait l'organe incapable de voir un deck qui commence par une
		/// Fallacy, c'est-à-dire exactement le défaut qu'il garde.</param>
		/// <param name="expectedOrder">Le nom de carte attendu pour la page 1, 2, …</param>
		public static IReadOnlyList<string> Check(
			IReadOnlyList<byte[]>? pages,
			IReadOnlyList<CardPrint>? cards,
			IReadOnlyList<string>? expectedOrder,
			double minRatio = MinSeparationRatio,
			double maxDistance = MaxBestDistance)
		{
			var failures = new List<string>();
			if (expectedOrder == null || expectedOrder.Count == 0)
			{
				failures.Add("aucun attendu fourni : la garde n'a rien à confronter");
				return failures;
			}
			if (cards == null || cards.Count < 2)
			{
				// Avec moins de deux candidats il n'existe aucune marge : la garde le DIT plutôt que
				// de rendre une confiance qu'elle ne peut pas fonder.
				failures.Add($"moins de deux cartes candidates ({cards?.Count ?? 0}) : aucune marge calculable");
				return failures;
			}
			if (pages == null || pages.Count < expectedOrder.Count)
			{
				failures.Add($"seulement {pages?.Count ?? 0} page(s) lue(s) pour {expectedOrder.Count} carte(s) attendue(s)");
				return failures;
			}

			for (var i = 0; i < expectedOrder.Count; i++)
			{
				var expected = expectedOrder[i];
				var m = Match(i + 1, pages[i], cards);

				if (m.BestName == null)
				{
					failures.Add($"page {m.PageNumber} : aucun candidat comparable");
					continue;
				}
				if (!string.Equals(m.BestName, expected, StringComparison.Ordinal))
				{
					failures.Add(
						$"page {m.PageNumber} : attendu « {expected} », obtenu « {m.BestName} » " +
						$"(écart {F(m.BestDistance)} ; poursuivant « {m.RunnerUpName} » à {F(m.RunnerUpDistance)})");
					continue;
				}
				if (m.BestDistance > maxDistance)
				{
					failures.Add(
						$"page {m.PageNumber} : « {m.BestName} » gagne mais de trop loin — écart {F(m.BestDistance)} " +
						$"> {F(maxDistance)} : la page ne ressemble à aucune carte référence");
				}
				if (!(m.Ratio >= minRatio))
				{
					failures.Add(
						$"page {m.PageNumber} : décision ambiguë sur « {m.BestName} » — marge {F(m.Ratio)}x " +
						$"< {F(minRatio)}x (poursuivant « {m.RunnerUpName} » à {F(m.RunnerUpDistance)})");
				}
			}

			return failures;
		}

		/// <summary>Formatage <b>invariant</b> : un message de garde ne doit pas changer de forme selon
		/// la culture de la machine (« 6,11 » sous une locale française, « 6.11 » ailleurs). Même
		/// raison que dans <c>PdfPageGeometryIntegrity</c>, mesurée le 05/11.</summary>
		internal static string F(double value) => value.ToString("F2", CultureInfo.InvariantCulture);
	}
}
