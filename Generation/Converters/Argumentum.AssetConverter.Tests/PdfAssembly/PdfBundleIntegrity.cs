using System;
using System.Collections.Generic;
using System.Linq;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    /// <summary>
    /// Pure logic for the #1121 bundle-integrity guard, extracted so it can be unit-tested
    /// without generated PDFs. The guard's two checks live in
    /// <c>Argumentum.AssetConverter.VisualTests.PdfBundleIntegrityTests</c> (which needs a
    /// real <c>Target/</c>); this class holds the count + parity logic so a CI test can prove
    /// the guard *would* fail on the #1121 defect (PokerCards_en 2 pages, PokerCards_ru absent).
    /// </summary>
    internal static class PdfBundleIntegrity
    {
        /// <summary>Expected PDFs per language in the 8-language bundle. 80 = 8 × 10.</summary>
        public const int ExpectedPdfsPerLanguage = 10;

        /// <summary>8 languages (post-i18n expansion).</summary>
        public static readonly string[] Languages =
            { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

        /// <summary>Page-count parity threshold: deviation &gt; this fraction of the median = failure.</summary>
        public const double PageCountParityThreshold = 0.10;

        /// <summary>
        /// Checks the per-language PDF count against the expected profile.
        /// Returns failure messages (missing dirs, wrong counts). Empty = pass.
        /// </summary>
        public static IReadOnlyList<string> CheckProfile(IReadOnlyDictionary<string, int> pdfCountsByLanguage)
        {
            var failures = new List<string>();
            var missing = Languages.Where(l => !pdfCountsByLanguage.ContainsKey(l)).ToList();
            if (missing.Count > 0)
                failures.Add($"Missing language document directories: {string.Join(", ", missing)}");

            foreach (var lang in Languages)
            {
                if (pdfCountsByLanguage.TryGetValue(lang, out int count) && count != ExpectedPdfsPerLanguage)
                    failures.Add($"{lang}: {count} PDFs (expected {ExpectedPdfsPerLanguage})");
            }
            return failures;
        }

        /// <summary>
        /// Checks page-count parity across languages for each document key.
        /// <paramref name="records"/> = (docKey, lang, pages) tuples. Documents present in
        /// fewer than 4 languages are skipped (no median). Returns failure messages naming the
        /// file, its page count, and the deviation. Empty = pass.
        /// </summary>
        public static IReadOnlyList<string> CheckParity(IEnumerable<(string DocKey, string Lang, int Pages, string File)> records)
        {
            var byDoc = records.GroupBy(r => r.DocKey).OrderBy(g => g.Key).ToList();
            var failures = new List<string>();

            foreach (var grp in byDoc)
            {
                var present = grp.ToList();
                if (present.Count < 4) continue; // not enough languages to define a median

                var pages = present.Select(r => (double)r.Pages).OrderBy(p => p).ToList();
                double median = Median(pages);

                foreach (var r in present)
                {
                    if (median == 0) continue;
                    double dev = Math.Abs(r.Pages - median) / median;
                    if (dev > PageCountParityThreshold)
                        failures.Add($"{r.File}: {r.Pages} pages ({dev:P0} vs median {median:0.#}, threshold {PageCountParityThreshold:P0})");
                }
            }
            return failures;
        }

        /// <summary>Derives a language-agnostic document key from a PDF filename.
        /// "Argumentum_PokerCards_en.pdf" → "PokerCards". Mirrors the VisualTests helper.</summary>
        public static string DocKeyFromName(string fileName, string lang)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(fileName);
            var suffix = "_" + lang;
            var idx = name.LastIndexOf(suffix, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0 && idx == name.Length - suffix.Length)
                name = name.Substring(0, idx);
            if (name.StartsWith("Argumentum_", StringComparison.OrdinalIgnoreCase))
                name = name.Substring("Argumentum_".Length);
            return name;
        }

        private static double Median(IReadOnlyList<double> sorted)
        {
            int n = sorted.Count;
            if (n == 0) return 0;
            if (n % 2 == 1) return sorted[n / 2];
            return (sorted[n / 2 - 1] + sorted[n / 2]) / 2.0;
        }
    }
}
