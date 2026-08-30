using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
	/// <summary>
	/// VOCABULARY LOCK on the Fallacies' AIF scheme columns (#498 / #677 « 0 fabrication »).
	/// </summary>
	/// <remarks>
	/// <para><b>Why this organ exists.</b> The « no fabricated AIF token » invariant was, until now,
	/// enforced only inside <c>docs/taxonomy/498-reconciliation-p2-apply.py</c> — a ONE-SHOT script
	/// that will be archived when #498 lands. Worse, that script derives its whitelist from the very
	/// file it checks (<c>native</c> = the tokens already used in prod), so it can only catch a token
	/// novel to a <i>proposal</i>; a token written straight into the CSV would pass unnoticed, and
	/// the check disappears entirely with the script.</para>
	/// <para><b>What it locks.</b> The 60 native AIF tokens measured on master <c>feb0f718</c>
	/// (2026-08-30) across 70 rows of <c>AIF_skosDirectRef</c> + <c>AIF_skosExceptionRef</c>. They are
	/// pinned EXPLICITLY here rather than re-derived from the CSV: a whitelist derived from the file
	/// under test is vacuous by construction — it accepts whatever it finds.</para>
	/// <para><b>Why a pin and not a rule.</b> AIF scheme/conflict names are a closed upstream
	/// vocabulary (AIFdb / Walton schemes), not a pattern. There is no syntactic predicate that
	/// separates <c>Analogy_Inference</c> (native) from <c>Comparison_Inference</c> (fabricated, and
	/// explicitly forbidden by <c>498-aif-faulty-comparison-cluster.md:102</c>). Only enumeration
	/// discriminates.</para>
	/// <para><b>Growing the vocabulary is meant to hurt.</b> A genuinely new native token makes BOTH
	/// tests red — that is the design. Add it to <see cref="NativeAifVocabulary"/> and bump
	/// <see cref="PinnedTokenCount"/> in the same commit, citing the upstream AIF source. The tranche
	/// P3 (25 attack-only rows with NO vetted cluster doc, thinnest precedent of the whole campaign:
	/// 17 of them are <c>undermine</c>, a shape the ratified corpus exercises only twice) is exactly
	/// where fabrication pressure lives.</para>
	/// </remarks>
	public class FallacyAifVocabularyLockTests
	{
		private const int PinnedTokenCount = 60;

		/// <summary>Rows carrying at least one scheme token, measured on master feb0f718. The
		/// P2-A write (#1230, owner-gated) ADDS 46 rows drawn from this same vocabulary — hence a
		/// floor, not an equality: the organ must survive that write without being edited.</summary>
		private const int MinimumRowsCarryingTokens = 70;

		private static readonly string[] SchemeColumns =
			{ "AIF_skosDirectRef", "AIF_skosExceptionRef" };

		/// <summary>The closed native vocabulary. Sorted ordinal; see the remarks before editing.</summary>
		private static readonly HashSet<string> NativeAifVocabulary = new(StringComparer.Ordinal)
		{
			"AlternativeMeans_Conflict", "Analogy_Inference", "ArbitraryVerbalClassification_Inference",
			"Bias_Inference", "BiasedClassification_Conflict", "CausalSlipperySlope_Inference",
			"CauseToEffect_Inference", "CircumstantialAdHominem_Inference", "Commitment_Conflict",
			"Commitment_Inference", "ConflictingGoals_Conflict", "CorrelationToCause_Inference",
			"Deductive_Inference", "Dialogue_Scheme", "DifferencesUndermineSimilarity_Conflict",
			"Dilemma_Inference", "DirectAdHominem_Inference", "EstablishedRule_Inference",
			"Ethotic_Inference", "EvidenceToHypothesis_Inference", "Example_Inference",
			"ExceptionSimilarityCase_Conflict", "ExceptionalCase_Inference", "ExpertOpinion_Inference",
			"ExpertiseInconsistency_Conflict", "FearAppeal_Inference", "FullSlipperySlope_Inference",
			"GeneralAcceptanceDoubt_Conflict", "Gradualism_Inference", "Ignorance_Inference",
			"InconsistentCommitment_Inference", "InductiveInference_Scheme", "Inference_Scheme",
			"IrrationalFearAppeal_Conflict", "LackOfCompleteKnowledge_Conflict", "LackOfPTKReliability_Scheme",
			"Logical_Conflict", "ModusPonens_Inference", "NegativeConsequences_Inference",
			"OpposedCommitment_Conflict", "OppositeConsequences_Conflict", "OtherCausalFactorsInvolved_Conflict",
			"PopularOpinion_Inference", "PopularPractice_Inference", "PositionToKnow_Inference",
			"PositiveConsequences_Inference", "PracticalReasoning_Inference", "PrecedentSlipperySlope_Inference",
			"Preference_Scheme", "PresumptiveInference_Scheme", "PropertyNotExistant_Conflict",
			"RequiredSteps_Conflict", "ResolvingInconsistency_Conflict", "SignFromOtherEvents_Conflict",
			"Sign_Inference", "VagueVerbalClassification_Inference", "VerbalClassification_Inference",
			"VerbalSlipperySlope_Inference", "Waste_Inference", "WeakestLink_Conflict"
		};

		private static string FallaciesCsv => Path.Combine(TestRepoRoot.Find(),
			"Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		/// <summary>Splits a cell into tokens. Prod stores multi-token cells as <c>"A, B"</c>;
		/// CsvHelper has already stripped the field quote-pair, so comma AND whitespace both
		/// separate.</summary>
		private static IEnumerable<string> Tokens(string cell) =>
			cell.Replace(',', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(t => t.Trim()).Where(t => t.Length > 0);

		private static (List<(string Pk, string Column, string Token)> All, int RowsWithTokens) Scan()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			var pks = csv.LoadColumn("PK");
			var columns = SchemeColumns.ToDictionary(c => c, c => csv.LoadColumn(c));

			foreach (var (name, values) in columns)
				values.Count.Should().Be(pks.Count,
					$"'{name}' and 'PK' are read from the same file by header name; a count mismatch " +
					"means a header was renamed, which the #497 HARD rules forbid.");

			var all = new List<(string, string, string)>();
			var rowsWithTokens = 0;
			for (var i = 0; i < pks.Count; i++)
			{
				var before = all.Count;
				foreach (var (name, values) in columns)
					foreach (var token in Tokens(values[i]))
						all.Add((pks[i].Trim(), name, token));
				if (all.Count > before) rowsWithTokens++;
			}
			return (all, rowsWithTokens);
		}

		/// <summary>THE lock: every scheme token in prod belongs to the pinned native vocabulary.</summary>
		[Fact]
		public void EveryAifSchemeToken_ComesFromThePinnedNativeVocabulary()
		{
			var (all, _) = Scan();

			var offenders = all
				.Where(x => !NativeAifVocabulary.Contains(x.Token))
				.Select(x => $"PK {x.Pk} / {x.Column} -> '{x.Token}'")
				.Distinct().OrderBy(s => s, StringComparer.Ordinal).ToArray();

			offenders.Should().BeEmpty(
				"the AIF scheme columns may only carry tokens from the closed native vocabulary " +
				$"({PinnedTokenCount} pinned, #677 « 0 fabrication »). Each line below is a token that " +
				"exists in NO upstream AIF source — the failure mode the cluster docs warn about " +
				"verbatim (« Do not fabricate a Comparison_Inference / Ambiguity_Conflict token »). " +
				"If a token IS genuinely native, add it to NativeAifVocabulary and bump " +
				"PinnedTokenCount in the same commit, citing the upstream source. Offenders:\n  " +
				string.Join("\n  ", offenders));
		}

		/// <summary>ANTI-VACUITY. Test 1 is a subset assertion: it passes trivially on an empty scan
		/// (renamed header, moved CSV, broken encoding). This one proves the scan reaches real data,
		/// and that the pin still describes the corpus rather than having drifted into a superset
		/// nobody uses.</summary>
		[Fact]
		public void TheScan_ReachesRealData_AndThePinMatchesWhatIsMeasured()
		{
			var (all, rowsWithTokens) = Scan();

			all.Should().NotBeEmpty("an empty scan would make the vocabulary lock vacuously green.");
			rowsWithTokens.Should().BeGreaterThanOrEqualTo(MinimumRowsCarryingTokens,
				$"master feb0f718 carries {MinimumRowsCarryingTokens} rows with >=1 scheme token and the " +
				"#498 campaign only ever ADDS rows; a drop means the reader stopped seeing the columns.");

			NativeAifVocabulary.Count.Should().Be(PinnedTokenCount,
				"the pin and its declared count must agree — a silent edit to one of them is how a " +
				"fabricated token gets laundered into the vocabulary.");
		}
	}
}
