using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde dérivée du fr-FR pour les pages juridiques en (resx neutre), es-ES, pt-BR et ru-RU
	/// (pool #458, dispatch ai-01 28/10 c.6456588555, décision (a)) : ces quatre valeurs portaient
	/// le gabarit DNN générique (« [Portal:PortalName] », ancre ACUERDO/ACORDO, DMCA 17 U.S.C. 512)
	/// alors que le fr-FR porte la rédaction LCEN qui fait foi (mentions LCEN, SAS, SIREN) ; elles
	/// sont désormais des traductions du fr-FR, précédées d'une note disant que la version française
	/// fait foi — comme zh/ar/fa avant elles (décision 2 c.6405473398).
	/// <para>
	/// CHOIX : toutes les attentes sont DÉRIVÉES de la valeur fr-FR au moment du test (séquence de
	/// balises, jetons portail, comptes d'identité, multiset des chiffres, découpe en segments et
	/// longueurs) ou relèvent de la langue (script, terminologie LCEN déjà arbitrée — publisher /
	/// editado por / guillemets de chaque langue / nº —, article masculin pt #1912, phrase d'en-tête
	/// imposée au traducteur). Figer les nouvelles chaînes traduites aurait reproduit le défaut du
	/// gabarit : un témoin qui survit à la vérité de l'artefact.
	/// </para>
	/// <para>
	/// ru n'a AUCUNE épingle de glossaire : la tête russe de l'ancienne valeur (ЮРИДИЧЕСКАЯ
	/// ИНФОРМАЦИЯ, ИДЕНТИФИКАЦИЯ САЙТА) était la traduction d'un texte que E2 remplace — l'épingler
	/// serait une contre-épingle. Ce qui reste de l'ancienne garde ru et ne dépend pas du gabarit
	/// est repris ICI, dérivé : identité complète (ARGUMENTUM SAS / 5 B RUE MARTEL / 75010 PARIS 10 /
	/// SIREN: 922020276 / MY INTELLIGENCE AGENCY), URL Légifrance et numéro de loi « 2004-575 ».
	/// </para>
	/// </summary>
	public class DnnLegalEnEsPtRuGuardTests
	{
		private const string FrResx = "DNNPlatform/App_GlobalResources/GlobalResources.fr-FR.resx";
		private static readonly string[] Keys = { "MESSAGE_PORTAL_TERMS.Text", "MESSAGE_PORTAL_PRIVACY.Text" };
		private static readonly Regex Tag = new Regex("<[^>]+>");
		private static readonly Regex AsciiDigits = new Regex("[0-9]+"); // \d .NET verrait aussi les chiffres persans

		// La note d'en-tête est insérée juste après le <div> d'ouverture du fr : 4 balises.
		private static readonly string[] NoteTags = { "<p align=\"left\">", "<em>", "</em>", "</p>" };

		// « en » = le resx NEUTRE (fallback anglais, servi sous /en).
		private static readonly Dictionary<string, string> ResxFile = new Dictionary<string, string>
		{
			["en"] = "DNNPlatform/App_GlobalResources/GlobalResources.resx",
			["es"] = "DNNPlatform/App_GlobalResources/GlobalResources.es-ES.resx",
			["pt"] = "DNNPlatform/App_GlobalResources/GlobalResources.pt-BR.resx",
			["ru"] = "DNNPlatform/App_GlobalResources/GlobalResources.ru-RU.resx",
		};

		// Phrase d'en-tête IMPOSÉE au traducteur (règle 2 du prompt) : épinglée entière, pas
		// seulement ses mots-clés — deux jetons laissaient passer « … is NOT authoritative »
		// (deuxième contre-revue Opus). Ce n'est pas une contre-épingle : c'est une exigence
		// posée au traducteur, au même titre que le glossaire.
		private static readonly Dictionary<string, string> NoteSigned = new Dictionary<string, string>
		{
			["en"] = "The French version of this page is the authoritative version.",
			["es"] = "La versión francesa de esta página es la versión de referencia.",
			["pt"] = "A versão francesa desta página é a versão de referência.",
			["ru"] = "Французская версия этой страницы является официальной.",
		};

		private static readonly Dictionary<string, string[]> FrenchResidueExcluded = new Dictionary<string, string[]>
		{
			// mots du résidu français qui SONT des mots de la langue cible : les retirer du
			// motif évite le faux rouge (mesuré par la relecture des traductions)
			["en"] = new[] { "site", "de" },           // « site » est anglais ; « de » vit dans « de facto/de jure »
			["es"] = new[] { "que", "de", "la", "un", "les" },
			["pt"] = new[] { "que", "de", "ou", "site" }, // « site » est courant en pt-BR
			["ru"] = Array.Empty<string>(),
		};

		/// <summary>Épingles de glossaire LCEN par langue (celles que DnnLegalMentionsResxGuardTests garde). ru : aucune.</summary>
		private static readonly Dictionary<string, string[]> GlossaryPins = new Dictionary<string, string[]>
		{
			["en"] = new[] { "LEGAL NOTICE", "SITE IDENTIFICATION", "is published by:", "“Publisher of the Site”", "“Host”", "“Argumentum”" },
			["es"] = new[] { "AVISO LEGAL", "IDENTIFICACIÓN DEL SITIO", "está editado por", "está alojado por", "«Editor del Sitio»", "«Proveedor de Alojamiento»", "«Argumentum»" },
			["pt"] = new[] { "AVISO LEGAL", "IDENTIFICAÇÃO DO SITE", "é editado por", "é hospedado por", "“Editor do Site”", "“Provedor de Hospedagem”", "“Argumentum”", "nº 2004-575" },
			["ru"] = Array.Empty<string>(),
		};

		private static readonly Dictionary<string, string[]> GlossaryBanned = new Dictionary<string, string[]>
		{
			["en"] = new[] { "edited by", "Editor of the Site" }, // faux-amis corrigés (revue ai-01 c.6143944905)
			["es"] = Array.Empty<string>(),
			["pt"] = new[] { "n.º" }, // graphie pt-PT ; le site cible est pt-BR (#1095)
			["ru"] = Array.Empty<string>(),
		};

		private static XDocument Load(string resxRelativePath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");
			return XDocument.Load(path);
		}

		private static XElement Entry(string resxRelativePath, string key)
		{
			var entry = Load(resxRelativePath).Root!.Elements("data")
				.FirstOrDefault(d => string.Equals(d.Attribute("name")?.Value, key, StringComparison.Ordinal));
			entry.Should().NotBeNull($"la clé {key} doit exister dans {resxRelativePath}");
			return entry!;
		}

		private static string ResxValue(string resxRelativePath, string key) => Entry(resxRelativePath, key).Element("value")?.Value ?? string.Empty;

		/// <summary>Texte visible : balises retirées, entités décodées (&amp;#8217; ne doit pas compter « 8217 »).</summary>
		private static string PlainText(string html) => WebUtility.HtmlDecode(Tag.Replace(html, " "));

		/// <summary>Vue servie : balises retirées, entités décodées, blancs compactés (le navigateur fait de même).
		/// Sans cela, « edited&amp;#160;by » ou « edited &lt;b&gt;by&lt;/b&gt; » échappe à une forme interdite,
		/// et un terme coupé par un repli de ligne du resx échapperait à une épingle (contre-revue Opus).</summary>
		private static string Served(string html) =>
			Regex.Replace(WebUtility.HtmlDecode(Tag.Replace(html, " ")).Replace("\r", " ").Replace("\n", " "), @"\s+", " ");

		private static Dictionary<string, int> DigitRuns(string text) => AsciiDigits.Matches(text)
			.GroupBy(m => m.Value).ToDictionary(g => g.Key, g => g.Count());

		[Theory]
		[InlineData("en")]
		[InlineData("es")]
		[InlineData("pt")]
		[InlineData("ru")]
		public void Resx_Legal_Keys_Exist_And_Preserve_Space(string locale)
		{
			foreach (var key in Keys)
			{
				// parenthèses obligatoires : « ?.Value.Should() » court-circuiterait l'assertion
				// elle-même quand l'attribut manque (garde vacue, mesurée par mutation)
				(Entry(ResxFile[locale], key).Element("value")!.Attribute(XNamespace.Xml + "space")?.Value)
					.Should().Be("preserve", $"{locale} {key} : <value xml:space=\"preserve\">");
			}
		}

		[Theory]
		[InlineData("en", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("en", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("es", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("es", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_PRIVACY.Text")]
		public void TagSequence_Is_Fr_Plus_HeaderNote(string locale, string key)
		{
			var fr = Tag.Matches(ResxValue(FrResx, key)).Select(m => m.Value).ToList();
			var loc = Tag.Matches(ResxValue(ResxFile[locale], key)).Select(m => m.Value).ToList();

			fr.Should().NotBeEmpty();
			loc.Should().HaveCount(fr.Count + NoteTags.Length, $"{locale} {key} : balises du fr + la note d'en-tête");
			loc.Skip(1).Take(NoteTags.Length).Should().Equal(NoteTags, "la note suit immédiatement le <div> d'ouverture");
			loc.Take(1).Concat(loc.Skip(1 + NoteTags.Length)).Should().Equal(fr, $"{locale} {key} : HTML du fr intact, balise pour balise");
		}

		[Theory]
		[InlineData("en", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("en", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("es", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("es", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_PRIVACY.Text")]
		public void HeaderNote_Says_French_Is_Authoritative(string locale, string key)
		{
			var value = ResxValue(ResxFile[locale], key);
			var note = Regex.Match(value, "^<div[^>]*><p align=\"left\"><em>([^<]+)</em></p>");
			note.Success.Should().BeTrue($"{locale} {key} : la note ouvre la page");
			note.Groups[1].Value.Trim().Should().Be(NoteSigned[locale],
				$"{locale} {key} : phrase d'en-tête imposée — la nommer gardait la polarité hors de portée");
		}

		[Theory]
		[InlineData("en", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("en", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("es", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("es", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_PRIVACY.Text")]
		public void PortalTokens_And_IdentityPins_Match_Fr(string locale, string key)
		{
			var fr = ResxValue(FrResx, key);
			var loc = ResxValue(ResxFile[locale], key);
			var probes = Regex.Matches(fr, @"\[Portal:\w+\]").Select(m => m.Value).Distinct()
				.Concat(new[]
				{
					"ARGUMENTUM SAS", "MY INTELLIGENCE AGENCY", "SIREN", "Argumentum", "922020276",
					// l'adresse et l'URL ne se traduisent pas : l'ancienne garde ru les épinglait,
					// E2 les DÉRIVE du fr (compte égal) au lieu de figer une chaîne par langue
					"5 B RUE MARTEL", "75010 PARIS 10",
					"ARGUMENTUM SAS, 5 B RUE MARTEL, 75010 PARIS 10",
					"MY INTELLIGENCE AGENCY, 5 B RUE MARTEL, 75010 PARIS 10",
					"SIREN: 922020276", "2004-575",
					"https://www.legifrance.gouv.fr/jorf/id/JORFTEXT000000801164", // vit dans un href
				}).ToList();
			// Une sonde ABSENTE du fr comparerait 0 à 0 (PRIVACY ne porte pas l'identité) :
			// elle n'est appliquée que là où le fr la porte, et l'ensemble appliqué doit être
			// non vide — sinon la garde serait verte sans rien mesurer.
			var applied = probes.Where(p => Regex.Matches(fr, Regex.Escape(p)).Count > 0).ToList();
			applied.Should().NotBeEmpty($"{locale} {key} : au moins une sonde doit être mesurable");
			foreach (var probe in applied)
			{
				Regex.Matches(loc, Regex.Escape(probe)).Count.Should().Be(Regex.Matches(fr, Regex.Escape(probe)).Count,
					$"{locale} {key} : « {probe} » copié autant de fois que dans le fr");
			}
		}

		[Theory]
		[InlineData("en", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("en", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("es", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("es", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_PRIVACY.Text")]
		public void Digits_Are_The_Fr_Digits(string locale, string key)
		{
			var want = DigitRuns(PlainText(ResxValue(FrResx, key)));
			want.Should().NotBeEmpty("le fr porte des numéros (loi, SIREN, dates) : un multiset vide serait un instrument aveugle");
			DigitRuns(PlainText(ResxValue(ResxFile[locale], key))).Should().BeEquivalentTo(want,
				$"{locale} {key} : nombres du fr, ni ajoutés ni perdus");
			Regex.IsMatch(ResxValue(ResxFile[locale], key), "[٠-٩۰-۹]").Should()
				.BeFalse("chiffres latins, jamais arabo-indiens ni persans");
		}

		/// <summary>Résidu français : mots-outils hors identité légale. Le motif doit d'abord voir le fr.</summary>
		private static readonly string[] FrenchFunctionWords =
			{ "le", "la", "les", "de", "des", "du", "et", "ou", "un", "une", "vous", "nous", "est", "dans", "pour", "par", "sur", "au", "aux", "ce", "ces", "cette", "qui", "que", "site" };

		private static Regex ResiduePattern(string locale)
		{
			var words = FrenchFunctionWords.Except(FrenchResidueExcluded[locale])
				.OrderByDescending(w => w.Length); // les formes longues d'abord
			return new Regex(@"(?<![A-Za-zÀ-ÿ])(" + string.Join("|", words) + @")(?![A-Za-zÀ-ÿ])",
				RegexOptions.IgnoreCase);
		}

		[Theory]
		[InlineData("en", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("en", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("es", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("es", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_PRIVACY.Text")]
		public void No_French_Residue_And_Target_Script(string locale, string key)
		{
			var pattern = ResiduePattern(locale);
			pattern.Matches(PlainText(ResxValue(FrResx, key))).Count
				.Should().BeGreaterThan(100, "auto-contrôle : le motif doit voir le résidu dans le fr lui-même");

			var text = PlainText(ResxValue(ResxFile[locale], key));
			pattern.Matches(text).Select(m => m.Value).Should().BeEmpty($"{locale} {key} : aucun mot-outil français");

			if (locale == "ru")
			{
				Regex.Matches(text, "[А-Яа-яЁё]").Count.Should().BeGreaterThan(1000, "ru : texte en cyrillique");
				// plafond d'écriture latine : mesuré 136 (TERMS) / 154 (PRIVACY) — l'identité
				// légale et le nom de la loi, rien d'autre. Un paragraphe anglais resté en place
				// dépasse ce plafond (contre-revue Opus, défaut 5).
				Regex.Matches(text, "[A-Za-zÀ-ÿ]").Count.Should().BeLessThan(400,
					"ru : seuls les noms propres et l'identité en latin (mesuré 136 / 154)");
			}
			else
			{
				Regex.IsMatch(text, "[А-Яа-яЁё؀-ۿ一-鿿]").Should().BeFalse($"{locale} : ni cyrillique, ni arabe, ni CJK");
				Regex.Matches(text, "[A-Za-zÀ-ÿ]").Count.Should().BeGreaterThan(1000, $"{locale} : texte en lettres latines");
			}
		}

		/// <summary>
		/// Glossaire LCEN imposé au traducteur — les mêmes épingles que la tête du grain
		/// #1876/#1883 garde (publisher / editado por / guillemets / nº). La vue servie
		/// (repli de ligne rendu espace) est celle des épingles : un pin coupé par un
		/// repli de ligne du resx reste vérifié comme le navigateur le rend.
		/// </summary>
		[Theory]
		[InlineData("en", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("es", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_TERMS.Text")]
		public void Lcen_Glossary_Terms_Are_Used(string locale, string key)
		{
			var served = Served(ResxValue(ResxFile[locale], key));
			foreach (var pin in GlossaryPins[locale])
				served.Should().Contain(pin, $"{locale} {key} : terme imposé « {pin} »");
		}

		/// <summary>Formes interdites : elles relèvent de la LANGUE, pas du bloc d'identification —
		/// elles s'appliquent donc aux deux clés, sans tenir compte de la casse (une tête en
		/// capitales passait « EDITED BY ») ni des balises (retirées par <see cref="Served"/>).</summary>
		[Theory]
		[InlineData("en", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("en", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("es", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("es", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_PRIVACY.Text")]
		public void Lcen_Banned_Forms_Are_Absent(string locale, string key)
		{
			var served = Served(ResxValue(ResxFile[locale], key));
			foreach (var banned in GlossaryBanned[locale])
				Regex.IsMatch(served, Regex.Escape(banned), RegexOptions.IgnoreCase).Should().BeFalse(
					$"{locale} {key} : forme interdite « {banned} » (casse ignorée)");
			// auto-contrôle : le motif voit bien la forme qu'il interdit
			foreach (var banned in GlossaryBanned[locale])
				Regex.IsMatch(served + " " + banned.ToUpperInvariant(), Regex.Escape(banned), RegexOptions.IgnoreCase)
					.Should().BeTrue($"auto-contrôle : « {banned} » doit être vu même en capitales");
		}

		/// <summary>
		/// pt-BR : le jeton portail est MASCULIN (décision #1912) — « o/do/no [Portal:PortalName] »,
		/// jamais « a/da/na [Portal:PortalName] ». L'épingle vivait dans DnnLegalPtEsGuardTests, dont
		/// le gabarit DNN est retiré : elle est reprise ici parce qu'elle ne dépend pas du gabarit et
		/// que le nouveau texte porte le jeton (« No site [Portal:PortalName] », mesuré).
		/// </summary>
		// Le deux-points est admis entre « site » et le jeton : l'artefact livré porte lui-même
		// « do site: [Portal:PortalName] » (contre-revue web2) — l'exclure ouvrait une échappatoire
		// symétrique sur ce jeton précis.
		private static readonly Regex PtFeminineBeforeToken =
			new Regex(@"(?<![A-Za-zÀ-ÿ])(a|da|na)\s+(?:site\s*:?\s*)?\[Portal:PortalName\]", RegexOptions.IgnoreCase);

		private static readonly Regex PtMasculineBeforeToken =
			new Regex(@"(?<![A-Za-zÀ-ÿ])(o|do|no)\s+(?:site\s*:?\s*)?\[Portal:PortalName\]", RegexOptions.IgnoreCase);

		[Theory]
		[InlineData("MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("MESSAGE_PORTAL_PRIVACY.Text")]
		public void PtBr_PortalTokenArticle_Is_Masculine(string key)
		{
			// auto-contrôles : chaque motif voit la forme qu'il décrit, y compris avec « site » intercalé
			PtFeminineBeforeToken.IsMatch("a [Portal:PortalName]").Should().BeTrue("féminin nu");
			PtFeminineBeforeToken.IsMatch("Na site [Portal:PortalName]").Should().BeTrue("féminin + nom");
			PtMasculineBeforeToken.IsMatch("No site [Portal:PortalName]").Should().BeTrue("masculin + nom");

			var served = Served(ResxValue(ResxFile["pt"], key));
			PtFeminineBeforeToken.IsMatch(served).Should().BeFalse(
				$"pt {key} : article masculin devant le jeton portail (mesuré « No site [Portal:PortalName] »)");
			// contrôle POSITIF : là où le fr porte le jeton, pt doit porter la forme masculine —
			// sans lui, l'absence de faute et l'absence de tout article se lisaient pareil.
			var fr = ResxValue(FrResx, key);
			if (fr.Contains("[Portal:PortalName]"))
				PtMasculineBeforeToken.IsMatch(served).Should().BeTrue(
					$"pt {key} : une forme masculine précède le jeton (le fr en porte un)");
		}

		/// <summary>
		/// Fidélité par segment : le fr et la traduction sont découpés aux mêmes balises (la séquence
		/// est déjà épinglée), la note d'en-tête est retirée de la traduction, puis chaque paragraphe
		/// est apparié. Un paragraphe vidé, tronqué ou dupliqué rougit — ce que ni la séquence de
		/// balises ni le multiset de chiffres ne voient (contre-revue Opus, défauts 4-5). Les bornes
		/// sont l'enveloppe MESURÉE sur les huit valeurs (par segment 0,50–1,70 ; global 0,844–1,012),
		/// élargie d'une marge — pas une préférence de style.
		/// </summary>
		private static readonly Regex NoteBlock = new Regex("<p align=\"left\"><em>.*?</em></p>", RegexOptions.Singleline);

		private static string[] TextNodes(string html) =>
			Regex.Split(html, "<[^>]+>").Select(t => WebUtility.HtmlDecode(t) ?? string.Empty).ToArray();

		[Theory]
		[InlineData("en", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("en", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("es", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("es", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("pt", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("ru", "MESSAGE_PORTAL_PRIVACY.Text")]
		public void Segment_Fidelity_Matches_Fr(string locale, string key)
		{
			var fr = ResxValue(FrResx, key);
			var loc = NoteBlock.Replace(ResxValue(ResxFile[locale], key), string.Empty);
			var frNodes = TextNodes(fr);
			var locNodes = TextNodes(loc);
			locNodes.Should().HaveCount(frNodes.Length,
				$"{locale} {key} : même découpe que le fr (les balises sont déjà épinglées)");

			var pairs = frNodes.Zip(locNodes, (a, b) => (Fr: a.Trim(), Loc: b.Trim()))
				.Where(p => p.Fr.Length >= 20).ToList();
			pairs.Should().HaveCountGreaterThan(40,
				"l'instrument doit voir les paragraphes (mesuré 63 segments TERMS / 52 PRIVACY)");

			pairs.Where(p => p.Loc.Length == 0).Select(p => p.Fr.Substring(0, Math.Min(26, p.Fr.Length)))
				.Should().BeEmpty($"{locale} {key} : aucun paragraphe du fr laissé vide");

			foreach (var (a, b) in pairs)
			{
				((double)b.Length / Math.Max(1, a.Length)).Should().BeInRange(0.35, 1.9,
					$"{locale} {key} : segment « {a.Substring(0, Math.Min(26, a.Length))} » — enveloppe mesurée 0,50–1,70");
			}

			// volume global mesuré sur le TEXTE (le HTML brut, identique par la séquence de balises,
			// ramènerait le ratio vers 1 et rendrait la borne plus lâche qu'annoncé)
			var frText = string.Concat(TextNodes(fr));
			var locText = string.Concat(TextNodes(loc));
			((double)locText.Length / frText.Length).Should().BeInRange(0.7, 1.4,
				$"{locale} {key} : volume de texte contre le fr (mesuré 0,844–1,012)");
		}

		// Imbrication HTML (pool #458 E3, miroir de DnnLegalZhArFaGuardTests). La séquence
		// de balises ci-dessus est comparée au fr : un défaut présent dans le fr ET dans la
		// traduction y reste vert. Cette garde-ci lit chaque valeur seule, le fr-FR compris.
		private static readonly Regex TagParts = new Regex("<(/?)([A-Za-z0-9]+)[^>]*?(/?)>");
		private static readonly HashSet<string> VoidTags = new HashSet<string>
			{ "area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "param", "source", "track", "wbr" };
		private static readonly HashSet<string> OptionalEnd = new HashSet<string> { "p", "li" };
		// ouvrantes qui ferment un p ouvert (spécification HTML), plus li/tr/td
		private static readonly HashSet<string> BlockTags = new HashSet<string>
		{
			"address", "article", "aside", "blockquote", "details", "dialog", "div", "dl", "dd", "dt", "fieldset",
			"figcaption", "figure", "footer", "form", "h1", "h2", "h3", "h4", "h5", "h6", "header", "hgroup", "hr",
			"main", "menu", "nav", "ol", "p", "pre", "section", "table", "ul", "li", "tr", "td",
		};

		/// <summary>
		/// Défauts d'imbrication : fermante orpheline, fermante qui passe par-dessus un élément
		/// encore ouvert, élément jamais fermé. p et li ont une fin optionnelle en HTML : un p se
		/// ferme à l'ouverture d'un bloc, un li à l'ouverture d'un li frère, les deux à la fin de
		/// leur parent — ce ne sont pas des défauts.
		/// </summary>
		private static List<string> NestingDefects(string html)
		{
			var stack = new List<string>();
			var defects = new List<string>();
			foreach (Match m in TagParts.Matches(html))
			{
				var name = m.Groups[2].Value.ToLowerInvariant();
				var opening = m.Groups[1].Value.Length == 0;
				// avant le saut des vides : hr est vide ET ferme le p
				if (opening && BlockTags.Contains(name) && stack.Count > 0 && stack[^1] == "p") stack.RemoveAt(stack.Count - 1);
				if (VoidTags.Contains(name) || m.Groups[3].Value == "/") continue;
				if (opening)
				{
					if (name == "li")
					{
						for (var i = stack.Count - 1; i >= 0 && stack[i] != "ul" && stack[i] != "ol"; i--)
						{
							if (stack[i] != "li") continue;
							if (stack.Skip(i + 1).All(OptionalEnd.Contains)) stack.RemoveRange(i, stack.Count - i);
							break;
						}
					}
					stack.Add(name);
					continue;
				}
				var open = stack.LastIndexOf(name);
				if (open < 0)
				{
					defects.Add($"</{name}> orpheline @{m.Index}");
					continue;
				}
				var above = stack.Skip(open + 1).Where(t => !OptionalEnd.Contains(t)).ToList();
				if (above.Count > 0) defects.Add($"</{name}> @{m.Index} ferme par-dessus <{string.Join("><", above)}>");
				stack.RemoveRange(open, stack.Count - open);
			}
			var left = stack.Where(t => !OptionalEnd.Contains(t)).ToList();
			if (left.Count > 0) defects.Add($"non fermé en fin de valeur : <{string.Join("><", left)}>");
			return defects;
		}

		[Theory]
		[InlineData("DNNPlatform/App_GlobalResources/GlobalResources.fr-FR.resx", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("DNNPlatform/App_GlobalResources/GlobalResources.fr-FR.resx", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("DNNPlatform/App_GlobalResources/GlobalResources.resx", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("DNNPlatform/App_GlobalResources/GlobalResources.resx", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("DNNPlatform/App_GlobalResources/GlobalResources.es-ES.resx", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("DNNPlatform/App_GlobalResources/GlobalResources.es-ES.resx", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("DNNPlatform/App_GlobalResources/GlobalResources.pt-BR.resx", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("DNNPlatform/App_GlobalResources/GlobalResources.pt-BR.resx", "MESSAGE_PORTAL_PRIVACY.Text")]
		[InlineData("DNNPlatform/App_GlobalResources/GlobalResources.ru-RU.resx", "MESSAGE_PORTAL_TERMS.Text")]
		[InlineData("DNNPlatform/App_GlobalResources/GlobalResources.ru-RU.resx", "MESSAGE_PORTAL_PRIVACY.Text")]
		public void Html_Is_Well_Nested(string resx, string key)
		{
			var value = ResxValue(resx, key);
			TagParts.Matches(value).Count.Should().BeGreaterThan(100, "auto-contrôle : une valeur sans balises rendrait un vert vide");
			NestingDefects(value).Should().BeEmpty($"{resx} {key} : HTML bien imbriqué");
		}

		/// <summary>Contrôle inverse : les quatre formes corrigées par E3 sont vues, et la fin optionnelle de p/li est tolérée.</summary>
		[Fact]
		public void NestingDefects_Sees_The_E3_Shapes()
		{
			var stray = NestingDefects("<div><p>x</p></div></span>");
			stray.Should().HaveCount(1);
			stray[0].Should().Contain("</span> orpheline");

			var liOverA = NestingDefects("<ul><li><a href=\"#t\"><font>x</font></li></a></ul>");
			liOverA.Should().HaveCount(2, "</li> passe par-dessus <a>, puis </a> n'a plus d'ouvrante");
			liOverA[0].Should().Contain("</li>").And.Contain("<a>");

			var ulOverA = NestingDefects("<ul><li><a href=\"#t\"><font>x</font></ul>");
			ulOverA.Should().HaveCount(1);
			ulOverA[0].Should().Contain("</ul>").And.Contain("<a>");

			var unclosed = NestingDefects("<div><p><a href=\"#top\">x</a></p>");
			unclosed.Should().HaveCount(1);
			unclosed[0].Should().Contain("<div>");

			NestingDefects("<div><p>a<p>b<ul><li>c<li>d</ul><br/><img src=\"i\"></div>")
				.Should().BeEmpty("p et li sans fermante, br et img vides : HTML valide");

			// blockquote ferme le p, wbr est vide : le </p> final n'a plus d'ouvrante
			var blockClosesP = NestingDefects("<p>a<blockquote>b</blockquote><wbr></p>");
			blockClosesP.Should().HaveCount(1);
			blockClosesP[0].Should().Contain("</p> orpheline");

			var hrClosesP = NestingDefects("<p>a<hr></p>");
			hrClosesP.Should().HaveCount(1, "hr est vide et ferme le p");
			hrClosesP[0].Should().Contain("</p> orpheline");
		}
	}
}
