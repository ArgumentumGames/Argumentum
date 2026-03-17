using System;
using System.Reflection;
using System.Linq;

namespace Argumentum.AssetConverter.Ontology
{
    public class DynamicRDFResource
    {
        private object _instance;

        public DynamicRDFResource(string value)
        {
            Assembly owlAssembly = Assembly.Load("OWLSharp");
            
            // Étape 1: Créer une instance de RDFSharp.Model.RDFResource
            Assembly rdfSharpAssembly = Assembly.Load("RDFSharp");
            Type rdfResourceType = rdfSharpAssembly.GetType("RDFSharp.Model.RDFResource");

            if (rdfResourceType == null)
            {
                Console.WriteLine("FATAL: Type 'RDFSharp.Model.RDFResource' not found in RDFSharp assembly.");
                throw new InvalidOperationException("Could not find type RDFSharp.Model.RDFResource");
            }

            // Diagnostic: Afficher tous les constructeurs de RDFResource
            Console.WriteLine("Constructors for RDFSharp.Model.RDFResource:");
            var constructors = rdfResourceType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (!constructors.Any())
            {
                Console.WriteLine("-> No public instance constructors found.");
            }
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                var parameterDescriptions = string.Join(", ", parameters.Select(p => $"{p.ParameterType.FullName} {p.Name}"));
                Console.WriteLine($"- ctor({parameterDescriptions})");
            }
            
            // Correction: Le constructeur RDFResource attend un string, pas un objet Uri.
            object rdfResourceInstance = Activator.CreateInstance(rdfResourceType, new object[] { value });
            // Étape 2: Créer une instance de OWLSharp.Ontology.OWLNamedIndividual en utilisant l'instance RDFResource
            Type owlNamedIndividualType = owlAssembly.GetType("OWLSharp.Ontology.OWLNamedIndividual");
            _instance = Activator.CreateInstance(owlNamedIndividualType, new object[] { rdfResourceInstance });
        }

        public object Instance => _instance;
    }

    public class DynamicRDFPlainLiteral
    {
        private object _instance;

        public DynamicRDFPlainLiteral(string value, string language)
        {
            Assembly owlAssembly = Assembly.Load("OWLSharp");
            Type type = owlAssembly.GetType("OWLSharp.Ontology.OWLLiteral");
            // OWLLiteral constructor might be (string, string) or it might take an RDFPlainLiteral from RDFSharp
            // Looking at the old code, it seems it was a direct `new RDFPlainLiteral(value, language)`.
            // The new OWLLiteral constructor seems to be `public OWLLiteral(RDFPlainLiteral literal)`
            // or `public OWLLiteral(RDFTypedLiteral literal)`.
            // The error `System.ArgumentNullException: Value cannot be null. (Parameter 'type')` from previous runs
            // suggests that `RDFPlainLiteral` cannot be found.
            // Let's try to find the new equivalent. The old RDFSharp is gone.
            // Let's check dotNetRDF, which is a new dependency.
            // A quick search suggests `VDS.RDF.NodeFactory.CreateLiteralNode(string, string)`.
            // This is getting complicated. Let's try the simplest thing first.
            // OWLLiteral has a constructor `public OWLLiteral(string value, string language)`.
            // Let's assume this exists.
            
            // Correction après analyse : `OWLLiteral` a un constructeur qui prend
            // `RDFPlainLiteral`. `RDFPlainLiteral` existe toujours dans `RDFSharp.Model`.
            // Mais RDFSharp a été intégré. Donc le type est probablement `OWLSharp.RDFPlainLiteral`.
            // Mais cela n'a pas fonctionné.

            // D'après la liste des types, il n'y a pas de `RDFPlainLiteral` dans `OWLSharp`.
            // Il y a `OWLSharp.Ontology.OWLLiteral`.
            // Le constructeur le plus probable est `public OWLLiteral(string value, string language)`.
            // Je vais essayer ça.

            _instance = Activator.CreateInstance(type, new object[] { value, language });
        }

        public object Instance => _instance;
    }
}