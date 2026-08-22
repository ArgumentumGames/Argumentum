using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Contract for issue #1132: a CSS rule that only applies to one language must actually match
	/// an element in the template once that template has been translated into that language.
	///
	/// <para><b>Why this test exists rather than a simpler one.</b> The eleven card templates each
	/// shipped a "RTL/CJK language support" block whose three hooks were all inert:</para>
	/// <list type="bullet">
	///   <item>a <c>lang</c> attribute selector — no template carries a <c>lang</c> attribute;</item>
	///   <item>an rtl direction selector — the templates declare <c>dir=auto</c>. The browser
	///   <i>computes</i> the direction, but the attribute's written value stays the string
	///   <c>auto</c>, which does not match a selector on the value <c>rtl</c>;</item>
	///   <item>class selectors suffixed by language (<c>.desc_ar</c>, <c>.exemple_fa</c>…) — class
	///   names are never translated. <see cref="CardSetLocalization.ApplyFieldConversions"/>
	///   replaces the pattern <c>&lt;name&gt;}}</c>, i.e. mustache <i>tokens</i>: it rewrites
	///   <c>{{desc_fr}}</c> but never the attribute <c>class="desc_fr"</c>, which is not followed
	///   by <c>}}</c>.</item>
	/// </list>
	/// <para>All three defects share one shape — a selector nobody ever confronted with the DOM it
	/// was supposed to match — so the guard is written against that shape rather than against the
	/// individual selectors. Asserting merely that <c>argu-lang-ar</c> appears would be tautological
	/// with the fix; asserting that <i>every</i> language-scoped selector resolves is what would have
	/// failed on the pre-fix templates, and what will fail again if someone adds a dead one.</para>
	/// </summary>
	public class LanguageSpecificCssContractTests
	{
		/// <summary>Languages whose scripts need dedicated CSS (Arabic, Persian, Chinese).</summary>
		private static readonly string[] ScriptLanguages = { "ar", "fa", "zh" };

		/// <summary>Languages that must be provably untouched: no CSS rule may target them.</summary>
		private static readonly string[] LatinCyrillicLanguages = { "en", "ru", "pt", "es" };

		/// <summary>Every localized card template, with the CardSet whose localization group owns it.</summary>
		public static readonly (string RelPath, string CardSet)[] Templates =
		{
			("Cards/Fallacies/Argumentum_Fallacies_Face_fr.json",     "Fallacies"),
			("Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json",   "Fallacies-2"),
			("Cards/Fallacies/Argumentum_Fallacies_Face_3_fr.json",   "Fallacies-3"),
			("Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json", "Fallacies-Web"),
			("Cards/Fallacies/Argumentum_Fallacies_Back_fr.json",     "Fallacies"),
			("Cards/Fallacies/Argumentum_Virtues_Face_fr.json",       "Virtues"),
			("Cards/Memo/Argumentum_Memo_Face_fr.json",               "Memo"),
			("Cards/Memo/Argumentum_Memo_Back_fr.json",               "Memo"),
			("Cards/Rules/Argumentum_Rules_fr.json",                  "Rules"),
			("Cards/Scenarii/Argumentum_Scenarii_Face_fr.json",       "Scenarii"),
			("Cards/Scenarii/Argumentum_Scenarii_Back_fr.json",       "Scenarii"),
		};

		public static IEnumerable<object[]> TemplatesByScriptLanguage() =>
			from t in Templates from l in ScriptLanguages select new object[] { t.RelPath, t.CardSet, l };

		public static IEnumerable<object[]> TemplatePaths() =>
			Templates.Select(t => new object[] { t.RelPath });

		private static CardSetDocument LoadTemplate(string relPath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), relPath);
			File.Exists(path).Should().BeTrue($"template must exist at {relPath}");
			// Utf8Json, explicitly — this is the deserializer production uses. CardSetInfo.cs imports
			// `Utf8Json` but not `System.Text.Json`, so its unqualified `JsonSerializer.Deserialize`
			// binds to Utf8Json, and the difference is not cosmetic: Argumentum_Virtues_Face_fr.json
			// carries a raw 0x0A inside its `css` string (line 29 — present on master, unrelated to
			// #1132). Utf8Json tolerates it; System.Text.Json rejects the file outright. A test that
			// picked the stricter parser would report a broken template that production loads daily.
			var doc = Utf8Json.JsonSerializer.Deserialize<CardSetDocument>(File.ReadAllBytes(path));
			doc.Should().NotBeNull($"{relPath} must deserialize as a CardSetDocument");
			return doc!;
		}

		private static CardSetLocalization LocalizationFor(string cardSet)
		{
			var loc = new AssetConverterConfig().LocalizationConfig.CardSetLocalizations
				.FirstOrDefault(l => l.CardSetNames.Contains(cardSet));
			loc.Should().NotBeNull($"the default LocalizationConfig must carry a mapping for CardSet '{cardSet}'");
			return loc!;
		}

		/// <summary>
		/// Runs the real translation chain in the real order used by
		/// <c>CardSetLocalization.TranslateCardSetInfo</c>: field conversions first, then static
		/// conversions. Calling the production methods (rather than re-implementing them here) is
		/// deliberate — a mirrored copy drifts silently from the code it claims to protect.
		/// </summary>
		private static string Translate(CardSetLocalization loc, string mustache, string destLang)
		{
			var translated = CardSetLocalization.ApplyFieldConversions(
				mustache, loc.FrontFieldConversions ?? new List<(string, List<(string, string)>)>(),
				loc.ExceptionPatterns, destLang);
			return loc.DoStaticConversions(translated, destLang);
		}

		/// <summary>All distinct class names appearing in any <c>class</c> attribute of the markup.</summary>
		private static HashSet<string> ClassNamesIn(string markup) =>
			Regex.Matches(markup, @"class\s*=\s*(['""])(?<v>.*?)\1")
				.SelectMany(m => m.Groups["v"].Value.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries))
				.Where(c => !c.Contains("{{"))   // ignore class names computed from data (e.g. cardClass)
				.ToHashSet(StringComparer.Ordinal);

		/// <summary>
		/// Class selectors in the stylesheet that are scoped to <paramref name="lang"/>, i.e. whose
		/// name ends in <c>-lang</c> or <c>_lang</c> (<c>.argu-lang-ar</c>, <c>.desc_ar</c>…).
		/// </summary>
		private static IEnumerable<string> LanguageScopedClassSelectors(string css, string lang) =>
			Regex.Matches(css, @"\.(?<n>[A-Za-z][A-Za-z0-9_-]*)")
				.Select(m => m.Groups["n"].Value)
				.Where(n => n.EndsWith("-" + lang, StringComparison.Ordinal)
						 || n.EndsWith("_" + lang, StringComparison.Ordinal))
				.Distinct(StringComparer.Ordinal);

		[Theory]
		[MemberData(nameof(TemplatesByScriptLanguage))]
		public void Every_language_scoped_css_selector_matches_an_element_once_translated(
			string relPath, string cardSet, string lang)
		{
			var doc = LoadTemplate(relPath);
			var selectors = LanguageScopedClassSelectors(doc.css ?? "", lang).ToList();

			selectors.Should().NotBeEmpty(
				$"{relPath} must carry at least one rule scoped to '{lang}' — {lang} needs a script-specific font");

			var classes = ClassNamesIn(Translate(LocalizationFor(cardSet), doc.mustache ?? "", lang));

			foreach (var selector in selectors)
			{
				classes.Should().Contain(selector,
					$"CSS rule '.{selector}' in {relPath} must match an element once the template is "
					+ $"translated to '{lang}'. If it does not, the rule is dead weight and the "
					+ $"script renders with the arbitrary system fallback — this is issue #1132.");
			}
		}

		/// <summary>Element names actually used by the markup (a template's own vocabulary).</summary>
		private static HashSet<string> ElementNamesIn(string markup) =>
			Regex.Matches(markup, @"<(?<n>[A-Za-z][A-Za-z0-9_-]*)")
				.Select(m => m.Groups["n"].Value)
				.ToHashSet(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Components of a language-scoped selector that resolve to nothing in the given markup.
		/// <para>Deliberately agnostic to <i>how</i> the rule is scoped — by class
		/// (<c>.argu-lang-ar</c>) or by attribute (<c>[lang="ar"]</c>) — because the defect this
		/// guards against is orthogonal to the anchoring mechanism: a compound selector whose
		/// descendant part was never confronted with the DOM. The classic instance is a dropped
		/// leading dot: <c>[lang="ar"] famille</c> reads as the element <c>&lt;famille&gt;</c>, which
		/// does not exist — <c>famille</c> is a <i>class</i>. The whole compound then matches nothing,
		/// silently, and the anchor being correct does not save it.</para>
		/// </summary>
		internal static IReadOnlyList<string> UnresolvedSelectorComponents(string css, string markup)
		{
			var stripped = Regex.Replace(css, @"/\*.*?\*/", " ", RegexOptions.Singleline);
			var classes = ClassNamesIn(markup);
			var elements = ElementNamesIn(markup);
			var unresolved = new List<string>();

			foreach (Match rule in Regex.Matches(stripped, @"(?<sel>[^{}]+)\{[^{}]*\}", RegexOptions.Singleline))
			foreach (var fragment in rule.Groups["sel"].Value.Split(','))
			{
				var frag = fragment.Trim();
				if (!Regex.IsMatch(frag, @"\.argu-lang-(?:ar|fa|zh)\b|\[lang\s*=\s*[""']?(?:ar|fa|zh)"))
					continue;

				foreach (var raw in frag.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries))
				{
					var part = raw.Trim();
					if (part.Length == 0 || part == "*" || part is ">" or "+" or "~") continue;
					if (part.StartsWith("[", StringComparison.Ordinal)) continue;          // the lang anchor
					if (part.StartsWith(":", StringComparison.Ordinal)) continue;          // pseudo-class
					if (part.StartsWith(".argu-lang-", StringComparison.Ordinal)) continue; // the class anchor

					var name = part.Split(':')[0].TrimEnd('>', '+', '~');
					var resolves = name.StartsWith(".", StringComparison.Ordinal)
						? classes.Contains(name.Substring(1))
						: elements.Contains(name);

					if (!resolves) unresolved.Add($"{frag}  ->  '{part}'");
				}
			}
			return unresolved;
		}

		/// <summary>
		/// Every component of every language-scoped selector must resolve against the translated
		/// markup — whatever anchoring style the rule uses.
		/// </summary>
		[Theory]
		[MemberData(nameof(TemplatesByScriptLanguage))]
		public void Every_component_of_a_language_scoped_selector_resolves_in_the_markup(
			string relPath, string cardSet, string lang)
		{
			var doc = LoadTemplate(relPath);
			var translated = Translate(LocalizationFor(cardSet), doc.mustache ?? "", lang);

			UnresolvedSelectorComponents(doc.css ?? "", translated).Should().BeEmpty(
				$"every part of a language-scoped selector in {relPath} must match something once the "
				+ $"template is translated to '{lang}'. A single unresolvable component kills the whole "
				+ $"compound, and the script falls back to whatever font the host happens to have (#1132).");
		}

		/// <summary>
		/// The guard above has nothing to check on a stylesheet that scopes by a single wildcard —
		/// which is by design, a shape with no enumeration cannot get the enumeration wrong. These two
		/// cases prove the guard is nonetheless not vacuous: it reports a dropped leading dot, and it
		/// stays silent on the same rule written correctly.
		/// </summary>
		[Fact]
		public void The_guard_reports_a_dropped_leading_dot()
		{
			const string markup = @"<div class=""cardContainer"" lang=""ar""><div class=""famille"">x</div></div>";

			UnresolvedSelectorComponents(@"[lang=""ar""] famille { font-family: 'Vazirmatn'; }", markup)
				.Should().ContainSingle().Which.Should().Contain("'famille'",
					"'famille' is a class here, so a dotless selector reads it as an element name and matches nothing");

			UnresolvedSelectorComponents(@"[lang=""ar""] .famille { font-family: 'Vazirmatn'; }", markup)
				.Should().BeEmpty("the same rule with its dot resolves");
		}

		[Theory]
		[MemberData(nameof(TemplatePaths))]
		public void No_css_rule_targets_a_language_that_must_stay_unchanged(string relPath)
		{
			var doc = LoadTemplate(relPath);
			foreach (var lang in LatinCyrillicLanguages)
			{
				LanguageScopedClassSelectors(doc.css ?? "", lang).Should().BeEmpty(
					$"{relPath} must not style '{lang}' — #1132 is scoped to Arabic/Persian/Chinese, and "
					+ $"the invariance of the other languages has to hold by construction, not by inspection");
			}
		}

		/// <summary>
		/// A font a rule names but the page never loads is as inert as a selector that matches
		/// nothing — the glyphs still fall back to whatever the host happens to have. Found this way:
		/// Argumentum_Scenarii_Back_fr.json shipped an empty <c>extCSS</c> while rendering a
		/// translated category name, so its script text had no font at all.
		/// </summary>
		[Theory]
		[MemberData(nameof(TemplatePaths))]
		public void Every_font_named_by_a_language_scoped_rule_is_actually_loaded(string relPath)
		{
			var doc = LoadTemplate(relPath);
			var css = doc.css ?? "";

			// Where a font can legitimately come from: an external stylesheet, or a local @font-face.
			var loaded = (doc.extCSS ?? "") + " " +
				string.Join(" ", Regex.Matches(css, @"@font-face\s*\{[^}]*\}", RegexOptions.Singleline)
					.Select(m => m.Value));

			var scoped = Regex.Matches(css,
				@"\.argu-lang-(?:ar|fa|zh)[^{]*\{(?<body>[^}]*)\}", RegexOptions.Singleline);

			foreach (var family in scoped
				.SelectMany(m => Regex.Matches(m.Groups["body"].Value, @"font-family:\s*(?<v>[^;]+);")
					.Select(f => f.Groups["v"].Value))
				.Select(v => Regex.Replace(v, @"!\s*important\s*$", "", RegexOptions.IgnoreCase))
				.SelectMany(v => v.Split(','))
				.Select(p => p.Trim().Trim('\'', '"').Trim())
				.Where(p => p.Length > 0 && !Generics.Contains(p, StringComparer.OrdinalIgnoreCase))
				.Distinct(StringComparer.OrdinalIgnoreCase))
			{
				// Google Fonts URLs spell the family with '+' separators.
				(loaded.Contains(family, StringComparison.OrdinalIgnoreCase)
				 || loaded.Contains(family.Replace(" ", "+"), StringComparison.OrdinalIgnoreCase))
					.Should().BeTrue(
						$"{relPath} names font '{family}' in a language-scoped rule but never loads it "
						+ $"(neither in extCSS nor in an @font-face) — the rule cannot take effect");
			}
		}

		/// <summary>CSS generic families, which are resolved by the browser and need no loading.</summary>
		private static readonly string[] Generics =
			{ "sans-serif", "serif", "monospace", "cursive", "fantasy", "system-ui", "important" };

		[Theory]
		[MemberData(nameof(TemplatesByScriptLanguage))]
		public void Language_marker_is_rewritten_by_the_static_conversion(string relPath, string cardSet, string lang)
		{
			var doc = LoadTemplate(relPath);
			doc.mustache.Should().Contain("argu-lang-fr",
				$"{relPath} must carry the language marker on its top-level containers");

			var translated = Translate(LocalizationFor(cardSet), doc.mustache!, lang);

			translated.Should().Contain($"argu-lang-{lang}",
				$"the static conversion must rewrite the marker for '{lang}' — without an entry in this "
				+ $"CardSet's StaticConversions, the marker stays FR and every rule below it is inert");
			translated.Should().NotContain("argu-lang-fr",
				"no container may keep the FR marker after translation");
		}

		public static IEnumerable<object[]> TemplatesByTranslatedLanguage() =>
			from t in Templates
			from l in ScriptLanguages.Concat(LatinCyrillicLanguages)
			select new object[] { t.RelPath, t.CardSet, l };

		/// <summary>
		/// The <c>lang</c> attribute must follow the render language on every template and every
		/// language — not only the ones with script-specific CSS. Nothing in this repository's CSS
		/// reads it; the browser does, to pick fallback fonts, shape cursive scripts and hyphenate,
		/// and so do screen readers. A card rendered in Arabic while announcing <c>lang="fr"</c> is
		/// telling the renderer something false about its own content.
		/// </summary>
		[Theory]
		[MemberData(nameof(TemplatesByTranslatedLanguage))]
		public void Lang_attribute_follows_the_render_language(string relPath, string cardSet, string lang)
		{
			var doc = LoadTemplate(relPath);
			doc.mustache.Should().Contain("lang=\"fr\"",
				$"{relPath} must anchor its containers with a lang attribute");

			var translated = Translate(LocalizationFor(cardSet), doc.mustache!, lang);

			translated.Should().Contain($"lang=\"{lang}\"",
				$"the static conversion must rewrite the lang attribute for '{lang}'");
			translated.Should().NotContain("lang=\"fr\"",
				$"no container may still declare French while rendering '{lang}'");
			translated.Should().NotContain("lang='fr'",
				"the single-quoted spelling would escape the conversion, which matches the double-quoted one");
		}
	}
}
