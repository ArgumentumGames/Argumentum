using System;
using System.IO;
using CsvHelper;

namespace Argumentum.CsvValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "..", "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");
            
            Console.WriteLine($"Starting validation of file: {csvPath}");

            if (!File.Exists(csvPath))
            {
                Console.WriteLine("Error: File not found!");
                return;
            }

            var badLines = 0;
            BadDataFound badDataFound = (context) =>
            {
                badLines++;
                if(context.Context != null && context.Context.Parser != null)
                {
                    Console.WriteLine($"\n[BAD DATA ROW] at line {context.Context.Parser.RawRow}:");
                }
                else
                {
                    Console.WriteLine($"\n[BAD DATA ROW] at unknown line:");
                }
                Console.WriteLine(context.RawRecord);
            };

            try
            {
                var fallacies = Fallacy.Load(csvPath, badDataFound);
                Console.WriteLine($"\nValidation finished.");
                Console.WriteLine($"Successfully loaded {fallacies.Count} records.");
                if (badLines > 0)
                {
                    Console.WriteLine($"Found {badLines} corrupted lines.");
                }
                else
                {
                    Console.WriteLine("No corrupted lines found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nAn unexpected error occurred during validation:");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
