<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.DDRMenu.TemplateEngine" Assembly="DotNetNuke.Web.DDRMenu" %>
<%@ Register TagPrefix="dnn" TagName="MENU" src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="ToSic" TagName="LanguageNavigation" src="LanguageNavigation.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>

<%-- Change the page title to contain the breadcrumbi in an SEO optimized way --%>
<%@ Register TagPrefix="tosic" TagName="PageTitle" src="optimize-page-title.ascx" %>
<tosic:PageTitle runat="server" />

<%-- Activate Quick-Edit in empty pages if 2sxc is installed
  more infos on 2sxc quick-edit: https://2sxc.org/en/blog/post/quick-edit-2-add-move-delete-modules-in-preview-touch-capable-for-dnn
--%>
<%@ Register TagPrefix="tosic" TagName="SxcQuickEdit" src="2sxc-quickedit.ascx" %>
<tosic:SxcQuickEdit runat="server" />
