using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Argumentum.AssetConverter.Mindmapper;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;

namespace Argumentum.AssetConverter.Dnn2sxc
{
    public class Dnn2sxcConfig
    {


        public string CsvPathFallacies { get; set; } = @"..\..\..\Data\Dnn2sxc\Argumentum Fallacies - Taxonomy.csv";
        public string Xml2sxcImportPathFallacies { get; set; } = @"..\..\..\Data\Dnn2sxc\2sxc Fallacy  Data 20210212131131.xml";
        public string Xml2sxcExportPathFallacies { get; set; } = @"..\..\..\Data\Dnn2sxc\2sxcContentExport_Argumentum_Fallacies_0.0.1.xml";


        public string TitleExpressionFr { get; set; } = @"{fallacy.TextFr}";

        public string DescriptionExpressionFr { get; set; } = @"{HttpUtility.HtmlEncode(fallacy.DescFr)}";

        public string ExampleExpressionFr { get; set; } = @"{HttpUtility.HtmlEncode(fallacy.ExampleFr)}";

        public string LinkExpressionFr { get; set; } = @"{fallacy.LinkFr}";

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> TitleFuncFr
        {
            get
            {
                return fallacy => TitleExpressionFr.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.TextFr}";
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> DescFuncFr
        {
            get
            {
                return fallacy => DescriptionExpressionFr.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<font size='4'>{HttpUtility.HtmlEncode(fallacy.DescFr)}</font>";
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> ExampleFuncFr
        {
            get
            {
                return fallacy => ExampleExpressionFr.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<i>{HttpUtility.HtmlEncode(fallacy.ExampleFr)}</i>";
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> LinkFuncFr
        {
            get
            {
                return fallacy => LinkExpressionFr.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.LinkFr}";
            }
        }

        public string TitleExpressionEn { get; set; } = @"{fallacy.TextEn}";

        public string DescriptionExpressionEn { get; set; } = @"{HttpUtility.HtmlEncode(fallacy.DescEn)}";

        public string ExampleExpressionEn { get; set; } = @"{HttpUtility.HtmlEncode(fallacy.ExampleEn)}";

        public string LinkExpressionEn { get; set; } = @"{fallacy.LinkEn}";

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> TitleFuncEn
        {
            get
            {
                return fallacy => TitleExpressionEn.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.TextEn}";
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> DescFuncEn
        {
            get
            {
                return fallacy => DescriptionExpressionEn.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<font size='4'>{HttpUtility.HtmlEncode(fallacy.DescEn)}</font>";
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> ExampleFuncEn
        {
            get
            {
                return fallacy => ExampleExpressionEn.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"<i>{HttpUtility.HtmlEncode(fallacy.ExampleEn)}</i>";
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public Func<Fallacy, string> LinkFuncEn
        {
            get
            {
                return fallacy => LinkExpressionEn.Interpolate(new Dictionary<string, object>() { { "fallacy", fallacy } }); // $"{fallacy.LinkEn}";
            }
        }




        public void Apply()
        {

            //var sexyContentSeralizer = new ConfigurationContainer()
            //    //.EnableImplicitTypingByInspecting<SexyContentData>()
            //    .UseAutoFormatting()
            //    .UseOptimizedNamespaces()
            //    .EnableImplicitTyping(typeof(SexyContentData))
            //    .EnableImplicitTyping(typeof(Entity))
            //    // Additional configurations...
            //    .Create();
            var sexyContentSeralizer = new XmlSerializer(typeof(SexyContentData), new []{ typeof(Entity) } );
            SexyContentData sexyContentData;
            var strFile = File.ReadAllText(Xml2sxcImportPathFallacies);
            using (var tr= System.IO.File.OpenText(Xml2sxcImportPathFallacies))
            {
                sexyContentData = (SexyContentData) sexyContentSeralizer.Deserialize(tr);
            }

            var fallacies = Fallacy.LoadFallacies(CsvPathFallacies);

            var fallaciesbyGuid = new Dictionary<string, (Entity entity, string path)>(fallacies.Count());
            var fallaciesByPath = new Dictionary<string, (Entity main, IList<Entity> children)>(fallacies.Count());
            var translationsEnFr = new Dictionary<Entity, Entity>(fallacies.Count);

            var enUsCulture = "en-US";
            var frFrCulture = "fr-FR";
            string fallacy2SxcType = "Fallacy";


            //Load existing 2sxc data

            Entity root = null;
            //root = sexyContentData.Entity.First(e => e.Name == "Fallacies" && e.Language == enUsCulture);


            int familyCounter = 0;
            foreach (var existingEntity in sexyContentData.Entity.Where(e=>e.Type == fallacy2SxcType))
            {
                string localPath;
                if (existingEntity.Language == enUsCulture)
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
                else
                {
                    var translated = fallaciesbyGuid[existingEntity.Guid];
                    translationsEnFr[translated.entity] = existingEntity;
                }
            }

            //Add/update with csv taxonomy

            foreach (var fallacy in fallacies)
            {
                
                var localPath = fallacy.Path;

                (Entity main, IList<Entity> children) fallacyNode;
                if (!fallaciesByPath.TryGetValue(localPath, out fallacyNode))
                {
                    fallacyNode = (new Entity(), new List<Entity>());
                    fallacyNode.main.Guid = Guid.NewGuid().ToString();
                    fallaciesByPath[localPath] = fallacyNode;
                    fallaciesbyGuid[fallacyNode.main.Guid] = (fallacyNode.main, localPath);
                    sexyContentData.Entity.Add(fallacyNode.main);
                }


                //var link = LinkFuncEn(fallacy);
                //if (!string.IsNullOrEmpty(link))
                //{
                //    fallacyNode. = link;
                //}
                //var descDoc = new XmlDocument();
                //descDoc.LoadXml($"{config.DescFunc(fallacy)}");
                //fallacyNode.Richcontent.Html.Body.Elements.Add(descDoc.DocumentElement);
                //descDoc.LoadXml($"{config.ExampleFunc(fallacy)}");
                //fallacyNode.Richcontent.Html.Body.Elements.Add(descDoc.DocumentElement);
                fallacyNode.main.Name = TitleFuncEn(fallacy);
                fallacyNode.main.Description = DescFuncEn(fallacy);
                fallacyNode.main.Example = ExampleFuncEn(fallacy);

                fallacyNode.main.Type = fallacy2SxcType;
                
                fallacyNode.main.Language = enUsCulture;

                var lastDotIndex = localPath.LastIndexOf('.');
                int familyNb;
                if (lastDotIndex > -1)
                {
                    
                    //familyNb = int.Parse(fallacy.Path[0].ToString(), CultureInfo.InvariantCulture);
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


                if (! translationsEnFr.TryGetValue(fallacyNode.main, out var translated))
                {
                    translated = new Entity();
                    
                    translationsEnFr[fallacyNode.main] = translated;
                    sexyContentData.Entity.Add(translated);

                    translated.Language = frFrCulture;
                    translated.Guid = fallacyNode.main.Guid;
                    translated.Parent = fallacyNode.main.Parent;
                    translated.Description = DescFuncFr(fallacy);
                    translated.Example = ExampleFuncFr(fallacy);

                    translated.Type = fallacy2SxcType;

                }

            }


            //Serialize to xml
            var direct = new DirectoryInfo(Xml2sxcExportPathFallacies);
            using (var tr = System.IO.File.OpenWrite(Xml2sxcExportPathFallacies))
            {
                sexyContentSeralizer.Serialize(tr, sexyContentData);
            }

        }
    }
}