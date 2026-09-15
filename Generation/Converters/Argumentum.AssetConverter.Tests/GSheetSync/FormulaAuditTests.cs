using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Argumentum.AssetConverter.GSheetSync;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
    /// <summary>
    /// Manual-only tests for auditing formulas across all GSheet spreadsheets.
    /// Requires OAuth credentials + network access. Run individually:
    ///   dotnet test --filter "FormulaAudit" --no-restore
    /// </summary>
    public class FormulaAuditTests
    {
        private readonly ITestOutputHelper _output;

        private static readonly (string Name, string SpreadsheetId, int Gid)[] Spreadsheets = new[]
        {
            ("Fallacies Taxonomy", "1TrQUyzXMMM-9pHdNWz1fdJ3xQ5XcHgwVH52SOnM61ow", 969304769),
            ("Scenarii Cards",     "1SQb9R7Dpi0jPz2JX-HXk1WFn9t68e3aq9MCGif7lM10", 1376497878),
            ("Virtues Taxonomy",   "1Asxe0Kb3_pLUSWJnB1HNiBG_EOaz_oU_X3eO9ixnVhA", 349188118),
            ("Rules Cards",        "1jnhlod6PLgvVI-Qgrz3sTYytMgnrMyZrHcc8htPn_DQ", 0),
        };

        private const string CredentialsPath = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\GSheet-OAuth-Credentials.txt";
        private const string TokenPath = @"G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\GSheet-RefreshToken.txt";

        public FormulaAuditTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Skip = "Manual only — requires OAuth + network access")]
        public async Task Audit_All_Spreadsheets_Formulas()
        {
            var authManager = new GSheetAuthManager(CredentialsPath, TokenPath);
            var credential = await authManager.GetCredentialAsync();
            var service = new GSheetService(credential);

            var sb = new StringBuilder();
            sb.AppendLine("# Formula Audit — All GSheet Spreadsheets");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}");
            sb.AppendLine();

            int totalProtected = 0;
            var auditErrors = new List<string>();

            foreach (var (name, spreadsheetId, gid) in Spreadsheets)
            {
                _output.WriteLine($"Auditing: {name}...");
                sb.AppendLine($"## {name}");
                sb.AppendLine($"Spreadsheet: `{spreadsheetId}` | GID: `{gid}`");
                sb.AppendLine();

                try
                {
                    var snapshot = await service.GetSheetWithFormulasAsync(spreadsheetId, gid);
                    var protectedCells = snapshot.ProtectedCells;
                    totalProtected += protectedCells.Count;

                    sb.AppendLine($"**Protected cells (formulas): {protectedCells.Count}**");
                    sb.AppendLine();

                    if (protectedCells.Count == 0)
                    {
                        sb.AppendLine("No formulas detected. All cells are literal values.");
                    }
                    else
                    {
                        // Group by column to show formula patterns
                        var headerRow = snapshot.Formulas.Count > 0 ? snapshot.Formulas[0] : null;
                        var byColumn = protectedCells
                            .GroupBy(c => c.Col)
                            .OrderBy(g => g.Key);

                        sb.AppendLine("| Column | Header | Formula Count | Sample Formula |");
                        sb.AppendLine("|--------|--------|---------------|----------------|");

                        foreach (var colGroup in byColumn)
                        {
                            var col = colGroup.Key;
                            var header = headerRow != null && col < headerRow.Count
                                ? headerRow[col]?.ToString() ?? $"(col {col})"
                                : $"(col {col})";
                            var sample = snapshot.Formulas[colGroup.First().Row][col]?.ToString() ?? "";
                            if (sample.Length > 80) sample = sample[..77] + "...";

                            sb.AppendLine($"| {SheetSnapshot.ToA1Notation(0, col)} | {header} | {colGroup.Count()} | `{sample}` |");
                        }

                        sb.AppendLine();

                        // List all unique formula patterns
                        var uniqueFormulas = new Dictionary<string, int>();
                        foreach (var (row, col) in protectedCells)
                        {
                            var formula = snapshot.Formulas[row][col]?.ToString() ?? "";
                            // Normalize cell references for grouping
                            var pattern = System.Text.RegularExpressions.Regex.Replace(
                                formula, @"[A-Z]+\d+", "CELL");
                            if (!uniqueFormulas.ContainsKey(pattern))
                                uniqueFormulas[pattern] = 0;
                            uniqueFormulas[pattern]++;
                        }

                        sb.AppendLine("### Unique Formula Patterns");
                        sb.AppendLine();
                        sb.AppendLine("| Pattern | Count | Example |");
                        sb.AppendLine("|---------|-------|---------|");

                        foreach (var (pattern, count) in uniqueFormulas
                            .OrderByDescending(x => x.Value))
                        {
                            // Find first example
                            var example = protectedCells
                                .FirstOrDefault(c =>
                                {
                                    var f = snapshot.Formulas[c.Row][c.Col]?.ToString() ?? "";
                                    var p = System.Text.RegularExpressions.Regex.Replace(
                                        f, @"[A-Z]+\d+", "CELL");
                                    return p == pattern;
                                });
                            var exampleFormula = snapshot.Formulas[example.Row][example.Col]?.ToString() ?? "";
                            if (exampleFormula.Length > 100) exampleFormula = exampleFormula[..97] + "...";

                            var pat = pattern.Length > 60 ? pattern[..57] + "..." : pattern;
                            sb.AppendLine($"| `{pat}` | {count} | `{exampleFormula}` |");
                        }
                    }

                    sb.AppendLine();
                    sb.AppendLine("---");
                    sb.AppendLine();
                }
                catch (Exception ex)
                {
                    // #1046 MED #10: count instead of swallow — without this the 4 audits
                    // could all fail and the test would still pass green.
                    auditErrors.Add($"{name}: {ex.Message}");
                    sb.AppendLine($"**ERROR**: {ex.Message}");
                    sb.AppendLine();
                    _output.WriteLine($"  ERROR: {ex.Message}");
                }
            }

            sb.AppendLine($"## Summary");
            sb.AppendLine();
            sb.AppendLine($"- Total protected cells across all spreadsheets: **{totalProtected}**");
            sb.AppendLine();

            var reportPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..", "..",
                "docs", "investigations", "2026-05-01-formulas-per-spreadsheet.md");
            reportPath = Path.GetFullPath(reportPath);

            var dir = Path.GetDirectoryName(reportPath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

            await File.WriteAllTextAsync(reportPath, sb.ToString(), new UTF8Encoding(false));

            _output.WriteLine($"Report written to: {reportPath}");
            _output.WriteLine($"Total protected cells: {totalProtected}");

            // #1046 MED #10 (assertion half): a manual run that fails to audit any
            // spreadsheet must be red, not a green report full of **ERROR** lines.
            Assert.Empty(auditErrors);
        }
    }
}
