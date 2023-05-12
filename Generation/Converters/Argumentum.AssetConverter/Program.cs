using Spectre.Console;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Utf8Json;

namespace Argumentum.AssetConverter
{
	class Program
	{
		static async Task Main(string[] args)
		{
			try
			{
				Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
				Console.InputEncoding = System.Text.Encoding.UTF8;
				Console.OutputEncoding = System.Text.Encoding.UTF8;

				AnsiConsole.Write(
					new FigletText("Argumentum")
						.Centered()
						.Color(Color.Blue));

				var sw = Stopwatch.StartNew();
				var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
				var config = AssetConverterConfig.GetConfig(configFileName, out var newConfig);
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
				await config.Apply(sw);
				Console.WriteLine($"Generation finished in {sw.Elapsed.TotalSeconds} seconds, press any key to close");
			}
			catch (Exception e)
			{
				AnsiConsole.WriteException(e);

			}
			Console.ReadKey();
		}


	}
}
