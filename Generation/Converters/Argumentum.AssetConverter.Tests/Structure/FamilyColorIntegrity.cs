using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Argumentum.AssetConverter.Tests.Structure
{
	/// <summary>
	/// #1460 <b>phase C</b>, organe 2 — <b>chaque carte porte la couleur de sa famille</b>,
	/// mesurée sur les <b>pixels</b> des PNG de récolte.
	///
	/// <para><b>Trois ancres, aucune re-dérivation.</b> La famille attendue d'une carte vient du
	/// CSV (colonne <c>Famille_camelCase</c> — la valeur <i>exacte</i> que la configuration injecte
	/// comme classe racine via <c>cardClass</c>) ; sa couleur vient du <b>gabarit</b>
	/// (variable <c>--color-background</c> du bloc <c>card.&lt;famille&gt;</c>) ; la couleur
	/// <b>réalisée</b> vient de l'histogramme du PNG. L'organe confronte les trois — il ne
	/// re-déduit la couleur de nulle part.</para>
	///
	/// <para><b>Mesuré, pas espéré</b> (07/11, arbre Release de la régén d'octobre, 8 langues ×
	/// 175 cartes = 1 400 cartes, échantillonnage 1 pixel sur 16) : la couleur dominante non
	/// blanche de <b>chaque</b> carte est <b>exactement</b> la couleur de sa famille (distance
	/// <b>0,0</b> partout — PNG sans perte, zone de teinte pleine). Les deux familles les plus
	/// proches du gabarit sont à <b>92,9</b> l'une de l'autre (<c>erreurMathématique</c> ↔
	/// <c>abusDeLangage</c>) ; la part minimale de la couleur dominante dans l'échantillon est
	/// <b>14,8 %</b>. D'où les trois bornes : <see cref="MaxColorDistance"/> = 30 (3,1× sous la
	/// première mauvaise famille), <see cref="MinFamilyColorSeparation"/> = 60 (l'organe refuse
	/// de verdir sur un gabarit où deux familles seraient indiscernables à son seuil) et
	/// <see cref="MinDominantShare"/> = 0,05 (2,9× sous le minimum mesuré).</para>
	///
	/// <para><b>Le défaut que l'organe garde</b> est documenté dans le CLAUDE.md : une classe de
	/// famille absente du gabarit produit une <b>carte à fond blanc</b> — et le renommage additif
	/// des familles impose que le nouveau nom cohabite avec l'ancien dans le même bloc CSS. Une
	/// classe CSV introuvable au gabarit est donc un échec <b>nommé</b>, pas un silhouette à
	/// tolérance.</para>
	///
	/// <para>⚠️ <b>La couleur est lue sur la valeur PARSÉE de la clé <c>css</c></b>, jamais sur les
	/// octets bruts du fichier JSON : dans le fichier, chaque guillemet est échappé et un regex
	/// sur le brut s'arrête au backslash — l'erreur mesurée en #1485 qui fit publier une
	/// déclaration « vide » qui en portait trois <c>url()</c>.</para>
	///
	/// <para><b>Ce que cet organe n'établit pas</b> : il ne dit rien de la <i>justesse
	/// esthétique</i> de la couleur (que la teinte soit la bonne <b>pour l'œil</b> reste un
	/// verdict humain — cf. charpente §2), ni de la lisibilité du texte, ni des couleurs
	/// secondaires (<c>--color-text-1…3</c>). Il mesure l'arbre qu'on lui donne contre le CSV et
	/// le gabarit qu'on lui donne : un arbre régénéré <b>avant</b> une édition du corpus validera
	/// l'ancien état contre le nouvel attendu (la fraîcheur relève de la garde d'arbre, organe 0
	/// de la charpente).</para>
	/// </summary>
	internal static class FamilyColorIntegrity
	{
		/// <summary>Distance RGB maximale entre la couleur dominante mesurée et la couleur de
		/// famille attendue. <b>Mesuré</b> le 07/11 sur 1 400 cartes : toutes à <b>0,0</b>
		/// exactement ; la première couleur d'une <i>autre</i> famille est à ≥ 92,9. 30 laisse la
		/// place à un futur rééchantillonnage de densité sans jamais toucher une famille voisine
		/// (facteur 3,1 entre le seuil et la première mauvaise réponse).</summary>
		public const double MaxColorDistance = 30.0;

		/// <summary>Écart minimal exigé entre deux couleurs de familles <i>distinctes</i> du
		/// gabarit. Garde <b>structurelle</b> de l'instrument : si le gabarit définit deux familles
		/// plus proches que 2× le seuil, le seuil ne peut plus distinguer l'une de l'autre —
		/// l'organe le déclare au lieu de verdir sur un gabarit où il serait aveugle. Mesuré : les
		/// deux familles les plus proches sont à <b>92,9</b> (réserve 1,55×).</summary>
		public const double MinFamilyColorSeparation = 60.0;

		/// <summary>Part minimale des points échantillonnés qui doit porter la couleur dominante.
		/// Une carte dont la classe de famille manque rend un fond blanc : la « dominante »
		/// résiduelle (traits de texte) peut encore tomber proche d'une couleur de famille par
		/// hasard, mais elle ne peut pas peser 5 %. <b>Mesuré</b> : minimum <b>14,8 %</b> sur les
		/// 1 400 cartes (réserve 2,9×).</summary>
		public const double MinDominantShare = 0.05;

		/// <summary>Une couleur RGB du gabarit ou du pixel.</summary>
		public readonly record struct Rgb(byte R, byte G, byte B)
		{
			/// <summary>Forme hexadécimale invariant, <b>en minuscules</b> comme écrit le gabarit —
			/// un message de garde doit être grepable contre la source qui l'a provoqué.</summary>
			public string Hex => $"#{R:x2}{G:x2}{B:x2}";
		}

		/// <summary>Une carte et sa famille attendue (la valeur de la colonne
		/// <c>Famille_camelCase</c> — celle que la configuration injecte comme classe racine).</summary>
		public sealed class CardFamily
		{
			public CardFamily(string name, string familyClass)
			{
				Name = name;
				FamilyClass = familyClass;
			}

			public string Name { get; }
			public string FamilyClass { get; }
		}

		/// <summary>La couleur dominante non blanche mesurée sur une carte, et le poids de cette
		/// dominante dans l'échantillon — une dominante sans son poids ne dirait pas si la zone
		/// colorée a fondu.</summary>
		public sealed class MeasuredDominant
		{
			public MeasuredDominant(string name, Rgb color, double share)
			{
				Name = name;
				Color = color;
				Share = share;
			}

			public string Name { get; }
			public Rgb Color { get; }

			/// <summary>Fraction des points échantillonnés portant la couleur dominante (0–1).</summary>
			public double Share { get; }
		}

		private static readonly Regex FamilyBlock = new(
			@"card\.([\p{L}]+)\s*\{([^{}]*)\}",
			RegexOptions.CultureInvariant | RegexOptions.Compiled);

		private static readonly Regex BackgroundVar = new(
			@"--color-background\s*:\s*#([0-9a-fA-F]{6})(?:[0-9a-fA-F]{2})?",
			RegexOptions.CultureInvariant | RegexOptions.Compiled);

		/// <summary>Extrait les couleurs de famille d'une clé <c>css</c> <b>déjà parsée</b> du
		/// gabarit. <c>[\p{L}]+</c> couvre les noms accentués (<c>erreurMathématique</c>) ; le
		/// hex à 8 chiffres (<c>#ffc307ff</c>) est accepté, son alpha ignoré. Un bloc sans
		/// <c>--color-background</c> est ignoré — c'est <see cref="Check"/> qui nomme les classes
		/// manquantes, pas le parseur.</summary>
		public static IReadOnlyDictionary<string, Rgb> ParseFamilyColors(string? css)
		{
			var colors = new Dictionary<string, Rgb>(StringComparer.Ordinal);
			if (string.IsNullOrEmpty(css)) return colors;

			foreach (Match block in FamilyBlock.Matches(css))
			{
				var color = BackgroundVar.Match(block.Groups[2].Value);
				if (!color.Success) continue;
				var h = color.Groups[1].Value;
				colors[block.Groups[1].Value] = new Rgb(
					Convert.ToByte(h.Substring(0, 2), 16),
					Convert.ToByte(h.Substring(2, 2), 16),
					Convert.ToByte(h.Substring(4, 2), 16));
			}

			return colors;
		}

		/// <summary>Distance euclidienne RGB entre deux couleurs.</summary>
		public static double Distance(Rgb a, Rgb b)
		{
			var dr = (double)a.R - b.R;
			var dg = (double)a.G - b.G;
			var db = (double)a.B - b.B;
			return Math.Sqrt(dr * dr + dg * dg + db * db);
		}

		/// <summary>
		/// Confronte chaque carte à sa couleur de famille réalisée. Rend des échecs <b>nommés</b>
		/// (carte, attendu, mesuré, valeurs) — jamais un booléen : un message de garde est lu par
		/// un humain avant d'être cru par une assertion.
		/// </summary>
		/// <param name="cards">Cartes et familles attendues (jointe CSV).</param>
		/// <param name="familyColors">Couleurs par classe, issues du gabarit parsé.</param>
		/// <param name="measured">Couleurs dominantes mesurées, une par carte.</param>
		public static IReadOnlyList<string> Check(
			IReadOnlyList<CardFamily>? cards,
			IReadOnlyDictionary<string, Rgb>? familyColors,
			IReadOnlyList<MeasuredDominant>? measured,
			double maxDistance = MaxColorDistance,
			double minSeparation = MinFamilyColorSeparation,
			double minShare = MinDominantShare)
		{
			var failures = new List<string>();
			if (cards == null || cards.Count == 0)
			{
				failures.Add("aucune carte fournie : la garde n'a rien à confronter");
				return failures;
			}
			if (familyColors == null || familyColors.Count == 0)
			{
				failures.Add("aucune couleur de famille lue dans le gabarit — la clé css est-elle vide, ou sans bloc card.<famille> ?");
				return failures;
			}

			// Garde structurelle AVANT toute carte : deux familles trop proches l'une de l'autre
			// rendent le seuil aveugle — l'instrument refuse de verdir sur un gabarit où il ne
			// pourrait pas distinguer, même si chaque carte tombe exactement sur sa teinte.
			var names = familyColors.Keys.OrderBy(n => n, StringComparer.Ordinal).ToList();
			for (var i = 0; i < names.Count; i++)
			{
				for (var j = i + 1; j < names.Count; j++)
				{
					var d = Distance(familyColors[names[i]], familyColors[names[j]]);
					if (d < minSeparation)
					{
						failures.Add(
							$"gabarit : « {names[i]} » ({familyColors[names[i]].Hex}) et « {names[j]} » " +
							$"({familyColors[names[j]].Hex}) ne sont qu'à {F(d)} l'une de l'autre (< {F(minSeparation)}) — " +
							$"au seuil de {F(maxDistance)} l'organe ne pourrait plus les distinguer : re-mesurer avant de verdir");
					}
				}
			}

			if (measured == null || measured.Count == 0)
			{
				failures.Add($"aucune mesure fournie : {cards.Count} carte(s) attendues, 0 mesurée(s)");
				return failures;
			}

			var index = new Dictionary<string, MeasuredDominant>(StringComparer.Ordinal);
			foreach (var m in measured)
			{
				if (!index.TryAdd(m.Name, m))
				{
					failures.Add($"« {m.Name} » : deux mesures du même fichier — la jointure est ambiguë");
				}
			}

			foreach (var card in cards)
			{
				if (!index.TryGetValue(card.Name, out var m))
				{
					failures.Add($"« {card.Name} » : aucune mesure de couleur dominante");
					continue;
				}
				if (!familyColors.TryGetValue(card.FamilyClass, out var expected))
				{
					failures.Add(
						$"« {card.Name} » : classe « {card.FamilyClass} » absente du gabarit — " +
						"symptôme documenté : carte sans couleur de famille (fond blanc)");
					continue;
				}

				var d = Distance(m.Color, expected);
				if (d > maxDistance)
				{
					failures.Add(
						$"« {card.Name} » : couleur dominante {m.Color.Hex} mesurée à {F(d)} de " +
						$"{expected.Hex} attendu (famille « {card.FamilyClass} ») > {F(maxDistance)}");
				}
				if (m.Share < minShare)
				{
					failures.Add(
						$"« {card.Name} » : couleur dominante {m.Color.Hex} présente sur seulement " +
						$"{P(m.Share)} des points échantillonnés (< {P(minShare)}) — la zone de famille a fondu");
				}
			}

			return failures;
		}

		/// <summary>Formatage <b>invariant</b> : un message de garde ne doit pas changer de forme
		/// selon la culture de la machine (« 6,11 » sous une locale française, « 6.11 » ailleurs).
		/// Même raison que <c>RulesFirstOrderIntegrity</c>, mesurée le 05/11.</summary>
		internal static string F(double value) => value.ToString("F2", CultureInfo.InvariantCulture);

		/// <summary>Idem, en pourcentage.</summary>
		internal static string P(double fraction) => fraction.ToString("P1", CultureInfo.InvariantCulture);
	}
}
