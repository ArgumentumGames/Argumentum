using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde de FORME (#1524) sur les cellules localisées du CSV Fallacies : la fiche
	/// entière (nom + explication + exemple, chacun balisé) ne doit jamais se retrouver
	/// dans une seule colonne. Constat d'origine : <c>desc_fa</c> de PK 944 portait les
	/// trois segments étiquetés sur 3 lignes, ce qui faisait fuiter l'exemple dans la
	/// définition et cassait la garde anti-circularité du dataset CoursIA
	/// (jsboige/CoursIA#17578) ; corrigé en amont le 24/09.
	///
	/// Les colonnes sont DÉRIVÉES du header (toutes les <c>text_*</c>/<c>desc_*</c>/
	/// <c>example_*</c>), jamais d'une liste de langues en dur. <c>example_en_bis</c>
	/// n'a pas de paire <c>desc_*</c> : il n'est couvert que par (a).
	///
	/// Ce que cette garde n'établit pas : la justesse des traductions, seulement leur forme.
	/// </summary>
	public class FallaciesCellShapeGateTests
	{
		private static string FallaciesCsv =>
			Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static readonly Regex LeadingTag = new Regex(@"^\[[^\]]{1,20}\]", RegexOptions.Compiled);

		private static (List<string> headers, List<string[]> rows) Load()
		{
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};
			using var reader = new StreamReader(FallaciesCsv);
			using var csv = new CsvReader(reader, config);
			csv.Read();
			csv.ReadHeader();
			var headers = (csv.HeaderRecord ?? Array.Empty<string>()).ToList();
			var rows = new List<string[]>();
			while (csv.Read())
			{
				var row = new string[headers.Count];
				for (var i = 0; i < headers.Count; i++)
				{
					row[i] = csv.GetField(i) ?? string.Empty;
				}
				rows.Add(row);
			}
			return (headers, rows);
		}

		private static string Pk(string[] row, List<string> headers) =>
			row[headers.IndexOf("PK")]?.Trim() ?? "";

		[Fact]
		public void No_Localized_Cell_Starts_With_A_Bracketed_Segment_Tag()
		{
			var (headers, rows) = Load();
			var columns = headers.Where(h =>
				h.StartsWith("text_", StringComparison.Ordinal) ||
				h.StartsWith("desc_", StringComparison.Ordinal) ||
				h.StartsWith("example_", StringComparison.Ordinal)).ToList();
			columns.Should().NotBeEmpty("the Fallacies CSV must expose localized columns");

			var violations = new List<string>();
			foreach (var row in rows)
			{
				foreach (var col in columns)
				{
					var cell = row[headers.IndexOf(col)];
					if (LeadingTag.IsMatch(cell))
					{
						violations.Add($"PK {Pk(row, headers)} · {col} : « {cell.Substring(0, Math.Min(60, cell.Length)).Replace('\n', ' ')}… »");
					}
				}
			}

			violations.Should().BeEmpty(
				"a localized cell must carry its own segment only — a leading [tag] means the whole card sheet " +
				"(name + explanation + example) was pasted into one column (#1524). Offenders: {0}",
				string.Join(" | ", violations));
		}

		[Fact]
		public void No_Desc_Cell_Contains_Its_Example()
		{
			var (headers, rows) = Load();
			var descs = headers.Where(h => h.StartsWith("desc_", StringComparison.Ordinal)).ToList();
			descs.Should().NotBeEmpty("the Fallacies CSV must expose desc_* columns");

			var violations = new List<string>();
			foreach (var row in rows)
			{
				foreach (var col in descs)
				{
					var lang = col.Substring("desc_".Length);
					var exIdx = headers.IndexOf("example_" + lang);
					if (exIdx < 0)
					{
						continue;
					}
					var desc = row[headers.IndexOf(col)];
					var example = row[exIdx];
					if (!string.IsNullOrEmpty(desc) && !string.IsNullOrEmpty(example) &&
						example.Trim().Length >= 20 &&
						desc.Contains(example.Trim(), StringComparison.Ordinal))
					{
						violations.Add($"PK {Pk(row, headers)} · {col} contient example_{lang}");
					}
				}
			}

			violations.Should().BeEmpty(
				"the definition column must not embed the example column — that leak breaks the " +
				"CoursIA fine-tuning anti-circularity guard (#1524). Offenders: {0}",
				string.Join(" | ", violations));
		}
	}
}