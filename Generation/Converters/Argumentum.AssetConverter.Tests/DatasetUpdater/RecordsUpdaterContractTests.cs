using System.Collections.Generic;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.DatasetUpdater
{
    /// <summary>
    /// Contract pin for <see cref="RecordsUpdater"/> — the LLM-facing function-tool that the
    /// DatasetUpdater translation loop exposes (wired in <c>DatasetUpdaterConfig</c> as the
    /// <c>UpdateRecord</c> tool the model calls for EVERY target field of EVERY record).
    ///
    /// Had ZERO coverage (no reference to RecordsUpdater/UpdateRecord/DecodeValue anywhere in the
    /// test project; DatasetUpdater had no dedicated test folder). Two surfaces pinned here:
    ///
    /// (1) <see cref="RecordsUpdater.UpdateRecord"/> — the observable contract: PK/field lookup,
    /// the LLM-facing error strings ("target record not found...", "field '...' not found...
    /// Available: ..."), the <c>FilledOverwriteCount</c> safety counter (tracks how many updates
    /// OVERWROTE a non-empty cell — a corruption-risk signal), and the "old==&gt;new" confirmation
    /// format the model reads back. A refactor that changed any of these would silently change
    /// the model's feedback loop or the overwrite metric, with no exception.
    ///
    /// (2) <c>DecodeValue</c> (private, exercised via UpdateRecord) — a 3-pass decode applied to
    /// every value before storage: <c>Regex.Unescape</c> → <c>HtmlDecode</c> → <c>UrlDecode</c>,
    /// IN THAT ORDER. The order is a real silent-wrong-output hazard: e.g. <c>%26amp%3B</c>
    /// yields <c>&amp;apm;</c>-ish results that differ depending on whether UrlDecode runs before
    /// or after HtmlDecode. These tests pin the order additively so a reorder fails loud.
    ///
    /// All tests drive the real public <c>UpdateRecord</c> entry (no production code changed;
    /// <c>DecodeValue</c> stays private). Pure &amp; deterministic given controlled <c>Records</c>.
    /// </summary>
    public class RecordsUpdaterContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // Helpers — build a RecordsUpdater with a controlled, deterministic record set.
        // ─────────────────────────────────────────────────────────────────────────────

        private static RecordsUpdater MakeUpdater()
        {
            // Insertion order (pk, name) is preserved by Dictionary<string,object> iteration,
            // so the "Available: ..." error string is deterministic.
            return new RecordsUpdater
            {
                PrimaryKeyField = "pk",
                Records = new List<Dictionary<string, object>>
                {
                    new() { ["pk"] = "1", ["name"] = "" },        // empty target cell
                    new() { ["pk"] = "2", ["name"] = "existing" } // pre-filled target cell
                }
            };
        }

        private static string Stored(RecordsUpdater u, string pk, string field)
            => u.Records.Find(r => r["pk"].ToString() == pk)![field].ToString()!;

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) DecodeValue — the 3-pass decode contract, exercised via UpdateRecord.
        //     DecodeValue(s) = UrlDecode(HtmlDecode(Regex.Unescape(s))).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Decode_PlainValue_PassesThroughUnchanged()
        {
            var u = MakeUpdater();
            u.UpdateRecord("1", "name", "hello world");
            Stored(u, "1", "name").Should().Be("hello world");
        }

        [Fact]
        public void Decode_HtmlEntity_IsHtmlDecoded()
        {
            // &lt; → "<" (HtmlDecode pass).
            var u = MakeUpdater();
            u.UpdateRecord("1", "name", "&lt;");
            Stored(u, "1", "name").Should().Be("<");
        }

        [Fact]
        public void Decode_PercentEncoding_IsUrlDecoded()
        {
            // %3C → "<" (UrlDecode pass).
            var u = MakeUpdater();
            u.UpdateRecord("1", "name", "%3C");
            Stored(u, "1", "name").Should().Be("<");
        }

        [Fact]
        public void Decode_RegexBackslashEscape_IsRegexUnescaped()
        {
            // The 2-char literal backslash-n (what the LLM sends as JSON "\\n") → real newline
            // (Regex.Unescape pass).
            var u = MakeUpdater();
            u.UpdateRecord("1", "name", "\\n");
            Stored(u, "1", "name").Should().Be("\n");
        }

        [Fact]
        public void Decode_OrderDependence_UrlDecodeAfterHtmlDecode_Pinned()
        {
            // THE fragile case: %26amp%3B.
            //   Current order (Unescape → HtmlDecode → UrlDecode):
            //     Unescape("%26amp%3B")="%26amp%3B"  HtmlDecode("%26amp%3B")="%26amp%3B"
            //     UrlDecode("%26amp%3B")="&amp;"   → final "&amp;"
            //   Reversed (UrlDecode first):
            //     UrlDecode("%26amp%3B")="&amp;"   HtmlDecode("&amp;")="&"  → final "&"
            // So the stored value DIFFERS by order. We pin the CURRENT order = "&amp;". A refactor
            // that swapped the two decode passes would land "&" instead → silent wrong translation.
            var u = MakeUpdater();
            u.UpdateRecord("1", "name", "%26amp%3B");
            Stored(u, "1", "name").Should().Be("&amp;",
                "DecodeValue runs HtmlDecode BEFORE UrlDecode; reversing yields '&' — order is load-bearing");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) UpdateRecord — the observable contract (lookup, errors, counters, return format).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void UpdateRecord_EmptyTarget_SetsValue_NoOverwriteCount()
        {
            var u = MakeUpdater();
            var ret = u.UpdateRecord("1", "name", "newval");

            Stored(u, "1", "name").Should().Be("newval");
            u.FilledOverwriteCount.Should().Be(0, "the target cell was empty — this is a fill, not an overwrite");
            u.CallCount.Should().Be(1);
            // Return format: "{existing}\n==>\n{newValue}" — existing was "".
            ret.Should().Be("\n==>\nnewval");
        }

        [Fact]
        public void UpdateRecord_FilledTarget_SetsValue_IncrementsOverwriteCount()
        {
            var u = MakeUpdater();
            var ret = u.UpdateRecord("2", "name", "new2");

            Stored(u, "2", "name").Should().Be("new2");
            u.FilledOverwriteCount.Should().Be(1, "the target cell was non-empty — this overwrites, a corruption-risk signal");
            u.CallCount.Should().Be(1);
            ret.Should().Be("existing\n==>\nnew2");
        }

        [Fact]
        public void UpdateRecord_RecordNotFound_ReturnsErrorMessage_RecordsUnchanged()
        {
            var u = MakeUpdater();
            var ret = u.UpdateRecord("99", "name", "x");

            ret.Should().Be("target record not found for pk=99");
            // No record mutated, no field set on any record.
            Stored(u, "1", "name").Should().Be("");
            Stored(u, "2", "name").Should().Be("existing");
            u.FilledOverwriteCount.Should().Be(0);
        }

        [Fact]
        public void UpdateRecord_FieldNotFound_ReturnsErrorMessage_ListingAvailableFields()
        {
            var u = MakeUpdater();
            var ret = u.UpdateRecord("1", "nonexistent", "x");

            // The LLM-facing feedback lists the record's actual keys so the model can self-correct.
            ret.Should().Be("field 'nonexistent' not found in record 1. Available: pk, name");
            u.FilledOverwriteCount.Should().Be(0);
        }

        [Fact]
        public void UpdateRecord_DecodeAppliedToStoredValue()
        {
            // Cross-check: the value is decoded before storage (not stored raw).
            var u = MakeUpdater();
            u.UpdateRecord("1", "name", "%3C");
            Stored(u, "1", "name").Should().Be("<", "newValue is UrlDecoded before being stored");
        }

        [Fact]
        public void UpdateRecord_CallCount_IncrementsPerCall()
        {
            var u = MakeUpdater();
            u.UpdateRecord("1", "name", "a");
            u.UpdateRecord("2", "name", "b");
            u.UpdateRecord("1", "name", "c");

            u.CallCount.Should().Be(3);
        }

        [Fact]
        public void UpdateRecord_OverwriteCount_AccumulatesAcrossOverwrites()
        {
            var u = MakeUpdater();
            u.UpdateRecord("2", "name", "first");   // overwrite (existing="existing") → +1
            u.UpdateRecord("2", "name", "second");  // overwrite (now "first") → +1
            u.UpdateRecord("1", "name", "fill");    // fill (empty) → +0

            u.FilledOverwriteCount.Should().Be(2);
            Stored(u, "2", "name").Should().Be("second");
        }

        [Fact]
        public void UpdateRecord_Determinism_SameInputsSameResult()
        {
            var u1 = MakeUpdater();
            var u2 = MakeUpdater();

            u1.UpdateRecord("1", "name", "%26amp%3B");
            u2.UpdateRecord("1", "name", "%26amp%3B");

            Stored(u1, "1", "name").Should().Be(Stored(u2, "1", "name"));
            u1.FilledOverwriteCount.Should().Be(u2.FilledOverwriteCount);
        }
    }
}
