using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde ④c du pool v20 (#458 c.5849450061) : chaque bandeau IMPRIMÉ de chaque
	/// carte du deck, niveaux 1 à 3 <b>y compris le niveau propre</b>, 8 langues,
	/// est égal au titre (<c>text_&lt;lang&gt;</c>) de la rangée de ce préfixe.
	/// Règle « imprimé » tirée du gabarit <c>Argumentum_Fallacies_Face_fr.json</c>
	/// (revue #1588) : <c>Famille</c> s'imprime si <c>Sous-Famille</c> rempli ;
	/// <c>Sous-Famille</c>/<c>Soussousfamille</c> s'impriment si
	/// <c>Soussousfamille</c> rempli — <c>template.Replace</c> portant les noms de
	/// champ par langue, conditions comprises.
	/// Aucune exception : les 5 cellules d'accents de #1589 (869, 1314, 1330 ancêtres ;
	/// 855, 1313 self), tolérées tant que #1589 n'était pas mergé, ont été retirées
	/// après son merge (chorégraphie du dispatch) — 0 exception.
	/// <para>Extension ⑱w du pool v22 (#458 c.5882003715 § « Arbitrage ⑱ », base
	/// <c>89f78bcd</c>) : l'invariant est étendu aux rangées HORS deck — les colonnes
	/// bandeaux alimentent la localisation des mindmaps, imprimées ou non. 422 cellules
	/// écrites (94 vides en, 320 alias, 8 casse pt ; 29 clusters, 269 rangées), les deux
	/// exceptions arbitraires alignées par la règle générale (ru 2.3.2
	/// « Игра власти », en 4.1.3 « Stork effect »). Le comparateur étendu est strict :
	/// aucune règle « imprimé », aucun titre vide toléré.</para>
	/// </summary>
	public class FallaciesDeckBandInvariantGuardTests
	{
		private static readonly string[] Languages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

		private static string[] BandColumns(string lang) => lang switch
		{
			"fr" => new[] { "Famille", "Sous-Famille", "Soussousfamille" },
			"en" => new[] { "Family", "Subfamily", "Subsubfamily" },
			_ => new[] { $"Family_{lang}", $"Subfamily_{lang}", $"Subsubfamily_{lang}" },
		};

		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static Dictionary<string, Dictionary<string, string>> LoadRowsByPath()
		{
			var content = File.ReadAllText(FallaciesCsv);
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
			rows.Should().NotBeEmpty("le CSV Fallacies doit charger.");
			return rows;
		}

		private sealed record BandMismatch(string CardPath, string Lang, string Column, string Band, string Title);

		private static List<BandMismatch> PrintedBandMismatches(Dictionary<string, Dictionary<string, string>> rows)
		{
			var mismatches = new List<BandMismatch>();
			foreach (var (path, row) in rows)
			{
				if (string.IsNullOrWhiteSpace(row.GetValueOrDefault("carte")))
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
						var title = ancestor.GetValueOrDefault("text_" + lang)?.Trim() ?? "";
						if (band.Length == 0 || title.Length == 0 || band == title)
							continue;
						// règle « imprimé » du gabarit, cellules de la même langue
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
				"l'invariant ④c : chaque bandeau imprimé du deck (niveaux 1-3, niveau propre compris, " +
				"8 langues) est égal au titre de la rangée de ce préfixe. Écarts restants : " +
				$"{string.Join(" ; ", mismatches.Select(m => $"{m.CardPath} [{m.Lang}] {m.Column} = «{m.Band}» vs «{m.Title}»"))}");
		}

		/// <summary>
		/// Comparateur ⑱w : toutes les rangées (deck ET hors deck), niveaux 1 à
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
						var title = ancestor.GetValueOrDefault("text_" + lang)?.Trim() ?? "";
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
			rows.Should().HaveCount(1408, "1408 chemins uniques attendus (anti-vacuité du balayage).");
			var mismatches = AllRowsBandMismatches(rows);

			mismatches.Should().BeEmpty(
				"l'invariant ⑱w (#458 c.5882003715) : chaque colonne bandeau, rangée du deck OU hors deck, " +
				"niveaux 1 à min(profondeur, 3), niveau propre compris, 8 langues, porte le titre actuel du " +
				"rang de ce préfixe — les colonnes bandeaux alimentent les mindmaps et l'OWL, imprimées ou non. " +
				$"Écarts restants : {string.Join(" ; ", mismatches.Take(12).Select(m => $"{m.CardPath} [{m.Lang}] {m.Column} = «{m.Band}» vs «{m.Title}»"))}");
		}

		[Fact]
		public void AdHominem_Ru_Title_Reverted_And_Unique()
		{
			var rows = LoadRowsByPath();
			rows["7.3"].GetValueOrDefault("text_ru").Should().Be("К человеку",
				"grain ④ v20 : #397 avait remis ce titre en latin ; il revient à sa valeur d'avant " +
				"(lue à 5e2477b5^) — ses 8 bandeaux russes disaient déjà « К человеку ».");
			rows.Values.Count(r => (r.GetValueOrDefault("text_ru") ?? "").Trim() == "К человеку").Should().Be(1,
				"le revert ne crée aucune collision de titre russe.");
		}

		[Fact]
		public void Deck_Still_Has_175_Cards()
		{
			var rows = LoadRowsByPath();
			rows.Values.Count(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("carte"))).Should().Be(175,
				"le grain ④ ne touche que des cellules de bandeau et un titre — le deck ne change pas de taille (#1288).");
		}
	}
}
