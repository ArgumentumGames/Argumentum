using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ExtendedXmlSerializer.Core.Sources;
using ImageMagick;
using Sprache;


namespace Argumentum.AssetConverter
{
	public class WebBasedGenerator
	{

		public static Stopwatch StopWatch = Stopwatch.StartNew();

		public WebBasedGeneratorConfig Config { get; set; }

		public WebBasedGenerator(WebBasedGeneratorConfig config, Stopwatch objSw)
		{
			Config = config;
			StopWatch = objSw;

		}


		/// <summary>
		/// Runs the program to generate card set documents from harvested images.
		/// </summary>
		public void Run()
		{
			var harvestManager = new HarvestManager() { Stopwatch = StopWatch, Config = Config };
			//var harvestDictionary = HarvestImages();
			var harvestDictionary = Task.Run(async () => await harvestManager.HarvestImages()).Result;

			var imageManager = new ImageFileGenerator(Config ,StopWatch);

			var docImages = imageManager.GenerateDocumentImages(harvestDictionary);

			GenerateCardSetDocuments(docImages);

		}
		
		/// <summary>
		/// Generates PDF documents from a ConcurrentDictionary of CardSetGenerationDocument and language, and a list of CardImages.
		/// </summary>
		private void GenerateCardSetDocuments(ConcurrentDictionary<(CardSetGenerationDocument document, string language), List<CardImages>> docImages)
		{
			Console.WriteLine($"{StopWatch.Elapsed}: Generation pdf documents");


			foreach (var docImageList in docImages)
			{
				var pdfDirectory = Config.GetPdfsDirectory(docImageList.Key.language);
				var densityDirectory = Path.Combine(pdfDirectory, $@".\density-{docImageList.Key.document.TargetDensity}\");
				if (!Directory.Exists(densityDirectory))
				{
					Directory.CreateDirectory(densityDirectory);
				}

				var targetFiles = new List<(string fileName, MagickImageCollection documentImages)>();
				MagickImageCollection collec;
				var documentName = LocalizationConfig.GetLocalizedFileName(docImageList.Key.document.DocumentName,
					Config.LocalizationConfig.DefaultLanguage, docImageList.Key.language);
				var baseName = Path.Combine(densityDirectory, documentName);

				var objPdfManager = new PdfManager() { Stopwatch = StopWatch };
				switch (docImageList.Key.document.DocumentFormat)
				{
					case CardDocumentFormat.AlternateFaceAndBack:
						collec = new MagickImageCollection(docImageList.Value.SelectMany(s =>
						{
							return new[] { new MagickImage(s.Front), new MagickImage(s.Back) };
						}));

						targetFiles.Add((baseName, collec));
						objPdfManager.GeneratePdfsFromImages(targetFiles);
						break;
					case CardDocumentFormat.BackFirstOneDocPerBack:

						var indexInsert = baseName.LastIndexOf('.');
						var cardsPerBack = docImageList.Value.GroupBy(card => card.Back).ToArray();
						for (int backIndex = 0; backIndex < cardsPerBack.Count(); backIndex++)
						{
							var frontsAndBack = cardsPerBack[backIndex];
							var backThenFronts = new[] { new MagickImage(frontsAndBack.Key) }.Concat(frontsAndBack.Select(card => new MagickImage(card.Front)));
							collec = new MagickImageCollection(backThenFronts);
							var newName = $"{baseName.Substring(0, indexInsert)}-{backIndex + 1}{baseName.Substring(indexInsert)}";
							targetFiles.Add((newName, collec));
						}
						objPdfManager.GeneratePdfsFromImages(targetFiles);
						break;
					case CardDocumentFormat.PrintAndPlay:
						objPdfManager.GeneratePrintAndPlay(baseName, docImageList.Key.document, docImageList.Value);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

			}
		}



	}
}


