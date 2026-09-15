using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// Contract: a mind map the pipeline generates must also be <i>viewable</i>. A document is
	/// delivered through an HTML wrapper — the wrapper is what carries svg-pan-zoom, the click
	/// handlers and the modal. A document with no wrapper is generated, committed, and reachable
	/// by nobody.
	///
	/// <para><b>Why this exists (#1253 §1).</b> The card mind map
	/// (<c>Argumentum_Fallacies_MindMap_cards_fr.mm</c>) is the only document that embeds the 176
	/// card thumbnails, and it was the only one of the three families with <b>no wrapper at all</b>
	/// and <b>no viewBox</b> on its single variant. Measured on master before the fix: a 5.3 MB raw
	/// Batik export, <c>width="8293" height="20229"</c>, no <c>viewBox</c>, referenced by zero HTML
	/// file, zero doc and zero test — while its two sibling families each ship
	/// <c>{main, _ext}</c> wrappers in all 8 languages.</para>
	///
	/// <para>That explains a measurement that had looked contradictory: the shipped SVGs carry no
	/// distortion whatsoever (11 440 <c>&lt;image&gt;</c> at ratio 1.000), yet the card map could
	/// still not be displayed correctly — an SVG without a viewBox is stretched non-uniformly by
	/// any container that constrains both dimensions, and the wrapper box is exactly that
	/// (<c>96vw × 93vh</c>). The defect was never a deformation inside a file; it was the absence
	/// of the file that would have shown it right.</para>
	///
	/// <para><b>Scope is derived from the configuration</b>, not tabulated: both creator configs are
	/// walked and every enabled document is asserted. A hardcoded list would keep passing after a
	/// fourth family is added without a wrapper — which is precisely how this one stayed invisible.
	/// <see cref="Scope_CoversTheCardThumbnailDocument"/> guards the scope itself.</para>
	/// </summary>
	public class MindMapDeliverabilityContractTests
	{
		private sealed record MindMapDoc(
			string Family,
			string DocumentName,
			IReadOnlyList<SVGFreemindMap> Variants,
			bool CarriesCardThumbnails);

		private static List<MindMapDoc> EnabledDocuments()
		{
			var config = new AssetConverterConfig();

			var documents = config.FallacyMindMapCreatorConfig.DocumentConfigs
				.Where(d => d.Enabled)
				.Select(d => new MindMapDoc("Fallacies", d.DocumentName, d.SVGMaps, d.InsertCardsThumbnails))
				.ToList();

			documents.AddRange(config.VirtueMindMapCreatorConfig.DocumentConfigs
				.Where(d => d.Enabled)
				.Select(d => new MindMapDoc("Virtues", d.DocumentName, d.SVGMaps, d.InsertCardsThumbnails)));

			return documents;
		}

		public static IEnumerable<object[]> DocumentNames() =>
			EnabledDocuments().Select(d => new object[] { d.DocumentName });

		private static MindMapDoc Document(string documentName) =>
			EnabledDocuments().Single(d => d.DocumentName == documentName);

		[Theory]
		[MemberData(nameof(DocumentNames))]
		public void EveryEnabledDocument_ShipsWithAViewer(string documentName)
		{
			var document = Document(documentName);

			var wrapped = document.Variants
				.Where(v => v.Enabled && v.HtmlWrappers != null && v.HtmlWrappers.Count > 0)
				.ToList();

			wrapped.Should().NotBeEmpty(
				"{0} ({1}) declares {2} enabled SVG variant(s) and not one of them carries an HtmlWrapper. " +
				"The pipeline will produce the SVG, commit it, and leave it unreachable: the wrapper is the " +
				"only viewer. This is the exact state the card map was in before #1253 §1.",
				documentName, document.Family, document.Variants.Count(v => v.Enabled));
		}

		[Theory]
		[MemberData(nameof(DocumentNames))]
		public void EveryWrappedVariant_DeclaresAViewBox(string documentName)
		{
			var document = Document(documentName);

			foreach (var variant in document.Variants.Where(v =>
				         v.Enabled && v.HtmlWrappers != null && v.HtmlWrappers.Count > 0))
			{
				IsUsableViewBox(variant.SvgViewBox).Should().BeTrue(
					"{0} / {1} is embedded in a wrapper whose box constrains BOTH dimensions " +
					"(SvgWidth='{2}', SvgHeight='{3}'), so without a parseable viewBox the browser " +
					"stretches it non-uniformly. Found: '{4}'.",
					documentName, variant.DocumentName, variant.SvgWidth, variant.SvgHeight,
					variant.SvgViewBox ?? "<null>");
			}
		}

		/// <summary>
		/// Guards the scope, not the config. If the card document is ever renamed, disabled or moved
		/// to another creator, this test goes red instead of the suite quietly ceasing to cover the
		/// one document the contract was written for.
		/// </summary>
		[Fact]
		public void Scope_CoversTheCardThumbnailDocument()
		{
			var documents = EnabledDocuments();

			documents.Should().NotBeEmpty("the contract is meaningless over an empty scope");

			var cardDocuments = documents.Where(d => d.CarriesCardThumbnails).ToList();
			cardDocuments.Should().HaveCount(1,
				"exactly one enabled document is expected to embed the card thumbnails; found {0}",
				cardDocuments.Count);
		}

		/// <summary>
		/// Inverse control. A guard that has only ever seen corrected configuration proves nothing
		/// about its own sensitivity. This rebuilds the pre-#1253 card document verbatim — one
		/// variant, no wrapper, no viewBox — and requires it to be rejected on both counts.
		/// </summary>
		[Fact]
		public void Detector_RejectsThePre1253CardDocument()
		{
			var regressed = new SVGFreemindMap
			{
				Enabled = true,
				DocumentName = "links.svg",
				WrapNodeByLink = true,
				RemoveImages = true,
				SetSVGNodeAttributes = false,
			};

			regressed.HtmlWrappers.Should().BeEmpty("the pre-fix variant declared no wrapper");
			IsUsableViewBox(regressed.SvgViewBox).Should().BeFalse(
				"the pre-fix variant declared no viewBox either");
		}

		/// <summary>
		/// Second inverse control: a wrapper alone is not enough. A variant can be perfectly
		/// reachable and still render stretched. This proves the two assertions are independent
		/// rather than one check wearing two hats.
		/// </summary>
		[Fact]
		public void Detector_SeparatesReachableFromCorrectlyShaped()
		{
			var reachableButUnshaped = new SVGFreemindMap
			{
				Enabled = true,
				DocumentName = "content.svg",
				SvgWidth = "96vw",
				SvgHeight = "93vh",
				HtmlWrappers = new List<Argumentum.AssetConverter.DocumentConfig>
				{
					new() { DocumentName = "whatever.html" }
				}
			};

			reachableButUnshaped.HtmlWrappers.Should().NotBeEmpty("it is reachable");
			IsUsableViewBox(reachableButUnshaped.SvgViewBox).Should().BeFalse(
				"but it would still be stretched by the 96vw x 93vh box");

			IsUsableViewBox("0 0 8500 20500").Should().BeTrue("the corrected form must be accepted");
			IsUsableViewBox("0 0 8500").Should().BeFalse("three numbers are not a viewBox");
			IsUsableViewBox("nope").Should().BeFalse("a non-numeric value is not a viewBox");
		}

		private static bool IsUsableViewBox(string? viewBox)
		{
			if (string.IsNullOrWhiteSpace(viewBox)) return false;

			var parts = viewBox.Split(new[] { ' ', ',' }, System.StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length != 4) return false;

			return parts.All(p => double.TryParse(p, NumberStyles.Float, CultureInfo.InvariantCulture, out _));
		}
	}
}
