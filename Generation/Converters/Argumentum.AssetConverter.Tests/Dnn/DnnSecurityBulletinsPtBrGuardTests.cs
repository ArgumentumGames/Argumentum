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
	/// #458 — onglet <b>Bulletins</b> de <c>Dnn.Security/App_LocalResources/security.pt-BR.resx</c>.
	///
	/// <para><b>Perimetre derive du composant.</b> Les cles viennent du segment
	/// <c>SecurityBulletinsPanelBody</c> du bundle <c>security-settings-bundle.js</c> (offsets
	/// 191705..197990) : <b>4 appels <c>.get()</c></b>. Le segment <c>BulletinItemRow</c>
	/// (199319..203824) ne porte <b>aucun</b> appel <c>.get()</c> : ses chaines arrivent par
	/// props depuis le parent. L'etiquette du pool disait « 6 » : 6e ecart de la decoupe, la
	/// mesure reste la regle. Resolution K puis K.Text : les 4 existent en <c>.Text</c>.</para>
	///
	/// <para><b>Etat libre : 4 valeurs reecrites, 0 cle creee</b> (362 -&gt; 362). Quatrieme
	/// onglet consécutif marque par l'espagnol : « BOLETINES » et « ENLACES » (es verbatim),
	/// la phrase finale avec « boletín » espagnol laisse intraduit, et l'hybride
	/// « DESCRIPção » qui n'existe dans AUCUNE des deux langues, plus « seguridade », hors
	/// es-ES (« seguridad ») et hors pt-BR (« segurança »). Reecriture minimale : seuls les
	/// mots fautifs changent, la structure etablie de la phrase reste.</para>
	///
	/// <para><b>Marqueurs</b> : 7, chacun verifie NON AVEUGLE (present dans l'es de l'onglet)
	/// et ABSENT du pt livre. Les hybrides absents de l'es-ES (<c>DESCRIPção</c>,
	/// <c>seguridade</c>) sont des formes historiques, jamais des marqueurs — un marqueur
	/// aveugle calibrerait l'ecran sur du vide.</para>
	/// </summary>
	public class DnnSecurityBulletinsPtBrGuardTests
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

		/// <summary>Les 4 cles de l'onglet — derivees du composant, pas d'une liste de prefixes.</summary>
		private static readonly string[] TabKeys =
		{
			"Bulletins.Text", "BulletinDescription.Text", "BulletinLink.Text",
			"BulletinsDoNotExist.Text",
		};

		/// <summary>Marqueurs espagnols de l'onglet : chacun est present dans l'es-ES de l'onglet
		/// (verifie par <c>Marker_Grid_Is_Non_Vacuous</c>) et absent du pt livre.</summary>
		private static readonly string[] SpanishMarkers =
		{
			"boletines", "bolet\u00edn", "descripci\u00f3n", "enlaces",
			"ning\u00fan", "no se ha encontrado", "seguridad",
		};

		/// <summary>Formes historiques de l'ONGLET, chacune temoignee par le litteral de l'ancienne
		/// cellule pt (3e element). Le generateur refuse une forme sans temoin et une forme survivante.</summary>
		private static readonly (string Form, string Why, string Witness)[] HistoricalForms =
		{
			("BOLETINES", "castillan (pt-BR : \u00ab BOLETINS \u00bb)", "BOLETINES"),
			("DESCRIP\u00e7\u00e3o", "hybride es/pt (es \u00ab DESCRIPCI\u00d3N \u00bb, pt-BR \u00ab DESCRI\u00c7\u00c3O \u00bb)", "DESCRIP\u00e7\u00e3o"),
			("ENLACES", "cellule egale a l'es-ES verbatim avant reecriture", "ENLACES"),
			("bolet\u00edn", "castillan laisse intraduit dans la phrase", "N\u00e3o foi encontrado nenhum bolet\u00edn de seguridade para a vers\u00e3o de DNN Platform {0}."),
			("seguridade", "forme hors es-ES et hors pt-BR (\u00ab seguran\u00e7a \u00bb)", "N\u00e3o foi encontrado nenhum bolet\u00edn de seguridade para a vers\u00e3o de DNN Platform {0}."),
		};

		/// <summary>Les 4 cellules livrees : empreinte SHA-256 de la valeur PLEINE (derivee du fichier).</summary>
		private static readonly (string Key, string Sha256)[] Pins =
		{
			("Bulletins.Text", "0e84610d0aa08f2c290e5a5e9005584135beb9f41611c3e02407b555b73e15ea"),
			("BulletinDescription.Text", "92a585e0ef9647e398b0904c296ee6aa36762ae88bf2ee78d8fbc327d1491863"),
			("BulletinLink.Text", "e26ef19029cc92e234d9457823ca0811b92db9ce9dbf8db99c0fdd75d11338f0"),
			("BulletinsDoNotExist.Text", "0f963d2680543e1bd456e3d002ab66f357e202f855470c6761317b09f8d5781a"),
		};

		/// <summary>Homographes LEGITIMES : aucune — apres reecriture, aucune cellule de l'onglet
		/// n'egale l'es-ES (verifie ci-dessous). La liste reste vide par construction.</summary>
		private static readonly (string Key, string Word, string NeutralWitness)[] DeclaredHomographs =
		{

		};

		[Fact]
		public void Bulletins_Tab_Keys_All_Present()
		{
			var pt = Load(PtResx);
			TabKeys.Where(k => !pt.ContainsKey(k)).Should().BeEmpty(
				"#458 : les 4 cles de l'onglet Bulletins sont livrees (aucune creation dans ce volet)");
			TabKeys.Should().HaveCount(4);
			TabKeys.Distinct().Should().HaveCount(4);
		}

		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_The_Tab_Es()
		{
			var es = Load(EsResx);
			var blind = SpanishMarkers
				.Where(m => !TabKeys.Any(k => es[k].ToLowerInvariant().Contains(m)))
				.ToList();
			blind.Should().BeEmpty("un marqueur aveugle sur l'es calibrerait l'ecran sur du vide");
			SpanishMarkers.Should().HaveCount(7).And.OnlyHaveUniqueItems();
			SpanishMarkers.Should().NotContain("versão",
				"« versão » est un mot portugais valide (il VIT dans la phrase livree) : jamais un marqueur");
		}

		[Fact]
		public void Bulletins_Tab_Carries_No_Spanish_Marker()
		{
			var pt = Load(PtResx);
			var offenders = TabKeys
				.SelectMany(k => SpanishMarkers
					.Where(m => pt[k].ToLowerInvariant().Contains(m))
					.Select(m => $"{k} : marqueur «{m}»"))
				.ToList();
			offenders.Should().BeEmpty("#458 : 4 cellules reecrites, 0 marqueur restant (mesure)");
		}

		[Fact]
		public void Historical_Forms_Are_Gone_And_Each_Has_A_Witness()
		{
			var pt = Load(PtResx);
			HistoricalForms.Should().HaveCount(5, "liste figee : chaque forme est temoignee");
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
				at.Should().Equal(et, $"les jetons de {key} sont ceux du neutre — « version {{0}} » reste « versão {{0}} »");
			}
		}

		[Fact]
		public void Pins_Exact_Values()
		{
			var pt = Load(PtResx);
			Pins.Should().HaveCount(4);
			var offenders = Pins
				.Where(p => !pt.ContainsKey(p.Key) || Sha256Hex(pt[p.Key]) != p.Sha256)
				.Select(p => p.Key)
				.ToList();
			offenders.Should().BeEmpty("#458 : chaque cellule de l'onglet est epinglee");
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
			pt["Bulletins.Text"].Should().Be("BOLETINS",
				"en-tete de colonne en capitales comme le neutre « BULLETINS » ; l'ancienne cellule etait « BOLETINES » (es)");
			pt["BulletinDescription.Text"].Should().Be("DESCRIÇÃO",
				"l'ancienne cellule « DESCRIPção » etait un hybride absent des deux langues");
			pt["BulletinLink.Text"].Should().Be("LINK",
				"le NEUTRE lui-meme dit « LINK » : l'emprunt est la forme etablie du pt-BR, l'es « ENLACES » etait la deviation");
			pt["BulletinsDoNotExist.Text"].Should().Contain("boletim de segurança")
				.And.NotContain("boletín").And.NotContain("seguridade",
				"« boletim » et « segurança » : les deux mots que l'ancienne phrase portait en espagnol/galicien");
			pt["BulletinsDoNotExist.Text"].Should().Contain("{0}",
				"le jeton de version du neutre est preserve");
			pt["BulletinsDoNotExist.Text"].Should().Contain("versão",
				"mot portugais valide — la colonne homographe es « versión » n'y touche pas");
		}

		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell_Except_Declared_Homographs()
		{
			var pt = Load(PtResx);
			var es = Load(EsResx);
			var declared = DeclaredHomographs.Select(h => h.Key).ToHashSet(StringComparer.Ordinal);
			var offenders = TabKeys
				.Where(k => pt[k] == es[k] && !declared.Contains(k))
				.ToList();
			offenders.Should().BeEmpty(
				"deux cellules egales a l'es avant reecriture (BOLETINES, ENLACES) ; aucune ne le reste legitimement");
			DeclaredHomographs.Should().BeEmpty("ce volet ne declare aucun homographe (mesure)");
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
			// si ce volet creait des cles. Il n'en cree aucune -> 362 ici, inchange depuis #2107.
			pt.Should().HaveCountGreaterThanOrEqualTo(362,
				"362 cles depuis #2107 (Login). Bulletins n'ajoute aucune cle : 362 reste vrai");
			foreach (var key in new[] { "Security_RequireValidProfileAtLogin.Text", "BasicLoginSettingsUpdateSuccess",
				"IpFilterUpdateError.Text", "plMaxUploadSize.Text", "OtherSettingsUpdateSuccess.Text" })
				pt.Should().ContainKey(key, "les cles des volets precedents survivent a la reecriture");
		}

		[Fact]
		public void Committed_Blob_Is_Utf8_Bom_With_Lf_Only()
		{
			// Invariant de TRANSPORT sur le blob committe : un runner Windows core.autocrlf=true
			// convertit LF -> CRLF AU CHECKOUT — un fichier sain apparait en CRLF dans l'arbre.
			// Volet Bulletins : premiere garde du sweep posee DIRECTEMENT sur DnnResxGuardSocle (#2112).
			var bytes = CommittedBlob("DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Security/App_LocalResources/security.pt-BR.resx");
			bytes.Length.Should().BeGreaterThan(1000, "le blob doit etre le fichier entier, pas un fragment");
			bytes.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "le BOM est celui du fichier d'origine");
			bytes.Count(b => b == 0x0D).Should().Be(0, "le blob committe est en LF seul");
		}

		private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
	}
}
