using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde des collisions inverses (grain 3 de #458, miroir de 2b) : deux
	/// concepts francais DISTINCTS ne portent pas le meme titre dans une langue
	/// (text_&lt;lang&gt; identique a la casse pres, text_fr differents). Le grain
	/// reecrit 13 cellules fautives hors deck (32 es, 829 es/fa/zh/pt, 704 es,
	/// 1372 ru, 309 fa, 1243 ar, 298 ar, 422 zh, 944 pt, 241 zh), puis 9 de plus
	/// en 3c (390 es/ar/fa/zh requalifiees par ai-01 01/10, 425 zh, 1403 es,
	/// 1295 zh, ru 26/188 agrammaticaux) : 0 cellule du deck, 0 colonne en
	/// touchee donc 0 IRI OWL. Les collisions restantes sont des exclusions
	/// nominatives par (PK, langue), en trois familles :
	/// (ii) polyhierarchie -- meme concept dans deux branches, decide ai-01
	/// 01/10 : pas de fusion, le meme titre traduit y est attendu ; (iii) la
	/// langue n'a qu'un mot pour deux nuances, terme distinct non etabli
	/// (verifie par recherche) ; deck : traduction imprimee v3, gardee. La garde
	/// echoue aussi si une exclusion devient morte (la liste ne pourrit pas).
	/// </summary>
	public class InverseTitleCollisionGuardTests
	{
		private static string FallaciesCsv => System.IO.Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static readonly string[] Languages = { "en", "ru", "pt", "es", "ar", "fa", "zh" };

		private static readonly HashSet<(int Pk, string Lang)> Excluded = new HashSet<(int, string)>
		{
            // (ii) polyhierarchie -- meme concept dans deux branches, decide ai-01 01/10 : pas de fusion, le meme titre traduit y est attendu.
            // en: 64/75/106/206/301/307/460/476/477/654/695/717/764/821/915/988/1042/1187/1239/1271/1300/1304/1348
            (64, "en"),
            (75, "en"),
            (106, "en"),
            (206, "en"),
            (301, "en"),
            (307, "en"),
            (460, "en"),
            (476, "en"),
            (477, "en"),
            (654, "en"),
            (695, "en"),
            (717, "en"),
            (764, "en"),
            (821, "en"),
            (915, "en"),
            (988, "en"),
            (1042, "en"),
            (1187, "en"),
            (1239, "en"),
            (1271, "en"),
            (1300, "en"),
            (1304, "en"),
            (1348, "en"),
            // ru: 26/75/188/301/460/476/695/915/988/1271/1300/1348
            (26, "ru"),
            (75, "ru"),
            (188, "ru"),
            (301, "ru"),
            (460, "ru"),
            (476, "ru"),
            (695, "ru"),
            (915, "ru"),
            (988, "ru"),
            (1271, "ru"),
            (1300, "ru"),
            (1348, "ru"),
            // pt: 26/75/188/301/559/570/695/821/1271/1304
            (26, "pt"),
            (75, "pt"),
            (188, "pt"),
            (301, "pt"),
            (559, "pt"),
            (570, "pt"),
            (695, "pt"),
            (821, "pt"),
            (1271, "pt"),
            (1304, "pt"),
            // es: 26/75/188/301/460/476/654/695/717/821/915/1271/1304/1366
            (26, "es"),
            (75, "es"),
            (188, "es"),
            (301, "es"),
            (460, "es"),
            (476, "es"),
            (654, "es"),
            (695, "es"),
            (717, "es"),
            (821, "es"),
            (915, "es"),
            (1271, "es"),
            (1304, "es"),
            (1366, "es"),
            // ar: 75/295/301/595/654/717/866/988/1123/1348
            (75, "ar"),
            (295, "ar"),
            (301, "ar"),
            (595, "ar"),
            (654, "ar"),
            (717, "ar"),
            (866, "ar"),
            (988, "ar"),
            (1123, "ar"),
            (1348, "ar"),
            // fa: 26/53/188/695/1119/1271
            (26, "fa"),
            (53, "fa"),
            (188, "fa"),
            (695, "fa"),
            (1119, "fa"),
            (1271, "fa"),
            // zh: 654/695/717/1042/1166/1239/1271/1344
            (654, "zh"),
            (695, "zh"),
            (717, "zh"),
            (1042, "zh"),
            (1166, "zh"),
            (1239, "zh"),
            (1271, "zh"),
            (1344, "zh"),

            // (iii) un seul mot pour deux nuances -- terme distinct non etabli (verifie par recherche, cf. PR du grain) ; re-examinable si un terme s'impose.
            // ru: 238/243
            (238, "ru"),
            (243, "ru"),
            // es: 890/1284
            (890, "es"),
            (1284, "es"),
            // ar: 43/46/172/238/251/286/290/296/422/510/618/642/758/765/773/805/906/913/916/931/938/952/1085/1278/1401
            (43, "ar"),
            (46, "ar"),
            (172, "ar"),
            (238, "ar"),
            (251, "ar"),
            (286, "ar"),
            (290, "ar"),
            (296, "ar"),
            (422, "ar"),
            (510, "ar"),
            (618, "ar"),
            (642, "ar"),
            (758, "ar"),
            (765, "ar"),
            (773, "ar"),
            (805, "ar"),
            (906, "ar"),
            (913, "ar"),
            (916, "ar"),
            (931, "ar"),
            (938, "ar"),
            (952, "ar"),
            (1085, "ar"),
            (1278, "ar"),
            (1401, "ar"),
            // fa: 84/219/231/232/238/255/259/275/294/312/313/325/469/510/758/777/791/793/805/913/916/1263/1367/1370
            (84, "fa"),
            (219, "fa"),
            (231, "fa"),
            (232, "fa"),
            (238, "fa"),
            (255, "fa"),
            (259, "fa"),
            (275, "fa"),
            (294, "fa"),
            (312, "fa"),
            (313, "fa"),
            (325, "fa"),
            (469, "fa"),
            (510, "fa"),
            (758, "fa"),
            (777, "fa"),
            (791, "fa"),
            (793, "fa"),
            (805, "fa"),
            (913, "fa"),
            (916, "fa"),
            (1263, "fa"),
            (1367, "fa"),
            (1370, "fa"),
            // zh: 227/228/229/253/262/263/275/278/294/667/820/913/916/1006
            (227, "zh"),
            (228, "zh"),
            (229, "zh"),
            (253, "zh"),
            (262, "zh"),
            (263, "zh"),
            (275, "zh"),
            (278, "zh"),
            (294, "zh"),
            (667, "zh"),
            (820, "zh"),
            (913, "zh"),
            (916, "zh"),
            (1006, "zh"),

            // deck : traduction imprimee v3, gardee (1174 ru : le Tarot v3 imprime Etnotsentrizm -- c'est le titre francais « Biais culturels » qui a change depuis ; decide ai-01 01/10).
            // ru: 1174/1202
            (1174, "ru"),
            (1202, "ru"),
		};

		/// <summary>NFKD + casse + trim -- la meme cle que l'instrument de mesure du grain.</summary>
		private static string Norm(string s) =>
			(s ?? "").Normalize(NormalizationForm.FormKD).ToLowerInvariant().Trim();

		/// <summary>Verifie une langue : un titre partage par des text_fr
		/// distincts doit etre integralement exempte, et une exemption doit
		/// encore collisionner. Retourne les anomalies (vide = vert).
		/// Factored pour que le controle inverse exerce CETTE logique.</summary>
		private static List<string> CheckRows(
			string lang,
			IReadOnlyList<(int Pk, string Fr, string Title)> rows,
			IEnumerable<(int Pk, string Lang)> excluded)
		{
			var messages = new List<string>();
			var excl = excluded.Where(e => e.Lang == lang).Select(e => e.Pk).ToHashSet();
			var used = new HashSet<int>();
			foreach (var g in rows.GroupBy(r => Norm(r.Title), StringComparer.Ordinal))
			{
				var members = g.ToList();
				if (members.Count < 2) continue;
				if (members.Select(m => Norm(m.Fr)).Distinct().Count() < 2) continue;
				foreach (var m in members) used.Add(m.Pk);
				var uncovered = members.Where(m => !excl.Contains(m.Pk)).ToList();
				if (uncovered.Count > 0)
					messages.Add(
						$"{lang}: «{g.Key}» partage par des concepts francais distincts " +
						$"({string.Join("/", members.Select(m => m.Pk))}) -- non exemptes : " +
						$"{string.Join("/", uncovered.Select(m => m.Pk))}");
			}
			foreach (var dead in excl.Where(p => !used.Contains(p)))
				messages.Add($"{lang}: l'exclusion PK {dead} ne collisionne plus -- retire-la");
			return messages;
		}

		private static List<(int Pk, string Fr, string Title)> LoadRows(string lang)
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			var titles = csv.LoadColumn($"text_{lang}");
			var fr = csv.LoadColumn("text_fr");
			var pks = csv.LoadColumn("PK");
			var rows = new List<(int, string, string)>();
			for (var i = 0; i < titles.Count; i++)
				if (int.TryParse(pks[i].Trim(), out var pk) && !string.IsNullOrWhiteSpace(titles[i]))
					rows.Add((pk, fr[i], titles[i]));
			return rows;
		}

		[Fact]
		public void Every_Language_Separates_Distinct_French_Concepts_But_Named_Exclusions()
		{
			var messages = new List<string>();
			foreach (var lang in Languages)
			{
				var rows = LoadRows(lang);
				rows.Should().HaveCountGreaterThan(1300,
					$"{lang}: colonne quasi vide -- la garde serait vacue.");
				messages.AddRange(CheckRows(lang, rows, Excluded));
			}
			messages.Should().BeEmpty(
				"garde des collisions inverses #458 grain 3+3c : 22 cellules corrigees, " +
				"les restes sont des exclusions nominatives (polyhierarchie, " +
				"un-mot-deux-nuances, deck imprime v3).");
		}

		[Fact]
		public void Checker_Fires_On_Injected_Collision_And_On_Dead_Exclusion()
		{
			var rows = new List<(int Pk, string Fr, string Title)>
			{
				(1, "Concept un", "Titre"), (2, "Concept deux", "Titre"), (3, "Concept trois", "Unique"),
			};
			CheckRows("zz", rows, Array.Empty<(int, string)>())
				.Should().ContainSingle(m => m.Contains("non exemptes : 1/2"),
					"une collision injectee non exemptee est vue, avec ses PK");

			CheckRows("zz", rows, new[] { (1, "zz"), (2, "zz") })
				.Should().BeEmpty("un groupe integralement exempte est silencieux");

			CheckRows("zz", rows, new[] { (1, "zz"), (2, "zz"), (99, "zz") })
				.Should().ContainSingle(m => m.Contains("l'exclusion PK 99 ne collisionne plus"),
					"une exclusion morte est signalee -- la liste ne pourrit pas");
		}
	}
}
