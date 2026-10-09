using System;
using System.Collections.Generic;

namespace Argumentum.AssetConverter.Mindmapper;

/// <summary>
/// Pure helper for mind map HTML wrapper templating. Extracted in issue #196 to make
/// wrapper generation testable without Playwright / filesystem / pipeline runs.
///
/// The templates used by the pipeline are <c>Cards/Fallacies/Mindmaps/included.html</c>
/// (inline SVG variant) and <c>external.html</c> (external <c>&lt;object&gt;</c> variant).
/// Each template carries exactly one of the two SVG tokens; the helper runs every
/// substitution unconditionally, which is a no-op on whichever token is absent:
/// <list type="bullet">
///   <item><c>[LANGUAGE]</c> — both templates. The 2-letter corpus language of the wrapper,
///       substituted into <c>&lt;html lang="..."&gt;</c>. Before T4a the templates hardcoded
///       <c>lang="en"</c>, so all 36 committed wrappers — French, Russian and Arabic ones
///       included — declared themselves English.</item>
///   <item><c>[DIRECTION]</c> — both templates, added by #457 T4b. <c>rtl</c> for the
///       right-to-left corpus languages, <c>ltr</c> otherwise, substituted into
///       <c>&lt;html lang="..." dir="..."&gt;</c>. Written for every language, not only the
///       RTL pair, so the wrapper states its direction instead of inheriting a default.</item>
///   <item><c>[TITLE]</c> — both templates, added by #457 T4b. The wrapper document title,
///       substituted into <c>&lt;title&gt;</c>. Before T4b the templates hardcoded
///       <c>&lt;title&gt;Taxonomy Mind Map&lt;/title&gt;</c> — the same defect class as
///       <c>lang="en"</c>, and it survived T4a: all 36 committed wrappers titled themselves
///       in English. The value is <em>not</em> derived from the language here; it is declared
///       per language by the caller (see <see cref="ResolveWrapperTitle"/>).</item>
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
    /// The language whose title a wrapper falls back to when its own is not declared: French,
    /// the corpus source language. A fallback is a <em>degradation</em>, never a silent default —
    /// <see cref="MindMapSvgWrapperWriter"/> logs a warning when it fires, and
    /// <c>MindmapWrapperLanguageTagTests</c> pins the exact language set so a missing
    /// declaration is a red test rather than a French title on a Chinese page.
    /// </summary>
    public const string WrapperTitleFallbackLanguage = "fr";

    /// <summary>
    /// The corpus languages written right to left. Kept as an explicit set rather than an
    /// <c>isRtl</c> flag per call site: the set is the fact, and a new RTL language added
    /// without touching it would silently get <c>dir="ltr"</c>.
    /// </summary>
    private static readonly HashSet<string> RightToLeftLanguages =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "ar", "fa" };

    /// <summary>
    /// The HTML <c>dir</c> attribute value for a corpus language: <c>rtl</c> for the languages in
    /// <see cref="RightToLeftLanguages"/>, <c>ltr</c> for everything else — including a null or
    /// blank language, which is "unknown" and therefore left-to-right by the HTML default.
    /// </summary>
    public static string DirectionFor(string language) =>
        language != null && RightToLeftLanguages.Contains(language) ? "rtl" : "ltr";

    /// <summary>
    /// True when <paramref name="titles"/> declares a non-blank title for exactly
    /// <paramref name="language"/>. Callers that want to distinguish "declared" from
    /// "fell back to French" ask this before <see cref="ResolveWrapperTitle"/>.
    /// </summary>
    public static bool HasWrapperTitleFor(IReadOnlyDictionary<string, string> titles, string language) =>
        titles != null
        && !string.IsNullOrWhiteSpace(language)
        && titles.TryGetValue(language, out var exact)
        && !string.IsNullOrWhiteSpace(exact);

    /// <summary>
    /// Resolves the wrapper title: the exact language when declared, else
    /// <see cref="WrapperTitleFallbackLanguage"/>, else <c>null</c> (no usable title at all).
    /// A null or blank <paramref name="language"/> never matches an exact entry — it goes
    /// straight to the fallback, so an unknown language renders a visible French title rather
    /// than an empty one.
    /// </summary>
    public static string ResolveWrapperTitle(IReadOnlyDictionary<string, string> titles, string language)
    {
        if (HasWrapperTitleFor(titles, language))
            return titles[language];

        return titles != null
               && titles.TryGetValue(WrapperTitleFallbackLanguage, out var fallback)
               && !string.IsNullOrWhiteSpace(fallback)
            ? fallback
            : null;
    }

    /// <summary>
    /// Applies every placeholder substitution in a single pass. Safe to call on both
    /// <c>included.html</c> and <c>external.html</c> templates: missing placeholders are no-ops.
    /// <paramref name="language"/> is required, not optional: the defect this fixes (#457 T4a)
    /// was a <em>silent</em> default — a defaulted parameter would reproduce it by construction.
    /// A null/empty language substitutes to the empty string (same contract as the other
    /// arguments), yielding <c>lang=""</c>, i.e. "unknown" rather than a wrong language.
    /// <paramref name="title"/> is nullable because it is <em>declared data</em>: an undeclared
    /// title yields an empty <c>&lt;title&gt;</c>, which is visible in the browser tab and
    /// therefore testable, rather than an invented one.
    /// </summary>
    /// <remarks>
    /// The three head tokens — <c>[LANGUAGE]</c>, <c>[DIRECTION]</c>, <c>[TITLE]</c> — are consumed
    /// <b>before</b> either SVG argument is injected: they live in the template head, so
    /// substituting them last would let an injected SVG body containing one of those literals be
    /// corrupted.
    /// </remarks>
    public static string FormatWrapper(string template, string svgRelativePath, string svgContent, string language, string title)
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
            .Replace("[DIRECTION]", DirectionFor(language))
            .Replace("[TITLE]", title ?? string.Empty)
            .Replace("[SVGPATH]", svgRelativePath ?? string.Empty)
            .Replace("[SVGCONTENT]", svgContent ?? string.Empty);
    }
}
