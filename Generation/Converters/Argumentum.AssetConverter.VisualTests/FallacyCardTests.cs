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
    /// pipeline regeneration, found via <see cref="TestRepoRoot.Find"/> + the converter's build
    /// output layout <c>bin/{Debug|Release}/net9.0-windows/Target/fr/Images/</c>. The control is
    /// a real artefact of the pipeline, the
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

            // 2. Actual pipeline output layout. The converter writes under its own build output
            // directory, NOT under the repository root: <repoRoot>/Generation/Converters/
            // Argumentum.AssetConverter/bin/{Debug|Release}/net9.0-windows/Target/{lang}/Images/.
            // #1072 anchored on '<repoRoot>/Target' — a layout asserted as "conventional" but
            // never measured against a populated tree; it exists on no machine, so this test was
            // red by construction from that merge (measured on ai-01, which carries both a
            // populated Debug and Release Target). Same root as PdfDimensionTests.cs:20-22.
            // Both configurations are probed because regenerations run in Release (CMYK bundle)
            // while local iterations run in Debug — the most recently written one wins.
            var converterBin = Path.Combine(
                repoRoot, "Generation", "Converters", "Argumentum.AssetConverter", "bin");

            var probed = new[] { "Release", "Debug" }
                .Select(cfg => Path.Combine(converterBin, cfg, "net9.0-windows", "Target", "fr", "Images"))
                .ToList();

            // The face cards do not sit at the top level of Images/: the harvester writes them
            // under <Images>/density-{n}/<CardSet>/. #1072 enumerated Images/ with
            // SearchOption.TopDirectoryOnly, which can only ever see the density-* directories
            // themselves — never a PNG. We target the Fallacies CardSet explicitly rather than
            // sweeping AllDirectories, because the sibling sets (Fallacies-Web, -Print&Play,
            // Memo, Rules) differ between configurations and would make "the alphabetically
            // first card" non-deterministic across machines.
            var imagesDir = probed
                .Where(Directory.Exists)
                .SelectMany(img => Directory.EnumerateDirectories(img, "density-*"))
                .Select(density => Path.Combine(density, "Fallacies"))
                .Where(Directory.Exists)
                .OrderByDescending(Directory.GetLastWriteTimeUtc)
                .FirstOrDefault();

            if (imagesDir is null)
            {
                Assert.Fail(
                    "No populated 'density-*/Fallacies' directory found. Probed Images roots:" +
                    string.Concat(probed.Select(p => $"{Environment.NewLine}  - {p}")) +
                    $"{Environment.NewLine}Run the pipeline at least once with " +
                    "`dotnet run --project Argumentum.AssetConverter` to populate " +
                    "Target/fr/Images/density-0/Fallacies/ before this test can run. " +
                    "See issue #957 — this is a fail-loud by design.");
            }

            _output.WriteLine($"Using CardSet directory: {imagesDir}");

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