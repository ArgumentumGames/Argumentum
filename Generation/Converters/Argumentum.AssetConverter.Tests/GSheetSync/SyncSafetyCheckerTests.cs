using Argumentum.AssetConverter.GSheetSync;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
    public class SyncSafetyCheckerTests
    {
        private static DiffResult MakeDiff(
            int totalOld = 10, int totalNew = 10,
            int added = 0, int deleted = 0, int modified = 0, int unchanged = 10,
            int totalCellsOld = 30, int cellsModified = 0,
            string[]? columnsAdded = null, string[]? columnsRemoved = null)
        {
            return new DiffResult
            {
                TotalRowsOld = totalOld,
                TotalRowsNew = totalNew,
                RowsAdded = added,
                RowsDeleted = deleted,
                RowsModified = modified,
                RowsUnchanged = unchanged,
                TotalCellsOld = totalCellsOld,
                CellsModified = cellsModified,
                ColumnsAdded = columnsAdded?.ToList() ?? new List<string>(),
                ColumnsRemoved = columnsRemoved?.ToList() ?? new List<string>()
            };
        }

        private static GSheetSyncConfig DefaultConfig => new GSheetSyncConfig();

        [Fact]
        public void Evaluate_NoChanges_IsSafe()
        {
            var diff = MakeDiff();
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, DefaultConfig);

            result.IsSafe.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.Warnings.Should().BeEmpty();
        }

        [Fact]
        public void Evaluate_ColumnStructureChange_AbortsWhenEnabled()
        {
            var diff = MakeDiff(columnsAdded: new[] { "NewCol" });
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, DefaultConfig);

            result.IsSafe.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Contains("Column structure change"));
        }

        [Fact]
        public void Evaluate_ColumnStructureChange_PassesWhenDisabled()
        {
            var diff = MakeDiff(columnsRemoved: new[] { "OldCol" });
            var config = new GSheetSyncConfig { AbortOnColumnStructureChange = false };
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, config);

            result.IsSafe.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Evaluate_DeletionExceedsThreshold_Aborts()
        {
            // 20% deletion > 10% default threshold
            var diff = MakeDiff(totalOld: 10, deleted: 2, unchanged: 8);
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, DefaultConfig);

            result.IsSafe.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Contains("Deletion threshold"));
        }

        [Fact]
        public void Evaluate_DeletionWithinThreshold_WarnsOnly()
        {
            // 5% deletion < 10% default threshold
            var diff = MakeDiff(totalOld: 20, deleted: 1, unchanged: 19);
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, DefaultConfig);

            result.IsSafe.Should().BeTrue();
            result.Warnings.Should().ContainSingle(e => e.Contains("will be deleted"));
        }

        [Fact]
        public void Evaluate_ModificationExceedsThreshold_Aborts()
        {
            // 40% modification > 30% default threshold
            var diff = MakeDiff(totalCellsOld: 100, cellsModified: 40);
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, DefaultConfig);

            result.IsSafe.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Contains("Modification threshold"));
        }

        [Fact]
        public void Evaluate_ModificationWithinThreshold_Passes()
        {
            // 10% modification < 30% default threshold
            var diff = MakeDiff(totalCellsOld: 100, cellsModified: 10);
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, DefaultConfig);

            result.IsSafe.Should().BeTrue();
        }

        [Fact]
        public void Evaluate_RowsAdded_GeneratesWarning()
        {
            var diff = MakeDiff(totalNew: 12, added: 2);
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, DefaultConfig);

            result.IsSafe.Should().BeTrue();
            result.Warnings.Should().ContainSingle(e => e.Contains("2 new rows will be added"));
        }

        [Fact]
        public void Evaluate_MultipleViolations_AllReported()
        {
            var diff = MakeDiff(
                totalOld: 10, deleted: 5, unchanged: 5,
                totalCellsOld: 30, cellsModified: 20,
                columnsAdded: new[] { "X" });
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, DefaultConfig);

            result.IsSafe.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
        }

        [Fact]
        public void Evaluate_ZeroRows_NoDivideByZero()
        {
            var diff = MakeDiff(totalOld: 0, totalNew: 0, unchanged: 0);
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, DefaultConfig);

            result.IsSafe.Should().BeTrue();
        }

        [Fact]
        public void Evaluate_CustomThresholds_Respected()
        {
            var config = new GSheetSyncConfig
            {
                MaxDeletionPercentage = 50,
                MaxModificationPercentage = 80
            };
            // 20% deletion < 50% custom, 40% modification < 80% custom
            var diff = MakeDiff(totalOld: 10, deleted: 2, unchanged: 8,
                totalCellsOld: 100, cellsModified: 40);
            var checker = new SyncSafetyChecker();

            var result = checker.Evaluate(diff, config);

            result.IsSafe.Should().BeTrue();
        }
    }
}
