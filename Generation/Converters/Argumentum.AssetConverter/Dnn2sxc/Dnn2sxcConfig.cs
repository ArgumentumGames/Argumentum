using System.Collections.Generic;
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
    /// autres cultures attachent leurs traductions par Guid partagé). Depuis #136 (b) les QUATRE
    /// taxonomies couvrent les 8 langues du corpus : Fallacies les avait déjà dans son entité,
    /// Virtues et Scenarii ont reçu les colonnes ar/fa/zh à cette occasion.</para>
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

        /// <summary>Fallacies — 1408 rangées, fixture 2021 (Guid historiques préservés), 8 cultures.</summary>
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
                    DescriptionExpression = @"{fallacy.DescEn}",
                    ExampleExpression = @"{fallacy.ExampleEn}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "fr-FR",
                    TitleExpression = @"{fallacy.TextFr}",
                    DescriptionExpression = @"{fallacy.DescFr}",
                    ExampleExpression = @"{fallacy.ExampleFr}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "ru-RU",
                    TitleExpression = @"{fallacy.TextRu}",
                    DescriptionExpression = @"{fallacy.DescRu}",
                    ExampleExpression = @"{fallacy.Exampleru}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "pt-BR",
                    TitleExpression = @"{fallacy.TextPt}",
                    DescriptionExpression = @"{fallacy.DescPt}",
                    ExampleExpression = @"{fallacy.ExamplePt}",
                },
                // #136 (b) — les 8 langues du corpus. Fallacy expose déjà ces colonnes : aucune
                // entité ni ClassMap à étendre, quatre entrées de config suffisent.
                new Dnn2sxcCultureConfig
                {
                    Culture = "es-ES",
                    TitleExpression = @"{fallacy.TextEs}",
                    DescriptionExpression = @"{fallacy.DescEs}",
                    ExampleExpression = @"{fallacy.ExampleEs}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "ar-SA",
                    TitleExpression = @"{fallacy.TextAr}",
                    DescriptionExpression = @"{fallacy.DescAr}",
                    ExampleExpression = @"{fallacy.ExampleAr}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "fa-IR",
                    TitleExpression = @"{fallacy.TextFa}",
                    DescriptionExpression = @"{fallacy.DescFa}",
                    ExampleExpression = @"{fallacy.ExampleFa}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "zh-CN",
                    TitleExpression = @"{fallacy.TextZh}",
                    DescriptionExpression = @"{fallacy.DescZh}",
                    ExampleExpression = @"{fallacy.ExampleZh}",
                },
            },
        };

        /// <summary>
        /// Virtues — 223 rangées (re-mesuré 15/09), <b>sans fixture 2021</b> : la racine est
        /// bootstrappée depuis la rangée CSV "0" (« Valid argument »). 8 cultures (fr/en/ru/pt/es/ar/fa/zh).
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
                    DescriptionExpression = @"{virtue.DescriptionEn}",
                    ExampleExpression = @"{virtue.RemarkEn}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "fr-FR",
                    TitleExpression = @"{virtue.TitleFr}",
                    DescriptionExpression = @"{virtue.DescriptionFr}",
                    ExampleExpression = @"{virtue.RemarkFr}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "ru-RU",
                    TitleExpression = @"{virtue.TitleRu}",
                    DescriptionExpression = @"{virtue.DescriptionRu}",
                    ExampleExpression = @"{virtue.RemarkRu}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "pt-BR",
                    TitleExpression = @"{virtue.TitlePt}",
                    DescriptionExpression = @"{virtue.DescriptionPt}",
                    ExampleExpression = @"{virtue.RemarkPt}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "es-ES",
                    TitleExpression = @"{virtue.TitleEs}",
                    DescriptionExpression = @"{virtue.DescriptionEs}",
                    ExampleExpression = @"{virtue.RemarkEs}",
                },
                // #136 (b) — ar/fa/zh exigent l'extension d'ArgumentVirtue + ClassMap (les colonnes
                // existaient au CSV, pas dans l'entité).
                new Dnn2sxcCultureConfig
                {
                    Culture = "ar-SA",
                    TitleExpression = @"{virtue.TitleAr}",
                    DescriptionExpression = @"{virtue.DescriptionAr}",
                    ExampleExpression = @"{virtue.RemarkAr}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "fa-IR",
                    TitleExpression = @"{virtue.TitleFa}",
                    DescriptionExpression = @"{virtue.DescriptionFa}",
                    ExampleExpression = @"{virtue.RemarkFa}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "zh-CN",
                    TitleExpression = @"{virtue.TitleZh}",
                    DescriptionExpression = @"{virtue.DescriptionZh}",
                    ExampleExpression = @"{virtue.RemarkZh}",
                },
            },
        };

        /// <summary>
        /// Scenarii — 167 scénarii, chemins de profondeur 3 SANS rangées intermédiaires : les
        /// 7 catégories et 21 sous-catégories sont synthétisées (voir
        /// <see cref="Dnn2sxcSyntheticGroupRow"/>). 8 cultures (fr/en/ru/pt/es/ar/fa/zh). Projection : titre → Name, contexte →
        /// Description, enjeu → Example.
        /// </summary>
        public Dnn2sxcTaxonomyExportConfig Scenarii { get; set; } = new Dnn2sxcTaxonomyExportConfig
        {
            EntityTypeName = "Scenario",
            RootName = "Scenarii",
            ContextKey = "scenario",
            CsvPath = @"..\..\..\..\..\..\Cards\Scenarii\Argumentum Scenarii - Cards.csv",
            ImportPath = "", // pas de fixture d'époque — bootstrap de racine (PR 3)
            ExportPath = @"..\..\..\Data\Dnn2sxc\2sxcContentExport_Argumentum_Scenarii_0.0.1.xml",
            Cultures =
            {
                new Dnn2sxcCultureConfig
                {
                    Culture = "en-US",
                    TitleExpression = @"{scenario.title}",
                    DescriptionExpression = @"{scenario.context}",
                    ExampleExpression = @"{scenario.issue}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "fr-FR",
                    TitleExpression = @"{scenario.titre}",
                    DescriptionExpression = @"{scenario.contexte}",
                    ExampleExpression = @"{scenario.enjeu}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "ru-RU",
                    TitleExpression = @"{scenario.title_ru}",
                    DescriptionExpression = @"{scenario.context_ru}",
                    ExampleExpression = @"{scenario.issue_ru}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "pt-BR",
                    TitleExpression = @"{scenario.title_pt}",
                    DescriptionExpression = @"{scenario.context_pt}",
                    ExampleExpression = @"{scenario.issue_pt}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "es-ES",
                    TitleExpression = @"{scenario.title_es}",
                    DescriptionExpression = @"{scenario.context_es}",
                    ExampleExpression = @"{scenario.issue_es}",
                },
                // #136 (b) — ar/fa/zh : extension de Scenario + ClassMap et des rangées de groupe.
                new Dnn2sxcCultureConfig
                {
                    Culture = "ar-SA",
                    TitleExpression = @"{scenario.title_ar}",
                    DescriptionExpression = @"{scenario.context_ar}",
                    ExampleExpression = @"{scenario.issue_ar}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "fa-IR",
                    TitleExpression = @"{scenario.title_fa}",
                    DescriptionExpression = @"{scenario.context_fa}",
                    ExampleExpression = @"{scenario.issue_fa}",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "zh-CN",
                    TitleExpression = @"{scenario.title_zh}",
                    DescriptionExpression = @"{scenario.context_zh}",
                    ExampleExpression = @"{scenario.issue_zh}",
                },
            },
        };

        /// <summary>
        /// Rules — 15 rangées PLATES (aucune hiérarchie : pas de colonne path), texte intégral en
        /// markdown par langue. C'est la seule taxonomie couvrant les 8 langues du corpus (fr/en/
        /// ru/pt/es/ar/fa/zh). Projection : Pk → Name (stable et unique — le titre d'affichage est
        /// un en-tête markdown DANS le texte, à rendre côté site), texte → Description, Example
        /// vide (pas de 3ᵉ champ). Chemins synthétiques « 1 »…« 15 » (l'ordre du CSV).
        /// </summary>
        /// <remarks>
        /// ⚠️ <b>Example = <c>""</c>, jamais <c>"{}"</c></b> (mesuré 29/10) :
        /// <see cref="UtilityExtensions.Interpolate"/> découpe sur <c>{(.+?)}</c> — un « {} » vide ne
        /// correspond pas au motif et <b>traverse littéralement</b> l'export. Écrit <c>@"{}"</c> ici,
        /// il produisait <c>&lt;Example&gt;{}&lt;/Example&gt;</c> sur les 120 entités (15 × 8), soit un
        /// gabarit affiché comme du contenu — même famille que le marqueur de vide 2sxc « [] ». La
        /// garde commune (<c>Dnn2sxcCommonExportContractTests</c>) refuse désormais les deux.
        /// </remarks>
        public Dnn2sxcTaxonomyExportConfig Rules { get; set; } = new Dnn2sxcTaxonomyExportConfig
        {
            EntityTypeName = "Rule",
            RootName = "Rules",
            ContextKey = "rule",
            CsvPath = @"..\..\..\..\..\..\Cards\Rules\Argumentum Rules - Cards.csv",
            ImportPath = "", // pas de fixture d'époque — bootstrap de racine (PR 4)
            ExportPath = @"..\..\..\Data\Dnn2sxc\2sxcContentExport_Argumentum_Rules_0.0.1.xml",
            Cultures =
            {
                new Dnn2sxcCultureConfig
                {
                    Culture = "en-US",
                    TitleExpression = @"{rule.Pk}",
                    DescriptionExpression = @"{rule.Text_en}",
                    ExampleExpression = "",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "fr-FR",
                    TitleExpression = @"{rule.Pk}",
                    DescriptionExpression = @"{rule.Text}",
                    ExampleExpression = "",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "ru-RU",
                    TitleExpression = @"{rule.Pk}",
                    DescriptionExpression = @"{rule.Text_ru}",
                    ExampleExpression = "",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "pt-BR",
                    TitleExpression = @"{rule.Pk}",
                    DescriptionExpression = @"{rule.Text_pt}",
                    ExampleExpression = "",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "es-ES",
                    TitleExpression = @"{rule.Pk}",
                    DescriptionExpression = @"{rule.Text_es}",
                    ExampleExpression = "",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "ar-SA",
                    TitleExpression = @"{rule.Pk}",
                    DescriptionExpression = @"{rule.Text_ar}",
                    ExampleExpression = "",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "fa-IR",
                    TitleExpression = @"{rule.Pk}",
                    DescriptionExpression = @"{rule.Text_fa}",
                    ExampleExpression = "",
                },
                new Dnn2sxcCultureConfig
                {
                    Culture = "zh-CN",
                    TitleExpression = @"{rule.Pk}",
                    DescriptionExpression = @"{rule.Text_zh}",
                    ExampleExpression = "",
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
            if (!string.IsNullOrEmpty(Scenarii?.CsvPath))
            {
                Dnn2sxcTaxonomyExporter.Export(Scenarii, BuildScenariiRows(Scenarii.CsvPath));
            }
            if (!string.IsNullOrEmpty(Rules?.CsvPath))
            {
                // Plate : pas de colonne path — l'ordre du CSV devient le chemin (« 1 »…« 15 »),
                // chaque rangée enfant directe de la racine.
                var index = 0;
                Dnn2sxcTaxonomyExporter.Export(Rules,
                    Rule.Load(Rules.CsvPath).Select(r => ((++index).ToString(System.Globalization.CultureInfo.InvariantCulture), (object)r)));
            }
        }

        /// <summary>
        /// Rangées Scenarii pour le moteur : les nœuds intermédiaires d'abord (7 catégories puis
        /// 21 sous-catégories, synthétisés depuis la première rangée de chaque groupe), puis les
        /// 167 feuilles — le moteur résout chaque parent au fil de l'eau, l'ordre parents-avant-
        /// enfants est donc porteur.
        /// </summary>
        private static IEnumerable<(string Path, object Entity)> BuildScenariiRows(string csvPath)
        {
            var scenarii = Scenario.Load(csvPath);
            var rows = new List<(string Path, object Entity)>();
            foreach (var group in scenarii.GroupBy(s => s.path.Split('.')[0]))
            {
                var first = group.First();
                rows.Add((group.Key, GroupRow(first, subcategory: false)));
            }
            foreach (var group in scenarii.GroupBy(s => s.path.Substring(0, s.path.LastIndexOf('.'))))
            {
                var first = group.First();
                rows.Add((group.Key, GroupRow(first, subcategory: true)));
            }
            rows.AddRange(scenarii.Select(s => (s.path, (object)s)));
            return rows;
        }

        /// <summary>
        /// Rangée de groupe : un groupe ne porte QUE des titres (ni contexte ni enjeu). Une seule
        /// table de sélection pour les 8 langues — <paramref name="subcategory"/> choisit la colonne
        /// plutôt que de dupliquer seize paramètres.
        /// </summary>
        private static Dnn2sxcSyntheticGroupRow GroupRow(Scenario source, bool subcategory)
        {
            return new Dnn2sxcSyntheticGroupRow
            {
                titre = subcategory ? source.sous_categorie : source.categorie,
                title = subcategory ? source.subcategory : source.category,
                title_ru = subcategory ? source.subcategory_ru : source.category_ru,
                title_pt = subcategory ? source.subcategory_pt : source.category_pt,
                title_es = subcategory ? source.subcategory_es : source.category_es,
                title_ar = subcategory ? source.subcategory_ar : source.category_ar,
                title_fa = subcategory ? source.subcategory_fa : source.category_fa,
                title_zh = subcategory ? source.subcategory_zh : source.category_zh,
            };
        }
    }
}
