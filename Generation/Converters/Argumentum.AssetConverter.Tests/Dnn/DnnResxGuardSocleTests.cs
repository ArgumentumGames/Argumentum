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

        // ── Mémoïsation + transport batch (#2138, options 1 puis 2) ──────────
        //
        // Six témoins CI (issue #2138) : des rafales de `git cat-file` tués au mur
        // de 30 s SIMULTANÉMENT — gel du runner, pas fichier lent. L'option 1
        // (#2140) a mémoïsé : un spawn par FICHIER distinct au lieu d'un par
        // lecture. Le témoin 7 est revenu quand même — des centaines de fichiers
        // distincts faisaient encore des centaines de spawns. L'option 2 (GO
        // ai-01, #2138 c.6630044378) fait traverser TOUS les blobs du run à UN
        // SEUL processus `git cat-file --batch` : plus de rafale possible.

        [Fact]
        public void AFileIsReadFromHeadOnlyOncePerRun()
        {
            var first = DnnResxGuardSocle.CommittedBlob(Known);
            var second = DnnResxGuardSocle.CommittedBlob(Known);

            first.Length.Should().BeGreaterThan(500,
                "les octets restent réels après le premier passage — une mémoire qui "
                + "rendrait un tableau vide blanchirait les contrôles BOM/LF des 14 gardes");
            ReferenceEquals(first, second).Should().BeTrue(
                "la mémoïsation doit rendre L'INSTANCE lue au premier passage, pas une "
                + "copie : c'est « chaque fichier n'est lu qu'une fois par run » rendu "
                + "observable (#2138). Deux tableaux distincts = deux lectures = le coût "
                + "que six témoins CI ont payé en timeouts");
        }

        /// <summary>
        /// Deux chemins commis, présents dans HEAD, et cités par AUCUN fichier du
        /// projet de tests (vérifié par grep au moment du choix) : ce fait est leur
        /// premier lecteur, donc leur première traversée du transport — sinon la
        /// mémoire servirait les deux lectures et le fait sur les spawns ne
        /// prouverait rien.
        /// </summary>
        private const string ColdA = "DNNPlatform/Config/SiteUrls.config";
        private const string ColdB = "DNNPlatform/Config/DotNetNuke.config";

        [Fact]
        public void DistinctColdFilesShareOneBatchProcess()
        {
            var spawnsBefore = DnnResxGuardSocle.BatchSpawnCount;
            var exchangesBefore = DnnResxGuardSocle.BatchExchangeCount;

            var a = DnnResxGuardSocle.CommittedBlob(ColdA);
            var b = DnnResxGuardSocle.CommittedBlob(ColdB);

            a.Length.Should().BeGreaterThan(100);
            b.Length.Should().BeGreaterThan(100,
                "les deux lectures doivent rendre des octets réels — un transport qui "
                + "rendrait des tableaux vides blanchirait tous les contrôles en aval");

            (DnnResxGuardSocle.BatchExchangeCount - exchangesBefore).Should().BeGreaterThanOrEqualTo(2,
                "les DEUX fichiers doivent avoir traversé le transport : si la mémoire "
                + "les servait, le fait sur les spawns mesurerait un cache, pas le "
                + "transport — c'est la garantie de non-vacuité du fait suivant");

            (DnnResxGuardSocle.BatchSpawnCount - spawnsBefore).Should().BeInRange(0, 1,
                "deux fichiers DISTINCTS doivent vivre dans le MÊME processus git "
                + "cat-file --batch : 0 si le transport était déjà debout, 1 s'il "
                + "vient de naître — jamais 2. La forme « un spawn par fichier » est "
                + "celle que le témoin 7 de #2138 a abattue : 2 ici, c'est elle qui "
                + "revient, avec les rafales qu'on a fuies");
        }

        /// <summary>
        /// Sous-commandes git qui déplacent HEAD ou l'arbre. La mémoïsation repose
        /// sur l'invariant « HEAD ne bouge pas pendant un run » : un test qui
        /// committerait en plein run lirait ensuite un blob périmé sans que rien
        /// ne rougisse localement — c'est ce verbe-là qu'on interdit, pas la
        /// lecture.
        /// </summary>
        private static readonly Regex GitWriteVerb = new Regex(
            "(?:\"git\"\\s*,\\s*\\$?\"|RunGit\\(\\s*\\$?\")"
            + "(commit|checkout|reset|merge|rebase|stash|pull|push|restore|clean|switch|tag|rm|mv|add)\\b",
            RegexOptions.Compiled);

        /// <summary>
        /// Tout fichier <c>.cs</c> du projet de tests, hors <c>bin/obj</c> : la garde
        /// couvre le projet entier, pas le seul répertoire Dnn — un commit mid-run
        /// n'importe où casserait la mémoire du socle.
        /// </summary>
        private static string[] TestSources() => Directory.GetFiles(
            Path.Combine(TestRepoRoot.Find(), "Generation", "Converters", "Argumentum.AssetConverter.Tests"),
            "*.cs", SearchOption.AllDirectories)
            .Where(p => !p.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                && !p.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
            .ToArray();

        [Fact]
        public void NoTestMovesHeadOrTheWorktreeMidRun()
        {
            var offenders = TestSources()
                .Where(p => GitWriteVerb.IsMatch(File.ReadAllText(p)))
                .Select(p => Path.GetFileName(p))
                .ToArray();

            offenders.Should().BeEmpty(
                "la mémoïsation de CommittedBlob (#2138) repose sur « HEAD ne bouge pas "
                + "pendant un run ». Les bancs rouge/vert committent ENTRE deux runs — un "
                + "test qui committerait au milieu rendrait la mémoire périmée : la garde "
                + "l'interdit AVANT que la corruption ne soit silencieuse");
        }

        [Fact]
        public void TheWriteVerbDetectorSeesAGitWriteCommandAndIgnoresProseAboutCheckouts()
        {
            GitWriteVerb.IsMatch(
                "var psi = new ProcessStartInfo(\"git\", \"commit -m temoin\")").Should().BeTrue(
                "contrôle inverse en ligne : un git commit lancé depuis un test DOIT être "
                + "vu, sinon la garde précédente est un vert à vide");

            GitWriteVerb.IsMatch("DnnBinUntrackGuard.RunGit(\"checkout master\", out var o, out _)").Should().BeTrue(
                "même forme via le helper RunGit — le détecteur vise les DEUX formes de "
                + "spawn du corpus");

            GitWriteVerb.IsMatch(
                "// le runner CI a core.autocrlf=true, qui convertit LF → CRLF au checkout")
                .Should().BeFalse(
                "« checkout » dans un commentaire est de la prose, pas une sous-commande : "
                + "cibler le début de la chaîne de commande, pas le mot, est toute la différence");

            GitWriteVerb.IsMatch(
                "File.Exists(path).Should().BeTrue($\"le resx doit exister dans le checkout ({rel})\");")
                .Should().BeFalse(
                "les messages d'assertion du corpus parlent du checkout sans lancer git : "
                + "le détecteur ne doit pas les signaler");
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
        public void TheSocleTimesOutItsBatchExchangesAndKeepsStderrDrained()
        {
            var src = File.ReadAllText(Path.Combine(DnnDir, "DnnResxGuardSocle.cs"));

            src.Should().Contain("cat-file --batch",
                "le transport de l'option 2 : UN processus conversationnel pour tout le run, "
                + "pas un spawn par fichier");
            src.Should().NotContain("cat-file -p",
                "la forme « un cat-file -p par fichier » est celle que le témoin 7 de #2138 a "
                + "abattue : sa revenue, même mémoïsée, recrée les rafales de spawns");
            src.Should().Contain("Wait(GitTimeoutMs)",
                "chaque échange doit être attendu AVEC délai, sinon un git cat-file --batch muet "
                + "gèlerait la jambe entière : c'est le mur de 30 s rendu tirable");
            src.Should().Contain("DrainStderrAsync",
                "stderr doit être vidé en continu : un tube d'erreur jamais lu se remplit et "
                + "bloque l'enfant — la panne qu'on prétend éviter deviendrait celle qu'on provoque");
            src.Should().NotContain("BaseStream.CopyTo(",
                "un drain SYNCHRONE se bloque si l'enfant n'écrit plus sans fermer son tube — "
                + "et le délai placé après lui ne couvrirait alors rien du tout, précisément dans "
                + "le cas qu'il nomme (« dépôt verrouillé, lecteur réseau lent »). Ce fait épingle "
                + "la FORME du code, faute de pouvoir provoquer un blocage de 30 s en CI : il ne "
                + "prouve pas l'absence de blocage, il empêche le retour de la forme qui en produit un");
        }
    }
}
