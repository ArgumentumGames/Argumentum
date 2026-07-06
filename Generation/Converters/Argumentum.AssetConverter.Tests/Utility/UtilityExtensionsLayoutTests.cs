using System;
using System.Collections.Generic;
using System.Linq;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Utility
{
    /// <summary>
    /// Regression tests for the layout-math and path utilities in <see cref="UtilityExtensions"/>.
    ///
    /// NEW additive file (dispatch `4rkh1s` tertiaire). CLAUDE.md flags two fragile areas these
    /// utilities underpin:
    ///  (1) PDF / mind-map grid layout math — <c>ToJaggedArray&lt;T&gt;</c> computes
    ///      <c>rowLength = ceil(count / columnLength)</c> and builds a row-major jagged grid
    ///      (trailing short row). The Print&amp;Play / mind-map column wrapping depends on this exact
    ///      ceiling-and-partial-row shape. A regression to a naive integer divide, or to dropping the
    ///      trailing short row, silently corrupts card grids.
    ///  (2) Relative-vs-absolute asset path resolution — <c>PathIsUrl</c> gates the rewrite of
    ///      <c>../../Cards/...</c> relative paths to absolute GitHub URLs (CLAUDE.md "Images
    ///      blanches/vides → chemins assets relatifs"). A regression here ships white/empty cards.
    ///
    /// These methods are public static extensions, deterministic, key-free, release-independent —
    /// pure-function regression targets. No existing file modified. Baseline additive.
    /// </summary>
    public class UtilityExtensionsLayoutTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // ToJaggedArray<T> — grid layout math (ceil rows + trailing short row).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void ToJaggedArray_Exact_Division_Builds_Full_Rows()
        {
            var source = new List<int> { 1, 2, 3, 4, 5, 6 };

            var grid = source.ToJaggedArray(columnLength: 3);

            grid.Should().HaveCount(2, "6 items / 3 columns = 2 full rows");
            grid[0].Should().Equal(new[] { 1, 2, 3 });
            grid[1].Should().Equal(new[] { 4, 5, 6 });
        }

        [Fact]
        public void ToJaggedArray_Trailing_Short_Row_Has_Only_Remaining_Items()
        {
            // 7 items / 3 columns = ceil(7/3) = 3 rows: [3, 3, 1]. The last row is SHORT (1 item).
            // A regression that pads the last row (e.g. default(T) fillers) would corrupt card grids
            // by injecting phantom blank cards into the final row.
            var source = Enumerable.Range(1, 7).ToList();

            var grid = source.ToJaggedArray(columnLength: 3);

            grid.Should().HaveCount(3, "ceil(7/3) = 3 rows");
            grid[0].Should().Equal(new[] { 1, 2, 3 });
            grid[1].Should().Equal(new[] { 4, 5, 6 });
            grid[2].Should().Equal(new[] { 7 }, "trailing row carries ONLY the remainder, no padding");
        }

        [Theory]
        [InlineData(1, 3, 1)]   // fewer items than columns → 1 short row
        [InlineData(3, 3, 1)]   // exactly one row
        [InlineData(4, 3, 2)]   // one full + one short
        [InlineData(6, 3, 2)]   // two full rows
        [InlineData(7, 3, 3)]   // two full + one short
        [InlineData(0, 3, 0)]   // empty source → zero rows (ceil(0/3) = 0)
        public void ToJaggedArray_Row_Count_Is_Ceiling_Of_Count_Over_Columns(int count, int columns, int expectedRows)
        {
            // Pins the rowLength = ceil(count / columns) contract that PDF/mind-map layout depends on.
            var source = Enumerable.Range(1, count).ToList();

            var grid = source.ToJaggedArray(columns);

            grid.Should().HaveCount(expectedRows);
        }

        [Fact]
        public void ToJaggedArray_Preserves_Row_Major_Order()
        {
            // The grid is filled row-by-row (global index = rowIndex * columnLength + colIndex).
            // A column-major regression would transpose the grid and scramble card positions.
            var source = Enumerable.Range(0, 12).ToList();

            var grid = source.ToJaggedArray(columnLength: 4);

            // Flatten back must equal the original order — proves row-major fill.
            grid.Flatten().Should().Equal(source, "row-major fill round-trips through Flatten");
            // And spot-check a known cell: index 5 = row 1, col 1.
            grid[1][1].Should().Be(5);
        }

        [Fact]
        public void ToJaggedArray_And_Flatten_Are_Inverses()
        {
            // Flatten is the documented inverse of ToJaggedArray. A round-trip must be lossless
            // for any column length >= 1. Guards both methods together.
            var original = Enumerable.Range(100, 23).ToList();

            foreach (var columns in new[] { 1, 2, 5, 7, 23, 100 })
            {
                var roundTrip = original.ToJaggedArray(columns).Flatten();
                roundTrip.Should().Equal(original, $"round-trip through ToJaggedArray({columns})+Flatten must be lossless");
            }
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // PathIsUrl — asset-path gate (relative paths → must be rewritten to absolute URLs).
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("http://example.com/x.png", true)]
        [InlineData("https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/x.png", true)]
        [InlineData("https://argumentum.myia.io", true)]
        [InlineData("http://argumentum.myia.io", true)]
        public void PathIsUrl_Recognizes_Http_And_Https_Schemes(string path, bool expected)
        {
            path.PathIsUrl().Should().Be(expected, "http/https absolute URLs are URLs");
        }

        [Theory]
        [InlineData("../../Cards/Fallacies/x.png", false)]   // the CLAUDE.md relative-path failure case
        [InlineData("../Cards/x.png", false)]
        [InlineData("Cards/Fallacies/x.png", false)]
        [InlineData("C:\\Cards\\Fallacies\\x.png", false)]   // Windows absolute file path — NOT a URL
        [InlineData("/var/cards/x.png", false)]              // Unix absolute file path — NOT a URL
        [InlineData("", false)]
        [InlineData("   ", false)]                           // whitespace-only
        public void PathIsUrl_Rejects_Relative_File_And_Windows_Paths(string path, bool expected)
        {
            // The decisive guard: the relative asset paths CLAUDE.md documents as the root cause of
            // "white/empty cards" must NOT register as URLs (so the caller knows to rewrite them).
            path.PathIsUrl().Should().Be(expected, "relative/file paths are not URLs");
        }

        [Fact]
        public void PathIsUrl_Returns_False_For_Null()
        {
            // The implementation guards with IsNullOrWhiteSpace and returns false before parsing.
            // Pinned separately (null is not a valid Theory string argument) so the null path is
            // still covered — a regression that removed the null guard would NullReferenceException.
            ((string)null!).PathIsUrl().Should().BeFalse("null path must not be treated as a URL");
        }

        [Fact]
        public void PathIsUrl_Handles_Whitespace_Padded_Urls()
        {
            // The implementation trims before parsing. A URL with leading/trailing whitespace must
            // still be recognized — guards a regression where the trim is removed and padded URLs
            // silently register as non-URLs (shipping relative-path cards).
            "  https://example.com/x.png  ".PathIsUrl().Should().BeTrue("trimmed URL must resolve");
        }
    }
}
