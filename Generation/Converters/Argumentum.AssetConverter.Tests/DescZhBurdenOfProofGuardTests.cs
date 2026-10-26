using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Garde du grain 2, pool v23 (#458 c.5964435596) : la définition chinoise de
    /// « Renverser la charge de la preuve » (PK 989) porte la DIRECTION de la charge.
    /// Arbitrage ai-01 du 03/10 (c.5964429559) : la cellule disait
    /// « c'est à l'adversaire de prouver votre position <b>vraie</b> » (为真) alors que
    /// les cinq autres langues traduites disent toutes « <b>fausse</b> » — ru
    /// « опровергать », pt « demonstrar a invalidez », ar « إثبات عدم صحة », es
    /// « hasta que se demuestre lo contrario », en « until proved false ». La phrase
    /// se contredisait (les deux membres de l'opposition disaient « prouver vrai »).
    /// Correction d'un caractère, 为真 → 为假, CSV seul — la re-dérivation n° 3
    /// propagera vers <c>Fallacies_zh.content.svg</c> et <c>Fallacies_zh.html</c>,
    /// qui portent encore la phrase fautive (mesuré par <c>git grep</c> au dispatch).
    /// La garde épingle la <b>direction</b> (为假), pas la phrase entière, pour ne pas
    /// geler une éventuelle retouche lexicale future.
    /// </summary>
    public class DescZhBurdenOfProofGuardTests
    {
        private const string CorrectedPolarity = "主张为假是对方的责任";
        private const string InvertedPolarity = "主张为真";

        private static string FallaciesCsv => Path.Combine(
            TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

        private static Dictionary<string, string> RowByPk(string pk)
        {
            var content = File.ReadAllText(FallaciesCsv);
            using var reader = new StringReader(content);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                BadDataFound = null,
                HeaderValidated = null,
            };
            using var csv = new CsvReader(reader, config);
            var hit = csv.GetRecords<dynamic>()
                .Select(r => (IDictionary<string, object>)r)
                .Select(d => d.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty, System.StringComparer.Ordinal))
                .FirstOrDefault(row => row.GetValueOrDefault("PK")?.Trim() == pk);
            hit.Should().NotBeNull("la rangée PK {0} doit exister dans le CSV Fallacies", pk);
            return hit;
        }

        [Fact]
        public void DescZh_Pk989_ShiftingBurdenOfProof_ProvesThePositionFALSE()
        {
            var row = RowByPk("989");

            // Ancrage d'identité : la garde doit parler de LA carte arbitrée, pas d'une autre.
            row.GetValueOrDefault("text_fr")?.Trim().Should().Be("Renverser la charge de la preuve",
                "la garde doit rester ancrée sur la carte arbitrée par c.5964429559");

            var descZh = row.GetValueOrDefault("desc_zh") ?? string.Empty;
            descZh.Should().Contain(CorrectedPolarity,
                "l'arbitrage du 03/10 exige que la charge porte sur réfuter la position (为假), "
                + "comme les cinq autres langues traduites");
            descZh.Should().NotContain(InvertedPolarity,
                "« prouver la position VRAIE » est le contresens arbitré (A) : "
                + "la phrase se contredisait, les deux membres disaient « prouver vrai »");
        }
    }
}
