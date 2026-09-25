using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
	/// <summary>
	/// Organe #1539 : la liste de matériel des cartes de règles ne doit annoncer **aucun nombre**
	/// de cartes mémo. Décision owner du 24/09 (« Q-14 ok pour a », interactive, VÉRIFIÉ) : le
	/// chiffre part, la ligne reste.
	/// </summary>
	/// <remarks>
	/// <para><b>Pourquoi aucune valeur n'est juste.</b> Les trois formats n'impriment pas le même
	/// nombre de cartes mémo : la boîte en imprime <b>7</b> (décision #1187), le P&amp;P A4 en imprime
	/// <b>5</b>, le P&amp;P Light en imprime <b>1</b>. Un chiffre en dur dans la liste de matériel est
	/// donc faux dans au moins deux des trois — et il l'était dans les cinq cartes, en huit langues.</para>
	/// <para><b>Deux invariants, pas un</b> : le défaut à épingler est « la ligne de matériel commence
	/// par un chiffre », mais une garde qui ne dit que cela est satisfaite en <i>supprimant</i> la ligne
	/// — un vert obtenu par disparition. Le second invariant (plancher de présence, <see cref="MinMemoCells"/>)
	/// ferme cette porte : les 40 cellules doivent toujours porter leur ligne de matériel, sans chiffre.</para>
	/// <para><b>Le mot « mémo » n'est pas le même dans les 8 langues</b> — <c>ajuda</c> (pt) n'est pas
	/// <c>ayuda</c> (es), l'arabe écrit <c>تذكير</c>, le persan <c>یادآور</c>, le chinois <c>提示卡</c>.
	/// Une sonde qui ne cherche que « mémo » rend <b>39</b> sur un corpus qui en porte <b>40</b> : c'est
	/// l'écart mesuré lors de la reconnaissance de ce grain, dû à la seule paire <c>ajuda</c>/<c>ayuda</c>.
	/// La liste ci-dessous est donc explicite et commentée, pas dérivée.</para>
	/// <para><b>Les chiffres non latins ne sont pas tous « des chiffres » pour le CLR</b> : les
	/// arabo-indiens (U+0660..) et étendus (U+06F0..) sont <see cref="char.IsDigit(char)"/>, les
	/// idéogrammes chinois 一二三四五六七八九十 <b>non</b> (catégorie OtherLetter). Un contrôle qui
	/// s'appuierait sur <c>char.IsDigit</c> laisserait passer <c>* 五张备忘卡</c> — l'issue nomme
	/// explicitement les trois systèmes, la détection est donc énumérée en clair.</para>
	/// </remarks>
	internal static class RulesMaterialMemoCountGate
	{
		public const string DeckCsvName = "Argumentum Rules - Cards.csv";
		public const string PrintAndPlayCsvName = "Argumentum Rules - Cards Print and Play.csv";

		/// <summary>Les 8 colonnes de texte des cartes de règles.</summary>
		public static readonly string[] TextColumns =
			{ "Text", "Text_en", "Text_ru", "Text_pt", "Text_ar", "Text_es", "Text_zh", "Text_fa" };

		/// <summary>
		/// Plancher de présence : nombre de cellules qui doivent porter une ligne de matériel « mémo ».
		/// Mesuré à <b>40</b> le 25/09/2026 (5 cartes × 8 langues : Rules_02/09/11/13 du deck + RulesPP_02).
		/// Le plancher est délibérément ÉGAL à la mesure, pas en dessous : il ne protège pas contre une
		/// baisse « acceptable », il rend rouge toute disparition de la ligne — y compris celle qui
		/// éteindrait l'invariant « pas de chiffre » par suppression.
		/// </summary>
		public const int MinMemoCells = 40;

		/// <summary>
		/// Mots « mémo » par langue, mesurés sur le corpus (jamais supposés). <c>ayuda</c> (es) et
		/// <c>ajuda</c> (pt) sont deux entrées distinctes : les confondre rend 39 au lieu de 40.
		/// </summary>
		private static readonly string[] MemoWords =
		{
			"mémo", "memo",           // fr (cartes mémo) · en (memo cards)
			"Мемо",                   // ru (карты Мемо)
			"ajuda",                  // pt (cartas de ajuda / ajuda-memória)
			"ayuda", "recordatorio",  // es (cartas de ayuda / cartas recordatorio / cartas de memo)
			"تذكير",                   // ar (بطاقات تذكير)
			"提示卡", "备忘卡",           // zh (提示卡 / 备忘卡)
			"یادآور",                  // fa (کارت‌های یادآور)
		};

		/// <summary>
		/// Les systèmes de chiffres que l'issue nomme, énumérés en clair (voir la remarque de classe :
		/// <see cref="char.IsDigit(char)"/> ne couvre PAS les idéogrammes chinois).
		/// </summary>
		private static readonly (string Name, char First, char Last)[] DigitRanges =
		{
			("arabe occidental", '0', '9'),
			("arabo-indien", '٠', '٩'),
			("arabo-indien étendu (persan)", '۰', '۹'),
		};

		private const string CjkDigits = "〇一二三四五六七八九十百千";

		private static readonly Regex MemoWordPattern = new Regex(
			string.Join("|", MemoWords.Select(Regex.Escape)),
			RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		/// <summary>
		/// Lit les deux CSV sous <paramref name="repoRoot"/>/Cards/Rules/ et retourne les violations
		/// (vide = invariant tenu). Une colonne manquante ou un pk dupliqué lève
		/// <see cref="InvalidOperationException"/> via <see cref="HarvestCardIdsCsv"/> (fail-loud).
		/// </summary>
		public static IReadOnlyList<string> FindViolations(string repoRoot)
		{
			var deck = ReadTable(System.IO.Path.Combine(repoRoot, "Cards", "Rules", DeckCsvName));
			var printAndPlay = ReadTable(System.IO.Path.Combine(repoRoot, "Cards", "Rules", PrintAndPlayCsvName));
			return CompareTables(deck, printAndPlay);
		}

		/// <summary>
		/// Cœur pur (aucune I/O) : exerce les deux invariants sur les deux tables réunies, pour que la
		/// garde ait été VUE rouge (#1046) sur des valeurs injectées et pas seulement crue verte.
		/// </summary>
		public static IReadOnlyList<string> CompareTables(
			Dictionary<string, Dictionary<string, string>> deck,
			Dictionary<string, Dictionary<string, string>> printAndPlay)
		{
			var failures = new List<string>();
			var cellsWithMemo = 0;

			foreach (var (tableName, table) in new[] { ("deck", deck), ("P&P", printAndPlay) })
			{
				foreach (var pk in table.Keys.OrderBy(k => k, StringComparer.Ordinal))
				{
					foreach (var column in TextColumns)
					{
						var cell = table[pk].TryGetValue(column, out var v) ? v : string.Empty;
						var bullets = MemoBullets(cell).ToList();
						var where = $"{pk} · {column} (langue {LanguageOf(column)}) [{tableName}]";

						if (bullets.Count > 1)
						{
							failures.Add(
								$"[doublon] {where} : {bullets.Count} lignes de matériel « mémo » dans la même cellule "
								+ "— la liste annoncerait deux fois le même matériel. Offenders: "
								+ string.Join(" | ", bullets));
							cellsWithMemo += bullets.Count;
							continue;
						}
						if (bullets.Count == 0)
						{
							continue;
						}

						cellsWithMemo++;
						var bullet = bullets[0];
						if (LeadingDigit(bullet) is { } digit)
						{
							failures.Add(
								$"[chiffre] {where} : la ligne « {bullet} » ouvre par un chiffre ({digit}) — "
								+ "aucun nombre de cartes mémo n'est juste (7 en boîte, 5 en P&P A4, 1 en P&P Light) "
								+ "et ce nombre est donc faux dans au moins deux des trois formats (#1539).");
						}
					}
				}
			}

			if (cellsWithMemo < MinMemoCells)
			{
				failures.Add(
					$"[plancher] {cellsWithMemo} cellule(s) portent une ligne de matériel « mémo », "
					+ $"attendu au moins {MinMemoCells} (5 cartes × 8 langues). "
					+ "Un compte INFÉRIEUR rend rouge au même titre qu'un chiffre : l'invariant « pas de chiffre » "
					+ "serait tenu par disparition de la ligne, pas par correction.");
			}

			return failures;
		}

		/// <summary>Ligne de liste (ouvre par « * ») qui mentionne le mot « mémo » de sa langue.</summary>
		internal static IEnumerable<string> MemoBullets(string cell)
		{
			if (string.IsNullOrEmpty(cell))
			{
				yield break;
			}
			foreach (var line in cell.Split('\n'))
			{
				var trimmed = line.Trim();
				if (trimmed.StartsWith("*", StringComparison.Ordinal) && MemoWordPattern.IsMatch(trimmed))
				{
					yield return line;
				}
			}
		}

		/// <summary>
		/// Le chiffre qui ouvre l'item de liste, avec le nom de son système — ou <c>null</c>. Le « * »
		/// (et l'espace qui le suit) fait partie du marqueur de liste, pas de la valeur.
		/// </summary>
		internal static string? LeadingDigit(string bulletLine)
		{
			var rest = bulletLine.TrimStart();
			if (!rest.StartsWith("*", StringComparison.Ordinal))
			{
				return null;
			}
			rest = rest[1..].TrimStart();
			if (rest.Length == 0)
			{
				return null;
			}
			var c = rest[0];
			foreach (var (name, first, last) in DigitRanges)
			{
				if (c >= first && c <= last)
				{
					return $"« {c} » ({name}, U+{(int)c:X4})";
				}
			}
			if (CjkDigits.IndexOf(c) >= 0)
			{
				return $"« {c} » (chiffre chinois)";
			}
			return null;
		}

		private static Dictionary<string, Dictionary<string, string>> ReadTable(string path)
		{
			var reader = new HarvestCardIdsCsv(path);
			var pks = reader.LoadColumn("pk");
			var columns = TextColumns.ToDictionary(c => c, c => reader.LoadColumn(c), StringComparer.Ordinal);
			var table = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
			for (var i = 0; i < pks.Count; i++)
			{
				var row = TextColumns.ToDictionary(c => c, c => columns[c][i], StringComparer.Ordinal);
				if (!table.TryAdd(pks[i], row))
					throw new InvalidOperationException($"RulesMaterialMemoCountGate: pk dupliqué '{pks[i]}' dans '{path}'.");
			}
			return table;
		}

		private static string LanguageOf(string column) => column == "Text" ? "fr" : column["Text_".Length..];
	}
}