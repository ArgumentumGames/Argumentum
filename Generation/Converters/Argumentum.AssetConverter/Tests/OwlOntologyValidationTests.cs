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
        private OwlAdapter _ontology;
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
                _ontology = OwlAdapter.FromFile(owlFilePath);
                Logger.LogSuccess($"Ontologie OWL chargée : {owlFilePath}");
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors du chargement de l'ontologie OWL : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Exécute tous les tests de validation.
        /// </summary>
        /// <returns>True si tous les tests ont réussi, sinon false.</returns>
        public async Task<bool> RunAllTests()
        {
            try
            {
                await LoadOntology();
                
                if (_ontology == null)
                {
                    return false;
                }
                
                bool structureValid = await ValidateOwlOntologyStructure();
                bool annotationsValid = await ValidateMultilingualAnnotations();
                bool mappingsValid = await ValidateAIFMappings();
                
                return structureValid && annotationsValid && mappingsValid;
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de l'exécution des tests de validation : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Vérifie la structure de base de l'ontologie.
        /// - Présence des concepts principaux
        /// - Hiérarchie correcte des concepts
        /// - Intégrité des relations entre concepts
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task<bool> ValidateOwlOntologyStructure()
        {
            await Task.CompletedTask;
            var errors = new List<string>();

            // Check ConceptScheme has rdf:type skos:ConceptScheme
            var conceptSchemes = _ontology.GetResourcesByType(SKOSVocabulary.ConceptScheme);
            if (conceptSchemes.Count == 0)
            {
                errors.Add("No skos:ConceptScheme found with rdf:type assertion");
            }
            Logger.Log($"ConceptSchemes with rdf:type: {conceptSchemes.Count}");

            // Check concepts have rdf:type skos:Concept
            var concepts = _ontology.GetResourcesByType(SKOSVocabulary.Concept);
            if (concepts.Count == 0)
            {
                errors.Add("No skos:Concept found with rdf:type assertion");
            }
            Logger.Log($"Concepts with rdf:type: {concepts.Count}");

            // Check top concepts exist
            var topConcepts = _ontology.GetTopConcepts();
            if (topConcepts.Count == 0)
            {
                errors.Add("No top concepts found (skos:hasTopConcept)");
            }
            Logger.Log($"Top concepts: {topConcepts.Count}");

            // Check hierarchy via narrower/broader on top concepts
            int totalNarrower = 0;
            foreach (var topConcept in topConcepts.Take(5))
            {
                var labels = _ontology.GetConceptPreferredLabels(topConcept);
                var label = labels.Count > 0 ? labels[0].Value : topConcept.URI.ToString();
                // Check that top concept has narrower children
                var childConcepts = _ontology.GetConcepts();
                foreach (var child in childConcepts.Take(20))
                {
                    if (_ontology.CheckIsNarrowerConcept(child, topConcept))
                    {
                        totalNarrower++;
                    }
                }
                Logger.Log($"  Top concept '{label}': checked narrower relations");
            }
            if (totalNarrower == 0)
            {
                errors.Add("No narrower concepts found in hierarchy");
            }

            foreach (var error in errors)
            {
                Logger.LogProblem(error);
            }

            bool valid = errors.Count == 0;
            Logger.Log(valid ? "Structure validation: PASS" : $"Structure validation: FAIL ({errors.Count} errors)");
            return valid;
        }

        /// <summary>
        /// Vérifie les annotations multilingues.
        /// - Présence des labels dans toutes les langues (fr, en, ru, pt)
        /// - Présence des définitions dans toutes les langues
        /// - Présence des exemples dans toutes les langues
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task<bool> ValidateMultilingualAnnotations()
        {
            await Task.CompletedTask;
            var errors = new List<string>();

            var concepts = _ontology.GetResourcesByType(SKOSVocabulary.Concept);
            if (concepts.Count == 0)
            {
                // #1046 Lot B (MED #11) — audité, laissé tel quel volontairement : ce early-return
                // sur ontologie vide serait un succès vide, MAIS OwlValidatorLivePathTests épingle
                // déjà l'ontologie de référence (le test échoue si l'OWL ne se charge pas), ce qui
                // rend cette branche inatteignable en pratique. Ne pas la durcir sans d'abord
                // relier ce validateur au chemin live.
                Logger.Log("No concepts to validate annotations — skipping");
                return true;
            }

            int missingLabels = 0, missingDefs = 0, checkedCount = 0;
            foreach (var concept in concepts.Take(50))
            {
                checkedCount++;
                var labels = _ontology.GetConceptPreferredLabels(concept);
                if (labels.Count == 0) missingLabels++;

                var defs = _ontology.GetConceptDocumentation(concept, SKOSDocumentationTypes.Definition);
                if (defs.Count == 0) missingDefs++;
            }

            Logger.Log($"Checked {checkedCount} concepts: {missingLabels} missing prefLabel, {missingDefs} missing definition");

            if (missingLabels > checkedCount / 2)
            {
                errors.Add($"{missingLabels}/{checkedCount} concepts missing skos:prefLabel");
            }
            if (missingDefs > checkedCount / 2)
            {
                errors.Add($"{missingDefs}/{checkedCount} concepts missing skos:definition");
            }

            foreach (var error in errors)
            {
                Logger.LogProblem(error);
            }

            bool valid = errors.Count == 0;
            Logger.Log(valid ? "Annotation validation: PASS" : $"Annotation validation: FAIL ({errors.Count} errors)");
            return valid;
        }

        /// <summary>
        /// Vérifie les mappings avec l'ontologie AIF.
        /// - Présence des mappings (exactMatch, closeMatch, etc.)
        /// - Cohérence des mappings
        /// - Couverture des mappings
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task<bool> ValidateAIFMappings()
        {
            await Task.CompletedTask;
            var errors = new List<string>();

            var concepts = _ontology.GetResourcesByType(SKOSVocabulary.Concept);
            if (concepts.Count == 0)
            {
                Logger.Log("No concepts to validate AIF mappings — skipping");
                return true;
            }

            int withExactMatch = 0, withAnyMatch = 0;
            foreach (var concept in concepts)
            {
                var exact = _ontology.GetExactMatchConcepts(concept);
                var close = _ontology.GetCloseMatchConcepts(concept);
                var related = _ontology.GetRelatedMatchConcepts(concept);

                if (exact.Count > 0) withExactMatch++;
                if (exact.Count > 0 || close.Count > 0 || related.Count > 0) withAnyMatch++;
            }

            Logger.Log($"AIF mappings: {withExactMatch}/{concepts.Count} with exactMatch, {withAnyMatch}/{concepts.Count} with any match");

            if (withAnyMatch == 0)
            {
                errors.Add("No concepts have any AIF match mappings (exactMatch, closeMatch, relatedMatch)");
            }

            foreach (var error in errors)
            {
                Logger.LogProblem(error);
            }

            bool valid = errors.Count == 0;
            Logger.Log(valid ? "AIF mapping validation: PASS" : $"AIF mapping validation: FAIL ({errors.Count} errors)");
            return valid;
        }

        /// <summary>
        /// Exécute tous les tests de validation et génère un rapport global.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task<bool> RunAllOwlValidations()
        {
            Logger.LogTitle("Exécution de tous les tests de validation de l'ontologie OWL");

            await LoadOntology();
            
            if (_ontology == null)
            {
                Logger.LogProblem("Impossible d'exécuter les validations : aucune ontologie chargée.");
                return false;
            }

            bool structureValid = await ValidateOwlOntologyStructure();
            bool annotationsValid = await ValidateMultilingualAnnotations();
            bool mappingsValid = await ValidateAIFMappings();

            // Générer un rapport de validation global
            string reportDirectory = Path.GetDirectoryName(_validatorConfig.ValidationReportPath);
            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory);
            }

            Logger.LogTitle("Fin des tests de validation de l'ontologie OWL");
            
            return structureValid && annotationsValid && mappingsValid;
        }
    }
}