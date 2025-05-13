using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OWLSharp;
using OWLSharp.Extensions.SKOS;
using RDFSharp.Model;

namespace Argumentum.AssetConverter.Ontology
{
    /// <summary>
    /// Types de documentation SKOS
    /// </summary>
    public enum SKOSDocumentationTypes
    {
        Definition,
        Example
    }

    /// <summary>
    /// Adaptateur pour la bibliothèque OWLSharp 4.6.1
    /// </summary>
    public class OwlAdapter
    {
        private dynamic _ontology;
        private string _namespace;

        public OwlAdapter(string ontologyNamespace)
        {
            _namespace = ontologyNamespace;
            
            try
            {
                // Créer une instance de OWLOntology par réflexion
                Type owlOntologyType = Type.GetType("OWLSharp.OWLOntology, OWLSharp");
                if (owlOntologyType == null)
                {
                    // Essayer avec un autre namespace possible
                    owlOntologyType = Type.GetType("OWLSharp.Model.OWLOntology, OWLSharp");
                }
                
                if (owlOntologyType != null)
                {
                    // Créer une instance avec le constructeur qui prend un string
                    _ontology = Activator.CreateInstance(owlOntologyType, new object[] { ontologyNamespace });
                    
                    // Appeler la méthode InitializeSKOS par réflexion
                    var initMethod = owlOntologyType.GetMethod("InitializeSKOS");
                    if (initMethod != null)
                    {
                        initMethod.Invoke(_ontology, null);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Type OWLOntology non trouvé");
                }
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors de la création de l'ontologie OWL : {ex.Message}");
                throw;
            }
        }

        public static OwlAdapter FromFile(string filePath)
        {
            try
            {
                // Créer une instance vide
                var adapter = new OwlAdapter("http://temp.namespace.org");
                
                // Trouver le type OWLOntology par réflexion
                Type owlOntologyType = Type.GetType("OWLSharp.OWLOntology, OWLSharp");
                if (owlOntologyType == null)
                {
                    owlOntologyType = Type.GetType("OWLSharp.Model.OWLOntology, OWLSharp");
                }
                
                if (owlOntologyType != null)
                {
                    // Trouver la méthode FromFile
                    var fromFileMethod = owlOntologyType.GetMethod("FromFile",
                        BindingFlags.Public | BindingFlags.Static);
                    
                    if (fromFileMethod != null)
                    {
                        // Trouver l'enum RDFFormats.RdfXml
                        Type rdfFormatsType = Type.GetType("RDFSharp.Model.RDFModelEnums+RDFFormats, RDFSharp");
                        if (rdfFormatsType != null)
                        {
                            object rdfXmlFormat = Enum.Parse(rdfFormatsType, "RdfXml");
                            
                            // Appeler la méthode FromFile
                            adapter._ontology = fromFileMethod.Invoke(null, new object[] { rdfXmlFormat, filePath });
                            
                            // Extraire le namespace de l'ontologie chargée
                            var ontologyProperty = owlOntologyType.GetProperty("Ontology");
                            if (ontologyProperty != null)
                            {
                                var ontologyValue = ontologyProperty.GetValue(adapter._ontology);
                                adapter._namespace = ontologyValue?.ToString() ?? "http://unknown.namespace.org";
                            }
                            
                            return adapter;
                        }
                    }
                }
                
                throw new InvalidOperationException("Impossible de charger l'ontologie OWL à partir du fichier");
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors du chargement de l'ontologie: {ex.Message}");
                throw;
            }
        }

        public void Annotate(RDFResource property, RDFPlainLiteral value)
        {
            _ontology.Annotate(property, value);
        }

        public void DeclareClass(RDFResource resource)
        {
            _ontology.Model.ClassModel.DeclareClass(resource);
        }

        public void DeclareObjectProperty(RDFResource resource)
        {
            _ontology.Model.PropertyModel.DeclareObjectProperty(resource);
        }

        public void DeclareConceptScheme(RDFResource scheme)
        {
            _ontology.DeclareSKOSConceptScheme(scheme);
        }

        public void DeclareConcept(RDFResource concept, RDFResource scheme)
        {
            _ontology.DeclareSKOSConcept(concept);
            _ontology.AddSKOSConceptToScheme(concept, scheme);
        }

        public void DeclareTopConcept(RDFResource concept, RDFResource scheme)
        {
            _ontology.DeclareSKOSTopConcept(concept, scheme);
        }

        public void DeclareNarrowerConcepts(RDFResource parentConcept, RDFResource childConcept)
        {
            _ontology.DeclareSKOSNarrowerConcept(parentConcept, childConcept);
        }

        public void DeclareExactMatchConcepts(RDFResource concept1, RDFResource concept2)
        {
            _ontology.DeclareSKOSExactMatch(concept1, concept2);
        }

        public void DeclareCloseMatchConcepts(RDFResource concept1, RDFResource concept2)
        {
            _ontology.DeclareSKOSCloseMatch(concept1, concept2);
        }

        public void DeclareBroadMatchConcepts(RDFResource concept1, RDFResource concept2)
        {
            _ontology.DeclareSKOSBroadMatch(concept1, concept2);
        }

        public void DeclareNarrowMatchConcepts(RDFResource concept1, RDFResource concept2)
        {
            _ontology.DeclareSKOSNarrowMatch(concept1, concept2);
        }

        public void DeclareRelatedMatchConcepts(RDFResource concept1, RDFResource concept2)
        {
            _ontology.DeclareSKOSRelatedMatch(concept1, concept2);
        }

        public void DeclareQualifiedCardinalityRestriction(RDFResource restrictionClass, RDFResource onProperty, int cardinality, RDFResource onClass)
        {
            _ontology.Model.ClassModel.DeclareQualifiedCardinalityRestriction(restrictionClass, onProperty, (uint)cardinality, onClass);
        }

        public void DeclareIntersectionClass(RDFResource intersectionClass, List<RDFResource> intersectionClassMembers)
        {
            _ontology.Model.ClassModel.DeclareIntersectionClass(intersectionClass, intersectionClassMembers);
        }

        public void DeclareUnionClass(RDFResource unionClass, List<RDFResource> unionClassMembers)
        {
            _ontology.Model.ClassModel.DeclareUnionClass(unionClass, unionClassMembers);
        }

        public void AnnotateConceptPreferredLabel(RDFResource concept, RDFPlainLiteral label)
        {
            _ontology.AnnotateSKOSPreferredLabel(concept, label);
        }

        public void AnnotateConcept(RDFResource concept, RDFResource property, RDFPlainLiteral value)
        {
            _ontology.Data.AddTriple(new RDFTriple(concept, property, value));
        }

        public void DocumentConcept(RDFResource concept, SKOSDocumentationTypes documentationType, RDFPlainLiteral value)
        {
            switch (documentationType)
            {
                case SKOSDocumentationTypes.Definition:
                    _ontology.AnnotateSKOSDefinition(concept, value);
                    break;
                case SKOSDocumentationTypes.Example:
                    _ontology.AnnotateSKOSExample(concept, value);
                    break;
            }
        }

        public void ToFile(OWLEnums.OWLFormats format, string filePath)
        {
            _ontology.ToFile(format, filePath);
        }

        public List<RDFResource> GetConcepts()
        {
            return _ontology.GetSKOSConcepts().ToList();
        }

        public List<RDFResource> GetTopConcepts()
        {
            return _ontology.GetSKOSTopConcepts().ToList();
        }

        public bool CheckIsNarrowerConcept(RDFResource concept, RDFResource parentConcept)
        {
            return _ontology.CheckHasSKOSNarrowerConcept(parentConcept, concept);
        }

        public List<RDFPlainLiteral> GetConceptPreferredLabels(RDFResource concept)
        {
            return _ontology.GetSKOSPreferredLabels(concept).ToList();
        }

        public List<RDFPlainLiteral> GetConceptDocumentation(RDFResource concept, SKOSDocumentationTypes documentationType)
        {
            switch (documentationType)
            {
                case SKOSDocumentationTypes.Definition:
                    return _ontology.GetSKOSDefinitions(concept).ToList();
                case SKOSDocumentationTypes.Example:
                    return _ontology.GetSKOSExamples(concept).ToList();
                default:
                    return new List<RDFPlainLiteral>();
            }
        }

        public List<RDFResource> GetExactMatchConcepts(RDFResource concept)
        {
            return _ontology.GetSKOSExactMatches(concept).ToList();
        }

        public List<RDFResource> GetCloseMatchConcepts(RDFResource concept)
        {
            return _ontology.GetSKOSCloseMatches(concept).ToList();
        }

        public List<RDFResource> GetRelatedMatchConcepts(RDFResource concept)
        {
            return _ontology.GetSKOSRelatedMatches(concept).ToList();
        }

        public bool CheckHasClass(RDFResource resource)
        {
            return _ontology.Model.ClassModel.CheckHasClass(resource);
        }

        public dynamic GetOntology()
        {
            return _ontology;
        }
    }
}