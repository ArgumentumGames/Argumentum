using System;
using Spectre.Console;

namespace LoggerFix
{
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
        public static void LogException(Exception ex)
        {
            Console.WriteLine("Méthode LogException modifiée qui n'utilise pas System.Diagnostics.StackTrace");
            
            // Solution qui n'utilise pas System.Diagnostics.StackTrace
            AnsiConsole.MarkupLine($"[red]{ex.GetType().Name}[/]: [bold red]{ex.Message}[/]");
            if (ex.StackTrace != null)
            {
                AnsiConsole.MarkupLine($"[dim]{ex.StackTrace}[/]");
            }
            
            // Ne pas utiliser cette méthode car elle dépend de System.Diagnostics.StackTrace
            // AnsiConsole.WriteException(ex);
        }
    }

    class LoggerTest
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test de la méthode LogException modifiée");
            
            try
            {
                throw new Exception("Test d'exception");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}