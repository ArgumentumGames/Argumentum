using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 Tier 1 — the 13 measured chrome keys must exist in the three missing culture
	/// files (zh-CN, ar-SA, fa-IR), with their reviewed values, and nowhere else. DNN falls
	/// back per key, so these are deliberately partial files, not complete language packs.
	/// The shared French login label also pins the arbitrated lowercase « courriel ».
	/// </summary>
	public class ChromeTier1LocalizationContractTests
	{
		private static readonly string DnnRoot = Path.Combine(TestRepoRoot.Find(), "DNNPlatform");
		private static readonly string[] Cultures = { "zh-CN", "ar-SA", "fa-IR" };

		private static readonly (string Stem, string Key)[] Scope =
		{
			("admin/Skins/App_LocalResources/Login.ascx", "Login.Text"),
			("admin/Skins/App_LocalResources/Login.ascx", "Logout.Text"),
			("DesktopModules/Admin/Authentication/App_LocalResources/Login.ascx", "ControlTitle_.Text"),
			("DesktopModules/Admin/Authentication/App_LocalResources/Login.ascx", "ControlTitle_login.Text"),
			("DesktopModules/Admin/Authentication/App_LocalResources/Login.ascx", "ControlTitle_signin.Text"),
			("DesktopModules/AuthenticationServices/DNN/App_LocalResources/Login.ascx", "cmdLogin.Text"),
			("DesktopModules/AuthenticationServices/DNN/App_LocalResources/Login.ascx", "Email.Text"),
			("DesktopModules/AuthenticationServices/DNN/App_LocalResources/Login.ascx", "cmdPassword.Text"),
			("DesktopModules/AuthenticationServices/DNN/App_LocalResources/Login.ascx", "cmdRegister.Text"),
			("DesktopModules/AuthenticationServices/DNN/App_LocalResources/Login.ascx", "Remember.Text"),
			("App_GlobalResources/SharedResources", "Password.Text"),
			("App_GlobalResources/SharedResources", "cmdCancel.Text"),
			("App_GlobalResources/SharedResources", "Home.Text"),
		};

		private static readonly string[] Values =
		{
			// zh-CN — Login, Logout, three title variants, button, email, reset,
			// register, remember, password, cancel, Home.
			"登录", "退出登录", "用户登录", "用户登录", "用户登录", "登录", "电子邮件地址：",
			"重置密码", "注册", "记住登录状态", "密码：", "取消", "首页",
			// ar-SA
			"تسجيل الدخول", "تسجيل الخروج", "تسجيل الدخول", "تسجيل الدخول", "تسجيل الدخول",
			"تسجيل الدخول", "عنوان البريد الإلكتروني:", "إعادة تعيين كلمة المرور", "تسجيل",
			"تذكر تسجيل الدخول", "كلمة المرور:", "إلغاء", "الرئيسية",
			// fa-IR
			"ورود", "خروج", "ورود کاربر", "ورود کاربر", "ورود کاربر", "ورود", "نشانی ایمیل:",
			"بازنشانی رمز عبور", "ثبت‌نام", "مرا به خاطر بسپار", "رمز عبور:", "لغو", "خانه",
		};

		public static IEnumerable<object[]> ExpectedCells()
		{
			for (var cultureIndex = 0; cultureIndex < Cultures.Length; cultureIndex++)
			{
				for (var keyIndex = 0; keyIndex < Scope.Length; keyIndex++)
				{
					var (stem, key) = Scope[keyIndex];
					yield return new object[]
					{
						stem, key, Cultures[cultureIndex], Values[cultureIndex * Scope.Length + keyIndex],
					};
				}
			}
		}

		[Theory]
		[MemberData(nameof(ExpectedCells))]
		public void EachTier1CellHasItsReviewedValue(string stem, string key, string culture, string expected)
		{
			var file = Path.Combine(DnnRoot, stem + "." + culture + ".resx");
			Assert.True(File.Exists(file), $"missing culture file: {file}");
			var data = XDocument.Load(file).Root!.Elements("data")
				.Where(e => (string?)e.Attribute("name") == key).ToArray();
			Assert.True(data.Length == 1, $"{culture}:{key} must occur exactly once in {stem}");
			Assert.Equal(expected, (string?)data[0].Element("value"));
			Assert.NotEqual("", expected);
		}

		[Fact]
		public void ScopeIsThirteenKeysAndPartialFilesContainNoExtraKeys()
		{
			Assert.Equal(13, Scope.Length);
			Assert.Equal(13 * 3, ExpectedCells().Count());
			foreach (var group in Scope.GroupBy(x => x.Stem))
			{
				var expectedKeys = group.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal).ToArray();
				foreach (var culture in Cultures)
				{
					var file = Path.Combine(DnnRoot, group.Key + "." + culture + ".resx");
					var actualKeys = XDocument.Load(file).Root!.Elements("data")
						.Select(e => (string?)e.Attribute("name")).OrderBy(x => x, StringComparer.Ordinal).ToArray();
					Assert.Equal(expectedKeys, actualKeys);
				}
			}
		}

		[Fact]
		public void FrenchEmailLabelUsesLowercaseCourriel()
		{
			var file = Path.Combine(DnnRoot,
				"DesktopModules/AuthenticationServices/DNN/App_LocalResources/Login.ascx.fr-FR.resx");
			var data = XDocument.Load(file).Root!.Elements("data")
				.Single(e => (string?)e.Attribute("name") == "Email.Text");
			Assert.Equal("Adresse de courriel :", (string?)data.Element("value"));
		}

		/// <summary>
		/// #458 — the pt-BR skin files were byte-copies of es-ES (« Iniciar » / « Salir »).
		/// The arbitrated pt-BR chrome labels are « Entrar » / « Sair » (ai-01, 26/10).
		/// </summary>
		public static IEnumerable<object[]> PtBrSkinLoginCells()
		{
			yield return new object[] { "admin/Skins/App_LocalResources/Login.ascx", "Login.Text", "Entrar" };
			yield return new object[] { "admin/Skins/App_LocalResources/Login.ascx", "Logout.Text", "Sair" };
			yield return new object[] { "admin/Skins/App_LocalResources/UserAndLogin.ascx", "Login.Text", "Entrar" };
			yield return new object[] { "admin/Skins/App_LocalResources/UserAndLogin.ascx", "Logout.Text", "Sair" };
		}

		[Theory]
		[MemberData(nameof(PtBrSkinLoginCells))]
		public void PtBrSkinLoginLabelsUseEntrarSair(string stem, string key, string expected)
		{
			var file = Path.Combine(DnnRoot, stem + ".pt-BR.resx");
			Assert.True(File.Exists(file), $"missing culture file: {file}");
			var data = XDocument.Load(file).Root!.Elements("data")
				.Where(e => (string?)e.Attribute("name") == key).ToArray();
			Assert.True(data.Length == 1, $"pt-BR:{key} must occur exactly once in {stem}");
			Assert.Equal(expected, (string?)data[0].Element("value"));
		}

		[Fact]
		public void PtBrSkinFilesCarryNoSpanishLoginLabels()
		{
			// Pins the defect signature, not just the new values: both carriers must
			// be free of the es-ES strings they were copied from. Bounded to these two
			// skin files — « Iniciar » is legitimate pt elsewhere (Start, installer).
			foreach (var stem in new[]
			{
				"admin/Skins/App_LocalResources/Login.ascx",
				"admin/Skins/App_LocalResources/UserAndLogin.ascx",
			})
			{
				var text = File.ReadAllText(Path.Combine(DnnRoot, stem + ".pt-BR.resx"));
				Assert.DoesNotContain("Iniciar", text);
				Assert.DoesNotContain("Salir", text);
			}
		}
	}
}
