using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du grain ① du pool v22 — casse de phrase pt/es/en (décision ai-01 du 27/09
	/// sur #1600 c.5852792148, typographie : on corrige toujours). 1416 cellules :
	/// pt deck 78 titres + 1076 bandeaux (direction A), es 2 titres + 103 bandeaux,
	/// en 111 titres + 40 bandeaux (Sophismes) et 3 titres + 4 bandeaux (Vertus).
	/// Noms propres gardés : Chewbacca, Christianity, Hitler/Hitlerum, Linda, Moon,
	/// Morgan's, Flintstones, Rogers, Latin. Locutions latines en casse de phrase
	/// (« Ipse dixit », « Ad fidentia », « Resgate ad hoc »). Coquille : Cafetaria →
	/// Cafeteria (95). Jumeaux structurels exécutés (595/1123, 759/1127, 768/843,
	/// 1365/168 pt ; 977/1350 en) : même concept, nœud deck + nœud hors deck, titres
	/// EN déjà identiques — pas une réaffectation.
	/// </summary>
	public class CassePhraseGuardTests
	{
		private static string FallaciesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");
		private static string VirtuesCsv => Path.Combine(
			TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

		private static readonly Regex WordRe = new(
			@"[A-Za-zÀ-ÖØ-öø-ÿĒēĪīŌōŪū']+", RegexOptions.Compiled);

		private static readonly HashSet<string> PtConnectives = new(StringComparer.Ordinal)
		{
			"de", "da", "do", "das", "dos", "à", "ao", "aos", "a", "o", "os", "as", "e", "em",
			"no", "na", "nos", "nas", "por", "para", "com", "sem", "sob", "sobre", "entre",
			"como", "que", "ad"
		};

		private static readonly HashSet<string> EnConnectives = new(StringComparer.Ordinal)
		{
			"of", "the", "and", "to", "in", "a", "an", "for", "from", "with", "or", "on", "at",
			"by", "as", "into", "upon", "over", "after", "before", "between", "against",
			"without", "within", "toward", "towards", "per", "via", "vs", "that", "is", "not", "ad"
		};

		/// <summary>Noms propres gardés capitalisés (① deck + ⑤ hors deck, arbitrage sur le
		/// précédent EN post-#1601 des mêmes rangées : « Motte and bailey », « Gish gallop »,
		/// « Golem effect » abattus ; « Van Gogh fallacy », « Clever Hans effect »,
		/// « Semmelweis reflex » gardés).</summary>
		private static readonly HashSet<string> ProperNouns = new(StringComparer.Ordinal)
		{
			// ① (deck)
			"Chewbacca", "Christianity", "Hitler", "Hitlerum", "Linda", "Moon", "Morgan's",
			"Flintstones", "Rogers", "Latin",
			// ⑤ (hors deck pt/es)
			"Sherlock", "Holmes", "Gold", "Semmelweis", "Lua", "Luna", "Kafkaiana", "Kafka",
			"França", "Barnum", "Pigmalião", "Pigmalión", "Mateus", "Mateo", "Hans", "Clever",
			"Gish", "V", "Van", "Gogh", "Von", "Restorff", "Woozle", "El", "Greco", "Carthago",
			"Cartago", "Galileu", "Galileo", "Neyman", "Murphy", "Simpson", "Stroop", "Weber",
			"Fechner", "IKEA", "Google", "Pangloss", "Russell", "Will",
			// ⑤ deuxième vague (rangées « mixtes » — connectifs pela/pelos/uma, es « a »)
			"Hanlon", "Morgan", "Morton", "Wobegon", "Picapiedra", "España"
		};

		/// <summary>Casse de phrase (⑤) : tout mot après le premier est minuscule, sauf nom
		/// propre en liste. C'est la post-condition exacte de la transformation du grain.</summary>
		private static List<string> NonSentenceCaseIn(string column, string? csvPath = null)
		{
			var csv = new HarvestCardIdsCsv(csvPath ?? FallaciesCsv);
			var titles = csv.LoadColumn(column);
			var violators = new List<string>();
			foreach (var raw in titles)
			{
				var t = raw.Trim();
				var words = WordRe.Matches(t).Select(m => m.Value).ToList();
				if (words.Count < 2)
					continue;
				if (words.Skip(1).Any(w => char.IsUpper(w[0]) && !ProperNouns.Contains(w)))
					violators.Add(t);
			}
			return violators;
		}

		/// <summary>Casse de titre = tous les mots porteurs capitalisés (connectifs minuscules
		/// admis, « ad » compris — leçon 1373 « Reductio ad Hitlerum », particule latine).</summary>
		private static bool IsTitleCase(string title, HashSet<string> connectives)
		{
			var words = WordRe.Matches(title).Select(m => m.Value).ToList();
			if (words.Count < 2)
				return false;
			var bearers = words.Where(w => !connectives.Contains(w.ToLowerInvariant())).ToList();
			if (bearers.Count == 0)
				return false;
			return bearers.All(w => char.IsUpper(w[0]));
		}

		private static List<string> TitleCaseIn(string column, string? deckColumn,
			HashSet<string> connectives, string? csvPath = null)
		{
			var csv = new HarvestCardIdsCsv(csvPath ?? FallaciesCsv);
			var titles = csv.LoadColumn(column);
			var decks = deckColumn == null ? null : csv.LoadColumn(deckColumn);
			var violators = new List<string>();
			for (var i = 0; i < titles.Count; i++)
			{
				if (decks != null && string.IsNullOrWhiteSpace(decks[i]))
					continue;
				var t = titles[i].Trim();
				if (t.Length > 0 && IsTitleCase(t, connectives))
					violators.Add(t);
			}
			return violators;
		}

		[Fact]
		public void Pt_Deck_Every_Title_IsSentenceCase_OrNamedException()
		{
			var violators = TitleCaseIn("text_pt", "carte", PtConnectives);
			var kept = new[] { "Falácia Nirvana", "Defesa de Chewbacca", "Reductio ad Hitlerum" };
			var unexpected = violators.Where(v => !kept.Contains(v, StringComparer.Ordinal)).ToList();
			unexpected.Should().BeEmpty(
				"① pt direction A : plus aucun titre deck en casse de titre. Exceptions nommées : " +
				"« Falácia Nirvana » (nom établi, décision ai-01), « Defesa de Chewbacca » et " +
				"« Reductio ad Hitlerum » (no-ops — seuls leurs noms propres portent des majuscules ; " +
				"1373 renommé ainsi par ⑥, l'ancien « Cartão Hitler » est sorti avec le renommage).");
			kept.Should().OnlyContain(k => violators.Contains(k, StringComparer.Ordinal),
				"les trois exceptions tenues doivent rester vivantes — une morte rend la garde aveugle.");
		}

		[Fact]
		public void En_Corpus_Every_Title_IsSentenceCase_OrProperNounException()
		{
			var violators = TitleCaseIn("text_en", null, EnConnectives);
			var kept = new[] { "Appeal to the Moon", "Cafeteria Christianity", "Proof by Latin",
				"Clever Linda", "Reductio ad Hitlerum" };
			var unexpected = violators.Where(v => !kept.Contains(v, StringComparer.Ordinal)).ToList();
			unexpected.Should().BeEmpty(
				"① en : fin de la casse de titre sur le corpus (111 titresSophismes + 3 Vertus retournés). " +
				"Exceptions : titres dont seuls des noms propres portent des majuscules.");
			kept.Should().OnlyContain(k => violators.Contains(k, StringComparer.Ordinal),
				"les cinq exceptions tenues restent vivantes (le classeur les voit en casse de titre par construction).");
		}

		[Fact]
		public void Pt_Samples_Applied()
		{
			TitlePt("3.1.1").Should().Be("Viés de amostragem",
				"① sens + casse : « Sesgo » est espagnol, le portugais dit « Viés » (décision ai-01).");
			TitlePt("5").Should().Be("Abuso da linguagem", "① pt : tête de famille 798.");
			TitlePt("1.1.3").Should().Be("Resgate ad hoc", "① : locution latine en casse de phrase.");
			TitlePt("3.1").Should().Be("Generalização abusiva", "① : tête deck 595.");
			TitlePt("1.2.3.3.2").Should().Be("Correção política", "① : 121 deck.");
			TitlePt("1.1.2").Should().Be("Justificação trivial", "① : 33 deck.");
			TitlePt("3.2.2").Should().Be("Falácia probabilística", "① : 644 deck.");
			TitlePt("2.3.2").Should().Be("Jogos de poder", "① : 420 deck.");
		}

		[Fact]
		public void Es_TwoDecisions_Applied_And_Nirvana_Kept()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			CellOf(csv, "3.2.2", "text_es").Should().Be("Probabilidades falseadas",
				"① es décision : « Probabilidades Falseadas » → casse de phrase (644).");
			CellOf(csv, "5", "text_es").Should().Be("Abuso del lenguaje",
				"① es décision : tête de famille 798, 88 bandeaux suivent.");
			CellOf(csv, "6.2.1.1.1.1", "text_es").Should().Be("Falacia del Nirvana",
				"① gardé : nom établi du sophisme en espagnol (décision ai-01, Wikipédia/Demsetz).");
		}

		[Fact]
		public void En_Samples_Applied()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			CellOf(csv, "1.3.2.1", "text_en").Should().Be("Fallacist's fallacy", "① : 154 deck.");
			CellOf(csv, "6.2.1.1.1.1", "text_en").Should().Be("Nirvana fallacy",
				"① : « Nirvana Fallacy » → casse de phrase (Nirvana reste capitalisé, décision explicite).");
			CellOf(csv, "1.2.1.3.3.1.1", "text_en").Should().Be("Cafeteria Christianity",
				"① : coquille « Cafetaria » corrigée ; « Christianity » garde sa majuscule (nom propre, décision).");
			CellOf(csv, "1.2.1.3.3", "text_en").Should().Be("Ipse dixit", "① : locution latine.");
			CellOf(csv, "3.2.1.2", "text_en").Should().Be("Post hoc fallacy", "① : 635.");
			CellOf(csv, "7.3.2.3.2", "text_en").Should().Be("Ad fidentia", "① : locution latine (décision).");
			CellOf(csv, "1.3.3.3", "text_en").Should().Be("Violating Morgan's canon",
				"① : « Morgan's » nom propre gardé, « canon » commun décapsulé.");
			CellOf(csv, "1.3.1.3.1.1.1", "text_en").Should().Be("The Flintstones fallacy",
				"① : « Flintstones » nom propre gardé.");
			CellOf(csv, "2.3.1.1.1.2", "text_en").Should().Be("Bait and switch", "① : 361 deck.");
			CellOf(csv, "2.3.1.1.1.2.1", "text_en").Should().Be("Praise sandwich", "① : 362 deck.");
			CellOf(csv, "7.3.2.1.1", "text_en").Should().Be("Reductio ad Hitlerum",
				"① no-op tenu : déjà en casse de phrase (« ad » minuscule, « Hitlerum » nom propre).");
		}

		[Fact]
		public void En_Virtues_ThreeTitles_Applied()
		{
			var csv = new HarvestCardIdsCsv(VirtuesCsv);
			CellOf(csv, "4.3.3.1", "title_en").Should().Be("Formal validity", "① Vertus 94.");
			CellOf(csv, "4.3.3.1.1.8", "title_en").Should().Be("Inference: resolution", "① Vertus 103.");
			CellOf(csv, "7.3.1", "title_en").Should().Be("Fair evaluation of the opposing position",
				"① Vertus 208 — ses bandeaux suivent (subsubfamily_en).");
			CellOf(csv, "7.3.1", "subsubfamily_en").Should().Be("Fair evaluation of the opposing position",
				"① : self-band de 208 suit le titre (leçon #1588).");
		}

		[Fact]
		public void RemovedForms_Absent_From_Titles_And_Bands()
		{
			var fallacies = new HarvestCardIdsCsv(FallaciesCsv);
			var removed = new Dictionary<string, string[]>
			{
				["text_pt|Family_pt|Subfamily_pt|Subsubfamily_pt"] = new[] {
					"Sesgo de Amostragem", "Abuso da Linguagem", "Justificação Trivial",
					"Generalização Abusiva", "Conclusão Precipitada", "Falsa Equivalência",
					"Homem de Palha", "Falácia Probabilística" },
				["text_es|Family_es|Subfamily_es|Subsubfamily_es"] = new[] {
					"Probabilidades Falseadas", "Abuso del Lenguaje" },
				["text_en|Family|Subfamily|Subsubfamily"] = new[] {
					"Nirvana Fallacy", "Fallacist's Fallacy", "Sunk Cost Fallacy",
					"Bait and Switch", "Praise Sandwich", "Ipse Dixit", "Ad Fidentia",
					"Post Hoc Fallacy", "Cafetaria" },
			};
			foreach (var (columns, forms) in removed)
				foreach (var col in columns.Split('|'))
				{
					var values = fallacies.LoadColumn(col);
					foreach (var form in forms)
						values.Count(v => string.Equals(v.Trim(), form, StringComparison.Ordinal)).Should().Be(0,
							$"la forme retirée «{form}» ne doit plus exister en {col} — les bandeaux ont suivi (①).");
				}
			var virtues = new HarvestCardIdsCsv(VirtuesCsv);
			foreach (var col in new[] { "title_en", "family_en", "subfamily_en", "subsubfamily_en" })
			{
				var values = virtues.LoadColumn(col);
				foreach (var form in new[] { "Formal Validity", "Inference: Resolution",
					"Fair Evaluation of the Opposing Position" })
					values.Count(v => string.Equals(v.Trim(), form, StringComparison.Ordinal)).Should().Be(0,
						$"la forme retirée «{form}» ne doit plus exister en {col} (Vertus, ①).");
			}
		}

			[Fact]
		public void Pt_Corpus_Every_Title_IsSentenceCase_OrNamedException()
		{
			var violators = NonSentenceCaseIn("text_pt");
			var kept = new[] { "Falácia Nirvana" };
			var unexpected = violators.Where(v => !kept.Contains(v, StringComparer.Ordinal)).ToList();
			unexpected.Should().BeEmpty(
				"⑤ pt : la casse de phrase couvre désormais le corpus ENTIER (deck ① + hors deck ⑤, " +
				"423 titres retournés). Seule exception nommée : « Falácia Nirvana » (nom établi du " +
				"sophisme, décision ai-01 #1600). Les noms propres (Sherlock Holmes, Gold, Semmelweis, " +
				"Van Gogh, Von Restorff…) restent capitalisés par construction du détecteur.");
			kept.Should().OnlyContain(k => violators.Contains(k, StringComparer.Ordinal),
				"l'exception tenue doit rester vivante — une morte rend la garde aveugle.");
		}

		[Fact]
		public void Es_Corpus_Every_Title_IsSentenceCase_OrNamedException()
		{
			var violators = NonSentenceCaseIn("text_es");
			var kept = new[] { "Falacia del Nirvana" };
			var unexpected = violators.Where(v => !kept.Contains(v, StringComparer.Ordinal)).ToList();
			unexpected.Should().BeEmpty(
				"⑤ es : la casse de phrase couvre le corpus entier (2 décisions ① + 25 titres hors " +
				"deck ⑤). Seule exception nommée : « Falacia del Nirvana » (nom établi, décision #1600). " +
				"L'ancienne exception 796 « Quaternio Terminorum (…) » est sortie par ⑥ : le titre est " +
				"devenu « Falacia de los cuatro términos », en casse de phrase.");
			kept.Should().OnlyContain(k => violators.Contains(k, StringComparer.Ordinal),
				"l'exception tenue doit rester vivante — une morte rend la garde aveugle.");
		}

		[Fact]
		public void Pt_Es_HorsDeck_Samples_Applied()
		{
			TitlePt("1.1.3.5.1").Should().Be("História ad hoc", "⑤ : locution latine (69).");
			TitlePt("1.2.1.3.3").Should().Be("Ipse dixit", "⑤ : locution latine (93).");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "2.3.2.3.2.2.1", "text_pt").Should().Be("Gish gallop",
				"⑤ : « Gish Gallop » suit le précédent EN « Gish gallop » (475 ; jumeaux 772/1331).");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "2.3.1.3.2.2", "text_pt").Should().Be("Efeito golem",
				"⑤ : « Golem effect » EN — golem est un nom commun (400).");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "6.3.2.3.1.4", "text_pt").Should().Be("Efeito halo",
				"⑤ : « Halo effect » EN — halo est un nom commun (1231).");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "2.3.1.1.1.4", "text_pt").Should().Be("Falácia da motte e bailey",
				"⑤ : « Motte and bailey » EN — terme de château commun, les deux abattus (364).");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "2.2.1.2.2.1", "text_pt").Should().Be("Apelo à “França que acorde cedo”",
				"⑤ : pays capitalisé, slogan en casse de phrase (306).");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "2.3.1.5.4", "text_pt").Should().Be("Efeito Hans, o inteligente",
				"⑤ : « Hans » nom propre gardé, adjectif abattu (412).");
			// no-ops tenus (noms propres) — la garde épingle aussi les non-gestes
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "1.1.2.2.4.1", "text_pt").Should().Be("Efeito Gold",
				"⑤ no-op tenu : « Gold » nom propre (49).");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "4.1.2.1.1", "text_pt").Should().Be("Falácia de Van Gogh",
				"⑤ no-op tenu : « Van Gogh » nom propre (709).");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "1.2.2.4.3", "text_es").Should().Be("El noble salvaje",
				"⑤ es : 111.");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "2.3.1.1.1.4", "text_es").Should().Be("Sofisma de motte y bailey",
				"⑤ es : 364.");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "2.3.2.3.1.1.1", "text_es").Should().Be("Efecto gaslight",
				"⑤ es : 459 — gaslighting est un commun.");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "2.3.1.5.4", "text_es").Should().Be("Efecto Clever Hans",
				"⑤ es no-op tenu : « Clever Hans » est le nom du cheval (412).");
			CellOf(new HarvestCardIdsCsv(FallaciesCsv), "7.1.3.3.2", "text_es").Should().Be("Delenda Carthago",
				"⑤ es no-op tenu : « Carthago » nom propre latin (1303).");
		}

		[Fact]
		public void Removed_G5_Forms_Absent_From_Titles_And_Bands()
		{
			var fallacies = new HarvestCardIdsCsv(FallaciesCsv);
			var removed = new Dictionary<string, string[]>
			{
				["text_pt|Family_pt|Subfamily_pt|Subsubfamily_pt"] = new[] {
					"Gish Gallop", "Efeito Halo", "Efeito Golem", "Efeito Placebo", "Efeito Nocebo",
					"Efeito Gaslight", "História Ad Hoc", "Ipse Dixit", "Falácia Post Hoc",
					"Teologia Fast-Food", "Efeito Swish", "Jogada de Pathos" },
				["text_es|Family_es|Subfamily_es|Subsubfamily_es"] = new[] {
					"El Noble Salvaje", "Efecto Gaslight", "Efecto Golem",
					"Sofisma de Motte y Bailey", "Correlación Ilusoria", "Tácticas de Presión" },
			};
			foreach (var (columns, forms) in removed)
				foreach (var col in columns.Split('|'))
				{
					var values = fallacies.LoadColumn(col);
					foreach (var form in forms)
						values.Count(v => string.Equals(v.Trim(), form, StringComparison.Ordinal)).Should().Be(0,
							$"la forme retirée «{form}» ne doit plus exister en {col} (⑤).");
				}
			var virtues = new HarvestCardIdsCsv(VirtuesCsv);
			foreach (var col in new[] { "title_pt", "family_pt", "subfamily_pt", "subsubfamily_pt",
				"title_es", "family_es", "subfamily_es", "subsubfamily_es" })
			{
				var values = virtues.LoadColumn(col);
				foreach (var form in new[] { "Inferência: Resolução", "Inferencia: Resolución" })
					values.Count(v => string.Equals(v.Trim(), form, StringComparison.Ordinal)).Should().Be(0,
						$"la forme retirée «{form}» ne doit plus exister en {col} (⑤ Vertus 103).");
			}
		}

		[Fact]
		public void Virtues_103_SentenceCase_Applied()
		{
			var csv = new HarvestCardIdsCsv(VirtuesCsv);
			CellOf(csv, "4.3.3.1.1.8", "title_pt").Should().Be("Inferência: resolução",
				"⑤ Vertus 103 pt : aligné sur l'anglais « Inference: resolution » (①).");
			CellOf(csv, "4.3.3.1.1.8", "title_es").Should().Be("Inferencia: resolución",
				"⑤ Vertus 103 es : aligné sur l'anglais « Inference: resolution » (①).");
		}

		[Fact]
		public void Fidelity_G6_Renames_Applied()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			CellOf(csv, "3.1.2", "text_es").Should().Be("Falacia del accidente",
				"⑥ es 614 : « Accidente » → locution complète (arbitrage ai-01 #458 c.5856549779).");
			CellOf(csv, "6.1.1", "text_es").Should().Be("Mentira",
				"⑥ es 889 : le substantif espagnol est « Mentira », pas l'infinitif « Mentir ».");
			CellOf(csv, "6.2.3.3", "text_es").Should().Be("Argumento del esfuerzo notable",
				"⑥ es 1015 : aligné sur le fr « Argument de l'effort notable ».");
			CellOf(csv, "4.3.3.3.1", "text_es").Should().Be("Falacia de los cuatro términos",
				"⑥ es 796 : le titre long latinisant laisse place au nom espagnol établi.");
			CellOf(csv, "4", "text_pt").Should().Be("Erro de raciocínio",
				"⑥ pt 696 : tête du sous-arbre 4 — « Lógicas defeituosas » était un calque, 101 bandeaux suivent.");
			CellOf(csv, "3.3.1.3.1", "text_pt").Should().Be("Ladeira escorregadia",
				"⑥ pt 677 : rejoint son jumeau hors deck 706 qui portait déjà ce nom (ai-01).");
			CellOf(csv, "4.3.3.3.1", "text_pt").Should().Be("Falácia dos quatro termos",
				"⑥ pt 796 : coquille « quatros » → « quatro » + alignement sur le nom établi.");
			CellOf(csv, "7.3.2.1.1", "text_pt").Should().Be("Reductio ad Hitlerum",
				"⑥ pt 1373 : « Cartão Hitler » → nom latin consacré, comme fr/en/ru.");
			CellOf(csv, "6.2.2.5.1", "text_pt").Should().Be("Argumento após contestação",
				"⑥ pt 1005 : « Alterar os postes da baliza » décrivait un autre sophisme (le déplacement " +
				"de buts = 973) ; le fr dit « Argument après contestation ».");
			CellOf(csv, "6.2.2.5.1", "text_ru").Should().Be("Аргумент после опровержения",
				"⑥ ru 1005 : proposition po-2024 validée par ai-01 — « argument après réfutation », " +
				"sans collision avec « Изменение направления » (973) ni « Спасение ad hoc » (55).");
			CellOf(csv, "6.2.2.5.1", "text_zh").Should().Be("被驳后改口",
				"⑥ zh 1005 : proposition po-2024 validée par ai-01 — « changer de discours après avoir " +
				"été réfuté », sans collision avec « 移动目标 » (973) ni « 特殊辩护 » (55).");
			CellOf(new HarvestCardIdsCsv(VirtuesCsv), "6.3.3.1", "title_en").Should().Be(
				"Recognition of personal ideological biases",
				"⑥ Vertus 175 en : la desc_en de la rangée dit elle-même « Recognition » (dossier ④).");
		}

		[Fact]
		public void Fidelity_G6_Bands_Followed()
		{
			var csv = new HarvestCardIdsCsv(FallaciesCsv);
			Action<string, string, int, string> count = (column, value, expected, because) =>
				csv.LoadColumn(column).Count(v => string.Equals(v.Trim(), value, StringComparison.Ordinal))
					.Should().Be(expected, because);
			count("Family_pt", "Erro de raciocínio", 102,
				"⑥ : les 102 bandeaux Family_pt du sous-arbre 4 suivent le titre renommé de 696 (self compris).");
			count("Subsubfamily_es", "Mentira", 53,
				"⑥ : les 53 bandeaux Subsubfamily_es du sous-arbre 6.1.1 suivent « Mentira » (self compris).");
			count("Subsubfamily_es", "Falacia del accidente", 7,
				"⑥ : les 7 bandeaux Subsubfamily_es du sous-arbre 3.1.2 suivent « Falacia del accidente ».");
			CellOf(csv, "4", "Family_pt").Should().Be("Erro de raciocínio",
				"⑥ : self-band de 696 suit son titre (règle #1588).");
			CellOf(csv, "6.1.1", "Subsubfamily_es").Should().Be("Mentira",
				"⑥ : self-band de 889 suit son titre.");
			CellOf(csv, "3.1.2", "Subsubfamily_es").Should().Be("Falacia del accidente",
				"⑥ : self-band de 614 suit son titre.");
		}

		[Fact]
		public void Removed_G6_Forms_Absent_From_Titles_And_Bands()
		{
			var fallacies = new HarvestCardIdsCsv(FallaciesCsv);
			var removed = new Dictionary<string, string[]>
			{
				["text_pt|Family_pt|Subfamily_pt|Subsubfamily_pt"] = new[] {
					"Lógicas defeituosas", "Pente fino", "Falácia dos quatros termos",
					"Cartão Hitler", "Alterar os postes da baliza" },
				["text_es|Family_es|Subfamily_es|Subsubfamily_es"] = new[] {
					"Accidente", "Mentir", "Esfuerzo notable",
					"Quaternio Terminorum (Falacia de los Cuatro Términos)" },
				["text_ru|Family_ru|Subfamily_ru|Subsubfamily_ru"] = new[] { "Перемещение ворот" },
				["text_zh|Family_zh|Subfamily_zh|Subsubfamily_zh"] = new[] { "移动界标" },
			};
			foreach (var (columns, forms) in removed)
				foreach (var col in columns.Split('|'))
				{
					var values = fallacies.LoadColumn(col);
					foreach (var form in forms)
						values.Count(v => string.Equals(v.Trim(), form, StringComparison.Ordinal)).Should().Be(0,
							$"la forme retirée «{form}» ne doit plus exister en {col} (⑥).");
				}
			var virtues = new HarvestCardIdsCsv(VirtuesCsv);
			foreach (var col in new[] { "title_en", "family_en", "subfamily_en", "subsubfamily_en" })
			{
				var values = virtues.LoadColumn(col);
				values.Count(v => string.Equals(v.Trim(), "Consideration of one's ideological biases", StringComparison.Ordinal))
					.Should().Be(0, "la forme retirée ne doit plus exister en " + col + " (⑥ Vertus 175).");
			}
		}

		private static string TitlePt(string path)
			=> CellOf(new HarvestCardIdsCsv(FallaciesCsv), path, "text_pt");

		private static string CellOf(HarvestCardIdsCsv csv, string path, string column)
		{
			var values = csv.LoadColumn(column, "path", new[] { path });
			values.Should().HaveCount(1, "une seule rangée porte ce path.");
			return values[0].Trim();
		}
	}
}
