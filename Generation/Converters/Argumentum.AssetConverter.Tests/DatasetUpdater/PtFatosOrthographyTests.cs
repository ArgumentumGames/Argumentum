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

namespace Argumentum.AssetConverter.Tests.DatasetUpdater
{
	/// <summary>
	/// Orthography contract for the pt columns of the corpus (#458 pocket ①, decided ai-01
	/// 10/10 c.6099000632): the corpus language target is the pt-BR spelling « fatos » —
	/// « factos » (pt-PT) is banned from every _pt column, in any case form.
	///
	/// The grain converted 35 occurrences (2026-10-12): 33 lowercase in Virtues (desc/remark
	/// prose + the « Fidelidade aos factos »/« Verdade dos factos » family labels), the
	/// capitalized title « Factos verificados » (Virtues pk 22, title_pt), and 1 in Scenarii
	/// (6.1.1 suggestion_pt). Decision is orthographic, NOT a family renaming — the canonical
	/// taxonomy labels keep their identity, only their spelling moves with the corpus norm
	/// (Fallacies already wrote « fatos » 111 times; pre-grain Virtues was the outlier).
	///
	/// Case-insensitivity is the point, not a nicety: the byte-surgery pass was case-sensitive
	/// and left « Factos verificados » standing — a post-surgery case-INSENSITIVE scan caught
	/// it. The guard scans the same way the miss was found.
	///
	/// Second referential checked: DatasetUpdater Resources/ (translation prompts) carry no
	/// « factos » — nothing to synchronize for future translation passes.
	///
	/// Falsifiability: red on the pre-grain tree (35 « factos » trip the ban, including the
	/// capitalized one); re-injecting « Facto » in any case trips fact 1; the synthetic
	/// literals of fact 4 prove the detector can see a 1 (a 0 is only an absence if the
	/// instrument could have seen a 1).
	/// </summary>
	public class PtFatosOrthographyTests
	{
		private static readonly (string relativePath, string name)[] TargetFiles =
		{
			(Path.Combine("Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv"), "fallacies"),
			(Path.Combine("Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv"), "virtues"),
			(Path.Combine("Cards", "Scenarii", "Argumentum Scenarii - Cards.csv"), "scenarii"),
			(Path.Combine("Cards", "Rules", "Argumentum Rules - Cards.csv"), "rules"),
			(Path.Combine("Cards", "Rules", "Argumentum Rules - Cards Print and Play.csv"), "rules-pp"),
		};

		/// <summary>« facto »/« factos » in any case form, word-bounded (« artefactos » must NOT trip).</summary>
		private static readonly Regex Factos = new(@"\bfactos?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private static string RepoRoot() => TestRepoRoot.Find();

		private static List<(string file, string pk, string column, string cell)> LoadPtCells()
		{
			var cells = new List<(string, string, string, string)>();
			foreach (var (relativePath, name) in TargetFiles)
			{
				var path = Path.Combine(RepoRoot(), relativePath);
				File.Exists(path).Should().BeTrue("the corpus CSV must exist at '{0}'", relativePath);

				var config = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					MissingFieldFound = null,
					HeaderValidated = null,
				};
				using var reader = new StreamReader(path);
				using var csv = new CsvReader(reader, config);
				csv.Read();
				csv.ReadHeader();
				var headers = csv.HeaderRecord ?? Array.Empty<string>();
				var ptIndexes = headers
					.Select((h, i) => (h, i))
					.Where(t => t.h.EndsWith("_pt", StringComparison.Ordinal))
					.Select(t => t.i)
					.ToArray();
				ptIndexes.Should().NotBeEmpty("'{0}' must expose at least one _pt column", name);

				while (csv.Read())
				{
					var pk = csv.GetField(0) ?? "";
					foreach (var index in ptIndexes)
					{
						cells.Add((name, pk, headers[index], csv.GetField(index) ?? ""));
					}
				}
			}

			cells.Should().NotBeEmpty("all corpus CSVs must load rows");
			return cells;
		}

		[Fact]
		public void PtColumns_NoFactosInAnyCaseForm()
		{
			var failures = new List<string>();
			foreach (var (file, pk, column, cell) in LoadPtCells())
			{
				var offender = Factos.Match(cell);
				if (offender.Success)
				{
					failures.Add($"[{file} / {pk} / {column}] '{offender.Value}' in: " +
					             $"'{cell.Truncate(80)}…'");
				}
			}

			failures.Should().BeEmpty(
				"pt corpus spelling is « fatos » (pt-BR, decision ai-01 10/10 c.6099000632); « factos » " +
				"(pt-PT) is banned from every _pt column, capital forms included:\n{0}",
				string.Join("\n", failures));
		}

		[Fact]
		public void VirtuesFamilyLabels_UseFatosSpelling()
		{
			// Witnesses that the grain's declared object is present and its count pinned: the
			// six « Fidelidade aos fatos » sub-family labels (pks 153-158), the sub-sub-family
			// « Verdade dos fatos » (pk 154), and the two family titles (pks 153/154).
			var cells = LoadPtCells().Where(c => c.file == "virtues").ToList();

			OccurrencesIn(cells, "subfamily_pt", "Fidelidade aos fatos").Should().Be(6,
				"the six Fidelidade sub-family labels moved to the « fatos » spelling (pks 153-158)");
			OccurrencesIn(cells, "subsubfamily_pt", "Verdade dos fatos").Should().BeGreaterOrEqualTo(1,
				"the Verdade sub-sub-family label moved to the « fatos » spelling (pk 154)");
			OccurrencesIn(cells, "title_pt", "Fidelidade aos fatos").Should().BeGreaterOrEqualTo(1,
				"the Fidelidade family title (pk 153) moved to the « fatos » spelling");
			OccurrencesIn(cells, "title_pt", "Verdade dos fatos").Should().BeGreaterOrEqualTo(1,
				"the Verdade family title (pk 154) moved to the « fatos » spelling");
		}

		[Fact]
		public void FallaciesCorpusNorm_IsFatos()
		{
			// Direction witness (anti-vacuity): the corpus norm really is « fatos » — Fallacies
			// pt prose carries it 111 times pre-grain and counting (Subfamily_pt labels alone
			// hold 85). If this floor drops, the corpus drifted, not the guard.
			var fallaciesFatos = LoadPtCells()
				.Where(c => c.file == "fallacies")
				.Sum(c => Occurrences(c.cell, "fatos"));
			fallaciesFatos.Should().BeGreaterOrEqualTo(100,
				"Fallacies pt prose is the « fatos » norm the grain aligned Virtues onto");
		}

		[Fact]
		public void ScenariiConvertedCell_UsesFatos()
		{
			// Witness of the single Scenarii conversion (6.1.1, suggestion_pt — was « factos »,
			// report #1858 line « à partir des faits », registre juridique).
			var cell = LoadPtCells()
				.Where(c => c.file == "scenarii" && c.pk == "6.1.1" && c.column == "suggestion_pt")
				.ToList();
			cell.Should().HaveCount(1, "Scenarii 6.1.1 suggestion_pt must load");
			cell[0].cell.Should().Contain("a partir dos fatos",
				"the converted Scenarii cell keeps the « fatos » spelling");
		}

		[Fact]
		public void Detector_SeesSyntheticFactos()
		{
			// Control inverse, in-memory literals (never the validated tree): the same regex
			// that scans the corpus must catch every case form — including the capitalized
			// title form the case-sensitive byte-surgery missed.
			foreach (var literal in new[] { "factos", "Factos verificados", "um facto", "FACTO" })
			{
				Factos.IsMatch(literal).Should().BeTrue(
					"the detector must see '{0}' — a 0 on the corpus is only an absence if the " +
					"instrument could have seen a 1", literal);
			}

			// And must NOT catch the legitimate neighbours (word-bounding).
			foreach (var literal in new[] { "fatos", "artefatos", "artefactos", "Fatos verificados" })
			{
				Factos.IsMatch(literal).Should().BeFalse(
					"word-bounding: '{0}' is not « facto(s) »", literal);
			}
		}

		private static int OccurrencesIn(
			List<(string file, string pk, string column, string cell)> cells, string column, string needle) =>
			cells.Where(c => c.column == column).Sum(c => Occurrences(c.cell, needle));

		private static int Occurrences(string haystack, string needle)
		{
			var count = 0;
			var index = 0;
			while ((index = haystack.IndexOf(needle, index, StringComparison.Ordinal)) >= 0)
			{
				count++;
				index += needle.Length;
			}

			return count;
		}
	}
}
