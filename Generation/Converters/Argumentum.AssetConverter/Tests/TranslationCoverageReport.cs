using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using Spectre.Console;
using System.Globalization;
using System.Text.Json;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Système de génération de rapports de couverture des traductions pour le projet Argumentum.
    /// Ce système permet d'analyser la taxonomie et de générer des rapports sur la couverture des traductions
    /// dans différentes langues (fr, en, ru, pt) pour différents types de champs (Text, Desc, Example, Link).
    /// </summary>
    public class TranslationCoverageReport
    {
        private readonly AssetConverterConfig _config;
        private readonly TranslationCoverageConfig _coverageConfig;
        private readonly Dictionary<string, Dictionary<string, Dictionary<string, double>>> _coverageData;
        private readonly Dictionary<string, Dictionary<string, List<string>>> _missingTranslations;
        private readonly Dictionary<string, Dictionary<string, double>> _overallCoverage;
        private readonly Dictionary<string, Dictionary<string, double>> _historicalData;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="TranslationCoverageReport"/>.
        /// </summary>
        /// <param name="config">La configuration de l'application.</param>
        public TranslationCoverageReport(AssetConverterConfig config)
        {
            _config = config;
            _coverageConfig = config.TranslationCoverageConfig;
            _coverageData = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            _missingTranslations = new Dictionary<string, Dictionary<string, List<string>>>();
            _overallCoverage = new Dictionary<string, Dictionary<string, double>>();
            _historicalData = new Dictionary<string, Dictionary<string, double>>();

            // Initialiser les structures de données
            foreach (var language in _coverageConfig.Languages)
            {
                _coverageData[language] = new Dictionary<string, Dictionary<string, double>>();
                _missingTranslations[language] = new Dictionary<string, List<string>>();
                _overallCoverage[language] = new Dictionary<string, double>();

                foreach (var fieldType in _coverageConfig.FieldTypes)
                {
                    _coverageData[language][fieldType] = new Dictionary<string, double>();
                    _missingTranslations[language][fieldType] = new List<string>();
                }
            }
        }

        /// <summary>
        /// Génère un rapport de couverture des traductions en analysant la taxonomie.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task<bool> GenerateTranslationCoverageReport()
        {
            try
            {
                Logger.LogTitle("Génération du rapport de couverture des traductions");
                Logger.Log("Analyse de la taxonomie pour les langues: " + string.Join(", ", _coverageConfig.Languages));

                // Charger les données de taxonomie
                var fallacies = await LoadTaxonomyData();
                if (fallacies == null || !fallacies.Any())
                {
                    Logger.LogProblem("Impossible de charger les données de taxonomie.");
                    return false;
                }

                Logger.LogSuccess($"Données de taxonomie chargées: {fallacies.Count} entrées");

                // Analyser la couverture des traductions
                AnalyzeCoverage(fallacies);

                // Calculer les pourcentages de couverture globale
                CalculateOverallCoverage();

                // Identifier les sections les moins bien couvertes
                IdentifyLowCoverageSections();

                Logger.LogSuccess("Analyse de la couverture des traductions terminée");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de la génération du rapport de couverture des traductions: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Charge les données de taxonomie à partir du fichier CSV.
        /// </summary>
        /// <returns>Une liste d'objets Fallacy représentant la taxonomie.</returns>
        private async Task<List<Fallacy>> LoadTaxonomyData()
        {
            try
            {
                // Déterminer le chemin du fichier de taxonomie
                string taxonomyFilePath = _config.UseDebugParams
                    ? _config.DataSets.FirstOrDefault(ds => ds.Name == KnownDataSets.FallaciesTaxonomy)?.DebugFilePath
                    : _config.DataSets.FirstOrDefault(ds => ds.Name == KnownDataSets.FallaciesTaxonomy)?.ReleaseFilePath;

                if (string.IsNullOrEmpty(taxonomyFilePath))
                {
                    Logger.LogProblem("Chemin du fichier de taxonomie non trouvé dans la configuration.");
                    return null;
                }

                // Si c'est un chemin relatif local, le convertir en chemin absolu
                if (!taxonomyFilePath.StartsWith("http", StringComparison.OrdinalIgnoreCase) && !Path.IsPathRooted(taxonomyFilePath))
                {
                    taxonomyFilePath = Path.Combine(Environment.CurrentDirectory, taxonomyFilePath);
                }

                Logger.Log($"Chargement des données de taxonomie depuis: {taxonomyFilePath}");

                // Charger et désérialiser les données
                List<Fallacy> fallacies;
                if (taxonomyFilePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    // Télécharger le fichier depuis l'URL
                    using (var client = new System.Net.WebClient())
                    {
                        string csvContent = await client.DownloadStringTaskAsync(taxonomyFilePath);
                        fallacies = ParseCsvContent(csvContent);
                    }
                }
                else
                {
                    // Lire le fichier local
                    if (!File.Exists(taxonomyFilePath))
                    {
                        Logger.LogProblem($"Le fichier de taxonomie n'existe pas: {taxonomyFilePath}");
                        return null;
                    }

                    string csvContent = await File.ReadAllTextAsync(taxonomyFilePath);
                    fallacies = ParseCsvContent(csvContent);
                }

                return fallacies;
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors du chargement des données de taxonomie: {ex.Message}");
                return null;
            }
        }
/// <summary>
        /// Parse le contenu CSV pour créer une liste d'objets Fallacy.
        /// </summary>
        /// <param name="csvContent">Le contenu CSV à parser.</param>
        /// <returns>Une liste d'objets Fallacy.</returns>
        private List<Fallacy> ParseCsvContent(string csvContent)
        {
            // Cette méthode est simplifiée et devrait être adaptée au format réel du CSV
            var fallacies = new List<Fallacy>();
            var lines = csvContent.Split('\n');
            
            // Supposer que la première ligne contient les en-têtes
            var headers = lines[0].Split(',');
            
            // Trouver les indices des colonnes pertinentes
            var headerIndices = new Dictionary<string, int>();
            for (int i = 0; i < headers.Length; i++)
            {
                var header = headers[i].Trim();
                headerIndices[header] = i;
            }
            
            // Parcourir les lignes de données
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;
                
                var values = line.Split(',');
                if (values.Length < headers.Length) continue;
                
                var fallacy = new Fallacy();
                
                // Remplir les propriétés de base
                if (headerIndices.ContainsKey("Id"))
                    fallacy.Id = values[headerIndices["Id"]].Trim();
                
                if (headerIndices.ContainsKey("Famille"))
                    fallacy.Famille = values[headerIndices["Famille"]].Trim();
                
                if (headerIndices.ContainsKey("Sous-Famille"))
                    fallacy.SousFamille = values[headerIndices["Sous-Famille"]].Trim();
                
                if (headerIndices.ContainsKey("Soussousfamille"))
                    fallacy.Soussousfamille = values[headerIndices["Soussousfamille"]].Trim();
                
                // Remplir les propriétés de traduction pour chaque langue et type de champ
                foreach (var language in _coverageConfig.Languages)
                {
                    foreach (var fieldType in _coverageConfig.FieldTypes)
                    {
                        string fieldName = fieldType.ToLowerInvariant();
                        
                        // Pour le français (langue de base)
                        if (language == "fr")
                        {
                            string headerName = $"{fieldName}_fr";
                            if (headerIndices.ContainsKey(headerName))
                                SetFallacyProperty(fallacy, language, fieldType, values[headerIndices[headerName]].Trim());
                        }
                        // Pour les autres langues
                        else
                        {
                            string headerName = $"{fieldName}_{language}";
                            if (headerIndices.ContainsKey(headerName))
                                SetFallacyProperty(fallacy, language, fieldType, values[headerIndices[headerName]].Trim());
                        }
                    }
                }
                
                fallacies.Add(fallacy);
            }
            
            return fallacies;
        }

        /// <summary>
        /// Définit une propriété de traduction sur un objet Fallacy en fonction de la langue et du type de champ.
        /// </summary>
        /// <param name="fallacy">L'objet Fallacy à modifier.</param>
        /// <param name="language">La langue de la traduction.</param>
        /// <param name="fieldType">Le type de champ.</param>
        /// <param name="value">La valeur à définir.</param>
        private void SetFallacyProperty(Fallacy fallacy, string language, string fieldType, string value)
        {
            switch (language)
            {
                case "fr":
                    switch (fieldType.ToLowerInvariant())
                    {
                        case "text": fallacy.TextFr = value; break;
                        case "desc": fallacy.DescFr = value; break;
                        case "example": fallacy.ExampleFr = value; break;
                        case "link": fallacy.LinkFr = value; break;
                    }
                    break;
                case "en":
                    switch (fieldType.ToLowerInvariant())
                    {
                        case "text": fallacy.TextEn = value; break;
                        case "desc": fallacy.DescEn = value; break;
                        case "example": fallacy.ExampleEn = value; break;
                        case "link": fallacy.LinkEn = value; break;
                    }
                    break;
                case "ru":
                    switch (fieldType.ToLowerInvariant())
                    {
                        case "text": fallacy.TextRu = value; break;
                        case "desc": fallacy.DescRu = value; break;
                        case "example": fallacy.ExampleRu = value; break;
                        case "link": fallacy.LinkRu = value; break;
                    }
                    break;
                case "pt":
                    switch (fieldType.ToLowerInvariant())
                    {
                        case "text": fallacy.TextPt = value; break;
                        case "desc": fallacy.DescPt = value; break;
                        case "example": fallacy.ExamplePt = value; break;
                        case "link": fallacy.LinkPt = value; break;
                    }
                    break;
            }
        }
/// <summary>
        /// Récupère la valeur d'une propriété de traduction d'un objet Fallacy en fonction de la langue et du type de champ.
        /// </summary>
        /// <param name="fallacy">L'objet Fallacy à interroger.</param>
        /// <param name="language">La langue de la traduction.</param>
        /// <param name="fieldType">Le type de champ.</param>
        /// <returns>La valeur de la propriété.</returns>
        private string GetFallacyProperty(Fallacy fallacy, string language, string fieldType)
        {
            switch (language)
            {
                case "fr":
                    switch (fieldType.ToLowerInvariant())
                    {
                        case "text": return fallacy.TextFr;
                        case "desc": return fallacy.DescFr;
                        case "example": return fallacy.ExampleFr;
                        case "link": return fallacy.LinkFr;
                    }
                    break;
                case "en":
                    switch (fieldType.ToLowerInvariant())
                    {
                        case "text": return fallacy.TextEn;
                        case "desc": return fallacy.DescEn;
                        case "example": return fallacy.ExampleEn;
                        case "link": return fallacy.LinkEn;
                    }
                    break;
                case "ru":
                    switch (fieldType.ToLowerInvariant())
                    {
                        case "text": return fallacy.TextRu;
                        case "desc": return fallacy.DescRu;
                        case "example": return fallacy.ExampleRu;
                        case "link": return fallacy.LinkRu;
                    }
                    break;
                case "pt":
                    switch (fieldType.ToLowerInvariant())
                    {
                        case "text": return fallacy.TextPt;
                        case "desc": return fallacy.DescPt;
                        case "example": return fallacy.ExamplePt;
                        case "link": return fallacy.LinkPt;
                    }
                    break;
            }
            return null;
        }

        /// <summary>
        /// Analyse la couverture des traductions pour chaque langue et type de champ.
        /// </summary>
        /// <param name="fallacies">La liste des fallacies à analyser.</param>
        private void AnalyzeCoverage(List<Fallacy> fallacies)
        {
            Logger.Log("Analyse de la couverture des traductions...");

            // Regrouper les fallacies par famille
            var fallaciesByFamily = fallacies.GroupBy(f => f.Famille).ToDictionary(g => g.Key, g => g.ToList());

            // Pour chaque langue (sauf le français qui est la langue de base)
            foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
            {
                Logger.Log($"Analyse de la couverture pour la langue: {language}");

                // Pour chaque famille
                foreach (var family in fallaciesByFamily.Keys)
                {
                    var familyFallacies = fallaciesByFamily[family];
                    
                    // Pour chaque type de champ
                    foreach (var fieldType in _coverageConfig.FieldTypes)
                    {
                        int totalFields = familyFallacies.Count;
                        int translatedFields = 0;

                        // Compter les champs traduits
                        foreach (var fallacy in familyFallacies)
                        {
                            string frValue = GetFallacyProperty(fallacy, "fr", fieldType);
                            string translatedValue = GetFallacyProperty(fallacy, language, fieldType);

                            if (!string.IsNullOrEmpty(frValue) && !string.IsNullOrEmpty(translatedValue))
                            {
                                translatedFields++;
                            }
                            else if (!string.IsNullOrEmpty(frValue) && string.IsNullOrEmpty(translatedValue))
                            {
                                // Ajouter à la liste des traductions manquantes
                                _missingTranslations[language][fieldType].Add($"{family} - {fallacy.Id} - {fieldType}");
                            }
                        }

                        // Calculer le pourcentage de couverture
                        double coverage = totalFields > 0 ? (double)translatedFields / totalFields * 100 : 0;
                        _coverageData[language][fieldType][family] = coverage;
                    }
                }
            }

            Logger.LogSuccess("Analyse de la couverture terminée");
        }

        /// <summary>
        /// Calcule les pourcentages de couverture globale pour chaque langue et type de champ.
        /// </summary>
        private void CalculateOverallCoverage()
        {
            Logger.Log("Calcul des pourcentages de couverture globale...");

            // Pour chaque langue (sauf le français qui est la langue de base)
            foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
            {
                // Pour chaque type de champ
                foreach (var fieldType in _coverageConfig.FieldTypes)
                {
                    // Calculer la moyenne des pourcentages de couverture pour toutes les familles
                    var familyCoverages = _coverageData[language][fieldType].Values.ToList();
                    double overallCoverage = familyCoverages.Count > 0 ? familyCoverages.Average() : 0;
                    
                    _overallCoverage[language][fieldType] = overallCoverage;
                }

                // Calculer la couverture globale pour la langue (moyenne de tous les types de champs)
                double languageOverallCoverage = _overallCoverage[language].Values.Average();
                _overallCoverage[language]["Overall"] = languageOverallCoverage;

                Logger.Log($"Couverture globale pour {language}: {languageOverallCoverage:F2}%");
            }

            Logger.LogSuccess("Calcul des pourcentages de couverture globale terminé");
        }

        /// <summary>
        /// Identifie les sections les moins bien couvertes pour chaque langue et type de champ.
        /// </summary>
        private void IdentifyLowCoverageSections()
        {
            if (!_coverageConfig.HighlightLowCoverageSections)
                return;

            Logger.Log("Identification des sections les moins bien couvertes...");

            // Pour chaque langue (sauf le français qui est la langue de base)
            foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
            {
                Logger.Log($"Sections les moins bien couvertes pour {language}:");

                // Pour chaque type de champ
                foreach (var fieldType in _coverageConfig.FieldTypes)
                {
                    // Trier les familles par couverture croissante
                    var sortedFamilies = _coverageData[language][fieldType]
                        .OrderBy(kv => kv.Value)
                        .Take(_coverageConfig.TopLowCoverageSectionsCount)
                        .ToList();

                    Logger.Log($"  Type de champ: {fieldType}");
                    foreach (var family in sortedFamilies)
                    {
                        Logger.Log($"    {family.Key}: {family.Value:F2}%");
                    }
                }
            }

            Logger.LogSuccess("Identification des sections les moins bien couvertes terminée");
        }

        /// <summary>
        /// Exporte le rapport de couverture des traductions au format HTML.
        /// </summary>
        /// <param name="outputPath">Le chemin du fichier HTML de sortie.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ExportReportToHtml(string outputPath)
        {
            try
            {
                Logger.Log($"Exportation du rapport HTML vers: {outputPath}");

                var sb = new StringBuilder();
                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html lang=\"fr\">");
                sb.AppendLine("<head>");
                sb.AppendLine("  <meta charset=\"UTF-8\">");
                sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                sb.AppendLine("  <title>Rapport de couverture des traductions - Argumentum</title>");
                sb.AppendLine("  <style>");
                sb.AppendLine("    body { font-family: Arial, sans-serif; margin: 20px; }");
                sb.AppendLine("    h1, h2, h3 { color: #333366; }");
                sb.AppendLine("    table { border-collapse: collapse; width: 100%; margin-bottom: 20px; }");
                sb.AppendLine("    th { background-color: #f2f2f2; padding: 8px; text-align: left; }");
                sb.AppendLine("    td { padding: 8px; }");
                sb.AppendLine("    tr:nth-child(even) { background-color: #f9f9f9; }");
                sb.AppendLine("    .good { background-color: #d4edda; }");
                sb.AppendLine("    .warning { background-color: #fff3cd; }");
                sb.AppendLine("    .danger { background-color: #f8d7da; }");
                sb.AppendLine("    .chart-container { width: 100%; height: 400px; }");
                sb.AppendLine("    .filter-container { margin-bottom: 20px; padding: 10px; background-color: #f8f8f8; border-radius: 5px; }");
                sb.AppendLine("    .missing-list { max-height: 300px; overflow-y: auto; border: 1px solid #ddd; padding: 10px; }");
                sb.AppendLine("  </style>");
                sb.AppendLine("  <script src=\"https://cdn.jsdelivr.net/npm/chart.js\"></script>");
                sb.AppendLine("  <script>");
                sb.AppendLine("    function filterTable(tableId, languageFilter, fieldTypeFilter) {");
                sb.AppendLine("      const table = document.getElementById(tableId);");
                sb.AppendLine("      const rows = table.getElementsByTagName('tr');");
                sb.AppendLine("      ");
                sb.AppendLine("      for (let i = 1; i < rows.length; i++) {");
                sb.AppendLine("        const row = rows[i];");
                sb.AppendLine("        const language = row.getAttribute('data-language');");
                sb.AppendLine("        const fieldType = row.getAttribute('data-field-type');");
                sb.AppendLine("        ");
                sb.AppendLine("        if ((languageFilter === 'all' || language === languageFilter) &&");
                sb.AppendLine("            (fieldTypeFilter === 'all' || fieldType === fieldTypeFilter)) {");
                sb.AppendLine("          row.style.display = '';");
                sb.AppendLine("        } else {");
                sb.AppendLine("          row.style.display = 'none';");
                sb.AppendLine("        }");
                sb.AppendLine("      }");
                sb.AppendLine("    }");
                sb.AppendLine("    ");
                sb.AppendLine("    function filterMissingList(languageFilter, fieldTypeFilter) {");
                sb.AppendLine("      const lists = document.getElementsByClassName('missing-list-item');");
                sb.AppendLine("      ");
                sb.AppendLine("      for (let i = 0; i < lists.length; i++) {");
                sb.AppendLine("        const item = lists[i];");
                sb.AppendLine("        const language = item.getAttribute('data-language');");
                sb.AppendLine("        const fieldType = item.getAttribute('data-field-type');");
                sb.AppendLine("        ");
                sb.AppendLine("        if ((languageFilter === 'all' || language === languageFilter) &&");
                sb.AppendLine("            (fieldTypeFilter === 'all' || fieldType === fieldTypeFilter)) {");
                sb.AppendLine("          item.style.display = '';");
                sb.AppendLine("        } else {");
                sb.AppendLine("          item.style.display = 'none';");
                sb.AppendLine("        }");
                sb.AppendLine("      }");
                sb.AppendLine("    }");
                sb.AppendLine("  </script>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
                sb.AppendLine("  <h1>Rapport de couverture des traductions - Argumentum</h1>");
                sb.AppendLine($"  <p>Date de génération: {DateTime.Now}</p>");

                // Résumé global
                sb.AppendLine("  <h2>Résumé global</h2>");
                sb.AppendLine("  <table>");
                sb.AppendLine("    <tr>");
                sb.AppendLine("      <th>Langue</th>");
                sb.AppendLine("      <th>Couverture globale</th>");
                foreach (var fieldType in _coverageConfig.FieldTypes)
                {
                    sb.AppendLine($"      <th>{fieldType}</th>");
                }
                sb.AppendLine("    </tr>");

                foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
                {
                    double overallCoverage = _overallCoverage[language]["Overall"];
                    string cssClass = GetCoverageClass(overallCoverage);

                    sb.AppendLine("    <tr>");
                    sb.AppendLine($"      <td>{language.ToUpper()}</td>");
                    sb.AppendLine($"      <td class=\"{cssClass}\">{overallCoverage:F2}%</td>");

                    foreach (var fieldType in _coverageConfig.FieldTypes)
                    {
                        double coverage = _overallCoverage[language][fieldType];
                        cssClass = GetCoverageClass(coverage);
                        sb.AppendLine($"      <td class=\"{cssClass}\">{coverage:F2}%</td>");
                    }

                    sb.AppendLine("    </tr>");
                }

                sb.AppendLine("  </table>");

                // Graphique de couverture globale
                sb.AppendLine("  <h2>Graphique de couverture globale</h2>");
                sb.AppendLine("  <div class=\"chart-container\">");
                sb.AppendLine("    <canvas id=\"overallCoverageChart\"></canvas>");
                sb.AppendLine("  </div>");

                // Filtres pour le tableau détaillé
                sb.AppendLine("  <h2>Couverture détaillée par famille</h2>");
                sb.AppendLine("  <div class=\"filter-container\">");
                sb.AppendLine("    <label for=\"languageFilter\">Langue:</label>");
                sb.AppendLine("    <select id=\"languageFilter\" onchange=\"filterTable('detailedCoverageTable', this.value, document.getElementById('fieldTypeFilter').value)\">");
                sb.AppendLine("      <option value=\"all\">Toutes</option>");
                foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
                {
                    sb.AppendLine($"      <option value=\"{language}\">{language.ToUpper()}</option>");
                }
                sb.AppendLine("    </select>");
                sb.AppendLine("    <label for=\"fieldTypeFilter\">Type de champ:</label>");
                sb.AppendLine("    <select id=\"fieldTypeFilter\" onchange=\"filterTable('detailedCoverageTable', document.getElementById('languageFilter').value, this.value)\">");
                sb.AppendLine("      <option value=\"all\">Tous</option>");
                foreach (var fieldType in _coverageConfig.FieldTypes)
                {
                    sb.AppendLine($"      <option value=\"{fieldType}\">{fieldType}</option>");
                }
                sb.AppendLine("    </select>");
                sb.AppendLine("  </div>");

                // Tableau détaillé
                sb.AppendLine("  <table id=\"detailedCoverageTable\">");
                sb.AppendLine("    <tr>");
                sb.AppendLine("      <th>Langue</th>");
                sb.AppendLine("      <th>Type de champ</th>");
                sb.AppendLine("      <th>Famille</th>");
                sb.AppendLine("      <th>Couverture</th>");
                sb.AppendLine("    </tr>");

                foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
                {
                    foreach (var fieldType in _coverageConfig.FieldTypes)
                    {
                        foreach (var family in _coverageData[language][fieldType].Keys)
                        {
                            double coverage = _coverageData[language][fieldType][family];
                            string cssClass = GetCoverageClass(coverage);

                            sb.AppendLine($"    <tr data-language=\"{language}\" data-field-type=\"{fieldType}\">");
                            sb.AppendLine($"      <td>{language.ToUpper()}</td>");
                            sb.AppendLine($"      <td>{fieldType}</td>");
                            sb.AppendLine($"      <td>{family}</td>");
                            sb.AppendLine($"      <td class=\"{cssClass}\">{coverage:F2}%</td>");
                            sb.AppendLine("    </tr>");
                        }
                    }
                }

                sb.AppendLine("  </table>");

                // Liste des éléments manquants
                sb.AppendLine("  <h2>Éléments manquants</h2>");
                sb.AppendLine("  <div class=\"filter-container\">");
                sb.AppendLine("    <label for=\"missingLanguageFilter\">Langue:</label>");
                sb.AppendLine("    <select id=\"missingLanguageFilter\" onchange=\"filterMissingList(this.value, document.getElementById('missingFieldTypeFilter').value)\">");
                sb.AppendLine("      <option value=\"all\">Toutes</option>");
                foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
                {
                    sb.AppendLine($"      <option value=\"{language}\">{language.ToUpper()}</option>");
                }
                sb.AppendLine("    </select>");
                sb.AppendLine("    <label for=\"missingFieldTypeFilter\">Type de champ:</label>");
                sb.AppendLine("    <select id=\"missingFieldTypeFilter\" onchange=\"filterMissingList(document.getElementById('missingLanguageFilter').value, this.value)\">");
                sb.AppendLine("      <option value=\"all\">Tous</option>");
                foreach (var fieldType in _coverageConfig.FieldTypes)
                {
                    sb.AppendLine($"      <option value=\"{fieldType}\">{fieldType}</option>");
                }
                sb.AppendLine("    </select>");
                sb.AppendLine("  </div>");

                sb.AppendLine("  <div class=\"missing-list\">");
                foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
                {
                    foreach (var fieldType in _coverageConfig.FieldTypes)
                    {
                        var missingItems = _missingTranslations[language][fieldType];
                        if (missingItems.Any())
                        {
                            sb.AppendLine($"    <div class=\"missing-list-item\" data-language=\"{language}\" data-field-type=\"{fieldType}\">");
                            sb.AppendLine($"      <h3>{language.ToUpper()} - {fieldType}</h3>");
                            sb.AppendLine("      <ul>");
                            foreach (var item in missingItems)
                            {
                                sb.AppendLine($"        <li>{item}</li>");
                            }
                            sb.AppendLine("      </ul>");
                            sb.AppendLine("    </div>");
                        }
                    }
                }
                sb.AppendLine("  </div>");

                // JavaScript pour les graphiques
                sb.AppendLine("  <script>");
                sb.AppendLine("    document.addEventListener('DOMContentLoaded', function() {");
                sb.AppendLine("      // Graphique de couverture globale");
                sb.AppendLine("      const overallCtx = document.getElementById('overallCoverageChart').getContext('2d');");
                sb.AppendLine("      new Chart(overallCtx, {");
                sb.AppendLine("        type: 'bar',");
                sb.AppendLine("        data: {");
                sb.AppendLine("          labels: [" + string.Join(", ", _coverageConfig.Languages.Where(l => l != "fr").Select(l => $"'{l.ToUpper()}'")) + "],");
                sb.AppendLine("          datasets: [");
                sb.AppendLine("            {");
                sb.AppendLine("              label: 'Couverture globale (%)',");
                sb.AppendLine("              data: [" + string.Join(", ", _coverageConfig.Languages.Where(l => l != "fr").Select(l => _overallCoverage[l]["Overall"].ToString(CultureInfo.InvariantCulture))) + "],");
                sb.AppendLine("              backgroundColor: 'rgba(54, 162, 235, 0.5)',");
                sb.AppendLine("              borderColor: 'rgba(54, 162, 235, 1)',");
                sb.AppendLine("              borderWidth: 1");
                sb.AppendLine("            }");
                sb.AppendLine("          ]");
                sb.AppendLine("        },");
                sb.AppendLine("        options: {");
                sb.AppendLine("          scales: {");
                sb.AppendLine("            y: {");
                sb.AppendLine("              beginAtZero: true,");
                sb.AppendLine("              max: 100");
                sb.AppendLine("            }");
                sb.AppendLine("          }");
                sb.AppendLine("        }");
                sb.AppendLine("      });");
                sb.AppendLine("    });");
                sb.AppendLine("  </script>");

                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                // Créer le répertoire de sortie si nécessaire
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Écrire le fichier HTML
                await File.WriteAllTextAsync(outputPath, sb.ToString());

                Logger.LogSuccess($"Rapport HTML exporté avec succès: {outputPath}");
                return;
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de l'exportation du rapport HTML: {ex.Message}");
            }
        }

        /// <summary>
        /// Exporte le rapport de couverture des traductions au format CSV.
        /// </summary>
        /// <param name="outputPath">Le chemin du fichier CSV de sortie.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ExportReportToCsv(string outputPath)
        {
            try
            {
                Logger.Log($"Exportation du rapport CSV vers: {outputPath}");

                var sb = new StringBuilder();

                // En-têtes
                sb.Append("Langue,Type de champ,Famille,Couverture (%)\n");

                // Données
                foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
                {
                    foreach (var fieldType in _coverageConfig.FieldTypes)
                    {
                        foreach (var family in _coverageData[language][fieldType].Keys)
                        {
                            double coverage = _coverageData[language][fieldType][family];
                            sb.Append($"{language},{fieldType},{family},{coverage:F2}\n");
                        }
                    }
                }

                // Créer le répertoire de sortie si nécessaire
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Écrire le fichier CSV
                await File.WriteAllTextAsync(outputPath, sb.ToString());

                Logger.LogSuccess($"Rapport CSV exporté avec succès: {outputPath}");
                return;
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de l'exportation du rapport CSV: {ex.Message}");
            }
        }

        /// <summary>
        /// Suit l'évolution de la couverture des traductions dans le temps.
        /// </summary>
        /// <param name="historyDirectory">Le répertoire pour stocker l'historique.</param>
        /// <param name="maxHistoryReports">Le nombre maximum de rapports d'historique à conserver.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task TrackProgressOverTime(string historyDirectory, int maxHistoryReports)
        {
            try
            {
                Logger.Log("Suivi de la progression dans le temps...");

                // Créer le répertoire d'historique si nécessaire
                if (!Directory.Exists(historyDirectory))
                {
                    Directory.CreateDirectory(historyDirectory);
                }

                // Préparer les données actuelles
                var currentData = new Dictionary<string, Dictionary<string, double>>();
                foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
                {
                    currentData[language] = new Dictionary<string, double>();
                    foreach (var fieldType in _coverageConfig.FieldTypes)
                    {
                        currentData[language][fieldType] = _overallCoverage[language][fieldType];
                    }
                    currentData[language]["Overall"] = _overallCoverage[language]["Overall"];
                }

                // Ajouter la date actuelle
                var historyEntry = new
                {
                    Date = DateTime.Now,
                    Coverage = currentData
                };

                // Charger l'historique existant
                var historyFile = Path.Combine(historyDirectory, "translation_coverage_history.json");
                List<object> history = new List<object>();

                if (File.Exists(historyFile))
                {
                    try
                    {
                        string historyJson = await File.ReadAllTextAsync(historyFile);
                        history = JsonSerializer.Deserialize<List<object>>(historyJson);
                    }
                    catch
                    {
                        Logger.LogWarning("Impossible de charger l'historique existant. Un nouveau fichier sera créé.");
                        history = new List<object>();
                    }
                }

                // Ajouter l'entrée actuelle
                history.Add(historyEntry);

                // Limiter le nombre d'entrées d'historique
                if (history.Count > maxHistoryReports)
                {
                    history = history.Skip(history.Count - maxHistoryReports).ToList();
                }

                // Enregistrer l'historique
                string json = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(historyFile, json);

                // Générer un graphique de tendance
                await GenerateProgressChart(historyDirectory, history);

                Logger.LogSuccess("Suivi de la progression terminé");
                return;
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors du suivi de la progression: {ex.Message}");
            }
        }

        /// <summary>
        /// Génère un graphique de tendance pour la progression de la couverture des traductions.
        /// </summary>
        /// <param name="outputDirectory">Le répertoire de sortie pour le graphique.</param>
        /// <param name="history">L'historique des données de couverture.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task GenerateProgressChart(string outputDirectory, List<object> history)
        {
            try
            {
                var chartFile = Path.Combine(outputDirectory, "translation_coverage_progress.html");

                var sb = new StringBuilder();
                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html lang=\"fr\">");
                sb.AppendLine("<head>");
                sb.AppendLine("  <meta charset=\"UTF-8\">");
                sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                sb.AppendLine("  <title>Progression de la couverture des traductions - Argumentum</title>");
                sb.AppendLine("  <style>");
                sb.AppendLine("    body { font-family: Arial, sans-serif; margin: 20px; }");
                sb.AppendLine("    h1, h2 { color: #333366; }");
                sb.AppendLine("    .chart-container { width: 100%; height: 400px; margin-bottom: 30px; }");
                sb.AppendLine("  </style>");
                sb.AppendLine("  <script src=\"https://cdn.jsdelivr.net/npm/chart.js\"></script>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
                sb.AppendLine("  <h1>Progression de la couverture des traductions - Argumentum</h1>");

                // Graphique global
                sb.AppendLine("  <h2>Progression globale</h2>");
                sb.AppendLine("  <div class=\"chart-container\">");
                sb.AppendLine("    <canvas id=\"globalProgressChart\"></canvas>");
                sb.AppendLine("  </div>");

                // Graphiques par langue
                foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
                {
                    sb.AppendLine($"  <h2>Progression pour {language.ToUpper()}</h2>");
                    sb.AppendLine("  <div class=\"chart-container\">");
                    sb.AppendLine($"    <canvas id=\"{language}ProgressChart\"></canvas>");
                    sb.AppendLine("  </div>");
                }

                // JavaScript pour les graphiques
                sb.AppendLine("  <script>");
                sb.AppendLine("    document.addEventListener('DOMContentLoaded', function() {");
                
                // Données pour les graphiques (à remplacer par les données réelles)
                sb.AppendLine("      // Données d'historique");
                sb.AppendLine("      const historyData = " + JsonSerializer.Serialize(history) + ";");
                
                // Graphique global
                sb.AppendLine("      // Graphique global");
                sb.AppendLine("      const globalCtx = document.getElementById('globalProgressChart').getContext('2d');");
                sb.AppendLine("      new Chart(globalCtx, {");
                sb.AppendLine("        type: 'line',");
                sb.AppendLine("        data: {");
                sb.AppendLine("          labels: historyData.map(entry => new Date(entry.Date).toLocaleDateString()),");
                sb.AppendLine("          datasets: [");
                
                // Un dataset par langue
                var colors = new[] { "rgba(54, 162, 235, 1)", "rgba(255, 99, 132, 1)", "rgba(75, 192, 192, 1)", "rgba(255, 159, 64, 1)" };
                int colorIndex = 0;
                
                foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
                {
                    string color = colors[colorIndex % colors.Length];
                    colorIndex++;
                    
                    sb.AppendLine("            {");
                    sb.AppendLine($"              label: '{language.ToUpper()} - Global',");
                    sb.AppendLine($"              data: historyData.map(entry => entry.Coverage['{language}']['Overall']),");
                    sb.AppendLine($"              borderColor: '{color}',");
                    sb.AppendLine("              tension: 0.1,");
                    sb.AppendLine("              fill: false");
                    sb.AppendLine("            },");
                }
                
                // Supprimer la virgule finale
                sb.Remove(sb.Length - 3, 1);
                
                sb.AppendLine("          ]");
                sb.AppendLine("        },");
                sb.AppendLine("        options: {");
                sb.AppendLine("          scales: {");
                sb.AppendLine("            y: {");
                sb.AppendLine("              beginAtZero: true,");
                sb.AppendLine("              max: 100,");
                sb.AppendLine("              title: {");
                sb.AppendLine("                display: true,");
                sb.AppendLine("                text: 'Couverture (%)'");
                sb.AppendLine("              }");
                sb.AppendLine("            },");
                sb.AppendLine("            x: {");
                sb.AppendLine("              title: {");
                sb.AppendLine("                display: true,");
                sb.AppendLine("                text: 'Date'");
                sb.AppendLine("              }");
                sb.AppendLine("            }");
                sb.AppendLine("          }");
                sb.AppendLine("        }");
                sb.AppendLine("      });");
                
                // Graphiques par langue
                foreach (var language in _coverageConfig.Languages.Where(l => l != "fr"))
                {
                    sb.AppendLine($"      // Graphique pour {language}");
                    sb.AppendLine($"      const {language}Ctx = document.getElementById('{language}ProgressChart').getContext('2d');");
                    sb.AppendLine($"      new Chart({language}Ctx, {{");
                    sb.AppendLine("        type: 'line',");
                    sb.AppendLine("        data: {");
                    sb.AppendLine("          labels: historyData.map(entry => new Date(entry.Date).toLocaleDateString()),");
                    sb.AppendLine("          datasets: [");
                    
                    // Un dataset par type de champ
                    colorIndex = 0;
                    foreach (var fieldType in _coverageConfig.FieldTypes)
                    {
                        string color = colors[colorIndex % colors.Length];
                        colorIndex++;
                        
                        sb.AppendLine("            {");
                        sb.AppendLine($"              label: '{fieldType}',");
                        sb.AppendLine($"              data: historyData.map(entry => entry.Coverage['{language}']['{fieldType}']),");
                        sb.AppendLine($"              borderColor: '{color}',");
                        sb.AppendLine("              tension: 0.1,");
                        sb.AppendLine("              fill: false");
                        sb.AppendLine("            },");
                    }
                    
                    // Supprimer la virgule finale
                    sb.Remove(sb.Length - 3, 1);
                    
                    sb.AppendLine("          ]");
                    sb.AppendLine("        },");
                    sb.AppendLine("        options: {");
                    sb.AppendLine("          scales: {");
                    sb.AppendLine("            y: {");
                    sb.AppendLine("              beginAtZero: true,");
                    sb.AppendLine("              max: 100,");
                    sb.AppendLine("              title: {");
                    sb.AppendLine("                display: true,");
                    sb.AppendLine("                text: 'Couverture (%)'");
                    sb.AppendLine("              }");
                    sb.AppendLine("            },");
                    sb.AppendLine("            x: {");
                    sb.AppendLine("              title: {");
                    sb.AppendLine("                display: true,");
                    sb.AppendLine("                text: 'Date'");
                    sb.AppendLine("              }");
                    sb.AppendLine("            }");
                    sb.AppendLine("          }");
                    sb.AppendLine("        }");
                    sb.AppendLine("      });");
                }
                
                sb.AppendLine("    });");
                sb.AppendLine("  </script>");
                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                await File.WriteAllTextAsync(chartFile, sb.ToString());
                Logger.LogSuccess($"Graphique de progression généré: {chartFile}");
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de la génération du graphique de progression: {ex.Message}");
            }
        }

        /// <summary>
        /// Détermine la classe CSS à utiliser en fonction du pourcentage de couverture.
        /// </summary>
        /// <param name="coverage">Le pourcentage de couverture.</param>
        /// <returns>La classe CSS correspondante.</returns>
        private string GetCoverageClass(double coverage)
        {
            if (coverage >= _coverageConfig.MinimumCoverageThreshold)
                return "good";
            else if (coverage >= _coverageConfig.MinimumCoverageThreshold * 0.7)
                return "warning";
            else
                return "danger";
        }
    }
}