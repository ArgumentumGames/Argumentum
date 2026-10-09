using System;
using System.Collections.Generic;
using Argumentum.AssetConverter.Entities;
using ImageMagick;

namespace Argumentum.AssetConverter.Mindmapper
{
    public class VirtueMindMapCreatorConfig : ParallelVirtueDocumentCreatorConfigBase<VirtueMindMapDocumentConfig>
    {
        public override string GetLogTitle()
        {
            return "Generating Virtue Freemind/Freeplane, SVG & Html Mindmaps";
        }

        public override string GetLogMessage()
        {
            return "Generating mindmaps for virtues.";
        }

        public override List<VirtueMindMapDocumentConfig> DocumentConfigs { get; set; } = new List<VirtueMindMapDocumentConfig>(new[]
        {
            new VirtueMindMapDocumentConfig()
            {
                Enabled = true,
                DocumentName = "Argumentum_Virtues_MindMap_fr.mm",
                DataSet = KnownDataSets.VirtuesTaxonomy,
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
                KeepOriginalSVG = false,
                NbBranchesRight = 4,
                SVGMaps = new List<SVGFreemindMap>(new[]
                {
                    new SVGFreemindMap()
                    {
                        Enabled = true,
                        DocumentName = "links.svg",
                        WrapNodeByLink = true,
                        SetSVGNodeAttributes = false,
                        RemoveImages = true
                    },
                    new SVGFreemindMap()
                    {
                        Enabled = true,
                        DocumentName = "content.svg",
                        SvgViewBox = "0 0 6625 5807",
                        SvgWidth = "96vw",
                        SvgHeight = "93vh",
                        WrapNodeByLink = false,
                        SetSVGNodeAttributes = true,
                        RemoveImages = true,
                        HtmlWrapperTitles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                        {
                            // #457 T4b: <title> = this map's own central topic (already localized in the sibling
                            // content.svg) + the untranslated brand suffix. Not retyped: derived from the SVG.
                            ["fr"] = "Argument valable \u2014 Argumentum",
                            ["en"] = "Valid argument \u2014 Argumentum",
                            ["ru"] = "\u041E\u0431\u043E\u0441\u043D\u043E\u0432\u0430\u043D\u043D\u044B\u0439 \u0430\u0440\u0433\u0443\u043C\u0435\u043D\u0442 \u2014 Argumentum",
                            ["pt"] = "Argumento v\u00E1lido \u2014 Argumentum",
                            ["es"] = "Argumento v\u00E1lido \u2014 Argumentum",
                            ["ar"] = "\u062D\u062C\u0629 \u0645\u0639\u062A\u0628\u0631\u0629 \u2014 Argumentum",
                            ["fa"] = "\u0627\u0633\u062A\u062F\u0644\u0627\u0644 \u0645\u0639\u062A\u0628\u0631 \u2014 Argumentum",
                            ["zh"] = "\u6709\u6548\u8BBA\u8BC1 \u2014 Argumentum",
                        },
                        HtmlWrappers = new List<DocumentConfig>(new[]
                        {
                            // Issue #196: use [LANGUAGE] placeholder so each language produces its
                            // own file name, mirroring the Fallacies convention. Previous hardcoded
                            // "_fr" caused every language to ship a misnamed file.
                            new DocumentConfig()
                            {
                                DocumentName    = "Argumentation_Virtues_[LANGUAGE].html",
                                TemplatePathRelease =
                                    "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Mindmaps/included.html",
                                TemplatePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Mindmaps\included.html"
                            },
                            new DocumentConfig()
                            {
                                DocumentName    = "Argumentation_Virtues_[LANGUAGE]_ext.html",
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