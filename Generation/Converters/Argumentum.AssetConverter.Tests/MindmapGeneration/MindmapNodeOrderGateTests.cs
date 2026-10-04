using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Argumentum.AssetConverter.Mindmapper;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// #1700 (reprise) — porte d'ORDRE sur les content.svg committés : les ids (PK) des nœuds
	/// cliquables, lus dans l'ordre du document, doivent suivre l'ordre de l'arbre en parcours
	/// préfixe À FRÈRES INVERSÉS (l'ordre que FreeMind/Batik peint — mesure ai-01 sur
	/// <c>Fallacies_fr.content.svg</c> @ <c>9e765e4b</c> : 892 ruptures pour l'ordre des items,
	/// 891 pour le préfixe droit, 2 pour le préfixe inversé). Instrument : hors-ordre =
	/// N − LIS (plus longue sous-suite strictement croissante des rangs préfixe-inversés),
	/// l'instrument de la revue c.5944197869.
	///
	/// <para><b>Plafond = la mesure exacte de l'arbre committé</b> (branch
	/// <c>fix/458-1698-mindmap-node-pairing</c>, SVG de la re-dérivation 2 #1695) : toute
	/// NOUVELLE rupture rend la porte rouge, une rupture réparée aussi (recalibrer avec la
	/// date, jamais effacer la ligne). Les ruptures résiduelles connues, mesurées et nommées
	/// ci-dessous, sont de vrais écarts du SVG Batik committé — l'appariement de production
	/// les absorbe (le rang les départage quand même) ; la re-dérivation n° 3 peut les faire
	/// disparaître : recalibrer alors.</para>
	///
	/// <para><b>PK responsables, mesurés le 02/10/2026</b> avec ce même instrument (racine
	/// corrigée au comparateur — <c>CompareBatikDocumentOrder</c> peint la racine première) :
	/// PK <b>825</b> « Mot à la mode » (<c>5.1.2.3.2.3</c>, l'universel : toutes les langues
	/// le peignent avant son frère <c>…2.24</c> PK 824) ; es <b>1404</b> « Insulte »
	/// (<c>7.3.3.5.1</c>) ; ar <b>46</b> « Appel à la normalité » (<c>1.1.2.2.2</c>) ; fa
	/// <b>793</b> (<c>4.3.3.2.2</c>) et <b>259</b> « Anaphore » (<c>2.1.3.2.2</c>) ; zh
	/// <b>241</b> « Sarcasme » (<c>2.1.2.2.3.2.1</c>).
	/// <b>Re-mesuré le 04/10/2026</b> sur l'arbre de la re-dérivation n° 3 (#1740, pairing
	/// #1700/#1711 appliqué aux 1408 nœuds) : hors-ordre = <b>1 partout</b> — les ruptures
	/// es 1404 · ar 46 · fa 793/259 · zh 241 sont <b>réparées</b> ; ne subsiste que le
	/// PK 825 universel. Plafonds reserrés à 1 (recalibré avec la date, lignes gardées).</para>
	///
	/// <para><b>Anti-vacuité.</b> Le compte de cliquables par langue est épinglé EXACTEMENT
	/// (identique à <see cref="MindmapClickableNodeDebtGateTests"/>, recalibré au 04/10) : un SVG vide
	/// rendrait la porte verte sans rien prouver. Un id absent du CSV fait échouer la lecture
	/// (pas de trou silencieux).</para>
	/// </summary>
	public class MindmapNodeOrderGateTests
	{
		private static readonly XNamespace Svg = "http://www.w3.org/2000/svg";

		/// <summary>langue, chemin du SVG sous Cards/Fallacies/Mindmaps, cliquables attendus, plafond hors-ordre.</summary>
		public static TheoryData<string, string, int, int> Cases = new()
		{
			{ "fr", @"fr\Fallacies_fr.content.svg", 1408, 1 },
			{ "en", @"en\Fallacies_en.content.svg", 1408, 1 },
			{ "ru", @"ru\Fallacies_ru.content.svg", 1408, 1 },
			{ "pt", @"pt\Fallacies_pt.content.svg", 1408, 1 },
			{ "es", @"es\Fallacies_es.content.svg", 1408, 1 },
			{ "ar", @"ar\Fallacies_ar.content.svg", 1408, 1 },
			{ "fa", @"fa\Fallacies_fa.content.svg", 1408, 1 },
			{ "zh", @"zh\Fallacies_zh.content.svg", 1408, 1 },
			{ "cards_fr", @"fr\Argumentum_Fallacies_MindMap_cards_fr.content.svg", 1408, 1 },
		};

		[Theory]
		[MemberData(nameof(Cases))]
		public void Clickable_Node_Ids_Follow_Batik_Prefix_Reversed_Order(string key, string svgRelativePath, int expectedClickable, int ceiling)
		{
			var ranks = PrefixReversedRanksFromCsv();
			var svgPath = Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies", "Mindmaps", svgRelativePath);
			File.Exists(svgPath).Should().BeTrue($"le SVG committé de {key} doit exister : {svgRelativePath}");

			var document = XDocument.Load(svgPath, LoadOptions.None);
			var clickableRanks = new List<int>();
			var unknownIds = new List<string>();
			foreach (var g in document.Descendants(Svg + "g"))
			{
				if ((string?)g.Attribute("class") != "node")
				{
					continue;
				}
				var id = ((string?)g.Attribute("id") ?? "").Trim();
				if (ranks.TryGetValue(id, out var rank))
				{
					clickableRanks.Add(rank);
				}
				else
				{
					unknownIds.Add(id);
				}
			}

			clickableRanks.Should().HaveCount(expectedClickable,
				$"anti-vacuité : {key} compte exactement {expectedClickable} nœuds cliquables (recalibré au 04/10, " +
				"identique à MindmapClickableNodeDebtGateTests) — un SVG vide rendrait cette porte verte sans prouver");
			unknownIds.Should().BeEmpty(
				$"chaque id cliquable de {key} doit exister dans le CSV (sinon la mesure saute des nœuds en silence)");

			var outOfOrder = clickableRanks.Count - LongestIncreasingSubsequence(clickableRanks);
			outOfOrder.Should().BeLessThanOrEqualTo(ceiling,
				$"{key} : les ids cliquables suivent l'ordre préfixe-à-frères-inversés de l'arbre (instrument LIS de " +
				$"la revue #1700). Mesuré 04/10 sur l'arbre de la re-dérivation n° 3 (#1740) : {ceiling} rupture(s) " +
				"réelle(s) du SVG Batik — ne subsiste que PK 825 « Mot à la mode » sur toutes les langues ; les " +
				"ruptures es 1404 · ar 46 · fa 793/259 · zh 241 du 02/10 sont réparées. " +
				"Toute NOUVELLE rupture rend cette porte rouge ; recalibrer avec la date, jamais effacer la ligne.");
		}

		/// <summary>
		/// PK → rang dans le parcours préfixe à frères inversés, via LE comparateur de production
		/// (<see cref="FallacyMindMapDocumentConfig.CompareBatikDocumentOrder"/>) : l'instrument de
		/// la porte et l'appariement qu'elle garde ne peuvent pas diverger.
		/// </summary>
		private static Dictionary<string, int> PrefixReversedRanksFromCsv()
		{
			var csvPath = Path.Combine(TestRepoRoot.Find(),
				"Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			List<(string Pk, string Path)> rows;
			using (var reader = new StringReader(File.ReadAllText(csvPath)))
			using (var csv = new CsvReader(reader, config))
			{
				rows = csv.GetRecords<dynamic>()
					.Select(r => (Pk: ((IDictionary<string, object>)r)["PK"]?.ToString()?.Trim() ?? "",
						Path: ((IDictionary<string, object>)r)["path"]?.ToString()?.Trim() ?? ""))
					.ToList();
			}
			rows.Should().HaveCount(1408, "la taxonomie des sophismes compte 1408 rangées.");

			var ordered = rows.OrderBy(r => r.Path, new BatikPathOrder()).ToList();
			var ranks = new Dictionary<string, int>(ordered.Count, StringComparer.Ordinal);
			for (var i = 0; i < ordered.Count; i++)
			{
				ranks.Add(ordered[i].Pk, i);
			}
			return ranks;
		}

		private sealed class BatikPathOrder : IComparer<string>
		{
			public int Compare(string? x, string? y)
			{
				return FallacyMindMapDocumentConfig.CompareBatikDocumentOrder(x ?? "", y ?? "");
			}
		}

		/// <summary>Patience sorting : longueur de la plus longue sous-suite STRICTEMENT croissante.</summary>
		private static int LongestIncreasingSubsequence(IReadOnlyList<int> sequence)
		{
			var tails = new List<int>();
			foreach (var value in sequence)
			{
				var index = tails.BinarySearch(value);
				if (index < 0)
				{
					index = ~index;
				}
				if (index == tails.Count)
				{
					tails.Add(value);
				}
				else
				{
					tails[index] = value;
				}
			}
			return tails.Count;
		}
	}
}
