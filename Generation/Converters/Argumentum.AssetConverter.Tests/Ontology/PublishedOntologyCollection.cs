using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// Collection xUnit serialisant tout test qui LIT les ontologies publiees de docs/ontology/.
    ///
    /// POURQUOI : OWLSharp charge un fichier via <c>OWLOntology.FromFileAsync</c>, qui l ouvre en
    /// acces EXCLUSIF. Tant qu une seule classe touchait un fichier donne, le probleme restait
    /// invisible : xUnit serialise les tests d une meme classe. Des qu une seconde classe lit le
    /// meme artefact, les deux collections tournent en parallele et le lecteur non-exclusif se
    /// prend un IOException << used by another process >>.
    ///
    /// MESURE FONDATRICE (CI run 33285555961, build Debug, 2026-08-30) :
    ///   OwlReasonableGraphContractTests.PublishedOntology_KeepsAnnotationsAlongsideAssertions
    ///   (argumentum_virtues.owl) FAIL -- IOException a File.ReadAllText, pendant que
    ///   VirtuesAifDerivationContractTests tenait le meme fichier via FromFile. Le build Release
    ///   du MEME commit passait : l ordonnancement, pas le contenu. Le vert local etait de la
    ///   chance, pas une preuve -- d ou l organe ci-dessous plutot qu un simple retry.
    /// </summary>
    [CollectionDefinition(Name)]
    public class PublishedOntologyCollection
    {
        public const string Name = "PublishedOntology";
    }

    /// <summary>
    /// Organe : toute classe de test qui reference docs/ontology/ DOIT porter
    /// [Collection(PublishedOntologyCollection.Name)].
    ///
    /// Sans cette garde, ajouter un quatrieme lecteur reintroduit la course silencieusement -- et
    /// elle se manifeste comme un echec intermittent sur une machine de CI, jamais en local.
    ///
    /// QUE FAIRE SI C EST ROUGE : ajouter l attribut sur la classe nommee. Ne pas retirer la
    /// classe de la liste : le rouge dit qu un lecteur non serialise existe.
    /// </summary>
    public class PublishedOntologyCollectionGuardTests
    {
        [Fact]
        public void EveryTestClassReadingPublishedOntology_JoinsTheSerialisingCollection()
        {
            var ontologyDir = Path.Combine(TestRepoRoot.Find(),
                "Generation", "Converters", "Argumentum.AssetConverter.Tests", "Ontology");

            var offenders = Directory.GetFiles(ontologyDir, "*.cs")
                .Select(f => new { File = Path.GetFileName(f), Text = File.ReadAllText(f) })
                .Where(x => x.Text.Contains("\"docs\", " + "\"ontology\"")
                         || x.Text.Contains("docs/" + "ontology/argumentum"))
                .Where(x => x.Text.Contains("public class "))
                .Where(x => !x.Text.Contains("[Collection(PublishedOntologyCollection.Name)]"))
                .Select(x => x.File)
                .OrderBy(n => n)
                .ToArray();

            offenders.Should().BeEmpty(
                "toute classe lisant docs/ontology/ doit etre serialisee : OWLSharp ouvre ces " +
                "fichiers en exclusif, et un lecteur parallele echoue en IOException sur la CI " +
                "sans jamais rougir en local. Ajouter [Collection(PublishedOntologyCollection.Name)] " +
                "sur : " + string.Join(", ", offenders));
        }
    }
}
