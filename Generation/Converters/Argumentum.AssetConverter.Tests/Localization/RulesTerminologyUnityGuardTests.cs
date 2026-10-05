using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
    /// <summary>
    /// Self-defending guard for grain 4 of pool #458 c.5985353780 (Règles — un terme pour un
    /// objet, PR from master 6a071b98, dossier docs/translation/458-fidelite-regles-6-langues-2026-10-04.md).
    ///
    /// MEASURED DEFECT FAMILY (the fix this guard pins): one game object named by two or three
    /// competing terms inside a single language corpus. Decision rule of the pool: the
    /// MAJORITY term MEASURED over the language corpus (Règles main + Print&amp;Play + Scénarios +
    /// sophismes), count cited; without a net majority it is an observation, not a correction.
    ///
    /// - ar « deck of cards »: رزمة ×10 (Rules_02/03/09/11 + PP_02/03) vs حزمة ×4 (Rules_09 ×2,
    ///   Rules_13 ×2) → رزمة wins 10/4; the 4 حزمة occurrences were rewritten. Both forms name
    ///   the same object the FR source calls « paquet » (component packs AND draw deck alike —
    ///   Rules_09 itself used رزمة for its draw deck and حزمة for its components).
    /// - es « memo cards »: « Cartas recordatorio » ×5 (Rules_02 ×2, Rules_09, PP_02 ×2) vs
    ///   « Cartas de ayuda » ×1 (Rules_11) vs « Cartas de memo » ×1 (Rules_13) → recordatorio
    ///   wins 5/1/1; both minority names rewritten at their component-list positions.
    /// - ru « memo cards »: « Карты Мемо » ×5 (Rules_02/09/11/13 + PP_02 component lists) vs
    ///   « Карты-памятки » ×2 (Rules_02 + PP_02 bodies) → Карты Мемо wins 5/2; both body
    ///   occurrences rewritten (« Карты Мемо кратко излагают эту классификацию. », FR witness
    ///   « Les cartes mémo résument cette classification. »).
    ///
    /// OBSERVATION, NOT CORRECTED (see Atout_Split_Stays_At_Its_Measured_Counts): fa « atout »
    /// is اَتو ×5 (Rules_14) vs حکم ×6 (Rules_15) — 5/6 is no net majority. The two حکم hits
    /// outside Règles (Scenarii 1.2.5 issue_fa « حکم اعدام », Fallacies 314 example_fa
    /// « حکم‌فرما ») are the VERDICT sense, not the trump sense, and were excluded from the
    /// count BEFORE deciding — a naive 5/8 tally would have manufactured a majority.
    ///
    /// The Print &amp; Play CSV is IN scope here (unlike the grain-7 punctuation guard): the ru
    /// minority term was measured present in PP_02, so leaving it would have kept the defect.
    /// </summary>
    public class RulesTerminologyUnityGuardTests
    {
        private const string MainRulesCsv = "Cards/Rules/Argumentum Rules - Cards.csv";
        private const string PpRulesCsv = "Cards/Rules/Argumentum Rules - Cards Print and Play.csv";

        // Minority forms eradicated by the fix — any hit, in either Rules CSV, is a regression.
        private static readonly (string Column, string Form)[] MinorityForms =
        {
            ("Text_ar", "حزمة"),               // حزمة
            ("Text_es", "Cartas de ayuda"),
            ("Text_es", "Cartas de memo"),
            ("Text_ru", "Карты-памятки"),      // Карты-памятки
        };

        // Majority forms with their POST-FIX measured counts (>= so pluralising prose can only
        // grow them; shrinking below the measured floor means a cell lost its term).
        private static readonly (string Csv, string Column, string Form, int Floor)[] MajorityFloors =
        {
            (MainRulesCsv, "Text_ar", "رزمة", 11),               // رزمة — 7 pre-fix + 4 rewritten
            (MainRulesCsv, "Text_es", "Cartas recordatorio", 4), // 3 pre-fix + 1 rewritten (case-exact)
            (PpRulesCsv,   "Text_es", "Cartas recordatorio", 1),
            (MainRulesCsv, "Text_ru", "Карты Мемо", 5),          // Карты Мемо — 4 + 1 rewritten
            (PpRulesCsv,   "Text_ru", "Карты Мемо", 2),          // 1 + 1 rewritten
        };

        // Full-cell SHA-256 pins (computed from the fixed CSVs, never from memory): any byte
        // change in one of the six corrected cells reddens its named fact. Rules_02 and
        // RulesPP_02 Text_ru pins are IDENTICAL — the two cells are byte-equal, a cross-file
        // consistency witness in its own right.
        private static readonly (string Csv, string Pk, string Column, string Sha256)[] CellPins =
        {
            (MainRulesCsv, "Rules_02",  "Text_ru", "a23bfc05251d0538630e2be5b6310f771812f0176d3804fa32c1b131498cca7e"),
            (MainRulesCsv, "Rules_09",  "Text_ar", "735a7ab8017508431287f23c5a513e8c04b258a18c42aa6a49382107f3902c42"),
            (MainRulesCsv, "Rules_11",  "Text_es", "653fc8262b038a2384a03c67c057c13ca6e0eb763624a3cd813b8d179884355d"),
            // Rules_13 ar/es re-pinned 05/10 in the same commit as the 32 -> 28 fix (grain 4 of
            // pool c.5988407613, GO owner): the full-cell pin reddens on ANY byte change, which
            // is its job — these two cells legitimately changed (« 32 cartes » -> 28, the source
            // contradiction of dossier regles-sources-fr-en §4.1). Re-derived from the corrected
            // CSV, cross-checked against RulesEnCorrectionsGuardTests' own pins (identical).
            (MainRulesCsv, "Rules_13",  "Text_ar", "3ea7636d8980c0799acd99b1cb1a442679fe0428e7e18dac2cca52432368bf0b"),
            (MainRulesCsv, "Rules_13",  "Text_es", "1f2ffdd998fcdfc05d80451fa7cdd6f522a03b4a7b0a60a11c6f4a93acd269be"),
            (PpRulesCsv,   "RulesPP_02", "Text_ru", "a23bfc05251d0538630e2be5b6310f771812f0176d3804fa32c1b131498cca7e"),
        };

        // fa observation: the atout split stays at its measured counts until re-arbitrated.
        private const string FaAtoutBorrowed = "اَتو"; // اَتو
        private const string FaAtoutHokm = "حکم";     // حکم
        private const int FaAtoutBorrowedCount = 5;    // Rules_14 only
        private const int FaAtoutHokmCount = 6;        // Rules_15 only

        private static string FindRepoRoot() => TestRepoRoot.Find();

        private static Dictionary<string, Dictionary<string, string>> ReadRulesCells(string csvPath, int expectedRows)
        {
            var byPk = new Dictionary<string, Dictionary<string, string>>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null,
            };
            using (var reader = new StreamReader(csvPath, Encoding.UTF8))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();
                var headers = csv.HeaderRecord ?? Array.Empty<string>();
                int pkIdx = Array.IndexOf(headers, "pk");
                pkIdx.Should().BeGreaterThanOrEqualTo(0, "the Rules CSV must expose the pk column");

                var columnIndex = new Dictionary<string, int>();
                foreach (var col in new[] { "Text_ar", "Text_es", "Text_ru", "Text_fa" })
                {
                    int idx = Array.IndexOf(headers, col);
                    if (idx >= 0) columnIndex[col] = idx;
                }
                columnIndex.Should().HaveCount(4,
                    "both Rules CSVs must expose the ar/es/ru/fa columns: {0}", string.Join(", ", columnIndex.Keys));

                while (csv.Read())
                {
                    var pk = csv.GetField(pkIdx) ?? string.Empty;
                    byPk[pk] = columnIndex.ToDictionary(kv => kv.Key, kv => csv.GetField(kv.Value) ?? string.Empty);
                }
            }
            byPk.Should().HaveCount(expectedRows,
                $"{Path.GetFileName(csvPath)} carries {expectedRows} rows");
            return byPk;
        }

        private static string Sha256Hex(string cell)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(cell))).ToLowerInvariant();
        }

        [Fact]
        public void Minority_Terms_Are_Absent_From_Both_Rules_Csv()
        {
            var offenders = new List<string>();
            foreach (var csv in new[] { MainRulesCsv, PpRulesCsv })
            {
                var byPk = ReadRulesCells(Path.Combine(FindRepoRoot(), csv), csv == MainRulesCsv ? 15 : 6);
                foreach (var (pk, columns) in byPk)
                {
                    foreach (var (column, form) in MinorityForms)
                    {
                        if (columns.TryGetValue(column, out var cell) && cell.Contains(form))
                            offenders.Add($"{Path.GetFileName(csv)} {pk}.{column}: '{form}'");
                    }
                }
            }
            offenders.Should().BeEmpty(
                "grain 4 (#458) unified one-term-per-object in the Règles corpus: حزمة→رزمة (10/4), " +
                "'Cartas de ayuda'/'Cartas de memo'→'Cartas recordatorio' (5/1/1), 'Карты-памятки'→'Карты Мемо' (5/2). " +
                "Any hit is a regression or a new split term. Offenders: {0}",
                string.Join(" | ", offenders));
        }

        [Fact]
        public void Majority_Terms_Stay_At_Or_Above_Their_Measured_Floors()
        {
            var shortfalls = new List<string>();
            foreach (var (csv, column, form, floor) in MajorityFloors)
            {
                var byPk = ReadRulesCells(Path.Combine(FindRepoRoot(), csv), csv == MainRulesCsv ? 15 : 6);
                int count = byPk.Values.Sum(cells => CountOccurrences(cells[column], form));
                if (count < floor)
                    shortfalls.Add($"{Path.GetFileName(csv)} {column} '{form}': {count} < {floor}");
            }
            shortfalls.Should().BeEmpty(
                "the majority terms were counted at these floors after the grain-4 fix (#458); falling below " +
                "one means a cell lost its term wholesale, which the corpus-zero screen cannot see. Shortfalls: {0}",
                string.Join(" | ", shortfalls));
        }

        [Fact]
        public void Corrected_Cells_Match_Their_Full_Content_Pins()
        {
            var mismatches = new List<string>();
            foreach (var (csv, pk, column, expectedSha) in CellPins)
            {
                var byPk = ReadRulesCells(Path.Combine(FindRepoRoot(), csv), csv == MainRulesCsv ? 15 : 6);
                string actual = Sha256Hex(byPk[pk][column]);
                if (actual != expectedSha)
                    mismatches.Add($"{Path.GetFileName(csv)} {pk}.{column}: sha256 {actual} != pin {expectedSha}");
            }
            mismatches.Should().BeEmpty(
                "the six grain-4 corrected cells are pinned full-content; a mismatch is ANY byte change, " +
                "terminological or not — if intentional, re-derive the pin in the same commit. Mismatches: {0}",
                string.Join(" | ", mismatches));
        }

        [Fact]
        public void Atout_Split_Stays_At_Its_Measured_Counts()
        {
            // fa « atout »: اَتو ×5 (Rules_14) vs حکم ×6 (Rules_15) — 5/6 is no NET majority, so
            // the pool rule makes this an observation, not a correction. This fact pins the split
            // at its measured counts: any NEW occurrence of either term reddens here and forces a
            // re-measure (and re-arbitration) rather than silently drifting the tally. Note the
            // two حکم hits outside Règles are the verdict sense (حکم اعدام / حکم‌فرما) and never
            // counted toward the trump tallies.
            var byPk = ReadRulesCells(Path.Combine(FindRepoRoot(), MainRulesCsv), 15);
            int borrowed = CountOccurrences(byPk["Rules_14"]["Text_fa"], FaAtoutBorrowed);
            int hokm = CountOccurrences(byPk["Rules_15"]["Text_fa"], FaAtoutHokm);

            borrowed.Should().Be(FaAtoutBorrowedCount,
                "Rules_14 fa carried اَتو ×5 at the grain-4 measurement (no net majority vs حکم ×6 ⇒ observation). " +
                "A different count means the corpus moved: re-measure BOTH terms and either unify or update this pin " +
                "in the same commit.");
            hokm.Should().Be(FaAtoutHokmCount,
                "Rules_15 fa carried حکم ×6 at the grain-4 measurement. See the Rules_14 note.");
        }

        private static int CountOccurrences(string cell, string form)
        {
            int count = 0, at = 0;
            while ((at = cell.IndexOf(form, at, StringComparison.Ordinal)) >= 0)
            {
                count++;
                at += form.Length;
            }
            return count;
        }
    }
}
