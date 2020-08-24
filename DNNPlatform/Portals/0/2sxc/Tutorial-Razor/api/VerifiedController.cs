using System.Web.Http;		// this enables [HttpGet] and [AllowAnonymous]
using DotNetNuke.Web.Api;	// this is to verify the AntiForgeryToken

[AllowAnonymous]			// define that all commands can be accessed without a login
[ValidateAntiForgeryToken]	// protects the API from users not on your site (CSRF protection)
// Inherit from ToSic...ApiController to get features like App, Data or Dnn - see https://r.2sxc.org/CustomWebApi
public class VerifiedController : ToSic.Sxc.Dnn.ApiController
{

	[HttpGet]				// [HttpGet] says we're listening to GET requests
	public string Hello()
	{
		return "Hello from the controller with ValidateAntiForgeryToken in /api";
	}

}
