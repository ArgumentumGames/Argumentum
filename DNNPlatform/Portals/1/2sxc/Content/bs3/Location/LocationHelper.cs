using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Globalization;
using ToSic.Razor.Blade;

public class LocationHelper: Custom.Hybrid.Code12
{
  // check a link, prepare target window, icon etc. based on various settings
  public MapInfo MapInfos(dynamic content) {
    // this will contain the result
    var mInfo = new MapInfo();

    // Language is used for the map-link
    var language = CmsContext.Culture.CurrentCode.Split(new[] { '-' })[0];

    // GPS is a JSON field, so we must use AsDynamic to access the properties
    var gps = AsDynamic(content.GPS);
    var gpsLong = gps.Longitude ?? 0; // use 0 if not defined
    var gpsLat = gps.Latitude ?? 0;	  // use 0 if not defined

    // this link will be used to open the Google-Directions in a new window
    var directionurl = gpsLong > 0
      // if we have coordinates, use them
      ? "https://www.google.com/maps/dir/" + gpsLat.ToString("R") + "," + gpsLong.ToString("R")
      // otherwise use the address
      : "https://maps.google.com/maps?daddr="
        + (content.Street + " " + content.ZipCode + " " + content.City + " " + content.Country)
          .Replace(" ", "+")
        + "&amp;saddr=&amp;f=d&amp;hl=" + language + "&amp;ie=UTF8&amp;z=16";

    // add properties
    mInfo.GpsLong = gpsLong;
    mInfo.GpsLat = gpsLat;
    mInfo.DirectionUrl = directionurl;

    return mInfo;
  }
  
}


public class MapInfo
{
  public double GpsLong;
  public double GpsLat;
  public string DirectionUrl;
}