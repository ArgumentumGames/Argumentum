using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for <see cref="ImageFileGenerator.NormalizeBackKey"/> — #204 secondary
    /// (cont. po-2024), the Golden Master (commit 0087f0ec) back-name normalization contract.
    ///
    /// When backs are harvested, each back's key is lower-cased and then — if it contains a hyphen —
    /// stripped to the suffix AFTER the last hyphen, KEEPING that hyphen as a leading prefix. So a
    /// harvested back key "scenarii-01-histoire" becomes the dict key "-histoire". This normalized key
    /// is what <see cref="ImageFileGenerator.ResolveCardBack"/> later substring-matches against face
    /// keys (a face must contain "-histoire", not the bare "histoire").
    ///
    /// This is the OTHER half of the face→back contract pinned in
    /// <see cref="FaceToBackMatchingContractTests"/> (ResolveCardBack): that contract chooses which
    /// back each face gets; THIS contract produces the keys that matching runs against. A regression
    /// here (e.g. stripping at the FIRST hyphen, or dropping the leading hyphen) silently realigns
    /// every back key so that ResolveCardBack falls through to the first-back fallback for every
    /// multi-back card set — wrong back behind every face, with correct count/geometry/ordering.
    ///
    /// Extracted output-neutral from <c>GenerateBacks</c>: the image is still loaded with the full
    /// lower-cased name; only the dictionary key is normalized.
    /// </summary>
    public class BackNameNormalizationContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // (1) The Golden Master strip rule — keep the suffix after the LAST hyphen.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void StripsToSuffixAfterLastHyphen_KeepingTheHyphen()
        {
            // "scenarii-01-histoire" → "-histoire" (everything before the LAST hyphen is dropped,
            // the hyphen itself is kept as a prefix — that prefix is part of the matching contract).
            ImageFileGenerator.NormalizeBackKey("scenarii-01-histoire")
                .Should().Be("-histoire");
        }

        [Fact]
        public void MultipleHyphens_StripsOnlyBeforeLast()
        {
            // Only the segment before the LAST hyphen is dropped — inner hyphens in the kept suffix
            // survive: "a-b-c" → "-c".
            ImageFileGenerator.NormalizeBackKey("a-b-c").Should().Be("-c");
        }

        [Fact]
        public void SingleHyphen_StripsPrefix()
        {
            // "scenarii-histoire" → "-histoire".
            ImageFileGenerator.NormalizeBackKey("scenarii-histoire").Should().Be("-histoire");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) No hyphen → unchanged (the back is keyed by its whole lower-cased name).
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void NoHyphen_ReturnedUnchanged()
        {
            // A back key with no hyphen is indexed as-is (e.g. a single-word back like "memo").
            ImageFileGenerator.NormalizeBackKey("memo").Should().Be("memo");
        }

        [Fact]
        public void NoHyphen_PreservesAlreadyNormalizedKey()
        {
            // A key that already starts with a hyphen (re-normalization, or a single-char-style key)
            // contains a hyphen → stripped at the last hyphen → returns itself from that hyphen.
            ImageFileGenerator.NormalizeBackKey("-histoire").Should().Be("-histoire");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) Edge cases.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void HyphenAtStart_ReturnedUnchanged()
        {
            // "-histoire" → LastIndexOf('-') == 0 → Substring(0) returns the whole string.
            ImageFileGenerator.NormalizeBackKey("-histoire").Should().Be("-histoire");
        }

        [Fact]
        public void TrailingHyphen_StripsToEmptyBeforeIt_KeepsHyphen()
        {
            // "histoire-" → last hyphen is the trailing one → Substring returns "-" (the kept suffix
            // is just the hyphen). This is a degenerate input but the rule is deterministic.
            ImageFileGenerator.NormalizeBackKey("histoire-").Should().Be("-");
        }
    }
}
