<section class="page-header background-10">
    <div class="container">
        <div class="row">
            <div class="col-xs-12 col-md-8 to-left">
                <h1 class="page-title"><%= Convert.ToString(PortalSettings.ActiveTab.Title)%></h1>
                <p><%= Convert.ToString(PortalSettings.ActiveTab.Description)%></p>
            </div>
            <div class="col-xs-12 col-md-4 to-right">
                <div runat="server" id="PageHeaderPane" />
            </div>
        </div>
    </div>
</section>