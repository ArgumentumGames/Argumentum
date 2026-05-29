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

		[Theory]
		[InlineData("ru", true)]
		[InlineData("en", false)]
		[InlineData("pt", false)]
		[InlineData("fr", false)]
		public void Fallacies_StaticConversions_Inject_LangRu_Body_Class_For_Ru_Only(string destLang, bool expectLangRu)
		{
			// #316: Cyrillic Fallacies titles run ~15-20% wider than Latin and clip off-card.
			// The fix injects a `lang-ru` marker class on the card body wrapper for RU only,
			// which engages the inert `.lang-ru .title` rules carried by the templates.
			// EN/PT/FR mustache must stay byte-for-byte untouched (no StaticConversion entry).
			var loc = GetFallaciesLocalization();
			const string template = "<div class=\"body\"><div class=\"title\"><div>NLP</div></div></div>";

			var converted = loc.DoStaticConversions(template, destLang);

			if (expectLangRu)
			{
				converted.Should().Contain("<div class=\"body lang-ru\">",
					"RU mustache must carry the lang-ru marker class so the .lang-ru CSS rules apply");
			}
			else
			{
				converted.Should().Be(template,
					$"{destLang} mustache must be left untouched by the RU-only overflow fix");
			}
		}

		[Theory]
		[InlineData("Cards/Fallacies/Argumentum_Fallacies_Face_fr.json")]
		[InlineData("Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json")]
		public void Fallacies_OverflowProne_Templates_Carry_LangRu_Title_Rules(string templateRelPath)
		{
			// The injected lang-ru body class is inert without matching CSS. Guard that the
			// large-title templates ship the `.lang-ru .title` font-size reduction plus the
			// overflow-wrap break-word escape hatch for extreme single Cyrillic words (#316).
			var template = ReadTemplate(templateRelPath);

			template.Should().Contain(".lang-ru .title",
				"overflow-prone Fallacies templates must carry the .lang-ru title rules (#316)");
			template.Should().Contain("overflow-wrap: break-word",
				"long single Cyrillic words must be allowed to break to avoid clipping off-card");
		}
	}
}
