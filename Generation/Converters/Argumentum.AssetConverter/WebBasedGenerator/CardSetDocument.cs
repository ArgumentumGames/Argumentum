using System;

namespace Argumentum.AssetConverter;

public class CardSetDocument:ICloneable
{
	public string name { get; set; }
	public string notes { get; set; }
	public int dpi { get; set; }
	public bool live { get; set; }
	public string psize { get; set; }
	public string pori { get; set; }
	public string csize { get; set; }
	public string cori { get; set; }
	public string cheight { get; set; }
	public string cwidth { get; set; }
	public string cunit { get; set; }
	public bool ccircle { get; set; }
	public int gsize { get; set; }
	public string gunit { get; set; }
	public float msize { get; set; }
	public string munit { get; set; }
	public float blsize { get; set; }
	public string blunit { get; set; }
	public float ssize { get; set; }
	public string sunit { get; set; }
	public bool cutline { get; set; }
	public float bradius { get; set; }
	public string brunit { get; set; }
	public bool overlay { get; set; }
	public float oopa { get; set; }
	public string oURL { get; set; }
	public string extCSS { get; set; }
	public string css { get; set; }
	public string csv { get; set; }
	public string mustache { get; set; }
	public bool useMustache { get; set; }
	public string cardClass { get; set; }
	public string rscount { get; set; }
	public string rsstyle { get; set; }
	public string cindices { get; set; }


	object ICloneable.Clone()
	{
		return this.Clone();
	}

	public CardSetDocument Clone()
	{
		return new CardSetDocument()
		{
			blsize = blsize,
			blunit = blunit,
			bradius = bradius,
			brunit = brunit,
			cardClass = cardClass,
			ccircle = ccircle,
			cheight = cheight,
			cindices = cindices,
			cori = cori,
			csize = csize,
			css = css,
			csv = csv,
			cunit = cunit,
			cutline = cutline,
			cwidth = cwidth,
			dpi = dpi,
			extCSS = extCSS,
			gsize = gsize,
			gunit = gunit,
			live = live,
			msize = msize,
			munit = munit,
			mustache = mustache,
			name = name,
			notes = notes,
			oURL = oURL,
			oopa = oopa,
			overlay = overlay,
			pori = pori,
			psize = psize,
			rscount = rscount,
			rsstyle = rsstyle,
			ssize = ssize,
			sunit = sunit,
			useMustache = useMustache
		};
	}
}