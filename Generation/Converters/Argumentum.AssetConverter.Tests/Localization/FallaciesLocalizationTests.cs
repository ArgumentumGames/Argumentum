using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Regression tests for issue #216 — non-FR PDFs were shipping French content because
	/// LocalizationConfig.FrontFieldConversions for Fallacies referenced field names
	/// ("Titre", "Definition", "Exemple", "Contre-Exemple") that never existed in the
	/// Handlebars templates. The templates use {{text_fr}}, {{desc_fr}}, {{example_fr}},
	/// {{Famille}}, {{Sous-Famille}}, {{Soussousfamille}} — these must stay in lockstep
	/// with the mapping source names.
	///
	/// The tests apply the substitution chain from CardSetLocalization.TranslateCardSetInfo
	/// directly against real template files on disk (no mocking), for every target language.
	/// </summary>
	public class FallaciesLocalizationTests
	{
		private static readonly string RepoRoot = FindRepoRoot();

		private static string FindRepoRoot()
		{
			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Cards", "Fallacies")))
			{
				dir = dir.Parent;
			}
			return dir?.FullName ?? throw new DirectoryNotFoundException("Could not locate repository root (Cards/Fallacies not found).");
		}

		private static string ReadTemplate(string relPath)
		{
			var path = Path.Combine(RepoRoot, relPath);
			return File.ReadAllText(path);
		}

		private static CardSetLocalization GetFallaciesLocalization()
		{
			var config = new AssetConverterConfig();
			var loc = config.LocalizationConfig.CardSetLocalizations
				.FirstOrDefault(l => l.CardSetNames.Contains(KnownCardSets.Fallacies));
			loc.Should().NotBeNull("the default LocalizationConfig must carry a Fallacies mapping");
			return loc!;
		}

		private static string ApplyFrontSubstitution(CardSetLocalization loc, string template, string destLang)
		{
			foreach (var fieldConversion in loc.FrontFieldConversions)
			{
				var sourcePattern = loc.FormatField(fieldConversion.sourceFieldName);
				var conv = fieldConversion.fieldConversions.FirstOrDefault(c => c.Language == destLang);
				if (string.IsNullOrEmpty(conv.destFieldName)) continue;
				var destPattern = loc.FormatField(conv.destFieldName);
				template = template.Replace(sourcePattern, destPattern);
			}
			return template;
		}

		// Mirrors the Back branch of CardSetLocalization.TranslateCardSetInfo (front:false) — at runtime
		// the Memo Back card is localized through BackFieldConversions, NOT FrontFieldConversions.
		private static string ApplyBackSubstitution(CardSetLocalization loc, string template, string destLang)
		{
			foreach (var fieldConversion in loc.BackFieldConversions)
			{
				var sourcePattern = loc.FormatField(fieldConversion.sourceFieldName);
				var conv = fieldConversion.fieldConversions.FirstOrDefault(c => c.Language == destLang);
				if (string.IsNullOrEmpty(conv.destFieldName)) continue;
				var destPattern = loc.FormatField(conv.destFieldName);
				template = template.Replace(sourcePattern, destPattern);
			}
			return template;
		}

		[Theory]
		[InlineData("Cards/Fallacies/Argumentum_Fallacies_Face_fr.json", "en")]
		[InlineData("Cards/Fallacies/Argumentum_Fallacies_Face_fr.json", "ru")]
		[InlineData("Cards/Fallacies/Argumentum_Fallacies_Face_fr.json", "pt")]
		[InlineData("Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json", "en")]
		[InlineData("Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json", "en")]
		[InlineData("Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json", "pt")]
		public void Fallacies_Templates_Translate_Away_All_French_Text_Placeholders(string templateRelPath, string destLang)
		{
			var original = ReadTemplate(templateRelPath);
			var loc = GetFallaciesLocalization();

			var translated = ApplyFrontSubstitution(loc, original, destLang);

			var langSuffix = destLang == "en" ? "_en" : $"_{destLang}";

			translated.Should().NotContain("{{text_fr}}",
				$"text_fr placeholder must be replaced in {destLang} template (this was the #216 root cause)");
			translated.Should().NotContain("{{desc_fr}}",
				$"desc_fr placeholder must be replaced in {destLang} template");
			translated.Should().Contain("text" + langSuffix,
				$"translated template must reference text{langSuffix}");
			translated.Should().Contain("desc" + langSuffix,
				$"translated template must reference desc{langSuffix}");
		}

		[Theory]
		[InlineData("en", "Family")]
		[InlineData("ru", "Family_ru")]
		[InlineData("pt", "Family_pt")]
		public void Fallacies_Family_Hierarchy_Uses_CsvColumn_Casing(string destLang, string expectedFamilyColumn)
		{
			var original = ReadTemplate("Cards/Fallacies/Argumentum_Fallacies_Face_fr.json");
			var loc = GetFallaciesLocalization();

			var translated = ApplyFrontSubstitution(loc, original, destLang);

			translated.Should().Contain("{{" + expectedFamilyColumn + "}}",
				$"{destLang} rendering must bind to the real CSV column '{expectedFamilyColumn}' (casing matters — CsvHelper is case-sensitive)");
			translated.Should().NotContain("{{Famille}}",
				$"{destLang} rendering must no longer reference the French {{{{Famille}}}} placeholder");
		}

		[Fact]
		public void Fallacies_Front_Conversions_Are_Ordered_MostSpecificFirst_To_Avoid_Partial_Matches()
		{
			var loc = GetFallaciesLocalization();
			var names = loc.FrontFieldConversions.Select(c => c.sourceFieldName).ToList();

			var soussousIndex = names.IndexOf("Soussousfamille");
			var sousIndex = names.IndexOf("Sous-Famille");
			var familleIndex = names.IndexOf("Famille");

			soussousIndex.Should().BeGreaterThanOrEqualTo(0, "Soussousfamille must be mapped");
			sousIndex.Should().BeGreaterThanOrEqualTo(0, "Sous-Famille must be mapped");
			familleIndex.Should().BeGreaterThanOrEqualTo(0, "Famille must be mapped");

			soussousIndex.Should().BeLessThan(sousIndex,
				"Soussousfamille must be substituted before Sous-Famille to prevent partial overlap");
			sousIndex.Should().BeLessThan(familleIndex,
				"Sous-Famille must be substituted before Famille to prevent partial overlap");
		}


		[Theory]
		[InlineData("en")]
		[InlineData("ru")]
		[InlineData("pt")]
		public void Memo_Back_Template_Subtitle_Is_Translated_And_Selector_Stays_FR(string destLang)
		{
			// Regression test for #358 / #443 — two bugs caught by ai-01 validation:
			//  Bug 1: apostrophe U+2019 in source vs U+0027 in template -> subtitle stays FR.
			//  Bug 2: converting text_fr->text_en in ifCond breaks family grouping (6/8 families vanish).
			var original = ReadTemplate("Cards/Memo/Argumentum_Memo_Back_fr.json");
			var loc = GetFallaciesLocalization();

			// Apply FrontFieldConversions (Famille->Family etc.)
			var translated = ApplyFrontSubstitution(loc, original, destLang);

			// Apply StaticConversions (subtitle translation)
			translated = loc.DoStaticConversions(translated, destLang);

			// (a) Subtitle must be translated — no more FR "L'art de jamais avoir tort"
			translated.Should().NotContain("L'art de jamais avoir tort",
				$"{destLang} Memo Back must have a translated subtitle, not the FR original (#358)");

			// (b) The ifCond selector must stay FR-invariant — text_fr must remain
			//     so that Famille(FR)==text_fr(FR) still groups all 8 families correctly.
			translated.Should().Contain("text_fr ",
				$"{destLang} Memo Back ifCond must keep text_fr (FR-invariant selector for family grouping)");
		}

		[Theory]
		[InlineData("en", "Family", "Subfamily", "Subsubfamily")]
		[InlineData("ru", "Family_ru", "Subfamily_ru", "Subsubfamily_ru")]
		[InlineData("pt", "Family_pt", "Subfamily_pt", "Subsubfamily_pt")]
		public void Memo_Back_Taxonomy_Display_Tokens_Are_Localized_While_Grouping_Selector_Stays_FR(
			string destLang, string family, string subfamily, string subsubfamily)
		{
			// Regression test for the #358/#435/#443 follow-up — the Memo Back card kept its taxonomy
			// labels in French (Famille / Sous-Famille / Soussousfamille) in EN/RU/PT because the Memo
			// BackFieldConversions only carried tagline_fr. At runtime the Back is rendered through
			// BackFieldConversions (TranslateCardSetInfo, front:false), so the taxonomy DISPLAY tokens
			// must be localized there — while the FR-invariant ifCond family selector (Famille == text_fr)
			// must stay untouched so the 8 families still group correctly.
			var original = ReadTemplate("Cards/Memo/Argumentum_Memo_Back_fr.json");
			var loc = GetFallaciesLocalization();

			var translated = ApplyBackSubstitution(loc, original, destLang);
			translated = loc.DoStaticConversions(translated, destLang);

			// (a) Display tokens localized to the real CSV columns (casing matters — CsvHelper is case-sensitive).
			translated.Should().Contain("{{" + family + "}}", $"{destLang} Back must bind family label to CSV column '{family}'");
			translated.Should().Contain("{{" + subfamily + "}}", $"{destLang} Back must bind subfamily label to CSV column '{subfamily}'");
			translated.Should().Contain("{{" + subsubfamily + "}}", $"{destLang} Back must bind subsubfamily label to CSV column '{subsubfamily}'");

			// (b) French display tokens must be gone.
			translated.Should().NotContain("{{Famille}}", $"{destLang} Back family label must no longer be the FR token");
			translated.Should().NotContain("{{Sous-Famille}}", $"{destLang} Back subfamily label must no longer be the FR token");
			translated.Should().NotContain("{{Soussousfamille}}", $"{destLang} Back subsubfamily label must no longer be the FR token");

			// (c) The FR-invariant grouping selector must survive: ifCond keeps Famille == text_fr.
			//     NB: this template is read raw from the .json (no JSON-unescape), so the operator quotes
			//     appear escaped on disk as \"==\" — assert against that on-disk form.
			translated.Should().Contain("Famille \\\"==\\\"", $"{destLang} Back ifCond family operand must stay FR (data-driven grouping)");
			translated.Should().Contain("text_fr ", $"{destLang} Back ifCond must keep text_fr (FR-invariant selector)");

			// (d) The CSS colour class binding ({{Famille_camelCase}}) must stay intact.
			translated.Should().Contain("Famille_camelCase", $"{destLang} Back CSS colour class binding must be preserved");

			// (e) Subtitle still localized via StaticConversions.
			translated.Should().NotContain("L'art de jamais avoir tort", $"{destLang} Back subtitle must be translated (#358)");
		}

		[Fact]
		public void Memo_Back_Conversions_Include_Taxonomy_Ordered_MostSpecificFirst()
		{
			var loc = GetFallaciesLocalization();
			var names = loc.BackFieldConversions.Select(c => c.sourceFieldName).ToList();

			names.Should().Contain("Soussousfamille", "Memo Back must localize the subsubfamily label (#443 follow-up)");
			names.Should().Contain("Sous-Famille", "Memo Back must localize the subfamily label (#443 follow-up)");
			names.Should().Contain("Famille", "Memo Back must localize the family label (#443 follow-up)");
			names.Should().Contain("tagline_fr", "the original tagline mapping must be preserved");

			var soussousIndex = names.IndexOf("Soussousfamille");
			var sousIndex = names.IndexOf("Sous-Famille");
			var familleIndex = names.IndexOf("Famille");

			soussousIndex.Should().BeLessThan(sousIndex,
				"Soussousfamille must precede Sous-Famille so 'Famille}}' does not clobber 'Sous-Famille}}'");
			sousIndex.Should().BeLessThan(familleIndex,
				"Sous-Famille must precede Famille to prevent partial overlap");
		}

		[Fact]
		public void Rules_Localization_Is_Configured_For_All_Target_Languages()
		{
			var config = new AssetConverterConfig();
			var loc = config.LocalizationConfig.CardSetLocalizations
				.FirstOrDefault(l => l.CardSetNames.Contains(KnownCardSets.Rules));
			loc.Should().NotBeNull("Rules must have a localization entry (regression from Golden Master April 2024)");

			var textConv = loc!.FrontFieldConversions.FirstOrDefault(c => c.sourceFieldName == "Text");
			textConv.fieldConversions.Should().Contain(c => c.Language == "en" && c.destFieldName == "Text_en");
			textConv.fieldConversions.Should().Contain(c => c.Language == "ru" && c.destFieldName == "Text_ru");
			textConv.fieldConversions.Should().Contain(c => c.Language == "pt" && c.destFieldName == "Text_pt");
		}
	}
}
