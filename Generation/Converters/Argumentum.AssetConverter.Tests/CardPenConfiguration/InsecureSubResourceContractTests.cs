using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.CardPenConfiguration
{
	/// <summary>
	/// Contract: no card template may pull a sub-resource (font, image, stylesheet, script)
	/// over plain <c>http://</c>.
	///
	/// <para><b>Why this exists.</b> Found 2026-08-26 during the go-live rehearsal of issue #1180.
	/// The DNN site's own stylesheet declared an <c>@font-face</c> whose six URLs were all
	/// <c>http://</c>; served from an HTTPS page, the browser blocks them as Mixed Content. There
	/// it was harmless — the family is declared but never selected, so the three blocked requests
	/// cost console noise and nothing else. The <i>card</i> side is where the same shape bites,
	/// and it bites silently:</para>
	/// <list type="bullet">
	///   <item>In Debug the harvest loads CardPen from <c>http://argumentum.myia.io</c> (local
	///   IIS). An <c>http://</c> font on an <c>http://</c> page is allowed — the card renders with
	///   the intended face and nothing looks wrong.</item>
	///   <item>In Release <c>JsonFilePathRelease</c> points at GitHub and CardPen is served from
	///   GitHub Pages, both HTTPS. The very same URL is now blocked, the browser silently
	///   substitutes the next font in the stack, and the PNG that goes to the printer carries the
	///   wrong typeface.</item>
	/// </list>
	/// <para>Nothing in the pipeline reports this: no exception, no missing file, no count
	/// mismatch — only glyphs that are not the ones anyone approved. The defect is visible
	/// exclusively in the configuration that ships, which is the configuration nobody renders
	/// interactively. That is precisely what a guard is for.</para>
	///
	/// <para><b>Scope is derived, not tabulated.</b> The template list is read from
	/// <see cref="WebBasedGeneratorConfig"/> itself, so adding a CardSet automatically extends the
	/// guard. A hardcoded list would rot into a guard that watches the templates we used to have —
	/// the failure mode where a check keeps passing because it stopped looking.</para>
	/// </summary>
	public class InsecureSubResourceContractTests
	{
		/// <summary>
		/// Matches <c>http://</c> only in <i>sub-resource</i> position: inside <c>url(…)</c>, or as
		/// the value of <c>src=</c> / <c>href=</c>. The backslash is optional throughout because
		/// these templates are JSON, so their embedded HTML/CSS arrives with escaped quotes.
		///
		/// <para>A bare <c>http://</c> elsewhere in a template is a <b>bibliographic link</b> —
		/// fallacyfiles.org, ditext.com, skepdic.com and friends appear 150+ times across the
		/// Fallacies corpus as source citations. A hyperlink is not a sub-resource: the browser
		/// never fetches it while rendering, so it cannot trip mixed content. Matching those would
		/// make the guard cry wolf on every card and get it muted, which is the same as not having
		/// it.</para>
		/// </summary>
		private static readonly Regex InsecureSubResource = new Regex(
			@"(?:url\(\s*\\?[""']?|(?:src|href)\s*=\s*\\?[""'])http://[^\s""'\\)]+",
			RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// Every template the generator actually consumes, taken from the shipped config: both
		/// faces and backs, both the Debug and the Release declaration. The two declarations name
		/// the same file, so they collapse to one entry — but reading both means a CardSet whose
		/// Release path drifted to a different template still gets its file checked.
		/// </summary>
		private static IEnumerable<(string Label, string RelPath)> DeclaredTemplates()
		{
			var config = new WebBasedGeneratorConfig();

			foreach (var cardSet in config.CardSets)
			{
				var sides = new[]
				{
					("face", cardSet.FaceCardSetInfo),
					("back", cardSet.BackCardSetInfo),
				};

				foreach (var (side, info) in sides)
				{
					if (info == null)
					{
						continue;
					}

					foreach (var declared in new[] { info.JsonFilePathDebug, info.JsonFilePathRelease })
					{
						var rel = ToRepoRelative(declared);
						if (rel != null)
						{
							yield return ($"{cardSet.Name}/{side}", rel);
						}
					}
				}
			}
		}

		/// <summary>
		/// Collapses a declared path to a repo-relative one. Debug paths are a stack of
		/// <c>..\</c> segments relative to the build output; Release paths are
		/// raw.githubusercontent URLs. Both end with the same <c>Cards/…</c> tail, which is what
		/// identifies the file on disk. Returns null for anything not pointing into <c>Cards/</c>.
		/// </summary>
		private static string? ToRepoRelative(string? declared)
		{
			if (string.IsNullOrWhiteSpace(declared))
			{
				return null;
			}

			var normalised = declared.Replace('\\', '/');
			var idx = normalised.IndexOf("Cards/", StringComparison.OrdinalIgnoreCase);
			return idx < 0 ? null : normalised.Substring(idx);
		}

		public static IEnumerable<object[]> ActiveTemplates() =>
			DeclaredTemplates()
				.GroupBy(t => t.RelPath, StringComparer.OrdinalIgnoreCase)
				.OrderBy(g => g.Key, StringComparer.Ordinal)
				.Select(g => new object[] { g.Key, string.Join(", ", g.Select(t => t.Label).Distinct()) });

		/// <summary>
		/// Anti-no-op. A <see cref="TheoryAttribute"/> whose <see cref="MemberDataAttribute"/>
		/// yields nothing reports green, and a guard that silently stops looking is worse than no
		/// guard: it certifies. This repository has already paid for that shape twice — a
		/// self-baselining SVG test (#1112) and a <c>BeGreaterThan(0)</c> ontology assertion that
		/// stayed green while 1230 cross-links could have decayed to 3 (#497). So the scope is
		/// asserted before anything is asserted about the scope.
		/// </summary>
		[Fact]
		public void TemplateScope_IsDerivedFromConfigAndNonEmpty()
		{
			var templates = ActiveTemplates().ToList();

			templates.Should().NotBeEmpty(
				"the guard reads its scope from WebBasedGeneratorConfig.CardSets. An empty scope means " +
				"the config shape changed (CardSets renamed, JsonFilePath* moved) and the Theory below " +
				"has silently become a no-op reporting success on zero files.");

			templates.Count.Should().BeGreaterThanOrEqualTo(10,
				"the shipped config declares eleven distinct card templates. A sudden drop means the " +
				"derivation broke rather than that CardSets were deleted — check ToRepoRelative before " +
				"lowering this number.");
		}

		[Theory]
		[MemberData(nameof(ActiveTemplates))]
		public void Template_PullsNoSubResourceOverPlainHttp(string relPath, string usedBy)
		{
			var path = Path.Combine(TestRepoRoot.Find(), relPath);

			File.Exists(path).Should().BeTrue(
				"{0} is declared in WebBasedGeneratorConfig (used by {1}) but is not on disk. A template " +
				"path that does not resolve fails the harvest at runtime, not at build time.",
				relPath, usedBy);

			var offenders = InsecureSubResource.Matches(File.ReadAllText(path))
				.Select(m => m.Value)
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();

			offenders.Should().BeEmpty(
				"{0} (used by {1}) must not fetch a sub-resource over plain http://. In Debug, CardPen is " +
				"served from local IIS over http:// and such a URL loads fine — the defect is invisible. " +
				"In Release, CardPen is served from GitHub Pages over https:// and the browser BLOCKS it, " +
				"substitutes a fallback font or drops the image, and the harvest still succeeds: the wrong " +
				"pixels reach the printer with no error anywhere. Offender(s): {2}. Fix by switching the " +
				"URL to https:// (db.onlinewebfonts.com serves both), or by vendoring the asset under " +
				"Cards/. Bibliographic http:// links in card TEXT are deliberately not matched — a " +
				"hyperlink is not a sub-resource.",
				relPath, usedBy, string.Join(" | ", offenders));
		}
	}
}
