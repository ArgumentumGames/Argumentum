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
	/// and must keep parsing the current utf-16-declared files until they are regenerated — this
	/// guards against a read-side regression while the artefacts are still stale.
	/// </summary>
	public class MindMapSvgEncodingTests
	{
		private static readonly string RepoRoot = TestRepoRoot.Find();

		/// <summary>A representative committed file that still carries the legacy mislabel
			/// (declared utf-16, physically UTF-8 BOM) — real artefact, not a synthetic fixture.</summary>
		private static readonly string LegacyUtf16DeclaredSvgPath =
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
		public void Legacy_Utf16Declared_Committed_Svg_Still_Parses_Via_TextReadPath()
		{
			// Regression guard for the read-side workaround in RegenerateInteractiveContentSvgAsync:
			// the 32 committed files still declare utf-16 (pre-regen). File.ReadAllText auto-detects
			// the BOM -> UTF-8, then XDocument.Parse ignores the declaration on a decoded string.
			// This must keep working until the artefacts are regenerated post-tag.
			File.Exists(LegacyUtf16DeclaredSvgPath).Should().BeTrue(
				"the committed Fallacies_fr.content.svg is the canonical legacy mislabelled fixture");

			var svgText = File.ReadAllText(LegacyUtf16DeclaredSvgPath);
			svgText.Should().Contain("encoding=\"utf-16\"",
				"pre-condition: this fixture is still the stale (pre-regen) form; update the assertion if it was regenerated");

			// The exact line used by the generator must not throw on the legacy declaration
			var act = () => XDocument.Parse(svgText);
			act.Should().NotThrow("the read-path workaround must tolerate the committed utf-16 declaration");
		}
	}
}
