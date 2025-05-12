using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Configuration pour le système de validation continue
    /// </summary>
    public class ContinuousValidationConfig
    {
        /// <summary>
        /// Indique si la validation de taxonomie doit être exécutée
        /// </summary>
        public bool ValidateTaxonomy { get; set; } = true;

        /// <summary>
        /// Indique si la validation d'ontologie OWL doit être exécutée
        /// </summary>
        public bool ValidateOwl { get; set; } = true;

        /// <summary>
        /// Indique si la validation des cartes générées doit être exécutée
        /// </summary>
        public bool ValidateCards { get; set; } = true;

        /// <summary>
        /// Intervalle de temps entre les validations automatiques (en minutes)
        /// </summary>
        public int ValidationInterval { get; set; } = 60;

        /// <summary>
        /// Indique si les validations doivent être exécutées après chaque modification détectée
        /// </summary>
        public bool ValidateOnChanges { get; set; } = true;

        /// <summary>
        /// Indique si les validations doivent être exécutées après chaque génération
        /// </summary>
        public bool ValidateAfterGeneration { get; set; } = true;

        /// <summary>
        /// Chemin pour le rapport de validation HTML global
        /// </summary>
        public string ValidationReportPath { get; set; } = Path.Combine("Output", "Validation", "continuous_validation_report.html");

        /// <summary>
        /// Répertoire pour stocker l'historique des rapports de validation
        /// </summary>
        public string ValidationHistoryDirectory { get; set; } = Path.Combine("Output", "Validation", "History");

        /// <summary>
        /// Nombre maximum de rapports d'historique à conserver
        /// </summary>
        public int MaxHistoryReports { get; set; } = 10;

        /// <summary>
        /// Liste des répertoires à surveiller pour les modifications
        /// </summary>
        public List<string> WatchDirectories { get; set; } = new List<string>
        {
            "Cards",
            "Generation"
        };

        /// <summary>
        /// Liste des extensions de fichiers à surveiller pour les modifications
        /// </summary>
        public List<string> WatchFileExtensions { get; set; } = new List<string>
        {
            ".csv",
            ".json",
            ".owl",
            ".png",
            ".jpg"
        };

        /// <summary>
        /// Indique si les notifications par email doivent être activées
        /// </summary>
        public bool EnableEmailNotifications { get; set; } = false;

        /// <summary>
        /// Adresse email pour l'envoi des notifications
        /// </summary>
        public string NotificationEmail { get; set; } = "";

        /// <summary>
        /// Serveur SMTP pour l'envoi des emails
        /// </summary>
        public string SmtpServer { get; set; } = "";

        /// <summary>
        /// Port SMTP pour l'envoi des emails
        /// </summary>
        public int SmtpPort { get; set; } = 587;

        /// <summary>
        /// Nom d'utilisateur SMTP pour l'envoi des emails
        /// </summary>
        public string SmtpUsername { get; set; } = "";

        /// <summary>
        /// Mot de passe SMTP pour l'envoi des emails
        /// </summary>
        public string SmtpPassword { get; set; } = "";

        /// <summary>
        /// Niveau de sévérité minimum pour déclencher une notification
        /// </summary>
        // Définition temporaire de l'énumération ValidationSeverity
        public enum ValidationSeverity
        {
            Info,
            Warning,
            Error,
            Critical
        }

        public ValidationSeverity NotificationThreshold { get; set; } = ValidationSeverity.Error;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ContinuousValidationConfig"/>
        /// </summary>
        public ContinuousValidationConfig()
        {
            // Valeurs par défaut déjà définies dans les propriétés
        }

        /// <summary>
        /// Exécute le système de validation continue
        /// </summary>
        /// <param name="config">La configuration de l'application</param>
        /// <returns>Une tâche représentant l'opération asynchrone</returns>
        public async Task Apply(AssetConverterConfig config)
        {
            Logger.LogTitle("Système de validation continue");

            var validationSystem = new ContinuousValidationSystem(config);

            // Exécuter la validation initiale
            await validationSystem.ExecuteAllValidations();

            // Si le mode interactif est activé, démarrer la surveillance des fichiers
            if (ValidateOnChanges)
            {
                validationSystem.StartFileWatcher();
            }

            // Si la validation périodique est activée, démarrer le timer
            if (ValidationInterval > 0)
            {
                validationSystem.ScheduleValidation(ValidationInterval);
            }

            Logger.LogSuccess("Système de validation continue initialisé");
        }
    }
}