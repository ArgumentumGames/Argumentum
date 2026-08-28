using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Localization
{
    /// <summary>
    /// Organ for #1189 — intellectual-property pseudonymization of the Scenarii deck
    /// (owner decision 2026-08-27, option (a): Scenarii only, Fallacies kept).
    ///
    /// The graphy table below is MEASURED on the CSV (2026-08-28, 6-script sweep), not copied
    /// from the dispatch: the dispatch's own list missed Волан-де-Морт (its «Волдеморт» has
    /// 0 hit in the file), the Persian ولدمورت, and four whole elements — Hogwarts/Poudlard
    /// (27 cells), Panoramix/Getafix (6), Neo (16), Frodo/Frodon (38, the largest single item).
    /// A Latin-only sweep would have "cleaned" fr/en/es/pt and left the exposure intact in
    /// 4 of 8 languages while looking done.
    ///
    /// Matching rules are measured, not guessed:
    /// <list type="bullet">
    /// <item>short Latin/Cyrillic tokens: word boundary AND case sensitivity — the corpus
    /// contains "planeou" (contains "neo"), "необычного" (contains "нео"), and lowercase
    /// "matrix"/"matrice"/"матрица" are common words in their languages;</item>
    /// <item>Arabic: word boundary ("سلوكه" contains "لوك"; "لوكي" is Loki, Norse public
    /// domain, and not Luke);</item>
    /// <item>CJK (≥3 chars) and unambiguous transliterations: substring.</item>
    /// </list>
    ///
    /// Deliberately OUT of the pattern set (handled by rewriting, not by patterns — noise
    /// here would eventually get the guard disabled): "the Force", the red/blue pills,
    /// "the Precious" (Прелесть/Tesoro/النفيس/宝贝/گران‌بها), bare anneau/ring/خاتم, druide,
    /// empire/galaxy/resistance, "monde des sorciers".
    ///
    /// The Moriarty control (path 2.3.4) is PUBLIC DOMAIN (Conan Doyle †1930) and is kept
    /// on purpose: it is tolerated EXPLICITLY by name, and a companion test fails if the
    /// tolerance erodes — an organ that silently counts Moriarty as a violation becomes a
    /// standing pressure to delete a perfectly licit card.
    /// </summary>
    public class ScenariiIntellectualPropertyTests
    {
        private const string ScenariiCsvRelPath = "Cards/Scenarii/Argumentum Scenarii - Cards.csv";
        private const int ExpectedColumnCount = 70;
        private const int ExpectedDataRows = 167;

        private static readonly string[] PseudonymizedPaths = { "2.3.2", "5.1.3", "5.2.2", "5.2.4", "5.2.7" };
        private const string MoriartyControlPath = "2.3.4";

        private enum MatchMode { WordCase, Word, Substring }

        private sealed record ProtectedPattern(string Work, string Label, string Graphy, MatchMode Mode);

        private static readonly ProtectedPattern[] ProtectedGraphies =
        {
            // Harry Potter (2.3.2)
            new("Harry Potter", "Voldemort", "Voldemort", MatchMode.Word),
            new("Harry Potter", "Voldemort", "Волан-де-Морт", MatchMode.Word),
            new("Harry Potter", "Voldemort", "Волдеморт", MatchMode.Word),
            new("Harry Potter", "Voldemort", "فولدمورت", MatchMode.Substring),
            new("Harry Potter", "Voldemort", "ولدمورت", MatchMode.Substring),
            new("Harry Potter", "Voldemort", "伏地魔", MatchMode.Substring),
            new("Harry Potter", "Hogwarts", "Poudlard", MatchMode.Word),
            new("Harry Potter", "Hogwarts", "Hogwarts", MatchMode.Word),
            new("Harry Potter", "Hogwarts", "Хогвартс", MatchMode.Word),
            new("Harry Potter", "Hogwarts", "هوغوورتس", MatchMode.Substring),
            new("Harry Potter", "Hogwarts", "霍格沃茨", MatchMode.Substring),
            new("Harry Potter", "Hogwarts", "هاگوارتز", MatchMode.Substring),
            // Astérix (5.1.3) — BOTH characters of the pair
            new("Astérix", "Obélix", "Obélix", MatchMode.Word),
            new("Astérix", "Obélix", "Obelix", MatchMode.Word),
            new("Astérix", "Obélix", "Обеликс", MatchMode.Word),
            new("Astérix", "Obélix", "أوبيليكس", MatchMode.Substring),
            new("Astérix", "Obélix", "奥贝利克斯", MatchMode.Substring),
            new("Astérix", "Panoramix", "Panoramix", MatchMode.Word),
            new("Astérix", "Panoramix", "Panorámix", MatchMode.Word),
            new("Astérix", "Panoramix", "Getafix", MatchMode.Word),
            new("Astérix", "Panoramix", "بانوراميكس", MatchMode.Substring),
            new("Astérix", "Panoramix", "帕诺拉米克斯", MatchMode.Substring),
            // The Matrix (5.2.2) — capital-M/-М forms only: lowercase matrix/matrice/матрица
            // are ordinary words in en/fr/ru and must not trip the guard
            new("The Matrix", "Title", "Matrix", MatchMode.WordCase),
            new("The Matrix", "Title", "Matrice", MatchMode.WordCase),
            new("The Matrix", "Title", "Матрица", MatchMode.WordCase),
            new("The Matrix", "Title", "المصفوفة", MatchMode.Substring),
            new("The Matrix", "Title", "黑客帝国", MatchMode.Substring),
            new("The Matrix", "Title", "ماتریکس", MatchMode.Substring),
            new("The Matrix", "Title", "ماتريكس", MatchMode.Substring),
            new("The Matrix", "Neo", "Neo", MatchMode.WordCase),
            new("The Matrix", "Neo", "Нео", MatchMode.WordCase),
            new("The Matrix", "Neo", "نيو", MatchMode.Word),
            new("The Matrix", "Neo", "尼奥", MatchMode.Substring),
            // Star Wars (5.2.4)
            new("Star Wars", "Title", "Star Wars", MatchMode.Substring),
            new("Star Wars", "Title", "Звездные войны", MatchMode.Substring),
            new("Star Wars", "Title", "Звёздные войны", MatchMode.Substring),
            new("Star Wars", "Title", "Guerra das Estrelas", MatchMode.Word),
            new("Star Wars", "Title", "حرب النجوم", MatchMode.Substring),
            new("Star Wars", "Title", "星球大战", MatchMode.Substring),
            new("Star Wars", "Title", "جنگ ستارگان", MatchMode.Substring),
            new("Star Wars", "Vader", "Dark Vador", MatchMode.Substring),
            new("Star Wars", "Vader", "Darth Vader", MatchMode.Substring),
            new("Star Wars", "Vader", "Дарт Вейдер", MatchMode.Substring),
            new("Star Wars", "Vader", "دارث فيدر", MatchMode.Substring),
            new("Star Wars", "Vader", "دارث ویدر", MatchMode.Substring),
            new("Star Wars", "Vader", "达斯·维达", MatchMode.Substring),
            new("Star Wars", "Luke", "Luke", MatchMode.WordCase),
            new("Star Wars", "Luke", "Lucas", MatchMode.WordCase), // pt drawer used this wrong name
            new("Star Wars", "Luke", "Luck", MatchMode.WordCase),  // en drawer TYPO — "Luke" alone misses it
            new("Star Wars", "Luke", "Люк", MatchMode.Word),
            new("Star Wars", "Luke", "لوك", MatchMode.Word),
            new("Star Wars", "Luke", "卢克", MatchMode.Substring),
            // The Lord of the Rings (5.2.7)
            new("LOTR", "Gollum", "Gollum", MatchMode.WordCase),
            new("LOTR", "Gollum", "Голлум", MatchMode.Word),
            new("LOTR", "Gollum", "咕噜", MatchMode.Substring),
            new("LOTR", "Gollum", "غولوم", MatchMode.Substring),
            new("LOTR", "Gollum", "گالوم", MatchMode.Substring),
            new("LOTR", "Frodo", "Frodon", MatchMode.Word),
            new("LOTR", "Frodo", "Frodo", MatchMode.Word),
            new("LOTR", "Frodo", "Фродо", MatchMode.Word),
            new("LOTR", "Frodo", "فرودو", MatchMode.Substring),
            new("LOTR", "Frodo", "弗罗多", MatchMode.Substring),
            new("LOTR", "OneRing", "Anneau unique", MatchMode.Word),
            new("LOTR", "OneRing", "One Ring", MatchMode.Word),
            new("LOTR", "OneRing", "Единое кольцо", MatchMode.Substring),
            new("LOTR", "OneRing", "Anel Único", MatchMode.Word),
            new("LOTR", "OneRing", "Anillo Único", MatchMode.Word),
            new("LOTR", "OneRing", "الخاتم الأوحد", MatchMode.Substring),
            new("LOTR", "OneRing", "至尊魔戒", MatchMode.Substring),
            new("LOTR", "OneRing", "魔戒", MatchMode.Substring),
            new("LOTR", "OneRing", "حلقهٔ یگانه", MatchMode.Substring),
            new("LOTR", "OneRing", "حلقه یگانه", MatchMode.Substring),
        };

        /// <summary>Moriarty is public domain (Conan Doyle †1930): NOT a violation — and the
        /// companion test asserts it is still THERE, so the tolerance cannot silently erode.</summary>
        private static readonly ProtectedPattern[] MoriartyGraphies =
        {
            new("Sherlock universe", "Moriarty", "Moriarty", MatchMode.Word),
            new("Sherlock universe", "Moriarty", "Мориарти", MatchMode.Word),
            new("Sherlock universe", "Moriarty", "موريارتي", MatchMode.Substring),
            new("Sherlock universe", "Moriarty", "莫里亚蒂", MatchMode.Substring),
            new("Sherlock universe", "Moriarty", "موریارتی", MatchMode.Substring),
        };

        private static readonly Dictionary<MatchMode, Func<string, string, bool>> Matchers = new()
        {
            [MatchMode.WordCase] = (text, g) => Regex.IsMatch(text, $@"\b{Regex.Escape(g)}\b"),
            [MatchMode.Word] = (text, g) => Regex.IsMatch(text, $@"\b{Regex.Escape(g)}\b", RegexOptions.IgnoreCase),
            [MatchMode.Substring] = (text, g) => text.Contains(g, StringComparison.OrdinalIgnoreCase),
        };

        private static bool IsMatch(ProtectedPattern p, string text) => Matchers[p.Mode](text, p.Graphy);

        // ---- minimal RFC4180 reader: quoted cells, doubled quotes, CRLF records, embedded newlines ----
        private sealed class CsvSheet
        {
            public List<string[]> Rows = new();
            public string[] Header = Array.Empty<string>();
        }

        private static CsvSheet ReadSheet(string path)
        {
            var text = File.ReadAllText(path);
            if (text.Length > 0 && text[0] == (char)0xFEFF) text = text[1..];
            var rows = new List<string[]>();
            var fields = new List<string>();
            var sb = new StringBuilder();
            var inQuotes = false;
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < text.Length && text[i + 1] == '"') { sb.Append('"'); i++; }
                        else inQuotes = false;
                    }
                    else sb.Append(c);
                }
                else if (c == '"') inQuotes = true;
                else if (c == ',') { fields.Add(sb.ToString()); sb.Clear(); }
                else if (c == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                {
                    fields.Add(sb.ToString()); sb.Clear();
                    rows.Add(fields.ToArray()); fields.Clear(); i++;
                }
                else if (c == '\n') // bare LF safety
                {
                    fields.Add(sb.ToString()); sb.Clear();
                    rows.Add(fields.ToArray()); fields.Clear();
                }
                else sb.Append(c);
            }
            if (sb.Length > 0 || fields.Count > 0) { fields.Add(sb.ToString()); rows.Add(fields.ToArray()); }
            return new CsvSheet { Rows = rows, Header = rows[0] };
        }

        private static CsvSheet LoadSheet()
        {
            var path = Path.Combine(TestRepoRoot.Find(), ScenariiCsvRelPath);
            // Fail loudly, never skip: a missing CSV must not read as "contract held" (#1112 lesson).
            File.Exists(path).Should().BeTrue("the Scenarii CSV must exist at {0}", ScenariiCsvRelPath);
            return ReadSheet(path);
        }

        private static IEnumerable<(string Path, string Column, string Value)> CellsOf(CsvSheet sheet, string pathValue)
        {
            var ix = Array.IndexOf(sheet.Header, "path");
            ix.Should().BeGreaterThanOrEqualTo(0, "the Scenarii CSV must carry a 'path' column");
            foreach (var row in sheet.Rows.Skip(1))
            {
                if (row.Length <= ix || row[ix] != pathValue) continue;
                for (int c = 0; c < row.Length && c < sheet.Header.Length; c++)
                    yield return (pathValue, sheet.Header[c], row[c]);
            }
        }

        [Fact]
        public void Pseudonymized_Cards_Carry_No_Protected_Graphy_In_Any_Script()
        {
            var sheet = LoadSheet();
            var violations = new List<string>();
            foreach (var path in PseudonymizedPaths)
            {
                var cells = CellsOf(sheet, path).ToList();
                cells.Should().NotBeEmpty("card {0} must exist in the Scenarii CSV", path);
                foreach (var (_, column, value) in cells)
                {
                    if (string.IsNullOrEmpty(value)) continue;
                    foreach (var p in ProtectedGraphies.Where(p => IsMatch(p, value)))
                        violations.Add($"{path}.{column} carries {p.Work}/{p.Label} graphy «{p.Graphy}» :: {Truncate(value)}");
                }
            }
            violations.Should().BeEmpty(
                "the 5 Scenarii cards were pseudonymized by owner decision (#1189): any hit below is a " +
                "reintroduction (or a residue the Latin-only sweep could not see — check non-Latin columns " +
                "first). Violations:{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, violations));
        }

        [Fact]
        public void Moriarty_Tolerance_Is_Explicit_And_Alive()
        {
            var sheet = LoadSheet();
            var cells = CellsOf(sheet, MoriartyControlPath).ToList();
            cells.Should().NotBeEmpty("the public-domain control card {0} must exist", MoriartyControlPath);
            var hits = cells.Count(cell => !string.IsNullOrEmpty(cell.Value)
                                           && MoriartyGraphies.Any(p => IsMatch(p, cell.Value)));
            hits.Should().BeGreaterThanOrEqualTo(10,
                "path {0} is the PUBLIC-DOMAIN control (Conan Doyle †1930), deliberately kept: it carries " +
                "Moriarty in ~24 cells. If this drops, someone scrubbed a licit card to silence the guard — " +
                "the tolerance must be revisited with the owner, never eroded silently", MoriartyControlPath);
        }

        [Fact]
        public void Csv_Structure_Is_Intact()
        {
            var sheet = LoadSheet();
            sheet.Header.Length.Should().Be(ExpectedColumnCount,
                "the Scenarii CSV has 70 columns; a header change breaks CsvHelper ClassMap identifiers");
            sheet.Rows.Skip(1).Should().HaveCount(ExpectedDataRows,
                "the deck is 167 data cards (#1187 count, measured 2026-08-28)");
            foreach (var path in PseudonymizedPaths.Append(MoriartyControlPath))
                sheet.Rows.Skip(1).Count(r => r[Array.IndexOf(sheet.Header, "path")] == path)
                    .Should().Be(1, "path {0} must match exactly one data row", path);
        }

        /// <summary>Inverse control on the DETECTOR (#1046 — a guard that cannot go red protects
        /// nothing): the measured substring traps must NOT hit, the real graphies MUST hit.</summary>
        [Fact]
        public void Detector_Sees_The_Defects_It_Guards_Against()
        {
            var neo = ProtectedGraphies.Single(p => p.Label == "Neo" && p.Graphy == "Neo");
            var lukAr = ProtectedGraphies.Single(p => p.Label == "Luke" && p.Graphy == "لوك");
            var volRu = ProtectedGraphies.Single(p => p.Label == "Voldemort" && p.Graphy == "Волан-де-Морт");
            var volZh = ProtectedGraphies.Single(p => p.Label == "Voldemort" && p.Graphy == "伏地魔");
            var mat = ProtectedGraphies.Single(p => p.Label == "Title" && p.Graphy == "Matrice");

            IsMatch(neo, "O falador planeou tudo").Should().BeFalse(
                "«planeou» contains 'neo' — case-sensitive word match is what keeps this corpus false positive out");
            IsMatch(neo, "необычного случая").Should().BeFalse("Cyrillic substring trap — case sensitivity");
            IsMatch(lukAr, "سلوكه كان مبرَّرًا").Should().BeFalse(
                "«سلوكه» contains 'لوك' — Arabic needs the word boundary");
            IsMatch(lukAr, "لوكي، إله المقالب").Should().BeFalse(
                "«لوكي» is Loki (Norse, public domain), not Luke — the boundary must hold");
            IsMatch(mat, "la matrice de décision").Should().BeFalse(
                "lowercase 'matrice' is an ordinary French word; only capital 'Matrice' is the title");
            IsMatch(neo, "Neo could free humankind").Should().BeTrue();
            IsMatch(volRu, "Лорд Волан-де-Морт").Should().BeTrue(
                "the REAL Russian graphy (the dispatch's «Волдеморт» had zero hits in the file)");
            IsMatch(volZh, "伏地魔大人").Should().BeTrue("CJK substring match");
        }

        private static string Truncate(string s) => s.Length <= 60 ? s : s[..60] + "…";
    }
}
