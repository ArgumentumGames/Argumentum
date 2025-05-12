using System;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Configuration pour la validation de la taxonomie des arguments fallacieux.
    /// </summary>
    public class TaxonomyValidatorConfig
    {
        /// <summary>
        /// Indique si la validation de la structure de la taxonomie doit être exécutée.
        /// </summary>
        public bool ValidateStructure { get; set; } = true;

        /// <summary>
        /// Indique si la validation de la complétude des traductions doit être exécutée.
        /// </summary>
        public bool ValidateTranslations { get; set; } = true;

        /// <summary>
        /// Indique si la validation de la cohérence terminologique doit être exécutée.
        /// </summary>
        public bool ValidateTerminology { get; set; } = true;

        /// <summary>
        /// Exécute les validations configurées.
        /// </summary>
        /// <param name="config">La configuration de l'application.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task Apply(AssetConverterConfig config)
        {
            Logger.LogTitle("Validation de la taxonomie des arguments fallacieux");

            var validator = new TaxonomyValidationTests(config);

            if (ValidateStructure && ValidateTranslations && ValidateTerminology)
            {
                // Si toutes les validations sont activées, exécuter la méthode qui les regroupe
                await validator.RunAllValidations();
            }
            else
            {
                // Sinon, exécuter les validations individuellement selon la configuration
                if (ValidateStructure)
                {
                    await validator.ValidateTaxonomyStructure();
                }

                if (ValidateTranslations)
                {
                    await validator.ValidateTranslationCompleteness();
                }

                if (ValidateTerminology)
                {
                    await validator.ValidateTerminologyConsistency();
                }
            }

            Logger.LogSuccess("Validation de la taxonomie terminée");
        }
    }
}