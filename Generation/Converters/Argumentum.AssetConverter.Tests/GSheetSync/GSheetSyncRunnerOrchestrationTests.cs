using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Argumentum.AssetConverter.GSheetSync;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
	/// <summary>
	/// #1618 — orchestration guards of <see cref="GSheetSyncRunner"/>, exercised
	/// with an in-memory <see cref="IGSheetService"/> (no OAuth, no network):
	/// point 1 (download runs the upload safety thresholds before writing),
	/// point 6 (atomic CSV write), point 7 (disabled guards are traced),
	/// point 8 (post-upload verification failures raise), point 10 (the
	/// orchestrations themselves are covered).
	/// Shares the "GSheetSyncConsoleCapture" collection: these tests redirect
	/// Console.Out and must not run in parallel with each other.
	/// </summary>
	[Collection("GSheetSyncConsoleCapture")]
	public class GSheetSyncRunnerOrchestrationTests : IDisposable
	{
		private readonly string _tempDir;
		private readonly string _csvPath;

		public GSheetSyncRunnerOrchestrationTests()
		{
			_tempDir = Path.Combine(Path.GetTempPath(), $"gsheet-runner-{Guid.NewGuid():N}");
			Directory.CreateDirectory(_tempDir);
			_csvPath = Path.Combine(_tempDir, "corpus.csv");
		}

		public void Dispose()
		{
			if (Directory.Exists(_tempDir))
			{
				Directory.Delete(_tempDir, true);
			}
		}

		private GSheetSyncConfig BaseConfig()
		{
			return new GSheetSyncConfig
			{
				Name = "Test corpus",
				SpreadsheetId = "spreadsheet-id",
				Gid = 42,
				LocalCsvPath = _csvPath,
				PrimaryKeyColumn = "pk",
				Direction = SyncDirection.Download,
				DryRun = false,
				RequireConfirmation = false,
			};
		}

		private static async Task<string> CaptureOutputAsync(Func<Task> action)
		{
			var originalOut = Console.Out;
			var sw = new StringWriter();
			Console.SetOut(sw);
			try
			{
				await action();
				return sw.ToString();
			}
			finally
			{
				Console.SetOut(originalOut);
			}
		}

		// ---------- point 1: download safety ----------

		[Fact]
		public async Task Download_TruncatedSheet_IsBlockedBeforeWrite()
		{
			const string original = "pk,name\n1,alpha\n2,beta\n3,gamma\n";
			await File.WriteAllTextAsync(_csvPath, original);

			// Upstream sheet truncated: headers survive, every data row is gone.
			var fake = new FakeGSheetService
			{
				SheetData = new List<IList<object>>
				{
					new List<object> { "pk", "name" },
				},
			};
			var runner = new GSheetSyncRunner(BaseConfig(), fake, "Fallacies");

			var output = await CaptureOutputAsync(() => runner.RunAsync());

			(await File.ReadAllTextAsync(_csvPath)).Should().Be(original,
				"a ~100% deletion diff must abort before the local CSV is touched");
			output.Should().Contain("Safety check FAILED");
			output.Should().Contain("Deletion threshold exceeded");
			File.Exists(_csvPath + ".tmp").Should().BeFalse();
		}

		[Fact]
		public async Task Download_FullyEmptySheet_IsBlockedByColumnStructure()
		{
			const string original = "pk,name\n1,alpha\n";
			await File.WriteAllTextAsync(_csvPath, original);

			var fake = new FakeGSheetService { SheetData = new List<IList<object>>() };
			var runner = new GSheetSyncRunner(BaseConfig(), fake, "Fallacies");

			var output = await CaptureOutputAsync(() => runner.RunAsync());

			(await File.ReadAllTextAsync(_csvPath)).Should().Be(original);
			output.Should().Contain("Safety check FAILED");
			output.Should().Contain("Column structure change");
		}

		[Fact]
		public async Task Download_HealthySheet_WritesCsvAndTracesDisabledConfirmation()
		{
			await File.WriteAllTextAsync(_csvPath, "pk,name\n1,alpha\n2,beta\n");

			var fake = new FakeGSheetService
			{
				SheetData = new List<IList<object>>
				{
					new List<object> { "pk", "name" },
					new List<object> { "1", "alpha" },
					new List<object> { "2", "beta-updated" },
				},
			};
			var runner = new GSheetSyncRunner(BaseConfig(), fake, "Fallacies");

			var output = await CaptureOutputAsync(() => runner.RunAsync());

			var after = await File.ReadAllTextAsync(_csvPath);
			after.Should().Contain("alpha");
			after.Should().Contain("beta-updated");
			File.Exists(_csvPath + ".tmp").Should().BeFalse();
			output.Should().Contain("RequireConfirmation=false",
				"a disabled guard must be traced (point 7)");
		}

		[Fact]
		public async Task Download_DryRun_DoesNotWrite()
		{
			const string original = "pk,name\n1,alpha\n2,beta\n";
			await File.WriteAllTextAsync(_csvPath, original);

			var config = BaseConfig();
			config.DryRun = true;

			var fake = new FakeGSheetService
			{
				SheetData = new List<IList<object>>
				{
					new List<object> { "pk", "name" },
					new List<object> { "1", "alpha" },
					new List<object> { "2", "beta-updated" },
				},
			};
			var runner = new GSheetSyncRunner(config, fake, "Fallacies");

			var output = await CaptureOutputAsync(() => runner.RunAsync());

			(await File.ReadAllTextAsync(_csvPath)).Should().Be(original);
			output.Should().Contain("[DRY RUN]");
		}

		// ---------- points 8 + 10: upload variants ----------

		[Fact]
		public async Task CellLevelUpload_VerificationMismatch_ThrowsNamedFailure()
		{
			await File.WriteAllTextAsync(_csvPath, "pk,name,value\n1,alpha-new,100\n2,beta,200\n");

			var sheetGrid = new List<IList<object>>
			{
				new List<object> { "pk", "name", "value" },
				new List<object> { "1", "alpha", "100" },
				new List<object> { "2", "beta", "200" },
			};
			var fake = new FakeGSheetService
			{
				SheetTitle = "Fallacies",
				Snapshot = new SheetSnapshot
				{
					Values = sheetGrid,
					Formulas = sheetGrid,
				},
				VerifyMismatches = new List<string>
				{
					"B2 (PK=1, col=name): expected 'alpha-new', got 'alpha'",
				},
			};

			var config = BaseConfig();
			config.Direction = SyncDirection.Upload;
			config.CreateBackupBeforeUpload = false;

			var runner = new GSheetSyncRunner(config, fake, "Fallacies");

			var originalOut = Console.Out;
			Console.SetOut(new StringWriter());
			InvalidOperationException ex;
			try
			{
				ex = await Assert.ThrowsAsync<InvalidOperationException>(() => runner.RunAsync());
			}
			finally
			{
				Console.SetOut(originalOut);
			}

			ex.Message.Should().Contain("FAILED");
			ex.Message.Should().Contain("Backup");
			fake.AppliedPatches.Should().HaveCount(1,
				"the patch ran; the failure surfaces at verification (point 8)");
		}

		[Fact]
		public async Task CellLevelUpload_SafetyFailure_AppliesNoPatches()
		{
			// The local CSV lost rows (bad edit): uploading it would drop 2 of the
			// 3 sheet rows — the row-level safety check must abort before patches,
			// before confirmation, and before any backup tab is created.
			await File.WriteAllTextAsync(_csvPath, "pk,name,value\n1,alpha,100\n");

			var sheetGrid = new List<IList<object>>
			{
				new List<object> { "pk", "name", "value" },
				new List<object> { "1", "alpha", "100" },
				new List<object> { "2", "beta", "200" },
				new List<object> { "3", "gamma", "300" },
			};
			var fake = new FakeGSheetService
			{
				SheetTitle = "Fallacies",
				Snapshot = new SheetSnapshot
				{
					Values = sheetGrid,
					Formulas = sheetGrid,
				},
			};

			var config = BaseConfig();
			config.Direction = SyncDirection.Upload;
			config.CreateBackupBeforeUpload = true;

			var runner = new GSheetSyncRunner(config, fake, "Fallacies");

			var output = await CaptureOutputAsync(() => runner.RunAsync());

			output.Should().Contain("Safety check FAILED");
			output.Should().Contain("Deletion threshold exceeded");
			fake.AppliedPatches.Should().BeEmpty();
			fake.CreatedBackups.Should().BeEmpty("no backup is needed when the upload never starts");
		}

		[Fact]
		public async Task FullSheetUpload_RowCountMismatch_ThrowsNamedFailure()
		{
			await File.WriteAllTextAsync(_csvPath, "pk,name\n1,alpha\n2,beta\n");

			var fake = new FakeGSheetService
			{
				SheetTitle = "Fallacies",
				SheetData = new List<IList<object>>
				{
					new List<object> { "pk", "name" },
					new List<object> { "1", "alpha" },
					new List<object> { "2", "beta" },
				},
				// Re-read after upload returns fewer rows than were written.
				VerifySheetData = new List<IList<object>>
				{
					new List<object> { "pk", "name" },
				},
			};

			var config = BaseConfig();
			config.Direction = SyncDirection.Upload;
			config.UseCellLevelUpload = false;
			config.CreateBackupBeforeUpload = false;

			var runner = new GSheetSyncRunner(config, fake, "Fallacies");

			var originalOut = Console.Out;
			Console.SetOut(new StringWriter());
			InvalidOperationException ex;
			try
			{
				ex = await Assert.ThrowsAsync<InvalidOperationException>(() => runner.RunAsync());
			}
			finally
			{
				Console.SetOut(originalOut);
			}

			ex.Message.Should().Contain("Row count mismatch");
			fake.UploadedGrids.Should().HaveCount(1, "the legacy overwrite itself did run");
		}

		// ---------- point 6: atomic write ----------

		[Fact]
		public async Task WriteCsvAtomically_ReplacesTargetWholly_NoTempResidue()
		{
			var target = Path.Combine(_tempDir, "atomic.csv");
			await File.WriteAllTextAsync(target, "old-contents-much-longer-than-the-new-one");

			await GSheetSyncRunner.WriteCsvAtomicallyAsync(target, "new\n");

			(await File.ReadAllTextAsync(target)).Should().Be("new\n");
			File.Exists(target + ".tmp").Should().BeFalse();
		}

		[Fact]
		public async Task WriteCsvAtomically_FailedRename_LeavesTargetAndCleansTemp()
		{
			// Target path is an existing DIRECTORY: the rename must fail, the temp
			// must be cleaned up, and the pre-existing target must survive.
			var targetDir = Path.Combine(_tempDir, "target-as-directory");
			Directory.CreateDirectory(targetDir);

			await Assert.ThrowsAnyAsync<Exception>(
				() => GSheetSyncRunner.WriteCsvAtomicallyAsync(targetDir, "data\n"));

			Directory.Exists(targetDir).Should().BeTrue("the failed write must not destroy the target");
			File.Exists(targetDir + ".tmp").Should().BeFalse("the temp file must be cleaned up");
		}

		// ---------- in-memory service ----------

		/// <summary>
		/// Mirrors the observable contract of <see cref="GSheetService"/> —
		/// including grid-to-CSV quoting — without OAuth or network.
		/// </summary>
		private sealed class FakeGSheetService : IGSheetService
		{
			public IList<IList<object>> SheetData { get; set; } = new List<IList<object>>();
			public IList<IList<object>> VerifySheetData { get; set; } = new List<IList<object>>();
			public string SheetTitle { get; set; } = "Sheet1";
			public SheetSnapshot Snapshot { get; set; } = new SheetSnapshot();
			public List<string> VerifyMismatches { get; set; } = new List<string>();
			public List<CellPatch> AppliedPatches { get; } = new List<CellPatch>();
			public List<string> CreatedBackups { get; } = new List<string>();
			public List<IList<IList<object>>> UploadedGrids { get; } = new List<IList<IList<object>>>();

			public Task<IList<IList<object>>> GetSheetDataAsync(string spreadsheetId, int gid)
				=> Task.FromResult(SheetData);

			public Task<SheetSnapshot> GetSheetWithFormulasAsync(string spreadsheetId, int gid)
				=> Task.FromResult(Snapshot);

			public Task<string> GetSheetTitleByGidAsync(string spreadsheetId, int gid)
				=> Task.FromResult(SheetTitle);

			public string GridToCsv(IList<IList<object>> grid)
			{
				if (grid == null || grid.Count == 0) return "";

				var sb = new StringBuilder();
				foreach (var row in grid)
				{
					if (row == null) continue;
					for (int i = 0; i < row.Count; i++)
					{
						if (i > 0) sb.Append(',');
						sb.Append(QuoteIfNeeded(row[i]?.ToString() ?? ""));
					}
					sb.Append('\n');
				}

				return sb.ToString();
			}

			private static string QuoteIfNeeded(string value)
			{
				if (value.IndexOfAny(new[] { ',', '"', '\n', '\r' }) < 0) return value;
				return "\"" + value.Replace("\"", "\"\"") + "\"";
			}

			public Task<string> CreateBackupSheetAsync(string spreadsheetId, string sourceSheetTitle)
			{
				var title = $"Backup fake-{CreatedBackups.Count}";
				CreatedBackups.Add(title);
				return Task.FromResult(title);
			}

			public Task BatchUpdateCellsAsync(
				string spreadsheetId, string sheetTitle, List<CellPatch> patches)
			{
				AppliedPatches.AddRange(patches);
				return Task.CompletedTask;
			}

			public Task<List<string>> VerifyCellPatchesAsync(
				string spreadsheetId, string sheetTitle, List<CellPatch> patches)
				=> Task.FromResult(VerifyMismatches);

			public Task UpdateSheetDataAsync(
				string spreadsheetId, string sheetTitle, IList<IList<object>> grid)
			{
				UploadedGrids.Add(grid);
				return Task.CompletedTask;
			}

			public Task<IList<IList<object>>> VerifySheetDataAsync(
				string spreadsheetId, string sheetTitle)
				=> Task.FromResult(VerifySheetData);
		}
	}
}