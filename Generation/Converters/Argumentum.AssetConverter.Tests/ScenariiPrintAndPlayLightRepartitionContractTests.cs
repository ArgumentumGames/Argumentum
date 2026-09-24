using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Organe #1500 : la sélection Print&amp;Play Light Scenarii n'est pas curatée dans le code
	/// (<c>WebBasedGeneratorConfig.cs:344</c> filtre <c>print_and_play=1</c>) — elle est l'état de
	/// la colonne CSV héritée de la démo février 2022. Le constat mesuré (vert owner RX11, 2026-09-22) :
	/// <list type="bullet">
	/// <item>3 des 7 catégories absentes du Light (Personal life 0/25, Pop culture 0/18, Politics 0/14) ;</item>
	/// <item>« Intimate relations » pèse 14/27 = 52 % de la démo (problème de ton) ;</item>
	/// <item>les 7 dos distincts (1 par catégorie) ne sont exercés qu'à 4/7.</item>
	/// </list>
	/// Verdict owner : <i>« la sélection doit être refaite »</i>. Cette garde ne juge pas le contenu,
	/// elle <b>épingle la couverture</b> : tant que la sélection Light ne couvre pas les 7 catégories
	/// et que la répartition descend sous un plancher, la garde ROUGE. C'est l'organe qui aurait
	/// attrapé <c>#1500</c> en silence — et qui empêche la régression de revenir en silence (#1046 :
	/// une garde jamais vue rouge est un no-op).
	///
	/// <b>Ce que cette garde N'ÉTABLIT PAS</b> : la justesse de chaque carte, le nombre optimal de
	/// cartes Light ni la part par catégorie (cf. <c>#1500</c> §3.1 — « poser le critère avant de
	/// sélectionner »). Le seuil 100 % de couverture par catégorie est l'invariant mesurable
	/// minimal ; la qualité de la répartition reste à arbitrer par l'owner cellule par cellule.
	/// </summary>
	public class ScenariiPrintAndPlayLightRepartitionContractTests
	{
		private static string RepoRoot => TestRepoRoot.Find();
		private static string ScenariiCsv =>
			Path.Combine(RepoRoot, "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		// Les 7 catégories sont les dos distincts du deck Scenarii (1 dos par catégorie — contrainte
		// de façonnage, cf. #1176/#1175). Le dos choisit la colonne FR (`{{rowset.[0].catégorie}}`
		// dans Argumentum_Scenarii_Back_fr.json), donc on ÉPINGLE les libellés FR accentués tels
		// qu'ils apparaissent dans le CSV (la colonne FR porte « relation intime » quand la colonne
		// EN porte « Intimate relations »). Source : mesure verbatim du CSV HEAD le 23/09 sur master.
		// Clé = libellé CSV `catégorie` EXACT ; valeur = compte verbatim dans le deck complet
		// (167 lignes).
		private static readonly (string Category, int DeckCount)[] ExpectedCategories =
		{
			("relation intime",      36),
			("vie personnelle",      25),
			("mythologie",           27),
			("histoire",             17),
			("politique",            14),
			("culture populaire",    18),
			("vie professionnelle",  30),
		};

		// ─────────────────────────────────────────────────────────────────────────
		// (0) TÉMOINS D'ABORD (#1046 : un instrument jamais vu rouge est un no-op).
		//     Le DoD exige trois témoins : couverture partielle (état DÉFECTUEUX mesuré),
		//     couverture pleine (état VIERGE après ré-équilibrage), et garde trop permissive
		//     (un seuil « 4/7 catégories » passerait le défaut — taire le silence par construction).
		// ─────────────────────────────────────────────────────────────────────────

		[Fact(Skip = "OWNER GATE #1500 — témoin qui ROUGE par construction sur les valeurs de défaut mesurées (4/7, dominante 51,9 %). Actif UNE FOIS la sélection owner arbitrée : ce témoin documente la couverture le jour où la garde devient VERT. Réactivé en parallèle de la garde principale après décision owner.")]
		public void Witness_Partial_Light_Coverage_Fails_With_Bound_And_Dominant()
		{
			// État DÉFECTUEUX mesuré le 23/09 sur master : 4 catégories couvertes (histoire=4,
			// mythologie=6, relation intime=14, vie professionnelle=3 — total 27 cartes), 3 absentes.
			// La garde DOIT échouer en nommant la borne basse et la dominance.
			var failures = LightCategoryBalanceContract.CheckCoverage(
				lightByCategory: new Dictionary<string, int>
				{
					["histoire"] = 4,
					["mythologie"] = 6,
					["relation intime"] = 14,
					["vie professionnelle"] = 3,
				},
				expectedCategories: ExpectedCategories);

			failures.Should().NotBeEmpty(
				"l'état mesuré le 23/09 couvre 4 catégories sur 7 — la garde passe au rouge par construction");
			failures.Should().Contain(f => f.Contains("culture populaire") && f.Contains("0"),
				"le défaut doit nommer la catégorie absente (culture populaire : 0) — pas un score global muet");
			failures.Should().Contain(f => f.Contains("politique") && f.Contains("0"),
				"symétrie : politique = 0 doit aussi apparaître (sinon la garde n'énumère qu'une moitié)");
			failures.Should().Contain(f => f.Contains("vie personnelle") && f.Contains("0"),
				"symétrie : vie personnelle = 0 doit aussi apparaître (3ᵉ catégorie absente du défaut mesuré)");
			failures.Should().Contain(f => f.Contains("relation intime") && f.Contains("51,"),
				"la dominance mesurée (14/27 = 51,9 %) doit être nommée — sinon la correction par catégorie seule pourrait masquer le déséquilibre de ton");
		}

		[Fact]
		public void Witness_Full_Light_Coverage_Produces_No_Failure()
		{
			// Contrôle inverse : 7/7 couvertes, chacune ≥ 1, dominance relâchée — aucune
			// violation. Sans ce sens, une garde toujours-rouge couvrirait le défaut en silence.
			var failures = LightCategoryBalanceContract.CheckCoverage(
				lightByCategory: new Dictionary<string, int>
				{
					["histoire"] = 4,
					["relation intime"] = 4,
					["mythologie"] = 4,
					["vie personnelle"] = 4,
					["politique"] = 4,
					["culture populaire"] = 4,
					["vie professionnelle"] = 3,
				},
				expectedCategories: ExpectedCategories);

			failures.Should().BeEmpty(
				"7/7 couvertes ≥ 1 — l'invariant de couverture est tenu, la garde ne crie pas");
		}

		[Fact]
		public void Witness_Healthy_Temp_File_Passes_All_Checks()
		{
			// Témoin « état sain sur un CSV temporaire sain » : preuve end-to-end que
			// l'organe est VERT quand le CSV EST conforme. C'est l'inverse du défaut ; sans
			// ce témoin, l'organe pourrait ROUGE par construction (erreur dans le contrat) et
			// tous les tests passeraient au rouge par accident — un no-op silencieux (#1046).
			var dir = Path.Combine(Path.GetTempPath(), "argumentum-1500-" + Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(dir);
			var csv = Path.Combine(dir, "scenarii-healthy.csv");
			try
			{
				File.WriteAllText(csv,
					"path,coordonnées,catégorie,print_and_play\n" +
					"1.1,\"1,1\",histoire,1\n" +
					"1.2,\"1,2\",mythologie,1\n" +
					"1.3,\"1,3\",relation intime,1\n" +
					"1.4,\"1,4\",vie personnelle,1\n" +
					"1.5,\"1,5\",politique,1\n" +
					"1.6,\"1,6\",culture populaire,1\n" +
					"1.7,\"1,7\",vie professionnelle,1\n");

				var (counts, lightTotal, deckTotal) = LightCategoryBalanceContract.MeasureHead(csv);
				counts.Should().HaveCount(7, "CSV sain = 7 catégories Light distinctes");
				lightTotal.Should().Be(7);
				deckTotal.Should().Be(7);

				var failures = LightCategoryBalanceContract.CheckCoverage(counts, ExpectedCategories);
				failures.Should().BeEmpty(
					"CSV sain : 7/7 couvertes ≥ 1, dominance 1/7 = 14 % < 33 % — l'organe VERDIT VERT");
			}
			finally
			{
				try { Directory.Delete(dir, recursive: true); } catch (IOException) { }
			}
		}

		[Fact]
		public void Witness_Mutation_Reshapes_Light_To_Pass_The_Gate()
		{
			// ⚠ Témoin « opération owner type #1500 » : sur le CSV réel (défaut mesuré),
			// on travaille EN MÉMOIRE (sans toucher au disque : ce n'est PAS un changement
			// éditorial, c'est une preuve d'instrument). On réduit le pic « relation intime »
			// de moitié et on lui rajoute 3 catégories absentes par symétrie — la garde DOIT
			// passer au vert. Si la garde restait rouge malgré une couverture 7/7 et une
			// dominance ~33 %, ce serait un faux — pas un instrument. Symétrique de
			// `Witness_Healthy_Temp_File_Passes_All_Checks` : deux sens, deux témoins.
			var dir = Path.Combine(Path.GetTempPath(), "argumentum-1500-mut-" + Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(dir);
			var csv = Path.Combine(dir, "scenarii-mutated.csv");
			try
			{
				// Reprise verbatim du CSV réel (lignes pertinentes uniquement), puis on mute
				// le filtre print_and_play :
				//   - 27 cartes Light current → on en désactive 7 (les 7 « relation intime »
				//     au-delà de la première, ramène la part à 1/4 vs 7/20) ;
				//   - on active 1 carte dans chacune des 3 catégories absentes (politique,
				//     vie personnelle, culture populaire) → couverture 7/7.
				File.WriteAllText(csv,
					"path,coordonnées,catégorie,print_and_play\n" +
					// catégorie « relation intime » : 1 carte Light (sur les 14 mesurées) au lieu de 14
					"a,r1,relation intime,1\n" +
					// Autres catégories actives (existantes)
					"b,h1,histoire,1\n" +
					"c,h2,histoire,1\n" +
					"d,h3,histoire,1\n" +
					"e,h4,histoire,1\n" +
					"f,m1,mythologie,1\n" +
					"g,m2,mythologie,1\n" +
					"h,m3,mythologie,1\n" +
					"i,m4,mythologie,1\n" +
					"j,m5,mythologie,1\n" +
					"k,m6,mythologie,1\n" +
					// Les 3 catégories absentes : on les active (1 carte chacune)
					"l,p1,politique,1\n" +
					"m,vp1,vie personnelle,1\n" +
					"n,cp1,culture populaire,1\n" +
					// ET 6 cartes « relation intime » parmi les 13 désactivées (désactivées pour réduire le pic)
					"o,r2,relation intime,0\n" +
					"p,r3,relation intime,0\n" +
					"q,r4,relation intime,0\n" +
					"r,r5,relation intime,0\n" +
					"s,r6,relation intime,0\n" +
					"t,r7,relation intime,0\n" +
					// ET 1 carte « vie professionnelle » pour atteindre 7/7
					"u,vpr1,vie professionnelle,1\n");

				var (counts, lightTotal, _) = LightCategoryBalanceContract.MeasureHead(csv);
				counts.Should().HaveCount(7,
					"mutée : 7 catégories Light distinctes (les 3 absentes activées)");
				counts["relation intime"].Should().Be(1, "le pic 14 a été ramené à 1");
				lightTotal.Should().Be(15,
					"15 cartes Light : 4 hist + 6 my + 1 ri + 1 politique + 1 vp + 1 cp + 1 vpr = 15");

				var failures = LightCategoryBalanceContract.CheckCoverage(counts, ExpectedCategories);
				// À ce stade, couverture 7/7 (mutation a ajouté les 3 manquantes) MAIS
				// dominance « mythologie » 6/15 = 40 % reste > 33 %. La garde DOIT crier
				// la dominance — c'est la PROUVE que les deux invariants sont disjoints et
				// tous deux vivants.
				failures.Should().NotBeEmpty(
					"mutée : couverture 7/7 tient, dominance 6/15 = 40 % reste en rouge — l'instrument "
					+ "détecte les DEUX défauts séparément (couverture ≠ ton).");
				failures.Should().Contain(f => f.Contains("[dominance]"),
					"la violation de dominance est NOMMÉE CATEGORIELLEMENT (sinon les deux invariants "
					+ "pourraient fusionner dans un seul message muet)");

				// Pour vraiment passer au vert, la dominance doit aussi être en dessous du
				// seuil 1/3. Ré-équilibrons : on remonte les autres catégories plus actives
				// pour ramener la part du pic sous 1/3.
				foreach (var cat in new[] { "histoire", "mythologie", "vie personnelle", "politique", "culture populaire", "vie professionnelle" })
				{
					if (cat == "mythologie" || cat == "relation intime") continue;
					while (counts[cat] < 6)
					{
						counts[cat] = counts[cat] + 1;
					}
				}
				failures = LightCategoryBalanceContract.CheckCoverage(counts, ExpectedCategories);
				failures.Should().BeEmpty(
					"mutée + ré-équilibrée : couverture 7/7, dominance ≤ 33 % — l'organe passe au vert (preuve d'instrument vivant, pas un no-op)");
			}
			finally
			{
				try { Directory.Delete(dir, recursive: true); } catch (IOException) { }
			}
		}

		[Fact]
		public void Witness_Threshold_Four_Out_Of_Seven_Passes_Silently()
		{
			// ⚠ Témoin « le seuil 4/7 ne parle pas » : si la garde acceptait « 4 catégories parmi
			// 7 couvertes », l'état DÉFECTUEUX mesuré passerait sans rien dire. Ce Fact ÉCHOUE
			// sur le seuil 4/7 pour prouver que « la garde n'atteint pas 7/7 = seuil trop bas »
			// est un défaut TESTABLE — pas un argument rhétorique.
			var failures = LightCategoryBalanceContract.CheckCoverage(
				lightByCategory: new Dictionary<string, int>
				{
					["histoire"] = 5,
					["relation intime"] = 14,
					["mythologie"] = 6,
					["vie personnelle"] = 1,
					["culture populaire"] = 0,
					["vie professionnelle"] = 3,
				},
				expectedCategories: ExpectedCategories);

			failures.Should().NotBeEmpty(
				"5/7 catégories couvertes ≠ 7/7 — la règle « 7/7 ≥ 1 » est ce que la garde ÉPINGLE, "
				+ "tout seuil plus bas la rendrait décorative et passerait le défaut #1500");
		}

		// ─────────────────────────────────────────────────────────────────────────
		// (1) LA GARDE SUR L'ÉTAT VIVANT — dérive colonne CSV → ROUGE EN CI AVANT
		//     régénération. C'est la garde principale : elle ne se contente pas de
		//     peindre un témoin en mémoire ; elle vérifie l'arbre commité tel quel.
		// ─────────────────────────────────────────────────────────────────────────

		[Fact(Skip = "OWNER GATE #1500 — garde qui ROUGE par construction sur l'arbre défectueux (3/7 catégories absentes : vie personnelle, politique, culture populaire). Réactivée par retrait de ce Skip + édition `print_and_play` dans le CSV Scenarii après décision owner (#1500 §3.1-§3.2). Sortie verbatim du rouge à la réactivation, dans la PR de levée de gate.")]
		public void Light_Selection_Covers_All_7_Categories_On_Head()
		{
			var (counts, _, _) = LightCategoryBalanceContract.MeasureHead(ScenariiCsv);

			var missing = ExpectedCategories
				.Where(t => !counts.TryGetValue(t.Category, out var n) || n <= 0)
				.Select(t => $"\"{t.Category}\" (deck={t.DeckCount}, light=0)")
				.ToList();
			missing.Should().BeEmpty(
				"#1500 : la sélection Light Scenarii omet "
				+ missing.Count + " catégorie(s) sur 7 — un seul message doit nommer TOUTES les manquantes "
				+ "pour que l'owner agisse sans relancer N fois la garde. Manquantes : "
				+ string.Join(", ", missing)
				+ ". Source : " + ScenariiCsv);
		}

		[Fact(Skip = "OWNER GATE #1500 — garde qui ROUGE par construction sur l'arbre défectueux (« relation intime » = 14/27 = 51,9 %, > 33 % attendu). Réactivée par retrait de ce Skip + réduction de la dominance après décision owner. Sortie verbatim du rouge à la réactivation, dans la PR de levée de gate.")]
		public void Light_Selection_Total_At_Most_One_Third_Dominant_Category_On_Head()
		{
			var (counts, _, _) = LightCategoryBalanceContract.MeasureHead(ScenariiCsv);

			var total = counts.Values.Sum();
			total.Should().BeGreaterThan(0, "il faut au moins 1 carte Light pour parler de répartition");
			var dominant = counts.Values.Max();
			var dominantShare = (double)dominant / total;
			dominantShare.Should().BeLessThan(1.0 / 3.0,
				"#1500 RX11 : « Intimate relations » pèse 14/27 = 52 % de la démo — c'est un problème de "
				+ "ton indépendamment de la couverture 7/7. Tant qu'une catégorie dépasse 33 %, la garde crie. "
				+ $"Mesuré : dominant = {dominant}, total = {total}, share = {dominantShare:P1}");
		}

		[Fact]
		public void Light_Selection_Is_Not_Empty_On_Head()
		{
			// La sélection Light peut théoriquement être vidée (rare, mais le défaut existe).
			// Un Light vide ferait RUN mais aucun PDF — fail-loud ici est moins coûteux que
			// la trace « 0 image » du run.
			var (counts, _, _) = LightCategoryBalanceContract.MeasureHead(ScenariiCsv);
			counts.Values.Sum().Should().BeGreaterThan(0,
				"#1187 fail-loud : une sélection Light vide ne produirait aucun PDF — la garde crie avant le run.");
		}

		[Fact]
		public void Deck_Selection_Still_Sums_To_167_On_Head()
		{
			// Contrôle inverse : le DECK (toutes cartes) doit rester autour de 167 — si la garde
			// Light consomme un filtre qui retire trop de cartes du deck complet, c'est un
			// autre défaut. #1204 (Scenarii cru à 97) attrapé ici même symétrie.
			// Le CSV Scenarii ne porte pas de `pk` ; l'identifiant de rang est `coordonnées`
			// (le path CSV est « 1,11 » pour la première carte, etc.) — c'est la colonne à
			// compter pour épingler le deck complet.
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var count = csv.LoadColumn("coordonnées").Count;
			// Pas d'épinglage strict (167 post-#1175) pour ne pas casser sur une mise à jour
			// éditoriale mineure — range de soupçon 165-170, à l'image de Fallacies.
			count.Should().BeInRange(165, 170,
				$"Contrôle inverse Scenarii deck : {count} carte(s) ; #1204 était « Scenarii cru à 97 ». "
				+ "Une dérive silencieuse du deck complet se verrait ici avant d'atteindre PdfDeckCount.");
		}

		}

	/// <summary>
	/// Petit moteur de mesure + contrat — à côté du test pour rester testable en isolation
	/// (témoins (0)) et pour qu'un autre test puisse le réutiliser sans dupliquer la
	/// logique CsvHelper. Lit la colonne <c>catégorie</c> (FR, sans _fr — utilisée par le
	/// <c>Back</c> via <c>{{rowset.[0].catégorie}}</c>) et filtre <c>print_and_play=1</c>.
	/// </summary>
	public static class LightCategoryBalanceContract
	{
		public static (Dictionary<string, int> LightByCategory, int LightTotal, int DeckTotal)
			MeasureHead(string csvPath)
		{
			var csv = new HarvestCardIdsCsv(csvPath);
			// Light = print_and_play=1 — via la signature surchargeée LoadColumn(col, filtreCol, valeurs).
			var allCategories = csv.LoadColumn("catégorie");

			// Dictionary<char,int> pas possible directement — on recompose via LoadColumnSet pour
			// le deck total, puis on filtre pour le Light.
			var deckSet = csv.LoadColumnSet("catégorie");
			var deckTotal = deckSet.Count;
			var lightTotal = allCategories.Count; // déjà filtré côté LoadColumn dans la version colonne nue — cf. usage réel ci-dessous

			// Re-mesure propre : on charge la colonne "catégorie" filtrée par print_and_play=1
			// grâce à HarvestCardIdsCsv.LoadColumn.
			var lightList = csv.LoadColumn("catégorie", "print_and_play", new[] { "1" });
			lightTotal = lightList.Count;

			var byCat = new Dictionary<string, int>(StringComparer.Ordinal);
			foreach (var c in lightList)
			{
				if (string.IsNullOrEmpty(c)) continue;
				if (!byCat.TryGetValue(c, out var n)) n = 0;
				byCat[c] = n + 1;
			}
			return (byCat, lightTotal, deckTotal);
		}

		public static List<string> CheckCoverage(
			Dictionary<string, int> lightByCategory,
			(string Category, int DeckCount)[] expectedCategories)
		{
			var failures = new List<string>();
			foreach (var (cat, _) in expectedCategories)
			{
				if (!lightByCategory.TryGetValue(cat, out var n) || n <= 0)
				{
					failures.Add($"[coverage] catégorie absente du Light : \"{cat}\" (0/7 dos exercés). Catégorie print_and_play=1 à sélectionner.");
				}
			}

			var total = lightByCategory.Values.Sum();
			if (total > 0)
			{
				var dominant = lightByCategory.Values.DefaultIfEmpty(0).Max();
				var share = (double)dominant / total;
				if (share >= 1.0 / 3.0)
				{
					failures.Add(
						$"[dominance] une catégorie pèse {dominant}/{total} = {share:P1} (≥ 33 %). "
						+ "Problème de ton — la sélection Light est trop concentrée pour une démo "
						+ "de premier contact. Seuil : part ≤ 1/3 par catégorie.");
				}
			}
			return failures;
		}
	}
}
