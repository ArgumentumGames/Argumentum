using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.DatasetUpdater
{
	/// <summary>
	/// Contract for the owner's 10/10 lot (#1502 c.6098939191, GO c.6098942133): the three rule
	/// changes decided interactively must hold on <c>Rules_06</c> (cards CSV) AND <c>RulesPP_06</c>
	/// (print &amp; play CSV), in ALL 8 language columns — the two files print the same rule text.
	///
	/// The three changes:
	/// 1. <b>1a</b> — several-cards variant, new winner rule: winners = cards the baratineur
	///    PLAYED (was: cards with the most votes), declared in vote order;
	/// 2. <b>2a</b> — jury-unanimity variant now applies « À 5 joueurs et plus » (Dixit variant);
	/// 3. <b>the two « + »</b> — draw lines for voted jurors / jurors who found the card now read
	///    <c>➜✅+1🎴</c> (Russian already conformed before the lot: measured 10/11).
	///
	/// Pinning strategy: the FR target sentences are the owner's decision text (verbatim);
	/// for translated columns we pin (a) absence of the OLD pre-lot fragments (each encodes the
	/// superseded winner rule), (b) presence of the player-count condition « 5 » in the unanimity
	/// bullet, (c) the emoji marker forms, (d) structural integrity (12 bullets per cell,
	/// identical cells across both files). Hand-copied expected translations are deliberately
	/// NOT pinned — translations are produced by the DatasetUpdater pass and reviewed cell by
	/// cell (relecture ai-01), the guard pins the LOT, not the prose.
	///
	/// Falsifiability: the guard is RED on pre-lot master (all three changes absent — that is the
	/// commit's inverse control) and stays green only while every column keeps the changes.
	/// </summary>
	public class Rules06OwnerLotContractTests
	{
		private static readonly (string File, string Pk)[] Targets =
		{
			(Path.Combine("Cards", "Rules", "Argumentum Rules - Cards.csv"), "Rules_06"),
			(Path.Combine("Cards", "Rules", "Argumentum Rules - Cards Print and Play.csv"), "RulesPP_06"),
		};

		private static readonly string[] LanguageSuffixes = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

		private static string ColumnFor(string lang) => lang == "fr" ? "Text" : $"Text_{lang}";

		/// <summary>Owner's decision text, c.6098939191 — the FR source of the lot.</summary>
		private const string NewFrSeveralCards =
			"Le baratineur peut poser plusieurs cartes. Autant de vainqueurs que le baratineur a posé " +
			"de cartes sont déclarés, dans l’ordre du nombre de votes ; il peut ainsi remporter tout " +
			"ou partie de ses cartes.";

		private const string NewFrUnanimityPrefix = "À 5 joueurs et plus, si la carte du baratineur est désignée";

		/// <summary>The superseded FR fragment (old winner rule) — must be gone from FR.</summary>
		private const string OldFrSeveralCardsFragment = "qu’il y a de cartes ayant obtenu le plus grand nombre de votes";

		/// <summary>Pre-lot translated fragments of the OLD winner rule — must be gone from every column.</summary>
		private static readonly Dictionary<string, string> OldSeveralCardsFragments = new(StringComparer.Ordinal)
		{
			["fr"] = OldFrSeveralCardsFragment,
			["en"] = "As many winners as there are cards with the greatest number of votes",
			["ru"] = "сколько карт набрали наибольшее число голосов",
			["pt"] = "quantas as cartas que tenham obtido o maior número de votos",
			["es"] = "como cartas hayan obtenido el mayor número de votos",
			["ar"] = "بعدد البطاقات التي حصلت على أكبر عدد من الأصوات",
			["fa"] = "به ازای هر کارتی که بیشترین تعداد رأی",
			["zh"] = "获得最高票数的牌有几张，就宣布几名获胜者",
		};

		/// <summary>
		/// Bullet positions in the rule cell (stable order defined by the FR source): 6 draw lines
		/// then 6 variant bullets — index 6 = jury-unanimity (2a), index 10 = several-cards (1a).
		/// Positional identification keeps the guard free of translated literals for LOCATING the
		/// bullets; the semantic pins are the absence fragments and the player-count condition.
		/// </summary>
		private const int UnanimityBulletIndex = 6;
		private const int SeveralCardsBulletIndex = 10;

		private const string DrawMarkerBroken = "➜✅ 1🎴"; // space before 1 — pre-lot form on the two "+1" draw lines
		private const string DrawMarkerFixed = "➜✅+1🎴";  // target form (ru already conformed)

		private static string RepoRoot() => TestRepoRoot.Find();

		private static Dictionary<(string file, string pk), Dictionary<string, string>> LoadCells()
		{
			var cells = new Dictionary<(string, string), Dictionary<string, string>>();
			foreach (var (relativePath, pk) in Targets)
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
				var indexByLang = LanguageSuffixes.ToDictionary(l => l, l => Array.IndexOf(headers, ColumnFor(l)));
				indexByLang.Values.Should().NotContain(-1,
					"'{0}' must expose all 8 language columns", relativePath);

				var found = false;
				while (csv.Read())
				{
					if ((csv.GetField(0) ?? "").Trim() != pk)
					{
						continue;
					}

					found = true;
					var byLang = LanguageSuffixes.ToDictionary(
						l => l, l => csv.GetField(indexByLang[l]) ?? "");
					cells[(relativePath, pk)] = byLang;
					break;
				}

				found.Should().BeTrue("row {0} must exist in {1}", pk, Path.GetFileName(relativePath));
			}

			return cells;
		}

		private static IEnumerable<string> BulletLines(string cell) =>
			cell.Split('\n').Select(l => l.TrimEnd('\r')).Where(l => l.StartsWith("* "));

		[Fact]
		public void OwnerLot_PlusMarkers_AllColumns()
		{
			var cells = LoadCells();
			var failures = new List<string>();
			foreach (var ((file, pk), byLang) in cells)
			{
				foreach (var lang in LanguageSuffixes)
				{
					var cell = byLang[lang];
					var broken = CountOccurrences(cell, DrawMarkerBroken);
					var fixedCount = CountOccurrences(cell, DrawMarkerFixed);
					if (broken != 0 || fixedCount != 2)
					{
						failures.Add($"[{Path.GetFileName(file)} / {pk} / {lang}] " +
						             $"'{DrawMarkerBroken}' x{broken} (want 0), '{DrawMarkerFixed}' x{fixedCount} (want 2)");
					}
				}
			}

			failures.Should().BeEmpty(
				"the two '+' draw lines must read '{0}' in every column (ru conformed before the lot; " +
				"the no-space form on the 'everyone draws' line is a different marker and stays):\n{1}",
				DrawMarkerFixed, string.Join("\n", failures));
		}

		[Fact]
		public void OwnerLot_FrSource_MatchesOwnerDecision()
		{
			var cells = LoadCells();
			foreach (var ((file, pk), byLang) in cells)
			{
				var fr = byLang["fr"];
				fr.Should().Contain(NewFrSeveralCards,
					"[{0}/{1}] the 1a sentence is the owner's decision text (c.6098939191)", file, pk);
				fr.Should().Contain(NewFrUnanimityPrefix,
					"[{0}/{1}] the unanimity variant carries the 'À 5 joueurs et plus' condition (2a)", file, pk);
				fr.Should().NotContain(OldFrSeveralCardsFragment,
					"[{0}/{1}] the superseded winner rule is gone", file, pk);
			}
		}

		[Fact]
		public void OwnerLot_TranslatedColumns_OldWinnerRuleGone()
		{
			var cells = LoadCells();
			var failures = new List<string>();
			foreach (var ((file, pk), byLang) in cells)
			{
				foreach (var lang in LanguageSuffixes)
				{
					if (byLang[lang].Contains(OldSeveralCardsFragments[lang]))
					{
						failures.Add($"[{Path.GetFileName(file)} / {pk} / {lang}] still carries the old 1a fragment: " +
						             $"'{OldSeveralCardsFragments[lang].Truncate(40)}…'");
					}
				}
			}

			failures.Should().BeEmpty(
				"every translated several-cards bullet must reflect the NEW winner rule (winners = cards " +
				"played, in vote order), so the pre-lot fragment encoding the old rule must be gone:\n{0}",
				string.Join("\n", failures));
		}

		[Fact]
		public void OwnerLot_UnanimityCarriesPlayerCount_AllColumns()
		{
			var cells = LoadCells();
			var failures = new List<string>();
			foreach (var ((file, pk), byLang) in cells)
			{
				foreach (var lang in LanguageSuffixes)
				{
					var bullets = BulletLines(byLang[lang]).ToList();
					bullets.Should().HaveCount(12,
						"[{0}/{1}/{2}] 6 draw lines + 6 variant bullets — positional lookup needs the full structure",
						Path.GetFileName(file), pk, lang);
					var bullet = bullets[UnanimityBulletIndex];
					if (!bullet.Contains("5"))
					{
						failures.Add($"[{Path.GetFileName(file)} / {pk} / {lang}] unanimity bullet has no player-count " +
						             $"condition (expected the '5 players and more' condition from 2a): '{bullet.Truncate(60)}…'");
					}
				}
			}

			failures.Should().BeEmpty(
				"the Dixit-variant condition 'À 5 joueurs et plus' (2a) must appear in every language:\n{0}",
				string.Join("\n", failures));
		}

		[Fact]
		public void OwnerLot_StructureAndFileParity_Intact()
		{
			var cells = LoadCells();
			var first = cells[Targets[0]];
			foreach (var lang in LanguageSuffixes)
			{
				var cards = first[lang];
				cards.Split('\n').Select(l => l.TrimEnd('\r')).Count(l => l.StartsWith("* "))
					.Should().Be(12, "[Cards/{0}] the rule keeps its 6 draw lines + 6 variant bullets", lang);
			}

			var pp = cells[Targets[1]];
			foreach (var lang in LanguageSuffixes)
			{
				first[lang].Should().Be(pp[lang],
					"[{0}] cards and print & play print the same rule text — the lot grafts identical content in both files", lang);
			}
		}

		private static int CountOccurrences(string haystack, string needle)
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

	internal static class StringLotExtensions
	{
		public static string Truncate(this string value, int max) =>
			value.Length <= max ? value : value.Substring(0, max);
	}
}
