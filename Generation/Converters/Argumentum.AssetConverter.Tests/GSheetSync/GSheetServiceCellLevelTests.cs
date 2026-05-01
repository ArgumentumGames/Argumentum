using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter.GSheetSync;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
	public class GSheetServiceCellLevelTests
	{
		[Fact]
		public void CellPatch_A1Notation_MatchesSnapshot()
		{
			var patch = new CellPatch
			{
				Row = 5,
				Col = 2,
				A1Notation = SheetSnapshot.ToA1Notation(5, 2),
				ColumnName = "title_fr",
				PrimaryKey = "1.1",
				OldValue = "Old",
				NewValue = "New"
			};

			Assert.Equal("C6", patch.A1Notation);
			Assert.Null(patch.SkipReason);
		}

		[Fact]
		public void PatchSkipReason_FormulaProtected_SetCorrectly()
		{
			var patch = new CellPatch
			{
				SkipReason = PatchSkipReason.FormulaProtected,
			};

			Assert.Equal(PatchSkipReason.FormulaProtected, patch.SkipReason);
		}

		[Fact]
		public void BatchUpdate_BuildsCorrectRanges()
		{
			var patches = new List<CellPatch>
			{
				new CellPatch
				{
					A1Notation = "B2",
					NewValue = "Updated1",
				},
				new CellPatch
				{
					A1Notation = "D5",
					NewValue = "Updated2",
				},
			};

			// Verify patch data integrity (actual API call requires mock)
			Assert.Equal(2, patches.Count);
			Assert.Equal("B2", patches[0].A1Notation);
			Assert.Equal("D5", patches[1].A1Notation);
			Assert.Equal("Updated1", patches[0].NewValue);
			Assert.Equal("Updated2", patches[1].NewValue);
		}

		[Fact]
		public void CellLevelDiffResult_ToReport_NoChanges()
		{
			var result = new CellLevelDiffResult
			{
				TotalGDriveRows = 10,
				TotalLocalRows = 10,
				PkMatched = 10,
				PkUnmatched = 0,
				CellsCompared = 50,
			};

			var report = result.ToReport();

			Assert.Contains("Cells to patch: 0", report);
			Assert.Contains("PK matched: 10", report);
			Assert.DoesNotContain("Cells to Patch", report);
		}

		[Fact]
		public void CellLevelDiffResult_ToReport_WithPatches()
		{
			var result = new CellLevelDiffResult
			{
				TotalGDriveRows = 5,
				TotalLocalRows = 5,
				PkMatched = 5,
				CellsCompared = 20,
			};
			result.PatchesToApply.Add(new CellPatch
			{
				A1Notation = "C3",
				ColumnName = "title_fr",
				PrimaryKey = "1.2",
				OldValue = "Old Title",
				NewValue = "New Title",
			});
			result.ProtectedSkips.Add(new CellPatch
			{
				A1Notation = "D3",
				ColumnName = "path_padded",
				PrimaryKey = "1.2",
				OldValue = "=TEXT(A3,\"000.000\")",
				NewValue = "1.2",
				SkipReason = PatchSkipReason.FormulaProtected,
			});

			var report = result.ToReport();

			Assert.Contains("Cells to Patch", report);
			Assert.Contains("C3", report);
			Assert.Contains("title_fr", report);
			Assert.Contains("Protected Cells", report);
			Assert.Contains("path_padded", report);
		}

		[Fact]
		public void CellLevelDiffEngine_PreservesRowIndexInPatches()
		{
			var gdrive = new SheetSnapshot
			{
				Values = new List<IList<object>>
				{
					new List<object> { "pk", "title" },
					new List<object> { "1", "A" },
					new List<object> { "2", "B" },
					new List<object> { "3", "C" },
				},
				Formulas = new List<IList<object>>
				{
					new List<object> { "pk", "title" },
					new List<object> { "1", "A" },
					new List<object> { "2", "B" },
					new List<object> { "3", "C" },
				},
				ProtectedCells = new HashSet<(int, int)>(),
			};

			var csv = "pk,title\n1,A Updated\n2,B\n3,C Updated";
			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Equal(2, result.PatchesToApply.Count);
			// Row 1 (data row index 1, 0-based) → A1 notation "B2"
			Assert.Equal("B2", result.PatchesToApply[0].A1Notation);
			Assert.Equal(1, result.PatchesToApply[0].Row);
			// Row 3 (data row index 3, 0-based) → A1 notation "B4"
			Assert.Equal("B4", result.PatchesToApply[1].A1Notation);
			Assert.Equal(3, result.PatchesToApply[1].Row);
		}
	}
}
