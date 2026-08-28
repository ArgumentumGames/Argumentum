using Argumentum.AssetConverter.Tests;             // TestRepoRoot (linked compile)
using Argumentum.AssetConverter.Tests.PdfAssembly; // #1187 organ (linked compile)
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
    /// #1187 organ, TARGET side: compares the page count of every PRODUCED deck PDF against the
    /// expectation derived from the authored chain (config × CSV × template) by
    /// <see cref="PdfDeckCountDerivation"/> — the same derivation whose five baselines are pinned
    /// in CI (<c>PdfDeckCountContractTests</c>: TarotCards 381 · TarotCards_Virtues 262 ·
    /// PokerCards 334 · PokerCards_P&amp;P_A4 38 · TarotCards_P&amp;P_A4 105, measured on the
    /// v0.9.0-review bundle). This is the check the #1121/#1176 structural guards cannot express:
    /// parity says all languages AGREE, profile says 10 files exist — neither says the deck has
    /// the RIGHT number of cards. #1204 (Scenarii cru à 97 cartes → devis faux) agrees in 8
    /// languages and counts as 1 file in each: it traverses both.
    ///
    /// ⚠ MÉMO — DÉCLARATION EXPLICITE (demandée par le dispatch #1187 : « Dis explicitement
    /// comment ton organe traite Memo côté PDF — soit tu le couvres, soit tu déclares le trou ») :
    /// cet organe COUVRE Memo au niveau PDF. Memo n'a pas de PDF autonome — il vit DANS
    /// Argumentum_TarotCards (1 carte = ceil(176 lignes taxonomy / rscount 200) × NbCopies=7 →
    /// 14 des 381 pages) et dans le P&amp;P (×5 → 5 des 318 instances). Le trou de l'organe CSV
    /// (#1212, Skip honnête « template-internal ») est donc fermé ICI : si le rscount du template
    /// Memo, le filtre carte∈{1,2} ou le NbCopies du document dérive, la dérivation change et ce
    /// test échoue en nommant la ligne Memo de la traçabilité. Pas de silence, pas de trou.
    ///
    /// Périmètre EXCLU, déclaré : les documents Web (A4/A0/Thumbnails) et les variantes désactivées
    /// (TarotCards_2/_3). Les documents Web sont dérivables par le même moteur (PrintAndPlay +
    /// NoBack) mais leur priorité #1187 est post-BAT — ils ne sont PAS vérifiés ici, et ce test
    /// ne doit pas être cité comme les couvrant.
    ///
    /// Expected-failure baseline (#1046 : une garde jamais vue rouge est un no-op) : sur un bundle
    /// régénéré depuis un CSV amputé (p.ex. Scenarii retombé à 97 lignes, #1204), ce test DOIT
    /// échouer sur les 8 langues avec « produced 194 page(s), derived expectation 334 ». Le sens
    /// rouge du vérificateur lui-même est prouvé en CI par les témoins de
    /// <c>PdfDeckCountContractTests.Witness_*</c>.
    ///
    /// Fail-loud si aucun bundle n'existe (#957 residu ii) : ce test ne passe JAMAIS vert sans
    /// avoir ouvert des PDF. VisualTests n'est pas exécuté par la CI (#1048) — il tourne sur les
    /// machines de régénération (po-2023 / ai-01) via dotnet test sur ce projet.
    /// </summary>
    public class PdfCardCountIntegrityTests : IDisposable
    {
        private readonly ITestOutputHelper _output;

        /// <summary>Bundle racine explicite — même variable d'environnement que
        /// PdfBundleIntegrityTests : interface opérateur unique pour cibler un bundle précis.</summary>
        private const string BundleRootEnvVar = "ARGUMENTUM_PDF_BUNDLE_ROOT";

        private static readonly string TargetRoot = Path.Combine(
            TestRepoRoot.Find(),
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Debug", "net9.0-windows", "Target");

        private static readonly string ReleaseTargetRoot = Path.Combine(
            TestRepoRoot.Find(),
            "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Release", "net9.0-windows", "Target");

        /// <summary>8 langues — périmètre identique à PdfBundleIntegrityTests.</summary>
        private static readonly string[] Languages =
            { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

        /// <summary>Documents du périmètre boîte + P&amp;P (les 7 documents activés hors Web).
        /// Les noms sont les DocumentName de la factory config (suffixe _fr) ; le nom localisé
        /// par langue est produit par le MÉCANISME DE PRODUCTION CardSetLocalization.GetLocalizedFileName
        /// — jamais ré-implémenté ici.</summary>
        private static readonly string[] PerimeterDocuments =
        {
            "Argumentum_TarotCards_fr.pdf",
            "Argumentum_TarotCards_Virtues_fr.pdf",
            "Argumentum_PokerCards_fr.pdf",
            "Argumentum_TarotCards_Print&Play_A4_fr.pdf",
            "Argumentum_PokerCards_Print&Play_A4_fr.pdf",
            "Argumentum_TarotCards_Print&Play_Light_A4_fr.pdf",
            "Argumentum_PokerCards_Print&Play_Light_A4_fr.pdf",
        };

        public PdfCardCountIntegrityTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Dispose() { }

        [Fact]
        public void Deck_Pdfs_Page_Counts_Match_Derived_Expectations_All_Languages()
        {
            var root = ResolveBundleRootOrFail();

            // La dérivation lit les sources FR (config + CSV + templates) : l'attente de pages est
            // IDENTIQUE pour les 8 langues — le contenu se traduit, la structure du deck non.
            // Dérivée une fois par document, devant chaque mesure (#1187 : jamais un compte produit
            // sans sa valeur attendue en face).
            var expectations = new Dictionary<string, DocumentPlan>(StringComparer.Ordinal);
            var config = new AssetConverterConfig();
            foreach (var docName in PerimeterDocuments)
            {
                var doc = config.WebBasedGeneratorConfig.CardSetDocuments
                    .FirstOrDefault(d => d.DocumentName == docName && d.Enabled)
                    ?? throw new InvalidOperationException(
                        $"PdfCardCountIntegrityTests: document '{docName}' absent ou désactivé dans la factory config — le périmètre déclaré et la config ont divergé, mettez à jour l'un ou l'autre.");
                expectations[docName] = PdfDeckCountDerivation.Derive(config, doc, TestRepoRoot.Find());
            }

            _output.WriteLine("# Attentes dérivées (config × CSV × template, invariantes par langue)");
            foreach (var (docName, plan) in expectations)
                _output.WriteLine($"  {docName}: {plan.ExpectedPages} pages — {plan.Breakdown}");
            _output.WriteLine("  MÉMO : couvert — 1 carte × NbCopies 7 (TarotCards) / ×5 (P&P), voir la ligne Memo de la traçabilité ci-dessus.");
            _output.WriteLine("  EXCLUS (déclarés) : documents Web A4/A0/Thumbnails (priorité #1187 post-BAT), TarotCards_2/_3 (désactivés).");
            _output.WriteLine("");

            var records = new List<(string Doc, string Lang, int ExpectedPages, int ActualPages, string Breakdown)>();
            var missingDirs = new List<string>();

            foreach (var lang in Languages)
            {
                var dir = Path.Combine(root, lang, "Documents", "density-0");
                if (!Directory.Exists(dir))
                {
                    missingDirs.Add(lang);
                    continue;
                }
                foreach (var docName in PerimeterDocuments)
                {
                    var plan = expectations[docName];
                    // Nom localisé par le mécanisme de production (ImageFileGenerator.cs:63).
                    var localizedName = CardSetLocalization.GetLocalizedFileName(docName, "fr", lang);
                    var pdfPath = Path.Combine(dir, localizedName);
                    if (!File.Exists(pdfPath))
                    {
                        records.Add((docName, lang, plan.ExpectedPages, 0,
                            $"{plan.Breakdown} — FICHIER ABSENT : '{pdfPath}' (une attente sans PDF produit est la borne basse violée au maximum)."));
                        continue;
                    }
                    try
                    {
                        using var pdf = PdfDocument.Open(pdfPath);
                        records.Add((docName, lang, plan.ExpectedPages, pdf.NumberOfPages, plan.Breakdown));
                    }
                    catch (Exception ex)
                    {
                        records.Add((docName, lang, plan.ExpectedPages, 0,
                            $"{plan.Breakdown} — PDF ILLISIBLE : {ex.GetType().Name}: {ex.Message.Split('\n')[0]}"));
                    }
                }
            }

            if (missingDirs.Count > 0)
                Assert.Fail($"Missing language document directories: {string.Join(", ", missingDirs)} — Target/ existe mais ces langues n'ont pas produit Documents/density-0/ (ce test n'a rien vérifié pour elles ; vérifiez la sortie du pipeline).");

            if (records.Count == 0)
                Assert.Fail($"Aucun PDF du périmètre trouvé sous {root} — ce test n'a rien vérifié (layout attendu : {{lang}}/Documents/density-0/*.pdf).");

            var failures = PdfCardCountIntegrity.CheckPageCounts(records);

            // Table de diagnostic complète : la mesure, avec son attente DERRIÈRE chaque nombre.
            _output.WriteLine("# Mesures (langue × document : pages produites / attendues)");
            var docOrder = PerimeterDocuments.ToList();
            foreach (var lang in Languages)
            {
                var rows = records.Where(r => r.Lang == lang).OrderBy(r => docOrder.IndexOf(r.Doc));
                _output.WriteLine($"  {lang}: " + string.Join("  ", rows.Select(r =>
                    $"{Path.GetFileNameWithoutExtension(CardSetLocalization.GetLocalizedFileName(r.Doc, "fr", lang))}={r.ActualPages}/{r.ExpectedPages}")));
            }

            if (failures.Count > 0)
                Assert.Fail(
                    $"#1187 PDF card-count organ — pages produites ≠ attentes dérivées (borne basse = aucune planche manquante, borne haute = aucune planche en trop), {failures.Count} violation(s) :\n  " +
                    string.Join("\n  ", failures));

            _output.WriteLine($"PASS: {records.Count} PDF ({PerimeterDocuments.Length} documents × {Languages.Length} langues) — chaque compte de pages égal à son attente dérivée, bornes respectées dans les deux sens.");
        }

        /// <summary>Bundle racine : la variable d'environnement si définie (doit exister — échec
        /// franc, jamais de repli silencieux sur Target/), sinon l'arbre Target généré (Release
        /// préféré, sinon Debug), sinon échec franc.</summary>
        private static string ResolveBundleRootOrFail()
        {
            var envRoot = Environment.GetEnvironmentVariable(BundleRootEnvVar);
            if (!string.IsNullOrWhiteSpace(envRoot))
            {
                if (!Directory.Exists(envRoot))
                    Assert.Fail($"{BundleRootEnvVar} est défini à '{envRoot}' qui n'existe pas — un bundle explicitement demandé échoue franc, sans repli ni vert (#1176 no-silent-skip).");
                return envRoot;
            }
            if (Directory.Exists(ReleaseTargetRoot)) return ReleaseTargetRoot;
            if (Directory.Exists(TargetRoot)) return TargetRoot;
            Assert.Fail("PdfCardCountIntegrityTests exige un bundle généré — lancez le pipeline d'abord (bin/.../Target introuvable). Ce test n'a rien vérifié.");
            throw new InvalidOperationException("unreachable"); // Assert.Fail ne revient pas, mais le compilateur veut un retour.
        }
    }
}
