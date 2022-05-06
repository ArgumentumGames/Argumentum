#if NETCOREAPP // Oqtane
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
#else // DNN
// 2sxclint:disable:no-web-namespace
using System.Web;
using System.Web.Http;
using FromQueryAttribute = System.Web.Http.FromUriAttribute; // Sync-names to match .net core naming
#endif
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using ToSic.Razor.Blade;

public class VCardController : Custom.Hybrid.Api12
{
  [HttpGet]
  [AllowAnonymous]
  public dynamic Get([FromQuery] string id)
  {
    var personEntity = App.Query["PersonUrlKey"].List.FirstOrDefault();
    if (personEntity == null) throw new Exception("Can't find person with id " + id);
    var person = AsDynamic(personEntity);

    var card = new VCard
    {
        FirstName = person.FirstName,
        LastName = person.LastName,
        Organization = Settings.CompanyName,
        JobTitle = person.Function,
        StreetAddress = person.Street,
        Zip = person.ZipCode,
        City = person.City,
        CountryName = person.Country,
        Phone = person.Phone,
        PhoneCompany = Settings.CompanyPhone,
        // Mobile = person.,// queryString["mobile"],
        Email = person.EMail,
        Url = Settings.CompanyUrl,
    };

    if (Text.Has(person.Image)) {
      var photoPath = Link.Image(person.Image, width: 250, height: 250, quality: 80, format: "jpg", type: "full");
      card.PhotoBase64 = CreateThumbnail(photoPath);
    }

    var mimeType = "text/vcard";

    var fileName = card.FirstName + " " + card.LastName;
    if (string.IsNullOrWhiteSpace(fileName))
        fileName = card.Organization;
    if (string.IsNullOrWhiteSpace(fileName))
        fileName = "contact";
    fileName += ".vcf";


    var cardString = card.ToString();
    var inputEncoding = Encoding.Default;
    var outputEncoding = Encoding.GetEncoding(28591); // ISO-8859-1
    var cardBytes = inputEncoding.GetBytes(cardString);
    var outputBytes = Encoding.Convert(inputEncoding, outputEncoding, cardBytes);

    return File(download: true, contents: outputBytes, contentType: mimeType, fileDownloadName: Path.GetFileName(fileName));
  }


  /// <summary>
  /// web request to image, so we can use image resizer for image preparation
  /// </summary>
  internal string CreateThumbnail(string absoluteUrl)
  {
    // try-catch important as it can often cause problems
    try {
      var byteArray = new HttpClient().GetByteArrayAsync(absoluteUrl).Result;
      return System.Convert.ToBase64String(byteArray);
    }
    catch (Exception e)
    {
      Log.Add("Error getting image file - path was:" + absoluteUrl);
      Log.Exception(e);
      return null;
    }
  }

  /// <summary>
  /// Helper class to construct a vCard
  /// </summary>
  internal class VCard
  {
    private const string AddressType = "WORK";
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Organization { get; set; }
    public string JobTitle { get; set; }
    public string StreetAddress { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string CountryName { get; set; }
    public string Phone { get; set; }
    public string PhoneCompany { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string Url { get; set; }
    public string PhotoBase64 { get; set; } // Prepared photo as base64 encoding, jpg

    public override string ToString()
    {
      var charSet = "CHARSET=iso-8859-1:";
      var builder = new StringBuilder();
      builder.AppendLine("BEGIN:VCARD");
      builder.AppendLine("VERSION:2.1");
      // Name
      builder.AppendLine("N;" + charSet + LastName + ";" + FirstName);
      // Full name
      if (Text.Has(FirstName) || Text.Has(FirstName))
          builder.AppendLine("FN;" + charSet + FirstName + " " + LastName);
      else
          builder.AppendLine("FN;" + charSet + Organization);
      // Address
      builder.Append("ADR;" + AddressType + ";PREF;" + charSet + ";;");
      builder.Append(StreetAddress + ";");
      builder.Append(City + ";;");
      builder.Append(Zip + ";");
      builder.AppendLine(CountryName);
      // Other data
      builder.AppendLine("ORG;" + charSet + Organization);
      builder.AppendLine("TITLE;" + charSet + JobTitle);
      builder.AppendLine("TEL;" + AddressType + ";VOICE;" + charSet + Phone);
      if (!string.IsNullOrWhiteSpace(PhoneCompany))
        builder.AppendLine("X-MS-TEL;VOICE;COMPANY:" + PhoneCompany);
      builder.AppendLine("TEL;CELL;VOICE:" + Mobile);
      builder.AppendLine("URL;" + AddressType + ":" + Url);
      builder.AppendLine("EMAIL;PREF;INTERNET:" + Email);

      // Add image
      if(Text.Has(PhotoBase64)) {
        builder.AppendLine("PHOTO;ENCODING=BASE64;TYPE=JPEG:");
        builder.AppendLine(PhotoBase64);
        builder.AppendLine(string.Empty);
      }

      builder.AppendLine("END:VCARD");

      return builder.ToString();
    }
  }

}
