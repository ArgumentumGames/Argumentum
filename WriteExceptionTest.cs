using System;
using Spectre.Console;

namespace WriteExceptionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test de la méthode AnsiConsole.WriteException()");
            
            try
            {
                throw new Exception("Test d'exception pour vérifier si System.Diagnostics.StackTrace est toujours problématique");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Tentative d'utilisation de AnsiConsole.WriteException()...");
                try
                {
                    // Cette méthode dépendait de System.Diagnostics.StackTrace dans les versions précédentes
                    AnsiConsole.WriteException(ex);
                    Console.WriteLine("SUCCESS: AnsiConsole.WriteException() a fonctionné sans erreur!");
                }
                catch (Exception innerEx)
                {
                    Console.WriteLine($"ERREUR: AnsiConsole.WriteException() a échoué: {innerEx.Message}");
                    Console.WriteLine($"Type d'exception: {innerEx.GetType().FullName}");
                    if (innerEx.InnerException != null)
                    {
                        Console.WriteLine($"Exception interne: {innerEx.InnerException.Message}");
                        Console.WriteLine($"Type d'exception interne: {innerEx.InnerException.GetType().FullName}");
                    }
                }
            }
        }
    }
}