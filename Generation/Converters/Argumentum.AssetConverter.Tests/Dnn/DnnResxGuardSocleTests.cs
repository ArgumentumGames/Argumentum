using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Garde du socle lui-même. Un socle non mesuré est une hypothèse partagée par
    /// 14 gardes : la moindre erreur s'y propage en silence, et un socle qui rendrait
    /// zéro octet ou un tableau vide ferait passer les contrôles BOM/LF de tous les
    /// lots pour de mauvaises raisons (« 0 CR trouvé » sur 0 octet est un vert).
    /// <para>
    /// Les trois faits ci-dessous ferment les trois portes : le socle rend bien des
    /// octets RÉELS, il refuse un chemin absent de HEAD avec un message qui nomme la
    /// cause, et il ne rend pas un tableau vide en cas d'échec silencieux.
    /// </para>
    /// </summary>
    public class DnnResxGuardSocleTests
    {
        /// <summary>
        /// Témoin : un fichier connu du dépôt, versionné depuis longtemps, donc présent
        /// dans HEAD sur toute branche issue de master.
        /// </summary>
        private const string Known = "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/Membership.ascx.resx";

        [Fact]
        public void TheSocleReturnsTheRealBytesOfACommittedFile()
        {
            var blob = DnnResxGuardSocle.CommittedBlob(Known);

            blob.Length.Should().BeGreaterThan(500,
                "le blob doit être le fichier entier, pas un fragment ou un tableau vide — "
                + "un socle muet rendrait « 0 CR » sur 0 octet et blanchirait tous les lots");
            var text = System.Text.Encoding.UTF8.GetString(blob);
            text.Should().Contain("<root>",
                "un resx committé porte sa racine XML : le socle lit bien le fichier demandé, pas un voisin");
            text.Should().Contain("text/microsoft-resx",
                "l'en-tête resx est l'invariant du fichier lu — un chemin qui rendrait autre chose "
                + "ferait passer les contrôles BOM/LF sur un objet qui n'est pas le bon");
        }

        [Fact]
        public void TheSocleNamesTheCauseWhenThePathIsNotInHead()
        {
            var act = () => DnnResxGuardSocle.CommittedBlob(
                "DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/__ce-fichier-n-existe-pas__.resx");

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*git cat-file*",
                    "l'échec doit nommer l'outil ET le chemin : c'est ce qui distingue « pas encore committé » "
                    + "d'une panne git, et c'est exactement le cas d'une garde lancée avant le commit")
                .WithMessage("*__ce-fichier-n-existe-pas__*");
        }

        [Fact]
        public void TheSocleRefusesAnEmptyPath()
        {
            var act = () => DnnResxGuardSocle.CommittedBlob("   ");

            act.Should().Throw<ArgumentException>(
                "un chemin vide demanderait « HEAD: » — un objet ambigu, pas un fichier : on refuse AVANT d'appeler git");
        }

        [Fact]
        public void TheTimeoutIsShortEnoughToFailFastAndLongEnoughForARealRepo()
        {
            DnnResxGuardSocle.GitTimeoutMs.Should().Be(30_000,
                "30 s : large pour un cat-file local, court pour ne pas geler un run CI. "
                + "Changer cette valeur est une décision — la garde l'épingle");
        }

        // ── Garde anti-régression du socle ────────────────────────────────────
        //
        // Le socle n'a de valeur que si les gardes l'APPELLENT. Une quinzième copie
        // le rend faux sans qu'aucun test ne rougisse : elle passe, elle répond, elle
        // mesure — avec l'ancienne implémentation.

        /// <summary>
        /// Déclaration de <c>CommittedBlob</c> suivie d'une ACCOLADE, c'est-à-dire d'un
        /// CORPS. Les formes déléguées (<c>=&gt; DnnResxGuardSocle.CommittedBlob(rel)</c>)
        /// ne matchent pas : c'est exactement ce qu'on veut distinguer.
        /// </summary>
        private static readonly Regex BlobBody = new Regex(
            @"static\s+byte\[\]\s+CommittedBlob\s*\([^)]*\)\s*\{", RegexOptions.Compiled);

        /// <summary>
        /// Échantillon d'un corps recopié, utilisé comme contrôle inverse EN LIGNE :
        /// si ce texte cessait d'être reconnu, le fait principal deviendrait un vert
        /// à vide sans que rien ne prévienne.
        /// </summary>
        private const string CopiedBodySample =
            "private static byte[] CommittedBlob(string rel)\n"
            + "{\n"
            + "    var psi = new ProcessStartInfo(\"git\", \"cat-file -p \\\"HEAD:...\\\"\");\n"
            + "    return null;\n"
            + "}";

        private static string DnnDir => Path.Combine(
            TestRepoRoot.Find(), "Generation", "Converters",
            "Argumentum.AssetConverter.Tests", "Dnn");

        private static string[] GuardSources() =>
            Directory.GetFiles(DnnDir, "*.cs", SearchOption.TopDirectoryOnly);

        private static bool HasLocalBody(string source) => BlobBody.IsMatch(source);

        [Fact]
        public void NoGuardReimplementsCommittedBlobWithItsOwnBody()
        {
            var offenders = GuardSources()
                .Where(p => !string.Equals(Path.GetFileName(p), "DnnResxGuardSocle.cs", StringComparison.Ordinal))
                .Where(p => HasLocalBody(File.ReadAllText(p)))
                .Select(Path.GetFileName)
                .ToArray();

            offenders.Should().BeEmpty(
                "le socle existe pour que la primitive git soit corrigée UNE fois. Les trois "
                + "dernières copies n'étaient pas seulement dupliquées, elles étaient DÉGRADÉES : "
                + "WaitForExit(30_000) sans vérifier son retour, puis lecture d'ExitCode sur un "
                + "processus qui pouvait tourner encore. Une copie qui revient, ce sont ces deux "
                + "défauts qui reviennent, dans un fichier que personne ne relit");
        }

        [Fact]
        public void TheAntiRegrowthInstrumentSeesACopiedBodyAndIgnoresACommentThatCitesTheCommand()
        {
            HasLocalBody(CopiedBodySample).Should().BeTrue(
                "contrôle inverse en ligne : un corps recopié DOIT être reconnu, sinon le fait "
                + "principal est un vert à vide — et il le resterait indéfiniment");

            HasLocalBody("// cf. DnnResxGuardSocle : git cat-file -p HEAD:<path>\n").Should().BeFalse(
                "un commentaire qui CITE la commande n'est pas une redéfinition : cibler le corps "
                + "de fonction, pas la sous-chaîne, est toute la différence");

            GuardSources().Count(p => File.ReadAllText(p).Contains("cat-file")).Should().BeGreaterThan(5,
                "et ce n'est pas théorique : le corpus contient bien des fichiers qui nomment "
                + "cat-file sans le redéfinir. Un détecteur écrit Contains(\"cat-file\") les "
                + "signalait tous — c'est pour cela que ce fait vise la déclaration suivie d'une accolade");
        }

        [Fact]
        public void TheSocleDrainsStdoutAsynchronouslySoItsTimeoutCanActuallyFire()
        {
            var src = File.ReadAllText(Path.Combine(DnnDir, "DnnResxGuardSocle.cs"));

            src.Should().Contain("CopyToAsync",
                "les deux tubes doivent être drainés sans bloquer l'appelant");
            src.Should().NotContain("BaseStream.CopyTo(",
                "un drain SYNCHRONE se bloque si l'enfant n'écrit plus sans fermer son tube — "
                + "et le délai placé après lui ne couvrirait alors rien du tout, précisément dans "
                + "le cas qu'il nomme (« dépôt verrouillé, lecteur réseau lent »). Ce fait épingle "
                + "la FORME du code, faute de pouvoir provoquer un blocage de 30 s en CI : il ne "
                + "prouve pas l'absence de blocage, il empêche le retour de la forme qui en produit un");
        }
    }
}
