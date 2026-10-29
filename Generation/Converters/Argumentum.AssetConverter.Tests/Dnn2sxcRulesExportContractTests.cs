using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Argumentum.AssetConverter.Dnn2sxc;
using Argumentum.AssetConverter.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// #136 partie 1, PR 4 — contrat de l'export Dnn2sxc RULES, qui referme le périmètre (4/4
    /// taxonomies). Spécificités mesurées : 15 rangées <b>plates</b> (aucune colonne path — les
    /// chemins « 1 »…« 15 » sont synthétisés depuis l'ordre du CSV, chaque rangée enfant directe
    /// de la racine), texte intégral en <b>markdown</b> par langue, et la seule taxonomie couvrant
    /// les <b>8 langues</b> du corpus (fr/en/ru/pt/es/ar/fa/zh — l'entité <c>Rule</c> porte les 8
    /// colonnes). Total attendu par culture : 1 racine + 15 = 16.
    /// </summary>
    /// <remarks>
    /// <para><b>Projection</b> : Pk → Name (stable et unique — le titre d'affichage est un en-tête
    /// markdown DANS le texte, à rendre côté site), texte brut → Description, Example vide
    /// (l'entité n'a pas de 3ᵉ champ). TEXTE BRUT, pas HtmlEncodé : voir le fait
    /// <c>Exported_Text_Carries_Raw_Accents…</c>.</para>
    /// <para><b>Ce que cette suite ne prouve pas</b> : l'acceptation par 2sxc v21 sur le portail
    /// (partie 2 de #136), ni le rendu du markdown côté site.</para>
    /// </remarks>
    public class Dnn2sxcRulesExportContractTests
    {
        private readonly ITestOutputHelper _output;

        public Dnn2sxcRulesExportContractTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private static string Repo => TestRepoRoot.Find();

        private static string CsvPath => Path.Combine(Repo,
            "Cards", "Rules", "Argumentum Rules - Cards.csv");

        private static string MasterCulture => new Dnn2sxcConfig().Rules.Cultures[0].Culture;

        private static string Val(XElement entity, string childName)
        {
            return entity.Element(childName)?.Value ?? string.Empty;
        }

        /// <summary>Config Rules seule (les 3 autres taxonomies désactivées), export temporaire.</summary>
        private static Dnn2sxcConfig NewConfig(string exportPath)
        {
            return new Dnn2sxcConfig
            {
                Fallacies = new Dnn2sxcTaxonomyExportConfig(), // désactivée : CsvPath vide
                Virtues = new Dnn2sxcTaxonomyExportConfig(),   // désactivée : CsvPath vide
                Scenarii = new Dnn2sxcTaxonomyExportConfig(),  // désactivée : CsvPath vide
                Rules = new Dnn2sxcTaxonomyExportConfig
                {
                    EntityTypeName = "Rule",
                    RootName = "Rules",
                    ContextKey = "rule",
                    CsvPath = CsvPath,
                    ImportPath = "", // pas de fixture d'époque — bootstrap de racine
                    ExportPath = exportPath,
                    Cultures = new Dnn2sxcConfig().Rules.Cultures,
                },
            };
        }

        private static string TempExportPath()
        {
            return Path.Combine(Path.GetTempPath(), $"dnn2sxc-rules-{Guid.NewGuid():N}.xml");
        }

        private static System.Collections.Generic.List<XElement> ExportEntities(XDocument document)
        {
            return document.Root!.Elements("Entity").ToList();
        }

        [Fact]
        public void Regenerated_Export_Is_Valid_Xml_Even_Over_A_Longer_Previous_File()
        {
            var exportPath = TempExportPath();
            try
            {
                File.WriteAllText(exportPath, new string('x', 2_000_000)); // contrôle inverse troncature

                NewConfig(exportPath).Apply();

                var loaded = XDocument.Load(exportPath); // lève si invalide — c'est le fait
                loaded.Root!.Name.LocalName.Should().Be("SexyContentData");
            }
            finally
            {
                File.Delete(exportPath);
            }
        }

        [Fact]
        public void Every_Culture_Carries_Root_Plus_All_Flat_Rules()
        {
            var exportPath = TempExportPath();
            try
            {
                NewConfig(exportPath).Apply();

                var rules = Rule.Load(CsvPath).Count;
                var expectedPerCulture = 1 + rules;

                var byCulture = ExportEntities(XDocument.Load(exportPath))
                    .GroupBy(e => Val(e, "Language"))
                    .ToDictionary(g => g.Key, g => g.ToList());

                // La seule taxonomie du corpus couvrant les 8 langues.
                var scopeCultures = new[]
                {
                    "en-US", "fr-FR", "ru-RU", "pt-BR", "es-ES", "ar-SA", "fa-IR", "zh-CN",
                };

                _output.WriteLine($"CSV: {rules} rangées ; cultures: {string.Join(", ", byCulture.Select(p => $"{p.Key}={p.Value.Count}"))}");

                byCulture.Keys.Should().BeEquivalentTo(scopeCultures,
                    "aucune culture ne préexiste (pas de fixture) : exactement les 8 du corpus Rules");
                foreach (var culture in scopeCultures)
                {
                    byCulture[culture].Count.Should().Be(expectedPerCulture,
                        $"{culture} : racine + {rules} rangées plates");
                }
            }
            finally
            {
                File.Delete(exportPath);
            }
        }

        [Fact]
        public void Every_Rule_Parents_The_Root_And_The_Root_Has_No_Parent()
        {
            // Structure plate : les 15 chemins synthétiques « 1 »…« 15 » sont tous enfants directs
            // de la racine — un Parent ailleurs casserait l'arbre à l'import 2sxc.
            var exportPath = TempExportPath();
            try
            {
                NewConfig(exportPath).Apply();

                var entities = ExportEntities(XDocument.Load(exportPath));
                var guids = entities.Select(e => Val(e, "Guid")).ToHashSet();
                guids.Should().NotBeEmpty("l'export doit porter des entités avant qu'on y lise la hiérarchie");

                var roots = entities.Where(e => string.IsNullOrEmpty(Val(e, "Parent"))).ToList();
                var rootGuid = Val(roots[0], "Guid");

                foreach (var culture in new[] { MasterCulture, "fr-FR", "zh-CN" })
                {
                    var inCulture = entities.Where(e => Val(e, "Language") == culture).ToList();
                    var cultureRoot = inCulture.Single(e => string.IsNullOrEmpty(Val(e, "Parent")));
                    var leaves = inCulture.Where(e => !string.IsNullOrEmpty(Val(e, "Parent"))).ToList();
                    leaves.Count.Should().Be(Rule.Load(CsvPath).Count,
                        $"{culture} : toutes les rangées, enfants de la racine");
                    leaves.Should().OnlyContain(e => Val(e, "Parent") == rootGuid,
                        $"{culture} : structure plate — chaque rangée pointe la racine, pas un autre nœud");
                }
            }
            finally
            {
                File.Delete(exportPath);
            }
        }

        [Fact]
        public void Two_Runs_Produce_A_Byte_Identical_Export()
        {
            var first = TempExportPath();
            var second = TempExportPath();
            try
            {
                NewConfig(first).Apply();
                NewConfig(second).Apply();

                File.ReadAllBytes(first).Should().Equal(File.ReadAllBytes(second),
                    "Guid déterministes + ordre CSV stable");
            }
            finally
            {
                File.Delete(first);
                File.Delete(second);
            }
        }

        [Fact]
        public void Root_Is_Named_From_Config_And_Rules_Carry_Pk_Names_And_Text()
        {
            var exportPath = TempExportPath();
            try
            {
                NewConfig(exportPath).Apply();

                var entities = ExportEntities(XDocument.Load(exportPath));

                // Pas de rangée CSV « 0 » : la racine garde le nom de la config, dans TOUTES les
                // cultures (le correctif bootstrap PR 3 — mesuré 196 vs 195 sur Scenarii).
                foreach (var culture in new[] { MasterCulture, "fr-FR", "ar-SA" })
                {
                    var root = entities.Single(e => Val(e, "Language") == culture
                        && string.IsNullOrEmpty(Val(e, "Parent")));
                    Val(root, "Name").Should().Be(new Dnn2sxcConfig().Rules.RootName,
                        $"{culture} : racine nommée par config");
                }

                // Témoins verbatim du corpus : le Pk « Rules_01 » comme Name, et le texte markdown
                // de la première règle (HtmlEncodé) comme Description française.
                var frRules = entities.Where(e => Val(e, "Language") == "fr-FR"
                    && !string.IsNullOrEmpty(Val(e, "Parent"))).ToList();
                frRules.Should().Contain(e => Val(e, "Name") == "Rules_01",
                    "le Name de la 1ʳᵉ rangée est son Pk — stable et indépendant de la culture");
                frRules.Should().Contain(e => Val(e, "Description").Contains("L’école des menteurs"),
                    "témoin verbatim du texte markdown français de la 1ʳᵉ règle (apostrophe typographique U+2019 et é brut, mesurés au CSV)");

                // Le témoin traduit : la même règle porte bien le texte EN, pas le FR.
                var enRules = entities.Where(e => Val(e, "Language") == "en-US"
                    && !string.IsNullOrEmpty(Val(e, "Parent"))).ToList();
                enRules.Should().Contain(e => Val(e, "Name") == "Rules_01"
                    && Val(e, "Description").Contains("school of liars"),
                    "témoin EN verbatim — chaque culture porte son propre texte");
            }
            finally
            {
                File.Delete(exportPath);
            }
        }

        [Fact]
        public void Every_Entity_Of_Every_Culture_Carries_A_NonEmpty_Description()
        {
            // Rules est la taxonomie du texte intégral : la donnée qui importe est la Description
            // (le markdown). Un Name vide serait déjà couvert par Pk ; une Description vide, non.
            var exportPath = TempExportPath();
            try
            {
                NewConfig(exportPath).Apply();

                var entities = ExportEntities(XDocument.Load(exportPath));
                var leaves = entities.Where(e => !string.IsNullOrEmpty(Val(e, "Parent"))).ToList();

                leaves.Where(e => string.IsNullOrEmpty(Val(e, "Description")))
                    .Should().BeEmpty("chaque règle, chaque culture, porte son texte intégral");
            }
            finally
            {
                File.Delete(exportPath);
            }
        }

        [Fact]
        public void Exported_Text_Carries_Raw_Accents_Not_Numeric_Entity_Double_Encoding()
        {
            // Défaut mesuré PR 4 (hérité du code 2023, présent dans les PR 1-3) : HttpUtility.HtmlEncode
            // encode tout non-ASCII en références numériques (« é » → « &#233; »), qui deviennent
            // double-échappées à la sérialisation (« &amp;#233; ») puis une string LITTÉRALE
            // « &#233; » à l'import 2sxc — rendu cassé ou dépendant du template. Le fixture 2021
            // réellement importé avec succès porte ses accents BRUTS (Name>Argument bâclé<) : le
            // brut est la convention qui marche. Les expressions n'encodent donc plus ; le
            // XmlSerializer échappe seul & < > à la couche XML.
            var exportPath = TempExportPath();
            try
            {
                NewConfig(exportPath).Apply();

                var entities = ExportEntities(XDocument.Load(exportPath));

                entities.SelectMany(e => new[] { Val(e, "Name"), Val(e, "Description"), Val(e, "Example") })
                    .Where(v => v.Contains("&#"))
                    .Should().BeEmpty("aucune valeur exportée ne doit porter de référence numérique littérale — accents bruts (convention du fixture 2021 importé avec succès)");

                // Témoin positif : l'accent survit brut à travers l'aller-retour XML.
                entities.Should().Contain(e => Val(e, "Language") == "fr-FR"
                    && Val(e, "Description").Contains("Règles du jeu"),
                    "l'accent du corpus survit brut dans l'export");
            }
            finally
            {
                File.Delete(exportPath);
            }
        }
    }
}
