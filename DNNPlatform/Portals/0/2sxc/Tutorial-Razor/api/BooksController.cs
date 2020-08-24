using System.Linq;        // this enables .Select(x => ...)
using System.Web.Http;    // this enables [HttpGet] and [AllowAnonymous]
using DotNetNuke.Web.Api; // this is to verify the AntiForgeryToken
using ToSic.Sxc.Conversion; // for the auto-conversion example below
// using Dynlist = System.Collections.Generic.IEnumerable<dynamic>;

[AllowAnonymous]            // define that all commands can be accessed without a login
[ValidateAntiForgeryToken]  // protects the API from users not on your site (CSRF protection)
// Inherit from ToSic...ApiController to get features like App, Data or Dnn - see https://r.2sxc.org/CustomWebApi
public class BooksController : ToSic.Sxc.Dnn.ApiController
{
  [HttpGet]
  public dynamic Persons()
  {
    // we could do: return App.Data["Persons"];
    // but the raw data is difficult to use, so we'll improve it

    // instead we'll create nice objects for our use
    return AsList(App.Data["Persons"])
      .Select(p => new {
        Id = p.EntityId,
        p.FirstName,
        p.LastName,
        Picture = p.Mugshot,
      });
  }

  [HttpGet]
  public dynamic PersonsAuto()
  {
    // this uses ToSic.Sxc.Conversion.DataToDictionary
    return new DataToDictionary(Edit.Enabled)
      .Convert(App.Data["Persons"]);
  }

  [HttpGet]
  public dynamic Books()
  {
    return AsList(App.Data["Books"])
      .Select(b => new {
        Id = b.EntityId,
        b.Title,
        b.Cover,
        Awards = AsList(b.Awards as object)
          .Select(a => a.EntityTitle)
      });
  }
}
