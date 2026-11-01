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
	/// #458 — onglet <b>IP filters</b> de <c>Dnn.Security/App_LocalResources/security.pt-BR.resx</c>.
	///
	/// <para><b>Perimetre derive du composant, pas d'une etiquette.</b> Les 29 cles viennent des
	/// segments <c>IpFiltersPanelBody</c> (12 ids), <c>IpFilterEditor</c> (13) et <c>IpFilterRow</c> (2)
	/// du bundle <c>security-settings-bundle.js</c>, plus les cles partagees que ce flux consomme
	/// (<c>Cancel/Save/Yes/No/DeleteSuccess/DeleteError/cmdAdd</c>). L'etiquette du pool disait 38 :
	/// ecart documente dans la PR. Methode calibree sur l'onglet Member (38 ids = 38 valeurs).</para>
	///
	/// <para><b>Etat livre : 18 valeurs reecrites + 3 cles creees</b> (359 -> 362 cles). Mesure
	/// avant/apres : marqueurs es <b>15 occurrences sur 12 cellules -> 0</b> ; formes historiques
	/// <b>19 -> 0</b>. Six cellules restent identiques a l'es-ES : ce sont des homographes legitimes,
	/// declares et verifies VIVANTS ci-dessous (dont <c>Notas</c>, cree par ce volet — le neutre dit
	/// « Notes » et <c>Notas</c> est la traduction juste des deux cotes).</para>
	///
	/// <para><b>Errata web2 appliques</b> : grille calibree sur l'es-ES du FICHIER restreint aux cles
	/// du neutre (<b>359</b> cles partagees ; <b>4</b> cles du neutre sont absentes de l'es-ES :
	/// <c>TabLoginSettings.Text</c>, <c>ScannerExplanation.Text</c>, <c>ModifiedSettingsExplaination.Text</c>,
	/// <c>ModifiedFilesExplaination.Text</c>), assertion bornee a l'onglet. <b>Six</b> candidats sont
	/// ECARTES par mesure, pas par gout : <c>única</c> (5 cellules pt-BR legitimes), <c>rango</c> (5),
	/// <c>combinará</c> (forme identique en pt), <c>actualizado</c> (88 cellules pt-BR du corpus),
	/// <c>seguro</c> (65), et les homographes <c>permitir</c>/<c>bloquear</c>/<c>máscara</c>. Deux
	/// marqueurs sont des LOCUTIONS, pas des mots : <c>o bloquea</c> (« bloquea » est une sous-chaine
	/// de « bloquear », valide en pt) et <c>se ha podido</c> (« podido » est un participe portugais).</para>
	///
	/// <para><b>Hors perimetre, mesure et nomme</b> : <c>CannotDelete.Text</c> porte encore du castillan
	/// (« regla », « causaría », « por que ») mais le bundle ne la reference <b>jamais</b> (0 occurrence)
	/// — cle morte, laissee en l'etat, pas oubliee.</para>
	/// </summary>
	public class DnnSecurityIpFiltersPtBrGuardTests
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


		/// <summary>Les 29 cles de l'onglet — derivees du composant, pas d'une liste de prefixes.</summary>
		private static readonly string[] TabKeys =
		{
			"AllowIP.Text", "Cancel.Text", "DeleteError.Text",
			"DeleteSuccess.Text", "DenyIP.Text", "FilterType.Header",
			"IPFiltersDisabled.Text", "IPValidation.ErrorMessage", "IpAddress.Header",
			"IpFilterDeletedWarning.Text", "IpFilterUpdateError.Text", "IpFilterUpdateSuccess.Text",
			"No.Text", "Notes.Header", "Notes.Text",
			"Save.Text", "TabIpFilters.Text", "Yes.Text",
			"cmdAdd.Text", "plFirstIP.Help", "plFirstIP.Text",
			"plNotes.Help", "plNotes.Text", "plRuleSpecifity.Help",
			"plRuleSpecifity.Text", "plRuleType.Help", "plRuleType.Text",
			"plSubnet.Help", "plSubnet.Text",
		};

		private static readonly string[] SpanishMarkers =
		{
			"vuelva", "introduzca", "regla", "direcci\u00f3n",
			"\u00e1mbito", "o bloquea", "est\u00e1 seguro", "primera",
			"se ha podido", "identificaci\u00f3n", "direcciones", "m\u00e1scara de red",
			"tipo filtro",
		};

		/// <summary>Formes historiques de l'ONGLET, chacune temoignee par un litteral de l'ancienne
		/// cellule pt (3e element) — une forme sans temoin est une forme devinee, refusee a la generation.</summary>
		private static readonly (string Form, string Why, string Witness)[] HistoricalForms =
		{
			("Vuelva a intentarlo", "castillan", "N\u00e3o foi podido eliminar o filtro. Vuelva a intentarlo."),
			("Introduzca", "castillan", "Introduzca um valor v\u00e1lido de IP/m\u00e1scara."),
			("DIRE\u00e7\u00e3o", "castillan", "DIRE\u00e7\u00e3o IP"),
			("fitro", "castillan", "N\u00e3o foi podido atualizar o fitro. Vuelva a intentarlo."),
			("foi actualizado o filtro", "castillan", "foi actualizado o filtro."),
			("foi eliminado o filtro", "castillan", "foi eliminado o filtro."),
			("Est\u00e1 seguro", "castillan", "Est\u00e1 seguro que quer eliminar este filtro?"),
			("Primera IP", "castillan", "Primera IP"),
			("Tipo de regla", "castillan", "Tipo de regla"),
			("\u00c1mbito da regla", "castillan", "\u00c1mbito da regla"),
			("regla", "castillan", "Mostra se a regla se aplica a uma \u00fanica IP o a um rango de direc\u00e7\u00f5es IP."),
			("rango de", "castillan", "Este valor pode ser um valor \u00fanico de filtro de IP o uma m\u00e1scara de red para calcular um rango de IPs."),
			("m\u00e1scara de red", "castillan", "Este valor pode ser um valor \u00fanico de filtro de IP o uma m\u00e1scara de red para calcular um rango de IPs."),
			("direc\u00e7\u00f5es", "castillan", "Mostra se a regla se aplica a uma \u00fanica IP o a um rango de direc\u00e7\u00f5es IP."),
			("combinar\u00e1", "castillan", "A m\u00e1scara de red se combinar\u00e1 com a primera IP para calcular o rango de IPs a filtrar."),
			("Acesso", "coquille/casse", "Mostra se a regla permite o bloquea o Acesso"),
			("ATEN\u00e7\u00e3o", "castillan", "ATEN\u00e7\u00e3o"),
			("DE IDENTIFICA\u00c7\u00c3O", "castillan", "FILTROS IP DE IDENTIFICA\u00c7\u00c3O"),
			("TIPO FILTRO", "castillan", "TIPO FILTRO"),
		};

		/// <summary>Les 29 cellules livrees : empreinte SHA-256 de la valeur PLEINE (derivee du fichier).</summary>
		private static readonly (string Key, string Sha256)[] Pins =
		{
			("AllowIP.Text", "4bf4e9a66ed74ab784bafbb7e058bda8093c6321d47c048c260cdbe72e182826"),
			("Cancel.Text", "bb9dbb406dcbd5248babc609bb3d021d384398f83dbdb8d53c9b0e32c2aad915"),
			("DeleteError.Text", "0e31d10ae1c4f02efd75c294f7ddbd9c2e853f45034eb150ad12a72b8baf10f5"),
			("DeleteSuccess.Text", "f1d66e97b974143a998c6824e74feb3fe7cb976682562dc89c6ca25619684cca"),
			("DenyIP.Text", "096dc9d4b0204272e51a84015b16a279a67c2fe60a723bdac5bf50f8abf90455"),
			("FilterType.Header", "5ef73749ce8dbd0eca6b417e00e9008bf5d7cc69a94682512807cc0af189c69e"),
			("IPFiltersDisabled.Text", "cb2eeafba1a337d9eecfd9e14b316d007b6fef40a30fafa20e7deca766a7de90"),
			("IPValidation.ErrorMessage", "d54ecef2d8e56b2d14caaf3abdd3f24949b63dfa86ff975d7b2e354a56cfc0bf"),
			("IpAddress.Header", "36e8389343241df62e6289ed161510ad0845c011584a64912e7a5369475f3b4b"),
			("IpFilterDeletedWarning.Text", "f651ff5ed68434bc9d6b17b432a7874620b6bf24bc3163bf6f87f3f8066454e4"),
			("IpFilterUpdateError.Text", "26f544d262c071528f298d385944874173cb21166394bc9230574fcb36a33be1"),
			("IpFilterUpdateSuccess.Text", "a874ed8f7518f166a0521700d39e4111ecce66ba1ce95f73862d79579c57e521"),
			("No.Text", "9bc947e58b62b2fef2954dc2813f3bfe458455fea1bb40b2d7a33e010dfe9ab6"),
			("Notes.Header", "8a6172e21a8758f7768bc353ffdbdb0b4daa4180ee5c82077de9ead26980ee61"),
			("Notes.Text", "0d70728f97c443eb88a7fd9186e27043d961f44810a0920f5f39de0bec85fcf6"),
			("Save.Text", "aef7cd5b2081351dcd4b3767cc84f77797ee1a945e758e8013961c283304b947"),
			("TabIpFilters.Text", "1023acd9dfbd06a6e34a8b9dd7b2e813296841d36c44133bbd09a2119600e0ad"),
			("Yes.Text", "9313455e45aa22430a92fddbb70f21312d3c944ab590dfb1da845c1a96b664fe"),
			("cmdAdd.Text", "39b0e8caabe9a85ece6ade8d378932e3cb5debcda278ee538fffb62a1a0b8783"),
			("plFirstIP.Help", "7f06fa340e609a506cfb88ddfdc4172f7ae9bd4a828dccc069347117ed2eb37a"),
			("plFirstIP.Text", "6dfbbad8f3f50694fc41d3ced312a0aea082b1134a0965698075e9c0449fb4e5"),
			("plNotes.Help", "d35b75b5d951ab7fc0dd8030b261de480f59666ac54c038d80aed679f9f7fccc"),
			("plNotes.Text", "8a6172e21a8758f7768bc353ffdbdb0b4daa4180ee5c82077de9ead26980ee61"),
			("plRuleSpecifity.Help", "4123ff39e1db962d29f088dcd46d2da482657f10dc71bf6f5fdeb84aa9c8dcde"),
			("plRuleSpecifity.Text", "6abe4c504612a69f2e58c91b13f9a116fd34fa3c56f2d6c68dfeb31f1011b4b1"),
			("plRuleType.Help", "737f0c9c8ac8e90ea30ce83aa27a2426b3e6d0c7ad89f537ffaacc7e994cc00b"),
			("plRuleType.Text", "ec062e2c806c6d7d01ca5e46b7322edd6a44a595d335567f4637abfbd93a90d6"),
			("plSubnet.Help", "948be2656d47040e2a309ef83cb13f81e603dc1ea2cbe4aaf306254ed85825f5"),
			("plSubnet.Text", "8ae77c8dd53aaf94daebb487f8f71170fcf83175042d35f126ee8fa169114975"),
		};

		/// <summary>Homographes LEGITIMES : la cellule pt egale l'es parce que le mot est identique
		/// dans les deux langues. Toute entree doit rester VIVANTE (verifie ci-dessous).</summary>
		private static readonly (string Key, string Word, string NeutralWitness)[] DeclaredHomographs =
		{
			("AllowIP.Text", "Permitir", "Allow"),
			("DenyIP.Text", "Bloquear", "Deny"),
			("Cancel.Text", "Cancelar", "Cancel"),
			("Notes.Header", "Notas", "Notes"),
			("plNotes.Text", "Notas", "Notes"),
			("plSubnet.Text", "M\u00e1scara", "Mask"),
		};

		[Fact]
		public void IpFilters_Tab_Keys_All_Present()
		{
			var pt = Load(PtResx);
			var missing = TabKeys.Where(k => !pt.ContainsKey(k)).ToList();
			missing.Should().BeEmpty("#458 : les 29 cles de l'onglet IP filters sont livrees (3 creees : Notes.Header, plNotes.Help, plNotes.Text).");
			TabKeys.Should().HaveCount(29);
			TabKeys.Distinct().Should().HaveCount(29);
		}

		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_File_Level_Es_Restricted_To_Neutral()
		{
			// errata web2 : la grille se calibre sur l'es-ES du FICHIER, restreint aux cles du neutre.
			// Calibrer sur l'onglet avait fabrique l'angle mort « Forzar » de #2085 ; calibrer sur les
			// cles es mortes calibrerait sur du contenu sans equivalent neutre.
			var es = Load(EsResx);
			var neu = Load(NeutralResx);
			var live = es.Where(kv => neu.ContainsKey(kv.Key)).ToList();
			live.Should().HaveCountGreaterThan(300, "es∩neutre : le fichier porte 359 cles partagees");
			neu.Keys.Where(k => !es.ContainsKey(k)).Should().HaveCount(4,
				"mesure : 4 cles du neutre sont absentes de l'es-ES (TabLoginSettings.Text, ScannerExplanation.Text, ModifiedSettingsExplaination.Text, ModifiedFilesExplaination.Text)");
			var hits = live.Sum(kv => SpanishMarkers.Count(m => kv.Value.ToLowerInvariant().Contains(m)));
			hits.Should().BeGreaterThan(45, "mesure : 53 occurrences des 13 marqueurs sur l'es-ES∩neutre — un ecran qui n'en voit aucun n'etablit rien");
			SpanishMarkers.Should().HaveCount(13).And.OnlyHaveUniqueItems();
			SpanishMarkers.Should().NotContain("única", "« única » est un mot portugais valide — ecarte comme enlace/caduca");
			SpanishMarkers.Should().NotContain("bloquea", "« bloquea » est une sous-chaine de « bloquear », valide en pt — d'ou la locution « o bloquea »");
			SpanishMarkers.Should().NotContain("podido", "« podido » est un participe portugais — d'ou la locution « se ha podido »");
		}

		[Fact]
		public void IpFilters_Tab_Carries_No_Spanish_Marker()
		{
			var pt = Load(PtResx);
			var offenders = TabKeys
				.SelectMany(k => SpanishMarkers
					.Where(m => pt[k].ToLowerInvariant().Contains(m))
					.Select(m => $"{k} : marqueur «{m}»"))
				.ToList();
			offenders.Should().BeEmpty("#458 : 15 occurrences sur 12 cellules avant reecriture, 0 apres (mesure)");
		}

		[Fact]
		public void Historical_Forms_Are_Gone_And_Each_Has_A_Witness()
		{
			var pt = Load(PtResx);
			HistoricalForms.Should().HaveCount(19, "liste figee : chaque forme est temoignee");
			foreach (var (form, why, witness) in HistoricalForms)
			{
				// (a) temoin : la forme vivait bien dans l'ancienne cellule (litteral embarque au generateur)
				witness.ToLowerInvariant().Contains(form.ToLowerInvariant()).Should().BeTrue(
					$"le temoin de «{form}» ({why}) doit contenir la forme");
				// (b) la forme a quitte l'onglet
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
				actual.Should().Equal(expected, $"les nombres de {key} sont ceux du neutre (transposition invisible a toute grille)");
				var et = System.Text.RegularExpressions.Regex.Matches(neu[key], tokens).Select(m => m.Value).OrderBy(x => x);
				var at = System.Text.RegularExpressions.Regex.Matches(pt[key], tokens).Select(m => m.Value).OrderBy(x => x);
				at.Should().Equal(et, $"les jetons de {key} sont ceux du neutre");
			}
		}

		[Fact]
		public void Pins_Exact_Values()
		{
			var pt = Load(PtResx);
			Pins.Should().HaveCount(29);
			var offenders = Pins
				.Where(p => !pt.ContainsKey(p.Key) || Sha256Hex(pt[p.Key]) != p.Sha256)
				.Select(p => p.Key)
				.ToList();
			offenders.Should().BeEmpty("#458 : chaque cellule de l'onglet est epinglee — une mutation sans marqueur doit nommer la cle");
		}

		[Fact]
		public void Values_Are_Text_Not_Nested_Xml()
		{
			// lecon M9 de #2085 : une balise brute non echappee devient un XElement enfant et DISPARAIT
			// de .Value — on mesure les NOEUDS, pas la chaine.
			var doc = XDocument.Load(PtResx);
			var offenders = doc.Root!.Elements("data")
				.Where(d => TabKeys.Contains((string)d.Attribute("name")!))
				.Where(d => d.Element("value")!.Nodes().Any(n => n is XElement))
				.Select(d => (string)d.Attribute("name")!)
				.ToList();
			offenders.Should().BeEmpty();
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
			// « atualizar » et « eliminar » : majoritaires dans le corpus pt-BR (79 vs 58 ; 84 vs 43)
			// ET dans l'onglet lui-meme avant reecriture (5 cellules « eliminar »).
			pt["IpFilterUpdateError.Text"].Should().Contain("atualizar", "precedent MemberSettingsError.Text du meme fichier");
			pt["DeleteError.Text"].Should().Contain("eliminar", "precedent Delete.Text / CannotDelete.Text du meme fichier");
			pt["IpFilterUpdateError.Text"].Should().Contain("Tente novamente mais tarde", "precedent MemberSettingsError.Text");
			pt["DeleteError.Text"].Should().Contain("Tente novamente mais tarde");
			pt["IpFilterDeletedWarning.Text"].Should().StartWith("Tem certeza de que deseja",
				"precedent roles.DeleteRole.Confirm (#2102)");
			pt["IPFiltersDisabled.Text"].Should().Contain("Contas de membros",
				"nom de l'onglet tel que TabMemberAccounts.Text le porte dans ce fichier");
			pt["plRuleSpecifity.Help"].Should().Contain("regra").And.NotContain("regla",
				"« regra » est le mot portugais ; « regla » est le castillan");
			pt["plFirstIP.Help"].Should().Contain("sub-rede").And.Contain("intervalo");
			TabKeys.Where(k => pt[k].Contains("regla", StringComparison.Ordinal)).Should().BeEmpty();
			TabKeys.Where(k => pt[k].Contains("actualizar", StringComparison.Ordinal)).Should().BeEmpty(
				"« actualizar » est la forme pt-PT ; le corpus pt-BR ecrit « atualizar » (79 vs 58)");
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
			offenders.Should().BeEmpty("7 cellules egales a l'es avant reecriture ; six le restent legitimement (homographes declares)");
			// l'exemption doit rester VIVANTE : une homographe declaree qui cesse de l'etre est une entree morte
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
			// plancher (pas compte exact) : le compte EXACT vit dans la garde du volet COURANT —
			// sinon chaque onglet suivant de la decoupe casse la garde du precedent (regle #2102,
			// deja appliquee a Registration 359). Login n'ajoute aucune cle -> 362 reste vrai.
			pt.Should().HaveCountGreaterThanOrEqualTo(362,
				"362 cles apres cet onglet (359 en base + 3 creees). Les volets suivants ajoutent les leurs : le compte EXACT vit dans leur garde");
			foreach (var key in new[] { "Security_UserNameMinLength.Text", "RegistrationSettingsError.Text", "TabMemberSettings.Text" })
				pt.Should().ContainKey(key, "les cles des onglets precedents survivent a la reecriture");
		}

		[Fact]
		public void Committed_Blob_Is_Utf8_Bom_With_Lf_Only()
		{
			// Invariant de TRANSPORT sur le blob committé : un runner Windows core.autocrlf=true
			// convertit LF -> CRLF AU CHECKOUT — un fichier sain apparaît en CRLF dans l'arbre.
			var bytes = CommittedBlob("DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Security/App_LocalResources/security.pt-BR.resx");
			bytes.Length.Should().BeGreaterThan(1000, "le blob doit etre le fichier entier, pas un fragment");
			bytes.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "le BOM est celui du fichier d'origine");
			bytes.Count(b => b == 0x0D).Should().Be(0, $"le blob committé de '{PtResx}' est en LF seul");
		}

		private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
	}
}
