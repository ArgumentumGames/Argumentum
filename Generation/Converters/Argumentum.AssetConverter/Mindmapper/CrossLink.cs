using System;

namespace Argumentum.AssetConverter.Mindmapper;

/// <summary>
/// #1181: aligned with the 8 crossLink_* CSV verbs of the Fallacies taxonomy
/// (crossLink_PredatesOn/_Denounces/_Leverages/_Allows/_Opposes/_Inverts/_Mirrors/_IsRelatedTo),
/// the same vocabulary the OWL emitter consumes (<c>OwlGeneratorConfig.CreateOwlDocument</c>).
/// The previous generation of this enum ({None, Identity, Opposite, AppealTo, Symmetric}) predated
/// the corpus columns and had an empty intersection with them, which left the Arrowlink rendering
/// block unreachable — the shipped mindmaps carried 0 arrows while the corpus holds 1230 links.
/// [Flags] so a map configuration can draw a subset of the verbs.
/// </summary>
[Flags]
public enum CrossLink
{
    None = 0,
    PredatesOn = 1 << 0,
    Denounces = 1 << 1,
    Leverages = 1 << 2,
    Allows = 1 << 3,
    Opposes = 1 << 4,
    Inverts = 1 << 5,
    Mirrors = 1 << 6,
    IsRelatedTo = 1 << 7,

    All = PredatesOn | Denounces | Leverages | Allows | Opposes | Inverts | Mirrors | IsRelatedTo,
}
