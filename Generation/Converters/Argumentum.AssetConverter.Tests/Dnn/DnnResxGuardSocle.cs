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
                var exchange = Task.Run(() => ExchangeOnce(spec, rel));
                if (!exchange.Wait(GitTimeoutMs))
                {
                    // Le processus ne répond plus : ExitCode lèverait une exception
                    // générique. On nomme la cause réelle, et on tue l'orphelin —
                    // un transport gelé ne doit pas geler le run entier derrière lui.
                    KillAndDropBatchProcess();
                    throw new InvalidOperationException(
                        $"git cat-file --batch sur '{rel}' n'a pas rendu la main en "
                        + $"{GitTimeoutMs / 1000} s (processus tué) : le dépôt est peut-être verrouillé "
                        + "par un autre index.lock ou un lecteur réseau lent");
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
        /// </summary>
        private static (byte[]? Blob, bool Missing, string? NonBlobType) ExchangeOnce(string spec, string rel)
        {
            var stdin = batchStdin ?? throw new IOException("transport batch absent (stdin)");
            var stdout = batchStdout ?? throw new IOException("transport batch absent (stdout)");

            var request = Encoding.UTF8.GetBytes(spec + "\n");
            stdin.Write(request, 0, request.Length);
            stdin.Flush();

            var header = ReadLine(stdout);
            if (header.EndsWith(" missing", StringComparison.Ordinal))
                return (null, true, null);

            var parts = header.Split(' ');
            if (parts.Length != 3 || !long.TryParse(parts[2], out var size) || size < 0)
                throw new IOException($"réponse illisible sur le transport batch : « {header} »");

            if (parts[1] != "blob")
            {
                // Consommer quand même contenu + LF : un octet laissé dans le tube
                // désynchroniserait TOUS les échanges suivants.
                SkipExactly(stdout, size + 1);
                return (null, false, parts[1]);
            }

            var payload = new byte[size];
            ReadExactly(stdout, payload);
            if (stdout.ReadByte() != '\n')
                throw new IOException("désynchronisation du transport batch : pas de LF final après le blob");

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

        /// <summary>Lit exactement <paramref name="count"/> octets ; un EOF en plein blob est une mort du transport.</summary>
        private static void ReadExactly(Stream stream, byte[] buffer)
        {
            var offset = 0;
            while (offset < buffer.Length)
            {
                var n = stream.Read(buffer, offset, buffer.Length - offset);
                if (n <= 0)
                    throw new IOException("EOF en plein blob : git cat-file --batch est mort");
                offset += n;
            }
        }

        /// <summary>Ignore exactement <paramref name="count"/> octets (type non-blob : garder le protocole aligné).</summary>
        private static void SkipExactly(Stream stream, long count)
        {
            var discard = new byte[8192];
            while (count > 0)
            {
                var n = stream.Read(discard, 0, (int)Math.Min(discard.Length, count));
                if (n <= 0)
                    throw new IOException("EOF en plein objet : git cat-file --batch est mort");
                count -= n;
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
