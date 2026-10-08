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
							HtmlWrapperTitles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
							{
								// #457 T4b: <title> = this map's own central topic (already localized in the sibling
								// content.svg) + the untranslated brand suffix. Not retyped: derived from the SVG.
								["fr"] = "Argument fallacieux \u2014 Argumentum",
								["en"] = "Fallacy \u2014 Argumentum",
								["ru"] = "\u0421\u043E\u0444\u0438\u0437\u043C\u044B (\u043B\u043E\u0436\u043D\u044B\u0435 \u0430\u0440\u0433\u0443\u043C\u0435\u043D\u0442\u044B) \u2014 Argumentum",
								["pt"] = "Fal\u00E1cia \u2014 Argumentum",
								["es"] = "Falacia \u2014 Argumentum",
								["ar"] = "\u0627\u0644\u0633\u0641\u0633\u0637\u0629 \u2014 Argumentum",
								["fa"] = "\u0628\u062D\u062B \u0648 \u062C\u062F\u0644 \u0633\u0641\u0633\u0637\u0647\u200C\u0622\u0645\u06CC\u0632 \u2014 Argumentum",
								["zh"] = "\u8C2C\u8BBA \u2014 Argumentum",
							},
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
							HtmlWrapperTitles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
							{
								// #457 T4b: <title> = this map's own central topic (already localized in the sibling
								// content.svg) + the untranslated brand suffix. Not retyped: derived from the SVG.
								["fr"] = "Argument fallacieux \u2014 Argumentum",
							},
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

	}

}