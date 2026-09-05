using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    /// <summary>
    /// Organe de comptage #1187, niveau 2 : <b>pages des PDF produits dérivées de la chaîne
    /// config → CSV → template</b> — le maillon manquant entre l'organe CSV-source
    /// (<c>CardSetExpectedCardCountContractTests</c>, #1212 : Rules 15, Scenarii 167…), la parité
    /// (<c>PdfBundleIntegrity</c> : profil 8×10, écart médian) et le fail-loud zéro
    /// (<c>EmptyImagesFailLoudTests</c> : le couple document×langue à 0 image). Aucun des trois
    /// n'épingle les comptes AU NIVEAU PDF : un PDF qui perd 3 cartes sur 192 traverse les trois.
    /// C'est précisément la dérive de #1204 (Scenarii annoncé 97, réel 167 — devis derrière).
    ///
    /// Ce fichier prouve la dérivation et le vérificateur SANS bundle : les cinq nombres épinglés
    /// sont des pages MESURÉES sur le bundle v0.9.0-review (20-24/08, baselines #1176/#1175 :
    /// TarotCards 381 · TarotCards_Virtues 262 · PokerCards 334 · PokerCards_P&amp;P_A4 38 ·
    /// TarotCards_P&amp;P_A4 105), et l'organe doit les reproduire par dérivation pure. Toute dérive
    /// du CSV ou de la config fait donc passer CETTE suite rouge AVANT toute régénération — le
    /// coût exact du défaut #1204 était un devis émis sur un chiffre faux.
    ///
    /// ⚠ <b>#1288 (05/09/2026) — la baseline TarotCards est passée de 381 à 379 pages.</b> Décision
    /// owner : PK 96 sort du deck (vrai doublon de PK 108) ⇒ Fallacies 176 → <b>175</b> cartes,
    /// instances 198 → 197, pages 381 → 379. C'est le seul cas où un nombre de ce fichier bouge
    /// sans être un défaut : l'organe a fait exactement son travail (rouge AVANT régénération),
    /// et la baseline a été re-dérivée, pas contournée. Conséquence à ne pas perdre de vue :
    /// <b>la dérivation est désormais EN AVANCE sur le bundle du 20-24/08</b>, qui porte encore
    /// 381 pages et 176 cartes — ce bundle est périmé pour l'impression tant qu'il n'est pas
    /// régénéré. Les quatre autres baselines sont inchangées ; TarotCards_P&amp;P_A4 reste à 105
    /// pages bien que ses instances passent de 318 à 317 (la planche 53 n'était pas pleine).
    ///
    /// Le comparateur contre les PDF réels (PdfPig) vit dans VisualTests
    /// (<c>PdfCardCountIntegrityTests</c>) : il tourne sur les machines de régénération, pas en CI.
    ///
    /// ⚠ Mémo — le maillon que l'organe CSV couvrait d'un Skip honnête est ICI couvert :
    /// Memo = ceil(176 lignes taxonomy / rscount 200 du template) = 1 carte, × NbCopies=7 dans le
    /// document TarotCards (config Documents) = 14 pages. La divergence « Memo 1 » (organe CSV,
    /// niveau récolte) vs « ×7 copies » (matrice façonniers) est ainsi TRACÉE dans la dérivation :
    /// les deux chiffres décrivent deux niveaux différents, et l'organe nomme les deux.
    /// </summary>
    public class PdfDeckCountContractTests
    {
        private readonly ITestOutputHelper _output;
        public PdfDeckCountContractTests(ITestOutputHelper output) => _output = output;

        private static string RepoRoot => TestRepoRoot.Find();

        private static DocumentPlan DeriveByName(string documentName)
        {
            var config = new AssetConverterConfig();
            var doc = config.WebBasedGeneratorConfig.CardSetDocuments
                .FirstOrDefault(d => d.DocumentName == documentName && d.Enabled);
            doc.Should().NotBeNull($"document '{documentName}' must exist and be enabled in the factory config");
            return PdfDeckCountDerivation.Derive(config, doc!, RepoRoot);
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (0) TÉMOINS ROUGES D'ABORD (#1046 : une garde jamais vue rouge est un no-op).
        //     Le vérificateur est exercé sur les états défectueux AVANT d'être cru vert :
        //     le témoin amputé du DoD (une planche en moins), la borne haute (une planche en
        //     trop), et le défaut #1204 lui-même (Scenarii cru à 97 cartes).
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Witness_Amputated_Sheet_Fails_Lower_Bound()
        {
            // Témoin du DoD : le TarotCards P&P A4 (105 pages attendues) amputé d'une planche
            // recto-verso → 103 pages produites. La garde DOIT échouer en nommant la borne basse.
            var failures = PdfCardCountIntegrity.CheckPageCounts(new[]
            {
                ("Argumentum_TarotCards_Print&Play_A4", "fr", 105, 103,
                 "format PrintAndPlay, 318 instance(s) (témoin)"),
            });
            failures.Should().ContainSingle()
                .Which.Should().Contain("LOWER BOUND violated").And.Contain("103").And.Contain("105");
        }

        [Fact]
        public void Witness_Extra_Sheet_Fails_Upper_Bound()
        {
            var failures = PdfCardCountIntegrity.CheckPageCounts(new[]
            {
                ("Argumentum_PokerCards_Print&Play_A4", "fr", 38, 40,
                 "format PrintAndPlay, 167 instance(s) (témoin)"),
            });
            failures.Should().ContainSingle()
                .Which.Should().Contain("UPPER BOUND violated").And.Contain("40").And.Contain("38");
        }

        [Fact]
        public void Witness_Defect_1204_Scenarii_At_97_Cards_Fails()
        {
            // Le défaut réel : Scenarii cru à 97 cartes (194 pages en format alterné) alors que la
            // chaîne CSV×config en contient 167 (334 pages). Une « produced count » sans valeur
            // attendue devant ne détecte rien — ici la valeur attendue est DÉRIVÉE, pas crue.
            var failures = PdfCardCountIntegrity.CheckPageCounts(new[]
            {
                ("Argumentum_PokerCards", "fr", 334, 194,
                 "Scenarii: 167 row(s) / rscount 1 → 167 card(s) × 1 copie(s) = 167 instance(s), avec dos."),
            });
            failures.Should().ContainSingle()
                .Which.Should().Contain("194").And.Contain("334").And.Contain("#1204");
        }

        [Fact]
        public void Witness_Matching_Produced_Count_Produces_No_Failure()
        {
            // Contrôle inverse : deux documents sains ne produisent AUCUN échec — sans ce sens,
            // un vérificateur toujours-rouge passerait les trois témoins ci-dessus.
            var failures = PdfCardCountIntegrity.CheckPageCounts(new[]
            {
                ("Argumentum_TarotCards", "fr", 379, 379, "ok"),
                ("Argumentum_PokerCards", "zh", 334, 334, "ok"),
            });
            failures.Should().BeEmpty();
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (1) LA DÉRIVATION REPRODUIT LES CINQ BASELINES MESURÉES (bundle v0.9.0-review).
        //     Chaque Fact épingle une page MESURÉE indépendamment ; l'organe doit la retrouver
        //     par config×CSV×template. Une dérive de source → rouge AVANT régénération.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void TarotCards_Derives_379_Pages()
        {
            var plan = DeriveByName("Argumentum_TarotCards_fr.pdf");
            _output.WriteLine(plan.Breakdown);
            plan.CardInstances.Should().Be(197,
                "15 Rules sans dos + Memo 1 carte × 7 copies + 175 Fallacies = 197 instances — le ×7 vient de la config Documents, pas d'une croyance ; "
                + "le 175 (et non 176) vient du retrait de PK 96 du deck, décision owner #1288");
            plan.ExpectedPages.Should().Be(379,
                "baseline #1176/#1175 RE-DÉRIVÉE après #1288 : 15 pages Rules sans dos + 2 pages × 182 instances avec dos (7 Memo + 175 Fallacies). "
                + "Le bundle mesuré du 20-24/08 porte encore 381 — il est périmé, pas contredit");
        }

        [Fact]
        public void TarotCards_Virtues_Derives_262_Pages()
        {
            var plan = DeriveByName("Argumentum_TarotCards_Virtues_fr.pdf");
            _output.WriteLine(plan.Breakdown);
            plan.ExpectedPages.Should().Be(262,
                "baseline MESURÉE (#1176) : 131 cartes Vertus (filtre card=1) × 2 pages, toutes avec dos (dos Fallacies partagé)");
        }

        [Fact]
        public void PokerCards_Derives_334_Pages()
        {
            var plan = DeriveByName("Argumentum_PokerCards_fr.pdf");
            _output.WriteLine(plan.Breakdown);
            plan.ExpectedPages.Should().Be(334,
                "baseline MESURÉE (#1176) : 167 Scenarii × 2 pages (7 dos distincts par catégorie, mais chaque carte porte le sien — la page compte, pas l'art du dos). "
                + "C'est la garde directe contre #1204 (Scenarii cru à 97 → 194 pages) : le CSV retombe à 97 lignes ⇒ ce Fact échoue avant toute régénération");
        }

        [Fact]
        public void PokerCards_PrintAndPlay_A4_Derives_38_Pages()
        {
            var plan = DeriveByName("Argumentum_PokerCards_Print&Play_A4_fr.pdf");
            _output.WriteLine(plan.Breakdown);
            plan.ExpectedPages.Should().Be(38,
                "baseline MESURÉE (#1176) : grille 3×3 (capacité via ComputePageGeometry : carte poker 63,5×88,9 mm sur A4) → ceil(167/9) = 19 planches × 2 pages (dos + face)");
        }

        [Fact]
        public void TarotCards_PrintAndPlay_A4_Derives_105_Pages()
        {
            var plan = DeriveByName("Argumentum_TarotCards_Print&Play_A4_fr.pdf");
            _output.WriteLine(plan.Breakdown);
            plan.ExpectedPages.Should().Be(105,
                "baseline MESURÉE (#1176) : 318 instances (RulesPP 6 sans dos + Fallacies 176 + Virtues 131 + MemoPP 1×5) sur grille 3×2 = 53 planches ; "
                + "la planche 0 (6 Rules) est SANS dos → pas de page dos ⇒ 52 dos + 53 faces = 105 — la suppression dos-par-planche de Compose est modélisée, pas devinée");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (2) LES MÉCANISMES — chacun épinglé séparément pour qu'une régression parle.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Memo_Card_Count_Is_Derived_From_Template_Rscount_Not_Hardcoded()
        {
            var plan = DeriveByName("Argumentum_TarotCards_fr.pdf");
            var memo = plan.CardSets.Single(c => c.CardSetName == KnownCardSets.Memo);
            _output.WriteLine(memo.ToString());

            memo.SourceRows.Should().Be(175, "le Memo lit la taxonomie FILTRÉE carte∈{1,2} — même source que Fallacies (175 depuis le retrait de PK 96, #1288)");
            memo.Rscount.Should().Be(200, "rscount du template Argumentum_Memo_Face_fr.json — lu du template, pas codé en dur");
            memo.Cards.Should().Be(1, "ceil(175/200) = 1 : la formule de groupement du harvest elle-même (CLAUDE.md « Leçons apprises »)");
            memo.NbCopies.Should().Be(7, "NbCopies=7 dans la config du document TarotCards — la source réelle du « ×7 copies » de la matrice façonniers");
            memo.Instances.Should().Be(7);
        }

        [Fact]
        public void Memo_Divergence_Harvest_Vs_Copies_Is_Visible_In_The_Breakdown()
        {
            // La matrice #1187 portait « Memo 1 (×7 copies) » et des totaux à Memo=1 — deux
            // chiffres pour un objet. L'organe ne tranche pas : il NOMME les deux niveaux
            // (1 carte récoltée, ×7 copies imposées) pour que l'arbitrage owner voie le delta
            // 197 instances vs 191 cartes uniques (#1288 : deck Fallacies 176 → 175, Tarot 192 → 191) :
            // 197+167 = 364 instances physiques imprimées, vs 358 cartes uniques.
            var plan = DeriveByName("Argumentum_TarotCards_fr.pdf");
            plan.Breakdown.Should().Contain("Memo: 175 row(s)")
                .And.Contain("rscount 200 → 1 card(s) × 7 copie(s) = 7 instance(s)");
        }

        [Fact]
        public void RulesPrintAndPlay_Is_Derived_From_Its_Own_Six_Row_Csv()
        {
            var plan = DeriveByName("Argumentum_TarotCards_Print&Play_A4_fr.pdf");
            var rules = plan.CardSets.Single(c => c.CardSetName == KnownCardSets.RulesPrintAndPlay);
            rules.SourceRows.Should().Be(6,
                "« Argumentum Rules - Cards Print and Play.csv » = 6 règles — PAS les 15 du Rules standard : deux DataSets distincts, deux comptes distincts");
            rules.HasBack.Should().BeFalse("aucun template de dos configuré (BackCardSetInfo laissé vide : le prédicat production — chemin de template vide ⇒ payload dos null — est le même ici qu'à la récolte)");
        }

        [Fact]
        public void BackFirstOneDocPerBack_Refuses_To_Guess_Loudly()
        {
            var config = new AssetConverterConfig();
            var doc = config.WebBasedGeneratorConfig.CardSetDocuments
                .First(d => d.DocumentName == "Argumentum_TarotCards_fr.pdf" && d.Enabled);
            doc.DocumentFormat = CardDocumentFormat.BackFirstOneDocPerBack;
            var act = () => PdfDeckCountDerivation.Derive(config, doc, RepoRoot);
            act.Should().Throw<NotSupportedException>()
                .WithMessage("*one PDF per distinct back art*refuses to guess*",
                    "le comptage BackFirstOneDocPerBack dépend du groupement des dos À LA RÉCOLTE ; deviner serait le cinquième chiffre interdit (#1187)");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (3) Les documents Light : dérivés et AFFICHÉS (pas épinglés — leurs pages ne sont
        //     pas dans les baselines mesurées ci-dessus ; le comparateur VisualTests les
        //     couvrira contre le bundle régénéré). Présents ici pour que toute dérive du
        //     filtre print_and_play soit VISIBLE dans la sortie CI, pas silencieuse.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Light_Documents_Are_Derived_And_Reported()
        {
            var tarot = DeriveByName("Argumentum_TarotCards_Print&Play_Light_A4_fr.pdf");
            var poker = DeriveByName("Argumentum_PokerCards_Print&Play_Light_A4_fr.pdf");
            _output.WriteLine($"Tarot Light: {tarot.ExpectedPages} pages — {tarot.Breakdown}");
            _output.WriteLine($"Poker Light: {poker.ExpectedPages} pages — {poker.Breakdown}");

            // Le filtre print_and_play=1 doit être LU du CSV, pas cru depuis les commentaires
            // de config : le commentaire dit « 8 cartes » Vertues-Light, le CSV en donne 24.
            var virtuesLight = tarot.CardSets.Single(c => c.CardSetName == KnownCardSets.VirtuesPrintAndPlayLight);
            virtuesLight.SourceRows.Should().Be(24,
                "mesuré sur le CSV : print_and_play=1 = 24 vertus — le commentaire de config « root + 7 family heads = 8 cards » est FIGÉ, pas un état du CSV");
        }
    }
}
