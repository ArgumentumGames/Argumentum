using System;
using System.Collections.Generic;

namespace Argumentum.AssetConverter.GSheetSync
{
	public class SafetyCheckResult
	{
		public bool IsSafe { get; set; }
		public List<string> Warnings { get; set; } = new List<string>();
		public List<string> Errors { get; set; } = new List<string>();
	}

	public class SyncSafetyChecker
	{
		public SafetyCheckResult Evaluate(DiffResult diff, GSheetSyncConfig config)
		{
			var result = new SafetyCheckResult { IsSafe = true };

			// Check 1: Column structure change
			if (config.AbortOnColumnStructureChange && diff.HasColumnStructureChange)
			{
				result.IsSafe = false;
				result.Errors.Add(
					$"Column structure change detected: " +
					$"+{diff.ColumnsAdded.Count} columns added, " +
					$"-{diff.ColumnsRemoved.Count} columns removed. " +
					$"Set AbortOnColumnStructureChange=false to override.");
			}

			// Check 2: Deletion threshold
			if (diff.DeletionPercentage > config.MaxDeletionPercentage)
			{
				result.IsSafe = false;
				result.Errors.Add(
					$"Deletion threshold exceeded: " +
					$"{diff.DeletionPercentage:F1}% rows deleted " +
					$"(max allowed: {config.MaxDeletionPercentage}%). " +
					$"Increase MaxDeletionPercentage to override.");
			}

			// Check 3: Modification threshold
			if (diff.ModificationPercentage > config.MaxModificationPercentage)
			{
				result.IsSafe = false;
				result.Errors.Add(
					$"Modification threshold exceeded: " +
					$"{diff.ModificationPercentage:F1}% cells modified " +
					$"(max allowed: {config.MaxModificationPercentage}%). " +
					$"Increase MaxModificationPercentage to override.");
			}

			// Warnings (non-blocking)
			if (diff.RowsDeleted > 0 && diff.DeletionPercentage <= config.MaxDeletionPercentage)
			{
				result.Warnings.Add(
					$"{diff.RowsDeleted} rows will be deleted ({diff.DeletionPercentage:F1}%).");
			}

			if (diff.RowsAdded > 0)
			{
				result.Warnings.Add($"{diff.RowsAdded} new rows will be added.");
			}

			return result;
		}
	}
}
