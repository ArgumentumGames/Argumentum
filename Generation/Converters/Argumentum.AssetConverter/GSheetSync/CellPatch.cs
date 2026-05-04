namespace Argumentum.AssetConverter.GSheetSync
{
	public enum PatchSkipReason
	{
		FormulaProtected,
		PkUnmatched,
		NoChange
	}

	public class CellPatch
	{
		public int Row { get; set; }
		public int Col { get; set; }
		public string A1Notation { get; set; } = "";
		public string ColumnName { get; set; } = "";
		public string PrimaryKey { get; set; } = "";
		public string OldValue { get; set; } = "";
		public string NewValue { get; set; } = "";
		public PatchSkipReason? SkipReason { get; set; }
	}
}
