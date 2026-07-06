using System;
using System.Collections.Generic;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Collections
{
    /// <summary>
    /// Contract pin for <see cref="EqualityComparerFactory.Create{T}"/> — #204 coverage sweep
    /// (cont. po-2024): the delegate-routing contract for lambda-built equality comparers.
    ///
    /// <see cref="EqualityComparerFactory.Create{T}"/> wraps two lambdas — a <c>getHashCode</c> and an
    /// <c>equals</c> — into an <see cref="IEqualityComparer{T}"/>. That comparer is then handed to LINQ
    /// dedup operations. The sole production caller builds <see cref="CardSetJob.Comparer"/> (dedup
    /// CardSetJobs by <c>Name</c>), which <c>HarvestManager</c> feeds to
    /// <c>.Distinct(CardSetJob.Comparer)</c> ([HarvestManager.cs:104]). So a bug here is a bug in the
    /// CardSet harvest dedup: a swapped wiring (the private <c>Comparer&lt;T&gt;</c> routing
    /// <c>Equals</c>→<c>getHashCode</c> lambda and <c>GetHashCode</c>→<c>equals</c> lambda) would
    /// silently collapse distinct CardSets into one, or fail to dedup duplicates — producing duplicate
    /// or missing harvests with no exception.
    ///
    /// The factory is pure &amp; deterministic (no I/O, no state beyond the captured delegates) and had
    /// ZERO isolated coverage (the harvest path exercises it only indirectly, through a successful run).
    /// These tests pin three contracts additively (no production code changed):
    /// <list type="bullet">
    /// <item>The returned comparer routes <see cref="IEqualityComparer{T}.Equals"/> to the
    /// <c>equals</c> delegate and <see cref="IEqualityComparer{T}.GetHashCode"/> to the
    /// <c>getHashCode</c> delegate — NOT swapped.</item>
    /// <item>Both null-delegate arguments throw <see cref="ArgumentNullException"/> (not a later
    /// <see cref="NullReferenceException"/> from inside the invoked lambda).</item>
    /// <item>The comparer genuinely functions for dedup when fed to LINQ <c>Distinct</c>/<c>Contains</c>
    /// — the end-to-end use that <c>HarvestManager</c> relies on.</item>
    /// </list>
    /// </summary>
    public class EqualityComparerFactoryContractTests
    {
        /// <summary>
        /// Minimal synthetic record used to exercise the factory in isolation, decoupled from any
        /// domain type that evolves. The <c>Id</c>-based equality below mirrors the real
        /// <c>CardSetJob.Comparer</c> pattern (dedup-by-a-single-string-field).
        /// </summary>
        private sealed class Sample
        {
            public string Id { get; init; }
        }

        // A real, observable getHashCode/equals pair keyed on Id. If the factory ever swaps the two
        // delegates, these callbacks are instrumented so the wrong one fires and the test sees it.
        private sealed class TrackedDelegates
        {
            public int GetHashCodeCalls;
            public int EqualsCalls;
            public new Func<Sample, int> GetHashCode { get; }
            public new Func<Sample, Sample, bool> Equals { get; }

            public TrackedDelegates()
            {
                GetHashCode = s => { GetHashCodeCalls++; return s.Id?.GetHashCode() ?? 0; };
                Equals = (a, b) => { EqualsCalls++; return a.Id == b.Id; };
            }
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) DELEGATE ROUTING — GetHashCode routes to the getHashCode delegate (NOT equals). A swapped
        //     wiring in the private Comparer<T> would route GetHashCode to the equals lambda, which has
        //     a different signature and wouldn't even compile — but a subtler swap (field assignment
        //     flipped) would route it to the wrong value. Pinned by counting which delegate fires.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void GetHashCode_RoutesToGetHashCodeDelegate_NotSwapped()
        {
            var del = new TrackedDelegates();
            var comparer = EqualityComparerFactory.Create<Sample>(del.GetHashCode, del.Equals);
            var sample = new Sample { Id = "x" };

            var hash = comparer.GetHashCode(sample);

            del.GetHashCodeCalls.Should().Be(1, "GetHashCode must invoke the getHashCode delegate exactly once.");
            del.EqualsCalls.Should().Be(0, "GetHashCode must NOT route to the equals delegate (swapped wiring would).");
            hash.Should().Be(("x").GetHashCode(),
                "the getHashCode delegate's return value is surfaced verbatim as the comparer's hash.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) DELEGATE ROUTING — Equals routes to the equals delegate (NOT getHashCode). The symmetric
        //     half of (1): the two interface methods must map to their SAME-nAMED delegate, never crossed.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Equals_RoutesToEqualsDelegate_NotSwapped()
        {
            var del = new TrackedDelegates();
            var comparer = EqualityComparerFactory.Create<Sample>(del.GetHashCode, del.Equals);
            var a = new Sample { Id = "x" };
            var b = new Sample { Id = "x" };

            var result = comparer.Equals(a, b);

            del.EqualsCalls.Should().Be(1, "Equals must invoke the equals delegate exactly once.");
            del.GetHashCodeCalls.Should().Be(0, "Equals must NOT route to the getHashCode delegate (swapped wiring would).");
            result.Should().BeTrue("the equals delegate returns true for equal Ids and that is surfaced verbatim.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) EQUALS semantics — the equals delegate fully decides equality. Same Id ⇒ equal (true);
        //     different Id ⇒ not equal (false). This is the HarvestManager dedup contract: two
        //     CardSetJobs with the same Name are treated as the same job.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Equals_ReturnsDelegateVerdict_SameVsDifferentId()
        {
            var del = new TrackedDelegates();
            var comparer = EqualityComparerFactory.Create<Sample>(del.GetHashCode, del.Equals);

            comparer.Equals(new Sample { Id = "A" }, new Sample { Id = "A" }).Should().BeTrue(
                "two samples with the same Id are equal per the delegate (the dedup contract).");
            comparer.Equals(new Sample { Id = "A" }, new Sample { Id = "B" }).Should().BeFalse(
                "two samples with different Ids are not equal per the delegate.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) NULL GUARDS — each null delegate throws ArgumentNullException up front (NOT a later
        //     NullReferenceException leaking from inside the invoked lambda). Callers that guard on
        //     ArgumentNullException specifically rely on this.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void NullGetHashCode_ThrowsArgumentNullException_NotDeferredNre()
        {
            var act = () => EqualityComparerFactory.Create<Sample>(null!, (a, b) => a.Id == b.Id);

            act.Should().Throw<ArgumentNullException>(
                "a null getHashCode delegate is rejected eagerly with ArgumentNullException, not deferred " +
                "to a NullReferenceException inside the lambda at invocation time.");
        }

        [Fact]
        public void NullEquals_ThrowsArgumentNullException_NotDeferredNre()
        {
            var act = () => EqualityComparerFactory.Create<Sample>(s => s.Id?.GetHashCode() ?? 0, null!);

            act.Should().Throw<ArgumentNullException>(
                "a null equals delegate is rejected eagerly with ArgumentNullException, not deferred " +
                "to a NullReferenceException inside the lambda at invocation time.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) VALID COMPARER does NOT throw when both delegates are non-null — the guards are
        //     independent (one bad delegate doesn't let the other's null slip through, but two good
        //     delegates produce a usable comparer).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void ValidDelegates_ReturnsNonNullComparer()
        {
            var comparer = EqualityComparerFactory.Create<Sample>(
                s => s.Id?.GetHashCode() ?? 0,
                (a, b) => a.Id == b.Id);

            comparer.Should().NotBeNull(
                "two non-null delegates always yield a usable, non-null comparer instance.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (6) END-TO-END DEDUP via LINQ Distinct — the actual use HarvestManager makes of the comparer.
        //     Duplicate-Id samples collapse to one; the distinct set preserves the originals of unique
        //     Ids. A swapped or broken comparer would either drop unique items (hash collisions) or fail
        //     to collapse duplicates (equals never fires correctly).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Distinct_CollapsesDuplicateIds_KeepsUniques()
        {
            var comparer = EqualityComparerFactory.Create<Sample>(
                s => s.Id?.GetHashCode() ?? 0,
                (a, b) => a.Id == b.Id);

            var jobs = new List<Sample>
            {
                new() { Id = "Fallacies" },
                new() { Id = "Virtues" },
                new() { Id = "Fallacies" }, // duplicate by Id
                new() { Id = "Scenarii" },
                new() { Id = "Virtues" },   // duplicate by Id
            };

            var distinct = jobs.Distinct(comparer).ToList();

            distinct.Should().HaveCount(3,
                "Distinct with the Id-keyed comparer collapses the two duplicate pairs, leaving one job " +
                "per unique Id — the exact dedup HarvestManager performs on the CardSetJob list.");
            distinct.Select(s => s.Id).Should().BeEquivalentTo(
                new[] { "Fallacies", "Virtues", "Scenarii" },
                "the survivor of each duplicate group is the first occurrence, and unique Ids are preserved.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (7) DETERMINISM — the same delegates yield a comparer whose Equals/GetHashCode are stable
        //     across repeated calls (no hidden state mutation in the factory or the returned comparer).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Deterministic_SameInputs_StableResults()
        {
            var comparer = EqualityComparerFactory.Create<Sample>(
                s => s.Id?.GetHashCode() ?? 0,
                (a, b) => a.Id == b.Id);
            var a = new Sample { Id = "A" };
            var b = new Sample { Id = "A" };

            var firstEquals = comparer.Equals(a, b);
            var firstHash = comparer.GetHashCode(a);
            var secondEquals = comparer.Equals(a, b);
            var secondHash = comparer.GetHashCode(a);

            firstEquals.Should().Be(secondEquals,
                "Equals is deterministic — same inputs, same verdict across calls.");
            firstHash.Should().Be(secondHash,
                "GetHashCode is deterministic — same input, same hash across calls.");
        }
    }
}
