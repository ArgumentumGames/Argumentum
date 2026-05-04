using System.Collections.Generic;
using Argumentum.AssetConverter.GSheetSync;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
	public class CellLevelDiffEngineTests
	{
		private static SheetSnapshot MakeSnapshot(
			IList<IList<object>> values,
			IList<IList<object>>? formulas = null)
		{
			formulas ??= values;
			return new SheetSnapshot
			{
				Values = values,
				Formulas = formulas,
				ProtectedCells = SheetSnapshot.BuildProtectedCells(formulas),
			};
		}

		private static string BuildCsv(string[] headers, params string[][] rows)
		{
			var lines = new List<string> { string.Join(",", headers) };
			foreach (var row in rows)
				lines.Add(string.Join(",", row));
			return string.Join("\n", lines);
		}

		[Fact]
		public void NoChanges_EmptyPatches()
		{
			var gdrive = MakeSnapshot(new List<IList<object>>
			{
				new List<object> { "pk", "title_fr", "desc_fr" },
				new List<object> { "1.1", "Foo", "Bar" },
				new List<object> { "1.2", "Baz", "Qux" },
			});

			var csv = BuildCsv(
				new[] { "pk", "title_fr", "desc_fr" },
				new[] { "1.1", "Foo", "Bar" },
				new[] { "1.2", "Baz", "Qux" });

			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Empty(result.PatchesToApply);
			Assert.Empty(result.ProtectedSkips);
			Assert.Equal(2, result.PkMatched);
			Assert.Equal(6, result.CellsCompared); // 2 rows × 3 cols (including pk)
		}

		[Fact]
		public void ChangedCells_GeneratePatches()
		{
			var gdrive = MakeSnapshot(new List<IList<object>>
			{
				new List<object> { "pk", "title_fr", "desc_fr" },
				new List<object> { "1.1", "Old Title", "Bar" },
				new List<object> { "1.2", "Baz", "Qux" },
			});

			var csv = BuildCsv(
				new[] { "pk", "title_fr", "desc_fr" },
				new[] { "1.1", "New Title", "Bar" },
				new[] { "1.2", "Baz Updated", "Qux" });

			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Equal(2, result.PatchesToApply.Count);
			Assert.Contains(result.PatchesToApply, p =>
				p.PrimaryKey == "1.1" && p.ColumnName == "title_fr" &&
				p.OldValue == "Old Title" && p.NewValue == "New Title");
			Assert.Contains(result.PatchesToApply, p =>
				p.PrimaryKey == "1.2" && p.ColumnName == "title_fr" &&
				p.OldValue == "Baz" && p.NewValue == "Baz Updated");
		}

		[Fact]
		public void FormulaCells_SkippedNotPatched()
		{
			var values = new List<IList<object>>
			{
				new List<object> { "pk", "title_fr", "path_padded" },
				new List<object> { "1.1", "Foo", "001.001" },
			};
			var formulas = new List<IList<object>>
			{
				new List<object> { "pk", "title_fr", "path_padded" },
				new List<object> { "1.1", "Foo", "=TEXT(A2,\"000.000\")" },
			};

			var gdrive = MakeSnapshot(values, formulas);

			var csv = BuildCsv(
				new[] { "pk", "title_fr", "path_padded" },
				new[] { "1.1", "Foo Updated", "1.1" });

			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			// title_fr should be patched (not protected)
			Assert.Single(result.PatchesToApply);
			Assert.Equal("title_fr", result.PatchesToApply[0].ColumnName);

			// path_padded should be skipped (formula protected)
			Assert.Single(result.ProtectedSkips);
			Assert.Equal("path_padded", result.ProtectedSkips[0].ColumnName);
			Assert.Equal(PatchSkipReason.FormulaProtected, result.ProtectedSkips[0].SkipReason);
		}

		[Fact]
		public void PartialPkMatch_SomeUnmatched()
		{
			var gdrive = MakeSnapshot(new List<IList<object>>
			{
				new List<object> { "pk", "title_fr" },
				new List<object> { "1.1", "Foo" },
			});

			var csv = BuildCsv(
				new[] { "pk", "title_fr" },
				new[] { "1.1", "Foo Updated" },
				new[] { "1.2", "New Entry" },
				new[] { "1.3", "Another New" });

			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Single(result.PatchesToApply);
			Assert.Equal(1, result.PkMatched);
			Assert.Equal(2, result.PkUnmatched); // 1.2 and 1.3 in local but not in GDrive
		}

		[Fact]
		public void EmptyGDrive_NoPatches()
		{
			var gdrive = MakeSnapshot(new List<IList<object>>());
			var csv = BuildCsv(
				new[] { "pk", "title_fr" },
				new[] { "1.1", "Foo" });

			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Empty(result.PatchesToApply);
			Assert.Equal(0, result.TotalGDriveRows);
		}

		[Fact]
		public void CommonColumnsOnly_IgnoresExtraColumns()
		{
			var gdrive = MakeSnapshot(new List<IList<object>>
			{
				new List<object> { "pk", "title_fr", "desc_fr" },
				new List<object> { "1.1", "Foo", "Bar" },
			});

			// Local CSV has extra column not in GDrive
			var csv = BuildCsv(
				new[] { "pk", "title_fr", "desc_fr", "extra_col" },
				new[] { "1.1", "Foo", "Bar Changed", "Ignored" });

			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Single(result.PatchesToApply);
			Assert.Equal("desc_fr", result.PatchesToApply[0].ColumnName);
		}

		[Fact]
		public void ToReport_ContainsAllSections()
		{
			var gdrive = MakeSnapshot(new List<IList<object>>
			{
				new List<object> { "pk", "title_fr", "path_padded" },
				new List<object> { "1.1", "Old", "001.001" },
			}, new List<IList<object>>
			{
				new List<object> { "pk", "title_fr", "path_padded" },
				new List<object> { "1.1", "Old", "=TEXT(A2,\"000.000\")" },
			});

			var csv = BuildCsv(
				new[] { "pk", "title_fr", "path_padded" },
				new[] { "1.1", "New", "1.1" });

			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);
			var report = result.ToReport();

			Assert.Contains("Cells to Patch", report);
			Assert.Contains("Protected Cells", report);
			Assert.Contains("title_fr", report);
			Assert.Contains("path_padded", report);
		}

		[Fact]
		public void WhitespaceDifference_Normalized()
		{
			var gdrive = MakeSnapshot(new List<IList<object>>
			{
				new List<object> { "pk", "desc_fr" },
				new List<object> { "1.1", "Hello World" },
			});

			var csv = BuildCsv(
				new[] { "pk", "desc_fr" },
				new[] { "1.1", "Hello World  " }); // trailing spaces

			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Empty(result.PatchesToApply);
		}

		[Fact]
		public void PkColumnCaseInsensitive()
		{
			var gdrive = MakeSnapshot(new List<IList<object>>
			{
				new List<object> { "PK", "title_fr" },
				new List<object> { "1.1", "Old" },
			});

			var csv = BuildCsv(
				new[] { "pk", "title_fr" },
				new[] { "1.1", "New" });

			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Single(result.PatchesToApply);
		}

		[Fact]
		public void MultipleFormulaCells_AllSkipped()
		{
			var values = new List<IList<object>>
			{
				new List<object> { "pk", "col_a", "col_b", "col_c" },
				new List<object> { "1.1", "val_a", "1", "val_c" },
			};
			var formulas = new List<IList<object>>
			{
				new List<object> { "pk", "col_a", "col_b", "col_c" },
				new List<object> { "1.1", "val_a", "=B2*2", "=CONCAT(A2,C2)" },
			};

			var gdrive = MakeSnapshot(values, formulas);

			var csv = BuildCsv(
				new[] { "pk", "col_a", "col_b", "col_c" },
				new[] { "1.1", "val_a_changed", "2", "val_c_changed" });

			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			// col_a should be patched (not protected)
			Assert.Single(result.PatchesToApply);
			Assert.Equal("col_a", result.PatchesToApply[0].ColumnName);
			// col_b and col_c should be skipped (formula protected)
			Assert.Equal(2, result.ProtectedSkips.Count);
			Assert.Contains(result.ProtectedSkips, p => p.ColumnName == "col_b");
			Assert.Contains(result.ProtectedSkips, p => p.ColumnName == "col_c");
		}
	}
}
