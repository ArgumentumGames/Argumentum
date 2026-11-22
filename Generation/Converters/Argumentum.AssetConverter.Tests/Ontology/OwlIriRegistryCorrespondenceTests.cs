using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Argumentum.AssetConverter;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
	/// <summary>
	/// #133 IRI freeze, round 2 (P24-36, pool #458) — the registry became the ONLY source of
	/// served IRIs in P24-34, and the 21/11 ai-01 mutation sweep left 3 mutants green: an
	/// arbitrary registry entry altered (pk 777), the `_Conflict` mint site and the scheme mint
	/// site reverted to `GetId`. A registry that is the single source but is only pinned on two
	/// named entries is pinned on two entries — this file closes that gap.
	///
	/// Part 1 — OFFLINE correspondence, registry ↔ committed blobs (docs/ontology/*.owl):
	/// (1a) the registry's distinct fragments EQUAL the blob's served concept set (the distinct
	///      subjects of skos:prefLabel) — an entry pointing outside the served set reddens;
	/// (1b) per pk: the PINNED fragment carries the row's fr AND en prefLabels in the blob —
	///      flipping pk 777 to any other served fragment reddens here (labels do not match);
	///      the value-level join that makes "the registry is pinned in full" true, not just
	///      set-shaped (homonyms are told apart by the fr∧en pair);
	/// (1c) the blob's skos:broader annotation count equals entities − 1 (every row except the
	///      root carries one parent edge: depth&gt;1 rows via their path, depth-1 family rows
	///      via the First() fallback in GetParent) — measured 1407/222 on the committed pair.
	/// Part 2 — PRODUCTION-channel synthetic discriminators: full generation through the public
	/// GenerateMindMapFile entry with the REAL corpus loaded via the production ClassMaps and
	/// two labels rewritten in memory (the P24-34 lesson, extended: the Fallacies corpus has no
	/// measured rename, so real labels mint exactly their pins — only a rewritten label makes
	/// the scheme and `_Conflict` sites observable):
	/// (2a) scheme IRIs come from the pinned root fragment (both corpora);
	/// (2b) the `_Conflict` union class comes from the pinned row fragment (Fallacies; the
	///      Count&gt;1 branch is forced deterministically with two direct refs).
	/// </summary>
	[Collection(PublishedOntologyCollection.Name)]
	public class OwlIriRegistryCorrespondenceTests
	{
		private const string FallaciesCsvRel = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv";
		private const string VirtuesCsvRel = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv";
		private const string FallaciesBlobRel = "docs/ontology/argumentum.owl";
		private const string VirtuesBlobRel = "docs/ontology/argumentum_virtues.owl";
		private const string FallaciesNs = "https://www.argumentum.games/argumentum_fallacies.owl#";
		private const string VirtuesNs = "https://www.argumentum.games/argumentum_virtues.owl#";
		private const string SkosNs = "http://www.w3.org/2004/02/skos/core#";

		private static readonly Regex PrefLabelRegex = new Regex(
			@"<AnnotationProperty IRI=""" + Regex.Escape(SkosNs + "prefLabel") +
			@"""\s*/>\s*<IRI>(?<iri>[^<]+)</IRI>\s*" +
			@"<Literal xml:lang=""(?<lang>\w+)"">(?<label>[^<]*)</Literal>",
			RegexOptions.Compiled);

		private static readonly Regex BroaderAnnotationRegex = new Regex(
			@"<AnnotationProperty IRI=""" + Regex.Escape(SkosNs + "broader") + @"""",
			RegexOptions.Compiled);

		private static string RepoRoot() => TestRepoRoot.Find();

		// ── corpus loading: the PRODUCTION channel — CsvBase<Fallacy, FallacyClassMap>.Load is
		//    the exact loader the pipeline invokes (via reflection) : its PrepareHeaderForMatch
		//    strips diacritics/underscores/hyphens and lowercases, which is how Name("pk")
		//    resolves against the uppercase 'PK' header of the Fallacies file. ───────────────

		private static List<Fallacy> LoadFallacies()
		{
			var path = Path.Combine(RepoRoot(), FallaciesCsvRel);
			File.Exists(path).Should().BeTrue("the Fallacies corpus must exist at '{0}'", FallaciesCsvRel);
			var rows = CsvBase<Fallacy, FallacyClassMap>.Load(path).ToList();
			rows.Should().HaveCount(1408, "row count measured 2026-11-21; moves WITH the registry extension");
			return rows;
		}

		private static List<Virtue> LoadVirtues()
		{
			var path = Path.Combine(RepoRoot(), VirtuesCsvRel);
			File.Exists(path).Should().BeTrue("the Virtues corpus must exist at '{0}'", VirtuesCsvRel);
			var rows = CsvBase<Virtue, VirtueClassMap>.Load(path).ToList();
			rows.Should().HaveCount(223, "row count measured 2026-11-21; moves WITH the registry extension");
			return rows;
		}

		/// <summary>Value-level join, per corpus. fr is STRICT: the pinned fragment must carry
		/// the row's fr label (the fr side never drifted on the committed pair). en carries the
		/// freeze semantics: the pinned fragment carries the row's en label, OR that label is
		/// served by NO fragment — the measured post-commit renames (Virtues pk 128/176, #1795):
		/// the artefact is frozen with the OLD label, the CSV moved, and the pin follows the pk.
		/// A flipped entry (mutant 'pk 777 altered') still reddens: its row's en label IS served
		/// (on the true fragment) but NOT on the wrong one.</summary>
		private static void AssertEachPinnedFragmentCarriesItsRowLabels(
			string blobRel, string ns,
			IEnumerable<(string pk, string fr, string en)> rows,
			IReadOnlyDictionary<string, string> registry,
			string corpus)
		{
			var blob = LoadBlobLabels(blobRel, ns);
			var allServedEn = blob.Values.SelectMany(v => v.En).ToHashSet();
			var offenders = new List<string>();

			foreach (var (pk, fr, en) in rows)
			{
				registry.TryGetValue(pk, out var pinned).Should().BeTrue(
					"coverage is pinned by OwlIriPinningContractTests; a miss here means that pin regressed ({0} pk {1}).",
					corpus, pk);
				if (!blob.TryGetValue(pinned!, out var labels))
				{
					offenders.Add($"{corpus} pk {pk}: fragment '{pinned}' absent from the committed blob");
					continue;
				}
				if (!string.IsNullOrEmpty(fr) && !labels.Item1.Contains(fr))
					offenders.Add($"{corpus} pk {pk}: '{pinned}' does not carry fr label '{fr}'");
				if (!string.IsNullOrEmpty(en) && !labels.Item2.Contains(en) && allServedEn.Contains(en))
					offenders.Add($"{corpus} pk {pk}: '{pinned}' does not carry en label '{en}' (and that label IS " +
						$"served elsewhere — a post-commit rename would leave it served NOWHERE)");
			}

			offenders.Should().BeEmpty(
				"the pk→fragment join is pinned at VALUE level: flipping any registry entry to another " +
				"served fragment breaks the fr∧en label correspondence (homonyms are told apart by the " +
				"fr∧en pair), and the only en divergence tolerated is the post-commit rename — a label " +
				"served by no fragment at all. First offendors: {0}",
				string.Join(" | ", offenders.Take(3)));
		}
		private static Dictionary<string, (HashSet<string> Fr, HashSet<string> En)> LoadBlobLabels(
			string blobRel, string ns)
		{
			var text = File.ReadAllText(Path.Combine(RepoRoot(), blobRel));
			var result = new Dictionary<string, (HashSet<string>, HashSet<string>)>();
			foreach (Match m in PrefLabelRegex.Matches(text))
			{
				var iri = m.Groups["iri"].Value;
				if (!iri.StartsWith(ns, StringComparison.Ordinal))
				{
					continue;
				}
				var frag = iri.Substring(ns.Length);
				var lang = m.Groups["lang"].Value;
				var label = WebUtility.HtmlDecode(m.Groups["label"].Value);
				if (!result.TryGetValue(frag, out var sets))
				{
					sets = (new HashSet<string>(), new HashSet<string>());
					result[frag] = sets;
				}
				if (lang == "fr") sets.Item1.Add(label);
				else if (lang == "en") sets.Item2.Add(label);
			}
			return result;
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (1a) Set correspondence — registry values == served concept set.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void Fallacies_RegistryValuesEqualTheServedConceptSet()
		{
			var served = LoadBlobLabels(FallaciesBlobRel, FallaciesNs).Keys.ToHashSet();
			var registry = OwlIriRegistry.Fallacies.Values.ToHashSet();

			served.Should().NotBeEmpty("the committed blob must carry prefLabel subjects for the control to bite.");
			var registryOnly = registry.Except(served).ToList();
			var servedOnly = served.Except(registry).ToList();
			registryOnly.Should().BeEmpty(
				"every registry value must be a SERVED concept fragment — a pin pointing outside the " +
				"committed artefact is a fabrication (mutant class 'pk 777 altered'). Offendors: {0}",
				string.Join(", ", registryOnly.Take(5)));
			servedOnly.Should().BeEmpty(
				"every served concept fragment must be pinned — an unpinned served IRI means the " +
				"registry and the artefact drifted apart. Offendors: {0}",
				string.Join(", ", servedOnly.Take(5)));
		}

		[Fact]
		public void Virtues_RegistryValuesEqualTheServedConceptSet()
		{
			var served = LoadBlobLabels(VirtuesBlobRel, VirtuesNs).Keys.ToHashSet();
			var registry = OwlIriRegistry.Virtues.Values.ToHashSet();

			served.Should().NotBeEmpty();
			registry.Except(served).ToList().Should().BeEmpty("same pin as Fallacies: no fabricated fragment.");
			served.Except(registry).ToList().Should().BeEmpty("same pin as Fallacies: no unpinned served IRI.");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (1b) Value correspondence — each pinned fragment carries ITS row's labels.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void Fallacies_EachPinnedFragmentCarriesItsRowLabels()
		{
			AssertEachPinnedFragmentCarriesItsRowLabels(
				FallaciesBlobRel, FallaciesNs,
				LoadFallacies().Select(f => (f.PK, f.TextFr, f.TextEn)),
				OwlIriRegistry.Fallacies, "Fallacies");
		}

		[Fact]
		public void Virtues_EachPinnedFragmentCarriesItsRowLabels()
		{
			AssertEachPinnedFragmentCarriesItsRowLabels(
				VirtuesBlobRel, VirtuesNs,
				LoadVirtues().Select(v => (v.Pk, v.TitleFr, v.TitleEn)),
				OwlIriRegistry.Virtues, "Virtues");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (1c) Structure correspondence — broader annotations == entities − 1.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void Fallacies_BroaderAnnotationCountMatchesTheTree()
		{
			var text = File.ReadAllText(Path.Combine(RepoRoot(), FallaciesBlobRel));
			var broaderAnnotations = BroaderAnnotationRegex.Matches(text).Count;

			broaderAnnotations.Should().Be(LoadFallacies().Count - 1,
				"every row except the root carries exactly one skos:broader annotation: depth>1 rows via " +
				"their path parent, depth-1 family rows via the First() fallback in GetParent (measured " +
				"1407 on the committed blob = 1408 rows − the root). A drift means the tree or the blob moved.");
		}

		[Fact]
		public void Virtues_BroaderAnnotationCountMatchesTheTree()
		{
			var text = File.ReadAllText(Path.Combine(RepoRoot(), VirtuesBlobRel));
			var broaderAnnotations = BroaderAnnotationRegex.Matches(text).Count;

			broaderAnnotations.Should().Be(LoadVirtues().Count - 1,
				"symmetric with Fallacies (measured 222 = 223 rows − the root).");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (2) PRODUCTION channel — full generation, real corpus, two labels rewritten.
		//     The scheme and `_Conflict` sites live inside the generation entry; only the
		//     serialized output observes them, and only a rewritten label makes the pin
		//     observable (real Fallacies labels mint exactly their pins — no measured rename).
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public async Task Fallacies_FullGeneration_SchemeAndConflictFollowTheRegistry()
		{
			var fallacies = LoadFallacies();
			var doc = new OwlGeneratorConfig().DocumentConfigs.Single();

			// Scheme discriminator: the ROOT row, label rewritten in memory.
			var root = fallacies[0];
			root.TextEn = "Zzz Rewritten Root Label";
			var rootPin = OwlIriRegistry.Fallacies[root.PK];
			var rootLabelMint = OwlDocumentConfig.GetId(root.TextEn);

			// `_Conflict` discriminator: force the Count>1 union branch deterministically —
			// two direct refs, no exception ref — and rewrite the label.
			var conflictRow = fallacies.First(f => f != root && !string.IsNullOrEmpty(f.AIFSkosMappingType));
			conflictRow.AIFSkosMappingType = "skos:exactMatch";
			conflictRow.AIFSkosDirectRef = "CorrespondenceRefA,CorrespondenceRefB";
			conflictRow.AIFSkosExceptionRef = "";
			conflictRow.TextEn = "Zzz Rewritten Conflict Label";
			var conflictPin = OwlIriRegistry.Fallacies[conflictRow.PK];
			var conflictLabelMint = OwlDocumentConfig.GetId(conflictRow.TextEn);

			var config = new AssetConverterConfig();
			await doc.GenerateMindMapFile(fallacies, config, "", "fr");
			var outputPath = ReadGeneratedOutput(config, "fr", doc.DocumentName);
			try
			{
				var output = File.ReadAllText(outputPath);
				output.Should().Contain($"{FallaciesNs}{rootPin}Scheme",
					"the scheme IRI is minted from the pinned ROOT fragment (mutant 'schéma → GetId' reddens here).");
				output.Should().NotContain($"{FallaciesNs}{rootLabelMint}Scheme",
					"the rewritten root label must NOT mint the scheme — contrast witness of the same pin.");
				output.Should().Contain($"{FallaciesNs}{conflictPin}_Conflict",
					"the union-class IRI is minted from the pinned ROW fragment (mutant '_Conflict → GetId' reddens here).");
				output.Should().NotContain($"{FallaciesNs}{conflictLabelMint}_Conflict",
					"the rewritten row label must NOT mint the union class — contrast witness.");
			}
			finally
			{
				File.Delete(outputPath);
			}
		}

		[Fact]
		public async Task Virtues_FullGeneration_SchemeFollowsTheRegistry()
		{
			var virtues = LoadVirtues();
			var doc = new VirtueOwlGeneratorConfig().DocumentConfigs.Single();

			var root = virtues[0];
			root.TitleEn = "Zzz Rewritten Root Label";
			var rootPin = OwlIriRegistry.Virtues[root.Pk];
			var rootLabelMint = VirtueOwlDocumentConfig.GetId(root.TitleEn);

			var config = new AssetConverterConfig();
			await doc.GenerateMindMapFile(virtues, config, "", "fr");
			var outputPath = ReadGeneratedOutput(config, "fr", doc.DocumentName);
			try
			{
				var output = File.ReadAllText(outputPath);
				output.Should().Contain($"{VirtuesNs}{rootPin}Scheme",
					"symmetric with Fallacies: the Virtues scheme IRI follows the pinned root fragment.");
				output.Should().NotContain($"{VirtuesNs}{rootLabelMint}Scheme",
					"contrast witness: the rewritten label must not mint the scheme.");
			}
			finally
			{
				File.Delete(outputPath);
			}
		}

		/// <summary>Resolves the artefact path the production entry just wrote (the test run's
		/// Target directory — never the repository) and asserts the file exists.</summary>
		private static string ReadGeneratedOutput(AssetConverterConfig config, string language, string documentName)
		{
			var path = Path.Combine(config.GetBaseTargetDirectory(language), "Ontology", documentName);
			File.Exists(path).Should().BeTrue(
				"the production entry must have written '{0}' — its absence means the entry contract moved.",
				path);
			return path;
		}
	}
}
