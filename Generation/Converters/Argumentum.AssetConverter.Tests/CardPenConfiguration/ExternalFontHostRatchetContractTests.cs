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
	/// Ratchet: no card template beyond a frozen list of ten may load a font from
	/// <c>fonts.cdnfonts.com</c> or <c>db.onlinewebfonts.com</c>.
	///
	/// <para><b>Why this exists.</b> A card face that depends on a third-party font CDN is a
	/// print reproducibility hazard with no failure signal: the day the CDN serves the family
	/// differently, throttles it, or disappears, the harvest still succeeds and the PNG that
	/// reaches the printer simply carries different glyphs — the same silent-defect family as
	/// the mixed-content font substitution of #1180. The ten templates that load from these
	/// hosts today are grandfathered; the list only ever shrinks.</para>
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
	/// under <c>Archive/</c>.</para>
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
		/// The ten templates grandfathered into loading from the external font hosts, frozen
		/// 2026-11-22 (measured: the parsed <c>css</c> key of every living template, see class
		/// remarks). This list is a ratchet, not an allowance to grow: a new template that needs
		/// one of these families must vendor the font under <c>Cards/</c> (or self-host it)
		/// instead of joining the list. An entry leaves it when its template stops loading
		/// externally or moves to <c>Archive/</c>.
		/// </summary>
		private static readonly string[] FrozenExternalFontTemplates =
		{
			"Cards/Fallacies/Argumentum_Fallacies_Back_fr.json",
			"Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json",
			"Cards/Fallacies/Argumentum_Fallacies_Face_fr.json",
			"Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json",
			"Cards/Fallacies/Argumentum_Virtues_Face_fr.json",
			"Cards/Memo/Argumentum_Memo_Back_fr.json",
			"Cards/Rules/Argumentum_Rules_Francais_edition_fevrier_2022_Print_and_Play.json",
			"Cards/Rules/Argumentum_Rules_fr.json",
			"Cards/Scenarii/Argumentum_Scenarii_Back_fr.json",
			"Cards/Scenarii/Argumentum_Scenarii_Face_fr.json",
		};

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
				"ten of the living templates carry external font loads in their css key, so at " +
				"least ten must carry a css string. Fewer means the css key was renamed or the " +
				"parse started skipping files — the load-position scan would then read an " +
				"absence that is not one (#1485).");
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
			frozen.Should().HaveCount(10,
				"ten templates were grandfathered on 2026-11-22. The number changes only when a " +
				"template stops loading externally (shrink, in its own PR) — never to admit a " +
				"new one.");

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
				"a template beyond the frozen list of ten loads a font from fonts.cdnfonts.com " +
				"or db.onlinewebfonts.com. This is a ratchet, not an allowance to grow: vendor " +
				"the font under Cards/ (or self-host it) in the template's own PR. New " +
				"offender(s): {0}",
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
