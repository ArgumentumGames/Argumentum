using System;
using FluentAssertions;
using ImageMagick;
using ImageMagick.Drawing;
using Xunit;

namespace Argumentum.AssetConverter.Tests.ImageProcessing
{
	/// <summary>
	/// Contract: <see cref="ImageHelper.ResizeInMM"/> must land a harvested card on its document
	/// geometry <b>without deforming it</b> — the bleed is cropped away, never squashed in.
	///
	/// <para><b>Why this exists.</b> Issue #1250. The method used to set
	/// <c>IgnoreAspectRatio = true</c> and resize the harvest straight onto the document box. CardPen
	/// renders every card at its gabarit size plus a bleed, so the source ratio never equalled the
	/// target ratio and the difference was applied to the artwork: +15.1 % vertical stretch on the
	/// Fallacies face, +12.8 % Rules, +9.0 % Memo, +4.0 % Scenarii, with a deck's back stretched
	/// differently from its face (bleed 5 mm vs 3 mm). Every card shipped since April 2024 carried it.
	/// Nothing caught it because the CardPen preview everyone looked at is correct — the deformation
	/// happened one stage later, inside this method.</para>
	///
	/// <para><b>This is the behavioural half of the guard.</b> Its declaration-level twin,
	/// <see cref="Argumentum.AssetConverter.Tests.CardPenConfiguration.HarvestGeometryContractTests"/>,
	/// checks that the gabarit and the document agree on a ratio. That check would stay green if this
	/// method regressed to stretching, because it never runs the resize; this one would stay green if
	/// the config drifted, because it uses its own synthetic geometry. Both are needed.</para>
	///
	/// <para><b>The measurement is a shape, not a dimension.</b> A test asserting only the output
	/// dimensions would pass under the old stretching code too — the stretch produced exactly the
	/// requested box, that was the whole problem. So the probe is a square drawn in the source: after
	/// a correct resize it is still square, after a stretch it is not.</para>
	/// </summary>
	public class ResizeInMmAspectTests
	{
		/// <summary>Fallacies face as CardPen actually harvests it: tarot 69.85 x 120.65 mm + 5 mm of bleed on each side, at 300 dpi.</summary>
		private const int HarvestWidthPx = 943;

		private const int HarvestHeightPx = 1543;

		/// <summary>Side of the centred square probe, comfortably inside the crop on every branch tested.</summary>
		private const int ProbeSidePx = 400;

		private const decimal TarotWidthMM = 70m;
		private const decimal TarotHeightMM = 120m;

		private static MagickImage BuildProbeImage()
		{
			var image = new MagickImage(MagickColors.White, HarvestWidthPx, HarvestHeightPx);
			image.Density = new Density(300, DensityUnit.PixelsPerInch);

			var left = (HarvestWidthPx - ProbeSidePx) / 2;
			var top = (HarvestHeightPx - ProbeSidePx) / 2;
			new Drawables()
				.FillColor(MagickColors.Black)
				.Rectangle(left, top, left + ProbeSidePx - 1, top + ProbeSidePx - 1)
				.Draw(image);

			return image;
		}

		/// <summary>Bounding box of the black probe, read back from the pixels rather than computed.</summary>
		private static (uint Width, uint Height) MeasureProbe(MagickImage image)
		{
			using var probe = (MagickImage)image.Clone();
			probe.ColorFuzz = new Percentage(25);
			probe.Trim();
			probe.ResetPage();
			return (probe.Width, probe.Height);
		}

		[Fact]
		public void ResizeInMM_LandsOnTheTargetBox()
		{
			using var image = BuildProbeImage();

			image.ResizeInMM(TarotWidthMM, TarotHeightMM, 0m);

			var expectedRatio = (double)(TarotHeightMM / TarotWidthMM);
			((double)image.Height / image.Width).Should().BeApproximately(expectedRatio, 0.005,
				"the card must come out at the geometry the document asked for ({0} x {1} mm)",
				TarotWidthMM, TarotHeightMM);

			// 300 dpi, so 70 mm and 120 mm resolve to ~827 x ~1417 px; allow a pixel of rounding.
			((int)image.Width).Should().BeInRange(825, 829);
			((int)image.Height).Should().BeInRange(1415, 1419);
		}

		[Fact]
		public void ResizeInMM_KeepsTheArtworkUndeformed()
		{
			using var image = BuildProbeImage();

			MeasureProbe(image).Should().Be(((uint)ProbeSidePx, (uint)ProbeSidePx),
				"the probe must start square, otherwise the assertion below measures the fixture");

			image.ResizeInMM(TarotWidthMM, TarotHeightMM, 0m);

			var (width, height) = MeasureProbe(image);
			((double)height / width).Should().BeApproximately(1.0, 0.01,
				"the square probe must still be square after the resize. The source carries a 5 mm bleed " +
				"that ResizeInMM crops; if it stretched it into the box instead, this ratio would drift by " +
				"the amount measured in #1250 (+4.8 % against the 70 x 120 target, +15.2 % against the old " +
				"60 x 113 one). Measured {0} x {1} px.", width, height);
		}

		/// <summary>
		/// Inverse control. A guard that has only ever seen correct output proves nothing about its own
		/// sensitivity (cf. #1112, a self-baselining SVG test that wrote its own expectation). This
		/// replays the historical algorithm — <c>AdaptiveResize</c> with <c>IgnoreAspectRatio = true</c>
		/// onto the old 60 x 113 mm target — on the same fixture, and requires the probe to come out
		/// visibly non-square at the rate the issue measured on the shipped tree.
		/// </summary>
		[Fact]
		public void Detector_SeesThePre1250Stretch()
		{
			using var image = BuildProbeImage();

			image.Density = image.Density.ChangeUnits(DensityUnit.PixelsPerCentimeter);
			var oldTarget = image.Density.ToGeometry(6.0, 11.3);
			oldTarget.Should().NotBeNull("the 60 x 113 mm historical target must resolve at this density");
			oldTarget!.IgnoreAspectRatio = true;
			image.AdaptiveResize(oldTarget);

			var (width, height) = MeasureProbe(image);
			var stretch = (double)height / width - 1.0;

			stretch.Should().BeGreaterThan(0.10,
				"the historical code stretched the Fallacies face by +15.1 % vertically; a probe that came " +
				"back square here would mean this test cannot see the defect it is meant to guard against. " +
				"Measured {0} x {1} px, stretch {2:+0.0%;-0.0%}.", width, height, stretch);
		}

		/// <summary>
		/// The white-margin path (<c>bordermm &gt; 0</c>) is unused by every shipped document, but it is
		/// live code. It must fit the artwork inside the reduced box and pad out to the full card —
		/// still without deforming anything.
		/// </summary>
		[Fact]
		public void ResizeInMM_WithBorder_PadsInsteadOfStretching()
		{
			using var image = BuildProbeImage();

			image.ResizeInMM(TarotWidthMM, TarotHeightMM, 5m);

			((int)image.Width).Should().BeInRange(825, 829, "the outer card size is unchanged by the border");
			((int)image.Height).Should().BeInRange(1415, 1419);

			var (width, height) = MeasureProbe(image);
			((double)height / width).Should().BeApproximately(1.0, 0.01,
				"the border path must not deform the artwork either. Measured {0} x {1} px.", width, height);
		}
	}
}
