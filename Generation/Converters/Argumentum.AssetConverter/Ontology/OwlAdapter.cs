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
    /// SKOS vocabulary constants (http://www.w3.org/2004/02/skos/core#)
    /// Used to add SKOS triples as raw OWL annotation assertions,
    /// bypassing the broken SKOSHelper extension methods in OWLSharp 4.9.0.
    /// </summary>
    public static class SKOSVocabulary
    {
        private const string NS = "http://www.w3.org/2004/02/skos/core#";

        public static readonly RDFResource ConceptScheme = new RDFResource($"{NS}ConceptScheme");
        public static readonly RDFResource Concept = new RDFResource($"{NS}Concept");
        public static readonly RDFResource InScheme = new RDFResource($"{NS}inScheme");
        public static readonly RDFResource HasTopConcept = new RDFResource($"{NS}hasTopConcept");
        public static readonly RDFResource TopConceptOf = new RDFResource($"{NS}topConceptOf");
        public static readonly RDFResource Narrower = new RDFResource($"{NS}narrower");
        public static readonly RDFResource Broader = new RDFResource($"{NS}broader");
        public static readonly RDFResource PrefLabel = new RDFResource($"{NS}prefLabel");
        public static readonly RDFResource Definition = new RDFResource($"{NS}definition");
        public static readonly RDFResource Example = new RDFResource($"{NS}example");
        public static readonly RDFResource ExactMatch = new RDFResource($"{NS}exactMatch");
        public static readonly RDFResource CloseMatch = new RDFResource($"{NS}closeMatch");
        public static readonly RDFResource BroadMatch = new RDFResource($"{NS}broadMatch");
        public static readonly RDFResource NarrowMatch = new RDFResource($"{NS}narrowMatch");
        public static readonly RDFResource RelatedMatch = new RDFResource($"{NS}relatedMatch");
    }

    /// <summary>
    /// Adaptateur pour la bibliothèque OWLSharp 4.9.0
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

        public void DeclareConceptScheme(RDFResource scheme)
        {
            DeclareClass(scheme);
            AnnotateConceptWithResource(scheme, RDFVocabulary.RDF.TYPE, SKOSVocabulary.ConceptScheme);
        }

        public void DeclareConcept(RDFResource concept, RDFResource scheme)
        {
            DeclareClass(concept);
            AnnotateConceptWithResource(concept, RDFVocabulary.RDF.TYPE, SKOSVocabulary.Concept);
            AnnotateConceptWithResource(concept, SKOSVocabulary.InScheme, scheme);
        }

        public void DeclareTopConcept(RDFResource concept, RDFResource scheme)
        {
            AnnotateConceptWithResource(scheme, SKOSVocabulary.HasTopConcept, concept);
            AnnotateConceptWithResource(concept, SKOSVocabulary.TopConceptOf, scheme);
        }

        public void DeclareNarrowerConcepts(RDFResource parentConcept, RDFResource childConcept)
        {
            AnnotateConceptWithResource(parentConcept, SKOSVocabulary.Narrower, childConcept);
            AnnotateConceptWithResource(childConcept, SKOSVocabulary.Broader, parentConcept);
        }

        public void DeclareExactMatchConcepts(RDFResource concept1, RDFResource concept2)
        {
            AnnotateConceptWithResource(concept1, SKOSVocabulary.ExactMatch, concept2);
        }

        public void DeclareCloseMatchConcepts(RDFResource concept1, RDFResource concept2)
        {
            AnnotateConceptWithResource(concept1, SKOSVocabulary.CloseMatch, concept2);
        }

        public void DeclareBroadMatchConcepts(RDFResource concept1, RDFResource concept2)
        {
            AnnotateConceptWithResource(concept1, SKOSVocabulary.BroadMatch, concept2);
        }

        public void DeclareNarrowMatchConcepts(RDFResource concept1, RDFResource concept2)
        {
            AnnotateConceptWithResource(concept1, SKOSVocabulary.NarrowMatch, concept2);
        }

        public void DeclareRelatedMatchConcepts(RDFResource concept1, RDFResource concept2)
        {
            AnnotateConceptWithResource(concept1, SKOSVocabulary.RelatedMatch, concept2);
        }

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

        public void AnnotateConceptPreferredLabel(RDFResource concept, RDFPlainLiteral label)
        {
            AnnotateConcept(concept, SKOSVocabulary.PrefLabel, label);
        }

        public void AnnotateConcept(RDFResource concept, RDFResource property, RDFPlainLiteral value)
        {
            _ontology.AnnotationAxioms.Add(new OWLAnnotationAssertion(new OWLAnnotationProperty(property), concept, new OWLLiteral(value)));
        }

        public void AnnotateConceptWithResource(RDFResource subject, RDFResource property, RDFResource value)
        {
            _ontology.AnnotationAxioms.Add(new OWLAnnotationAssertion(new OWLAnnotationProperty(property), subject, value));
        }

        public void DocumentConcept(RDFResource concept, SKOSDocumentationTypes documentationType, RDFPlainLiteral value)
        {
            var property = documentationType switch
            {
                SKOSDocumentationTypes.Definition => SKOSVocabulary.Definition,
                SKOSDocumentationTypes.Example => SKOSVocabulary.Example,
                _ => throw new ArgumentOutOfRangeException(nameof(documentationType))
            };
            AnnotateConcept(concept, property, value);
        }

        public Task ToFileAsync(OWLEnums.OWLFormats format, string filePath)
        {
            return _ontology.ToFileAsync(format, filePath);
        }

        public List<RDFResource> GetConcepts()
        {
            return GetAnnotationSubjects(SKOSVocabulary.Concept);
        }

        public List<RDFResource> GetTopConcepts()
        {
            return GetAnnotationObjects(SKOSVocabulary.HasTopConcept);
        }

        public bool CheckIsNarrowerConcept(RDFResource concept, RDFResource parentConcept)
        {
            return _ontology.AnnotationAxioms.OfType<OWLAnnotationAssertion>()
                .Any(a => a.AnnotationProperty.GetIRI().Equals(SKOSVocabulary.Narrower.URI)
                    && a.SubjectIRI.Equals(parentConcept.URI)
                    && a.ValueIRI != null && a.ValueIRI.Equals(concept.URI));
        }

        public List<RDFPlainLiteral> GetConceptPreferredLabels(RDFResource concept)
        {
            return GetLiteralAnnotations(concept, SKOSVocabulary.PrefLabel);
        }

        public List<RDFPlainLiteral> GetConceptDocumentation(RDFResource concept, SKOSDocumentationTypes documentationType)
        {
            var property = documentationType switch
            {
                SKOSDocumentationTypes.Definition => SKOSVocabulary.Definition,
                SKOSDocumentationTypes.Example => SKOSVocabulary.Example,
                _ => throw new ArgumentOutOfRangeException(nameof(documentationType))
            };
            return GetLiteralAnnotations(concept, property);
        }

        public List<RDFResource> GetExactMatchConcepts(RDFResource concept)
        {
            return GetResourceAnnotations(concept, SKOSVocabulary.ExactMatch);
        }

        public List<RDFResource> GetCloseMatchConcepts(RDFResource concept)
        {
            return GetResourceAnnotations(concept, SKOSVocabulary.CloseMatch);
        }

        public List<RDFResource> GetRelatedMatchConcepts(RDFResource concept)
        {
            return GetResourceAnnotations(concept, SKOSVocabulary.RelatedMatch);
        }

        private List<RDFResource> GetAnnotationSubjects(RDFResource typeResource)
        {
            return _ontology.AnnotationAxioms.OfType<OWLAnnotationAssertion>()
                .Where(a => a.AnnotationProperty.GetIRI().Equals(RDFVocabulary.RDF.TYPE.URI)
                    && a.ValueIRI != null && a.ValueIRI.Equals(typeResource.URI))
                .Select(a => new RDFResource(a.SubjectIRI.ToString()))
                .ToList();
        }

        private List<RDFResource> GetAnnotationObjects(RDFResource property)
        {
            return _ontology.AnnotationAxioms.OfType<OWLAnnotationAssertion>()
                .Where(a => a.AnnotationProperty.GetIRI().Equals(property.URI)
                    && a.ValueIRI != null)
                .Select(a => new RDFResource(a.ValueIRI.ToString()))
                .ToList();
        }

        private List<RDFResource> GetResourceAnnotations(RDFResource subject, RDFResource property)
        {
            return _ontology.AnnotationAxioms.OfType<OWLAnnotationAssertion>()
                .Where(a => a.AnnotationProperty.GetIRI().Equals(property.URI)
                    && a.SubjectIRI.Equals(subject.URI)
                    && a.ValueIRI != null)
                .Select(a => new RDFResource(a.ValueIRI.ToString()))
                .ToList();
        }

        private List<RDFPlainLiteral> GetLiteralAnnotations(RDFResource subject, RDFResource property)
        {
            return _ontology.AnnotationAxioms.OfType<OWLAnnotationAssertion>()
                .Where(a => a.AnnotationProperty.GetIRI().Equals(property.URI)
                    && a.SubjectIRI.Equals(subject.URI)
                    && a.ValueLiteral != null)
                .Select(a => {
                    var literal = a.ValueLiteral.GetLiteral();
                    return literal is RDFPlainLiteral plain ? plain : new RDFPlainLiteral(literal.Value);
                })
                .ToList();
        }

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