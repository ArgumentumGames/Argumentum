using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
// Correction de la directive using
using static Argumentum.AssetConverter.WebBasedGenerator;
using System.Text.RegularExpressions;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Tests de validation pour les cartes générées.
    /// Ces tests permettent de vérifier la qualité et la cohérence des cartes dans toutes les langues supportées.
    /// </summary>
    public class CardGenerationValidationTests
    {
        private readonly AssetConverterConfig _config;
        private readonly CardValidatorConfig _validatorConfig;
        private StringBuilder _reportBuilder;
        private int _totalErrors;
        private int _totalWarnings;
        private Dictionary<string, List<string>> _cardFilesByLanguage;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CardGenerationValidationTests"/>.
        /// </summary>
        /// <param name="config">La configuration de l'application.</param>
        public CardGenerationValidationTests(AssetConverterConfig config)
        {
            _config = config;
            _validatorConfig = config.CardValidatorConfig;
            _reportBuilder = new StringBuilder();
            _totalErrors = 0;
            _totalWarnings = 0;
            _cardFilesByLanguage = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Vérifie l'existence des fichiers de cartes pour chaque jeu et langue.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ValidateCardFilesExistence()
        {
            Logger.LogTitle("Validation de l'existence des fichiers de cartes");
            _reportBuilder.AppendLine("<h2>Validation de l'existence des fichiers de cartes</h2>");
            _reportBuilder.AppendLine("<table border='1'>");
            _reportBuilder.AppendLine("<tr><th>Type de jeu</th><th>Langue</th><th>Statut</th><th>Détails</th></tr>");

            int errorCount = 0;
            
            foreach (var cardSetType in _validatorConfig.CardSetTypes)
            {
                foreach (var language in _validatorConfig.Languages)
                {
                    string cardSetPath = _validatorConfig.GetCardSetPath(cardSetType, language);
                    bool directoryExists = Directory.Exists(cardSetPath);
                    
                    if (!directoryExists)
                    {
                        errorCount++;
                        _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td>{language}</td><td class='error'>Erreur</td><td>Répertoire non trouvé: {cardSetPath}</td></tr>");
                        continue;
                    }

                    // Vérifier les fichiers d'images dans le répertoire
                    var imageFiles = Directory.GetFiles(cardSetPath, "*.png")
                        .Concat(Directory.GetFiles(cardSetPath, "*.jpg"))
                        .ToList();

                    if (imageFiles.Count == 0)
                    {
                        errorCount++;
                        _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td>{language}</td><td class='error'>Erreur</td><td>Aucun fichier d'image trouvé dans {cardSetPath}</td></tr>");
                    }
                    else
                    {
                        _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td>{language}</td><td class='success'>Succès</td><td>{imageFiles.Count} fichiers d'image trouvés</td></tr>");
                        
                        // Stocker les fichiers pour les validations ultérieures
                        string key = $"{cardSetType}_{language}";
                        if (!_cardFilesByLanguage.ContainsKey(key))
                        {
                            _cardFilesByLanguage[key] = new List<string>();
                        }
                        _cardFilesByLanguage[key].AddRange(imageFiles);
                    }
                }
            }

            _reportBuilder.AppendLine("</table>");
            _totalErrors += errorCount;

            if (errorCount > 0)
            {
                Logger.LogProblem($"Validation de l'existence des fichiers de cartes : {errorCount} erreurs détectées");
            }
            else
            {
                Logger.LogSuccess("Validation de l'existence des fichiers de cartes : aucune erreur détectée");
            }
        }

        /// <summary>
        /// Vérifie la qualité des images générées (dimensions, résolution, absence d'erreurs).
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ValidateCardImagesQuality()
        {
            Logger.LogTitle("Validation de la qualité des images de cartes");
            _reportBuilder.AppendLine("<h2>Validation de la qualité des images de cartes</h2>");
            _reportBuilder.AppendLine("<table border='1'>");
            _reportBuilder.AppendLine("<tr><th>Type de jeu</th><th>Langue</th><th>Fichier</th><th>Dimensions</th><th>DPI</th><th>Format</th><th>Statut</th></tr>");

            int errorCount = 0;
            int warningCount = 0;

            // Si la validation d'existence n'a pas été exécutée, récupérer les fichiers maintenant
            if (_cardFilesByLanguage.Count == 0)
            {
                foreach (var cardSetType in _validatorConfig.CardSetTypes)
                {
                    foreach (var language in _validatorConfig.Languages)
                    {
                        string cardSetPath = _validatorConfig.GetCardSetPath(cardSetType, language);
                        if (Directory.Exists(cardSetPath))
                        {
                            var imageFiles = Directory.GetFiles(cardSetPath, "*.png")
                                .Concat(Directory.GetFiles(cardSetPath, "*.jpg"))
                                .ToList();

                            string key = $"{cardSetType}_{language}";
                            if (!_cardFilesByLanguage.ContainsKey(key))
                            {
                                _cardFilesByLanguage[key] = new List<string>();
                            }
                            _cardFilesByLanguage[key].AddRange(imageFiles);
                        }
                    }
                }
            }

            // Vérifier la qualité de chaque image
            foreach (var entry in _cardFilesByLanguage)
            {
                string[] parts = entry.Key.Split('_');
                string cardSetType = parts[0];
                string language = parts[1];
                
                var expectedDimensions = _validatorConfig.ExpectedDimensions.ContainsKey(cardSetType) 
                    ? _validatorConfig.ExpectedDimensions[cardSetType] 
                    : (825, 1125); // Dimensions par défaut si non spécifiées

                foreach (var imagePath in entry.Value)
                {
                    try
                    {
                        using (var image = new MagickImage(imagePath))
                        {
                            string fileName = Path.GetFileName(imagePath);
                            bool hasError = false;
                            string statusClass = "success";
                            StringBuilder statusDetails = new StringBuilder();

                            // Vérifier les dimensions
                            if (image.Width != expectedDimensions.Width || image.Height != expectedDimensions.Height)
                            {
                                hasError = true;
                                statusClass = "error";
                                statusDetails.Append($"Dimensions incorrectes (attendu: {expectedDimensions.Width}x{expectedDimensions.Height}) ");
                            }

                            // Vérifier la résolution DPI
                            if (image.Density.X < _validatorConfig.ExpectedDpi || image.Density.Y < _validatorConfig.ExpectedDpi)
                            {
                                if (!hasError)
                                {
                                    statusClass = "warning";
                                    warningCount++;
                                }
                                statusDetails.Append($"Résolution DPI insuffisante (attendu: {_validatorConfig.ExpectedDpi}, trouvé: {image.Density.X}x{image.Density.Y}) ");
                            }

                            // Vérifier le format de l'image
                            string format = image.Format.ToString();
                            
                            _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td>{language}</td><td>{fileName}</td>" +
                                $"<td>{image.Width}x{image.Height}</td><td>{image.Density.X}x{image.Density.Y}</td><td>{format}</td>" +
                                $"<td class='{statusClass}'>{(hasError ? "Erreur" : (statusDetails.Length > 0 ? "Avertissement" : "Succès"))}: {statusDetails}</td></tr>");

                            if (hasError)
                            {
                                errorCount++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        string fileName = Path.GetFileName(imagePath);
                        _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td>{language}</td><td>{fileName}</td>" +
                            $"<td colspan='3'>Impossible d'analyser l'image</td><td class='error'>Erreur: {ex.Message}</td></tr>");
                    }
                }
            }

            _reportBuilder.AppendLine("</table>");
            _totalErrors += errorCount;
            _totalWarnings += warningCount;

            if (errorCount > 0)
            {
                Logger.LogProblem($"Validation de la qualité des images de cartes : {errorCount} erreurs et {warningCount} avertissements détectés");
            }
            else if (warningCount > 0)
            {
                Logger.LogWarning($"Validation de la qualité des images de cartes : {warningCount} avertissements détectés");
            }
            else
            {
                Logger.LogSuccess("Validation de la qualité des images de cartes : aucune erreur détectée");
            }
        }

        /// <summary>
        /// Vérifie la cohérence entre les versions linguistiques des cartes.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ValidateMultilingualConsistency()
        {
            Logger.LogTitle("Validation de la cohérence multilingue des cartes");
            _reportBuilder.AppendLine("<h2>Validation de la cohérence multilingue des cartes</h2>");
            _reportBuilder.AppendLine("<table border='1'>");
            _reportBuilder.AppendLine("<tr><th>Type de jeu</th><th>Carte</th><th>Langues comparées</th><th>Différence</th><th>Statut</th></tr>");

            int errorCount = 0;
            int warningCount = 0;

            // Pour chaque type de jeu de cartes
            foreach (var cardSetType in _validatorConfig.CardSetTypes)
            {
                // Récupérer les fichiers de la langue de référence (français)
                string refLanguage = "fr";
                string refKey = $"{cardSetType}_{refLanguage}";
                
                if (!_cardFilesByLanguage.ContainsKey(refKey) || _cardFilesByLanguage[refKey].Count == 0)
                {
                    _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td colspan='4' class='error'>Aucun fichier de référence trouvé pour la langue {refLanguage}</td></tr>");
                    errorCount++;
                    continue;
                }

                var refFiles = _cardFilesByLanguage[refKey];
                
                // Pour chaque langue cible
                foreach (var targetLanguage in _validatorConfig.Languages.Where(l => l != refLanguage))
                {
                    string targetKey = $"{cardSetType}_{targetLanguage}";
                    
                    if (!_cardFilesByLanguage.ContainsKey(targetKey) || _cardFilesByLanguage[targetKey].Count == 0)
                    {
                        _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td colspan='4' class='error'>Aucun fichier trouvé pour la langue {targetLanguage}</td></tr>");
                        errorCount++;
                        continue;
                    }

                    var targetFiles = _cardFilesByLanguage[targetKey];
                    
                    // Vérifier que le nombre de cartes est identique
                    if (refFiles.Count != targetFiles.Count)
                    {
                        _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td>Toutes</td><td>{refLanguage} vs {targetLanguage}</td>" +
                            $"<td>Nombre de cartes différent ({refFiles.Count} vs {targetFiles.Count})</td><td class='error'>Erreur</td></tr>");
                        errorCount++;
                    }

                    // Extraire les identifiants de cartes pour faire correspondre les fichiers entre les langues
                    var refCardIds = ExtractCardIds(refFiles);
                    var targetCardIds = ExtractCardIds(targetFiles);
                    
                    // Vérifier les cartes manquantes
                    var missingCards = refCardIds.Keys.Except(targetCardIds.Keys).ToList();
                    if (missingCards.Any())
                    {
                        foreach (var cardId in missingCards)
                        {
                            _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td>{cardId}</td><td>{refLanguage} vs {targetLanguage}</td>" +
                                $"<td>Carte manquante dans la langue {targetLanguage}</td><td class='error'>Erreur</td></tr>");
                            errorCount++;
                        }
                    }

                    // Vérifier les cartes supplémentaires
                    var extraCards = targetCardIds.Keys.Except(refCardIds.Keys).ToList();
                    if (extraCards.Any())
                    {
                        foreach (var cardId in extraCards)
                        {
                            _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td>{cardId}</td><td>{refLanguage} vs {targetLanguage}</td>" +
                                $"<td>Carte supplémentaire dans la langue {targetLanguage}</td><td class='warning'>Avertissement</td></tr>");
                            warningCount++;
                        }
                    }

                    // Comparer les images pour les cartes communes
                    var commonCards = refCardIds.Keys.Intersect(targetCardIds.Keys).ToList();
                    foreach (var cardId in commonCards)
                    {
                        string refFilePath = refCardIds[cardId];
                        string targetFilePath = targetCardIds[cardId];
                        
                        try
                        {
                            double difference = CompareImages(refFilePath, targetFilePath);
                            string status = "success";
                            string statusText = "Succès";
                            
                            if (difference > _validatorConfig.ImageDifferenceThreshold)
                            {
                                if (difference > _validatorConfig.ImageDifferenceThreshold * 2)
                                {
                                    status = "error";
                                    statusText = "Erreur";
                                    errorCount++;
                                }
                                else
                                {
                                    status = "warning";
                                    statusText = "Avertissement";
                                    warningCount++;
                                }
                            }
                            
                            _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td>{cardId}</td><td>{refLanguage} vs {targetLanguage}</td>" +
                                $"<td>Différence d'image: {difference:P2}</td><td class='{status}'>{statusText}</td></tr>");
                        }
                        catch (Exception ex)
                        {
                            _reportBuilder.AppendLine($"<tr><td>{cardSetType}</td><td>{cardId}</td><td>{refLanguage} vs {targetLanguage}</td>" +
                                $"<td>Erreur lors de la comparaison: {ex.Message}</td><td class='error'>Erreur</td></tr>");
                            errorCount++;
                        }
                    }
                }
            }

            _reportBuilder.AppendLine("</table>");
            _totalErrors += errorCount;
            _totalWarnings += warningCount;

            if (errorCount > 0)
            {
                Logger.LogProblem($"Validation de la cohérence multilingue des cartes : {errorCount} erreurs et {warningCount} avertissements détectés");
            }
            else if (warningCount > 0)
            {
                Logger.LogWarning($"Validation de la cohérence multilingue des cartes : {warningCount} avertissements détectés");
            }
            else
            {
                Logger.LogSuccess("Validation de la cohérence multilingue des cartes : aucune erreur détectée");
            }
        }

        /// <summary>
        /// Exécute tous les tests de validation et génère un rapport global.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task RunAllCardValidations()
        {
            Logger.LogTitle("Exécution de tous les tests de validation des cartes");
            
            // Initialiser le rapport HTML
            InitializeHtmlReport();
            
            // Exécuter les validations
            await ValidateCardFilesExistence();
            await ValidateCardImagesQuality();
            await ValidateMultilingualConsistency();
            
            // Finaliser et enregistrer le rapport
            FinalizeHtmlReport();
            
            // Afficher le résumé
            if (_totalErrors > 0)
            {
                Logger.LogProblem($"Validation des cartes terminée avec {_totalErrors} erreurs et {_totalWarnings} avertissements");
            }
            else if (_totalWarnings > 0)
            {
                Logger.LogWarning($"Validation des cartes terminée avec {_totalWarnings} avertissements");
            }
            else
            {
                Logger.LogSuccess("Validation des cartes terminée avec succès");
            }
            
            Logger.LogTitle("Rapport de validation enregistré dans : " + _validatorConfig.ValidationReportPath);
        }

        /// <summary>
        /// Initialise le rapport HTML.
        /// </summary>
        private void InitializeHtmlReport()
        {
            _reportBuilder = new StringBuilder();
            _reportBuilder.AppendLine("<!DOCTYPE html>");
            _reportBuilder.AppendLine("<html>");
            _reportBuilder.AppendLine("<head>");
            _reportBuilder.AppendLine("<meta charset='utf-8'>");
            _reportBuilder.AppendLine("<title>Rapport de validation des cartes Argumentum</title>");
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
            _reportBuilder.AppendLine("</style>");
            _reportBuilder.AppendLine("</head>");
            _reportBuilder.AppendLine("<body>");
            _reportBuilder.AppendLine("<h1>Rapport de validation des cartes Argumentum</h1>");
            _reportBuilder.AppendLine($"<p>Date de génération : {DateTime.Now}</p>");
        }

        /// <summary>
        /// Finalise et enregistre le rapport HTML.
        /// </summary>
        private void FinalizeHtmlReport()
        {
            // Ajouter le résumé
            _reportBuilder.AppendLine("<div class='summary'>");
            _reportBuilder.AppendLine("<h2>Résumé</h2>");
            
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
            
            // Fermer le document HTML
            _reportBuilder.AppendLine("</body>");
            _reportBuilder.AppendLine("</html>");
            
            // Créer le répertoire de sortie si nécessaire
            string reportDirectory = Path.GetDirectoryName(_validatorConfig.ValidationReportPath);
            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory);
            }
            
            // Enregistrer le rapport
            File.WriteAllText(_validatorConfig.ValidationReportPath, _reportBuilder.ToString());
        }

        /// <summary>
        /// Extrait les identifiants de cartes à partir des noms de fichiers.
        /// </summary>
        /// <param name="filePaths">Liste des chemins de fichiers.</param>
        /// <returns>Dictionnaire associant les identifiants de cartes aux chemins de fichiers.</returns>
        private Dictionary<string, string> ExtractCardIds(List<string> filePaths)
        {
            var result = new Dictionary<string, string>();
            
            foreach (var filePath in filePaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                
                // Extraire l'identifiant de la carte à partir du nom de fichier
                // Supposons que le format est "CardType_CardId_Language.ext" ou similaire
                string cardId = fileName;
                
                // Essayer d'extraire un identifiant numérique si présent
                var match = Regex.Match(fileName, @"(\d+)");
                if (match.Success)
                {
                    cardId = match.Value;
                }
                
                result[cardId] = filePath;
            }
            
            return result;
        }

        /// <summary>
        /// Compare deux images et retourne un score de différence.
        /// </summary>
        /// <param name="imagePath1">Chemin de la première image.</param>
        /// <param name="imagePath2">Chemin de la deuxième image.</param>
        /// <returns>Score de différence entre 0.0 (identiques) et 1.0 (complètement différentes).</returns>
        private double CompareImages(string imagePath1, string imagePath2)
        {
            using (var image1 = new MagickImage(imagePath1))
            using (var image2 = new MagickImage(imagePath2))
            {
                // Redimensionner les images à la même taille si nécessaire
                if (image1.Width != image2.Width || image1.Height != image2.Height)
                {
                    image2.Resize(image1.Width, image1.Height);
                }
                
                // Comparer les images
                using (var diff = new MagickImage())
                {
                    // Calculer la différence
                    var error = image1.Compare(image2, ErrorMetric.RootMeanSquared, diff);
                    return error;
                }
            }
        }
    }
}