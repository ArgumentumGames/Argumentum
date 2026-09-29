using System;
using System.Collections.Generic;
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
	///  - the backoff is REQUESTED between attempts — exactly twice for three attempts, each time
	///    with the configured value — and never requested when <c>TimeSpan.Zero</c> (#1659:
	///    observed through the injected delay seam, because a wall-clock bound is blind to the guard);
	///  - the default path (no injection) still waits the real <c>Task.Delay</c>;
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
		// (4) Backoff is applied between attempts — split by what each instrument can see.
		//   (a) backoff = Zero → the delay is NEVER REQUESTED. Observed through the #1659
		//       injection seam, never through a stopwatch: `Task.Delay(TimeSpan.Zero)` returns
		//       almost immediately, so a wall-clock bound cannot tell "the guard fired" from
		//       "the guard was removed". The assertion was blind to the branch it names, and
		//       load-flaky besides — measured 2026-09-29 (#1659): 1033 ms against the 500 ms
		//       bound on a runner shared with five Dependabot runs, green on re-run, same SHA.
		//   (b) backoff > 0 with 3 attempts → the injected delay is called exactly twice, each
		//       time with the configured backoff.
		//   (c) the default path (production passes no `delay`) still waits the REAL
		//       `Task.Delay`: the seam must not silently default to a no-op, and (b) cannot see
		//       that because it injects.
		//
		//   All positive checks assert a LOWER bound on wall clock only, deliberately. An upper
		//   bound on wall-clock is not the contract of a backoff — "the delay was applied" is.
		//   It used to also assert elapsed <= 2*backoff + 2000ms, which made the test flaky the
		//   moment CI began running tests for real (#911): measured 2026-07-26, this test took
		//   3 s and failed the 2 240 ms bound on PRs #928/#935 while 24 sibling runs passed —
		//   26 dependabot runs firing inside 5 minutes contend for the runner, and the diffs in
		//   question touched only vendored npm lockfiles, so no production code could be
		//   implicated. Do not restore an upper bound here: a bigger number only moves the load
		//   level at which it lies. The lower bound still fails hard if the backoff is skipped.
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

			var delayCalls = 0;
			Func<TimeSpan, Task> forbiddenDelay = _ =>
			{
				Interlocked.Increment(ref delayCalls);
				return Task.CompletedTask;
			};

			var result = await HarvestManager.RetryAsync(failsTwice, attempts: 3, TimeSpan.Zero, "unit-no-delay", forbiddenDelay);

			result.Should().BeTrue();
			invocations.Should().Be(3, "the two simulated failures must still be retried");
			delayCalls.Should().Be(0,
				"a zero backoff must never REQUEST a delay: the guard is the contract, and removing " +
				"it has to redden this assertion (#1659)");
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

			// Pass-through recorder: counts the requests AND delegates to the real Task.Delay, so
			// the elapsed lower bound below still measures the production delay path.
			var recorded = new List<TimeSpan>();
			Func<TimeSpan, Task> recordingDelay = span =>
			{
				recorded.Add(span);
				return Task.Delay(span);
			};

			var backoff = TimeSpan.FromMilliseconds(120);
			var sw = System.Diagnostics.Stopwatch.StartNew();
			var result = await HarvestManager.RetryAsync(failsTwice, attempts: 3, backoff, "unit-delay", recordingDelay);
			sw.Stop();

			result.Should().BeTrue();
			// 3 attempts => 2 inter-attempt delays, each of exactly the configured backoff.
			recorded.Should().HaveCount(2, "3 attempts must request exactly 2 inter-attempt delays");
			recorded.Should().OnlyContain(span => span == backoff,
				"each requested delay must be the configured backoff");
			sw.Elapsed.Should().BeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(2 * 120 - 30),
				"two inter-attempt delays of {0} must be applied", backoff);
			// No upper bound: see the note above (a) / (b). Runner contention is not a defect
			// of RetryAsync, and an upper bound cannot distinguish the two.
		}

		[Fact]
		public async Task DefaultDelayPath_StillWaitsTheRealBackoff()
		{
			// (c) Every other test injects, so a default that silently became a no-op would
			// delete the backoff from the pipeline — RetryFailedHarvestSetsAsync passes no
			// `delay` — without reddening anything else. Wall-clock LOWER bound only, for the
			// reasons in (a).
			var invocations = 0;
			Func<Task> failsTwice = () =>
			{
				var current = Interlocked.Increment(ref invocations);
				return current >= 3 ? Task.CompletedTask : throw new InvalidOperationException("fail");
			};

			var backoff = TimeSpan.FromMilliseconds(120);
			var sw = System.Diagnostics.Stopwatch.StartNew();
			var result = await HarvestManager.RetryAsync(failsTwice, attempts: 3, backoff, "unit-default-delay");
			sw.Stop();

			result.Should().BeTrue();
			sw.Elapsed.Should().BeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(2 * 120 - 30),
				"the default delay must be the real Task.Delay, not a no-op");
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
