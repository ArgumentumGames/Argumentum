using System;

namespace Argumentum.AssetConverter.Mindmapper;

/// <summary>
/// Pure helper for mind map HTML wrapper templating. Extracted in issue #196 to make
/// wrapper generation testable without Playwright / filesystem / pipeline runs.
///
/// The templates used by the pipeline are <c>Cards/Fallacies/Mindmaps/included.html</c>
/// (inline SVG variant) and <c>external.html</c> (external <c>&lt;object&gt;</c> variant).
/// They each carry three placeholder tokens — the helper runs every substitution
/// unconditionally, which is a no-op on whichever token is absent:
/// <list type="bullet">
///   <item><c>[SVGPATH]</c> — external.html only. Relative path from the wrapper directory to
///       the SVG file, injected inside <c>&lt;object data="..."&gt;</c>.</item>
///   <item><c>[SVGCONTENT]</c> — included.html only. Full inline SVG markup, dropped directly
///       into the <c>#mindmap</c> container.</item>
///   <item><c>[LANG]</c> — both templates. The BCP-47 language of the map's <em>content</em>,
///       injected into <c>&lt;html lang="..."&gt;</c>. Added by T4-a: the templates used to
///       hard-code <c>lang="en"</c>, so all <b>34</b> committed wrappers (17 inlining + 17
///       <c>_ext</c>, measured 2026-10-08) declared English regardless of their language
///       directory — screen readers, search engines and the browser's translation prompt all
///       read the page as English. The 2 files at <c>Mindmaps/</c> root are the templates
///       themselves, not wrappers.</item>
/// </list>
/// <c>FormatWrapper_External_TemplateHasNoSvgContentPlaceholder</c> and the
/// <c>grep</c>-backed precondition in the unit tests pin this contract.
/// </summary>
public static class MindMapHtmlWrapper
{
    /// <summary>
    /// Applies the placeholder substitutions in a single pass. Safe to call on both
    /// <c>included.html</c> and <c>external.html</c> templates: missing placeholders are no-ops.
    /// </summary>
    /// <param name="language">
    /// BCP-47 language tag of the map content (e.g. <c>fr</c>, <c>ar</c>, <c>zh</c>). Required:
    /// an absent value would emit <c>lang=""</c>, which is invalid HTML and silently wrong for
    /// assistive technology — so an empty language fails fast instead.
    /// </param>
    public static string FormatWrapper(string template, string svgRelativePath, string svgContent, string language)
    {
        if (template == null) throw new ArgumentNullException(nameof(template));
        if (string.IsNullOrWhiteSpace(language))
            throw new ArgumentException(
                "A non-empty BCP-47 language is required: emitting an empty lang attribute would be invalid HTML.",
                nameof(language));

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
            .Replace("[SVGCONTENT]", svgContent ?? string.Empty)
            .Replace("[LANG]", language.Trim());
    }
}
