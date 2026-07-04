using System;
using System.Threading;
using System.Threading.Tasks;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
	/// <summary>
	/// Contract pin for <see cref="HarvestManager.RetryAsync"/> — dispatch #98vo07 secondaire
	/// (gap DoD #613). The retry-serial fix (#676 / issue #613 Option C) extracted its retry/backoff
	/// contract into the pure, deterministic, <c>internal static</c> <see cref="HarvestManager.RetryAsync"/>
	/// helper precisely so this contract is unit-testable without a browser or a Playwright harvest
	/// (precedent: <see cref="HarvestManager.ComputeExpectedImageCount"/>). These tests pin the
	/// contract additively:
	///  - success on the first attempt returns <c>true</c> and invokes the action exactly once;
	///  - success on the N-th attempt returns <c>true</c> and invokes the action N times;
	///  - permanent failure returns <c>false</c> and NEVER throws (the helper swallows the last
	///    exception so the caller's aggregate-error path can report the residual failed-set list);
	///  - the backoff is actually applied between attempts (and skipped when <c>TimeSpan.Zero</c>);
	///  - <c>attempts &lt; 1</c> is clamped to 1.
	/// The helper is reached in production via <c>RetryFailedHarvestSetsAsync</c> (the post-loop
	/// serial retry seam), which itself only fires when
	/// <c>WebBasedGeneratorConfig.HarvestSetRetryAttempts &gt; 0</c> and the parallel harvest loop
	/// left a non-empty <c>failedSets</c> bag (issue #614 path). See
	/// <c>docs/investigations/2026-07-04-retry-serial-smoke-test.md</c> for the static path analysis
	/// and the rationale for why a runtime smoke-test is deferred to the next release regen.
	/// </summary>
	public class HarvestManagerRetryAsyncTests
	{
		// ─────────────────────────────────────────────────────────────────────────────
		// (1) Success on the first attempt — returns true, action invoked exactly once,
		//     no backoff delay incurred.
		// ─────────────────────────────────────────────────────────────────────────────
		[Fact]
		public async Task SucceedsOnFirstAttempt_ReturnsTrue_AndInvokesOnce()
		{
			var invocations = 0;
			Func<Task> action = () =>
			{
				Interlocked.Increment(ref invocations);
				return Task.CompletedTask;
			};

			var result = await HarvestManager.RetryAsync(action, attempts: 3, TimeSpan.FromMilliseconds(50), "unit-first");

			result.Should().BeTrue("the action succeeded on the first attempt");
			invocations.Should().Be(1, "a successful first attempt must not retry");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (2) Success on the N-th attempt — returns true, action invoked exactly N times.
		//     Demonstrates the retry actually re-invokes the action after a failure.
		// ─────────────────────────────────────────────────────────────────────────────
		[Theory]
		[InlineData(2)]
		[InlineData(3)]
		public async Task SucceedsOnNthAttempt_ReturnsTrue_AndInvokesNTimes(int successOnAttempt)
		{
			var invocations = 0;
			Func<Task> action = () =>
			{
				var current = Interlocked.Increment(ref invocations);
				if (current < successOnAttempt)
				{
					throw new InvalidOperationException($"simulated failure {current}");
				}
				return Task.CompletedTask;
			};

			var result = await HarvestManager.RetryAsync(action, attempts: successOnAttempt + 2, TimeSpan.Zero, "unit-nth");

			result.Should().BeTrue($"the action succeeded on attempt {successOnAttempt}");
			invocations.Should().Be(successOnAttempt, "retry must stop at the first success");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (3) Permanent failure — returns false, invokes exactly `attempts` times, and
		//     NEVER throws. This is the critical non-throwing guarantee: the caller
		//     (RetryFailedHarvestSetsAsync) relies on a bool, not an exception, to build
		//     its residual bag so the [HARVEST-PARTIAL] aggregate-error path can report
		//     the still-failing sets instead of aborting on the first residual failure.
		// ─────────────────────────────────────────────────────────────────────────────
		[Fact]
		public async Task AlwaysFails_ReturnsFalse_DoesNotThrow_InvokesAttemptTimes()
		{
			var invocations = 0;
			Func<Task> alwaysFails = () =>
			{
				Interlocked.Increment(ref invocations);
				throw new InvalidOperationException("permanent simulated failure");
			};

			Func<Task<bool>> act = () => HarvestManager.RetryAsync(alwaysFails, attempts: 4, TimeSpan.Zero, "unit-fail");

			var result = await act.Should().NotThrowAsync("the helper must swallow the last exception, not propagate it");
			result.Subject.Should().BeFalse("every attempt failed");
			invocations.Should().Be(4, "the action must be attempted exactly `attempts` times before giving up");
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (4) Backoff is applied between attempts. Two checks:
		//   (a) backoff = Zero → no delay, the total elapsed stays well under any positive
		//       backoff (fast path, no accidental stall);
		//   (b) backoff > 0 with 3 attempts (2 inter-attempt delays) → elapsed >= 2*backoff.
		//   The margin is generous to stay robust on a loaded CI box.
		// ─────────────────────────────────────────────────────────────────────────────
		[Fact]
		public async Task BackoffZero_DoesNotDelay()
		{
			var invocations = 0;
			Func<Task> failsTwice = () =>
			{
				var current = Interlocked.Increment(ref invocations);
				return current >= 3 ? Task.CompletedTask : throw new InvalidOperationException("fail");
			};

			var sw = System.Diagnostics.Stopwatch.StartNew();
			var result = await HarvestManager.RetryAsync(failsTwice, attempts: 3, TimeSpan.Zero, "unit-no-delay");
			sw.Stop();

			result.Should().BeTrue();
			sw.ElapsedMilliseconds.Should().BeLessThan(500, "Zero backoff must not introduce any delay");
		}

		[Fact]
		public async Task BackoffPositive_AppliesDelayBetweenAttempts()
		{
			var invocations = 0;
			Func<Task> failsTwice = () =>
			{
				var current = Interlocked.Increment(ref invocations);
				return current >= 3 ? Task.CompletedTask : throw new InvalidOperationException("fail");
			};

			var backoff = TimeSpan.FromMilliseconds(120);
			var sw = System.Diagnostics.Stopwatch.StartNew();
			var result = await HarvestManager.RetryAsync(failsTwice, attempts: 3, backoff, "unit-delay");
			sw.Stop();

			result.Should().BeTrue();
			// 3 attempts => 2 inter-attempt delays => >= 2 * backoff.
			sw.Elapsed.Should().BeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(2 * 120 - 30),
				"two inter-attempt delays of {0} must be applied", backoff);
			// Generous upper bound to stay robust on a loaded box.
			sw.Elapsed.Should().BeLessThanOrEqualTo(TimeSpan.FromMilliseconds(2 * 120 + 2000));
		}

		// ─────────────────────────────────────────────────────────────────────────────
		// (5) attempts < 1 is clamped to 1 — defensive contract. The action is invoked
		//     exactly once (no off-by-one infinite loop, no zero-invocation silent return).
		// ─────────────────────────────────────────────────────────────────────────────
		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		public async Task AttemptsBelowOne_ClampedToOne_InvokesOnce(int invalidAttempts)
		{
			var invocations = 0;
			Func<Task> action = () =>
			{
				Interlocked.Increment(ref invocations);
				return Task.CompletedTask;
			};

			var result = await HarvestManager.RetryAsync(action, invalidAttempts, TimeSpan.Zero, "unit-clamp");

			result.Should().BeTrue("the clamped single attempt succeeded");
			invocations.Should().Be(1, "attempts < 1 must be clamped to exactly one invocation");
		}
	}
}
