using System;

namespace Argumentum.AssetConverter.Mindmapper;

/// <summary>
/// #569/#1274 — the FreeMind SendKeys automation is dead without a foreground window on the
/// interactive desktop, and every one of its failure modes is silent. Measured chain: a detached
/// or minimized RDP session leaves <c>GetForegroundWindow()</c> NULL, <c>SendKeysSafe</c> swallows
/// the Win32Exception, a failed export logs a warning and the loop moves to the next document,
/// and the pipeline exits 0 over stale SVGs. This guard refuses to start instead: one throw
/// before FreeMind is even launched, carrying the operator remedy — not just the cause.
/// </summary>
public static class InteractiveForegroundGuard
{
	/// <summary>
	/// Throws if the interactive desktop has no foreground window. The window handle is injected
	/// so tests can prove the guard fires (control inverse: a guard never seen failing is a no-op).
	/// </summary>
	public static void EnsureForegroundWindowExists(Func<IntPtr> getForegroundWindow)
	{
		var hwnd = getForegroundWindow();
		if (hwnd == IntPtr.Zero)
		{
			throw new InvalidOperationException(
				"No foreground window on the interactive desktop (GetForegroundWindow() == 0). "
				+ "Every FreeMind SendKeys export would fail silently and the run would exit 0 over stale SVGs (#569, #1274). "
				+ "Remedy: the session is detached or minimized — reconnect the RDP session and keep it non-minimized, "
				+ "or park the console so an interactive desktop survives disconnections: "
				+ "tscon <sessionId> /dest:console  (sessionId from: query session). Then rerun the Mindmapper pass.");
		}
	}
}
