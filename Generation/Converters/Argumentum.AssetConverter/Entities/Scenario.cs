using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities
{
    public class Scenario : CsvBase<Scenario, ScenarioClassMap>, ICsvBase
    {
        public string GetId()
        {
            return path ?? string.Empty;
        }

        public string path { get; set; }
        public string coordonnees { get; set; }
        public string categorie { get; set; }
        public string sous_categorie { get; set; }
        public string titre { get; set; }
        public string baratineur { get; set; }
        public string piocheur { get; set; }
        public string contexte { get; set; }
        public string enjeu { get; set; }
        public string suggestion { get; set; }
        public string CCby { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string title { get; set; }
        public string smoothTalker { get; set; }
        public string drawer { get; set; }
        public string context { get; set; }
        public string issue { get; set; }
        public string suggestion_en { get; set; }
        public string category_ru { get; set; }
        public string subcategory_ru { get; set; }
        public string title_ru { get; set; }
        public string smoothTalker_ru { get; set; }
        public string drawer_ru { get; set; }
        public string context_ru { get; set; }
        public string issue_ru { get; set; }
        public string suggestion_ru { get; set; }
        public string category_pt { get; set; }
        public string subcategory_pt { get; set; }
        public string title_pt { get; set; }
        public string smoothTalker_pt { get; set; }
        public string drawer_pt { get; set; }
        public string context_pt { get; set; }
        public string issue_pt { get; set; }
        public string suggestion_pt { get; set; }
        public string edition_fevrier_2022 { get; set; }
        public string print_and_play_fevrier_2022 { get; set; }
        public string print_and_play { get; set; }
    }

    public sealed class ScenarioClassMap : ClassMap<Scenario>
    {
        public ScenarioClassMap()
        {
            
            Map(m => m.path).Name("path");
            Map(m => m.coordonnees).Name("coordonnées");
            Map(m => m.categorie).Name("catégorie");
            Map(m => m.sous_categorie).Name("sous-catégorie");
            Map(m => m.titre).Name("titre");
            Map(m => m.baratineur).Name("baratineur");
            Map(m => m.piocheur).Name("piocheur");
            Map(m => m.contexte).Name("contexte");
            Map(m => m.enjeu).Name("enjeu");
            Map(m => m.suggestion).Name("suggestion");
            Map(m => m.CCby).Name("CCby");
            Map(m => m.category).Name("category");
            Map(m => m.subcategory).Name("subcategory");
            Map(m => m.title).Name("title");
            Map(m => m.smoothTalker).Name("smoothTalker");
            Map(m => m.drawer).Name("drawer");
            Map(m => m.context).Name("context");
            Map(m => m.issue).Name("issue");
            Map(m => m.suggestion_en).Name("suggestion_en");
            Map(m => m.category_ru).Name("category_ru");
            Map(m => m.subcategory_ru).Name("subcategory_ru");
            Map(m => m.title_ru).Name("title_ru");
            Map(m => m.smoothTalker_ru).Name("smoothTalker_ru");
            Map(m => m.drawer_ru).Name("drawer_ru");
            Map(m => m.context_ru).Name("context_ru");
            Map(m => m.issue_ru).Name("issue_ru");
            Map(m => m.suggestion_ru).Name("suggestion_ru");
            Map(m => m.category_pt).Name("category_pt");
            Map(m => m.subcategory_pt).Name("subcategory_pt");
            Map(m => m.title_pt).Name("title_pt");
            Map(m => m.smoothTalker_pt).Name("smoothTalker_pt");
            Map(m => m.drawer_pt).Name("drawer_pt");
            Map(m => m.context_pt).Name("context_pt");
            Map(m => m.issue_pt).Name("issue_pt");
            Map(m => m.suggestion_pt).Name("suggestion_pt");
            Map(m => m.edition_fevrier_2022).Name("édition février 2022");
            Map(m => m.print_and_play_fevrier_2022).Name("print_&_play_fevrier_2022");
            Map(m => m.print_and_play).Name("print_and_play").Optional();
        }
    }
}