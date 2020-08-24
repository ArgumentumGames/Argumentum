<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="JQUERY" Src="~/Admin/Skins/jQuery.ascx" %>
<%@ Register TagPrefix="dnn" TagName="META" Src="~/Admin/Skins/Meta.ascx" %>
<%@ Register TagPrefix="dnn" TagName="MENU" Src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TEXT" Src="~/Admin/Skins/Text.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<!--#include file = "layouts/system/_header-includes.ascx" -->
<div class="dnnbooster">
    <header>
        <div class="fixed-top controlbarfix">        
            <!--#include file = "layouts/topbar.ascx" -->
            <!--#include file = "layouts/searchbox.ascx" -->  
            <!--#include file = "layouts/navbar.ascx" -->    
        </div>
    </header><!-- End Header Section -->

    <div class="header-spacer"></div>
    <!--#include file = "layouts/page-header.ascx" -->

    <div class="container">
         <div class="row"><div class="col-md-12"><div id="ContentPane" runat="server" /></div> </div>
    </div><!-- End : Content Pane : full width -->

    <section>
        <!--#include file = "layouts/content-areas.ascx" -->
    </section><!-- End : Content Areas -->

    <footer>
        <div class="container">
            <!--#include file = "layouts/footer.ascx" -->
        </div>
        <div class="legal-footer">
            <div class="container">
                <!--#include file = "layouts/footer-legal.ascx" -->
            </div>
        </div>
    </footer>  

</div> <!-- ./dnnbootster -->
<!--#include file = "layouts/system/_footer-includes.ascx" -->

