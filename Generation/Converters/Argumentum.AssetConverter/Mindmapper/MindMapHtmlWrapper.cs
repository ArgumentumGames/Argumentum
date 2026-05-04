using System;

namespace Argumentum.AssetConverter.Mindmapper;

/// <summary>
/// Pure helper for mind map HTML wrapper templating. Extracted in issue #196 to make
/// wrapper generation testable without Playwright / filesystem / pipeline runs.
///
/// The templates used by the pipeline are <c>Cards/Fallacies/Mindmaps/included.html</c>
/// (inline SVG variant) and <c>external.html</c> (external <c>&lt;object&gt;</c> variant).
/// They each carry exactly one placeholder token — the helper runs both substitutions
/// unconditionally, which is a no-op on whichever token is absent:
/// <list type="bullet">
///   <item><c>[SVGPATH]</c> — external.html only. Relative path from the wrapper directory to
///       the SVG file, injected inside <c>&lt;object data="..."&gt;</c>.</item>
///   <item><c>[SVGCONTENT]</c> — included.html only. Full inline SVG markup, dropped directly
///       into the <c>#mindmap</c> container.</item>
/// </list>
/// <c>FormatWrapper_External_TemplateHasNoSvgContentPlaceholder</c> and the
/// <c>grep</c>-backed precondition in the unit tests pin this contract.
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

        // Strip XML declaration from inline SVG — invalid inside HTML documents
        // (browsers may misinterpret encoding="utf-16" and break rendering)
        if (svgContent != null)
        {
            var xmlDeclStart = svgContent.IndexOf("<?xml", StringComparison.Ordinal);
            if (xmlDeclStart >= 0)
            {
                var xmlDeclEnd = svgContent.IndexOf("?>", xmlDeclStart, StringComparison.Ordinal);
                if (xmlDeclEnd >= 0)
                    svgContent = svgContent.Remove(xmlDeclStart, xmlDeclEnd + 2 - xmlDeclStart).TrimStart();
            }
        }

        return template
            .Replace("[SVGPATH]", svgRelativePath ?? string.Empty)
            .Replace("[SVGCONTENT]", svgContent ?? string.Empty);
    }
}
