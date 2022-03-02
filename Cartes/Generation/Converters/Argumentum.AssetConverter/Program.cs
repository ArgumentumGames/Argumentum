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
                var config = AssetConverterConfig.GetConfig(Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json"));
                Console.WriteLine($"Config loaded: {sw.Elapsed}");
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
