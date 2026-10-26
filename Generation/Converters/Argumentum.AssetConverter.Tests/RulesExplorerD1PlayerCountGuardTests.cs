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
	/// <c>Text_pt</c>, <c>Text_ru</c>, <c>Text_zh</c>, <c>Text_ar</c>, <c>Text_fa</c> du
	/// CSV Règles). Une forme de gabarit
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
			["ar"] = "Text_ar",
			["fa"] = "Text_fa",
		};

		/// <summary>Langues dont le gabarit CONVERTIT le système de chiffres, avec la raison
		/// mesurée. Le canal est à deux sens : une conversion sans corpus non-ASCII serait une
		/// invention ; un corpus non-ASCII sans conversion laisserait la légende en ASCII sous
		/// une carte en chiffres persans. Mesure 22/10 sur le CSV Règles : <c>Text_fa</c> porte
		/// 94 chiffres persans contre 20 ASCII, les sept autres colonnes 0 non-ASCII.</summary>
		private static readonly IReadOnlyDictionary<string, string> ConvertingLangs = new Dictionary<string, string>
		{
			["fa"] = "Text_fa : 94 chiffres persans contre 20 ASCII (mesuré 22/10 sur le CSV Règles)",
		};

		// ── Détecteur pur (calibrable sur des textes fabriqués) ──────────────────────────

		/// <summary>Extrait le commutateur <c>playersFormat</c> d'un gabarit : branches
		/// ordonnées (lang, format), le repli final en dernier avec lang="fr". Jette si le
		/// commutateur a été renommé/retiré ou si une branche perd ses jetons {0}/{1}.
		/// <paramref name="anchor"/> cible le second commutateur (<c>var playersFormatSingle</c>,
		/// forme min==max ajoutée le 22/10) ; <paramref name="requireInterval"/> exige
		/// alors {0} SEUL — un {1} dans la forme simple ferait rendre « de 4 à 4 joueurs »,
		/// le défaut même que le correctif supprime.</summary>
		internal static IReadOnlyList<(string Lang, string Format)> ParseCultureSwitch(string templateText, string anchor = "var playersFormat", bool requireInterval = true)
		{
			var anchorIndex = templateText.IndexOf(anchor, StringComparison.Ordinal);
			if (anchorIndex < 0)
				throw new InvalidOperationException(
					$"GardeD1: '{anchor}' introuvable — le commutateur D1 a été renommé ou retiré du gabarit.");
			var end = templateText.IndexOf("\";", anchorIndex, StringComparison.Ordinal);
			if (end < 0)
				throw new InvalidOperationException(
					"GardeD1: fin du commutateur ('\";') introuvable après '" + anchor + "'.");

			var slice = templateText.Substring(anchorIndex, end + 1 - anchorIndex);
			var branches = new List<(string Lang, string Format)>();
			foreach (Match m in Regex.Matches(slice, @"lang == ""(?'lang'[a-z-]+)"" \? ""(?'fmt'[^""]*)"""))
				branches.Add((m.Groups["lang"].Value, m.Groups["fmt"].Value));
			var fallback = Regex.Match(slice, @":\s*""(?'fmt'[^""]*)""\s*$");
			if (!fallback.Success)
				throw new InvalidOperationException(
					"GardeD1: repli final introuvable — le commutateur doit se terminer par ': \"format\";' (repli fr).");
			branches.Add(("fr", fallback.Groups["fmt"].Value));

			foreach (var (lang, format) in branches)
			{
				if (requireInterval && (!format.Contains("{0}") || !format.Contains("{1}")))
					throw new InvalidOperationException(
						$"GardeD1: la branche '{lang}' porte « {format} » sans {{0}}/{{1}} — l'attestation numérique serait vide.");
				if (!requireInterval && (!format.Contains("{0}") || format.Contains("{1}")))
					throw new InvalidOperationException(
						$"GardeD1: la branche '{lang}' du commutateur min==max porte « {format} » — la forme simple exige {{0}} SANS {{1}}, sinon la légende rend « de 4 à 4 joueurs ».");
			}
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

		/// <summary>Systèmes de chiffres décimaux d'une cellule/colonne : ASCII (U+0030–U+0039)
		/// contre tout autre chiffre décimal Unicode Nd (arabo-indien U+0660–, persan U+06F0–…).
		/// L'ASCII est testé en premier : <c>char.IsDigit</c> est vrai pour lui aussi.</summary>
		internal static (int Ascii, int NonAscii) DigitSystemCounts(string columnText)
		{
			var ascii = 0;
			var nonAscii = 0;
			foreach (var ch in columnText ?? string.Empty)
			{
				if (ch >= '0' && ch <= '9') ascii++;
				else if (char.IsDigit(ch)) nonAscii++;
			}
			return (ascii, nonAscii);
		}

		/// <summary>Branches dont le SYSTÈME DE CHIFFRES servi ne suit pas le corpus.
		/// Le gabarit rend les nombres via <c>string.Format</c>, et .NET n'applique JAMAIS
		/// <c>NumberFormatInfo.NativeDigits</c> : la forme servie est ASCII pour toutes les
		/// langues. Ce canal ne l'affirme pas d'autorité — il le confronte au corpus et
		/// rougit si une langue bascule vers un autre système sans que le gabarit suive.
		/// Vide = vert.</summary>
		internal static List<string> DigitSystemOffenders(
			IReadOnlyList<(string Lang, string Format)> branches,
			IReadOnlyDictionary<string, string> columnTextByLang,
			ICollection<string> convertingLangs)
		{
			var offenders = new List<string>();
			foreach (var (lang, format) in branches)
			{
				if (!columnTextByLang.TryGetValue(lang, out var column) || string.IsNullOrEmpty(column))
				{
					offenders.Add($"{lang} : colonne CSV absente ou vide — système de chiffres non jugeable");
					continue;
				}
				if (!AttestationPattern(format).IsMatch(column))
				{
					offenders.Add($"{lang} : forme « {format} » non attestée — système de chiffres non jugeable");
					continue;
				}
				var (ascii, nonAscii) = DigitSystemCounts(column);
				var converts = convertingLangs.Contains(lang);
				if (nonAscii > ascii && !converts)
					offenders.Add($"{lang} : le corpus porte {nonAscii} chiffres non-ASCII contre {ascii} ASCII mais le gabarit ne convertit pas — la légende servie serait en ASCII sous une carte qui ne l'est pas");
				if (nonAscii <= ascii && converts)
					offenders.Add($"{lang} : le gabarit convertit alors que le corpus est ASCII-dominant ({ascii} ASCII contre {nonAscii} non-ASCII) — conversion sans preuve d'usage");
			}
			return offenders;
		}

		/// <summary>Blocs de contrôle Razor écrits SANS accolades. Payé le 22/10 : un
		/// <c>for</c> suivi d'un <c>if</c> non accoladés dans le corps d'un <c>@foreach</c> ont
		/// fait rendre « Error Showing Content » à la page servie — le parseur Razor ne prend
		/// pas le corps d'un <c>for</c> non accoladé pour du code, et la page entière est
		/// perdue (trouvé par bisection, 2 allers-retours sur la préprod). Un bloc dont le
		/// corps tient sur une seule instruction n'est pas une exception : ici, les accolades.
		/// Vide = vert.</summary>
		internal static List<string> UnbracedControlBlocks(string templateText)
		{
			var offenders = new List<string>();
			var lines = (templateText ?? string.Empty).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i].TrimStart();
				var keyword = Regex.Match(line, @"^(?:@)?(?<kw>for|if|while|foreach)\s*\(");
				if (!keyword.Success)
					continue;
				// Le corps sur la même ligne (« for (...) { ») ou l'accolade à la ligne suivante.
				var sameLine = line.IndexOf('{') >= 0;
				var nextLine = false;
				for (var j = i + 1; j < lines.Length && !nextLine; j++)
				{
					var next = lines[j].Trim();
					if (next.Length == 0) continue;
					nextLine = next.StartsWith("{", StringComparison.Ordinal);
				}
				if (!sameLine && !nextLine)
					offenders.Add($"ligne {i + 1} : « {line.Trim()} » sans accolade — le parseur Razor peut perdre la page entière");
			}
			return offenders;
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

			// Anti-vacuité : le commutateur d'aujourd'hui porte 8 branches (repli fr +
			// en/es/pt/ru/zh/ar/fa, mesuré 22/10 sur 503460c6). Un parseur qui ne verrait
			// plus rien rendrait la garde verte sur 0 branche — le plancher attrape ça.
			detail.Should().HaveCount(c => c >= 8,
				"le commutateur D1 porte aujourd'hui 8 branches (fr + en/es/pt/ru/zh/ar/fa) — moins signifie un parseur cassé ou un commutateur amputé");
			detail.Select(b => b.Lang).Should().ContainInOrder(new[] { "en", "es", "pt", "ru", "zh", "ar", "fa", "fr" },
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

		/// <summary>Canal du système de chiffres (grain ⑲). La forme fr est le contrôle
		/// positif : sans elle, un compte à 0 ne distinguerait pas « corpus ASCII » d'un
		/// instrument aveugle.</summary>
		[Fact]
		public void Le_Systeme_De_Chiffres_Servi_Suit_Celui_Dominant_Du_Corpus()
		{
			var root = TestRepoRoot.Find();
			var (header, rows) = ReadRulesCsv(root);
			var texts = ColumnTextsByLang(header, rows);
			var detail = ParseCultureSwitch(File.ReadAllText(GabaritPath(root, DetailName)));

			// Contrôle positif : l'instrument VOIT des chiffres non-ASCII quand il y en a
			// (mesuré 22/10 : Text_fa en porte 274 pour 3 813 ASCII). Un zéro ici
			// signalerait un compteur cassé, pas un corpus propre.
			DigitSystemCounts(texts["fa"]).NonAscii.Should().BeGreaterThan(0,
				"contrôle positif : le corpus fa porte des chiffres persans (mesuré 274) — 0 signalerait un instrument aveugle");

			// Le mécanisme doit EXISTER dans les deux gabarits, pas seulement être déclaré ici :
			// la table de conversion, et un appel qui passe par la variable convertie (plus
			// aucun formatage inline dans la sortie Razor).
			foreach (var name in new[] { DetailName, ListName })
			{
				var templateText = File.ReadAllText(GabaritPath(root, name));
				foreach (var (convertedLang, reason) in ConvertingLangs)
				{
					templateText.Should().Contain("۰۱۲۳۴۵۶۷۸۹",
						$"{name} doit porter la table de chiffres persans — {convertedLang} : {reason}");
					templateText.Should().NotContain("@string.Format(playersFormat",
						$"{name} doit rendre via la variable convertie, pas formater inline dans la sortie Razor");
					templateText.Should().Contain("@playersText",
						$"{name} doit sortir la variable convertie");
				}
			}

			// Le parseur Razor ne signale rien : une accolade manquante se paie par une page
			// morte (« Error Showing Content »), pas par un test rouge. D'où ce canal statique.
			foreach (var name in new[] { DetailName, ListName })
			{
				var unbraced = UnbracedControlBlocks(File.ReadAllText(GabaritPath(root, name)));
				unbraced.Should().BeEmpty(
					$"{name} : un bloc de contrôle sans accolades fait perdre la page servie entière — " +
					string.Join(" | ", unbraced));
			}

			var offenders = DigitSystemOffenders(detail, texts, ConvertingLangs.Keys.ToList());
			offenders.Should().BeEmpty(
				"le système de chiffres servi (ASCII, ce que rend string.Format) doit rester celui qui domine le corpus de chaque langue — " +
				"une langue qui bascule vers des chiffres non-ASCII sans conversion dans le gabarit est signalée. Contrevenants : " +
				string.Join(" | ", offenders));
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

		/// <summary>Mutation du canal « système de chiffres » : une colonne fabriquée où les
		/// chiffres non-ASCII dominent doit être signalée, et la même colonne en ASCII ne doit
		/// pas l'être — les deux sens, sinon le canal ne prouve que sa propre vacuité.</summary>
		[Fact]
		public void Detecteur_Rouge_Sur_Systeme_De_Chiffres_Non_Servi()
		{
			var branches = new List<(string Lang, string Format)> { ("fa", "از {0} تا {1} بازیکن") };

			var persian = new Dictionary<string, string>
			{
				["fa"] = "از ۳ تا ۱۰ بازیکن، و ۲۰ و ۳۰ و ۴۰ و ۵۰ و ۶۰ و ۷۰",
			};
			// 1) corpus persan, aucune conversion déclarée => rouge nominatif.
			var offenders = DigitSystemOffenders(branches, persian, new List<string>());
			offenders.Should().ContainSingle("une majorité de chiffres persans sans conversion doit être signalée");
			offenders[0].Should().Contain("fa").And.Contain("non-ASCII",
				"le signalement est nominatif : langue ET système de chiffres");

			// 2) même corpus, conversion déclarée => vert.
			DigitSystemOffenders(branches, persian, new List<string> { "fa" }).Should().BeEmpty(
				"un corpus persan AVEC conversion ne doit rien signaler — sinon le canal rougit sur la forme saine");

			// 3) corpus ASCII, conversion déclarée => rouge (conversion sans preuve d'usage).
			var ascii = new Dictionary<string, string>
			{
				["fa"] = "از 3 تا 10 بازیکن، و 20 و 30 و 40 و 50 و 60 و 70",
			};
			var invented = DigitSystemOffenders(branches, ascii, new List<string> { "fa" });
			invented.Should().ContainSingle("une conversion sans corpus non-ASCII doit être signalée");
			invented[0].Should().Contain("ASCII-dominant");
		}

		/// <summary>Calibration du canal « accolades » dans les deux sens : la forme qui a cassé
		/// la préprod le 22/10 doit être signalée, et la même en accolades ne doit pas l'être.</summary>
		[Fact]
		public void Detecteur_Rouge_Sur_Bloc_Sans_Accolades()
		{
			var broken = "@foreach (var x in AsList(Data))\n{\n    if (lang == \"fa\")\n        playersText = Convert(playersText);\n}";
			var offenders = UnbracedControlBlocks(broken);
			offenders.Should().ContainSingle("un if non accoladé doit être signalé");
			offenders[0].Should().Contain("if", "le signalement nomme le bloc");

			var fixedForm = "@foreach (var x in AsList(Data))\n{\n    if (lang == \"fa\")\n    {\n        playersText = Convert(playersText);\n    }\n}";
			UnbracedControlBlocks(fixedForm).Should().BeEmpty(
				"la même forme en accolades ne doit RIEN signaler — sinon le canal rougit sur le correctif");
		}

		// ── Canal min == max (dispatch 22/10, item 3 — « de 4 à 4 joueurs », 8 langues) ──

		/// <summary>Le corpus lui-même écrit la forme simple sur la fiche 4/4 (Rules_13 :
		/// « *Règles du jeu : 4 joueurs* » dans <c>Text</c> et ses 7 traductions, mesuré
		/// 22/10) : le commutateur <c>playersFormatSingle</c> doit exister dans les DEUX
		/// gabarits, identique branche à branche, chaque forme attestée par les cartes de
		/// sa langue — le même contrat que le commutateur d'intervalle.</summary>
		[Fact]
		public void La_Forme_Min_Egal_Max_Est_Synchronisee_Et_Attestee()
		{
			var root = TestRepoRoot.Find();
			var detail = ParseCultureSwitch(File.ReadAllText(GabaritPath(root, DetailName)), "var playersFormatSingle", requireInterval: false);
			var list = ParseCultureSwitch(File.ReadAllText(GabaritPath(root, ListName)), "var playersFormatSingle", requireInterval: false);

			// Anti-vacuité et ordre : mêmes 8 branches que le commutateur d'intervalle.
			detail.Should().HaveCount(c => c >= 8,
				"le commutateur min==max porte aujourd'hui 8 branches (fr + en/es/pt/ru/zh/ar/fa) — moins signifie un parseur cassé ou un commutateur amputé");
			detail.Select(b => b.Lang).Should().ContainInOrder(new[] { "en", "es", "pt", "ru", "zh", "ar", "fa", "fr" },
				"l'ordre mesuré des branches du commutateur min==max");

			SwitchDifferences(detail, list).Should().BeEmpty(
				"D1 min==max (#458) : les deux gabarits doivent porter le MÊME commutateur playersFormatSingle — une divergence fait retomber liste OU détail sur le repli fr, silencieusement. Divergences : " +
				string.Join(" | ", SwitchDifferences(detail, list)));

			var (header, rows) = ReadRulesCsv(root);
			var texts = ColumnTextsByLang(header, rows);
			UnattestedBranches(detail, texts).Should().BeEmpty(
				"la forme simple min==max doit être attestée par les cartes de SA langue — le CSV écrit déjà la forme sur la fiche 4/4 (Rules_13, mesuré 22/10 : « 4 joueurs », « 4 players », « 4 jugadores », « 4 jogadores », « 4 игрока », « 4名玩家 », « 4 لاعبين », « ۴ بازیکن »). Non attestées : " +
				string.Join(" | ", UnattestedBranches(detail, texts)));

			// La conversion des chiffres persans s'applique à la variable partagée playersText :
			// le canal du système de chiffres doit aussi passer sur la forme simple.
			DigitSystemOffenders(detail, texts, ConvertingLangs.Keys.ToList()).Should().BeEmpty(
				"le système de chiffres de la forme simple doit suivre le corpus (fa convertit — « ۴ بازیکن » servi, mesuré 22/10). Contrevenants : " +
				string.Join(" | ", DigitSystemOffenders(detail, texts, ConvertingLangs.Keys.ToList())));
		}

		/// <summary>Mutation de synchronisation du commutateur min==max : la branche ru
		/// retirée du texte RÉEL du RuleList (chirurgie dans la tranche du commutateur
		/// seul, pour ne pas toucher le commutateur d'intervalle), re-parse et
		/// comparaison — le signalement doit être nominatif 'ru'.</summary>
		[Fact]
		public void Detecteur_Rouge_Sur_Commutateur_Min_Egal_Max_Desynchronise()
		{
			var root = TestRepoRoot.Find();
			var detail = ParseCultureSwitch(File.ReadAllText(GabaritPath(root, DetailName)), "var playersFormatSingle", requireInterval: false);
			var listText = File.ReadAllText(GabaritPath(root, ListName));

			var anchor = listText.IndexOf("var playersFormatSingle", StringComparison.Ordinal);
			anchor.Should().BeGreaterOrEqualTo(0, "le commutateur min==max doit exister dans le gabarit vivant");
			var end = listText.IndexOf("\";", anchor, StringComparison.Ordinal);
			var slice = listText.Substring(anchor, end + 1 - anchor);

			var mutatedSlice = Regex.Replace(slice, @"lang == ""ru"" \? ""[^""]*""", string.Empty);
			mutatedSlice.Should().NotBe(slice, "la mutation doit toucher la tranche (la branche ru simple doit exister)");

			var mutated = listText.Replace(slice, mutatedSlice);
			var diffs = SwitchDifferences(detail, ParseCultureSwitch(mutated, "var playersFormatSingle", requireInterval: false));
			diffs.Should().NotBeEmpty("un commutateur min==max désynchronisé doit être signalé");
			diffs.Should().Contain(d => d.Contains("'ru'"), "le signalement est nominatif : la branche manquante est 'ru'");
		}

		/// <summary>Mutation d'attestation de la forme simple : « {0} participants »
		/// (mot absent du corpus Text, mesuré au grain ⑬) doit être signalé ; contrôle
		/// positif d'abord sur la forme fr réelle « {0} joueurs ».</summary>
		[Fact]
		public void Detecteur_Rouge_Sur_Forme_Simple_Non_Attestee()
		{
			var root = TestRepoRoot.Find();
			var (header, rows) = ReadRulesCsv(root);
			var texts = ColumnTextsByLang(header, rows);
			var detail = ParseCultureSwitch(File.ReadAllText(GabaritPath(root, DetailName)), "var playersFormatSingle", requireInterval: false);

			UnattestedBranches(new List<(string Lang, string Format)> { detail.First(b => b.Lang == "fr") }, texts)
				.Should().BeEmpty("contrôle positif : la forme fr simple réelle doit être attestée dans Text (fiche 4/4, mesuré 22/10)");

			var mutated = detail
				.Select(b => b.Lang == "fr" ? ("fr", "{0} participants") : b)
				.ToList();
			var unattested = UnattestedBranches(mutated, texts);
			unattested.Should().ContainSingle("seule la branche mutée doit être signalée");
			unattested[0].Should().Contain("fr").And.Contain("participants",
				"le signalement est nominatif : langue ET forme du mutant");
		}

		/// <summary>Un {1} dans le commutateur simple EST le défaut d'origine (« de 4 à
		/// 4 joueurs ») : le parseur doit le refuser, nominativement.</summary>
		[Fact]
		public void Detecteur_Rouge_Sur_Forme_Simple_Portant_Un_Intervalle()
		{
			var root = TestRepoRoot.Find();
			var listText = File.ReadAllText(GabaritPath(root, ListName));
			var anchor = listText.IndexOf("var playersFormatSingle", StringComparison.Ordinal);
			anchor.Should().BeGreaterOrEqualTo(0, "le commutateur min==max doit exister dans le gabarit vivant");
			var end = listText.IndexOf("\";", anchor, StringComparison.Ordinal);
			var slice = listText.Substring(anchor, end + 1 - anchor);

			var mutated = listText.Replace(slice,
				Regex.Replace(slice, @": ""\{0\} joueurs""\s*$", @": ""de {0} à {1} joueurs"""));
			mutated.Should().NotBe(listText, "la mutation doit toucher la tranche (le repli fr simple doit exister)");

			var act = () => ParseCultureSwitch(mutated, "var playersFormatSingle", requireInterval: false);
			act.Should().Throw<InvalidOperationException>()
				.WithMessage("*{1}*« de 4 à 4 joueurs »*",
					"la forme simple qui porte un intervalle est le défaut d'origine — le refus doit être nominatif");
		}
	}
}
