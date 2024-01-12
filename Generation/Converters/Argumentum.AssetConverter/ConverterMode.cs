using System;

namespace Argumentum.AssetConverter
{
	[Flags]
	public enum ConverterMode
	{
		None = 0,
		BatchImageProcessor = 1 << 0, // 1
		WebBasedImageGeneration = 1 << 1, // 2
		Mindmapper = 1 << 2, // 4
		Dnn2sxc = 1 << 3, // 8
		DatasetUpdater = 1 << 4, // 16
	}
}