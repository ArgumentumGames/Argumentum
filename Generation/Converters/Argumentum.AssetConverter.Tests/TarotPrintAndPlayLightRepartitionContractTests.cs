using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Organe de la sélection <c>KnownCardSets.FallaciesPrintAndPlayLight</c> (35 cartes de
	/// sophismes, filtre <c>print_and_play=1</c> hérité de la démo février 2022). Cette sélection
	/// n'est curatée nulle part dans le code : elle EST l'état de la colonne CSV.
	///
	/// <para><b>Le document qu'elle sert — nommé, pas numéroté.</b> <c>FallaciesPrintAndPlayLight</c>
	/// est agrégée par un seul document, <c>Argumentum_TarotCards_Print&amp;Play_Light_A4_fr.pdf</c>,
	/// aux côtés de <c>KnownCardSets.RulesPrintAndPlay</c> et <c>KnownCardSets.MemoPrintAndPlay</c>.
	/// Le document homologue <c>Argumentum_PokerCards_Print&amp;Play_Light_A4_fr.pdf</c> n'agrège
	/// qu'un jeu, <c>KnownCardSets.ScenariiPrintAndPlay</c> — un lecteur qui suit les <i>noms</i>
	/// voit cela d'un coup d'œil ; celui qui suit les numéros de ligne de
	/// <c>WebBasedGeneratorConfig</c> attribue au Poker le jeu du bloc Tarot (erreur commise dans
	/// la première version de cet organe, corrigée en revue). C'est pourquoi cette garde-là ne cite
	/// plus aucun numéro de ligne : les symboles sont stables, les lignes non.</para>
	///
	/// <para><b>Pourquoi cette sélection a son organe.</b> <c>#1534</c>/<c>#1500</c> ont épinglé la
	/// répartition de <c>KnownCardSets.ScenariiPrintAndPlay</c> (3 des 7 catégories absentes,
	/// « relation intime » à 14/27 = 52 %). La sélection Scenarii vit dans un autre document et un
	/// autre CSV — <b>même contrat, autre sélection</b>. Celle-ci, les 35 cartes de sophismes,
	/// n'avait pas d'organe : une dérive de la colonne <c>print_and_play</c> sur la taxonomie
	/// Fallacies n'aurait été vue par personne, exactement comme celle de Scenarii l'a été par
	/// personne pendant trois ans.</para>
	///
	/// <para><b>ÉTAT MESURÉ AU 2026-09-30 — LA GARDE EST VERTE, ET AUCUN DÉFAUT N'EST MESURÉ ICI.</b>
	/// 35 cartes Light, 7 familles imprimées à 5 cartes chacune (5/35 = 14,3 % pour la dominante,
	/// très en dessous du seuil 33 %). Contrairement à <c>ScenariiPrintAndPlay</c>, <b>cette garde n'est pas
	/// née d'une régression réparée mais d'une capacité non protégée</b> : l'état est sain, et ce qui
	/// manquait était le témoin qui rougirait si l'état cessait de l'être. La doctrine
	/// <c>#1046</c> — « une garde jamais vue rouge est un no-op » — est honorée par les témoins de
	/// la section (0), qui exercent le rouge sur des valeurs injectées, pas par un rouge du dépôt.</para>
	///
	/// <para><b>Ce que cette garde N'ÉTABLIT PAS</b> : la justesse des 35 cartes retenues, le nombre
	/// optimal de cartes Light, ni la part par famille au-delà du plafond de dominance. Elle épingle
	/// les deux invariants mesurables de <c>#1500</c> §3 (couverture totale, dominance ≤ 1/3) — plus
	/// l'<b>exclusion elle-même</b>, mesurée ci-dessous plutôt que recopiée d'une prose.</para>
	///
	/// <para><b>Virtues Light n'est PAS gardée ici, et c'est un choix mesuré.</b>
	/// <c>#1501</c>/<c>#1514</c> a retiré <c>KnownCardSets.VirtuesPrintAndPlayLight</c> du document
	/// <c>Argumentum_TarotCards_Print&amp;Play_Light_A4_fr.pdf</c> (décision owner du 22/09) ; le
	/// CardSet reste défini dans <c>WebBasedGeneratorConfig</c> — « on retire une
	/// AGRÉGATION, pas une capacité ». Aucun document n'imprime donc cette sélection aujourd'hui :
	/// il n'y a pas de carte rendue dont la répartition puisse dériver devant un lecteur. Le jour où
	/// la capacité est rebranchée, l'organe à écrire est le même qu'ici, avec la colonne
	/// <c>family_fr</c> et les 8 familles du corpus Vertus (mesuré le 2026-09-30 : 24 cartes Light,
	/// 8/8 familles, dominante 4/24 = 16,7 %).</para>
	/// </summary>
	public class TarotPrintAndPlayLightRepartitionContractTests
	{
		private static string RepoRoot => TestRepoRoot.Find();
		private static string FallaciesCsv =>
			Path.Combine(RepoRoot, "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");

		// La colonne de catégorie de cette taxonomie est `Famille` (FR), à l'image de `catégorie`
		// pour Scenarii et `family_fr` pour Virtues. Elle est passée à MeasureHead en tant que
		// paramètre depuis que l'organe sert les deux sélections.
		private const string FamilleColumn = "Famille";

		// Clé = libellé CSV `Famille` EXACT ; valeur = compte verbatim dans le deck complet
		// (175 lignes à `carte` non vide, post-#1288).
		//
		// ⚠️ CES 7 LIBELLÉS NE SONT PAS UNE LISTE RECOPIÉE : ils sont re-mesurés contre le CSV par
		// le Fact `ExpectedCategories_Are_Exactly_The_Corpus_Families_Minus_The_Root_Row`, qui exige
		// que l'écart entre les familles du corpus et celles-ci soit EXACTEMENT la rangée racine.
		// Le fichier Scenarii sœur porte ce même avertissement en commentaire (« la 6ᵉ a été
		// orthographiée "culture populaire" […], ce libellé N'EXISTE PAS dans le CSV ») : une clé
		// épinglée fausse rougit pour la mauvaise raison et ne peut jamais verdir. Ici l'avertissement
		// est exécutable, pas seulement écrit.
		private static readonly (string Category, int DeckCount)[] ExpectedCategories =
		{
			("Insuffisance",          24),
			("Influence",             30),
			("Erreur mathématique",   25),
			("Erreur de raisonnement", 24),
			("Abus de langage",       23),
			("Tricherie",             25),
			("Obstruction",           24),
		};

		// ─────────────────────────────────────────────────────────────────────────
		// (0) TÉMOINS D'ABORD (#1046 : un instrument jamais vu rouge est un no-op).
		//     Couverture partielle (l'instrument SAIT nommer une famille vidée) et
		//     couverture pleine (il ne crie pas sur un état conforme).
		// ─────────────────────────────────────────────────────────────────────────

		[Fact]
		public void Witness_A_Family_Emptied_From_Light_Is_Named_By_The_Gate()
		{
			// Valeurs injectées en mémoire : Tricherie vidée, Insuffisance dominante à 18/35.
			// Le témoin n'affirme rien sur l'arbre ; il affirme que l'instrument SAIT voir
			// l'absence ET la nommer. Vert par construction, comme son symétrique ci-dessous.
			var failures = LightCategoryBalanceContract.CheckCoverage(
				lightByCategory: new Dictionary<string, int>
				{
					["Insuffisance"] = 18,
					["Influence"] = 5,
					["Erreur mathématique"] = 4,
					["Erreur de raisonnement"] = 4,
					["Abus de langage"] = 4,
					["Tricherie"] = 0,
					["Obstruction"] = 0,
				},
				expectedCategories: ExpectedCategories,
				exercisedLabel: "familles exercées");

			failures.Should().NotBeEmpty(
				"2 familles vidées sur 7 — la garde DOIT rougir, sinon elle ne protège rien");
			failures.Should().Contain(f => f.Contains("Tricherie") && f.Contains("0"),
				"le défaut doit NOMMER la famille absente (Tricherie : 0), pas rendre un score global muet");
			failures.Should().Contain(f => f.Contains("Obstruction") && f.Contains("0"),
				"symétrie : la seconde famille vidée doit apparaître aussi — un message qui n'en nomme "
				+ "qu'une obligerait l'owner à relancer la garde N fois");
			failures.Should().Contain(f => f.Contains("0/7 familles exercées"),
				"#1665 : le libellé accordé vient de l'appelant Tarot (« familles exercées », féminin), "
				+ "pas du littéral « dos exercés » du contrat — c'est l'appelant qui connaît le genre "
				+ "(renvoi c.5909445912 : le participe vient avec le nom)");
			// ⚠️ Assertion portée sur la FRACTION (18/35), jamais sur le pourcentage rendu :
			// `{share:P1}` dépend de la culture du runner (la v1 de l'organe Scenarii assertait
			// « 51, » et était verte en local, rouge en CI — mesuré 2026-09-25). La fraction est
			// le même fait, écrit sans séparateur décimal, donc identique sur toute machine.
			failures.Should().Contain(f => f.Contains("Insuffisance") && f.Contains("18/35"),
				"la dominance doit être NOMMÉE ET QUANTIFIÉE — la couverture seule ne dit rien du ton");
		}

		[Fact]
		public void Witness_Full_Light_Coverage_Produces_No_Failure()
		{
			// Contrôle inverse : 7/7 couvertes, dominante 5/35 = 14,3 % — l'état réellement
			// mesuré sur HEAD. Sans ce sens, une garde toujours-rouge couvrirait le défaut
			// en silence au lieu de le signaler.
			var failures = LightCategoryBalanceContract.CheckCoverage(
				lightByCategory: new Dictionary<string, int>
				{
					["Insuffisance"] = 5,
					["Influence"] = 5,
					["Erreur mathématique"] = 5,
					["Erreur de raisonnement"] = 5,
					["Abus de langage"] = 5,
					["Tricherie"] = 5,
					["Obstruction"] = 5,
				},
				expectedCategories: ExpectedCategories,
				exercisedLabel: "familles exercées");

			failures.Should().BeEmpty(
				"7/7 couvertes ≥ 1 et dominante 5/35 sous le seuil — l'invariant est tenu, la garde ne crie pas");
		}

		[Fact]
		public void Witness_A_Tarot_Light_Shaped_Csv_Passes_All_Checks()
		{
			// Témoin de PLOMBERIE : un CSV à la forme de la taxonomie Fallacies (colonne `Famille`,
			// 7 familles imprimées) traverse la surcharge `MeasureHead(csv, "Famille")` et verdit.
			// Il prouve que le nom de colonne se résout et que la mesure rend ce qu'elle annonce.
			// ⚠️ Il ne prouve RIEN sur les libellés de famille : il les fabrique lui-même, donc il
			// serait vert même si `ExpectedCategories` portait des clés inexistantes dans le vrai
			// CSV. C'est le Fact (1) sur l'état vivant qui établit cette vérité-là.
			var dir = Path.Combine(Path.GetTempPath(), "argumentum-1500-tarot-" + Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(dir);
			var csv = Path.Combine(dir, "tarot-light-healthy.csv");
			try
			{
				File.WriteAllText(csv,
					"path,Famille,carte,print_and_play\n" +
					"1.1.1,Insuffisance,1,1\n" +
					"1.2.1,Influence,1,1\n" +
					"1.3.1,Erreur mathématique,1,1\n" +
					"1.4.1,Erreur de raisonnement,1,1\n" +
					"1.5.1,Abus de langage,1,1\n" +
					"1.6.1,Tricherie,1,1\n" +
					"1.7.1,Obstruction,1,1\n");

				var (counts, lightTotal, deckTotal) = LightCategoryBalanceContract.MeasureHead(csv, FamilleColumn);
				counts.Should().HaveCount(7, "CSV sain = 7 familles Light distinctes");
				lightTotal.Should().Be(7);
				deckTotal.Should().Be(7, "7 familles distinctes portent une carte dans ce CSV");

				var failures = LightCategoryBalanceContract.CheckCoverage(counts, ExpectedCategories, "familles exercées");
				failures.Should().BeEmpty("CSV sain : 7/7 couvertes ≥ 1, dominante 1/7 < 33 % — l'organe verdit");
			}
			finally
			{
				try { Directory.Delete(dir, recursive: true); } catch (IOException) { }
			}
		}

		// ─────────────────────────────────────────────────────────────────────────
		// (1) LA GARDE SUR L'ÉTAT VIVANT — la colonne `print_and_play` de la
		//     taxonomie Fallacies, telle qu'elle est committée. C'est cette colonne
		//     qui définit `KnownCardSets.FallaciesPrintAndPlayLight`.
		// ─────────────────────────────────────────────────────────────────────────

		[Fact]
		public void Tarot_Light_Covers_All_7_Printed_Families_On_Head()
		{
			var (counts, _, _) = LightCategoryBalanceContract.MeasureHead(FallaciesCsv, FamilleColumn);

			var missing = ExpectedCategories
				.Where(t => !counts.TryGetValue(t.Category, out var n) || n <= 0)
				.Select(t => $"\"{t.Category}\" (deck={t.DeckCount}, light=0)")
				.ToList();
			missing.Should().BeEmpty(
				"la sélection Light du Tarot omet " + missing.Count + " famille(s) imprimée(s) sur 7 — "
				+ "un seul message doit nommer TOUTES les manquantes pour que l'owner agisse sans relancer "
				+ "la garde N fois. Manquantes : " + string.Join(", ", missing)
				+ ". Source : " + FallaciesCsv);
		}

		[Fact]
		public void Tarot_Light_Dominant_Family_Stays_Under_One_Third_On_Head()
		{
			var (counts, _, _) = LightCategoryBalanceContract.MeasureHead(FallaciesCsv, FamilleColumn);

			var total = counts.Values.Sum();
			total.Should().BeGreaterThan(0, "il faut au moins 1 carte Light pour parler de répartition");
			var dominant = counts.Values.Max();
			var dominantShare = (double)dominant / total;
			dominantShare.Should().BeLessThan(1.0 / 3.0,
				"#1500 RX11 : une famille qui pèse plus d'un tiers de la démo Light est un problème de ton, "
				+ "indépendamment de la couverture 7/7. Mesuré : dominant = " + dominant + ", total = " + total
				+ ", soit " + dominant + "/" + total);
		}

		[Fact]
		public void Tarot_Light_Is_Not_Empty_On_Head()
		{
			// Une sélection Light vide ferait RUNNER la chaîne sans rendre aucun PDF — fail-loud
			// ici est moins coûteux que la trace « 0 image » d'un run complet (#1187).
			var (counts, lightTotal, _) = LightCategoryBalanceContract.MeasureHead(FallaciesCsv, FamilleColumn);
			lightTotal.Should().BeGreaterThan(0,
				"#1187 fail-loud : une sélection Light vide ne produirait aucun PDF — la garde crie avant le run.");
			counts.Values.Sum().Should().Be(lightTotal,
				"aucune carte Light ne doit porter une `Famille` vide : elle échapperait au décompte par "
				+ "famille tout en comptant dans le total, donc la couverture serait fausse par le bas");
		}

		/// <summary>
		/// L'exclusion qui rend <see cref="ExpectedCategories"/> légitime, MESURÉE au lieu d'être
		/// supposée. Le corpus Fallacies porte 8 libellés <c>Famille</c> distincts ; le deck n'en
		/// imprime que 7. L'écart est exactement la rangée racine (<c>PK=0</c>, <c>path=0</c>,
		/// <c>depth=0</c>, <c>carte</c> vide) — une rangée de structure, jamais une famille qui
		/// aurait perdu ses cartes.
		/// <para>Ce Fact existe pour une raison précise : sans lui, un futur lecteur qui compte
		/// 8 familles face à une liste de 7 ne peut pas distinguer « la 8ᵉ est la racine, par
		/// construction » de « la 8ᵉ a été silencieusement retirée de la liste pour faire verdir
		/// la garde ». C'est la porte de sortie qu'un <c>ExpectedCategories</c> non contraint
		/// laisse toujours ouverte — la même forme que l'avertissement de l'organe Scenarii sur
		/// « culture populaire », mais ici exécutable.</para>
		/// </summary>
		[Fact]
		public void ExpectedCategories_Are_Exactly_The_Corpus_Families_Minus_The_Root_Row()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);

			var corpusFamilies = csv.LoadColumnSet(FamilleColumn);
			corpusFamilies.Should().HaveCount(8,
				"la taxonomie Fallacies porte 8 libellés `Famille` distincts (aucune rangée sans famille) : "
				+ string.Join(" · ", corpusFamilies.OrderBy(f => f, StringComparer.Ordinal)));

			var lightFamilies = csv.LoadColumnSet(FamilleColumn, "print_and_play", new[] { "1" });
			corpusFamilies.Except(lightFamilies).Should().BeEquivalentTo(new[] { "Argument fallacieux" },
				"l'écart entre le corpus et la sélection Light doit être EXACTEMENT la rangée racine. "
				+ "S'il s'élargit, une famille imprimée a été vidée du Light sans que la liste des 7 "
				+ "attendues ne le dise — et la garde de couverture, elle, ne le verrait pas.");

			// La racine est mesurée comme telle, pas supposée : la rangée qui porte ce libellé
			// n'a pas de carte. Si elle en gagnait une, elle entrerait au deck et les 7 attendues
			// deviendraient 8 — la garde doit le dire plutôt que de rester verte sur une liste périmée.
			var rootCarte = csv.LoadColumn("carte", FamilleColumn, new[] { "Argument fallacieux" });
			rootCarte.Should().HaveCount(1, "une seule rangée porte le libellé racine dans la taxonomie");
			rootCarte[0].Trim().Should().BeEmpty(
				"la rangée racine n'est pas une carte : `carte` vide. Le jour où elle en porterait une, "
				+ "le décompte des familles imprimées passerait à 8 et cette garde devrait être révisée "
				+ "— pas contournée.");
		}

		[Fact]
		public void Tarot_Light_Is_A_Subset_Of_The_Deck_On_Head()
		{
			// Contrôle inverse, à l'image de `Deck_Selection_Still_Sums_To_167_On_Head` (Scenarii) :
			// si le filtre Light se mettait à consommer des cartes du deck complet — ou à en rendre
			// qui n'en sont pas —, la répartition mesurée plus haut serait celle d'un ensemble qui
			// n'existe pas. #1204 (« Scenarii cru à 97 ») a été attrapé par cette symétrie.
			var csv = new HarvestCardIdsCsv(FallaciesCsv);

			var deckCards = csv.LoadColumn("carte").Count(v => !string.IsNullOrWhiteSpace(v));
			deckCards.Should().BeInRange(170, 180,
				"contrôle inverse du deck Fallacies : " + deckCards + " carte(s) à `carte` non vide. "
				+ "L'épinglage EXACT (175, post-#1288) vit dans FallaciesDeckBandInvariantGuardTests."
				+ "Deck_Still_Has_175_Cards ; ici on ne surveille que la dérive grossière. Un deck effondré "
				+ "rendrait la couverture Light vraie sur un ensemble qui n'est plus le jeu.");

			var lightCarte = csv.LoadColumn("carte", "print_and_play", new[] { "1" });
			lightCarte.Should().HaveCount(35,
				"`KnownCardSets.FallaciesPrintAndPlayLight` compte 35 cartes (démo #645, filtre "
				+ "`print_and_play=1` de la taxonomie Fallacies)");
			lightCarte.Count(v => string.IsNullOrWhiteSpace(v)).Should().Be(0,
				"chaque carte Light doit porter une carte (`carte` non vide) : une carte Light sans carte "
				+ "serait comptée dans une famille et absente du jeu — la sélection Light est un "
				+ "SOUS-ENSEMBLE du deck, pas un ensemble parallèle.");
		}
	}
}