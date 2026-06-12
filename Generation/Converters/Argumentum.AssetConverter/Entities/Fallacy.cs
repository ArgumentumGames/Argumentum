using System;
using System.Text;
using Argumentum.AssetConverter.Mindmapper;
using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities
{
	public class Fallacy : CsvBase<Fallacy, FallacyClassMap>, IMindMapItem, ICsvBase
	{
		public string GetId()
		{
			return Id;
		}

		public string Family => Famille;
	       public string SubFamily => SousFamille;
	       public string SubSubFamily => Soussousfamille;
	       public string Title => NomVulgarisé;
	       public string Text => TextFr;
	       public string Description => DescFr;
	       public string Example => ExampleFr;
	       public string Link => LinkFrFallback;
	          public string Id { get; set; }

	       public string LinkFrFallback => string.IsNullOrEmpty(LinkFr) ? LinkEn : LinkFr;

	       public string LinkEnFallback => string.IsNullOrEmpty(LinkEn) ? LinkFr : LinkEn;

        public string LinkRuFallback => string.IsNullOrEmpty(LinkRu) ? string.IsNullOrEmpty(LinkEn) ? LinkFr : LinkEn : LinkRu;

        public string LinkPtFallback => string.IsNullOrEmpty(LinkPt) ? string.IsNullOrEmpty(LinkEn) ? LinkFr : LinkEn : LinkPt;

        public string LinkEsFallback => string.IsNullOrEmpty(LinkEs) ? string.IsNullOrEmpty(LinkEn) ? LinkFr : LinkEn : LinkEs;

        public string LinkArFallback => string.IsNullOrEmpty(LinkAr) ? string.IsNullOrEmpty(LinkEn) ? LinkFr : LinkEn : LinkAr;

        public string LinkFaFallback => string.IsNullOrEmpty(LinkFa) ? string.IsNullOrEmpty(LinkEn) ? LinkFr : LinkEn : LinkFa;

        public string LinkZhFallback => string.IsNullOrEmpty(LinkZh) ? string.IsNullOrEmpty(LinkEn) ? LinkFr : LinkEn : LinkZh;

        //public string FileName => $"argumentum_{Path}-{TextFr.ToLower().Replace(" ","_")}";

        public string PK { get; set; }
        public string Path { get; set; }
        public string DecimalPath { get; set; }
        public int Depth { get; set; }
        public string Famille { get; set; }
        public string FamilleCamelCase { get; set; }
        public string SousFamille { get; set; }
        public string Soussousfamille { get; set; }
        public string État { get; set; }
        public int? Carte { get; set; }
        public int? Niveau { get; set; }
        public string NomVulgarisé { get; set; }


        public string TextFr { get; set; }
        public int? LTfr { get; set; }

        public string DescFr { get; set; }
        public int? Lfr115 { get; set; }
        public string ExampleFr { get; set; }
        public int? Lxfr145 { get; set; }

        public string LinkFr { get; set; }

        public string Subfamily { get; set; }
        public string Subsubfamily { get; set; }

        public string TextEn { get; set; }
        public int? LTen { get; set; }

        public string SimpleNameEn { get; set; }


        public string DescEn { get; set; }
        public int? Len115 { get; set; }
        public string ExampleEn { get; set; }

        public string ExampleEnBis { get; set; }

        public string LinkEn { get; set; }
        public int? Lxen145 { get; set; }
        public string Remarques { get; set; }

        public string Latin { get; set; }

        public string ExemplePolitique { get; set; }
        public string PoliticalExampleEn { get; set; }
        public string Proverbe { get; set; }


        public string FamilyRu { get; set; }
        public string SubfamilyRu { get; set; }
        public string SubsubfamilyRu { get; set; }


        public string TextRu { get; set; }

        public string DescRu { get; set; }

        public string Exampleru { get; set; }

        public string LinkRu { get; set; }

        public string FamilyPt { get; set; }
        public string SubfamilyPt { get; set; }
        public string SubsubfamilyPt { get; set; }

        public string TextPt { get; set; }

        public string DescPt { get; set; }

        public string ExamplePt { get; set; }

        public string LinkPt { get; set; }

        public string FamilyEs { get; set; }
        public string SubfamilyEs { get; set; }
        public string SubsubfamilyEs { get; set; }

        public string TextEs { get; set; }

        public string DescEs { get; set; }

        public string ExampleEs { get; set; }

        public string LinkEs { get; set; }

        public string FamilyAr { get; set; }
        public string SubfamilyAr { get; set; }
        public string SubsubfamilyAr { get; set; }

        public string TextAr { get; set; }

        public string DescAr { get; set; }

        public string ExampleAr { get; set; }

        public string LinkAr { get; set; }

        public string FamilyFa { get; set; }
        public string SubfamilyFa { get; set; }
        public string SubsubfamilyFa { get; set; }

        public string TextFa { get; set; }

        public string DescFa { get; set; }

        public string ExampleFa { get; set; }

        public string LinkFa { get; set; }

        public string FamilyZh { get; set; }
        public string SubfamilyZh { get; set; }
        public string SubsubfamilyZh { get; set; }

        public string TextZh { get; set; }

        public string DescZh { get; set; }

        public string ExampleZh { get; set; }

        public string LinkZh { get; set; }

        public string PlusLienstransverses { get; set; }
        public string TypeLienTransverse { get; set; }
        public string Shape { get; set; }
        public string Image { get; set; }
        public string SvgColor { get; set; }
        public string SvgIllustration { get; set; }

        public string AIFSkosDirectRef { get; set; }

        public string AIFSkosExceptionRef { get; set; }

        public string AIFSkosOther { get; set; }

        public string AIFSkosMappingType { get; set; }


		public int? PrintAndPlay { get; set; }

        public string DécimalPathPadded { get; set; }

        public string DepthMax4 { get; set; }

        // IMindMapItem explicit implementation for Pk (interface requires both Pk and PK)
        string IMindMapItem.Pk { get => PK; set => PK = value; }
	}

    public sealed class FallacyClassMap : ClassMap<Fallacy>
    {
        public FallacyClassMap()
        {
            Map(m => m.PK).Name("pk");
            Map(m => m.Path).Name("path");
            Map(m => m.DecimalPath).Name("decimal_path").Optional();
            Map(m => m.Depth).Name("depth");
            Map(m => m.Famille).Name("Famille");
            Map(m => m.FamilleCamelCase).Name("Famille_camelCase");
            Map(m => m.SousFamille).Name("Sous-Famille");
            Map(m => m.Soussousfamille).Name("Soussousfamille");
            Map(m => m.État).Name("état");
            Map(m => m.Carte).Name("carte");
            Map(m => m.Niveau).Name("niveau");
            Map(m => m.NomVulgarisé).Name("nom_vulgarisé");
            Map(m => m.TextFr).Name("text_fr");
            Map(m => m.DescFr).Name("desc_fr");
            Map(m => m.Lfr115).Name("Lfr115").Optional();
            Map(m => m.ExampleFr).Name("example_fr");
            Map(m => m.LTfr).Name("LTfr").Optional();
            Map(m => m.LinkFr).Name("link_fr");
            Map(m => m.SimpleNameEn).Name("Simple_name_en").Optional();
            Map(m => m.LTen).Name("LTen").Optional();
            Map(m => m.Family).Name("Family").Optional();
            Map(m => m.Subfamily).Name("Subfamily").Optional();
            Map(m => m.Subsubfamily).Name("Subsubfamily").Optional();
            Map(m => m.TextEn).Name("text_en").Optional();
            Map(m => m.DescEn).Name("desc_en").Optional();
            Map(m => m.Len115).Name("Len115").Optional();
            Map(m => m.ExampleEn).Name("example_en").Optional();
            Map(m => m.Lxen145).Name("Lxen145").Optional();
            Map(m => m.ExampleEnBis).Name("example_en_bis").Optional();
            Map(m => m.LinkEn).Name("link_en").Optional();
            Map(m => m.FamilyRu).Name("Family_ru").Optional();
            Map(m => m.SubfamilyRu).Name("Subfamily_ru").Optional();
            Map(m => m.SubsubfamilyRu).Name("Subsubfamily_ru").Optional();
            Map(m => m.TextRu).Name("text_ru").Optional();
            Map(m => m.DescRu).Name("desc_ru").Optional();
            Map(m => m.Exampleru).Name("example_ru").Optional();
            Map(m => m.LinkRu).Name("link_ru").Optional();
            Map(m => m.FamilyPt).Name("Family_pt").Optional();
            Map(m => m.SubfamilyPt).Name("Subfamily_pt").Optional();
            Map(m => m.SubsubfamilyPt).Name("Subsubfamily_pt").Optional();
            Map(m => m.TextPt).Name("text_pt").Optional();
            Map(m => m.DescPt).Name("desc_pt").Optional();
            Map(m => m.ExamplePt).Name("example_pt").Optional();
            Map(m => m.LinkPt).Name("link_pt").Optional();
            Map(m => m.FamilyEs).Name("Family_es").Optional();
            Map(m => m.SubfamilyEs).Name("Subfamily_es").Optional();
            Map(m => m.SubsubfamilyEs).Name("Subsubfamily_es").Optional();
            Map(m => m.TextEs).Name("text_es").Optional();
            Map(m => m.DescEs).Name("desc_es").Optional();
            Map(m => m.ExampleEs).Name("example_es").Optional();
            Map(m => m.LinkEs).Name("link_es").Optional();
            Map(m => m.FamilyAr).Name("Family_ar").Optional();
            Map(m => m.SubfamilyAr).Name("Subfamily_ar").Optional();
            Map(m => m.SubsubfamilyAr).Name("Subsubfamily_ar").Optional();
            Map(m => m.TextAr).Name("text_ar").Optional();
            Map(m => m.DescAr).Name("desc_ar").Optional();
            Map(m => m.ExampleAr).Name("example_ar").Optional();
            Map(m => m.LinkAr).Name("link_ar").Optional();
            Map(m => m.FamilyFa).Name("Family_fa").Optional();
            Map(m => m.SubfamilyFa).Name("Subfamily_fa").Optional();
            Map(m => m.SubsubfamilyFa).Name("Subsubfamily_fa").Optional();
            Map(m => m.TextFa).Name("text_fa").Optional();
            Map(m => m.DescFa).Name("desc_fa").Optional();
            Map(m => m.ExampleFa).Name("example_fa").Optional();
            Map(m => m.LinkFa).Name("link_fa").Optional();
            Map(m => m.FamilyZh).Name("Family_zh").Optional();
            Map(m => m.SubfamilyZh).Name("Subfamily_zh").Optional();
            Map(m => m.SubsubfamilyZh).Name("Subsubfamily_zh").Optional();
            Map(m => m.TextZh).Name("text_zh").Optional();
            Map(m => m.DescZh).Name("desc_zh").Optional();
            Map(m => m.ExampleZh).Name("example_zh").Optional();
            Map(m => m.LinkZh).Name("link_zh").Optional();
            Map(m => m.Lxfr145).Name("Lxfr145").Optional();
            Map(m => m.Remarques).Name("Remarques");
            Map(m => m.Latin).Name("Latin").Optional();
            Map(m => m.ExemplePolitique).Name("exemple politique").Optional();
            Map(m => m.PoliticalExampleEn).Name("political_example_en").Optional();
            Map(m => m.Proverbe).Name("proverbe").Optional();
            Map(m => m.PlusLienstransverses).Name("+liens transverses").Optional();
            Map(m => m.TypeLienTransverse).Name("type lien transverse").Optional();
            Map(m => m.Shape).Name("shape").Optional();
            Map(m => m.Image).Name("image").Optional();
            Map(m => m.SvgColor).Name("svg_color").Optional();
            Map(m => m.SvgIllustration).Name("svg_illustration").Optional();
            Map(m => m.AIFSkosDirectRef).Name("AIF_skosDirectRef").Optional();
            Map(m => m.AIFSkosExceptionRef).Name("AIF_skosExceptionRef").Optional();
            Map(m => m.AIFSkosOther).Name("AIF_skosOther").Optional();
            Map(m => m.AIFSkosMappingType).Name("AIF_skosMappingType").Optional();
            Map(m => m.PrintAndPlay).Name("print_and_play").Optional();
            Map(m => m.DécimalPathPadded).Name("decimal_path_padded");
            Map(m => m.DepthMax4).Name("depth_max4");
            Map(m => m.Id).Name("PK");
  }
 }

          



}
