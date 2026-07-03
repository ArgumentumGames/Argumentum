using System;
using System.IO;
using Argumentum.AssetConverter;
using FluentAssertions;
using Spectre.Console;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Utility
{
    /// <summary>
    /// Regression tests for issue #630: a message containing square brackets (e.g. the
    /// <c>[HARVEST-FAILURE]</c> marker emitted by the #614 per-set resilience path) was fed
    /// unescaped to Spectre.Console markup rendering. The StyleParser then threw
    /// <c>InvalidOperationException("Could not find color or style 'HARVEST-FAILURE'")</c>
    /// from inside the resilience catch block, killing the whole run instead of degrading
    /// gracefully.
    ///
    /// Two guarantees are pinned here:
    ///  (1) bracketed messages render literally (Markup.Escape on every console path), and
    ///  (2) Logger.Log never throws on any rendering failure (plain-text fallback), because
    ///      #614 calls it from catch paths where a throw is fatal to the run.
    /// </summary>
    public class LoggerMarkupSafetyTests
    {
        /// <summary>
        /// Routes AnsiConsole to a plain StringWriter for the duration of a test so output can
        /// be asserted, then restores the previous console.
        /// </summary>
        private static string CaptureConsole(Action action)
        {
            var writer = new StringWriter();
            var console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.No,
                ColorSystem = ColorSystemSupport.NoColors,
                Interactive = InteractionSupport.No,
                Out = new AnsiConsoleOutput(writer),
            });
            var previous = AnsiConsole.Console;
            AnsiConsole.Console = console;
            try
            {
                action();
            }
            finally
            {
                AnsiConsole.Console = previous;
            }
            return writer.ToString();
        }

        [Fact]
        public void Log_Problem_WithHarvestFailureMarker_DoesNotThrow_AndRendersLiterally()
        {
            // Exact shape emitted by HarvestManager's #614 resilience path.
            var message = "[HARVEST-FAILURE] Card set 'FallaciesTarot' / 'fr' failed and was skipped: boom";

            string output = null;
            var act = () => { output = CaptureConsole(() => Logger.Log(message, MessageType.Problem)); };

            act.Should().NotThrow("a bracketed failure marker must not be parsed as Spectre style markup (#630)");
            output.Should().Contain("[HARVEST-FAILURE]", "the marker must survive rendering literally for log greppability");
        }

        [Theory]
        [InlineData(MessageType.Title)]
        [InlineData(MessageType.Problem)]
        [InlineData(MessageType.Instructions)]
        [InlineData(MessageType.Explanations)]
        [InlineData(MessageType.Warning)]
        [InlineData(MessageType.Success)]
        [InlineData(MessageType.Info)]
        public void Log_AnyMessageType_WithBracketedContent_DoesNotThrow(MessageType messageType)
        {
            // Brackets show up in real messages: failure markers, file paths, exception text,
            // Mustache/template fragments quoted in diagnostics.
            var message = "path [C:\\x] marker [HARVEST-FAILURE] template {{field}} style [bold red]";

            var act = () => CaptureConsole(() => Logger.Log(message, messageType));

            act.Should().NotThrow($"no console rendering path may propagate for {messageType} (#630)");
        }

        [Fact]
        public void Log_InvalidMessageType_StillThrowsArgumentOutOfRange()
        {
            // The render-failure fallback must not swallow the guard clause for invalid enums.
            var act = () => CaptureConsole(() => Logger.Log("x", (MessageType)999));

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void LogException_WithBracketsInExceptionMessage_DoesNotThrow()
        {
            var ex = new InvalidOperationException("outer [HARVEST-FAILURE]",
                new IOException("inner [bold red] not-a-style"));

            var act = () => CaptureConsole(() => Logger.LogException(ex));

            act.Should().NotThrow("exception messages routinely contain brackets and are rendered on failure paths");
        }
    }
}
