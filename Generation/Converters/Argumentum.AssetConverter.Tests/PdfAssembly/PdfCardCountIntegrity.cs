using System;
using System.Collections.Generic;
using System.Linq;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    /// <summary>
    /// Pure page-emission models for the #1187 PDF-level card-count organ. This class mirrors,
    /// without rendering, exactly what each <c>CardDocumentFormat</c> emits page-wise in
    /// <c>PdfManager</c>/<c>PrintAndPlayDocument</c>:
    /// <list type="bullet">
    /// <item><description><c>AlternateFaceAndBack</c> — 2 pages per card instance that HAS a back
    /// (back page immediately before its front, <c>OrderImagesForAlternateFaceAndBack</c>), 1 page
    /// per backless card (Rules).</description></item>
    /// <item><description><c>FacesOnly</c> — 1 page per card instance.</description></item>
    /// <item><description><c>PrintAndPlay</c> — the deck is chunked into grid sheets whose capacity
    /// comes from <see cref="PrintAndPlayDocument.ComputePageGeometry"/> (the documented pure
    /// function — truncation, not rounding); each chunk emits its back sheet only when at least one
    /// card on it has a back, then always its front sheet (<c>PrintAndPlayDocument.Compose</c>).</description></item>
    /// <item><description><c>BackFirstOneDocPerBack</c> — emits one PDF per distinct back art; its
    /// page count depends on how back images group, which this organ does not model: derivation
    /// <b>throws</b> rather than guessing. No enabled boîte document uses it today.</description></item>
    /// </list>
    /// Everything here is deterministic and I/O-free so the CI suite can prove the checker goes
    /// RED on an amputated witness before it is ever trusted green (#1046 lesson: a guard never
    /// seen red is a no-op).
    /// </summary>
    internal static class PdfCardCountIntegrity
    {
        /// <summary>
        /// Compares derived expected page counts against actually produced page counts. A record
        /// whose actual differs from expected by even one page is a failure — for per-card formats
        /// one page IS one card side, and for grid formats one sheet IS 2×capacity page slots, so
        /// exact equality is precisely the two-sided bound of the #1187 DoD (lower bound: no
        /// missing sheet; upper bound: no extra sheet). The failure names the violated bound by
        /// the sign of the delta.
        /// </summary>
        /// <param name="records">(document, language, expectedPages, actualPages, breakdown) tuples.
        /// <paramref name="records"/>'s <c>Breakdown</c> is the per-CardSet derivation trace and is
        /// included verbatim in the failure so the number in front of the reader is never a bare
        /// integer with no provenance (#1187 root cause: three diverging numbers for one object).</param>
        public static IReadOnlyList<string> CheckPageCounts(
            IEnumerable<(string Doc, string Lang, int ExpectedPages, int ActualPages, string Breakdown)> records)
        {
            var failures = new List<string>();
            foreach (var r in records)
            {
                if (r.ActualPages == r.ExpectedPages) continue;
                var bound = r.ActualPages < r.ExpectedPages
                    ? $"LOWER BOUND violated — {r.ExpectedPages - r.ActualPages} page(s) missing (missing sheet(s)/card(s))"
                    : $"UPPER BOUND violated — {r.ActualPages - r.ExpectedPages} page(s) extra (extra sheet(s))";
                failures.Add(
                    $"{r.Lang}/{r.Doc}: produced {r.ActualPages} page(s), derived expectation {r.ExpectedPages}. {bound}. " +
                    $"Derivation: {r.Breakdown} If the CSV or the document config changed since the last regen, the PDF is stale — regenerate before quoting any card count (#1204 lesson).");
            }
            return failures;
        }

        /// <summary>AlternateFaceAndBack emission: 2 pages per backed instance, 1 per backless.</summary>
        public static int ExpectedPagesAlternateFaceAndBack(IReadOnlyList<bool> instanceHasBack)
            => instanceHasBack.Count(b => b) * 2 + instanceHasBack.Count(b => !b);

        /// <summary>FacesOnly emission: 1 page per instance.</summary>
        public static int ExpectedPagesFacesOnly(int cardInstances) => cardInstances;

        /// <summary>
        /// PrintAndPlay emission, mirroring <see cref="PrintAndPlayDocument.Compose"/>: the geometry
        /// (columns/rows/capacity/sheet count) comes from the renderer's own pure function — never
        /// recomputed here — and each sheet emits a back page only when at least one of its cards
        /// has a back (or never, when <paramref name="noBack"/>), then always a front page.
        /// </summary>
        public static int ExpectedPagesPrintAndPlay(
            float pageWidthPoints, float pageHeightPoints,
            float cardWidthPoints, float cardHeightPoints,
            float totalMarginPoints, bool hasHeader, int configuredNbColumns,
            IReadOnlyList<bool> instanceHasBack, bool noBack)
        {
            var geometry = PrintAndPlayDocument.ComputePageGeometry(
                pageWidthPoints, pageHeightPoints, cardWidthPoints, cardHeightPoints,
                totalMarginPoints, hasHeader, configuredNbColumns, instanceHasBack.Count);

            int pages = 0;
            for (int chunk = 0; chunk < geometry.NbPages; chunk++)
            {
                var chunkHasAnyBack = instanceHasBack
                    .Skip(chunk * geometry.NbCardsPerPage)
                    .Take(geometry.NbCardsPerPage)
                    .Any(hasBack => hasBack);
                if (!noBack && chunkHasAnyBack) pages++;
                pages++;
            }
            return pages;
        }
    }
}
