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
	/// #458 — onglet <b>More (Other Settings)</b> de <c>Dnn.Security/App_LocalResources/security.pt-BR.resx</c>.
	///
	/// <para><b>Perimetre derive du composant.</b> Les cles viennent du segment <c>OtherSettingsPanelBody</c>
	/// du bundle <c>security-settings-bundle.js</c> (offsets 107763..124401) : <b>29 ids</b>, dont
	/// <c>SwitchOn</c>/<c>SwitchOff</c> hors neutre (ressources globales, es-ES seule) =&gt; <b>27 cles
	/// livrees ici</b>. Resolution de cle par la regle corrigee par le volet Login : essayer <c>K</c> puis
	/// <c>K.Text</c>, garder celle qui existe au neutre.</para>
	///
	/// <para><b>Etat libre : 19 valeurs reecrites, 0 cle creee</b> (362 -&gt; 362 cles). Troisieme onglet
	/// consécutif <b>traduit depuis l'espagnol</b> (apres IP filters et Login) : « Vuelva a intentarlo »,
	/// « errores », « después de », « tamaño », « se ejecuta », et deux hybrides hors es-ES
	/// (<c>foiya</c>, <c>tienem</c>). Neuf cellules restent conformes, dont deux messages d'erreur
	/// numeriques deja justes et l'aide <c>plDefaultEndUserExtensionWhitelist.Help</c>, correcte.</para>
	///
	/// <para><b>L'es-ES lui-meme porte une coquille</b> sur <c>AsyncTimeout.ErrorMessage</c> (« entre 09 y
	/// 10000 ») que le pt-BR ancien n'a PAS heritee (il dit 90, comme le neutre) — le pt n'etait donc pas
	/// une copie litterale, mais une traduction dont la SOURCE etait l'es. Constate, consigne, la cellule
	/// reste inchangee.</para>
	///
	/// <para><b>Marqueurs</b> : 12, chacun verifie NON AVEUGLE (present dans l'es de l'onglet) et ABSENT du
	/// pt livre. Les hybrides absents de l'es-ES (<c>foiya</c>, <c>tienem</c>, <c>em pantalla</c>,
	/// <c>Tenga em conta</c>) sont des formes historiques, jamais des marqueurs.</para>
	/// </summary>
	public class DnnSecurityOtherSettingsPtBrGuardTests
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

		/// <summary>Les 27 cles de l'onglet — derivees du composant, pas d'une liste de prefixes.</summary>
		private static readonly string[] TabKeys =
		{
			"OtherSettingsUpdateSuccess.Text", "OtherSettingsError.Text", "OtherSettingsRestoreWarning.Text",
			"Yes.Text", "No.Text", "plShowCriticalErrors.Help",
			"plShowCriticalErrors.Text", "GlobalSetting.Text", "plDebugMode.Help",
			"plDebugMode.Text", "plRememberMe.Help", "plRememberMe.Text",
			"plAutoAccountUnlock.Help", "plAutoAccountUnlock.Text", "AutoAccountUnlockDuration.ErrorMessage",
			"plAsyncTimeout.Help", "plAsyncTimeout.Text", "AsyncTimeout.ErrorMessage",
			"plMaxUploadSize.Help", "plMaxUploadSize.Text", "maxUploadSize.Error",
			"plFileExtensions.Help", "plFileExtensions.Text", "plDefaultEndUserExtensionWhitelist.Help",
			"plDefaultEndUserExtensionWhitelist.Text", "Cancel.Text", "Save.Text",
		};

		/// <summary>Marqueurs espagnols de l'onglet : chacun est present dans l'es-ES de l'onglet
		/// (verifie par <c>Marker_Grid_Is_Non_Vacuous</c>) et absent du pt livre.</summary>
		private static readonly string[] SpanishMarkers =
		{
			"cambios afectar\u00e1n", "cancelar los cambios", "cargar", "casilla de verificaci\u00f3n",
			"despu\u00e9s de", "en pantalla", "errores", "est\u00e1 seguro que desea",
			"se ejecuta", "suban", "tama\u00f1o", "vuelva a intentarlo",
		};

		/// <summary>Formes historiques de l'ONGLET, chacune temoignee par le litteral de l'ancienne
		/// cellule pt (3e element). Le generateur refuse une forme sans temoin et une forme survivante.</summary>
		private static readonly (string Form, string Why, string Witness)[] HistoricalForms =
		{
			("foi actualizado", "pt-PT (pt-BR ecrit \u00ab atualizado \u00bb)", "A configura\u00e7\u00e3o foi actualizado."),
			("foi podido", "castillan (\u00ab nao foi possivel \u00bb)", "N\u00e3o foi podido atualizar a configura\u00e7\u00e3o. Vuelva a intentarlo."),
			("Vuelva a intentarlo", "castillan", "N\u00e3o foi podido atualizar a configura\u00e7\u00e3o. Vuelva a intentarlo."),
			("Est\u00e1 seguro que quer", "castillan", "Est\u00e1 seguro que quer cancelar os cambios?"),
			("os cambios", "castillan", "Est\u00e1 seguro que quer cancelar os cambios?"),
			("errores", "castillan (pt : \u00ab erros \u00bb)", "Mostrar errores cr\u00edticos em pantalla"),
			("em pantalla", "hybride es/pt (pt-BR : \u00ab na tela \u00bb)", "Mostrar errores cr\u00edticos em pantalla"),
			("tienem que", "castillan + faute (\u00ab tienen \u00bb)", "Mostra se os errores que se env\u00eden como par\u00e1metros nas URLs se tienem que mostrar em l\u00ednea em a p\u00e1gina."),
			("em l\u00ednea", "castillan", "Mostra se os errores que se env\u00eden como par\u00e1metros nas URLs se tienem que mostrar em l\u00ednea em a p\u00e1gina."),
			("par\u00e1metro global", "castillan", "Este um par\u00e1metro global. Os cambios afectar\u00e1n a todos os portais da instala\u00e7\u00e3o."),
			("afectar\u00e1n", "castillan (pt : \u00ab afetar\u00e3o \u00bb)", "Este um par\u00e1metro global. Os cambios afectar\u00e1n a todos os portais da instala\u00e7\u00e3o."),
			("se ejecuta", "castillan", "Si est\u00e1 activo, a instala\u00e7\u00e3o se ejecuta em modo de depura\u00e7\u00e3o. De esta manera algunas partes da aplica\u00e7\u00e3o registrar\u00e1n os errores de forma m\u00e1s extensa. Tenga em conta que isto pode comportar uma degrada\u00e7\u00e3o do rendimiento do sistema."),
			("De esta manera", "castillan", "Si est\u00e1 activo, a instala\u00e7\u00e3o se ejecuta em modo de depura\u00e7\u00e3o. De esta manera algunas partes da aplica\u00e7\u00e3o registrar\u00e1n os errores de forma m\u00e1s extensa. Tenga em conta que isto pode comportar uma degrada\u00e7\u00e3o do rendimiento do sistema."),
			("Tenga em conta", "hybride es/pt", "Si est\u00e1 activo, a instala\u00e7\u00e3o se ejecuta em modo de depura\u00e7\u00e3o. De esta manera algunas partes da aplica\u00e7\u00e3o registrar\u00e1n os errores de forma m\u00e1s extensa. Tenga em conta que isto pode comportar uma degrada\u00e7\u00e3o do rendimiento do sistema."),
			("podr\u00e1n", "castillan (pt : \u00ab poder\u00e3o \u00bb)", "Configura a casilla de verifica\u00e7\u00e3o Recordar nos controles de Acesso. Se activa esta op\u00e7\u00e3o os usuarios podr\u00e1n criar cookies persistentes para recordar a identifica\u00e7\u00e3o"),
			("controles de Acesso", "calque (\u00ab controle de login \u00bb)", "Habilitar recordar em controles de Acesso"),
			("foiya", "castillan + faute (pour \u00ab ya \u00bb)", "Despu\u00e9s de que uma conta foiya bloqueado debido a intentos de inicio de sesi\u00f3n incorretos, \u00e9sta pode desbloquearse despu\u00e9s de que haya pasado um cierto periodo de tempo. Escriba o n\u00famero de minutos que h\u00e1 que esperar para que a conta se desbloquee autom\u00e1ticamente. Escriba \"0\" para desactivar a caracter\u00edstica de autodesbloqueo"),
			("desbloquearse", "castillan (pt : \u00ab desbloquear-se \u00bb)", "Despu\u00e9s de que uma conta foiya bloqueado debido a intentos de inicio de sesi\u00f3n incorretos, \u00e9sta pode desbloquearse despu\u00e9s de que haya pasado um cierto periodo de tempo. Escriba o n\u00famero de minutos que h\u00e1 que esperar para que a conta se desbloquee autom\u00e1ticamente. Escriba \"0\" para desactivar a caracter\u00edstica de autodesbloqueo"),
			("peti\u00e7\u00e3o as\u00edncrona", "calque es", "Tempo de validez de peti\u00e7\u00e3o as\u00edncrona"),
			("se mantendr\u00e1", "castillan", "Entre o tempo m\u00e1ximo, em segundos, que se mantendr\u00e1 activa uma llamada as\u00edncrona antes de cerrar a peti\u00e7\u00e3o. O valor tem que estar entre 90-9999 segundos."),
			("Escriba", "castillan (pt : \u00ab Escreva \u00bb)", "Escriba as extens\u00f5es de Arquivos permitidas (separadas por comas) para os Arquivos que suban os administradores do portal."),
			("que suban", "castillan", "Escriba as extens\u00f5es de Arquivos permitidas (separadas por comas) para os Arquivos que suban os administradores do portal."),
			("tama\u00f1o m\u00e1ximo dos Arquivos", "castillan", "Mostra o tama\u00f1o m\u00e1ximo dos Arquivos que se podem cargar n\u00e3o portal. O valor m\u00ednimo de 12 MB."),
			("Tama\u00f1o m\u00e1ximo de carga", "cellule identique a l'es-ES avant reecriture", "Tama\u00f1o m\u00e1ximo de carga"),
		};

		/// <summary>Les 27 cellules livrees : empreinte SHA-256 de la valeur PLEINE (derivee du fichier).</summary>
		private static readonly (string Key, string Sha256)[] Pins =
		{
			("OtherSettingsUpdateSuccess.Text", "4364636e73b697def53aed8ddc34fb63fa952e785d7b20742d45e0f55ae18f70"),
			("OtherSettingsError.Text", "79a4e0913e0f1ede0657201cc3265fd2d4fbe379236774837a9ee7921e4c5b5d"),
			("OtherSettingsRestoreWarning.Text", "88a77a9679ffcd2a0d00f68aacf593df9cbb274c25c3d3a37517529a0f2ca15b"),
			("Yes.Text", "9313455e45aa22430a92fddbb70f21312d3c944ab590dfb1da845c1a96b664fe"),
			("No.Text", "9bc947e58b62b2fef2954dc2813f3bfe458455fea1bb40b2d7a33e010dfe9ab6"),
			("plShowCriticalErrors.Help", "77fc14373f71698d053a0201ed8201b9985f49504c76c96f34b6e749188867b9"),
			("plShowCriticalErrors.Text", "643a69d76af89e3ec13e4315b2b5fbc15ab66e97e57c58c7838478b7d1cf80e6"),
			("GlobalSetting.Text", "732ecc2891db682ddffcb8d674c1e8e833f64f03cd9c9d1a044bb49067207224"),
			("plDebugMode.Help", "472d18e9430600c372d4af5e4c367e471738b23d03963ead30df893212e0e313"),
			("plDebugMode.Text", "255882db8631533010151bc84dce412c51709a028996c30a9f24667b90e73319"),
			("plRememberMe.Help", "af3b1eeac4298d11f77e61dbd1276561d417fa2cca15f3693eb358df7030f913"),
			("plRememberMe.Text", "d142823bcf2c67a18fa7912dfca73087a44fba5ff1aaef91e5e4ae4d83134c8f"),
			("plAutoAccountUnlock.Help", "76f73ae423f1b5e375af0035ecd9649a8536cc9d6ced999f8b643b6bbd8f31bb"),
			("plAutoAccountUnlock.Text", "bb45c379b200a5735d5e8b578d4a2c5e8a3333b8b745579574e3409e9e60de7b"),
			("AutoAccountUnlockDuration.ErrorMessage", "f1669b3f5dec3b1d9d8c31b168ddd1672d449d5e2cffa62b6fbd62a5fa905b66"),
			("plAsyncTimeout.Help", "e4be163c98b8915492411de4226a923c4c4da836ef29bde98e725f597c41a07b"),
			("plAsyncTimeout.Text", "dd1a3e4c37e3632827797ae49b937665d911169c3772eadc69569cabef16ecf4"),
			("AsyncTimeout.ErrorMessage", "05a7df177fc48763f1991fe5b38a952d61d519a32773b8b69102d6d74bb011a8"),
			("plMaxUploadSize.Help", "f48d91040048f17970d7fc6c8946b7b9fee6d410bb0c453dbdc5fc4dacf86e85"),
			("plMaxUploadSize.Text", "6470786e08333f99db4464217310e45b2215a6a20464787ee28d38856ac41ff7"),
			("maxUploadSize.Error", "ff9a81a14631f809a60b0251e4614e39ea97764b3c353ed2c60d4772eb7a11e0"),
			("plFileExtensions.Help", "c18324392d0e607a242076a3abcb90e2fc56c75eb22a5c85446615a9d1c44b87"),
			("plFileExtensions.Text", "04974e56ffa5a28ece733985bf4a23a5cdeca78366d7d2aeb01354f1664590fe"),
			("plDefaultEndUserExtensionWhitelist.Help", "3bf5432d199ab1e17163a8bd8aa068dfe88220082f9d8598657a6dd4b1928c87"),
			("plDefaultEndUserExtensionWhitelist.Text", "3eee4b54a323260643a14ef6b3f57e454843169f835f26989dcb3e69433bfea6"),
			("Cancel.Text", "bb9dbb406dcbd5248babc609bb3d021d384398f83dbdb8d53c9b0e32c2aad915"),
			("Save.Text", "aef7cd5b2081351dcd4b3767cc84f77797ee1a945e758e8013961c283304b947"),
		};

		/// <summary>Homographe LEGITIME : la cellule pt egale l'es parce que le mot est identique
		/// dans les deux langues. Toute entree doit rester VIVANTE (verifie ci-dessous).</summary>
		private static readonly (string Key, string Word, string NeutralWitness)[] DeclaredHomographs =
		{
			("Cancel.Text", "Cancelar", "Cancel"),
		};

		[Fact]
		public void OtherSettings_Tab_Keys_All_Present()
		{
			var pt = Load(PtResx);
			TabKeys.Where(k => !pt.ContainsKey(k)).Should().BeEmpty(
				"#458 : les 27 cles de l'onglet More sont livrees (aucune creation dans ce volet)");
			TabKeys.Should().HaveCount(27);
			TabKeys.Distinct().Should().HaveCount(27);
		}

		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_The_Tab_Es()
		{
			var es = Load(EsResx);
			var blind = SpanishMarkers
				.Where(m => !TabKeys.Any(k => es[k].ToLowerInvariant().Contains(m)))
				.ToList();
			blind.Should().BeEmpty("un marqueur aveugle sur l'es calibrerait l'ecran sur du vide");
			SpanishMarkers.Should().HaveCount(12).And.OnlyHaveUniqueItems();
			SpanishMarkers.Should().NotContain("utilizar",
				"« utilizar » est un verbe portugais valide : jamais un marqueur");
		}

		[Fact]
		public void OtherSettings_Tab_Carries_No_Spanish_Marker()
		{
			var pt = Load(PtResx);
			var offenders = TabKeys
				.SelectMany(k => SpanishMarkers
					.Where(m => pt[k].ToLowerInvariant().Contains(m))
					.Select(m => $"{k} : marqueur «{m}»"))
				.ToList();
			offenders.Should().BeEmpty("#458 : 19 cellules reecrites, 0 marqueur restant (mesure)");
		}

		[Fact]
		public void Historical_Forms_Are_Gone_And_Each_Has_A_Witness()
		{
			var pt = Load(PtResx);
			HistoricalForms.Should().HaveCount(24, "liste figee : chaque forme est temoignee");
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
				actual.Should().Equal(expected, $"les nombres de {key} sont ceux du neutre (l'es dit « 09 », le pt ne l'a jamais suivi)");
				var et = System.Text.RegularExpressions.Regex.Matches(neu[key], tokens).Select(m => m.Value).OrderBy(x => x);
				var at = System.Text.RegularExpressions.Regex.Matches(pt[key], tokens).Select(m => m.Value).OrderBy(x => x);
				at.Should().Equal(et, $"les jetons de {key} sont ceux du neutre");
			}
		}

		[Fact]
		public void Pins_Exact_Values()
		{
			var pt = Load(PtResx);
			Pins.Should().HaveCount(27);
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
			pt["plMaxUploadSize.Text"].Should().Be("Tamanho máximo de upload (MB)",
				"l'unite « (MB) » vient du neutre « Max Upload Size (MB) » — l'ancienne cellule etait l'es verbatim");
			pt["maxUploadSize.Error"].Should().Contain("entre 12 e {0}", "le jeton {0} du neutre est preserve");
			pt["plDebugMode.Text"].Should().Be("Modo de depuração");
			pt["plRememberMe.Text"].Should().Contain("Lembrar-me").And.Contain("login",
				"« controle de login » : la meme terminologie que le volet Login (#2107)");
			pt["plAutoAccountUnlock.Text"].Should().Contain("automaticamente").And.NotContain("después");
			pt["OtherSettingsError.Text"].Should().Contain("Tente novamente mais tarde",
				"precedent MemberSettingsError.Text / IpFilterUpdateError.Text / BasicLoginSettingsError");
			pt["OtherSettingsRestoreWarning.Text"].Should().StartWith("Tem certeza de que deseja",
				"precedent #2102 / #2104 / #2107");
			TabKeys.Where(k => pt[k].Contains("actualizar", StringComparison.Ordinal)
			                 || pt[k].Contains("actualizado", StringComparison.Ordinal)).Should().BeEmpty(
				"« actualizado » est la forme pt-PT ; le corpus pt-BR ecrit « atualizado »");
			TabKeys.Where(k => pt[k].Contains("erros críticos em ", StringComparison.Ordinal)).Should().BeEmpty(
				"pt-BR : « erros críticos na tela » (l'es dit « en pantalla »)");
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
			offenders.Should().BeEmpty("deux cellules egales a l'es avant reecriture ; une le reste legitimement");
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
				"362 cles depuis #2107 (Login). Les volets suivants ajoutent les leurs : le compte EXACT vit dans leur garde");
			foreach (var key in new[] { "Security_RequireValidProfileAtLogin.Text", "BasicLoginSettingsUpdateSuccess", "IpFilterUpdateError.Text" })
				pt.Should().ContainKey(key, "les cles des volets precedents survivent a la reecriture");
		}

		[Fact]
		public void Committed_Blob_Is_Utf8_Bom_With_Lf_Only()
		{
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
