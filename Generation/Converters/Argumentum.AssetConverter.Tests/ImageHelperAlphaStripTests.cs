using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ImageMagick;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #134 (720 dpi) — contrat de régression du strip d'alpha sur blanc
	/// (<c>ImageHelper.StripAlphaOnWhite</c>). Le round-trip per-image retiré par #1321 strippait
	/// l'alpha par effet de bord de <c>Alpha(AlphaOption.Remove)</c> ; sans lui, le RGBA des
	/// captures domtoimage survit jusqu'aux PDFs (SMask) et la passe PDF/X aplatit la transparence
	/// à sa résolution device par défaut (720 dpi). Ce contrat épingle la propriété qui autorise
	/// le strip : il est une <b>identité exacte sur tout pixel α=255</b> — le garde-fou contre un
	/// re-jeu du décalage pixels du round-trip #1111. Contrôle mesuré du 12/09 sur le cache
	/// harvest réel (16 fichiers, 6 jeux + ar, 29 517 995 pixels opaques, 0 écart, en mémoire et
	/// après ré-encodage PNG) ; ces Facts en sont la version instrumentée et exécutable.
	/// </summary>
	public class ImageHelperAlphaStripTests
	{
		/// <summary>4 px : rouge opaque · gris opaque · bleu semi-transparent (α=128) · transparent.</summary>
		private static MagickImage BuildRgba()
		{
			byte[] raw =
			{
				255, 0, 0, 255,
				17, 200, 94, 255,
				0, 0, 255, 128,
				90, 90, 90, 0,
			};
			var settings = new MagickReadSettings { Width = 4, Height = 1, Format = MagickFormat.Rgba, Depth = 8 };
			return new MagickImage(raw, settings);
		}

		private static List<(int X, ushort R, ushort G, ushort B)> SnapshotOpaque(MagickImage img)
		{
			var c = img.Channels.ToList();
			uint iR = (uint)c.IndexOf(PixelChannel.Red), iG = (uint)c.IndexOf(PixelChannel.Green),
				iB = (uint)c.IndexOf(PixelChannel.Blue), iA = (uint)c.IndexOf(PixelChannel.Alpha);
			var opaque = new List<(int, ushort, ushort, ushort)>();
			foreach (var p in img.GetPixels())
			{
				if (p.GetChannel(iA) == ushort.MaxValue)
				{
					opaque.Add((p.X, p.GetChannel(iR), p.GetChannel(iG), p.GetChannel(iB)));
				}
			}
			return opaque;
		}

		private static void AssertPixels(MagickImage img, IEnumerable<(int X, ushort R, ushort G, ushort B)> expected)
		{
			var c = img.Channels.ToList();
			uint iR = (uint)c.IndexOf(PixelChannel.Red), iG = (uint)c.IndexOf(PixelChannel.Green), iB = (uint)c.IndexOf(PixelChannel.Blue);
			var px = img.GetPixels();
			foreach (var (x, r, g, b) in expected)
			{
				var p = px[x, 0];
				p.GetChannel(iR).Should().Be(r, $"pixel opaque #{x} doit être inchangé octet-pour-octet");
				p.GetChannel(iG).Should().Be(g, $"pixel opaque #{x} doit être inchangé octet-pour-octet");
				p.GetChannel(iB).Should().Be(b, $"pixel opaque #{x} doit être inchangé octet-pour-octet");
			}
		}

		[Fact]
		public void StripAlphaOnWhite_Is_Exact_Identity_On_Opaque_Pixels_And_Removes_The_Channel()
		{
			using var img = BuildRgba();
			img.Channels.Should().Contain(PixelChannel.Alpha, "domtoimage capture du RGBA");
			var opaqueBefore = SnapshotOpaque(img);
			opaqueBefore.Should().HaveCount(2, "2 des 4 pixels de l'échantillon sont opaques");

			img.StripAlphaOnWhite();

			img.Channels.Should().NotContain(PixelChannel.Alpha, "le canal alpha doit disparaître du PNG écrit");
			AssertPixels(img, opaqueBefore);
		}

		[Fact]
		public void StripAlphaOnWhite_Blends_PartialAlpha_Over_White_Instead_Of_Dropping_It()
		{
			using var img = BuildRgba();
			img.StripAlphaOnWhite();

			var c = img.Channels.ToList();
			uint iR = (uint)c.IndexOf(PixelChannel.Red), iG = (uint)c.IndexOf(PixelChannel.Green), iB = (uint)c.IndexOf(PixelChannel.Blue);
			var p = img.GetPixels()[2, 0]; // bleu α=128

			// Composité vers le blanc : R et G partis de 0 doivent être tirés vers le haut par la
			// contribution blanche (α=128 ⇒ ~50 %), sans atteindre le blanc pur ; B reste au max
			// (255 chez la source comme chez le blanc). Si l'alpha était simplement abandonné,
			// R=G=0 — c'est le discriminateur.
			p.GetChannel(iR).Should().BeGreaterThan((ushort)0, "le blanc doit contribuer au canal rouge");
			p.GetChannel(iR).Should().BeLessThan(ushort.MaxValue, "le bleu d'origine (R=0) doit encore peser 50 %");
			p.GetChannel(iG).Should().BeGreaterThan((ushort)0, "le blanc doit contribuer au canal vert");
			p.GetChannel(iG).Should().BeLessThan(ushort.MaxValue, "le bleu d'origine (G=0) doit encore peser 50 %");
			p.GetChannel(iB).Should().Be(ushort.MaxValue, "B=255 chez la source ET chez le blanc : inchangé");
		}

		[Fact]
		public void StripAlphaOnWhite_Maps_FullyTransparent_To_PureWhite()
		{
			using var img = BuildRgba();
			img.StripAlphaOnWhite();

			var c = img.Channels.ToList();
			uint iR = (uint)c.IndexOf(PixelChannel.Red), iG = (uint)c.IndexOf(PixelChannel.Green), iB = (uint)c.IndexOf(PixelChannel.Blue);
			var p = img.GetPixels()[3, 0]; // transparent

			p.GetChannel(iR).Should().Be(ushort.MaxValue);
			p.GetChannel(iG).Should().Be(ushort.MaxValue);
			p.GetChannel(iB).Should().Be(ushort.MaxValue);
		}

		[Fact]
		public void StripAlphaOnWhite_Survives_The_Png_Codec_RoundTrip()
		{
			using var img = BuildRgba();
			var opaqueBefore = SnapshotOpaque(img);

			img.StripAlphaOnWhite();
			var png = img.ToByteArray(MagickFormat.Png);

			using var reloaded = new MagickImage(png, new MagickReadSettings { ColorSpace = ColorSpace.sRGB });
			reloaded.Channels.Should().NotContain(PixelChannel.Alpha, "le PNG ré-encodé ne doit plus porter de canal alpha");
			AssertPixels(reloaded, opaqueBefore);
		}
	}
}
