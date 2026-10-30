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
	/// Garde du sweep pt-BR (pool #458) : <c>Dnn.Users/App_LocalResources/users.pt-BR.resx</c>
	/// — la console d'administration des utilisateurs (233 clés).
	/// <para>
	/// Le fichier n'était pas du portugais teinté d'espagnol : c'était un texte
	/// espagnol avec des mots portugais insérés (« Introduzca a respuesta para a
	/// pregunta da contrasenha », « Empieze a escribir para adicionar um rol ao
	/// usuario », « Não foin podido eliminar todos os usuarios »). Il est retraduit
	/// en entier : <b>209 des 231 valeurs</b> changent, et <b>2 clés absentes</b>
	/// sont restaurées (<c>UserId.Help</c>, <c>UserId.Text</c>) — la complétude et
	/// la langue sont deux mesures distinctes.
	/// </para>
	/// <para>
	/// Terminologie, adossée aux fichiers pt-BR déjà mergés et non au corpus
	/// (le corpus pt-BR est lui-même la contamination : « contrasenha » 182
	/// occurrences contre « senha » 114, « Eliminar » 120 contre « Excluir » 40) :
	/// <c>Senha</c> (pas « contrasenha »), <c>Usuário</c> accentué, <c>Função</c>
	/// pour Role (décision du sweep 6/6), <c>Excluir</c>/<c>Salvar</c>/<c>Status</c>,
	/// <c>Sobrenome</c> pour LastName (aucun précédent dans le corpus — choix
	/// explicite), <c>Nome de exibição</c> (précédent : deux fichiers mergés),
	/// <c>Informe</c> pour « Enter » (précédent : roles.pt-BR.resx).
	/// </para>
	/// <para>
	/// Les 17 cellules HTML sont dérivées du <b>neutre</b> par remplacement de
	/// prose : leur séquence de balises est donc identique par construction, et
	/// la garde le vérifie plutôt que de le supposer.
	/// </para>
	/// </summary>
	public class DnnUsersPtBrGuardTests
	{
		private const string PtResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Users/App_LocalResources/users.pt-BR.resx";
		private const string EsResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Users/App_LocalResources/users.es-ES.resx";
		private const string NeutralResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Users/App_LocalResources/Users.resx";

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

		private static string ResxValue(string rel, string key) => Cells(rel)[key];

		// ------------------------------------------------------------------
		// Grille calibrée SUR CE FICHIER (calibrate_users.py) : 39 marqueurs qui
		// touchent tous l'es-ES d'ici. 39 autres candidats sont AVEUGLES sur cet
		// es-ES (0 occurrence : « Ruta », « Ancho », « Parámetros », « usted »…) et
		// sont donc ABSENTS : un marqueur aveugle rendrait un 0 qui ne prouve rien.
		// Exclusions mesurées, pas devinées : « que », « se », « como », « está »,
		// « será », « desde », « todos », « perfil » sont du portugais valide ;
		// « roles » n'apparaît qu'en nom de commande (--roles, add-roles).
		// ------------------------------------------------------------------
		private static readonly Regex[] SpanishMarkers =
		{
			new(@"\w*(ción|siones)\b", RegexOptions.IgnoreCase),
			new(@"\w*sión\b", RegexOptions.IgnoreCase),
			new(@"\w*ñ\w*", RegexOptions.IgnoreCase),
			new(@"[¿¡]", RegexOptions.IgnoreCase),
			new(@"\busuarios?\b", RegexOptions.IgnoreCase),
			new(@"\busuario\b", RegexOptions.IgnoreCase),
			new(@"\bsitio\b", RegexOptions.IgnoreCase),
			new(@"\bnombre\b", RegexOptions.IgnoreCase),
			new(@"\bel\b", RegexOptions.IgnoreCase),
			new(@"\blos\b", RegexOptions.IgnoreCase),
			new(@"\blas\b", RegexOptions.IgnoreCase),
			new(@"\bdel\b", RegexOptions.IgnoreCase),
			new(@"\buna\b", RegexOptions.IgnoreCase),
			new(@"\bpermisos?\b", RegexOptions.IgnoreCase),
			new(@"\btiene\b", RegexOptions.IgnoreCase),
			new(@"\bpuede\b", RegexOptions.IgnoreCase),
			new(@"\bdebe\b", RegexOptions.IgnoreCase),
			new(@"\bIntroduzca\b", RegexOptions.IgnoreCase),
			new(@"\bTamaño\b", RegexOptions.IgnoreCase),
			new(@"\barchivo\w*\b", RegexOptions.IgnoreCase),
			new(@"\bcarpeta\b", RegexOptions.IgnoreCase),
			new(@"\bsolo\b", RegexOptions.IgnoreCase),
			new(@"\bnuev[oa]\b", RegexOptions.IgnoreCase),
			new(@"\bconfiguraci[óo]n\b", RegexOptions.IgnoreCase),
			new(@"\bCambiar\b", RegexOptions.IgnoreCase),
			new(@"\bfecha\b", RegexOptions.IgnoreCase),
			new(@"\bcontrase[ñn]a\b", RegexOptions.IgnoreCase),
			new(@"\bbuscar\b", RegexOptions.IgnoreCase),
			new(@"\beliminar\b", RegexOptions.IgnoreCase),
			new(@"\bactiv[oa]s?\b", RegexOptions.IgnoreCase),
			new(@"\bapellidos?\b", RegexOptions.IgnoreCase),
			new(@"\brespuesta\b", RegexOptions.IgnoreCase),
			new(@"\bpregunta\b", RegexOptions.IgnoreCase),
			new(@"\bdespués\b", RegexOptions.IgnoreCase),
			new(@"\bhaya\b", RegexOptions.IgnoreCase),
			new(@"\bInicio\b", RegexOptions.IgnoreCase),
			new(@"\brol\b", RegexOptions.IgnoreCase),
			new(@"\bcreación\b", RegexOptions.IgnoreCase),
			new(@"\bestab\w*\b", RegexOptions.IgnoreCase),
		};

		[Fact]
		public void Keys_Match_The_Neutral_File()
		{
			var pt = Cells(PtResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			var ne = Cells(NeutralResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			pt.Should().Equal(ne, "le fichier pt-BR doit porter exactement les clés du neutre");
			pt.Should().HaveCount(233, "233 clés au neutre — deux étaient absentes avant le sweep");
		}

		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_Es()
		{
			var esCells = Cells(EsResx).Values.ToArray();
			SpanishMarkers
				.Where(m => !esCells.Any(v => m.IsMatch(v)))
				.Select(m => m.ToString())
				.Should().BeEmpty("chaque marqueur doit être vu par l'es-ES (sinon son 0 sur le pt-BR est aveugle)");
		}

		/// <summary>Invariant de FICHIER ENTIER : les 233 cellules sont en portugais.</summary>
		[Fact]
		public void Every_Cell_Carries_No_Spanish_Marker()
		{
			var offenders = new List<string>();
			foreach (var (key, value) in Cells(PtResx))
			foreach (var marker in SpanishMarkers)
				if (marker.IsMatch(value))
					offenders.Add($"{key} [{marker}] :: {value}");
			offenders.Should().BeEmpty(
				"les 233 cellules pt-BR de la console utilisateurs sont en portugais — tout marqueur restant est une réinjection : {0}",
				string.Join(" | ", offenders));
		}

		/// <summary>Formes macaroniques réellement présentes avant le sweep :
		/// présence prouvée dans le littéral historique, absence exigée du fichier courant.</summary>
		[Fact]
		public void Historical_Macaronic_Forms_Are_Gone()
		{
			var historical = new Dictionary<string, string>
			{
				["contrasenha"] = "Introduzca a respuesta para a pregunta da contrasenha.",
				["Apellidos:"] = "Apellidos:",
				["Empieze"] = "Empieze a escribir para adicionar um rol ao usuario.",
				["nenhum rol"] = "Não foi encontrado nenhum rol.",
				["Adicionar usuario"] = "Adicionar usuario",
				["Guardar"] = "Guardar",
				["Añadir"] = "Añadir",
				["Introduzca o email"] = "Introduzca o email.",
				["Vuelva a entrar"] = "Vuelva a entrar a contrasenha para confirmarla.",
				["recordatorio"] = "recordatorio de contrasenha.",
				["Tem que introducir"] = "Tem que introducir a respuesta.",
				["deshabilitada"] = "foi sido deshabilitada para este sitio",
				["Comandos de usuarios"] = "Comandos de usuarios",
				["Eliminar usuario"] = "Eliminar usuario",
				["superusuario"] = "Convertir em superusuario",
				["tienem"] = "A contrasenha e a confirmação tienem que coincidir.",
				["foin"] = "Não foin podido eliminar todos os usuarios.",
				["hubiera"] = "como si não hubiera consentido.",
				["aceptado"] = "não foi aceptado por razones de seguridade.",
				["seguridade"] = "por motivos de seguridade.",
				["escogido"] = "Verifique que foi escogido uma contrasenha",
				["compleja"] = "o suficientemente larga e compleja",
				["papelera de reciclaje"] = "se pode encontrar o a papelera de reciclaje",
				["ejecutar um segundo comando"] = "tem que ejecutar um segundo comando",
				["cualquier confusión"] = "para evitar cualquier confusión",
				["Estado"] = "Estado de aprobação do usuario.",
				["Usuarios registrados"] = "Usuarios registrados",
			};
			var current = string.Join("\n", Cells(PtResx).Values);
			foreach (var (form, witness) in historical)
			{
				witness.Should().Contain(form, "témoin littéral : la forme historique doit être présente dans le littéral");
				current.Should().NotContain(form, $"« {form} » est une forme retirée — sa réapparition est une régression");
			}
		}

		private static readonly Dictionary<string, string> Pins = new()
		{
			["AccountData.Text"] = "Dados da conta",
			["AccountSettings.Text"] = "Configurações da conta",
			["Add.Text"] = "Adicionar",
			["AddRolePlaceHolder.Text"] = "Comece a digitar para adicionar uma função a este usuário.",
			["All.Text"] = "Todos",
			["Answer.Help"] = "Informe uma resposta para a pergunta de senha.",
			["Answer.Required"] = "Você deve informar uma resposta de senha.",
			["Answer.Text"] = "Resposta da senha:",
			["Approved.Help"] = "Indica se este usuário está autorizado no site.",
			["Approved.Text"] = "Autorizado:",
			["Authorized.Header"] = "Status",
			["Authorized.Text"] = "Autorizado",
			["Cancel.Text"] = "Cancelar",
			["CannotAddUser.Text"] = "Este site está configurado para exigir que os usuários informem uma Pergunta e uma Resposta para receber lembretes de senha.  Esta configuração é incompatível com a adição de usuários por administradores, portanto foi desativada para este site.",
			["CannotChangePassword.Text"] = "Sua configuração exige que o usuário informe uma Pergunta e uma Resposta para o lembrete de senha.  Quando esta configuração está aplicada, os administradores não conseguem alterar a senha de um usuário, portanto o recurso foi desativado para este site.",
			["ChangePassword.Text"] = "Alterar senha",
			["ChangeSuccessful.Text"] = "Senha alterada com sucesso",
			["Confirm.Help"] = "Informe novamente a senha para confirmar.",
			["Confirm.Required"] = "Você deve informar uma confirmação de senha.",
			["Confirm.Text"] = "Confirmar senha:",
			["ConfirmMismatch.ErrorMessage"] = "A senha e a confirmação de senha devem coincidir.",
			["CouldNotRemoveAll.Text"] = "Não foi possível excluir todos os usuários",
			["Created.Header"] = "Ingressou",
			["CreatedDate.Help"] = "A data em que esta conta de usuário foi criada.",
			["CreatedDate.Text"] = "Data de criação:",
			["Delete.Text"] = "Excluir",
			["DeleteRole.Confirm"] = "Tem certeza de que deseja remover a função '{0}' de '{1}'?",
			["DeleteUser.Confirm"] = "Tem certeza de que deseja excluir este usuário?",
			["DeleteUser.Text"] = "Excluir usuário",
			["Deleted.Text"] = "Excluídos",
			["DemoteFromSuperUser.Text"] = "Tornar usuário comum",
			["DemoteToRegularUser.Text"] = "Tornar usuário comum",
			["DisplayName.Help"] = "Informe um nome de exibição.",
			["DisplayName.RegExError"] = "O nome de exibição é inválido.",
			["DisplayName.Required"] = "O nome de exibição é obrigatório.",
			["DisplayName.Text"] = "Nome de exibição:",
			["Email.Header"] = "Email",
			["Email.Help"] = "Informe um endereço de email válido.",
			["Email.RegExError"] = "Você deve informar um endereço de email válido.",
			["Email.Required"] = "O email é obrigatório.",
			["Email.Text"] = "Endereço de email:",
			["Expires.Header"] = "Expira",
			["False.Text"] = "Não",
			["FirstName.Help"] = "Informe um nome.",
			["FirstName.RegExError"] = "O nome é inválido.",
			["FirstName.Required"] = "O nome é obrigatório.",
			["FirstName.Text"] = "Nome:",
			["ForceChangePassword.Text"] = "Forçar alteração de senha",
			["HasAgreedToTerms.Help"] = "O usuário aceitou os termos de uso e a declaração de privacidade deste site por meio do mecanismo de consentimento de dados.",
			["HasAgreedToTerms.Text"] = "Consentiu com os termos",
			["HasAgreedToTerms.title"] = "O usuário aceitou os termos de uso",
			["HasNotAgreedToTerms.Text"] = "Não consentiu com os termos",
			["HasNotAgreedToTerms.title"] = "O usuário não aceitou os termos de uso",
			["InSufficientPermissions.Text"] = "Você não tem permissões suficientes para executar esta ação.",
			["InvalidPasswordAnswer.Text"] = "A resposta da senha é inválida.",
			["IsDeleted.Help"] = "Indica se este usuário está excluído.",
			["IsDeleted.Text"] = "Excluído:",
			["IsOnLine.Help"] = "Indica se o usuário está online no momento.",
			["IsOnLine.Text"] = "Usuário está online:",
			["IsOwner.Text"] = "É proprietário",
			["LastActivityDate.Help"] = "A data em que este usuário esteve ativo no site pela última vez.",
			["LastActivityDate.Text"] = "Data da última atividade:",
			["LastConsented.Help"] = "A última vez em que o usuário consentiu com os termos deste site. Se os termos foram alterados desde então, o usuário pode ser marcado como não tendo consentido.",
			["LastConsented.Text"] = "Último consentimento em",
			["LastLockoutDate.Help"] = "A data em que este usuário foi bloqueado no site pela última vez devido a repetidas tentativas de login com falha.",
			["LastLockoutDate.Text"] = "Data do último bloqueio:",
			["LastLoginDate.Help"] = "A data em que este usuário fez login no site pela última vez.",
			["LastLoginDate.Text"] = "Data do último login:",
			["LastName.Help"] = "Informe um sobrenome.",
			["LastName.RegExError"] = "O sobrenome é inválido.",
			["LastName.Required"] = "O sobrenome é obrigatório.",
			["LastName.Text"] = "Sobrenome:",
			["LastPasswordChangeDate.Help"] = "A data em que este usuário alterou a senha pela última vez.",
			["LastPasswordChangeDate.Text"] = "Última alteração de senha:",
			["LockedOut.Help"] = "Indica se o usuário está bloqueado no site no momento, devido a repetidas tentativas de login com falha.",
			["LockedOut.Text"] = "Bloqueado:",
			["ManageProfile.title"] = "Configurações do perfil",
			["ManageRoles.title"] = "Funções do usuário",
			["ManageSettings.title"] = "Configurações da conta",
			["Name.Header"] = "Nome",
			["Never.Text"] = "Nunca",
			["NewConfirm.Help"] = "Informe novamente sua nova senha para confirmar.",
			["NewConfirm.Required"] = "Você deve informar uma confirmação de senha.",
			["NewConfirm.Text"] = "Confirmar senha:",
			["NewConfirmMismatch.ErrorMessage"] = "A nova senha e a confirmação de senha devem coincidir.",
			["NewPassword.Help"] = "Informe sua nova senha.",
			["NewPassword.Required"] = "Você deve informar uma senha.",
			["NewPassword.Text"] = "Nova senha:",
			["NoRoles.Text"] = "Nenhuma função encontrada.",
			["Notify.Text"] = "Enviar um email ao novo usuário",
			["OptionUnavailable.Text"] = "A opção de redefinir a senha não está disponível no momento.",
			["Password.Help"] = "Informe uma senha para este usuário.",
			["Password.Required"] = "Você deve informar uma senha.",
			["Password.Text"] = "Senha:",
			["PasswordInvalid.Text"] = "Você deve informar uma senha válida.  Consulte o administrador do site se você não conhece os requisitos de senha.",
			["PasswordManagement.Text"] = "Gerenciamento de senhas",
			["PasswordResetFailed.Text"] = "Sua nova senha não foi aceita por motivos de segurança.  Informe uma senha que você não tenha usado antes e que seja longa e complexa o suficiente para atender aos requisitos de complexidade de senha do site.",
			["PasswordResetFailed_PasswordInHistory.Text"] = "Sua nova senha não foi aceita por motivos de segurança. Informe uma senha que não tenha sido usada antes.",
			["PasswordSent.Text"] = "Se o nome de usuário informado estiver correto, você receberá em breve um novo email com um link para redefinir sua senha.",
			["PromoteToSuperUser.Text"] = "Tornar superusuário",
			["Prompt_AddRoles_Description.Text"] = "Adicione uma ou mais funções de segurança do DNN a um usuário.",
			["Prompt_AddRoles_FlagEnd.Text"] = "Data de término da função.",
			["Prompt_AddRoles_FlagId.Text"] = "ID do usuário ao qual as funções serão adicionadas. Se um número for passado como primeiro argumento, você não precisa\n            usar a <span class=\"mono\">--id</span> explicitamente",
			["Prompt_AddRoles_FlagRoles.Text"] = "Lista de nomes de funções do DNN separados por vírgula a aplicar ao usuário.",
			["Prompt_AddRoles_FlagStart.Text"] = "Data de início da vigência da função.",
			["Prompt_AddRoles_NotFound.Text"] = "Não é possível encontrar a função{0} '{1}'. O parâmetro --roles aceita somente funções existentes.",
			["Prompt_AddRoles_ResultHtml.Text"] = "<h4>Adicionar uma função a um usuário</h4>\n    <code class=\"block\">add-roles --id 23 --roles Editor</code> OR\n    <code class=\"block\">add-roles 23 --roles Editor</code> OR\n    <code class=\"block\">add-roles --id 23 \"Editor\"</code>\n    <h4>Adicionar uma função com várias palavras a um usuário</h4>\n    <code class=\"block\">add-roles --id 23 --roles \"Article Reviewer\"</code>\n\n    <h4>Adicionar várias funções a um usuário</h4>\n    <code class=\"block\">add-roles --id 23 --roles \"Editor, Writer, Article Reviewer\"</code>",
			["Prompt_CannotPurgeUser.Text"] = "Não é possível remover definitivamente um usuário que não tenha sido excluído primeiro. Tente delete-user.",
			["Prompt_DateParseError.Text"] = "Não foi possível interpretar a data {0} '{1}'. Tente usar o formato YYYY-MM-DD.",
			["Prompt_DeleteUser_Description.Text"] = "Exclui o usuário especificado do portal. Após a exclusão, o usuário ainda pode ser recuperado. Para excluir o usuário permanentemente, siga este comando com o comando <code>purge-user</code> command.",
			["Prompt_DeleteUser_FlagId.Text"] = "O ID do usuário. Se a opção não for usada, o ID do usuário deve ser o primeiro argumento.",
			["Prompt_DeleteUser_FlagNotify.Text"] = "Se verdadeiro, o email de notificação \"Unregister User\" será enviado (normalmente ao administrador do site)",
			["Prompt_DeleteUser_ResultHtml.Text"] = "<h4>Excluir um usuário</h4>\n    <p>Isto exclui o usuário com ID 345. O usuário não é excluído permanentemente neste momento. Ele fica em uma espécie de lixeira. Você pode usar <code>get-user 345</code> para ver os detalhes do usuário e verá que a propriedade <code>IsDeleted</code> agora é True.  Assim, você pode usar o comando <code>restore-user</code> para recuperar o registro do usuário ou pode recuperá-lo pela interface de usuário do DNN. Se quiser excluir o usuário permanentemente, será preciso executar um segundo comando: <code>purge-user</code></p>\n    <code class=\"block\">delete-user 345</code>\n    <p>Esta é a forma mais explícita do código acima.</p>\n    <code class=\"block\">delete-user --id 345</code>\n\n    <h4>Excluir um usuário e enviar notificação</h4>\n    <p>Isto exclui o usuário com ID 345 e envia uma notificação por email. Como acima, o usuário não é excluído permanentemente neste momento. Se quiser excluir o usuário permanentemente, será preciso executar um segundo comando: <code>purge-user</code></p>\n    <code class=\"block\">delete-user 345 --notify true</code>\n    <p>Esta é a forma mais explícita do código acima.</p>\n    <code class=\"block\">delete-user --id 345 --notify true</code>",
			["Prompt_EmailSent.Text"] = "Um email foi enviado ao usuário.",
			["Prompt_GetUser_Description.Text"] = "Permite exibir as informações do usuário atual ou recuperar um usuário pelo seu User ID, nome de usuário ou endereço de email.\n    Suporta busca parcial. Retorna o primeiro usuário encontrado.",
			["Prompt_GetUser_FlagEmail.Text"] = "O endereço de email do usuário que está sendo recuperado. Você pode usar o asterisco (<span class=\"mono\"> * </span>) as a wildcard\n          para significar 0 ou mais caracteres.",
			["Prompt_GetUser_FlagId.Text"] = "Especifica explicitamente o UserId do usuário que está sendo recuperado.",
			["Prompt_GetUser_FlagUsername.Text"] = "O nome de usuário do usuário que está sendo recuperado. Você pode usar o asterisco (<span class=\"mono\"> * </span>) as a wildcard\n          para significar 0 ou mais caracteres.",
			["Prompt_GetUser_ResultHtml.Text"] = "<div>\n    <h4>Obter o usuário atual</h4>\n    <code class=\"block\">\n      get-user\n    </code>\n\n    <h4>Obter usuário pelo ID de usuário</h4>\n\n    <h5>Uso implícito da opção <span class=\"mono\">--id</span></h5>\n    <p>Ao especificar um único valor após o nome do comando, se ele for um número inteiro, o Prompt assumirá que é um ID de usuário</p>\n    <code class=\"block\">get-user 345</code>\n\n    <h5>Uso explícito da opção <span class=\"mono\">--id</span></h5>\n    <p>Você pode usar explicitamente a opção <span class=\"mono\">--id</span> para evitar qualquer confusão</p>\n    <code class=\"block\">get-user --id 345</code>\n    <table class=\"command-result-tbl\">\n      <tr>\n        <td>UserId</td>\n        <td>:</td>\n        <td>345</td>\n      </tr>\n      <tr>\n        <td>Username</td>\n        <td>:</td>\n        <td>jsmith</td>\n      </tr>\n      <tr>\n        <td>Email</td>\n        <td>:</td>\n        <td>jsmith@sample.com</td>\n      </tr>\n      <tr>\n        <td>DisplayName</td>\n        <td>:</td>\n        <td>John Smith</td>\n      </tr>\n      <tr>\n        <td>FirstName</td>\n        <td>:</td>\n        <td>John</td>\n      </tr>\n      <tr>\n        <td>LastName</td>\n        <td>:</td>\n        <td>Smith</td>\n      </tr>\n      <tr>\n        <td>LastLogin</td>\n        <td>:</td>\n        <td>2016-12-06T09:31:38.413-07:00</td>\n      </tr>\n      <tr>\n        <td>IsAuthorized</td>\n        <td>:</td>\n        <td>true</td>\n      </tr>\n    </table>\n\n    <h4>Obter usuário pelo email</h4>\n\n    <h5>Uso implícito da opção <span class=\"mono\">--email</span></h5>\n    <p>Ao especificar um único valor após o nome do comando, se ele contiver o símbolo arroba (<span class=\"mono\"> @ </span>),\n      o Prompt assumirá que é um email</p>\n    <code class=\"block\">get-user jsmith@sample.com</code>\n\n    <h5>Uso explícito da opção <span class=\"mono\">--email</span></h5>\n    <p>Você pode usar explicitamente a opção <span class=\"mono\">--email</span> para evitar qualquer confusão</p>\n    <code class=\"block\">get-user --email jsmith@sample.com</code>\n\n    <table class=\"command-result-tbl\">\n      <tr>\n        <td>UserId</td>\n        <td>:</td>\n        <td>345</td>\n      </tr>\n      <tr>\n        <td>Username</td>\n        <td>:</td>\n        <td>jsmith</td>\n      </tr>\n      <tr>\n        <td>Email</td>\n        <td>:</td>\n        <td>jsmith@sample.com</td>\n      </tr>\n      <tr>\n        <td>DisplayName</td>\n        <td>:</td>\n        <td>John Smith</td>\n      </tr>\n      <tr>\n        <td>FirstName</td>\n        <td>:</td>\n        <td>John</td>\n      </tr>\n      <tr>\n        <td>LastName</td>\n        <td>:</td>\n        <td>Smith</td>\n      </tr>\n      <tr>\n        <td>LastLogin</td>\n        <td>:</td>\n        <td>2016-12-06T09:31:38.413-07:00</td>\n      </tr>\n      <tr>\n        <td>IsAuthorized</td>\n        <td>:</td>\n        <td>true</td>\n      </tr>\n    </table>\n\n    <h4>Pesquisar usuário por email usando curingas</h4>\n    <h5>Uso implícito da opção <span class=\"mono\">--email</span></h5>\n    <p>\n      Ao especificar um único valor após o nome do comando, se ele contiver o símbolo arroba (<span class=\"mono\"> @ </span>),\n      o Prompt assumirá que é um email. Somente o primeiro registro de usuário correspondente será retornado. Tente usar <span class=\"mono\">list-users</span>      se você quiser encontrar todos os usuários correspondentes.\n    </p>\n    <code class=\"block\">get-user jsmith*@sample.com</code>\n    <h5>Uso explícito da opção <span class=\"mono\">--email</span></h5>\n    <code class=\"block\">get-user --email jsmith*@sample.com</code>\n\n    <table class=\"command-result-tbl\">\n      <tr>\n        <td>UserId</td>\n        <td>:</td>\n        <td>345</td>\n      </tr>\n      <tr>\n        <td>Username</td>\n        <td>:</td>\n        <td>jsmith</td>\n      </tr>\n      <tr>\n        <td>Email</td>\n        <td>:</td>\n        <td>jsmith@sample.com</td>\n      </tr>\n      <tr>\n        <td>DisplayName</td>\n        <td>:</td>\n        <td>John Smith</td>\n      </tr>\n      <tr>\n        <td>FirstName</td>\n        <td>:</td>\n        <td>John</td>\n      </tr>\n      <tr>\n        <td>LastName</td>\n        <td>:</td>\n        <td>Smith</td>\n      </tr>\n      <tr>\n        <td>LastLogin</td>\n        <td>:</td>\n        <td>2016-12-06T09:31:38.413-07:00</td>\n      </tr>\n      <tr>\n        <td>IsAuthorized</td>\n        <td>:</td>\n        <td>true</td>\n      </tr>\n    </table>\n\n    <h4>Obter usuário pelo nome de usuário</h4>\n    <h5>Uso implícito da opção <span class=\"mono\">--username</span></h5>\n    <p>Ao especificar um único valor após o nome do comando, se ele não for um número e não contiver o símbolo arroba (\n      <span class=\"mono\"> @ </span>), o Prompt assumirá que é um nome de usuário</p>\n    <code class=\"block\">get-user jsmith</code>\n\n    <h5>Uso explícito da opção <span class=\"mono\">--username</span></h5>\n    <p>Você pode usar explicitamente a opção <span class=\"mono\">--username</span> para evitar qualquer confusão</p>\n    <code class=\"block\">get-user --username jsmith</code>\n\n    <table class=\"command-result-tbl\">\n      <tr>\n        <td>UserId</td>\n        <td>:</td>\n        <td>345</td>\n      </tr>\n      <tr>\n        <td>Username</td>\n        <td>:</td>\n        <td>jsmith</td>\n      </tr>\n      <tr>\n        <td>Email</td>\n        <td>:</td>\n        <td>jsmith@sample.com</td>\n      </tr>\n      <tr>\n        <td>DisplayName</td>\n        <td>:</td>\n        <td>John Smith</td>\n      </tr>\n      <tr>\n        <td>FirstName</td>\n        <td>:</td>\n        <td>John</td>\n      </tr>\n      <tr>\n        <td>LastName</td>\n        <td>:</td>\n        <td>Smith</td>\n      </tr>\n      <tr>\n        <td>LastLogin</td>\n        <td>:</td>\n        <td>2016-12-06T09:31:38.413-07:00</td>\n      </tr>\n      <tr>\n        <td>IsAuthorized</td>\n        <td>:</td>\n        <td>true</td>\n      </tr>\n    </table>\n\n    <h4>Pesquisar usuário por nome de usuário usando curingas</h4>\n    <h5>Uso implícito da opção <span class=\"mono\">--username</span></h5>\n    <p>Ao especificar um único valor após o nome do comando, se ele não for um número e não contiver o símbolo arroba (\n      <span class=\"mono\"> @ </span>), o Prompt assumirá que é um nome de usuário</p>\n    <code class=\"block\">get-user --username jsmith*</code>\n    <h5>Uso explícito da opção <span class=\"mono\">--username</span></h5>\n    <code class=\"block\">get-user jsmith*</code> (o valor não pode conter o símbolo @ nem ser interpretado como um número inteiro para que isto\n    funcione)\n\n    <table class=\"command-result-tbl\">\n      <tr>\n        <td>UserId</td>\n        <td>:</td>\n        <td>345</td>\n      </tr>\n      <tr>\n        <td>Username</td>\n        <td>:</td>\n        <td>jsmith</td>\n      </tr>\n      <tr>\n        <td>Email</td>\n        <td>:</td>\n        <td>jsmith@sample.com</td>\n      </tr>\n      <tr>\n        <td>DisplayName</td>\n        <td>:</td>\n        <td>John Smith</td>\n      </tr>\n      <tr>\n        <td>FirstName</td>\n        <td>:</td>\n        <td>John</td>\n      </tr>\n      <tr>\n        <td>LastName</td>\n        <td>:</td>\n        <td>Smith</td>\n      </tr>\n      <tr>\n        <td>LastLogin</td>\n        <td>:</td>\n        <td>2016-12-06T09:31:38.413-07:00</td>\n      </tr>\n      <tr>\n        <td>IsAuthorized</td>\n        <td>:</td>\n        <td>true</td>\n      </tr>\n    </table>\n  </div>",
			["Prompt_IfSpecifiedMustHaveValue.Text"] = "Se você especificar a opção --{0}, ela deve ser definida como True ou False.",
			["Prompt_InvalidFlag.Text"] = "Opção inválida '--{0}'. Você quis dizer --{1}?",
			["Prompt_ListUsers_Description.Text"] = "Localiza usuários por email ou nome de usuário. Suporta correspondência parcial em Email ou Nome de usuário. Você só pode pesquisar por uma opção por vez. As opções não podem ser combinadas.",
			["Prompt_ListUsers_FlagEmail.Text"] = "Um padrão de pesquisa a procurar no endereço de email do usuário. Você pode usar o asterisco (<span class=\"mono\"> * </span>) como curinga para significar 0 ou mais caracteres",
			["Prompt_ListUsers_FlagMax.Text"] = "Tamanho da página. O máximo é 500.",
			["Prompt_ListUsers_FlagPage.Text"] = "Número da página para exibir os registros.",
			["Prompt_ListUsers_FlagRole.Text"] = "O nome da função de segurança do DNN cujos membros você deseja listar. IMPORTANTE: a busca com curinga NÃO está habilitada para esta opção no momento. Os nomes das funções não diferenciam maiúsculas de minúsculas, mas o espaçamento deve coincidir.",
			["Prompt_ListUsers_FlagUsername.Text"] = "Um padrão de pesquisa a procurar no nome de usuário. Você pode usar o asterisco (<span class=\"mono\"> * </span>) como curinga para significar 0 ou mais caracteres",
			["Prompt_ListUsers_ResultHtml.Text"] = "<div>\n        <h4>Listar todos os usuários no portal atual</h4>\n        <code class=\"block\">\n            list-users\n        </code>\n\n        <h4>Pesquisar usuários por email</h4>\n        <h5>Uso implícito da opção <span class=\"mono\">--email</span></h5>\n        <p>Ao especificar um único valor após o nome do comando, se ele contiver o símbolo arroba (<span class=\"mono\"> @ </span>), o Prompt assumirá que é um email</p>\n        <code class=\"block\">list-users jsmith@sample.com</code>\n        <h5>Uso explícito da opção <span class=\"mono\">--email</span></h5>\n        <code class=\"block\">list-users --email jsmith@sample.com</code>\n        <table class=\"command-result-tbl\">\n            <tr><td>UserId</td><td>:</td><td>345</td></tr>\n            <tr><td>Username</td><td>:</td><td>jsmith</td></tr>\n            <tr><td>Email</td><td>:</td><td>jsmith@sample.com</td></tr>\n            <tr><td>DisplayName</td><td>:</td><td>John Smith</td></tr>\n            <tr><td>FirstName</td><td>:</td><td>John</td></tr>\n            <tr><td>LastName</td><td>:</td><td>Smith</td></tr>\n            <tr><td>LastLogin</td><td>:</td><td>2016-12-06T09:31:38.413-07:00</td></tr>\n        </table>\n\n        <h4>Pesquisar usuários por email usando curingas</h4>\n        <h5>Uso implícito da opção <span class=\"mono\">--email</span></h5>\n        <p>Ao especificar um único valor após o nome do comando, se ele contiver o símbolo arroba (<span class=\"mono\"> @ </span>), o Prompt assumirá que é um email</p>\n        <code class=\"block\">list-users jsmith*@sample.com</code>\n        <h5>Uso explícito da opção <span class=\"mono\">--email</span></h5>\n        <code class=\"block\">list-users --email jsmith*@sample.com</code>\n\n        <table class=\"command-result-tbl\">\n            <thead>\n                <tr>\n                    <th>UserId</th>\n                    <th>Username</th>\n                    <th>Email</th>\n                    <th>DisplayName</th>\n                    <th>FirstName</th>\n                    <th>LastName</th>\n                    <th>LastLogin</th>\n                    <th>IsAuthorized</th>\n                </tr>\n            </thead>\n            <tbody>\n                <tr>\n                    <td>345</td>\n                    <td>jsmith</td>\n                    <td>jsmith@sample.com</td>\n                    <td>John Smith</td>\n                    <td>John</td>\n                    <td>Smith</td>\n                    <td>2016-12-06T09:31:38.413-07:00</td>\n                    <td>true</td>\n                </tr>\n                <tr>\n                    <td>1095</td>\n                    <td>jsmithson</td>\n                    <td>jsmithson@sample.com</td>\n                    <td>Jerry Smithson</td>\n                    <td>Jerry</td>\n                    <td>Smithson</td>\n                    <td>2016-12-063T08:15:28.123-07:00</td>\n                    <td>true</td>\n                </tr>\n            </tbody>\n        </table>\n\n        <h4>Pesquisar usuários por nome de usuário</h4>\n        <h5>Uso implícito da opção <span class=\"mono\">--username</span></h5>\n        <p>Ao especificar um único valor após o nome do comando, se ele não for um número e não contiver o símbolo arroba (<span class=\"mono\"> @ </span>), o Prompt assumirá que é um nome de usuário</p>\n        <code class=\"block\">list-users jsmith</code>\n        <h5>Uso explícito da opção <span class=\"mono\">--username</span></h5>\n        <code class=\"block\">list-users --username jsmith</code>\n        <table class=\"command-result-tbl\">\n            <tr><td>UserId</td><td>:</td><td>345</td></tr>\n            <tr><td>Username</td><td>:</td><td>jsmith</td></tr>\n            <tr><td>Email</td><td>:</td><td>jsmith@sample.com</td></tr>\n            <tr><td>DisplayName</td><td>:</td><td>John Smith</td></tr>\n            <tr><td>FirstName</td><td>:</td><td>John</td></tr>\n            <tr><td>LastName</td><td>:</td><td>Smith</td></tr>\n            <tr><td>LastLogin</td><td>:</td><td>2016-12-06T09:31:38.413-07:00</td></tr>\n            <tr><td>IsAuthorized</td><td>:</td><td>true</td></tr>\n            <tr><td>Created</td><td>:</td><td>2016-12-06T09:31:38</td></tr>\n            <tr><td>IsDeleted</td><td>:</td><td>false</td></tr>\n        </table>\n\n        <h4>Pesquisar usuário por nome de usuário usando curingas</h4>\n        <h5>Uso implícito da opção <span class=\"mono\">--username</span></h5>\n        <p>Ao especificar um único valor após o nome do comando, se ele não for um número e não contiver o símbolo arroba (<span class=\"mono\"> @ </span>), o Prompt assumirá que é um nome de usuário</p>\n        <code class=\"block\">list-users jsmith*</code>\n        <h5>Uso explícito da opção <span class=\"mono\">--username</span></h5>\n        <code class=\"block\">list-users --username jsmith*</code>\n\n        <table class=\"command-result-tbl\">\n            <thead>\n                <tr>\n                    <th>UserId</th>\n                    <th>Username</th>\n                    <th>Email</th>\n                    <th>LastLogin</th>\n                    <th>IsDeleted</th>\n                    <th>IsAuthorized</th>\n                </tr>\n            </thead>\n            <tbody>\n                <tr>\n                    <td>345</td>\n                    <td>jsmith</td>\n                    <td>jsmith@sample.com</td>\n                    <td>2016-12-06T09:31:38.413-07:00</td>\n                    <td>false</td>\n                    <td>true</td>\n                </tr>\n                <tr>\n                    <td>1095</td>\n                    <td>jsmithson</td>\n                    <td>jsmithson@sample.com</td>\n                    <td>2016-12-063T08:15:28.123-07:00</td>\n                    <td>false</td>\n                    <td>true</td>\n                </tr>\n            </tbody>\n        </table>\n\n        <h4>Listar usuários em uma função de segurança do DNN</h4>\n        <code class=\"block\">list-users --role \"Registered Users\"</code>\n        <table class=\"command-result-tbl\">\n            <thead>\n                <tr>\n                    <th>UserId</th>\n                    <th>Username</th>\n                    <th>Email</th>\n                    <th>LastLogin</th>\n                    <th>IsDeleted</th>\n                    <th>IsAuthorized</th>\n                </tr>\n            </thead>\n            <tbody>\n                <tr>\n                    <td>2</td>\n                    <td>admin</td>\n                    <td>admin@mysite.com</td>\n                    <td>2016-12-01T06:03:10-07:00</td>\n                    <td>false</td>\n                    <td>true</td>\n                </tr>\n                <tr>\n                    <td>3</td>\n                    <td>tuser1</td>\n                    <td>tuser1@mysite.com</td>\n                    <td>2016-12-01T09:31:38.413-07:00</td>\n                    <td>false</td>\n                    <td>true</td>\n                </tr>\n                <tr>\n                    <td>31</td>\n                    <td>jsmith</td>\n                    <td>jsmith@mysite.com</td>\n                    <td>2016-12-19T12:23:39-07:00</td>\n                    <td>false</td>\n                    <td>true</td>\n                </tr>\n            </tbody>\n        </table>\n    </div>",
			["Prompt_NewUser_Description.Text"] = "Cria um novo registro de usuário no portal",
			["Prompt_NewUser_FlagApproved.Text"] = "Indica se a conta do usuário está autorizada. O padrão é 'true' se não especificado.",
			["Prompt_NewUser_FlagEmail.Text"] = "O endereço de email do usuário",
			["Prompt_NewUser_FlagFirstname.Text"] = "O nome do usuário",
			["Prompt_NewUser_FlagLastname.Text"] = "O sobrenome do usuário",
			["Prompt_NewUser_FlagNotify.Text"] = "Se verdadeiro, o email do sistema será enviado ao usuário no endereço de email dele. O tipo de email é determinado\n            pelo tipo de registro. Se isto não for informado, as configurações padrão do portal serão observadas. Se o modo de\n            registro do site estiver definido como Nenhum, nenhum email será enviado, independentemente do valor de <code>--notify</code>.",
			["Prompt_NewUser_FlagPassword.Text"] = "A senha da conta do usuário. Se não for especificada, uma senha aleatória será gerada. Observe que, a menos que você\n            esteja forçando o usuário a redefinir a senha, você deve garantir que <em>someone</em> seja notificado da senha.",
			["Prompt_NewUser_FlagUsername.Text"] = "O nome de usuário do usuário",
			["Prompt_NewUser_ResultHtml.Text"] = "<h4>Criar um novo usuário apenas com os valores obrigatórios</h4>\n    <p>Isto gera uma senha aleatória e a atribui ao usuário recém-criado. O nome de exibição é gerado pela concatenação\n      do nome e do sobrenome. Observe que, como \"van Doorne\" contém um espaço, precisamos colocá-lo entre aspas duplas. O\n      nome de exibição resultante será \"Erik van Doorne\".</p>\n    <code class=\"block\">new-user --username edoorne --email edoorne@myemail.com --firstname Erik --lastname \"van Doorne\"</code>",
			["Prompt_NoUserId.Text"] = "Nenhum ID de usuário informado. Nada a fazer.",
			["Prompt_NothingToSetUser.Text"] = "Nada a atualizar. Informe uma ou mais opções com valores para atualizar o usuário ou digite 'help set-user' para obter mais ajuda",
			["Prompt_OnlyOneFlagRequired.Text"] = "Você deve especificar uma e apenas uma opção: --{0}, --{1} ou --{2}.",
			["Prompt_PasswordReset.Text"] = "A senha do usuário foi redefinida.",
			["Prompt_ResetPassword_Description.Text"] = "Define o token de redefinição de senha para o usuário. <em>Ele não redefine automaticamente a senha do usuário</em>. Por padrão,\n    este comando enviará ao usuário o email de redefinição de senha. Você pode substituir este comportamento especificando a opção <code>--notify</code>    com o valor <code>false</code>",
			["Prompt_ResetPassword_FlagId.Text"] = "ID do usuário para o qual enviar o link de redefinição de senha.",
			["Prompt_ResetPassword_FlagNotify.Text"] = "Se verdadeiro, o email do sistema será enviado ao usuário no endereço de email dele.",
			["Prompt_ResetPassword_ResultHtml.Text"] = "<code class=\"block\">\n    reset-password <em>userId</em>\n  </code>",
			["Prompt_RestoreNotRequired.Text"] = "Este usuário não foi excluído. Nada a restaurar.",
			["Prompt_RolesEmpty.Text"] = "--roles não pode estar vazio.",
			["Prompt_SearchUserParameterRequired.Text"] = "Para pesquisar um usuário, você deve especificar --id (UserId), --email (Email do usuário) ou --name (nome de usuário).",
			["Prompt_SetUser_Description.Text"] = "Permite definir vários valores relacionados ao usuário especificado.",
			["Prompt_SetUser_FlagApproved.Text"] = "Status de aprovação do usuário.",
			["Prompt_SetUser_FlagDisplayname.Text"] = "O nome a ser exibido para o usuário.",
			["Prompt_SetUser_FlagEmail.Text"] = "O endereço de email a definir para o usuário.",
			["Prompt_SetUser_FlagFirstname.Text"] = "O nome do usuário.",
			["Prompt_SetUser_FlagId.Text"] = "O UserId do usuário que está sendo atualizado. Este valor é obrigatório, mas se você passar o valor como primeiro argumento após o\n                    nome do comando, não é necessário usar explicitamente a opção <span class=\"mono\">--id</span>. (veja os exemplos abaixo)",
			["Prompt_SetUser_FlagLastname.Text"] = "O sobrenome do usuário.",
			["Prompt_SetUser_FlagPassword.Text"] = "A nova senha a atribuir ao usuário. A senha deve ser válida de acordo com as regras de senha do site.",
			["Prompt_SetUser_FlagUsername.Text"] = "O nome de usuário a definir para o usuário.",
			["Prompt_SetUser_ResultHtml.Text"] = "<div>\n        <h4>Aprovar/Desaprovar um usuário</h4>\n        <p>\n            Aprova/desaprova o usuário com ID 345. Neste exemplo, true é passado para a opção <span class=\"mono\">--approve</span>.\n        </p>\n        <code class=\"block\">\n            set-user 345 --approve true\n        </code>\n\n        <h4>Alterar o nome de usuário de um usuário</h4>\n        <p>Altera o nome de usuário do usuário com ID 345 para <em>john.smith</em>.</p>\n        <code class=\"block\">\n            set-user 345 --username \"john.smith\"\n        </code>\n\n        <h4>Alterar as propriedades de nome de um usuário</h4>\n        <p>\n            Define o nome, o sobrenome e o nome de exibição do usuário com ID 345. Observe que <em>John</em> e <em>Smith</em>            não precisam estar entre aspas, mas <em>Johnny Smith</em> precisa, pois tem mais de uma palavra.\n        </p>\n        <code class=\"block\">\n            set-user 345 --firstname John --lastname Smith --displayname \"Johnny Smith\"\n        </code>\n\n    </div>",
			["Prompt_StartDateGreaterThanEnd.Text"] = "A data de início não pode ser posterior à data de término.",
			["Prompt_UserAlreadyDeleted.Text"] = "O usuário já está excluído. Deseja excluir permanentemente? Use \\\"purge-user\\\"",
			["Prompt_UserDeletionFailed.Text"] = "O usuário foi encontrado, mas o sistema não consegue excluí-lo.",
			["Prompt_UserIdIsRequired.Text"] = "Você deve especificar um ID de usuário válido como primeiro argumento ou usar a opção --id.",
			["Prompt_UserPurged.Text"] = "O usuário foi removido permanentemente do site.",
			["Prompt_UsersCategory.Text"] = "Comandos de usuário",
			["Question.Help"] = "Informe uma pergunta de senha a ser usada ao recuperar a senha.",
			["Question.Required"] = "Você deve informar uma pergunta de senha.",
			["Question.Text"] = "Pergunta de senha:",
			["Random.Help"] = "Marque esta caixa para gerar uma senha aleatória.",
			["Random.Text"] = "Senha aleatória",
			["RegisterationFailed.Text"] = "O registro falhou. Tente novamente mais tarde.",
			["RegisteredUsers.Text"] = "Usuários registrados",
			["RemoveDeleted.Btn"] = "Remover usuários excluídos",
			["RemoveDeleted.Confirm"] = "Deseja remover permanentemente os usuários excluídos? Esta operação não pode ser desfeita.",
			["RemoveDeleted.Done"] = "Todos os usuários excluídos foram removidos",
			["RemoveUser.Confirm"] = "Tem certeza de que deseja remover este usuário permanentemente?",
			["RemoveUser.Text"] = "Remover usuário permanentemente",
			["RequestedRemoval.Text"] = "Remoção solicitada",
			["RequestsRemoval.title"] = "O usuário solicitou a remoção do site",
			["ResetPassword.Text"] = "Enviar link de redefinição de senha",
			["RestoreUser.Text"] = "Restaurar usuário",
			["Role.Header"] = "Função",
			["RoleIsNotApproved.Text"] = "Não é possível atribuir uma função que não está aprovada.",
			["Roles.Title"] = "Funções",
			["SearchPlaceHolder.Text"] = "Pesquisar usuários",
			["SendEmail.Text"] = "Enviar email",
			["ShowLabel.Text"] = "Mostrar: ",
			["Start.Header"] = "Início",
			["SuperUsers.Text"] = "Superusuários",
			["SwitchOff.Text"] = "Desativado",
			["SwitchOn.Text"] = "Ativado",
			["True.Text"] = "Sim",
			["UnAuthorized.Text"] = "Não autorizado",
			["UpdatePassword.Help"] = "Indica se este usuário é forçado a atualizar a senha.",
			["UpdatePassword.Text"] = "Atualizar senha:",
			["UserAuthorized.Text"] = "Usuário autorizado com sucesso.",
			["UserCreated.Text"] = "Usuário criado com sucesso.",
			["UserDeleteError.Text"] = "Falha ao excluir o usuário.",
			["UserDeleted.Text"] = "Usuário excluído com sucesso.",
			["UserFolder.Help"] = "A pasta que armazena os arquivos deste usuário.",
			["UserFolder.Text"] = "Pasta do usuário:",
			["UserId.Help"] = "O identificador exclusivo do usuário.",
			["UserId.Text"] = "ID do usuário:",
			["UserNotFound.Text"] = "Usuário não encontrado.",
			["UserPasswordUpdateChanged.Text"] = "O usuário deve atualizar a senha no próximo login.",
			["UserRemoveError.Text"] = "Não é possível remover o usuário.",
			["UserRestoreError.Text"] = "Não é possível restaurar o usuário.",
			["UserRestored.Text"] = "Usuário restaurado com sucesso",
			["UserUnAuthorized.Text"] = "Usuário desautorizado com sucesso.",
			["UserUnLocked.Text"] = "Usuário desbloqueado com sucesso.",
			["UserUnlockError.Text"] = "Falha ao desbloquear o usuário.",
			["UserUpdated.Text"] = "Usuário atualizado com sucesso.",
			["Username.Help"] = "Informe um nome de usuário. Ele deve ter pelo menos o comprimento mínimo configurado e deve ser um valor alfanumérico.",
			["Username.RegExError"] = "O nome de usuário informado é inválido.",
			["Username.Required"] = "O nome de usuário é obrigatório.",
			["Username.Text"] = "Nome de usuário:",
			["UsernameNotUnique.Text"] = "O nome de usuário deve ser único.",
			["ViewProfile.Text"] = "Ver perfil",
			["btnApply.Text"] = "Aplicar",
			["btnCancel.Text"] = "Cancelar",
			["btnCreate.Text"] = "Criar",
			["btnCreateUser.Text"] = "Adicionar usuário",
			["btnSave.Text"] = "Salvar",
			["cmUnlockUser.Text"] = "Desbloquear usuário",
			["cmdAuthorize.Text"] = "Autorizar usuário",
			["cmdPassword.Text"] = "Forçar alteração de senha",
			["cmdUnAuthorize.Text"] = "Desautorizar usuário",
			["cmdUnLock.Text"] = "Desbloquear conta",
			["nav_Users.Text"] = "Usuários",
			["noUsers.Text"] = "Nenhum usuário encontrado.",
			["rolesPageInfoText.Text"] = "Página {0} de {1}",
			["rolesSummaryText.Text"] = "Exibindo {0}-{1} de {2}",
			["usersPageSizeOptionText.Text"] = "{0} usuários por página",
			["usersSummaryText.Text"] = "Exibindo {0}-{1} de {2}",
		};

		[Fact]
		public void Pins_Exact_Values()
		{
			Pins.Should().HaveCount(233, "les 233 clés sont épinglées : la grille de marqueurs et l'épingle couvrent le même périmètre");
			foreach (var (key, expected) in Pins)
				ResxValue(PtResx, key).Should().Be(expected, "pin du sweep pt-BR — clé {0}.", key);
		}

		[Fact]
		public void Placeholder_Parity_With_Neutral()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			var token = new Regex(@"\{\d+\}");
			foreach (var key in ne.Keys)
			{
				var expected = token.Matches(ne[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal).ToArray();
				var actual = token.Matches(pt[key]).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal).ToArray();
				actual.Should().Equal(expected, "placeholders de {0} : le neutre porte {1}", key, string.Join(",", expected));
			}
		}

		/// <summary>Les 17 cellules HTML du neutre et leurs jumelles pt-BR portent la
		/// MÊME séquence de balises : c'est une traduction, pas une réécriture.</summary>
		[Fact]
		public void Html_Skeleton_Matches_Neutral()
		{
			var ne = Cells(NeutralResx);
			var pt = Cells(PtResx);
			var keys = ne.Where(c => c.Value.Contains('<')).Select(c => c.Key).OrderBy(k => k, StringComparer.Ordinal).ToArray();
			keys.Should().HaveCount(17, "17 cellules du neutre portent du balisage — témoin du périmètre");
			Func<string, string[]> tags = html =>
				Regex.Matches(html, @"</?[a-zA-Z][^>]*>").Select(m => m.Value.ToLowerInvariant()).ToArray();
			foreach (var key in keys)
				tags(pt[key]).Should().Equal(tags(ne[key]),
					"la séquence de balises de {0} doit être celle du neutre (traduction, pas réécriture)", key);
			ResxValue(PtResx, "Prompt_GetUser_ResultHtml.Text").Should().Contain("Obter o usuário atual",
				"le titre est traduit, mais la structure de la table de résultats reste celle du neutre");
		}

		[Fact]
		public void Values_Are_Text_Not_Nested_Xml()
		{
			var offenders = Load(PtResx).Root!.Elements("data")
				.Where(d => d.Element("value") != null && d.Element("value")!.HasElements)
				.Select(d => d.Attribute("name")!.Value)
				.ToArray();
			offenders.Should().BeEmpty(
				"aucune valeur ne doit contenir d'élément XML : les balises sont échappées (&lt;div&gt;) : {0}",
				string.Join(" | ", offenders));
		}

		[Fact]
		public void No_Space_Before_Punctuation()
		{
			var spaceBefore = new Regex(@"\s+[:;]");
			var offenders = Cells(PtResx)
				.Where(c => spaceBefore.IsMatch(c.Value))
				.Select(c => $"{c.Key} :: {c.Value}")
				.ToArray();
			offenders.Should().BeEmpty("pt-BR : aucun espace avant « : » ni « ; » : {0}", string.Join(" | ", offenders));
		}

		/// <summary>Terminologie d'interface : les mots-clés sont portugais.</summary>
		[Fact]
		public void Terminology_Is_PtBr()
		{
			var pt = Cells(PtResx);
			var joined = string.Join("\n", pt.Values);
			pt.Values.Count(v => v.Contains("usuário")).Should().BeGreaterThanOrEqualTo(40,
				"témoin du périmètre : les cellules qui nomment l'utilisateur");
			pt.Values.Count(v => v.Contains("senha")).Should().BeGreaterThanOrEqualTo(25,
				"témoin du périmètre : les cellules du mot de passe");
			joined.Should().NotContain("contrasenha").And.NotContain("contraseña").And.NotContain("Apellidos")
				.And.NotContain("Eliminar").And.NotContain("Guardar").And.NotContain("Añadir");
			joined.Should().NotMatchRegex(@"\brol\b", "« rol » est la forme espagnole (décision du sweep 6/6 : « função »)");
			pt["Role.Header"].Should().Be("Função");
			pt["Roles.Title"].Should().Be("Funções");
			pt["btnSave.Text"].Should().Be("Salvar");
			pt["Authorized.Header"].Should().Be("Status");
			pt["Delete.Text"].Should().Be("Excluir");
			pt["LastName.Text"].Should().Be("Sobrenome:");
			pt["DisplayName.Text"].Should().Be("Nome de exibição:");
			pt["UserId.Text"].Should().Be("ID do usuário:", "clé restaurée par le sweep");
			pt["ShowLabel.Text"].Should().Be("Mostrar: ", "l'espace finale du neutre fait partie de la valeur");
		}

		/// <summary>Avant le sweep, 48 cellules étaient byte-égales à l'es-ES
		/// (« Añadir », « Guardar », « Estado », « Buscar usuarios »…). Après, il n'en
		/// reste que 14 — des homographes parfaits.</summary>
		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell_Except_Legitimate_Homographs()
		{
			var pt = Cells(PtResx);
			var es = Cells(EsResx);
			var legitimate = new[] { "All.Text", "Approved.Text", "Authorized.Text", "btnApply.Text", "btnCancel.Text", "Cancel.Text", "Email.Header", "LockedOut.Text", "Never.Text", "Prompt_ResetPassword_ResultHtml.Text", "rolesPageInfoText.Text", "SendEmail.Text", "ViewProfile.Text" };
			var equal = pt.Where(c => es.TryGetValue(c.Key, out var v) && v == c.Value)
				.Select(c => c.Key).OrderBy(k => k, StringComparer.Ordinal).ToArray();
			equal.Should().Equal(legitimate.OrderBy(k => k, StringComparer.Ordinal).ToArray(),
				"seuls les homographes parfaits sont légitimement identiques à l'es-ES");
		}

		/// <summary>Transport : le fichier est UTF-8 avec BOM et à fins de ligne LF.
		/// Une réécriture en mode texte sous Windows traduirait tout en CRLF — le
		/// parseur XML normalise CRLF vers LF, donc aucune valeur ne bougerait et
		/// le défaut serait invisible.</summary>
		[Fact]
		public void File_Is_Utf8_Bom_With_Lf_Only()
		{
			var bytes = File.ReadAllBytes(Path_(PtResx));
			bytes.Take(3).Should().Equal(new byte[] { 0xEF, 0xBB, 0xBF }, "le BOM est celui du fichier d'origine");
			var crlf = 0;
			for (var i = 0; i < bytes.Length - 1; i++)
				if (bytes[i] == 0x0D && bytes[i + 1] == 0x0A)
					crlf++;
			crlf.Should().Be(0, "le fichier d'origine est en LF seul : une réécriture ne doit pas le recâbler en CRLF");
		}
	}
}
