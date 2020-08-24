// In case you need to know more about WebApis, 
// visit the tutorials on https://2sxc.org/dnn-tutorials/
using DotNetNuke.Web.Api;
using ToSic.SexyContent.WebApi;
using System;
using System.Web.Http;

[AllowAnonymous]
[ValidateAntiForgeryToken]
public class SimpleController : SxcApiController
{
	[HttpGet]
	public string Hello(string name = null)
	{
		return name == null ? "Hello from the controller in web api" : "Hello " + name;
	}


	[HttpGet]
	public int[] Numbers()
	{
		return new int[] { 7, 12, 2972 };
	}


	[HttpGet]
	public dynamic Something(string name) 
	{
		return new {
			Name = name,
			Birthday = new DateTime(2012, 05, 12)
		};

	}

}
