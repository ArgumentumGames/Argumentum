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

        // --- #1176: rendered-content distinction across languages ---

        /// <summary>Explicit bundle override. When set, the content-distinction guard runs
        /// against that directory (layout {lang}/Documents/density-0/*.pdf) and MUST exist —
        /// an explicit request that cannot be satisfied fails loud, it never falls back to
        /// Target/ and never passes green (#1176 no-silent-skill rule, cf #909/#957).</summary>
        private const string BundleRootEnvVar = "ARGUMENTUM_PDF_BUNDLE_ROOT";

        /// <summary>
        /// #1176 expected value, declared per the DoD. ZERO identical FACE pages between any
        /// two languages of any document — no exception, ever. Basis (measured on the
        /// v0.9.0-review assets of 24/08, sha256-pinned, PyMuPDF 25 DPI page rasterization):
        /// TarotCards fr/en 0/381 · TarotCards_Print&amp;Play_A4 fr/en 0/105 ·
        /// Fallacies_Web_A4 fr/en 0/15 · TarotCards_Virtues fr/en 0/262 ·
        /// PokerCards all 15 pairs among {fr,en,es,pt,fa,zh} 0 identical faces /334 ·
        /// PokerCards_Print&amp;Play_A4 fa/fr 0/38 (both roles).
        /// Backs follow the documented category-label table below.
        /// </summary>
        private const int ExpectedIdenticalPagesPerLanguagePair = 0;

        /// <summary>
        /// #1176 documented exception: LEGITIMATELY shared BACK pages, exact-count asserted.
        /// The PokerCards (Scenarii deck) back design is the CATEGORY card — same illustration
        /// in every language, only the label text varies. When two languages' category labels
        /// render to the same bytes (the template uppercases the label), their backs are
        /// honestly identical. Measured on the v0.9.0-review assets (sha256-pinned vs the
        /// release API, 25 DPI raster, 25/08) and cross-derived from the source CSV
        /// (Cards/Scenarii/Argumentum Scenarii - Cards.csv: 7 categories with counts
        /// 36/30/27/25/18/17/14 — exactly the 7 back designs measured in the PDFs):
        /// - en+fr: 'Pop culture' / 'pop culture' → 18 identical back pages (223..257), 0 faces;
        /// - es+pt: 'Política' / 'política' → 14 identical back pages (259..285), 0 faces;
        /// - every other pair of the 15 measured: 0 (all labels differ).
        /// The exact count makes this a drift tripwire: a category rename in the CSV moves the
        /// observed count and the test fails asking for the table to be re-derived — in BOTH
        /// directions (more sharing than documented is as much a finding as less).
        /// Scoped to docKey "PokerCards" ONLY: the Print&amp;Play variant is an imposition of
        /// the same deck but its sheets mix face images (always language-distinct), so any
        /// identical page there is a defect — it fails loud and names the page.
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, int>> ExpectedSharedBackPages =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["PokerCards"] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["en+fr"] = 18,
                    ["es+pt"] = 14,
                },
            };

        /// <summary>
        /// #1176 guard: no document may render IDENTICAL content in two languages. A full
        /// recopy of the FR render has the FR pagination, format and size — the structural
        /// checks above are maximally green BECAUSE of the defect. This guard hashes the
        /// rendered content instead: the ordered image XObject streams of each page
        /// (QuestPDF documents are pure-image; PdfPig enumerates the streams, cf
        /// PdfAuditor). The container is never hashed (deflate/zlib drift — #871 lesson).
        ///
        /// Form obligations (each paid for by an incident):
        /// - sweeps ALL languages present per document (grouping, never a chosen pair —
        ///   the ar/fr pair hid the PokerCards en/es/ru recopy because ar was healthy);
        /// - labels pages by recto-verso role, odd = back, even = face (#1141 lived on 131
        ///   odd pages; grid documents get a nominal parity label);
        /// - names every colliding page + language set in the failure (no boolean verdict);
        /// - tolerates ONLY the documented category-label back sharing
        ///   (ExpectedSharedBackPages), verified as an EXACT count — never a blanket
        ///   backs-are-fine pass.
        /// Expected-failure baseline: on the 24/08 bundle this test MUST fail naming
        /// PokerCards_Print&amp;Play_A4 pages 1..38 identical across {en, es, fr, ru}
        /// (#1177 defect). If it runs green on a bundle where that defect is present, it
        /// verifies nothing.
        /// </summary>
        [Fact]
        public void All_Documents_Render_Distinctly_Across_Languages()
        {
            var root = ResolveBundleRootOrFail();
            if (root == null)
                Assert.Fail("PdfBundleIntegrityTests require a generated Target/ — run the pipeline first. This test verified nothing.");

            var missingLangDirs = new List<string>();
            var byDocLang = new Dictionary<string, SortedDictionary<string, string>>();
            foreach (var lang in Languages)
            {
                var dir = GetPdfDir(root, lang);
                if (!Directory.Exists(dir))
                {
                    missingLangDirs.Add(lang);
                    continue;
                }
                foreach (var pdf in Directory.GetFiles(dir, "*.pdf"))
                {
                    var key = NormalizeDocKey(DocKeyFromName(Path.GetFileName(pdf), lang));
                    if (!byDocLang.TryGetValue(key, out var langs))
                        byDocLang[key] = langs = new SortedDictionary<string, string>();
                    langs[lang] = pdf;
                }
            }

            if (byDocLang.Count == 0)
                Assert.Fail($"No PDFs found under {root} — test verified nothing (check the bundle layout: {{lang}}/Documents/density-0/*.pdf).");

            var failures = new List<string>();
            var notes = new List<string>();
            int totalDocumentedSharedBacks = 0;

            foreach (var (docKey, langs) in byDocLang.OrderBy(kv => kv.Key))
            {
                if (langs.Count < 2)
                {
                    notes.Add($"SKIP (n=1): {docKey} present only in {string.Join(",", langs.Keys)} — no cross-language comparison possible");
                    continue;
                }

                // Page-content key per language: ordered per-image stream hashes of the page.
                var pageKeys = new Dictionary<string, List<string>>();
                var unopenable = new List<string>();
                foreach (var (lang, file) in langs)
                {
                    try
                    {
                        pageKeys[lang] = PageContentKeys(file);
                    }
                    catch (Exception ex)
                    {
                        unopenable.Add($"{lang}: {ex.GetType().Name}: {ex.Message.Split('\n')[0]}");
                    }
                }

                if (pageKeys.Count < 2)
                {
                    failures.Add($"{docKey}: only {pageKeys.Count} openable render(s) of {langs.Count} language file(s) — comparison impossible. Unopenable: {string.Join("; ", unopenable)}");
                    continue;
                }
                if (unopenable.Count > 0)
                    notes.Add($"WARN {docKey}: unopenable: {string.Join("; ", unopenable)}");

                var counts = pageKeys.Values.Select(v => v.Count).ToList();
                if (counts.Distinct().Count() > 1)
                    notes.Add($"WARN {docKey}: page counts differ across languages ({string.Join("/", counts)} in {string.Join(",", pageKeys.Keys)}) — comparing the first {counts.Min()} only; parity is asserted separately");

                int minPages = counts.Min();
                var roleLabel = DocRoleLabel(docKey);
                int compared = 0, skippedNoImages = 0;
                var docFailures = new List<string>();
                var sharedBackPages = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
                var backExceptions = ExpectedSharedBackPages.TryGetValue(docKey, out var table) ? table : null;

                for (int i = 0; i < minPages; i++)
                {
                    var groups = pageKeys.GroupBy(kv => kv.Value[i]).ToList();
                    foreach (var g in groups)
                    {
                        if (g.Key.Length == 0)
                        {
                            // A page with no image XObject in at least one language: structurally
                            // non-comparable (blank/empty page) — visible note, never a silent pass
                            // and never a structural false red.
                            skippedNoImages++;
                            continue;
                        }
                        if (g.Count() >= 2)
                        {
                            var setKey = string.Join("+", g.Select(kv => kv.Key).OrderBy(l => l, StringComparer.Ordinal));
                            var parity = i % 2 == 0 ? "back" : "face"; // 0-based even = 1-indexed odd = back
                            if (parity == "back" && backExceptions != null && backExceptions.ContainsKey(setKey))
                            {
                                if (!sharedBackPages.TryGetValue(setKey, out var pages))
                                    sharedBackPages[setKey] = pages = new List<int>();
                                pages.Add(i + 1);
                            }
                            else
                            {
                                docFailures.Add($"{docKey} page {i + 1} ({parity}{roleLabel}): identical rendered content across {string.Join(", ", g.Select(kv => kv.Key))}");
                            }
                        }
                    }
                    compared++;
                }

                // Exact-count verification of the documented shared-back exceptions — drift
                // tripwire in both directions (more sharing than documented is as much a
                // finding as less). Verified only when every language of the pair is present
                // in the bundle; a partial bundle gets a visible note, never a green check
                // and never a false red.
                if (backExceptions != null)
                {
                    foreach (var (setKey, expected) in backExceptions)
                    {
                        var pairLangs = setKey.Split('+');
                        if (!pairLangs.All(l => pageKeys.ContainsKey(l)))
                        {
                            notes.Add($"NOTE {docKey}: documented shared-back pair {setKey.Replace("+", ", ")} not verifiable on this bundle (absent: {string.Join(",", pairLangs.Where(l => !pageKeys.ContainsKey(l)))}) — exact count checked only when all its languages are present");
                            continue;
                        }
                        var observed = sharedBackPages.TryGetValue(setKey, out var pages) ? pages.Count : 0;
                        if (observed != expected)
                            docFailures.Add($"{docKey}: shared back pages across {setKey.Replace("+", ", ")}: observed {observed}, documented {expected} (pages: {(pages != null ? string.Join(",", pages) : "none")}) — if a category label changed in Cards/Scenarii, re-derive ExpectedSharedBackPages (#1176)");
                    }
                }

                var sharedBackTotal = sharedBackPages.Values.Sum(p => p.Count);
                totalDocumentedSharedBacks += sharedBackTotal;
                _output.WriteLine($"{docKey}: {langs.Count} langs × {compared} pages compared, {docFailures.Count} identical, {sharedBackTotal} documented shared backs ({string.Join("; ", sharedBackPages.Select(kv => $"{kv.Key}: {kv.Value.Count}"))}), {skippedNoImages} skipped (no images)");
                failures.AddRange(docFailures);
            }

            foreach (var n in notes)
                _output.WriteLine(n);
            if (missingLangDirs.Count > 0)
                _output.WriteLine($"WARN: missing language dirs (not compared): {string.Join(", ", missingLangDirs)}");

            if (failures.Count > 0)
            {
                var shown = failures.Take(60).ToList();
                var more = failures.Count > 60 ? $"\n  … (+{failures.Count - 60} more)" : "";
                Assert.Fail($"Identical rendered content across languages (expected {ExpectedIdenticalPagesPerLanguagePair} identical FACE page per language pair; backs: 0 except the documented category-label table ExpectedSharedBackPages — a shared page means one language is not localized, #1176/#1177):\n  {string.Join("\n  ", shown)}{more}");
            }

            _output.WriteLine($"PASS: {byDocLang.Count(kv => kv.Value.Count >= 2)} document type(s) compared across languages, 0 identical pages, {totalDocumentedSharedBacks} documented shared backs.");
        }

        /// <summary>Resolves the bundle root: the env var when explicitly set (must exist —
        /// fails loud otherwise, no fallback, no green), else the generated Target tree
        /// (Release preferred, else Debug), else null for the caller to fail loud.</summary>
        private static string? ResolveBundleRootOrFail()
        {
            var envRoot = Environment.GetEnvironmentVariable(BundleRootEnvVar);
            if (!string.IsNullOrWhiteSpace(envRoot))
            {
                if (!Directory.Exists(envRoot))
                    Assert.Fail($"{BundleRootEnvVar} is set to '{envRoot}' which does not exist — an explicitly requested bundle must fail loud, never fall back to Target/ nor pass green (#1176 no-silent-skip).");
                return envRoot;
            }
            return ResolveTargetRoot();
        }

        /// <summary>Release uploads rename "Print&amp;Play" to "Print.Play"; the local Target
        /// tree keeps "Print&amp;Play". Normalizing lets one expected-value space cover both
        /// bundle layouts.</summary>
        private static string NormalizeDocKey(string docKey) =>
            docKey.Replace("Print.Play", "Print&Play", StringComparison.OrdinalIgnoreCase);

        /// <summary>Fallacies_Web documents are card grids (no recto-verso); the back/face
        /// parity label is nominal there.</summary>
        private static string DocRoleLabel(string docKey) =>
            docKey.Contains("Fallacies_Web", StringComparison.OrdinalIgnoreCase) ? " — grid doc, parity label nominal" : "";

        /// <summary>Per-page rendered-content key: the ordered MD5 of each image XObject's
        /// raw stream on that page. Pure-image QuestPDF documents make this the content of
        /// the page as rendered. Empty page → empty key (handled by the caller).</summary>
        private static List<string> PageContentKeys(string pdfPath)
        {
            using var doc = PdfDocument.Open(pdfPath);
            using var md5 = System.Security.Cryptography.MD5.Create();
            var keys = new List<string>(doc.NumberOfPages);
            foreach (var page in doc.GetPages())
            {
                var parts = new List<string>();
                foreach (var image in page.GetImages())
                    parts.Add(Convert.ToHexString(md5.ComputeHash(image.RawBytes.ToArray())));
                keys.Add(string.Join("|", parts));
            }
            return keys;
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
