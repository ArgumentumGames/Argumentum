using System;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using ImageMagick;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// #1197 organ: the PNGs the cards mindmaps reference by path are decoded by FreeMind at
	/// their REAL source resolution — the 60x60 display attributes protect nothing, and the
	/// print pipeline writes the sources at WidthMM x 300 dpi (50 mm -&gt; 590x590, ~22 MB per
	/// language), which OOMs the 32-bit JVM at the export step, after the whole image
	/// generation has been spent. The .mm must therefore reference bounded variants, never the
	/// print originals.
	///
	/// RED witness (pre-fix, master `1fbffda2`): the .mm produced by the #1181 run embeds
	/// 176 &lt;img src&gt; all pointing into the RAW CardSet directory
	/// (`..\Images\density-0\Fallacies-Web-Thumbnails\...`), whose files measure 590x590.
	///
	/// The bound is measured on the SOURCE pixel dimensions of the variant file — not on any
	/// display attribute — and every test below builds its own synthetic fixtures, so the organ
	/// holds in CI without a populated Target/ tree.
	/// </summary>
	public class MindMapThumbnailBoundTests
	{
		/// <summary>
		/// Sanity ceiling for any declared bound: 176 thumbnails decoded by a 32-bit JVM must
		/// stay in the few-MB range. A bound above this would not protect the export.
		/// </summary>
		private const int JvmProtectionCeiling = 256;

		private static string WriteSyntheticPng(string path, int width, int height)
		{
			using var image = new MagickImage(new MagickColor("#811da3"), (uint)width, (uint)height);
			image.Write(path, MagickFormat.Png);
			return path;
		}

		private static (int Width, int Height) ReadDimensions(string path)
		{
			using var image = new MagickImage(path);
			return ((int)image.Width, (int)image.Height);
		}

		[Fact]
		public void EnsureBoundedVariant_OversizedSource_WritesVariantWithinDeclaredBound()
		{
			using var temp = new TempDirectory();
			// 590x590: exactly what the print pipeline produces for the 50 mm thumbnail CardSet
			var source = WriteSyntheticPng(Path.Combine(temp.Path, "argumentum_fallacies_1.png"), 590, 590);
			var variantDir = Path.Combine(temp.Path, "variant");

			var variant = MindMapThumbnailVariant.EnsureBoundedVariant(
				source, variantDir, MindMapThumbnailVariant.DefaultMaxEdge);

			var (width, height) = ReadDimensions(variant);
			width.Should().BeInRange(1, MindMapThumbnailVariant.DefaultMaxEdge);
			height.Should().BeInRange(1, MindMapThumbnailVariant.DefaultMaxEdge);
		}

		[Fact]
		public void EnsureBoundedVariant_NonSquareSource_PreservesAspectRatio()
		{
			using var temp = new TempDirectory();
			var source = WriteSyntheticPng(Path.Combine(temp.Path, "wide.png"), 1000, 700);
			var variantDir = Path.Combine(temp.Path, "variant");

			var variant = MindMapThumbnailVariant.EnsureBoundedVariant(source, variantDir, 120);

			var (width, height) = ReadDimensions(variant);
			width.Should().Be(120, "the longest edge must hit the bound exactly");
			height.Should().BeInRange(80, 90, "1000x700 scaled by 0.12 lands at ~84");
		}

		[Fact]
		public void EnsureBoundedVariant_SourceWithinBound_IsCopiedNotUpscaled()
		{
			using var temp = new TempDirectory();
			var source = WriteSyntheticPng(Path.Combine(temp.Path, "small.png"), 90, 60);
			var variantDir = Path.Combine(temp.Path, "variant");

			var variant = MindMapThumbnailVariant.EnsureBoundedVariant(source, variantDir, 120);

			var (width, height) = ReadDimensions(variant);
			(width, height).Should().Be((90, 60),
				"the .mm must never gain pixels the source did not have — upscaling would only feed the JVM");
		}

		[Fact]
		public void EnsureBoundedVariant_MissingSource_ThrowsFileNotFound()
		{
			using var temp = new TempDirectory();
			var variantDir = Path.Combine(temp.Path, "variant");

			var act = () => MindMapThumbnailVariant.EnsureBoundedVariant(
				Path.Combine(temp.Path, "absent.png"), variantDir, 120);

			act.Should().Throw<FileNotFoundException>(
				"a matched thumbnail that disappears between scan and read is a failure, not a silent skip (#1179 policy)");
		}

		[Fact]
		public void EnsureBoundedVariant_NonPositiveBound_Throws()
		{
			using var temp = new TempDirectory();
			var source = WriteSyntheticPng(Path.Combine(temp.Path, "any.png"), 10, 10);

			var act = () => MindMapThumbnailVariant.EnsureBoundedVariant(
				source, Path.Combine(temp.Path, "variant"), 0);

			act.Should().Throw<ArgumentOutOfRangeException>(
				"an unbounded variant directory would re-arm the FreeMind OOM mine — the bound must be declared, not defaulted to nothing");
		}

		[Fact]
		public void EnsureBoundedVariant_CurrentVariantIsReused_StaleVariantIsRefreshed()
		{
			using var temp = new TempDirectory();
			var source = WriteSyntheticPng(Path.Combine(temp.Path, "thumb.png"), 590, 590);
			var variantDir = Path.Combine(temp.Path, "variant");
			var variant = MindMapThumbnailVariant.EnsureBoundedVariant(source, variantDir, 120);

			File.SetLastWriteTimeUtc(source, new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));
			File.SetLastWriteTimeUtc(variant, new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc));
			MindMapThumbnailVariant.EnsureBoundedVariant(source, variantDir, 120)
				.Should().Be(variant, "a variant newer than its source must not be rewritten");

			WriteSyntheticPng(variant, 590, 590); // simulate a stale variant at print resolution
			File.SetLastWriteTimeUtc(variant, new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc));
			var refreshed = MindMapThumbnailVariant.EnsureBoundedVariant(source, variantDir, 120);
			var (width, height) = ReadDimensions(refreshed);
			(width, height).Should().Be((120, 120),
				"a variant older than its source is stale and must be regenerated within the bound");
		}

		[Fact]
		public void GetVariantDirectory_IsDisjointSiblingOfTheSourceCardSetFolder()
		{
			var cardSetDirectory = Path.Combine("Target", "fr", "Images", "density-0", "Fallacies-Web-Thumbnails")
				+ Path.DirectorySeparatorChar;

			var variantDirectory = MindMapThumbnailVariant.GetVariantDirectory(cardSetDirectory);

			variantDirectory.Should().EndWith("Fallacies-Web-Thumbnails-Mindmap");
			variantDirectory.Should().NotBe(cardSetDirectory.TrimEnd(Path.DirectorySeparatorChar));
			Path.GetFullPath(variantDirectory).StartsWith(Path.GetFullPath(cardSetDirectory), StringComparison.Ordinal)
				.Should().BeFalse("the variant must live OUTSIDE the source CardSet folder — the print PDF enumerates that folder and must keep seeing only the full-resolution originals");
		}

		/// <summary>
		/// The routing contract itself: the path the .mm embeds for a card node must resolve into
		/// the bounded variant directory. This is the assertion that goes red if someone re-points
		/// the resolver at the print CardSet folder — the exact pre-#1197 behavior, where 176
		/// img src pointed at 590x590 print originals.
		/// </summary>
		[Fact]
		public void ResolveThumbnailPathForItem_EmbedsTheBoundedVariant_NotThePrintOriginal()
		{
			using var temp = new TempDirectory();
			var config = new AssetConverterConfig { BaseTargetDirectoryName = Path.Combine(temp.Path, "Target") + Path.DirectorySeparatorChar };
			var document = config.FallacyMindMapCreatorConfig.DocumentConfigs.Single(d => d.InsertCardsThumbnails);

			// A synthetic CardSet tree exactly where the resolver will look, holding a
			// print-resolution source like the pipeline writes (50 mm x 300 dpi = 590x590).
			var cardSetDirectory = ImageHelper.GetImageFolder(config, document, "fr", document.ThumbnailsCardSetName);
			WriteSyntheticPng(Path.Combine(cardSetDirectory, "argumentum_fallacies_1..insuffisance.png"), 590, 590);

			var item = new Fallacy { Path = "1", TextFr = "Insuffisance" };
			var embedded = document.ResolveThumbnailPathForItem(config, "fr", item);

			embedded.Should().NotBeNullOrEmpty(
				"a matching source exists in the CardSet folder — an empty path here means the pattern moved and this organ went vacuous");
			embedded.Should().Contain("Fallacies-Web-Thumbnails-Mindmap",
				"the .mm must reference the bounded variant directory");
			embedded.Should().NotContain("Fallacies-Web-Thumbnails" + Path.DirectorySeparatorChar,
				"the .mm must never reference the print CardSet folder — FreeMind decodes those PNGs at full resolution (32-bit JVM OOM, #1197)");

			var documentDirectory = config.GetDocumentDirectory("fr");
			var absolute = Path.GetFullPath(Path.Combine(documentDirectory, embedded));
			File.Exists(absolute).Should().BeTrue("the embedded path must resolve from the document directory, where FreeMind opens the .mm");
			var (width, height) = ReadDimensions(absolute);
			width.Should().BeInRange(1, document.ThumbnailsMaxEdge);
			height.Should().BeInRange(1, document.ThumbnailsMaxEdge);
		}

		/// <summary>
		/// The live compiled-default configs: every mindmap document that inserts card
		/// thumbnails must declare a bound that actually protects the JVM. The CardSet's Dpi
		/// cannot be the guard — it only drives the capture scale; the written PNG is
		/// WidthMM x 300 dpi (ImageHelper forces the density to 300 before ResizeInMM).
		/// </summary>
		[Fact]
		public void LiveMindMapConfigs_EveryThumbnailInsertingDocumentDeclaresABoundedVariant()
		{
			var config = new AssetConverterConfig();
			var bounds = config.FallacyMindMapCreatorConfig.DocumentConfigs
				.Where(d => d.InsertCardsThumbnails)
				.Select(d => (DocumentName: d.DocumentName, Bound: d.ThumbnailsMaxEdge))
				.Concat(config.VirtueMindMapCreatorConfig.DocumentConfigs
					.Where(d => d.InsertCardsThumbnails)
					.Select(d => (DocumentName: d.DocumentName, Bound: d.ThumbnailsMaxEdge)))
				.ToList();

			bounds.Should().NotBeEmpty(
				"the cards mindmap inserts thumbnails by design — an empty list means the config moved and this organ went vacuous");

			foreach (var (documentName, bound) in bounds)
			{
				bound.Should().BeInRange(1, JvmProtectionCeiling,
					"document '{0}' declares a thumbnail bound that would not protect FreeMind's 32-bit JVM", documentName);
			}
		}

		private sealed class TempDirectory : IDisposable
		{
			public TempDirectory()
			{
				Path = System.IO.Path.Combine(
					System.IO.Path.GetTempPath(), "argu-1197-" + Guid.NewGuid().ToString("N"));
				Directory.CreateDirectory(Path);
			}

			public string Path { get; }

			public void Dispose()
			{
				try
				{
					Directory.Delete(Path, true);
				}
				catch (IOException)
				{
				}
			}
		}
	}
}
