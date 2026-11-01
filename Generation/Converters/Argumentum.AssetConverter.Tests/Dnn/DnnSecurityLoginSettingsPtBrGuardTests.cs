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
	/// #458 — onglet <b>Login / Basic Settings</b> de <c>Dnn.Security/App_LocalResources/security.pt-BR.resx</c>.
	///
	/// <para><b>Perimetre derive du composant.</b> Les cles viennent du segment
	/// <c>BasicSettingsPanelBody</c> (offsets 79419..92840 du bundle <c>security-settings-bundle.js</c>) :
	/// <b>24 ids</b>. Deux d'entre eux — <c>BasicLoginSettingsUpdateSuccess</c> et
	/// <c>BasicLoginSettingsError</c> — n'ont <b>pas</b> de suffixe <c>.Text</c> dans le resx : la regle
	/// « id sans point =&gt; id.Text », valable pour l'onglet IP filters, est donc <b>fausse</b> ici ; le
	/// generateur essaie les deux formes et garde celle qui existe au neutre. <c>SwitchOn.Text</c> /
	/// <c>SwitchOff.Text</c> sont absents du neutre (ressources globales, presentes en es-ES seule) :
	/// hors fichier, nommees, non oubliees. =&gt; <b>22 cles livrees ici</b>. L'etiquette du pool disait
	/// « 17 » : ecart mesure, documente dans la PR.</para>
	///
	/// <para><b>Etat livre : 16 valeurs reecrites, 0 cle creee</b> (362 -&gt; 362 cles). L'onglet etait
	/// <b>traduit depuis l'espagnol</b>, pas depuis le neutre : <c>BasicLoginSettingsError</c> finissait
	/// par « Vuelva a intentarlo. » et <c>RequireValidProfileAtLogin.Text</c> etait du castillan pur
	/// (« Requerir perfil válido para iniciar sesión »). Meme famille de defaut que l'onglet IP filters
	/// (#2104), ou la typo <c>fitro</c> heritee de l'es l'avait prouve : la bonne question n'est pas
	/// « est-ce du portugais ? » mais <b>« de quelle langue vient cette cellule ? »</b>.</para>
	///
	/// <para><b>Marqueurs</b> : grille calibree sur l'es-ES de l'onglet, assertion bornee a l'onglet.
	/// Chaque marqueur est verifie NON AVEUGLE (present dans l'es) et ABSENT du pt livre. Sont
	/// <b>ecartes par mesure</b>, pas par gout : <c>predeterminado</c> (adjectif portugais valide -&gt;
	/// forme historique, pas marqueur) et <c>utilizar</c> (verbe portugais valide -&gt; traite en
	/// terminologie, « Usar »). <c>contrasenha</c>, <c>actividades dos miembros</c> et <c>tienem</c>
	/// n'existent <b>pas</b> en es-ES : ce sont des hybrides, donc des formes historiques, pas des
	/// marqueurs — un marqueur aveugle sur l'es calibrerait l'ecran sur du vide.</para>
	///
	/// <para><b>Deux homographes declares et vivants</b> : <c>Administrador principal</c> et
	/// <c>Cancelar</c> sont identiques en es et en pt (le neutre dit « Primary Administrator » /
	/// « Cancel »). L'exemption est comptee, nommee et re-verifiee a chaque execution.</para>
	///
	/// <para><b>Suite de la decoupe (mesure, pas promesse)</b> : <c>TabBasicLoginSettings.Text</c> porte
	/// encore « CONFIGURAÇÃO BÁSICA DE IDENTIFICAÇÃO » alors que ce volet ecrit « login » dans le corps
	/// de l'onglet. Cet intitule vit dans la barre d'onglets (<c>Root</c>), hors du composant mesure :
	/// il est laisse a un volet ulterieur <b>nomme</b>, avec <c>TabIpFilters.Text</c> (« FILTROS IP DE
	/// LOGIN », deja aligne) pour temoin de la cible.</para>
	/// </summary>
	public class DnnSecurityLoginSettingsPtBrGuardTests
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

		/// <summary>Les 22 cles de l'onglet — derivees du composant, pas d'une liste de prefixes.</summary>
		private static readonly string[] TabKeys =
		{
			"BasicLoginSettingsUpdateSuccess", "BasicLoginSettingsError", "LoginSettingsRestoreWarning.Text",
			"Yes.Text", "No.Text", "DefaultAuthProvider.Help",
			"DefaultAuthProvider.Text", "plAdministrator.Help", "plAdministrator.Text",
			"RedirectionMovedToSiteSettings.Text", "Security_RequireValidProfileAtLogin.Help", "Security_RequireValidProfileAtLogin.Text",
			"Security_CaptchaLogin.Help", "Security_CaptchaLogin.Text", "Security_CaptchaRetrivePassword.Help",
			"Security_CaptchaRetrivePassword.Text", "Security_CaptchaChangePassword.Help", "Security_CaptchaChangePassword.Text",
			"plHideLoginControl.Help", "plHideLoginControl.Text", "Cancel.Text",
			"Save.Text",
		};

		/// <summary>Marqueurs espagnols de l'onglet : chacun est present dans l'es-ES de l'onglet
		/// (verifie par <c>Marker_Grid_Is_Non_Vacuous</c>) et absent du pt livre.</summary>
		private static readonly string[] SpanishMarkers =
		{
			"asociar usuarios", "cancelar los cambios", "enlace a la", "formularios",
			"identificaci\u00f3n", "iniciar sesi\u00f3n", "recibir\u00e1", "requerir perfil",
			"se ha podido", "si se va a utilizar", "soporten", "vuelva a intentarlo",
		};

		/// <summary>Formes historiques de l'ONGLET, chacune temoignee par le litteral de l'ancienne
		/// cellule pt (3e element). Le generateur refuse une forme sans temoin et une forme survivante.</summary>
		private static readonly (string Form, string Why, string Witness)[] HistoricalForms =
		{
			("foi actualizado a configura\u00e7\u00e3o", "pt-PT + inversion (pt-BR : \u00ab foram atualizadas \u00bb)", "foi actualizado a configura\u00e7\u00e3o."),
			("foi podido", "castillan (\u00ab no fue posible \u00bb)", "N\u00e3o foi podido guardar a configura\u00e7\u00e3o de identifica\u00e7\u00e3o. Vuelva a intentarlo."),
			("Vuelva a intentarlo", "castillan", "N\u00e3o foi podido guardar a configura\u00e7\u00e3o de identifica\u00e7\u00e3o. Vuelva a intentarlo."),
			("Est\u00e1 seguro que quer", "castillan", "Est\u00e1 seguro que quer cancelar os cambios?"),
			("os cambios", "castillan", "Est\u00e1 seguro que quer cancelar os cambios?"),
			("predeterminado", "castillan \u2014 pt-BR ecrit \u00ab padr\u00e3o \u00bb (corpus 70 vs 56)", "Provedor de autentica\u00e7\u00e3o predeterminado"),
			("para a identifica\u00e7\u00e3o de usuarios", "castillan", "Pode selecionar o provedor de autentica\u00e7\u00e3o predeterminado para a identifica\u00e7\u00e3o de usuarios. somente pode selecionar os provedores que soporten autentica\u00e7\u00e3o de formularios."),
			("soporten", "castillan (\u00ab suportem \u00bb)", "Pode selecionar o provedor de autentica\u00e7\u00e3o predeterminado para a identifica\u00e7\u00e3o de usuarios. somente pode selecionar os provedores que soporten autentica\u00e7\u00e3o de formularios."),
			("actividades dos miembros", "hybride es/pt (\u00ab atividades dos membros \u00bb)", "O usu\u00e1rio administrador do portal que recibir\u00e1 notifica\u00e7\u00f5es das actividades dos miembros."),
			("tienem que completar", "castillan + faute (\u00ab t\u00eam \u00bb)", "Ative esta op\u00e7\u00e3o se os usuarios tienem que completar todos os campos obrigat\u00f3rios durante o inicio de sesi\u00f3n."),
			("inicio de sesi\u00f3n", "castillan (\u00ab login \u00bb)", "Ative esta op\u00e7\u00e3o se os usuarios tienem que completar todos os campos obrigat\u00f3rios durante o inicio de sesi\u00f3n."),
			("Requerir perfil", "castillan (\u00ab Exigir perfil \u00bb)", "Requerir perfil v\u00e1lido para iniciar sesi\u00f3n"),
			("Mostra si se va a utilizar", "castillan (\u00ab Marque esta op\u00e7\u00e3o \u00bb)", "Mostra si se va a utilizar um controle CAPTCHA para poder asociar usuarios por ejemplo OpenId, LiveId, Cardspace."),
			("para asociar usuarios", "castillan (\u00ab para associar logins \u00bb)", "Utilizar CAPTCHA para asociar usuarios"),
			("contrasenha", "hybride es/pt \u2014 pt-BR ecrit \u00ab senha \u00bb", "Mostra si se va a utilizar um controle CAPTCHA para recuperar a contrasenha"),
			("o cambio de contrasenha", "castillan (\u00ab a altera\u00e7\u00e3o de senhas \u00bb)", "Entre se quiser utilizar um controle CAPTCHA para o cambio de contrasenha"),
			("enlace a p\u00e1gina", "hybride es/pt (\u00ab o link de login na p\u00e1gina \u00bb)", "Marque esta op\u00e7\u00e3o para ocultar o enlace a p\u00e1gina de identifica\u00e7\u00e3o."),
			("controle de identifica\u00e7\u00e3o", "calque (\u00ab controle de login \u00bb)", "Ocultar controle de identifica\u00e7\u00e3o"),
		};

		/// <summary>Les 22 cellules livrees : empreinte SHA-256 de la valeur PLEINE (derivee du fichier).</summary>
		private static readonly (string Key, string Sha256)[] Pins =
		{
			("BasicLoginSettingsUpdateSuccess", "a8e9ab39fa6c65988a5da74ba6c3e4f12cb2365a952ac773cb62d60fcf30be97"),
			("BasicLoginSettingsError", "db0ae306c592c9a2925aa5a1f530bb9edbc438e6ffbb85e25f9ceb903c08f81c"),
			("LoginSettingsRestoreWarning.Text", "88a77a9679ffcd2a0d00f68aacf593df9cbb274c25c3d3a37517529a0f2ca15b"),
			("Yes.Text", "9313455e45aa22430a92fddbb70f21312d3c944ab590dfb1da845c1a96b664fe"),
			("No.Text", "9bc947e58b62b2fef2954dc2813f3bfe458455fea1bb40b2d7a33e010dfe9ab6"),
			("DefaultAuthProvider.Help", "29a54bae4932cb6da2064f01866e913bd295ff06d9e630c196b8de6d0669aeff"),
			("DefaultAuthProvider.Text", "b798f3366ba3fa54f8b11baf2bba4bfa8fd36f9161114e0c86c2ae83731eee3d"),
			("plAdministrator.Help", "6286ba5c4e59754bf55dcd56c28dff11cc652203ca9fa15bc623abc515e444e4"),
			("plAdministrator.Text", "69b1e69a3dc8666525d30c36062940b61104de58a5fc8c495cc8c259beb45c52"),
			("RedirectionMovedToSiteSettings.Text", "b0cc68e245d8c819b7f79cb85ea1486293d3455a0b0954efa5e76c0f3006abc3"),
			("Security_RequireValidProfileAtLogin.Help", "a5e9f3d40e99c3c15565d278a2fadd5626a4de201a8cdf1f690bb15d35e37588"),
			("Security_RequireValidProfileAtLogin.Text", "7c919dd6fe7c094def68d096fbd0348a24d967f80eae8bef3baf42918535d627"),
			("Security_CaptchaLogin.Help", "9a549e234f44ddba488ed0449200ea8a896ae69d3df5d5efe89544ba10f6eb6a"),
			("Security_CaptchaLogin.Text", "f825597c8ec8343caa9b1b3661a798e26df3c1a8c008725446c705a8c9241fae"),
			("Security_CaptchaRetrivePassword.Help", "8a24ab07affc2b1595405921572ba2b3962ba2a2e1a48dbf7113c6ce82d7b2b2"),
			("Security_CaptchaRetrivePassword.Text", "040a679f9c189fab69dd5911bb68eeb1bb74eeb12b865b0aabb0d410479355c4"),
			("Security_CaptchaChangePassword.Help", "785c2f9573e575833b457f8b004e7ce008a92482dc1a68a7be76bbace9046ae6"),
			("Security_CaptchaChangePassword.Text", "5b144fa1f59a647a39c4bd9ecb0cb456fa0db4037b019e71c3ed43a406a3bec4"),
			("plHideLoginControl.Help", "baad8dae33a3f29f212ded29c60adf98f93b5127aeecf4ca2827367bbb080773"),
			("plHideLoginControl.Text", "00f6d4cf769a024ca6c15b5c0dbe422acf67c67f180595b247a050e32374e624"),
			("Cancel.Text", "bb9dbb406dcbd5248babc609bb3d021d384398f83dbdb8d53c9b0e32c2aad915"),
			("Save.Text", "aef7cd5b2081351dcd4b3767cc84f77797ee1a945e758e8013961c283304b947"),
		};

		/// <summary>Homographes LEGITIMES : la cellule pt egale l'es parce que le mot est identique
		/// dans les deux langues. Toute entree doit rester VIVANTE (verifie ci-dessous).</summary>
		private static readonly (string Key, string Word, string NeutralWitness)[] DeclaredHomographs =
		{
			("plAdministrator.Text", "Administrador principal", "Primary Administrator"),
			("Cancel.Text", "Cancelar", "Cancel"),
		};

		[Fact]
		public void Login_Tab_Keys_All_Present()
		{
			var pt = Load(PtResx);
			TabKeys.Where(k => !pt.ContainsKey(k)).Should().BeEmpty(
				"#458 : les 22 cles de l'onglet Login sont livrees (aucune creation dans ce volet)");
			TabKeys.Should().HaveCount(22);
			TabKeys.Distinct().Should().HaveCount(22);
		}

		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_The_Tab_Es()
		{
			// un ecran qui ne voit rien dans l'es n'etablit rien : chaque marqueur doit avoir au moins
			// une cellule es de l'onglet pour temoin.
			var es = Load(EsResx);
			var blind = SpanishMarkers
				.Where(m => !TabKeys.Any(k => es[k].ToLowerInvariant().Contains(m)))
				.ToList();
			blind.Should().BeEmpty("un marqueur aveugle sur l'es calibrerait l'ecran sur du vide");
			SpanishMarkers.Should().HaveCount(12).And.OnlyHaveUniqueItems();
			SpanishMarkers.Should().NotContain("predeterminado",
				"« predeterminado » est un adjectif portugais valide : forme historique, jamais marqueur");
			SpanishMarkers.Should().NotContain("utilizar",
				"« utilizar » est un verbe portugais valide : traite en terminologie, pas en marqueur");
		}

		[Fact]
		public void Login_Tab_Carries_No_Spanish_Marker()
		{
			var pt = Load(PtResx);
			var offenders = TabKeys
				.SelectMany(k => SpanishMarkers
					.Where(m => pt[k].ToLowerInvariant().Contains(m))
					.Select(m => $"{k} : marqueur «{m}»"))
				.ToList();
			offenders.Should().BeEmpty("#458 : 16 cellules reecrites, 0 marqueur restant (mesure)");
		}

		[Fact]
		public void Historical_Forms_Are_Gone_And_Each_Has_A_Witness()
		{
			var pt = Load(PtResx);
			HistoricalForms.Should().HaveCount(18, "liste figee : chaque forme est temoignee");
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
			foreach (var key in TabKeys)
			{
				var expected = System.Text.RegularExpressions.Regex.Matches(neu[key], @"\d+").Select(m => m.Value);
				var actual = System.Text.RegularExpressions.Regex.Matches(pt[key], @"\d+").Select(m => m.Value);
				actual.Should().Equal(expected, $"les nombres de {key} sont ceux du neutre");
			}
		}

		[Fact]
		public void Pins_Exact_Values()
		{
			var pt = Load(PtResx);
			Pins.Should().HaveCount(22);
			var offenders = Pins
				.Where(p => !pt.ContainsKey(p.Key) || Sha256Hex(pt[p.Key]) != p.Sha256)
				.Select(p => p.Key)
				.ToList();
			Pins.Should().NotContain(p => p.Sha256.Length != 64, "empreinte tronquee = epinglage mort");
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
			// « padrão » : 70 cellules du corpus pt-BR contre 56 pour « predeterminado », et ces 56 sont
			// majoritairement de la contamination castillane (« Su cuenta ... contrasenha predeterminada »).
			pt["DefaultAuthProvider.Text"].Should().Be("Provedor de autenticação padrão");
			pt["Security_CaptchaLogin.Text"].Should().StartWith("Usar CAPTCHA",
				"« Usar » est la forme des cellules CAPTCHA deja conformes (Security_CaptchaRegister.Text)");
			pt["Security_CaptchaChangePassword.Text"].Should().StartWith("Usar CAPTCHA");
			pt["Security_CaptchaRetrivePassword.Text"].Should().StartWith("Usar CAPTCHA");
			pt["plHideLoginControl.Text"].Should().Contain("login").And.NotContain("identificação");
			pt["plAdministrator.Help"].Should().Contain("atividades").And.Contain("membros");
			pt["BasicLoginSettingsError"].Should().Contain("Tente novamente mais tarde",
				"precedent MemberSettingsError.Text / IpFilterUpdateError.Text du meme fichier");
			pt["LoginSettingsRestoreWarning.Text"].Should().StartWith("Tem certeza de que deseja",
				"precedent #2102 / #2104");
			TabKeys.Where(k => pt[k].Contains("actualizar", StringComparison.Ordinal)).Should().BeEmpty(
				"« actualizar » est la forme pt-PT ; le corpus pt-BR ecrit « atualizar »");
			TabKeys.Where(k => pt[k].Contains("contrasenha", StringComparison.Ordinal)).Should().BeEmpty(
				"le pt-BR ecrit « senha » (280 cellules) et non « contrasenha » (100, toutes contaminees)");
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
			offenders.Should().BeEmpty("six cellules egales a l'es avant reecriture ; deux le restent legitimement");
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
			// plancher (pas compte exact) : le compte EXACT vit dans la garde du volet COURANT.
			// Ce volet n'ajoute aucune cle -> 362 ici ; les onglets suivants de la decoupe monteront.
			pt.Should().HaveCountGreaterThanOrEqualTo(362,
				"362 cles apres #2104 (IP filters). Les volets suivants ajoutent les leurs : le compte EXACT vit dans leur garde");
			foreach (var key in new[] { "Security_UserNameMinLength.Text", "IpFilterUpdateError.Text", "plRuleType.Text" })
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

		private static byte[] CommittedBlob(string rel)
		{
			var psi = new ProcessStartInfo("git", $"cat-file -p \"HEAD:{rel}\"")
			{
				WorkingDirectory = TestRepoRoot.Find(),
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			};
			using var process = Process.Start(psi)
				?? throw new InvalidOperationException("git n'a pas demarre");
			var stderr = process.StandardError.ReadToEndAsync();
			using var buffer = new MemoryStream();
			process.StandardOutput.BaseStream.CopyTo(buffer);
			process.WaitForExit(30_000);
			if (process.ExitCode != 0)
				throw new InvalidOperationException(
					$"git cat-file sur '{rel}' a echoue ({process.ExitCode}) : {stderr.Result}");
			return buffer.ToArray();
		}
	}
}
