<%--
  This is the main part of our Themes
  It's in a separate file so it can be shared

  Note that it's included using the include-syntax, not as a web-control. 
  This is important, because otherwise Dnn won't detect the panes in here
--%>

<%@ Control language="C#" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<!--#include file="register.ascx"-->

<a class="visually-hidden-focusable" rel="nofollow" href="#to-shine-page-main"><%= LocalizeString("SkipLink.MainContent") %></a>
<nav id="to-shine-page-navigation" class="navbar navbar-expand-lg bg-white">
  <div class="container">
    <a class="navbar-brand" href="<%= DotNetNuke.Common.Globals.NavigateURL(PortalController.GetCurrentPortalSettings().HomeTabId) %>" title="2shine DNN BS5 2sxc (change this in the theme-body.ascx)">
      <img alt="Logo" class="logo img-fluid" src="<%=SkinPath%>images/logo.svg">
    </a>
    <button class="navbar-toggler" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasNavbar">
      <span class="navbar-toggler-icon"></span>
    </button>

    <div class="offcanvas offcanvas-start border-0" id="offcanvasNavbar" tabindex="-1" >
      <div class="offcanvas-header">
        <a class="navbar-brand" href="<%= DotNetNuke.Common.Globals.NavigateURL(PortalController.GetCurrentPortalSettings().HomeTabId) %>" title="2shine DNN BS5 2sxc (change this in the theme-body.ascx)">
          <img alt="Logo" class="logo img-fluid" src="<%=SkinPath%>images/logo.svg">
        </a>
        <button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas"></button>
      </div>
      <div class="offcanvas-body">
        <div class="d-flex flex-column justify-content-end align-items-end flex-grow-1">
          <div class="d-none d-lg-block">
            <dnn:MENU MenuStyle="nav/main" NodeSelector="<%# NavNodeSelector %>" runat="server" />
          </div>

          <div class="d-block d-lg-none w-100">
            <dnn:MENU MenuStyle="nav/main-mobile" NodeSelector="*,0,6" runat="server" />
          </div>

          <div class="d-flex align-items-center order-lg-first">
            <%
            if(LocaleController.Instance.GetLocales(0).Count() > 1) {
            %>
              <ToSic:languagenavigation runat="server" Languages="de-DE:DE,en-US:EN,fr-FR:FR,it-IT:IT" />
            <%
            }
            %> 
            <%
            if (Request.IsAuthenticated) {
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
        </div>
        
      </div>
    </div>
  </div>
</nav>

<div id="to-shine-page-header-pane" class="container-xxl px-0 <%= (HeaderPane.Attributes["class"] ?? "").Contains("DNNEmptyPane") ? "to-shine-header-pane-empty" : "" %>">
  <div id="HeaderPane" runat="server" containertype="G" containername="2shineBS5" containersrc="fullwidthWithoutPadding.ascx"></div>
</div>
<main id="to-shine-page-main">
  <%
  if(ShowBreadcrumb){
  %>
    <div class="to-shine-page-breadcrumb" aria-label="breadcrumb">
      <div class="container py-2">
        <a class="to-shine-page-breadcrumb-link to-shine-page-breadcrumb-home" aria-current="page" href="<%= DotNetNuke.Common.Globals.NavigateURL(PortalSettings.HomeTabId) %>"><%= LocalizeString("Home.Text") %></a>
        <span>&nbsp;&rsaquo;&nbsp;</span><span class="to-shine-page-breadcrumb-trigger display-inline display-md-none"><a aria-current="page">...</a></span>
        <dnn:BREADCRUMB runat="server" aria-current="page" Separator="<span>&nbsp;&rsaquo;&nbsp;</span>" CssClass="to-shine-page-breadcrumb-link" RootLevel="0" />
      </div>		
    </div>
  <%
  }
  %>
  <% 
  if(ShowSidebarNavigation) {
  %>
  <div class="container">
    <div class="row">
      <div class="col-xs-12 col-lg-9 order-lg-2 ly-col-contentpane">
  <%
  }
  %>
        <div id="ContentPane" runat="server" containertype="G" containername="2shineBS5" containersrc="default.ascx"></div>
  <% 
  if(ShowSidebarNavigation) {
  %>        
      </div>
      <div class="col-xs-12 col-lg-3 order-lg-1 ly-col-leftpane">
        <div id="to-shine-nav-sub" class="d-none d-sm-block">  
          <dnn:MENU MenuStyle="nav/sub" NodeSelector="+0,0,2" runat="server" />
        </div>
      </div>
    </div>
  </div>
  <%
  }
  %>
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
        <strong itemprop="name">Argumentum</strong>
      </li>
      <li>
        <span itemprop="address" itemscope itemtype="http://schema.org/PostalAddress">
          <span itemprop="streetAddress">5B rue Martel</span>,
          <span itemprop="postalCode">75010</span>
          <span itemprop="addressLocality">Paris</span>,
          <span itemprop="addressCountry">France</span>
        </span>
      </li>
        <li><a href="tel:+33689156807">+33 6 89 15 68 07</a></li>
      <li>
        <span data-madr1="contact" data-madr2="argumentum" data-madr3="games" data-linktext=""></span>
      </li>
    </ul>
    <div class="to-shine-footer-imprint">
        <dnn:login id="DnnLogin" cssclass="to-shine-page-login d-none d-lg-inline-flex" rel="nofollow" runat="server" />
        <%-- 
          Terms and Privacy Links are set in "Site Settings" > "Site Behavior"
        --%>
        <a href="/terms" title="<%= LocalizeString("Imprint.Text") %>"><%= LocalizeString("Imprint.Text") %></a> | 
        <a href="/privacy" title="<%= LocalizeString("Privacy.Text") %>"><%= LocalizeString("Privacy.Text") %></a>
    </div>
  </div>
</footer>

<!-- include files -->
<dnn:DnnCssInclude runat="server" FilePath="dist/dnn-default.min.css" Priority="99" PathNameAlias="SkinPath" HtmlAttributesAsString="rel:'stylesheet preload', as:'style'" />
<dnn:DnnCssInclude runat="server" FilePath="dist/theme.min.css" Priority="100" PathNameAlias="SkinPath" HtmlAttributesAsString="rel:'stylesheet preload', as:'style'" />
<dnn:DnnJsInclude runat="server" FilePath="dist/lib/bootstrap.bundle.min.js" ForceProvider="DnnFormBottomProvider" Priority="100" PathNameAlias="SkinPath" HtmlAttributesAsString="defer, async" />
<dnn:DnnJsInclude runat="server" FilePath="dist/theme.min.js" ForceProvider="DnnFormBottomProvider" Priority="130" PathNameAlias="SkinPath" HtmlAttributesAsString="defer, async" />

<dnn:DnnCssExclude runat="server" Name="dnndefault" />
<script runat="server">

  protected bool IsHome = false;

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    AttachCustomHeader("<meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no' />");

    var portal = DotNetNuke.Entities.Portals.PortalSettings.Current;
    IsHome = portal.HomeTabId == portal.ActiveTab.TabID;

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
      return Localization.GetString(key, ResourceFilePath);
  }

  protected string ResourceFilePath
  {
    get
    {
      return _resPath ?? (_resPath = Localization.GetResourceFile(this, System.IO.Path.GetFileName(AppRelativeTemplateSourceDirectory + "2shine.resx" )));
    }
  }
  private string _resPath;
</script>
