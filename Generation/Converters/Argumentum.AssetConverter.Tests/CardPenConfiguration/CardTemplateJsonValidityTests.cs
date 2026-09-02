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
	/// Contract: every card gabarit the pipeline ships must deserialize with the very API the
	/// pipeline uses — <c>System.Text.Json.JsonSerializer.Deserialize</c>, called from
	/// <c>CardSetInfo.GetCardSetDocument</c>.
	///
	/// <para><b>Why this exists.</b> Found on 2026-09-02 by the #1250 geometry organ, which could not
	/// parse <c>Cards/Fallacies/Argumentum_Virtues_Face_fr.json</c>. The file had been invalid JSON
	/// since <c>6d22f79a</c> (2026-08-06, the #981/#982a family rename): the additive rename inserted
	/// the three selectors
	/// <c>card.exactitudeMathématique, card.rigueurMathématique, card.sensQuantitatif</c> into the
	/// <c>css</c> string separated by <b>raw</b> 0x0A bytes, where every other line break in that
	/// string is an escaped <c>\n</c>. RFC 8259 forbids unescaped control characters inside a string,
	/// and System.Text.Json enforces it: the shipped deserializer throws
	/// <c>'0x0A' is invalid within a JSON string. Path: $.css</c>. Verified by running that exact call
	/// on the file, not inferred.</para>
	///
	/// <para><b>Why nothing noticed for four weeks.</b> The defect is invisible to every instrument
	/// that was looking. It is not a CSS error — as CSS the block is valid, which is why reading the
	/// diff shows nothing wrong. It is not caught by a build, a lint, or a test, because no test read
	/// the gabarits. And it is silent in a pipeline run that reuses a cached harvest, because the
	/// template is only parsed on the way to producing one. It surfaces on the first clean
	/// regeneration — which is precisely what the print run (#1187) requires.</para>
	///
	/// <para><b>Scope is derived from disk</b>, not tabulated: every <c>*.json</c> under
	/// <c>Cards/</c> that declares a <c>csize</c>, excluding the frozen <c>Archive/</c> trees. A
	/// hardcoded list would stop covering a gabarit added later, which is the failure mode where a
	/// guard stays green because it stopped looking.</para>
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
				"Virtues, ...). A collapsed scope means the discovery broke and the Theory below is certifying " +
				"nothing.");
		}

		[Theory]
		[MemberData(nameof(Templates))]
		public void Template_DeserializesWithThePipelineReader(string relativePath)
		{
			var bytes = File.ReadAllBytes(Path.Combine(TestRepoRoot.Find(), relativePath));

			var act = () => JsonSerializer.Deserialize<JsonElement>(StripBom(bytes));

			act.Should().NotThrow<JsonException>(
				"{0} is loaded by CardSetInfo.GetCardSetDocument through JsonSerializer.Deserialize. A gabarit " +
				"that does not parse cannot render a single card, and the failure only appears on a clean " +
				"regeneration — a cached harvest hides it (this is how the Virtues face template stayed broken " +
				"from 2026-08-06 to 2026-09-02). The usual cause is a raw line break inside a string value: in " +
				"JSON it must be written as an escaped \n, even though the surrounding CSS would accept either.",
				relativePath);
		}

		/// <summary>
		/// Inverse control. A guard that has only ever seen valid files proves nothing about its own
		/// sensitivity (cf. #1112). This feeds it the historical defect verbatim — a css value whose
		/// selectors are separated by raw 0x0A — and requires rejection.
		/// </summary>
		[Fact]
		public void Detector_RejectsARawLineBreakInsideAString()
		{
			var broken = Encoding.UTF8.GetBytes(
				"{\"csize\":\"tarot\",\"css\":\"card.exactitudeMathématique,\ncard.sensQuantitatif { color: red; }\"}");

			var act = () => JsonSerializer.Deserialize<JsonElement>(broken);

			act.Should().Throw<JsonException>(
				"the detector must reject the exact shape that shipped broken for four weeks; if this passed, " +
				"the Theory above would be green on a broken tree.");
		}

		/// <summary>Companion control: the same content with the break escaped must be accepted, so the check above is about the escaping and not about the selectors.</summary>
		[Fact]
		public void Detector_AcceptsTheEscapedForm()
		{
			var repaired = Encoding.UTF8.GetBytes(
				"{\"csize\":\"tarot\",\"css\":\"card.exactitudeMathématique,\\ncard.sensQuantitatif { color: red; }\"}");

			var parsed = JsonSerializer.Deserialize<JsonElement>(repaired);

			parsed.GetProperty("css").GetString().Should().Contain("\n",
				"the escaped form must parse to the same CSS text the raw form intended — the repair changes " +
				"the file, not the stylesheet.");
		}

		private static byte[] StripBom(byte[] bytes) =>
			bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF
				? bytes.Skip(3).ToArray()
				: bytes;
	}
}
