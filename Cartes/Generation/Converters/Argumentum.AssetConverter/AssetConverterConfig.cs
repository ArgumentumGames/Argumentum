using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;

namespace Argumentum.AssetConverter
{

    public enum ConverterMode
    {
        BatchImageProcessor,
        WebBasedImageGeneration
    }


    public class AssetConverterConfig
    {

       
        public ConverterMode Mode { get; set; } = ConverterMode.WebBasedImageGeneration;


        public BatchImageConverterConfig BatchImageConverterConfig { get; set; } = new BatchImageConverterConfig();

        public WebBasedGeneratorConfig WebBasedGeneratorConfig { get; set; } = new WebBasedGeneratorConfig();



        public static AssetConverterConfig GetConfig(string path)
        {
            AssetConverterConfig toReturn;
            CompositeResolver.RegisterAndSetAsDefault(new IJsonFormatter[] { new TimeSpanFormatter() }, new IJsonFormatterResolver[] { StandardResolver.Default });
            if (!File.Exists(path))
            {
                toReturn = new AssetConverterConfig();
                var strNewConfig = JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(toReturn));
                File.WriteAllText(path, strNewConfig);
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

       

    }
}
