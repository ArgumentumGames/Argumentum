using System.Collections;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter
{
    public interface IMindMapDocumentConfig
    {
        string DataSet { get; set; }
        Task GenerateMindMapFile(IList objects, AssetConverterConfig config, string targetDirectory, string language);
    }
}