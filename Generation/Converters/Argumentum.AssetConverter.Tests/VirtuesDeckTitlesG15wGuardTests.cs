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
	/// Épingles du grain ⑮w (arbitrage #458 c.5888518774) : les 7 titres arbitrés
	/// des Vertus sont en place et les formes retirées ont disparu du deck ;
	/// la cascade de pk 208 (rang de bandeau imprimé : la rangée 208 et les
	/// cartes 209/210/211 portent son titre dans <c>subsubfamily_&lt;lang&gt;</c>)
	/// est épinglée en valeur. L'invariant générique vit dans
	/// <see cref="VirtuesDeckBandInvariantGuardTests"/> ; ces épingles portent
	/// les valeurs, y compris les titres de rangs de profondeur 4 (pk 7, 157)
	/// qu'aucun bandeau ne reflète — une réversion de titre y serait invisible
	/// pour la seule garde d'invariant.
	/// </summary>
	public class VirtuesDeckTitlesG15wGuardTests
	{
		private static string VirtuesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static Dictionary<string, Dictionary<string, string>> LoadRowsByPk()
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
				var pk = row.GetValueOrDefault("pk")?.Trim();
				if (!string.IsNullOrEmpty(pk))
					rows[pk] = row;
			}
			rows.Should().HaveCount(223, "le CSV Vertus compte 223 rangées.");
			return rows;
		}

		[Fact]
		public void Arbitrated_Titles_Are_In_Place_And_Retired_Forms_Gone_From_Deck()
		{
			var rows = LoadRowsByPk();
			rows.Values.Count(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("card")))
				.Should().Be(131, "le deck des Vertus compte 131 cartes — anti-vacuité.");

			rows["7"].GetValueOrDefault("title_pt").Should().Be("Objetivo claro",
				"⑮w pk 7 pt : orthographe post-réforme de 1990, « claro » et non « estabelecido ».");
			rows["7"].GetValueOrDefault("title_ru").Should().Be("Ясная цель",
				"⑮w pk 7 ru : « objectif clair », pas « objectif établi ».");
			rows["157"].GetValueOrDefault("title_pt").Should().Be("Considerações nuançadas",
				"⑮w pk 157 pt : le verbe est « nuançar » — « nuanceadas » n'est pas attesté.");
			rows["208"].GetValueOrDefault("title_pt").Should().Be("Avaliação leal da posição contrária",
				"⑮w pk 208 pt : « leal » porte le fair-play, miroir du fr.");
			rows["208"].GetValueOrDefault("title_es").Should().Be("Evaluación leal de la posición adversa",
				"⑮w pk 208 es : idem, « competencia leal ».");
			rows["208"].GetValueOrDefault("title_ru").Should().Be("Справедливая оценка позиции оппонента",
				"⑮w pk 208 ru : « loyale », pas « raisonnable ».");
			rows["208"].GetValueOrDefault("title_ar").Should().Be("تقييم منصف للموقف المخالف",
				"⑮w pk 208 ar : « منصف » (loyal), le fa dit déjà « منصفانه ».");

			var retired = new[]
			{
				("title_pt", "Objectivo estabelecido"),
				("title_ru", "Установленная цель"),
				("title_pt", "Considerações nuanceadas"),
				("title_pt", "Avaliação razoável da posição contrária"),
				("title_es", "Evaluación razonable de la posición adversa"),
				("title_ru", "Разумная оценка позиции оппонента"),
				("title_ar", "تقييم معقول للموقف المخالف"),
			};
			var deck = rows.Values.Where(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("card"))).ToList();
			foreach (var (column, form) in retired)
				deck.Count(r => (r.GetValueOrDefault(column) ?? "").Trim() == form)
					.Should().Be(0, $"la forme retirée «{form}» ({column}) a disparu du deck.");
		}

		[Fact]
		public void Pk208_Banner_Rank_Cascade_Is_Pinned()
		{
			var rows = LoadRowsByPk();
			foreach (var lang in new[] { "pt", "es", "ru", "ar" })
			{
				var title = (rows["208"].GetValueOrDefault("title_" + lang) ?? string.Empty).Trim();
				title.Should().NotBeNullOrEmpty($"pk 208 doit avoir un titre {lang}.");
				foreach (var pk in new[] { "208", "209", "210", "211" })
					rows[pk].GetValueOrDefault("subsubfamily_" + lang).Should().Be(title,
						$"⑮w : pk {pk} [{lang}] — le rang 7.3.1 est un bandeau imprimé, " +
						"la rangée et ses 3 cartes portent son titre (5 cellules/langue, 20 au total).");
			}
		}
	}
}