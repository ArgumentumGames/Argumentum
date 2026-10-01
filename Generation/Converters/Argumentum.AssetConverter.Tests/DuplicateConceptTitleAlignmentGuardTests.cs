using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du grain « concepts dupliqués » (dispatch #458 c.5915478409, clôture du point 2 de
	/// #1622 côté titres) : quand un concept figure DEUX FOIS dans la taxonomie — même
	/// <c>text_fr</c> ET même <c>text_en</c>, l'identité éditoriale du corpus — et qu'UNE des
	/// copies est une carte imprimée, la copie hors deck porte LE MÊME TITRE que la carte dans
	/// chaque langue : « la copie hors deck sur la carte imprimée ».
	///
	/// <para><b>Pourquoi cette règle.</b> Deux rangées jumelles produisent la même identité OWL
	/// (l'IRI dérive du titre anglais) : des titres divergents y font porter à UN concept deux
	/// <c>prefLabel</c> différents par langue — SKOS n'admet qu'un <c>prefLabel</c> par langue et
	/// par concept (#1622 point 2). Le grain a aligné 71 cellules sur 25 rangées hors deck
	/// (fa 23 · ru 13 · ar 13 · zh 10 · pt 7 · es 5), toutes en profondeur &gt; 3 : aucun bandeau
	/// ne référence leur titre, l'invariant ⑱w (bandeau = titre du rang) est neutre, et AUCUN
	/// PDF ne bouge (la copie imprimée est la source, jamais la cible).</para>
	///
	/// <para><b>Périmètre assumé (écart déclaré dans la PR).</b> Les paires jumelles dont AUCUNE
	/// copie n'est imprimée (52 groupes mesurés) restent hors périmètre : la règle est muette
	/// quand il n'y a pas de carte imprimée à faire autorité — arbitrage à part. Les paires
	/// homonymes (même titre anglais, titres français DIFFÉRENTS = concepts distincts) sont hors
	/// clé par construction : la clé est le couple (fr, en).</para>
	///
	/// <para><b>Anti-vacuité.</b> Le nombre de groupes imprimés est épinglé (23 au grain, mesuré
	/// sur <c>3bd84bc1</c>) : une lecture vide rendrait l'invariant vert sans rien prouver. Le
	/// compte ne peut pas descendre sans qu'un concept dupliqué disparaisse — recaler avec la
	/// date, jamais effacer la ligne.</para>
	///
	/// <para><b>Grain ②b (dispatch c.5920874826, suite du point 2 de #1622).</b> La clé
	/// <c>(text_fr, desc_fr)</c> — titre ET définition français identiques — capture les groupes
	/// que la clé (fr, en) ne voyait pas : 34 groupes, dont 27 divergents (87 cellules). Le grain
	/// a écrit les 83 cellules éditables (la paire « Concept volé » est exclue nominativement,
	/// voir plus bas) : le meilleur des deux titres par langue, critère = le terme reconnu dans
	/// la langue, pas un calque de l'anglais. Une seule des écritures change l'identifiant OWL
	/// (<c>GetId</c> dérive du titre EN) : « Question piège » — la copie PK 701 rejoint la carte
	/// PK 179 sur « Loaded question », <c>trickQuestion</c> tombe de l'ensemble produit (orpheline
	/// nommée du census en attendant la re-dérivation n°2) — annoncé à CoursIA au resync. « Pétition
	/// de principe analogique » — le tiret est aligné sur « Question-begging analogy » (PK 703 reçoit
	/// la forme de 840) SANS effet d'IRI : <c>GetId</c> retire le tiret, les deux formes mintent déjà
	/// le même fragment <c>questionBeggingAnalogy</c> (la collision convergente que #1622 a mesurée) —
	/// l'alignement répare le double <c>prefLabel</c> EN, il ne déplace aucune identité.</para>
	///
	/// <para><b>Exclusion nominative.</b> « Self-refuting idea » (PK 779) / « Stolen concept
	/// fallacy » (PK 828) portent le même couple (titre, définition) français mais dénotent deux
	/// notions DISTINCTES en anglais : la consigne du dispatch est de ne rien écrire et de le
	/// rapporter — la question de savoir si le français les confond à tort reste ouverte côté
	/// owner. La garde exclut cette paire NOMINATIVEMENT (par PK), jamais par valeur.</para>
	///
	/// <para><b>Anti-vacuité recalibrée.</b> Groupes imprimés sous la clé (fr, en) : 24 après ②b
	/// (les 23 du grain #1686 + « Question piège » dont la copie a rejoint la carte via
	/// l'alignement anglais ; recalibré le 2026-10-01, était 23 sur <c>3bd84bc1</c>). Groupes
	/// sous la clé (fr, définition) : 34 épinglés — recaler avec la date, jamais effacer la
	/// ligne.</para>
	/// </summary>
	public class DuplicateConceptTitleAlignmentGuardTests
	{
		private static readonly string[] Languages = { "fr", "en", "ru", "pt", "es", "ar", "fa", "zh" };

		private static List<Dictionary<string, string>> LoadRows()
		{
			var csvPath = Path.Combine(TestRepoRoot.Find(),
				"Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				MissingFieldFound = null,
				BadDataFound = null,
				HeaderValidated = null,
			};
			using var reader = new StringReader(File.ReadAllText(csvPath));
			using var csv = new CsvReader(reader, config);
			var rows = new List<Dictionary<string, string>>();
			foreach (var record in csv.GetRecords<dynamic>())
			{
				var dict = (IDictionary<string, object>)record;
				rows.Add(dict.ToDictionary(kv => kv.Key, kv => kv.Value?.ToString() ?? string.Empty,
					StringComparer.Ordinal));
			}
			rows.Should().HaveCount(1408, "la taxonomie des sophismes compte 1408 rangées.");
			return rows;
		}

		private static string Cell(Dictionary<string, string> row, string column) =>
			(row.GetValueOrDefault(column) ?? string.Empty).Trim();

		/// <summary>
		/// Les paires jumelles AVEC carte imprimée : (identité fr+en) → membres. Partagé par
		/// l'invariant et le témoin nommé.
		/// </summary>
		private static List<List<Dictionary<string, string>>> DeckBackedDuplicateGroups(
			List<Dictionary<string, string>> rows)
		{
			return rows
				.Where(r => Cell(r, "text_fr").Length > 0 && Cell(r, "text_en").Length > 0)
				.GroupBy(r => (Fr: Cell(r, "text_fr"), En: Cell(r, "text_en")))
				.Where(g => g.Count() > 1 && g.Any(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("carte"))))
				.Select(g => g.OrderBy(r => r.GetValueOrDefault("PK"), StringComparer.Ordinal).ToList())
				.ToList();
		}

		[Fact]
		public void OffDeck_Copies_Of_Printed_Concepts_Carry_The_Card_Titles()
		{
			var rows = LoadRows();
			var groups = DeckBackedDuplicateGroups(rows);

			groups.Should().HaveCount(24,
				"anti-vacuité : 24 concepts dupliqués à carte imprimée (les 23 du grain #1686 + « Question " +
				"piège » dont la copie PK 701 a rejoint la carte PK 179 par l'alignement anglais du grain " +
				"②b ; recalibré le 01/10, était 23 sur 3bd84bc1). Le compte ne peut pas descendre sans " +
				"qu'un concept dupliqué disparaisse — recaler avec la date, jamais effacer la ligne.");

			var failures = new List<string>();
			foreach (var group in groups)
			{
				var deck = group.First(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("carte")));
				foreach (var off in group.Where(r => string.IsNullOrWhiteSpace(r.GetValueOrDefault("carte"))))
				{
					foreach (var lang in Languages)
					{
						var deckTitle = Cell(deck, "text_" + lang);
						var offTitle = Cell(off, "text_" + lang);
						if (deckTitle != offTitle)
							failures.Add(
								$"PK {off.GetValueOrDefault("PK")} [{lang}] «{offTitle}» != carte PK " +
								$"{deck.GetValueOrDefault("PK")} «{deckTitle}»");
					}
				}
			}

			failures.Should().BeEmpty(
				"l'invariant du grain : chaque copie hors deck d'un concept imprimé porte le titre de " +
				"LA CARTE dans toutes les langues (71 cellules alignées, fa 23 · ru 13 · ar 13 · zh 10 · " +
				$"pt 7 · es 5). Écarts restants : {string.Join(" ; ", failures.Take(8))}");
		}

		[Fact]
		public void Pk1123_Es_Aligned_On_Its_Printed_Twin()
		{
			// Témoin nommé : « Généralisation abusive » / « Faulty generalisation » — la carte est
			// le PK 595 (rang 3.1), la copie hors deck est le PK 1123 (6.3.1.2.2.1.3). Avant le
			// grain, la copie portait « Generalización excesiva » et « 错误的普遍化 ».
			var rows = LoadRows();
			var byPk = rows.ToDictionary(r => Cell(r, "PK"), StringComparer.Ordinal);
			var deck = byPk["595"];
			var off = byPk["1123"];
			(!string.IsNullOrWhiteSpace(deck.GetValueOrDefault("carte"))).Should().BeTrue(
				"le témoin suppose que le PK 595 est la carte imprimée du concept.");
			string.IsNullOrWhiteSpace(off.GetValueOrDefault("carte")).Should().BeTrue(
				"le témoin suppose que le PK 1123 est la copie hors deck.");
			Cell(off, "text_es").Should().Be(Cell(deck, "text_es"),
				"la copie hors deck a pris le titre espagnol de la carte imprimée.");
			Cell(off, "text_zh").Should().Be(Cell(deck, "text_zh"),
				"idem en chinois — pas l'ancien 错误的普遍化.");
		}

		/// <summary>
		/// Les groupes de la clé ②b : (titre français, définition française) identiques. C'est la clé
		/// du dispatch c.5920874826 — elle voit des jumeaux que la clé (fr, en) ignore, parce que le
		/// titre anglais peut DIVERGER entre les deux rangées (c'est même l'objet des deux
		/// alignements EN du grain).
		/// </summary>
		private static List<List<Dictionary<string, string>>> FrTitleAndDefinitionDuplicateGroups(
			List<Dictionary<string, string>> rows)
		{
			return rows
				.Where(r => Cell(r, "text_fr").Length > 0 && Cell(r, "desc_fr").Length > 0)
				.GroupBy(r => (Fr: Cell(r, "text_fr"), Def: Cell(r, "desc_fr")))
				.Where(g => g.Count() > 1)
				.Select(g => g.OrderBy(r => r.GetValueOrDefault("PK"), StringComparer.Ordinal).ToList())
				.ToList();
		}

		[Fact]
		public void FrTitleAndDefinition_Groups_Are_Uniform_Except_The_Nominative_Pair()
		{
			var rows = LoadRows();
			var groups = FrTitleAndDefinitionDuplicateGroups(rows);

			groups.Should().HaveCount(34,
				"anti-vacuité : 34 groupes (titre + définition français identiques) mesurés au grain ②b " +
				"(dd902694 + écriture, 01/10). Recaler avec la date, jamais effacer la ligne.");

			// L'exclusion est NOMINATIVE, par PK — jamais par valeur : la consigne du dispatch est
			// que cette paire-là ne s'écrit pas tant que l'owner n'a pas tranché si le français
			// confond à tort deux notions que l'anglais distingue.
			var excluded = new HashSet<string> { "779", "828" };
			var byPk = rows.ToDictionary(r => Cell(r, "PK"), StringComparer.Ordinal);
			Cell(byPk["779"], "text_en").Should().Be("Self-refuting idea",
				"l'exclusion nominative suppose que le PK 779 porte exactement ce titre anglais.");
			Cell(byPk["828"], "text_en").Should().Be("Stolen concept fallacy",
				"idem PK 828 — si l'un des deux change, l'exclusion doit être réexaminée, pas contournée.");

			var failures = new List<string>();
			foreach (var group in groups)
			{
				if (group.All(r => excluded.Contains(Cell(r, "PK"))))
					continue;
				foreach (var lang in Languages)
				{
					var vals = new HashSet<string>(group.Select(r => Cell(r, "text_" + lang)));
					if (vals.Count > 1)
						failures.Add(
							$"PK {string.Join("/", group.Select(r => r.GetValueOrDefault("PK")))} [{lang}] : " +
							$"{string.Join(" ≠ ", vals.OrderBy(v => v, StringComparer.Ordinal))}");
				}
			}

			failures.Should().BeEmpty(
				"l'invariant ②b : chaque groupe (titre + définition français identiques) porte UN titre " +
				"par langue, hors la paire exclue nommément (PK 779/828, notions distinctes en anglais, " +
				$"arbitrage owner ouvert). Écarts restants : {string.Join(" ; ", failures.Take(8))}");
		}

		[Fact]
		public void QuestionPiege_Copy_Joins_Its_Printed_Twin_And_The_Hyphen_Is_Aligned()
		{
			// Témoin des deux alignements anglais du grain ②b — les seuls qui changent l'IRI OWL
			// (GetId dérive du titre EN) et qui sont donc annoncés à CoursIA au resync.
			var rows = LoadRows();
			var byPk = rows.ToDictionary(r => Cell(r, "PK"), StringComparer.Ordinal);

			// « Question piège » : la carte est le PK 179 (rang 2.1.1.1.1), la copie hors deck le
			// PK 701 (4.1.1.2), qui portait « Trick question ».
			var card = byPk["179"];
			var copy = byPk["701"];
			(!string.IsNullOrWhiteSpace(card.GetValueOrDefault("carte"))).Should().BeTrue(
				"le témoin suppose que le PK 179 est la carte imprimée.");
			string.IsNullOrWhiteSpace(copy.GetValueOrDefault("carte")).Should().BeTrue(
				"le témoin suppose que le PK 701 est la copie hors deck.");
			Cell(copy, "text_en").Should().Be("Loaded question",
				"l'alignement EN du grain : la copie rejoint la carte (anciennement « Trick question ») — " +
				"les deux rangées produisent désormais le MÊME IRI OWL (loadedQuestion).");
			foreach (var lang in Languages)
				Cell(copy, "text_" + lang).Should().Be(Cell(card, "text_" + lang),
					$"la copie hors deck porte le titre de la carte en {lang}, comme au grain #1686.");

			// « Pétition de principe analogique » : écart de tiret — « Question begging analogy »
			// (PK 703) aligné sur « Question-begging analogy » (PK 840). Sans effet d'IRI : GetId
			// retire le tiret, les deux formes mintent déjà questionBeggingAnalogy (mesure #1622 —
			// la paire était la collision convergente épinglée) ; l'alignement répare le double
			// prefLabel EN.
			Cell(byPk["703"], "text_en").Should().Be("Question-begging analogy",
				"l'écart de tiret est résolu au profit de la forme avec tiret (PK 840).");
			Cell(byPk["703"], "text_en").Should().Be(Cell(byPk["840"], "text_en"),
				"les deux rangées du concept portent désormais le même titre anglais.");
		}

		[Fact]
		public void Deck_Still_Has_175_Cards()
		{
			var rows = LoadRows();
			rows.Count(r => !string.IsNullOrWhiteSpace(r.GetValueOrDefault("carte"))).Should().Be(175,
				"le grain n'écrit que des copies hors deck — le deck ne change pas de taille (#1288).");
		}
	}
}
