using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Argumentum.AssetConverter.GSheetSync;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
    public class FallaciesPullTests
    {
        private readonly ITestOutputHelper _output;

        private const string CredentialsPath = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\GSheet-OAuth-Credentials.txt";
        private const string TokenPath = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\GSheet-RefreshToken.txt";

        private const string SpreadsheetId = "1TrQUyzXMMM-9pHdNWz1fdJ3xQ5XcHgwVH52SOnM61ow";
        private const int Gid = 969304769;

        public FallaciesPullTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "Manual only — requires OAuth + network access")]
        public async Task Compare_Fallacies_GDrive_vs_Local()
        {
            var authManager = new GSheetAuthManager(CredentialsPath, TokenPath);
            var credential = await authManager.GetCredentialAsync();
            var service = new GSheetService(credential);

            var gridData = await service.GetSheetDataAsync(SpreadsheetId, Gid);
            _output.WriteLine($"GDrive: {gridData.Count} rows (1 header + {gridData.Count - 1} data)");

            // Read local CSV
            var localPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv");
            localPath = Path.GetFullPath(localPath);

            var localRows = new List<string[]>();
            using var reader = new StreamReader(localPath, new UTF8Encoding(true));
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            });

            csv.Read();
            csv.ReadHeader();
            var localHeader = csv.HeaderRecord ?? Array.Empty<string>();
            _output.WriteLine($"Local: {localHeader.Length} columns, header: {string.Join(",", localHeader.Take(10))}...");

            while (csv.Read())
            {
                var row = new string[csv.Parser.Count];
                for (int i = 0; i < csv.Parser.Count; i++)
                {
                    row[i] = csv.GetField(i) ?? "";
                }
                localRows.Add(row);
            }
            _output.WriteLine($"Local: {localRows.Count} data rows");

            // GDrive header
            var gdriveHeader = gridData[0].Select(c => c?.ToString() ?? "").ToArray();
            _output.WriteLine($"GDrive: {gdriveHeader.Length} columns, header: {string.Join(",", gdriveHeader.Take(10))}...");

            // Column count comparison
            _output.WriteLine($"\nColumn count: Local={localHeader.Length}, GDrive={gdriveHeader.Length}");

            // Find column differences
            var localCols = localHeader.ToHashSet();
            var gdriveCols = gdriveHeader.ToHashSet();
            var added = gdriveCols.Except(localCols).ToList();
            var removed = localCols.Except(gdriveCols).ToList();
            if (added.Any()) _output.WriteLine($"Columns only in GDrive: {string.Join(", ", added)}");
            if (removed.Any()) _output.WriteLine($"Columns only in Local: {string.Join(", ", removed)}");

            // Row count comparison
            var gdriveDataRows = gridData.Skip(1).ToList();
            _output.WriteLine($"\nData rows: Local={localRows.Count}, GDrive={gdriveDataRows.Count}");
            _output.WriteLine($"Difference: {gdriveDataRows.Count - localRows.Count}");

            // PK column index (should be 0)
            int pkColLocal = Array.IndexOf(localHeader, "PK");
            if (pkColLocal < 0) pkColLocal = Array.IndexOf(localHeader, "pk");
            int pkColGdrive = Array.IndexOf(gdriveHeader, "PK");
            if (pkColGdrive < 0) pkColGdrive = Array.IndexOf(gdriveHeader, "pk");
            _output.WriteLine($"\nPK column index: Local={pkColLocal}, GDrive={pkColGdrive}");

            if (pkColLocal < 0 || pkColGdrive < 0)
            {
                // #1046 Lot C (LOW #12): the bare `return` silently abandoned the rest of the
                // comparison — if this manual script is ever un-skipped, a missing pk column
                // must fail loud instead of ending as an unconditional pass.
                (pkColLocal >= 0 && pkColGdrive >= 0).Should().BeTrue(
                    $"pk column not found (Local={pkColLocal}, GDrive={pkColGdrive}) — comparison cannot proceed");
            }

            // Build PK lookup
            var localByPk = localRows.ToDictionary(r => r[pkColLocal], r => r);
            var gdriveByPk = new Dictionary<string, IList<object>>();
            foreach (var row in gdriveDataRows)
            {
                var pk = row.Count > pkColGdrive ? row[pkColGdrive]?.ToString() ?? "" : "";
                if (!string.IsNullOrEmpty(pk))
                {
                    gdriveByPk[pk] = row;
                }
            }

            _output.WriteLine($"\nPKs: Local={localByPk.Count}, GDrive={gdriveByPk.Count}");

            // Find extra GDrive PKs
            var extraInGdrive = gdriveByPk.Keys.Except(localByPk.Keys).OrderBy(k => int.TryParse(k, out var n) ? n : 0).ToList();
            var extraInLocal = localByPk.Keys.Except(gdriveByPk.Keys).OrderBy(k => int.TryParse(k, out var n) ? n : 0).ToList();

            if (extraInGdrive.Any())
            {
                _output.WriteLine($"\n--- PKs only in GDrive ({extraInGdrive.Count}) ---");
                foreach (var pk in extraInGdrive)
                {
                    var row = gdriveByPk[pk];
                    var name = row.Count > 11 ? row[11]?.ToString() ?? "" : "";
                    var famille = row.Count > 4 ? row[4]?.ToString() ?? "" : "";
                    _output.WriteLine($"  PK={pk}: Famille={famille}, nom_vulgarisé={name}");
                }
            }

            if (extraInLocal.Any())
            {
                _output.WriteLine($"\n--- PKs only in Local ({extraInLocal.Count}) ---");
                foreach (var pk in extraInLocal)
                {
                    var row = localByPk[pk];
                    var name = row.Length > 11 ? row[11] : "";
                    var famille = row.Length > 4 ? row[4] : "";
                    _output.WriteLine($"  PK={pk}: Famille={famille}, nom_vulgarisé={name}");
                }
            }

            // Compare common PKs for differences
            int modifiedCount = 0;
            var sampleDiffs = new List<string>();
            foreach (var pk in gdriveByPk.Keys.Intersect(localByPk.Keys))
            {
                var gdriveRow = gdriveByPk[pk];
                var localRow = localByPk[pk];
                bool different = false;
                int maxCols = Math.Max(localRow.Length, gdriveRow.Count);
                for (int c = 0; c < maxCols; c++)
                {
                    var localVal = c < localRow.Length ? localRow[c] : "";
                    var gdriveVal = c < gdriveRow.Count ? gdriveRow[c]?.ToString() ?? "" : "";
                    if (localVal != gdriveVal)
                    {
                        different = true;
                        break;
                    }
                }
                if (different)
                {
                    modifiedCount++;
                    if (sampleDiffs.Count < 10)
                    {
                        sampleDiffs.Add(pk);
                    }
                }
            }
            _output.WriteLine($"\nCommon PKs with differences: {modifiedCount}");
            if (sampleDiffs.Any())
            {
                _output.WriteLine($"Sample modified PKs: {string.Join(", ", sampleDiffs)}");
            }
        }
    }
}
