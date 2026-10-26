using System;
using System.Collections.Generic;
using System.Linq;

namespace Argumentum.AssetConverter.Mindmapper;


public class SVGFreemindMap : DocumentConfig, ICloneable
{

	public bool SetSVGNodeAttributes { get; set; }

	public string SvgWidth { get; set; } 

	public string SvgHeight { get; set; } 


	public string SvgViewBox { get; set; }

	public bool WrapNodeByLink { get; set; }

	/// <summary>
	/// #1248 dual palette: render this variant's cross-links in the high-contrast study register
	/// (FallacyMindMapDocumentConfig.CrossLinkColorsStudy) instead of the subtle default baked
	/// into the .mm. Set on the links.svg study variant only.
	/// </summary>
	public bool HighContrastCrossLinks { get; set; }


	public List<DocumentConfig> HtmlWrappers { get; set; } = new List<DocumentConfig>();

	/// <summary>
	/// #457 T4b: the localized <c>&lt;title&gt;</c> of this variant's HTML wrappers, keyed by
	/// corpus language ("fr", "en", …). Declared here rather than on each
	/// <see cref="DocumentConfig"/> because both wrapper halves of a variant — the inlining one
	/// and the <c>_ext</c> one — are the same document and must carry the same title.
	/// <para>
	/// The values are the map's own central topic as already localized in
	/// <c>content.svg</c>, so the title cannot drift from the corpus it titles. Missing an
	/// entry is not silent: <see cref="MindMapHtmlWrapper.ResolveWrapperTitle"/> degrades to
	/// <see cref="MindMapHtmlWrapper.WrapperTitleFallbackLanguage"/> and
	/// <see cref="MindMapSvgWrapperWriter"/> logs a warning naming the language.
	/// </para>
	/// </summary>
	public Dictionary<string, string> HtmlWrapperTitles { get; set; } =
		new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	public bool RemoveImages { get; set; }


	protected override DocumentConfig GetClone()
	{
		var toReturn = (SVGFreemindMap) this.MemberwiseClone();
		toReturn.HtmlWrappers = new List<DocumentConfig>(this.HtmlWrappers.Select(htmlDoc => (DocumentConfig)htmlDoc.Clone()));
		// MemberwiseClone shares the reference — copy it, so a clone cannot mutate its source's titles.
		toReturn.HtmlWrapperTitles = new Dictionary<string, string>(this.HtmlWrapperTitles, StringComparer.OrdinalIgnoreCase);
		return toReturn;
	}
}



