using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
	/// <summary>
	/// THREE-STATE CENSUS of the Virtues' AIF attack columns (#989 architecture B).
	/// </summary>
	/// <remarks>
	/// <para><b>Why this organ exists.</b> The #989 write (ai-01 arbitration
	/// <c>msg-20260831T172136-w5x7gm</c>, engraved <c>issues/989#issuecomment-5479634124</c>)
	/// replaces the convention-filled default column (206/13/3, ~93% default) with the measured
	/// attack types of the opposed fallacies — strict majority wins, and a tie is a <i>declared
	/// gap</i>: <c>AIF_attackType</c>/<c>AIF_attackedNode</c> left empty and the reason written in
	/// <c>AIF_skosOther</c> (the corpus note-only idiom). The GO's condition is explicit:
	/// <b>a declared gap must not be indistinguishable from an untouched cell</b>. This census is
	/// that condition, executable — the Virtues-side sibling of
	/// <see cref="FallacySkosCensusTests"/> (#1239).</para>
	/// <para><b>Perimeter.</b> The 222 rows with a non-empty <c>crossLink_Opposes</c> — exactly the
	/// write's perimeter (pk 0, the root <i>Argument valable</i>, is out: it opposes nothing).
	/// On master the same 222 rows are those carrying <c>AIF_skosDirectRef</c>; the two predicates
	/// coincide, the census pins the one the arbitration named.</para>
	/// <para><b>The three states</b> (arbitration criteria):</para>
	/// <list type="table">
	/// <item><term>mapped</term><description><c>AIF_attackType</c> non-empty — a derived value from
	/// the opposed fallacies' measured attack types. Measured 142 after the write (undermine 74,
	/// undercut 63, rebut 5).</description></item>
	/// <item><term>declared-gap</term><description><c>AIF_skosOther</c> non-empty AND
	/// <c>AIF_attackType</c> empty — an honest tie, declared. Measured 80 (every one an exact 1-1
	/// tie among the opposed values).</description></item>
	/// <item><term>untouched</term><description>both empty — a perimeter row the write missed.
	/// Measured 0; anything else is red, because that row's emptiness is indistinguishable from
	/// non-treatment.</description></item>
	/// </list>
	/// <para><b>Floors, never equalities</b> (except the arbitration-pinned ones): the perimeter
	/// (222) and untouched (0) are exact per the GO — they are the anti-vacuity floor
	/// « mappées + lacunes déclarées = 222, chaque ligne porte soit une valeur dérivée soit une
	/// déclaration de lacune ». The mapped count is a floor so a future legitimate revision
	/// (more measured fallacy values dissolving ties) raises it green-ward without a test edit;
	/// only a regression goes red.</para>
	/// <para><b>Pair consistency</b>: every non-empty attack type must carry its deterministic
	/// node (undermine→I-node, undercut→RA-node, rebut→CA-node — the corpus vocabulary, also the
	/// ratified #707§4 contract on the fallacies side). Guards a partial write that would change
	/// one column without the other.</para>
	/// </remarks>
	public class VirtueAifCensusTests
	{
		private const int PerimeterCount = 222;
		private const int MappedFloor = 142;

		private static readonly IReadOnlyDictionary<string, string> NodeByType =
			new Dictionary<string, string>
			{
				["undermine"] = "I-node",
				["undercut"] = "RA-node",
				["rebut"] = "CA-node",
			};

		private static string VirtuesCsv => Path.Combine(TestRepoRoot.Find(),
			"Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static (int Perimeter, int Mapped, int DeclaredGap, int Untouched, int Rows) Census()
		{
			var csv = new HarvestCardIdsCsv(VirtuesCsv);
			var pks = csv.LoadColumn("pk");
			var opposes = csv.LoadColumn("crossLink_Opposes");
			var attackType = csv.LoadColumn("AIF_attackType");
			var attackedNode = csv.LoadColumn("AIF_attackedNode");
			var skosOther = csv.LoadColumn("AIF_skosOther");

			int perimeter = 0, mapped = 0, declaredGap = 0, untouched = 0;
			for (var i = 0; i < pks.Count; i++)
			{
				var inPerimeter = !string.IsNullOrWhiteSpace(opposes[i]);
				if (!inPerimeter) continue;
				perimeter++;

				var hasType = !string.IsNullOrWhiteSpace(attackType[i]);
				var hasNote = !string.IsNullOrWhiteSpace(skosOther[i]);
				if (hasType)
					mapped++;
				else if (hasNote)
					declaredGap++;
				else
					untouched++;
			}
			return (perimeter, mapped, declaredGap, untouched, pks.Count);
		}

		[Fact]
		public void Census_Perimeter_IsThe222RowsWithOpposes()
		{
			var (perimeter, _, _, _, _) = Census();

			perimeter.Should().Be(PerimeterCount,
				"the #989 perimeter is the 222 rows carrying crossLink_Opposes (ai-01 GO msg-20260831T172136-w5x7gm); " +
				"a different count means the corpus or the column changed — re-derive, do not rebalance the organ");
		}

		[Fact]
		public void Census_EveryPerimeterRow_IsMappedOrDeclaredGap()
		{
			var (perimeter, mapped, declaredGap, untouched, _) = Census();

			(mapped + declaredGap + untouched).Should().Be(perimeter,
				"every perimeter row must land in exactly one census bucket (mapped={0}, declared-gap={1}, untouched={2}, perimeter={3}). " +
				"A shortfall means a row carries a shape no bucket describes — classify it or serialize it away; " +
				"do not widen a bucket to make the sum pass",
				mapped, declaredGap, untouched, perimeter);

			untouched.Should().Be(0,
				"the anti-vacuity floor: mapped ({0}) + declared gaps ({1}) must equal the perimeter ({2}). " +
				"An empty, undeclared cell is not a gap — it is non-treatment, indistinguishable from a missing write. " +
				"Either derive the value or declare the gap in AIF_skosOther",
				mapped, declaredGap, perimeter);
		}

		[Fact]
		public void Census_MappedRows_MustStayAtOrAboveFloor()
		{
			var (perimeter, mapped, declaredGap, _, _) = Census();

			mapped.Should().BeGreaterThanOrEqualTo(MappedFloor,
				"142 rows hold a strict-majority value on master after the #989 architecture B write " +
				"(undermine 74 / undercut 63 / rebut 5 against {0} declared gaps); a future revision can only raise " +
				"this floor by dissolving ties with newly measured fallacy values — a fall means rows lost their " +
				"derived values", declaredGap);
		}

		[Fact]
		public void Census_AttackTypeAndNode_MustStayPaired()
		{
			var csv = new HarvestCardIdsCsv(VirtuesCsv);
			var pks = csv.LoadColumn("pk");
			var opposes = csv.LoadColumn("crossLink_Opposes");
			var attackType = csv.LoadColumn("AIF_attackType");
			var attackedNode = csv.LoadColumn("AIF_attackedNode");

			var offenders = new List<string>();
			for (var i = 0; i < pks.Count; i++)
			{
				if (string.IsNullOrWhiteSpace(opposes[i])) continue;
				var type = attackType[i].Trim();
				if (type.Length == 0) continue; // declared gap — both cells empty, checked below

				if (!NodeByType.TryGetValue(type, out var expectedNode))
				{
					offenders.Add($"pk {pks[i]}: unknown attack type '{type}'");
					continue;
				}
				if (!string.Equals(attackedNode[i].Trim(), expectedNode, StringComparison.Ordinal))
					offenders.Add($"pk {pks[i]}: '{type}' must carry '{expectedNode}', found '{attackedNode[i].Trim()}'");
			}

			offenders.Should().BeEmpty(
				"attackedNode is deterministic from attackType (undermine→I-node, undercut→RA-node, rebut→CA-node); " +
				"a violation means a partial or hand-edited write");
		}

		[Fact]
		public void Census_OutOfPerimeterRoot_MustStayEmpty()
		{
			var csv = new HarvestCardIdsCsv(VirtuesCsv);
			var pks = csv.LoadColumn("pk");
			var opposes = csv.LoadColumn("crossLink_Opposes");
			var attackType = csv.LoadColumn("AIF_attackType");
			var attackedNode = csv.LoadColumn("AIF_attackedNode");
			var skosOther = csv.LoadColumn("AIF_skosOther");

			var offenders = new List<string>();
			for (var i = 0; i < pks.Count; i++)
			{
				if (!string.IsNullOrWhiteSpace(opposes[i])) continue;
				if (!string.IsNullOrWhiteSpace(attackType[i]) || !string.IsNullOrWhiteSpace(attackedNode[i]) ||
				    !string.IsNullOrWhiteSpace(skosOther[i]))
					offenders.Add($"pk {pks[i]}");
			}

			offenders.Should().BeEmpty(
				"rows outside the perimeter (the root 'Argument valable' opposes nothing) must carry no attack " +
				"value, no node and no note — a value there would be derived from nothing");
		}
	}
}
