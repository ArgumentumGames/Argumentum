using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter.GSheetSync;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
    /// <summary>
    /// Contract tests for the home-grown CSV↔grid converters (<see cref="GSheetService.CsvToGrid"/>
    /// and <see cref="GSheetService.GridToCsv"/>). These two functions are the bridge between a
    /// Google Sheet download and a local CSV, and between a local CSV and a Sheets upload — so the
    /// invariant that matters most is **round-trip symmetry** (grid → csv → grid identity) on the
    /// edge-case data that real Argumentum CSVs actually contain: embedded newlines inside quoted
    /// fields (descriptions, examples), escaped double-quotes, empty cells, ragged rows.
    ///
    /// Two fragile behaviours are pinned here that are NOT obvious from reading the code:
    ///
    /// 1. <see cref="GSheetService.CsvToGrid"/> is configured with <c>HasHeaderRecord = true</c>,
    ///    but because the parse loop is manual (<c>while (csv.Read())</c> reading fields each
    ///    iteration, with no separate <c>ReadHeader()</c> step), the header line is NOT dropped —
    ///    it is returned as the first grid row. This is deliberate: Sheets expects the header row
    ///    in the uploaded grid. A "cleanup" that switched to <c>GetRecords&lt;T&gt;()</c> or added
    ///    an explicit header skip would silently drop row 0 and break every upload.
    ///
    /// 2. The read loop is bounded by <c>csv.Parser.Count</c>, which is the **current record's**
    ///    field count (dynamic, per-row), not the header's. Ragged rows (rows with fewer/more
    ///    fields than the header) are therefore preserved without throwing, and round-trip without
    ///    padding/truncation.
    /// </summary>
    public class CsvGridConversionContractTests
    {
        private static readonly GSheetService Service = new GSheetService(null!);

        /// <summary>
        /// Asserts the critical round-trip invariant: grid → GridToCsv → CsvToGrid yields a grid
        /// structurally identical to the input (same row count, same cells per row).
        /// </summary>
        private static void AssertRoundTripIdentity(IList<IList<object>> original)
        {
            var csv = Service.GridToCsv(original);
            var roundTripped = GSheetService.CsvToGrid(csv);

            roundTripped.Should().HaveCount(original.Count,
                "round-trip must not add or drop rows");
            for (int i = 0; i < original.Count; i++)
            {
                roundTripped[i].Select(c => c.ToString())
                    .Should().Equal(original[i].Select(c => c.ToString()),
                        $"row {i} must round-trip cell-for-cell");
            }
        }

        // --- CsvToGrid: RFC 4180 edge cases on the parse direction ---

        [Fact]
        public void CsvToGrid_ParsesQuotedFieldWithEmbeddedNewline()
        {
            // A newline embedded inside a quoted field must be read as part of that single field,
            // NOT as a record separator. This is RFC 4180 §2.6 and the highest-risk CSV edge case
            // for Argumentum data (descriptions/examples routinely span multiple lines).
            var csv = "pk,desc\n1,\"line1\nline2\"\n";

            var grid = GSheetService.CsvToGrid(csv);

            grid.Should().HaveCount(2, "the embedded newline must not create a third row");
            grid[1].Should().HaveCount(2);
            grid[1][1].ToString().Should().Be("line1\nline2");
        }

        [Fact]
        public void CsvToGrid_ParsesEscapedDoubleQuotes()
        {
            // RFC 4180: a double-quote inside a quoted field is escaped by doubling it ("").
            var csv = "pk,quote\n1,\"He said \"\"hi\"\"\"\n";

            var grid = GSheetService.CsvToGrid(csv);

            grid[1][1].ToString().Should().Be("He said \"hi\"");
        }

        // --- Round-trip symmetry on edge-case content (the sync invariant) ---

        [Fact]
        public void RoundTrip_PreservesQuotedFieldWithEmbeddedNewline()
        {
            var original = new List<IList<object>>
            {
                new List<object> { "pk", "desc" },
                new List<object> { "1", "First line\nSecond line\nThird line" },
            };

            AssertRoundTripIdentity(original);
        }

        [Fact]
        public void RoundTrip_PreservesEscapedDoubleQuotes()
        {
            var original = new List<IList<object>>
            {
                new List<object> { "pk", "quote" },
                new List<object> { "1", "She said \"hello\", then \"goodbye\"" },
            };

            AssertRoundTripIdentity(original);
        }

        [Fact]
        public void RoundTrip_PreservesEmptyAndCommaFields()
        {
            // Empty cells and cells containing commas (which force quoting on write) must survive
            // the round-trip without loss or re-splitting.
            var original = new List<IList<object>>
            {
                new List<object> { "pk", "name", "note" },
                new List<object> { "1", "", "a,b,c" },
                new List<object> { "2", "Foo, Bar", "" },
            };

            AssertRoundTripIdentity(original);
        }

        [Fact]
        public void RoundTrip_PreservesRaggedRows()
        {
            // Rows with differing field counts must round-trip without padding or truncation.
            // This pins the dynamic csv.Parser.Count bound (see class XML doc, behaviour #2).
            var original = new List<IList<object>>
            {
                new List<object> { "a", "b", "c" },
                new List<object> { "1", "2" },          // fewer fields than header
                new List<object> { "x", "y", "z", "w" }, // more fields than header
            };

            AssertRoundTripIdentity(original);
        }

        [Fact]
        public void RoundTrip_SingleColumnCsv_NoCommas()
        {
            // Degenerate single-column CSV (no delimiter at all) must still round-trip.
            var original = new List<IList<object>>
            {
                new List<object> { "header" },
                new List<object> { "value1" },
                new List<object> { "value2" },
            };

            AssertRoundTripIdentity(original);
        }

        // --- Degenerate inputs (no NRE, no spurious rows) ---

        [Fact]
        public void CsvToGrid_EmptyString_ReturnsEmptyGrid()
        {
            var grid = GSheetService.CsvToGrid("");

            grid.Should().BeEmpty("an empty CSV must not yield a phantom empty row");
        }

        [Fact]
        public void GridToCsv_EmptyOrNullGrid_ReturnsEmptyString()
        {
            Service.GridToCsv(new List<IList<object>>()).Should().BeEmpty();
            Service.GridToCsv(null).Should().BeEmpty();
        }

        [Fact]
        public void GridToCsv_NullCells_BecomeEmptyStrings()
        {
            // Pins the `cell?.ToString() ?? ""` guard: a null cell serialises as an empty string,
            // not as the literal text "null", and must not throw.
            var grid = new List<IList<object>>
            {
                new List<object> { "a", null, "c" },
            };

            var csv = Service.GridToCsv(grid);
            var roundTripped = GSheetService.CsvToGrid(csv);

            roundTripped[0].Select(c => c.ToString()).Should().Equal("a", "", "c");
        }

        [Fact]
        public void CsvToGrid_TrailingNewline_DoesNotCreatePhantomRow()
        {
            // A single trailing newline (the normal CSV terminator) must not produce an extra empty
            // row. Two trailing newlines (one genuine empty line) DO produce one empty row.
            var csvWithTerminator = "a,b\n1,2\n";

            var grid = GSheetService.CsvToGrid(csvWithTerminator);

            grid.Should().HaveCount(2, "the terminator newline must not add a third row");
        }
    }
}
