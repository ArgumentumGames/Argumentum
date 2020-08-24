using System.Web.Http;
using ToSic.SexyContent.WebApi;
using DotNetNuke.Web.Api;

[AllowAnonymous]
[ValidateAntiForgeryToken]
public class DemoController : SxcApiController
{
	[HttpGet]
	public string Hello()
	{
		return "Hello from the staging controller - we're still developing this, so you may see errors. Once it's tested, we'll copy this to /live/api/ so the public users will see this too.";
	}
}