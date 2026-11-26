using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
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
		public void Inject_PrependsTheFacesOnce_AndOnlyWhereAFamilyIsNamed()
		{
			var template = ".card { color: red; font-family: 'Barlow', 'Rubik', sans-serif; }";

			var injected = EmbeddedFontFaces.Inject(template);

			injected.Should().StartWith(EmbeddedFontFaces.Marker);
			injected.Should().EndWith(template,
				"the template's own rules must come last: same specificity, later wins");

			EmbeddedFontFaces.Inject(injected).Should().Be(injected,
				"a document rendered twice (page pool, retry) must not carry the CSS twice");

			var unreferenced = ".card { color: red; font-family: 'Dosis', sans-serif; }";
			EmbeddedFontFaces.Inject(unreferenced).Should().Be(unreferenced,
				"a document that names none of the embedded families gets no rule and no base64: " +
				"measured on the Memo face, which moved 364 314 px when the faces were injected into " +
				"a document that uses none of them (dossier 2026-11-26-1485-polices-avant-apres, §4)");

			EmbeddedFontFaces.Inject(string.Empty).Should().Be(string.Empty,
				"an empty style sheet names nothing, so it gets nothing");
		}

		[Theory]
		[InlineData("font-family: 'Barlow', 'Rubik', sans-serif;", "the plain declaration")]
		[InlineData("font: 300 1.2em 'Barlow';", "the shorthand carries the family too")]
		[InlineData("--font-heading: BARLOW;", "a custom property, any case (a CSS family name is case-insensitive)")]
		[InlineData("font-family: \"rubik\";", "double quotes")]
		[InlineData("font-family: 'Alfa Slab One';", "a name with a space")]
		[InlineData("font-family: 'Alfa  Slab\tOne';", "inner white space is not a single space")]
		[InlineData("/* 'Barlow' replaces DINPro */", "a comment is not a declaration, but the gate errs on injecting")]
		public void ReferencesAnEmbeddedFamily_SeesTheFamilyWhereverItIsNamed(string declaration, string because)
		{
			EmbeddedFontFaces.ReferencesAnEmbeddedFamily(declaration).Should().BeTrue(because + ": " +
				"a document the gate wrongly skips renders in the fallback font with no message — " +
				"the defect this whole change removes");
		}

		[Theory]
		[InlineData("font-family: 'Dosis', 'Gadugi', sans-serif;", "the Memo face's own stack")]
		[InlineData("font-family: 'Bebas Neue', 'Oswald', sans-serif;", "a family that stays remote")]
		[InlineData("font-family: 'Barlow2';", "a longer name is not the family")]
		[InlineData("font-family: 'Barlow-Light';", "so is a hyphenated one")]
		public void ReferencesAnEmbeddedFamily_DoesNotSeeWhatIsNotTheFamily(string css, string because)
		{
			EmbeddedFontFaces.ReferencesAnEmbeddedFamily(css).Should().BeFalse(because);
		}

		[Fact]
		public void ReferencesAnEmbeddedFamily_IsFalseWithoutAStyleSheet()
		{
			EmbeddedFontFaces.ReferencesAnEmbeddedFamily(null).Should().BeFalse();
			EmbeddedFontFaces.ReferencesAnEmbeddedFamily(string.Empty).Should().BeFalse();
		}

		[Fact]
		public void ReferencingACorpusOfTemplates_OnlyTheOnesThatNameAFamilyReceiveTheFaces()
		{
			var templates = Directory
				.EnumerateFiles(Path.Combine(TestRepoRoot.Find(), "Cards"), "*.json", SearchOption.AllDirectories)
				.Where(path => !path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
					.Any(segment => string.Equals(segment, "Archive", StringComparison.OrdinalIgnoreCase)))
				.Select(path => (Path: path, Document: JsonDocument.Parse(File.ReadAllText(path))))
				.Where(t => t.Document.RootElement.ValueKind == JsonValueKind.Object
					&& t.Document.RootElement.TryGetProperty("css", out _))
				.ToList();

			templates.Should().HaveCountGreaterOrEqualTo(10,
				"an anti-collapse floor, not the corpus size: the twelve card templates outside Archive/ " +
				"measured on 2026-11-26 are one more than the enabled CardSets use. A glob that stopped " +
				"matching would otherwise certify an empty set");

			var withoutFaces = templates
				.Where(t => !EmbeddedFontFaces.ReferencesAnEmbeddedFamily(
					t.Document.RootElement.GetProperty("css").GetString()))
				.Select(t => Path.GetFileName(t.Path))
				.OrderBy(name => name, StringComparer.Ordinal)
				.ToList();

			withoutFaces.Should().BeEquivalentTo(
				new[] { "Argumentum_Fallacies_Face_3_fr.json", "Argumentum_Memo_Face_fr.json" },
				"exactly two templates name none of the embedded families: the Memo face, whose render " +
				"moved 364 314 px with the faces injected into a document that uses none of them, and " +
				"Face_3, which no enabled document harvests. A third one arriving here is either a " +
				"template that lost its family — a regression the gate would now turn into a silent " +
				"fallback — or a new template the gate starves");

			foreach (var template in templates)
			{
				var outsideCss = template.Document.RootElement
					.EnumerateObject()
					.Where(p => p.Name != "css"
						&& p.Value.ValueKind == JsonValueKind.String
						&& EmbeddedFontFaces.ReferencesAnEmbeddedFamily(p.Value.GetString()))
					.Select(p => p.Name)
					.ToList();

				outsideCss.Should().BeEmpty(
					"the gate reads the css key only, and {0} names an embedded family outside it: a " +
					"document the gate skips would render in the fallback font with no message",
					Path.GetFileName(template.Path));
			}
		}

		[Fact]
		public void EveryVendoredBinary_MatchesItsPinnedDigest()
		{
			EmbeddedFontFaces.DeclaredDigests.Should().HaveCount(EmbeddedFontFaces.DeclaredFiles.Count,
				"every declared file must carry exactly one digest, or a file could be vendored with no " +
				"expectation attached to it");

			foreach (var relative in EmbeddedFontFaces.DeclaredFiles)
			{
				var path = Path.Combine(FontDirectory, relative.Replace('/', Path.DirectorySeparatorChar));
				var digest = Convert.ToHexString(
					System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();

				digest.Should().Be(EmbeddedFontFaces.DeclaredDigests[relative],
					"{0} must be the file whose digest was measured and published in Cards/fonts/README.md; " +
					"a download that came back different is the silent fallback this class exists to remove",
					relative);
			}
		}

		[Fact]
		public void TheFontsReadme_HoldsTheSameDigestTable()
		{
			var readme = File.ReadAllText(Path.Combine(FontDirectory, "README.md"));

			var rows = Regex.Matches(
				readme,
				@"^\|\s*`([^`]+\.woff2)`\s*\|\s*(\d+)\s*\|\s*`([0-9a-f]{64})`\s*\|\s*$",
				RegexOptions.Multiline);

			rows.Should().HaveCount(EmbeddedFontFaces.DeclaredFiles.Count,
				"the README is the published table: one row per vendored binary, none missing, none extra");

			long total = 0;
			foreach (Match row in rows)
			{
				var file = row.Groups[1].Value;
				var bytes = long.Parse(row.Groups[2].Value, CultureInfo.InvariantCulture);

				EmbeddedFontFaces.DeclaredDigests.Should().ContainKey(file,
					"the README publishes {0}, which EmbeddedFontFaces does not declare", file);
				EmbeddedFontFaces.DeclaredDigests[file].Should().Be(row.Groups[3].Value,
					"the README and the declaration must pin the same digest for {0}, or the published " +
					"table certifies a file the harvest does not check", file);

				var path = Path.Combine(FontDirectory, file.Replace('/', Path.DirectorySeparatorChar));
				new FileInfo(path).Length.Should().Be(bytes,
					"the README publishes {0}'s size, and a size is the first thing a truncated download changes", file);

				total += bytes;
			}

			var stated = Regex.Match(readme, @"([\d\s]+)bytes of woff2 in total");
			stated.Success.Should().BeTrue(
				"the README states the total; without it the table is a sum nobody made");

			long.Parse(Regex.Replace(stated.Groups[1].Value, @"\s+", string.Empty), CultureInfo.InvariantCulture)
				.Should().Be(total,
					"the total the README publishes must be the sum of its own rows");
		}

		[Fact]
		public void AlteredBinary_Throws_InsteadOfEmbeddingIt()
		{
			using var directory = FontDirectoryCopy.WithFlippedByte("barlow/Barlow-Bold-latin.woff2", 100);

			Action build = () => EmbeddedFontFaces.BuildCss(directory.Path);

			build.Should().Throw<InvalidDataException>()
				.Which.Message.Should().Contain("SHA-256")
				.And.Contain("Barlow-Bold-latin.woff2",
					"the counter-review measured this mutation green on every test: the four magic bytes " +
					"were intact, and Chromium would have rejected the face without a word");
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

			// Read through the comments, not around them: a guard that searched the raw file was
			// green when the call was commented out — the natural form of a regression, and the
			// mutation the counter-review measured (228 tests, exit code 0).
			var code = WithoutComments(source);

			code.Should().Contain("EmbeddedFontFaces.Inject",
				"the scrubber must not be the reason the call disappears: the live line carries it");

			var method = Regex.Match(code,
				@"Task<CardPenHarvest>\s+GenerateImages\s*\(\s*IPage\s+page\s*,\s*CardSetPayload\s+cardSetDocument\s*,\s*CardSetInfo\s+cardSetInfo\s*\)");
			method.Success.Should().BeTrue(
				"this guard reads HarvestManager.GenerateImages(IPage, CardSetPayload, CardSetInfo). Its " +
				"signature changed: re-point the guard, or it certifies a method it no longer reads");

			var inject = Regex.Match(code, @"EmbeddedFontFaces\.Inject\((\w+)\.CardSetDocument\.css\)");
			inject.Success.Should().BeTrue(
				"no live call to EmbeddedFontFaces.Inject remains in HarvestManager.cs: the vendored fonts " +
				"are declared, tested, and never sent to CardPen — every template falls back to whatever " +
				"font the machine has");

			inject.Index.Should().BeGreaterThan(method.Index,
				"the faces are injected inside GenerateImages, the method that serialises the document");

			var body = code.Substring(method.Index, inject.Index - method.Index);
			Regex.IsMatch(body, @"^\s*(?:public|private|internal|protected)\s", RegexOptions.Multiline)
				.Should().BeFalse(
					"another member is declared between the GenerateImages signature and the call: the " +
					"injection left the method that serialises the document");

			var serialize = new Regex(@"JsonSerializer\.Serialize\((\w+)\.CardSetDocument\)")
				.Match(code, inject.Index + inject.Length);
			serialize.Success.Should().BeTrue(
				"the document is no longer serialised after the injection: the css that was enriched is " +
				"not the css CardPen receives");
			serialize.Groups[1].Value.Should().Be(inject.Groups[1].Value,
				"the css that receives the faces must belong to the document that is serialised");

			// Inverse control, in the test: the same file with the call commented out must fail the
			// search above. Without it, a scrubber that removed everything would certify anything.
			const string call = "cardSetDocument.CardSetDocument.css = EmbeddedFontFaces.Inject(cardSetDocument.CardSetDocument.css);";
			var commented = source.Replace(call, "// " + call);
			commented.Should().NotBe(source, "the mutant must have landed: the call is still written as this guard expects");

			Regex.IsMatch(WithoutComments(commented), @"EmbeddedFontFaces\.Inject\((\w+)\.CardSetDocument\.css\)")
				.Should().BeFalse(
					"a commented-out call is exactly what the guard must see through: the counter-review " +
					"measured the raw-file version green on it");
		}

		/// <summary>
		/// The source with every comment character replaced by a space — same length, so an index
		/// found in the result is the same index in the argument. Literals are copied through, because
		/// their <c>//</c> and <c>/*</c> are text: a string, a character, a verbatim string with its
		/// doubled quotes, an interpolated string with the code of its holes (which may hold literals
		/// of its own) and a raw string. An apostrophe inside a comment never reaches the literal
		/// branch, the comment being skipped as a whole.
		/// </summary>
		private static string WithoutComments(string source)
		{
			var characters = source.ToCharArray();
			var index = 0;
			while (index < characters.Length)
			{
				index = ScanUnit(characters, index);
			}

			return new string(characters);
		}

		/// <summary>Scans one unit of code at <paramref name="index"/> and returns the index after it.</summary>
		private static int ScanUnit(char[] characters, int index)
		{
			var current = characters[index];
			var next = index + 1 < characters.Length ? characters[index + 1] : '\0';

			if (current == '/')
			{
				if (next == '/')
				{
					return BlankLineComment(characters, index);
				}

				return next == '*' ? BlankBlockComment(characters, index) : index + 1;
			}

			if (current == '\'')
			{
				return SkipCharacter(characters, index);
			}

			var verbatim = false;
			var interpolated = false;
			var quote = index;
			while (quote < characters.Length && (characters[quote] == '@' || characters[quote] == '$'))
			{
				if (characters[quote] == '@')
				{
					verbatim = true;
				}
				else
				{
					interpolated = true;
				}

				quote++;
			}

			if (quote >= characters.Length || characters[quote] != '"')
			{
				return index + 1;
			}

			var raw = quote + 2 < characters.Length && characters[quote + 1] == '"' && characters[quote + 2] == '"';
			return raw
				? SkipRawString(characters, quote)
				: SkipString(characters, quote, verbatim, interpolated);
		}

		private static int SkipCharacter(char[] characters, int openingQuote)
		{
			var content = openingQuote + 1;
			content = content < characters.Length && characters[content] == '\\'
				? SkipEscape(characters, content)
				: Math.Min(characters.Length, content + 1);
			return Math.Min(characters.Length, content + 1);
		}

		/// <summary>Returns the index just past the escape sequence whose backslash is at <paramref name="index"/>.</summary>
		private static int SkipEscape(char[] characters, int index)
		{
			var indexAfterBackslash = index + 1;
			if (indexAfterBackslash >= characters.Length)
			{
				return indexAfterBackslash;
			}

			var kind = characters[indexAfterBackslash];
			if (kind != 'u' && kind != 'U' && kind != 'x')
			{
				return indexAfterBackslash + 1;
			}

			var hexadecimal = kind == 'u' ? 4 : kind == 'U' ? 8 : 4;
			var digits = 0;
			while (digits < hexadecimal && indexAfterBackslash + 1 + digits < characters.Length
				&& Uri.IsHexDigit(characters[indexAfterBackslash + 1 + digits]))
			{
				digits++;
			}

			return indexAfterBackslash + 1 + digits;
		}

		private static int SkipString(char[] characters, int openingQuote, bool verbatim, bool interpolated)
		{
			var index = openingQuote + 1;
			while (index < characters.Length)
			{
				var current = characters[index];

				if (current == '"')
				{
					if (verbatim && index + 1 < characters.Length && characters[index + 1] == '"')
					{
						index += 2;
						continue;
					}

					return index + 1;
				}

				if (interpolated && current == '{')
				{
					if (index + 1 < characters.Length && characters[index + 1] == '{')
					{
						index += 2;
						continue;
					}

					index = ScanHole(characters, index + 1);
					continue;
				}

				index += !verbatim && current == '\\' ? 2 : 1;
			}

			return index;
		}

		/// <summary>
		/// The code of an interpolation hole, from just after its <c>{</c> to just after its matching
		/// <c>}</c> — comments included, since they are comments.
		/// </summary>
		private static int ScanHole(char[] characters, int index)
		{
			var depth = 1;
			while (index < characters.Length && depth > 0)
			{
				if (characters[index] == '}')
				{
					depth--;
					index++;
				}
				else if (characters[index] == '{')
				{
					depth++;
					index++;
				}
				else
				{
					index = ScanUnit(characters, index);
				}
			}

			return index;
		}

		private static int SkipRawString(char[] characters, int openingQuote)
		{
			var quotes = 0;
			while (openingQuote + quotes < characters.Length && characters[openingQuote + quotes] == '"')
			{
				quotes++;
			}

			var index = openingQuote + quotes;
			while (index < characters.Length)
			{
				if (characters[index] != '"')
				{
					index++;
					continue;
				}

				var run = 0;
				while (index + run < characters.Length && characters[index + run] == '"')
				{
					run++;
				}

				if (run >= quotes)
				{
					return index + run;
				}

				index += run;
			}

			return index;
		}

		private static int BlankLineComment(char[] characters, int index)
		{
			var end = index;
			while (end < characters.Length && characters[end] != '\n')
			{
				end++;
			}

			Blank(characters, index, end);
			return end;
		}

		private static int BlankBlockComment(char[] characters, int index)
		{
			var end = index + 2;
			while (end + 1 < characters.Length && !(characters[end] == '*' && characters[end + 1] == '/'))
			{
				end++;
			}

			end = end + 1 < characters.Length ? end + 2 : characters.Length;
			Blank(characters, index, end);
			return end;
		}

		private static void Blank(char[] characters, int from, int to)
		{
			for (var position = from; position < to; position++)
			{
				if (characters[position] != '\n' && characters[position] != '\r')
				{
					characters[position] = ' ';
				}
			}
		}

		[Theory]
		[InlineData("var a = 1; // commented();\nlive();", "var a = 1;", "commented();")]
		[InlineData("/* commented();\n   still commented(); */ live();", "live();", "commented();")]
		[InlineData("var url = \"http://x/y\"; live();", "http://x/y", null)]
		[InlineData("var s = \"/* not a comment */\"; live();", "live();", null)]
		[InlineData("var c = '\"'; live();", "live();", null)]
		[InlineData("var v = @\"a \"\"quoted\"\" // not a comment\"; // commented();\nlive();", "not a comment", "commented();")]
		[InlineData("var t = $\"{(a ? \"x\" : \"y\")}\"; // commented();\nlive();", "live();", "commented();")]
		[InlineData("var r = \"\"\"raw \"\" not closed // here\"\"\"; // commented();\nlive();", "not closed // here", "commented();")]
		[InlineData("// l'appel est commenté();\nlive();", "live();", "commenté();")]
		[InlineData("var p = $\"{{literal}} {name}\"; // commented();\nlive();", "{{literal}} {name}", "commented();")]
		public void WithoutComments_BlanksCommentsAndNothingElse(string source, string kept, string? blanked)
		{
			var code = WithoutComments(source);

			code.Should().HaveLength(source.Length, "an index found in the result must be the index in the argument");
			code.Count(c => c == '\n').Should().Be(source.Count(c => c == '\n'), "a line found in the result must be the line in the argument");

			code.Should().Contain(kept, "text that is not a comment is copied through, quotes included");
			if (blanked != null)
			{
				code.Should().NotContain(blanked, "a comment is not code, whatever it quotes");
			}
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

			/// <summary>One byte flipped past the header: the WOFF2 magic survives, the file does not.</summary>
			public static FontDirectoryCopy WithFlippedByte(string relativeFile, int offset)
			{
				var copy = Clone();
				var path = System.IO.Path.Combine(copy.Path, relativeFile.Replace('/', System.IO.Path.DirectorySeparatorChar));
				var bytes = File.ReadAllBytes(path);
				bytes[offset] ^= 0xFF;
				File.WriteAllBytes(path, bytes);
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
