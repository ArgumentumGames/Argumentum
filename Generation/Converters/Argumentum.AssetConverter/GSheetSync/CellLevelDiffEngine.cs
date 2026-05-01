using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.GSheetSync
{
	public class CellLevelDiffEngine
	{
		private readonly string _primaryKeyColumn;

		public CellLevelDiffEngine(string primaryKeyColumn)
		{
			_primaryKeyColumn = primaryKeyColumn;
		}

		public CellLevelDiffResult Compare(SheetSnapshot gdrive, string localCsv)
		{
			var result = new CellLevelDiffResult();

			// Parse local CSV
			var (localHeaders, localRows) = ParseCsv(localCsv);

			if (gdrive.Values == null || gdrive.Values.Count == 0)
			{
				result.TotalGDriveRows = 0;
				result.TotalLocalRows = localRows.Count;
				return result;
			}

			// Extract GDrive headers (row 0)
			var gdriveHeaders = ExtractHeaders(gdrive.Values);
			if (gdriveHeaders.Count == 0)
				return result;

			// Build GDrive PK index: pk -> (gdriveRow index, gdriveRow data)
			var gdrivePkIndex = IndexGdriveRows(gdrive.Values, gdriveHeaders);
			// Build local PK index: pk -> (localRow index, localRow data)
			var localPkIndex = IndexLocalRows(localHeaders, localRows);

			result.TotalGDriveRows = gdrivePkIndex.Count;
			result.TotalLocalRows = localPkIndex.Count;

			// Common columns between GDrive and local
			var gdriveColSet = new HashSet<string>(gdriveHeaders, StringComparer.OrdinalIgnoreCase);
			var localColSet = new HashSet<string>(localHeaders, StringComparer.OrdinalIgnoreCase);
			var commonColumns = gdriveHeaders.Where(h => localColSet.Contains(h)).ToList();

			// For each local row with a matching PK in GDrive, compare cells
			foreach (var (pk, localEntry) in localPkIndex)
			{
				if (!gdrivePkIndex.TryGetValue(pk, out var gdriveEntry))
				{
					result.PkUnmatched++;
					result.PkUnmatchedRows.Add(new CellPatch
					{
						PrimaryKey = pk,
						SkipReason = PatchSkipReason.PkUnmatched
					});
					continue;
				}

				result.PkMatched++;

				foreach (var col in commonColumns)
				{
					var gdriveColIdx = gdriveHeaders.FindIndex(h =>
						string.Equals(h, col, StringComparison.OrdinalIgnoreCase));
					var localColIdx = localHeaders.FindIndex(h =>
						string.Equals(h, col, StringComparison.OrdinalIgnoreCase));

					var gdriveVal = GetCellValue(gdrive.Values, gdriveEntry.RowIdx, gdriveColIdx);
					var localVal = localEntry.RowData[localColIdx] ?? "";

					var normGdrive = NormalizeValue(gdriveVal);
					var normLocal = NormalizeValue(localVal.ToString());

					result.CellsCompared++;

					if (normGdrive == normLocal)
						continue;

					var a1 = SheetSnapshot.ToA1Notation(gdriveEntry.RowIdx, gdriveColIdx);
					var patch = new CellPatch
					{
						Row = gdriveEntry.RowIdx,
						Col = gdriveColIdx,
						A1Notation = a1,
						ColumnName = col,
						PrimaryKey = pk,
						OldValue = normGdrive,
						NewValue = normLocal
					};

					if (gdrive.ProtectedCells.Contains((gdriveEntry.RowIdx, gdriveColIdx)))
					{
						patch.SkipReason = PatchSkipReason.FormulaProtected;
						result.ProtectedSkips.Add(patch);
					}
					else
					{
						result.PatchesToApply.Add(patch);
					}
				}
			}

			// Count GDrive PKs not in local
			foreach (var pk in gdrivePkIndex.Keys)
			{
				if (!localPkIndex.ContainsKey(pk))
					result.PkUnmatched++;
			}

			return result;
		}

		private static List<string> ExtractHeaders(IList<IList<object>> grid)
		{
			if (grid.Count == 0) return new List<string>();
			var headerRow = grid[0];
			if (headerRow == null) return new List<string>();
			return headerRow.Select(h => h?.ToString() ?? "").ToList();
		}

		private static string GetCellValue(IList<IList<object>> grid, int row, int col)
		{
			if (row < 0 || row >= grid.Count) return "";
			var rowData = grid[row];
			if (rowData == null || col < 0 || col >= rowData.Count) return "";
			return rowData[col]?.ToString() ?? "";
		}

		private static Dictionary<string, (int RowIdx, Dictionary<string, object> RowData)> IndexGdriveRows(
			IList<IList<object>> grid, List<string> headers)
		{
			var index = new Dictionary<string, (int, Dictionary<string, object>)>(StringComparer.OrdinalIgnoreCase);

			var pkColIdx = headers.FindIndex(h =>
				string.Equals(h, "pk", StringComparison.OrdinalIgnoreCase));
			if (pkColIdx < 0)
				pkColIdx = headers.FindIndex(h =>
					string.Equals(h, "PK", StringComparison.Ordinal));

			if (pkColIdx < 0)
				return index;

			for (int rowIdx = 1; rowIdx < grid.Count; rowIdx++)
			{
				var row = grid[rowIdx];
				if (row == null || pkColIdx >= row.Count) continue;

				var pk = row[pkColIdx]?.ToString()?.Trim() ?? "";
				if (string.IsNullOrEmpty(pk)) continue;
				if (index.ContainsKey(pk)) continue;

				var rowData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
				for (int c = 0; c < headers.Count && c < row.Count; c++)
				{
					rowData[headers[c]] = row[c] ?? "";
				}

				index[pk] = (rowIdx, rowData);
			}

			return index;
		}

		private static Dictionary<string, (int RowIdx, List<object> RowData)> IndexLocalRows(
			List<string> headers, List<List<object>> rows)
		{
			var index = new Dictionary<string, (int, List<object>)>(StringComparer.OrdinalIgnoreCase);

			var pkColIdx = headers.FindIndex(h =>
				string.Equals(h, "pk", StringComparison.OrdinalIgnoreCase));
			if (pkColIdx < 0)
				pkColIdx = headers.FindIndex(h =>
					string.Equals(h, "PK", StringComparison.Ordinal));

			if (pkColIdx < 0)
				return index;

			for (int i = 0; i < rows.Count; i++)
			{
				var row = rows[i];
				if (pkColIdx >= row.Count) continue;

				var pk = row[pkColIdx]?.ToString()?.Trim() ?? "";
				if (string.IsNullOrEmpty(pk)) continue;
				if (index.ContainsKey(pk)) continue;

				index[pk] = (i + 1, row); // +1 because GDrive row 0 is header
			}

			return index;
		}

		private static (List<string> Headers, List<List<object>> Rows) ParseCsv(string csvContent)
		{
			var headers = new List<string>();
			var rows = new List<List<object>>();

			using var reader = new StringReader(csvContent);
			using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				MissingFieldFound = null,
				BadDataFound = null,
				TrimOptions = TrimOptions.None,
			});

			csv.Read();
			csv.ReadHeader();
			if (csv.HeaderRecord != null)
				headers = csv.HeaderRecord.ToList();

			while (csv.Read())
			{
				var row = new List<object>();
				for (int i = 0; i < csv.Parser.Count; i++)
				{
					row.Add(csv.GetField(i) ?? "");
				}
				rows.Add(row);
			}

			return (headers, rows);
		}

		private static string NormalizeValue(string value)
		{
			if (string.IsNullOrEmpty(value)) return "";
			return value.Replace("\r\n", "\n").Replace("\r", "\n").Trim();
		}
	}
}
