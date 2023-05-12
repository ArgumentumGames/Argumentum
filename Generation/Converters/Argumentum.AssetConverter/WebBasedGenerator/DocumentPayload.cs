using System;
using System.Text;

namespace Argumentum.AssetConverter;

public class DocumentPayload
{
	public string FileName { get; set; }
	public Byte[] Content { get; set; }

	public string MimeType { get; set; }

	public string AsString() => Encoding.UTF8.GetString(Content);


}