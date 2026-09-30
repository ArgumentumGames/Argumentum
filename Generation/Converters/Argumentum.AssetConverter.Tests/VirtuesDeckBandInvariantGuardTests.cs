using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde ⑩ du pool v21 (décision ai-01 27/09 sur #1596) : chaque bandeau IMPRIMÉ
	/// de chaque carte du deck des Vertus, niveaux 1 à 3 <b>y compris le niveau
	/// propre</b>, 8 langues, est égal au titre (<c>title_&lt;lang&gt;</c>) de la
	/// rangée de ce préfixe. Règle « imprimé » tirée du gabarit
	/// <c>Argumentum_Virtues_Face_fr.json</c> : <c>family</c> s'imprime si
	/// <c>subfamily</c> rempli ; <c>subfamily</c>/<c>subsubfamily</c> s'impriment si
	/// <c>subsubfamily</c> rempli (même cascade que les Fallacies, #1588 — les
	/// conditions <c>{{#if}}</c> suivent <c>template.Replace</c> par langue).
	/// Aucune exception. Les Vertus n'ont jamais été imprimées : la règle owner du
	/// 22/09 (revenir au dernier imprimé) ne s'applique pas — le titre courant fait foi.
	/// <para>Extension ⑲w du pool v22 (#458 c.5888518774 § « Arbitrage ⑲ », base
	/// <c>f15df451</c>) : l'invariant est étendu aux 223 rangées ET aux cellules non
	/// imprimées du deck, 8 langues, <b>fr compris</b>, règle ⑱ telle quelle sans
	/// exception — 136 cellules écrites (117 hors deck + 19 deck non imprimées, 58
	/// rangées), « parce qu'un changement de gabarit les rendrait visibles ».
	/// Le titre fait foi : 4.3.3 ru porte « Доказательное рассуждение » (le titre),
	/// pas l'ancien bandeau « Убедительное ».</para>
	/// </summary>
	public class VirtuesDeckBandInvariantGuardTests
	{
		private static readonly string[] Languages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

		private static string[] BandColumns(string lang)
			=> new[] { $"family_{lang}", $"subfamily_{lang}", $"subsubfamily_{lang}" };

		private static string VirtuesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static Dictionary<string, Dictionary<string, string>> LoadRowsByPath()
		{
			var content = File.ReadAllText(VirtuesCsv);
			using var reader = new StringReader(content);
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			using var csv = new CsvReader(reader, config);
			var rows = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
			foreach (var record in csv.GetRecords<dynamic>())
			{
				var dict = (IDictionary<string, object>)record;
				var row = dict.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty, StringComparer.Ordinal);
				var path = row.GetValueOrDefault("path")?.Trim();
				if (!string.IsNullOrEmpty(path))
					rows[path] = row;
			}
			rows.Should().NotBeEmpty("le CSV Virtues doit charger.");
			return rows;
		}

		private sealed record BandMismatch(string CardPath, string Lang, string Column, string Band, string Title);

		private static List<BandMismatch> PrintedBandMismatches(Dictionary<string, Dictionary<string, string>> rows)
		{
			var mismatches = new List<BandMismatch>();
			foreach (var (path, row) in rows)
			{
				if (string.IsNullOrWhiteSpace(row.GetValueOrDefault("card")))
					continue; // hors deck : jamais imprimé
				var segs = path.Split('.');
				for (var k = 1; k <= 3 && k <= segs.Length; k++)
				{
					var ancestorPath = string.Join(".", segs.Take(k));
					if (!rows.TryGetValue(ancestorPath, out var ancestor))
						continue;
					foreach (var lang in Languages)
					{
						var bandCols = BandColumns(lang);
						var band = row.GetValueOrDefault(bandCols[k - 1])?.Trim() ?? "";
						var title = ancestor.GetValueOrDefault("title_" + lang)?.Trim() ?? "";
						if (band.Length == 0 || title.Length == 0 || band == title)
							continue;
						// règle « imprimé » du gabarit Virtues, cellules de la même langue
						var sub = row.GetValueOrDefault(bandCols[1])?.Trim() ?? "";
						var subsub = row.GetValueOrDefault(bandCols[2])?.Trim() ?? "";
						var printed = k == 1 ? sub.Length > 0 : subsub.Length > 0;
						if (printed)
							mismatches.Add(new BandMismatch(path, lang, bandCols[k - 1], band, title));
					}
				}
			}
			return mismatches;
		}

		[Fact]
		public void Every_Printed_Deck_Band_Matches_Its_Level_Title()
		{
			var rows = LoadRowsByPath();
			var mismatches = PrintedBandMismatches(rows);

			mismatches.Should().BeEmpty(
				"l'invariant ⑩ : chaque bandeau imprimé du deck des Vertus (niveaux 1-3, niveau propre " +
				"compris, 8 langues) est égal au titre de la rangée de ce préfixe — les 201 cellules du " +
				$"grain ont suivi. Écarts restants : " +
				$"{string.Join(" ; ", mismatches.Select(m => $"{m.CardPath} [{m.Lang}] {m.Column} = «{m.Band}» vs «{m.Title}»"))}");
		}

		/// <summary>
		/// Comparateur ⑲w : les 223 rangées (deck ET hors deck), niveaux 1 à
		/// min(profondeur, 3), niveau propre compris, 8 langues — strict, sans règle
		/// « imprimé » ni tolérance de titre vide. Partagé par le test d'invariant
		/// étendu et par tout futur grain qui voudrait mesurer avant d'écrire.
		/// </summary>
		private static List<BandMismatch> AllRowsBandMismatches(Dictionary<string, Dictionary<string, string>> rows)
		{
			var mismatches = new List<BandMismatch>();
			foreach (var (path, row) in rows)
			{
				var segs = path.Split('.');
				for (var k = 1; k <= 3 && k <= segs.Length; k++)
				{
					var ancestorPath = string.Join(".", segs.Take(k));
					if (!rows.TryGetValue(ancestorPath, out var ancestor))
						continue;
					foreach (var lang in Languages)
					{
						var bandCols = BandColumns(lang);
						var band = row.GetValueOrDefault(bandCols[k - 1])?.Trim() ?? "";
						var title = ancestor.GetValueOrDefault("title_" + lang)?.Trim() ?? "";
						if (band != title)
							mismatches.Add(new BandMismatch(path, lang, bandCols[k - 1], band, title));
					}
				}
			}
			return mismatches;
		}

		[Fact]
		public void Every_Row_Band_OffDeck_Included_Matches_Its_Rank_Title()
		{
			var rows = LoadRowsByPath();
			rows.Should().HaveCount(223, "223 chemins uniques attendus (anti-vacuité du balayage).");
			var mismatches = AllRowsBandMismatches(rows);

			mismatches.Should().BeEmpty(
				"l'invariant ⑲w (#458 c.5888518774) : chaque colonne bandeau des Vertus, rangée du deck " +
				"(cellules non imprimées comprises) OU hors deck, niveaux 1 à min(profondeur, 3), niveau propre " +
				"compris, 8 langues y compris le fr, porte le titre actuel du rang de ce préfixe. " +
				$"Écarts restants : {string.Join(" ; ", mismatches.Take(12).Select(m => $"{m.CardPath} [{m.Lang}] {m.Column} = «{m.Band}» vs «{m.Title}»"))}");
		}

		[Fact]
		public void BiggestNode_OuvertureAuDialogue_Followed()
		{
			var rows = LoadRowsByPath();
			// pk 186 « Ouverture au dialogue » : le nœud le plus touché du grain (36 cellules,
			// 4 langues — fr/ar/fa/zh portaient « Disponibilité au dialogue » et équivalents) ;
			// ⑲w a ensuite aligné ses 4 rangées hors deck (7.1.3.1.1, 7.1.3.3.1, 7.1.3.4.1,
			// 7.1.3.4.2) : la forme retirée a disparu du corpus entier.
			rows["7.1.3"].GetValueOrDefault("subsubfamily_fr").Should().Be("Ouverture au dialogue",
				"le bandeau propre de la tête 7.1.3 (self cell, leçon #1588) a suivi son titre.");
			rows.Values.Count(r => (r.GetValueOrDefault("subsubfamily_fr") ?? "").Trim() == "Disponibilité au dialogue")
				.Should().Be(0, "depuis ⑲w la forme retirée a disparu des 223 rangées, deck et hors deck.");
			var deck = rows.Values.Where(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("card"))).ToList();
			deck.Count(r => (r.GetValueOrDefault("subsubfamily_fr") ?? "").Trim() == "Ouverture au dialogue")
				.Should().Be(9, "les 9 cartes deck sous 7.1.3 portent le titre actuel.");
		}

		[Fact]
		public void Deck_Still_Has_131_Cards()
		{
			var rows = LoadRowsByPath();
			rows.Values.Count(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("card"))).Should().Be(131,
				"le grain ⑩ ne touche que des cellules de bandeau — le deck ne change pas de taille.");
		}
	}
	}
