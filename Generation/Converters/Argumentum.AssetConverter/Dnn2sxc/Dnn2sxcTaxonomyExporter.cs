using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Argumentum.AssetConverter.Dnn2sxc
{
    /// <summary>
    /// Réglages d'export d'une taxonomie vers le format 2sxc (#136 partie 1). Le CSV n'est pas lu
    /// ici : la taxonomie fournit ses rangées typées à <see cref="Dnn2sxcTaxonomyExporter.Export"/>,
    /// les expressions par culture interpolant l'entité sous <see cref="ContextKey"/>
    /// (ex. <c>{fallacy.TextFr}</c>, <c>{virtue.TitleFr}</c>).
    /// </summary>
    public class Dnn2sxcTaxonomyExportConfig
    {
        /// <summary>Chemin du CSV source (lu par la taxonomie, pas par le moteur).</summary>
        public string CsvPath { get; set; }

        /// <summary>Valeur de l'attribut <c>Type</c> des entités exportées (ex. <c>Fallacy</c>).</summary>
        public string EntityTypeName { get; set; }

        /// <summary>Nom de l'entité racine (la rangée CSV <c>path</c> = "0" la met à jour).</summary>
        public string RootName { get; set; }

        /// <summary>Clé d'interpolation des expressions (ex. <c>fallacy</c>, <c>virtue</c>).</summary>
        public string ContextKey { get; set; }

        /// <summary>
        /// XML d'import d'époque (2021), optionnel : préserve les Guid existants, la hiérarchie et
        /// le contenu des champs <b>dont le CSV est muet</b> (repli). Le CSV fait foi partout où
        /// il porte une valeur — traductions comprises, comme pour la culture maîtresse.
        /// Vide ou absent ⇒ bootstrap de racine depuis la rangée CSV "0".
        /// </summary>
        public string ImportPath { get; set; }

        public string ExportPath { get; set; }

        /// <summary>
        /// Cultures exportées — la <b>première</b> est la culture <i>maîtresse</i> (porte la
        /// hiérarchie ; les autres attachent leurs traductions par Guid partagé).
        /// </summary>
        public List<Dnn2sxcCultureConfig> Cultures { get; set; } = new List<Dnn2sxcCultureConfig>();
    }

    /// <summary>
    /// Moteur d'export 2sxc partagé par les taxonomies (#136 partie 1). Corps historique de
    /// <c>Dnn2sxcConfig.Apply</c> (Fallacies, PR #2047), généralisé : chaque taxonomie fournit ses
    /// rangées (chemin hiérarchique + entité interpolable), ses expressions par culture et ses
    /// chemins — le comportement est inchangé pour Fallacies (la régénération doit rester
    /// byte-identique à l'export committé, gardé par test).
    /// </summary>
    /// <remarks>
    /// <para><b>Guid</b> : les Guid du fixture d'import sont préservés (continuité base live) ; les
    /// chemins nouveaux reçoivent un Guid déterministe dérivé du chemin — deux régénérations
    /// produisent le même export, et un ré-import 2sxc met à jour au lieu de dupliquer.</para>
    /// <para><b>Troncature</b> : export via <see cref="File.Create(string)"/> (tronquant). L'ancien
    /// <c>File.OpenWrite</c> a laissé l'export 2023 committé corrompu (octets résiduels du fichier
    /// précédent après <c>&lt;/SexyContentData&gt;</c>).</para>
    /// <para><b>Racine</b> : sans fixture, la racine est créée (puis mise à jour par la rangée CSV
    /// "0" comme toute rangée) — l'ancien code faisait NRE sur <c>root.Guid</c>.</para>
    /// </remarks>
    public static class Dnn2sxcTaxonomyExporter
    {
        public static void Export(Dnn2sxcTaxonomyExportConfig config, IEnumerable<(string Path, object Entity)> rows)
        {
            var sexyContentSeralizer = new XmlSerializer(typeof(SexyContentData), new[] { typeof(Entity) });
            SexyContentData sexyContentData;
            if (!string.IsNullOrEmpty(config.ImportPath) && File.Exists(config.ImportPath))
            {
                using (var tr = File.OpenText(config.ImportPath))
                {
                    sexyContentData = (SexyContentData)sexyContentSeralizer.Deserialize(tr);
                }
            }
            else
            {
                // Bootstrap sans fixture (#136 partie 1) : une taxonomie sans XML d'import 2021 part
                // d'un export vide — la racine est créée depuis la rangée CSV "0".
                sexyContentData = new SexyContentData { Entity = new List<Entity>() };
            }

            var masterCulture = config.Cultures[0].Culture;
            var translationCultures = config.Cultures.Skip(1).ToList();

            var rowsList = rows.ToList();
            var entitiesByGuid = new Dictionary<string, (Entity entity, string path)>(rowsList.Count);
            var entitiesByPath = new Dictionary<string, (Entity main, IList<Entity> children)>(rowsList.Count);
            var translationsByCulture = new Dictionary<string, Dictionary<Entity, Entity>>(
                config.Cultures.Select(c => new KeyValuePair<string, Dictionary<Entity, Entity>>(c.Culture, new Dictionary<Entity, Entity>())));

            string entityTypeName = config.EntityTypeName;

            //Load existing 2sxc data

            // Traductions créées par le bootstrap (pas par le fixture) : la boucle CSV les met à
            // jour complètement, champ vide compris. Les traductions héritées du fixture suivent la
            // même règle (« le CSV fait foi ») mais gardent leur valeur sur un champ dont le CSV est
            // muet — c'est la seule différence entre les deux familles.
            var syntheticTranslations = new HashSet<Entity>();

            Entity root = null;
            int familyCounter = 0;
            foreach (var existingEntity in sexyContentData.Entity.Where(e => e.Type == entityTypeName))
            {
                string localPath;
                if (existingEntity.Language == masterCulture)
                {
                    if (string.IsNullOrEmpty(existingEntity.Parent))
                    {
                        root = existingEntity;
                        localPath = "0";
                    }
                    else
                    {
                        var parent = entitiesByGuid[existingEntity.Parent];
                        var children = entitiesByPath[parent.path].children;
                        if (existingEntity.Parent == root.Guid)
                        {
                            familyCounter++;
                            localPath = familyCounter.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            localPath = $"{parent.path}.{children.Count + 1}";
                        }
                        children.Add(existingEntity);
                    }

                    entitiesByPath[localPath] = (existingEntity, new List<Entity>());
                    entitiesByGuid[existingEntity.Guid] = (existingEntity, localPath);

                }
                else if (translationsByCulture.TryGetValue(existingEntity.Language, out var translationsOfCulture)
                    && entitiesByGuid.TryGetValue(existingEntity.Guid, out var translated))
                {
                    translationsOfCulture[translated.entity] = existingEntity;
                }
            }

            if (root == null)
            {
                // Bootstrap de racine : nécessaire dès qu'une taxonomie n'a pas de fixture 2021
                // (Virtues/Scenarii/Rules) — l'ancien code faisait NRE sur root.Guid à cette absence.
                root = new Entity
                {
                    Guid = DeterministicGuid(entityTypeName, "0").ToString(),
                    Language = masterCulture,
                    Name = config.RootName,
                    Type = entityTypeName,
                };
                entitiesByPath["0"] = (root, new List<Entity>());
                entitiesByGuid[root.Guid] = (root, "0");
                sexyContentData.Entity.Add(root);

                // La racine existe dans TOUTES les cultures, comme toute entité — sinon une
                // taxonomie sans rangée CSV "0" (Scenarii) exporte sa racine uniquement en
                // culture maîtresse et chaque culture traduite manque une entité (mesuré : 196
                // vs 195). Si le CSV porte une rangée "0", la boucle ci-dessous la met à jour
                // (traductions synthétiques : mise à jour complète, contrairement au fixture).
                foreach (var culture in translationCultures)
                {
                    var translatedRoot = new Entity
                    {
                        Guid = root.Guid,
                        Language = culture.Culture,
                        Name = config.RootName,
                        Type = entityTypeName,
                    };
                    translationsByCulture[culture.Culture][root] = translatedRoot;
                    syntheticTranslations.Add(translatedRoot);
                    sexyContentData.Entity.Add(translatedRoot);
                }
            }

            //Add/update with csv taxonomy

            foreach (var row in rowsList)
            {

                var localPath = row.Path;

                (Entity main, IList<Entity> children) node;
                if (!entitiesByPath.TryGetValue(localPath, out node))
                {
                    node = (new Entity(), new List<Entity>());
                    // Guid déterministe : l'export committé doit être reproductible d'une regen à l'autre.
                    node.main.Guid = DeterministicGuid(entityTypeName, localPath).ToString();
                    entitiesByPath[localPath] = node;
                    entitiesByGuid[node.main.Guid] = (node.main, localPath);
                    sexyContentData.Entity.Add(node.main);
                }

                node.main.Name = Evaluate(config.ContextKey, config.Cultures[0].TitleExpression, row.Entity);
                node.main.Description = Evaluate(config.ContextKey, config.Cultures[0].DescriptionExpression, row.Entity);
                node.main.Example = Evaluate(config.ContextKey, config.Cultures[0].ExampleExpression, row.Entity);

                node.main.Type = entityTypeName;

                node.main.Language = masterCulture;

                var lastDotIndex = localPath.LastIndexOf('.');
                int familyNb;
                if (lastDotIndex > -1)
                {
                    var parentPath = localPath.Substring(0, lastDotIndex);
                    var parentNode = entitiesByPath[parentPath];
                    node.main.Parent = parentNode.main.Guid;
                }
                else
                {
                    familyNb = int.Parse(localPath);
                    if (familyNb > 0)
                    {
                        node.main.Parent = root.Guid;
                    }
                }

                foreach (var culture in translationCultures)
                {
                    var translationsOfCulture = translationsByCulture[culture.Culture];
                    if (!translationsOfCulture.TryGetValue(node.main, out var translated))
                    {
                        translated = new Entity();

                        translationsOfCulture[node.main] = translated;
                        sexyContentData.Entity.Add(translated);

                        translated.Language = culture.Culture;
                        translated.Guid = node.main.Guid;
                        translated.Parent = node.main.Parent;
                        translated.Type = entityTypeName;
                        translated.Name = Evaluate(config.ContextKey, culture.TitleExpression, row.Entity);
                        translated.Description = Evaluate(config.ContextKey, culture.DescriptionExpression, row.Entity);
                        translated.Example = Evaluate(config.ContextKey, culture.ExampleExpression, row.Entity);
                    }
                    else if (syntheticTranslations.Contains(translated))
                    {
                        // Traduction synthétique du bootstrap : le CSV fait foi — mise à jour
                        // complète (la racine de Virtues reçoit ainsi « Argument valable »).
                        translated.Name = Evaluate(config.ContextKey, culture.TitleExpression, row.Entity);
                        translated.Description = Evaluate(config.ContextKey, culture.DescriptionExpression, row.Entity);
                        translated.Example = Evaluate(config.ContextKey, culture.ExampleExpression, row.Entity);
                    }
                    else
                    {
                        // Le CSV fait foi partout où il porte une valeur — traductions comprises : la
                        // culture maîtresse est déjà réécrite rangée par rangée, et une traduction
                        // figée sur le fixture 2021 servait un texte que le corpus ne portait plus
                        // (mesuré : PK 2 « Argument bâclé », description 2021 ≠ desc_fr du CSV).
                        // Le fixture ne subsiste qu'en REPLI pour les champs dont le CSV est muet —
                        // sans ce repli, une traduction qu'il porte seul serait effacée. Un repli
                        // n'est jamais le marqueur de vide 2sxc (« [] ») : il s'afficherait tel quel.
                        translated.Name = PreferCsv(Evaluate(config.ContextKey, culture.TitleExpression, row.Entity), translated.Name);
                        translated.Description = PreferCsv(Evaluate(config.ContextKey, culture.DescriptionExpression, row.Entity), translated.Description);
                        translated.Example = PreferCsv(Evaluate(config.ContextKey, culture.ExampleExpression, row.Entity), translated.Example);
                    }
                }

            }


            //Le marqueur de vide vaut absence sur TOUS les champs texte, pas seulement les trois que la
            //boucle CSV réécrit. Card/Difficulty/Date viennent du fixture 2021 verbatim — aucune rangée
            //CSV ne les alimente, donc PreferCsv ne les voyait jamais (mesuré 29/10 : 17 « [] » sur
            //6 entités Fallacies, 3 champs ; un littéral s'affiche tel quel sur le site).
            StripEmptyMarkers(sexyContentData);

            //Serialize to xml — File.Create TRUNCATE : l'ancien File.OpenWrite ne tronquait pas,
            //et l'export 2023 committé en porte la corruption (octets résiduels après </SexyContentData>).
            using (var stream = File.Create(config.ExportPath))
            {
                sexyContentSeralizer.Serialize(stream, sexyContentData);
            }

        }

        private static string Evaluate(string contextKey, string expression, object entity)
        {
            return expression.Interpolate(new Dictionary<string, object>() { { contextKey, entity } });
        }

        /// <summary>
        /// Marqueur de valeur vide du dialecte 2sxc : le fixture 2021 sérialise un champ texte sans
        /// valeur en <c>[]</c> (liste vide). Ce n'est pas du contenu — exporté tel quel il s'affiche
        /// littéralement sur le site (mesuré : deux <c>Example</c> fr-FR du fixture).
        /// </summary>
        private const string EmptyMarker = "[]";

        /// <summary>
        /// Le CSV fait foi dès qu'il porte une valeur ; sinon la valeur en place (fixture) est
        /// conservée, sauf si c'est le marqueur de vide — qui vaut absence, donc chaîne vide.
        /// </summary>
        private static string PreferCsv(string fromCsv, string current)
        {
            if (!string.IsNullOrEmpty(fromCsv))
            {
                return fromCsv;
            }
            return current == EmptyMarker ? string.Empty : current;
        }

        /// <summary>
        /// Champs d'identité de l'entité : un « [] » y serait une incohérence de structure, pas un
        /// contenu vide — on n'y touche pas. Les autres membres <c>string</c> sont du texte et suivent
        /// la règle du marqueur, y compris ceux qu'aucune expression CSV n'alimente.
        /// </summary>
        private static readonly string[] IdentityMembers =
        {
            nameof(Entity.Guid), nameof(Entity.Language), nameof(Entity.Parent), nameof(Entity.Type)
        };

        /// <summary>
        /// Applique <see cref="EmptyMarker"/> à <b>tous</b> les champs texte avant sérialisation.
        /// La liste des champs est <b>dérivée</b> de <see cref="Entity"/> par réflexion : un champ
        /// ajouté plus tard entre dans la règle sans qu'on y pense, seul un membre à déclarer comme
        /// identité échappe (cf. <see cref="IdentityMembers"/>).
        /// </summary>
        private static void StripEmptyMarkers(SexyContentData data)
        {
            var textProperties = typeof(Entity)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string) && !IdentityMembers.Contains(p.Name, StringComparer.Ordinal))
                .ToList();

            foreach (var entity in data.Entity)
            {
                foreach (var property in textProperties)
                {
                    if ((string)property.GetValue(entity) == EmptyMarker)
                    {
                        property.SetValue(entity, string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Guid stable dérivé du chemin (MD5 de <c>Dnn2sxc|&lt;type&gt;|&lt;chemin&gt;</c>) — pas de la
        /// cryptographie, une identité : deux régénérations assignent le même Guid au même chemin, et
        /// un ré-import 2sxc met à jour au lieu de dupliquer.
        /// </summary>
        private static Guid DeterministicGuid(string entityTypeName, string localPath)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes($"Dnn2sxc|{entityTypeName}|{localPath}"));
                return new Guid(hash);
            }
        }
    }
}
