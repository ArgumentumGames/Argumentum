using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter.Mindmapper
{
    /// <summary>
    /// #2190 (voie B) — one deterministic, globally unique TINT per cross-link, written into the
    /// .mm at generation time so a Batik SVG export can be mapped back to its link exactly.
    ///
    /// WHY TINTS: the shipped maps draw 581 transverse arrows per language, all in 8 verb colors.
    /// #2190 asks which source and target a given drawn arrow connects — and the arrow's identity
    /// is NOT recoverable from the export: measured 07/11/2026 (verdict `.tmp/voies/voie-a-verdict-2026-11-07.md`,
    /// 8/8 languages), the order FreeMind/Batik draws the traits diverges from the .mm arrowlink
    /// order on 369/581 arrows, no alternative traversal rule reproduces it (358-388 divergences
    /// each), and the permutation is byte-identical across languages — a deterministic but opaque
    /// function of the tree. Coloring each arrow with its own tint at .mm generation time makes the
    /// identity travel WITH the drawing: the SVG reader maps each tint back to (verb, source, target),
    /// restores the verb pastel (the palette is unchanged in every shipped artifact) and annotates
    /// the group with data-crosslink-* attributes. "Voie A" (reconstruct the order) is refuted;
    /// this is "voie B", arbitrated in #2190.
    ///
    /// TINT GEOMETRY (measured constraints, not taste):
    /// 1. Every verb keeps its base G channel — families with distinct base G are disjoint by
    ///    construction. Two pairs share a G: (PredatesOn, Allows) both G=224, (Leverages, Mirrors)
    ///    both G=255. At drift 25 their R/B boxes stay disjoint: PredatesOn keeps R in [230..255]
    ///    while Allows stays in [179..204]; Leverages keeps B in [230..255] while Mirrors stays in
    ///    [189..214]. Global uniqueness is re-ASSERTED at Build time over the real corpus.
    /// 2. The verb pastel itself (grid index 0) is never used — a tint equal to a restored value
    ///    would be unreadable. Tints for ordinal n use grid index n+1.
    /// 3. Tints stay out of the study register (CrossLinkColorsStudy — every study G differs from
    ///    every grid G), out of the 7 family node colors, and out of the light shipped stroke
    ///    colors listed on CrossLinkColors (checked there 2026-08-31; re-asserted at Build).
    /// 4. Worst verb today is Leverages at 327 links (2026-11-07, per-language, all 8 identical);
    ///    drift 25 gives (25+1)^2 - 1 = 675 tints per verb — 2x headroom. A corpus doubling past
    ///    capacity throws loudly at Build instead of colliding silently.
    /// 5. If restoration cannot run (legacy export still carrying pastels, reader disabled), the
    ///    map degrades to what it is today: arrows in verb colors, identity simply absent —
    ///    never to a wrong identity.
    ///
    /// The Virtues map never gets tints: its creator config never enables CrossLinks (#979/#984 —
    /// no virtue↔fallacy correspondence is drawn), so Build is never called on it.
    /// </summary>
    public sealed class CrossLinkTintTable
    {
        /// <summary>Per-channel drift of a tint from its verb pastel. 25 ⇒ 675 tints/verb.</summary>
        internal const int Drift = 25;

        /// <summary>Grid side; tints per verb = Side² − 1 (index 0 is the pastel itself).</summary>
        internal const int Side = Drift + 1;

        public sealed class TintEntry
        {
            /// <summary>0-based ordinal WITHIN the verb — the input of the tint function.</summary>
            public int VerbOrdinal { get; init; }
            public CrossLink Verb { get; init; }
            public string SourceId { get; init; }
            public string SourcePath { get; init; }
            public string TargetId { get; init; }
            public string TargetPath { get; init; }
            public string TintHex { get; init; }
            /// <summary>"r,g,b" — the key a Batik stroke attribute normalizes to.</summary>
            public string TintRgbKey { get; init; }
            /// <summary>"r,g,b" of the verb pastel this tint restores to.</summary>
            public string PastelRgbKey { get; init; }
        }

        private readonly Dictionary<(string sourceId, CrossLink verb), List<TintEntry>> _entriesBySourceVerb;
        private readonly Dictionary<string, TintEntry> _entryByTintRgbKey;
        private readonly HashSet<string> _pastelRgbKeys;

        public IReadOnlyList<TintEntry> Entries { get; }

        public int Count => Entries.Count;

        private CrossLinkTintTable(List<TintEntry> entries,
            Dictionary<(string, CrossLink), List<TintEntry>> entriesBySourceVerb,
            Dictionary<string, TintEntry> entryByTintRgbKey,
            HashSet<string> pastelRgbKeys)
        {
            Entries = entries;
            _entriesBySourceVerb = entriesBySourceVerb;
            _entryByTintRgbKey = entryByTintRgbKey;
            _pastelRgbKeys = pastelRgbKeys;
        }

        /// <summary>
        /// Walks the items exactly as <c>CreateMindMapNodes</c> does (document order, skip
        /// PK-less rows) and assigns one tint per resolved link. Deterministic: two builds over
        /// the same corpus produce byte-identical tints (pinned by tests) — the reader rebuilds
        /// the table in the same run and must land on the same assignment.
        /// </summary>
        public static CrossLinkTintTable Build(IList<IMindMapItem> items, CrossLink enabled)
        {
            var itemsByPath = CrossLinkResolver.ItemsByPath(items);

            var perVerbOrdinal = new Dictionary<CrossLink, int>();
            var entries = new List<TintEntry>();
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.PK) || item is not Fallacy fallacy)
                {
                    continue;
                }

                foreach (var (verb, targets) in CrossLinkResolver.ResolveOrdered(fallacy, itemsByPath, enabled))
                {
                    foreach (var target in targets)
                    {
                        var ordinal = perVerbOrdinal.TryGetValue(verb, out var seen) ? seen : 0;
                        perVerbOrdinal[verb] = ordinal + 1;

                        var tintHex = TintFor(verb, ordinal, out var pastelHex);
                        entries.Add(new TintEntry
                        {
                            VerbOrdinal = ordinal,
                            Verb = verb,
                            SourceId = fallacy.Id,
                            SourcePath = fallacy.Path,
                            TargetId = target.Id,
                            TargetPath = target.Path,
                            TintHex = tintHex,
                            TintRgbKey = HexToRgbKey(tintHex),
                            PastelRgbKey = HexToRgbKey(pastelHex),
                        });
                    }
                }
            }

            var bySourceVerb = entries.GroupBy(e => (e.SourceId, e.Verb))
                .ToDictionary(group => group.Key, group => group.ToList());
            var byTint = entries.GroupBy(e => e.TintRgbKey)
                .ToDictionary(group => group.Key, group => group.First());

            var pastels = new HashSet<string>();
            foreach (CrossLink verb in Enum.GetValues(typeof(CrossLink)))
            {
                if (verb == CrossLink.None || verb == CrossLink.All)
                {
                    continue;
                }
                pastels.Add(HexToRgbKey(FallacyMindMapDocumentConfig.GetCrossLinkColor(verb)));
                pastels.Add(HexToRgbKey(FallacyMindMapDocumentConfig.GetStudyCrossLinkColor(verb)));
            }

            var table = new CrossLinkTintTable(entries, bySourceVerb, byTint, pastels);
            table.EnsureInvariants();
            return table;
        }

        /// <summary>
        /// Throws (build-time defect, never a per-run data condition) if any measured invariant
        /// of the tint geometry is violated on the real corpus: global tint uniqueness, tints
        /// disjoint from both color registers, per-verb capacity.
        /// </summary>
        private void EnsureInvariants()
        {
            if (Entries.Select(e => e.TintHex).Distinct().Count() != Entries.Count)
            {
                throw new InvalidOperationException(
                    "#2190 CrossLinkTintTable: two links received the same tint — the per-family " +
                    "grid geometry no longer guarantees global uniqueness (see the class comment; " +
                    "CrossLinkColors or Drift changed without re-proving disjointness).");
            }

            var registerCollisions = Entries.Where(e => _pastelRgbKeys.Contains(e.TintRgbKey)).ToList();
            if (registerCollisions.Count > 0)
            {
                throw new InvalidOperationException(
                    $"#2190 CrossLinkTintTable: tint '{registerCollisions[0].TintHex}' collides with a " +
                    "pastel or study register color — a restored value would be unreadable as a tint. " +
                    "Re-check the drift bound against both registers.");
            }

            foreach (var group in Entries.GroupBy(e => e.Verb))
            {
                var needed = group.Count();
                if (needed > Side * Side - 1)
                {
                    throw new InvalidOperationException(
                        $"#2190 CrossLinkTintTable: verb {group.Key} needs {needed} tints, capacity is " +
                        $"{Side * Side - 1} at drift {Drift}. Raise Drift (and re-prove family disjointness) " +
                        "— the corpus grew past the table.");
                }
            }
        }

        /// <summary>
        /// The pure tint function: ordinal n (0-based, per verb, across the whole map) → a hex
        /// tint near the verb pastel. Grid index k = n+1 (index 0 is the pastel itself, never a
        /// tint); dr = k / Side descends R, db = k % Side descends B, G stays at the base value.
        /// </summary>
        internal static string TintFor(CrossLink verb, int ordinal, out string pastelHex)
        {
            pastelHex = FallacyMindMapDocumentConfig.GetCrossLinkColor(verb);
            var r0 = Convert.ToInt32(pastelHex.Substring(1, 2), 16);
            var g0 = Convert.ToInt32(pastelHex.Substring(3, 2), 16);
            var b0 = Convert.ToInt32(pastelHex.Substring(5, 2), 16);

            var k = ordinal + 1;
            var dr = k / Side;
            var db = k % Side;

            if (r0 - dr < 0 || b0 - db < 0)
            {
                throw new InvalidOperationException(
                    $"#2190 CrossLinkTintTable: verb {verb} ordinal {ordinal} walks off the channel floor " +
                    $"(base {pastelHex}, dr {dr}, db {db}) — capacity exceeded, see EnsureInvariants.");
            }

            return $"#{r0 - dr:x2}{g0:x2}{b0 - db:x2}";
        }

        /// <summary>
        /// Per-item tint handout, aligned by construction with
        /// <see cref="CrossLinkResolver.ResolveOrdered"/> over the same item: the writer walks
        /// both lists in lockstep. Duplicate (source, verb, target) triples — none in the corpus
        /// today — still receive DISTINCT tints because the handout is ordinal-based, not
        /// triple-keyed.
        /// </summary>
        public List<(CrossLink Verb, List<string> Tints)> TintsForItem(IMindMapItem item)
        {
            var result = new List<(CrossLink, List<string>)>();
            foreach (var verb in Enum.GetValues(typeof(CrossLink)).Cast<CrossLink>())
            {
                if (verb == CrossLink.None || verb == CrossLink.All)
                {
                    continue;
                }
                if (_entriesBySourceVerb.TryGetValue((item.Id, verb), out var list))
                {
                    result.Add((verb, list.Select(e => e.TintHex).ToList()));
                }
            }

            return result;
        }

        /// <summary>
        /// Maps a Batik stroke attribute value ("rgb(204,255,255)", spaces tolerated) to its link
        /// entry, or null when the value is not a tint of this table.
        /// </summary>
        public TintEntry TryGetEntryByStroke(string strokeAttribute)
        {
            var key = NormalizeRgbKey(strokeAttribute);
            return key != null && _entryByTintRgbKey.TryGetValue(key, out var entry) ? entry : null;
        }

        /// <summary>
        /// #2190 reader pass: rewrites every tint-stroked cross-link group of a Batik export back
        /// to its verb pastel (stroke and, when it carried the tint, fill) and annotates it with
        /// data-crosslink-verb/source/target (taxonomy paths — the corpus columns' own key). The
        /// identity comes from the TINT, never from drawing order (refuted, voie A).
        ///
        /// Degradations are loud, not silent: full restore logs the count; zero restore over a
        /// document still carrying verb pastels is the legacy-export signature (pre-tint .mm
        /// re-exported or tint-free SVG on disk) and logs a warning, NOT a problem — the shipped
        /// palette is already final in that case; zero tint AND zero pastel is a lost carrier and
        /// logs a problem. A partial restore (FreeMind collapsing near-identical tints) is the
        /// worst case — some arrows restored, some not — and logs a problem with both counts.
        /// </summary>
        public static int RestoreTints(XDocument svgDoc, XNamespace svgNamespace, CrossLinkTintTable table,
            string fileName)
        {
            var restored = 0;
            var pastelStrokes = 0;
            foreach (var group in svgDoc.Descendants(svgNamespace + "g"))
            {
                var stroke = (string)group.Attribute("stroke");
                if (string.IsNullOrEmpty(stroke))
                {
                    continue;
                }

                var entry = table.TryGetEntryByStroke(stroke);
                if (entry == null)
                {
                    if (table._pastelRgbKeys.Contains(NormalizeRgbKey(stroke)))
                    {
                        pastelStrokes++;
                    }
                    continue;
                }

                var pastelRgb = $"rgb({entry.PastelRgbKey})";
                group.SetAttributeValue("stroke", pastelRgb);
                if ((string)group.Attribute("fill") == stroke)
                {
                    group.SetAttributeValue("fill", pastelRgb);
                }
                group.SetAttributeValue("class", "crosslink");
                group.SetAttributeValue("data-crosslink-verb", entry.Verb.ToString());
                group.SetAttributeValue("data-crosslink-source", entry.SourcePath);
                group.SetAttributeValue("data-crosslink-target", entry.TargetPath);
                restored++;
            }

            if (table.Count == 0)
            {
                return restored;
            }

            if (restored == table.Count)
            {
                Logger.LogSuccess($"CrossLink tints restored to the verb palette: {restored}/{table.Count} on {fileName}");
            }
            else if (restored == 0)
            {
                if (pastelStrokes > 0)
                {
                    Logger.LogWarning(
                        $"No cross-link tint found on {fileName} but {pastelStrokes} verb-pastel strokes are present — " +
                        "legacy export (the .mm predates the tints or FreeMind re-exported a stale .mm). The palette is " +
                        "already final; only the per-link identity (data-crosslink-*) is absent.");
                }
                else
                {
                    Logger.LogProblem(
                        $"No cross-link tint and no verb pastel found on {fileName} while the corpus resolves " +
                        $"{table.Count} links — the tint carrier was lost between the .mm and this SVG. " +
                        "Investigate before trusting any identity read from this export.");
                }
            }
            else
            {
                Logger.LogProblem(
                    $"Partial cross-link tint restore on {fileName}: {restored}/{table.Count}. Missing tints mean " +
                    "either arrows lost in export or tints FreeMind collapsed into one another (adjacent grid tints " +
                    "differ by 1 on a single channel) — every un-restored stroke stays tinted and identityless.");
            }

            return restored;
        }

        internal static string NormalizeRgbKey(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            var trimmed = value.Trim();
            if (trimmed.StartsWith("rgb(", StringComparison.OrdinalIgnoreCase) && trimmed.EndsWith(")"))
            {
                trimmed = trimmed.Substring(4, trimmed.Length - 5);
            }
            var parts = trimmed.Split(',');
            if (parts.Length != 3)
            {
                return null;
            }
            return $"{parts[0].Trim()},{parts[1].Trim()},{parts[2].Trim()}";
        }

        private static string HexToRgbKey(string hex) =>
            $"{Convert.ToInt32(hex.Substring(1, 2), 16)},{Convert.ToInt32(hex.Substring(3, 2), 16)},{Convert.ToInt32(hex.Substring(5, 2), 16)}";
    }
}
