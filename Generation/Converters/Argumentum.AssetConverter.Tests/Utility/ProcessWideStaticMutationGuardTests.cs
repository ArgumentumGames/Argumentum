using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Utility
{
	/// <summary>
	/// #1192 guard. A test that assigns a process-wide static races with every other xUnit
	/// collection running in parallel: it is GREEN in isolation and RED in the full suite —
	/// the worst signature, because the isolated re-run "proves" the test is fine.
	///
	/// That is exactly how <c>LoggerArchiveTests</c> shipped red on master: it swapped
	/// <c>Logger.LogFile</c>, and any concurrent <c>Logger.Log()</c> from another collection
	/// wrote through the swapped path. The cure was to remove the mutation (pass the path
	/// explicitly), not to serialise the whole suite.
	///
	/// This organ fails if the pattern comes back anywhere in the test project.
	/// </summary>
	public class ProcessWideStaticMutationGuardTests
	{
		/// <summary>
		/// Process-wide statics of the production code that a test must never assign. Add to
		/// this list when another process-wide mutable static appears.
		/// <para>
		/// <c>Program.IsInteractive</c>: flipped and restored by two parallel classes, the
		/// restore of one re-armed the key-press prompt under the other and stalled CI jobs
		/// (see <see cref="TestProcessSetup"/>). Its single sanctioned writer is that module
		/// initializer, exempted below by file name.
		/// </para>
		/// </summary>
		private static readonly string[] ForbiddenAssignments =
		{
			@"Logger\s*\.\s*LogFile\s*=(?!=)",
			@"Program\s*\.\s*IsInteractive\s*=(?!=)",
		};

		[Fact]
		public void NoTestSource_AssignsAProcessWideStatic()
		{
			var testRoot = Path.Combine(TestRepoRoot.Find(),
				"Generation", "Converters", "Argumentum.AssetConverter.Tests");
			Directory.Exists(testRoot).Should().BeTrue($"the test project must be locatable at '{testRoot}'");

			var offenders = Directory.EnumerateFiles(testRoot, "*.cs", SearchOption.AllDirectories)
				.Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
				         && !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
				         // this guard names the forbidden pattern; it must not flag itself
				         && !f.EndsWith(nameof(ProcessWideStaticMutationGuardTests) + ".cs", StringComparison.Ordinal)
				         // the one sanctioned writer: sets the flag once, before any test runs
				         && !string.Equals(Path.GetFileName(f), nameof(TestProcessSetup) + ".cs", StringComparison.Ordinal))
				.SelectMany(f => File.ReadLines(f)
					.Select((line, i) => (File: f, No: i + 1, Line: line))
					// a mention inside a comment documents the hazard, it does not create it
					.Where(x => !x.Line.TrimStart().StartsWith("//", StringComparison.Ordinal))
					.Where(x => ForbiddenAssignments.Any(p => Regex.IsMatch(x.Line, p))))
				.Select(x => $"{Path.GetFileName(x.File)}:{x.No} → {x.Line.Trim()}")
				.ToList();

			offenders.Should().BeEmpty(
				"assigning a process-wide static from a test races with the parallel suite "
				+ "(green alone, red in suite). Pass the value explicitly instead — see #1192.");
		}

		/// <summary>
		/// Contrôle inverse : l'organe doit pouvoir VOIR le défaut. Une garde incapable de
		/// rougir est un no-op, et un no-op se lit comme un succès.
		/// </summary>
		[Fact]
		public void TheGuardPattern_ActuallyMatchesTheDefect_AndSparesLegitimateCode()
		{
			var pattern = ForbiddenAssignments[0];

			Regex.IsMatch("            Logger.LogFile = liveLog;", pattern)
				.Should().BeTrue("the exact line that made master red must be caught");
			Regex.IsMatch("Logger . LogFile   =   x;", pattern)
				.Should().BeTrue("whitespace must not smuggle the assignment past the guard");

			Regex.IsMatch("var original = Logger.LogFile;", pattern)
				.Should().BeFalse("READING the static is harmless");
			Regex.IsMatch("Logger.LogFile == expected", pattern)
				.Should().BeFalse("comparing is not assigning");
		}

		/// <summary>
		/// Contrôle inverse du second motif : les deux lignes exactes qui ont bloqué la CI
		/// (l'assignation et la restauration) doivent être vues, la lecture épargnée.
		/// </summary>
		[Fact]
		public void TheInteractivePattern_SeesTheFlipAndTheRestore_AndSparesTheRead()
		{
			var pattern = ForbiddenAssignments[1];

			Regex.IsMatch("                Program.IsInteractive = false;", pattern)
				.Should().BeTrue("the flip that a parallel class undid must be caught");
			Regex.IsMatch("                Program.IsInteractive = originalInteractive;", pattern)
				.Should().BeTrue("the restore is the write that re-armed the prompt: it must be caught too");

			Regex.IsMatch("            var originalInteractive = Program.IsInteractive;", pattern)
				.Should().BeFalse("READING the flag is harmless");
			Regex.IsMatch("if (Program.IsInteractive == false)", pattern)
				.Should().BeFalse("comparing is not assigning");
		}

		/// <summary>
		/// The module initializer is what makes the removal safe: without it the flag keeps
		/// its console-app default (<c>true</c>) and any test reaching the SVG-not-found branch
		/// waits for a key press forever. Deterministic since no test writes the flag any more.
		/// </summary>
		[Fact]
		public void TheTestProcess_RunsNonInteractive()
		{
			Program.IsInteractive.Should().BeFalse(
				"TestProcessSetup must have disabled the key-press prompts before the first test");
		}
	}
}
