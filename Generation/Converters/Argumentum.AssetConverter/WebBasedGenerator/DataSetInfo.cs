using System.Text;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter;





public class DataSetInfo
{

	public string Name { get; set; }

	public string FilePath { get; set; }


	private string _StringContent;

	public async Task<string>   GetContent()
	{
		if (string.IsNullOrEmpty(_StringContent))
		{
			var payLoad = await FilePath.GetDocumentPayload();
			_StringContent = Encoding.UTF8.GetString(payLoad.Content);
		}

		return _StringContent;

	}



}



public static class KnownDataSets
{
	public static string Fallacies = "Fallacies";
	public static string Scenarii = "Scenarii";
	public static string Rules = "Rules";


}
