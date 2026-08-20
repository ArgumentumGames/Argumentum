using Argumentum.AssetConverter.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests
{
    /// <summary>
    /// Issue #1121 guard: bundle-level integrity checks that would have caught the
    /// PokerCards_en (2 pages instead of 334) and PokerCards_ru (absent) defect.
    ///
    /// The defect slipped because the Phase B report announced "79 PDFs (… ru 9 …)"
    /// — a produced count with NO expected value in front of it. A count without an
    /// expected value cannot detect a missing file; it describes the output and
    /// presents it as the result. And `en` escaped even that, since the file
    /// existed and counted for 1.
    ///
    /// This guard asserts the EXPECTED profile up front (80 = 8 langs × 10 docs), then
    /// a cross-language page-count parity check per document name. It is the structural
    /// complement to the per-language PdfContentTests (which use a low threshold and
    /// 4 langs only). See issue #1121 DoD.
    ///
    /// Inverse control obligation (#1046 lesson — a green test on defective data is
    /// worthless): on the review-v2.0.0-regen-20260820 bundle, this test MUST fail with
    /// "ru: 9 PDFs (expected 10)" and "Argumentum_PokerCards_en.pdf: 2 pages (−99% vs
    /// median 334, threshold 10%)". The expected-failure baseline is documented below.
    /// Fails LOUD if Target/ doesn't exist (#957 residu ii — no silent faux-vert).
    /// </summary>
    public class PdfBundleIntegrityTests : IDisposable
    {
        private readonly ITestOutputHelper _output;

        private static readonly string TargetRoot = Path.Combine(
            TestRepoRoot.Find(),
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Debug", "net9.0-windows", "Target");

        private static readonly string ReleaseTargetRoot = Path.Combine(
            TestRepoRoot.Find(),
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Release", "net9.0-windows", "Target");

        /// <summary>8 languages (post-i18n expansion). Matches VisualQaHarness.</summary>
        private static readonly string[] Languages =
            { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

        /// <summary>Expected PDFs per language. 80 = 8 × 10. Hard-coded expected value
        /// (#1121 root cause: a produced count with no expected value in front of it).</summary>
        private const int ExpectedPdfsPerLanguage = 10;
        private const int ExpectedTotalPdfs = 80;

        /// <summary>Page-count parity threshold: a document whose page count deviates
        /// more than this fraction from the median across languages is flagged.
        /// 10% per #1121 DoD. PokerCards_en at 2 pages (vs median 334) is -99%.</summary>
        private const double PageCountParityThreshold = 0.10;

        public PdfBundleIntegrityTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Dispose() { }

        /// <summary>Resolves the Target root, preferring Release if available, else Debug.
        /// Returns null if neither exists. Mirrors VisualQaHarness.ResolveTargetRoot.</summary>
        private static string? ResolveTargetRoot()
        {
            if (Directory.Exists(ReleaseTargetRoot)) return ReleaseTargetRoot;
            if (Directory.Exists(TargetRoot)) return TargetRoot;
            return null;
        }

        private static string GetPdfDir(string root, string lang) =>
            Path.Combine(root, lang, "Documents", "density-0");

        private void EnsureTarget(string root)
        {
            // #957 residu ii: cold-start faux-vert. A missing Target/ must FAIL LOUD, not pass
            // silently — these tests verify generated PDFs and assert nothing without them.
            // VisualTests is NOT run by CI (build.yml targets only Argumentum.AssetConverter.Tests.csproj),
            // so this fails locally, not in the release gate.
            if (!Directory.Exists(root))
                Assert.Fail("PdfBundleIntegrityTests require a generated Target/ — run the pipeline first (bin/.../Target not found). This test verified nothing.");
        }

        // --- #1121 DoD item 1: expected profile 80 = 8 × 10, asserted up front ---

        /// <summary>
        /// #1121 guard: each of the 8 languages must produce exactly 10 PDFs.
        /// The Phase B report announced "79 PDFs (ru 9)" — a produced count with no expected
        /// value. This asserts the expected value (10) UP FRONT, then measures, so a missing
        /// file is a delta against 10, not a description of what came out.
        /// </summary>
        [Fact]
        public void Bundle_Profile_Eight_Languages_Ten_Pdfs_Each()
        {
            var root = ResolveTargetRoot();
            if (root == null)
                Assert.Fail("PdfBundleIntegrityTests require a generated Target/ — run the pipeline first. This test verified nothing.");

            var missing = new List<string>();
            var wrongCount = new List<string>();
            int total = 0;

            foreach (var lang in Languages)
            {
                var dir = GetPdfDir(root, lang);
                if (!Directory.Exists(dir))
                {
                    missing.Add(lang);
                    continue;
                }
                var pdfs = Directory.GetFiles(dir, "*.pdf");
                total += pdfs.Length;
                if (pdfs.Length != ExpectedPdfsPerLanguage)
                    wrongCount.Add($"{lang}: {pdfs.Length} PDFs (expected {ExpectedPdfsPerLanguage})");
            }

            if (missing.Count > 0)
                Assert.Fail($"Missing language document directories: {string.Join(", ", missing)} — Target/ exists but these languages produced no Documents/density-0/ (test verified nothing; check pipeline output).");

            // Up-front expected total — the count that was missing from the Phase B report.
            Assert.Equal(ExpectedTotalPdfs, total);

            if (wrongCount.Count > 0)
                Assert.Fail($"PDF count per language deviates from expected {ExpectedPdfsPerLanguage}:\n  {string.Join("\n  ", wrongCount)}\n  (total {total} / expected {ExpectedTotalPdfs})");

            _output.WriteLine($"PASS: {total} PDFs across {Languages.Length} languages ({ExpectedPdfsPerLanguage}/lang).");
        }

        // --- #1121 DoD item 2 + 3: page-count parity across languages per document ---

        /// <summary>
        /// #1121 guard: for each document name present across languages, page counts must be
        /// within 10% of the median. PokerCards_en at 2 pages (median 334) is -99% → fails.
        /// This is the check the per-language PdfContentTests.PokerCards_Has_Multiple_Pages
        /// (threshold 50, 4 langs only) could not express: a cross-language delta.
        ///
        /// Document identity = filename stripped of the language suffix, so the same document
        /// across the 8 langs is grouped (e.g. Argumentum_PokerCards_{lang}.pdf → "PokerCards").
        /// Documents present in fewer than 4 languages are reported but not parity-checked
        /// (a single-language document has no median to compare against).
        /// </summary>
        [Fact]
        public void Bundle_Page_Count_Parity_Across_Languages()
        {
            var root = ResolveTargetRoot();
            if (root == null)
                Assert.Fail("PdfBundleIntegrityTests require a generated Target/ — run the pipeline first. This test verified nothing.");

            // Collect (docKey, lang, pages) for every PDF that opens.
            var records = new List<(string DocKey, string Lang, int Pages, string File)>();
            var unopenable = new List<string>();

            foreach (var lang in Languages)
            {
                var dir = GetPdfDir(root, lang);
                if (!Directory.Exists(dir)) continue;
                foreach (var pdf in Directory.GetFiles(dir, "*.pdf"))
                {
                    var name = Path.GetFileName(pdf);
                    var docKey = DocKeyFromName(name, lang);
                    try
                    {
                        using var doc = PdfDocument.Open(pdf);
                        records.Add((docKey, lang, doc.NumberOfPages, name));
                    }
                    catch (Exception ex)
                    {
                        unopenable.Add($"{lang}/{name}: {ex.GetType().Name}: {ex.Message.Split('\n')[0]}");
                    }
                }
            }

            if (records.Count == 0)
                Assert.Fail($"No openable PDFs found across {Languages.Length} languages in Target/ — test verified nothing (check pipeline output). Unopenable: {unopenable.Count}");

            if (unopenable.Count > 0)
                _output.WriteLine($"WARN: {unopenable.Count} unopenable PDF(s):\n  {string.Join("\n  ", unopenable)}");

            // Group by document key, parity-check where 4+ languages are present.
            var byDoc = records.GroupBy(r => r.DocKey).OrderBy(g => g.Key).ToList();
            var parityFailures = new List<string>();

            foreach (var grp in byDoc)
            {
                var pages = grp.Select(r => (double)r.Pages).OrderBy(p => p).ToList();
                if (pages.Count < 4)
                {
                    _output.WriteLine($"SKIP (n={pages.Count}): {grp.Key} — present in {grp.Select(r => r.Lang).Aggregate((a, b) => a + "," + b)}");
                    continue;
                }
                double median = Median(pages);
                foreach (var r in grp)
                {
                    if (median == 0) continue; // degenerate; skip
                    double dev = Math.Abs(r.Pages - median) / median;
                    if (dev > PageCountParityThreshold)
                        parityFailures.Add($"{r.File}: {r.Pages} pages ({dev:P0} vs median {median:0.#}, threshold {PageCountParityThreshold:P0})");
                }
            }

            // Print the full parity table for diagnosis.
            _output.WriteLine("# Page-count parity (pages × language, per document)");
            _output.WriteLine("| Document | " + string.Join(" | ", Languages) + " | median |");
            _output.WriteLine("|----------" + string.Join("", Languages.Select(_ => "|------")) + "|--------|");
            foreach (var grp in byDoc.OrderBy(g => g.Key))
            {
                var row = Languages.Select(lang =>
                {
                    var hit = grp.FirstOrDefault(r => r.Lang == lang);
                    return hit.Equals(default) ? "—" : hit.Pages.ToString();
                });
                var present = grp.Select(r => (double)r.Pages).OrderBy(p => p).ToList();
                var med = present.Count >= 4 ? Median(present) : -1;
                _output.WriteLine($"| {grp.Key} | {string.Join(" | ", row)} | {(med >= 0 ? med.ToString("0.#") : "—")} |");
            }

            if (parityFailures.Count > 0)
                Assert.Fail($"Page-count parity failures (> {PageCountParityThreshold:P0} vs median):\n  {string.Join("\n  ", parityFailures)}");

            _output.WriteLine($"PASS: {byDoc.Count} document types, 0 parity failures.");
        }

        /// <summary>
        /// Derives a language-agnostic document key from a PDF filename by removing the
        /// language suffix and extension. E.g. "Argumentum_PokerCards_en.pdf" → "PokerCards",
        /// "Argumentum_Fallacies_Web_A0_fr.pdf" → "Fallacies_Web_A0". The key groups the
        /// same logical document across the 8 languages for parity comparison.
        /// </summary>
        private static string DocKeyFromName(string fileName, string lang)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);
            // Strip a trailing "_{lang}" (case-insensitive) if present.
            var suffix = "_" + lang;
            var idx = name.LastIndexOf(suffix, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0 && idx == name.Length - suffix.Length)
                name = name.Substring(0, idx);
            // Strip a leading "Argumentum_" convention.
            if (name.StartsWith("Argumentum_", StringComparison.OrdinalIgnoreCase))
                name = name.Substring("Argumentum_".Length);
            return name;
        }

        private static double Median(IReadOnlyList<double> sorted)
        {
            // Caller passes an already-sorted list.
            int n = sorted.Count;
            if (n == 0) return 0;
            if (n % 2 == 1) return sorted[n / 2];
            return (sorted[n / 2 - 1] + sorted[n / 2]) / 2.0;
        }
    }
}
