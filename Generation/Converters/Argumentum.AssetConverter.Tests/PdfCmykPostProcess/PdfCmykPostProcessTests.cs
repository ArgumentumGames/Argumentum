using System.IO;
using System.Threading.Tasks;
using Argumentum.AssetConverter;
using Argumentum.AssetConverter.PdfCmykPostProcess;
using FluentAssertions;
using ImageMagick;
using Xunit;

namespace Argumentum.AssetConverter.Tests.PdfCmykPostProcess
{
    /// <summary>
    /// Contract pin for the Ghostscript CMYK + OutputIntent post-process (#632).
    ///
    /// These tests do NOT require Ghostscript to be installed: they verify the gating
    /// (Release-only by default, master toggle), the exact Ghostscript argument contract
    /// (ai-01 POC-validated — see issue #632), the PDFX_def.ps generation, and the ICC
    /// profile extraction from Magick.NET. The graceful-skip-when-GS-absent behavior is
    /// covered by <see cref="ResolveGhostscript_returns_null_when_binary_absent"/>.
    /// </summary>
    public class PdfCmykPostProcessTests
    {
        [Fact]
        public async Task Apply_IsEnabled_false_skips_without_requiring_ghostscript()
        {
            // A disabled stage must short-circuit before touching the filesystem or GS.
            var masterConfig = new AssetConverterConfig();
            var config = new PdfCmykPostProcessConfig { IsEnabled = false, GhostscriptPath = "definitely-not-real-gs" };

            Func<Task> act = () => PdfCmykPostProcessConfig.Apply(masterConfig, config);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public void GetEnabled_is_release_only_by_default()
        {
            var config = new PdfCmykPostProcessConfig();
            var debugConfig = new AssetConverterConfig(); // isInDebugMode=true under Debug tests
            var releaseConfig = new AssetConverterConfig { ForceReleaseParams = true };

            // Default pair: EnabledDebug=false, EnabledRelease=true → OFF in Debug, ON in Release.
            config.GetEnabled(debugConfig).Should().BeFalse("Debug is preview-only by default");
            config.GetEnabled(releaseConfig).Should().BeTrue("Release is printer-quality by default");
        }

        [Fact]
        public void GetEnabled_respects_master_toggle_and_explicit_overrides()
        {
            var releaseConfig = new AssetConverterConfig { ForceReleaseParams = true };

            var offEverywhere = new PdfCmykPostProcessConfig { EnabledRelease = false };
            offEverywhere.GetEnabled(releaseConfig).Should().BeFalse("EnabledRelease=false overrides build mode");

            var onInDebug = new PdfCmykPostProcessConfig { EnabledDebug = true };
            var debugConfig = new AssetConverterConfig();
            onInDebug.GetEnabled(debugConfig).Should().BeTrue("EnabledDebug=true enables it in Debug");
        }

        [Fact]
        public void ResolveGhostscript_returns_null_when_binary_absent()
        {
            // A bogus name not on PATH and not absolute-existing must resolve to null, not throw.
            var processor = new PdfCmykPostProcessor(
                new PdfCmykPostProcessConfig { GhostscriptPath = "definitely-not-a-real-gs-binary-xyz123" });

            var resolved = processor.ResolveGhostscript();

            resolved.Should().BeNull("an absent GS binary must skip the stage gracefully (#632 spec)");
        }

        [Fact]
        public void BuildGhostscriptArguments_matches_poc_validated_command()
        {
            var args = PdfCmykPostProcessor.BuildGhostscriptArguments(
                iccPath: @"C:\icc\USWebCoatedSWOP.icc",
                outputPdf: @"C:\out\doc-cmyk.pdf",
                pdfxDefPath: @"C:\tmp\PDFX_def.ps",
                inputPdf: @"C:\docs\doc.pdf");

            // POC-validated invariants (issue #632): PDFX, CMYK strategy+model, Flate lossless,
            // no downsample, ICC permit-read, then -o, PDFX_def.ps, input in order.
            args.Should().Contain("-dPDFX");
            args.Should().Contain("-sColorConversionStrategy=CMYK");
            args.Should().Contain("-dProcessColorModel=/DeviceCMYK");
            args.Should().Contain("-dColorImageFilter=/FlateEncode");
            args.Should().Contain("-dGrayImageFilter=/FlateEncode");
            args.Should().Contain("-dDownsampleColorImages=false");
            args.Should().Contain("--permit-file-read=\"C:\\icc\\USWebCoatedSWOP.icc\"");
            args.Should().Contain("-o \"C:\\out\\doc-cmyk.pdf\"");
            args.Should().Contain("\"C:\\tmp\\PDFX_def.ps\"");
            args.Should().Contain("\"C:\\docs\\doc.pdf\"");
        }

        [Fact]
        public void BuildPdfxDefPostscript_embeds_icc_path_and_output_intent_markers()
        {
            var ps = PdfCmykPostProcessor.BuildPdfxDefPostscript(@"C:\profiles\SWOP.icc");

            // ICC path injected (forward-slash normalized for Postscript).
            ps.Should().Contain("/ICCProfile (C:/profiles/SWOP.icc) def");
            // PDF/X OutputIntent contract: N=4 CMYK, GTS_PDFX type, SWOP registry identifiers.
            ps.Should().Contain("<</N 4>>");
            ps.Should().Contain("/S /GTS_PDFX");
            ps.Should().Contain("/Type /OutputIntent");
            ps.Should().Contain("(CGATS TR 001)");
            ps.Should().Contain("(U.S. Web Coated \\(SWOP\\) v2)");
            ps.Should().Contain("/RegistryName (http://www.color.org)");
            // Catalog wiring.
            ps.Should().Contain("/OutputIntents");
        }

        [Fact]
        public void Magick_icc_profile_extracts_nonempty_swop_bytes()
        {
            // The ICC profile used both by the per-image ConvertToCmyk conversion AND by this
            // GS OutputIntent — verifies the extraction API works and yields the ~557 KB SWOP profile.
            var bytes = ColorProfiles.USWebCoatedSWOP.ToByteArray();

            bytes.Should().NotBeNull();
            bytes.Length.Should().BeGreaterThan(500_000, "USWebCoatedSWOP ICC is ~557 KB per ai-01 POC (#632)");
        }
    }
}
