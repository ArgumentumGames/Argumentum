using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Playwright;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
	/// <summary>
	/// Organ for the #190 overflow detector: it must actually REPORT an overflow, not merely
	/// exist.
	///
	/// From its introduction until 2026-09-01 the detector never produced a single finding.
	/// <c>ILocator.EvaluateAsync(expression, arg)</c> calls the expression with the ELEMENT as
	/// first argument and <c>arg</c> as second, but the snippet was declared <c>(args) =&gt;</c> —
	/// so <c>args</c> bound to the &lt;body&gt; element, <c>args.selectors</c> was undefined, and
	/// the JS threw "selectors is not iterable". The call site in HarvestManager catches every
	/// exception to keep the harvest non-fatal and logs a single line, so the feature was dead
	/// while the pipeline stayed green: a muted guard is a no-op, and a no-op that logs is still
	/// a no-op.
	///
	/// A source-level check on the arrow signature would be tautological. This organ instead
	/// drives the real Playwright path against a synthetic CardPen-shaped iframe and asserts the
	/// detector's OUTPUT, so it is red on any future regression of the binding, of the payload
	/// shape, or of the measurement itself.
	///
	/// Built-in inverse control: the fixture holds one overflowing card AND one clean card. An
	/// organ that reported "everything overflows" would pass a bare "&gt; 0" assertion while being
	/// just as broken as one that reports nothing.
	/// </summary>
	public class OverflowDetectorBindingTests : IDisposable
	{
		private readonly string _fixtureDir;

		public OverflowDetectorBindingTests()
		{
			_fixtureDir = Path.Combine(Path.GetTempPath(), "ArgumentumTests", Guid.NewGuid().ToString());
			Directory.CreateDirectory(_fixtureDir);
		}

		/// <summary>
		/// Two cards shaped like CardPen output. The first clips a long family label inside a
		/// fixed-height box with overflow:hidden — the exact mechanism measured on the 60x113
		/// geometry of #1250, where "Erreur de raisonnement" wraps onto a second line and is cut
		/// by max-height. The second holds a short label that fits.
		/// </summary>
		private const string CardFixtureHtml = @"<!DOCTYPE html>
<html><head><meta charset=""utf-8""><style>
  card { display: block; width: 240px; margin: 8px; font-family: sans-serif; }
  .famille { height: 20px; overflow: hidden; font-size: 18px; line-height: 20px; }
</style></head><body>
  <card>
    <div class=""cardName"">carte-qui-deborde</div>
    <div class=""famille"">Erreur de raisonnement tres longue qui passe a la ligne</div>
  </card>
  <card>
    <div class=""cardName"">carte-propre</div>
    <div class=""famille"">Court</div>
  </card>
</body></html>";

		[Fact]
		public async Task DetectAsync_OnACardThatClipsItsFamilyLabel_ReportsTheOverflow()
		{
			var innerPath = Path.Combine(_fixtureDir, "cards.html");
			File.WriteAllText(innerPath, CardFixtureHtml);

			var hostPath = Path.Combine(_fixtureDir, "host.html");
			File.WriteAllText(hostPath,
				"<!DOCTYPE html><html><body><iframe id=\"cpOutput\" width=\"400\" height=\"400\" src=\"cards.html\"></iframe></body></html>");

			Microsoft.Playwright.Program.Main(new[] { "install", "chromium" })
				.Should().Be(0, "the organ drives the real Playwright path, not a stub");

			using var playwright = await Playwright.CreateAsync();
			await using var browser = await playwright.Chromium.LaunchAsync(
				new BrowserTypeLaunchOptions { Headless = true });
			var page = await browser.NewPageAsync();
			await page.GotoAsync(new Uri(hostPath).AbsoluteUri);

			var report = await OverflowDetector.DetectAsync(
				page.FrameLocator("#cpOutput"), "Synthetic", "fr");

			report.Cards.Should().HaveCount(2,
				"the detector walks every <card> of the iframe and keeps the clean ones in the report");

			var overflowing = report.Cards.Where(c => c.Findings.Any()).ToList();
			overflowing.Should().HaveCount(1,
				"exactly one of the two fixture cards clips its family label — a detector that " +
				"reported both, or neither, would be as broken as the pre-fix binding");

			overflowing[0].CardName.Should().Be("carte-qui-deborde");
			var finding = overflowing[0].Findings.Single();
			finding.Selector.Should().Be(".famille");
			finding.ExcessHeight.Should().BeGreaterThan(2,
				"the clipped second line must exceed the default 2 px tolerance");

			report.CardsWithOverflowCount.Should().Be(1,
				"the aggregate the harvest logs must agree with the per-card findings");
		}

		/// <summary>
		/// Fixture for the two defects seen on the 25/09 bundle (#1567) that the self-overflow
		/// predicate alone cannot see: a title crossing out of its banner (the es
		/// "Quaternio terminorum" case — the last line slides under the green banner and the
		/// illustration disappears) and a text block cut by the card edge (the Vertues remark
		/// sliced at the bottom of the card). The third card is clean: an organ that flagged
		/// every card would be as broken as one that flags none.
		/// </summary>
		private const string GeometryFixtureHtml = @"<!DOCTYPE html>
<html><head><meta charset=""utf-8""><style>
  card { display: block; width: 240px; margin: 8px; font-family: sans-serif; }
</style></head><body>
  <card>
    <div class=""cardName"">titre-qui-passe-sous-le-bandeau</div>
    <div class=""header"" style=""height: 30px; background: #008000;"">
      <div class=""title"" style=""font-size: 16px; line-height: 20px;"">Quaternio terminorum (falacia de los cuatro terminos) muy largo</div>
    </div>
    <div class=""imageSection"" style=""height: 60px; background: #ccc;"">image</div>
  </card>
  <card style=""height: 120px; overflow: hidden;"">
    <div class=""cardName"">texte-coupe-par-le-bas</div>
    <div class=""texte"" style=""height: 200px;"">Un texte qui tient dans sa boite mais pas dans la carte.</div>
  </card>
  <card>
    <div class=""cardName"">carte-propre-geometrie</div>
    <div class=""header"" style=""height: 30px; background: #008000;"">
      <div class=""title"" style=""font-size: 12px; line-height: 14px;"">Titre court</div>
    </div>
    <div class=""imageSection"" style=""height: 60px; background: #ccc;"">image</div>
  </card>
</body></html>";

		[Fact]
		public async Task DetectAsync_OnTitleCrossingItsBannerAndOnTextCutByTheCard_ReportsBothKinds()
		{
			var innerPath = Path.Combine(_fixtureDir, "geometry.html");
			File.WriteAllText(innerPath, GeometryFixtureHtml);

			var hostPath = Path.Combine(_fixtureDir, "host-geometry.html");
			File.WriteAllText(hostPath,
				"<!DOCTYPE html><html><body><iframe id=\"cpOutput\" width=\"400\" height=\"600\" src=\"geometry.html\"></iframe></body></html>");

			Microsoft.Playwright.Program.Main(new[] { "install", "chromium" })
				.Should().Be(0, "the organ drives the real Playwright path, not a stub");

			using var playwright = await Playwright.CreateAsync();
			await using var browser = await playwright.Chromium.LaunchAsync(
				new BrowserTypeLaunchOptions { Headless = true });
			var page = await browser.NewPageAsync();
			await page.GotoAsync(new Uri(hostPath).AbsoluteUri);

			var report = await OverflowDetector.DetectAsync(
				page.FrameLocator("#cpOutput"), "SyntheticGeometry", "es");

			report.Cards.Should().HaveCount(3);

			// (a) The title grows past its fixed-height banner: nothing overflows INSIDE the
			// title box, so the self predicate is blind to it by construction.
			var banner = report.Cards.Single(c => c.CardName == "titre-qui-passe-sous-le-bandeau");
			banner.Findings.Should().Contain(f => f.Kind == "container" && f.Selector == ".title",
				"a title whose box crosses its banner is the #1567 Quaternio defect");
			banner.Findings.Should().NotContain(f => f.Kind == "self",
				"the title fits inside its own box — only the container comparison sees the defect");

			// (b) The text block fits its own box but is cut by the card edge.
			var clipped = report.Cards.Single(c => c.CardName == "texte-coupe-par-le-bas");
			clipped.Findings.Should().Contain(f => f.Kind == "card" && f.Selector == ".texte",
				"a block reaching past the card edge is the #1567 Vertues remark defect");
			clipped.Findings.Should().NotContain(f => f.Kind == "self",
				"the text fits its own box — the clip happens at the card boundary");

			// (c) Inverse control: the same shape, correctly sized, stays silent.
			var clean = report.Cards.Single(c => c.CardName == "carte-propre-geometrie");
			clean.Findings.Should().BeEmpty(
				"an organ that flags every card would be as broken as one that flags none");

			report.CardsWithOverflowCount.Should().Be(2);
		}

		/// <summary>
		/// Fixture reproducing the REAL Sophismes face DOM as committed in
		/// Argumentum_Fallacies_Face{,_2,_Web}_*.json — the reservation ai-01 voiced on #1568:
		/// the earlier geometry fixture hosted .title inside .header, a shape the real template
		/// never produces. In the committed template .title is the sibling AFTER .image inside
		/// .imageSection (.imageSection &gt; .image + .title), with .image in flex-grow:1;
		/// min-height:0 and .title in flex-shrink:0. When the wrapped title is taller than the
		/// space the .body column allots to .imageSection, .image collapses to min-height:0 and
		/// the title's box crosses .imageSection's bottom edge, sliding under the .texte banner
		/// below — the exact geometry of the es "Quaternio terminorum" card of the 25/09 bundle.
		/// The second card is the same DOM with a one-line title: an organ that flagged it too
		/// would be as broken as one that flags nothing.
		/// </summary>
		private const string RealSophismesFaceFixtureHtml = @"<!DOCTYPE html>
<html><head><meta charset=""utf-8""><style>
  /* Structure and flex properties mirrored from Argumentum_Fallacies_Face_fr.json */
  card { display: block; width: 240px; height: 360px; margin: 8px; font-family: sans-serif; }
  .cardName { display: none; }
  .body { display: flex; flex-flow: column nowrap; height: 100%; }
  .header { display: flex; justify-content: center; }
  .famille { font-size: 16px; line-height: 30px; }
  .imageSection { min-height: 0; flex-shrink: 1; display: flex; flex-flow: column;
                  justify-content: flex-start; align-items: flex-start; width: 100%; height: 100%; }
  .image { width: 100%; flex-grow: 1; min-height: 0; background: #ccc; }
  .image img { max-width: 100%; max-height: 100%; }
  .title { width: 100%; box-sizing: border-box; flex-shrink: 0; min-height: 2.5em;
           display: flex; justify-content: center; align-items: center;
           font-size: 16px; line-height: 20px; letter-spacing: 0.05em; text-transform: uppercase;
           padding: 9px 12px; background: #008000; color: #fff; }
  .title > div { flex-shrink: 1; width: 100%; }
  .texte { padding: 50px 0; background: #008000; color: #fff; }
</style></head><body>
  <card>
    <div class=""cardName"">titre-reel-dom-qui-deborde</div>
    <div class=""body"">
      <div class=""header"">
        <div class=""famille"">Erreurs de raisonnement</div>
      </div>
      <div class=""imageSection"">
        <div class=""image"">
          <img src=""data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw=="">
        </div>
        <div class=""title"">
          <div>RAISONNEMENT RAISONNEMENT RAISONNEMENT RAISONNEMENT RAISONNEMENT RAISONNEMENT RAISONNEMENT RAISONNEMENT RAISONNEMENT RAISONNEMENT RAISONNEMENT RAISONNEMENT RAISONNEMENT</div>
        </div>
      </div>
      <div class=""texte"">
        <div class=""desc_fr"">Definition courte</div>
        <div class=""exemple_fr"">Exemple court</div>
      </div>
    </div>
  </card>
  <card>
    <div class=""cardName"">titre-reel-dom-propre</div>
    <div class=""body"">
      <div class=""header"">
        <div class=""famille"">Erreurs de raisonnement</div>
      </div>
      <div class=""imageSection"">
        <div class=""image"">
          <img src=""data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw=="">
        </div>
        <div class=""title"">
          <div>Appel a la nature</div>
        </div>
      </div>
      <div class=""texte"">
        <div class=""desc_fr"">Definition courte</div>
        <div class=""exemple_fr"">Exemple court</div>
      </div>
    </div>
  </card>
</body></html>";

		[Fact]
		public async Task DetectAsync_OnRealSophismesFaceDom_TitleEscapingTheImageSection_ReportsContainerFinding()
		{
			var innerPath = Path.Combine(_fixtureDir, "real-face.html");
			File.WriteAllText(innerPath, RealSophismesFaceFixtureHtml);

			var hostPath = Path.Combine(_fixtureDir, "host-real-face.html");
			File.WriteAllText(hostPath,
				"<!DOCTYPE html><html><body><iframe id=\"cpOutput\" width=\"400\" height=\"600\" src=\"real-face.html\"></iframe></body></html>");

			Microsoft.Playwright.Program.Main(new[] { "install", "chromium" })
				.Should().Be(0, "the organ drives the real Playwright path, not a stub");

			using var playwright = await Playwright.CreateAsync();
			await using var browser = await playwright.Chromium.LaunchAsync(
				new BrowserTypeLaunchOptions { Headless = true });
			var page = await browser.NewPageAsync();
			await page.GotoAsync(new Uri(hostPath).AbsoluteUri);

			var report = await OverflowDetector.DetectAsync(
				page.FrameLocator("#cpOutput"), "RealSophismesFace", "fr");

			report.Cards.Should().HaveCount(2);

			// The too-long title: flex-shrink:0 keeps its box at full content height, .image
			// has collapsed to min-height:0, so the title's box crosses .imageSection's
			// bottom edge and slides under the .texte banner — a "container" finding, the
			// geometry of the es Quaternio terminorum card of #1567.
			var overflower = report.Cards.Single(c => c.CardName == "titre-reel-dom-qui-deborde");
			overflower.Findings.Should().Contain(f => f.Kind == "container" && f.Selector == ".title",
				"in the committed template the title escapes .imageSection, not a .header banner");
			overflower.Findings.Should().NotContain(f => f.Kind == "card",
				"the .texte zone below absorbs the excess: the title crosses its container, " +
				"not the card edge");
			overflower.Findings.Should().NotContain(f => f.Kind == "self",
				"the title has no overflow:hidden: it paints outside its box instead of clipping");
			overflower.Findings.Single(f => f.Selector == ".title").ExcessHeight.Should().BeGreaterThan(2,
				"the escape must exceed the default 2 px tolerance");

			// Inverse control: same real DOM, one-line title — .image flex-grows to fill the
			// leftover space and nothing overflows anywhere.
			var clean = report.Cards.Single(c => c.CardName == "titre-reel-dom-propre");
			clean.Findings.Should().BeEmpty(
				"an organ that flags every card would be as broken as one that flags none");

			report.CardsWithOverflowCount.Should().Be(1);
		}

		public void Dispose()
		{
			if (Directory.Exists(_fixtureDir))
			{
				try { Directory.Delete(_fixtureDir, true); } catch (IOException) { }
			}
		}
	}
}
