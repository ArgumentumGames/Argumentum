using System.Collections.Generic;
using System.Runtime.Serialization;
using ImageMagick;

namespace Argumentum.AssetConverter;

public class CardSetGenerationDocument: DocumentConfig
{
	[DataMember(Order = 0)]
	public override bool Enabled { get; set; }

	[DataMember(Order = 1)]
	public override string DocumentName { get; set; }

	public List<DocumentCardSet> CardSets { get; set; }

	public int TargetDensity { get; set; } = 0;

	public MagickFormat ImageFormat { get; set; } = MagickFormat.Png;

	public CardDocumentFormat DocumentFormat { get; set; } = CardDocumentFormat.BackFirstOneDocPerBack;

	public bool NoBack { get; set; }

	public string PageSize { get; set; } = "A4";

	public string Header { get; set; } = "";



}