using System.Collections.Generic;

namespace Argumentum.AssetConverter.GSheetSync
{
	/// <summary>
	/// Formula-aware snapshot of a Google Sheets tab. Pairs evaluated values
	/// with their underlying formulas so cell-level diff/patch operations can
	/// skip cells whose values are derived (e.g. <c>path_padded</c>).
	/// </summary>
	public class SheetSnapshot
	{
		public IList<IList<object>> Values { get; set; } = new List<IList<object>>();

		public IList<IList<object>> Formulas { get; set; } = new List<IList<object>>();

		public HashSet<(int Row, int Col)> ProtectedCells { get; set; } = new HashSet<(int, int)>();

		/// <summary>
		/// Builds the set of protected cells from a formula grid. A cell is
		/// considered protected when its formula representation begins with
		/// <c>=</c>, indicating Sheets evaluates it from other cells.
		/// </summary>
		public static HashSet<(int Row, int Col)> BuildProtectedCells(IList<IList<object>> formulas)
		{
			var protectedCells = new HashSet<(int Row, int Col)>();
			if (formulas == null) return protectedCells;

			for (int row = 0; row < formulas.Count; row++)
			{
				var rowCells = formulas[row];
				if (rowCells == null) continue;

				for (int col = 0; col < rowCells.Count; col++)
				{
					var cell = rowCells[col]?.ToString();
					if (!string.IsNullOrEmpty(cell) && cell.StartsWith("="))
					{
						protectedCells.Add((row, col));
					}
				}
			}

			return protectedCells;
		}

		/// <summary>
		/// Converts a 0-based (row, col) into A1 notation (e.g. (0, 0) -> "A1",
		/// (1, 26) -> "AA2"). Used to surface human-readable cell references in
		/// diff reports.
		/// </summary>
		public static string ToA1Notation(int row, int col)
		{
			var columnLetters = "";
			var c = col;
			while (c >= 0)
			{
				columnLetters = (char)('A' + (c % 26)) + columnLetters;
				c = (c / 26) - 1;
			}
			return columnLetters + (row + 1);
		}
	}
}
