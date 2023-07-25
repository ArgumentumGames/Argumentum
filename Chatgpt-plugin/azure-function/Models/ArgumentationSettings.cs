using Argumentum.AssetConverter;
using Spectre.Console;
using System.Collections.Concurrent;

namespace Models;

public class ArgumentationSettings
{

	public int ChunkSize { get; set; } = 3;

	public int NbMessageCalls { get; set; } = 3;

	public int SkipChunkNb { get; set; } = 0;

	public int TakeChunkNb { get; set; } = 0;

	public int MaxDegreeOfParallelismWebService { get; set; } = 8;

	public List<string> FieldsShort { get; set; } = new List<string>(new[]
	{
		"path",
		"text_en",
	});

	public List<string> FieldsLong { get; set; } = new List<string>(new[]
	{
		"path",
		"Family",
		"Subfamily",
		"Subsubfamily",
		"text_en",
		"desc_en",
		"example_en",
		"link_en"
	});

	public string PrimaryField { get; set; } = "path";

	

	public DataSetInfo SourceDataset { get; set; } = new DataSetInfo()
	{
		Name = "Argumentum - Fallacies - Taxonomy",
		ReleaseFilePath = "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Argumentum%20Fallacies%20-%20Taxonomy.csv",
		DebugFilePath = @"..\..\..\..\..\Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv",
	};




	
}





