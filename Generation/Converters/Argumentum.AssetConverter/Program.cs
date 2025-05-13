using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					Console.SetBufferSize(Console.BufferWidth, Int16.MaxValue - 1);
				}
				AnsiConsole.Write(new FigletText("Argumentum").Centered().Color(Color.Blue));

				Logger.LogExplanations("Welcome to Argumentum Swiss army knife. This application does a lot of generation in the background that go \"Brrrrrr...\" on the foreground. \nYou may have occasional instructions to follow, but this is about mainly relaxing and watching files and documents being created. \nEnjoy the ride !");

				Logger.LogTitle("Loading Configuration");

				Logger.LogExplanations("You can control most things that the application does through a large configuration file. \nDefault configuration has pretty much everything enabled for generation, which should take a little north of 1h.");

				// Vérifier si des arguments de ligne de commande sont fournis
				if (args.Length > 0)
				{
					// Traiter les arguments de ligne de commande
					if (args[0].Equals("--validate-taxonomy", StringComparison.OrdinalIgnoreCase))
					{
						Logger.LogTitle("Mode de validation de taxonomie");
						
						var taxonomyConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
						var taxonomyConfig = AssetConverterConfig.GetConfig(taxonomyConfigFileName, out var _);
						
						// Activer uniquement le mode de validation de taxonomie
						taxonomyConfig.Mode = ConverterMode.TaxonomyValidator;
						
						// Configurer les options de validation en fonction des arguments
						if (args.Length > 1)
						{
							foreach (var arg in args.Skip(1))
							{
								if (arg.Equals("--structure", StringComparison.OrdinalIgnoreCase))
								{
									taxonomyConfig.TaxonomyValidatorConfig.ValidateStructure = true;
									taxonomyConfig.TaxonomyValidatorConfig.ValidateTranslations = false;
									taxonomyConfig.TaxonomyValidatorConfig.ValidateTerminology = false;
								}
								else if (arg.Equals("--translations", StringComparison.OrdinalIgnoreCase))
								{
									taxonomyConfig.TaxonomyValidatorConfig.ValidateStructure = false;
									taxonomyConfig.TaxonomyValidatorConfig.ValidateTranslations = true;
									taxonomyConfig.TaxonomyValidatorConfig.ValidateTerminology = false;
								}
								else if (arg.Equals("--terminology", StringComparison.OrdinalIgnoreCase))
								{
									taxonomyConfig.TaxonomyValidatorConfig.ValidateStructure = false;
									taxonomyConfig.TaxonomyValidatorConfig.ValidateTranslations = false;
									taxonomyConfig.TaxonomyValidatorConfig.ValidateTerminology = true;
								}
							}
						}
						
						await taxonomyConfig.Apply().ConfigureAwait(false);
						return;
					}
					else if (args[0].Equals("--validate-owl", StringComparison.OrdinalIgnoreCase))
					{
						Logger.LogTitle("Mode de validation d'ontologie OWL");
						
						var owlConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
						var owlConfig = AssetConverterConfig.GetConfig(owlConfigFileName, out var _);
						
						// Activer uniquement le mode de validation d'ontologie OWL
						owlConfig.Mode = ConverterMode.OwlValidator;
						
						// Configurer les options de validation en fonction des arguments
						if (args.Length > 1)
						{
							foreach (var arg in args.Skip(1))
							{
								if (arg.Equals("--structure", StringComparison.OrdinalIgnoreCase))
								{
									owlConfig.OwlValidatorConfig.ValidateStructure = true;
									owlConfig.OwlValidatorConfig.ValidateMultilingualAnnotations = false;
									owlConfig.OwlValidatorConfig.ValidateAIFMappings = false;
								}
								else if (arg.Equals("--annotations", StringComparison.OrdinalIgnoreCase))
								{
									owlConfig.OwlValidatorConfig.ValidateStructure = false;
									owlConfig.OwlValidatorConfig.ValidateMultilingualAnnotations = true;
									owlConfig.OwlValidatorConfig.ValidateAIFMappings = false;
								}
								else if (arg.Equals("--mappings", StringComparison.OrdinalIgnoreCase))
								{
									owlConfig.OwlValidatorConfig.ValidateStructure = false;
									owlConfig.OwlValidatorConfig.ValidateMultilingualAnnotations = false;
									owlConfig.OwlValidatorConfig.ValidateAIFMappings = true;
								}
							}
						}
						
						await owlConfig.Apply().ConfigureAwait(false);
						return;
					}
					else if (args[0].Equals("--validate-cards", StringComparison.OrdinalIgnoreCase))
					{
						Logger.LogTitle("Mode de validation des cartes générées");
						
						var cardsConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
						var cardsConfig = AssetConverterConfig.GetConfig(cardsConfigFileName, out var _);
						
						// Activer uniquement le mode de validation des cartes
						cardsConfig.Mode = ConverterMode.CardValidator;
						
						// Configurer les options de validation en fonction des arguments
						if (args.Length > 1)
						{
							foreach (var arg in args.Skip(1))
							{
								if (arg.Equals("--existence", StringComparison.OrdinalIgnoreCase))
								{
									cardsConfig.CardValidatorConfig.ValidateFileExistence = true;
									cardsConfig.CardValidatorConfig.ValidateImageQuality = false;
									cardsConfig.CardValidatorConfig.ValidateMultilingualConsistency = false;
								}
								else if (arg.Equals("--quality", StringComparison.OrdinalIgnoreCase))
								{
									cardsConfig.CardValidatorConfig.ValidateFileExistence = false;
									cardsConfig.CardValidatorConfig.ValidateImageQuality = true;
									cardsConfig.CardValidatorConfig.ValidateMultilingualConsistency = false;
								}
								else if (arg.Equals("--consistency", StringComparison.OrdinalIgnoreCase))
								{
									cardsConfig.CardValidatorConfig.ValidateFileExistence = false;
									cardsConfig.CardValidatorConfig.ValidateImageQuality = false;
									cardsConfig.CardValidatorConfig.ValidateMultilingualConsistency = true;
								}
							}
						}
						
						await cardsConfig.Apply().ConfigureAwait(false);
						return;
					}
					else if (args[0].Equals("--continuous-validation", StringComparison.OrdinalIgnoreCase))
					{
						Logger.LogTitle("Mode de validation continue");
						
						var continuousConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
						var continuousConfig = AssetConverterConfig.GetConfig(continuousConfigFileName, out var _);
						
						// Activer uniquement le mode de validation continue
						continuousConfig.Mode = ConverterMode.ContinuousValidator;
						
						// Configurer les options de validation en fonction des arguments
						if (args.Length > 1)
						{
							foreach (var arg in args.Skip(1))
							{
								if (arg.Equals("--interval", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
								{
									int intervalIndex = args.ToList().IndexOf(arg) + 1;
									if (int.TryParse(args[intervalIndex], out int interval))
									{
										continuousConfig.ContinuousValidationConfig.ValidationInterval = interval;
									}
								}
								else if (arg.Equals("--watch", StringComparison.OrdinalIgnoreCase))
								{
									continuousConfig.ContinuousValidationConfig.ValidateOnChanges = true;
								}
								else if (arg.Equals("--no-watch", StringComparison.OrdinalIgnoreCase))
								{
									continuousConfig.ContinuousValidationConfig.ValidateOnChanges = false;
								}
								else if (arg.Equals("--taxonomy", StringComparison.OrdinalIgnoreCase))
								{
									continuousConfig.ContinuousValidationConfig.ValidateTaxonomy = true;
									continuousConfig.ContinuousValidationConfig.ValidateOwl = false;
									continuousConfig.ContinuousValidationConfig.ValidateCards = false;
								}
								else if (arg.Equals("--owl", StringComparison.OrdinalIgnoreCase))
								{
									continuousConfig.ContinuousValidationConfig.ValidateTaxonomy = false;
									continuousConfig.ContinuousValidationConfig.ValidateOwl = true;
									continuousConfig.ContinuousValidationConfig.ValidateCards = false;
								}
								else if (arg.Equals("--cards", StringComparison.OrdinalIgnoreCase))
								{
									continuousConfig.ContinuousValidationConfig.ValidateTaxonomy = false;
									continuousConfig.ContinuousValidationConfig.ValidateOwl = false;
									continuousConfig.ContinuousValidationConfig.ValidateCards = true;
								}
							}
						}
						
						await continuousConfig.Apply().ConfigureAwait(false);
						return;
					}
					else if (args[0].Equals("--translation-coverage", StringComparison.OrdinalIgnoreCase))
					{
						Logger.LogTitle("Mode de rapport de couverture des traductions");
						
						var translationConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
						var translationConfig = AssetConverterConfig.GetConfig(translationConfigFileName, out var _);
						
						// Activer uniquement le mode de rapport de couverture des traductions
						translationConfig.Mode = ConverterMode.TranslationCoverage;
						
						// Configurer les options de rapport en fonction des arguments
						if (args.Length > 1)
						{
							foreach (var arg in args.Skip(1))
							{
								if (arg.Equals("--languages", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
								{
									int languagesIndex = args.ToList().IndexOf(arg) + 1;
									string[] languages = args[languagesIndex].Split(',');
									translationConfig.TranslationCoverageConfig.Languages = languages.ToList();
								}
								else if (arg.Equals("--fields", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
								{
									int fieldsIndex = args.ToList().IndexOf(arg) + 1;
									string[] fields = args[fieldsIndex].Split(',');
									translationConfig.TranslationCoverageConfig.FieldTypes = fields.ToList();
								}
								else if (arg.Equals("--threshold", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
								{
									int thresholdIndex = args.ToList().IndexOf(arg) + 1;
									if (int.TryParse(args[thresholdIndex], out int threshold))
									{
										translationConfig.TranslationCoverageConfig.MinimumCoverageThreshold = threshold;
									}
								}
								else if (arg.Equals("--no-charts", StringComparison.OrdinalIgnoreCase))
								{
									translationConfig.TranslationCoverageConfig.GenerateProgressCharts = false;
								}
							}
						}
						
						await translationConfig.Apply().ConfigureAwait(false);
						return;
					}
					else if (args[0].Equals("--optimize-parallelism", StringComparison.OrdinalIgnoreCase))
					{
						Logger.LogTitle("Mode d'optimisation du parallélisme");
						
						var parallelismConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
						var parallelismConfig = AssetConverterConfig.GetConfig(parallelismConfigFileName, out var _);
						
						// Activer uniquement le mode d'optimisation du parallélisme
						parallelismConfig.Mode = ConverterMode.ParallelismOptimizer;
						
						// Configurer les options d'optimisation en fonction des arguments
						if (args.Length > 1)
						{
							foreach (var arg in args.Skip(1))
							{
								if (arg.Equals("--run-before-generation", StringComparison.OrdinalIgnoreCase))
								{
									parallelismConfig.ParallelismOptimizerConfig.RunBeforeGeneration = true;
								}
								else if (arg.Equals("--no-run-before-generation", StringComparison.OrdinalIgnoreCase))
								{
									parallelismConfig.ParallelismOptimizerConfig.RunBeforeGeneration = false;
								}
								else if (arg.Equals("--dynamic-adjustment", StringComparison.OrdinalIgnoreCase))
								{
									parallelismConfig.ParallelismOptimizerConfig.DynamicAdjustment = true;
								}
								else if (arg.Equals("--no-dynamic-adjustment", StringComparison.OrdinalIgnoreCase))
								{
									parallelismConfig.ParallelismOptimizerConfig.DynamicAdjustment = false;
								}
								else if (arg.Equals("--monitoring-interval", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
								{
									int intervalIndex = args.ToList().IndexOf(arg) + 1;
									if (int.TryParse(args[intervalIndex], out int interval))
									{
										parallelismConfig.ParallelismOptimizerConfig.MonitoringIntervalSeconds = interval;
									}
								}
								else if (arg.Equals("--target-cpu-usage", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
								{
									int usageIndex = args.ToList().IndexOf(arg) + 1;
									if (int.TryParse(args[usageIndex], out int usage))
									{
										parallelismConfig.ParallelismOptimizerConfig.TargetCpuUsagePercent = usage;
									}
								}
								else if (arg.Equals("--target-memory-usage", StringComparison.OrdinalIgnoreCase) && args.Length > args.ToList().IndexOf(arg) + 1)
								{
									int usageIndex = args.ToList().IndexOf(arg) + 1;
									if (int.TryParse(args[usageIndex], out int usage))
									{
										parallelismConfig.ParallelismOptimizerConfig.TargetMemoryUsagePercent = usage;
									}
								}
								else if (arg.Equals("--detailed-report", StringComparison.OrdinalIgnoreCase))
								{
									parallelismConfig.ParallelismOptimizerConfig.GenerateDetailedReport = true;
								}
								else if (arg.Equals("--no-detailed-report", StringComparison.OrdinalIgnoreCase))
								{
									parallelismConfig.ParallelismOptimizerConfig.GenerateDetailedReport = false;
								}
							}
						}
						
						await parallelismConfig.Apply().ConfigureAwait(false);
						return;
					}
					else if (args[0].Equals("--generate-documentation", StringComparison.OrdinalIgnoreCase))
					{
						Logger.LogTitle("Mode de génération de documentation");
						
						var docConfigFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
						var docConfig = AssetConverterConfig.GetConfig(docConfigFileName, out var _);
						
						string inputDir = Path.Combine(Environment.CurrentDirectory, "Documentation");
						string outputDir = Path.Combine(Environment.CurrentDirectory, "Output", "Documentation");
						
						// Configurer les options de génération de documentation en fonction des arguments
						if (args.Length > 1)
						{
							foreach (var arg in args.Skip(1))
							{
								if (arg.StartsWith("--input=", StringComparison.OrdinalIgnoreCase))
								{
									inputDir = arg.Substring("--input=".Length);
								}
								else if (arg.StartsWith("--output=", StringComparison.OrdinalIgnoreCase))
								{
									outputDir = arg.Substring("--output=".Length);
								}
							}
						}
						
						Logger.LogInfoMessage($"Génération de la documentation à partir de {inputDir} vers {outputDir}");
						
						var docGenerator = new Documentation.DocumentationGenerator(inputDir, outputDir);
						await docGenerator.GenerateAsync();
						
						Logger.LogSuccess($"Documentation générée avec succès dans {outputDir}");
						return;
					}
				}

				var configFileName = Path.Combine(Environment.CurrentDirectory, "AssetConverterConfig.json");
				var config = AssetConverterConfig.GetConfig(configFileName, out var newConfig);
				if (newConfig)
				{
					Logger.LogInstructions($"If you wish to edit the configuration file, then close this window and relaunch application after applying edits to the configuration file.\n If you wish to run the default configuration, press any key.");
					
					Console.ReadKey();
				}
				
				await config.Apply().ConfigureAwait(false);

				
				Logger.Log($"Generation finished.");
				Logger.LogInstructions($"Thanks for the ride. \nLet's now check those files located in {Environment.CurrentDirectory}\nPress any key to close");

				// Afficher l'aide pour la validation de taxonomie, d'ontologie OWL et des cartes
				Logger.LogExplanations("Pour exécuter les tests de validation de taxonomie, utilisez la commande suivante :\n" +
					"Argumentum.AssetConverter.exe --validate-taxonomy [options]\n\n" +
					"Options disponibles :\n" +
					"  --structure    : Valide uniquement la structure de la taxonomie\n" +
					"  --translations : Valide uniquement la complétude des traductions\n" +
					"  --terminology  : Valide uniquement la cohérence terminologique\n\n" +
					"Sans option, tous les tests de validation seront exécutés.\n\n" +
					"Pour exécuter les tests de validation de l'ontologie OWL, utilisez la commande suivante :\n" +
					"Argumentum.AssetConverter.exe --validate-owl [options]\n\n" +
					"Options disponibles :\n" +
					"  --structure   : Valide uniquement la structure de l'ontologie\n" +
					"  --annotations : Valide uniquement les annotations multilingues\n" +
					"  --mappings    : Valide uniquement les mappings avec l'ontologie AIF\n\n" +
					"Sans option, tous les tests de validation seront exécutés.\n\n" +
					"Pour exécuter les tests de validation des cartes générées, utilisez la commande suivante :\n" +
					"Argumentum.AssetConverter.exe --validate-cards [options]\n\n" +
					"Options disponibles :\n" +
					"  --existence   : Valide uniquement l'existence des fichiers de cartes\n" +
					"  --quality     : Valide uniquement la qualité des images de cartes\n" +
					"  --consistency : Valide uniquement la cohérence multilingue des cartes\n\n" +
					"Sans option, tous les tests de validation seront exécutés.\n\n" +
					"Pour exécuter le système de validation continue, utilisez la commande suivante :\n" +
					"Argumentum.AssetConverter.exe --continuous-validation [options]\n\n" +
					"Options disponibles :\n" +
					"  --interval N  : Définit l'intervalle de validation automatique en minutes (N)\n" +
					"  --watch       : Active la surveillance des fichiers pour déclencher des validations automatiques\n" +
					"  --no-watch    : Désactive la surveillance des fichiers\n" +
					"  --taxonomy    : Valide uniquement la taxonomie\n" +
					"  --owl         : Valide uniquement l'ontologie OWL\n" +
					"  --cards       : Valide uniquement les cartes générées\n\n" +
					"Sans option, tous les types de validation seront exécutés selon la configuration par défaut.\n\n" +
					"Pour générer un rapport de couverture des traductions, utilisez la commande suivante :\n" +
					"Argumentum.AssetConverter.exe --translation-coverage [options]\n\n" +
					"Options disponibles :\n" +
					"  --languages fr,en,ru,pt : Spécifie les langues à analyser (séparées par des virgules)\n" +
					"  --fields Text,Desc,Example,Link : Spécifie les types de champs à analyser (séparés par des virgules)\n" +
					"  --threshold N : Définit le seuil minimum de couverture acceptable (en pourcentage)\n" +
					"  --no-charts : Désactive la génération des graphiques de progression\n\n" +
					"Sans option, toutes les langues et tous les types de champs seront analysés selon la configuration par défaut.\n\n" +
					"Pour optimiser les paramètres de parallélisme, utilisez la commande suivante :\n" +
					"Argumentum.AssetConverter.exe --optimize-parallelism [options]\n\n" +
					"Options disponibles :\n" +
					"  --run-before-generation    : Exécute l'optimisation avant chaque génération\n" +
					"  --no-run-before-generation : Désactive l'exécution automatique avant génération\n" +
					"  --dynamic-adjustment       : Active l'ajustement dynamique pendant la génération\n" +
					"  --no-dynamic-adjustment    : Désactive l'ajustement dynamique pendant la génération\n" +
					"  --monitoring-interval N    : Définit l'intervalle de surveillance en secondes (N)\n" +
					"  --target-cpu-usage N       : Définit le pourcentage cible d'utilisation du CPU (N)\n" +
					"  --target-memory-usage N    : Définit le pourcentage cible d'utilisation de la mémoire (N)\n" +
					"  --detailed-report          : Active la génération d'un rapport détaillé\n" +
					"  --no-detailed-report       : Désactive la génération d'un rapport détaillé\n\n" +
					"Sans option, l'optimisation sera exécutée avec les paramètres par défaut.\n\n" +
					"Pour générer la documentation HTML à partir des fichiers Markdown, utilisez la commande suivante :\n" +
					"Argumentum.AssetConverter.exe --generate-documentation [options]\n\n" +
					"Options disponibles :\n" +
					"  --input=<chemin>  : Spécifie le répertoire contenant les fichiers Markdown (par défaut: ./Documentation)\n" +
					"  --output=<chemin> : Spécifie le répertoire de sortie pour les fichiers HTML (par défaut: ./Output/Documentation)\n\n" +
					"La documentation générée inclut une table des matières, un index de recherche et une navigation conviviale.");
			}
			catch (Exception e)
			{
				Logger.LogException(e);

			}
		}


	}
}
