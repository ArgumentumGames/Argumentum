using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Http;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using Argumentum.AssetConverter.PdfAuditor;
using AutoMapper;
using ExtendedXmlSerializer.Core.Sources;
using ImageMagick;
using Spectre.Console;
using Sprache;
using Rule = Spectre.Console.Rule;


namespace Argumentum.AssetConverter
{
    public partial class WebBasedGenerator
	{

		public AssetConverterConfig AssetConverterConfig { get; set; }

		public WebBasedGeneratorConfig Config { get; set; }

		

		public async Task Run()
		{
			var tempLogSwitch = Logger.LogInfo;
			if (!Config.ShowInfoLogs)
			{
				Logger.LogInfo = false;
			}

			await using (var harvestManager = new HarvestManager() { Config = Config, AssetConverterConfig = AssetConverterConfig })
			{
				var harvestDictionary = await harvestManager.HarvestImages();

				var imageManager = new ImageFileGenerator() { Config = Config, AssetConverterConfig = AssetConverterConfig };
				var docImages = imageManager.GenerateDocumentImages(harvestDictionary);
				GenerateCardSetDocuments(docImages);
			}
			
			Logger.LogInfo = tempLogSwitch;
		}





		/// <summary>
		/// Generates PDF documents for a given card set document configuration and language.
		/// </summary>
		/// <param name="docImages">The card images to generate the documents from.</param>
		private void GenerateCardSetDocuments(ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>> docImages)
		{
			Logger.Log("Checking if QuestPdfGeneration mode is enabled.");
			if (!AssetConverterConfig.Mode.HasFlag(ConverterMode.QuestPdfGeneration))
			{
				Logger.Log("QuestPdfGeneration mode is disabled. Skipping PDF generation.");
				return;
			}
			Logger.Log("QuestPdfGeneration mode is enabled. Entering PDF generation logic.");

			Logger.LogTitle("Generating pdf documents");

			Logger.LogExplanations("In this third stage, Pdf documents are compiled from the individual image files. \nThose are essentially Print&Play documents for individual printers, professional services printing formats, and various posters");

			var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = Config.EnableParallelism?  Config.MaxDegreeOfParallelismDocuments : 1 };

			Logger.Log($"Found {docImages.Count} document configurations to process for PDF generation.");

			var pdfLock = new object();
			Parallel.ForEach(docImages, parallelOptionsDocuments, docImageList =>
			{
				try
				{
					var pdfDirectory = AssetConverterConfig.GetDocumentDirectory(docImageList.Key.language);
					var densityDirectory = docImageList.Key.document.GetDensityDirectory(pdfDirectory);

					var targetFiles = new List<(string fileName, Func<MagickImageCollection> documentImages)>();

					var documentName = CardSetLocalization.GetLocalizedFileName(docImageList.Key.document.DocumentName,
						AssetConverterConfig.LocalizationConfig.DefaultLanguage, docImageList.Key.language);
					var baseName = Path.Combine(densityDirectory, documentName);

					Logger.Log($"Start Generating pdf document {baseName}");

					lock (pdfLock)
					{
						var objPdfManager = new PdfManager();

						switch (docImageList.Key.document.DocumentFormat)
						{
							case CardDocumentFormat.FacesOnly:
								objPdfManager.GenerateFacesOnly(baseName, docImageList.Value,
									AssetConverterConfig.OverwriteExistingDocs);
								break;
							case CardDocumentFormat.AlternateFaceAndBack:
								objPdfManager.GenerateAlternateFaceAndBack(baseName, docImageList.Value,
									AssetConverterConfig.OverwriteExistingDocs);
								break;
							case CardDocumentFormat.BackFirstOneDocPerBack:
								objPdfManager.GenerateBackFirstOneDocPerBack(baseName, docImageList.Value,
									AssetConverterConfig.OverwriteExistingDocs);
								break;
							case CardDocumentFormat.PrintAndPlay:
								objPdfManager.GeneratePrintAndPlay(baseName, docImageList.Key.document,
									docImageList.Value, AssetConverterConfig.OverwriteExistingDocs);
								break;
							default:
								throw new InvalidOperationException(
									$"Document format {docImageList.Key.document.DocumentFormat} unsupported");
						}

						if (AssetConverterConfig.Mode.HasFlag(ConverterMode.PdfAuditor))
						{
							var pdfPath = $"{baseName}.pdf";
							var auditResult = global::Argumentum.AssetConverter.PdfAuditor.PdfAuditor.AuditPdf(pdfPath, docImageList.Key.document, docImageList.Value);
							
							if (auditResult.IsSuccess)
							{
								Logger.LogSuccess("PDF Audit Report:");
							}
							else
							{
								Logger.Log("ERROR: PDF Audit Report:");
							}

							foreach (var message in auditResult.Messages)
							{
								Logger.Log(message);
							}
						}
					}
				}
				catch (Exception e)
				{
					Logger.LogException(e);
				}
			});
		}




	

		

		


	}
}