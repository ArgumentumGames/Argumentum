using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for the CMYK Debug/Release resolver on <see cref="DocumentCardSet"/>.
    ///
    /// Since #1111 a standard run in EITHER build mode produces RGB PNGs: the per-image
    /// sRGB→CMYK→RGB round-trip is retired from the Release generation path (the PNG write
    /// destroys CMYK — #632 — so it only shifted pixels; the authoritative CMYK path is the
    /// Ghostscript post-process, <c>PdfCmykPostProcess</c> / <c>--pdf-cmyk</c>).
    /// <see cref="DocumentCardSet"/> still carries the <c>XxxDebug</c>/<c>XxxRelease</c> pair and
    /// the <see cref="DocumentCardSet.GetConvertToCmyk"/> resolver:
    /// <code>config.UseDebugParams ? ConvertToCmykDebug : ConvertToCmykRelease</code>, where
    /// <c>UseDebugParams</c> = <c>(isInDebugMode || ForceDebugParams) &amp;&amp; !ForceReleaseParams</c>,
    /// but both now default to false.
    ///
    /// These tests pin: (1) a fresh DocumentCardSet resolves to RGB (no per-image conversion) in
    /// both modes — a drifted default would silently reintroduce the retired pixel-shifting
    /// round-trip (the GO v0.9.0 bundle was produced WITH it; the next bundle is validated
    /// WITHOUT it); (2) the resolver stays a pure passthrough for explicit values.
    ///
    /// Deterministic across build modes: the <c>ForceDebugParams</c>/<c>ForceReleaseParams</c> flags
    /// drive <c>UseDebugParams</c> directly, so the assertions hold whether the test assembly is
    /// compiled Debug or Release (independent of the <c>#if DEBUG</c> <c>isInDebugMode</c> term).
    /// </summary>
    public class DocumentCardSetCmykContractTests
    {
        /// <summary>
        /// Config forced into Debug-params resolution: <c>ForceDebugParams</c> sets the first term
        /// of <c>UseDebugParams</c> true, so <c>UseDebugParams</c> is true regardless of the
        /// compile-time <c>#if DEBUG</c> flag.
        /// </summary>
        private static AssetConverterConfig ForcedDebug() => new AssetConverterConfig
        {
            ForceDebugParams = true,
            ForceReleaseParams = false
        };

        /// <summary>
        /// Config forced into Release-params resolution: <c>ForceReleaseParams</c> makes the
        /// <c>&amp;&amp; !ForceReleaseParams</c> term false, so <c>UseDebugParams</c> is false.
        /// This is the documented JSON override ("ForceReleaseParams = true to use Release params
        /// in Debug builds").
        /// </summary>
        private static AssetConverterConfig ForcedRelease() => new AssetConverterConfig
        {
            ForceReleaseParams = true
        };

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) DEFAULTS — since #1111 both modes resolve to RGB: a standard Release run no
        //     longer converts PNGs per-image (the round-trip shifted pixels — #632/#1111 —
        //     and CMYK for print comes from PdfCmykPostProcess). Pinning this catches a
        //     drifted default (e.g. ConvertToCmykRelease=true) that would silently
        //     reintroduce the pixel shift no visual verdict covers.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Defaults_DebugResolution_YieldsRgb_NoCmykConversion()
        {
            var cardSet = new DocumentCardSet();
            cardSet.ConvertToCmykDebug.Should().BeFalse(
                "Debug builds use RGB (preview-friendly, smaller files) — the documented default");

            cardSet.GetConvertToCmyk(ForcedDebug()).Should().BeFalse(
                "in Debug-params resolution the resolver returns ConvertToCmykDebug, which defaults " +
                "to false (RGB); a regression here would balloon Debug previews to CMYK size");
        }

        [Fact]
        public void Defaults_ReleaseResolution_YieldsRgb_NoPerImageConversion()
        {
            var cardSet = new DocumentCardSet();
            cardSet.ConvertToCmykRelease.Should().BeFalse(
                "#1111: the per-image sRGB→CMYK→RGB round-trip is retired from the Release " +
                "generation path — the PNG write destroys CMYK (#632), so it only shifted pixels");

            cardSet.GetConvertToCmyk(ForcedRelease()).Should().BeFalse(
                "a standard Release run must NOT convert PNGs per-image (#1111 DoD); CMYK for " +
                "print is applied by the authoritative Ghostscript post-process (--pdf-cmyk)");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) The resolver is a PURE PASSTHROUGH — it forwards the per-mode field verbatim,
        //     not a hardcoded color-space decision. Explicit (even non-default) values are
        //     respected per mode. Catches a regression that hardcodes the result instead of
        //     reading the pair.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Resolver_ForwardsCustomValues_PerMode()
        {
            var cardSet = new DocumentCardSet
            {
                ConvertToCmykDebug = true,   // non-default: Debug explicitly wants CMYK
                ConvertToCmykRelease = false // default since #1111
            };

            cardSet.GetConvertToCmyk(ForcedDebug()).Should().BeTrue(
                "the resolver must forward ConvertToCmykDebug verbatim in Debug mode, even when " +
                "custom-inverted — it is a passthrough, not a hardcoded RGB decision");
            cardSet.GetConvertToCmyk(ForcedRelease()).Should().BeFalse(
                "the resolver must forward ConvertToCmykRelease verbatim in Release mode, even when " +
                "custom-inverted — it is a passthrough, not a hardcoded CMYK decision");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) OVERRIDE PRIORITY — the documented "ForceReleaseParams = true to use Release params
        //     in Debug builds". With BOTH force flags set, Release wins because UseDebugParams is
        //     gated by `&amp;&amp; !ForceReleaseParams`. Uses an explicitly-set Release value
        //     (true) so the priority assertion is independent of the #1111 default flip.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void ForceReleaseParams_OverridesForceDebugParams_YieldsReleaseValue()
        {
            var cardSet = new DocumentCardSet
            {
                ConvertToCmykDebug = false,
                ConvertToCmykRelease = true // explicit non-default value; priority is what's pinned
            };
            var bothForced = new AssetConverterConfig
            {
                ForceDebugParams = true,
                ForceReleaseParams = true
            };

            // ForceReleaseParams dominates: UseDebugParams = (… || ForceDebugParams) && !ForceReleaseParams = false.
            cardSet.GetConvertToCmyk(bothForced).Should().BeTrue(
                "ForceReleaseParams must win over ForceDebugParams (UseDebugParams is gated by " +
                "'&& !ForceReleaseParams'), so a forced Release run yields the Release CMYK value " +
                "even in a Debug build — the documented override");
        }
    }
}
