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
		static void Main(string[] args)
		{
			try
			{
				Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
				Console.InputEncoding = System.Text.Encoding.UTF8;
				Console.OutputEncoding = System.Text.Encoding.UTF8;

				AnsiConsole.Write(new FigletText("Argumentum").Centered().Color(Color.Blue));

				Logger.LogExplanations("Welcome to Argumentum Swiss army knife. This application does a lot of generation in the background that go \"Brrrrrr...\" on the foreground. \nYou may have occasional instructions to follow, but this is about mainly relaxing and watching files and documents being created. \nEnjoy the ride !");

				Logger.LogTitle("Loading Configuration");

				Logger.LogExplanations("You can control most things that the application does through a large configuration file. \nDefault configuration has pretty much everything enabled for generation, which should take a little north of 1h.");


				var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
				var config = AssetConverterConfig.GetConfig(configFileName, out var newConfig);
				if (newConfig)
				{
					Logger.LogInstructions($"If you wish to edit the configuration file, then close this window and relaunch application after applying  edits to the configuration file.\n If you wish to run the default configuration, press any key.");
					
					Console.ReadKey();
				}
				
				config.Apply();
				Logger.Log($"Generation finished.");
				Logger.LogInstructions($"Thanks for the ride. \nLet's now check those files located in {Environment.CurrentDirectory}\nPress any key to close");
			}
			catch (Exception e)
			{
				Logger.LogException(e);

			}
			Console.ReadKey();
		}


	}
}
