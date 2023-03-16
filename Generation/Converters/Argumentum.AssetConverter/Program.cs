using System;
using System.Diagnostics;
using System.IO;

namespace Argumentum.AssetConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                bool newConfig;
                var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
                var config = AssetConverterConfig.GetConfig(configFileName, out newConfig);
                if (newConfig)
                {
                    Console.WriteLine($"New Config file created : {sw.Elapsed} \n {configFileName} ");
                    Console.WriteLine($"Please edit configuration File to define the documents to generate and relaunch application");
                }
                else
                {
                    Console.WriteLine($"Config loaded: {sw.Elapsed}");
                    config.Apply(sw);

                    Console.WriteLine($"Generation finished in {sw.Elapsed.TotalSeconds} seconds, press any key to close");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
            Console.ReadKey();
        }


    }
}
