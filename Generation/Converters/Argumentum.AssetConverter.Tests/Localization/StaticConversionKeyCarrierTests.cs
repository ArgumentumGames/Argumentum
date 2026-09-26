using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
	/// <summary>
	/// Garde générique « clé de conversion ↔ gabarit servi » (#458 pool v18 grain 5, leçon #1571).
	/// Une clé <c>StaticConversions</c> vit dans <c>AssetConverterConfig</c> (compilée) tandis que
	/// le texte qu'elle remplace vit dans le gabarit JSON servi : si l'un bouge sans l'autre, la
	/// conversion devient un no-op silencieux et les 7 langues non-FR retombent sur le français.
	/// Les gardes existantes ne couvraient que deux surfaces épinglees (sous-titre Memo via
	/// <see cref="FallaciesLocalizationTests"/>, pieds de page Rules via
	/// <see cref="RulesCssStaticConversionTests"/>) ; celle-ci couvre TOUTES les clés de TOUS les
	/// groupes, en derivant l'ensemble des gabarits servis de la vraie config (jamais codes en dur).
	///
	/// Regle : pour chaque groupe de <c>CardSetLocalizations</c>, chaque <c>sourceText</c> doit
	/// apparaitre dans AU MOINS UN des gabarits fr servis par les CardSets du groupe (face + dos).
	/// « Au moins un » et non « tous » : la conversion est documentee comme no-op legitime sur les
	/// gabarits du groupe qui ne portent pas le texte (ex. le sous-titre Memo est absent des
	/// gabarits Fallacies). La surface scannee est les touches VIVANTES du gabarit —
	/// <c>mustache</c> et <c>css</c> — jamais la cle <c>csv</c> embarquee, instrument mort
	/// remplace a l'injection runtime (#1390, #1579).
	/// </summary>
	public class StaticConversionKeyCarrierTests
	{
		private static string RepoRoot => TestRepoRoot.Find();

		/// <summary>
		/// Resolves a config-relative Debug path (<c>..\..\..\..\..\..\Cards\…</c>, relative to the
		/// converter's bin output) to a repo-root-relative path. The walk-up prefix is stripped by
		/// locating the <c>Cards\</c> anchor rather than counting segments, so the guard survives a
		/// change of output depth (same rationale as <c>TestRepoRoot</c>, issue noted by ai-01
		/// cycle 64 on MindMapSvgEncodingTests).
		/// </summary>
		private static string ResolveDebugPath(string configPath)
		{
			if (string.IsNullOrWhiteSpace(configPath))
				return string.Empty;
			var normalized = configPath.Replace('/', '\\');
			var idx = normalized.IndexOf("Cards\\", StringComparison.Ordinal);
			return idx < 0
				? string.Empty
				: Path.Combine(RepoRoot, normalized.Substring(idx));
		}

		/// <summary>The live rendering surface of a template: its mustache and css keys, parsed.</summary>
		private static string ReadLiveSurface(string absolutePath)
		{
			using var doc = JsonDocument.Parse(File.ReadAllText(absolutePath));
			var parts = new List<string>();
			foreach (var key in new[] { "mustache", "css" })
			{
				if (doc.RootElement.TryGetProperty(key, out var value)
					&& value.ValueKind == JsonValueKind.String)
				{
					parts.Add(value.GetString() ?? string.Empty);
				}
			}
			return string.Join('\n', parts);
		}

		[Fact]
		public void Every_StaticConversion_Key_Has_A_Carrier_Template_In_Its_Group()
		{
			var config = new AssetConverterConfig();
			var cardSets = config.WebBasedGeneratorConfig.CardSets;

			var failures = new List<string>();
			var measuredKeys = 0;
			var measuredTemplates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			foreach (var group in config.LocalizationConfig.CardSetLocalizations)
			{
				// Gabarits fr servis par les CardSets du groupe (face + dos), derives de la config.
				var served = cardSets
					.Where(c => group.CardSetNames.Contains(c.Name))
					.SelectMany(c => new[] { c.FaceCardSetInfo, c.BackCardSetInfo }
						.Where(i => i != null && !string.IsNullOrEmpty(i.JsonFilePathDebug)))
					.Select(i => ResolveDebugPath(i.JsonFilePathDebug))
					.Where(p => !string.IsNullOrEmpty(p))
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToList();

				served.Should().NotBeEmpty(
					$"le groupe [{string.Join(", ", group.CardSetNames)}] declare des conversions mais " +
					"aucun CardSet servi ne lui correspond dans WebBasedGeneratorConfig — cablage a verifier.");

				var surfaces = served
					.Select(p => (Path: p, Surface: File.Exists(p) ? ReadLiveSurface(p) : null))
					.ToList();

				surfaces.Where(s => s.Surface == null).ToList().Should().BeEmpty(
					"chaque gabarit declare par la config doit exister dans le checkout.");

				foreach (var s in surfaces) measuredTemplates.Add(s.Path);

				foreach (var conversion in group.StaticConversions)
				{
					measuredKeys++;
					if (!surfaces.Any(s => s.Surface.Contains(conversion.sourceText, StringComparison.Ordinal)))
					{
						failures.Add(
							$"groupe [{string.Join(", ", group.CardSetNames)}] : la cle " +
							$"{conversion.sourceText[..Math.Min(40, conversion.sourceText.Length)]}… " +
							"n'apparait dans AUCUN gabarit servi du groupe (touches mustache/css) — " +
							"la conversion est un no-op : le gabarit et la cle ont du diverger (#1571).");
					}
				}
			}

			measuredKeys.Should().BeGreaterThan(0, "la config par defaut porte des CardSetLocalizations.");
			failures.Should().BeEmpty(
				$"toute cle StaticConversions doit avoir un porteur parmi les gabarits fr servis par son groupe. {failures.Count} cle(s) orpheline(s) sur {measuredKeys}, gabarits balayes : {measuredTemplates.Count}.");
		}
	}
}
