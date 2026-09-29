using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde de l'audit #1612 — majuscule initiale des titres Scenarii. 16 cellules
	/// corrigées (en 5 : title ; pt 11 : title_pt), mesure sur master `822876f5`,
	/// re-mesurée identique sur `6374fee4`. Le titre est rendu en tête de la face
	/// ({{titre}} → title/title_&lt;lang&gt;, AssetConverterConfig.cs:279) : le défaut
	/// se voyait sur la carte imprimée. Langues à casse seulement — ar/fa/zh sont des
	/// écritures sans casse, hors périmètre par construction. La casse INTÉRIEURE des
	/// titres (casse de phrase ou non) n'est pas couverte ici : question de norme par
	/// langue, pas un défaut (#1612 §3).
	/// </summary>
	public class ScenariiTitleCaseGuardTests
	{
		private static string ScenariiCsv => System.IO.Path.Combine(
			TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		private static readonly string[] CasedTitleColumns =
			{ "titre", "title", "title_pt", "title_es", "title_ru" };

		/// <summary>Détecteur : un titre dont le premier caractère (après trim) est
		/// minuscule. Factored pour le contrôle inverse sur données synthétiques.</summary>
		private static List<string> LowercaseStarts(IEnumerable<(string Column, string Title)> cells)
		{
			var offenders = new List<string>();
			foreach (var (column, raw) in cells)
			{
				var t = raw.Trim();
				if (t.Length > 0 && char.IsLower(t[0]))
					offenders.Add($"{column}: {t}");
			}
			return offenders;
		}

		private static List<string> LowercaseStartsInCsv()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var cells = CasedTitleColumns.SelectMany(c =>
				csv.LoadColumn(c).Select(v => (Column: c, Title: v)));
			return LowercaseStarts(cells);
		}

		[Fact]
		public void Every_Cased_Scenarii_Title_Starts_Uppercase()
		{
			var offenders = LowercaseStartsInCsv();
			offenders.Should().BeEmpty(
				"#1612 : plus aucun titre Scenarii (titre, title, title_pt, title_es, " +
				"title_ru) ne commence par une minuscule — 16 cellules corrigées " +
				"(en 5, pt 11). Une majuscule initiale est une règle typographique de " +
				"chaque langue à casse : elle se voit sur la carte imprimée.");
		}

		[Fact]
		public void Detector_Fires_On_Injected_Lowercase_Title()
		{
			// Contrôle inverse (#1612 §2) : sans témoin rouge, la garde vide ne prouve
			// rien — elle peut être morte par construction.
			var synthetic = new[]
			{
				(Column: "title", Title: "The wolf and the lamb"),
				(Column: "title_pt", Title: "baby"),
				(Column: "title_ru", Title: "Скромный вклад"),
				(Column: "title", Title: "move in together"),
			};
			var offenders = LowercaseStarts(synthetic);
			offenders.Should().BeEquivalentTo(new[] { "title_pt: baby", "title: move in together" },
				"le détecteur voit exactement les deux minuscules injectées — ni plus, ni moins.");
		}

		[Fact]
		public void Sixteen_Cells_Applied()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			CellOf(csv, "2.1.8", "title").Should().Be("The wolf and the lamb",
				"#1612 en : « the wolf and the lamb » → majuscule initiale.");
			CellOf(csv, "2.1.8", "title_pt").Should().Be("O lobo e o cordeiro",
				"#1612 pt : « o lobo e o cordeiro » — l'article initial porte la majuscule.");
			CellOf(csv, "3.2.4", "title").Should().Be("Baby",
				"#1612 en : « baby » → « Baby ».");
			CellOf(csv, "6.2.3", "title").Should().Be("Dubious alliance",
				"#1612 en : « dubious alliance ».");
			CellOf(csv, "1.3.4", "title_pt").Should().Be("Direitos humanos",
				"#1612 pt : « direitos humanos ».");
			CellOf(csv, "6.1.1", "title_pt").Should().Be("Moralização",
				"#1612 pt : « moralização » — ç/ã intacts, seule l'initiale monte.");
			CellOf(csv, "7.1.2", "title_pt").Should().Be("A escadaria",
				"#1612 pt : « a escadaria » — article initial.");
			CellOf(csv, "7.1.6", "title_pt").Should().Be("Herança",
				"#1612 pt : « herança ».");
		}

		private static string CellOf(HarvestCardIdsCsv csv, string path, string column)
		{
			var values = csv.LoadColumn(column, "path", new[] { path });
			values.Should().HaveCount(1, "une seule rangée porte ce path.");
			return values[0].Trim();
		}
	}
}
