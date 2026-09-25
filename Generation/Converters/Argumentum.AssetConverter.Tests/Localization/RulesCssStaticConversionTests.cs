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
	///
	/// ⚠️ <b>Restatement, #1537 p.2 (25/09)</b> : l'arbitrage a tranché au-delà de cette prémisse —
	/// les intitulés vivent bien dans le CSV Rules, dans les cellules de couverture
	/// (<c>Text_&lt;lang&gt;</c> de Rules_01/07/09/11/13, ligne <c>## …</c>), et le dispatch a nommé
	/// cette source. Les 35 conversions sont désormais REGISTRÉES, dérivées
	/// (<c>Config_Footer_Translations_Are_Derived_From_The_Rules_Csv_Cover_Rows</c>) ; l'ancien
	/// témoin « reste français » a été retourné en sa forme verte
	/// (<c>Footer_Conversions_Land_In_All_Non_Fr_Languages_As_Exact_Replacements</c>), avec témoin
	/// rouge conservé sur une localisation nue.
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
					// UseDebugParams is computed from the BUILD mode (#if DEBUG): Debug locally, but
					// the CI Release leg runs this same test — asserting Debug there is red by
					// construction (measured: run 36087727689, both InlineData failed on it).
					// Setting BOTH template paths to the spiked file makes the test
					// mode-independent: whichever path GetJsonFilePath selects, it lands here.
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
						JsonFilePathRelease = spikedPath,
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
		public void Footer_Conversions_Land_In_All_Non_Fr_Languages_As_Exact_Replacements()
		{
			// Vert de #1537 p.2 : les 35 traductions vivent dans la config — le css cloné de chaque
			// langue non-fr ne porte PLUS aucun des 5 pieds français, et porte l'intitulé dérivé du CSV.
			// Contrôle par DELTA : le converti est EXACTEMENT le template avec les 5 remplacements —
			// rien d'autre n'a bougé (une conversion trop large serait visible ici).
			var css = LoadRulesCss();
			var loc = GetRulesLocalization();
			var langs = new[] { "en", "ru", "pt", "es", "ar", "fa", "zh" };
			foreach (var lang in langs)
			{
				var converted = loc.ApplyCssStaticConversions(css, lang);
				var expected = css;
				foreach (var footer in FrenchFooters)
				{
					var dest = loc.StaticConversions
						.Single(c => string.Equals(c.sourceText, footer, StringComparison.Ordinal))
						.textConversions.Single(t => t.Language == lang).destText;
					expected = expected.Replace(footer, dest);
				}
				converted.Should().Be(expected,
					$"the {lang} clone must be the template with EXACTLY the 5 footer replacements and nothing else");
				foreach (var footer in FrenchFooters)
				{
					converted.Should().NotContain(footer,
						$"the {lang} css must no longer carry the French footer \"{footer}\"");
				}
			}

			// fr = no-op : aucune entrée fr pour les pieds, donc le css source est inchangé à l'octet.
			loc.ApplyCssStaticConversions(css, "fr").Should().Be(css,
				"fr is the source language — the footer conversions carry no fr entry, so the css must be untouched");

			// Témoin rouge : une localisation SANS conversions de pieds laisse le css français dans
			// toutes les langues — c'est le défaut d'avant, asserté pour prouver que le vert ci-dessus
			// vient bien de la config et non d'autre chose.
			var bare = new CardSetLocalization
			{
				CardSetNames = new System.Collections.Generic.List<string> { KnownCardSets.Rules },
				StaticConversions = new System.Collections.Generic.List<(string sourceText, System.Collections.Generic.List<(string Language, string destText)> textConversions)>(),
			};
			foreach (var lang in langs)
			{
				var converted = bare.ApplyCssStaticConversions(css, lang);
				foreach (var footer in FrenchFooters)
				{
					converted.Should().Contain(footer,
						$"without footer conversions the {lang} css stays French — this is the pre-#1537-p.2 defect, " +
						"asserted so the green form above is not vacuous");
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

		/// <summary>
		/// Les pieds de page → la ligne de couverture du CSV Rules dont l'intitulé <c>## …</c> les porte.
		/// Source de dérivation nommée par le dispatch #1537 p.2 (ai-01, 25/09).
		/// </summary>
		private static readonly (string Footer, string Pk)[] FooterCoverRows =
		{
			("L'ÉCOLE DES MENTEURS", "Rules_01"),
			("LE BINGO MIXOLOGIE ARGUMENTATIVE", "Rules_07"),
			("LE DERNIER BEAU PARLEUR", "Rules_09"),
			("LE MOULIN À BARATIN", "Rules_11"),
			("LA PARLOTE COINCHÉE", "Rules_13"),
		};

		private static readonly string[] NonFrLanguages = { "en", "ru", "pt", "es", "ar", "fa", "zh" };

		/// <summary>Charge le CSV Rules par en-tête (pk → ligne), jamais par index positionnel.</summary>
		private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> LoadRulesCsvRows()
		{
			var path = Path.Combine(TestRepoRoot.Find(), "Cards", "Rules", "Argumentum Rules - Cards.csv");
			File.Exists(path).Should().BeTrue($"Rules CSV must exist at {path}");
			var cfg = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};
			using var reader = new StreamReader(path);
			using var csv = new CsvHelper.CsvReader(reader, cfg);
			csv.Read();
			csv.ReadHeader();
			var rows = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>();
			while (csv.Read())
			{
				var row = new System.Collections.Generic.Dictionary<string, string>(StringComparer.Ordinal);
				foreach (var h in csv.HeaderRecord!)
				{
					row[h] = csv.GetField(h) ?? string.Empty;
				}
				rows[row["pk"]] = row;
			}
			return rows;
		}

		/// <summary>L'intitulé de couverture d'une ligne : la première ligne <c>## …</c> de sa cellule Text_&lt;lang&gt;.</summary>
		private static string CoverHeading(string cell)
		{
			foreach (var rawLine in cell.Replace("\r\n", "\n").Split('\n'))
			{
				var line = rawLine.Trim();
				if (line.StartsWith("## ", StringComparison.Ordinal))
				{
					return line.Substring(2).Trim();
				}
			}
			return string.Empty;
		}

		[Fact]
		public void Config_Footer_Translations_Are_Derived_From_The_Rules_Csv_Cover_Rows()
		{
			// #1537 p.2 : les 35 traductions sont DÉRIVÉES des lignes de couverture du CSV Rules
			// (l'intitulé `## <Name>` de Text_<lang>, pk Rules_01/07/09/11/13) — aucune valeur
			// inventée. Ce Fact re-dérive du CSV à chaque exécution et compare : si le CSV ou la
			// config dérive, le rouge NOMME le couple (pied, langue).
			var loc = GetRulesLocalization();
			var mapped = loc.StaticConversions
				.Where(c => FooterCoverRows.Any(m => string.Equals(m.Footer, c.sourceText, StringComparison.Ordinal)))
				.ToList();
			mapped.Should().HaveCount(5, "exactly the 5 variant footers must be registered — no more, no less");

			var rows = LoadRulesCsvRows();
			foreach (var (footer, pk) in FooterCoverRows)
			{
				var conv = loc.StaticConversions.Single(c => string.Equals(c.sourceText, footer, StringComparison.Ordinal));
				conv.textConversions.Select(t => t.Language).Should().BeEquivalentTo(NonFrLanguages,
					$"\"{footer}\" carries exactly the 7 non-fr languages");
				foreach (var lang in NonFrLanguages)
				{
					var dest = conv.textConversions.Single(t => t.Language == lang).destText;
					var col = lang == "fr" ? "Text" : $"Text_{lang}";
					var fromCsv = CoverHeading(rows[pk][col]);
					fromCsv.Should().NotBeEmpty($"{pk} must carry a ## heading in {col} — this organ's premiss");
					dest.Should().Be(fromCsv,
						$"the {lang} footer of \"{footer}\" must be the CSV cover-row heading VERBATIM (derived from {pk}.{col}, never invented)");
				}
			}
		}
	}
}
