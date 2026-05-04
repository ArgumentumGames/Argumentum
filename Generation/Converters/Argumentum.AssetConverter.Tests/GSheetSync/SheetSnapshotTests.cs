using System.Collections.Generic;
using Argumentum.AssetConverter.GSheetSync;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
    public class SheetSnapshotTests
    {
        [Fact]
        public void BuildProtectedCells_DetectsFormulaCells()
        {
            var formulas = new List<IList<object>>
            {
                new List<object> { "pk", "title", "path_padded" },
                new List<object> { "1", "Foo", "=TEXT(A2,\"00\")" },
                new List<object> { "2", "Bar", "=TEXT(A3,\"00\")" },
            };

            var protectedCells = SheetSnapshot.BuildProtectedCells(formulas);

            protectedCells.Should().BeEquivalentTo(new[] { (1, 2), (2, 2) });
        }

        [Fact]
        public void BuildProtectedCells_IgnoresPlainValues()
        {
            var formulas = new List<IList<object>>
            {
                new List<object> { "pk", "title" },
                new List<object> { "1", "Foo" },
                new List<object> { "2", "Bar" },
            };

            var protectedCells = SheetSnapshot.BuildProtectedCells(formulas);

            protectedCells.Should().BeEmpty();
        }

        [Fact]
        public void BuildProtectedCells_HandlesNullsAndRaggedRows()
        {
            // Sheets API returns ragged rows (trailing empty cells omitted).
            // BuildProtectedCells must tolerate that without throwing.
            var formulas = new List<IList<object>>
            {
                new List<object> { "pk", "title", "extra" },
                new List<object> { "1", "Foo" },
                null!,
                new List<object> { "2", null!, "=A4*2" },
            };

            var protectedCells = SheetSnapshot.BuildProtectedCells(formulas);

            protectedCells.Should().BeEquivalentTo(new[] { (3, 2) });
        }

        [Fact]
        public void BuildProtectedCells_EmptyOrNullInput_ReturnsEmptySet()
        {
            SheetSnapshot.BuildProtectedCells(null).Should().BeEmpty();
            SheetSnapshot.BuildProtectedCells(new List<IList<object>>()).Should().BeEmpty();
        }

        [Fact]
        public void BuildProtectedCells_LiteralStartingWithEqualsInQuotes_NotProtected()
        {
            // A literal string "=" prefixed by a quote in source CSV would be
            // surfaced by FORMULA render option as "'=foo" (leading apostrophe).
            // Sheets does NOT consider this a formula. The BuildProtectedCells
            // contract is "starts with =" because the FORMULA option already
            // strips quote-prefixes; we must not over-protect literal strings
            // that happen to contain "=" mid-cell.
            var formulas = new List<IList<object>>
            {
                new List<object> { "1", "a=b", "==", "= literal " },
            };

            var protectedCells = SheetSnapshot.BuildProtectedCells(formulas);

            // (0,2) is "==" which Sheets treats as a formula attempt — protected.
            // (0,3) is "= literal " — also a formula attempt syntactically.
            // (0,1) "a=b" is plain text — not protected.
            protectedCells.Should().BeEquivalentTo(new[] { (0, 2), (0, 3) });
        }

        [Theory]
        [InlineData(0, 0, "A1")]
        [InlineData(0, 25, "Z1")]
        [InlineData(0, 26, "AA1")]
        [InlineData(1, 26, "AA2")]
        [InlineData(41, 2, "C42")]
        [InlineData(0, 51, "AZ1")]
        [InlineData(0, 52, "BA1")]
        [InlineData(99, 701, "ZZ100")]
        [InlineData(0, 702, "AAA1")]
        public void ToA1Notation_ConvertsCorrectly(int row, int col, string expected)
        {
            SheetSnapshot.ToA1Notation(row, col).Should().Be(expected);
        }
    }
}
