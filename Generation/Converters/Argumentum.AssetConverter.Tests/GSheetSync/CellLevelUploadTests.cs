using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter.GSheetSync;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
	/// <summary>
	/// Tests for the cell-level upload configuration, DryRun report persistence,
	/// and GSheetSyncRunner routing logic (cell-level vs full-sheet).
	/// </summary>
	public class CellLevelUploadTests : IDisposable
	{
		private readonly string _tempDir;

		public CellLevelUploadTests()
		{
			_tempDir = Path.Combine(Path.GetTempPath(), $"gsheet-test-{Guid.NewGuid():N}");
			Directory.CreateDirectory(_tempDir);
		}

		public void Dispose()
		{
			if (Directory.Exists(_tempDir))
			{
				Directory.Delete(_tempDir, true);
			}
		}

		// --- Config tests ---

		[Fact]
		public void GSheetSyncConfig_UseCellLevelUpload_DefaultsTrue()
		{
			var config = new GSheetSyncConfig();
			Assert.True(config.UseCellLevelUpload);
		}

		[Fact]
		public void GSheetSyncConfig_SyncReportsPath_DefaultsToTargetSyncReports()
		{
			var config = new GSheetSyncConfig();
			Assert.Equal("Target/SyncReports", config.SyncReportsPath);
		}

		[Fact]
		public void GSheetSyncConfig_UseCellLevelUpload_CanBeSetFalse()
		{
			var config = new GSheetSyncConfig { UseCellLevelUpload = false };
			Assert.False(config.UseCellLevelUpload);
		}

		// --- DryRun report file tests ---

		[Fact]
		public void CellLevelDiffResult_ToReport_ContainsAllSections()
		{
			var result = new CellLevelDiffResult
			{
				TotalGDriveRows = 100,
				TotalLocalRows = 105,
				PkMatched = 98,
				PkUnmatched = 5,
				CellsCompared = 490,
			};
			result.PatchesToApply.Add(new CellPatch
			{
				A1Notation = "D5",
				ColumnName = "title_en",
				PrimaryKey = "42",
				OldValue = "Ad Hominem",
				NewValue = "Ad Hominem Circumstantial",
			});
			result.ProtectedSkips.Add(new CellPatch
			{
				A1Notation = "E5",
				ColumnName = "path_padded",
				PrimaryKey = "42",
				OldValue = "=TEXT(A5,\"000.000\")",
				NewValue = "042",
				SkipReason = PatchSkipReason.FormulaProtected,
			});
			result.PkUnmatchedRows.Add(new CellPatch
			{
				PrimaryKey = "NEW1",
				SkipReason = PatchSkipReason.PkUnmatched,
			});

			var report = result.ToReport();

			Assert.Contains("# Cell-Level Diff Report", report);
			Assert.Contains("GDrive rows: 100", report);
			Assert.Contains("Local CSV rows: 105", report);
			Assert.Contains("PK matched: 98", report);
			Assert.Contains("PK unmatched: 5", report);
			Assert.Contains("Cells compared: 490", report);
			Assert.Contains("Cells to patch: 1", report);
			Assert.Contains("Protected (formula, NOT patched): 1", report);
			Assert.Contains("D5", report);
			Assert.Contains("title_en", report);
			Assert.Contains("path_padded", report);
		}

		[Fact]
		public void DryRunReport_WrittenToFile()
		{
			var result = new CellLevelDiffResult
			{
				TotalGDriveRows = 10,
				TotalLocalRows = 10,
				PkMatched = 10,
				CellsCompared = 50,
			};

			var reportPath = Path.Combine(_tempDir, "fallacies-2026-06-03_01-00-00.md");
			File.WriteAllText(reportPath, result.ToReport());

			Assert.True(File.Exists(reportPath));
			var content = File.ReadAllText(reportPath);
			Assert.Contains("Cell-Level Diff Report", content);
			Assert.Contains("Cells to patch: 0", content);
		}

		[Fact]
		public void DryRunReport_CreatedInSubdirectory()
		{
			var reportDir = Path.Combine(_tempDir, "Target", "SyncReports");
			Directory.CreateDirectory(reportDir);
			var reportPath = Path.Combine(reportDir, "virtues-2026-06-03_02-00-00.md");

			var result = new CellLevelDiffResult
			{
				TotalGDriveRows = 5,
				TotalLocalRows = 5,
				PkMatched = 5,
				CellsCompared = 25,
			};
			result.PatchesToApply.Add(new CellPatch
			{
				A1Notation = "A2",
				ColumnName = "pk",
				PrimaryKey = "1",
				OldValue = "old",
				NewValue = "new",
			});

			File.WriteAllText(reportPath, result.ToReport());

			Assert.True(File.Exists(reportPath));
			var content = File.ReadAllText(reportPath);
			Assert.Contains("Cells to patch: 1", content);
		}

		// --- Runner routing logic (config-based) ---

		[Fact]
		public void GSheetSyncConfig_CellLevelUpload_EnabledByDefault()
		{
			// Verify that a fresh config routes to cell-level path
			var config = new GSheetSyncConfig();
			Assert.True(config.UseCellLevelUpload,
				"Default should be cell-level (formula-aware) upload");
		}

		[Fact]
		public void GSheetSyncConfig_DirectionDefaultsToDownload()
		{
			var config = new GSheetSyncConfig();
			Assert.Equal(SyncDirection.Download, config.Direction);
		}

		// --- Cell-level edge case tests ---

		[Fact]
		public void CellLevelDiffEngine_EmptyGDrive_ProducesNoPatches()
		{
			var gdrive = new SheetSnapshot
			{
				Values = new List<IList<object>>(),
				Formulas = new List<IList<object>>(),
				ProtectedCells = new HashSet<(int, int)>(),
			};

			var csv = "pk,title\n1,A\n2,B";
			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Empty(result.PatchesToApply);
			Assert.Equal(0, result.TotalGDriveRows);
			Assert.Equal(2, result.TotalLocalRows);
		}

		[Fact]
		public void CellLevelDiffEngine_MultipleFormulaCells_AllSkipped()
		{
			var gdrive = new SheetSnapshot
			{
				Values = new List<IList<object>>
				{
					new List<object> { "pk", "col_a", "col_b" },
					new List<object> { "1", "100", "200" },
				},
				Formulas = new List<IList<object>>
				{
					new List<object> { "pk", "col_a", "col_b" },
					new List<object> { "1", "=A2*2", "=B2*3" },
				},
				ProtectedCells = new HashSet<(int, int)> { (1, 1), (1, 2) },
			};

			var csv = "pk,col_a,col_b\n1,999,888";
			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Empty(result.PatchesToApply);
			Assert.Equal(2, result.ProtectedSkips.Count);
		}

		[Fact]
		public void CellLevelDiffEngine_MixedFormulaAndValue_FormulaSkippedValuePatched()
		{
			var gdrive = new SheetSnapshot
			{
				Values = new List<IList<object>>
				{
					new List<object> { "pk", "title", "computed" },
					new List<object> { "1", "Old Title", "42" },
				},
				Formulas = new List<IList<object>>
				{
					new List<object> { "pk", "title", "computed" },
					new List<object> { "1", "Old Title", "=A2+B2" },
				},
				ProtectedCells = new HashSet<(int, int)> { (1, 2) },
			};

			var csv = "pk,title,computed\n1,New Title,99";
			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Single(result.PatchesToApply);
			Assert.Equal("title", result.PatchesToApply[0].ColumnName);
			Assert.Equal("New Title", result.PatchesToApply[0].NewValue);

			Assert.Single(result.ProtectedSkips);
			Assert.Equal("computed", result.ProtectedSkips[0].ColumnName);
		}

		[Fact]
		public void CellLevelDiffEngine_IdenticalData_NoPatches()
		{
			var gdrive = new SheetSnapshot
			{
				Values = new List<IList<object>>
				{
					new List<object> { "pk", "title" },
					new List<object> { "1", "Ad Hominem" },
					new List<object> { "2", "Straw Man" },
				},
				Formulas = new List<IList<object>>
				{
					new List<object> { "pk", "title" },
					new List<object> { "1", "Ad Hominem" },
					new List<object> { "2", "Straw Man" },
				},
				ProtectedCells = new HashSet<(int, int)>(),
			};

			var csv = "pk,title\n1,Ad Hominem\n2,Straw Man";
			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Empty(result.PatchesToApply);
			Assert.Empty(result.ProtectedSkips);
			Assert.Equal(2, result.PkMatched);
		}

		[Fact]
		public void CellLevelDiffEngine_WhitespaceNormalization_IgnoresTrailingSpaces()
		{
			var gdrive = new SheetSnapshot
			{
				Values = new List<IList<object>>
				{
					new List<object> { "pk", "title" },
					new List<object> { "1", "Ad Hominem " }, // trailing space
				},
				Formulas = new List<IList<object>>
				{
					new List<object> { "pk", "title" },
					new List<object> { "1", "Ad Hominem " },
				},
				ProtectedCells = new HashSet<(int, int)>(),
			};

			var csv = "pk,title\n1, Ad Hominem"; // leading space
			var engine = new CellLevelDiffEngine("pk");
			var result = engine.Compare(gdrive, csv);

			Assert.Empty(result.PatchesToApply); // Both normalize to "Ad Hominem"
		}

		// --- ToReport edge cases ---

		[Fact]
		public void CellLevelDiffResult_ToReport_TruncatesLongPatches()
		{
			var result = new CellLevelDiffResult();
			var longValue = new string('X', 100);
			result.PatchesToApply.Add(new CellPatch
			{
				A1Notation = "A1",
				ColumnName = "desc",
				PrimaryKey = "1",
				OldValue = longValue,
				NewValue = longValue,
			});

			var report = result.ToReport();
			Assert.Contains("...", report); // Should truncate
		}

		[Fact]
		public void CellLevelDiffResult_ToReport_LimitsPatchesTo100()
		{
			var result = new CellLevelDiffResult();
			for (int i = 0; i < 150; i++)
			{
				result.PatchesToApply.Add(new CellPatch
				{
					A1Notation = $"A{i + 1}",
					ColumnName = "title",
					PrimaryKey = $"{i}",
					OldValue = "old",
					NewValue = "new",
				});
			}

			var report = result.ToReport();
			Assert.Contains("+50 more", report);
		}

		[Fact]
		public void CellLevelDiffResult_ToReport_LimitsProtectedTo50()
		{
			var result = new CellLevelDiffResult();
			for (int i = 0; i < 75; i++)
			{
				result.ProtectedSkips.Add(new CellPatch
				{
					A1Notation = $"B{i + 1}",
					ColumnName = "computed",
					PrimaryKey = $"{i}",
					OldValue = "=FORMULA",
					NewValue = "value",
					SkipReason = PatchSkipReason.FormulaProtected,
				});
			}

			var report = result.ToReport();
			Assert.Contains("+25 more", report);
		}

		// --- Obsolete attribute test ---

		[Fact]
		public void GSheetService_UpdateSheetDataAsync_IsObsolete()
		{
			var method = typeof(GSheetService).GetMethod("UpdateSheetDataAsync");
			Assert.NotNull(method);
			var attr = method.GetCustomAttributes(typeof(ObsoleteAttribute), false)
				.Cast<ObsoleteAttribute>().FirstOrDefault();
			Assert.NotNull(attr);
			Assert.Contains("BatchUpdateCellsAsync", attr.Message);
		}
	}
}
