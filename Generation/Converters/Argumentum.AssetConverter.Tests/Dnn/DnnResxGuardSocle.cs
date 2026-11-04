using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
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
    /// <c>git cat-file</c> (exit 128) — c'est voulu, pas un défaut : mieux vaut un
    /// rouge bruyant qu'un vert optimiste sur un blob qui n'existe pas encore.
    /// </para>
    /// <para>
    /// Chaque chemin n'est lu sous git qu'<b>une fois par run</b> (mémoire interne,
    /// #2138) — les lectures répétées servent le blob déjà lu, ce qui supprime les
    /// spawns en rafale que les jambes CI parallèles ne survivaient pas toujours.
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
        /// <para>
        /// Pourquoi : un spawn <c>git cat-file</c> PAR LECTURE mettait des dizaines de
        /// processus en vol sur les jambes CI parallèles, et six témoins (#2138) ont
        /// montré des rafales entières tuées au mur de 30 s simultanément — gel du
        /// runner, pas fichier lent. Ne lire chaque fichier qu'une fois par run
        /// supprime la majorité des spawns ; le délai de 30 s reste inchangé.
        /// </para>
        /// </summary>
        private static readonly ConcurrentDictionary<string, byte[]> CommittedBlobCache =
            new(StringComparer.Ordinal);

        /// <summary>
        /// Lit le blob de <c>HEAD:&lt;rel&gt;</c> (relatif à la racine du dépôt) —
        /// une seule fois par run, les lectures suivantes servent la mémoire.
        /// </summary>
        /// <param name="rel">Chemin dépôt, séparateurs « / ».</param>
        /// <exception cref="InvalidOperationException">
        /// git absent, sortie non nulle, ou délai dépassé — le message NOMME la cause
        /// et le chemin, jamais une exception générique de <see cref="Process.ExitCode"/>
        /// levée sur un processus encore vivant.
        /// </exception>
        internal static byte[] CommittedBlob(string rel)
        {
            if (string.IsNullOrWhiteSpace(rel))
                throw new ArgumentException("chemin vide : rien à lire dans HEAD", nameof(rel));

            return CommittedBlobCache.GetOrAdd(rel, ReadCommittedBlobFromGit);
        }

        /// <summary>Lecture effective sous git, appelée au plus une fois par chemin et par run.</summary>
        private static byte[] ReadCommittedBlobFromGit(string rel)
        {
            var psi = new ProcessStartInfo("git", $"cat-file -p \"HEAD:{rel}\"")
            {
                WorkingDirectory = TestRepoRoot.Find(),
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

            using (process)
            {
                // Les DEUX tubes sont drainés en asynchrone, et cette forme n'est pas
                // cosmétique : un drain synchrone (`CopyTo`, ce que faisaient les trois
                // dernières copies) se bloque si l'enfant n'écrit plus sans fermer son
                // tube — et le délai placé APRÈS lui ne couvrirait alors rien du tout,
                // précisément dans le cas qu'il prétend couvrir (« dépôt verrouillé,
                // lecteur réseau lent »). Un `CopyTo` avant `WaitForExit` protège du
                // blocage d'écriture de l'enfant, pas de son blocage tout court.
                var stdout = DrainAsync(process.StandardOutput.BaseStream);
                var stderr = process.StandardError.ReadToEndAsync();

                if (!process.WaitForExit(GitTimeoutMs))
                {
                    // Le processus tourne encore : ExitCode lèverait une exception
                    // générique. On nomme la cause réelle, et on tue l'orphelin.
                    try { process.Kill(entireProcessTree: true); } catch { /* déjà terminé */ }
                    throw new InvalidOperationException(
                        $"git cat-file -p HEAD:{rel} n'a pas rendu la main en "
                        + $"{GitTimeoutMs / 1000} s (processus tué) : le dépôt est peut-être verrouillé "
                        + "par un autre index.lock ou un lecteur réseau lent");
                }

                bool drained;
                try
                {
                    drained = Task.WaitAll(new Task[] { stdout, stderr }, GitTimeoutMs);
                }
                catch (AggregateException ex)
                {
                    throw new InvalidOperationException(
                        $"git cat-file sur '{rel}' : lecture des tubes interrompue "
                        + $"({ex.GetBaseException().Message})", ex);
                }

                if (!drained)
                {
                    // Sorti, mais les tubes restent ouverts : un descendant garde
                    // l'héritage du descripteur. Sans cette borne, on attendrait ici
                    // indéfiniment un EOF qui ne vient pas.
                    try { process.Kill(entireProcessTree: true); } catch { /* déjà terminé */ }
                    throw new InvalidOperationException(
                        $"git cat-file -p HEAD:{rel} : processus sorti, mais ses tubes n'ont pas été "
                        + $"fermés en {GitTimeoutMs / 1000} s (un descendant en garde-t-il l'héritage ?)");
                }

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException(
                        $"git cat-file sur '{rel}' a échoué ({process.ExitCode}) : {stderr.Result} — "
                        + "si l'ajout n'est pas encore committé, ce rouge est attendu : "
                        + "committer AVANT de mesurer BOM et fins de ligne");
                }

                return stdout.Result;
            }
        }

        /// <summary>Vide un tube jusqu'à EOF, sans bloquer l'appelant.</summary>
        private static async Task<byte[]> DrainAsync(Stream stream)
        {
            using var buffer = new MemoryStream();
            await stream.CopyToAsync(buffer).ConfigureAwait(false);
            return buffer.ToArray();
        }
    }
}
