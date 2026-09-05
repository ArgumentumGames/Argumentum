using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// #1247 organ, axis 2 — "une cible ne peut pas devenir un hub".
    /// Owner criterion (GO 02/09/2026): « si quelque chose devient systématique ou si un nœud
    /// devient un hub trop gros, c'est que la pertinence est noyée par un problème de scope ».
    /// This organ measures the HUB half: a crosslink target that accumulates a large number of
    /// incoming links stops being informative — it becomes a hub whose very centrality drowns
    /// the relevance (the #1247 problem: 359 of the 1255 rendered relations point at the 7
    /// family roots, which are the 7 most-linked targets).
    ///
    /// Measured on master ae84c91c (re-derived independently on the PR branch, identical):
    /// the sorted incoming-link counts of targets are:
    ///
    ///   family roots: 1: 110 · 2: 85 · 5: 72 · 4: 38 · 7: 31 · 6: 14 · 3: 9
    ///   then the 8th target (2.3.3.4.2.2.4 "Inclination"): 6
    ///
    /// The cliff is empty between 9 and 6 — there is no continuum, there is a CLIFF between
    /// the family roots and everything else. That is the empirical signature of the scope
    /// problem the owner describes, and it is where the threshold must sit: a target with
    /// ≥ 8 incoming links is a hub. Choosing 8 (strictly inside the empty band) is measured,
    /// not invented — the test documents that the band is empty.
    ///
    /// The organ applies to every target, including the family roots themselves: the
    /// retargeting pass (#1247 PR-B) must bring each root under the threshold by requalifying
    /// its incoming links to the precise sub-node of the mechanism (or dropping them), and a
    /// retarget that merely moves the hub onto a deep sub-node must be caught by the same
    /// organ — a hub is a hub wherever it sits.
    ///
    /// Metric: incoming resolvable links per target — every target receives a count from each
    /// source whose cell names it; self-links and unresolvable targets are dropped (resolver
    /// semantics, identical to CrossLinkSystematicVerbTests so the two axes measure the same
    /// corpus without overlap). A target crossing the threshold is a hub wherever it sits — a
    /// retarget that moves the hub onto a deep sub-node must trip the same organ.
    ///
    /// Inverse control (mandatory, #1112): HubGuard_FiresOnAFabricatedHubTarget feeds the SAME
    /// engine a synthetic corpus where one target receives 9 incoming links (must be flagged)
    /// and another receives 6 (must not) — a guard that cannot fire is the #1046 no-op.
    /// </summary>
    public class CrossLinkHubTargetTests
    {
        private const int HubThreshold = 8;

        private static readonly string[] CrossLinkVerbs =
        {
            "PredatesOn", "Denounces", "Leverages", "Allows", "Opposes", "Inverts", "Mirrors", "IsRelatedTo",
        };

        [Fact]
        public void CorpusCrossLinkTargets_DoNotBecomeHubs()
        {
            var incoming = ComputeIncomingPerTarget(LoadCorpus());

            var hubs = FlagHubTargets(incoming);
            hubs.Should().BeEmpty(
                "a crosslink target with ≥ {0} incoming resolvable links is a hub: the owner criterion (#1247, " +
                "02/09: « si … un nœud devient un hub trop gros, c'est que la pertinence est noyée par un " +
                "problème de scope ») requires the retargeting pass to requalify every such target to its " +
                "precise mechanism sub-node or drop the link. The measured cliff is empty between 9 (the 7th " +
                "target, a family root) and 6 (the 8th target, 2.3.3.4.2.2.4 Inclination) — the threshold sits " +
                "in that empty band. Incoming counts:{1}",
                HubThreshold, FormatIncoming(incoming));
        }

        [Fact]
        public void HubGuard_FiresOnAFabricatedHubTarget()
        {
            // Inverse control (#1112): the same engine must flag a fabricated target that receives
            // 9 incoming links and NOT flag one that receives 6 — a guard that cannot fire is the
            // #1046 no-op.
            var corpus = Enumerable.Range(0, 9)
                .Select(i => ($"s.{i + 1}", "Leverages", "9.9.9"))   // 9 source rows -> hub target
                .Concat(Enumerable.Range(0, 6)
                    .Select(i => ($"s.{i + 1}", "IsRelatedTo", "8.8.8"))) // 6 rows -> under-threshold target
                .Concat(new[]
                {
                    ("9.9.9", "Mirrors", ""),   // declares target path '9.9.9' as a known node
                    ("8.8.8", "Mirrors", ""),   // declares target path '8.8.8' as a known node
                })
                .ToList();

            var flagged = FlagHubTargets(ComputeIncomingPerTarget(corpus));

            flagged.Should().Equal(new[] { "9.9.9 (9 incoming)" },
                "the fabricated corpus gives '9.9.9' 9 incoming resolvable links (≥ {0} = hub, must be flagged) " +
                "while '8.8.8' stays at 6 (< {0} = must NOT be flagged) — the guard must discriminate, not flag " +
                "everything nor nothing",
                HubThreshold);
        }

        /// <summary>
        /// The organ's engine, shared by the corpus assertion and the inverse control.
        /// </summary>
        internal static IReadOnlyDictionary<string, int> ComputeIncomingPerTarget(
            IEnumerable<(string Path, string Verb, string Targets)> corpus)
        {
            var rows = corpus.ToList();
            var knownPaths = new HashSet<string>(
                rows.Select(r => r.Path.Trim()).Where(p => p.Length > 0), StringComparer.Ordinal);

            var incoming = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var (path, _, targets) in rows)
            {
                var source = path.Trim();
                foreach (var target in targets.Split(';').Select(t => t.Trim()).Where(t => t.Length > 0))
                {
                    if (knownPaths.Contains(target) && !string.Equals(target, source, StringComparison.Ordinal))
                    {
                        incoming[target] = incoming.TryGetValue(target, out var c) ? c + 1 : 1;
                    }
                }
            }
            return incoming;
        }

        internal static IReadOnlyList<string> FlagHubTargets(IReadOnlyDictionary<string, int> incoming)
        {
            return incoming.Where(kv => kv.Value >= HubThreshold)
                .Select(kv => $"{kv.Key} ({kv.Value} incoming)")
                .OrderByDescending(s => s, StringComparer.Ordinal)
                .ToList();
        }

        private static string FormatIncoming(IReadOnlyDictionary<string, int> incoming)
        {
            var builder = new System.Text.StringBuilder();
            foreach (var kv in incoming.OrderByDescending(kv => kv.Value).Take(20))
            {
                var label = kv.Key;
                builder.Append($"\n  {label,-24} {kv.Value}");
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