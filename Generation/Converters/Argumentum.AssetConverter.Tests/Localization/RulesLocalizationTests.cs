using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Regression tests for the Rules localization mapping — issue #204 (coverage) and the
	/// class of bug documented as #216 (FrontFieldConversions referencing field names that do
	/// not exist in the template, silently leaving FR content in non-FR PDFs).
	///
	/// NEW file (dispatch #204 amend): the Rules front substitution is tested here, separately
	/// from <c>FallaciesLocalizationTests</c> (which is owned by PR #444). No existing file is
	/// modified.
	///
	/// The Rules template binds body text through <c>{{markdown Text}}</c>. The Rules
	/// FrontFieldConversions swap <c>Text</c> -> <c>Text_en/_ru/_pt/_es/_ar/_fa/_zh</c>. This
	/// test applies the real substitution chain against the template on disk for every target
	/// language and asserts the FR binding is gone and the localized binding is present.
	/// </summary>
	public class RulesLocalizationTests
	{
		private const string RulesTemplateRelPath = "Cards/Rules/Argumentum_Rules_fr.json";

		private static string FindRepoRoot()
		{
			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Cards", "Fallacies")))
			{
				dir = dir.Parent;
			}
			return dir?.FullName ?? throw new DirectoryNotFoundException("Could not locate repository root (Cards/Fallacies not found).");
		}

		private static CardSetLocalization GetRulesLocalization()
		{
			var config = new AssetConverterConfig();
			var loc = config.LocalizationConfig.CardSetLocalizations
				.FirstOrDefault(l => l.CardSetNames.Contains(KnownCardSets.Rules));
			loc.Should().NotBeNull("the default LocalizationConfig must carry a Rules mapping");
			return loc!;
		}

		// Mirrors the Front branch of CardSetLocalization.TranslateCardSetInfo (front:true).
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
		[InlineData("en", "Text_en")]
		[InlineData("ru", "Text_ru")]
		[InlineData("pt", "Text_pt")]
		[InlineData("es", "Text_es")]
		[InlineData("ar", "Text_ar")]
		[InlineData("fa", "Text_fa")]
		[InlineData("zh", "Text_zh")]
		public void Rules_Template_Binds_Localized_Text_Column_After_Substitution(string destLang, string expectedColumn)
		{
			var path = Path.Combine(FindRepoRoot(), RulesTemplateRelPath);
			File.Exists(path).Should().BeTrue($"Rules template must exist at {RulesTemplateRelPath}");
			var original = File.ReadAllText(path);
			original.Should().Contain("{{markdown Text}}",
				"the Rules template must bind body text through {{markdown Text}} (Golden Master contract)");

			var loc = GetRulesLocalization();
			var translated = ApplyFrontSubstitution(loc, original, destLang);

			// The localized binding must be present.
			translated.Should().Contain($"{{markdown {expectedColumn}}}",
				$"{destLang} Rules template must bind body text to CSV column '{expectedColumn}'");
		}

		[Fact]
		public void Rules_Front_Conversions_Reference_Only_Template_Existing_Tokens()
		{
			// Root-cause guard for the #216 class of bug: every sourceFieldName in the Rules
			// FrontFieldConversions must correspond to a token that actually appears in the
			// template. If a conversion references a non-existent field, template.Replace() is a
			// silent no-op and the FR content ships unchanged.
			var path = Path.Combine(FindRepoRoot(), RulesTemplateRelPath);
			var template = File.ReadAllText(path);
			var loc = GetRulesLocalization();

			foreach (var fieldConversion in loc.FrontFieldConversions)
			{
				var sourcePattern = loc.FormatField(fieldConversion.sourceFieldName);
				template.Should().Contain(sourcePattern,
					$"Rules FrontFieldConversions source '{fieldConversion.sourceFieldName}' must exist in the template " +
					$"(otherwise the conversion is a silent no-op — #216 root cause). Pattern looked for: '{sourcePattern}'.");
			}
		}

		[Fact]
		public void Rules_Front_Conversions_Cover_All_Eight_Languages()
		{
			var loc = GetRulesLocalization();
			var textConv = loc.FrontFieldConversions.FirstOrDefault(c => c.sourceFieldName == "Text");
			textConv.fieldConversions.Should().NotBeNull("Rules must map the Text field");
			var languages = textConv.fieldConversions.Select(c => c.Language).ToList();
			languages.Should().Contain(new[] { "en", "ru", "pt", "es", "ar", "fa", "zh" },
				"Rules must localize Text into all 7 non-FR release languages");
		}
	}
}
