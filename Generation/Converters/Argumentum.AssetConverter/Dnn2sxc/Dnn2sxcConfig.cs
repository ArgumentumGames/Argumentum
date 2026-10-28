using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter.Dnn2sxc
{
    /// <summary>
    /// Génération des entités 2sxc depuis le corpus CSV (#136 partie 1).
    /// </summary>
    /// <remarks>
    /// <para><b>Cultures</b> : la <b>première</b> entrée de <see cref="Cultures"/> est la culture
    /// <i>maîtresse</i> — celle qui porte la hiérarchie (l'arbre est construit sur ses entités, les
    /// autres cultures attachent leurs traductions par Guid partagé). Historiquement en dur
    /// (<c>en-US</c> maître, <c>fr-FR</c> traduction) ; la liste couvre désormais le scope de #136
    /// (fr/en/ru/pt) et l'extension aux 8 langues du corpus = une entrée de config.</para>
    /// <para><b>Guid</b> : les Guid des entités du XML d'import 2021 sont préservés (continuité avec
    /// la base live) ; les chemins nouveaux reçoivent un Guid <see cref="DeterministicGuid"/> dérivé
    /// du chemin, pour que deux régénérations produisent le même export (un <c>Guid.NewGuid()</c>
    /// rendrait chaque regen non reproductible et ferait dupliquer les entités à l'import 2sxc).</para>
    /// <para><b>Troncature</b> : l'export s'écrit via <see cref="File.Create(string)"/> qui tronque.
    /// L'ancien <c>File.OpenWrite</c> ne tronque pas — et l'export committé de 2023 en est la preuve
    /// vivante : la sérialisation s'y termine ligne 15357 sur <c>&lt;/SexyContentData&gt;</c> suivi des
    /// octets résiduels du fichier précédent, plus long (<c>&lt;/Example&gt;…&lt;/Entity&gt;</c>) —
    /// le fichier du dépôt est un XML invalide.</para>
    /// <para><b>Config morte retirée</b> : <c>TitleFuncFr</c>/<c>LinkFuncFr</c>/<c>LinkFuncEn</c>
    /// étaient définies et jamais appelées — l'entité fr-FR ne recevait jamais de <c>Name</c> (le
    /// fixture 2021 porte pourtant des Name traduits : « Argument fallacieux ») et <see cref="Entity"/>
    /// n'a aucun champ Link. Les traductions reçoivent désormais leur Name ; les expressions Link
    /// disparaissent.</para>
    /// </remarks>
    public class Dnn2sxcConfig
    {


        /// <summary>
        /// Corpus vivant (même fichier que le DataSet Fallacies du pipeline) — l'ancienne valeur
        /// pointait le snapshot 2021 de <c>Data/Dnn2sxc/</c>, dont l'entête n'est même plus lisible
        /// par le ClassMap courant (mesuré : <c>ValidateHeader</c> lève). Ce snapshot reste dans le
        /// dépôt comme pièce d'époque, aux côtés du fixture XML 2021.
        /// </summary>
        public string CsvPathFallacies { get; set; } = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv";
        public string Xml2sxcImportPathFallacies { get; set; } = @"..\..\..\Data\Dnn2sxc\2sxc Fallacy  Data 20210212131131.xml";
        public string Xml2sxcExportPathFallacies { get; set; } = @"..\..\..\Data\Dnn2sxc\2sxcContentExport_Argumentum_Fallacies_0.0.1.xml";

        /// <summary>Nom de l'entité racine (la rangée CSV <c>path</c> = "0" la met à jour).</summary>
        public string RootName { get; set; } = "Fallacies";

        /// <summary>Valeur de l'attribut <c>Type</c> des entités exportées.</summary>
        public string EntityTypeName { get; set; } = "Fallacy";

        /// <summary>
        /// Cultures exportées — la première est la culture maîtresse (voir les remarks de la classe).
        /// Codes alignés sur le portail (l'interop #457 T1 liste <c>pt-BR</c>, pas <c>pt-PT</c>).
        /// ⚠️ Piège mesuré pour ru : le champ CSV s'appelle <c>Exampleru</c> (casse, <c>Fallacy.cs</c>),
        /// pas <c>ExampleRu</c>.
        /// </summary>
        public List<Dnn2sxcCultureConfig> Cultures { get; set; } = new List<Dnn2sxcCultureConfig>
        {
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
        };

        private Func<Fallacy, string> TitleFunc(Dnn2sxcCultureConfig culture)
        {
            return fallacy => culture.TitleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } });
        }

        private Func<Fallacy, string> DescFunc(Dnn2sxcCultureConfig culture)
        {
            return fallacy => culture.DescriptionExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } });
        }

        private Func<Fallacy, string> ExampleFunc(Dnn2sxcCultureConfig culture)
        {
            return fallacy => culture.ExampleExpression.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } });
        }


        public void Apply()
        {

            var sexyContentSeralizer = new XmlSerializer(typeof(SexyContentData), new []{ typeof(Entity) });
            SexyContentData sexyContentData;
            if (!string.IsNullOrEmpty(Xml2sxcImportPathFallacies) && File.Exists(Xml2sxcImportPathFallacies))
            {
                using (var tr = System.IO.File.OpenText(Xml2sxcImportPathFallacies))
                {
                    sexyContentData = (SexyContentData) sexyContentSeralizer.Deserialize(tr);
                }
            }
            else
            {
                // Bootstrap sans fixture (#136 partie 1) : une taxonomie sans XML d'import 2021 part
                // d'un export vide — la racine est créée depuis la rangée CSV "0".
                sexyContentData = new SexyContentData { Entity = new List<Entity>() };
            }

            var fallacies = Fallacy.Load(CsvPathFallacies);

            var masterCulture = Cultures[0].Culture;
            var translationCultures = Cultures.Skip(1).ToList();

            var fallaciesbyGuid = new Dictionary<string, (Entity entity, string path)>(fallacies.Count());
            var fallaciesByPath = new Dictionary<string, (Entity main, IList<Entity> children)>(fallacies.Count());
            var translationsByCulture = new Dictionary<string, Dictionary<Entity, Entity>>(
                Cultures.Select(c => new KeyValuePair<string, Dictionary<Entity, Entity>>(c.Culture, new Dictionary<Entity, Entity>())));

            string fallacy2SxcType = EntityTypeName;

            //Load existing 2sxc data

            Entity root = null;
            int familyCounter = 0;
            foreach (var existingEntity in sexyContentData.Entity.Where(e=>e.Type == fallacy2SxcType))
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
                        var parent = fallaciesbyGuid[existingEntity.Parent];
                        var children = fallaciesByPath[parent.path].children;
                        if (existingEntity.Parent == root.Guid)
                        {
                            familyCounter++;
                            localPath = familyCounter.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            localPath = $"{parent.path}.{children.Count+1}";
                        }
                        children.Add(existingEntity);
                    }

                    fallaciesByPath[localPath] = (existingEntity, new List<Entity>());
                    fallaciesbyGuid[existingEntity.Guid] = (existingEntity, localPath);

                }
                else if (translationsByCulture.TryGetValue(existingEntity.Language, out var translationsOfCulture)
                    && fallaciesbyGuid.TryGetValue(existingEntity.Guid, out var translated))
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
                    Guid = DeterministicGuid("0").ToString(),
                    Language = masterCulture,
                    Name = RootName,
                    Type = fallacy2SxcType,
                };
                fallaciesByPath["0"] = (root, new List<Entity>());
                fallaciesbyGuid[root.Guid] = (root, "0");
                sexyContentData.Entity.Add(root);
            }

            //Add/update with csv taxonomy

            foreach (var fallacy in fallacies)
            {

                var localPath = fallacy.Path;

                (Entity main, IList<Entity> children) fallacyNode;
                if (!fallaciesByPath.TryGetValue(localPath, out fallacyNode))
                {
                    fallacyNode = (new Entity(), new List<Entity>());
                    // Guid déterministe : l'export committé doit être reproductible d'une regen à l'autre.
                    fallacyNode.main.Guid = DeterministicGuid(localPath).ToString();
                    fallaciesByPath[localPath] = fallacyNode;
                    fallaciesbyGuid[fallacyNode.main.Guid] = (fallacyNode.main, localPath);
                    sexyContentData.Entity.Add(fallacyNode.main);
                }

                fallacyNode.main.Name = TitleFunc(Cultures[0])(fallacy);
                fallacyNode.main.Description = DescFunc(Cultures[0])(fallacy);
                fallacyNode.main.Example = ExampleFunc(Cultures[0])(fallacy);

                fallacyNode.main.Type = fallacy2SxcType;

                fallacyNode.main.Language = masterCulture;

                var lastDotIndex = localPath.LastIndexOf('.');
                int familyNb;
                if (lastDotIndex > -1)
                {
                    var parentPath = localPath.Substring(0, lastDotIndex);
                    var parentNode = fallaciesByPath[parentPath];
                    fallacyNode.main.Parent = parentNode.main.Guid;
                }
                else
                {
                    familyNb = int.Parse(localPath);
                    if (familyNb > 0)
                    {
                        fallacyNode.main.Parent = root.Guid;
                    }
                }

                foreach (var culture in translationCultures)
                {
                    var translationsOfCulture = translationsByCulture[culture.Culture];
                    if (! translationsOfCulture.TryGetValue(fallacyNode.main, out var translated))
                    {
                        translated = new Entity();

                        translationsOfCulture[fallacyNode.main] = translated;
                        sexyContentData.Entity.Add(translated);

                        translated.Language = culture.Culture;
                        translated.Guid = fallacyNode.main.Guid;
                        translated.Parent = fallacyNode.main.Parent;
                        translated.Type = fallacy2SxcType;
                        translated.Name = TitleFunc(culture)(fallacy);
                        translated.Description = DescFunc(culture)(fallacy);
                        translated.Example = ExampleFunc(culture)(fallacy);
                    }
                    else
                    {
                        // Comble les champs vides sans écraser le contenu porté par le fixture
                        // (convention mesurée : les fr-FR du fixture 2021 ont un Name traduit).
                        if (string.IsNullOrEmpty(translated.Name))
                        {
                            translated.Name = TitleFunc(culture)(fallacy);
                        }
                        if (string.IsNullOrEmpty(translated.Description))
                        {
                            translated.Description = DescFunc(culture)(fallacy);
                        }
                        if (string.IsNullOrEmpty(translated.Example))
                        {
                            translated.Example = ExampleFunc(culture)(fallacy);
                        }
                    }
                }

            }


            //Serialize to xml — File.Create TRUNCATE : l'ancien File.OpenWrite ne tronquait pas,
            //et l'export 2023 committé en porte la corruption (octets résiduels après </SexyContentData>).
            using (var tr = System.IO.File.Create(Xml2sxcExportPathFallacies))
            {
                sexyContentSeralizer.Serialize(tr, sexyContentData);
            }

        }

        /// <summary>
        /// Guid stable dérivé du chemin (MD5 de <c>Dnn2sxc|&lt;type&gt;|&lt;chemin&gt;</c>) — pas de la
        /// cryptographie, une identité : deux régénérations assignent le même Guid au même chemin, et
        /// un ré-import 2sxc met à jour au lieu de dupliquer.
        /// </summary>
        private Guid DeterministicGuid(string localPath)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes($"Dnn2sxc|{EntityTypeName}|{localPath}"));
                return new Guid(hash);
            }
        }
    }
}
