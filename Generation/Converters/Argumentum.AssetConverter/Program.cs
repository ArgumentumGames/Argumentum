using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Utf8Json;

namespace Argumentum.AssetConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                var sw = Stopwatch.StartNew();
                bool newConfig;
                var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
                var config = AssetConverterConfig.GetConfig(configFileName, out newConfig);
                if (newConfig)
                {
                    Console.WriteLine($"The default config include all assets in all languages. If you wish to edit that configuration file to choose the generated documents, then close this window and relaunch application after applying  edits to the configuration file.\n");
                    Console.WriteLine($"If you wish to run the default configuration, press any key");
                    
                    Console.ReadKey();
				}
                else
                {
                    Console.WriteLine($"{sw.Elapsed}: Config loaded: \n {configFileName}");

				}
                config.Apply(sw);
                Console.WriteLine($"Generation finished in {sw.Elapsed.TotalSeconds} seconds, press any key to close");
			}
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
            Console.ReadKey();
        }


    }
}
