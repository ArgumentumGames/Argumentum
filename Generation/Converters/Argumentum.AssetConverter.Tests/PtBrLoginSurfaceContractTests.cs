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
	}
}
