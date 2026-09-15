using System.IO;
using System.Text;
using System.Xml.Linq;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// Pins the #804 residue-3 contract: the mind-map SVG serializer must emit an XML declaration
	/// whose encoding matches the physical byte encoding of the written file (UTF-8 with BOM).
	///
	/// Root cause: <c>XmlWriter.Create(StringBuilder)</c> binds to a <see cref="StringWriter"/>
	/// (UTF-16), so the declaration used to read <c>encoding="utf-16"</c> while the bytes were
	/// UTF-8 — a latent mislabel carried by the 32 committed <c>*.content.svg</c> / <c>*.links.svg</c>
	/// (× 8 langs × Fallacies+Virtues). The <see cref="MindMapSvgWriter"/> helper normalizes the
	/// declaration to UTF-8; the on-disk artefacts realign on the next regeneration (post-tag).
	///
	/// The legacy read-path (<c>File.ReadAllText</c> + <c>XDocument.Parse</c>, see
	/// <c>FallacyMindMapDocumentConfig.RegenerateInteractiveContentSvgAsync</c>) is encoding-agnostic
	/// and must keep parsing utf-16-declared input (stale or externally supplied SVGs) — this
	/// guards against a read-side regression; the committed artefacts realigned to utf-8 with
	/// the 2026-08-20 regeneration (PR #1120), closing #804 on disk.
	/// </summary>
	public class MindMapSvgEncodingTests
	{
		private static readonly string RepoRoot = TestRepoRoot.Find();

		/// <summary>The canonical committed content.svg — the real artefact this class pins
			/// (UTF-8 BOM with matching declaration since the 2026-08-20 regeneration; it carried
			/// the legacy utf-16 mislabel before that, see #804).</summary>
		private static readonly string CommittedSvgPath =
			Path.Combine(RepoRoot, "Cards", "Fallacies", "Mindmaps", "fr", "Fallacies_fr.content.svg");

		[Fact]
		public void MindMapSvgWriter_Emits_Utf8_Declaration_Even_When_Source_Declares_Utf16()
		{
			// Arrange — a document whose source declaration (e.g. from FreeMind/Batik) says utf-16
			XNamespace svgNs = "http://www.w3.org/2000/svg";
			var doc = new XDocument(
				new XDeclaration("1.0", "utf-16", null),
				new XElement(svgNs + "svg"));

			// Act
			var serialized = MindMapSvgWriter.WriteToString(doc);

			// Assert — the emitted declaration must declare UTF-8 (Encoding.UTF8.WebName = "utf-8"),
			// matching the physical byte encoding used by callers (File.WriteAllText with Encoding.UTF8).
			serialized.Should().StartWith("<?xml");
			serialized.Should().Contain($"encoding=\"{Encoding.UTF8.WebName}\"",
				"the declaration must match the UTF-8 bytes written by File.WriteAllText(..., Encoding.UTF8)");
			serialized.Should().NotContain("encoding=\"utf-16\"",
				"the UTF-16 default of XmlWriter-on-StringBuilder is exactly the #804 mislabel being fixed");
		}

		[Fact]
		public void Fallacy_GetSvgContent_Emits_Utf8_Declaration()
		{
			// Public-surface check through the Fallacy config's own serializer (the path content.svg
			// and links.svg actually go through at generation time).
			XNamespace svgNs = "http://www.w3.org/2000/svg";
			var doc = new XDocument(
				new XDeclaration("1.0", "utf-16", null),
				new XElement(svgNs + "svg"));

			var serialized = FallacyMindMapDocumentConfig.GetSvgContent(doc);

			serialized.Should().Contain($"encoding=\"{Encoding.UTF8.WebName}\"");
			serialized.Should().NotContain("encoding=\"utf-16\"");
		}

		[Fact]
		public void Regenerated_Committed_Svg_Declares_Utf8_And_Parses_Via_TextReadPath()
		{
			// The 32 committed files were regenerated on 2026-08-20 (final corpus, PR #1120):
			// they now carry the #804-aligned form — UTF-8 BOM bytes with a matching utf-8
			// declaration. Pin that the regeneration did not reintroduce the mislabel.
			File.Exists(CommittedSvgPath).Should().BeTrue(
				"the committed Fallacies_fr.content.svg is the canonical artefact fixture");

			var svgText = File.ReadAllText(CommittedSvgPath);
			svgText.Should().Contain("encoding=\"utf-8\"",
				"post-regen form: declaration must match the UTF-8 BOM bytes written by MindMapSvgWriter callers");
			svgText.Should().NotContain("encoding=\"utf-16\"");

			// The exact line used by the generator must not throw
			var act = () => XDocument.Parse(svgText);
			act.Should().NotThrow("the read-path must keep parsing the committed artefact");
		}

		[Fact]
		public void Legacy_Utf16Declared_Synthetic_Svg_Still_Parses_Via_TextReadPath()
		{
			// Read-path robustness guard for RegenerateInteractiveContentSvgAsync, kept after the
			// committed artefacts realigned: File.ReadAllText auto-detects the BOM -> UTF-8, then
			// XDocument.Parse ignores the declaration on a decoded string. A stale or externally
			// supplied utf-16-declared SVG must therefore never break the read path.
			var svgText = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\n<svg xmlns=\"http://www.w3.org/2000/svg\"/>";

			var act = () => XDocument.Parse(svgText);
			act.Should().NotThrow("the read-path workaround must tolerate a utf-16 declaration on a decoded string");
		}
	}
}
