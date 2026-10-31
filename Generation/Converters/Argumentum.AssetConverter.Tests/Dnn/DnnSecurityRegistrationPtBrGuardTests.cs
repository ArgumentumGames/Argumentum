using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using CsvHelper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 — onglet <b>Registration</b> de <c>Dnn.Security/App_LocalResources/security.pt-BR.resx</c>.
	///
	/// <para><b>Perimetre, mesure et non recopie.</b> Les 63 cles de l'onglet sont derivees du COMPOSANT
	/// <c>RegistrationSettingsPanelBody</c> du bundle <c>security-settings-bundle.js</c> (ids de ressources),
	/// restreintes a celles presentes dans le neutre <c>security.resx</c> : 65 ids, moins <c>SwitchOn/Off</c>
	/// qui vivent dans les ressources GLOBALES (absent du neutre de ce fichier), moins rien d'autre.
	/// Methode calibree sur l'onglet Member deja livre (38 ids = les 38 valeurs de la decoupe ai-01).</para>
	///
	/// <para><b>Etat livre : 57 valeurs reecrites + 4 cles creees</b> (355 -> 359 cles).
	/// Mesure avant/apres : marqueurs es <b>{0} -> 0</b> sur {1} cellules ; formes pt-PT/macaroniques
	/// <b>{2} -> 0</b>. Une seule cellule reste identique a l'es-ES : <c>Cancel.Text</c> = « Cancelar »,
	/// homographe legitime (declare et vivant ci-dessous, jamais une tolerance muette).</para>
	///
	/// <para><b>Errata web2 appliques</b> : la grille est calibree sur l'es-ES du FICHIER restreint aux cles
	/// du neutre (105 cles es mortes mesurees : 4 cles du neutre sont absentes de l'es-ES), tandis que
	/// l'assertion reste bornee a l'onglet. <c>número</c> est ECARTE de la grille : mot portugais valide.
	/// <c>enlace</c>/<c>caduca</c> restent ecartes pour la meme raison.</para>
	/// </summary>
	public class DnnSecurityRegistrationPtBrGuardTests
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


		/// <summary>Les 63 cles de l'onglet — derivees du composant, pas d'une liste de prefixes.</summary>
		private static readonly string[] TabKeys =
		{
			"Cancel.Text", "MaxInvalidPasswordAttemptsTitle.Help", "MaxInvalidPasswordAttemptsTitle.Text",
			"MinNonAlphanumericCharactersTitle.Help", "MinNonAlphanumericCharactersTitle.Text", "MinPasswordLengthTitle.Help",
			"MinPasswordLengthTitle.Text", "No.Text", "PasswordAttemptWindowTitle.Help",
			"PasswordAttemptWindowTitle.Text", "PasswordFormat.Help", "PasswordFormat.Text",
			"PasswordResetEnabledTitle.Help", "PasswordResetEnabledTitle.Text", "PasswordRetrievalEnabled.Help",
			"PasswordRetrievalEnabled.Text", "PasswordStrengthRegularExpressionTitle.Help", "PasswordStrengthRegularExpressionTitle.Text",
			"RedirectionMovedToSiteSettings.Text", "RegistrationSettingsError.Text", "RegistrationSettingsRestoreWarning.Text",
			"RegistrationSettingsUpdateSuccess.Text", "Registration_ExcludeTerms.Help", "Registration_ExcludeTerms.Text",
			"Registration_RandomPassword.Help", "Registration_RandomPassword.Text", "Registration_RequireConfirmPassword.Help",
			"Registration_RequireConfirmPassword.Text", "Registration_RequireUniqueDisplayName.Help", "Registration_RequireUniqueDisplayName.Text",
			"Registration_UseAuthProviders.Help", "Registration_UseAuthProviders.Text", "Registration_UseEmailAsUserName.Help",
			"Registration_UseEmailAsUserName.Text", "Registration_UseProfanityFilter.Help", "Registration_UseProfanityFilter.Text",
			"RequiresQuestionAndAnswerTitle.Help", "RequiresQuestionAndAnswerTitle.Text", "RequiresUniqueEmail.Help",
			"RequiresUniqueEmail.Text", "Save.Text", "Security_CaptchaRegister.Help",
			"Security_CaptchaRegister.Text", "Security_DisplayNameFormat.Help", "Security_DisplayNameFormat.Text",
			"Security_EmailValidation.Help", "Security_EmailValidation.Text", "Security_RequireValidProfile.Help",
			"Security_RequireValidProfile.Text", "Security_UserNameMinLength.ErrorMessage", "Security_UserNameMinLength.Help",
			"Security_UserNameMinLength.Text", "Security_UserNameValidation.Help", "Security_UserNameValidation.Text",
			"Yes.Text", "plEnableRegisterNotification.Help", "plEnableRegisterNotification.Text",
			"plUserRegistration.Help", "plUserRegistration.Text", "registrationFieldsLabel.Help",
			"registrationFieldsLabel.Text", "registrationFormTypeLabel.Help", "registrationFormTypeLabel.Text",
		};

		private static readonly string[] SpanishMarkers =
		{
			"forzar", "contrase\u00f1a", "d\u00edas", "t\u00e9rminos", "despu\u00e9s",
			"podr\u00e1n", "guardar\u00e1n", "recibir\u00e1n", "activar", "cierre de sesi\u00f3n",
			"caducidad", "sitio web", "pantalla", "gesti\u00f3n", "vuelva",
			"cambios", "usuarios", "recordatorio", "secci\u00f3n", "mensaje",
			"configuraci\u00f3n",
		};

		/// <summary>Formes pt-PT/macaroniques de l'ONGLET, chacune temoignee par un litteral de l'ancienne
		/// cellule pt (3e element) — une forme sans temoin est une forme devinee, refusee a la generation.</summary>
		private static readonly (string Form, string Why, string Witness)[] HistoricalForms =
		{
			("contrasenha", "forme es/ pt-PT (aveugle sur l'es-ES)", "Expresi\u00f3n regular que se utilizar\u00e1 para evaluar a complejidade da contrasenha. Somente se pode modificar em o arquivo web.config cambiando o valor do nodo passwordStrengthRegularExpression de AspNetSqlMembershipProvider. Aten\u00e7\u00e3o: esta valida\u00e7\u00e3o \u00e9 diferente da visualiza\u00e7\u00e3o de seguridade introducida na vers\u00e3o 7.1.0, a cual somente muestra sugerencias de seguridade, mientras que esta expressi\u00f3n \u00e9 um requisito para as novas contrasenhas (si est\u00e1 definido)."),
			("puedo", "forme es/ pt-PT", "Mostra o n\u00famero m\u00ednimo de caracteres especiales que deve incluir a contrasenha. Somente se puedo configurar em o arquivo web.config."),
			("caja", "forme es/ pt-PT", "Marque esta op\u00e7\u00e3o para utilizar o EMail como nome do usu\u00e1rio. Si marca esta op\u00e7\u00e3o n\u00e3o se mostrar\u00e1 a caja de nome do usu\u00e1rio n\u00e3o registro."),
			("T\u00e9rminos", "forme es/ pt-PT", "T\u00e9rminos excluidos"),
			("intentos", "forme es/ pt-PT", "O per\u00edodo de tempo (em minutos) para controlar o m\u00e1ximo de intentos de identifica\u00e7\u00e3o"),
			("Vuelva", "forme es/ pt-PT", "N\u00e3o foi podido atualizar a configura\u00e7\u00e3o de registro. Vuelva a intentarlo."),
			("usuarios", "forme es/ pt-PT", "O tipo de registro de usuarios a utilizar n\u00e3o portal. O registro Privado require que os administradores aprueben os novos registros para activarlos. O registro P\u00fablico permite Acesso inmediato a os usuarios. O registro Verificado requiere a valida\u00e7\u00e3o do email do usuario utilizado n\u00e3o registro."),
			("escriban", "forme es/ pt-PT", "Marque esta op\u00e7\u00e3o para asignar contrasenhas aleatorias a os usuarios n\u00e3o registro em vez de permitir que escriban a suya."),
			("lenguaje", "forme es/ pt-PT", "Si marca esta op\u00e7\u00e3o de utilizar\u00e1 o filtro de lenguaje inapropiado para o nome do usu\u00e1rio e nome para mostrar durante o registro"),
			("inmediato", "forme es/ pt-PT", "O tipo de registro de usuarios a utilizar n\u00e3o portal. O registro Privado require que os administradores aprueben os novos registros para activarlos. O registro P\u00fablico permite Acesso inmediato a os usuarios. O registro Verificado requiere a valida\u00e7\u00e3o do email do usuario utilizado n\u00e3o registro."),
			("require", "forme es/ pt-PT", "O tipo de registro de usuarios a utilizar n\u00e3o portal. O registro Privado require que os administradores aprueben os novos registros para activarlos. O registro P\u00fablico permite Acesso inmediato a os usuarios. O registro Verificado requiere a valida\u00e7\u00e3o do email do usuario utilizado n\u00e3o registro."),
			("aprueben", "forme es/ pt-PT", "O tipo de registro de usuarios a utilizar n\u00e3o portal. O registro Privado require que os administradores aprueben os novos registros para activarlos. O registro P\u00fablico permite Acesso inmediato a os usuarios. O registro Verificado requiere a valida\u00e7\u00e3o do email do usuario utilizado n\u00e3o registro."),
			("sugerir\u00e1", "forme es/ pt-PT", "Pode obligar a os usuarios a que utilicem um nome para mostrar \u00fanico. Si um usuario utiliza um nome que j\u00e1 se utiliza se le sugerir\u00e1 um novo nome."),
			("mostrar\u00e1", "forme es/ pt-PT", "Pode introducir uma expresi\u00f3n regular para validar os nomes de usuarios proporcionados. Si cambia esta op\u00e7\u00e3o tem que proporcionar a mensagem de error que se mostrar\u00e1 a os usuarios quando selecionem um nome do usu\u00e1rio inv\u00e1lido em Configura\u00e7\u00e3o - Configura\u00e7\u00e3o do portal - Idiomas."),
			("activado", "forme es/ pt-PT", "Recupera\u00e7\u00e3o de contrasenha activado"),
			("soportan", "forme es/ pt-PT", "Selecione esta op\u00e7\u00e3o para utilizar provedores de autentica\u00e7\u00e3o durante o registro. Aten\u00e7\u00e3o: n\u00e3o todos os provedores soportan esta op\u00e7\u00e3o."),
			("pregunta", "forme es/ pt-PT", "Mostra si \u00e9 necess\u00e1rio que o usu\u00e1rio introduzca uma pregunta e respuesta de seguridade para poder recuperar a contrasenha. Os Administradores n\u00e3o podr\u00e1n adicionar novos usuario o editar os existentes si esta op\u00e7\u00e3o est\u00e1 activada."),
			("respuesta", "forme es/ pt-PT", "Mostra si \u00e9 necess\u00e1rio que o usu\u00e1rio introduzca uma pregunta e respuesta de seguridade para poder recuperar a contrasenha. Os Administradores n\u00e3o podr\u00e1n adicionar novos usuario o editar os existentes si esta op\u00e7\u00e3o est\u00e1 activada."),
			("seguridade", "forme es/ pt-PT (aveugle sur l'es-ES)", "Expresi\u00f3n regular que se utilizar\u00e1 para evaluar a complejidade da contrasenha. Somente se pode modificar em o arquivo web.config cambiando o valor do nodo passwordStrengthRegularExpression de AspNetSqlMembershipProvider. Aten\u00e7\u00e3o: esta valida\u00e7\u00e3o \u00e9 diferente da visualiza\u00e7\u00e3o de seguridade introducida na vers\u00e3o 7.1.0, a cual somente muestra sugerencias de seguridade, mientras que esta expressi\u00f3n \u00e9 um requisito para as novas contrasenhas (si est\u00e1 definido)."),
			("cambiando", "forme es/ pt-PT", "Expresi\u00f3n regular que se utilizar\u00e1 para evaluar a complejidade da contrasenha. Somente se pode modificar em o arquivo web.config cambiando o valor do nodo passwordStrengthRegularExpression de AspNetSqlMembershipProvider. Aten\u00e7\u00e3o: esta valida\u00e7\u00e3o \u00e9 diferente da visualiza\u00e7\u00e3o de seguridade introducida na vers\u00e3o 7.1.0, a cual somente muestra sugerencias de seguridade, mientras que esta expressi\u00f3n \u00e9 um requisito para as novas contrasenhas (si est\u00e1 definido)."),
			("nodo", "forme es/ pt-PT", "Expresi\u00f3n regular que se utilizar\u00e1 para evaluar a complejidade da contrasenha. Somente se pode modificar em o arquivo web.config cambiando o valor do nodo passwordStrengthRegularExpression de AspNetSqlMembershipProvider. Aten\u00e7\u00e3o: esta valida\u00e7\u00e3o \u00e9 diferente da visualiza\u00e7\u00e3o de seguridade introducida na vers\u00e3o 7.1.0, a cual somente muestra sugerencias de seguridade, mientras que esta expressi\u00f3n \u00e9 um requisito para as novas contrasenhas (si est\u00e1 definido)."),
			("expresi\u00f3n", "forme es/ pt-PT", "Pode introducir uma expresi\u00f3n regular para validar os nomes de usuarios proporcionados. Si cambia esta op\u00e7\u00e3o tem que proporcionar a mensagem de error que se mostrar\u00e1 a os usuarios quando selecionem um nome do usu\u00e1rio inv\u00e1lido em Configura\u00e7\u00e3o - Configura\u00e7\u00e3o do portal - Idiomas."),
			("Expresi\u00f3n", "forme es/ pt-PT", "Expresi\u00f3n regular que se utilizar\u00e1 para evaluar a complejidade da contrasenha. Somente se pode modificar em o arquivo web.config cambiando o valor do nodo passwordStrengthRegularExpression de AspNetSqlMembershipProvider. Aten\u00e7\u00e3o: esta valida\u00e7\u00e3o \u00e9 diferente da visualiza\u00e7\u00e3o de seguridade introducida na vers\u00e3o 7.1.0, a cual somente muestra sugerencias de seguridade, mientras que esta expressi\u00f3n \u00e9 um requisito para as novas contrasenhas (si est\u00e1 definido)."),
			("tiene", "forme es/ pt-PT", "Ative esta op\u00e7\u00e3o se os usuarios tienem que completar todos os campos obrigat\u00f3rios durante o registro."),
		};

		/// <summary>Les 63 cellules livrees : empreinte SHA-256 de la valeur PLEINE (derivee du fichier).</summary>
		private static readonly (string Key, string Sha256)[] Pins =
		{
			("Cancel.Text", "bb9dbb406dcbd5248babc609bb3d021d384398f83dbdb8d53c9b0e32c2aad915"),
			("MaxInvalidPasswordAttemptsTitle.Help", "76abe26911f8901efaa2150f20d53e0f4e4a262519a67fbc2e726194ab12eb02"),
			("MaxInvalidPasswordAttemptsTitle.Text", "5469a748cfb4e9629f63934ddbd0bff5a0ee333e6f3b1dbecf4581668dd09a31"),
			("MinNonAlphanumericCharactersTitle.Help", "b1b68dcf20341b344873ee3de59e93d8e5e7958799d5b00ace1bddcf393d282c"),
			("MinNonAlphanumericCharactersTitle.Text", "74a1313bfde02fbcb78f2dc0fac6c72ee5315cb5b0097d3fc059272deaf5a66c"),
			("MinPasswordLengthTitle.Help", "36e54fb00a99cd09009aff4e9514f73eba3fbd1be678ee8f349ede03677a6ff1"),
			("MinPasswordLengthTitle.Text", "2ac1ebc9cd9f1690f47560f1281de9069f7a7361aab1ed45d4169b486b10a3d2"),
			("No.Text", "9bc947e58b62b2fef2954dc2813f3bfe458455fea1bb40b2d7a33e010dfe9ab6"),
			("PasswordAttemptWindowTitle.Help", "c255059ebfa49580ec62101fcba1f6dfb506e33747a33db738fa77d68bcd93dd"),
			("PasswordAttemptWindowTitle.Text", "96b90e729448b89c403682d0ab49ca00ffea6f7ed7a939859bc65076a5823db3"),
			("PasswordFormat.Help", "ae79ec7288dcb576a4e02fe2793ca5fcfec2c2406450b12b1a1607f34f8bd5f3"),
			("PasswordFormat.Text", "daa5fb55c462753471c63ff6e971a305b63acc472fd78f79f6c6d801d61e4bd3"),
			("PasswordResetEnabledTitle.Help", "d3921075fa58d755342ec0d294b3158ad7247fadb4f8fcdb76e936f580353cf4"),
			("PasswordResetEnabledTitle.Text", "dded78619c4d45975a5ee477d784d2621ef157772543af1844cc47a491fe0b89"),
			("PasswordRetrievalEnabled.Help", "5d571aa6a32fda62863bc5a22fd419592108559f2f273504f7037cb231824cb2"),
			("PasswordRetrievalEnabled.Text", "6ead353b866af771a6f2ae7de9d5e930967f11f8b40878525a7f024975efffc7"),
			("PasswordStrengthRegularExpressionTitle.Help", "4ea6e6852bdb557414cfa47aba573108c88e68bcd903ecd0a57bbcba074d0760"),
			("PasswordStrengthRegularExpressionTitle.Text", "2a0d4a4edbc2e78f6394ad630d274926cc1efc2eaf9a2883ea261c3f9c28eac8"),
			("RedirectionMovedToSiteSettings.Text", "b0cc68e245d8c819b7f79cb85ea1486293d3455a0b0954efa5e76c0f3006abc3"),
			("RegistrationSettingsError.Text", "7303020e6471eb74ad450406d2578eae9a92d29765da4306f4d97915e514538b"),
			("RegistrationSettingsRestoreWarning.Text", "88a77a9679ffcd2a0d00f68aacf593df9cbb274c25c3d3a37517529a0f2ca15b"),
			("RegistrationSettingsUpdateSuccess.Text", "9ff2e8e0740816bcd6b93ea5b144f6ac5e6939be19fc508b5bbea40a723096cc"),
			("Registration_ExcludeTerms.Help", "788b165e785d2a70a187a1ce7bee7f1fd51e9fe9eda734f2d936dc28abec3bde"),
			("Registration_ExcludeTerms.Text", "343f5e7ae57418681c49f478130630615b3092cfaf2c2d7f7b1123e733b25204"),
			("Registration_RandomPassword.Help", "6d4b49b3bd639e2196c52b870ffac4e1e67a98a3613fdc8f698bb4234daeaf8a"),
			("Registration_RandomPassword.Text", "9d92cb27d6f39ea4e3c09d5b61ab99edb5737e87e63f775b45e28d0d97d8d239"),
			("Registration_RequireConfirmPassword.Help", "5591dd3312f216005683a6862dca7e5fe52ff1bebecad5d29a610d4063905771"),
			("Registration_RequireConfirmPassword.Text", "b1d80d362c316e6d464058ced2d023d1da6efe664dae37bb7ed05930ae07e6f2"),
			("Registration_RequireUniqueDisplayName.Help", "536ab236cde8b2e6fd86b63895cb55a3bbdd07107c75c7308fdbd1d12252c19f"),
			("Registration_RequireUniqueDisplayName.Text", "cebad65ce0a4c4b09acc5245507870bab6b7238e96c34d40e0a74f670fd3d4ee"),
			("Registration_UseAuthProviders.Help", "02f359fb0dd73066e67108d8f7903e78a44f3d2ba745622e278234c98548dcc1"),
			("Registration_UseAuthProviders.Text", "8c045d6331b269e36472b6a0d58a85da6f29bdd35294052ad89a4ed36c4f8407"),
			("Registration_UseEmailAsUserName.Help", "afbfccbe50487f899b9c699d539864fc1a468b3e0fdd6b6eb08369226a507373"),
			("Registration_UseEmailAsUserName.Text", "6a514e34c9a12bb47973cdec372d9b5604053a7146ed882986ae4f75b6fb9b4d"),
			("Registration_UseProfanityFilter.Help", "218434de13c8c5e970409646766e97cab7be5be29ad42462d2d6a1a03c5989a6"),
			("Registration_UseProfanityFilter.Text", "3456190d5faef78ceec9400bebbc0ea7f7eba54a0acc0810a9f5aaed8d5fb137"),
			("RequiresQuestionAndAnswerTitle.Help", "b6294e4f8f4b872548dacf66c8ce9aacaf26b6ac4ed74f6b331e82cc9e18c782"),
			("RequiresQuestionAndAnswerTitle.Text", "10b9bedca4511045876c3f01ede2c719a4b77e40fb878589a9fb69a8cf11c78d"),
			("RequiresUniqueEmail.Help", "3f6e0b38afac6254981f32ef162a420c50eb247deefb3f5b2014ad41d7a9f630"),
			("RequiresUniqueEmail.Text", "7944455bfed8de4d9e18e408839a92f8bd16432c0bf7021c8639daf065e076ae"),
			("Save.Text", "aef7cd5b2081351dcd4b3767cc84f77797ee1a945e758e8013961c283304b947"),
			("Security_CaptchaRegister.Help", "014d654c5bfa74b6d5277b469e6996320de0b542c6c29fed7331363dcb01d3cc"),
			("Security_CaptchaRegister.Text", "b4423623e6328cfde9766073d78ed07630245b468db01d013638007ae44b29ad"),
			("Security_DisplayNameFormat.Help", "ca11e43c9c41af8fb26c6f8dc126bd8382b4620b8e26dee293fe9918e17a08d2"),
			("Security_DisplayNameFormat.Text", "0f37d7dc98b5e5814afab58daf779464df28d008c16e1a603cd89bdfff428b30"),
			("Security_EmailValidation.Help", "8b48fa77f336fd616e7581f4e0f6d260a97e9e1fbfd02c0d4a4513f485069293"),
			("Security_EmailValidation.Text", "04d9d7f9348688dd721ebdda5d9991a82543dd08a1930de18b5f29fe7fbb3507"),
			("Security_RequireValidProfile.Help", "7c5c0309fb7837e70a391663a632eac00a307726d270af2f8e639bb3de214137"),
			("Security_RequireValidProfile.Text", "9352dad452558b9e3025a22234a7377a6b29934f1d9a0df1ad2ec53e8e9bc829"),
			("Security_UserNameMinLength.ErrorMessage", "02c93425c7e257d477efbdba6f4249a12772fc69a6a784af959483c2f2fed867"),
			("Security_UserNameMinLength.Help", "fb34cdc21e449299e9fa55d192d9b00c4466afeca8f0637d5fced2966094536e"),
			("Security_UserNameMinLength.Text", "a767f319e29a158d9ed10942ea21d0b84bc0e0356b1ca7f3bc98b443ffa9db12"),
			("Security_UserNameValidation.Help", "81c159e7ac38129527d10e722840be12d459c8d6a5584e466f8f6717690748a3"),
			("Security_UserNameValidation.Text", "ebbffaf9bb126581a1575e608be7aba8c15a6770b0329790b5c7908c7f6f5bd7"),
			("Yes.Text", "9313455e45aa22430a92fddbb70f21312d3c944ab590dfb1da845c1a96b664fe"),
			("plEnableRegisterNotification.Help", "65576c862b18ac98c9127228dffedd7310e40684faccde64d819b82460247556"),
			("plEnableRegisterNotification.Text", "33b264f83b9f98ae3065329109f950eb07f80d81f30105d9ac920e980b75c9d2"),
			("plUserRegistration.Help", "bb66e2a798a1e75149043a561445b0fde376f0e047b5d6302c4402f56dc30852"),
			("plUserRegistration.Text", "87e13694f7dae8d464baa30356b84ca61b235e3dbb4217a3a08dee94e5d2f893"),
			("registrationFieldsLabel.Help", "40b2fd47dd49c9fd6ff1b7a339483bf6494fad2de8c80d87dcfc8ad572bcb358"),
			("registrationFieldsLabel.Text", "d412e2776e30fd5287b27676414ede769ff8ab0ff7a5c4b7acb50dfc236204d4"),
			("registrationFormTypeLabel.Help", "1570dc8e08da703f1b1dbd2f400ae2e4f02a22dd365680b14e4695a118a86a17"),
			("registrationFormTypeLabel.Text", "08f1647ae0584a549084b141d1082fc0728c42adba8ab6ecc1a00c571b049ea5"),
		};

		/// <summary>Homographes LEGITIMES : la cellule pt egale l'es parce que le mot est identique
		/// dans les deux langues. Toute entree doit rester VIVANTE (verifie ci-dessous).</summary>
		private static readonly (string Key, string Word, string NeutralWitness)[] DeclaredHomographs =
		{
			("Cancel.Text", "Cancelar", "Cancel"),
		};

		[Fact]
		public void Registration_Tab_Keys_All_Present()
		{
			var pt = Load(PtResx);
			var missing = TabKeys.Where(k => !pt.ContainsKey(k)).ToList();
			missing.Should().BeEmpty("#458 : les 63 cles de l'onglet Registration sont livrees (4 creees : No.Text, Security_UserNameMinLength.{Text,Help,ErrorMessage}).");
			TabKeys.Should().HaveCount(63);
			TabKeys.Distinct().Should().HaveCount(63);
		}

		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_File_Level_Es_Restricted_To_Neutral()
		{
			// errata web2 : la grille se calibre sur l'es-ES du FICHIER, restreint aux cles du neutre
			// (calibrer sur l'onglet avait fabrique l'angle mort « Forzar » de #2085 ;
			//  calibrer sur les 105 cles es mortes calibrerait sur du contenu sans equivalent neutre).
			var es = Load(EsResx);
			var neu = Load(NeutralResx);
			var live = es.Where(kv => neu.ContainsKey(kv.Key)).ToList();
			live.Should().HaveCountGreaterThan(300, "es∩neutre : le fichier porte ~359 cles partagees");
			var hits = live.Sum(kv => SpanishMarkers.Count(m => kv.Value.ToLowerInvariant().Contains(m)));
			hits.Should().BeGreaterThan(150, "mesure : 171 occurrences des 21 marqueurs sur l'es-ES∩neutre — un ecran qui n'en voit aucun n'etablit rien");
			SpanishMarkers.Should().HaveCount(21).And.OnlyHaveUniqueItems();
			SpanishMarkers.Should().NotContain("número", "« número » est un mot portugais valide — ecarte comme enlace/caduca");
		}

		[Fact]
		public void Registration_Tab_Carries_No_Spanish_Marker()
		{
			var pt = Load(PtResx);
			var offenders = TabKeys
				.SelectMany(k => SpanishMarkers
					.Where(m => pt[k].ToLowerInvariant().Contains(m))
					.Select(m => $"{k} : marqueur «{m}»"))
				.ToList();
			offenders.Should().BeEmpty("#458 : 20 occurrences sur 17 cellules avant reecriture, 0 apres (mesure)");
		}

		[Fact]
		public void Historical_Macaronic_Forms_Are_Gone_And_Each_Has_A_Witness()
		{
			var pt = Load(PtResx);
			HistoricalForms.Should().HaveCount(24, "liste figee : chaque forme est temoignee");
			foreach (var (form, why, witness) in HistoricalForms)
			{
				// (a) temoin : la forme vivait bien dans l'ancienne cellule (litteral embarque au generateur)
				witness.ToLowerInvariant().Contains(form.ToLowerInvariant()).Should().BeTrue(
					$"le temoin de «{form}» ({why}) doit contenir la forme");
				// (b) la forme a quitte l'onglet
				TabKeys.Where(k => pt[k].Contains(form, StringComparison.Ordinal)).Should().BeEmpty(
					$"forme historique «{form}» ({why}) reintroduite");
			}
		}

		[Fact]
		public void Numeric_Bounds_Match_The_Neutral()
		{
			var pt = Load(PtResx);
			var neu = Load(NeutralResx);
			foreach (var key in TabKeys)
			{
				var expected = System.Text.RegularExpressions.Regex.Matches(neu[key], @"\d+").Select(m => m.Value);
				var actual = System.Text.RegularExpressions.Regex.Matches(pt[key], @"\d+").Select(m => m.Value);
				actual.Should().Equal(expected, $"les nombres de {key} sont ceux du neutre (transposition invisible a toute grille)");
			}
			// bornes du seul couple numerique du volet, epinglees a la lettre
			pt["Security_UserNameMinLength.ErrorMessage"].Should().Contain("maior que 0");
			pt["Security_UserNameMinLength.ErrorMessage"].Should().Contain("menor ou igual a 100");
			pt["PasswordStrengthRegularExpressionTitle.Help"].Should().Contain("7.1.0");
		}

		[Fact]
		public void Pins_Exact_Values()
		{
			var pt = Load(PtResx);
			Pins.Should().HaveCount(63);
			var offenders = Pins
				.Where(p => !pt.ContainsKey(p.Key) || Sha256Hex(pt[p.Key]) != p.Sha256)
				.Select(p => p.Key)
				.ToList();
			offenders.Should().BeEmpty("#458 : chaque cellule de l'onglet est epinglee — une mutation sans marqueur doit nommer la cle");
		}

		[Fact]
		public void Placeholder_Parity()
		{
			var pt = Load(PtResx);
			var neu = Load(NeutralResx);
			const string pattern = @"\{\d\}|\[[A-Z]+\]";
			foreach (var key in TabKeys)
			{
				var expected = System.Text.RegularExpressions.Regex.Matches(neu[key], pattern).Select(m => m.Value).OrderBy(x => x);
				var actual = System.Text.RegularExpressions.Regex.Matches(pt[key], pattern).Select(m => m.Value).OrderBy(x => x);
				actual.Should().Equal(expected, $"les jetons de {key} sont ceux du neutre");
			}
			pt["Security_DisplayNameFormat.Help"].Should().Contain("[FIRSTNAME] [LASTNAME]");
		}

		[Fact]
		public void Values_Are_Text_Not_Nested_Xml()
		{
			// lecon M9 de #2085 : une balise brute non echappee devient un XElement enfant et DISPARAIT
			// de .Value — on mesure les NOEUDS, pas la chaine.
			var doc = XDocument.Load(PtResx);
			var offenders = doc.Root!.Elements("data")
				.Where(d => TabKeys.Contains((string)d.Attribute("name")!))
				.Where(d => d.Element("value")!.Nodes().Any(n => n is XElement))
				.Select(d => (string)d.Attribute("name")!)
				.ToList();
			offenders.Should().BeEmpty();
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
			TabKeys.Count(k => pt[k].Contains("nome de exibição", StringComparison.Ordinal))
				.Should().BeGreaterThan(0, "« nome de exibição » : 7 precedents dans les fichiers pt-BR propres (users, recyclebin)");
			TabKeys.Where(k => pt[k].Contains("nome para mostrar", StringComparison.Ordinal)).Should().BeEmpty(
				"« nome para mostrar » est le calque espagnol (« nombre para mostrar »)");
			pt["Save.Text"].Should().Be("Salvar", "precedent roles.Save.Text / users.btnSave.Text (fichiers propres)");
			pt["Yes.Text"].Should().Be("Sim", "« Sí » etait de l'espagnol — defaut de SENS dans une cle partagee");
			pt["No.Text"].Should().Be("Não");
			pt["Cancel.Text"].Should().Be("Cancelar");
			pt["RegistrationSettingsError.Text"].Should().Contain("Tente novamente mais tarde",
				"precedent users.RegisterationFailed.Text");
			pt["RegistrationSettingsRestoreWarning.Text"].Should().StartWith("Tem certeza de que deseja",
				"precedent roles.DeleteRole.Confirm");
			pt["plUserRegistration.Help"].Should().Contain("Usuários registrados",
				"nom de la fonction tel que users.RegisteredUsers.Text le porte");
			TabKeys.Count(k => pt[k].Contains("e-mail", StringComparison.Ordinal))
				.Should().BeGreaterThan(0, "autre nom pt-BR (users, SendPassword)");
			TabKeys.Where(k => pt[k].Contains("e-mail especial", StringComparison.Ordinal)).Should().BeEmpty();
		}

		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell_Except_Declared_Homographs()
		{
			var pt = Load(PtResx);
			var es = Load(EsResx);
			var declared = DeclaredHomographs.Select(h => h.Key).ToHashSet(StringComparer.Ordinal);
			var offenders = TabKeys
				.Where(k => es.ContainsKey(k) && pt[k] == es[k] && !declared.Contains(k))
				.ToList();
			offenders.Should().BeEmpty("7 cellules egales a l'es avant reecriture ; seule « Cancelar » l'est legitimement");
			// l'exemption doit rester VIVANTE : une homographe declaree qui cesse de l'etre est une entree morte
			foreach (var (key, word, neutralWitness) in DeclaredHomographs)
			{
				pt[key].Should().Be(word);
				es[key].Should().Be(word, "un homographe declare doit etre identique dans les deux langues");
				Load(NeutralResx)[key].Should().Be(neutralWitness, "temoin neutre de l'homographe");
			}
		}

		[Fact]
		public void Legacy_Keys_Survive_The_Rewrite()
		{
			var pt = Load(PtResx);
			// plancher (pas compte exact) : le compte EXACT vit dans la garde du volet COURANT —
			// sinon chaque onglet suivant de la decoupe casse la garde du precedent (regle #2102).
			// IP filters ajoute ses 3 cles -> 362 dans la garde IP filters.
			pt.Should().HaveCountGreaterThanOrEqualTo(359,
				"anti-effondrement : 355 (base #2094) + 4 cles creees par Registration. Les onglets suivants de la decoupe ajoutent les leurs (IP filters -> 362) : le compte EXACT vit dans la garde de chaque onglet, pas ici");
			foreach (var key in new[] { "AsyncTimeout.ErrorMessage", "SslOn.Text", "TabMemberSettings.Text", "plForceLogOutAfterPasswordChanged.Text" })
				pt.Should().ContainKey(key, "les cles des onglets precedents survivent a la reecriture");
		}

		[Fact]
		public void Committed_Blob_Is_Utf8_Bom_With_Lf_Only()
		{
			// Invariant de TRANSPORT sur le blob committé : un runner Windows core.autocrlf=true
			// convertit LF -> CRLF AU CHECKOUT — un fichier sain apparaît en CRLF dans l'arbre.
			var bytes = CommittedBlob("DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Security/App_LocalResources/security.pt-BR.resx");
			bytes.Length.Should().BeGreaterThan(1000, "le blob doit etre le fichier entier, pas un fragment");
			bytes.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "le BOM est celui du fichier d'origine");
			bytes.Count(b => b == 0x0D).Should().Be(0, $"le blob committé de '{PtResx}' est en LF seul");
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
