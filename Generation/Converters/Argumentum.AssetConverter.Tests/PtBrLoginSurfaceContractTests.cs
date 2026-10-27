using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 D4 — the pt-BR login surface was seeded from es-ES and mixes Spanish words
	/// (« contrasenha », « Estándar », « usuario », « Iniciar »). This contract pins the
	/// re-translated values, file by file, and asserts the declared hispanisms are gone.
	/// « Iniciar » stays legitimate outside the login surface (Start, installer) — the
	/// signature is asserted per login file, never across all pt-BR resources.
	/// </summary>
	public class PtBrLoginSurfaceContractTests
	{
		private static readonly string DnnRoot = Path.Combine(TestRepoRoot.Find(), "DNNPlatform");

		// D4 file 1 — the login form itself (first thing a visitor meets).
		private const string AuthServicesLogin =
			"DesktopModules/AuthenticationServices/DNN/App_LocalResources/Login.ascx";

		// D4 file 2 — the authentication control (associate/create/password lifecycle),
		// macaronic es-PT end to end before the 26/10 retranslation.
		private const string AdminAuthLogin =
			"DesktopModules/Admin/Authentication/App_LocalResources/Login.ascx";

		// D4 file 3 — the UserAndLogin skin object (header user menu). Registered by
		// nvQuickTheme but not placed by any committed skin: latent surface, fixed anyway.
		private const string UserAndLogin = "admin/Skins/App_LocalResources/UserAndLogin.ascx";

		public static IEnumerable<object[]> File1Cells()
		{
			yield return new object[] { AuthServicesLogin, "cmdLogin.Text", "Entrar" };
			yield return new object[] { AuthServicesLogin, "cmdPassword.Text", "Redefinir senha" };
			yield return new object[] { AuthServicesLogin, "cmdRegister.Text", "Registrar" };
			yield return new object[] { AuthServicesLogin, "Email.Text", "Endereço de e-mail:" };
			yield return new object[] { AuthServicesLogin, "InvalidVerificationCode.Text", "Código de verificação incorreto" };
			yield return new object[] { AuthServicesLogin, "Remember.Text", "Lembrar login" };
			yield return new object[] { AuthServicesLogin, "Title.Text", "Padrão" };
			yield return new object[] { AuthServicesLogin, "UserAlreadyVerified.Text", "Usuário já verificado" };
			yield return new object[] { AuthServicesLogin, "UserDoesNotExist.Text", "Usuário não existe" };
		}

		[Theory]
		[MemberData(nameof(File1Cells))]
		public void AuthServicesLoginPtBrHasItsReviewedValues(string stem, string key, string expected)
		{
			var file = Path.Combine(DnnRoot, stem + ".pt-BR.resx");
			Assert.True(File.Exists(file), $"missing culture file: {file}");
			var data = XDocument.Load(file).Root!.Elements("data")
				.Where(e => (string?)e.Attribute("name") == key).ToArray();
			Assert.True(data.Length == 1, $"pt-BR:{key} must occur exactly once in {stem}");
			Assert.Equal(expected, (string?)data[0].Element("value"));
		}

		[Fact]
		public void AuthServicesLoginPtBrCarriesNoDeclaredHispanisms()
		{
			// Case-sensitive on purpose: « Usuário » is the correct pt-BR word and must
			// never match. The list is the defect signature measured on 26/10.
			var text = File.ReadAllText(Path.Combine(DnnRoot, AuthServicesLogin + ".pt-BR.resx"));
			foreach (var hispanism in new[] { "Iniciar", "contrasenha", "Estándar", "usuario", "validadeo" })
			{
				Assert.DoesNotContain(hispanism, text);
			}
		}

		public static IEnumerable<object[]> File2Cells()
		{
			yield return new object[] { AdminAuthLogin, "AssociateHelp.Text", "Você pode associar seus dados de login a uma conta de usuário existente informando seu nome de usuário e senha." };
			yield return new object[] { AdminAuthLogin, "AssociateTitle.Text", "Associar a uma conta existente" };
			yield return new object[] { AdminAuthLogin, "AssociationFailed.Text", "Erro ao associar a identidade ao usuário. Lembre-se de que nomes de usuário e senhas diferenciam maiúsculas de minúsculas." };
			yield return new object[] { AdminAuthLogin, "AuthenticatedHelp.Text", "Você foi autenticado com sucesso. No entanto, esta identidade não é membro deste site. Você pode associar esta identidade a uma conta de usuário existente (informando o nome de usuário e a senha) ou registrar-se como novo membro." };
			yield return new object[] { AdminAuthLogin, "AuthenticatedTitle.Text", "Autenticado" };
			yield return new object[] { AdminAuthLogin, "cmdAssociate.Text", "Associar a uma conta de usuário" };
			yield return new object[] { AdminAuthLogin, "cmdCreate.Text", "Criar uma nova conta" };
			yield return new object[] { AdminAuthLogin, "cmdForgotPassword.Text", "Esqueceu sua senha?" };
			yield return new object[] { AdminAuthLogin, "cmdPassword.Text", "Recuperar senha" };
			yield return new object[] { AdminAuthLogin, "cmdProceed.Text", "Continuar mesmo assim (alterar a senha depois)" };
			yield return new object[] { AdminAuthLogin, "cmdRegister.Text", "Registrar" };
			yield return new object[] { AdminAuthLogin, "ControlTitle_.Text", "Login do usuário" };
			yield return new object[] { AdminAuthLogin, "ControlTitle_login.Text", "Login do usuário" };
			yield return new object[] { AdminAuthLogin, "ControlTitle_signin.Text", "Login do usuário" };
			yield return new object[] { AdminAuthLogin, "CreateTitle.Text", "Criar uma nova conta" };
			yield return new object[] { AdminAuthLogin, "EnterCode.Text", "Informe seu código de verificação" };
			yield return new object[] { AdminAuthLogin, "FailedToRemoveAccount.Text", "Não foi possível remover a conta. Entre em contato com o administrador do site e informe que você deseja ser removido." };
			yield return new object[] { AdminAuthLogin, "IPAddressBanned.Text", "Você não pode fazer login neste site a partir deste endereço IP." };
			yield return new object[] { AdminAuthLogin, "InvalidCode.Text", "Código de verificação incorreto" };
			yield return new object[] { AdminAuthLogin, "LoginFailed.Text", "Falha no login. Lembre-se de que as senhas diferenciam maiúsculas de minúsculas." };
			yield return new object[] { AdminAuthLogin, "MustConsent.Text", "Você deve aceitar nossos termos para continuar" };
			yield return new object[] { AdminAuthLogin, "PasswordExpired.Text", "Sua senha expirou em {0}. Atualize sua senha antes de continuar." };
			yield return new object[] { AdminAuthLogin, "PasswordExpiring.Text", "Sua senha expirará em {0}. Atualize sua senha antes de continuar." };
			yield return new object[] { AdminAuthLogin, "PasswordInvalid.Text", "Você deve informar uma senha válida. Entre em contato com o administrador do site se não souber os requisitos de senha." };
			yield return new object[] { AdminAuthLogin, "PasswordMismatch.Text", "As senhas informadas não coincidem." };
			yield return new object[] { AdminAuthLogin, "PasswordMissing.Text", "Você deve fornecer sua senha atual para alterar a senha." };
			yield return new object[] { AdminAuthLogin, "PasswordNotDifferent.Text", "A nova senha é igual à senha anterior. Informe uma senha diferente." };
			yield return new object[] { AdminAuthLogin, "PasswordSent.Text", "A senha foi enviada para<br>seu endereço de e-mail." };
			yield return new object[] { AdminAuthLogin, "PasswordUpdate.Text", "Atualize sua senha antes de continuar." };
			yield return new object[] { AdminAuthLogin, "plToken.Help", "O token exclusivo que identifica o usuário." };
			yield return new object[] { AdminAuthLogin, "plToken.Text", "Token do usuário:" };
			yield return new object[] { AdminAuthLogin, "plType.Help", "O tipo de autenticação" };
			yield return new object[] { AdminAuthLogin, "plType.Text", "Tipo:" };
			yield return new object[] { AdminAuthLogin, "ProfileUpdate.Text", "Atualize seu perfil antes de continuar." };
			yield return new object[] { AdminAuthLogin, "RegisterTitle.Text", "Registrar uma nova conta" };
			yield return new object[] { AdminAuthLogin, "Remember.Text", "Lembrar login" };
			yield return new object[] { AdminAuthLogin, "UserLockedOut.Text", "Esta conta foi bloqueada após muitas tentativas de login malsucedidas. Aguarde {0} minutos antes de tentar novamente. Se você esqueceu sua senha, use a opção \"Recuperar senha\" antes de entrar em contato com o administrador do site." };
			yield return new object[] { AdminAuthLogin, "UserLockedOut_ContactAdmin.Text", "Esta conta foi bloqueada após muitas tentativas de login malsucedidas. Entre em contato com o administrador do site." };
			yield return new object[] { AdminAuthLogin, "UsernameError.Text", "O nome do usuário não existe" };
			yield return new object[] { AdminAuthLogin, "UserNotAuthorized.Text", "No momento, você não está autorizado a fazer login neste site." };
		}

		[Theory]
		[MemberData(nameof(File2Cells))]
		public void AdminAuthLoginPtBrHasItsReviewedValues(string stem, string key, string expected)
		{
			var file = Path.Combine(DnnRoot, stem + ".pt-BR.resx");
			Assert.True(File.Exists(file), $"missing culture file: {file}");
			var data = XDocument.Load(file).Root!.Elements("data")
				.Where(e => (string?)e.Attribute("name") == key).ToArray();
			Assert.True(data.Length == 1, $"pt-BR:{key} must occur exactly once in {stem}");
			Assert.Equal(expected, (string?)data[0].Element("value"));
		}

		[Fact]
		public void AdminAuthLoginPtBrCarriesNoDeclaredHispanisms()
		{
			// The 26/10 defect signature of the macaronic file, bounded to this file only:
			// Spanish tokens and macaronic constructs measured before the retranslation.
			// « associar », « registrar-se », « aceitar », « utilize » are the pt-BR words
			// and never match their Spanish look-alikes below (case-sensitive).
			var text = File.ReadAllText(Path.Combine(DnnRoot, AdminAuthLogin + ".pt-BR.resx"));
			foreach (var hispanism in new[]
			{
				"asociar", "Asociar", "sesión", "contrasenha", "Olvidó", "Iniciar", "mayúsculas",
				"produci", "Póngase", "ponerse", "Actualice", "identificarse", "registrarse",
				"utilice", "desconoce", "aceptar", "foi sido", "sensívels", "caducó", "caducará",
			})
			{
				Assert.DoesNotContain(hispanism, text);
			}
		}

		public static IEnumerable<object[]> File3Cells()
		{
			yield return new object[] { UserAndLogin, "Account.Text", "Editar conta" };
			yield return new object[] { UserAndLogin, "EditProfile.Text", "Editar perfil" };
			yield return new object[] { UserAndLogin, "Login.Text", "Entrar" };
			yield return new object[] { UserAndLogin, "Logout.Text", "Sair" };
			yield return new object[] { UserAndLogin, "MessageCount.Text", "{0} mensagens" };
			yield return new object[] { UserAndLogin, "Messages.Text", "Mensagens" };
			yield return new object[] { UserAndLogin, "Notifications.Text", "Notificações" };
			yield return new object[] { UserAndLogin, "OneMessage.Text", "1 mensagem" };
			yield return new object[] { UserAndLogin, "Profile.Text", "Meu perfil" };
			yield return new object[] { UserAndLogin, "ProfilePicture.Text", "Minha foto de perfil" };
			yield return new object[] { UserAndLogin, "Register.Text", "Registrar" };
		}

		[Theory]
		[MemberData(nameof(File3Cells))]
		public void UserAndLoginPtBrHasItsReviewedValues(string stem, string key, string expected)
		{
			var file = Path.Combine(DnnRoot, stem + ".pt-BR.resx");
			Assert.True(File.Exists(file), $"missing culture file: {file}");
			var data = XDocument.Load(file).Root!.Elements("data")
				.Where(e => (string?)e.Attribute("name") == key).ToArray();
			Assert.True(data.Length == 1, $"pt-BR:{key} must occur exactly once in {stem}");
			Assert.Equal(expected, (string?)data[0].Element("value"));
		}

		[Fact]
		public void UserAndLoginPtBrCarriesNoDeclaredHispanisms()
		{
			// Bounded to this file: the es-ES seed values that survived in pt-BR on 26/10,
			// plus the es-ES words of its sibling cells. Case-sensitive — « Mensagens »,
			// « Notificações », « Sair » are the pt-BR words and never match.
			var text = File.ReadAllText(Path.Combine(DnnRoot, UserAndLogin + ".pt-BR.resx"));
			foreach (var hispanism in new[]
			{
				"Mi perfil", "Mi imagen", "imagen", "Iniciar", "Salir", "cuenta", "mensaje", "Notificaciones",
			})
			{
				Assert.DoesNotContain(hispanism, text);
			}
		}
	}
}
