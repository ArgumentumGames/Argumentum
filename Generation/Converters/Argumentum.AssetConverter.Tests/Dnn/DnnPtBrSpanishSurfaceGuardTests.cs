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
	/// Garde D3 (pool #458, dispatch ai-01 c.6242623963 item po-2024.1) : le
	/// paquet pt-BR construit depuis develop portait des valeurs ESPAGNOLES
	/// sur la surface visible (fil d'Ariane « Inicio », « Ayuda »,
	/// « Añadir », courriels « Apreciado… Gracias por registrarse… ») —
	/// mesuré : 80 valeurs à marqueur fort sur 6 fichiers pt-BR (crible
	/// self-testé, plus strict que la borne haute 52+21 d'ai-01 qui comptait
	/// aussi des mots valides en pt). Cette PR corrige les 44 clés de la
	/// surface visible (28 GlobalResources + 16 SharedResources) en pt-BR,
	/// registre você (norme BR #1879/#1890) : salutation « Prezado(a) »,
	/// « Atenciosamente », senha (jamais contrasenha), usuário, sessão.
	/// <list type="bullet">
	/// <item>Découverte neuve : LogStart/LogEnd étaient INVERSÉS en plus
	/// d'être espagnols (EN Start→PT « Final », EN End→PT « Inicio ») —
	/// invisible au crible espagnol, vu par contraste EN.</item>
	/// <item>Tokens [X:Y] préservés VERBATIM par clé (les casses
	/// UserName/Username coexistent même dans le resx EN neutre) ; la seule
	/// réparation de token est le crochet fermant de [Portal:Email dans
	/// EMAIL_USER_UNAUTHORIZED_BODY (EN : [Portal:Email]).</item>
	/// <item>Le RESTE du paquet (43 valeurs mesurées : admin, workflow,
	/// exceptions, upload, pays) reste contaminé — chiffré valeur par
	/// valeur dans le corps de la PR, décision correction ciblée vs
	/// remplacement = ai-01.</item>
	/// </list>
	/// </summary>
	public class DnnPtBrSpanishSurfaceGuardTests
	{
		private const string GlobalResx = "DNNPlatform/App_GlobalResources/GlobalResources.pt-BR.resx";
		private const string SharedResx = "DNNPlatform/App_GlobalResources/SharedResources.pt-BR.resx";

		private static string ResxValue(string resxRelativePath, string key)
		{
			var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");
			var entry = XDocument.Load(path).Root!.Elements("data")
				.FirstOrDefault(d => string.Equals(d.Attribute("name")?.Value, key, StringComparison.Ordinal));
			entry.Should().NotBeNull($"la clé {key} doit exister dans {resxRelativePath}");
			return entry!.Element("value")!.Value;
		}

		// Les 44 clés corrigées (surface visible), par fichier.
		private static readonly string[] GlobalKeys =
		{
			"//Home.String", "LogStart.Text", "LogEnd.Text",
			"ModuleHelp.Action", "OnlineHelp.Action", "NoLoginControl.Text",
			"EMAIL_PASSWORD_REMINDER_BODY.Text", "EMAIL_PASSWORD_REMINDER_SUBJECT.Text",
			"EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_BODY.Text", "EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_SUBJECT.Text",
			"EMAIL_PASSWORD_UPDATED_BODY.Text", "EMAIL_PASSWORD_UPDATED_SUBJECT.Text",
			"EMAIL_PORTAL_SIGNUP_BODY.Text", "EMAIL_PROFILE_UPDATED_BODY.Text",
			"EMAIL_ROLE_ASSIGNMENT_BODY.Text", "" + "EMAIL_ROLE_UNASSIGNMENT_BODY.Text", "EMAIL_ROLE_UPDATE_BODY.Text",
			"EMAIL_AFFILIATE_NOTIFICATION_BODY.Text",
			"EMAIL_USER_AUTHORIZED_BODY.Text", "EMAIL_USER_AUTHORIZED_SUBJECT.Text",
			"EMAIL_USER_REGISTRATION_PRIVATE_BODY.Text", "EMAIL_USER_REGISTRATION_PUBLIC_BODY.Text",
			"EMAIL_USER_REGISTRATION_VERIFIED_BODY.Text",
			"EMAIL_USER_UNAUTHORIZED_BODY.Text", "EMAIL_USER_UNAUTHORIZED_SUBJECT.Text",
			"EMAIL_USER_UPDATED_OWN_PASSWORD_BODY.Text", "EMAIL_USER_UPDATED_OWN_PASSWORD_SUBJECT.Text",
			"EMAIL_USER_LOCKOUT_BODY.Text",
		};

		private static readonly string[] SharedKeys =
		{
			"Home.Text", "Add.Text", "cmdHelp.Text", "Help.Text", "ModuleHelp.Text",
			"User.Text", "PermissionUserHeader.Text", "DropDownList.SelectUserDefaultText",
			"UserEmailExists.Text", "UserNameExists.Text", "DeletetUser.Text",
			"AllUsers.Text", "UnauthenticatedUsers.Text", "Wizard.Success.SuccessDetail",
			"Year.Text", "Years.Text",
		};

		private const string FileUploadResx = "DNNPlatform/App_GlobalResources/FileUpload.pt-BR.resx";
		private const string WebControlsResx = "DNNPlatform/App_GlobalResources/WebControls.pt-BR.resx";

		// Passe 3 (dispatch ai-01 18/10 23:11, réserve NanoClaw #1899 confirmée) :
		// famille temporelle « Hace {0}… », contrôles standard (Upload/cmdReturn
		// + cmdStop même barre), et module d'upload WebControls CORRIGÉ EN ENTIER
		// (14 clés cassées sur 22 — corriger les seules citées aurait laissé un
		// dialogue moitié espagnol et forcément une passe 4 pour 2 boutons).
		private static readonly string[] SharedKeysPass3 =
		{
			"SecondsAgo.Text", "MinuteAgo.Text", "MinutesAgo.Text", "HourAgo.Text", "HoursAgo.Text",
			"DayAgo.Text", "DaysAgo.Text", "WeekAgo.Text", "WeeksAgo.Text", "MonthAgo.Text", "MonthsAgo.Text",
			"Day.Text", "Days.Text", "Upload.Text", "cmdReturn.Text", "cmdStop.Text",
		};

		private static readonly string[] FileUploadKeys = { "ErrorMessage.Text" };

		private static readonly string[] WebControlsUploadKeys =
		{
			"FileUpload.CloseButton.Text", "FileUpload.DecompressingFile.Text", "FileUpload.DecompressLabel.Text",
			"FileUpload.EmptyFileUpload.Error", "FileUpload.FileAlreadyExists.Error", "FileUpload.FileUploadCancelled.Error",
			"FileUpload.FileUploaded.Text", "FileUpload.FileUploadFailed.Error", "FileUpload.InvalidFileExtensions.Error",
			"FileUpload.KeepButton.Text", "FileUpload.ReplaceButton.Text", "FileUpload.UploadFromWebMethod.Text",
			"FileUpload.UploadStopped.Text", "FileUpload.UrlTooltip.Text",
		};

		// Marqueurs espagnols FORTS — formes qui ne sont PAS du portugais
		// valide (self-test du script de mesure : Inicio/Ayuda/Añadir/Gracias
		// hit ; Início/Ajuda/Adicionar/Obrigado miss).
		private static readonly Regex[] SpanishMarkers =
		{
			new(@"\w*ñ\w*", RegexOptions.IgnoreCase),
			new(@"[¿¡]"),
			new(@"\w*(ción|siones)\b", RegexOptions.IgnoreCase),
			new(@"\w*sión\b", RegexOptions.IgnoreCase),
			new(@"contraseñ\w*", RegexOptions.IgnoreCase),
			new(@"\busuarios?\b", RegexOptions.IgnoreCase),
			new(@"\bInicio\b"),
			new(@"\bAyuda\b"),
			new(@"\bborrador\w*\b", RegexOptions.IgnoreCase),
			new(@"\bgracias\b", RegexOptions.IgnoreCase),
			new(@"\bbienvenid[oa]s?\b", RegexOptions.IgnoreCase),
			new(@"\b\w*registrarse\b", RegexOptions.IgnoreCase),
			new(@"\bapreciad[oa]s?\b", RegexOptions.IgnoreCase),
			new(@"\baños\b", RegexOptions.IgnoreCase),
			new(@"\bespañol\w*\b", RegexOptions.IgnoreCase),
			new(@"\bsu\s+correo\b", RegexOptions.IgnoreCase),
			// Élargissement passe 3 — cibles ai-01 (Hace/Cargar/Volver + upload) et
			// lexique du module. Self-test : « Há 5 dias », « Carregar », « Voltar »,
			// « Parar », « Manter », « Fechar », « Substituir », « exemplo » miss ;
			// « Hace », « días », « Cargar », « Mantener », « ejemplo » hit.
			new(@"\bHace\b"),
			new(@"\bdías?\b"),                                              // accent = es ; pt « dia/dias »
			new(@"\bcargar\b|\bcargad[oa]s?\b", RegexOptions.IgnoreCase),   // 1 r = es ; pt « carregar »
			new(@"\bvolver\b", RegexOptions.IgnoreCase),
			new(@"\bMantener\b|\bDetener\b|\bCerrar\b|\bReemplazar\b"),
			new(@"\bIntroduzca\b|\bejemplo\b", RegexOptions.IgnoreCase),
			new(@"\bDescomprim\w*", RegexOptions.IgnoreCase),               // es ; pt « descompactar »
			new(@"\bdetenid[oa]s?\b", RegexOptions.IgnoreCase),
			new(@"\bDesde\s+URL\b"),
		};

		[Fact]
		public void FixedSurfaceKeys_CarryNoSpanishMarker()
		{
			var offenders = new List<string>();
			foreach (var (path, keys) in new[] { (GlobalResx, GlobalKeys), (SharedResx, SharedKeys) })
			foreach (var key in keys)
			foreach (var marker in SpanishMarkers)
				if (marker.IsMatch(ResxValue(path, key)))
					offenders.Add($"{key} [{marker}]");
			offenders.Should().BeEmpty(
				"les 44 clés de la surface visible pt-BR sont corrigées en portugais (registre você) — " +
				"tout marqueur restant est une réinjection : {0}", string.Join(", ", offenders));
		}

		[Fact]
		public void UiPins_ExactValues()
		{
			var pins = new Dictionary<(string path, string key), string>
			{
				[(SharedResx, "Home.Text")] = "Início",
				[(SharedResx, "Add.Text")] = "Adicionar",
				[(SharedResx, "cmdHelp.Text")] = "Ajuda",
				[(SharedResx, "Help.Text")] = "Ajuda",
				[(SharedResx, "ModuleHelp.Text")] = "Ajuda do módulo",
				[(SharedResx, "User.Text")] = "Usuário:",
				[(SharedResx, "PermissionUserHeader.Text")] = "Usuário",
				[(SharedResx, "DropDownList.SelectUserDefaultText")] = "Selecione um usuário",
				[(SharedResx, "DeletetUser.Text")] = "Tem certeza de que deseja excluir este usuário?",
				[(SharedResx, "AllUsers.Text")] = "Todos os usuários",
				[(SharedResx, "UnauthenticatedUsers.Text")] = "Usuários não autenticados",
				[(SharedResx, "Year.Text")] = "Ano",
				[(SharedResx, "Years.Text")] = "Anos",
				[(GlobalResx, "//Home.String")] = "Início",
				[(GlobalResx, "ModuleHelp.Action")] = "Ajuda",
				[(GlobalResx, "OnlineHelp.Action")] = "Ajuda online",
			};
			foreach (var ((path, key), expected) in pins)
				ResxValue(path, key).Should().Be(expected, "pin D3 — clé visible {0}.", key);
		}

		/// <summary>Découverte neuve D3 : la paire était INVERSÉE en plus
		/// d'être espagnole (EN Start→PT « Final », EN End→PT « Inicio »).</summary>
		[Fact]
		public void LogStartEnd_NotSpanish_AndNotSwapped()
		{
			ResxValue(GlobalResx, "LogStart.Text").Should().Be("Início ",
				"EN « Start » — l'ancienne valeur pt « Final » était le mot de LogEnd : paire inversée.");
			ResxValue(GlobalResx, "LogEnd.Text").Should().Be("Final ",
				"EN « End » — l'ancienne valeur pt « Inicio » était le mot de LogStart : paire inversée.");
		}

		[Fact]
		public void PlayerEmails_VoceRegister_SalutationAndClosing()
		{
			// LOCKOUT = alerte ADMIN (l'EN n'a pas de salutation non plus) :
			// seule exclusion, les 15 courriels joueurs ouvrent sur Prezado(a).
			var bodies = GlobalKeys
				.Where(k => k.StartsWith("EMAIL_") && k.EndsWith("_BODY.Text") && k != "EMAIL_USER_LOCKOUT_BODY.Text")
				.ToArray();
			bodies.Should().HaveCount(15, "15 courriels joueurs salués (LOCKOUT = alerte admin sans salutation, comme l'EN).");
			foreach (var key in bodies)
			{
				var v = ResxValue(GlobalResx, key);
				v.Should().StartWith("Prezado(a) ",
					"{0} : la salutation espagnole « Apreciado » est remplacée (registre você).", key);
				v.Should().ContainAny("Atenciosamente", "Obrigado",
					"{0} : la formule d'usage pt-BR doit clore le courriel (espagnol « Gracias » exclu).", key);
			}
			ResxValue(GlobalResx, "EMAIL_USER_LOCKOUT_BODY.Text").Should().StartWith("Uma tentativa",
				"LOCKOUT : alerte admin — pas de salutation (miroir EN « An attempt was made… »).");
		}

		// Intégrité des tokens [X:Y] — préservés VERBATIM par la correction
		// de langue (les tokens perdus casseraient les courriels réels).
		private static readonly Dictionary<string, string[]> ExpectedTokens = new()
		{
			["EMAIL_PASSWORD_REMINDER_BODY.Text"] = new[]
			{ "[User:DisplayName]", "[Portal:PortalName]", "[Portal:URL]", "[User:UserName]", "[Portal:PASSWORDREMINDERURL]", "[Membership:PasswordResetToken]" },
			["EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]" },
			["EMAIL_PASSWORD_UPDATED_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]", "[Portal:Email]" },
			["EMAIL_PORTAL_SIGNUP_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:URL]", "[Membership:UserName]", "[User:VerificationCode]", "[Host:HostTitle]" },
			["EMAIL_PROFILE_UPDATED_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]" },
			["EMAIL_ROLE_ASSIGNMENT_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]", "[Custom:0]", "[Custom:1]", "[Custom:2]", "[Custom:3]" },
			["EMAIL_ROLE_UNASSIGNMENT_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]", "[Custom:0]", "[Custom:1]" },
			["EMAIL_ROLE_UPDATE_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]", "[Custom:0]", "[Custom:1]", "[Custom:2]", "[Custom:3]" },
			["EMAIL_AFFILIATE_NOTIFICATION_BODY.Text"] = new[] { "[Custom:0]", "[Portal:PortalName]", "[Custom:1]" },
			["EMAIL_USER_AUTHORIZED_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]", "[Portal:URL]", "[User:UserName]" },
			["EMAIL_USER_REGISTRATION_PRIVATE_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]", "[Portal:URL]", "[User:UserName]" },
			["EMAIL_USER_REGISTRATION_PUBLIC_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]", "[Portal:URL]", "[User:UserName]" },
			["EMAIL_USER_REGISTRATION_VERIFIED_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]", "[Portal:URL]", "[User:UserName]", "[Custom:0]", "[Custom:1]" },
			["EMAIL_USER_UNAUTHORIZED_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]", "[Portal:URL]", "[User:UserName]", "[Portal:Email]" },
			["EMAIL_USER_UPDATED_OWN_PASSWORD_BODY.Text"] = new[] { "[User:DisplayName]", "[Portal:PortalName]", "[Portal:URL]", "[User:Username]" },
			["EMAIL_USER_LOCKOUT_BODY.Text"] = new[] { "[Custom:0]", "[Date:Current]", "[Portal:PortalName]" },
		};

		[Fact]
		public void PlayerEmails_TokensPreservedVerbatim()
		{
			foreach (var (key, tokens) in ExpectedTokens)
			foreach (var token in tokens)
				ResxValue(GlobalResx, key).Should().Contain(token,
					"{0} : le token {1} doit survivre à la correction de langue (substitution réelle des courriels).", key, token);
		}

		[Fact]
		public void EmailSubjects_Pinned()
		{
			ResxValue(GlobalResx, "EMAIL_PASSWORD_REMINDER_SUBJECT.Text").Should().Be("[Portal:PortalName] Lembrete de senha");
			ResxValue(GlobalResx, "EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_SUBJECT.Text").Should().Be("[Portal:PortalName] Lembrete de senha");
			ResxValue(GlobalResx, "EMAIL_PASSWORD_UPDATED_SUBJECT.Text").Should().Be("[Portal:PortalName] Senha atualizada");
			ResxValue(GlobalResx, "EMAIL_USER_AUTHORIZED_SUBJECT.Text").Should().Be("Seu registro para [Portal:URL] foi aprovado");
			ResxValue(GlobalResx, "EMAIL_USER_UNAUTHORIZED_SUBJECT.Text").Should().Be("Sua conta foi desautorizada em [Portal:URL]");
			ResxValue(GlobalResx, "EMAIL_USER_UPDATED_OWN_PASSWORD_SUBJECT.Text").Should().Be("[Portal:PortalName] Senha atualizada");
		}

		/// <summary>Contrôle inverse : le reste du paquet NON corrigé porte
		/// encore des marqueurs (la garde est scopée aux 44 clés — la
		/// décision sur le reste attend l'arbitrage ai-01, elle n'est pas
		/// encore un invariant).</summary>
		[Fact]
		public void RestOfPackage_StillContaminated_UntilArbitration()
		{
			var v = ResxValue(SharedResx, "DefaultWorkflowState1.StateName");
			v.Should().Be("Borrador",
				"le reste du paquet n'est PAS encore corrigé (workflow admin) — ce témoin prouve que la garde " +
				"marque-taux ci-dessus est scopée aux 44 clés et non à tout le fichier ; sa correction rougira " +
				"ce fait au grain suivant (chiffrage → arbitrage ai-01).");
		}

		// ===================== Passe 3 (18/10, dispatch 23:11) =====================

		[Fact]
		public void Pass3_TimeFamily_StandardControls_UploadFlow_CarryNoSpanishMarker()
		{
			var offenders = new List<string>();
			foreach (var (path, keys) in new[]
			{
				(SharedResx, SharedKeysPass3),
				(FileUploadResx, FileUploadKeys),
				(WebControlsResx, WebControlsUploadKeys),
			})
			foreach (var key in keys)
			foreach (var marker in SpanishMarkers)
				if (marker.IsMatch(ResxValue(path, key)))
					offenders.Add($"{key} [{marker}]");
			offenders.Should().BeEmpty(
				"passe 3 : famille temporelle, contrôles standard et module d'upload pt-BR corrigés — " +
				"tout marqueur restant est une réinjection : {0}", string.Join(", ", offenders));
		}

		/// <summary>Le module d'upload WebControls est corrigé EN ENTIER (passe 3) :
		/// la garde marque-taux couvre les 22 clés du préfixe FileUpload.*, y
		/// compris celles qui étaient déjà saines — une réinjection n'importe où
		/// dans le module rougit, pas seulement sur les clés listées.</summary>
		[Fact]
		public void WebControls_WholeFileUploadModule_NoSpanishMarker()
		{
			var path = Path.Combine(TestRepoRoot.Find(), WebControlsResx.Replace('/', Path.DirectorySeparatorChar));
			var offenders = new List<string>();
			foreach (var d in XDocument.Load(path).Root!.Elements("data"))
			{
				var name = d.Attribute("name")?.Value ?? "";
				if (!name.StartsWith("FileUpload.", StringComparison.Ordinal))
					continue;
				var val = (d.Element("value")?.Value ?? d.Value).Trim();
				foreach (var marker in SpanishMarkers)
					if (marker.IsMatch(val))
						offenders.Add($"{name} [{marker}]");
			}
			offenders.Should().BeEmpty(
				"module d'upload WebControls pt-BR sain sur TOUTES ses clés (passe 3) : {0}",
				string.Join(", ", offenders));
		}

		[Fact]
		public void Pass3_Pins_ExactValues()
		{
			var pins = new Dictionary<(string path, string key), string>
			{
				[(SharedResx, "SecondsAgo.Text")] = "Há {0} segundos",
				[(SharedResx, "MinuteAgo.Text")] = "Há {0} minuto",
				[(SharedResx, "MinutesAgo.Text")] = "Há {0} minutos",
				[(SharedResx, "HourAgo.Text")] = "Há {0} hora",
				[(SharedResx, "HoursAgo.Text")] = "Há {0} horas",
				[(SharedResx, "DayAgo.Text")] = "Há {0} dia",
				[(SharedResx, "DaysAgo.Text")] = "Há {0} dias",
				[(SharedResx, "WeekAgo.Text")] = "Há {0} semana",
				[(SharedResx, "WeeksAgo.Text")] = "Há {0} semanas",
				[(SharedResx, "MonthAgo.Text")] = "Há {0} mês",
				[(SharedResx, "MonthsAgo.Text")] = "Há {0} meses",
				[(SharedResx, "Day.Text")] = "Dia",
				[(SharedResx, "Days.Text")] = "Dias",
				[(SharedResx, "Upload.Text")] = "Carregar",
				[(SharedResx, "cmdReturn.Text")] = "Voltar",
				[(SharedResx, "cmdStop.Text")] = "Parar",
				[(FileUploadResx, "ErrorMessage.Text")] = "Não foi possível carregar o arquivo '{0}': {1}.",
				[(WebControlsResx, "FileUpload.CloseButton.Text")] = "Fechar",
				[(WebControlsResx, "FileUpload.KeepButton.Text")] = "Manter",
				[(WebControlsResx, "FileUpload.ReplaceButton.Text")] = "Substituir",
				[(WebControlsResx, "FileUpload.FileUploadFailed.Error")] = "Erro no carregamento",
				[(WebControlsResx, "FileUpload.FileUploadCancelled.Error")] = "Carregamento cancelado",
				[(WebControlsResx, "FileUpload.UploadStopped.Text")] = "Carregamento de arquivo interrompido",
				[(WebControlsResx, "FileUpload.UrlTooltip.Text")] =
					"Insira a URL do recurso (por exemplo http://dominio.com/imagens/imagem.png)",
			};
			foreach (var ((path, key), expected) in pins)
				ResxValue(path, key).Should().Be(expected, "pin D3 passe 3 — clé visible {0}.", key);
		}

		// ===================== Passe 4 (19/10, dispatch ai-01 00:28) =====================

		// Les 12 clés V (visibles joueur/visiteur) du classement passe 3. Les
		// 3 clés modules citées par ai-01 (« Recordar inicio de sesión » ×2
		// Login.ascx, Security_EmailValidation.Help security.resx) vivent dans
		// des resx NON suivis git — geste webroot po-2023, hors PR.
		private static readonly string[] SharedKeysPass4 =
		{
			"Monthly.Text", "MyFolderName.Text", "PasswordInadequate.Text",
			"PrivateConfirmationMessage.Text", "PrivateMembership.Text",
			"PublicMembership.Text", "VerifiedMembership.Text", "Wizard.Success.FailureDetail",
		};

		private static readonly string[] GlobalKeysPass4 =
		{
			"//ActivityFeed//MyProfile.String", "404.Error", "EMAIL_MESSAGING_DISPATCH_BODY.Text",
		};

		private static readonly string[] WebControlsKeysPass4 = { "MyFolder.Text" };

		// Marqueurs passe 4 — espagnol macaronique des notes d'inscription et
		// du 404. Self-test : « Meu perfil », « Minha pasta », « Mensal »,
		// « domínio », « banco de dados », « etapas », « processo », « assinaturas »,
		// « direitos », « aqui », « site » miss ; les formes es hit.
		// ⚠ « dominio »/« proceso »/« pasos » sans accent ni double s = es ; les
		// formes pt sont « domínio »/« processo »/« passos ». Cette grille reste
		// HORS du Fact préfixe FileUpload.* : UrlTooltip cite l'URL exemple
		// « http://dominio.com/… » (pass 3) qui rougirait à tort.
		private static readonly Regex[] SpanishMarkersPass4 =
		{
			new(@"\bMi\s+(perfil|pasta)\b"),
			new(@"\bMensual\b"),
			new(@"\bdominio\b"),
			new(@"\bdatos\b"),
			new(@"\bpasos\b"),
			new(@"\bproceso\b"),
			new(@"\bsuscrip\w*", RegexOptions.IgnoreCase),
			new(@"\bderechos\b"),
			new(@"\baquí\b"),
			new(@"\bsitio\b"),
			new(@"\bhaya\b"),
		};

		[Fact]
		public void Pass4_VisibleKeys_NoSpanishMarker()
		{
			var offenders = new List<string>();
			foreach (var (path, keys) in new[]
			{
				(SharedResx, SharedKeysPass4),
				(GlobalResx, GlobalKeysPass4),
				(WebControlsResx, WebControlsKeysPass4),
			})
			foreach (var key in keys)
			foreach (var marker in SpanishMarkers.Concat(SpanishMarkersPass4))
				if (marker.IsMatch(ResxValue(path, key)))
					offenders.Add($"{key} [{marker}]");
			offenders.Should().BeEmpty(
				"passe 4 : notes d'inscription, 404, profil/dossier et courriel membres pt-BR corrigés — " +
				"tout marqueur restant est une réinjection : {0}", string.Join(", ", offenders));
		}

		[Fact]
		public void Pass4_Pins_ExactValues()
		{
			ResxValue(SharedResx, "Monthly.Text").Should().Be("Mensal");
			ResxValue(SharedResx, "MyFolderName.Text").Should().Be("Minha pasta");
			ResxValue(WebControlsResx, "MyFolder.Text").Should().Be("Minha pasta");
			ResxValue(GlobalResx, "//ActivityFeed//MyProfile.String").Should().Be("Meu perfil");
			ResxValue(SharedResx, "PasswordInadequate.Text").Should().Be(
				"A nova senha não é válida por motivos de segurança. Digite uma nova senha suficientemente " +
				"longa e complexa para cumprir os requisitos de segurança deste site.");
			ResxValue(SharedResx, "Wizard.Success.FailureDetail").Should().Be(
				"<p>Ocorreu um erro no assistente. Revise a mensagem exibida abaixo e notifique o seu provedor.</p>");
			ResxValue(SharedResx, "PublicMembership.Text").Should().Be(
				"<strong>*Nota:</strong> O registro neste site é público. Depois de enviar as suas informações " +
				"de registro, você terá acesso ao site.");
			foreach (var (key, expected) in new[]
			{
				("PrivateMembership.Text", "O registro neste site é privado."),
				("PrivateConfirmationMessage.Text", "Foi enviado um e-mail com os detalhes do seu registro"),
				("VerifiedMembership.Text", "um link para verificar a sua conta."),
			})
				ResxValue(SharedResx, key).Should().Contain(expected, "pin D3 passe 4 — {0}.", key);
			var err404 = ResxValue(GlobalResx, "404.Error");
			err404.Should().Contain("O domínio {0} não existe no banco de dados", "pin passe 4 — 404.Error.");
			err404.Should().Contain("Nome de domínio:", "pin passe 4 — les 5 lignes d'exemples URL du 404.");
			var email = ResxValue(GlobalResx, "EMAIL_MESSAGING_DISPATCH_BODY.Text");
			email.Should().Contain("Clique aqui para ver online.", "pin passe 4 — préheader du courriel membres.");
			email.Should().Contain("Gerencie as suas assinaturas", "pin passe 4 — pied du courriel membres.");
			email.Should().Contain("Todos os direitos reservados.", "pin passe 4 — copyright du courriel membres.");
			email.Should().Contain("cancelar a assinatura", "pin passe 4 — désabonnement du courriel membres.");
			foreach (var token in new[]
			{ "[NOTIFICATIONURL]", "[SITEURL]", "[LOGOURL]", "[PORTALNAME]", "[MESSAGEBODY]", "[UNSUBSCRIBEURL]" })
				email.Should().Contain(token, "EMAIL_MESSAGING_DISPATCH_BODY : token {0} préservé.", token);
		}
	}
}
