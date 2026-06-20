using System.Linq;
using Argumentum.AssetConverter.Entities;
using CsvHelper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Parsing
{
    /// <summary>
    /// Regression tests for <see cref="TestFallacyCardClassMap"/> via the real load path
    /// <see cref="CsvBase{T,TMap}.LoadFromContent"/>. <see cref="TestFallacyCard"/> is the simplified
    /// fallacy entity used for visual-test card generation — the LAST entity ClassMap without a
    /// dedicated regression suite (this PR closes the ClassMap matrix at 100%: Fallacy #485,
    /// Rule/ArgumentVirtue/Scenario #476, Virtue #559, DnnUiString, and now TestFallacyCard).
    ///
    /// The ClassMap declares 5 columns, ALL required (non-<c>.Optional()</c>): Id, Title, Type,
    /// IllustrationPath, Description. The guards these tests pin:
    /// <list type="bullet">
    /// <item><description>All 5 columns map to their properties (a header-name drift = silent null
    /// properties = empty visual-test cards).</description></item>
    /// <item><description><c>GetId()</c> returns <c>Id</c> — the key used by the image generator's
    /// output dictionary; if the binding breaks, cards have no key.</description></item>
    /// <item><description>A required column absent from the header throws
    /// <see cref="HeaderValidationException"/> (CsvHelper strict default — the contract that
    /// prevents silent data loss).</description></item>
    /// <item><description>A short data row (fewer cells than the header) does NOT throw —
    /// <c>MissingFieldFound</c> is a log callback in <see cref="CsvBase{T,TMap}.LoadFromContent"/>,
    /// so a ragged row logs and yields missing fields as null (the documented
    /// <c>CsvBaseStrictContractTests</c> behavior).</description></item>
    /// </list>
    ///
    /// Additive only: no existing test or production code is modified. Inline CSV keeps each test
    /// self-contained. Companion to <c>VirtueClassMapRegressionTests</c> (#559) and
    /// <c>EntityClassMapRegressionTests</c> (#476).
    /// </summary>
    public class TestFallacyCardClassMapRegressionTests
    {
        private const string Header = "Id,Title,Type,IllustrationPath,Description";
        private const string FullRow = "card-1,Ad Hominem,fallacy,/img/adhominem.png,Attacking the person";

        /// <summary>
        /// All 5 required columns map to their properties.
        /// </summary>
        [Fact]
        public void TestFallacyCard_LoadFromContent_MapsAllColumns()
        {
            var csv = Header + "\n" + FullRow + "\n";

            var cards = TestFallacyCard.LoadFromContent(csv);

            cards.Should().ContainSingle();
            var c = cards[0];
            c.Id.Should().Be("card-1");
            c.Title.Should().Be("Ad Hominem");
            c.Type.Should().Be("fallacy");
            c.IllustrationPath.Should().Be("/img/adhominem.png");
            c.Description.Should().Be("Attacking the person");
        }

        /// <summary>
        /// GetId() returns Id — the key for the image generator's output dictionary.
        /// </summary>
        [Fact]
        public void TestFallacyCard_LoadFromContent_GetIdReturnsId()
        {
            var csv = Header + "\n" + FullRow + "\n";

            var cards = TestFallacyCard.LoadFromContent(csv);

            cards.Should().ContainSingle();
            cards[0].GetId().Should().Be("card-1");
        }

        /// <summary>
        /// A required column absent from the header must throw HeaderValidationException — all 5
        /// TestFallacyCard columns are non-Optional, so the strict default applies. This is the
        /// contract that prevents silent data loss: a column drift fails loud, not silent.
        /// </summary>
        [Fact]
        public void TestFallacyCard_LoadFromContent_RequiredColumnAbsent_Throws()
        {
            // Drop "Description" (a required column) from the header.
            var csv = "Id,Title,Type,IllustrationPath\n" + FullRow + "\n";

            var act = () => TestFallacyCard.LoadFromContent(csv);

            act.Should().Throw<HeaderValidationException>(
                "Description is a non-Optional column; its absence must fail loud, not silently null the property");
        }

        /// <summary>
        /// A short data row (fewer cells than the header) does NOT throw — MissingFieldFound is a
        /// log callback in LoadFromContent, so a ragged row yields null for the missing fields
        /// instead of throwing. This mirrors the CsvBaseStrictContractTests short-row contract.
        /// </summary>
        [Fact]
        public void TestFallacyCard_LoadFromContent_ShortRow_DoesNotThrow()
        {
            // Header has 5 columns; the row supplies only the first 3.
            var csv = Header + "\n" + "card-1,Ad Hominem,fallacy\n";

            var act = () => TestFallacyCard.LoadFromContent(csv);

            act.Should().NotThrow();
            var cards = TestFallacyCard.LoadFromContent(csv);
            cards.Should().ContainSingle();
            cards[0].Id.Should().Be("card-1");
            cards[0].Title.Should().Be("Ad Hominem");
            cards[0].Description.Should().BeEmpty(
                "the short row leaves Description unset — CsvHelper yields string.Empty for the missing " +
                "field (MissingFieldFound logs, does not throw). Not null: the string default is \"\"");
        }

        /// <summary>
        /// Multiple rows load in order.
        /// </summary>
        [Fact]
        public void TestFallacyCard_LoadFromContent_MultipleRows_LoadInOrder()
        {
            var csv =
                Header + "\n" +
                "card-1,Ad Hominem,fallacy,/img/1.png,Desc 1\n" +
                "card-2,Straw Man,fallacy,/img/2.png,Desc 2\n";

            var cards = TestFallacyCard.LoadFromContent(csv);

            cards.Should().HaveCount(2);
            cards[0].Id.Should().Be("card-1");
            cards[1].Id.Should().Be("card-2");
        }
    }
}
