using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Argumentum.AssetConverter.GSheetSync
{
	public class GSheetService
	{
		private readonly SheetsService _sheetsService;

		public GSheetService(UserCredential credential)
		{
			_sheetsService = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer
			{
				HttpClientInitializer = credential,
				ApplicationName = "Argumentum GSheet Sync",
			});
		}

		/// <summary>
		/// Downloads all data from a sheet tab identified by its GID.
		/// Returns the raw cell values as a 2D grid.
		/// </summary>
		public async Task<IList<IList<object>>> GetSheetDataAsync(string spreadsheetId, int gid)
		{
			var sheetTitle = await GetSheetTitleByGidAsync(spreadsheetId, gid);
			var range = $"'{sheetTitle}'";
			var request = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
			var response = await request.ExecuteAsync();
			return response.Values ?? new List<IList<object>>();
		}

		/// <summary>
		/// Downloads a sheet tab twice in parallel — once with FORMULA render
		/// option (so cells holding formulas surface as <c>"=…"</c> strings)
		/// and once with UNFORMATTED_VALUE (so cells surface their evaluated
		/// values). Pairs the two grids into a <see cref="SheetSnapshot"/>
		/// with the protected-cells set pre-computed.
		/// </summary>
		public async Task<SheetSnapshot> GetSheetWithFormulasAsync(string spreadsheetId, int gid)
		{
			var sheetTitle = await GetSheetTitleByGidAsync(spreadsheetId, gid);
			var range = $"'{sheetTitle}'";

			var formulaRequest = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
			formulaRequest.ValueRenderOption =
				SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.FORMULA;

			var valueRequest = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
			valueRequest.ValueRenderOption =
				SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;

			var formulaTask = formulaRequest.ExecuteAsync();
			var valueTask = valueRequest.ExecuteAsync();
			await Task.WhenAll(formulaTask, valueTask);

			var formulas = formulaTask.Result.Values ?? new List<IList<object>>();
			var values = valueTask.Result.Values ?? new List<IList<object>>();

			return new SheetSnapshot
			{
				Values = values,
				Formulas = formulas,
				ProtectedCells = SheetSnapshot.BuildProtectedCells(formulas),
			};
		}

		/// <summary>
		/// Converts a 2D grid of cell values to a CSV string.
		/// </summary>
		public string GridToCsv(IList<IList<object>> grid)
		{
			if (grid == null || grid.Count == 0)
				return "";

			using var writer = new StringWriter();
			using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = false,
			});

			foreach (var row in grid)
			{
				if (row != null)
				{
					foreach (var cell in row)
					{
						csv.WriteField(cell?.ToString() ?? "");
					}
				}

				csv.NextRecord();
			}

			return writer.ToString();
		}

		/// <summary>
		/// Resolves a GID (numeric tab ID) to the sheet title needed by the API.
		/// </summary>
		public async Task<string> GetSheetTitleByGidAsync(string spreadsheetId, int gid)
		{
			var request = _sheetsService.Spreadsheets.Get(spreadsheetId);
			request.IncludeGridData = false;
			var spreadsheet = await request.ExecuteAsync();

			foreach (var sheet in spreadsheet.Sheets)
			{
				var properties = sheet.Properties;
				if (properties?.SheetId == gid)
				{
					return properties.Title;
				}
			}

			throw new InvalidOperationException(
				$"Sheet with GID {gid} not found in spreadsheet {spreadsheetId}.");
		}

		/// <summary>
		/// Creates a backup tab from existing data, named "Backup YYYY-MM-DD HH-mm-ss".
		/// Returns the title of the new backup sheet.
		/// </summary>
		public async Task<string> CreateBackupSheetAsync(string spreadsheetId, string sourceSheetTitle)
		{
			var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
			var backupTitle = $"Backup {timestamp}";

			// Step 1: Read source data
			var range = $"'{sourceSheetTitle}'";
			var getRequest = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
			var getResponse = await getRequest.ExecuteAsync();
			var sourceData = getResponse.Values;

			if (sourceData == null || sourceData.Count == 0)
			{
				throw new InvalidOperationException(
					$"Source sheet '{sourceSheetTitle}' is empty — nothing to backup.");
			}

			// Step 2: Add a new sheet tab
			var addSheetRequest = new Request
			{
				AddSheet = new AddSheetRequest
				{
					Properties = new SheetProperties
					{
						Title = backupTitle,
					}
				}
			};

			var batchUpdateRequest = new BatchUpdateSpreadsheetRequest
			{
				Requests = new List<Request> { addSheetRequest }
			};

			var batchUpdate = _sheetsService.Spreadsheets.BatchUpdate(
				batchUpdateRequest, spreadsheetId);
			await batchUpdate.ExecuteAsync();

			// Step 3: Write source data to the new backup tab
			var backupRange = $"'{backupTitle}'";
			var valueRange = new ValueRange
			{
				Range = backupRange,
				Values = sourceData
			};

			var updateRequest = _sheetsService.Spreadsheets.Values.Update(
				valueRange, spreadsheetId, backupRange);
			updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
			await updateRequest.ExecuteAsync();

			return backupTitle;
		}

		/// <summary>
		/// Writes a 2D grid to a sheet tab (clears existing data first, then writes).
		/// Obsolete: prefer cell-level upload via <see cref="BatchUpdateCellsAsync"/>
		/// which preserves formulas and avoids destructive full-sheet overwrite.
		/// </summary>
		[Obsolete("Use BatchUpdateCellsAsync for formula-aware cell-level updates. " +
		          "Full-sheet clear+write destroys formulas.")]
		public async Task UpdateSheetDataAsync(string spreadsheetId, string sheetTitle, IList<IList<object>> grid)
		{
			var range = $"'{sheetTitle}'";

			// Clear existing data
			var clearRequest = _sheetsService.Spreadsheets.Values.Clear(
				new ClearValuesRequest(), spreadsheetId, range);
			await clearRequest.ExecuteAsync();

			// Write new data
			var valueRange = new ValueRange
			{
				Range = range,
				Values = grid
			};

			var updateRequest = _sheetsService.Spreadsheets.Values.Update(
				valueRange, spreadsheetId, range);
			updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
			await updateRequest.ExecuteAsync();
		}

		/// <summary>
		/// Re-reads sheet data after upload for verification.
		/// </summary>
		public async Task<IList<IList<object>>> VerifySheetDataAsync(string spreadsheetId, string sheetTitle)
		{
			var range = $"'{sheetTitle}'";
			var request = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
			var response = await request.ExecuteAsync();
			return response.Values ?? new List<IList<object>>();
		}

		/// <summary>
		/// Parses a CSV string into a 2D grid of objects suitable for Sheets API upload.
		/// </summary>
		public static IList<IList<object>> CsvToGrid(string csvContent)
		{
			var grid = new List<IList<object>>();

			using var reader = new StringReader(csvContent);
			using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true,
				Delimiter = ",",
			});

			while (csv.Read())
			{
				var row = new List<object>();
				for (int i = 0; i < csv.Parser.Count; i++)
				{
					row.Add(csv.GetField(i) ?? "");
				}
				grid.Add(row);
			}

			return grid;
		}

		/// <summary>
		/// Applies targeted cell patches via <c>Values.BatchUpdate</c>.
		/// Each patch writes a single cell using RAW input mode so accidental
		/// <c>=</c> prefixes from CSV values are never interpreted as formulas.
		/// </summary>
		public async Task BatchUpdateCellsAsync(
			string spreadsheetId, string sheetTitle, List<CellPatch> patches)
		{
			if (patches == null || patches.Count == 0) return;

			var valueRanges = new List<ValueRange>();
			foreach (var patch in patches)
			{
				var a1 = $"'{sheetTitle}'!{patch.A1Notation}";
				valueRanges.Add(new ValueRange
				{
					Range = a1,
					Values = new List<IList<object>>
					{
						new List<object> { patch.NewValue }
					}
				});
			}

			var batchRequest = new BatchUpdateValuesRequest
			{
				Data = valueRanges,
				ValueInputOption = "RAW",
			};

			var request = _sheetsService.Spreadsheets.Values.BatchUpdate(
				batchRequest, spreadsheetId);
			var response = await request.ExecuteAsync();

			if (response.TotalUpdatedCells != patches.Count)
			{
				throw new InvalidOperationException(
					$"Batch update mismatch: expected {patches.Count} cells updated, " +
					$"got {response.TotalUpdatedCells}.");
			}
		}

		/// <summary>
		/// Re-reads patched cells and verifies each one matches the expected new value.
		/// Returns a list of mismatch descriptions (empty if all verified).
		/// </summary>
		public async Task<List<string>> VerifyCellPatchesAsync(
			string spreadsheetId, string sheetTitle, List<CellPatch> patches)
		{
			var mismatches = new List<string>();
			if (patches == null || patches.Count == 0) return mismatches;

			var ranges = patches
				.Select(p => $"'{sheetTitle}'!{p.A1Notation}")
				.ToList();

			var request = _sheetsService.Spreadsheets.Values.BatchGet(
				spreadsheetId);
			request.Ranges = ranges;
			request.ValueRenderOption =
				SpreadsheetsResource.ValuesResource.BatchGetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;

			var response = await request.ExecuteAsync();
			var valueRanges = response.ValueRanges ?? new List<ValueRange>();

			for (int i = 0; i < patches.Count; i++)
			{
				if (i >= valueRanges.Count)
				{
					mismatches.Add(
						$"{patches[i].A1Notation} (PK={patches[i].PrimaryKey}): " +
						$"no data returned from Sheets API");
					continue;
				}

				var vr = valueRanges[i];
				var actualValue = vr.Values?.FirstOrDefault()?.FirstOrDefault()?.ToString() ?? "";

				if (!string.Equals(
					NormalizeCell(actualValue),
					NormalizeCell(patches[i].NewValue),
					StringComparison.Ordinal))
				{
					mismatches.Add(
						$"{patches[i].A1Notation} (PK={patches[i].PrimaryKey}, " +
						$"col={patches[i].ColumnName}): " +
						$"expected '{Truncate(patches[i].NewValue, 50)}', " +
						$"got '{Truncate(actualValue, 50)}'");
				}
			}

			return mismatches;
		}

		private static string NormalizeCell(string value)
		{
			if (string.IsNullOrEmpty(value)) return "";
			return value.Replace("\r\n", "\n").Replace("\r", "\n").Trim();
		}

		private static string Truncate(string value, int maxLength)
		{
			if (value == null) return "";
			if (value.Length <= maxLength) return value;
			return value.Substring(0, maxLength - 3) + "...";
		}
	}
}
