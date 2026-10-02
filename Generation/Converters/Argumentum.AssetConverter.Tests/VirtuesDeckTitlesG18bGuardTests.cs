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
	/// Épingles du grain ⑱b (arbitrage ai-01 #458 c.5946000193, cycle L) : retrait du kasra
	/// U+0650 des QUATRE derniers titres fa qui en portaient — signalés par le témoin ⑱
	/// (le dossier ⑰ ne les voyait pas), arbitrés par ai-01 : pk 19, 50, 184 (ezafe) et
	/// 126 (kasra PHONÉTIQUE de la transcription « Fresison », retiré comme les autres).
	///
	/// <para><b>Gardé par décision explicite</b> : le redoublement (shadda U+0651) de pk 83
	/// « ردّ علل جایگزین » — un redoublement consonantique légitime, PAS un kasra — et les
	/// noms latins des modes (transcriptions latines des syllogismes, 126 comprise).</para>
	///
	/// <para><b>Portée</b> : les 4 rangées sont hors deck (card vide, profondeur 4-7) —
	/// aucun n'est un rang de bandeau, la cascade ⑩/⑲w attendue est NULLE (l'invariant
	/// bandeau reste vert sans aucune écriture de bandeau).</para>
	/// </summary>
	public class VirtuesDeckTitlesG18bGuardTests
	{
		private const char Kasra = 'ِ';
		private const char Shadda = 'ّ';

		private static string VirtuesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static Dictionary<string, Dictionary<string, string>> LoadRowsByPk()
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			using var reader = new StringReader(File.ReadAllText(VirtuesCsv));
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
		public void Kasra_Is_Gone_From_Every_Fa_Title_And_The_Four_Retired_Cells_Are_Pinned()
		{
			var rows = LoadRowsByPk();

			// Les 4 cellules arbitrées — valeurs DÉRIVÉES du retrait du seul caractère U+0650.
			rows["19"].GetValueOrDefault("title_fa").Should().Be("گواهی غیرمناقشه‌برانگیز",
				"⑱b pk 19 fa : kasra d'ezafe retiré, le ZWNJ de غیرمناقشه‌برانگیز reste.");
			rows["50"].GetValueOrDefault("title_fa").Should().Be("آرایه‌های پشتیبان روشنی",
				"⑱b pk 50 fa : kasra d'ezafe retiré.");
			rows["126"].GetValueOrDefault("title_fa").Should().Be("قیاس فرسیسون",
				"⑱b pk 126 fa : kasra PHONÉTIQUE retiré comme les ezafe (arbitrage ai-01) — la " +
				"transcription latine « Fresison » reste lisiblement فرسیسون.");
			rows["184"].GetValueOrDefault("title_fa").Should().Be("سطح کافی دلیل",
				"⑱b pk 184 fa : kasra d'ezafe retiré.");

			// Anti-dérive GLOBALE : après ⑱ (162/164) et ⑱b (19/50/126/184), AUCUN titre fa
			// ne porte plus de kasra — le scan des 223 cellules est la garantie que la famille
			// entière est traitée, pas seulement les rangées arbitrées.
			rows.Values.Select(r => r.GetValueOrDefault("title_fa") ?? "")
				.Should().OnlyContain(t => t.IndexOf(Kasra) < 0,
					"après ⑱+⑱b, plus aucun title_fa ne porte U+0650 (les 6 cellules concernées " +
					"sont retirées : 19/50/126/184 ici, 162/164 en ⑱).");
		}

		[Fact]
		public void Pk83_Keeps_Its_Shadda_And_The_Latin_Mode_Names_Stay()
		{
			var rows = LoadRowsByPk();

			// Décision explicite de l'arbitrage : le redoublement (shadda U+0651) de 83 est
			// GARDÉ — c'est un redoublement consonantique (ردّ = radd), pas un kasra. Ce
			// témoin prouve que ⑱b retire le kasra SEUL, pas les diacritiques en bloc.
			rows["83"].GetValueOrDefault("title_fa").Should().Be("ردّ علل جایگزین",
				"le redoublement de pk 83 (shadda sur ردّ) est gardé par l'arbitrage ⑱b.");
			rows["83"].GetValueOrDefault("title_fa").Should().Contain(Shadda.ToString(),
				"la shadda U+0651 reste présente dans 83 — seuls les kasra U+0650 sont retirés.");
		}
	}
}
