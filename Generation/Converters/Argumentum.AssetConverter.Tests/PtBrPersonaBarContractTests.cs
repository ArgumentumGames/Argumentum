using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 D4 file 4/4 — the Persona Bar (DNN admin toolbar) pt-BR resource file was seeded
	/// from es-ES and was macaronic: 40 of its 65 cells were retranslated on 27/10, then the
	/// 4 kept cells that the Opus review marked FIX (nav_DocCenter, nav_Settings, nav_SiteAssets,
	/// permissiongrid.Display Name) in a follow-up. This contract pins every value of the file
	/// and asserts the declared hispanisms are gone.
	/// Kept in its own class so it never collides with <see cref="PtBrLoginSurfaceContractTests"/>.
	/// </summary>
	public class PtBrPersonaBarContractTests
	{
		private static readonly string PtBrFile = Path.Combine(TestRepoRoot.Find(), "DNNPlatform",
			"DesktopModules/Admin/Dnn.PersonaBar/App_LocalResources/personabar.pt-BR.resx");

		public static IEnumerable<object[]> Cells()
		{
			yield return new object[] { "btn_CloseDialog.Text", "OK" };
			yield return new object[] { "btn_LoadMore.Text", "Carregar mais" };
			yield return new object[] { "CriticalUpdate.Text", "[ Atualização crítica ]" };
			yield return new object[] { "Documentation.Text", "Documentação" };
			yield return new object[] { "err_BetweenMinMax.Text", "O valor tem que estar entre o mínimo e máximo permitidos." };
			yield return new object[] { "err_Email.Text", "Informe um endereço de e-mail válido." };
			yield return new object[] { "err_Minimum.Text", "O texto tem que ter ao menos {0} caracteres." };
			yield return new object[] { "err_MinLessThanMax.Text", "O valor mínimo tem que ser menor que o máximo." };
			yield return new object[] { "err_NoLargerThanExp.Text", "Os pontos de reputação não podem ser maiores que os pontos de experiência." };
			yield return new object[] { "err_NonDecimalNumber.Text", "Números decimais não são permitidos." };
			yield return new object[] { "err_NonNegativeNumber.Text", "Números negativos não são permitidos." };
			yield return new object[] { "err_NoUserFolder.Text", "{0} não possui nenhum recurso." };
			yield return new object[] { "err_Number.Text", "Somente números são permitidos." };
			yield return new object[] { "err_OneActionNeeded.Text", "É necessária ao menos uma ação." };
			yield return new object[] { "err_PositiveNumber.Text", "Somente números positivos são permitidos." };
			yield return new object[] { "err_Required.Text", "O texto é obrigatório." };
			yield return new object[] { "err_RoleAssignedToUser.Text", "Esta função já foi atribuída a este usuário." };
			yield return new object[] { "Framework.Text", ".Net Framework" };
			yield return new object[] { "LicenseKey.Text", "Chave de licença" };
			yield return new object[] { "LockEditMode.Help", "Quando bloqueado, o ícone aparecerá em azul e você permanecerá no modo de edição mesmo ao navegar para outras páginas." };
			yield return new object[] { "LockEditMode.Text", "Clique para bloquear o modo de edição." };
			yield return new object[] { "nav_Analytics.Text", "Análises" };
			yield return new object[] { "nav_Content.Text", "Conteúdo" };
			yield return new object[] { "nav_Dashboard.Text", "Painel" };
			yield return new object[] { "nav_DocCenter.Text", "Central de documentação" };
			yield return new object[] { "nav_Edit.Text", "Editar" };
			yield return new object[] { "nav_GlobalAssets.Text", "Recursos globais" };
			yield return new object[] { "nav_Help.Text", "Ajuda" };
			yield return new object[] { "nav_Home.Text", "Início" };
			yield return new object[] { "nav_Logout.Text", "Sair" };
			yield return new object[] { "nav_Manage.Text", "Gerenciar" };
			yield return new object[] { "nav_Settings.Text", "Configurações" };
			yield return new object[] { "nav_Site.Text", "Ver o site" };
			yield return new object[] { "nav_SiteAssets.Text", "Recursos do site" };
			yield return new object[] { "nav_Tasks.Text", "Tarefas" };
			yield return new object[] { "nav_Tools.Text", "Ferramentas" };
			yield return new object[] { "NormalUpdate.Text", "[ Nova atualização ]" };
			yield return new object[] { "permissiongrid.Actions.Text", "Ações" };
			yield return new object[] { "permissiongrid.Add Content.Text", "Adicionar conteúdo" };
			yield return new object[] { "permissiongrid.Add User.Text", "Adicionar usuário" };
			yield return new object[] { "permissiongrid.Add.Text", "Adicionar" };
			yield return new object[] { "permissiongrid.AllRoles.Text", "< Todas as funções >" };
			yield return new object[] { "permissiongrid.Copy.Text", "Copiar" };
			yield return new object[] { "permissiongrid.Delete.Text", "Excluir" };
			yield return new object[] { "permissiongrid.Display Name.Text", "Nome de exibição:" };
			yield return new object[] { "permissiongrid.Edit Content.Text", "Editar conteúdo" };
			yield return new object[] { "permissiongrid.Edit Tab.Text", "Controle total" };
			yield return new object[] { "permissiongrid.Edit.Text", "Controle total" };
			yield return new object[] { "permissiongrid.Export.Text", "Exportar" };
			yield return new object[] { "permissiongrid.Filter By Group.Text", "Filtrar por grupo" };
			yield return new object[] { "permissiongrid.GlobalRoles.Text", "< Funções globais >" };
			yield return new object[] { "permissiongrid.Import.Text", "Importar" };
			yield return new object[] { "permissiongrid.Manage Settings.Text", "Gerenciar configurações" };
			yield return new object[] { "permissiongrid.Navigate.Text", "Navegar" };
			yield return new object[] { "permissiongrid.Select Role.Text", "Selecionar função" };
			yield return new object[] { "permissiongrid.Type-roles.Text", "Funções" };
			yield return new object[] { "permissiongrid.Type-users.Text", "Usuários" };
			yield return new object[] { "permissiongrid.View Tab.Text", "Ver" };
			yield return new object[] { "permissiongrid.View.Text", "Ver" };
			yield return new object[] { "ServerName.Text", "Nome do servidor" };
			yield return new object[] { "title_Tasks.Text", "Tarefas" };
			yield return new object[] { "txt_ViewIndex.Text", "Mostrando {0} de {1}" };
			yield return new object[] { "UnlockEditMode.Help", "Quando desbloqueado, você sairá do modo de edição ao navegar para outra página ou ao concluir suas alterações." };
			yield return new object[] { "UnlockEditMode.Text", "Clique para desbloquear o modo de edição." };
			yield return new object[] { "UpgradeLicense.Text", "[ Atualizar para o Evoq ]" };
		}

		[Fact]
		public void PersonaBarPtBrHasExactlyThePinnedKeys()
		{
			var keys = XDocument.Load(PtBrFile).Root!.Elements("data")
				.Select(e => (string?)e.Attribute("name")).OrderBy(k => k, System.StringComparer.Ordinal).ToArray();
			var pinned = Cells().Select(c => (string)c[0]).OrderBy(k => k, System.StringComparer.Ordinal).ToArray();
			Assert.Equal(pinned, keys);
		}

		[Theory]
		[MemberData(nameof(Cells))]
		public void PersonaBarPtBrHasItsReviewedValues(string key, string expected)
		{
			Assert.True(File.Exists(PtBrFile), $"missing culture file: {PtBrFile}");
			var data = XDocument.Load(PtBrFile).Root!.Elements("data")
				.Where(e => (string?)e.Attribute("name") == key).ToArray();
			Assert.True(data.Length == 1, $"pt-BR:{key} must occur exactly once");
			Assert.Equal(expected, (string?)data[0].Element("value"));
		}

		[Fact]
		public void PersonaBarPtBrCarriesNoDeclaredHispanisms()
		{
			// Bounded to this file: the es-ES seed and the macaronic constructs measured on 27/10,
			// each one live in the pre-fix file. Case-sensitive on purpose — « usuário », « Sair »,
			// « Funções », « Atualização » are the pt-BR words and never match.
			var text = File.ReadAllText(PtBrFile);
			foreach (var hispanism in new[]
			{
				"Salir", "Ayuda", "Herramientas", "Tablón", "Gestionar", "Añadir", "Usuarios", "usuario",
				"Tareas", "Cargar", "Aceptar", "Clave", "licencia", "globales", "escribir", "posee",
				"permiten", "decimales", "requiere", "asignado", "sitio", "Acções", "Cópiar", "ctualização",
				"puntos", "mantendrá", "cambios", " rol", "Atualizar a Evoq",
			})
			{
				Assert.DoesNotContain(hispanism, text);
			}
		}
	}
}
