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
    /// #1181 organ: the transverse cross-links of the corpus (crossLink_* columns, 8 verbs) must be
    /// VISIBLE on the shipped Fallacies mindmaps. Before the fix, all 41 shipped SVGs carried
    /// 0 cross-link connector while the corpus holds over a thousand declared links — the Arrowlink
    /// block was unreachable (enum vocabulary disjoint from the corpus, CrossLinks never assigned).
    /// Measured signature: 0 stroked path in any verb color (Batik exports carry no marker-end at
    /// all — see CountArrows).
    ///
    /// The expectation is DERIVED from the taxonomy CSV at test time with the resolver's own
    /// semantics (split on ';', trim, drop unresolvable, drop self-links) — never a hardcoded
    /// number, and never a mere "> 0": a decay from ~1230 arrows to 3 must fail. Cross-links are
    /// keyed by taxonomy PATH, which is language-independent, so EVERY Fallacies map — each
    /// language, each variant (original / links / content / cards) — must carry EXACTLY the corpus
    /// count. The Virtues maps carry none (the Virtues taxonomy has no crossLink columns) and are
    /// deliberately not asserted here.
    /// </summary>
    public class CrossLinkArrowCountTests
    {
        private static readonly string[] Languages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

        private static readonly (string Verb, bool Symmetric)[] CrossLinkVerbs =
        {
            ("predatesOn", false),
            ("denounces", false),
            ("leverages", false),
            ("allows", false),
            ("opposes", true),
            ("inverts", true),
            ("mirrors", true),
            ("isRelatedTo", true),
        };

        [Fact]
        public void ShippedFallaciesMindmaps_CarryExactlyTheCorpusCrossLinkCount()
        {
            var expected = CountResolvableCorpusLinks();
            expected.Should().BeGreaterThan(0,
                "the taxonomy must declare at least one resolvable crossLink_* target, otherwise this organ " +
                "degenerates to 0 == 0 (the #1046 no-op guard)");

            foreach (var svgPath in EnumerateFallaciesSvgs())
            {
                var arrowCount = CountArrows(svgPath);
                arrowCount.Should().Be(expected,
                    "map '{0}' must draw one arrow per resolvable corpus cross-link. Cross-links are keyed by " +
                    "taxonomy path (language-independent), so every Fallacies map carries the same count. " +
                    "Corpus says {1}. A LOWER count means arrows were lost (generation or post-processing " +
                    "regression — the pre-#1181 tree carried 0); a HIGHER count means duplicate or spurious " +
                    "arrows (e.g. symmetric verbs double-drawn).",
                    Path.GetFileName(svgPath), expected);
            }
        }

        private static IEnumerable<string> EnumerateFallaciesSvgs()
        {
            var mindmapsRoot = Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies", "Mindmaps");
            foreach (var language in Languages)
            {
                foreach (var pattern in new[]
                         {
                             $"Fallacies_{language}.svg",
                             $"Fallacies_{language}.links.svg",
                             $"Fallacies_{language}.content.svg",
                         })
                {
                    var path = Path.Combine(mindmapsRoot, language, pattern);
                    File.Exists(path).Should().BeTrue(
                        $"the shipped mindmap '{pattern}' must exist for language '{language}' — a missing file " +
                        "must fail the organ, not slip past it");
                    yield return path;
                }
            }

            // FR-only cards variant (FallacyMindMapCreatorConfig: FR by design, no Translations)
            var cardsSvg = Path.Combine(mindmapsRoot, "fr", "Argumentum_Fallacies_MindMap_cards_fr.svg");
            File.Exists(cardsSvg).Should().BeTrue("the FR cards mindmap is part of the shipped inventory (41 SVGs)");
            yield return cardsSvg;
        }

        /// <summary>
        /// Counts the cross-link connectors FreeMind/Batik draws in its SVG export, one stroked
        /// path per arrowlink, colored by verb. FreeMind exports via the Batik Graphics2D
        /// generator, which never emits semantic marker-end references — it flattens every shape
        /// (including arrowheads) into generic paths carrying a stroke color. The verb palette
        /// (FallacyMindMapDocumentConfig.CrossLinkColors) is therefore the only stable signature:
        /// each color must also stay countable, i.e. distinct from every other stroke color the
        /// export can emit (tree edges carry the bright family colors; Batik's default is black —
        /// which is exactly why Denounces cannot be #000000, it would merge with the default).
        /// The rgb() strings are derived from GetCrossLinkColor so the organ tracks the palette
        /// table automatically.
        /// </summary>
        private static int CountArrows(string svgPath)
        {
            var svg = File.ReadAllText(svgPath);
            var count = 0;
            foreach (var (verb, _) in CrossLinkVerbs)
            {
                var pascal = char.ToUpperInvariant(verb[0]) + verb.Substring(1);
                var hex = Argumentum.AssetConverter.Mindmapper.FallacyMindMapDocumentConfig
                    .GetCrossLinkColor(System.Enum.Parse<Argumentum.AssetConverter.Mindmapper.CrossLink>(pascal));
                var rgb = string.Join(",",
                    Convert.ToInt32(hex.Substring(1, 2), 16),
                    Convert.ToInt32(hex.Substring(3, 2), 16),
                    Convert.ToInt32(hex.Substring(5, 2), 16));
                var needle = $"stroke=\"rgb({rgb})\"";
                var index = 0;
                while ((index = svg.IndexOf(needle, index, StringComparison.Ordinal)) >= 0)
                {
                    count++;
                    index += needle.Length;
                }
            }
            return count;
        }

        /// <summary>
        /// Recomputes from the taxonomy CSV exactly what the mindmap resolver will draw, with the
        /// resolver's own semantics (split on ';', trim, drop unresolvable, drop self-links).
        /// Mirrors the OWL corpus counter of OwlE2EGenerationValidationTests (#1182).
        /// </summary>
        private static int CountResolvableCorpusLinks()
        {
            var csvPath = Path.Combine(TestRepoRoot.Find(), "Cards", "Fallacies",
                "Argumentum Fallacies - Taxonomy.csv");
            File.Exists(csvPath).Should().BeTrue(
                "the taxonomy CSV is the source both the mindmaps and this expectation are derived from");

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
            var knownPaths = new HashSet<string>(
                rows.Where(r => pathCol < r.Length).Select(r => r[pathCol].Trim()),
                StringComparer.Ordinal);

            var resolvable = 0;
            foreach (var (verb, _) in CrossLinkVerbs)
            {
                var col = Col("crossLink_" + char.ToUpperInvariant(verb[0]) + verb.Substring(1));
                foreach (var row in rows)
                {
                    if (col >= row.Length || string.IsNullOrWhiteSpace(row[col]))
                    {
                        continue;
                    }
                    var self = pathCol < row.Length ? row[pathCol].Trim() : string.Empty;
                    resolvable += row[col].Split(';').Select(x => x.Trim()).Where(x => x.Length > 0)
                        .Count(target => knownPaths.Contains(target)
                                         && !string.Equals(target, self, StringComparison.Ordinal));
                }
            }
            return resolvable;
        }
    }
}
