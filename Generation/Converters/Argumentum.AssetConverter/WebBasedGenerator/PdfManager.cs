using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using ImageMagick;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Argumentum.AssetConverter;

public class PdfManager
{

	public Stopwatch Stopwatch { get; set; }

	private const float InchToCentimetre = 2.54f;
	private const float InchToPoints = 72;
	private float MmToPointsFactor = 0.1f / InchToCentimetre * InchToPoints;

	/// <summary>
	/// Generates a pdf file from config arguments and a list of generated images to compose the set.
	/// </summary>
	/// <param name="fileName">The result pdf file name</param>
	/// <param name="docConfig">The configuration to process</param>
	/// <param name="images">The images requested by the configuration to build the pdf document</param>
	public void GeneratePrintAndPlay(string fileName, CardSetGenerationDocument docConfig, List<CardImages> images)
	{




		var pageSizeType = typeof(PageSizes);
		var dynProp = pageSizeType.GetProperty(docConfig.PageSize, BindingFlags.Static | BindingFlags.Public);

		var pageSize = (PageSize)dynProp.GetValue(null);
		//var pageMarginMm = 7f;
		var pageMarginMm = 0f;
		//var imagePaddingMm = 2f;


		var cardWidthPoints = ((float)docConfig.CardSets[0].FrontCards.WidthMM) * MmToPointsFactor;
		var cardHeightPoints = ((float)docConfig.CardSets[0].FrontCards.HeigthMM) * MmToPointsFactor;


		var totalMarginPoints = 2 * pageMarginMm * MmToPointsFactor;
		var contentWidthPoints = pageSize.Width - totalMarginPoints;
		var contentHeightPoints = pageSize.Height - totalMarginPoints;

		var nbColumns = (int)(contentWidthPoints / cardWidthPoints);
		var nbRows = (int)(contentHeightPoints / cardHeightPoints);

		var nbCardsPerPage = nbRows * nbColumns;
		var nbPages = (int)Math.Ceiling((decimal)images.Count / (decimal)nbCardsPerPage);

		var docMetadata = new DocumentMetadata()
		{
			ApplyCaching = true,
			Author = "Argumentum",
			Creator = "Argumentum",
			Producer = "Argumentum",
			Subject = "Jeu de carte sur l'argumentation",
			Keywords = "Argumentation, rhétorique, arguments fallacieux, sophismes, éloquence",
			Title = "Argumentum Print & Play"
		};

		Document.Create(container =>
			{
				for (int pageIndex = 0; pageIndex < nbPages; pageIndex++)
				{
					var pageCards = images.Skip(pageIndex * nbCardsPerPage);
					if (pageIndex < nbPages - 1)
					{
						pageCards = pageCards.Take(nbCardsPerPage);
					}
					//else
					//{
					//    Debugger.Break();
					//}

					var pageCardsArray = pageCards.ToArray();

					//var cardArray = pageCards.ToList().ToJaggedArray(nbColumns);
					if (!docConfig.NoBack)
					{
						GenerateCardsPage(container, docConfig, pageSize, pageMarginMm, nbColumns, pageCardsArray, cardWidthPoints, cardImages => new MagickImage(cardImages.Back));
						pageCardsArray = pageCardsArray.ToJaggedArray(nbColumns).Select(row => row.Reverse().ToArray())
							.ToArray().Flatten();
					}

					GenerateCardsPage(container, docConfig, pageSize, pageMarginMm, nbColumns, pageCardsArray, cardWidthPoints, cardImages => new MagickImage(cardImages.Front));
				}


			})
			.WithMetadata(docMetadata)
			.GeneratePdf(fileName);
		Console.WriteLine($"{Stopwatch.Elapsed}: Generated pdf document {fileName}");

	}

	private static void GenerateCardsPage(IDocumentContainer container, CardSetGenerationDocument docConfig, PageSize pageSize, float pageMarginMm,
		int nbColumns, CardImages[] pageCardsArray, float cardWidthPoints, Func<CardImages, MagickImage> frontOrBack)
	{

		container.Page(page =>
		{
			page.Size(pageSize);
			page.Margin(pageMarginMm, Unit.Millimetre);
			page.PageColor(Colors.White);
			page.DefaultTextStyle(x => x.FontSize(20));

			if (!string.IsNullOrEmpty(docConfig.Header))
			{
				var imagePath = Path.Combine(Environment.CurrentDirectory, docConfig.Header);
				var currentContainer = page.Header();
				currentContainer = currentContainer.AlignCenter();
				currentContainer = currentContainer.Height(pageSize.Height / 40);
				currentContainer = currentContainer.Padding(pageSize.Width / 150);
				currentContainer.Image(imagePath, ImageScaling.FitHeight);

			}


			page.Content()
				.Padding(0)
				.AlignCenter()
				.AlignTop()
				.Column(c =>
				{

					c.Item()
						.AlignCenter()
						.AlignTop()
						.Grid(g =>
						{
							g.AlignCenter();
							g.Spacing(0, Unit.Millimetre);
							g.Columns(nbColumns);
							for (int cardIndex = 0; cardIndex < pageCardsArray.Length; cardIndex++)
							{
								var gridCell = g.Item()
									//.Border(0.2f, Unit.Millimetre)
									.AlignCenter()
									.AlignMiddle()
									.Width(cardWidthPoints);
								var pageCard = pageCardsArray[cardIndex];
								if (pageCard != null)
								{
									MagickImage toPrint = frontOrBack(pageCard);

									PrintMagickImageIntoGridCell(toPrint, gridCell);

								}
							}
						});
				});


			//page.Footer()
			//    .AlignCenter()
			//    .Text(x =>
			//    {
			//        x.Span("Page ");
			//        x.CurrentPageNumber();
			//    });
		});
	}

	/// <summary>
	/// uses one of the IContainer.Image extension method overloads to add MagickImage object to the corresponding grid cell
	/// </summary>
	/// <param name="toPrint">the MagickImage object to render</param>
	/// <param name="gridCell">the IContainer grid cell in which to add the target image</param>
	private static void PrintMagickImageIntoGridCell(MagickImage toPrint, IContainer gridCell)
	{
		if (!string.IsNullOrEmpty(toPrint.FileName))
		{
			gridCell.Image(toPrint.FileName);
		}
		else
		{
			using (var memStream = new MemoryStream())
			{
				toPrint.Write(memStream);
				gridCell.Image(memStream.ToArray());
			}
		}
	}


	public void GeneratePdfsFromImages(List<(string fileName, MagickImageCollection documentImages)> targetFiles)
	{
		foreach (var targetFile in targetFiles)
		{
			targetFile.documentImages.Write(targetFile.fileName);
			Console.WriteLine($"{Stopwatch.Elapsed}: Generated pdf document {targetFile.fileName}");
		}
	}


	//private List<ImageMagick.MagickImage> GenerateImages(string exampleName)

}