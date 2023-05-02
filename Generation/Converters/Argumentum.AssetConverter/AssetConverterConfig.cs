using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Argumentum.AssetConverter.Dnn2sxc;
using Argumentum.AssetConverter.Mindmapper;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;

namespace Argumentum.AssetConverter
{
    public class AssetConverterConfig
    {


        public ConverterMode Mode { get; set; } = ConverterMode.WebBasedImageGeneration; //ConverterMode.Mindmapper;

        public BatchImageConverterConfig BatchImageConverterConfig { get; set; } = new BatchImageConverterConfig();

        public WebBasedGeneratorConfig WebBasedGeneratorConfig { get; set; } = new WebBasedGeneratorConfig();

        public MindMapCreatorConfig MindMapCreatorConfig { get; set; } = new MindMapCreatorConfig();

        public Dnn2sxcConfig Dnn2sxcConfig { get; set; } = new Dnn2sxcConfig();



        public static AssetConverterConfig GetConfig(string path, out bool newConfig)
        {
            AssetConverterConfig toReturn;
            CompositeResolver.RegisterAndSetAsDefault(new IJsonFormatter[] { new TimeSpanFormatter() }, new IJsonFormatterResolver[] { StandardResolver.Default });
            newConfig = false;
            if (!File.Exists(path))
            {
                toReturn = new AssetConverterConfig();
                var strNewConfig = JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(toReturn));
                Console.WriteLine($"Saving Config {path},\n File Content:\n {strNewConfig}");
                File.WriteAllText(path, strNewConfig);
                newConfig = true;
            }

            
            using var configStream = File.OpenRead(path);
            toReturn = JsonSerializer.Deserialize<AssetConverterConfig>(configStream);
            return toReturn;
        }


        public bool Apply(Stopwatch objSw)
        {
            switch (Mode)
            {
                case ConverterMode.BatchImageProcessor:
                    BatchImageConverterConfig.Apply();
                    break;
                case ConverterMode.WebBasedImageGeneration:
                    WebBasedGeneratorConfig.Apply(objSw);
                    break;
                case ConverterMode.Mindmapper:
                    MindMapCreatorConfig.Run(null);
                    break;
                case ConverterMode.Dnn2sxc:
                    Dnn2sxcConfig.Apply(objSw);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }



    }
}
