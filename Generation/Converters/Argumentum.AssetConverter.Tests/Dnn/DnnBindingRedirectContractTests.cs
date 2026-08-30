using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Dnn
{
    /// <summary>
    /// Contrat : pour les assemblages que nous EPINGLONS deliberement, le bindingRedirect des
    /// configs versionnees et la DLL livree dans DNNPlatform/bin/ doivent designer la MEME version.
    ///
    /// INCIDENT FONDATEUR (2026-08-28/30, preprod dnn.argumentum.myia.io)
    /// Le 28/06 une install d'extension DNN a reecrit le web.config de preprod pour exiger
    /// ICSharpCode.SharpZipLib 1.4.2.13, alors qu'aucune 1.4.2.13 n'existait sur le disque. Le
    /// mismatch est reste LATENT deux mois : SharpZipLib n'etait jamais resolue au demarrage. Le
    /// deploy du 28/08 a redemarre l'app domain -> premiere resolution -> ConfigurationErrorsException
    /// -> 500 sur toutes les requetes, ~25 h d'indisponibilite, et le renouvellement ACME du SAN
    /// (52 hotes) mis en peril parce qu'une pool morte ne sert pas le challenge HTTP-01.
    ///
    /// Le piege n'est pas le mismatch : c'est sa LATENCE. Un redirect qui pointe vers une version
    /// absente ne rougit nulle part tant que personne ne charge l'assemblage. Ce test rend la
    /// verification immediate au lieu de la laisser au prochain redemarrage d'app domain.
    ///
    /// POURQUOI UNE LISTE ET PAS TOUS LES REDIRECTS
    /// Mesure du 30/08 sur master : 81 couples redirect<->DLL presente, dont 38 discordants. La
    /// grande majorite sont des app.config de DesktopModules/, artefacts de BUILD que le runtime ne
    /// lit pas (seuls le web.config racine et les <assembly>.dll.config sont charges). Un invariant
    /// universel serait donc rouge pour des raisons etrangeres a l'incident. On epingle ce qui a
    /// deja coute une indisponibilite ; ajouter une entree est une ligne.
    ///
    /// QUE FAIRE SI C'EST ROUGE : aligner la DLL ET les redirects sur la meme version, dans le meme
    /// commit. Ne pas relacher le test : le rouge dit que le deploy livrera une configuration qui
    /// ne peut pas se resoudre.
    /// </summary>
    public class DnnBindingRedirectContractTests
    {
        private static string RepoRoot => TestRepoRoot.Find();

        /// <summary>Assemblages epingles : nom d'assemblage tel qu'il figure dans assemblyIdentity.</summary>
        public static TheoryData<string> PinnedAssemblies => new() { "ICSharpCode.SharpZipLib" };

        [Theory]
        [MemberData(nameof(PinnedAssemblies))]
        public void PinnedAssembly_RedirectsAgreeWithShippedDll(string assemblyName)
        {
            var dll = Path.Combine(RepoRoot, "DNNPlatform", "bin", assemblyName + ".dll");
            File.Exists(dll).Should().BeTrue($"{assemblyName}.dll doit etre livree dans DNNPlatform/bin/");

            var shipped = AssemblyName.GetAssemblyName(dll).Version!.ToString();

            var offenders = ConfigsRedirecting(assemblyName)
                .Where(x => x.NewVersion != shipped)
                .Select(x => $"{x.Path} -> newVersion={x.NewVersion}")
                .ToArray();

            offenders.Should().BeEmpty(
                $"{assemblyName} livree en {shipped} : tout bindingRedirect versionne doit designer " +
                "cette version, sinon le prochain redemarrage d'app domain leve une " +
                "ConfigurationErrorsException et le site rend 500 sur toutes les requetes. " +
                "Divergences : " + string.Join(" | ", offenders));
        }

        [Theory]
        [MemberData(nameof(PinnedAssemblies))]
        public void PinnedAssembly_IsActuallyRedirectedSomewhere(string assemblyName)
        {
            // Contre-controle : si la recherche ne trouve plus AUCUN redirect, le test precedent
            // passerait au vert sur une liste vide -- un vert qui ne mesure rien. Cf. la garde
            // equivalente cote ontologies : un organe doit pouvoir VOIR le defaut qu'il surveille.
            ConfigsRedirecting(assemblyName).Should().NotBeEmpty(
                $"aucun bindingRedirect trouve pour {assemblyName} : soit il a ete retire (auquel cas " +
                "retirer aussi l'entree epinglee), soit la recherche est cassee et l'autre test est " +
                "vert par vacuite.");
        }

        private static (string Path, string NewVersion)[] ConfigsRedirecting(string assemblyName)
        {
            var configs = Directory.EnumerateFiles(
                Path.Combine(RepoRoot, "DNNPlatform"), "*.config", SearchOption.AllDirectories);

            return configs.SelectMany(path =>
            {
                XDocument doc;
                try { doc = XDocument.Load(path); } catch { return Enumerable.Empty<(string, string)>(); }

                return doc.Descendants()
                    .Where(e => e.Name.LocalName == "dependentAssembly")
                    .Select(e => new
                    {
                        Id = e.Elements().FirstOrDefault(c => c.Name.LocalName == "assemblyIdentity"),
                        Redirect = e.Elements().FirstOrDefault(c => c.Name.LocalName == "bindingRedirect"),
                    })
                    .Where(x => x.Id?.Attribute("name")?.Value == assemblyName && x.Redirect != null)
                    .Select(x => (
                        Path.GetRelativePath(RepoRoot, path).Replace(Path.DirectorySeparatorChar, '/'),
                        x.Redirect!.Attribute("newVersion")?.Value ?? "<absent>"));
            }).ToArray();
        }
    }
}
