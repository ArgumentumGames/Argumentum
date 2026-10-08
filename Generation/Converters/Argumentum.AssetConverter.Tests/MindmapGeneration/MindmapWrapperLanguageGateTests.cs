using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.MindmapGeneration
{
    /// <summary>
    /// T4-a (pool #458) — language organ: an HTML wrapper under
    /// <c>Cards/Fallacies/Mindmaps/&lt;lang&gt;/</c> must declare that same language in its
    /// root <c>&lt;html lang="…"&gt;</c>.
    ///
    /// Why this organ exists: both root templates (<c>included.html</c>, <c>external.html</c>)
    /// hard-coded <c>lang="en"</c>. Every wrapper inherits whatever the template says, so the
    /// committed tree declared English for all of them — measured 2026-10-08 before the fix:
    /// <b>34/34 wrappers carried <c>lang="en"</c></b>, including <c>ar/</c>, <c>zh/</c> and
    /// <c>fr/</c>. That is not cosmetic: <c>lang</c> drives screen-reader pronunciation, the
    /// browser's translation prompt, hyphenation and search-engine language attribution — an
    /// Arabic map declared <c>en</c> is read aloud as broken English.
    ///
    /// The defect is invisible in a diff: one attribute inside a 0.5–6 MB file, in a build
    /// step whose skip is a plain <c>Logger.Log</c> (<c>OverwriteExistingHtmlMaps</c>, see
    /// <see cref="MindmapWrapperFreshnessGateTests"/>). Only a standing organ catches it.
    ///
    /// <b>Scope is all 34 wrappers, and a floor of 34 is correct here.</b> This deliberately
    /// differs from the defect-surface rule for the freshness organ, where a floor of 34 was
    /// red on a healthy tree: that metric counted inlined URLs/thumbnails, which the 17
    /// <c>_ext</c> wrappers carry zero of <em>by construction</em> (they reference the SVG
    /// through <c>&lt;object data&gt;</c>). A <c>lang</c> attribute is on the <c>&lt;html&gt;</c>
    /// element of <em>both</em> halves — measured 17/17 inlining + 17/17 <c>_ext</c> before the
    /// fix. So here the total IS the defect surface, and a collapsed count is a real red.
    /// </summary>
    public class MindmapWrapperLanguageGateTests
    {
        // ── Core detector (pure over a directory — calibratable on a fabricated tree) ─────

        /// <summary>The language a wrapper is expected to declare: its own directory name.</summary>
        internal static string ExpectedLanguageOf(string wrapperPath)
            => new DirectoryInfo(Path.GetDirectoryName(wrapperPath)!).Name;

        /// <summary>
        /// The language the wrapper actually declares, or <c>null</c> when it declares none.
        /// Reads the <c>&lt;html&gt;</c> start tag only — a <c>lang</c> on any other element
        /// (e.g. a <c>&lt;span lang="la"&gt;</c> inside the inlined SVG) is not the document
        /// language and must not be mistaken for it.
        /// </summary>
        internal static string? DeclaredLanguageOf(string wrapperHtml)
        {
            var m = Regex.Match(wrapperHtml, @"<html\b[^>]*\blang\s*=\s*""(?'lang'[^""]*)""",
                RegexOptions.IgnoreCase);
            return m.Success ? m.Groups["lang"].Value : null;
        }

        /// <summary>
        /// All wrappers under the mindmaps directory (both the inlining and the <c>_ext</c> half),
        /// ordered for stable failure messages.
        ///
        /// Enumerates the <c>&lt;lang&gt;/</c> directories ONLY: the two root templates
        /// (<c>included.html</c>, <c>external.html</c>) live directly under <c>Mindmaps/</c>, are
        /// the <em>source</em> of the wrappers rather than wrappers, and legitimately carry the
        /// unsubstituted <c>[LANG]</c> token — a recursive <c>SearchOption.AllDirectories</c>
        /// would flag them as mislabeled and calibrate this organ to a permanent red.
        /// </summary>
        internal static List<string> AllWrappers(string mindmapDir)
        {
            var files = new List<string>();
            foreach (var langDir in Directory.EnumerateDirectories(mindmapDir))
                files.AddRange(Directory.EnumerateFiles(langDir, "*.html", SearchOption.TopDirectoryOnly));
            return files.OrderBy(p => p, StringComparer.Ordinal).ToList();
        }

        /// <summary>The nominative list of mislabeled wrappers: declared language ≠ own directory.</summary>
        internal static List<string> MislabeledWrappers(string mindmapDir)
        {
            var bad = new List<string>();
            foreach (var wrapper in AllWrappers(mindmapDir))
            {
                var expected = ExpectedLanguageOf(wrapper);
                var declared = DeclaredLanguageOf(File.ReadAllText(wrapper));
                if (declared is null)
                    bad.Add($"{expected}/{Path.GetFileName(wrapper)} declares NO <html lang> attribute");
                else if (!string.Equals(declared, expected, StringComparison.Ordinal))
                    bad.Add($"{expected}/{Path.GetFileName(wrapper)} declares lang=\"{declared}\"");
            }
            return bad;
        }

        // ── The organ ────────────────────────────────────────────────────────────────────

        [Fact]
        public void Wrappers_DeclareTheLanguageOfTheirOwnDirectory()
        {
            var repoRoot = TestRepoRoot.Find();
            var mindmapDir = Path.Combine(repoRoot, "Cards", "Fallacies", "Mindmaps");

            var wrappers = AllWrappers(mindmapDir);
            wrappers.Should().HaveCount(c => c >= 34,
                "34 wrappers ship today — 17 inlining + 17 _ext across 8 languages, plus the FR-only "
                + "Fallacies_cards_fr pair; fewer means a wrapper family has collapsed. The two root "
                + "templates (included.html/external.html) sit at the directory root and are NOT wrappers");

            var bad = MislabeledWrappers(mindmapDir);
            bad.Should().BeEmpty(
                "a wrapper whose <html lang> does not match its own directory mislabels the document for "
                + "screen readers, search engines and the browser translation prompt (measured 34/34 "
                + "wrong before T4-a). Mislabeled: " + string.Join(" | ", bad));
        }

        /// <summary>
        /// Cross-check with the freshness organ: every inlining wrapper the other organ pairs to
        /// an SVG must have its <c>_ext</c> sibling on the tree. Ties the two halves together so
        /// the <c>_ext</c> family cannot silently disappear while the floor of 34 is satisfied by
        /// duplicates — the floor alone would not notice.
        /// </summary>
        [Fact]
        public void EveryInliningWrapper_HasAnExtSibling_AndBothAreChecked()
        {
            var repoRoot = TestRepoRoot.Find();
            var mindmapDir = Path.Combine(repoRoot, "Cards", "Fallacies", "Mindmaps");

            var pairs = MindmapWrapperFreshnessGateTests.FindPairs(mindmapDir);
            pairs.Should().HaveCount(c => c >= 17, "17 inlining wrapper/SVG pairs ship today");

            var checkedFiles = AllWrappers(mindmapDir).ToHashSet(StringComparer.Ordinal);
            var missing = new List<string>();
            foreach (var (wrapper, _) in pairs)
            {
                var ext = Path.Combine(Path.GetDirectoryName(wrapper)!,
                    Path.GetFileNameWithoutExtension(wrapper) + "_ext.html");
                if (!File.Exists(ext))
                    missing.Add(Path.GetFileName(ext));
                else
                    checkedFiles.Contains(ext).Should().BeTrue(
                        $"the language organ must also cover '{ext}'");
            }
            missing.Should().BeEmpty(
                "every inlining wrapper has an _ext sibling today; a missing one means the external "
                + "delivery half was lost. Missing: " + string.Join(" | ", missing));
        }

        // ── Calibration — prove the detector can see both a 1 and a 0 ─────────────────────

        /// <summary>
        /// Fabricated tree, no git needed: three wrappers, exactly one of them mislabeled. The
        /// detector must name that one and only that one. Includes the inverse control (the same
        /// tree, corrected) so the 1 is not read as "the detector always fires" — a detector that
        /// reds on everything proves nothing (a <c>0</c> is only an absence if the instrument
        /// could have seen a <c>1</c>).
        /// </summary>
        [Fact]
        public void Detector_NamesTheMislabeledWrapper_AndOnlyThatOne()
        {
            var root = Path.Combine(Path.GetTempPath(), "argu-langgate-" + Guid.NewGuid().ToString("N"));
            try
            {
                var mk = new Func<string, string, string>((lang, declared) =>
                {
                    var dir = Path.Combine(root, "Cards", "Fallacies", "Mindmaps", lang);
                    Directory.CreateDirectory(dir);
                    var p = Path.Combine(dir, $"W_{lang}{Guid.NewGuid().ToString("N")}.html");
                    File.WriteAllText(p, $"<!DOCTYPE html>\n<html lang=\"{declared}\">\n<body>x</body>\n</html>\n");
                    return p;
                });

                // Calibrated positives: correct in their own directory.
                mk("ar", "ar");
                mk("zh", "zh");
                // The single defect: an 'en'-inherited wrapper sitting in the French directory.
                var wrong = mk("fr", "en");

                var mindmapDir = Path.Combine(root, "Cards", "Fallacies", "Mindmaps");

                using (new FluentAssertions.Execution.AssertionScope())
                {
                    MislabeledWrappers(mindmapDir).Should().HaveCount(1,
                        "exactly one of the three calibrated wrappers is mislabeled");
                    MislabeledWrappers(mindmapDir).Single().Should().Contain(Path.GetFileName(wrong),
                        "the detector must name the mislabeled file, not merely count it");
                }

                // Inverse control: correct the defect in place — the detector must go to zero.
                File.WriteAllText(wrong, "<!DOCTYPE html>\n<html lang=\"fr\">\n<body>x</body>\n</html>\n");
                MislabeledWrappers(mindmapDir).Should().BeEmpty(
                    "with the defect corrected the detector must report nothing — otherwise it is "
                    + "calibrated to a constant red and cannot discriminate");

                // Second positive: a wrapper with no <html lang> at all is the same defect class
                // (an absent lang is not "unset", it is a document declaring nothing).
                var missingAttr = Path.Combine(root, "Cards", "Fallacies", "Mindmaps", "ru", "W_ru_none.html");
                Directory.CreateDirectory(Path.GetDirectoryName(missingAttr)!);
                File.WriteAllText(missingAttr, "<!DOCTYPE html>\n<html>\n<body>x</body>\n</html>\n");
                MislabeledWrappers(mindmapDir)
                    .Where(m => m.Contains("declares NO")).Should().HaveCount(1,
                        "a wrapper with no lang attribute must be flagged, not skipped");
            }
            finally
            {
                try { Directory.Delete(root, recursive: true); } catch { /* best effort */ }
            }
        }
    }
}
