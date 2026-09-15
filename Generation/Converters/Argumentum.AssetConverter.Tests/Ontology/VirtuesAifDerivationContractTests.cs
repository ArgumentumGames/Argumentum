using System;
using System.Collections.Generic;
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
	/// #989 architecture B organ — the Virtues AIF attack layer is PUBLISHED WITH ITS DERIVATION.
	/// The Virtues' AIF_attackType/AIF_attackedNode values were written by the architecture-B rule
	/// (ai-01 arbitration 2026-08-31, msg-20260831T172136-w5x7gm): strict majority of the opposed
	/// fallacies' measured attack types, exact tie ⇒ declared gap (empty cells + AIF_skosOther
	/// note) — 142 mapped / 80 declared gaps, not line-by-line editorial judgment. The issue DoD
	/// requires that derivation to be DECLARED in the published ontology. These tests pin that:
	///
	/// (1-4) the provenance marker is COMPUTED — re-deriving the script's fingerprint classifies
	///       a signature-consistent pair as "script-derived" and any deviation (uncoupled pair,
	///       gap note on a valued row) as "human-reviewed" (pure contract, fabricated witnesses;
	///       a constant marker would fail here);
	/// (5)   the committed argumentum_virtues.owl carries an aifAttackTypeProvenance annotation
	///       on EVERY aifAttackType assertion — 0 unmarked;
	/// (6)   the committed counts match the corpus exactly, with the expected split derived from
	///       the CSV at test time by an INDEPENDENT in-test implementation of the fingerprint
	///       (like CrossLinkArrowCountTests derives from the CSV: editing the CSV without
	///       regenerating docs/ontology/argumentum_virtues.owl goes RED);
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
		// (1-4) DeriveAttackTypeProvenance — sensitivity proofs on fabricated witnesses.
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void Provenance_FingerprintConsistentPair_IsScriptDerived()
		{
			var witness = new Virtue
			{
				AIFAttackType = "undercut",
				AIFAttackedNode = "RA-node",
				CrossLinkOpposes = "520;1000",
				AIFSkosOther = ""
			};
			VirtueOwlDocumentConfig.DeriveAttackTypeProvenance(witness).Should().Be("script-derived",
				"a stored pair carrying the script fingerprint (deterministic type-node coupling, no gap " +
				"note on a valued row) carries the derivation marker — the corpus value WAS produced by " +
				"the architecture-B script.");
		}

		[Fact]
		public void Provenance_UncoupledPair_IsHumanReviewed()
		{
			var witness = new Virtue
			{
				AIFAttackType = "undercut",
				AIFAttackedNode = "I-node", // the coupling is undercut->RA-node
				CrossLinkOpposes = "520;1000"
			};
			VirtueOwlDocumentConfig.DeriveAttackTypeProvenance(witness).Should().Be("human-reviewed",
				"a stored pair that breaks the deterministic type-node coupling proves a judgment REPLACED " +
				"the derived value — the marker must flip, which is exactly what a hand revision does.");
		}

		[Fact]
		public void Provenance_ValuedRowCarryingGapNote_IsHumanReviewed()
		{
			var witness = new Virtue
			{
				AIFAttackType = "undermine",
				AIFAttackedNode = "I-node",
				CrossLinkOpposes = "520;1000",
				AIFSkosOther = "Declared gap (#989 architecture B ...): tie, no strict majority"
			};
			VirtueOwlDocumentConfig.DeriveAttackTypeProvenance(witness).Should().Be("human-reviewed",
				"gap separation is half the fingerprint: the script writes EITHER a coupled pair OR a gap " +
				"note, never both — a valued row carrying the note is a post-script hand edit.");
		}

		[Fact]
		public void Provenance_UnknownType_IsHumanReviewed()
		{
			var witness = new Virtue { AIFAttackType = "dismantle", AIFAttackedNode = "RA-node", CrossLinkOpposes = "520" };
			VirtueOwlDocumentConfig.DeriveAttackTypeProvenance(witness).Should().Be("human-reviewed",
				"the three-valued vocabulary (undercut/undermine/rebut) is part of the ratified contract; " +
				"a fourth token cannot be a script output.");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (4) The declaration is self-contained (readable without the issue).
		// ─────────────────────────────────────────────────────────────────────────────

		[Fact]
		public void DerivationDeclaration_NamesRuleMarkersAndFingerprint()
		{
			foreach (var token in new[] { "script-derived", "human-reviewed", "architecture B", "strict majority",
				"declared gap", "undercut", "RA-node", "142" })
			{
				VirtueOwlDocumentConfig.DerivationDeclaration.Should().Contain(token,
					"the declaration is the whole point of the architecture-B write: rule, markers and the " +
					"script fingerprint must be legible to a reader who has never opened #989.");
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

		/// <summary>INDEPENDENT re-derivation of the #989 architecture B fingerprint (do NOT call
		/// the generator's version — the organ derives its expectation separately, like
		/// CrossLinkArrowCountTests derives from the CSV). Signature-level, deliberately: a valued
		/// row must carry the deterministic type→node coupling and never a gap note; a perimeter
		/// row left empty must carry the declared-gap note. The majority rule itself is NOT
		/// re-derivable from the Virtues CSV alone (it needs the Fallacies corpus — generator
		/// Phase 3); the resulting counts are pinned by VirtueAifCensusTests.</summary>
		private static (int filled, int ruleConsistent) CorpusExpectation()
		{
			var csv = new HarvestCardIdsCsv(VirtuesCsv);
			var types = csv.LoadColumn("AIF_attackType");
			var nodes = csv.LoadColumn("AIF_attackedNode");
			var opposes = csv.LoadColumn("crossLink_Opposes");
			var others = csv.LoadColumn("AIF_skosOther");
			types.Count.Should().Be(nodes.Count).And.Be(opposes.Count).And.Be(others.Count,
				"the four columns are read from the same file by header name; a count mismatch means the " +
				"header was edited, which the #497 HARD rules forbid.");

			var expectedNodeByType = new Dictionary<string, string>
			{
				["undermine"] = "I-node",
				["undercut"] = "RA-node",
				["rebut"] = "CA-node",
			};

			int filled = 0, consistent = 0;
			for (var i = 0; i < types.Count; i++)
			{
				var t = types[i].Trim();
				var hasNote = others[i].Trim().Length > 0;
				if (t.Length == 0)
				{
					// The root (pk 0) opposes nothing and stays empty; a PERIMETER row left empty
					// must be a DECLARED gap — an undeclared empty cell is non-treatment.
					(opposes[i].Trim().Length == 0 || hasNote).Should().BeTrue(
						"row {0} has a non-empty crossLink_Opposes but no attack type and no AIF_skosOther " +
						"note — an undeclared empty is exactly the ambiguity #989 architecture B forbids",
						i);
					continue;
				}
				filled++;
				var expectedNode = expectedNodeByType.TryGetValue(t, out var en) ? en : null;
				if (nodes[i].Trim() == expectedNode && !hasNote) consistent++;
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

			filled.Should().BeGreaterThanOrEqualTo(142,
				"142 rows hold a strict-majority value after the #989 architecture B write (undermine 74 / " +
				"undercut 63 / rebut 5, against 80 declared gaps); the floor only rises when a future revision " +
				"dissolves ties with newly measured fallacy values — a fall means rows lost their derived values.");
			consistent.Should().Be(filled,
				"current corpus state: ALL valued pairs carry the script fingerprint (deterministic type→node " +
				"coupling and no gap note on a valued row). If this fails, the corpus gained a genuinely " +
				"revised line — then the split below shifts with it and the ontology must be regenerated; the " +
				"assertion failing FIRST on the corpus side is by design (distinguish a corpus change from a " +
				"stale ontology).");

			scriptDerived.Should().Be(consistent,
				"the emitter re-derives the fingerprint per row: consistent pair => 'script-derived'.");
			humanReviewed.Should().Be(filled - consistent,
				"deviating pair => 'human-reviewed' (0 today; any future human revision lands here).");
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

			published.Should().Contain("rdf-schema#comment").And.Contain("architecture B").And.Contain("script-derived",
				"the derivation declaration is an ontology-level rdfs:comment in the published " +
				"argumentum_virtues.owl: rule, markers and fingerprint legible to a reader who has " +
				"never opened #989 (the second DoD disjunct: 'la dérivation est déclarée').");
		}
	}
}
