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

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du canon des 4 cartes de téléchargement (pool #458 E1, décision D1-(A)
	/// c.6436567243) : Tarot / Poker × complet / léger, Title / Description / LinkText
	/// × 8 langues, écrits dans le canon avant toute injection. Pas de Link : l'URL de
	/// l'asset se dérive de la prérelease #2025 après publication.
	/// Les valeurs sont DÉRIVÉES au moment du test, jamais recopiées ici : le registre
	/// de la carte P&P existante (6A24AE92) pour la description, le libellé du lien et
	/// le nom « Print and Play », la liste Matériel des règles (Rules_02) pour le nom du
	/// paquet. Seules les deux formes de la variante (complète / allégée) sont épinglées.
	/// </summary>
	public class DnnDownloadCardsCanonGuardTests
	{
		private const string CanonCsv = "docs/dnn-localization/download-cards-canon.csv";
		private const string RegistryCsv = "docs/dnn-localization/eav-content.csv";
		private const string RulesCsv = "Cards/Rules/Argumentum Rules - Cards.csv";
		private const string PrintAndPlayCard = "6A24AE92-8177-44EF-841B-6A7C41937597";
		private static readonly string[] Languages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };
		private static readonly string[] Fields = { "Title", "Description", "LinkText" };

		private static readonly Dictionary<(string Deck, string Variant), string> Cards = new Dictionary<(string, string), string>
		{
			[("tarot", "full")] = "Tarot complet", [("tarot", "light")] = "Tarot léger",
			[("poker", "full")] = "Poker complet", [("poker", "light")] = "Poker léger",
		};

		// nom du paquet : présent tel quel dans la liste Matériel de Rules_02, et dans le titre
		private static readonly Dictionary<string, Dictionary<string, string>> DeckAnchor = new Dictionary<string, Dictionary<string, string>>
		{
			["tarot"] = new Dictionary<string, string>
			{
				["fr"] = "arguments fallacieux", ["en"] = "fallacy cards", ["ru"] = "с софизмами", ["pt"] = "argumentos falaciosos",
				["es"] = "argumento falaz", ["ar"] = "الحجج المغلوطة", ["fa"] = "استدلال‌های مغالطی", ["zh"] = "谬误论证牌",
			},
			["poker"] = new Dictionary<string, string>
			{
				["fr"] = "scénario", ["en"] = "scenario cards", ["ru"] = "со сценариями", ["pt"] = "cartas de cenário",
				["es"] = "cartas de escenario", ["ar"] = "بطاقات السيناريو", ["fa"] = "سناریو", ["zh"] = "情景卡牌",
			},
		};

		// la variante dans le titre (complète / allégée), forme du titre ; celle de la description est dans DescriptionSwap
		private static readonly Dictionary<string, (string Full, string Light)> TitleVariant = new Dictionary<string, (string, string)>
		{
			["fr"] = ("complète", "allégée"), ["en"] = ("complete", "streamlined"), ["ru"] = ("полная", "упрощённая"),
			["pt"] = ("completa", "simplificada"), ["es"] = ("completa", "simplificada"), ["ar"] = ("كاملة", "مبسطة"),
			["fa"] = ("کامل", "ساده‌شده"), ["zh"] = ("完整版", "精简版"),
		};

		// le cadre du titre : seuls mots admis hors nom du paquet, nom « Print and Play » et variante (aucun slogan, E1)
		private static readonly Dictionary<string, string[]> TitleFrame = new Dictionary<string, string[]>
		{
			["fr"] = new[] { "Cartes", "d", "version" }, ["en"] = new[] { "version" }, ["ru"] = new[] { "Карты", "версия" },
			["pt"] = new[] { "Cartas", "de", "versão" }, ["es"] = new[] { "Cartas", "de", "versión" }, ["ar"] = new[] { "بطاقات", "نسخة" },
			["fa"] = new[] { "کارت", "های", "نسخه" }, ["zh"] = Array.Empty<string>(),
		};

		// séparateurs du cadre : blanc, tiret long, apostrophe, ZWNJ et hamza suscrite de l'ezafe persan (ـهٔ)
		private static readonly Regex FrameToken = new Regex("[^\\s—’‌ٔ]+");

		private static readonly Dictionary<string, (string Light, string Full)> DescriptionSwap = new Dictionary<string, (string, string)>
		{
			["fr"] = ("une version allégée", "la version complète"), ["en"] = ("a streamlined version", "the complete version"),
			["ru"] = ("упрощённую версию", "полную версию"), ["pt"] = ("uma versão simplificada", "a versão completa"),
			["es"] = ("una versión simplificada", "la versión completa"), ["ar"] = ("نسخة مبسطة", "النسخة الكاملة"),
			["fa"] = ("نسخه‌ای ساده‌شده", "نسخهٔ کامل"), ["zh"] = ("精简版", "完整版"),
		};

		// toute forme d'adresse : schéma, www, chemin de portail DNN, fichier d'asset, release GitHub
		private static readonly string[] UrlShapes = { "://", @"\bwww\.", "/Portals/", @"\.(pdf|zip)\b", @"github\.com", "/releases/" };
		private static readonly Regex UrlLike = new Regex(string.Join("|", UrlShapes), RegexOptions.IgnoreCase);

		// une sonde par forme, qui ne porte QUE cette forme : une branche retirée du motif laisse sa sonde aveugle
		private static readonly string[] UrlProbes = { "ftp://host", "www.example.org", "/Portals/0/Images", "kit.pdf", "github.com/ArgumentumGames", "/releases/tag/v2" };

		private static string FullPath(string relative) => Path.Combine(TestRepoRoot.Find(), relative.Replace('/', Path.DirectorySeparatorChar));

		private static List<Dictionary<string, string>> ReadCsv(string relative, out string[] header)
		{
			var path = FullPath(relative);
			File.Exists(path).Should().BeTrue($"le fichier doit exister dans le checkout ({relative})");
			var config = new CsvConfiguration(CultureInfo.InvariantCulture) { MissingFieldFound = null, HeaderValidated = null };
			var rows = new List<Dictionary<string, string>>();
			using (var reader = new StreamReader(path))
			using (var csv = new CsvReader(reader, config))
			{
				csv.Read();
				csv.ReadHeader();
				header = csv.HeaderRecord ?? Array.Empty<string>();
				while (csv.Read())
				{
					var row = new Dictionary<string, string>(StringComparer.Ordinal);
					for (var i = 0; i < header.Length; i++) row[header[i]] = csv.GetField(i) ?? string.Empty;
					rows.Add(row);
				}
			}
			return rows;
		}

		private static List<Dictionary<string, string>> Canon() => ReadCsv(CanonCsv, out _);

		private static Dictionary<string, string> CanonRow(string key)
		{
			var matches = Canon().Where(r => r["key"] == key).ToList();
			matches.Should().HaveCount(1, $"la clé {key} doit figurer une fois dans le canon");
			return matches[0];
		}

		private static Dictionary<string, string> Registry(string field)
		{
			var key = $"{PrintAndPlayCard}|{field}";
			var matches = ReadCsv(RegistryCsv, out _).Where(r => r["key"] == key).ToList();
			matches.Should().HaveCount(1, $"le registre doit porter {key}");
			return matches[0];
		}

		private static string Rules02(string lang)
		{
			var column = lang == "fr" ? "Text" : $"Text_{lang}";
			var matches = ReadCsv(RulesCsv, out _).Where(r => r["pk"] == "Rules_02").ToList();
			matches.Should().HaveCount(1, "la règle Rules_02 (liste Matériel) doit exister");
			matches[0].Should().ContainKey(column);
			return matches[0][column];
		}

		private static IEnumerable<string> ExpectedKeys() =>
			from card in Cards.Keys from field in Fields select $"dl.{card.Deck}.{card.Variant}.{field}";

		public static IEnumerable<object[]> LanguageCases() => Languages.Select(l => new object[] { l });

		[Fact]
		public void Header_Is_The_E1_Columns()
		{
			ReadCsv(CanonCsv, out var header);
			header.Should().Equal(new[] { "key", "card", "field" }.Concat(Languages).Concat(new[] { "notes" }),
				"colonnes de la décision E1 : key,card,field, les 8 langues, notes — et aucune colonne Link");
		}

		[Fact]
		public void Keys_Are_Exactly_The_12_E1_Keys()
		{
			var rows = Canon();
			rows.Select(r => r["key"]).Should().HaveCount(12).And.OnlyHaveUniqueItems()
				.And.BeEquivalentTo(ExpectedKeys(), "4 cartes × Title / Description / LinkText, rien d'autre");
			foreach (var row in rows)
			{
				var parts = row["key"].Split('.');
				row["card"].Should().Be(Cards[(parts[1], parts[2])], $"{row["key"]} : la colonne card nomme la carte de la clé");
				row["field"].Should().Be(parts[3], $"{row["key"]} : la colonne field est le dernier segment de la clé");
			}
		}

		[Fact]
		public void No_Url_Anywhere_In_The_File()
		{
			// auto-contrôle : chaque forme est vue seule (une sonde vue par deux branches masquerait le retrait de l'une)
			foreach (var probe in UrlProbes)
			{
				UrlLike.IsMatch(probe).Should().BeTrue($"le motif doit voir « {probe} »");
				UrlShapes.Count(s => Regex.IsMatch(probe, s, RegexOptions.IgnoreCase)).Should().Be(1, $"« {probe} » ne porte qu'une forme");
			}
			UrlLike.IsMatch(Registry("Link")["fr"]).Should().BeTrue("le motif doit voir le Link de la carte P&P");

			var text = File.ReadAllText(FullPath(CanonCsv));
			UrlLike.Matches(text).Select(m => m.Value).Should().BeEmpty("le Link se remplit après la publication #2025, jamais dans le canon");
		}

		[Fact]
		public void Every_Language_Cell_Is_Filled_And_Trimmed()
		{
			var rows = Canon();
			rows.Should().HaveCount(12);
			foreach (var row in rows)
				foreach (var lang in Languages)
				{
					row[lang].Should().NotBeNullOrWhiteSpace($"{row["key"]} [{lang}]");
					row[lang].Should().Be(row[lang].Trim(), $"{row["key"]} [{lang}] : aucun blanc en bordure");
				}
		}

		[Theory]
		[MemberData(nameof(LanguageCases))]
		public void LinkText_Is_The_PrintAndPlay_Card_LinkText(string lang)
		{
			var want = Registry("LinkText")[lang];
			want.Should().NotBeNullOrWhiteSpace($"le registre porte le libellé du lien [{lang}]");
			foreach (var card in Cards.Keys)
				CanonRow($"dl.{card.Deck}.{card.Variant}.LinkText")[lang].Should().Be(want, $"{card} [{lang}] : libellé de la carte P&P");
		}

		[Theory]
		[MemberData(nameof(LanguageCases))]
		public void Light_Description_Is_The_PrintAndPlay_Card_Description(string lang)
		{
			var want = Registry("Description")[lang];
			want.Should().Contain(DescriptionSwap[lang].Light, $"la description P&P [{lang}] parle de la version allégée");
			foreach (var deck in new[] { "tarot", "poker" })
				CanonRow($"dl.{deck}.light.Description")[lang].Should().Be(want, $"{deck} léger [{lang}] : description de la carte P&P, inchangée");
		}

		[Theory]
		[MemberData(nameof(LanguageCases))]
		public void Full_Description_Swaps_Only_The_Variant(string lang)
		{
			var registry = Registry("Description")[lang];
			var (light, full) = DescriptionSwap[lang];
			Regex.Matches(registry, Regex.Escape(light)).Count.Should().Be(1, $"[{lang}] la variante allégée figure une fois dans la description P&P");
			var want = registry.Replace(light, full);
			want.Should().NotBe(registry);
			foreach (var deck in new[] { "tarot", "poker" })
				CanonRow($"dl.{deck}.full.Description")[lang].Should().Be(want, $"{deck} complet [{lang}] : description P&P, seule la variante change");
		}

		[Theory]
		[MemberData(nameof(LanguageCases))]
		public void Title_Names_Its_Deck_Its_Variant_And_Print_And_Play(string lang)
		{
			var rules = Rules02(lang);
			var ppName = Regex.Match(Registry("Title")[lang], "[\"«“].+?[\"»”]");
			ppName.Success.Should().BeTrue($"le titre P&P du registre [{lang}] cite le nom « Print and Play » entre guillemets");

			var titles = new List<string>();
			foreach (var card in Cards.Keys)
			{
				var title = CanonRow($"dl.{card.Deck}.{card.Variant}.Title")[lang];
				titles.Add(title);
				var other = card.Deck == "tarot" ? "poker" : "tarot";
				// à la casse près : le titre ouvre sur une capitale, la liste Matériel non
				rules.Should().ContainEquivalentOf(DeckAnchor[card.Deck][lang], $"[{lang}] le nom du paquet {card.Deck} vient de la liste Matériel des règles");
				title.Should().ContainEquivalentOf(DeckAnchor[card.Deck][lang], $"{card} [{lang}] : nomme son paquet")
					.And.NotContainEquivalentOf(DeckAnchor[other][lang], $"{card} [{lang}] : ne nomme pas l'autre paquet")
					.And.Contain(ppName.Value, $"{card} [{lang}] : nom « Print and Play » du registre");
				var (fullWord, lightWord) = TitleVariant[lang];
				var variantWord = card.Variant == "full" ? fullWord : lightWord;
				if (card.Variant == "full") title.Should().Contain(fullWord).And.NotContain(lightWord, $"{card} [{lang}] : version complète");
				else title.Should().Contain(lightWord).And.NotContain(fullWord, $"{card} [{lang}] : version allégée");

				var residue = Regex.Replace(title, Regex.Escape(DeckAnchor[card.Deck][lang]), " ", RegexOptions.IgnoreCase)
					.Replace(ppName.Value, " ").Replace(variantWord, " ");
				FrameToken.Matches(residue).Select(m => m.Value).Where(w => !TitleFrame[lang].Contains(w))
					.Should().BeEmpty($"{card} [{lang}] : rien d'autre que paquet, « Print and Play » et variante (« {title} »)");
			}
			titles.Should().OnlyHaveUniqueItems($"[{lang}] quatre titres distincts");
		}

		[Fact]
		public void Arabic_And_Persian_Letters_Are_Their_Own()
		{
			foreach (var row in Canon())
			{
				row["fa"].Should().NotContainAny(new[] { "ي", "ك", "ى" }, $"{row["key"]} fa : ی/ک persans, jamais ي/ك/ى arabes");
				row["ar"].Should().NotContainAny(new[] { "ی", "ک" }, $"{row["key"]} ar : ي/ك arabes, jamais ی/ک persans");
			}
		}
	}
}
