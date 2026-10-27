using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du grain juridique zh-CN / ar-SA / fa-IR (pool #458 item 5, décision 2
	/// c.6405473398) : les trois cultures n'avaient aucun GlobalResources, /terms et
	/// /privacy y rendaient le texte générique du neutre. Source = le fr-FR vivant
	/// (mentions LCEN, SAS, SIREN), traduit unité par unité ; la version française fait
	/// foi et une phrase d'en-tête le dit dans chaque langue.
	/// Toutes les attentes sont DÉRIVÉES du fr-FR au moment du test (séquence de
	/// balises, jetons portail, épingles d'identité, multiset de chiffres) : la
	/// couverture de cette garde est la fidélité mécanique au fr, pas un golden.
	/// </summary>
	public class DnnLegalZhArFaGuardTests
	{
		private const string FrResx = "DNNPlatform/App_GlobalResources/GlobalResources.fr-FR.resx";
		private static readonly string[] Keys = { "MESSAGE_PORTAL_TERMS.Text", "MESSAGE_PORTAL_PRIVACY.Text" };
		private static readonly Regex Tag = new Regex("<[^>]+>");
		private static readonly Regex AsciiDigits = new Regex("[0-9]+"); // \d .NET verrait aussi les chiffres persans

		// La note d'en-tête est insérée juste après le <div> d'ouverture du fr : 4 balises.
		private static readonly string[] NoteTags = { "<p align=\"left\">", "<em>", "</em>", "</p>" };

		private static readonly Dictionary<string, string> FrenchWordInNote = new Dictionary<string, string>
		{
			["zh-CN"] = "法文", ["ar-SA"] = "الفرنسي", ["fa-IR"] = "فرانسوی",
		};

		private static readonly Dictionary<string, int> FrenchMonths = new Dictionary<string, int>
		{
			["janvier"] = 1, ["février"] = 2, ["mars"] = 3, ["avril"] = 4, ["mai"] = 5, ["juin"] = 6,
			["juillet"] = 7, ["août"] = 8, ["septembre"] = 9, ["octobre"] = 10, ["novembre"] = 11, ["décembre"] = 12,
		};

		private static string LocaleResx(string locale) => $"DNNPlatform/App_GlobalResources/GlobalResources.{locale}.resx";

		private static XDocument Load(string resxRelativePath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");
			return XDocument.Load(path);
		}

		private static XElement Entry(string resxRelativePath, string key)
		{
			var entry = Load(resxRelativePath).Root!.Elements("data")
				.FirstOrDefault(d => string.Equals(d.Attribute("name")?.Value, key, StringComparison.Ordinal));
			entry.Should().NotBeNull($"la clé {key} doit exister dans {resxRelativePath}");
			return entry!;
		}

		private static string ResxValue(string resxRelativePath, string key) => Entry(resxRelativePath, key).Element("value")?.Value ?? string.Empty;

		/// <summary>Texte visible : balises retirées, entités décodées (&amp;#8217; ne doit pas compter « 8217 »).</summary>
		private static string PlainText(string html) => WebUtility.HtmlDecode(Tag.Replace(html, " "));

		private static Dictionary<string, int> DigitRuns(string text) => AsciiDigits.Matches(text)
			.GroupBy(m => m.Value).ToDictionary(g => g.Key, g => g.Count());

		public static IEnumerable<object[]> Cases() =>
			from locale in FrenchWordInNote.Keys
			from key in Keys
			select new object[] { locale, key };

		[Theory]
		[InlineData("zh-CN")]
		[InlineData("ar-SA")]
		[InlineData("fa-IR")]
		public void Resx_Carries_Only_The_Two_Legal_Keys_Preserved(string locale)
		{
			var names = Load(LocaleResx(locale)).Root!.Elements("data").Select(d => d.Attribute("name")?.Value).ToList();
			names.Should().BeEquivalentTo(Keys, "le repli DNN est par clé : le fichier ne porte que les deux pages juridiques");
			foreach (var key in Keys)
			{
				// parenthèses obligatoires : « ?.Value.Should() » court-circuiterait l'assertion
				// elle-même quand l'attribut manque (garde vacue, mesurée par mutation)
				(Entry(LocaleResx(locale), key).Element("value")!.Attribute(XNamespace.Xml + "space")?.Value)
					.Should().Be("preserve", $"{locale} {key} : <value xml:space=\"preserve\">");
			}
		}

		[Theory]
		[MemberData(nameof(Cases))]
		public void TagSequence_Is_Fr_Plus_HeaderNote(string locale, string key)
		{
			var fr = Tag.Matches(ResxValue(FrResx, key)).Select(m => m.Value).ToList();
			var loc = Tag.Matches(ResxValue(LocaleResx(locale), key)).Select(m => m.Value).ToList();

			fr.Should().NotBeEmpty();
			loc.Should().HaveCount(fr.Count + NoteTags.Length, $"{locale} {key} : balises du fr + la note d'en-tête");
			loc.Skip(1).Take(NoteTags.Length).Should().Equal(NoteTags, "la note suit immédiatement le <div> d'ouverture");
			loc.Take(1).Concat(loc.Skip(1 + NoteTags.Length)).Should().Equal(fr, $"{locale} {key} : HTML du fr intact, balise pour balise");
		}

		[Theory]
		[MemberData(nameof(Cases))]
		public void HeaderNote_Says_French_Is_Authoritative(string locale, string key)
		{
			var value = ResxValue(LocaleResx(locale), key);
			var note = Regex.Match(value, "^<div[^>]*><p align=\"left\"><em>([^<]+)</em></p>");
			note.Success.Should().BeTrue($"{locale} {key} : la note ouvre la page");
			note.Groups[1].Value.Should().Contain(FrenchWordInNote[locale], "la note nomme le français comme version de référence");
		}

		[Theory]
		[MemberData(nameof(Cases))]
		public void PortalTokens_And_IdentityPins_Match_Fr(string locale, string key)
		{
			var fr = ResxValue(FrResx, key);
			var loc = ResxValue(LocaleResx(locale), key);
			var probes = Regex.Matches(fr, @"\[Portal:\w+\]").Select(m => m.Value).Distinct()
				.Concat(new[] { "ARGUMENTUM SAS", "922020276", "MY INTELLIGENCE AGENCY", "SIREN", "Argumentum" });
			foreach (var probe in probes)
			{
				Regex.Matches(loc, Regex.Escape(probe)).Count.Should().Be(Regex.Matches(fr, Regex.Escape(probe)).Count,
					$"{locale} {key} : « {probe} » copié autant de fois que dans le fr");
			}
		}

		/// <summary>
		/// Chaque nombre du fr est recopié tel quel, en chiffres latins. Seule tolérance :
		/// en zh-CN une date « 21 juin 2004 » s'écrit 2004年6月21日, le mois devient un nombre.
		/// </summary>
		[Theory]
		[MemberData(nameof(Cases))]
		public void Digits_Are_The_Fr_Digits(string locale, string key)
		{
			var frText = PlainText(ResxValue(FrResx, key));
			var locText = PlainText(ResxValue(LocaleResx(locale), key));
			var want = DigitRuns(frText);
			if (locale == "zh-CN")
			{
				var months = Regex.Matches(frText, @"[0-9]{1,2}\s(" + string.Join("|", FrenchMonths.Keys) + @")\s[0-9]{4}");
				foreach (Match m in months)
				{
					var n = FrenchMonths[m.Groups[1].Value].ToString();
					want[n] = want.GetValueOrDefault(n) + 1;
				}
			}
			want.Should().NotBeEmpty("le fr porte des numéros (loi, SIREN, dates) : un multiset vide serait un instrument aveugle");
			DigitRuns(locText).Should().BeEquivalentTo(want, $"{locale} {key} : nombres du fr, ni ajoutés ni perdus");
			Regex.IsMatch(locText, "[٠-٩۰-۹]").Should().BeFalse("décision 2 : chiffres latins, jamais arabo-indiens ni persans");
		}

		/// <summary>Résidu français : mots-outils hors identité légale. Le motif doit d'abord voir le fr.</summary>
		private static readonly Regex FrenchFunctionWord = new Regex(
			@"(?<![A-Za-zÀ-ÿ])(le|la|les|de|des|du|et|ou|un|une|vous|nous|est|dans|pour|par|sur|au|aux|ce|ces|cette|qui|que|site)(?![A-Za-zÀ-ÿ])",
			RegexOptions.IgnoreCase);

		[Theory]
		[MemberData(nameof(Cases))]
		public void No_French_Residue_And_Target_Script(string locale, string key)
		{
			FrenchFunctionWord.Matches(PlainText(ResxValue(FrResx, key))).Count
				.Should().BeGreaterThan(100, "auto-contrôle : le motif doit voir le résidu dans le fr lui-même");

			var text = PlainText(ResxValue(LocaleResx(locale), key));
			FrenchFunctionWord.Matches(text).Select(m => m.Value).Should().BeEmpty($"{locale} {key} : aucun mot-outil français");

			if (locale == "zh-CN")
			{
				Regex.Matches(text, "[一-鿿]").Count.Should().BeGreaterThan(1000, "zh-CN : texte en caractères chinois");
				// formes propres au traditionnel (chacune a une forme simplifiée distincte)
				text.Intersect("們這個為與於對說會來時從種樣機網頁資訊權條隱護處據務關係經過還將發應當則該須確認證單選擇購買實際")
					.Should().BeEmpty("zh-CN : chinois simplifié");
			}
			else
			{
				Regex.Matches(text, "[؀-ۿ]").Count.Should().BeGreaterThan(1000, $"{locale} : texte en écriture arabe");
				if (locale == "fa-IR")
					text.Should().NotContainAny(new[] { "ي", "ك", "ى" }, "fa-IR : ی/ک persans, jamais ي/ك/ى arabes");
				else
					text.Should().NotContainAny(new[] { "ی", "ک" }, "ar-SA : ي/ك arabes, jamais ی/ک persans");
			}
		}

		// Imbrication HTML (pool #458 E3). La séquence de balises ci-dessus est comparée au fr :
		// un défaut présent dans le fr ET dans la traduction y reste vert. Cette garde-ci lit
		// chaque valeur seule, le fr-FR compris.
		private static readonly Regex TagParts = new Regex("<(/?)([A-Za-z0-9]+)[^>]*?(/?)>");
		private static readonly HashSet<string> VoidTags = new HashSet<string>
			{ "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr" };
		private static readonly HashSet<string> OptionalEnd = new HashSet<string> { "p", "li" };
		// ouvrantes qui ferment un p ouvert (spécification HTML), plus li/tr/td
		private static readonly HashSet<string> BlockTags = new HashSet<string>
		{
			"address", "article", "aside", "blockquote", "details", "dialog", "div", "dl", "dd", "dt", "fieldset",
			"figcaption", "figure", "footer", "form", "h1", "h2", "h3", "h4", "h5", "h6", "header", "hgroup", "hr",
			"main", "menu", "nav", "ol", "p", "pre", "section", "table", "ul", "li", "tr", "td",
		};

		/// <summary>
		/// Défauts d'imbrication : fermante orpheline, fermante qui passe par-dessus un élément
		/// encore ouvert, élément jamais fermé. p et li ont une fin optionnelle en HTML : un p se
		/// ferme à l'ouverture d'un bloc, un li à l'ouverture d'un li frère, les deux à la fin de
		/// leur parent — ce ne sont pas des défauts.
		/// </summary>
		private static List<string> NestingDefects(string html)
		{
			var stack = new List<string>();
			var defects = new List<string>();
			foreach (Match m in TagParts.Matches(html))
			{
				var name = m.Groups[2].Value.ToLowerInvariant();
				var opening = m.Groups[1].Value.Length == 0;
				// avant le saut des vides : hr est vide ET ferme le p
				if (opening && BlockTags.Contains(name) && stack.Count > 0 && stack[^1] == "p") stack.RemoveAt(stack.Count - 1);
				if (VoidTags.Contains(name) || m.Groups[3].Value == "/") continue;
				if (opening)
				{
					if (name == "li")
					{
						for (var i = stack.Count - 1; i >= 0 && stack[i] != "ul" && stack[i] != "ol"; i--)
						{
							if (stack[i] != "li") continue;
							if (stack.Skip(i + 1).All(OptionalEnd.Contains)) stack.RemoveRange(i, stack.Count - i);
							break;
						}
					}
					stack.Add(name);
					continue;
				}
				var open = stack.LastIndexOf(name);
				if (open < 0)
				{
					defects.Add($"</{name}> orpheline @{m.Index}");
					continue;
				}
				var above = stack.Skip(open + 1).Where(t => !OptionalEnd.Contains(t)).ToList();
				if (above.Count > 0) defects.Add($"</{name}> @{m.Index} ferme par-dessus <{string.Join("><", above)}>");
				stack.RemoveRange(open, stack.Count - open);
			}
			var left = stack.Where(t => !OptionalEnd.Contains(t)).ToList();
			if (left.Count > 0) defects.Add($"non fermé en fin de valeur : <{string.Join("><", left)}>");
			return defects;
		}

		public static IEnumerable<object[]> NestingCases() =>
			from resx in new[] { FrResx }.Concat(FrenchWordInNote.Keys.Select(LocaleResx))
			from key in Keys
			select new object[] { resx, key };

		[Theory]
		[MemberData(nameof(NestingCases))]
		public void Html_Is_Well_Nested(string resx, string key)
		{
			var value = ResxValue(resx, key);
			TagParts.Matches(value).Count.Should().BeGreaterThan(100, "auto-contrôle : une valeur sans balises rendrait un vert vide");
			NestingDefects(value).Should().BeEmpty($"{resx} {key} : HTML bien imbriqué");
		}

		/// <summary>Contrôle inverse : les quatre formes corrigées par E3 sont vues, et la fin optionnelle de p/li est tolérée.</summary>
		[Fact]
		public void NestingDefects_Sees_The_E3_Shapes()
		{
			var stray = NestingDefects("<div><p>x</p></div></span>");
			stray.Should().HaveCount(1);
			stray[0].Should().Contain("</span> orpheline");

			var liOverA = NestingDefects("<ul><li><a href=\"#t\"><font>x</font></li></a></ul>");
			liOverA.Should().HaveCount(2, "</li> passe par-dessus <a>, puis </a> n'a plus d'ouvrante");
			liOverA[0].Should().Contain("</li>").And.Contain("<a>");

			var ulOverA = NestingDefects("<ul><li><a href=\"#t\"><font>x</font></ul>");
			ulOverA.Should().HaveCount(1);
			ulOverA[0].Should().Contain("</ul>").And.Contain("<a>");

			var unclosed = NestingDefects("<div><p><a href=\"#top\">x</a></p>");
			unclosed.Should().HaveCount(1);
			unclosed[0].Should().Contain("<div>");

			NestingDefects("<div><p>a<p>b<ul><li>c<li>d</ul><br/><img src=\"i\"></div>")
				.Should().BeEmpty("p et li sans fermante, br et img vides : HTML valide");

			// blockquote ferme le p, wbr est vide : le </p> final n'a plus d'ouvrante
			var blockClosesP = NestingDefects("<p>a<blockquote>b</blockquote><wbr></p>");
			blockClosesP.Should().HaveCount(1);
			blockClosesP[0].Should().Contain("</p> orpheline");

			var hrClosesP = NestingDefects("<p>a<hr></p>");
			hrClosesP.Should().HaveCount(1, "hr est vide et ferme le p");
			hrClosesP[0].Should().Contain("</p> orpheline");
		}
	}
}
