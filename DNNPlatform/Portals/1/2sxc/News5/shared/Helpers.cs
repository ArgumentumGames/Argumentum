using ToSic.Razor.Blade;
using System;
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
        if(cal is GregorianCalendar) { greg = (GregorianCalendar)cal; break; }
      }
      if(greg == null) { greg = new GregorianCalendar(); }
      ci.DateTimeFormat.Calendar = greg;
    }
    return d.ToString("d", ci);
  }
}

  