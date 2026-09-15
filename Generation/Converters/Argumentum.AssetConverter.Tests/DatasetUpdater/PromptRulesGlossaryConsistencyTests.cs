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
	/// Guard for issue #1076: the locked Rules glossary embedded in
	/// <c>PromptRulesCascadeDriftUser.txt</c> is a hard-coded second referential alongside the
	/// CSV source. A line that locks terminology the corpus does not attest re-injects defects
	/// on the next CascadeDrift pass — either by overwriting a corrected term (the original
	/// <c>piocheur/comprador</c> class) or by introducing vocabulary the game never uses
	/// (the <c>défausse</c> ghost line, dropped by the 2026-08-19 corpus regeneration).
	///
	/// This test asserts glossary ↔ corpus agreement at the LEMMA level, mirroring
	/// <c>tools/1076-glossary-regen.py</c> (two engines, same contract):
	/// - fr/en/pt/es: word-boundary match, case-insensitive, simple plural inflection allowed;
	/// - ru: stem match (corpus uses declined forms, the glossary carries the nominative);
	/// - ar/fa/zh: substring semantics (no usable word boundaries at script frontiers).
	///
	/// Fail-loud is the point: a family rename, a drifted term or a new ghost line must turn
	/// the build red at the PR that introduces it, not surface as a silent re-injection one
	/// translation pass later.
	/// </summary>
	public class PromptRulesGlossaryConsistencyTests
	{
		private const string PromptRelativePath =
			"Generation/Converters/Argumentum.AssetConverter/DatasetUpdater/Resources/PromptRulesCascadeDriftUser.txt";

		private static readonly string[] RulesCsvRelativePaths =
		{
			Path.Combine("Cards", "Rules", "Argumentum Rules - Cards.csv"),
			Path.Combine("Cards", "Rules", "Argumentum Rules - Cards Print and Play.csv"),
		};

		private static readonly string[] LanguageSuffixes = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

		private static readonly string[] LatinLanguages = { "fr", "en", "pt", "es" };

		/// <summary>
		/// Minimum expected glossary lines. The 2026-08-19 regeneration kept 10 (11 minus the
		/// <c>défausse</c> ghost). A count below this means the glossary block moved, shrank or
		/// was reformatted — which must fail loud rather than let the attestation pass vacuously
		/// over zero lines (the #1048/#909 class: an instrument that cannot see cannot fail).
		/// </summary>
		private const int MinGlossaryLines = 10;

		private static string RepoRoot() => TestRepoRoot.Find();

		[Fact]
		public void Glossary_EveryTermIsAttestedInTheRulesCorpus()
		{
			var glossary = ParseGlossary();
			glossary.Should().HaveCountGreaterThanOrEqualTo(MinGlossaryLines,
				"the locked glossary block must exist and be parsed — a silent empty parse would make this guard vacuous");

			var corpus = LoadCorpusByLanguage();

			var failures = new List<string>();
			foreach (var terms in glossary)
			{
				foreach (var lang in LanguageSuffixes)
				{
					if (!terms.TryGetValue(lang, out var term) || string.IsNullOrWhiteSpace(term))
					{
						failures.Add($"[{terms.GetValueOrDefault("fr")}] lang '{lang}' missing/empty in glossary line");
						continue;
					}
					if (!IsAttested(term, lang, corpus[lang]))
					{
						failures.Add($"[{terms["fr"]} -> {lang}] term '{term}' not attested in the Rules corpus " +
							$"(lemma level). Either the corpus drifted from the glossary or the line is a ghost " +
							$"— regenerate via tools/1076-glossary-regen.py, do not hand-edit.");
					}
				}
			}

			failures.Should().BeEmpty(
				"every locked glossary term must be attested in the Rules CSV of its language; " +
				"a glossary the corpus contradicts is the re-injection channel issue #1076 closed:\n{0}",
				string.Join("\n", failures));
		}

		/// <summary>
		/// Parses lines of the form <c>- piocheur (FR) = Reader (EN) = ... = 抽牌者 (ZH)</c>
		/// into lang -> term maps. Non-glossary lines are skipped.
		/// </summary>
		private static List<Dictionary<string, string>> ParseGlossary()
		{
			var path = Path.Combine(RepoRoot(), PromptRelativePath);
			File.Exists(path).Should().BeTrue("the CascadeDrift Rules prompt must exist at '{0}'", path);

			var glossary = new List<Dictionary<string, string>>();
			foreach (var rawLine in File.ReadAllLines(path))
			{
				var line = rawLine.Trim();
				if (!line.StartsWith("-") || !line.Contains("(FR) ="))
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
		/// Concatenates every non-blank cell of the 8 Rules language columns across both Rules
		/// CSVs (CsvHelper handles the quoted multi-line cells).
		/// </summary>
		private static Dictionary<string, string> LoadCorpusByLanguage()
		{
			var columns = new Dictionary<string, string>
			{
				["fr"] = "Text",
			};
			foreach (var lang in LanguageSuffixes.Where(l => l != "fr"))
			{
				columns[lang] = $"Text_{lang}";
			}

			var builders = LanguageSuffixes.ToDictionary(l => l, _ => new StringBuilder());
			foreach (var relativePath in RulesCsvRelativePaths)
			{
				var path = Path.Combine(RepoRoot(), relativePath);
				File.Exists(path).Should().BeTrue("the Rules CSV must exist at '{0}'", path);

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

		internal static bool IsAttested(string term, string lang, string corpus)
		{
			if (LatinLanguages.Contains(lang))
			{
				var pattern = @"(?<!\w)" + Regex.Escape(term) + @"(?:s|es)?(?!\w)";
				return Regex.IsMatch(corpus, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
			}

			if (lang == "ru")
			{
				if (corpus.Contains(term))
				{
					return true;
				}
				var stem = term.Length > 3 ? term[..^1] : term;
				return corpus.Contains(stem);
			}

			// ar/fa/zh — substring of the term, its longest word, or that word minus a short
			// agglutination/morphology tail.
			if (corpus.Contains(term))
			{
				return true;
			}
			var longestWord = term.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
				.OrderByDescending(w => w.Length)
				.FirstOrDefault() ?? term;
			return corpus.Contains(longestWord) ||
			       (longestWord.Length > 4 && corpus.Contains(longestWord[..^2]));
		}
	}
}
