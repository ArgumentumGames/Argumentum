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
	/// Typography contract for the Persian (fa) rules prose (#1502, dispatched 11/10 c.6107217348,
	/// review c.6107203518): fa prose writes its numbers in PERSIAN digits (U+06F0..U+06F9) —
	/// measured on master: 86 Persian vs 27 Latin, the Latin ones all in Markdown titles or
	/// pictogram markers. This grain converts the 4 remaining Latin prose digits (Rules_05:
	/// « 3 یا 4 … 2 », « 1 امتیاز »; Rules_06: « در بازیِ 3 یا 4 نفره 2 کارت ») and pins the rule.
	///
	/// Exemptions (both measured conventions, not stylistic choices):
	/// <list type="bullet">
	/// <item><b>Markdown titles</b> — lines whose content starts with '#' (e.g. « ### 4. هیئت داوران »)
	/// keep Latin digits (they mirror the section numbering of every other language column);</item>
	/// <item><b>Pictogram markers</b> — parenthesized draw markers like « (❌🏆➜✅1🎴) » keep Latin
	/// digits: that is marker code shared byte-for-byte across the 8 columns, not prose.</item>
	/// </list>
	///
	/// The review also cited a parasitic space before « ، » in Rules_06 fa — measured byte-level on
	/// pre-merge and post-merge trees: no comma of the four target cells is preceded by any
	/// whitespace char (the citation elides the pictogram; the actual sequence is
	/// « برمی‌دارند (❌🏆➜✅1🎴)، یا », where the only space precedes the opening parenthesis, as in FR).
	/// Nothing to fix there; reported in the PR.
	///
	/// Falsifiability: red on the pre-grain tree (the 4 Latin prose digits trip fact 1; Rules_05
	/// has no Persian digit yet, tripping fact 3); a re-injected Latin prose digit trips fact 1.
	/// </summary>
	public class FaProseDigitTypographyTests
	{
		private static readonly string[] TargetFiles =
		{
			Path.Combine("Cards", "Rules", "Argumentum Rules - Cards.csv"),
			Path.Combine("Cards", "Rules", "Argumentum Rules - Cards Print and Play.csv"),
		};

		/// <summary>
		/// Pictographic / symbol test for marker spans: dingbat &amp; symbol blocks, plus any
		/// astral char (high surrogate = emoji blocks U+1F000+). Kept in code — .NET regex has
		/// no astral range syntax.
		/// </summary>
		private static bool ContainsPictograph(string span) => span.Any(IsPictographic);

		private static bool IsPictographic(char c) =>
			(c >= '☀' && c <= '➿') || (c >= '⬀' && c <= '⯿') || char.IsHighSurrogate(c);

		/// <summary>A parenthesized span that contains at least one pictographic char — a draw marker.</summary>
		private static readonly Regex MarkerParenthetical = new(
			@"\([^()]*\)",
			RegexOptions.Compiled);

		private static readonly Regex LatinDigits = new(@"[0-9]", RegexOptions.Compiled);
		private static readonly Regex PersianDigits = new(@"[۰-۹]", RegexOptions.Compiled);

		private static string RepoRoot() => TestRepoRoot.Find();

		private static List<(string file, string pk, string cell)> LoadFaCells()
		{
			var cells = new List<(string, string, string)>();
			foreach (var relativePath in TargetFiles)
			{
				var path = Path.Combine(RepoRoot(), relativePath);
				File.Exists(path).Should().BeTrue("the Rules CSV must exist at '{0}'", relativePath);

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
				var faIndex = Array.IndexOf(headers, "Text_fa");
				faIndex.Should().BeGreaterOrEqualTo(0, "'{0}' must expose the Text_fa column", relativePath);

				while (csv.Read())
				{
					cells.Add((relativePath, csv.GetField(0) ?? "", csv.GetField(faIndex) ?? ""));
				}
			}

			cells.Should().NotBeEmpty("both Rules CSVs must load rows");
			return cells;
		}

		/// <summary>
		/// The part of a cell the typography rule applies to: title lines dropped, marker
		/// parentheticals stripped. Marker stripping is gated on the span actually containing a
		/// pictographic char, so a plain parenthetical with a digit is still prose and trips.
		/// </summary>
		private static string ProseSkeleton(string cell)
		{
			var kept = new List<string>();
			foreach (var rawLine in cell.Split('\n'))
			{
				var line = rawLine.TrimEnd('\r');
				var content = line.TrimStart('*', ' ');
				if (content.StartsWith("#", StringComparison.Ordinal))
				{
					continue; // Markdown title — section numbering stays Latin by convention
				}

				var stripped = MarkerParenthetical.Replace(line, m =>
					ContainsPictograph(m.Value) ? "" : m.Value);
				kept.Add(stripped);
			}

			return string.Join("\n", kept);
		}

		[Fact]
		public void FaProseLatinDigits_NoneOutsideTitlesAndMarkers()
		{
			var failures = new List<string>();
			foreach (var (file, pk, cell) in LoadFaCells())
			{
				foreach (var rawLine in ProseSkeleton(cell).Split('\n'))
				{
					var offender = LatinDigits.Match(rawLine);
					if (offender.Success)
					{
						failures.Add($"[{Path.GetFileName(file)} / {pk}] Latin digit '{offender.Value}' in fa prose: " +
						             $"'{rawLine.TrimEnd('\r').Truncate(80)}…'");
					}
				}
			}

			failures.Should().BeEmpty(
				"fa prose numbers are written in Persian digits (U+06F0..U+06F9); Latin digits survive only in " +
				"Markdown titles (section numbering) and pictogram markers (shared marker code):\n{0}",
				string.Join("\n", failures));
		}

		[Fact]
		public void FaMarkerDigits_StayLatinInRules06()
		{
			// Witness that the marker exemption is exercised and its convention pinned: the draw
			// markers keep Latin digits (marker code shared across the 8 columns).
			foreach (var (file, pk, cell) in LoadFaCells().Where(c => c.pk.EndsWith("_06", StringComparison.Ordinal)))
			{
				cell.Should().Contain("➜✅+1🎴",
					"[{0}/{1}] the two '+1' draw markers keep their Latin digit (marker code, not prose)",
					Path.GetFileName(file), pk);
				Occurrences(cell, "➜✅+1🎴").Should().Be(2,
					"[{0}/{1}] exactly two '+1' draw lines (lot #1862)", Path.GetFileName(file), pk);
				cell.Should().Contain("✅1🎴", "the several-players marker digit stays Latin");
				cell.Should().Contain("❌0🎴", "the winner marker digit stays Latin");
			}
		}

		[Fact]
		public void FaPersianDigits_PresentInRules05()
		{
			// Positive witness of the grain: Rules_05 fa had ZERO Persian digit before (its 3, 4, 2, 1
			// were all Latin prose); after the grain its prose counts are Persian.
			foreach (var (file, pk, cell) in LoadFaCells().Where(c => c.pk.EndsWith("_05", StringComparison.Ordinal)))
			{
				PersianDigits.IsMatch(cell).Should().BeTrue(
					"[{0}/{1}] the converted counts (۳ یا ۴, ۲ کارت, ۱ امتیاز) must be Persian digits",
					Path.GetFileName(file), pk);
				cell.Should().Contain("۱ امتیاز", "the 1-point count is now Persian");
			}
		}

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
