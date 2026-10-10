namespace Argumentum.AssetConverter;

public static class KnownDataSets
{
	public static string None = "";
	//public static string Fallacies = "Fallacies";
	//public static string FallaciesPrintAndPlay = "Fallacies - Print & Play";
	public static string Scenarii = "Scenarii";
	//public static string ScenariiPrintAndPlay = "Scenarii - Print & Play";
	public static string Rules = "Rules";
	public static string RulesPrintAndPlay = "Rules - Print & Play";
	public static string FallaciesTaxonomy = "Fallacies - Taxonomy";
	public static string VirtuesTaxonomy = "Fallacies - Virtues";
	public static string DnnUiStrings = "DNN UI Strings";
	// T2 site content (issue #457): 2sxc app 33 pivot rows. DatasetUpdater-only dataset —
	// the updater engine reads raw CSV headers (GetDictionaryFromCsv, no ClassMap), so no
	// CsvType entity is wired; the harvest/mindmap paths null-guard CsvType and never see it.
	public static string DnnApp33Content = "DNN App33 Content";
}