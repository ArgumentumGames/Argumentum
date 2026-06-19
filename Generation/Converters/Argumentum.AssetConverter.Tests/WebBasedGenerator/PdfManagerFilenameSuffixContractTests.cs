using System;
using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="PdfManager.InsertSuffixBeforeExtension"/> — #204 secondary
    /// (cont. po-2024): the per-back / FacesOnly PDF filename-suffix contract.
    ///
    /// <see cref="PdfManager.GenerateBackFirstOneDocPerBack"/> emits one PDF per distinct back art, plus
    /// an extra <c>-FacesOnly</c> PDF for back-less cards. Each output filename takes the deck's base
    /// name and inserts a suffix just before its FINAL dot: <c>Cards.pdf</c> + <c>"-1"</c> →
    /// <c>Cards-1.pdf</c>; <c>Cards.pdf</c> + <c>"-FacesOnly"</c> → <c>Cards-FacesOnly.pdf</c>. The suffix
    /// carries its own leading separator.
    ///
    /// This was previously inlined three times (the <c>LastIndexOf('.')</c> split computed once, then
    /// two <c>Substring</c> interpolations) in a method already flagged by a <c>BUGFIX CORRIGÉ</c>
    /// comment. Extracted output-neutral into <see cref="PdfManager.InsertSuffixBeforeExtension"/> so the
    /// naming contract is unit-testable in isolation. The extraction preserves the original
    /// <c>Substring(0, LastIndexOf('.'))</c> / <c>Substring(LastIndexOf('.'))</c> split EXACTLY —
    /// including its behavior on a dotless name (<c>LastIndexOf</c> returns <c>-1</c>, and
    /// <c>Substring(0, -1)</c> throws <see cref="ArgumentOutOfRangeException"/>). That throw is the
    /// existing contract, not a bug to fix silently: call sites always pass an extension-bearing base
    /// name, and a future caller passing a dotless name should fail loud exactly as before.
    ///
    /// A regression here (inserting at the FIRST dot, dropping the extension, swapping the
    /// counter to 0-based, or naively switching to <c>Path.GetFileNameWithoutExtension</c> — which
    /// would change behavior on dotless names) silently produces wrongly-named PDFs that overwrite
    /// each other or land in the wrong slot, caught only by inspecting the output directory.
    /// </summary>
    public class PdfManagerFilenameSuffixContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // (1) THE HEADLINE — a standard extension-bearing base name gets the suffix inserted
        //     before the FINAL dot. The extension is preserved verbatim. This is the per-back
        //     counter case (suffix "-1") and the FacesOnly case (suffix "-FacesOnly").
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void StandardExtension_SuffixBeforeFinalDot_ExtensionPreserved()
        {
            // "Cards.pdf" + "-1" → "Cards-1.pdf". The ".pdf" stays attached; only the stem is extended.
            PdfManager.InsertSuffixBeforeExtension("Cards.pdf", "-1")
                .Should().Be("Cards-1.pdf",
                    "the suffix is inserted before the FINAL dot and the extension is preserved — " +
                    "the per-back PDF #1 for the 'Cards' deck.");
        }

        [Fact]
        public void FacesOnlySuffix_SameSplit()
        {
            // The FacesOnly variant uses the same split, just a different (fixed) suffix string.
            PdfManager.InsertSuffixBeforeExtension("Cards.pdf", "-FacesOnly")
                .Should().Be("Cards-FacesOnly.pdf",
                    "the FacesOnly PDF uses the identical stem/extension split as the per-back PDFs.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) Per-back COUNTER — the suffix embeds the 1-based back index. The caller formats the
        //     counter as $"-{backIndex + 1}" (the loop variable is 0-based; the filename is 1-based).
        //     The method itself is counter-agnostic — it inserts whatever suffix string it receives —
        //     so these cases pin the END-TO-END caller contract: backIndex 0→suffix "-1", 1→"-2", etc.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData(0, "TarotCards-1.pdf")]   // first back  (0-based loop var 0 → filename "-1")
        [InlineData(1, "TarotCards-2.pdf")]   // second back (0-based loop var 1 → filename "-2")
        [InlineData(9, "TarotCards-10.pdf")]  // tenth back  (0-based loop var 9 → filename "-10")
        public void PerBackCounter_OneBased_LoopVarPlusOne(int loopVar, string expected)
        {
            // The caller passes $"-{loopVar + 1}" — the 0-based loop variable shifted to a 1-based
            // filename. A caller off-by-one (passing $"-{loopVar}") would produce "TarotCards-0.pdf"
            // for the first back, which this assertion rejects.
            PdfManager.InsertSuffixBeforeExtension("TarotCards.pdf", $"-{loopVar + 1}")
                .Should().Be(expected,
                    $"the per-back suffix is the 0-based loop variable + 1: loop var {loopVar} → " +
                    $"filename \"{loopVar + 1}\".");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) FINAL dot, not the first — a base name with multiple dots inserts before the LAST one,
        //     so the earlier dots stay in the stem. "Cards.v2.pdf" + "-1" → "Cards.v2-1.pdf" (the
        //     ".pdf" is the extension; "v2" stays in the stem). A regression that split at the FIRST
        //     dot would yield "Cards-1.v2.pdf" — wrong extension, wrong stem.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void MultipleDots_SplitsAtFinalDot_KeepsInnerDotsInStem()
        {
            PdfManager.InsertSuffixBeforeExtension("Cards.v2.pdf", "-1")
                .Should().Be("Cards.v2-1.pdf",
                    "the split is at the LAST dot, so inner dots stay in the stem and only the true " +
                    "extension '.pdf' is preserved. A first-dot split would wrongly yield 'Cards-1.v2.pdf'.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) Two-dot / dotless-extension variants — a name ending in a dot, or with a dot only as
        //     the extension separator with an empty stem. Edge cases of the LastIndexOf('.') split.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void DotOnlyAsExtension_EmptyStem_SuffixBeforeDot()
        {
            // ".pdf" (stem empty, LastIndexOf('.') == 0) + "-1" → "-1.pdf". Degenerate but deterministic.
            PdfManager.InsertSuffixBeforeExtension(".pdf", "-1")
                .Should().Be("-1.pdf",
                    "when the stem is empty (the dot is at index 0), the suffix is inserted before it " +
                    "and the extension '.pdf' is still preserved.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) DOTLESS NAME FAILS LOUD — the existing contract. LastIndexOf('.') returns -1, and
        //     Substring(0, -1) throws ArgumentOutOfRangeException. This is NOT a bug to fix silently:
        //     call sites always pass an extension-bearing base name, and switching to
        //     Path.GetFileNameWithoutExtension would SILENTLY change behavior (producing "Cards-1"
        //     instead of throwing). Pinning the throw keeps the contract fail-loud for a future caller
        //     that mistakenly passes a dotless name.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void DotlessName_ThrowsArgumentOutOfRange_FailsLoud()
        {
            // "Cards" (no dot) → LastIndexOf('.') == -1 → Substring(0, -1) throws. This is the existing
            // behavior, preserved output-neutral. A naive "fix" using Path.GetFileNameWithoutExtension
            // would return "Cards-1" (no extension) instead — a silent behavior change this test rejects.
            Action act = () => PdfManager.InsertSuffixBeforeExtension("Cards", "-1");

            act.Should().Throw<ArgumentOutOfRangeException>(
                "a dotless base name has no extension split point — LastIndexOf('.') returns -1 and " +
                "Substring(0, -1) throws. This is the existing fail-loud contract: call sites always " +
                "pass an extension-bearing name, and the method must NOT silently coerce a dotless name " +
                "(e.g. via Path.GetFileNameWithoutExtension), which would change behavior.");
        }
    }
}
