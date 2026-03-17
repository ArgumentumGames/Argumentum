using System.Collections;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter
{
    public abstract class VirtueDocumentConfigBase : DocumentConfig, IMindMapDocumentConfig
    {
        public string DataSet { get; set; } = @"..\..\..\Data\Mindmap\Argumentum Virtues - Taxonomy.csv";

        public abstract Task GenerateMindMapFile(IList objects, AssetConverterConfig config,
            string targetDirectory, string language);
    }
}