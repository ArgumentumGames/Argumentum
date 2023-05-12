using System;
using System.Diagnostics;
using Spectre.Console;
using Spectre.Console.Json;

namespace Argumentum.AssetConverter;

public enum MessageType
{
	Info,
	Title,
	Problem,
	Instructions,
	Warning,
	Success,
	Explanations
}


public class Logger
{

	public static Stopwatch Stopwatch;

	public static bool LogInfo = true;
	

	public static void Log(string message, MessageType messageType = MessageType.Info)
	{
		if (Stopwatch == null)
		{
			Stopwatch = Stopwatch.StartNew();
		}


		switch (messageType)
		{
			case MessageType.Info:
			case MessageType.Success:
			case MessageType.Warning:
				var markup = messageType == MessageType.Info ? "dim" : messageType == MessageType.Warning ? "sandybrown" : "green3";
				if (LogInfo || messageType != MessageType.Info)
				{
					AnsiConsole.MarkupLine($"{Stopwatch.Elapsed}: [{markup}]{message}[/]");
				}
				break;
			case MessageType.Title:
				AnsiConsole.WriteLine();
				AnsiConsole.WriteLine();
				var rule = new Rule($"[bold]{message}[/]");
				AnsiConsole.Write(rule);
				AnsiConsole.WriteLine();
				break;
			case MessageType.Problem:
				AnsiConsole.WriteLine();
				AnsiConsole.MarkupLine($"[bold red]{message}[/]");
				AnsiConsole.WriteLine();
				break;
			case MessageType.Instructions:
			case MessageType.Explanations:
				AnsiConsole.WriteLine();
				var header = messageType.ToString();
				var color = messageType==MessageType.Instructions ? Color.Yellow : Color.PaleGreen1;
				AnsiConsole.Write(
					new Panel(message)
						.Header(header)
						.Collapse()
						.RoundedBorder()
						.BorderColor(color));
				AnsiConsole.WriteLine();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
		}


	}

	public static void LogSuccess(string message)
	{
		Log(message, MessageType.Success);
	}


	public static void LogInstructions(string message)
	{
		Log(message, MessageType.Instructions);
	}

	public static void LogExplanations(string message)
	{
		Log(message, MessageType.Explanations);
	}

	public static void LogProblem(string message)
	{
		Log(message, MessageType.Problem);
	}

	public static void LogTitle(string message)
	{
		Log(message, MessageType.Title);
	}


	public static void LogException(Exception ex)
	{
		LogProblem("Execution error");
		AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
	}

	public static void LogJson(string strNewConfig)
	{
		var json = new JsonText(strNewConfig);

		AnsiConsole.Write(json);
		AnsiConsole.WriteLine();
	}

	public static void LogWarning(string message)
	{
		Log(message, MessageType.Warning);
	}
}