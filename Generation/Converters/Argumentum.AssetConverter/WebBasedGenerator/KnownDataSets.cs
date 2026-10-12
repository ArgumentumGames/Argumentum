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
	// T2 volume (issue #457, volume opened #458 c.6107217348): the app33 volume worklists
	// derived from the 2026-10-09 export — fr-filled prose rows only, the 7 demo entities
	// (manifest rendering.demoMarkers) and the reviewed trial entity excluded, non-prose
	// attributes (file refs, fa-icons, Link/Window/VideoLink paths & enums) filtered out.
	// Same DatasetUpdater-only wiring as DnnApp33Content above.
	public static string DnnApp33ContentVolume = "DNN App33 Content Volume";
	public static string DnnApp33LinkVolume = "DNN App33 Link Volume";
	public static string DnnApp33VideoVolume = "DNN App33 Video Volume";
	// Owner's Rules 06 lot (issue #1502 c.6098939191): the 2 changed bullets of rule 6, one
	// row per bullet. DatasetUpdater-only dataset — the updater engine reads raw CSV headers
	// (GetDictionaryFromCsv, no ClassMap), so no CsvType entity is wired; the harvest/mindmap
	// paths null-guard CsvType and never see it (same wiring as the T2 app33 dataset, #457).
	public static string Rules06LotPhrases = "Rules 06 Owner Lot Phrases";
}