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
	}
}
