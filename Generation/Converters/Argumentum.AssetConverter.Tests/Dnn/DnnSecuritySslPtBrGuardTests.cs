using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du sweep pt-BR (pool #458) : <c>Dnn.Security/App_LocalResources/security.pt-BR.resx</c>,
	/// onglet <b>SSL</b> — 21 valeurs, l'unite de decoupe arbitree (un onglet, &lt;= 120 valeurs par PR).
	/// <para>
	/// Ce fichier porte les deux defauts a la fois, mais pas sur les memes cles :
	/// <b>9 cles sur 21 etaient absentes</b> (43 % de l'onglet s'affichait en anglais dans une UI
	/// pt-BR : <c>SslOn</c>, <c>SslOff</c>, <c>SslAdvanced</c>, <c>SslSetAllPagesSecure</c>), et
	/// <b>12 valeurs sur 12 etaient macaroniques</b> — espagnol a mots portugais
	/// (« Valor de cabecera SSL Offloading », « Está seguro que quer cancelar os cambios? »,
	/// « Não foi podido atualizar a configuração SSL. Vuelva a intentarlo. »).
	/// </para>
	/// <para>
	/// ⚠️ Une des valeurs ne dit pas seulement mal : elle dit <b>autre chose</b>.
	/// <c>plSSLEnforced.Text</c> portait « Forzar SSL » (SSL force) pour un neutre
	/// « Mixed Mode » : un administrateur lisant le pt-BR reglait un mode different de celui
	/// qu'il croyait. Le pin ci-dessous est le seul temoin possible de ce defaut —
	/// « Forzar » n'existe nulle part dans l'es-ES de ce fichier, donc aucun marqueur
	/// d'absence ne pouvait l'attraper.
	/// </para>
	/// <para>
	/// Terminologie adossee aux fichiers pt-BR <b>deja merges</b> : <c>Ativado</c>/<c>Desativado</c>
	/// (users.pt-BR.resx, <c>SwitchOn</c>/<c>SwitchOff</c>), <c>Tem certeza de que deseja</c>
	/// (SharedResources, users), <c>Tente novamente mais tarde</c> (roles, users), <c>Marque esta
	/// opção</c> (roles), <c>Não foi possível</c> (users, roles). Deux choix sans precedent,
	/// declares : <c>Modo misto</c> (0 occurrence — mais « modo mixto » est un marqueur espagnol)
	/// et <c>balanceador de rede</c> (0 occurrence). <c>cabeçalho</c> s'appuie sur
	/// ModuleSettings.ascx.pt-BR.resx : les 9 occurrences de « cabecera » du corpus vivent dans
	/// sitesettings/servers/pages, trois fichiers encore macaroniques — c'est la contamination,
	/// pas un precedent.
	/// </para>
	/// <para>
	/// Portee : les assertions de marqueurs et de pins sont <b>bornees a l'onglet SSL</b>.
	/// 121 autres cellules du meme fichier portent encore de l'espagnol (autres onglets, hors
	/// decoupe) : une assertion sur le fichier entier serait rouge aujourd'hui, a juste titre,
	/// mais elle ne mesurerait pas ce que cette PR corrige.
	/// </para>
	/// </summary>
	public class DnnSecuritySslPtBrGuardTests
	{
		private const string PtResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Security/App_LocalResources/security.pt-BR.resx";
		private const string EsResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Security/App_LocalResources/security.es-ES.resx";
		private const string NeutralResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Security/App_LocalResources/Security.resx";

		private static string Path_(string rel) =>
			Path.Combine(TestRepoRoot.Find(), rel.Replace('/', Path.DirectorySeparatorChar));

		private static XDocument Load(string rel)
		{
			var path = Path_(rel);
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({rel})");
			return XDocument.Load(path);
		}

		private static Dictionary<string, string> Cells(string rel) =>
			Load(rel).Root!.Elements("data")
				.ToDictionary(d => d.Attribute("name")!.Value, d => d.Element("value")?.Value ?? "");

		// L'onglet SSL : l'unite de decoupe de cette PR.
		private static readonly string[] SslKeys =
		{
			"TabSslSettings.Text",
			"plSSLEnforced.Text",
			"plSSLEnforced.Help",
			"plSSLURL.Text",
			"plSSLURL.Help",
			"plSTDURL.Text",
			"plSTDURL.Help",
			"plSSLOffload.Text",
			"plSSLOffload.Help",
			"SslSettingsUpdateSuccess.Text",
			"SslSettingsError.Text",
			"SslSettingsRestoreWarning.Text",
			"SslOn.Text",
			"SslOn.Help",
			"SslOff.Text",
			"SslOff.Help",
			"SslAdvanced.Text",
			"SslAdvanced.Help",
			"SslSetAllPagesSecure.Text",
			"SslSetAllPagesSecure.Confirm",
			"SslSetAllPagesSecure.Completed",
		};

		// Grille calibree SUR L'ES-ES DE CET ONGLET (calibrate_ssl.py) : chaque marqueur
		// touche au moins une cellule es-ES d'ici, sinon il rendrait un 0 qui ne prouve
		// rien. « inseguras » est ECARTE : portugais valide (« paginas inseguras »).
		private static readonly string[] SpanishMarkers =
		{
			"accesible",
			"cabecera",
			"comprobar",
			"procesamiento",
			"podido",
			"vuelva",
			"cambios",
			"está seguro",
			"estándar",
			"necesitará",
			"sitio",
			"ejemplo",
			"alojamiento",
			"suministra",
			"dirección",
			"opción",
			"configuración",
			"cuenta",
			"redirijan",
			"establecer",
			"desea",
			"tiene",
			"puede",
			"debe",
			"nombre",
			"mixto",
		};

		[Fact]
		public void Ssl_Tab_Keys_Are_All_Present_In_Pt()
		{
			var pt = Cells(PtResx);
			var absent = SslKeys.Where(k => !pt.ContainsKey(k)).ToArray();
			absent.Should().BeEmpty(
				"9 des 21 cles de l'onglet SSL manquaient : l'UI les affichait en neutre anglais");
			SslKeys.Should().HaveCount(21);
		}

		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_Es()
		{
			var es = Cells(EsResx);
			var blind = SpanishMarkers
				.Where(m => !SslKeys.Any(k => es.TryGetValue(k, out var v)
					&& v.ToLowerInvariant().Contains(m, StringComparison.Ordinal)))
				.ToArray();
			blind.Should().BeEmpty(
				"un marqueur aveugle sur l'es-ES de cet onglet rend un 0 qui ne prouve rien");
			SpanishMarkers.Should().HaveCount(26);
		}

		[Fact]
		public void Ssl_Tab_Carries_No_Spanish_Marker()
		{
			var pt = Cells(PtResx);
			var dirty = SslKeys
				.SelectMany(k => SpanishMarkers
					.Where(m => pt[k].ToLowerInvariant().Contains(m, StringComparison.Ordinal))
					.Select(m => $"{k} : « {m} » dans « {pt[k]} »"))
				.ToArray();
			dirty.Should().BeEmpty("l'onglet SSL est traduit : aucune cellule ne doit plus porter d'espagnol");
		}

		[Fact]
		public void Historical_Macaronic_Forms_Are_Gone()
		{
			// Le temoin doit CONTENIR la forme : un temoin qui ne la contient pas ne prouve
			// rien. Chaque couple ci-dessous est verifie dans les deux sens.
			var witnesses = new (string Form, string Witness)[]
			{
				("cabecera", "Valor de cabecera SSL Offloading"),
				("comprobar", "Indique el nombre de la cabecera HTTP que se debe comprobar para saber si se ha utilizado un balanceador de red para el procesamiento de SSL (SSL Offloading)"),
				("procesamiento", "Indique el nombre de la cabecera HTTP que se debe comprobar para saber si se ha utilizado un balanceador de red para el procesamiento de SSL (SSL Offloading)"),
				("estándar", "Opcionalmente puede especificar una URL que será usada para las conexiones seguras para este sitio. Esto solo es necesario si no tiene un certificado SSL instalado para la URL estándar del sitio. Por ejemplo si utiliza una cuenta de alojamiento que le suministra un certificado SSL compartido."),
				("Vuelva a intentarlo", "No se ha podido actualizar la configuración SSL. Vuelva a intentarlo."),
				("Está seguro", "¿Está seguro que desea cancelar los cambios?"),
				("alojamiento", "Opcionalmente puede especificar una URL que será usada para las conexiones seguras para este sitio. Esto solo es necesario si no tiene un certificado SSL instalado para la URL estándar del sitio. Por ejemplo si utiliza una cuenta de alojamiento que le suministra un certificado SSL compartido."),
				("necesitará", "Si especifica una URL SSL necesitará indicar una dirección estándar URL para conexiones no seguras."),
				("ejemplo", "Opcionalmente puede especificar una URL que será usada para las conexiones seguras para este sitio. Esto solo es necesario si no tiene un certificado SSL instalado para la URL estándar del sitio. Por ejemplo si utiliza una cuenta de alojamiento que le suministra un certificado SSL compartido."),
				("suministra", "Opcionalmente puede especificar una URL que será usada para las conexiones seguras para este sitio. Esto solo es necesario si no tiene un certificado SSL instalado para la URL estándar del sitio. Por ejemplo si utiliza una cuenta de alojamiento que le suministra un certificado SSL compartido."),
				("dirección", "Si especifica una URL SSL necesitará indicar una dirección estándar URL para conexiones no seguras."),
				("accesible", "Marque esta opción que las páginas marcadas como inseguras no puedan ser accesibles mediante HTTPS y se redirijan siempre a HTTP."),
				("podido", "No se ha podido actualizar la configuración SSL. Vuelva a intentarlo."),
				("cambios", "¿Está seguro que desea cancelar los cambios?"),
				("sitio", "Opcionalmente puede especificar una URL que será usada para las conexiones seguras para este sitio. Esto solo es necesario si no tiene un certificado SSL instalado para la URL estándar del sitio. Por ejemplo si utiliza una cuenta de alojamiento que le suministra un certificado SSL compartido."),
				("configuración", "CONFIGURACIÓN SSL"),
				("opción", "Marque esta opción que las páginas marcadas como inseguras no puedan ser accesibles mediante HTTPS y se redirijan siempre a HTTP."),
				("mixto", "Modo mixto"),
				("cuenta", "Opcionalmente puede especificar una URL que será usada para las conexiones seguras para este sitio. Esto solo es necesario si no tiene un certificado SSL instalado para la URL estándar del sitio. Por ejemplo si utiliza una cuenta de alojamiento que le suministra un certificado SSL compartido."),
				("establecer", "Establecer todas las páginas como seguras"),
				("redirijan", "Marque esta opción que las páginas marcadas como inseguras no puedan ser accesibles mediante HTTPS y se redirijan siempre a HTTP."),
				("desea", "¿Está seguro que desea cancelar los cambios?"),
				("tiene", "Opcionalmente puede especificar una URL que será usada para las conexiones seguras para este sitio. Esto solo es necesario si no tiene un certificado SSL instalado para la URL estándar del sitio. Por ejemplo si utiliza una cuenta de alojamiento que le suministra un certificado SSL compartido."),
				("puede", "Opcionalmente puede especificar una URL que será usada para las conexiones seguras para este sitio. Esto solo es necesario si no tiene un certificado SSL instalado para la URL estándar del sitio. Por ejemplo si utiliza una cuenta de alojamiento que le suministra un certificado SSL compartido."),
				("debe", "Indique el nombre de la cabecera HTTP que se debe comprobar para saber si se ha utilizado un balanceador de red para el procesamiento de SSL (SSL Offloading)"),
				("nombre", "Indique el nombre de la cabecera HTTP que se debe comprobar para saber si se ha utilizado un balanceador de red para el procesamiento de SSL (SSL Offloading)"),
			};
			witnesses.Should().OnlyContain(w =>
				w.Witness.ToLowerInvariant().Contains(w.Form.ToLowerInvariant(), StringComparison.Ordinal),
				"un temoin qui ne contient pas la forme ne prouve pas que la forme est detectable");
			var pt = Cells(PtResx);
			var survivors = witnesses
				.SelectMany(w => SslKeys
					.Where(k => pt[k].ToLowerInvariant().Contains(w.Form.ToLowerInvariant(), StringComparison.Ordinal))
					.Select(k => $"{k} : « {w.Form} »"))
				.ToArray();
			survivors.Should().BeEmpty("ces formes sont l'espagnol que la PR a retire de l'onglet SSL");
		}

		private static readonly Dictionary<string, string> Pins = new()
		{
			["TabSslSettings.Text"] = "CONFIGURAÇÃO SSL",
			["plSSLEnforced.Text"] = "Modo misto",
			["plSSLEnforced.Help"] = "Marque esta opção para que as páginas marcadas como não seguras não possam ser acessadas por HTTPS e sejam sempre redirecionadas para HTTP.",
			["plSSLURL.Text"] = "URL SSL",
			["plSSLURL.Help"] = "Opcionalmente, especifique uma URL para ser usada nas conexões seguras deste site. Isso só é necessário se você não tiver um certificado SSL instalado para a URL padrão do site. Um exemplo é uma conta de hospedagem compartilhada em que o provedor oferece uma URL SSL compartilhada.",
			["plSTDURL.Text"] = "URL padrão",
			["plSTDURL.Help"] = "Se uma URL SSL for especificada acima, especifique a URL padrão para as conexões não seguras.",
			["plSSLOffload.Text"] = "Valor do cabeçalho de SSL Offload",
			["plSSLOffload.Help"] = "Defina o nome do cabeçalho HTTP que será verificado para saber se um balanceador de rede usou o SSL Offload.",
			["SslSettingsUpdateSuccess.Text"] = "A configuração SSL foi atualizada.",
			["SslSettingsError.Text"] = "Não foi possível atualizar a configuração SSL. Tente novamente mais tarde.",
			["SslSettingsRestoreWarning.Text"] = "Tem certeza de que deseja cancelar as alterações?",
			["SslOn.Text"] = "Ativado",
			["SslOn.Help"] = "Use este modo para exibir todo o conteúdo de forma segura.",
			["SslOff.Text"] = "Desativado",
			["SslOff.Help"] = "Use esta opção quando não houver um certificado SSL instalado no servidor.",
			["SslAdvanced.Text"] = "Avançado",
			["SslAdvanced.Help"] = "Neste modo, seu site exibirá páginas seguras e não seguras, conforme a configuração de cada página. Observe que você tem [NumberOfNonSecureTabs] páginas não seguras neste site.",
			["SslSetAllPagesSecure.Text"] = "Definir todas as páginas como seguras",
			["SslSetAllPagesSecure.Confirm"] = "Deseja definir todas as páginas deste site para que sejam acessíveis apenas por https?",
			["SslSetAllPagesSecure.Completed"] = "Todas as páginas do site foram definidas como seguras.",
		};

		[Fact]
		public void Pins_Exact_Values()
		{
			var pt = Cells(PtResx);
			Pins.Should().HaveCount(21);
			foreach (var (key, expected) in Pins)
			{
				pt.Should().ContainKey(key);
				pt[key].Should().Be(expected, $"la valeur de {key} est epinglee telle qu'arbitree");
			}
		}

		[Fact]
		public void Placeholder_Parity_With_Neutral()
		{
			var token = new Regex(@"\[[A-Za-z0-9_]+\]|\{\d+\}");
			var pt = Cells(PtResx);
			var neu = Cells(NeutralResx);
			foreach (var k in SslKeys)
			{
				var a = token.Matches(neu[k]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal);
				var b = token.Matches(pt[k]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal);
				b.Should().Equal(a, $"{k} doit conserver les jetons du neutre ([Token], {{0}}) : un jeton perdu casse le rendu, un jeton invente casse le code");
			}
			pt["SslAdvanced.Help"].Should().Contain("[NumberOfNonSecureTabs]",
				"ce jeton est substitue au rendu : le traduire ou le perdre laisse la page sans compteur");
		}

		[Fact]
		public void Values_Are_Text_Not_Nested_Xml()
		{
			// ⚠️ Mesure sur les NOEUDS, pas sur la valeur. Une balise brute echappee
			// correctement (&lt;b&gt;) est un texte ; une balise brute NON echappee devient
			// un XElement enfant du <value>, et `Element("value").Value` la fait
			// DISPARAITRE de la chaine : un test sur la chaine la declare absente.
			// C'est exactement ce qu'un banc de mutations a attrape ici (M9 survivante).
			var nested = Load(PtResx).Root!.Elements("data")
				.Select(d => new { Key = d.Attribute("name")!.Value, Elem = d.Element("value") })
				.Where(x => x.Elem != null && x.Elem.Nodes().Any(n => n is XElement))
				.Select(x => x.Key)
				.ToArray();
			nested.Should().BeEmpty(
				"une balise BRUTE (non echappee) est lue comme du XML imbrique : elle casse le parseur du module");
		}

		[Fact]
		public void No_Space_Before_Punctuation()
		{
			var bad = new Regex(@"[ \t]([,;:!?]|\.[ \t]*$)");
			var pt = Cells(PtResx);
			var hits = SslKeys.Where(k => bad.IsMatch(pt[k])).ToArray();
			hits.Should().BeEmpty("une espace avant la ponctuation est un artefact de traduction");
		}

		[Fact]
		public void Terminology_Is_PtBr()
		{
			var pt = Cells(PtResx);
			pt["plSSLEnforced.Text"].Should().Be("Modo misto",
				"le neutre dit « Mixed Mode » ; « Forzar SSL » decrivait un autre reglage que celui applique");
			pt["plSTDURL.Text"].Should().Be("URL padrão", "« padrão » a 64 occurrences dans 20 fichiers merges, « estándar » est l'espagnol");
			pt["SslOn.Text"].Should().Be("Ativado", "precedent exact : users.pt-BR.resx SwitchOn.Text");
			pt["SslOff.Text"].Should().Be("Desativado", "precedent exact : users.pt-BR.resx SwitchOff.Text");
			pt["SslAdvanced.Text"].Should().Be("Avançado");
			pt["plSSLOffload.Text"].Should().Contain("cabeçalho", "« cabecera » est l'espagnol (9 occurrences, toutes dans des fichiers non balayes)");
			pt["SslSettingsError.Text"].Should().Contain("Tente novamente mais tarde", "precedent : roles.pt-BR.resx et users.pt-BR.resx");
			pt["SslSettingsRestoreWarning.Text"].Should().StartWith("Tem certeza de que deseja", "precedent : SharedResources et users");
			foreach (var k in SslKeys)
			{
				pt[k].Should().NotContain("contrasenha", $"{k}");
				pt[k].Should().NotContain("cabecera", $"{k}");
			}
		}

		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell_Except_Legitimate_Homographs()
		{
			// « URL SSL » est identique dans les trois langues : ce n'est pas une contamination.
			var homographs = new[] { "plSSLURL.Text" };
			var pt = Cells(PtResx);
			var es = Cells(EsResx);
			var equal = SslKeys
				.Where(k => !homographs.Contains(k) && pt[k].Trim().Length > 0 && pt[k].Trim() == es[k].Trim())
				.ToArray();
			equal.Should().BeEmpty("une cellule identique a l'espagnol est une cellule non traduite");
		}

		[Fact]
		public void Legacy_Keys_Survive_The_Rewrite()
		{
			// 13 cles heritees que le neutre ne porte plus : elles ne sont pas du ressort de
			// cette PR, mais une reecriture ne doit pas les emporter en silence.
			var legacy = new[]
			{
				"CheckTelerikVulnerabilityFailure.Text",
				"CheckTelerikVulnerabilityName.Text",
				"CheckTelerikVulnerabilityReason.Text",
				"CheckTelerikVulnerabilitySuccess.Text",
				"Não.Text",
				"plSSLEnabled.Help",
				"plSSLEnabled.Text",
				"Redirect_AfterLogin.Help",
				"Redirect_AfterLogin.Text",
				"Redirect_AfterLogout.Help",
				"Redirect_AfterLogout.Text",
				"Redirect_AfterRegistration.Help",
				"Redirect_AfterRegistration.Text",
			};
			var pt = Cells(PtResx);
			legacy.Where(k => !pt.ContainsKey(k)).Should().BeEmpty(
				"ces cles heritees existaient avant la reecriture : leur disparition serait une perte");
			pt.Should().HaveCountGreaterThanOrEqualTo(354,
				"anti-effondrement : 345 + 9 cles creees par l'onglet SSL. Les onglets suivants de la decoupe ajoutent les leurs (Member Accounts -> 355) : le compte EXACT vit dans la garde de chaque onglet, pas ici");
		}

		[Fact]
		public void Committed_Blob_Is_Utf8_Bom_With_Lf_Only()
		{
			// Invariant de TRANSPORT : il s'affirme sur le blob committe, jamais sur l'arbre de
			// travail. Un runner Windows a core.autocrlf=true : il convertit LF -> CRLF AU
			// CHECKOUT, donc un fichier sain y apparait en CRLF (defaut mesure sur #2078).
			var bytes = CommittedBlob(PtResx);
			bytes.Length.Should().BeGreaterThan(1000, "le blob doit etre le fichier entier, pas un fragment");
			bytes.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "le BOM est celui du fichier d'origine");
			bytes.Count(b => b == 0x0D).Should().Be(0,
				"le blob committe est en LF seul : une reecriture ne doit pas le recabler en CRLF");
		}

		private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
	}
}
