using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Argumentum.AssetConverter
{


    public class Fallacy
    {


        public static IList<Fallacy> LoadFallacies(string filePath)
        {
            IEnumerable<Fallacy> fallacies;
            Console.WriteLine($"Loading csv fallacies from file {filePath}");
            var fileContent = File.ReadAllText( filePath );
			return LoadFallaciesFromContent(fileContent);
		}

        private static IList<Fallacy> LoadFallaciesFromContent(string fileContent)
        {
	        IEnumerable<Fallacy> fallacies;
	        using (var reader = new StringReader(fileContent))
	        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
	        {
		        csv.Context.RegisterClassMap<FallacyClassMap>();
		        fallacies = csv.GetRecords<Fallacy>().SkipLast(1).ToList();
	        }
	        Console.WriteLine($"Loaded {fallacies.Count()} fallacies");
	        return fallacies.ToList();
        }

		public static async Task< IList<Fallacy>> LoadFallacies(DataSetInfo dataSet)
        {
	        IEnumerable<Fallacy> fallacies;
	        Console.WriteLine($"Loading csv fallacies from dataSet {dataSet.Name}");
	        var payLoad = await dataSet.GetContent();
	        return LoadFallaciesFromContent(payLoad);
        }



		public string LinkFrFallbackEn => string.IsNullOrEmpty(LinkFr) ? LinkEn : LinkFr;

        public string LinkEnFallbackEn => LinkEn;
        public string FileName => $"{Path}_{TextFr.ToLower().Replace(" ","_")}";
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
        public string SimpleNameEn { get; set; }
        public string TextEn { get; set; }
        public int? LTen { get; set; }
        public string Family { get; set; }
        public string Subfamily { get; set; }
        public string Subsubfamily { get; set; }
        public string LinkFr { get; set; }
        public string LinkEn { get; set; }
        public string DescFr { get; set; }
        public int? Lfr115 { get; set; }
        public string ExampleFr { get; set; }
        public int? Lxfr145 { get; set; }
        public string DescEn { get; set; }
        public int? Len115 { get; set; }
        public string ExampleEn { get; set; }
        public int? Lxen145 { get; set; }
        public string Remarques { get; set; }
        public string Latin { get; set; }
        public string Exemplepolitique { get; set; }
        public string PoliticalExampleEn { get; set; }
        public string Proverbe { get; set; }
        public string Lienstransverses { get; set; }
        public string Typelientransverse { get; set; }
        public string Shape { get; set; }
        public string Image { get; set; }
        public string SvgColor { get; set; }
        public string SvgIllustration { get; set; }
    }

    public class FallacyClassMap : ClassMap<Fallacy>
    {
        public FallacyClassMap()
        {
            Map(m => m.PK).Name("PK");
            Map(m => m.Path).Name("path");
            Map(m => m.DecimalPath).Name("decimal_path");
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
            Map(m => m.LTfr).Name("LTfr");
            Map(m => m.SimpleNameEn).Name("Simple_name_en");
            Map(m => m.TextEn).Name("text_en");
            Map(m => m.LTen).Name("LTen");
            Map(m => m.Family).Name("Family");
            Map(m => m.Subfamily).Name("Subfamily");
            Map(m => m.Subsubfamily).Name("Subsubfamily");
            Map(m => m.LinkFr).Name("link_fr");
            Map(m => m.LinkEn).Name("link_en");
            Map(m => m.DescFr).Name("desc_fr");
            Map(m => m.Lfr115).Name("Lfr115");
            Map(m => m.ExampleFr).Name("example_fr");
            Map(m => m.Lxfr145).Name("Lxfr145");
            Map(m => m.DescEn).Name("desc_en");
            Map(m => m.Len115).Name("Len115");
            Map(m => m.ExampleEn).Name("example_en");
            Map(m => m.Lxen145).Name("Lxen145");
            Map(m => m.Remarques).Name("Remarques");
            Map(m => m.Latin).Name("latin");
            Map(m => m.Exemplepolitique).Name("exemple politique");
            Map(m => m.PoliticalExampleEn).Name("political_example_en");
            Map(m => m.Proverbe).Name("proverbe");
            Map(m => m.Lienstransverses).Name("+liens transverses");
            Map(m => m.Typelientransverse).Name("type lien transverse");
            Map(m => m.Shape).Name("shape");
            Map(m => m.Image).Name("image");
            Map(m => m.SvgColor).Name("svg_color");
            Map(m => m.SvgIllustration).Name("svg_illustration");
        }
    }



}
