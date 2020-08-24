using System.Web.Http;
using ToSic.SexyContent.WebApi;
using DotNetNuke.Web.Api;

[AllowAnonymous]
[ValidateAntiForgeryToken]
public class DemoController : SxcApiController
{
	[HttpGet]
	[AllowAnonymous]
	public string Hello()
	{
		return "Hello from the live controller";
	}
}