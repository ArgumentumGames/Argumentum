using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
            src.Should().Contain("AwaitExchange(exchange, rel, trace, GitTimeoutMs",
                "chaque échange doit être attendu AVEC délai, sinon un git cat-file --batch muet "
                + "gèlerait la jambe entière : c'est le mur de 30 s rendu tirable. Le délai a "
                + "déménagé dans AwaitExchange avec l'instrument (#2138) — le fait épingle donc "
                + "le PASSAGE de GitTimeoutMs à cet endroit, pas la forme d'appel d'avant");
            src.Should().Contain("exchange.Wait(timeoutMs)",
                "et c'est bien CE délai-là qui borne l'attente : un AwaitExchange qui recevrait "
                + "GitTimeoutMs sans s'en servir serait un vert de forme");
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

        // ── Cohérence Mark/Snapshot (review #2155, remarque 2) ────────────────
        //
        // `Mark` écrivait la marque PUIS l'étape ; l'ancien `Snapshot` ne
        // revérifiait que l'étape, qui lag derrière la marque : un instantané
        // pouvait rendre Stage = NotStarted avec un StartedAtMs déjà posé —
        // c'est-à-dire le diagnostic « POOL AFFAMÉ » sur un échange démarré.
        // L'étape est désormais DÉRIVÉE de la plus haute marque posée.

        [Fact]
        public void TheStageIsDerivedFromTheHighestMarkPosed()
        {
            var trace = new BatchExchangeProgress();

            trace.Snapshot().Stage.Should().Be(BatchExchangeStage.NotStarted,
                "aucune marque posée : l'étape dérivée est l'absence d'étape, et toutes les "
                + "marques restent à leur sentinelle -1");

            trace.Mark(BatchExchangeStage.Started);
            var oneMark = trace.Snapshot();
            oneMark.Stage.Should().Be(BatchExchangeStage.Started,
                "la marque Started est posée : l'étape ne peut PAS rester NotStarted — c'est le "
                + "couple incohérent de #2155 rendu impossible par construction");
            oneMark.StartedAtMs.Should().BeGreaterThanOrEqualTo(0,
                "une marque posée est un horodatage, jamais la sentinelle -1");

            trace.Mark(BatchExchangeStage.HeaderRead);
            var twoMarks = trace.Snapshot();
            twoMarks.Stage.Should().Be(BatchExchangeStage.HeaderRead,
                "la plus haute marque posée commande l'étape, même en sautant une étape "
                + "intermédiaire");
            twoMarks.HeaderAtMs.Should().BeGreaterThanOrEqualTo(0);
            twoMarks.RequestAtMs.Should().Be(-1,
                "aucune marque RequestWritten posée : la photographie rend ce qui est marqué, "
                + "rien d'autre — dériver n'est pas inventer");
            twoMarks.StartedAtMs.Should().BeGreaterThanOrEqualTo(0,
                "les marques inférieures restent visibles avec la haute : la lecture descendante "
                + "de Snapshot (acquire) couvre tout ce que le thread d'échange a posé avant");
        }

        /// <summary>
        /// Contrôle inverse concurrent de la remarque 2 de #2155 : pendant qu'un
        /// thread dédié pose les quatre marques, AUCUN instantané ne doit rendre
        /// le couple (Stage = NotStarted, StartedAtMs posé). Sur l'ancienne
        /// implémentation — une boucle qui revérifiait le champ d'étape, écrit
        /// APRÈS la marque — cette fenêtre était réelle ; l'étape étant dérivée
        /// des marques elles-mêmes, le couple ne peut plus se produire, quelle
        /// que soit l'entrelace.
        /// <para>
        /// ⚠️ La non-vacuité est PAR CONSTRUCTION, pas par synchronisation : le
        /// premier jet comptait les passages de boucle (> 1 000) et la jambe
        /// Release de la CI l'a cassé avec « found 1 » — sur un runner chargé,
        /// le thread de test est désordonnancé entre StartNew et la boucle, et
        /// le banc mesurait l'ordonnanceur, pas le socle (même famille que le
        /// commentaire LongRunning ci-dessus). Désormais chaque round porte DEUX
        /// témoins pris séquentiellement par CE thread — avant l'ouverture du
        /// gate (aucune marque posée) et après la pose complète — plus la boucle
        /// concurrente comme câble de détection quand l'entrelace se produit.
        /// </para>
        /// </summary>
        [Fact]
        public async Task SnapshotsTakenWhileMarksArePosedNeverPairNotStartedWithAStartedMark()
        {
            const int rounds = 200;
            long snapshots = 0;
            for (var round = 0; round < rounds; round++)
            {
                var trace = new BatchExchangeProgress();
                using var go = new ManualResetEventSlim(false);
                var poser = Task.Factory.StartNew(() =>
                {
                    go.Wait();
                    trace.Mark(BatchExchangeStage.Started);
                    Thread.SpinWait(2_000);
                    trace.Mark(BatchExchangeStage.RequestWritten);
                    Thread.SpinWait(2_000);
                    trace.Mark(BatchExchangeStage.HeaderRead);
                    Thread.SpinWait(2_000);
                    trace.Mark(BatchExchangeStage.BlobRead);
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                try
                {
                    // Témoin pré-pose : le gate est fermé, AUCUNE marque ne peut
                    // exister — ce témoin ne dépend d'aucun ordonnancement.
                    var before = trace.Snapshot();
                    snapshots++;
                    before.Stage.Should().Be(BatchExchangeStage.NotStarted,
                        "le poseur attend le gate : aucune marque posée, l'étape dérivée est "
                        + "NotStarted et les horodatages restent à leur sentinelle");
                    before.StartedAtMs.Should().Be(-1,
                        "StartedAtMs = -1 et Stage = NotStarted : cohérent dans l'AUTRE sens — "
                        + "l'incohérence condamnée par #2155 est le couple inverse");

                    go.Set();

                    while (!poser.IsCompleted)
                    {
                        var s = trace.Snapshot();
                        snapshots++;
                        if (s.StartedAtMs >= 0)
                        {
                            s.Stage.Should().NotBe(BatchExchangeStage.NotStarted,
                                "StartedAtMs posé + Stage NotStarted = le couple incohérent de "
                                + "#2155 : le message de timeout lirait « POOL AFFAMÉ » sur un "
                                + "échange démarré");
                        }
                    }

                    // Témoin post-pose : les quatre marques sont posées.
                    var after = trace.Snapshot();
                    snapshots++;
                    after.Stage.Should().Be(BatchExchangeStage.BlobRead,
                        "toutes les marques posées, l'étape dérivée est la dernière");
                    after.StartedAtMs.Should().BeGreaterThanOrEqualTo(0);
                }
                finally
                {
                    go.Set();
                    await poser;
                }
            }

            snapshots.Should().BeGreaterThanOrEqualTo(2L * rounds,
                "non-vacuité PAR CONSTRUCTION : chaque round prend deux témoins séquentiels "
                + "(pré-gate, post-pose) — ce planché ne mesure pas l'ordonnanceur, seulement "
                + "que les témoins ont bien été pris ; les passages concurrents de la boucle "
                + "sont du câble de détection en plus, jamais une condition du vert");
        }

        // ── Lancement hors pool (#2138 étape 2, témoin 9) ─────────────────────
        //
        // Le témoin 9 (run 42202111548, tentative 1, les deux jambes) a mesuré
        // l'échange JAMAIS démarré : pool à 25-27 threads, 15-19 tâches en
        // attente. Task.Run mettait l'échange en FILE derrière les gardes
        // parallèles, et les 30 s mesuraient une attente de thread, pas git.
        // Le correctif lance l'échange sur un thread DÉDIÉ
        // (TaskCreationOptions.LongRunning) : hors de la file du pool. Le
        // défaut est REPRODUIT ici, pas raconté.

        /// <summary>
        /// Fichier commis, ancien, cité par AUCUN autre fait du projet (le
        /// grep du choix de ColdA/ColdB a été rejoué en le couvrant). La sonde
        /// de production traverse <c>BatchExchange</c> DIRECTEMENT — hors de la
        /// mémoire du socle : même si un autre fait lisait ce chemin un jour,
        /// l'échange serait refait et la mesure structurelle resterait réelle.
        /// </summary>
        private const string SaturationWitness = "docs/ontology/aif-export/README.md";

#pragma warning disable xUnit1031 // Le Wait bloquant borné EST le mécanisme reproduit (Task.Run attendu par Wait, témoin 9) — pas un accident du banc.
        /// <summary>
        /// Contrôle inverse de l'étape 2, en trois sondes dont DEUX sont
        /// structurelles : sous un pool saturé de façon contrôlée, la forme
        /// ancienne (tâche de pool attendue par <c>Wait</c> — exactement le
        /// couple que <c>BatchExchange</c> utilisait) ne démarre pas sous une
        /// borne courte ; la forme neuve (<see cref="TaskCreationOptions.LongRunning"/>)
        /// démarre ET rapporte qu'elle tourne hors du pool ; et l'échange de
        /// PRODUCTION traverse le transport sans jamais consommer un thread du
        /// pool (<c>ExchangeBodyOnPoolThreadCount</c> inchangé).
        /// <para>
        /// La sonde temporelle est garantie PAR CONSTRUCTION : les bloqueurs,
        /// mis en file AVANT elle, dépassent le compte de threads du pool de 8 ;
        /// au plus <c>ThreadCount</c> peuvent tourner, donc au moins 8 restent
        /// DEVANT dans la file — qui ne démarre qu'après 8 injections (~1/s),
        /// hors de portée d'une fenêtre de 250 ms, quel que soit le runner.
        /// </para>
        /// <para>
        /// ⚠️ <b>La saturation a deux formes, et il faut les deux</b> : soit au
        /// moins un bloqueur tourne (le pool dispatche), soit AUCUN n'a démarré
        /// alors que la file contient les bloqueurs — le pool est alors déjà
        /// affamé, et la sonde est encore PLUS valide. Un plancher « ≥ 1
        /// bloqueur en cours » lisait cette seconde forme comme un banc
        /// dégénéré : c'est exactement ainsi que ce fait a rougi une fois sous
        /// la suite complète (06/11, [15 s] = 5 s de témoin + 10 s de
        /// jointure), alors que la propriété testée tenait. Le préalable est
        /// donc « des bloqueurs occupent ou remplissent le pool », jamais « le
        /// pool a bien voulu dispatcher ».
        /// </para>
        /// <para>
        /// ⚠️ Saturation bornée et auto-libérée (le banc ne doit pas affamer les
        /// autres gardes) : les bloqueurs tiennent 20 s au plus puis rendent la
        /// main d'eux-mêmes, et le <c>finally</c> ouvre le gate et JOINT tout —
        /// dans le cas nominal ils sortent en quelques centaines de ms, la suite
        /// ne garde ni thread occupé ni item en attente après ce fait.
        /// </para>
        /// </summary>
        [Fact]
        public void TheExchangeLaunchDoesNotQueueBehindTheThreadPool()
        {
            // Sous son minimum, le pool crée ses threads SANS délai : la file ne
            // commence qu'au-delà de max(ThreadCount, minimum). Partir de ThreadCount
            // seul rougissait la sonde 1 lancée isolément sur une machine à 32 cœurs
            // (pool encore froid, 32 threads créables à la demande) — mesuré 3/3.
            ThreadPool.GetMinThreads(out var minWorkerThreads, out _);
            var poolThreads = Math.Max(ThreadPool.ThreadCount, minWorkerThreads);
            var blockers = poolThreads + 8;
            using var gate = new ManualResetEventSlim(false);
            long runningBlockers = 0;

            var blockerTasks = new Task[blockers];
            for (var i = 0; i < blockers; i++)
            {
                blockerTasks[i] = Task.Run(() =>
                {
                    Interlocked.Increment(ref runningBlockers);
                    gate.Wait(TimeSpan.FromSeconds(20));
                });
            }

            try
            {
                // PRÉALABLE — deux formes de saturation, voir le résumé.
                // ⚠️ Les valeurs assertées sont celles que la boucle a LUES, pas
                // une relecture d'après coup : au second passage du pool, un
                // bloqueur quitte la file et un `pending` relu (13 pour 14 mis
                // en file) ferait échouer un préalable qui était vrai à
                // l'instant du constat (mesuré : 94 ms, 0 en cours / 13 en
                // attente alors que la condition d'entrée était satisfaite).
                long running = 0;
                long pending = 0;
                var deadline = DateTime.UtcNow.AddSeconds(5);
                while (true)
                {
                    running = Volatile.Read(ref runningBlockers);
                    pending = ThreadPool.PendingWorkItemCount;
                    if (running >= 1 || pending >= blockers || DateTime.UtcNow > deadline)
                        break;

                    Thread.Sleep(5);
                }

                (running >= 1 || pending >= blockers).Should().BeTrue(
                    "la saturation doit être établie avant de sonder : sinon le banc mesurerait "
                    + $"son propre délai de démarrage — {blockers} bloqueurs mis en file, "
                    + $"observé : {running} en cours et {pending} en attente");

                // SONDE 1 — forme ANCIENNE (temporelle) : le défaut REPRODUIT,
                // une tâche de pool mise en file derrière les bloqueurs.
                var oldForm = Task.Run(() => "l'échange a démarré");
                oldForm.Wait(250).Should().BeFalse(
                    "sous pool saturé, une tâche Task.Run mise en file ne démarre pas en "
                    + "250 ms : c'est exactement le couple du témoin 9 (Task.Run attendu "
                    + "par Wait) — si ce sonde passe, le banc ne mesure plus la file du pool");

                // SONDE 2 — forme NEUVE (structurelle) : démarre, et hors pool.
                var newForm = Task.Factory.StartNew(
                    () => Thread.CurrentThread.IsThreadPoolThread,
                    CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                newForm.Wait(5_000).Should().BeTrue(
                    "un thread dédié démarre même quand tout le pool est occupé : le "
                    + "lancement ne dépend plus d'un thread libre du pool");
                newForm.Result.Should().BeFalse(
                    "et ce thread n'est PAS un thread du pool : c'est ce qui retire "
                    + "l'attente de file, pas seulement ce qui la contourne");

                // SONDE 3 — le VRAI chemin de production, mesuré LÀ où le
                // mécanisme se produit : le corps de l'échange n'occupe aucun
                // item de pool. Déterministe : ni horloge ni état du pool.
                var onPoolBefore = DnnResxGuardSocle.ExchangeBodyOnPoolThreadCount;
                var blob = DnnResxGuardSocle.BatchExchange(
                    "HEAD:" + SaturationWitness, SaturationWitness);
                blob.Length.Should().BeGreaterThan(100,
                    "sous saturation, le socle lit quand même le blob : c'est la promesse "
                    + "de l'étape 2 rendue observable sur le chemin réel");
                DnnResxGuardSocle.ExchangeBodyOnPoolThreadCount.Should().Be(onPoolBefore,
                    "l'échange de production n'a pas tourné sur un thread du pool : revenir à "
                    + "Task.Run ferait monter ce compteur — c'est le retour du témoin 9, et "
                    + "cette mesure ne dépend d'aucun ordonnancement");
            }
            finally
            {
                gate.Set();
                Task.WaitAll(blockerTasks, TimeSpan.FromSeconds(10));
            }
        }
#pragma warning restore xUnit1031
    }
}
