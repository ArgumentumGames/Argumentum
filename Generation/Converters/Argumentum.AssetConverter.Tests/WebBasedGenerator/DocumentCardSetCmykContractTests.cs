using Argumentum.AssetConverter;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.WebBasedGenerator
{
    /// <summary>
    /// Contract pin for the CMYK Debug/Release resolver on <see cref="DocumentCardSet"/>.
    ///
    /// The pipeline produces images with different color spaces per build mode (documented in the
    /// project CLAUDE.md "Debug vs Release Builds" table):
    /// <list type="table">
    /// <listheader><term>Mode</term><description>CMYK conversion</description></listheader>
    /// <item><term>Debug (<c>dotnet run</c>)</term><description>Disabled — RGB, preview-friendly, smaller files</description></item>
    /// <item><term>Release (<c>-c Release</c>)</term><description>Enabled — CMYK, printer quality</description></item>
    /// </list>
    /// <see cref="DocumentCardSet"/> carries a <c>XxxDebug</c>/<c>XxxRelease</c> property pair and a
    /// <see cref="DocumentCardSet.GetConvertToCmyk"/> resolver:
    /// <code>config.UseDebugParams ? ConvertToCmykDebug : ConvertToCmykRelease</code>, where
    /// <c>UseDebugParams</c> = <c>(isInDebugMode || ForceDebugParams) &amp;&amp; !ForceReleaseParams</c>.
    ///
    /// No test exercised this resolver before. A swapped ternary or drifted default
    /// would silently flip the color space per build mode — Debug previews would balloon to CMYK
    /// size, or Release print output would ship as RGB. These tests pin the contract additively.
    ///
    /// Deterministic across build modes: the <c>ForceDebugParams</c>/<c>ForceReleaseParams</c> flags
    /// drive <c>UseDebugParams</c> directly, so the assertions hold whether the test assembly is
    /// compiled Debug or Release (independent of the <c>#if DEBUG</c> <c>isInDebugMode</c> term).
    /// Additive only: no production code or existing test is modified. Dispatch #204 primaire.
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
        // (1) DEFAULTS — the documented Debug/Release color-space contract. A fresh
        //     DocumentCardSet resolves to RGB in Debug and CMYK in Release. This is the table
        //     in CLAUDE.md; pinning it catches a drifted default (e.g. ConvertToCmykDebug=true)
        //     that would silently ship CMYK-sized Debug previews.
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
        public void Defaults_ReleaseResolution_YieldsCmyk()
        {
            var cardSet = new DocumentCardSet();
            cardSet.ConvertToCmykRelease.Should().BeTrue(
                "Release builds enable CMYK (printer quality) — the documented default");

            cardSet.GetConvertToCmyk(ForcedRelease()).Should().BeTrue(
                "in Release-params resolution the resolver returns ConvertToCmykRelease, which " +
                "defaults to true (CMYK); a regression here would ship RGB print output");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) The resolver is a PURE PASSTHROUGH — it forwards the per-mode field verbatim,
        //     not a hardcoded color-space decision. Custom (even inverted) values are respected
        //     per mode. Catches a regression that hardcodes the result instead of reading the pair.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Resolver_ForwardsCustomInvertedValues_PerMode()
        {
            var cardSet = new DocumentCardSet
            {
                ConvertToCmykDebug = true,   // inverted: Debug wants CMYK
                ConvertToCmykRelease = false // inverted: Release wants RGB
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
        //     gated by `&amp;&amp; !ForceReleaseParams`. This pins the override priority so a
        //     forced Release run really yields the Release color space even in a Debug build.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void ForceReleaseParams_OverridesForceDebugParams_YieldsReleaseValue()
        {
            var cardSet = new DocumentCardSet
            {
                ConvertToCmykDebug = false,
                ConvertToCmykRelease = true
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
