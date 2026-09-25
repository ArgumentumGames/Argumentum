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
    /// Argumentum_TarotCards (1 carte = ceil(175 lignes taxonomy / rscount 200) × NbCopies=7 →
    /// 14 des 379 pages) et dans le P&amp;P (×5 → 5 des 317 instances) — comptes post-#1288
    /// (176→175 cartes, 381→379 pages, 318→317 instances). Le trou de l'organe CSV
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

        /// <summary>Les 4 documents Print &amp; Play du périmètre — sous-ensemble STRICT de
        /// <see cref="PerimeterDocuments"/> (l'appartenance est assertée à l'exécution : les deux
        /// listes ne peuvent pas diverger en silence). Tous ont <c>NoBack</c> non posé (= false) :
        /// chaque planche émet un dos puis ses faces.</summary>
        private static readonly string[] PnpDocuments =
        {
            "Argumentum_TarotCards_Print&Play_A4_fr.pdf",
            "Argumentum_PokerCards_Print&Play_A4_fr.pdf",
            "Argumentum_TarotCards_Print&Play_Light_A4_fr.pdf",
            "Argumentum_PokerCards_Print&Play_Light_A4_fr.pdf",
        };

        /// <summary>
        /// #1536 p.2 — LE TÉMOIN AU RENDU. p.1 (#1544) a corrigé <c>PrintAndPlayDocument</c> et l'a
        /// prouvé sur la FONCTION PURE (<c>PrintAndPlayRectoVersoParityContractTests</c> : séquence
        /// Back·Front alternée, Tarot P&amp;P 106 pages). Ce Fact-ci ferme la dernière maille : il
        /// ouvre les PDF RÉELLEMENT PRODUITS et exige que leur nombre de pages soit PAIR.
        ///
        /// ⚠️ Pourquoi la parité est le prédicat qui compte : l'attente de pages du P&amp;P est
        /// dérivée en APPELANT <c>EmittedPageSequence</c> — la fonction qui portait le défaut. Avant
        /// #1536, dérivation et rendu s'accordaient sur le même nombre faux (105, impair) : le
        /// contrôle d'égalité #1187 était donc VERT sur le défaut, et c'est précisément pour ça
        /// qu'il a survécu. La parité ne partage aucun code avec le suspect — un document à dos dont
        /// une page n'a pas de verso est impair, quelle que soit la dérivation. L'égalité est
        /// néanmoins restatée ici, pour qu'aucun compte ne soit lu sans son attente en face.
        ///
        /// Expected-failure baseline (#1046 : une garde jamais vue rouge est un no-op) : sur un
        /// bundle produit AVANT le merge de #1544, ce Fact DOIT être rouge sur les 8 langues avec
        /// « produced 105 page(s) — ODD count ». Le voir vert partout ne prouve pas l'absence du
        /// défaut : vérifiez d'abord que le bundle est postérieur au fix.
        /// </summary>
        [Fact]
        public void Pnp_Pdfs_Page_Counts_Are_Even_And_Match_The_Emitted_Sequence_All_Languages()
        {
            var root = ResolveBundleRootOrFail();
            var config = new AssetConverterConfig();

            foreach (var docName in PnpDocuments)
                Assert.True(PerimeterDocuments.Contains(docName),
                    $"'{docName}' est dans PnpDocuments mais pas dans PerimeterDocuments — les deux listes ont divergé, et ce Fact mesurerait un document que l'organe #1187 ne couvre pas.");

            // Précondition : la parité n'est exigée QUE des documents à dos. Un de ces 4 passé à
            // NoBack=true rendrait la garde VACUITAIRE (toute séquence de faces est valide) — on
            // échoue donc en le nommant, plutôt que de rendre un vert qui ne mesure plus rien.
            var plans = new List<(string DocName, int ExpectedPages)>();
            foreach (var docName in PnpDocuments)
            {
                var doc = config.WebBasedGeneratorConfig.CardSetDocuments
                    .FirstOrDefault(d => d.DocumentName == docName && d.Enabled)
                    ?? throw new InvalidOperationException(
                        $"PnpDocuments: document '{docName}' absent ou désactivé dans la factory config — le périmètre déclaré et la config ont divergé, mettez à jour l'un ou l'autre (voir le message de Assert.Fail de Deck_Pdfs_* pour le même contrôle).");
                if (doc.NoBack)
                    Assert.Fail($"{docName} porte NoBack=true : ce document n'émet plus de dos, la garde de parité #1536 ne le couvre donc PLUS. Retirez-le de PnpDocuments en le déclarant, jamais en silence.");
                var plan = PdfDeckCountDerivation.Derive(config, doc, TestRepoRoot.Find());
                plans.Add((docName, plan.ExpectedPages));
            }

            _output.WriteLine("# Attentes dérivées (config × CSV × template, invariantes par langue)");
            foreach (var (docName, expectedPages) in plans)
                _output.WriteLine($"  {docName}: {expectedPages} pages attendues (EmittedPageSequence : chaque planche émet Back+Front)");
            _output.WriteLine("  NOBACK : les 4 documents ci-dessus ont NoBack=false — vérifié à l'exécution (un document à faces seules n'a pas de verso à exiger).");
            _output.WriteLine("");

            var records = new List<(string Doc, string Lang, int ActualPages, int ExpectedPages)>();
            var missingDirs = new List<string>();

            foreach (var lang in Languages)
            {
                var dir = Path.Combine(root, lang, "Documents", "density-0");
                if (!Directory.Exists(dir))
                {
                    missingDirs.Add(lang);
                    continue;
                }
                foreach (var (docName, expectedPages) in plans)
                {
                    var localizedName = CardSetLocalization.GetLocalizedFileName(docName, "fr", lang);
                    var pdfPath = Path.Combine(dir, localizedName);
                    if (!File.Exists(pdfPath))
                    {
                        // Un PDF absent ne peut pas être mesuré : on l'enregistre à 0 plutôt que de
                        // sauter la ligne en silence. ⚠️ 0 est PAIR — c'est le contrôle d'ÉGALITÉ
                        // (0 ≠ attendu) qui le nomme, pas la parité, qui ne peut rien dire d'un
                        // document qu'elle n'a pas ouvert (cf. la leçon « un zéro n'est une absence
                        // que si l'instrument pouvait voir un un »).
                        records.Add((docName, lang, 0, expectedPages));
                        continue;
                    }
                    using var pdf = PdfDocument.Open(pdfPath);
                    records.Add((docName, lang, pdf.NumberOfPages, expectedPages));
                }
            }

            if (missingDirs.Count > 0)
                Assert.Fail($"Missing language document directories: {string.Join(", ", missingDirs)} — Target/ existe mais ces langues n'ont pas produit Documents/density-0/ (ce Fact n'a rien vérifié pour elles).");

            if (records.Count == 0)
                Assert.Fail($"Aucun PDF P&P trouvé sous {root} — ce Fact n'a rien vérifié (layout attendu : {{lang}}/Documents/density-0/*Print&Play*.pdf).");

            _output.WriteLine("# Mesures (langue × document : pages produites / attendues — la parité est calculée sur la PRODUITE)");
            foreach (var lang in Languages)
            {
                var rows = records.Where(r => r.Lang == lang);
                _output.WriteLine($"  {lang}: " + string.Join("  ", rows.Select(r =>
                    $"{Path.GetFileNameWithoutExtension(CardSetLocalization.GetLocalizedFileName(r.Doc, "fr", lang))}={r.ActualPages}/{r.ExpectedPages}")));
            }

            // (1) LE prédicat indépendant — la parité de la mesure produite.
            var parityFailures = PdfCardCountIntegrity.CheckPnpRectoVersoParity(
                records.Select(r => (r.Doc, r.Lang, r.ActualPages)));

            // (2) L'égalité, restatée pour qu'aucun compte ne soit lu sans son attente en face.
            var countFailures = PdfCardCountIntegrity.CheckPageCounts(
                records.Select(r => (r.Doc, r.Lang, r.ExpectedPages, r.ActualPages,
                    "P&P : attente dérivée par PrintAndPlayDocument.EmittedPageSequence (chaque planche émet Back+Front — page de dos blanche pour une planche sans dos, #1536)")));

            Assert.True(parityFailures.Count == 0 && countFailures.Count == 0,
                $"#1536 p.2 — parité recto-verso des PDF P&P PRODUITS ({records.Count} mesures : {PnpDocuments.Length} documents × {Languages.Length} langues).\n" +
                $"  parité (prédicat indépendant de la dérivation) : {parityFailures.Count} violation(s)\n" +
                (parityFailures.Count > 0 ? "    " + string.Join("\n    ", parityFailures) + "\n" : "") +
                $"  égalité (pages produites ≠ attente dérivée) : {countFailures.Count} violation(s)\n" +
                (countFailures.Count > 0 ? "    " + string.Join("\n    ", countFailures) : ""));

            _output.WriteLine($"PASS: {records.Count} PDF P&P — tous PAIRS, et chaque compte égal à sa séquence d'émission dérivée.");
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
