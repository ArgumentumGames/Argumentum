using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;

namespace Argumentum.AssetConverter.PdfCmykPostProcess
{
    /// <summary>
    /// Runs the Ghostscript CMYK + OutputIntent post-pass over every PDF under the
    /// pipeline Target directory. See <see cref="PdfCmykPostProcessConfig"/> for scope.
    /// </summary>
    public class PdfCmykPostProcessor
    {
        private readonly PdfCmykPostProcessConfig _config;

        public PdfCmykPostProcessor(PdfCmykPostProcessConfig config)
        {
            _config = config;
        }

        /// <summary>Discovers every <c>*.pdf</c> under the Target root and converts each via Ghostscript.</summary>
        public async Task ProcessAllAsync(AssetConverterConfig masterConfig)
        {
            var gsResolved = ResolveGhostscript();
            if (gsResolved == null)
            {
                Logger.LogWarning(
                    $"PDF CMYK post-process: Ghostscript binary '{_config.GhostscriptPath}' not found "
                    + "(not on PATH and not an absolute existing path). Skipping the whole stage. "
                    + "Install Ghostscript (e.g. conda-forge user-scope) or set GhostscriptPath to enable CMYK+OutputIntent conversion.");
                return;
            }

            var targetRoot = Path.Combine(Environment.CurrentDirectory, masterConfig.BaseTargetDirectoryName);
            if (!Directory.Exists(targetRoot))
            {
                Logger.LogWarning($"PDF CMYK post-process: Target root '{targetRoot}' does not exist — nothing to convert.");
                return;
            }

            var pdfs = DiscoverPdfs(targetRoot);
            if (pdfs.Count == 0)
            {
                Logger.Log($"PDF CMYK post-process: no PDFs found under '{targetRoot}'. Nothing to convert.");
                return;
            }

            // Prepare the ICC profile once (custom path or extracted from Magick.NET).
            var iccPath = ResolveIccProfile();
            if (iccPath == null)
            {
                Logger.LogWarning("PDF CMYK post-process: ICC profile could not be resolved. Skipping the whole stage.");
                return;
            }

            Logger.Log($"PDF CMYK post-process: {pdfs.Count} PDF(s) to convert, GS='{gsResolved}', ICC='{iccPath}'.");

            int converted = 0, failed = 0;
            foreach (var pdf in pdfs)
            {
                var ok = await ConvertOneAsync(gsResolved, iccPath, pdf);
                if (ok) converted++; else failed++;
            }

            Logger.LogSuccess(
                $"PDF CMYK post-process complete: {converted}/{pdfs.Count} converted" + (failed > 0 ? $", {failed} failed (originals preserved)." : "."));
        }

        /// <summary>Recursively enumerates PDFs under the Target root.</summary>
        private static List<string> DiscoverPdfs(string targetRoot)
        {
            var result = new List<string>(64);
            try
            {
                result.AddRange(Directory.EnumerateFiles(targetRoot, "*.pdf", SearchOption.AllDirectories));
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return result;
        }

        /// <summary>Converts a single PDF in-place via Ghostscript (atomic move on success).</summary>
        public async Task<bool> ConvertOneAsync(string gsPath, string iccPath, string inputPdf)
        {
            var tmpDir = Path.Combine(Path.GetTempPath(), "argumentum-cmyk-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tmpDir);
            var outputPdf = Path.Combine(tmpDir, Path.GetFileNameWithoutExtension(inputPdf) + "-cmyk.pdf");
            var pdfxDefPath = Path.Combine(tmpDir, "PDFX_def.ps");

            try
            {
                await File.WriteAllTextAsync(pdfxDefPath, BuildPdfxDefPostscript(iccPath));

                var args = BuildGhostscriptArguments(iccPath, outputPdf, pdfxDefPath, inputPdf);
                var (exitCode, stderr) = await RunProcessAsync(gsPath, args, _config.TimeoutSeconds);

                if (exitCode != 0 || !File.Exists(outputPdf))
                {
                    Logger.LogWarning($"PDF CMYK post-process FAILED for '{inputPdf}' (GS exit={exitCode}). Original preserved. GS stderr: {stderr}");
                    return false;
                }

                // Atomic-ish replace: move the converted file over the original.
                File.Move(outputPdf, inputPdf, overwrite: true);
                Logger.LogSuccess($"PDF CMYK post-process OK: '{inputPdf}'.");
                return true;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                Logger.LogWarning($"PDF CMYK post-process EXCEPTION for '{inputPdf}'. Original preserved.");
                return false;
            }
            finally
            {
                TryCleanup(tmpDir);
            }
        }

        /// <summary>Resolves the GS binary: absolute existing path, else PATH probe. Null if not found.</summary>
        public string ResolveGhostscript()
        {
            var path = _config.GhostscriptPath;
            if (File.Exists(path)) return path;

            // "gswin64c" / "gs" — probe whether the shell can find it.
            try
            {
                var (exitCode, _) = RunProcessAsync(path, "--version", 10).GetAwaiter().GetResult();
                if (exitCode == 0) return path;
            }
            catch (Exception)
            {
                // Win32Exception when the binary is absent from PATH.
            }
            return null;
        }

        /// <summary>Resolves the ICC profile: custom path, else extracts Magick.NET USWebCoatedSWOP to a temp file.</summary>
        private string ResolveIccProfile()
        {
            if (!string.IsNullOrWhiteSpace(_config.IccProfilePath))
            {
                if (File.Exists(_config.IccProfilePath)) return Path.GetFullPath(_config.IccProfilePath);
                Logger.LogWarning($"PDF CMYK post-process: custom IccProfilePath '{_config.IccProfilePath}' not found.");
            }

            try
            {
                // Same profile as the per-image ConvertToCmyk pixel conversion (ImageHelper.cs) — color-consistent, zero new licensing.
                var bytes = ColorProfiles.USWebCoatedSWOP.ToByteArray();
                var iccPath = Path.Combine(Path.GetTempPath(), "argumentum-USWebCoatedSWOP.icc");
                File.WriteAllBytes(iccPath, bytes);
                return iccPath;
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return null;
            }
        }

        /// <summary>Builds the PDFX_def.ps postscript wrapper embedding the ICC as the PDF/X OutputIntent.</summary>
        public static string BuildPdfxDefPostscript(string iccPath)
        {
            // Normalize to a Postscript-compatible absolute path (forward slashes, escaped backslashes for the (…) literal).
            var psIccPath = iccPath.Replace("\\", "/");

            // Postscript pdfmark named objects use SINGLE braces ({icc_PDFX}). Only the first
            // segment below is interpolated ($"…"); the plain "…" segments must therefore use
            // single braces — "{{" in a non-interpolated segment stays a literal double brace,
            // which Ghostscript parses as a nested procedure and dies with a typecheck error.
            return $"/ICCProfile ({psIccPath}) def\n"
                + "[/_objdef {icc_PDFX} /type /stream /OBJ pdfmark\n"
                + "[{icc_PDFX} <</N 4>> /PUT pdfmark\n"
                + "[{icc_PDFX} ICCProfile (r) file /PUT pdfmark\n"
                + "[/_objdef {OutputIntent_PDFX} /type /dict /OBJ pdfmark\n"
                + "[{OutputIntent_PDFX} <</S /GTS_PDFX /Type /OutputIntent /DestOutputProfile {icc_PDFX} /OutputConditionIdentifier (CGATS TR 001) /Info (U.S. Web Coated \\(SWOP\\) v2) /RegistryName (http://www.color.org)>> /PUT pdfmark\n"
                + "[{Catalog} <</OutputIntents [ {OutputIntent_PDFX} ]>> /PUT pdfmark\n";
        }

        /// <summary>Builds the Ghostscript argument list (ai-01 POC-validated, #632).</summary>
        public static string BuildGhostscriptArguments(string iccPath, string outputPdf, string pdfxDefPath, string inputPdf)
        {
            // -dPDFX + Flate lossless + CMYK, no downsampling, ICC permit-read.
            return $"-dPDFX -dBATCH -dNOPAUSE -sDEVICE=pdfwrite "
                + $"-sColorConversionStrategy=CMYK -dProcessColorModel=/DeviceCMYK "
                + $"-dAutoFilterColorImages=false -dColorImageFilter=/FlateEncode "
                + $"-dAutoFilterGrayImages=false -dGrayImageFilter=/FlateEncode "
                + $"-dDownsampleColorImages=false "
                + $"-dDownsampleGrayImages=false "
                + $"-dDownsampleMonoImages=false "
                + $"--permit-file-read=\"{iccPath}\" "
                + $"-o \"{outputPdf}\" \"{pdfxDefPath}\" \"{inputPdf}\"";
        }

        /// <summary>Synchronous-style process runner with timeout + stderr capture.</summary>
        private static async Task<(int exitCode, string stderr)> RunProcessAsync(string fileName, string arguments, int timeoutSeconds)
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            using var process = new Process { StartInfo = psi, EnableRaisingEvents = true };
            process.Start();

            // Only stderr is captured (GS writes progress there). stdout is not redirected to
            // avoid a filled pipe buffer on large outputs (redirect without read = deadlock risk).
            var stderrTask = process.StandardError.ReadToEndAsync();

            var exited = process.WaitForExit(timeoutSeconds * 1000);
            if (!exited)
            {
                try { process.Kill(true); } catch { /* best-effort */ }
                var partialStderr = await stderrTask;
                return (-1, $"TIMEOUT after {timeoutSeconds}s. Partial stderr: {partialStderr}");
            }

            var stderr = await stderrTask;
            return (process.ExitCode, stderr);
        }

        private static void TryCleanup(string dir)
        {
            try { if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true); }
            catch { /* best-effort */ }
        }
    }
}
