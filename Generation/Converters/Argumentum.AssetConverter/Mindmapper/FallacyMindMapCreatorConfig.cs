using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Argumentum.AssetConverter.Entities;
using AutoMapper;
using CsvHelper;
using ImageMagick;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;


namespace Argumentum.AssetConverter.Mindmapper
{
	public class FallacyMindMapCreatorConfig : ParallelFallacyDocumentCreatorConfigBase<FallacyMindMapDocumentConfig>
	{

		public override string GetLogTitle()
		{
			return "Generating Fallacy Freemind/Freeplane, SVG & Html Mindmaps";
		}

		public override string GetLogMessage()
		{
			return "In this last stage, Freemind mindmaps are generated from the same dataset that was used for cards pdfs. \nOptional Manual intervention is required for SVG processing. Once a Freemind mindmap is generated, you get prompted to use the free tool to generate an SVG file, which is then further processed for HTML generation. \nNote that Html files with the svg file embedded externally will only display properly when hosted behind a URL, whereas html documents with svg embedded inside will also display properly when opened locally";
		}

		public override List<FallacyMindMapDocumentConfig> DocumentConfigs { get; set; } = new List<FallacyMindMapDocumentConfig>(new[]
			{
				new FallacyMindMapDocumentConfig()
				{
					Enabled = true,
					DocumentName = "Fallacies_fr.mm",
					// #1181: draw the transverse cross-links (8 corpus verbs) on every language map
					CrossLinks = CrossLink.All,
					DataSet = KnownDataSets.FallaciesTaxonomy,
					Translations = new List<(string sourceLang, string destLang)>(new[]
					{
						("fr", "en"),
						("fr", "ru"),
						("fr", "pt"),
						("fr", "es"),
						("fr", "ar"),
						("fr", "fa"),
						("fr", "zh")
					}),
					ImageFormat = MagickFormat.Png,
					TargetDensity = 0,
					KeepOriginalSVG = true,
					SVGMaps = new List<SVGFreemindMap>(new[]
					{
						new SVGFreemindMap()
						{
							Enabled = true,
							DocumentName = "links.svg",
							SvgWidth = "200vh",
							SvgHeight = "450vh",
							SvgViewBox = "0 0 8500 20000",
							WrapNodeByLink = true,
							SetSVGNodeAttributes = false,
							RemoveImages = true,
							// #1248: this tall variant is the study view of the cross-link layer
							HighContrastCrossLinks = true
						},
						new SVGFreemindMap()
						{
							Enabled = true,
							DocumentName = "content.svg",
							SvgWidth = "96vw",
							SvgHeight = "93vh",
							SvgViewBox = "0 0 8500 20000",
							WrapNodeByLink = false,
							SetSVGNodeAttributes = true,
							RemoveImages = true,
							HtmlWrappers = new List<DocumentConfig>(new[]
							{
								new DocumentConfig()
								{
									DocumentName	= "Fallacies_[LANGUAGE].html",
									TemplatePathRelease =
										"https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Mindmaps/included.html",
									TemplatePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Mindmaps\included.html"
								},
								new DocumentConfig()
								{
									DocumentName    = "Fallacies_[LANGUAGE]_ext.html",
									TemplatePathRelease =
										"https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Mindmaps/external.html",
									TemplatePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Mindmaps\external.html"
								},

							})


						},
					})
				},
				new FallacyMindMapDocumentConfig()
				{
					Enabled = true,
					DocumentName = "Argumentum_Fallacies_MindMap_cards_fr.mm",
					CrossLinks = CrossLink.All,
					Format = MindMapFormat.Freemind,
					DataSet = KnownDataSets.FallaciesTaxonomy,
					InsertCardsThumbnails = true,
					ThumbnailsCardSetName = KnownCardSets.FallaciesWebThumbnails,
					//Translations = new List<(string sourceLang, string destLang)>(new[]
					//{
					//	("fr", "en"),
					//	("fr", "ru"),
					//	("fr", "pt")
					//}),
					ImageFormat = MagickFormat.Png,
					TargetDensity = 0,
					// #1253 §1: this document is the only one embedding the card thumbnails, and it was the
					// only one with no viewer at all — its single variant declared no viewBox and carried no
					// HTML wrapper, so the 176 cards shipped as a 5.3 MB raw Batik export nothing referenced.
					// The variants below mirror Fallacies_fr / Argumentum_Virtues_MindMap: a study links.svg
					// plus a content.svg carrying the wrappers. RemoveImages strips the FreeMind node icons
					// but deliberately spares width="60" elements (FallacyMindMapDocumentConfig.RemoveImages),
					// which is exactly the card box declared by CardExpression — so the cards survive it.
					SVGMaps = new List<SVGFreemindMap>(new[]
					{
						new SVGFreemindMap()
						{
							Enabled = true,
							DocumentName = "links.svg",
							SvgWidth = "200vh",
							SvgHeight = "450vh",
							SvgViewBox = "0 0 8500 20500",
							WrapNodeByLink = true,
							RemoveImages = true,
							SetSVGNodeAttributes = false,
						},
						new SVGFreemindMap()
						{
							Enabled = true,
							DocumentName = "content.svg",
							SvgWidth = "96vw",
							SvgHeight = "93vh",
							// Superset of the measured raw export (8293 x 20229). preserveAspectRatio defaults
							// to meet, so rounding up only adds margin — undersizing would clip.
							SvgViewBox = "0 0 8500 20500",
							WrapNodeByLink = false,
							SetSVGNodeAttributes = true,
							RemoveImages = true,
							HtmlWrappers = new List<DocumentConfig>(new[]
							{
								new DocumentConfig()
								{
									DocumentName    = "Fallacies_cards_[LANGUAGE].html",
									TemplatePathRelease =
										"https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Mindmaps/included.html",
									TemplatePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Mindmaps\included.html"
								},
								new DocumentConfig()
								{
									DocumentName    = "Fallacies_cards_[LANGUAGE]_ext.html",
									TemplatePathRelease =
										"https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Mindmaps/external.html",
									TemplatePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Mindmaps\external.html"
								},
							})
						},
					})
				}
			});



		//public FallacyMindMapCreatorConfig()
		//{
		//    var newConfig = new List<MindMapDocumentConfig>();
		//    newConfig.Add(new MindMapDocumentConfig(){DocumentName = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm" });

		//    var frConfigCards = new MindMapDocumentConfig() { DocumentName = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2_cards.mm" };
		//    frConfigCards.InsertCardsThumbnails = true;

		//    newConfig.Add(frConfigCards);

		//    var enConfig = new MindMapDocumentConfig(){DocumentName = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm" };
		//    enConfig.DocumentName = enConfig.DocumentName.Replace("Fr", "En");
		//    enConfig.DescriptionExpression = enConfig.DescriptionExpression.Replace("Fr", "En");
		//    enConfig.TitleExpression = enConfig.TitleExpression.Replace("Fr", "En");
		//    enConfig.ExampleExpression = enConfig.ExampleExpression.Replace("Fr", "En");
		//    enConfig.LinkExpression = enConfig.LinkExpression.Replace("Fr", "En");
		//    newConfig.Add(enConfig);

		//    var enConfigCards = new MindMapDocumentConfig(){DocumentName = @"..\..\..\Data\Mindmap\Argumentum_Fallacies_MindMap_Fr_2.mm" };
		//    enConfigCards.InsertCardsThumbnails = true;
		//    enConfigCards.DocumentName = enConfig.DocumentName.Replace(".mm", "_cards.mm");
		//    enConfigCards.DescriptionExpression = enConfig.DescriptionExpression;
		//    enConfigCards.TitleExpression = enConfig.TitleExpression;
		//    enConfigCards.ExampleExpression = enConfig.ExampleExpression;
		//    enConfigCards.LinkExpression = enConfig.LinkExpression;
		//    enConfigCards.CardExpression = enConfig.CardExpression.Replace("Fr", "En");
		//    newConfig.Add(enConfigCards);

		//    MindMapDocuments = newConfig;
		//}


		//public void Run(string[] args)
		//{

		//    foreach (var config in this.MindMapDocuments)
		//    {

		//        var fallacies = Fallacy.Load(config.DataSet);
		//       config.GenerateMindMapFile( fallacies, null, "", "").GetAwaiter().GetResult();

		//    }
		//    Logger.LogTitle($"Generation finished, press any key to close");
		//    Console.ReadKey();

		//}
	}

}