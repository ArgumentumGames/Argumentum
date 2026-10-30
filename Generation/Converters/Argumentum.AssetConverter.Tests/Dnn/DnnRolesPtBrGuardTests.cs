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
	/// Garde du sweep pt-BR (pool #458), fichier 6/6 :
	/// <c>DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Roles/App_LocalResources/roles.pt-BR.resx</c>
	/// — le module « Funções » du PersonaBar (création, édition, groupes).
	/// <para>
	/// Le fichier est retraduit EN ENTIER (108 des 123 valeurs existantes),
	/// et <b>3 clés absentes du neutre sont restaurées</b> : le fichier pt-BR
	/// n'en portait que 123 là où le neutre en a 126 — <c>Actions.Header</c>,
	/// <c>Prompt_RoleFound.Text</c> et <c>RoleUpdated.SystemRoleError</c>
	/// n'existaient pas du tout. <b>Deux des trois s'affichaient donc en
	/// anglais</b> (<c>Prompt_RoleFound</c>, <c>RoleUpdated.SystemRoleError</c>) ;
	/// la troisième, <c>Actions.Header</c>, porte une valeur <b>vide au neutre
	/// comme en es-ES</b> — sa restauration achète la <b>parité de clés</b>, pas
	/// un affichage. Une clé absente reste une clé absente, même quand sa valeur
	/// est vide.
	/// </para>
	/// <para>
	/// Terminologie : <b>« função » pour Role</b> — et non « rol ». Le corpus
	/// porte 133 « rol », mais ces occurrences vivent dans des phrases
	/// espagnoles (« Entre um rol válido », « Actualize a página e inténtelo ») :
	/// ce n'est pas un précédent, c'est la contamination elle-même. Les
	/// occurrences en phrase PROPRE disent « função de segurança »
	/// (<c>GlobalResources.pt-BR.resx</c>, EMAIL_ROLE_*_BODY) et
	/// « Selecionar função » (<c>personabar.pt-BR.resx</c>). « Excluir » pour
	/// delete et « Usuários » accentué prolongent les sweeps 1→5.
	/// </para>
	/// <para>
	/// Défaut NON linguistique trouvé en chemin : <c>Prompt_ListRoles_ResultHtml</c>
	/// portait « Translator (<b>em</b>-US) » — le code de locale avait été
	/// corrompu (le neutre dit <c>en-US</c>). Corrigé, et épinglé.
	/// </para>
	/// </summary>
	public class DnnRolesPtBrGuardTests
	{
		private const string PtResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Roles/App_LocalResources/roles.pt-BR.resx";
		private const string EsResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Roles/App_LocalResources/roles.es-ES.resx";
		private const string NeutralResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Roles/App_LocalResources/Roles.resx";

		private static XDocument Load(string resxRelativePath)
		{
			var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
			File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");
			return XDocument.Load(path);
		}

		private static Dictionary<string, string> Cells(string resxRelativePath) =>
			Load(resxRelativePath).Root!.Elements("data")
				.ToDictionary(d => d.Attribute("name")!.Value, d => d.Element("value")?.Value ?? "");

		private static string ResxValue(string resxRelativePath, string key) => Cells(resxRelativePath)[key];

		// ------------------------------------------------------------------
		// Grille de marqueurs espagnols, calibrée SUR CE FICHIER : chaque entrée
		// est auto-contrôlée contre l'es-ES du même module (sinon elle lit 0
		// sur un zéro qu'elle n'aurait pas su voir).
		// Exclusions volontaires, mesurées ici et NON devinées :
		//   « todos »  — mot portugais parfaitement valide (« todos os grupos ») ;
		//   « conta »  — « conta de usuário » est du portugais ;
		//   « esta »   — démonstratif portugais (« esta função ») ;
		//   « número » — identique dans les deux langues ;
		//   « roles »  — noms de commandes (`list-roles`, `set-role`).
		// ------------------------------------------------------------------
		private static readonly Regex[] SpanishMarkers =
		{
			new(@"[¿¡]"),
			new(@"\w*(ción|siones)\b"),
			new(@"\busuarios?\b", RegexOptions.IgnoreCase),
			new(@"\busuario\b", RegexOptions.IgnoreCase),
			new(@"\bIntroduzca\b", RegexOptions.IgnoreCase),
			new(@"\bsitio\b", RegexOptions.IgnoreCase),
			new(@"\bpuede\b", RegexOptions.IgnoreCase),
			new(@"\btiene\b", RegexOptions.IgnoreCase),
			new(@"\bpermisos?\b", RegexOptions.IgnoreCase),
			new(@"\bnombre\b", RegexOptions.IgnoreCase),
			new(@"\bseguridad\b", RegexOptions.IgnoreCase),
			new(@"\bguardar\b", RegexOptions.IgnoreCase),
			new(@"\beliminar\b", RegexOptions.IgnoreCase),
			new(@"\bañadir\b", RegexOptions.IgnoreCase),
			new(@"\bdel\b", RegexOptions.IgnoreCase),
			new(@"\blos\b", RegexOptions.IgnoreCase),
			new(@"\blas\b", RegexOptions.IgnoreCase),
			new(@"\brol\b", RegexOptions.IgnoreCase),
			new(@"\bpodr[áa]n\b", RegexOptions.IgnoreCase),
			new(@"\bAsignar\b", RegexOptions.IgnoreCase),
			new(@"\bMuestra\b", RegexOptions.IgnoreCase),
			new(@"\bObtiene\b", RegexOptions.IgnoreCase),
			new(@"\bTamaño\b", RegexOptions.IgnoreCase),
			new(@"\bEl\b|\bLa\b", RegexOptions.IgnoreCase),
			new(@"\bSu\b", RegexOptions.IgnoreCase),
		};

		[Fact]
		public void Keys_Match_The_Neutral_File()
		{
			var pt = Cells(PtResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			var ne = Cells(NeutralResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			pt.Should().Equal(ne, "le fichier pt-BR doit porter exactement les clés du neutre (aucune ajoutée, aucune retirée)");
			pt.Should().HaveCount(126);
		}

		/// <summary>Le fichier ne portait que 123 clés : trois messages du
		/// PersonaBar s'affichaient donc en anglais. Ce test est le témoin de
		/// la restauration — pas seulement du compte.</summary>
		[Fact]
		public void Previously_Absent_Keys_Are_Restored()
		{
			var restored = new[] { "Actions.Header", "Prompt_RoleFound.Text", "RoleUpdated.SystemRoleError" };
			var pt = Cells(PtResx);
			foreach (var key in restored)
				pt.Should().ContainKey(key, $"« {key} » manquait au fichier pt-BR alors que le neutre le porte");
			ResxValue(PtResx, "Prompt_RoleFound.Text").Should().Contain("{0}");
			ResxValue(PtResx, "RoleUpdated.SystemRoleError").Should().NotBeNullOrWhiteSpace();
			ResxValue(PtResx, "Actions.Header").Should().BeEmpty("le neutre et l'es-ES portent tous deux une valeur vide");
		}

		[Fact]
		public void Marker_Grid_Is_Non_Vacuous_On_Es()
		{
			var esCells = Cells(EsResx).Values.ToArray();
			var blind = SpanishMarkers
				.Where(m => !esCells.Any(v => m.IsMatch(v)))
				.Select(m => m.ToString())
				.ToArray();
			blind.Should().BeEmpty("chaque marqueur doit être vu par l'es-ES (sinon son 0 sur le pt-BR est aveugle)");
		}

		/// <summary>Invariant de FICHIER ENTIER : les 126 cellules sont en
		/// portugais, donc aucun marqueur espagnol ne subsiste nulle part.</summary>
		[Fact]
		public void Every_Cell_Carries_No_Spanish_Marker()
		{
			var offenders = new List<string>();
			foreach (var (key, value) in Cells(PtResx))
			foreach (var marker in SpanishMarkers)
				if (marker.IsMatch(value))
					offenders.Add($"{key} [{marker}] :: {value}");
			offenders.Should().BeEmpty(
				"les 126 cellules pt-BR de « Funções » sont en portugais — tout marqueur restant est une réinjection : {0}",
				string.Join(" | ", offenders));
		}

		/// <summary>Formes macaroniques réellement présentes avant le sweep :
		/// présence prouvée dans le littéral historique, absence exigée du
		/// fichier courant.</summary>
		[Fact]
		public void Historical_Macaronic_Forms_Are_Gone()
		{
			var historical = new Dictionary<string, string>
			{
				["se asignará automáticamente"] = "o rol se asignará automáticamente a todos os usuarios do portal",
				["podrán suscribirse"] = "Os usuarios podrán suscribirse ao rol entrando este código em a página de gestión de servicios de su conta.",
				["Introduzca a descripção"] = "Introduzca a descripção do rol.",
				["propriedadees"] = "Somente se actualizarán as propriedadees que entre não comando.",
				["foi producido um error"] = "foi producido um error ao eliminar o rol. Vuelva a intentarlo.",
				["Tamaño de página"] = "Tamaño de página. O valor máximo é 500.",
				["primer argumento"] = "Tem que indicar o ID do rol como primer argumento o utilizar a opção --id.",
				["si inclui o ID"] = "O nome da opção <span class=\"mono\">--id</span> é opcional si inclui o ID como primer argumento do comando.",
				["Obtiene um rol"] = "<h4>Obtiene um rol de seguridade de DNN</h4>",
				["Muestra a lista de roles"] = "Muestra a lista de roles de seguridade do portal.",
				["em este grupo"] = "Não há nenhum rol em este grupo.",
				["Não há nenhum usuario em este rol"] = "Não há nenhum usuario em este rol.",
				["Asignar a usuarios"] = "Asignar a usuarios",
				["Descripção"] = "Descripção",
				["Asignação automática"] = "Asignação automática",
				["Translator (em-US)"] = "<td>Translator (em-US)</td>",
			};
			var current = string.Join("\n", Cells(PtResx).Values);
			foreach (var (form, witness) in historical)
			{
				witness.Should().Contain(form, "témoin littéral : la forme historique doit être présente dans le littéral");
				current.Should().NotContain(form,
					$"« {form} » est une forme retirée — sa réapparition est une régression");
			}
		}

		private static readonly Dictionary<string, string> Pins = new()
		{
			["ActionCancelled.Message"] = "Cancelado.",
			["Actions.Header"] = "",
			["Add.Text"] = "Adicionar",
			["AddUserPlaceHolder.Text"] = "Comece a digitar para adicionar um usuário a esta função",
			["AllGroups.Text"] = "[Todos os grupos]",
			["Approved.Text"] = "Aprovado",
			["AssignToExistUsers.Help"] = "Atribuir esta função a todos os usuários existentes.",
			["AssignToExistUsers.Text"] = "Atribuir aos usuários existentes",
			["Auto.Header"] = "Auto",
			["AutoAssignment.Help"] = "Marque esta opção se os usuários forem atribuídos automaticamente a esta função.",
			["AutoAssignment.Text"] = "Atribuição automática",
			["Both.Text"] = "Ambos",
			["Cancel.Text"] = "Cancelar",
			["CannotAssginUserToUnApprovedRole.Text"] = "Não é possível atribuir um usuário a uma função não aprovada.",
			["Create.Text"] = "Criar nova função",
			["Delete.Text"] = "Excluir",
			["DeleteInconsistency.Error"] = "Ocorreu uma inconsistência. Atualize a página e tente novamente.",
			["DeleteRole.Confirm"] = "Tem certeza de que deseja excluir esta função?",
			["DeleteRole.Error"] = "Falha ao excluir a função. Tente novamente mais tarde.",
			["DeleteRole.Message"] = "Função excluída com sucesso.",
			["DeleteRoleGroup.Confirm"] = "Tem certeza de que deseja excluir este grupo de funções?",
			["DeleteRoleGroup.Error"] = "Falha ao excluir o grupo de funções. Tente novamente mais tarde.",
			["DeleteRoleGroup.Message"] = "Grupo de funções excluído com sucesso.",
			["DeleteUser.Confirm"] = "Tem certeza de que deseja remover este usuário da função?",
			["Description.Help"] = "Informe uma descrição da função.",
			["Description.Text"] = "Descrição",
			["Disabled.Text"] = "Desativado",
			["DuplicateRole.Text"] = "Já existe uma função com esse nome.",
			["DuplicateRoleGroup.Text"] = "O grupo de funções já existe.",
			["EditRole.Text"] = "Editar função",
			["Expires.Header"] = "Expira",
			["GlobalRolesGroup.Text"] = "[Funções globais]",
			["GroupDescription.Help"] = "Informe uma descrição do grupo de funções.",
			["GroupDescription.Text"] = "Descrição",
			["GroupName.Header"] = "Grupo",
			["GroupName.Help"] = "Informe um nome para o grupo de funções.",
			["GroupName.Required"] = "Este é um campo obrigatório.",
			["GroupName.Text"] = "Nome do grupo",
			["InSufficientPermissions.Text"] = "Você não tem permissões suficientes para executar esta ação.",
			["InvalidRequest.Text"] = "Solicitação inválida.",
			["LoadMore.Text"] = "Carregar mais",
			["Members.Header"] = "Membros",
			["NewGroup.Text"] = "Novo grupo",
			["NoData.Text"] = "Não há nenhuma função neste grupo.",
			["NoUsers.Text"] = "Não há nenhum usuário nesta função.",
			["PageInfo.Text"] = "Página {0} de {1}",
			["PageSummary.Text"] = "Exibindo {0}-{1} de {2}",
			["Pending.Text"] = "Pendente",
			["PermissionsByRole.Text"] = "Usuários na função",
			["Prompt_DeleteRole_Description.Text"] = "Exclui definitivamente a função de segurança do DNN informada. Não é possível excluir as funções de segurança internas do DNN Administrator, RegisteredUser,\n    Subscriber ou UnverifiedUser. <small><strong class=\"text-danger\">AVISO: esta é uma ação permanente e não pode ser desfeita</strong></small>",
			["Prompt_DeleteRole_FlagId.Text"] = "O ID da função de segurança a ser excluída. Este valor é obrigatório. No entanto, se você informar o id como primeiro argumento após o comando, não precisa usar explicitamente a opção <span class=\"mono\">--id</span>.",
			["Prompt_DeleteRole_ResultHtml.Text"] = "<h4>Excluir definitivamente uma função de segurança do DNN</h4>\n  <code class=\"block\">\n    delete-role 11\n  </code>\n\n  <h4>Resultados</h4>\n  <table class=\"command-result-tbl\">\n    <tr>\n      <td>Função 'Public' (11) excluída com sucesso</td>\n    </tr>\n  </table>",
			["Prompt_FlagEmpty.Text"] = "--{0} não pode ficar vazio.",
			["Prompt_GetRole_Description.Text"] = "Recupera os detalhes de uma função de segurança do DNN.",
			["Prompt_GetRole_FlagId.Text"] = "O ID da função de segurança. Este valor é obrigatório. No entanto, se você informar o id como primeiro argumento após o comando, não precisa usar explicitamente a opção <span class=\"mono\">--id</span>.",
			["Prompt_GetRole_ResultHtml.Text"] = "<h4>Obter uma função de segurança do DNN</h4>\n  <code class=\"block\">\n    get-role 11\n  </code>\n\n  <h4>Resultados</h4>\n  <table class=\"command-result-tbl\">\n    <tr>\n      <td>RoleId</td><td>:</td><td>11</td>\n    </tr>\n    <tr>\n      <td>RoleGroupId</td><td>:</td><td>-1</td>\n    </tr>\n    <tr>\n      <td>RoleName</td>\n      <td>:</td>\n      <td>Public</td>\n    </tr>\n    <tr>\n      <td>Description</td>\n      <td>:</td>\n      <td>Role for all users</td>\n    </tr>\n    <tr>\n      <td>IsPublic</td>\n      <td>:</td>\n      <td>true</td>\n    </tr>\n    <tr>\n      <td>AutoAssign</td>\n      <td>:</td>\n      <td>true</td>\n    </tr>\n    <tr>\n      <td>UserCount</td>\n      <td>:</td>\n      <td>5</td>\n    </tr>\n    <tr>\n      <td>CreatedDate</td>\n      <td>:</td>\n      <td>2016-12-31T14:53:44.033</td>\n    </tr>\n    <tr>\n      <td>CreatedBy</td>\n      <td>:</td>\n      <td>1</td>\n    </tr>\n    <tr>\n      <td>ModifiedDate</td>\n      <td>:</td>\n      <td>2017-01-02T08:07:39.233</td>\n    </tr>\n    <tr>\n      <td>ModifiedBy</td>\n      <td>:</td>\n      <td>1</td>\n    </tr>\n    <tr>\n      <td colspan=\"3\">1 função encontrada</td>\n    </tr>\n  </table>",
			["Prompt_InvalidRoleStatus.Text"] = "Valor inválido para --{0}. Esperado 'pending', 'approved' ou 'disabled'",
			["Prompt_ListRolesFailed.Text"] = "Falha ao listar as funções.",
			["Prompt_ListRoles_Description.Text"] = "Recupera uma lista das funções de segurança do DNN do portal.",
			["Prompt_ListRoles_FlagMax.Text"] = "Tamanho da página. O máximo é 500.",
			["Prompt_ListRoles_FlagPage.Text"] = "Número da página para exibir os registros.",
			["Prompt_ListRoles_ResultHtml.Text"] = "<div>\n        <h4>Obter informações do portal atual</h4>\n        <code class=\"block\">\n            list-roles\n        </code>\n        <h4>Resultados</h4>\n        <table class=\"command-result-tbl\">\n            <thead>\n                <tr>\n                    <th>RoleId</th>\n                    <th>RoleGroupId</th>\n                    <th>RoleName</th>\n                    <th>Description</th>\n                    <th>IsPublic</th>\n                    <th>AutoAssign</th>\n                    <th>UserCount</th>\n                    <th>CreatedDate</th>\n                </tr>\n            </thead>\n            <tbody>\n                <tr>\n                    <td>0</td>\n                    <td>-1</td>\n                    <td>Administrators</td>\n                    <td>Administrators of this Website</td>\n                    <td>false</td>\n                    <td>false</td>\n                    <td>1</td>\n                    <td>2016-12-01T06:03:11.35</td>\n                </tr>\n                <tr>\n                    <td>5</td>\n                    <td>-1</td>\n                    <td>My New Role</td>\n                    <td>A test role</td>\n                    <td>false</td>\n                    <td>false</td>\n                    <td>0</td>\n                    <td>2016-12-15T07:28:16.49</td>\n                </tr>\n                <tr>\n                    <td>1</td>\n                    <td>-1</td>\n                    <td>Registered Users</td>\n                    <td>Registered Users</td>\n                    <td>false</td>\n                    <td>true</td>\n                    <td>5</td>\n                    <td>2016-12-01T06:03:11.357</td>\n                </tr>\n                <tr>\n                    <td>2</td>\n                    <td>-1</td>\n                    <td>Subscribers</td>\n                    <td>A public role for site subscriptions</td>\n                    <td>true</td>\n                    <td>true</td>\n                    <td>5</td>\n                    <td>2016-12-01T06:03:11.39</td>\n                </tr>\n                <tr>\n                    <td>3</td>\n                    <td>-1</td>\n                    <td>Translator (en-US)</td>\n                    <td>A role for English (United States) translators</td>\n                    <td>false</td>\n                    <td>false</td>\n                    <td>0</td>\n                    <td>2016-12-01T06:03:11.39</td>\n                </tr>\n                <tr>\n                    <td>4</td>\n                    <td>-1</td>\n                    <td>Unverified Users</td>\n                    <td>Unverified Users</td>\n                    <td>false</td>\n                    <td>false</td>\n                    <td>0</td>\n                    <td>2016-12-01T06:03:11.393</td>\n                </tr>\n            </tbody>\n        </table>\n    </div>",
			["Prompt_NewRole_Description.Text"] = "Cria uma nova função de segurança do DNN para o portal.",
			["Prompt_NewRole_FlagAutoAssign.Text"] = "Quando verdadeiro, esta função será atribuída automaticamente aos usuários do site, incluindo os usuários existentes.",
			["Prompt_NewRole_FlagDescription.Text"] = "Uma descrição da função.",
			["Prompt_NewRole_FlagIsPublic.Text"] = "Quando verdadeiro, os usuários poderão ver a função e atribuir a si mesmos a ela.",
			["Prompt_NewRole_FlagRoleName.Text"] = "O nome da função de segurança. Este valor é obrigatório. No entanto, se você informar o nome como primeiro argumento após o comando, não precisa usar explicitamente a opção <span class=\"mono\">--name</span>.",
			["Prompt_NewRole_FlagStatus.Text"] = "Status da função. Os valores possíveis são \"approved\", \"pending\" e \"disabled\".",
			["Prompt_NewRole_ResultHtml.Text"] = "<h4>Criar uma nova função de segurança do DNN (sintaxe mínima)</h4>\n  <code class=\"block\">\n    new-role Role1\n  </code>\n  <h4>Resultados</h4>\n  <table class=\"command-result-tbl\">\n    <tr>\n      <td>RoleId</td><td>:</td><td>9</td>\n    </tr>\n    <tr>\n      <td>RoleGroupId</td><td>:</td><td>-1</td>\n    </tr>\n    <tr>\n      <td>RoleName</td>\n      <td>:</td>\n      <td>Role1</td>\n    </tr>\n    <tr>\n      <td>Description</td>\n      <td>:</td>\n      <td></td>\n    </tr>\n    <tr>\n      <td>IsPublic</td>\n      <td>:</td>\n      <td>false</td>\n    </tr>\n    <tr>\n      <td>AutoAssign</td>\n      <td>:</td>\n      <td>false</td>\n    </tr>\n    <tr>\n      <td>UserCount</td>\n      <td>:</td>\n      <td>0</td>\n    </tr>\n    <tr>\n      <td>CreatedDate</td>\n      <td>:</td>\n      <td>2016-12-31T14:53:44.033</td>\n    </tr>\n    <tr>\n      <td colspan=\"3\">Função criada com sucesso.</td>\n    </tr>\n  </table>\n\n\n  <h4>Criar uma nova função de segurança do DNN</h4>\n  <code class=\"block\">\n    new-role \"General Public\" --description \"Role for all users\" --public true --autoassign true\n  </code>\n  <h4>Resultados</h4>\n  <table class=\"command-result-tbl\">\n    <tr>\n      <td>RoleId</td><td>:</td><td>10</td>\n    </tr>\n    <tr>\n      <td>RoleGroupId</td><td>:</td><td>-1</td>\n    </tr>\n    <tr>\n      <td>RoleName</td>\n      <td>:</td>\n      <td>General Public</td>\n    </tr>\n    <tr>\n      <td>Description</td>\n      <td>:</td>\n      <td>Role for all users</td>\n    </tr>\n    <tr>\n      <td>IsPublic</td>\n      <td>:</td>\n      <td>true</td>\n    </tr>\n    <tr>\n      <td>AutoAssign</td>\n      <td>:</td>\n      <td>true</td>\n    </tr>\n    <tr>\n      <td>UserCount</td>\n      <td>:</td>\n      <td>5</td>\n    </tr>\n    <tr>\n      <td>CreatedDate</td>\n      <td>:</td>\n      <td>2016-12-31T15:06:02.563</td>\n    </tr>\n    <tr>\n      <td colspan=\"3\">Função criada com sucesso.</td>\n    </tr>\n  </table>",
			["Prompt_NoRoleWithId.Text"] = "Nenhuma função encontrada com o ID {0}.",
			["Prompt_NoRoles.Text"] = "Nenhuma função encontrada.",
			["Prompt_NothingToUpdate.Text"] = "Nada a atualizar!",
			["Prompt_RoleFound.Text"] = "A função '{0}' foi encontrada.",
			["Prompt_RoleIdIsRequired.Text"] = "Você deve informar um ID de função válido como primeiro argumento ou usar a opção --id.",
			["Prompt_RoleIdNegative.Text"] = "O valor de RoleId deve ser maior ou igual a zero (0)",
			["Prompt_RoleIdNotInt.Text"] = "O RoleId deve ser um número inteiro.",
			["Prompt_RoleNameRequired.Text"] = "Você deve informar um nome para a função como primeiro argumento ou usar a opção --{0}. Nomes com espaços e caracteres especiais devem ser colocados entre aspas duplas.",
			["Prompt_RolesCategory.Text"] = "Comandos de função",
			["Prompt_SetRole_Description.Text"] = "Define ou atualiza propriedades de uma função de segurança do DNN. Somente as propriedades informadas serão atualizadas na função.",
			["Prompt_SetRole_FlagAutoAssign.Text"] = "Quando verdadeiro, esta função será atribuída automaticamente aos usuários do site, incluindo os usuários existentes.",
			["Prompt_SetRole_FlagDescription.Text"] = "Uma descrição da função.",
			["Prompt_SetRole_FlagId.Text"] = "O ID da função de segurança. Este valor é obrigatório. No entanto, se você informar o id como primeiro argumento após o comando, não precisa usar explicitamente a opção <span class=\"mono\">--id</span>.",
			["Prompt_SetRole_FlagIsPublic.Text"] = "Quando verdadeiro, os usuários poderão ver a função e atribuir a si mesmos a ela.",
			["Prompt_SetRole_FlagRoleName.Text"] = "O nome da função de segurança.",
			["Prompt_SetRole_FlagStatus.Text"] = "Status da função. Os valores possíveis são \"approved\", \"pending\" e \"disabled\".",
			["Prompt_SetRole_ResultHtml.Text"] = "<h4>Atualizar uma função de segurança do DNN</h4>\n  <code class=\"block\">\n    set-role 10 --name Public\n  </code>\n\n  <h4>Resultados</h4>\n  <table class=\"command-result-tbl\">\n    <tr>\n      <td>RoleId</td><td>:</td><td>10</td>\n    </tr>\n    <tr>\n      <td>RoleGroupId</td><td>:</td><td>-1</td>\n    </tr>\n    <tr>\n      <td>RoleName</td>\n      <td>:</td>\n      <td>Public</td>\n    </tr>\n    <tr>\n      <td>Description</td>\n      <td>:</td>\n      <td>Role for all users</td>\n    </tr>\n    <tr>\n      <td>IsPublic</td>\n      <td>:</td>\n      <td>true</td>\n    </tr>\n    <tr>\n      <td>AutoAssign</td>\n      <td>:</td>\n      <td>true</td>\n    </tr>\n    <tr>\n      <td>UserCount</td>\n      <td>:</td>\n      <td>5</td>\n    </tr>\n    <tr>\n      <td>CreatedDate</td>\n      <td>:</td>\n      <td>2016-12-31T14:53:44.033</td>\n    </tr>\n    <tr>\n      <td colspan=\"3\">Função criada com sucesso.</td>\n    </tr>\n  </table>",
			["Prompt_UnableToParseBool.Text"] = "Não foi possível processar o valor '{1}' da opção --{0}. O valor deve ser True ou False",
			["Public.Text"] = "Público",
			["PublicRole.Help"] = "Marque esta opção se os usuários puderem assinar esta função pela página Gerenciar serviços da conta de usuário.",
			["RoleAdded.Error"] = "Falha ao criar a função. Tente novamente mais tarde.",
			["RoleAdded.Message"] = "Função criada com sucesso.",
			["RoleGroupUpdated.Error"] = "Falha ao atualizar o grupo de funções. Tente novamente mais tarde.",
			["RoleGroupUpdated.Message"] = "Grupo de funções atualizado com sucesso.",
			["RoleName.Header"] = "Nome da função",
			["RoleName.Help"] = "Informe o nome da função.",
			["RoleName.Required"] = "Este é um campo obrigatório.",
			["RoleName.Text"] = "Nome da função",
			["RoleUpdated.Error"] = "Falha ao atualizar a função. Tente novamente mais tarde.",
			["RoleUpdated.Message"] = "Função atualizada com sucesso.",
			["RoleUpdated.SystemRoleError"] = "Não é possível modificar as funções do sistema.",
			["Save.Text"] = "Salvar",
			["Search.Text"] = "Pesquisar",
			["SearchPlaceHolder.Text"] = "Pesquisar funções por palavra-chave",
			["SecurityRole.Text"] = "Função de segurança",
			["SecurityRoleDeleteNotAllowed.Text"] = "As funções do sistema não podem ser excluídas.",
			["SendEmail.Text"] = "Enviar e-mail",
			["SocialGroup.Text"] = "Grupo social",
			["Start.Header"] = "Início",
			["UpdateGroup.Text"] = "Atualizar grupo",
			["UserNotFound.Text"] = "Usuário não encontrado.",
			["Users.Header"] = "Usuários",
			["Users.Text"] = "Usuários",
			["UsersInRole.Text"] = "Usuários na função",
			["isOwner.Text"] = "É proprietário",
			["lblNewGroup.Text"] = "[Novo grupo]",
			["nav_Roles.Text"] = "Funções",
			["plRSVPCode.Help"] = "Informe um código RSVP para a função. Os usuários podem assinar esta função facilmente digitando este código na página Gerenciar serviços da conta de usuário.",
			["plRSVPCode.Text"] = "Código RSVP",
			["plRSVPLink.Help"] = "Um link que permite aos usuários assinar esta função será exibido quando um código RSVP for salvo para esta função.",
			["plRSVPLink.Text"] = "Link RSVP",
			["plRoleGroups.Help"] = "Selecione o grupo de funções ao qual esta função pertence.",
			["plRoleGroups.Text"] = "Grupo de funções",
			["securityModeListLabel.Help"] = "Selecione o modo de segurança desta função/grupo.",
			["securityModeListLabel.Text"] = "Modo de segurança:",
			["statusListLabel.Help"] = "Selecione o status desta função/grupo.",
			["statusListLabel.Text"] = "Status",
		};

		[Fact]
		public void Pins_Exact_Values()
		{
			Pins.Should().HaveCount(126, "les 126 clés sont épinglées : la garde marque-taux et l'épingle couvrent le même périmètre");
			foreach (var (key, expected) in Pins)
				ResxValue(PtResx, key).Should().Be(expected, "pin du sweep pt-BR — clé {0}.", key);
		}

		/// <summary>Parité des placeholders exigée sur CHAQUE clé, dans les deux
		/// sens — c'est ce qui garantit que les trois clés restaurées portent
		/// bien leur <c>{0}</c>.</summary>
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

		/// <summary>Le squelette de balises des cellules qui portent du HTML est
		/// identique au neutre (valeur DÉCODÉE — le fichier porte les chevrons
		/// échappés).</summary>
		[Fact]
		public void Html_Skeleton_Matches_Neutral()
		{
			var ne = Cells(NeutralResx);
			var pt = Cells(PtResx);
			var keys = ne.Where(c => c.Value.Contains('<')).Select(c => c.Key).ToArray();
			keys.Should().HaveCount(10, "10 cellules du neutre portent du balisage — témoin du périmètre");
			Func<string, string[]> tags = html =>
				Regex.Matches(html, @"</?[a-zA-Z][^>]*>").Select(m => m.Value.ToLowerInvariant()).ToArray();
			foreach (var key in keys)
				tags(pt[key]).Should().Equal(tags(ne[key]),
					"la séquence de balises de {0} doit être celle du neutre (traduction, pas réécriture)", key);
		}

		/// <summary>Un <c>&lt;value&gt;</c> de resx est du TEXTE : une balise écrite
		/// brute y devient un élément XML enfant et la valeur lue par
		/// l'application s'arrête au premier chevron.</summary>
		[Fact]
		public void Values_Are_Text_Not_Nested_Xml()
		{
			var offenders = Load(PtResx).Root!.Elements("data")
				.Where(d => d.Element("value") != null && d.Element("value")!.HasElements)
				.Select(d => d.Attribute("name")!.Value)
				.ToArray();
			offenders.Should().BeEmpty(
				"aucune valeur ne doit contenir d'élément XML : les balises sont échappées (&lt;p&gt;) : {0}",
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

		/// <summary>Terminologie : « função » partout où le neutre dit « role »,
		/// et pas de résidu pt-PT (« Actualizar », « Acções », « Guardar ») ni
		/// espagnol (« rol »). Les noms de commandes (`list-roles`, `set-role`)
		/// sont hors périmètre : ce sont des identifiants, pas du texte.</summary>
		[Fact]
		public void Terminology_Funcao_Not_Rol()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			// Périmètre : les cellules de PROSE qui nomment la notion de rôle.
			// Deux exclusions, chacune mesurée : les noms de commandes
			// (`list-roles`, `set-role`…) sont retirés AVANT le test ; et les
			// cellules qui portent du balisage sont hors périmètre, parce que
			// leurs tableaux de démonstration contiennent des données anglaises
			// (« My New Role », « A test role ») qui ne se traduisent pas.
			var cmd = new Regex(@"\b(list|set|new|get|delete)-roles?\b", RegexOptions.IgnoreCase);
			var roleCells = ne
				.Where(c => !c.Value.Contains('<'))
				.Where(c => Regex.IsMatch(cmd.Replace(c.Value, " "), @"\broles?\b", RegexOptions.IgnoreCase))
				.Select(c => c.Key).ToArray();
			roleCells.Should().HaveCountGreaterThan(50, "témoin du périmètre : les cellules de prose qui nomment la notion de rôle");
			foreach (var key in roleCells)
				pt[key].Should().MatchRegex("[Ff]un[çc][ãõ]",
					"« função » est le terme pt-BR retenu pour Role ({0})", key);
			var joined = string.Join("\n", pt.Values);
			joined.Should().NotMatchRegex(@"\brol\b", "« rol » est la forme espagnole — le corpus la porte, pas le fichier livré");
			joined.Should().NotContain("Actualizar").And.NotContain("Acções").And.NotContain("Guardar");
			joined.Should().NotContain("Eliminar", "pt-BR de ce fichier : « excluir » (convention des sweeps 1→5)");
			joined.Should().Contain("Funções");
		}

		/// <summary>Avant le sweep, 35 cellules étaient byte-égales à l'es-ES
		/// (« Eliminar », « Añadir », « Usuarios », « Guardar »…). Après, il
		/// n'en reste que 10 — des homographes parfaits, corrects dans les deux
		/// langues : ce ne sont pas des non-traductions.</summary>
		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell_Except_Legitimate_Homographs()
		{
			var pt = Cells(PtResx);
			var es = Cells(EsResx);
			var legitimate = new[]
			{
				"ActionCancelled.Message", "Auto.Header", "Both.Text", "Cancel.Text",
				"Expires.Header", "GroupName.Header", "PageInfo.Text", "Public.Text",
				"SocialGroup.Text", "plRSVPCode.Text",
			};
			var equal = pt.Where(c => es.TryGetValue(c.Key, out var v) && v == c.Value)
				.Select(c => c.Key).OrderBy(k => k, StringComparer.Ordinal).ToArray();
			equal.Should().Equal(legitimate.OrderBy(k => k, StringComparer.Ordinal).ToArray(),
				"seuls les homographes parfaits sont légitimement identiques à l'es-ES");
		}
	}
}
