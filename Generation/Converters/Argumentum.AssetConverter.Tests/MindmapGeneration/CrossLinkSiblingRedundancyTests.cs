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
    /// #1247 organ, axis 1 (CORRECTED per ai-01 arbitration 03/09 — replaces the numeric-share
    /// organ of the first PR-A iteration, rejected with the branch state it produced).
    ///
    /// The rejected organ gave Mirrors a 99% sibling share and Inverts 100% — twin scores,
    /// opposite verdicts: Mirrors between siblings says "these two resemble each other", which
    /// the shared parent already encodes, while Inverts between siblings carries a POLARITY the
    /// tree cannot encode (Définition trop large >< Définition trop restrictive · Archaïsme ><
    /// Anachronisme · Erreur inverse du parieur >< Erreur du parieur). A score that does not
    /// separate two cases requiring opposite decisions does not measure what it is used to
    /// decide. The numeric threshold and its band are therefore GONE; the replacement is a
    /// semantic partition taken BEFORE any measurement:
    ///
    ///  - RESEMBLANCE verbs — Mirrors, IsRelatedTo: a sibling link here restates the tree
    ///    (proximity = relatedness), so it is redundant by definition ⇒ ZERO tolerance,
    ///    absolute, no threshold to defend.
    ///  - DIRECTION/POLARITY verbs — Inverts, Opposes, Allows, Leverages, PredatesOn,
    ///    Denounces: they assert a direction or a polarity that co-parentage does NOT imply;
    ///    co-parentage is the precondition that makes the relation stateable, not what makes
    ///    it redundant ⇒ OUT OF SCOPE, never flagged, whatever their sibling share.
    ///
    /// Expected state on this branch (final corpus of the #1247 delivery lineage): Mirrors
    /// siblings = 0 (358 removed), Inverts siblings = 41 (RESTORED — the first PR-A iteration
    /// wrongly removed them), IsRelatedTo siblings = 0 (171 removed by the tertiary pass).
    /// Until the tertiary pass landed, this fact was RED BY DESIGN on IsRelatedTo (same pattern
    /// as the hub organ staying red until its final tranche); the lineage's end state is 0 for
    /// every resemblance verb.
    ///
    /// Inverse control (mandatory, #1112): a fabricated corpus carrying sibling links under a
    /// resemblance verb AND under direction/polarity verbs must flag ONLY the resemblance
    /// verbs — on the pre-PR-A corpus this is the required "red on Mirrors, green on Inverts".
    /// </summary>
    public class CrossLinkSiblingRedundancyTests
    {
        private static readonly string[] ResemblanceVerbs = { "Mirrors", "IsRelatedTo" };

        private static readonly string[] CrossLinkVerbs =
        {
            "PredatesOn", "Denounces", "Leverages", "Allows", "Opposes", "Inverts", "Mirrors", "IsRelatedTo",
        };

        [Fact]
        public void ResemblanceVerbs_CarryNoSiblingLinks()
        {
            var counts = ComputeSiblingCounts(LoadCorpus());
            var flagged = FlagResemblanceVerbs(counts);

            flagged.Should().BeEmpty(
                "a sibling link under a resemblance verb ({0}) restates the shared parent and carries no " +
                "information the taxonomy does not already encode, while the direction/polarity verbs are " +
                "out of scope by the #1247 semantic partition (ai-01, 03/09: twin sibling shares hid " +
                "opposite verdicts — Mirrors 99% removed, Inverts 100% restored). Per-verb sibling counts:{1}",
                string.Join("/", ResemblanceVerbs), FormatCounts(counts));
        }

        [Fact]
        public void PartitionGuard_FlagsResemblanceOnly_NotDirectionOrPolarity()
        {
            // Inverse control (#1112): the same engine, fed a corpus where every verb carries a
            // sibling link, must flag exactly the resemblance verbs and leave Inverts and the
            // other direction/polarity verbs alone — this is the "red on Mirrors, green on
            // Inverts" discrimination the rejected single-score organ could not make. Every
            // target is a declared path of the fabricated corpus: an unresolvable target would
            // be dropped before the sibling test and silently change the counts under assertion.
            var corpus = new List<(string Path, string Verb, string Targets)>
            {
                ("1.1", "Mirrors", "1.2"),        // resemblance sibling — must be flagged
                ("1.2", "Mirrors", "1.1"),        // resemblance sibling — must be flagged
                ("1.1.1", "IsRelatedTo", "1.1.2"),// resemblance sibling — must be flagged
                ("1.1.1", "Inverts", "1.1.2"),    // polarity sibling — must be LEFT ALONE
                ("1.1.1", "Opposes", "1.1.2"),    // polarity sibling — must be LEFT ALONE
                ("1.1.1", "Leverages", "1.1.2"),  // direction sibling — must be LEFT ALONE
                ("1.1.2", "Denounces", "5.1"),    // non-sibling, and declares 1.1.2 as a path
                                                  // (an undeclared target would be dropped as
                                                  // unresolvable and silently defuse the guard)
            };

            var flagged = FlagResemblanceVerbs(ComputeSiblingCounts(corpus));

            flagged.Should().Equal(new[] { "IsRelatedTo", "Mirrors" },
                "the fabricated corpus gives every verb exactly one sibling link; the partition must " +
                "condemn the resemblance verbs (which restate the tree) and spare Inverts/Opposes/" +
                "Leverages (whose sibling links carry polarity or direction the tree cannot encode) — " +
                "a guard that flags them all, or none, is the single-score organ already taken in default");
        }

        /// <summary>
        /// The organ's engine, shared by the corpus assertion and the inverse control so the
        /// guard that fires on the fabricated corpus is provably the same guard the corpus runs.
        /// </summary>
        internal static IReadOnlyDictionary<string, (int Total, int Sibling)> ComputeSiblingCounts(
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

        internal static IReadOnlyList<string> FlagResemblanceVerbs(
            IReadOnlyDictionary<string, (int Total, int Sibling)> counts)
        {
            return ResemblanceVerbs
                .Where(v => counts[v].Sibling > 0)
                .OrderBy(v => v, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>Path minus its last dotted segment; null for family roots (no encoded parent).</summary>
        private static string? Parent(string path)
        {
            var lastDot = path.LastIndexOf('.');
            return lastDot < 0 ? null : path.Substring(0, lastDot);
        }

        private static string FormatCounts(IReadOnlyDictionary<string, (int Total, int Sibling)> counts)
        {
            var builder = new StringBuilder();
            foreach (var verb in CrossLinkVerbs)
            {
                var (total, sibling) = counts[verb];
                var scope = ResemblanceVerbs.Contains(verb) ? "<= IN SCOPE" : "   out of scope";
                builder.Append($"\n  {verb,-12} {sibling,3}/{total,-3} {scope}");
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
