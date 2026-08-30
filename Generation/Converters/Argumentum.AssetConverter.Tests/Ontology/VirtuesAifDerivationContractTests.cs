using System;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using OWLSharp.Ontology;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
	/// <summary>
	/// #989 branch B organ — the Virtues AIF attack layer is PUBLISHED WITH ITS DERIVATION.
	/// The Virtues' AIF_attackType/AIF_attackedNode values were back-filled by a deterministic
	/// 3-branch script (plan #750 v2, 206/222 default), not by line-by-line editorial judgment;
	/// the issue DoD requires that derivation to be DECLARED in the published ontology. These
	/// tests pin that:
	///
	/// (1-4) the provenance marker is COMPUTED — re-deriving the ratified rule classifies a
	///       rule-consistent pair as "script-derived" and any deviation as "human-reviewed"
	///       (pure contract, fabricated witnesses; a constant marker would fail here);
	/// (5)   the committed argumentum_virtues.owl carries an aifAttackTypeProvenance annotation
	///       on EVERY aifAttackType assertion — 0 unmarked;
	/// (6)   the committed counts match the corpus exactly, with the expected split derived from
	///       the CSV at test time by an INDEPENDENT in-test implementation of the rule (like
	///       CrossLinkArrowCountTests derives from the CSV: editing the CSV without regenerating
	///       docs/ontology/argumentum_virtues.owl goes RED);
	/// (7)   the ontology-level declaration comment survives the OWL2XML round-trip, so a reader
	///       who has never seen #989 can tell a derived value from a reviewed one.
	/// </summary>
	[Collection(PublishedOntologyCollection.Name)]
	public class VirtuesAifDerivationContractTests
	{
		private static string RepoRoot => TestRepoRoot.Find();

		private static string VirtuesCsv => Path.Combine(RepoRoot, "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static readonly Lazy<OwlAdapter> _virtuesOntology = new Lazy<OwlAdapter>(() =>
			OwlAdapter.FromFile(Path.Combine(RepoRoot, "docs", "ontology", "argumentum_virtues.owl")));

		private static OwlAdapter VirtuesOntology => _virtuesOntology.Value;

		// ─────────────────────────────────────────────────────────────────────────────
		// (1-3) DeriveAttackTypeProvenance — sensitivity proofs on fabricated witnesses.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void Provenance_RuleConsistentPair_IsScriptDerived()
		{
			var witness = new Virtue
			{
				AIFAttackType = "undercut",
				AIFAttackedNode = "RA-node",
				CrossLinkOpposes = "520;1000"
			};
			VirtueOwlDocumentConfig.DeriveAttackTypeProvenance(witness).Should().Be("script-derived",
				"a stored pair equal to the rule output (default branch: no override fallacy opposed) " +
				"carries the derivation marker — the corpus value WAS produced by the script.");
		}

		[Fact]
		public void Provenance_DeviatingPair_IsHumanReviewed()
		{
			var witness = new Virtue
			{
				AIFAttackType = "rebut",
				AIFAttackedNode = "CA-node",
				CrossLinkOpposes = "" // rule says undercut/RA-node here
			};
			VirtueOwlDocumentConfig.DeriveAttackTypeProvenance(witness).Should().Be("human-reviewed",
				"a stored pair that deviates from the rule output proves a judgment REPLACED the derived " +
				"value — the marker must flip, which is exactly what a future #989 branch-A revision does.");
		}

		[Fact]
		public void Provenance_OverridePrecedence_UndermineBeatsRebut()
		{
			var both = new Virtue { AIFAttackType = "undermine", AIFAttackedNode = "I-node", CrossLinkOpposes = "889;340" };
			VirtueOwlDocumentConfig.DeriveAttackTypeProvenance(both).Should().Be("script-derived",
				"a virtue opposing both 889 (undermine set) and 340 (rebut set) gets undermine/I-node — " +
				"the script checks the undermine branch FIRST (elif chain); the marker re-derivation must " +
				"mirror that precedence exactly.");
			var rebutOnly = new Virtue { AIFAttackType = "rebut", AIFAttackedNode = "CA-node", CrossLinkOpposes = "340" };
			VirtueOwlDocumentConfig.DeriveAttackTypeProvenance(rebutOnly).Should().Be("script-derived",
				"opposing 340 alone selects the rebut branch.");
		}

		[Fact]
		public void Provenance_SeparatorIsSemicolonSpaceSeparatedDoesNotFire()
		{
			var witness = new Virtue { AIFAttackType = "undercut", AIFAttackedNode = "RA-node", CrossLinkOpposes = "889, 340" };
			VirtueOwlDocumentConfig.DeriveAttackTypeProvenance(witness).Should().Be("script-derived",
				"crossLink columns are ';'-separated path/PK lists; a comma-separated cell must not be " +
				"parsed as two PKs (the corpus never uses commas there).");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (4) The declaration is self-contained (readable without the issue).
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void DerivationDeclaration_NamesRuleMarkersAndFingerprint()
		{
			foreach (var token in new[] { "script-derived", "human-reviewed", "undercut", "889", "340", "1:1" })
			{
				VirtueOwlDocumentConfig.DerivationDeclaration.Should().Contain(token,
					"the declaration is the whole point of branch B: rule, markers and the 1:1 script " +
					"fingerprint must be legible to a reader who has never opened #989.");
			}
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (5-6) The committed ontology matches the corpus (load-path organ).
		// ─────────────────────────────────────────────────────────────────────────────

		private static (int attackType, int scriptDerived, int humanReviewed) CountCommitted()
		{
			var assertions = VirtuesOntology.GetOntology().AnnotationAxioms.OfType<OWLAnnotationAssertion>().ToList();
			int attackType = assertions.Count(a => a.AnnotationProperty.GetIRI().ToString().EndsWith("aifAttackType")
				&& !a.AnnotationProperty.GetIRI().ToString().EndsWith("aifAttackTypeProvenance"));
			int provenance = assertions.Count(a => a.AnnotationProperty.GetIRI().ToString().EndsWith("aifAttackTypeProvenance"));
			int scriptDerived = assertions.Count(a => a.AnnotationProperty.GetIRI().ToString().EndsWith("aifAttackTypeProvenance")
				&& a.ValueLiteral != null && a.ValueLiteral.GetLiteral().ToString().Contains("script-derived"));
			return (attackType, scriptDerived, provenance - scriptDerived);
		}

		/// <summary>INDEPENDENT re-implementation of the plan #750 v2 rule (do NOT call the
		/// generator's version — the organ derives its expectation separately, like
		/// CrossLinkArrowCountTests derives from the CSV).</summary>
		private static (int filled, int ruleConsistent) CorpusExpectation()
		{
			var csv = new HarvestCardIdsCsv(VirtuesCsv);
			var types = csv.LoadColumn("AIF_attackType");
			var nodes = csv.LoadColumn("AIF_attackedNode");
			var opposes = csv.LoadColumn("crossLink_Opposes");
			types.Count.Should().Be(nodes.Count).And.Be(opposes.Count,
				"the three columns are read from the same file by header name; a count mismatch means the " +
				"header was edited, which the #497 HARD rules forbid.");

			int filled = 0, consistent = 0;
			for (var i = 0; i < types.Count; i++)
			{
				var t = types[i].Trim();
				if (t.Length == 0) continue; // root Virtue (pk 0, no scheme) — empty by design
				filled++;
				var opp = opposes[i].Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToHashSet();
				var (rt, rn) = opp.Contains("889") || opp.Contains("804") ? ("undermine", "I-node")
					: opp.Contains("340") ? ("rebut", "CA-node")
					: ("undercut", "RA-node");
				if (t == rt && nodes[i].Trim() == rn) consistent++;
			}
			return (filled, consistent);
		}

		[Fact]
		public void CommittedOntology_EveryAttackTypeCarriesProvenance_0Unmarked()
		{
			var (attackType, _, _) = CountCommitted();
			var csv = new HarvestCardIdsCsv(VirtuesCsv);
			var expectedFilled = csv.LoadColumn("AIF_attackType").Count(v => v.Trim().Length > 0);

			attackType.Should().Be(expectedFilled,
				"one aifAttackType assertion per non-empty CSV cell (root pk 0 emits nothing). A mismatch " +
				"means the Virtues CSV was edited without regenerating docs/ontology/argumentum_virtues.owl " +
				"(--generate-owl), or the emission changed.");

			var subjects = VirtuesOntology.GetOntology().AnnotationAxioms.OfType<OWLAnnotationAssertion>();
			var typed = subjects.Where(a => a.AnnotationProperty.GetIRI().ToString().EndsWith("aifAttackType")
					&& !a.AnnotationProperty.GetIRI().ToString().EndsWith("aifAttackTypeProvenance"))
				.Select(a => a.SubjectIRI.ToString()).ToHashSet();
			var marked = subjects.Where(a => a.AnnotationProperty.GetIRI().ToString().EndsWith("aifAttackTypeProvenance"))
				.Select(a => a.SubjectIRI.ToString()).ToHashSet();
			typed.Except(marked).Should().BeEmpty(
				"every attack-typed virtue carries its derivation marker — an unmarked value is exactly the " +
				"ambiguity #989 exists to remove (0 unmarked is the DoD's 'la dérivation est déclarée').");
		}

		[Fact]
		public void CommittedOntology_ProvenanceSplit_MatchesIndependentRuleDerivation()
		{
			var (filled, consistent) = CorpusExpectation();
			var (attackType, scriptDerived, humanReviewed) = CountCommitted();

			filled.Should().BeGreaterThanOrEqualTo(220);
			consistent.Should().Be(filled,
				"current corpus state: ALL 222 filled pairs are rule-consistent (the 1:1 script fingerprint). " +
				"If this fails, the corpus gained a genuinely revised line — then the split below shifts with " +
				"it and the ontology must be regenerated; the assertion failing FIRST on the corpus side is " +
				"by design (distinguish a corpus change from a stale ontology).");

			scriptDerived.Should().Be(consistent,
				"the emitter re-derives the ratified rule per row: rule-consistent pair => 'script-derived'.");
			humanReviewed.Should().Be(filled - consistent,
				"deviating pair => 'human-reviewed' (0 today; any future branch-A revision lands here).");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (7) The declaration is readable in the PUBLISHED file. Pinned on the raw file, not the
		//     OWLSharp reload: the ontology-level declaration is an OWL2XML <Annotation> child of
		//     <Ontology> (structural metadata), which the AnnotationAssertion stream never carries —
		//     measuring it through the reloaded axioms would measure the reader's known gaps (#133),
		//     not the artifact.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void CommittedOntology_DeclarationComment_IsReadableInThePublishedFile()
		{
			var published = File.ReadAllText(Path.Combine(RepoRoot, "docs", "ontology", "argumentum_virtues.owl"));

			published.Should().Contain("rdf-schema#comment").And.Contain("3-branch rule").And.Contain("script-derived",
				"the derivation declaration is an ontology-level rdfs:comment in the published " +
				"argumentum_virtues.owl: rule, markers and 1:1 fingerprint legible to a reader who has " +
				"never opened #989 (the second DoD disjunct: 'la dérivation est déclarée').");
		}
	}
}
