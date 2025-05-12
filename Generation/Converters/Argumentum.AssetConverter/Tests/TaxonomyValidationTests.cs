using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Tests de validation de la taxonomie des arguments fallacieux.
    /// Ces tests permettent de vérifier la qualité et la cohérence des données multilingues.
    /// </summary>
    public class TaxonomyValidationTests
    {
        private readonly AssetConverterConfig _config;
        private IList<Fallacy> _fallacies;
        private readonly List<string> _supportedLanguages = new List<string> { "fr", "en", "ru", "pt" };

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="TaxonomyValidationTests"/>.
        /// </summary>
        /// <param name="config">La configuration de l'application.</param>
        public TaxonomyValidationTests(AssetConverterConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Charge les données de taxonomie à partir du fichier CSV.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task LoadTaxonomyData()
        {
            var fallaciesDataSet = _config.DataSets.FirstOrDefault(ds => ds.Name == KnownDataSets.FallaciesTaxonomy);
            if (fallaciesDataSet == null)
            {
                Logger.LogProblem("Le jeu de données de taxonomie des arguments fallacieux n'a pas été trouvé dans la configuration.");
                return;
            }

            _fallacies = await Fallacy.LoadAsync(fallaciesDataSet, _config.UseDebugParams);
            Logger.LogSuccess($"Données de taxonomie chargées : {_fallacies.Count} entrées.");
        }

        /// <summary>
        /// Vérifie la cohérence structurelle de la taxonomie.
        /// - Unicité des chemins (Path)
        /// - Cohérence de la profondeur (Depth)
        /// - Existence des parents pour chaque nœud
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ValidateTaxonomyStructure()
        {
            Logger.LogTitle("Validation de la structure de la taxonomie");

            if (_fallacies == null)
            {
                await LoadTaxonomyData();
            }

            if (_fallacies == null || !_fallacies.Any())
            {
                Logger.LogProblem("Impossible de valider la structure de la taxonomie : aucune donnée chargée.");
                return;
            }

            int errorCount = 0;
            StringBuilder report = new StringBuilder();

            // Vérification de l'unicité des chemins
            var duplicatePaths = _fallacies
                .GroupBy(f => f.Path)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicatePaths.Any())
            {
                errorCount += duplicatePaths.Count;
                report.AppendLine("Chemins en double détectés :");
                foreach (var path in duplicatePaths)
                {
                    report.AppendLine($"  - {path}");
                }
                report.AppendLine();
            }

            // Vérification de la cohérence de la profondeur
            var depthErrors = _fallacies
                .Where(f => f.Path.Split('.').Length - 1 != f.Depth)
                .ToList();

            if (depthErrors.Any())
            {
                errorCount += depthErrors.Count;
                report.AppendLine("Incohérences de profondeur détectées :");
                foreach (var fallacy in depthErrors)
                {
                    int expectedDepth = fallacy.Path.Split('.').Length - 1;
                    report.AppendLine($"  - {fallacy.Path} : profondeur déclarée = {fallacy.Depth}, profondeur calculée = {expectedDepth}");
                }
                report.AppendLine();
            }

            // Vérification de l'existence des parents
            var allPaths = _fallacies.Select(f => f.Path).ToHashSet();
            var missingParents = new List<(string Path, string ParentPath)>();

            foreach (var fallacy in _fallacies.Where(f => f.Depth > 0))
            {
                string[] pathParts = fallacy.Path.Split('.');
                string parentPath = string.Join(".", pathParts.Take(pathParts.Length - 1));
                
                if (!allPaths.Contains(parentPath))
                {
                    missingParents.Add((fallacy.Path, parentPath));
                }
            }

            if (missingParents.Any())
            {
                errorCount += missingParents.Count;
                report.AppendLine("Parents manquants détectés :");
                foreach (var (path, parentPath) in missingParents)
                {
                    report.AppendLine($"  - {path} : parent manquant = {parentPath}");
                }
                report.AppendLine();
            }

            // Affichage du rapport
            if (errorCount > 0)
            {
                Logger.LogProblem($"Validation de la structure de la taxonomie : {errorCount} erreurs détectées");
                Logger.Log(report.ToString());
            }
            else
            {
                Logger.LogSuccess("Validation de la structure de la taxonomie : aucune erreur détectée");
            }
        }

        /// <summary>
        /// Vérifie la complétude des traductions.
        /// - Pour chaque langue (fr, en, ru, pt)
        /// - Pour chaque champ (Text, Desc, Example)
        /// - Génère un rapport des traductions manquantes
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ValidateTranslationCompleteness()
        {
            Logger.LogTitle("Validation de la complétude des traductions");

            if (_fallacies == null)
            {
                await LoadTaxonomyData();
            }

            if (_fallacies == null || !_fallacies.Any())
            {
                Logger.LogProblem("Impossible de valider la complétude des traductions : aucune donnée chargée.");
                return;
            }

            var missingTranslations = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (var language in _supportedLanguages)
            {
                missingTranslations[language] = new Dictionary<string, List<string>>();
                missingTranslations[language]["Text"] = new List<string>();
                missingTranslations[language]["Desc"] = new List<string>();
                missingTranslations[language]["Example"] = new List<string>();
                missingTranslations[language]["Family"] = new List<string>();
                missingTranslations[language]["Subfamily"] = new List<string>();
                missingTranslations[language]["Subsubfamily"] = new List<string>();
            }

            foreach (var fallacy in _fallacies)
            {
                // Vérification des traductions en anglais
                if (language_is_missing(fallacy, "en", "Text", fallacy.TextEn))
                    missingTranslations["en"]["Text"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "en", "Desc", fallacy.DescEn))
                    missingTranslations["en"]["Desc"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "en", "Example", fallacy.ExampleEn))
                    missingTranslations["en"]["Example"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "en", "Family", fallacy.Family))
                    missingTranslations["en"]["Family"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "en", "Subfamily", fallacy.Subfamily))
                    missingTranslations["en"]["Subfamily"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "en", "Subsubfamily", fallacy.Subsubfamily))
                    missingTranslations["en"]["Subsubfamily"].Add(fallacy.Path);

                // Vérification des traductions en russe
                if (language_is_missing(fallacy, "ru", "Text", fallacy.TextRu))
                    missingTranslations["ru"]["Text"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "ru", "Desc", fallacy.DescRu))
                    missingTranslations["ru"]["Desc"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "ru", "Example", fallacy.ExampleRu))
                    missingTranslations["ru"]["Example"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "ru", "Family", fallacy.FamilyRu))
                    missingTranslations["ru"]["Family"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "ru", "Subfamily", fallacy.SubfamilyRu))
                    missingTranslations["ru"]["Subfamily"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "ru", "Subsubfamily", fallacy.SubsubfamilyRu))
                    missingTranslations["ru"]["Subsubfamily"].Add(fallacy.Path);

                // Vérification des traductions en portugais
                if (language_is_missing(fallacy, "pt", "Text", fallacy.TextPt))
                    missingTranslations["pt"]["Text"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "pt", "Desc", fallacy.DescPt))
                    missingTranslations["pt"]["Desc"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "pt", "Example", fallacy.ExamplePt))
                    missingTranslations["pt"]["Example"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "pt", "Family", fallacy.FamilyPt))
                    missingTranslations["pt"]["Family"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "pt", "Subfamily", fallacy.SubfamilyPt))
                    missingTranslations["pt"]["Subfamily"].Add(fallacy.Path);
                if (language_is_missing(fallacy, "pt", "Subsubfamily", fallacy.SubsubfamilyPt))
                    missingTranslations["pt"]["Subsubfamily"].Add(fallacy.Path);
            }

            // Génération du rapport
            StringBuilder report = new StringBuilder();
            int totalMissing = 0;

            foreach (var language in _supportedLanguages.Where(l => l != "fr")) // Le français est la langue de référence
            {
                int languageMissing = 0;
                StringBuilder languageReport = new StringBuilder();

                foreach (var field in missingTranslations[language].Keys)
                {
                    var missing = missingTranslations[language][field];
                    if (missing.Any())
                    {
                        languageMissing += missing.Count;
                        languageReport.AppendLine($"  - {field} : {missing.Count} traductions manquantes");
                        
                        // Limiter l'affichage à 5 exemples pour ne pas surcharger le rapport
                        int exampleCount = Math.Min(5, missing.Count);
                        for (int i = 0; i < exampleCount; i++)
                        {
                            languageReport.AppendLine($"    * {missing[i]}");
                        }
                        
                        if (missing.Count > exampleCount)
                        {
                            languageReport.AppendLine($"    * ... et {missing.Count - exampleCount} autres");
                        }
                    }
                }

                if (languageMissing > 0)
                {
                    totalMissing += languageMissing;
                    report.AppendLine($"Langue {language} : {languageMissing} traductions manquantes");
                    report.AppendLine(languageReport.ToString());
                }
                else
                {
                    report.AppendLine($"Langue {language} : toutes les traductions sont présentes");
                }
            }

            // Affichage du rapport
            if (totalMissing > 0)
            {
                Logger.LogProblem($"Validation de la complétude des traductions : {totalMissing} traductions manquantes");
                Logger.Log(report.ToString());
            }
            else
            {
                Logger.LogSuccess("Validation de la complétude des traductions : toutes les traductions sont présentes");
            }
        }

        /// <summary>
        /// Vérifie si une traduction est manquante pour un champ donné.
        /// </summary>
        /// <param name="fallacy">L'argument fallacieux à vérifier.</param>
        /// <param name="language">La langue à vérifier.</param>
        /// <param name="fieldName">Le nom du champ à vérifier.</param>
        /// <param name="fieldValue">La valeur du champ à vérifier.</param>
        /// <returns>True si la traduction est manquante, false sinon.</returns>
        private bool language_is_missing(Fallacy fallacy, string language, string fieldName, string fieldValue)
        {
            // Si le champ est vide en français, on ne considère pas la traduction comme manquante
            if (fieldName == "Text" && string.IsNullOrWhiteSpace(fallacy.TextFr))
                return false;
            if (fieldName == "Desc" && string.IsNullOrWhiteSpace(fallacy.DescFr))
                return false;
            if (fieldName == "Example" && string.IsNullOrWhiteSpace(fallacy.ExampleFr))
                return false;
            if (fieldName == "Family" && string.IsNullOrWhiteSpace(fallacy.Famille))
                return false;
            if (fieldName == "Subfamily" && string.IsNullOrWhiteSpace(fallacy.SousFamille))
                return false;
            if (fieldName == "Subsubfamily" && string.IsNullOrWhiteSpace(fallacy.Soussousfamille))
                return false;

            return string.IsNullOrWhiteSpace(fieldValue);
        }

        /// <summary>
        /// Vérifie la cohérence terminologique.
        /// - Cohérence des termes clés entre les langues
        /// - Détection des incohérences potentielles
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ValidateTerminologyConsistency()
        {
            Logger.LogTitle("Validation de la cohérence terminologique");

            if (_fallacies == null)
            {
                await LoadTaxonomyData();
            }

            if (_fallacies == null || !_fallacies.Any())
            {
                Logger.LogProblem("Impossible de valider la cohérence terminologique : aucune donnée chargée.");
                return;
            }

            // Dictionnaire pour stocker les termes par langue et par niveau hiérarchique
            var termsByLanguageAndLevel = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            foreach (var language in _supportedLanguages)
            {
                termsByLanguageAndLevel[language] = new Dictionary<string, HashSet<string>>
                {
                    ["Family"] = new HashSet<string>(),
                    ["Subfamily"] = new HashSet<string>(),
                    ["Subsubfamily"] = new HashSet<string>()
                };
            }

            // Collecte des termes
            foreach (var fallacy in _fallacies)
            {
                // Français
                if (!string.IsNullOrWhiteSpace(fallacy.Famille))
                    termsByLanguageAndLevel["fr"]["Family"].Add(fallacy.Famille.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(fallacy.SousFamille))
                    termsByLanguageAndLevel["fr"]["Subfamily"].Add(fallacy.SousFamille.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(fallacy.Soussousfamille))
                    termsByLanguageAndLevel["fr"]["Subsubfamily"].Add(fallacy.Soussousfamille.ToLowerInvariant());

                // Anglais
                if (!string.IsNullOrWhiteSpace(fallacy.Family))
                    termsByLanguageAndLevel["en"]["Family"].Add(fallacy.Family.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(fallacy.Subfamily))
                    termsByLanguageAndLevel["en"]["Subfamily"].Add(fallacy.Subfamily.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(fallacy.Subsubfamily))
                    termsByLanguageAndLevel["en"]["Subsubfamily"].Add(fallacy.Subsubfamily.ToLowerInvariant());

                // Russe
                if (!string.IsNullOrWhiteSpace(fallacy.FamilyRu))
                    termsByLanguageAndLevel["ru"]["Family"].Add(fallacy.FamilyRu.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(fallacy.SubfamilyRu))
                    termsByLanguageAndLevel["ru"]["Subfamily"].Add(fallacy.SubfamilyRu.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(fallacy.SubsubfamilyRu))
                    termsByLanguageAndLevel["ru"]["Subsubfamily"].Add(fallacy.SubsubfamilyRu.ToLowerInvariant());

                // Portugais
                if (!string.IsNullOrWhiteSpace(fallacy.FamilyPt))
                    termsByLanguageAndLevel["pt"]["Family"].Add(fallacy.FamilyPt.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(fallacy.SubfamilyPt))
                    termsByLanguageAndLevel["pt"]["Subfamily"].Add(fallacy.SubfamilyPt.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(fallacy.SubsubfamilyPt))
                    termsByLanguageAndLevel["pt"]["Subsubfamily"].Add(fallacy.SubsubfamilyPt.ToLowerInvariant());
            }

            // Vérification de la cohérence du nombre de termes entre les langues
            StringBuilder report = new StringBuilder();
            bool hasInconsistencies = false;

            foreach (var level in new[] { "Family", "Subfamily", "Subsubfamily" })
            {
                report.AppendLine($"Niveau : {level}");
                
                foreach (var language in _supportedLanguages)
                {
                    int termCount = termsByLanguageAndLevel[language][level].Count;
                    report.AppendLine($"  - {language} : {termCount} termes uniques");
                }

                // Vérification de la cohérence du nombre de termes
                var termCounts = _supportedLanguages
                    .Select(lang => termsByLanguageAndLevel[lang][level].Count)
                    .Distinct()
                    .ToList();

                if (termCounts.Count > 1)
                {
                    hasInconsistencies = true;
                    report.AppendLine("  => Incohérence détectée : nombre de termes différent entre les langues");
                }

                report.AppendLine();
            }

            // Vérification des incohérences terminologiques spécifiques
            var terminologyInconsistencies = new List<string>();

            // Parcourir chaque entrée de la taxonomie
            foreach (var fallacy in _fallacies)
            {
                // Vérifier la cohérence des familles
                if (!string.IsNullOrWhiteSpace(fallacy.Famille) && !string.IsNullOrWhiteSpace(fallacy.Family))
                {
                    // Vérifier si d'autres entrées utilisent la même famille en français mais une famille différente en anglais
                    var inconsistentFamilies = _fallacies
                        .Where(f => f.Path != fallacy.Path && 
                                   f.Famille == fallacy.Famille && 
                                   !string.IsNullOrWhiteSpace(f.Family) && 
                                   f.Family != fallacy.Family)
                        .ToList();

                    if (inconsistentFamilies.Any())
                    {
                        hasInconsistencies = true;
                        terminologyInconsistencies.Add($"Famille '{fallacy.Famille}' traduite différemment en anglais : '{fallacy.Family}' vs '{inconsistentFamilies.First().Family}'");
                    }
                }

                // Vérifier la cohérence des sous-familles
                if (!string.IsNullOrWhiteSpace(fallacy.SousFamille) && !string.IsNullOrWhiteSpace(fallacy.Subfamily))
                {
                    var inconsistentSubfamilies = _fallacies
                        .Where(f => f.Path != fallacy.Path && 
                                   f.SousFamille == fallacy.SousFamille && 
                                   !string.IsNullOrWhiteSpace(f.Subfamily) && 
                                   f.Subfamily != fallacy.Subfamily)
                        .ToList();

                    if (inconsistentSubfamilies.Any())
                    {
                        hasInconsistencies = true;
                        terminologyInconsistencies.Add($"Sous-famille '{fallacy.SousFamille}' traduite différemment en anglais : '{fallacy.Subfamily}' vs '{inconsistentSubfamilies.First().Subfamily}'");
                    }
                }
            }

            // Ajouter les incohérences terminologiques au rapport
            if (terminologyInconsistencies.Any())
            {
                report.AppendLine("Incohérences terminologiques détectées :");
                foreach (var inconsistency in terminologyInconsistencies.Take(10)) // Limiter à 10 exemples
                {
                    report.AppendLine($"  - {inconsistency}");
                }
                
                if (terminologyInconsistencies.Count > 10)
                {
                    report.AppendLine($"  - ... et {terminologyInconsistencies.Count - 10} autres incohérences");
                }
            }

            // Affichage du rapport
            if (hasInconsistencies)
            {
                Logger.LogProblem("Validation de la cohérence terminologique : des incohérences ont été détectées");
                Logger.Log(report.ToString());
            }
            else
            {
                Logger.LogSuccess("Validation de la cohérence terminologique : aucune incohérence détectée");
            }
        }

        /// <summary>
        /// Exécute tous les tests de validation et génère un rapport global.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task RunAllValidations()
        {
            Logger.LogTitle("Exécution de tous les tests de validation de taxonomie");

            await LoadTaxonomyData();
            
            if (_fallacies == null || !_fallacies.Any())
            {
                Logger.LogProblem("Impossible d'exécuter les validations : aucune donnée chargée.");
                return;
            }

            await ValidateTaxonomyStructure();
            await ValidateTranslationCompleteness();
            await ValidateTerminologyConsistency();

            Logger.LogTitle("Fin des tests de validation de taxonomie");
        }
    }
}