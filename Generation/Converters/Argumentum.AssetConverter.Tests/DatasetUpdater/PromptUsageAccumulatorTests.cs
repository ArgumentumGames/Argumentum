using System;
using System.Linq;
using System.Threading.Tasks;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;
using static Argumentum.AssetConverter.DatasetUpdaterConfig;

namespace Argumentum.AssetConverter.Tests.DatasetUpdater
{
	/// <summary>
	/// Contract for <see cref="PromptUsage"/>, the run-scoped API usage accumulator behind the
	/// T2 volume condition (#458 c.6107217348): "usage par appel journalisé (jetons, coût) — le coût
	/// se mesure, il ne s'estime plus". The accumulator feeds the end-of-run cost line, so its
	/// arithmetic inputs (call count, token totals) must be exact under the campaign's real
	/// concurrency (chunks run with MaxDegreeOfParallelismWebService > 1). Cached input tokens are
	/// accumulated separately (subset of input, billed cheaper — #1864 review) and priced by
	/// <see cref="ComputeRunCost"/>: exact with a declared cache price, UPPER BOUND without.
	/// </summary>
	public class PromptUsageAccumulatorTests
	{
		[Fact]
		public void Add_then_Snapshot_sums_calls_and_tokens()
		{
			PromptUsage.Reset();
			PromptUsage.Add(1000, 200);
			PromptUsage.Add(300, 50, cachedInputTokens: 300);

			var (calls, input, output, cached) = PromptUsage.Snapshot();

			calls.Should().Be(2);
			input.Should().Be(1300);
			output.Should().Be(250);
			cached.Should().Be(300, "cached tokens are a subset of input and must not add to it");
		}

		[Fact]
		public void Reset_zeroes_the_accumulator()
		{
			PromptUsage.Reset();
			PromptUsage.Add(12, 3, 4);
			PromptUsage.Reset();

			PromptUsage.Snapshot().Should().Be((0, 0, 0, 0),
				"a campaign must not inherit a previous campaign's usage — the cost line would be wrong by that amount");
		}

		[Fact]
		public async Task Concurrent_Adds_lose_nothing()
		{
			PromptUsage.Reset();
			const int workers = 8;
			const int addsPerWorker = 500;

			await Task.WhenAll(Enumerable.Range(0, workers).Select(_ => Task.Run(() =>
			{
				for (var i = 0; i < addsPerWorker; i++)
				{
					PromptUsage.Add(10, 2, 6);
				}
			})));

			var (calls, input, output, cached) = PromptUsage.Snapshot();
			calls.Should().Be(workers * addsPerWorker,
				"every billable call counts — a lost Add is a call whose cost the run summary never sees");
			input.Should().Be(workers * addsPerWorker * 10);
			output.Should().Be(workers * addsPerWorker * 2);
			cached.Should().Be(workers * addsPerWorker * 6);
		}

		[Fact]
		public void ComputeRunCost_without_prices_is_not_computed()
		{
			var report = ComputeRunCost(1000, 200, 400, null, 1.25, 0.125);

			report.Kind.Should().Be(RunCostKind.NotComputed,
				"a missing price must never read as a zero cost");
		}

		[Fact]
		public void ComputeRunCost_with_cache_price_is_exact()
		{
			// (600 fresh + 400 cached + 200 out) tokens at per-MILLION prices → $0.0028 exactly
			var report = ComputeRunCost(1000, 200, 400, 1.25, 10.0, 0.125);

			report.Kind.Should().Be(RunCostKind.Exact);
			report.Cost.Should().BeApproximately(0.0028, 1e-12);
			report.Detail.Should().Contain("cached @").And.Contain("x400");
		}

		[Fact]
		public void ComputeRunCost_with_cached_tokens_and_no_cache_price_is_upper_bound()
		{
			// Full-price figure: 1000 in @ $1.25/MTok + 200 out @ $10/MTok = $0.00325 — but 400 of
			// the input is cached and billed cheaper, so this overstates the true cost.
			var report = ComputeRunCost(1000, 200, 400, 1.25, 10.0, null);

			report.Kind.Should().Be(RunCostKind.UpperBound,
				"without a declared cache price the full-price total must be labelled, not passed off as exact");
			report.Cost.Should().BeApproximately(0.00325, 1e-12);
			report.Detail.Should().Contain("400 cached input tokens").And.Contain("CachedInputTokenPricePerMillion");
		}

		[Fact]
		public void ComputeRunCost_without_cached_tokens_is_exact_full_price()
		{
			var report = ComputeRunCost(1000, 200, 0, 1.25, 10.0, null);

			report.Kind.Should().Be(RunCostKind.Exact);
			report.Cost.Should().BeApproximately(0.00325, 1e-12);
		}

		[Fact]
		public void ComputeRunCost_clamps_cached_into_input()
		{
			// A provider can never report more cached than input; if it somehow did, the fresh
			// input share must not go negative.
			var report = ComputeRunCost(500, 100, 900, 1.0, 1.0, 0.5);

			report.Kind.Should().Be(RunCostKind.Exact);
			// 0 fresh + 500 cached @ $0.5/MTok + 100 out @ $1/MTok = $0.00025 + $0.0001 = $0.00035
			report.Cost.Should().BeApproximately(0.00035, 1e-12);
		}
	}
}
