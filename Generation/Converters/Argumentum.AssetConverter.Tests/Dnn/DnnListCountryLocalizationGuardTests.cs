using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 9-bis — garde des packs List_Country ar-SA / fa-IR / zh-CN.
	/// Le neutre est une COQUILLE : 256 clés ISO alpha-2 dont les valeurs sont
	/// TOUTES vides (mesuré #2065) — le contenu vit dans les fichiers de culture
	/// (fr-FR/ru-RU/pt-BR/es-ES = témoins de la convention). Les trois packs
	/// nouveaux suivent le précédent ru (#1907, c2b1d849e) : 254/256 valeurs
	/// CLDR release-47 main/{ar,fa,zh}.xml + AN/CS codes ISO abrogés déclarés [K]
	/// (valeurs historiques établies). Source de vérité reproductible :
	/// docs/dnn-localization/458-item9bis-list-country-cldr-47.json + le
	/// constructeur byte-exact 458-item9bis-list-country-build.py.
	/// <list type="bullet">
	/// <item>release-47 ne shippe AUCUN main/{zh-CN,ar-SA,fa-IR}.xml (mesuré sur
	/// l'inventaire common/main, 1000 fichiers) — la locale de base est la
	/// résolution CLDR pour ces cultures.</item>
	/// <item>Contrôle de version : List_Country.ru-RU.resx committé reproduit
	/// cldr-ru.xml release-47 byte-exact sur les 254 clés CLDR.</item>
	/// <item>La garde « neutre vide jamais livré » de DnnAgrCoreResxIntegrityGuardTests
	/// ne s'applique PAS ici : elle protège les fichiers à neutre non vide. Pour
	/// List_Country, l'inverse est la convention — le neutre vide est le porte-clés.</item>
	/// </item>
	/// </list>
	/// </summary>
	public class DnnListCountryLocalizationGuardTests
	{
		private const string NeutralResx = "DNNPlatform/App_GlobalResources/List_Country.resx";
		private const string FrenchWitnessResx = "DNNPlatform/App_GlobalResources/List_Country.fr-FR.resx";

		private static readonly string[] Cultures = { "ar-SA", "fa-IR", "zh-CN" };

		/// <summary>Pins CLDR par culture : US/FR/DE/GB/ZZ (parité avec la garde ru)
		/// + IR/CN (pays des cultures fa-IR/zh-CN elles-mêmes) + AN/CS ([K] hors CLDR).</summary>
		private static readonly (string Culture, string Key, string Expected)[] Pins =
		{
			("ar-SA", "US.Text", "الولايات المتحدة"),
			("ar-SA", "FR.Text", "فرنسا"),
			("ar-SA", "DE.Text", "ألمانيا"),
			("ar-SA", "GB.Text", "المملكة المتحدة"),
			("ar-SA", "ZZ.Text", "منطقة غير معروفة"),
			("ar-SA", "IR.Text", "إيران"),
			("ar-SA", "CN.Text", "الصين"),
			("ar-SA", "AN.Text", "جزر الأنتيل الهولندية"), // [K] ISO abrogé 2010, absent CLDR
			("ar-SA", "CS.Text", "صربيا والجبل الأسود"),   // [K] ISO abrogé 2006, absent CLDR
			("fa-IR", "US.Text", "ایالات متحده"),
			("fa-IR", "FR.Text", "فرانسه"),
			("fa-IR", "DE.Text", "آلمان"),
			("fa-IR", "GB.Text", "بریتانیا"),
			("fa-IR", "ZZ.Text", "ناحیهٔ نامشخص"),
			("fa-IR", "IR.Text", "ایران"),
			("fa-IR", "CN.Text", "چین"),
			("fa-IR", "AN.Text", "آنتیل هلند"),              // [K]
			("fa-IR", "CS.Text", "صربستان و مونته‌نگرو"),    // [K] — ZWNJ dans مونته‌نگرو
			("zh-CN", "US.Text", "美国"),
			("zh-CN", "FR.Text", "法国"),
			("zh-CN", "DE.Text", "德国"),
			("zh-CN", "GB.Text", "英国"),
			("zh-CN", "ZZ.Text", "未知地区"),
			("zh-CN", "IR.Text", "伊朗"),
			("zh-CN", "CN.Text", "中国"),
			("zh-CN", "AN.Text", "荷属安的列斯"),            // [K]
			("zh-CN", "CS.Text", "塞尔维亚和黑山"),          // [K]
		};

		private static XDocument Load(string resxRelativePath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");
			return XDocument.Load(path);
		}

		private static List<(string Key, string Value)> Cells(string resxRelativePath) =>
			Load(resxRelativePath).Root!.Elements("data")
				.Select(d => (d.Attribute("name")!.Value, (string?)d.Element("value") ?? ""))
				.ToList();

		private static Dictionary<string, string> CellMap(string resxRelativePath) =>
			Cells(resxRelativePath).ToDictionary(kv => kv.Item1, kv => kv.Item2);

		/// <summary>La coquille : le neutre porte 256 codes ISO alpha-2, valeurs
		/// toutes vides. Ce fait FONDE les packs (le contenu vit en culture) :
		/// s'il change, la convention des 7 fichiers change avec lui.</summary>
		[Fact]
		public void NeutralIsTheEmptyShellOf256IsoCodes()
		{
			var cells = Cells(NeutralResx);
			cells.Should().HaveCount(256, "le neutre porte les 256 codes ISO alpha-2.");
			cells.Should().OnlyContain(kv => kv.Value.Length == 0,
				"la coquille : toutes les valeurs neutres sont vides (mesuré #2065) — " +
				"le contenu vit dans les fichiers de culture.");
			cells.Select(kv => kv.Key).Should().OnlyContain(k => k.EndsWith(".Text", StringComparison.Ordinal),
				"chaque clé est un code ISO alpha-2 suffixé .Text.");
		}

		[Fact]
		public void EachPackCoversExactlyTheNeutralKeySetInTheSameOrder()
		{
			var neutralKeys = Cells(NeutralResx).Select(kv => kv.Key).ToList();
			foreach (var culture in Cultures)
			{
				var cells = Cells($"DNNPlatform/App_GlobalResources/List_Country.{culture}.resx");
				cells.Should().HaveCount(256, "{0} : 256 codes, ni plus ni moins.", culture);
				cells.Select(kv => kv.Key).Should().Equal(neutralKeys,
					"{0} : jeu de clés == neutre, dans le MÊME ordre (le constructeur préserve " +
					"l'ordre ; un tri ou un réordonnancement casserait la reconstruction byte-exact).", culture);
			}
		}

		[Fact]
		public void NoPackDeliversAnEmptyOrWhitespaceValue()
		{
			foreach (var culture in Cultures)
			{
				var empty = Cells($"DNNPlatform/App_GlobalResources/List_Country.{culture}.resx")
					.Where(kv => string.IsNullOrWhiteSpace(kv.Value)).Select(kv => kv.Key).ToList();
				empty.Should().BeEmpty(
					"{0} : aucune valeur vide — la coquille neutre ne justifie AUCUN vide livré " +
					"(témoin fr-FR : 256/256 remplis).", culture);
			}
		}

		[Fact]
		public void EveryValueCarriesTheLanguageScript()
		{
			foreach (var culture in Cultures)
			{
				var cells = Cells($"DNNPlatform/App_GlobalResources/List_Country.{culture}.resx");
				var expectedScript = culture == "zh-CN" ? "CJK" : "écriture arabe/persane";
				cells.Should().OnlyContain(
					kv => culture == "zh-CN"
						? kv.Value.Any(c => c >= '一' && c <= '鿿')
						: kv.Value.Any(IsArabicBlock),
					"{0} : les 256 valeurs portent l'{1} (mesuré sur la source CLDR avant livraison).",
					culture, expectedScript);
			}
		}

		[Fact]
		public void NoValueRecyclesTheFrenchWitness()
		{
			// le défaut observable de « texte recopié » : contre une culture à écriture
			// disjointe, une valeur == fr-FR est forcément une recopie (mesuré : 0/256).
			var fr = CellMap(FrenchWitnessResx);
			foreach (var culture in Cultures)
			{
				var recycled = Cells($"DNNPlatform/App_GlobalResources/List_Country.{culture}.resx")
					.Where(kv => fr.TryGetValue(kv.Key, out var f) && f == kv.Value)
					.Select(kv => kv.Key).ToList();
				recycled.Should().BeEmpty("{0} : aucune valeur égale au témoin fr-FR.", culture);
			}
		}

		/// <summary>zh-CN est à écriture disjointe de ar-SA : aucune valeur ne peut
		/// légitimement coïncider (mesuré CLDR : 0/256). ar-SA ↔ fa-IR partagent
		/// l'écriture arabe et 9 translittérations légitimes (EG مصر, BT بوتان…) :
		/// l'identité n'y est PAS un défaut et n'est pas assertée.</summary>
		[Fact]
		public void ZhIsDisjointFromArabicScriptPacks()
		{
			var ar = CellMap("DNNPlatform/App_GlobalResources/List_Country.ar-SA.resx");
			var fa = CellMap("DNNPlatform/App_GlobalResources/List_Country.fa-IR.resx");
			var zh = CellMap("DNNPlatform/App_GlobalResources/List_Country.zh-CN.resx");
			var collisions = zh.Keys.Where(k => zh[k] == ar[k] || zh[k] == fa[k]).ToList();
			collisions.Should().BeEmpty(
				"zh partage son jeu de clés mais aucune valeur avec ar/fa (écritures disjointes).");
		}

		[Theory]
		[MemberData(nameof(PinRows))]
		public void PinnedCldrValues(string culture, string key, string expected)
		{
			var map = CellMap($"DNNPlatform/App_GlobalResources/List_Country.{culture}.resx");
			map.Should().ContainKey(key).WhoseValue.Should().Be(expected,
				"pin CLDR release-47 ({0}/{1}{2}).", culture, key,
				key.StartsWith("AN.", StringComparison.Ordinal) || key.StartsWith("CS.", StringComparison.Ordinal)
					? " — [K] code abrogé, valeur historique établie"
					: "");
		}

		public static IEnumerable<object[]> PinRows()
		{
			foreach (var (culture, key, expected) in Pins)
			{
				yield return new object[] { culture, key, expected };
			}
		}

		/// <summary>Invariant de transport, mesuré sur le <b>blob committé</b>, pas
		/// sur l'arbre de travail : le runner CI a core.autocrlf=true, qui convertit
		/// LF → CRLF au checkout (trou #2078, remède d4e6b345 — même instrument).
		/// Le constructeur préserve l'enveloppe du neutre octet par octet : BOM
		/// UTF-8, LF seul.</summary>
		[Fact]
		public void CommittedBlobsKeepBomAndLfEnvelope()
		{
			foreach (var culture in Cultures)
			{
				var raw = CommittedBlob($"DNNPlatform/App_GlobalResources/List_Country.{culture}.resx");
				raw.Length.Should().BeGreaterThan(5000, "{0} : le blob est le fichier entier, pas un fragment.", culture);
				raw.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "{0} : BOM UTF-8 (enveloppe du neutre).", culture);
				raw.Count(b => b == 0x0D).Should().Be(0, "{0} : LF seul (enveloppe du neutre).", culture);
			}
		}

		/// <summary>Lit le blob de <c>HEAD:&lt;path&gt;</c> — indépendant de
		/// core.autocrlf (instrument #2078 / d4e6b345).</summary>
		private static byte[] CommittedBlob(string rel)
		{
			var psi = new System.Diagnostics.ProcessStartInfo("git", $"cat-file -p \"HEAD:{rel}\"")
			{
				WorkingDirectory = TestRepoRoot.Find(),
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			};
			using var process = System.Diagnostics.Process.Start(psi)
				?? throw new InvalidOperationException("git n'a pas démarré");
			var stderr = process.StandardError.ReadToEndAsync();
			using var buffer = new MemoryStream();
			process.StandardOutput.BaseStream.CopyTo(buffer);
			process.WaitForExit(10_000);
			var err = stderr.Result;
			if (process.ExitCode != 0)
			{
				throw new InvalidOperationException(
					$"git cat-file a échoué ({process.ExitCode}) sur {rel} : {err}");
			}

			return buffer.ToArray();
		}

		private static bool IsArabicBlock(char c) =>
			(c >= '؀' && c <= 'ۿ') || (c >= 'ݐ' && c <= 'ݿ') ||
			(c >= 'ﭐ' && c <= '﷿') || (c >= 'ﹰ' && c <= '﻿');
	}
}
