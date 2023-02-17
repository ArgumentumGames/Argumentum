<%@ Control language="C#" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%-- The ID contains the module-id, which lets content inside add CSS affecting this --%>
<div id="module-<%= ModuleConfiguration.ModuleID %>" class="to-shine-background-container py-4">
  <div id="ContentPane" class="container" runat="server"></div>
</div>