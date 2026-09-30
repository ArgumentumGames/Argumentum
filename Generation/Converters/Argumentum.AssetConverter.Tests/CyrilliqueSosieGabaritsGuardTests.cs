using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde #1669 : aucune lettre cyrillique sosie dans une colonne de langue latine
	/// des gabarits de cartes vivants.
	///
	/// Le défaut fondateur : PK=611, colonne <c>desc_fr</c>, « la fausseté d'une
	/// conсlusion » — le second <c>с</c> est U+0441 (cyrillique), pas U+0063 (latin).
	/// Le glyphe est identique, donc la coquille est invisible à la lecture ET à un
	/// <c>grep</c> du mot : « conclusion » écrit avec U+0441 ne matche pas la recherche
	/// « conclusion ». DINPro couvre U+0441, d'où son invisibilité à l'impression ; une
	/// police candidate qui ne le couvre pas le rendrait depuis une fonte de repli,
	/// produisant un glyphe étranger au milieu du mot (mesure #1485 : Barlow et Archivo
	/// manquent exactement ce caractère).
	///
	/// Le corps de la garde est le MÊME balayage que
	/// <c>docs/investigations/2026-09-30-1485-substitution-polices/scan_cyrillique.py</c> :
	/// les gabarits vivants de <c>Cards/</c>, leurs colonnes latines, lues contre la table
	/// des sosies. Ce n'est PAS un no-op — sur le master d'avant correction ce balayage
	/// rend <b>6</b> occurrences (une par fichier porteur, toutes PK=611 <c>desc_fr</c>,
	/// position 35), <b>0</b> après.
	///
	/// Périmètre DÉLIBÉRÉMENT restreint aux gabarits vivants, et l'exclusion de
	/// <c>Archive/</c> est portante : les gabarits archivés portent encore <b>2</b>
	/// occurrences de la MÊME coquille (v3 <c>Argumentum_Fallacies_Face_Web_Light_fr</c>
	/// et <c>Argumentum_Fallacies_Face_Web_Thumbnails_fr</c>, PK=611 <c>desc_fr</c>).
	/// Ce sont des instantanés figés (2022 / v2 / v3) : les inclure rendrait la garde
	/// rouge sur une correction complète pour le corpus vivant.
	///
	/// Les colonnes russes (<c>title_ru</c>, <c>desc_ru</c>, <c>text_ru</c>…) sont
	/// légitimement en cyrillique : elles sortent du balayage par le filtre de suffixe de
	/// langue, pas par une liste d'exceptions à maintenir.
	/// </summary>
	public class CyrilliqueSosieGabaritsGuardTests
	{
		/// <summary>Suffixes de colonnes dont la langue d'écriture est latine — le cyrillique y est une coquille.</summary>
		private static readonly string[] LanguesLatines = { "_fr", "_en", "_es", "_pt" };

		/// <summary>
		/// Cyrillique visuellement confondable avec une lettre latine. Ce n'est pas « tout le
		/// cyrillique » : seuls les caractères dont le glyphe se confond rendent la coquille
		/// invisible, et c'est cette invisibilité qui fait le défaut. Table identique à celle de
		/// <c>scan_cyrillique.py</c> — les deux mesures doivent rester comparables.
		/// </summary>
		private static readonly IReadOnlyDictionary<char, char> Sosies = new Dictionary<char, char>
		{
			['а'] = 'a', ['е'] = 'e', ['о'] = 'o', ['р'] = 'p', ['с'] = 'c',
			['х'] = 'x', ['у'] = 'y', ['к'] = 'k', ['м'] = 'm', ['т'] = 't',
			['н'] = 'h', ['в'] = 'b', ['і'] = 'i', ['ѕ'] = 's', ['ј'] = 'j',
			['А'] = 'A', ['В'] = 'B', ['Е'] = 'E', ['К'] = 'K', ['М'] = 'M',
			['Н'] = 'H', ['О'] = 'O', ['Р'] = 'P', ['С'] = 'C', ['Т'] = 'T',
			['Х'] = 'X', ['У'] = 'Y',
		};

		private sealed record Finding(string File, string Pk, string Colonne, int Position, char Sosie, char Latin, string Extraits);

		private static string Racine() => TestRepoRoot.Find();

		/// <summary>Les gabarits de cartes vivants : tout <c>Cards/**/*.json</c> hors <c>Archive/</c>.</summary>
		private static IReadOnlyList<string> GabaritsVivants()
		{
			var separateurs = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
			return Directory.EnumerateFiles(Path.Combine(Racine(), "Cards"), "*.json", SearchOption.AllDirectories)
				.Where(p => !p.Split(separateurs).Contains("Archive", StringComparer.OrdinalIgnoreCase))
				.OrderBy(p => p, StringComparer.Ordinal)
				.ToList();
		}

		/// <summary>Lit le CSV embarqué dans la propriété <c>csv</c> du gabarit, avec les tolérances du moteur de production.</summary>
		private static (string[] Entete, List<string[]> Lignes) LireCsv(string contenu)
		{
			using var lecteur = new StringReader(contenu);
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			using var csv = new CsvReader(lecteur, config);
			if (!csv.Read() || !csv.ReadHeader() || csv.HeaderRecord is null)
				return (Array.Empty<string>(), new List<string[]>());
			var entete = csv.HeaderRecord.ToArray();
			var lignes = new List<string[]>();
			while (csv.Read())
			{
				if (csv.Parser.Record is { } record)
					lignes.Add(record.ToArray());
			}
			return (entete, lignes);
		}

		private static string Extraits(string cellule, int position)
		{
			var debut = Math.Max(0, position - 25);
			var fin = Math.Min(cellule.Length, position + 25);
			return cellule.Substring(debut, fin - debut).Replace('\n', ' ').Replace('\r', ' ');
		}

		/// <summary>Balaye tout le corpus vivant et rend les sosies trouvés, plus les compteurs d'anti-vacuité.</summary>
		private static (List<Finding> Findings, int Gabarits, int Colonnes, int Cellules) Balayer()
		{
			var findings = new List<Finding>();
			var gabarits = 0;
			var colonnes = 0;
			var cellules = 0;

			foreach (var chemin in GabaritsVivants())
			{
				using var json = JsonDocument.Parse(File.ReadAllText(chemin));
				if (!json.RootElement.TryGetProperty("csv", out var proprieteCsv))
					continue;
				var contenu = proprieteCsv.GetString();
				if (string.IsNullOrEmpty(contenu))
					continue;

				var (entete, lignes) = LireCsv(contenu);
				if (entete.Length == 0)
					continue;
				var latines = Enumerable.Range(0, entete.Length)
					.Where(i => LanguesLatines.Any(s => entete[i].EndsWith(s, StringComparison.Ordinal)))
					.ToList();
				if (latines.Count == 0)
					continue;

				gabarits++;
				colonnes += latines.Count;
				var relatif = Path.GetRelativePath(Racine(), chemin).Replace('\\', '/');

				foreach (var ligne in lignes)
				{
					foreach (var i in latines)
					{
						if (i >= ligne.Length)
							continue;
						var cellule = ligne[i];
						if (string.IsNullOrEmpty(cellule))
							continue;
						cellules++;
						for (var position = 0; position < cellule.Length; position++)
						{
							if (Sosies.TryGetValue(cellule[position], out var latin))
								findings.Add(new Finding(relatif, ligne[0], entete[i], position,
									cellule[position], latin, Extraits(cellule, position)));
						}
					}
				}
			}

			return (findings, gabarits, colonnes, cellules);
		}

		[Fact]
		public void Aucune_Cellule_Latine_Ne_Porte_Un_Sosie_Cyrillique()
		{
			var (findings, gabarits, colonnes, cellules) = Balayer();

			// Anti-vacuité : un glob cassé rendrait 0 finding sur 0 fichier et la garde
			// passerait au vert sans avoir rien regardé. Les planchers sont larges (mesure du
			// 2026-09-30 : 12 gabarits, 75 colonnes, 8 777 cellules) — ils attrapent la
			// dégénérescence du balayage, pas une retouche de CSV.
			gabarits.Should().BeGreaterThanOrEqualTo(10,
				"le balayage doit atteindre le corpus vivant de Cards/ — un glob cassé rendrait " +
				"0 finding sur 0 fichier et la garde deviendrait un no-op.");
			colonnes.Should().BeGreaterThanOrEqualTo(40,
				"les 12 gabarits vivants portent 75 colonnes latines (mesure 2026-09-30) : " +
				"un effondrement de ce compte signale un filtre de langue cassé.");
			cellules.Should().BeGreaterThanOrEqualTo(5000,
				"8 777 cellules latines non vides sont lues (mesure 2026-09-30) : c'est ce volume " +
				"qui rend le vert signifiant.");

			findings.Should().BeEmpty(
				"#1669 : une lettre cyrillique dans une colonne latine est invisible à la lecture " +
				"comme à un grep du mot, et une police candidate qui ne la couvre pas la rendrait " +
				"depuis une fonte de repli. Trouvé : " +
				string.Join(" | ", findings.Take(10).Select(f =>
					$"{f.File} {f.Colonne} PK={f.Pk} pos={f.Position} U+{(int)f.Sosie:X4} (sosie latin {f.Latin}) ...{f.Extraits}...")));
		}

		[Fact]
		public void Les_Six_Porteurs_De_1669_Portent_La_Forme_Latine()
		{
			// Contrôle positif du balayage : il ne vaut que s'il atteint réellement les six
			// cellules du défaut. Le fragment est unique dans tout le corpus — mesuré :
			// 6 occurrences sur l'ensemble des gabarits (Archive comprise), toutes PK=611
			// desc_fr. C'est aussi la garde anti-régression de la correction elle-même :
			// réintroduire U+0441 dans une de ces six cellules rendrait ce test rouge.
			var porteurs = new[]
			{
				"Cards/Fallacies/Argumentum_Fallacies_Face_fr.json",
				"Cards/Fallacies/Argumentum_Fallacies_Face_2_fr.json",
				"Cards/Fallacies/Argumentum_Fallacies_Face_3_fr.json",
				"Cards/Fallacies/Argumentum_Fallacies_Face_Web_fr.json",
				"Cards/Memo/Argumentum_Memo_Face_fr.json",
				"Cards/Memo/Argumentum_Memo_Back_fr.json",
			};

			porteurs.Should().HaveCount(6, "#1669 porte sur six gabarits vivants.");

			foreach (var relatif in porteurs)
			{
				var chemin = Path.Combine(Racine(), relatif.Replace('/', Path.DirectorySeparatorChar));
				File.Exists(chemin).Should().BeTrue($"{relatif} est un porteur de #1669.");

				using var json = JsonDocument.Parse(File.ReadAllText(chemin));
				var (entete, lignes) = LireCsv(json.RootElement.GetProperty("csv").GetString());
				var colonne = Array.IndexOf(entete, "desc_fr");
				colonne.Should().BeGreaterThanOrEqualTo(0, $"{relatif} porte la colonne desc_fr.");

				var portantes = lignes
					.Where(l => colonne < l.Length && l[colonne].Contains("fausseté d'une conclusion", StringComparison.Ordinal))
					.ToList();
				portantes.Should().HaveCount(1,
					$"#1669 : une seule cellule desc_fr de {relatif} porte la phrase corrigée.");
				portantes[0][0].Should().Be("611",
					$"mesure #1669 : les six occurrences sont PK=611 desc_fr — une septième apparaîtrait ici.");
			}
		}
	}
}