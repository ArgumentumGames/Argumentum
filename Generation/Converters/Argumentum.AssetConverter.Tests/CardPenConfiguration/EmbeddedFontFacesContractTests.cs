using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.CardPenConfiguration
{
	/// <summary>
	/// Contract for <see cref="EmbeddedFontFaces"/>: the card fonts are embedded in the payload
	/// from the binaries vendored under <c>Cards/fonts/</c>, and every way that can fail — a
	/// missing directory, a missing file, a file damaged by a line-ending conversion — throws
	/// and names what it looked for (#1485, 3.b).
	///
	/// <para><b>Why each assertion exists.</b> The defect this replaces was silent: a harvest
	/// that lost its font host still produced PNGs, with different glyphs and no message. A
	/// contract that only checked "some CSS is produced" would reproduce that silence one layer
	/// up, so the tests below check the bytes (each <c>data:</c> URI decodes to a WOFF2 whose
	/// length is the file's), the split of the code space (faces that share a family and a
	/// weight must carry a <c>unicode-range</c>, or the later one replaces the earlier one
	/// instead of completing it), and the loudness of each failure.</para>
	/// </summary>
	public class EmbeddedFontFacesContractTests
	{
		private static readonly Regex FaceBlock = new Regex(
			@"@font-face\s*\{(.*?)\}", RegexOptions.Singleline | RegexOptions.Compiled);

		private static readonly Regex Family = new Regex(
			@"font-family:\s*'([^']+)';", RegexOptions.Compiled);

		private static readonly Regex Weight = new Regex(
			@"font-weight:\s*(\d+)(?:\s+(\d+))?;", RegexOptions.Compiled);

		private static readonly Regex DataUri = new Regex(
			@"url\((data:font/woff2;base64,([A-Za-z0-9+/=]+))\)\s*format\('woff2'\)", RegexOptions.Compiled);

		private static readonly Regex UnicodeRange = new Regex(
			@"unicode-range:\s*([^;]+);", RegexOptions.Compiled);

		private static string FontDirectory =>
			Path.Combine(TestRepoRoot.Find(), "Cards", "fonts");

		private sealed record DeclaredFace(string Family, string Weight, string Base64, string UnicodeRange);

		private static List<DeclaredFace> DeclaredFaces(string css)
		{
			var faces = new List<DeclaredFace>();
			foreach (Match block in FaceBlock.Matches(css))
			{
				var body = block.Groups[1].Value;
				var family = Family.Match(body);
				var weight = Weight.Match(body);
				var uri = DataUri.Match(body);
				var range = UnicodeRange.Match(body);

				family.Success.Should().BeTrue("every embedded face declares its family: {0}", body);
				weight.Success.Should().BeTrue("every embedded face declares its weight: {0}", body);
				uri.Success.Should().BeTrue(
					"every embedded face carries its binary as a data: URI in woff2 format, so the " +
					"harvest has nothing to fetch: {0}", body);

				faces.Add(new DeclaredFace(
					family.Groups[1].Value,
					weight.Groups[2].Success
						? $"{weight.Groups[1].Value} {weight.Groups[2].Value}"
						: weight.Groups[1].Value,
					uri.Groups[2].Value,
					range.Success ? range.Groups[1].Value.Trim() : string.Empty));
			}

			return faces;
		}

		[Fact]
		public void BuildCss_CarriesOneFacePerDeclaredFile_AndStartsWithTheMarker()
		{
			var css = EmbeddedFontFaces.BuildCss(FontDirectory);
			var faces = DeclaredFaces(css);

			css.Should().StartWith(EmbeddedFontFaces.Marker,
				"Inject() recognises a payload that already carries the faces by this first line. " +
				"Without it a retried render embeds the CSS twice.");

			faces.Should().HaveCount(EmbeddedFontFaces.DeclaredFiles.Count,
				"every declared file must produce exactly one @font-face, and no face may exist " +
				"without a declared file: a missing face silently drops a weight to the fallback.");

			faces.Select(f => f.Family).Distinct().Should().BeEquivalentTo(
				new[] { "Barlow", "Rubik", "Alfa Slab One" },
				"three free families carry the cards (#1485 substitution table)");
		}

		[Fact]
		public void EveryEmbeddedBinary_IsTheVendoredFile_ByteForByte()
		{
			var faces = DeclaredFaces(EmbeddedFontFaces.BuildCss(FontDirectory));
			var byBase64 = faces.Select(f => f.Base64).ToHashSet(StringComparer.Ordinal);

			foreach (var relative in EmbeddedFontFaces.DeclaredFiles)
			{
				var path = Path.Combine(FontDirectory, relative.Replace('/', Path.DirectorySeparatorChar));
				File.Exists(path).Should().BeTrue("{0} is declared by EmbeddedFontFaces", relative);

				var bytes = File.ReadAllBytes(path);
				byBase64.Should().Contain(Convert.ToBase64String(bytes),
					"the embedded copy of {0} must be the file on disk — an embedded font that no " +
					"longer matches its file hides a damaged or stale download", relative);
			}
		}

		[Fact]
		public void EmbeddedBinaries_DecodeToWoff2()
		{
			foreach (var face in DeclaredFaces(EmbeddedFontFaces.BuildCss(FontDirectory)))
			{
				var bytes = Convert.FromBase64String(face.Base64);
				bytes.Length.Should().BeGreaterThan(1000, "a real woff2 subset, not a stub");
				System.Text.Encoding.ASCII.GetString(bytes, 0, 4).Should().Be("wOF2",
					"{0} {1} must carry the WOFF2 signature — the browser silently ignores a font " +
					"it cannot parse, which is the silent fallback this whole change removes",
					face.Family, face.Weight);
			}
		}

		[Fact]
		public void TheFourDINProWeights_AreAllPresent()
		{
			var faces = DeclaredFaces(EmbeddedFontFaces.BuildCss(FontDirectory));

			var barlow = faces.Where(f => f.Family == "Barlow").Select(f => f.Weight).ToList();
			barlow.Should().OnlyContain(w => new[] { "300", "500", "700", "900" }.Contains(w),
				"the templates ask DINPro for exactly these four weights (measured: every DINPro " +
				"@font-face in the ten templates, 300/500/700/900)");
			barlow.Distinct().Should().HaveCount(4,
				"all four weights must survive the substitution, or a heading silently renders at " +
				"a synthesised weight");

			faces.Where(f => f.Family == "Rubik").Should().OnlyContain(
				f => f.Weight == "300 900",
				"Rubik is embedded as one variable file per subset: its wght axis spans 300–900 " +
				"(measured on the vendored files, fvar), so it covers the same four weights");

			faces.Should().ContainSingle(f => f.Family == "Alfa Slab One" && f.Weight == "400",
				"the Rules masthead uses a single 400 face");
		}

		[Fact]
		public void FacesSharingFamilyAndWeight_CarryADistinctUnicodeRange()
		{
			var faces = DeclaredFaces(EmbeddedFontFaces.BuildCss(FontDirectory));

			foreach (var group in faces.GroupBy(f => (f.Family, f.Weight)))
			{
				if (group.Count() == 1)
				{
					continue;
				}

				group.Should().OnlyContain(f => !string.IsNullOrEmpty(f.UnicodeRange),
					"'{0}' {1} is carried by {2} faces (one per subset). Without a unicode-range the " +
					"later rule REPLACES the earlier one instead of splitting the code space, and " +
					"every glyph of the lost subset falls through to the system font — silently",
					group.Key.Family, group.Key.Weight, group.Count());

				group.Select(f => f.UnicodeRange).Distinct(StringComparer.Ordinal).Should().HaveCount(
					group.Count(),
					"two subsets of the same family and weight with the same range would still " +
					"replace each other");
			}

			var cyrillic = faces.Single(f => f.Family == "Rubik" && f.UnicodeRange.Contains("U+0400-045F"));
			cyrillic.Weight.Should().Be("300 900",
				"the Cyrillic face is the variable one, and it is what carries the `ru` cards: " +
				"Barlow has no Cyrillic at all (measured, #2389)");
		}

		[Fact]
		public void VendoredDirectory_HoldsExactlyTheDeclaredFiles()
		{
			var declared = EmbeddedFontFaces.DeclaredFiles
				.Select(f => f.Replace('\\', '/'))
				.OrderBy(f => f, StringComparer.Ordinal)
				.ToList();

			var onDisk = Directory
				.EnumerateFiles(FontDirectory, "*.woff2", SearchOption.AllDirectories)
				.Select(p => Path.GetRelativePath(FontDirectory, p).Replace('\\', '/'))
				.OrderBy(f => f, StringComparer.Ordinal)
				.ToList();

			onDisk.Should().BeEquivalentTo(declared,
				"a vendored font that is not declared is dead weight the payload pays for on every " +
				"CardSet, and a declared font that is absent fails at harvest time; keeping the two " +
				"lists equal is what makes the directory auditable");
		}

		[Fact]
		public void Licences_TravelWithTheBinaries()
		{
			foreach (var family in new[] { "Barlow", "Rubik", "AlfaSlabOne" })
			{
				var path = Path.Combine(FontDirectory, "licences", $"{family}-OFL.txt");
				File.Exists(path).Should().BeTrue(
					"the SIL Open Font License requires the licence text to travel with the font " +
					"binaries; {0} is vendored and embedded, so its licence must be in the repository",
					family);

				var text = File.ReadAllText(path);
				text.Should().Contain("SIL OPEN FONT LICENSE Version 1.1",
					"{0}'s licence file must be the OFL 1.1 text itself, not a placeholder", family);
			}
		}

		[Fact]
		public void MissingDirectory_Throws_AndNamesIt()
		{
			var missing = Path.Combine(Path.GetTempPath(), "argumentum-fonts-" + Guid.NewGuid().ToString("N"));

			Action build = () => EmbeddedFontFaces.BuildCss(missing);

			build.Should().Throw<DirectoryNotFoundException>()
				.Which.Message.Should().Contain(missing,
					"the failure message is the only thing that tells an operator what to restore");
		}

		[Fact]
		public void MissingFile_Throws_AndNamesTheFile()
		{
			using var directory = FontDirectoryCopy.Without("barlow/Barlow-Bold-latin.woff2");

			Action build = () => EmbeddedFontFaces.BuildCss(directory.Path);

			build.Should().Throw<FileNotFoundException>()
				.Which.Message.Should().Contain("Barlow-Bold-latin.woff2");
		}

		[Fact]
		public void DamagedFile_Throws_InsteadOfEmbeddingIt()
		{
			using var directory = FontDirectoryCopy.WithDamage("barlow/Barlow-Bold-latin.woff2");

			Action build = () => EmbeddedFontFaces.BuildCss(directory.Path);

			build.Should().Throw<InvalidDataException>()
				.Which.Message.Should().Contain("WOFF2",
					"a font file damaged by a CRLF round-trip still has the right name and size class, " +
					"so the signature is the only cheap proof that the bytes are a font");
		}

		[Fact]
		public void Inject_PrependsTheFacesOnce()
		{
			var template = ".card { color: red; }";

			var injected = EmbeddedFontFaces.Inject(template);

			injected.Should().StartWith(EmbeddedFontFaces.Marker);
			injected.Should().EndWith(template,
				"the template's own rules must come last: same specificity, later wins");

			EmbeddedFontFaces.Inject(injected).Should().Be(injected,
				"a document rendered twice (page pool, retry) must not carry the CSS twice");

			EmbeddedFontFaces.Inject(string.Empty).Should().Be(EmbeddedFontFaces.Css + "\n");
		}

		[Fact]
		public void ResolveFontDirectory_FindsTheVendoredDirectory()
		{
			var configured = Environment.GetEnvironmentVariable(EmbeddedFontFaces.FontDirectoryEnvironmentVariable);
			if (!string.IsNullOrEmpty(configured))
			{
				// The hook is set in this process: the resolver is expected to honour it, and its
				// own contract (throw when it points nowhere) is covered by MissingDirectory_Throws.
				Path.GetFullPath(configured).TrimEnd('\\', '/')
					.Should().BeEquivalentTo(Path.GetFullPath(EmbeddedFontFaces.ResolveFontDirectory()).TrimEnd('\\', '/'));
				return;
			}

			Path.GetFullPath(EmbeddedFontFaces.ResolveFontDirectory()).TrimEnd('\\', '/')
				.Should().BeEquivalentTo(Path.GetFullPath(FontDirectory).TrimEnd('\\', '/'),
					"the resolver walks up from the test assembly and must land on the same directory " +
					"the tests measure — if it does not, the payload and the contract are not the " +
					"same fonts");
		}

		/// <summary>
		/// The hook, pinned where it can be read. Everything above proves what
		/// <see cref="EmbeddedFontFaces"/> produces; none of it proves the harvest uses it.
		/// <see cref="EmbeddedFontFaces.Inject"/> is called once, in
		/// <c>HarvestManager.GenerateImages</c>, on the css of the document that is serialised
		/// for CardPen right after. That method needs a live Playwright page, so the call is read
		/// in the source, as <c>DnnResxGuardSocleTests</c> and <c>News5DateBoundGuardTests</c>
		/// read theirs. The behavioural proof stays the harvest (the payload starts with the
		/// marker); this guard only keeps a refactor from dropping the line while every other
		/// test here stays green.
		/// </summary>
		[Fact]
		public void HarvestManager_InjectsTheFacesIntoTheCssItSerialises()
		{
			var source = File.ReadAllText(Path.Combine(
				TestRepoRoot.Find(),
				"Generation", "Converters", "Argumentum.AssetConverter", "WebBasedGenerator", "Cardpen", "HarvestManager.cs"));

			var method = Regex.Match(source,
				@"Task<CardPenHarvest>\s+GenerateImages\s*\(\s*IPage\s+page\s*,\s*CardSetPayload\s+cardSetDocument\s*,\s*CardSetInfo\s+cardSetInfo\s*\)");
			method.Success.Should().BeTrue(
				"this guard reads HarvestManager.GenerateImages(IPage, CardSetPayload, CardSetInfo). Its " +
				"signature changed: re-point the guard, or it certifies a method it no longer reads");

			var inject = Regex.Match(source, @"EmbeddedFontFaces\.Inject\((\w+)\.CardSetDocument\.css\)");
			inject.Success.Should().BeTrue(
				"no call to EmbeddedFontFaces.Inject remains in HarvestManager.cs: the vendored fonts are " +
				"declared, tested, and never sent to CardPen — every template falls back to whatever " +
				"font the machine has");

			inject.Index.Should().BeGreaterThan(method.Index,
				"the faces are injected inside GenerateImages, the method that serialises the document");

			var body = source.Substring(method.Index, inject.Index - method.Index);
			Regex.IsMatch(body, @"^\s*(?:public|private|internal|protected)\s", RegexOptions.Multiline)
				.Should().BeFalse(
					"another member is declared between the GenerateImages signature and the call: the " +
					"injection left the method that serialises the document");

			var serialize = new Regex(@"JsonSerializer\.Serialize\((\w+)\.CardSetDocument\)")
				.Match(source, inject.Index + inject.Length);
			serialize.Success.Should().BeTrue(
				"the document is no longer serialised after the injection: the css that was enriched is " +
				"not the css CardPen receives");
			serialize.Groups[1].Value.Should().Be(inject.Groups[1].Value,
				"the css that receives the faces must belong to the document that is serialised");
		}

		/// <summary>A throwaway copy of <c>Cards/fonts</c>, one file removed or damaged.</summary>
		private sealed class FontDirectoryCopy : IDisposable
		{
			private FontDirectoryCopy(string path)
			{
				Path = path;
			}

			public string Path { get; }

			public static FontDirectoryCopy Without(string relativeFile)
			{
				var copy = Clone();
				File.Delete(System.IO.Path.Combine(copy.Path, relativeFile.Replace('/', System.IO.Path.DirectorySeparatorChar)));
				return copy;
			}

			public static FontDirectoryCopy WithDamage(string relativeFile)
			{
				var copy = Clone();
				File.WriteAllText(
					System.IO.Path.Combine(copy.Path, relativeFile.Replace('/', System.IO.Path.DirectorySeparatorChar)),
					"this is not a font");
				return copy;
			}

			private static FontDirectoryCopy Clone()
			{
				var target = System.IO.Path.Combine(
					System.IO.Path.GetTempPath(),
					"argumentum-fonts-copy-" + Guid.NewGuid().ToString("N"));
				System.IO.Directory.CreateDirectory(target);

				foreach (var source in System.IO.Directory.EnumerateFiles(FontDirectory, "*.woff2", SearchOption.AllDirectories))
				{
					var relative = System.IO.Path.GetRelativePath(FontDirectory, source);
					var destination = System.IO.Path.Combine(target, relative);
					System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(destination)!);
					File.Copy(source, destination);
				}

				return new FontDirectoryCopy(target);
			}

			public void Dispose()
			{
				try
				{
					System.IO.Directory.Delete(Path, recursive: true);
				}
				catch (IOException)
				{
					// A leftover temp directory is not worth failing a green test over.
				}
			}
		}
	}
}
