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

namespace Argumentum.AssetConverter.Tests.DatasetUpdater
{
	/// <summary>
	/// Constructions pt-PT bannies des colonnes _pt (#458 poches ②④, décision ai-01 10/10 :
	/// passe de correction différentielle, « estar a + infinitif » et lexical isolé). Le corpus
	/// cible le pt-BR depuis la décision du 14/08 — la poche ① (factos → fatos, #1871) a réglé
	/// l'orthographe ; celle-ci règle la syntaxe et le lexical.
	///
	/// Le grain converti 27 occurrences (2026-10-14) : 19 « estar a + INF » → « estar +
	/// gerúndio » (avec proclise BR le cas échéant : está se tornando, estão se queixando), 1
	/// clitique de perception (« vi-o a aproveitar » → « vi-o aproveitando »), 7 lexical
	/// (camião/camiões → caminhão/caminhões avec accord, ecrã → tela avec le genre — do meu →
	/// da minha —, contactos → contatos, desporto → esporte ×2, pequeno-almoço → café da
	/// manhã, receção → recepção). Contrôle après : tiers A=0, B = les 16 vós maintenus, C =
	/// les 2 dossiê maintenus ; témoins BR 1510 → 1515 (ils montent).
	///
	/// HORS PÉRIMÈTRE (déclaré, jamais pincé par cette garde) : « vós/vosso » = registre, pas
	/// dialecte — différé au grain « registre pt/es » (la voix divine Scenarii 5.2.1 garde son
	/// vós, choix stylistique ai-01) ; « dossiê » = employé par les deux variantes (tier C).
	/// Les pt-PT NON couverts par l'instrument #1858 (golo, guarda-redes, Tenciona…) sont
	/// signalés au corps de la PR comme candidats d'extension de l'instrument, pas corrigés.
	///
	/// Falsifiabilité : rouge sur l'arbre pré-grain (19 estar-a + lexical) ; ré-injecter
	/// « estás a ouvir » dans une cellule _pt pince le bannissement ; les littéraux
	/// synthétiques prourent que le détecteur voit un 1 (un 0 n'est une absence que si
	/// l'instrument pouvait voir un 1).
	/// </summary>
	public class PtBrazilianConstructionTests
	{
		private static readonly (string relativePath, string name)[] TargetFiles =
		{
			(Path.Combine("Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv"), "fallacies"),
			(Path.Combine("Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv"), "virtues"),
			(Path.Combine("Cards", "Scenarii", "Argumentum Scenarii - Cards.csv"), "scenarii"),
			(Path.Combine("Cards", "Rules", "Argumentum Rules - Cards.csv"), "rules"),
			(Path.Combine("Cards", "Rules", "Argumentum Rules - Cards Print and Play.csv"), "rules-pp"),
		};

		/// <summary>
		/// Tier B syntaxique : « estar a + infinitif », forme CONJUGUÉE (estou/estás/está/estão/
		/// estava/estivesse…). Recopié à l'identique du scanner #1858 — la première version de
		/// cette garde ne portait que « estar » littéral et était AVEUGLE aux conjugués
		/// (« estás a ouvir » invisible) : le témoin synthétique l'a pincée avant livraison.
		/// L'exemption « a partir » (préposition figée) est conservée.
		/// </summary>
		private static readonly Regex EstarConjugaAInfinitivo = new(
			@"\best(?:ou|á|ás|amos|ão|ava|avas|ávamos|avam|arei|arás|ará|aremos|arão|eja|ejas|ejamos|ejam|ivesse|ivessem|iver|ivermos|iverem)\s+a\s+(?!partir\b)\w+",
			RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>Tier B syntaxique, forme INFINITIVE après modal (« devem estar a »).</summary>
		private static readonly Regex EstarInfinitivoAInfinitivo = new(
			@"\bestar\s+a\s+(?!partir\b)\w+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private static readonly Regex CliticoMaisAInfinitivo = new(
			@"\b(?:vi|via|viu|vejo|vemos|ouvi|ouviu|ouviram|encontrei|encontrou|apanhou|apanhei|deparou|surpreendi)-?(?:o|a|os|as|lo|la|los|las|no|na|nos|nas)\s+a\s+\w+r\b",
			RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>
		/// Tier A lexical : les mots qui n'existent pas au Brésil. Chaque entrée est la forme
		/// continentale bannie avec son équivalent BR (le grain les a convertis ; le bannissement
		/// vaut pour tout retour, future traduction comprise).
		/// </summary>
		private static readonly (Regex pattern, string banned, string brazilian)[] LexicalContinental =
		{
			(new Regex(@"\bcami(?:ão|ões)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "camião", "caminhão"),
			(new Regex(@"\btelem[óo]ve(?:l|is|es)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "telemóvel", "celular"),
			(new Regex(@"\bautocarros?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "autocarro", "ônibus"),
			(new Regex(@"\bcomboios?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "comboio", "trem"),
			(new Regex(@"\bpequeno-almo(ç|c)o\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "pequeno-almoço", "café da manhã"),
			(new Regex(@"\becr[ãa]s?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "ecrã", "tela"),
			(new Regex(@"\bficheiros?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "ficheiro", "arquivo"),
			(new Regex(@"\butilizador(?:es)?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "utilizador", "usuário"),
			(new Regex(@"\bequipas?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "equipa", "equipe"),
			(new Regex(@"\bcasa de banho\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "casa de banho", "banheiro"),
			(new Regex(@"\bdesportos?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "desporto", "esporte"),
			// tier B lexical
			(new Regex(@"\bsec[çc][ãa]o\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "secção", "seção"),
			(new Regex(@"\brece[çc][ãa]o\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "receção", "recepção"),
			(new Regex(@"\bcontactos?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "contacto", "contato"),
			(new Regex(@"\bplaneamento\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "planeamento", "planejamento"),
		};

		private static string RepoRoot() => TestRepoRoot.Find();

		private static List<(string file, string pk, string column, string cell)> LoadPtCells()
		{
			var cells = new List<(string, string, string, string)>();
			foreach (var (relativePath, name) in TargetFiles)
			{
				var path = Path.Combine(RepoRoot(), relativePath);
				File.Exists(path).Should().BeTrue("the corpus CSV must exist at '{0}'", relativePath);

				var config = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					MissingFieldFound = null,
					HeaderValidated = null,
				};
				using var reader = new StreamReader(path);
				using var csv = new CsvReader(reader, config);
				csv.Read();
				csv.ReadHeader();
				var headers = csv.HeaderRecord ?? Array.Empty<string>();
				var ptIndexes = headers
					.Select((h, i) => (h, i))
					.Where(t => t.h.EndsWith("_pt", StringComparison.Ordinal))
					.Select(t => t.i)
					.ToArray();
				ptIndexes.Should().NotBeEmpty("'{0}' must expose at least one _pt column", name);

				while (csv.Read())
				{
					var pk = csv.GetField(0) ?? "";
					foreach (var index in ptIndexes)
					{
						cells.Add((name, pk, headers[index], csv.GetField(index) ?? ""));
					}
				}
			}

			cells.Should().NotBeEmpty("all corpus CSVs must load rows");
			return cells;
		}

		[Fact]
		public void PtColumns_NoEstarAInfinitivo()
		{
			var failures = new List<string>();
			foreach (var (file, pk, column, cell) in LoadPtCells())
			{
				var offender = EstarConjugaAInfinitivo.Match(cell);
				if (!offender.Success)
				{
					offender = EstarInfinitivoAInfinitivo.Match(cell);
				}

				if (offender.Success)
				{
					failures.Add($"[{file} / {pk} / {column}] '{offender.Value}' in: '{cell.Truncate(80)}…'");
				}
			}

			failures.Should().BeEmpty(
				"pt corpus syntaxe est pt-BR « estar + gérondio » (poche ②, décision ai-01 10/10) : " +
				"« estar a + infinitif » (pt-PT) est banni de toute colonne _pt, la préposition figée " +
				"« a partir » exceptée:\n{0}",
				string.Join("\n", failures));
		}

		[Fact]
		public void PtColumns_NoPerceptionCliticPlusAInfinitivo()
		{
			var failures = new List<string>();
			foreach (var (file, pk, column, cell) in LoadPtCells())
			{
				var offender = CliticoMaisAInfinitivo.Match(cell);
				if (offender.Success)
				{
					failures.Add($"[{file} / {pk} / {column}] '{offender.Value}' in: '{cell.Truncate(80)}…'");
				}
			}

			failures.Should().BeEmpty(
				"« vi-o a aproveitar » → « vi-o aproveitando » (poche ④, grain 2026-10-14) : le clitique " +
				"de perception + a + INF est banni des colonnes _pt:\n{0}",
				string.Join("\n", failures));
		}

		[Fact]
		public void PtColumns_NoContinentalLexicon()
		{
			var failures = new List<string>();
			foreach (var (file, pk, column, cell) in LoadPtCells())
			{
				foreach (var (pattern, banned, brazilian) in LexicalContinental)
				{
					var offender = pattern.Match(cell);
					if (offender.Success)
					{
						failures.Add($"[{file} / {pk} / {column}] '{offender.Value}' ({banned} → BR « {brazilian} »)");
					}
				}
			}

			failures.Should().BeEmpty(
				"lexique continental banni des colonnes _pt (poche ④, tiers A/B du scanner #1858). " +
				"Hors périmètre déclaré : vós/vosso (registre, grain ㉚) et dossiê (les deux variantes):\n{0}",
				string.Join("\n", failures));
		}

		[Fact]
		public void ConvertedCells_CarryTheBrazilianForms()
		{
			// Témoins du grain : la cellule double-convertie (pk 390, les deux tokens), le titre
			// converti avec sa majuscule, le genre ajusté (ecrã → da minha tela), et la proclise BR.
			var cells = LoadPtCells();
			Cell(cells, "fallacies", "390", "example_pt").Should().Contain("estás ouvindo",
				"pk 390 porte les DEUX conversions du grain (estás a ouvir → estás ouvindo)");
			Cell(cells, "fallacies", "390", "example_pt").Should().Contain("estão discutindo",
				"pk 390 porte les DEUX conversions du grain (estão a discutir → estão discutindo)");
			Cell(cells, "fallacies", "630", "example_pt").Should().Contain("na frente da minha tela",
				"ecrã (masc) → tela (fém) : l'article et le possessif suivent le genre");
			Cell(cells, "scenarii", "3.2.9", "title_pt").Should().Be("Esporte ou sofá?",
				"le titre converti, majuscule comprise");
			Cell(cells, "scenarii", "4.1.14", "context_pt").Should().Contain("está se tornando",
				"la proclise brésilienne remplace la construction a + INF pronominale");
			Cell(cells, "virtues", "96", "remark_pt").Should().Contain("Está chovendo",
				"le Modus Ponens de Virtues parle brésilien");
		}

		[Fact]
		public void Detector_SeesSyntheticContinental()
		{
			// Contrôle inverse, littéraux en mémoire (jamais l'arbre validé) : le détecteur doit
			// voir un 1 pour chaque forme bannie — un 0 sur le corpus n'est une absence que si
			// l'instrument pouvait voir un 1.
			foreach (var literal in new[]
			{
				"mas estás a ouvir?", "Os vizinhos estão a discutir", "Estou a repensar",
				"vi-o a aproveitar", "Um camião", "os camiões", "no meu ecrã", "os seus contactos",
				"Desporto ou sofá?", "este pequeno-almoço", "a receção", "Está a chover",
			})
			{
				var seen = EstarConjugaAInfinitivo.IsMatch(literal)
				           || EstarInfinitivoAInfinitivo.IsMatch(literal)
				           || CliticoMaisAInfinitivo.IsMatch(literal)
				           || LexicalContinental.Any(t => t.pattern.IsMatch(literal));
				seen.Should().BeTrue(
					"le détecteur doit voir '{0}' — sinon le vert du corpus n'est qu'une cécité", literal);
			}

			// Et ne doit PAS pincer les voisins légitimes (exemptions & hors périmètre déclaré).
			foreach (var literal in new[]
			{
				"estás ouvindo", "estão discutindo", "vi-o aproveitando", "Um caminhão",
				"da minha tela", "os seus contatos", "Esporte ou sofá?", "café da manhã",
				"a recepção", "Está chovendo",
				"Ele deve convencê-lo a continuar.", // « a » préposition : les deux variantes
				"a partir de amanhã",                // exemption du scanner #1858
				"confiai em mim, vós que ouvis",     // vós = registre, hors périmètre déclaré
				"envie o dossiê até sexta",          // dossiê : les deux variantes (tier C)
			})
			{
				var seen = EstarConjugaAInfinitivo.IsMatch(literal)
				           || EstarInfinitivoAInfinitivo.IsMatch(literal)
				           || CliticoMaisAInfinitivo.IsMatch(literal)
				           || LexicalContinental.Any(t => t.pattern.IsMatch(literal));
				seen.Should().BeFalse("'{0}' n'est pas une construction continentale bannie", literal);
			}
		}

		private static string Cell(
			List<(string file, string pk, string column, string cell)> cells,
			string file, string pk, string column) =>
			cells.Single(c => c.file == file && c.pk == pk && c.column == column).cell;
	}
}
