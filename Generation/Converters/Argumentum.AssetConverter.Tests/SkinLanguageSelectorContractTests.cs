using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Contract of the #458 language-switcher grain: the visible switcher lists the 8 cultures the
	/// portal serves (fr, en, es, pt, ru, zh, ar, fa), not a 5-culture subset.
	///
	/// Why it exists: <c>LanguageNavigation.ascx</c> renders whatever its <c>Languages</c> attribute
	/// names — it never reads the portal's locale list — and that attribute is hardcoded in THREE
	/// places (theme-body.ascx once, Landingpage.ascx twice: offcanvas + desktop). Measured
	/// 2026-10-26 on the preprod: 5 entries (FR, EN, ES, PT, RU) on all 8 language routes while all
	/// 8 routes answered 200, so the zh/ar/fa cultures existed and were simply not listed. A list
	/// that lives in three copies drifts by construction; the guard pins all three at once.
	///
	/// The predicates are pure functions of the template text and each is exercised on a mutated
	/// copy as well, so a predicate that matched nothing cannot read as a pass.
	/// </summary>
	public class SkinLanguageSelectorContractTests
	{
		private static readonly string SkinDir = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "_default", "Skins", "2shineBS5");

		private static readonly string[] MarkupFiles =
		{
			Path.Combine(SkinDir, "controls", "theme-body.ascx"),
			Path.Combine(SkinDir, "Landingpage.ascx"),
		};

		/// <summary>
		/// Expected entries, in render order: the five historical entries first, then the three
		/// cultures added when the minimal chrome shipped. Order is part of the contract — the
		/// switcher renders in attribute order and reordering it silently moves every link.
		/// </summary>
		private static readonly string[] ExpectedEntries =
		{
			"fr-FR:FR", "en-US:EN", "es-ES:ES", "pt-BR:PT", "ru-RU:RU",
			"zh-CN:ZH", "ar-SA:AR", "fa-IR:FA",
		};

		private static readonly Regex LanguagesAttribute =
			new Regex(@"languagenavigation\b[^>]*?Languages\s*=\s*""([^""]*)""",
				RegexOptions.IgnoreCase | RegexOptions.Singleline);

		private static string[] SelectorValues(string markup) =>
			LanguagesAttribute.Matches(markup).Cast<Match>().Select(m => m.Groups[1].Value).ToArray();

		/// <summary>
		/// Returns null when the value is a well-formed 8-entry selector, or a short reason when it
		/// is not. Pure, so the same predicate can be pointed at a mutant.
		/// </summary>
		private static string? ValidateSelector(string value)
		{
			var entries = value.Split(',').Select(e => e.Trim()).ToArray();
			if (entries.Length != ExpectedEntries.Length)
			{
				return "expected " + ExpectedEntries.Length + " entries, found " + entries.Length;
			}
			if (entries.Distinct(StringComparer.Ordinal).Count() != entries.Length)
			{
				return "duplicate culture in the selector";
			}
			for (var i = 0; i < entries.Length; i++)
			{
				var parts = entries[i].Split(':');
				if (parts.Length != 2 || parts[0].Length == 0 || parts[1].Length == 0)
				{
					return "malformed entry '" + entries[i] + "' (expected culture:LABEL)";
				}
				if (!string.Equals(entries[i], ExpectedEntries[i], StringComparison.Ordinal))
				{
					return "entry " + i + " is '" + entries[i] + "', expected '" + ExpectedEntries[i] + "'";
				}
			}
			return null;
		}

		/// <summary>
		/// Three occurrences, one in theme-body and two in Landingpage. Asserted by count so that a
		/// renamed attribute, a removed control or a fourth copy added elsewhere cannot pass silently.
		/// </summary>
		[Fact]
		public void TheThreeHardcodedListsAllNameTheEightCultures()
		{
			var all = new List<(string File, string Value)>();
			foreach (var file in MarkupFiles)
			{
				var values = SelectorValues(File.ReadAllText(file));
				Assert.True(values.Length > 0, "no Languages attribute found in " + file);
				all.AddRange(values.Select(v => (Path.GetFileName(file), v)));
			}

			Assert.True(all.Count == 3,
				"expected exactly 3 hardcoded language lists (1 in theme-body.ascx, 2 in Landingpage.ascx), found "
				+ all.Count + ": " + string.Join(" | ", all.Select(a => a.File)));

			foreach (var (file, value) in all)
			{
				var error = ValidateSelector(value);
				Assert.True(error == null, file + ": " + error + " — value was \"" + value + "\"");
			}
		}

		[Fact]
		public void AllThreeListsAreIdentical()
		{
			var values = MarkupFiles.SelectMany(f => SelectorValues(File.ReadAllText(f))).Distinct(StringComparer.Ordinal).ToArray();
			Assert.True(values.Length == 1,
				"the three lists disagree — the switcher would render differently per template: "
				+ string.Join(" | ", values));
		}

		/// <summary>
		/// The switcher offers what the portal serves: every listed culture must be one of the 8 the
		/// site answers on, and the 8 must all be there. Named separately from the exact-order check
		/// because this is the invariant a reviewer reads, while the order check is the implementation
		/// detail that keeps the rendered menu stable.
		/// </summary>
		[Fact]
		public void TheListedCulturesAreExactlyTheOnesThePortalServes()
		{
			var value = SelectorValues(File.ReadAllText(MarkupFiles[0])).Single();
			var cultures = value.Split(',').Select(e => e.Split(':')[0]).ToArray();
			Assert.Equal(
				new[] { "fr-FR", "en-US", "es-ES", "pt-BR", "ru-RU", "zh-CN", "ar-SA", "fa-IR" }.OrderBy(c => c, StringComparer.Ordinal),
				cultures.OrderBy(c => c, StringComparer.Ordinal));
		}

		// --- inverse controls: the predicate must go RED on the shapes it is meant to reject ---

		[Fact]
		public void TheFiveCultureListThisGrainReplacesIsRejected()
		{
			Assert.NotNull(ValidateSelector("fr-FR:FR,en-US:EN,es-ES:ES,pt-BR:PT,ru-RU:RU"));
		}

		[Fact]
		public void ADuplicateCultureIsRejected()
		{
			// 8 entries but one culture twice, one missing: length alone would read as a pass.
			Assert.NotNull(ValidateSelector(
				"fr-FR:FR,en-US:EN,es-ES:ES,pt-BR:PT,ru-RU:RU,zh-CN:ZH,ar-SA:AR,ar-SA:FA"));
		}

		[Fact]
		public void AMalformedEntryIsRejected()
		{
			Assert.NotNull(ValidateSelector(
				"fr-FR:FR,en-US:EN,es-ES:ES,pt-BR:PT,ru-RU:RU,zh-CN:ZH,ar-SA,fa-IR:FA"));
		}

		[Fact]
		public void TheCurrentListIsAccepted()
		{
			Assert.Null(ValidateSelector(string.Join(",", ExpectedEntries)));
		}
	}
}
