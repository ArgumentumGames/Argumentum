using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Contrôles inverses de l'instrument de #2138, condition (1) : au timeout, le
    /// message doit DIRE où l'échange s'est arrêté, et les trois causes doivent
    /// produire TROIS messages distincts.
    /// <para>
    /// Pourquoi un faux transport plutôt qu'un vrai gel de 30 s : un banc qui
    /// attendrait le mur réel coûterait une demi-minute par cas et ne pourrait pas
    /// provoquer un pool affamé de façon déterministe. Chaque cas construit ici
    /// l'état exact que le vrai chemin produit (tâche jamais démarrée, requête
    /// écrite sans réponse, en-tête lu et contenu interrompu) et passe par le MÊME
    /// <c>AwaitExchange</c> et le MÊME <c>DescribeTimeout</c> que la production.
    /// </para>
    /// <para>
    /// ⚠️ Sans ces trois cas, l'instrument serait un vert à vide : un
    /// <c>DescribeTimeout</c> que rien n'appelle, ou qui rendrait toujours la même
    /// ligne, passerait n'importe quelle relecture.
    /// </para>
    /// </summary>
    public class DnnResxGuardSocleTimeoutDiagnosisTests
    {
        private const string Rel = "DNNPlatform/Config/SiteUrls.config";
        private const int ShortTimeoutMs = 250;

        private static string TimeoutMessageFor(Task<(byte[]? Blob, bool Missing, string? NonBlobType)> exchange,
            BatchExchangeProgress trace)
        {
            var act = () => DnnResxGuardSocle.AwaitExchange(exchange, Rel, trace, ShortTimeoutMs, () => { });

            return act.Should().Throw<InvalidOperationException>(
                    "un échange qui n'a pas rendu la main doit lever, jamais rendre un blob vide")
                .Which.Message;
        }

        /// <summary>
        /// Cas 1 — la tâche n'a JAMAIS obtenu de thread : c'est la famine du pool,
        /// et c'est la seule lecture compatible avec des échecs simultanés sur des
        /// fichiers différents. Une <see cref="TaskCompletionSource{TResult}"/> non
        /// complétée reproduit exactement cet état : la tâche existe, elle ne
        /// s'exécute jamais.
        /// </summary>
        [Fact]
        public void A_TaskThatNeverStarted_IsDiagnosedAsThreadPoolStarvation()
        {
            var trace = new BatchExchangeProgress();
            var neverStarted = new TaskCompletionSource<(byte[]?, bool, string?)>(
                TaskCreationOptions.RunContinuationsAsynchronously).Task;

            var message = TimeoutMessageFor(neverStarted, trace);

            message.Should().Contain("n'a pas rendu la main",
                "le motif que le relevé CI compte doit survivre à l'instrument, sinon la "
                + "condition (3) de #2138 deviendrait inmesurable");
            message.Should().Contain("JAMAIS DÉMARRÉ",
                "l'échange n'a pas commencé : c'est la marque d'un pool affamé, la piste "
                + "SUPPOSÉE d'ai-01 — l'instrument doit la nommer, pas la supposer");
            message.Should().Contain("POOL DE THREADS",
                "la cause doit être nommée pour que le correctif puisse la viser");
            message.Should().NotContain("en-tête",
                "rien n'a été échangé : parler d'en-tête enverrait le diagnostic sur git, "
                + "qui n'a jamais été sollicité");
        }

        /// <summary>
        /// Cas 2 — l'échange A démarré et a écrit sa requête, mais aucun en-tête
        /// n'est revenu : c'est git qui est muet. Le faux transport rend zéro octet
        /// et bloque, comme un <c>cat-file</c> figé sur un verrou.
        /// </summary>
        [Fact]
        public void AnExchangeThatWroteItsRequestButGotNoHeader_IsDiagnosedAsSilentGit()
        {
            var trace = new BatchExchangeProgress();
            using var silent = new BlockingStream(header: string.Empty);

            var task = Task.Run(() => DnnResxGuardSocle.ExchangeOnce(
                "HEAD:" + Rel, Rel, Stream.Null, silent, trace));
            try
            {
                WaitForStage(trace, BatchExchangeStage.RequestWritten);
                var message = TimeoutMessageFor(task, trace);

                message.Should().Contain("requête écrite",
                    "l'instrument doit distinguer « rien n'a été envoyé » de « envoyé, pas de réponse »");
                message.Should().Contain("AUCUN EN-TÊTE",
                    "l'état atteint est l'attente de l'en-tête : la cause est côté git");
                message.Should().Contain("GIT",
                    "nommer git est tout l'intérêt du cas : le correctif ne vise pas le pool");
                message.Should().NotContain("JAMAIS DÉMARRÉ",
                    "la tâche a démarré : confondre les deux cas ferait corriger le mauvais coupable");
            }
            finally
            {
                silent.Release();
                Observe(task);
            }
        }

        /// <summary>
        /// Cas 3 — l'en-tête est arrivé, une partie du contenu aussi, puis plus
        /// rien : le tube ne rend plus. C'est le seul cas où les octets lus
        /// comptent, et c'est eux qui le distinguent du cas 2.
        /// </summary>
        [Fact]
        public void AnExchangeBlockedInsideThePayload_IsDiagnosedAsAPipeThatStoppedMidContent()
        {
            var trace = new BatchExchangeProgress();
            // 100 octets annoncés, 10 servis, puis blocage : « au milieu » au sens strict.
            using var partial = new BlockingStream(header: "0123456789abcdef blob 100\n", payloadBytes: "0123456789");

            var task = Task.Run(() => DnnResxGuardSocle.ExchangeOnce(
                "HEAD:" + Rel, Rel, Stream.Null, partial, trace));
            try
            {
                WaitForStage(trace, BatchExchangeStage.HeaderRead);
                var message = TimeoutMessageFor(task, trace);

                message.Should().Contain("AU MILIEU DU CONTENU",
                    "l'en-tête est arrivé et le contenu a commencé : ni git muet, ni pool affamé");
                message.Should().Contain("10 / 100 octets",
                    "les octets lus SONT la mesure qui sépare ce cas du précédent — un message "
                    + "sans eux rendrait les deux cas indiscernables");
                message.Should().NotContain("AUCUN EN-TÊTE",
                    "l'en-tête a été lu : le nier enverrait le diagnostic sur la mauvaise cause");
            }
            finally
            {
                partial.Release();
                Observe(task);
            }
        }

        /// <summary>
        /// Le contrôle qui rend les trois précédents signifiants : les messages
        /// doivent être DISTINCTS. Trois cas qui rendraient la même ligne
        /// reproduiraient exactement le défaut que l'instrument doit lever.
        /// </summary>
        [Fact]
        public void TheThreeCausesProduceThreeDifferentMessages()
        {
            var neverStarted = DnnResxGuardSocle.DescribeTimeout(Rel, ShortTimeoutMs,
                new BatchExchangeSnapshot(BatchExchangeStage.NotStarted, -1, -1, -1, -1, -1, 0));
            var noHeader = DnnResxGuardSocle.DescribeTimeout(Rel, ShortTimeoutMs,
                new BatchExchangeSnapshot(BatchExchangeStage.RequestWritten, 3, 4, -1, -1, -1, 0));
            var midContent = DnnResxGuardSocle.DescribeTimeout(Rel, ShortTimeoutMs,
                new BatchExchangeSnapshot(BatchExchangeStage.HeaderRead, 2, 3, 9, -1, 100, 10));

            new[] { neverStarted, noHeader, midContent }.Should().OnlyHaveUniqueItems(
                "trois causes distinctes qui rendraient le même texte laisseraient le "
                + "prochain témoin aussi aveugle que les huit précédents");
        }

        /// <summary>
        /// Le chemin nominal marque bien TOUTES les étapes : sans cette preuve, les
        /// trois cas ci-dessus pourraient passer sur un instrument qui ne reçoit
        /// jamais de marque et se contente d'afficher « pas démarré ».
        /// </summary>
        [Fact]
        public void ASuccessfulExchangeReachesTheLastStageAndReportsItsByteCount()
        {
            var trace = new BatchExchangeProgress();
            const string payload = "contenu-du-blob";
            using var complete = new BlockingStream(header: "deadbeef blob " + payload.Length + "\n",
                payloadBytes: payload + "\n", blockWhenExhausted: false);

            var result = DnnResxGuardSocle.ExchangeOnce("HEAD:" + Rel, Rel, Stream.Null, complete, trace);

            result.Blob.Should().NotBeNull();
            Encoding.UTF8.GetString(result.Blob!).Should().Be(payload,
                "le faux transport sert un protocole complet : le contenu rendu est celui annoncé");

            var snapshot = trace.Snapshot();
            snapshot.Stage.Should().Be(BatchExchangeStage.BlobRead,
                "le chemin nominal doit atteindre la dernière étape — sinon le timeout ne "
                + "pourrait jamais distinguer un succès d'un blocage après lecture");
            snapshot.AnnouncedBytes.Should().Be(payload.Length);
            snapshot.PayloadBytesRead.Should().Be(payload.Length,
                "le compte d'octets doit être celui du contenu, sans le LF protocolaire");
            snapshot.StartedAtMs.Should().BeGreaterThanOrEqualTo(0,
                "une marque réelle est un horodatage, jamais la valeur sentinelle -1");
        }

        /// <summary>
        /// Attend que l'échange ait ATTEINT une étape avant d'appliquer le délai.
        /// Sans cette attente, le banc mesure la latence de démarrage de
        /// <c>Task.Run</c> sous la suite complète — et il a échoué exactement ainsi
        /// une fois : c'est la famine du pool que l'instrument de #2138 traque,
        /// pas un défaut du socle. Un banc qui dépend de l'ordonnanceur est
        /// instable par construction, et le rouge qu'il produit est illisible.
        /// </summary>
        private static void WaitForStage(BatchExchangeProgress trace, BatchExchangeStage expected)
        {
            var deadline = DateTime.UtcNow.AddSeconds(10);
            while (trace.Snapshot().Stage < expected)
            {
                if (DateTime.UtcNow > deadline)
                    throw new InvalidOperationException(
                        $"le faux transport n'a pas atteint l'étape {expected} en 10 s : "
                        + "le banc ne peut plus rien mesurer (état observé : " + trace.Snapshot().Stage + ")");

                Thread.Sleep(5);
            }
        }

        private static void Observe(Task task) =>
            task.ContinueWith(t => _ = t.Exception, TaskContinuationOptions.OnlyOnFaulted);

        /// <summary>
        /// Faux transport : sert un en-tête puis un contenu, et BLOQUE quand la
        /// réserve est épuisée (sauf <c>blockWhenExhausted: false</c>, pour le cas
        /// nominal, où l'épuisement est une fin normale). <see cref="Release"/>
        /// débloque le thread pour que le banc ne laisse pas de tâche pendue.
        /// </summary>
        private sealed class BlockingStream : Stream
        {
            private readonly byte[] script;
            private readonly bool blockWhenExhausted;
            private readonly ManualResetEventSlim gate = new(false);
            private int position;

            internal BlockingStream(string header, string payloadBytes = "", bool blockWhenExhausted = true)
            {
                script = Encoding.UTF8.GetBytes(header + payloadBytes);
                this.blockWhenExhausted = blockWhenExhausted;
            }

            internal void Release() => gate.Set();

            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => throw new NotSupportedException();
            public override long Position
            {
                get => throw new NotSupportedException();
                set => throw new NotSupportedException();
            }

            public override void Flush() { }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (position < script.Length)
                {
                    var n = Math.Min(count, script.Length - position);
                    Array.Copy(script, position, buffer, offset, n);
                    position += n;
                    return n;
                }

                if (!blockWhenExhausted)
                    return 0;

                gate.Wait(TimeSpan.FromSeconds(5));
                return 0;
            }

            public override int ReadByte()
            {
                if (position < script.Length)
                    return script[position++];

                if (!blockWhenExhausted)
                    return -1;

                gate.Wait(TimeSpan.FromSeconds(5));
                return -1;
            }

            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
            public override void SetLength(long value) => throw new NotSupportedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    gate.Set();
                    gate.Dispose();
                }

                base.Dispose(disposing);
            }
        }
    }
}
