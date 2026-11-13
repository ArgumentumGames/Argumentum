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
        /// #498 grain 2 — the five AIF mapping columns (AIF_skosDirectRef, AIF_skosExceptionRef,
        /// AIF_skosOther, AIF_criticalQuestion, AIF_skosMappingType) map to their properties when
        /// present. The row mirrors the REAL prod row path=6.3 (« Raisonnement biaisé », the grappe
        /// head of the 10-node articulation sample): AIF_criticalQuestion is the FR-prose critical
        /// question the fallacy skips (RFC4180-quoted in prod because it contains a comma — the
        /// corpus typography is U+2019 apostrophes + plain space before « ? »), AIF_skosMappingType
        /// a skos:*Match token. Mirrors Virtue_LoadFromContent_Maps499RelationalAndAifColumns
        /// (#1192) on the Fallacies side.
        /// </summary>
        [Fact]
        public void Fallacy_LoadFromContent_MapsAifCriticalQuestionColumn()
        {
            var aifCols =
                "AIF_skosDirectRef,AIF_skosExceptionRef,AIF_skosOther,AIF_criticalQuestion,AIF_skosMappingType";
            // Real prod values of the path=6.3 row (measured on the 105-col CSV): direct ref +
            // exception ref tokens, empty skosOther, the quoted FR critical question, a skos token.
            var aifRow =
                "Bias_Inference" +                 // AIF_skosDirectRef (Walton scheme label)
                ",PresumptiveInference_Scheme" +   // AIF_skosExceptionRef (exception scheme label)
                "," +                              // AIF_skosOther (empty — reserved for declared gaps)
                ",\"La conclusion repose-t-elle sur des raisons, plutôt que sur un biais qui a orienté le choix des informations ?\"" +
                ",skos:closeMatch";                // AIF_skosMappingType (skos:*Match tokens only)

            var csv = FallacyRequiredHeader + "," + aifCols + "\n" + FallacyRequiredRow + "," + aifRow + "\n";

            var fallacies = Fallacy.LoadFromContent(csv);

            fallacies.Should().ContainSingle();
            var f = fallacies[0];
            f.AIFSkosDirectRef.Should().Be("Bias_Inference");
            f.AIFSkosExceptionRef.Should().Be("PresumptiveInference_Scheme");
            f.AIFCriticalQuestion.Should().Be(
                "La conclusion repose-t-elle sur des raisons, plutôt que sur un biais qui a orienté le choix des informations ?",
                "the quoted FR prose (comma inside) must map verbatim, RFC4180 unquoted by CsvHelper");
            f.AIFSkosMappingType.Should().Be("skos:closeMatch",
                "the token column stays token-only; the FR prose lives in AIF_criticalQuestion, never here");
            f.AIFSkosOther.Should().BeEmpty("empty cell maps to empty string, not null (CsvHelper contract)");
        }

        /// <summary>
        /// #498 grain 2 — AIF_criticalQuestion is .Optional(): a header set without it (a
        /// pre-#498-grain-2 CSV, or a harvest header) must load without throwing and leave the
        /// property null. Losing the .Optional() flag would make HeaderValidated throw when the
        /// multilingual harvest loads a header missing the AIF block — the exact fragility class
        /// this file pins (same shape as Fallacy_OptionalLocalizedColumns_AbsentDoesNotThrow).
        /// </summary>
        [Fact]
        public void Fallacy_LoadFromContent_AifCriticalQuestionAbsent_DoesNotThrow()
        {
            var csv = FallacyRequiredHeader + "\n" + FallacyRequiredRow + "\n";

            var act = () => Fallacy.LoadFromContent(csv);

            act.Should().NotThrow();
            var fallacies = act().Should().Subject;
            fallacies.Should().ContainSingle();
            fallacies.First().AIFCriticalQuestion.Should().BeNull(
                "AIF_criticalQuestion is Optional and absent → null, not throw");
        }

        /// <summary>
        /// #498 grain 12 — the imitated-scheme column (AIF_imitatedScheme, component (a) of
        /// the #498 generative rule) maps to its property when present. The row mirrors the
        /// REAL prod row path=2.1.2.3 (PK 244, « Cynisme ») — the only row carrying the column
        /// in the group-2.1 PR: the FR-prose phrase names what the fallacy IMITATES where AIF
        /// has no scheme for it (cadrage #2229 representation table). The phrase is
        /// RFC4180-quoted in prod (prose convention, measured on skosOther/criticalQuestion),
        /// so the test exercises the quoted path too. Absence is already pinned by
        /// Fallacy_OptionalLocalizedColumns_AbsentDoesNotThrow (required header only, no AIF
        /// block → loads, property null) — the column is .Optional() like every AIF column.
        /// </summary>
        [Fact]
        public void Fallacy_LoadFromContent_MapsAifImitatedSchemeColumn()
        {
            var aifCols = "AIF_skosDirectRef,AIF_skosExceptionRef,AIF_imitatedScheme,AIF_skosOther";
            // Real prod values of the path=2.1.2.3 row as written by the grain-12 PR (106-col
            // CSV): empty refs (no correspondence — that is the point of the representation),
            // the quoted FR imitation phrase, empty skosOther (reserved for declared gaps; the
            // census distinguishes the two states). First term is field 1's VALUE (empty
            // string), each following term starts with its separator comma — same shape as
            // the critical-question row above.
            var aifRow =
                "" +                               // AIF_skosDirectRef (empty — no AIF correspondence)
                "," +                              // AIF_skosExceptionRef (empty)
                ",\"imite la défiance méthodique envers les affirmations — la dévaluation généralisée tient lieu d'examen\"" +
                ",";                               // AIF_skosOther (empty — declared gaps only)

            var csv = FallacyRequiredHeader + "," + aifCols + "\n" + FallacyRequiredRow + "," + aifRow + "\n";

            var fallacies = Fallacy.LoadFromContent(csv);

            fallacies.Should().ContainSingle();
            var f = fallacies[0];
            f.AIFImitatedScheme.Should().Be(
                "imite la défiance méthodique envers les affirmations — la dévaluation généralisée tient lieu d'examen",
                "the quoted FR prose must map verbatim, RFC4180 unquoted by CsvHelper");
            f.AIFSkosDirectRef.Should().BeEmpty("empty cell maps to empty string, not null (CsvHelper contract)");
            f.AIFSkosExceptionRef.Should().BeEmpty();
            f.AIFSkosOther.Should().BeEmpty(
                "an imitation is NOT a declared gap — skosOther stays empty so the census buckets stay disjoint");
        }

        /// <summary>
        /// #498 grains 12/13 — AIF_imitatedScheme is .Optional(): a header set without it
        /// (a pre-grain-12 105-col CSV, or a harvest header) must load without throwing and
        /// leave the property null. Losing the .Optional() flag would make HeaderValidated
        /// throw on every base predating the column — the exact fragility class this file
        /// pins (same shape as Fallacy_LoadFromContent_AifCriticalQuestionAbsent_DoesNotThrow).
        /// Re-instated by grain 13: the #2239 re-derivation had dropped this guard.
        /// </summary>
        [Fact]
        public void Fallacy_LoadFromContent_AifImitatedSchemeAbsent_DoesNotThrow()
        {
            var csv = FallacyRequiredHeader + ",AIF_skosExceptionRef,AIF_skosOther\n" +
                      FallacyRequiredRow + ",,\n";

            var act = () => Fallacy.LoadFromContent(csv);

            act.Should().NotThrow();
            var fallacies = act().Should().Subject;
            fallacies.Should().ContainSingle();
            fallacies.First().AIFImitatedScheme.Should().BeNull(
                "AIF_imitatedScheme is Optional and absent → null, not throw");
        }

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
