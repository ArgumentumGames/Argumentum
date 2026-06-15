using System.Linq;
using Argumentum.AssetConverter.Entities;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Parsing
{
    /// <summary>
    /// Regression tests for <see cref="FallacyClassMap"/> via the real load path
    /// <see cref="CsvBase{T,TMap}.LoadFromContent"/>. Fallacy is the largest entity
    /// (~1408 rows) and its ClassMap declares ~45 localized columns across 8 languages
    /// (en/ru/pt/es/ar/fa/zh) plus Latin / print_and_play / AIF_* metadata — all .Optional().
    ///
    /// The critical guard: a single localized column losing its .Optional() would make
    /// HeaderValidated throw when the multilingual harvest loads a FR-only header set,
    /// silently breaking the whole pipeline (same fragility class as the #216/#477
    /// localization-mapping regression). These tests pin that contract.
    ///
    /// Note: EntityClassMapRegressionTests (#476) documents “Fallacy already has coverage
    /// elsewhere” — that is stale; no prior test exercised Fallacy.LoadFromContent (only an
    /// incidental ClassMap registration in MmGeneratorTests, which asserts no mappings).
    ///
    /// Additive only: no existing test or production code is modified. Inline CSV keeps
    /// each test self-contained (no external fixture files).
    /// </summary>
    public class FallacyClassMapRegressionTests
    {
        // Every non-Optional FallacyClassMap column. HeaderValidated requires all of these
        // to be present in the header; every other column (localized en/ru/pt/es/ar/fa/zh,
        // Latin, print_and_play, AIF_*, svg_*, etc.) is .Optional() and may be omitted.
        private const string FallacyRequiredHeader =
            "pk,path,depth,Famille,Famille_camelCase,Sous-Famille,Soussousfamille,état," +
            "carte,niveau,nom_vulgarisé,text_fr,desc_fr,example_fr,link_fr,Remarques," +
            "decimal_path_padded,depth_max4";

        // A representative FR-only data row matching FallacyRequiredHeader (18 fields).
        private const string FallacyRequiredRow =
            "1.1,1.1,1,Logique,logique,Pétition,Principe,actif,1,2,Pétition de principe," +
            "Texte FR,Desc FR,Exemple FR,https://fr,Remarque,1.1,1";

        /// <summary>
        /// Loads a representative Fallacy row (required FR columns + a few localized columns)
        /// and asserts each maps to its property. The single "pk" column populates BOTH the
        /// PK property (Name "pk") and the Id property (Name "PK", normalized to "pk" by
        /// PrepareHeaderForMatch), so GetId() (which returns Id) resolves.
        /// </summary>
        [Fact]
        public void Fallacy_LoadFromContent_MapsRequiredColumns()
        {
            var csv =
                FallacyRequiredHeader + ",text_en,desc_en,Family_zh\n" +
                FallacyRequiredRow + ",Text EN,Desc EN,Famille ZH\n";

            var fallacies = Fallacy.LoadFromContent(csv);

            fallacies.Should().ContainSingle();
            var f = fallacies[0];
            f.PK.Should().Be("1.1");
            f.Path.Should().Be("1.1");
            f.Famille.Should().Be("Logique");
            f.TextFr.Should().Be("Texte FR");
            f.DescFr.Should().Be("Desc FR");
            f.ExampleFr.Should().Be("Exemple FR");
            f.LinkFr.Should().Be("https://fr");
            f.Id.Should().Be("1.1");
            f.GetId().Should().Be("1.1");
            // Localized Optional columns, when present, must map.
            f.TextEn.Should().Be("Text EN");
            f.DescEn.Should().Be("Desc EN");
            f.FamilyZh.Should().Be("Famille ZH");
        }

        /// <summary>
        /// Highest-value guard. A header set containing ONLY the required (non-Optional)
        /// columns must parse without throwing, and every localized/Optional property stays
        /// null. If any of the ~45 localized columns lost its .Optional(), HeaderValidated
        /// would throw here — the exact regression that would silently break the multilingual
        /// harvest. (MissingFieldFound only logs; HeaderValidated throws on absent non-Optional.)
        /// </summary>
        [Fact]
        public void Fallacy_OptionalLocalizedColumns_AbsentDoesNotThrow()
        {
            var csv = FallacyRequiredHeader + "\n" + FallacyRequiredRow + "\n";

            var act = () => Fallacy.LoadFromContent(csv);

            act.Should().NotThrow();
            var f = act().Should().Subject.First();
            f.PK.Should().Be("1.1");
            // Representative Optional columns across every language + meta must be null.
            f.TextEn.Should().BeNull();
            f.DescRu.Should().BeNull();
            f.ExamplePt.Should().BeNull();
            f.LinkEs.Should().BeNull();
            f.FamilyAr.Should().BeNull();
            f.SubfamilyFa.Should().BeNull();
            f.DescZh.Should().BeNull();
            f.PrintAndPlay.Should().BeNull();
            f.Latin.Should().BeNull();
        }
    }
}
