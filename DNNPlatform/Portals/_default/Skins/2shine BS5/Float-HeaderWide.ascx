<script runat="server">
  // Settings which determine what variation of the skin is loaded
  public bool ShowSidebarNavigation = false;
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

