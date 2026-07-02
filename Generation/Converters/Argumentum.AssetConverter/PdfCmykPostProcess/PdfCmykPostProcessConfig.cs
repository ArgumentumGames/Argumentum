using System.Threading.Tasks;

namespace Argumentum.AssetConverter.PdfCmykPostProcess
{
    /// <summary>
    /// Configuration for the Ghostscript CMYK + OutputIntent post-process (#632).
    ///
    /// Converts the RGB-300-lossless (FlateDecode) PDFs produced by QuestPDF into
    /// DeviceCMYK + OutputIntent (CGATS TR 001 / SWOP) print-ready PDFs via a
    /// Ghostscript post-pass. Runs as a standalone stage AFTER PDF generation
    /// (dispatched after Task.WhenAll in AssetConverterConfig.Apply), so it can be
    /// run on an existing bundle WITHOUT re-harvesting.
    ///
    /// Scope honesty (#632 spec): the output targets CMYK colorspace + an
    /// OutputIntent profile, NOT formal PDF/X-3 certification (no trim/bleed boxes).
    /// </summary>
    public class PdfCmykPostProcessConfig
    {
        /// <summary>Master toggle. When false the stage is skipped entirely.</summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>Enable in Debug builds (default OFF — Debug is preview-only).</summary>
        public bool EnabledDebug { get; set; } = false;

        /// <summary>Enable in Release builds (default ON — printer quality).</summary>
        public bool EnabledRelease { get; set; } = true;

        /// <summary>Resolved enable flag: Release-only by default (see <see cref="AssetConverterConfig.UseReleaseParams"/>).</summary>
        public bool GetEnabled(AssetConverterConfig config)
            => config.UseReleaseParams ? EnabledRelease : EnabledDebug;

        /// <summary>
        /// Ghostscript executable name/path. Defaults to "gswin64c" (Windows GS console,
        /// resolves via PATH). Override with an absolute path (e.g. a conda-forge user-scope
        /// install) when GS is not on PATH. If the binary cannot be found, the stage
        /// skips every PDF with a warning rather than crashing.
        /// </summary>
        public string GhostscriptPath { get; set; } = "gswin64c";

        /// <summary>
        /// Optional absolute path to a custom ICC profile. When null/empty, the profile is
        /// extracted at runtime from <c>ImageMagick.ColorProfiles.USWebCoatedSWOP</c> — the
        /// SAME profile used by the per-image <c>ConvertToCmyk</c> pixel conversion, so the
        /// GS OutputIntent is color-consistent with the source pipeline (zero new licensing).
        /// </summary>
        public string IccProfilePath { get; set; }

        /// <summary>Per-PDF Ghostscript timeout in seconds (default 180).</summary>
        public int TimeoutSeconds { get; set; } = 180;

        /// <summary>
        /// Entry point dispatched from <see cref="AssetConverterConfig.Apply"/> when
        /// <see cref="ConverterMode.PdfCmykPostProcess"/> is set. Gated by
        /// <see cref="GetEnabled"/> (Release-only by default) and the master toggle.
        /// </summary>
        public static async Task Apply(AssetConverterConfig masterConfig)
        {
            var config = new PdfCmykPostProcessConfig();
            await Apply(masterConfig, config);
        }

        /// <summary>Overload accepting an explicit config instance (testable).</summary>
        public static async Task Apply(AssetConverterConfig masterConfig, PdfCmykPostProcessConfig config)
        {
            if (!config.IsEnabled)
            {
                Logger.Log("PDF CMYK post-process is disabled (IsEnabled=false). Skipping.");
                return;
            }

            if (!config.GetEnabled(masterConfig))
            {
                Logger.Log("PDF CMYK post-process is OFF for this build configuration. Skipping.");
                return;
            }

            var processor = new PdfCmykPostProcessor(config);
            await processor.ProcessAllAsync(masterConfig);
        }
    }
}
