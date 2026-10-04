using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Argumentum.AssetConverter.Mindmapper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
	/// <summary>
	/// #458 grain 4 (dispatch c.5975630522) — <b>contrôle inverse</b> de la visibilité du
	/// repli XSLT : un export FreeMind simulé SANS fichier produit doit laisser une trace
	/// d'AVERTISSEMENT dans le journal, et l'artefact dégradé doit être compté pour le
	/// résumé de fin de passe mindmaps.
	///
	/// <para><b>Ce qui est éprouvé</b> — le point d'insertion de PRODUCTION (chaîne du
	/// créateur Virtues : <c>TryFreeMindSvgExport</c> → repli XSLT), pas le repli appelé
	/// directement : c'est la cascade qui a laissé la carte blanche entrer dans #1740.
	/// FreeMind est « configuré mais absent du disque » (<c>FreeMindPath</c> vers un
	/// exécutable inexistant) : l'export échoue sans lancer de GUI ni attendre 90 s, et
	/// c'est le cas « export sans SVG » du dispatch — même cascade terminale que
	/// « fenêtre FreeMind introuvable » ou « SVG non détecté après keystrokes ».</para>
	///
	/// <para><b>Avant #458 grain 4</b> : la cascade sortait sur <c>LogSuccess</c> — un
	/// artefact aux coordonnées vides (« x="NaN" » ×223 sur les 223 nœuds du triplet zh
	/// Vertus de la rd3, mesuré 04/10) se lisait comme un succès. Le nom de fichier du
	/// test porte un GUID pour que les assertions de journal ne puissent PAS être
	/// satisfaites par une ligne d'une exécution antérieure (journal en append).</para>
	///
	/// <para><b>Filets voisins</b> : <c>MindmapXsltFallbackGateTests</c> (#1740) garde
	/// l'ARBRE committé (marqueurs x="NaN" / « Processing node level ») ; ici on garde la
	/// PASSE — avertissement au journal + comptage pour le résumé. Le chemin Fallacies n'a
	/// pas de repli (retiré par 75a049d3) : cette garde ne couvre que la voie Virtues.</para>
	/// </summary>
	[Trait("Category", "Integration")]
	public class MindmapXsltFallbackSignalTests : IDisposable
	{
		private readonly string _tempDir;

		public MindmapXsltFallbackSignalTests()
		{
			_tempDir = Path.Combine(Path.GetTempPath(), "MindmapXsltFallbackSignal", Path.GetRandomFileName());
			Directory.CreateDirectory(_tempDir);
		}

		public void Dispose()
		{
			try
			{
				if (Directory.Exists(_tempDir))
				{
					Directory.Delete(_tempDir, true);
				}
			}
			catch
			{
				// best-effort : un verrou transitoire ne doit pas faire échouer le test.
			}
		}

		[Fact]
		public void SimulatedFreeMindExportWithoutSvg_WarnsInJournal_AndCountsTheDegradedArtifact()
		{
			// Nom unique par exécution : le journal est en append, une assertion « Contain »
			// sur un nom fixe serait verte à vide dès la deuxième exécution.
			var fileName = $"fallback-signal-{Guid.NewGuid():N}.mm";
			var mmPath = Path.Combine(_tempDir, fileName);
			var svgPath = Path.ChangeExtension(mmPath, ".svg");
			File.WriteAllText(mmPath, "<map version=\"1.0.1\"><node TEXT=\"Racine\"><node TEXT=\"Enfant\"/></node></map>");

			var config = new AssetConverterConfig
			{
				OverwriteExistingDocs = true,
				// « Présent dans la config, absent du disque » : échec d'export immédiat et
				// déterministe (pas de GUI, pas d'attente de fenêtre), indépendant de la
				// variable d'environnement de la machine.
				FreeMindPath = Path.Combine(_tempDir, "FreeMind-absent.exe"),
			};

			FallacyMindMapDocumentConfig.ResetXsltFallbackSignals();
			var engagementsBefore = FallacyMindMapDocumentConfig.XsltFallbackEngagements;
			var artifactsBefore = FallacyMindMapDocumentConfig.XsltFallbackArtifacts;

			var generator = new VirtueMindMapDocumentConfig { DocumentName = fileName };
			var method = typeof(VirtueMindMapDocumentConfig).GetMethod(
				"TryAutomateSvgConversion", BindingFlags.NonPublic | BindingFlags.Instance, null,
				new[] { typeof(string), typeof(string), typeof(AssetConverterConfig), typeof(bool) }, null);
			method.Should().NotBeNull("le point d'insertion de production (chaîne Virtues) doit exister.");

			var produced = (bool)method!.Invoke(generator, new object[] { mmPath, svgPath, config, false })!;

			// Le repli « réussit » : c'est précisément le piège que le grain 4 rend visible.
			produced.Should().BeTrue(
				"le repli XSLT produit un fichier non vide — c'est son succès apparent qui a laissé " +
				"une carte blanche entrer dans #1740 ; s'il échoue ici, c'est la chaîne qui a changé.");

			// …et ce fichier est bien de la famille « voie morte » : il porte les marqueurs NaN
			// que le GABARIT lui-même définit — lus dans mm2svg.xslt, plus de littéral
			// recopié ici (réserve #1740 c.5975615062, pool c.5976781537). Si un jour le
			// stylesheet calcule de vraies coordonnées, la liste trouvée change et cette
			// assertion rougit — c'est voulu : le mot « dead path » du résumé devra être
			// re-mesuré, pas récité.
			var svgContent = File.ReadAllText(svgPath);
			var (_, nanMarkers) = MindmapXsltFallbackMarkers.ReadMarkersFromTemplate(
				MindmapXsltFallbackMarkers.TemplatePath());
			var found = nanMarkers.Where(m => m.IsMatch(svgContent)).Select(m => m.ToString()).ToList();
			found.Should().BeEquivalentTo(new[] { "x=\"NaN\"", "y=\"NaN\"" },
				"l'artefact du repli porte les deux attributs de coordonnées non calculées du gabarit " +
				"(mesuré le 04/10 sur cette carte minuscule — page blanche aux ids présents)");

			// Comptage pour le résumé de fin de passe. Delta (pas valeur absolue) : une autre
			// classe de tests peut appeler TryXsltSvgConversion en parallèle (xUnit parallélise
			// les collections) — le mutant qui retire l'incrément rend bien delta 0 → rouge.
			(FallacyMindMapDocumentConfig.XsltFallbackEngagements - engagementsBefore)
				.Should().BeGreaterOrEqualTo(1, "l'engagement du repli doit être compté.");
			(FallacyMindMapDocumentConfig.XsltFallbackArtifacts - artifactsBefore)
				.Should().BeGreaterOrEqualTo(1, "l'artefact dégradé produit doit être compté pour le résumé.");

			// Journal : chaque étape de la cascade au niveau AVERTISSEMENT, nommant le fichier.
			var journal = File.ReadAllText(Logger.LogFile);
			journal.Should().Contain("[Warning] FreeMind not found",
				"l'export simulé sans SVG doit d'abord dire pourquoi il échoue.");
			journal.Should().Contain($"[Warning] FreeMind GUI unavailable, falling back to XSLT for {fileName}",
				"l'entrée dans le repli est un événement — Information ne suffisait pas (#458 grain 4).");
			journal.Should().Contain($"[Warning] Using XSLT fallback for SVG conversion of {fileName}",
				"l'engagement du repli doit être visible et nommer la carte concernée.");
			journal.Should().Contain($"[Warning] SVG produced via XSLT fallback (degraded, dead path #184): {svgPath}",
				"l'artefact dégradé ne doit plus sortir sous une ligne de succès.");
		}

		[Fact]
		public void SummaryLine_StatesOnlyWhatWasMeasured()
		{
			FallacyMindMapDocumentConfig.BuildXsltFallbackSummaryLine(0, 0)
				.Should().Be("Mindmap pass summary: XSLT fallback never engaged (no degraded fallback artifact).",
					"cas sain : la ligne ne doit PAS prétendre que tous les SVG viennent de Batik (le chemin " +
					"Fallacies n'a pas de repli — une panne FreeMind y laisse aucun SVG, non compté ici).");

			FallacyMindMapDocumentConfig.BuildXsltFallbackSummaryLine(2, 0)
				.Should().Contain("engaged 2 time(s)").And.Contain("produced no artifact",
					"repli engagé sans artefact (stylesheet absente / transform en échec) : dit, pas tu.");

			FallacyMindMapDocumentConfig.BuildXsltFallbackSummaryLine(2, 3)
				.Should().Contain("PRODUCED 3 degraded SVG(s)")
				.And.Contain("Do not copy them into the repository",
					"artefacts dégradés : le résumé doit porter la consigne de remplacement (voie morte #184).");
		}
	}
}