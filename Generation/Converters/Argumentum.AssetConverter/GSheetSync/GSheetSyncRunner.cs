using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.GSheetSync
{
	public class GSheetSyncRunner
	{
		private readonly GSheetSyncConfig _config;

		public GSheetSyncRunner(GSheetSyncConfig config)
		{
			_config = config;
		}

		public async Task RunAsync()
		{
			Console.WriteLine($"  Spreadsheet: {_config.SpreadsheetId} (GID: {_config.Gid})");
			Console.WriteLine($"  Local CSV:   {_config.LocalCsvPath}");
			Console.WriteLine($"  Direction:   {_config.Direction}");
			Console.WriteLine($"  DryRun:      {_config.DryRun}");
			Console.WriteLine();

			if (_config.Direction == SyncDirection.Download || _config.Direction == SyncDirection.Both)
			{
				await RunDownloadAsync();
			}

			if (_config.Direction == SyncDirection.Upload || _config.Direction == SyncDirection.Both)
			{
				await RunUploadAsync();
			}
		}

		private async Task RunDownloadAsync()
		{
			Console.WriteLine("--- Download: GSheet → Local CSV ---");

			var (service, sheetTitle) = await InitializeServiceAsync();

			// Step 1: Download sheet data
			Console.WriteLine("  Downloading sheet data...");
			var gridData = await service.GetSheetDataAsync(_config.SpreadsheetId, _config.Gid);
			var downloadedCsv = service.GridToCsv(gridData);

			Console.WriteLine($"  Downloaded {gridData.Count} rows from '{sheetTitle}'");

			// Step 2: Load local CSV for comparison
			string localCsv = "";
			string localPath = ResolveLocalPath(_config.LocalCsvPath);

			if (File.Exists(localPath))
			{
				localCsv = await File.ReadAllTextAsync(localPath);
				Console.WriteLine($"  Local CSV: {localCsv.Split('\n').Length} lines");
			}
			else
			{
				Console.WriteLine($"  Local CSV not found at {localPath} — will create new file.");
			}

			// Step 3: Compute and display diff
			var diffEngine = new CsvDiffEngine(
				_config.PrimaryKeyColumn,
				_config.MaxOverwriteExamplesToShow,
				_config.CsvDelimiter);

			var diff = diffEngine.Compare(localCsv, downloadedCsv);
			DiffReport.PrintToConsole(diff, "Local CSV", "GSheet (downloaded)");

			// Step 4: Dry-run check
			if (_config.DryRun)
			{
				Console.WriteLine("  [DRY RUN] No local files modified. Set DryRun=false to apply changes.");
				return;
			}

			// Step 5: User confirmation
			if (_config.RequireConfirmation)
			{
				Console.Write("  Apply these changes to local CSV? (y/N): ");
				var response = Console.ReadLine()?.Trim().ToLowerInvariant();
				if (response != "y" && response != "yes")
				{
					Console.WriteLine("  Cancelled by user.");
					return;
				}
			}

			// Step 6: Write local CSV
			var directory = Path.GetDirectoryName(localPath);
			if (!string.IsNullOrEmpty(directory))
			{
				Directory.CreateDirectory(directory);
			}

			await File.WriteAllTextAsync(localPath, downloadedCsv);
			Console.WriteLine($"  ✓ Local CSV updated: {localPath}");
		}

		private async Task RunUploadAsync()
		{
			if (_config.UseCellLevelUpload)
			{
				await RunCellLevelUploadAsync();
			}
			else
			{
				await RunFullSheetUploadAsync();
			}
		}

		/// <summary>
		/// Cell-level upload: downloads GDrive with formulas, diffs against local CSV,
		/// patches only changed non-formula cells. Preserves formulas on GDrive.
		/// </summary>
		private async Task RunCellLevelUploadAsync()
		{
			Console.WriteLine("--- Upload: Local CSV → GSheet (cell-level, formula-aware) ---");

			// Step 1: Load local CSV
			string localPath = ResolveLocalPath(_config.LocalCsvPath);

			if (!File.Exists(localPath))
			{
				Console.WriteLine($"  ✗ Local CSV not found: {localPath}");
				return;
			}

			var localCsv = await File.ReadAllTextAsync(localPath);
			Console.WriteLine($"  Local CSV: {localCsv.Split('\n').Length} lines");

			// Step 2: Initialize service and download formula-aware snapshot
			var (service, sheetTitle) = await InitializeServiceAsync();

			Console.WriteLine("  Downloading formula-aware snapshot...");
			var snapshot = await service.GetSheetWithFormulasAsync(_config.SpreadsheetId, _config.Gid);
			Console.WriteLine($"  Downloaded {snapshot.Values.Count} rows ({snapshot.ProtectedCells.Count} formula-protected cells)");

			// Step 3: Compute cell-level diff
			var cellDiffEngine = new CellLevelDiffEngine(_config.PrimaryKeyColumn);
			var cellDiff = cellDiffEngine.Compare(snapshot, localCsv);

			Console.WriteLine($"  Cell diff: {cellDiff.PatchesToApply.Count} patches, " +
			                  $"{cellDiff.ProtectedSkips.Count} formula-protected, " +
			                  $"{cellDiff.PkUnmatched} PK unmatched");

			// Step 3b: Also run row-level diff for safety checks
			var currentSheetCsv = service.GridToCsv(snapshot.Values);
			var rowDiffEngine = new CsvDiffEngine(
				_config.PrimaryKeyColumn,
				_config.MaxOverwriteExamplesToShow,
				_config.CsvDelimiter);
			var rowDiff = rowDiffEngine.Compare(currentSheetCsv, localCsv);

			// Step 4: Safety check (row-level thresholds)
			var safetyChecker = new SyncSafetyChecker();
			var safetyResult = safetyChecker.Evaluate(rowDiff, _config);

			if (safetyResult.Warnings.Count > 0)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				foreach (var warning in safetyResult.Warnings)
				{
					Console.WriteLine($"  ⚠ {warning}");
				}
				Console.ResetColor();
			}

			if (!safetyResult.IsSafe)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("  ✗ Safety check FAILED — upload aborted:");
				foreach (var error in safetyResult.Errors)
				{
					Console.WriteLine($"    ✗ {error}");
				}
				Console.ResetColor();
				return;
			}

			// Step 5: Write DryRun report to file
			var reportDir = ResolveLocalPath(_config.SyncReportsPath);
			Directory.CreateDirectory(reportDir);
			var reportPath = Path.Combine(reportDir,
				$"{_config.Name}-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.md");
			var reportContent = cellDiff.ToReport();
			await File.WriteAllTextAsync(reportPath, reportContent);
			Console.WriteLine($"  DryRun report written: {reportPath}");

			if (cellDiff.PatchesToApply.Count == 0)
			{
				Console.WriteLine("  ✓ No patches to apply — local and GDrive are in sync.");
				return;
			}

			// Step 6: Dry-run check
			if (_config.DryRun)
			{
				Console.WriteLine("  [DRY RUN] No remote changes made. Set DryRun=false to apply changes.");
				Console.WriteLine($"  Report: {reportPath}");
				return;
			}

			// Step 7: User confirmation
			if (_config.RequireConfirmation)
			{
				Console.Write("  Upload cell-level patches to Google Sheets? (y/N): ");
				var response = Console.ReadLine()?.Trim().ToLowerInvariant();
				if (response != "y" && response != "yes")
				{
					Console.WriteLine("  Cancelled by user.");
					return;
				}
			}

			// Step 8: Create backup
			if (_config.CreateBackupBeforeUpload)
			{
				Console.WriteLine("  Creating backup tab...");
				var backupTitle = await service.CreateBackupSheetAsync(
					_config.SpreadsheetId, sheetTitle);
				Console.WriteLine($"  ✓ Backup created: '{backupTitle}'");
			}

			// Step 9: Apply patches
			Console.WriteLine($"  Applying {cellDiff.PatchesToApply.Count} cell patches...");
			await service.BatchUpdateCellsAsync(
				_config.SpreadsheetId, sheetTitle, cellDiff.PatchesToApply);
			Console.WriteLine($"  ✓ {cellDiff.PatchesToApply.Count} cells patched");

			// Step 10: Verification
			Console.WriteLine("  Verifying patches...");
			var mismatches = await service.VerifyCellPatchesAsync(
				_config.SpreadsheetId, sheetTitle, cellDiff.PatchesToApply);

			if (mismatches.Count > 0)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"  ⚠ {mismatches.Count} verification mismatches:");
				foreach (var mm in mismatches.Take(10))
				{
					Console.WriteLine($"    ⚠ {mm}");
				}
				if (mismatches.Count > 10)
					Console.WriteLine($"    ... (+{mismatches.Count - 10} more)");
				Console.ResetColor();
			}
			else
			{
				Console.WriteLine("  ✓ All patches verified — upload successful.");
			}
		}

		/// <summary>
		/// Full-sheet upload (legacy): clears entire sheet and writes local CSV.
		/// Destroys formulas. Kept for backward compatibility.
		/// </summary>
		private async Task RunFullSheetUploadAsync()
		{
			Console.WriteLine("--- Upload: Local CSV → GSheet (full-sheet, legacy) ---");

			// Step 1: Load local CSV
			string localPath = ResolveLocalPath(_config.LocalCsvPath);

			if (!File.Exists(localPath))
			{
				Console.WriteLine($"  ✗ Local CSV not found: {localPath}");
				return;
			}

			var localCsv = await File.ReadAllTextAsync(localPath);
			Console.WriteLine($"  Local CSV: {localCsv.Split('\n').Length} lines");

			// Step 2: Initialize service and download current sheet data
			var (service, sheetTitle) = await InitializeServiceAsync();

			Console.WriteLine("  Downloading current sheet data for comparison...");
			var gridData = await service.GetSheetDataAsync(_config.SpreadsheetId, _config.Gid);
			var currentSheetCsv = service.GridToCsv(gridData);

			// Step 3: Compute diff (local → sheet = what will change)
			var diffEngine = new CsvDiffEngine(
				_config.PrimaryKeyColumn,
				_config.MaxOverwriteExamplesToShow,
				_config.CsvDelimiter);

			var diff = diffEngine.Compare(currentSheetCsv, localCsv);
			DiffReport.PrintToConsole(diff, "GSheet (current)", "Local CSV (upload)");

			// Step 4: Safety check
			var safetyChecker = new SyncSafetyChecker();
			var safetyResult = safetyChecker.Evaluate(diff, _config);

			if (safetyResult.Warnings.Count > 0)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				foreach (var warning in safetyResult.Warnings)
				{
					Console.WriteLine($"  ⚠ {warning}");
				}
				Console.ResetColor();
			}

			if (!safetyResult.IsSafe)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("  ✗ Safety check FAILED — upload aborted:");
				foreach (var error in safetyResult.Errors)
				{
					Console.WriteLine($"    ✗ {error}");
				}
				Console.ResetColor();
				return;
			}

			// Step 5: Dry-run check
			if (_config.DryRun)
			{
				Console.WriteLine("  [DRY RUN] No remote changes made. Set DryRun=false to apply changes.");
				return;
			}

			// Step 6: User confirmation
			if (_config.RequireConfirmation)
			{
				Console.Write("  Upload these changes to Google Sheets? (y/N): ");
				var response = Console.ReadLine()?.Trim().ToLowerInvariant();
				if (response != "y" && response != "yes")
				{
					Console.WriteLine("  Cancelled by user.");
					return;
				}
			}

			// Step 7: Create backup
			if (_config.CreateBackupBeforeUpload)
			{
				Console.WriteLine("  Creating backup tab...");
				var backupTitle = await service.CreateBackupSheetAsync(
					_config.SpreadsheetId, sheetTitle);
				Console.WriteLine($"  ✓ Backup created: '{backupTitle}'");
			}

			// Step 8: Upload (legacy full-sheet overwrite)
#pragma warning disable CS0618 // Suppress obsolete warning for backward compat
			Console.WriteLine("  Uploading data (full-sheet overwrite)...");
			var uploadGrid = GSheetService.CsvToGrid(localCsv);
			await service.UpdateSheetDataAsync(
				_config.SpreadsheetId, sheetTitle, uploadGrid);
#pragma warning restore CS0618
			Console.WriteLine($"  ✓ Uploaded {uploadGrid.Count} rows to '{sheetTitle}'");

			// Step 9: Verification
			Console.WriteLine("  Verifying upload...");
			var verifyGrid = await service.VerifySheetDataAsync(
				_config.SpreadsheetId, sheetTitle);
			Console.WriteLine($"  ✓ Verification: {verifyGrid.Count} rows on remote sheet");

			if (verifyGrid.Count != uploadGrid.Count)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"  ⚠ Row count mismatch: uploaded {uploadGrid.Count}, found {verifyGrid.Count}");
				Console.ResetColor();
			}
			else
			{
				Console.WriteLine("  ✓ Row count matches — upload successful.");
			}
		}

		private async Task<(GSheetService service, string sheetTitle)> InitializeServiceAsync()
		{
			var authManager = new GSheetAuthManager(
				_config.OAuthCredentialsPath,
				_config.RefreshTokenPath);

			var credential = await authManager.GetCredentialAsync();
			var service = new GSheetService(credential);

			var sheetTitle = await service.GetSheetTitleByGidAsync(
				_config.SpreadsheetId, _config.Gid);

			return (service, sheetTitle);
		}

		private static string ResolveLocalPath(string path)
		{
			if (Path.IsPathRooted(path))
				return path;

			// Relative paths are relative to the project output directory
			var baseDir = AppDomain.CurrentDomain.BaseDirectory;
			return Path.GetFullPath(Path.Combine(baseDir, path));
		}
	}
}
