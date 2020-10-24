using System;
using System.Diagnostics;
using System.IO;

namespace Argumentum.AssetConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var config = AssetConverterConfig.GetConfig(Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json"));
            config.Apply();

            Console.WriteLine($"Generation finished in {sw.Elapsed.TotalSeconds} seconds, press any key to close");
            Console.ReadKey();
        }



        



    }
}
