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
                    ("fr", "pt")
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