using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Tests;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests
{
    /// <summary>
    /// Visual regression test for the Fallacies Tarot FR face card.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Prior implementation (cycles &lt; #1067) launched a full in-process pipeline + live
    /// Playwright harvest to capture a single test-only PNG named <c>chewbacca-defense_face.png</c>
    /// that did not exist on disk (cf. <c>git ls-files</c>, 0 hits). It was a *render-end-to-end
    /// 1 carte* test, but it relied on the network (CardPen local IIS), Chromium, and a
    /// CardPen template loaded by name. Every link in that chain was a *vector for a crash*,
    /// not a *control*: the very incident that triggered #1067 (Verify.ImageSharp version
    /// conflict, fixed in #1069 / PR #1069 by swapping to SixLabors.ImageSharp) blew up
    /// precisely here.
    /// </para>
    /// <para>
    /// This new implementation (#1067 design α, accepted by jsboige 2026-08-12) drops the
    /// live-harvest: the test now reads a **real Fallacies face card** produced by the last
    /// pipeline regeneration, found via <see cref="TestRepoRoot.Find"/> + the conventional
    /// <c>Target/fr/Images/</c> layout. The control is a real artefact of the pipeline, the
    /// dependency surface is one filesystem path, and the test fails loudly if
    /// <c>Target/</c> has not been populated by a recent regeneration (issue #957 — *Target
    /// empty but test passes*).
    /// </para>
    /// <para>
    /// The render-end-to-end coverage 1-carte migrates to the regeneration lane (po-2023)
    /// and to <c>PdfSnapshotTests</c> (which already lock PDF dimensions against the
    /// generated PDFs). What this test now guards is the simpler invariant: *the last
    /// regeneration produced a Fallacies face card that is visually stable against its
    /// baseline*.
    /// </para>
    /// </remarks>
    public class FallacyCardTests
    {
        private readonly ITestOutputHelper _output;

        public FallacyCardTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Render_NominalCard()
        {
            // 1. Locate the repository root via the shared TestRepoRoot helper.
            var repoRoot = TestRepoRoot.Find();

            // 2. Conventional pipeline output layout: <repoRoot>/Target/fr/Images/.
            // GetImagesDirectory is CWD-relative (Environment.CurrentDirectory), but VisualTests
            // can run with CWD != repoRoot, so we anchor on TestRepoRoot.Find() instead.
            var imagesDir = Path.Combine(repoRoot, "Target", "fr", "Images");

            if (!Directory.Exists(imagesDir))
            {
                Assert.Fail(
                    $"Target image directory not found at '{imagesDir}'. " +
                    "Run the pipeline at least once with `dotnet run --project Argumentum.AssetConverter` " +
                    "to populate Target/fr/Images/ before this test can run. " +
                    "See issue #957 — this is a fail-loud by design.");
            }

            // 3. Pick a representative Fallacies face card. We pick the first PNG whose name
            // ends in '_face.png' (the convention used by the harvesting pipeline), excluding
            // Virtues/Scenarii subtrees which live under their own directories. Failing loudly
            // here is preferred to silently falling back to a wrong artefact.
            var facePngs = Directory
                .EnumerateFiles(imagesDir, "*_face.png", SearchOption.TopDirectoryOnly)
                    .OrderBy(p => p, StringComparer.Ordinal)
                    .ToList();

            if (facePngs.Count == 0)
            {
                Assert.Fail(
                    $"No '*_face.png' artefacts found under '{imagesDir}'. " +
                    "The Fallacies regeneration did not produce any face card image. " +
                    "Re-run the pipeline, then re-run this test.");
            }

            // 4. Use a deterministic representative — the alphabetically first card — so the
            // baseline is reproducible across machines.
            var imageFile = facePngs[0];
            _output.WriteLine($"Using artefact: {imageFile}");

            var imageBytes = await File.ReadAllBytesAsync(imageFile);

            // 5. Visual snapshot via Verify. The first run on a fresh branch creates the
            // .verified.png baseline; subsequent runs compare against it. Failures reproduce
            // the diff image into TestResults/, so reviewers can eyeball the delta.
            await Verifier.Verify(imageBytes, "png");
        }
    }
}