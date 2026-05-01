using System.Collections.Generic;
using System.Linq;

namespace Argumentum.AssetConverter.GSheetSync
{
	public class CellLevelDiffResult
	{
		public List<CellPatch> PatchesToApply { get; set; } = new List<CellPatch>();
		public List<CellPatch> ProtectedSkips { get; set; } = new List<CellPatch>();
		public List<CellPatch> PkUnmatchedRows { get; set; } = new List<CellPatch>();
		public int TotalGDriveRows { get; set; }
		public int TotalLocalRows { get; set; }
		public int PkMatched { get; set; }
		public int PkUnmatched { get; set; }
		public int CellsCompared { get; set; }

		public string ToReport()
		{
			var lines = new List<string>
			{
				"# Cell-Level Diff Report",
				"",
				"## Summary",
				"",
				$"- GDrive rows: {TotalGDriveRows}",
				$"- Local CSV rows: {TotalLocalRows}",
				$"- PK matched: {PkMatched}",
				$"- PK unmatched: {PkUnmatched}",
				$"- Cells compared: {CellsCompared}",
				$"- Cells to patch: {PatchesToApply.Count}",
				$"- Protected (formula, NOT patched): {ProtectedSkips.Count}",
				""
			};

			if (PatchesToApply.Count > 0)
			{
				lines.Add("## Cells to Patch");
				lines.Add("");
				lines.Add("| A1 | Column | PK | Old | New |");
				lines.Add("|----|--------|----|-----|-----|");
				foreach (var p in PatchesToApply.Take(100))
				{
					lines.Add($"| {p.A1Notation} | {p.ColumnName} | {p.PrimaryKey} | {Truncate(p.OldValue, 40)} | {Truncate(p.NewValue, 40)} |");
				}
				if (PatchesToApply.Count > 100)
					lines.Add($"| ... | ... | ... | ... | ... | _(+{PatchesToApply.Count - 100} more)_");
				lines.Add("");
			}

			if (ProtectedSkips.Count > 0)
			{
				lines.Add("## Protected Cells (formula, NOT patched)");
				lines.Add("");
				lines.Add("| A1 | Column | PK | GDrive Formula | Local CSV Value (ignored) |");
				lines.Add("|----|--------|----|----------------|---------------------------|");
				foreach (var p in ProtectedSkips.Take(50))
				{
					lines.Add($"| {p.A1Notation} | {p.ColumnName} | {p.PrimaryKey} | {Truncate(p.OldValue, 50)} | {Truncate(p.NewValue, 50)} |");
				}
				if (ProtectedSkips.Count > 50)
					lines.Add($"| ... | ... | ... | ... | ... | _(+{ProtectedSkips.Count - 50} more)_");
				lines.Add("");
			}

			return string.Join("\n", lines);
		}

		private static string Truncate(string value, int maxLength)
		{
			if (value == null) return "";
			if (value.Length <= maxLength) return value;
			return value.Substring(0, maxLength - 3) + "...";
		}
	}
}
