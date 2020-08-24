using System.Linq;        // this enables .Select(x => ...)
using System.Web.Http;    // this enables [HttpGet] and [AllowAnonymous]
using DotNetNuke.Web.Api; // this is to verify the AntiForgeryToken
using Dynlist = System.Collections.Generic.IEnumerable<dynamic>;

[AllowAnonymous]            // define that all commands can be accessed without a login
[ValidateAntiForgeryToken]  // protects the API from users not on your site (CSRF protection)
// Inherit from ToSic...ApiController to get features like App, Data or Dnn - see https://r.2sxc.org/CustomWebApi
public class AuthorsQueryController : ToSic.Sxc.Dnn.ApiController
{
  [HttpGet]
  public dynamic Get(int authorId)
  {
    var query = App.Query["AuthorsWithBooks"];
    query.Params("AuthorId", authorId.ToString());
    var a = AsDynamic(query["Current"].First());

    return new {
        Id = a.EntityId,
        a.FirstName,
        a.LastName,
        Picture = a.Mugshot,
        Books = AsList(query["CurrentBooks"])
          .Select(b => new {
            Id = b.EntityId,
            b.Title
          })
      };
  }

}
