using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter;

public abstract class FallacyDocumentConfigBase : DocumentConfig, IMindMapDocumentConfig
{
    public string DataSet { get; set; } = @"..\..\..\Data\Mindmap\Argumentum Fallacies - Taxonomy.csv";

    public abstract Task GenerateMindMapFile(IList objects, AssetConverterConfig config,
        string targetDirectory, string language);
}