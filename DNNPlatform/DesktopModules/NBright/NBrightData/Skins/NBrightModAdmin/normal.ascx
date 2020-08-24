<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="NAV" Src="~/Admin/Skins/Nav.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TEXT" Src="~/Admin/Skins/Text.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LEFTMENU" Src="~/Admin/Skins/LeftMenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LINKS" Src="~/Admin/Skins/Links.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="STYLES" Src="~/Admin/Skins/Styles.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.DDRMenu.TemplateEngine" Assembly="DotNetNuke.Web.DDRMenu" %>
<%@ Register TagPrefix="dnn" TagName="MENU" src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="CONTROLPANEL" Src="~/Admin/Skins/controlpanel.ascx" %>
<%@ Register TagPrefix="dnn" TagName="META" Src="~/Admin/Skins/Meta.ascx" %>
<dnn:Meta runat="server" Name="viewport" Content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
<div class="pagemaster">
<div class="page">

<div class="langpane"><dnn:LANGUAGE runat="server" id="dnnLANGUAGE" showMenu="False" showLinks="True" SelectedItemTemplate="&lt;a class='langsel' href='[URL]' title='[CULTURE:NATIVENAME]'&gt;[CULTURE:TWOLETTERISOCODE]&lt;/a&gt;" AlternateTemplate="&lt;a href='[URL]' title='[CULTURE:NATIVENAME]'&gt;[CULTURE:TWOLETTERISOCODE]&lt;/a&gt;" ItemTemplate="&lt;a href='[URL]' title='[CULTURE:NATIVENAME]'&gt;[CULTURE:TWOLETTERISOCODE]&lt;/a&gt;" /></div>
<div id="ContentPane" runat="server" class="contentpane"></div>
<div class="bottom"><div class="footer"><span>&copy;</span><i class="nwb-icon"></i><span>by Nevoweb</span></div></div>

</div>
</div>