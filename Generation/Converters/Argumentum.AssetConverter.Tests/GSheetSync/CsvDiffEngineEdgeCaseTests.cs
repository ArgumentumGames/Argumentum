using System.Text;
using Argumentum.AssetConverter.GSheetSync;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
    /// <summary>
    /// Edge-case regression tests for <see cref="CsvDiffEngine"/> — dispatch #204 (tertiaire).
    ///
    /// NEW additive file: <c>CsvDiffEngineTests.cs</c> already covers the happy path (identical /
    /// added / deleted / modified / sample-cap / column add-remove / missing-PK fallback / empty CSV
    /// / semicolon delimiter / CRLF-vs-LF normalization / modification % / long-value truncation).
    /// This file complements it on the edge cases called out in the dispatch that are genuinely
    /// untested today: empty-vs-absent cell, embedded newline inside a quoted cell, embedded comma
    /// and escaped quotes inside quoted cells, BOM-prefixed input, duplicate PK, empty-PK row, and
    /// whitespace-trim symmetry between the PK index and the cell normalizer.
    ///
    /// No existing file is modified. Baseline must stay green (additive).
    /// </summary>
    public class CsvDiffEngineEdgeCaseTests
    {
        private const string HeaderPk = "pk";

        // CsvDiffEngineTests.MakeCsv joins cells with "," — it cannot express quoted fields
        // (embedded commas / quotes / newlines), so the edge-case file builds raw CSV strings.
        private static string Csv(params string[] lines) => string.Join("\n", lines) + "\n";

        [Fact]
        public void Empty_Cell_Middle_Of_Row_Is_Detected_As_Modification()
        {
            // "present-but-empty cell" vs "non-empty cell" must count as a modification.
            // This is distinct from row-deletion: the row still exists (same PK), one cell changed
            // from a value to "". NormalizeValue("") == "" so the diff fires.
            var oldCsv = Csv("pk,Name,Value", "1,Foo,100");
            var newCsv = Csv("pk,Name,Value", "1,Foo,");

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            result.RowsModified.Should().Be(1, "clearing a cell is a modification, not a row change");
            result.CellsModified.Should().Be(1);
            result.SampleOverwrites.Should().ContainSingle()
                .Which.NewValue.Should().Be("");
        }

        [Fact]
        public void Present_Empty_Cell_Vs_Absent_Column_Are_Not_Conflated()
        {
            // A row present in both sides, with an empty cell, must be Modified (not Unchanged,
            // not Deleted). The contract: "" != previous-value. Pinning this guards against a
            // future "treat empty as unchanged" regression that would silently swallow real clears.
            var oldCsv = Csv("pk,Name", "1,Foo", "2,Bar");
            var newCsv = Csv("pk,Name", "1,Foo", "2,");

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            result.RowsUnchanged.Should().Be(1, "row 1 untouched");
            result.RowsModified.Should().Be(1, "row 2 cell cleared");
            result.RowsDeleted.Should().Be(0);
        }

        [Fact]
        public void Embedded_Newline_Inside_Quoted_Cell_Is_Preserved_For_Diff()
        {
            // RFC 4180: a newline inside a quoted field is data, not a record separator.
            // CsvDiffEngineTests already proves CRLF-vs-LF is normalized to "no diff" — this test
            // proves the orthogonal case: a genuine content newline (present in new, absent in old)
            // IS detected as a modification. Guards the parser distinguishing field-newline from
            // record-newline (the #216-class silent-contamination cousin: a parse regression here
            // would merge rows and silently hide edits).
            var oldCsv = Csv("pk,Body", "1,\"single line\"");
            var newCsv = Csv("pk,Body", "1,\"line one\nline two\"");

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            result.CellsModified.Should().Be(1, "the embedded newline changed the cell value");
            result.RowsModified.Should().Be(1);
            result.TotalRowsNew.Should().Be(1, "the embedded newline must NOT be parsed as a new row");
            result.SampleOverwrites[0].NewValue.Should().Contain("\n");
        }

        [Fact]
        public void Embedded_Comma_Inside_Quoted_Cell_Does_Not_Split_The_Field()
        {
            // The shared MakeCsv helper cannot build this case (it joins raw cells). A comma inside
            // quotes must stay inside the single field — a regression to naive Split(',') would
            // silently shift every column to the right and corrupt the diff.
            var oldCsv = Csv("pk,Name", "1,\"Foo, Bar\"");
            var newCsv = Csv("pk,Name", "1,\"Foo, Baz\"");

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            result.CellsModified.Should().Be(1);
            result.SampleOverwrites[0].OldValue.Should().Be("Foo, Bar");
            result.SampleOverwrites[0].NewValue.Should().Be("Foo, Baz");
        }

        [Fact]
        public void Escaped_Double_Quote_Inside_Quoted_Cell_Is_Unescaped()
        {
            // RFC 4180: "" inside a quoted field is a literal ". Verify the parsed value carries
            // the literal quote (so a diff on quote content is detected), and that the engine never
            // sees a phantom trailing quote.
            var oldCsv = Csv("pk,Quote", "1,\"say \"\"hi\"\"\"");
            var newCsv = Csv("pk,Quote", "1,\"say \"\"bye\"\"\"");

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            result.CellsModified.Should().Be(1);
            result.SampleOverwrites[0].OldValue.Should().Be("say \"hi\"");
            result.SampleOverwrites[0].NewValue.Should().Be("say \"bye\"");
        }

        [Fact]
        public void BOM_Prefixed_Header_Does_Not_Break_Primary_Key_Lookup()
        {
            // A UTF-8 BOM (﻿) at the very start of the CSV sticks to the first header name.
            // If the parser left it there, the PK column would be "﻿pk" and IndexRows would
            // fall back to row-position indexing — a silent wrong-diff. This pins that the PK
            // still resolves (RowsUnchanged == 1, i.e. matched by value, not position) on BOM input.
            var oldCsv = "﻿pk,Name\n1,Alpha\n";
            var newCsv = "﻿pk,Name\n1,Alpha\n";

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            // Whether or not the BOM is stripped, identical CSVs must round-trip to no-diff.
            // The meaningful invariant: a value change is detected (proves PK indexing is alive,
            // not position fallback). Assert that here via a second comparison.
            result.RowsUnchanged.Should().Be(1, "identical rows round-trip with no diff");

            var changedCsv = "﻿pk,Name\n1,Alpha-X\n";
            var changed = engine.Compare(oldCsv, changedCsv);
            changed.RowsModified.Should().Be(1, "PK lookup must still resolve on BOM input (else position fallback would mask this as Added+Deleted)");
        }

        [Fact]
        public void Duplicate_Primary_Key_Keeps_First_Occurrence()
        {
            // IndexRows documents: on duplicate PK, only the first occurrence is kept. Pin this so
            // a regression to "last wins" or "throw" is caught. Two rows share pk "1"; the row kept
            // for diff is the first (Name=Alpha).
            var oldCsv = Csv("pk,Name", "1,Alpha", "1,Beta");
            var newCsv = Csv("pk,Name", "1,Alpha");

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            // First occurrence "Alpha" is kept on old side; new side "Alpha" matches → unchanged.
            result.RowsUnchanged.Should().Be(1);
            result.RowsDeleted.Should().Be(0, "duplicate 2nd row (Beta) was never indexed — not a tracked row");
            result.TotalRowsOld.Should().Be(1, "only one distinct PK indexed despite 2 physical rows");
        }

        [Fact]
        public void Empty_Primary_Key_Row_Is_Excluded_From_Diff()
        {
            // IndexRows documents: rows with an empty PK are excluded. Pin this so a regression to
            // "empty PK matched against another empty PK" (false merging) is caught.
            var oldCsv = Csv("pk,Name", ",Ghost", "1,Real");
            var newCsv = Csv("pk,Name", "1,Real");

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            result.TotalRowsOld.Should().Be(1, "the empty-PK 'Ghost' row is excluded, not indexed");
            result.RowsUnchanged.Should().Be(1);
            result.RowsDeleted.Should().Be(0);
        }

        [Fact]
        public void Primary_Key_Trim_Symmetry_Matches_Cell_Normalizer()
        {
            // IndexRows trims the PK (key.Trim()) and is OrdinalIgnoreCase. A PK with trailing
            // whitespace ("1 ") must still match a clean PK ("1") on the other side. This pins the
            // symmetry: if PK-trim were ever removed, row "1 " would become a separate key and the
            // diff would wrongly report Added+Deleted.
            var oldCsv = Csv("pk,Name", "1 ,Alpha");
            var newCsv = Csv("pk,Name", "1,Alpha");

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            result.RowsUnchanged.Should().Be(1, "PK trim must reconcile '1 ' with '1'");
            result.RowsAdded.Should().Be(0);
            result.RowsDeleted.Should().Be(0);
        }

        [Fact]
        public void Truncate_Appends_Ellipsis_And_Stays_Under_MaxLength()
        {
            // Truncate(value, 80): if length <= 80 return as-is; else Substring(0, 77) + "...".
            // Pin the exact shape so a future refactor (e.g. changing to 80 + "..." = 83 chars)
            // is caught. This is the SampleOverwrites contract callers rely on for display width.
            var oldCsv = Csv("pk,Name", "1,Short");
            var newCsv = Csv("pk,Name", "1," + new string('A', 200));

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            var sample = result.SampleOverwrites[0];
            sample.NewValue.Should().Be(new string('A', 77) + "...", "truncation shape = first 77 chars + ellipsis");
            sample.NewValue.Length.Should().Be(80);
        }

        [Fact]
        public void Primary_Key_Match_Is_Case_Insensitive()
        {
            // IndexRows uses StringComparer.OrdinalIgnoreCase. A PK differing only in case
            // ("PK1" vs "pk1") must match the same row. Pinning this guards a regression to
            // ordinal-sensitive indexing (which would wrongly split rows by case).
            var oldCsv = Csv("pk,Name", "PK1,Alpha");
            var newCsv = Csv("pk,Name", "pk1,Alpha-Modified");

            var engine = new CsvDiffEngine(HeaderPk);
            var result = engine.Compare(oldCsv, newCsv);

            result.RowsModified.Should().Be(1, "PK match is case-insensitive");
            result.RowsAdded.Should().Be(0);
            result.RowsDeleted.Should().Be(0);
        }
    }
}
