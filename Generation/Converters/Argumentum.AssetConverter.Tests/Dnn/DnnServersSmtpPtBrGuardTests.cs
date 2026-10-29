using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// Garde du sweep pt-BR (pool #458 c.6436567243 item 8), fichier 4/5 :
	/// <c>Dnn.Servers/App_LocalResources/servers.pt-BR.resx</c> — **passe 1 =
	/// l'onglet SMTP entier** (23 clés : 15 corrections, 8 conservées).
	/// Le fichier porte ~169 clés dont ~64 à marqueurs espagnols forts et 42
	/// byte-égales à l'es-ES : il excède le plafond du pool (≤ 40 cellules/PR),
	/// d'où la découpe « une passe visible par PR » (précédent D4, passes 3/4).
	/// Registre você, terminologie D4/#2059/#2062 : senha, usuário, e-mail,
	/// Informe, redefinir.
	/// <para>
	/// Choix mesurés : le NEUTRE fait foi (fr corrobore) — l'es-ES porte un
	/// texte plus ancien sur plSMTPEnableSSL.Help et ampute plSMTPPassword.Help
	/// de sa seconde phrase (« Leave blank to not to change it »), restaurée.
	/// « Activar » est conservé : forme établie du corpus pt-BR livré
	/// (SharedResources, pages, taskscheduler, seo…) et homographe es/pt.
	/// « guia » (onglet) est SANS précédent corpus — déclaré dans la PR.
	/// </para>
	/// <para>
	/// Défaut structurel déclaré, hors périmètre de cette passe : le pt-BR
	/// n'a que 169 clés sur les 200 du neutre (31 absentes) — un défaut de
	/// complétude distinct d'un défaut de langue, grain séparé.
	/// </para>
	/// </summary>
	public class DnnServersSmtpPtBrGuardTests
	{
		private const string PtResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Servers/App_LocalResources/servers.pt-BR.resx";
		private const string EsResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Servers/App_LocalResources/servers.es-ES.resx";
		private const string NeutralResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Servers/App_LocalResources/Servers.resx";

		/// <summary>Périmètre de la passe : l'onglet SMTP.</summary>
		private static readonly string[] PassKeys =
		{
			"EmailSentMessage.Text", "EmailTest.Text", "errorMessageSendingTestEmail.Text",
			"errorMessageLoadingSmtpServerTab.Text", "errorMessageUpdatingSmtpServerTab.Text",
			"plSMTPAuthentication.Help", "plSMTPAuthentication.Text", "plSMTPEnableSSL.Help",
			"plSMTPEnableSSL.Text", "plSMTPMode.Help", "plSMTPMode.Text", "plSMTPPassword.Help",
			"plSMTPPassword.Text", "plSMTPServer.Help", "plSMTPServer.Text", "plSMTPUsername.Help",
			"plSMTPUsername.Text", "SMTPAnonymous.Text", "SMTPBasic.Text", "SMTPNTLM.Text",
			"GlobalSmtpHostSetting.Text", "SiteSmtpHostSetting.Text", "tabSmtpServerTitle.Text",
		};

		private static XDocument Load(string resxRelativePath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");
			return XDocument.Load(path);
		}

		private static Dictionary<string, string> Cells(string resxRelativePath) =>
			Load(resxRelativePath).Root!.Elements("data")
				.ToDictionary(d => d.Attribute("name")!.Value, d => d.Element("value")?.Value ?? "");

		private static string ResxValue(string resxRelativePath, string key) => Cells(resxRelativePath)[key];

		// ------------------------------------------------------------------
		// Grille de marqueurs espagnols FORTS — formes qui ne sont PAS du
		// portugais valide, auto-contrôlées sur l'es-ES (fichier entier).
		// Exclusions volontaires (valables dans les deux langues ou établies
		// du corpus pt-BR) : Activar, método, desde, guardar, clic, guia.
		// ------------------------------------------------------------------
		private static readonly Regex[] SpanishMarkers =
		{
			new(@"\w*ñ\w*"),
			new(@"\w*ción\b", RegexOptions.IgnoreCase),
			new(@"\bprueba\b", RegexOptions.IgnoreCase),
			new(@"\bcargar\b|\bcargando\b", RegexOptions.IgnoreCase),
			new(@"\bproducido\b", RegexOptions.IgnoreCase),
			new(@"\bintroduzca\b", RegexOptions.IgnoreCase),
			new(@"\bescriba\b", RegexOptions.IgnoreCase),
			new(@"\bpuerto\b", RegexOptions.IgnoreCase),
			new(@"\bsitio\b", RegexOptions.IgnoreCase),
			new(@"\busuario\b", RegexOptions.IgnoreCase),   // es sans accent ; pt « usuário »
			new(@"\bpropio\b", RegexOptions.IgnoreCase),    // pt : « próprio »
			// « servicios » et « requieren » retirés : le texte es-ES de
			// plSMTPEnableSSL.Help a été réécrit (MailKit) et ne porte plus
			// ces mots — marqueurs aveugles, détectés par le self-contrôle.
			// Couverts par les pins exacts des cellules concernées.
		};

		[Fact]
		public void Pass_Keys_Exist_In_Both_Files()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			PassKeys.Should().HaveCount(23);
			foreach (var key in PassKeys)
			{
				pt.Keys.Should().Contain(key, "la passe vit dans le fichier pt-BR ({0})", key);
				ne.Keys.Should().Contain(key, "…et dans le neutre ({0})", key);
			}
		}

		/// <summary>Auto-contrôle de l'instrument : chaque marqueur doit
		/// toucher au moins une cellule de l'es-ES (fichier entier).</summary>
		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_Es()
		{
			var esCells = Cells(EsResx).Values.ToArray();
			var blind = SpanishMarkers
				.Where(m => !esCells.Any(v => m.IsMatch(v)))
				.Select(m => m.ToString())
				.ToArray();
			blind.Should().BeEmpty("chaque marqueur doit être vu par l'es-ES (sinon son 0 sur le pt-BR est aveugle)");
		}

		/// <summary>La grille ne s'applique qu'aux 23 cellules DE LA PASSE :
		/// le reste du fichier (autres onglets) attend ses passes suivantes —
		/// un marqueur hors passe n'est pas une régression de ce grain.</summary>
		[Fact]
		public void Pass_Cells_Carry_No_Spanish_Marker()
		{
			var pt = Cells(PtResx);
			var offenders = new List<string>();
			foreach (var key in PassKeys)
			foreach (var marker in SpanishMarkers)
				if (marker.IsMatch(pt[key]))
					offenders.Add($"{key} [{marker}] :: {pt[key]}");
			offenders.Should().BeEmpty(
				"les 23 cellules de l'onglet SMTP sont en portugais — tout marqueur restant est une réinjection : {0}",
				string.Join(" | ", offenders));
		}

		/// <summary>Contrôle falsifiant par littéraux : formes macaroniques
		/// de l'ancienne valeur, présence prouvée dans le littéral historique,
		/// absence exigée des cellules de la passe.</summary>
		[Fact]
		public void Historical_Macaronic_Forms_Are_Gone()
		{
			var historical = new Dictionary<string, string>
			{
				// les deux casses : le libellé capitalise, l'aide ne le fait pas
				["Contrasenha"] = "Contrasenha SMTP:",
				["contrasenha"] = "Introduzca a contrasenha do servidor SMTP",
				["Introduzca"] = "Introduzca o usuário de autentação do SMTP",
				["Escriba"] = "Escriba o método de autentação do SMTP",
				["Prueba de configuração"] = "Prueba de configuração SMTP",
				["foi producido"] = "foi producido um error ao enviar o email de prueba.",
				["pestaña"] = "ao cargar a pestaña do servidor SMTP",
				["Anónimo"] = "Anónimo",
				["puerto"] = "Servidor SMTP e puerto:",
				["Usuario SMTP:"] = "Usuario SMTP:",
				["Sitio le permite"] = "O modo Sitio le permite especificar sus propios parámetros",
			};
			var current = string.Join("\n", PassKeys.Select(k => ResxValue(PtResx, k)));
			foreach (var (form, witness) in historical)
			{
				witness.Should().Contain(form, "témoin littéral : la forme historique doit être présente dans le littéral");
				current.Should().NotContain(form,
					$"« {form} » est une forme macaronique retirée — sa réapparition est une régression");
			}
		}

		private static readonly Dictionary<string, string> Pins = new()
		{
			// 15 corrections
			["EmailSentMessage.Text"] = "E-mail enviado com sucesso de {0} para {1}",
			["EmailTest.Text"] = "Testar configurações SMTP",
			["errorMessageSendingTestEmail.Text"] = "Ocorreu um erro ao tentar enviar o e-mail de teste",
			["errorMessageLoadingSmtpServerTab.Text"] = "Erro ao carregar a guia do servidor SMTP",
			["errorMessageUpdatingSmtpServerTab.Text"] = "Erro ao atualizar as configurações do servidor SMTP",
			["plSMTPAuthentication.Help"] = "Informe o método de autenticação do servidor SMTP. O padrão é Anônimo.",
			["plSMTPEnableSSL.Help"] = "Usado para serviços SMTP que exigem conexão segura. Em geral, essa configuração não é necessária.",
			["plSMTPMode.Help"] = "O modo Host usa todas as configurações SMTP definidas no nível da aplicação. O modo Site permite que você selecione seu próprio servidor SMTP, porta e método de autenticação.",
			["plSMTPPassword.Help"] = "Informe a senha do servidor SMTP. Deixe em branco para não alterá-la.",
			["plSMTPPassword.Text"] = "Senha SMTP:",
			["plSMTPServer.Help"] = "Informe o nome (endereço) e a porta do servidor SMTP que será usado para enviar e-mails deste site.",
			["plSMTPServer.Text"] = "Servidor SMTP e porta:",
			["plSMTPUsername.Help"] = "Informe o nome de usuário do servidor SMTP.",
			["plSMTPUsername.Text"] = "Usuário SMTP:",
			["SMTPAnonymous.Text"] = "Anônimo",
			// 8 conservées (pt sain ou homographe légitime)
			["plSMTPAuthentication.Text"] = "Autenticação SMTP:",
			["plSMTPEnableSSL.Text"] = "Activar SSL para SMTP:",
			["plSMTPMode.Text"] = "Modo SMTP:",
			["SMTPBasic.Text"] = "Básico",
			["SMTPNTLM.Text"] = "NTLM",
			["GlobalSmtpHostSetting.Text"] = "Global",
			["SiteSmtpHostSetting.Text"] = "{0}",
			["tabSmtpServerTitle.Text"] = "Servidor SMTP",
		};

		[Fact]
		public void Pins_Exact_Values()
		{
			Pins.Should().HaveCount(23);
			foreach (var (key, expected) in Pins)
				ResxValue(PtResx, key).Should().Be(expected, "pin du sweep pt-BR — clé {0}.", key);
		}

		/// <summary>Parité des placeholders sur les clés de la passe, dans les
		/// deux sens (le neutre n'en porte que sur EmailSentMessage et
		/// SiteSmtpHostSetting).</summary>
		[Fact]
		public void Placeholder_Parity_With_Neutral_On_Pass()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			var token = new Regex(@"\{\d+\}");
			foreach (var key in PassKeys)
			{
				var expected = token.Matches(ne[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal).ToArray();
				var actual = token.Matches(pt[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal).ToArray();
				actual.Should().Equal(expected, "placeholders de {0} : le neutre porte {1}", key, string.Join(",", expected));
			}
			ResxValue(PtResx, "EmailSentMessage.Text").Should().Contain("{0}").And.Contain("{1}",
				"le message de test interpelle l'expéditeur ({0}) et le destinataire ({1})");
		}

		/// <summary>Convention pt-BR : pas d'espace avant les deux-points.</summary>
		[Fact]
		public void No_Space_Before_Punctuation_On_Pass()
		{
			var spaceBefore = new Regex(@"\s+[:;]");
			var offenders = PassKeys
				.Where(k => spaceBefore.IsMatch(ResxValue(PtResx, k)))
				.Select(k => $"{k} :: {ResxValue(PtResx, k)}")
				.ToArray();
			offenders.Should().BeEmpty("pt-BR : aucun espace avant « : » ni « ; » : {0}", string.Join(" | ", offenders));
		}

		/// <summary>Toute cellule de la passe dont le neutre parle de mot de
		/// passe (ou d'utilisateur) dit « senha » (« usuário ») — terminologie
		/// D4, insensible à la casse des libellés.</summary>
		[Fact]
		public void Password_And_User_Cells_Use_The_Established_Terminology()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			var passwordCells = PassKeys.Where(k => Regex.IsMatch(ne[k], "password", RegexOptions.IgnoreCase)).ToArray();
			passwordCells.Should().Equal(new[] { "plSMTPPassword.Help", "plSMTPPassword.Text" },
				"témoin du périmètre : 2 cellules password dans la passe");
			foreach (var key in passwordCells)
				Regex.IsMatch(pt[key], "senha", RegexOptions.IgnoreCase).Should().BeTrue(
					"terminologie D4 : « senha », jamais « contrasenha » ({0})", key);
			ResxValue(PtResx, "plSMTPUsername.Text").Should().Contain("Usuário",
				"« usuário » accentué, jamais la forme es « Usuario »");
		}

		/// <summary>La phrase amputée est restaurée : le neutre de
		/// plSMTPPassword.Help porte « Leave blank to not to change it »,
		/// absent de l'ancienne valeur pt (et présent dans l'es-ES — preuve
		/// que la source pt était une génération derrière).</summary>
		[Fact]
		public void Restored_Meaning_Password_Blank_Sentence()
		{
			ResxValue(PtResx, "plSMTPPassword.Help").Should().Contain("Deixe em branco para não alterá-la",
				"la seconde phrase du neutre est restaurée, pas seulement la langue");
		}

		/// <summary>Avant la passe, 9 cellules étaient byte-égales à l'es-ES.
		/// Les 7 qui restent sont des homographes légitimes (pt sain, même
		/// écriture en es) ; les 2 autres (Usuario SMTP:, Anónimo) étaient des
		/// non-traductions et sont corrigées.</summary>
		[Fact]
		public void Only_Whitelisted_Pass_Cells_May_Equal_The_Spanish_Cell()
		{
			var pt = Cells(PtResx);
			var es = Cells(EsResx);
			var equal = PassKeys.Where(k => es.TryGetValue(k, out var v) && v == pt[k])
				.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			equal.Should().Equal(new[]
			{
				"GlobalSmtpHostSetting.Text", "SMTPBasic.Text", "SMTPNTLM.Text", "SiteSmtpHostSetting.Text",
				"plSMTPEnableSSL.Text", "plSMTPMode.Text", "tabSmtpServerTitle.Text",
			}, "sept homographes légitimes ; toute autre égalité de la passe est une non-traduction");
		}
	}
}
