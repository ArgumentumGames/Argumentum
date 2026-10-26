using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
    /// <summary>
    /// Self-defending guard for grain 1 of pool #458 c.5985353780 (Scénarios — corrections pt,
    /// PR from master 6a071b98, dossier docs/translation/458-fidelite-scenarii-pt-2026-10-04.md §3).
    ///
    /// MEASURED DEFECT FAMILY (the nine cells this guard pins):
    /// - 1.1.1 issue_pt: missing final period — the ONLY pt issue cell in that state (measured
    ///   1/167) while both sources punctuate (FR « …la reine d'Égypte. », EN « …queen of Egypt.»).
    /// - 1.2.2 smoothTalker_pt: «Uma escravidão» named the INSTITUTION where FR « Un esclavagiste »
    ///   and EN « A slave driver » name the AGENT — no other language made that substitution.
    /// - 1.3.2 drawer_pt: «Um general de 5 -estrela» — space-hyphen AND a number slip
    ///   (EN « The 5-star general »). Fixed to «Um general de 5 estrelas».
    /// - 2.2.7 title_pt: «Pénélope» — French accents in a pt title, while the SAME card's pt
    ///   drawer already wrote the correct «Penélope» (internal incoherence invisible to a
    ///   whole-cell screen).
    /// - 3.2.7 title_pt: «É amor na praia» — a copula turned a nominal title (« L'amour à la
    ///   plage », es « Amor en la playa ») into a sentence, with no source.
    /// - 5.1.2 smoothTalker_pt: «Shtroumphissime» — the French word left as-is; the other five
    ///   languages all forge their own smurf superlative (es «El Pitufísimo», ru «Антивакс»…).
    ///   Replaced by «O Smurfíssimo», consistent with the SAME card's pt «Smurf» usage.
    /// - The space-hyphen family « \S- » (4 cells): 1.3.2, 5.3.2 («Um anti -vaccina»),
    ///   5.3.4 («Um turista sul -coreano»), 6.1.2 («Porta -voz do governo»). The space-dash
    ///   malformation was measured at EXACTLY these four in the pt columns, and at ZERO in the
    ///   FR reference columns (baratineur/piocheur) — the inverse control that makes the
    ///   generalized screen meaningful.
    ///
    /// NOTE for the EN file (grain 6, NOT this grain): 5.3.2 smoothTalker (EN) still carries
    /// «An anti -vaccin» — the same malformation, on a row this grain did not touch.
    /// </summary>
    public class ScenariiPtCorrectionGuardTests
    {
        private const string ScenariiCsv = "Cards/Scenarii/Argumentum Scenarii - Cards.csv";

        // Full-cell SHA-256 pins, derived from the corrected CSV (nine pt cells).
        private static readonly (string Pk, string Column, string Sha256)[] CellPins =
        {
            ("1.1.1", "issue_pt", "ad0974f945feae8690cfef553137a58c0c5571e8e7dbf7d86ed99bdd3a223376"),
            ("1.2.2", "smoothTalker_pt", "cfdb21656638175090c126df027eb8a367b36e67cde0c3a1714059ac3b36c9a3"),
            ("1.3.2", "drawer_pt", "118123be7195bad4c2b1c982eaa17e01aa64a12952865b27fe8537fa5c582731"),
            ("2.2.7", "title_pt", "76c464c3108b8aa87b67d0ad14694d8e1c8c8e0bde212e8225341c5973aa0be9"),
            ("3.2.7", "title_pt", "2ce52059317fd12dc833bb6c71a93af5111d3a31410f73260f0452591817aca5"),
            ("5.1.2", "smoothTalker_pt", "0e0c1fb3dc6b2f79e0e3e36c27502eee408a522ffd07294f9b493e4559adf84e"),
            ("5.3.2", "smoothTalker_pt", "da46e1101bcc09a70caf40916e99da59640acbaf6f2f7b61ad94eb0f29466bc8"),
            ("5.3.4", "drawer_pt", "96abd83699ef08b0a0eece0e04e1ddabc777ac19ea5daf6c1b57a0bb2f37b722"),
            ("6.1.2", "smoothTalker_pt", "17dd7d5f0e4128085f3aa3d2b310fbf1a47e88d9cd172c718b6396f0f96854fb"),
        };

        // Sub-strings the fix eradicated from the pt columns (readable failures).
        private static readonly (string Column, string Form, string Why)[] Eradicated =
        {
            ("smoothTalker_pt", "escravidão", "1.2.2 named the institution, not the agent"),
            ("title_pt", "Pénélope", "2.2.7 carried French accents"),
            ("title_pt", "É amor na praia", "3.2.7 was a copula sentence, not the nominal title"),
            ("smoothTalker_pt", "shtroumph", "5.1.2 must not keep the French word (case-insensitive)"),
            ("drawer_pt", "5 -estrela", "1.3.2 space-hyphen + number slip"),
            ("smoothTalker_pt", "anti -vaccina", "5.3.2 space-hyphen"),
            ("drawer_pt", "sul -coreano", "5.3.4 space-hyphen"),
            ("smoothTalker_pt", "Porta -voz", "6.1.2 space-hyphen"),
        };

        private static string FindRepoRoot() => TestRepoRoot.Find();

        private static Dictionary<string, Dictionary<string, string>> ReadRow(string csvPath)
        {
            var byPk = new Dictionary<string, Dictionary<string, string>>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null,
            };
            using var reader = new StreamReader(csvPath, Encoding.UTF8);
            using var csv = new CsvReader(reader, config);
            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord ?? Array.Empty<string>();
            int pathIdx = Array.IndexOf(headers, "path");
            pathIdx.Should().BeGreaterThanOrEqualTo(0, "the Scenarii CSV must expose the path column");
            while (csv.Read())
            {
                var pk = csv.GetField(pathIdx) ?? string.Empty;
                var cells = new Dictionary<string, string>();
                for (int i = 0; i < headers.Length; i++)
                    cells[headers[i]] = csv.GetField(i) ?? string.Empty;
                byPk[pk] = cells;
            }
            byPk.Should().HaveCount(167, "the Scenarii CSV carries 167 rows");
            return byPk;
        }

        private static string Sha256Hex(string cell)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(cell))).ToLowerInvariant();
        }

        private static string[] PtColumns(IEnumerable<string> headers) =>
            headers.Where(h => h.EndsWith("_pt", StringComparison.Ordinal)).ToArray();

        [Fact]
        public void Corrected_Pt_Cells_Match_Their_Full_Content_Pins()
        {
            var byPk = ReadRow(Path.Combine(FindRepoRoot(), ScenariiCsv));
            var mismatches = new List<string>();
            foreach (var (pk, column, expected) in CellPins)
            {
                string actual = Sha256Hex(byPk[pk][column]);
                if (actual != expected)
                    mismatches.Add($"{pk}.{column}: sha256 {actual} != pin {expected}");
            }
            mismatches.Should().BeEmpty(
                "the nine grain-1 pt cells are pinned full-content; a mismatch is ANY byte change. " +
                "If intentional, re-derive the pin in the same commit. Mismatches: {0}",
                string.Join(" | ", mismatches));
        }

        [Fact]
        public void Eradicated_Pt_Forms_Are_Absent_From_The_Whole_Corpus()
        {
            var byPk = ReadRow(Path.Combine(FindRepoRoot(), ScenariiCsv));
            var offenders = new List<string>();
            foreach (var (pk, cells) in byPk)
            {
                foreach (var (column, form, why) in Eradicated)
                {
                    if (cells[column].Contains(form, StringComparison.OrdinalIgnoreCase))
                        offenders.Add($"{pk}.{column}: '{form}' ({why})");
                }
            }
            offenders.Should().BeEmpty(
                "the grain-1 pt corrections (#458) were measured absent from the pt corpus after the fix; " +
                "any hit is a regression or a new instance of the same family. Offenders: {0}",
                string.Join(" | ", offenders));
        }

        [Fact]
        public void Pt_Columns_Carry_No_Space_Hyphen_Malformation()
        {
            // Generalized screen of the « \S- » family: a non-space character, a space, then a
            // hyphen. Calibrated: EXACTLY four occurrences in the pt columns before the fix, and
            // ZERO in the FR reference columns (baratineur/piocheur) — so a hit here is a defect,
            // not pt style. A legitimate spaced dash would have to be re-measured and, if it is
            // genuine, named as an exclusion WITH its reason.
            var byPk = ReadRow(Path.Combine(FindRepoRoot(), ScenariiCsv));
            var pattern = new Regex(@"\S -", RegexOptions.Compiled);
            var offenders = new List<string>();
            foreach (var (pk, cells) in byPk)
            {
                foreach (var column in PtColumns(cells.Keys))
                {
                    var match = pattern.Match(cells[column]);
                    if (match.Success)
                        offenders.Add($"{pk}.{column}: …{Excerpt(cells[column], match.Index)}…");
                }
            }
            offenders.Should().BeEmpty(
                "no pt column may carry a space-hyphen malformation (the FR reference columns carry none). " +
                "Offenders: {0}", string.Join(" | ", offenders));
        }

        [Fact]
        public void Corrected_Pt_Forms_Are_Present_And_Intact()
        {
            var byPk = ReadRow(Path.Combine(FindRepoRoot(), ScenariiCsv));
            var missing = new List<string>();
            void Expect(string pk, string column, string form)
            {
                if (!byPk[pk][column].Contains(form, StringComparison.Ordinal))
                    missing.Add($"{pk}.{column} lacks '{form}'");
            }
            Expect("1.1.1", "issue_pt", "a rainha do Egito.");
            Expect("1.2.2", "smoothTalker_pt", "escravagista");
            Expect("1.3.2", "drawer_pt", "5 estrelas");
            Expect("2.2.7", "title_pt", "Penélope");
            Expect("3.2.7", "title_pt", "Amor na praia");
            Expect("5.1.2", "smoothTalker_pt", "Smurfíssimo");
            Expect("5.3.2", "smoothTalker_pt", "antivacina");
            Expect("5.3.4", "drawer_pt", "sul-coreano");
            Expect("6.1.2", "smoothTalker_pt", "Porta-voz");
            missing.Should().BeEmpty("the corrected pt forms must stay present. Missing: {0}",
                string.Join(" | ", missing));
        }

        [Fact]
        public void Smurf_Superlative_Is_Portuguese_Not_French()
        {
            // The 5.1.2 pt baratineur must not be the French word, and must use the pt smurf name
            // the SAME card already uses elsewhere («Smurf»): the five-language pattern is a
            // forged superlative in the language, not a loan.
            var byPk = ReadRow(Path.Combine(FindRepoRoot(), ScenariiCsv));
            var ptCell = byPk["5.1.2"]["smoothTalker_pt"];
            ptCell.Should().NotContain("shtroumph", "the French word must not survive in a pt cell (case-insensitive)");
            ptCell.Should().Contain("Smurf", "the forged pt superlative builds on the Smurf name this card already uses");

            // Witness on the French side: the FR cell legitimately keeps the French word — the
            // guard's screen is about the pt column, not the source.
            byPk["5.1.2"]["baratineur"].Should().Contain("shtroumphissime",
                "the FR source keeps the French word; the pt fix is about the pt column only");
        }

        private static string Excerpt(string cell, int index, int window = 35)
        {
            int start = Math.Max(0, index - window);
            int end = Math.Min(cell.Length, index + window);
            return cell[start..end].Replace('\n', ' ');
        }
    }
}
