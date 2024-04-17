using System;
using System.Collections.Generic;
using System.Linq;

namespace Argumentum.AssetConverter.Mindmapper;


public class SVGFreemindMap : DocumentConfig, ICloneable
{

	public bool SetSVGNodeAttributes { get; set; }

	public string SvgWidth { get; set; } =  "200vh";

	public string SvgHeight { get; set; } = "450vh";

	public bool WrapNodeByLink { get; set; }


	public List<DocumentConfig> HtmlWrappers { get; set; } = new List<DocumentConfig>();
	public bool RemoveImages { get; set; }


	protected override DocumentConfig GetClone()
	{
		var toReturn = (SVGFreemindMap) this.MemberwiseClone();
		toReturn.HtmlWrappers = new List<DocumentConfig>(this.HtmlWrappers.Select(htmlDoc => (DocumentConfig)htmlDoc.Clone()));
		return toReturn;
	}
}



