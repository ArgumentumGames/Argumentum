using System;
using System.IO;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Utility
{
	/// <summary>
	/// #1194 guard. Without an <c>xunit.runner.json</c>, xUnit sets <c>maxParallelThreads</c> to the
	/// machine's core count. That made the suite's effective parallelism an ENVIRONMENT variable:
	/// ~32 collections in parallel on the dev box, ~4 on a GitHub runner.
	///
	/// That gap is not theoretical — it is exactly how #1192 escaped. The CI run on the red master
	/// (b670073f) honestly executed all 888 tests and reported 883 pass / 0 fail; the same commit was
	/// red locally. CI was not lying and it was not skipping tests: the race simply needs heavy
	/// interleaving to surface, and a 4-core runner does not provide it.
	///
	/// Pinning the value makes parallelism reproducible across machines, so a concurrency defect has
	/// the same chance of surfacing in CI as on the machine that found it. It raises that chance —
	/// it does not guarantee any particular race reproduces.
	///
	/// This organ exists because the config's failure mode is SILENT: if the file stops being copied
	/// next to the assembly (a dropped <c>CopyToOutputDirectory</c> rule), xUnit finds nothing, falls
	/// back to core count, and every run stays green while the guarantee is gone. So the assertion is
	/// deliberately made against the file BESIDE THE TEST ASSEMBLY, never against the source tree —
	/// checking the source copy would pass in precisely the case this guard must catch.
	/// </summary>
	public class TestParallelismPinnedGuardTests
	{
		private const string ConfigName = "xunit.runner.json";

		private static string ConfigBesideAssembly() =>
			Path.Combine(AppContext.BaseDirectory, ConfigName);

		[Fact]
		public void RunnerConfig_IsCopiedNextToTheTestAssembly()
		{
			var path = ConfigBesideAssembly();

			File.Exists(path).Should().BeTrue(
				$"xUnit only reads '{ConfigName}' from the test assembly's own directory " +
				$"('{AppContext.BaseDirectory}'). If it is missing here, the pin is inert and " +
				"parallelism silently reverts to the machine's core count — green, and unguarded.");
		}

		[Fact]
		public void RunnerConfig_PinsParallelismToAFixedValue()
		{
			using var doc = JsonDocument.Parse(File.ReadAllText(ConfigBesideAssembly()));
			var root = doc.RootElement;

			root.TryGetProperty("maxParallelThreads", out var max).Should().BeTrue(
				"an absent 'maxParallelThreads' is the defaulted, machine-dependent behaviour this guard exists to forbid");

			max.ValueKind.Should().Be(JsonValueKind.Number,
				"the value must be a fixed count; 'default' and 'unlimited' both re-introduce the core-count dependency");
			max.GetInt32().Should().BeGreaterThan(1,
				"pinning to 1 would serialise the suite and hide races instead of surfacing them");

			root.TryGetProperty("parallelizeTestCollections", out var parallelize).Should().BeTrue();
			parallelize.GetBoolean().Should().BeTrue(
				"collections must still run in parallel — the pin fixes HOW MANY, not WHETHER");
		}

		/// <summary>
		/// Inverse control, wired into the organ: the assertions above must actually REJECT the
		/// shapes that reintroduce machine-dependent parallelism. A guard never witnessed failing
		/// is indistinguishable from one that cannot fail.
		/// </summary>
		[Theory]
		[InlineData("{}", "no pin at all")]
		[InlineData(@"{""maxParallelThreads"": ""default""}", "the string 'default'")]
		[InlineData(@"{""maxParallelThreads"": ""unlimited""}", "the string 'unlimited'")]
		[InlineData(@"{""maxParallelThreads"": 1}", "serialised to a single thread")]
		public void Guard_RejectsConfigsThatUnpinParallelism(string json, string because)
		{
			using var doc = JsonDocument.Parse(json);
			var root = doc.RootElement;

			var pinned = root.TryGetProperty("maxParallelThreads", out var max)
			             && max.ValueKind == JsonValueKind.Number
			             && max.GetInt32() > 1;

			pinned.Should().BeFalse($"a config with {because} must not read as pinned");
		}

		[Fact]
		public void Guard_AcceptsAProperlyPinnedConfig()
		{
			using var doc = JsonDocument.Parse(@"{""maxParallelThreads"": 32, ""parallelizeTestCollections"": true}");
			var root = doc.RootElement;

			root.GetProperty("maxParallelThreads").GetInt32().Should().BeGreaterThan(1);
			root.GetProperty("parallelizeTestCollections").GetBoolean().Should().BeTrue();
		}
	}
}
