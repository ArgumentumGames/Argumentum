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
	/// Garde du sweep pt-BR (pool #458), fichier 7/7 :
	/// <c>DesktopModules/Admin/HtmlEditorManager/App_LocalResources/ProviderConfiguration.ascx.pt-BR.resx</c>
	/// — la configuration du fournisseur d'éditeur HTML (RadEditor).
	/// <para>
	/// Le fichier est retraduit EN ENTIER (123 des 142 valeurs) : ce n'était
	/// pas du portugais constellé d'espagnol, c'était <b>un texte espagnol</b>
	/// avec des mots portugais insérés (« Ancho do borda », « Si está marcado,
	/// o portal atual estará asociado », « As opciones … no suelen
	/// documentarse »). <b>35 valeurs étaient byte-égales à l'es-ES</b>.
	/// Le compte de clés était juste (142 = 142) : ici le défaut est la langue,
	/// pas la complétude — l'inverse du grain précédent.
	/// </para>
	/// <para>
	/// Défauts non linguistiques corrigés en chemin :
	/// <c>PortalRoot.Text</c> portait une espace finale absente du neutre ;
	/// <c>lblEditorType.Text</c> utilisait <c>&lt;strong&gt;</c> là où le neutre
	/// porte <c>&lt;b&gt;</c> (la séquence de balises doit être celle du neutre) ;
	/// <c>MaxTemplateSize.Help</c> et <c>Template Manager Settings.Title</c>
	/// traduisaient « template » par « planilha » (<b>feuille de calcul</b>) —
	/// faux de sens, désormais « modelo ».
	/// </para>
	/// <para>
	/// Terminologie : <c>Caminho</c> (et non « Ruta »), <c>Altura</c> /
	/// <c>Largura</c> (et non « Alto » / « Ancho »), <c>Tema</c>, <c>função</c>
	/// pour Role (décision du sweep 6/6), <c>Configurações</c> pour Settings
	/// (le neutre dit « Settings », l'es-ES dit « Parámetros » — l'espagnol a
	/// choisi un mot qui n'est pas le sien).
	/// </para>
	/// </summary>
	public class DnnHtmlEditorProviderConfigPtBrGuardTests
	{
		private const string PtResx = "DNNPlatform/DesktopModules/Admin/HtmlEditorManager/App_LocalResources/ProviderConfiguration.ascx.pt-BR.resx";
		private const string EsResx = "DNNPlatform/DesktopModules/Admin/HtmlEditorManager/App_LocalResources/ProviderConfiguration.ascx.es-ES.resx";
		private const string NeutralResx = "DNNPlatform/DesktopModules/Admin/HtmlEditorManager/App_LocalResources/ProviderConfiguration.ascx.resx";

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
		// est auto-contrôlée contre l'es-ES du même fichier.
		// Exclusions volontaires, mesurées ici et non devinées :
		//   « este », « esta » — démonstratifs portugais valides ;
		//   « página »         — identique dans les deux langues ;
		//   « Mostrar »        — infinitif portugais valide ;
		//   « ¿ », « ¡ »       — absents de cet es-ES (marqueur aveugle ici).
		// ------------------------------------------------------------------
		private static readonly Regex[] SpanishMarkers =
		{
			new(@"\w*(ción|siones)\b", RegexOptions.IgnoreCase),
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
			new(@"\bser[áa]n\b", RegexOptions.IgnoreCase),
			new(@"\btiene\b", RegexOptions.IgnoreCase),
			new(@"\bpuede\b", RegexOptions.IgnoreCase),
			new(@"\bdebe\b", RegexOptions.IgnoreCase),
			new(@"\bUtilice\b", RegexOptions.IgnoreCase),
			new(@"\bRuta\b", RegexOptions.IgnoreCase),
			new(@"\bAncho\b", RegexOptions.IgnoreCase),
			new(@"\bAlto\b", RegexOptions.IgnoreCase),
			new(@"\bTamaño\b", RegexOptions.IgnoreCase),
			new(@"\bPar[áa]metros\b", RegexOptions.IgnoreCase),
			new(@"\bgestor\b", RegexOptions.IgnoreCase),
			new(@"\barchivo\b", RegexOptions.IgnoreCase),
			new(@"\bproveedor\b", RegexOptions.IgnoreCase),
			new(@"\bcarpeta\b", RegexOptions.IgnoreCase),
			new(@"\bextensiones\b", RegexOptions.IgnoreCase),
			new(@"\bAnule\b", RegexOptions.IgnoreCase),
			new(@"\busted\b", RegexOptions.IgnoreCase),
			new(@"\bsolo\b", RegexOptions.IgnoreCase),
			new(@"\bnuevo\b|\bnueva\b", RegexOptions.IgnoreCase),
			new(@"\bconfiguraci[óo]n\b", RegexOptions.IgnoreCase),
			new(@"\bCambiar\b", RegexOptions.IgnoreCase),
			new(@"\bEnlazar\b", RegexOptions.IgnoreCase),
		};

		[Fact]
		public void Keys_Match_The_Neutral_File()
		{
			var pt = Cells(PtResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			var ne = Cells(NeutralResx).Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();
			pt.Should().Equal(ne, "le fichier pt-BR doit porter exactement les clés du neutre (aucune ajoutée, aucune retirée)");
			pt.Should().HaveCount(142);
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

		/// <summary>Invariant de FICHIER ENTIER : les 142 cellules sont en
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
				"les 142 cellules pt-BR de la configuration RadEditor sont en portugais — tout marqueur restant est une réinjection : {0}",
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
				["desbordamiento"] = "similar a propriedade de desbordamiento do elemento DIV",
				["Redimensionar automáticamente altura"] = "Redimensionar automáticamente altura:",
				["Ancho do borda"] = "Ancho do borda:",
				["Ancho:"] = "Ancho:",
				["Alto:"] = "Alto:",
				["Ruta:"] = "Ruta:",
				["Tamaño máximo de Arquivo"] = "Tamaño máximo de Arquivo:",
				["devnestar"] = "As extensões também devnestar na lista de anfitrões",
				["diccionario"] = "Nome do diccionario personalizado de correção:",
				["está marcado"] = "Si está marcado, o portal atual estará asociado",
				["Enlazar ao rol"] = "Enlazar ao rol",
				["suelen documentarse"] = "As opções de correção ortográfica não suelen documentarse.",
				["cuadro de diálogo"] = "quando se carga com o cuadro de diálogo",
				["Pegar HTML"] = "Pegar HTML:",
				["Nombre de página"] = "Nombre de página na URL",
				["planilhas"] = "Tamaño máximo em bytes para cargar planilhas",
				["Parámetros de gestor de medios"] = "Parámetros de gestor de medios",
				["dejar o campo vazio"] = "usted deve dejar o campo vazio",
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
			["AutoResizeHeight.Help"] = "O valor da propriedade AutoResizeHeight indica se o RadEditor redimensionará automaticamente sua altura para corresponder à altura do conteúdo.\n\nEsta propriedade é semelhante à propriedade Overflow do elemento DIV, que não é suportada pelos elementos IFRAME.\n\nQuando a propriedade AutoResizeHeight é definida como true, a área de conteúdo do editor é redimensionada para corresponder à altura do conteúdo e nenhuma barra de rolagem aparece.",
			["AutoResizeHeight.Text"] = "Redimensionar altura automaticamente:",
			["BorderWidth.Help"] = "Define a borda em pixels ao redor do controle",
			["BorderWidth.Text"] = "Largura da borda:",
			["CannotManageHTMLEditorProviders.Text"] = "Você não pode gerenciar provedores do editor HTML",
			["Client Script Settings.Title"] = "Configurações de scripts de cliente",
			["Common Settings.Title"] = "Configurações gerais",
			["ContentAreaMode.Help"] = "Escolha entre editar em um documento separado dentro de um iframe (padrão) ou diretamente em uma div na página atual.",
			["ContentAreaMode.Text"] = "Modo da área de conteúdo",
			["ContentFilters.Help"] = "Os filtros do RadEditor para ASP.NET AJAX são pequenos trechos de código, chamados em sequência para processar o conteúdo do editor quando o modo (html / design / visualizar) é alternado ou quando a página é enviada. Cada filtro está descrito aqui: <a href=\"http://www.telerik.com/help/aspnet-ajax/contentfilters.html\" target=\"_blank\">Ajuda on-line da Telerik</a>",
			["ContentFilters.Text"] = "Filtros de conteúdo:",
			["ControlTitle_.Text"] = "Gerenciamento do editor HTML",
			["ControlTitle_.Text1"] = "Gerenciador do editor HTML",
			["CssFile.Help"] = "O arquivo CSS a ser usado pelo editor no modo WYSIWYG. Este arquivo deve residir no diretório do tema ou no diretório do site. O caminho definido aqui é relativo ao seu pai (por exemplo, /editor.css é um arquivo na raiz do seu tema)",
			["CssFile.Text"] = "Arquivo CSS:",
			["Developer Settings.Title"] = "Configurações do desenvolvedor",
			["Document Manager Settings.Title"] = "Configurações do gerenciador de documentos",
			["DocumentsFilters.Help"] = "Uma lista separada por ponto e vírgula das possíveis extensões a serem usadas no gerenciador de documentos. As extensões também devem estar na lista de extensões permitidas das configurações do host. Use o seguinte formato: *.extensãonome",
			["DocumentsFilters.Text"] = "Filtros:",
			["DocumentsPath.Help"] = "A pasta com a qual o gerenciador de documentos inicia ao carregar a caixa de diálogo",
			["DocumentsPath.Text"] = "Caminho:",
			["EditConfigTab.Text"] = "Configuração do editor",
			["EditModes.Help"] = "O RadEditor oferece três modos de edição diferentes: Design, HTML e Visualizar:\n\n<ul><li>Design: no modo Design você pode editar e formatar o conteúdo usando os botões, as listas suspensas e as caixas de diálogo da barra de ferramentas do RadEditor. </li><li>HTML: o modo HTML dá acesso direto ao HTML do conteúdo. Este modo é usado com mais frequência por usuários avançados.</li><li>Visualizar: mostra como o conteúdo ficará antes de atualizar a página.</li></ul>",
			["EditModes.Text"] = "Modos de edição:",
			["EnableResize.Help"] = "Obtém ou define o valor que indica se os usuários poderão redimensionar o controle RadEditor no cliente.",
			["EnableResize.Text"] = "Permitir redimensionamento:",
			["FileBrowserContentProviderTypeName.Help"] = "O provedor de conteúdo é uma classe que controla como os arquivos e as pastas das caixas de diálogo do gerenciador de arquivos são tratados. Substitua esta configuração se você tiver outro provedor de conteúdo no lugar. ",
			["FileBrowserContentProviderTypeName.Text"] = "Provedor do navegador de arquivos:",
			["Flash Manager Settings.Title"] = "Configurações do gerenciador de Flash",
			["FlashFilters.Help"] = "Uma lista separada por ponto e vírgula das possíveis extensões a serem usadas no gerenciador de Flash. As extensões também devem estar na lista de extensões permitidas das configurações do host. Use o seguinte formato: *.extensãonome",
			["FlashFilters.Text"] = "Filtros:",
			["FlashPath.Help"] = "A pasta com a qual o gerenciador de Flash inicia ao carregar a caixa de diálogo",
			["FlashPath.Text"] = "Caminho:",
			["HTMLEditorPageDescription.Text"] = "Fornece opções de configuração para o provedor do editor HTML.",
			["HTMLEditorPageName.Text"] = "Gerenciador do editor HTML",
			["Height.Help"] = "A altura total do controle do editor",
			["Height.Text"] = "Altura:",
			["Image Manager Settings.Title"] = "Configurações do gerenciador de imagens",
			["ImagesFilters.Help"] = "Uma lista separada por ponto e vírgula das possíveis extensões a serem usadas no gerenciador de imagens. As extensões também devem estar na lista de extensões permitidas das configurações do host. Use o seguinte formato: *.extensãonome",
			["ImagesFilters.Text"] = "Filtros:",
			["ImagesPath.Help"] = "A pasta com a qual o gerenciador de imagens inicia ao carregar a caixa de diálogo",
			["ImagesPath.Text"] = "Caminho:",
			["Language.Help"] = "Se definido, substituirá o idioma do contexto do site. Por padrão, a localidade atual é usada; na maioria dos casos você deve deixar este campo vazio",
			["Language.Text"] = "Idioma:",
			["LinksType.Help"] = "\"Usar nome da página\": um link para uma página do site é criado assim: default.aspx?TabName=Home;<br />\"Usar ID da página\": um link para uma página do site é criado assim: default.aspx?TabId=100;<br />\"Normal\": um link para uma página do site é criado como um link normal.",
			["LinksType.Text"] = "Tipo de links de página:",
			["LinksType_Normal.Text"] = "Normal",
			["LinksType_UseTabId.Text"] = "Usar ID da página na URL",
			["LinksType_UseTabName.Text"] = "Usar nome da página na URL",
			["LinksUseRelativeUrls.Help"] = "Se definido como false, os links são criados como absolutos",
			["LinksUseRelativeUrls.Text"] = "Habilitar links de URL relativos:",
			["MaxDocumentSize.Help"] = "Tamanho máximo de upload de documentos em bytes",
			["MaxDocumentSize.Text"] = "Tamanho máximo do arquivo:",
			["MaxFlashSize.Help"] = "Tamanho máximo de upload de Flash em bytes",
			["MaxFlashSize.Text"] = "Tamanho máximo do arquivo:",
			["MaxImageSize.Help"] = "Tamanho máximo de upload de imagens em bytes",
			["MaxImageSize.Text"] = "Tamanho máximo do arquivo:",
			["MaxMediaSize.Help"] = "Tamanho máximo de upload de vídeos em bytes",
			["MaxMediaSize.Text"] = "Tamanho máximo do arquivo:",
			["MaxSilverlightSize.Help"] = "Tamanho máximo de upload de Silverlight em bytes",
			["MaxSilverlightSize.Text"] = "Tamanho máximo do arquivo:",
			["MaxTemplateSize.Help"] = "Tamanho máximo de upload de modelos em bytes",
			["MaxTemplateSize.Text"] = "Tamanho máximo do arquivo:",
			["Media Manager Settings.Title"] = "Configurações do gerenciador de mídia",
			["MediaFilters.Help"] = "Uma lista separada por ponto e vírgula das possíveis extensões a serem usadas no gerenciador de mídia (imagens/vídeos). As extensões também devem estar na lista de extensões permitidas das configurações do host. Use o seguinte formato: *.extensãonome",
			["MediaFilters.Text"] = "Filtros:",
			["MediaPath.Help"] = "A pasta com a qual o gerenciador de mídia inicia ao carregar a caixa de diálogo",
			["MediaPath.Text"] = "Caminho:",
			["NewLineBr.Help"] = "A propriedade NewLineBr especifica se o editor deve inserir uma nova linha (tag <BR>) ou um novo parágrafo (tag <P>) quando a tecla Enter é pressionada. O valor padrão é true (tag <BR>), para refletir de perto o comportamento dos processadores de texto de desktop. Nesse caso, você pode inserir tags de parágrafo pressionando Ctrl+M ou o botão Novo parágrafo.\n\nSe você definir a propriedade NewLineBr como false, uma tag de parágrafo será inserida a cada retorno de carro (pressionando Enter). Nesse caso, pressionar Shift+Enter inserirá uma tag <BR>.\n\nTrabalhando com listas com marcadores/numeradas no Telerik RadEditor\nListas são tipos de parágrafos e aceitam os mesmos atributos que as tags de parágrafo. Isso significa que, sempre que você inserir uma lista no seu HTML, você insere um novo parágrafo. Desde o Telerik RadEditor v5.05 o mecanismo NewLineBr foi aprimorado. No entanto, isso torna impossível, por exemplo, iniciar uma lista com marcadores/numerada com a linha em branco. <a href=\"http://www.telerik.com/help/aspnet-ajax/usingthenewlinebrproperty.html\" target=\"_blank\">Leia a ajuda on-line para mais informações</a>",
			["NewLineBr.Text"] = "Renderizar novas linhas como quebras:",
			["NotCurrentProvider.Text"] = "O RadEditorProvider não é o provedor de editor HTML padrão no momento; nenhuma alteração será aplicada ao provedor atual. Se você quiser que a configuração tenha efeito, atualize o provedor padrão para 'DotNetNuke.RadEditorProvider' na lista suspensa à esquerda.",
			["OnClientCommandExecuting.Help"] = "Define um nome de função a ser chamada quando o editor dispara o evento OnExecute no cliente (disparado imediatamente antes de um comando ser executado). A função pode ser colocada no arquivo js definido na configuração ScriptToLoad. Este evento é útil se você quiser adicionar suporte a emoticons, por exemplo. (Leia <a href=\"http://radeditor.codeplex.com/Thread/View.aspx?ThreadId=231612\" target=\"_blank\">esta mensagem do fórum</a> para mais informações)",
			["OnClientCommandExecuting.Text"] = "Execução de comando:",
			["OnClientPasteHtml.Help"] = "Define um nome de função a ser chamada quando o editor dispara o evento OnPaste (cada vez que algo é colado no código HTML da área de edição HTML do editor). A função pode ser colocada no arquivo js definido na configuração ScriptToLoad.\n\n<a href=\"http://radeditor.codeplex.com/wikipage?title=OnClientPasteExample\" target=\"_blank\">Veja este exemplo para um exemplo detalhado</a>",
			["OnClientPasteHtml.Text"] = "Colar HTML:",
			["PortalRoot.Text"] = "[PortalRoot]",
			["ScriptToLoad.Help"] = "Se definido, o provedor carregará o arquivo javascript especificado aqui. Este deve ser um caminho relativo ao diretório do tema atual.\n\n<a href=\"http://radeditor.codeplex.com/wikipage?title=OnClientPasteExample\" target=\"_blank\">Veja este exemplo para um exemplo detalhado</a>",
			["ScriptToLoad.Text"] = "Script a carregar:",
			["ShowPortalLinks.Help"] = "Se definido como true, uma lista suspensa aparecerá na barra de ferramentas, renderizando uma árvore de páginas do site. Quando este recurso está ativo, você pode marcar um bloco de texto e selecionar uma página dessa árvore para fazer o editor vincular o texto a uma página específica.\n\nVocê deve definir a ferramenta CustomLinks na configuração da sua barra de ferramentas para que isso funcione!",
			["ShowPortalLinks.Text"] = "Mostrar links do site:",
			["Silverlight Manager Settings.Title"] = "Configurações do gerenciador de Silverlight",
			["SilverlightFilters.Help"] = "Uma lista separada por ponto e vírgula das possíveis extensões a serem usadas no gerenciador de Silverlight. As extensões também devem estar na lista de extensões permitidas das configurações do host. Use o seguinte formato: *.extensãonome",
			["SilverlightFilters.Text"] = "Filtros:",
			["SilverlightPath.Help"] = "A pasta com a qual o gerenciador de Silverlight inicia ao carregar a caixa de diálogo",
			["SilverlightPath.Text"] = "Caminho:",
			["Skin.Help"] = "O tema a ser usado pelo controle do editor",
			["Skin.Text"] = "Tema:",
			["SpellAllowAddCustom.Help"] = "As opções de verificação ortográfica raramente estão documentadas, infelizmente. Em geral, deixe estes campos vazios a menos que você saiba o que está fazendo. <a href=\"http://www.telerik.com/products/aspnet-ajax/spell.aspx\" target=\"_blank\">Saiba mais sobre as opções de verificação ortográfica no site da Telerik</a>",
			["SpellAllowAddCustom.Text"] = "Permitir ortografia personalizada:",
			["SpellCheckProvider.Help"] = "As opções de verificação ortográfica raramente estão documentadas, infelizmente. Em geral, deixe estes campos vazios a menos que você saiba o que está fazendo. <a href=\"http://www.telerik.com/products/aspnet-ajax/spell.aspx\" target=\"_blank\">Saiba mais sobre as opções de verificação ortográfica no site da Telerik</a>",
			["SpellCheckProvider.Text"] = "Nome do provedor de verificação ortográfica:",
			["SpellCustomDictionarySourceTypeName.Help"] = "As opções de verificação ortográfica raramente estão documentadas, infelizmente. Em geral, deixe estes campos vazios a menos que você saiba o que está fazendo. <a href=\"http://www.telerik.com/products/aspnet-ajax/spell.aspx\" target=\"_blank\">Saiba mais sobre as opções de verificação ortográfica no site da Telerik</a>",
			["SpellCustomDictionarySourceTypeName.Text"] = "Nome do dicionário ortográfico personalizado:",
			["SpellCustomDictionarySuffix.Help"] = "As opções de verificação ortográfica raramente estão documentadas, infelizmente. Em geral, deixe estes campos vazios a menos que você saiba o que está fazendo. <a href=\"http://www.telerik.com/products/aspnet-ajax/spell.aspx\" target=\"_blank\">Saiba mais sobre as opções de verificação ortográfica no site da Telerik</a>",
			["SpellCustomDictionarySuffix.Text"] = "Sufixo do dicionário ortográfico personalizado:",
			["SpellDictionaryLanguage.Help"] = "As opções de verificação ortográfica raramente estão documentadas, infelizmente. Em geral, deixe estes campos vazios a menos que você saiba o que está fazendo. <a href=\"http://www.telerik.com/products/aspnet-ajax/spell.aspx\" target=\"_blank\">Saiba mais sobre as opções de verificação ortográfica no site da Telerik</a>",
			["SpellDictionaryLanguage.Text"] = "Idioma do dicionário ortográfico:",
			["SpellDictionaryPath.Help"] = "As opções de verificação ortográfica raramente estão documentadas, infelizmente. Em geral, deixe estes campos vazios a menos que você saiba o que está fazendo. <a href=\"http://www.telerik.com/products/aspnet-ajax/spell.aspx\" target=\"_blank\">Saiba mais sobre as opções de verificação ortográfica no site da Telerik</a>",
			["SpellDictionaryPath.Text"] = "Caminho do dicionário ortográfico:",
			["SpellEditDistance.Help"] = "As opções de verificação ortográfica raramente estão documentadas, infelizmente. Em geral, deixe estes campos vazios a menos que você saiba o que está fazendo. <a href=\"http://www.telerik.com/products/aspnet-ajax/spell.aspx\" target=\"_blank\">Saiba mais sobre as opções de verificação ortográfica no site da Telerik</a>",
			["SpellEditDistance.Text"] = "Distância de edição ortográfica:",
			["SpellFragmentIgnoreOptions.Help"] = "As opções de verificação ortográfica raramente estão documentadas, infelizmente. Em geral, deixe estes campos vazios a menos que você saiba o que está fazendo. <a href=\"http://www.telerik.com/products/aspnet-ajax/spell.aspx\" target=\"_blank\">Saiba mais sobre as opções de verificação ortográfica no site da Telerik</a>",
			["SpellFragmentIgnoreOptions.Text"] = "Opções de fragmentos ignorados na verificação ortográfica:",
			["SpellWordIgnoreOptions.Help"] = "As opções de verificação ortográfica raramente estão documentadas, infelizmente. Em geral, deixe estes campos vazios a menos que você saiba o que está fazendo. <a href=\"http://www.telerik.com/products/aspnet-ajax/spell.aspx\" target=\"_blank\">Saiba mais sobre as opções de verificação ortográfica no site da Telerik</a>",
			["SpellWordIgnoreOptions.Text"] = "Opções de palavras ignoradas na verificação ortográfica:",
			["StripFormattingOptions.Help"] = "Os desenvolvedores podem impor a formatação do conteúdo usando a propriedade StripFormattingOptions. Como resultado, a remoção de formatação será aplicada a todo o conteúdo que os usuários tentarem colar. A configuração EditorStripFormattingOptions pode ter qualquer um dos seguintes valores, ou uma combinação deles:\n<ul>\n<li>None: cola o conteúdo da área de transferência como está. </li>\n<li>NoneSupressCleanMessage: não remove nada ao colar e não faz perguntas. </li>\n<li>MSWord: remove as tags específicas do Word ao colar, preservando fontes e tamanhos de texto. </li>\n<li>MSWordNoFonts: remove as tags específicas do Word ao colar, preservando apenas os tamanhos de texto. </li>\n<li>MSWordRemoveAll: remove as tags específicas do Word ao colar, eliminando tanto as fontes quanto os tamanhos de texto. </li>\n<li>Css: remove os estilos CSS ao colar. </li>\n<li>Font: remove as tags Font ao colar. </li>\n<li>Span: remove as tags Span ao colar. </li>\n<li>All: remove toda a formatação HTML e cola texto sem formatação. </li>\n<li>AllExceptNewLines: limpa todas as tags, exceto \"br\" e as novas linhas (\\n), ao colar.</li></ul>",
			["StripFormattingOptions.Text"] = "Opções de remoção de formatação:",
			["Template Manager Settings.Title"] = "Configurações do gerenciador de modelos",
			["TemplateFilters.Help"] = "Uma lista separada por ponto e vírgula das possíveis extensões a serem usadas no gerenciador de modelos. As extensões também devem estar na lista de extensões permitidas das configurações do host. Use o seguinte formato: *.extensãonome",
			["TemplateFilters.Text"] = "Filtros:",
			["TemplatePath.Help"] = "A pasta com a qual o gerenciador de modelos inicia ao carregar a caixa de diálogo",
			["TemplatePath.Text"] = "Caminho:",
			["Toolbar Settings.Title"] = "Configurações da barra de ferramentas",
			["ToolbarConfigTab.Text"] = "Configuração da barra de ferramentas",
			["ToolbarMode.Help"] = "O RadEditor para ASP.NET AJAX introduz uma propriedade chamada ToolbarMode, que especifica o comportamento da barra de ferramentas. Estas são as diferentes opções para definir o ToolbarMode:<ul><li>Default - a barra de ferramentas é estática e posicionada sobre a área de conteúdo\n</li><li>PageTop - neste modo, quando um determinado editor recebe o foco, sua barra de ferramentas aparece ancorada no topo da página\n</li><li>ShowOnFocus - aqui a barra de ferramentas aparece logo acima do editor quando ele recebe o foco.\n</li><li>Floating - a barra de ferramentas aparece em uma janela e permite que o usuário a mova pela página\n</li></ul>",
			["ToolbarMode.Text"] = "Modo:",
			["ToolsWidth.Help"] = "Define a largura da barra de ferramentas do editor; isto só deve ser usado quando o modo da barra de ferramentas estiver definido como algo diferente de Default!",
			["ToolsWidth.Text"] = "Largura:",
			["UserFolder.Text"] = "[Pasta do usuário]",
			["Width.Help"] = "A largura total do controle do editor. ",
			["Width.Text"] = "Largura:",
			["btnEditToolsFile.Text"] = "Configurar as barras de ferramentas do editor",
			["btnEnable.Text"] = "Alterar",
			["btnUpdateConfig.Text"] = "Atualizar configuração",
			["cmdCopy.Text"] = "Copiar",
			["cmdCreate.Text"] = "Criar",
			["lblChangeProvider.Help"] = "Selecionar um provedor diferente na lista suspensa e clicar em 'Alterar' mudará o provedor do editor HTML de toda a instalação.",
			["lblChangeProvider.Text"] = "Alterar provedor",
			["lblCurrentProvider.Help"] = "Este é o provedor do editor HTML definido atualmente para esta instalação. ",
			["lblCurrentProvider.Text"] = "Provedor atual",
			["lblEditorConfig.Text"] = "Configuração do editor",
			["lblEditorType.Text"] = "O provedor atual nesta instalação DNN é <b>{0}</b>",
			["lblMode.Help"] = "A função com a qual você deseja associar esta configuração do editor.",
			["lblMode.Text"] = "Vincular à função",
			["lblNotAuthorized.Text"] = "Você não está autorizado a usar este módulo.",
			["lblNote.Text"] = "A finalidade deste módulo é registrar e configurar o provedor Rad Editor. Ele ajuda os usuários host a definir o provedor de HTML no web.config e a manter sua configuração.",
			["lblOnlyHost.Text"] = "Somente usuários host (superusuários) podem usar este módulo.",
			["lblPortal.Help"] = "Se marcado, o site atual será associado a esta configuração do editor. ",
			["lblPortal.Text"] = "Vincular ao site",
			["lblSelectedProvider.Text"] = "Provedor atual",
			["lblTabs.Help"] = "Selecionar uma página na exibição em árvore associará esta configuração do editor à página selecionada.",
			["lblTabs.Text"] = "Vincular à página",
		};

		[Fact]
		public void Pins_Exact_Values()
		{
			Pins.Should().HaveCount(142, "les 142 clés sont épinglées : la garde marque-taux et l'épingle couvrent le même périmètre");
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

		/// <summary>Le squelette de balises des cellules HTML est celui du
		/// neutre — y compris ses <c>&lt;BR&gt;</c> / <c>&lt;P&gt;</c> en
		/// MAJUSCULES, qui sont de la documentation citée et non du balisage
		/// à « normaliser » en <c>&lt;br/&gt;</c>.</summary>
		[Fact]
		public void Html_Skeleton_Matches_Neutral()
		{
			var ne = Cells(NeutralResx);
			var pt = Cells(PtResx);
			var keys = ne.Where(c => c.Value.Contains('<')).Select(c => c.Key).ToArray();
			keys.Should().HaveCount(19, "19 cellules du neutre portent du balisage — témoin du périmètre");
			Func<string, string[]> tags = html =>
				Regex.Matches(html, @"</?[a-zA-Z][^>]*>").Select(m => m.Value.ToLowerInvariant()).ToArray();
			foreach (var key in keys)
				tags(pt[key]).Should().Equal(tags(ne[key]),
					"la séquence de balises de {0} doit être celle du neutre (traduction, pas réécriture)", key);
			ResxValue(PtResx, "lblEditorType.Text").Should().Contain("<b>{0}</b>",
				"le neutre porte <b> : la version pt-BR utilisait <strong>, ce qui déviait la séquence");
		}

		[Fact]
		public void Values_Are_Text_Not_Nested_Xml()
		{
			var offenders = Load(PtResx).Root!.Elements("data")
				.Where(d => d.Element("value") != null && d.Element("value")!.HasElements)
				.Select(d => d.Attribute("name")!.Value)
				.ToArray();
			offenders.Should().BeEmpty(
				"aucune valeur ne doit contenir d'élément XML : les balises sont échappées (&lt;ul&gt;) : {0}",
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

		/// <summary>Terminologie : les mots-clés d'interface sont portugais.
		/// « Caminho » et non « Ruta », « Altura »/« Largura » et non
		/// « Alto »/« Ancho », « Tema » et non « Skin », « Configurações » et
		/// non « Parámetros », « função » et non « rol ».</summary>
		[Fact]
		public void Terminology_Is_PtBr()
		{
			var pt = Cells(PtResx);
			pt.Values.Count(v => v.Contains("Caminho")).Should().BeGreaterThanOrEqualTo(7,
				"témoin du périmètre : les cellules « Path » du neutre");
			var joined = string.Join("\n", pt.Values);
			joined.Should().NotContain("Ruta").And.NotContain("Ancho").And.NotContain("Alto:")
				.And.NotContain("Parámetros").And.NotContain("planilha");
			joined.Should().NotMatchRegex(@"\brol\b", "« rol » est la forme espagnole (décision du sweep 6/6 : « função »)");
			pt["Height.Text"].Should().Be("Altura:");
			pt["Width.Text"].Should().Be("Largura:");
			pt["Skin.Text"].Should().Be("Tema:");
			pt["lblMode.Text"].Should().Be("Vincular à função");
		}

		/// <summary>Avant le sweep, 35 cellules étaient byte-égales à l'es-ES
		/// (« Ruta: », « Ancho: », « Alto: », « Parámetros generales »…).
		/// Après, il n'en reste que 11 — des homographes parfaits.</summary>
		[Fact]
		public void No_Cell_Equals_The_Spanish_Cell_Except_Legitimate_Homographs()
		{
			var pt = Cells(PtResx);
			var es = Cells(EsResx);
			var legitimate = new[]
			{
				"DocumentsFilters.Text", "FlashFilters.Text", "ImagesFilters.Text",
				"Language.Text", "LinksType_Normal.Text", "MediaFilters.Text",
				"SilverlightFilters.Text", "TemplateFilters.Text",
				"ToolbarMode.Text", "cmdCopy.Text",
			};
			var equal = pt.Where(c => es.TryGetValue(c.Key, out var v) && v == c.Value)
				.Select(c => c.Key).OrderBy(k => k, StringComparer.Ordinal).ToArray();
			equal.Should().Equal(legitimate.OrderBy(k => k, StringComparer.Ordinal).ToArray(),
				"seuls les homographes parfaits sont légitimement identiques à l'es-ES");
		}
	}
}
