using System.Linq;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter.Dnn2sxc
{
    /// <summary>
    /// Génération des entités 2sxc depuis les corpus CSV (#136 partie 1). Chaque taxonomie est une
    /// <see cref="Dnn2sxcTaxonomyExportConfig"/> exécutée par le moteur commun
    /// <see cref="Dnn2sxcTaxonomyExporter"/> ; ajouter une taxonomie (Scenarii/Rules, PR 3-4) = une
    /// entrée de config + un appel.
    /// </summary>
    /// <remarks>
    /// <para><b>Cultures</b> : la <b>première</b> entrée de <c>Cultures</c> est la culture
    /// <i>maîtresse</i> — celle qui porte la hiérarchie (l'arbre est construit sur ses entités, les
    /// autres cultures attachent leurs traductions par Guid partagé). Fallacies couvre le scope #136
    /// (fr/en/ru/pt) ; Virtues porte en plus l'es (l'entité a les 5 colonnes) — l'extension aux 8
    /// langues du corpus = une entrée de config.</para>
    /// <para><b>Projection Virtues</b> : le dialecte XML n'a que Name/Description/Example comme
    /// champs textuels (<see cref="Entity"/>) — Title → Name, Description → Description,
    /// Remark → Example.</para>
    /// <para>Historique PR #2047 : le moteur ci-dessous était le corps de <c>Apply()</c> (Fallacies
    /// only, cultures fr/en en dur) ; le défaut <c>File.OpenWrite</c> y a laissé l'export 2023
    /// committé corrompu, et le CSV d'origine pointait un snapshot 2021 illisible par le ClassMap
    /// courant — les deux corrigés là.</para>
    /// </remarks>
    public class Dnn2sxcConfig
    {

        /// <summary>Fallacies — 1408 rangées, fixture 2021 (Guid historiques préservés), 4 cultures.</summary>
        public Dnn2sxcTaxonomyExportConfig Fallacies { get; set; } = new Dnn2sxcTaxonomyExportConfig
        {
            EntityTypeName = "Fallacy",
            RootName = "Fallacies",
            ContextKey = "fallacy",
            CsvPath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv",
            ImportPath = @"..\..\..\Data\Dnn2sxc\2sxc Fallacy  Data 20210212131131.xml",
            ExportPath = @"..\..\..\Data\Dnn2sxc\2sxcContentExport_Argumentum_Fallacies_0.0.1.xml",
            Cultures =
            {
                // ⚠️ Piège mesuré pour ru : le champ CSV s'appelle Exampleru (casse, Fallacy.cs),
                // pas ExampleRu.
                new Dnn2sxcCultureConfig
                {
                    Culture = "en-US",
                    TitleExpression = @"{fallacy.TextEn}",
                    DescriptionExpression = @"{HttpUtility.HtmlEncode(fallacy.DescEn)}",
                    ExampleExpression = @"{HttpUtility.HtmlEncode(fallacy.ExampleEn)}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "fr-FR",
                    TitleExpression = @"{fallacy.TextFr}",
                    DescriptionExpression = @"{HttpUtility.HtmlEncode(fallacy.DescFr)}",
                    ExampleExpression = @"{HttpUtility.HtmlEncode(fallacy.ExampleFr)}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "ru-RU",
                    TitleExpression = @"{fallacy.TextRu}",
                    DescriptionExpression = @"{HttpUtility.HtmlEncode(fallacy.DescRu)}",
                    ExampleExpression = @"{HttpUtility.HtmlEncode(fallacy.Exampleru)}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "pt-BR",
                    TitleExpression = @"{fallacy.TextPt}",
                    DescriptionExpression = @"{HttpUtility.HtmlEncode(fallacy.DescPt)}",
                    ExampleExpression = @"{HttpUtility.HtmlEncode(fallacy.ExamplePt)}",
                },
            },
        };

        /// <summary>
        /// Virtues — 223 rangées (re-mesuré 15/09), <b>sans fixture 2021</b> : la racine est
        /// bootstrappée depuis la rangée CSV "0" (« Valid argument »). 5 cultures (fr/en/ru/pt/es).
        /// </summary>
        public Dnn2sxcTaxonomyExportConfig Virtues { get; set; } = new Dnn2sxcTaxonomyExportConfig
        {
            EntityTypeName = "Virtue",
            RootName = "Virtues",
            ContextKey = "virtue",
            CsvPath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Virtues - Taxonomy.csv",
            ImportPath = "", // pas de fixture d'époque — bootstrap de racine (PR 2)
            ExportPath = @"..\..\..\Data\Dnn2sxc\2sxcContentExport_Argumentum_Virtues_0.0.1.xml",
            Cultures =
            {
                new Dnn2sxcCultureConfig
                {
                    Culture = "en-US",
                    TitleExpression = @"{virtue.TitleEn}",
                    DescriptionExpression = @"{HttpUtility.HtmlEncode(virtue.DescriptionEn)}",
                    ExampleExpression = @"{HttpUtility.HtmlEncode(virtue.RemarkEn)}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "fr-FR",
                    TitleExpression = @"{virtue.TitleFr}",
                    DescriptionExpression = @"{HttpUtility.HtmlEncode(virtue.DescriptionFr)}",
                    ExampleExpression = @"{HttpUtility.HtmlEncode(virtue.RemarkFr)}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "ru-RU",
                    TitleExpression = @"{virtue.TitleRu}",
                    DescriptionExpression = @"{HttpUtility.HtmlEncode(virtue.DescriptionRu)}",
                    ExampleExpression = @"{HttpUtility.HtmlEncode(virtue.RemarkRu)}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "pt-BR",
                    TitleExpression = @"{virtue.TitlePt}",
                    DescriptionExpression = @"{HttpUtility.HtmlEncode(virtue.DescriptionPt)}",
                    ExampleExpression = @"{HttpUtility.HtmlEncode(virtue.RemarkPt)}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "es-ES",
                    TitleExpression = @"{virtue.TitleEs}",
                    DescriptionExpression = @"{HttpUtility.HtmlEncode(virtue.DescriptionEs)}",
                    ExampleExpression = @"{HttpUtility.HtmlEncode(virtue.RemarkEs)}",
                },
            },
        };

        public void Apply()
        {
            // Chaque taxonomie ne tourne que si elle est configurée (CsvPath non vide) — le
            // commutateur des tests et des usages partiels. Fallacies d'abord (continuité
            // historique du mode), puis chaque taxonomie ajoutée.
            if (!string.IsNullOrEmpty(Fallacies?.CsvPath))
            {
                Dnn2sxcTaxonomyExporter.Export(Fallacies,
                    Fallacy.Load(Fallacies.CsvPath).Select(f => (f.Path, (object)f)));
            }
            if (!string.IsNullOrEmpty(Virtues?.CsvPath))
            {
                Dnn2sxcTaxonomyExporter.Export(Virtues,
                    ArgumentVirtue.Load(Virtues.CsvPath).Select(v => (v.Path, (object)v)));
            }
        }
    }
}
