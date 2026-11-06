using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter.Tests;
using Argumentum.AssetConverter.Tests.Structure;
using ImageMagick;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests.Structure
{
	/// <summary>
	/// #1460 <b>phase C</b>, organe 1 — l'organe qui <b>ouvre les PDF</b>. Il vit dans VisualTests
	/// pour la même raison que l'organe de phase A : c'est ici que vivent les lecteurs
	/// (<c>PdfPig</c>, <c>Magick.NET</c>), et les deux suites n'ont volontairement aucun
	/// <c>ProjectReference</c> entre elles (#1054). Le moteur, lui, reste <b>une seule source sur
	/// disque</b> (<c>Tests/Structure/RulesFirstOrderIntegrity.cs</c>), compilée ici par link.
	///
	/// <para><b>Ce que l'organe mesure.</b> Les 15 premières pages de
	/// <c>Argumentum_TarotCards_{lang}.pdf</c> doivent être <c>rules_01…rules_15</c>, dans cet
	/// ordre, contre <b>toutes</b> les cartes de l'arbre comme candidats — restreindre aux Rules
	/// rendrait l'organe aveugle au défaut qu'il garde (un deck qui commence par une Fallacy).</para>
	///
	/// <para><b>Comment il lit une page.</b> Magick.NET délègue le PDF à Ghostscript : la page est
	/// <b>rasterisée</b> (150 dpi par défaut ici), pas l'image embarquée extraite. Mesuré le 06/11 sur
	/// <b>les 8 langues</b> (120 pages, 1 min 53) : la séparation reste nette (page 1 entre 6,34 et
	/// 6,52 selon la langue, contre <b>82,6–83,2</b> pour la meilleure mauvaise), et deux instruments
	/// indépendants (Magick.NET côté C#, pypdf+PIL côté Python) concordent (6,11 vs 5,70 sur <c>fr</c>).
	/// ⚠️ Sans Ghostscript sur le PATH, la lecture échoue : c'est un <b>échec</b> nommé, jamais un
	/// skip silencieux.</para>
	///
	/// <para><b>Les 8 langues, pas la première trouvée.</b> Un deck peut être juste en <c>fr</c> et
	/// faux dans une seule autre langue ; s'arrêter à la première mesurable aurait rendu cette faute
	/// invisible. Chaque langue porte son propre attendu (son propre <c>rules_XX_face.png</c>), et les
	/// échecs sont accumulés puis rendus <b>langue par langue</b>.</para>
	///
	/// <para><b>Armement.</b> <c>ARGUMENTUM_PDF_BUNDLE_ROOT</c> — la variable déjà utilisée par les
	/// autres organes de ce projet. Absente <b>et</b> arbre absent : la mesure est déclarée
	/// <b>NON MESURABLE</b> et rendue telle quelle. Présente mais inutilisable : <b>échec</b>, car un
	/// gate armé affirme « j'ai un arbre ».</para>
	/// </summary>
	public class RulesFirstOrderGateTests
	{
		private const string BundleRootEnvVar = "ARGUMENTUM_PDF_BUNDLE_ROOT";

		/// <summary>Rayon de comparaison: les 15 premières pages portent les 15 règles (#119, #438).</summary>
		private const int RulesCount = 15;

		private static readonly string TargetRoot = Path.Combine(
			TestRepoRoot.Find(), "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Release",
			"net9.0-windows", "Target");

		private readonly ITestOutputHelper _output;
		public RulesFirstOrderGateTests(ITestOutputHelper output) => _output = output;

		[Fact]
		public void The_First_Pages_Of_The_Tarot_Deck_Are_The_Rules_Cards()
		{
			var envRoot = Environment.GetEnvironmentVariable(BundleRootEnvVar);
			var root = string.IsNullOrWhiteSpace(envRoot) ? TargetRoot : envRoot;

			if (!Directory.Exists(root))
			{
				// Nominal case of this organ: a machine without a bundle, exactly like CI.
				_output.WriteLine($"{BundleRootEnvVar} absent et arbre par défaut absent ({root}) — NON MESURABLE.");
				return;
			}

			var languages = LanguagesWithBothArtefacts(root);
			Assert.True(languages.Count > 0,
				$"sous « {root} », aucune langue ne porte à la fois le PDF Tarot et son dossier d'images Rules — " +
				"un arbre qui existe mais ne contient pas les objets de la mesure est un échec, pas un skip");

			var allFailures = new List<string>();

			foreach (var lang in languages)
			{
				var pdf = Path.Combine(root, lang, "Documents", "density-0", $"Argumentum_TarotCards_{lang}.pdf");
				var imagesRoot = Path.Combine(root, lang, "Images", "density-0");

				var cards = LoadCards(imagesRoot);
				var rulesNames = cards
					.Where(c => c.Name.StartsWith("Rules/", StringComparison.Ordinal))
					.Select(c => c.Name)
					.OrderBy(n => n, StringComparer.Ordinal)
					.Take(RulesCount)
					.ToList();

				_output.WriteLine($"=== {lang} : {cards.Count} candidats, {rulesNames.Count} règles ===");

				if (rulesNames.Count != RulesCount)
				{
					allFailures.Add($"{lang} : {rulesNames.Count} carte(s) Rules dans l'arbre, {RulesCount} attendues");
					continue;
				}

				var pages = LoadPages(pdf, rulesNames.Count);
				if (pages.Count != rulesNames.Count)
				{
					allFailures.Add($"{lang} : {pages.Count} page(s) lisibles dans le PDF, {rulesNames.Count} attendues");
					continue;
				}

				foreach (var (print, index) in pages.Select((p, i) => (p, i)))
				{
					var m = RulesFirstOrderIntegrity.Match(index + 1, print, cards);
					_output.WriteLine($"  page {index + 1,2} → {m.BestName,-40} écart {RulesFirstOrderIntegrity.F(m.BestDistance)} · " +
						$"2nd à {RulesFirstOrderIntegrity.F(m.RunnerUpDistance)} · marge {RulesFirstOrderIntegrity.F(m.Ratio)}x");
				}

				allFailures.AddRange(RulesFirstOrderIntegrity.Check(pages, cards, rulesNames).Select(f => $"{lang} : {f}"));
			}

			Assert.True(allFailures.Count == 0,
				$"le deck Tarot doit commencer par les {RulesCount} cartes Rules, dans l'ordre (#119) — " +
				"mesuré sur les pixels, aucune couche texte n'existe dans ces PDF. Échecs : " +
				string.Join(" | ", allFailures));
		}

		/// <summary>Toutes les langues qui portent <b>les deux</b> objets de la mesure. Toutes, pas la
		/// première : un deck peut être correct en <c>fr</c> et faux dans une seule autre langue, et
		/// s'arrêter à la première trouvée aurait rendu cette faute invisible.</summary>
		private static List<string> LanguagesWithBothArtefacts(string root)
		{
			var languages = new List<string>();
			foreach (var dir in Directory.GetDirectories(root).OrderBy(d => d, StringComparer.Ordinal))
			{
				var lang = Path.GetFileName(dir);
				var pdf = Path.Combine(dir, "Documents", "density-0", $"Argumentum_TarotCards_{lang}.pdf");
				var rules = Path.Combine(dir, "Images", "density-0", "Rules");
				if (File.Exists(pdf) && Directory.Exists(rules)) languages.Add(lang);
			}
			return languages;
		}

		private static List<RulesFirstOrderIntegrity.CardPrint> LoadCards(string imagesRoot)
		{
			var cards = new List<RulesFirstOrderIntegrity.CardPrint>();

			foreach (var family in new[] { "Rules", "Fallacies" })
			{
				var dir = Path.Combine(imagesRoot, family);
				if (!Directory.Exists(dir)) continue;

				foreach (var path in Directory.GetFiles(dir, "*.png").OrderBy(p => p, StringComparer.Ordinal))
				{
					using var image = new MagickImage(path);
					cards.Add(new RulesFirstOrderIntegrity.CardPrint(family + "/" + Path.GetFileName(path), Fingerprint(image)));
				}
			}

			return cards;
		}

		private static List<byte[]> LoadPages(string pdfPath, int count)
		{
			var pages = new List<byte[]>();

			for (var i = 0; i < count; i++)
			{
				var settings = new MagickReadSettings
				{
					Density = new Density(150, 150),
					FrameIndex = (uint)i,
					FrameCount = 1u
				};

				using var collection = new MagickImageCollection(pdfPath, settings);
				if (collection.Count == 0) break;

				pages.Add(Fingerprint(collection[0]));
			}

			return pages;
		}

		/// <summary>Luminance réduite à une grille fixe. <c>IgnoreAspectRatio</c> est explicite : le
		/// redimensionnement par défaut conserve le ratio et rendrait des empreintes de tailles
		/// différentes, donc incomparables (mesuré pendant la mise au point : 64×37 au lieu de 64×64).</summary>
		private static byte[] Fingerprint(IMagickImage<ushort> source)
		{
			using var clone = source.Clone();
			clone.ColorSpace = ColorSpace.Gray;
			clone.Resize(new MagickGeometry(RulesFirstOrderIntegrity.Grid, RulesFirstOrderIntegrity.Grid)
			{
				IgnoreAspectRatio = true
			});
			return clone.GetPixels().ToByteArray(PixelMapping.RGB) ?? Array.Empty<byte>();
		}
	}
}
