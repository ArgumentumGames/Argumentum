using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Argumentum.AssetConverter;
using QuestPDF.Helpers;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Argumentum.AssetConverter.PdfAuditor
{
    public static class PdfAuditor
    {
        public static AuditResult AuditPdf(string pdfPath, CardSetDocumentConfig docConfig, List<CardImages> images)
        {
            var result = new AuditResult { IsSuccess = true };
            result.Messages.Add($"Starting PDF audit for: {pdfPath}");
         
            try
            {
            	if (!File.Exists(pdfPath))
            	{
            		result.IsSuccess = false;
            		result.Messages.Add($"PDF file not found at: {pdfPath}");
            		return result;
            	}
         
            	var isBooklet = docConfig.CardSets.Any(cs => cs.CardSetName == KnownCardSets.RulesPrintAndPlay);
            	if (isBooklet)
            	{
            		result.Messages.Add("Skipping PDF audit for booklet type as it is not yet implemented.");
            		return result;
            	}
         
            	var extractedImageHashes = new List<string>();
            	using (var pdf = PdfDocument.Open(pdfPath))
            	{
            		foreach (var page in pdf.GetPages())
            		{
            			foreach (var image in page.GetImages())
            			{
            				extractedImageHashes.Add(ComputeRawBytesHash(image.RawBytes.ToArray()));
            			}
            		}
            	}
         
            	var expectedImagePaths = GetExpectedImageOrder(docConfig, images);
            	if (extractedImageHashes.Count != expectedImagePaths.Count)
            	{
            		result.IsSuccess = false;
            		result.Messages.Add($"Image count mismatch. Expected {expectedImagePaths.Count} images, but found {extractedImageHashes.Count} in the PDF.");
            		return result;
            	}
         
            	for (int i = 0; i < expectedImagePaths.Count; i++)
            	{
            		var expectedPath = expectedImagePaths[i];
            		var expectedHash = ComputeFileHash(expectedPath);
            		var extractedHash = extractedImageHashes[i];
         
            		if (expectedHash != extractedHash)
            		{
            			result.IsSuccess = false;
            			result.Messages.Add($"Mismatch at image {i + 1}: Expected hash {expectedHash} for '{Path.GetFileName(expectedPath)}', but got {extractedHash} from PDF.");
            		}
            	}
         
            	if (result.IsSuccess)
            	{
            		result.Messages.Add("PDF audit completed successfully. All image hashes match.");
            	}
            }
            catch (Exception ex)
            {
            	result.IsSuccess = false;
            	result.Messages.Add($"An unexpected error occurred during PDF audit: {ex.Message}");
            }
            
            return result;
        }

        private static List<string> GetExpectedImageOrder(CardSetDocumentConfig docConfig, List<CardImages> images)
        {
            var cardWidthPoints = ((float)docConfig.CardSets[0].FrontCards.WidthMM) * (0.1f / 2.54f * 72);
            var cardHeightPoints = ((float)docConfig.CardSets[0].FrontCards.HeigthMM) * (0.1f / 2.54f * 72);

            // ✅ Page-grid geometry (nbColumns/nbRows/nbCardsPerPage) extracted output-neutral into
            // ComputeAuditPageGeometry so this fragile layout contract is unit-testable without a render.
            var geometry = ComputeAuditPageGeometry(cardWidthPoints, cardHeightPoints, docConfig.NbColumns);
            int nbColumns = geometry.NbColumns;
            var nbCardsPerPage = geometry.NbCardsPerPage;

            // Pure sequence (backs-row-reversed-then-fronts, page by page), then the File.Exists filter
            // is applied at the boundary only — so the ordering contract is unit-testable in isolation.
            return BuildExpectedImageOrder(images, nbCardsPerPage, nbColumns, docConfig.NoBack)
                .Where(p => !string.IsNullOrEmpty(p) && File.Exists(p))
                .ToList();
        }

        /// <summary>
        /// Page-grid geometry the audit assumes when chunking the deck into page-sized groups for
        /// <see cref="BuildExpectedImageOrder"/>. Pure &amp; deterministic — no I/O, no PDF render.
        /// </summary>
        public readonly struct AuditPageGeometry
        {
            /// <summary>Grid columns per sheet. The configured value when &gt; 0, else floor(pageWidth / cardWidth).</summary>
            public readonly int NbColumns;
            /// <summary>Grid rows per sheet: floor(pageHeight / cardHeight).</summary>
            public readonly int NbRows;
            /// <summary>Cards that fit on one sheet: NbRows × NbColumns.</summary>
            public readonly int NbCardsPerPage;

            public AuditPageGeometry(int nbColumns, int nbRows, int nbCardsPerPage)
            {
                NbColumns = nbColumns;
                NbRows = nbRows;
                NbCardsPerPage = nbCardsPerPage;
            }
        }

        /// <summary>
        /// Derives the page-grid geometry the audit uses to chunk the deck into page-sized groups, from
        /// the resolved card size and configured column count. Pure &amp; deterministic — no I/O, no PDF
        /// render. Extracted output-neutral from <see cref="GetExpectedImageOrder"/> (the inline
        /// arithmetic previously inlined there) so the audit's layout contract is unit-testable.
        /// </summary>
        /// <remarks>
        /// <b>KNOWN DIVERGENCE FROM THE RENDERER — latent bug, flagged for a separate behavior-change
        /// PR, NOT fixed here (output-neutral).</b> The audit computes its geometry against a
        /// <b>hardcoded <see cref="PageSizes.A4"/></b>, while the renderer
        /// (<see cref="PrintAndPlayDocument.ComputePageGeometry"/>) resolves the actual
        /// <c>docConfig.PageSize</c> via reflection. The audit also does NOT subtract the page margin
        /// (<c>totalMarginPoints</c>) nor the header band (<c>pageHeight / 10</c> when
        /// <c>docConfig.Header</c> is set) that the renderer subtracts from the content area. As a
        /// result the audit's <c>NbCardsPerPage</c> is systematically LARGER than the renderer's whenever
        /// the document is not plain A4-without-header — chunking the deck into pages of the wrong
        /// capacity and desynchronizing the expected image order from the actual render, which silently
        /// produces false audit mismatches (or false passes). Pinned as-is here so the current contract
        /// is observable and the divergence is fixable in isolation; a fix that routes this through
        /// <see cref="PrintAndPlayDocument.ComputePageGeometry"/> belongs in its own PR.
        /// </remarks>
        public static AuditPageGeometry ComputeAuditPageGeometry(
            float cardWidthPoints, float cardHeightPoints, int configuredNbColumns)
        {
            var pageSize = PageSizes.A4; // Assuming A4 for calculation, needs to be dynamic if possible.
            int nbColumns = configuredNbColumns > 0
                ? configuredNbColumns
                : (int)(pageSize.Width / cardWidthPoints);
            int nbRows = (int)(pageSize.Height / cardHeightPoints);
            int nbCardsPerPage = nbRows * nbColumns;

            return new AuditPageGeometry(nbColumns, nbRows, nbCardsPerPage);
        }

        /// <summary>
        /// Produces the expected image-path sequence the audit compares a rendered recto-verso PDF
        /// against: for each page-sized chunk of cards, the BACKS come first (per-row reversed, via
        /// <see cref="PrintAndPlayDocument.ReorderBacksForRectoVerso{T}"/>) then the FRONTS in natural
        /// order. Pure &amp; deterministic — no file I/O, no PDF render.
        ///
        /// Extracted output-neutral from <see cref="GetExpectedImageOrder"/> so the audit's ordering
        /// contract is unit-testable. The per-row reversal shares the EXACT same method the renderer
        /// (<see cref="PrintAndPlayDocument.Compose"/>) uses — pinned by
        /// <c>PrintAndPlayRectoVersoContractTests</c> — so the audit's expected order can never drift
        /// from the renderer's actual order. Previously this was an inline duplicate
        /// (<c>ToJaggedArray/Reverse/Flatten</c>) that "must match PdfManager exactly" by convention;
        /// a change to the renderer's reversal would have silently desynchronized the audit, producing
        /// false audit failures (or worse, false passes) with no signal beyond the PDF render.
        /// </summary>
        /// <param name="images">All card images of the deck, in face order.</param>
        /// <param name="nbCardsPerPage">Page grid capacity (rows × columns).</param>
        /// <param name="nbColumns">Grid column count, driving the per-row back reversal.</param>
        /// <param name="noBack">When true, backs are omitted (faces ship alone).</param>
        public static IEnumerable<string> BuildExpectedImageOrder(
            IEnumerable<CardImages> images, int nbCardsPerPage, int nbColumns, bool noBack)
        {
            foreach (var pageCards in images.Chunk(nbCardsPerPage))
            {
                if (!noBack)
                {
                    var backCardsArray = PrintAndPlayDocument.ReorderBacksForRectoVerso(pageCards, nbColumns);
                    foreach (var back in backCardsArray)
                        yield return back?.Back;
                }
                foreach (var card in pageCards)
                    yield return card.Front;
            }
        }

        private static string ComputeFileHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        
        private static string ComputeRawBytesHash(byte[] bytes)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

    }
}