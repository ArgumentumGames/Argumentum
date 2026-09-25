using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Regression tests for #1537: the CardPen template's <c>css</c> key carries hard-coded
	/// FRENCH text that no mustache substitution can ever reach — the Rules variant footers are
	/// <c>content:</c> pseudo-elements (<c>[class~="2..6"]:before { content: "L'ÉCOLE DES
	/// MENTEURS"; }</c> …), so a mustache-only localization ships them French in all 8 languages.
	///
	/// Until #1537, <see cref="CardSetLocalization.TranslateCardSetInfo"/> ran StaticConversions
	/// on <c>mustache</c> only; <c>Clone()</c> copies <c>css</c> verbatim, so nothing in the
	/// pipeline ever rewrote it. The fix routes <c>css</c> through the same StaticConversions.
	///
	/// These tests pin three things, in the house style (#1046 — a guard never seen red is a
	/// no-op, so the absent-translation witness is exercised FIRST):
	/// <list type="number">
	/// <item><description>The premiss: the Rules template css on disk really carries the 5 French
	/// footers (if a future template edit removes them, this organ must say so — not pass vacuously).</description></item>
	/// <item><description>The mechanism: StaticConversions DO reach the css now — a red witness on
	/// a synthetic clone (conversions absent → label stays French) and the green form (a labeled
	/// conversion registered → label gone in the converted css), via the production path.</description></item>
	/// <item><description>The arbitration boundary: the config carries NO footer translation yet —
	/// the 5×7 translations have no committed source (measured during #1537: the variant names
	/// exist ONLY in the French css and in French DNN slugs; inventing them here would be an
	/// independent translation, forbidden). This Fact fails loudly the day conversions land without
	/// updating the test, forcing the measurement to be restated.</description></item>
	/// </list>
	/// </summary>
	public class RulesCssStaticConversionTests
	{
		private const string RulesTemplateRelPath = "Cards/Rules/Argumentum_Rules_fr.json";

		private static readonly string[] FrenchFooters =
		{
			"L'ÉCOLE DES MENTEURS",
			"LE BINGO MIXOLOGIE ARGUMENTATIVE",
			"LE DERNIER BEAU PARLEUR",
			"LE MOULIN À BARATIN",
			"LA PARLOTE COINCHÉE",
		};

		private static string LoadRulesCss()
		{
			var path = Path.Combine(TestRepoRoot.Find(), RulesTemplateRelPath);
			File.Exists(path).Should().BeTrue($"Rules template must exist at {RulesTemplateRelPath}");
			using var doc = JsonDocument.Parse(File.ReadAllText(path));
			doc.RootElement.TryGetProperty("css", out var css).Should().BeTrue(
				"the Rules template must carry a css key — parsing the JSON, not grepping the raw bytes " +
				"(the css lives inside a JSON string; a regex on the raw file reads escapes as content, #1485 lesson)");
			return css.GetString()!;
		}

		private static CardSetLocalization GetRulesLocalization()
		{
			var config = new AssetConverterConfig();
			var loc = config.LocalizationConfig.CardSetLocalizations
				.FirstOrDefault(l => l.CardSetNames.Contains(KnownCardSets.Rules));
			loc.Should().NotBeNull("the default LocalizationConfig must carry a Rules mapping");
			return loc!;
		}

		/// <summary>
		/// Builds a <see cref="CardSetInfo"/> whose template resolves to a REAL FILE under the test
		/// temp directory (Debug branch of GetJsonFilePath), seeded with the on-disk Rules template
		/// whose css is spiked with a conversion target — then runs the REAL
		/// <see cref="CardSetLocalization.TranslateCardSetInfo"/> on it. This is the only way to
		/// prove the css is actually routed through StaticConversions: a test calling
		/// DoStaticConversions directly passes on the PRE-#1537 code too (that method worked on
		/// mustache all along — measured: both early drafts of this suite went green with the fix
		/// line deleted). <paramref name="useLocalCardpen"/> drives which URL form
		/// <c>GetDocumentPayload</c> accepts (file:// vs http://).
		/// </summary>
		private static async System.Threading.Tasks.Task<string> TranslateViaProductionPath(
			bool useLocalCardpen, string spikedCss, string sourceTemplatePath, string destLang)
		{
			var tempDir = Path.Combine(Path.GetTempPath(), "ArgumentumTests_1537", Guid.NewGuid().ToString());
			Directory.CreateDirectory(tempDir);
			try
			{
				var templateBytes = File.ReadAllBytes(sourceTemplatePath);
				using (var doc = JsonDocument.Parse(templateBytes))
				{
					// Seed the spiked css into a copy of the real template (mustache untouched —
					// the #1537 surface is css-only). Write without BOM, matching CardPen's files.
					var root = doc.RootElement;
					using var ms = new MemoryStream();
					using (var writer = new Utf8JsonWriter(ms))
					{
						writer.WriteStartObject();
						foreach (var prop in root.EnumerateObject())
						{
							if (prop.NameEquals("css"))
							{
								writer.WriteString("css", spikedCss);
							}
							else
							{
								prop.WriteTo(writer);
							}
						}
						writer.WriteEndObject();
					}
					var spikedPath = Path.Combine(tempDir, "Rules_Template_Spiked.json");
					File.WriteAllBytes(spikedPath, ms.ToArray());

					var config = new AssetConverterConfig();
					config.WebBasedGeneratorConfig.UseLocalCardpen = useLocalCardpen;
					// UseDebugParams is computed from the build mode (isInDebugMode) — in a Debug test
					// run it is already true, so JsonFilePathDebug is the selected path by construction.
					// Asserted (not assumed): under a Release runner GetJsonFilePath would resolve
					// JsonFilePathRelease and this test would silently read nothing.
					config.UseDebugParams.Should().BeTrue(
						"the test suite runs Debug — UseDebugParams must be true so JsonFilePathDebug is selected");
					var loc = new CardSetLocalization
					{
						CardSetNames = new System.Collections.Generic.List<string> { KnownCardSets.Rules },
						StaticConversions = new System.Collections.Generic.List<(string sourceText, System.Collections.Generic.List<(string Language, string destText)> textConversions)>
						{
							("L'ÉCOLE DES MENTEURS — VARIANTE MARQUEE",
								new System.Collections.Generic.List<(string Language, string destText)> { ("en", "THE SCHOOL OF LIARS") }),
						},
					};
					// A plain local path in BOTH cases: PathIsUrl only recognizes http/https, and the
					// non-URL branch of GetDocumentPayload treats the value as a filesystem path
					// (a "file:///..." string would be path-combined and fail to open — measured).
					// The UseLocalCardpen toggle still discriminates the css '../../Cards/' rewrite.
					var source = new CardSetInfo
					{
						DataSet = KnownDataSets.Rules,
						JsonFilePathDebug = spikedPath,
					};

					var payload = await loc.TranslateCardSetInfo(source, front: true, ("fr", destLang), config);
					payload.Should().NotBeNull(
						"TranslateCardSetInfo must load the spiked template from the local file — a null payload means the test setup is broken, not the fix");
					return payload!.CardSetDocument.css;
				}
			}
			finally
			{
				Directory.Delete(tempDir, true);
			}
		}

		[Fact]
		public void Rules_Template_Css_Carries_The_Five_French_Variant_Footers()
		{
			var css = LoadRulesCss();
			foreach (var footer in FrenchFooters)
			{
				css.Should().Contain(footer,
					$"the Rules template css must carry the footer \"{footer}\" — this organ's premiss. " +
					"If a template edit removes it, this test must be restated, not silently green");
			}
		}

		[Fact]
		public void Witness_Without_Footer_Conversions_The_Css_Stays_French_In_All_Non_Fr_Languages()
		{
			// Red witness: the CURRENT config (no footer translations registered) leaves all 5
			// French footers in the css of every non-FR clone. This is the defect #1537 names —
			// asserted as present so the day it turns red, the translations have landed and this
			// witness must flip to its green form (asserting the translations instead).
			var css = LoadRulesCss();
			var loc = GetRulesLocalization();
			foreach (var lang in new[] { "en", "ru", "pt", "es", "ar", "fa", "zh" })
			{
				var converted = loc.ApplyCssStaticConversions(css, lang);
				foreach (var footer in FrenchFooters)
				{
					converted.Should().Contain(footer,
						$"with no footer StaticConversions registered, the {lang} css must still carry the French footer " +
						$"\"{footer}\" — if this turns red, the translations landed: flip this witness to assert them");
				}
			}
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async System.Threading.Tasks.Task Static_Conversions_Do_Reach_The_Css_Key_Through_The_Production_Path(bool useLocalCardpen)
		{
			// Green mechanism proof on the REAL production path: TranslateCardSetInfo itself, run
			// against a spiked template file — NOT DoStaticConversions called directly (that method
			// worked on mustache BEFORE #1537; calling it from a test passes on the pre-fix code —
			// measured twice in this very session, both early drafts green with the fix deleted).
			// The localization is a DEDICATED instance (not the config's shared one): xUnit runs
			// test classes in parallel and every AssetConverterConfig() hands out the same Rules
			// localization object — registering the spike on it would leak into a concurrent test.
			var templatePath = Path.Combine(TestRepoRoot.Find(), RulesTemplateRelPath);
			var css = LoadRulesCss();
			var spiked = css.Replace("L'ÉCOLE DES MENTEURS", "L'ÉCOLE DES MENTEURS — VARIANTE MARQUEE");
			spiked.Should().NotBe(css, "the spike must actually alter the css — otherwise this test is vacuous");

			var convertedCss = await TranslateViaProductionPath(useLocalCardpen, spiked, templatePath, "en");


			convertedCss.Should().Contain("THE SCHOOL OF LIARS",
				"the StaticConversion registered on the Rules localization must reach the CLONED css — " +
				"pre-#1537, TranslateCardSetInfo left returnDoc.css untouched and this was impossible");
			convertedCss.Should().NotContain("L'ÉCOLE DES MENTEURS — VARIANTE MARQUEE",
				"the French (spiked) footer must be gone from the converted css");
			// Anti-clobber: the other 4 footers (no conversion registered) must survive verbatim.
			foreach (var footer in FrenchFooters.Skip(1))
			{
				convertedCss.Should().Contain(footer,
					$"footer \"{footer}\" has no conversion registered — it must survive verbatim (no accidental clobber)");
			}
		}

		[Fact]
		public void Config_Carries_No_Footer_Translation_Yet_Arbitration_Pending()
		{
			// The #1537 ASK (pool #458, 2026-09-25) is pending: the 5×7 footer translations have no
			// committed source (the variant names exist ONLY in the French css and in French DNN
			// slugs/site titles — the issue's premise that they live in Text_<lang> of the opening
			// card is measured false; those cells are rule bodies). This Fact pins the premiss:
			// the Rules StaticConversions must NOT reference any of the 5 French footers. When the
			// arbitration lands and the conversions are added, this Fact goes red BY DESIGN — the
			// red is the signal to flip it to its positive form (each footer HAS its 7 translations).
			var loc = GetRulesLocalization();
			foreach (var footer in FrenchFooters)
			{
				loc.StaticConversions.Should().NotContain(
					c => string.Equals(c.sourceText, footer, StringComparison.Ordinal),
					$"no footer translation is committed yet (arbitration pending) — \"{footer}\" must be absent from " +
					"the Rules StaticConversions; when the translations land, flip this Fact to assert their presence");
			}
		}
	}
}
