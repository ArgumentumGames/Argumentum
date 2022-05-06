// Add namespaces to enable security in Oqtane & Dnn despite the differences
#if NETCOREAPP
using Microsoft.AspNetCore.Authorization; // .net core [AllowAnonymous] & [Authorize]
using Microsoft.AspNetCore.Mvc;           // .net core [HttpGet] / [HttpPost] etc.
#else
// 2sxclint:disable:no-dnn-namespaces - 2sxclint:disable:no-web-namespace
using System.Web.Http;		// this enables [HttpGet] and [AllowAnonymous]
using DotNetNuke.Web.Api;	// this is to verify the AntiForgeryToken
#endif
using System.Xml;
using System.IO;
using ToSic.Razor.Blade;

[AllowAnonymous]			// define that all commands can be accessed without a login
public class BlogController : Custom.Hybrid.Api12
{
  public const string AtomNsCode = "atom";
  public const string AtomNamespace = "http://www.w3.org/2005/Atom";
  public const string EmptyRssDocument = "<?xml version='1.0' encoding='utf-8'?>"
      + "<rss version='2.0' xmlns:" + AtomNsCode + "='" + AtomNamespace + "'></rss>";
  public const string ErrDetailsPage = "Error: 'DetailsPage' app setting is missing. Please configure to get RSS to work.";

  [HttpGet]
  public dynamic Rss()
  {
    // 1. Prepare
    // 1.1 Figure out what page will show post details based on settings
    // If the settings are configured, it's something like "page:27"
    var detailsPageId = Text.Has(Settings.DetailsPage)
      ? int.Parse((Settings.Get("DetailsPage", convertLinks: false)).Split(':')[1])
      : 0; // when 'DetailsPage' app setting is missing.

    // 1.2 This will be null or a message. To be used instead of links
    var linkErrMessage = (detailsPageId == 0) ? ErrDetailsPage : null;

    // 2. Build main XML document
    // 2.1 Create the XmlDocument and get root node
    var rssDoc = new XmlDocument();
    rssDoc.PreserveWhitespace = true;
    rssDoc.LoadXml(EmptyRssDocument);
    var root = rssDoc.DocumentElement;

    // 2.1 Add a warning to the XML in case we don't know what page to use for links
    // we use a custom xml namespace "warning" to not break the RSS standard
    if(detailsPageId == 0) {
      var warningTag = AddNamespaceTag(root, "warning", "warning", "http://warning");
      warningTag.InnerText = "The links for the details cannot work yet, because the App isn't fully configured. You must set a details page.";
    }

    // 3. Create Channel
    // 3.1 Create <channel> node and set important values
    var channel = AddTag(root, "channel");
    AddTag(channel, "title", Resources.BlogTitle);
    AddTag(channel, "link", linkErrMessage ?? Link.To(pageId: detailsPageId, type: "full"));
    AddTag(channel, "description", Resources.RssDescription);

    // 3.2 Create the <atom> tag with all the attributes. It needs to have the namespace "atom" for valid RSS
    var atom = AddNamespaceTag(channel, AtomNsCode, "link", AtomNamespace);
    AddAttribute(atom, "rel", "self");
    AddAttribute(atom, "type", "application/rss+xml");
    AddAttribute(atom, "href", Link.To(api: "api/Blog/Rss", type: "full"));

    // 3.3 Add all the posts from the query to this channel
    foreach(var post in AsList(App.Query["BlogPosts"]["AllPosts"])) {

      var itemNode = AddTag(channel, "item");
      AddTag(itemNode, "title", post.EntityTitle);
      AddTag(itemNode, "link", linkErrMessage ?? Link.To(pageId: detailsPageId, parameters: "details=" + post.UrlKey, type: "full"));
      AddTag(itemNode, "description", Tags.Strip(post.Teaser)); // Tags.Strip makes sure no HTML makes it into the teaser
      var guidNode = AddTag(itemNode, "guid", post.EntityGuid.ToString());
      AddAttribute(guidNode, "isPermaLink", "false");
      AddTag(itemNode, "pubDate", post.PublicationMoment.ToString("R"));
    }

    return File(download: false, fileDownloadName: "rss.xml", contents: rssDoc);
  }


  private XmlElement AddTag(XmlElement parent, string name, string value = null) {
    var node = parent.OwnerDocument.CreateElement(name);
    node.InnerText = value;
    parent.AppendChild(node);
    return node;
  }

  private XmlAttribute AddAttribute(XmlElement parent, string name, string value) {
    var node = parent.OwnerDocument.CreateAttribute(name);
    node.Value = value;
    return parent.Attributes.Append(node);
  }

  
  private XmlElement AddNamespaceTag(XmlElement parent, string name, string tagNs, string link) {
    var node = parent.OwnerDocument.CreateElement(name, tagNs, link);
    parent.AppendChild(node);
    return node;
  }

}
