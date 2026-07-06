using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Regression tests for <see cref="DataSetInfo.GetHierarchicalRecords"/> grouping used by the
    /// DatasetUpdater <c>DivisionMode.PKHierarchicalChar</c> path.
    ///
    /// Issue #409 Bug 1: at <c>PKHierarchyLevel &gt;= 2</c> the root selection required a PK to have
    /// EXACTLY <c>level-1</c> separators. A dot-less top-level PK ("0".."7" on Virtues' <c>path</c>
    /// column) has 0 separators, so it was never selected as a root, and — having no separator — could
    /// not be picked up as a child of any deeper root via <c>StartsWith</c> either. Those records were
    /// silently dropped from every group and never sent to the model. These tests assert that every
    /// input record is now covered by at least one group at level &gt;= 2.
    /// </summary>
    public class PKHierarchicalGroupingTests
    {
        private static List<Dictionary<string, object>> BuildRecords(params string[] paths) =>
            paths.Select(p => new Dictionary<string, object> { ["path"] = (object)p }).ToList();

        private static HashSet<string> CoveredPks(List<List<Dictionary<string, object>>> groups) =>
            groups.SelectMany(group => group)
                .Select(record => record["path"].ToString()!)
                .ToHashSet();

        [Fact]
        public void Level2_DotLessTerminalRoot_IsIncluded()
        {
            // "0" has no children: pure terminal dot-less root. Before the fix it was orphaned.
            var paths = new[] { "0", "1", "1.1", "1.2" };
            var records = BuildRecords(paths);

            var groups = DataSetInfo.GetHierarchicalRecords(records, "path", '.', 2, includeChildren: true, maxChildren: 12);

            CoveredPks(groups).Should().Contain("0");
        }

        [Fact]
        public void Level2_DotLessRootsWithChildren_AreAllCovered()
        {
            // Mirrors the Virtues "path" column shape: dot-less roots "0".."3" plus their subtrees.
            // Before the fix the parent walk skipped the dot-less parents entirely, dropping "0".."3".
            var paths = new[]
            {
                "0",
                "1", "1.1", "1.1.1", "1.1.2", "1.2", "1.2.1", "1.3",
                "2", "2.1", "2.1.1", "2.2",
                "3", "3.1",
            };
            var records = BuildRecords(paths);

            var groups = DataSetInfo.GetHierarchicalRecords(records, "path", '.', 2, includeChildren: true, maxChildren: 12);

            var covered = CoveredPks(groups);
            var missing = paths.Where(p => !covered.Contains(p)).ToList();

            missing.Should().BeEmpty("every record must reach the model at level >= 2 (issue #409)");
            covered.Should().Contain(new[] { "0", "1", "2", "3" });
        }

        [Fact]
        public void Level3_DotLessAndShallowRoots_AreAllCovered()
        {
            // At level 3 the standard roots require exactly 2 separators. Dot-less roots ("0") and
            // one-dot shallow terminals ("4.1" with no children) must still be covered.
            var paths = new[]
            {
                "0",
                "1", "1.1", "1.1.1", "1.1.2", "1.2", "1.2.1", "1.2.1.1",
                "4", "4.1",
            };
            var records = BuildRecords(paths);

            var groups = DataSetInfo.GetHierarchicalRecords(records, "path", '.', 3, includeChildren: true, maxChildren: 12);

            var covered = CoveredPks(groups);
            var missing = paths.Where(p => !covered.Contains(p)).ToList();

            missing.Should().BeEmpty("every record must reach the model at level >= 3 (issue #409)");
            covered.Should().Contain(new[] { "0", "4", "4.1" });
        }

        [Fact]
        public void AllRecordsAreCovered_DoesNotThrowOnDotLessRoot()
        {
            // Regression for the latent crash exposed when a dot-less root entered the parent walk:
            // LastIndexOf('.') == -1 made Substring(0, -1) throw. Must not throw now.
            var records = BuildRecords("0", "1", "1.1");

            var act = () => DataSetInfo.GetHierarchicalRecords(records, "path", '.', 2, includeChildren: true, maxChildren: 12);

            act.Should().NotThrow();
        }

        [Fact]
        public void Level1_StandardBehaviorUnchanged_AllRootsCovered()
        {
            // At level 1 every record with 0 separators is a standard root; deeper records are children.
            var paths = new[] { "0", "1", "1.1", "2", "2.1", "2.1.1" };
            var records = BuildRecords(paths);

            var groups = DataSetInfo.GetHierarchicalRecords(records, "path", '.', 1, includeChildren: true, maxChildren: 12);

            CoveredPks(groups).Should().Contain(paths);
        }
    }
}
