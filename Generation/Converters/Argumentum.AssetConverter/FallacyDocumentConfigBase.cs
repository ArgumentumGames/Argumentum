using System.Collections.Generic;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter;

public abstract class FallacyDocumentConfigBase : DocumentConfig
{
	public string DataSet { get; set; } = @"..\..\..\Data\Mindmap\Argumentum Fallacies - Taxonomy.csv";

	public abstract Task GenerateFallacyFile(IList<Fallacy> fallacies, AssetConverterConfig config,
		string targetDirectory, string language);

}