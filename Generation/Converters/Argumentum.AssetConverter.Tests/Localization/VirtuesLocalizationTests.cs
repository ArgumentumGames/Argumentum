using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Regression tests for the Virtues localization mapping — issue #204 (coverage) and the
	/// #216 class of bug (FrontFieldConversions referencing field names absent from the template).
	///
	/// NEW file (dispatch #204 amend): the Virtues front substitution is tested here, separately
	/// from <c>FallaciesLocalizationTests</c> (owned by PR #444). No existing file is modified.
	///
	/// The Virtues template (located under Cards/Fallacies/Argumentum_Virtues_Face_fr.json)
	/// binds through <c>{{title_fr}}</c>, <c>{{description_fr}}</c>, <c>{{remark_fr}}</c> (via
	/// <c>{{breaklines remark_fr}}</c>), <c>{{family_fr}}</c>, <c>{{subfamily_fr}}</c>,
	/// <c>{{subsubfamily_fr}}</c>. The Virtues FrontFieldConversions swap each <c>_fr</c>
	/// suffix to the target language. These tests apply the real substitution chain against the
	/// template on disk and assert each FR binding is replaced by its localized counterpart.
	/// </summary>
	public class VirtuesLocalizationTests
	{
		private const string VirtuesTemplateRelPath = "Cards/Fallacies/Argumentum_Virtues_Face_fr.json";

		private static string FindRepoRoot()
		{
			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Cards", "Fallacies")))
			{
				dir = dir.Parent;
			}
			return dir?.FullName ?? throw new DirectoryNotFoundException("Could not locate repository root (Cards/Fallacies not found).");
		}

		private static CardSetLocalization GetVirtuesLocalization()
		{
			var config = new AssetConverterConfig();
			var loc = config.LocalizationConfig.CardSetLocalizations
				.FirstOrDefault(l => l.CardSetNames.Contains(KnownCardSets.Virtues));
			loc.Should().NotBeNull("the default LocalizationConfig must carry a Virtues mapping");
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
		[InlineData("en")]
		[InlineData("ru")]
		[InlineData("pt")]
		[InlineData("es")]
		[InlineData("ar")]
		[InlineData("fa")]
		[InlineData("zh")]
		public void Virtues_Template_Translates_All_FR_Placeholders_To_Target_Language(string destLang)
		{
			var path = Path.Combine(FindRepoRoot(), VirtuesTemplateRelPath);
			File.Exists(path).Should().BeTrue($"Virtues template must exist at {VirtuesTemplateRelPath}");
			var original = File.ReadAllText(path);

			var loc = GetVirtuesLocalization();
			var translated = ApplyFrontSubstitution(loc, original, destLang);

			var suffix = $"_{destLang}";

			// Every FR field that carries localized CSV content must be swapped to the target language.
			foreach (var frField in new[] { "title_fr", "description_fr", "remark_fr", "family_fr", "subfamily_fr", "subsubfamily_fr" })
			{
				var destField = frField.Replace("_fr", suffix);
				translated.Should().Contain(destField,
					$"{destLang} Virtues template must bind {frField} → {destField}");
			}
		}

		[Fact]
		public void Virtues_Front_Conversions_Reference_Only_Template_Existing_Tokens()
		{
			// Root-cause guard for the #216 class of bug: every sourceFieldName in the Virtues
			// FrontFieldConversions must correspond to a token actually present in the template.
			// A conversion referencing a non-existent field is a silent no-op (template.Replace
			// finds nothing) and the FR content ships unchanged.
			var path = Path.Combine(FindRepoRoot(), VirtuesTemplateRelPath);
			var template = File.ReadAllText(path);
			var loc = GetVirtuesLocalization();

			// Guard (#1046 MED #5): an empty FrontFieldConversions list would skip the loop entirely
			// and pass green — the total-loss form of the #216 bug this test exists to catch.
			loc.FrontFieldConversions.Should().NotBeEmpty(
				"Virtues FrontFieldConversions must not be empty — an empty list skips every token check");

			foreach (var fieldConversion in loc.FrontFieldConversions)
			{
				var sourcePattern = loc.FormatField(fieldConversion.sourceFieldName);
				template.Should().Contain(sourcePattern,
					$"Virtues FrontFieldConversions source '{fieldConversion.sourceFieldName}' must exist in the template " +
					$"(otherwise the conversion is a silent no-op — #216 root cause). Pattern looked for: '{sourcePattern}'.");
			}
		}

		[Fact]
		public void Virtues_Family_Subtokens_Resist_Suffix_Overlap_Under_Config_Order()
		{
			// Note on ordering: unlike the Fallacies Memo Back (where Famille/Sous-Famille/
			// Soussousfamille MUST be ordered most-specific-first because a shorter suffix can
			// clobber a longer token), the Virtues config is NOT ordered most-specific-first
			// (family_fr before subfamily_fr before subsubfamily_fr). This is still correct
			// because: FormatField appends "}}", and although "family_fr}}" IS a suffix-substring
			// of "subfamily_fr}}", replacing it first yields "{{subfamily_en}}" — and since the
			// destination also carries the "_en" suffix, the subsequent subfamily/subsubfamily
			// steps become harmless no-ops (they no longer match). The end bindings are correct.
			// This test pins that invariant empirically: under the actual config order, every
			// FR family token ends up bound to its localized column with NO residual FR suffix.
			var loc = GetVirtuesLocalization();
			var template = "{{family_fr}} {{subfamily_fr}} {{subsubfamily_fr}}";

			foreach (var destLang in new[] { "en", "ru", "pt", "es", "ar", "fa", "zh" })
			{
				var translated = ApplyFrontSubstitution(loc, template, destLang);
				var suffix = $"_{destLang}";
				translated.Should().Contain($"family{suffix}", $"{destLang}: family must be localized");
				translated.Should().Contain($"subfamily{suffix}", $"{destLang}: subfamily must be localized");
				translated.Should().Contain($"subsubfamily{suffix}", $"{destLang}: subsubfamily must be localized");
				translated.Should().NotContain("family_fr", $"{destLang}: no residual FR family token after conversion");
			}
		}
	}
}
