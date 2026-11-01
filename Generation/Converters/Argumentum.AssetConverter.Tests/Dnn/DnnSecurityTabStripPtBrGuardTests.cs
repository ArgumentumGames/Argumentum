using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 — volet <b>barre d'onglets</b> de <c>Dnn.Security/App_LocalResources/security.pt-BR.resx</c>.
	///
	/// <para><b>Perimetre derive du composant.</b> Les cles viennent de deux segments du bundle
	/// <c>security-settings-bundle.js</c> : <c>Body</c> (offsets 68551..77859, 17 appels
	/// <c>.get()</c> dont 11 encore non livres) et <c>App</c> (61061..62265, 1 appel :
	/// <c>nav_Security</c>), soit <b>12 cles</b>. Resolution K puis K.Text : les 12 existent en
	/// <c>.Text</c>. Les 8 defauts mesures sont tous espagnols ou hybrides.</para>
	///
	/// <para><b>Etat libre : 8 valeurs reecrites, 0 cle creee</b> (362 -&gt; 362). Deux cellules
	/// sont l'es verbatim (<c>AUDITORÍA</c>, <c>Más</c>), trois sont des hybrides qui n'existent
	/// dans aucune des deux langues (<c>MÁS OPÇÕES DE SEGURIDADE</c>, <c>Análisis de
	/// seguridade</c>, <c>Boletines de seguridade</c>), une porte « Seguridade », hors pt-BR
	/// comme hors es-ES, et la phrase d'aide empile trois castillanismes (« parámetro »,
	/// « cambios », « afectarán ») sur une syntaxe elle-meme calquee de l'es-ES, qui omet le
	/// verbe (« Este un parámetro ») : le « é » manquant est donc un heritage, pas une
	/// coquille propre au pt.</para>
	///
	/// <para><b>Ce volet nomme aussi un non-item</b> : <c>TabBasicLoginSettings.Text</c>,
	/// cite par le pool comme prochaine tache, etait DEJA correct
	/// (« CONFIGURAÇÃO BÁSICA DE IDENTIFICAÇÃO »). Il est epingle ici pour ne pas regresser,
	/// mais il n'a demande aucune reecriture — l'etiquette du pool pointait une cle saine.</para>
	///
	/// <para><b>Un residu mesure, volontairement NON corrige</b> : <c>TabSuperuserActivity.Text</c>
	/// porte « ACTIVIDADE », qui est du <b>pt-PT</b> (bresilien : « atividade »). Ce n'est pas une
	/// contamination espagnole : la mesure du corpus donne <b>25 « actividade » contre 1
	/// « atividade »</b> dans les 140 resx pt-BR, et <b>8 formes dans ce seul fichier</b>. Corriger
	/// une cellule isolerait l'onglet de son propre panneau : le variant est systematique et
	/// appartient a son propre passage. La garde epingle la valeur MESUREE, pas la valeur souhaitee —
	/// ce test a d'ailleurs attrape une faute de lecture du generateur (« ATIVIDADE » au lieu de
	/// « ACTIVIDADE »), preuve que l'epingle se derive de la mesure et jamais de l'intention.</para>
	///
	/// <para><b>Marqueurs</b> : 12, chacun verifie NON AVEUGLE (present dans l'es des onglets)
	/// et ABSENT du pt livre. « más » et « auditoría » ne sont pas des mots portugais, et
	/// « parámetro »/« parâmetro » different par la voyelle accentuee : le marqueur ne peut
	/// donc pas se declencher sur la correction elle-meme.</para>
	/// </summary>
	public class DnnSecurityTabStripPtBrGuardTests
	{
		private static string PtResx => Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "DesktopModules", "Admin", "Dnn.PersonaBar", "Modules",
			"Dnn.Security", "App_LocalResources", "security.pt-BR.resx");

		private static string NeutralResx => Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "DesktopModules", "Admin", "Dnn.PersonaBar", "Modules",
			"Dnn.Security", "App_LocalResources", "security.resx");

		private static string EsResx => Path.Combine(
			TestRepoRoot.Find(), "DNNPlatform", "DesktopModules", "Admin", "Dnn.PersonaBar", "Modules",
			"Dnn.Security", "App_LocalResources", "security.es-ES.resx");

		private static Dictionary<string, string> Load(string path)
		{
			var root = XDocument.Load(path).Root!;
			return root.Elements("data")
				.ToDictionary(d => (string)d.Attribute("name")!, d => (string?)d.Element("value") ?? string.Empty,
					StringComparer.Ordinal);
		}

		private static string Sha256Hex(string cell)
		{
			using var sha = SHA256.Create();
			return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(cell))).ToLowerInvariant();
		}

		/// <summary>Les 12 cles de la barre d'onglets — derivees des composants Body et App.</summary>
		private static readonly string[] TabKeys =
		{
			"GlobalSettingsTab.Text", "nav_Security.Text", "TabAuditChecks.Text",
			"TabBasicLoginSettings.Text", "TabLoginSettings.Text", "TabMore.Text",
			"TabMoreSecuritySettings.Text", "TabRegistrationSettings.Text", "TabScannerCheck.Text",
			"TabSecurityAnalyzer.Text", "TabSecurityBulletins.Text", "TabSuperuserActivity.Text",
		};

		/// <summary>Marqueurs espagnols : chacun est present dans l'es des onglets (verifie par
		/// <c>Marker_Grid_Is_Non_Vacuous</c>) et absent du pt livre.</summary>
		private static readonly string[] SpanishMarkers =
		{
			"afectar\u00e1n", "an\u00e1lisis", "auditor\u00eda", "boletines",
			"cambios", "configuraci\u00f3n", "esc\u00e1ner", "identificaci\u00f3n",
			"m\u00e1s", "par\u00e1metro", "seguridad", "verificaci\u00f3n",
		};

		/// <summary>Formes historiques de l'ONGLET, chacune temoignee par le litteral de l'ancienne
		/// cellule pt (3e element). Le generateur refuse une forme sans temoin et une forme survivante.</summary>
		private static readonly (string Form, string Why, string Witness)[] HistoricalForms =
		{
			("par\u00e1metro", "castillan dans la phrase d'aide (pt-BR : \u00ab par\u00e2metro \u00bb)", "Este um par\u00e1metro global. Os cambios afectar\u00e1n a todos os portais da instala\u00e7\u00e3o."),
			("cambios", "castillan \u00ab cambios \u00bb (pt-BR : \u00ab altera\u00e7\u00f5es \u00bb)", "Este um par\u00e1metro global. Os cambios afectar\u00e1n a todos os portais da instala\u00e7\u00e3o."),
			("afectar\u00e1n", "futur castillan (pt-BR : \u00ab afetar\u00e3o \u00bb)", "Este um par\u00e1metro global. Os cambios afectar\u00e1n a todos os portais da instala\u00e7\u00e3o."),
			("Seguridade", "forme hors pt-BR (pt-BR : \u00ab Seguran\u00e7a \u00bb)", "Seguridade"),
			("AUDITOR\u00cdA", "cellule egale a l'es-ES verbatim", "AUDITOR\u00cdA"),
			("M\u00e1s", "cellule egale a l'es-ES verbatim (pt-BR : \u00ab Mais \u00bb)", "M\u00e1s"),
			("M\u00c1S OP\u00c7\u00d5ES DE SEGURIDADE", "hybride : \u00ab M\u00c1S \u00bb es + \u00ab OP\u00c7\u00d5ES \u00bb pt + \u00ab SEGURIDADE \u00bb hors pt", "M\u00c1S OP\u00c7\u00d5ES DE SEGURIDADE"),
			("ESC\u00c1NER", "graphie castillane (pt : le neutre dit \u00ab SCANNER \u00bb)", "VERIFICA\u00c7\u00c3O DE ESC\u00c1NER"),
			("An\u00e1lisis de seguridade", "hybride : \u00ab An\u00e1lisis \u00bb es + \u00ab seguridade \u00bb hors pt", "An\u00e1lisis de seguridade"),
			("Boletines de seguridade", "hybride : \u00ab Boletines \u00bb es + \u00ab seguridade \u00bb hors pt", "Boletines de seguridade"),
		};

		/// <summary>Les 12 cellules livrees : empreinte SHA-256 de la valeur PLEINE.</summary>
		private static readonly (string Key, string Sha256)[] Pins =
		{
			("GlobalSettingsTab.Text", "f72ae65f60d0c91709c213f680a9c5a8630aa4d76bdafa8ccc362d3012f19c68"),
			("nav_Security.Text", "d5668a0b8310b59f8af527bf66f56c37aa35a7d4868fea6986f9dae9d908d3fc"),
			("TabAuditChecks.Text", "d99f95ebb121716c4e1b06b131552d98fb32e3934fa1f8e3d59282f0676d89e5"),
			("TabBasicLoginSettings.Text", "c596bfec677f15e93c62102746b70730931b360c93d1516da9639801851de3fa"),
			("TabLoginSettings.Text", "5eb0c10d8e89a05791237b42353712126e24d4de659e954ab2776ba293e86abb"),
			("TabMore.Text", "962d86dfa1c5d9b119ebc139c077e4494477b25f2fd3c26deee4caec76034b64"),
			("TabMoreSecuritySettings.Text", "76a5b60fd828a4b59e8886440d4e280bdd2b7dd0502043501ea6f27f6d480f69"),
			("TabRegistrationSettings.Text", "79a19fa431b1eb2519635481674424c5465bdaeef857d1e3622d22fc8187c603"),
			("TabScannerCheck.Text", "ed7f8d08b987a63a2f69b693c00c602b0e02e720f053c9da4c7c8a67361d2142"),
			("TabSecurityAnalyzer.Text", "48a9b93b1716d2b4d2a071d0ae777e4d5073921a0bc6ab97ba608e1c41e62f4d"),
			("TabSecurityBulletins.Text", "975a786b81b36d1dd719554ceb9fa640dce51620a353d589a7a59723463b8820"),
			("TabSuperuserActivity.Text", "be051b5dd284a0d10c88e661e54d985acb7042db6071bca9f98197c77c0800d6"),
		};

		/// <summary>Homographes LEGITIMES (calcule par le generateur contre l'es-ES).</summary>
		private static readonly (string Key, string Word, string NeutralWitness)[] DeclaredHomographs =
		{

		};

		[Fact]
		public void Tab_Strip_Keys_All_Present()
		{
			var pt = Load(PtResx);
			TabKeys.Where(k => !pt.ContainsKey(k)).Should().BeEmpty(
				"#458 : les 12 cles de la barre d'onglets sont livrees (aucune creation dans ce volet)");
			TabKeys.Should().HaveCount(12);
			TabKeys.Distinct().Should().HaveCount(12);
		}

		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_The_Tab_Es()
		{
			var es = Load(EsResx);
			var blind = SpanishMarkers
				.Where(m => !TabKeys.Any(k => es.ContainsKey(k) && es[k].ToLowerInvariant().Contains(m)))
				.ToList();
			blind.Should().BeEmpty("un marqueur aveugle sur l'es calibrerait l'ecran sur du vide");
			SpanishMarkers.Should().HaveCount(12).And.OnlyHaveUniqueItems();
			SpanishMarkers.Should().NotContain(new[] { "mais", "segurança", "configuração", "análise", "boletins",
				"identificação", "verificação", "parâmetro" },
				"les mots PORTUGAIS de la correction ne sont jamais des marqueurs : ils vivent dans la cellule livree");
		}

		[Fact]
		public void Tab_Strip_Carries_No_Spanish_Marker()
		{
			var pt = Load(PtResx);
			var offenders = TabKeys
				.SelectMany(k => SpanishMarkers
					.Where(m => pt[k].ToLowerInvariant().Contains(m))
					.Select(m => $"{k} : marqueur «{m}»"))
				.ToList();
			offenders.Should().BeEmpty("#458 : 8 cellules reecrites, 0 marqueur restant (mesure)");
		}

		[Fact]
		public void Historical_Forms_Are_Gone_And_Each_Has_A_Witness()
		{
			var pt = Load(PtResx);
			HistoricalForms.Should().HaveCount(10, "liste figee : chaque forme est temoignee");
			foreach (var (form, why, witness) in HistoricalForms)
			{
				witness.ToLowerInvariant().Contains(form.ToLowerInvariant()).Should().BeTrue(
					$"le temoin de «{form}» ({why}) doit contenir la forme — sinon la forme est devinee");
				TabKeys.Where(k => pt[k].Contains(form, StringComparison.Ordinal)).Should().BeEmpty(
					$"forme historique «{form}» ({why}) reintroduite");
			}
		}

		[Fact]
		public void Numeric_Bounds_And_Tokens_Match_The_Neutral()
		{
			var pt = Load(PtResx);
			var neu = Load(NeutralResx);
			const string tokens = @"\{\d\}|\[[A-Z]+\]";
			foreach (var key in TabKeys)
			{
				var expected = System.Text.RegularExpressions.Regex.Matches(neu[key], @"\d+").Select(m => m.Value);
				var actual = System.Text.RegularExpressions.Regex.Matches(pt[key], @"\d+").Select(m => m.Value);
				actual.Should().Equal(expected, $"les nombres de {key} sont ceux du neutre");
				var et = System.Text.RegularExpressions.Regex.Matches(neu[key], tokens).Select(m => m.Value).OrderBy(x => x);
				var at = System.Text.RegularExpressions.Regex.Matches(pt[key], tokens).Select(m => m.Value).OrderBy(x => x);
				at.Should().Equal(et, $"les jetons de {key} sont ceux du neutre");
			}
		}

		[Fact]
		public void Pins_Exact_Values()
		{
			var pt = Load(PtResx);
			Pins.Should().HaveCount(12);
			var offenders = Pins
				.Where(p => !pt.ContainsKey(p.Key) || Sha256Hex(pt[p.Key]) != p.Sha256)
				.Select(p => p.Key)
				.ToList();
			offenders.Should().BeEmpty("#458 : chaque cellule de la barre d'onglets est epinglee");
		}

		[Fact]
		public void Values_Are_Text_Not_Nested_Xml()
		{
			var doc = XDocument.Load(PtResx);
			var offenders = doc.Root!.Elements("data")
				.Where(d => TabKeys.Contains((string)d.Attribute("name")!))
				.Where(d => d.Element("value")!.Nodes().Any(n => n is XElement))
				.Select(d => (string)d.Attribute("name")!)
				.ToList();
			offenders.Should().BeEmpty("une balise brute non echappee devient un XElement enfant et disparait de .Value");
		}

		[Fact]
		public void No_Space_Before_Punctuation()
		{
			var pt = Load(PtResx);
			var offenders = TabKeys
				.Select(k => (k, v: System.Text.RegularExpressions.Regex.Match(pt[k], @" [.,;:!?]")))
				.Where(x => x.v.Success)
				.Select(x => $"{x.k} : «{x.v.Value}»")
				.ToList();
			offenders.Should().BeEmpty();
		}

		[Fact]
		public void Terminology_Is_PtBr()
		{
			var pt = Load(PtResx);
			pt["nav_Security.Text"].Should().Be("Segurança",
				"l'ancienne cellule « Seguridade » n'est ni pt-BR ni es-ES (« seguridad »)");
			pt["TabAuditChecks.Text"].Should().Be("AUDITORIA",
				"l'es-ES porte l'accent tonique « AUDITORÍA » ; le pt-BR n'accentue pas ce mot");
			pt["TabMore.Text"].Should().Be("Mais",
				"« Más » etait l'es-ES verbatim");
			pt["TabMoreSecuritySettings.Text"].Should().Be("MAIS OPÇÕES DE SEGURANÇA",
				"« MÁS … SEGURIDADE » : un castillan et une forme hors pt-BR dans la meme cellule");
			pt["TabSecurityAnalyzer.Text"].Should().Be("Análise de segurança",
				"« Análisis de seguridade » etait un hybride absent des deux langues");
			pt["TabSecurityBulletins.Text"].Should().Be("Boletins de segurança",
				"« Boletines de seguridade » etait un hybride absent des deux langues");
			pt["TabScannerCheck.Text"].Should().Contain("SCANNER").And.NotContain("ESCÁNER",
				"aucune des deux graphies n'avait d'occurrence etablie dans les 140 resx pt-BR : " +
				"le neutre nomme l'appareil « SCANNER », c'est lui qui tranche, pas l'orthographe es");
			pt["GlobalSettingsTab.Text"].Should().Contain("Este é um parâmetro global")
				.And.Contain("alterações").And.Contain("afetarão").And.NotContain("cambios");
			pt["TabBasicLoginSettings.Text"].Should().Be("CONFIGURAÇÃO BÁSICA DE IDENTIFICAÇÃO",
				"cle nommee par le pool : elle etait DEJA correcte avant ce volet (aucune reecriture)");
			pt["TabSuperuserActivity.Text"].Should().Be("ACTIVIDADE DE SUPERUSUÁRIOS",
				"cellule NON touchee par ce volet : « actividade » est du pt-PT, pas une contamination espagnole. " +
				"Mesure : 25 « actividade » contre 1 « atividade » dans les 140 resx pt-BR, et 8 formes dans ce " +
				"seul fichier — variant systematique, renvoye a son propre passage : corriger une seule cellule " +
				"creerait une divergence avec le panneau qui porte la meme forme");
		}

		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell_Except_Declared_Homographs()
		{
			var pt = Load(PtResx);
			var es = Load(EsResx);
			var declared = DeclaredHomographs.Select(h => h.Key).ToHashSet(StringComparer.Ordinal);
			var offenders = TabKeys
				.Where(k => es.ContainsKey(k) && pt[k] == es[k] && !declared.Contains(k))
				.ToList();
			offenders.Should().BeEmpty("AUDITORÍA et Más etaient l'es verbatim ; aucun ne le reste");
			var neu = Load(NeutralResx);
			foreach (var (key, word, neutralWitness) in DeclaredHomographs)
			{
				pt[key].Should().Be(word);
				es[key].Should().Be(word, "un homographe declare doit etre identique dans les deux langues");
				neu[key].Should().Be(neutralWitness, "temoin neutre de l'homographe");
			}
		}

		[Fact]
		public void Legacy_Keys_Survive_The_Rewrite()
		{
			var pt = Load(PtResx);
			// plancher (pas compte exact) : le compte EXACT vivrait dans la garde du volet COURANT
			// si ce volet creait des cles. Il n'en cree aucune -> 362, inchange depuis #2107.
			pt.Should().HaveCountGreaterThanOrEqualTo(362,
				"362 cles depuis #2107 (Login). La barre d'onglets n'ajoute aucune cle");
			foreach (var key in new[] { "Security_RequireValidProfileAtLogin.Text", "BasicLoginSettingsUpdateSuccess",
				"IpFilterUpdateError.Text", "plMaxUploadSize.Text", "OtherSettingsUpdateSuccess.Text",
				"BulletinsDoNotExist.Text" })
				pt.Should().ContainKey(key, "les cles des volets precedents survivent a la reecriture");
		}

		[Fact]
		public void Committed_Blob_Is_Utf8_Bom_With_Lf_Only()
		{
			// Invariant de TRANSPORT sur le blob committe : un runner Windows core.autocrlf=true
			// convertit LF -> CRLF AU CHECKOUT — un fichier sain apparait en CRLF dans l'arbre.
			var bytes = CommittedBlob("DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Security/App_LocalResources/security.pt-BR.resx");
			bytes.Length.Should().BeGreaterThan(1000, "le blob doit etre le fichier entier, pas un fragment");
			bytes.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "le BOM est celui du fichier d'origine");
			bytes.Count(b => b == 0x0D).Should().Be(0, "le blob committe est en LF seul");
		}

		private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
	}
}
