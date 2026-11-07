using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Argumentum.AssetConverter.Tests;
using Argumentum.AssetConverter.Tests.Structure;
using ImageMagick;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.VisualTests.Structure
{
	/// <summary>
	/// #1460 <b>phase C</b>, organe 2 — l'organe qui <b>ouvre les PNG de récolte</b> et confronte
	/// la couleur de famille <b>réalisée</b> à celle que le gabarit déclare. Il vit dans
	/// VisualTests comme l'organe 1 (<c>Magick.NET</c> y vit ; aucune référence entre les deux
	/// suites, #1054) et le moteur reste <b>une seule source sur disque</b>
	/// (<c>Tests/Structure/FamilyColorIntegrity.cs</c>), compilé ici par link.
	///
	/// <para><b>Ce que l'organe mesure.</b> Pour chaque langue qui porte un dossier
	/// <c>Images/density-0/Fallacies</c>, les <b>175</b> cartes du paquet : la couleur dominante
	/// non blanche du PNG doit être la couleur <c>--color-background</c> du bloc
	/// <c>card.&lt;Famille_camelCase&gt;</c> du gabarit, à <b>30</b> près (mesuré : 0,0 partout),
	/// et peser au moins <b>5 %</b> de l'échantillon (mesuré : ≥ 14,8 %). Aucun PDF n'est ouvert :
	/// cet organe ne dépend pas de Ghostscript.</para>
	///
	/// <para><b>La jointure PNG ↔ CSV</b> se fait par le préfixe
	/// <c>argumentum_fallacies_{path}..</c> — les virgules du <c>path</c> CSV deviennent des
	/// points dans le nom de fichier (mesuré : 175/175 appariements uniques). Le préfixe est
	/// non ambigu car le point y est le séparateur de niveaux : <c>_1..</c> ne peut pas matcher
	/// <c>_1.1..</c>. Un préfixe qui matche ≠ 1 fichier est un échec <b>nommé</b> de la langue
	/// entière, pas une carte silencieusement sautée.</para>
	///
	/// <para><b>Les 8 langues, pas la première trouvée</b> : une classe peut être correcte en
	/// <c>fr</c> et manquer dans une seule autre langue ; chaque langue porte le même attendu
	/// (la famille vient du CSV français, seule colonne de classe — la localisation ne réécrit
	/// pas <c>cardClass</c>).</para>
	///
	/// <para><b>Armement.</b> <c>ARGUMENTUM_PDF_BUNDLE_ROOT</c> — la variable des autres organes.
	/// Absente <b>et</b> arbre par défaut absent : <b>NON MESURABLE</b>, rendu tel quel.
	/// Présente mais inutilisable : <b>échec</b>, car un gate armé affirme « j'ai un arbre ».</para>
	/// </summary>
	public class FamilyColorGateTests
	{
		private const string BundleRootEnvVar = "ARGUMENTUM_PDF_BUNDLE_ROOT";

		private static readonly string TargetRoot = Path.Combine(
			TestRepoRoot.Find(), "Generation", "Converters", "Argumentum.AssetConverter", "bin", "Release",
			"net9.0-windows", "Target");

		private readonly ITestOutputHelper _output;
		public FamilyColorGateTests(ITestOutputHelper output) => _output = output;

		[Fact]
		public void Every_Fallacy_Card_Carries_Its_Family_Color()
		{
			var envRoot = Environment.GetEnvironmentVariable(BundleRootEnvVar);
			var root = string.IsNullOrWhiteSpace(envRoot) ? TargetRoot : envRoot;

			if (!Directory.Exists(root))
			{
				// Nominal case of this organ: a machine without a bundle, exactly like CI.
				_output.WriteLine($"{BundleRootEnvVar} absent et arbre par défaut absent ({root}) — NON MESURABLE.");
				return;
			}

			// ── L'attendu : CSV (famille par carte) × gabarit (couleur par famille) ──
			var repoRoot = TestRepoRoot.Find();
			var csvPath = Path.Combine(repoRoot, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");
			var templatePath = Path.Combine(repoRoot, "Cards", "Fallacies", "Argumentum_Fallacies_Face_fr.json");

			var reader = new HarvestCardIdsCsv(csvPath);
			var paths = reader.LoadColumn("path");
			var cartes = reader.LoadColumn("carte");
			var classes = reader.LoadColumn("Famille_camelCase");
			Assert.True(paths.Count == cartes.Count && paths.Count == classes.Count,
				$"les trois colonnes lues n'ont pas le même nombre de lignes ({paths.Count}/{cartes.Count}/{classes.Count}) — " +
				$"une relecture non alignée ferait correspondre une carte à la famille de sa voisine");

			var cards = new List<FamilyColorIntegrity.CardFamily>();
			for (var i = 0; i < paths.Count; i++)
			{
				if (string.IsNullOrWhiteSpace(cartes[i])) continue; // nœud sans carte (1 408 nœuds, 175 cartes)
				cards.Add(new FamilyColorIntegrity.CardFamily(Prefix(paths[i]), classes[i]));
			}
			Assert.True(cards.Count > 0,
				$"aucune ligne avec colonne carte non vide dans {csvPath} ({paths.Count} nœuds lus) — " +
				$"l'attendu est vide, la mesure n'aurait rien à confronter");
			_output.WriteLine($"{cards.Count} cartes attendues (colonne carte non vide sur {paths.Count} nœuds)");

			string? css = null;
			Assert.True(File.Exists(templatePath), $"gabarit introuvable : {templatePath}");
			using (var doc = JsonDocument.Parse(File.ReadAllText(templatePath)))
			{
				css = doc.RootElement.TryGetProperty("css", out var cssElement) ? cssElement.GetString() : null;
			}

			var familyColors = FamilyColorIntegrity.ParseFamilyColors(css);
			Assert.True(familyColors.Count > 0,
				$"aucune couleur de famille parsée dans la clé css de {templatePath} — " +
				"la clé est-elle absente, vide, ou sans bloc card.<famille> portant --color-background ?");

			var separations = familyColors.Keys
				.SelectMany(a => familyColors.Keys.Where(b => string.CompareOrdinal(a, b) < 0)
					.Select(b => (a, b, d: FamilyColorIntegrity.Distance(familyColors[a], familyColors[b]))))
				.OrderBy(t => t.d)
				.Take(3)
				.ToList();
			foreach (var (a, b, d) in separations)
			{
				_output.WriteLine($"familles les plus proches : {a} ↔ {b} à {FamilyColorIntegrity.F(d)}");
			}

			// ── La mesure : langue par langue, toutes celles qui portent le dossier d'images ──
			var languages = Directory.GetDirectories(root)
				.Select(Path.GetFileName)
				.Where(lang => Directory.Exists(Path.Combine(root, lang!, "Images", "density-0", "Fallacies")))
				.OrderBy(l => l, StringComparer.Ordinal)
				.Select(l => l!)
				.ToList();
			Assert.True(languages.Count > 0,
				$"sous « {root} », aucune langue ne porte Images/density-0/Fallacies — " +
				"un arbre qui existe mais ne contient pas les objets de la mesure est un échec, pas un skip");

			var allFailures = new List<string>();

			foreach (var lang in languages)
			{
				var imageDir = Path.Combine(root, lang, "Images", "density-0", "Fallacies");
				var files = Directory.GetFiles(imageDir, "*_face.png")
					.Select(Path.GetFileName)
					.Select(f => f!)
					.OrderBy(f => f, StringComparer.Ordinal)
					.ToList();

				var measured = new List<FamilyColorIntegrity.MeasuredDominant>();
				var joinFailures = new List<string>();
				FamilyColorIntegrity.MeasuredDominant? worstShare = null;
				var worstDistance = 0.0;

				foreach (var card in cards)
				{
					var hits = files.Where(f => f.StartsWith(card.Name, StringComparison.Ordinal)).ToList();
					if (hits.Count != 1)
					{
						joinFailures.Add($"{card.Name} → {hits.Count} fichier(s)");
						continue;
					}

					var dominant = Dominant(card.Name, Path.Combine(imageDir, hits[0]));
					if (dominant == null)
					{
						joinFailures.Add($"{card.Name} → aucune couleur non blanche mesurable");
						continue;
					}

					measured.Add(dominant);
					if (worstShare == null || dominant.Share < worstShare.Share) worstShare = dominant;
					var d = FamilyColorIntegrity.Distance(dominant.Color, familyColors[card.FamilyClass]);
					if (d > worstDistance) worstDistance = d;
				}

				if (joinFailures.Count > 0)
				{
					allFailures.Add($"{lang} : jointure défaillante — " + string.Join(" | ", joinFailures.Take(5)));
					continue;
				}

				_output.WriteLine(
					$"=== {lang} : {measured.Count} cartes mesurées · pire distance {FamilyColorIntegrity.F(worstDistance)} · " +
					$"part dominante min {(worstShare == null ? "n/a" : FamilyColorIntegrity.P(worstShare.Share))}");

				allFailures.AddRange(
					FamilyColorIntegrity.Check(cards, familyColors, measured)
						.Select(f => $"{lang} : {f}"));
			}

			Assert.True(allFailures.Count == 0,
				"chaque carte du paquet Fallacies doit porter la couleur de sa famille (colonne Famille_camelCase × " +
				"--color-background du gabarit) — mesuré sur les pixels des PNG de récolte. Échecs : " +
				string.Join(" | ", allFailures));
		}

		/// <summary>Le préfixe de jointure : <c>argumentum_fallacies_{path}..</c>, virgules du CSV
		/// devenues points. Mesuré 175/175 appariements uniques sur les 8 langues.</summary>
		private static string Prefix(string csvPath) => $"argumentum_fallacies_{csvPath.Replace(',', '.')}..";

		/// <summary>Couleur dominante non blanche, échantillonnage 1 point sur 16 (1 pixel sur 4
		/// en x et en y). « Non blanche » : au moins un canal &lt; 240 — le panneau de contenu des
		/// cartes est blanc et ses bords antialiasés tourneraient autour de 250+ sur les trois
		/// canaux.</summary>
		private static FamilyColorIntegrity.MeasuredDominant? Dominant(string name, string pngPath)
		{
			using var image = new MagickImage(pngPath);
			var rgb = image.GetPixels().ToByteArray(PixelMapping.RGB);
			if (rgb == null || rgb.Length == 0) return null;

			var counts = new Dictionary<int, long>();
			long samples = 0;
			for (var y = 0; y < image.Height; y += 4)
			{
				var rowOffset = y * image.Width * 3;
				for (var x = 0; x < image.Width; x += 4)
				{
					var i = rowOffset + x * 3;
					if (i + 2 >= rgb.Length) break;
					samples++;
					var key = (rgb[i] << 16) | (rgb[i + 1] << 8) | rgb[i + 2];
					counts.TryGetValue(key, out var n);
					counts[key] = n + 1;
				}
			}
			if (samples == 0) return null;

			var bestKey = -1;
			long bestCount = 0;
			foreach (var pair in counts)
			{
				var r = (pair.Key >> 16) & 0xFF;
				var g = (pair.Key >> 8) & 0xFF;
				var b = pair.Key & 0xFF;
				if (Math.Min(r, Math.Min(g, b)) >= 240) continue; // blanc / quasi blanc
				if (pair.Value > bestCount)
				{
					bestCount = pair.Value;
					bestKey = pair.Key;
				}
			}
			if (bestKey < 0) return null;

			var color = new FamilyColorIntegrity.Rgb(
				(byte)((bestKey >> 16) & 0xFF), (byte)((bestKey >> 8) & 0xFF), (byte)(bestKey & 0xFF));
			return new FamilyColorIntegrity.MeasuredDominant(name, color, (double)bestCount / samples);
		}
	}
}
