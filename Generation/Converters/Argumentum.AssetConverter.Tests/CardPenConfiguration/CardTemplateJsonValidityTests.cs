using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.CardPenConfiguration
{
	/// <summary>
	/// Two independent contracts on the shipped card gabarits, deliberately kept apart because they
	/// are enforced by <b>different parsers that disagree in both directions</b>:
	///
	/// <list type="number">
	/// <item><b>The pipeline contract</b> — <see cref="Template_LoadsWithTheParserThePipelineActuallyUses"/>.
	/// <c>CardSetInfo.GetCardSetDocument</c> (CardSetInfo.cs:66) calls <c>JsonSerializer.Deserialize</c>
	/// <b>unqualified</b>. That file's using-directives are <c>Utf8Json</c> and
	/// <c>System.Text.Json.Serialization</c> — and the latter namespace contains no <c>JsonSerializer</c>
	/// type — so the call binds to <b><c>Utf8Json.JsonSerializer</c></b>. This test therefore uses
	/// Utf8Json and the same target type. It is the only test here that can fail when a gabarit
	/// genuinely cannot be rendered.</item>
	/// <item><b>RFC 8259 hygiene</b> — <see cref="Template_IsAlsoValidJsonForEveryOtherReader"/>. Every
	/// other consumer of these files is stricter: <c>System.Text.Json</c> (used for the config at
	/// AssetConverterConfig.cs:493 and the harvest at HarvestManager.cs:931), Python <c>json.load</c>
	/// in the sync and audit scripts, <c>jq</c>, and editors. A gabarit that only Utf8Json accepts is
	/// not a rendering defect, but it silently breaks tooling.</item>
	/// </list>
	///
	/// <para><b>Neither subsumes the other, so do not collapse them.</b> Utf8Json accepts raw control
	/// characters inside strings that System.Text.Json rejects; conversely a file the pipeline cannot
	/// load may well satisfy System.Text.Json. Keeping only the strict one produces the dangerous
	/// direction — <b>green while the pipeline is broken</b>.</para>
	///
	/// <para><b>Correction of the record (2026-09-02).</b> This file was first written asserting that it
	/// exercised "the very API the pipeline uses — System.Text.Json". That was <b>wrong at the call
	/// site</b>, and the severity claimed from it was wrong with it. The real history:
	/// <c>Cards/Fallacies/Argumentum_Virtues_Face_fr.json</c> violated RFC 8259 from <c>6d22f79a</c>
	/// (2026-08-06, where the #981/#982a additive family rename inserted three selectors into the
	/// <c>css</c> string separated by <b>raw</b> 0x0A) until its repair on 2026-09-02. Escaping it was
	/// right. But it <b>never broke production</b>: measured by po-2023, the 2026-08-24 and 2026-08-28
	/// full regenerations both traversed this gabarit with a fresh harvest and the exact family palette,
	/// and the pre-fix file deserializes cleanly under
	/// <c>Utf8Json.JsonSerializer.Deserialize&lt;CardSetDocument&gt;</c> — verified by running it on the
	/// file recovered from <c>6d22f79a</c>, not inferred.
	/// <see cref="TheTwoParsers_DisagreeOnTheHistoricalDefect"/> pins that divergence so it cannot be
	/// re-collapsed by a later reader.</para>
	///
	/// <para><b>Scope is derived from disk</b>, not tabulated: every <c>*.json</c> under <c>Cards/</c>
	/// that declares a <c>csize</c>, excluding the frozen <c>Archive/</c> trees, selected by <b>raw
	/// text</b> so that a file which fails to parse is still in scope.</para>
	/// </summary>
	public class CardTemplateJsonValidityTests
	{
		private static IEnumerable<string> TemplatePaths()
		{
			var root = TestRepoRoot.Find();
			return Directory.EnumerateFiles(Path.Combine(root, "Cards"), "*.json", SearchOption.AllDirectories)
				.Select(p => p.Replace(Path.DirectorySeparatorChar, '/'))
				.Where(p => !p.Contains("/Archive/", StringComparison.OrdinalIgnoreCase))
				.Where(LooksLikeGabarit)
				.Select(p => Path.GetRelativePath(root, p).Replace(Path.DirectorySeparatorChar, '/'))
				.OrderBy(p => p, StringComparer.Ordinal);
		}

		/// <summary>
		/// A gabarit is recognised by its <c>csize</c> key. Matching on raw text rather than parsing
		/// is deliberate: a file that fails to parse must still be selected, otherwise the broken
		/// templates are exactly the ones the scope drops.
		/// </summary>
		private static bool LooksLikeGabarit(string absolutePath)
		{
			try
			{
				return File.ReadAllText(absolutePath).Contains("\"csize\"", StringComparison.Ordinal);
			}
			catch (IOException)
			{
				return false;
			}
		}

		public static IEnumerable<object[]> Templates() => TemplatePaths().Select(p => new object[] { p });

		[Fact]
		public void TemplateScope_IsDerivedAndNonEmpty()
		{
			TemplatePaths().Should().HaveCountGreaterThanOrEqualTo(10,
				"the shipped tree carries a dozen gabarits (Fallacies face/back/web, Rules, Memo, Scenarii, " +
				"Virtues, ...). A collapsed scope means the discovery broke and the Theories below are certifying " +
				"nothing.");
		}

		/// <summary>
		/// The pipeline contract. Same parser and same target type as CardSetInfo.cs:66. Twin:
		/// <see cref="Template_IsAlsoValidJsonForEveryOtherReader"/> — that one is stricter and cannot
		/// stand in for this one, because the parsers diverge in both directions.
		/// </summary>
		[Theory]
		[MemberData(nameof(Templates))]
		public void Template_LoadsWithTheParserThePipelineActuallyUses(string relativePath)
		{
			var bytes = File.ReadAllBytes(Path.Combine(TestRepoRoot.Find(), relativePath));

			var act = () => Utf8Json.JsonSerializer.Deserialize<CardSetDocument>(StripBom(bytes));

			act.Should().NotThrow(
				"{0} is loaded by CardSetInfo.GetCardSetDocument, whose unqualified JsonSerializer binds to " +
				"Utf8Json (see the class docstring). A gabarit this parser rejects cannot render a single card, " +
				"and a cached harvest hides it because the template is only read on the way to producing one — " +
				"so the failure surfaces on the first clean regeneration, which is exactly what a print run needs.",
				relativePath);
		}

		/// <summary>
		/// RFC 8259 hygiene, for the readers that are not the pipeline. Twin:
		/// <see cref="Template_LoadsWithTheParserThePipelineActuallyUses"/> — a failure here is a tooling
		/// defect, not a rendering defect, and the two must not be merged.
		/// </summary>
		[Theory]
		[MemberData(nameof(Templates))]
		public void Template_IsAlsoValidJsonForEveryOtherReader(string relativePath)
		{
			var bytes = File.ReadAllBytes(Path.Combine(TestRepoRoot.Find(), relativePath));

			var act = () => JsonSerializer.Deserialize<JsonElement>(StripBom(bytes));

			act.Should().NotThrow<JsonException>(
				"{0} must be readable by System.Text.Json, python json.load and jq, which every sync, audit and " +
				"diff script around these files uses. The usual cause of a failure here is a raw line break " +
				"inside a string value: JSON requires an escaped backslash-n even where the surrounding CSS " +
				"accepts either. This is what shipped in the Virtues face template from 2026-08-06 to " +
				"2026-09-02 — it broke tooling, and notably it did NOT break rendering.",
				relativePath);
		}

		/// <summary>
		/// Inverse control for the hygiene contract: the historical defect verbatim — a css value whose
		/// selectors are separated by a raw 0x0A — must be rejected.
		/// </summary>
		[Fact]
		public void HygieneDetector_RejectsARawLineBreakInsideAString()
		{
			var act = () => JsonSerializer.Deserialize<JsonElement>(HistoricalDefect);

			act.Should().Throw<JsonException>(
				"the hygiene check must reject the exact shape that shipped for four weeks; if this passed, the " +
				"Theory above would be green on a non-conforming tree.");
		}

		/// <summary>
		/// Inverse control for the <b>pipeline</b> contract, which needs a witness of its own: the shape
		/// the hygiene check rejects is one Utf8Json happily accepts, so reusing it here would assert
		/// nothing. A truncated document is refused by both.
		/// </summary>
		[Fact]
		public void PipelineDetector_RejectsATruncatedDocument()
		{
			var truncated = Encoding.UTF8.GetBytes("{\"csize\":\"tarot\",\"css\":\"card.a { color: red; }\"");

			var act = () => Utf8Json.JsonSerializer.Deserialize<CardSetDocument>(truncated);

			act.Should().Throw<Exception>(
				"without a witness of its own, the pipeline Theory could be green because Utf8Json accepts " +
				"everything it is handed rather than because the gabarits are sound.");
		}

		/// <summary>
		/// Pins the divergence itself. Recorded because the first version of this file assumed the two
		/// parsers were interchangeable, mis-stated which one the pipeline uses, and drew a false
		/// severity from it. If a later reader deletes one of the two Theories as redundant, this fails
		/// and says why they are not.
		/// </summary>
		[Fact]
		public void TheTwoParsers_DisagreeOnTheHistoricalDefect()
		{
			var strict = () => JsonSerializer.Deserialize<JsonElement>(HistoricalDefect);
			strict.Should().Throw<JsonException>("System.Text.Json enforces RFC 8259 on control characters");

			var pipeline = () => Utf8Json.JsonSerializer.Deserialize<CardSetDocument>(HistoricalDefect);
			pipeline.Should().NotThrow(
				"Utf8Json tolerates the raw control character — which is why the Virtues gabarit rendered " +
				"correctly through the 2026-08-24 and 2026-08-28 regenerations while being invalid JSON. The two " +
				"contracts in this class are therefore not redundant: neither one implies the other.");
		}

		/// <summary>Companion control: the same content with the break escaped must be accepted, so the checks above are about the escaping and not about the selectors.</summary>
		[Fact]
		public void HygieneDetector_AcceptsTheEscapedForm()
		{
			var repaired = Encoding.UTF8.GetBytes(
				"{\"csize\":\"tarot\",\"css\":\"card.exactitudeMathématique,\\ncard.sensQuantitatif { color: red; }\"}");

			var parsed = JsonSerializer.Deserialize<JsonElement>(repaired);

			parsed.GetProperty("css").GetString().Should().Contain("\n",
				"the escaped form must parse to the same CSS text the raw form intended — the repair changes " +
				"the file, not the stylesheet.");
		}

		private static byte[] HistoricalDefect => Encoding.UTF8.GetBytes(
			"{\"csize\":\"tarot\",\"css\":\"card.exactitudeMathématique,\ncard.sensQuantitatif { color: red; }\"}");

		private static byte[] StripBom(byte[] bytes) =>
			bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF
				? bytes.Skip(3).ToArray()
				: bytes;
	}
}
