using System.Web.Http;
using ToSic.SexyContent.WebApi;
using DotNetNuke.Web.Api;

[AllowAnonymous]
[ValidateAntiForgeryToken]
public class SharedCodeController : SxcApiController
{
	[HttpGet]
	[AllowAnonymous]
	public string Hello()
	{
		var shared = CreateInstance("../FunctionsBasic.cs");
		return shared.SayHello();
	}
}