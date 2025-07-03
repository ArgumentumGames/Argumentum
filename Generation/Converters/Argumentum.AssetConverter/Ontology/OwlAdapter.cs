using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using OWLSharp;
using OWLSharp.Ontology;
using RDFSharp.Model;
using VDS.RDF;
using VDS.RDF.Parsing;

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
        private OWLOntology _ontology;
        private string _namespace;

        public OwlAdapter(string ontologyNamespace)
        {
            _namespace = ontologyNamespace;
            Uri = new Uri(ontologyNamespace);
            
            try
            {
                Type owlOntologyType = Type.GetType("OWLSharp.Ontology.OWLOntology, OWLSharp");
                if (owlOntologyType == null)
                {
                    owlOntologyType = Type.GetType("OWLOntology, OWLSharp");
                }
                
                if (owlOntologyType != null)
                {
                    var constructor = owlOntologyType.GetConstructor(new[] { typeof(Uri), typeof(Uri) });
                    if (constructor == null)
                    {
                        constructor = owlOntologyType.GetConstructor(new[] { typeof(Uri) });
                        if (constructor == null)
                        {
                           throw new InvalidOperationException("Impossible de trouver un constructeur approprié pour OWLOntology.");
                        }
                        _ontology = (OWLOntology)constructor.Invoke(new object[] { Uri });
                    }
                    else
                    {
                        _ontology = (OWLOntology)constructor.Invoke(new object[] { Uri, null });
                    }

                    if (_ontology == null)
                    {
                        throw new InvalidOperationException("La création de l'ontologie vide a échoué.");
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

        public Uri Uri { get; private set; }

        public static OwlAdapter FromFile(string filePath)
        {
            try
            {
                OWLOntology ontology = OWLOntology.FromFileAsync(OWLEnums.OWLFormats.OWL2XML, filePath).GetAwaiter().GetResult();
                
                if (ontology == null)
                {
                    throw new InvalidOperationException("Le chargement de l'ontologie a retourné null.");
                }

                var adapter = new OwlAdapter(ontology.IRI.ToString());
                adapter._ontology = ontology;
                adapter._namespace = ontology.IRI.ToString();
                
                return adapter;
            }
            catch (Exception ex)
            {
                Logger.LogProblem($"Erreur lors du chargement de l'ontologie: {ex.Message}");
                throw new InvalidOperationException("Impossible de charger l'ontologie OWL à partir du fichier", ex);
            }
        }

        public void Annotate(RDFResource property, RDFPlainLiteral value)
        {
            _ontology.Annotate(new OWLAnnotation(new OWLAnnotationProperty(property), new OWLLiteral(value)));
        }

        public void DeclareClass(RDFResource resource)
        {
            _ontology.DeclarationAxioms.Add(new OWLDeclaration(new OWLClass(resource)));
        }

        public void DeclareObjectProperty(RDFResource resource)
        {
            _ontology.DeclarationAxioms.Add(new OWLDeclaration(new OWLObjectProperty(resource)));
        }

        // public void DeclareConceptScheme(RDFResource scheme)
        // {
        //     _ontology.DeclareSKOSConceptScheme(scheme);
        // }

        // public void DeclareConcept(RDFResource concept, RDFResource scheme)
        // {
        //     _ontology.DeclareSKOSConcept(concept);
        //     _ontology.AddSKOSConceptToScheme(concept, scheme);
        // }

        // public void DeclareTopConcept(RDFResource concept, RDFResource scheme)
        // {
        //     _ontology.DeclareSKOSTopConcept(concept, scheme);
        // }

        // public void DeclareNarrowerConcepts(RDFResource parentConcept, RDFResource childConcept)
        // {
        //     _ontology.DeclareSKOSNarrowerConcept(parentConcept, childConcept);
        // }

        // public void DeclareExactMatchConcepts(RDFResource concept1, RDFResource concept2)
        // {
        //     _ontology.DeclareSKOSExactMatch(concept1, concept2);
        // }

        // public void DeclareCloseMatchConcepts(RDFResource concept1, RDFResource concept2)
        // {
        //     _ontology.DeclareSKOSCloseMatch(concept1, concept2);
        // }

        // public void DeclareBroadMatchConcepts(RDFResource concept1, RDFResource concept2)
        // {
        //     _ontology.DeclareSKOSBroadMatch(concept1, concept2);
        // }

        // public void DeclareNarrowMatchConcepts(RDFResource concept1, RDFResource concept2)
        // {
        //     _ontology.DeclareSKOSNarrowMatch(concept1, concept2);
        // }

        // public void DeclareRelatedMatchConcepts(RDFResource concept1, RDFResource concept2)
        // {
        //     _ontology.DeclareSKOSRelatedMatch(concept1, concept2);
        // }

        public void DeclareQualifiedCardinalityRestriction(RDFResource restrictionClass, RDFResource onProperty, int cardinality, RDFResource onClass)
        {
            var onPropertyExpression = new OWLObjectProperty(onProperty);
            var onClassExpression = new OWLClass(onClass);
            var cardinalityRestriction = new OWLObjectExactCardinality(onPropertyExpression, (uint)cardinality, onClassExpression);
            var subClass = new OWLClass(restrictionClass);
            _ontology.ClassAxioms.Add(new OWLSubClassOf(subClass, cardinalityRestriction));
        }

        public void DeclareIntersectionClass(RDFResource intersectionClass, List<RDFResource> intersectionClassMembers)
        {
            var intersectionOf = new OWLObjectIntersectionOf(intersectionClassMembers.Select(m => new OWLClass(m)).ToList<OWLClassExpression>());
            var classExpressions = new List<OWLClassExpression> { new OWLClass(intersectionClass), intersectionOf };
            var equivalentClassesAxiom = new OWLEquivalentClasses(classExpressions);
            _ontology.ClassAxioms.Add(equivalentClassesAxiom);
        }

        public void DeclareUnionClass(RDFResource unionClass, List<RDFResource> unionClassMembers)
        {
            var unionOf = new OWLObjectUnionOf(unionClassMembers.Select(m => new OWLClass(m)).ToList<OWLClassExpression>());
            var classExpressions = new List<OWLClassExpression> { new OWLClass(unionClass), unionOf };
            var equivalentClassesAxiom = new OWLEquivalentClasses(classExpressions);
            _ontology.ClassAxioms.Add(equivalentClassesAxiom);
        }

        // public void AnnotateConceptPreferredLabel(RDFResource concept, RDFPlainLiteral label)
        // {
        //     _ontology.AnnotateSKOSPreferredLabel(concept, label);
        // }

        public void AnnotateConcept(RDFResource concept, RDFResource property, RDFPlainLiteral value)
        {
            _ontology.AnnotationAxioms.Add(new OWLAnnotationAssertion(new OWLAnnotationProperty(property), concept, new OWLLiteral(value)));
        }

        // public void DocumentConcept(RDFResource concept, SKOSDocumentationTypes documentationType, RDFPlainLiteral value)
        // {
        //     switch (documentationType)
        //     {
        //         case SKOSDocumentationTypes.Definition:
        //             _ontology.AnnotateSKOSDefinition(concept, value);
        //             break;
        //         case SKOSDocumentationTypes.Example:
        //             _ontology.AnnotateSKOSExample(concept, value);
        //             break;
        //     }
        // }

        public Task ToFileAsync(OWLEnums.OWLFormats format, string filePath)
        {
            return _ontology.ToFileAsync(format, filePath);
        }

        // public List<RDFResource> GetConcepts()
        // {
        //     return _ontology.GetSKOSConcepts().ToList();
        // }

        // public List<RDFResource> GetTopConcepts()
        // {
        //     return _ontology.GetSKOSTopConcepts().ToList();
        // }

        // public bool CheckIsNarrowerConcept(RDFResource concept, RDFResource parentConcept)
        // {
        //     return _ontology.CheckHasSKOSNarrowerConcept(parentConcept, concept);
        // }

        // public List<RDFPlainLiteral> GetConceptPreferredLabels(RDFResource concept)
        // {
        //     return _ontology.GetSKOSPreferredLabels(concept).ToList();
        // }

        // public List<RDFPlainLiteral> GetConceptDocumentation(RDFResource concept, SKOSDocumentationTypes documentationType)
        // {
        //     switch (documentationType)
        //     {
        //         case SKOSDocumentationTypes.Definition:
        //             return _ontology.GetSKOSDefinitions(concept).ToList();
        //         case SKOSDocumentationTypes.Example:
        //             return _ontology.GetSKOSExamples(concept).ToList();
        //         default:
        //             return new List<RDFPlainLiteral>();
        //     }
        // }

        // public List<RDFResource> GetExactMatchConcepts(RDFResource concept)
        // {
        //     return _ontology.GetSKOSExactMatches(concept).ToList();
        // }

        // public List<RDFResource> GetCloseMatchConcepts(RDFResource concept)
        // {
        //     return _ontology.GetSKOSCloseMatches(concept).ToList();
        // }

        // public List<RDFResource> GetRelatedMatchConcepts(RDFResource concept)
        // {
        //     return _ontology.GetSKOSRelatedMatches(concept).ToList();
        // }

        public bool CheckHasClass(RDFResource resource)
        {
            return _ontology.DeclarationAxioms.Any(ax => ax.Expression is OWLClass cls && cls.GetIRI().Equals(resource.URI));
        }

        public OWLOntology GetOntology()
        {
            return _ontology;
        }
    }
}