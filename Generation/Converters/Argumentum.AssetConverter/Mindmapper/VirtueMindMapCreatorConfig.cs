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
                Translations = new List<(string sourceLang, string destLang)>(),
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
                        WrapNodeByLink = false,
                        SetSVGNodeAttributes = true,
                        RemoveImages = true,
                        HtmlWrappers = new List<DocumentConfig>(new[]
                        {
                            new DocumentConfig()
                            {
                                DocumentName    = "Argumentation_Virtues_fr.html",
                                TemplatePathRelease =
                                    "https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/Fallacies/Mindmaps/included.html",
                                TemplatePathDebug = @"..\..\..\..\..\..\Cards\Fallacies\Mindmaps\included.html"
                            },
                            new DocumentConfig()
                            {
                                DocumentName    = "Argumentation_Virtues_fr_ext.html",
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