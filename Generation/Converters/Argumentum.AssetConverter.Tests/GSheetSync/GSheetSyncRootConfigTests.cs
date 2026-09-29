using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Argumentum.AssetConverter.GSheetSync;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
	/// <summary>
	/// #1618 point 2 — a failing corpus must not stop the following ones, and the
	/// aggregated failure must still surface to the caller.
	/// </summary>
	public class GSheetSyncRootConfigTests
	{
		private static GSheetSyncConfig Corpus(string name, bool enabled = true)
			=> new GSheetSyncConfig { Name = name, Enabled = enabled };

		[Fact]
		public async Task RunAll_OneCorpusFails_OthersStillRun_ThenAggregates()
		{
			var ran = new List<string>();

			var ex = await Assert.ThrowsAsync<AggregateException>(() =>
				GSheetSyncRootConfig.RunAllAsync(
					new[] { Corpus("A"), Corpus("B"), Corpus("C") },
					c =>
					{
						ran.Add(c.Name);
						if (c.Name == "B")
						{
							throw new InvalidOperationException("boom");
						}
						return Task.CompletedTask;
					}));

			ran.Should().Equal(new[] { "A", "B", "C" },
				"every corpus must still be attempted");
			ex.InnerExceptions.Should().ContainSingle()
				.Which.Should().BeOfType<InvalidOperationException>();
			ex.Message.Should().Contain("B");
		}

		[Fact]
		public async Task RunAll_AllSucceed_CompletesWithoutThrowing()
		{
			var ran = new List<string>();

			await GSheetSyncRootConfig.RunAllAsync(
				new[] { Corpus("A"), Corpus("B") },
				c => { ran.Add(c.Name); return Task.CompletedTask; });

			ran.Should().Equal("A", "B");
		}

		[Fact]
		public async Task RunAll_DisabledCorpus_IsSkipped()
		{
			var ran = new List<string>();

			await GSheetSyncRootConfig.RunAllAsync(
				new[] { Corpus("A"), Corpus("B", enabled: false), Corpus("C") },
				c => { ran.Add(c.Name); return Task.CompletedTask; });

			ran.Should().Equal("A", "C");
		}
	}
}