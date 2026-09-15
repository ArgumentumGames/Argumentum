using System;
using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter.Mindmapper;

/// <summary>
/// #1181: resolves the transverse cross-links of the Fallacies taxonomy into mindmap arrows.
/// Mirrors the OWL emitter table (<c>OwlGeneratorConfig.CreateOwlDocument</c>, crossLinkVerbs):
/// same split on ';', same trim, same skip of unresolvable paths and self-links. Targets are
/// TAXONOMIC PATHS ("7.1.2.3"), not PKs, and a cell may list several separated by ';'.
/// Drawing is one-directional (source→target): the four symmetric verbs are symmetric in meaning,
/// and a second arrow on the same pair would only overdraw the first.
/// </summary>
public static class CrossLinkResolver
{
    public static readonly (CrossLink Verb, Func<Fallacy, string> Raw, bool Symmetric)[] Verbs =
    {
        (CrossLink.PredatesOn, f => f.CrossLinkPredatesOn, false),
        (CrossLink.Denounces, f => f.CrossLinkDenounces, false),
        (CrossLink.Leverages, f => f.CrossLinkLeverages, false),
        (CrossLink.Allows, f => f.CrossLinkAllows, false),
        (CrossLink.Opposes, f => f.CrossLinkOpposes, true),
        (CrossLink.Inverts, f => f.CrossLinkInverts, true),
        (CrossLink.Mirrors, f => f.CrossLinkMirrors, true),
        (CrossLink.IsRelatedTo, f => f.CrossLinkIsRelatedTo, true),
    };

    /// <summary>
    /// Path→item index over the map's own items; first occurrence wins on a duplicated path
    /// (paths are the taxonomy's structural key — a duplicate would be a corpus defect, and the
    /// OWL emitter's fallaciesByPath lookup makes the same first-wins assumption).
    /// </summary>
    public static Dictionary<string, IMindMapItem> ItemsByPath(IEnumerable<IMindMapItem> items)
    {
        var byPath = new Dictionary<string, IMindMapItem>();
        foreach (var item in items)
        {
            byPath.TryAdd(item.Path, item);
        }
        return byPath;
    }

    public static IEnumerable<(CrossLink Verb, IMindMapItem Target)> Resolve(Fallacy source,
        IReadOnlyDictionary<string, IMindMapItem> itemsByPath, CrossLink enabled)
    {
        foreach (var (verb, raw, _) in Verbs)
        {
            if (!enabled.HasFlag(verb))
            {
                continue;
            }

            var value = raw(source);
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            foreach (var targetPath in value.Split(';').Select(x => x.Trim()).Where(x => x.Length > 0))
            {
                if (!itemsByPath.TryGetValue(targetPath, out var target))
                {
                    continue;
                }
                if (target.Path == source.Path)
                {
                    continue;
                }
                yield return (verb, target);
            }
        }
    }
}
