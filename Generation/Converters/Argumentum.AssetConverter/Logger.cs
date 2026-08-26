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

			// #1179: never delete the previous run's log. The old File.Delete destroyed the generation
			// log the moment a follow-up pass (e.g. --pdf-cmyk) started in the same tree, making the
			// originating exception unrecoverable — that cost a full investigation cycle on #1177.
			ArchivePreviousLog();
		}
		catch (Exception ex)
		{
			AnsiConsole.MarkupLine($"[bold red]Error during logger initialization: {Markup.Escape(ex.Message)}[/]");
		}
	}

	/// <summary>
	/// Moves the current <c>file_logger.log</c> to a timestamped sibling (<c>file_logger-yyyyMMdd-HHmmss.log</c>)
	/// so a later run (e.g. the CMYK pass over the same tree) cannot overwrite the previous run's log.
	/// No-op when no live log exists. A new live log is created lazily by the next <see cref="Log"/> call.
	/// </summary>
	public static void ArchivePreviousLog()
	{
		if (!File.Exists(LogFile))
		{
			return;
		}

		var directory = Path.GetDirectoryName(LogFile);
		var timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
		var archivePath = Path.Combine(directory, $"file_logger-{timestamp}.log");
		var counter = 1;
		while (File.Exists(archivePath))
		{
			archivePath = Path.Combine(directory, $"file_logger-{timestamp}-{counter++}.log");
		}

		File.Move(LogFile, archivePath);
		AnsiConsole.MarkupLine($"[dim]Previous generation log archived to '{Markup.Escape(archivePath)}'[/]");
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
			AnsiConsole.MarkupLine($"[bold red]Failed to write to log file '{Markup.Escape(LogFile)}'. Exception: {Markup.Escape(ex.Message)}[/]");
		}

		try
		{
			switch (messageType)
			{
				case MessageType.Info:
				case MessageType.Success:
				case MessageType.Warning:
					var markup = messageType == MessageType.Info ? "dim" : messageType == MessageType.Warning ? "sandybrown" : "green3";
					if (LogInfo || messageType != MessageType.Info)
					{
						AnsiConsole.MarkupLine($"{Stopwatch.Elapsed}: [{markup}]{Markup.Escape(message)}[/]");
					}
					break;
				case MessageType.Title:
					AnsiConsole.WriteLine();
					AnsiConsole.WriteLine();
					var rule = new Rule($"[bold]{Markup.Escape(message)}[/]");
					AnsiConsole.Write(rule);
					AnsiConsole.WriteLine();
					break;
				case MessageType.Problem:
					AnsiConsole.WriteLine();
					AnsiConsole.MarkupLine($"[bold red]{Markup.Escape(message)}[/]");
					AnsiConsole.WriteLine();
					break;
				case MessageType.Instructions:
				case MessageType.Explanations:
					AnsiConsole.WriteLine();
					var header = messageType.ToString();
					var color = messageType==MessageType.Instructions ? Color.Yellow : Color.PaleGreen1;
					AnsiConsole.Write(
						new Panel(Markup.Escape(message))
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
		catch (Exception renderEx) when (renderEx is not ArgumentOutOfRangeException)
		{
			// Console rendering must never propagate (#630): the #614 per-set resilience calls
			// Log() from its catch path, so a Spectre StyleParser failure here would kill the
			// whole run instead of degrading gracefully. Fall back to plain (markup-free) output.
			AnsiConsole.WriteLine($"{Stopwatch.Elapsed}: [{messageType}] {message} (console render failed: {renderEx.Message})");
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
		// Always log the raw exception type+message first (Spectre formatter has a known IndexOutOfRangeException bug)
		var raw = $"{ex.GetType().FullName}: {ex.Message}";
		var inner = ex.InnerException;
		while (inner != null)
		{
			raw += $" --> {inner.GetType().FullName}: {inner.Message}";
			inner = inner.InnerException;
		}
		AnsiConsole.MarkupLine($"[bold red]{Markup.Escape(raw)}[/]");
		try
		{
			AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
		}
		catch (Exception spectreEx)
		{
			AnsiConsole.MarkupLine($"[yellow]Spectre formatter failed: {Markup.Escape(spectreEx.Message)}. Stack:[/]");
			AnsiConsole.WriteLine(ex.StackTrace ?? "(no stack)");
		}
		try
		{
			lock (fileLock)
			{
				File.AppendAllText(LogFile, $"{Stopwatch.Elapsed}: [EXCEPTION] {ex.ToString().Replace(Environment.NewLine, " ")}{Environment.NewLine}");
			}
		}
		catch (Exception logEx)
		{
			AnsiConsole.MarkupLine($"[bold red]Failed to write exception to log file '{Markup.Escape(LogFile)}'. Exception: {Markup.Escape(logEx.Message)}[/]");
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