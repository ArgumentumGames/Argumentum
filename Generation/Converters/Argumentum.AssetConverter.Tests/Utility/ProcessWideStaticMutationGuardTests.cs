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
		/// Statics of the production Logger that a test must never assign. Add to this list
		/// when another process-wide mutable static appears.
		/// </summary>
		private static readonly string[] ForbiddenAssignments =
		{
			@"Logger\s*\.\s*LogFile\s*=(?!=)",
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
				         && !f.EndsWith(nameof(ProcessWideStaticMutationGuardTests) + ".cs", StringComparison.Ordinal))
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
	}
}
