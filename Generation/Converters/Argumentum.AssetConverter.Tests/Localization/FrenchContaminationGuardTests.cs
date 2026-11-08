using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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
	/// #1460 phase B, second organ — <b>no French function word survives in a non-French column</b>.
	///
	/// <para>
	/// Source, like the first organ (<see cref="Argumentum.AssetConverter.Tests.Structure.PlaceholderResidueGuardTests"/>):
	/// <b>card template × DataSet CSV</b>, never the PDF (0 text layer) and never the
	/// <c>.harvest.json</c> (pixels only). Concretely two layers:
	/// </para>
	/// <para>
	/// 1. <b>Corpus layer</b> — every localized column (<c>_en _ru _pt _es _ar _fa _zh</c>) of the CSVs
	/// of the DataSets declared by the enabled CardSets, plus the three bare-English taxonomy columns
	/// (<c>Family</c>, <c>Subfamily</c>, <c>Subsubfamily</c>), <c>link_*</c> columns excluded (URLs are
	/// Latin by construction — measured 08/11: every Latin-script cell of the ar/fa/zh columns of the
	/// Fallacies CSV that is a URL is legitimate). The corpus is probed whole, not only the columns the
	/// templates reference, because corpus columns circulate through GSheet sync and any future template
	/// can consume one — the defect this organ found at calibration lived exactly there (see below).
	/// </para>
	/// <para>
	/// 2. <b>Embedded layer</b> — for the CardSets that declare <c>DataSet = None</c> (the card backs),
	/// the template's embedded <c>csv</c> IS the data source (#813/#812 note of the first organ); the
	/// localized columns it carries are probed when their stem is referenced by the template's mustache
	/// (<c>Argumentum_Fallacies_Back_fr.json</c> references <c>tagline_fr</c>, so the localization map
	/// feeds it <c>tagline_&lt;lang&gt;</c> — the #216/#1141 family). Embedded CSVs of sets that DO
	/// declare a DataSet are stale snapshots and are NOT probed: measured 08/11, they still carry a
	/// 2022 translator note in French inside <c>example_en</c> (row 49) while the corpus twin is clean.
	/// </para>
	/// <para>
	/// <b>Probes are named per language</b>: an accented tier (homograph-free in all seven languages)
	/// and a base tier of French function words, minus the words that are legitimate words of the
	/// target language — each removal MEASURED on the corpus (08/11, master <c>db037c78</c>): Spanish
	/// dative <c>les</c> (17 cells), Spanish/Portuguese verb <c>une</c> (3 cells). The exclusions are
	/// load-bearing: the test below re-probes those words raw and requires the corpus to still contain
	/// them, so a future "cleanup" cannot silently turn the calibration vacuous.
	/// </para>
	/// <para>
	/// <b>Found at calibration and fixed in the same PR</b>: PK 753 (<c>Erreur de modalité aléthique</c>)
	/// carried the French short name « Confondre le faux et l'impossible » in <c>Simple_name_en</c> —
	/// the only French cell in an English column of the corpus (61 cells measured, 60 English). The
	/// replacement, « Confusing the false with the impossible », follows the row's own <c>desc_en</c>
	/// (« Confusing what is necessarily false… »). <c>Simple_name_en</c> is consumed by no living
	/// template (Archive/2022 and CardPen examples only), so the fix has zero render impact. The
	/// inverse control of this organ re-introduces exactly that historical cell and requires detection.
	/// </para>
	/// <para>
	/// Like the first organ this is a <b>structural</b> assertion (zero, not a proportion): a French
	/// function word in a non-French column is a defect whatever its frequency — the failure family of
	/// #216 (EN/RU/PT PDFs shipping French content) and of the pre-<c>37600e4a</c> Scenarii bug. The
	/// script-proportion organ (third of the phase) is the statistical one.
	/// </para>
	/// </summary>
	public class FrenchContaminationGuardTests
	{
		private readonly ITestOutputHelper _output;

		public FrenchContaminationGuardTests(ITestOutputHelper output) => _output = output;

		/// <summary>
		/// The seven non-French languages of the corpus. French itself (<c>_fr</c>, and the bare French
		/// columns <c>Famille</c>, <c>text_fr</c>…) is never probed — it is the legitimate content.
		/// </summary>
		private static readonly string[] ProbedLanguages = { "en", "ru", "pt", "es", "ar", "fa", "zh" };

		/// <summary>
		/// Accented French function words — homograph-free in English, Spanish, Portuguese (the accent
		/// differs: <c>más/muito/mismo/mesmo</c>), and script-foreign to ru/ar/fa/zh. Applied to every
		/// language without exception — except the loan phrases below.
		/// </summary>
		private static readonly string[] AccentedProbes =
		{
			"très", "après", "être", "était", "même", "déjà"
		};

		/// <summary>
		/// French loan phrases used AS SUCH by other languages — the word alone would fire, the fixed
		/// expression is legitimate target-language prose. MEASURED at first calibration run (08/11):
		/// « Déjà vu » as <c>title_es</c> and <c>title_pt</c> of a Scenarii card (2 cells), the
		/// international name of the phenomenon, not a translation slip. Each entry is a named,
		/// measured exemption — an exemption without a measured carrier is vacuous.
		/// </summary>
		private static readonly string[] LoanPhrasesExemptFromProbes =
		{
			"déjà vu"
		};

		/// <summary>
		/// Base tier: French function words with no accent to save them. Applied per language minus the
		/// exclusions of <see cref="HomographsOfTheTargetLanguage"/>.
		/// </summary>
		private static readonly string[] BaseProbes =
		{
			"avec", "dans", "aux", "dont", "cette", "moins", "depuis", "toujours", "ainsi", "chez",
			"selon", "faut", "peut", "doit", "reste", "contre", "rien", "des", "du", "alors",
			"et", "les", "une", "pour", "pas", "chose", "faux", "dire", "jamais"
		};

		/// <summary>
		/// Words of the BASE tier that are legitimate words of the target language, measured on the
		/// corpus (08/11, <c>db037c78</c>) or lexical:
		/// <list type="bullet">
		/// <item><b>en</b>: <c>pour</c> (English verb), <c>pas</c> (ballet loan, "pas de deux"),
		/// <c>chose</c> (past of choose), <c>dire</c> (English adjective). NOT excluded despite lexical
		/// loan risk, measured 0 legitimate uses: <c>faux</c> ("faux pas"), <c>et</c> (Latin "et
		/// cetera"), <c>les/une/des/du</c> — the 1 measured <c>faux</c>/<c>et</c> hit was the true
		/// contamination fixed in this PR.</item>
		/// <item><b>es</b>: <c>les</c> (dative pronoun — 17 measured cells: "les permitía", "les
		/// gusta"…), <c>une</c> (3sg of <c>unar/unir</c> — 1 measured: "que nos une").</item>
		/// <item><b>pt</b>: <c>une</c> (3sg of <c>unir</c> — 2 measured: "que nos une"), <c>jamais</c>
		/// (Portuguese word for "never").</item>
		/// <item><b>ru/ar/fa/zh</b>: none — the scripts differ; any Latin function word is foreign.</item>
		/// </list>
		/// </summary>
		private static readonly Dictionary<string, HashSet<string>> HomographsOfTheTargetLanguage =
			new Dictionary<string, HashSet<string>>(StringComparer.Ordinal)
			{
				["en"] = new HashSet<string>(StringComparer.Ordinal) { "pour", "pas", "chose", "dire" },
				["es"] = new HashSet<string>(StringComparer.Ordinal) { "les", "une" },
				["pt"] = new HashSet<string>(StringComparer.Ordinal) { "une", "jamais" },
				["ru"] = new HashSet<string>(StringComparer.Ordinal),
				["ar"] = new HashSet<string>(StringComparer.Ordinal),
				["fa"] = new HashSet<string>(StringComparer.Ordinal),
				["zh"] = new HashSet<string>(StringComparer.Ordinal),
			};

		/// <summary>
		/// Columns of the corpus CSVs that carry English content without a suffix. Measured 08/11 on the
		/// Fallacies taxonomy (1 408 + 1 400 + 1 379 non-empty cells, 0 French-probe hits). Only these
		/// three are named: a bare column of another CSV has not been measured as English.
		/// </summary>
		private static readonly Dictionary<string, string[]> BareEnglishColumns =
			new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
			{
				["Argumentum Fallacies - Taxonomy.csv"] = new[] { "Family", "Subfamily", "Subsubfamily" }
			};

		[Fact]
		public void No_French_Function_Word_Survives_In_English_Columns() =>
			Language_Is_Free_Of_French("en", "English");

		[Fact]
		public void No_French_Function_Word_Survives_In_Russian_Columns() =>
			Language_Is_Free_Of_French("ru", "Russian");

		[Fact]
		public void No_French_Function_Word_Survives_In_Portuguese_Columns() =>
			Language_Is_Free_Of_French("pt", "Portuguese");

		[Fact]
		public void No_French_Function_Word_Survives_In_Spanish_Columns() =>
			Language_Is_Free_Of_French("es", "Spanish");

		[Fact]
		public void No_French_Function_Word_Survives_In_Arabic_Columns() =>
			Language_Is_Free_Of_French("ar", "Arabic");

		[Fact]
		public void No_French_Function_Word_Survives_In_Farsi_Columns() =>
			Language_Is_Free_Of_French("fa", "Farsi");

		[Fact]
		public void No_French_Function_Word_Survives_In_Chinese_Columns() =>
			Language_Is_Free_Of_French("zh", "Chinese");

		[Fact]
		public void The_Scan_Is_Not_Vacuous()
		{
			var tables = ScanTables().ToList();

			// MEASURED on master db037c78 (08/11), link_* columns excluded:
			//   en ≈ 11 350 cells (incl. 4 187 bare-English taxonomy cells)
			//   ru/pt/es/ar/fa/zh ≈ 11 000 cells each — plus the 7 tagline cells of the back template.
			// The floors sit ~30% below: they are anti-collapse floors, not equalities. If a DataSet
			// stops loading (path change, config drift) the count collapses and this goes red for the
			// right reason instead of the two guards above passing over an empty corpus.
			tables.Should().HaveCountGreaterThanOrEqualTo(5,
				"the scan must reach the corpus CSVs of the card corpora (Fallacies, Virtues, Scenarii, " +
				$"Rules, Rules P&P) plus the embedded back — saw {tables.Count}: " +
				string.Join(" · ", tables.Select(t => t.Label)));

			foreach (var lang in ProbedLanguages)
			{
				var cells = tables.Sum(t => t.ProbedColumns(lang).Sum(c => t.NonEmptyCount(c)));
				cells.Should().BeGreaterThanOrEqualTo(8_000,
					$"[{lang}] the scan must actually read localized cells; saw {cells}");
				_output.WriteLine($"[{lang}] {cells} cellules sondées, {LanguageHits(tables, lang).Count()} atteinte(s)");
			}
		}

		[Fact]
		public void Homograph_Exclusions_Are_LoadBearing_On_The_Corpus()
		{
			// The exclusions of HomographsOfTheTargetLanguage are calibrated against cells that EXIST.
			// If a future pass rewrites the Spanish "les" or the Portuguese "une" (they are legitimate
			// target-language prose, see the samples in the map's doc), the corpus stops carrying them,
			// the exclusion becomes vacuous, and this test says so: re-measure and update the map, do
			// not delete the exclusion to make it green.
			var tables = ScanTables().ToList();

			var rawEs = RawHits(tables, "es", "les");
			rawEs.Should().HaveCountGreaterThanOrEqualTo(10,
				"the Spanish dative 'les' was measured at 17 cells on 08/11; an exclusion calibrated on " +
				"cells that no longer exist is vacuous — re-measure before touching the map");

			var rawPt = RawHits(tables, "pt", "une");
			rawPt.Should().HaveCountGreaterThanOrEqualTo(1,
				"the Portuguese verb 'une' was measured at 2 cells on 08/11; same rule as above");

			// Same discipline for the loan-phrase exemption: the corpus must still carry « Déjà vu »
			// in a non-French column (measured 08/11: title_es + title_pt of one Scenarii card),
			// otherwise the exemption is vacuous and must be re-measured, not deleted.
			var loan = RawHits(tables, "pt", "déjà");
			loan.Should().HaveCountGreaterThanOrEqualTo(1,
				"the « Déjà vu » loan-phrase exemption is calibrated on 2 measured cells; if they are " +
				"gone, re-measure the exemption instead of deleting it");
		}

		[Fact]
		public void Reintroduced_Historical_Contamination_Is_Detected()
		{
			// Inverse control against the REAL corpus, not a fixture: put the historical French cell of
			// PK 753 back into a copy of the Fallacies table, and inject fresh French into a Russian
			// column. Both must be detected, naming the probe — otherwise the guard above is a
			// transcription, not a detector.
			var tables = ScanTables().ToList();
			var fallacies = tables.First(t => t.Label.EndsWith("Fallacies - Taxonomy.csv", StringComparison.OrdinalIgnoreCase));

			var mutated = fallacies.Clone();
			var historical = mutated.SetCell(pk: "753", column: "Simple_name_en",
				value: "Confondre le faux et l'impossible");
			historical.Should().BeTrue("PK 753 and its Simple_name_en column must exist in the Fallacies corpus");

			var enHits = LanguageHits(new[] { mutated }, "en");
			enHits.Should().NotBeEmpty(
				"the historical PK 753 cell (French in an English column, fixed in this PR) must trip the " +
				"English probe set on re-introduction");
			enHits.Should().Contain(h => h.Probes.Contains("faux") || h.Probes.Contains("et"),
				"the cell 'Confondre le faux et l'impossible' is detectable through its unambiguous parts");

			var ruColumn = mutated.Header.FirstOrDefault(h => h.EndsWith("_ru", StringComparison.Ordinal)
			                                                   && !h.StartsWith("link", StringComparison.OrdinalIgnoreCase)
			                                                   && mutated.NonEmptyCount(h) > 0);
			ruColumn.Should().NotBeNull("the Fallacies corpus carries non-empty Russian columns");
			var ruRow = mutated.Rows.FindIndex(r => !string.IsNullOrWhiteSpace(r[Array.IndexOf(mutated.Header, ruColumn!)]));
			mutated.Rows[ruRow][Array.IndexOf(mutated.Header, ruColumn!)] =
				"Cette affirmation très répandue reste pourtant fausse.";

			var ruHits = LanguageHits(new[] { mutated }, "ru");
			ruHits.Should().Contain(h => h.Probes.Contains("très") || h.Probes.Contains("cette"),
				"injected French in a Russian column must be detected through the accented tier or the " +
				"base tier");
		}

		// ────────────────────────────── engine ──────────────────────────────

		private void Language_Is_Free_Of_French(string lang, string englishName)
		{
			var hits = LanguageHits(ScanTables(), lang).ToList();
			hits.Should().BeEmpty(
				$"a French function word inside a {englishName} column is the #216 failure family " +
				"(a conversion map or a translation pass that silently left French in place). Probes are " +
				"named per language; homographs of the target language are excluded and their exclusion is " +
				"guarded by Homograph_Exclusions_Are_LoadBearing_On_The_Corpus. Hits: " +
				string.Join(" · ", hits));
		}

		private static IEnumerable<Hit> LanguageHits(IEnumerable<CsvTable> tables, string lang)
		{
			var probes = ProbesFor(lang, unfiltered: false);
			var regex = ProbeRegex(probes);
			foreach (var table in tables)
			{
				foreach (var column in table.ProbedColumns(lang))
				{
					var idx = Array.IndexOf(table.Header, column);
					for (var i = 0; i < table.Rows.Count; i++)
					{
						var value = table.Rows[i][idx];
						if (string.IsNullOrWhiteSpace(value)) continue;

						var matches = regex.Matches(WithoutExemptLoanPhrases(value))
							.Select(m => m.Value)
							.Distinct(StringComparer.OrdinalIgnoreCase)
							.ToList();
						if (matches.Count > 0)
							yield return new Hit(table.Label, column, i + 2, table.PkOf(i), matches, Snippet(value));
					}
				}
			}
		}

		/// <summary>Probes one raw word with NO homograph filtering — for the load-bearing control.</summary>
		private static IEnumerable<Hit> RawHits(IEnumerable<CsvTable> tables, string lang, string word) =>
			LanguageHitsWithWords(tables, lang, new[] { word });

		private static IEnumerable<Hit> LanguageHitsWithWords(IEnumerable<CsvTable> tables, string lang, IReadOnlyList<string> words)
		{
			var regex = ProbeRegex(words);
			foreach (var table in tables)
			{
				foreach (var column in table.ProbedColumns(lang))
				{
					var idx = Array.IndexOf(table.Header, column);
					for (var i = 0; i < table.Rows.Count; i++)
					{
						var value = table.Rows[i][idx];
						if (string.IsNullOrWhiteSpace(value)) continue;

						// RAW path: no loan-phrase stripping — this instrument exists precisely to prove
						// the corpus still carries the phrases the exemptions are calibrated on.
						var matches = regex.Matches(value).Select(m => m.Value).ToList();
						if (matches.Count > 0)
							yield return new Hit(table.Label, column, i + 2, table.PkOf(i), matches, Snippet(value));
					}
				}
			}
		}

		private static List<string> ProbesFor(string lang, bool unfiltered)
		{
			var probes = new List<string>(AccentedProbes);
			probes.AddRange(unfiltered
				? BaseProbes
				: BaseProbes.Where(p => !HomographsOfTheTargetLanguage[lang].Contains(p)));
			return probes;
		}

		/// <summary>
		/// Word boundary by lookaround on Unicode letters/digits/underscore, NOT <c>\b</c>: in .NET a
		/// CJK character is a word character, so <c>\bavec\b</c> would miss "他avec我" — a Latin French
		/// word glued to Chinese text is precisely the contamination being probed for.
		/// </summary>
		private static Regex ProbeRegex(IReadOnlyList<string> words) =>
			new Regex(
				"(?<![\\p{L}\\p{N}_])(" + string.Join("|", words.Select(Regex.Escape)) + ")(?![\\p{L}\\p{N}_])",
				RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

		/// <summary>Strips the loan phrases before probing, so their words do not fire.</summary>
		private static string WithoutExemptLoanPhrases(string value)
		{
			foreach (var phrase in LoanPhrasesExemptFromProbes)
				value = Regex.Replace(value, Regex.Escape(phrase), " ", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
			return value;
		}

		private static string Snippet(string value)
		{
			var flat = Regex.Replace(value, @"\s+", " ").Trim();
			return flat.Length <= 70 ? flat : flat.Substring(0, 70) + "…";
		}

		// ────────────────────────────── sources ──────────────────────────────

		private static IEnumerable<CsvTable> ScanTables()
		{
			var config = new AssetConverterConfig();
			var dataSets = config.DataSets
				.Where(d => d != null && !string.IsNullOrWhiteSpace(d.Name))
				.GroupBy(d => d.Name, StringComparer.Ordinal)
				.ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

			var corpusPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			// Corpus layer: the DataSets the enabled CardSets actually declare.
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
				var table = ReadCsvTable(path, File.ReadAllText(path));
				if (table != null) yield return table;
			}

			// Embedded layer: sets with NO DataSet — the embedded csv IS the data source. Only the
			// columns whose stem the mustache references are probed (see the class doc for why the
			// stale snapshots of DataSet-declaring sets are excluded).
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

					var table = ReadCsvTable(resolved, embedded);
					if (table == null) continue;
					table.RestrictColumnsToStems(ReferencedStems(mustache));
					if (table.Header.Length > 0) yield return table;
				}
			}
		}

		/// <summary>
		/// Stems of the fields the mustache references: <c>tagline_fr</c> → <c>tagline</c>,
		/// <c>rowset.[0].cat</c> → <c>cat</c>. Any trailing two-letter language suffix is stripped.
		/// </summary>
		private static HashSet<string> ReferencedStems(string mustache)
		{
			var stems = new HashSet<string>(StringComparer.Ordinal);
			foreach (Match m in Regex.Matches(mustache, @"\{\{[#^/]?\s*([A-Za-z_][A-Za-z0-9_.\[\]]*)"))
			{
				var name = m.Groups[1].Value;
				var dot = name.IndexOf('.');
				if (dot >= 0) name = name.Substring(dot + 1).TrimStart(']', '[', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9').TrimStart('.');
				name = name.Trim();
				if (name.Length < 2) continue;

				// strip a trailing _xx language suffix (fr included — the back templates reference the
				// French field and the conversion map swaps in the target language)
				if (name.Length > 3 && name[name.Length - 3] == '_') name = name.Substring(0, name.Length - 3);
				if (name.Length > 0) stems.Add(name);
			}
			return stems;
		}

		private static CsvTable? ReadCsvTable(string pathForLabel, string csvContent)
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

				// Index-based reads, positional header mapping: the corpus carries duplicated column
				// names (the embedded face CSVs even carry a leading-space ' text_ru'), and a name-based
				// GetField on a duplicate is at best first-match. Position is the only stable key.
				var header = csv.HeaderRecord ?? Array.Empty<string>();
				var rows = new List<string[]>();
				while (csv.Read())
				{
					var row = new string[header.Length];
					for (var i = 0; i < header.Length; i++)
						row[i] = csv.GetField(i) ?? string.Empty;
					rows.Add(row);
				}

				return new CsvTable(Path.GetFileName(pathForLabel), header, rows, BareEnglishColumns);
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Resolves a config path at its <c>Cards</c> (or <c>docs</c>) segment under the located
		/// repository root — the same re-anchoring contract as the first organ.
		/// </summary>
		private static string? ResolveRepoRelative(string configPath)
		{
			if (string.IsNullOrWhiteSpace(configPath)) return null;
			var normalized = configPath.Replace('\\', '/');
			var idx = normalized.IndexOf("Cards/", StringComparison.OrdinalIgnoreCase);
			if (idx < 0) return null;

			var tail = normalized.Substring(idx).Replace('/', Path.DirectorySeparatorChar);
			var root = TestRepoRoot.Find();
			return Path.Combine(root, tail);
		}

		private sealed class Hit
		{
			public Hit(string file, string column, int line, string pk, List<string> probes, string snippet)
			{
				File = file; Column = column; Line = line; Pk = pk; Probes = probes; Snippet = snippet;
			}

			public string File { get; }
			public string Column { get; }
			public int Line { get; }
			public string Pk { get; }
			public List<string> Probes { get; }
			public string Snippet { get; }

			public override string ToString()
			{
				var pk = string.IsNullOrWhiteSpace(Pk) ? "" : $" PK {Pk}";
				return $"{File} → {Column} (ligne {Line}{pk}) sondes [{string.Join(",", Probes)}] : « {Snippet} »";
			}
		}

		private sealed class CsvTable
		{
			private readonly Dictionary<string, string[]> _bareEnglishColumns;
			private string[] _header; // mutated by RestrictColumnsToStems (embedded layer)
			private readonly int _pkIndex;

			public CsvTable(string label, string[] header, List<string[]> rows,
				Dictionary<string, string[]> bareEnglishColumns)
			{
				Label = label;
				_header = header;
				Rows = rows;
				_bareEnglishColumns = bareEnglishColumns;
				_pkIndex = Array.FindIndex(header, h => h.Equals("PK", StringComparison.OrdinalIgnoreCase)
				                                        || h.Equals("pk", StringComparison.OrdinalIgnoreCase));
			}

			public string Label { get; }
			public string[] Header => _header;
			public List<string[]> Rows { get; }

			/// <summary>Localized columns of <paramref name="lang"/> (plus the named bare-English
			/// columns for en), <c>link_*</c> excluded; restricted to referenced stems once restricted.</summary>
			public IEnumerable<string> ProbedColumns(string lang)
			{
				var suffix = "_" + lang;
				foreach (var h in _header)
				{
					if (h.StartsWith("link", StringComparison.OrdinalIgnoreCase)) continue;
					if (h.EndsWith(suffix, StringComparison.Ordinal)) yield return h;
				}

				if (lang == "en"
				    && _bareEnglishColumns.TryGetValue(Label, out var bare)
				    && !_restricted)
				{
					foreach (var h in bare)
						if (Array.IndexOf(_header, h) >= 0)
							yield return h;
				}
			}

			public int NonEmptyCount(string column)
			{
				var idx = Array.IndexOf(_header, column);
				if (idx < 0) return 0;
				var count = 0;
				foreach (var row in Rows)
					if (!string.IsNullOrWhiteSpace(row[idx])) count++;
				return count;
			}

			public string PkOf(int rowIndex) =>
				_pkIndex >= 0 && rowIndex >= 0 && rowIndex < Rows.Count ? Rows[rowIndex][_pkIndex] : "";

			public bool SetCell(string pk, string column, string value)
			{
				var colIdx = Array.IndexOf(_header, column);
				if (colIdx < 0 || _pkIndex < 0) return false;
				for (var i = 0; i < Rows.Count; i++)
				{
					if (!string.Equals(Rows[i][_pkIndex], pk, StringComparison.Ordinal)) continue;
					Rows[i][colIdx] = value;
					return true;
				}
				return false;
			}

			private bool _restricted;

			/// <summary>Embedded-layer filter: keep only localized columns whose stem is referenced.</summary>
			public void RestrictColumnsToStems(HashSet<string> stems)
			{
				_restricted = true;
				_header = _header.Where(h =>
				{
					if (h.StartsWith("link", StringComparison.OrdinalIgnoreCase)) return false;
					var match = ProbedLanguages
						.Select(l => ("_" + l, l))
						.Concat(new[] { ("_fr", "fr") })
						.FirstOrDefault(t => h.EndsWith(t.Item1, StringComparison.Ordinal));
					if (match.Item1 == null) return false;
					var stem = h.Substring(0, h.Length - match.Item1.Length);
					return stems.Contains(stem);
				}).ToArray();
			}

			public CsvTable Clone() => new CsvTable(Label, (string[])_header.Clone(),
				Rows.Select(r => (string[])r.Clone()).ToList(), _bareEnglishColumns);
		}
	}
}
