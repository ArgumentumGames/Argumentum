using System;

namespace Argumentum.AssetConverter.Mindmapper;

/// <summary>
/// Pure helper for mind map HTML wrapper templating. Extracted in issue #196 to make
/// wrapper generation testable without Playwright / filesystem / pipeline runs.
///
/// The templates used by the pipeline are <c>Cards/Fallacies/Mindmaps/included.html</c>
/// (inline SVG variant) and <c>external.html</c> (external <c>&lt;object&gt;</c> variant).
/// They share two placeholder tokens:
/// <list type="bullet">
///   <item><c>[SVGPATH]</c> — relative path from the wrapper directory to the SVG file. Used by the
///       external variant inside <c>&lt;object data="..."&gt;</c>. Also present in included.html
///       for fallback, harmless there.</item>
///   <item><c>[SVGCONTENT]</c> — the full inline SVG markup. Used only by the included variant.
///       External.html does not contain this token.</item>
/// </list>
/// </summary>
public static class MindMapHtmlWrapper
{
    /// <summary>
    /// Applies the two placeholder substitutions in a single pass. Safe to call on both
    /// <c>included.html</c> and <c>external.html</c> templates: missing placeholders are no-ops.
    /// </summary>
    public static string FormatWrapper(string template, string svgRelativePath, string svgContent)
    {
        if (template == null) throw new ArgumentNullException(nameof(template));
        return template
            .Replace("[SVGPATH]", svgRelativePath ?? string.Empty)
            .Replace("[SVGCONTENT]", svgContent ?? string.Empty);
    }
}
