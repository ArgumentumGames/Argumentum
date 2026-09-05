using System;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// #569/#1274 organ: the Mindmapper FreeMind pass must refuse to start when the interactive
	/// desktop has no foreground window. Before the guard, a detached/minimized RDP session made
	/// every export fail silently (SendKeysSafe swallowing the Win32Exception, the loop skipping
	/// to the next document) and the pipeline exited 0 over stale SVGs — three silent fallbacks
	/// in a row. The guard's failure is demonstrated here by injection (control inverse: a guard
	/// never seen failing is a no-op), and the message must carry the remedy, not just the cause.
	/// </summary>
	public class InteractiveForegroundGuardTests
	{
		[Fact]
		public void NoForegroundWindow_FailsLoudWithRemedy()
		{
			var act = () => Argumentum.AssetConverter.Mindmapper.InteractiveForegroundGuard
				.EnsureForegroundWindowExists(() => IntPtr.Zero);

			act.Should().Throw<InvalidOperationException>()
				.Which.Message.Should()
				.Contain("GetForegroundWindow() == 0").And
				.Contain("stale SVGs").And
				.Contain("tscon").And
				.Contain("query session");
		}

		[Fact]
		public void ForegroundWindowExists_DoesNotThrow()
		{
			var act = () => Argumentum.AssetConverter.Mindmapper.InteractiveForegroundGuard
				.EnsureForegroundWindowExists(() => new IntPtr(0x10124));

			act.Should().NotThrow();
		}
	}
}
