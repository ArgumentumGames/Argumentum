using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// #1247 organ, axis 1 — "un verbe ne peut pas se réduire à redire l'arbre".
    /// Owner criterion (GO 02/09/2026): « si quelque chose devient systématique ou si un nœud
    /// devient un hub trop gros, c'est que la pertinence est noyée par un problème de scope ».
    /// This organ measures the SYSTEMATIC half: a crossLink_* verb whose resolvable links are
    /// (near-)entirely sibling-to-sibling links states nothing the tree does not already encode —
    /// sibling proximity IS the tree. Measured on master ae84c91c (2026-09-02, re-derived
    /// independently on the PR branch, identical), the sorted sibling shares are:
    ///
    ///   Denounces 0/9 = 0.0% · PredatesOn 4/31 = 12.9% · Leverages 204/402 = 50.7% ·
    ///   IsRelatedTo 171/321 = 53.3% · Allows 50/66 = 75.8% · Opposes 22/25 = 88.0% ·
    ///   Mirrors 358/360 = 99.4% · Inverts 41/41 = 100%
    ///
    /// TWO empty bands separate those points, and each carries a different organ:
    ///  - (12.9%, 50.7%): the END-STATE band of the triage pass (#1247 tertiary). Leverages,
    ///    IsRelatedTo, Allows and Opposes sit above it BY DESIGN until the case-by-case triage
    ///    of their ~451 sibling links (owner: « dans certains cas c'est pertinent »). A threshold
    ///    there would hold this organ red long after this PR — it is NOT this PR's threshold.
    ///  - (88.0%, 99.4%): the band separating "systematic in the exact sense of the criterion"
    ///    (Mirrors 99.4%, Inverts 100% — the verb has all but ceased to say anything else) from
    ///    "to be triaged" (≤ 88.0%). THIS organ's threshold (95%) sits in the middle of it, so
    ///    the removal of the 399 Mirrors/Inverts sibling links turns the organ green in the same
    ///    PR that performs it (#1112: the organ is written first, its red recorded in the PR
    ///    body, and the surgery lands in the same PR — never a green-by-construction test, and
    ///    never a conditional Skip).
    ///
    /// Sibling = same PARENT, and the parent must be a real node: family roots (paths with no
    /// '.', e.g. '2') have no encoded parent, so a root→root link is NOT a sibling link — it
    /// belongs to the hub axis (#1247 PR-B, root-target retargeting). Counting it here would
    /// let the hub surgery shift this organ's numbers. Cross-check: with this definition the
    /// corpus partitions into 850 sibling / 359 root-target / 46 genuinely transverse links —
    /// exactly the #1247 census.
    ///
    /// Inverse control (mandatory, #1112): SiblingShareGuard_FiresOnAFabricatedSystematicVerb
    /// feeds the SAME engine a synthetic corpus where one verb is 100% sibling — the guard must
    /// flag it, otherwise this organ is the #1046 no-op.
    /// </summary>
    public class CrossLinkSystematicVerbTests
    {
        private const double SystematicShareThreshold = 0.95;

        private static readonly string[] CrossLinkVerbs =
        {
            "PredatesOn", "Denounces", "Leverages", "Allows", "Opposes", "Inverts", "Mirrors", "IsRelatedTo",
        };

        [Fact]
        public void CorpusCrossLinkVerbs_DoNotReduceToRestatingTheTree()
        {
            var shares = ComputeSiblingShares(LoadCorpus());
            var systematic = FlagSystematicVerbs(shares);

            systematic.Should().BeEmpty(
                "a crossLink_* verb whose resolvable links are ≥ {0:P0} sibling links restates the tree and " +
                "carries no information the taxonomy does not already encode (owner criterion #1247, 02/09: " +
                "« si quelque chose devient systématique … c'est que la pertinence est noyée par un problème " +
                "de scope »). Per-verb sibling shares:{1}",
                SystematicShareThreshold, FormatShares(shares));
        }

        [Fact]
        public void SiblingShareGuard_FiresOnAFabricatedSystematicVerb()
        {
            // Inverse control (#1112): the same engine, fed a synthetic corpus where 'Mirrors'
            // is 100% sibling while the other verbs stay mixed or transverse, must flag exactly
            // the systematic one — a guard that cannot fire is the #1046 no-op. Every target is
            // a declared path of the fabricated corpus: an unresolvable target would be dropped
            // before the sibling test and silently change the shares under assertion.
            var corpus = new List<(string Path, string Verb, string Targets)>
            {
                ("1.1", "Denounces", "5.1"),             // cross-family: healthy, 0% sibling
                ("5.1", "Opposes", "1.1"),               // transverse: 0% sibling
                ("1.1.1", "Mirrors", "1.1.2"),           // sibling
                ("1.1.2", "Mirrors", "1.1.1"),           // sibling — 'Mirrors' is 2/2 = 100%
                ("1.1.1", "Leverages", "1.1.2;5.1"),     // one sibling + one cross-family = 50%
            };

            var flagged = FlagSystematicVerbs(ComputeSiblingShares(corpus));

            flagged.Should().Equal(new[] { "Mirrors" },
                "the fabricated corpus makes 'Mirrors' 2/2 sibling (systematic) while 'Leverages' is 1/2, " +
                "'Denounces' 0/1 and 'Opposes' 0/1 — the guard must discriminate, not flag everything " +
                "nor nothing");
        }

        /// <summary>
        /// The organ's engine, shared by the corpus assertion and the inverse control so the
        /// guard that fires on the fabricated corpus is provably the same guard the corpus runs.
        /// </summary>
        internal static IReadOnlyDictionary<string, (int Total, int Sibling)> ComputeSiblingShares(
            IEnumerable<(string Path, string Verb, string Targets)> corpus)
        {
            var rows = corpus.ToList();
            var knownPaths = new HashSet<string>(
                rows.Select(r => r.Path.Trim()).Where(p => p.Length > 0), StringComparer.Ordinal);

            var result = new Dictionary<string, (int Total, int Sibling)>();
            foreach (var verb in CrossLinkVerbs)
            {
                var total = 0;
                var sibling = 0;
                foreach (var (path, rowVerb, targets) in rows)
                {
                    if (!string.Equals(rowVerb, verb, StringComparison.Ordinal)
                        || string.IsNullOrWhiteSpace(targets))
                    {
                        continue;
                    }
                    var source = path.Trim();
                    var sourceParent = Parent(source);
                    foreach (var target in targets.Split(';').Select(t => t.Trim())
                                 .Where(t => t.Length > 0))
                    {
                        if (!knownPaths.Contains(target)
                            || string.Equals(target, source, StringComparison.Ordinal))
                        {
                            continue; // resolver semantics: unresolvable and self-links are never drawn
                        }
                        total++;
                        var targetParent = Parent(target);
                        if (sourceParent != null && targetParent != null
                            && string.Equals(sourceParent, targetParent, StringComparison.Ordinal))
                        {
                            sibling++;
                        }
                    }
                }
                result[verb] = (total, sibling);
            }
            return result;
        }

        internal static IReadOnlyList<string> FlagSystematicVerbs(
            IReadOnlyDictionary<string, (int Total, int Sibling)> shares)
        {
            return shares
                .Where(kv => kv.Value.Total > 0
                             && (double)kv.Value.Sibling / kv.Value.Total >= SystematicShareThreshold)
                .Select(kv => kv.Key)
                .OrderBy(v => v, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>Path minus its last dotted segment; null for family roots (no encoded parent).</summary>
        private static string? Parent(string path)
        {
            var lastDot = path.LastIndexOf('.');
            return lastDot < 0 ? null : path.Substring(0, lastDot);
        }

        private static string FormatShares(IReadOnlyDictionary<string, (int Total, int Sibling)> shares)
        {
            var builder = new StringBuilder();
            foreach (var verb in CrossLinkVerbs)
            {
                var (total, sibling) = shares[verb];
                var share = total > 0 ? (double)sibling / total : 0.0;
                builder.Append($"\n  {verb,-12} {sibling,3}/{total,-3} = {share.ToString("P1", CultureInfo.InvariantCulture),7}");
            }
            return builder.ToString();
        }

        private static IEnumerable<(string Path, string Verb, string Targets)> LoadCorpus()
        {
            var csvPath = Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies",
                "Argumentum Fallacies - Taxonomy.csv");
            File.Exists(csvPath).Should().BeTrue(
                "the taxonomy CSV is the source this organ derives its measurement from");

            using var reader = new StreamReader(csvPath, new System.Text.UTF8Encoding(true));
            using var csv = new CsvReader(reader,
                new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true });
            csv.Read();
            csv.ReadHeader();
            var header = csv.HeaderRecord ?? Array.Empty<string>();

            var rows = new List<string[]>();
            while (csv.Read())
            {
                var row = new string[csv.Parser.Count];
                for (var i = 0; i < csv.Parser.Count; i++)
                {
                    row[i] = csv.GetField(i) ?? string.Empty;
                }
                rows.Add(row);
            }

            int Col(string name)
            {
                var i = Array.FindIndex(header, h => string.Equals(h, name, StringComparison.Ordinal));
                return i < 0
                    ? throw new InvalidOperationException(
                        $"Column '{name}' absent from the taxonomy CSV — renaming a column without the ClassMap " +
                        "breaks the pipeline silently; this test refuses to guess")
                    : i;
            }

            var pathCol = Col("path");
            foreach (var verb in CrossLinkVerbs)
            {
                var col = Col("crossLink_" + verb);
                foreach (var row in rows)
                {
                    yield return (
                        pathCol < row.Length ? row[pathCol] : string.Empty,
                        verb,
                        col < row.Length ? row[col] : string.Empty);
                }
            }
        }
    }
}
