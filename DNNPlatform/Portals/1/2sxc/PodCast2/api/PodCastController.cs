// Add namespaces to enable security in Oqtane & Dnn despite the differences
#if NETCOREAPP
using Microsoft.AspNetCore.Authorization; // .net core [AllowAnonymous] & [Authorize]
using Microsoft.AspNetCore.Mvc;           // .net core [HttpGet] / [HttpPost] etc.
#else
// 2sxclint:disable:no-dnn-namespaces - 2sxclint:disable:no-web-namespace
using System.Web.Http;		// this enables [HttpGet] and [AllowAnonymous]
using DotNetNuke.Web.Api;	// this is to verify the AntiForgeryToken
#endif
using System.Linq;
using System.Xml;
using System.IO;
using System;
using ToSic.Razor.Blade;

[AllowAnonymous]			// define that all commands can be accessed without a login
public class PodCastController : Custom.Hybrid.Api12
{
  // Atom XML Namespace for RSS
  public const string AtomNsCode = "atom";
  public const string AtomNamespace = "http://www.w3.org/2005/Atom";
  
  // ITunes XML Namespace for integration into Apple ecosystem
  public const string ItunesNsCode = "itunes";
  public const string ItunesNamespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";

  // CreativeCommons XML Namespace for copyright information
  public const string CreativeCommonsNsCode = "creativeCommons";
  public const string CreativeCommonsNamespace = "http://backend.userland.com/creativeCommonsRssModule";

  // Basic XML Document to start with
  public const string EmptyRssDocument = "<?xml version='1.0' encoding='utf-8'?>"
      + "<rss version='2.0' xmlns:" + AtomNsCode + "='" + AtomNamespace + "' "
      +  "xmlns:" + ItunesNsCode + "='" + ItunesNamespace + "' "
      +  "xmlns:" + CreativeCommonsNsCode + "='" + CreativeCommonsNamespace + "' "
      +"></rss>";
  public const string ErrDetailsPage = "Error: 'DetailsPage' app setting is missing. Please configure to get RSS to work.";
  
  [HttpGet]
  public dynamic Rss()
  {
    // get all posts as delived from the standard query
    var episodes = AsList(Content.Parents("Episode") as object).OrderByDescending(e => e.Date);

    // results in "2019" or "2017-2019"
    var copyrightYear = episodes.Last().Date.Year == episodes.First().Date.Year
      ? episodes.First().Date.Year
      : episodes.Last().Date.Year + "-" + episodes.First().Date.Year;

    var copyrightNotice = "Copyright © " + copyrightYear + " " + Content.Owner.FullName 
      + (Content.License.Link != "copyright/" ?  " (" + Content.License.Link + ")" : "");

    // 1. Prepare
    // 1.1 Figure out what page will show post details based on settings
    // If the settings are configured, it's something like "page:27"
    var detailsPageId = Text.Has(Settings.DetailsPage)
      ? int.Parse((Settings.Get("DetailsPage", convertLinks: false)).Split(':')[1])
      : int.Parse(CmsContext.Page.Parameters["PageId"]); // when 'DetailsPage' app setting is missing.

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
    AddTag(channel, "generator", "2sxc PodCast App");
    AddTag(channel, "title", Content.Title);
    AddTag(channel, "link", Link.To(pageId: detailsPageId, type: "full") ?? linkErrMessage);
    AddTag(channel, "description", Tags.Strip(Content.Description));
    AddTag(channel, "language", Tags.Strip(Content.Language));
    AddTag(channel, "copyright", copyrightNotice);
    AddTag(channel, "managingEditor", Content.Owner.Email + " (" + Content.Owner.FullName + ")");
    var image = AddTag(channel, "image");
    AddTag(image, "title", Content.Title);
    AddTag(image, "url", Link.Image(Content.Image, type: "full"));
    AddTag(image, "link", Link.To(type: "full"));

    // 3.2 Create <creativeCommons:license> tag and add Creative Commons license if isn't copyright
    if(Content.License.Link != "copyright/") 
      AddNamespaceTag(channel, CreativeCommonsNsCode, "license", CreativeCommonsNamespace, Content.License.Link);
    
    // 3.3 Add required Itunes values to channel
    AddChannelItunes(channel);

    // 3.4 Add Atom tag to channel
    AddChannelAtom(channel);
    
    // 3.5 Add all the episodes from the query to this channel
    // get protocol and host to complete the urls of the episodes and the one of the image
    var fullLink = Link.To(type: "full");
    var websiteRoot = fullLink.Substring(0, fullLink.IndexOf("/", fullLink.IndexOf("//") + 2));
    foreach(var episode in episodes) AddEpisode(websiteRoot, channel, episode);

    return File(download: false, fileDownloadName: "rss.xml", contents: rssDoc);
  }














  #region functions that add xml sections 

  // Adds required <itunes:xy> tags to channel
  private void AddChannelItunes(XmlElement channel) {
    // Get all posts as delived from the standard query
    var episodes = AsList(Content.Parents("Episode") as object).OrderByDescending(e => e.Date);
    var imageUrl = Link.Image(Content.Image, type: "full");

    AddNamespaceTag(channel, ItunesNsCode, "summary", ItunesNamespace, Tags.Strip(Content.Description));
    AddNamespaceTag(channel, ItunesNsCode, "author", ItunesNamespace, Content.Owner.FullName);
    AddNamespaceTag(channel, ItunesNsCode, "explicit", ItunesNamespace, episodes.Any(e => e.Explicit == true) ? "yes" : "no");
    var itunesImage = AddNamespaceTag(channel, ItunesNsCode, "image", ItunesNamespace);
    // Image size of 3000 x 3000 is required by itunes
    AddAttribute(itunesImage, "href", Link.Image(url: imageUrl, width: 3000, height: 3000));

    var itunesOwner = AddNamespaceTag(channel, ItunesNsCode, "owner", ItunesNamespace);
    AddNamespaceTag(itunesOwner, ItunesNsCode, "name", ItunesNamespace, Content.Owner.FullName);
    AddNamespaceTag(itunesOwner, ItunesNsCode, "email", ItunesNamespace, Content.Owner.Email);

    var itunesCategory = AddNamespaceTag(channel, ItunesNsCode, "category", ItunesNamespace);
    AddAttribute(itunesCategory, "text", Content.Category.MainCategory);

    // Add subcategory if needed
    if(Text.Has(Content.Category.SubCategory)) {
      var itunesSubCategory = AddNamespaceTag(itunesCategory, ItunesNsCode, "category", ItunesNamespace);
      AddAttribute(itunesSubCategory, "text", Content.Category.SubCategory);
    }
  }

  // Adds <atom> tag to channel
  private void AddChannelAtom(XmlElement channel) {
    var atomLink = AddNamespaceTag(channel, AtomNsCode, "link", AtomNamespace);
    AddAttribute(atomLink, "href", Link.To(api: "api/PodCast/Rss", parameters: CmsContext.Page.Parameters, type: "full"));
    AddAttribute(atomLink, "rel", "self");
    AddAttribute(atomLink, "type", "application/rss+xml");
  }

  // Adds Episode to channel
  private void AddEpisode(string websiteRoot, XmlElement channel, dynamic episode) {
    var publicationDate = (episode.Date ?? DateTime.Now).ToString("R");
    var authorItems = AsList(episode.Author as object); 
    var authorFullName = authorItems.Any() ? authorItems.First().FullName : "";
    var authorEmail = authorItems.Any() ? authorItems.First().Email : "";
    
    var itemNode = AddTag(channel, "item");
    AddTag(itemNode, "title", episode.Title);
    AddTag(itemNode, "description", Tags.Strip(episode.Description));

    var itemGuid = AddTag(itemNode, "guid", episode.EntityGuid.ToString());
    AddAttribute(itemGuid, "isPermaLink", "false");
    AddTag(itemNode, "category", Text.Has(Content.Category.SubCategory) ? Content.Category.SubCategory : Content.Category.MainCategory);
    AddTag(itemNode, "author", authorEmail + " (" + authorFullName + ")");
    AddTag(itemNode, "pubDate", publicationDate);

    var enclosure = AddTag(itemNode, "enclosure");
    AddAttribute(enclosure, "url", websiteRoot + episode.Audio);
    AddAttribute(enclosure, "type", "audio/mpeg");
    AddAttribute(enclosure, "length", "1024");
    AddTag(itemNode, "link", Link.To());

    // Adds required <itunes:xy> tags
    AddItemItunes(itemNode, episode, authorFullName);
  }

  // Adds required <itunes:xy> tags to itemNode
  private void AddItemItunes(XmlElement itemNode, dynamic episode, string authorFullName) {
    var duration = TimeSpan.FromMinutes(Convert.ToFloat(episode.Duration ?? 0)).ToString("hh\\:mm") + ":00";
    AddNamespaceTag(itemNode, ItunesNsCode, "subtitle", ItunesNamespace, Text.Crop(Tags.Strip(episode.Description), 255));
    AddNamespaceTag(itemNode, ItunesNsCode, "summary", ItunesNamespace, Tags.Strip(episode.Description));
    AddNamespaceTag(itemNode, ItunesNsCode, "author", ItunesNamespace, authorFullName);
    AddNamespaceTag(itemNode, ItunesNsCode, "duration", ItunesNamespace, duration);
    AddNamespaceTag(itemNode, ItunesNsCode, "explicit", ItunesNamespace, episode.Explicit ? "yes" : "no");
  }

  #endregion
  
  
  #region helper functions for creating new XML elements and attributes 

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

  private XmlElement AddNamespaceTag(XmlElement parent, string name, string tagNs, string link, string value = null) {
    var node = parent.OwnerDocument.CreateElement(name, tagNs, link);
    if(value != null) node.InnerText = value;
    parent.AppendChild(node);
    return node;
  }

  #endregion
}
