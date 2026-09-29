using System.Collections.Generic;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.GSheetSync
{
	/// <summary>
	/// Surface of <see cref="GSheetService"/> consumed by <see cref="GSheetSyncRunner"/>.
	/// Exists so orchestration paths (download guards, upload guards) can be exercised
	/// by tests with an in-memory implementation — no OAuth, no network.
	/// </summary>
	public interface IGSheetService
	{
		Task<IList<IList<object>>> GetSheetDataAsync(string spreadsheetId, int gid);

		Task<SheetSnapshot> GetSheetWithFormulasAsync(string spreadsheetId, int gid);

		Task<string> GetSheetTitleByGidAsync(string spreadsheetId, int gid);

		string GridToCsv(IList<IList<object>> grid);

		Task<string> CreateBackupSheetAsync(string spreadsheetId, string sourceSheetTitle);

		Task BatchUpdateCellsAsync(string spreadsheetId, string sheetTitle, List<CellPatch> patches);

		Task<List<string>> VerifyCellPatchesAsync(string spreadsheetId, string sheetTitle, List<CellPatch> patches);

		Task UpdateSheetDataAsync(string spreadsheetId, string sheetTitle, IList<IList<object>> grid);

		Task<IList<IList<object>>> VerifySheetDataAsync(string spreadsheetId, string sheetTitle);
	}
}