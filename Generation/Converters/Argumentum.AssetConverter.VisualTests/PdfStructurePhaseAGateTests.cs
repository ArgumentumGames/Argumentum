using Argumentum.AssetConverter.Tests;                 // TestRepoRoot (linked compile)
using Argumentum.AssetConverter.Tests.PdfAssembly;     // #1187 derivation + #1460 phase A checker (linked compile)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests
{
    /// <summary>
    /// #1460 <b>phase A</b> — l'organe : les PDF <b>produits</b> ont-ils le format annoncé, et le
    /// compte de pages <b>dérivé</b> s'y matérialise-t-il ?
    ///
    /// <para><b>Pourquoi ici et pas en CI.</b> Il faut ouvrir des PDF : <c>PdfPig</c> n'est référencé
    /// que par ce projet, et la CI n'a aucun arbre <c>Target/</c>. Le <b>contrôle inverse</b> pur vit
    /// donc dans la suite CI (<c>PdfStructurePhaseAContractTests</c>, 8 témoins sans artefact), et
    /// cet organe-ci ne fait qu'appliquer le même vérificateur à de vrais fichiers.</para>
    ///
    /// <para><b>La garde d'arbre est la précondition, pas une politesse.</b> Famille « regen sans
    /// clobber = stale trap » : un arbre antérieur au corpus rendrait un vert qui ne mesure rien.
    /// L'arbre local du 05/11 en est l'exemple vivant — 64 PDF, datés du 15/08, corpus du 31/10.
    /// ⇒ quand l'arbre n'est pas MESURABLE, l'organe est <b>ROUGE</b> et le dit, avec la commande
    /// qui rend le verdict (<c>tools/1460-structure-charpente-pilot.py</c>). ⛔ Pas de skip : un
    /// skip reproduirait exactement le défaut que la garde ferme (#909 : une étape de test qui ne
    /// lançait rien et restait verte).</para>
    ///
    /// <para><b>Le nominal d'une page « carte » est dérivable de la config — et l'organe l'asserte.</b>
    /// (Correction du 05/11 au soir : la première mouture de ce commentaire affirmait le contraire —
    /// erreur d'analyse, voir §3.1 du doc phase A.) <c>PdfManager</c> écrit bien ces PDF par
    /// Magick.NET à la taille <b>physique du PNG</b>, mais c'est la <b>config qui fixe cette
    /// taille</b> : <c>ImageHelper.cs:170-185</c> force 300 dpi puis
    /// <c>ResizeInMM(FrontCards/BackCards.WidthMM, HeigthMM)</c> sur chaque PNG de carte avant
    /// l'écriture. Preuve à l'appui : le Poker produit mesure <b>exactement</b> son nominal de
    /// config (63,5 × 88,9 mm). L'organe asserte donc le nominal dérivable (config ± la tolérance
    /// pointeur), en plus de l'uniformité et de l'égalité inter-langues ; la bande de plausibilité
    /// reste en garde-fou pour un document carte <i>sans</i> nominal de config. ⛔ Aucun chiffre
    /// n'est recopié : l'attendu se lit dans la config au moment du run (#1187).</para>
    ///
    /// <para><b>Le mesure du 05/11 (Tarot 59,9 × 112,9 mm) était l'âge de l'arbre, pas un défaut de
    /// dérivation.</b> #1250 (02/09) a changé le nominal Tarot (front ET back, toutes langues) ;
    /// l'arbre local daté du 15/08 est <b>antérieur</b> et porte l'ancien nominal. La garde d'arbre
    /// le refuse déjà (STALE) ; le témoin CI correspondant vérifie que <c>Check</c> lui-même
    /// nomme l'écart ancien/nouveau nominal. La première validation réelle de l'assertion reste la
    /// prochaine régénération fraîche.</para>
    /// </summary>
    public class PdfStructurePhaseAGateTests
    {
        private readonly ITestOutputHelper _output;

        /// <summary>Même interface opérateur que <c>PdfCardCountIntegrityTests</c> : une seule
        /// variable d'environnement pour cibler un bundle précis.</summary>
        private const string BundleRootEnvVar = "ARGUMENTUM_PDF_BUNDLE_ROOT";

        private static readonly string RepoRoot = TestRepoRoot.Find();

        private static readonly string TargetDebug = Path.Combine(RepoRoot,
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Debug", "net9.0-windows", "Target");

        private static readonly string TargetRelease = Path.Combine(RepoRoot,
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Release", "net9.0-windows", "Target");

        /// <summary>8 langues — périmètre identique à <c>PdfBundleIntegrityTests</c> et
        /// <c>PdfCardCountIntegrityTests</c>.</summary>
        private static readonly string[] Languages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

        /// <summary>
        /// Bande de plausibilité d'une page « carte », en mm. Elle ne prétend pas être la taille
        /// voulue — elle <b>exclut les formats qui ne sont pas des cartes</b> : une page A4
        /// (210 × 297) ou A0 (841 × 1189) tombe hors bande, et une page dimensionnée en pixels
        /// à 96 dpi (≈ 264 × 454 mm pour une carte de 70 × 120 mm) aussi. C'est le seul prédicat
        /// de taille qu'on puisse opposer à une page carte sans recopier un nominal.
        /// </summary>
        private const double CardMinWidthMm = 40, CardMaxWidthMm = 100;
        private const double CardMinHeightMm = 60, CardMaxHeightMm = 160;

        public PdfStructurePhaseAGateTests(ITestOutputHelper output) => _output = output;

        [Fact]
        public void Produced_Pdfs_Meet_The_Announced_Format_And_The_Derived_Page_Counts()
        {
            var root = ResolveBundleRoot();
            var config = new AssetConverterConfig();
            var docs = config.WebBasedGeneratorConfig.CardSetDocuments
                .Where(d => d.Enabled)
                .ToList();

            _output.WriteLine($"# Arbre mesuré : {root}");

            // ── 0. LA GARDE D'ARBRE — précondition, pas politesse.
            var corpusNewest = NewestFile(Path.Combine(RepoRoot, "Cards"), "*.csv", skipArchive: true);
            var treeNewest = NewestFile(root, "*", skipArchive: false);
            var pdfPaths = Directory.EnumerateFiles(root, "*.pdf", SearchOption.AllDirectories).ToList();

            var expected = new List<(CardSetDocumentConfig Doc, string DocName, string Lang, string Path)>();
            foreach (var lang in Languages)
                foreach (var doc in docs)
                    expected.Add((doc, doc.DocumentName, lang,
                        System.IO.Path.Combine(root, lang, "Documents", "density-0",
                            CardSetLocalization.GetLocalizedFileName(doc.DocumentName, "fr", lang))));

            var missing = expected.Count(e => !File.Exists(e.Path));
            var (verdict, motif) = PdfPageGeometryIntegrity.JudgeTree(
                treeExists: Directory.Exists(root), pdfCount: pdfPaths.Count,
                corpusNewest: corpusNewest, treeNewest: treeNewest, missingExpected: missing);

            _output.WriteLine($"# Garde d'arbre : {verdict} — {motif}");
            _output.WriteLine($"# Corpus le plus récent : {corpusNewest:yyyy-MM-dd HH:mm} · arbre : {treeNewest:yyyy-MM-dd HH:mm}");
            _output.WriteLine($"# PDF présents : {pdfPaths.Count} · attendus (documents activés × 8 langues) : {expected.Count}");

            if (verdict == TreeVerdict.NotMeasurable)
            {
                Assert.Fail(
                    $"#1460 phase A — NON MESURABLE : {motif}.\n" +
                    $"  Arbre mesuré : {root}\n" +
                    $"  Cette assertion est ROUGE et non un skip : un arbre absent, vide ou antérieur au corpus ne peut pas porter " +
                    $"l'état courant du corpus, et rendre vert ici reproduirait le défaut que la garde ferme (#909).\n" +
                    $"  Regardez la garde indépendante : python tools/1460-structure-charpente-pilot.py --repo \"{RepoRoot}\"\n" +
                    $"  Régénérez (clobber) avant de relancer cet organe — Debug et Release sont des arbres INDÉPENDANTS.");
            }

            if (verdict == TreeVerdict.Divergent)
            {
                var absent = expected.Where(e => !File.Exists(e.Path)).Select(e => $"{e.Lang}/{e.DocName}").ToList();
                Assert.Fail(
                    $"#1460 phase A — DIVERGENT : {motif}.\n" +
                    $"  {absent.Count} PDF attendus absents (documents activés de la config) :\n    " +
                    string.Join("\n    ", absent.Take(20)) + (absent.Count > 20 ? $"\n    … et {absent.Count - 20} autres" : "") +
                    "\n  Un arbre frais qui ignore un document activé n'est ni conforme ni vide : c'est un inventaire qui contredit la config.");
            }

            // ── 1. GÉOMÉTRIE : le format déclaré (A4/A0) se lit dans le PDF ; une page carte est
            //       uniforme, de la même taille dans les 8 langues, et dans la bande des cartes.
            var geometryRows = new List<PdfPageGeometryIntegrity.SizeExpectation>();
            var cardMeasurements = new List<(string Doc, string Lang, double WidthMm, double HeightMm, bool HasConfigNominal, double ConfigWidthMm, double ConfigHeightMm)>();
            var counts = new List<(string Doc, string Lang, int Expected, int Actual, string Breakdown)>();
            var unreadable = new List<string>();

            foreach (var (doc, docName, lang, path) in expected)
            {
                DocumentPlan plan;
                try
                {
                    plan = PdfDeckCountDerivation.Derive(config, doc, RepoRoot);
                }
                catch (Exception ex)
                {
                    unreadable.Add($"{lang}/{docName} — DÉRIVATION IMPOSSIBLE : {ex.GetType().Name}: {ex.Message.Split('\n')[0]}");
                    continue;
                }

                int pages;
                double widthPt, heightPt;
                int distinctSizes;
                try
                {
                    using var pdf = PdfDocument.Open(path);
                    pages = pdf.NumberOfPages;
                    // Borne NOMMÉE : première, médiane et dernière page — l'intérieur n'est pas balayé
                    // (un PDF de 381 pages coûte des secondes par page ; le défaut visé — une autre
                    // PageSize — se voit dès la première).
                    var sample = new[] { 1, Math.Max(1, pages / 2), pages }.Distinct().ToArray();
                    var sizes = sample.Select(i => (W: pdf.GetPage(i).Width, H: pdf.GetPage(i).Height)).ToList();
                    distinctSizes = sizes.Select(s => (Math.Round(s.W, 2), Math.Round(s.H, 2))).Distinct().Count();
                    (widthPt, heightPt) = (sizes[0].W, sizes[0].H);
                }
                catch (Exception ex)
                {
                    unreadable.Add($"{lang}/{docName} — PDF ILLISIBLE : {ex.GetType().Name}: {ex.Message.Split('\n')[0]}");
                    continue;
                }

                counts.Add((docName, lang, plan.ExpectedPages, pages, plan.Breakdown));

                if (doc.DocumentFormat == CardDocumentFormat.PrintAndPlay)
                {
                    var format = PdfPageGeometryIntegrity.Declared(doc.PageSize);
                    geometryRows.Add(new PdfPageGeometryIntegrity.SizeExpectation(
                        docName, lang, format.WidthMm, format.HeightMm,
                        $"format déclaré {format.Id} ({format.Origin})", widthPt, heightPt, distinctSizes));
                }
                else
                {
                    var front = doc.CardSets.Count > 0 ? doc.CardSets[0].FrontCards : null;
                    var configWidth = front == null ? 0d : (double)front.WidthMM;
                    var configHeight = front == null ? 0d : (double)front.HeigthMM;
                    var hasNominal = configWidth > 0 && configHeight > 0;
                    cardMeasurements.Add((docName, lang, widthPt * PdfPageGeometryIntegrity.MmPerPoint,
                        heightPt * PdfPageGeometryIntegrity.MmPerPoint, hasNominal, configWidth, configHeight));

                    // Le nominal EST dérivable : ImageHelper redimensionne chaque PNG de carte à
                    // WidthMM × HeigthMM à 300 dpi (ResizeInMM, ImageHelper.cs:170-185) et Magick
                    // pose la page à la taille physique du PNG — preuve : le Poker produit mesure
                    // exactement son nominal. On asserte donc le nominal de config (± tolérance) ;
                    // la self-expectation ne subsiste que pour un document carte SANS nominal.
                    geometryRows.Add(hasNominal
                        ? new PdfPageGeometryIntegrity.SizeExpectation(
                            docName, lang, configWidth, configHeight,
                            $"nominal de config : {docName} FrontCards {configWidth} x {configHeight} mm — " +
                            "chaîne ImageHelper.ResizeInMM (300 dpi) → PNG → page Magick",
                            widthPt, heightPt, distinctSizes)
                        : new PdfPageGeometryIntegrity.SizeExpectation(
                            docName, lang,
                            widthPt * PdfPageGeometryIntegrity.MmPerPoint, heightPt * PdfPageGeometryIntegrity.MmPerPoint,
                            "aucun nominal de config (WidthMM/HeigthMM à 0) : self-expectation, " +
                            "uniformité/égalité inter-langues/bande seulement",
                            widthPt, heightPt, distinctSizes));
                }
            }

            // 1a. Uniformité + format déclaré (le Check ne compare que ce qui a un attendu).
            var geometryFailures = PdfPageGeometryIntegrity.Check(geometryRows).ToList();

            // 1b. Pages cartes : bande de plausibilité + égalité STRICTE entre les 8 langues.
            foreach (var m in cardMeasurements)
            {
                if (m.WidthMm < CardMinWidthMm || m.WidthMm > CardMaxWidthMm ||
                    m.HeightMm < CardMinHeightMm || m.HeightMm > CardMaxHeightMm)
                {
                    geometryFailures.Add(
                        $"{m.Lang}/{m.Doc}: page carte {m.WidthMm:F1} x {m.HeightMm:F1} mm — hors de la bande carte " +
                        $"[{CardMinWidthMm}-{CardMaxWidthMm}] x [{CardMinHeightMm}-{CardMaxHeightMm}] mm. " +
                        "Une page de deck qui prend un format de planche (A4/A0) ou une taille en pixels signale que " +
                        "la composition ou la densité de récolte a changé (#1460 phase A).");
                }
            }

            foreach (var byDoc in cardMeasurements.GroupBy(m => m.Doc))
            {
                var shapes = byDoc.Select(m => (Math.Round(m.WidthMm, 1), Math.Round(m.HeightMm, 1))).Distinct().ToList();
                if (shapes.Count > 1)
                {
                    geometryFailures.Add(
                        $"{byDoc.Key}: {shapes.Count} tailles de page DIFFÉRENTES selon la langue " +
                        $"({string.Join(", ", shapes.Select(s => $"{s.Item1:F1}x{s.Item2:F1}"))} mm). " +
                        "La traduction ne change pas la géométrie d'une carte : un écart inter-langues signale une récolte " +
                        "à une densité différente pour cette langue.");
                }
            }

            // 1c. Le compte DÉRIVÉ se matérialise — réutilise l'organe #1187, il ne le duplique pas.
            var countFailures = PdfCardCountIntegrity.CheckPageCounts(
                counts.Select(c => (c.Doc, c.Lang, c.Expected, c.Actual, c.Breakdown)));

            // ── 2. Sortie de diagnostic : la mesure, avec son attendu derrière chaque nombre.
            _output.WriteLine("# Géométrie");
            foreach (var g in geometryRows.OrderBy(g => g.Doc).ThenBy(g => g.Lang))
                _output.WriteLine($"  {g.Lang}/{g.Doc}: {g.ActualWidthPt:F2} x {g.ActualHeightPt:F2} pt");
            if (cardMeasurements.Count > 0)
            {
                _output.WriteLine("# Pages cartes — nominal de config ASSERTÉ (chaîne ResizeInMM → PNG → page), pas seulement rapporté");
                foreach (var m in cardMeasurements.Where(m => m.Lang == "fr"))
                    _output.WriteLine(m.HasConfigNominal
                        ? $"  {m.Doc}: produit {m.WidthMm:F1} x {m.HeightMm:F1} mm · nominal de config {m.ConfigWidthMm:F1} x {m.ConfigHeightMm:F1} mm (asserté ±1 mm)"
                        : $"  {m.Doc}: produit {m.WidthMm:F1} x {m.HeightMm:F1} mm · aucun nominal de config (bande + égalité inter-langues seulement)");
            }

            if (unreadable.Count > 0)
                Assert.Fail("#1460 phase A — fichiers non mesurables :\n  " + string.Join("\n  ", unreadable));

            if (geometryFailures.Count > 0 || countFailures.Count > 0)
            {
                Assert.Fail(
                    $"#1460 phase A — {expected.Count} PDF mesurés ({docs.Count} documents activés × {Languages.Length} langues).\n" +
                    $"  géométrie : {geometryFailures.Count} violation(s)\n" +
                    (geometryFailures.Count > 0 ? "    " + string.Join("\n    ", geometryFailures) + "\n" : "") +
                    $"  comptes de pages (attente dérivée config × CSV × gabarit, organe #1187) : {countFailures.Count} violation(s)\n" +
                    (countFailures.Count > 0 ? "    " + string.Join("\n    ", countFailures) : ""));
            }

            _output.WriteLine($"PASS: {expected.Count} PDF — format uniforme et conforme, et chaque compte de pages égal à sa dérivation.");
        }

        /// <summary>Fichier le plus récent sous <paramref name="root"/> (par date de dernière
        /// écriture). <c>Archive/</c> est exclu pour le corpus : ce sont des instantanés historiques,
        /// ils datent le passé, pas le présent (même règle que le pilote).</summary>
        private static DateTime NewestFile(string root, string pattern, bool skipArchive)
        {
            if (!Directory.Exists(root)) return DateTime.MinValue;
            var newest = DateTime.MinValue;
            foreach (var file in Directory.EnumerateFiles(root, pattern, SearchOption.AllDirectories))
            {
                if (skipArchive && file.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                        .Any(seg => string.Equals(seg, "Archive", StringComparison.OrdinalIgnoreCase)))
                    continue;
                var written = File.GetLastWriteTime(file);
                if (written > newest) newest = written;
            }
            return newest;
        }

        /// <summary>Racine du bundle : la variable d'environnement si définie (échec franc si elle
        /// n'existe pas), sinon Release puis Debug — même résolution que
        /// <c>PdfCardCountIntegrityTests</c>. ⛔ Aucun repli silencieux vers un arbre vide.</summary>
        private static string ResolveBundleRoot()
        {
            var envRoot = Environment.GetEnvironmentVariable(BundleRootEnvVar);
            if (!string.IsNullOrWhiteSpace(envRoot))
            {
                if (!Directory.Exists(envRoot))
                    Assert.Fail($"{BundleRootEnvVar} pointe sur '{envRoot}', qui n'existe pas — un bundle explicitement demandé échoue franc, sans repli ni vert.");
                return envRoot;
            }
            if (Directory.Exists(TargetRelease)) return TargetRelease;
            if (Directory.Exists(TargetDebug)) return TargetDebug;
            Assert.Fail("PdfStructurePhaseAGateTests exige un arbre généré — lancez le pipeline d'abord (bin/.../Target introuvable). Cette assertion n'a rien vérifié.");
            throw new InvalidOperationException("unreachable");
        }
    }
}
