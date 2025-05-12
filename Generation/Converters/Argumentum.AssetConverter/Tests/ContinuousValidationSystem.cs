using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Spectre.Console;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Système de validation continue pour le projet Argumentum.
    /// Ce système permet d'exécuter automatiquement les tests de validation (taxonomie, ontologie OWL, cartes générées)
    /// et de générer des rapports de validation.
    /// </summary>
    public class ContinuousValidationSystem
    {
        private readonly AssetConverterConfig _config;
        private readonly ContinuousValidationConfig _validationConfig;
        private readonly TaxonomyValidationTests _taxonomyValidator;
        private readonly OwlOntologyValidationTests _owlValidator;
        private readonly CardGenerationValidationTests _cardValidator;
        private readonly StringBuilder _reportBuilder;
        private readonly Dictionary<string, int> _validationStats;
        private readonly List<ValidationResult> _validationResults;
        private System.Timers.Timer _validationTimer;
        private List<FileSystemWatcher> _fileWatchers;
        private bool _isValidationInProgress;
        private DateTime _lastValidationTime;
        private int _totalErrors;
        private int _totalWarnings;
        private int _successfulValidations;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ContinuousValidationSystem"/>.
        /// </summary>
        /// <param name="config">La configuration de l'application.</param>
        public ContinuousValidationSystem(AssetConverterConfig config)
        {
            _config = config;
            _validationConfig = config.ContinuousValidationConfig;
            _reportBuilder = new StringBuilder();
            _validationStats = new Dictionary<string, int>();
            _validationResults = new List<ValidationResult>();
            _fileWatchers = new List<FileSystemWatcher>();
            _isValidationInProgress = false;
            _lastValidationTime = DateTime.MinValue;
            _totalErrors = 0;
            _totalWarnings = 0;
            _successfulValidations = 0;

            // Initialiser les validateurs
            _taxonomyValidator = new TaxonomyValidationTests(config);
            _owlValidator = new OwlOntologyValidationTests(config);
            _cardValidator = new CardGenerationValidationTests(config);

            // Créer le répertoire d'historique s'il n'existe pas
            if (!Directory.Exists(_validationConfig.ValidationHistoryDirectory))
            {
                Directory.CreateDirectory(_validationConfig.ValidationHistoryDirectory);
            }
        }

        /// <summary>
        /// Exécute tous les tests de validation (taxonomie, ontologie OWL, cartes générées) et génère un rapport global.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task<bool> ExecuteAllValidations()
        {
            if (_isValidationInProgress)
            {
                Logger.LogWarning("Une validation est déjà en cours. Veuillez réessayer plus tard.");
                return false;
            }

            try
            {
                _isValidationInProgress = true;
                _lastValidationTime = DateTime.Now;
                _totalErrors = 0;
                _totalWarnings = 0;
                _validationResults.Clear();

                Logger.LogTitle("Exécution de tous les tests de validation");
                
                // Initialiser le rapport HTML
                InitializeHtmlReport();
                
                // Exécuter les validations configurées
                bool taxonomyResult = true;
                bool owlResult = true;
                bool cardResult = true;

                // Validation de la taxonomie
                if (_validationConfig.ValidateTaxonomy)
                {
                    Logger.Log("Exécution des tests de validation de taxonomie...");
                    taxonomyResult = _taxonomyValidator.RunAllTests();
                    
                    var result = new ValidationResult
                    {
                        Type = "Taxonomie",
                        Success = taxonomyResult,
                        Timestamp = DateTime.Now,
                        Details = "Validation de la structure et de la cohérence de la taxonomie"
                    };
                    
                    _validationResults.Add(result);
                    
                    if (!taxonomyResult)
                    {
                        _totalErrors++;
                    }
                }

                // Validation de l'ontologie OWL
                if (_validationConfig.ValidateOwl)
                {
                    Logger.Log("Exécution des tests de validation d'ontologie OWL...");
                    owlResult = _owlValidator.RunAllTests();
                    
                    var result = new ValidationResult
                    {
                        Type = "Ontologie OWL",
                        Success = owlResult,
                        Timestamp = DateTime.Now,
                        Details = "Validation de la structure et de la cohérence de l'ontologie OWL"
                    };
                    
                    _validationResults.Add(result);
                    
                    if (!owlResult)
                    {
                        _totalErrors++;
                    }
                }

                // Validation des cartes générées
                if (_validationConfig.ValidateCards)
                {
                    Logger.Log("Exécution des tests de validation des cartes générées...");
                    await _cardValidator.RunAllCardValidations();
                    
                    // Déterminer le résultat en fonction des erreurs et avertissements
                    cardResult = _cardValidator._totalErrors == 0;
                    
                    var result = new ValidationResult
                    {
                        Type = "Cartes générées",
                        Success = cardResult,
                        Timestamp = DateTime.Now,
                        Details = $"Validation des cartes générées: {_cardValidator._totalErrors} erreurs, {_cardValidator._totalWarnings} avertissements"
                    };
                    
                    _validationResults.Add(result);
                    
                    if (!cardResult)
                    {
                        _totalErrors += _cardValidator._totalErrors;
                        _totalWarnings += _cardValidator._totalWarnings;
                    }
                }

                // Générer le rapport de validation
                await GenerateValidationReport();
                
                // Envoyer des notifications si nécessaire
                if (_totalErrors > 0 || (_totalWarnings > 0 && _validationConfig.NotificationThreshold == ValidationSeverity.Warning))
                {
                    await NotifyValidationResults();
                }

                // Mettre à jour les statistiques
                UpdateValidationStats();

                // Résultat global
                bool overallResult = taxonomyResult && owlResult && cardResult;
                
                if (overallResult)
                {
                    _successfulValidations++;
                    Logger.LogSuccess("Tous les tests de validation ont réussi.");
                }
                else
                {
                    Logger.LogProblem($"Certains tests de validation ont échoué. Erreurs: {_totalErrors}, Avertissements: {_totalWarnings}");
                }

                return overallResult;
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de l'exécution des validations: {ex.Message}");
                _totalErrors++;
                return false;
            }
            finally
            {
                _isValidationInProgress = false;
            }
        }

        /// <summary>
        /// Génère un rapport HTML complet avec résumé des résultats, détails des problèmes, statistiques et graphiques.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task GenerateValidationReport()
        {
            try
            {
                Logger.Log("Génération du rapport de validation...");

                // Ajouter le résumé des résultats
                _reportBuilder.AppendLine("<h2>Résumé des résultats</h2>");
                _reportBuilder.AppendLine("<table border='1'>");
                _reportBuilder.AppendLine("<tr><th>Type de validation</th><th>Statut</th><th>Détails</th><th>Horodatage</th></tr>");

                foreach (var result in _validationResults)
                {
                    string statusClass = result.Success ? "success" : "error";
                    string statusText = result.Success ? "Succès" : "Échec";
                    
                    _reportBuilder.AppendLine($"<tr><td>{result.Type}</td><td class='{statusClass}'>{statusText}</td>" +
                        $"<td>{result.Details}</td><td>{result.Timestamp:yyyy-MM-dd HH:mm:ss}</td></tr>");
                }

                _reportBuilder.AppendLine("</table>");

                // Ajouter les statistiques
                _reportBuilder.AppendLine("<h2>Statistiques de validation</h2>");
                _reportBuilder.AppendLine("<div class='stats'>");
                _reportBuilder.AppendLine($"<p>Nombre total de validations: {_validationStats.GetValueOrDefault("TotalValidations", 0)}</p>");
                _reportBuilder.AppendLine($"<p>Validations réussies: {_validationStats.GetValueOrDefault("SuccessfulValidations", 0)}</p>");
                _reportBuilder.AppendLine($"<p>Validations échouées: {_validationStats.GetValueOrDefault("FailedValidations", 0)}</p>");
                _reportBuilder.AppendLine($"<p>Taux de réussite: {_validationStats.GetValueOrDefault("SuccessRate", 0)}%</p>");
                _reportBuilder.AppendLine("</div>");

                // Ajouter un graphique simple en HTML/CSS
                _reportBuilder.AppendLine("<h2>Graphique des résultats</h2>");
                _reportBuilder.AppendLine("<div class='chart'>");
                _reportBuilder.AppendLine("<div class='chart-container'>");
                
                int successRate = _validationStats.GetValueOrDefault("SuccessRate", 0);
                int failureRate = 100 - successRate;
                
                _reportBuilder.AppendLine($"<div class='chart-bar success' style='width: {successRate}%;'>{successRate}%</div>");
                _reportBuilder.AppendLine($"<div class='chart-bar error' style='width: {failureRate}%;'>{failureRate}%</div>");
                _reportBuilder.AppendLine("</div>");
                _reportBuilder.AppendLine("<div class='chart-legend'>");
                _reportBuilder.AppendLine("<span class='legend-item'><span class='legend-color success'></span> Succès</span>");
                _reportBuilder.AppendLine("<span class='legend-item'><span class='legend-color error'></span> Échec</span>");
                _reportBuilder.AppendLine("</div>");
                _reportBuilder.AppendLine("</div>");

                // Finaliser le rapport
                FinalizeHtmlReport();

                // Sauvegarder une copie dans l'historique
                string historyFileName = $"validation_report_{DateTime.Now:yyyyMMdd_HHmmss}.html";
                string historyFilePath = Path.Combine(_validationConfig.ValidationHistoryDirectory, historyFileName);
                File.WriteAllText(historyFilePath, _reportBuilder.ToString());

                // Nettoyer les anciens rapports si nécessaire
                CleanupOldReports();

                Logger.LogSuccess($"Rapport de validation généré: {_validationConfig.ValidationReportPath}");
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de la génération du rapport de validation: {ex.Message}");
            }
        }

        /// <summary>
        /// Permet de planifier des validations automatiques (intervalles réguliers, après modifications, après générations).
        /// </summary>
        /// <param name="intervalMinutes">Intervalle en minutes entre les validations.</param>
        public void ScheduleValidation(int intervalMinutes)
        {
            if (intervalMinutes <= 0)
            {
                Logger.LogWarning("L'intervalle de validation doit être supérieur à 0 minute.");
                return;
            }

            try
            {
                // Arrêter le timer existant s'il y en a un
                if (_validationTimer != null)
                {
                    _validationTimer.Stop();
                    _validationTimer.Dispose();
                }

                // Créer un nouveau timer
                _validationTimer = new System.Timers.Timer(intervalMinutes * 60 * 1000); // Convertir en millisecondes
                _validationTimer.Elapsed += async (sender, e) => await OnValidationTimerElapsed(sender, e);
                _validationTimer.AutoReset = true;
                _validationTimer.Start();

                Logger.Log($"Validation automatique planifiée toutes les {intervalMinutes} minutes.");
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de la planification de la validation: {ex.Message}");
            }
        }

        /// <summary>
        /// Démarre la surveillance des fichiers pour déclencher des validations automatiques lors des modifications.
        /// </summary>
        public void StartFileWatcher()
        {
            try
            {
                // Arrêter les watchers existants s'il y en a
                StopFileWatcher();

                // Créer de nouveaux watchers pour chaque répertoire configuré
                foreach (var directory in _validationConfig.WatchDirectories)
                {
                    string fullPath = Path.GetFullPath(directory);
                    
                    if (!Directory.Exists(fullPath))
                    {
                        Logger.LogWarning($"Le répertoire à surveiller n'existe pas: {fullPath}");
                        continue;
                    }

                    var watcher = new FileSystemWatcher(fullPath)
                    {
                        IncludeSubdirectories = true,
                        EnableRaisingEvents = true
                    };

                    // Configurer les filtres d'extension si spécifiés
                    if (_validationConfig.WatchFileExtensions.Count > 0)
                    {
                        watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                    }

                    // Abonner aux événements
                    watcher.Changed += OnFileChanged;
                    watcher.Created += OnFileChanged;
                    watcher.Deleted += OnFileChanged;
                    watcher.Renamed += OnFileRenamed;

                    _fileWatchers.Add(watcher);
                    Logger.Log($"Surveillance des fichiers activée pour: {fullPath}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors du démarrage de la surveillance des fichiers: {ex.Message}");
            }
        }

        /// <summary>
        /// Arrête la surveillance des fichiers.
        /// </summary>
        public void StopFileWatcher()
        {
            foreach (var watcher in _fileWatchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Changed -= OnFileChanged;
                watcher.Created -= OnFileChanged;
                watcher.Deleted -= OnFileChanged;
                watcher.Renamed -= OnFileRenamed;
                watcher.Dispose();
            }

            _fileWatchers.Clear();
            Logger.Log("Surveillance des fichiers désactivée.");
        }

        /// <summary>
        /// Envoie des notifications sur les résultats (email, fichier de log, interface utilisateur).
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task NotifyValidationResults()
        {
            try
            {
                // Vérifier si les notifications par email sont activées
                if (_validationConfig.EnableEmailNotifications && !string.IsNullOrEmpty(_validationConfig.NotificationEmail))
                {
                    // Vérifier le seuil de notification
                    bool shouldNotify = false;
                    
                    if (_validationConfig.NotificationThreshold == ValidationSeverity.Error && _totalErrors > 0)
                    {
                        shouldNotify = true;
                    }
                    else if (_validationConfig.NotificationThreshold == ValidationSeverity.Warning && (_totalErrors > 0 || _totalWarnings > 0))
                    {
                        shouldNotify = true;
                    }
                    else if (_validationConfig.NotificationThreshold == ValidationSeverity.Info)
                    {
                        shouldNotify = true;
                    }

                    if (shouldNotify)
                    {
                        await SendEmailNotification();
                    }
                }

                // Journaliser les résultats
                LogValidationResults();
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de l'envoi des notifications: {ex.Message}");
            }
        }

        /// <summary>
        /// Initialise le rapport HTML.
        /// </summary>
        private void InitializeHtmlReport()
        {
            _reportBuilder.Clear();
            _reportBuilder.AppendLine("<!DOCTYPE html>");
            _reportBuilder.AppendLine("<html>");
            _reportBuilder.AppendLine("<head>");
            _reportBuilder.AppendLine("<meta charset='utf-8'>");
            _reportBuilder.AppendLine("<title>Rapport de validation continue Argumentum</title>");
            _reportBuilder.AppendLine("<style>");
            _reportBuilder.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            _reportBuilder.AppendLine("h1 { color: #333366; }");
            _reportBuilder.AppendLine("h2 { color: #336699; margin-top: 30px; }");
            _reportBuilder.AppendLine("table { border-collapse: collapse; width: 100%; margin-bottom: 20px; }");
            _reportBuilder.AppendLine("th { background-color: #f2f2f2; padding: 8px; text-align: left; }");
            _reportBuilder.AppendLine("td { padding: 8px; }");
            _reportBuilder.AppendLine(".success { color: green; }");
            _reportBuilder.AppendLine(".warning { color: orange; }");
            _reportBuilder.AppendLine(".error { color: red; }");
            _reportBuilder.AppendLine(".summary { font-weight: bold; margin: 20px 0; padding: 10px; background-color: #f8f8f8; }");
            _reportBuilder.AppendLine(".stats { margin: 20px 0; padding: 15px; background-color: #f8f8f8; border-radius: 5px; }");
            _reportBuilder.AppendLine(".chart { margin: 20px 0; }");
            _reportBuilder.AppendLine(".chart-container { height: 30px; background-color: #f0f0f0; border-radius: 5px; overflow: hidden; display: flex; }");
            _reportBuilder.AppendLine(".chart-bar { height: 100%; text-align: center; color: white; line-height: 30px; }");
            _reportBuilder.AppendLine(".chart-bar.success { background-color: green; }");
            _reportBuilder.AppendLine(".chart-bar.error { background-color: red; }");
            _reportBuilder.AppendLine(".chart-legend { margin-top: 10px; display: flex; }");
            _reportBuilder.AppendLine(".legend-item { margin-right: 20px; display: flex; align-items: center; }");
            _reportBuilder.AppendLine(".legend-color { width: 15px; height: 15px; display: inline-block; margin-right: 5px; }");
            _reportBuilder.AppendLine(".legend-color.success { background-color: green; }");
            _reportBuilder.AppendLine(".legend-color.error { background-color: red; }");
            _reportBuilder.AppendLine("</style>");
            _reportBuilder.AppendLine("</head>");
            _reportBuilder.AppendLine("<body>");
            _reportBuilder.AppendLine("<h1>Rapport de validation continue Argumentum</h1>");
            _reportBuilder.AppendLine($"<p>Date de génération : {DateTime.Now}</p>");
            
            // Ajouter un résumé global
            _reportBuilder.AppendLine("<div class='summary'>");
            if (_totalErrors > 0)
            {
                _reportBuilder.AppendLine($"<p class='error'>Validation terminée avec {_totalErrors} erreurs et {_totalWarnings} avertissements</p>");
            }
            else if (_totalWarnings > 0)
            {
                _reportBuilder.AppendLine($"<p class='warning'>Validation terminée avec {_totalWarnings} avertissements</p>");
            }
            else
            {
                _reportBuilder.AppendLine("<p class='success'>Validation terminée avec succès</p>");
            }
            _reportBuilder.AppendLine("</div>");
        }

        /// <summary>
        /// Finalise et enregistre le rapport HTML.
        /// </summary>
        private void FinalizeHtmlReport()
        {
            // Fermer le document HTML
            _reportBuilder.AppendLine("</body>");
            _reportBuilder.AppendLine("</html>");
            
            // Créer le répertoire de sortie si nécessaire
            string reportDirectory = Path.GetDirectoryName(_validationConfig.ValidationReportPath);
            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory);
            }
            
            // Enregistrer le rapport
            File.WriteAllText(_validationConfig.ValidationReportPath, _reportBuilder.ToString());
        }

        /// <summary>
        /// Met à jour les statistiques de validation.
        /// </summary>
        private void UpdateValidationStats()
        {
            int totalValidations = _validationStats.GetValueOrDefault("TotalValidations", 0) + 1;
            int successfulValidations = _validationStats.GetValueOrDefault("SuccessfulValidations", 0);
            
            if (_totalErrors == 0)
            {
                successfulValidations++;
            }
            
            int failedValidations = totalValidations - successfulValidations;
            int successRate = totalValidations > 0 ? (successfulValidations * 100) / totalValidations : 0;
            
            _validationStats["TotalValidations"] = totalValidations;
            _validationStats["SuccessfulValidations"] = successfulValidations;
            _validationStats["FailedValidations"] = failedValidations;
            _validationStats["SuccessRate"] = successRate;
        }

        /// <summary>
        /// Nettoie les anciens rapports d'historique.
        /// </summary>
        private void CleanupOldReports()
        {
            try
            {
                var historyFiles = Directory.GetFiles(_validationConfig.ValidationHistoryDirectory, "validation_report_*.html")
                    .OrderByDescending(f => File.GetCreationTime(f))
                    .Skip(_validationConfig.MaxHistoryReports)
                    .ToList();

                foreach (var file in historyFiles)
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Erreur lors du nettoyage des anciens rapports: {ex.Message}");
            }
        }

        /// <summary>
        /// Envoie une notification par email.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task SendEmailNotification()
        {
            try
            {
                if (string.IsNullOrEmpty(_validationConfig.SmtpServer) || 
                    string.IsNullOrEmpty(_validationConfig.SmtpUsername) || 
                    string.IsNullOrEmpty(_validationConfig.SmtpPassword))
                {
                    Logger.LogWarning("Configuration SMTP incomplète. Impossible d'envoyer l'email de notification.");
                    return;
                }

                using (var client = new SmtpClient(_validationConfig.SmtpServer, _validationConfig.SmtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_validationConfig.SmtpUsername, _validationConfig.SmtpPassword);

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(_validationConfig.SmtpUsername);
                        message.To.Add(_validationConfig.NotificationEmail);
                        message.Subject = $"Rapport de validation Argumentum - {(_totalErrors > 0 ? "ÉCHEC" : "SUCCÈS")}";
                        message.IsBodyHtml = true;

                        // Créer un résumé simple pour l'email
                        StringBuilder emailBody = new StringBuilder();
                        emailBody.AppendLine("<h2>Résumé de la validation Argumentum</h2>");
                        emailBody.AppendLine($"<p>Date: {DateTime.Now}</p>");
                        
                        if (_totalErrors > 0)
                        {
                            emailBody.AppendLine($"<p style='color: red;'>Validation terminée avec {_totalErrors} erreurs et {_totalWarnings} avertissements</p>");
                        }
                        else if (_totalWarnings > 0)
                        {
                            emailBody.AppendLine($"<p style='color: orange;'>Validation terminée avec {_totalWarnings} avertissements</p>");
                        }
                        else
                        {
                            emailBody.AppendLine("<p style='color: green;'>Validation terminée avec succès</p>");
                        }

                        emailBody.AppendLine("<h3>Résultats détaillés</h3>");
                        emailBody.AppendLine("<ul>");
                        
                        foreach (var result in _validationResults)
                        {
                            string color = result.Success ? "green" : "red";
                            emailBody.AppendLine($"<li style='color: {color};'>{result.Type}: {(result.Success ? "Succès" : "Échec")} - {result.Details}</li>");
                        }
                        
                        emailBody.AppendLine("</ul>");
                        emailBody.AppendLine("<p>Pour plus de détails, veuillez consulter le rapport complet.</p>");

                        message.Body = emailBody.ToString();

                        await client.SendMailAsync(message);
                        Logger.Log($"Notification par email envoyée à {_validationConfig.NotificationEmail}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de l'envoi de l'email de notification: {ex.Message}");
            }
        }

        /// <summary>
        /// Journalise les résultats de validation.
        /// </summary>
        private void LogValidationResults()
        {
            Logger.LogTitle("Résultats de validation");
            
            foreach (var result in _validationResults)
            {
                if (result.Success)
                {
                    Logger.LogSuccess($"{result.Type}: {result.Details}");
                }
                else
                {
                    Logger.LogProblem($"{result.Type}: {result.Details}");
                }
            }
            
            if (_totalErrors > 0)
            {
                Logger.LogProblem($"Validation terminée avec {_totalErrors} erreurs et {_totalWarnings} avertissements");
            }
            else if (_totalWarnings > 0)
            {
                Logger.LogWarning($"Validation terminée avec {_totalWarnings} avertissements");
            }
            else
            {
                Logger.LogSuccess("Validation terminée avec succès");
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le timer de validation.
        /// </summary>
        private async Task OnValidationTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_isValidationInProgress)
            {
                Logger.Log("Exécution de la validation planifiée...");
                await ExecuteAllValidations();
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour les modifications de fichiers.
        /// </summary>
        private async void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            // Vérifier si l'extension du fichier est surveillée
            if (_validationConfig.WatchFileExtensions.Count > 0)
            {
                string extension = Path.GetExtension(e.FullPath).ToLowerInvariant();
                if (!_validationConfig.WatchFileExtensions.Contains(extension))
                {
                    return;
                }
            }

            // Éviter les déclenchements multiples en vérifiant le temps écoulé depuis la dernière validation
            if ((DateTime.Now - _lastValidationTime).TotalSeconds < 10)
            {
                return;
            }

            Logger.Log($"Modification détectée: {e.FullPath}");
            Logger.Log("Démarrage de la validation automatique...");
            
            // Attendre un peu pour s'assurer que les fichiers sont complètement écrits
            await Task.Delay(2000);
            
            await ExecuteAllValidations();
        }

        /// <summary>
        /// Gestionnaire d'événement pour les renommages de fichiers.
        /// </summary>
        private async void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            // Même logique que pour OnFileChanged
            await Task.Delay(1);
            OnFileChanged(sender, e);
        }
    }

    /// <summary>
    /// Représente un résultat de validation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Type de validation (Taxonomie, Ontologie OWL, Cartes générées).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Indique si la validation a réussi.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Horodatage de la validation.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Détails de la validation.
        /// </summary>
        public string Details { get; set; }
    }
}