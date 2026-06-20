using System.IO;
using System.Linq;
using Argumentum.AssetConverter;
using Argumentum.AssetConverter.Entities;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Entities
{
    /// <summary>
    /// Contract pins for two pure, zero-coverage helpers surfaced by the #204 coverage sweep
    /// (cont. po-2024, tertiaire): <see cref="Virtue.Carte"/> (the only int.TryParse guard in the
    /// entity layer) and <see cref="UtilityExtensions.RemoveInvalidFileNameChars"/> (the OS-invalid
    /// filename char filter). Both are already standalone &amp; pure — these tests just pin their
    /// contracts additively.
    /// </summary>
    public class VirtueCarteAndFileNamePureContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // Virtue.Carte — int.TryParse guard. Returns the parsed card number, or null on ANY parse
        // failure (empty, whitespace, non-numeric, null). This is the ONLY parsing/dtype logic in
        // the whole entity layer; downstream code relies on null meaning "no card assigned" vs an
        // explicit card 0. A regression to int.Parse (throws) or to a hardcoded 0 (phantom cards)
        // would silently corrupt card-number detection.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("7", 7)]
        [InlineData("0", 0)]          // explicit zero is a real card number, distinct from null
        [InlineData("42", 42)]
        public void Carte_NumericCard_ParsesToInt(string card, int expected)
        {
            var virtue = new Virtue { Card = card };
            virtue.Carte.Should().Be(expected, $"Card='{card}' parses to a real card number.");
        }

        [Theory]
        [InlineData("")]              // empty CSV cell
        [InlineData(" ")]             // whitespace
        [InlineData("N/A")]           // non-numeric placeholder
        [InlineData("card 7")]        // text + number
        public void Carte_UnparseableCard_YieldsNull(string card)
        {
            var virtue = new Virtue { Card = card };
            virtue.Carte.Should().BeNull(
                $"Card='{card}' cannot be parsed as an int, so Carte is null — meaning 'no card number " +
                $"assigned', NOT card 0.");
        }

        [Fact]
        public void Carte_NullCard_YieldsNull()
        {
            // Card is a string column; a null (unset) value must also yield null, not throw.
            // int.TryParse(null, ...) returns false, so the guard handles this without NRE.
            var virtue = new Virtue { Card = null };
            virtue.Carte.Should().BeNull("a null Card does not crash the TryParse guard and yields null.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // UtilityExtensions.RemoveInvalidFileNameChars — strips every char in
        // Path.GetInvalidFileNameChars() from a filename. Used defensively by ImageHelper to keep
        // generated image paths valid on the current OS. A regression that kept invalid chars
        // (or stripped valid ones) would ship broken filenames. Pinned against the live OS set.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void RemoveInvalidFileNameChars_StripsOsInvalidChars_KeepsRest()
        {
            // '\n' and '|' are invalid filename chars on Windows; 'file', 'name', '.', 'txt' are valid.
            var cleaned = "file\n|name.txt".RemoveInvalidFileNameChars();
            cleaned.Should().Be("filename.txt",
                "the newline and pipe (both OS-invalid) are stripped, the rest is kept verbatim.");
        }

        [Fact]
        public void RemoveInvalidFileNameChars_CleanString_Unchanged()
        {
            var cleaned = "normal_filename-1.txt".RemoveInvalidFileNameChars();
            cleaned.Should().Be("normal_filename-1.txt",
                "a filename with no invalid characters is returned unchanged.");
        }

        [Fact]
        public void RemoveInvalidFileNameChars_ResultNeverContainsOsInvalidChar()
        {
            // Generic invariant: regardless of input, the output contains ZERO chars from the live
            // OS invalid set. This is the actual contract ImageHelper relies on.
            var invalid = Path.GetInvalidFileNameChars();
            var inputs = new[] { "a\nb", "c|d<e", "ok.txt", "", "x\0y", "p:q*r?s" };

            foreach (var input in inputs)
            {
                var cleaned = input.RemoveInvalidFileNameChars();
                cleaned.Any(ch => invalid.Contains(ch)).Should().BeFalse(
                    $"output for input '{input}' must contain no OS-invalid filename character.");
            }
        }
    }
}
