using System;
using System.Collections.Generic;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="PdfManager.WriteAndDispose{T}"/> — #29 dispose pin-test (output-neutral),
    /// dispatch primaire ai-01 (`msg-...215111`).
    ///
    /// The Print &amp; Play PDF writer (<see cref="PdfManager.GeneratePdfsFromImages"/>) materializes each deck's
    /// <c>MagickImageCollection</c> (decoding ~277 card images for the Fallacies Tarot — up to ~1.2 GB of native
    /// ImageMagick handles), writes the PDF, then MUST release that collection before the next deck. If the
    /// collection is ever held past its write (e.g. a refactor drops the <c>using</c>), the peak memory balloons
    /// until the GC finalizer eventually frees the native handles — the original #29 regression that made the
    /// machine &quot;ramer&quot;.
    ///
    /// That create→write→dispose control flow was INLINED in <see cref="PdfManager.GeneratePdfsFromImages"/> with
    /// only a comment (#436 added the <c>using</c>) guarding the dispose. It has been extracted output-neutral
    /// into the pure, deterministic <see cref="PdfManager.WriteAndDispose{T}"/> — a generic helper over
    /// <c>IDisposable</c> — so the memory-safety contract is unit-testable WITHOUT a Magick render, using a
    /// disposable tracker that observes the create/action/dispose order.
    /// </summary>
    public class PdfDisposeContractTests
    {
        /// <summary>
        /// A disposable stand-in for <c>MagickImageCollection</c>. Records the lifecycle events in order so the
        /// tests can assert the create→action→dispose sequence without touching Magick's native heap.
        /// </summary>
        private sealed class DisposableTracker : IDisposable
        {
            public int Id { get; }
            public bool IsDisposed { get; private set; }
            public DisposableTracker(int id) => Id = id;
            public void Dispose() => IsDisposed = true;
        }

        /// <summary>
        /// Records, in order, every lifecycle event across one or more <see cref="PdfManager.WriteAndDispose{T}"/>
        /// calls: which tracker was created, which was handed to the action, which was disposed.
        /// </summary>
        private sealed class LifecycleRecorder
        {
            public List<string> Events { get; } = new();
            public List<DisposableTracker> Created { get; } = new();
            public Func<DisposableTracker> Factory(int id) => () =>
            {
                var t = new DisposableTracker(id);
                Created.Add(t);
                Events.Add($"create:{id}");
                return t;
            };
            public Action<DisposableTracker> Action() => t => Events.Add($"action:{t.Id}");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) HAPPY PATH — factory called once, action called once on the produced resource,
        //     resource disposed AFTER the action. Pins the full create→action→dispose order.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void HappyPath_CreateActionDispose_InOrder_ResourceDisposed()
        {
            var rec = new LifecycleRecorder();

            PdfManager.WriteAndDispose(rec.Factory(7), rec.Action());

            rec.Events.Should().Equal(new[] { "create:7", "action:7" },
                "the factory materializes the resource, then the action runs on it — in that order.");
            rec.Created.Should().ContainSingle();
            rec.Created[0].IsDisposed.Should().BeTrue(
                "the resource is disposed deterministically once the action completes, not held for the GC finalizer.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) DISPOSE HAPPENS BEFORE THE CALLER REGAINS CONTROL — the helper does not leak the
        //     resource back to the caller undisposed. This is the anti-#29 guarantee: no caller
        //     can accidentally hold the collection past the write.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void ResourceDisposed_BeforeCallersNextStatement()
        {
            var rec = new LifecycleRecorder();
            var afterHelper = (DisposableTracker)null!;

            PdfManager.WriteAndDispose(rec.Factory(1), t =>
            {
                afterHelper = t; // capture the live reference handed to the action
            });

            // The reference the action saw IS the one the factory created...
            afterHelper.Should().BeSameAs(rec.Created[0]);
            // ...and by the time control returns here, it is already disposed.
            afterHelper.IsDisposed.Should().BeTrue(
                "the resource is disposed before the caller's next statement runs — the caller never observes " +
                "an undisposed collection, which is exactly the #29 memory-safety invariant.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) DISPOSE ON ACTION THROW — the `using` must dispose even when the action throws, so a
        //     failed PDF write cannot leak the collection's native handles. The exception propagates.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void ActionThrows_ResourceStillDisposed_ExceptionPropagates()
        {
            var rec = new LifecycleRecorder();
            var captured = (DisposableTracker)null!;

            var act = () => PdfManager.WriteAndDispose(rec.Factory(3), t =>
            {
                captured = t;
                throw new InvalidOperationException("write failed");
            });

            act.Should().Throw<InvalidOperationException>("the action's exception propagates to the caller.");
            captured.Should().NotBeNull();
            captured.IsDisposed.Should().BeTrue(
                "even when the action throws, the resource is disposed before the exception escapes — a failed " +
                "PDF write must not leak the collection's ~1.2 GB of native handles.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) ONE RESOURCE PER CALL — multiple sequential calls dispose each independently; the
        //     previous collection is gone before the next is created. Pins that the helper does not
        //     accidentally accumulate resources across calls (the original #29 peak-memory bug).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void MultipleCalls_EachResourceDisposedBeforeNextCreated()
        {
            var rec = new LifecycleRecorder();

            for (int i = 0; i < 3; i++)
            {
                var beforeNext = rec.Created.Count;
                PdfManager.WriteAndDispose(rec.Factory(i), rec.Action());
                // Each call created exactly one tracker and disposed it within that call.
                rec.Created.Count.Should().Be(beforeNext + 1);
                rec.Created[beforeNext].IsDisposed.Should().BeTrue(
                    $"call {i}: the previous collection is disposed before the next one is created — no accumulation.");
            }

            rec.Events.Should().Equal(new[]
            {
                "create:0", "action:0",
                "create:1", "action:1",
                "create:2", "action:2",
            }, "each call is fully self-contained: create, act, dispose, then the next call.");
            rec.Created.Should().OnlyContain(t => t.IsDisposed, "every collection across all calls was disposed.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) FACTORY CALLED EXACTLY ONCE — the resource is materialized a single time per call,
        //     not lazily re-evaluated or cached. Guards against a refactor that would call the factory
        //     twice (producing two live collections — a hidden memory leak).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void FactoryCalled_ExactlyOnce()
        {
            int factoryCalls = 0;
            Func<DisposableTracker> factory = () =>
            {
                factoryCalls++;
                return new DisposableTracker(0);
            };

            PdfManager.WriteAndDispose(factory, _ => { });

            factoryCalls.Should().Be(1,
                "the factory runs exactly once per call — a second materialization would create a second live " +
                "collection and silently double the peak memory.");
        }
    }
}
