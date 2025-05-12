using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Configuration pour la validation de l'ontologie OWL
    /// </summary>
    public class OwlValidatorConfig
    {
        /// <summary>
        /// Chemin vers le fichier d'ontologie OWL à valider
        /// </summary>
        public string OwlFilePath { get; set; }

        /// <summary>
        /// Liste des langues à valider pour les annotations multilingues
        /// </summary>
        public List<string> LanguagesToValidate { get; set; } = new List<string> { "fr", "en", "ru", "pt" };

        /// <summary>
        /// Chemin vers le fichier d'ontologie AIF pour la validation des mappings
        /// </summary>
        public string AifOwlFilePath { get; set; }

        /// <summary>
        /// Chemin de sortie pour le rapport de validation
        /// </summary>
        public string ValidationReportPath { get; set; }

        /// <summary>
        /// Indique si la validation de la structure de l'ontologie doit être exécutée
        /// </summary>
        public bool ValidateStructure { get; set; } = true;

        /// <summary>
        /// Indique si la validation des annotations multilingues doit être exécutée
        /// </summary>
        public bool ValidateMultilingualAnnotations { get; set; } = true;

        /// <summary>
        /// Indique si la validation des mappings AIF doit être exécutée
        /// </summary>
        public bool ValidateAIFMappings { get; set; } = true;

        /// <summary>
        /// Niveau de détail pour la journalisation
        /// </summary>
        public int VerbosityLevel { get; set; } = 1;

        /// <summary>
        /// Indique si les avertissements doivent être traités comme des erreurs
        /// </summary>
        public bool TreatWarningsAsErrors { get; set; } = false;

        /// <summary>
        /// Concepts principaux qui doivent être présents dans l'ontologie
        /// </summary>
        public List<string> RequiredConcepts { get; set; } = new List<string>
        {
            "Fallacy",
            "FormalFallacy",
            "InformalFallacy",
            "LogicalFallacy",
            "LanguageFallacy",
            "RelevanceFallacy"
        };

        /// <summary>
        /// Relations qui doivent être présentes dans l'ontologie
        /// </summary>
        public List<string> RequiredRelations { get; set; } = new List<string>
        {
            "hasFallacyType",
            "hasExample",
            "hasDefinition",
            "hasParentFallacy"
        };

        /// <summary>
        /// Types de mappings AIF à valider
        /// </summary>
        public List<string> MappingTypes { get; set; } = new List<string>
        {
            "exactMatch",
            "closeMatch",
            "relatedMatch"
        };

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="OwlValidatorConfig"/>
        /// </summary>
        public OwlValidatorConfig()
        {
            // Valeurs par défaut
            OwlFilePath = Path.Combine("Output", "Ontology", "argumentum.owl");
            AifOwlFilePath = Path.Combine("Ontology", "Resources", "AIF.owl");
            ValidationReportPath = Path.Combine("Output", "Validation", "owl_validation_report.txt");
        }

        /// <summary>
        /// Exécute les validations configurées
        /// </summary>
        /// <param name="config">La configuration de l'application</param>
        /// <returns>Une tâche représentant l'opération asynchrone</returns>
        public async Task Apply(AssetConverterConfig config)
        {
            Logger.LogTitle("Validation de l'ontologie OWL");

            var validator = new OwlOntologyValidationTests(config);

            if (ValidateStructure && ValidateMultilingualAnnotations && ValidateAIFMappings)
            {
                // Si toutes les validations sont activées, exécuter la méthode qui les regroupe
                await validator.RunAllOwlValidations();
            }
            else
            {
                // Sinon, exécuter les validations individuellement selon la configuration
                if (ValidateStructure)
                {
                    await validator.ValidateOwlOntologyStructure();
                }

                if (ValidateMultilingualAnnotations)
                {
                    await validator.ValidateMultilingualAnnotations();
                }

                if (ValidateAIFMappings)
                {
                    await validator.ValidateAIFMappings();
                }
            }

            Logger.LogSuccess("Validation de l'ontologie OWL terminée");
        }
    }
}