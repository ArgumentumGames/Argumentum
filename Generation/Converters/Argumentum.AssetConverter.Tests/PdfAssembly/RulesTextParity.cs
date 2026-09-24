using System.Text.RegularExpressions;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
	/// <summary>
	/// Organe de parité #1502 (⑪) : le texte des 6 cartes Print &amp; Play doit être **octet à octet**
	/// celui des cartes Rules_01..06 du deck principal, sur les 8 colonnes de texte. Décision owner
	/// du 23/09 (#1502 c.5801212274, « Les règles devraient être les même en print&play ») : le P&P
	/// était une édition ANTÉRIEURE des règles (jauge 4-8, 7 points de règle divergents) — l'origine
	/// du défaut était qu'une modification du deck pouvait oublier le P&P sans qu'aucun organe ne
	/// vire au rouge. Cette garde ferme exactement ce silence : toute divergence deck↔P&P échoue en CI
	/// AVANT régénération, en nommant la carte et la langue.
	/// </summary>
	/// <remarks>
	/// La lecture passe par <see cref="HarvestCardIdsCsv"/> (encodage robuste, appariement d'en-têtes
	/// compatible production) — pas un parseur ad hoc. Le cœur <see cref="CompareTables"/> est pur
	/// (aucune I/O) : le témoin rouge de <c>PdfDeckCountContractTests</c> l'exerce sur des tables en
	/// mémoire avec une divergence injectée, pour que la garde ait été VUE rouge (#1046) et pas
	/// seulement crue verte.
	/// </remarks>
	internal static class RulesTextParity
	{
		public const string DeckCsvName = "Argumentum Rules - Cards.csv";
		public const string PrintAndPlayCsvName = "Argumentum Rules - Cards Print and Play.csv";

		/// <summary>Les 8 colonnes de texte couvertes par la décision owner (l'édition P&P n'ajoute aucune langue).</summary>
		public static readonly string[] TextColumns =
			{ "Text", "Text_en", "Text_ru", "Text_pt", "Text_ar", "Text_es", "Text_zh", "Text_fa" };

		private static readonly Regex PrintAndPlayPkPattern = new(@"^RulesPP_0[1-6]$", RegexOptions.Compiled);

		/// <summary>
		/// Lit les deux CSV sous <paramref name="repoRoot"/>/Cards/Rules/ et retourne les violations de
		/// parité (vide = parité tenue). Toute colonne manquante ou pk dupliqué lève
		/// <see cref="InvalidOperationException"/> (fail-loud, jamais de parité crue sur une lecture amputée).
		/// </summary>
		public static IReadOnlyList<string> FindViolations(string repoRoot)
		{
			var deck = ReadTable(Path.Combine(repoRoot, "Cards", "Rules", DeckCsvName));
			var printAndPlay = ReadTable(Path.Combine(repoRoot, "Cards", "Rules", PrintAndPlayCsvName));
			return CompareTables(deck, printAndPlay);
		}

		/// <summary>
		/// Cœur pur : compare Rules_0N ↔ RulesPP_0N (N = 1..6) sur les 8 colonnes. Chaque violation
		/// nomme la carte, la colonne ET la langue, les deux longueurs, et la position de la première
		/// divergence — assez pour agir sans re-dérouler l'instrument.
		/// </summary>
		public static IReadOnlyList<string> CompareTables(
			IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> deck,
			IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> printAndPlay)
		{
			var failures = new List<string>();
			for (var n = 1; n <= 6; n++)
			{
				var printAndPlayPk = $"RulesPP_0{n}";
				var deckPk = $"Rules_0{n}";
				if (!printAndPlay.TryGetValue(printAndPlayPk, out var ppRow))
				{
					failures.Add($"{printAndPlayPk} : ligne absente du CSV Print & Play — la parité 6 cartes est rompue côté P&P");
					continue;
				}
				if (!deck.TryGetValue(deckPk, out var deckRow))
				{
					failures.Add($"{printAndPlayPk} : pas de contrepartie {deckPk} dans le CSV du deck");
					continue;
				}
				foreach (var column in TextColumns)
				{
					var deckText = deckRow.TryGetValue(column, out var d) ? d : string.Empty;
					var ppText = ppRow.TryGetValue(column, out var p) ? p : string.Empty;
					if (!string.Equals(deckText, ppText, StringComparison.Ordinal))
					{
						failures.Add(
							$"{printAndPlayPk} colonne {column} (langue {LanguageOf(column)}) : {ppText.Length} car. au lieu de {deckText.Length} (deck {deckPk}), "
							+ $"première divergence au caractère {FirstDiffIndex(deckText, ppText)}");
					}
				}
			}

			// Une ligne P&P hors RulesPP_01..06 (renommée, dupliquée, ajoutée) doit aussi faire rouge :
			// la décision owner porte sur CES six cartes, pas sur « toutes les lignes du fichier ».
			foreach (var unexpected in printAndPlay.Keys.Where(k => !PrintAndPlayPkPattern.IsMatch(k)).OrderBy(k => k, StringComparer.Ordinal))
			{
				failures.Add($"{unexpected} : ligne inattendue dans le CSV Print & Play (motif attendu RulesPP_01..06)");
			}

			return failures;
		}

		/// <summary>
		/// Lit un CSV Rules en table pk → (colonne → valeur), via le lecteur robuste de l'organe
		/// #1187 (encodage BOM/UTF-16/Latin-1, en-têtes compatibles production).
		/// </summary>
		private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> ReadTable(string path)
		{
			var reader = new HarvestCardIdsCsv(path);
			var pks = reader.LoadColumn("pk");
			var columns = TextColumns.ToDictionary(c => c, c => reader.LoadColumn(c), StringComparer.Ordinal);
			var table = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.Ordinal);
			for (var i = 0; i < pks.Count; i++)
			{
				var row = TextColumns.ToDictionary(c => c, c => columns[c][i], StringComparer.Ordinal);
				if (!table.TryAdd(pks[i], row))
					throw new InvalidOperationException($"RulesTextParity: pk dupliqué '{pks[i]}' dans '{path}'.");
			}
			return table;
		}

		private static string LanguageOf(string column) => column == "Text" ? "fr" : column["Text_".Length..];

		private static int FirstDiffIndex(string a, string b)
		{
			var n = Math.Min(a.Length, b.Length);
			for (var i = 0; i < n; i++)
			{
				if (a[i] != b[i]) return i;
			}
			return n; // l'un est un préfixe de l'autre
		}
	}
}
