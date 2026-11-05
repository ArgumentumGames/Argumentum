using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Où en est un échange sur le transport batch au moment où on le regarde.
    /// Les valeurs sont ordonnées comme le protocole : une valeur plus grande
    /// signifie « au moins arrivé là ».
    /// </summary>
    internal enum BatchExchangeStage
    {
        /// <summary>
        /// Aucun thread n'a encore exécuté <c>ExchangeOnce</c> : l'attente porte
        /// sur le pool de threads, pas sur git — la tâche n'a jamais commencé.
        /// </summary>
        NotStarted = 0,

        /// <summary>Le corps de l'échange a commencé : il a obtenu un thread.</summary>
        Started = 1,

        /// <summary>Requête écrite ET vidée sur l'entrée du processus batch.</summary>
        RequestWritten = 2,

        /// <summary>En-tête de réponse lu (blob annoncé, « missing », ou type non-blob).</summary>
        HeaderRead = 3,

        /// <summary>Contenu entièrement lu et fin de ligne finale vérifiée.</summary>
        BlobRead = 4,
    }

    /// <summary>
    /// Vue cohérente de l'avancement d'un échange, lue d'un seul tenant : les
    /// champs sont marqués séparément, une lecture naïve pourrait voir la marque
    /// d'une étape et l'horodatage d'une autre.
    /// </summary>
    internal readonly struct BatchExchangeSnapshot
    {
        internal BatchExchangeSnapshot(
            BatchExchangeStage stage, long startedAtMs, long requestAtMs, long headerAtMs,
            long blobAtMs, long announcedBytes, long payloadBytesRead)
        {
            Stage = stage;
            StartedAtMs = startedAtMs;
            RequestAtMs = requestAtMs;
            HeaderAtMs = headerAtMs;
            BlobAtMs = blobAtMs;
            AnnouncedBytes = announcedBytes;
            PayloadBytesRead = payloadBytesRead;
        }

        internal BatchExchangeStage Stage { get; }

        /// <summary>Millisecondes écoulées depuis la création du suivi, au moment où l'échange a obtenu son thread.</summary>
        internal long StartedAtMs { get; }

        /// <summary>Millisecondes écoulées quand la requête a été vidée sur stdin.</summary>
        internal long RequestAtMs { get; }

        /// <summary>Millisecondes écoulées quand l'en-tête de réponse a été lu.</summary>
        internal long HeaderAtMs { get; }

        /// <summary>Millisecondes écoulées quand le contenu a été entièrement lu.</summary>
        internal long BlobAtMs { get; }

        /// <summary>Taille annoncée par l'en-tête, -1 tant qu'aucun en-tête n'est lu.</summary>
        internal long AnnouncedBytes { get; }

        /// <summary>Octets du contenu déjà lus, 0 tant qu'aucun n'est arrivé.</summary>
        internal long PayloadBytesRead { get; }
    }

    /// <summary>
    /// Instrument de #2138 : horodate chaque étape d'un échange pour que le
    /// message de timeout dise OÙ l'échange s'est arrêté au lieu de le supposer.
    /// <para>
    /// Avant cet instrument, trois causes très différentes produisaient le MÊME
    /// message — le pool de threads affamé (l'échange n'a jamais commencé, les
    /// 30 s mesurent une attente de thread), git muet (requête écrite, en-tête
    /// jamais revenu), et un tube bloqué au milieu du contenu (en-tête lu, N
    /// octets sur M). Le diagnostic les sépare ; sans lui, chaque re-run vert se
    /// lisait comme une preuve que ce n'était « rien ».
    /// </para>
    /// <para>
    /// Écrit par le thread de l'échange, lu par le thread qui attend : chaque
    /// marque est un <see cref="Volatile.Write{T}(ref T, T)"/> (release) et
    /// <see cref="Snapshot"/> relit jusqu'à voir un couple cohérent.
    /// </para>
    /// </summary>
    internal sealed class BatchExchangeProgress
    {
        private readonly Stopwatch clock = Stopwatch.StartNew();
        private readonly long[] marks = { -1, -1, -1, -1, -1 };
        private int stage = (int)BatchExchangeStage.NotStarted;
        private long announcedBytes = -1;
        private long payloadBytesRead;

        /// <summary>Marque l'entrée dans une étape ; l'horodatage précède la marque (release).</summary>
        internal void Mark(BatchExchangeStage next)
        {
            Volatile.Write(ref marks[(int)next], clock.ElapsedMilliseconds);
            Volatile.Write(ref stage, (int)next);
        }

        /// <summary>Taille annoncée par l'en-tête de réponse.</summary>
        internal void ReportHeader(long announced) => Volatile.Write(ref announcedBytes, announced);

        /// <summary>Avancement de la lecture du contenu, mis à jour à chaque fragment.</summary>
        internal void ReportBytes(long read) => Volatile.Write(ref payloadBytesRead, read);

        /// <summary>
        /// Photographie de l'avancement. Relit tant que la marque bouge : le
        /// thread de l'échange peut avancer entre deux champs, et un message qui
        /// mêlerait l'étape d'un instant à l'horodatage d'un autre enverrait le
        /// diagnostic sur la mauvaise piste.
        /// </summary>
        internal BatchExchangeSnapshot Snapshot()
        {
            while (true)
            {
                var seen = Volatile.Read(ref stage);
                var started = Volatile.Read(ref marks[(int)BatchExchangeStage.Started]);
                var request = Volatile.Read(ref marks[(int)BatchExchangeStage.RequestWritten]);
                var header = Volatile.Read(ref marks[(int)BatchExchangeStage.HeaderRead]);
                var blob = Volatile.Read(ref marks[(int)BatchExchangeStage.BlobRead]);
                if (seen != Volatile.Read(ref stage))
                    continue;

                return new BatchExchangeSnapshot(
                    (BatchExchangeStage)seen, started, request, header, blob,
                    Volatile.Read(ref announcedBytes), Volatile.Read(ref payloadBytesRead));
            }
        }
    }

    /// <summary>
    /// Socle des gardes <c>*ResxGuardTests</c> : les primitives que chaque garde de
    /// lot réimplémentait à l'identique — dupliquées 14 fois, donc corrigées 14 fois
    /// quand l'une se révélait fausse.
    /// <para>
    /// <b>Pourquoi le blob committé et non le fichier de l'arbre de travail :</b> le
    /// runner CI a <c>core.autocrlf=true</c>, l'arbre Windows local non. Une garde
    /// qui lit <see cref="File.ReadAllBytes(string)"/> mesure donc deux objets
    /// différents selon l'hôte, et le verdict BOM/LF change de camp en changeant de
    /// machine. <see cref="CommittedBlob"/> lit <c>HEAD:&lt;path&gt;</c>, identique
    /// partout — l'écart local/CI devient un écart du dépôt, pas du poste.
    /// </para>
    /// <para>
    /// Corollaire opérationnel : <b>le fichier doit être committé AVANT d'être
    /// mesuré.</b> Une garde lancée sur un ajout non committé échoue sur
    /// <c>git cat-file</c> — c'est voulu, pas un défaut : mieux vaut un rouge
    /// bruyant qu'un vert optimiste sur un blob qui n'existe pas encore.
    /// </para>
    /// <para>
    /// <b>Transport (#2138, option 2) :</b> TOUS les blobs du run traversent UN
    /// SEUL processus <c>git cat-file --batch</c> gardé en vie, requête-réponse sur
    /// ses tubes. L'option 1 (mémoïsation, #2140) avait réduit les spawns à un par
    /// FICHIER distinct ; le témoin 7 est revenu quand même : des centaines de
    /// fichiers faisaient encore des centaines de spawns, et les jambes CI
    /// parallèles ne survivaient pas à toutes les rafales. Un processus unique
    /// supprime la famille entière — plus de rafale possible. La mémoïsation reste
    /// (un chemin ne traverse le transport qu'une fois par run) et chaque échange
    /// garde le délai de 30 s : le transport a changé, pas la vigilance.
    /// </para>
    /// </summary>
    internal static class DnnResxGuardSocle
    {
        /// <summary>Délai au-delà duquel on renonce à mesurer, journalisé nommément.</summary>
        internal const int GitTimeoutMs = 30_000;

        /// <summary>
        /// Mémoire des blobs déjà lus, clé = chemin dépôt. <c>HEAD</c> ne bouge pas
        /// pendant un run de tests — aucun test ne commit mid-run, épingle par
        /// <c>NoTestMovesHeadOrTheWorktreeMidRun</c> — donc un blob lu une fois vaut
        /// pour tout le run. Les échecs (chemin absent de HEAD, timeout) ne sont PAS
        /// mis en mémoire : un retry peut suivre un commit survenu entre-temps.
        /// </summary>
        private static readonly ConcurrentDictionary<string, byte[]> CommittedBlobCache =
            new(StringComparer.Ordinal);

        // ── Transport : UN processus git cat-file --batch pour tout le run ──────
        //
        // Le protocole : « spec\n » sur stdin, puis sur stdout soit
        // « oid type taille\n » + contenu + « \n », soit « spec missing\n ».
        // Tous les échanges passent par BatchGate — une seule conversation à la
        // fois sur les tubes — et chacun est borné par GitTimeoutMs.

        private static readonly object BatchGate = new();
        private static Process? batchProcess;
        private static Stream? batchStdin;
        private static Stream? batchStdout;
        private static readonly StringBuilder StderrTail = new();
        private static readonly object StderrLock = new();

        /// <summary>
        /// Nombre de spawns du processus batch depuis le début du run. Observable
        /// par la garde : deux fichiers DISTINCTS doivent faire vivre au plus UN
        /// processus — la forme « un spawn par fichier » est celle que le témoin 7
        /// de #2138 a abattue, et elle ne doit pas revenir.
        /// </summary>
        internal static long BatchSpawnCount;

        /// <summary>Nombre d'échanges requête-réponse effectués sur le transport batch.</summary>
        internal static long BatchExchangeCount;

        /// <summary>
        /// Lit le blob de <c>HEAD:&lt;rel&gt;</c> (relatif à la racine du dépôt) —
        /// une seule fois par run, les lectures suivantes servent la mémoire.
        /// </summary>
        /// <param name="rel">Chemin dépôt, séparateurs « / ».</param>
        /// <exception cref="InvalidOperationException">
        /// git absent, objet absent de HEAD, type non-blob, ou délai dépassé — le
        /// message NOMME la cause et le chemin, jamais une exception générique de
        /// <see cref="Process.ExitCode"/> levée sur un processus encore vivant.
        /// </exception>
        internal static byte[] CommittedBlob(string rel)
        {
            if (string.IsNullOrWhiteSpace(rel))
                throw new ArgumentException("chemin vide : rien à lire dans HEAD", nameof(rel));

            return CommittedBlobCache.GetOrAdd(rel, ReadCommittedBlobFromGit);
        }

        /// <summary>Lecture effective sous git, appelée au plus une fois par chemin et par run.</summary>
        private static byte[] ReadCommittedBlobFromGit(string rel) => BatchExchange("HEAD:" + rel, rel);

        /// <summary>
        /// Un échange requête-réponse sur le processus batch, sérialisé par
        /// <see cref="BatchGate"/> : le délai de 30 s couvre L'ÉCHANGE ENTIER —
        /// un git qui ne répond plus est tué, pas attendu, et l'appel suivant
        /// repartira d'un transport neuf.
        /// </summary>
        private static byte[] BatchExchange(string spec, string rel)
        {
            lock (BatchGate)
            {
                EnsureBatchProcess(rel);

                Interlocked.Increment(ref BatchExchangeCount);
                var trace = new BatchExchangeProgress();
                var exchange = Task.Run(() => ExchangeOnce(spec, rel, batchStdin, batchStdout, trace));
                return AwaitExchange(exchange, rel, trace, GitTimeoutMs, KillAndDropBatchProcess);
            }
        }

        /// <summary>
        /// Attend un échange K fois borné, et au timeout nomme la cause mesurée.
        /// Séparé de <see cref="BatchExchange"/> pour que le diagnostic soit
        /// exerçable sur un faux transport, sans processus git et sans risquer de
        /// tuer le transport réel partagé par les gardes parallèles.
        /// </summary>
        /// <param name="onTimeout">Ce qu'il faut jeter à l'expiration (le vrai transport, ou rien pour un faux).</param>
        internal static byte[] AwaitExchange(
            Task<(byte[]? Blob, bool Missing, string? NonBlobType)> exchange,
            string rel,
            BatchExchangeProgress trace,
            int timeoutMs,
            Action onTimeout)
        {
            if (!exchange.Wait(timeoutMs))
            {
                // Le processus ne répond plus : ExitCode lèverait une exception
                // générique. On nomme la cause réelle, et on tue l'orphelin —
                // un transport gelé ne doit pas geler le run entier derrière lui.
                var diagnosis = DescribeTimeout(rel, timeoutMs, trace.Snapshot());
                onTimeout();
                throw new InvalidOperationException(diagnosis);
            }

            (byte[]? Blob, bool Missing, string? NonBlobType) result;
            try
            {
                result = exchange.Result;
            }
            catch (AggregateException ex)
            {
                // Transport mort ou désynchronisé : l'état des tubes n'est plus
                // garanti — on jette le processus pour que l'appel suivant
                // reparte proprement, et on nomme ce qu'on sait de sa mort.
                var inner = ex.GetBaseException();
                var exit = batchProcess is { HasExited: true } dead
                    ? $" (git a quitté, code {dead.ExitCode})"
                    : string.Empty;
                DropBatchProcess();
                throw new InvalidOperationException(
                    $"git cat-file --batch sur '{rel}' : échange interrompu{exit} — "
                    + $"{inner.Message} ; stderr : {StderrSnapshot()}", inner);
            }

            if (result.Missing)
            {
                throw new InvalidOperationException(
                    $"git cat-file --batch : '{rel}' est absent de HEAD — "
                    + "si l'ajout n'est pas encore committé, ce rouge est attendu : "
                    + "committer AVANT de mesurer BOM et fins de ligne");
            }

            if (result.NonBlobType is { } badType)
            {
                throw new InvalidOperationException(
                    $"git cat-file --batch sur '{rel}' : l'objet est de type « {badType} », "
                    + "pas un blob — une garde ne mesure que des fichiers");
            }

            return result.Blob
                ?? throw new InvalidOperationException(
                    $"git cat-file --batch sur '{rel}' : réponse sans blob ni diagnostic "
                    + "— impossible par construction, le transport est jeté");
        }

        /// <summary>
        /// Message de timeout : il DIT ce que la trace a vu, et il en tire la
        /// cause. Les trois formes sont distinguables à la lecture du journal —
        /// c'est la condition (1) de #2138, et c'est ce qui manquait aux huit
        /// témoins : tous rendaient la même ligne.
        /// </summary>
        /// <param name="rel">Chemin dépôt, nommé pour retrouver la garde fautive.</param>
        /// <param name="timeoutMs">Le mur appliqué à l'échange.</param>
        /// <param name="s">Photographie de l'avancement au moment du timeout.</param>
        internal static string DescribeTimeout(string rel, int timeoutMs, BatchExchangeSnapshot s)
        {
            var head = $"git cat-file --batch sur '{rel}' n'a pas rendu la main en "
                + $"{timeoutMs / 1000} s (processus tué)";

            switch (s.Stage)
            {
                case BatchExchangeStage.NotStarted:
                    // L'état du pool au moment T est la mesure qui rend la famine
                    // visible : un pool large avec des tâches en attente dit que
                    // l'échange n'a simplement pas encore son tour, ce qui est la
                    // signature des témoins (défauts simultanés, re-runs verts).
                    var pool = $"pool : {ThreadPool.ThreadCount} threads, "
                        + $"{ThreadPool.PendingWorkItemCount} tâche(s) en attente, "
                        + $"{ThreadPool.CompletedWorkItemCount} terminée(s)";
                    return head + " — diagnostic : l'échange n'a JAMAIS DÉMARRÉ (aucun thread "
                        + "ne l'a pris en " + timeoutMs + " ms ; " + pool + ") : c'est le POOL "
                        + "DE THREADS qui est affamé (Task.Run attendu par Wait), pas git — "
                        + "les 30 s mesurent une attente de thread";

                case BatchExchangeStage.Started:
                    return head + " — diagnostic : l'échange a démarré à " + s.StartedAtMs + " ms "
                        + "mais n'a pas écrit sa requête en " + timeoutMs + " ms : blocage à "
                        + "l'ENTRÉE du transport (écriture sur stdin ou attente du verrou)";

                case BatchExchangeStage.RequestWritten:
                    return head + " — diagnostic : l'échange a démarré à " + s.StartedAtMs
                        + " ms, requête écrite à " + s.RequestAtMs + " ms, mais AUCUN EN-TÊTE "
                        + "n'est revenu en " + timeoutMs + " ms : git n'a pas répondu — c'est "
                        + "GIT qui est muet (dépôt verrouillé, lecteur lent), pas le pool";

                case BatchExchangeStage.HeaderRead:
                    return head + " — diagnostic : l'échange a démarré à " + s.StartedAtMs
                        + " ms, en-tête lu à " + s.HeaderAtMs + " ms (" + s.AnnouncedBytes
                        + " octets annoncés), mais " + s.PayloadBytesRead + " / "
                        + s.AnnouncedBytes + " octets du contenu lus : blocage AU MILIEU DU "
                        + "CONTENU — le tube de sortie ne rend plus (descendant qui en garde "
                        + "l'héritage ?)";

                default:
                    return head + " — diagnostic : l'échange a TERMINÉ à " + s.BlobAtMs
                        + " ms (" + s.PayloadBytesRead + " octets lus) mais la tâche n'a pas "
                        + "rendu la main : blocage APRÈS la lecture, à la sortie de l'échange";
            }
        }

        /// <summary>
        /// Démarre le processus batch s'il n'existe pas encore. Un processus mort
        /// est jeté puis remplacé : les échecs ne sont pas mémoïsés, l'appel qui a
        /// vu la mort signale, les suivants repartent d'un transport neuf.
        /// </summary>
        private static void EnsureBatchProcess(string rel)
        {
            if (batchProcess is { HasExited: false } && batchStdin is not null && batchStdout is not null)
                return;

            DropBatchProcess();

            var psi = new ProcessStartInfo("git", "cat-file --batch")
            {
                WorkingDirectory = TestRepoRoot.Find(),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            Process process;
            try
            {
                process = Process.Start(psi)
                    ?? throw new InvalidOperationException("git n'a pas démarré");
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                throw new InvalidOperationException(
                    $"git est introuvable sur le PATH : impossible de lire '{rel}'", ex);
            }

            batchProcess = process;
            batchStdin = process.StandardInput.BaseStream;
            batchStdout = process.StandardOutput.BaseStream;
            Interlocked.Increment(ref BatchSpawnCount);

            // stderr est vidé EN CONTINU : un tube d'erreur jamais lu se remplit
            // (64 Ko) et bloque l'enfant pour de bon — la panne qu'on prétend
            // éviter deviendrait la panne qu'on provoque.
            _ = DrainStderrAsync(process.StandardError.BaseStream);
        }

        /// <summary>
        /// UNE requête sur les tubes. Ne lève JAMAIS pour « absent de HEAD » ou
        /// « type non-blob » (résultats normaux, rendus comme tels) — seules les
        /// pannes de transport lèvent, car elles doivent jeter le processus.
        /// <para>
        /// Chaque étape est marquée dans <paramref name="trace"/> AVANT l'appel
        /// bloquant qui la suit : au timeout, la marque dit ce qui a été ATTEINT,
        /// jamais ce qui allait l'être. Les deux flux sont des paramètres pour que
        /// le contrôle inverse puisse brancher un faux transport sans processus.
        /// </para>
        /// </summary>
        /// <param name="spec">Spécification envoyée à <c>git cat-file --batch</c>.</param>
        /// <param name="rel">Chemin dépôt, pour les messages.</param>
        /// <param name="pipeIn">Entrée du transport ; <c>null</c> = absent.</param>
        /// <param name="pipeOut">Sortie du transport ; <c>null</c> = absent.</param>
        /// <param name="trace">Suivi d'avancement, alimenté à chaque étape.</param>
        internal static (byte[]? Blob, bool Missing, string? NonBlobType) ExchangeOnce(
            string spec, string rel, Stream? pipeIn, Stream? pipeOut, BatchExchangeProgress trace)
        {
            trace.Mark(BatchExchangeStage.Started);

            var stdin = pipeIn ?? throw new IOException("transport batch absent (stdin)");
            var stdout = pipeOut ?? throw new IOException("transport batch absent (stdout)");

            var request = Encoding.UTF8.GetBytes(spec + "\n");
            stdin.Write(request, 0, request.Length);
            stdin.Flush();
            trace.Mark(BatchExchangeStage.RequestWritten);

            var header = ReadLine(stdout);
            trace.Mark(BatchExchangeStage.HeaderRead);

            if (header.EndsWith(" missing", StringComparison.Ordinal))
            {
                trace.ReportHeader(0);
                return (null, true, null);
            }

            var parts = header.Split(' ');
            if (parts.Length != 3 || !long.TryParse(parts[2], out var size) || size < 0)
                throw new IOException($"réponse illisible sur le transport batch : « {header} »");

            trace.ReportHeader(size);

            if (parts[1] != "blob")
            {
                // Consommer quand même contenu + LF : un octet laissé dans le tube
                // désynchroniserait TOUS les échanges suivants.
                SkipExactly(stdout, size + 1, trace);
                trace.Mark(BatchExchangeStage.BlobRead);
                return (null, false, parts[1]);
            }

            var payload = new byte[size];
            ReadExactly(stdout, payload, trace);
            if (stdout.ReadByte() != '\n')
                throw new IOException("désynchronisation du transport batch : pas de LF final après le blob");

            trace.Mark(BatchExchangeStage.BlobRead);
            return (payload, false, null);
        }

        /// <summary>Une ligne du protocole (ASCII), bornée : une ligne sans fin est une désynchronisation.</summary>
        private static string ReadLine(Stream stream)
        {
            var line = new StringBuilder(128);
            int b;
            while ((b = stream.ReadByte()) >= 0)
            {
                if (b == '\n')
                    return line.ToString();
                if (b != '\r')
                    line.Append((char)b);
                if (line.Length > 4096)
                    throw new IOException("en-tête de réponse batch anormalement long (désynchronisation ?)");
            }

            throw new IOException("EOF sur le transport batch : git cat-file --batch est mort");
        }

        /// <summary>
        /// Lit exactement <paramref name="count"/> octets ; un EOF en plein blob
        /// est une mort du transport. L'avancement est reporté à chaque fragment :
        /// c'est lui qui distingue « bloqué au milieu du contenu » d'un transport
        /// muet, au timeout.
        /// </summary>
        private static void ReadExactly(Stream stream, byte[] buffer, BatchExchangeProgress trace)
        {
            var offset = 0;
            while (offset < buffer.Length)
            {
                var n = stream.Read(buffer, offset, buffer.Length - offset);
                if (n <= 0)
                    throw new IOException("EOF en plein blob : git cat-file --batch est mort");
                offset += n;
                trace.ReportBytes(offset);
            }
        }

        /// <summary>Ignore exactement <paramref name="count"/> octets (type non-blob : garder le protocole aligné).</summary>
        private static void SkipExactly(Stream stream, long count, BatchExchangeProgress trace)
        {
            var discard = new byte[8192];
            var remaining = count;
            while (remaining > 0)
            {
                var n = stream.Read(discard, 0, (int)Math.Min(discard.Length, remaining));
                if (n <= 0)
                    throw new IOException("EOF en plein objet : git cat-file --batch est mort");
                remaining -= n;
                trace.ReportBytes(count - remaining);
            }
        }

        /// <summary>
        /// Vide stderr en continu en gardant une fenêtre glissante des derniers
        /// caractères — le diagnostic d'une mort de git, pas un tube qui bloque.
        /// </summary>
        private static async Task DrainStderrAsync(Stream stream)
        {
            var buffer = new byte[4096];
            try
            {
                int n;
                while ((n = await stream.ReadAsync(buffer).ConfigureAwait(false)) > 0)
                {
                    var chunk = Encoding.UTF8.GetString(buffer, 0, n);
                    lock (StderrLock)
                    {
                        StderrTail.Append(chunk);
                        if (StderrTail.Length > 2000)
                            StderrTail.Remove(0, StderrTail.Length - 1000);
                    }
                }
            }
            catch
            {
                // Tube fermé au kill du processus : la fin de vie normale du drain.
            }
        }

        private static string StderrSnapshot()
        {
            lock (StderrLock)
            {
                var tail = StderrTail.ToString();
                return tail.Length > 400 ? tail[^400..] : tail;
            }
        }

        /// <summary>Kill puis disposal du transport : le gel annoncé au timeout, l'EOF constaté en échange.</summary>
        private static void KillAndDropBatchProcess()
        {
            try { batchProcess?.Kill(entireProcessTree: true); } catch { /* déjà mort */ }
            DropBatchProcess();
        }

        private static void DropBatchProcess()
        {
            try { batchStdin?.Dispose(); } catch { /* déjà fermé */ }
            try { batchStdout?.Dispose(); } catch { /* déjà fermé */ }
            try { batchProcess?.Dispose(); } catch { /* déjà fermé */ }
            batchStdin = null;
            batchStdout = null;
            batchProcess = null;
        }
    }
}
