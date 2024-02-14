using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using Humanizer;
using ImageMagick;
using OWLSharp;
using OWLSharp.Extensions.SKOS;
using QuestPDF.Elements;
using RDFSharp.Model;

namespace Argumentum.AssetConverter.Ontology
{
    public class OwlGeneratorConfig: ParallelFallacyDocumentCreatorConfigBase<OwlDocumentConfig>
	{

		public override string GetLogTitle()
		{
			return "Generating Owl vocabularies";
		}

		public override string GetLogMessage()
		{
			return "In this, an OWL vocabulary generated from the same dataset that was used for cards pdfs and mindmaps.";
		}

		public override List<OwlDocumentConfig> DocumentConfigs { get; set; } = new List<OwlDocumentConfig>(new[]
			{
				new OwlDocumentConfig()
				{
					Enabled = true,
					DocumentName = "argumentum_fallacies.owl",
					DataSet = KnownDataSets.FallaciesTaxonomy,
					OntologyNamespace = "https://www.argumentum.games/argumentum_fallacies.owl#",
					ExternalReferenceOntologyNamespaceURI = "http://www.arg.dundee.ac.uk/aif#",
					ExternalReferenceOntologyUri = "http://www.arg.dundee.ac.uk/wp-content/uploads/AIF.owl",
					Comment = "Fallacies, biases, manipulations in argumentation",
					Version = new Version(1,0,0),
					Creator = "Argumentum",
					
				}
			});

	}

    public class OwlDocumentConfig : FallacyDocumentConfigBase
    {


	    public static string GetId(string text)
	    {
			return text.Camelize().Replace("'","").Replace("-","").Replace(",","");
	    }

	    public string OntologyNamespace { get; set; } = "";

	    public string ExternalReferenceOntologyNamespaceURI { get; set; } = "";


	    public string ExternalReferenceOntologyUri { get; set; } = "";


		public string Comment { get; set; } = "";


		public string Creator { get; set; } = "";


		public Version Version { get; set; } = new Version(1,0,0);
		

		public override async Task GenerateFallacyFile(IList<Fallacy> fallacies, AssetConverterConfig config, string targetDirectory, string language)
	    {
			if (string.IsNullOrEmpty(language))
				language = config.LocalizationConfig.DefaultLanguage;


			var fileName = DocumentName;
			if (!string.IsNullOrEmpty(targetDirectory))
			{
				fileName = Path.Combine(targetDirectory, fileName);

			}
			var documentPath = Path.Combine(targetDirectory, DocumentName);

			CreateOwlDocument(fallacies, config, language, documentPath, fileName);
		}

	    private void CreateOwlDocument(IList<Fallacy> fallacies, AssetConverterConfig config, string language, string documentPath, string fileName)
	    {

		    var fallaciesByPath = fallacies.ToDictionary(f => f.Path, f => f);
		    Fallacy GetParent(Fallacy f)
		    {
			    if (f.Depth <= 1)
				    return fallacies.First();
			    var parentPath = f.Path.Substring(0, f.Path.LastIndexOf('.'));
			    return fallaciesByPath[parentPath];
			    

			}
			
			var ontology = new OWLOntology(OntologyNamespace);

			//Metadata init


			ontology.Annotate(RDFVocabulary.RDFS.COMMENT,
				new RDFPlainLiteral(Comment, "en"));

			ontology.Annotate(RDFVocabulary.OWL.VERSION_INFO,
				new RDFPlainLiteral(Version.ToString()));

			ontology.Annotate(RDFVocabulary.DC.CREATOR,
				new RDFPlainLiteral(Creator.ToString()));


			//SKOS init


			ontology.InitializeSKOS();



			// AIF init


			//ontology.Import(new RDFResource(AIFUri));


			var aifConflictUri = $"{ExternalReferenceOntologyNamespaceURI}Conflict";
			var conflictResource = new RDFResource(aifConflictUri);

			ontology.Model.ClassModel.DeclareClass(conflictResource);

			var aifHasConflictUri = $"{ExternalReferenceOntologyNamespaceURI}hasConflictedElement";
			var hasConflictResource = new RDFResource(aifHasConflictUri);

			ontology.Model.PropertyModel.DeclareObjectProperty(hasConflictResource);



			// Scheme declararation

			var schemeName = GetId(fallacies.First().TextEn);
			RDFResource mainScheme = new RDFResource($"{OntologyNamespace}{schemeName}Scheme" );
			ontology.DeclareConceptScheme(mainScheme);

			var concepts = new Dictionary<Fallacy, RDFResource>();
			

			//var aifNamespace = new RDFNamespace("aif", AIFNamespaceURI);


			var conflictedTypedInferences = new Dictionary<string, RDFResource>();


			foreach (var fallacy in fallacies)
			{

				var fallacyConcept = this.GetFallacyConcept(fallacy, ontology, mainScheme);
				concepts[fallacy] = fallacyConcept;


				// Hierarchy

				var parentFallacy = GetParent(fallacy);

				if (parentFallacy == fallacy)
				{
					ontology.DeclareTopConcept(fallacyConcept, mainScheme);
				}
				else
				{
					var parentResource = concepts[parentFallacy];
					try
					{
						ontology.DeclareNarrowerConcepts(parentResource, fallacyConcept);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
					
				}

				//AIF mappings

				if (!string.IsNullOrEmpty(fallacy.AIFSkosMappingType))
				{
					var directMappings = fallacy.AIFSkosDirectRef.Split(',').Select(x=>x.Trim()).Where(x=>!string.IsNullOrEmpty(x));
					var exceptionMappings = fallacy.AIFSkosExceptionRef.Split(',').Select(x=>x.Trim()).Where(x => !string.IsNullOrEmpty(x));

					var mappedConcepts = new List<RDFResource>();



					//Direct mappings

					foreach (var directMapping in directMappings)
					{
						var aifUri = $"{ExternalReferenceOntologyNamespaceURI}{directMapping}";

						var directConcept = new RDFResource(aifUri);

						mappedConcepts.Add(directConcept);
						
					}

					
					//Indirect exception mappings

					foreach (var exceptionMapping in exceptionMappings)
					{
						

						
						if (!conflictedTypedInferences.TryGetValue(exceptionMapping,out var typedInferenceConflictResource))
						{
							var aifUri = $"{ExternalReferenceOntologyNamespaceURI}{exceptionMapping}";

							var regularInferenceType = new RDFResource(aifUri);

							var conflictedTypedInferenceUri = $"{OntologyNamespace}{exceptionMapping}_Conflicted";


							var hasConflictedTypedInference = new RDFResource(conflictedTypedInferenceUri);

							ontology.Model.ClassModel.DeclareClass(hasConflictedTypedInference);

							ontology.Model.ClassModel.DeclareQualifiedCardinalityRestriction(hasConflictedTypedInference, hasConflictResource, 1, regularInferenceType);


							var typedInferenceConflictResourceUri = $"{OntologyNamespace}{exceptionMapping}_Conflict";

							typedInferenceConflictResource = new RDFResource(typedInferenceConflictResourceUri);

							ontology.Model.ClassModel.DeclareClass(typedInferenceConflictResource);
							ontology.Model.ClassModel.DeclareIntersectionClass(typedInferenceConflictResource, new() { conflictResource, hasConflictedTypedInference });

							conflictedTypedInferences[exceptionMapping] = typedInferenceConflictResource;
						}



						mappedConcepts.Add(typedInferenceConflictResource);
					}

					

					if (mappedConcepts.Count>0)
					{
						RDFResource mappedConcept = mappedConcepts.First();

						if (mappedConcepts.Count>1)
						{
							var fallacyId = GetId(fallacy.TextEn);
							var fallacyConflictUri = $"{OntologyNamespace}{fallacyId}_Conflict";


							RDFResource conflictUnionClass = new RDFResource(fallacyConflictUri);

							ontology.Model.ClassModel.DeclareClass(conflictUnionClass);

							ontology.Model.ClassModel.DeclareUnionClass(conflictUnionClass, mappedConcepts);

							mappedConcept = conflictUnionClass;

						}


						//ontology.Model.ClassModel.DeclareEquivalentClasses(conflictEquivalentClass, intersectionClass);


						switch (fallacy.AIFSkosMappingType)
						{
							case "skos:exactMatch":
								ontology.DeclareExactMatchConcepts(fallacyConcept, mappedConcept);
								break;
							case "skos:closeMatch":
								ontology.DeclareCloseMatchConcepts(fallacyConcept, mappedConcept);
								break;
							case "skos:broadMatch":
								ontology.DeclareBroadMatchConcepts(fallacyConcept, mappedConcept);
								break;
							case "skos:narrowMatch":
								ontology.DeclareNarrowMatchConcepts(fallacyConcept, mappedConcept);
								break;
							case "skos:relatedMatch":
								ontology.DeclareRelatedMatchConcepts(fallacyConcept, mappedConcept);
								break;
						}


					}

				}

			}

			//Saving

			var xmlFormat = OWLEnums.OWLFormats.OwlXml;
			// WRITE OWL2/XML FILE
			ontology.ToFile(xmlFormat, fileName);

		}

	    private RDFResource GetFallacyConcept(Fallacy targetFallacy,
		    OWLOntology ontology, RDFResource mainScheme)
	    {
		    var fallacyId = GetId(targetFallacy.TextEn);
		    var fallacyUri = $"{OntologyNamespace}{fallacyId}";

			RDFResource fallacyResource = new RDFResource(fallacyUri);
			ontology.DeclareConcept(fallacyResource, mainScheme);

			ontology.AnnotateConceptPreferredLabel(fallacyResource, new RDFPlainLiteral(targetFallacy.TextFr, "fr"));
			ontology.AnnotateConceptPreferredLabel(fallacyResource, new RDFPlainLiteral(targetFallacy.TextEn, "en"));
			
			ontology.DocumentConcept(fallacyResource, SKOSEnums.SKOSDocumentationTypes.Definition, new RDFPlainLiteral(targetFallacy.DescFr, "fr"));
			ontology.DocumentConcept(fallacyResource, SKOSEnums.SKOSDocumentationTypes.Definition, new RDFPlainLiteral(targetFallacy.DescEn, "en"));

			ontology.DocumentConcept(fallacyResource, SKOSEnums.SKOSDocumentationTypes.Example, new RDFPlainLiteral(targetFallacy.ExampleFr, "fr"));
			ontology.DocumentConcept(fallacyResource, SKOSEnums.SKOSDocumentationTypes.Example, new RDFPlainLiteral(targetFallacy.ExampleEn, "en"));


			if (!string.IsNullOrEmpty(targetFallacy.LinkEn))
			{
				ontology.AnnotateConcept(fallacyResource, RDFVocabulary.RDFS.SEE_ALSO, new RDFPlainLiteral(targetFallacy.LinkEn, "en"));
			}
			if (!string.IsNullOrEmpty(targetFallacy.LinkFr))
			{
				ontology.AnnotateConcept(fallacyResource, RDFVocabulary.RDFS.SEE_ALSO, new RDFPlainLiteral(targetFallacy.LinkFr, "fr"));
			}


			return fallacyResource;
		}
	}
}