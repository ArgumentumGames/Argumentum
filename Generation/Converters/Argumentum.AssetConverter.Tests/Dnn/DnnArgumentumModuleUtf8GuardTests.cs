using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Dnn
{
	/// <summary>
	/// Garde d'encodage du module 2sxc `Argumentum/` (suite de #1948 W9, pool #458 item 5).
	///
	/// INCIDENT FONDATEUR (2026-10/11, #1948) — le défaut a pris les DEUX formes, et chaque
	/// forme aveugle un instrument différent :
	///   1. `argumentum.css` portait deux octets `E9` (cp1252) : le fichier **n'était pas de
	///      l'UTF-8 valide**, et servi `text/css` sans charset le navigateur décode avec
	///      l'encodage du document -> U+FFFD.
	///   2. `_RulesExplorer_RuleDetail.cshtml` portait déjà le U+FFFD **en UTF-8 valide**
	///      (`EF BF BD`) : un décodeur strict ne voit RIEN, et un `grep` sur `é` ne le voit pas
	///      davantage. C'est le mojibake figé.
	/// Les deux côtés décodaient vers le même U+FFFD, donc les sélecteurs matchaient encore et
	/// la palette rendait 7/7 : le défaut était **invisible à l'œil** et n'aurait rougi nulle
	/// part. #1948 a réparé les fichiers appariés ; il n'a laissé aucune garde. C'est elle ici.
	///
	/// Pourquoi deux assertions et pas une : décoder strictement attrape la forme 1 et rate la
	/// forme 2 ; chercher U+FFFD attrape la forme 2 et rate la forme 1 (un fichier non-UTF-8 ne
	/// se décode pas, on ne peut même pas y chercher). Les deux ensemble couvrent l'incident.
	///
	/// Le périmètre est DÉRIVÉ du disque, jamais recopié : un fichier ajouté au module entre
	/// dans la garde sans qu'on touche au test. Le plancher ci-dessous est un garde-fou
	/// anti-effondrement (une énumération qui ne voit plus rien rendrait deux verts à vide),
	/// pas un contrat de compte — cf. la leçon des 34/17 wrappers de mindmaps.
	/// </summary>
	public class DnnArgumentumModuleUtf8GuardTests
	{
		private const string ModuleRelativePath = "DNNPlatform/Portals/1/2sxc/Argumentum";
		private const string StylesheetRelativePath = ModuleRelativePath + "/src/styles/argumentum.css";

		/// <summary>Extensions texte du module. Le binaire (le cas échéant) reste hors garde.</summary>
		private static readonly string[] TextExtensions = { ".cshtml", ".css", ".js", ".json", ".txt", ".xml" };

		/// <summary>Décodeur STRICT : un octet invalide lève, il ne devient pas U+FFFD.</summary>
		private static readonly UTF8Encoding StrictUtf8 = new UTF8Encoding(false, true);

		private static string RepoRoot => TestRepoRoot.Find();

		private static string ModuleRoot => Path.Combine(RepoRoot, ModuleRelativePath.Replace('/', Path.DirectorySeparatorChar));

		private static string StylesheetPath => Path.Combine(RepoRoot, StylesheetRelativePath.Replace('/', Path.DirectorySeparatorChar));

		/// <summary>Chemins relatifs au dépôt, triés — le même ordre sur toute machine.</summary>
		internal static IReadOnlyList<string> TextFiles()
		{
			Directory.Exists(ModuleRoot).Should().BeTrue(
				$"le module 2sxc est versionné dans le dépôt ; {ModuleRelativePath} doit exister sur tout checkout");
			return Directory
				.EnumerateFiles(ModuleRoot, "*", SearchOption.AllDirectories)
				.Where(f => TextExtensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
				.Select(f => Path.GetRelativePath(RepoRoot, f).Replace(Path.DirectorySeparatorChar, '/'))
				.OrderBy(f => f, StringComparer.Ordinal)
				.ToList();
		}

		public static TheoryData<string> ModuleTextFiles()
		{
			var data = new TheoryData<string>();
			foreach (var file in TextFiles())
			{
				data.Add(file);
			}
			return data;
		}

		[Fact]
		public void ModuleTextFileEnumerationIsNotEmpty()
		{
			// Anti-effondrement : une énumération muette rendrait la théorie verte sans rien
			// mesurer. Le plancher n'est pas un compte attendu, seulement un « pas zéro ».
			var files = TextFiles();
			files.Should().NotBeEmpty($"{ModuleRelativePath} doit contenir des fichiers texte");
			files.Should().Contain(f => f.EndsWith("argumentum.css", StringComparison.Ordinal));
			files.Should().Contain(f => f.EndsWith(".cshtml", StringComparison.Ordinal));
		}

		/// <summary>
		/// Forme 1 de #1948 : le fichier doit être de l'UTF-8 **valide**. `E9` seul lève ici.
		/// </summary>
		[Theory]
		[MemberData(nameof(ModuleTextFiles))]
		public void EveryModuleTextFileDecodesAsStrictUtf8(string relativePath)
		{
			var path = Path.Combine(RepoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
			var bytes = File.ReadAllBytes(path);
			Action decode = () => StrictUtf8.GetString(bytes);
			decode.Should().NotThrow(
				$"{relativePath} doit être de l'UTF-8 valide : un octet cp1252 isolé (E9) le rend "
				+ "indécodable et le navigateur affiche U+FFFD (#1948)");
		}

		/// <summary>
		/// Forme 2 de #1948 : le U+FFFD figé est de l'UTF-8 **valide**, donc invisible ci-dessus.
		/// C'est un mojibake déjà écrit sur le site — on le refuse dans le module.
		/// </summary>
		[Theory]
		[MemberData(nameof(ModuleTextFiles))]
		public void NoModuleTextFileCarriesAReplacementCharacter(string relativePath)
		{
			var path = Path.Combine(RepoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
			var text = StrictUtf8.GetString(File.ReadAllBytes(path));
			text.Count(c => c == '�').Should().Be(0,
				$"{relativePath} ne doit porter aucun U+FFFD : c'est le mojibake que #1948 a réparé, "
				+ "et il est indiscernable d'un caractère absent à la lecture");
		}

		/// <summary>
		/// Le CSS servait `text/css` sans charset : le `@charset` doit être la première chose du
		/// fichier (la spécification CSS l'exige ; placé plus bas il est ignoré).
		/// </summary>
		[Fact]
		public void StylesheetDeclaresItsCharsetFirst()
		{
			var bytes = File.ReadAllBytes(StylesheetPath);
			var hasBom = bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;
			hasBom.Should().BeFalse(
				"un BOM décale le `@charset` : il doit commencer au premier octet");

			var head = StrictUtf8.GetString(bytes, 0, Math.Min(bytes.Length, 32));
			head.Should().StartWith("@charset \"utf-8\";",
				$"{StylesheetRelativePath} est servi text/css sans charset, et le @charset n'est "
				+ "honoré que s'il est la première chose du fichier (casse libre, valeur exacte)");
		}

		/// <summary>
		/// Contrôle INVERSE, dans la suite : les deux instruments ci-dessus doivent rougir sur le
		/// défaut qu'ils prétendent attraper. Sans lui, deux verts peuvent dire « rien à voir »
		/// aussi bien que « rien n'est regardé ».
		/// </summary>
		[Fact]
		public void BothInstrumentsRejectTheInjectedDefect()
		{
			var css = File.ReadAllBytes(StylesheetPath);

			// Témoin positif : le contenu réel passe les deux instruments.
			StrictUtf8.GetString(css).Count(c => c == '�').Should().Be(0);
			Action decodeOriginal = () => StrictUtf8.GetString(css);
			decodeOriginal.Should().NotThrow();

			// Forme 1 : un octet E9 (cp1252) greffé dans un commentaire -> le décodeur doit lever.
			var latin1 = css.Concat(new byte[] { (byte)'/', (byte)'*', 0xE9, (byte)'*', (byte)'/' }).ToArray();
			Action decodeLatin1 = () => StrictUtf8.GetString(latin1);
			decodeLatin1.Should().Throw<DecoderFallbackException>(
				"un octet cp1252 isolé doit rougir la garde — c'est exactement l'état d'avant #1948");

			// Forme 2 : le U+FFFD est de l'UTF-8 valide -> seul le compteur le voit.
			var mojibake = Encoding.UTF8.GetBytes(StrictUtf8.GetString(css) + "/* � */");
			Action decodeMojibake = () => StrictUtf8.GetString(mojibake);
			decodeMojibake.Should().NotThrow("le U+FFFD est de l'UTF-8 valide : c'est pourquoi il faut deux instruments");
			StrictUtf8.GetString(mojibake).Count(c => c == '�').Should().Be(1,
				"le compteur de U+FFFD doit voir le mojibake que le décodeur strict laisse passer");
		}
	}
}
