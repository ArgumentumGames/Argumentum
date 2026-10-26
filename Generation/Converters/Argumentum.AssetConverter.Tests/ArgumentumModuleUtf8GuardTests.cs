using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// UTF-8 guard for the 2sxc <c>Argumentum</c> module (#1948 follow-up, pool #458 23/10 17:17 item 5).
	/// The W9 defect was two cp1252 <c>0xE9</c> bytes inside <c>argumentum.css</c> — a file served
	/// as <c>text/css</c> without a charset, so the browser decoded it with the page encoding and
	/// read U+FFFD. The stylesheet now declares its charset on line 1, and this guard keeps every
	/// text file of the module decodable as strict UTF-8.
	/// </summary>
	public class ArgumentumModuleUtf8GuardTests
	{
		private static readonly string ModuleRoot = Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "Portals", "1", "2sxc", "Argumentum");

		private static readonly string[] TextExtensions =
		{
			".cshtml", ".css", ".js", ".json", ".html", ".xml", ".txt"
		};

		/// <summary>Strict decoder: any invalid UTF-8 byte sequence throws instead of yielding U+FFFD.</summary>
		private static string DecodeStrictUtf8(byte[] bytes) =>
			new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true).GetString(bytes);

		[Fact]
		public void EveryModuleTextFileDecodesAsStrictUtf8()
		{
			var offenders = new List<string>();
			var scanned = 0;
			foreach (var path in Directory.EnumerateFiles(ModuleRoot, "*", SearchOption.AllDirectories)
				.Where(p => TextExtensions.Contains(Path.GetExtension(p), StringComparer.OrdinalIgnoreCase)))
			{
				scanned++;
				try
				{
					_ = DecodeStrictUtf8(File.ReadAllBytes(path));
				}
				catch (DecoderFallbackException)
				{
					offenders.Add(Path.GetRelativePath(TestRepoRoot.Find(), path));
				}
			}

			Assert.True(offenders.Count == 0,
				$"{scanned} files scanned; not decodable as strict UTF-8: {string.Join(", ", offenders)}");
			Assert.True(scanned > 0, $"no text file found under '{ModuleRoot}' — the guard is scanning nothing");
		}

		[Fact]
		public void ArgumentumCssDeclaresUtf8CharsetOnItsFirstLine()
		{
			var css = Path.Combine(ModuleRoot, "src", "styles", "argumentum.css");
			var firstLine = DecodeStrictUtf8(File.ReadAllBytes(css)).Split('\n')[0].TrimEnd('\r');
			Assert.Equal("@charset \"utf-8\";", firstLine);
		}

		[Fact]
		public void InverseControl_ALoneE9ByteMustFailStrictDecoding()
		{
			// The W9 defect byte: a lone cp1252 0xE9 is not valid UTF-8. A guard whose decoder
			// could not see it would report a healthy tree on a corrupted one — the empty-result
			// instrument trap. The real-file mutation replay is documented in the PR body.
			Assert.Throws<DecoderFallbackException>(() => DecodeStrictUtf8(new byte[] { 0x41, 0xE9, 0x42 }));
		}
	}
}
