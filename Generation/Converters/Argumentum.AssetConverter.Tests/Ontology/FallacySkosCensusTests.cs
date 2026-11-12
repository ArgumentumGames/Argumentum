using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
	/// <summary>
	/// CENSUS of the Fallacies' AIF/skos columns (#498): three arbitrated states
	/// (P3 palier-1) + the grain-12 declared-imitation state.
	/// </summary>
	/// <remarks>
	/// <para><b>Why this organ exists.</b> Until the P3 palier-1 arbitration (ai-01,
	/// <c>msg-20260830T231148-kkk5hl</c>, engraved <c>issues/498#issuecomment-5471844652</c>), the
	/// campaign's stop-gate metric <c>attack-only</c> was fed by a census whose definition —
	/// « modeled = at least one skos cell » — cannot distinguish a genuine correspondence from a
	/// <i>declared gap</i>. The note-only shape accepted for PK 698/667 (skosOther with no ref,
	/// honest serialization of « no native AIF scheme exists ») would have counted as modeled and
	/// silently moved the stop-gate from 29 to 27 with no real mapping behind it. This test splits
	/// the census so the number that gates tranches keeps describing what its name says.</para>
	/// <para><b>The three states</b> (arbitration table, verbatim criteria):</para>
	/// <list type="table">
	/// <item><term>mapped</term><description>at least one ref — <c>AIF_skosDirectRef</c> OR
	/// <c>AIF_skosExceptionRef</c> non-empty. Measured 116 on <c>170acf30</c>.</description></item>
	/// <item><term>declared-gap</term><description><c>AIF_skosOther</c> non-empty AND no ref.
	/// Measured 0 on <c>170acf30</c> (the shape is new; P3 note-only writes create it).</description></item>
	/// <item><term>untouched</term><description>all four skos cells empty. Measured 1292.</description></item>
	/// </list>
	/// <para><b>The fourth state</b> (#498 grain 12): <c>AIF_imitatedScheme</c> non-empty AND no ref
	/// AND no <c>AIF_skosOther</c> — the FR-prose « imite … » of component (a), created by the
	/// group-2.1 PR. The three arbitrated states keep their verbatim criteria; the new column is
	/// neither a correspondence (it names what is imitated, not an AIF IRI) nor an absence — so it
	/// gets its own bucket, preserving the stop-gate semantics of <i>mapped</i>. A row with BOTH
	/// skosOther and imitatedScheme classifies as declared-gap (bucket precedence, one row one
	/// bucket); the group PRs write at most one representation per node (cadrage #2229 partition).</para>
	/// <para><b>Floors, never equalities</b> (except the structural sum). Every count is pinned as
	/// a floor so a legitimate write — including the gated P3 palier writes this organ is the
	/// precondition for — turns a floor green-ward without forcing a test edit. Only a regression
	/// (rows losing their refs, or the corpus shrinking) goes red.</para>
	/// <para><b>The sum is the unclassified-shape tripwire.</b> The buckets are asserted to sum to
	/// the file's data-row count computed at test time, not to a pinned constant: a row carrying
	/// e.g. <c>AIF_skosMappingType</c> alone (a shape no bucket describes, and one the corpus does
	/// not carry today) falls through all three and drives the sum below the row count — fail-loud,
	/// because « which bucket does this belong to » is exactly the decision such a row hides.
	/// The corpus size itself carries a floor (1408) so data loss is caught while organic growth
	/// stays green.</para>
	/// </remarks>
	public class FallacySkosCensusTests
	{
		private const int CorpusFloor = 1408;

		/// <summary>Rows with at least one ref, measured on master 170acf30 (116). This is the
		/// number that feeds <c>attack-only</c> — the campaign's stop-gate — so it is the one
		/// count here that must never silently <i>fall</i>.</summary>
		private const int MappedFloor = 116;

		private static readonly string[] RefColumns = { "AIF_skosDirectRef", "AIF_skosExceptionRef" };
		private static readonly string[] AllSkosColumns =
			{ "AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosOther", "AIF_skosMappingType", "AIF_imitatedScheme" };

		private static string FallaciesCsv => Path.Combine(TestRepoRoot.Find(),
			"Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		private static (int Mapped, int DeclaredGap, int DeclaredImitation, int Untouched, int Rows) Census()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			var pks = csv.LoadColumn("PK");
			var refs = RefColumns.ToDictionary(c => c, c => csv.LoadColumn(c));
			var all = AllSkosColumns.ToDictionary(c => c, c => csv.LoadColumn(c));

			int mapped = 0, declaredGap = 0, declaredImitation = 0, untouched = 0;
			for (var i = 0; i < pks.Count; i++)
			{
				var hasRef = refs.Values.Any(col => !string.IsNullOrWhiteSpace(col[i]));
				if (hasRef)
					mapped++;
				else if (!string.IsNullOrWhiteSpace(all["AIF_skosOther"][i]))
					declaredGap++;
				else if (!string.IsNullOrWhiteSpace(all["AIF_imitatedScheme"][i]))
					declaredImitation++;
				else if (all.Values.All(col => string.IsNullOrWhiteSpace(col[i])))
					untouched++;
				// else: unclassified shape (e.g. MappingType alone) — deliberately counted in NO
				// bucket; the sum invariant below trips and names the decision it hides.
			}
			return (mapped, declaredGap, declaredImitation, untouched, pks.Count);
		}

		[Fact]
		public void Census_FourBuckets_MustSumToCorpusRowCount()
		{
			var (mapped, declaredGap, declaredImitation, untouched, rows) = Census();
			var sum = mapped + declaredGap + declaredImitation + untouched;

			rows.Should().BeGreaterThanOrEqualTo(CorpusFloor,
				"the Fallacies corpus carries {0} data rows on master 170acf30; a smaller count means data loss",
				CorpusFloor);
			sum.Should().Be(rows,
				"every row must land in exactly one census bucket (mapped={0}, declared-gap={1}, " +
				"declared-imitation={2}, untouched={3}, rows={4}). " +
				"A shortfall means rows carry a skos shape no bucket describes — classify it or serialize it away; " +
				"do not widen a bucket to make the sum pass",
				mapped, declaredGap, declaredImitation, untouched, rows);
		}

		[Fact]
		public void Census_MappedRows_MustStayAtOrAboveFloor()
		{
			var (mapped, _, _, _, _) = Census();

			mapped.Should().BeGreaterThanOrEqualTo(MappedFloor,
				"mapped rows feed the attack-only stop-gate (116 on master 170acf30); a legitimate write " +
				"only raises this count — a fall means rows lost their refs");
		}
	}
}
