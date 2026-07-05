using Argumentum.AssetConverter.Mindmapper;
using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities
{
    public class Virtue : CsvBase<Virtue, VirtueClassMap>, IMindMapItem
    {
        public string Family => FamilyFr;
        public string SubFamily => SubfamilyFr;
        public string SubSubFamily => SubsubfamilyFr;
  public string Title => TitleFr;
        public string Text => TitleFr;
        public string Description => DescriptionFr;
        public string Example => string.Empty; // No example in source for Virtues
        public string Link => LinkFr;
        public int? Carte => int.TryParse(Card, out int c) ? c : null;
        public string PK { get => Pk; set => Pk = value; }
        public string DecimalPath { get; set; }


        public string Pk { get; set; }
        public string Path { get; set; }
        public int Depth { get; set; }
        public string DecimalPathPadded { get; set; }
        public string FamilyFr { get; set; }
        public string SubfamilyFr { get; set; }
        public string SubsubfamilyFr { get; set; }
        public string TitleFr { get; set; }
        public string DescriptionFr { get; set; }
        public string RemarkFr { get; set; }
        public string LinkFr { get; set; }
        public string FamilyFrCamelcase { get; set; }
        public string DepthMax4 { get; set; }
        public string Card { get; set; }
        public string Update { get; set; }
        public string Locked { get; set; }

        public string FamilyEn { get; set; }
        public string SubfamilyEn { get; set; }
        public string SubsubfamilyEn { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionEn { get; set; }
        public string RemarkEn { get; set; }
        public string LinkEn { get; set; }

        public string FamilyRu { get; set; }
        public string SubfamilyRu { get; set; }
        public string SubsubfamilyRu { get; set; }
        public string TitleRu { get; set; }
        public string DescriptionRu { get; set; }
        public string RemarkRu { get; set; }
        public string LinkRu { get; set; }

        public string FamilyPt { get; set; }
        public string SubfamilyPt { get; set; }
        public string SubsubfamilyPt { get; set; }
        public string TitlePt { get; set; }
        public string DescriptionPt { get; set; }
        public string RemarkPt { get; set; }
        public string LinkPt { get; set; }

        public string FamilyEs { get; set; }
        public string SubfamilyEs { get; set; }
        public string SubsubfamilyEs { get; set; }
        public string TitleEs { get; set; }
        public string DescriptionEs { get; set; }
        public string RemarkEs { get; set; }
        public string LinkEs { get; set; }

        public string FamilyAr { get; set; }
        public string SubfamilyAr { get; set; }
        public string SubsubfamilyAr { get; set; }
        public string TitleAr { get; set; }
        public string DescriptionAr { get; set; }
        public string RemarkAr { get; set; }
        public string LinkAr { get; set; }

        public string FamilyFa { get; set; }
        public string SubfamilyFa { get; set; }
        public string SubsubfamilyFa { get; set; }
        public string TitleFa { get; set; }
        public string DescriptionFa { get; set; }
        public string RemarkFa { get; set; }
        public string LinkFa { get; set; }

        public string FamilyZh { get; set; }
        public string SubfamilyZh { get; set; }
        public string SubsubfamilyZh { get; set; }
        public string TitleZh { get; set; }
        public string DescriptionZh { get; set; }
        public string RemarkZh { get; set; }
        public string LinkZh { get; set; }

        // #499 Phase 1 — 12 relational/AIF columns appended to the Virtues prod CSV (66→78).
        // crossLink_Opposes is the only one populated (the prevented Fallacy-family PK list);
        // the other 7 relation types + AIF Exception/Other are structurally empty by design.
        public string CrossLinkPredatesOn { get; set; }
        public string CrossLinkDenounces { get; set; }
        public string CrossLinkLeverages { get; set; }
        public string CrossLinkAllows { get; set; }
        public string CrossLinkOpposes { get; set; }
        public string CrossLinkInverts { get; set; }
        public string CrossLinkMirrors { get; set; }
        public string CrossLinkIsRelatedTo { get; set; }

        public string AIFSkosDirectRef { get; set; }
        public string AIFSkosExceptionRef { get; set; }
        public string AIFSkosOther { get; set; }
        public string AIFSkosMappingType { get; set; }
    }

    public sealed class VirtueClassMap : ClassMap<Virtue>
    {
        public VirtueClassMap()
        {
            Map(m => m.Id).Name("pk");
            Map(m => m.Pk).Name("pk");
            Map(m => m.Path).Name("path");
            Map(m => m.Depth).Name("depth");
            Map(m => m.DecimalPathPadded).Name("decimal_path_padded");
            // Map(m => m.DecimalPath).Name("decimal_path");
            Map(m => m.FamilyFr).Name("family_fr");
            Map(m => m.SubfamilyFr).Name("subfamily_fr");
            Map(m => m.SubsubfamilyFr).Name("subsubfamily_fr");
            Map(m => m.TitleFr).Name("title_fr");
            Map(m => m.DescriptionFr).Name("description_fr");
            Map(m => m.RemarkFr).Name("remark_fr");
            Map(m => m.LinkFr).Name("link_fr");
            Map(m => m.FamilyFrCamelcase).Name("family_fr_camelcase");
            Map(m => m.DepthMax4).Name("depth_max4");
            Map(m => m.Card).Name("card");
            Map(m => m.Update).Name("update");
            Map(m => m.Locked).Name("locked");

            Map(m => m.FamilyEn).Name("family_en").Optional();
            Map(m => m.SubfamilyEn).Name("subfamily_en").Optional();
            Map(m => m.SubsubfamilyEn).Name("subsubfamily_en").Optional();
            Map(m => m.TitleEn).Name("title_en").Optional();
            Map(m => m.DescriptionEn).Name("description_en").Optional();
            Map(m => m.RemarkEn).Name("remark_en").Optional();
            Map(m => m.LinkEn).Name("link_en").Optional();

            Map(m => m.FamilyRu).Name("family_ru").Optional();
            Map(m => m.SubfamilyRu).Name("subfamily_ru").Optional();
            Map(m => m.SubsubfamilyRu).Name("subsubfamily_ru").Optional();
            Map(m => m.TitleRu).Name("title_ru").Optional();
            Map(m => m.DescriptionRu).Name("description_ru").Optional();
            Map(m => m.RemarkRu).Name("remark_ru").Optional();
            Map(m => m.LinkRu).Name("link_ru").Optional();

            Map(m => m.FamilyPt).Name("family_pt").Optional();
            Map(m => m.SubfamilyPt).Name("subfamily_pt").Optional();
            Map(m => m.SubsubfamilyPt).Name("subsubfamily_pt").Optional();
            Map(m => m.TitlePt).Name("title_pt").Optional();
            Map(m => m.DescriptionPt).Name("description_pt").Optional();
            Map(m => m.RemarkPt).Name("remark_pt").Optional();
            Map(m => m.LinkPt).Name("link_pt").Optional();

            Map(m => m.FamilyEs).Name("family_es").Optional();
            Map(m => m.SubfamilyEs).Name("subfamily_es").Optional();
            Map(m => m.SubsubfamilyEs).Name("subsubfamily_es").Optional();
            Map(m => m.TitleEs).Name("title_es").Optional();
            Map(m => m.DescriptionEs).Name("description_es").Optional();
            Map(m => m.RemarkEs).Name("remark_es").Optional();
            Map(m => m.LinkEs).Name("link_es").Optional();

            Map(m => m.FamilyAr).Name("family_ar").Optional();
            Map(m => m.SubfamilyAr).Name("subfamily_ar").Optional();
            Map(m => m.SubsubfamilyAr).Name("subsubfamily_ar").Optional();
            Map(m => m.TitleAr).Name("title_ar").Optional();
            Map(m => m.DescriptionAr).Name("description_ar").Optional();
            Map(m => m.RemarkAr).Name("remark_ar").Optional();
            Map(m => m.LinkAr).Name("link_ar").Optional();

            Map(m => m.FamilyFa).Name("family_fa").Optional();
            Map(m => m.SubfamilyFa).Name("subfamily_fa").Optional();
            Map(m => m.SubsubfamilyFa).Name("subsubfamily_fa").Optional();
            Map(m => m.TitleFa).Name("title_fa").Optional();
            Map(m => m.DescriptionFa).Name("description_fa").Optional();
            Map(m => m.RemarkFa).Name("remark_fa").Optional();
            Map(m => m.LinkFa).Name("link_fa").Optional();

            Map(m => m.FamilyZh).Name("family_zh").Optional();
            Map(m => m.SubfamilyZh).Name("subfamily_zh").Optional();
            Map(m => m.SubsubfamilyZh).Name("subsubfamily_zh").Optional();
            Map(m => m.TitleZh).Name("title_zh").Optional();
            Map(m => m.DescriptionZh).Name("description_zh").Optional();
            Map(m => m.RemarkZh).Name("remark_zh").Optional();
            Map(m => m.LinkZh).Name("link_zh").Optional();

            // #499 Phase 1 — 12 relational/AIF columns. All Optional(): 9 are structurally
            // empty by design; 3 are populated for the 222 real Virtue nodes (pk=0 root empty).
            Map(m => m.CrossLinkPredatesOn).Name("crossLink_PredatesOn").Optional();
            Map(m => m.CrossLinkDenounces).Name("crossLink_Denounces").Optional();
            Map(m => m.CrossLinkLeverages).Name("crossLink_Leverages").Optional();
            Map(m => m.CrossLinkAllows).Name("crossLink_Allows").Optional();
            Map(m => m.CrossLinkOpposes).Name("crossLink_Opposes").Optional();
            Map(m => m.CrossLinkInverts).Name("crossLink_Inverts").Optional();
            Map(m => m.CrossLinkMirrors).Name("crossLink_Mirrors").Optional();
            Map(m => m.CrossLinkIsRelatedTo).Name("crossLink_IsRelatedTo").Optional();
            Map(m => m.AIFSkosDirectRef).Name("AIF_skosDirectRef").Optional();
            Map(m => m.AIFSkosExceptionRef).Name("AIF_skosExceptionRef").Optional();
            Map(m => m.AIFSkosOther).Name("AIF_skosOther").Optional();
            Map(m => m.AIFSkosMappingType).Name("AIF_skosMappingType").Optional();
        }
    }
}