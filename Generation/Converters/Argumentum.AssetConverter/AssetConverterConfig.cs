using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Argumentum.AssetConverter.Dnn2sxc;
using Argumentum.AssetConverter.Mindmapper;
using Spectre.Console;
using Spectre.Console.Json;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;

namespace Argumentum.AssetConverter
{
    public class AssetConverterConfig
    {
        public ConverterMode Mode { get; set; } = ConverterMode.WebBasedImageGeneration; //ConverterMode.Mindmapper;
       
        public WebBasedGeneratorConfig WebBasedGeneratorConfig { get; set; } = new WebBasedGeneratorConfig();

        public BatchImageConverterConfig BatchImageConverterConfig { get; set; } = new BatchImageConverterConfig();

        public Dnn2sxcConfig Dnn2sxcConfig { get; set; } = new Dnn2sxcConfig();

		public MindMapCreatorConfig MindMapCreatorConfig { get; set; } = new MindMapCreatorConfig();

        public static AssetConverterConfig GetConfig(string path, out bool newConfig)
        {
            AssetConverterConfig toReturn;
            CompositeResolver.RegisterAndSetAsDefault(new IJsonFormatter[] { new TimeSpanFormatter() }, new IJsonFormatterResolver[] { StandardResolver.Default });
            newConfig = false;
            if (!File.Exists(path))
            {

                toReturn = new AssetConverterConfig();
                var strNewConfig = JsonSerializer.PrettyPrint(JsonSerializer.ToJsonString(toReturn));


                File.WriteAllText(path, strNewConfig);
                newConfig = true;


				//Logger.LogJson(strNewConfig);

				Logger.LogSuccess($"Config file created: {path}");
			}

            
            using var configStream = File.OpenRead(path);

            

			toReturn = JsonSerializer.Deserialize<AssetConverterConfig>(configStream);

			Logger.Log($"Config loaded: {path}");
			return toReturn;
        }

        


        public  bool Apply()
        {
            switch (Mode)
            {
                case ConverterMode.BatchImageProcessor:
                    BatchImageConverterConfig.Apply();
                    break;
                case ConverterMode.WebBasedImageGeneration:
					 WebBasedGeneratorConfig.Apply();
                    break;
                case ConverterMode.Mindmapper:
                    MindMapCreatorConfig.Run(null);
                    break;
                case ConverterMode.Dnn2sxc:
                    Dnn2sxcConfig.Apply();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }



    }
}
