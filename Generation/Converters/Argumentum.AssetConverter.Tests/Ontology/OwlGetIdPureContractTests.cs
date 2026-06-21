using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// Contract pin for <see cref="OwlDocumentConfig.GetId"/> — #204 coverage sweep (cont. po-2024):
    /// the OWL IRI-fragment transform contract.
    ///
    /// The OWL generator turns each fallacy's display name into the fragment identifier of its concept
    /// IRI (e.g. <c>https://www.argumentum.games/...#AdHominem</c>). That transform is
    /// <see cref="OwlDocumentConfig.GetId"/>: Humanizer <c>Camelize()</c> (PascalCase join) then strip
    /// apostrophes, hyphens and commas — characters forbidden in IRI fragments. A bug here produces
    /// duplicate or invalid concept IRIs, which silently corrupts the generated ontology (collision,
    /// or a fragment that breaks IRI resolution). It is pure &amp; deterministic — no I/O, no config
    /// state — yet had ZERO isolated coverage (the E2E ontology tests exercise it only indirectly).
    /// These tests pin the contract additively.
    /// </summary>
    public class OwlGetIdPureContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────────
        // (1) Camelize — Humanizer Camelize() produces camelCase (first segment lowercased, subsequent
        //     segments kept/uppercased-on-first-letter) and joins the words into a single token. The
        //     exact outputs below were captured from the real Humanizer 2.14.1 transform.
        // ─────────────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("Ad Hominem", "adHominem")]
        [InlineData("straw man", "strawMan")]
        [InlineData("red herring", "redHerring")]
        [InlineData("Hasty Generalization", "hastyGeneralization")]
        public void Camelize_JoinsWordsCamelCase_NoSpaces(string input, string expected)
        {
            OwlDocumentConfig.GetId(input).Should().Be(expected,
                "Camelize() joins the words into a single camelCase token, dropping the spaces.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) Apostrophes stripped — French elisions (l', d', qu') would otherwise land in the IRI.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Apostrophes_Stripped()
        {
            // "L'appel" → Camelize "l'appel" → apostrophe stripped → "lappel".
            OwlDocumentConfig.GetId("L'appel").Should().Be("lappel",
                "apostrophes are stripped after Camelize so French elisions don't leak into the IRI fragment.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) Hyphens stripped — compound names ("A-B") would create an invalid fragment otherwise.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Hyphens_Stripped()
        {
            // "A-B" → Camelize "a-b" → hyphen stripped → "aB".
            OwlDocumentConfig.GetId("A-B").Should().Be("aB",
                "hyphens are stripped so compound names produce a valid IRI fragment.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (4) Commas stripped.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Commas_Stripped()
        {
            // "A, B" → Camelize "a, b" → comma stripped → "aB".
            OwlDocumentConfig.GetId("A, B").Should().Be("aB",
                "commas are stripped so list-style names produce a valid IRI fragment.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (5) End-to-end on a realistic French fallacy name — the canonical use case. NOTE: Humanizer
        //     Camelize() PRESERVES accented characters (it does not ASCII-fold), so "à" survives and is
        //     uppercased to "À" at the segment start. This is the OBSERVED contract — the fragment is
        //     apostrophe/hyphen/comma/space-free but may contain accented letters. Pinned exactly so a
        //     future Humanizer upgrade (or an added ASCII-fold) that changes accent handling is caught.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void FrenchFallacyName_ProducesFragment_AccentsPreserved()
        {
            // "Appel à l'autorité" → Camelize "appel À l'autorité" → apostrophe stripped → "appelÀLautorité".
            OwlDocumentConfig.GetId("Appel à l'autorité").Should().Be("appelÀLautorité",
                "Camelize joins the words camelCase, preserves the accented 'à' (uppercased to 'À' at the " +
                "segment start), and the trailing Replace chain strips only apostrophes/hyphens/commas.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (6) DETERMINISM — same input always yields the same output (pure function).
        //     Guards against a refactor that introduces state/time/randomness.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Deterministic_SameInput_SameOutput()
        {
            const string input = "Hasty Generalization";
            var first = OwlDocumentConfig.GetId(input);
            var second = OwlDocumentConfig.GetId(input);

            first.Should().Be(second, "GetId is a pure function — identical inputs produce identical fragments.");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (7) NO forbidden characters remain — the generic invariant the OWL generator relies on.
        //     Regardless of input, the fragment must never contain ', -, or ,.
        // ─────────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Result_NeverContainsForbiddenChars()
        {
            var inputs = new[]
            {
                "Ad Hominem", "L'appel à l'autorité", "A-B, C", "red-herring",
                "tu, quoque", "Slippery Slope", "post hoc", "Ménage à trois",
            };

            foreach (var input in inputs)
            {
                var id = OwlDocumentConfig.GetId(input);
                id.Should().NotContain("'", $"fragment for '{input}' must be apostrophe-free");
                id.Should().NotContain("-", $"fragment for '{input}' must be hyphen-free");
                id.Should().NotContain(",", $"fragment for '{input}' must be comma-free");
            }
        }
    }
}
