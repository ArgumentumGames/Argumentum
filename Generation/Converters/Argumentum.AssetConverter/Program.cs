using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

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
                    Console.WriteLine($"New Default Config file created : {sw.Elapsed} \n {configFileName} ");
                    Console.WriteLine($"If you wish to edit that configuration file to define the documents to generate then close this window and relaunch application after edits.");
                    Console.WriteLine($"If you wish to run the default configuration, press a key");
                    Console.ReadKey();
				}
                else
                {
                    Console.WriteLine($"Config loaded: {sw.Elapsed} \n {configFileName}");

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
