using System;
using System.Collections.Generic;
using System.Linq;

namespace Argumentum.AssetConverter.Mindmapper;


public class SVGFreemindMap : DocumentConfig, ICloneable
{

	public bool SetSVGNodeAttributes { get; set; }

	public bool WrapNodeByLink { get; set; }


	public List<DocumentConfig> HtmlWrappers { get; set; } = new List<DocumentConfig>();


	protected override DocumentConfig GetClone()
	{
		return new SVGFreemindMap()
		{
			SetSVGNodeAttributes = SetSVGNodeAttributes,
			WrapNodeByLink = WrapNodeByLink,
			HtmlWrappers = new List<DocumentConfig>(this.HtmlWrappers.Select(htmlDoc => (DocumentConfig) htmlDoc.Clone()))
		};
	}
}



