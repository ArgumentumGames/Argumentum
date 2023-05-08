using System.Text;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter;





public class DataSetInfo
{

	public string Name { get; set; }

	public string FilePath { get; set; }
	public string DebugFilePath { get; set; }



	private string _StringContent;

	public async Task<string>   GetContent(bool debugPath)
	{
		if (string.IsNullOrEmpty(_StringContent))
		{
			var strPath = debugPath && !string.IsNullOrEmpty(DebugFilePath) ? DebugFilePath: FilePath;
			var payLoad = await strPath.GetDocumentPayload();
			_StringContent = Encoding.UTF8.GetString(payLoad.Content);
		}

		return _StringContent;

	}



}



public static class KnownDataSets
{
	public static string None = "";
	public static string Fallacies = "Fallacies";
	public static string FallaciesPrintAndPlay = "Fallacies - Print & Play";
	public static string Scenarii = "Scenarii";
	public static string ScenariiPrintAndPlay = "Scenarii - Print & Play";
	public static string Rules = "Rules";
	public static string RulesPrintAndPlay = "Rules - Print & Play";
	public static string FallaciesTaxonomy = "Fallacies - Taxonomy";
}
