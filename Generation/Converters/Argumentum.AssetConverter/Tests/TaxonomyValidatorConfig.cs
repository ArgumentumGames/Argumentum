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
        /// <returns>True si toutes les validations exécutées ont réussi, sinon false.</returns>
        public async Task<bool> Apply(AssetConverterConfig config)
        {
            Logger.LogTitle("Validation de la taxonomie des arguments fallacieux");

            var validator = new TaxonomyValidationTests(config);

            bool success;
            if (ValidateStructure && ValidateTranslations && ValidateTerminology)
            {
                // Si toutes les validations sont activées, exécuter la méthode qui les regroupe
                success = await validator.RunAllValidations();
            }
            else
            {
                // Sinon, exécuter les validations individuellement selon la configuration
                success = true;
                if (ValidateStructure)
                {
                    success &= await validator.ValidateTaxonomyStructure();
                }

                if (ValidateTranslations)
                {
                    success &= await validator.ValidateTranslationCompleteness();
                }

                if (ValidateTerminology)
                {
                    success &= await validator.ValidateTerminologyConsistency();
                }
            }

            // #1046 Lot B (Surface1 #8): the aggregate bool was discarded and the success line
            // logged unconditionally — a failing taxonomy run still printed "terminée" as a success.
            if (success)
            {
                Logger.LogSuccess("Validation de la taxonomie terminée");
            }
            else
            {
                Logger.LogProblem("Validation de la taxonomie terminée AVEC ÉCHEC");
            }
            return success;
        }
    }
}