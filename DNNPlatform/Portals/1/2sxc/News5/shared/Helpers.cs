using ToSic.Razor.Blade;
using System;
using System.Collections.Generic;
using System.Globalization;

public class Helpers: Custom.Hybrid.Code12
{
  // @* Show info to admin whether the article will publish or is already expired *@
  public dynamic AdminArticleInformation(dynamic article) {
    if(Edit.Enabled && article.ShowFrom != null && article.ShowFrom.Date > DateTime.Now.Date) {
      return Tag.Div(App.Resources.LabelShowFromPill + " " + article.ShowFrom.Date.ToString("d")).Class("alert").Class("alert-warning");
    }

    if(Edit.Enabled && article.ShowTo != null && article.ShowTo.Date <= DateTime.Now.Date) {
      return Tag.Div(App.Resources.LabelShowToPill + " " + article.ShowTo.Date.ToString("d")).Class("alert").Class("alert-danger");
    }

    return Tag.Div();
  }

  // @* Show warning that the details page needs to be changed *@
  public dynamic AdminDetailsPageConfigWarning() {
    if(App.Settings.DetailsPageShowWarning) {
      return Tag.Div(App.Resources.LabelAdminDetailPageWarning).Class("alert").Class("alert-danger");
    }

    return Tag.Div();
  }

  /**
  * Returns a safe url to a post details page
  */
  public dynamic LinkToDetailsPage(dynamic article) {
    var detailsPage = AsDynamic(App.Settings).Get("DetailsPage", convertLinks: false);
    if(!detailsPage.Contains(":")) return "";
    var detailsPageId = int.Parse(detailsPage.Split(':')[1]);

    return Link.To(pageId: detailsPageId, parameters: "details=" + article.UrlKey);
  }

  /**
  * #458 ⑰-a (ai-01 arbitration 2026-10-21): public dates render in the GREGORIAN calendar.
  * ar-SA defaults to UmAlQura and fa-IR to the Persian calendar, so a culture-formatted
  * "d" date would show Hijri/Persian years (1448/1405) on /ar and /fa. Clone the request
  * culture and force Gregorian only when needed — Gregorian is in OptionalCalendars for
  * both cultures (verified on .NET Framework 4.8, #458 c.6314358564). Cultures already
  * Gregorian keep the untouched clone, so fr/en/es/pt/ru/zh render byte-identical.
  */
  public string DateShortGregorian(DateTime d) {
    var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
    if(!(ci.Calendar is GregorianCalendar)) {
      GregorianCalendar greg = null;
      foreach(var cal in ci.OptionalCalendars) {
        var g = cal as GregorianCalendar;
        if(g == null) { continue; }
        // prefer Localized (day-month order): ar-SA lists USEnglish first -> would render 2/28/2023
        if(g.CalendarType == GregorianCalendarTypes.Localized) { greg = g; break; }
        if(greg == null) { greg = g; }
      }
      if(greg == null) { greg = new GregorianCalendar(); }
      ci.DateTimeFormat.Calendar = greg;
    }
    return d.ToString("d", ci);
  }

  /**
  * #458 C1 (ai-01 arbitration 2026-10-23): the two date bounds of the News pipeline used to be
  * ValueFilter nodes comparing a culture-RENDERED string ("[DateTime:System]" / "[DateTime:Now]")
  * with a culture-DEPENDENT DateTime.TryParse. When the render and the parse calendars disagree
  * the bound is wrong: a Persian rendering parsed as Latin fails => DateTime.MinValue => the whole
  * list empties (/fa/Actus, 22/10); a Latin rendering parsed as Persian reads 2026 as a Persian
  * year => 2647 => articles dated 2027 and even 2222 pass. Both bounds are done here with typed
  * DateTime comparisons instead: no string ever round-trips, so the outcome is the same under
  * every culture of the site (measured on .NET Framework 4.8 over the 121-combo matrix).
  * Semantics kept identical to the retired filters: keep when ShowFrom <= now AND ShowTo > now.
  */
  public bool IsVisibleNow(dynamic article) {
    var now = DateTime.Now;
    if(article.ShowFrom != null && ((DateTime)article.ShowFrom) > now) { return false; }
    if(article.ShowTo != null && ((DateTime)article.ShowTo) <= now) { return false; }
    return true;
  }

  public IEnumerable<dynamic> VisibleNews(IEnumerable<dynamic> articles) {
    var visible = new List<dynamic>();
    if(articles == null) { return visible; }
    foreach(var article in articles) {
      if(IsVisibleNow(article)) { visible.Add(article); }
    }
    return visible;
  }
}

  