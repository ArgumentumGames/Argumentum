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
	public bool RemoveImages { get; set; }


	protected override DocumentConfig GetClone()
	{
		var toReturn = (SVGFreemindMap) this.MemberwiseClone();
		toReturn.HtmlWrappers = new List<DocumentConfig>(this.HtmlWrappers.Select(htmlDoc => (DocumentConfig)htmlDoc.Clone()));
		return toReturn;
	}
}



