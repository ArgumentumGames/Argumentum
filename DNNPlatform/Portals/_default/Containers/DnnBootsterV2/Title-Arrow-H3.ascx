<%@ Control AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<div class="bootsterContainer dnnc-clean">
    <h3><i class="fa fa-angle-right" aria-hidden="true"></i> <dnn:TITLE runat="server" id="dnnTITLE"/></h3>
	<div id="ContentPane" runat="server" class="contentpane"></div>
</div>
