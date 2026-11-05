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
    /// <para>⚠️ Le témoin n° 2 encode une <b>mesure</b>, pas une opinion : la config déclare le Tarot
    /// à 70 × 120 mm, et le PDF produit mesure 59,9 × 112,9 mm. Un organe qui pinçait le nominal de
    /// la config serait rouge sur <b>chaque</b> PDF du deck ; ce témoin fixe que le nominal retenu est
    /// celui de l'artefact, et que l'écart avec la config reste <b>visible</b> plutôt que résolu en silence.</para>
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
        public void Witness_Config_Nominal_70x120_Is_Red_On_The_Produced_Tarot()
        {
            // LE témoin de la mesure du 05/11 : si l'organe pinnait le couple de la config
            // (FrontCards.WidthMM/HeigthMM = 70 x 120), il serait rouge sur les 8 langues, puisque
            // la page du deck est la taille PHYSIQUE du PNG (Magick), mesurée 169,92 x 320,16 pt.
            var withConfigNominal = PdfPageGeometryIntegrity.Check(new[]
            {
                Row(70, 120, 169.92, 320.16, origin: "FrontCards.WidthMM/HeigthMM de la config (70 x 120)"),
            });
            withConfigNominal.Should().ContainSingle(
                "le nominal de la config n'est PAS la taille de page du deck produit — la pinçer rendrait l'organe rouge sur un arbre sain");
            withConfigNominal[0].Should().Contain("59.9").And.Contain("112.9").And.Contain("70.0").And.Contain("120.0");

            // Le même PDF, mesuré contre l'attendu de l'ARTEFACT, est silencieux : c'est le nominal retenu.
            var withMeasuredNominal = PdfPageGeometryIntegrity.Check(new[]
            {
                Row(169.92 * PdfPageGeometryIntegrity.MmPerPoint, 320.16 * PdfPageGeometryIntegrity.MmPerPoint,
                    169.92, 320.16, origin: "taille physique des PNG du CardSet (mesurée 05/11, 8/8 langues)"),
            });
            withMeasuredNominal.Should().BeEmpty();
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
        public void Inverse_Control_The_Measured_Healthy_Set_Is_Silent()
        {
            // Les 8 lignes MESURÉES le 05/11 sur l'arbre local (8/8 langues), chacune avec son
            // propre attendu : le vérificateur doit être muet. Sans ce sens, un vérificateur
            // toujours-rouge passerait tous les témoins ci-dessus.
            var tarotW = 169.92 * PdfPageGeometryIntegrity.MmPerPoint;
            var tarotH = 320.16 * PdfPageGeometryIntegrity.MmPerPoint;
            var rows = new List<PdfPageGeometryIntegrity.SizeExpectation>
            {
                new("Argumentum_Fallacies_Web_A0_fr.pdf", "fr", 2384 * PdfPageGeometryIntegrity.MmPerPoint, 3371 * PdfPageGeometryIntegrity.MmPerPoint, "PageSizes.A0", 2384, 3371, 1),
                new("Argumentum_Fallacies_Web_A4_fr.pdf", "fr", A4WidthMm, A4HeightMm, "PageSizes.A4", 595, 842, 1),
                new("Argumentum_Fallacies_Web_Thumbnails_A4_fr.pdf", "zh", A4WidthMm, A4HeightMm, "PageSizes.A4", 595, 842, 1),
                new("Argumentum_PokerCards_fr.pdf", "fr", 63.5, 88.9, "PNG 2,5 x 3,5 in", 180, 252, 1),
                new("Argumentum_PokerCards_Print&Play_A4_fr.pdf", "ar", A4WidthMm, A4HeightMm, "PageSizes.A4", 595, 842, 1),
                new("Argumentum_TarotCards_fr.pdf", "pt", tarotW, tarotH, "PNG (169,92 x 320,16 pt)", 169.92, 320.16, 1),
                new("Argumentum_TarotCards_Print&Play_A4_ru.pdf", "ru", A4WidthMm, A4HeightMm, "PageSizes.A4", 595, 842, 1),
                new("Argumentum_TarotCards_Virtues_fa.pdf", "fa", tarotW, tarotH, "PNG (169,92 x 320,16 pt)", 169.92, 320.16, 1),
            };

            var failures = PdfPageGeometryIntegrity.Check(rows);
            _output.WriteLine(string.Join("\n", failures));
            failures.Should().BeEmpty("les 8 géométries mesurées le 05/11 sont conformes à leurs attendus respectifs");
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
