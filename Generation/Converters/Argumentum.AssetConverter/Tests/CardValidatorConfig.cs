using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Configuration pour la validation des cartes générées
    /// </summary>
    public class CardValidatorConfig
    {
        /// <summary>
        /// Répertoire de base pour les cartes générées
        /// </summary>
        public string BaseCardsDirectory { get; set; } = "Cards";

        /// <summary>
        /// Liste des langues à valider
        /// </summary>
        public List<string> Languages { get; set; } = new List<string> { "fr", "en", "ru", "pt" };

        /// <summary>
        /// Types de jeux de cartes à valider
        /// </summary>
        public List<string> CardSetTypes { get; set; } = new List<string> { "Fallacies", "Rules", "Scenarii", "Memo" };

        /// <summary>
        /// Dimensions attendues pour les images de cartes (largeur, hauteur)
        /// </summary>
        public Dictionary<string, (int Width, int Height)> ExpectedDimensions { get; set; } = new Dictionary<string, (int Width, int Height)>
        {
            { "Fallacies", (825, 1125) },
            { "Rules", (825, 1125) },
            { "Scenarii", (825, 1125) },
            { "Memo", (825, 1125) }
        };

        /// <summary>
        /// Résolution DPI attendue pour les images
        /// </summary>
        public int ExpectedDpi { get; set; } = 300;

        /// <summary>
        /// Seuil de tolérance pour les différences d'images entre les langues
        /// Plus la valeur est basse, plus la comparaison est stricte (0.0 à 1.0)
        /// </summary>
        public double ImageDifferenceThreshold { get; set; } = 0.05;

        /// <summary>
        /// Chemin pour le rapport de validation HTML
        /// </summary>
        public string ValidationReportPath { get; set; } = Path.Combine("Output", "Validation", "card_validation_report.html");

        /// <summary>
        /// Indique si la validation de l'existence des fichiers doit être exécutée
        /// </summary>
        public bool ValidateFileExistence { get; set; } = true;

        /// <summary>
        /// Indique si la validation de la qualité des images doit être exécutée
        /// </summary>
        public bool ValidateImageQuality { get; set; } = true;

        /// <summary>
        /// Indique si la validation de la cohérence multilingue doit être exécutée
        /// </summary>
        public bool ValidateMultilingualConsistency { get; set; } = true;

        /// <summary>
        /// Niveau de détail pour la journalisation
        /// </summary>
        public int VerbosityLevel { get; set; } = 1;

        /// <summary>
        /// Indique si les avertissements doivent être traités comme des erreurs
        /// </summary>
        public bool TreatWarningsAsErrors { get; set; } = false;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CardValidatorConfig"/>
        /// </summary>
        public CardValidatorConfig()
        {
            // Valeurs par défaut déjà définies dans les propriétés
        }

        /// <summary>
        /// Obtient le chemin complet d'un jeu de cartes spécifique
        /// </summary>
        /// <param name="cardSetType">Type de jeu de cartes</param>
        /// <param name="language">Langue du jeu de cartes</param>
        /// <returns>Chemin complet du répertoire du jeu de cartes</returns>
        public string GetCardSetPath(string cardSetType, string language)
        {
            return Path.Combine(BaseCardsDirectory, cardSetType, language);
        }

        /// <summary>
        /// Exécute les validations configurées
        /// </summary>
        /// <param name="config">La configuration de l'application</param>
        /// <returns>Une tâche représentant l'opération asynchrone</returns>
        public async Task Apply(AssetConverterConfig config)
        {
            Logger.LogTitle("Validation des cartes générées");

            var validator = new CardGenerationValidationTests(config);

            if (ValidateFileExistence && ValidateImageQuality && ValidateMultilingualConsistency)
            {
                // Si toutes les validations sont activées, exécuter la méthode qui les regroupe
                await validator.RunAllCardValidations();
            }
            else
            {
                // Sinon, exécuter les validations individuellement selon la configuration
                if (ValidateFileExistence)
                {
                    await validator.ValidateCardFilesExistence();
                }

                if (ValidateImageQuality)
                {
                    await validator.ValidateCardImagesQuality();
                }

                if (ValidateMultilingualConsistency)
                {
                    await validator.ValidateMultilingualConsistency();
                }
            }

            Logger.LogSuccess("Validation des cartes générées terminée");
        }
    }
}