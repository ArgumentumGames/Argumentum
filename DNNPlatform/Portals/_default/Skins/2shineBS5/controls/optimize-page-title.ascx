<%@ Control language="C#" Inherits="System.Web.UI.UserControl" %>

<script runat="server">
	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		NormalizePageTitle();
	}
	
	// SEO page title script - v 2.0
    private void NormalizePageTitle()
    {
        const string separator = " | ";
        const int depth = 5;
        const bool includePortalName = true;
        
		if(String.IsNullOrEmpty(PortalSettings.Current.ActiveTab.Title))
			Page.Title = String.Join(separator, PortalSettings.Current.ActiveTab.BreadCrumbs.Cast<DotNetNuke.Entities.Tabs.TabInfo>()
				.Reverse().Take(depth).Select(LocalizedTabName).Distinct().ToArray()) +
				(includePortalName ? separator + PortalSettings.Current.PortalName : "");
    }

	// #458: TabName is a single French value (Tabs has no per-culture title column), so an interior
	// page shows a French word in the browser tab on the 7 non-fr cultures. The localized label
	// lives next to the menu keys (Menu.Tab<Id>.Text). Same lookup as the nav templates (#1962):
	// name the culture file explicitly and never fall back to the base file — DNN resolves the
	// base file for any culture without a PortalLocalization row (en-US here).
	private static string LocalizedTabName(DotNetNuke.Entities.Tabs.TabInfo tab)
	{
		try
		{
			var key = "Menu.Tab" + tab.TabID + ".Text";
			var root = "~/Portals/_default/Skins/2shineBS5/App_LocalResources/2shine";
			var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
			var s = String.IsNullOrEmpty(culture)
				? DotNetNuke.Services.Localization.Localization.GetString(key, root + ".resx")
				: DotNetNuke.Services.Localization.Localization.GetString(key, root + "." + culture + ".resx");
			return (String.IsNullOrWhiteSpace(s) || s == key) ? tab.TabName : s;
		}
		catch { return tab.TabName; }
	}
</script>

