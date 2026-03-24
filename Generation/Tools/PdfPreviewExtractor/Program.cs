using System;
using System.IO;
using Docnet.Core;
using Docnet.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PdfPreviewExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputDir = "outputs";
            string outputDir = null;

            // Simple manual parsing
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--input-dir" && i + 1 < args.Length)
                {
                    inputDir = args[i + 1];
                    i++;
                }
                else if (args[i] == "--output-dir" && i + 1 < args.Length)
                {
                    outputDir = args[i + 1];
                    i++;
                }
            }
            
            // Handle relative paths correctly from execution context
            if (!Path.IsPathRooted(inputDir))
            {
                inputDir = Path.GetFullPath(inputDir);
            }

            Run(inputDir, outputDir);
        }

        static void Run(string inputDir, string outputDir)
        {
            if (!Directory.Exists(inputDir))
            {
                Console.WriteLine($"Error: Input directory '{inputDir}' does not exist.");
                // Try relative to current dir just in case
                var currentDirInput = Path.Combine(Directory.GetCurrentDirectory(), inputDir);
                 if (Directory.Exists(currentDirInput))
                 {
                     inputDir = currentDirInput;
                     Console.WriteLine($"Found input directory at: {inputDir}");
                 }
                 else
                 {
                    return;
                 }
            }

            if (string.IsNullOrEmpty(outputDir))
            {
                outputDir = Path.Combine(inputDir, "_Previews");
            }
            
            if (!Path.IsPathRooted(outputDir))
            {
                outputDir = Path.GetFullPath(outputDir);
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var pdfFiles = Directory.GetFiles(inputDir, "*.pdf");

            if (pdfFiles.Length == 0)
            {
                 Console.WriteLine($"No PDF files found in '{inputDir}'.");
                 return;
            }
            
            Console.WriteLine($"Found {pdfFiles.Length} PDF files in '{inputDir}'. Processing...");
            Console.WriteLine($"Output Directory: {outputDir}");

            foreach (var pdfPath in pdfFiles)
            {
                try
                {
                    ProcessPdf(pdfPath, outputDir);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing '{Path.GetFileName(pdfPath)}': {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                }
            }
             Console.WriteLine("Done.");
        }

        static void ProcessPdf(string pdfPath, string outputDir)
        {
             var fileName = Path.GetFileNameWithoutExtension(pdfPath);
             Console.WriteLine($"Processing: {fileName}");

             using (var docReader = DocLib.Instance.GetDocReader(pdfPath, new PageDimensions(1080 * 2, 1920 * 2))) 
             {
                 var pageCount = docReader.GetPageCount();
                 int[] pagesToExtract = { 1, 2, 10 };

                 foreach(var pageNum in pagesToExtract)
                 {
                     int pageIndex = pageNum - 1; // 0-based index
                     bool isLastPage = false;
                     
                     // Special handling for Page 10 request
                     if (pageNum == 10)
                     {
                         if (pageCount < 10)
                         {
                             pageIndex = pageCount - 1; // Use Last page
                             isLastPage = true;
                             Console.WriteLine($"  - Page 10 requested, but doc has {pageCount} pages. Using last page (Page {pageCount}).");
                         }
                     }
                     else if (pageIndex >= pageCount)
                     {
                         Console.WriteLine($"  - Page {pageNum} skipped (doc has only {pageCount} pages).");
                         continue;
                     }

                     using (var pageReader = docReader.GetPageReader(pageIndex))
                     {
                         var width = pageReader.GetPageWidth();
                         var height = pageReader.GetPageHeight();
                         var rawBytes = pageReader.GetImage(); // Raw BGRA bytes

                         if (rawBytes == null || rawBytes.Length == 0)
                         {
                             Console.WriteLine($"    Warning: Extracted raw bytes are empty for page {pageIndex + 1}.");
                             continue;
                         }

                         // Load BGRA pixel data
                         using (var image = Image.LoadPixelData<Bgra32>(rawBytes, width, height))
                         {
                             string suffix = isLastPage ? $"_Page{pageCount}_Last" : $"_Page{pageNum}";
                             // If page 10 was requested and it IS page 10 (and not last because <10), name it Page10
                             if (pageNum == 10 && !isLastPage) suffix = "_Page10";

                             string outPath = Path.Combine(outputDir, $"{fileName}{suffix}.png");
                             
                             image.SaveAsPng(outPath);
                             Console.WriteLine($"  -> Saved: {Path.GetFileName(outPath)}");
                         }
                     }
                 }
             }
        }
    }
}
