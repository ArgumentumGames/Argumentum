using System;
using System.Diagnostics;
using Spectre.Console;

namespace Argumentum.AssetConverter;

public enum MessageType
{
	Info,
	Title
}


public class Logger
{

	public static Stopwatch Stopwatch;

	public static void Log(string message, MessageType messageType = MessageType.Info)
	{
		if (Stopwatch == null)
		{
			Stopwatch = Stopwatch.StartNew();
		}


		switch (messageType)
		{
			case MessageType.Info:

				Logger.Log("{message}");
				break;
			case MessageType.Title:
				AnsiConsole.WriteLine();
				var rule = new Rule($"[red]{message}[/]");
				AnsiConsole.Write(rule);
				AnsiConsole.WriteLine();

				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
		}


	}





}