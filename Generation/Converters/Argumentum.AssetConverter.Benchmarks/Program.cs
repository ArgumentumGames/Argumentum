using Argumentum.AssetConverter.Benchmarks;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(RuleParsingBenchmarks).Assembly).Run(args);

return 0;
