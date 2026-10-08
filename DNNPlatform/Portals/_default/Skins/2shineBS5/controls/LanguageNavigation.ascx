<%@ Control Language="C#" AutoEventWireup="true" %>

<script runat="server">
    public string Languages { get; set; }

    protected string[] LanguagesArray
    {
        get { return Languages.Split(','); }
    }

    DotNetNuke.UI.Skins.Controls.LanguageTokenReplace LanguageTokenReplace = new DotNetNuke.UI.Skins.Controls.LanguageTokenReplace();

    string GetCurrentTabUrlForLanguage(string language)
    {
        LanguageTokenReplace.Language = language;
        return LanguageTokenReplace.ReplaceEnvironmentTokens("[URL]");
    }

    bool ShowLanguageSwitchForLanguage(string language)
    {
        // Shared-pages model (portal Content Localization disabled): pages carry no per-culture tab,
        // so the switch shows whenever the language is enabled for the portal and the current page is viewable.
        var locale = new LocaleController().GetLocale(PortalSettings.Current.PortalId, language);
        if (locale == null)
            return false;

        var permissionProvider = new DotNetNuke.Security.Permissions.PermissionProvider();
        var tab = PortalSettings.Current.ActiveTab;
        return permissionProvider.HasTabPermission(permissionProvider.GetTabPermissions(tab.TabID, PortalSettings.Current.PortalId), "VIEW");
    }

</script>

<ul class="to-shine-page-language">
    <% foreach (var language in LanguagesArray) { %>
        <% var lang = language.Split(':'); %>
        <% if (ShowLanguageSwitchForLanguage(lang[0]))
            { %>
            <li class="<%= "nav-" + lang[0].ToLower() %><%= lang[0].ToLower() == CultureInfo.CurrentCulture.ToString().ToLower() ? " active" : "" %>">
                <a href="<%= GetCurrentTabUrlForLanguage(lang[0]) %>"><%= lang[1] %></a>
            </li>
        <% } %>
    <% } %>
</ul>
