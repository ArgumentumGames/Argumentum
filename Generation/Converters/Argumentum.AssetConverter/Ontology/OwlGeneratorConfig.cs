using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using Humanizer;
using ImageMagick;
using OWLSharp;
// SKOS via raw OWL annotations (SKOSHelper broken in OWLSharp 4.9.0)
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
					DocumentName = "argumentum.owl",
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
			// The space strip is a no-op under Humanizer 2.x (Camelize already joined the words) but
			// mandatory under 3.x, whose Camelize keeps raw spaces around punctuation like '(' '.' '"'
			// — without it, 6 corpus labels (e.g. "drinking the Kool-Aid (politics)") would produce
			// IRIs containing literal spaces, which are invalid in IRI fragments (#951).
			return text.Camelize().Replace("'","").Replace("-","").Replace(",","").Replace(" ","");
	    }

	    public string OntologyNamespace { get; set; } = "";

	    public string ExternalReferenceOntologyNamespaceURI { get; set; } = "";


	    public string ExternalReferenceOntologyUri { get; set; } = "";


		public string Comment { get; set; } = "";


		public string Creator { get; set; } = "";


		public Version Version { get; set; } = new Version(1,0,0);
		

		public override async Task GenerateMindMapFile(IList objects, AssetConverterConfig config, string targetDirectory, string language)
		{
			var fallacyList = objects.Cast<Fallacy>().ToList();
			if (string.IsNullOrEmpty(language))
				language = config.LocalizationConfig.DefaultLanguage;

			// Définir le répertoire de sortie correct
			var outputDir = Path.Combine(config.GetBaseTargetDirectory(language), "Ontology");
			Directory.CreateDirectory(outputDir); // S'assurer que le répertoire existe

			var fileName = Path.Combine(outputDir, DocumentName);

			// On efface l'ancien fichier pour forcer la regénération
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			Logger.Log($"Creating  Owl document {DocumentName} in {outputDir}");
			await CreateOwlDocument(fallacyList, config, language, fileName);
		}

				 private async Task CreateOwlDocument(IList<Fallacy> fallacies, AssetConverterConfig config, string language, string fileName)
	    {
	        var fallaciesByPath = fallacies.ToDictionary(f => f.Path, f => f);
	        Fallacy GetParent(Fallacy f)
	        {
	            if (f.Depth <= 1)
	                return fallacies.First();
	            var parentPath = f.Path.Substring(0, f.Path.LastIndexOf('.'));
	            return fallaciesByPath[parentPath];
	        }
	  
	        var ontology = new OwlAdapter(OntologyNamespace);

	        // AIF init
	        var aifConflictUri = $"{ExternalReferenceOntologyNamespaceURI}Conflict";
	        var conflictResource = new RDFResource(aifConflictUri);

	        ontology.DeclareClass(conflictResource);

	        //Metadata init
	        ontology.Annotate(RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral(Comment, "en"));
	        ontology.Annotate(RDFVocabulary.OWL.VERSION_INFO, new RDFPlainLiteral(Version.ToString()));
	        ontology.Annotate(RDFVocabulary.DC.CREATOR, new RDFPlainLiteral(Creator.ToString()));

	        var aifHasConflictUri = $"{ExternalReferenceOntologyNamespaceURI}hasConflictedElement";
	        var hasConflictResource = new RDFResource(aifHasConflictUri);

	        ontology.DeclareObjectProperty(hasConflictResource);

	        // Scheme declaration
	        var schemeName = GetId(fallacies.First().TextEn);
	        RDFResource mainScheme = new RDFResource($"{OntologyNamespace}{schemeName}Scheme" );
	        ontology.DeclareConceptScheme(mainScheme);

	        var concepts = new Dictionary<Fallacy, RDFResource>();
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
	                        ontology.DeclareClass(hasConflictedTypedInference);
	                        ontology.DeclareQualifiedCardinalityRestriction(hasConflictedTypedInference, hasConflictResource, 1, regularInferenceType);

	                        var typedInferenceConflictResourceUri = $"{OntologyNamespace}{exceptionMapping}_Conflict";
	                        typedInferenceConflictResource = new RDFResource(typedInferenceConflictResourceUri);

	                        ontology.DeclareClass(typedInferenceConflictResource);
	                        var intersectionList = new List<RDFResource> { conflictResource, hasConflictedTypedInference };
	                        ontology.DeclareIntersectionClass(typedInferenceConflictResource, intersectionList);

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
	                        ontology.DeclareClass(conflictUnionClass);
	                        ontology.DeclareUnionClass(conflictUnionClass, mappedConcepts);

	                        mappedConcept = conflictUnionClass;
	                    }

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


	        // ── Transverse cross-links + AIF attack typing (2nd pass: concepts is now fully populated) ──
	        var crossLinkVerbs = new (string Predicate, Func<Fallacy, string> Get, bool Symmetric)[]
	        {
	            ("predatesOn",  f => f.CrossLinkPredatesOn,  false),
	            ("denounces",   f => f.CrossLinkDenounces,   false),
	            ("leverages",   f => f.CrossLinkLeverages,   false),
	            ("allows",      f => f.CrossLinkAllows,      false),
	            ("opposes",     f => f.CrossLinkOpposes,     true),
	            ("inverts",     f => f.CrossLinkInverts,     true),
	            ("mirrors",     f => f.CrossLinkMirrors,     true),
	            ("isRelatedTo", f => f.CrossLinkIsRelatedTo, true),
	        };

	        var crossLinkProps = new Dictionary<string, RDFResource>();
	        foreach (var v in crossLinkVerbs)
	        {
	            var prop = new RDFResource($"{OntologyNamespace}{v.Predicate}");
	            ontology.DeclareObjectProperty(prop);
	            crossLinkProps[v.Predicate] = prop;
	        }

	        var aifAttackTypeProp = new RDFResource($"{OntologyNamespace}aifAttackType");
	        var aifAttackedNodeProp = new RDFResource($"{OntologyNamespace}aifAttackedNode");
	        ontology.DeclareObjectProperty(aifAttackedNodeProp);
	        var aifNodeResources = new Dictionary<string, RDFResource>();
	        RDFResource AifNode(string nodeName)
	        {
	            if (!aifNodeResources.TryGetValue(nodeName, out var res))
	            {
	                res = new RDFResource($"{ExternalReferenceOntologyNamespaceURI}{nodeName}");
	                ontology.DeclareClass(res);
	                aifNodeResources[nodeName] = res;
	            }
	            return res;
	        }

	        foreach (var fallacy in fallacies)
	        {
	            var sourceConcept = concepts[fallacy];

	            foreach (var v in crossLinkVerbs)
	            {
	                var raw = v.Get(fallacy);
	                if (string.IsNullOrWhiteSpace(raw)) continue;
	                foreach (var targetPath in raw.Split(';').Select(x => x.Trim()).Where(x => x.Length > 0))
	                {
	                    if (!fallaciesByPath.TryGetValue(targetPath, out var targetFallacy)) continue;
	                    if (targetFallacy == fallacy) continue;
	                    var targetConcept = concepts[targetFallacy];
	                    ontology.AnnotateConceptWithResource(sourceConcept, crossLinkProps[v.Predicate], targetConcept);
	                    // #133 : la meme arete en assertion, pour qu'un raisonneur la voie (cf OwlAdapter.DeclareObjectAssertion).
	                    ontology.DeclareObjectAssertion(sourceConcept, crossLinkProps[v.Predicate], targetConcept);
	                    if (v.Symmetric)
	                    {
	                        ontology.AnnotateConceptWithResource(targetConcept, crossLinkProps[v.Predicate], sourceConcept);
	                        ontology.DeclareObjectAssertion(targetConcept, crossLinkProps[v.Predicate], sourceConcept);
	                    }
	                }
	            }

	            if (!string.IsNullOrWhiteSpace(fallacy.AIFAttackType))
	            {
	                ontology.AnnotateConcept(sourceConcept, aifAttackTypeProp, new RDFPlainLiteral(fallacy.AIFAttackType.Trim()));
	                if (!string.IsNullOrWhiteSpace(fallacy.AIFAttackedNode))
	                    ontology.AnnotateConceptWithResource(sourceConcept, aifAttackedNodeProp, AifNode(fallacy.AIFAttackedNode.Trim()));
	                    // #133 : idem pour l'arete d'attaque AIF -- c'est elle que le critere
	                    // "delta d'inference non vide" de CoursIA#13567 exige de pouvoir raisonner.
	                    ontology.DeclareObjectAssertion(sourceConcept, aifAttackedNodeProp, AifNode(fallacy.AIFAttackedNode.Trim()));
	            }
	        }

	        //Saving
	        await ontology.ToFileAsync(OWLEnums.OWLFormats.OWL2XML, fileName);
	        Logger.LogSuccess($"Owl document {fileName} successfully saved");
	    }
	    private RDFResource GetFallacyConcept(Fallacy targetFallacy,
	     OwlAdapter ontology, RDFResource mainScheme)
	    {
	        var fallacyId = GetId(targetFallacy.TextEn);
	        var fallacyUri = $"{OntologyNamespace}{fallacyId}";

	        RDFResource fallacyResource = new RDFResource(fallacyUri);
	        ontology.DeclareConcept(fallacyResource, mainScheme);

	        ontology.AnnotateConceptPreferredLabel(fallacyResource, new RDFPlainLiteral(targetFallacy.TextFr, "fr"));
	        ontology.AnnotateConceptPreferredLabel(fallacyResource, new RDFPlainLiteral(targetFallacy.TextEn, "en"));
	  
	        ontology.DocumentConcept(fallacyResource, Ontology.SKOSDocumentationTypes.Definition, new RDFPlainLiteral(targetFallacy.DescFr, "fr"));
	        ontology.DocumentConcept(fallacyResource, Ontology.SKOSDocumentationTypes.Definition, new RDFPlainLiteral(targetFallacy.DescEn, "en"));

	        ontology.DocumentConcept(fallacyResource, Ontology.SKOSDocumentationTypes.Example, new RDFPlainLiteral(targetFallacy.ExampleFr, "fr"));
	        ontology.DocumentConcept(fallacyResource, Ontology.SKOSDocumentationTypes.Example, new RDFPlainLiteral(targetFallacy.ExampleEn, "en"));

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