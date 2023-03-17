using System;

namespace Argumentum.AssetConverter;

public class DocumentPayload
{
	public string FileName { get; set; }
	public Byte[] Content { get; set; }

	public string MimeType { get; set; }


}