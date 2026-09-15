using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Argumentum.AssetConverter.Mindmapper
{
	/// <summary>
	/// Serializes mind-map <see cref="XDocument"/> instances to SVG strings with an XML
	/// declaration that matches the physical byte encoding (issue #804).
	///
	/// A plain <c>XmlWriter.Create(<see cref="StringBuilder"/>)</c> binds to a <see cref="StringWriter"/>
	/// whose <see cref="StringWriter.Encoding"/> is UTF-16, so the emitted declaration reads
	/// <c>encoding="utf-16"</c> — even though every caller (<see cref="File.WriteAllText(string, string, Encoding)"/>
	/// with <see cref="Encoding.UTF8"/>) writes the file as UTF-8 (with BOM). The committed
	/// <c>*.content.svg</c> / <c>*.links.svg</c> (× 8 langs × Fallacies+Virtues = 32 files) carry
	/// this latent mislabel. <see cref="WriteToString"/> normalizes the declaration to UTF-8 so the
	/// emitted text matches its bytes; the 32 on-disk artefacts realign on the next regeneration
	/// (post-tag, #804 residue 3).
	///
	/// The legacy read-path (<c>File.ReadAllText</c> + <c>XDocument.Parse</c>) is encoding-agnostic
	/// and keeps working on the current utf-16-declared files until they are regenerated.
	/// </summary>
	internal static class MindMapSvgWriter
	{
		/// <summary>
		/// Serializes <paramref name="svgDoc"/> to an indented SVG string whose XML declaration
		/// declares UTF-8 (matching the physical byte encoding used by callers).
		/// </summary>
		public static string WriteToString(XDocument svgDoc)
		{
			var sb = new StringBuilder();
			var settings = new XmlWriterSettings
			{
				Indent = true,
				IndentChars = "\t", // use tab for indentation
				NewLineChars = Environment.NewLine,
				NewLineHandling = NewLineHandling.Replace
			};
			using (var writer = XmlWriter.Create(new Utf8StringWriter(sb), settings))
			{
				svgDoc.Save(writer);
			}
			return sb.ToString();
		}

		/// <summary>
		/// A <see cref="StringWriter"/> that reports UTF-8 as its <see cref="Encoding"/>, so an
		/// <see cref="XmlWriter"/> created on it emits <c>encoding="UTF-8"</c> in the XML
		/// declaration instead of the <see cref="StringWriter"/> default (UTF-16). The underlying
		/// storage is still UTF-16 (<see cref="StringBuilder"/>); callers are responsible for
		/// writing the returned string to disk with <see cref="Encoding.UTF8"/>.
		/// </summary>
		private sealed class Utf8StringWriter : StringWriter
		{
			public Utf8StringWriter(StringBuilder sb) : base(sb) { }
			public override Encoding Encoding => Encoding.UTF8;
		}
	}
}
