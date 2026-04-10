using System.Collections.Generic;

namespace Argumentum.AssetConverter.GSheetSync
{
	public enum SyncDirection
	{
		Download,
		Upload,
		Both
	}

	public class GSheetSyncConfig
	{
		public bool Enabled { get; set; } = false;

		public string Name { get; set; } = "";

		public string SpreadsheetId { get; set; } = "";

		public int Gid { get; set; }

		public string LocalCsvPath { get; set; } = "";

		public string PrimaryKeyColumn { get; set; } = "pk";

		public char CsvDelimiter { get; set; } = ',';

		public string OAuthCredentialsPath { get; set; } =
			@"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\GSheet-OAuth-Credentials.txt";

		public string RefreshTokenPath { get; set; } =
			@"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\GSheet-RefreshToken.txt";

		public SyncDirection Direction { get; set; } = SyncDirection.Download;

		public bool DryRun { get; set; } = true;

		public bool RequireConfirmation { get; set; } = true;

		public bool CreateBackupBeforeUpload { get; set; } = true;

		// Safety thresholds for upload
		public double MaxDeletionPercentage { get; set; } = 10.0;

		public double MaxModificationPercentage { get; set; } = 30.0;

		public bool AbortOnColumnStructureChange { get; set; } = true;

		public int MaxOverwriteExamplesToShow { get; set; } = 5;
	}
}
