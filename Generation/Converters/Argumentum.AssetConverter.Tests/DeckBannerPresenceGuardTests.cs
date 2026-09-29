using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Garde ⑰ (#1632) : PRÉSENCE des bandeaux du deck. La garde ④c
    /// (<see cref="FallaciesDeckBandInvariantGuardTests"/>) compare la VALEUR des
    /// bandeaux imprimés — une cellule vide n'est pas imprimée, donc invisible pour
    /// elle. Ici : tout niveau de bandeau imprimé en français sur une carte du deck
    /// doit être présent (cellule non vide) dans les sept autres langues.
    ///
    /// Défaut fondateur : « Playing the victim » (2.3.2.3.4, deck en) s'imprimait
    /// sans AUCUN bandeau — ses colonnes Subfamily/Subsubfamily anglaises étaient
    /// vides depuis la base d'avril 2024, alors que le français imprime
    /// « Influence / Manipulation mentale / Jeu de pouvoir ». Le gabarit n'imprime
    /// la famille que si la sous-famille est remplie, et les deux niveaux suivants
    /// que si la sous-sous-famille l'est : deux cellules vides effacent les trois
    /// bandeaux.
    ///
    /// Règle « imprimé » (gabarit Fallacies Face, revue #1588) : le niveau 1
    /// s'imprime si la cellule de niveau 2 est remplie ; les niveaux 2-3 si la
    /// cellule de niveau 3 est remplie.
    /// </summary>
    public class DeckBannerPresenceGuardTests
    {
        private static readonly string[] Languages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

        private static string[] BandColumns(string lang) => lang switch
        {
            "fr" => new[] { "Famille", "Sous-Famille", "Soussousfamille" },
            "en" => new[] { "Family", "Subfamily", "Subsubfamily" },
            _ => new[] { $"Family_{lang}", $"Subfamily_{lang}", $"Subsubfamily_{lang}" },
        };

        private static string FallaciesCsv => Path.Combine(
            TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

        private static Dictionary<string, Dictionary<string, string>> LoadRowsByPath()
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
            var rows = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
            foreach (var record in csv.GetRecords<dynamic>())
            {
                var dict = (IDictionary<string, object>)record;
                var row = dict.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty, StringComparer.Ordinal);
                var path = row.GetValueOrDefault("path")?.Trim();
                if (!string.IsNullOrEmpty(path))
                    rows[path] = row;
            }
            rows.Should().NotBeEmpty("le CSV Fallacies doit charger.");
            return rows;
        }

        private sealed record MissingBanner(string CardPath, string Lang, string Level, string FrenchCell);

        private static List<MissingBanner> MissingBanners(Dictionary<string, Dictionary<string, string>> rows)
        {
            var missing = new List<MissingBanner>();
            foreach (var (path, row) in rows)
            {
                if (string.IsNullOrWhiteSpace(row.GetValueOrDefault("carte")))
                    continue; // hors deck : jamais imprimé (les vides hors deck = grain ⑱)
                var fr = BandColumns("fr");
                var frSub = (row.GetValueOrDefault(fr[1]) ?? "").Trim();
                var frSubsub = (row.GetValueOrDefault(fr[2]) ?? "").Trim();
                foreach (var lang in Languages.Skip(1))
                {
                    var cols = BandColumns(lang);
                    for (var k = 1; k <= 3; k++)
                    {
                        var printedInFrench = k == 1 ? frSub.Length > 0 : frSubsub.Length > 0;
                        if (!printedInFrench)
                            continue; // pas imprimé en français ⇒ aucune obligation
                        var cell = (row.GetValueOrDefault(cols[k - 1]) ?? "").Trim();
                        if (cell.Length == 0)
                            missing.Add(new MissingBanner(path, lang, cols[k - 1],
                                k == 1 ? frSub : frSubsub));
                    }
                }
            }
            return missing;
        }

        [Fact]
        public void French_Printed_Banner_Levels_Are_Present_In_All_Languages()
        {
            var rows = LoadRowsByPath();
            var missing = MissingBanners(rows);
            missing.Should().BeEmpty(
                "⑰ (#1632) : tout niveau de bandeau imprimé en français sur une carte du deck " +
                "est imprimé (cellule non vide) dans les 7 autres langues — deux cellules vides " +
                "effacent les trois bandeaux. Manquants : " +
                $"{string.Join(" ; ", missing.Select(m => $"{m.CardPath} [{m.Lang}] {m.Level} (fr imprime «{m.FrenchCell}»)"))}");
        }

        [Fact]
        public void PlayingTheVictim_En_Banners_Restored()
        {
            var rows = LoadRowsByPath();
            var row = rows["2.3.2.3.4"];
            (row.GetValueOrDefault("Family") ?? "").Trim().Should().Be("Influence",
                "niveau 1 inchangé — seul le vide des niveaux 2-3 masquait les trois bandeaux.");
            (row.GetValueOrDefault("Subfamily") ?? "").Trim().Should().Be("Psychological manipulation",
                "#1632 : titre anglais de l'ancêtre 2.3, déjà imprimé sur les voisines — aucun choix éditorial.");
            (row.GetValueOrDefault("Subsubfamily") ?? "").Trim().Should().Be("Power games",
                "#1632 : titre anglais de l'ancêtre 2.3.2.");
        }
    }
}
