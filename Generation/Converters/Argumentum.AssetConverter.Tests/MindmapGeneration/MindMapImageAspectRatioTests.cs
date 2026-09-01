using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// #1253 organ: every &lt;image&gt; of every shipped mindmap carries preserveAspectRatio="none",
	/// so the declared display box is applied to the embedded PNG with NO aspect negotiation at all.
	/// The shipped artefacts are geometrically correct today only because the declared box ratio
	/// equals the embedded source ratio EXACTLY — measured 2026-09-01 on the 41 committed SVGs:
	/// 11 440 images, 0 deviation (176 card thumbnails at 60x60 on 120x120 sources, plus the
	/// FreeMind node icons at 28x16 / 44x16 / 32x16 / 16x16, each on a source of the same size).
	///
	/// That equality is load-bearing and was, until this organ, unguarded. Change one side only —
	/// the hardcoded box in FallacyMindMapDocumentConfig (width="60" height="60") or the shape of
	/// the thumbnail CardSet (FallaciesWebThumbnails renders csize:"squareTile", i.e. square) —
	/// and every card in every mindmap is silently stretched, with no error and no visual alarm in
	/// the pipeline. That is the mindmap-side twin of the print-side defect of #1250
	/// (ImageHelper.ResizeInMM sets IgnoreAspectRatio = true).
	///
	/// RED witness: set the .mm markup to width="60" height="113" (a tarot-proportioned box) while
	/// the thumbnail CardSet still produces square sources — 176 images at box ratio 0.531 against
	/// source ratio 1.000, organ red. Symmetrically, moving the thumbnail CardSet to a tarot
	/// template without widening the box turns the same 176 images red.
	///
	/// The invariant asserted is the ratio, not the attribute: a future fix that replaces
	/// preserveAspectRatio="none" with a letterboxing value is an improvement, and must not be
	/// frozen out by this organ.
	/// </summary>
	public class MindMapImageAspectRatioTests
	{
		/// <summary>
		/// Relative tolerance on box-ratio vs source-ratio. The shipped inventory measures 0
		/// deviation, so 2 % is pure slack for a future non-integer box, not an allowance.
		/// </summary>
		private const double RatioTolerance = 0.02;

		/// <summary>
		/// No-op guard (#1046 family): the inventory measured 11 440 images. A regex that stops
		/// matching, or a Mindmaps tree that stops being committed, must turn this organ RED
		/// rather than let it pass vacuously on an empty enumeration.
		/// </summary>
		private const int MinimumInspectedImages = 10000;

		private static readonly Regex ImageTag =
			new Regex(@"<image\b[^>]*?/>", RegexOptions.Singleline | RegexOptions.Compiled);

		private static readonly Regex Base64Href =
			new Regex("href=\"data:image/png;base64,([^\"]+)\"", RegexOptions.Compiled);

		[Fact]
		public void ShippedMindmapImages_AreDrawnAtTheirSourceAspectRatio()
		{
			var svgFiles = EnumerateMindmapSvgs().ToList();
			svgFiles.Should().NotBeEmpty("the committed Mindmaps tree is the artefact under test");

			var inspected = 0;
			var deviations = new List<string>();

			foreach (var svgPath in svgFiles)
			{
				var svg = File.ReadAllText(svgPath);
				foreach (Match tag in ImageTag.Matches(svg))
				{
					var hrefMatch = Base64Href.Match(tag.Value);
					if (!hrefMatch.Success)
					{
						// An <image> that is not an embedded PNG cannot be measured here; the
						// shipped inventory holds none, so its appearance is itself a signal.
						deviations.Add($"{Path.GetFileName(svgPath)}: <image> without an embedded PNG source");
						continue;
					}

					// Strip the data URI before reading the geometry attributes: the base64 payload
					// must never be scanned for width=/height=.
					var attributes = tag.Value.Remove(hrefMatch.Index, hrefMatch.Length);
					var boxWidth = ReadNumericAttribute(attributes, "width");
					var boxHeight = ReadNumericAttribute(attributes, "height");
					if (boxWidth is null || boxHeight is null)
					{
						deviations.Add($"{Path.GetFileName(svgPath)}: <image> without numeric width/height");
						continue;
					}

					var (sourceWidth, sourceHeight) = ReadPngDimensions(hrefMatch.Groups[1].Value);
					inspected++;

					var boxRatio = boxWidth.Value / boxHeight.Value;
					var sourceRatio = (double)sourceWidth / sourceHeight;
					var drift = Math.Abs(boxRatio / sourceRatio - 1d);
					if (drift > RatioTolerance)
					{
						deviations.Add(string.Format(CultureInfo.InvariantCulture,
							"{0}: box {1}x{2} (ratio {3:F3}) on a {4}x{5} source (ratio {6:F3}) — stretched x{7:F3}",
							Path.GetFileName(svgPath), boxWidth, boxHeight, boxRatio,
							sourceWidth, sourceHeight, sourceRatio, boxRatio / sourceRatio));
					}
				}
			}

			inspected.Should().BeGreaterThanOrEqualTo(MinimumInspectedImages,
				"the shipped mindmaps hold 11 440 measurable images; a lower count means the parser, " +
				"not the artefacts, changed — and a silent 0 would make this organ a no-op");

			deviations.Should().BeEmpty(
				"every <image> carries preserveAspectRatio=\"none\", so any gap between the declared box " +
				"and the embedded source is applied verbatim as a stretch on the shipped artefact");
		}

		private static double? ReadNumericAttribute(string tag, string name)
		{
			var match = Regex.Match(tag, $@"\b{name}=""([\d.]+)""");
			if (!match.Success) return null;
			return double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Reads width/height out of the PNG IHDR chunk. Only the first bytes are needed
		/// (8-byte signature + 8-byte chunk header + 8 bytes of dimensions), so the base64 payload
		/// is decoded on a 64-character prefix rather than in full — 11 440 full decodes would
		/// otherwise put tens of megabytes through the test.
		/// </summary>
		private static (int Width, int Height) ReadPngDimensions(string base64)
		{
			var prefix = base64.Length >= 64 ? base64.Substring(0, 64) : base64;
			prefix = prefix.Substring(0, prefix.Length / 4 * 4);
			var bytes = Convert.FromBase64String(prefix);

			bytes.Length.Should().BeGreaterThanOrEqualTo(24, "a PNG header must be readable");
			bytes.Take(8).Should().Equal(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
				"the embedded sources are PNG by configuration (MagickFormat.Png)");

			var width = (bytes[16] << 24) | (bytes[17] << 16) | (bytes[18] << 8) | bytes[19];
			var height = (bytes[20] << 24) | (bytes[21] << 16) | (bytes[22] << 8) | bytes[23];
			return (width, height);
		}

		/// <summary>
		/// Every committed mindmap SVG, all taxonomies and all languages — not just the file that
		/// happened to carry the reported defect. A per-file scope would have measured Fallacies FR
		/// and declared the family clean.
		/// </summary>
		private static IEnumerable<string> EnumerateMindmapSvgs()
		{
			var cardsRoot = Path.Combine(TestRepoRoot.Find(), "Cards");
			Directory.Exists(cardsRoot).Should().BeTrue("the Cards tree carries the shipped mindmaps");

			return Directory
				.EnumerateDirectories(cardsRoot, "Mindmaps", SearchOption.AllDirectories)
				.SelectMany(dir => Directory.EnumerateFiles(dir, "*.svg", SearchOption.AllDirectories))
				.OrderBy(path => path, StringComparer.Ordinal);
		}
	}
}
