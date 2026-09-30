using System;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter.Ontology;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// #1651 — every IRI fragment the generators mint from a corpus title must be legal under
    /// RFC 3987 (<c>ifragment</c>), checked AT THE GENERATOR, over BOTH corpora.
    ///
    /// <para><b>Why generator-level and not file-level</b>: the two served ontologies are only
    /// refreshed by the #1525 re-derivation, so an organ anchored on them would be red by
    /// construction until then. <see cref="OwlIriFragmentValidityTests"/> reads them on purpose and
    /// polices raw spaces only (its docstring says so).</para>
    ///
    /// <para><b>The defect it closes</b>: PK 1368's English title minted <c>calling"Cards"</c> — an
    /// ASCII quote is FORBIDDEN in an IRI fragment — and that fragment IS in the served ontology
    /// today (Class + NamedIndividual). The corpus title is now 'Calling “cards”' (typographic,
    /// path 7.3.1.3): stripping the quotes AFTER Camelize() would mint <c>callingcards</c> — quote
    /// gone, but the word's capital swallowed by the quote Camelize tried to uppercase (measured on
    /// the merged tree, review of #1661) — so the strip runs BEFORE Camelize() and mints
    /// <c>callingCards</c>.</para>
    ///
    /// <para><b>Scope</b>: the corpus-derived fragments (Fallacies <c>text_en</c> → TextEn, Virtues
    /// <c>title_en</c> → TitleEn) — the inputs the generators feed to GetId
    /// (<c>OwlDocumentConfig.GetId</c> on <c>Fallacy.TextEn</c>, <c>VirtueOwlDocumentConfig.GetId</c>
    /// on <c>Virtue.TitleEn</c>). The generators' other GetId input, the AIF scheme mapping of the
    /// goodTenorOf block, is not a corpus title and stays covered on the served file by the #951
    /// space organ.</para>
    ///
    /// <para><b>Collection</b>: it joins <see cref="PublishedOntologyCollection"/> because its prose
    /// names docs/ontology/ — the guard matches text, not I/O — and keeping that rule absolute is
    /// what stops a fourth reader from reintroducing the CI-only IOException race. The organ itself
    /// reads the two CSVs and nothing else.</para>
    /// </summary>
    [Collection(PublishedOntologyCollection.Name)]
    public class OwlIriFragmentGrammarTests
    {
        private static string FallaciesCsv => Path.Combine(
            TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

        private static string VirtuesCsv => Path.Combine(
            TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

        // ── RFC 3987 §2.2/§2.3 ───────────────────────────────────────────────────────────────────
        //   ifragment  = *( ipchar / "/" / "?" )
        //   ipchar     = iunreserved / pct-encoded / sub-delims / ":" / "@"
        //   sub-delims = "!" / "$" / "&" / "'" / "(" / ")" / "*" / "+" / "," / ";" / "="
        //   iunreserved = ALPHA / DIGIT / "-" / "." / "_" / "~" / ucschar
        // ucschar is the non-ASCII allowance an IRI keeps over a URI (accents, en-dashes, CJK): legal,
        // and the corpus relies on it (naïveRealism, peak–endRule). An ASCII '"' belongs to no class —
        // that is the whole point of the organ.
        private static readonly (int Low, int High)[] UcscharRanges =
        {
            (0x00A0, 0xD7FF), (0xF900, 0xFDCF), (0xFDF0, 0xFFEF), (0x10000, 0x1FFFD),
        };

        private static readonly string SubDelims = "!$&'()*+,;=";

        /// <summary>
        /// True when <paramref name="fragment"/> conforms to <c>ifragment</c>. Empty is rejected: it is
        /// grammatically legal, but as a MINTED identifier it would collide every quote-only title.
        /// </summary>
        private static bool IsLegalIfragment(string fragment)
            => !string.IsNullOrEmpty(fragment) && fragment.All(IsIpharSlashOrQuestion);

        private static bool IsIpharSlashOrQuestion(char c)
        {
            if (c == '/' || c == '?')
            {
                return true;
            }
            if (c < 0x80)
            {
                return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')
                       || "-._~:@".IndexOf(c) >= 0 || SubDelims.IndexOf(c) >= 0;
            }
            return UcscharRanges.Any(r => c >= r.Low && c <= r.High);
        }

        // ── The inverse control on the INSTRUMENT ────────────────────────────────────────────────
        [Fact]
        public void Validator_RejectsTheFragmentPk1368Shipped()
        {
            IsLegalIfragment("calling\"Cards\"").Should().BeFalse(
                "an ASCII quote is forbidden in an IRI fragment (RFC 3987) — this is verbatim the " +
                "fragment docs/ontology/argumentum.owl ships for PK 1368. If this assertion flips, the " +
                "validator can no longer turn red and every check below is vacuous.");

            IsLegalIfragment("callingCards").Should().BeTrue(
                "the letter-only control must pass, otherwise the validator is rejecting everything");

            IsLegalIfragment("appelàLautorité").Should().BeTrue(
                "accents are ucschar — legal in an IRI — and the corpus relies on them");

            IsLegalIfragment("").Should().BeFalse(
                "an empty minted identifier would collide every quote-only title");
        }

        // ── The invariant over both corpora ──────────────────────────────────────────────────────
        [Fact]
        public void FallaciesEnTitles_MintLegalFragments()
            => EveryEnglishTitle_MintsALegalFragment(FallaciesCsv, "text_en", OwlDocumentConfig.GetId, 1000);

        [Fact]
        public void VirtuesEnTitles_MintLegalFragments()
            => EveryEnglishTitle_MintsALegalFragment(VirtuesCsv, "title_en", VirtueOwlDocumentConfig.GetId, 200);

        private static void EveryEnglishTitle_MintsALegalFragment(
            string csvPath, string column, Func<string, string> getId, int antiVacuityFloor)
        {
            var titles = new HarvestCardIdsCsv(csvPath).LoadColumn(column);

            titles.Should().HaveCountGreaterThan(antiVacuityFloor,
                "an empty/header-only read would make this organ degenerate to 0 == 0 (#1046)");

            var offenders = titles
                .Select(title => new { Title = title, Id = getId(title) })
                .Where(x => !IsLegalIfragment(x.Id))
                .Select(x => $"'{x.Title}' -> '{x.Id}'")
                .ToList();

            offenders.Should().BeEmpty(
                "every English title must mint an RFC 3987 ifragment. Offenders: {0}",
                string.Join(" | ", offenders));
        }

        // ── The strip itself — RED exactly when the #1651 fix is absent ───────────────────────────
        [Theory]
        [InlineData("Calling \"Cards\"")]        // the ASCII form the served ontology shipped
        [InlineData("Calling “cards”")]          // PK 1368's current corpus title (path 7.3.1.3)
        [InlineData("« Guillemets »")]           // French quoting
        [InlineData("A \"B\" «C» “D”")]          // mixed forms in one title
        public void Quotes_StrippedFromTheMintedFragment(string title)
        {
            var id = OwlDocumentConfig.GetId(title);

            id.Should().NotContain("\"").And.NotContain("“").And.NotContain("”")
                .And.NotContain("«").And.NotContain("»");

            IsLegalIfragment(id).Should().BeTrue(
                "stripping the quotes must leave a legal fragment, got '{0}'", id);
        }

        [Theory]
        [InlineData("Calling “cards”")]          // PK 1368's CURRENT corpus title (path 7.3.1.3) — the
                                                 // form the #1525 regeneration will actually mint
        [InlineData("Calling \"Cards\"")]        // the ASCII form the served ontology shipped
        public void Pk1368_BothQuoteForms_MintCallingCards(string title)
        {
            // Pinned on BOTH forms because they split the labor differently: under the pre-review
            // chain (quotes stripped AFTER Camelize) the ASCII form still yielded 'callingCards' — its
            // capital C comes from the input — while the current typographic title yielded
            // 'callingcards' (measured on the merged tree, review of #1661): Camelize uppercases the
            // character following the space, which is the quote '“', not the 'c'. Only the pin on the
            // CURRENT title reddens when the strip moves back after Camelize().
            OwlDocumentConfig.GetId(title).Should().Be("callingCards",
                "PK 1368's IRI is ...#callingCards at the #1525 regeneration (#1651), whichever quote " +
                "form the title carries — the quote strip must run before Camelize");
        }

        [Fact]
        public void VirtuesTransform_MirrorsTheFallaciesQuoteStrip()
        {
            // VirtueOwlGenerationContractTests already pins the two transforms as byte-identical on
            // apostrophes/hyphens/commas; the quote strip must not create a divergence there.
            foreach (var title in new[] { "Calling \"Cards\"", "Calling “cards”", "« Guillemets »" })
            {
                VirtueOwlDocumentConfig.GetId(title).Should().Be(
                    OwlDocumentConfig.GetId(title),
                    "the two GetId transforms are pinned byte-identical; '{0}' must not diverge", title);
            }
        }
    }
}