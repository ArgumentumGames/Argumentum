using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Argumentum.AssetConverter.GSheetSync;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
    /// <summary>
    /// Manual-only test for pulling Rules GDrive baseline.
    /// Requires OAuth credentials + network access.
    /// </summary>
    public class RulesPullTests
    {
        private readonly ITestOutputHelper _output;

        private const string CredentialsPath = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\GSheet-OAuth-Credentials.txt";
        private const string TokenPath = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\GSheet-RefreshToken.txt";

        public RulesPullTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "Manual only — requires OAuth + network access")]
        public async Task Pull_Rules_GDrive_Baseline()
        {
            var authManager = new GSheetAuthManager(CredentialsPath, TokenPath);
            var credential = await authManager.GetCredentialAsync();
            var service = new GSheetService(credential);

            // Download GDrive data
            var gridData = await service.GetSheetDataAsync(
                "1jnhlod6PLgvVI-Qgrz3sTYytMgnrMyZrHcc8htPn_DQ", 0);

            _output.WriteLine($"Downloaded {gridData.Count} rows from GDrive (1 header + {gridData.Count - 1} data)");

            // Read local CSV to compare
            var localPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\..\..\..\Cards\Rules\Argumentum Rules - Cards.csv");
            localPath = Path.GetFullPath(localPath);
            var localContent = await File.ReadAllTextAsync(localPath, new UTF8Encoding(true));

            // GDrive has no pk column — add it
            // Build new CSV: pk,Text,Text_en,Text_ru,Text_pt,print_and_play
            var sb = new StringBuilder();
            sb.Append("pk,");

            // Header row from GDrive (skip adding pk to each data row's header since we prepend it)
            var headerRow = gridData[0];
            for (int i = 0; i < headerRow.Count; i++)
            {
                sb.Append(EscapeCsvField(headerRow[i]?.ToString() ?? ""));
                if (i < headerRow.Count - 1) sb.Append(',');
            }
            sb.AppendLine();

            // Data rows
            for (int row = 1; row < gridData.Count; row++)
            {
                var pk = $"Rules_{row:D2}";
                sb.Append(EscapeCsvField(pk));
                sb.Append(',');

                var dataRow = gridData[row];
                for (int col = 0; col < dataRow.Count; col++)
                {
                    sb.Append(EscapeCsvField(dataRow[col]?.ToString() ?? ""));
                    if (col < dataRow.Count - 1) sb.Append(',');
                }
                sb.AppendLine();
            }

            var newCsv = sb.ToString();

            // Show diff summary
            var diffEngine = new CsvDiffEngine("pk");
            var diff = diffEngine.Compare(localContent, newCsv);

            _output.WriteLine($"\nDiff: {diff.TotalRowsOld} → {diff.TotalRowsNew} rows");
            _output.WriteLine($"  Added: {diff.RowsAdded}, Deleted: {diff.RowsDeleted}, Modified: {diff.RowsModified}");
            _output.WriteLine($"  Columns added: [{string.Join(", ", diff.ColumnsAdded)}]");
            _output.WriteLine($"  Columns removed: [{string.Join(", ", diff.ColumnsRemoved)}]");

            // Write the new CSV (UTF-8 no-BOM)
            await File.WriteAllTextAsync(localPath, newCsv, new UTF8Encoding(false));
            _output.WriteLine($"\nWritten to: {localPath}");
        }

        private static string EscapeCsvField(string field)
        {
            if (field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }
            return field;
        }

        [Fact(Skip = "Manual only — requires OAuth + network access")]
        public async Task DryRun_Rules_GDrive_Baseline()
        {
            var authManager = new GSheetAuthManager(CredentialsPath, TokenPath);
            var credential = await authManager.GetCredentialAsync();
            var service = new GSheetService(credential);

            var gridData = await service.GetSheetDataAsync(
                "1jnhlod6PLgvVI-Qgrz3sTYytMgnrMyZrHcc8htPn_DQ", 0);

            _output.WriteLine($"GDrive: {gridData.Count} rows (header + {gridData.Count - 1} data)");
            _output.WriteLine($"GDrive columns: {string.Join(", ", gridData[0])}");

            // Check last 8 rows (existing 18 + new 6)
            _output.WriteLine("\n--- Last 8 data rows (existing tail + new rows) ---");
            for (int i = Math.Max(1, gridData.Count - 8); i < gridData.Count; i++)
            {
                var row = gridData[i];
                var textPreview = row[0]?.ToString()?.Substring(0, Math.Min(60, (row[0]?.ToString() ?? "").Length)) ?? "(empty)";
                var ppValue = row.Count > 4 ? row[4]?.ToString() ?? "" : "(missing)";
                _output.WriteLine($"  Row {i}: pk=Rules_{i:D2} | Text={textPreview}... | print_and_play={ppValue}");
            }

            // Compare with local first 18 rows
            _output.WriteLine("\n--- Diff check: first 18 rows local vs GDrive ---");
            var localPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\..\..\..\Cards\Rules\Argumentum Rules - Cards.csv");
            localPath = Path.GetFullPath(localPath);
            var localContent = await File.ReadAllTextAsync(localPath, new UTF8Encoding(true));

            var localLines = localContent.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            _output.WriteLine($"Local: {localLines.Count} lines (including header with multiline fields)");
        }
    }
}
