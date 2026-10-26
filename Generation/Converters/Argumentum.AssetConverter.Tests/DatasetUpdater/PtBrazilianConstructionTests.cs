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
	/// Le grain converti 27 remplacements dans 24 cellules (2026-10-14 ; re-mesuré au merge `c57916da`, diff _pt intégral — l'écart 26 relevé en revue tenait au comptage) : 19 « estar a + INF » → « estar +
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
	/// EXTENSION #1858 (pool ai-01 c.6166753012, 2026-10-14) : les pt-PT relevés au corps des
	/// PR #1878/#1879 (golo, guarda-redes, Tenciona) deviennent marqueurs bannis ET corrigés :
	/// 5 remplacements dans 4 cellules, tout Scenarii 7.2.10/7.3.4 — golo → gol ×2,
	/// guarda-redes → goleiro ×2 (contexte + smoothTalker), Tenciona → Pretende. Chaque
	/// nouveau marqueur passe le contrôle inverse (Detector_SeesSyntheticContinental).
	///
			/// EXTENSION REGISTRE (pool ai-01 c.6203990682, arbitrage 1, 2026-10-16) : le grain
		/// « registre pt » promis par le HORS PÉRIMÈTRE ci-dessous ATERRIT ici.
		/// vós/convosco/vosso/vossa ET les désinences verbales 2pl européennes
		/// (-ais/-eis/-is/ís) deviennent marqueurs bannis des colonnes _pt : 24 remplacements
		/// dans 20 cellules (16 desc_pt : les 9 nommées par l'arbitrage + 7 trouvées par la
		/// mesure élargie — 362/511/621/694/860/1282 verbe 2pl à sujet zéro, 1053 clitique
		/// « vos » ; 4 example_pt dialogue : 346/453/457/808). Exemptions DÉCLARÉES :
		/// « Vossa + titre » (honorifique, légitime dans les deux variantes — Scenarii 2.1.8
		/// « Vossa Majestade ») ; Scenarii 5.2.1 suggestion_pt (citation biblique, choix
		/// stylistique ai-01). Les formes TU singulières (constróis pk 1129, atribuis pk
		/// 1131) restent hors périmètre : registre singulier, grain séparé — stoplistées
		/// ici pour ne pas être pincées par le motif -is.
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
			(new Regex(@"\bgolos?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "golo", "gol"),
			(new Regex(@"\bguarda-redes\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "guarda-redes", "goleiro"),
			(new Regex(@"\btenciona(?:r|s|m|mos|va|vam|ram|ndo)?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase), "tenciona(r)", "pretende(r)"),
		};

		/// <summary>
		/// Tier registre 2pl européen (grain 2026-10-16) : sujet rendu (vós/vos/
		/// convosco/vosso/vossa), désinences verbales -ais/-eis/-is/ís (motif dérivé
		/// de l'instrument #1723, stoplist étendue aux 201 nominaux (167 passe 5-corpus + 33 #1723 + OVNIs) MESURÉS sur les 5
		/// corpus), et formes 2pl hors désinences (fordes/confiai/fazei, mesurées pk 346).
		/// </summary>
		private static readonly Regex SegundaPessoaPluralLexical = new(
			@"\b(?:v[óo]s|convosco|voss[ao]s?)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private static readonly Regex SegundaPessoaPluralMorpho = new(
			@"\b\w{3,}(?:ais|eis|is|\u00eds)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private static readonly Regex SegundaPessoaPluralFormes = new(
			@"\b(?:fordes|confiai|fazei)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>URLs : les colonnes link_pt portent https://… dont les segments
		/// (Divisão…) ressemblent à des mots — mesuré pk 625, jamais une forme verbale.</summary>
		private static readonly Regex UrlSpan = new(@"https?://\S+",
			RegexOptions.Compiled);

		/// <summary>Honorifiques : « Vossa Majestade/Excelência/… » est légitime dans
		/// les deux variantes (Scenarii 2.1.8) — le span est retiré avant classification.</summary>
		private static readonly Regex HonorificVossa = new(
			@"\bVossa\s+(?:Majestade|Excel[eê]ncia|Santidade|Alteza|Senhoria)\b",
			RegexOptions.Compiled);

		/// <summary>Citation biblique, Scenarii 5.2.1 suggestion_pt (choix stylistique ai-01).
		/// Exemption par cellule nommée — le motif resterait capable de la voir (témoin).</summary>
		private static bool IsExemptedCitation(string file, string pk, string column) =>
			file == "scenarii" && pk == "5.2.1" && column == "suggestion_pt";

		/// <summary>Nominaux mesurés : 175 mots distincts de la passe du 16/10 sur les 5
		/// corpus, moins les 8 formes verbales 2pl bannies ; formes TU constróis/atribuis
		/// conservées. Un mot absent de cette liste ET finissant par une désinence 2pl =
		/// défaut potentiel.</summary>
		private static readonly HashSet<string> MorphoNominalStop = new()
		{
			"aceitáveis",
			"acessíveis",
			"adicionais",
			"ambientais",
			"amores",
			"ancestrais",
			"animais",
			"anos",
			"aplicáveis",
			"atribuis",
			"atuais",
			"automóveis",
			"canais",
			"cannabis",
			"capitalais",
			"civis",
			"colaterais",
			"comparáveis",
			"compreensíveis",
			"condicionais",
			"confidenciais",
			"confiáveis",
			"consensuais",
			"constróis",
			"contestáveis",
			"contextuais",
			"contratuais",
			"convencionais",
			"cores",
			"corporais",
			"credenciais",
			"credíveis",
			"cristais",
			"cruciais",
			"críveis",
			"culturais",
			"demais",
			"denominacionais",
			"depois",
			"desfavoráveis",
			"difíceis",
			"dimatis",
			"disamis",
			"disponíveis",
			"distinguíveis",
			"divis",
			"dóceis",
			"emocionais",
			"espaciais",
			"especiais",
			"essenciais",
			"estruturais",
			"estáveis",
			"excepcionais",
			"faciais",
			"factuais",
			"favoráveis",
			"fazeres",
			"finais",
			"fiáveis",
			"flexíveis",
			"flores",
			"fundamentais",
			"fáceis",
			"gerais",
			"gramaticais",
			"guanais",
			"hendiadis",
			"heróis",
			"homossexuais",
			"hostis",
			"hotéis",
			"identificáveis",
			"iguais",
			"imorais",
			"imparciais",
			"imperceptíveis",
			"impossíveis",
			"imprevisíveis",
			"improváveis",
			"incompatíveis",
			"incompreensíveis",
			"incontestáveis",
			"incríveis",
			"indispensáveis",
			"individuais",
			"inevitáveis",
			"infalíveis",
			"infiéis",
			"informais",
			"iniciais",
			"inimagináveis",
			"institucionais",
			"insubstanciais",
			"insuportáveis",
			"intelectuais",
			"inteligíveis",
			"intermináveis",
			"inverificáveis",
			"invisíveis",
			"inúteis",
			"irracionais",
			"irreais",
			"irrecuperáveis",
			"irrefutáveis",
			"jamais",
			"justificáveis",
			"legais",
			"locais",
			"louis",
			"lápis",
			"mais",
			"marciais",
			"materiais",
			"matinais",
			"mensuráveis",
			"mentais",
			"metais",
			"modais",
			"montanais",
			"morais",
			"mortais",
			"multifatoriais",
			"móveis",
			"naturais",
			"notáveis",
			"níveis",
			"observáveis",
			"oficiais",
			"operacionais",
			"ovnis",
			"paises",
			"palpáveis",
			"papéis",
			"parciais",
			"paris",
			"passíveis",
			"país",
			"perceptíveis",
			"pessoais",
			"plausíveis",
			"possíveis",
			"potenciais",
			"prejudiciais",
			"previsíveis",
			"principais",
			"profissionais",
			"proposicionais",
			"prováveis",
			"quadris",
			"quais",
			"quantificáveis",
			"questionáveis",
			"raciais",
			"racionais",
			"radicais",
			"razoáveis",
			"raízes",
			"reais",
			"refutáveis",
			"renováveis",
			"reproduzíveis",
			"residenciais",
			"responsáveis",
			"répteis",
			"saudáveis",
			"seis",
			"semmelweis",
			"sempre",
			"sensoriais",
			"sensíveis",
			"sexuais",
			"sinais",
			"sindicais",
			"situacionais",
			"sociais",
			"solucionáveis",
			"substanciais",
			"subtis",
			"superficiais",
			"susceptíveis",
			"suscetíveis",
			"tangíveis",
			"temporais",
			"testáveis",
			"tradicionais",
			"triviais",
			"trois",
			"três",
			"tu",
			"ténis",
			"unilaterais",
			"universais",
			// Passe 3 Virtues (21/10) : « provas úteis » — pluriel de l'adjectif « útil »,
			// aucun verbe *utir. Accent sur l'antépénultième (Ú-teis), là où une 2pl
			// porte l'accent sur la finale (atribuís, constróis). Mesuré absent des 5
			// corpus avant cette passe, donc jamais listé — contrôle négatif ci-dessous.
			"úteis",
			"valores",
			"variáveis",
			"verbais",
			"verificáveis",
			"vezes",
			"visíveis",
			"vocês",
			"vulneráveis",
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
		public void PtColumns_NoSegundaPessoaPluralContinental()
		{
			var failures = new List<string>();
			foreach (var (file, pk, column, cell) in LoadPtCells())
			{
				if (IsExemptedCitation(file, pk, column))
				{
					continue;
				}

				var value = UrlSpan.Replace(HonorificVossa.Replace(cell, " "), " ");
				var offender = SegundaPessoaPluralLexical.Match(value);
				if (!offender.Success)
				{
					offender = SegundaPessoaPluralFormes.Match(value);
				}

				if (!offender.Success)
				{
					foreach (Match m in SegundaPessoaPluralMorpho.Matches(value))
					{
						if (!MorphoNominalStop.Contains(m.Value.ToLowerInvariant()))
						{
							offender = m;
							break;
						}
					}
				}

				if (offender.Success)
				{
					failures.Add($"[{file} / {pk} / {column}] '{offender.Value}' in: '{cell.Truncate(80)}…'");
				}
			}

			failures.Should().BeEmpty(
				"registre pt = vocês (grain registre 2026-10-16, arbitrage 1 du pool c.6203990682) : " +
				"vós/convosco/vosso/vossa et désinences verbales 2pl européennes sont bannis de toute " +
				"colonne _pt. Exemptions déclarées : Vossa + titre (honorifique) et Scenarii 5.2.1 " +
				"suggestion_pt (citation biblique, ai-01). Formes TU singulières hors périmètre :\n{0}",
			string.Join("\n", failures));
		}

		[Fact]
		public void ConvertedRegisterCells_CarryVoces()
		{
			// Témoins du grain registre : les 9 nommées + les 7 de la mesure élargie + les
			// 4 example_pt, dont les imprimées (362/511/621/1282/808) qui conditionnent le deck.
			var cells = LoadPtCells();
			Cell(cells, "fallacies", "362", "desc_pt").Should().StartWith("Vocês enquadram",
				"pk 362 [IMPRIMÉE], mesure élargie : Enquadrais → Vocês enquadram");
			Cell(cells, "fallacies", "511", "desc_pt").Should().StartWith("Vocês procuram",
				"pk 511 [IMPRIMÉE], mesure élargie : Procurais → Vocês procuram");
			Cell(cells, "fallacies", "621", "desc_pt").Should().StartWith("Vocês atribuem",
				"pk 621 [IMPRIMÉE], mesure élargie : Atribuís → Vocês atribuem");
			Cell(cells, "fallacies", "1282", "desc_pt").Should().StartWith("Vocês afirmam",
				"pk 1282 [IMPRIMÉE], mesure élargie : Afirmais → Vocês afirmam");
			Cell(cells, "fallacies", "472", "desc_pt").Should().StartWith("Vocês exercem",
				"pk 472, arbitrage 1 : Exerceis → Vocês exercem");
			Cell(cells, "fallacies", "1056", "desc_pt").Should().StartWith("As suas expectativas",
				"pk 1056 : vossas → suas, le verbe était déjà 3pl");
			Cell(cells, "fallacies", "860", "desc_pt").Should().StartWith("Vocês empregam",
				"pk 860, mesure élargie : même verbe Empregais que 1298, trouvé par le motif dérivé");
			Cell(cells, "fallacies", "694", "desc_pt").Should().StartWith("Vocês utilizam um símbolo",
				"pk 694, mesure élargie");
			Cell(cells, "fallacies", "463", "desc_pt").Should().Contain("os seus desejos",
				"pk 463 : sujet Vocês déjà BR, seul le possessif portait le registre EP");
			Cell(cells, "fallacies", "947", "desc_pt").Should().Contain("às suas crenças",
				"pk 947, arbitrage 1");
			Cell(cells, "fallacies", "1170", "desc_pt").Should().Contain("as suas necessidades",
				"pk 1170, arbitrage 1");
			Cell(cells, "fallacies", "1171", "desc_pt").Should().Contain("as suas opiniões",
				"pk 1171, arbitrage 1");
			Cell(cells, "fallacies", "1053", "desc_pt").Should().Contain("do que eles compreendem vocês.",
				"pk 1053 : clitique vos invisible au grep vós initial — la raison d'être de la mesure élargie");
			Cell(cells, "fallacies", "394", "desc_pt").Should().Contain("Vocês utilizam").And.Contain("as suas realiza",
				"pk 394, arbitrage 1 : Utilizais + vossas → Vocês utilizam + suas");
			Cell(cells, "fallacies", "408", "desc_pt").Should().Contain("Vocês utilizam").And.Contain("às suas ideias",
				"pk 408, arbitrage 1");
			Cell(cells, "fallacies", "1298", "desc_pt").Should().StartWith("Vocês empregam").And.Contain("as suas palavras",
				"pk 1298, le cas nommé par le dispatch (Empregais)");
			Cell(cells, "fallacies", "346", "example_pt").Should().StartWith("Você compreenderá quando for mais velho").And.Contain("faça o que eu digo",
				"pk 346 : l'original fr adresse UN singulier (« plus âgé ») — alignement du nombre, pas seulement du dialecte");
			Cell(cells, "fallacies", "453", "example_pt").Should().Contain("Como já lhes disse",
				"pk 453 : clitique EP vos → lhes (dizer à qqn, indirect)");
			Cell(cells, "fallacies", "457", "example_pt").Should().Contain("Os senhores dizem que eu lhes prometi",
				"pk 457 : registre mixte dans une seule bouche (os senhores BR + vos EP) → cohérent");
			Cell(cells, "fallacies", "808", "example_pt").Should().Be("Eu adoro vocês, portanto não amo vocês.",
				"pk 808 [IMPRIMÉE] : le jeu de mots adorer/aimer n'est pas une citation attribuée — dialogue converti");
		}

		[Fact]
		public void Detector_SeesSyntheticSegundaPessoaPlural()
		{
			// Contrôle inverse : chaque forme bannie doit être VUE par le motif dérivé —
			// un 0 sur le corpus n'est une absence que si l'instrument pouvait voir un 1.
			foreach (var literal in new[]
			{
				"Empregais definições",
				"as vossas palavras",
				"Vós que ouvis",
				"o que vos digo",
				"Atribuís a um grupo",
				"Enquadrais uma crítica",
				"Compreendereis quando fordes",
				"confiai em mim",
				"fazei o que eu digo",
				"Convosco falarei",
				"Utilizais técnicas",
			})
			{
				var value = UrlSpan.Replace(HonorificVossa.Replace(literal, " "), " ");
				var seen = SegundaPessoaPluralLexical.IsMatch(value)
					|| SegundaPessoaPluralFormes.IsMatch(value)
					|| SegundaPessoaPluralMorpho.Matches(value).Any(m =>
						!MorphoNominalStop.Contains(m.Value.ToLowerInvariant()));
				seen.Should().BeTrue(
					"le détecteur 2pl doit voir '{0}' — sinon le vert du corpus n'est qu'une cécité", literal);
			}

			// Et ne doit PAS pincer les voisins légitimes (stoplist mesurée + exemptions).
			foreach (var literal in new[]
			{
				"Vocês utilizam",
				"como já lhes disse",
				"Eu adoro vocês",
				"Vocês empregam",
				"mais vale seis que três",
				"Os animais reais são mortais",
				"quais são os sensoriais",
				"as bases verificáveis",
				"constróis uma representação",
				"atribuis erroneamente tuas emoções",
				"que Vossa Majestade não se zangue",
				"apoiado em provas úteis para a conclusão",
				"https://pt.wikipedia.org/wiki/Divis%C3%A3o_(fal%C3%A1cia)",
			})
			{
				var value = UrlSpan.Replace(HonorificVossa.Replace(literal, " "), " ");
				var seen = SegundaPessoaPluralLexical.IsMatch(value)
					|| SegundaPessoaPluralFormes.IsMatch(value)
					|| SegundaPessoaPluralMorpho.Matches(value).Any(m =>
						!MorphoNominalStop.Contains(m.Value.ToLowerInvariant()));
				seen.Should().BeFalse("'{0}' n'est pas une 2pl continentale bannie", literal);
			}
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
			Cell(cells, "scenarii", "7.2.10", "suggestion_pt").Should().Contain("Pretende fazer",
				"tenciona → Pretende (extension #1858, grain 2026-10-14)");
			Cell(cells, "scenarii", "7.3.4", "context_pt").Should().Contain("o goleiro que",
				"guarda-redes → goleiro (extension #1858)");
			Cell(cells, "scenarii", "7.3.4", "context_pt").Should().Contain("um gol decisivo",
				"golo → gol (extension #1858)");
			Cell(cells, "scenarii", "7.3.4", "smoothTalker_pt").Should().Be("O goleiro",
				"cellule entière convertie (guarda-redes → goleiro)");
			Cell(cells, "scenarii", "7.3.4", "suggestion_pt").Should().Contain("evitado este gol",
				"golo → gol (extension #1858)");
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
				"Desporto ou sofá?", "este pequeno-almoço", "a receção", "Está a chover", "um golo decisivo", "o guarda-redes sofreu", "Tenciona fazer alguma coisa",
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
				"a recepção", "Está chovendo", "um gol decisivo", "o goleiro sofreu", "Pretende fazer alguma coisa",
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
