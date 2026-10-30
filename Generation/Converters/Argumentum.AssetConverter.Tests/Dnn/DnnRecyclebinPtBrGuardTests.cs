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
	/// Garde du sweep pt-BR (pool #458 c.6436567243 item 8), fichier 5/5 :
	/// <c>DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Recyclebin/App_LocalResources/recyclebin.pt-BR.resx</c>
	/// — le module « Corbeille » du PersonaBar (lister, restaurer et purger
	/// pages, modules et utilisateurs).
	/// <para>
	/// Le fichier est retraduit EN ENTIER (84 des 93 valeurs), pas sur un
	/// sous-ensemble : la contamination ne se limitait pas aux 35 cellules que
	/// la grille de marqueurs voyait. Deux familles échappaient au crible :
	/// les formes hispaniques non marquées (« papelera de reciclagem » = es
	/// « papelera » + pt « reciclagem », « Sí », « Usuario ») et le portugais
	/// EUROPEEN (« Acções », « seleccionados », « esta eliminada »), invisible
	/// à toute grille d'espagnismes parce qu'il n'est pas de l'espagnol.
	/// </para>
	/// <para>
	/// Terminologie : « Lixeira » (pt-BR standard pour Recycle Bin) et
	/// « Ações » (pt-BR, et non « Acções ») — <b>aucun précédent dans le corpus
	/// livré</b>, qui porte lui-même « papelera de reciclagem » et « Acções » ;
	/// choix déclaré dans la PR. « Excluir » pour delete, comme la surface D3
	/// déjà livrée (« deseja excluir este usuário »). « Usuário » accentué
	/// (D4), « página mãe » pour parent page.
	/// </para>
	/// <para>
	/// Défauts NON linguistiques trouvés en chemin : <c>ModuleNotFound</c>
	/// avait PERDU son second placeholder (« not found on page "{1}" ») ;
	/// <c>recyclebin_RestorePagesConfirm</c> demandait de… SUPPRIMER
	/// (« Confirme que quer eliminar este módulo ») au lieu de restaurer ;
	/// <c>Prompt_RestorePage_FlagId</c> parlait du « ID do módulo » là où il
	/// s'agit de la page. Les trois sont corrigés et épinglés.
	/// </para>
	/// </summary>
	public class DnnRecyclebinPtBrGuardTests
	{
		private const string PtResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Recyclebin/App_LocalResources/recyclebin.pt-BR.resx";
		private const string EsResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Recyclebin/App_LocalResources/recyclebin.es-ES.resx";
		private const string NeutralResx = "DNNPlatform/DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.Recyclebin/App_LocalResources/Recyclebin.resx";

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
		// Grille de marqueurs espagnols — formes qui ne sont PAS du portugais
		// brésilien valide. Chaque entrée est auto-contrôlée : elle doit toucher
		// au moins une cellule de l'es-ES, sinon l'instrument lit 0 sur un zéro
		// dont il n'aurait pas su voir un 1.
		// Exclusions volontaires (valides en pt, un marqueur naïf en ferait un
		// faux rouge) : perfil, conta, esta/este, que, para, con, sido,
		// eliminad/restaurad (« eliminado » et « restaurado » sont du portugais),
		// si.
		// ------------------------------------------------------------------
		private static readonly Regex[] SpanishMarkers =
		{
			new(@"[¿¡]"),
			new(@"\w*(ción|siones)\b"),
			new(@"\busuarios?\b", RegexOptions.IgnoreCase),
			new(@"\bIntroduzca\b|\bejemplo\b", RegexOptions.IgnoreCase),
			new(@"\bsitio\b", RegexOptions.IgnoreCase),
			new(@"\bhaya\b", RegexOptions.IgnoreCase),
			new(@"\bnombre\b", RegexOptions.IgnoreCase),
			new(@"\bfecha\b", RegexOptions.IgnoreCase),
			new(@"\bpuede\b", RegexOptions.IgnoreCase),
			new(@"\btiene\b", RegexOptions.IgnoreCase),
			new(@"\bpermisos?\b", RegexOptions.IgnoreCase),
			new(@"\bpapelera\b", RegexOptions.IgnoreCase),
			new(@"seleccionad", RegexOptions.IgnoreCase),
			new(@"\bprimero\b", RegexOptions.IgnoreCase),
			new(@"\bmás\b", RegexOptions.IgnoreCase),
			new(@"\bhija\b", RegexOptions.IgnoreCase),
			new(@"\bmadre\b", RegexOptions.IgnoreCase),
			new(@"recibir", RegexOptions.IgnoreCase),
			new(@"\bUtilice\b", RegexOptions.IgnoreCase),
			new(@"según", RegexOptions.IgnoreCase),
			new(@"producid", RegexOptions.IgnoreCase),
		};

		[Fact]
		public void Keys_Match_The_Neutral_File()
		{
			var pt = Cells(PtResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			var ne = Cells(NeutralResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			pt.Should().Equal(ne, "le fichier pt-BR doit porter exactement les clés du neutre (aucune ajoutée, aucune retirée)");
			pt.Should().HaveCount(93);
		}

		/// <summary>Auto-contrôle de l'instrument : chaque marqueur doit toucher
		/// au moins une cellule de l'es-ES.</summary>
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

		/// <summary>Invariant de FICHIER ENTIER : les 93 cellules sont retraduites,
		/// donc aucun marqueur espagnol ne peut subsister nulle part.</summary>
		[Fact]
		public void Every_Cell_Carries_No_Spanish_Marker()
		{
			var offenders = new List<string>();
			foreach (var (key, value) in Cells(PtResx))
			foreach (var marker in SpanishMarkers)
				if (marker.IsMatch(value))
					offenders.Add($"{key} [{marker}] :: {value}");
			offenders.Should().BeEmpty(
				"les 93 cellules pt-BR de la Corbeille sont en portugais — tout marqueur restant est une réinjection : {0}",
				string.Join(" | ", offenders));
		}

		/// <summary>Formes macaroniques ou de portugais européen que l'es-ES ne
		/// porte pas telles quelles (donc invisibles à la grille) : présence
		/// prouvée dans le littéral historique, absence exigée du fichier courant.</summary>
		[Fact]
		public void Historical_Macaronic_Forms_Are_Gone()
		{
			var historical = new Dictionary<string, string>
			{
				["apelera de reciclagem"] = "Papelera de reciclagem",
				["Acções"] = "Acções",
				["Sí"] = "Sí",
				["foi sido"] = "foi sido previamente eliminado",
				["foiyan"] = "hasta que foiyan eliminado sus páginas hijas",
				["foiya "] = "hasta que foiya restaurado sua página madre",
				["primer argumento"] = "pode incluir o ID do módulo como primer argumento",
				["em caso que"] = "em caso que existan",
				["hace falta"] = "Não hace falta recuperarlo",
				["hijas"] = "sus páginas hijas",
				["página madre"] = "sua página madre",
				["seleccionad"] = "os módulos seleccionados",
				["más explícita"] = "a forma más explícita do ejemplo",
				["según su"] = "según su ID",
				["esta eliminada"] = "A página esta eliminada",
				["recibirá"] = "recibirá um mensagem",
				["em a página"] = "em a página {1}",
				["do usuario."] = "O ID do usuario.",
				["um usuario que"] = "Recupera um usuario que",
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
			["CanNotDeleteModule.Text"] = "Você não tem permissão para excluir o módulo com ID '{0}'.",
			["CanNotDeleteTab.Text"] = "Você não tem permissão para excluir a página com ID '{0}'.",
			["ModuleNotFound.Text"] = "O módulo com ID '{0}' não foi encontrado na página '{1}'.",
			["ModuleNotSoftDeleted.Text"] = "O módulo com ID '{0}' não está na Lixeira.",
			["PageNotFound.Text"] = "A página com ID '{0}' não foi encontrada.",
			["PageNotFoundWithName.Text"] = "A página com o nome '{0}' não foi encontrada.",
			["Prompt_CannotPurgeUser.Text"] = "Não é possível excluir definitivamente um usuário que não foi excluído primeiro. Tente delete-user.",
			["Prompt_FlagNotInt.Text"] = "--{0} deve ser um número inteiro.\\n",
			["Prompt_FlagNotPositiveInt.Text"] = "--{0} deve ser maior que 0.\\n",
			["Prompt_FlagRequired.Text"] = "--{0} é obrigatório.\\n",
			["Prompt_MainParamRequired.Text"] = "O {0} é obrigatório. Use a opção --{1} ou informe o valor como primeiro argumento depois do nome do comando.\\n",
			["Prompt_ModulePurgedSuccessfully.Text"] = "O módulo com ID \"{0}\" foi excluído definitivamente.",
			["Prompt_ModuleRestoredSuccessfully.Text"] = "O módulo com ID \"{0}\" foi restaurado com sucesso.",
			["Prompt_PagePurgedSuccessfully.Text"] = "A página com ID \"{0}\" foi excluída definitivamente.",
			["Prompt_PageRestoredSuccessfully.Text"] = "A página com ID \"{0}\" e nome \"{1}\" foi restaurada com sucesso.",
			["Prompt_PurgeModule_Description.Text"] = "Exclui definitivamente um módulo. O módulo deve estar na Lixeira.",
			["Prompt_PurgeModule_FlagId.Text"] = "Especifica explicitamente o ID do módulo a ser excluído definitivamente. A opção não é obrigatória: você pode informar o ID como primeiro argumento do comando.",
			["Prompt_PurgeModule_FlagPageId.Text"] = "Especifica explicitamente o ID da página onde o módulo foi originalmente adicionado.",
			["Prompt_PurgeModule_ResultHtml.Text"] = "<h4>Excluir definitivamente um módulo específico</h4>\n        <p>O código abaixo exclui definitivamente o módulo com ID 359</p>\n        <code class=\"block\">purge-module 359 --pageid 20</code>\n\n        <h4>Resultado</h4>\n        O módulo com ID 359 foi excluído definitivamente.",
			["Prompt_PurgePage_Description.Text"] = "Exclui definitivamente uma página do portal que já havia sido excluída e enviada para a Lixeira do DNN.",
			["Prompt_PurgePage_FlagDeleteChildren.Text"] = "Especifica que, se a página tiver páginas filhas, o comando deve excluí-las todas ou exibir um erro.",
			["Prompt_PurgePage_FlagId.Text"] = "Especifica explicitamente o ID da página a ser excluída definitivamente. A opção não é obrigatória: você pode informar o ID como primeiro argumento do comando.",
			["Prompt_PurgePage_ResultHtml.Text"] = "<div>\n        <h4>Excluir definitivamente uma página pelo seu ID</h4>\n        <code class=\"block\">\n            purge-page 999\n        </code>\n        OU\n        <code class=\"block\">\n            purge-page --id 999\n        </code>\n\n        <h4>Excluir uma página e todas as suas descendentes definitivamente</h4>\n        <code class=\"block\">\n            purge-page --id 999 --deletechildren true\n        </code>\n    </div>",
			["Prompt_PurgeUser_Description.Text"] = "Exclui definitivamente o usuário especificado. O usuário <strong>precisa</strong> já estar excluído (na Lixeira). Se você executar o comando <code>get-user</code> e a propriedade <code>IsDeleted</code> não tiver o valor <code>true</code>, receberá um erro ao tentar este comando. Nesse caso, use primeiro <code>delete-user</code>.",
			["Prompt_PurgeUser_FlagId.Text"] = "O ID do usuário. Se a opção não for usada, o ID deve ser o primeiro argumento.",
			["Prompt_PurgeUser_ResultHtml.Text"] = "<h4>Excluir definitivamente um usuário</h4>\n    <p>Exclui definitivamente o usuário com ID 345. Se você executar o comando: <code>get-user 345</code> receberá uma mensagem de usuário não encontrado.</p>\n    <code class=\"block\">purge-user 345</code>\n    <p>Esta é a forma mais explícita do exemplo anterior.</p>\n    <code class=\"block\">purge-user --id 345</code>",
			["Prompt_RecylcleBinCategory.Text"] = "Comandos da Lixeira",
			["Prompt_RestoreModule_Description.Text"] = "Restaura um módulo da Lixeira do DNN.",
			["Prompt_RestoreModule_FlagId.Text"] = "Especifica explicitamente o ID do módulo a ser recuperado. A opção não é obrigatória: você pode informar o ID como primeiro argumento do comando.",
			["Prompt_RestoreModule_FlagPageId.Text"] = "O ID da página onde o módulo que você deseja restaurar estava antes da exclusão.",
			["Prompt_RestoreModule_ResultHtml.Text"] = "<h4>Restaurar um módulo da Lixeira</h4>\n        <code class=\"block\">restore-module 359 --pageid 71</code>\n\n        <h4>Resultado</h4>\n        O módulo com ID 359 foi restaurado.",
			["Prompt_RestoreNotRequired.Text"] = "O usuário não está excluído. Não é preciso restaurá-lo.",
			["Prompt_RestorePageNoParams.Text"] = "Você deve especificar o ID ou o nome da página.",
			["Prompt_RestorePage_Description.Text"] = "Restaura uma página da Lixeira do DNN.",
			["Prompt_RestorePage_FlagId.Text"] = "Especifica explicitamente o ID da página a ser restaurada. O uso da opção não é obrigatório: você pode simplesmente informar o valor do ID como primeiro argumento. Obrigatório se <span class=\"mono\">--parentid</span> e <span class=\"mono\">--name</span> não forem especificados.",
			["Prompt_RestorePage_FlagName.Text"] = "Especifica o nome (não o título) da página a ser restaurada. Pode ser combinado com <span class=\"mono\">--parentid</span> para selecionar uma página com uma página mãe específica. Obrigatório se <span class=\"mono\">--parentid</span> e <span class=\"mono\">--name</span> não forem especificados.",
			["Prompt_RestorePage_FlagParentId.Text"] = "Obrigatório se você quiser excluir uma página pelo nome e a página for filha de outra. Nesse caso, informe o ID da página mãe.",
			["Prompt_RestorePage_ResultHtml.Text"] = "<div>\n        <h4>Restaurar uma página excluída pelo seu ID</h4>\n        <code class=\"block\">\n            restore-page 999\n        </code>\n        OU\n        <code class=\"block\">\n            restore-page --id 999\n        </code>\n\n        <h4>Restaurar uma página excluída pelo seu nome</h4>\n        <code class=\"block\">\n            restore-page --name \"Page1\"\n        </code>\n\n        <h4>Restaurar uma página excluída pelo seu nome e pelo ID da página mãe</h4>\n        <code class=\"block\">\n            restore-page --name \"Page1\" --parentid 30\n        </code>\n    </div>",
			["Prompt_RestoreUser_Description.Text"] = "Recupera um usuário que foi excluído, mas não removido definitivamente.",
			["Prompt_RestoreUser_FlagId.Text"] = "O ID do usuário. Se a opção não for usada, o ID deve ser o primeiro argumento.",
			["Prompt_RestoreUser_ResultHtml.Text"] = "<h4>Recuperar um usuário excluído</h4>\n        <p>Restaura o usuário com ID 345. Se o usuário não estiver excluído, você receberá uma mensagem informando que não há nada a restaurar. Se o usuário já tiver sido removido definitivamente ou não existir, receberá uma mensagem de usuário não encontrado.</p>\n        <code class=\"block\">restore-user 345</code>\n        <p>Esta é a forma mais explícita do exemplo anterior.</p>\n        <code class=\"block\">restore-user --id 345</code>",
			["Prompt_UserPurged.Text"] = "O usuário foi removido definitivamente do site.",
			["Service_EmptyRecycleBinError.Text"] = "Alguns dos itens não foram excluídos.",
			["Service_RemoveTabError.Text"] = "Ocorreu um erro ao excluir a página: {0}",
			["Service_RemoveTabModuleError.Text"] = "Ocorreu um erro ao excluir os módulos da página: {0}",
			["Service_RemoveTabParentTabError"] = "A página {0} não pode ser excluída enquanto suas páginas filhas não forem excluídas primeiro.",
			["Service_RemoveTabWithChildError.Text"] = "A página {0} não pode ser excluída enquanto suas páginas filhas não forem excluídas primeiro.",
			["Service_RemoveUserError.Text"] = "Ocorreu um erro ao excluir o usuário: {0}",
			["Service_RestoreModuleError.Text"] = "Ocorreu um erro ao restaurar o módulo {0} na página {1}. A página está excluída.",
			["Service_RestoreTabError.Text"] = "A página {0} não pode ser restaurada enquanto sua página mãe não for restaurada primeiro.",
			["Service_RestoreTabModuleError.Text"] = "Ocorreu um erro ao restaurar a página:<br/>{0}",
			["Service_RestoreUserError.Text"] = "Ocorreu um erro ao restaurar o usuário.",
			["TabNotSoftDeleted.Text"] = "A página com ID '{0}' não está na Lixeira.",
			["UserNotFound.Text"] = "O usuário com ID '{0}' não foi encontrado.",
			["UserRestored.Text"] = "O usuário foi restaurado com sucesso.",
			["nav_Recyclebin.Text"] = "Lixeira",
			["recyclebin_Actions.Text"] = "Ações",
			["recyclebin_CancelConfirm.Text"] = "Não",
			["recyclebin_Delete.Text"] = "Excluir",
			["recyclebin_DeleteConfirm.Text"] = "Sim",
			["recyclebin_DeletedDate.Text"] = "Data",
			["recyclebin_EmptyRecycleBin.Text"] = "Esvaziar a Lixeira",
			["recyclebin_EmptyRecycleBinConfirm.Text"] = "Deseja esvaziar todos os arquivos da Lixeira?",
			["recyclebin_ModuleTitle.Text"] = "Título do módulo",
			["recyclebin_Modules.Text"] = "Módulos",
			["recyclebin_NoConfirm.Text"] = "Não",
			["recyclebin_NoItems.Text"] = "A Lixeira está vazia no momento",
			["recyclebin_NoModules.Text"] = "Não há módulos na Lixeira",
			["recyclebin_NoPages.Text"] = "Não há páginas na Lixeira",
			["recyclebin_NoUsers.Text"] = "Não há usuários na Lixeira",
			["recyclebin_Page.Text"] = "Página",
			["recyclebin_Pages.Text"] = "Páginas",
			["recyclebin_RemoveModuleConfirm.Text"] = "<p>Confirme que deseja excluir este módulo.</p>",
			["recyclebin_RemoveModulesConfirm.Text"] = "<p>Confirme que deseja excluir os módulos selecionados.</p>",
			["recyclebin_RemovePageConfirm.Text"] = "<p>Confirme que deseja excluir esta página.</p>",
			["recyclebin_RemovePagesConfirm.Text"] = "<p>Confirme que deseja excluir as páginas selecionadas.</p>",
			["recyclebin_RemoveUserConfirm.Text"] = "<p>Confirme que deseja excluir este usuário.</p>",
			["recyclebin_RemoveUsersConfirm.Text"] = "<p>Confirme que deseja excluir os usuários selecionados.</p>",
			["recyclebin_Restore.Text"] = "Restaurar",
			["recyclebin_RestoreModuleConfirm.Text"] = "<p>Confirme que deseja restaurar este módulo.</p>",
			["recyclebin_RestoreModulesConfirm.Text"] = "<p>Confirme que deseja restaurar os módulos selecionados.</p>",
			["recyclebin_RestorePageConfirm.Text"] = "<p>Confirme que deseja restaurar esta página.</p>",
			["recyclebin_RestorePageInvalid.Text"] = "Primeiro você precisa restaurar a página mãe.",
			["recyclebin_RestorePagesConfirm.Text"] = "<p>Confirme que deseja restaurar as páginas selecionadas.</p>",
			["recyclebin_RestorePagesInvalid.Text"] = "As páginas que você tentou restaurar precisam ter a página mãe selecionada ao mesmo tempo.",
			["recyclebin_RestoreUserConfirm.Text"] = "<p>Confirme que deseja restaurar este usuário.</p>",
			["recyclebin_RestoreUsersConfirm.Text"] = "<p>Confirme que deseja restaurar os usuários selecionados.</p>",
			["recyclebin_Title.Text"] = "Lixeira",
			["recyclebin_UnableToSelectAllModules.Text"] = "Não é possível excluir nem restaurar um módulo cuja página esteja na Lixeira.",
			["recyclebin_UserDisplayName.Text"] = "Nome de exibição",
			["recyclebin_Username.Text"] = "Nome de usuário",
			["recyclebin_Users.Text"] = "Usuários",
			["recyclebin_YesConfirm.Text"] = "Sim",
		};

		[Fact]
		public void Pins_Exact_Values()
		{
			Pins.Should().HaveCount(93, "les 93 clés sont épinglées : la garde marque-taux et l'épingle couvrent le même périmètre");
			foreach (var (key, expected) in Pins)
				ResxValue(PtResx, key).Should().Be(expected, "pin du sweep pt-BR — clé {0}.", key);
		}

		/// <summary>Parité des placeholders exigée sur CHAQUE clé, dans les deux
		/// sens. C'est ce contrôle qui a rattrapé <c>ModuleNotFound</c>, qui avait
		/// perdu son <c>{1}</c> alors que le neutre en porte deux.</summary>
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
			ResxValue(PtResx, "ModuleNotFound.Text").Should().Contain("{0}").And.Contain("{1}",
				"défaut corrigé : le neutre localise le module ET la page (« not found on page \"{1}\" »)");
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
			keys.Should().HaveCount(21, "21 cellules du neutre portent du balisage — témoin du périmètre");
			Func<string, string[]> tags = html =>
				Regex.Matches(html, @"</?[a-zA-Z][^>]*>").Select(m => m.Value.ToLowerInvariant()).ToArray();
			foreach (var key in keys)
				tags(pt[key]).Should().Equal(tags(ne[key]),
					"la séquence de balises de {0} doit être celle du neutre (traduction, pas réécriture)", key);
		}

		/// <summary>Un <c>&lt;value&gt;</c> de resx est du TEXTE : une balise écrite
		/// brute y devient un élément XML enfant et la valeur lue par
		/// l'application s'arrête au premier chevron. Défaut réellement introduit
		/// puis corrigé pendant ce grain — la garde l'épingle.</summary>
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

		/// <summary>Terminologie : « Lixeira » partout où le neutre parle de la
		/// corbeille, « Ações », « Usuário » accentué, et pas de portugais
		/// européen résiduel (« Acções », « seleccionados », « eliminar »).</summary>
		[Fact]
		public void Terminology_Lixeira_Acoes_Excluir()
		{
			var pt = Cells(PtResx);
			var ne = Cells(NeutralResx);
			var binCells = ne.Where(c => Regex.IsMatch(c.Value, "recycle bin", RegexOptions.IgnoreCase))
				.Select(c => c.Key).ToArray();
			binCells.Should().HaveCountGreaterThan(8, "témoin du périmètre : les cellules qui nomment la corbeille");
			foreach (var key in binCells)
				pt[key].Should().Contain("Lixeira", "« Lixeira » est le terme pt-BR retenu ({0})", key);
			var joined = string.Join("\n", pt.Values);
			joined.Should().NotContain("papelera").And.NotContain("Acções").And.NotContain("seleccionad");
			joined.Should().NotContain("eliminar", "pt-BR de ce fichier : « excluir » (convention D3)");
			joined.Should().Contain("Ações");
		}

		/// <summary>Avant le sweep, 9 cellules étaient byte-égales à l'es-ES
		/// (« Eliminar », « Sí », « Usuarios », « Módulos »…). Après, il n'en
		/// reste que 4 — des homographes parfaits, corrects dans les deux
		/// langues : ce ne sont pas des non-traductions.</summary>
		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell_Except_Legitimate_Homographs()
		{
			var pt = Cells(PtResx);
			var es = Cells(EsResx);
			var legitimate = new[]
			{
				"recyclebin_Modules.Text", "recyclebin_Page.Text",
				"recyclebin_Pages.Text", "recyclebin_Restore.Text",
			};
			var equal = pt.Where(c => es.TryGetValue(c.Key, out var v) && v == c.Value)
				.Select(c => c.Key).OrderBy(k => k, StringComparer.Ordinal).ToArray();
			equal.Should().Equal(legitimate.OrderBy(k => k, StringComparer.Ordinal).ToArray(),
				"seuls « Módulos », « Página », « Páginas » et « Restaurar » sont légitimement identiques à l'es-ES");
		}
	}
}
