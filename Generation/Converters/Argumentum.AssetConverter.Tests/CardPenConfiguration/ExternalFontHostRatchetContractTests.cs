using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.CardPenConfiguration
{
	/// <summary>
	/// Ratchet: no card template beyond a frozen list may load a font from
	/// <c>fonts.cdnfonts.com</c> or <c>db.onlinewebfonts.com</c>. The list held ten templates
	/// from 2026-11-22 and is <b>empty since 2026-11-26</b>.
	///
	/// <para><b>Why this exists.</b> A card face that depends on a third-party font CDN is a
	/// print reproducibility hazard with no failure signal: the day the CDN serves the family
	/// differently, throttles it, or disappears, the harvest still succeeds and the PNG that
	/// reaches the printer simply carries different glyphs — the same silent-defect family as
	/// the mixed-content font substitution of #1180. The ten templates that loaded from these
	/// hosts were grandfathered when the ratchet was written; the list only ever shrinks.</para>
	///
	/// <para><b>The CSS is read parsed, never as raw bytes</b> (#1485). A template's CSS lives
	/// inside a JSON string, so every quote arrives escaped; an instrument that regexes the raw
	/// file can read a full declaration as an empty capture. Here each file is parsed with
	/// <see cref="JsonDocument"/> and only the unescaped value of the <c>css</c> key is
	/// matched, in load position (<c>url(…)</c> or <c>@import "…"</c>) — a domain mentioned in
	/// a comment or a bibliographic link is not a load.</para>
	///
	/// <para><b>Scope is the living tree, not the config declaration.</b> Every
	/// <c>Cards/**/*.json</c> outside <c>Archive/</c> is scanned, wired or not. Measured
	/// 2026-11-22: 12 living templates, of which 11 are declared in
	/// <c>WebBasedGeneratorConfig</c>; the twelfth (<c>Rules … 2022 Print_and_Play</c>) is an
	/// unwired leftover that still carries both hosts — including plain-<c>http://</c>
	/// onlinewebfonts URLs that the config-scoped <see cref="InsecureSubResourceContractTests"/>
	/// never sees because nothing declares the file. A template leaves the scope only by moving
	/// under <c>Archive/</c>. Since 2026-11-26 the allowance is <b>empty</b>: the ten
	/// grandfathered templates moved to the vendored families (<c>Cards/fonts/</c>,
	/// <see cref="Argumentum.AssetConverter.EmbeddedFontFaces"/>), and
	/// <see cref="NoRemoteFontLoad_OutsideTheFrozenStylesheets"/> holds the wider invariant —
	/// no remote font load of any host, in any key of the template. Both tests stay because the
	/// failure modes differ: this one names the two hosts a reviewer knows, that one a host or
	/// a key nobody has seen yet.</para>
	///
	/// <para><b>What emptying the allowance did NOT remove, and what pins it.</b> No template
	/// declares a remote <c>@font-face</c> any more, but all twelve living templates still
	/// load a stylesheet from <c>fonts.googleapis.com</c> through their <c>extCSS</c> key —
	/// CardPen turns it into <c>&lt;link rel="stylesheet" crossorigin="anonymous"&gt;</c> in
	/// the output document (<c>Generation/CardPen/js/main.js</c>, "Summon the goog"). Measured
	/// 2026-11-26: five distinct URLs over twelve templates, carrying <c>Bebas Neue</c> (ten
	/// templates), <c>PT Sans</c> 700 (nine), the three ar/fa/zh glyph families
	/// <c>Noto Naskh Arabic</c> / <c>Noto Sans SC</c> / <c>Vazirmatn</c> (eleven), and six
	/// one-offs (<c>Poppins</c> and <c>Raleway</c> on the Fallacies-3 document; <c>Oswald</c>,
	/// <c>PT Sans Narrow</c>, <c>Dosis</c>, <c>Noto Sans JP</c> on the Memo back). Those loads
	/// are load-bearing — the glyph families are exactly what the <c>.argu-lang-{ar,fa,zh} *</c>
	/// rules name, and <see cref="LanguageSpecificCssContractTests"/> requires them to be
	/// loaded — and #1485's substitution does not embed them.
	/// <see cref="ExternalStylesheets_MatchTheFrozenList"/> makes the remainder a reviewed
	/// list instead of a blind spot, and
	/// <see cref="NoRemoteFontLoad_OutsideTheFrozenStylesheets"/> makes the same statement for
	/// a load that would hide in <c>mustache</c> or in a future key.</para>
	/// </summary>
	public class ExternalFontHostRatchetContractTests
	{
		/// <summary>
		/// Matches one of the two external font hosts, only where the browser would actually
		/// fetch it: inside <c>url(…)</c>, or as the string argument of <c>@import</c>. The
		/// scheme is optional so that a protocol-relative or plain-<c>http</c> load cannot slip
		/// under the bar. Trailing characters are captured so failure messages name the file
		/// being loaded, not just the host.
		/// </summary>
		private static readonly Regex ExternalFontLoad = new Regex(
			@"(?:(?:url\(\s*['""]?)|@import\s+['""])\s*(?:https?:)?//(?:fonts\.cdnfonts\.com|db\.onlinewebfonts\.com)[^\s'"")]*",
			RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// The templates grandfathered into loading from the external font hosts. Frozen on
		/// 2026-11-22 at ten, <b>emptied on 2026-11-26</b> by the substitution that moved every
		/// card family to the vendored files under <c>Cards/fonts/</c> (#1485): the four DINPro
		/// weights left all ten templates, and the last two third-party declarations — Alfa Slab
		/// One's (Rules, live) and TrendSlab's (2022 Print&amp;Play, unwired) — were replaced or
		/// removed as well. The list is a ratchet, not an allowance to grow: a new template that
		/// needs a family must vendor the font under <c>Cards/</c> and declare it in
		/// <see cref="Argumentum.AssetConverter.EmbeddedFontFaces"/> instead of joining this list,
		/// which only ever shrinks.
		/// </summary>
		private static readonly string[] FrozenExternalFontTemplates = Array.Empty<string>();

		/// <summary>
		/// Every living template on disk: any JSON under <c>Cards/</c> that has not been
		/// archived. Repo-relative, forward slashes, ordinal-sorted so failures read stably.
		/// </summary>
		private static List<string> LivingTemplates()
		{
			var repoRoot = TestRepoRoot.Find();
			var cardsRoot = Path.Combine(repoRoot, "Cards");

			return Directory.EnumerateFiles(cardsRoot, "*.json", SearchOption.AllDirectories)
				.Select(p => Path.GetRelativePath(repoRoot, p).Replace('\\', '/'))
				.Where(rel => !rel.Split('/').Contains("Archive", StringComparer.OrdinalIgnoreCase))
				.OrderBy(rel => rel, StringComparer.Ordinal)
				.ToList();
		}

		/// <summary>
		/// The templates whose parsed <c>css</c> key loads from one of the two hosts, with the
		/// matching excerpts for the failure messages. Files with no string <c>css</c> property
		/// are not card templates carrying CSS and are skipped.
		/// </summary>
		private static Dictionary<string, List<string>> TemplatesLoadingFromExternalHosts()
		{
			var repoRoot = TestRepoRoot.Find();
			var offenders = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

			foreach (var rel in LivingTemplates())
			{
				using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(repoRoot, rel)));
				if (!doc.RootElement.TryGetProperty("css", out var cssElement)
					|| cssElement.ValueKind != JsonValueKind.String)
				{
					continue;
				}

				var css = cssElement.GetString()!;
				var hits = ExternalFontLoad.Matches(css)
					.Select(m => m.Value)
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToList();

				if (hits.Count > 0)
				{
					offenders[rel] = hits;
				}
			}

			return offenders;
		}

		/// <summary>
		/// Anti-noop. Both scope assertions guard the guard: an enumeration that silently
		/// yields nothing reports green forever. The floors are the 2026-11-22 measurement —
		/// 12 living templates, all 12 carrying a string <c>css</c> key. A drop below either
		/// floor means the derivation broke (Cards moved, key renamed, Archive filter gone
		/// wrong) rather than that templates were deleted — fix the derivation before touching
		/// the numbers.
		/// </summary>
		[Fact]
		public void LivingTemplateScope_IsNonEmptyAndCarriesCss()
		{
			var living = LivingTemplates();

			living.Count.Should().BeGreaterThanOrEqualTo(12,
				"the ratchet scans every Cards/**/*.json outside Archive/. Twelve were measured " +
				"on 2026-11-22; an empty or shrunk scope means the enumeration broke and the " +
				"ratchet below has silently become a no-op certifying zero files.");

			var repoRoot = TestRepoRoot.Find();
			var cssCarriers = living
				.Count(rel =>
				{
					using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(repoRoot, rel)));
					return doc.RootElement.TryGetProperty("css", out var css)
						&& css.ValueKind == JsonValueKind.String;
				});

			cssCarriers.Should().BeGreaterThanOrEqualTo(10,
				"ten of the living templates declared the card typefaces in their css key before " +
				"the #1485 substitution (twelve carry a css string in total), so at least ten must " +
				"still carry one. Fewer means the css key was renamed or the parse started skipping " +
				"files — the load-position scan would then read an absence that is not one (#1485).");
		}

		/// <summary>
		/// A <c>@font-face</c> whose <c>src</c> is a remote URL — of <b>any</b> host, not only
		/// the font hosts of <see cref="FontHostUrl"/>. Trailing characters are captured so a
		/// failure message names the file being loaded.
		/// </summary>
		private static readonly Regex RemoteFontFace = new Regex(
			@"@font-face\s*\{[^}]*?url\(\s*['""]?(?:https?:)?//[^)]*",
			RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

		/// <summary>
		/// A URL on a host whose business is serving font files: the hosts these templates have
		/// used (<c>fonts.googleapis.com</c>, <c>fonts.cdnfonts.com</c>,
		/// <c>db.onlinewebfonts.com</c>) and their obvious neighbours. A deny-list, therefore
		/// never complete — a family hotlinked from anywhere else is still caught by
		/// <see cref="RemoteFontFace"/> when it goes through a <c>@font-face</c>.
		/// </summary>
		private static readonly Regex FontHostUrl = new Regex(
			@"(?:https?:)?//(?:[a-z0-9-]+\.)*(?:googleapis\.com|gstatic\.com|bunny\.net|typekit\.net|cdnfonts\.com|onlinewebfonts\.com)(?:/[^\s'""<>)\\]*)?",
			RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// The external stylesheets the living templates still load: the value of each one's
		/// <c>extCSS</c> key, verbatim. Frozen on 2026-11-26 at twelve templates and five
		/// distinct URLs (see the class remarks). It is the same ratchet as
		/// <see cref="FrozenExternalFontTemplates"/> — an entry leaves it only when the template
		/// stops loading fonts remotely, by vendoring the families under <c>Cards/fonts/</c>
		/// and declaring them in <see cref="Argumentum.AssetConverter.EmbeddedFontFaces"/>, and
		/// never to admit a family or a weight.
		/// </summary>
		private static readonly Dictionary<string, string> FrozenExternalStylesheets =
			new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			{
				["Cards/Fallacies/Argumentum_Fallacies_Back_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
				["Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
				["Cards/Fallacies/Argumentum_Fallacies_Face_3_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&family=Raleway:ital,wght@0,100;0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,100;1,200;1,300;1,400;1,500;1,600;1,700;1,800;1,900&family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
				["Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
				["Cards/Fallacies/Argumentum_Fallacies_Face_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
				["Cards/Fallacies/Argumentum_Virtues_Face_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
				["Cards/Memo/Argumentum_Memo_Back_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Bebas+Neue&family=Oswald:wght@400;500&family=PT+Sans+Narrow:wght@400;700&family=Dosis:wght@200;300;400;500;523;600;700;800&family=Noto+Sans+JP:wght@100;300;400;500;700;900&family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
				["Cards/Memo/Argumentum_Memo_Face_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
				["Cards/Rules/Argumentum_Rules_Francais_edition_fevrier_2022_Print_and_Play.json"] =
					"https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&display=swap",
				["Cards/Rules/Argumentum_Rules_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
				["Cards/Scenarii/Argumentum_Scenarii_Back_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
				["Cards/Scenarii/Argumentum_Scenarii_Face_fr.json"] =
					"https://fonts.googleapis.com/css2?family=Bebas+Neue&family=PT+Sans:wght@700&family=Noto+Naskh+Arabic:wght@400;700&family=Noto+Sans+SC:wght@400;700&family=Vazirmatn:wght@400;700&display=swap",
			};

		/// <summary>Every string in a parsed template, with the top-level key it sits under.</summary>
		private static IEnumerable<(string Key, string Value)> StringsByTopLevelKey(JsonElement root)
		{
			foreach (var property in root.EnumerateObject())
			{
				foreach (var value in Strings(property.Value))
				{
					yield return (property.Name, value);
				}
			}
		}

		private static IEnumerable<string> Strings(JsonElement element)
		{
			switch (element.ValueKind)
			{
				case JsonValueKind.String:
					yield return element.GetString()!;
					break;

				case JsonValueKind.Object:
					foreach (var property in element.EnumerateObject())
					{
						foreach (var value in Strings(property.Value))
						{
							yield return value;
						}
					}

					break;

				case JsonValueKind.Array:
					foreach (var item in element.EnumerateArray())
					{
						foreach (var value in Strings(item))
						{
							yield return value;
						}
					}

					break;
			}
		}

		/// <summary>The remote stylesheet a living template loads, or none: its non-blank <c>extCSS</c>.</summary>
		private static Dictionary<string, string> ExternalStylesheets()
		{
			var repoRoot = TestRepoRoot.Find();
			var actual = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			foreach (var rel in LivingTemplates())
			{
				using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(repoRoot, rel)));
				if (doc.RootElement.ValueKind == JsonValueKind.Object
					&& doc.RootElement.TryGetProperty("extCSS", out var ext)
					&& ext.ValueKind == JsonValueKind.String
					&& !string.IsNullOrWhiteSpace(ext.GetString()))
				{
					actual[rel] = ext.GetString()!;
				}
			}

			return actual;
		}

		/// <summary>
		/// The invariant the ratchet was a stepping stone to (#1485, 3.b): <b>a living template
		/// references a remote font only through its frozen <c>extCSS</c> stylesheet.</b>
		/// Three ways to load a font remotely are read, in every string of the template and
		/// not only its <c>css</c> key — a <c>@font-face</c> with a remote <c>src</c> (any
		/// host), and, outside <c>extCSS</c>, any URL on a font host (an <c>@import</c>, a
		/// <c>&lt;link&gt;</c> in the mustache, a bare <c>url(…)</c>). The <c>css</c>-key-only
		/// instrument this replaces read "no remote font face" on a tree where twelve templates
		/// out of twelve loaded fonts remotely: a zero is only an absence if the instrument
		/// could have seen a one.
		/// </summary>
		[Fact]
		public void NoRemoteFontLoad_OutsideTheFrozenStylesheets()
		{
			var repoRoot = TestRepoRoot.Find();
			var offenders = new List<string>();

			foreach (var rel in LivingTemplates())
			{
				using var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(repoRoot, rel)));

				foreach (var (key, value) in StringsByTopLevelKey(doc.RootElement))
				{
					foreach (Match hit in RemoteFontFace.Matches(value))
					{
						offenders.Add($"{rel} [{key}] @font-face → {Regex.Match(hit.Value, @"(?:https?:)?//[^'"")\s]+").Value}");
					}

					// extCSS is the allowance itself, checked entry by entry by the frozen list.
					if (string.Equals(key, "extCSS", StringComparison.Ordinal))
					{
						continue;
					}

					foreach (Match hit in FontHostUrl.Matches(value))
					{
						offenders.Add($"{rel} [{key}] → {hit.Value}");
					}
				}
			}

			offenders.Should().BeEmpty(
				"a card template loads a font from a remote host outside its extCSS stylesheet. A " +
				"harvest that loses that host still succeeds and prints different glyphs — vendor " +
				"the file under Cards/fonts/ and declare it in EmbeddedFontFaces. Offender(s): {0}",
				string.Join(" | ", offenders));
		}

		/// <summary>
		/// The <c>extCSS</c> remainder, pinned: the templates that load an external stylesheet,
		/// and the exact stylesheet each one loads, equal the frozen list. Both directions — a
		/// family or a weight added to a URL, a URL moved to another template, a new template
		/// with one, and an entry left behind when a template stops loading remotely are all
		/// a reviewed change or a red test.
		/// </summary>
		[Fact]
		public void ExternalStylesheets_MatchTheFrozenList()
		{
			var frozen = FrozenExternalStylesheets;
			frozen.Should().HaveCount(12,
				"twelve living templates each loaded one Google Fonts stylesheet when the list was " +
				"frozen on 2026-11-26. The number changes only when a template stops loading fonts " +
				"remotely (shrink, in its own PR) — never to admit a family or a weight.");

			var actual = ExternalStylesheets();

			var changed = actual
				.Where(kv => !frozen.TryGetValue(kv.Key, out var url)
					|| !string.Equals(url, kv.Value, StringComparison.Ordinal))
				.OrderBy(kv => kv.Key, StringComparer.Ordinal)
				.Select(kv => $"{kv.Key} → {kv.Value}")
				.ToList();

			changed.Should().BeEmpty(
				"a template loads an external stylesheet that is not the frozen one — a family or " +
				"a weight was added to its URL, or a template that had none now has one. These " +
				"loads are a ratchet: vendor the family under Cards/fonts/ and declare it in " +
				"EmbeddedFontFaces instead. Changed or new: {0}",
				string.Join(" | ", changed));

			var stale = frozen
				.Where(kv => !actual.ContainsKey(kv.Key))
				.OrderBy(kv => kv.Key, StringComparer.Ordinal)
				.Select(kv => kv.Key)
				.ToList();

			stale.Should().BeEmpty(
				"these templates are in the frozen list but no longer load an external stylesheet — " +
				"remove them from the list in this same change so the ratchet stays tight: {0}",
				string.Join(" | ", stale));
		}

		/// <summary>
		/// The ratchet itself, in both directions: no template beyond the frozen list loads
		/// externally, and every frozen entry still does. One-sided equality would let the
		/// allowance rot in either direction — a stale entry widens it invisibly, an
		/// unreviewed new entry would be the widening itself.
		/// </summary>
		[Fact]
		public void ExternalFontHostUsage_MatchesTheFrozenRatchet()
		{
			var frozen = FrozenExternalFontTemplates;
			frozen.Should().HaveCount(0,
				"the allowance was frozen at ten on 2026-11-22 and emptied on 2026-11-26 by the #1485 " +
				"substitution. The number changes only when a template stops loading externally " +
				"(shrink, in its own PR) — never to admit a new one: a new template vendors its " +
				"font under Cards/fonts/ and declares it in EmbeddedFontFaces.");

			var repoRoot = TestRepoRoot.Find();
			foreach (var path in frozen)
			{
				File.Exists(Path.Combine(repoRoot, path)).Should().BeTrue(
					"{0} is listed in the frozen allowance but is not on disk. A typo'd entry " +
					"never matches anything and silently loosens the ratchet — it would also " +
					"fail the stale direction below, but the diagnosis should name the cause.",
					path);
			}

			var offenders = TemplatesLoadingFromExternalHosts();
			var frozenSet = new HashSet<string>(frozen, StringComparer.OrdinalIgnoreCase);

			var unexpected = offenders.Keys
				.Where(k => !frozenSet.Contains(k))
				.OrderBy(k => k, StringComparer.Ordinal)
				.ToList();

			unexpected.Should().BeEmpty(
				"a template loads a font from fonts.cdnfonts.com or db.onlinewebfonts.com and no " +
				"template is grandfathered any more. This is a ratchet, not an allowance to grow: " +
				"vendor the font under Cards/fonts/ and declare it in EmbeddedFontFaces, in the " +
				"template's own PR. New offender(s): {0}",
				string.Join(" | ", unexpected.Select(u => $"{u} → {string.Join(", ", offenders[u])}")));

			var stale = frozen
				.Where(f => !offenders.ContainsKey(f))
				.OrderBy(f => f, StringComparer.Ordinal)
				.ToList();

			stale.Should().BeEmpty(
				"these templates are listed in the frozen allowance but no longer load from the " +
				"external hosts — remove them from the list in this same change so the ratchet " +
				"stays tight: {0}",
				string.Join(" | ", stale));
		}
	}
}
