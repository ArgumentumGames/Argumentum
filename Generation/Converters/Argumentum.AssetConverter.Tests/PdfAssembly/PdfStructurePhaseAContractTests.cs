using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    /// <summary>
    /// #1460 <b>phase A</b> — le contrôle inverse de l'organe de géométrie, exécuté en CI
    /// <b>sans aucun artefact</b> (l'organe lui-même vit dans VisualTests, seul projet qui référence
    /// PdfPig, et il exige un arbre frais).
    ///
    /// <para>Une garde jamais vue rouge est un no-op (#1046) : chaque prédicat de
    /// <see cref="PdfPageGeometryIntegrity"/> est donc exercé sur l'état <b>défectueux</b> AVANT
    /// d'être cru vert. Le fichier porte aussi le <b>contrôle inverse du contrôle</b> — un jeu sain
    /// ne produit aucun échec — sans quoi un vérificateur toujours-rouge passerait tous les témoins.</para>
    ///
    /// <para>⚠️ Le témoin n° 2 encode le <b>discriminant ancien/nouveau nominal</b>, pas une opinion.
    /// Le nominal d'une page carte EST dérivable de la config (<c>ImageHelper.cs:170-185</c> :
    /// <c>ResizeInMM</c> à 300 dpi sur chaque PNG, puis page Magick à la taille du PNG — le Poker
    /// produit mesure exactement son nominal), donc l'organe l'<b>asserte</b>. La mesure du 05/11
    /// (Tarot produit sous le nominal d'avant #1250, config passée au nouveau nominal le 02/09)
    /// doit donc être <b>rouge</b> avec les deux nominaux nommés — c'est le discriminateur
    /// arbre-périmé/nominal-changé. La première validation réelle de l'assertion reste la
    /// prochaine régénération fraîche.</para>
    /// </summary>
    public class PdfStructurePhaseAContractTests
    {
        private readonly ITestOutputHelper _output;
        public PdfStructurePhaseAContractTests(ITestOutputHelper output) => _output = output;

        private const double A4WidthMm = 595.0 * PdfPageGeometryIntegrity.MmPerPoint;
        private const double A4HeightMm = 842.0 * PdfPageGeometryIntegrity.MmPerPoint;

        private static PdfPageGeometryIntegrity.SizeExpectation Row(
            double expectedWidthMm, double expectedHeightMm,
            double actualWidthPt, double actualHeightPt,
            int distinctPageSizes = 1, string origin = "témoin")
            => new("Argumentum_TarotCards_fr.pdf", "fr", expectedWidthMm, expectedHeightMm, origin,
                   actualWidthPt, actualHeightPt, distinctPageSizes);

        // ─────────────────────────────────────────────────────────────────────────
        // (1) LE FORMAT SE VOIT DANS LES DEUX SENS DE L'ERREUR.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Witness_A4_Expectation_On_An_A0_Page_Is_Red()
        {
            // Un document Web A4 composé sur une page A0 : 841 mm au lieu de 210. La garde doit le
            // nommer avec les deux valeurs, jamais un booléen.
            var failures = PdfPageGeometryIntegrity.Check(new[]
            {
                Row(A4WidthMm, A4HeightMm, 2384, 3371, origin: "format déclaré A4 = 595 x 842 pt"),
            });

            failures.Should().ContainSingle();
            failures[0].Should().Contain("841.0").And.Contain("1189.2")   // la mesure produite
                        .And.Contain("209.9").And.Contain("A4");          // l'attendu, nommé
        }

        [Fact]
        public void Witness_Tarot_Produced_Under_The_Old_Nominal_Is_Red_Against_The_Current_Config()
        {
            // LE discriminant ancien/nouveau nominal. Le nominal d'une page carte est dérivable de la
            // config (ResizeInMM à 300 dpi → PNG → page Magick ; preuve : le Poker produit = son
            // nominal exact), donc l'organe asserte le nominal courant. La mesure du 05/11 (arbre du
            // 15/08, antérieur au changement de nominal du 02/09) doit donc être ROUGE : une page
            // produite sous l'ancien nominal, attendue sous le nominal courant de la config.
            var oldNominalPage = PdfPageGeometryIntegrity.Check(new[]
            {
                Row(70, 120, 169.92, 320.16,
                    origin: "nominal de config FrontCards 70 x 120 mm (post-02/09) — chaîne ImageHelper.ResizeInMM (300 dpi) → PNG → page"),
            });
            oldNominalPage.Should().ContainSingle(
                "une page produite sous l'ancien nominal doit être rouge contre le nominal courant — c'est le discriminateur arbre-périmé/nominal-changé");
            oldNominalPage[0].Should().Contain("59.9").And.Contain("112.9")   // la mesure produite (ancien nominal)
                            .And.Contain("70.0").And.Contain("120.0");       // l'attendu de config, nommé

            // Contrôle inverse AU NIVEAU DU CHECK : une page au nominal courant (mm→pt exact de la
            // config — la chaîne ResizeInMM l'impose par construction) est silencieuse.
            // ⚠️ Ce n'est PAS une mesure de régénération : la première validation réelle de
            // l'assertion reste la prochaine régénération fraîche.
            var ptPerMm = 72.0 / 25.4;
            var currentNominalPage = PdfPageGeometryIntegrity.Check(new[]
            {
                Row(70, 120, 70 * ptPerMm, 120 * ptPerMm,
                    origin: "nominal de config FrontCards 70 x 120 mm (mm→pt exact de la chaîne ResizeInMM)"),
            });
            currentNominalPage.Should().BeEmpty();
        }

        [Fact]
        public void Witness_Tolerance_Edge_Inside_Passes_Outside_Fails()
        {
            // La tolérance est un seuil, pas une intention : 0,9 mm d'écart passe, 1,1 mm échoue.
            var inside = 595.0 + 0.9 / PdfPageGeometryIntegrity.MmPerPoint;  // +0,9 mm
            var outside = 595.0 + 1.1 / PdfPageGeometryIntegrity.MmPerPoint; // +1,1 mm

            PdfPageGeometryIntegrity.Check(new[] { Row(A4WidthMm, A4HeightMm, inside, 842) })
                .Should().BeEmpty("0,9 mm d'écart est dans la tolérance pointeur de 1 mm");

            PdfPageGeometryIntegrity.Check(new[] { Row(A4WidthMm, A4HeightMm, outside, 842) })
                .Should().ContainSingle("1,1 mm d'écart est hors tolérance — sinon la tolérance n'est pas un seuil");
        }

        [Fact]
        public void Witness_NonUniform_Pages_Are_Red()
        {
            // Aucun contrôle de nombre de pages ne voit un PDF qui change de format en cours de route.
            var failures = PdfPageGeometryIntegrity.Check(new[] { Row(A4WidthMm, A4HeightMm, 595, 842, distinctPageSizes: 2) });

            failures.Should().ContainSingle().Which.Should().Contain("DISTINCTES");
        }

        [Fact]
        public void Inverse_Control_The_Healthy_Set_Is_Silent()
        {
            // Le corpus sain, chaque ligne avec l'attendu que l'organe lui oppose aujourd'hui.
            // Les lignes A4/A0/Poker sont MESURÉES (05/11) ; les lignes Tarot/Virtues portent le
            // nominal de CONFIG courant en mm→pt exact (chaîne ResizeInMM) — le seul Tarot mesuré
            // est celui de l'arbre périmé, et sa rougeur est précisément le témoin n° 2. Sans ce
            // sens inverse, un vérificateur toujours-rouge passerait tous les témoins ci-dessus.
            var ptPerMm = 72.0 / 25.4;
            var rows = new List<PdfPageGeometryIntegrity.SizeExpectation>
            {
                new("Argumentum_Fallacies_Web_A0_fr.pdf", "fr", 2384 * PdfPageGeometryIntegrity.MmPerPoint, 3371 * PdfPageGeometryIntegrity.MmPerPoint, "PageSizes.A0 (mesuré 05/11)", 2384, 3371, 1),
                new("Argumentum_Fallacies_Web_A4_fr.pdf", "fr", A4WidthMm, A4HeightMm, "PageSizes.A4 (mesuré 05/11)", 595, 842, 1),
                new("Argumentum_Fallacies_Web_Thumbnails_A4_fr.pdf", "zh", A4WidthMm, A4HeightMm, "PageSizes.A4 (mesuré 05/11)", 595, 842, 1),
                new("Argumentum_PokerCards_fr.pdf", "fr", 63.5, 88.9, "nominal de config FrontCards 63,5 x 88,9 mm — MESURÉ exact (05/11)", 180, 252, 1),
                new("Argumentum_PokerCards_Print&Play_A4_fr.pdf", "ar", A4WidthMm, A4HeightMm, "PageSizes.A4 (mesuré 05/11)", 595, 842, 1),
                new("Argumentum_TarotCards_fr.pdf", "pt", 70, 120, "nominal de config 70 x 120 mm (mm→pt exact de la chaîne ResizeInMM)", 70 * ptPerMm, 120 * ptPerMm, 1),
                new("Argumentum_TarotCards_Print&Play_A4_ru.pdf", "ru", A4WidthMm, A4HeightMm, "PageSizes.A4 (mesuré 05/11)", 595, 842, 1),
                new("Argumentum_TarotCards_Virtues_fa.pdf", "fa", 70, 120, "nominal de config 70 x 120 mm (mm→pt exact de la chaîne ResizeInMM)", 70 * ptPerMm, 120 * ptPerMm, 1),
            };

            var failures = PdfPageGeometryIntegrity.Check(rows);
            _output.WriteLine(string.Join("\n", failures));
            failures.Should().BeEmpty("le corpus sain (mesuré + nominal de config) est silencieux pour chaque ligne");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (2) LA TABLE DES FORMATS DÉCLARÉS N'EST PAS UNE RECOPIE : elle est confrontée
        //     à QuestPDF lui-même, la bibliothèque qui compose la page. Sinon « A4 = 210 mm »
        //     serait un chiffre de documentation, et un bump de QuestPDF ne la réveillerait pas.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Declared_Formats_Are_The_QuestPDF_Values_Not_A_Copy()
        {
            // ⚠️ La table N'EST PAS comparée à un couple recopié (595 x 842) mais à ce que
            // QuestPDF répond : c'est la seule façon qu'un bump de la bibliothèque la réveille.
            // Mesuré le 05/11 : PageSizes.A4 vaut en réalité 595,4 x 841,9 pt (210,04 x 297,00 mm),
            // et le MediaBox PRODUIT est 595 x 842 pt — QuestPDF arrondit le MediaBox au point
            // entier. L'écart (0,14 mm) est très à l'intérieur de la tolérance pointeur de 1 mm :
            // c'est exactement pour cela que la spec #1460 impose une tolérance et non l'octet.
            foreach (var id in new[] { "A4", "A0" })
            {
                var property = typeof(QuestPDF.Helpers.PageSizes).GetProperty(id);
                property.Should().NotBeNull($"QuestPDF.Helpers.PageSizes.{id} doit exister — la table déclarée s'y adosse");
                var size = property!.GetValue(null)!;

                var width = ReadSize(size, "Width");
                var height = ReadSize(size, "Height");
                _output.WriteLine($"PageSizes.{id} = {width} x {height} pt = " +
                                  $"{width * PdfPageGeometryIntegrity.MmPerPoint:F2} x {height * PdfPageGeometryIntegrity.MmPerPoint:F2} mm");

                var declared = PdfPageGeometryIntegrity.Declared(id);
                declared.WidthMm.Should().BeApproximately(width * PdfPageGeometryIntegrity.MmPerPoint, 0.2,
                    $"la table déclarée de {id} doit suivre QuestPDF, pas une valeur de documentation");
                declared.HeightMm.Should().BeApproximately(height * PdfPageGeometryIntegrity.MmPerPoint, 0.2);
            }
        }

        private static double ReadSize(object value, string member)
        {
            var type = value.GetType();
            var property = type.GetProperty(member);
            if (property != null) return Convert.ToDouble(property.GetValue(value));
            var field = type.GetField(member);
            if (field != null) return Convert.ToDouble(field.GetValue(value));
            throw new InvalidOperationException($"PageSizes type exposes no '{member}'");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (3) LA GARDE D'ARBRE — les quatre états de la charpente §1, dont l'état 1
        //     (DIVERGENT) que la charpente déclarait INATTEIGNABLE et que la phase A branche.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void JudgeTree_Refuses_Absent_Empty_And_Stale_Trees()
        {
            var corpus = new DateTime(2026, 10, 31, 15, 9, 0);
            var older = corpus.AddDays(-77);

            PdfPageGeometryIntegrity.JudgeTree(false, 0, corpus, default, 0).Verdict
                .Should().Be(TreeVerdict.NotMeasurable, "un arbre absent n'est pas un arbre conforme");
            PdfPageGeometryIntegrity.JudgeTree(true, 0, corpus, corpus, 0).Verdict
                .Should().Be(TreeVerdict.NotMeasurable, "un arbre sans PDF ne peut rien porter");
            var stale = PdfPageGeometryIntegrity.JudgeTree(true, 64, corpus, older, 80);
            stale.Verdict.Should().Be(TreeVerdict.NotMeasurable);
            stale.Motif.Should().Contain("STALE").And.Contain("77.0");
        }

        [Fact]
        public void JudgeTree_Reports_Divergent_When_A_Fresh_Tree_Misses_An_Expected_Pdf()
        {
            // ⚠️ L'état que la charpente §3 déclarait INATTEIGNABLE : la phase A lui branche sa
            // source d'attendu (config × CSV → 10 documents × 8 langues = 80 PDF). Un arbre frais
            // qui n'en porte que 64 est DIVERGENT — pas MESURABLE, pas NON MESURABLE.
            // C'est l'état de l'arbre local du 05/11 : 64 PDF, les deux documents Light manquant.
            var corpus = new DateTime(2026, 10, 31, 15, 9, 0);
            var fresh = corpus.AddDays(1);

            var divergent = PdfPageGeometryIntegrity.JudgeTree(true, 64, corpus, fresh, 16);
            divergent.Verdict.Should().Be(TreeVerdict.Divergent);
            divergent.Motif.Should().Contain("16");

            var complete = PdfPageGeometryIntegrity.JudgeTree(true, 80, corpus, fresh, 0);
            complete.Verdict.Should().Be(TreeVerdict.Measurable);
            complete.Motif.Should().Contain("80");
        }
    }
}
