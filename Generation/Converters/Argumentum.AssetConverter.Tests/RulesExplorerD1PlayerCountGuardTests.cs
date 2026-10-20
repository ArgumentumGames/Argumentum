using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde D1 (#458, pool c.6285651619 grain ⑬ — po-2023) : le compteur de joueurs des
	/// pages Règles est localisé par un commutateur de culture COPIÉ EN DUR dans les deux
	/// gabarits RulesExplorer (<c>_RulesExplorer_RuleDetail.cshtml</c> et
	/// <c>_RulesExplorer_RuleList.cshtml</c>), avec repli fr. Aucun test ne lisait ces
	/// gabarits (mesure ai-01, cycle 20/10 09:17) : #1906 (D1-ru) puis #1910 (D1-zh) ont
	/// donc été mergées sans garde, chacune devant éditer les DEUX fichiers à l'identique.
	///
	/// La garde prouve deux choses :
	/// <list type="number">
	/// <item><b>Synchronisation</b> — les deux commutateurs sont identiques, branche à
	/// branche, dans le même ordre. Le défaut fondateur : une PR D1 qui édite un gabarit
	/// et pas l'autre — la liste OU le détail d'une langue retombe silencieusement sur le
	/// repli fr (le commutateur ne peut pas « échouer », il se replie).</item>
	/// <item><b>Attestation</b> — la chaîne de chaque branche, nombres substitués, existe
	/// dans les cartes de SA langue (<c>Text</c>, <c>Text_en</c>, <c>Text_es</c>,
	/// <c>Text_pt</c>, <c>Text_ru</c>, <c>Text_zh</c> du CSV Règles). Une forme de gabarit
	/// que rien ne porte dans le corpus serait servie sans preuve d'usage.</item>
	/// </list>
	///
	/// Les deux canaux sont calibrés rouge sur des mutations réelles (texte du gabarit
	/// désynchronisé ; forme syntaxiquement saine mais absente des cartes), et le canal
	/// d'attestation porte un contrôle positif — la forme fr réelle DOIT être trouvée
	/// avant que le rouge du mutant ne prouve quoi que ce soit.
	/// </summary>
	public class RulesExplorerD1PlayerCountGuardTests
	{
		private const string GabaretsDir = "DNNPlatform/Portals/1/2sxc/Argumentum";
		private const string DetailName = "_RulesExplorer_RuleDetail.cshtml";
		private const string ListName = "_RulesExplorer_RuleList.cshtml";
		private const string RulesCsv = "Cards/Rules/Argumentum Rules - Cards.csv";

		/// <summary>Colonne du CSV Règles pour chaque langue du commutateur (le repli fr
		/// lit la colonne sans suffixe — mesuré sur l'en-tête du CSV, 20/10).</summary>
		private static readonly IReadOnlyDictionary<string, string> ColumnByLang = new Dictionary<string, string>
		{
			["fr"] = "Text",
			["en"] = "Text_en",
			["es"] = "Text_es",
			["pt"] = "Text_pt",
			["ru"] = "Text_ru",
			["zh"] = "Text_zh",
		};

		// ── Détecteur pur (calibrable sur des textes fabriqués) ──────────────────────────

		/// <summary>Extrait le commutateur <c>playersFormat</c> d'un gabarit : branches
		/// ordonnées (lang, format), le repli final en dernier avec lang="fr". Jette si le
		/// commutateur a été renommé/retiré ou si une branche perd ses jetons {0}/{1}.</summary>
		internal static IReadOnlyList<(string Lang, string Format)> ParseCultureSwitch(string templateText)
		{
			var anchor = templateText.IndexOf("var playersFormat", StringComparison.Ordinal);
			if (anchor < 0)
				throw new InvalidOperationException(
					"GardeD1: 'var playersFormat' introuvable — le commutateur D1 a été renommé ou retiré du gabarit.");
			var end = templateText.IndexOf("\";", anchor, StringComparison.Ordinal);
			if (end < 0)
				throw new InvalidOperationException(
					"GardeD1: fin du commutateur ('\";') introuvable après 'var playersFormat'.");

			var slice = templateText.Substring(anchor, end + 1 - anchor);
			var branches = new List<(string Lang, string Format)>();
			foreach (Match m in Regex.Matches(slice, @"lang == ""(?'lang'[a-z-]+)"" \? ""(?'fmt'[^""]*)"""))
				branches.Add((m.Groups["lang"].Value, m.Groups["fmt"].Value));
			var fallback = Regex.Match(slice, @":\s*""(?'fmt'[^""]*)""\s*$");
			if (!fallback.Success)
				throw new InvalidOperationException(
					"GardeD1: repli final introuvable — le commutateur doit se terminer par ': \"format\";' (repli fr).");
			branches.Add(("fr", fallback.Groups["fmt"].Value));

			foreach (var (lang, format) in branches)
				if (!format.Contains("{0}") || !format.Contains("{1}"))
					throw new InvalidOperationException(
						$"GardeD1: la branche '{lang}' porte « {format} » sans {{0}}/{{1}} — l'attestation numérique serait vide.");
			return branches;
		}

		/// <summary>Différences nominatives entre deux commutateurs : branche à branche
		/// (même position, même langue, même chaîne), puis branches orphelines.</summary>
		internal static List<string> SwitchDifferences(
			IReadOnlyList<(string Lang, string Format)> left,
			IReadOnlyList<(string Lang, string Format)> right)
		{
			var diffs = new List<string>();
			int i;
			for (i = 0; i < Math.Min(left.Count, right.Count); i++)
				if (left[i].Lang != right[i].Lang || left[i].Format != right[i].Format)
					diffs.Add($"position {i} : {DetailName} '{left[i].Lang}' « {left[i].Format} » vs {ListName} '{right[i].Lang}' « {right[i].Format} »");
			for (var j = i; j < left.Count; j++)
				diffs.Add($"branche '{left[j].Lang}' présente dans {DetailName} mais pas dans {ListName}");
			for (var j = i; j < right.Count; j++)
				diffs.Add($"branche '{right[j].Lang}' présente dans {ListName} mais pas dans {DetailName}");
			return diffs;
		}

		/// <summary>Motif d'attestation d'une forme : {0} et {1} remplacés par des nombres,
		/// le reste littéral. La forme servie est un SEGMENT de cellule markdown
		/// (« *Règles du jeu : de 3 à 10 joueurs* ») — pas d'ancre.
		/// Construit caractère par caractère : <c>Regex.Escape</c> moderne échappe
		/// l'accolade ouvrante mais PAS la fermante (<c>"{0}"</c> → <c>\{0}</c>), donc un
		/// <c>Replace</c> sur une forme échappée supposée ne survit pas à cette asymétrie
		/// (mesuré : le motif restait littéral et l'attestation rendait 0).</summary>
		internal static Regex AttestationPattern(string format)
		{
			var sb = new System.Text.StringBuilder();
			for (var i = 0; i < format.Length; i++)
			{
				var token = format.Length - i >= 3
					&& format[i] == '{'
					&& (format[i + 1] == '0' || format[i + 1] == '1')
					&& format[i + 2] == '}';
				if (token)
				{
					sb.Append(format[i + 1] == '0' ? @"(?'n0'\d+)" : @"(?'n1'\d+)");
					i += 2;
				}
				else
				{
					sb.Append(Regex.Escape(format[i].ToString()));
				}
			}
			return new Regex(sb.ToString());
		}

		/// <summary>Branches dont la forme n'est portée par AUCUNE cellule de sa langue,
		/// nombres substitués. Vide = vert.</summary>
		internal static List<string> UnattestedBranches(
			IReadOnlyList<(string Lang, string Format)> branches,
			IReadOnlyDictionary<string, string> columnTextByLang)
		{
			var unattested = new List<string>();
			foreach (var (lang, format) in branches)
			{
				if (!columnTextByLang.TryGetValue(lang, out var column) || string.IsNullOrEmpty(column))
				{
					unattested.Add($"{lang} : colonne CSV absente ou vide");
					continue;
				}
				if (!AttestationPattern(format).IsMatch(column))
					unattested.Add($"{lang} : « {format} » — 0 attestation dans les cartes après substitution des nombres");
			}
			return unattested;
		}

		// ── Lecture du corpus ────────────────────────────────────────────────────────────

		private static string GabaritPath(string root, string name)
			=> Path.Combine(root, GabaretsDir.Replace('/', Path.DirectorySeparatorChar), name);

		/// <summary>Lit le CSV Règles avec les tolérances du moteur de production.</summary>
		private static (string[] Header, List<string[]> Rows) ReadRulesCsv(string root)
		{
			var path = Path.Combine(root, RulesCsv.Replace('/', Path.DirectorySeparatorChar));
			using var reader = new StringReader(File.ReadAllText(path));
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			using var csv = new CsvReader(reader, config);
			if (!csv.Read() || !csv.ReadHeader() || csv.HeaderRecord is null)
				return (Array.Empty<string>(), new List<string[]>());
			var header = csv.HeaderRecord.ToArray();
			var rows = new List<string[]>();
			while (csv.Read())
				if (csv.Parser.Record is { } record)
					rows.Add(record.ToArray());
			return (header, rows);
		}

		/// <summary>Concatène chaque colonne de langue : l'attestation cherche dans TOUTES
		/// les cartes de la langue, pas dans une seule.</summary>
		private static IReadOnlyDictionary<string, string> ColumnTextsByLang(string[] header, List<string[]> rows)
		{
			var texts = new Dictionary<string, string>();
			foreach (var (lang, column) in ColumnByLang)
			{
				var idx = Array.IndexOf(header, column);
				texts[lang] = idx < 0
					? string.Empty
					: string.Join("\n", rows.Where(r => idx < r.Length).Select(r => r[idx]));
			}
			return texts;
		}

		// ── L'organe ─────────────────────────────────────────────────────────────────────

		[Fact]
		public void Les_Deux_Gabarits_Portent_Le_Meme_Commutateur_Et_Chaque_Forme_Est_Attestee()
		{
			var root = TestRepoRoot.Find();
			var detailPath = GabaritPath(root, DetailName);
			var listPath = GabaritPath(root, ListName);
			foreach (var path in new[] { detailPath, listPath })
				File.Exists(path).Should().BeTrue(
					$"'{path}' est un gabarit RulesExplorer vivant servi par la préprod — déplacé ou renommé ?");

			var detail = ParseCultureSwitch(File.ReadAllText(detailPath));
			var list = ParseCultureSwitch(File.ReadAllText(listPath));

			// Anti-vacuité : le commutateur d'aujourd'hui porte 6 branches (repli fr +
			// en/es/pt/ru/zh, mesuré 20/10 sur dd912f67). Un parseur qui ne verrait plus
			// rien rendrait la garde verte sur 0 branche — le plancher attrape ça.
			detail.Should().HaveCount(c => c >= 6,
				"le commutateur D1 porte aujourd'hui 6 branches (fr + en/es/pt/ru/zh) — moins signifie un parseur cassé ou un commutateur amputé");
			detail.Select(b => b.Lang).Should().ContainInOrder(new[] { "en", "es", "pt", "ru", "zh", "fr" },
				"l'ordre mesuré des branches — une langue déplacée doit passer par ici, pas par le diff");

			SwitchDifferences(detail, list).Should().BeEmpty(
				"D1 (#458) : les deux gabarits RulesExplorer doivent porter le MÊME commutateur de culture, branche à branche — " +
				"une divergence fait retomber la liste OU le détail d'une langue sur le repli fr, silencieusement. Divergences : " +
				string.Join(" | ", SwitchDifferences(detail, list)));

			var (header, rows) = ReadRulesCsv(root);
			rows.Should().HaveCount(c => c >= 15,
				"le CSV Règles porte 15 cartes (mesure 20/10) — un effondrement signale un CSV lu ailleurs ou un parseur cassé");
			var texts = ColumnTextsByLang(header, rows);
			UnattestedBranches(detail, texts).Should().BeEmpty(
				"la chaîne de chaque branche D1 doit être attestée dans les cartes de SA langue après substitution des nombres " +
				"(« celle des cartes », décision ai-01 20/10 01:17) — une forme que le corpus ne porte pas est servie sans preuve d'usage. Non attestées : " +
				string.Join(" | ", UnattestedBranches(detail, texts)));
		}

		// ── Calibrations (le dispatch exige le rouge démontré sur chaque canal) ──────────

		/// <summary>Mutation de synchronisation : la ligne ru est retirée du texte RÉEL du
		/// gabarit RuleList (chirurgie de chaîne), puis re-parse et comparaison — le
		/// défaut fondateur de la garde, une PR D1 qui édite un gabarit et pas l'autre.</summary>
		[Fact]
		public void Detecteur_Rouge_Sur_Gabarit_Desynchronise()
		{
			var root = TestRepoRoot.Find();
			var detail = ParseCultureSwitch(File.ReadAllText(GabaritPath(root, DetailName)));
			var listText = File.ReadAllText(GabaritPath(root, ListName));

			var mutated = Regex.Replace(listText, @"\r?\n\s*: lang == ""ru"" \? ""[^""]*""", string.Empty);
			mutated.Should().NotBe(listText, "la mutation doit toucher le texte (la branche ru doit exister dans le gabarit vivant)");

			var diffs = SwitchDifferences(detail, ParseCultureSwitch(mutated));
			diffs.Should().NotBeEmpty("un gabarit désynchronisé doit être signalé");
			diffs.Should().Contain(d => d.Contains("'ru'"), "le signalement est nominatif : la branche manquante est 'ru'");
		}

		/// <summary>Mutation d'attestation : une forme syntaxiquement saine ({0}/{1} en
		/// place) mais absente des cartes. Contrôle positif d'abord : la forme fr RÉELLE
		/// doit être trouvée — un détecteur qui ne verrait pas le 1 ne prouve rien.</summary>
		[Fact]
		public void Detecteur_Rouge_Sur_Forme_Non_Attestee()
		{
			var root = TestRepoRoot.Find();
			var (header, rows) = ReadRulesCsv(root);
			var texts = ColumnTextsByLang(header, rows);
			var detail = ParseCultureSwitch(File.ReadAllText(GabaritPath(root, DetailName)));

			// Contrôle positif : l'instrument voit le 1 (mesuré : 4 attestations fr).
			UnattestedBranches(new List<(string Lang, string Format)> { detail.First(b => b.Lang == "fr") }, texts)
				.Should().BeEmpty("contrôle positif : la forme fr réelle doit être attestée dans Text (4 occurrences mesurées)");

			// Mutant : « participants » au lieu de « joueurs » — absent du corpus Text
			// (mesuré : 0 attestation, le mot lui-même n'y figure pas).
			var mutated = detail
				.Select(b => b.Lang == "fr" ? ("fr", "de {0} à {1} participants") : b)
				.ToList();
			var unattested = UnattestedBranches(mutated, texts);
			unattested.Should().ContainSingle("seule la branche mutée doit être signalée");
			unattested[0].Should().Contain("fr").And.Contain("participants",
				"le signalement est nominatif : langue ET forme du mutant");
		}
	}
}
