using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// #1460 phase B, third organ — <b>the script-bearing languages keep their script</b>.
	///
	/// <para>
	/// The first two organs are structural (a placeholder that renders literally, a French function
	/// word in a foreign column: zero tolerated). This one is the <b>statistical</b> organ the
	/// phase contract asks for: « proportion d'écritures cyrillique, CJK et arabe — phase
	/// statistique, donc seuil nommé et justifié ». It catches the mode no word probe can: a
	/// translation pass that fails wholesale leaves the column in the WRONG script but possibly in
	/// a language no probe list covers — the pre-<c>37600e4a</c> Scenarii bug (columns silently
	/// untranslated) reads as a script collapse, whatever the filler language is.
	/// </para>
	/// <para>
	/// <b>Metric</b>: a cell is <i>script-dominant</i> when the majority of its letters belong to
	/// the expected script (Cyrillic for ru, CJK ideographs for zh, Arabic script for ar and fa —
	/// Farsi shares the script with Arabic, its specific letters are inside the same ranges).
	/// Per language, the proportion of script-dominant cells over non-empty localized cells.
	/// </para>
	/// <para>
	/// <b>Named thresholds, each justified by measurement</b> (calibration 08/11 on <c>db037c78</c>,
	/// corpus layer, <c>link_*</c> excluded — URLs are Latin by construction):
	/// <list type="bullet">
	/// <item><b>Corpus ≥ 95 %</b> per language. Measured healthy: ru 99,86 % · zh 99,80 % ·
	/// ar 99,99 % · fa 99,92 %. The historical failure mode is a whole file's column set
	/// untranslated: Scenarii alone (1 336 ru cells of 11 062) collapsing takes the proportion to
	/// 87,9 % — under the floor. The floor also tolerates a 25× growth of the legitimate Latin
	/// carriers (zh: 22 measured, 553 allowed).</item>
	/// <item><b>Per file ≥ 80 %</b> (files with ≥ 6 probed cells). Measured minimum: 83,3 %
	/// (Rules P&P zh — ONE legitimate Latin carrier of 6 cells; small-N is why this floor is not
	/// 90 %). A collapsed file reads ≈ 0 %. The corpus floor catches the large files' partial
	/// drift that a per-file 80 % would miss (a tenth of Scenarii untranslated = 89,8 % passes
	/// the per-file floor, fails the corpus one).</item>
	/// </list>
	/// The legitimate Latin-dominant carriers are measured and named: Latin fallacy names kept
	/// verbatim in Russian (« Credo quia absurdum », « PIDOOMA »), Latin legal maxims embedded in
	/// zh/ar prose (« Nemo auditur… »), syllogism mnemonics (« Barbara 三段论 »), acronyms
	/// (« DARVO »), proper nouns (« Aurelia Cotta », « Ergo sum »). They are the reason the
	/// threshold is a proportion and not 100 %.
	/// </para>
	/// <para>
	/// Source, like the other two organs: template × CSV — corpus layer (DataSets of the enabled
	/// CardSets) plus embedded layer for the CardSets without DataSet (the backs), restricted to
	/// mustache-referenced stems. The Chinese back title <c>tagline_zh</c> is a script-bearing
	/// cell of the embedded layer: a French back under a <c>_zh</c> suffix is exactly the #1141
	/// family, and lands here as a per-file collapse of a one-cell file.
	/// </para>
	/// </summary>
	public class ScriptProportionGuardTests
	{
		private readonly ITestOutputHelper _output;

		public ScriptProportionGuardTests(ITestOutputHelper output) => _output = output;

		/// <summary>Script-bearing languages and the script their columns must be dominant in.</summary>
		private static readonly (string Lang, string Script, string English)[] ScriptLanguages =
		{
			("ru", "cyrillic", "Russian"),
			("zh", "cjk", "Chinese"),
			("ar", "arabic", "Arabic"),
			("fa", "arabic", "Farsi"),
		};

		/// <summary>Corpus-level floor — see the class doc for the measured justification.</summary>
		private const double CorpusDominantFloor = 0.95;

		/// <summary>Per-file floor — see the class doc for the measured justification.</summary>
		private const double PerFileDominantFloor = 0.80;

		/// <summary>Files below this many probed cells are not held to the per-file floor (small-N).</summary>
		private const int PerFileMinimumCells = 6;

		[Fact]
		public void Russian_Columns_Keep_Their_Cyrillic_Script() =>
			Language_Keeps_Its_Script("ru", "cyrillic", "Russian");

		[Fact]
		public void Chinese_Columns_Keep_Their_Cjk_Script() =>
			Language_Keeps_Its_Script("zh", "cjk", "Chinese");

		[Fact]
		public void Arabic_Columns_Keep_Their_Arabic_Script() =>
			Language_Keeps_Its_Script("ar", "arabic", "Arabic");

		[Fact]
		public void Farsi_Columns_Keep_Their_Arabic_Script() =>
			Language_Keeps_Its_Script("fa", "arabic", "Farsi");

		[Fact]
		public void The_Measurement_Is_Not_Vacuous()
		{
			var stats = ScriptStats().ToList();

			stats.Should().HaveCountGreaterThanOrEqualTo(4,
				"the four script-bearing languages must each be measured — saw: " +
				string.Join(" · ", stats.Select(s => s.Lang)));

			foreach (var s in stats)
			{
				s.Files.Should().HaveCountGreaterThanOrEqualTo(4,
					$"[{s.Lang}] the measurement must reach the corpus files — saw {s.Files.Count}");
				s.TotalCells.Should().BeGreaterThanOrEqualTo(8_000,
					$"[{s.Lang}] the measurement must read a corpus-scale cell population; saw {s.TotalCells}");
				_output.WriteLine(
					$"[{s.Lang}] {s.DominantCells}/{s.TotalCells} = {s.DominantRatio:P2} dominant, " +
					$"porteurs latins légitimes {s.TotalCells - s.DominantCells}");
			}
		}

		[Fact]
		public void A_Wholesale_Translation_Failure_Collapses_The_Metric()
		{
			// Inverse control on REAL data, the historical mode: the Scenarii Russian columns
			// replaced wholesale (in memory) by untranslated filler. The corpus proportion must drop
			// under the corpus floor AND the file proportion under the per-file floor — otherwise the
			// thresholds are numbers, not detectors.
			var tables = LoadTables().ToList();
			var scenarii = tables.First(t =>
				t.Label.EndsWith("Scenarii - Cards.csv", StringComparison.OrdinalIgnoreCase));

			var mutated = scenarii.Clone();
			mutated.ReplaceLocalizedCells("ru", "Le contenu non traduit de la carte scenario.");
			mutated.Should().NotBeSameAs(scenarii);

			var corpusWithout = tables.Where(t => !ReferenceEquals(t, scenarii)).ToList();
			var stats = ScriptStatsOf(corpusWithout.Append(mutated), "ru", "cyrillic");

			stats.DominantRatio.Should().BeLessThan(CorpusDominantFloor,
				$"Scenarii ru collapsing (1 336 cells) takes the corpus from ~99,9 % to ~88 % — under the " +
				$"floor of {CorpusDominantFloor:P0}; measured {stats.DominantRatio:P2}");

			var fileStats = stats.Files.First(f => f.Label == mutated.Label);
			fileStats.DominantRatio.Should().BeLessThan(PerFileDominantFloor,
				$"the collapsed file itself must read under the per-file floor; measured {fileStats.DominantRatio:P2}");
		}

		[Fact]
		public void The_Measured_Latin_Carriers_Are_Within_The_Named_Tolerance()
		{
			// The thresholds are justified by MEASURED legitimate carriers (22 zh cells worst case,
			// listed in the class doc). If a future edit multiplies them — a machine pass dropping
			// Latin text into target-script columns — the corpus floor goes red for the right reason.
			// This companion keeps the calibration honest: the current corpus sits far ABOVE the
			// floor, and this test pins the measured margin so a silent drift is visible.
			var stats = ScriptStats().ToDictionary(s => s.Lang);

			foreach (var (lang, _, _) in ScriptLanguages)
			{
				var s = stats[lang];
				s.DominantRatio.Should().BeGreaterThanOrEqualTo(0.99,
					$"[{lang}] measured 08/11 at ≥ 99,80 %; a drift under 99 % is not noise — it is content " +
					"changing script and must be arbitrated, not absorbed by the 95 % floor");
			}
		}

		// ────────────────────────────── engine ──────────────────────────────

		private void Language_Keeps_Its_Script(string lang, string script, string english)
		{
			var stats = ScriptStatsOf(LoadTables(), lang, script);

			stats.DominantRatio.Should().BeGreaterThanOrEqualTo(CorpusDominantFloor,
				$"[{lang}] a script proportion under the floor is the wholesale-translation mode " +
				$"(pre-37600e4a Scenarii bug: whole column sets silently untranslated). Corpus: " +
				$"{stats.DominantCells}/{stats.TotalCells} = {stats.DominantRatio:P2}");

			var collapsed = stats.Files
				.Where(f => f.TotalCells >= PerFileMinimumCells && f.DominantRatio < PerFileDominantFloor)
				.ToList();
			collapsed.Should().BeEmpty(
				$"[{lang}] each corpus file keeps its script (floor {PerFileDominantFloor:P0} for files ≥ " +
				$"{PerFileMinimumCells} cells; small-N files tolerate their one legitimate Latin carrier). " +
				"Collapsed files: " + string.Join(" · ", collapsed.Select(f => $"{f.Label} {f.DominantCells}/{f.TotalCells}")));
		}

		private IEnumerable<LanguageStats> ScriptStats()
		{
			var tables = LoadTables().ToList();
			foreach (var (lang, script, _) in ScriptLanguages)
				yield return ScriptStatsOf(tables, lang, script);
		}

		private static LanguageStats ScriptStatsOf(IEnumerable<CsvTable> tables, string lang, string script)
		{
			var files = new List<FileStats>();
			var dominantTotal = 0;
			var cellsTotal = 0;

			foreach (var table in tables)
			{
				var dominant = 0;
				var cells = 0;
				foreach (var column in table.ColumnsOf(lang))
				{
					var idx = Array.IndexOf(table.Header, column);
					foreach (var row in table.Rows)
					{
						var value = row[idx];
						if (string.IsNullOrWhiteSpace(value)) continue;
						cells++;
						if (IsDominant(value, script)) dominant++;
					}
				}

				if (cells > 0)
				{
					files.Add(new FileStats(table.Label, dominant, cells));
					dominantTotal += dominant;
					cellsTotal += cells;
				}
			}

			return new LanguageStats(lang, dominantTotal, cellsTotal, files);
		}

		/// <summary>
		/// Majority of the cell's <c>\p{L}</c> letters belong to the expected script. Digits and
		/// punctuation count for neither side (an Arabic-Indic digit is not a letter); a Latin URL
		/// inside a target-script sentence does not flip the cell unless it outweighs the rest.
		/// </summary>
		private static bool IsDominant(string value, string script)
		{
			var counts = new Dictionary<string, int>();
			var total = 0;
			foreach (var ch in value)
			{
				if (!char.IsLetter(ch)) continue;
				total++;
				var s = ScriptOf(ch);
				if (s == null) continue;
				counts.TryGetValue(s, out var n);
				counts[s] = n + 1;
			}

			if (total == 0) return false;
			counts.TryGetValue(script, out var scriptCount);
			return scriptCount * 2 > total;
		}

		/// <summary>Unicode ranges per script — the calibration instrument's own classification.</summary>
		private static string? ScriptOf(char ch)
		{
			var o = (int)ch;
			if (o >= 0x0400 && o <= 0x04FF) return "cyrillic";
			if ((o >= 0x4E00 && o <= 0x9FFF) || (o >= 0x3400 && o <= 0x4DBF) || (o >= 0xF900 && o <= 0xFAFF))
				return "cjk";
			if ((o >= 0x0600 && o <= 0x06FF) || (o >= 0x0750 && o <= 0x077F)
			    || (o >= 0xFB50 && o <= 0xFDFF) || (o >= 0xFE70 && o <= 0xFEFF))
				return "arabic";
			if (o < 0x0250) return "latin";
			return null; // other scripts (Greek, Hebrew…) count toward neither side
		}

		// ────────────────────────────── sources ──────────────────────────────

		private static IEnumerable<CsvTable> LoadTables()
		{
			var config = new AssetConverterConfig();
			var dataSets = config.DataSets
				.Where(d => d != null && !string.IsNullOrWhiteSpace(d.Name))
				.GroupBy(d => d.Name, StringComparer.Ordinal)
				.ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

			var corpusPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (var set in config.WebBasedGeneratorConfig.CardSets.Where(c => c != null && c.IsEnabled))
			{
				foreach (var info in new[] { set.FaceCardSetInfo, set.BackCardSetInfo }.Where(i => i != null))
				{
					if (string.IsNullOrWhiteSpace(info.DataSet)) continue;
					if (!dataSets.TryGetValue(info.DataSet, out var ds)) continue;
					var path = ResolveRepoRelative(ds.DebugFilePath);
					if (path != null && File.Exists(path)) corpusPaths.Add(path);
				}
			}

			foreach (var path in corpusPaths.OrderBy(p => p, StringComparer.OrdinalIgnoreCase))
			{
				var table = ReadCsvTable(path, File.ReadAllText(path), restrictToStems: null);
				if (table != null) yield return table;
			}

			var seenEmbedded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (var set in config.WebBasedGeneratorConfig.CardSets.Where(c => c != null && c.IsEnabled))
			{
				foreach (var info in new[] { set.FaceCardSetInfo, set.BackCardSetInfo }.Where(i => i != null))
				{
					if (!string.IsNullOrWhiteSpace(info.DataSet)) continue;
					var resolved = ResolveRepoRelative(info.JsonFilePathDebug);
					if (resolved == null || !File.Exists(resolved) || !seenEmbedded.Add(resolved)) continue;

					string mustache;
					string embedded;
					try
					{
						using var doc = JsonDocument.Parse(File.ReadAllText(resolved));
						mustache = doc.RootElement.TryGetProperty("mustache", out var m) && m.ValueKind == JsonValueKind.String
							? m.GetString() ?? string.Empty
							: string.Empty;
						embedded = doc.RootElement.TryGetProperty("csv", out var c) && c.ValueKind == JsonValueKind.String
							? c.GetString() ?? string.Empty
							: string.Empty;
					}
					catch (JsonException)
					{
						continue;
					}

					if (string.IsNullOrEmpty(embedded)) continue;

					var table = ReadCsvTable(resolved, embedded, ReferencedStems(mustache));
					if (table != null) yield return table;
				}
			}
		}

		/// <summary>Same stem extraction as the second organ (backs reference the FR field).</summary>
		private static HashSet<string> ReferencedStems(string mustache)
		{
			var stems = new HashSet<string>(StringComparer.Ordinal);
			foreach (Match m in Regex.Matches(mustache, @"\{\{[#^/]?\s*([A-Za-z_][A-Za-z0-9_.\[\]]*)"))
			{
				var name = m.Groups[1].Value;
				var dot = name.IndexOf('.');
				if (dot >= 0)
					name = name.Substring(dot + 1).TrimStart(']', '[', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9').TrimStart('.');
				name = name.Trim();
				if (name.Length < 2) continue;
				if (name.Length > 3 && name[name.Length - 3] == '_') name = name.Substring(0, name.Length - 3);
				if (name.Length > 0) stems.Add(name);
			}
			return stems;
		}

		private static CsvTable? ReadCsvTable(string pathForLabel, string csvContent, HashSet<string>? restrictToStems)
		{
			try
			{
				using var reader = new StringReader(csvContent);
				using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					MissingFieldFound = null,
					HeaderValidated = null,
					BadDataFound = null,
				});
				if (!csv.Read() || !csv.ReadHeader()) return null;

				var header = csv.HeaderRecord ?? Array.Empty<string>();
				var rows = new List<string[]>();
				while (csv.Read())
				{
					var row = new string[header.Length];
					for (var i = 0; i < header.Length; i++)
						row[i] = csv.GetField(i) ?? string.Empty;
					rows.Add(row);
				}

				return new CsvTable(Path.GetFileName(pathForLabel), header, rows, restrictToStems);
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static string? ResolveRepoRelative(string configPath)
		{
			if (string.IsNullOrWhiteSpace(configPath)) return null;
			var normalized = configPath.Replace('\\', '/');
			var idx = normalized.IndexOf("Cards/", StringComparison.OrdinalIgnoreCase);
			if (idx < 0) return null;

			var tail = normalized.Substring(idx).Replace('/', Path.DirectorySeparatorChar);
			return Path.Combine(TestRepoRoot.Find(), tail);
		}

		private sealed class LanguageStats
		{
			public LanguageStats(string lang, int dominantCells, int totalCells, List<FileStats> files)
			{
				Lang = lang; DominantCells = dominantCells; TotalCells = totalCells; Files = files;
			}

			public string Lang { get; }
			public int DominantCells { get; }
			public int TotalCells { get; }
			public List<FileStats> Files { get; }
			public double DominantRatio => TotalCells == 0 ? 0 : (double)DominantCells / TotalCells;
		}

		private sealed class FileStats
		{
			public FileStats(string label, int dominantCells, int totalCells)
			{
				Label = label; DominantCells = dominantCells; TotalCells = totalCells;
			}

			public string Label { get; }
			public int DominantCells { get; }
			public int TotalCells { get; }
			public double DominantRatio => TotalCells == 0 ? 0 : (double)DominantCells / TotalCells;
		}

		private sealed class CsvTable
		{
			private readonly string[] _header;
			private readonly HashSet<string>? _restrictToStems;

			public CsvTable(string label, string[] header, List<string[]> rows, HashSet<string>? restrictToStems)
			{
				Label = label;
				_header = header;
				Rows = rows;
				_restrictToStems = restrictToStems;
			}

			public string Label { get; }
			public string[] Header => _header;
			public List<string[]> Rows { get; }

			public IEnumerable<string> ColumnsOf(string lang)
			{
				var suffix = "_" + lang;
				foreach (var h in _header)
				{
					if (h.StartsWith("link", StringComparison.OrdinalIgnoreCase)) continue;
					if (!h.EndsWith(suffix, StringComparison.Ordinal)) continue;
					if (_restrictToStems != null)
					{
						var stem = h.Substring(0, h.Length - suffix.Length);
						if (!_restrictToStems.Contains(stem)) continue;
					}
					yield return h;
				}
			}

			/// <summary>Replaces every non-empty localized cell of <paramref name="lang"/> — the mutation lever.</summary>
			public void ReplaceLocalizedCells(string lang, string filler)
			{
				foreach (var column in ColumnsOf(lang).ToList())
				{
					var idx = Array.IndexOf(_header, column);
					for (var i = 0; i < Rows.Count; i++)
						if (!string.IsNullOrWhiteSpace(Rows[i][idx]))
							Rows[i][idx] = filler;
				}
			}

			public CsvTable Clone() => new CsvTable(Label, (string[])_header.Clone(),
				Rows.Select(r => (string[])r.Clone()).ToList(), _restrictToStems);
		}
	}
}
