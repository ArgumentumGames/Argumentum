using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Argumentum.AssetConverter;

/// <summary>
/// The card fonts travel inside the payload, as <c>data:</c> URIs built from the binaries
/// vendored under <c>Cards/fonts/</c> (#1485, 3.b).
/// </summary>
/// <remarks>
/// <para>
/// Until this class existed, every card template declared its own <c>@font-face</c> blocks
/// pointing at third-party hosts. That is a print reproducibility hazard with no failure
/// signal: the day a host throttles or disappears, the harvest still succeeds and the PNG
/// that reaches the printer carries a different typeface. For the faces declared here —
/// DINPro's four weights, now Barlow with Rubik carrying the Cyrillic, and the Rules masthead
/// (Alfa Slab One) — embedding removes the dependency: there is no request to fail.
/// </para>
/// <para>
/// <b>What is not here.</b> The templates still load one Google Fonts stylesheet through
/// their <c>extCSS</c> key for <c>Bebas Neue</c>, <c>PT Sans</c> and the ar/fa/zh glyph
/// families (<c>Noto Naskh Arabic</c>, <c>Noto Sans SC</c>, <c>Vazirmatn</c>). Those loads are
/// measured, load-bearing and pinned template by template by
/// <c>ExternalFontHostRatchetContractTests</c>; they are not embedded by #1485. A harvest is
/// therefore not offline yet — it is offline <i>for the substituted families</i>.
/// </para>
/// <para>
/// <b>Why a data URI and not a relative URL.</b> A relative <c>url()</c> is rewritten to
/// <c>/Cards/…</c> for the local CardPen (<see cref="CardSetInfo"/>), which the local site
/// serves from its own tree — never from the worktree being rendered, so a font added on a
/// branch 404s and the card silently falls back (measured 2026-11-26: the served
/// <c>/Cards/</c> is the main checkout's). A <c>data:</c> URI is the only form that is
/// identical in Debug and Release, on a branch and on <c>master</c>.
/// </para>
/// <para>
/// <b>Fail loud.</b> A missing font file throws, naming the path. A silent skip here would
/// be the very defect this class removes.
/// </para>
/// </remarks>
public static class EmbeddedFontFaces
{
	/// <summary>
	/// Overrides the directory the fonts are read from. Test and CI hook; unset in a normal
	/// run, where the directory is found by walking up from the executable.
	/// </summary>
	public const string FontDirectoryEnvironmentVariable = "ARGUMENTUM_FONT_DIR";

	/// <summary>Repo-relative location of the vendored binaries.</summary>
	public const string FontDirectoryRelativePath = "Cards/fonts";

	/// <summary>
	/// First line of the built CSS. <see cref="Inject"/> uses it to recognise a payload that
	/// already carries the faces, so a document rendered twice is not embedded twice.
	/// </summary>
	public const string Marker = "/* argumentum-embedded-fonts */";

	private const string Woff2Magic = "wOF2";

	/// <summary>One <c>@font-face</c> rule. <see cref="WeightHigh"/> equals <see cref="WeightLow"/> for a static face.</summary>
	private sealed class Face
	{
		public Face(string family, int weightLow, int weightHigh, string file, string unicodeRange)
		{
			Family = family;
			WeightLow = weightLow;
			WeightHigh = weightHigh;
			File = file;
			UnicodeRange = unicodeRange;
		}

		public string Family { get; }
		public int WeightLow { get; }
		public int WeightHigh { get; }
		public string File { get; }

		/// <summary>Copied verbatim from the CSS that served the file; empty = no range.</summary>
		public string UnicodeRange { get; }
	}

	/// <summary>
	/// The declared faces, in CSS order. The <c>unicode-range</c> of a face that shares its
	/// family and weight with another (Barlow latin / latin-ext, Rubik cyrillic / cyrillic-ext)
	/// is not optional: without it the later rule replaces the earlier one instead of
	/// splitting the code space, and the lost subset's glyphs fall through to the system font.
	/// </summary>
	private static readonly Face[] Faces =
	{
		new Face("Barlow", 300, 300, "barlow/Barlow-Light-latin.woff2",
			"U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+0304, U+0308, U+0329, U+2000-206F, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD"),
		new Face("Barlow", 300, 300, "barlow/Barlow-Light-latin-ext.woff2",
			"U+0100-02BA, U+02BD-02C5, U+02C7-02CC, U+02CE-02D7, U+02DD-02FF, U+0304, U+0308, U+0329, U+1D00-1DBF, U+1E00-1E9F, U+1EF2-1EFF, U+2020, U+20A0-20AB, U+20AD-20C4, U+2113, U+2C60-2C7F, U+A720-A7FF"),
		new Face("Barlow", 500, 500, "barlow/Barlow-Medium-latin.woff2",
			"U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+0304, U+0308, U+0329, U+2000-206F, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD"),
		new Face("Barlow", 500, 500, "barlow/Barlow-Medium-latin-ext.woff2",
			"U+0100-02BA, U+02BD-02C5, U+02C7-02CC, U+02CE-02D7, U+02DD-02FF, U+0304, U+0308, U+0329, U+1D00-1DBF, U+1E00-1E9F, U+1EF2-1EFF, U+2020, U+20A0-20AB, U+20AD-20C4, U+2113, U+2C60-2C7F, U+A720-A7FF"),
		new Face("Barlow", 700, 700, "barlow/Barlow-Bold-latin.woff2",
			"U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+0304, U+0308, U+0329, U+2000-206F, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD"),
		new Face("Barlow", 700, 700, "barlow/Barlow-Bold-latin-ext.woff2",
			"U+0100-02BA, U+02BD-02C5, U+02C7-02CC, U+02CE-02D7, U+02DD-02FF, U+0304, U+0308, U+0329, U+1D00-1DBF, U+1E00-1E9F, U+1EF2-1EFF, U+2020, U+20A0-20AB, U+20AD-20C4, U+2113, U+2C60-2C7F, U+A720-A7FF"),
		new Face("Barlow", 900, 900, "barlow/Barlow-Black-latin.woff2",
			"U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+0304, U+0308, U+0329, U+2000-206F, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD"),
		new Face("Barlow", 900, 900, "barlow/Barlow-Black-latin-ext.woff2",
			"U+0100-02BA, U+02BD-02C5, U+02C7-02CC, U+02CE-02D7, U+02DD-02FF, U+0304, U+0308, U+0329, U+1D00-1DBF, U+1E00-1E9F, U+1EF2-1EFF, U+2020, U+20A0-20AB, U+20AD-20C4, U+2113, U+2C60-2C7F, U+A720-A7FF"),
		new Face("Rubik", 300, 900, "rubik/Rubik-cyrillic-ext.woff2",
			"U+0460-052F, U+1C80-1C8A, U+20B4, U+2DE0-2DFF, U+A640-A69F, U+FE2E-FE2F"),
		new Face("Rubik", 300, 900, "rubik/Rubik-cyrillic.woff2",
			"U+0301, U+0400-045F, U+0490-0491, U+04B0-04B1, U+2116"),
		new Face("Alfa Slab One", 400, 400, "alfaslabone/AlfaSlabOne-Regular-latin.woff2",
			"U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+0304, U+0308, U+0329, U+2000-206F, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD"),
	};

	private static readonly Lazy<string> Built = new Lazy<string>(
		() => BuildCss(ResolveFontDirectory()),
		LazyThreadSafetyMode.ExecutionAndPublication);

	/// <summary>
	/// The <see cref="FontDirectoryRelativePath"/>-relative file of every declared face, in
	/// declaration order. Exposed so a contract test can hold the vendored directory and the
	/// declaration together: a file that is present and undeclared is dead weight, a declared
	/// file that is absent fails the build at harvest time.
	/// </summary>
	public static IReadOnlyList<string> DeclaredFiles { get; } =
		Array.ConvertAll(Faces, face => face.File);

	/// <summary>
	/// The <c>@font-face</c> rules for every vendored face, built once per process. Throws
	/// if the vendored directory or one of its files is missing — see the class remarks.
	/// </summary>
	public static string Css => Built.Value;

	/// <summary>
	/// Returns <paramref name="css"/> with <see cref="Css"/> prepended, unless it already
	/// carries the faces. The embedded rules come first so that a template's own rule of
	/// equal specificity still wins.
	/// </summary>
	public static string Inject(string css)
	{
		if (!string.IsNullOrEmpty(css) && css.StartsWith(Marker, StringComparison.Ordinal))
		{
			return css;
		}

		return Css + "\n" + (css ?? string.Empty);
	}

	/// <summary>
	/// Where the fonts are read from: <see cref="FontDirectoryEnvironmentVariable"/> if set,
	/// otherwise the first <c>Cards/fonts</c> directory found walking up from the executable
	/// — the run is <c>dotnet run</c> from <c>bin/&lt;Config&gt;/net9.0-windows</c>, so the
	/// repository root is a few levels up.
	/// </summary>
	public static string ResolveFontDirectory()
	{
		var configured = Environment.GetEnvironmentVariable(FontDirectoryEnvironmentVariable);
		if (!string.IsNullOrWhiteSpace(configured))
		{
			if (!Directory.Exists(configured))
			{
				throw new DirectoryNotFoundException(
					$"{FontDirectoryEnvironmentVariable} points at '{configured}', which does not exist. " +
					$"The card fonts cannot be embedded without it.");
			}

			return configured;
		}

		var directory = new DirectoryInfo(AppContext.BaseDirectory);
		while (directory != null)
		{
			var candidate = Path.Combine(directory.FullName, "Cards", "fonts");
			if (Directory.Exists(candidate))
			{
				return candidate;
			}

			directory = directory.Parent;
		}

		throw new DirectoryNotFoundException(
			$"No '{FontDirectoryRelativePath}' directory found walking up from '{AppContext.BaseDirectory}'. " +
			$"The card fonts cannot be embedded without it — set {FontDirectoryEnvironmentVariable} to the directory.");
	}

	/// <summary>
	/// Builds the <c>@font-face</c> rules from the files under <paramref name="fontDirectory"/>.
	/// Pure with respect to its argument: no cache, no environment, so a test can point it at
	/// any directory.
	/// </summary>
	public static string BuildCss(string fontDirectory)
	{
		if (string.IsNullOrWhiteSpace(fontDirectory) || !Directory.Exists(fontDirectory))
		{
			throw new DirectoryNotFoundException(
				$"The embedded card fonts were not found: '{fontDirectory}' does not exist. " +
				$"The card fonts cannot be embedded without it.");
		}

		var builder = new StringBuilder();
		builder.Append(Marker).Append('\n');

		foreach (var face in Faces)
		{
			var path = Path.Combine(fontDirectory, face.File.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(path))
			{
				throw new FileNotFoundException(
					$"The embedded card font '{face.File}' (family '{face.Family}') is missing. " +
					$"The card fonts cannot be embedded without it.", path);
			}

			var bytes = File.ReadAllBytes(path);
			var magic = Encoding.ASCII.GetString(bytes, 0, Math.Min(Woff2Magic.Length, bytes.Length));
			if (magic != Woff2Magic)
			{
				throw new InvalidDataException(
					$"'{face.File}' is not a WOFF2 file (starts with '{magic}', expected '{Woff2Magic}'). " +
					$"A file damaged by a line-ending conversion or a wrong download cannot be embedded.");
			}

			builder.Append("@font-face {\n");
			builder.Append("    font-family: '").Append(face.Family).Append("';\n");
			builder.Append("    font-style: normal;\n");
			builder.Append("    font-weight: ").Append(face.WeightLow);
			if (face.WeightHigh != face.WeightLow)
			{
				builder.Append(' ').Append(face.WeightHigh);
			}

			builder.Append(";\n");

			// `block`, not `swap`: a harvest must never capture a card before its face is
			// ready, and there is no network wait to minimise.
			builder.Append("    font-display: block;\n");
			if (!string.IsNullOrEmpty(face.UnicodeRange))
			{
				builder.Append("    unicode-range: ").Append(face.UnicodeRange).Append(";\n");
			}

			builder.Append("    src: url(data:font/woff2;base64,")
				.Append(Convert.ToBase64String(bytes))
				.Append(") format('woff2');\n");
			builder.Append("}\n");
		}

		return builder.ToString();
	}
}
