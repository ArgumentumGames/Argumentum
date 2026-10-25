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
    // statically typed locals: the bound reader is NOT a dynamic call, so a wrong shape
    // is handled by NewsDateBound instead of failing the binder at render time
    object rawFrom = article.ShowFrom;
    object rawTo = article.ShowTo;
    var showFrom = NewsDateBound(rawFrom);
    var showTo = NewsDateBound(rawTo);
    if(showFrom.HasValue && showFrom.Value > now) { return false; }
    if(showTo.HasValue && showTo.Value <= now) { return false; }
    return true;
  }

  /**
  * #458 W13 (web2 contre-revue of C1, 2026-10-23): reads one date bound and can never throw.
  * The bounds used to be read by an unconditional cast of a `dynamic`; if a field ever arrives
  * as a string — the shape the retired string filter tolerated — that cast raises
  * RuntimeBinderException inside the view and takes the WHOLE list page down, which is a worse
  * outcome than a wrong bound.
  * Measured 2026-10-25 (read only, TsDynDataAttribute): both News content types (140 and 325)
  * declare ShowFrom and ShowTo as DateTime, so the cast cannot fail TODAY. This is defence in
  * depth for the day the field type changes in the 2sxc UI, or the item is not a typed entity.
  * Accepted shapes: absent => no bound; DateTime; DateTimeOffset; string, parsed with the
  * INVARIANT culture because a culture-dependent parse is precisely what C1 removed from the
  * site (a Persian rendering parsed as Latin used to empty /fa/Actus).
  * An unreadable value is treated as ABSENT, i.e. the article stays visible: a parse failure
  * must never turn into a hiding decision — that is the exact failure family C1 removes.
  */
  private static DateTime? NewsDateBound(object value) {
    if(value == null) { return null; }
    if(value is DateTime) { return (DateTime)value; }
    if(value is DateTimeOffset) { return ((DateTimeOffset)value).DateTime; }
    var text = value as string;
    if(text != null) {
      DateTime parsed;
      if(DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed)) { return parsed; }
    }
    return null;
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

  