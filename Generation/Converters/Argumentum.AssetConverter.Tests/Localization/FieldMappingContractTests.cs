using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Contract tests for the #216 root cause, pinned via the pure extraction
	/// <see cref="CardSetLocalization.ApplyFieldConversions"/> /
	/// <see cref="CardSetLocalization.FindAbsentSourceFields"/>.
	///
	/// #216 in one line: <c>LocalizationConfig.FrontFieldConversions</c> did
	/// <c>template.Replace(sourceField, destField)</c> on field names that were ABSENT from the
	/// Mustache template. <c>string.Replace</c> is silent when the pattern is missing (returns the
	/// string unchanged), so every non-FR PDF silently kept its French content with no error — a
	/// silent false-pass. These tests pin the two halves of the contract:
	///   (a) a mapping whose source field EXISTS in the template → it is replaced;
	///   (b) a mapping whose source field is ABSENT → it is silently skipped by ApplyFieldConversions
	///       (the hazard) BUT surfaced by FindAbsentSourceFields (the fail-loud companion);
	///   (c) Fallacies/Rules/Virtues/Scenarii lockstep — every mapped source field must be present
	///       in a real template (the regression guard that would have caught #216).
	///
	/// The four sibling test files (Fallacies/Rules/Virtues/Scenarii LocalizationTests) each
	/// re-implement the apply loop inline; this file drives the shared pure methods directly.
	/// Migrating those helpers onto <see cref="CardSetLocalization.ApplyFieldConversions"/> is a
	/// follow-up (single source of truth), out of scope for this gate-safe extraction.
	/// </summary>
	public class FieldMappingContractTests
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

		private static string ReadTemplate(string relPath) => File.ReadAllText(Path.Combine(RepoRoot, relPath));

		private static CardSetLocalization GetLocalization(string cardSet)
		{
			var config = new AssetConverterConfig();
			var loc = config.LocalizationConfig.CardSetLocalizations
				.FirstOrDefault(l => l.CardSetNames.Contains(cardSet));
			loc.Should().NotBeNull($"the default LocalizationConfig must carry a {cardSet} mapping");
			return loc!;
		}

		// Synthetic field-conversion list matching the production tuple shape.
		private static List<(string sourceFieldName, List<(string Language, string destFieldName)> fieldConversions)> Conversions(
			params (string source, string dest)[] enMappings)
			=> enMappings
				.Select(m => (m.source, new List<(string Language, string destFieldName)> { ("en", m.dest) }))
				.ToList();

		// ---- (a) source field PRESENT in template → replaced --------------------------------

		[Fact]
		public void ApplyFieldConversions_PresentField_IsReplaced()
		{
			const string template = "<div>{{Titre}}</div>";

			var result = CardSetLocalization.ApplyFieldConversions(template, Conversions(("Titre", "Title")), null, "en");

			result.Should().Be("<div>{{Title}}</div>", "the present source token Titre}} must be rewritten to Title}}");
		}

		[Fact]
		public void ApplyFieldConversions_ReplacesAllOccurrences_NotJustFirst()
		{
			// template.Replace replaces every occurrence — the contract is global, not first-match.
			const string template = "{{Titre}} and again {{Titre}}";

			var result = CardSetLocalization.ApplyFieldConversions(template, Conversions(("Titre", "Title")), exceptionPatterns: null, "en");

			result.Should().Be("{{Title}} and again {{Title}}");
		}

		// ---- (b) source field ABSENT → silent skip (hazard) + surfaced by fail-loud --------

		[Fact]
		public void ApplyFieldConversions_AbsentField_IsSilentlySkipped_NoThrow()
		{
			// This IS the #216 footgun, pinned explicitly: a field absent from the template is
			// silently skipped by template.Replace (no error, no log) — so a wrong mapping name
			// leaves content untranslated with zero signal. FindAbsentSourceFields is the remedy.
			const string template = "<div>{{Titre}}</div>";

			var act = () => CardSetLocalization.ApplyFieldConversions(
				template, Conversions(("Titre", "Title"), ("GhostField", "RealField")), null, "en");

			act.Should().NotThrow("an absent source pattern must never raise");
			act().Should().Be("<div>{{Title}}</div>",
				"Titre is replaced and the absent GhostField is a silent no-op (the #216 hazard)");
		}

		[Fact]
		public void FindAbsentSourceFields_ReportsAbsentField_AndOmitsPresentOne()
		{
			const string template = "<div>{{Titre}}</div>";

			var absent = CardSetLocalization.FindAbsentSourceFields(
				template, Conversions(("Titre", "Title"), ("GhostField", "RealField")), "en");

			absent.Should().ContainSingle(f => f == "GhostField",
				"the fail-loud companion must surface exactly the absent source field");
			absent.Should().NotContain("Titre", "a present source field must not be flagged absent");
		}

		[Fact]
		public void FindAbsentSourceFields_ReturnsEmpty_WhenEveryFieldIsPresent()
		{
			const string template = "<div>{{Titre}}{{Desc}}</div>";

			var absent = CardSetLocalization.FindAbsentSourceFields(
				template, Conversions(("Titre", "Title"), ("Desc", "Description")), "en");

			absent.Should().BeEmpty("no source field is absent when all tokens are present");
		}

		[Fact]
		public void ApplyFieldConversions_NoConversionForDestLang_SkipsSilently()
		{
			// A field with no dest-lang entry is skipped — only fields carrying the requested
			// language are substituted (mirrors TranslateCardSetInfo's FirstOrDefault(destLang) guard).
			const string template = "<div>{{Titre}}</div>";
			var conversions = new List<(string, List<(string, string)>)>
			{
				("Titre", new List<(string, string)> { ("ru", "Zagolovok") }), // no "en" entry
			};

			var result = CardSetLocalization.ApplyFieldConversions(template, conversions, null, "en");

			result.Should().Be(template, "a field without an 'en' conversion must be left untouched");
		}

		[Fact]
		public void FindAbsentSourceFields_NoConversionForDestLang_IsNotReportedAbsent()
		{
			// Symmetric to the above: a field with no dest-lang entry is neither replaced nor
			// reported absent — it is simply out of scope for this language.
			const string template = "<div>{{Titre}}</div>";
			var conversions = new List<(string, List<(string, string)>)>
			{
				("Titre", new List<(string, string)> { ("ru", "Zagolovok") }),
			};

			var absent = CardSetLocalization.FindAbsentSourceFields(template, conversions, "en");

			absent.Should().BeEmpty("a field with no 'en' conversion is out of scope, not absent");
		}

		// ---- (c) CardSet ↔ template lockstep (the #216 regression guard) -------------------

		// InlineData requires compile-time constants; KnownCardSets.* are static readonly, so the
		// string literals mirror their values ("Fallacies"/"Virtues"/"Rules"/"Scenarii").
		[Theory]
		[InlineData("Fallacies", "Cards/Fallacies/Argumentum_Fallacies_Face_fr.json")]
		[InlineData("Virtues", "Cards/Fallacies/Argumentum_Virtues_Face_fr.json")]
		[InlineData("Rules", "Cards/Rules/Argumentum_Rules_fr.json")]
		[InlineData("Scenarii", "Cards/Scenarii/Argumentum_Scenarii_Face_fr.json")]
		public void EveryMappedFrontField_IsPresentInRealTemplate_NoSilentFalsePass(string cardSet, string templateRelPath)
		{
			// THE #216 REGRESSION GUARD. For each CardSet, every source field mapped for 'en' must
			// actually appear in the representative template. If a mapping references a field that
			// is not in the template, FindAbsentSourceFields returns it here and the test fails loud
			// — exactly the signal #216 lacked. (This would have caught the original bug: the
			// Fallacies mapping named "Titre"/"Definition"/"Exemple" which the templates never had.)
			var loc = GetLocalization(cardSet);
			loc.FrontFieldConversions.Should().NotBeEmpty($"{cardSet} must carry front field conversions");
			var template = ReadTemplate(templateRelPath);

			var absent = CardSetLocalization.FindAbsentSourceFields(template, loc.FrontFieldConversions, "en");

			absent.Should().BeEmpty(
				$"every 'en'-mapped front field of {cardSet} must exist in {templateRelPath}; " +
				$"absent fields = [{string.Join(", ", absent)}] would be silently skipped → untranslated content (#216)");
		}

		[Fact]
		public void ApplyFieldConversions_OnRealFallaciesTemplate_ReplacesFrenchPlaceholders()
		{
			// Grounding: the pure method, fed the real Fallacies config + template, performs the
			// same #216 fix as the production path (text_fr → text_en etc.) — i.e. the extraction
			// is behaviour-preserving, not just structurally pure.
			var loc = GetLocalization(KnownCardSets.Fallacies);
			var template = ReadTemplate("Cards/Fallacies/Argumentum_Fallacies_Face_fr.json");

			var result = CardSetLocalization.ApplyFieldConversions(template, loc.FrontFieldConversions, loc.ExceptionPatterns, "en");

			result.Should().NotContain("{{text_fr}}", "the pure method must replace text_fr (the #216 token) on the real template");
			result.Should().NotContain("{{desc_fr}}", "the pure method must replace desc_fr on the real template");
			result.Should().Contain("text_en", "the 'en' replacement token must be present");
			result.Should().Contain("desc_en", "the 'en' replacement token must be present");
		}
	}
}
