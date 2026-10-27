using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du formulaire de newsletter (pool #458 D2) : le fragment app 48
	/// (FB8362B1|Html, inséré par #2036), traduit dans les 7 langues. Seul le texte
	/// visible se traduit ; le reste est le squelette du fr à l'octet près : balises,
	/// attributs, URL, identifiants de formulaire, CSS, code du script et
	/// <c>window.LOCALE</c>. Les textes HTML sont DÉRIVÉS au moment du test du
	/// formulaire déjà traduit 44088360|Text (mêmes textes fr visibles) ; les messages
	/// de validation du script sont la seule traduction neuve de D2.
	/// </summary>
	public class DnnNewsletterFormGuardTests
	{
		private const string RegistryCsv = "docs/dnn-localization/eav-content.csv";
		private const string Form = "FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91|Html";
		private const string TranslatedForm = "44088360-FA3E-45CB-BFA7-81440906202D|Text";
		private static readonly string[] Languages = { "en", "ru", "pt", "es", "ar", "fa", "zh" };

		// opaque pour le découpage : commentaires, blocs <style> et <script>, balises ; le texte entre eux est visible
		private static readonly Regex Opaque = new Regex("<!--.*?-->|<style\\b[^>]*>.*?</style>|<script\\b[^>]*>.*?</script>|<[^>]+>",
			RegexOptions.Singleline | RegexOptions.IgnoreCase);
		private static readonly Regex ScriptBlock = new Regex("^(<script\\b[^>]*>)(.*)(</script>)$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
		// littéraux du script, apostrophes ou guillemets droits, échappements compris
		private static readonly Regex Literal = new Regex("(['\"])((?:\\\\.|(?!\\1).)*)\\1", RegexOptions.Singleline);
		private static readonly Regex Escape = new Regex("\\\\(.)");
		private static readonly Regex Whitespace = new Regex("\\s+");
		private static readonly Regex Token = new Regex("\\{\\w+\\}");
		private const char Hole = '\u0000';
		// identifiant, pas texte : la langue du script Sendinblue reste celle du fr
		private const string KeptLiteralPrefix = "window.LOCALE = ";

		/// <summary>squelette à trous + cœur de chaque trou : texte HTML (sans ses blancs de bord) ou littéral du script</summary>
		private static (string Skeleton, List<(string Kind, string Core)> Holes) Split(string value)
		{
			var skeleton = new List<string>();
			var holes = new List<(string, string)>();
			void TextRun(string run)
			{
				var core = run.Trim();
				if (core.Length == 0) { skeleton.Add(run); return; }
				var a = run.IndexOf(core, StringComparison.Ordinal);
				skeleton.Add(run.Substring(0, a) + Hole + run.Substring(a + core.Length));
				holes.Add(("txt", core));
			}
			var pos = 0;
			foreach (Match m in Opaque.Matches(value))
			{
				TextRun(value.Substring(pos, m.Index - pos));
				var script = ScriptBlock.Match(m.Value);
				if (script.Success)
				{
					var body = script.Groups[2].Value;
					var p = 0;
					skeleton.Add(script.Groups[1].Value);
					foreach (Match lit in Literal.Matches(body))
					{
						if (body.Substring(0, lit.Index).EndsWith(KeptLiteralPrefix, StringComparison.Ordinal)) continue;
						var inner = lit.Groups[2];
						skeleton.Add(body.Substring(p, inner.Index - p) + Hole);
						holes.Add(("js", inner.Value));
						p = inner.Index + inner.Length;
					}
					skeleton.Add(body.Substring(p) + script.Groups[3].Value);
				}
				else skeleton.Add(m.Value);
				pos = m.Index + m.Length;
			}
			TextRun(value.Substring(pos));
			return (string.Concat(skeleton), holes);
		}

		// forme comparée : entités HTML décodées, ou échappements du script retirés ; blancs réduits
		private static string Visible((string Kind, string Core) hole) =>
			Whitespace.Replace(hole.Kind == "txt" ? WebUtility.HtmlDecode(hole.Core) : Escape.Replace(hole.Core, "$1"), " ").Trim();

		private static List<string> Visibles(string value, string kind) =>
			Split(value).Holes.Where(h => h.Kind == kind).Select(Visible).Where(t => t.Length > 0).ToList();

		private static Dictionary<string, string> Registry(string key)
		{
			var path = Path.Combine(TestRepoRoot.Find(), RegistryCsv.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le fichier doit exister dans le checkout ({RegistryCsv})");
			var config = new CsvConfiguration(CultureInfo.InvariantCulture) { MissingFieldFound = null, HeaderValidated = null };
			var rows = new List<Dictionary<string, string>>();
			using (var reader = new StreamReader(path))
			using (var csv = new CsvReader(reader, config))
			{
				csv.Read();
				csv.ReadHeader();
				var header = csv.HeaderRecord ?? Array.Empty<string>();
				while (csv.Read())
				{
					if (csv.GetField(0) != key) continue;
					var row = new Dictionary<string, string>(StringComparer.Ordinal);
					for (var i = 0; i < header.Length; i++) row[header[i]] = csv.GetField(i) ?? string.Empty;
					rows.Add(row);
				}
			}
			rows.Should().HaveCount(1, $"le registre doit porter {key} une fois");
			return rows[0];
		}

		public static IEnumerable<object[]> LanguageCases() => Languages.Select(l => new object[] { l });

		[Fact]
		public void Split_Witness()
		{
			// témoin du découpage : texte HTML et littéral du script sont des trous ; commentaire, CSS, attribut, LOCALE n'en sont pas
			var (skeleton, holes) = Split("<!-- a --><style>p{x:'y'}</style><p class=\"c\"> Texte </p><script>window.LOCALE = 'fr'; m = \"l'a \\\"b\\\"\";</script>");
			skeleton.Should().Be("<!-- a --><style>p{x:'y'}</style><p class=\"c\"> " + Hole + " </p><script>window.LOCALE = 'fr'; m = \"" + Hole + "\";</script>");
			holes.Should().Equal(("txt", "Texte"), ("js", "l'a \\\"b\\\""));
			Visible(holes[1]).Should().Be("l'a \"b\"", "les échappements du script sont retirés pour la comparaison");
		}

		[Fact]
		public void Fr_Form_Is_Split_Into_Its_Visible_Texts()
		{
			var fr = Registry(Form)["fr"];
			var (skeleton, holes) = Split(fr);
			var parts = skeleton.Split(Hole);
			parts.Length.Should().Be(holes.Count + 1, "un trou par texte visible");
			string.Concat(parts.Zip(holes, (p, h) => p + h.Core).Append(parts[parts.Length - 1]))
				.Should().Be(fr, "le squelette et ses trous reconstituent le fr octet pour octet");
			holes.Where(h => h.Core.Contains('<') || h.Core.Contains('>')).Should().BeEmpty("aucune balise n'est avalée par un trou");
			skeleton.Should().Contain("window.LOCALE = 'fr'", "LOCALE reste dans le squelette");
			Visibles(fr, "js").Should().HaveCount(6, "les 6 messages du script Sendinblue, hors LOCALE");
			Visibles(fr, "txt").Should().Equal(Visibles(Registry(TranslatedForm)["fr"], "txt"),
				$"le fragment et {TranslatedForm} portent les mêmes textes fr visibles, dans le même ordre");
		}

		[Theory]
		[MemberData(nameof(LanguageCases))]
		public void Skeleton_Is_The_Fr_Bytes(string lang)
		{
			Split(Registry(Form)[lang]).Skeleton.Should().Be(Split(Registry(Form)["fr"]).Skeleton,
				$"[{lang}] balises, attributs, URL, identifiants, CSS, code du script et LOCALE : les octets du fr, seul le texte visible change");
		}

		[Theory]
		[MemberData(nameof(LanguageCases))]
		public void Html_Texts_Are_The_Translated_Form_Texts(string lang)
		{
			var want = Visibles(Registry(TranslatedForm)[lang], "txt");
			want.Should().NotBeEmpty($"{TranslatedForm} [{lang}] porte les textes du formulaire traduit");
			Visibles(Registry(Form)[lang], "txt").Should().Equal(want, $"[{lang}] textes HTML du formulaire déjà traduit, dans l'ordre");
		}

		[Theory]
		[MemberData(nameof(LanguageCases))]
		public void Script_Messages_Are_Translated(string lang)
		{
			var fr = Visibles(Registry(Form)["fr"], "js");
			var loc = Visibles(Registry(Form)[lang], "js");
			loc.Should().HaveCount(fr.Count, $"[{lang}] un message par message du fr");
			for (var i = 0; i < fr.Count; i++)
			{
				loc[i].Should().NotBe(fr[i], $"[{lang}] message {i} : traduit, pas recopié du fr");
				Token.Matches(loc[i]).Select(m => m.Value).Should().Equal(Token.Matches(fr[i]).Select(m => m.Value),
					$"[{lang}] message {i} : mêmes jetons {{…}} que le fr (« {loc[i]} »)");
				for (var k = i + 1; k < fr.Count; k++)
					if (fr[k] == fr[i])
						loc[k].Should().Be(loc[i], $"[{lang}] messages {i} et {k} : une même phrase fr, une même traduction");
			}
		}

		[Fact]
		public void Arabic_And_Persian_Letters_Are_Their_Own()
		{
			var row = Registry(Form);
			var fa = string.Join(" ", Visibles(row["fa"], "txt").Concat(Visibles(row["fa"], "js")));
			var ar = string.Join(" ", Visibles(row["ar"], "txt").Concat(Visibles(row["ar"], "js")));
			fa.Should().NotContainAny(new[] { "ي", "ك", "ى" }, "fa : ی/ک persans, jamais ي/ك/ى arabes");
			ar.Should().NotContainAny(new[] { "ی", "ک" }, "ar : ي/ك arabes, jamais ی/ک persans");
		}
	}
}
