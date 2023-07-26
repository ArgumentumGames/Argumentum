using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities;

public class ArgumentVirtue : CsvBase<ArgumentVirtue, ArgumentVirtueClassMap>
{
	public string Pk { get; set; }
	public string Path { get; set; }
	public int Depth { get; set; }
	public string DecimalPathPadded { get; set; }
	public string FamilyFr { get; set; }
	public string SubfamilyFr { get; set; }
	public string SubsubfamilyFr { get; set; }
	public string TitleFr { get; set; }
	public string DescriptionFr { get; set; }
	public string RemarkFr { get; set; }
	public string LinkFr { get; set; }
	public string FamilyFrCamelcase { get; set; }
	public string DepthMax4 { get; set; }
	public string Card { get; set; }
	public string Update { get; set; }
	public string Locked { get; set; }
}

public sealed class ArgumentVirtueClassMap : ClassMap<ArgumentVirtue>
{
	public ArgumentVirtueClassMap()
	{
		Map(m => m.Pk).Name("pk");
		Map(m => m.Path).Name("path");
		Map(m => m.Depth).Name("depth");
		Map(m => m.DecimalPathPadded).Name("decimal_path_padded");
		Map(m => m.FamilyFr).Name("family_fr");
		Map(m => m.SubfamilyFr).Name("subfamily_fr");
		Map(m => m.SubsubfamilyFr).Name("subsubfamily_fr");
		Map(m => m.TitleFr).Name("title_fr");
		Map(m => m.DescriptionFr).Name("description_fr");
		Map(m => m.RemarkFr).Name("remark_fr");
		Map(m => m.LinkFr).Name("link_fr");
		Map(m => m.FamilyFrCamelcase).Name("family_fr_camelcase");
		Map(m => m.DepthMax4).Name("depth_max4");
		Map(m => m.Card).Name("card");
		Map(m => m.Update).Name("update");
		Map(m => m.Locked).Name("locked");
	}
}