<%--
  This is the main part of our Themes
  It's in a separate file so it can be shared

  Note that it's included using the include-syntax, not as a web-control. 
  This is important, because otherwise Dnn won't detect the panes in here
--%>

<%@ Control language="C#" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.DDRMenu.TemplateEngine" Assembly="DotNetNuke.Web.DDRMenu" %>
<%@ Register TagPrefix="dnn" TagName="MENU" src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="ToSic" TagName="LanguageNavigation" src="controls/LanguageNavigation.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%-- Change the page title to contain the breadcrumbi in an SEO optimized way --%>
<%@ Register TagPrefix="tosic" TagName="PageTitle" src="controls/optimize-page-title.ascx" %>
<tosic:PageTitle runat="server" />

<%-- 
  Set common CSS classes on the body which determine the Layout 
  Layout="Fullscreen" are: Default, FloatWideHeader, Fullscreen, Centered
  Navigation="Left": Right, Center, Left
--%>
<%@ Register TagPrefix="tosic" TagName="BodyCssClasses" src="controls/body-css-classes.ascx" %>
<tosic:BodyCssClasses runat="server" Layout="Landingpage" Navigation="right"/>

<%-- Activate Quick-Edit in empty pages if 2sxc is installed
  more infos on 2sxc quick-edit: https://2sxc.org/en/blog/post/quick-edit-2-add-move-delete-modules-in-preview-touch-capable-for-dnn
--%>
<%@ Register TagPrefix="tosic" TagName="SxcQuickEdit" src="controls/2sxc-quickedit.ascx" %>
<tosic:SxcQuickEdit runat="server" />

<a class="visually-hidden-focusable" rel="nofollow" href="#to-shine-page-main"><%= LocalizeString("SkipLink.MainContent") %></a>
<header id="to-shine-page-header">
  <div class="container d-flex justify-content-between align-items-center py-3">			
    <a class="logo" href="<%= DotNetNuke.Common.Globals.NavigateURL(PortalController.GetCurrentPortalSettings().HomeTabId) %>" title="2shine DNN BS5 2sxc (change this in the theme-body.ascx)">			
      <img alt="Logo" class="img-fluid" src="<%=SkinPath%>images/logo.svg">
    </a>
    <div class="to-shine-mobile-hamburger" data-bs-toggle="offcanvas" data-bs-target="#offcanvasStart" aria-controls="offcanvasStart" title="Menu">
      <div>
        <span></span>
        <span></span>
        <span></span>
      </div>
    </div>
  <div class="offcanvas offcanvas-start" tabindex="-1" id="offcanvasStart" aria-labelledby="offcanvasTopLabel">
    <div class="offcanvas-header">
      <a class="logo" href="<%= DotNetNuke.Common.Globals.NavigateURL(PortalController.GetCurrentPortalSettings().HomeTabId) %>" title="2shine DNN BS5 2sxc  (change this in the theme-body.ascx)">			
        <img alt="Logo" class="img-fluid" src="<%#SkinPath%>images/logo.svg">
      </a>
      <button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
    </div>
    <div class="offcanvas-body bg-primary px-0">
      <dnn:MENU MenuStyle="nav/main-mobile" NodeSelector="*,0,6" runat="server" />
      <ToSic:languagenavigation runat="server" Languages="de-DE:DE,en-US:EN,fr-FR:FR,it-IT:IT" />
    </div>
  </div>
    
    <nav id="nav-desktop" class="d-none d-lg-flex flex-column align-items-end">
      <div class="d-none d-lg-flex">
        <ToSic:languagenavigation runat="server" Languages="de-DE:DE,en-US:EN,fr-FR:FR,it-IT:IT" />
        <%
        if(DotNetNuke.Security.PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName)) {
        %>
          <a href="?ctl=logoff" Title="Logoff" class="to-shine-login" target="_self">
            <svg version="1.1" id="Ebene_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
              viewBox="0 0 12.2 14.5" style="enable-background:new 0 0 12.2 14.5;" xml:space="preserve">
            <g>
              <path d="M12.2,8.8v4.4c0,0.7-0.6,1.3-1.3,1.3H1.3c-0.7,0-1.3-0.6-1.3-1.3V8.8c0-0.7,0.6-1.3,1.3-1.3H2V4.7c0-2.3,1.8-4.2,4.1-4.2
                s4.2,1.9,4.2,4.2v0.4c0,0.4-0.3,0.7-0.7,0.7H8.8c-0.4,0-0.7-0.3-0.7-0.7V4.7c0-1.1-0.9-2-2-2c-1.1,0-1.9,0.9-1.9,2v2.8h6.8
                C11.7,7.5,12.2,8.1,12.2,8.8z"/>
            </g>
            </svg>
          </a>
        <%
        } else {
        %>        
          <a href="?ctl=login" Title="Login" class="to-shine-login" target="_self">
            <svg version="1.1" id="Lock" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
              viewBox="0 0 12.2 14" style="enable-background:new 0 0 12.2 14;" xml:space="preserve">
              <g>
                <path d="M12.2,7.4v5.2c0,0.7-0.6,1.3-1.3,1.3H1.3C0.6,14,0,13.4,0,12.7V7.4c0-0.7,0.6-1.3,1.3-1.3H2v-2C2,1.9,3.8,0,6.1,0
                  s4.2,1.9,4.2,4.2v2h0.7C11.7,6.1,12.2,6.7,12.2,7.4z M8.1,4.2c0-1.1-0.9-2-2-2s-2,0.9-2,2v2h3.9V4.2z"/>
              </g>
            </svg>
          </a>
        <%
        }
        %>        
      </div>
    </nav>
  </div>
</header>

<main id="to-shine-page-main">
  <div id="ContentPane" runat="server" containertype="G" containername="2shine BS5" containersrc="fullwidthWithoutPadding.ascx"></div>
  
  <a id="to-shine-to-top" href="#" title="Nach oben" rel="nofollow">
    <svg xmlns="http://www.w3.org/2000/svg" width="19.032" height="20.034" viewBox="0 0 19.032 20.034">
      <g id="Group_2" data-name="Group 2" transform="translate(-1055.984 -551.276)">
        <path id="Path_2" data-name="Path 2" d="M8.1,16.2,0,8.1,8.1,0" transform="translate(1073.602 552.69) rotate(90)" fill="none" stroke="#fff" stroke-linecap="round" stroke-width="2"/>
        <line id="Line_1" data-name="Line 1" y2="17.599" transform="translate(1065.481 552.711)" fill="none" stroke="#fff" stroke-linecap="round" stroke-width="2"/>
      </g>
    </svg>
  </a>
</main>
<footer id="to-shine-page-footer">
  <div class="container py-4 d-flex justify-content-md-between flex-column flex-md-row text-white">
    <ul class="to-shine-footer-address" itemscope itemtype="http://schema.org/LocalBusiness">
      <li>
        <strong itemprop="name">2shine DNN BS5 2sxc </strong>
      </li>
      <li>
        <span itemprop="address" itemscope itemtype="http://schema.org/PostalAddress">
          <span itemprop="streetAddress">Shine Road 77</span>,
          <span itemprop="postalCode">50355</span>
          <span itemprop="addressLocality">Shine City</span>,
          <span itemprop="addressCountry">Shine Country</span>
        </span>
      </li>
        <li><a href="tel:+41817506777">+41 81 750 67 77</a></li>
      <li>
        <span data-madr1="shine" data-madr2="example" data-madr3="com" data-linktext=""></span>
      </li>
    </ul>
    <div class="to-shine-footer-imprint">
      <dnn:login id="DnnLogin" cssclass="to-shine-page-login d-none d-lg-inline-flex" rel="nofollow" runat="server" />  
      <%-- 
        Terms and Privacy Links are set in "Site Settings" > "Site Behavior"
      --%>
      <a href="<%= DotNetNuke.Common.Globals.NavigateURL(PortalController.GetCurrentPortalSettings().PrivacyTabId) %>" title="<%= LocalizeString("Imprint.Text") %>"><%= LocalizeString("Imprint.Text") %></a> | 
      <a href="<%= DotNetNuke.Common.Globals.NavigateURL(PortalController.GetCurrentPortalSettings().TermsTabId) %>" title="<%= LocalizeString("Privacy.Text") %>"><%= LocalizeString("Privacy.Text") %></a>
    </div>
  </div>
</footer>

<!-- include files -->
<dnn:DnnCssInclude runat="server" FilePath="dist/theme.min.css" Priority="100" PathNameAlias="SkinPath" />
<dnn:DnnJsInclude runat="server" FilePath="node_modules/bootstrap/dist/js/bootstrap.bundle.min.js" ForceProvider="DnnFormBottomProvider" Priority="100" PathNameAlias="SkinPath"  />
<dnn:DnnJsInclude runat="server" FilePath="dist/theme.min.js" ForceProvider="DnnFormBottomProvider" Priority="130" PathNameAlias="SkinPath" />
<script runat="server">
  
  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    AttachCustomHeader("<meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no' />");

    // Set various FavIcon and Icon headers according to best practices
    // The next line is disabled by default, because it requires RazorBlade to be installed.
    // How to install RazorBlade 3: https://azing.org/dnn-community/r/zbh8JC5T
    // How to create best-practice FavIcons: https://azing.org/dnn-community/r/UhgWJbxh
    // ToSic.Razor.Blade.HtmlPage.AddIconSet(SkinPath + "favicon.png");
  }
  protected void AttachExternalCSS(string CSSPath) { AttachCustomHeader("<link type='text/css' rel='stylesheet' href='" + CSSPath + "' />"); }
  protected void AttachExternalJS(string JSPath) { AttachCustomHeader("<script type='text/javascript' src='" + JSPath + "'></scr" + "ipt>"); }
  protected void AttachCustomHeader(string CustomHeader) { HtmlHead HtmlHead = (HtmlHead)Page.FindControl("Head"); if ((HtmlHead != null)) { HtmlHead.Controls.Add(new LiteralControl(CustomHeader));	}	}

  protected string LocalizeString(string key)
  {
      return Localization.GetString(key, Localization.GetResourceFile(this, System.IO.Path.GetFileName(this.AppRelativeVirtualPath)));
  }
</script>


