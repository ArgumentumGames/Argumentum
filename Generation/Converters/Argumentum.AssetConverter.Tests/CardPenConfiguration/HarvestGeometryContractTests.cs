using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.CardPenConfiguration
{
	/// <summary>
	/// Contract: for every card a document ships, the geometry CardPen renders (gabarit card size +
	/// bleed) and the geometry the document resizes it to (<see cref="DocumentCard.WidthMM"/> ×
	/// <see cref="DocumentCard.HeigthMM"/>) must have the same aspect ratio, within 1 %.
	///
	/// <para><b>Why this exists.</b> Issue #1250 (measured 2026-09-01): CardPen renders the Fallacies
	/// face as a standard tarot + 5 mm bleed (943 × 1543 px, ratio 1.6363) while the document config
	/// resizes it to 60 × 113 mm (ratio 1.8833). <c>ImageHelper.ResizeInMM</c> sets
	/// <c>IgnoreAspectRatio = true</c> and never crops the bleed, so the mismatch is applied verbatim
	/// to the printed card: +15.1 % vertical stretch on the Fallacies face, +12.8 % Rules, +9.0 % Memo,
	/// +4.0 % Scenarii — and the back of a deck is stretched differently from its face (bleed 5 mm vs
	/// 3 mm). Every card shipped since April 2024 is deformed; nobody noticed because the CardPen
	/// preview, which everyone looked at, is correct — the deformation happens one stage later, in the
	/// resize. This organ sits exactly on that seam.</para>
	///
	/// <para><b>How it was fixed, and what this now measures.</b> Branch (A) was retained (owner GO,
	/// 2026-09-02): the tarot documents were retargeted from the non-standard 60 x 113 mm to the
	/// standard 70 x 120 mm, and <c>ImageHelper.ResizeInMM</c> was changed from
	/// <c>IgnoreAspectRatio = true</c> to cover-and-crop, so the bleed is trimmed off rather than
	/// squashed in. This organ therefore compares the gabarit <b>trim</b> ratio — not the
	/// bleed-inclusive one — against the document target. Every shipped gabarit trims to 1.7273
	/// (tarot) or 1.4000 (poker); the retargeted couples land at +0.76 % and 0.00 % drift.
	/// It is a declaration-level guard: that the two configured sides agree. The behavioural half —
	/// that the resize really crops instead of stretching — is
	/// <see cref="Argumentum.AssetConverter.Tests.ImageProcessing.ResizeInMmAspectTests"/>, which
	/// carries its own red witness. Neither is sufficient alone: this one would stay green if
	/// ResizeInMM regressed to stretching, and that one would stay green if the config drifted.</para>
	///
	/// <para><b>Scope is derived, not tabulated</b> (same discipline as
	/// <see cref="InsecureSubResourceContractTests"/>): the couples are read from
	/// <see cref="WebBasedGeneratorConfig"/> itself — every <c>CardSetDocuments</c> entry joined to
	/// its <c>CardSets</c> declaration by name — and the expected render geometry is read from the
	/// gabarit JSON plus CardPen's own <c>cardSizes</c> table in <c>Generation/CardPen/js/const.js</c>.
	/// Nothing is hardcoded, so adding a CardSet or a document automatically extends the guard.</para>
	/// </summary>
	public class HarvestGeometryContractTests
	{
		/// <summary>Tolerance on the aspect-ratio drift, per issue #1250 ("Organe à prévoir").</summary>
		private const double RatioTolerance = 0.01;

		/// <summary>
		/// CardPen's standard card sizes, read from <c>Generation/CardPen/js/const.js</c> — the same
		/// table the renderer itself resolves <c>csize</c> against. Entries are
		/// <c>name: [height, width, unit]</c> where the unit token is a JS variable
		/// (<c>ins</c>/<c>mms</c>/<c>pxs</c>). Reading the file (instead of tabulating sizes here)
		/// means the organ cannot drift from what CardPen actually renders.
		/// </summary>
		private static readonly Lazy<Dictionary<string, (double HeightMM, double WidthMM)>> CardSizes =
			new Lazy<Dictionary<string, (double, double)>>(ParseCardSizes);

		private static readonly Regex CardSizeEntry = new Regex(
			@"(?<name>\w+)\s*:\s*\[\s*(?<h>[\d.]+)\s*,\s*(?<w>[\d.]+)\s*,\s*(?<unit>ins|mms|pxs)\s*\]",
			RegexOptions.Compiled);

		private static Dictionary<string, (double HeightMM, double WidthMM)> ParseCardSizes()
		{
			var constJs = Path.Combine(TestRepoRoot.Find(), "Generation", "CardPen", "js", "const.js");
			File.Exists(constJs).Should().BeTrue(
				"the guard resolves CardPen standard sizes from Generation/CardPen/js/const.js. If the file " +
				"moved, the guard would be comparing against a size table CardPen no longer uses.");

			var text = File.ReadAllText(constJs);
			var block = Regex.Match(text, @"var\s+cardSizes\s*=\s*\{(?<body>[\s\S]*?)\};");
			block.Success.Should().BeTrue("cardSizes table not found in const.js — CardPen moved it?");

			var sizes = new Dictionary<string, (double, double)>(StringComparer.Ordinal);
			foreach (Match m in CardSizeEntry.Matches(block.Groups["body"].Value))
			{
				var unit = m.Groups["unit"].Value == "ins" ? "in"
					: m.Groups["unit"].Value == "mms" ? "mm"
					: "px";
				// px units are dpi-relative; no shipped standard size uses them, so resolve at 300 dpi.
				var toMm = unit == "in" ? 25.4 : unit == "mm" ? 1.0 : 25.4 / 300.0;
				sizes[m.Groups["name"].Value] = (
					ParseDouble(m.Groups["h"].Value) * toMm,
					ParseDouble(m.Groups["w"].Value) * toMm);
			}

			sizes.Should().NotBeEmpty("const.js declares dozens of standard sizes; an empty parse means the " +
				"entry regex no longer matches the file's format.");
			return sizes;
		}

		private static double ParseDouble(string value) =>
			double.Parse(value, CultureInfo.InvariantCulture);

		/// <summary>
		/// Every (gabarit, resize target) couple the shipped config produces: one entry per document
		/// card-set side (face/back) whose CardSet declares a template and whose DocumentCard declares
		/// a non-zero target. Disabled documents are included on purpose — geometry rot in a disabled
		/// document ships the day it is re-enabled.
		/// </summary>
		private static IEnumerable<(string Label, string RelPath, decimal WidthMM, decimal HeigthMM)> DeclaredCouples()
		{
			var config = new WebBasedGeneratorConfig();

			var cardSetsByName = config.CardSets
				.Where(cs => cs.Name != null)
				.GroupBy(cs => cs.Name, StringComparer.Ordinal)
				.ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

			foreach (var document in config.CardSetDocuments)
			{
				foreach (var docCardSet in document.CardSets ?? Enumerable.Empty<DocumentCardSet>())
				{
					if (string.IsNullOrEmpty(docCardSet.CardSetName) ||
					    !cardSetsByName.TryGetValue(docCardSet.CardSetName, out var cardSet))
					{
						continue;
					}

					var sides = new[]
					{
						("face", cardSet.FaceCardSetInfo, docCardSet.FrontCards),
						("back", cardSet.BackCardSetInfo, docCardSet.BackCards),
					};

					foreach (var (side, info, target) in sides)
					{
						if (info == null || target == null)
						{
							continue;
						}

						if (target.WidthMM <= 0 || target.HeigthMM <= 0)
						{
							continue;
						}

						var rel = ToRepoRelative(info.JsonFilePathDebug);
						if (rel != null)
						{
							yield return ($"{cardSet.Name}/{side}", rel, target.WidthMM, target.HeigthMM);
						}
					}
				}
			}
		}

		/// <summary>Same path collapse as <see cref="InsecureSubResourceContractTests"/>: keep the Cards/ tail.</summary>
		private static string? ToRepoRelative(string? declared)
		{
			if (string.IsNullOrWhiteSpace(declared))
			{
				return null;
			}

			var normalised = declared.Replace('\\', '/');
			var idx = normalised.IndexOf("Cards/", StringComparison.OrdinalIgnoreCase);
			return idx < 0 ? null : normalised.Substring(idx);
		}

		public static IEnumerable<object[]> ActiveCouples() =>
			DeclaredCouples()
				.GroupBy(c => (c.RelPath, c.WidthMM, c.HeigthMM))
				.OrderBy(g => g.Key.RelPath, StringComparer.Ordinal)
				.ThenBy(g => g.Key.WidthMM)
				.Select(g => new object[]
				{
					g.Key.RelPath,
					g.Key.WidthMM,
					g.Key.HeigthMM,
					string.Join("; ", g.Select(c => $"{c.Label} → {g.Key.WidthMM}×{g.Key.HeigthMM}mm").Distinct()),
				});

		/// <summary>Anti-noop, same discipline as the sub-resource contract: assert the scope before the scope's claims.</summary>
		[Fact]
		public void GeometryScope_IsDerivedFromConfigAndNonEmpty()
		{
			var couples = DeclaredCouples().ToList();

			couples.Should().NotBeEmpty(
				"the guard derives its scope from WebBasedGeneratorConfig (CardSetDocuments joined to CardSets " +
				"by CardSetName). An empty scope means the join broke (names drifted, CardSetDocuments moved) and " +
				"the Theory below has silently become a no-op certifying zero cards.");

			couples.Select(c => (c.RelPath, c.WidthMM, c.HeigthMM)).Distinct().Count().Should().BeGreaterThanOrEqualTo(10,
				"the shipped config couples ~12 distinct gabarits to tarot, poker and web-square targets. A sudden " +
				"drop means the derivation broke rather than that documents were deleted.");
		}

		[Theory]
		[MemberData(nameof(ActiveCouples))]
		public void HarvestGeometry_MatchesDocumentTargetRatioWithin1Percent(
			string relPath, decimal widthMM, decimal heigthMM, string usedBy)
		{
			var path = Path.Combine(TestRepoRoot.Find(), relPath);

			File.Exists(path).Should().BeTrue(
				"{0} is declared in WebBasedGeneratorConfig (used by {1}) but is not on disk.",
				relPath, usedBy);

			var (renderHeightMM, renderWidthMM, bleedMM, sizeName) = ExpectedRenderGeometry(path);

			var trimRatio = renderHeightMM / renderWidthMM;
			var targetRatio = (double)(heigthMM / widthMM);
			var drift = trimRatio / targetRatio - 1.0;

			trimRatio.Should().BeGreaterThan(0);
			drift.Should().BeInRange(-RatioTolerance, RatioTolerance,
				"{0} (used by {1}) renders {2} at a trim size of {3:0.00} x {4:0.00} mm (plus {5:0.0} mm of " +
				"bleed, which ImageHelper.ResizeInMM crops) -> trim ratio h/w {6:0.0000}, but its document card " +
				"targets {7} x {8} mm -> ratio {9:0.0000}: a {10:+0.0%;-0.0%} aspect drift. Anything beyond the " +
				"tolerance is applied to the printed card as a stretch, which is exactly the defect of #1250 " +
				"(the owner report: \"cartes toutes etirees en hauteur\"). Reconcile one side with the other: " +
				"re-render the gabarit at the document geometry, or retarget the document to the gabarit.",
				relPath, usedBy, sizeName, renderHeightMM, renderWidthMM, bleedMM,
				trimRatio, widthMM, heigthMM, targetRatio, drift);
		}

		/// <summary>
		/// Mirrors CardPen's own <c>context.size.card(data, true)</c> (main.js): resolve
		/// <c>csize</c> — a standard name from const.js, or <c>custom</c> = [<c>cheight</c>,
		/// <c>cwidth</c>, <c>cunit</c>] — add 2 × <c>blsize</c> on each side when positive, then apply
		/// <c>cori</c> (landscape swaps height and width).
		/// </summary>
		private static (double HeightMM, double WidthMM, double BleedMM, string SizeName) ExpectedRenderGeometry(
			string templatePath)
		{
			using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(templatePath));
			var root = doc.RootElement;

			string StringValue(string key) => root.TryGetProperty(key, out var v) && v.ValueKind == System.Text.Json.JsonValueKind.String
				? v.GetString() ?? ""
				: "";

			double NumberValue(string key, double fallback = 0)
			{
				if (!root.TryGetProperty(key, out var v))
				{
					return fallback;
				}

				var raw = v.ValueKind == System.Text.Json.JsonValueKind.Number
					? v.GetRawText()
					: v.ValueKind == System.Text.Json.JsonValueKind.String ? v.GetString() : null;
				return raw != null && double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed)
					? parsed
					: fallback;
			}

			var sizeName = StringValue("csize");
			sizeName.Should().NotBeNullOrEmpty(
				"template {0} has no csize — CardPen would render it at an undefined size", templatePath);

			double heightMM;
			double widthMM;

			if (sizeName == "custom")
			{
				var cheight = StringValue("cheight");
				var cwidth = StringValue("cwidth");
				(cheight.Length > 0 && cwidth.Length > 0).Should().BeTrue(
					"template {0} declares csize \"custom\" but leaves cwidth/cheight empty — CardPen renders [0,0]",
					templatePath);

				var unit = StringValue("cunit");
				var toMm = unit == "in" ? 25.4 : unit == "mm" ? 1.0 : 25.4 / NumberValue("dpi", 300);
				heightMM = ParseDouble(cheight) * toMm;
				widthMM = ParseDouble(cwidth) * toMm;
			}
			else
			{
				CardSizes.Value.ContainsKey(sizeName).Should().BeTrue(
					"template {0} declares csize \"{1}\", which CardPen's const.js cardSizes table does not define — " +
					"the renderer would fall back to NaN dimensions", templatePath, sizeName);
				(heightMM, widthMM) = CardSizes.Value[sizeName];
			}

			// #1250: the bleed is deliberately NOT added to the expectation. ImageHelper.ResizeInMM
			// crops it (cover-and-crop) instead of squashing it into the card, so what reaches the
			// printed card is the trim geometry. It is still returned, and named in the failure
			// message, because a bleed that differs between a deck's face and its back is a
			// fabrication defect in its own right even when the geometry is now correct.
			var bleedMM = NumberValue("blsize");
			if (bleedMM > 0)
			{
				var bleedUnit = StringValue("blunit");
				var bleedToMm = bleedUnit == "in" ? 25.4 : bleedUnit == "mm" ? 1.0 : 1.0; // blunit is in/mm in every shipped template
				bleedMM *= bleedToMm;
			}

			if (StringValue("cori") == "landscape")
			{
				(heightMM, widthMM) = (widthMM, heightMM);
			}

			return (heightMM, widthMM, bleedMM, sizeName);
		}
	}
}
