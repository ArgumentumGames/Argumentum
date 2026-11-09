using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.CardPenConfiguration
{
	/// <summary>
	/// Contract: every card template declared in <see cref="WebBasedGeneratorConfig"/> must use
	/// real CSS generic font families. The misspelling <c>sans-setif</c> (found 2026-10-19 in the
	/// live Memo face, fallback of `.title { font-family:'Bebas Neue', sans-setif; }`, #1485
	/// family) names a family that does not exist: the browser treats it as an unknown name and
	/// silently falls back to the inherited default, so the intended generic fallback never
	/// applies when Bebas Neue is unavailable — the declaration reads as a working fallback while
	/// being a no-op. The same file already carries the correct form in its #1225 counter-rule
	/// (`'Bebas Neue', sans-serif !important`), which is how the two spellings coexisted unnoticed.
	///
	/// <para><b>Scope is derived, not tabulated.</b> The live-template list is read from
	/// <see cref="WebBasedGeneratorConfig"/> exactly like
	/// <see cref="BrandFontCounterRuleContractTests"/> — both the Debug path (worktree) and the
	/// Release path (raw master URL) normalise to the same repo-relative file, so adding a CardSet
	/// automatically extends the guard.</para>
	///
	/// <para><b>Frozen copies deliberately keep the typo.</b> The three templates under
	/// <c>Cards/Memo/Archive/2022/</c> are a historical export and stay byte-intact (ai-01,
	/// 2026-10-19: « Archive/2022/ intacte »); they are pinned <i>as still carrying</i> the typo so
	/// a well-meaning sweep that "fixes" the archive fails loudly instead of rewriting history.
	/// <c>Generation/CardPen/examples/</c> (fork demo material, never consumed by the pipeline)
	/// also keeps its copies — documented here, deliberately not pinned.</para>
	/// </summary>
	public class MemoGenericFontFallbackContractTests
	{
		private const string LiveMemoFaceRelPath = "Cards/Memo/Argumentum_Memo_Face_fr.json";

		/// <summary>The exact misspelling this guard exists for. Literal, case-insensitive.</summary>
		private static readonly Regex MisspeltGeneric = new Regex(
			@"sans-setif",
			RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// The corrected base rule: `.title { font-family:'Bebas Neue', sans-serif; … }`. Anchored on
		/// the rule block so the correct spelling elsewhere in the same stylesheet (`.familyContainer`,
		/// `.familyDesc`, the #1225 counter-rule) cannot satisfy the witness on its own.
		/// </summary>
		private static readonly Regex TitleFallback = new Regex(
			@"\.title\s*\{[^}]*font-family\s*:\s*'Bebas Neue',\s*sans-serif\s*;",
			RegexOptions.Compiled);

		private static readonly string[] FrozenArchiveRelPaths =
		{
			"Cards/Memo/Archive/2022/Argumentum_Memo_Face_English.json",
			"Cards/Memo/Archive/2022/Argumentum_Memo_Face_Francais.json",
			"Cards/Memo/Archive/2022/Argumentum_Memo_Face_Francais_Print_and_Play.json",
		};

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
		/// Anti-no-op: the derivation must still see templates, and must still see the live Memo
		/// face specifically — the file that carried the typo. A config reshuffle that drops it
		/// (CardSet renamed, path moved) would otherwise leave the Theory green on a scope that no
		/// longer contains the template this guard was written for.
		/// </summary>
		[Fact]
		public void DeclaredTemplates_IncludeLiveMemoFace()
		{
			var templates = ActiveTemplates().Select(o => (string)o[0]).ToList();

			templates.Should().NotBeEmpty(
				"the guard reads its scope from WebBasedGeneratorConfig.CardSets. An empty scope means " +
				"the config shape changed and the Theory has silently become a no-op.");

			templates.Should().Contain(t => string.Equals(t, LiveMemoFaceRelPath, StringComparison.OrdinalIgnoreCase),
				"{0} is the template this guard was written for (the `sans-setif` carrier, 2026-10-19). " +
				"It is declared in WebBasedGeneratorConfig; if it moved, update the constant — do not let " +
				"the guard drift away from its subject.",
				LiveMemoFaceRelPath);
		}

		/// <summary>
		/// Witness of the fix: the live Memo face's `.title` rule carries the corrected generic
		/// fallback. Without this, deleting the (fixed) rule outright would satisfy the "no
		/// misspelling" negative while removing the declaration the fix exists to correct.
		/// </summary>
		[Fact]
		public void LiveMemoFace_TitleFallbackIsSansSerif()
		{
			var path = Path.Combine(TestRepoRoot.Find(), LiveMemoFaceRelPath);
			File.Exists(path).Should().BeTrue("{0} must exist on disk.", LiveMemoFaceRelPath);

			using var doc = JsonDocument.Parse(File.ReadAllText(path));
			var css = doc.RootElement.GetProperty("css").GetString() ?? "";

			TitleFallback.IsMatch(css).Should().BeTrue(
				"the live Memo face `.title` rule must declare 'Bebas Neue' with the real generic " +
				"fallback `sans-serif` (fixed 2026-11-09; the misspelling `sans-setif` named a family " +
				"that does not exist, so the fallback silently resolved to the inherited default).");
		}

		[Theory]
		[MemberData(nameof(ActiveTemplates))]
		public void DeclaredTemplate_DeclaresNoMisspeltGenericFamily(string relPath, string usedBy)
		{
			var path = Path.Combine(TestRepoRoot.Find(), relPath);
			File.Exists(path).Should().BeTrue(
				"{0} (used by {1}) is declared in WebBasedGeneratorConfig but is not on disk.",
				relPath, usedBy);

			string css;
			string mustache;
			try
			{
				using var doc = JsonDocument.Parse(File.ReadAllText(path));
				css = doc.RootElement.GetProperty("css").GetString() ?? "";
				mustache = doc.RootElement.GetProperty("mustache").GetString() ?? "";
			}
			catch (JsonException)
			{
				// A template whose JSON is not parseable as-is cannot be linted for a font
				// declaration (e.g. the Virtues face carries a literal newline); skip — same
				// semantics as BrandFontCounterRuleContractTests.
				return;
			}

			MisspeltGeneric.IsMatch(css).Should().BeFalse(
				"{0} (used by {1}) declares the misspelt generic family `sans-setif`. The family does " +
				"not exist, so the fallback silently resolves to the inherited default; the real " +
				"generic is `sans-serif`.",
				relPath, usedBy);

			MisspeltGeneric.IsMatch(mustache).Should().BeFalse(
				"{0} (used by {1}): the same misspelling is checked in the mustache HTML — a style " +
				"attribute or an inline <style> block would carry the identical silent no-op.",
				relPath, usedBy);
		}

		/// <summary>
		/// Boundary pin: the frozen archive copies keep the typo they were exported with. This is
		/// what makes "fix the live template" a scoped gesture rather than a sweep — an edit to
		/// <c>Archive/2022/</c> fails here even though it would look like the same correction.
		/// </summary>
		[Fact]
		public void FrozenArchiveCopies_KeepTheirHistoricalTypo()
		{
			foreach (var rel in FrozenArchiveRelPaths)
			{
				var path = Path.Combine(TestRepoRoot.Find(), rel);
				File.Exists(path).Should().BeTrue(
					"{0} is a frozen 2022 export pinned by this guard; it cannot be deleted as part of " +
					"a font-fallback fix.", rel);

				var raw = File.ReadAllText(path);
				MisspeltGeneric.IsMatch(raw).Should().BeTrue(
					"{0} is a frozen historical export (ai-01, 2026-10-19: « Archive/2022/ intacte »). " +
					"It keeps the typo it was exported with; correcting the live template must not " +
					"rewrite the archive — that would falsify the historical record the typo dates from.",
					rel);
			}
		}
	}
}
