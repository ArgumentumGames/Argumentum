using ToSic.Razor.Blade;

public class Links : Custom.Hybrid.Code12
{
  /// <summary>
  /// Returns a safe url to a post details page
  /// </summary>
  public dynamic LinkToDetailsPage(dynamic post) {
    return Link.To(pageId: DetailsPageId, parameters: "details=" + post.UrlKey);
  }

  /// <summary>
  /// Get / cache the page which will show the details of a post
  /// </summary>
  private int DetailsPageId
  {
    get
    {
      if (_detailsPageId != 0) return _detailsPageId;
      _detailsPageId = Text.Has(Settings.DetailsPage)
        ? Convert.ToInt((Settings.Get("DetailsPage", convertLinks: false)).Split(':')[1])
        : CmsContext.Page.Id;
      return _detailsPageId;
    }
  }
  private int _detailsPageId;
}
