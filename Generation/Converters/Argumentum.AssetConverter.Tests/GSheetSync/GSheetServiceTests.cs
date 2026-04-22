using System.Linq;
using Argumentum.AssetConverter.GSheetSync;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
    public class GSheetServiceTests
    {
        [Fact]
        public void CsvToGrid_PreservesHeaderRow()
        {
            // Google Sheets expects the header row to be part of the uploaded grid:
            // the first sheet row is the column headers, not a data row.
            var csv = "col1,col2,col3\nA1,B1,C1\nA2,B2,C2\n";

            var grid = GSheetService.CsvToGrid(csv);

            grid.Should().HaveCount(3, "header + 2 data rows must all be uploaded");
            grid[0].Select(c => c.ToString()).Should().Equal("col1", "col2", "col3");
            grid[1].Select(c => c.ToString()).Should().Equal("A1", "B1", "C1");
            grid[2].Select(c => c.ToString()).Should().Equal("A2", "B2", "C2");
        }

        [Fact]
        public void CsvToGrid_HandlesQuotedFieldsWithCommas()
        {
            var csv = "pk,desc\n1,\"Hello, world\"\n2,\"a,b,c\"\n";

            var grid = GSheetService.CsvToGrid(csv);

            grid.Should().HaveCount(3);
            grid[1][1].ToString().Should().Be("Hello, world");
            grid[2][1].ToString().Should().Be("a,b,c");
        }

        [Fact]
        public void CsvToGrid_EmptyCells_PreservedAsEmptyStrings()
        {
            var csv = "a,b,c\n1,,3\n,2,\n";

            var grid = GSheetService.CsvToGrid(csv);

            grid.Should().HaveCount(3);
            grid[1].Select(c => c.ToString()).Should().Equal("1", "", "3");
            grid[2].Select(c => c.ToString()).Should().Equal("", "2", "");
        }

        [Fact]
        public void CsvToGrid_RoundTrip_ViaGridToCsv()
        {
            // Full symmetry: download (grid) → GridToCsv → CsvToGrid must yield same grid.
            // This is the critical invariant for the download/upload sync path.
            var originalCsv = "pk,name,value\n1,Foo,100\n2,Bar,200\n3,Baz,300\n";

            var grid1 = GSheetService.CsvToGrid(originalCsv);

            var service = new GSheetService(null!);
            var csvAgain = service.GridToCsv(grid1);
            var grid2 = GSheetService.CsvToGrid(csvAgain);

            grid1.Should().HaveCount(grid2.Count);
            for (int i = 0; i < grid1.Count; i++)
            {
                grid1[i].Select(c => c.ToString())
                    .Should().Equal(grid2[i].Select(c => c.ToString()));
            }
        }
    }
}
