<ul class="list-inline general-functions">            

    <!-- Search -->
    <li class="list-inline-item">
        <a href="#searchReveal"
            data-toggle="collapse" 
            data-target="#searchReveal" 
            aria-expanded="false" 
            aria-controls="searchReveal">
                <i class="material-icons">search</i>
        </a>        
    </li><!-- ./Search -->    

    <!-- Login -->
    <li class="list-inline-item">
        <dnn:LOGIN ID="dnnLogin" 
            CssClass="LoginLink" 
            runat="server" 
            LegacyMode="false" 
            LogoffText="&lt;i class=&quot;material-icons&quot;&gt;lock&lt;/i&gt;"
            Text="&lt;i class=&quot;material-icons&quot;&gt;lock_open&lt;/i&gt;" />
    </li><!-- ./Login -->

    <%  If DotNetNuke.Security.PortalSecurity.IsInRole("Registered Users") Then %>
        <li class="list-inline-item">
            <div class="notification-holder  text-xs-center"><dnn:USER ID="dnnUserNotifications" runat="server" LegacyMode="false" CssClass="notifications" /></div>
        </li>
        <li class="list-inline-item">
            <div class="message-holder text-xs-center"><dnn:USER ID="dnnUserMessages" runat="server" LegacyMode="false" CssClass="messages" /></div>
        </li>
        <li class="list-inline-item">
            <div class="profile-holder text-xs-center"><dnn:USER ID="dnnUserProfile" runat="server" LegacyMode="false" CssClass="profile" /></div>
        </li>
    <% Else %>
        <!-- Register -->
        <li class="list-inline-item">
            <dnn:USER ID="dnnUserRegister" runat="server" LegacyMode="false" CssClass="register" Text="&lt;i class=&quot;material-icons&quot;&gt;perm_identity&lt;/i&gt;" />
        </li>
    <% End If %>

</ul>