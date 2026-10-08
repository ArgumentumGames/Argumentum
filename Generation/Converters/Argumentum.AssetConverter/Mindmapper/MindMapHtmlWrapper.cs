using System;

namespace Argumentum.AssetConverter.Mindmapper;

/// <summary>
/// Pure helper for mind map HTML wrapper templating. Extracted in issue #196 to make
/// wrapper generation testable without Playwright / filesystem / pipeline runs.
///
/// The templates used by the pipeline are <c>Cards/Fallacies/Mindmaps/included.html</c>
/// (inline SVG variant) and <c>external.html</c> (external <c>&lt;object&gt;</c> variant).
/// Both carry <c>[LANGUAGE]</c>; each carries exactly one of the other two — the helper runs
/// all three substitutions unconditionally, which is a no-op on whichever token is absent:
/// <list type="bullet">
///   <item><c>[LANGUAGE]</c> — both templates. The 2-letter corpus language of the wrapper,
///       substituted into <c>&lt;html lang="..."&gt;</c>. Before T4a the templates hardcoded
///       <c>lang="en"</c>, so all 36 committed wrappers — French, Russian and Arabic ones
///       included — declared themselves English.</item>
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
    /// Applies the three placeholder substitutions in a single pass. Safe to call on both
    /// <c>included.html</c> and <c>external.html</c> templates: missing placeholders are no-ops.
    /// <paramref name="language"/> is required, not optional: the defect this fixes (#457 T4a)
    /// was a <em>silent</em> default — a defaulted parameter would reproduce it by construction.
    /// A null/empty language substitutes to the empty string (same contract as the other two
    /// arguments), yielding <c>lang=""</c>, i.e. "unknown" rather than a wrong language.
    /// </summary>
    /// <remarks>
    /// <c>[LANGUAGE]</c> is consumed <b>first</b>, before either SVG argument is injected: it is
    /// the only token that lives in the template head, so substituting it last would let an
    /// injected SVG body containing the literal text <c>[LANGUAGE]</c> be corrupted.
    /// </remarks>
    public static string FormatWrapper(string template, string svgRelativePath, string svgContent, string language)
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
            .Replace("[LANGUAGE]", language ?? string.Empty)
            .Replace("[SVGPATH]", svgRelativePath ?? string.Empty)
            .Replace("[SVGCONTENT]", svgContent ?? string.Empty);
    }
}
