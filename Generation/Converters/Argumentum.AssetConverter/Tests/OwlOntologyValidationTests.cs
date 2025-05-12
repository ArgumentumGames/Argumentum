using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Ontology;
using OWLSharp;
using OWLSharp.Extensions.SKOS;
using RDFSharp.Model;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Tests de validation pour l'ontologie OWL générée à partir de la taxonomie des arguments fallacieux.
    /// Ces tests permettent de vérifier la qualité et la cohérence de l'ontologie.
    /// </summary>
    public class OwlOntologyValidationTests
    {
        private readonly AssetConverterConfig _config;
        private OWLOntology _ontology;
        private readonly OwlValidatorConfig _validatorConfig;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="OwlOntologyValidationTests"/>.
        /// </summary>
        /// <param name="config">La configuration de l'application.</param>
        public OwlOntologyValidationTests(AssetConverterConfig config)
        {
            _config = config;
            _validatorConfig = config.OwlValidatorConfig;
        }

        /// <summary>
        /// Charge l'ontologie OWL à partir du fichier spécifié.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task LoadOntology()
        {
            string owlFilePath = _validatorConfig.OwlFilePath;
            
            if (!File.Exists(owlFilePath))
            {
                Logger.LogProblem($"Le fichier d'ontologie OWL n'existe pas : {owlFilePath}");
                return;
            }

            try
            {
                _ontology = OWLOntology.FromFile(OWLEnums.OWLFormats.OwlXml, owlFilePath);
                Logger.LogSuccess($"Ontologie OWL chargée : {owlFilePath}");
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors du chargement de l'ontologie OWL : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Vérifie la structure de base de l'ontologie.
        /// - Présence des concepts principaux
        /// - Hiérarchie correcte des concepts
        /// - Intégrité des relations entre concepts
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ValidateOwlOntologyStructure()
        {
            Logger.LogTitle("Validation de la structure de l'ontologie OWL");

            if (_ontology == null)
            {
                await LoadOntology();
            }

            if (_ontology == null)
            {
                Logger.LogProblem("Impossible de valider la structure de l'ontologie : aucune ontologie chargée.");
                return;
            }

            int errorCount = 0;
            StringBuilder report = new StringBuilder();

            // Vérification de la présence des concepts principaux
            report.AppendLine("Vérification de la présence des concepts principaux :");
            foreach (var requiredConcept in _validatorConfig.RequiredConcepts)
            {
                bool conceptExists = false;
                
                // Recherche du concept dans l'ontologie
                foreach (var concept in _ontology.Model.ClassModel.Classes)
                {
                    if (concept.ToString().EndsWith(requiredConcept))
                    {
                        conceptExists = true;
                        break;
                    }
                }

                if (!conceptExists)
                {
                    errorCount++;
                    report.AppendLine($"  - Concept manquant : {requiredConcept}");
                }
                else
                {
                    report.AppendLine($"  - Concept présent : {requiredConcept}");
                }
            }
            report.AppendLine();

            // Vérification de la présence des relations requises
            report.AppendLine("Vérification de la présence des relations requises :");
            foreach (var requiredRelation in _validatorConfig.RequiredRelations)
            {
                bool relationExists = false;
                
                // Recherche de la relation dans l'ontologie
                foreach (var property in _ontology.Model.PropertyModel.Properties)
                {
                    if (property.ToString().EndsWith(requiredRelation))
                    {
                        relationExists = true;
                        break;
                    }
                }

                if (!relationExists)
                {
                    errorCount++;
                    report.AppendLine($"  - Relation manquante : {requiredRelation}");
                }
                else
                {
                    report.AppendLine($"  - Relation présente : {requiredRelation}");
                }
            }
            report.AppendLine();

            // Vérification de la hiérarchie des concepts
            report.AppendLine("Vérification de la hiérarchie des concepts :");
            
            // Vérifier que chaque concept a un parent (sauf le concept racine)
            var rootConcepts = _ontology.GetTopConcepts();
            var allConcepts = _ontology.GetConcepts();
            
            if (rootConcepts.Count == 0)
            {
                errorCount++;
                report.AppendLine("  - Aucun concept racine trouvé dans l'ontologie");
            }
            else
            {
                report.AppendLine($"  - {rootConcepts.Count} concept(s) racine(s) trouvé(s)");
            }

            // Vérifier que chaque concept non-racine a un parent
            var conceptsWithoutParent = new List<RDFResource>();
            foreach (var concept in allConcepts)
            {
                if (!rootConcepts.Contains(concept))
                {
                    bool hasParent = false;
                    foreach (var potentialParent in allConcepts)
                    {
                        if (concept != potentialParent && _ontology.CheckIsNarrowerConcept(concept, potentialParent))
                        {
                            hasParent = true;
                            break;
                        }
                    }

                    if (!hasParent)
                    {
                        conceptsWithoutParent.Add(concept);
                    }
                }
            }

            if (conceptsWithoutParent.Count > 0)
            {
                errorCount += conceptsWithoutParent.Count;
                report.AppendLine("  - Concepts sans parent détectés :");
                foreach (var concept in conceptsWithoutParent)
                {
                    report.AppendLine($"    * {concept}");
                }
            }
            report.AppendLine();

            // Affichage du rapport
            if (errorCount > 0)
            {
                Logger.LogProblem($"Validation de la structure de l'ontologie OWL : {errorCount} erreurs détectées");
                Logger.Log(report.ToString());
            }
            else
            {
                Logger.LogSuccess("Validation de la structure de l'ontologie OWL : aucune erreur détectée");
                if (_validatorConfig.VerbosityLevel > 0)
                {
                    Logger.Log(report.ToString());
                }
            }
        }

        /// <summary>
        /// Vérifie les annotations multilingues.
        /// - Présence des labels dans toutes les langues (fr, en, ru, pt)
        /// - Présence des définitions dans toutes les langues
        /// - Présence des exemples dans toutes les langues
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ValidateMultilingualAnnotations()
        {
            Logger.LogTitle("Validation des annotations multilingues de l'ontologie OWL");

            if (_ontology == null)
            {
                await LoadOntology();
            }

            if (_ontology == null)
            {
                Logger.LogProblem("Impossible de valider les annotations multilingues : aucune ontologie chargée.");
                return;
            }

            int errorCount = 0;
            StringBuilder report = new StringBuilder();

            // Récupérer tous les concepts de l'ontologie
            var allConcepts = _ontology.GetConcepts();
            
            // Pour chaque concept, vérifier les annotations multilingues
            foreach (var concept in allConcepts)
            {
                report.AppendLine($"Concept : {concept}");
                
                // Vérification des labels préférés (prefLabel)
                var missingPrefLabels = new List<string>();
                foreach (var language in _validatorConfig.LanguagesToValidate)
                {
                    var prefLabels = _ontology.GetConceptPreferredLabels(concept)
                        .Where(l => l.Language.Equals(language, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    
                    if (prefLabels.Count == 0)
                    {
                        missingPrefLabels.Add(language);
                    }
                }

                if (missingPrefLabels.Count > 0)
                {
                    errorCount += missingPrefLabels.Count;
                    report.AppendLine($"  - Labels préférés manquants : {string.Join(", ", missingPrefLabels)}");
                }
                
                // Vérification des définitions
                var missingDefinitions = new List<string>();
                foreach (var language in _validatorConfig.LanguagesToValidate)
                {
                    var definitions = _ontology.GetConceptDocumentation(concept, SKOSEnums.SKOSDocumentationTypes.Definition)
                        .Where(d => d.Language.Equals(language, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    
                    if (definitions.Count == 0)
                    {
                        missingDefinitions.Add(language);
                    }
                }

                if (missingDefinitions.Count > 0)
                {
                    errorCount += missingDefinitions.Count;
                    report.AppendLine($"  - Définitions manquantes : {string.Join(", ", missingDefinitions)}");
                }
                
                // Vérification des exemples
                var missingExamples = new List<string>();
                foreach (var language in _validatorConfig.LanguagesToValidate)
                {
                    var examples = _ontology.GetConceptDocumentation(concept, SKOSEnums.SKOSDocumentationTypes.Example)
                        .Where(e => e.Language.Equals(language, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    
                    if (examples.Count == 0)
                    {
                        missingExamples.Add(language);
                    }
                }

                if (missingExamples.Count > 0)
                {
                    errorCount += missingExamples.Count;
                    report.AppendLine($"  - Exemples manquants : {string.Join(", ", missingExamples)}");
                }

                report.AppendLine();
            }

            // Affichage du rapport
            if (errorCount > 0)
            {
                Logger.LogProblem($"Validation des annotations multilingues : {errorCount} erreurs détectées");
                Logger.Log(report.ToString());
            }
            else
            {
                Logger.LogSuccess("Validation des annotations multilingues : aucune erreur détectée");
                if (_validatorConfig.VerbosityLevel > 0)
                {
                    Logger.Log(report.ToString());
                }
            }
        }

        /// <summary>
        /// Vérifie les mappings avec l'ontologie AIF.
        /// - Présence des mappings (exactMatch, closeMatch, etc.)
        /// - Cohérence des mappings
        /// - Couverture des mappings
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task ValidateAIFMappings()
        {
            Logger.LogTitle("Validation des mappings AIF de l'ontologie OWL");

            if (_ontology == null)
            {
                await LoadOntology();
            }

            if (_ontology == null)
            {
                Logger.LogProblem("Impossible de valider les mappings AIF : aucune ontologie chargée.");
                return;
            }

            // Charger l'ontologie AIF si elle existe
            OWLOntology aifOntology = null;
            if (File.Exists(_validatorConfig.AifOwlFilePath))
            {
                try
                {
                    aifOntology = OWLOntology.FromFile(OWLEnums.OWLFormats.OwlXml, _validatorConfig.AifOwlFilePath);
                    Logger.LogSuccess($"Ontologie AIF chargée : {_validatorConfig.AifOwlFilePath}");
                }
                catch (Exception ex)
                {
                    Logger.LogProblem($"Erreur lors du chargement de l'ontologie AIF : {ex.Message}");
                }
            }
            else
            {
                Logger.LogProblem($"Le fichier d'ontologie AIF n'existe pas : {_validatorConfig.AifOwlFilePath}");
                Logger.Log("La validation des mappings AIF sera limitée à la vérification de leur présence.");
            }

            int errorCount = 0;
            StringBuilder report = new StringBuilder();

            // Récupérer tous les concepts de l'ontologie
            var allConcepts = _ontology.GetConcepts();
            
            // Compteurs pour les statistiques
            int conceptsWithMappings = 0;
            Dictionary<string, int> mappingTypeCount = new Dictionary<string, int>();
            foreach (var mappingType in _validatorConfig.MappingTypes)
            {
                mappingTypeCount[mappingType] = 0;
            }

            // Pour chaque concept, vérifier les mappings AIF
            foreach (var concept in allConcepts)
            {
                bool hasMappings = false;
                report.AppendLine($"Concept : {concept}");
                
                // Vérifier les différents types de mappings
                foreach (var mappingType in _validatorConfig.MappingTypes)
                {
                    List<RDFResource> mappedResources = new List<RDFResource>();
                    
                    switch (mappingType)
                    {
                        case "exactMatch":
                            mappedResources = _ontology.GetExactMatchConcepts(concept);
                            break;
                        case "closeMatch":
                            mappedResources = _ontology.GetCloseMatchConcepts(concept);
                            break;
                        case "relatedMatch":
                            mappedResources = _ontology.GetRelatedMatchConcepts(concept);
                            break;
                    }

                    if (mappedResources.Count > 0)
                    {
                        hasMappings = true;
                        mappingTypeCount[mappingType] += mappedResources.Count;
                        
                        report.AppendLine($"  - {mappingType} : {mappedResources.Count} mapping(s)");
                        foreach (var resource in mappedResources)
                        {
                            report.AppendLine($"    * {resource}");
                            
                            // Si l'ontologie AIF est disponible, vérifier que la ressource existe
                            if (aifOntology != null && resource.ToString().Contains(_validatorConfig.AifOwlFilePath))
                            {
                                bool resourceExists = aifOntology.Model.ClassModel.CheckHasClass(resource);
                                if (!resourceExists)
                                {
                                    errorCount++;
                                    report.AppendLine($"      ! La ressource n'existe pas dans l'ontologie AIF");
                                }
                            }
                        }
                    }
                }

                if (hasMappings)
                {
                    conceptsWithMappings++;
                }
                else
                {
                    report.AppendLine("  - Aucun mapping AIF trouvé");
                }

                report.AppendLine();
            }

            // Statistiques globales
            report.AppendLine("Statistiques des mappings AIF :");
            report.AppendLine($"  - {conceptsWithMappings} concept(s) sur {allConcepts.Count} ont des mappings AIF ({(double)conceptsWithMappings / allConcepts.Count * 100:F1}%)");
            foreach (var mappingType in _validatorConfig.MappingTypes)
            {
                report.AppendLine($"  - {mappingType} : {mappingTypeCount[mappingType]} mapping(s)");
            }
            report.AppendLine();

            // Affichage du rapport
            if (errorCount > 0)
            {
                Logger.LogProblem($"Validation des mappings AIF : {errorCount} erreurs détectées");
                Logger.Log(report.ToString());
            }
            else
            {
                Logger.LogSuccess("Validation des mappings AIF : aucune erreur détectée");
                Logger.Log(report.ToString());
            }
        }

        /// <summary>
        /// Exécute tous les tests de validation et génère un rapport global.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task RunAllOwlValidations()
        {
            Logger.LogTitle("Exécution de tous les tests de validation de l'ontologie OWL");

            await LoadOntology();
            
            if (_ontology == null)
            {
                Logger.LogProblem("Impossible d'exécuter les validations : aucune ontologie chargée.");
                return;
            }

            await ValidateOwlOntologyStructure();
            await ValidateMultilingualAnnotations();
            await ValidateAIFMappings();

            // Générer un rapport de validation global
            string reportDirectory = Path.GetDirectoryName(_validatorConfig.ValidationReportPath);
            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory);
            }

            Logger.LogTitle("Fin des tests de validation de l'ontologie OWL");
        }
    }
}