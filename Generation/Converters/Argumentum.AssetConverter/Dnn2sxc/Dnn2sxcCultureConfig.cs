namespace Argumentum.AssetConverter.Dnn2sxc
{
    /// <summary>
    /// Expressions d'une culture pour l'export Dnn2sxc (#136 partie 1). Chaque expression est interpolée
    /// contre une entité <c>Fallacy</c> chargée du CSV (même mécanisme que les anciennes propriétés
    /// <c>TitleExpressionFr</c>/<c>…En</c>, généralisées : ajouter une langue = ajouter une entrée).
    /// </summary>
    public class Dnn2sxcCultureConfig
    {
        /// <summary>Code de culture 2sxc/DNN porté par l'élément <c>&lt;Language&gt;</c> (ex. <c>en-US</c>, <c>pt-BR</c>).</summary>
        public string Culture { get; set; }

        public string TitleExpression { get; set; }

        public string DescriptionExpression { get; set; }

        public string ExampleExpression { get; set; }
    }
}
