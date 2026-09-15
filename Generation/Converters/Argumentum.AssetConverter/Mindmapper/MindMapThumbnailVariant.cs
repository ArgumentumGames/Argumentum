using System;
using System.IO;
using ImageMagick;

namespace Argumentum.AssetConverter.Mindmapper
{
	/// <summary>
	/// #1197: the cards mindmaps embed one thumbnail per card node, and FreeMind decodes the
	/// SOURCE PNG at its real resolution before shrinking it to the 60x60 display size — the
	/// display attributes protect nothing. The print pipeline writes those sources at
	/// WidthMM x 300 dpi (50 mm -&gt; 590x590, ~22 MB per language), which OOMs FreeMind's
	/// 32-bit JVM at the export step, after the whole image generation has been spent.
	/// This helper maintains a bounded variant directory SIBLING to the source CardSet folder:
	/// the .mm references only variants, while the print PDF keeps consuming the
	/// full-resolution originals.
	/// </summary>
	public static class MindMapThumbnailVariant
	{
		/// <summary>
		/// Longest-edge bound for the variant PNGs. 120 = the 60 px display size with 2x zoom
		/// headroom; validated in production conditions by the #1181 cards export (25/25 maps
		/// after a manual 120x120 downscale of the 590x590 sources).
		/// </summary>
		public const int DefaultMaxEdge = 120;

		public static string GetVariantDirectory(string cardSetDirectory)
		{
			var trimmed = cardSetDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			return Path.Combine(
				Path.GetDirectoryName(trimmed) ?? throw new ArgumentException(
					$"'{cardSetDirectory}' has no parent directory — expected a CardSet folder inside the images tree", nameof(cardSetDirectory)),
				Path.GetFileName(trimmed) + "-Mindmap");
		}

		/// <summary>
		/// Returns the path of a variant of <paramref name="sourcePath"/> whose longest edge is
		/// at most <paramref name="maxEdge"/> px, creating or refreshing it when missing or older
		/// than the source. Never upscales: a source already within the bound is copied as-is,
		/// so the .mm never references the print-resolution original.
		/// </summary>
		public static string EnsureBoundedVariant(string sourcePath, string variantDirectory, int maxEdge)
		{
			if (maxEdge <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxEdge), maxEdge,
					"the thumbnail bound must be a positive pixel count — an unbounded variant directory would re-arm the FreeMind OOM mine (#1197)");
			}
			if (!File.Exists(sourcePath))
			{
				throw new FileNotFoundException(
					$"Thumbnail source not found: '{sourcePath}' — a matched thumbnail that disappears between scan and read is a failure, not a silent skip", sourcePath);
			}

			Directory.CreateDirectory(variantDirectory);
			var variantPath = Path.Combine(variantDirectory, Path.GetFileName(sourcePath));
			if (File.Exists(variantPath)
				&& File.GetLastWriteTimeUtc(variantPath) >= File.GetLastWriteTimeUtc(sourcePath))
			{
				return variantPath;
			}

			using var source = new MagickImage(sourcePath);
			if (Math.Max(source.Width, source.Height) > maxEdge)
			{
				source.Resize((uint)maxEdge, (uint)maxEdge);
			}
			source.Write(variantPath, MagickFormat.Png);
			return variantPath;
		}
	}
}
