<script runat="server">
  // Settings which determine what variation of the skin is loaded
  public bool ShowSidebarNavigation = false;
  // Turns Breadcrumbs on this layout on and off
  public bool ShowBreadcrumb = true;
  // Sets the Node selectors for the main desktop navigation -> determin which pages are laoded and included in the navigation 
  //* -> means the root level, 0 -> means skip the nav-level-0, 1 -> means the nav-level-1
  public string NavNodeSelector = "*,0,1";
</script>

<%-- 
  Set common CSS classes on the body which determine the Layout 
  Layout="Fullscreen" are: Default, FloatWideHeader, Fullscreen, Centered
  Navigation="Left": Right, Center, Left
--%>
<%@ Register TagPrefix="tosic" TagName="BodyCssClasses" src="controls/body-css-classes.ascx" %>
<tosic:BodyCssClasses runat="server" Layout="FloatWideHeader" Navigation=""/>

<%-- All Themes share the same main part - they only differ in the css classes etc. --%>
<!--#include file="controls/theme-body.ascx"-->

