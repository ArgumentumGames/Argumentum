using System.Runtime.CompilerServices;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Process-wide state the whole test run needs, set ONCE before any test executes.
	/// <para>
	/// <see cref="Program.IsInteractive"/> defaults to <c>true</c> for the console app: when the
	/// mind map generators find no SVG export, they print instructions and wait for a key press
	/// (<c>DisplaySvgFileNotFoundMessage</c>). No test process has anyone at the keyboard, so in
	/// a test that wait never ends.
	/// </para>
	/// <para>
	/// The flag used to be flipped by each test that needed it (save, assign <c>false</c>,
	/// restore in <c>finally</c>). That is the #1192 hazard: two classes doing it are two xUnit
	/// collections running in parallel, and the restore of one puts <c>true</c> back while the
	/// other is still generating. Measured on CI: <c>CrossLinkTintTableTests</c> and
	/// <c>MmGeneratorTests</c> overlapping in time, and the job stalled at the key press until
	/// it was cancelled (runs 42412342409 attempt 1, 42479460671 attempt 1) — the same four
	/// <c>CrossLinkTintTableTests</c> missing from both logs, while the jobs where the two
	/// classes did not overlap passed.
	/// </para>
	/// <para>
	/// The cure follows the #1192 doctrine: remove the mutation from the tests. One value, set
	/// before the first test, never written again — there is nothing left to race with.
	/// <c>ProcessWideStaticMutationGuardTests</c> forbids any test from assigning the flag again.
	/// </para>
	/// </summary>
	internal static class TestProcessSetup
	{
#pragma warning disable CA2255 // A test assembly is the documented legitimate use of a module initializer.
		[ModuleInitializer]
		internal static void DisableInteractivePrompts() => Program.IsInteractive = false;
#pragma warning restore CA2255
	}
}
