using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using Humanizer;
using OWLSharp;
using RDFSharp.Model;

namespace Argumentum.AssetConverter.Ontology
{
    /// <summary>
    /// #499 Phase 2 — OWL/SKOS generator for the Virtues taxonomy (mono-corpus).
    /// Mirrors <see cref="OwlGeneratorConfig"/> (Fallacies), emitting argumentum_virtues.owl:
    /// a concept scheme with the 7-family hierarchy (skos:narrower), one concept per Virtue node,
    /// and a custom AIF link <c>aif:goodTenorOf</c> to the Walton argumentation scheme
    /// declared in <see cref="Virtue.AIFSkosDirectRef"/> (the critical question prose in
    /// <see cref="Virtue.AIFCriticalQuestion"/> is carried as an rdfs:comment annotation).
    ///
    /// Cross-corpus Virtue↔Fallacy links (crossLink_Opposes PK→URI resolution) are deferred to
    /// Phase 3: resolving Fallacy PKs requires loading the Fallacies corpus (new architecture,
    /// single-dataset in this pass).
    /// </summary>
    public class VirtueOwlGeneratorConfig : ParallelVirtueDocumentCreatorConfigBase<VirtueOwlDocumentConfig>
	{

		public override string GetLogTitle()
		{
			return "Generating Virtues Owl vocabulary";
		}

		public override string GetLogMessage()
		{
			return "In this, an OWL vocabulary generated from the same Virtues dataset that was used for cards pdfs and mindmaps.";
		}

		public override List<VirtueOwlDocumentConfig> DocumentConfigs { get; set; } = new List<VirtueOwlDocumentConfig>(new[]
			{
				new VirtueOwlDocumentConfig()
				{
					Enabled = true,
					DocumentName = "argumentum_virtues.owl",
					DataSet = KnownDataSets.VirtuesTaxonomy,
					OntologyNamespace = "https://www.argumentum.games/argumentum_virtues.owl#",
					ExternalReferenceOntologyNamespaceURI = "http://www.arg.dundee.ac.uk/aif#",
					ExternalReferenceOntologyUri = "http://www.arg.dundee.ac.uk/wp-content/uploads/AIF.owl",
					Comment = "Virtuous argumentation taxonomy — the mirror of the fallacies axis (223 nodes, 7 families)",
					Version = new Version(1,0,0),
					Creator = "Argumentum",

				}
			});

	}

    /// <summary>
    /// Document config driving the Virtues OWL pass. Mirrors <see cref="OwlDocumentConfig"/>
    /// (Fallacies), adapted to the Virtue entity and the AIF "good tenor of a scheme" semantics.
    /// </summary>
    public class VirtueOwlDocumentConfig : VirtueDocumentConfigBase
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


		public override async Task GenerateMindMapFile(IList objects, AssetConverterConfig config, string targetDirectory, string language)
		{
			var virtueList = objects.Cast<Virtue>().ToList();
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

			Logger.Log($"Creating Virtues Owl document {DocumentName} in {outputDir}");
			await CreateVirtueOwlDocument(virtueList, config, language, fileName);
		}

		private async Task CreateVirtueOwlDocument(IList<Virtue> virtues, AssetConverterConfig config, string language, string fileName)
	    {
	        var virtuesByPath = virtues.ToDictionary(v => v.Path, v => v);
	        Virtue GetParent(Virtue v)
	        {
	            if (v.Depth <= 1)
	                return virtues.First();
	            var parentPath = v.Path.Substring(0, v.Path.LastIndexOf('.'));
	            return virtuesByPath[parentPath];
	        }

	        var ontology = new OwlAdapter(OntologyNamespace);

	        // Metadata init
	        ontology.Annotate(RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral(Comment, "en"));
	        ontology.Annotate(RDFVocabulary.OWL.VERSION_INFO, new RDFPlainLiteral(Version.ToString()));
	        ontology.Annotate(RDFVocabulary.DC.CREATOR, new RDFPlainLiteral(Creator.ToString()));

	        // AIF object property: a Virtue is the "good tenor of" a Walton argumentation scheme
	        // (i.e. the correct practice / answer to the scheme's critical questions). The Fallacies
	        // switch over skos:*Match tokens cannot fire here: since #989 the Virtue critical
	        // questions live in AIF_criticalQuestion and the Virtue AIF_skosMappingType column is
	        // empty (no SKOS mapping is defined for Virtues yet — design adaptation 1).
	        var aifGoodTenorOfUri = $"{ExternalReferenceOntologyNamespaceURI}goodTenorOf";
	        var goodTenorOfProperty = new RDFResource(aifGoodTenorOfUri);
	        ontology.DeclareObjectProperty(goodTenorOfProperty);

	        // Scheme declaration
	        var schemeName = GetId(virtues.First().TitleEn);
	        RDFResource mainScheme = new RDFResource($"{OntologyNamespace}{schemeName}Scheme");
	        ontology.DeclareConceptScheme(mainScheme);

	        var concepts = new Dictionary<Virtue, RDFResource>();

	        foreach (var virtue in virtues)
	        {
	            var virtueConcept = this.GetVirtueConcept(virtue, ontology, mainScheme);
	            concepts[virtue] = virtueConcept;

	            // Hierarchy
	            var parentVirtue = GetParent(virtue);

	            if (parentVirtue == virtue)
	            {
	                ontology.DeclareTopConcept(virtueConcept, mainScheme);
	            }
	            else
	            {
	                var parentResource = concepts[parentVirtue];
	                try
	                {
	                    ontology.DeclareNarrowerConcepts(parentResource, virtueConcept);
	                }
	                catch (Exception e)
	                {
	                    Console.WriteLine(e);
	                }
	            }

	            // AIF good-tenor link: Virtue → Walton scheme concept(s), prose critical question
	            // carried as an rdfs:comment annotation. Cross-corpus crossLink_Opposes is Phase 3.
	            if (!string.IsNullOrEmpty(virtue.AIFSkosDirectRef))
	            {
	                var schemeMappings = virtue.AIFSkosDirectRef.Split(',')
	                    .Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x));

	                foreach (var schemeMapping in schemeMappings)
	                {
	                    var schemeUri = $"{ExternalReferenceOntologyNamespaceURI}{schemeMapping}";
	                    var schemeConcept = new RDFResource(schemeUri);
	                    ontology.AnnotateConceptWithResource(virtueConcept, goodTenorOfProperty, schemeConcept);
	                }

	                // The critical-question prose lives in AIF_criticalQuestion since #989 (it was
	                // mis-housed in AIF_skosMappingType, which is now skos:*Match-only and empty on
	                // the Virtues side); carry the prose as a free-text rdfs:comment so it stays
	                // consumable and lossless.
	                if (!string.IsNullOrEmpty(virtue.AIFCriticalQuestion))
	                {
	                    ontology.AnnotateConcept(virtueConcept, RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral(virtue.AIFCriticalQuestion, "fr"));
	                }
	            }
	        }

	        //Saving
	        await ontology.ToFileAsync(OWLEnums.OWLFormats.OWL2XML, fileName);
	        Logger.LogSuccess($"Virtue Owl document {fileName} successfully saved");
	    }

	    private RDFResource GetVirtueConcept(Virtue targetVirtue,
	     OwlAdapter ontology, RDFResource mainScheme)
	    {
	        var virtueId = GetId(targetVirtue.TitleEn);
	        var virtueUri = $"{OntologyNamespace}{virtueId}";

	        RDFResource virtueResource = new RDFResource(virtueUri);
	        ontology.DeclareConcept(virtueResource, mainScheme);

	        ontology.AnnotateConceptPreferredLabel(virtueResource, new RDFPlainLiteral(targetVirtue.TitleFr, "fr"));
	        ontology.AnnotateConceptPreferredLabel(virtueResource, new RDFPlainLiteral(targetVirtue.TitleEn, "en"));

	        ontology.DocumentConcept(virtueResource, Ontology.SKOSDocumentationTypes.Definition, new RDFPlainLiteral(targetVirtue.DescriptionFr, "fr"));
	        ontology.DocumentConcept(virtueResource, Ontology.SKOSDocumentationTypes.Definition, new RDFPlainLiteral(targetVirtue.DescriptionEn, "en"));

	        // Virtues carry no example field in the source taxonomy (Virtue.Example == string.Empty).

	        if (!string.IsNullOrEmpty(targetVirtue.LinkEn))
	        {
	            ontology.AnnotateConcept(virtueResource, RDFVocabulary.RDFS.SEE_ALSO, new RDFPlainLiteral(targetVirtue.LinkEn, "en"));
	        }
	        if (!string.IsNullOrEmpty(targetVirtue.LinkFr))
	        {
	            ontology.AnnotateConcept(virtueResource, RDFVocabulary.RDFS.SEE_ALSO, new RDFPlainLiteral(targetVirtue.LinkFr, "fr"));
	        }

	        return virtueResource;
	    }
	}
}
