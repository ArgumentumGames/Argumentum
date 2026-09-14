using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
	/// <summary>
	/// STOP-GATE on the Fallacies' attack-only organ (#498, P3 palier 2+).
	/// </summary>
	/// <remarks>
	/// <para><b>Why this organ exists.</b> The #458 pool comment names the trap verbatim: of the 29
	/// attack-only rows measured on <c>d7013df1</c> (2026-09-15, matching the 14/09 census on
	/// <c>#498</c>), <b>four carry a ratified verdict</b> — the vetted cluster docs declare them
	/// « sans scheme AIF natif » and serialize them <c>attack-columns-only</c>, which is exactly
	/// their current state. Re-modeling those four would <b>contradict a ratified verdict</b>, and
	/// a future P3 pass starting from the 29 raw rows instead of the 25 eligible ones (6 carry an
	/// unapplied proposition, 19 have no doc at all) would do precisely that — silently. Until now
	/// that verdict lived only in prose (the #458 comment and the cluster docs): nothing in the
	/// test suite turns red if someone writes a scheme token into PK 690.</para>
	/// <para><b>What is pinned.</b></para>
	/// <list type="bullet">
	/// <item><b>The four ratified anchors, as equalities</b> — the only rows whose end state is
	/// itself the ratified content: all four skos cells empty AND the attack columns carrying the
	/// verdict's exact values. This is the deliberate contrast with <see cref="FallacySkosCensusTests"/>'s
	/// floors: for these four, « final » is the verdict, so any move in either direction reopens an
	/// arbitration rather than riding a legitimate write.</item>
	/// <item><b>The backlog as a ceiling, not an equality</b> — the 25 non-ratified rows are the
	/// P3 palier-2+ backlog; an owner-gated GO write models some of them and the count legitimately
	/// falls without a test edit. An <i>increase</i> means new attack-only rows were serialized
	/// without ratification — red. The floor of 4 is anti-vacuity: a reader or classifier that
	/// stopped matching anything would produce 0, not a healthy backlog.</item>
	/// </list>
	/// <para><b>Mutation controls</b> (test 3): the gate is fed three in-memory mutations — the
	/// exact violations it exists for — and must flag every one, while returning zero violations on
	/// the unmutated corpus. A detector that flagged nothing (tautology) or everything (vacuous)
	/// would fail the same test.</para>
	/// <para><b>Derivation is from the CSV</b>, per column name, through the same encoding-tolerant
	/// reader as the two sibling organs; <see cref="HarvestCardIdsCsv"/> throws on a renamed header,
	/// so a silent empty read cannot happen.</para>
	/// </remarks>
	public class FallacyAttackOnlyStopGateTests
	{
		/// <summary>Attack-only rows measured on master d7013df1 (2026-09-15): 29 = undermine 18 /
		/// undercut 10 / rebut 1. Of those, 4 are ratified final (below) and 25 are the P3 palier-2+
		/// backlog eligible for owner-gated modeling writes.</summary>
		private const int AttackOnlyCeiling = 29;

		/// <summary>Anti-vacuity floor = the four ratified anchors, which the sibling test pins as
		/// present and attack-only. Bump together with <see cref="RatifiedAttackOnlyAnchors"/> if a
		/// fifth verdict is ever ratified.</summary>
		private const int AttackOnlyFloor = 4;

		private static readonly string[] AllSkosColumns =
			{ "AIF_skosDirectRef", "AIF_skosExceptionRef", "AIF_skosOther", "AIF_skosMappingType" };

		/// <summary>The ratified verdicts (vetted cluster docs; anchor serialization merged via
		/// #753/#760). Skos-empty AND these exact attack values ARE the verdict — see remarks.</summary>
		private static readonly (string Pk, string AttackType, string Node, string VerdictDoc)[] RatifiedAttackOnlyAnchors =
		{
			("690",  "undercut", "RA-node", "498-aif-operation-inappropriee-cluster.md"),
			("1282", "rebut",    "CA-node", "498-aif-relativisme-abusif-cluster.md (serialized by #760)"),
			("1345", "undercut", "RA-node", "498-aif-complication-exageree-cluster.md"),
			("1352", "undermine", "I-node", "498-aif-empoisonner-le-puits-cluster.md"),
		};

		private static string FallaciesCsv => Path.Combine(TestRepoRoot.Find(),
			"Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		/// <summary>The corpus columns the gate reads, as mutable lists so the mutation controls
		/// can violate them in memory without touching the file.</summary>
		private sealed class Sheet
		{
			public List<string> Pk = new();
			public Dictionary<string, List<string>> Skos = new();
			public List<string> AttackType = new();
			public List<string> AttackedNode = new();

			public int IndexOfPk(string pk) => Pk.FindIndex(p => p.Trim() == pk);

			public void SetCell(string pk, string column, string value)
			{
				var i = IndexOfPk(pk);
				i.Should().BeGreaterThanOrEqualTo(0, "the mutation control targets PK {0}, which the loaded sheet must contain", pk);
				if (column == "AIF_attackType") AttackType[i] = value;
				else if (column == "AIF_attackedNode") AttackedNode[i] = value;
				else Skos[column][i] = value;
			}
		}

		private static Sheet LoadSheet()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			var sheet = new Sheet { Pk = csv.LoadColumn("PK").ToList() };
			foreach (var c in AllSkosColumns)
			{
				var col = csv.LoadColumn(c).ToList();
				col.Count.Should().Be(sheet.Pk.Count,
					"'{0}' and 'PK' are read from the same file by header name; a count mismatch means a " +
					"header was renamed, which the #497 HARD rules forbid.", c);
				sheet.Skos[c] = col;
			}
			sheet.AttackType = csv.LoadColumn("AIF_attackType").ToList();
			sheet.AttackedNode = csv.LoadColumn("AIF_attackedNode").ToList();
			sheet.AttackType.Count.Should().Be(sheet.Pk.Count);
			sheet.AttackedNode.Count.Should().Be(sheet.Pk.Count);
			return sheet;
		}

		/// <summary>Pure classifier: a row is attack-only when an attack column carries a value and
		/// all four skos cells are empty (#498 census definition, 29/08 arbitration).</summary>
		private static bool IsAttackOnly(Sheet s, int i) =>
			(!string.IsNullOrWhiteSpace(s.AttackType[i]) || !string.IsNullOrWhiteSpace(s.AttackedNode[i]))
			&& s.Skos.Values.All(col => string.IsNullOrWhiteSpace(col[i]));

		private static List<string> AttackOnlyPks(Sheet s) =>
			Enumerable.Range(0, s.Pk.Count).Where(i => IsAttackOnly(s, i))
				.Select(i => s.Pk[i].Trim()).ToList();

		/// <summary>Violations of the four ratified verdicts: skos no longer empty, or attack
		/// columns no longer carrying the exact ratified values.</summary>
		private static List<string> RatifiedViolations(Sheet s) =>
			RatifiedAttackOnlyAnchors.SelectMany(a =>
			{
				var i = s.IndexOfPk(a.Pk);
				if (i < 0) return new List<string> { $"PK {a.Pk}: row not found in the corpus" };
				var v = new List<string>();
				if (!s.Skos.Values.All(col => string.IsNullOrWhiteSpace(col[i])))
					v.Add($"PK {a.Pk}: skos cells are no longer empty — the verdict ({a.VerdictDoc}) ratifies " +
						"« sans scheme AIF natif », attack-columns-only IS the final state");
				if (s.AttackType[i].Trim() != a.AttackType)
					v.Add($"PK {a.Pk}: AIF_attackType is '{s.AttackType[i].Trim()}' — the ratified verdict says '{a.AttackType}' ({a.VerdictDoc})");
				if (s.AttackedNode[i].Trim() != a.Node)
					v.Add($"PK {a.Pk}: AIF_attackedNode is '{s.AttackedNode[i].Trim()}' — the ratified verdict says '{a.Node}' ({a.VerdictDoc})");
				return v;
			}).ToList();

		[Fact]
		public void AttackOnlyBacklog_MustStayWithinTheRatifiedBounds()
		{
			var pks = AttackOnlyPks(LoadSheet());

			pks.Count.Should().BeInRange(AttackOnlyFloor, AttackOnlyCeiling,
				"attack-only measured 29 on master d7013df1 (undermine 18 / undercut 10 / rebut 1): " +
				$"4 are ratified final ({string.Join(", ", RatifiedAttackOnlyAnchors.Select(a => a.Pk))}) and 25 are the " +
				"P3 palier-2+ backlog. An owner-gated modeling write moves the count DOWN without a test edit; " +
				"a count ABOVE the ceiling means new attack-only rows were serialized without ratification, and " +
				"a count BELOW the floor means the reader stopped seeing the corpus (the four ratified rows are " +
				"pinned as present by the sibling test). Current set: " + string.Join(" ", pks.OrderBy(p => p, StringComparer.Ordinal)));
		}

		[Fact]
		public void RatifiedAttackOnlyAnchors_MustKeepTheirVerdictExactly()
		{
			var violations = RatifiedViolations(LoadSheet());

			violations.Should().BeEmpty(
				"these four rows carry a RATIFIED verdict declaring them without a native AIF scheme and " +
				"serializing them attack-columns-only — their current state IS the verdict (cluster docs listed " +
				"per row below; trap named in #458 c.5666260217). Re-modeling them contradicts a ratified " +
				"arbitration: reopen the arbitration BEFORE writing, do not route around this gate. " +
				"Ratified anchors: " + string.Join("; ",
					RatifiedAttackOnlyAnchors.Select(a => $"PK {a.Pk} = {a.AttackType}/{a.Node} [{a.VerdictDoc}]")) +
				". Violations:\n  " + string.Join("\n  ", violations));
		}

		/// <summary>MUTATION CONTROLS — the gate is fed the exact violations it exists for, in
		/// memory, and must catch every one; the unmutated corpus must yield zero. Proves the pins
		/// are not tautological (matching nothing) nor vacuous (flagging everything).</summary>
		[Fact]
		public void MutationControls_TheStopGateCatchesWhatItExistsFor()
		{
			// Positive control: the unmutated corpus violates nothing.
			RatifiedViolations(LoadSheet()).Should().BeEmpty(
				"the mutation controls are only meaningful if the real corpus is clean to begin with");

			// Mutation 1 — the exact trap the gate exists for: a scheme token written into a
			// ratified skos-empty anchor (PK 690). The pin must flag it, even though the row then
			// LEAVES the attack-only set (the ceiling alone would stay green — that is why the
			// four anchors are equalities, not a count).
			var m1 = LoadSheet();
			m1.SetCell("690", "AIF_skosDirectRef", "Logical_Conflict");
			RatifiedViolations(m1).Should().ContainSingle(v => v.StartsWith("PK 690: skos cells"),
				"injecting a scheme token into a ratified skos-empty anchor is the precise contradiction " +
				"of the verdict; the gate must say so");
			AttackOnlyPks(m1).Should().NotContain("690",
				"the derivation must also react: a skos-bearing row is no longer attack-only");

			// Mutation 2 — attackType rewritten on 1352. The row STAYS attack-only (a count would
			// not move), only the value pin catches it: proves the equalities check values, not
			// just skos-emptiness.
			var m2 = LoadSheet();
			m2.SetCell("1352", "AIF_attackType", "undercut");
			RatifiedViolations(m2).Should().ContainSingle(v => v.StartsWith("PK 1352: AIF_attackType"),
				"a rewritten attack value on a ratified anchor contradicts the verdict even though the " +
				"attack-only count does not move");

			// Mutation 3 — attack columns blanked on 1282 (a row losing its verdict). The pin must
			// flag it AND the derivation must drop the row: proves the classifier actually reads
			// the attack columns (anti-vacuity in the decreasing direction).
			var m3 = LoadSheet();
			m3.SetCell("1282", "AIF_attackType", "");
			m3.SetCell("1282", "AIF_attackedNode", "");
			RatifiedViolations(m3).Should().Contain(v => v.StartsWith("PK 1282: AIF_attackType"),
				"blanking the attack columns removes the serialized verdict itself — the loudest violation");
			AttackOnlyPks(m3).Should().NotContain("1282",
				"a row with no attack value is not attack-only, whatever its skos state");
		}
	}
}
