// The OpenAI.Responses namespace is [Experimental("OPENAI001")] in the SDK. This test asserts the
// exact effort-level mapping, so it references the enum members directly — hence the suppression.
#pragma warning disable OPENAI001
using Argumentum.AssetConverter;
using FluentAssertions;
using OpenAI.Responses;
using Xunit;

namespace Argumentum.AssetConverter.Tests.DatasetUpdater
{
    /// <summary>
    /// Unit cover for the Responses-API path added to <see cref="Prompt"/> (issue #141 re-scope:
    /// gpt-5.x must route through /v1/responses with a capped reasoning effort — Chat Completions
    /// burns the budget on hidden reasoning and returns empty Content).
    ///
    /// The live <c>SendViaResponses</c> call needs the network, so it is not unit-testable here.
    /// What IS pure logic and load-bearing is <see cref="Prompt.ParseReasoningEffort"/>: it maps a
    /// config string to a <see cref="ResponseReasoningEffortLevel"/> and — critically — falls back
    /// to <c>Low</c> (the intended default for cost-bounded translation runs) for any unrecognised
    /// value rather than throwing. Throwing here would abort an entire translation campaign on one
    /// bad config string; that contract is worth pinning.
    ///
    /// Also pins that the Prompt Responses knobs (<c>UseResponsesApi</c>, <c>ReasoningEffort</c>)
    /// default to the legacy path (off / null) so existing gpt-4.1 behaviour is unchanged unless a
    /// task explicitly opts in — the change is additive and reversible by construction.
    /// </summary>
    public class PromptResponsesApiTests
    {
        [Theory]
        [InlineData("minimal")]
        [InlineData("low")]
        [InlineData("medium")]
        [InlineData("high")]
        public void ParseReasoningEffort_RecognisedLevel_MapsToMatchingMember(string input)
        {
            // ParseReasoningEffort must map each canonical token to its matching effort-level
            // member (compared struct-to-struct, not via ToString, because
            // ResponseReasoningEffortLevel is a custom struct whose serialized value is lowercase
            // while nameof()/ToString() disagree on casing).
            var actual = Prompt.ParseReasoningEffort(input);
            var expected = input switch
            {
                "minimal" => ResponseReasoningEffortLevel.Minimal,
                "low" => ResponseReasoningEffortLevel.Low,
                "medium" => ResponseReasoningEffortLevel.Medium,
                "high" => ResponseReasoningEffortLevel.High,
                _ => ResponseReasoningEffortLevel.Low,
            };
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("LOW")]      // upper-case
        [InlineData("  Low  ")]  // surrounding whitespace
        [InlineData("low")]      // canonical
        public void ParseReasoningEffort_IsCaseInsensitiveAndTrimsWhitespace(string input)
        {
            Prompt.ParseReasoningEffort(input)
                .Should().Be(ResponseReasoningEffortLevel.Low);
        }

        [Theory]
        [InlineData("ultra")]      // unknown level
        [InlineData("")]           // empty
        [InlineData("n/a")]        // junk
        public void ParseReasoningEffort_UnknownValue_FallsBackToLow_NotThrows(string input)
        {
            // The contract: a bad config value must NOT abort a translation campaign. It maps to
            // Low (the cost-bounded default) rather than throwing.
            var act = () => Prompt.ParseReasoningEffort(input);
            act.Should().NotThrow();
            act().Should().Be(ResponseReasoningEffortLevel.Low);
        }

        [Fact]
        public void Prompt_ResponsesApiDefaults_OffAndNull_LegacyPathPreserved()
        {
            // A freshly constructed Prompt must NOT opt into the Responses path and must NOT set a
            // reasoning effort. This guarantees the change is additive: tasks that do not set these
            // flags keep the existing Chat Completions behaviour byte-for-byte.
            var prompt = new Prompt();
            prompt.UseResponsesApi.Should().BeFalse("the Responses path is opt-in only");
            prompt.ReasoningEffort.Should().BeNull("no reasoning effort is applied unless explicitly configured");
        }
    }
}
