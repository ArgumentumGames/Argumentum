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
        /// v0.9.0-review assets of 24-25/08, sha256-pinned against the release):
        /// TarotCards fr/en 0/381 · TarotCards_Print&amp;Play_A4 fr/en 0/105 ·
        /// Fallacies_Web_A4 fr/en 0/15 · TarotCards_Virtues fr/en 0/262 ·
        /// PokerCards all 15 pairs among {fr,en,es,pt,fa,zh} 0 identical faces /334.
        /// Backs follow the computed sheet predicate below — on the same release bundle the
        /// only cross-language back sharing is: base deck en+fr 18 (pop culture label
        /// coincidence) · es+pt 14 (política) · ar+fa 17 (History: the labels differ as
        /// strings but fold to the same glyphs in the template font, see NormalizeLabel);
        /// Print&amp;Play_A4 en+fr 1 (page 27, homogeneous sheet #13) · ar+fa 1 (page 1,
        /// sheet #0) · es+pt 0.
        /// </summary>
        private const int ExpectedIdenticalPagesPerLanguagePair = 0;

        /// <summary>All PokerCards documents are impositions of the same Scenarii deck —
        /// the shared-back predicate applies to the whole family, never to one variant.</summary>
        private const string ScenariiDeckDocKeyPrefix = "PokerCards";

        /// <summary>
        /// Back-sheet size per Scenarii-deck document: how many card BACKS one back page
        /// carries. This is an imposition FACT, not an exemption — the base deck renders one
        /// card per page (334 pages = 167 backs + 167 faces), the Print&amp;Play A4 imposes a
        /// 3×3 grid (38 pages = 19 back sheets × 9 + 19 face sheets; sheets alternate, back
        /// sheets on 1-indexed odd pages, back sheet s carrying deck cards [s·k … s·k+k−1]).
        /// The predicate generalizes over it: a back page is legitimately shared between two
        /// languages iff EVERY card its sheet carries belongs to a category whose label
        /// coincides between them. With k=1 that degenerates to per-card coincidence; with
        /// k=9 only a homogeneous sheet counts — pop culture spans deck indices 111–128, so
        /// exactly one full sheet (#13) is homogeneous and lands on page 2×13+1 = 27, while
        /// the 14 politics cards straddle two mixed sheets and the Print&amp;Play expects none
        /// for es/pt. A Scenarii-deck document with no registered sheet size fails loud
        /// instead of silently expecting zero (#1176).
        /// </summary>
        private static readonly Dictionary<string, int> BackSheetSizes =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["PokerCards"] = 1,
                ["PokerCards_Print&Play_A4"] = 9,
            };

        private static readonly Dictionary<int, Dictionary<string, List<int>>> SharedBackPagesCache = new();

        private static Dictionary<string, List<int>> SharedBackPagesFor(int backSheetSize)
        {
            if (!SharedBackPagesCache.TryGetValue(backSheetSize, out var cached))
                SharedBackPagesCache[backSheetSize] = cached = ComputeExpectedSharedBackPages(backSheetSize);
            return cached;
        }

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
        /// - tolerates ONLY the computed category-label back sharing
        ///   (ComputeExpectedSharedBackPages, parameterized by the document's back-sheet
        ///   size), verified as EXACT PAGE POSITIONS — never a blanket backs-are-fine pass.
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
                // Computed predicate: a back page is admissible only if every source string
                // its SHEET renders (the category labels of all cards it carries) coincides
                // between the languages — derived from the CSV at test time, parameterized by
                // the document's back-sheet size, never a hardcoded page whitelist. Applies to
                // the whole Scenarii-deck family; an unregistered variant fails loud (#1176).
                Dictionary<string, List<int>>? expectedSharedBackPages = null;
                var backSheetSize = 0;
                if (docKey.StartsWith(ScenariiDeckDocKeyPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    if (!BackSheetSizes.TryGetValue(docKey, out backSheetSize))
                        docFailures.Add($"{docKey}: Scenarii-deck document with no registered back-sheet size — register the imposition fact in {nameof(BackSheetSizes)}, never exempt the document (#1176).");
                    else
                        expectedSharedBackPages = SharedBackPagesFor(backSheetSize);
                }

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
                            if (parity == "back" && expectedSharedBackPages != null && expectedSharedBackPages.ContainsKey(setKey))
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

                // Exact-POSITION verification of the computed shared-back pages — the drift
                // tripwire in both directions, and the explicit assertion of the deck-order
                // hypothesis. The derivation holds only if the deck is laid out in CSV row
                // order and back sheet s carries cards [s·k … s·k+k−1] on page 2s+1; if the
                // deck order, the imposition, or a category label changes, the computed page
                // positions move and this check goes RED NAMING the hypothesis instead of
                // rendering a silently-wrong count. Verified only when every language of the
                // pair is present in the bundle; a partial bundle gets a visible note, never
                // a green check and never a false red.
                if (expectedSharedBackPages != null)
                {
                    foreach (var (setKey, expectedPages) in expectedSharedBackPages)
                    {
                        var pairLangs = setKey.Split('+');
                        if (!pairLangs.All(l => pageKeys.ContainsKey(l)))
                        {
                            notes.Add($"NOTE {docKey}: computed shared-back pair {setKey.Replace("+", ", ")} not verifiable on this bundle (absent: {string.Join(",", pairLangs.Where(l => !pageKeys.ContainsKey(l)))}) — positions checked only when all its languages are present");
                            continue;
                        }
                        var observedPages = sharedBackPages.TryGetValue(setKey, out var pages)
                            ? pages : new List<int>();
                        var missing = expectedPages.Except(observedPages).OrderBy(p => p).ToList();
                        var extra = observedPages.Except(expectedPages).OrderBy(p => p).ToList();
                        if (missing.Count > 0 || extra.Count > 0)
                            docFailures.Add($"{docKey}: shared back pages across {setKey.Replace("+", ", ")}: computed [{string.Join(",", expectedPages)}], observed [{string.Join(",", observedPages)}]" +
                                $"{(missing.Count > 0 ? $", expected-but-absent [{string.Join(",", missing)}]" : "")}{(extra.Count > 0 ? $", unexpected [{string.Join(",", extra)}]" : "")}" +
                                $" — the derivation assumes deck order = CSV row order and back sheet s (cards s×{backSheetSize}…s×{backSheetSize}+{backSheetSize - 1}) on page 2s+1, labels compared as the render compares them (case + script-variant fold). If the deck order, the imposition, or a category label changed, re-derive the predicate — do not tune the page list (#1176).");
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
                Assert.Fail($"Identical rendered content across languages (expected {ExpectedIdenticalPagesPerLanguagePair} identical FACE page per language pair; backs: only a whole back SHEET whose category labels all coincide between the pair, per the computed predicate ComputeExpectedSharedBackPages parameterized by back-sheet size — a shared page otherwise means one language is not localized, #1176/#1177):\n  {string.Join("\n  ", shown)}{more}");
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

        /// <summary>Maps each deck language to its Scenarii CSV category column. The base
        /// Scenarii deck's FR column is the accented 'catégorie', the EN column is the plain
        /// 'category', and the other six use the per-language 'category_{lang}' suffix.</summary>
        private static readonly (string Lang, string Col)[] SharedBackCategoryColumns =
        {
            ("fr", "catégorie"), ("en", "category"), ("ru", "category_ru"), ("pt", "category_pt"),
            ("es", "category_es"), ("ar", "category_ar"), ("fa", "category_fa"), ("zh", "category_zh"),
        };

        /// <summary>
        /// #1176 computed predicate (constraint (a)): derive the expected shared-back PAGE
        /// POSITIONS per language pair from the source CSV instead of a hardcoded page list.
        /// The deck is the CSV's rows in order; back sheet s carries deck cards
        /// [s·k … s·k+k−1] (k = backSheetSize) and renders on 1-indexed page 2s+1. A back
        /// page is legitimately shared between two languages iff EVERY card its sheet carries
        /// belongs to a category whose label coincides between them, compared the way the
        /// render compares them (see NormalizeLabel). With k=1 this is per-card coincidence;
        /// with k=9 only a homogeneous sheet counts. Asserting POSITIONS (not just counts)
        /// is the explicit deck-order hypothesis: a reorder or a label change moves the
        /// computed positions and the exact-position check goes red naming the assumption.
        /// Throws FileNotFoundException if the CSV is absent — the predicate has no source,
        /// and that must fail loud, never pass silently.
        /// </summary>
        private static Dictionary<string, List<int>> ComputeExpectedSharedBackPages(int backSheetSize)
        {
            var csvPath = Path.Combine(TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");
            if (!File.Exists(csvPath))
                throw new FileNotFoundException(
                    "PdfBundleIntegrityTests: the Scenarii CSV is required to compute which back pages may be legitimately shared across languages (#1176) — not found at " + csvPath,
                    csvPath);

            var records = ParseSimpleCsv(csvPath);
            var result = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
            if (records.Count == 0) return result;

            var header = records[0];
            var colIdx = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < header.Count; i++) colIdx[header[i]] = i;

            if (!colIdx.ContainsKey("category"))
                throw new InvalidDataException("Scenarii CSV is missing the 'category' (English) column used as the category identity for shared-back derivation.");

            int enCol = colIdx["category"];
            // Deck order = CSV row order (asserted by the exact-position check upstream).
            var deck = new List<string>();
            var catLabels = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);

            foreach (var row in records.Skip(1))
            {
                if (enCol >= row.Count) continue;
                var cat = row[enCol].Trim();
                if (cat.Length == 0) continue;
                deck.Add(cat);
                if (!catLabels.TryGetValue(cat, out var labs))
                    catLabels[cat] = labs = new(StringComparer.Ordinal);
                foreach (var (lang, col) in SharedBackCategoryColumns)
                {
                    if (colIdx.TryGetValue(col, out var ci) && ci < row.Count)
                    {
                        var v = row[ci].Trim();
                        if (v.Length > 0) labs[lang] = v;
                    }
                }
            }

            var langs = SharedBackCategoryColumns.Select(x => x.Lang).ToArray();
            for (int a = 0; a < langs.Length; a++)
                for (int b = a + 1; b < langs.Length; b++)
                {
                    var pages = new List<int>();
                    for (int s = 0; s * backSheetSize < deck.Count; s++)
                    {
                        bool coincides = true;
                        for (int j = s * backSheetSize; coincides && j < Math.Min((s + 1) * backSheetSize, deck.Count); j++)
                        {
                            var labs = catLabels[deck[j]];
                            coincides = labs.TryGetValue(langs[a], out var va)
                                && labs.TryGetValue(langs[b], out var vb)
                                && NormalizeLabel(va) == NormalizeLabel(vb);
                        }
                        if (coincides) pages.Add(2 * s + 1);
                    }
                    if (pages.Count > 0)
                    {
                        var key = string.Join("+", new[] { langs[a], langs[b] }.OrderBy(l => l, StringComparer.Ordinal));
                        result[key] = pages;
                    }
                }
            return result;
        }

        /// <summary>The template normalizes the category label (uppercases it) before rendering,
        /// so the coincidence predicate compares labels the same way the render does. It also
        /// folds script variants onto the glyph the template font actually draws: measured on
        /// the v0.9.0-review bundle, the History labels تاريخ (ar) and تاریخ (fa) — Arabic vs
        /// Persian yeh, distinct codepoints — render to byte-identical back images, and the
        /// sharing scopes exactly to that category's contiguous deck run (17 base-deck backs,
        /// P&amp;P sheet #0 page 1), not to the whole document. String equality would call that
        /// a defect; the render says otherwise.</summary>
        private static string NormalizeLabel(string label) =>
            FoldScriptVariants(label.Trim().ToUpperInvariant());

        /// <summary>Folds Persian letter variants onto their Arabic look-alikes (same glyph in
        /// the deck's template font): yeh U+06CC→U+064A, keheh U+06A9→U+0643.</summary>
        private static string FoldScriptVariants(string s) =>
            s.Replace('ی', 'ي').Replace('ک', 'ك');

        /// <summary>Minimal RFC 4180 CSV reader (double-quote escaping, quoted newlines) — enough
        /// to read the Scenarii category columns without pulling CsvHelper's configuration into
        /// the VisualTests suite.</summary>
        private static List<List<string>> ParseSimpleCsv(string path)
        {
            var rows = new List<List<string>>();
            var cur = new List<string>();
            var field = new System.Text.StringBuilder();
            bool inQuotes = false;
            using (var reader = new StreamReader(path))
            {
                int ch;
                while ((ch = reader.Read()) >= 0)
                {
                    char c = (char)ch;
                    if (inQuotes)
                    {
                        if (c == '"')
                        {
                            if (reader.Peek() == '"') { field.Append('"'); reader.Read(); }
                            else inQuotes = false;
                        }
                        else field.Append(c);
                    }
                    else if (c == '"') inQuotes = true;
                    else if (c == ',') { cur.Add(field.ToString()); field.Clear(); }
                    else if (c == '\r') { /* swallow */ }
                    else if (c == '\n') { cur.Add(field.ToString()); field.Clear(); rows.Add(cur); cur = new(); }
                    else field.Append(c);
                }
            }
            if (field.Length > 0 || cur.Count > 0) { cur.Add(field.ToString()); rows.Add(cur); }
            return rows;
        }

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
