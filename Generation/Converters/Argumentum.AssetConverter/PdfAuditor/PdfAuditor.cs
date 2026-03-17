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
            var pageSize = PageSizes.A4; // Assuming A4 for calculation, needs to be dynamic if possible.
            var cardWidthPoints = ((float)docConfig.CardSets[0].FrontCards.WidthMM) * (0.1f / 2.54f * 72);
            var cardHeightPoints = ((float)docConfig.CardSets[0].FrontCards.HeigthMM) * (0.1f / 2.54f * 72);
            int nbColumns = docConfig.NbColumns > 0 ? docConfig.NbColumns : (int)(pageSize.Width / cardWidthPoints);
            var nbRows = (int)(pageSize.Height / cardHeightPoints);
            var nbCardsPerPage = nbRows * nbColumns;

            var pages = images.Chunk(nbCardsPerPage);
            var orderedPaths = new List<string>();

            foreach (var pageCards in pages)
            {
                // Back page logic - must match PdfManager exactly
                if (!docConfig.NoBack)
                {
                    var backCardsArray = pageCards.ToJaggedArray(nbColumns).Select(row => row.Reverse().ToArray()).ToArray().Flatten();
                    orderedPaths.AddRange(backCardsArray.Select(c => c?.Back));
                }

                // Front page logic
                orderedPaths.AddRange(pageCards.Select(c => c.Front));
            }
            return orderedPaths.Where(p => !string.IsNullOrEmpty(p) && File.Exists(p)).ToList();
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