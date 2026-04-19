using Argumentum.AssetConverter.GSheetSync;
using FluentAssertions;
using System.IO;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
    public class DiffReportTests
    {
        private static DiffResult MakeDiff(
            int totalOld = 10, int totalNew = 10,
            int added = 0, int deleted = 0, int modified = 0, int unchanged = 10,
            int totalCellsOld = 30, int cellsModified = 0,
            string[]? columnsAdded = null, string[]? columnsRemoved = null,
            CellChange[]? overwrites = null)
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
                ColumnsRemoved = columnsRemoved?.ToList() ?? new List<string>(),
                SampleOverwrites = overwrites?.ToList() ?? new List<CellChange>()
            };
        }

        private static string CaptureOutput(DiffResult diff, string source = "LOCAL", string target = "REMOTE")
        {
            var sw = new StringWriter();
            var originalOut = Console.Out;
            try
            {
                Console.SetOut(sw);
                DiffReport.PrintToConsole(diff, source, target);
                return sw.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Fact]
        public void PrintToConsole_NoChanges_ContainsSummary()
        {
            var diff = MakeDiff();
            var output = CaptureOutput(diff);

            output.Should().Contain("Diff Report: LOCAL → REMOTE");
            output.Should().Contain("Rows: 10 → 10");
            output.Should().Contain("+0 added, -0 deleted");
        }

        [Fact]
        public void PrintToConsole_ColumnChanges_ShownInOutput()
        {
            var diff = MakeDiff(columnsAdded: new[] { "NewCol" }, columnsRemoved: new[] { "OldCol" });
            var output = CaptureOutput(diff);

            output.Should().Contain("Column structure change detected");
            output.Should().Contain("Columns added:   NewCol");
            output.Should().Contain("Columns removed:  OldCol");
        }

        [Fact]
        public void PrintToConsole_DeletionRate_ShownInOutput()
        {
            var diff = MakeDiff(totalOld: 10, deleted: 2, unchanged: 8);
            var output = CaptureOutput(diff);

            output.Should().Contain("Deletion rate:");
            // Number format is locale-dependent (20.0% or 20,0%)
            (output.Contains("20.0%") || output.Contains("20,0%")).Should().BeTrue("deletion percentage should appear");
            output.Should().Contain("of rows deleted");
        }

        [Fact]
        public void PrintToConsole_ModificationRate_ShownInOutput()
        {
            var diff = MakeDiff(totalCellsOld: 100, cellsModified: 25);
            var output = CaptureOutput(diff);

            output.Should().Contain("Modification rate:");
            // Number format is locale-dependent (25.0% or 25,0%)
            (output.Contains("25.0%") || output.Contains("25,0%")).Should().BeTrue("modification percentage should appear");
            output.Should().Contain("of cells changed");
        }

        [Fact]
        public void PrintToConsole_SampleOverwrites_ShownInOutput()
        {
            var diff = MakeDiff(overwrites: new[]
            {
                new CellChange { PrimaryKey = "42", ColumnName = "Name", OldValue = "Old", NewValue = "New" }
            });
            var output = CaptureOutput(diff);

            output.Should().Contain("[42] Name");
            output.Should().Contain("OLD: Old");
            output.Should().Contain("NEW: New");
        }

        [Fact]
        public void PrintToConsole_EmptyDiff_NoErrors()
        {
            var diff = MakeDiff(totalOld: 0, totalNew: 0, unchanged: 0);
            var output = CaptureOutput(diff);

            output.Should().Contain("Diff Report:");
            output.Should().Contain("Rows: 0 → 0");
            output.Should().NotContain("Column structure");
            output.Should().NotContain("Deletion rate");
            output.Should().NotContain("Modification rate");
        }

        [Fact]
        public void PrintToConsole_MultipleOverwrites_AllShown()
        {
            var diff = MakeDiff(overwrites: new[]
            {
                new CellChange { PrimaryKey = "1", ColumnName = "A", OldValue = "a", NewValue = "b" },
                new CellChange { PrimaryKey = "2", ColumnName = "B", OldValue = "c", NewValue = "d" }
            });
            var output = CaptureOutput(diff);

            output.Should().Contain("[1] A");
            output.Should().Contain("[2] B");
        }
    }
}
