using System;
using System.IO;
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
	private static readonly object fileLock = new object();

	private static string GetLogFilePath()
	{
		// Commence à partir du répertoire de base de l'application (ex: .../bin/Debug/net8.0)
		var assemblyDir = AppContext.BaseDirectory;
		// Remonte jusqu'à trouver le répertoire racine du projet "Argumentum"
		var projectRoot = new DirectoryInfo(assemblyDir);
		while (projectRoot != null && !projectRoot.Name.Equals("Argumentum", StringComparison.OrdinalIgnoreCase))
		{
			projectRoot = projectRoot.Parent;
		}

		if (projectRoot == null)
		{
			// Fallback au cas où la structure de dossiers changerait
			projectRoot = new DirectoryInfo(assemblyDir);
		}

		return Path.Combine(projectRoot.FullName, "Logs", "file_logger.log");
	}

	public static string LogFile = GetLogFilePath();

	static Logger()
	{
		try
		{
			var logDirectory = Path.GetDirectoryName(LogFile);
			if (!Directory.Exists(logDirectory))
			{
				Directory.CreateDirectory(logDirectory);
			}

			if (File.Exists(LogFile))
			{
				File.Delete(LogFile);
			}
		}
		catch (Exception ex)
		{
			AnsiConsole.MarkupLine($"[bold red]Error during logger initialization: {ex.Message}[/]");
		}
	}

	public static Stopwatch Stopwatch;

	public static bool LogInfo = true;
	

	public static void Log(string message, MessageType messageType = MessageType.Info)
	{
		if (Stopwatch == null)
		{
			Stopwatch = Stopwatch.StartNew();
		}


		try
		{
			lock (fileLock)
			{
				var formattedMessage = message.Replace(Environment.NewLine, " ");
				File.AppendAllText(LogFile, $"{Stopwatch.Elapsed}: [{messageType}] {formattedMessage}{Environment.NewLine}");
			}
		}
		catch (Exception ex)
		{
			// Log initialization errors to the console to ensure they are visible
			// as the file logger itself may be the source of the problem.
			AnsiConsole.MarkupLine($"[bold red]Failed to write to log file '{LogFile}'. Exception: {ex.Message}[/]");
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
		// Utiliser directement AnsiConsole.WriteException car le problème est résolu dans Spectre.Console 0.50.0
		AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
		try
		{
			lock (fileLock)
			{
				File.AppendAllText(LogFile, $"{Stopwatch.Elapsed}: [EXCEPTION] {ex.ToString().Replace(Environment.NewLine, " ")}{Environment.NewLine}");
			}
		}
		catch (Exception logEx)
		{
			AnsiConsole.MarkupLine($"[bold red]Failed to write exception to log file '{LogFile}'. Exception: {logEx.Message}[/]");
		}
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

	public static void LogInformation(string message)
	{
		Log(message, MessageType.Success);
	}

	public static void LogInfoMessage(string message)
	{
		Log(message, MessageType.Info);
	}
}