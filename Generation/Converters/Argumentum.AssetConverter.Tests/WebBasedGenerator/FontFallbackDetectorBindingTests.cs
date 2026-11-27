using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Playwright;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
	/// <summary>
	/// Organ for the #1485 font fallback detector: it must REPORT the stacks whose declared
	/// families do not paint their letters — through the real Playwright path.
	/// </summary>
	/// <remarks>
	/// Same lesson as <c>OverflowDetectorBindingTests</c>: a source-level check on the arrow
	/// signature would be tautological, and a muted catch can keep a detector dead for months
	/// while the pipeline stays green. This organ drives <c>DetectAsync</c> against an iframe
	/// holding deterministic embedded fonts (built by <c>tools/fonts/make-probe-fonts.py</c>:
	/// a few hundred bytes each, no network, no system font in the measurements) and asserts
	/// its OUTPUT: the verdict of every stack, the letters no declared family paints, the text
	/// nodes affected, the <c>@font-face</c> in error, and the log lines.
	/// <para>
	/// Built-in inverse controls: the fixture holds four defective stacks AND five silent ones
	/// (clean, clean through a companion family — design (a) of #1485 —, generic-only, and
	/// not-laid-out). An organ that flagged every stack, or none, would be as broken as the
	/// other.
	/// </para>
	/// </remarks>
	public class FontFallbackDetectorBindingTests : IDisposable
	{
		private readonly string _fixtureDir;

		public FontFallbackDetectorBindingTests()
		{
			_fixtureDir = Path.Combine(Path.GetTempPath(), "ArgumentumTests", Guid.NewGuid().ToString());
			Directory.CreateDirectory(_fixtureDir);
		}

	private const string LatinOnlyFontBase64 =
		"AAEAAAAKAIAAAwAgT1MvMl8nWgIAAACsAAAAYGNtYXAAEADTAAABDAAAAChnbHlmA8AClAAAATQAAAAiaGVhZF8WQOUAAAFYAAAANmhoZWEDIgEv" +
		"AAABkAAAACRobXR4A+gAAAAAAbQAAAAIbG9jYQARAAAAAAG8AAAABm1heHAABAAGAAABxAAAACBuYW1lFfstKwAAAeQAAAEccG9zdP+fADIAAAMA" +
		"AAAAIAAEAfQBkAAFAAACigJY/7UBXgKKAlj/tQFeADIA+gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABOT05FAEAAIAB+AyD/OAAAAyAAyAAA" +
		"AAEAAAAAAfQCvAAAACAAAAAAAAEAAwAKAAAADAANAAAAAAAcAAAAAAAAAAEAAAAgAAAAfgAAAAEAAQAAAAABkAK8AAMAAAEBAQEBkAAA/nAAAAK8" +
		"AAD9RAAAAAAAAQAAAAEAAOgLARFfDzz1AAMD6AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAgAAAAAAAAABAAADIP84AAAB9AAAAAAAAAAB" +
		"AAAAAAAAAAAAAAAAAAAAAgH0AAAB9AAAAAAAAAARAAAAAQAAAAIABAABAAAAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAYATgADAAEECQABACgAAAAD" +
		"AAEECQACAA4AKAADAAEECQADADIANgADAAEECQAEACgAaAADAAEECQAFABYAkAADAAEECQAGACgApgBBAHIAZwB1AG0AZQBuAHQAdQBtAFAAcgBv" +
		"AGIAZQBMAGEAdABpAG4AUgBlAGcAdQBsAGEAcgBBAHIAZwB1AG0AZQBuAHQAdQBtAFAAcgBvAGIAZQBMAGEAdABpAG4AOwAgADEALgAwAEEAcgBn" +
		"AHUAbQBlAG4AdAB1AG0AUAByAG8AYgBlAEwAYQB0AGkAbgBWAGUAcgBzAGkAbwBuACAAMQAuADAAQQByAGcAdQBtAGUAbgB0AHUAbQBQAHIAbwBi" +
		"AGUATABhAHQAaQBuAAMAAAAAAAD/nAAyAAAAAAAAAAAAAAAAAAAAAAAAAAA=";

	private const string CyrillicCapsFontBase64 =
		"AAEAAAAKAIAAAwAgT1MvMmMXXbMAAACsAAAAYGNtYXAAEAh0AAABDAAAAChnbHlmA8AClAAAATQAAAAiaGVhZF8WQOUAAAFYAAAANmhoZWEDIgEv" +
		"AAABkAAAACRobXR4A+gAAAAAAbQAAAAIbG9jYQARAAAAAAG8AAAABm1heHAABAAGAAABxAAAACBuYW1lF50utQAAAeQAAAEscG9zdP+fADIAAAMQ" +
		"AAAAIAAEAfQBkAAFAAACigJY/7UBXgKKAlj/tQFeADIA+gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABOT05FAEAEEAQvAyD/OAAAAyAAyAAA" +
		"AAEAAAAAAfQCvAAAACAAAAAAAAEAAwAKAAAADAANAAAAAAAcAAAAAAAAAAEAAAQQAAAELwAAAAEAAQAAAAABkAK8AAMAAAEBAQEBkAAA/nAAAAK8" +
		"AAD9RAAAAAAAAQAAAAEAANzm5zlfDzz1AAMD6AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAgAAAAAAAAABAAADIP84AAAB9AAAAAAAAAAB" +
		"AAAAAAAAAAAAAAAAAAAAAgH0AAAB9AAAAAAAAAARAAAAAQAAAAIABAABAAAAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAYATgADAAEECQABACwAAAAD" +
		"AAEECQACAA4ALAADAAEECQADADYAOgADAAEECQAEACwAcAADAAEECQAFABYAnAADAAEECQAGACwAsgBBAHIAZwB1AG0AZQBuAHQAdQBtAFAAcgBv" +
		"AGIAZQBDAHkAcgBDAGEAcABzAFIAZQBnAHUAbABhAHIAQQByAGcAdQBtAGUAbgB0AHUAbQBQAHIAbwBiAGUAQwB5AHIAQwBhAHAAcwA7ACAAMQAu" +
		"ADAAQQByAGcAdQBtAGUAbgB0AHUAbQBQAHIAbwBiAGUAQwB5AHIAQwBhAHAAcwBWAGUAcgBzAGkAbwBuACAAMQAuADAAQQByAGcAdQBtAGUAbgB0" +
		"AHUAbQBQAHIAbwBiAGUAQwB5AHIAQwBhAHAAcwADAAAAAAAA/5wAMgAAAAAAAAAAAAAAAAAAAAAAAAAA";

	private const string CyrillicLowerFontBase64 =
		"AAEAAAAKAIAAAwAgT1MvMmM3XdMAAACsAAAAYGNtYXAAEAi0AAABDAAAAChnbHlmA8AClAAAATQAAAAiaGVhZF8WQOUAAAFYAAAANmhoZWEDIgEv" +
		"AAABkAAAACRobXR4A+gAAAAAAbQAAAAIbG9jYQARAAAAAAG8AAAABm1heHAABAAGAAABxAAAACBuYW1lGPwvegAAAeQAAAE0cG9zdP+fADIAAAMY" +
		"AAAAIAAEAfQBkAAFAAACigJY/7UBXgKKAlj/tQFeADIA+gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABOT05FAEAEMARPAyD/OAAAAyAAyAAA" +
		"AAEAAAAAAfQCvAAAACAAAAAAAAEAAwAKAAAADAANAAAAAAAcAAAAAAAAAAEAAAQwAAAETwAAAAEAAQAAAAABkAK8AAMAAAEBAQEBkAAA/nAAAAK8" +
		"AAD9RAAAAAAAAQAAAAEAANno5N9fDzz1AAMD6AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAgAAAAAAAAABAAADIP84AAAB9AAAAAAAAAAB" +
		"AAAAAAAAAAAAAAAAAAAAAgH0AAAB9AAAAAAAAAARAAAAAQAAAAIABAABAAAAAAACAAAAAAAAAAAAAAAAAAAAAAAAAAYATgADAAEECQABAC4AAAAD" +
		"AAEECQACAA4ALgADAAEECQADADgAPAADAAEECQAEAC4AdAADAAEECQAFABYAogADAAEECQAGAC4AuABBAHIAZwB1AG0AZQBuAHQAdQBtAFAAcgBv" +
		"AGIAZQBDAHkAcgBMAG8AdwBlAHIAUgBlAGcAdQBsAGEAcgBBAHIAZwB1AG0AZQBuAHQAdQBtAFAAcgBvAGIAZQBDAHkAcgBMAG8AdwBlAHIAOwAg" +
		"ADEALgAwAEEAcgBnAHUAbQBlAG4AdAB1AG0AUAByAG8AYgBlAEMAeQByAEwAbwB3AGUAcgBWAGUAcgBzAGkAbwBuACAAMQAuADAAQQByAGcAdQBt" +
		"AGUAbgB0AHUAbQBQAHIAbwBiAGUAQwB5AHIATABvAHcAZQByAAMAAAAAAAD/nAAyAAAAAAAAAAAAAAAAAAAAAAAAAAA=";

		private const string Latin = "ArgumentumProbeLatin";
		private const string CyrillicCaps = "ArgumentumProbeCyrCaps";
		private const string CyrillicLower = "ArgumentumProbeCyrLower";

		private static string Face(string family, string base64)
			=> "@font-face { font-family: '" + family + "'; src: url(data:font/ttf;base64," + base64
			   + ") format('truetype'); }";

		/// <summary>
		/// A face whose source is not a font at all: the page uses it, the browser tries to load
		/// it, fails, and leaves the face in status <c>error</c> (measured 25/11/2026) — the #1485
		/// TrendSlab shape: declared, needed by the page, never loaded.
		/// </summary>
		private const string DeadFaceCss
			= "@font-face { font-family: 'DeadFace'; src: url(data:font/woff2;base64,AAAA) format('woff2'); }";

		/// <summary>
		/// Eight cards shaped like CardPen output, one per stack shape of #1485:
		/// <list type="number">
		/// <item>a dead family alone with Latin text (the face in error);</item>
		/// <item>a Latin-only family with Cyrillic text (Barlow, 62/62 letters missing);</item>
		/// <item>the same family with Latin text (it IS loaded and works);</item>
		/// <item>Latin family + Cyrillic companion — design (a), clean;</item>
		/// <item>a lower-case-only Cyrillic family with one capital letter — partial;</item>
		/// <item>a capital-only family + a lower-case one — split between declared families;</item>
		/// <item>Cyrillic text inside a display:none card — counted, never reported;</item>
		/// <item>a stack naming no family at all — generic, not a finding.</item>
		/// </list>
		/// </summary>
		private string EightStacksHtml()
		{
			var latin = "'" + Latin + "', sans-serif";
			var companion = "'" + Latin + "', '" + CyrillicLower + "', sans-serif";
			return "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><style>"
				+ DeadFaceCss
				+ Face(Latin, LatinOnlyFontBase64)
				+ Face(CyrillicCaps, CyrillicCapsFontBase64)
				+ Face(CyrillicLower, CyrillicLowerFontBase64)
				+ "card { display: block; width: 400px; margin: 8px; }"
				+ ".cardName { display: none; }"
				+ "#absente { font-family: 'DeadFace', sans-serif; }"
				+ "#sans-cyr { font-family: " + latin + "; }"
				+ "#propre { font-family: " + latin + "; }"
				+ "#compagne { font-family: " + companion + "; }"
				+ "#partielle { font-family: '" + CyrillicLower + "', sans-serif; }"
				+ "#partagee { font-family: '" + CyrillicCaps + "', '" + CyrillicLower + "', sans-serif; }"
				+ "#cachee { font-family: " + latin + "; display: none; }"
				+ "#generique { font-family: sans-serif; }"
				+ "</style></head><body>"
				+ "<card><div class=\"cardName\">carte-police-absente</div><div id=\"absente\" class=\"texte\">Texte orphelin</div></card>"
				+ "<card><div class=\"cardName\">carte-sans-cyrillique</div><div id=\"sans-cyr\" class=\"texte\">Логическая ошибка</div></card>"
				+ "<card><div class=\"cardName\">carte-propre</div><div id=\"propre\" class=\"texte\">Logical fallacy</div></card>"
				+ "<card><div class=\"cardName\">carte-avec-compagne</div><div id=\"compagne\" class=\"texte\">ошибка логическая · Logical fallacy</div></card>"
				+ "<card><div class=\"cardName\">carte-partielle</div><div id=\"partielle\" class=\"texte\">Ошибка логическая</div></card>"
				+ "<card><div class=\"cardName\">carte-partagee</div><div id=\"partagee\" class=\"texte\">Логическая Ошибка</div></card>"
				+ "<card><div class=\"cardName\">carte-cachee</div><div id=\"cachee\" class=\"texte\">Скрытый текст</div></card>"
				+ "<card><div class=\"cardName\">carte-generique</div><div id=\"generique\" class=\"texte\">Texte système</div></card>"
				+ "</body></html>";
		}

		private string WriteFixture(string name, string cardsHtml)
		{
			var inner = Path.Combine(_fixtureDir, name + "-cards.html");
			File.WriteAllText(inner, cardsHtml, new UTF8Encoding(false));
			var host = Path.Combine(_fixtureDir, name + "-host.html");
			File.WriteAllText(host,
				"<!DOCTYPE html><html><body><iframe id=\"cpOutput\" width=\"600\" height=\"600\" src=\""
				+ name + "-cards.html\"></iframe></body></html>",
				new UTF8Encoding(false));
			return host;
		}

		private static void InstallChromium()
		{
			Microsoft.Playwright.Program.Main(new[] { "install", "chromium" })
				.Should().Be(0, "the organ drives the real Playwright path, not a stub");
		}

		[Fact]
		public async Task DetectAsync_OnFourDefectiveStacksAndFiveSilentOnes_ReportsExactlyTheFour()
		{
			var hostPath = WriteFixture("eight", EightStacksHtml());
			InstallChromium();

			using var playwright = await Playwright.CreateAsync();
			await using var browser = await playwright.Chromium.LaunchAsync(
				new BrowserTypeLaunchOptions { Headless = true });
			var page = await browser.NewPageAsync();
			await page.GotoAsync(new Uri(hostPath).AbsoluteUri);

			var iframe = page.FrameLocator("#cpOutput");
			var report = await FontFallbackDetector.DetectAsync(iframe, "SyntheticFonts", "ru", "face");

			report.CardSetName.Should().Be("SyntheticFonts");
			report.Language.Should().Be("ru");
			report.Side.Should().Be("face");

			// The walk sees everything: 8 card names (not laid out) + 8 text blocks (7 laid out,
			// the hidden card included in the count but never in a finding).
			report.IsEmpty.Should().BeFalse();
			report.Stats.TextNodes.Should().Be(16);
			report.Stats.RenderedTextNodes.Should().Be(7);
			report.Stats.HiddenTextNodes.Should().Be(9);
			report.Stats.Groups.Should().Be(9);

			// ---- the four defective stacks, looked up by declared families (stack order)
			// rather than by the computed string, whose quoting is a CSSOM detail.
			FontStackProbe Probe(string script, params string[] families)
				=> report.Probes.Single(p => p.Script == script
					&& p.Coverage.Select(c => c.Family).SequenceEqual(families));

			var absent = Probe("Cyrillic", Latin);
			absent.Verdict.Should().Be(FontFallbackDetector.VerdictFallback);
			absent.RenderedBy.Should().BeNull("no declared family paints any of these letters");
			absent.LetterCount.Should().Be(17);
			absent.MissingLetterCount.Should().Be(17);
			absent.Coverage.Single().Covered.Should().Be(0,
				"the Latin-only family is loaded and paints none of the Cyrillic letters — the Barlow case of #1485");
			absent.TextNodes.Should().Be(2);
			absent.RenderedTextNodes.Should().Be(1, "the hidden card of this stack is counted, not reported");
			absent.AffectedTextNodes.Should().Be(1);
			absent.Examples.Single().CardName.Should().Be("carte-sans-cyrillique");
			absent.Examples.Single().Selector.Should().Be("div.texte");
			absent.Examples.Single().Snippet.Should().Be("Логическая ошибка");

			var deadFace = Probe("Latin", "DeadFace");
			deadFace.Verdict.Should().Be(FontFallbackDetector.VerdictFallback);
			deadFace.LetterCount.Should().Be(11);
			deadFace.AffectedTextNodes.Should().Be(1);
			deadFace.Examples.Single().CardName.Should().Be("carte-police-absente");

			var partial = Probe("Cyrillic", CyrillicLower);
			partial.Verdict.Should().Be(FontFallbackDetector.VerdictPartial);
			partial.Missing.Should().Be("О", "the capital О is outside the lower-case companion family");
			partial.LetterCount.Should().Be(13);
			partial.RenderedBy.Should().Be(CyrillicLower);
			partial.Coverage.Single().Covered.Should().Be(12);
			partial.AffectedTextNodes.Should().Be(1);
			partial.Examples.Single().CardName.Should().Be("carte-partielle");

			var split = Probe("Cyrillic", CyrillicCaps, CyrillicLower);
			split.Verdict.Should().Be(FontFallbackDetector.VerdictSplit);
			split.MissingLetterCount.Should().Be(0, "declared families paint everything — just not the same one");
			split.Coverage.Select(c => c.Covered).Should().ContainInOrder(new[] { 2, 11 },
				"stack order: the capital Л and О by the first family, the 11 lower-case letters by the second");
			split.RenderedBy.Should().Be(CyrillicCaps + " | " + CyrillicLower);
			split.AffectedTextNodes.Should().Be(1);
			split.Examples.Single().CardName.Should().Be("carte-partagee");

			// ---- the five silent stacks (inverse controls)
			var companion = Probe("Cyrillic", Latin, CyrillicLower);
			companion.Verdict.Should().Be(FontFallbackDetector.VerdictClean,
				"design (a) of #1485: a Latin family + a Cyrillic companion paints every letter with a declared family");
			companion.RenderedBy.Should().Be(CyrillicLower);
			companion.AffectedTextNodes.Should().Be(0);
			companion.Examples.Should().BeEmpty();
			companion.IsFinding.Should().BeFalse();

			var hidden = report.Probes.Where(p => p.RenderedTextNodes == 0).ToList();
			hidden.Should().ContainSingle("the eight card names are not laid out: one stack, zero node reported");
			hidden[0].IsFinding.Should().BeFalse("text that is not laid out is never a finding");

			var generic = report.Probes.Single(p => p.Coverage.Count == 0);
			generic.Verdict.Should().Be(FontFallbackDetector.VerdictGeneric,
				"a stack naming no family declares nothing — not a finding");
			generic.AffectedTextNodes.Should().Be(0);
			generic.IsFinding.Should().BeFalse();

			Probe("Latin", Latin).Verdict.Should().Be(FontFallbackDetector.VerdictClean);
			Probe("Latin", Latin, CyrillicLower).Verdict.Should().Be(FontFallbackDetector.VerdictClean);

			// ---- aggregate and faces
			report.FindingCount.Should().Be(4);
			report.AffectedTextNodes.Should().Be(4, "one laid-out text block per defective stack");
			report.HasFinding.Should().BeTrue();

			report.Stats.DeclaredFaces.Should().Be(4);
			report.Stats.ErrorFaces.Should().Be(1);
			var errors = report.ErrorFaces.ToList();
			errors.Should().ContainSingle();
			errors[0].Family.Should().Be("DeadFace");
			errors[0].IsError.Should().BeTrue();

			// ---- the probe leaves no trace in the document it measured
			var familiesInDocument = await iframe.Locator("body").EvaluateAsync<string[]>(
				"(element) => Array.from(element.ownerDocument.fonts).map(f => f.family)");
			familiesInDocument.Should().NotContain(FontFallbackDetector.SentinelFamily,
				"the sentinel face is added for the measurement and deleted afterwards");
			familiesInDocument.Should().BeEquivalentTo(new[] { "DeadFace", Latin, CyrillicCaps, CyrillicLower });

			// ---- the log lines: 1 face in error + 4 findings + 1 summary, all Problem
			var lines = FontFallbackDetector.BuildLogLines(report, "qa/fonts.md");
			lines.Should().HaveCount(6);
			lines.Should().OnlyContain(l => l.IsProblem);
			lines[0].Message.Should().Be(
				"[FONT-LOAD-FAILURE] SyntheticFonts ru face: @font-face 'DeadFace' (weight normal, style normal) " +
				"is in status 'error' — declared, needed by the page, not loaded.");
			lines.Skip(1).Take(4).Should().OnlyContain(l => l.Message.StartsWith("[FONT-FALLBACK] SyntheticFonts ru face: "));
			lines.Select(l => l.Message).Should().Contain(m => m.Contains(
				"Cyrillic (400, normal) — '" + CyrillicLower + "' renders 12/13 letters — the other 1 (О) go to the system fallback font"));
			lines[5].Message.Should().Be(
				"[Fonts] SyntheticFonts ru face: 4 face(s) declared — 3 loaded, 1 in error, 0 unused; " +
				"9 stack(s) x script(s) probed — 4 with a fallback, 4 text node(s) affected. Report: qa/fonts.md");

			// ---- the markdown report names the finding and its example
			var md = FontFallbackDetector.FormatMarkdown(report);
			md.Should().Contain("| Cyrillic | fallback |");
			md.Should().Contain("carte-sans-cyrillique");
			md.Should().Contain("## Faces in error");
			md.Should().Contain("| `DeadFace` |");
		}

		/// <summary>
		/// The clean side of #1485: the substitution chosen for the Russian templates is a
		/// companion family in the same stack. Nothing must be reported, the third declared face
		/// must be counted as unused (declared, never needed — not a defect), and the summary must
		/// be a single Info line.
		/// </summary>
		[Fact]
		public async Task DetectAsync_OnAStackWithACyrillicCompanionFamily_ReportsNoFinding()
		{
			var html = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><style>"
				+ Face(Latin, LatinOnlyFontBase64)
				+ Face(CyrillicCaps, CyrillicCapsFontBase64)
				+ Face(CyrillicLower, CyrillicLowerFontBase64)
				+ "card { display: block; width: 400px; margin: 8px; }"
				+ ".cardName { display: none; }"
				+ "#latin { font-family: '" + Latin + "', sans-serif; }"
				+ "#compagne { font-family: '" + Latin + "', '" + CyrillicLower + "', sans-serif; }"
				+ "#systeme { font-family: sans-serif; }"
				+ "</style></head><body>"
				+ "<card><div class=\"cardName\">carte-latine</div><div id=\"latin\" class=\"texte\">Logical fallacy</div></card>"
				+ "<card><div class=\"cardName\">carte-compagne</div><div id=\"compagne\" class=\"texte\">ошибка логическая</div></card>"
				+ "<card><div class=\"cardName\">carte-systeme</div><div id=\"systeme\" class=\"texte\">Texte système</div></card>"
				+ "</body></html>";
			var hostPath = WriteFixture("clean", html);
			InstallChromium();

			using var playwright = await Playwright.CreateAsync();
			await using var browser = await playwright.Chromium.LaunchAsync(
				new BrowserTypeLaunchOptions { Headless = true });
			var page = await browser.NewPageAsync();
			await page.GotoAsync(new Uri(hostPath).AbsoluteUri);

			var report = await FontFallbackDetector.DetectAsync(
				page.FrameLocator("#cpOutput"), "CleanBack", "ru", "back");

			report.IsEmpty.Should().BeFalse();
			report.FindingCount.Should().Be(0);
			report.AffectedTextNodes.Should().Be(0);
			report.HasFinding.Should().BeFalse("a companion family is a declared family: nothing fell back");
			report.Stats.DeclaredFaces.Should().Be(3);
			report.Stats.ErrorFaces.Should().Be(0);
			report.Stats.UnloadedFaces.Should().Be(1,
				"the capital-only face is declared and the page needs none of its letters — declared but unused is not a defect");

			var companion = report.Probes.Single(p => p.Script == "Cyrillic");
			companion.Verdict.Should().Be(FontFallbackDetector.VerdictClean);
			companion.RenderedBy.Should().Be(CyrillicLower);
			companion.MissingLetterCount.Should().Be(0);

			var lines = FontFallbackDetector.BuildLogLines(report);
			lines.Should().ContainSingle("a clean probe is one summary line, nothing else");
			lines[0].IsProblem.Should().BeFalse();
			lines[0].Message.Should().StartWith(
				"[Fonts] CleanBack ru back: 3 face(s) declared — 2 loaded, 0 in error, 1 unused;");
		}

		/// <summary>
		/// A probe that measures nothing must say so at Problem level — "no fallback detected"
		/// over zero laid-out text node would read as a clean bill while the instrument was
		/// blind. Same family as the XSLT fallback that passed silently for months.
		/// </summary>
		[Fact]
		public async Task DetectAsync_OnAnIframeWithNoLaidOutText_SaysSoAtProblemLevel()
		{
			var html = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><style>"
				+ Face(Latin, LatinOnlyFontBase64)
				+ "card { display: none; }"
				+ "#cachee { font-family: '" + Latin + "', sans-serif; }"
				+ "</style></head><body>"
				+ "<card><div class=\"cardName\">carte-cachee</div><div id=\"cachee\" class=\"texte\">Логическая ошибка</div></card>"
				+ "</body></html>";
			var hostPath = WriteFixture("hidden-only", html);
			InstallChromium();

			using var playwright = await Playwright.CreateAsync();
			await using var browser = await playwright.Chromium.LaunchAsync(
				new BrowserTypeLaunchOptions { Headless = true });
			var page = await browser.NewPageAsync();
			await page.GotoAsync(new Uri(hostPath).AbsoluteUri);

			var report = await FontFallbackDetector.DetectAsync(
				page.FrameLocator("#cpOutput"), "HiddenOnly", "fr", "face");

			report.IsEmpty.Should().BeTrue("nothing is laid out, so nothing was measured");
			report.Stats.TextNodes.Should().Be(2);
			report.Stats.RenderedTextNodes.Should().Be(0);
			report.HasFinding.Should().BeFalse("the Cyrillic fallback is real but not painted — out of scope");

			var lines = FontFallbackDetector.BuildLogLines(report, "qa/hidden.md");
			lines.Should().ContainSingle();
			lines[0].IsProblem.Should().BeTrue();
			lines[0].Message.Should().Be(
				"[Fonts] HiddenOnly fr face: no laid-out text found in the iframe — this probe says nothing about fonts. Report: qa/hidden.md");

			var md = FontFallbackDetector.FormatMarkdown(report);
			md.Should().Contain("this report says nothing about fonts");
		}

		/// <summary>
		/// <c>tools/fonts/make-probe-fonts.py</c> builds the four fonts deterministically and
		/// <c>--check</c> compares the C# constants against it (that comparison needs Python).
		/// These fingerprints pin the BYTES here: an accidentally edited constant is red on every
		/// machine, Python or not.
		/// </summary>
		[Fact]
		public void EmbeddedProbeFonts_MatchThePinnedBytesOfTheirBuilder()
		{
			AssertFingerprint("ArgumentumProbeSentinel", FontFallbackDetector.SentinelFontBase64, 824,
				"C079BCE3B49277577C987B60D0DF6CB807D878470F2DDE2228D1B75360818EF5");
			AssertFingerprint(Latin, LatinOnlyFontBase64, 800,
				"4646DCF6EFC366CD519EF68D96EA71D2CFA10F2DB1869E2A3A2997CDE68F8CA7");
			AssertFingerprint(CyrillicCaps, CyrillicCapsFontBase64, 816,
				"255CBDEC2401FE4FE268391B1A3FA872DF1F2D50577974B4F321D9BD3F535EB2");
			AssertFingerprint(CyrillicLower, CyrillicLowerFontBase64, 824,
				"578FDC8BA10272630C461903FA3D614537DC72B2DBA8EBB83EF16D0F13A8336A");
		}

		private static void AssertFingerprint(string family, string base64, int expectedLength, string expectedSha256)
		{
			var bytes = Convert.FromBase64String(base64);
			bytes.Length.Should().Be(expectedLength, "{0} must keep the exact size the builder emits", family);
			Convert.ToHexString(SHA256.HashData(bytes)).Should().Be(expectedSha256,
				"{0} is built by tools/fonts/make-probe-fonts.py — rerun it with --csharp and --check instead of editing the constant",
				family);
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
