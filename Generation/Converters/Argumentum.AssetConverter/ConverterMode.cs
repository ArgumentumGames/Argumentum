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
		OwlGenerator = 1 << 5, // 32
		TaxonomyValidator = 1 << 6, // 64
		OwlValidator = 1 << 7, // 128
		CardValidator = 1 << 8, // 256
		ContinuousValidator = 1 << 9, // 512
		TranslationCoverage = 1 << 10, // 1024
		ParallelismOptimizer = 1 << 11, // 2048
	}
}