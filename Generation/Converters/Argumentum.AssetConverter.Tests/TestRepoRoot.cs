using System;
using System.IO;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Locates the repository root at runtime by walking up from the test assembly's output
	/// directory until it finds the <c>Cards/Fallacies</c> marker directory.
	/// </summary>
	/// <remarks>
	/// This replaces the fragile <c>Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..",
	/// "..", ... ×6))</c> idiom that computes the repo root by counting directory segments. That
	/// count breaks the instant the test project is moved or the TFM output path depth changes
	/// (e.g. <c>net9.0</c> vs <c>net9.0-windows</c>). The walk-up is depth-agnostic and survives
	/// relocation (issue noted by ai-01 cycle 64 on <c>MindMapSvgEncodingTests</c>).
	/// </remarks>
	internal static class TestRepoRoot
	{
		/// <summary>
		/// Marker directory present at the repository root. <c>Cards/Fallacies</c> is committed
		/// and stable; it is the same anchor used by the harvesting/pipeline code paths.
		/// </summary>
		private static readonly string Marker = Path.Combine("Cards", "Fallacies");

		/// <summary>
		/// Returns the absolute path to the repository root, located by walking up from the test
		/// assembly's base directory until a parent contains the <c>Cards/Fallacies</c> marker.
		/// Throws if the marker is not found (e.g. running the DLL outside a checkout).
		/// </summary>
		public static string Find()
		{
			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, Marker)))
			{
				dir = dir.Parent;
			}
			return dir?.FullName
				?? throw new DirectoryNotFoundException(
					$"Could not locate repository root (marker '{Marker}' not found above '{AppContext.BaseDirectory}').");
		}
	}
}
