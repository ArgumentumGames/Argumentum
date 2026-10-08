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
	/// #458 grain 4 (dispatch c.5975630522) — visibilité du repli XSLT ; <b>#458 reprise
	/// (décision c.5977924514)</b> — le repli Vertus est RETIRÉ et un export attendu sans
	/// SVG fait <b>échouer la passe</b> : résumé, puis <c>throw [MINDMAP-PARTIAL]</c> en
	/// nommant la carte, comme le <c>[HARVEST-PARTIAL]</c> de #613.
	///
	/// <para><b>Ce qui est éprouvé</b> — les points d'insertion de PRODUCTION (les chaînes
	/// des DEUX créateurs : <c>TryAutomateSvgConversion</c> Sophismes et Vertus), pas des
	/// fonctions appelées directement. FreeMind est « configuré mais absent du disque »
	/// (<c>FreeMindPath</c> vers un exécutable inexistant) : l'export échoue sans lancer de
	/// GUI ni attendre 90 s, et c'est le cas « export sans SVG » du dispatch — même cascade
	/// terminale que « fenêtre FreeMind introuvable » ou « SVG non détecté après
	/// keystrokes ». Le nom de fichier du test porte un GUID pour que les assertions de
	/// journal ne puissent PAS être satisfaites par une ligne d'une exécution antérieure
	/// (journal en append).</para>
	///
	/// <para><b>Avant la reprise</b> : côté Vertus, le repli XSLT « réussissait » et
	/// l'artefact dégradé (x="NaN" ×223 sur 223 nœuds, mesuré 04/10) sortait sous un
	/// LogSuccess — la carte blanche est entrée dans #1740. Côté Sophismes (repli déjà
	/// retiré par 75a049d3), une panne FreeMind ne laissait <b>aucune trace comptée</b>
	/// (§6 du dossier #1744) : la passe sortait verte avec 0 SVG. La reprise ferme les
	/// deux voies.</para>
	///
	/// <para><b>Filets voisins</b> : <c>MindmapXsltFallbackGateTests</c> (#1740) garde
	/// l'ARBRE committé ; <c>TryXsltSvgConversion</c> reste appelable HORS PASSE pour un
	/// usage explicite (épinglé ici même, voie morte #184).</para>
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

		private static readonly Type[] ConversionSignature =
			{ typeof(string), typeof(string), typeof(AssetConverterConfig), typeof(bool) };

		private static bool InvokeConversion(object generator, string mmPath, string svgPath, AssetConverterConfig config)
		{
			var method = generator.GetType().GetMethod(
				"TryAutomateSvgConversion", BindingFlags.NonPublic | BindingFlags.Instance, null, ConversionSignature, null);
			method.Should().NotBeNull("le point d'insertion de production doit exister ({0}).", generator.GetType().Name);
			return (bool)method!.Invoke(generator, new object[] { mmPath, svgPath, config, false })!;
		}

		private static string WriteTempMm(string dir, string fileName)
		{
			var mmPath = Path.Combine(dir, fileName);
			File.WriteAllText(mmPath, "<map version=\"1.0.1\"><node TEXT=\"Racine\"><node TEXT=\"Enfant\"/></node></map>");
			return mmPath;
		}

		/// <summary>
		/// Longueur observée du journal partagé (0 s'il n'existe pas encore) — sert à ne lire que la
		/// QUEUE ajoutée par ce test, jamais le contenu déposé par les autres collections.
		/// </summary>
		private static long LogLength()
		{
			var info = new FileInfo(Logger.LogFile);
			return info.Exists ? info.Length : 0;
		}

		/// <summary>
		/// Lit la queue du journal partagé à partir de <paramref name="offset"/>.
		///
		/// <para><b>Pourquoi pas <c>File.ReadAllText</c>.</b> <c>Logger.Log</c> écrit sous
		/// <c>lock (fileLock)</c> via <c>File.AppendAllText</c>, qui ouvre en <c>FileAccess.Write</c>
		/// + <c>FileShare.Read</c>. <c>File.ReadAllText</c> ouvre en <c>FileAccess.Read</c> +
		/// <c>FileShare.Read</c> : un partage qui n'autorise PAS l'écriture concurrente. Si un autre
		/// fil est <i>dans</i> <c>AppendAllText</c> au même instant, <b>l'ouverture du lecteur
		/// échoue</b> (violation de partage) et le test tombe par exception au lieu d'assertion —
		/// le motif « rouge dans la suite complète, vert en isolation » que <c>Logger.cs</c>
		/// documente déjà pour <c>LogFile</c>. Ici l'ouverture déclare <c>FileShare.ReadWrite</c>
		/// (l'écrivain concurrent est accepté) et la lecture est bornée à la queue ajoutée depuis
		/// <paramref name="offset"/> (le texte des autres collections n'entre pas dans l'assertion).</para>
		///
		/// <para>⚠️ Ceci <b>retire une classe de risque</b> — ouverture refusée, contamination — ;
		/// ce n'est <b>pas</b> la démonstration de la cause d'un flake observé une fois.</para>
		/// </summary>
		private static string ReadLogTail(long offset)
		{
			if (!File.Exists(Logger.LogFile))
			{
				return string.Empty;   // rien n'a été écrit : l'assertion le dira, on ne masque rien
			}

			using var stream = new FileStream(
				Logger.LogFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			if (offset > stream.Length)
			{
				offset = 0;            // journal archivé/recréé entre les deux mesures : on relit tout
			}

			stream.Seek(offset, SeekOrigin.Begin);
			using var reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}

		[Fact]
		public void VirtuesChain_FreeMindAbsent_NoXsltFallbackAnymore_ExportRecordedMissing()
		{
			// Contrôle inverse du retrait (c.5977924514) : FreeMind absent → PLUS AUCUN
			// repli, plus aucun SVG produit — et l'absence est comptée pour la passe.
			var fileName = $"virtues-nofallback-{Guid.NewGuid():N}.mm";
			var mmPath = WriteTempMm(_tempDir, fileName);
			var svgPath = Path.ChangeExtension(mmPath, ".svg");

			var config = new AssetConverterConfig
			{
				OverwriteExistingDocs = true,
				// « Présent dans la config, absent du disque » : échec d'export immédiat et
				// déterministe (pas de GUI, pas d'attente de fenêtre).
				FreeMindPath = Path.Combine(_tempDir, "FreeMind-absent.exe"),
			};

			FallacyMindMapDocumentConfig.ResetXsltFallbackSignals();

			// Frontière de lecture : tout ce que le journal gagne À PARTIR D'ICI est de ce test.
			var logLengthBefore = LogLength();
			var produced = InvokeConversion(new VirtueMindMapDocumentConfig { DocumentName = fileName }, mmPath, svgPath, config);

			produced.Should().BeFalse(
				"le repli XSLT est retiré de la chaîne Vertus (c.5977924514) : une panne FreeMind " +
				"doit laisser 0 SVG, pas un artefact dégradé — s'il revient, la reprise a été défait.");
			File.Exists(svgPath).Should().BeFalse("aucun SVG ne doit être produit par la voie morte.");

			FallacyMindMapDocumentConfig.MissingSvgExports.Should().Contain(Path.GetFileName(svgPath),
				"l'export attendu sans SVG doit être enregistré pour le résumé de fin de passe.");

			var journal = ReadLogTail(logLengthBefore);
			journal.Should().Contain("[Warning] FreeMind not found",
				"l'export doit d'abord dire pourquoi il échoue.");
			journal.Should().NotContain($"falling back to XSLT for {fileName}",
				"le repli XSLT est retiré de la chaîne Vertus — cette ligne ne doit plus exister.");
			journal.Should().NotContain($"Using XSLT fallback for SVG conversion of {fileName}",
				"l'engagement du repli ne doit plus être atteignable depuis la passe.");
		}

		[Fact]
		public void FallaciesChain_FreeMindAbsent_ExportRecordedMissing_PassThrowsNamingTheCard()
		{
			// Le trou du §6 du dossier #1744 : côté Sophismes (pas de repli depuis
			// 75a049d3), une panne FreeMind ne laissait AUCUNE trace comptée. La passe
			// doit maintenant ÉCHOUER en nommant la carte (contrôle inverse du dispatch).
			var fileName = $"fallacies-partial-{Guid.NewGuid():N}.mm";
			var mmPath = WriteTempMm(_tempDir, fileName);
			var svgPath = Path.ChangeExtension(mmPath, ".svg");

			var config = new AssetConverterConfig
			{
				OverwriteExistingDocs = true,
				FreeMindPath = Path.Combine(_tempDir, "FreeMind-absent.exe"),
			};

			FallacyMindMapDocumentConfig.ResetXsltFallbackSignals();

			var produced = InvokeConversion(new FallacyMindMapDocumentConfig { DocumentName = fileName }, mmPath, svgPath, config);

			produced.Should().BeFalse("FreeMind absent : aucun SVG produit.");
			FallacyMindMapDocumentConfig.MissingSvgExports.Should().Contain(Path.GetFileName(svgPath),
				"l'export attendu sans SVG doit être enregistré par la chaîne Sophismes aussi.");

			// Résumé puis throw : le contrat [HARVEST-PARTIAL] (#613) appliqué aux mindmaps.
			FallacyMindMapDocumentConfig.BuildMissingSvgSummaryLine(FallacyMindMapDocumentConfig.MissingSvgExports.Count)
				.Should().Contain("MISSING", "le résumé dit l'absence mesurée, il ne la tait pas.");

			var act = () => FallacyMindMapDocumentConfig.ThrowIfMissingSvgExports();
			act.Should().ThrowExactly<InvalidOperationException>()
				.Which.Message.Should().Contain("[MINDMAP-PARTIAL]")
				.And.Contain(Path.GetFileName(svgPath),
					"la passe échoue en NOMMANT la carte — un échec anonyme ne se répare pas.");
		}

		[Fact]
		public void MissingSvgExports_ThrowIsSilentWhenNothingIsMissing()
		{
			// Cas sain : fenêtre ouverte sans rien enregistrer → la passe sort verte.
			FallacyMindMapDocumentConfig.ResetXsltFallbackSignals();
			FallacyMindMapDocumentConfig.MissingSvgExports.Should().BeEmpty(
				"la remise à zéro ouvre une fenêtre propre pour la passe.");
			var act = () => FallacyMindMapDocumentConfig.ThrowIfMissingSvgExports();
			act.Should().NotThrow("aucun export manquant : la passe ne doit pas échouer.");
		}

		[Fact]
		public void MissingSummaryLine_StatesOnlyWhatWasMeasured()
		{
			FallacyMindMapDocumentConfig.BuildMissingSvgSummaryLine(0)
				.Should().Be("Mindmap pass summary: every expected SVG export was produced (0 missing).",
					"cas sain : dit le mesuré (0 manquant), ne prétend pas « tous issus de Batik ».");

			FallacyMindMapDocumentConfig.BuildMissingSvgSummaryLine(3)
				.Should().Contain("3 expected SVG export(s) MISSING")
					.And.Contain("FreeMind GUI produced no SVG",
						"l'absence doit nommer sa cause mesurée (export GUI sans SVG).");
		}

		[Fact]
		public void RetiredXsltPath_ExplicitOutOfPassCall_StillMeasuresTheDeadPath()
		{
			// TryXsltSvgConversion n'est plus appelée par AUCUNE passe (c.5977924514) ;
			// elle reste pour un usage EXPLICITE hors passe — ce fait épingle sa nature :
			// l'appel direct produit toujours la voie morte #184 (coordonnées NaN LUES
			// DANS LE GABARIT mm2svg.xslt, pas un littéral recopié — réserve #1740
			// c.5975615062). Si le stylesheet calcule un jour de vraies coordonnées, cette
			// assertion rougit : le mot « dead path » devra être re-mesuré, pas récité.
			var fileName = $"explicit-xslt-{Guid.NewGuid():N}.mm";
			var mmPath = WriteTempMm(_tempDir, fileName);
			var svgPath = Path.ChangeExtension(mmPath, ".svg");

			FallacyMindMapDocumentConfig.ResetXsltFallbackSignals();
			var engagementsBefore = FallacyMindMapDocumentConfig.XsltFallbackEngagements;
			var artifactsBefore = FallacyMindMapDocumentConfig.XsltFallbackArtifacts;

			var produced = FallacyMindMapDocumentConfig.TryXsltSvgConversion(mmPath, svgPath);

			produced.Should().BeTrue(
				"le repli produit un fichier non vide — c'est précisément ce succès apparent " +
				"qui a laissé une carte blanche entrer dans #1740 ; s'il échoue, la voie a changé.");

			var svgContent = File.ReadAllText(svgPath);
			var (_, nanMarkers) = MindmapXsltFallbackMarkers.ReadMarkersFromTemplate(
				MindmapXsltFallbackMarkers.TemplatePath());
			var found = nanMarkers.Where(m => m.IsMatch(svgContent)).Select(m => m.ToString()).ToList();
			found.Should().BeEquivalentTo(new[] { "x=\"NaN\"", "y=\"NaN\"" },
				"l'artefact du repli porte les deux attributs de coordonnées non calculées du gabarit.");

			// Comptage (delta : xUnit parallélise les collections, une autre classe peut
			// appeler TryXsltSvgConversion en parallèle) — le mutant qui retire
			// l'incrément rend delta 0 → rouge.
			(FallacyMindMapDocumentConfig.XsltFallbackEngagements - engagementsBefore)
				.Should().BeGreaterOrEqualTo(1, "l'engagement explicite reste compté pour le résumé.");
			(FallacyMindMapDocumentConfig.XsltFallbackArtifacts - artifactsBefore)
				.Should().BeGreaterOrEqualTo(1, "l'artefact dégradé reste compté pour le résumé.");
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
