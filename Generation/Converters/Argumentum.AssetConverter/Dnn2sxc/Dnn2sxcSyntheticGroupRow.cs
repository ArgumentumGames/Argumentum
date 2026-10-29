namespace Argumentum.AssetConverter.Dnn2sxc
{
    /// <summary>
    /// Rangée synthétique de groupe (catégorie ou sous-catégorie Scenarii) pour l'export Dnn2sxc
    /// (#136 partie 1, PR 3). Le CSV Scenarii ne porte que les feuilles — ses chemins sont de
    /// profondeur 3 (« 1.1.1 ») sans rangée « 1 » ni « 1.1 » — alors que le moteur construit
    /// l'arbre en résolvant chaque parent : les nœuds intermédiaires sont donc synthétisés depuis
    /// les colonnes catégorie/sous-catégorie de la première rangée de chaque groupe.
    /// </summary>
    /// <remarks>
    /// Les expressions de culture Scenarii référencent ces membres (Dynamic LINQ, sensible à la
    /// casse) — la classe doit les déclarer TOUS pour que la compilation d'expression réussisse
    /// aussi sur une rangée de groupe ; seules les formes « titre » portent une valeur, les
    /// champs textuels restent vides (un groupe n'a ni contexte ni enjeu).
    /// </remarks>
    public class Dnn2sxcSyntheticGroupRow
    {
        public string titre { get; set; }
        public string title { get; set; }
        public string title_ru { get; set; }
        public string title_pt { get; set; }
        public string title_es { get; set; }
        public string title_ar { get; set; }
        public string title_fa { get; set; }
        public string title_zh { get; set; }

        public string contexte { get; set; }
        public string context { get; set; }
        public string context_ru { get; set; }
        public string context_pt { get; set; }
        public string context_es { get; set; }
        public string context_ar { get; set; }
        public string context_fa { get; set; }
        public string context_zh { get; set; }

        public string enjeu { get; set; }
        public string issue { get; set; }
        public string issue_ru { get; set; }
        public string issue_pt { get; set; }
        public string issue_es { get; set; }
        public string issue_ar { get; set; }
        public string issue_fa { get; set; }
        public string issue_zh { get; set; }
    }
}
