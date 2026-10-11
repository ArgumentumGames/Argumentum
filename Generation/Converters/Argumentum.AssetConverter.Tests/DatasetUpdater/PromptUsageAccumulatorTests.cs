using System;
using System.Linq;
using System.Threading.Tasks;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.DatasetUpdater
{
	/// <summary>
	/// Contract for <see cref="PromptUsage"/>, the run-scoped API usage accumulator behind the
	/// T2 volume condition (#458 c.6107217348): "usage par appel journalisé (jetons, coût) — le coût
	/// se mesure, il ne s'estime plus". The accumulator feeds the end-of-run cost line, so its
	/// arithmetic inputs (call count, token totals) must be exact under the campaign's real
	/// concurrency (chunks run with MaxDegreeOfParallelismWebService > 1).
	/// </summary>
	public class PromptUsageAccumulatorTests
	{
		[Fact]
		public void Add_then_Snapshot_sums_calls_and_tokens()
		{
			PromptUsage.Reset();
			PromptUsage.Add(1000, 200);
			PromptUsage.Add(300, 50);

			var (calls, input, output) = PromptUsage.Snapshot();

			calls.Should().Be(2);
			input.Should().Be(1300);
			output.Should().Be(250);
		}

		[Fact]
		public void Reset_zeroes_the_accumulator()
		{
			PromptUsage.Reset();
			PromptUsage.Add(12, 3);
			PromptUsage.Reset();

			PromptUsage.Snapshot().Should().Be((0, 0, 0),
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
					PromptUsage.Add(10, 2);
				}
			})));

			var (calls, input, output) = PromptUsage.Snapshot();
			calls.Should().Be(workers * addsPerWorker,
				"every billable call counts — a lost Add is a call whose cost the run summary never sees");
			input.Should().Be(workers * addsPerWorker * 10);
			output.Should().Be(workers * addsPerWorker * 2);
		}
	}
}
