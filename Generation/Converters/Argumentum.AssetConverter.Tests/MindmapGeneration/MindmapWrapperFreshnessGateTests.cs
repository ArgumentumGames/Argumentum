using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// #1438 grain ⑪ (pool #458 c.5750264212) — freshness organ: an inlining wrapper (.html
    /// that embeds a mindmap SVG verbatim) must never be OLDER than the .content.svg it
    /// inlines.
    ///
    /// Why this organ exists: the mindmap re-derivation repairs the SVGs, and the wrapper
    /// rewrite sits behind <c>OverwriteExistingHtmlMaps</c> (<c>AssetConverterConfig.cs:460</c>),
    /// which has NO initializer and therefore defaults to <c>false</c>; the skip guard is
    /// written three times and its skip is a plain <c>Logger.Log</c>, not a <c>LogWarning</c>
    /// (ai-01, #1438 c.5750260550). A re-derivation run without the named gesture leaves
    /// every wrapper embedding the PRE-repair SVG, and nothing in the log distinguishes that
    /// from success — the fallback-XSLT family of silent green. This organ catches the skip
    /// ON THE TREE (commit dates), not in the log.
    ///
    /// Pairing is rule-derived from the measured file naming (wrapper stems and SVG stems
    /// differ per family — served stem ≠ committed name):
    /// <code>
    ///   Fallacies_{lang}.content.svg               ↔ Fallacies_{lang}.html
    ///   Argumentum_Virtues_MindMap_{lang}.content.svg ↔ Argumentation_Virtues_{lang}.html
    ///   Argumentum_Fallacies_MindMap_cards_fr.content.svg ↔ Fallacies_cards_fr.html
    /// </code>
    /// Each pair is additionally VERIFIED by content (the wrapper must embed a mid-slice of
    /// its SVG), so a renamed or re-pointed file fails loud instead of pairing wrong.
    /// Content-only first-match pairing was measured INSUFFICIENT while scoping this organ:
    /// derived SVGs (cards vs full map) share slices and produce false pairs.
    ///
    /// Baseline measured by the authoring lane (2026-09-20): the committed tree is fully
    /// synchronized — 17/17 pairs carry the same commit date. This organ is GREEN today by
    /// measurement; its red day is the FUTURE: a re-derivation that commits new SVGs while
    /// the wrapper skip fires makes every touched pair stale, nominatively.
    ///
    /// Same explicit-run gate as <see cref="MindmapDeadLinkGateTests"/>
    /// (<c>ARGUMENTUM_DEADLINK_GATE=1</c>), inert in CI. The companion
    /// <c>ARGUMENTUM_DEADLINK_GATE=1 dotnet test --filter Gate</c> runs both organs. Requires
    /// a NON-shallow clone: commit dates are read from git, and on a shallow checkout every
    /// file reports the single fetched commit — the organ would be inert, so it throws
    /// instead of passing silently.
    /// </summary>
    public class MindmapWrapperFreshnessGateTests
    {
        private const string GateEnvVariable = "ARGUMENTUM_DEADLINK_GATE";

        // ── Core detector (pure over a repo root — calibratable on a fabricated repo) ─────

        /// <summary>All (wrapper, svg) pairs under the mindmaps directory, with a content guard.</summary>
        internal static List<(string Wrapper, string Svg)> FindPairs(string mindmapDir)
        {
            var pairs = new List<(string Wrapper, string Svg)>();
            foreach (var svg in Directory.EnumerateFiles(mindmapDir, "*.content.svg", SearchOption.AllDirectories)
                         .OrderBy(p => p, StringComparer.Ordinal))
            {
                var wrapper = WrapperNameFor(Path.GetFileName(svg));
                if (wrapper is null) continue;
                var wrapperPath = Path.Combine(Path.GetDirectoryName(svg)!, wrapper);
                if (!File.Exists(wrapperPath))
                    throw new FileNotFoundException(
                        $"FreshnessGate: content SVG '{svg}' has no expected inlining wrapper '{wrapperPath}'.", wrapperPath);
                // Content guard: the wrapper embeds a mid-slice of THIS svg. Guards against a
                // renamed/re-pointed pair; 400 chars of a >8 KB file are distinctive enough
                // given the pairing rule already ran (content alone was measured too weak).
                var svgText = File.ReadAllText(svg);
                if (svgText.Length <= 8_000)
                    throw new InvalidOperationException(
                        $"FreshnessGate: '{svg}' is unexpectedly small ({svgText.Length} chars) — the mid-slice guard assumes full exports.");
                var mid = svgText.Substring(svgText.Length / 2, 400);
                File.ReadAllText(wrapperPath).Should().Contain(mid,
                    $"the inlining wrapper '{wrapperPath}' must embed the mid-slice of the SVG it is paired to by naming rule");
                pairs.Add((wrapperPath, svg));
            }
            return pairs;
        }

        /// <summary>Maps a committed .content.svg file name to its inlining wrapper's name
        /// (served stem ≠ committed name — measured naming, three families).</summary>
        internal static string? WrapperNameFor(string contentSvgName) => contentSvgName switch
        {
            "Argumentum_Fallacies_MindMap_cards_fr.content.svg" => "Fallacies_cards_fr.html",
            var n when Regex.IsMatch(n, @"^Fallacies_(?'lang'\w+)\.content\.svg$")
                => Regex.Replace(n, @"^Fallacies_(?'lang'\w+)\.content\.svg$", "Fallacies_${lang}.html"),
            var n when Regex.IsMatch(n, @"^Argumentum_Virtues_MindMap_(?'lang'\w+)\.content\.svg$")
                => Regex.Replace(n, @"^Argumentum_Virtues_MindMap_(?'lang'\w+)\.content\.svg$", "Argumentation_Virtues_${lang}.html"),
            _ => null,
        };

        /// <summary>Commit date (ISO 8601) of the last commit touching a file, read from git.</summary>
        internal static DateTimeOffset CommitDateOf(string file, string workingDir)
        {
            var iso = RunGit($"log -1 --format=%cI -- \"{file}\"", workingDir);
            if (string.IsNullOrEmpty(iso))
                throw new InvalidOperationException(
                    $"FreshnessGate: no commit found for '{file}' — untracked files cannot be freshness-checked.");
            return DateTimeOffset.Parse(iso);
        }

        /// <summary>The nominative list of stale pairs: wrappers strictly older than their SVG.</summary>
        internal static List<string> StalePairs(string repoRoot)
        {
            var mindmapDir = Path.Combine(repoRoot, "Cards", "Fallacies", "Mindmaps");
            var stale = new List<string>();
            foreach (var (wrapper, svg) in FindPairs(mindmapDir))
            {
                var wrapperDate = CommitDateOf(wrapper, repoRoot);
                var svgDate = CommitDateOf(svg, repoRoot);
                if (wrapperDate < svgDate)
                    stale.Add($"{Path.GetFileName(wrapper)} ({wrapperDate:yyyy-MM-dd HH:mm}) embeds an SVG last written {svgDate:yyyy-MM-dd HH:mm} ({Path.GetFileName(svg)})");
            }
            return stale;
        }

        // ── The organ ────────────────────────────────────────────────────────────────────

        [Fact]
        public void InliningWrappers_AreNotOlderThanTheSvgTheyEmbed()
        {
            if (Environment.GetEnvironmentVariable(GateEnvVariable) != "1")
            {
                return;
            }

            var repoRoot = TestRepoRoot.Find();

            // Fail loud on a shallow clone: every file would report the single fetched commit
            // and the organ would be inert — the exact silence it exists to prevent.
            var shallow = RunGit("rev-parse --is-shallow-repository", repoRoot);
            if (shallow.Contains("true", StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "FreshnessGate: shallow clone detected — commit dates are meaningless here. "
                    + "Run the gate from a full clone.");

            var pairs = FindPairs(Path.Combine(repoRoot, "Cards", "Fallacies", "Mindmaps"));
            pairs.Should().HaveCount(c => c >= 17,
                "17 inlining wrapper/SVG pairs ship today (Fallacies + Argumentation_Virtues per language "
                + "+ Fallacies_cards_fr); fewer means the inline delivery has collapsed");

            var stale = StalePairs(repoRoot);
            stale.Should().BeEmpty(
                "an inlining wrapper older than the SVG it embeds means the SVG was re-derived and the "
                + $"wrapper rewrite was silently skipped (OverwriteExistingHtmlMaps=false, {GateEnvVariable}=1 "
                + "acceptance run). The named gesture is required on the re-derivation pass. Stale pairs: "
                + string.Join(" | ", stale));
        }

        // ── Calibration (required by the dispatch: prove the detector REDS on a known case) ──

        /// <summary>Body of the same-commit calibration minis: sequential zero-padded indexes
        /// (<c>000000000001…</c>) — every 400-char window is UNIQUE in the body (a run of
        /// identical chars would be periodic and a shifted window would re-match itself),
        /// which is what makes the window-shift proof below unambiguous.</summary>
        private static string SequentialBody(int count)
            => string.Concat(Enumerable.Range(0, count).Select(i => i.ToString("d6")));

        /// <summary>
        /// Grain ② of pool #458 v5 — the SAME-COMMIT case: a re-derivation that commits the
        /// new SVG while the wrapper rewrite is skipped, both in ONE commit. The date organ
        /// is structurally INERT here (equal dates), so the only net is the mid-slice
        /// content guard. Measured on the real tree (2026-09-20, po-2024): a dead-URL
        /// mutation lands inside the 400-char mid window for only 1/17 files — the guard
        /// does NOT catch the skip by direct hit. It catches it by SHIFT: the window is
        /// recomputed on the CURRENT file, so any mutation with total length delta Δ≠0
        /// (structural for #1438: +45 chars per wayback mirror, −URL per cleared link,
        /// 403 occurrences) moves the window by Δ/2, and the mid window is unique in 16/17
        /// real SVGs (2× in ru Virtues), so a moved window no longer matches the stale
        /// slice embedded in the skipped wrapper. This calibration proves that channel on
        /// a fabricated repo, with the WORST-CASE placement (mutation entirely in the last
        /// quarter, after the window — exactly the shape of zh Fallacies/Virtues, the two
        /// real files with zero mutations before the mid) and EQUAL commit dates.
        /// </summary>
        [Fact]
        public void ContentGuard_TurnsRedOnSameCommitSkip()
        {
            if (Environment.GetEnvironmentVariable(GateEnvVariable) != "1")
            {
                return;
            }

            var root = Path.Combine(Path.GetTempPath(), "argu-freshness-samecommit-" + Guid.NewGuid().ToString("N"));
            var dir = Path.Combine(root, "Cards", "Fallacies", "Mindmaps", "fr");
            Directory.CreateDirectory(dir);
            try
            {
                RunGit("init -q", root);
                var body = SequentialBody(4_000); // 24 000 chars, every 400-char window unique
                var svgPath = Path.Combine(dir, "Fallacies_fr.content.svg");
                var wrapperPath = Path.Combine(dir, "Fallacies_fr.html");
                File.WriteAllText(svgPath, body);
                File.WriteAllText(wrapperPath, "<html><body>" + body.Substring(body.Length / 2, 400) + "</body></html>");
                CommitAll(root, "2026-01-01T10:00:00", "pair committed together");

                // The silent skip, same-commit shape: the SVG is re-derived (last quarter
                // rewritten, net +400 chars — a Δ≠0 mutation AFTER the window, the worst
                // case), the wrapper is NOT touched, and both states land in ONE commit
                // carrying the SAME committer date as the initial one.
                var tail = body.Substring(body.Length - 300);
                var grown = body.Substring(0, body.Length - 300) + new string('Z', 100) + tail + tail;
                File.WriteAllText(svgPath, grown);
                CommitAll(root, "2026-01-01T10:00:00", "svg re-derived, wrapper skipped, same commit date");

                // Date organ: inert by construction on this shape — equal commit dates means
                // no pair can be stale by date. (StalePairs itself cannot be called here: it
                // runs FindPairs, whose content guard throws — which is exactly the point
                // proven next. The date channel is therefore asserted on its own primitive.)
                var wrapperDate = CommitDateOf(wrapperPath, root);
                var svgDate = CommitDateOf(svgPath, root);
                (wrapperDate == svgDate).Should().BeTrue(
                    "the date organ is structurally inert in the same-commit case (equal commit "
                    + "dates) — this assert documents the inertness the content guard must cover");

                // Content guard: the window moved (Δ/2 = +200) and windows are unique in this
                // body, so the wrapper's stale slice no longer matches — FindPairs must THROW.
                Action act = () => FindPairs(Path.Combine(root, "Cards", "Fallacies", "Mindmaps"));
                act.Should().Throw<Exception>().Which.Message.Should().Contain("mid-slice",
                    "the content guard must catch the same-commit skip via the window shift");
            }
            finally
            {
                DeleteReadOnlyTree(root);
            }
        }

        /// <summary>
        /// Control inverse of <see cref="ContentGuard_TurnsRedOnSameCommitSkip"/>: the
        /// measured BLIND SPOT of the content guard — a Δ=0 (iso-length) mutation landing
        /// outside the mid window leaves the window byte-identical, so neither the date
        /// organ (equal dates) nor the content guard (unchanged window) can see the skip.
        /// Documented and ACCEPTED (anti-pendulum, pool #458 v5 grain ②): a re-derivation
        /// whose total length delta is exactly zero across all 17 files is non-nominal for
        /// #1438 (wayback mirroring adds +45 chars per mirror, clearing removes whole
        /// URLs), and the date organ remains the net for cross-commit staleness. This test
        /// exists so the blind spot is a MEASURED, executable fact — not an unknown.
        /// </summary>
        [Fact]
        public void ContentGuard_DocumentedBlindSpot_ZeroDeltaMutationFarFromTheWindow()
        {
            if (Environment.GetEnvironmentVariable(GateEnvVariable) != "1")
            {
                return;
            }

            var root = Path.Combine(Path.GetTempPath(), "argu-freshness-zerodelta-" + Guid.NewGuid().ToString("N"));
            var dir = Path.Combine(root, "Cards", "Fallacies", "Mindmaps", "fr");
            Directory.CreateDirectory(dir);
            try
            {
                RunGit("init -q", root);
                var body = SequentialBody(4_000);
                var svgPath = Path.Combine(dir, "Fallacies_fr.content.svg");
                var wrapperPath = Path.Combine(dir, "Fallacies_fr.html");
                File.WriteAllText(svgPath, body);
                File.WriteAllText(wrapperPath, "<html><body>" + body.Substring(body.Length / 2, 400) + "</body></html>");
                CommitAll(root, "2026-01-01T10:00:00", "pair committed together");

                // Iso-length rewrite of the last 300 chars (Δ=0, far after the window) —
                // the wrapper is skipped, same commit date. Neither organ can see it.
                File.WriteAllText(svgPath, body.Substring(0, body.Length - 300) + new string('Q', 300));
                CommitAll(root, "2026-01-01T10:00:00", "svg re-derived iso-length, wrapper skipped, same date");

                var pairs = FindPairs(Path.Combine(root, "Cards", "Fallacies", "Mindmaps"));
                pairs.Should().HaveCount(1, "the content guard passes: the window did not move (Δ=0) nor change");
                StalePairs(root).Should().BeEmpty("the date organ passes: equal commit dates — the documented blind spot");
            }
            finally
            {
                DeleteReadOnlyTree(root);
            }
        }

        /// <summary>Fabricates a mini-repo where the SVG is committed AFTER its wrapper, with
        /// FORCED distinct committer dates (GIT_COMMITTER_DATE — no sleep), and proves the
        /// detector reports exactly that pair as stale. A detector that cannot red here is
        /// inert on the real tree too.</summary>
        [Fact]
        public void Detector_TurnsRedOnDeliberateDesynchronization()
        {
            if (Environment.GetEnvironmentVariable(GateEnvVariable) != "1")
            {
                return;
            }

            var root = Path.Combine(Path.GetTempPath(), "argu-freshness-calib-" + Guid.NewGuid().ToString("N"));
            var dir = Path.Combine(root, "Cards", "Fallacies", "Mindmaps", "fr");
            Directory.CreateDirectory(dir);
            try
            {
                RunGit("init -q", root);
                // A filler body large enough for the mid-slice guard (>8 KB), with a mid-slice
                // the wrapper embeds — same shape as the real exports.
                var body = string.Concat(Enumerable.Repeat("x", 6_000)) + "MIDSLICE-MARKER"
                         + string.Concat(Enumerable.Repeat("y", 6_000));
                var svgPath = Path.Combine(dir, "Fallacies_fr.content.svg");
                var wrapperPath = Path.Combine(dir, "Fallacies_fr.html");
                File.WriteAllText(svgPath, body);
                File.WriteAllText(wrapperPath, "<html><body>" + body.Substring(body.Length / 2, 400) + "</body></html>");
                CommitAll(root, "2026-01-01T10:00:00", "pair committed together");

                // The silent skip, reproduced: the SVG is re-derived and committed, the wrapper
                // is NOT touched. Distinct forced date, one hour later. The mutation touches
                // only the PREAMBLE of the SVG — the mid-slice stays byte-identical so the
                // content guard above stays green and this calibration isolates exactly what
                // it must prove: that the DATE comparison alone catches the skip (the content
                // guard is stricter and reds first whenever the re-derived SVG changed its
                // middle; this is the safety net behind it).
                File.WriteAllText(svgPath, body.Replace(string.Concat(Enumerable.Repeat("x", 6_000)),
                    string.Concat(Enumerable.Repeat("z", 6_000))));
                CommitAll(root, "2026-01-01T11:00:00", "svg re-derived, wrapper skipped");

                var stale = StalePairs(root);
                stale.Should().ContainSingle(
                    "exactly the deliberately desynchronized pair must be reported");
                stale[0].Should().Contain("Fallacies_fr.html",
                    "the stale report is nominative — it must name the wrapper");
            }
            finally
            {
                DeleteReadOnlyTree(root);
            }
        }

        /// <summary>git object files are read-only on Windows; a plain recursive delete throws
        /// UnauthorizedAccessException on them. Normalize attributes first.</summary>
        private static void DeleteReadOnlyTree(string path)
        {
            if (!Directory.Exists(path)) return;
            foreach (var f in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                File.SetAttributes(f, FileAttributes.Normal);
            Directory.Delete(path, recursive: true);
        }

        private static void CommitAll(string root, string committerDateIso, string message)
        {
            // -c user.* keeps the mini-repo independent of the machine's git identity;
            // GIT_COMMITTER_DATE/GIT_AUTHOR_DATE force distinct dates with no sleeps.
            RunGit($"-c user.email=cal@example.com -c user.name=cal add -A", root);
            RunGitWithEnv(new Dictionary<string, string>
                {
                    ["GIT_COMMITTER_DATE"] = committerDateIso,
                    ["GIT_AUTHOR_DATE"] = committerDateIso,
                },
                $"-c user.email=cal@example.com -c user.name=cal commit -q -m \"{message}\"", root);
        }

        private static string RunGit(string arguments, string workingDir)
            => RunGitWithEnv(new Dictionary<string, string>(), arguments, workingDir);

        private static string RunGitWithEnv(IReadOnlyDictionary<string, string> env, string arguments, string workingDir)
        {
            var psi = new ProcessStartInfo("git", arguments)
            {
                WorkingDirectory = workingDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            foreach (var (key, value) in env) psi.EnvironmentVariables[key] = value;
            using var process = Process.Start(psi)
                ?? throw new InvalidOperationException($"FreshnessGate: git failed to start for '{arguments}'.");
            var stdout = process.StandardOutput.ReadToEnd();
            var stderr = process.StandardError.ReadToEnd();
            process.WaitForExit(30_000);
            if (process.ExitCode != 0)
                throw new InvalidOperationException(
                    $"FreshnessGate: git {arguments} failed ({process.ExitCode}): {stderr}");
            return stdout.Trim();
        }
    }
}
