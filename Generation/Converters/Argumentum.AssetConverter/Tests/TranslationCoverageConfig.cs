using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Configuration pour le système de rapport de couverture des traductions
    /// </summary>
    public class TranslationCoverageConfig
    {
        /// <summary>
        /// Liste des langues à analyser pour la couverture des traductions
        /// </summary>
        public List<string> Languages { get; set; } = new List<string> { "fr", "en", "ru", "pt" };

        /// <summary>
        /// Liste des types de champs à analyser pour la couverture des traductions
        /// </summary>
        public List<string> FieldTypes { get; set; } = new List<string> { "Text", "Desc", "Example", "Link" };

        /// <summary>
        /// Seuil de couverture minimum acceptable (en pourcentage)
        /// </summary>
        public int MinimumCoverageThreshold { get; set; } = 80;

        /// <summary>
        /// Chemin pour le rapport HTML de couverture des traductions
        /// </summary>
        public string CoverageReportPath { get; set; } = Path.Combine("Output", "TranslationCoverage", "translation_coverage_report.html");

        /// <summary>
        /// Chemin pour le rapport CSV de couverture des traductions
        /// </summary>
        public string CsvReportPath { get; set; } = Path.Combine("Output", "TranslationCoverage", "translation_coverage_report.csv");

        /// <summary>
        /// Répertoire pour stocker l'historique des rapports de couverture
        /// </summary>
        public string CoverageHistoryDirectory { get; set; } = Path.Combine("Output", "TranslationCoverage", "History");

        /// <summary>
        /// Nombre maximum de rapports d'historique à conserver
        /// </summary>
        public int MaxHistoryReports { get; set; } = 10;

        /// <summary>
        /// Indique si les graphiques de progression doivent être générés
        /// </summary>
        public bool GenerateProgressCharts { get; set; } = true;

        /// <summary>
        /// Indique si les sections les moins bien couvertes doivent être mises en évidence
        /// </summary>
        public bool HighlightLowCoverageSections { get; set; } = true;

        /// <summary>
        /// Nombre de sections à mettre en évidence dans le rapport
        /// </summary>
        public int TopLowCoverageSectionsCount { get; set; } = 5;

        /// <summary>
        /// Indique si les notifications par email doivent être activées
        /// </summary>
        public bool EnableEmailNotifications { get; set; } = false;

        /// <summary>
        /// Adresse email pour l'envoi des notifications
        /// </summary>
        public string NotificationEmail { get; set; } = "";

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="TranslationCoverageConfig"/>
        /// </summary>
        public TranslationCoverageConfig()
        {
            // Valeurs par défaut déjà définies dans les propriétés
        }

        /// <summary>
        /// Exécute le système de rapport de couverture des traductions
        /// </summary>
        /// <param name="config">La configuration de l'application</param>
        /// <returns>Une tâche représentant l'opération asynchrone</returns>
        public async Task Apply(AssetConverterConfig config)
        {
            Logger.LogTitle("Système de rapport de couverture des traductions");

            // Créer les répertoires de sortie si nécessaire
            string reportDirectory = Path.GetDirectoryName(CoverageReportPath);
            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory);
            }

            if (!Directory.Exists(CoverageHistoryDirectory))
            {
                Directory.CreateDirectory(CoverageHistoryDirectory);
            }

            // Exécuter la génération du rapport de couverture
            var coverageReport = new TranslationCoverageReport(config);
            await coverageReport.GenerateTranslationCoverageReport();

            // Exporter les rapports
            await coverageReport.ExportReportToHtml(CoverageReportPath);
            await coverageReport.ExportReportToCsv(CsvReportPath);

            // Suivre la progression dans le temps
            if (GenerateProgressCharts)
            {
                await coverageReport.TrackProgressOverTime(CoverageHistoryDirectory, MaxHistoryReports);
            }

            Logger.LogSuccess("Rapport de couverture des traductions généré avec succès");
        }
    }
}