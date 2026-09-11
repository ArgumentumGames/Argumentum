using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Source-level contract tests for the RulesExplorer culture cascade (PR #674, issue #649).
	/// The two .cshtml views are interpreted by 2sxc at DNN runtime — they are never compiled
	/// by dotnet build — so the Loc() cascade cannot be executed without the 2sxc sandbox AND
	/// the Rule content-type gaining suffixed fields (a DB change gated on the 2sxc portal
	/// export). These tests pin the SOURCE structure that the runtime gate will validate:
	/// helper presence, cascade order, FR-preserving generic fallback, and call-site routing.
	/// They are deterministic and DB-free by construction (text contract only, line endings
	/// normalized: RuleDetail is CRLF, RuleList is LF — do not match raw bytes).
	/// </summary>
	public class DnnRulesExplorerLocContractTests
	{
		private const string RuleListPath = "DNNPlatform/Portals/1/2sxc/Argumentum/_RulesExplorer_RuleList.cshtml";
		private const string RuleDetailPath = "DNNPlatform/Portals/1/2sxc/Argumentum/_RulesExplorer_RuleDetail.cshtml";

		private static string RepoRoot()
		{
			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null && !(Directory.Exists(Path.Combine(dir.FullName, "Generation")) && Directory.Exists(Path.Combine(dir.FullName, "DNNPlatform"))))
			{
				dir = dir.Parent;
			}

			dir.Should().NotBeNull("the test must run from inside the repository (walking up from the test bin to the repo root)");
			return dir!.FullName;
		}

		private static string ReadView(string relativePath) =>
			File.ReadAllText(Path.Combine(RepoRoot(), relativePath)).Replace("\r\n", "\n");

		public static TheoryData<string, string> Views => new()
		{
			{ "RuleList", RuleListPath },
			{ "RuleDetail", RuleDetailPath },
		};

		[Theory]
		[MemberData(nameof(Views))]
		public void LocHelper_ImplementsDocumentedCascadeOrder(string _, string path)
		{
			var view = ReadView(path);

			view.Should().Contain("string Loc(dynamic entity, string field)");

			// Cascade: <field>_<lang> → (if lang≠en) <field>_en → <field>_fr → <field> (generic).
			// The generic fallback is the FR-preserving difference vs FallacyExplorer (#490):
			// without it Loc() returns "" until the DB gains suffixed fields.
			var primary = view.IndexOf("pick(field + \"_\" + lang)", StringComparison.Ordinal);
			var english = view.IndexOf("pick(field + \"_en\")", StringComparison.Ordinal);
			var french = view.IndexOf("pick(field + \"_fr\")", StringComparison.Ordinal);
			var generic = view.IndexOf("return pick(field);", StringComparison.Ordinal);

			primary.Should().BeGreaterThan(0, "the primary culture pick must exist");
			english.Should().BeGreaterThan(primary, "the _en fallback must be tried after the primary culture");
			french.Should().BeGreaterThan(english, "the _fr fallback must be tried after _en");
			generic.Should().BeGreaterThan(french, "the generic (unsuffixed) fallback must come last — it preserves the current FR rendering during the DB-migration transition");

			view.Should().Contain("if (lang != \"en\")", "the _en pick is guarded to skip a redundant lookup when the culture is already en");
		}

		[Theory]
		[MemberData(nameof(Views))]
		public void LocHelper_LangSourceIsBounded(string _, string path)
		{
			var view = ReadView(path);

			// Culture comes from the server-side 2sxc context, never from user input…
			view.Should().Contain("CmsContext.Culture.CurrentCode");
			// …is reduced to its 2-letter code, checked against the 8 supported languages,
			// and falls back to fr when unsupported/empty.
			view.Should().Contain("supported.Contains(lang)");
			view.Should().Contain("lang = \"fr\"");
		}

		[Fact]
		public void RuleList_RoutesDocumentedFieldsThroughLoc()
		{
			var view = ReadView(RuleListPath);

			Count(view, "Loc(ruleEntity, \"EntityTitle\")").Should().Be(1);
			Count(view, "Loc(ruleEntity, \"Summary\")").Should().Be(1);
		}

		[Fact]
		public void RuleDetail_RoutesDocumentedFieldsThroughLoc()
		{
			var view = ReadView(RuleDetailPath);

			// Exact routing as reviewed (source-level review, PR #674): the entity fields
			// that carry localizable content all go through Loc(); conditional sections
			// (Variants, Memo) test emptiness through Loc() too, so an empty localized
			// value hides the section the same way the raw field did.
			Count(view, "Loc(ruleEntity, \"EntityTitle\")").Should().Be(3);
			Count(view, "Loc(ruleEntity, \"Summary\")").Should().Be(1);
			Count(view, "Loc(ruleEntity, \"Material\")").Should().Be(1);
			Count(view, "Loc(ruleEntity, \"Installation\")").Should().Be(1);
			Count(view, "Loc(ruleEntity, \"Content\")").Should().Be(1);
			Count(view, "Loc(ruleEntity, \"Variants\")").Should().Be(2);
			Count(view, "Loc(ruleEntity, \"Memo\")").Should().Be(2);
		}

		private static int Count(string haystack, string needle)
		{
			var count = 0;
			var index = 0;
			while ((index = haystack.IndexOf(needle, index, StringComparison.Ordinal)) >= 0)
			{
				count++;
				index += needle.Length;
			}

			return count;
		}
	}
}
