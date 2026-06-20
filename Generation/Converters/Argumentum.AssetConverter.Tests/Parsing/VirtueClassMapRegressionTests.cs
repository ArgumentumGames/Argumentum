using System.Linq;
using Argumentum.AssetConverter.Entities;
using CsvHelper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Parsing
{
    /// <summary>
    /// Regression tests for <see cref="VirtueClassMap"/> via the real load path
    /// <see cref="CsvBase{T,TMap}.LoadFromContent"/>. Virtue is the taxonomy entity for the
    /// Virtues lane (#499 scale-up, 223 nodes / 194 leaves) and its ClassMap declares 16 required
    /// FR columns (lines 75-92 : pk, path, depth, decimal_path_padded, family_fr,
    /// subfamily_fr, subsubfamily_fr, title_fr, description_fr, remark_fr, link_fr,
    /// family_fr_camelcase, depth_max4, card, update, locked) plus 28 localized columns
    /// (en/ru/pt/es × 7) all <c>.Optional()</c>.
    ///
    /// The critical guards these tests pin:
    /// <list type="bullet">
    /// <item><description>The 16 required FR columns must all resolve (a header drift here = silent
    /// data loss on Virtue generation, same fragility class as the #216/#477 localization
    /// regression).</description></item>
    /// <item><description>The 28 optional localized columns may be absent without throwing
    /// (<c>.Optional()</c> contract — a column losing its Optional flag would make
    /// <c>HeaderValidated</c> throw when the harvest loads a FR-only header set).</description></item>
    /// <item><description>The double map of <c>pk</c> → <c>Id</c> AND <c>Pk</c> (both Name "pk"):
    /// <c>GetId()</c> returns <c>Id</c>, which must resolve from the <c>pk</c> column.</description></item>
    /// </list>
    ///
    /// Additive only: no existing test or production code is modified. Inline CSV keeps each
    /// test self-contained (no external fixture files). Companion to
    /// <c>FallacyClassMapRegressionTests</c> and <c>EntityClassMapRegressionTests</c> — Virtue
    /// was the remaining uncovered entity ClassMap (alongside <c>TestFallacyCard</c>, which is
    /// visual-test-only and lower-stakes).
    /// </summary>
    public class VirtueClassMapRegressionTests
    {
        // Every non-Optional VirtueClassMap column (Virtue.cs:75-92). HeaderValidated requires all
        // of these in the header; every localized column (en/ru/pt/es × 7 = 28) is .Optional().
        private const string VirtueRequiredHeader =
            "pk,path,depth,decimal_path_padded,family_fr,subfamily_fr,subsubfamily_fr," +
            "title_fr,description_fr,remark_fr,link_fr,family_fr_camelcase,depth_max4," +
            "card,update,locked";

        // A representative FR-only data row matching VirtueRequiredHeader (16 fields).
        // depth (3rd field) is an int property → the cell must be int-parseable.
        private const string VirtueRequiredRow =
            "1.1,1.1,1,1.1,Arguments vertueux,Argument pertinent,,Présentation claire," +
            "Desc FR,Remarque FR,https://fr,argumentsVertueux,1,1,2024-01-01,false";

        /// <summary>
        /// Loads a representative Virtue row (required FR columns + a couple of localized columns)
        /// and asserts each required column maps to its property.
        /// </summary>
        [Fact]
        public void Virtue_LoadFromContent_MapsRequiredColumns()
        {
            var csv =
                VirtueRequiredHeader + ",title_en,title_pt\n" +
                VirtueRequiredRow + ",Clear presentation,Apresentação clara\n";

            var virtues = Virtue.LoadFromContent(csv);

            virtues.Should().ContainSingle();
            var v = virtues[0];
            v.Pk.Should().Be("1.1");
            v.Path.Should().Be("1.1");
            v.Depth.Should().Be(1);           // depth is an int property, parsed from the "depth" column
            v.DecimalPathPadded.Should().Be("1.1");
            v.FamilyFr.Should().Be("Arguments vertueux");
            v.SubfamilyFr.Should().Be("Argument pertinent");
            v.SubsubfamilyFr.Should().Be(""); // empty cell in the inline row
            v.TitleFr.Should().Be("Présentation claire");
            v.DescriptionFr.Should().Be("Desc FR");
            v.RemarkFr.Should().Be("Remarque FR");
            v.LinkFr.Should().Be("https://fr");
            v.FamilyFrCamelcase.Should().Be("argumentsVertueux");
            v.DepthMax4.Should().Be("1");
            v.Card.Should().Be("1");
            v.Update.Should().Be("2024-01-01");
            v.Locked.Should().Be("false");
        }

        /// <summary>
        /// The single "pk" column populates BOTH the Pk property (Name "pk") and the Id property
        /// (Name "pk"), so GetId() (which returns Id) resolves. This double-map is a silent-failure
        /// source if either binding is dropped: GetId() would return null, breaking any keyed
        /// generation pass.
        /// </summary>
        [Fact]
        public void Virtue_LoadFromContent_PkPopulatesBothIdAndPk()
        {
            var csv = VirtueRequiredHeader + "\n" + VirtueRequiredRow + "\n";

            var virtues = Virtue.LoadFromContent(csv);

            virtues.Should().ContainSingle();
            virtues[0].Pk.Should().Be("1.1");
            virtues[0].Id.Should().Be("1.1", "the pk column is mapped to both Id and Pk");
            virtues[0].GetId().Should().Be("1.1");
        }

        /// <summary>
        /// The localized columns (en/ru/pt/es) are all .Optional() — a FR-only header set must
        /// load without throwing. A column losing its .Optional() flag would make HeaderValidated
        /// throw, silently breaking the multilingual harvest.
        /// </summary>
        [Fact]
        public void Virtue_LoadFromContent_OptionalLocalizedColumnsAbsent_DoesNotThrow()
        {
            // Only the 16 required FR columns — no en/ru/pt/es at all.
            var csv = VirtueRequiredHeader + "\n" + VirtueRequiredRow + "\n";

            var act = () => Virtue.LoadFromContent(csv);

            act.Should().NotThrow();
            var virtues = Virtue.LoadFromContent(csv);
            virtues.Should().ContainSingle();
            virtues[0].TitleEn.Should().BeNull("title_en is Optional and absent → null, not throw");
            virtues[0].TitlePt.Should().BeNull();
            virtues[0].FamilyEs.Should().BeNull();
        }

        /// <summary>
        /// When localized columns ARE present, they map to their properties (Optional just means
        /// "may be absent", not "ignored when present").
        /// </summary>
        [Fact]
        public void Virtue_LoadFromContent_OptionalLocalizedColumnsPresent_MapsThem()
        {
            var csv =
                VirtueRequiredHeader + ",title_en,title_pt,family_es\n" +
                VirtueRequiredRow + ",Clear presentation,Apresentação clara,Virtudes\n";

            var virtues = Virtue.LoadFromContent(csv);

            virtues.Should().ContainSingle();
            virtues[0].TitleEn.Should().Be("Clear presentation");
            virtues[0].TitlePt.Should().Be("Apresentação clara");
            virtues[0].FamilyEs.Should().Be("Virtudes");
        }

        /// <summary>
        /// A required FR column absent from the header must throw HeaderValidationException —
        /// CsvHelper's default (VirtueClassMap does not set HeaderValidated, so the strict default
        /// applies). This is the contract that prevents silent data loss: a required column drift
        /// fails loud, not silent.
        /// </summary>
        [Fact]
        public void Virtue_LoadFromContent_RequiredColumnAbsent_Throws()
        {
            // Drop "title_fr" (a required FR column) from the header AND the row.
            var header = VirtueRequiredHeader.Replace(",title_fr", "");
            var row = VirtueRequiredRow; // row has one extra field, but MissingFieldFound logs-not-throws
            var csv = header + "\n" + row + "\n";

            var act = () => Virtue.LoadFromContent(csv);

            act.Should().Throw<HeaderValidationException>(
                "title_fr is a non-Optional column; its absence must fail loud, not silently null the property");
        }
    }
}
