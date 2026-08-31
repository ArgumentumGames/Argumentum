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

	        // ── #989 architecture B — AIF attack typing with derivation provenance ──
	        // The Virtues' AIF_attackType/AIF_attackedNode values were written by the architecture-B
	        // rule (ai-01 arbitration 2026-08-31): strict majority of the opposed fallacies' measured
	        // attack types, exact tie ⇒ declared gap (empty cells + AIF_skosOther note; the gap rows
	        // emit nothing here, like the root). Publishing the valued rows bare would suggest
	        // line-by-line argumentative judgment, so every emitted assertion carries a provenance
	        // marker. The marker is RE-DERIVED at emission (see DeriveAttackTypeProvenance): a
	        // stored pair carrying the script's fingerprint is "script-derived"; any deviation is
	        // "human-reviewed" — a future real revision flips the markers with no schema change.
	        var aifAttackTypeProp = new RDFResource($"{OntologyNamespace}aifAttackType");
	        var aifAttackedNodeProp = new RDFResource($"{OntologyNamespace}aifAttackedNode");
	        ontology.DeclareObjectProperty(aifAttackedNodeProp);
	        var aifAttackTypeProvenanceProp = new RDFResource($"{OntologyNamespace}aifAttackTypeProvenance");
	        ontology.Annotate(RDFVocabulary.RDFS.COMMENT, new RDFPlainLiteral(DerivationDeclaration, "en"));

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
	                    // #133 : la meme arete en assertion (cf OwlAdapter.DeclareObjectAssertion).
	                    ontology.DeclareObjectAssertion(virtueConcept, goodTenorOfProperty, schemeConcept);
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

	            // #989 branch B — AIF attack typing + derivation provenance (mirrors the Fallacies
	            // emission shape in OwlDocumentConfig.CreateOwlDocument). The root Virtue (pk 0, no
	            // scheme) has empty AIF_attackType by design and emits nothing here.
	            if (!string.IsNullOrWhiteSpace(virtue.AIFAttackType))
	            {
	                var storedType = virtue.AIFAttackType.Trim();
	                var storedNode = virtue.AIFAttackedNode?.Trim() ?? "";
	                ontology.AnnotateConcept(virtueConcept, aifAttackTypeProp, new RDFPlainLiteral(storedType));
	                if (!string.IsNullOrWhiteSpace(storedNode))
	                {
	                    ontology.AnnotateConceptWithResource(virtueConcept, aifAttackedNodeProp, AifNode(storedNode));
	                    // #133 : idem, pour que l'arete d'attaque soit raisonnable et pas seulement lisible.
	                    ontology.DeclareObjectAssertion(virtueConcept, aifAttackedNodeProp, AifNode(storedNode));
	                }
	                ontology.AnnotateConcept(virtueConcept, aifAttackTypeProvenanceProp,
	                    new RDFPlainLiteral(DeriveAttackTypeProvenance(virtue)));
	            }
	        }

	        //Saving
	        await ontology.ToFileAsync(OWLEnums.OWLFormats.OWL2XML, fileName);
	        Logger.LogSuccess($"Virtue Owl document {fileName} successfully saved");
	    }

	    /// <summary>
	    /// #989 architecture B — classifies the STORED pair against the script's fingerprint.
	    /// The corpus was written by the architecture-B rule (ai-01 arbitration 2026-08-31,
	    /// msg-20260831T172136-w5x7gm): strict majority of the opposed fallacies' measured
	    /// AIF_attackType values, exact tie ⇒ declared gap (empty cells + AIF_skosOther note).
	    /// The script's outputs share a verifiable signature WITHOUT loading the Fallacies
	    /// corpus: a deterministic type→node coupling (undercut→RA-node, undermine→I-node,
	    /// rebut→CA-node) and gap separation (a valued row never carries the gap note). That
	    /// signature is what this marker re-derives: signature-consistent ⇒ "script-derived",
	    /// any deviation ⇒ "human-reviewed". Re-deriving the majority itself would require the
	    /// cross-corpus pk→AIF_attackType map (generator Phase 3, deliberately not crossed here).
	    /// Public static so the organ test can drive it on fabricated witnesses (sensitivity
	    /// proof: the marker is computed, not a constant).
	    /// </summary>
	    public static string DeriveAttackTypeProvenance(Virtue virtue)
	    {
	        var storedType = (virtue.AIFAttackType ?? "").Trim();
	        var storedNode = (virtue.AIFAttackedNode ?? "").Trim();
	        var hasGapNote = !string.IsNullOrWhiteSpace(virtue.AIFSkosOther);
	        if (storedType.Length == 0 || hasGapNote)
	        {
	            // No derived value to mark (the emitter never calls the marker for an empty
	            // type), or a valued row carrying a gap declaration — not a script output shape.
	            return "human-reviewed";
	        }
	        var expectedNode = storedType switch
	        {
	            "undercut" => "RA-node",
	            "undermine" => "I-node",
	            "rebut" => "CA-node",
	            _ => null,
	        };
	        return storedNode == expectedNode ? "script-derived" : "human-reviewed";
	    }

	    /// <summary>
	    /// The derivation declaration carried as an ontology-level rdfs:comment — the whole point
	    /// of #989: a reader who has never seen the issue can tell a derived value from a
	    /// reviewed one.
	    /// </summary>
	    public const string DerivationDeclaration =
	        "aifAttackType/aifAttackedNode on these virtues were written by the #989 architecture B " +
	        "rule (ai-01 arbitration 2026-08-31): each virtue carries the strict majority of the " +
	        "measured AIF_attackType values of the fallacies it opposes (crossLink_Opposes); an exact " +
	        "tie is a declared gap — the cells stay empty and the reason is serialized in AIF_skosOther " +
	        "(142 mapped: undermine 74, undercut 63, rebut 5; 80 declared gaps). Each assertion carries " +
	        "aifAttackTypeProvenance, re-derived at emission: 'script-derived' when the stored pair " +
	        "shows the script fingerprint (deterministic type-node coupling — undercut/RA-node, " +
	        "undermine/I-node, rebut/CA-node — and no gap note on a valued row), 'human-reviewed' when " +
	        "it deviates. Re-deriving the majority per row would require the Fallacies corpus " +
	        "(pk to AIF_attackType), deliberately deferred with the generator's Phase 3 cross-corpus " +
	        "architecture; until then the marker verifies the script's signature, not its rule (#989).";

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
