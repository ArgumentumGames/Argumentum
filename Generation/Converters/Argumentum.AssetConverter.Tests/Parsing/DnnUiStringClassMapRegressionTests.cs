using System.Linq;
using Argumentum.AssetConverter.Entities;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Parsing
{
    /// <summary>
    /// Regression tests for DnnUiStringClassMap (issue #457 — DNN UI strings localization).
    /// These exercise the REAL load path — <see cref="CsvBase{T,TMap}.LoadFromContent"/> —
    /// which registers the ClassMap and applies PrepareHeaderForMatch (diacritics/underscore/
    /// hyphen/space stripping + lowercasing), so the "source_file" header resolves and
    /// MissingFieldFound only logs.
    ///
    /// DUAL PATH NOTE: the DatasetUpdater engine reads raw CSV headers via a PLAIN
    /// CsvConfiguration (DataSetInfo.GetDictionaryFromCsv — no ClassMap), so FieldsToInclude/
    /// FieldsToUpdate in the task config use the literal header names. This test guards the
    /// harvest/validation entity path. Inline CSV keeps each test self-contained; rows are
    /// built via string.Join to avoid header/column miscounts.
    /// </summary>
    public class DnnUiStringClassMapRegressionTests
    {
        private static readonly string[] Columns =
        {
            "key", "context", "source_file", "fr", "en", "ru", "pt", "es", "ar", "fa", "zh", "notes"
        };

        private static string Row(params string[] values)
        {
            // Pads/truncates to the full 12-column width so callers only specify the leading cells.
            var full = new string[Columns.Length];
            for (var i = 0; i < full.Length && i < values.Length; i++)
            {
                full[i] = values[i] ?? "";
            }

            return string.Join(",", full);
        }

        private static string Csv(params string[] rows) =>
            string.Join(",", Columns) + "\n" + string.Join("\n", rows) + "\n";

        /// <summary>
        /// Full header set: Key maps the "key" PK, FR is the source, the 7 localized columns
        /// map to their language properties. "source_file" resolves via PrepareHeaderForMatch
        /// (underscore stripped before matching). Placeholders {0}/{1} round-trip untouched.
        /// </summary>
        [Fact]
        public void DnnUiString_LoadFromContent_MapsKeyFrAndLocalizedColumns()
        {
            var csv = Csv(Row(
                "ui.rules.players_range",
                "Rules player-count line",
                "_RulesExplorer_RuleList.cshtml:15",
                "de {0} à {1} joueurs",
                "from {0} to {1} players",
                "от {0} до {1} игроков",
                "de {0} a {1} jogadores",
                "de {0} a {1} jugadores",
                "من {0} إلى {1} لاعب",
                "از {0} تا {1} بازیکن",
                "{0} 到 {1} 名玩家",
                "keep {0}/{1}"));

            var strings = DnnUiString.LoadFromContent(csv);

            strings.Should().ContainSingle();
            var s = strings[0];
            s.Key.Should().Be("ui.rules.players_range");
            s.Context.Should().Be("Rules player-count line");
            s.SourceFile.Should().Be("_RulesExplorer_RuleList.cshtml:15");
            s.Fr.Should().Be("de {0} à {1} joueurs");
            s.En.Should().Be("from {0} to {1} players");
            s.Ru.Should().Be("от {0} до {1} игроков");
            s.Pt.Should().Be("de {0} a {1} jogadores");
            s.Es.Should().Be("de {0} a {1} jugadores");
            s.Ar.Should().Be("من {0} إلى {1} لاعب");
            s.Fa.Should().Be("از {0} تا {1} بازیکن");
            s.Zh.Should().Be("{0} 到 {1} 名玩家");
            s.Notes.Should().Be("keep {0}/{1}");
        }

        /// <summary>
        /// GetId() returns Key when present.
        /// </summary>
        [Fact]
        public void DnnUiString_GetId_ReturnsKey()
        {
            var csv = Csv(Row("res.RuleSummary", "section heading", "_Detail.cshtml:37", "Résumé"));

            var strings = DnnUiString.LoadFromContent(csv);

            strings[0].Key.Should().Be("res.RuleSummary");
            strings[0].GetId().Should().Be("res.RuleSummary");
        }

        /// <summary>
        /// Only "key" is required; every other column is Optional. A header set reduced to
        /// key + fr must parse without throwing and leave the other properties null.
        /// </summary>
        [Fact]
        public void DnnUiString_OptionalColumns_AbsentDoesNotThrow()
        {
            var csv = "key,fr\nres.RuleSummary,Résumé\n";

            var act = () => DnnUiString.LoadFromContent(csv);

            act.Should().NotThrow();
            var s = act().Should().Subject.First();
            s.Key.Should().Be("res.RuleSummary");
            s.Fr.Should().Be("Résumé");
            s.En.Should().BeNull();
            s.Ru.Should().BeNull();
            s.Pt.Should().BeNull();
            s.SourceFile.Should().BeNull();
            s.Notes.Should().BeNull();
        }

        /// <summary>
        /// An empty FR source (res.RuleMemoInstructions — DB-only, "export required before
        /// translation") must still load with a null Fr, so the DatasetUpdater prompt can skip
        /// it. Guards the documented "pas de source → pas de traduction" rule.
        /// </summary>
        [Fact]
        public void DnnUiString_EmptyFrSource_LoadsWithNullFr()
        {
            var csv = Csv(Row("res.RuleMemoInstructions", "memo instructions", "_Detail.cshtml:59", ""));

            var strings = DnnUiString.LoadFromContent(csv);

            strings.Should().ContainSingle();
            strings[0].Key.Should().Be("res.RuleMemoInstructions");
            strings[0].Fr.Should().BeNullOrEmpty();
        }
    }
}
