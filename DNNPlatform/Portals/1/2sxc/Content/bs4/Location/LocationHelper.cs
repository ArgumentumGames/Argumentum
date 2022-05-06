using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Globalization;
using ToSic.Razor.Blade;

public class LocationHelper: Custom.Hybrid.Code12
{
  // check a link, prepare target window, icon etc. based on various settings
  public dynamic MapInfos(dynamic content) {

    // Language is used for the map-link
    var language = CmsContext.Culture.CurrentCode.Split(new[] { '-' })[0];

    // GPS is a JSON field, so we must use AsDynamic to access the properties
    var gps = AsDynamic(content.GPS);
    var gpsLong = Convert.ToDouble(gps.Longitude, fallback: 0);
    var gpsLat = Convert.ToDouble(gps.Latitude, fallback: 0);

    // this link will be used to open the Google-Directions in a new window
    var directionurl = gpsLong > 0
      // if we have coordinates, use them
      ? "https://www.google.com/maps/dir/" + Convert.ForCode(gpsLat) + "," + Convert.ForCode(gpsLong)
      // otherwise use the address
      : "https://maps.google.com/maps?daddr="
        + (content.Street + " " + content.ZipCode + " " + content.City + " " + content.Country)
          .Replace(" ", "+")
        + "&amp;saddr=&amp;f=d&amp;hl=" + language + "&amp;ie=UTF8&amp;z=16";

    return AsDynamic(new {
      GpsLong = gpsLong,
      GpsLat = gpsLat,
      DirectionUrl = directionurl,
    });
  }
  
}