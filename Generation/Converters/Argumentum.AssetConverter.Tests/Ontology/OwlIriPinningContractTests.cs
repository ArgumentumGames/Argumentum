using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Ontology;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using RDFSharp.Model;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
	/// <summary>
	/// #133 IRI freeze (P24-34, pool #458) — an English label never mints a served IRI.
	/// Measured 2026-10-09 (docs/ontology/133-owl-virtues-peremption-mesuree-2026-10-09.md):
	/// regenerating the Virtues ontology would rename 2 served IRIs (pk 128 and 176 — English
	/// titles renamed by #1795 after the artefact was committed) because concept fragments were
	/// minted from GetId(TitleEn/TextEn). Since this grain, both generators resolve every
	/// fragment through the pk-keyed OwlIriRegistry and REFUSE loudly on an unknown pk.
	///
	/// Three pins per corpus, symmetric:
	/// (1) coverage — every live CSV row has a pin; a new row fails here until the registry
	///     extends (never at regen time);
	/// (2) label-independence — rewriting the entity's EN label does not move the fragment, and
	///     the MEASURED rename (Virtues pk 128) resolves to its SERVED fragment through the
	///     production concept-mint channel (GetVirtueConcept / GetFallacyConcept), so reverting
	///     a generator call site to GetId reddens here — the #2332 lesson: a test-side replica
	///     of the call pins nothing;
	/// (3) refusal — an unknown pk throws, and the message carries the pk plus the fragment the
	///     label WOULD have minted: the refusal message IS the extension procedure.
	/// The blob-level DoD (0 served IRI disappears on a branch regen; versionInfo carried) is
	/// measured on the regenerated artefacts — evidence in the PR, not here.
	/// </summary>
	public class OwlIriPinningContractTests
	{
		private const string FallaciesCsvRel = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv";
		private const string VirtuesCsvRel = "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv";
		private const string VirtuesNs = "https://www.argumentum.games/argumentum_virtues.owl#";
		private const string FallaciesNs = "https://www.argumentum.games/argumentum_fallacies.owl#";

		// The measured rename, frozen as the test witness: CSV title_en of pk 128 as of
		// 2026-11-21 vs the SERVED fragment the committed blob carries (#1795 renamed the title
		// after the artefact was committed; the peremption report pinned both sides).
		private const string Pk128CurrentTitleEn = "Solid informal logic";
		private const string Pk128ServedFragment = "acceptableInformalLogic";

		private static string RepoRoot() => TestRepoRoot.Find();

		private static List<string> ReadCsvColumn(string relativePath, string column)
		{
			var path = Path.Combine(RepoRoot(), relativePath);
			File.Exists(path).Should().BeTrue("the corpus CSV must exist at '{0}'", relativePath);

			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				HeaderValidated = null,
			};
			using var reader = new StreamReader(path);
			using var csv = new CsvReader(reader, config);
			csv.Read();
			csv.ReadHeader();
			var headers = csv.HeaderRecord ?? Array.Empty<string>();
			var idx = Array.IndexOf(headers, column);
			idx.Should().BeGreaterOrEqualTo(0, "'{0}' must expose the '{1}' column", relativePath, column);

			var values = new List<string>();
			while (csv.Read())
			{
				values.Add(csv.GetField(idx) ?? "");
			}
			values.Should().NotBeEmpty("'{0}' must load rows", relativePath);
			return values;
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (1) Coverage — the registry spans the live corpus, per corpus.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void Fallacies_RegistryCoversEveryCsvRow()
		{
			var pks = ReadCsvColumn(FallaciesCsvRel, "PK");

			pks.Should().HaveCount(1408,
				"row count measured 2026-11-21: if rows were legitimately added, this expectation moves " +
				"WITH the registry extension in the same diff — a new row must fail here first, never at regen time.");
			var unpinned = pks.Where(pk => !OwlIriRegistry.Fallacies.ContainsKey(pk)).ToList();
			unpinned.Should().BeEmpty(
				"every live Fallacies row must carry a pinned fragment — a new row extends OwlIriRegistry " +
				"in the same reviewed diff (P24-34: an English label never mints a served IRI).");
		}

		[Fact]
		public void Virtues_RegistryCoversEveryCsvRow()
		{
			var pks = ReadCsvColumn(VirtuesCsvRel, "pk");

			pks.Should().HaveCount(223,
				"row count measured 2026-11-21: moves WITH the registry extension, same diff.");
			var unpinned = pks.Where(pk => !OwlIriRegistry.Virtues.ContainsKey(pk)).ToList();
			unpinned.Should().BeEmpty(
				"every live Virtues row must carry a pinned fragment — symmetric with Fallacies.");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (2) Label-independence — the measured rename, through the production channel.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void Virtues_MeasuredRenameKeepsItsServedIriThroughTheConceptChannel()
		{
			var virtue = new Virtue
			{
				Pk = "128",
				TitleFr = "Logique informelle acceptable",
				TitleEn = Pk128CurrentTitleEn,
				DescriptionFr = "Définition témoin.",
				DescriptionEn = "Witness definition.",
			};

			// Contrast witness: the fragment the CURRENT label would mint — the exact hazard
			// the 2026-10-09 report measured on pk 128.
			VirtueOwlDocumentConfig.GetId(virtue.TitleEn).Should().Be("solidInformalLogic",
				"witness: this is what the label would mint — proves the pin is not vacuous.");

			// Resolver level.
			VirtueOwlDocumentConfig.ResolveVirtueIriFragment(virtue).Should().Be(Pk128ServedFragment,
				"the fragment is pk-pinned: the served IRI keeps its identity across label edits.");

			// PRODUCTION channel: the exact method CreateVirtueOwlDocument calls per concept.
			// Reverting its mint site to GetId(TitleEn) reddens THIS assertion (#2332 lesson).
			var doc = new VirtueOwlGeneratorConfig().DocumentConfigs.Single();
			var adapter = new OwlAdapter(VirtuesNs);
			var scheme = new RDFResource($"{VirtuesNs}testScheme");
			var resource = doc.GetVirtueConcept(virtue, adapter, scheme);

			resource.ToString().Should().EndWith("#" + Pk128ServedFragment,
				"the concept IRI minted by the production channel carries the SERVED fragment, " +
				"not the label-derived one (P24-34 DoD: a served IRI never follows the label).");
		}

		[Fact]
		public void Virtues_RewrittenLabelDoesNotMoveTheFragment()
		{
			var virtue = new Virtue { Pk = "128", TitleEn = "A completely different label" };

			VirtueOwlDocumentConfig.ResolveVirtueIriFragment(virtue).Should().Be(Pk128ServedFragment,
				"mechanical label-independence: whatever the label becomes, the fragment is the pin. " +
				"The label only feeds the unknown-pk refusal message.");
		}

		[Fact]
		public void Fallacies_RewrittenLabelDoesNotMoveTheFragment()
		{
			var fallacy = new Fallacy { PK = "4", TextEn = "A completely different label" };

			OwlDocumentConfig.ResolveFallacyIriFragment(fallacy).Should().Be(OwlIriRegistry.Fallacies["4"],
				"symmetric with Virtues: the fragment is pk-pinned, the label never mints.");
		}

		[Fact]
		public void Fallacies_ConceptChannelMintsThePinnedFragment()
		{
			// Synthetic discriminator: unlike Virtues pk 128, no Fallacies row has a MEASURED
			// rename (label ≠ served fragment) today — so the label is rewritten on purpose.
			// With the real CSV label, a reverted call site would mint exactly the pin and the
			// fact could not bite (the trap the 21/11 #2332 review flagged).
			var fallacy = new Fallacy
			{
				PK = "4",
				TextFr = "Appel à l'ignorance",
				TextEn = "A completely different label",
				DescFr = "Définition témoin.",
				DescEn = "Witness definition.",
				ExampleFr = "Exemple témoin.",
				ExampleEn = "Witness example.",
			};

			// Mirror of the Virtues concept-channel fact: the production mint method of the
			// Fallacies generator. Reverting its mint site to GetId(TextEn) reddens HERE.
			var doc = new OwlGeneratorConfig().DocumentConfigs.Single();
			var adapter = new OwlAdapter(FallaciesNs);
			var scheme = new RDFResource($"{FallaciesNs}testScheme");
			var resource = doc.GetFallacyConcept(fallacy, adapter, scheme);

			resource.ToString().Should().EndWith("#" + OwlIriRegistry.Fallacies["4"],
				"the Fallacies concept IRI carries the pinned fragment — per-generator pin, " +
				"mirror of the Virtues fact (both call sites, not one).");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (3) Refusal — an unknown pk is loud and actionable.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void Virtues_UnknownPkRefusesLoudly()
		{
			var act = () => VirtueOwlDocumentConfig.ResolveVirtueIriFragment(
				new Virtue { Pk = "999999", TitleEn = "Ghost Virtue" });

			act.Should().Throw<InvalidOperationException>()
				.Which.Message.Should().Contain("999999").And.Contain("ghostVirtue",
					"the refusal message names the pk and the fragment the label WOULD have minted — " +
					"it IS the extension procedure for a new corpus row.");
		}

		[Fact]
		public void Fallacies_UnknownPkRefusesLoudly()
		{
			var act = () => OwlDocumentConfig.ResolveFallacyIriFragment(
				new Fallacy { PK = "999998", TextEn = "Ghost Fallacy" });

			act.Should().Throw<InvalidOperationException>()
				.Which.Message.Should().Contain("999998").And.Contain("ghostFallacy",
					"symmetric with Virtues: fail-loud, never a silent label-minted IRI.");
		}
	}
}
