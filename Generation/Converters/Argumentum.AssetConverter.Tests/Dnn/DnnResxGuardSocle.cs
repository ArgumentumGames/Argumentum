using System;
using System.Diagnostics;
using System.IO;

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
    /// </summary>
    internal static class DnnResxGuardSocle
    {
        /// <summary>Délai au-delà duquel on renonce à mesurer, journalisé nommément.</summary>
        internal const int GitTimeoutMs = 30_000;

        /// <summary>
        /// Lit le blob de <c>HEAD:&lt;rel&gt;</c> (relatif à la racine du dépôt).
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
                var stderr = process.StandardError.ReadToEndAsync();
                using var buffer = new MemoryStream();
                process.StandardOutput.BaseStream.CopyTo(buffer);

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

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException(
                        $"git cat-file sur '{rel}' a échoué ({process.ExitCode}) : {stderr.Result} — "
                        + "si l'ajout n'est pas encore committé, ce rouge est attendu : "
                        + "committer AVANT de mesurer BOM et fins de ligne");
                }

                return buffer.ToArray();
            }
        }
    }
}
