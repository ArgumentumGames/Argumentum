using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// File profonde c.5993735448 grain 2 (3a) — les 11 cellules EN imprimees corrigees
	/// selon le GO owner du 05/10 (c.5990803984 : « oui a tout », forme choisie dans la PR,
	/// une raison par cellule), dossier
	/// <c>docs/translation/458-soumission-owner-texte-imprime-2026-10-05.md</c> (§2, §«Ecrit»).
	/// <para>12 cellules EN + 3 siblings ru par la regle du GO : « si une langue jamais imprimee
	/// reproduit le meme defaut que l'EN (un nom a la place d'un role, un calque, le titre laisse
	/// en francais...), on la corrige dans la meme PR, puisque c'est la meme rangee ».
	/// Les siblings NON defectueux restent en l'etat et sont epingles comme observations
	/// (pt «A quermesse» = mot portugais assimile, pas du francais oublie ; fa/zh/ar 5.2.1
	/// ne peuvent structurellement pas porter le defaut du nom divin sans article ni marquage
	/// nominal). Chirurgie par span de champ : 15 cellules exactement, re-parse complet,
	/// delta +36 octets, 167 enregistrements, pas de BOM.</para>
	/// </summary>
	public class ScenariiEnPrintedCorrectionsGuardTests
	{
		private static string ScenariiCsv => System.IO.Path.Combine(
			TestRepoRoot.Find(), "Cards", "Scenarii", "Argumentum Scenarii - Cards.csv");

		/// <summary>Les 15 cellules ecrites : rangee, colonne, valeur pleine attendue, valeur d'avant, la raison.</summary>
		private static readonly (string Path, string Column, string Expected, string Was, string Why)[] Restored =
		{
			("1.3.2", "smoothTalker", "The President of the United States", "Truman", "le titre dit President Truman and the A-Bomb : le baratineur EST le president — un nom propre nu a la place du role (dossier §2.1)"),
			("2.2.5", "smoothTalker", "A family court judge", "Salomon", "le FR joue «Salomon» comme ROLE (le juge sage) ; l'EN imprimait le nom propre nu — la recommandation du dossier rend le role (§2.2)"),
			("2.3.5", "drawer", "the specter", "the statue of the governor", "calque de «la statue du gouverneur» ; le FR dit «le spectre» (§2.3)"),
			("7.3.2", "drawer", "A parent", "A relative", "«A relative» generique ; le FR dit «Un parent» et la famille 7.3.x pince le personnage par role (§2.4)"),
			("7.1.5", "title", "The Fair", "Kermesse", "«Kermesse» n'est pas un mot anglais ; «The Fair» est le mot que le context/suggestion de la carte utilisent deja (contexte et enjeu traduisent deja fair) (§2.5)"),
			("3.1.6", "drawer", "A person met online", "A person I met online", "calque de sous-phrase relative ; les drawer du corpus sont des NU nus, pas des propositions (§2.6)"),
			("5.1.2", "smoothTalker", "the Smurfiest", "the smurf", "le jeu de la carte est le superlatif (le PLUS schtroumpf des schtroumpfs) ; «the smurf» levelle le nom generique ; FR «Le shtroumphissime» (§2.7)"),
			("5.1.2", "title", "The coup d'État", "The coup d'etat", "accent grave manquant sur Etat — typographie francaise correcte, meme rangee que le superlatif (dossier §2.8, incluse ici)"),
			("4.1.1", "title", "Showing off", "Rolling mechanics", "«Rolling mechanics» est le calque MOT-A-MOT de «Rouler des mecaniques» ; l'idiome anglais est showing off (§2.10)"),
			("4.3.1", "suggestion_en", "My God, I'm seeing things!", "My God, I'm so dizzy!", "la carte parle d'hallucinations (voir des choses), pas de vertige ; FR «la berlue» (§2.11)"),
			("7.1.2", "suggestion_en", "So? Why did you push him?", "What's up? Why did you push him?", "«What's up?» registre petit-copain hors ton du corpus ; FR «Alors ?», neutre — «So?» tient l'agacement (§2.13)"),
			("5.2.1", "suggestion_en", "Then I will bring down the arm of terrible anger, of furious and fearful vengeance on the ungodly hordes that chase and destroy the sheep of God. And you will know why my name is the Lord when the vengeance of the Almighty will fall upon you!", "Then I will bring down the arm of terrible anger, of furious and fearful vengeance on the ungodly hordes that chase and destroy the sheep of God. And you will know why my name is eternal when the vengeance of the Almighty will fall upon you!", "la formule biblique dit que le NOM divin EST le Seigneur ; «my name is eternal» transforme le nom en adjectif ; FR «mon nom est l'Éternel» (§2.14)"),
			// siblings ru — regle du GO : meme defaut, meme rangee, meme PR
			("7.1.5", "title_ru", "Школьный праздник", "Праздник", "generique («fete») — le meme defaut que l'EN «Kermesse» ; le context_ru dit «праздник своего ребёнка», l'es dit deja «Fiesta escolar»"),
			("5.1.2", "smoothTalker_ru", "Смурфейший", "Смурфик-бунтарь", "«Смурфик-бунтарь» (schtroumpf rebelle) forge un AUTRE personnage ; le superlatif russe. RELECTURE ai-01 (c.6027007896, 07/10) : -ейш- se colle au theme смурф- sans voyelle de liaison («основа + -ейш-») — la forme au -и- est retiree, «Смурфейший» ecrite"),
			("5.2.1", "suggestion_ru", "И поражу я дланью ужасного гнева и яростной мести бесчестные орды тех, кто истребляет овец Божиих. И познаешь ты, почему Имя мое — Вечный, когда и на тебя обрушится возмездие Всевышнего.", "И поражу я дланью ужасного гнева и яростной мести бесчестные орды тех, кто истребляет овец Божиих. И познаешь ты, почему Имя мое вечно, когда и на тебя обрушится возмездие Всевышнего.", "«Имя мое вечно» = le meme defaut que l'EN : le nom divin rendu comme adjectif ; «Имя мое — Вечный» rend le NOM"),
		};

		/// <summary>
		/// Formes anciennes ERADIQUEES du corpus entier (compte 0 exigé). Chacune etait
		/// presente au moins une fois avant la chirurgie (assert cur==old du script de span).
		/// «Truman» et «Salomon» en sont EXCLUS : les titres/contextes legitimes les gardent
		/// (Truman x8 dans le titre et le contexte de 1.3.2 ; Salomon x2 = le titre FR et le
		/// title EN de 2.2.5, la carte ETANT le jugement de Salomon) — c'est l'epingle
		/// pleine-cellule du smoothTalker qui couvre le nom-a-la-place-du-role.
		/// </summary>
		private static readonly (string Form, string Where)[] Eradicated =
		{
			("the statue of the governor", "2.3.5.drawer — le calque"),
			("A relative", "7.3.2.drawer — le generique"),
			("Kermesse", "7.1.5.title — le mot francais dans un titre EN"),
			("A person I met online", "3.1.6.drawer — la sous-phrase relative"),
			("the smurf", "5.1.2.smoothTalker — le nom generique (distinguer de Papa Smurf, casse differente)"),
			("The coup d'etat", "5.1.2.title — l'etat sans accent (l'accent etait le seul delta)"),
			("Rolling mechanics", "4.1.1.title — le calque mot-a-mot"),
			("I'm so dizzy", "4.3.1.suggestion_en — le vertige"),
			("What's up?", "7.1.2.suggestion_en — le registre familier"),
			("my name is eternal", "5.2.1.suggestion_en — le nom divin comme adjectif"),
			("Смурфик-бунтарь", "5.1.2.smoothTalker_ru — le personnage forge"),
			("Смурфиейший", "5.1.2.smoothTalker_ru — la forme au -и- epenthetique sans base morphologique, retiree en relecture ai-01 (c.6027007896)"),
			("Имя мое вечно", "5.2.1.suggestion_ru — le nom divin comme adjectif"),
		};

		/// <summary>
		/// Siblings non defectueux, EPINGLES pour qu'un balayage futur ne les « corrige » pas a tort :
		/// «A quermesse» est un mot portugais assimile (dictionnaire pt), PAS du francais oublie.
		/// fa/zh/ar 5.2.1 : le defaut du nom divin (article + copule nominale) n'y est pas
		/// reproductible structurellement — observation consignee, aucune epingle de valeur.
		/// </summary>
		private static readonly (string Path, string Column, string Current)[] SiblingObservations =
		{
			("7.1.5", "title_pt", "A quermesse"),
		};

		internal static List<string> Mismatches(
			IEnumerable<(string Path, string Column, string Expected, string Actual)> cells)
		{
			var offenders = new List<string>();
			foreach (var (path, column, expected, actual) in cells)
				if (!string.Equals(expected, actual, StringComparison.Ordinal))
					offenders.Add($"{path}.{column} : attendu «{Truncate(expected)}», lu «{Truncate(actual)}»");
			return offenders;
		}

		private static string Truncate(string s) => s.Length <= 60 ? s : s.Substring(0, 57) + "…";

		private static List<(string, string, string, string)> ReadPinned()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var read = new List<(string, string, string, string)>();
			foreach (var (path, column, expected, _, _) in Restored)
			{
				var v = csv.LoadColumn(column, "path", new[] { path });
				v.Should().HaveCount(1, $"la rangee {path} existe et est unique.");
				read.Add((path, column, expected, v[0]));
			}
			return read;
		}

		[Fact]
		public void Restored_Cells_Carry_The_Owner_Approved_Corrections()
		{
			Mismatches(ReadPinned()).Should().BeEmpty(
				"#458 grain 3a : les 11 cellules EN du GO owner (c.5990803984) et les 3 siblings ru " +
				"defectueuses sont ecrites, valeur pleine, une raison par cellule.");
		}

		[Fact]
		public void Old_Forms_Are_Eradicated_From_The_Corpus()
		{
			var corpus = System.IO.File.ReadAllText(ScenariiCsv);
			var offenders = Eradicated
				.Where(e => corpus.Contains(e.Form, StringComparison.Ordinal))
				.Select(e => $"«{e.Form}» encore present ({e.Where})")
				.ToList();
			offenders.Should().BeEmpty(
				"les formes anciennes sont eradiquees du corpus entier, pas seulement de leur cellule — " +
				"sauf «Truman», exclu a dessein (titre et contexte de 1.3.2 le gardent).");
		}

		[Fact]
		public void Restored_Table_Is_Not_Vacuous()
		{
			Restored.Should().HaveCount(15, "12 EN (11 cellules owner + le coup d'Etat, meme rangee que le superlatif) + 3 siblings ru.");
			Restored.Select(r => (r.Path, r.Column)).Distinct().Should().HaveCount(15, "15 cellules distinctes.");
			Restored.Select(r => r.Path).Distinct().Should().HaveCount(11, "les 11 rangees du GO.");
			Restored.Count(r => !r.Column.EndsWith("_ru")).Should().Be(12, "12 cellules EN.");
			Restored.Count(r => r.Column.EndsWith("_ru")).Should().Be(3, "3 siblings ru (regle du GO).");
			Restored.Should().OnlyContain(r => r.Was.Length > 0 && r.Why.Length > 20, "chaque epingle porte son avant et sa raison.");
			Restored.Select(r => r.Why).Distinct().Should().HaveCount(15, "15 raisons distinctes, pas un copier-coller.");
			Eradicated.Should().HaveCount(13);
			Eradicated.Select(e => e.Form).Distinct().Should().HaveCount(13);
		}

		[Fact]
		public void Sibling_Observations_Stay_Pinned()
		{
			var csv = new HarvestCardIdsCsv(ScenariiCsv);
			var offenders = new List<string>();
			foreach (var (path, column, current) in SiblingObservations)
			{
				var v = csv.LoadColumn(column, "path", new[] { path });
				v.Should().HaveCount(1);
				if (!string.Equals(current, v[0], StringComparison.Ordinal))
					offenders.Add($"{path}.{column} : «A quermesse» est un mot pt assimile — si cette cellule a change, c'est qu'un balayage l'a « corrigee » a tort.");
			}
			offenders.Should().BeEmpty(
				"les siblings non defectueuses ne bougent pas : la regle du GO corrige le memE defaut, pas le voisinage.");
		}

		[Fact]
		public void Detector_Fires_On_The_Pre_Correction_Value()
		{
			var reverted = new[]
			{
				("5.1.2", "title", Restored[7].Expected, "The coup d'etat"),
				("7.1.5", "title_ru", Restored[12].Expected, "Праздник"),
				("2.2.5", "smoothTalker", Restored[1].Expected, Restored[1].Expected),
			};
			var offenders = Mismatches(reverted);
			offenders.Should().HaveCount(2, "seules les cellules revertes doivent rougir : le temoin sain reste vert.");
			offenders.Should().Contain(s => s.Contains("5.1.2.title"));
			offenders.Should().Contain(s => s.Contains("7.1.5.title_ru"));
		}
	}
}
