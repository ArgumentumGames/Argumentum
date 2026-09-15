using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Regression tests for the Scenarii localization mapping — issue #204 (coverage) and the
	/// #216 class of bug (FrontFieldConversions referencing field names absent from the template).
	///
	/// NEW file (dispatch #204 amend): the Scenarii front substitution is tested here, separately
	/// from <c>FallaciesLocalizationTests</c> (owned by PR #444). No existing file is modified.
	///
	/// The Scenarii template binds through FR tokens <c>{{titre}}</c>, <c>{{catégorie}}</c>,
	/// <c>{{contexte}}</c>, <c>{{enjeu}}</c>, <c>{{baratineur}}</c>, <c>{{piocheur}}</c>. The
	/// Scenarii FrontFieldConversions swap each to the localized CSV column. These tests apply
	/// the real substitution chain against the template on disk and assert each FR binding is
	/// replaced by its localized counterpart for every release language.
	/// </summary>
	public class ScenariiLocalizationTests
	{
		private const string ScenariiFaceTemplateRelPath = "Cards/Scenarii/Argumentum_Scenarii_Face_fr.json";

		private static string FindRepoRoot()
		{
			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "Cards", "Fallacies")))
			{
				dir = dir.Parent;
			}
			return dir?.FullName ?? throw new DirectoryNotFoundException("Could not locate repository root (Cards/Fallacies not found).");
		}

		private static CardSetLocalization GetScenariiLocalization()
		{
			var config = new AssetConverterConfig();
			var loc = config.LocalizationConfig.CardSetLocalizations
				.FirstOrDefault(l => l.CardSetNames.Contains(KnownCardSets.Scenarii));
			loc.Should().NotBeNull("the default LocalizationConfig must carry a Scenarii mapping");
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
		[InlineData("en", "title", "category", "context", "issue", "smoothTalker", "drawer")]
		[InlineData("ru", "title_ru", "category_ru", "context_ru", "issue_ru", "smoothTalker_ru", "drawer_ru")]
		[InlineData("pt", "title_pt", "category_pt", "context_pt", "issue_pt", "smoothTalker_pt", "drawer_pt")]
		public void Scenarii_Face_Template_Translates_Core_FR_Tokens_To_Target_Language(
			string destLang, string title, string category, string context, string issue, string smoothTalker, string drawer)
		{
			var path = Path.Combine(FindRepoRoot(), ScenariiFaceTemplateRelPath);
			File.Exists(path).Should().BeTrue($"Scenarii Face template must exist at {ScenariiFaceTemplateRelPath}");
			var original = File.ReadAllText(path);

			// Golden-Master contract: the FR template binds these exact tokens.
			original.Should().Contain("{{titre}}", "Scenarii Face must bind titre");
			original.Should().Contain("{{catégorie}}", "Scenarii Face must bind catégorie (accented FR header)");

			var loc = GetScenariiLocalization();
			var translated = ApplyFrontSubstitution(loc, original, destLang);

			translated.Should().Contain($"{{{{{title}}}}}", $"{destLang} Scenarii must bind titre → {title}");
			translated.Should().Contain($"{{{{{category}}}}}", $"{destLang} Scenarii must bind catégorie → {category}");
			translated.Should().Contain($"{{{{{context}}}}}", $"{destLang} Scenarii must bind contexte → {context}");
			translated.Should().Contain($"{{{{{issue}}}}}", $"{destLang} Scenarii must bind enjeu → {issue}");
			translated.Should().Contain($"{{{{{smoothTalker}}}}}", $"{destLang} Scenarii must bind baratineur → {smoothTalker}");
			translated.Should().Contain($"{{{{{drawer}}}}}", $"{destLang} Scenarii must bind piocheur → {drawer}");

			// The FR tokens must no longer be present as bindings.
			translated.Should().NotContain("{{titre}}", $"{destLang} Scenarii must no longer bind FR titre");
			translated.Should().NotContain("{{catégorie}}", $"{destLang} Scenarii must no longer bind FR catégorie");
		}

		[Fact]
		public void Scenarii_Front_Conversions_Reference_Only_Template_Existing_Tokens()
		{
			// Root-cause guard for the #216 class of bug: every sourceFieldName in the Scenarii
			// FrontFieldConversions must correspond to a token actually present in the template.
			// A conversion referencing a non-existent field is a silent no-op (template.Replace
			// finds nothing) and the FR content ships unchanged.
			var path = Path.Combine(FindRepoRoot(), ScenariiFaceTemplateRelPath);
			var template = File.ReadAllText(path);
			var loc = GetScenariiLocalization();

			// Guard (#1046 MED #4): an empty FrontFieldConversions list would skip the loop entirely
			// and pass green — the total-loss form of the #216 bug this test exists to catch.
			loc.FrontFieldConversions.Should().NotBeEmpty(
				"Scenarii FrontFieldConversions must not be empty — an empty list skips every token check");

			foreach (var fieldConversion in loc.FrontFieldConversions)
			{
				var sourcePattern = loc.FormatField(fieldConversion.sourceFieldName);
				template.Should().Contain(sourcePattern,
					$"Scenarii FrontFieldConversions source '{fieldConversion.sourceFieldName}' must exist in the template " +
					$"(otherwise the conversion is a silent no-op — #216 root cause). Pattern looked for: '{sourcePattern}'.");
			}
		}

		[Fact]
		public void Scenarii_Has_ExceptionPatterns_For_Category_Asset_Filenames()
		{
			// Scenarii templates reference image assets by the FR category name
			// ({{rowset.[0].catégorie}}.jpg). When catégorie is localized, the asset path would
			// break — ExceptionPatterns backtrack restores the FR category inside asset refs.
			// This guard ensures the ExceptionPatterns are still declared (regression from
			// Golden Master would silently break image resolution in non-FR Scenarii).
			var loc = GetScenariiLocalization();
			loc.ExceptionPatterns.Should().NotBeEmpty(
				"Scenarii must declare ExceptionPatterns so category-based asset filenames keep resolving after localization");
			loc.ExceptionPatterns.Should().Contain(p => p.Contains("catégorie"),
				"at least one ExceptionPattern must reference the FR catégorie token used in asset filenames");
		}
	}
}
