using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.DatasetUpdater
{
	/// <summary>
	/// Guard for the T2 trial re-run (issue #457, review c.6098996342): the app33 translation
	/// prompt must carry a game glossary DERIVED from the cards CSV — never hand-copied — so the
	/// site translations reuse the exact game names the cards print. The trial of 10/10 renamed
	/// the flagship game in en/zh/fa (« school FOR liars », 骗子学校, مدرسه دروغ‌گوها) precisely
	/// because the prompt carried no glossary.
	///
	/// Contract (mirrors <c>PromptRulesGlossaryConsistencyTests</c> / #1076, two engines:
	/// <c>tools/1859-app33-glossary-regen.py</c> writes the block, this test keeps it honest):
	/// - the 5 game names are the <c>## </c> headings of rows Rules_01/_07/_09/_11/_13 of
	///   <c>Argumentum Rules - Cards.csv</c>, in all 8 language columns — EXACT (Ordinal) match,
	///   they are proper nouns;
	/// - the card term (« cartes ») must be attested in the Rules corpus of its language, and
	///   the un-attested rival « карточк » must stay absent (measured 10/10: карты stem ×231,
	///   карточк ×0 — the review's evidence, re-derived here on every run).
	///
	/// Fail-loud is the point: a corpus rename, a hand-edited glossary line or a lost block must
	/// turn the build red at the PR that introduces it. The block parses to zero lines when the
	/// sentinels are missing — the floor catches that instead of passing vacuously (#1048/#909).
	/// </summary>
	public class PromptDnnApp33GlossaryConsistencyTests
	{
		private const string PromptRelativePath =
			"Generation/Converters/Argumentum.AssetConverter/DatasetUpdater/Resources/PromptDnnApp33TranslateMultiUser.txt";

		private static readonly string[] RulesCsvRelativePaths =
		{
			Path.Combine("Cards", "Rules", "Argumentum Rules - Cards.csv"),
			Path.Combine("Cards", "Rules", "Argumentum Rules - Cards Print and Play.csv"),
		};

		/// <summary>The main cards CSV is the derivation source for game names (the review's referential).</summary>
		private const string GameNamesCsvPath = "Cards/Rules/Argumentum Rules - Cards.csv";

		private static readonly string[] GameNameRows = { "Rules_01", "Rules_07", "Rules_09", "Rules_11", "Rules_13" };

		private static readonly string[] LanguageSuffixes = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

		private static readonly string[] LatinLanguages = { "fr", "en", "pt", "es" };

		private const int MinGlossaryLines = 6;

		private const string BlockBegin = "<!-- GLOSSARY-APP33:BEGIN -->";
		private const string BlockEnd = "<!-- GLOSSARY-APP33:END -->";

		private static string RepoRoot() => TestRepoRoot.Find();

		[Fact]
		public void Glossary_GameNamesMatchRulesHeadingsVerbatim()
		{
			var glossary = ParseGlossary();
			glossary.Should().HaveCountGreaterThanOrEqualTo(MinGlossaryLines,
				"the locked app33 glossary block must exist between its sentinels and parse — " +
				"a missing block or silent empty parse would make this guard vacuous " +
				"(and is exactly the state the 10/10 trial shipped in: no glossary, renamed games)");

			var expected = DeriveGameNamesFromCardsCsv();
			var failures = new List<string>();
			foreach (var rowId in GameNameRows)
			{
				var expectedTerms = expected[rowId];
				var line = glossary.FirstOrDefault(g => string.Equals(g["fr"], expectedTerms["fr"], StringComparison.Ordinal));
				if (line == null)
				{
					failures.Add($"[{rowId}] no glossary line keyed by the FR game name '{expectedTerms["fr"]}'");
					continue;
				}

				foreach (var lang in LanguageSuffixes)
				{
					if (!string.Equals(line[lang], expectedTerms[lang], StringComparison.Ordinal))
					{
						failures.Add($"[{rowId} -> {lang}] glossary says '{line[lang]}' but the cards CSV " +
						             $"heading says '{expectedTerms[lang]}' — regenerate via " +
						             $"tools/1859-app33-glossary-regen.py, never hand-edit");
					}
				}
			}

			failures.Should().BeEmpty(
				"the 5 game names are the official names the cards print; the site must reuse them " +
				"word for word (review c.6098996342):\n{0}", string.Join("\n", failures));
		}

		[Fact]
		public void Glossary_CardTermAttestedAndRivalAbsentPerLanguage()
		{
			var glossary = ParseGlossary();
			glossary.Should().HaveCountGreaterThanOrEqualTo(MinGlossaryLines, "see the game-names fact");

			var cardLine = glossary.FirstOrDefault(g => string.Equals(g["fr"], "cartes", StringComparison.Ordinal));
			cardLine.Should().NotBeNull("the glossary must carry one 'cartes' line");

			var corpus = LoadRulesCorpusByLanguage();
			var failures = new List<string>();
			foreach (var lang in LanguageSuffixes)
			{
				if (!IsAttested(cardLine[lang], lang, corpus[lang]))
				{
					failures.Add($"[cartes -> {lang}] term '{cardLine[lang]}' not attested in the Rules corpus " +
					             $"(lemma level) — the corpus drifted from the glossary, or the line is a ghost");
				}
			}

			// The measured rival: the Russian rules never say « карточк » (stem ×0 vs карты ×231,
			// review c.6098996342). If it ever appears the derivation must be redone deliberately.
			if (corpus["ru"].Contains("карточк"))
			{
				failures.Add("[cartes -> ru] the rival 'карточк' is now attested in the corpus — " +
				             "re-derive the card term instead of keeping the locked карты");
			}

			failures.Should().BeEmpty(
				"every locked card term must be attested in the Rules corpus of its language:\n{0}",
				string.Join("\n", failures));
		}

		[Fact]
		public void Prompt_TargetsAllSevenLanguagesIncludingPt()
		{
			var prompt = File.ReadAllText(Path.Combine(RepoRoot(), PromptRelativePath));
			prompt.Should().Contain("`en`/`ru`/`pt`/`es`/`ar`/`fa`/`zh`",
				"#1855 (prompts pt-BR) is merged — the app33 pass targets all 7 languages, pt included; " +
				"the UpdateRecord field enumeration in rule 1 must not regress to the 6-language trial set");
			prompt.Should().NotContain("HORS périmètre de cette passe",
				"pt left the out-of-scope carve-out when #1855 (305a0ccb) merged — the old exclusion " +
				"sentence would tell the model to skip pt forever");
		}

		/// <summary>
		/// Parses the sentinel-delimited glossary block: lines of the form
		/// <c>- cartes (FR) = cards (EN) = … = 卡牌 (ZH)</c> into lang -> term maps.
		/// </summary>
		private static List<Dictionary<string, string>> ParseGlossary()
		{
			var path = Path.Combine(RepoRoot(), PromptRelativePath);
			File.Exists(path).Should().BeTrue("the app33 translation prompt must exist at '{0}'", path);

			var lines = File.ReadAllLines(path);
			var glossary = new List<Dictionary<string, string>>();
			var inBlock = false;
			foreach (var rawLine in lines)
			{
				var line = rawLine.Trim();
				if (line == BlockBegin) { inBlock = true; continue; }
				if (line == BlockEnd) { inBlock = false; continue; }
				if (!inBlock || !line.StartsWith("-") || !line.Contains("(FR) ="))
				{
					continue;
				}

				var terms = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				foreach (var part in line.TrimStart('-').Split('='))
				{
					var match = Regex.Match(part.Trim(), @"^(.+?)\s*\((FR|EN|RU|PT|ES|AR|FA|ZH)\)$");
					if (match.Success)
					{
						terms[match.Groups[2].Value.ToLowerInvariant()] = match.Groups[1].Value.Trim();
					}
				}

				if (terms.Count == LanguageSuffixes.Length)
				{
					glossary.Add(terms);
				}
			}

			return glossary;
		}

		/// <summary>
		/// Derives the 5 game names from the cards CSV: the first <c>## </c> markdown heading of
		/// each target row's 8 language cells. Missing row, column or heading = loud failure
		/// (the derivation source must be able to see every expected name).
		/// </summary>
		private static Dictionary<string, Dictionary<string, string>> DeriveGameNamesFromCardsCsv()
		{
			var columns = new Dictionary<string, string> { ["fr"] = "Text" };
			foreach (var lang in LanguageSuffixes.Where(l => l != "fr"))
			{
				columns[lang] = $"Text_{lang}";
			}

			var path = Path.Combine(RepoRoot(), GameNamesCsvPath);
			File.Exists(path).Should().BeTrue("the cards Rules CSV must exist at '{0}'", path);

			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};

			var result = new Dictionary<string, Dictionary<string, string>>();
			using (var reader = new StreamReader(path))
			using (var csv = new CsvReader(reader, config))
			{
				csv.Read();
				csv.ReadHeader();
				var headers = csv.HeaderRecord ?? Array.Empty<string>();
				var indexByLang = columns.ToDictionary(kv => kv.Key, kv => Array.IndexOf(headers, kv.Value));
				indexByLang.Values.Should().NotContain(-1,
					"the cards Rules CSV must expose all 8 language columns");

				while (csv.Read())
				{
					var pk = csv.GetField(0)?.Trim();
					if (pk == null || !GameNameRows.Contains(pk))
					{
						continue;
					}

					var names = new Dictionary<string, string>(StringComparer.Ordinal);
					foreach (var lang in LanguageSuffixes)
					{
						var cell = csv.GetField(indexByLang[lang]) ?? "";
						var match = Regex.Match(cell, @"^##\s*(.+?)\s*$", RegexOptions.Multiline);
						match.Success.Should().BeTrue(
							"row {0} must open its '{1}' cell with a '## ' game-name heading", pk, columns[lang]);
						names[lang] = match.Groups[1].Value;
					}

					result[pk] = names;
				}
			}

			result.Keys.Should().BeEquivalentTo(GameNameRows,
				"every game-name row ({0}) must be found in the cards CSV", string.Join(", ", GameNameRows));
			return result;
		}

		/// <summary>Concatenates the 8 Rules language columns across both Rules CSVs (quoted multi-line cells handled by CsvHelper).</summary>
		private static Dictionary<string, string> LoadRulesCorpusByLanguage()
		{
			var columns = new Dictionary<string, string> { ["fr"] = "Text" };
			foreach (var lang in LanguageSuffixes.Where(l => l != "fr"))
			{
				columns[lang] = $"Text_{lang}";
			}

			var builders = LanguageSuffixes.ToDictionary(l => l, _ => new StringBuilder());
			foreach (var relativePath in RulesCsvRelativePaths)
			{
				var path = Path.Combine(RepoRoot(), relativePath);
				File.Exists(path).Should().BeTrue("the Rules CSV must exist at '{0}'", relativePath);

				var config = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					MissingFieldFound = null,
					HeaderValidated = null,
				};
				using (var reader = new StreamReader(path))
				using (var csv = new CsvReader(reader, config))
				{
					csv.Read();
					csv.ReadHeader();
					var headers = csv.HeaderRecord ?? Array.Empty<string>();
					var indexByLang = columns.ToDictionary(
						kv => kv.Key,
						kv => Array.IndexOf(headers, kv.Value));

					indexByLang.Values.Should().NotContain(-1,
						"the Rules CSV '{0}' must expose all 8 language columns", relativePath);

					while (csv.Read())
					{
						foreach (var lang in LanguageSuffixes)
						{
							var cell = csv.GetField(indexByLang[lang]);
							if (!string.IsNullOrWhiteSpace(cell))
							{
								builders[lang].Append(cell).Append('\n');
							}
						}
					}
				}
			}

			return builders.ToDictionary(kv => kv.Key, kv => kv.Value.ToString());
		}

		/// <summary>Latin: word-boundary, case-insensitive, simple plural. ru: stem. ar/fa/zh: substring (no usable word boundaries).</summary>
		private static bool IsAttested(string term, string lang, string corpus)
		{
			if (LatinLanguages.Contains(lang))
			{
				var pattern = @"(?<!\w)" + Regex.Escape(term) + @"(?:s|es)?(?!\w)";
				return Regex.IsMatch(corpus, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
			}

			return lang == "ru"
				? Regex.IsMatch(corpus, Regex.Escape(term), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)
				: corpus.Contains(term);
		}
	}
}
