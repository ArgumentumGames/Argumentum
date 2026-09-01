using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// #951 follow-up organ: every IRI fragment minted into the two committed ontologies must be
    /// free of raw spaces — a space is invalid in an IRI fragment, and #133 (publication) can fire
    /// at any moment since its gate opened on 27/08. The 14 Walton-scheme IRIs of
    /// argumentum_virtues.owl shipped with spaces because the AIF_skosDirectRef scheme names were
    /// concatenated raw in the goodTenorOf block (VirtueOwlGeneratorConfig); they are now routed
    /// through GetId like every other fragment we mint. This organ holds the line against the next
    /// raw-name path.
    /// Deliberate non-goal: it does NOT police characters that are legal in IRI fragments —
    /// hyphens, dots, slashes survive (AifNode's canonical AIF names I-node/RA-node keep theirs on
    /// purpose, and GetId's historical ./ leftovers are legal, if inelegant).
    /// </summary>
    [Collection(PublishedOntologyCollection.Name)]
    public class OwlIriFragmentValidityTests
    {
        [Theory]
        [InlineData("argumentum.owl")]
        [InlineData("argumentum_virtues.owl")]
        public void CommittedOntology_IriFragments_CarryNoRawSpace(string owlFileName)
        {
            var owlPath = Path.Combine(TestRepoRoot.Find(), "docs", "ontology", owlFileName);
            File.Exists(owlPath).Should().BeTrue(
                $"the committed ontology '{owlFileName}' must exist — a missing file must fail the organ, " +
                "not slip past it");

            var content = File.ReadAllText(owlPath);
            var fragments = new HashSet<string>(StringComparer.Ordinal);
            var index = 0;
            while ((index = content.IndexOf("IRI=\"", index, StringComparison.Ordinal)) >= 0)
            {
                var end = content.IndexOf('"', index + 5);
                if (end < 0)
                {
                    break;
                }
                var iri = content.Substring(index + 5, end - index - 5);
                var hashIndex = iri.LastIndexOf('#');
                if (hashIndex >= 0 && hashIndex + 1 < iri.Length)
                {
                    fragments.Add(iri.Substring(hashIndex + 1));
                }
                index = end;
            }

            fragments.Count.Should().BeGreaterThan(0,
                "the ontology must mint at least one fragment, otherwise this organ degenerates to " +
                "0 == 0 (the #1046 no-op guard)");

            var withSpace = fragments.Where(f => f.Contains(' '))
                .OrderBy(f => f, StringComparer.Ordinal).ToList();
            withSpace.Should().BeEmpty(
                "a raw space is invalid in an IRI fragment and #133 can publish at any moment. " +
                "Offenders: {0}",
                string.Join(", ", withSpace));
        }
    }
}
