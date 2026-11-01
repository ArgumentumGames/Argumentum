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
	/// volet <b>Member Accounts</b> (38 valeurs) — deuxieme PR de la decoupe par onglet.
	/// <para>Basee sur master ou #2085 (onglet SSL, <c>19b64a81</c>) est deja merge : le delta
	/// de cette PR sur le resx est exactement le volet Member (+1 cle, 37 valeurs).</para>
	/// <para>
	/// Le volet portait les deux cellules que la contre-revue web2 de #2085 avait nommees
	/// (<c>plForceLogOutAfterPasswordChanged.{Text,Help}</c> : <c>Forzar</c> ET <c>contrasenha</c>)
	/// et <b>deux defauts de SENS invisibles a toute grille de marqueurs</b> :
	/// <c>AsyncTimeout.ErrorMessage</c> disait « entre <b>09</b> e 10000 » pour un neutre
	/// « greater than or equal to <b>90</b> » (transposition de chiffres), et
	/// <c>AdminMembershipResetLinkValidity.ErrorMessage</c> avait PERDU « Administrator » —
	/// identique mot pour mot a la version non-admin. Les bornes numeriques du neutre
	/// sont donc epinglees cle par cle.
	/// </para>
	/// <para>
	/// ⚠️ Methodologie corrigee d'apres web2 : la grille est calibree sur l'es-ES du
	/// <b>fichier</b>, restreint aux cles du NEUTRE — 101 cles es n'ont pas d'equivalent
	/// neutre (calibrer dessus calibre sur du contenu mort), et calibrer sur l'onglet seul
	/// avait fabrique l'angle mort « Forzar » de #2085. L'assertion reste bornee au volet.
	/// Les formes propres au pt macaronique (<c>contrasenha</c>) sont AVEUGLES sur l'es-ES
	/// (qui dit <c>contraseña</c>) : elles passent par le mecanisme a temoin, temoin = la
	/// cellule pt ANCIENNE, qui doit CONTENIR la forme.
	/// </para>
	/// <para>
	/// Terminologie (fichiers pt-BR deja merges, jamais le corpus) : <c>senha</c> (users),
	/// <c>redefinição</c> (GlobalResources 27 cellules), <c>link</c> (20 cellules ;
	/// <c>enlace</c> = 41 cellules de contamination, ECARTE de la grille car mot portugais
	/// valide), <c>Ativar</c> (<c>Activar</c> = 59 cellules de contamination pt-PT/es),
	/// <c>deve ser um número inteiro</c> (roles, users, recyclebin), <c>expiração</c>,
	/// <c>Membros</c> (roles), <c>alterar a senha</c> (Password.ascx, users). Sans precedent,
	/// declares : <c>Forçar encerramento da sessão</c>, <c>força da senha</c>,
	/// <c>redirecionamento</c>, <c>Contas de membros</c>.
	/// </para>
	/// </summary>
	public class DnnSecurityMemberPtBrGuardTests
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

		private static readonly string[] TabKeys =
		{
			"AdminMembershipResetLinkValidity.ErrorMessage",
			"AsyncTimeout.ErrorMessage",
			"AutoAccountUnlockDuration.ErrorMessage",
			"MemberSettingsError.Text",
			"MemberSettingsRestoreWarning.Text",
			"MemberSettingsUpdateSuccess.Text",
			"MembershipDaysBeforePasswordReuse.ErrorMessage",
			"MembershipNumberPasswords.ErrorMessage",
			"MembershipResetLinkValidity.ErrorMessage",
			"PasswordConfig_PasswordExpiry.Help",
			"PasswordConfig_PasswordExpiry.Text",
			"PasswordConfig_PasswordExpiryReminder.Help",
			"PasswordConfig_PasswordExpiryReminder.Text",
			"PasswordExpiry.ErrorMessage",
			"PasswordExpiryReminder.ErrorMessage",
			"RedirectionMovedToSiteSettings.Text",
			"TabMemberAccounts.Text",
			"TabMemberSettings.Text",
			"plAdminResetLinkValidity.Help",
			"plAdminResetLinkValidity.Text",
			"plDefaultEndUserExtensionWhitelist.Help",
			"plDefaultEndUserExtensionWhitelist.Text",
			"plEnableBannedList.Help",
			"plEnableBannedList.Text",
			"plEnableIPChecking.Help",
			"plEnableIPChecking.Text",
			"plEnablePasswordHistory.Help",
			"plEnablePasswordHistory.Text",
			"plEnableStrengthMeter.Help",
			"plEnableStrengthMeter.Text",
			"plForceLogOutAfterPasswordChanged.Help",
			"plForceLogOutAfterPasswordChanged.Text",
			"plNumberPasswords.Help",
			"plNumberPasswords.Text",
			"plPasswordDays.Help",
			"plPasswordDays.Text",
			"plResetLinkValidity.Help",
			"plResetLinkValidity.Text",
		};

		// Calibree sur l'es-ES du FICHIER, restreint aux cles du NEUTRE (lecon web2 #2085).
		// Aucun marqueur n'est un mot portugais valide : « enlace » et « caduca » sont ECARTES
		// (portugais legitimes), « caducidad » et « cierre de sesión » gardes (es seulement).
		private static readonly string[] SpanishMarkers =
		{
			"forzar",
			"contraseña",
			"días",
			"términos",
			"veces",
			"después",
			"podrán",
			"guardarán",
			"recibirán",
			"activar",
			"cierre de sesión",
			"caducidad",
			"cual",
			"sitio web",
			"pantalla",
			"gestión",
			"vuelva",
			"cambios",
			"usuarios",
			"recordatorio",
		};

		[Fact]
		public void Member_Tab_Keys_All_Present_In_Pt()
		{
			var pt = Cells(PtResx);
			TabKeys.Where(k => !pt.ContainsKey(k)).Should().BeEmpty(
				"RedirectionMovedToSiteSettings.Text manquait : le message de deplacement du reglage s'affichait en neutre");
			TabKeys.Should().HaveCount(38);
		}

		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_File_Level_Es_Restricted_To_Neutral_Keys()
		{
			var es = Cells(EsResx);
			var neu = Cells(NeutralResx);
			// 101 cles es n'ont pas d'equivalent neutre : calibrer dessus calibrerait sur du
			// contenu que le produit ne sert plus. Le perimetre de calibration est l'intersection.
			var live = es.Where(kv => neu.ContainsKey(kv.Key)).Select(kv => kv.Value).ToArray();
			live.Should().HaveCountGreaterThan(300, "le perimetre de calibration doit etre le fichier vivant");
			var blind = SpanishMarkers
				.Where(m => !live.Any(v => v.ToLowerInvariant().Contains(m, StringComparison.Ordinal)))
				.ToArray();
			blind.Should().BeEmpty("un marqueur aveugle rend un 0 qui ne prouve rien (lecon #2085 : Forzar existait AU FICHIER, pas dans l'onglet)");
			SpanishMarkers.Should().HaveCount(20);
		}

		[Fact]
		public void Member_Tab_Carries_No_Spanish_Marker()
		{
			var pt = Cells(PtResx);
			var dirty = TabKeys
				.SelectMany(k => SpanishMarkers
					.Where(m => pt[k].ToLowerInvariant().Contains(m, StringComparison.Ordinal))
					.Select(m => $"{k} : « {m} »"))
				.ToArray();
			dirty.Should().BeEmpty();
		}

		[Fact]
		public void Historical_Macaronic_Forms_Are_Gone()
		{
			// Formes propres au pt macaronique, AVEUGLES sur l'es-ES (contrasena vs contrasenha).
			// Temoin = la cellule pt ANCIENNE, litteral qui doit CONTENIR la forme.
			var witnesses = new (string Form, string Witness)[]
			{
				("contrasenha", "Mostra o tempo (em minutos) durante o cual será válido o enlace para reiniciar a contrasenha."),
				("tienem", "Entre os días que tienem que pasar antes de poder reutilizar uma contrasenha."),
				("validez", "Tempo de validez do enlace de reinicio (minutos)"),
				("tem que ser", "O tempo de validez dos enlaces de reinicio tem que ser um número entre 0 e 10000."),
				("mudar", "Mostra cada quando tempo os usuarios tienem que mudar a contrasenha (0 mostra que a contrasenha não caduca nunca)."),
				("caduca", "Mostra cada quando tempo os usuarios tienem que mudar a contrasenha (0 mostra que a contrasenha não caduca nunca)."),
				("reinicio", "Tempo de validez do enlace de reinicio (minutos)"),
			};
			witnesses.Should().OnlyContain(w =>
				w.Witness.ToLowerInvariant().Contains(w.Form.ToLowerInvariant(), StringComparison.Ordinal),
				"un temoin qui ne contient pas la forme ne prouve rien");
			var pt = Cells(PtResx);
			var survivors = witnesses
				.SelectMany(w => TabKeys
					.Where(k => pt[k].ToLowerInvariant().Contains(w.Form.ToLowerInvariant(), StringComparison.Ordinal))
					.Select(k => $"{k} : « {w.Form} »"))
				.ToArray();
			survivors.Should().BeEmpty("contrasenha est la forme que users et roles ont eliminee : 182 occurrences dans le corpus contaminé");
		}

		// Bornes numeriques LUES DANS LE NEUTRE (greater than... X and less than Y).
		// Le pt disait « entre 09 e 10000 » pour un neutre « 90 » : transposition de chiffres.
		private static readonly (string Key, string Lo, string Hi)[] NumericBounds =
		{
			("AdminMembershipResetLinkValidity.ErrorMessage", "0", "10000"),
			("AsyncTimeout.ErrorMessage", "90", "10000"),
			("AutoAccountUnlockDuration.ErrorMessage", "0", "1000"),
			("MembershipDaysBeforePasswordReuse.ErrorMessage", "0", "10000"),
			("MembershipNumberPasswords.ErrorMessage", "0", "10000"),
			("MembershipResetLinkValidity.ErrorMessage", "0", "10000"),
			("PasswordExpiry.ErrorMessage", "0", "10000"),
			("PasswordExpiryReminder.ErrorMessage", "0", "10000"),
		};

		[Fact]
		public void Numeric_Bounds_Match_The_Neutral()
		{
			var pt = Cells(PtResx);
			NumericBounds.Should().HaveCount(8);
			foreach (var (key, lo, hi) in NumericBounds)
			{
				pt[key].Should().Contain(lo, $"{key} : borne basse du neutre");
				pt[key].Should().Contain(hi, $"{key} : borne haute du neutre");
			}
			pt["AsyncTimeout.ErrorMessage"].Should().Contain("igual a 90",
				"le neutre exige un minimum de 90 ; le pt disait « 09 » — un admin reglait sous la borne");
		}

		[Fact]
		public void Admin_And_NonAdmin_ErrorMessages_Are_Distinct()
		{
			var pt = Cells(PtResx);
			pt["AdminMembershipResetLinkValidity.ErrorMessage"].Should().Contain("administrador",
				"le neutre dit « Administrator reset link timeouts » ; le pt avait PERDU le qualificatif");
			pt["AdminMembershipResetLinkValidity.ErrorMessage"].Should().NotBe(pt["MembershipResetLinkValidity.ErrorMessage"],
				"les deux messages etaient identiques mot pour mot : l'admin ne savait pas quel reglage il lisait");
		}

		private static readonly Dictionary<string, string> Pins = new()
		{
			["AdminMembershipResetLinkValidity.ErrorMessage"] = "O tempo de validade do link de redefinição do administrador deve ser um número inteiro maior que 0 e menor que 10000.",
			["AsyncTimeout.ErrorMessage"] = "O tempo antes do tempo limite deve ser um número inteiro maior ou igual a 90 e menor que 10000.",
			["AutoAccountUnlockDuration.ErrorMessage"] = "A duração do desbloqueio automático da conta deve ser um número inteiro maior ou igual a 0 e menor que 1000.",
			["MemberSettingsError.Text"] = "Não foi possível atualizar a configuração de membros. Tente novamente mais tarde.",
			["MemberSettingsRestoreWarning.Text"] = "Tem certeza de que deseja cancelar as alterações?",
			["MemberSettingsUpdateSuccess.Text"] = "A configuração de membros foi atualizada.",
			["MembershipDaysBeforePasswordReuse.ErrorMessage"] = "O número de dias antes da reutilização da senha deve ser um número inteiro maior ou igual a 0 e menor que 10000.",
			["MembershipNumberPasswords.ErrorMessage"] = "O número de senhas a armazenar deve ser um número inteiro maior ou igual a 0 e menor que 10000.",
			["MembershipResetLinkValidity.ErrorMessage"] = "O tempo de validade do link de redefinição deve ser um número inteiro maior que 0 e menor que 10000.",
			["PasswordConfig_PasswordExpiry.Help"] = "Informe o número de dias antes que o usuário precise alterar a senha. Informe 0 (zero) se a senha nunca deve expirar.",
			["PasswordConfig_PasswordExpiry.Text"] = "Expiração da senha (em dias):",
			["PasswordConfig_PasswordExpiryReminder.Help"] = "Informe o número de dias de antecedência com que os usuários receberão o aviso de que sua senha está prestes a expirar.",
			["PasswordConfig_PasswordExpiryReminder.Text"] = "Aviso de expiração da senha (em dias):",
			["PasswordExpiry.ErrorMessage"] = "A expiração da senha deve ser um número inteiro maior ou igual a 0 e menor que 10000.",
			["PasswordExpiryReminder.ErrorMessage"] = "O aviso de expiração da senha deve ser um número inteiro maior ou igual a 0 e menor que 10000.",
			["RedirectionMovedToSiteSettings.Text"] = "O redirecionamento foi movido para as Configurações do site",
			["TabMemberAccounts.Text"] = "Contas de membros",
			["TabMemberSettings.Text"] = "GESTÃO DE MEMBROS",
			["plAdminResetLinkValidity.Help"] = "Tempo, em minutos, de validade dos links de redefinição de senha enviados pelo administrador do site.",
			["plAdminResetLinkValidity.Text"] = "Tempo de validade do link de redefinição do administrador (em minutos)",
			["plDefaultEndUserExtensionWhitelist.Help"] = "Lista padrão de extensões de arquivos que usuários finais podem enviar. Administradores podem substituir essa lista, mas nunca podem permitir mais tipos de arquivo do que os definidos na lista principal acima. Observe que esta lista será filtrada pela lista acima ao salvar.",
			["plDefaultEndUserExtensionWhitelist.Text"] = "Lista padrão de extensões permitidas para usuários finais",
			["plEnableBannedList.Help"] = "Marque esta opção para verificar as senhas em uma lista de itens proibidos.",
			["plEnableBannedList.Text"] = "Ativar lista de senhas proibidas",
			["plEnableIPChecking.Help"] = "Define se o endereço IP é verificado no momento do login",
			["plEnableIPChecking.Text"] = "Ativar verificação de endereço IP",
			["plEnablePasswordHistory.Help"] = "Define se uma lista de senhas usadas recentemente é mantida e verificada para impedir a reutilização.",
			["plEnablePasswordHistory.Text"] = "Ativar histórico de senhas",
			["plEnableStrengthMeter.Help"] = "Define se o medidor de força da senha é exibido na tela de registro",
			["plEnableStrengthMeter.Text"] = "Ativar verificação de força da senha",
			["plForceLogOutAfterPasswordChanged.Help"] = "Força o encerramento da sessão do usuário em outros navegadores após a senha ser alterada.",
			["plForceLogOutAfterPasswordChanged.Text"] = "Forçar encerramento da sessão após alterar a senha",
			["plNumberPasswords.Help"] = "Informe o número de senhas a armazenar para a verificação de reutilização",
			["plNumberPasswords.Text"] = "Número de senhas a armazenar",
			["plPasswordDays.Help"] = "Informe o tempo, em dias, que deve passar antes que uma senha possa ser reutilizada",
			["plPasswordDays.Text"] = "Número de dias antes de reutilizar uma senha",
			["plResetLinkValidity.Help"] = "Os links de redefinição de senha são válidos apenas durante esse tempo (em minutos).",
			["plResetLinkValidity.Text"] = "Tempo de validade do link de redefinição (em minutos)",
		};

		[Fact]
		public void Pins_Exact_Values()
		{
			var pt = Cells(PtResx);
			Pins.Should().HaveCount(38);
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
			foreach (var k in TabKeys)
			{
				var x = token.Matches(neu[k]).Select(m => m.Value).OrderBy(v => v, StringComparer.Ordinal);
				var y = token.Matches(pt[k]).Select(m => m.Value).OrderBy(v => v, StringComparer.Ordinal);
				y.Should().Equal(x, $"{k} doit conserver les jetons du neutre");
			}
		}

		[Fact]
		public void Values_Are_Text_Not_Nested_Xml()
		{
			// Mesure sur les NOEUDS : une balise brute devient XElement enfant et DISPARAIT
			// de Element("value").Value — un test sur la chaine la declarerait absente (M9, #2085).
			var nested = Load(PtResx).Root!.Elements("data")
				.Select(d => new { Key = d.Attribute("name")!.Value, Elem = d.Element("value") })
				.Where(x => x.Elem != null && x.Elem.Nodes().Any(n => n is XElement))
				.Select(x => x.Key)
				.ToArray();
			nested.Should().BeEmpty();
		}

		[Fact]
		public void No_Space_Before_Punctuation()
		{
			var bad = new Regex(@"[ \t]([,;:!?]|\.[ \t]*$)");
			var pt = Cells(PtResx);
			TabKeys.Where(k => bad.IsMatch(pt[k])).Should().BeEmpty();
		}

		[Fact]
		public void Terminology_Is_PtBr()
		{
			var pt = Cells(PtResx);
			pt["TabMemberAccounts.Text"].Should().Be("Contas de membros", "le neutre dit « Member Accounts » ; « Contas » seul était une troncature");
			pt["plEnablePasswordHistory.Text"].Should().Contain("Ativar").And.NotContain("Activar", "pt-BR = Ativar ; Activar = 59 cellules de contamination");
			pt["plResetLinkValidity.Text"].Should().Contain("link").And.NotContain("enlace", "link : 20 cellules propres ; enlace : 41 cellules contaminées");
			pt["PasswordConfig_PasswordExpiry.Text"].Should().Contain("Expiração").And.NotContain("caducidad");
			pt["MembershipNumberPasswords.ErrorMessage"].Should().Contain("deve ser um número inteiro", "precedent : roles, users, recyclebin");
			pt["plForceLogOutAfterPasswordChanged.Text"].Should().Contain("Forçar").And.NotContain("Forzar");
			foreach (var k in TabKeys)
			{
				pt[k].Should().NotContain("contrasenha", $"{k} : forme eliminee par users et roles");
				pt[k].Should().NotContain("días", $"{k} : es ; pt = dias");
			}
		}

		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell()
		{
			// Aucun homographe vrai dans ce volet (mesure) : liste vide, et c'est le fait a garder.
			var pt = Cells(PtResx);
			var es = Cells(EsResx);
			TabKeys.Where(k => pt[k].Trim().Length > 0 && pt[k].Trim() == es.GetValueOrDefault(k, "").Trim())
				.Should().BeEmpty("une cellule identique a l'espagnol est une cellule non traduite");
		}

		[Fact]
		public void Legacy_Keys_Survive_The_Rewrite()
		{
			var pt = Cells(PtResx);
			// plancher (pas compte exact) : le compte EXACT vit dans la garde du volet COURANT —
			// sinon chaque onglet suivant de la decoupe casse la garde du precedent (regle #2094).
			// Registration ajoute ses 4 cles -> 359 dans la garde Registration.
			pt.Should().HaveCountGreaterThanOrEqualTo(355,
				"anti-effondrement : 354 (base #2085) + RedirectionMovedToSiteSettings.Text. Les onglets suivants de la decoupe ajoutent les leurs (Registration -> 359) : le compte EXACT vit dans la garde de chaque onglet, pas ici");
			pt.Should().ContainKey("Não.Text").And.ContainKey("Redirect_AfterLogin.Text",
				"les cles heritees ne doivent pas etre emportees par une reecriture");
		}

		[Fact]
		public void Committed_Blob_Is_Utf8_Bom_With_Lf_Only()
		{
			// Invariant de TRANSPORT sur le blob committe : un runner Windows core.autocrlf=true
			// convertit LF -> CRLF AU CHECKOUT — un fichier sain apparait en CRLF dans l'arbre.
			var bytes = CommittedBlob(PtResx);
			bytes.Length.Should().BeGreaterThan(1000, "le blob doit etre le fichier entier, pas un fragment");
			bytes.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "le BOM est celui du fichier d'origine");
			bytes.Count(b => b == 0x0D).Should().Be(0, $"le blob committé de '{PtResx}' est en LF seul");
		}

		private static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);
	}
}
