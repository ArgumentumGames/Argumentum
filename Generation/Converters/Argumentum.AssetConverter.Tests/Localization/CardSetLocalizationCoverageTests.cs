using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Regression guard for issue #1141 — three CardSets declared in
	/// <c>WebBasedGeneratorConfig</c> (FallaciesPrintAndPlayLight, VirtuesPrintAndPlayLight,
	/// ScenariiPrintAndPlayFull) belonged to NO CardSetLocalizations group, so
	/// <c>LocalizationConfig.TranslateCardSet</c> resolved no match, logged a warning nobody
	/// read, and rendered the FR template verbatim: 14 of 80 bundle PDFs shipped French
	/// content under a foreign-language suffix — across every bundle ever produced, not a
	/// regression of #1130/#1132.
	///
	/// A warning that nobody reads is not a guard. This test fails at the moment a CardSet is
	/// declared without a localization group — including the next CardSet someone adds.
	///
	/// It lives in this project (not VisualTests) because this is the suite CI actually runs.
	/// </summary>
	public class CardSetLocalizationCoverageTests
	{
		[Fact]
		public void Every_Declared_CardSet_Belongs_To_A_Localization_Group()
		{
			var config = new AssetConverterConfig();

			var declared = config.WebBasedGeneratorConfig.CardSets
				.Select(cs => cs.Name)
				.Where(n => !string.IsNullOrWhiteSpace(n))
				.Distinct()
				.ToList();

			var covered = config.LocalizationConfig.CardSetLocalizations
				.SelectMany(l => l.CardSetNames)
				.Distinct()
				.ToList();

			declared.Should().NotBeEmpty("WebBasedGeneratorConfig must declare its card sets");
			covered.Should().NotBeEmpty("LocalizationConfig must carry at least one group");

			var orphans = declared.Where(n => !covered.Contains(n)).ToList();
			orphans.Should().BeEmpty(
				$"every CardSet declared in WebBasedGeneratorConfig must belong to a CardSetLocalizations " +
				$"group, otherwise TranslateCardSet skips localization and the set ships FRENCH content " +
				$"in all non-FR languages (#1141, same silent mode as #216). Orphans: {string.Join(", ", orphans)}");
		}

		[Fact]
		public void Localization_Groups_Reference_Only_Declared_CardSets()
		{
			// Inverse direction: a group naming a CardSet that no longer exists would make the
			// coverage test above under-report (coverage of a ghost set is not coverage).
			var config = new AssetConverterConfig();

			var declared = config.WebBasedGeneratorConfig.CardSets
				.Select(cs => cs.Name)
				.Where(n => !string.IsNullOrWhiteSpace(n))
				.ToHashSet();

			var ghosts = config.LocalizationConfig.CardSetLocalizations
				.SelectMany(l => l.CardSetNames)
				.Where(n => !declared.Contains(n))
				.ToList();

			ghosts.Should().BeEmpty(
				$"CardSetLocalizations groups must only reference CardSets that WebBasedGeneratorConfig " +
				$"actually declares (a stale entry masks real coverage). Ghosts: {string.Join(", ", ghosts)}");
		}

		[Fact]
		public void Virtues_Group_Carries_BackFieldConversions_For_The_Shared_Fallacies_Back()
		{
			// #1141 defect B: the Virtues deck reuses the Fallacies back template
			// (Argumentum_Fallacies_Back_fr.json, mustache {{tagline_fr}}). Without a
			// BackFieldConversions entry the tagline conversion of #1130 exists only in the
			// Fallacies group and every non-FR Virtues back renders the FRENCH tagline
			// (measured on the 2026-08-23 bundle: 3 SHA / 8 languages).
			var config = new AssetConverterConfig();
			var loc = config.LocalizationConfig.CardSetLocalizations
				.FirstOrDefault(l => l.CardSetNames.Contains(KnownCardSets.Virtues));

			loc.Should().NotBeNull("the default LocalizationConfig must carry a Virtues group");

			var backSources = loc!.BackFieldConversions
				.Select(c => c.sourceFieldName)
				.ToList();

			backSources.Should().Contain("tagline_fr",
				"the Virtues back is the Fallacies back ({{tagline_fr}}) — without the conversion the " +
				"tagline stays FR in all non-FR languages (#1141)");
		}
	}
}
