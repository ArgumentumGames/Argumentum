using System;
using System.Linq;

namespace Argumentum.AssetConverter.GSheetSync
{
	public static class DiffReport
	{
		public static void PrintToConsole(DiffResult diff, string sourceLabel, string targetLabel)
		{
			Console.WriteLine();
			Console.WriteLine($"=== Diff Report: {sourceLabel} → {targetLabel} ===");
			Console.WriteLine();

			// Summary stats
			Console.WriteLine("Summary:");
			Console.WriteLine($"  Rows: {diff.TotalRowsOld} → {diff.TotalRowsNew} " +
			                  $"(+{diff.RowsAdded} added, -{diff.RowsDeleted} deleted, " +
			                  $"~{diff.RowsModified} modified, ={diff.RowsUnchanged} unchanged)");
			Console.WriteLine($"  Cells modified: {diff.CellsModified} / {diff.TotalCellsOld} " +
			                  $"({diff.ModificationPercentage:F1}%)");
			Console.WriteLine();

			// Column changes
			if (diff.HasColumnStructureChange)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("⚠ Column structure change detected!");
				if (diff.ColumnsAdded.Count > 0)
					Console.WriteLine($"  Columns added:   {string.Join(", ", diff.ColumnsAdded)}");
				if (diff.ColumnsRemoved.Count > 0)
					Console.WriteLine($"  Columns removed:  {string.Join(", ", diff.ColumnsRemoved)}");
				Console.ResetColor();
				Console.WriteLine();
			}

			// Safety indicators
			if (diff.DeletionPercentage > 0)
			{
				Console.ForegroundColor = diff.DeletionPercentage > 10 ? ConsoleColor.Red : ConsoleColor.Yellow;
				Console.WriteLine($"⚠ Deletion rate: {diff.DeletionPercentage:F1}% of rows deleted");
				Console.ResetColor();
			}

			if (diff.ModificationPercentage > 0)
			{
				Console.ForegroundColor = diff.ModificationPercentage > 30 ? ConsoleColor.Red : ConsoleColor.Yellow;
				Console.WriteLine($"⚠ Modification rate: {diff.ModificationPercentage:F1}% of cells changed");
				Console.ResetColor();
			}

			// Sample overwritten cells
			if (diff.SampleOverwrites.Count > 0)
			{
				Console.WriteLine();
				Console.WriteLine($"Sample overwritten cells (showing first {diff.SampleOverwrites.Count}):");
				Console.WriteLine(new string('-', 80));

				foreach (var change in diff.SampleOverwrites)
				{
					Console.WriteLine($"  [{change.PrimaryKey}] {change.ColumnName}:");
					Console.WriteLine($"    OLD: {change.OldValue}");
					Console.WriteLine($"    NEW: {change.NewValue}");
				}

				Console.WriteLine(new string('-', 80));
			}

			Console.WriteLine();
		}
	}
}
