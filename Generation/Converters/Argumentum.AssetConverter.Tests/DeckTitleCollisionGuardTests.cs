using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde de collision des titres du deck (arbitrage ⑫/⑬/⑭, #458 c.5882003715) :
	/// dans chaque langue, deux cartes du deck ne portent jamais le même titre.
	/// Arrive avec ⑫w (zh, 3 collisions résolues : 55/956, 833/834, 1360/1398) ;
	/// ⑬w retire l'exception ar (667/846), ⑭w les deux exceptions fa (667/846,
	/// 784/834) : plus aucune exception, les 8 langues sont à 0. La garde échoue
	/// aussi si une exception redevient morte (la liste ne pourrit pas) ou si
	/// elle nomme une carte hors deck — mécanisme conservé pour les grains
	/// suivants et exercé par le contrôle inverse synthétique. Les titres sont
	/// rendus en tête de face (text_&lt;lang&gt;) : une collision se voit sur la
	/// carte imprimée, indistinguable pour le lecteur.
	/// </summary>
	public class DeckTitleCollisionGuardTests
	{
		private static string FallaciesCsv => System.IO.Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static readonly string[] Languages =
			{ "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

		/// <summary>Vérifie une langue : les paires déclarées doivent encore
		/// collisionner, et aucune autre. Retourne les anomalies (vide = vert).
		/// Factored pour que le contrôle inverse exerce CETTE logique.</summary>
		private static List<string> CheckLanguage(
			string lang,
			Dictionary<string, string> deckTitles,
			(string A, string B)[] expected)
		{
			var messages = new List<string>();
			foreach (var (a, b) in expected)
			{
				if (!deckTitles.TryGetValue(a, out var ta) || !deckTitles.TryGetValue(b, out var tb))
					messages.Add($"{lang}: l'exception {a}/{b} nomme une carte hors deck");
				else if (!string.Equals(ta, tb, StringComparison.Ordinal))
					messages.Add(
						$"{lang}: l'exception {a}/{b} ne collisionne plus ({ta} ≠ {tb}) — " +
						"retire-la de la liste");
			}

			var expectedPks = expected.SelectMany(e => new[] { e.A, e.B }).ToHashSet();
			var grouped = new Dictionary<string, List<string>>(StringComparer.Ordinal);
			foreach (var (pk, title) in deckTitles)
			{
				if (!grouped.TryGetValue(title, out var pks))
					grouped[title] = pks = new List<string>();
				pks.Add(pk);
			}
			foreach (var (title, pks) in grouped.Where(g => g.Value.Count > 1))
			{
				var extra = pks.Where(p => !expectedPks.Contains(p)).ToList();
				if (extra.Count > 0)
					messages.Add($"{lang}: collision non exemptée «{title}» : {string.Join("/", pks)}");
			}
			return messages;
		}

		private static Dictionary<string, string> LoadDeckTitles(string lang)
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			var all = csv.LoadColumn($"text_{lang}");
			var deck = csv.LoadColumn("carte");
			var byPk = csv.LoadColumn("PK");
			var map = new Dictionary<string, string>(StringComparer.Ordinal);
			for (var i = 0; i < all.Count; i++)
				if (!string.IsNullOrWhiteSpace(deck[i]))
					map[byPk[i].Trim()] = all[i].Trim();
			return map;
		}

		[Fact]
		public void Every_Language_Deck_Has_No_Title_Collision_But_Named_Exceptions()
		{
			var messages = new List<string>();
			foreach (var lang in Languages)
			{
				var titles = LoadDeckTitles(lang);
				titles.Should().HaveCount(175,
					$"{lang}: le deck compte 175 cartes (#1288) — un deck vide rendrait " +
					"cette garde vacue.");
				messages.AddRange(CheckLanguage(lang, titles, Array.Empty<(string, string)>()));
			}
			messages.Should().BeEmpty(
				"garde de collision #458 : les 8 langues à 0 (⑫w zh 3 paires, ⑬w ar 1, " +
				"⑭w fa 2 — aucune exception restante).");
		}

		[Fact]
		public void Checker_Fires_On_Injected_Collision_On_Dead_Exception_And_On_OffDeck_Exception()
		{
			var injected = new Dictionary<string, string>(StringComparer.Ordinal)
			{
				["1"] = "Sophisme", ["2"] = "Sophisme", ["3"] = "Unique",
			};
			CheckLanguage("zz", injected, Array.Empty<(string, string)>())
				.Should().ContainSingle(m => m.Contains("collision non exemptée «Sophisme» : 1/2"),
					"une collision injectée non exemptée est vue, avec ses PK");

			var deadException = new Dictionary<string, string>(StringComparer.Ordinal)
			{
				["667"] = "ابهام", ["846"] = "أخرى",
			};
			CheckLanguage("zz", deadException, new[] { ("667", "846") })
				.Should().ContainSingle(m => m.Contains("ne collisionne plus"),
					"une exception morte est signalée — la liste ne pourrit pas");

			var offDeck = new Dictionary<string, string>(StringComparer.Ordinal) { ["667"] = "ابهام" };
			CheckLanguage("zz", offDeck, new[] { ("667", "846") })
				.Should().ContainSingle(m => m.Contains("hors deck"),
					"une exception nommant une carte hors deck est signalée");
		}
	}
}